using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Krypton.Toolkit;
using FileHandeling;
using ImageAndMovieFileExtentions;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        #region Rotate - Rotate one media file
        private bool Rotate(FileEntry fileEntry, int rotateDegrees, ref string error)
        {
            bool coverted = false;
            try
            {
                Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                if (metadata == null)
                {
                    error += (error == "" ? "" : "\r\n") + "Failed to rotated: " + fileEntry.FileFullPath + "\r\nMetadata was missing, need load metadata first.";
                }
                else if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsImageFormat(fileEntry.FileFullPath))
                {
                    try
                    {
                        ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.RoateImage(fileEntry.FileFullPath, rotateDegrees);
                        coverted = true;
                    }
                    catch (Exception ex)
                    {
                        coverted = false;
                        error += (error == "" ? "" : "\r\n") + "Failed to rotated: " + fileEntry.FileFullPath + "\r\nError message: " + ex.Message;
                    }
                }
                else if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(fileEntry.FileFullPath))
                {

                    string outputFolder = Path.GetDirectoryName(fileEntry.FileFullPath);
                    string tempOutputfile = Path.Combine(outputFolder, "temp_" + Guid.NewGuid().ToString() + Path.GetExtension(fileEntry.FileFullPath));

                    try
                    {
                        timerSaveProgessRemoveProgress.Start();
                        coverted =  ImageAndMovieFileExtentionsUtility.RotateVideo(fileEntry.FileFullPath, tempOutputfile, rotateDegrees);
                        coverted = true;
                    }
                    catch (Exception ex)
                    {
                        coverted = false;
                        error += (error == "" ? "" : "\r\n") + "Failed to rotated: " + fileEntry.FileFullPath + "\r\nError message: " + ex.Message;
                    }

                    try
                    {

                        if (coverted && new System.IO.FileInfo(tempOutputfile).Length > 0)
                        {
                            FileHandler.Delete(fileEntry.FileFullPath, Properties.Settings.Default.MoveToRecycleBin);
                            FileHandler.Move(tempOutputfile, fileEntry.FileFullPath);
                        }
                        else File.Delete(tempOutputfile);
                    }
                    catch (Exception ex)
                    {
                        error += (error == "" ? "" : "\r\n") + ex.Message;
                    }
                }

                if (coverted) //Replicate metadata and save back to media file
                {
                    Metadata metadataOriginal = new Metadata(metadata);

                    if (metadata != null)
                    {
                        Metadata metadataToSave = new Metadata(metadata);
                        metadataToSave.PersonalRegionRotate(rotateDegrees);
                        AddQueueSaveUsingExiftoolMetadataUpdatedByUserLock(metadataToSave, metadataOriginal);
                    }
                }
            } 
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            return coverted;
        }
        #endregion 

        #region Rotate - Files init
        List<FileEntry> fileEntriesRotateMedia = new List<FileEntry>();
        private static readonly Object fileEntriesRotateMediaLock = new Object();
        private bool isThreadRunning = false;

        private int GetFileEntriesRotateMediaCountLock()
        {
            try
            {
                lock (fileEntriesRotateMediaLock) return fileEntriesRotateMedia.Count();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                return 0;
            }
        }

        private int GetFileEntriesRotateMediaCountDirty()
        {
            try
            {
                return fileEntriesRotateMedia.Count();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                return 0;
            }
        }

        private void RotateInit(ImageListView imageListView, int rotateDegrees)
        {
            try
            {
                string filesMissingMetadata = "";
                lock (fileEntriesRotateMediaLock)
                {
                    foreach (ImageListViewItem item in imageListView1.SelectedItems)
                    {
                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(item.FileFullPath, item.DateModified, MetadataBrokerType.ExifTool));
                        if (metadata == null) filesMissingMetadata += (filesMissingMetadata == "" ? "" : "\r\n") + item.FileFullPath;
                    }

                }

                if (!string.IsNullOrWhiteSpace(filesMissingMetadata))
                {
                    KryptonMessageBox.Show("Need wait until metadata is read from media files befor rotating. Otherwise metadata from original media file will be lost.",
                        "Can't start rotate of media files yet.", (KryptonMessageBoxButtons)MessageBoxButtons.YesNo, KryptonMessageBoxIcon.Exclamation, showCtrlCopy: true);

                }
                else if (MessageBox.Show(
                    "You are in progress to start rotate media file(s)\r\n" +
                    "The rotating process will happend in the background, and may take a while...\r\n\r\n" +
                    "PS... Rotating will overwrite original media files.\r\n\r\n" +
                    "Are you sure you want to continue?",
                    "Rotate?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {


                    lock (fileEntriesRotateMediaLock)
                    {
                        foreach (ImageListViewItem item in imageListView1.SelectedItems)
                        {
                            fileEntriesRotateMedia.Add(new FileEntry(item.FileFullPath, item.DateModified));
                        }
                    }

                    if (!isThreadRunning)
                    {
                        try
                        {
                            isThreadRunning = true;
                            string errors = "";

                            Thread thread = new Thread(() =>
                            {
                                bool listEmpty = true;
                                do
                                {
                                    FileEntry fileEntriesRotateMediaInProgress = null;
                                    lock (fileEntriesRotateMediaLock)
                                    {
                                        if (fileEntriesRotateMedia.Count() > 0) fileEntriesRotateMediaInProgress = new FileEntry(fileEntriesRotateMedia[0]);
                                    }

                                    Rotate(fileEntriesRotateMediaInProgress, rotateDegrees, ref errors);


                                    lock (fileEntriesRotateMediaLock)
                                    {
                                        if (fileEntriesRotateMedia.Count() > 0) fileEntriesRotateMedia.RemoveAt(0);

                                    }

                                    if (GetFileEntriesRotateMediaCountLock() > 0) listEmpty = false;
                                    else listEmpty = true;


                                } while (!listEmpty);

                                isThreadRunning = false;
                            });
                            thread.Start();

                            if (errors != "")
                            {
                                FormMessageBox formMessageBox = new FormMessageBox("Warning", errors);
                                formMessageBox.ShowDialog();
                            }
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Rotate - mmpeg Progress - Process bar
        private Stopwatch stopwatchRemoveSaveProgressbar = new Stopwatch();
        private TimeSpan durationMpegVideoConvertion = new TimeSpan();

        private void timerSaveProgessRemoveProgress_Tick(object sender, EventArgs e)
        {
            try
            {
                if (stopwatchRemoveSaveProgressbar.Elapsed.TotalMilliseconds > 1000)
                {
                    ProgressbarSaveProgress(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

    }
}
