using DataGridViewGeneric;
using Exiftool;
using FileDateTime;
using FileHandeling;
using Krypton.Toolkit;
using Manina.Windows.Forms;
using MetadataLibrary;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Thumbnails;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        private AutoResetEvent WaitExittoolReadCacheThread = null;
        private AutoResetEvent WaitThumbnailReadCacheThread = null;

        private static ThreadPriority threadPriority = ThreadPriority.Lowest;

        #region Thread variables

        #region LazyLoading
        private static readonly Object _ThreadLazyLoadingMetadataFolderLock = new Object();
        private static Thread _ThreadLazyLoadingMetadataFolder = null; //

        private static readonly Object _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnailsLock = new Object();
        private static Thread _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails = null; //

        private static readonly Object _ThreadLazyLoadingMediaThumbnailLock = new Object();
        private static Thread _ThreadLazyLoadingMediaThumbnail = null; //
        #endregion 

        #region Read From Source
        private static readonly Object _ThreadReadMetadataFromSourceExiftoolLock = new Object();
        private static Thread _ThreadReadMetadataFromSourceExiftool = null;

        private static readonly Object _ThreadReadFromSourceMicrosoftPhotosLock = new Object();
        private static Thread _ThreadReadFromSourceMicrosoftPhotos = null; //

        private static readonly Object _ThreadReadFromSourceWindowsLiveGalleryLock = new Object();
        private static Thread _ThreadReadFromSourceWindowsLiveGallery = null; //
        #endregion

        #region Save to Database
        private static readonly Object _ThreadSaveUsingExiftoolToMediaLock = new Object();
        private static Thread _ThreadSaveUsingExiftoolToMedia = null; //

        private static readonly Object _ThreadSaveToDatabaseMediaThumbnailLock = new Object();
        private static Thread _ThreadSaveToDatabaseMediaThumbnail = null; //

        private static readonly Object _ThreadSaveToDatabaseRegionAndThumbnailLock = new Object();
        private static Thread _ThreadSaveToDatabaseRegionAndThumbnail = null; //

        private static readonly Object _ThreadRenameMediaFilesLock = new Object();
        private static Thread _ThreadRenameMediaFiles = null; //
        #endregion

        #endregion

        #region Process Queues

        #region LazyLoading
        private static List<FileEntryAttribute> commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails = new List<FileEntryAttribute>();
        private static readonly Object commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock = new Object();

        private static List<FileEntryAttribute> commonQueueLazyLoadingMediaThumbnail = new List<FileEntryAttribute>();
        private static readonly Object commonQueueLazyLoadingThumbnailLock = new Object();
        #endregion

        #region Read from source
        //Exiftool
        private static List<FileEntry> commonQueueReadMetadataFromSourceExiftool = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromSourceExiftoolLock = new Object();

        //Microsoft Photos
        private static List<FileEntry> commonQueueReadMetadataFromSourceMicrosoftPhotos = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromSourceMicrosoftPhotosLock = new Object();

        //Windows Live Photo Gallery
        private static List<FileEntry> commonQueueReadMetadataFromSourceWindowsLivePhotoGallery = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock = new Object();
        #endregion

        #region Save to database
        //Region "Face" thumbnails
        private static List<Metadata>  commonQueueSaveToDatabaseRegionAndThumbnail = new List<Metadata>();
        private static readonly Object commonQueueSaveToDatabaseRegionAndThumbnailLock = new Object();

        //Thumbnail
        private static List<FileEntryImage> commonQueueSaveToDatabaseMediaThumbnail = new List<FileEntryImage>();
        private static readonly Object      commonQueueSaveToDatabaseMediaThumbnailLock = new Object();

        //Rename
        private static Dictionary<string, string> commonQueueRenameMediaFiles = new Dictionary<string, string>();
        private static readonly Object            commonQueueRenameMediaFilesLock = new Object();
        #endregion

        #region Exiftool Save
        private static List<Metadata> exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUser = new List<Metadata>();
        private static readonly Object exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUserLock = new Object();

        private static List<Metadata> exiftoolSave_OrigialMetadataBeforeUserUpdate = new List<Metadata>();
        private static readonly Object exiftoolSave_OrigialMetadataBeforeUserUpdateLock = new Object();

        private static List<Metadata> exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify = new List<Metadata>();
        private static readonly Object exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock = new Object();

        private static List<FileEntry> exiftoolSave_MediaFilesNotInDatabase = new List<FileEntry>(); //It's globale, just to manage to show count status
        private static readonly Object exiftoolSave_MediaFilesNotInDatabaseLock = new Object();

        private static List<Metadata> exiftoolSave_QueueSubsetMetadataToSave = new List<Metadata>();
        private static readonly Object exiftoolSave_QueueSubsetMetadataToSaveLock = new Object();
        #endregion

        #endregion

        #region Error handling
        private static Dictionary<string, string> queueErrorQueue = new Dictionary<string, string>();
        private static readonly Object queueErrorQueueLock = new Object();
        #endregion

        #region Get Count of items in Queue with Lock
        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueLazyLoadingAllSourcesAllMetadataAndThumbnailCountLock()
        {
            lock   (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock) return commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Count;
        }

        private int CommonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsCountDirty()
        {
            lock   (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock) return commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueLazyLoadingThumbnailCountLock()
        {
            lock   (commonQueueLazyLoadingThumbnailLock) return commonQueueLazyLoadingMediaThumbnail.Count;
        }

        private int CommonQueueLazyLoadingThumbnailCountDirty()
        {
            lock   (commonQueueLazyLoadingThumbnailLock) return commonQueueLazyLoadingMediaThumbnail.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueSaveToDatabaseRegionAndThumbnailCountLock()
        {
            lock   (commonQueueSaveToDatabaseRegionAndThumbnailLock) return commonQueueSaveToDatabaseRegionAndThumbnail.Count;
        }

        private int CommonQueueSaveToDatabaseRegionAndThumbnailCountDirty()
        {
            return  commonQueueSaveToDatabaseRegionAndThumbnail.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueSaveToDatabaseMediaThumbnailCountLock()
        {
            lock   (commonQueueSaveToDatabaseMediaThumbnailLock) return commonQueueSaveToDatabaseMediaThumbnail.Count;
        }

        private int CommonQueueSaveToDatabaseMediaThumbnailCountDirty()
        {
            return  commonQueueSaveToDatabaseMediaThumbnail.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueReadMetadataFromMicrosoftPhotosCountLock()
        {
            lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) return commonQueueReadMetadataFromSourceMicrosoftPhotos.Count;
        }

        private int CommonQueueReadMetadataFromSourceMicrosoftPhotosCountDirty()
        {
            return  commonQueueReadMetadataFromSourceMicrosoftPhotos.Count;
        }

        private int CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountLock()
        {
            lock   (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) return commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Count;
        }

        private int CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountDirty()
        {
            return  commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueReadMetadataFromSourceExiftoolCountLock()
        {
            lock   (commonQueueReadMetadataFromSourceExiftoolLock) return commonQueueReadMetadataFromSourceExiftool.Count;
        }

        private int CommonQueueReadMetadataFromSourceExiftoolCountDirty()
        {
            return  commonQueueReadMetadataFromSourceExiftool.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountLock()
        {
            lock   (exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUserLock) return exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUser.Count;
        }

        private int CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountDirty()
        {
            return exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUser.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int ExiftoolSave_MediaFilesNotInDatabaseCountLock()
        {
            lock   (exiftoolSave_MediaFilesNotInDatabaseLock) return exiftoolSave_MediaFilesNotInDatabase.Count;
        }

        private int ExiftoolSave_MediaFilesNotInDatabaseCountDirty()
        {
            return exiftoolSave_MediaFilesNotInDatabase.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        
        private int CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty()
        {
            return exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueRenameCountLock()
        {
            lock (commonQueueRenameMediaFilesLock) return commonQueueRenameMediaFiles.Count;
        }

        private int CommonQueueRenameCountDirty()
        {
            return commonQueueRenameMediaFiles.Count;
        }
        #endregion

        #region Start Thread, IsAnyThreadRunning, Tell when all queues are empty
        /// <summary>
        /// Start all background thread every x ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerStartThread_Tick(object sender, EventArgs e)
        {
            if (!GlobalData.IsApplicationClosing)
            {
                StartThreads();
                TriggerAutoResetEventQueueEmpty();
            }
        }

        /// <summary>
        /// Start all the threas when items in queue
        /// </summary>
        private void StartThreads()
        {
            threadPriority = (ThreadPriority)Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity;
            ThreadReadFromSourceMetadataExiftool();            //Read from cache first, then exifdata, 
            ThreadReadFromSourceMetadataMicrosoftPhotos();
            ThreadReadFromSourceMetadataWindowsLiveGallery();
            ThreadSaveUsingExiftoolToMedia();
            ThreadSaveToDatabaseMediaThumbnail();
            ThreadSaveToDatabaseRegionAndThumbnail();

            ThreadRenameMediaFiles();
            ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails();
            ThreadLazyLoadingMediaThumbnail();
        }

        /// <summary>
        /// Check if any thread is running
        /// </summary>
        /// <returns>True if a thread is processing</returns>
        private bool IsAnyThreadRunning()
        {
            return (
                (_ThreadSaveToDatabaseMediaThumbnail != null && _ThreadSaveToDatabaseMediaThumbnail.IsAlive) ||
                (_ThreadReadMetadataFromSourceExiftool != null && _ThreadReadMetadataFromSourceExiftool.IsAlive) ||
                (_ThreadReadFromSourceWindowsLiveGallery != null && _ThreadReadFromSourceWindowsLiveGallery.IsAlive) ||
                (_ThreadReadFromSourceMicrosoftPhotos != null && _ThreadReadFromSourceMicrosoftPhotos.IsAlive) ||
                (_ThreadSaveToDatabaseRegionAndThumbnail != null && _ThreadSaveToDatabaseRegionAndThumbnail.IsAlive) ||
                (_ThreadSaveUsingExiftoolToMedia != null && _ThreadSaveUsingExiftoolToMedia.IsAlive)
                );
        }

        /// <summary>
        /// Check if any thread is running Except ThreadThumbnailRegion
        /// This is used by ThreadThumbnailRegion wait until free resources to use
        /// </summary>
        /// <returns>True if a thread is processing (Except ThreadThumbnailRegion)</returns>
        private bool IsThreadRunningExcept_ThreadSaveToDatabaseRegionAndThumbnail()
        {
            return
                CommonQueueSaveToDatabaseMediaThumbnailCountDirty() > 0 ||
                CommonQueueReadMetadataFromSourceExiftoolCountDirty() > 0 ||
                CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountDirty() > 0 ||
                CommonQueueReadMetadataFromSourceMicrosoftPhotosCountDirty() > 0 ||
                //queueThumbnailRegion.Count > 0 ||
                CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountDirty() > 0;
        }

        /// <summary>
        /// On 32bit version, out of meomory occures oftem, was needed to wait until qeue was ended to free up reasources
        /// </summary>
        private void TriggerAutoResetEventQueueEmpty()
        {
            if (CommonQueueSaveToDatabaseMediaThumbnailCountDirty() == 0 &&
                CommonQueueReadMetadataFromSourceExiftoolCountDirty() == 0 &&
                CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountDirty() == 0 &&
                CommonQueueReadMetadataFromSourceMicrosoftPhotosCountDirty() == 0 &&
                CommonQueueSaveToDatabaseRegionAndThumbnailCountDirty() == 0 &&
                CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountDirty() == 0
                )
            {
                //When out of memory, then wait for cache to empty
                if (ReadImageOutOfMemoryWillWaitCacheEmpty != null) ReadImageOutOfMemoryWillWaitCacheEmpty.Set();
            }
        }
        #endregion

        #region Preloadning

        #region ClearQueue - Exiftool
        public void ClearAllQueues()
        {
            GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = true;
            GlobalData.IsStopAndEmptyThumbnailQueueRequest = true;

            MetadataDatabaseCache.StopCaching = true;
            ThumbnailDatabaseCache.StopCaching = true;

            lock (commonQueueReadMetadataFromSourceExiftoolLock) commonQueueReadMetadataFromSourceExiftool.Clear();
            lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) commonQueueReadMetadataFromSourceMicrosoftPhotos.Clear();
            lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Clear();
            //lock (commonQueueSaveThumbnailToDatabaseLock) commonQueueSaveThumbnailToDatabase.Clear();

            WaitExittoolReadCacheThread = new AutoResetEvent(false);
            WaitThumbnailReadCacheThread = new AutoResetEvent(false);

            StartThreads();

            WaitExittoolReadCacheThread.WaitOne(10000);
            WaitExittoolReadCacheThread = null;

            WaitThumbnailReadCacheThread.WaitOne(10000);
            WaitThumbnailReadCacheThread = null;

            MetadataDatabaseCache.StopCaching = false;
            ThumbnailDatabaseCache.StopCaching = false;

            GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = false;
            GlobalData.IsStopAndEmptyThumbnailQueueRequest = false;

        }
        #endregion

        #region Preloading - Metadata - Thread - Faster Sqlite read for a list of files
        /// <summary>
        /// Faster read of metadata and put into the cache
        /// </summary>
        /// <param name="fileEntries">List of FileEntires to put in cache</param>
        public void PreloadCacheFileEntries(HashSet<FileEntry> fileEntries, string selectedFolder)
        {            
            try
            {
                bool isThreadRunning;
                int retry = 200;
                do
                {
                    lock (_ThreadLazyLoadingMetadataFolderLock) isThreadRunning = (_ThreadLazyLoadingMetadataFolder != null);
                    if (isThreadRunning)
                    {
                        MetadataDatabaseCache.StopCaching = true;
                        ThumbnailDatabaseCache.StopCaching = true;
                        Task.Delay(10).Wait(); //Wait thread stopping
                        Logger.Debug("CacheFileEntries - sleep(100) - ThreadRunning is running");
                    }
                } while (isThreadRunning && retry-- > 0);

                lock (_ThreadLazyLoadingMetadataFolderLock) isThreadRunning = (_ThreadLazyLoadingMetadataFolder != null);

                if (isThreadRunning) return; //Still running, give up

                lock (_ThreadLazyLoadingMetadataFolderLock)
                {
                    _ThreadLazyLoadingMetadataFolder = new Thread(() =>
                    {
                        #region
                        try
                        {
                            if (selectedFolder != null)
                            {
                                if (cacheFolderThumbnails) databaseAndCacheThumbnail.ReadToCacheFolder(selectedFolder);
                                if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas(selectedFolder, MetadataBrokerType.ExifTool);
                                if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas(selectedFolder, MetadataBrokerType.WindowsLivePhotoGallery);
                                if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas(selectedFolder, MetadataBrokerType.MicrosoftPhotos);
                                if (cacheFolderWebScraperDataSets) databaseAndCacheMetadataExiftool.ReadToCacheWebScraperDataSet(fileEntries); //Don't have folder
                            }
                            else
                            {
                                if (cacheFolderThumbnails) databaseAndCacheThumbnail.ReadToCache(fileEntries); //Read missing, new media files added
                                if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCache(fileEntries, MetadataBrokerType.ExifTool); //Read missing, new media files added
                                if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCache(fileEntries, MetadataBrokerType.WindowsLivePhotoGallery); //Read missing, new media files added
                                if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCache(fileEntries, MetadataBrokerType.MicrosoftPhotos); //Read missing, new media files added
                                if (cacheFolderWebScraperDataSets) databaseAndCacheMetadataExiftool.ReadToCacheWebScraperDataSet(fileEntries); //Read missing, new media files added
                            }
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("CacheFileEntries crashed.", "CacheFileEntries failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            Logger.Error(ex, "CacheFileEntries crashed");
                        }
                        finally
                        {
                            MetadataDatabaseCache.StopCaching = false;
                            ThumbnailDatabaseCache.StopCaching = false;
                            lock (_ThreadLazyLoadingMetadataFolderLock) _ThreadLazyLoadingMetadataFolder = null;
                        }
                        #endregion
                    });
                }

                lock (_ThreadLazyLoadingMetadataFolderLock) if (_ThreadLazyLoadingMetadataFolder != null)
                {
                    _ThreadLazyLoadingMetadataFolder.Priority = threadPriority;
                    _ThreadLazyLoadingMetadataFolder.Start();                    
                }
                else Logger.Error("_ThreadCacheSelectedFastRead was not able to start");
                

            }
            catch
            {
                //Retry after crash, eg. thread creation failed
                MetadataDatabaseCache.StopCaching = false;
                ThumbnailDatabaseCache.StopCaching = false;
            }
        }
        #endregion 

        #endregion

        #region DataGridView - GetDataGridViewWatingToBePopulatedCount
        public int GetDataGridView_ColumnsEntriesInReadQueues_Count()
        {
            DataGridView dataGridView = GetAnyAgregatedDataGridView();
            int queueCount = 0;
            if (dataGridView != null)
            {
                for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
                {
                    if (!DataGridViewHandler.IsColumnPopulated(dataGridView, columnIndex))
                    {
                        queueCount++;
                    }
                    else
                    {
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                        if (dataGridViewGenericColumn != null)
                        {
                            if (IsFilesInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftoolLock(dataGridViewGenericColumn.FileEntryAttribute)) queueCount++;
                        }
                    }
                }
            }
            return queueCount;
        }
        #endregion

        #region LazyLoading - All Sources - AddQueue - (Read order: Cache, Database, Source)
        //Caller: DataGridView_AfterPopulateSelectedFiles_LazyLoadFromDatabaseThenSourceAllVersions
        //Caller: imageListView1_RetrieveItemMetadataDetails
        //Caller: ThreadSaveUsingExiftoolToMedia
        public void AddQueueReadFromSourceIfMissing_AllSoruces(FileEntry fileEntry)
        {
            //When file is DELETE, LastWriteDateTime become null
            List<FileEntryAttribute> fileEntryAttributes = new List<FileEntryAttribute>();
            fileEntryAttributes.Add(new FileEntryAttribute(fileEntry, FileEntryVersion.CurrentVersionInDatabase));
            AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttributes);

            TriggerAutoResetEventQueueEmpty();
        }

        //public void AddQueueReadFromSourceIfMissing_AllSoruces(FileEntry fileEntry)
        //{
        //    //When file is DELETE, LastWriteDateTime become null
        //    if (fileEntry.LastWriteDateTime != null)
        //    {
        //        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
        //        if (metadata == null) 
        //            AddQueueReadFromSourceExiftoolLock(fileEntry); //If Metadata don't exist in database, put it in read queue
        //        if (!databaseAndCacheMetadataMicrosoftPhotos.IsMetadataInCache(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos))) 
        //            AddQueueReadFromSourceMetadataMicrosoftPhotosLock(fileEntry);
        //        if (!databaseAndCacheMetadataWindowsLivePhotoGallery.IsMetadataInCache(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery))) 
        //            AddQueueReadFromSourceWindowsLivePhotoGalleryLock(fileEntry);
        //    }

        //    TriggerAutoResetEventQueueEmpty();
        //}
        #endregion

        #region LazyLoading - Cache and Database - All type of Metadata And Region Thumbnails - Add Queue - (** Doesn't add Read from Source **)
        //Caller: DataGridView_AfterPopulateSelectedFiles_LazyLoadFromDatabaseThenSourceAllVersions
        //Caller: DataGridView_Populate_SelectedItemsInvoke
        //Caller: AutoCorrectRunDataGridView_Click
        //Caller: AutoCorrectFormDataGridView_Click
        public void AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(List<FileEntryAttribute> fileEntryAttributes)
        {
            if (fileEntryAttributes == null) return;
            lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
            {
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes)
                {
                    if (!commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Contains(fileEntryAttribute)) commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Add(fileEntryAttribute);
                }
            }
        }
        #endregion

        #region LazyLoading - Cache and Database - All type of Metadata And Region Thumbnails - Thread - **Populate** - (** Doesn't add Read from Source **)
        public void ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails()
        {
            try
            {
                lock (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnailsLock) if (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails != null || CommonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsCountDirty() <= 0) return;
                lock (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnailsLock)
                {
                    _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails = new Thread(() =>
                    {
                        #region
                        try
                        {
                            Logger.Trace("ThreadLazyLoadningMetadata - started");
                            int count = CommonQueueLazyLoadingAllSourcesAllMetadataAndThumbnailCountLock();
                            while (count > 0 && CommonQueueLazyLoadingAllSourcesAllMetadataAndThumbnailCountLock() > 0 && !GlobalData.IsApplicationClosing)
                            {
                                count--;
                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails[0]);

                                bool readColumn = false;
                                switch (fileEntryAttribute.FileEntryVersion)
                                {
                                    case FileEntryVersion.AutoCorrect:
                                    case FileEntryVersion.ExtractedNowFromMediaFile:
                                    case FileEntryVersion.CurrentVersionInDatabase:
                                        readColumn = true;
                                        break;
                                    case FileEntryVersion.Error:
                                        if ((showWhatColumns & ShowWhatColumns.ErrorColumns) > 0) readColumn = true;
                                        break;
                                    case FileEntryVersion.Historical:
                                        if ((showWhatColumns & ShowWhatColumns.HistoryColumns) > 0) readColumn = true;
                                        break;
                                    
                                    default:
                                        throw new Exception("Not implemeneted");
                                }

                                bool isMetadataFound_ThenPopulateTheFoundData = false;
                                bool populateDataGrid = true;
                                bool populateImageListViewItemThumbnail = false;
                                bool populateImageListViewItemMetadata = false;

                                if (readColumn)
                                {
                                    MetadataBrokerType metadataBrokerType = MetadataBrokerType.ExifTool;
                                    

                                    //If error Broker type attribute, set correct Broker type
                                    if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.Error) 
                                        metadataBrokerType |= MetadataBrokerType.ExifToolWriteError;

                                    FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileEntryAttribute, metadataBrokerType);
                                    if (!databaseAndCacheMetadataExiftool.IsMetadataInCache(fileEntryBrokerExiftool))
                                    {
                                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
                                        if (metadata != null)
                                        {
                                            //Check if Region Thumnbail missing, if yes, then create
                                            if (metadata.PersonalRegionIsThumbnailMissing()) AddQueueSaveToDatabaseRegionAndThumbnailLock(metadata);

                                            isMetadataFound_ThenPopulateTheFoundData = true;
                                            populateDataGrid = true;
                                            //populateImageListViewItemThumbnail = true;
                                            populateImageListViewItemMetadata = true;
                                        } else AddQueueReadFromSourceExiftoolLock(fileEntryAttribute); //Didn't exists in Database, need read from source
                                    }

                                    FileEntryBroker fileEntryBrokerMicrosoftPhotos = new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.MicrosoftPhotos);
                                    if (!databaseAndCacheMetadataMicrosoftPhotos.IsMetadataInCache(fileEntryBrokerMicrosoftPhotos))
                                    {   
                                        Metadata metadata = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(fileEntryBrokerMicrosoftPhotos);
                                        if (metadata != null)
                                        {
                                            //Check if Region Thumnbail missing, if yes, then create
                                            if (metadata.PersonalRegionIsThumbnailMissing()) AddQueueSaveToDatabaseRegionAndThumbnailLock(metadata);

                                            isMetadataFound_ThenPopulateTheFoundData = true;
                                            populateDataGrid = true;
                                            //populateImageListViewItemThumbnail = true;
                                            //populateImageListViewItemMetadata = true;
                                        } else AddQueueReadFromSourceMetadataMicrosoftPhotosLock(fileEntryAttribute); //Didn't exists in Database, need read from source
                                    }

                                    FileEntryBroker fileEntryBrokerWindowsLivePhotoGallery = new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.WindowsLivePhotoGallery);
                                    if (!databaseAndCacheMetadataWindowsLivePhotoGallery.IsMetadataInCache(fileEntryBrokerWindowsLivePhotoGallery))
                                    {
                                        Metadata metadata = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(fileEntryBrokerWindowsLivePhotoGallery);                                        
                                        if (metadata != null)
                                        {
                                            //Check if Region Thumnbail missing, if yes, then create
                                            if (metadata.PersonalRegionIsThumbnailMissing()) AddQueueSaveToDatabaseRegionAndThumbnailLock(metadata);

                                            isMetadataFound_ThenPopulateTheFoundData = true;
                                            populateDataGrid = true;
                                            //populateImageListViewItemThumbnail = true;
                                            //populateImageListViewItemMetadata = true;
                                        } 
                                        else AddQueueReadFromSourceWindowsLivePhotoGalleryLock(fileEntryAttribute); //Didn't exists in Database, need read from source
                                    }

                                    //Metadata folder will change to common folder name "WebScraper"
                                    FileEntryBroker fileEntryBrokerWebScraper = new FileEntryBroker(MetadataDatabaseCache.WebScapingFolderName,
                                            fileEntryAttribute.FileName, (DateTime)fileEntryAttribute.LastWriteDateTime, MetadataBrokerType.WebScraping);

                                    if (!databaseAndCacheMetadataExiftool.IsMetadataInCache(fileEntryBrokerWebScraper))
                                    {
                                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadWebScraperMetadataFromCacheOrDatabase(fileEntryBrokerWebScraper);
                                        
                                        if (metadata != null && metadata.PersonalRegionIsThumbnailMissing())
                                        {
                                            Metadata metadataWithFullPath = new Metadata(metadata);
                                            metadataWithFullPath.FileName = fileEntryAttribute.FileName;
                                            metadataWithFullPath.FileDirectory = fileEntryAttribute.Directory;
                                            metadataWithFullPath.FileDateModified = fileEntryAttribute.LastWriteDateTime;
                                            AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataWithFullPath);
                                        }

                                        if (metadata != null)
                                        {
                                            isMetadataFound_ThenPopulateTheFoundData = true;
                                            populateDataGrid = true;
                                            //populateImageListViewItemThumbnail = true;
                                            //populateImageListViewItemMetadata = true;
                                        }
                                        //else - no need read from source, this is not sutible for WebScraping
                                    }

                                    //if (isMetadataFound_ThenPopulateTheFoundData) 
                                    DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(fileEntryAttribute,
                                        populateDataGrid: populateDataGrid,
                                        populateImageListViewItemThumbnail: populateImageListViewItemThumbnail,
                                        populateImageListViewItemMetadata: populateImageListViewItemMetadata);
                                }

                                

                                lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock) commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.RemoveAt(0);

                                if (GlobalData.IsApplicationClosing) lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock) commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Clear();
                                TriggerAutoResetEventQueueEmpty();
                            }
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("LazyLoadingMetadata crashed. The 'LazyLoadingMetadata' was cleared.", "Saving Region Thumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock) commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Clear();  //Avoid loop, due to unknown error
                            Logger.Error(ex, "ThreadLazyLoadningMetadata");
                        }
                        finally
                        {
                            lock (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnailsLock) _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails = null;
                            Logger.Trace("ThreadLazyLoadningMetadata - ended");
                        }
                        #endregion
                    });

                    lock (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnailsLock) if (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails != null)
                    {
                        _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails.Priority = threadPriority;
                        _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails.Start();                        
                    }
                    else Logger.Error("_ThreadLazyLoadingMetadata was not able to start");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ThreadLazyLoadningMetadata.Start failed.");
                //_ThreadLazyLoadingMetadata = null;
            }
        }
        #endregion

        #region Media Thumbnail - LazyLoading 

        #region Media Thumbnail - LazyLoading - Add Read Queue - (Read order: Cache, Database, Source)
        public void AddQueueLazyLoadningMediaThumbnailLock(List<FileEntryAttribute> fileEntryAttributes)
        {
            if (fileEntryAttributes == null) return;
            lock (commonQueueLazyLoadingThumbnailLock)
            {
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes)
                {
                    if (!commonQueueLazyLoadingMediaThumbnail.Contains(fileEntryAttribute)) commonQueueLazyLoadingMediaThumbnail.Add(fileEntryAttribute);
                }
            }
        }
        #endregion

        #region Media Thumbnail - LazyLoading - Add Read Queue - (Read order: Cache, Database, Source)
        public void AddQueueLazyLoadningMediaThumbnailLock(FileEntryAttribute fileEntryAttribute)
        {
            if (fileEntryAttribute == null) return;
            lock (commonQueueLazyLoadingThumbnailLock)
            {
                if (!commonQueueLazyLoadingMediaThumbnail.Contains(fileEntryAttribute)) commonQueueLazyLoadingMediaThumbnail.Add(fileEntryAttribute);                
            }
        }
        #endregion

        #region Media Thumbnail - LazyLoading - Thread - (Read order: Cache, Database, Source)
        public void ThreadLazyLoadingMediaThumbnail()
        {
            try
            {
                if (WaitThumbnailReadCacheThread != null && CommonQueueLazyLoadingThumbnailCountDirty() == 0) WaitThumbnailReadCacheThread.Set();

                lock (_ThreadLazyLoadingMediaThumbnailLock)
                    if (GlobalData.IsStopAndEmptyThumbnailQueueRequest || _ThreadLazyLoadingMediaThumbnail != null || CommonQueueLazyLoadingThumbnailCountDirty() <= 0) return;

                lock (_ThreadLazyLoadingMediaThumbnailLock)
                {
                    _ThreadLazyLoadingMediaThumbnail = new Thread(() =>
                    {
                        #region
                        try
                        {
                            int count = CommonQueueLazyLoadingThumbnailCountLock();
                            while (count > 0 && CommonQueueLazyLoadingThumbnailCountLock() > 0 && !GlobalData.IsStopAndEmptyThumbnailQueueRequest && !GlobalData.IsApplicationClosing)
                            {
                                count--;

                                if (!GlobalData.IsStopAndEmptyThumbnailQueueRequest && commonQueueLazyLoadingMediaThumbnail.Count > 0) //In case clear, due to user screen interaction
                                {
                                    FileEntryAttribute fileEntryAttribute;
                                    lock (commonQueueLazyLoadingThumbnailLock) fileEntryAttribute = commonQueueLazyLoadingMediaThumbnail[0];

                                    if (!databaseAndCacheThumbnail.DoesThumbnailExistInCache(fileEntryAttribute))
                                    {
                                        bool isFileInCloud = FileHandler.IsFileInCloud(fileEntryAttribute.FileFullPath);
                                        bool dontReadFileFromCloud = Properties.Settings.Default.AvoidOfflineMediaFiles;
                                        Image thumbnail = GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(fileEntryAttribute, dontReadFileFromCloud, isFileInCloud);                                       
                                    }
                                }
                                lock (commonQueueLazyLoadingThumbnailLock) commonQueueLazyLoadingMediaThumbnail.RemoveAt(0);

                                if (GlobalData.IsApplicationClosing || GlobalData.IsStopAndEmptyThumbnailQueueRequest)
                                    lock (commonQueueLazyLoadingThumbnailLock) commonQueueLazyLoadingMediaThumbnail.Clear();
                            }
                            if (WaitThumbnailReadCacheThread != null) WaitThumbnailReadCacheThread.Set();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("LazyLoadningThumbnail crashed.", "LazyLoadningThumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (commonQueueLazyLoadingThumbnailLock) commonQueueLazyLoadingMediaThumbnail.Clear(); //Avoid loop, due to unknown error
                            Logger.Error(ex, "ThreadLazyLoadningThumbnail thread failed. ");
                        }
                        finally
                        {
                            lock (_ThreadLazyLoadingMediaThumbnailLock) _ThreadLazyLoadingMediaThumbnail = null;
                        }
                        #endregion
                    });

                }

                lock (_ThreadLazyLoadingMediaThumbnailLock) if (_ThreadLazyLoadingMediaThumbnail != null)
                {
                    _ThreadLazyLoadingMediaThumbnail.Priority = threadPriority;
                    _ThreadLazyLoadingMediaThumbnail.Start();                    
                }
                else Logger.Error("_ThreadLazyLoadingThumbnail was not able to start");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ThreadLazyLoadningThumbnail.Start failed. ");
            }
        }
        #endregion

        #region Media Thumbnail - SaveToDatabase - Add Save Queue
        public void AddQueueSaveToDatabaseMediaThumbnailLock(FileEntryImage fileEntryImage)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueSaveToDatabaseMediaThumbnailLock)
            {
                if (!commonQueueSaveToDatabaseMediaThumbnail.Contains(fileEntryImage))
                {
                    commonQueueSaveToDatabaseMediaThumbnail.Add(fileEntryImage);
                }
                else if (fileEntryImage.Image != null) //If image are already read, save it
                {
                    int index = commonQueueSaveToDatabaseMediaThumbnail.IndexOf(fileEntryImage);
                    if (index >= 0) commonQueueSaveToDatabaseMediaThumbnail[index].Image = fileEntryImage.Image; //Image has been found, updated the entry, so image will not be needed to load again.
                }
            }
        }
        #endregion

        #region Media Thumbnail - SaveToDatabase - Thread 
        public void ThreadSaveToDatabaseMediaThumbnail()
        {
            try
            {
                lock (_ThreadSaveToDatabaseMediaThumbnailLock) if (_ThreadSaveToDatabaseMediaThumbnail != null || CommonQueueSaveToDatabaseMediaThumbnailCountDirty() <= 0) return;

                lock (_ThreadSaveToDatabaseMediaThumbnailLock)
                {
                    _ThreadSaveToDatabaseMediaThumbnail = new Thread(() =>
                    {
                        #region While data in thread
                        try
                        {
                            Logger.Trace("ThreadSaveThumbnail - started");
                            int count = CommonQueueSaveToDatabaseMediaThumbnailCountLock();
                            while (count > 0 && CommonQueueSaveToDatabaseMediaThumbnailCountLock() > 0 && !GlobalData.IsApplicationClosing) //In case some more added to the queue or App will close
                            {
                                count--;

                                if (CommonQueueReadMetadataFromSourceExiftoolCountLock() > 0) break; //Wait all metadata readfirst
                                if (CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountLock() > 0) break; //Write first, read later on...

                                try
                                {
                                    FileEntryImage fileEntryImage;
                                    lock (commonQueueSaveToDatabaseMediaThumbnailLock)
                                    {
                                        fileEntryImage = new FileEntryImage(commonQueueSaveToDatabaseMediaThumbnail[0]);
                                    }

                                    bool wasThumnbailEmptyAndReloaded = false;
                                    try
                                    {
                                        if (fileEntryImage.Image == null)
                                        {
                                            fileEntryImage.Image = LoadMediaCoverArtThumbnail(fileEntryImage.FileFullPath, ThumbnailSaveSize, false);
                                            if (fileEntryImage.Image != null) wasThumnbailEmptyAndReloaded = true;                                            
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadSaveThumbnail - LoadMediaCoverArtThumbnail failed");
                                    }

                                    try
                                    {
                                        if (fileEntryImage.Image != null)
                                        {
                                            databaseAndCacheThumbnail.TransactionBeginBatch();
                                            databaseAndCacheThumbnail.WriteThumbnail(fileEntryImage, fileEntryImage.Image);
                                            databaseAndCacheThumbnail.TransactionCommitBatch();

                                            if (wasThumnbailEmptyAndReloaded)
                                            {
                                                DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute(new FileEntryAttribute(fileEntryImage, FileEntryVersion.ExtractedNowFromMediaFile), fileEntryImage.Image);
                                                DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute(new FileEntryAttribute(fileEntryImage, FileEntryVersion.Error), fileEntryImage.Image);
                                                ImageListViewReloadThumbnailAndMetadataInvoke(imageListView1, fileEntryImage.FileFullPath);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadSaveThumbnail - WriteThumbnail failed");
                                    }
                                    

                                    lock (commonQueueSaveToDatabaseMediaThumbnailLock)
                                    {
                                        if (commonQueueSaveToDatabaseMediaThumbnail.Count > 0) commonQueueSaveToDatabaseMediaThumbnail.RemoveAt(0);
                                    }

                                    if (GlobalData.IsApplicationClosing) lock (commonQueueSaveToDatabaseMediaThumbnailLock) commonQueueSaveToDatabaseMediaThumbnail.Clear();
                                }
                                catch (Exception ex)
                                {
                                    KryptonMessageBox.Show("Saving ThreadSaveThumbnail crashed. The ThreadSaveThumbnail queue was cleared. Nothing was saved.", "Saving ThreadSaveThumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                    lock (commonQueueSaveToDatabaseMediaThumbnailLock) { commonQueueSaveToDatabaseMediaThumbnail.Clear(); } //Avoid loop, due to unknown error
                                    Logger.Error(ex, "ThreadSaveThumbnail");
                                }
                            }

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, "ThreadSaveThumbnail");
                        }
                        finally
                        {
                            lock (_ThreadSaveToDatabaseMediaThumbnailLock) _ThreadSaveToDatabaseMediaThumbnail = null;
                            Logger.Trace("ThreadSaveThumbnail - ended");
                        }
                        #endregion
                    });

                    lock (_ThreadSaveToDatabaseMediaThumbnailLock) if (_ThreadSaveToDatabaseMediaThumbnail != null)
                    {
                        _ThreadSaveToDatabaseMediaThumbnail.Priority = threadPriority;
                        _ThreadSaveToDatabaseMediaThumbnail.Start();                        
                    }
                    else Logger.Error("_ThreadSaveThumbnail was not able to start");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "_ThreadThumbnailMedia failed to start.");
                //_ThreadSaveThumbnail = null;
            }
        }
        #endregion

        #endregion

        #region Exiftool 

        #region Exiftool - ReadFromSource Metadata Exiftool - AddQueue - (Read order: Cache, Database, Source)
        /// <summary>
        /// Add File Entry to Read "Metadata Queue"
        /// Inside Queue -> 
        ///     1. "Extract Exiftool data" ->> Store in database
        ///     2. Updated DataGridView to show new/updated Metadata
        ///     3. Add Metadata to Region Queue
        ///     -- Region Queue: Read Media Poster -> Extract Region Thmbnail
        /// </summary>
        /// <param name="fileEntry"></param>
        public void AddQueueReadFromSourceExiftoolLock(FileEntry fileEntry)
        {
            lock (commonQueueReadMetadataFromSourceExiftoolLock)
            {
                //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
                //bool existInQueue = ExistInExiftoolReadQueue(fileEntry);
                //if (!existInQueue)
                bool existInQueue = IsFolderInQueueReadAndSaveMetadataFromSourceExiftoolLock(fileEntry.FileFullPath);
                //if (ExistInExiftoolWriteQueue(fileEntry)) existInQueue = true;

                if (!existInQueue) commonQueueReadMetadataFromSourceExiftool.Add(fileEntry);
            }
            RemoveError(fileEntry.FileFullPath);

        }
        #endregion

        #region Exiftool - ReadFromSource Metadata Exiftool - Thread - **Populate** - (Read order: Cache, Database, Source)
        public void ThreadReadFromSourceMetadataExiftool()
        {
            try
            {
                if (WaitExittoolReadCacheThread != null && CommonQueueReadMetadataFromSourceExiftoolCountDirty() <= 0) WaitExittoolReadCacheThread.Set();
                if (GlobalData.IsStopAndEmptyExiftoolReadQueueRequest || _ThreadReadMetadataFromSourceExiftool != null || CommonQueueReadMetadataFromSourceExiftoolCountDirty() <= 0) return;
                if (ExiftoolSave_MediaFilesNotInDatabaseCountLock() > 0) return; //Don't start double read of exiftool

                lock (_ThreadReadMetadataFromSourceExiftoolLock)
                {
                    _ThreadReadMetadataFromSourceExiftool = new Thread(() =>
                    {
                        #region
                        try
                        {
                            Logger.Trace("ThreadCollectMetadataExiftool - started");

                            bool showCliWindow = Properties.Settings.Default.ApplicationDebugExiftoolReadShowCliWindow;
                            ProcessPriorityClass processPriorityClass = (ProcessPriorityClass)Properties.Settings.Default.ApplicationDebugExiftoolReadThreadPrioity;

                            int count = CommonQueueReadMetadataFromSourceExiftoolCountLock();
                            while (count > 0 && !GlobalData.IsStopAndEmptyExiftoolReadQueueRequest && CommonQueueReadMetadataFromSourceExiftoolCountLock() > 0 && !GlobalData.IsApplicationClosing) //In case some more added to the queue
                            {
                                count--;
                                if (CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountLock() > 0) break; //Write first, read later on...

                                int rangeToRemove; //Remember how many in queue now
                                List<FileEntry> mediaFilesNotInDatabase_NeedCheckInCloud = new List<FileEntry>();
                                List<FileEntry> mediaFilesReadFromDatabase_NeedUpdated_DataGridView_ImageList = new List<FileEntry>();

                                #region From the Read Queue - Find files not alread in database
                                lock (commonQueueReadMetadataFromSourceExiftoolLock)
                                {
                                    rangeToRemove = commonQueueReadMetadataFromSourceExiftool.Count;

                                    databaseAndCacheMetadataExiftool.CheckListOfFileEntriesWhatInCachedAndInDatabase(MetadataBrokerType.ExifTool,
                                        commonQueueReadMetadataFromSourceExiftool.GetRange(0, rangeToRemove),
                                        out List<FileEntry> mediaFilesWasNotInCache,
                                        out mediaFilesNotInDatabase_NeedCheckInCloud,
                                        out mediaFilesReadFromDatabase_NeedUpdated_DataGridView_ImageList);
                                    
                                    try
                                    {
                                        commonQueueReadMetadataFromSourceExiftool.RemoveRange(0, rangeToRemove);
                                    } catch (Exception ex)
                                    {
                                        commonQueueReadMetadataFromSourceExiftool.Clear();
                                        Logger.Error(ex);
                                    }
                                }
                                #endregion

                                foreach (FileEntry fileEntryReadFromDatabase in mediaFilesReadFromDatabase_NeedUpdated_DataGridView_ImageList)
                                {
                                    DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(
                                        new FileEntryAttribute(fileEntryReadFromDatabase, FileEntryVersion.ExtractedNowFromMediaFile),
                                        populateDataGrid: true, populateImageListViewItemThumbnail: false, populateImageListViewItemMetadata: true);
                                }

                                #region Check if need avoid files in cloud, if yes, don't read files in cloud
                                if (Properties.Settings.Default.AvoidReadExifFromCloud)
                                {
                                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase_NeedCheckInCloud)
                                    {
                                        FileStatus fileStatus = FileHandler.GetFileStatus(fileEntry.FileFullPath);

                                        if (!fileStatus.FileExists)
                                        {
                                            fileStatus.FileProcessStatus = FileProcessStatus.FileInaccessible;
                                            ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                                        }
                                        else if (fileStatus.FileExists && !fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            //File existin and local, process with file
                                            fileStatus.FileProcessStatus = FileProcessStatus.ExiftoolProcessing;
                                            ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                                            exiftoolSave_MediaFilesNotInDatabase.Add(fileEntry); 
                                        }
                                        else if (fileStatus.FileExists && fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            //File exist, and offline, don't read
                                            fileStatus.FileProcessStatus = FileProcessStatus.ExiftoolWillNotProcessingFileInCloud;
                                            ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                                            //When in cloud, and can't read, also need to populate dataGridView but will become with empty rows in column
                                            DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(
                                                new FileEntryAttribute(fileEntry, FileEntryVersion.ExtractedNowFromMediaFile),
                                                populateDataGrid: true, populateImageListViewItemThumbnail: false, populateImageListViewItemMetadata: true);
                                        }
                                        ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                                    }
                                }
                                else
                                {
                                    
                                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase_NeedCheckInCloud)
                                    {
                                        FileStatus fileStatus = FileHandler.GetFileStatus(fileEntry.FileFullPath);

                                        if (!fileStatus.FileExists) 
                                        {
                                            fileStatus.FileProcessStatus = FileProcessStatus.FileInaccessible;
                                            ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                                        }
                                        else if (fileStatus.FileExists && !fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            //File existin and local, process with file
                                            fileStatus.FileProcessStatus = FileProcessStatus.InExiftoolReadQueue;
                                            ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                                            exiftoolSave_MediaFilesNotInDatabase.Add(fileEntry); //File in not in clode, process with file
                                        }
                                        else if (fileStatus.FileExists && fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            //Get download process to start
                                            FileHandler.TouchOfflineFileToGetFileOnline(fileEntry.FileFullPath);
                                            fileStatus.FileProcessStatus = FileProcessStatus.WaitOfflineBecomeLocal;
                                            ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);

                                            //Add last in queue and wait for become downloaded
                                            AddQueueReadFromSourceExiftoolLock(fileEntry);
                                        }
                                    }
                                    //exiftoolSave_MediaFilesNotInDatabase.AddRange(mediaFilesNotInDatabase_NeedCheckInCloud);
                                }
                                #endregion

                                //DataGridView_ImageListView_Populate_FileEntryAttributeInvoke
                                //DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute(file);


                                while (!GlobalData.IsStopAndEmptyExiftoolReadQueueRequest && ExiftoolSave_MediaFilesNotInDatabaseCountLock() > 0 && !GlobalData.IsApplicationClosing)
                                {
                                    #region Create a subset of files to Read using Exiftool command line with parameters, and remove subset from queue
                                    int range = 0;
                                    //On computers running Microsoft Windows XP or later, the maximum length of the string that you can 
                                    //use at the command prompt is 8191 characters. On computers running Microsoft Windows 2000 or 
                                    //Windows NT 4.0, the maximum length of the string that you can use at the command prompt is 2047 
                                    //characters.
                                    bool useArgFile = true;

                                    int argumnetLength = 80; //Init command length;
                                    int maxParameterCommandLength = 2047;
                                    if (useArgFile) maxParameterCommandLength = 50000;
                                    List<String> useExiftoolOnThisSubsetOfFiles;

                                    lock (exiftoolSave_MediaFilesNotInDatabaseLock)
                                    {
                                        while (argumnetLength < maxParameterCommandLength && range < exiftoolSave_MediaFilesNotInDatabase.Count)
                                        {
                                            argumnetLength += exiftoolSave_MediaFilesNotInDatabase[range].FileFullPath.Length + 3; //+3 = space and 2x"
                                            range++;
                                        }

                                        if (argumnetLength > maxParameterCommandLength) range--;
                                        useExiftoolOnThisSubsetOfFiles = new List<string>();
                                        if (range > 100) 
                                            range = 100;
                                        for (int index = 0; index < range; index++) useExiftoolOnThisSubsetOfFiles.Add(exiftoolSave_MediaFilesNotInDatabase[index].FileFullPath);
                                    }
                                    #endregion

                                    #region Read using Exiftool
                                    string lastKnownExiftoolError = "";
                                    List<Metadata> metadataReadbackExiftoolAfterSaved = new List<Metadata>();
                                    try
                                    {
                                        if (argumnetLength < maxParameterCommandLength) useArgFile = false;
                                        exiftoolReader.Read(MetadataBrokerType.ExifTool, useExiftoolOnThisSubsetOfFiles, out metadataReadbackExiftoolAfterSaved, useArgFile, showCliWindow, processPriorityClass);
                                    }
                                    catch (Exception ex)
                                    {
                                        lastKnownExiftoolError = ex.Message;
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - Running Exiftool failed.");
                                    }
                                    #endregion

                                    #region Check if all files are read by Exiftool
                                    try
                                    {
                                        string filesNotRead = "";
                                        foreach (string fullFilePath in useExiftoolOnThisSubsetOfFiles)
                                        {
                                            
                                            if (!Metadata.IsFullFilenameInList(metadataReadbackExiftoolAfterSaved, fullFilePath))
                                            {
                                                //Media file was not read an error message estimate reason
                                                string errorMesssage = lastKnownExiftoolError;

                                                FileStatus fileStatus = FileHandler.GetFileStatus(fullFilePath, FileProcessStatus.FileInaccessible, 
                                                    checkLockedStatus: true, checkLockStatusTimeout: FileHandler.GetFileLockedStatusTimeout);

                                                if (!fileStatus.FileExists) errorMesssage += (errorMesssage == "" ? "" : "\r\n") + "File doesn't exist. ";
                                                else
                                                {
                                                    if (fileStatus.HasAnyLocks) errorMesssage += (errorMesssage == "" ? "" : "\r\n") + "File is Locked. ";
                                                    if (fileStatus.IsInCloudOrVirtualOrOffline) errorMesssage += (errorMesssage == "" ? "" : "\r\n") + "File is offline. ";
                                                }
                                                errorMesssage += (errorMesssage == "" ? "" : "\r\n") + lastKnownExiftoolError;

                                                AddError(Path.GetDirectoryName(fullFilePath), Path.GetFileName(fullFilePath), DateTime.Now,
                                                    AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                                    AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                                    "Exiftool failed reading data from file, got an error: " + errorMesssage + ": " + fullFilePath, false);
                                                filesNotRead += (filesNotRead == "" ? "" : ";") + filesNotRead;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(filesNotRead)) Logger.Error("Exiftool failed read all files. Files not read: " + filesNotRead);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - Check if all files are read by Exiftool failed.");
                                    }
                                    #endregion

                                    #region Verify readback after saved. (Saved data is in "to be verified" queue)
                                    try
                                    {

                                        foreach (Metadata metadataRead in metadataReadbackExiftoolAfterSaved)
                                        {
                                            lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock)
                                            {
                                                if (ExiftoolWriter.HasWriteMetadataErrors(metadataRead, exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify,
                                                    out Metadata metadataUpdatedByUserCopy, out string writeErrorDesciption))
                                                {
                                                    AddError(metadataUpdatedByUserCopy.FileEntryBroker.Directory, metadataUpdatedByUserCopy.FileEntryBroker.FileName, metadataUpdatedByUserCopy.FileEntryBroker.LastWriteDateTime,
                                                        AddErrorExiftooRegion, AddErrorExiftooCommandVerify, AddErrorExiftooParameterVerify, AddErrorExiftooParameterVerify, writeErrorDesciption);

                                                    Metadata metadataError = new Metadata(metadataUpdatedByUserCopy);
                                                    metadataError.FileDateModified = DateTime.Now;
                                                    metadataError.Broker |= MetadataBrokerType.ExifToolWriteError;
                                                    databaseAndCacheMetadataExiftool.TransactionBeginBatch();
                                                    databaseAndCacheMetadataExiftool.Write(metadataError);
                                                    databaseAndCacheMetadataExiftool.TransactionCommitBatch();

                                                    bool isFileInCloud = FileHandler.IsFileInCloud(metadataError.FileFullPath);
                                                    bool dontReadFileFromCloud = Properties.Settings.Default.AvoidOfflineMediaFiles;
                                                    GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(metadataError.FileEntryBroker, true, false);
                                                    DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(
                                                        new FileEntryAttribute(metadataError.FileFullPath, (DateTime)metadataError.FileDateModified, FileEntryVersion.Error), 
                                                        populateDataGrid: true, populateImageListViewItemThumbnail: false, populateImageListViewItemMetadata: true);
                                                }

                                                //Data was read, (even with errors), need to updated datagrid
                                                AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataRead);
                                                ImageListViewHandler.SetItemDirty(imageListView1, metadataRead.FileFullPath);
                                                DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(
                                                    new FileEntryAttribute(metadataRead.FileFullPath, (DateTime)metadataRead.FileDateModified, FileEntryVersion.ExtractedNowFromMediaFile), 
                                                    populateDataGrid: true, populateImageListViewItemThumbnail: false, populateImageListViewItemMetadata: true);
                                                DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(
                                                    new FileEntryAttribute(metadataRead.FileFullPath, (DateTime)metadataRead.FileDateModified, FileEntryVersion.Historical), 
                                                    populateDataGrid: true, populateImageListViewItemThumbnail: false, populateImageListViewItemMetadata: true);
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - Verify readback after saved failed.");
                                    }
                                    #endregion

                                    #region Write dummy record for when Exiftool failed due to corrupt files
                                    try
                                    {
                                        if (!string.IsNullOrWhiteSpace(lastKnownExiftoolError)) //Exiftool crached, don't read again
                                        {
                                            lock (exiftoolSave_MediaFilesNotInDatabaseLock)
                                            {
                                                for (int index = 0; index < range; index++)
                                                {
                                                    if (!Metadata.IsFullFilenameInList(metadataReadbackExiftoolAfterSaved, exiftoolSave_MediaFilesNotInDatabase[index].FileFullPath))
                                                    {
                                                        try
                                                        {
                                                            Metadata metadataDummy = new Metadata(MetadataBrokerType.ExifTool);
                                                            metadataDummy.FileDateModified = exiftoolSave_MediaFilesNotInDatabase[index].LastWriteDateTime;
                                                            metadataDummy.FileName = exiftoolSave_MediaFilesNotInDatabase[index].FileName;
                                                            metadataDummy.FileDirectory = exiftoolSave_MediaFilesNotInDatabase[index].Directory;
                                                            metadataDummy.FileMimeType = FormDatabaseCleaner.CorruptFile; //Also used
                                                            metadataDummy.PersonalComments = "Exiftool failed";
                                                            metadataDummy.FileDateCreated = exiftoolSave_MediaFilesNotInDatabase[index].LastWriteDateTime;
                                                            metadataDummy.FileDateAccessed = DateTime.Now;
                                                            metadataDummy.FileSize = 0;
                                                            try
                                                            {
                                                                if (File.Exists(exiftoolSave_MediaFilesNotInDatabase[index].FileFullPath))
                                                                {
                                                                    FileInfo fileInfo = new FileInfo(exiftoolSave_MediaFilesNotInDatabase[index].FileFullPath);
                                                                    metadataDummy.FileDateCreated = fileInfo.CreationTime;
                                                                    metadataDummy.FileDateAccessed = fileInfo.LastAccessTime;
                                                                    metadataDummy.FileSize = fileInfo.Length;
                                                                } else
                                                                {
                                                                    metadataDummy.PersonalComments = "File doesn't exist - Exiftool failed";
                                                                }
                                                            } catch { }
                                                            databaseAndCacheMetadataExiftool.TransactionBeginBatch();
                                                            databaseAndCacheMetadataExiftool.Write(metadataDummy);
                                                            databaseAndCacheMetadataExiftool.TransactionCommitBatch();

                                                            Metadata metadataError = new Metadata(metadataDummy);
                                                            metadataError.FileDateModified = DateTime.Now;
                                                            metadataError.Broker |= MetadataBrokerType.ExifToolWriteError;
                                                            databaseAndCacheMetadataExiftool.TransactionBeginBatch();
                                                            databaseAndCacheMetadataExiftool.Write(metadataError);
                                                            databaseAndCacheMetadataExiftool.TransactionCommitBatch();

                                                            DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(
                                                                new FileEntryAttribute(metadataError.FileFullPath, (DateTime)metadataError.FileDateModified, FileEntryVersion.Error),
                                                                populateDataGrid: true, populateImageListViewItemThumbnail: false, populateImageListViewItemMetadata: true);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Logger.Error(ex, "ThreadCollectMetadataExiftool - Write dummy record for when Exiftool failed due to corrupt files.");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    } catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - Write dummy record for when Exiftool failed due to corrupt files.");
                                    }
                                    #endregion
                                    
                                    #region mediaFilesNotInDatabase.RemoveRange(0, range)
                                    try
                                    {
                                        lock (exiftoolSave_MediaFilesNotInDatabaseLock) exiftoolSave_MediaFilesNotInDatabase.RemoveRange(0, range); //Remove subset from queue before update status bar
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - mediaFilesNotInDatabase.RemoveRange(0, range) failed.");
                                    }
                                    #endregion 
                                }
                            }

                            if (GlobalData.IsApplicationClosing || GlobalData.IsStopAndEmptyExiftoolReadQueueRequest)
                                lock (commonQueueReadMetadataFromSourceExiftoolLock) commonQueueReadMetadataFromSourceExiftool.Clear();

                            //PopulateDataGridViewForSelectedItemsExtrasDelayed();

                            if (WaitExittoolReadCacheThread != null) WaitExittoolReadCacheThread.Set();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("Read exiftool thread crashed.", "Read exiftool thread failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (commonQueueReadMetadataFromSourceExiftoolLock) commonQueueReadMetadataFromSourceExiftool.Clear();
                            Logger.Error(ex, "ThreadCollectMetadataExiftool");
                        }
                        finally
                        {
                            GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = false;
                            //lock(_ThreadCollectMetadataExiftoolLock)
                            lock (_ThreadReadMetadataFromSourceExiftoolLock) _ThreadReadMetadataFromSourceExiftool = null;
                            Logger.Trace("ThreadCollectMetadataExiftool - ended");
                        }
                        #endregion
                    });

                    //(_ThreadCollectMetadataExiftoolLock) 
                    lock(_ThreadReadMetadataFromSourceExiftoolLock) if (_ThreadReadMetadataFromSourceExiftool != null)
                    {
                        _ThreadReadMetadataFromSourceExiftool.Priority = threadPriority;
                        _ThreadReadMetadataFromSourceExiftool.Start();                        
                    }
                    else Logger.Error("_ThreadCollectMetadataExiftool was not able to start");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "_ThreadExiftool.Start failed. ");
                //_ThreadCollectMetadataExiftool = null;
            }            
        }
        #endregion

        #region Exiftool - SaveMetadata UpdatedByUser - Add Queue
        public void AddQueueSaveUsingExiftoolMetadataUpdatedByUserLock(Metadata metadataToSave, Metadata metadataOriginal)
        {
            lock (exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUserLock) exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUser.Add(metadataToSave);
            lock (exiftoolSave_OrigialMetadataBeforeUserUpdateLock) exiftoolSave_OrigialMetadataBeforeUserUpdate.Add(metadataOriginal);
        }
        #endregion

        #region Exiftool - VerifyMetadata - Add Queue
        public void AddQueueVerifyMetadataLock(Metadata metadataToVerifyAfterSavedByExiftool)
        {
            lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock) exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify.Add(metadataToVerifyAfterSavedByExiftool);
        }
        #endregion 

        #region Exiftool - SaveUsingExiftool To Media - Thread
        public void ThreadSaveUsingExiftoolToMedia()
        {
            try
            {
                lock (_ThreadSaveUsingExiftoolToMediaLock) if (_ThreadSaveUsingExiftoolToMedia != null || CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountDirty() <= 0) return;

                lock (_ThreadSaveUsingExiftoolToMediaLock)
                {
                    _ThreadSaveUsingExiftoolToMedia = new Thread(() =>
                    {
                        #region Process all
                        try
                        {
                            if (!GlobalData.IsApplicationClosing)
                            {
                                Logger.Trace("ThreadSaveMetadata - started");

                                #region Init Write Variables and Parameters
                                bool writeXtraAtomOnMediaFile = Properties.Settings.Default.XtraAtomWriteOnFile;

                                string writeMetadataTagsVariable = Properties.Settings.Default.WriteMetadataTags;
                                string writeMetadataKeywordAddVariable = Properties.Settings.Default.WriteMetadataKeywordAdd;

                                string writeXtraAtomAlbumVariable = Properties.Settings.Default.XtraAtomAlbumVariable;
                                bool writeXtraAtomAlbumVideo = Properties.Settings.Default.XtraAtomAlbumVideo;

                                string writeXtraAtomCategoriesVariable = Properties.Settings.Default.XtraAtomCategoriesVariable;
                                bool writeXtraAtomCategoriesVideo = Properties.Settings.Default.XtraAtomCategoriesVideo;

                                string writeXtraAtomCommentVariable = Properties.Settings.Default.XtraAtomCommentVariable;
                                bool writeXtraAtomCommentPicture = Properties.Settings.Default.XtraAtomCommentPicture;
                                bool writeXtraAtomCommentVideo = Properties.Settings.Default.XtraAtomCommentVideo;

                                string writeXtraAtomKeywordsVariable = Properties.Settings.Default.XtraAtomKeywordsVariable;
                                bool writeXtraAtomKeywordsPicture = Properties.Settings.Default.XtraAtomKeywordsPicture;
                                bool writeXtraAtomKeywordsVideo = Properties.Settings.Default.XtraAtomKeywordsVideo;

                                bool writeXtraAtomRatingPicture = Properties.Settings.Default.XtraAtomRatingPicture;
                                bool writeXtraAtomRatingVideo = Properties.Settings.Default.XtraAtomRatingVideo;

                                string writeXtraAtomSubjectVariable = Properties.Settings.Default.XtraAtomSubjectVariable;
                                bool writeXtraAtomSubjectPicture = Properties.Settings.Default.XtraAtomSubjectPicture;
                                bool wtraAtomSubjectVideo = Properties.Settings.Default.XtraAtomSubjectVideo;

                                string writeXtraAtomSubtitleVariable = Properties.Settings.Default.XtraAtomSubtitleVariable;
                                bool writeXtraAtomSubtitleVideo = Properties.Settings.Default.XtraAtomSubtitleVideo;

                                string writeXtraAtomArtistVariable = Properties.Settings.Default.XtraAtomArtistVariable;
                                bool writeXtraAtomArtistVideo = Properties.Settings.Default.XtraAtomArtistVideo;

                                bool writeCreatedDateAndTimeAttribute = Properties.Settings.Default.WriteMetadataCreatedDateFileAttribute;
                                bool writeAddAutokeywords = Properties.Settings.Default.WriteMetadataAddAutoKeywords;
                                int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                                bool showCliWindow = Properties.Settings.Default.ApplicationDebugExiftoolWriteShowCliWindow;
                                ProcessPriorityClass processPriorityClass = (ProcessPriorityClass)Properties.Settings.Default.ApplicationDebugExiftoolWriteThreadPrioity;

                                List<string> allowedFileNameDateTimeFormats = FileDateTime.FileDateTimeReader.ConvertStringOfDatesToList(Properties.Settings.Default.RenameDateFormats);
                                #endregion

                                int writeCount = Math.Min(Properties.Settings.Default.ExiftoolMaximumWriteBach, CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountLock());
                                lock (exiftoolSave_QueueSubsetMetadataToSaveLock) exiftoolSave_QueueSubsetMetadataToSave = new List<Metadata>();    //This new values for saving (changes done by user)
                                List<Metadata> queueSubsetMetadataOrginalBeforeUserEdit = new List<Metadata>(); //Before updated by user, need this to check if any updates

                                #region Create a subset queue for writing
                                try
                                {
                                    for (int i = 0; i < writeCount; i++)
                                    {
                                        //Remeber 
                                        Metadata metadataWrite;
                                        Metadata metadataOrginal;

                                        lock (exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUserLock)
                                        {
                                            metadataWrite = exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUser[0];
                                            lock (exiftoolSave_OrigialMetadataBeforeUserUpdateLock) metadataOrginal = exiftoolSave_OrigialMetadataBeforeUserUpdate[0];

                                            lock (commonQueueReadMetadataFromSourceExiftoolLock)
                                            {
                                                if (!GlobalData.IsApplicationClosing)
                                                {
                                                    //Also include Metadata ToBeSaved that are Equal with OrgianalBeforeUserEdit 
                                                    if (metadataOrginal != metadataWrite) AddWatcherShowExiftoolSaveProcessQueue(metadataWrite.FileEntryBroker.FileFullPath);

                                                    lock (exiftoolSave_QueueSubsetMetadataToSaveLock) exiftoolSave_QueueSubsetMetadataToSave.Add(metadataWrite);
                                                    queueSubsetMetadataOrginalBeforeUserEdit.Add(metadataOrginal);
                                                }
                                            }

                                            //Remove
                                            exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUser.RemoveAt(0);
                                            lock (exiftoolSave_OrigialMetadataBeforeUserUpdateLock) exiftoolSave_OrigialMetadataBeforeUserUpdate.RemoveAt(0);

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error(ex, "ThreadSaveMetadata - Create a subset queue for writing");
                                }
                                #endregion

                                #region Save Metadatas using Exiftool  
                                //Wait file to be unlocked, if used by a process. E.g. some application writing to file, or OneDrive doing backup
                                //Will create DEADLOCK lock (commonQueueSubsetMetadataToSaveLock) 
                                if (!GlobalData.IsApplicationClosing) FileHandler.WaitLockedFilesToBecomeUnlocked(exiftoolSave_QueueSubsetMetadataToSave, true, this);

                                List<FileEntry> mediaFilesUpdatedByExiftool = new List<FileEntry>();
                                string exiftoolErrorMessage = "";

                                if (!GlobalData.IsApplicationClosing)
                                {
                                    try
                                    {
                                        lock (exiftoolSave_QueueSubsetMetadataToSaveLock)
                                        {
                                            #region AutoKeywords
                                            foreach (Metadata metadataCopy in exiftoolSave_QueueSubsetMetadataToSave)
                                            {
                                                if (writeAddAutokeywords && metadataCopy != null)
                                                {
                                                    List<string> newKeywords = AutoKeywordHandler.NewKeywords(autoKeywordConvertions, metadataCopy.LocationName, metadataCopy.PersonalTitle,
                                                        metadataCopy.PersonalAlbum, metadataCopy.PersonalDescription, metadataCopy.PersonalComments, metadataCopy.PersonalKeywordTags);
                                                    foreach (string keyword in newKeywords)
                                                    {
                                                        metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(keyword), false);
                                                    }
                                                }
                                            }
                                            #endregion
                                            
                                            UpdateStatusAction("Batch update a subset of " + exiftoolSave_QueueSubsetMetadataToSave.Count + " media files...");
                                            ExiftoolWriter.WriteMetadata(
                                            exiftoolSave_QueueSubsetMetadataToSave, queueSubsetMetadataOrginalBeforeUserEdit, allowedFileNameDateTimeFormats,
                                            writeMetadataTagsVariable, writeMetadataKeywordAddVariable, out mediaFilesUpdatedByExiftool,
                                            showCliWindow, processPriorityClass);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Something did go wrong, need to tell users, what was saved and not, in general nothing was saved
                                        lock (exiftoolSave_QueueSubsetMetadataToSaveLock)
                                        {
                                            foreach (Metadata metadataCopy in exiftoolSave_QueueSubsetMetadataToSave)
                                            {
                                                if (!FileEntry.Contains(mediaFilesUpdatedByExiftool, metadataCopy.FileEntry)) mediaFilesUpdatedByExiftool.Add(metadataCopy.FileEntry);
                                            }
                                        }

                                        exiftoolErrorMessage = ex.Message;
                                        Logger.Error(ex, "EXIFTOOL.EXE error...");
                                    }
                                }
                                #endregion

                                #region Write Xtra Atom properites
                                //Wait file to be unlocked, if used by a process. E.g. some application writing to file, or OneDrive doing backup
                                //Will create DEADLOCK lock (commonQueueSubsetMetadataToSaveLock)
                                //if (!GlobalData.IsApplicationClosing) FileHandler.WaitLockedFilesToBecomeUnlocked(commonQueueSubsetMetadataToSave, true, this);
                                //No need to wait, will wait for each file inside ExiftoolWriter.WriteXtraAtom

                                Dictionary<string, string> writeXtraAtomErrorMessageForFile = new Dictionary<string, string>();
                                List<FileEntry> filesUpdatedByWriteXtraAtom = new List<FileEntry>();
                                try
                                {

                                    if (!GlobalData.IsApplicationClosing)
                                    {
                                        UpdateStatusAction("Write Xtra Atom to " + exiftoolSave_QueueSubsetMetadataToSave.Count + " media files...");
                                        if (writeXtraAtomOnMediaFile)
                                        {
                                            filesUpdatedByWriteXtraAtom = ExiftoolWriter.WriteXtraAtom(
                                                exiftoolSave_QueueSubsetMetadataToSave, queueSubsetMetadataOrginalBeforeUserEdit, allowedFileNameDateTimeFormats,
                                                writeXtraAtomAlbumVariable, writeXtraAtomAlbumVideo,
                                                writeXtraAtomCategoriesVariable, writeXtraAtomCategoriesVideo,
                                                writeXtraAtomCommentVariable, writeXtraAtomCommentPicture, writeXtraAtomCommentVideo,
                                                writeXtraAtomKeywordsVariable, writeXtraAtomKeywordsPicture, writeXtraAtomKeywordsVideo,
                                                writeXtraAtomRatingPicture, writeXtraAtomRatingVideo,
                                                writeXtraAtomSubjectVariable, writeXtraAtomSubjectPicture, wtraAtomSubjectVideo,
                                                writeXtraAtomSubtitleVariable, writeXtraAtomSubtitleVideo,
                                                writeXtraAtomArtistVariable, writeXtraAtomArtistVideo,
                                                out writeXtraAtomErrorMessageForFile, this);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error(ex, "ThreadSaveMetadata - Write Xtra Atom properites");
                                }
                                #endregion

                                #region File Create date and Time attribute
                                //Wait file to be unlocked, if used by a process. E.g. some application writing to file, or OneDrive doing backup
                                //Will create DEADLOCK lock (commonQueueSubsetMetadataToSaveLock)
                                if (!GlobalData.IsApplicationClosing) FileHandler.WaitLockedFilesToBecomeUnlocked(exiftoolSave_QueueSubsetMetadataToSave, true, this);

                                try
                                {
                                    if (!GlobalData.IsApplicationClosing)
                                    {
                                        if (writeCreatedDateAndTimeAttribute)
                                        {
                                            lock (exiftoolSave_QueueSubsetMetadataToSaveLock) foreach (Metadata metadata in exiftoolSave_QueueSubsetMetadataToSave)
                                                {
                                                    if (metadata.TryParseDateTakenToUtc(out DateTime? dateTakenWithOffset))
                                                    {
                                                        if (metadata?.FileDateCreated != null &&
                                                            metadata?.MediaDateTaken != null &&
                                                            metadata?.MediaDateTaken < DateTime.Now &&
                                                            Math.Abs(((DateTime)dateTakenWithOffset.Value.ToUniversalTime() - (DateTime)metadata?.FileDateCreated.Value.ToUniversalTime()).TotalSeconds) > writeCreatedDateAndTimeAttributeTimeIntervalAccepted) //No need to change
                                                        {
                                                            try
                                                            {
                                                                File.SetCreationTime(metadata.FileFullPath, (DateTime)dateTakenWithOffset);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Logger.Error(ex, "File.SetCreationTime failed...");
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.Warn("ThreadSaveMetadata - Was not able to convert time, missing UTC and/or MediaTaken time");
                                                    }
                                                }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error(ex, "ThreadSaveMetadata - File Create date and Time attribute");
                                }
                                #endregion


                                #region Check if all files was updated, if updated, add to verify queue
                                //Wait file to be unlocked, if used by a process. E.g. some application writing to file, or OneDrive doing backup
                                //Will create DEADLOCK lock (commonQueueSubsetMetadataToSaveLock)
                                if (!GlobalData.IsApplicationClosing) FileHandler.WaitLockedFilesToBecomeUnlocked(exiftoolSave_QueueSubsetMetadataToSave, true, this);

                                if (!GlobalData.IsApplicationClosing)
                                {
                                    foreach (FileEntry fileSuposeToBeUpdated in mediaFilesUpdatedByExiftool)
                                    {
                                        try
                                        {
                                            #region Write to Xtra Atom failed?
                                            //Check if writing Xtra Atom properties failed
                                            if (writeXtraAtomErrorMessageForFile.ContainsKey(fileSuposeToBeUpdated.FileFullPath))
                                            {
                                                AddError(fileSuposeToBeUpdated.Directory, fileSuposeToBeUpdated.FileName, fileSuposeToBeUpdated.LastWriteDateTime,
                                                    AddErrorExiftooRegion, AddErrorExiftooCommandWrite, AddErrorExiftooParameterWrite, AddErrorExiftooParameterWrite,
                                                    "Failed write Xtra Atom property to file: " + fileSuposeToBeUpdated.FileFullPath + "\r\n" +
                                                    "Failed write Xtra Atom property to file: " + fileSuposeToBeUpdated.FileFullPath + "\r\n" +
                                                    "Error message:" + writeXtraAtomErrorMessageForFile[fileSuposeToBeUpdated.FileFullPath] +
                                                    "File staus:" + fileSuposeToBeUpdated.FileFullPath + "\r\n" + FileHandler.FileStatusText(fileSuposeToBeUpdated.FileFullPath));
                                            }
                                            #endregion

                                            #region Write using Exiftool failed?
                                            DateTime currentLastWrittenDateTime = File.GetLastWriteTime(fileSuposeToBeUpdated.FileFullPath);
                                            DateTime previousLastWrittenDateTime = (DateTime)fileSuposeToBeUpdated.LastWriteDateTime;

                                            //Find last known writtenDate for file
                                            //int index = FileEntry.FindIndex(filesUpdatedByWriteXtraAtom, fileSuposeToBeUpdated);
                                            //if (index > -1) previousLastWrittenDateTime = filesUpdatedByWriteXtraAtom[index].LastWriteDateTime;
                                            //Check if file is updated, if file LastWrittenDateTime has changed, file is updated
                                            if (currentLastWrittenDateTime <= previousLastWrittenDateTime)
                                            {
                                                AddError(fileSuposeToBeUpdated.Directory, fileSuposeToBeUpdated.FileName, fileSuposeToBeUpdated.LastWriteDateTime,
                                                        AddErrorExiftooRegion, AddErrorExiftooCommandWrite, AddErrorExiftooParameterWrite, AddErrorExiftooParameterWrite,
                                                        "EXIFTOOL.EXE failed write to file:" + fileSuposeToBeUpdated.FileFullPath + "\r\n" +
                                                        "Message return from Exiftool: " + exiftoolErrorMessage + "\r\n" +
                                                        "File staus:" + fileSuposeToBeUpdated.FileFullPath + "\r\n" + FileHandler.FileStatusText(fileSuposeToBeUpdated.FileFullPath));
                                            }
                                            #endregion 

                                            int indexInVerifyQueue = Metadata.FindFileEntryInList(exiftoolSave_QueueSubsetMetadataToSave, fileSuposeToBeUpdated);
                                            if (indexInVerifyQueue > -1 && indexInVerifyQueue < exiftoolSave_QueueSubsetMetadataToSave.Count)
                                            {
                                                Metadata currentMetadata;
                                                lock (exiftoolSave_QueueSubsetMetadataToSaveLock) currentMetadata = new Metadata(exiftoolSave_QueueSubsetMetadataToSave[indexInVerifyQueue]);
                                                currentMetadata.FileDateModified = currentLastWrittenDateTime;
                                                if (File.Exists(currentMetadata.FileFullPath) && currentLastWrittenDateTime != previousLastWrittenDateTime) AddQueueVerifyMetadataLock(currentMetadata);
                                                AddQueueReadFromSourceIfMissing_AllSoruces(currentMetadata.FileEntryBroker);
                                                ImageListViewReloadThumbnailAndMetadataInvoke(imageListView1, fileSuposeToBeUpdated.FileFullPath);
                                            }
                                            else
                                            {
                                                Logger.Warn("Was not able to removed from queue, didn't exist in queue anymore: " + fileSuposeToBeUpdated);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Error(ex, "ThreadSaveMetadata - Check if all files was updated, if updated, add to verify queue");
                                        }
                                    }
                                }
                                #endregion

                                //Wait file to be unlocked, if used by a process. E.g. some application writing to file, or OneDrive doing backup
                                //Will create DEADLOCK lock (commonQueueSubsetMetadataToSaveLock)
                                if (!GlobalData.IsApplicationClosing) FileHandler.WaitLockedFilesToBecomeUnlocked(exiftoolSave_QueueSubsetMetadataToSave, true, this);

                                //Clean up
                                lock (exiftoolSave_QueueSubsetMetadataToSaveLock) exiftoolSave_QueueSubsetMetadataToSave.Clear();
                                queueSubsetMetadataOrginalBeforeUserEdit.Clear();
                                mediaFilesUpdatedByExiftool.Clear();

                                //Status updated for user
                                ShowExiftoolSaveProgressClear();
                            }

                            if (GlobalData.IsApplicationClosing)
                            {
                                lock (exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUserLock) exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUser.Clear();
                                lock (exiftoolSave_OrigialMetadataBeforeUserUpdateLock) exiftoolSave_OrigialMetadataBeforeUserUpdate.Clear();
                            }

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("Saving ThreadSaveMetadata crashed. The ThreadSaveMetadata queue was cleared. Nothing was saved.", "Saving ThreadSaveMetadata failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (exiftoolSave_QueueSubsetMetadataToSaveLock) exiftoolSave_QueueSubsetMetadataToSave.Clear();
                            Logger.Error(ex, "ThreadSaveMetadata: ");
                        }
                        finally
                        {
                            lock(_ThreadSaveUsingExiftoolToMediaLock) _ThreadSaveUsingExiftoolToMedia = null;
                            Logger.Trace("ThreadSaveMetadata - ended");
                        }

                        #endregion

                    });

                    lock (_ThreadSaveUsingExiftoolToMediaLock) if (_ThreadSaveUsingExiftoolToMedia != null)
                    {
                        _ThreadSaveUsingExiftoolToMedia.Priority = threadPriority;
                        _ThreadSaveUsingExiftoolToMedia.Start();                        
                    }
                    else Logger.Error("_ThreadSaveMetadata was not able to start");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "_ThreadSaveMetadata.Start failed. ");
            }
        }
        #endregion

        #endregion

        #region MicrosoftPhotos

        #region MicrosoftPhotos - ReadFromSource Metadata MicrosoftPhotos - AddQueue
        /// <summary>
        /// Add File Entry to Read "Metadata Queue"
        /// Inside Queue -> 
        ///     1. "Extract Exiftool data" ->> Store in database
        ///     2. Updated DataGridView to show new/updated Metadata
        ///     3. Add Metadata to Region Queue
        ///     -- Region Queue: Read Media Poster -> Extract Region Thmbnail
        /// </summary>
        /// <param name="fileEntry"></param>
        public void AddQueueReadFromSourceMetadataMicrosoftPhotosLock(FileEntry fileEntry)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock)
            {
                if (!commonQueueReadMetadataFromSourceMicrosoftPhotos.Contains(fileEntry)) commonQueueReadMetadataFromSourceMicrosoftPhotos.Add(fileEntry);
            }
        }
        #endregion

        #region MicrosoftPhotos - ReadFromSource Metadata MicrosoftPhotos - **Populate** - Thread 
        public void ThreadReadFromSourceMetadataMicrosoftPhotos()
        {
            try
            {
                lock (_ThreadReadFromSourceMicrosoftPhotosLock) if (_ThreadReadFromSourceMicrosoftPhotos != null || CommonQueueReadMetadataFromSourceMicrosoftPhotosCountDirty() <= 0) return;

                lock (_ThreadReadFromSourceMicrosoftPhotosLock)
                {
                    _ThreadReadFromSourceMicrosoftPhotos = new Thread(() =>
                    {
                        #region
                        try
                        {
                            Logger.Trace("ThreadCollectMetadataMicrosoftPhotos - started");
                            int count = CommonQueueReadMetadataFromMicrosoftPhotosCountLock();
                            while (CommonQueueReadMetadataFromMicrosoftPhotosCountLock() > 0 && !GlobalData.IsApplicationClosing)
                            {
                                count--;

                                #region Common for ThreadCollectMetadataWindowsLiveGallery and ThreadCollectMetadataMicrosoftPhotos
                                MetadataDatabaseCache database = databaseAndCacheMetadataMicrosoftPhotos;
                                ImetadataReader databaseSourceReader = databaseMicrosoftPhotos;
                                MetadataBrokerType broker = MetadataBrokerType.MicrosoftPhotos;

                                while (databaseSourceReader != null && !GlobalData.IsApplicationClosing && CommonQueueReadMetadataFromMicrosoftPhotosCountLock() > 0) //In case some more added to the queue
                                {
                                    Metadata metadataMicrosoftPhotos;
                                    FileEntry fileEntry;
                                    lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) fileEntry = new FileEntry(commonQueueReadMetadataFromSourceMicrosoftPhotos[0]);

                                    if (File.Exists(fileEntry.FileFullPath))
                                    {
                                        FileEntryBroker fileEntryBroker = new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos);
                                        if (!database.IsMetadataInCache(fileEntryBroker))
                                        {
                                            Metadata metadata = database.ReadMetadataFromCacheOrDatabase(fileEntryBroker);
                                            if (metadata == null)
                                            {
                                                metadataMicrosoftPhotos = databaseSourceReader.Read(broker, fileEntry.FileFullPath); 
                                                if (metadataMicrosoftPhotos != null) 
                                                {
                                                    //Windows Live Photo Gallery writes direclty to database from sepearte thread when found
                                                    database.TransactionBeginBatch();
                                                    database.Write(metadataMicrosoftPhotos);
                                                    database.TransactionCommitBatch();
                                                    AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataMicrosoftPhotos);

                                                    DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(
                                                        new FileEntryAttribute(metadataMicrosoftPhotos.FileFullPath, (DateTime)metadataMicrosoftPhotos.FileDateModified, FileEntryVersion.ExtractedNowFromMediaFile),
                                                        populateDataGrid: true, populateImageListViewItemThumbnail: false, populateImageListViewItemMetadata: false);
                                                }
                                            }
                                        }
                                    }
                                    else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                                    lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) if (commonQueueReadMetadataFromSourceMicrosoftPhotos.Count > 0) commonQueueReadMetadataFromSourceMicrosoftPhotos.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar
                                }
                                #endregion
                            }

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) commonQueueReadMetadataFromSourceMicrosoftPhotos.Clear();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("ThreadCollectMetadataMicrosoftPhotos crashed.", "ThreadCollectMetadataMicrosoftPhotos failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            Logger.Error(ex, "ThreadCollectMetadataMicrosoftPhotos failed");
                        }
                        finally
                        {
                            lock (_ThreadReadFromSourceMicrosoftPhotosLock) _ThreadReadFromSourceMicrosoftPhotos = null;
                            Logger.Trace("ThreadCollectMetadataMicrosoftPhotos - ended");
                        }
                        #endregion

                    });

                    lock (_ThreadReadFromSourceMicrosoftPhotosLock) if (_ThreadReadFromSourceMicrosoftPhotos != null)
                    {
                        _ThreadReadFromSourceMicrosoftPhotos.Priority = threadPriority;
                        _ThreadReadFromSourceMicrosoftPhotos.Start();                        
                    }
                    else Logger.Error("_ThreadMicrosoftPhotos was not able to start");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "_ThreadMicrosoftPhotos.Start failed. ");
                //_ThreadMicrosoftPhotos = null;
            }
            
        }
        #endregion

        #endregion

        #region WindowsLivePhotoGallery

        #region WindowsLivePhotoGallery - ReadFromSource Metadata WindowsLivePhotoGallery - AddQueue
        /// <summary>
        /// Add File Entry to Read "Metadata Queue"
        /// Inside Queue -> 
        ///     1. "Extract Exiftool data" ->> Store in database
        ///     2. Updated DataGridView to show new/updated Metadata
        ///     3. Add Metadata to Region Queue
        ///     -- Region Queue: Read Media Poster -> Extract Region Thmbnail
        /// </summary>
        /// <param name="fileEntry"></param>
        public void AddQueueReadFromSourceWindowsLivePhotoGalleryLock(FileEntry fileEntry)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock)
            {
                if (!commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Contains(fileEntry)) commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Add(fileEntry);
            }
        }
        #endregion

        #region WindowsLivePhotoGallery - ReadFromSource Metadata WindowsLiveGallery - **Populate** - Thread - 
        public void ThreadReadFromSourceMetadataWindowsLiveGallery()
        {
            try
            {
                lock (_ThreadReadFromSourceWindowsLiveGalleryLock) if (_ThreadReadFromSourceWindowsLiveGallery != null || CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountDirty() <= 0) return;

                lock (_ThreadReadFromSourceWindowsLiveGalleryLock)
                {
                    _ThreadReadFromSourceWindowsLiveGallery = new Thread(() =>
                    {
                        #region
                        try
                        {
                            Logger.Trace("ThreadCollectMetadataWindowsLiveGallery - started");
                            int count = CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountLock();
                            while (count > 0 && CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountLock() > 0 && !GlobalData.IsApplicationClosing)
                            {
                                count--;

                                #region Common for ThreadCollectMetadataWindowsLiveGallery and ThreadCollectMetadataMicrosoftPhotos
                                MetadataDatabaseCache database = databaseAndCacheMetadataWindowsLivePhotoGallery;
                                ImetadataReader databaseSourceReader = databaseWindowsLivePhotGallery;
                                MetadataBrokerType broker = MetadataBrokerType.WindowsLivePhotoGallery;

                                while (databaseSourceReader != null && !GlobalData.IsApplicationClosing && CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountLock() > 0) //In case some more added to the queue
                                {
                                    Metadata metadataWindowsLivePhotoGallery;
                                    FileEntry fileEntry;
                                    lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) fileEntry = new FileEntry(commonQueueReadMetadataFromSourceWindowsLivePhotoGallery[0]);

                                    if (File.Exists(fileEntry.FileFullPath))
                                    {
                                        FileEntryBroker fileEntryBroker = new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery);
                                        if (!database.IsMetadataInCache(fileEntryBroker))
                                        {
                                            Metadata metadata = database.ReadMetadataFromCacheOrDatabase(fileEntryBroker);
                                            if (metadata == null)
                                            {
                                                //Read from broker as Microsoft Photos, Windows Live Photo Gallery (Using NamedPipes)
                                                metadataWindowsLivePhotoGallery = databaseSourceReader.Read(broker, fileEntry.FileFullPath);
                                                if (metadataWindowsLivePhotoGallery != null)
                                                {
                                                    database.TransactionBeginBatch();
                                                    database.Write(metadataWindowsLivePhotoGallery);
                                                    database.TransactionCommitBatch();
                                                    AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataWindowsLivePhotoGallery);

                                                    DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(
                                                        new FileEntryAttribute(metadataWindowsLivePhotoGallery.FileFullPath, (DateTime)metadataWindowsLivePhotoGallery.FileDateModified, FileEntryVersion.ExtractedNowFromMediaFile),
                                                        populateDataGrid: true, populateImageListViewItemThumbnail: false, populateImageListViewItemMetadata: false);
                                                }
                                            }
                                        }
                                    }
                                    else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                                    lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) if (commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Count > 0) commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar
                                }
                                #endregion
                            }

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Clear();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("ThreadCollectMetadataWindowsLiveGallery crashed.", "ThreadCollectMetadataWindowsLiveGallery failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Clear();
                            Logger.Error(ex, "ThreadCollectMetadataWindowsLiveGallery failed");
                        }
                        finally
                        {
                            lock(_ThreadReadFromSourceWindowsLiveGalleryLock) _ThreadReadFromSourceWindowsLiveGallery = null;
                            Logger.Trace("ThreadCollectMetadataWindowsLiveGallery - ended");
                        }
                        #endregion
                    });

                    lock (_ThreadReadFromSourceWindowsLiveGalleryLock) if (_ThreadReadFromSourceWindowsLiveGallery != null)
                    {
                        _ThreadReadFromSourceWindowsLiveGallery.Priority = threadPriority; 
                        _ThreadReadFromSourceWindowsLiveGallery.Start();                        
                    }
                    else Logger.Error("_ThreadWindowsLiveGallery was not able to start");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "_ThreadWindowsLiveGallery.Start failed.");
            }
            
        }
        #endregion

        #endregion

        #region RegionAndThumbnail

        #region RegionAndThumbnail - SaveToDatabase RegionAndThumbnail - AddQueue
        private void AddQueueSaveToDatabaseRegionAndThumbnailLock(Metadata metadata)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueSaveToDatabaseRegionAndThumbnailLock)
            {
                if (!commonQueueSaveToDatabaseRegionAndThumbnail.Contains(metadata)) commonQueueSaveToDatabaseRegionAndThumbnail.Add(new Metadata(metadata));
            }
        }
        #endregion

        #region RegionAndThumbnail - SaveToDatabase RegionAndThumbnail - **Populate** - Thread
        /// <summary>
        /// Read list of Metadata with list of face region inside
        /// 1. Read media poster for "Media file"
        /// 2. Get Region Thumbnail from "Poster"
        /// 3. Save to database
        /// 4. Updated DataGridView with new "pictures"
        /// </summary>
        public void ThreadSaveToDatabaseRegionAndThumbnail()
        {
            try
            {
                if (IsThreadRunningExcept_ThreadSaveToDatabaseRegionAndThumbnail()) return; //Wait other thread to finnish first. Otherwise it will generate high load on disk use

                lock (_ThreadSaveToDatabaseRegionAndThumbnailLock) if (_ThreadSaveToDatabaseRegionAndThumbnail != null || CommonQueueSaveToDatabaseRegionAndThumbnailCountDirty() <= 0) return;

                lock (_ThreadSaveToDatabaseRegionAndThumbnailLock)
                {
                    _ThreadSaveToDatabaseRegionAndThumbnail = new Thread(() =>
                    {
                        #region
                        Logger.Trace("ThreadReadMediaPosterSaveRegions - started");
                        try
                        {
                            int curentCommonQueueReadPosterAndSaveFaceThumbnailsCount = CommonQueueSaveToDatabaseRegionAndThumbnailCountLock();
                            bool onlyDoWhatIsInCacheToAvoidHarddriveOverload = (IsThreadRunningExcept_ThreadSaveToDatabaseRegionAndThumbnail() == true);
                            bool dontReadFilesInCloud = Properties.Settings.Default.AvoidOfflineMediaFiles;

                            int indexSource = 0;

                            while (indexSource < curentCommonQueueReadPosterAndSaveFaceThumbnailsCount)  //Loop until queue empty or checked all
                            {
                                try
                                {
                                    FileEntryBroker fileEntryBrokerRegion;
                                    lock (commonQueueSaveToDatabaseRegionAndThumbnailLock) { fileEntryBrokerRegion = new FileEntryBroker(commonQueueSaveToDatabaseRegionAndThumbnail[indexSource].FileEntryBroker); }

                                    int fileIndexFound; //Loop all files and check more version of the file
                                    bool fileFoundNeedCheckForMoreWithSameFilename; //Due to remove item, need loop queue once more
                                    bool fileFoundRemoveFromList; //If other ques not empty, only create Regions on cahced posters, when others queues emoty start working on hardrive
                                    bool fileFoundInList = false;

                                    do //Loop the queue, to find regions for WLPG, Photos, WebScraping, Exiftoool then save and remove all with same filename
                                    {
                                        fileFoundNeedCheckForMoreWithSameFilename = false;
                                        fileFoundRemoveFromList = false;
                                        fileIndexFound = -1;

                                        Image image = null; //No image loaded

                                        int queueCount = CommonQueueSaveToDatabaseRegionAndThumbnailCountLock(); //Mark count that we will work with. 

                                        for (int thumbnailIndex = indexSource; thumbnailIndex < queueCount; thumbnailIndex++) //Not need to check already checked -> thumbnailIndex = indexSource
                                        {
                                            Metadata metadataActiveAlreadyCopy = null;
                                            lock (commonQueueSaveToDatabaseRegionAndThumbnailLock) { metadataActiveAlreadyCopy = commonQueueSaveToDatabaseRegionAndThumbnail[thumbnailIndex]; }

                                            //Find current file entry in queue, Exiftool, Microsoft Photos, Windows Live Gallery, etc...
                                            if (FilesCutCopyPasteDrag.IsFilenameEqual(metadataActiveAlreadyCopy.FileFullPath, fileEntryBrokerRegion.FileFullPath) &&
                                            metadataActiveAlreadyCopy.FileDateModified == fileEntryBrokerRegion.LastWriteDateTime)
                                            {

                                                fileIndexFound = thumbnailIndex;
                                                fileFoundInList = true;
                                                fileFoundNeedCheckForMoreWithSameFilename = true;

                                                
                                                if (metadataActiveAlreadyCopy.PersonalRegionList.Count == 0)
                                                {
                                                    fileFoundRemoveFromList = true; //No regions to create, remove from queue
                                                }
                                                else
                                                {
                                                    #region When Face Regions - Load poster
                                                    if (onlyDoWhatIsInCacheToAvoidHarddriveOverload)
                                                    {
                                                        if (image != null)
                                                        {
                                                            //DEBUG Does the images come
                                                        }
                                                        image = PosterCacheRead(fileEntryBrokerRegion.FileFullPath);
                                                        if (image != null) fileFoundRemoveFromList = true;
                                                        else fileFoundRemoveFromList = false; //Not in cache, need wait for loading starts (that's after all other queue empty)
                                                    }
                                                    else
                                                    {
                                                        fileFoundRemoveFromList = true;
                                                        bool isFileUnLockedAndExist = FileHandler.WaitLockedFileToBecomeUnlocked(fileEntryBrokerRegion.FileFullPath, false, this);

                                                        
                                                        if (
                                                            isFileUnLockedAndExist && (
                                                            fileEntryBrokerRegion.Broker == MetadataBrokerType.WebScraping || //If source WebScraper, date and time will not match                                                        
                                                            File.GetLastWriteTime(fileEntryBrokerRegion.FileFullPath) == fileEntryBrokerRegion.LastWriteDateTime) //Check if the current Metadata are same as newest file... If not file exist anymore, date will become {01.01.1601 01:00:00}
                                                        )
                                                        {
                                                            bool isFullSizeThumbnail = true;
                                                            foreach (RegionStructure regionStructure in metadataActiveAlreadyCopy.PersonalRegionList)
                                                            {
                                                                if (regionStructure.AreaX != 0 ||
                                                                    regionStructure.AreaY != 0 ||
                                                                    regionStructure.AreaWidth != 1 ||
                                                                    regionStructure.AreaHeight != 1)
                                                                {
                                                                    isFullSizeThumbnail = false;
                                                                    break;
                                                                }
                                                            }
                                                            if (isFullSizeThumbnail) image = databaseAndCacheThumbnail.ReadThumbnailFromCacheOrDatabase(fileEntryBrokerRegion);

                                                            try
                                                            {
                                                                if (image == null)
                                                                {
                                                                    bool isFileInCloud = FileHandler.IsFileInCloud(fileEntryBrokerRegion.FileFullPath);
                                                                    if (!isFileInCloud || (isFileInCloud && !dontReadFilesInCloud)) image = LoadMediaCoverArtPoster(fileEntryBrokerRegion.FileFullPath);
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Logger.Error(ex, "ThreadReadMediaPosterSaveRegions - LoadMediaCoverArtPoster");
                                                            }

                                                            if (image == null) //If failed load cover art, often occur after filed is moved or deleted
                                                            {
                                                                if (!(FileHandler.IsFileInCloud(fileEntryBrokerRegion.FileFullPath) && dontReadFilesInCloud))
                                                                {
                                                                    string writeErrorDesciption = 
                                                                        "Failed loading mediafile. Was not able to update thumbnail for region for the file:" + fileEntryBrokerRegion.FileFullPath + "\r\n" +
                                                                        "File staus:" + fileEntryBrokerRegion.FileFullPath + "\r\n" + FileHandler.FileStatusText(fileEntryBrokerRegion.FileFullPath);
                                                                    Logger.Error(writeErrorDesciption);

                                                                    AddError(
                                                                        fileEntryBrokerRegion.Directory,
                                                                        fileEntryBrokerRegion.FileName,
                                                                        fileEntryBrokerRegion.LastWriteDateTime,
                                                                        AddErrorFileSystemRegion, AddErrorFileSystemRead,
                                                                        AddErrorFileSystemRead, AddErrorFileSystemRead,
                                                                        writeErrorDesciption);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion 

                                                    #region Image found - Save it - and update data grid
                                                    if (image != null) //Save regions when have image poster 
                                                    {
                                                        try
                                                        {
                                                            databaseAndCacheThumbnail.TransactionBeginBatch();                                                            
                                                            RegionThumbnailHandler.SaveThumbnailsForRegioList_AlsoWebScarper(databaseAndCacheMetadataExiftool, metadataActiveAlreadyCopy, image);                                                            
                                                            databaseAndCacheThumbnail.TransactionCommitBatch();
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Logger.Error(ex, "ThreadReadMediaPosterSaveRegions - SaveThumbnailsForRegioList");
                                                        }
                                                        DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(
                                                            new FileEntryAttribute(fileEntryBrokerRegion, FileEntryVersion.ExtractedNowFromMediaFile),
                                                            populateDataGrid: true, populateImageListViewItemThumbnail: true, populateImageListViewItemMetadata: true); //Updated Gridview
                                                    }
                                                    #endregion
                                                }

                                                if (fileFoundNeedCheckForMoreWithSameFilename) break; //No need to search more.
                                            }
                                        } //end of loop: for (int thumbnailIndex = indexSource; thumbnailIndex < queueCount; thumbnailIndex++)

                                        lock (commonQueueSaveToDatabaseRegionAndThumbnailLock)
                                        {
                                            if (fileFoundRemoveFromList && fileIndexFound > -1)
                                            {
                                                curentCommonQueueReadPosterAndSaveFaceThumbnailsCount--;
                                                commonQueueSaveToDatabaseRegionAndThumbnail.RemoveAt(fileIndexFound);
                                                //Check next FileEntry in queue, current will be next, due to removed an item
                                            }
                                            else indexSource++; //Check next FileEntry in queue
                                        }


                                    } while (fileFoundNeedCheckForMoreWithSameFilename);

                                    if (!fileFoundInList) //Should never occur ;-)
                                    {
                                        string writeErrorDesciption = "ThreadReadMediaPosterSaveRegions, file not found list for updated:" + fileEntryBrokerRegion.FileFullPath;
                                        Logger.Error(writeErrorDesciption);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    KryptonMessageBox.Show("Saving Region Thumbnail crashed. The 'save region queue' was cleared. Nothing was saved.", "Saving Region Thumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                    lock (commonQueueSaveToDatabaseRegionAndThumbnailLock) { commonQueueSaveToDatabaseRegionAndThumbnail.Clear(); } //Avoid loop, due to unknown error
                                    Logger.Error(ex, "ThreadReadMediaPosterSaveRegions crashed");
                                }
                            } //while (indexSource < curentCommonQueueReadPosterAndSaveFaceThumbnailsCount);

                            if (GlobalData.IsApplicationClosing) lock (commonQueueSaveToDatabaseRegionAndThumbnailLock) commonQueueSaveToDatabaseRegionAndThumbnail.Clear();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, "ThreadReadMediaPosterSaveRegions");
                        }
                        finally
                        {
                            lock(_ThreadSaveToDatabaseRegionAndThumbnailLock) _ThreadSaveToDatabaseRegionAndThumbnail = null;
                            Logger.Trace("ThreadReadMediaPosterSaveRegions - ended");
                        }
                        #endregion
                    });

                    lock (_ThreadSaveToDatabaseRegionAndThumbnailLock) if (_ThreadSaveToDatabaseRegionAndThumbnail != null)
                    {
                        _ThreadSaveToDatabaseRegionAndThumbnail.Priority = threadPriority;
                        _ThreadSaveToDatabaseRegionAndThumbnail.Start();                        
                    }
                    else Logger.Error("_ThreadThumbnailRegion was not able to start");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "_ThreadThumbnailRegion.Start failed.");
            }

        }
        #endregion

        #endregion

        #region Check ThreadQueues

        #region Check ThreadQueues - IsFolderInQueueSaveToDatabaseRegionAndThumbnail
        public bool IsFolderInQueueSaveToDatabaseRegionAndThumbnailLock (string folder)
        {
            bool folderInUse = false;
            lock (commonQueueSaveToDatabaseRegionAndThumbnailLock)
                foreach (Metadata metadata in commonQueueSaveToDatabaseRegionAndThumbnail)
                {
                    if (metadata.FileFullPath.StartsWith(folder))
                    {
                        folderInUse = true;
                        break;
                    }
                }
            return folderInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFileInQueueSaveToDatabaseRegionAndThumbnail
        public bool IsFileInQueueSaveToDatabaseRegionAndThumbnailLock(string fullFilename)
        {
            bool fileInUse = false;

            lock (commonQueueSaveToDatabaseRegionAndThumbnailLock)
                foreach (Metadata metadata in commonQueueSaveToDatabaseRegionAndThumbnail)
                {
                    if (FilesCutCopyPasteDrag.IsFilenameEqual(metadata.FileFullPath, fullFilename))
                    {
                        fileInUse = true;
                        break;
                    }
                }
            return fileInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFolderInQueueSaveToDatabaseMediaThumbnail
        public bool IsFolderInQueueSaveToDatabaseMediaThumbnailLock(string folder)
        {
            bool folderInUse = false;

            lock (commonQueueSaveToDatabaseMediaThumbnailLock)
                foreach (FileEntryImage fileEntry in commonQueueSaveToDatabaseMediaThumbnail)
                {
                    if (fileEntry.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                    {
                        folderInUse = true;
                        break;
                    }
                }
            return folderInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFileInQueueSaveToDatabaseMediaThumbnail
        public bool IsFileInQueueSaveToDatabaseMediaThumbnailLock(string fullFilename)
        {
            bool fileInUse = false;
            lock (commonQueueSaveToDatabaseMediaThumbnailLock)
                foreach (FileEntryImage fileEntry in commonQueueSaveToDatabaseMediaThumbnail)
                {
                    if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntry.FileFullPath, fullFilename))
                    {
                        fileInUse = true;
                        break;
                    }
                }
            return fileInUse; 
        }
        #endregion

        #region Check ThreadQueues - ExistFolderQueueReadMetadataFromSourceMicrosoftPhotos
        public bool IsFolderInQueueReadMetadataFromSourceMicrosoftPhotosLock(string folder)
        {
            bool folderInUse = false;

            lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock)
                foreach (FileEntry fileEntry in commonQueueReadMetadataFromSourceMicrosoftPhotos)
                {
                    if (fileEntry.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                    {
                        folderInUse = true;
                        break;
                    }
                }
            return folderInUse;
        }
        #endregion

        #region Check ThreadQueues - ExistFileQueueReadMetadataFromSourceMicrosoftPhotos
        public bool IsFileInQueueReadMetadataFromSourceMicrosoftPhotosLock(string fullFilename)
        {
            bool fileInUse = false;

            lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock)
            {
                foreach (FileEntry fileEntry in commonQueueReadMetadataFromSourceMicrosoftPhotos)
                {
                    if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntry.FileFullPath, fullFilename))
                    {
                        fileInUse = true;
                        break;
                    }
                }
            }
            return fileInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFolderInQueueReadMetadataFromSourceWindowsLivePhotoGallery
        public bool IsFolderInQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock(string folder)
        {
            bool folderInUse = false;
            
            if (!folderInUse)
                lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock)
                    foreach (FileEntry fileEntry in commonQueueReadMetadataFromSourceWindowsLivePhotoGallery)
                    {
                        if (fileEntry.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                        {
                            folderInUse = true;
                            break;
                        }
                    }
            
            return folderInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFileInQueueReadMetadataFromSourceWindowsLivePhotoGallery
        public bool IsFileInQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock(string fullFilename)
        {
            bool fileInUse = false;
            lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock)
                foreach (FileEntry fileEntry in commonQueueReadMetadataFromSourceWindowsLivePhotoGallery)
                {
                    if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntry.FileFullPath, fullFilename))
                    {
                        fileInUse = true;
                        break;
                    }
                }
            
            return fileInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFolderInQueueReadAndSaveMetadataFromSourceExiftool
        public bool IsFolderInQueueReadAndSaveMetadataFromSourceExiftoolLock(string folder)
        {
            bool folderInUse = false;

            if (!folderInUse)
                lock (commonQueueReadMetadataFromSourceExiftoolLock)
                    foreach (FileEntry fileEntry in commonQueueReadMetadataFromSourceExiftool)
                    {
                        if (fileEntry.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                        {
                            folderInUse = true;
                            break;
                        }
                    }

            if (!folderInUse)
                lock (exiftoolSave_MediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in exiftoolSave_MediaFilesNotInDatabase)
                    {
                        if (fileEntry.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                        {
                            folderInUse = true;
                            break;
                        }
                    }
            return folderInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFileInQueueReadAndSaveMetadataFromSourceExiftool
        public bool IsFileInQueueReadAndSaveMetadataFromSourceExiftoolLock(string fullFilename)
        {
            bool fileInUse = false;
            
            if (!fileInUse)
                lock (commonQueueReadMetadataFromSourceExiftoolLock)
                    foreach (FileEntry fileEntry in commonQueueReadMetadataFromSourceExiftool)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntry.FileFullPath, fullFilename))
                        {
                            fileInUse = true;
                            break;
                        }
                    }

            if (!fileInUse)
                lock (exiftoolSave_MediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in exiftoolSave_MediaFilesNotInDatabase)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntry.FileFullPath, fullFilename))
                        {
                            fileInUse = true;
                            break;
                        }
                    }
            return fileInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFolderInQueueSaveUsingExiftoolMetadata
        public bool IsFolderInQueueSaveUsingExiftoolMetadataLock(string folder)
        {
            bool folderInUse = false;
            
            if (!folderInUse)
                lock (exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUserLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUser)
                    {
                        if (metadata.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                        {
                            folderInUse = true;
                            break;
                        }
                    }
            

            if (!folderInUse)
                lock (exiftoolSave_QueueSubsetMetadataToSaveLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSubsetMetadataToSave)
                    {
                        if (metadata.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                        {
                            folderInUse = true;
                            break;
                        }
                    }

            if (!folderInUse)
                lock (exiftoolSave_MediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in exiftoolSave_MediaFilesNotInDatabase)
                    {
                        if (fileEntry.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                        {
                            folderInUse = true;
                            break;
                        }
                    }

            return folderInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFileInQueueSaveUsingExiftoolMetadata
        public bool IsFileInQueueSaveUsingExiftoolMetadataLock(string fullFilename)
        {
            bool fileInUse = false;
            if (!fileInUse)
                lock (exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUserLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUser)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(metadata.FileFullPath, fullFilename))
                        {
                            fileInUse = true;
                            break;
                        }
                    }
            
            if (!fileInUse)
                lock (exiftoolSave_QueueSaveUsingExiftoolMetadataUpdatedByUserLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSubsetMetadataToSave)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(metadata.FileFullPath, fullFilename))
                        {
                            fileInUse = true;
                            break;
                        }
                    }
            
            if (!fileInUse)
                lock (exiftoolSave_MediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in exiftoolSave_MediaFilesNotInDatabase)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntry.FileFullPath, fullFilename))
                        {
                            fileInUse = true;
                            break;
                        }
                    }
            
            return fileInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFilesInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftool
        public bool IsFilesInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftoolLock(FileEntryAttribute fileEntryAttribute)
        {
            try
            {
                lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
                    if (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Contains(fileEntryAttribute)) return true;

                lock (commonQueueReadMetadataFromSourceExiftoolLock)
                    if (commonQueueReadMetadataFromSourceExiftool.Contains(fileEntryAttribute.FileEntry)) return true;

                lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
                {
                    foreach (FileEntryAttribute fileEntryAttributeCheck in commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntryAttributeCheck.FileFullPath, fileEntryAttribute.FileFullPath)) return true;
                    }
                }

                lock (commonQueueReadMetadataFromSourceExiftoolLock)
                {
                    foreach (FileEntry fileEntryCheck in commonQueueReadMetadataFromSourceExiftool)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntryCheck.FileFullPath, fileEntryAttribute.FileFullPath)) return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        #endregion

        #region Check ThreadQueues - IsFileInAnyQueue - collections
        #region Check ThreadQueues - IsFileInAnyQueue - HashSet<string> listOfFiles
        /// <summary>
        /// Check if given files is in one of queue and wait to be processed
        /// </summary>
        /// <param name="imageListView">Check seletced files in the ImageListView is waiting to be process</param>
        /// <returns>True, one of file waiting to be process in one of queues</returns>
        public bool IsFileInAnyQueueLock(HashSet<string> listOfFiles)
        {
            bool fileInUse = false;
            foreach (string fullFileName in listOfFiles)
            {
                fileInUse = IsFileInAnyQueueLock(fullFileName);
                if (fileInUse) break;
            }
            return fileInUse;
        }
        #endregion 

        #region Check ThreadQueues - IsFileInAnyQueue - StringCollection
        /// <summary>
        /// Check if given files is in one of queue and wait to be processed
        /// </summary>
        /// <param name="fileList">StringCollection fileList to check</param>
        /// <returns>True, one of file waiting to be process in one of queues</returns>
        public bool IsFileInAnyQueueLock(StringCollection fileList)
        {
            bool fileInUse = false;
            foreach (string fileFullPath in fileList)
            {
                fileInUse = IsFileInAnyQueueLock(fileFullPath);
                if (fileInUse) break;
            }
            return fileInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFileInAnyQueue - ImageListViewSelectedItemCollection
        /// <summary>
        /// Check if given files is in one of queue and wait to be processed
        /// </summary>
        /// <param name="fileList">ImageListViewSelectedItemCollection : IList<ImageListViewItem> fileList to check</param>
        /// <returns>True, one of file waiting to be process in one of queues</returns>
        public bool IsFileInAnyQueueLock(ImageListViewSelectedItemCollection imageListViewItems)
        {
            bool fileInUse = false;
            foreach (ImageListViewItem imageListViewItem in imageListViewItems)
            {
                fileInUse = IsFileInAnyQueueLock(imageListViewItem.FileFullPath);
                if (fileInUse) break;
            }
            return fileInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFileInAnyQueue - Filename
        /// <summary>
        /// Check if given file is in one of queue and wait to be processed
        /// </summary>
        /// <param name="fullFilename">File to check if in queue</param>
        /// <returns>True, the file is waiting to be process in one of queues</returns>
        public bool IsFileInAnyQueueLock(string fullFilename)
        {
            bool fileInUse = false;
            if (!fileInUse) fileInUse = IsFileInQueueSaveToDatabaseRegionAndThumbnailLock(fullFilename);
            if (!fileInUse) fileInUse = IsFileInQueueSaveToDatabaseMediaThumbnailLock(fullFilename);
            if (!fileInUse) fileInUse = IsFileInQueueReadMetadataFromSourceMicrosoftPhotosLock(fullFilename);
            if (!fileInUse) fileInUse = IsFileInQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock(fullFilename);
            if (!fileInUse) fileInUse = IsFileInQueueReadAndSaveMetadataFromSourceExiftoolLock(fullFilename);
            if (!fileInUse) fileInUse = IsFileInQueueSaveUsingExiftoolMetadataLock(fullFilename);

            return fileInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFolderInAnyQueue - Folder
        /// <summary>
        /// Check if given files is in one of queue and wait to be processed
        /// </summary>
        /// <param name="folder">Folder to check is in queue</param>
        /// <returns>True, one of folders waiting to be process in one of queues</returns>
        public bool IsFolderInAnyQueueLock(string folder)
        {
            bool folderInUse = false;
            if (!folderInUse) folderInUse = IsFolderInQueueSaveToDatabaseRegionAndThumbnailLock(folder);
            if (!folderInUse) folderInUse = IsFolderInQueueSaveToDatabaseMediaThumbnailLock(folder);
            if (!folderInUse) folderInUse = IsFolderInQueueReadMetadataFromSourceMicrosoftPhotosLock(folder);
            if (!folderInUse) folderInUse = IsFolderInQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock(folder);
            if (!folderInUse) folderInUse = IsFolderInQueueReadAndSaveMetadataFromSourceExiftoolLock(folder);
            if (!folderInUse) folderInUse = IsFolderInQueueSaveUsingExiftoolMetadataLock(folder);
            return folderInUse;
        }
        #endregion
        #endregion

        #endregion

        #region Rename Media Files

        #region Rename - AddQueueRenameMediaFilesLock
        public void AddQueueRenameMediaFilesLock(string fullFileName, string renameVariable)
        {
            lock (commonQueueRenameMediaFilesLock)
            {
                if (commonQueueRenameMediaFiles.ContainsKey(fullFileName))
                {
                    commonQueueRenameMediaFiles[fullFileName] = renameVariable;
                }
                else
                {
                    commonQueueRenameMediaFiles.Add(fullFileName, renameVariable);
                }
            }
        }
        #endregion

        #region Rename - ThreadRenameMediaFiles
        public void ThreadRenameMediaFiles()
        {
            try
            {
                lock (_ThreadRenameMediaFilesLock) if (_ThreadRenameMediaFiles != null || CommonQueueRenameCountDirty() <= 0) return;

                lock (_ThreadRenameMediaFilesLock)
                {
                     
                    _ThreadRenameMediaFiles = new Thread(() =>
                    {
                        try
                        {
                            #region Rename
                            Logger.Trace("ThreadRename - started");
                            int count = CommonQueueRenameCountLock();
                            while (count > 0 && CommonQueueRenameCountLock() > 0 && !GlobalData.IsApplicationClosing)
                            {
                                count--;
                                string fullFilename = "";
                                string renameVaiable = "";
                                bool fileInUse = false;

                                #region Find a file ready for rename
                                lock (commonQueueRenameMediaFilesLock)
                                {

                                    foreach (KeyValuePair<string, string> keyValuePair in commonQueueRenameMediaFiles)
                                    {
                                        fullFilename = keyValuePair.Key;
                                        renameVaiable = keyValuePair.Value;
                                        fileInUse = IsFileInAnyQueueLock(fullFilename);
                                        if (!fileInUse) 
                                            break; //File not in use found, start rename it
                                    }

                                }
                                #endregion

                                #region Remove from queue
                                if (!fileInUse)
                                {
                                    lock (commonQueueRenameMediaFilesLock)
                                    {
                                        if (commonQueueRenameMediaFiles.ContainsKey(fullFilename)) commonQueueRenameMediaFiles.Remove(fullFilename);
                                    }
                                }
                                #endregion

                                #region Do the renameing process
                                try
                                {
                                    if (!fileInUse)
                                    {
                                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fullFilename, File.GetLastWriteTime(fullFilename), MetadataBrokerType.ExifTool));

                                        if (metadata != null)
                                        {
                                            DataGridViewHandlerRename.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                                            DataGridViewHandlerRename.RenameVaribale = renameVaiable;
                                            DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;

                                            #region Get Old Filename
                                            string oldDirectory = Path.GetDirectoryName(fullFilename);
                                            #endregion

                                            #region Get new Filename
                                            string newFilename = DataGridViewHandlerRename.CreateNewFilename(renameVaiable, fullFilename, metadata);                                            
                                            string newFullFilename = FileHandler.CombinePathAndName(oldDirectory, newFilename);
                                            #endregion

                                            RenameFile_Thread_UpdateTreeViewFolderBrowser(treeViewFolderBrowser1, imageListView1, CommonQueueRenameCountLock(), fullFilename, newFullFilename);

                                        }
                                        else
                                        {
                                            Logger.Error("Was not able to read metadata after rename ");
                                            AddError(
                                                Path.GetDirectoryName(fullFilename),
                                                Path.GetFileName(fullFilename),
                                                File.GetLastWriteTime(fullFilename),
                                                AddErrorFileSystemRegion, AddErrorFileSystemMove, fullFilename, "New name is unknown (missing metadata)",
                                                "Failed rename " + fullFilename + " to : New name is unknown(missing metadata)" + "\r\n" +
                                                "File staus:" + fullFilename + "\r\n" + FileHandler.FileStatusText(fullFilename));
                                        }

                                    }
                                } catch (Exception ex)
                                {
                                    Logger.Trace(ex, "ThreadRename - Do the renameing process");

                                }
                                #endregion

                            }

                            #endregion
                            
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("Saving ThreadRename crashed. The ThreadRename queue was cleared. Nothing was saved.", "Saving ThreadRename failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (commonQueueRenameMediaFilesLock) commonQueueRenameMediaFiles.Clear();
                            Logger.Error(ex, "ThreadRename");
                        } finally
                        {
                            lock (_ThreadRenameMediaFilesLock) _ThreadRenameMediaFiles = null;
                            Logger.Trace("ThreadRename - ended");
                        }
                    });

                    lock (_ThreadRenameMediaFilesLock) if (_ThreadRenameMediaFiles != null)
                    {
                        _ThreadRenameMediaFiles.Priority = threadPriority; 
                        _ThreadRenameMediaFiles.Start();                        
                    }
                    else Logger.Error("_ThreadRenameMedafiles was not able to start");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "_ThreadRenameMedafiles.Start failed. ");
            }
            
            
        }
        #endregion 

        #endregion

        #region Error Message handling
        private static string listOfErrors = "";
        private static bool hasWriteAndVerifyMetadataErrors = false;

        const string AddErrorFileSystemRegion = "FileSystem";
        const string AddErrorFileSystemCopy = "Copy";
        const string AddErrorFileSystemMove = "Move";
        const string AddErrorFileSystemRead = "Read";
        const string AddErrorFileSystemCopyFolder = "Copy Folder";
        const string AddErrorFileSystemMoveFolder = "Move Folder";
        const string AddErrorFileSystemCreateFolder = "Create Folder";
        const string AddErrorFileSystemDeleteFolder = "Delete Folder";

        const string AddErrorPropertiesRegion = "Properties";
        const string AddErrorPropertiesCommandWrite = "Write";
        const string AddErrorPropertiesParameterWrite = "Write";
        const string AddErrorExiftooRegion = "Exiftool";
        const string AddErrorExiftooCommandVerify = "Verify";
        const string AddErrorExiftooParameterVerify = "Verify";
        const string AddErrorExiftooCommandWrite = "Write";
        const string AddErrorExiftooParameterWrite = "Write";
        const string AddErrorExiftooCommandRead = "Read";
        const string AddErrorExiftooParameterRead = "Read";
        const string AddErrorParameterNone = "Error";

        

        public void AddError(
            string fileDirectory, string fileName, DateTime fileDateModified,
            string region, string command, string oldValue, string newValue,
            string warning)
        {
            AddError(fileDirectory, fileName, fileDateModified,
            region, command, oldValue,
            region, command, newValue,
            warning, true);
        }

        public void AddError(string fileDirectory, string region, string command, string oldValue, string newValue, string warning)
        {
            DateTime dateTimeLastWrittenDate = DateTime.Now;
            try
            {
                dateTimeLastWrittenDate = Directory.GetLastWriteTime(fileDirectory);
            }
            catch { }

            AddError(fileDirectory, "", dateTimeLastWrittenDate,
            region, command, oldValue,
            region, command, newValue,
            warning, false);
        }


        public void AddError(
            string fileDirectory, string fileName, DateTime fileDateModified,
            string oldRegion, string oldCommand, string oldParameter,
            string newRegion, string newCommand, string newParameter,
            string warning, bool writeToDatabase)
        {
            if (writeToDatabase)
            {
                ExiftoolData exiftoolDataOld = new ExiftoolData(fileName, fileDirectory, fileDateModified, oldRegion, oldCommand, oldParameter);
                ExiftoolData exiftoolDataNew = new ExiftoolData(fileName, fileDirectory, fileDateModified, newRegion, newCommand, newParameter);
                databaseExiftoolWarning.Write(exiftoolDataOld, exiftoolDataNew, warning);
            }

            string fullFilePath = Path.Combine(fileDirectory, fileName);
            lock (queueErrorQueueLock)
            {
                if (!queueErrorQueue.ContainsKey(fullFilePath)) queueErrorQueue.Add(fullFilePath, warning);
            }

            listOfErrors += warning + "\r\n";
            hasWriteAndVerifyMetadataErrors = true;
            UpdateStatusAction("Saving metadata has errors...");
        }

        public void RemoveError(string fullFilePath)
        {
            lock (queueErrorQueueLock)
            {
                if (queueErrorQueue.ContainsKey(fullFilePath)) queueErrorQueue.Remove(fullFilePath);
            }
        }

        private static FormMessageBox formMessageBoxWarnings = null;
        private void timerShowErrorMessage_Tick(object sender, EventArgs e)
        {
            timerShowErrorMessage.Stop();
            if (hasWriteAndVerifyMetadataErrors)
            {
                string errors = listOfErrors;
                listOfErrors = "";
                hasWriteAndVerifyMetadataErrors = false;

                //MessageBox.Show(errors, "Warning or Errors has occured!", MessageBoxButtons.OK);
                if (formMessageBoxWarnings == null || formMessageBoxWarnings.IsDisposed) formMessageBoxWarnings = new FormMessageBox("Warning", errors);
                else formMessageBoxWarnings.AppendMessage(errors);
                formMessageBoxWarnings.Owner = this;
                formMessageBoxWarnings.Show();
            }
            try
            {
                timerShowErrorMessage.Start();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "timerShowErrorMessage.Start failed.");
            }
        }
        #endregion

    }
}
