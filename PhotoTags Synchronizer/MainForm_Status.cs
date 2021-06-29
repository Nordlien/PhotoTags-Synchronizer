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
        public void UpdateStatusAction(string text)
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

        private static FormMessageBox formMessageBoxThread = null;
       
        

        private void toolStripProgressBarThreadQueue_Click(object sender, EventArgs e)
        {
            string messageBoxQueuesInfo = "";
            /*
            messageBoxQueuesInfo += string.Format("Files: {0} Selected {1} ", imageListView1.Items.Count, imageListView1.SelectedItems.Count) + "\r\n";
            try
            {
                foreach (ImageListViewItem imageListViewItem in imageListView1.Items)
                    messageBoxQueuesInfo += "Selected: " + imageListViewItem.Selected.ToString() + " Name: " + imageListViewItem.FileFullPath + "\r\n";
            } catch { }
            */

            try
            {
                messageBoxQueuesInfo += "Last file Locked by process: " + Exiftool.ExiftoolWriter.FileLockedByProcess + "\r\n";
            }
            catch { }

            /*
            try
            {

                lock (fileEntriesRotateMediaLock)
                    foreach (FileEntry fileEntry in fileEntriesRotateMedia)
                        messageBoxQueuesInfo += "File wait to be rotated: " + fileEntry.FileFullPath + "\r\n";
            }
            catch { }
            */



            /*if (GlobalData.ProcessCounterDelete > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Delete: {0}", GlobalData.ProcessCounterDelete);
            threadQueuCount += GlobalData.ProcessCounterDelete;*/

            /*if (GlobalData.ProcessCounterRefresh > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Reload: {0}", GlobalData.ProcessCounterRefresh);
            threadQueuCount += GlobalData.ProcessCounterRefresh;*/


            try
            {

                lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) 
                foreach (FileEntry fileEntry in commonQueueReadMetadataFromWindowsLivePhotoGallery)
                        messageBoxQueuesInfo += "WLPG: " +
                            fileEntry.LastWriteDateTime.ToString() + " " +
                            (Exiftool.ExiftoolWriter.IsFileReadOnly(fileEntry.FileFullPath) == true ? "ReadOnly" : "Writeable") + " " +
                            (Exiftool.ExiftoolWriter.IsFileLockedByProcess(fileEntry.FileFullPath) == true ? "Locked" : "Open") + " " +
                            (Exiftool.ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath) == true ? "In cloud" : "Local") + " " +
                            (Exiftool.ExiftoolWriter.IsFileVirual(fileEntry.FileFullPath) == true ? "Virtual" : "Real") + " " +
                            fileEntry.FileFullPath + "\r\n";
            }
            catch { }

            try
            {

                lock (commonQueueReadMetadataFromMicrosoftPhotosLock)
                    foreach (FileEntry fileEntry in commonQueueReadMetadataFromMicrosoftPhotos)
                        messageBoxQueuesInfo += "MS Photos: " +
                            fileEntry.LastWriteDateTime.ToString() + " " +
                            (Exiftool.ExiftoolWriter.IsFileReadOnly(fileEntry.FileFullPath) == true ? "ReadOnly" : "Writeable") + " " +
                            (Exiftool.ExiftoolWriter.IsFileLockedByProcess(fileEntry.FileFullPath) == true ? "Locked" : "Open") + " " +
                            (Exiftool.ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath) == true ? "In cloud" : "Local") + " " +
                            (Exiftool.ExiftoolWriter.IsFileVirual(fileEntry.FileFullPath) == true ? "Virtual" : "Real") + " " +
                            fileEntry.FileFullPath + "\r\n";
            }
            catch { }

            try
            {

                lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                    foreach (Metadata fileEntry in commonQueueReadPosterAndSaveFaceThumbnails)
                        messageBoxQueuesInfo += "Thumbnails: " +
                            fileEntry.PersonalRegionList.Count.ToString() + " " +
                            fileEntry.FileDateModified.ToString() + " " +
                            (Exiftool.ExiftoolWriter.IsFileReadOnly(fileEntry.FileFullPath) == true ? "ReadOnly" : "Writeable") + " " +
                            (Exiftool.ExiftoolWriter.IsFileLockedByProcess(fileEntry.FileFullPath) == true ? "Locked" : "Open") + " " +
                            (Exiftool.ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath) == true ? "In cloud" : "Local") + " " +
                            (Exiftool.ExiftoolWriter.IsFileVirual(fileEntry.FileFullPath) == true ? "Virtual" : "Real") + " " +
                            fileEntry.FileFullPath + "\r\n";
            }
            catch { }

            try
            {

                lock (commonQueueReadMetadataFromExiftoolLock) 
                foreach (FileEntry fileEntry in commonQueueReadMetadataFromExiftool)
                        messageBoxQueuesInfo += "Wait verify: " +
                            fileEntry.LastWriteDateTime.ToString() + " " +
                            (Exiftool.ExiftoolWriter.IsFileReadOnly(fileEntry.FileFullPath) == true ? "ReadOnly" : "Writeable") + " " +
                            (Exiftool.ExiftoolWriter.IsFileLockedByProcess(fileEntry.FileFullPath) == true ? "Locked" : "Open") + " " +
                            (Exiftool.ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath) == true ? "In cloud" : "Local") + " " +
                            (Exiftool.ExiftoolWriter.IsFileVirual(fileEntry.FileFullPath) == true ? "Virtual" : "Real") + " " +
                            fileEntry.FileFullPath + "\r\n";
            }
            catch { }


            /*
            try
            {
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Read:");

                lock (_readToCacheQueuesLock)
                {
                    foreach (KeyValuePair<int, int> keyValuePair in readToCacheQueues)
                    {
                        toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                            //"#" + keyValuePair.Key.ToString() + " " +
                            keyValuePair.Value;
                        threadQueuCount += keyValuePair.Value;
                    }
                }
            }
            catch { }
            
            if (deleteRecordQueues.Count > 0)
            {
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + "Delete: ";
                try
                {
                    lock (_deleteRecordQueuesLock)
                    {
                        foreach (KeyValuePair<int, int> keyValuePair in deleteRecordQueues)
                        {
                            toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + keyValuePair.Value;
                            threadQueuCount += keyValuePair.Value;
                        }
                    }
                }
                catch
                {
                }
            }
            */

            
            try
            {

                lock (mediaFilesNotInDatabaseLock)
                    foreach (string fileEntry in mediaFilesNotInDatabase)
                        messageBoxQueuesInfo += "File not in database: " +
                            (Exiftool.ExiftoolWriter.IsFileReadOnly(fileEntry) == true ? "ReadOnly" : "Writeable") + " " +
                            (Exiftool.ExiftoolWriter.IsFileLockedByProcess(fileEntry) == true ? "Locked" : "Open") + " " +
                            (Exiftool.ExiftoolWriter.IsFileInCloud(fileEntry) == true ? "In cloud" : "Local") + " " +
                            (Exiftool.ExiftoolWriter.IsFileVirual(fileEntry) == true ? "Virtual" : "Real") + " " +
                            fileEntry + "\r\n";
            }
            catch { }

            try
            {
                lock (commonQueueSaveMetadataUpdatedByUserLock)
                    foreach (Metadata fileEntry in commonQueueSaveMetadataUpdatedByUser)
                        messageBoxQueuesInfo += "To be saved: " +
                            fileEntry.PersonalRegionList.Count.ToString() + " " +
                            fileEntry.FileDateModified.ToString() + " " +
                            (Exiftool.ExiftoolWriter.IsFileReadOnly(fileEntry.FileFullPath) == true ? "ReadOnly" : "Writeable") + " " +
                            (Exiftool.ExiftoolWriter.IsFileLockedByProcess(fileEntry.FileFullPath) == true ? "Locked" : "Open") + " " +
                            (Exiftool.ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath) == true ? "In cloud" : "Local") + " " +
                            (Exiftool.ExiftoolWriter.IsFileVirual(fileEntry.FileFullPath) == true ? "Virtual" : "Real") + " " +
                            fileEntry.FileFullPath + "\r\n";
            }
            catch { }

            try
            {
                lock (commonQueueSubsetMetadataToSaveLock)
                    foreach (Metadata fileEntry in commonQueueSubsetMetadataToSave)
                        messageBoxQueuesInfo += "Saving: " +
                            fileEntry.PersonalRegionList.Count.ToString() + " " +
                            fileEntry.FileDateModified.ToString() + " " +
                            (Exiftool.ExiftoolWriter.IsFileReadOnly(fileEntry.FileFullPath) == true ? "ReadOnly" : "Writeable") + " " +
                            (Exiftool.ExiftoolWriter.IsFileLockedByProcess(fileEntry.FileFullPath) == true ? "Locked" : "Open") + " " +
                            (Exiftool.ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath) == true ? "In cloud" : "Local") + " " +
                            (Exiftool.ExiftoolWriter.IsFileVirual(fileEntry.FileFullPath) == true ? "Virtual" : "Real") + " " +
                            fileEntry.FileFullPath + "\r\n";
            }
            catch { }

            try
            {

                lock (commonQueueMetadataWrittenByExiftoolReadyToVerifyLock)
                    foreach (Metadata fileEntry in commonQueueMetadataWrittenByExiftoolReadyToVerify)
                        messageBoxQueuesInfo += "To be verified: " +
                            fileEntry.PersonalRegionList.Count.ToString() + " " +
                            fileEntry.FileDateModified.ToString() + " " +
                            (Exiftool.ExiftoolWriter.IsFileReadOnly(fileEntry.FileFullPath) == true ? "ReadOnly" : "Writeable") + " " +
                            (Exiftool.ExiftoolWriter.IsFileLockedByProcess(fileEntry.FileFullPath) == true ? "Locked" : "Open") + " " +
                            (Exiftool.ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath) == true ? "In cloud" : "Local") + " " +
                            (Exiftool.ExiftoolWriter.IsFileVirual(fileEntry.FileFullPath) == true ? "Virtual" : "Real") + " " +
                            fileEntry.FileFullPath + "\r\n";
            }
            catch { }

            //lock (commonQueueRenameLock) return commonQueueRename.Count;
            try
            {

                lock (commonQueueRenameLock)
                    foreach (KeyValuePair<string, string> keyValuePair in commonQueueRename)
                        messageBoxQueuesInfo += "To be renamed: " +
                            keyValuePair.Key + " " +
                            keyValuePair.Value + "\r\n";
            }
            catch { }

            /*
            if (CommonQueueLazyLoadingMetadataCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Metadata: {0}", CommonQueueLazyLoadingMetadataCountDirty());
            threadQueuCount += CommonQueueLazyLoadingMetadataCountDirty();

            if (CommonQueueLazyLoadingThumbnailCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Thumbnail: {0}", CommonQueueLazyLoadingThumbnailCountDirty());
            threadQueuCount += CommonQueueLazyLoadingThumbnailCountDirty();
            */

            if (formMessageBoxThread == null || formMessageBoxThread.IsDisposed) formMessageBoxThread = new FormMessageBox(messageBoxQueuesInfo);
            //else formMessageBoxWarnings.AppendMessage(errors);
            formMessageBoxThread.Owner = this;
            formMessageBoxThread.Show();
        }

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
            
            int threadQueuCount = 0;

            if (!string.IsNullOrWhiteSpace(Exiftool.ExiftoolWriter.FileLockedByProcess)) {
                threadQueuCount++;
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    "Locked file: " + Path.GetFileName(Exiftool.ExiftoolWriter.FileLockedByProcess);
            }

            if (GetFileEntriesRotateMediaCountDirty() > 0) 
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + 
                    string.Format("Rotate: {0}", GetFileEntriesRotateMediaCountDirty());
            threadQueuCount += GetFileEntriesRotateMediaCountDirty();

            if (GlobalData.ProcessCounterDelete > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + 
                    string.Format("Delete: {0}", GlobalData.ProcessCounterDelete);
            threadQueuCount += GlobalData.ProcessCounterDelete;

            if (GlobalData.ProcessCounterRefresh > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + 
                    string.Format("Reload: {0}", GlobalData.ProcessCounterRefresh);
            threadQueuCount += GlobalData.ProcessCounterRefresh;

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
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Save Regions: {0}", regionCount); 
            threadQueuCount += regionCount;

            if (CommonQueueReadMetadataFromExiftoolCountDirty() > 0)
            {
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("ExifCheck:{0}", CommonQueueReadMetadataFromExiftoolCountDirty());
                threadQueuCount += CommonQueueReadMetadataFromExiftoolCountDirty();
            }

            if (readToCacheQueues.Count > 0)
            {
                try
                {
                    toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                        string.Format("Read:");

                    lock (_readToCacheQueuesLock)
                    {
                        foreach (KeyValuePair<int, int> keyValuePair in readToCacheQueues)
                        {
                            toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
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
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + "Delete: ";
                try
                {
                    lock (_deleteRecordQueuesLock)
                    {
                        foreach (KeyValuePair<int, int> keyValuePair in deleteRecordQueues)
                        {
                            toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") + keyValuePair.Value;
                            threadQueuCount += keyValuePair.Value;
                        }
                    }
                }
                catch
                {
                }
            }


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
            
            if (CommonQueueLazyLoadingMetadataCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Metadata: {0}", CommonQueueLazyLoadingMetadataCountDirty());
            threadQueuCount += CommonQueueLazyLoadingMetadataCountDirty();

            if (CommonQueueLazyLoadingThumbnailCountDirty() > 0)
                toolStripStatusThreadQueueCount.Text += (toolStripStatusThreadQueueCount.Text == "" ? "" : " ") +
                    string.Format("Thumbnail: {0}", CommonQueueLazyLoadingThumbnailCountDirty());
            threadQueuCount += CommonQueueLazyLoadingThumbnailCountDirty();

            int lasyLoadingDataGridViewCount = ThreadLazyLoadingDataGridViewQueueSizeDirty();
            if (lasyLoadingDataGridViewCount == 0) 
                LazyLoadingDataGridViewProgressEndReached();

            toolStripStatusThreadQueueCount.Text = "(" + threadQueuCount + ") " + toolStripStatusThreadQueueCount.Text;

            #region Updated progressbar
            try
            {
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
            } catch (Exception ex)
            {
                Logger.Error(ex.Message);
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

        private Stopwatch stopwatchhDelayShowLazyLoadingDataGridViewProgressbar = new Stopwatch();

        #region LazyLoadingDataGridViewProgress - End Reached
        public void LazyLoadingDataGridViewProgressEndReached()
        {
            timerLazyLoadingDataGridViewProgressRemoveProgessbar.Interval = 1000;
            timerLazyLoadingDataGridViewProgressRemoveProgessbar.Start();
            LazyLoadingDataGridViewProgressUpdateStatus(0);
        }
        #endregion

        #region LazyLoadingDataGridViewProgress - Update Status
        public void LazyLoadingDataGridViewProgressUpdateStatus(int queueSize)
        {
            if (queueSize > toolStripProgressBarLazyLoadingDataGridViewProgress.Maximum) toolStripProgressBarLazyLoadingDataGridViewProgress.Maximum = queueSize;
            int queueCount = toolStripProgressBarLazyLoadingDataGridViewProgress.Maximum - queueSize;
            toolStripProgressBarLazyLoadingDataGridViewProgress.Value = queueCount;

            if (queueSize > 1)
            {
                if (!toolStripProgressBarLazyLoadingDataGridViewProgress.Visible) //Delayed visible
                {
                    if (stopwatchhDelayShowLazyLoadingDataGridViewProgressbar == null)
                    {
                        stopwatchhDelayShowLazyLoadingDataGridViewProgressbar = new Stopwatch();
                        stopwatchhDelayShowLazyLoadingDataGridViewProgressbar.Restart();
                    }

                    if (!stopwatchhDelayShowLazyLoadingDataGridViewProgressbar.IsRunning) stopwatchhDelayShowLazyLoadingDataGridViewProgressbar.Restart();

                    if (stopwatchhDelayShowLazyLoadingDataGridViewProgressbar.IsRunning && stopwatchhDelayShowLazyLoadingDataGridViewProgressbar.ElapsedMilliseconds > 600)
                    {
                        toolStripLabelLazyLoadingDataGridViewProgress.Visible = true;
                        toolStripProgressBarLazyLoadingDataGridViewProgress.Visible = true;
                        
                    }
                }
            } 
            else
            {
                toolStripProgressBarLazyLoadingDataGridViewProgress.Visible = false;
                toolStripLabelLazyLoadingDataGridViewProgress.Visible = false;
                toolStripProgressBarLazyLoadingDataGridViewProgress.Value = 0;
                toolStripProgressBarLazyLoadingDataGridViewProgress.Maximum = 1;

            }
        }
        
        #endregion
        
        #endregion

    }
}