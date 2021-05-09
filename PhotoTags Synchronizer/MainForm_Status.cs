using System;
using System.Windows.Forms;
using System.IO;
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

        #region DisplayAllQueueStatus
        
        #region DisplayAllQueueStatus - Tick
        private void timerStatusUpdate_Tick(object sender, EventArgs e)
        {
            DisplayAllQueueStatus();
        }
        #endregion 


        #region DisplayAllQueueStatus - Updated display
        private Stopwatch stopwatchLastDisplayed = new Stopwatch();
        private void DisplayAllQueueStatus()
        {
            if (!stopwatchLastDisplayed.IsRunning) stopwatchLastDisplayed.Start();
            if (stopwatchLastDisplayed.ElapsedMilliseconds < 500) 
                return;
            stopwatchLastDisplayed.Restart();

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
                    foreach (Metadata metadataRegionCount in commonQueueReadPosterAndSaveFaceThumbnails) regionCount += metadataRegionCount.PersonalRegionList.Count;
                }
            } catch { }
            
            int threadQueuCount = 0;
            if (GetFileEntriesRotateMediaCountDirty() > 0) 
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Rotate: {0}", GetFileEntriesRotateMediaCountDirty());
            threadQueuCount += GetFileEntriesRotateMediaCountDirty();

            if (CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty() > 0)
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("WLPG: {0}", CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty());
            threadQueuCount += CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty();
            
            if (CommonQueueReadMetadataFromMicrosoftPhotosCountDirty() > 0)
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("MP: {0}", CommonQueueReadMetadataFromMicrosoftPhotosCountDirty());
            threadQueuCount += CommonQueueReadMetadataFromMicrosoftPhotosCountDirty();
            
            if (CommonQueueSaveThumbnailToDatabaseCountDirty() > 0)
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Thumbnails: {0}", CommonQueueSaveThumbnailToDatabaseCountDirty());
            threadQueuCount += CommonQueueSaveThumbnailToDatabaseCountDirty();
            
            if (CommonQueueReadPosterAndSaveFaceThumbnailsCountDirty() + regionCount > 0 )
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Regions: {0}/{1}", 
                CommonQueueReadPosterAndSaveFaceThumbnailsCountDirty(), regionCount);
            threadQueuCount += CommonQueueReadPosterAndSaveFaceThumbnailsCountDirty();
            threadQueuCount += regionCount;
            
            if (CommonQueueReadMetadataFromExiftoolCountDirty() + MediaFilesNotInDatabaseCountDirty() + CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty() > 0)
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Exif: Check:{0} Exiftool:{1} Verify:{2}",
                CommonQueueReadMetadataFromExiftoolCountDirty(), MediaFilesNotInDatabaseCountDirty(), CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty());
            
            threadQueuCount += CommonQueueReadMetadataFromExiftoolCountDirty();
            threadQueuCount += MediaFilesNotInDatabaseCountDirty();
            threadQueuCount += CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty();
            
            if (CountSaveQueueLock() > 0)                                   
               toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Saving: {0}", CountSaveQueueLock());
            threadQueuCount += CountSaveQueueLock();
            
            if (CommonQueueRenameCountDirty() > 0)
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Rename: {0}", CommonQueueRenameCountDirty());
            threadQueuCount += CommonQueueRenameCountDirty();
            
            if (CommonQueuePreloadingMetadataCountDirty() + CommonQueueLazyLoadingMetadataCountDirty() + CommonQueueLazyLoadingThumbnailCountDirty() > 0)
                toolStripStatusQueue.Text += (toolStripStatusQueue.Text == "" ? "" : " ") + string.Format("Preload: {0}, Metadata: {1}, Thumbnail: {2}",
                CommonQueuePreloadingMetadataCountDirty(),
                CommonQueueLazyLoadingMetadataCountDirty(),
                CommonQueueLazyLoadingThumbnailCountDirty()); 
            threadQueuCount += CommonQueuePreloadingMetadataCountDirty();
            threadQueuCount += CommonQueueLazyLoadingMetadataCountDirty();
            threadQueuCount += CommonQueueLazyLoadingThumbnailCountDirty();
            
            LoadDataThreadProgerssCountDown(threadQueuCount);
        }
        #endregion

        #region LoadDataThreadProgerssCountDown
        private void LoadDataThreadProgerssCountDown(int queueRemainding)
        {
            if (queueRemainding > toolStripProgressBarThreadQueue.Maximum) toolStripProgressBarThreadQueue.Maximum = queueRemainding;
            toolStripProgressBarThreadQueue.Value = queueRemainding;
            if (queueRemainding != 0) toolStripProgressBarThreadQueue.Visible = true;
            else
            {
                toolStripProgressBarThreadQueue.Maximum = 1;
                toolStripProgressBarThreadQueue.Visible = false;
            } 
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
        private int CountSaveQueueLock()
        {
            int countToSave = CommonQueueSaveMetadataUpdatedByUserCountLock();
            try
            {  
                lock (fileSaveSizeLock)
                {                    
                    if (countToSave == 0 && fileSaveSizeWatcher.Count == 0) return countToSave; //Zero 0
                    foreach (KeyValuePair<string, long> keyValuePair in fileSaveSizeWatcher) if (keyValuePair.Value == 0) countToSave++; //In progress with Exiftool
                }
            }
            catch { } //It's not thread safe, if error, don't care
            return countToSave;
        }
        #endregion

        #endregion


        #region Updated General LoadDataGridViewProgerssAdd
        private Stopwatch stopwatchhDelayShowProgressbar = new Stopwatch();
        private int queueCountIndex = 0;
        private int queueCountSize = 0;
        public void LoadDataGridViewProgerssAdd()
        {
            queueCountIndex++;
            if (queueCountIndex > queueCountSize) 
                queueCountSize = queueCountIndex + 100;
            LoadDataGridViewProgerss(queueCountSize, queueCountIndex);
        }

        public void LoadDataGridViewProgerssEnded()
        {
            timerUpdateDataGridViewLoadingProgressbarRemove.Interval = 3000;
            timerUpdateDataGridViewLoadingProgressbarRemove.Start();
        }
        private void LoadDataGridViewProgerssCountDown(int queueSize)
        {
            if (queueSize > toolStripProgressBarDataGridViewLoading.Maximum) toolStripProgressBarDataGridViewLoading.Maximum = queueSize;
            LoadDataGridViewProgerss(toolStripProgressBarDataGridViewLoading.Maximum, toolStripProgressBarDataGridViewLoading.Maximum - queueSize);
        }

        private void LoadDataGridViewProgerss(int queueSize, int queueCount = 0)
        {
            queueCountIndex = queueCount;
            queueCountSize = queueSize;

            if (queueSize > toolStripProgressBarDataGridViewLoading.Maximum) toolStripProgressBarDataGridViewLoading.Maximum = queueSize;
            toolStripProgressBarDataGridViewLoading.Value = queueCount;

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

        }
        

        private void timerUpdateDataGridViewLoadingProgressbarRemove_Tick(object sender, EventArgs e)
        {
            stopwatchhDelayShowProgressbar.Stop();
            toolStripProgressBarDataGridViewLoading.Visible = false;
            toolStripProgressBarDataGridViewLoading.Maximum = 1;
            queueCountIndex = 0;
            queueCountSize = 0;
            timerUpdateDataGridViewLoadingProgressbarRemove.Stop(); 
        }

        #endregion 

    }
}
