using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SqliteDatabase;
using DataGridViewGeneric;
using Manina.Windows.Forms;
using MetadataLibrary;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {

        #region UpdateStatusAction
        
        #region UpdateStatusAction - Remove (+Timer stop)
        private void timerActionStatusRemove_Tick(object sender, EventArgs e)
        {
            timerActionStatusRemove.Stop();
            toolStripStatusAction.Text = "Waiting actions...";
        }
        #endregion

        #region UpdateStatusAction - (+Timer start)
        private void UpdateStatusAction(string text)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(UpdateStatusAction), text);
                return;
            }

            toolStripStatusAction.Text = text;
            timerActionStatusRemove.Stop(); //Restart
            timerActionStatusRemove.Start();
        }
        #endregion 

        #region UpdatedStatusAction - Trigger by imageListView1_ThumbnailCaching
        private void imageListView1_ThumbnailCaching(object sender, ItemEventArgs e)
        {
            UpdateStatusAction(string.Format("Cacheing image: {0}", e.Item.Text));
        }
        #endregion

        #region UpdatedStatusAction - Trigger by ExiftoolReader_afterNewMediaFoundEvent
        private void ExiftoolReader_afterNewMediaFoundEvent(FileEntry fileEntry)
        {
            lock (commonQueueReadMetadataFromExiftoolLock) commonQueueReadMetadataFromExiftool.Remove(fileEntry);            
            UpdateStatusAction("Exiftool read and cached: " + fileEntry.FileName);
            DisplayAllQueueStatus(); //Update number count also
        }
        #endregion

        #endregion

        #region #region DisplayAllQueueStatus
        
        #region DisplayAllQueueStatus - Tick
        private void timerStatusUpdate_Tick(object sender, EventArgs e)
        {
            DisplayAllQueueStatus();
        }
        #endregion 

        private string ThreadStateStatus(System.Threading.Thread thread)
        {
            if (thread == null) return "";
            return thread.ThreadState.ToString();
        }

        #region DisplayAllQueueStatus - Updated display        
        private void DisplayAllQueueStatus()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(DisplayAllQueueStatus));
                return;
            }

            toolStripStatusFilesAndSelected.Text = string.Format("Files: {0} Selected {1} ", imageListView1.Items.Count, imageListView1.SelectedItems.Count);

            int regionCount = 0;
            try
            {
                lock (commonQueueReadPosterAndSaveFaceThumbnailsLock) //CommonQueueReadPosterAndSaveFaceThumbnailsCountLock()
                {
                    //It's not thread safe
                    foreach (Metadata metadataRegionCount in commonQueueReadPosterAndSaveFaceThumbnails) regionCount += metadataRegionCount.PersonalRegionList.Count;
                }
            } catch { }

            toolStripStatusQueue.Text = "";

            //string.Format("WLPG{0}:{1} MP{2}:{3} Thumbnails{4}:{5} Regions{6}:{7}|{8} Exif{9}: Check:{10} Exiftool:{11} Saving{12}:{13} Verify:{14} Rename:{15} Lazy({16}{17},{18}{19},{20}{21})",
            bool showAlways = false;

            if (showAlways || CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() > 0 || (_ThreadWindowsLiveGallery != null && _ThreadWindowsLiveGallery.ThreadState != System.Threading.ThreadState.Stopped))
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("WLPG-{0}:{1}", 
                ThreadStateStatus(_ThreadWindowsLiveGallery), CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock());

            if (showAlways || CommonQueueReadMetadataFromMicrosoftPhotosCountLock() > 0 || (_ThreadMicrosoftPhotos != null && _ThreadMicrosoftPhotos.ThreadState != System.Threading.ThreadState.Stopped))
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("MP-{0}:{1}", 
                ThreadStateStatus(_ThreadMicrosoftPhotos), CommonQueueReadMetadataFromMicrosoftPhotosCountLock());

            if (showAlways || CommonQueueSaveThumbnailToDatabaseCountLock() > 0 || (_ThreadThumbnailMedia != null && _ThreadThumbnailMedia.ThreadState != System.Threading.ThreadState.Stopped))
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Thumbnails-{0}:{1}", 
                ThreadStateStatus(_ThreadThumbnailMedia), CommonQueueSaveThumbnailToDatabaseCountLock());
                
            if (showAlways || CommonQueueReadPosterAndSaveFaceThumbnailsCountLock() + regionCount > 0 || (_ThreadThumbnailRegion == null && _ThreadThumbnailMedia.ThreadState != System.Threading.ThreadState.Stopped))
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Regions-{0}:{1}|{2}", 
                ThreadStateStatus(_ThreadThumbnailRegion), CommonQueueReadPosterAndSaveFaceThumbnailsCountLock(), regionCount);

            if (showAlways || CommonQueueReadMetadataFromExiftoolCountLock() +  MediaFilesNotInDatabaseCountLock() + CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountLock() > 0 || (_ThreadExiftool != null && _ThreadExiftool.ThreadState != System.Threading.ThreadState.Stopped))
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Exif-{0}: Check:{1} Exiftool:{2} Verify:{3}",
                ThreadStateStatus(_ThreadExiftool), CommonQueueReadMetadataFromExiftoolCountLock(), MediaFilesNotInDatabaseCountLock(), CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountLock());
            
            if (showAlways || CountSaveQueue() > 0 || (_ThreadSaveMetadata == null && _ThreadSaveMetadata.ThreadState != System.Threading.ThreadState.Stopped))                                   
               toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Saving-{0}:{1} Verify:{2}", 
               ThreadStateStatus(_ThreadSaveMetadata), CountSaveQueue(), CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountLock());

            if (showAlways || CommonQueueRenameCountLock() > 0 || (_ThreadRenameMedafiles == null && _ThreadRenameMedafiles.ThreadState != System.Threading.ThreadState.Stopped))
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Rename-{0}:{1}", 
               ThreadStateStatus(_ThreadRenameMedafiles), CommonQueueRenameCountLock());

            if (showAlways || CommonQueuePreloadingMetadataCountLock() + CommonQueueLazyLoadingMetadataCountLock() + CommonQueueLazyLoadingThumbnailCountLock() > 0)
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Preload-{0}:{1}, Metadata-{2}:{3}, Thumbnail-{4}:{5}",
                ThreadStateStatus(_ThreadPreloadingMetadata), CommonQueuePreloadingMetadataCountLock(),
                ThreadStateStatus(_ThreadLazyLoadingMetadata), CommonQueueLazyLoadingMetadataCountLock(),
                ThreadStateStatus(_ThreadLazyLoadingThumbnail), CommonQueueLazyLoadingThumbnailCountLock());
        }
        #endregion

        #endregion 


        #region UpdateExiftoolSaveStatus - Show Exiftool write progress
        
        Dictionary<string, long> fileSaveSizeWatcher = new Dictionary<string, long>();
        private static readonly Object fileSaveSizeLock = new Object();
        
        #region UpdateExiftoolSaveStatus - Trigger by Timer Tick
        private void timerShowExiftoolSaveProgress_Tick(object sender, EventArgs e)
        {
            UpdateExiftoolSaveStatus();
        }
        #endregion

        #region UpdateExiftoolSaveStatus - Update display 
        private void UpdateExiftoolSaveStatus()
        {
            try
            {
                lock (fileSaveSizeLock)
                {
                    if (fileSaveSizeWatcher.Count == 0) return;
                    foreach (KeyValuePair<string, long> keyValuePair in fileSaveSizeWatcher)
                    {
                        try
                        {
                            string tempFile = keyValuePair.Key + "_exiftool_tmp";
                            if (File.Exists(tempFile))
                            {
                                long tempFileSize = new FileInfo(tempFile).Length;
                                if (keyValuePair.Value != tempFileSize) UpdateStatusAction("Exiftool written " + tempFileSize + " bytes on " + Path.GetFileName(keyValuePair.Key));
                                fileSaveSizeWatcher[keyValuePair.Key] = tempFileSize;
                                DisplayAllQueueStatus();
                                break;
                            }
                        } catch { }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region UpdateExiftoolSaveStatus - ShowExiftoolSaveProgressClear()
        private void ShowExiftoolSaveProgressClear()
        {
            lock (fileSaveSizeLock) fileSaveSizeWatcher.Clear();
            //timerShowExiftoolSaveProgress.Start();
        }
        #endregion 

        #region UpdateExiftoolSaveStatus - AddWatcherShowExiftoolSaveProcessQueue
        private void AddWatcherShowExiftoolSaveProcessQueue(string fullFileName)
        {
            lock (fileSaveSizeLock)
            {
                if (!fileSaveSizeWatcher.ContainsKey(fullFileName)) fileSaveSizeWatcher.Add(fullFileName, 0);
            }
        }
        #endregion 

        #region UpdateExiftoolSaveStatus - CountSaveQueue
        private int CountSaveQueue()
        {
            int countToSave = CommonQueueSaveMetadataUpdatedByUserCountLock();
            try
            {  
                lock (fileSaveSizeLock)
                {                    
                    if (countToSave == 0 && fileSaveSizeWatcher.Count == 0) return countToSave; //Zero 0
                    foreach (KeyValuePair<string, long> keyValuePair in fileSaveSizeWatcher) if (keyValuePair.Value == 0) countToSave++; //In progress with Exiftool
                    //if (countToSave >= 0) countToSave++; //Something is in progress saving
                }
            }
            catch { } //It's not thread safe, if error, don't care
            return countToSave;
        }
        #endregion

        #endregion

        #region Updated Progressbar
        private Stopwatch stopwatchhDelayShowProgressbar = new Stopwatch();
        private void LoadDataGridViewProgerss(int queueSize, int queueCount = 0)
        {
            int threadLazyLoadingQueueSize = queueSize + queueCount;
            if (threadLazyLoadingQueueSize > toolStripProgressBarDataGridViewLoading.Maximum) toolStripProgressBarDataGridViewLoading.Maximum = threadLazyLoadingQueueSize;            
            toolStripProgressBarDataGridViewLoading.Value = toolStripProgressBarDataGridViewLoading.Maximum - threadLazyLoadingQueueSize;

            if (!toolStripProgressBarDataGridViewLoading.Visible)
            {
                if (stopwatchhDelayShowProgressbar == null)
                {
                    stopwatchhDelayShowProgressbar = new Stopwatch();
                    stopwatchhDelayShowProgressbar.Restart();
                } 
                if (!stopwatchhDelayShowProgressbar.IsRunning) stopwatchhDelayShowProgressbar.Restart();

                if (stopwatchhDelayShowProgressbar.IsRunning && stopwatchhDelayShowProgressbar.ElapsedMilliseconds > 600) toolStripProgressBarDataGridViewLoading.Visible = true;
            }

            if (queueSize == 0 && queueCount == 0)
            {
                timerUpdateDataGridViewLoadingProgressbarRemove.Interval = 3000;
                timerUpdateDataGridViewLoadingProgressbarRemove.Start();
            }
        }

        private void timerUpdateDataGridViewLoadingProgressbarRemove_Tick(object sender, EventArgs e)
        {
            stopwatchhDelayShowProgressbar.Stop();
            toolStripProgressBarDataGridViewLoading.Visible = false;
            

            timerUpdateDataGridViewLoadingProgressbarRemove.Stop(); 
        }

        #endregion 

    }
}
