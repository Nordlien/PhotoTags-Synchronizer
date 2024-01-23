using System;
using System.IO;
using Manina.Windows.Forms;
using MetadataLibrary;
using System.Collections.Generic;
using System.Diagnostics;
using FileHandeling;
using Krypton.Toolkit;
using System.Drawing;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        #region UpdateStatus - Show what's going on text for x ms.
        
        #region UpdateStatusAction - Remove (+Timer stop)
        private void timerShowStatusText_RemoveTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                timerShowStatusText_RemoveTimer.Stop();
                StatusActionText = "Waiting actions...";
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region UpdateStatusAction - (+Timer start)
        public void UpdateStatusAction(string text, bool forceRefresh = false)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new Action<string, bool>(UpdateStatusAction), text, forceRefresh);
                    return;
                }

                StatusActionText = text;
                if (forceRefresh) kryptonStatusStrip1.Refresh();

                timerShowStatusText_RemoveTimer.Stop(); //Restart
                timerShowStatusText_RemoveTimer.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region UpdateStatusActionDelayed 
        private DateTime whenNeedUpdatedDataGridViewLazyLoadingCounter = DateTime.Now;
        public void UpdateStatusActionDelayedRefresh(string text)
        {
            try
            {
                if (DateTime.Now < whenNeedUpdatedDataGridViewLazyLoadingCounter) return;

                if (InvokeRequired)
                {
                    this.BeginInvoke(new Action<string>(UpdateStatusActionDelayedRefresh), text);
                    return;
                }

                UpdateStatusAction(text, forceRefresh: true);
                whenNeedUpdatedDataGridViewLazyLoadingCounter = DateTime.Now.AddMilliseconds(300);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 


        #region UpdateStatusImageListView
        public void UpdateStatusImageListView(string text)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new Action<string>(UpdateStatusImageListView), text);
                    return;
                }

                this.kryptonPageMediaFiles.TextDescription = text;
                StatusActionText = text;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region UpdatedStatusAction - Trigger by imageListView1_ThumbnailCaching
        private void imageListView1_ThumbnailCaching(object sender, ItemEventArgs e)
        {   
            try
            {
                UpdateStatusAction(string.Format("Cacheing image: {0}", e.Item.Text));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region UpdatedStatusAction - Trigger by ExiftoolReader_afterNewMediaFoundEvent
        private void ExiftoolReader_afterNewMediaFoundEvent(FileEntry fileEntry)
        {
            try
            {
                lock (commonQueueReadMetadataFromSourceExiftoolLock) commonQueueReadMetadataFromSourceExiftool.Remove(fileEntry);
                UpdateStatusAction("Metadata read using Exiftool: " + fileEntry.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region Display All Numbers of items in Thread Queues
        
        #region DisplayAllQueueStatus - Tick - 400 ms
        private void timerStatusThreadQueue_Tick(object sender, EventArgs e)
        {
            DisplayAllQueueStatus();
        }
        #endregion 

        private static FormMessageBox formMessageBoxThread = null;

        #region -- Show Status Form / Window --
        private void AddTaskToFileTasks(Dictionary<string, List<string>> fileTasks, string fullFileName, DateTime? modifiedDate, string task)
        {
            try
            {

                if (!fileTasks.ContainsKey(fullFileName))
                {
                    fileTasks.Add(fullFileName, new List<string>());
                    FileStatus fileStatus = FileHandler.GetFileStatus(
                        fullFileName, checkLockedStatus: true, checkLockStatusTimeout: FileHandler.GetFileLockedStatusTimeout);

                    if (!fileStatus.FileExists) fileTasks[fullFileName].Add("File not exists");
                    else
                    {
                        if (fileStatus.IsReadOnly) fileTasks[fullFileName].Add("ReadOnly");
                        if (fileStatus.IsFileLockedReadAndWrite) fileTasks[fullFileName].Add("**Locked** for read and write");
                        else if (fileStatus.IsFileLockedForRead) fileTasks[fullFileName].Add("**Locked** for read");
                        if (fileStatus.IsInCloudOrVirtualOrOffline) fileTasks[fullFileName].Add("Is offline");
                    }
                }

                fileTasks[fullFileName].Add(task);
                if (modifiedDate != null)
                {
                    DateTime? lastWriteTime = null;
                    try
                    {
                        if (File.Exists(fullFileName)) lastWriteTime = FileHandler.GetLastWriteTime(fullFileName, waitAndRetry: false);
                    }
                    catch { }
                    fileTasks[fullFileName].Add("  " + modifiedDate.ToString() + " vs. " + (lastWriteTime == modifiedDate ? "" : (lastWriteTime == null ? "File not exists or can't read last access time" : lastWriteTime.ToString())));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void ActionSeeProcessQueue()
        {
            try
            {
                Dictionary<string, List<string>> fileTasks = new Dictionary<string, List<string>>();
                string messageBoxQueuesInfo = "List of all process queues...\r\n";

                try
                {
                    messageBoxQueuesInfo += string.Format("Files: {0} Selected {1}\r\n", imageListView1.Items.Count, imageListView1.SelectedItems.Count);
                    if (CommonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsCountDirty() > 0)
                        messageBoxQueuesInfo += string.Format("Lazy loading queue: {0}\r\n", CommonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsCountDirty());
                    if (!string.IsNullOrWhiteSpace(FileHandler.FileLockedByProcess))
                        messageBoxQueuesInfo += "**Locked file: " + FileHandler.FileLockedByProcess;
                }
                catch { }

                try
                {
                    if (countInvokeCalls > 0)
                        messageBoxQueuesInfo += string.Format("DataGridView: {0} invoke queue\r\n", countInvokeCalls);
                }
                catch { }

                try
                {
                    lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
                    {
                        foreach (FileEntry fileEntry in commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails)
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.LastWriteDateTime, "Lazy loading: read/check from all sources");
                    }
                }
                catch { }

                try
                {
                    lock (commonQueueLazyLoadingMediaThumbnailLock)
                    {
                        foreach (FileEntry fileEntry in commonQueueLazyLoadingMediaThumbnail)
                        {
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.LastWriteDateTime, "Lazy loading: Media Thumbnail");
                        }
                    }
                }
                catch { }

                try
                {
                    lock (commonLazyLoadingMapNomnatatimLock)
                    {
                        foreach (FileEntry fileEntry in commonLazyLoadingMapNomnatatim.Keys)
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.LastWriteDateTime, "Lazy loading: in queue, MapNomnatatim");
                    }
                }
                catch { }


                if (readToCacheQueues.Count > 0)
                {
                    try
                    {
                        lock (_readToCacheQueuesLock)
                        {
                            messageBoxQueuesInfo += "Read to cache:";
                            foreach (KeyValuePair<int, int> keyValuePair in readToCacheQueues) ProgressBackgroundStatusText += " " + keyValuePair.Value;
                            messageBoxQueuesInfo += "\r\n";
                        }
                    }
                    catch
                    {
                    }
                }

                if (deleteRecordQueues.Count > 0)
                {
                    try
                    {
                        lock (_deleteRecordQueuesLock)
                        {
                            messageBoxQueuesInfo += "Delete records:";
                            foreach (KeyValuePair<int, int> keyValuePair in deleteRecordQueues) messageBoxQueuesInfo += " " + keyValuePair.Value;
                            messageBoxQueuesInfo += "\r\n";
                        }
                    }
                    catch
                    {
                    }
                }

                messageBoxQueuesInfo += "\r\n";

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
                    lock (commonQueueReadMetadataFromSourceExiftoolLock)
                        foreach (FileEntry fileEntry in commonQueueReadMetadataFromSourceExiftool)
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.LastWriteDateTime, "Exiftool read: in queue, wait on turn");
                }
                catch { }

                try
                {
                    lock (exiftool_MediaFilesNotInDatabaseLock)
                    {
                        foreach (FileEntry fileEntry in exiftool_MediaFilesNotInDatabase)
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, null, "Exiftool read, in process");
                    }
                }
                catch { }

                try
                {
                    lock (FileHandler.CloundFileTouchedAndWhenLock)
                    {
                        foreach (KeyValuePair<string, DateTime> filenameKeyWhenValue in FileHandler.CloundFileTouchedAndWhen)
                            AddTaskToFileTasks(fileTasks, filenameKeyWhenValue.Key, null, "Downloading... Started: " + filenameKeyWhenValue.Value.ToString());
                    }
                }
                catch { }

                try
                {
                    lock (FileHandler.CloundFileTouchedFailedAndWhenLock)
                    {
                        foreach (KeyValuePair<string, DateTime> filenameKeyWhenValue in FileHandler.CloundFileTouchedFailedAndWhen)
                            AddTaskToFileTasks(fileTasks, filenameKeyWhenValue.Key, null, "Failed downloading, timeout at: " + filenameKeyWhenValue.Value.ToString());
                    }
                }
                catch { }

                try
                {
                    //lock ()
                    {
                        foreach (KeyValuePair<string, DateTime> filenameKeyWhenValue in FileHandler.CloundFileTouchedFailedAndWhen)
                            AddTaskToFileTasks(fileTasks, filenameKeyWhenValue.Key, null, "Downloading... Started: " + filenameKeyWhenValue.Value.ToString());
                    }
                }
                catch { }

                try
                {
                    lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock)
                    {
                        foreach (Metadata fileEntry in exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify)
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.FileDateModified, "In queue for: Will be verified after Exiftool readback");
                    }
                }
                catch { }

                try
                {
                    lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGallery)
                    {
                        foreach (FileEntry fileEntry in commonQueueReadMetadataFromSourceWindowsLivePhotoGallery)
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.LastWriteDateTime, "In queue for: Read meta information from Windows Live Photo Gallery");
                    }
                }
                catch { }

                try
                {
                    lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock)
                    {
                        foreach (FileEntry fileEntry in commonQueueReadMetadataFromSourceMicrosoftPhotos)
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.LastWriteDateTime, "In queue for: Read meta information from Microsoft Photos");
                    }
                }
                catch { }

                try
                {
                    lock (commonQueueSaveToDatabaseRegionAndThumbnailLock)
                    {
                        foreach (Metadata fileEntry in commonQueueSaveToDatabaseRegionAndThumbnail)
                            if (fileEntry.PersonalRegionList.Count > 0)
                                AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.FileDateModified, "In queue for: Create thumbnail for region: " + fileEntry.PersonalRegionList.Count.ToString());
                    }
                }
                catch { }

                try
                {
                    lock (exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUserLock)
                    {
                        foreach (Metadata fileEntry in exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser)
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.FileDateModified, "In queue for: Wait to be saved with " + fileEntry.PersonalRegionList.Count.ToString() + " regions");
                    }
                }
                catch { }

                try
                {
                    lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser)
                    {
                        foreach (Metadata fileEntry in exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser)
                            AddTaskToFileTasks(fileTasks, fileEntry.FileFullPath, fileEntry.FileDateModified, "In queue for: Saving bulk using exiftool with " + fileEntry.PersonalRegionList.Count.ToString() + " regions");
                    }
                }
                catch { }

                try
                {
                    lock (commonQueueRenameMediaFilesLock)
                    {
                        foreach (KeyValuePair<string, string> keyValuePair in commonQueueRenameMediaFiles)
                            AddTaskToFileTasks(fileTasks, keyValuePair.Key, null, "In queue for: Wait rename to " + keyValuePair.Value);
                    }
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
                    if (string.IsNullOrWhiteSpace(messageBoxQueuesInfo)) messageBoxQueuesInfo = "\r\nThe queue is empty.\r\nHere you will see all task in all queues\r\n";
                    if (formMessageBoxThread == null || formMessageBoxThread.IsDisposed) formMessageBoxThread = new FormMessageBox("Task list", messageBoxQueuesInfo);
                    else formMessageBoxThread.UpdateMessage(messageBoxQueuesInfo);
                    formMessageBoxThread.Owner = this;
                    formMessageBoxThread.Show();
                }
                catch { }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DisplayAllQueueStatus - Updated display
        private Stopwatch stopwatchLastDisplayed = new Stopwatch();
        private Stopwatch stopwatchLastDisplayedExiftoolWaitCloud = new Stopwatch();
        private void DisplayAllQueueStatus()
        {
            try
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




                //UpdateRibbonsWhenWorkspaceChanged();






                UpdateStatusImageListView(string.Format("Files: {0} Selected {1} ", imageListView1.Items.Count, imageListView1.SelectedItems.Count));

                string progressBackgroundStatusText = "";
                int threadQueuCount = 0;

                if (!string.IsNullOrWhiteSpace(FileHandler.FileLockedByProcess))
                {
                    threadQueuCount++;
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        "Locked file: " + Path.GetFileName(FileHandler.FileLockedByProcess);
                }

                if (GetFileEntriesRotateMediaCountDirty() > 0)
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Rotate: {0}", GetFileEntriesRotateMediaCountDirty());
                threadQueuCount += GetFileEntriesRotateMediaCountDirty();

                if (GlobalData.ProcessCounterReadProperties > 0)
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Properties: {0}", GlobalData.ProcessCounterReadProperties);
                threadQueuCount += GlobalData.ProcessCounterReadProperties;


                if (CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountDirty() > 0)
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Read WLPG: {0}", CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountDirty());
                threadQueuCount += CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountDirty();

                if (CommonQueueReadMetadataFromSourceMicrosoftPhotosCountDirty() > 0)
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Read MP: {0}", CommonQueueReadMetadataFromSourceMicrosoftPhotosCountDirty());
                threadQueuCount += CommonQueueReadMetadataFromSourceMicrosoftPhotosCountDirty();

                if (CommonQueueSaveToDatabaseMediaThumbnailCountDirty() > 0)
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Save Thumbnails: {0}", CommonQueueSaveToDatabaseMediaThumbnailCountDirty());
                threadQueuCount += CommonQueueSaveToDatabaseMediaThumbnailCountDirty();

                int regionCount = 0;
                try
                {
                    lock (commonQueueSaveToDatabaseRegionAndThumbnailLock) //CommonQueueReadPosterAndSaveFaceThumbnailsCountLock()
                    {
                        foreach (Metadata metadataRegionCount in commonQueueSaveToDatabaseRegionAndThumbnail) regionCount += metadataRegionCount.PersonalRegionList.Count;
                    }
                }
                catch { }

                if (regionCount > 0)
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Save Regions: {0}", regionCount);
                threadQueuCount += regionCount;

                if (CommonQueueReadMetadataFromSourceExiftoolCountDirty() + ExiftoolSave_MediaFilesNotInDatabaseCountDirty() > 0)
                {
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Exiftool: {0} in process: {1}", CommonQueueReadMetadataFromSourceExiftoolCountDirty(), ExiftoolSave_MediaFilesNotInDatabaseCountDirty());
                    threadQueuCount += CommonQueueReadMetadataFromSourceExiftoolCountDirty();
                    if (!stopwatchLastDisplayedExiftoolWaitCloud.IsRunning) stopwatchLastDisplayedExiftoolWaitCloud.Start();
                    if (stopwatchLastDisplayedExiftoolWaitCloud.ElapsedMilliseconds > 10000)
                    {
                        try
                        {
                            int countWaitFileInCloud = 0;
                            foreach (FileEntry fileEntry in exiftool_MediaFilesNotInDatabase)
                            {
                                FileStatus fileStatus = FileHandler.GetFileStatus(fileEntry.FileFullPath);
                                if (fileStatus.IsInCloudOrVirtualOrOffline) countWaitFileInCloud++;
                            }
                            if (countWaitFileInCloud > 0)
                                progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") + string.Format("wait cloud:{0} ", countWaitFileInCloud);
                        }
                        catch
                        {

                        }
                        stopwatchLastDisplayedExiftoolWaitCloud.Restart();
                    }
                }

                try
                {
                    lock (FileHandler.CloundFileTouchedAndWhenLock)
                    {
                        if (FileHandler.CloundFileTouchedAndWhen.Count > 0)
                            progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                                string.Format("In cloud: {0}", FileHandler.CloundFileTouchedAndWhen.Count);
                    }
                }
                catch { }

                try
                {
                    lock (FileHandler.CloundFileTouchedFailedAndWhenLock)
                    {
                        if (FileHandler.CloundFileTouchedFailedAndWhen.Count > 0)
                        {
                            int countTimeouts = 0;
                            foreach (DateTime dateTime in FileHandler.CloundFileTouchedFailedAndWhen.Values) if (DateTime.Now > dateTime) countTimeouts++;

                            if (countTimeouts > 0) progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") + string.Format("Timeouts: {0}", countTimeouts);
                        }
                    }
                }
                catch { }

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

                if (CommonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsCountDirty() > 0)
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Metadata: {0}", CommonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsCountDirty());
                threadQueuCount += CommonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsCountDirty();

                if (CommonQueueLazyLoadingThumbnailCountDirty() > 0)
                    progressBackgroundStatusText += (progressBackgroundStatusText == "" ? "" : " ") +
                        string.Format("Thumbnail: {0}", CommonQueueLazyLoadingThumbnailCountDirty());
                threadQueuCount += CommonQueueLazyLoadingThumbnailCountDirty();

                //int lasyLoadingDataGridViewCount = ThreadLazyLoadingDataGridViewQueueSizeDirty() + DataGridViewLazyLoadingCount();
                //if (lasyLoadingDataGridViewCount == 0) LazyLoadingDataGridViewProgressEndReached();

                try
                {
                    if (threadQueuCount == 0) progressBackgroundStatusText = "";
                    else progressBackgroundStatusText = "(" + threadQueuCount + ") " + progressBackgroundStatusText;
                    ProgressBackgroundStatusText = progressBackgroundStatusText;
                }
                catch
                {
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
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "DisplayAllQueueStatus");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DatabaseAndCacheMetadataExiftool_OnRecordReadToCache
        private Dictionary<int, int> readToCacheQueues = new Dictionary<int, int>();
        private static readonly Object _readToCacheQueuesLock = new Object();
        private void DatabaseAndCacheMetadataExiftool_OnRecordReadToCache(object sender, ReadToCacheFileEntriesRecordEventArgs e)
        {
            try
            {
                int queueLeft = e.FileEntries - (e.KeywordCount + e.MetadataCount + e.RegionCount);

                lock (_readToCacheQueuesLock)
                {
                    if (e.InitCounter && e.FileEntries > 0 && queueLeft > 0)
                    {
                        lock (_readToCacheQueuesLock) if (!readToCacheQueues.ContainsKey(e.HashQueue)) readToCacheQueues.Add(e.HashQueue, queueLeft);
                    }
                    else
                    {
                        lock (_readToCacheQueuesLock)
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
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DatabaseAndCacheMetadataExiftool_OnDeleteRecord
        private Dictionary<int, int> deleteRecordQueues = new Dictionary<int, int>();
        private static readonly Object _deleteRecordQueuesLock = new Object();
        private void DatabaseAndCacheMetadataExiftool_OnDeleteRecord(object sender, DeleteRecordEventArgs e)
        {
            try
            {
                int queueLeft = e.FileEntries - e.Count;
                lock (_deleteRecordQueuesLock)
                {
                    if (e.InitCounter && e.FileEntries > 0 && queueLeft > 0)
                    {

                        lock (_readToCacheQueuesLock) if (!readToCacheQueues.ContainsKey(e.HashQueue)) readToCacheQueues.Add(e.HashQueue, queueLeft);
                    }
                    else
                    {
                        lock (_readToCacheQueuesLock)
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
                }
                DisplayAllQueueStatus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            try
            {
                lock (_fileSaveSizeLock) fileSaveSizeWatcher.Clear();
                //timerShowExiftoolSaveProgress.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region UpdateExiftoolSaveStatus - AddWatcherShowExiftoolSaveProcessQueue
        private void AddWatcherShowExiftoolSaveProcessQueue(string fullFileName)
        {
            try
            {
                lock (_fileSaveSizeLock)
                {
                    if (!fileSaveSizeWatcher.ContainsKey(fullFileName)) fileSaveSizeWatcher.Add(fullFileName, 0);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region UpdateExiftoolSaveStatus - CountSaveQueue
        private int CountSaveQueueLock()
        {
            int countToSave = CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountLock();
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

        #region ProgressbarBackgroundProgress

        #region ProgressBackgroundStatusText
        private string ProgressBackgroundStatusText
        {
            get
            {
                return toolStripStatusLabelStatusProcess.Text;
            }
            set
            {
                kryptonRibbonGroupLabelToolsProgressBackgroundBackgroundProcessText.TextLine1 = value;
                toolStripStatusLabelStatusProcess.Text = value;
            }
        }
        #endregion

        #region ProgressbarBackgroundProgressQueueRemainding(int queueRemainding)
        private void ProgressbarBackgroundProgressQueueRemainding(int queueRemainding)
        {
            try
            {
                if (queueRemainding > progressBarBackground.Maximum) progressBarBackground.Maximum = queueRemainding;
                progressBarBackground.Value = progressBarBackground.Maximum - queueRemainding;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ProgressbarBackgroundProgress(bool enabled, int value, int minimum, int maximum)
        private void ProgressbarBackgroundProgress(bool enabled, int value, int minimum, int maximum)
        {
            try
            {
                progressBarBackground.Minimum = minimum;
                progressBarBackground.Maximum = maximum;
                progressBarBackground.Value = value;
                ProgressbarSaveProgress(enabled);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ProgressbarBackgroundProgress(bool enabled)
        private void ProgressbarBackgroundProgress(bool enabled)
        {
            this.kryptonRibbonGroupLabelToolsProgressBackground.Enabled = enabled;
            this.kryptonRibbonGroupCustomControlToolsProgressBackground.Enabled = enabled;
            this.kryptonRibbonGroupLabelToolsProgressBackgroundBackgroundProcessText.Enabled = enabled;
        }

        #endregion


        #endregion

        #region LazyLoadingDataGridViewProgress - ProgressCircle

        #region GetProgressCircle(int procentage)
        private Bitmap GetProgressCircle(int procentage, out int imageIndex)
        {
            imageIndex = 0;
            try
            {                
                if (procentage < 0)
                {
                    imageIndex = 0;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle00_16x16;
                }
                else if (procentage <= 6)
                {
                    imageIndex = 1;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle01_16x16;
                }
                else if (procentage <= 12)
                {
                    imageIndex = 2;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle02_16x16;
                }
                else if (procentage <= 18)
                {
                    imageIndex = 3;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle03_16x16;
                }
                else if (procentage <= 24)
                {
                    imageIndex = 4;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle04_16x16;
                }
                else if (procentage <= 29)
                {
                    imageIndex = 5;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle05_16x16;
                }
                else if (procentage <= 35)
                {
                    imageIndex = 6;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle06_16x16;
                }
                else if (procentage <= 41)
                {
                    imageIndex = 7;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle07_16x16;
                }
                else if (procentage <= 47)
                {
                    imageIndex = 8;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle08_16x16;
                }
                else if (procentage <= 53)
                {
                    imageIndex = 9;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle09_16x16;
                }
                else if (procentage <= 59)
                {
                    imageIndex = 10;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle10_16x16;
                }
                else if (procentage <= 65)
                {
                    imageIndex = 11;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle11_16x16;
                }
                else if (procentage <= 71)
                {
                    imageIndex = 12;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle12_16x16;
                }
                else if (procentage <= 76)
                {
                    imageIndex = 13;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle13_16x16;
                }
                else if (procentage <= 82)
                {
                    imageIndex = 14;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle14_16x16;
                }
                else if (procentage <= 88)
                {
                    imageIndex = 15;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle15_16x16;
                }
                else if (procentage <= 94)
                {
                    imageIndex = 16;
                    return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle16_16x16;
                }
                imageIndex = 17;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            return PhotoTagsSynchronizer.Properties.Resources.ProgressCircle17_16x16;
        }
        #endregion

        #region SetButtonSpecNavigator
        private Stopwatch stopwatchCircleProgressbar = new Stopwatch();
        private void SetButtonSpecNavigator(Krypton.Navigator.ButtonSpecNavigator buttonSpecNavigator, int value, int maximum)
        {
            try
            {
                if (GlobalData.IsApplicationClosing) return;
                if (InvokeRequired)
                {
                    this.BeginInvoke(new Action<Krypton.Navigator.ButtonSpecNavigator, int, int>(SetButtonSpecNavigator), buttonSpecNavigator, value, maximum);
                    return;
                }

                int procentage = 0;
                if (value >= maximum) procentage = 100;
                if (maximum == 0) procentage = -1;
                else procentage = (int)(((double)value / (double)maximum) * 100);

                buttonSpecNavigator.Image = GetProgressCircle(procentage, out int imageIndex);

                if (buttonSpecNavigator.Tag == null && !(buttonSpecNavigator.Tag is int)) buttonSpecNavigator.Tag = -1;

                if ((int)buttonSpecNavigator.Tag != imageIndex)
                {
                    if (!stopwatchCircleProgressbar.IsRunning || stopwatchCircleProgressbar.ElapsedMilliseconds > 700)
                    {
                        buttonSpecNavigator.Tag = imageIndex;
                        stopwatchCircleProgressbar.Restart();
                        kryptonPageToolboxTagsDetails.SuspendLayout();
                        //DataGridViewHandler.SuspendLayoutSetDelay(dataGridViewTagsAndKeywords, true);
                        kryptonWorkspaceCellToolbox.Refresh(); //Hack to get the circle to refresh
                                                               //DataGridViewHandler.SuspendLayoutSetDelay(dataGridViewTagsAndKeywords, true);
                        kryptonPageToolboxTagsDetails.ResumeLayout();
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

        #region LazyLoadingDataGridViewProgress - Update Status
        public void LazyLoadingDataGridViewProgressUpdateStatus(int queueRemainding)
        {
            try
            {
                if (queueRemainding > progressBarLazyLoading.Maximum) progressBarLazyLoading.Maximum = queueRemainding;
                if (queueRemainding == 0) progressBarLazyLoading.Maximum = 0;
                if (queueRemainding == -1)
                {
                    progressBarLazyLoading.Maximum = 0;
                    queueRemainding = 0;
                }
                progressBarLazyLoading.Value = progressBarLazyLoading.Maximum - queueRemainding;

                UpdateStatusActionDelayedRefresh("Loading DataGridView: " + queueRemainding + " / " + progressBarLazyLoading.Maximum);
                SetButtonSpecNavigator(buttonSpecNavigatorDataGridViewProgressCircle, progressBarLazyLoading.Value, progressBarLazyLoading.Maximum);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region LoadingItemsImageListView
        public void LoadingItemsImageListView(int value, int maximum)
        {
            try
            {
                SetButtonSpecNavigator(buttonSpecNavigatorImageListViewLoadStatus, maximum - value, maximum);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion


        //---- Tools Progress and Status 
        #region StatusActionText
        private string StatusActionText
        {
            get { return kryptonRibbonGroupLabelToolsCurrentActions.TextLine1; }
            set
            {
                kryptonRibbonGroupLabelToolsCurrentActions.TextLine1 = value;
                toolStripStatusLabelStatusAction.Text = value;
            }
        }
        #endregion

        #region ProgressbarSaveAndConvertProgress

        #region ProgressbarSaveAndConvertProgress(bool enabled, int value)
        private void ProgressbarSaveAndConvertProgress(bool enabled, int value)
        {
            try
            {
                progressBarSaveConvert.Value = value;
                ProgressbarSaveProgress(enabled);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ProgressbarSaveProgress(bool enabled, int value, int minimum, int maximum, string descrption)
        private void ProgressbarSaveAndConvertProgress(bool enabled, int value, int minimum, int maximum, string descrption)
        {
            try
            {
                progressBarSaveConvert.Minimum = minimum;
                progressBarSaveConvert.Maximum = maximum;
                progressBarSaveConvert.Value = value;
                ProgressbarSaveProgress(enabled);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ProgressbarSaveProgress(bool visible)
        private void ProgressbarSaveProgress(bool visible)
        {
            try
            {
                kryptonRibbonGroupTripleProgressStatusSave.Visible = visible;
                kryptonRibbonGroupCustomControlToolsProgressSave.Visible = visible;
                kryptonRibbonGroupLabelToolsProgressSave.Visible = visible;
                kryptonRibbonGroupLabelToolsProgressSaveText.Visible = visible;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region FilesCutCopyPasteDrag_OnFileSystemAction
        private void FilesCutCopyPasteDrag_OnFileSystemAction(object sender, FileSystemActionEventArgs e)
        {
            try 
            {
                UpdateStatusActionDelayedRefresh(e.Action + " Source:" + e.Source + (string.IsNullOrWhiteSpace(e.Destination) ? "" : " Destination:" + e.Destination));
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