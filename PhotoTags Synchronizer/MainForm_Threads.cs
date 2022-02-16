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
using System.Linq;
using System.Threading;
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

        private static readonly Object _ThreadLazyLoadingMapNomnatatimLock = new Object();
        private static Thread _ThreadLazyLoadingMapNomnatatim = null; //
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

        private static Dictionary<FileEntryAttribute, bool> commonLazyLoadingMapNomnatatim = new Dictionary<FileEntryAttribute, bool>();
        private static readonly Object commonLazyLoadingMapNomnatatimLock = new Object();
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

        private static List<FileEntry> exiftool_MediaFilesNotInDatabase = new List<FileEntry>(); //It's globale, just to manage to show count status
        private static readonly Object exiftool_MediaFilesNotInDatabaseLock = new Object();

        private static List<Metadata> exiftoolSave_QueueSubsetMetadataToSave = new List<Metadata>();
        private static readonly Object exiftoolSave_QueueSubsetMetadataToSaveLock = new Object();
        #endregion

        #endregion

        #region Error handling
        private static Dictionary<string, string> queueErrorQueue = new Dictionary<string, string>();
        private static readonly Object queueErrorQueueLock = new Object();
        #endregion

        #region Get Count of items in Queue with Lock
        private int CommonQueueLazyLoadingMapNomnatatimLock()
        {
            lock (commonLazyLoadingMapNomnatatimLock) return commonLazyLoadingMapNomnatatim.Count;
        }

        private int CommonQueueLazyLoadingMapNomnatatimDirty()
        {
            try
            {
                return commonLazyLoadingMapNomnatatim.Count;
            } catch
            {
                return 0;
            }
        }


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
            lock   (exiftool_MediaFilesNotInDatabaseLock) return exiftool_MediaFilesNotInDatabase.Count;
        }

        private int ExiftoolSave_MediaFilesNotInDatabaseCountDirty()
        {
            return exiftool_MediaFilesNotInDatabase.Count;
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
            ThreadQueueLazyLoadingMapNomnatatim();
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
            ThumbnailPosterDatabaseCache.StopCaching = true;
            lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock) commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Clear();
            lock (commonQueueReadMetadataFromSourceExiftoolLock) commonQueueReadMetadataFromSourceExiftool.Clear();
            lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) commonQueueReadMetadataFromSourceMicrosoftPhotos.Clear();
            lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Clear();
            lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock) exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify.Clear();

            WaitExittoolReadCacheThread = new AutoResetEvent(false);
            WaitThumbnailReadCacheThread = new AutoResetEvent(false);

            StartThreads();

            WaitExittoolReadCacheThread.WaitOne(10000);
            WaitExittoolReadCacheThread = null;

            WaitThumbnailReadCacheThread.WaitOne(10000);
            WaitThumbnailReadCacheThread = null;

            MetadataDatabaseCache.StopCaching = false;
            ThumbnailPosterDatabaseCache.StopCaching = false;

            GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = false;
            GlobalData.IsStopAndEmptyThumbnailQueueRequest = false;

        }
        #endregion

        //#region Preloading - Metadata - Thread - Faster Sqlite read for a list of files
        ///// <summary>
        ///// Faster read of metadata and put into the cache
        ///// </summary>
        ///// <param name="fileEntries">List of FileEntires to put in cache</param>
        //public void PreloadCacheFileEntries(HashSet<FileEntry> fileEntries, string selectedFolder)
        //{            
        //    try
        //    {
        //        bool isThreadRunning;
        //        int retry = 200;
        //        do
        //        {
        //            lock (_ThreadLazyLoadingMetadataFolderLock) isThreadRunning = (_ThreadLazyLoadingMetadataFolder != null);
        //            if (isThreadRunning)
        //            {
        //                MetadataDatabaseCache.StopCaching = true;
        //                ThumbnailPosterDatabaseCache.StopCaching = true;
        //                Task.Delay(10).Wait(); //Wait thread stopping
        //                Logger.Debug("CacheFileEntries - sleep(100) - ThreadRunning is running");
        //            }
        //        } while (isThreadRunning && retry-- > 0);

        //        lock (_ThreadLazyLoadingMetadataFolderLock) isThreadRunning = (_ThreadLazyLoadingMetadataFolder != null);

        //        if (isThreadRunning) return; //Still running, give up

        //        lock (_ThreadLazyLoadingMetadataFolderLock)
        //        {
        //            _ThreadLazyLoadingMetadataFolder = new Thread(() =>
        //            {
        //                #region
        //                try
        //                {
        //                    if (selectedFolder != null)
        //                    {
        //                        if (cacheFolderThumbnails) databaseAndCacheThumbnailPoster.ReadToCacheFolder(selectedFolder);
        //                        if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas(selectedFolder, MetadataBrokerType.ExifTool);
        //                        if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas(selectedFolder, MetadataBrokerType.WindowsLivePhotoGallery);
        //                        if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas(selectedFolder, MetadataBrokerType.MicrosoftPhotos);
        //                        if (cacheFolderWebScraperDataSets) databaseAndCacheMetadataExiftool.ReadToCacheWebScraperDataSet(fileEntries); //Don't have folder
        //                    }
        //                    else
        //                    {
        //                        if (cacheFolderThumbnails) databaseAndCacheThumbnailPoster.ReadToCache(fileEntries); //Read missing, new media files added
        //                        if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCache(fileEntries, MetadataBrokerType.ExifTool); //Read missing, new media files added
        //                        if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCache(fileEntries, MetadataBrokerType.WindowsLivePhotoGallery); //Read missing, new media files added
        //                        if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCache(fileEntries, MetadataBrokerType.MicrosoftPhotos); //Read missing, new media files added
        //                        if (cacheFolderWebScraperDataSets) databaseAndCacheMetadataExiftool.ReadToCacheWebScraperDataSet(fileEntries); //Read missing, new media files added
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    KryptonMessageBox.Show("CacheFileEntries crashed.", "CacheFileEntries failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
        //                    Logger.Error(ex, "CacheFileEntries crashed");
        //                }
        //                finally
        //                {
        //                    MetadataDatabaseCache.StopCaching = false;
        //                    ThumbnailPosterDatabaseCache.StopCaching = false;
        //                    lock (_ThreadLazyLoadingMetadataFolderLock) _ThreadLazyLoadingMetadataFolder = null;
        //                }
        //                #endregion
        //            });
        //        }

        //        lock (_ThreadLazyLoadingMetadataFolderLock) if (_ThreadLazyLoadingMetadataFolder != null)
        //        {
        //            _ThreadLazyLoadingMetadataFolder.Priority = threadPriority;
        //            _ThreadLazyLoadingMetadataFolder.Start();                    
        //        }
        //        else Logger.Error("_ThreadCacheSelectedFastRead was not able to start");
                

        //    }
        //    catch
        //    {
        //        //Retry after crash, eg. thread creation failed
        //        MetadataDatabaseCache.StopCaching = false;
        //        ThumbnailPosterDatabaseCache.StopCaching = false;
        //    }
        //}
        //#endregion 

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
                            if (IsFileInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftoolLock(dataGridViewGenericColumn.FileEntryAttribute)) queueCount++;
                        }
                    }
                }
            }
            return queueCount;
        }
        #endregion

        #region LazyLoading - All Versions / All Sources / Metadata And Region Thumbnails - AddQueue - (Read order: Cache, Database, Source) - AfterPopulateSelectedFiles 
        private void AddQueueLazyLoadningAllVersionsAllSourcesMetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(HashSet<FileEntry> imageListViewSelectItems)
        {
            List<FileEntryAttribute> lazyLoadingAllExiftoolVersionOfMediaFile = new List<FileEntryAttribute>();

            foreach (FileEntry fileEntry in imageListViewSelectItems)
            {
                List<FileEntryAttribute> fileEntryAttributeDateVersions =
                    databaseAndCacheMetadataExiftool.ListFileEntryAttributesCache(MetadataBrokerType.ExifTool,
                    fileEntry.FileFullPath, File.GetLastWriteTime(fileEntry.FileFullPath));
                lazyLoadingAllExiftoolVersionOfMediaFile.AddRange(fileEntryAttributeDateVersions);

                FileStatus fileStaus = FileHandler.GetFileStatus(fileEntry.FileFullPath,
                    exiftoolProcessStatus: ExiftoolProcessStatus.InExiftoolReadQueue);

                FileHandler.RemoveOfflineFileTouched(fileEntry.FileFullPath);
                FileHandler.RemoveOfflineFileTouchedFailed(fileEntry.FileFullPath);

                ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStaus);
            }

            AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(lazyLoadingAllExiftoolVersionOfMediaFile);
            AddQueueLazyLoadningMediaThumbnailLock(lazyLoadingAllExiftoolVersionOfMediaFile);
            AddQueueLazyLoadingMapNomnatatimLock(lazyLoadingAllExiftoolVersionOfMediaFile, forceReloadUsingReverseGeocoder: false);
            StartThreads();
        }
        #endregion 

        #region LazyLoading - All Sources / Metadata And Region Thumbnails - AddQueue - (Read order: Cache, Database, Source)
        public void AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(FileEntryAttribute fileEntryAttribute)
        {
            List<FileEntryAttribute> fileEntryAttributes = new List<FileEntryAttribute>();
            fileEntryAttributes.Add(fileEntryAttribute);
            AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttributes);
            TriggerAutoResetEventQueueEmpty();
        }
        #endregion

        #region LazyLoading - All Sources / Metadata And Region Thumbnails - AddQueue - (Read order: Cache, Database, Source)
        public void AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(List<FileEntryAttribute> fileEntryAttributes)
        {
            if (fileEntryAttributes == null) return;
            lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
            {
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes)
                {
                    if (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Contains(fileEntryAttribute))
                        commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Remove(fileEntryAttribute);
                    commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Insert(0, fileEntryAttribute);

                    //Need add last when not thread safe
                    //if (!commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Contains(fileEntryAttribute))
                    //    commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Add(fileEntryAttribute);
                }
            }
        }
        #endregion

        #region LazyLoading - All Sources / Metadata And Region Thumbnails - Thread - **Populate** - (** Doesn't add Read from Source **)
        public void ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails()
        {
            try
            {
                lock (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnailsLock) if (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails != null || CommonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsCountDirty() <= 0) return;
                lock (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnailsLock)
                {
                    _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails = new Thread(() =>
                    {
                        #region Do the work
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
                                    case FileEntryVersion.MetadataToSave:
                                    case FileEntryVersion.CompatibilityFixedAndAutoUpdated:
                                    case FileEntryVersion.CurrentVersionInDatabase:
                                    case FileEntryVersion.ExtractedNowUsingExiftool:
                                        readColumn = true;
                                        break;
                                    case FileEntryVersion.ExtractedNowUsingReadMediaFile:
                                    case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:
                                    case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:
                                    case FileEntryVersion.ExtractedNowUsingWebScraping:
                                        readColumn = true;
                                        break;
                                    case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:
                                    case FileEntryVersion.ExtractedNowUsingExiftoolWithError:
                                        readColumn = true;
                                        break;
                                    case FileEntryVersion.Error:
                                        if ((showWhatColumns & ShowWhatColumns.ErrorColumns) > 0) readColumn = true;
                                        else DataGridView_SetColumnVisibleStatus_FileEntryAttributeInvoke(fileEntryAttribute, false);

                                        if (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails[0].FileEntry == fileEntryAttribute.FileEntry)
                                        {
                                            FileStatus fileStatus = FileHandler.GetFileStatus(fileEntryAttribute.FileFullPath, fileInaccessibleOrError: true,
                                                fileErrorMessage: "Exitfool failed", exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);
                                            ImageListView_UpdateItemFileStatusInvoke(fileEntryAttribute.FileFullPath, fileStatus);
                                        }
                                        break;
                                    case FileEntryVersion.Historical:
                                        if ((showWhatColumns & ShowWhatColumns.HistoryColumns) > 0) readColumn = true;
                                        else DataGridView_SetColumnVisibleStatus_FileEntryAttributeInvoke(fileEntryAttribute, false);
                                        break;
                                    default:
                                        throw new Exception("Not implemeneted");
                                }

                                bool isMetadataExiftoolFound = false;
                                bool isMetadataExiftoolErrorFound = false;

                                if (readColumn)
                                {
                                    #region Exiftool with or without Error
                                    MetadataBrokerType metadataBrokerType = MetadataBrokerType.ExifTool;
                                    
                                    //If error Broker type attribute, set correct Broker type
                                    if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.Error) 
                                        metadataBrokerType |= MetadataBrokerType.ExifToolWriteError;

                                    FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileEntryAttribute, metadataBrokerType);
                                    Metadata metadataExiftool = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);

                                    if (metadataExiftool == null) //If Null, need retry, until Exiftool save metadata in Database,
                                                                  //if exiftool failes a dummy error record will be create
                                    {
                                        FileEntryBroker fileEntryBrokerExiftoolError = new FileEntryBroker(fileEntryAttribute,
                                        MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError);

                                        bool test = databaseAndCacheMetadataExiftool.IsMetadataInCache(fileEntryBrokerExiftoolError);
                                        Metadata metadataExiftoolError = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftoolError);
                                        if (metadataExiftoolError == null)
                                        {
                                            AddQueueReadFromSourceExiftoolLock(fileEntryAttribute); //Didn't exists in Database, need read from source
                                        }
                                        else
                                        {
                                            FileStatus fileStaus = FileHandler.GetFileStatus(fileEntryBrokerExiftoolError.FileFullPath,
                                                exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError,
                                                fileErrorMessage: "File has error recorded, you can retry read again", checkLockedStatus: true);
                                            ImageListView_UpdateItemFileStatusInvoke(fileEntryBrokerExiftoolError.FileFullPath, fileStaus);
                                            isMetadataExiftoolErrorFound = true;
                                        }
                                    }
                                    else
                                    {
                                        isMetadataExiftoolFound = true;
                                        //Check if Region Thumnbail missing, if yes, then create
                                        if (metadataExiftool.PersonalRegionIsThumbnailMissing()) 
                                            AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataExiftool);

                                        FileStatus fileStaus = FileHandler.GetFileStatus(metadataExiftool.FileFullPath,
                                        exiftoolProcessStatus: ExiftoolProcessStatus.WaitAction);
                                        ImageListView_UpdateItemFileStatusInvoke(metadataExiftool.FileFullPath, fileStaus);
                                        
                                    }
                                    #endregion

                                    #region Microsoft Photos
                                    FileEntryBroker fileEntryBrokerMicrosoftPhotos = new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.MicrosoftPhotos);
                                    Metadata metadataMicrosoftPhotos = null;
                                    if (!databaseAndCacheMetadataMicrosoftPhotos.IsMetadataInCache(fileEntryBrokerMicrosoftPhotos))
                                    {   
                                        metadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(fileEntryBrokerMicrosoftPhotos);
                                        if (metadataMicrosoftPhotos != null)
                                        {
                                            //Check if Region Thumnbail missing, if yes, then create
                                            if (metadataMicrosoftPhotos.PersonalRegionIsThumbnailMissing()) AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataMicrosoftPhotos);
                                        }
                                        else AddQueueReadFromSourceMetadataMicrosoftPhotosLock(fileEntryAttribute); //Didn't exists in Database, need read from source
                                    }
                                    #endregion

                                    #region WindowsLivePhotoGallery
                                    FileEntryBroker fileEntryBrokerWindowsLivePhotoGallery = new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.WindowsLivePhotoGallery);
                                    Metadata metadataWindowsLivePhotoGallery = null;
                                    if (!databaseAndCacheMetadataWindowsLivePhotoGallery.IsMetadataInCache(fileEntryBrokerWindowsLivePhotoGallery))
                                    {
                                        metadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(fileEntryBrokerWindowsLivePhotoGallery);                                        
                                        if (metadataWindowsLivePhotoGallery != null)
                                        {
                                            //Check if Region Thumnbail missing, if yes, then create
                                            if (metadataWindowsLivePhotoGallery.PersonalRegionIsThumbnailMissing()) AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataWindowsLivePhotoGallery);
                                        }
                                        else AddQueueReadFromSourceWindowsLivePhotoGalleryLock(fileEntryAttribute); //Didn't exists in Database, need read from source
                                    }
                                    #endregion

                                    #region WebScraper
                                    //Metadata folder will change to common folder name "WebScraper"
                                    FileEntryBroker fileEntryBrokerWebScraper = new FileEntryBroker(MetadataDatabaseCache.WebScapingFolderName,
                                            fileEntryAttribute.FileName, (DateTime)fileEntryAttribute.LastWriteDateTime, MetadataBrokerType.WebScraping);

                                    if (!databaseAndCacheMetadataExiftool.IsMetadataInCache(fileEntryBrokerWebScraper))
                                    {
                                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadWebScraperMetadataFromCacheOrDatabase(fileEntryBrokerWebScraper);

                                        if (metadata != null)
                                        {
                                            //Check if Region Thumnbail missing, if yes, then create
                                            if (metadata.PersonalRegionIsThumbnailMissing())
                                            {
                                                Metadata metadataWithFullPath = new Metadata(metadata);
                                                metadataWithFullPath.FileName = fileEntryAttribute.FileName;
                                                metadataWithFullPath.FileDirectory = fileEntryAttribute.Directory;
                                                metadataWithFullPath.FileDateModified = fileEntryAttribute.LastWriteDateTime;
                                                AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataWithFullPath);
                                            }
                                        }
                                    }
                                    #endregion

                                    bool isCurrent = FileEntryVersionHandler.IsCurrenOrUpdatedVersion(fileEntryAttribute.FileEntryVersion);
                                    if (isCurrent && isMetadataExiftoolFound) //Don't care about historical or error columns
                                        ImageListView_UpdateItemExiftoolMetadataInvoke(fileEntryAttribute);

                                    if (isMetadataExiftoolFound || isMetadataExiftoolErrorFound) // No need update others before Exiftool has data || isMetadataExiftoolErrorFound || isMetadataOtherSourceFound) 
                                        DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute);
                                }

                                #region Remove from Queue
                                lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
                                {
                                    if (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Count > 0) //The queue can be clear when change folder and search
                                    {
                                        //Check if queue been removed and still same item
                                        if (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails[0].FileEntry == fileEntryAttribute.FileEntry)
                                        {
                                            commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.RemoveAt(0);
                                        } else
                                        {
                                            count = 0; //New queue was create, need start over
                                        }
                                    } else //It's no more in queue
                                    {
                                        count = 0; //Start over, no more in queue
                                    }
                                }
                                #endregion

                                if (GlobalData.IsApplicationClosing) lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock) commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Clear();
                                TriggerAutoResetEventQueueEmpty();
                            }
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("LazyLoadingMetadata crashed.\r\n" + 
                                "The 'LazyLoadingMetadata' was cleared.\r\n" + 
                                "Exception message:" + ex.Message + "\r\n",
                                "Saving Region Thumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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

                                    if (!databaseAndCacheThumbnailPoster.DoesThumbnailExistInCache(fileEntryAttribute))
                                    {
                                        bool dontReadFileFromCloud = Properties.Settings.Default.AvoidOfflineMediaFiles;
                                        FileStatus fileStatus = FileHandler.GetFileStatus(fileEntryAttribute.FileFullPath);
                                        Image thumbnail = GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(fileEntryAttribute, dontReadFileFromCloud, fileStatus);                                       
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
                            KryptonMessageBox.Show("LazyLoadningThumbnail crashed." + 
                                "\r\nException message:" + ex.Message + "\r\n",
                                "LazyLoadningThumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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

        #endregion

        #region Media Thumbnail - Save

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
                                        fileEntryImage = new FileEntryImage(commonQueueSaveToDatabaseMediaThumbnail[0]);

                                    bool wasThumnbailEmptyAndReloaded = false;
                                    try
                                    {
                                        if (fileEntryImage.Image == null)
                                        {
                                            FileStatus fileStatus = FileHandler.GetFileStatus(fileEntryImage.FileFullPath);

                                            if (fileEntryImage.AllowLoadFromCloud && fileStatus.IsInCloudOrVirtualOrOffline)
                                            {
                                                fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitOfflineBecomeLocal;
                                                FileHandler.TouchOfflineFileToGetFileOnline(fileEntryImage.FileFullPath);

                                                ImageListView_UpdateItemFileStatusInvoke(fileEntryImage.FileFullPath, fileStatus);
                                            }

                                            //No need to check, LoadMediaCoverArtThumbnail will check if (!fileStatus.IsInCloudOrVirtualOrOffline)
                                            fileEntryImage.Image = LoadMediaCoverArtThumbnail(fileEntryImage.FileFullPath, ThumbnailSaveSize, fileStatus);
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
                                            databaseAndCacheThumbnailPoster.WriteThumbnail(fileEntryImage, fileEntryImage.Image);
                                            
                                            if (wasThumnbailEmptyAndReloaded)
                                            {
                                                DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute(new FileEntryAttribute(fileEntryImage, FileEntryVersion.ExtractedNowUsingReadMediaFile), fileEntryImage.Image);
                                                DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute(new FileEntryAttribute(fileEntryImage, FileEntryVersion.Error), fileEntryImage.Image);
                                                ImageListView_UpdateItemThumbnailUpdateAllInvoke(fileEntryImage);
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
                                    KryptonMessageBox.Show("Saving ThreadSaveThumbnail crashed.\r\n"+
                                        "The ThreadSaveThumbnail queue was cleared. Nothing was saved.\r\n" + 
                                        "Exception message:" + ex.Message + "\r\n",
                                        "Saving ThreadSaveThumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                bool existInQueue = IsFolderInQueueReadAndSaveMetadataFromSourceExiftoolLock(fileEntry.FileFullPath); 
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
                        #region Do The Work
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

                                #region Check if need avoid files in cloud, if yes, don't read files in cloud
                                if (Properties.Settings.Default.AvoidReadExifFromCloud)
                                {

                                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase_NeedCheckInCloud)
                                    {
                                        FileStatus fileStatus = FileHandler.GetFileStatus(fileEntry.FileFullPath);

                                        #region File not Exist, forget file 
                                        if (!fileStatus.FileExists)
                                        {
                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError;

                                            FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist);
                                            DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute);
                                        }
                                        #endregion
                                        #region File exist and not in clud, proceed
                                        else if (fileStatus.FileExists && !fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.ExiftoolProcessing;
                                            lock (exiftool_MediaFilesNotInDatabaseLock) exiftool_MediaFilesNotInDatabase.Add(fileEntry); 
                                        }
                                        #endregion
                                        #region File exist and offline, DON'T TOUCH file, not allowed
                                        else if (fileStatus.FileExists && fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.ExiftoolWillNotProcessingFileInCloud;

                                            //When in cloud, and can't read, also need to populate dataGridView but will become with empty rows in column
                                            FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.NotAvailable);
                                            DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute);
                                        }
                                        #endregion
                                        
                                        ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                                    }
                                }
                                else
                                {
                                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase_NeedCheckInCloud)
                                    {
                                        FileStatus fileStatus = FileHandler.GetFileStatus(fileEntry.FileFullPath);

                                        #region File not Exist, forget file
                                        if (!fileStatus.FileExists) 
                                        {
                                            fileStatus.FileExists = false;
                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError;

                                            FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist);
                                            DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute);
                                        }
                                        #endregion
                                        #region File exist and not in clud, proceed
                                        else if (fileStatus.FileExists && !fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.InExiftoolReadQueue;
                                            lock (exiftool_MediaFilesNotInDatabaseLock) exiftool_MediaFilesNotInDatabase.Add(fileEntry); //File in not in clode, process with file
                                        }
                                        #endregion
                                        #region File exist and offline, touch file and put back last in queue
                                        else if (fileStatus.FileExists && fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            #region Touched file failed
                                            if (FileHandler.IsOfflineFileTouchedAndFailedWithoutTimedOut(fileEntry.FileFullPath))
                                            {
                                                fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError; //TimeOut Error
                                                fileStatus.FileErrorMessage = "Failed download";

                                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.ExtractedNowUsingExiftoolTimeout);
                                                DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute);
                                            }
                                            #endregion
                                            else
                                            #region Touch file and put back in queue
                                            {
                                                FileHandler.TouchOfflineFileToGetFileOnline(fileEntry.FileFullPath);
                                                fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitOfflineBecomeLocal;

                                                AddQueueReadFromSourceExiftoolLock(fileEntry); //Add last in queue and wait for become downloaded
                                            }
                                            #endregion 
                                        }
                                        #endregion
                                        
                                        ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                                    }
                                }
                                #endregion

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

                                    lock (exiftool_MediaFilesNotInDatabaseLock)
                                    {
                                        while (argumnetLength < maxParameterCommandLength && range < exiftool_MediaFilesNotInDatabase.Count)
                                        {
                                            argumnetLength += exiftool_MediaFilesNotInDatabase[range].FileFullPath.Length + 3; //+3 = space and 2x"
                                            range++;
                                        }

                                        if (argumnetLength > maxParameterCommandLength) range--;
                                        useExiftoolOnThisSubsetOfFiles = new List<string>();
                                        if (range > 100)
                                            range = 100;
                                        for (int index = 0; index < range; index++)
                                            useExiftoolOnThisSubsetOfFiles.Add(exiftool_MediaFilesNotInDatabase[index].FileFullPath);
                                    }
                                    #endregion

                                    #region useExiftoolOnThisSubsetOfFiles - To - Read using Exiftool - Update ImageListView - Item Status
                                    string lastKownExiftoolGenericErrorMessage = "";
                                    Dictionary<string, string> exiftoolErrorMessageOnFiles = new Dictionary<string, string>();

                                    List<Metadata> metadataReadFromExiftool = new List<Metadata>();
                                    try
                                    {
                                        if (argumnetLength < maxParameterCommandLength) useArgFile = false;
                                        #region Update ImageListView Exiftool - process status
                                        foreach (string fullFilename in useExiftoolOnThisSubsetOfFiles)
                                        {
                                            FileStatus fileStatus = FileHandler.GetFileStatus(fullFilename, exiftoolProcessStatus: ExiftoolProcessStatus.ExiftoolProcessing);
                                            ImageListView_UpdateItemFileStatusInvoke(fullFilename, fileStatus);
                                        }
                                        #endregion
                                        exiftoolReader.Read(MetadataBrokerType.ExifTool, useExiftoolOnThisSubsetOfFiles, 
                                            out metadataReadFromExiftool,
                                            out exiftoolErrorMessageOnFiles, out lastKownExiftoolGenericErrorMessage,
                                            useArgFile, showCliWindow, processPriorityClass);
                                    }
                                    catch (Exception ex)
                                    {
                                        lastKownExiftoolGenericErrorMessage = 
                                            (string.IsNullOrWhiteSpace(lastKownExiftoolGenericErrorMessage) ? "" : "\r\n") + ex.Message;
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - Running Exiftool failed.");
                                    }
                                    #endregion

                                    #region NOT READ CHECK - Check if all files are read by Exiftool - Check against - metadataReadFromExiftool - if not read, no need to verify
                                    try
                                    {                                        
                                        foreach (string fullFilePath in useExiftoolOnThisSubsetOfFiles)
                                        {
                                            #region File put for Read, failed to be Read
                                            if (!Metadata.IsFullFilenameInList(metadataReadFromExiftool, fullFilePath))
                                            {
                                                #region FileStatus - Find Error Message for file or use Genral Exiftool error
                                                string exiftoolErrorMessageForFile;
                                                if (exiftoolErrorMessageOnFiles.ContainsKey(fullFilePath))
                                                    exiftoolErrorMessageForFile = exiftoolErrorMessageOnFiles[fullFilePath];
                                                else
                                                    exiftoolErrorMessageForFile = lastKownExiftoolGenericErrorMessage;
                                                
                                                FileStatus fileStatus = FileHandler.GetFileStatus(
                                                    fullFilePath, checkLockedStatus: true, checkLockStatusTimeout: FileHandler.GetFileLockedStatusTimeout,
                                                    fileInaccessibleOrError: true, fileErrorMessage: exiftoolErrorMessageForFile,
                                                    exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);

                                                exiftoolErrorMessageForFile =
                                                    "Issue: EXIFTOOL.EXE failed to read metadata\r\n" +
                                                    "File name: " + fullFilePath + "\r\n" +
                                                    "File staus: " + fileStatus.ToString() + "\r\n" +
                                                    "Error Message: " + exiftoolErrorMessageForFile;
                                                #endregion

                                                #region Find FileEntry Information
                                                FileEntry fileEntryMetadataNotRead;
                                                lock (exiftool_MediaFilesNotInDatabaseLock)
                                                {
                                                    int indexFileEntry = FileEntry.FindIndex(exiftool_MediaFilesNotInDatabase, fullFilePath, range);

                                                    if (indexFileEntry != -1) 
                                                        fileEntryMetadataNotRead = new FileEntry(exiftool_MediaFilesNotInDatabase[indexFileEntry]);
                                                    else
                                                    {
                                                        try
                                                        {
                                                            fileEntryMetadataNotRead = new FileEntry(fullFilePath, File.GetLastAccessTime(fullFilePath));
                                                        }
                                                        catch {
                                                            fileEntryMetadataNotRead = new FileEntry(fullFilePath, DateTime.Now);
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region Create dummy Metadata - MetadataBrokerType.ExifTool
                                                Metadata metadataDummy = new Metadata(MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError);
                                                metadataDummy.FileDateModified = fileEntryMetadataNotRead.LastWriteDateTime;
                                                metadataDummy.FileName = fileEntryMetadataNotRead.FileName;
                                                metadataDummy.FileDirectory = fileEntryMetadataNotRead.Directory;
                                                metadataDummy.FileMimeType = FormDatabaseCleaner.CorruptFile; //Also used
                                                metadataDummy.PersonalTitle = "Exiftool failed";
                                                metadataDummy.PersonalComments = FileHandler.ConvertFileStatusToText(fileStatus) + " " + lastKownExiftoolGenericErrorMessage;
                                                metadataDummy.FileDateCreated = fileEntryMetadataNotRead.LastWriteDateTime;
                                                metadataDummy.FileDateAccessed = DateTime.Now;
                                                metadataDummy.FileSize = 0;
                                                try
                                                {
                                                    if (File.Exists(fileEntryMetadataNotRead.FileFullPath))
                                                    {
                                                        FileInfo fileInfo = new FileInfo(fileEntryMetadataNotRead.FileFullPath);
                                                        metadataDummy.FileDateCreated = fileInfo.CreationTime;
                                                        metadataDummy.FileDateAccessed = fileInfo.LastAccessTime;
                                                        metadataDummy.FileSize = fileInfo.Length;
                                                    }
                                                    else
                                                    {
                                                        metadataDummy.PersonalComments = "File doesn't exist - Exiftool failed";
                                                    }
                                                }
                                                catch { }
                                                databaseAndCacheMetadataExiftool.Write(metadataDummy);
                                                #endregion

                                                #region Save Error, Update ImageListView Exiftool status
                                                ImageListView_UpdateItemFileStatusInvoke(metadataDummy.FileFullPath, fileStatus);

                                                FileEntryAttribute fileEntryAttributeError = new FileEntryAttribute(
                                                    metadataDummy.FileFullPath,
                                                    (DateTime)metadataDummy.FileDateModified,
                                                    FileEntryVersion.Error);

                                                AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttributeError);

                                                AddError(
                                                    Path.GetDirectoryName(fullFilePath), Path.GetFileName(fullFilePath), DateTime.Now,
                                                    AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                                    AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                                    exiftoolErrorMessageForFile, false);
                                                #endregion

                                                #region Remove from Verdify queue, no need verify 
                                                lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock)
                                                {
                                                    int verifyPosition = Metadata.FindFullFilenameInList(exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify, fullFilePath);
                                                    if (verifyPosition != -1) exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify.RemoveAt(verifyPosition);
                                                }
                                                #endregion
                                            }
                                            #endregion 
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - Check if all files are read by Exiftool failed.");
                                    }
                                    #endregion

                                    #region WHAT's READ CHECK - Verify previous saved data. (Saved data in exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify)
                                    try
                                    {
                                        foreach (Metadata metadataRead in metadataReadFromExiftool)
                                        {
                                            lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock)
                                            {
                                                bool isMetadataHavingErrors = false;
                                                #region Verify - Read back not Equal to saved - Then Add Error, Populate DataGridView and ImageListView
                                                if (ExiftoolWriter.HasWriteMetadataErrors(
                                                    metadataRead,
                                                    exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify,
                                                    out Metadata metadataUpdatedByUserCopy,
                                                    out string writeErrorDesciption))
                                                {
                                                    isMetadataHavingErrors = true;

                                                    #region Add Veridy Error (Not read error)
                                                    AddError(
                                                        metadataUpdatedByUserCopy.FileEntryBroker.Directory, metadataUpdatedByUserCopy.FileEntryBroker.FileName, metadataUpdatedByUserCopy.FileEntryBroker.LastWriteDateTime,
                                                        AddErrorExiftooRegion, AddErrorExiftooCommandVerify, 
                                                        AddErrorExiftooParameterVerify, AddErrorExiftooParameterVerify, 
                                                        "Issue: Verified Metadata is not equal with written.\r\n" + 
                                                        "File Name: " + metadataUpdatedByUserCopy.FileFullPath + "\r\n" +
                                                        "Error Message: " + writeErrorDesciption);
                                                    #endregion

                                                    #region Save Metadata with Broker Exiftool | WriteError 
                                                    Metadata metadataError = new Metadata(metadataUpdatedByUserCopy);
                                                    //metadataError.FileDateModified = DateTime.Now;
                                                    metadataError.Broker |= MetadataBrokerType.ExifToolWriteError;
                                                    databaseAndCacheMetadataExiftool.Write(metadataError);
                                                    #endregion

                                                    DataGridViewSetMetadataOnAllDataGridView(metadataRead);
                                                    DataGridViewSetDirtyFlagAfterSave(metadataRead, true);

                                                    #region DatagridView - Add new Error Coloumn
                                                    FileEntryAttribute fileEntryAttributeError = new FileEntryAttribute(
                                                        metadataError.FileFullPath, (DateTime)metadataError.FileDateModified, FileEntryVersion.Error);
                                                    AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttributeError);
                                                    #endregion
                                                }
                                                #endregion

                                                //Data was read, (even with errors), need to updated datagrid
                                                AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataRead);

                                                FileStatus fileStatus = FileHandler.GetFileStatus(metadataRead.FileFullPath);
                                                if (isMetadataHavingErrors)
                                                {
                                                    fileStatus.FileInaccessibleOrError = true;
                                                    fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError;
                                                    
                                                }
                                                else 
                                                    fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitAction;

                                                #region Populate - Current ImageListView Item - Current DataGridView Column
                                                FileEntryAttribute fileEntryAttributeExtractedNowUsingExiftool = new FileEntryAttribute(metadataRead.FileFullPath,
                                                    (DateTime)metadataRead.FileDateModified,
                                                    FileEntryVersion.ExtractedNowUsingExiftool);

                                                ImageListView_UpdateItemExiftoolMetadataInvoke(fileEntryAttributeExtractedNowUsingExiftool);
                                                ImageListView_UpdateItemFileStatusInvoke(metadataRead.FileFullPath, fileStatus);

                                                if (!isMetadataHavingErrors) AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttributeExtractedNowUsingExiftool);
                                                #endregion

                                                #region Populate - Current/Historical - DataGridView
                                                FileEntryAttribute fileEntryAttributeHistorical = new FileEntryAttribute(metadataRead.FileFullPath,
                                                    (DateTime)metadataRead.FileDateModified,
                                                    FileEntryVersion.Historical);
                                                AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttributeHistorical);
                                                #endregion
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - Verify readback after saved failed.");
                                    }
                                    #endregion

                                    #region mediaFilesNotInDatabase.RemoveRange(0, range)
                                    try
                                    {
                                        lock (exiftool_MediaFilesNotInDatabaseLock) exiftool_MediaFilesNotInDatabase.RemoveRange(0, range); //Remove subset from queue before update status bar
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - mediaFilesNotInDatabase.RemoveRange(0, range) failed.");
                                    }
                                    #endregion 
                                }

                                Thread.Sleep(10);
                            }

                            if (GlobalData.IsApplicationClosing || GlobalData.IsStopAndEmptyExiftoolReadQueueRequest)
                                lock (commonQueueReadMetadataFromSourceExiftoolLock) commonQueueReadMetadataFromSourceExiftool.Clear();


                            if (WaitExittoolReadCacheThread != null) WaitExittoolReadCacheThread.Set();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("Read exiftool thread crashed.\r\n" + 
                                "Exception message:" + ex.Message + "\r\n", 
                                "Read exiftool thread failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (commonQueueReadMetadataFromSourceExiftoolLock) commonQueueReadMetadataFromSourceExiftool.Clear();
                            Logger.Error(ex, "ThreadCollectMetadataExiftool");
                        }
                        finally
                        {
                            GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = false;
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

                                #region Init - Write with Exiftool - Variables and Parameters
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
                                int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                                bool showCliWindow = Properties.Settings.Default.ApplicationDebugExiftoolWriteShowCliWindow;
                                ProcessPriorityClass processPriorityClass = (ProcessPriorityClass)Properties.Settings.Default.ApplicationDebugExiftoolWriteThreadPrioity;

                                List<string> allowedFileNameDateTimeFormats = FileDateTimeReader.ConvertStringOfDatesToList(Properties.Settings.Default.RenameDateFormats);
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
                                List<FileEntry> mediaFilesUpdatedByExiftool = new List<FileEntry>();
                                string exiftoolErrorMessage = "";

                                if (!GlobalData.IsApplicationClosing)
                                {
                                    try
                                    {
                                        lock (exiftoolSave_QueueSubsetMetadataToSaveLock)
                                        {
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
                                if (!GlobalData.IsApplicationClosing)
                                {
                                    foreach (FileEntry fileSuposeToBeUpdated in mediaFilesUpdatedByExiftool)
                                    {
                                        try
                                        {
                                            #region Check if - Write to Xtra Atom failed?
                                            if (writeXtraAtomErrorMessageForFile.ContainsKey(fileSuposeToBeUpdated.FileFullPath))
                                            {
                                                FileStatus fileStatus = FileHandler.GetFileStatus(
                                                    fileSuposeToBeUpdated.FileFullPath, checkLockedStatus: true,
                                                    fileInaccessibleOrError: true, fileErrorMessage: writeXtraAtomErrorMessageForFile[fileSuposeToBeUpdated.FileFullPath],
                                                    exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);
                                                ImageListView_UpdateItemFileStatusInvoke(fileSuposeToBeUpdated.FileFullPath, fileStatus);
                                                
                                                AddError(
                                                    fileSuposeToBeUpdated.Directory, fileSuposeToBeUpdated.FileName, fileSuposeToBeUpdated.LastWriteDateTime,
                                                    AddErrorExiftooRegion, AddErrorExiftooCommandWrite, AddErrorExiftooParameterWrite, AddErrorExiftooParameterWrite,
                                                    "Issue: Failed write Xtra Atom property to file.\r\n" +
                                                    "File Name:   " + fileSuposeToBeUpdated.FileFullPath + "\r\n" +
                                                    "File Status: " + fileStatus.ToString() + "\r\n" +
                                                    "Error message:" + writeXtraAtomErrorMessageForFile[fileSuposeToBeUpdated.FileFullPath]);
                                            }
                                            #endregion

                                            #region Check if - Write using Exiftool failed?
                                            DateTime currentLastWrittenDateTime = File.GetLastWriteTime(fileSuposeToBeUpdated.FileFullPath);
                                            DateTime previousLastWrittenDateTime = (DateTime)fileSuposeToBeUpdated.LastWriteDateTime;

                                            //Find last known writtenDate for file
                                            //Check if file is updated, if file LastWrittenDateTime has changed, file is updated
                                            if (currentLastWrittenDateTime <= previousLastWrittenDateTime)
                                            {
                                                FileStatus fileStatus = FileHandler.GetFileStatus(
                                                    fileSuposeToBeUpdated.FileFullPath, checkLockedStatus: true,
                                                    fileInaccessibleOrError: true, fileErrorMessage: exiftoolErrorMessage,
                                                    exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);
                                                ImageListView_UpdateItemFileStatusInvoke(fileSuposeToBeUpdated.FileFullPath, fileStatus);
                                                
                                                #region Exiftool failed to write, no need to verify data that hasn't been written
                                                int indexExiftoolFailedOn = Metadata.FindFileEntryInList(exiftoolSave_QueueSubsetMetadataToSave, fileSuposeToBeUpdated);
                                                if (indexExiftoolFailedOn > -1 && indexExiftoolFailedOn < exiftoolSave_QueueSubsetMetadataToSave.Count)
                                                {
                                                    exiftoolSave_QueueSubsetMetadataToSave.RemoveAt(indexExiftoolFailedOn);
                                                }
                                                #endregion

                                                string lockedByProcessesText = "";
                                                if (fileStatus.HasAnyLocks) lockedByProcessesText = FileHandler.GetLockedByText(fileSuposeToBeUpdated.FileFullPath);
                                                
                                                AddError(
                                                    fileSuposeToBeUpdated.Directory, fileSuposeToBeUpdated.FileName, fileSuposeToBeUpdated.LastWriteDateTime,
                                                    AddErrorExiftooRegion, AddErrorExiftooCommandWrite, AddErrorExiftooParameterWrite, AddErrorExiftooParameterWrite,
                                                    "Issue: EXIFTOOL.EXE failed write to file." + "\r\n" +
                                                    "File Name:  " + fileSuposeToBeUpdated.FileFullPath + "\r\n" +
                                                    "File Status: " + FileHandler.GetFileStatusText(fileSuposeToBeUpdated.FileFullPath, checkLockedStatus: true, showLockedByProcess: true) + "\r\n" +
                                                    "Error Message: " + exiftoolErrorMessage); 
                                            }
                                            #endregion

                                            #region Add to Verify queue, clear thumbnail on ImageListView (exiftoolSave_QueueSubsetMetadataToSave contains only written)
                                            //When Exiftool - Write - Fail, then not in "exiftoolSave_QueueSubsetMetadataToSave"
                                            int indexInVerifyQueue = Metadata.FindFileEntryInList(exiftoolSave_QueueSubsetMetadataToSave, fileSuposeToBeUpdated);
                                            if (indexInVerifyQueue > -1 && indexInVerifyQueue < exiftoolSave_QueueSubsetMetadataToSave.Count)
                                            {
                                                Metadata currentMetadata;
                                                lock (exiftoolSave_QueueSubsetMetadataToSaveLock) currentMetadata = 
                                                    new Metadata(exiftoolSave_QueueSubsetMetadataToSave[indexInVerifyQueue]);
                                                currentMetadata.FileDateModified = currentLastWrittenDateTime;

                                                #region Save the metatdata into DataGridView(s) - saved metadata should also be readed back, if not, verify will tell save failed
                                                DataGridViewSetMetadataOnAllDataGridView(currentMetadata);
                                                DataGridViewSetDirtyFlagAfterSave(currentMetadata, false);
                                                #endregion

                                                #region If file was updated - Add to Verify queue
                                                if (File.Exists(currentMetadata.FileFullPath) && currentLastWrittenDateTime != previousLastWrittenDateTime) 
                                                    AddQueueVerifyMetadataLock(currentMetadata);
                                                #endregion

                                                FileEntryAttribute fileEntryAttribute = 
                                                    new FileEntryAttribute(currentMetadata.FileEntryBroker, FileEntryVersion.CurrentVersionInDatabase);
                                                AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttribute);
                                                ImageListView_UpdateItemThumbnailUpdateAllInvoke(currentMetadata.FileEntryBroker);

                                                FileEntryAttribute fileEntryAttributeHistoricalc = 
                                                    new FileEntryAttribute(fileSuposeToBeUpdated, FileEntryVersion.Historical);
                                                AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttributeHistoricalc);
                                            }
                                            else
                                            {
                                                Logger.Warn("Was not able to removed from queue, didn't exist in queue anymore: " + fileSuposeToBeUpdated);
                                            }
                                            #endregion
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Error(ex, "ThreadSaveMetadata - Check if all files was updated, if updated, add to verify queue");
                                        }
                                    }
                                }
                                #endregion

                                #region Clean up
                                lock (exiftoolSave_QueueSubsetMetadataToSaveLock) exiftoolSave_QueueSubsetMetadataToSave.Clear();
                                queueSubsetMetadataOrginalBeforeUserEdit.Clear();
                                mediaFilesUpdatedByExiftool.Clear();
                                #endregion

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
                            KryptonMessageBox.Show("Saving ThreadSaveMetadata crashed.\r\n" +
                                "The ThreadSaveMetadata queue was cleared. Nothing was saved.\r\n" +
                                "Exception message:" + ex.Message + "\r\n",
                                "Saving ThreadSaveMetadata failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                                        
                                        Metadata metadata = database.ReadMetadataFromCacheOrDatabase(fileEntryBroker);
                                        if (metadata == null)
                                        {
                                            metadataMicrosoftPhotos = databaseSourceReader.Read(broker, fileEntry.FileFullPath);
                                            if (metadataMicrosoftPhotos != null)
                                            {
                                                //Windows Live Photo Gallery writes direclty to database from sepearte thread when found
                                                database.Write(metadataMicrosoftPhotos);
                                                AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataMicrosoftPhotos);

                                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(metadataMicrosoftPhotos.FileFullPath, (DateTime)metadataMicrosoftPhotos.FileDateModified, FileEntryVersion.ExtractedNowUsingMicrosoftPhotos);
                                                //DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute); //Don't updated now, can block current process for updates
                                                AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttribute);
                                            }
                                        }
                                    }
                                    else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                                    lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) if (commonQueueReadMetadataFromSourceMicrosoftPhotos.Count > 0) commonQueueReadMetadataFromSourceMicrosoftPhotos.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar
                                }
                            }
                            #endregion

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) commonQueueReadMetadataFromSourceMicrosoftPhotos.Clear();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("ThreadCollectMetadataMicrosoftPhotos crashed.\r\n" + 
                                "Exception message:" + ex.Message + "\r\n",
                                "ThreadCollectMetadataMicrosoftPhotos failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                        #region DO the stuff
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
                                        
                                        Metadata metadata = database.ReadMetadataFromCacheOrDatabase(fileEntryBroker);
                                        if (metadata == null)
                                        {
                                            //Read from broker as Microsoft Photos, Windows Live Photo Gallery (Using NamedPipes)
                                            metadataWindowsLivePhotoGallery = databaseSourceReader.Read(broker, fileEntry.FileFullPath);
                                            if (metadataWindowsLivePhotoGallery != null)
                                            {
                                                database.Write(metadataWindowsLivePhotoGallery);
                                                AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataWindowsLivePhotoGallery);

                                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(metadataWindowsLivePhotoGallery.FileFullPath, (DateTime)metadataWindowsLivePhotoGallery.FileDateModified, FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery);
                                                //DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute); //Don't updated now, can block current process for updates
                                                AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttribute);
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
                            KryptonMessageBox.Show("ThreadCollectMetadataWindowsLiveGallery crashed.\r\n" + 
                                "Exception message:" + ex.Message + "\r\n", 
                                "ThreadCollectMetadataWindowsLiveGallery failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                        #region Do the Work
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
                                    FileEntryBroker current_FileEntryBrokerRegion;

                                    lock (commonQueueSaveToDatabaseRegionAndThumbnailLock)
                                    {
                                        current_FileEntryBrokerRegion = new FileEntryBroker(commonQueueSaveToDatabaseRegionAndThumbnail[indexSource].FileEntryBroker);
                                    }

                                    int fileIndexFound; //Loop all files and check more version of the file
                                    bool fileFoundInQueue_NeedCheckForMoreWithSameFilename; //Due to remove item, need loop queue once more
                                    bool fileNeedRemoveFromList; //If other ques not empty, only create Regions on cahced posters, when others queues emoty start working on hardrive
                                    bool fileFoundInList = false;

                                    do //Loop the queue, to find regions for WLPG, Photos, WebScraping, Exiftoool then save and remove all with same filename
                                    {
                                        fileFoundInQueue_NeedCheckForMoreWithSameFilename = false;
                                        fileNeedRemoveFromList = false;
                                        fileIndexFound = -1;

                                        Image image = null; //No image loaded

                                        int queueCount = CommonQueueSaveToDatabaseRegionAndThumbnailCountLock(); //Mark count that we will work with. 

                                        for (int thumbnailIndex = indexSource; thumbnailIndex < queueCount; thumbnailIndex++) //Not need to check already checked -> thumbnailIndex = indexSource
                                        {
                                            Metadata checkAgaistAll_MetadataActiveAlreadyCopy = null;
                                            lock (commonQueueSaveToDatabaseRegionAndThumbnailLock) { checkAgaistAll_MetadataActiveAlreadyCopy = commonQueueSaveToDatabaseRegionAndThumbnail[thumbnailIndex]; }

                                            //Find current file entry in queue, Exiftool, Microsoft Photos, Windows Live Gallery, etc...
                                            if (
                                                FilesCutCopyPasteDrag.IsFilenameEqual(
                                                    checkAgaistAll_MetadataActiveAlreadyCopy.FileFullPath,
                                                    current_FileEntryBrokerRegion.FileFullPath) &&
                                                checkAgaistAll_MetadataActiveAlreadyCopy.FileDateModified == current_FileEntryBrokerRegion.LastWriteDateTime)
                                            {
                                                fileIndexFound = thumbnailIndex;
                                                fileFoundInList = true;
                                                fileFoundInQueue_NeedCheckForMoreWithSameFilename = true;

                                                int coundMissingRegionThumbnails = 0;
                                                foreach (RegionStructure regionStructureCheckForThumbnails in checkAgaistAll_MetadataActiveAlreadyCopy.PersonalRegionList)
                                                {
                                                    if (regionStructureCheckForThumbnails.Thumbnail == null)
                                                        coundMissingRegionThumbnails++;
                                                }

                                                if (coundMissingRegionThumbnails == 0)
                                                {
                                                    #region When Face Regions not exists - (Remove from queue)
                                                    fileNeedRemoveFromList = true; //No regions to create, remove from queue
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Load Poser when Allowed and from fastest access

                                                    #region When Face Regions exists - Load cached poster (Remove from queue - when found poster thumbnail)
                                                    if (onlyDoWhatIsInCacheToAvoidHarddriveOverload)
                                                    {
                                                        image = PosterCacheRead(current_FileEntryBrokerRegion.FileFullPath);
                                                        if (image != null) fileNeedRemoveFromList = true;
                                                    }
                                                    #endregion
                                                    else
                                                    #region When Face Regions exists but missing - Load poster
                                                    {
                                                        bool fileExists;
                                                        bool fileHasCorrectDate;

                                                        if (File.Exists(current_FileEntryBrokerRegion.FileFullPath))
                                                        {
                                                            fileExists = true;

                                                            if (current_FileEntryBrokerRegion.Broker == MetadataBrokerType.WebScraping || //If source WebScraper, date and time will not match                                                        
                                                                File.GetLastWriteTime(current_FileEntryBrokerRegion.FileFullPath) == current_FileEntryBrokerRegion.LastWriteDateTime) //Check if the current Metadata are same as newest file 
                                                                fileHasCorrectDate = true;
                                                            else 
                                                                fileHasCorrectDate = false;
                                                        }
                                                        else
                                                        {
                                                            fileExists = false;
                                                            fileHasCorrectDate = false;
                                                        }

                                                        if (fileExists && fileHasCorrectDate)
                                                        {
                                                            #region Is Only Full size Thumbnails
                                                            bool isFullSizeThumbnail = true;
                                                            foreach (RegionStructure regionStructure in checkAgaistAll_MetadataActiveAlreadyCopy.PersonalRegionList)
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
                                                            #endregion

                                                            #region If only Full size Thumbnails, try load Poster Thumbnail
                                                            if (isFullSizeThumbnail)
                                                            {
                                                                image = databaseAndCacheThumbnailPoster.ReadThumbnailFromCacheOrDatabase(current_FileEntryBrokerRegion);

                                                                if (image == null)
                                                                {
                                                                    FileStatus fileStatus = FileHandler.GetFileStatus(current_FileEntryBrokerRegion.FileFullPath);
                                                                    image = LoadMediaCoverArtThumbnail(current_FileEntryBrokerRegion.FileFullPath, ThumbnailSaveSize, fileStatus);
                                                                }
                                                            }
                                                            #endregion

                                                            #region Load Poster, or trigger Image Download
                                                            string exceptionError = null;
                                                            bool didExceptionOccureWhenLoading = false;
                                                            try
                                                            {
                                                                if (image == null)
                                                                {
                                                                    FileStatus fileStatus = FileHandler.GetFileStatus(current_FileEntryBrokerRegion.FileFullPath);
                                                                    if (!fileStatus.IsInCloudOrVirtualOrOffline)
                                                                    {
                                                                        image = LoadMediaCoverArtPosterWithCache(current_FileEntryBrokerRegion.FileFullPath);
                                                                    }
                                                                    else if (fileStatus.IsInCloudOrVirtualOrOffline && !dontReadFilesInCloud) //Allow download from cloud
                                                                    {
                                                                        FileHandler.TouchOfflineFileToGetFileOnline(current_FileEntryBrokerRegion.FileFullPath);
                                                                    }
                                                                    else
                                                                    {
                                                                        //If all other fails, use Fallback on thumbnail
                                                                        if (current_FileEntryBrokerRegion.Broker != MetadataBrokerType.ExifTool)
                                                                        {
                                                                            image = databaseAndCacheThumbnailPoster.ReadThumbnailFromCacheOrDatabase(current_FileEntryBrokerRegion);
                                                                            if (image == null)
                                                                            {
                                                                                image = LoadMediaCoverArtThumbnail(current_FileEntryBrokerRegion.FileFullPath, ThumbnailSaveSize, fileStatus);
                                                                                if (image != null) fileStatus.FileErrorMessage += " Creating Poster Region Thumbnails for " + current_FileEntryBrokerRegion.Broker + " from Media file, with low resolution.";
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                exceptionError = ex.Message;
                                                                didExceptionOccureWhenLoading = true;
                                                                Logger.Error(ex, "ThreadReadMediaPosterSaveRegions - LoadMediaCoverArtPoster");
                                                            }
                                                            #endregion

                                                            #region Error Handling (In some cases, remove from queue)
                                                            if (image == null) //If failed load cover art, often occur after filed is moved or deleted
                                                            {
                                                                FileStatus fileStatus = FileHandler.GetFileStatus(
                                                                    current_FileEntryBrokerRegion.FileFullPath, checkLockedStatus: true,
                                                                    fileInaccessibleOrError: true, fileErrorMessage: exceptionError);

                                                                if (fileStatus.IsInCloudOrVirtualOrOffline)
                                                                {
                                                                    #region Download from cloud failed in time (Remove frome queue)
                                                                    if (FileHandler.IsOfflineFileTouchedAndFailedWithoutTimedOut(current_FileEntryBrokerRegion.FileFullPath))
                                                                    {
                                                                        fileNeedRemoveFromList = true;
                                                                        didExceptionOccureWhenLoading = true; //Was not able to download in time
                                                                        exceptionError += (string.IsNullOrWhiteSpace(exceptionError) ? "" : "\r\n") + "Download from clound timeout.";
                                                                    }
                                                                    #endregion

                                                                    #region If file not touch by now (by this, Exiftool or other processes) - Give error message
                                                                    if (dontReadFilesInCloud &&
                                                                        !FileHandler.IsOfflineFileTouchedAndWithoutTimeout(current_FileEntryBrokerRegion.FileFullPath))
                                                                    {
                                                                        fileNeedRemoveFromList = true;
                                                                        didExceptionOccureWhenLoading = true; //Not allowed to download cloud file
                                                                        exceptionError += (string.IsNullOrWhiteSpace(exceptionError) ? "" : "\r\n") +
                                                                            "File is offline and config is set up with 'Don't download files from cloud'.";
                                                                    }
                                                                    #endregion 
                                                                }

                                                                #region Error occured (Remove frome queue)
                                                                if (didExceptionOccureWhenLoading)
                                                                {
                                                                    fileNeedRemoveFromList = true;

                                                                    ImageListView_UpdateItemFileStatusInvoke(current_FileEntryBrokerRegion.FileFullPath, fileStatus);

                                                                    string writeErrorDesciption =
                                                                        "Issue: Failed loading poster for mediafile. " +
                                                                        "Was not able to update " + current_FileEntryBrokerRegion.Broker + " thumbnail for region(s) for the media file.\r\n" +
                                                                        "File Name:   " + current_FileEntryBrokerRegion.FileFullPath + "\r\n" +
                                                                        "File Status: " + FileHandler.ConvertFileStatusToText(fileStatus) + "\r\n" +
                                                                        "Error Message: " + exceptionError;
                                                                    Logger.Error(writeErrorDesciption);

                                                                    AddError(
                                                                        current_FileEntryBrokerRegion.Directory,
                                                                        current_FileEntryBrokerRegion.FileName,
                                                                        current_FileEntryBrokerRegion.LastWriteDateTime,
                                                                        AddErrorFileSystemRegion, AddErrorFileSystemRead,
                                                                        AddErrorFileSystemRead, AddErrorFileSystemRead,
                                                                        writeErrorDesciption);
                                                                }
                                                                #endregion
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            #region Error handling (Remove from queue)
                                                            fileNeedRemoveFromList = true; //File not exist or to old, remove from list
                                                            FileStatus fileStatus = FileHandler.GetFileStatus(
                                                                    current_FileEntryBrokerRegion.FileFullPath, checkLockedStatus: true,
                                                                    fileInaccessibleOrError: true);
                                                            if (!fileStatus.FileExists) fileStatus.FileErrorMessage = "File not found.";
                                                            if (!fileHasCorrectDate) fileStatus.FileErrorMessage += 
                                                                "File date has changed. Need create thumbnail for different date. Requested: " + 
                                                                current_FileEntryBrokerRegion.LastWriteDateTime.ToString() + " vs. File date: " +
                                                                File.GetLastWriteTime(current_FileEntryBrokerRegion.FileFullPath);

                                                            //if (fileStatus.FileInaccessibleOrError) fileStatus.FileErrorMessage = "Loading Media Poster failed.";
                                                            ImageListView_UpdateItemFileStatusInvoke(current_FileEntryBrokerRegion.FileFullPath, fileStatus);

                                                            #region Fallback on Low resolution
                                                            if (image == null) image = databaseAndCacheThumbnailPoster.ReadThumbnailFromCacheOrDatabase(current_FileEntryBrokerRegion);
                                                            if (image == null)
                                                            {
                                                                image = LoadMediaCoverArtThumbnail(current_FileEntryBrokerRegion.FileFullPath, ThumbnailSaveSize, fileStatus);
                                                                if (image != null) fileStatus.FileErrorMessage += " Creating Poster Region Thumbnails for " + current_FileEntryBrokerRegion.Broker + " from Media file, with low resolution.";
                                                            }
                                                            #endregion

                                                            string errorDesciption =
                                                                "Issue: Issue while create thumbnails for regions.\r\n" +
                                                                "File Name: " + current_FileEntryBrokerRegion.FileFullPath + "\r\n" +
                                                                "File Status: " + FileHandler.ConvertFileStatusToText(fileStatus) + "\r\n" +
                                                                "Error Message: " + fileStatus.FileErrorMessage;

                                                            Logger.Error(errorDesciption);

                                                            AddError(
                                                                current_FileEntryBrokerRegion.Directory,
                                                                current_FileEntryBrokerRegion.FileName,
                                                                current_FileEntryBrokerRegion.LastWriteDateTime,
                                                                AddErrorFileSystemRegion, AddErrorFileSystemRead,
                                                                AddErrorFileSystemRead, AddErrorFileSystemRead,
                                                                errorDesciption);
                                                            #endregion
                                                        }
                                                    }
                                                    #endregion
                                                    
                                                    #endregion

                                                    #region Image found - Save it - and update data grid (Remove from queue)
                                                    if (image != null) //Save regions when have image poster 
                                                    {
                                                        fileNeedRemoveFromList = true;

                                                        try
                                                        {
                                                            ThumbnailRegionHandler.SaveThumbnailsForRegionList_AlsoWebScarper(databaseAndCacheMetadataExiftool, checkAgaistAll_MetadataActiveAlreadyCopy, new Bitmap(image));
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Logger.Error(ex, "ThreadReadMediaPosterSaveRegions - SaveThumbnailsForRegioList");
                                                        }

                                                        FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(current_FileEntryBrokerRegion, FileEntryVersion.ExtractedNowUsingReadMediaFile);
                                                        DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute); //Updated Gridview

                                                        FileEntryAttribute fileEntryAttributeHistorical = new FileEntryAttribute(current_FileEntryBrokerRegion, FileEntryVersion.Historical);
                                                        DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttributeHistorical);
                                                    }
                                                    #endregion
                                                }

                                                if (fileFoundInQueue_NeedCheckForMoreWithSameFilename) break; //No need to search more.
                                            }
                                        } //end of loop: for (int thumbnailIndex = indexSource; thumbnailIndex < queueCount; thumbnailIndex++)

                                        #region If found and need need remove from List, then remove and check next
                                        if (fileNeedRemoveFromList && fileIndexFound > -1)
                                        {
                                            lock (commonQueueSaveToDatabaseRegionAndThumbnailLock)
                                            {                                        
                                                curentCommonQueueReadPosterAndSaveFaceThumbnailsCount--;
                                                commonQueueSaveToDatabaseRegionAndThumbnail.RemoveAt(fileIndexFound);
                                                //Check next FileEntry in queue, current will be next, due to removed an item
                                            }
                                        }
                                        else indexSource++; //Check next FileEntry in queue
                                        #endregion

                                    } while (fileFoundInQueue_NeedCheckForMoreWithSameFilename);

                                    if (!fileFoundInList) //Should never occur ;-)
                                    {
                                        string writeErrorDesciption = "ThreadReadMediaPosterSaveRegions, file not found list for updated:" + current_FileEntryBrokerRegion.FileFullPath;
                                        Logger.Error(writeErrorDesciption);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    KryptonMessageBox.Show("Saving Region Thumbnail crashed. The 'save region queue' was cleared.\r\n" + 
                                        "Nothing was saved.\r\n" + 
                                        "Exception message:" + ex.Message + "\r\n",
                                        "Saving Region Thumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                    lock (commonQueueSaveToDatabaseRegionAndThumbnailLock) { commonQueueSaveToDatabaseRegionAndThumbnail.Clear(); } //Avoid loop, due to unknown error
                                    Logger.Error(ex, "ThreadReadMediaPosterSaveRegions crashed");
                                    break; //Need jump out of while or endless loop
                                }

                                Thread.Sleep(10);
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

        #region Check ThreadQueues - IsFolderQueueReadMetadataFromSourceMicrosoftPhotos
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

        #region Check ThreadQueues - IsFileQueueReadMetadataFromSourceMicrosoftPhotos
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
                lock (exiftool_MediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in exiftool_MediaFilesNotInDatabase)
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
                lock (exiftool_MediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in exiftool_MediaFilesNotInDatabase)
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
                lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock)
                    foreach (Metadata metadata in exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify)
                    {
                        if (metadata.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                        {
                            folderInUse = true;
                            break;
                        }
                    }

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
            return folderInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFileInQueueSaveUsingExiftoolMetadata
        public bool IsFileInQueueSaveUsingExiftoolMetadataLock(string fullFilename)
        {
            bool fileInUse = false;
            if (!fileInUse)
                lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock)
                    foreach (Metadata metadata in exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(metadata.FileFullPath, fullFilename))
                        {
                            fileInUse = true;
                            break;
                        }
                    }
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
                lock (exiftoolSave_QueueSubsetMetadataToSaveLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSubsetMetadataToSave)
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

        #region Check ThreadQueues - IsFilesInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftool
        public bool IsFolderInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftoolLock(string folder)
        {
            try
            {
                lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
                {
                    foreach (FileEntryAttribute fileEntryAttributeCheck in commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails)
                    {
                        if (fileEntryAttributeCheck.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase)) return true;
                    }
                }

                lock (commonQueueReadMetadataFromSourceExiftoolLock)
                {
                    foreach (FileEntry fileEntryCheck in commonQueueReadMetadataFromSourceExiftool)
                    {
                        if (fileEntryCheck.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase)) return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        #endregion

        #region Check ThreadQueues - IsFilesInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftool
        public bool IsFileInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftoolLock(string fileFullPath)
        {
            try
            {
                lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
                {
                    foreach (FileEntryAttribute fileEntryAttributeCheck in commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntryAttributeCheck.FileFullPath, fileFullPath)) return true;
                    }
                }

                lock (commonQueueReadMetadataFromSourceExiftoolLock)
                {
                    foreach (FileEntry fileEntryCheck in commonQueueReadMetadataFromSourceExiftool)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(fileEntryCheck.FileFullPath, fileFullPath)) return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }
        #endregion

        #region Check ThreadQueues - IsFilesInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftool
        public bool IsFileInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftoolLock(FileEntryAttribute fileEntryAttribute)
        {
            try
            {
                lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
                    if (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Contains(fileEntryAttribute)) return true;

                lock (commonQueueReadMetadataFromSourceExiftoolLock)
                    if (commonQueueReadMetadataFromSourceExiftool.Contains(fileEntryAttribute.FileEntry)) return true;

                return IsFileInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftoolLock(fileEntryAttribute.FileFullPath);
            }
            catch
            {
            }
            return false;
        }
        #endregion

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

        #region Check ThreadQueues - IsFileInAnyQueue - Blocking Rename - Filename
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
            if (!fileInUse) fileInUse = IsFileInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftoolLock(fullFilename);

            return fileInUse;
        }
        #endregion

        #region Check ThreadQueues - IsFolderInAnyQueue - Blocking Rename - Folder
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
            if (!folderInUse) folderInUse = IsFolderInQueueLazyloadingAllSourcesOrReadMetadataFromSourceExiftoolLock(folder);
            return folderInUse;
        }
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

                                #region File in Queue? - Find a file ready for rename
                                lock (commonQueueRenameMediaFilesLock)
                                {

                                    foreach (KeyValuePair<string, string> keyValuePair in commonQueueRenameMediaFiles)
                                    {
                                        fullFilename = keyValuePair.Key;
                                        renameVaiable = keyValuePair.Value;
                                        fileInUse = IsFileInAnyQueueLock(fullFilename);
                                        if (!fileInUse) break; //File not in use found, start rename it
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
                                            string error = 
                                                "Failed rename " + fullFilename + " to : New name is unknown(missing metadata)";
                                            Logger.Error(error);
                                            
                                            FileStatus fileStatus = FileHandler.GetFileStatus(
                                                fullFilename, checkLockedStatus: true,
                                                fileInaccessibleOrError: true, fileErrorMessage: error);
                                            ImageListView_UpdateItemFileStatusInvoke(fullFilename, fileStatus);

                                            error =
                                                "Issue: Failed to rename file. Missing metadata for creating new filename\r\n" +
                                                "File Name:   " + fullFilename + "\r\n" +
                                                "File Status: " + fileStatus.ToString() + "\r\n" +
                                                "Error Message: Missing Metatdata, can't create new filename";

                                            Logger.Error(error);

                                            AddError(
                                                Path.GetDirectoryName(fullFilename), Path.GetFileName(fullFilename), File.GetLastWriteTime(fullFilename),
                                                AddErrorFileSystemRegion, AddErrorFileSystemMove, fullFilename, "New name is unknown (missing metadata)",
                                                error);
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
                            KryptonMessageBox.Show("Saving ThreadRename crashed.\r\n" +
                                "The ThreadRename queue was cleared. Nothing was saved.\r\n" + 
                                "Exception message:" + ex.Message + "\r\n", 
                                "Saving ThreadRename failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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

        #region MapNomnatatim
        
        #region MapNomnatatim - LazyLoading - Add Read Queue
        public void AddQueueLazyLoadingMapNomnatatimLock(FileEntryAttribute fileEntryAttribute, bool forceReloadUsingReverseGeocoder)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonLazyLoadingMapNomnatatimLock)
            {
                if (!commonLazyLoadingMapNomnatatim.ContainsKey(fileEntryAttribute)) commonLazyLoadingMapNomnatatim.Add(fileEntryAttribute, forceReloadUsingReverseGeocoder);
            }
        }
        #endregion

        #region Media Thumbnail - LazyLoading - Add Read Queue
        public void AddQueueLazyLoadingMapNomnatatimLock(List<FileEntryAttribute> fileEntryAttributes, bool forceReloadUsingReverseGeocoder)
        {
            if (fileEntryAttributes == null) return;
            lock (commonLazyLoadingMapNomnatatimLock)
            {
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes)
                {
                    if (!commonLazyLoadingMapNomnatatim.ContainsKey(fileEntryAttribute)) commonLazyLoadingMapNomnatatim.Add(fileEntryAttribute, forceReloadUsingReverseGeocoder);
                }
            }
        }
        #endregion

        #region MapNomnatatim - LazyLoading - Thread 
        //private static readonly Object _ThreadLazyLoadingMapNomnatatimLock = new Object();
        //private static Thread _ThreadLazyLoadingMapNomnatatim = null; //
        public void ThreadQueueLazyLoadingMapNomnatatim()
        {
            if (!DataGridViewHandler.GetIsAgregated(dataGridViewMap)) return;
            if (DataGridViewHandler.GetIsPopulating(dataGridViewMap)) return;
            try
            {
                lock (_ThreadLazyLoadingMapNomnatatimLock) if (_ThreadLazyLoadingMapNomnatatim != null ||
                        CommonQueueLazyLoadingMapNomnatatimDirty() <= 0) return;

                lock (_ThreadLazyLoadingMapNomnatatimLock)
                {
                    _ThreadLazyLoadingMapNomnatatim = new Thread(() =>
                    {
                        #region
                        try
                        {
                            Logger.Trace("ThreadQueueLazyLoadingMapNomnatatim - started");
                            int count = CommonQueueLazyLoadingMapNomnatatimLock();
                            while (count > 0 && !GlobalData.IsApplicationClosing)
                            {
                                count--;

                                #region Do the work
                                while (!GlobalData.IsApplicationClosing && CommonQueueLazyLoadingMapNomnatatimLock() > 0) //In case some more added to the queue
                                {
                                    bool isPopulated = false;
                                    KeyValuePair<FileEntryAttribute,bool> fileEntryAttributeAndAllowUseMetadataLocationInfo;

                                    lock (commonLazyLoadingMapNomnatatimLock) fileEntryAttributeAndAllowUseMetadataLocationInfo = 
                                        commonLazyLoadingMapNomnatatim.First();

                                    int columnIndex = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridViewMap, 
                                        fileEntryAttributeAndAllowUseMetadataLocationInfo.Key, out FileEntryVersionCompare fileEntryVersionCompare);

                                    if (columnIndex != -1)
                                    {
                                        isPopulated = DataGridViewHandler.IsColumnPopulated(dataGridViewMap, columnIndex);
                                        if (isPopulated) DataGridView_Populate_MapLocation(
                                            fileEntryAttributeAndAllowUseMetadataLocationInfo.Key,
                                            fileEntryAttributeAndAllowUseMetadataLocationInfo.Value);
                                    }
                                    
                                    lock (commonLazyLoadingMapNomnatatimLock) 
                                        if (commonLazyLoadingMapNomnatatim.Count > 0) 
                                            commonLazyLoadingMapNomnatatim.Remove(fileEntryAttributeAndAllowUseMetadataLocationInfo.Key); //Remove from queue after read. Otherwise wrong text in status bar

                                    if (!isPopulated && fileEntryVersionCompare != FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch) 
                                        AddQueueLazyLoadingMapNomnatatimLock(
                                            fileEntryAttributeAndAllowUseMetadataLocationInfo.Key,
                                            fileEntryAttributeAndAllowUseMetadataLocationInfo.Value);
                                }
                                #endregion
                            }

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) commonQueueReadMetadataFromSourceMicrosoftPhotos.Clear();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("ThreadQueueLazyLoadingMapNomnatatim crashed." + 
                                "\r\nException message:" + ex.Message + "\r\n", 
                                "ThreadQueueLazyLoadingMapNomnatatim failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            Logger.Error(ex, "ThreadQueueLazyLoadingMapNomnatatim failed");
                        }
                        finally
                        {
                            lock (_ThreadLazyLoadingMapNomnatatimLock) _ThreadLazyLoadingMapNomnatatim = null;
                            Logger.Trace("ThreadQueueLazyLoadingMapNomnatatim - ended");
                        }
                        #endregion

                    });

                    lock (_ThreadLazyLoadingMapNomnatatimLock) if (_ThreadLazyLoadingMapNomnatatim != null)
                    {
                        _ThreadLazyLoadingMapNomnatatim.Priority = threadPriority;
                        _ThreadLazyLoadingMapNomnatatim.Start();
                    }
                    else Logger.Error("_ThreadLazyLoadingMapNomnatatim was not able to start");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "_ThreadLazyLoadingMapNomnatatim.Start failed. ");
            }

        }
        #endregion


        #endregion
    }
}
