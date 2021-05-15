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

        #region UpdateStatus - Show what's going on text for x ms.
        
        #region UpdateStatusAction - Remove (+Timer stop)
        private void timerShowStatusText_RemoveTimer_Tick(object sender, EventArgs e)
        {
            timerShowStatusText_RemoveTimer.Stop();
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
            timerShowStatusText_RemoveTimer.Stop(); //Restart
            timerShowStatusText_RemoveTimer.Start();
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
            UpdateStatusAction("Metadata read using Exiftool: " + fileEntry.FileName);
        }
        #endregion

        #endregion

        #region Display All Numbers of items in Thread Queues
        
        #region DisplayAllQueueStatus - Tick - 400 ms
        private void timerStatusThreadQueue_Tick(object sender, EventArgs e)
        {
            DisplayAllQueueStatus();
            //Application.DoEvents();
        }
        #endregion 

        #region DisplayAllQueueStatus - Updated display
        private Stopwatch stopwatchLastDisplayed = new Stopwatch();
        private void DisplayAllQueueStatus()
        {
            if (!stopwatchLastDisplayed.IsRunning) stopwatchLastDisplayed.Start();
            if (stopwatchLastDisplayed.ElapsedMilliseconds < 500)
                return; //Avoid to much refresh
            stopwatchLastDisplayed.Restart();

            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(DisplayAllQueueStatus));
                return;
            }

            toolStripStatusFilesAndSelected.Text = string.Format("Files: {0} Selected {1} ", imageListView1.Items.Count, imageListView1.SelectedItems.Count);

            toolStripStatusThreadQueueCount.Text = "";
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
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Rotate: {0}", GetFileEntriesRotateMediaCountDirty());
            threadQueuCount += GetFileEntriesRotateMediaCountDirty();

            if (CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Read WLPG: {0}", CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty());
            threadQueuCount += CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty();

            if (CommonQueueReadMetadataFromMicrosoftPhotosCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Read MP: {0}", CommonQueueReadMetadataFromMicrosoftPhotosCountDirty());
            threadQueuCount += CommonQueueReadMetadataFromMicrosoftPhotosCountDirty();

            if (CommonQueueSaveThumbnailToDatabaseCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Save Thumbnails: {0}", CommonQueueSaveThumbnailToDatabaseCountDirty());
            threadQueuCount += CommonQueueSaveThumbnailToDatabaseCountDirty();

            if (regionCount > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Save Regions: {0}", regionCount); //CommonQueueReadPosterAndSaveFaceThumbnailsCountDirty(), 
            //threadQueuCount += CommonQueueReadPosterAndSaveFaceThumbnailsCountDirty();
            threadQueuCount += regionCount;


            if (CommonQueueReadMetadataFromExiftoolCountDirty() > 0)
            {
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("DB Read:{0}", CommonQueueReadMetadataFromExiftoolCountDirty());
                threadQueuCount += CommonQueueReadMetadataFromExiftoolCountDirty();
                try
                {
                    foreach (KeyValuePair<int, int> keyValuePair in readToCacheQueues)
                        toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                            //"#" + keyValuePair.Key.ToString() + " " +
                            keyValuePair.Value;
                }
                catch { }
            }

            try
            {
                if (deleteRecordQueues.Count > 0)
                {
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + "Delete: ";

                    foreach (KeyValuePair<int, int> keyValuePair in deleteRecordQueues)
                        toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + keyValuePair.Value;
                }
            }
            catch { }
            

            if (MediaFilesNotInDatabaseCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Exiftool:{0}", MediaFilesNotInDatabaseCountDirty());
                threadQueuCount += MediaFilesNotInDatabaseCountDirty();

            if (CountSaveQueueLock() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                     string.Format("Saving: {0}", CountSaveQueueLock());
            threadQueuCount += CountSaveQueueLock();

            if (CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Verify:{0}", CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty());            
            threadQueuCount += CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty();

            if (CommonQueueRenameCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + 
                    string.Format("Rename: {0}", CommonQueueRenameCountDirty());
            threadQueuCount += CommonQueueRenameCountDirty();
            
            if (CommonQueuePreloadingMetadataCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + 
                    string.Format("Preload: {0}", CommonQueuePreloadingMetadataCountDirty()); 
            threadQueuCount += CommonQueuePreloadingMetadataCountDirty();

            if (CommonQueueLazyLoadingMetadataCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Metadata: {0}", CommonQueueLazyLoadingMetadataCountDirty());
            threadQueuCount += CommonQueueLazyLoadingMetadataCountDirty();
            
            if (CommonQueueLazyLoadingThumbnailCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Thumbnail: {0}", CommonQueueLazyLoadingThumbnailCountDirty());
            threadQueuCount += CommonQueueLazyLoadingThumbnailCountDirty();

            toolStripStatusThreadQueueCount.Text = "(" + threadQueuCount + ") " + toolStripStatusThreadQueueCount.Text;
            
            #region Updated progressbar
            int queueRemainding = threadQueuCount;
            if (queueRemainding > toolStripProgressBarThreadQueue.Maximum) toolStripProgressBarThreadQueue.Maximum = queueRemainding;
            toolStripProgressBarThreadQueue.Value = toolStripProgressBarThreadQueue.Maximum - queueRemainding;
            if (queueRemainding != 0)
            {
                toolStripStatusThreadQueueCount.Visible = true;
                toolStripProgressBarThreadQueue.Visible = true;
                toolStripLabelThreadQueue.Visible = true;
            }
            else
            {
                toolStripStatusThreadQueueCount.Visible = false;
                toolStripLabelThreadQueue.Visible = false;
                toolStripProgressBarThreadQueue.Maximum = 1;
                toolStripProgressBarThreadQueue.Visible = false;
            }
            #endregion 

        }
        #endregion

        #region DatabaseAndCacheMetadataExiftool_OnRecordReadToCache
        private Dictionary<int, int> readToCacheQueues = new Dictionary<int, int>();
        private void DatabaseAndCacheMetadataExiftool_OnRecordReadToCache(object sender, ReadToCacheFileEntriesRecordEventArgs e)
        {
            int queueLeft = e.FileEntries - (e.KeywordCount + e.MetadataCount + e.RegionCount) / 3;
            if (queueLeft == 0 || e.Aborted)
            {
                if (!readToCacheQueues.ContainsKey(e.HashQueue)) readToCacheQueues.Remove(e.HashQueue);
            } 
            else
            {
                if (!readToCacheQueues.ContainsKey(e.HashQueue)) readToCacheQueues.Add(e.HashQueue, queueLeft);
                else readToCacheQueues[e.HashQueue] = queueLeft;
            }            
        }
        #endregion

        #region DatabaseAndCacheMetadataExiftool_OnDeleteRecord
        private Dictionary<int, int> deleteRecordQueues = new Dictionary<int, int>();
        private void DatabaseAndCacheMetadataExiftool_OnDeleteRecord(object sender, DeleteRecordEventArgs e)
        {
            int queueLeft = e.FileEntries - e.Count;
            if (queueLeft == 0)
            {
                if (!deleteRecordQueues.ContainsKey(e.HashQueue)) deleteRecordQueues.Remove(e.HashQueue);
            }
            else
            {
                if (!deleteRecordQueues.ContainsKey(e.HashQueue)) deleteRecordQueues.Add(e.HashQueue, queueLeft);
                else deleteRecordQueues[e.HashQueue] = queueLeft;
                
            }
            //DisplayAllQueueStatus();
        }

        #endregion

        #endregion 


        #region UpdateExiftoolSaveStatus - Show Exiftool write progress, find Exiftool tmp file and show filesize on screen

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

        #region Updated General Progess (Delete Count, Do something progress)

        private Stopwatch stopwatchhDelayShowProgressbar = new Stopwatch();
        private int generalProgressCountIndex = 0;
        private int generalProgressCountSize = 0;

        #region GeneralProgressIncrement
        public void GeneralProgressIncrement()
        {
            generalProgressCountIndex++;
            if (generalProgressCountIndex > generalProgressCountSize) 
                generalProgressCountSize = generalProgressCountIndex + 100;
            GeneralProgressIncrementSetProgerss(generalProgressCountSize, generalProgressCountIndex);
        }
        #endregion 

        #region GeneralProgressEndReached
        public void GeneralProgressEndReached()
        {
            timerUpdateGeneralProgressRemoveProgessbar.Interval = 1000;
            timerUpdateGeneralProgressRemoveProgessbar.Start();
        }
        #endregion

        #region GeneralProgressIncrementCountdown
        private void GeneralProgressIncrementCountdown(int queueSize)
        {
            if (queueSize > toolStripProgressBarGeneralProgress.Maximum) toolStripProgressBarGeneralProgress.Maximum = queueSize;
            GeneralProgressIncrementSetProgerss(toolStripProgressBarGeneralProgress.Maximum, toolStripProgressBarGeneralProgress.Maximum - queueSize);
        }
        #endregion

        #region GeneralProgressIncrementSetProgerss
        private void GeneralProgressIncrementSetProgerss(int queueSize, int queueCount = 0)
        {

            generalProgressCountIndex = queueCount;
            generalProgressCountSize = queueSize;

            if (queueSize > toolStripProgressBarGeneralProgress.Maximum) toolStripProgressBarGeneralProgress.Maximum = queueSize;
            toolStripProgressBarGeneralProgress.Value = queueCount;

            if (queueSize != queueCount && queueSize > 1)
            {
                toolStripProgressBarGeneralProgress.Visible = true;
                toolStripLabelGeneralProgress.Visible = true;
            }
            else
            {
                toolStripProgressBarGeneralProgress.Visible = false;
                toolStripLabelGeneralProgress.Visible = false;
                toolStripProgressBarGeneralProgress.Value = 0;
                toolStripProgressBarGeneralProgress.Maximum = 1;
                generalProgressCountIndex = 0;
                generalProgressCountSize = 0;
            }

            if (toolStripProgressBarGeneralProgress.Visible)
            {
                if (stopwatchhDelayShowProgressbar == null)
                {
                    stopwatchhDelayShowProgressbar = new Stopwatch();
                    stopwatchhDelayShowProgressbar.Restart();
                }
                if (!stopwatchhDelayShowProgressbar.IsRunning) stopwatchhDelayShowProgressbar.Restart();
                /*
                if (stopwatchhDelayShowProgressbar.IsRunning && stopwatchhDelayShowProgressbar.ElapsedMilliseconds < 600)
                {
                    toolStripProgressBarGeneralProgress.Visible = true;
                    toolStripLabelGeneralProgress.Visible = true;
                }*/
            }           
        }
        #endregion

        #region timerUpdateGeneralProgressRemoveProgessbar_Tick
        private void timerUpdateGeneralProgressRemoveProgessbar_Tick(object sender, EventArgs e)
        {
            stopwatchhDelayShowProgressbar.Stop();
            toolStripProgressBarGeneralProgress.Visible = false;
            toolStripLabelGeneralProgress.Visible = false;
            toolStripProgressBarGeneralProgress.Maximum = 1;
            generalProgressCountIndex = 0;
            generalProgressCountSize = 0;
            timerUpdateGeneralProgressRemoveProgessbar.Stop(); 
        }
        #endregion 

        #endregion

    }
}
