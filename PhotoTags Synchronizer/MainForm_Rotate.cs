using ApplicationAssociations;
using DataGridViewGeneric;
using Exiftool;
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
using static Manina.Windows.Forms.ImageListView;
using FileHandeling;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region Rotate

        #region Rotate - Rotate one media file
        private bool Rotate(FileEntry fileEntry, int rotateDegrees, ref string error)
        {
            bool coverted = false;

            Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
            if (metadata == null)
            {
                error += (error == "" ? "" : "\r\n") + "Failed to rotated: " + fileEntry.FileFullPath + "\r\nMetadata was missing, need load metadata first.";
            }
            else if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsImageFormat(fileEntry.FileFullPath))
            {
                try
                {
                    bool isFileUnLockedAndExist = FileHandler.WaitLockedFileToBecomeUnlocked(fileEntry.FileFullPath, true, this);
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
                    bool isFileUnLockedAndExist = FileHandler.WaitLockedFileToBecomeUnlocked(fileEntry.FileFullPath, true, this);
                    timerSaveProgessRemoveProgress.Start();

                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                    durationMpegVideoConvertion = new TimeSpan(-1);

                    ffMpeg.ConvertProgress += FfMpeg_ConvertProgress;
                    ffMpeg.LogReceived += FfMpeg_LogReceived;
                    switch (rotateDegrees)
                    {
                        case 90:
                            ffMpeg.Invoke("-y -i \"" + fileEntry.FileFullPath + "\" -vf \"transpose=1\" \"" + tempOutputfile + "\"");
                            break;
                        case 180:
                            ffMpeg.Invoke("-y -i \"" + fileEntry.FileFullPath + "\" -vf \"transpose=2,transpose=2\" \"" + tempOutputfile + "\"");
                            break;
                        case 270:
                            ffMpeg.Invoke("-y -i \"" + fileEntry.FileFullPath + "\" -vf \"transpose=2\" \"" + tempOutputfile + "\"");
                            break;
                        default:
                            throw new NotImplementedException();
                    }

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
                        File.Delete(fileEntry.FileFullPath);
                        File.Move(tempOutputfile, fileEntry.FileFullPath);
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
                Metadata metadataOriginal = new Metadata(MetadataBrokerType.Empty);


                if (metadata != null)
                {                   
                    metadata.PersonalRegionRotate(rotateDegrees);
                    AddQueueSaveMetadataUpdatedByUserLock(metadata, metadataOriginal);
                }
                //ImageListViewReloadThumbnailInvoke(imageListView1, fileEntry.FileFullPath);
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
            lock (fileEntriesRotateMediaLock) return fileEntriesRotateMedia.Count();            
        }

        private int GetFileEntriesRotateMediaCountDirty()
        {
            return fileEntriesRotateMedia.Count();            
        }

        private void RotateInit(ImageListView imageListView, int rotateDegrees)
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
                MessageBox.Show("Need wait until metadata is read from media files befor rotating. Otherwise metadata from original media file will be lost.",
                    "Can't start rotate of media files yet.", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            }
            else if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
              "PhotoTagsSynchronizerApplication", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
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
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        #endregion

        #region Rotate - mmpeg Progress - Process bar
        private Stopwatch stopwatchRemoveSaveProgressbar = new Stopwatch();
        private TimeSpan durationMpegVideoConvertion = new TimeSpan();

        private void timerSaveProgessRemoveProgress_Tick(object sender, EventArgs e)
        {
            if (stopwatchRemoveSaveProgressbar.Elapsed.TotalMilliseconds > 1000)
            {
                ProgressbarSaveProgress(false);
            }
        }

        private void MmpegProgress(string data)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(MmpegProgress), data);
                return;
            }

            stopwatchRemoveSaveProgressbar.Restart();

            //  Duration: 00:00:06.96, start: 0.000000, bitrate: 2446 kb/s
            if (data.StartsWith("  Duration:") && data.Contains("start:") && data.Contains("bitrate:"))
            {


                int startIndex = data.IndexOf("Duration:") + "Duration:".Length;
                int endIndex = data.IndexOf(", start:");
                if (startIndex >= 0 && endIndex > startIndex)
                {
                    string durationString = data.Substring(startIndex, endIndex - startIndex);

                    bool didParase = false;
                    if (TimeSpan.TryParse(durationString, CultureInfo.InvariantCulture, out TimeSpan result))
                    {
                        durationMpegVideoConvertion = result;
                        didParase = true;
                    }
                    else if (TimeSpan.TryParse(durationString, CultureInfo.CurrentCulture, out TimeSpan result2))
                    {
                        durationMpegVideoConvertion = result2;
                        didParase = true;
                    }
                    if (didParase)
                    {
                        ProgressbarSaveAndConvertProgress(true, (int)durationMpegVideoConvertion.TotalMilliseconds / 100, 0, 100, "ffmmpeg rotate");
                    }
                }
            }

            //frame=   68 fps= 17 q=29.0 size=       0kB time=00:00:02.35 bitrate=   0.2kbits/s speed=0.601x 
            if (data.StartsWith("frame=") && data.Contains("fps=") && data.Contains("time=") && data.Contains("bitrate="))
            {
                int startIndex = data.IndexOf("time=") + "time=".Length;
                int endIndex = data.IndexOf(" bitrate=");

                if (startIndex >= 0 && endIndex > startIndex)
                {
                    TimeSpan locationMpegVideoConvertion = new TimeSpan();
                    string progressTimeString = data.Substring(startIndex, endIndex - startIndex);

                    bool didParase = false;
                    if (TimeSpan.TryParse(progressTimeString, CultureInfo.InvariantCulture, out TimeSpan result))
                    {
                        didParase = true;
                        locationMpegVideoConvertion = result;
                    }
                    else if (TimeSpan.TryParse(progressTimeString, CultureInfo.CurrentCulture, out TimeSpan result2))
                    {
                        didParase = true;
                        locationMpegVideoConvertion = result2;
                    }
                    if (didParase)
                    {
                        ProgressbarSaveAndConvertProgress(true, (int)durationMpegVideoConvertion.TotalMilliseconds / 100);
                    }
                }
            }
        }

        private void FfMpeg_LogReceived(object sender, NReco.VideoConverter.FFMpegLogEventArgs e)
        {
            MmpegProgress(e.Data);
        }


        private void FfMpeg_ConvertProgress(object sender, NReco.VideoConverter.ConvertProgressEventArgs e)
        {
            Debug.WriteLine(e.TotalDuration.ToString() + " " + e.Processed.ToString());
        }
        #endregion

        #endregion 
    }
}
