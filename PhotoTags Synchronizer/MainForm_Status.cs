using System;
using System.Windows.Forms;
using System.IO;
using Manina.Windows.Forms;
using MetadataLibrary;
using System.Collections.Generic;
using System.Diagnostics;
using FileHandeling;
using Krypton.Toolkit;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using DataGridViewGeneric;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        #region UpdateStatus - Show what's going on text for x ms.
        
        #region UpdateStatusAction - Remove (+Timer stop)
        private void timerShowStatusText_RemoveTimer_Tick(object sender, EventArgs e)
        {
            timerShowStatusText_RemoveTimer.Stop();
            StatusActionText = "Waiting actions...";
        }
        #endregion

        #region UpdateStatusAction - (+Timer start)
        public void UpdateStatusAction(string text)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(UpdateStatusAction), text);
                return;
            }

            StatusActionText = text;
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

        private static FormMessageBox formMessageBoxThread = null;

        #region Show Status Form / Window
        private void AddTaskToFileTasks(Dictionary<string, List<string>> fileTasks, string fullFileName, DateTime? modifiedDate, string task)
        {
            if (!fileTasks.ContainsKey(fullFileName))
            {
                fileTasks.Add(fullFileName, new List<string>());
                if (!File.Exists(fullFileName)) fileTasks[fullFileName].Add("File not exists");
                else
                {
                    if (FileHandler.IsFileReadOnly(fullFileName)) fileTasks[fullFileName].Add("ReadOnly");
                    if (FileHandler.IsFileLockedByProcess(fullFileName, 500)) fileTasks[fullFileName].Add("**Locked** or timeout when open file");                                            
                    if (FileHandler.IsFileInCloud(fullFileName)) fileTasks[fullFileName].Add("In cloud");
                    if (FileHandler.IsFileVirtual(fullFileName)) fileTasks[fullFileName].Add("Virtual file");
                }
            }
            fileTasks[fullFileName].Add(task);
            if (modifiedDate != null) 
            {
                DateTime? lastAccessTime = null;
                try
                {
                    if (File.Exists(fullFileName)) lastAccessTime = File.GetLastAccessTime(fullFileName);
                } catch { }                
                fileTasks[fullFileName].Add("  " + modifiedDate.ToString() + " vs. " + (lastAccessTime == modifiedDate ? "" : (lastAccessTime == null ? "File not exists or can't read last access time" : lastAccessTime.ToString())));                
            } 
        }

        private void ActionSeeProcessQueue()
        {
            Dictionary<string, List<string>> fileTasks = new Dictionary<string, List<string>>();
            string messageBoxQueuesInfo = "List of all process queues...\r\n";

            try
            {
                messageBoxQueuesInfo += string.Format("Files: {0} Selected {1}\r\n", imageListView1.Items.Count, imageListView1.SelectedItems.Count);
                if (CommonQueueLazyLoadingMetadataCountDirty() > 0)
                    messageBoxQueuesInfo += string.Format("Lazy loading queue: {0}\r\n", CommonQueueLazyLoadingMetadataCountDirty());
                if (!string.IsNullOrWhiteSpace(FileHandler.FileLockedByProcess))                
                    messageBoxQueuesInfo += "**Locked file: " + FileHandler.FileLockedByProcess;                
            }
            catch { }


            if (readToCacheQueues.Count > 0)
            {
                try
                {
                    messageBoxQueuesInfo += "Read to cache:";
                    lock (_readToCacheQueuesLock)
                        foreach (KeyValuePair<int, int> keyValuePair in readToCacheQueues) ProgressBackgroundStatusText += " " + keyValuePair.Value;
                    messageBoxQueuesInfo += "\r\n";
                }
                catch
                {
                }
            }

            if (deleteRecordQueues.Count > 0)
            {
                try
                {
                    messageBoxQueuesInfo += "Delete records:";
                    lock (_deleteRecordQueuesLock)
                    {
                        foreach (KeyValuePair<int, int> keyValuePair in deleteRecordQueues) messageBoxQueuesInfo += " " + keyValuePair.Value;
                        messageBoxQueuesInfo += "\r\n";
                    }
                }
                catch
                {
                }
            }
            
            messageBoxQueuesInfo = "\r\n";

            try
            {
                if (!string.IsNullOrEmpty(FileHandler.FileLockedByProcess)) messageBoxQueuesInfo += "Last file Locked by process: " + FileHandler.FileLockedByProcess + "\r\n";
            }
            catch { }

            try
            {
                lock (_fileSaveSizeLock)
                {
                    foreach (KeyValuePair<string, long> keyValuePair in fileSaveSizeWatcher)
                        AddTaskToFileTasks(fileTasks, keyValuePair.Key, null, "Written: " + keyValuePair.Value);                    
                }
            }
            catch { }

            try
            {
                lock (commonQueueReadMetadataFromExiftoolLock)
                    foreach (FileEntry fileEntry in commonQueueReadMetadataFromExiftool)
                        AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.LastWriteDateTime, "Exiftool read: in queue, wait on turn");
            }
            catch { }

            try
            {
                lock (mediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase)
                        AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, null, "Exiftool read, in process");
            }
            catch { }

            try
            {
                lock (commonQueueMetadataWrittenByExiftoolReadyToVerifyLock)
                    foreach (Metadata fileEntry in commonQueueMetadataWrittenByExiftoolReadyToVerify)
                        AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.FileDateModified, "Will be verified after Exiftool readback");
            }
            catch { }

            try
            {
                lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) 
                foreach (FileEntry fileEntry in commonQueueReadMetadataFromWindowsLivePhotoGallery)
                        AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.LastWriteDateTime, "Read meta information from Windows Live Photo Gallery");
            }
            catch { }

            try
            {
                lock (commonQueueReadMetadataFromMicrosoftPhotosLock)
                    foreach (FileEntry fileEntry in commonQueueReadMetadataFromMicrosoftPhotos)
                        AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.LastWriteDateTime, "Read meta information from Microsoft Photos");
            }
            catch { }

            try
            {

                lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                    foreach (Metadata fileEntry in commonQueueReadPosterAndSaveFaceThumbnails)
                        if (fileEntry.PersonalRegionList.Count > 0)
                        AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.FileDateModified, "Create thumbnail for region: " + fileEntry.PersonalRegionList.Count.ToString());
            }
            catch { }

            try
            {
                lock (commonQueueSaveMetadataUpdatedByUserLock)
                    foreach (Metadata fileEntry in commonQueueSaveMetadataUpdatedByUser)
                        AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.FileDateModified, "Wait to be saved with " + fileEntry.PersonalRegionList.Count.ToString() + " regions");
            }
            catch { }

            try
            {
                lock (commonQueueSubsetMetadataToSaveLock)
                    foreach (Metadata fileEntry in commonQueueSubsetMetadataToSave)
                        AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.FileDateModified, "Saving bulk using exiftool with " + fileEntry.PersonalRegionList.Count.ToString() + " regions");                
            }
            catch { }

            try
            {
                lock (commonQueueRenameLock)
                    foreach (KeyValuePair<string, string> keyValuePair in commonQueueRename)
                        AddTaskToFileTasks(fileTasks, keyValuePair.Key, null, "Wait rename to " + keyValuePair.Value);
            }
            catch { }

            foreach (KeyValuePair<string, List<string>> keyValuePair in fileTasks)
            {
                messageBoxQueuesInfo += keyValuePair.Key + "\r\n"; //filename
                foreach (string task in keyValuePair.Value) messageBoxQueuesInfo += "  " + task + "\r\n"; //tasks
            }

            try
            {
                Logger.Warn(messageBoxQueuesInfo);
                if (formMessageBoxThread == null || formMessageBoxThread.IsDisposed) formMessageBoxThread = new FormMessageBox("Task list", messageBoxQueuesInfo);
                else formMessageBoxThread.UpdateMessage(messageBoxQueuesInfo);
                formMessageBoxThread.Owner = this;
                formMessageBoxThread.Show();
            }
            catch { }
            
        }
        #endregion

        #region StatusFilesAndSelected
        private string StatusFilesAndSelected
        {
            set { this.kryptonPageMediaFiles.TextDescription = value; }
        }
        #endregion 

        #region DisplayAllQueueStatus - Updated display
        private Stopwatch stopwatchLastDisplayed = new Stopwatch();
        private Stopwatch stopwatchLastDisplayedExiftoolWaitCloud = new Stopwatch();
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

            StatusFilesAndSelected = string.Format("Files: {0} Selected {1} ", imageListView1.Items.Count, imageListView1.SelectedItems.Count);

            string progressBackgroundStatusText = "";
            int threadQueuCount = 0;

            if (!string.IsNullOrWhiteSpace(FileHandler.FileLockedByProcess)) {
                threadQueuCount++;
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    "Locked file: " + Path.GetFileName(FileHandler.FileLockedByProcess);
            }

            if (GetFileEntriesRotateMediaCountDirty() > 0) 
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") + 
                    string.Format("Rotate: {0}", GetFileEntriesRotateMediaCountDirty());
            threadQueuCount += GetFileEntriesRotateMediaCountDirty();

            if (GlobalData.ProcessCounterDelete > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") + 
                    string.Format("Delete: {0}", GlobalData.ProcessCounterDelete);
            threadQueuCount += GlobalData.ProcessCounterDelete;

            if (GlobalData.ProcessCounterReadProperties > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    string.Format("Properties: {0}", GlobalData.ProcessCounterReadProperties);
            threadQueuCount += GlobalData.ProcessCounterReadProperties;

            if (GlobalData.ProcessCounterRefresh > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") + 
                    string.Format("Reload: {0}", GlobalData.ProcessCounterRefresh);
            threadQueuCount += GlobalData.ProcessCounterRefresh;

            if (CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty() > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    string.Format("Read WLPG: {0}", CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty());
            threadQueuCount += CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty();

            if (CommonQueueReadMetadataFromMicrosoftPhotosCountDirty() > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    string.Format("Read MP: {0}", CommonQueueReadMetadataFromMicrosoftPhotosCountDirty());
            threadQueuCount += CommonQueueReadMetadataFromMicrosoftPhotosCountDirty();

            if (CommonQueueSaveThumbnailToDatabaseCountDirty() > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    string.Format("Save Thumbnails: {0}", CommonQueueSaveThumbnailToDatabaseCountDirty());
            threadQueuCount += CommonQueueSaveThumbnailToDatabaseCountDirty();

            int regionCount = 0;
            try
            {
                lock (commonQueueReadPosterAndSaveFaceThumbnailsLock) //CommonQueueReadPosterAndSaveFaceThumbnailsCountLock()
                {
                    foreach (Metadata metadataRegionCount in commonQueueReadPosterAndSaveFaceThumbnails) regionCount += metadataRegionCount.PersonalRegionList.Count;
                }
            }
            catch { }

            if (regionCount > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    string.Format("Save Regions: {0}", regionCount); 
            threadQueuCount += regionCount;

            if (CommonQueueReadMetadataFromExiftoolCountDirty() + MediaFilesNotInDatabaseCountDirty() > 0)
            {
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    string.Format("Exiftool: {0} in process: {1}", CommonQueueReadMetadataFromExiftoolCountDirty(), MediaFilesNotInDatabaseCountDirty());
                threadQueuCount += CommonQueueReadMetadataFromExiftoolCountDirty();
                if (!stopwatchLastDisplayedExiftoolWaitCloud.IsRunning) stopwatchLastDisplayedExiftoolWaitCloud.Start();
                if (stopwatchLastDisplayedExiftoolWaitCloud.ElapsedMilliseconds > 10000)
                {
                    try
                    {
                        int countWaitFileInCloud = 0;
                        int countWaitFileIsVirtual = 0;
                        foreach (FileEntry fileEntry in mediaFilesNotInDatabase)
                        {
                            if (FileHandler.IsFileInCloud(fileEntry.FileFullPath)) countWaitFileInCloud++;
                            if (FileHandler.IsFileVirtual(fileEntry.FileFullPath)) countWaitFileIsVirtual++;
                        }
                        if (countWaitFileInCloud + countWaitFileIsVirtual > 0) 
                            progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                                string.Format("wait cloud:{0} virtual: {1}", countWaitFileInCloud, countWaitFileIsVirtual);
                    }
                    catch
                    {

                    }
                    stopwatchLastDisplayedExiftoolWaitCloud.Restart();
                }
            }

            if (readToCacheQueues.Count > 0)
            {
                try
                {
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Caching: ");

                    lock (_readToCacheQueuesLock)
                    {
                        foreach (KeyValuePair<int, int> keyValuePair in readToCacheQueues)
                        {
                            progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                                //"#" + keyValuePair.Key.ToString() + " " +
                                keyValuePair.Value;
                            threadQueuCount += keyValuePair.Value;
                        }
                    }
                }
                catch
                {
                }
            }

            if (deleteRecordQueues.Count > 0)
            {
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") + "Delete: ";
                try
                {
                    lock (_deleteRecordQueuesLock)
                    {
                        foreach (KeyValuePair<int, int> keyValuePair in deleteRecordQueues)
                        {
                            progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") + keyValuePair.Value;
                            threadQueuCount += keyValuePair.Value;
                        }
                    }
                }
                catch
                {
                }
            }

            if (CountSaveQueueLock() > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                     string.Format("Saving: {0}", CountSaveQueueLock());
            threadQueuCount += CountSaveQueueLock();

            if (CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty() > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    string.Format("Verify:{0}", CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty());            
            threadQueuCount += CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty();

            if (CommonQueueRenameCountDirty() > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") + 
                    string.Format("Rename: {0}", CommonQueueRenameCountDirty());
            threadQueuCount += CommonQueueRenameCountDirty();
            
            if (CommonQueueLazyLoadingMetadataCountDirty() > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    string.Format("Metadata: {0}", CommonQueueLazyLoadingMetadataCountDirty());
            threadQueuCount += CommonQueueLazyLoadingMetadataCountDirty();

            if (CommonQueueLazyLoadingThumbnailCountDirty() > 0)
                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                    string.Format("Thumbnail: {0}", CommonQueueLazyLoadingThumbnailCountDirty());
            threadQueuCount += CommonQueueLazyLoadingThumbnailCountDirty();

            //int lasyLoadingDataGridViewCount = ThreadLazyLoadingDataGridViewQueueSizeDirty() + DataGridViewLazyLoadingCount();
            //if (lasyLoadingDataGridViewCount == 0) LazyLoadingDataGridViewProgressEndReached();

            try
            {
                if (threadQueuCount == 0) progressBackgroundStatusText = "Nothing...";
                else progressBackgroundStatusText = "(" + threadQueuCount + ") " + progressBackgroundStatusText;
                ProgressBackgroundStatusText = progressBackgroundStatusText;
            }
            catch { 
            }

            #region Updated progressbar
            try
            {
                int queueRemainding = threadQueuCount;
                ProgressbarBackgroundProgressQueueRemainding(queueRemainding);

                if (queueRemainding != 0)
                {
                    ProgressbarBackgroundProgress(true);
                }
                else
                { 
                    ProgressbarBackgroundProgress(false, 1, 0, 1);                    
                }
            } catch (Exception ex)
            {
                Logger.Error(ex, "DisplayAllQueueStatus");
            }
            #endregion 

        }
        #endregion

        #region DatabaseAndCacheMetadataExiftool_OnRecordReadToCache
        private Dictionary<int, int> readToCacheQueues = new Dictionary<int, int>();
        private static readonly Object _readToCacheQueuesLock = new Object();
        private void DatabaseAndCacheMetadataExiftool_OnRecordReadToCache(object sender, ReadToCacheFileEntriesRecordEventArgs e)
        {
            int queueLeft = e.FileEntries - (e.KeywordCount + e.MetadataCount + e.RegionCount);

            lock (_readToCacheQueuesLock)
            {
                if (e.InitCounter && e.FileEntries > 0 && queueLeft > 0)
                {
                    if (!readToCacheQueues.ContainsKey(e.HashQueue)) readToCacheQueues.Add(e.HashQueue, queueLeft);
                }
                else
                {
                    if (readToCacheQueues.ContainsKey(e.HashQueue))
                    {
                        if (queueLeft == 0 || e.Aborted)
                        {
                            if (readToCacheQueues.ContainsKey(e.HashQueue)) readToCacheQueues.Remove(e.HashQueue);
                        }
                        else
                        {
                            readToCacheQueues[e.HashQueue] = queueLeft;
                        }
                    }
                    else
                    {

                    }
                }
            }
        }
        #endregion

        #region DatabaseAndCacheMetadataExiftool_OnDeleteRecord
        private Dictionary<int, int> deleteRecordQueues = new Dictionary<int, int>();
        private static readonly Object _deleteRecordQueuesLock = new Object();
        private void DatabaseAndCacheMetadataExiftool_OnDeleteRecord(object sender, DeleteRecordEventArgs e)
        {
            int queueLeft = e.FileEntries - e.Count;
            lock (_deleteRecordQueuesLock)
            {
                if (e.InitCounter && e.FileEntries > 0 && queueLeft > 0)
                {
                    if (!readToCacheQueues.ContainsKey(e.HashQueue)) readToCacheQueues.Add(e.HashQueue, queueLeft);
                }
                else
                {
                    if (readToCacheQueues.ContainsKey(e.HashQueue))
                    {
                        if (queueLeft == 0 || e.Aborted)
                        {
                            if (readToCacheQueues.ContainsKey(e.HashQueue)) readToCacheQueues.Remove(e.HashQueue);
                        }
                        else
                        {
                            readToCacheQueues[e.HashQueue] = queueLeft;
                        }
                    }
                }
            }
            DisplayAllQueueStatus();
        }

        #endregion

        #endregion 

        #region UpdateExiftoolSaveStatus - Show Exiftool write progress, find Exiftool tmp file and show filesize on screen

        Dictionary<string, long> fileSaveSizeWatcher = new Dictionary<string, long>();
        private static readonly Object _fileSaveSizeLock = new Object();
        
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
                lock (_fileSaveSizeLock)
                {
                    if (fileSaveSizeWatcher.Count == 0) return;
                    foreach (KeyValuePair<string, long> keyValuePair in fileSaveSizeWatcher)
                    {
                        try
                        {
                            string tempFile = keyValuePair.Key + "_exiftool_tmp";                            
                            if (File.Exists(tempFile))
                            {
                                long tempFileSize = 0;
                                try
                                {
                                    tempFileSize = new FileInfo(tempFile).Length; //In case file already gone
                                    if (keyValuePair.Value != tempFileSize) UpdateStatusAction("Exiftool written " + tempFileSize + " bytes on " + Path.GetFileName(keyValuePair.Key));
                                    fileSaveSizeWatcher[keyValuePair.Key] = tempFileSize;
                                }
                                catch { }
                                
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
            lock (_fileSaveSizeLock) fileSaveSizeWatcher.Clear();
            //timerShowExiftoolSaveProgress.Start();
        }
        #endregion 

        #region UpdateExiftoolSaveStatus - AddWatcherShowExiftoolSaveProcessQueue
        private void AddWatcherShowExiftoolSaveProcessQueue(string fullFileName)
        {
            lock (_fileSaveSizeLock)
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
                lock (_fileSaveSizeLock)
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

        #region LazyLoadingDataGridViewProgress


        #region LazyLoadingDataGridViewProgress - End Reached
        public void LazyLoadingDataGridViewProgressEndReached()
        {
            LazyLoadingDataGridViewProgressUpdateStatus(0);
        }
        #endregion

        #region LazyLoadingDataGridViewProgressHide
        private static Thread _ThreadDelayLazyLoadingHide = null;
        public void LazyLoadingDataGridViewProgressHide()
        {
            if (IsProgressbarLazyLoadingProgressVisible) //Delayed visible
            {
                if (_ThreadDelayLazyLoadingHide == null)
                {
                    try
                    {
                        _ThreadDelayLazyLoadingHide = new Thread(() =>
                        {
                            Task.Delay(100).Wait();

                            if (lastQueueSize == 0 && this.IsHandleCreated) ProgressbarLazyLoadingProgress(false);
                            _ThreadDelayLazyLoadingHide = null;
                        });

                        if (_ThreadDelayLazyLoadingHide != null) _ThreadDelayLazyLoadingHide.Start();
                    }
                    catch
                    {
                        _ThreadDelayLazyLoadingHide = null;
                    }
                }
            }
        }

        #region LazyLoadingDataGridViewProgressStarted
        private static Thread _ThreadDelayLazyLoadingShow = null;
        public void LazyLoadingDataGridViewProgressShow()
        {
            if (!IsProgressbarLazyLoadingProgressVisible) //Delayed visible
            {
                if (_ThreadDelayLazyLoadingShow == null)
                {
                    try
                    {
                        _ThreadDelayLazyLoadingShow = new Thread(() =>
                        {
                            Task.Delay(10).Wait();
                            ProgressbarLazyLoadingProgress(lastQueueSize > 0);
                            _ThreadDelayLazyLoadingShow = null;
                        });

                        if (_ThreadDelayLazyLoadingShow != null) _ThreadDelayLazyLoadingShow.Start();
                    }
                    catch
                    {
                        _ThreadDelayLazyLoadingShow = null;
                    }
                }

            }
        }
        #endregion

        #endregion

        #region GetProgressCircle(int procentage)
        private Bitmap GetProgressCircle(int procentage)
        {
            if (procentage <= 6) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle01_16x16;
            else if (procentage <= 12) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle02_16x16;
            else if (procentage <= 18) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle03_16x16;
            else if (procentage <= 24) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle04_16x16;
            else if (procentage <= 29) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle05_16x16;
            else if (procentage <= 35) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle06_16x16;
            else if (procentage <= 41) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle07_16x16;
            else if (procentage <= 47) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle08_16x16;
            else if (procentage <= 53) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle09_16x16;
            else if (procentage <= 59) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle10_16x16;
            else if (procentage <= 65) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle11_16x16;
            else if (procentage <= 71) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle12_16x16;
            else if (procentage <= 76) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle13_16x16;
            else if (procentage <= 82) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle14_16x16;
            else if (procentage <= 88) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle15_16x16;
            else if (procentage <= 94) return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle16_16x16;
            return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle17_16x16;
        }
        #endregion

        #region SetButtonSpecNavigator
        private Stopwatch stopwatch = new Stopwatch();
        private void SetButtonSpecNavigator(Krypton.Navigator.ButtonSpecNavigator buttonSpecNavigator, int value, int maximum)
        {
            int procentage = 0;
            if (value >= maximum) procentage = 100;
            else procentage = (int)(((double)value / (double)maximum) * 100);
            buttonSpecNavigatorDataGridViewProgressCircle.Image = GetProgressCircle(procentage);
            buttonSpecNavigatorDataGridViewProgressCircle.ImageStates.ImageNormal = GetProgressCircle(procentage);

            if (!stopwatch.IsRunning || stopwatch.ElapsedMilliseconds > 200)
            {
                stopwatch.Restart();
                buttonSpecNavigatorDataGridViewProgressCircle.Visible = false;
                buttonSpecNavigatorDataGridViewProgressCircle.Visible = true;
                //DataGridViewHandler.SuspendLayoutSetDelay(GetActiveTabDataGridView(), false);
                //kryptonWorkspaceCellToolbox.Refresh();
                //DataGridViewHandler.ResumeLayoutDelayed(GetActiveTabDataGridView());
            }
        }

        #endregion

        #region ProgressbarLazyLoadingProgressLazyLoadingRemainding(int queueRemainding)
        private int ProgressbarLazyLoadingProgressLazyLoadingRemainding(int queueRemainding)
        {           
            if (queueRemainding > progressBarLazyLoading.Maximum) progressBarLazyLoading.Maximum = queueRemainding;
            if (queueRemainding == 0) progressBarLazyLoading.Maximum = 0;
            progressBarLazyLoading.Value = progressBarLazyLoading.Maximum - queueRemainding;
            SetButtonSpecNavigator(buttonSpecNavigatorDataGridViewProgressCircle, progressBarLazyLoading.Value, progressBarLazyLoading.Maximum);
            return progressBarLazyLoading.Value;
        }
        #endregion

        #region ProgressbarLazyLoadingProgress(bool visible)
        private void ProgressbarLazyLoadingProgress(bool visible)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<bool>(ProgressbarLazyLoadingProgress), visible);
                return;
            }

            if (!visible) 
                progressBarLazyLoading.Maximum = 0;

            kryptonRibbonGroupTripleToolsProgressStatusWork.Visible = visible;
            kryptonRibbonGroupLabelToolsProgressLazyloading.Enabled = visible;
            kryptonRibbonGroupCustomControlToolsProgressLazyloading.Enabled = visible;
            buttonSpecNavigatorDataGridViewProgressCircle.Visible = visible;
        }
        #endregion

        #region IsProgressbarLazyLoadingProgressVisible
        private bool IsProgressbarLazyLoadingProgressVisible
        {
            get { return kryptonRibbonGroupTripleToolsProgressStatusWork.Visible; }
        }
        #endregion

        #region LazyLoadingDataGridViewProgress - Update Status
        private static int lastQueueSize = 0;
        public void LazyLoadingDataGridViewProgressUpdateStatus(int queueSize)
        {
            int queueCount = ProgressbarLazyLoadingProgressLazyLoadingRemainding(queueSize);
            lastQueueSize = queueSize;

            if (queueSize > 0)
            {
                LazyLoadingDataGridViewProgressShow();
            }
            else
            {
                LazyLoadingDataGridViewProgressHide();
            }
        }         
        #endregion
        
        #endregion

    }
}