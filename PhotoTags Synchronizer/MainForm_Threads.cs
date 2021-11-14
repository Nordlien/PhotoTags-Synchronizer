using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using MetadataLibrary;
using DataGridViewGeneric;
using NLog;
using Exiftool;
using System.Diagnostics;
using FileDateTime;
using Thumbnails;
using System.Collections.Specialized;
using FileHandeling;
using System.Threading.Tasks;
using Manina.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        private AutoResetEvent WaitExittoolReadCacheThread = null;
        private AutoResetEvent WaitThumbnailReadCacheThread = null;

        private static ThreadPriority threadPriority = ThreadPriority.Lowest;
        #region Thread variables
        private static readonly Object _ThreadCacheSelectedFastReadLock = new Object();
        private static Thread _ThreadCacheSelectedFastRead = null; //

        private static readonly Object _ThreadLazyLoadingMetadataLock = new Object();
        private static Thread _ThreadLazyLoadingMetadata = null; //

        private static readonly Object _ThreadLazyLoadingThumbnailLock = new Object();
        private static Thread _ThreadLazyLoadingThumbnail = null; //

        private static readonly Object __ThreadCollectMetadataExiftoolLock = new Object();
        private static Thread _ThreadCollectMetadataExiftool = null; 
        
        private static readonly Object _ThreadSaveThumbnailLock = new Object();
        private static Thread _ThreadSaveThumbnail = null; //

        private static readonly Object _ThreadMicrosoftPhotosLock = new Object();
        private static Thread _ThreadMicrosoftPhotos = null; //

        private static readonly Object _ThreadWindowsLiveGalleryLock = new Object();
        private static Thread _ThreadWindowsLiveGallery = null; //

        private static readonly Object _ThreadThumbnailRegionLock = new Object();
        private static Thread _ThreadThumbnailRegion = null; //

        private static readonly Object _ThreadSaveMetadataLock = new Object();
        private static Thread _ThreadSaveMetadata = null; //

        private static readonly Object _ThreadRenameMedafilesLock = new Object();
        private static Thread _ThreadRenameMedafiles = null; //
        #endregion

        #region Queue listes
        private static List<FileEntryAttribute> commonQueueLazyLoadingMetadata = new List<FileEntryAttribute>();
        private static readonly Object commonQueueLazyLoadingMetadataLock = new Object();

        private static List<FileEntryAttribute> commonQueueLazyLoadingThumbnail = new List<FileEntryAttribute>();
        private static readonly Object commonQueueLazyLoadingThumbnailLock = new Object();

        //Region "Face" thumbnails
        private static List<Metadata> commonQueueReadPosterAndSaveFaceThumbnails = new List<Metadata>();
        private static readonly Object commonQueueReadPosterAndSaveFaceThumbnailsLock = new Object();

        //Thumbnail
        private static List<FileEntryImage> commonQueueSaveThumbnailToDatabase = new List<FileEntryImage>();
        private static readonly Object commonQueueSaveThumbnailToDatabaseLock = new Object();

        //Microsoft Photos
        private static List<FileEntry> commonQueueReadMetadataFromMicrosoftPhotos = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromMicrosoftPhotosLock = new Object();

        //Windows Live Photo Gallery
        private static List<FileEntry> commonQueueReadMetadataFromWindowsLivePhotoGallery = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromWindowsLivePhotoGalleryLock = new Object();

        //Exif
        private static List<FileEntry> commonQueueReadMetadataFromExiftool = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromExiftoolLock = new Object();

        private static List<FileEntry> mediaFilesNotInDatabase = new List<FileEntry>(); //It's globale, just to manage to show count status
        private static readonly Object mediaFilesNotInDatabaseLock = new Object();

        private static List<Metadata> commonQueueSaveMetadataUpdatedByUser = new List<Metadata>();
        private static readonly Object commonQueueSaveMetadataUpdatedByUserLock = new Object();
        private static List<Metadata> commonQueueSubsetMetadataToSave = new List<Metadata>();
        private static readonly Object commonQueueSubsetMetadataToSaveLock = new Object();

        private static List<Metadata> commonOrigialMetadataBeforeUserUpdate = new List<Metadata>();
        private static readonly Object commonOrigialMetadataBeforeUserUpdateLock = new Object();
        private static List<Metadata> commonQueueMetadataWrittenByExiftoolReadyToVerify = new List<Metadata>();
        private static readonly Object commonQueueMetadataWrittenByExiftoolReadyToVerifyLock = new Object();

        //Rename
        private static Dictionary<string, string> commonQueueRename = new Dictionary<string, string>();
        private static readonly Object commonQueueRenameLock = new Object();

        //Error handling
        private static Dictionary<string, string> queueErrorQueue = new Dictionary<string, string>();
        private static readonly Object queueErrorQueueLock = new Object();
        #endregion

        #region Get Count of items in Queue with Lock
        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueLazyLoadingMetadataCountLock()
        {
            lock (commonQueueLazyLoadingMetadataLock) return commonQueueLazyLoadingMetadata.Count;
        }

        private int CommonQueueLazyLoadingMetadataCountDirty()
        {
            lock (commonQueueLazyLoadingMetadataLock) return commonQueueLazyLoadingMetadata.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueLazyLoadingThumbnailCountLock()
        {
            lock (commonQueueLazyLoadingThumbnailLock) return commonQueueLazyLoadingThumbnail.Count;
        }

        private int CommonQueueLazyLoadingThumbnailCountDirty()
        {
            lock (commonQueueLazyLoadingThumbnailLock) return commonQueueLazyLoadingThumbnail.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueReadPosterAndSaveFaceThumbnailsCountLock()
        {
            lock (commonQueueReadPosterAndSaveFaceThumbnailsLock) return commonQueueReadPosterAndSaveFaceThumbnails.Count;
        }

        private int CommonQueueReadPosterAndSaveFaceThumbnailsCountDirty()
        {
            return commonQueueReadPosterAndSaveFaceThumbnails.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueSaveThumbnailToDatabaseCountLock()
        {
            lock (commonQueueSaveThumbnailToDatabaseLock) return commonQueueSaveThumbnailToDatabase.Count;
        }

        private int CommonQueueSaveThumbnailToDatabaseCountDirty()
        {
            return commonQueueSaveThumbnailToDatabase.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueReadMetadataFromMicrosoftPhotosCountLock()
        {
            lock (commonQueueReadMetadataFromMicrosoftPhotosLock) return commonQueueReadMetadataFromMicrosoftPhotos.Count;
        }

        private int CommonQueueReadMetadataFromMicrosoftPhotosCountDirty()
        {
            return commonQueueReadMetadataFromMicrosoftPhotos.Count;
        }

        private int CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock()
        {
            lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) return commonQueueReadMetadataFromWindowsLivePhotoGallery.Count;
        }

        private int CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty()
        {
            return commonQueueReadMetadataFromWindowsLivePhotoGallery.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueReadMetadataFromExiftoolCountLock()
        {
            lock (commonQueueReadMetadataFromExiftoolLock) return commonQueueReadMetadataFromExiftool.Count;
        }

        private int CommonQueueReadMetadataFromExiftoolCountDirty()
        {
            return commonQueueReadMetadataFromExiftool.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueSaveMetadataUpdatedByUserCountLock()
        {
            lock (commonQueueSaveMetadataUpdatedByUserLock) return commonQueueSaveMetadataUpdatedByUser.Count;
        }

        private int CommonQueueSaveMetadataUpdatedByUserCountDirty()
        {
            return commonQueueSaveMetadataUpdatedByUser.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int MediaFilesNotInDatabaseCountLock()
        {
            lock (mediaFilesNotInDatabaseLock) return mediaFilesNotInDatabase.Count;
        }

        private int MediaFilesNotInDatabaseCountDirty()
        {
            return mediaFilesNotInDatabase.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        
        private int CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty()
        {
            return commonQueueMetadataWrittenByExiftoolReadyToVerify.Count;
        }

        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueRenameCountLock()
        {
            lock (commonQueueRenameLock) return commonQueueRename.Count;
        }

        private int CommonQueueRenameCountDirty()
        {
            return commonQueueRename.Count;
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
            ThreadCollectMetadataExiftool();            //Read from cache first, then exifdata, 
            ThreadCollectMetadataMicrosoftPhotos();
            ThreadCollectMetadataWindowsLiveGallery();
            ThreadSaveMetadata();
            ThreadSaveThumbnail();
            ThreadReadMediaPosterSaveRegions();

            ThreadRename();
            ThreadLazyLoadingDataGridViewMetadata();
            ThreadLazyLazyLoadingDataGridViewThumbnail();
        }

        /// <summary>
        /// Check if any thread is running
        /// </summary>
        /// <returns>True if a thread is processing</returns>
        private bool IsAnyThreadRunning()
        {
            return (
                (_ThreadSaveThumbnail != null && _ThreadSaveThumbnail.IsAlive) ||
                (_ThreadCollectMetadataExiftool != null && _ThreadCollectMetadataExiftool.IsAlive) ||
                (_ThreadWindowsLiveGallery != null && _ThreadWindowsLiveGallery.IsAlive) ||
                (_ThreadMicrosoftPhotos != null && _ThreadMicrosoftPhotos.IsAlive) ||
                (_ThreadThumbnailRegion != null && _ThreadThumbnailRegion.IsAlive) ||
                (_ThreadSaveMetadata != null && _ThreadSaveMetadata.IsAlive)
                );
        }

        /// <summary>
        /// Check if any thread is running Except ThreadThumbnailRegion
        /// This is used by ThreadThumbnailRegion wait until free resources to use
        /// </summary>
        /// <returns>True if a thread is processing (Except ThreadThumbnailRegion)</returns>
        private bool IsThreadRunningExcept_ThreadThumbnailRegion()
        {
            return
                CommonQueueSaveThumbnailToDatabaseCountDirty() > 0 ||
                CommonQueueReadMetadataFromExiftoolCountDirty() > 0 ||
                CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty() > 0 ||
                CommonQueueReadMetadataFromMicrosoftPhotosCountDirty() > 0 ||
                //queueThumbnailRegion.Count > 0 ||
                CommonQueueSaveMetadataUpdatedByUserCountDirty() > 0;
        }

        /// <summary>
        /// On 32bit version, out of meomory occures oftem, was needed to wait until qeue was ended to free up reasources
        /// </summary>
        private void TriggerAutoResetEventQueueEmpty()
        {
            if (CommonQueueSaveThumbnailToDatabaseCountDirty() == 0 &&
                CommonQueueReadMetadataFromExiftoolCountDirty() == 0 &&
                CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty() == 0 &&
                CommonQueueReadMetadataFromMicrosoftPhotosCountDirty() == 0 &&
                CommonQueueReadPosterAndSaveFaceThumbnailsCountDirty() == 0 &&
                CommonQueueSaveMetadataUpdatedByUserCountDirty() == 0
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

            lock (commonQueueReadMetadataFromExiftoolLock) commonQueueReadMetadataFromExiftool.Clear();
            lock (commonQueueReadMetadataFromMicrosoftPhotosLock) commonQueueReadMetadataFromMicrosoftPhotos.Clear();
            lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) commonQueueReadMetadataFromWindowsLivePhotoGallery.Clear();
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
        public void CacheFileEntries(HashSet<FileEntry> fileEntries, string selectedFolder)
        {            
            try
            {
                bool isThreadRunning;
                int retry = 200;
                do
                {
                    lock (_ThreadCacheSelectedFastReadLock) isThreadRunning = (_ThreadCacheSelectedFastRead != null);
                    if (isThreadRunning)
                    {
                        MetadataDatabaseCache.StopCaching = true;
                        ThumbnailDatabaseCache.StopCaching = true;
                        Task.Delay(10).Wait(); //Wait thread stopping
                        Logger.Debug("CacheFileEntries - sleep(100) - ThreadRunning is running");
                    }
                } while (isThreadRunning && retry-- > 0);

                lock (_ThreadCacheSelectedFastReadLock) isThreadRunning = (_ThreadCacheSelectedFastRead != null);

                if (isThreadRunning) return; //Still running, give up

                lock (_ThreadCacheSelectedFastReadLock)
                {
                    _ThreadCacheSelectedFastRead = new Thread(() =>
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
                            KryptonMessageBox.Show("CacheFileEntries crashed.", "CacheFileEntries failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Logger.Error(ex, "CacheFileEntries crashed");
                        }
                        finally
                        {
                            MetadataDatabaseCache.StopCaching = false;
                            ThumbnailDatabaseCache.StopCaching = false;
                            lock (_ThreadCacheSelectedFastReadLock) _ThreadCacheSelectedFastRead = null;
                        }
                        #endregion
                    });
                }

                lock (_ThreadCacheSelectedFastReadLock) if (_ThreadCacheSelectedFastRead != null)
                {
                    _ThreadCacheSelectedFastRead.Priority = threadPriority;
                    _ThreadCacheSelectedFastRead.Start();                    
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

        #region LazyLoadingDataGridView - DataGridView - Metadata

        #region LazyLoadingDataGridView - ThreadLazyLoadingQueueSize()
        public int ThreadLazyLoadingDataGridViewQueueSizeDirty()
        {
            return
                MediaFilesNotInDatabaseCountDirty() +
                CommonQueueLazyLoadingThumbnailCountDirty() +
                CommonQueueLazyLoadingMetadataCountDirty() +
                CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty() +
                CommonQueueReadMetadataFromMicrosoftPhotosCountDirty() +
                CommonQueueReadMetadataFromExiftoolCountDirty();
                //CommonQueueSaveThumbnailToDatabaseCountDirty() +
                //CommonQueueReadPosterAndSaveFaceThumbnailsCountDirty() +
                //CommonQueueSaveMetadataUpdatedByUserCountDirty();
        }
        #endregion 

        #region LazyLoadingDataGridView - Metadata - AddQueue - Read from Cache, then Database, then Source and Save
        private static Dictionary<string, DateTime> dontCheckAndReadOfflineFilesTwice = new Dictionary<string, DateTime>();
        public void AddQueueLazyLoadingDataGridViewMetadataReadToCacheOrUpdateFromSoruce(FileEntry fileEntry)
        {
            //When file is DELETE, LastWriteDateTime become null
            if (fileEntry.LastWriteDateTime != null)
            {
                Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                if (metadata == null) AddQueueExiftoolLock(fileEntry); //If Metadata don't exist in database, put it in read queue
                else ImageListViewSetItemDirty(fileEntry.FileFullPath); //Refresh ImageListView with metadata

                //if (!databaseAndCacheMetadataExiftool.IsMetadataInCache(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool)) AddQueueExiftoolLock(fileEntry);
                if (!databaseAndCacheMetadataMicrosoftPhotos.IsMetadataInCache(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos))) AddQueueMicrosoftPhotosLock(fileEntry);
                if (!databaseAndCacheMetadataWindowsLivePhotoGallery.IsMetadataInCache(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery))) AddQueueWindowsLivePhotoGalleryLock(fileEntry);
                
            }
            else
            {
                Debug.WriteLine("AddQueueAllUpadtedFileEntry was delete: (Check why), rename of exiftool maybe, need back then... " + fileEntry.FileFullPath);
            }

            TriggerAutoResetEventQueueEmpty();
        }
        #endregion

        #region LazyLoadingDataGridView - Metadata - AddQueue - Only Read
        public void AddQueueLazyLoadningDataGridViewMetadataLock(List<FileEntryAttribute> fileEntryAttributes)
        {
            if (fileEntryAttributes == null) return;
            lock (commonQueueLazyLoadingMetadataLock)
            {
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes)
                {
                    if (!commonQueueLazyLoadingMetadata.Contains(fileEntryAttribute)) commonQueueLazyLoadingMetadata.Add(fileEntryAttribute);
                }
            }
        }
        #endregion

        #region LazyLoadning - Metadata - Thread 
        public void ThreadLazyLoadingDataGridViewMetadata()
        {
            try
            {
                lock (_ThreadLazyLoadingMetadataLock) if (_ThreadLazyLoadingMetadata != null || CommonQueueLazyLoadingMetadataCountDirty() <= 0) return;
                lock (_ThreadLazyLoadingMetadataLock)
                {
                    _ThreadLazyLoadingMetadata = new Thread(() =>
                    {
                        #region
                        try
                        {
                            Logger.Trace("ThreadLazyLoadningMetadata - started");
                            int count = CommonQueueLazyLoadingMetadataCountLock();
                            while (count > 0 && CommonQueueLazyLoadingMetadataCountLock() > 0 && !GlobalData.IsApplicationClosing)
                            {
                                count--;
                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(commonQueueLazyLoadingMetadata[0]);

                                bool readColumn = false;
                                switch (fileEntryAttribute.FileEntryVersion)
                                {
                                    case FileEntryVersion.Current:
                                        readColumn = true;
                                        break;
                                    case FileEntryVersion.Error:
                                        if ((showWhatColumns & ShowWhatColumns.ErrorColumns) > 0) readColumn = true;
                                        break;
                                    case FileEntryVersion.Historical:
                                        if ((showWhatColumns & ShowWhatColumns.HistoryColumns) > 0) readColumn = true;
                                        break;
                                    case FileEntryVersion.AutoCorrect:
                                        readColumn = true;
                                        break;
                                    default:
                                        throw new Exception("Not implemeneted");
                                }

                                if (readColumn)
                                {
                                    MetadataBrokerType metadataBrokerType = MetadataBrokerType.ExifTool;
                                    //If error Broker type attribute, set correct Broker type
                                    if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.Error) metadataBrokerType |= MetadataBrokerType.ExifToolWriteError;
                                    if (databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntryAttribute, metadataBrokerType)) == null)
                                    {
                                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, metadataBrokerType));
                                        //If metadata found, check if Thumnbail for regions are created, if the application stopped during this process, thumbnail missing
                                        if (metadata != null && metadata.PersonalRegionIsThumbnailMissing()) AddQueueCreateRegionFromPosterLock(metadata);
                                    }

                                    if (databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.MicrosoftPhotos)) == null)
                                    {
                                        Metadata metadata = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.MicrosoftPhotos));
                                        if (metadata != null && metadata.PersonalRegionIsThumbnailMissing()) AddQueueCreateRegionFromPosterLock(metadata);
                                    }

                                    if (databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.WindowsLivePhotoGallery)) == null)
                                    {
                                        Metadata metadata = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.WindowsLivePhotoGallery));
                                        if (metadata != null && metadata.PersonalRegionIsThumbnailMissing()) AddQueueCreateRegionFromPosterLock(metadata);
                                    }
                                }

                                lock (commonQueueLazyLoadingMetadataLock) commonQueueLazyLoadingMetadata.RemoveAt(0);
                                PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(fileEntryAttribute);

                                if (GlobalData.IsApplicationClosing) lock (commonQueueLazyLoadingMetadataLock) commonQueueLazyLoadingMetadata.Clear();
                                TriggerAutoResetEventQueueEmpty();
                            }
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("LazyLoadingMetadata crashed. The 'LazyLoadingMetadata' was cleared.", "Saving Region Thumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lock (commonQueueLazyLoadingMetadataLock) commonQueueLazyLoadingMetadata.Clear();  //Avoid loop, due to unknown error
                            Logger.Error(ex, "ThreadLazyLoadningMetadata");
                        }
                        finally
                        {
                            lock (_ThreadLazyLoadingMetadataLock) _ThreadLazyLoadingMetadata = null;
                            Logger.Trace("ThreadLazyLoadningMetadata - ended");
                        }
                        #endregion
                    });

                    lock (_ThreadLazyLoadingMetadataLock) if (_ThreadLazyLoadingMetadata != null)
                    {
                        _ThreadLazyLoadingMetadata.Priority = threadPriority;
                        _ThreadLazyLoadingMetadata.Start();                        
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

        #endregion

        #region LazyLoadingDataGridView - Thumbnail

        #region LazyLoadingDataGridView - Add Queue
        public void AddQueueLazyLoadningDataGridViewThumbnailLock(List<FileEntryAttribute> fileEntryAttributes)
        {
            if (fileEntryAttributes == null) return;
            lock (commonQueueLazyLoadingThumbnailLock)
            {
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes)
                {
                    if (!commonQueueLazyLoadingThumbnail.Contains(fileEntryAttribute)) commonQueueLazyLoadingThumbnail.Add(fileEntryAttribute);
                }
            }
        }
        #endregion

        #region LazyLoadingDataGridView - Thumbnail - Thread LazyLoadning
        public void ThreadLazyLazyLoadingDataGridViewThumbnail()
        {
            try
            {
                if (WaitThumbnailReadCacheThread != null && CommonQueueLazyLoadingThumbnailCountDirty() == 0) WaitThumbnailReadCacheThread.Set();

                lock (_ThreadLazyLoadingThumbnailLock)
                    if (GlobalData.IsStopAndEmptyThumbnailQueueRequest || _ThreadLazyLoadingThumbnail != null || CommonQueueLazyLoadingThumbnailCountDirty() <= 0) return;

                lock (_ThreadLazyLoadingThumbnailLock)
                {
                    _ThreadLazyLoadingThumbnail = new Thread(() =>
                    {
                        #region
                        try
                        {
                            int count = CommonQueueLazyLoadingThumbnailCountLock();
                            while (count > 0 && CommonQueueLazyLoadingThumbnailCountLock() > 0 && !GlobalData.IsStopAndEmptyThumbnailQueueRequest && !GlobalData.IsApplicationClosing)
                            {
                                count--;

                                if (!GlobalData.IsStopAndEmptyThumbnailQueueRequest && commonQueueLazyLoadingThumbnail.Count > 0) //In case clear, due to user screen interaction
                                {
                                    FileEntryAttribute fileEntryAttribute;
                                    lock (commonQueueLazyLoadingThumbnailLock) fileEntryAttribute = commonQueueLazyLoadingThumbnail[0];

                                    if (!databaseAndCacheThumbnail.DoesThumbnailExistInCache(fileEntryAttribute))
                                    {
                                        Image image = databaseAndCacheThumbnail.ReadThumbnailFromCacheOrDatabase(fileEntryAttribute.FileEntry);
                                        if (image != null) UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(fileEntryAttribute, image);
                                    }
                                }
                                lock (commonQueueLazyLoadingThumbnailLock) commonQueueLazyLoadingThumbnail.RemoveAt(0);

                                if (GlobalData.IsApplicationClosing || GlobalData.IsStopAndEmptyThumbnailQueueRequest)
                                    lock (commonQueueLazyLoadingThumbnailLock) commonQueueLazyLoadingThumbnail.Clear();
                            }
                            if (WaitThumbnailReadCacheThread != null) WaitThumbnailReadCacheThread.Set();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("LazyLoadningThumbnail crashed.", "LazyLoadningThumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lock (commonQueueLazyLoadingThumbnailLock) commonQueueLazyLoadingThumbnail.Clear(); //Avoid loop, due to unknown error
                            Logger.Error(ex, "ThreadLazyLoadningThumbnail thread failed. ");
                        }
                        finally
                        {
                            lock (_ThreadLazyLoadingThumbnailLock) _ThreadLazyLoadingThumbnail = null;
                        }
                        #endregion
                    });

                }

                lock (_ThreadLazyLoadingThumbnailLock) if (_ThreadLazyLoadingThumbnail != null)
                {
                    _ThreadLazyLoadingThumbnail.Priority = threadPriority;
                    _ThreadLazyLoadingThumbnail.Start();                    
                }
                else Logger.Error("_ThreadLazyLoadingThumbnail was not able to start");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ThreadLazyLoadningThumbnail.Start failed. ");
            }
        }
        #endregion

        #region AddQueue - AddQueueSaveThumbnailMedia
        public void AddQueueSaveThumbnailMediaLock(FileEntryImage fileEntryImage)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueSaveThumbnailToDatabaseLock)
            {
                if (!commonQueueSaveThumbnailToDatabase.Contains(fileEntryImage))
                {
                    commonQueueSaveThumbnailToDatabase.Add(fileEntryImage);
                }
                else if (fileEntryImage.Image != null) //If image are already read, save it
                {
                    int index = commonQueueSaveThumbnailToDatabase.IndexOf(fileEntryImage);
                    if (index >= 0) commonQueueSaveThumbnailToDatabase[index].Image = fileEntryImage.Image; //Image has been found, updated the entry, so image will not be needed to load again.
                }
            }
        }
        #endregion

        #region Thread - SaveThumbnail
        public void ThreadSaveThumbnail()
        {
            try
            {
                lock (_ThreadSaveThumbnailLock) if (_ThreadSaveThumbnail != null || CommonQueueSaveThumbnailToDatabaseCountDirty() <= 0) return;

                lock (_ThreadSaveThumbnailLock)
                {
                    _ThreadSaveThumbnail = new Thread(() =>
                    {
                        #region While data in thread
                        try
                        {
                            Logger.Trace("ThreadSaveThumbnail - started");
                            int count = CommonQueueSaveThumbnailToDatabaseCountLock();
                            while (count > 0 && CommonQueueSaveThumbnailToDatabaseCountLock() > 0 && !GlobalData.IsApplicationClosing) //In case some more added to the queue or App will close
                            {
                                count--;

                                if (CommonQueueReadMetadataFromExiftoolCountLock() > 0) break; //Wait all metadata readfirst
                                if (CommonQueueSaveMetadataUpdatedByUserCountLock() > 0) break; //Write first, read later on...

                                try
                                {
                                    FileEntryImage fileEntryImage;
                                    lock (commonQueueSaveThumbnailToDatabaseLock)
                                    {
                                        fileEntryImage = new FileEntryImage(commonQueueSaveThumbnailToDatabase[0]);
                                    }

                                    try
                                    {
                                        if (fileEntryImage.Image == null)
                                        {
                                            fileEntryImage.Image = LoadMediaCoverArtThumbnail(fileEntryImage.FileFullPath, ThumbnailSaveSize, false);
                                            if (fileEntryImage.Image != null)
                                                ImageListViewReloadThumbnailAndMetadataInvoke(imageListView1, fileEntryImage.FileFullPath);
                                        }
                                    } 
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadSaveThumbnail - LoadMediaCoverArtThumbnail failed");
                                    }

                                    try
                                    {
                                        if (fileEntryImage.Image != null) // && !databaseAndCacheThumbnail.DoesThumbnailExist(fileEntryImage))
                                        {
                                            databaseAndCacheThumbnail.TransactionBeginBatch();
                                            databaseAndCacheThumbnail.WriteThumbnail(fileEntryImage, fileEntryImage.Image);
                                            databaseAndCacheThumbnail.TransactionCommitBatch();

                                            UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(new FileEntryAttribute(fileEntryImage, FileEntryVersion.Current), fileEntryImage.Image);
                                            UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(new FileEntryAttribute(fileEntryImage, FileEntryVersion.Error), fileEntryImage.Image);
                                        }
                                    } 
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadSaveThumbnail - WriteThumbnail failed");
                                    }

                                    lock (commonQueueSaveThumbnailToDatabaseLock)
                                    {
                                        if (commonQueueSaveThumbnailToDatabase.Count > 0) commonQueueSaveThumbnailToDatabase.RemoveAt(0);
                                    }

                                    if (GlobalData.IsApplicationClosing) lock (commonQueueSaveThumbnailToDatabaseLock) commonQueueSaveThumbnailToDatabase.Clear();
                                }
                                catch (Exception ex)
                                {
                                    KryptonMessageBox.Show("Saving ThreadSaveThumbnail crashed. The ThreadSaveThumbnail queue was cleared. Nothing was saved.", "Saving ThreadSaveThumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    lock (commonQueueSaveThumbnailToDatabaseLock) { commonQueueSaveThumbnailToDatabase.Clear(); } //Avoid loop, due to unknown error
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
                            lock (_ThreadSaveThumbnailLock) _ThreadSaveThumbnail = null;
                            Logger.Trace("ThreadSaveThumbnail - ended");
                        }
                        #endregion
                    });

                    lock (_ThreadSaveThumbnailLock) if (_ThreadSaveThumbnail != null)
                    {
                        _ThreadSaveThumbnail.Priority = threadPriority;
                        _ThreadSaveThumbnail.Start();                        
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

        #region AddQueue - AddQueueExiftool(FileEntry fileEntry)
        /// <summary>
        /// Add File Entry to Read "Metadata Queue"
        /// Inside Queue -> 
        ///     1. "Extract Exiftool data" ->> Store in database
        ///     2. Updated DataGridView to show new/updated Metadata
        ///     3. Add Metadata to Region Queue
        ///     -- Region Queue: Read Media Poster -> Extract Region Thmbnail
        /// </summary>
        /// <param name="fileEntry"></param>
        public void AddQueueExiftoolLock(FileEntry fileEntry)
        {
            lock (commonQueueReadMetadataFromExiftoolLock)
            {
                //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
                //bool existInQueue = ExistInExiftoolReadQueue(fileEntry);
                //if (!existInQueue)
                bool existInQueue = ExistFolderInQueueReadMetadataFromExiftool(fileEntry.FileFullPath);
                //if (ExistInExiftoolWriteQueue(fileEntry)) existInQueue = true;

                if (!existInQueue) commonQueueReadMetadataFromExiftool.Add(fileEntry);
            }
            RemoveError(fileEntry.FileFullPath);

        }
        #endregion

        #region Thread - ThreadCollectMetadataExiftool
        public void ThreadCollectMetadataExiftool()
        {
            try
            {
                if (WaitExittoolReadCacheThread != null && CommonQueueReadMetadataFromExiftoolCountDirty() <= 0) WaitExittoolReadCacheThread.Set();
                if (GlobalData.IsStopAndEmptyExiftoolReadQueueRequest || _ThreadCollectMetadataExiftool != null || CommonQueueReadMetadataFromExiftoolCountDirty() <= 0) return;
                if (MediaFilesNotInDatabaseCountLock() > 0) return; //Don't start double read of exiftool

                lock (__ThreadCollectMetadataExiftoolLock)
                {
                    _ThreadCollectMetadataExiftool = new Thread(() =>
                    {
                        #region
                        try
                        {
                            Logger.Trace("ThreadCollectMetadataExiftool - started");

                            bool showCliWindow = Properties.Settings.Default.ApplicationDebugExiftoolReadShowCliWindow;
                            ProcessPriorityClass processPriorityClass = (ProcessPriorityClass)Properties.Settings.Default.ApplicationDebugExiftoolReadThreadPrioity;

                            int count = CommonQueueReadMetadataFromExiftoolCountLock();
                            while (count > 0 && !GlobalData.IsStopAndEmptyExiftoolReadQueueRequest && CommonQueueReadMetadataFromExiftoolCountLock() > 0 && !GlobalData.IsApplicationClosing) //In case some more added to the queue
                            {
                                count--;
                                if (CommonQueueSaveMetadataUpdatedByUserCountLock() > 0) break; //Write first, read later on...

                                int rangeToRemove; //Remember how many in queue now
                                List<FileEntry> mediaFilesNotInDatabaseCheckInCloud = new List<FileEntry>();

                                #region From the Read Queue - Find files not alread in database
                                List<FileEntry> mediaFilesNotInDatabaseCheckInCloudCopy;
                                lock (commonQueueReadMetadataFromExiftoolLock)
                                {
                                    mediaFilesNotInDatabaseCheckInCloudCopy = new List<FileEntry>(commonQueueReadMetadataFromExiftool);
                                    rangeToRemove = mediaFilesNotInDatabaseCheckInCloudCopy.Count;
                                }

                                //Avoid look for long time
                                mediaFilesNotInDatabaseCheckInCloud.AddRange(
                                    databaseAndCacheMetadataExiftool.ListAllMissingFileEntries(MetadataBrokerType.ExifTool, mediaFilesNotInDatabaseCheckInCloudCopy.GetRange(0, rangeToRemove)));

                                lock (commonQueueReadMetadataFromExiftoolLock)
                                {
                                    mediaFilesNotInDatabaseCheckInCloudCopy.Clear();
                                    commonQueueReadMetadataFromExiftool.RemoveRange(0, rangeToRemove);
                                }
                                #endregion

                                #region Check if need avoid files in cloud, if yes, don't read files in cloud
                                if (Properties.Settings.Default.AvoidReadExifFromCloud)
                                {
                                    foreach (FileEntry fileEntry in mediaFilesNotInDatabaseCheckInCloud)
                                    {
                                        //Don't add files from cloud in queue
                                        if (!FileHandler.IsFileInCloud(fileEntry.FileFullPath)) mediaFilesNotInDatabase.Add(fileEntry);
                                        else PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(fileEntry, FileEntryVersion.Current)); //Also populate dataGridView when in cloud, but empty rows in column
                                    }
                                }
                                else
                                {
                                    mediaFilesNotInDatabase.AddRange(mediaFilesNotInDatabaseCheckInCloud);
                                }
                                #endregion

                                while (!GlobalData.IsStopAndEmptyExiftoolReadQueueRequest && MediaFilesNotInDatabaseCountLock() > 0 && !GlobalData.IsApplicationClosing)
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

                                    lock (mediaFilesNotInDatabaseLock)
                                    {
                                        while (argumnetLength < maxParameterCommandLength && range < mediaFilesNotInDatabase.Count)
                                        {
                                            argumnetLength += mediaFilesNotInDatabase[range].FileFullPath.Length + 3; //+3 = space and 2x"
                                            range++;
                                        }

                                        if (argumnetLength > maxParameterCommandLength) range--;
                                        useExiftoolOnThisSubsetOfFiles = new List<string>();
                                        for (int index = 0; index < range; index++) useExiftoolOnThisSubsetOfFiles.Add(mediaFilesNotInDatabase[index].FileFullPath);
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
                                                string errorMesssage = lastKnownExiftoolError;
                                                if (!File.Exists(fullFilePath)) errorMesssage += (errorMesssage == "" ? "" : "\r\n") + "File doesn't exist. ";
                                                else
                                                {
                                                    if (FileHandler.IsFileLockedByProcess(fullFilePath, FileHandler.GetFileLockedStatusTimeout)) errorMesssage += (errorMesssage == "" ? "" : "\r\n") + "File is Locked. ";
                                                    if (FileHandler.IsFileInCloud(fullFilePath)) errorMesssage += (errorMesssage == "" ? "" : "\r\n") + "File is in clound only. ";
                                                    if (FileHandler.IsFileVirtual(fullFilePath)) errorMesssage += (errorMesssage == "" ? "" : "\r\n") + "File is in virtual only. ";
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
                                            lock (commonQueueMetadataWrittenByExiftoolReadyToVerifyLock)
                                            {
                                                if (ExiftoolWriter.HasWriteMetadataErrors(metadataRead, commonQueueMetadataWrittenByExiftoolReadyToVerify,
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

                                                    AddQueueSaveThumbnailMediaLock(new FileEntryImage(metadataError.FileEntryBroker, null));
                                                    PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadataError.FileFullPath, (DateTime)metadataError.FileDateModified, FileEntryVersion.Error));
                                                }

                                                //Data was read, (even with errors), need to updated datagrid
                                                AddQueueCreateRegionFromPosterLock(metadataRead);
                                                ImageListViewSetItemDirty(metadataRead.FileFullPath);
                                                PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadataRead.FileFullPath, (DateTime)metadataRead.FileDateModified, FileEntryVersion.Current));
                                                PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadataRead.FileFullPath, (DateTime)metadataRead.FileDateModified, FileEntryVersion.Historical));
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
                                            lock (mediaFilesNotInDatabaseLock)
                                            {
                                                for (int index = 0; index < range; index++)
                                                {
                                                    if (!Metadata.IsFullFilenameInList(metadataReadbackExiftoolAfterSaved, mediaFilesNotInDatabase[index].FileFullPath))
                                                    {
                                                        try
                                                        {
                                                            Metadata metadataDummy = new Metadata(MetadataBrokerType.ExifTool);
                                                            metadataDummy.FileDateModified = mediaFilesNotInDatabase[index].LastWriteDateTime;
                                                            metadataDummy.FileName = mediaFilesNotInDatabase[index].FileName;
                                                            metadataDummy.FileDirectory = mediaFilesNotInDatabase[index].Directory;
                                                            metadataDummy.FileMimeType = FormDatabaseCleaner.CorruptFile; //Also used
                                                            metadataDummy.PersonalComments = "Exiftool failed";
                                                            try
                                                            {
                                                                FileInfo fileInfo = new FileInfo(mediaFilesNotInDatabase[index].FileFullPath);
                                                                metadataDummy.FileDateCreated = fileInfo.CreationTime;
                                                                metadataDummy.FileDateAccessed = fileInfo.LastAccessTime;
                                                                metadataDummy.FileSize = fileInfo.Length;
                                                            } catch
                                                            {
                                                                metadataDummy.FileDateCreated = mediaFilesNotInDatabase[index].LastWriteDateTime;
                                                                metadataDummy.FileDateAccessed = DateTime.Now;
                                                                metadataDummy.FileSize = 0;
                                                            }
                                                            databaseAndCacheMetadataExiftool.TransactionBeginBatch();
                                                            databaseAndCacheMetadataExiftool.Write(metadataDummy);
                                                            databaseAndCacheMetadataExiftool.TransactionCommitBatch();

                                                            Metadata metadataError = new Metadata(metadataDummy);
                                                            metadataError.FileDateModified = DateTime.Now;
                                                            metadataError.Broker |= MetadataBrokerType.ExifToolWriteError;
                                                            databaseAndCacheMetadataExiftool.TransactionBeginBatch();
                                                            databaseAndCacheMetadataExiftool.Write(metadataError);
                                                            databaseAndCacheMetadataExiftool.TransactionCommitBatch();

                                                            PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadataError.FileFullPath, (DateTime)metadataError.FileDateModified, FileEntryVersion.Error));
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
                                        lock (mediaFilesNotInDatabaseLock) mediaFilesNotInDatabase.RemoveRange(0, range); //Remove subset from queue before update status bar
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(ex, "ThreadCollectMetadataExiftool - mediaFilesNotInDatabase.RemoveRange(0, range) failed.");
                                    }
                                    #endregion 
                                }
                            }

                            if (GlobalData.IsApplicationClosing || GlobalData.IsStopAndEmptyExiftoolReadQueueRequest)
                                lock (commonQueueReadMetadataFromExiftoolLock) commonQueueReadMetadataFromExiftool.Clear();

                            PopulateDataGridViewForSelectedItemsExtrasDelayed();

                            if (WaitExittoolReadCacheThread != null) WaitExittoolReadCacheThread.Set();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("Read exiftool thread crashed.", "Read exiftool thread failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lock (commonQueueReadMetadataFromExiftoolLock) commonQueueReadMetadataFromExiftool.Clear();
                            Logger.Error(ex, "ThreadCollectMetadataExiftool");
                        }
                        finally
                        {
                            GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = false;
                            //lock(_ThreadCollectMetadataExiftoolLock)
                            lock (__ThreadCollectMetadataExiftoolLock) _ThreadCollectMetadataExiftool = null;
                            Logger.Trace("ThreadCollectMetadataExiftool - ended");
                        }
                        #endregion
                    });

                    //(_ThreadCollectMetadataExiftoolLock) 
                    lock(__ThreadCollectMetadataExiftoolLock) if (_ThreadCollectMetadataExiftool != null)
                    {
                        _ThreadCollectMetadataExiftool.Priority = threadPriority;
                        _ThreadCollectMetadataExiftool.Start();                        
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

        #region AddQueue - AddQueueSaveMetadataUpdatedByUser
        public void AddQueueSaveMetadataUpdatedByUserLock(Metadata metadataToSave, Metadata metadataOriginal)
        {
            lock (commonQueueSaveMetadataUpdatedByUserLock) commonQueueSaveMetadataUpdatedByUser.Add(metadataToSave);
            lock (commonOrigialMetadataBeforeUserUpdateLock) commonOrigialMetadataBeforeUserUpdate.Add(metadataOriginal);
        }
        #endregion

        #region AddQueue - AddQueueVerifyMetadata(Metadata metadataToVerify)
        public void AddQueueVerifyMetadataLock(Metadata metadataToVerifyAfterSavedByExiftool)
        {
            lock (commonQueueMetadataWrittenByExiftoolReadyToVerifyLock) commonQueueMetadataWrittenByExiftoolReadyToVerify.Add(metadataToVerifyAfterSavedByExiftool);
        }
        #endregion 

        #region Thread - ThreadSaveMetadata
        public void ThreadSaveMetadata()
        {
            try
            {
                lock (_ThreadSaveMetadataLock) if (_ThreadSaveMetadata != null || CommonQueueSaveMetadataUpdatedByUserCountDirty() <= 0) return;

                lock (_ThreadSaveMetadataLock)
                {
                    _ThreadSaveMetadata = new Thread(() =>
                    {
                        #region
                        try
                        {
                            if (!GlobalData.IsApplicationClosing)
                            {
                                Logger.Trace("ThreadSaveMetadata - started");

                                #region Init Write Variables and Parameters
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

                                int writeCount = CommonQueueSaveMetadataUpdatedByUserCountLock();
                                lock (commonQueueSubsetMetadataToSaveLock) commonQueueSubsetMetadataToSave = new List<Metadata>();    //This new values for saving (changes done by user)
                                List<Metadata> queueSubsetMetadataOrginalBeforeUserEdit = new List<Metadata>(); //Before updated by user, need this to check if any updates

                                #region Create a subset queue for writing
                                try
                                {
                                    for (int i = 0; i < writeCount; i++)
                                    {
                                        //Remeber 
                                        Metadata metadataWrite;
                                        Metadata metadataOrginal;

                                        lock (commonQueueSaveMetadataUpdatedByUserLock)
                                        {
                                            metadataWrite = commonQueueSaveMetadataUpdatedByUser[0];
                                            lock (commonOrigialMetadataBeforeUserUpdateLock) metadataOrginal = commonOrigialMetadataBeforeUserUpdate[0];

                                            lock (commonQueueReadMetadataFromExiftoolLock)
                                            {
                                                if (!GlobalData.IsApplicationClosing)
                                                {
                                                    //Also include Metadata ToBeSaved that are Equal with OrgianalBeforeUserEdit 
                                                    if (metadataOrginal != metadataWrite) AddWatcherShowExiftoolSaveProcessQueue(metadataWrite.FileEntryBroker.FileFullPath);

                                                    lock (commonQueueSubsetMetadataToSaveLock) commonQueueSubsetMetadataToSave.Add(metadataWrite);
                                                    queueSubsetMetadataOrginalBeforeUserEdit.Add(metadataOrginal);
                                                }
                                            }

                                            //Remove
                                            commonQueueSaveMetadataUpdatedByUser.RemoveAt(0);
                                            lock (commonOrigialMetadataBeforeUserUpdateLock) commonOrigialMetadataBeforeUserUpdate.RemoveAt(0);

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
                                if (!GlobalData.IsApplicationClosing) FileHandler.WaitLockedFilesToBecomeUnlocked(commonQueueSubsetMetadataToSave, true, this);

                                List<FileEntry> mediaFilesUpdatedByExiftool = new List<FileEntry>();
                                string exiftoolErrorMessage = "";

                                if (!GlobalData.IsApplicationClosing)
                                {
                                    try
                                    {
                                        lock (commonQueueSubsetMetadataToSaveLock)
                                        {
                                            #region AutoKeywords
                                            foreach (Metadata metadataCopy in commonQueueSubsetMetadataToSave)
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
                                            
                                            UpdateStatusAction("Batch update a subset of " + commonQueueSubsetMetadataToSave.Count + " media files...");
                                            ExiftoolWriter.WriteMetadata(
                                            commonQueueSubsetMetadataToSave, queueSubsetMetadataOrginalBeforeUserEdit, allowedFileNameDateTimeFormats,
                                            writeMetadataTagsVariable, writeMetadataKeywordAddVariable, out mediaFilesUpdatedByExiftool,
                                            showCliWindow, processPriorityClass);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Something did go wrong, need to tell users, what was saved and not, in general nothing was saved
                                        lock (commonQueueSubsetMetadataToSaveLock)
                                        {
                                            foreach (Metadata metadataCopy in commonQueueSubsetMetadataToSave)
                                            {
                                                if (!FileEntry.Contains(mediaFilesUpdatedByExiftool, metadataCopy.FileEntry)) mediaFilesUpdatedByExiftool.Add(metadataCopy.FileEntry);
                                            }
                                        }

                                        exiftoolErrorMessage = ex.Message;
                                        Logger.Error(ex, "EXIFTOOL.EXE error...");
                                    }
                                }
                                #endregion

                                #region Remove from verify queue
                                //private static List<Metadata> commonOrigialMetadataBeforeUserUpdate = new List<Metadata>();
                                //private static readonly Object commonOrigialMetadataBeforeUserUpdateLock = new Object();
                                //private static List<Metadata> commonQueueMetadataWrittenByExiftoolReadyToVerify = new List<Metadata>();
                                //private static readonly Object commonQueueMetadataWrittenByExiftoolReadyToVerifyLock = new Object();
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
                                        UpdateStatusAction("Write Xtra Atom to " + commonQueueSubsetMetadataToSave.Count + " media files...");

                                        filesUpdatedByWriteXtraAtom = ExiftoolWriter.WriteXtraAtom(
                                            commonQueueSubsetMetadataToSave, queueSubsetMetadataOrginalBeforeUserEdit, allowedFileNameDateTimeFormats,
                                            writeXtraAtomAlbumVariable, writeXtraAtomAlbumVideo,
                                            writeXtraAtomCategoriesVariable, writeXtraAtomCategoriesVideo,
                                            writeXtraAtomCommentVariable, writeXtraAtomCommentPicture, writeXtraAtomCommentVideo,
                                            writeXtraAtomKeywordsVariable, writeXtraAtomKeywordsVideo,
                                            writeXtraAtomRatingPicture, writeXtraAtomRatingVideo,
                                            writeXtraAtomSubjectVariable, writeXtraAtomSubjectPicture, wtraAtomSubjectVideo,
                                            writeXtraAtomSubtitleVariable, writeXtraAtomSubtitleVideo,
                                            writeXtraAtomArtistVariable, writeXtraAtomArtistVideo,
                                            out writeXtraAtomErrorMessageForFile, this);
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
                                if (!GlobalData.IsApplicationClosing) FileHandler.WaitLockedFilesToBecomeUnlocked(commonQueueSubsetMetadataToSave, true, this);

                                try
                                {
                                    if (!GlobalData.IsApplicationClosing)
                                    {
                                        if (writeCreatedDateAndTimeAttribute)
                                        {
                                            lock (commonQueueSubsetMetadataToSaveLock) foreach (Metadata metadata in commonQueueSubsetMetadataToSave)
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
                                if (!GlobalData.IsApplicationClosing) FileHandler.WaitLockedFilesToBecomeUnlocked(commonQueueSubsetMetadataToSave, true, this);

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
                                                    "Error message:" + writeXtraAtomErrorMessageForFile[fileSuposeToBeUpdated.FileFullPath]);
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
                                                        "Message return from Exiftool: " + exiftoolErrorMessage);
                                            }
                                            #endregion 

                                            int indexInVerifyQueue = Metadata.FindFileEntryInList(commonQueueSubsetMetadataToSave, fileSuposeToBeUpdated);
                                            if (indexInVerifyQueue > -1 && indexInVerifyQueue < commonQueueSubsetMetadataToSave.Count)
                                            {
                                                Metadata currentMetadata;
                                                lock (commonQueueSubsetMetadataToSaveLock) currentMetadata = new Metadata(commonQueueSubsetMetadataToSave[indexInVerifyQueue]);
                                                currentMetadata.FileDateModified = currentLastWrittenDateTime;
                                                if (File.Exists(currentMetadata.FileFullPath) && currentLastWrittenDateTime != previousLastWrittenDateTime) AddQueueVerifyMetadataLock(currentMetadata);
                                                AddQueueLazyLoadingDataGridViewMetadataReadToCacheOrUpdateFromSoruce(currentMetadata.FileEntryBroker);
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
                                if (!GlobalData.IsApplicationClosing) FileHandler.WaitLockedFilesToBecomeUnlocked(commonQueueSubsetMetadataToSave, true, this);

                                //Clean up
                                lock (commonQueueSubsetMetadataToSaveLock) commonQueueSubsetMetadataToSave.Clear();
                                queueSubsetMetadataOrginalBeforeUserEdit.Clear();
                                mediaFilesUpdatedByExiftool.Clear();

                                //Status updated for user
                                ShowExiftoolSaveProgressClear();
                            }

                            if (GlobalData.IsApplicationClosing)
                            {
                                lock (commonQueueSaveMetadataUpdatedByUserLock) commonQueueSaveMetadataUpdatedByUser.Clear();
                                lock (commonOrigialMetadataBeforeUserUpdateLock) commonOrigialMetadataBeforeUserUpdate.Clear();
                            }

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("Saving ThreadSaveMetadata crashed. The ThreadSaveMetadata queue was cleared. Nothing was saved.", "Saving ThreadSaveMetadata failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lock (commonQueueSubsetMetadataToSaveLock) commonQueueSubsetMetadataToSave.Clear();
                            Logger.Error(ex, "ThreadSaveMetadata: ");
                        }
                        finally
                        {
                            lock(_ThreadSaveMetadataLock) _ThreadSaveMetadata = null;
                            Logger.Trace("ThreadSaveMetadata - ended");
                        }

                        #endregion

                    });

                    lock (_ThreadSaveMetadataLock) if (_ThreadSaveMetadata != null)
                    {
                        _ThreadSaveMetadata.Priority = threadPriority;
                        _ThreadSaveMetadata.Start();                        
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

        #region AddQueue - AddQueueMicrosoftPhotos(FileEntry fileEntry)
        /// <summary>
        /// Add File Entry to Read "Metadata Queue"
        /// Inside Queue -> 
        ///     1. "Extract Exiftool data" ->> Store in database
        ///     2. Updated DataGridView to show new/updated Metadata
        ///     3. Add Metadata to Region Queue
        ///     -- Region Queue: Read Media Poster -> Extract Region Thmbnail
        /// </summary>
        /// <param name="fileEntry"></param>
        public void AddQueueMicrosoftPhotosLock(FileEntry fileEntry)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueReadMetadataFromMicrosoftPhotosLock)
            {
                if (!commonQueueReadMetadataFromMicrosoftPhotos.Contains(fileEntry)) commonQueueReadMetadataFromMicrosoftPhotos.Add(fileEntry);
            }
        }
        #endregion

        #region Thread - ThreadCollectMetadataMicrosoftPhotos 
        public void ThreadCollectMetadataMicrosoftPhotos()
        {
            try
            {
                lock (_ThreadMicrosoftPhotosLock) if (_ThreadMicrosoftPhotos != null || CommonQueueReadMetadataFromMicrosoftPhotosCountDirty() <= 0) return;

                lock (_ThreadMicrosoftPhotosLock)
                {
                    _ThreadMicrosoftPhotos = new Thread(() =>
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
                                    lock (commonQueueReadMetadataFromMicrosoftPhotosLock) fileEntry = new FileEntry(commonQueueReadMetadataFromMicrosoftPhotos[0]);

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
                                                    AddQueueCreateRegionFromPosterLock(metadataMicrosoftPhotos);

                                                    PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadataMicrosoftPhotos.FileFullPath, (DateTime)metadataMicrosoftPhotos.FileDateModified, FileEntryVersion.Current));
                                                }
                                            }
                                        }
                                    }
                                    else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                                    lock (commonQueueReadMetadataFromMicrosoftPhotosLock) if (commonQueueReadMetadataFromMicrosoftPhotos.Count > 0) commonQueueReadMetadataFromMicrosoftPhotos.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar
                                }
                                #endregion
                            }

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromMicrosoftPhotosLock) commonQueueReadMetadataFromMicrosoftPhotos.Clear();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("ThreadCollectMetadataMicrosoftPhotos crashed.", "ThreadCollectMetadataMicrosoftPhotos failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Logger.Error(ex, "ThreadCollectMetadataMicrosoftPhotos failed");
                        }
                        finally
                        {
                            lock (_ThreadMicrosoftPhotosLock) _ThreadMicrosoftPhotos = null;
                            Logger.Trace("ThreadCollectMetadataMicrosoftPhotos - ended");
                        }
                        #endregion

                    });

                    lock (_ThreadMicrosoftPhotosLock) if (_ThreadMicrosoftPhotos != null)
                    {
                        _ThreadMicrosoftPhotos.Priority = threadPriority;
                        _ThreadMicrosoftPhotos.Start();                        
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

        #region AddQueue - AddQueueWindowsLivePhotoGallery(FileEntry fileEntry)
        /// <summary>
        /// Add File Entry to Read "Metadata Queue"
        /// Inside Queue -> 
        ///     1. "Extract Exiftool data" ->> Store in database
        ///     2. Updated DataGridView to show new/updated Metadata
        ///     3. Add Metadata to Region Queue
        ///     -- Region Queue: Read Media Poster -> Extract Region Thmbnail
        /// </summary>
        /// <param name="fileEntry"></param>
        public void AddQueueWindowsLivePhotoGalleryLock(FileEntry fileEntry)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock)
            {
                if (!commonQueueReadMetadataFromWindowsLivePhotoGallery.Contains(fileEntry)) commonQueueReadMetadataFromWindowsLivePhotoGallery.Add(fileEntry);
            }
        }
        #endregion

        #region Thread - ThreadCollectMetadataWindowsLiveGallery
        public void ThreadCollectMetadataWindowsLiveGallery()
        {
            try
            {
                lock (_ThreadWindowsLiveGalleryLock) if (_ThreadWindowsLiveGallery != null || CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountDirty() <= 0) return;

                lock (_ThreadWindowsLiveGalleryLock)
                {
                    _ThreadWindowsLiveGallery = new Thread(() =>
                    {
                        #region
                        try
                        {
                            Logger.Trace("ThreadCollectMetadataWindowsLiveGallery - started");
                            int count = CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock();
                            while (count > 0 && CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() > 0 && !GlobalData.IsApplicationClosing)
                            {
                                count--;

                                #region Common for ThreadCollectMetadataWindowsLiveGallery and ThreadCollectMetadataMicrosoftPhotos
                                MetadataDatabaseCache database = databaseAndCacheMetadataWindowsLivePhotoGallery;
                                ImetadataReader databaseSourceReader = databaseWindowsLivePhotGallery;
                                MetadataBrokerType broker = MetadataBrokerType.WindowsLivePhotoGallery;

                                while (databaseSourceReader != null && !GlobalData.IsApplicationClosing && CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() > 0) //In case some more added to the queue
                                {
                                    Metadata metadataWindowsLivePhotoGallery;
                                    FileEntry fileEntry;
                                    lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) fileEntry = new FileEntry(commonQueueReadMetadataFromWindowsLivePhotoGallery[0]);

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
                                                    AddQueueCreateRegionFromPosterLock(metadataWindowsLivePhotoGallery);

                                                    PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadataWindowsLivePhotoGallery.FileFullPath, (DateTime)metadataWindowsLivePhotoGallery.FileDateModified, FileEntryVersion.Current));
                                                }
                                            }
                                        }
                                    }
                                    else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                                    lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) if (commonQueueReadMetadataFromWindowsLivePhotoGallery.Count > 0) commonQueueReadMetadataFromWindowsLivePhotoGallery.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar
                                }
                                #endregion
                            }

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) commonQueueReadMetadataFromWindowsLivePhotoGallery.Clear();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("ThreadCollectMetadataWindowsLiveGallery crashed.", "ThreadCollectMetadataWindowsLiveGallery failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) commonQueueReadMetadataFromWindowsLivePhotoGallery.Clear();
                            Logger.Error(ex, "ThreadCollectMetadataWindowsLiveGallery failed");
                        }
                        finally
                        {
                            lock(_ThreadWindowsLiveGalleryLock) _ThreadWindowsLiveGallery = null;
                            Logger.Trace("ThreadCollectMetadataWindowsLiveGallery - ended");
                        }
                        #endregion
                    });

                    lock (_ThreadWindowsLiveGalleryLock) if (_ThreadWindowsLiveGallery != null)
                    {
                        _ThreadWindowsLiveGallery.Priority = threadPriority; 
                        _ThreadWindowsLiveGallery.Start();                        
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

        #region Region RegionFromPoster

        #region AddQueue - AddQueueCreateRegionFromPoster(Metadata metadata)
        private void AddQueueCreateRegionFromPosterLock(Metadata metadata)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
            {
                if (!commonQueueReadPosterAndSaveFaceThumbnails.Contains(metadata)) commonQueueReadPosterAndSaveFaceThumbnails.Add(new Metadata(metadata));
            }
        }
        #endregion

        #region Thread - ReadMediaPosterSaveRegions()
        /// <summary>
        /// Read list of Metadata with list of face region inside
        /// 1. Read media poster for "Media file"
        /// 2. Get Region Thumbnail from "Poster"
        /// 3. Save to database
        /// 4. Updated DataGridView with new "pictures"
        /// </summary>
        public void ThreadReadMediaPosterSaveRegions()
        {
            try
            {
                if (IsThreadRunningExcept_ThreadThumbnailRegion()) return; //Wait other thread to finnish first. Otherwise it will generate high load on disk use

                lock (_ThreadThumbnailRegionLock) if (_ThreadThumbnailRegion != null || CommonQueueReadPosterAndSaveFaceThumbnailsCountDirty() <= 0) return;

                lock (_ThreadThumbnailRegionLock)
                {
                    _ThreadThumbnailRegion = new Thread(() =>
                    {
                        #region
                        Logger.Trace("ThreadReadMediaPosterSaveRegions - started");
                        try
                        {
                            int curentCommonQueueReadPosterAndSaveFaceThumbnailsCount = CommonQueueReadPosterAndSaveFaceThumbnailsCountLock();
                            bool onlyDoWhatIsInCacheToAvoidHarddriveOverload = (IsThreadRunningExcept_ThreadThumbnailRegion() == true);
                            bool dontReadFilesInCloud = Properties.Settings.Default.AvoidOfflineMediaFiles;

                            int indexSource = 0;

                            while (indexSource < curentCommonQueueReadPosterAndSaveFaceThumbnailsCount)  //Loop until queue empty or checked all
                            {
                                try
                                {
                                    FileEntry fileEntryRegion;
                                    lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                                    { fileEntryRegion = new FileEntry(commonQueueReadPosterAndSaveFaceThumbnails[indexSource].FileEntryBroker); }

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

                                        int queueCount = CommonQueueReadPosterAndSaveFaceThumbnailsCountLock(); //Mark count that we will work with. 

                                        for (int thumbnailIndex = indexSource; thumbnailIndex < queueCount; thumbnailIndex++) //Not need to check already checked -> thumbnailIndex = indexSource
                                        {
                                            Metadata metadataActiveAlreadyCopy = null;
                                            lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                                            { metadataActiveAlreadyCopy = commonQueueReadPosterAndSaveFaceThumbnails[thumbnailIndex]; }

                                            //Find current file entry in queue, Exiftool, Microsoft Photos, Windows Live Gallery, etc...
                                            if (metadataActiveAlreadyCopy.FileFullPath == fileEntryRegion.FileFullPath &&
                                            metadataActiveAlreadyCopy.FileDateModified == fileEntryRegion.LastWriteDateTime)
                                            {

                                                fileIndexFound = thumbnailIndex;
                                                fileFoundInList = true;
                                                fileFoundNeedCheckForMoreWithSameFilename = true;

                                                //When found entry, check if has Face Regions to save
                                                if (metadataActiveAlreadyCopy.PersonalRegionList.Count == 0)
                                                {
                                                    fileFoundRemoveFromList = true; //No regions to create, remove from queue
                                                }
                                                else
                                                {
                                                    if (onlyDoWhatIsInCacheToAvoidHarddriveOverload)
                                                    {
                                                        image = PosterCacheRead(fileEntryRegion.FileFullPath);
                                                        if (image != null)
                                                            fileFoundRemoveFromList = true;
                                                        else
                                                            fileFoundRemoveFromList = false; //Not in cache, need wait for loading starts (that's after all other queue empty)
                                                    }
                                                    else
                                                    {
                                                        fileFoundRemoveFromList = true;
                                                        bool isFileUnLockedAndExist = FileHandler.WaitLockedFileToBecomeUnlocked(fileEntryRegion.FileFullPath, false, this);

                                                        //Check if the current Metadata are same as newst file... If not file exist anymore, date will become {01.01.1601 01:00:00}
                                                        if (isFileUnLockedAndExist && File.GetLastWriteTime(fileEntryRegion.FileFullPath) == fileEntryRegion.LastWriteDateTime)
                                                        {
                                                            bool isFileInCloud = FileHandler.IsFileInCloud(fileEntryRegion.FileFullPath);

                                                            try
                                                            {
                                                                if (!isFileInCloud || (isFileInCloud && !dontReadFilesInCloud)) image = LoadMediaCoverArtPoster(fileEntryRegion.FileFullPath);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Logger.Error(ex, "ThreadReadMediaPosterSaveRegions - LoadMediaCoverArtPoster");
                                                            }

                                                            if (image == null) //If failed load cover art, often occur after filed is moved or deleted
                                                            {
                                                                if (!(FileHandler.IsFileInCloud(fileEntryRegion.FileFullPath) && dontReadFilesInCloud))
                                                                {
                                                                    string writeErrorDesciption = "Failed loading mediafile. Was not able to update thumbnail for region for the file:" + fileEntryRegion.FileFullPath;
                                                                    Logger.Error(writeErrorDesciption);

                                                                    AddError(
                                                                        fileEntryRegion.Directory,
                                                                        fileEntryRegion.FileName,
                                                                        fileEntryRegion.LastWriteDateTime,
                                                                        AddErrorFileSystemRegion, AddErrorFileSystemRead,
                                                                        AddErrorFileSystemRead, AddErrorFileSystemRead,
                                                                        writeErrorDesciption);
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (image != null) //Save regions when have image poster 
                                                    {
                                                        try
                                                        {
                                                            databaseAndCacheThumbnail.TransactionBeginBatch();
                                                            RegionThumbnailHandler.SaveThumbnailsForRegioList(databaseAndCacheMetadataExiftool, metadataActiveAlreadyCopy, image);
                                                            databaseAndCacheThumbnail.TransactionCommitBatch();
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Logger.Error(ex, "ThreadReadMediaPosterSaveRegions - SaveThumbnailsForRegioList");
                                                        }
                                                        PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(fileEntryRegion, FileEntryVersion.Current)); //Updated Gridview
                                                    }
                                                }

                                                if (fileFoundNeedCheckForMoreWithSameFilename) break; //No need to search more.
                                            }
                                        } //end of loop: for (int thumbnailIndex = indexSource; thumbnailIndex < queueCount; thumbnailIndex++)

                                        lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                                        {
                                            if (fileFoundRemoveFromList && fileIndexFound > -1)
                                            {
                                                curentCommonQueueReadPosterAndSaveFaceThumbnailsCount--;
                                                commonQueueReadPosterAndSaveFaceThumbnails.RemoveAt(fileIndexFound);
                                                //Check next FileEntry in queue, current will be next, due to removed an item
                                            }
                                            else indexSource++; //Check next FileEntry in queue
                                        }


                                    } while (fileFoundNeedCheckForMoreWithSameFilename);

                                    if (!fileFoundInList) //Should never occur ;-)
                                    {
                                        string writeErrorDesciption = "ThreadReadMediaPosterSaveRegions, file not found list for updated:" + fileEntryRegion.FileFullPath;
                                        Logger.Error(writeErrorDesciption);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    KryptonMessageBox.Show("Saving Region Thumbnail crashed. The 'save region queue' was cleared. Nothing was saved.", "Saving Region Thumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    lock (commonQueueReadPosterAndSaveFaceThumbnailsLock) { commonQueueReadPosterAndSaveFaceThumbnails.Clear(); } //Avoid loop, due to unknown error
                                    Logger.Error(ex, "ThreadReadMediaPosterSaveRegions crashed");
                                }
                            } //while (indexSource < curentCommonQueueReadPosterAndSaveFaceThumbnailsCount);

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadPosterAndSaveFaceThumbnailsLock) commonQueueReadPosterAndSaveFaceThumbnails.Clear();

                            TriggerAutoResetEventQueueEmpty();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, "ThreadReadMediaPosterSaveRegions");
                        }
                        finally
                        {
                            lock(_ThreadThumbnailRegionLock) _ThreadThumbnailRegion = null;
                            Logger.Trace("ThreadReadMediaPosterSaveRegions - ended");
                        }
                        #endregion
                    });

                    lock (_ThreadThumbnailRegionLock) if (_ThreadThumbnailRegion != null)
                    {
                        _ThreadThumbnailRegion.Priority = threadPriority;
                        _ThreadThumbnailRegion.Start();                        
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

        #region IsFileInThredQueue

        #region IsFileInThreadQueue
        /// <summary>
        /// Check if given files is in one of queue and wait to be processed
        /// </summary>
        /// <param name="fileList">StringCollection fileList to check</param>
        /// <returns>True, one of file waiting to be process in one of queues</returns>
        public bool IsFileInThreadQueueLock(StringCollection fileList)
        {
            bool fileInUse = false;
            foreach (string fullFilename in fileList)
            {
                fileInUse = IsFileInThreadQueueLock(fullFilename);
                if (fileInUse) break;
            }
            return fileInUse;
        }
        #endregion

        #region IsFileInThreadQueue
        /// <summary>
        /// Check if given files is in one of queue and wait to be processed
        /// </summary>
        /// <param name="fileList">ImageListViewSelectedItemCollection : IList<ImageListViewItem> fileList to check</param>
        /// <returns>True, one of file waiting to be process in one of queues</returns>
        public bool IsFileInThreadQueueLock(ImageListViewSelectedItemCollection imageListViewItems)
        {
            bool fileInUse = false;
            foreach (ImageListViewItem imageListViewItem in imageListViewItems)
            {
                fileInUse = IsFileInThreadQueueLock(imageListViewItem.FileFullPath);
                if (fileInUse) break;
            }
            return fileInUse;
        }
        #endregion

        #region ExistFolderInReadPosterAndSaveFaceThumbnailsQueue
        public bool ExistFolderInReadPosterAndSaveFaceThumbnailsQueue (string folder)
        {
            bool folderInUse = false;
            lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                foreach (Metadata metadata in commonQueueReadPosterAndSaveFaceThumbnails)
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

        #region ExistFileInReadPosterAndSaveFaceThumbnailsQueue
        public bool ExistFileInReadPosterAndSaveFaceThumbnailsQueue(string fullFilename)
        {
            bool fileInUse = false;

            lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                foreach (Metadata metadata in commonQueueReadPosterAndSaveFaceThumbnails)
                {
                    if (metadata.FileFullPath == fullFilename)
                    {
                        fileInUse = true;
                        break;
                    }
                }
            return fileInUse;
        }
        #endregion

        #region ExistFolderInQueueSaveThumbnailToDatabase
        public bool ExistFolderInQueueSaveThumbnailToDatabase(string folder)
        {
            bool folderInUse = false;

            lock (commonQueueSaveThumbnailToDatabaseLock)
                foreach (FileEntryImage fileEntry in commonQueueSaveThumbnailToDatabase)
                {
                    if (fileEntry.FileFullPath.StartsWith(folder))
                    {
                        folderInUse = true;
                        break;
                    }
                }
            return folderInUse;
        }
        #endregion

        #region ExistFileInQueueSaveThumbnailToDatabase
        public bool ExistFileInQueueSaveThumbnailToDatabase(string fullFilename)
        {
            bool fileInUse = false;
            lock (commonQueueSaveThumbnailToDatabaseLock)
                foreach (FileEntryImage fileEntry in commonQueueSaveThumbnailToDatabase)
                {
                    if (fileEntry.FileFullPath == fullFilename)
                    {
                        fileInUse = true;
                        break;
                    }
                }
            return fileInUse; 
        }
        #endregion

        #region ExistFolderQueueReadMetadataFromMicrosoftPhotos
        public bool ExistFolderQueueReadMetadataFromMicrosoftPhotos(string folder)
        {
            bool folderInUse = false;

            lock (commonQueueReadMetadataFromMicrosoftPhotosLock)
                foreach (FileEntry fileEntry in commonQueueReadMetadataFromMicrosoftPhotos)
                {
                    if (fileEntry.FileFullPath.StartsWith(folder))
                    {
                        folderInUse = true;
                        break;
                    }
                }
            return folderInUse;
        }
        #endregion

        #region ExistFileQueueReadMetadataFromMicrosoftPhotos
        public bool ExistFileQueueReadMetadataFromMicrosoftPhotos(string fullFilename)
        {
            bool fileInUse = false;

            lock (commonQueueReadMetadataFromMicrosoftPhotosLock)
            {
                foreach (FileEntry fileEntry in commonQueueReadMetadataFromMicrosoftPhotos)
                {
                    if (fileEntry.FileFullPath == fullFilename)
                    {
                        fileInUse = true;
                        break;
                    }
                }
            }
            return fileInUse;
        }
        #endregion

        #region ExistFolderInQueueReadMetadataFromWindowsLivePhotoGallery
        public bool ExistFolderInQueueReadMetadataFromWindowsLivePhotoGallery(string folder)
        {
            bool folderInUse = false;
            
            if (!folderInUse)
                lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock)
                    foreach (FileEntry fileEntry in commonQueueReadMetadataFromWindowsLivePhotoGallery)
                    {
                        if (fileEntry.FileFullPath.StartsWith(folder))
                        {
                            folderInUse = true;
                            break;
                        }
                    }
            
            return folderInUse;
        }
        #endregion

        #region ExistFileInQueueReadMetadataFromWindowsLivePhotoGallery
        public bool ExistFileInQueueReadMetadataFromWindowsLivePhotoGallery(string fullFilename)
        {
            bool fileInUse = false;
            lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock)
                foreach (FileEntry fileEntry in commonQueueReadMetadataFromWindowsLivePhotoGallery)
                {
                    if (fileEntry.FileFullPath == fullFilename)
                    {
                        fileInUse = true;
                        break;
                    }
                }
            
            return fileInUse;
        }
        #endregion

        #region ExistFolderInQueueReadMetadataFromExiftool
        public bool ExistFolderInQueueReadMetadataFromExiftool(string folder)
        {
            bool folderInUse = false;

            //commonQueueReadMetadataFromExiftool
            //mediaFilesNotInDatabaseCheckInCloud
            //MediaFilesNotInDatabaseCountLock

            if (!folderInUse)
                lock (commonQueueReadMetadataFromExiftoolLock)
                    foreach (FileEntry fileEntry in commonQueueReadMetadataFromExiftool)
                    {
                        if (fileEntry.FileFullPath.StartsWith(folder))
                        {
                            folderInUse = true;
                            break;
                        }
                    }

            if (!folderInUse)
                lock (mediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase)
                    {
                        if (fileEntry.FileFullPath.StartsWith(folder))
                        {
                            folderInUse = true;
                            break;
                        }
                    }
            return folderInUse;
        }
        #endregion

        #region ExistFileInQueueReadMetadataFromExiftool
        public bool ExistFileInQueueReadMetadataFromExiftool(string fullFilename)
        {
            bool fileInUse = false;
            
            if (!fileInUse)
                lock (commonQueueReadMetadataFromExiftoolLock)
                    foreach (FileEntry fileEntry in commonQueueReadMetadataFromExiftool)
                    {
                        if (fileEntry.FileFullPath == fullFilename)
                        {
                            fileInUse = true;
                            break;
                        }
                    }

            if (!fileInUse)
                lock (mediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase)
                    {
                        if (fileEntry.FileFullPath == fullFilename)
                        {
                            fileInUse = true;
                            break;
                        }
                    }
            return fileInUse;
        }
        #endregion

        #region ExisitFolderInQueueSaveMetadata
        public bool ExisitFolderInQueueSaveMetadata(string folder)
        {
            bool folderInUse = false;
            
            if (!folderInUse)
                lock (commonQueueSaveMetadataUpdatedByUserLock)
                    foreach (Metadata metadata in commonQueueSaveMetadataUpdatedByUser)
                    {
                        if (metadata.FileFullPath.StartsWith(folder))
                        {
                            folderInUse = true;
                            break;
                        }
                    }
            

            if (!folderInUse)
                lock (commonQueueSubsetMetadataToSaveLock)
                    foreach (Metadata metadata in commonQueueSubsetMetadataToSave)
                    {
                        if (metadata.FileFullPath.StartsWith(folder))
                        {
                            folderInUse = true;
                            break;
                        }
                    }

            if (!folderInUse)
                lock (mediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase)
                    {
                        if (fileEntry.FileFullPath.StartsWith(folder))
                        {
                            folderInUse = true;
                            break;
                        }
                    }

            return folderInUse;
        }
        #endregion

        #region ExisitFileInQueueSaveMetadata
        public bool ExisitFileInQueueSaveMetadata(string fullFilename)
        {
            bool fileInUse = false;
            if (!fileInUse)
                lock (commonQueueSaveMetadataUpdatedByUserLock)
                    foreach (Metadata metadata in commonQueueSaveMetadataUpdatedByUser)
                    {
                        if (metadata.FileFullPath == fullFilename)
                        {
                            fileInUse = true;
                            break;
                        }
                    }
            
            if (!fileInUse)
                lock (commonQueueSaveMetadataUpdatedByUserLock)
                    foreach (Metadata metadata in commonQueueSubsetMetadataToSave)
                    {
                        if (metadata.FileFullPath == fullFilename)
                        {
                            fileInUse = true;
                            break;
                        }
                    }
            
            if (!fileInUse)
                lock (mediaFilesNotInDatabaseLock)
                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase)
                    {
                        if (fileEntry.FileFullPath == fullFilename)
                        {
                            fileInUse = true;
                            break;
                        }
                    }
            
            return fileInUse;
        }
        #endregion 

        #region IsFileInThreadQueue
        /// <summary>
        /// Check if given files is in one of queue and wait to be processed
        /// </summary>
        /// <param name="imageListView">Check seletced files in the ImageListView is waiting to be process</param>
        /// <returns>True, one of file waiting to be process in one of queues</returns>
        public bool IsFileInThreadQueueLock(Manina.Windows.Forms.ImageListView imageListView)
        {
            bool fileInUse = false;
            foreach (Manina.Windows.Forms.ImageListViewItem listViewItem in imageListView.SelectedItems)
            {
                fileInUse = IsFileInThreadQueueLock(listViewItem.FileFullPath);
                if (fileInUse) break;
            }
            return fileInUse;
        }
        #endregion 

        #region IsFolderInThreadQueue
        /// <summary>
        /// Check if given files is in one of queue and wait to be processed
        /// </summary>
        /// <param name="folder">Folder to check is in queue</param>
        /// <returns>True, one of folders waiting to be process in one of queues</returns>
        public bool IsFolderInThreadQueueLock(string folder)
        {
            bool folderInUse = false;
            if (!folderInUse) folderInUse = ExistFolderInReadPosterAndSaveFaceThumbnailsQueue(folder);
            if (!folderInUse) folderInUse = ExistFolderInQueueSaveThumbnailToDatabase(folder);
            if (!folderInUse) folderInUse = ExistFolderQueueReadMetadataFromMicrosoftPhotos(folder);
            if (!folderInUse) folderInUse = ExistFolderInQueueReadMetadataFromWindowsLivePhotoGallery(folder);
            if (!folderInUse) folderInUse = ExistFolderInQueueReadMetadataFromExiftool(folder);
            if (!folderInUse) folderInUse = ExisitFolderInQueueSaveMetadata(folder);
            return folderInUse;
        }
        #endregion 

        #region IsFileInThreadQueue
        /// <summary>
        /// Check if given file is in one of queue and wait to be processed
        /// </summary>
        /// <param name="fullFilename">File to check if in queue</param>
        /// <returns>True, the file is waiting to be process in one of queues</returns>
        public bool IsFileInThreadQueueLock(string fullFilename)
        {
            bool fileInUse = false;
            if (!fileInUse) fileInUse = ExistFileInReadPosterAndSaveFaceThumbnailsQueue(fullFilename);
            if (!fileInUse) fileInUse = ExistFileInQueueSaveThumbnailToDatabase(fullFilename);
            if (!fileInUse) fileInUse = ExistFileQueueReadMetadataFromMicrosoftPhotos(fullFilename);
            if (!fileInUse) fileInUse = ExistFileInQueueReadMetadataFromWindowsLivePhotoGallery(fullFilename);
            if (!fileInUse) fileInUse = ExistFileInQueueReadMetadataFromExiftool(fullFilename);
            if (!fileInUse) fileInUse = ExisitFileInQueueSaveMetadata(fullFilename);

            return fileInUse;
        }
        #endregion

        #endregion 

        #region Rename

        #region Rename - AddQueueRename
        public void AddQueueRenameLock(string fullFileName, string renameVariable)
        {
            lock (commonQueueRenameLock)
            {
                if (commonQueueRename.ContainsKey(fullFileName))
                {
                    commonQueueRename[fullFileName] = renameVariable;
                }
                else
                {
                    commonQueueRename.Add(fullFileName, renameVariable);
                }
            }
        }
        #endregion

        #region Rename - ThreadRename
        public void ThreadRename()
        {
            try
            {
                lock (_ThreadRenameMedafilesLock) if (_ThreadRenameMedafiles != null || CommonQueueRenameCountDirty() <= 0) return;

                lock (_ThreadRenameMedafilesLock)
                {
                     
                    _ThreadRenameMedafiles = new Thread(() =>
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
                                lock (commonQueueRenameLock)
                                {

                                    foreach (KeyValuePair<string, string> keyValuePair in commonQueueRename)
                                    {
                                        fullFilename = keyValuePair.Key;
                                        renameVaiable = keyValuePair.Value;
                                        fileInUse = IsFileInThreadQueueLock(fullFilename);
                                        if (!fileInUse) 
                                            break; //File not in use found, start rename it
                                    }

                                }
                                #endregion

                                #region Remove from queue
                                if (!fileInUse)
                                {
                                    lock (commonQueueRenameLock)
                                    {
                                        if (commonQueueRename.ContainsKey(fullFilename)) commonQueueRename.Remove(fullFilename);
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

                                            RenameFile_Thread_UpdateTreeViewFolderBroswer(treeViewFolderBrowser1, imageListView1, fullFilename, newFullFilename);

                                        }
                                        else
                                        {
                                            Logger.Error("Was not able to read metadata after rename ");
                                            AddError(
                                                Path.GetDirectoryName(fullFilename),
                                                Path.GetFileName(fullFilename),
                                                File.GetLastWriteTime(fullFilename),
                                                AddErrorFileSystemRegion, AddErrorFileSystemMove, fullFilename, "New name is unknown (missing metadata)",
                                                "Failed rename " + fullFilename + " to : New name is unknown(missing metadata)");
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
                            KryptonMessageBox.Show("Saving ThreadRename crashed. The ThreadRename queue was cleared. Nothing was saved.", "Saving ThreadRename failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lock (commonQueueRenameLock) commonQueueRename.Clear();
                            Logger.Error(ex, "ThreadRename");
                        } finally
                        {
                            lock (_ThreadRenameMedafilesLock) _ThreadRenameMedafiles = null;
                            Logger.Trace("ThreadRename - ended");
                        }
                    });

                    lock (_ThreadRenameMedafilesLock) if (_ThreadRenameMedafiles != null)
                    {
                        _ThreadRenameMedafiles.Priority = threadPriority; 
                        _ThreadRenameMedafiles.Start();                        
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
