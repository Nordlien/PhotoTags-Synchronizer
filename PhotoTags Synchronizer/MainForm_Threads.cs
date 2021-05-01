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
using Manina.Windows.Forms;
using System.Diagnostics;
using FileDateTime;
using Thumbnails;

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : Form
    {
        private static readonly Object _ThreadCacheSelectedFastReadLock = new Object();
        private static Thread threadCacheSelectedFastRead = null;
        private static Thread _ThreadCacheSelectedFastRead
        {
            get { lock (_ThreadCacheSelectedFastReadLock) { return threadCacheSelectedFastRead; } }
            set { lock (_ThreadCacheSelectedFastReadLock) { threadCacheSelectedFastRead = value; } }
        }
        /*
        {
            get { lock (_Lock) { return data; } }
            set { lock (_Lock) { data = value; } }
        }
        */

        private static readonly Object _ThreadCacheFolderFastReadLock = new Object();
        private static Thread threadCacheFolderFastRead = null;
        private static Thread _ThreadCacheFolderFastRead
        {
            get { lock (_ThreadCacheFolderFastReadLock) { return threadCacheFolderFastRead; } }
            set { lock (_ThreadCacheFolderFastReadLock) { threadCacheFolderFastRead = value; } }
        }

        private static readonly Object _ThreadPreloadingMetadataLock = new Object();
        private static Thread threadPreloadingMetadata = null;
        private static Thread _ThreadPreloadingMetadata
        {
            get { lock (_ThreadPreloadingMetadataLock) { return threadPreloadingMetadata; } }
            set { lock (_ThreadPreloadingMetadataLock) { threadPreloadingMetadata = value; } }
        }

        private static readonly Object _ThreadLazyLoadingMetadataLock = new Object();
        private static Thread threadLazyLoadingMetadata = null;
        private static Thread _ThreadLazyLoadingMetadata
        {
            get { lock (_ThreadLazyLoadingMetadataLock) { return  threadLazyLoadingMetadata; } }
            set { lock (_ThreadLazyLoadingMetadataLock) { threadLazyLoadingMetadata = value; } }
        }

        private static readonly Object _ThreadLazyLoadingThumbnailLock = new Object();
        private static Thread threadLazyLoadingThumbnail = null;
        private static Thread _ThreadLazyLoadingThumbnail
        {
            get { lock (_ThreadLazyLoadingThumbnailLock) { return threadLazyLoadingThumbnail; } }
            set { lock (_ThreadLazyLoadingThumbnailLock) { threadLazyLoadingThumbnail = value; } }
        }

        private static readonly Object _ThreadExiftoolLock = new Object();
        private static Thread threadExiftool = null;
        private static Thread _ThreadExiftool
        {
            get { lock (_ThreadExiftoolLock) { return threadExiftool; } }
            set { lock (_ThreadExiftoolLock) { threadExiftool = value; } }
        }

        private static readonly Object _ThreadThumbnailMediaLock = new Object();
        private static Thread threadThumbnailMedia = null;
        private static Thread _ThreadThumbnailMedia
        {
            get { lock (_ThreadThumbnailMediaLock) { return threadThumbnailMedia; } }
            set { lock (_ThreadThumbnailMediaLock) { threadThumbnailMedia = value; } }
        }

        private static readonly Object _ThreadMicrosoftPhotosLock = new Object();
        private static Thread threadMicrosoftPhotos = null;
        private static Thread _ThreadMicrosoftPhotos
        {
            get { lock (_ThreadMicrosoftPhotosLock) { return threadMicrosoftPhotos; } }
            set { lock (_ThreadMicrosoftPhotosLock) { threadMicrosoftPhotos = value; } }
        }

        private static readonly Object _ThreadWindowsLiveGalleryLock = new Object();
        private static Thread threadWindowsLiveGallery = null;
        private static Thread _ThreadWindowsLiveGallery
        {
            get { lock (_ThreadWindowsLiveGalleryLock) { return threadWindowsLiveGallery; } }
            set { lock (_ThreadWindowsLiveGalleryLock) { threadWindowsLiveGallery = value; } }
        }

        private static readonly Object _ThreadThumbnailRegionLock = new Object();
        private static Thread threadThumbnailRegion = null;
        private static Thread _ThreadThumbnailRegion
        {
            get { lock (_ThreadThumbnailRegionLock) { return threadThumbnailRegion; } }
            set { lock (_ThreadThumbnailRegionLock) { threadThumbnailRegion = value; } }
        }

        private static readonly Object _ThreadSaveMetadataLock = new Object();
        private static Thread threadSaveMetadata = null;
        private static Thread _ThreadSaveMetadata
        {
            get { lock (_ThreadSaveMetadataLock) { return threadSaveMetadata; } }
            set { lock (_ThreadSaveMetadataLock) { threadSaveMetadata = value; } }
        }

        private static readonly Object _ThreadRenameMedafilesLock = new Object();
        private static Thread threadRenameMedafiles = null;
        private static Thread _ThreadRenameMedafiles
        {
            get { lock (_ThreadRenameMedafilesLock) { return threadRenameMedafiles; } }
            set { lock (_ThreadRenameMedafilesLock) { threadRenameMedafiles = value; } }
        }



        private static List<FileEntryAttribute> commonQueuePreloadingMetadata = new List<FileEntryAttribute>();
        private static readonly Object commonQueuePreloadingMetadataLock = new Object();

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
        
        private static List<string> mediaFilesNotInDatabase = new List<string>(); //It's globale, just to manage to show count status
        private static readonly Object mediaFilesNotInDatabaseLock = new Object();

        private static List<Metadata> commonQueueSaveMetadataUpdatedByUser = new List<Metadata>();
        private static readonly Object commonQueueSaveMetadataUpdatedByUserLock = new Object();
        private static List<Metadata> commonOrigialMetadataBeforeUserUpdate = new List<Metadata>();
        private static readonly Object commonOrigialMetadataBeforeUserUpdateLock = new Object();
        private static List<Metadata> commonQueueMetadataWrittenByExiftoolReadyToVerify = new List<Metadata>();
        private static readonly Object commonQueueMetadataWrittenByExiftoolReadyToVerifyLock = new Object();

        //Rename
        private static Dictionary<string, string> commonQueueRename = new Dictionary<string, string>();
        private static readonly Object  commonQueueRenameLock = new Object();

        //Error handling
        private static Dictionary<string, string> queueErrorQueue = new Dictionary<string, string>();
        private static readonly Object queueErrorQueueLock = new Object();


        #region Lock
        private int CommonQueuePreloadingMetadataCountLock()
        {
            lock (commonQueuePreloadingMetadataLock) return commonQueuePreloadingMetadata.Count;
        }

        private int CommonQueueLazyLoadingMetadataCountLock()
        {
            lock (commonQueueLazyLoadingMetadataLock) return commonQueueLazyLoadingMetadata.Count;
        }

        private int CommonQueueLazyLoadingThumbnailCountLock()
        {
            lock (commonQueueLazyLoadingThumbnailLock) return commonQueueLazyLoadingThumbnail.Count;
        }

        private int CommonQueueReadPosterAndSaveFaceThumbnailsCountLock()
        {
            lock (commonQueueReadPosterAndSaveFaceThumbnailsLock) return commonQueueReadPosterAndSaveFaceThumbnails.Count;            
        }

        private int CommonQueueSaveThumbnailToDatabaseCountLock()
        {
            lock (commonQueueSaveThumbnailToDatabaseLock) return commonQueueSaveThumbnailToDatabase.Count;
        }

        private int CommonQueueReadMetadataFromMicrosoftPhotosCountLock()
        {
            lock (commonQueueReadMetadataFromMicrosoftPhotosLock) return commonQueueReadMetadataFromMicrosoftPhotos.Count;
        }

        private int CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock()
        {
            lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) return commonQueueReadMetadataFromWindowsLivePhotoGallery.Count;
        }

        private int CommonQueueReadMetadataFromExiftoolCountLock()
        {
            lock (commonQueueReadMetadataFromExiftoolLock) return commonQueueReadMetadataFromExiftool.Count;
        }

        private int CommonQueueSaveMetadataUpdatedByUserCountLock()
        {
            lock (commonQueueSaveMetadataUpdatedByUserLock) return commonQueueSaveMetadataUpdatedByUser.Count;
        }

        private int CommonOrigialMetadataBeforeUserUpdateCountLock()
        {
            lock (commonOrigialMetadataBeforeUserUpdateLock) return commonOrigialMetadataBeforeUserUpdate.Count;
        }

        private int MediaFilesNotInDatabaseCountLock()
        {
            lock (mediaFilesNotInDatabaseLock) return mediaFilesNotInDatabase.Count;            
        }

        private int CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountLock()
        {
            lock (commonQueueMetadataWrittenByExiftoolReadyToVerifyLock) return commonQueueMetadataWrittenByExiftoolReadyToVerify.Count;
        }

        private int CommonQueueRenameCountLock()
        {
            lock (commonQueueRenameLock) return commonQueueRename.Count;
        }
        #endregion

        #region Start Thread, IsAnyThreadRunning, Tell when all queues are empty
        private void timerStartThread_Tick(object sender, EventArgs e)
        {
            StartThreads();
            TriggerAutoResetEventQueueEmpty();
        }

        private void StartThreads()
        {
            ThreadCollectMetadataExiftool();            //Read from cache first, then exifdata, 
            ThreadCollectMetadataMicrosoftPhotos();
            ThreadCollectMetadataWindowsLiveGallery();
            ThreadSaveMetadata();
            ThreadSaveThumbnail();
            ThreadReadMediaPosterSaveRegions();

            ThreadRename();
            ThreadLazyLoadningMetadata();
            ThreadLazyLoadningThumbnail();
            ThreadPreloadningMetadata();
            
        }

        private bool IsAnyThreadRunning()
        {
            return (
                (_ThreadThumbnailMedia != null && _ThreadThumbnailMedia.IsAlive) ||
                (_ThreadExiftool != null && _ThreadExiftool.IsAlive) ||
                (_ThreadWindowsLiveGallery != null && _ThreadWindowsLiveGallery.IsAlive) ||
                (_ThreadMicrosoftPhotos != null && _ThreadMicrosoftPhotos.IsAlive) ||
                (_ThreadThumbnailRegion != null && _ThreadThumbnailRegion.IsAlive) ||
                (_ThreadSaveMetadata != null && _ThreadSaveMetadata.IsAlive)
                );
        }

        private bool IsThreadRunningExcept_ThreadThumbnailRegion()
        {
            return
                CommonQueueSaveThumbnailToDatabaseCountLock() > 0 ||
                CommonQueueReadMetadataFromExiftoolCountLock() > 0 ||
                CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() > 0 ||
                CommonQueueReadMetadataFromMicrosoftPhotosCountLock() > 0 ||
                //queueThumbnailRegion.Count > 0 ||
                CommonQueueSaveMetadataUpdatedByUserCountLock() > 0;
        }

        private void TriggerAutoResetEventQueueEmpty()
        {
            if (CommonQueueSaveThumbnailToDatabaseCountLock() == 0 &&
                CommonQueueReadMetadataFromExiftoolCountLock() == 0 &&
                CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() == 0 &&
                CommonQueueReadMetadataFromMicrosoftPhotosCountLock() == 0 &&
                CommonQueueReadPosterAndSaveFaceThumbnailsCountLock() == 0 &&
                CommonQueueSaveMetadataUpdatedByUserCountLock() == 0
                )
            {
                //When out of memory, then wait for cache to empty
                if (ReadImageOutOfMemoryWillWaitCacheEmpty != null) ReadImageOutOfMemoryWillWaitCacheEmpty.Set();
            }
        }
        #endregion

        #region Preloadning

        #region Preloadning - Metadata - Clear
        public void ClearQueuePreloadningMetadata()
        {
            lock (commonQueuePreloadingMetadataLock)
            {
                commonQueuePreloadingMetadata.Clear();
            }
            StartThreads();
        }
        #endregion

        #region Preloadning - Metadata - AddQueue - Only Read
        public void AddQueuePreloadningMetadata(FileEntryAttribute fileEntryAttribute)
        {
            lock (commonQueuePreloadingMetadataLock)
            {
                if (!commonQueuePreloadingMetadata.Contains(fileEntryAttribute)) commonQueuePreloadingMetadata.Add(fileEntryAttribute);
            }
            StartThreads();
        }
        #endregion

        #region Preloadning - Metadata - Thread 
        public void ThreadPreloadningMetadata()
        {
            if ((_ThreadPreloadingMetadata == null /*|| !_ThreadPreloadingMetadata.IsAlive*/) &&
                CommonQueuePreloadingMetadataCountLock() > 0 &&
                ThreadLazyLoadingQueueSize() == 0)
            {
                try
                {
                    _ThreadPreloadingMetadata = new Thread(() =>
                    {
                        while (CommonQueuePreloadingMetadataCountLock() > 0 && !GlobalData.IsApplicationClosing && ThreadLazyLoadingQueueSize() == 0)
                        {
                            //FileEntryAttribute fileEntryAttribute;
                            //lock (commonQueuePreloadingMetadataLock) fileEntryAttribute = new FileEntryAttribute(commonQueuePreloadingMetadata[0]);
                            //_ = databaseAndCacheMetadataExiftool.ReadToCache(commonQueuePreloadingMetadataLock)
                            //_ = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.ExifTool));
                            //_ = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.MicrosoftPhotos));
                            //_ = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.WindowsLivePhotoGallery));

                            //lock (commonQueuePreloadingMetadataLock) if (commonQueuePreloadingMetadata.Count > 0) commonQueuePreloadingMetadata.RemoveAt(0);

                            List<FileEntryBroker> fileEntryBrokers = new List<FileEntryBroker>();
                            int countInQueue = CommonQueuePreloadingMetadataCountLock();
                            lock (commonQueuePreloadingMetadataLock)
                            {
                                if (countInQueue > 0)
                                {
                                    for (int indexQueue = 0; indexQueue < countInQueue; indexQueue++)
                                    {
                                        fileEntryBrokers.Add(new FileEntryBroker(commonQueuePreloadingMetadata[indexQueue], MetadataBrokerType.ExifTool));
                                        fileEntryBrokers.Add(new FileEntryBroker(commonQueuePreloadingMetadata[indexQueue], MetadataBrokerType.MicrosoftPhotos));
                                        fileEntryBrokers.Add(new FileEntryBroker(commonQueuePreloadingMetadata[indexQueue], MetadataBrokerType.WindowsLivePhotoGallery));
                                    }
                                    commonQueuePreloadingMetadata.RemoveRange(0, countInQueue);
                                }
                            }
                        }
                        Application.DoEvents();
                        StartThreads();
                        _ThreadPreloadingMetadata = null;
                    });
                
                    if (_ThreadPreloadingMetadata != null) _ThreadPreloadingMetadata.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("ThreadPreloadningMetadata.Start failed. " + ex.Message);
                    _ThreadPreloadingMetadata = null;
                }
            }

        }
        #endregion

        #region Preloading - Metadata - Thread - Faster Sqlite read for a list of files
        public void CacheSelected(List<FileEntry> searchFilterResult)
        {
            try
            {
                if (MetadataDatabaseCache.StopCaching == true || ThumbnailDatabaseCache.StopCaching == true)
                {
                    MetadataDatabaseCache.StopCaching = false;
                    ThumbnailDatabaseCache.StopCaching = false;
                    return;
                }

                if (_ThreadCacheSelectedFastRead == null)
                {
                    _ThreadCacheSelectedFastRead = new Thread(() =>
                    {
                        databaseAndCacheMetadataExiftool.ReadToCache(searchFilterResult, MetadataBrokerType.ExifTool);
                        databaseAndCacheThumbnail.ReadToCache(searchFilterResult);
                        if (cacheFolderThumbnails) databaseAndCacheThumbnail.ReadToCache(searchFilterResult); //Read missing, new media files added
                        if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCache(searchFilterResult, MetadataBrokerType.ExifTool); //Read missing, new media files added
                        if (cacheFolderWebScraperDataSets)
                            databaseAndCacheMetadataExiftool.ReadToCacheWebScraperDataSet(searchFilterResult); //Read missing, new media files added
                        MetadataDatabaseCache.StopCaching = false;
                        ThumbnailDatabaseCache.StopCaching = false;
                        _ThreadCacheSelectedFastRead = null;
                    });
                    _ThreadCacheSelectedFastRead.Start();
                }
            }
            catch
            {
                //Retry after crash, eg. thread creation failed
                MetadataDatabaseCache.StopCaching = false;
                ThumbnailDatabaseCache.StopCaching = false;
                _ThreadCacheSelectedFastRead = null;
            }
        }
        #endregion 

        #region Preloading - Metadata - Thread - Faster Sqlite read All mediafiles in *Folder*

        public void CacheFolder(string selectedFolder, FileInfo[] filesFoundInDirectory, bool recursive)
        {
            try
            {
                if (MetadataDatabaseCache.StopCaching == true || ThumbnailDatabaseCache.StopCaching == true)
                {
                    MetadataDatabaseCache.StopCaching = false; 
                    ThumbnailDatabaseCache.StopCaching = false; 
                    return; 
                }

                if (_ThreadCacheFolderFastRead == null)
                {
                    _ThreadCacheFolderFastRead = new Thread(() =>
                    {
                        if (cacheFolderThumbnails) databaseAndCacheThumbnail.ReadToCacheFolder(selectedFolder); //Read only once per folder
                        if (cacheFolderThumbnails) databaseAndCacheThumbnail.ReadToCache(filesFoundInDirectory); //Read missing, new media files added
                        if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheWhereParameters(MetadataBrokerType.Empty, selectedFolder, null, null, true); //Read only once per folder
                        if (cacheFolderMetadatas) databaseAndCacheMetadataExiftool.ReadToCache(filesFoundInDirectory, MetadataBrokerType.ExifTool); //Read missing, new media files added
                        if (cacheFolderWebScraperDataSets) databaseAndCacheMetadataExiftool.ReadToCacheWebScraperDataSet(filesFoundInDirectory); //Read missing, new media files added
                        MetadataDatabaseCache.StopCaching = false;
                        ThumbnailDatabaseCache.StopCaching = false;
                        _ThreadCacheFolderFastRead = null;
                    });
                    _ThreadCacheFolderFastRead.Start();
                }
            
            }
            catch 
            {
                //Retry after crash, eg. thread creation failed
                MetadataDatabaseCache.StopCaching = false;
                ThumbnailDatabaseCache.StopCaching = false;
                _ThreadCacheFolderFastRead = null;
            }
        }
        #endregion 


        #endregion

        #region LazyLoadning - Metadata

        #region LazyLoadding - ThreadLazyLoadingQueueSize()
        public int ThreadLazyLoadingQueueSize()
        {
            return
                //CommonQueueSaveThumbnailToDatabaseCountLock() +
                MediaFilesNotInDatabaseCountLock() +
                CommonQueueLazyLoadingThumbnailCountLock() +
                CommonQueueLazyLoadingMetadataCountLock() +
                CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() +
                CommonQueueReadMetadataFromMicrosoftPhotosCountLock() +
                CommonQueueReadMetadataFromExiftoolCountLock();
            //  CommonQueueReadPosterAndSaveFaceThumbnailsCountLock() +
            //  CommonQueueSaveMetadataUpdatedByUserCountLock();
        }
        #endregion 

        #region LazyLoadning - Metadata - AddQueue - Read from Cache, then Database, then Source and Save
        public void AddQueueMetadataReadToCacheOrUpdateFromSoruce(FileEntry fileEntry)
        {
            //When file is DELETE, LastWriteDateTime become null
            if (fileEntry.LastWriteDateTime != null)
            {
                if (File.GetLastWriteTime(fileEntry.FileFullPath) == fileEntry.LastWriteDateTime) //Don't add old files in queue
                {
                    Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                   
                    if (metadata == null) AddQueueExiftool(fileEntry); //If Metadata don't exisit in database, put it in read queue
                    
                    //if (!databaseAndCacheMetadataExiftool.MetadataHasBeenRead(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool))) AddQueueExiftool(fileEntry);
                    if (!databaseAndCacheMetadataMicrosoftPhotos.MetadataHasBeenRead(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos))) AddQueueMicrosoftPhotos(fileEntry);
                    if (!databaseAndCacheMetadataWindowsLivePhotoGallery.MetadataHasBeenRead(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery))) AddQueueWindowsLivePhotoGallery(fileEntry);
                }
            }
            else
            {
                Debug.WriteLine("AddQueueAllUpadtedFileEntry was delete: (Check why), rename of exiftool maybe, need back then... " + fileEntry.FileFullPath);
            }

            StartThreads();
            TriggerAutoResetEventQueueEmpty();
        }
        #endregion

        #region LazyLoadning - Metadata - AddQueue - Only Read
        public void AddQueueLazyLoadningMetadata(List<FileEntryAttribute> fileEntryAttributes)
        {
            if (fileEntryAttributes == null) return;
            lock (commonQueueLazyLoadingMetadataLock)
            {
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes)
                {
                    if (!commonQueueLazyLoadingMetadata.Contains(fileEntryAttribute)) commonQueueLazyLoadingMetadata.Add(fileEntryAttribute);
                }
            }
            StartThreads();
        }
        #endregion

        #region LazyLoadning - Metadata - Thread 
        public void ThreadLazyLoadningMetadata()
        {
            if((_ThreadLazyLoadingMetadata == null /*|| !_ThreadLazyLoadingMetadata.IsAlive*/) && CommonQueueLazyLoadingMetadataCountLock() > 0)
            {
                try
                {
                    _ThreadLazyLoadingMetadata = new Thread(() =>
                    {
                        //DataGridViewSuspendInvoke();
                    
                        while (CommonQueueLazyLoadingMetadataCountLock() > 0 && !GlobalData.IsApplicationClosing)
                        {
                            int queueCount = CommonQueueLazyLoadingMetadataCountLock();

                            int updatedDataGridCount = queueCount;
                            for (int queueIndex = 0; queueIndex < queueCount; queueIndex++)
                            {
                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(commonQueueLazyLoadingMetadata[queueIndex]);

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
                                        if (metadata != null && metadata.PersonalRegionIsThumbnailMissing()) AddQueueCreateRegionFromPoster(metadata);
                                    }

                                    if (databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.MicrosoftPhotos)) == null)
                                    {
                                        Metadata metadata = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.MicrosoftPhotos));
                                        if (metadata != null && metadata.PersonalRegionIsThumbnailMissing()) AddQueueCreateRegionFromPoster(metadata);
                                    }

                                    if (databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.WindowsLivePhotoGallery)) == null)
                                    {
                                        Metadata metadata = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.WindowsLivePhotoGallery));
                                        if (metadata != null && metadata.PersonalRegionIsThumbnailMissing()) AddQueueCreateRegionFromPoster(metadata);
                                    }
                                }

                                updatedDataGridCount--;
                                PopulateDataGridViewForFileEntryAttributeInvoke(fileEntryAttribute, updatedDataGridCount);
                            }

                            lock (commonQueueLazyLoadingMetadataLock)
                            {
                                for (int queueIndex = 0; queueIndex < queueCount; queueIndex++) 
                                    commonQueueLazyLoadingMetadata.RemoveAt(0);                            
                            }
                        }

                        //DataGridViewResumeInvoke();
                    
                        Application.DoEvents();
                        StartThreads();
                        _ThreadLazyLoadingMetadata = null;
                    });

                    if (_ThreadLazyLoadingMetadata != null) _ThreadLazyLoadingMetadata.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("ThreadLazyLoadningMetadata.Start failed. " + ex.Message);
                    _ThreadLazyLoadingMetadata = null;
                }
            }

        }
        #endregion 

        #endregion

        #region LazyLoadning - Thumbnail

        #region AddQueueLazyLoadningThumbnail - Only Read
        public void AddQueueLazyLoadningThumbnail(List<FileEntryAttribute> fileEntryAttributes)
        {
            if (fileEntryAttributes == null) return;
            lock (commonQueueLazyLoadingThumbnailLock)
            {
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes)
                {
                    if (!commonQueueLazyLoadingThumbnail.Contains(fileEntryAttribute)) commonQueueLazyLoadingThumbnail.Add(fileEntryAttribute);
                }
            }
            StartThreads();
        }
        #endregion

        #region LazyLoadning - Thread LazyLoadning
        public void ThreadLazyLoadningThumbnail()
        {
            if (_ThreadLazyLoadingThumbnail == null && CommonQueueLazyLoadingThumbnailCountLock() > 0)
            {

                _ThreadLazyLoadingThumbnail = new Thread(() =>
                {
                    while (CommonQueueLazyLoadingThumbnailCountLock() > 0 && !GlobalData.IsApplicationClosing)
                    {
                        int queueCount = CommonQueueLazyLoadingThumbnailCountLock();

                        for (int queueIndex = 0; queueIndex < queueCount; queueIndex++)
                        {
                            FileEntryAttribute fileEntryAttribute = commonQueueLazyLoadingThumbnail[queueIndex];

                            if (!databaseAndCacheThumbnail.DoesThumbnailExistInCache(fileEntryAttribute))
                            {
                                Image image = databaseAndCacheThumbnail.ReadThumbnailFromCacheOrDatabase(fileEntryAttribute.FileEntry);
                                if (image != null) UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(fileEntryAttribute, image);
                            }
                        }

                        lock (commonQueueLazyLoadingThumbnailLock)
                        {
                            for (int queueIndex = 0; queueIndex < queueCount; queueIndex++) commonQueueLazyLoadingThumbnail.RemoveAt(0);
                        }
                    }

                    //DataGridViewResumeInvoke();

                    Application.DoEvents();
                    StartThreads();
                    _ThreadLazyLoadingThumbnail = null;
                });
                try
                {
                    if (_ThreadLazyLoadingThumbnail != null) _ThreadLazyLoadingThumbnail.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("ThreadLazyLoadningThumbnail.Start failed. " + ex.Message);
                }
            }

        }
        #endregion

        #region AddQueue - AddQueueSaveThumbnailMedia
        public void AddQueueSaveThumbnailMedia(FileEntryImage fileEntryImage)
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
            if ((_ThreadThumbnailMedia == null /*|| !_ThreadThumbnailMedia.IsAlive*/) && CommonQueueSaveThumbnailToDatabaseCountLock() > 0)
            {
                try
                {
                    _ThreadThumbnailMedia = new Thread(() =>
                    {
                        //DataGridViewSuspendInvoke();

                        while (CommonQueueSaveThumbnailToDatabaseCountLock() > 0 && !GlobalData.IsApplicationClosing) //In case some more added to the queue or App will close
                        {
                            if (CommonQueueReadMetadataFromExiftoolCountLock() > 0) break; //Wait all metadata readfirst
                            if (CommonQueueSaveMetadataUpdatedByUserCountLock() > 0) break; //Write first, read later on...

                            try
                            {
                                FileEntryImage fileEntryImage;
                                lock (commonQueueSaveThumbnailToDatabaseLock)
                                {
                                    fileEntryImage = new FileEntryImage(commonQueueSaveThumbnailToDatabase[0]);
                                }

                                if (fileEntryImage.Image == null)
                                {
                                    fileEntryImage.Image = LoadMediaCoverArtThumbnail(fileEntryImage.FileFullPath, ThumbnailSaveSize, false);
                                    if (fileEntryImage.Image != null) ImageListViewReloadThumbnailInvoke(imageListView1, fileEntryImage.FileFullPath);
                                }

                                if (fileEntryImage.Image != null && !databaseAndCacheThumbnail.DoesThumbnailExist(fileEntryImage))
                                {
                                    databaseAndCacheThumbnail.TransactionBeginBatch();
                                    databaseAndCacheThumbnail.WriteThumbnail(fileEntryImage, fileEntryImage.Image);
                                    databaseAndCacheThumbnail.TransactionCommitBatch();

                                    UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(new FileEntryAttribute(fileEntryImage, FileEntryVersion.Current), fileEntryImage.Image);
                                    UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(new FileEntryAttribute(fileEntryImage, FileEntryVersion.Error), fileEntryImage.Image);
                                }
                                else
                                {
                                    //DEBUG, Manage to reproduce when select lot files and run log AutoCorrect Updates, Refresh
                                }

                                lock (commonQueueSaveThumbnailToDatabaseLock)
                                {
                                    if (commonQueueSaveThumbnailToDatabase.Count > 0) commonQueueSaveThumbnailToDatabase.RemoveAt(0);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("ThreadSaveThumbnail: " + ex.Message);
                            }
                            DisplayAllQueueStatus();
                        }

                        if (GlobalData.IsApplicationClosing) lock (commonQueueSaveThumbnailToDatabaseLock) commonQueueSaveThumbnailToDatabase.Clear();

                        //DataGridViewResumeInvoke();
                    
                        StartThreads();
                        TriggerAutoResetEventQueueEmpty();
                        _ThreadThumbnailMedia = null;
                    });

                    if (_ThreadThumbnailMedia != null) _ThreadThumbnailMedia.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadThumbnailMedia failed to start. " + ex.Message);
                    _ThreadThumbnailMedia = null;
                }
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
        public void AddQueueExiftool(FileEntry fileEntry)
        {
            lock (commonQueueReadMetadataFromExiftoolLock)
            {
                //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
                if (!commonQueueReadMetadataFromExiftool.Contains(fileEntry)) commonQueueReadMetadataFromExiftool.Add(fileEntry);
            }    
            RemoveError(fileEntry.FileFullPath);
            
        }
        #endregion

        #region Thread - ThreadCollectMetadataExiftool
        public void ThreadCollectMetadataExiftool()
        {
            if ((_ThreadExiftool == null /*|| !_ThreadExiftool.IsAlive*/) && CommonQueueReadMetadataFromExiftoolCountLock() > 0)
            {
                try
                {
                    _ThreadExiftool = new Thread(() =>
                    {
                        //DataGridViewSuspendInvoke();

                        while (CommonQueueReadMetadataFromExiftoolCountLock() > 0 && !GlobalData.IsApplicationClosing) //In case some more added to the queue
                        {
                            if (CommonQueueSaveMetadataUpdatedByUserCountLock() > 0) break; //Write first, read later on...

                            int rangeToRemove; //Remember how many in queue now
                            List<string> mediaFilesNotInDatabaseCheckInCloud = new List<string>(); 

                            #region From the Read Queue - Find files not alread in database
                            lock (commonQueueReadMetadataFromExiftoolLock)
                            {
                                rangeToRemove = commonQueueReadMetadataFromExiftool.Count;
                                mediaFilesNotInDatabaseCheckInCloud.AddRange(databaseAndCacheMetadataExiftool.ListAllMissingFileEntries(MetadataBrokerType.ExifTool, commonQueueReadMetadataFromExiftool.GetRange(0, rangeToRemove)));
                                commonQueueReadMetadataFromExiftool.RemoveRange(0, rangeToRemove);
                            }
                            #endregion

                            #region Check if need avoid files in cloud, if yes, don't read files in cloud
                            if (Properties.Settings.Default.AvoidOfflineMediaFiles)
                            {
                                foreach (string fullFileName in mediaFilesNotInDatabaseCheckInCloud)
                                {
                                    //Don't add files from cloud in queue
                                    if (!ExiftoolWriter.IsFileInCloud(fullFileName)) mediaFilesNotInDatabase.Add(fullFileName);
                                }
                            }
                            else
                            {
                                mediaFilesNotInDatabase.AddRange(mediaFilesNotInDatabaseCheckInCloud);
                            }
                            #endregion 

                            while (MediaFilesNotInDatabaseCountLock() > 0 && !GlobalData.IsApplicationClosing)
                            {
                                #region Create a subset of files to Read using Exiftool command line with parameters, and remove subset from queue
                                int range = 0;
                                //On computers running Microsoft Windows XP or later, the maximum length of the string that you can 
                                //use at the command prompt is 8191 characters. On computers running Microsoft Windows 2000 or 
                                //Windows NT 4.0, the maximum length of the string that you can use at the command prompt is 2047 
                                //characters.

                                int argumnetLength = 80; //Init command length;
                                List<String> useExiftoolOnThisSubsetOfFiles;
                                lock (mediaFilesNotInDatabaseLock)
                                {
                                    while (argumnetLength < 2047 && range < mediaFilesNotInDatabase.Count)
                                    {
                                        argumnetLength += mediaFilesNotInDatabase[range].Length + 3; //+3 = space and 2x"
                                        range++;
                                    }

                                    if (argumnetLength > 2047) range--;
                                    useExiftoolOnThisSubsetOfFiles = mediaFilesNotInDatabase.GetRange(0, range);
                                }
                                #endregion

                                #region Read using Exiftool
                                List<Metadata> metadataReadbackExiftoolAfterSaved = new List<Metadata>();
                                try
                                {
                                    metadataReadbackExiftoolAfterSaved = exiftoolReader.Read(MetadataBrokerType.ExifTool, useExiftoolOnThisSubsetOfFiles);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error("Running Exiftool failed" + ex.Message);
                                }
                                #endregion

                                #region Check if all files are read by Exiftool
                                string filesNotRead = "";
                                foreach (string fullFilePath in useExiftoolOnThisSubsetOfFiles)
                                {
                                    if (!Metadata.IsFullFilenameInList(metadataReadbackExiftoolAfterSaved, fullFilePath))
                                    {
                                        AddError(Path.GetDirectoryName(fullFilePath), Path.GetFileName(fullFilePath), DateTime.Now,
                                            AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                            AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                            "The file was not read by exiftool", false);
                                        filesNotRead += (filesNotRead == "" ? "" : ";") + filesNotRead;
                                    }
                                }
                                if (!string.IsNullOrEmpty(filesNotRead)) Logger.Error("Exiftool fail with read all files. Files not read: " + filesNotRead);
                                #endregion

                                #region Verify readback after saved. (Saved data is in "to be verified" queue)
                            
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

                                            AddQueueSaveThumbnailMedia(new FileEntryImage(metadataError.FileEntryBroker, null));
                                            PopulateDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadataError.FileFullPath, (DateTime)metadataError.FileDateModified, FileEntryVersion.Error));
                                        }
                                    }
                                    AddQueueCreateRegionFromPoster(metadataRead);

                                    PopulateDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadataRead.FileFullPath, (DateTime)metadataRead.FileDateModified, FileEntryVersion.Current));
                                    PopulateDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadataRead.FileFullPath, (DateTime)metadataRead.FileDateModified, FileEntryVersion.Historical));
                                    //RefreshHeaderImageAndRegionsOnActiveDataGridView(fileEntryAttribute);
                                }
                                #endregion

                                lock (mediaFilesNotInDatabaseLock) mediaFilesNotInDatabase.RemoveRange(0, range); //Remove subset from queue before update status bar
                                DisplayAllQueueStatus();
                            }
                        }

                        exiftoolReader.MetadataGroupPrioityWrite(); //Updated json config file if new tags found

                        if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromExiftoolLock) commonQueueReadMetadataFromExiftool.Clear();

                        //DataGridViewResumeInvoke();
                        PopulateDataGridViewForSelectedItemsExtrasInvoke();

                        StartThreads();
                        TriggerAutoResetEventQueueEmpty();
                        _ThreadExiftool = null;
                    });

                
                    if (_ThreadExiftool != null) _ThreadExiftool.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadExiftool.Start failed. " + ex.Message);
                    _ThreadExiftool = null;
                }
            }
        }
        #endregion

        #region ClearQueue - Exiftool
        public void ClearQueueExiftool()
        {
            lock (commonQueueReadMetadataFromExiftoolLock)
            {
                commonQueueReadMetadataFromExiftool.Clear();
            }
        }
        #endregion

        #region AddQueue - AddQueueSaveMetadataUpdatedByUser
        public void AddQueueSaveMetadataUpdatedByUser(Metadata metadataToSave, Metadata metadataOriginal)
        {
            lock (commonQueueSaveMetadataUpdatedByUserLock) commonQueueSaveMetadataUpdatedByUser.Add(metadataToSave);
            lock (commonOrigialMetadataBeforeUserUpdateLock) commonOrigialMetadataBeforeUserUpdate.Add(metadataOriginal);
            DisplayAllQueueStatus();
        }
        #endregion

        #region AddQueue - AddQueueVerifyMetadata(Metadata metadataToVerify)
        public void AddQueueVerifyMetadata(Metadata metadataToVerifyAfterSavedByExiftool)
        {
            lock (commonQueueMetadataWrittenByExiftoolReadyToVerifyLock) commonQueueMetadataWrittenByExiftoolReadyToVerify.Add(metadataToVerifyAfterSavedByExiftool);
            DisplayAllQueueStatus();
        }
        #endregion 

        #region Thread - ThreadSaveMetadata
        public void ThreadSaveMetadata()
        {
            if ((_ThreadSaveMetadata == null /*|| !_ThreadSaveMetadata.IsAlive*/) && CommonQueueSaveMetadataUpdatedByUserCountLock() > 0)
            {
                try
                {
                    _ThreadSaveMetadata = new Thread(() =>
                    {
                        #region Init Write Variables and Parameters
                        string writeMetadataTagsVariable = Properties.Settings.Default.WriteMetadataTags;
                        string writeMetadataKeywordDeleteVariable = Properties.Settings.Default.WriteMetadataKeywordDelete;
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

                        List<string> allowedFileNameDateTimeFormats = FileDateTime.FileDateTimeReader.ConvertStringOfDatesToList(Properties.Settings.Default.RenameDateFormats);
                        #endregion

                        while (CommonQueueSaveMetadataUpdatedByUserCountLock() > 0 && !GlobalData.IsApplicationClosing)
                        {
                            ShowExiftoolSaveProgressClear();

                            int writeCount = CommonQueueSaveMetadataUpdatedByUserCountLock();
                            List<Metadata> queueSubsetMetadataToSave = new List<Metadata>();    //This new values for saving (changes done by user)
                            List<Metadata> queueSubsetMetadataOrginalBeforeUserEdit = new List<Metadata>(); //Before updated by user, need this to check if any updates

                            #region Create a subset queue for writing
                            for (int i = 0; i < writeCount; i++)
                            {
                                //Remeber 
                                Metadata metadataWrite;
                                Metadata metadataOrginal;

                                lock (commonQueueSaveMetadataUpdatedByUserLock) metadataWrite = commonQueueSaveMetadataUpdatedByUser[0];
                                lock (commonOrigialMetadataBeforeUserUpdateLock) metadataOrginal = commonOrigialMetadataBeforeUserUpdate[0];

                                //Remove
                                lock (commonQueueSaveMetadataUpdatedByUserLock) commonQueueSaveMetadataUpdatedByUser.RemoveAt(0);
                                lock (commonOrigialMetadataBeforeUserUpdateLock) commonOrigialMetadataBeforeUserUpdate.RemoveAt(0);

                                lock (commonQueueReadMetadataFromExiftoolLock)
                                {
                                    if (!GlobalData.IsApplicationClosing)
                                    {
                                        //Also include Metadata ToBeSaved that are Equal with OrgianalBeforeUserEdit 
                                        if (metadataOrginal != metadataWrite) AddWatcherShowExiftoolSaveProcessQueue(metadataWrite.FileEntryBroker.FileFullPath);

                                        queueSubsetMetadataToSave.Add(metadataWrite);
                                        queueSubsetMetadataOrginalBeforeUserEdit.Add(metadataOrginal);
                                    }
                                }
                            }
                            #endregion

                            //Wait file to be unloacked, if used by a process. E.g. some application writing to file, or OneDrive doing backup
                            if (!GlobalData.IsApplicationClosing) ExiftoolWriter.WaitLockedFilesToBecomeUnlocked(queueSubsetMetadataToSave);

                            #region Write Xtra Atom properites
                            Dictionary<string, string> writeXtraAtomErrorMessageForFile = new Dictionary<string, string>();
                            List<FileEntry> filesUpdatedByWritePropertiesAndLastWriteTime = new List<FileEntry>();

                            if (!GlobalData.IsApplicationClosing)
                            {
                                UpdateStatusAction("Write Xtra Atom to " + queueSubsetMetadataToSave.Count + " media files...");

                                filesUpdatedByWritePropertiesAndLastWriteTime = ExiftoolWriter.WriteXtraAtom(
                                    queueSubsetMetadataToSave, queueSubsetMetadataOrginalBeforeUserEdit, allowedFileNameDateTimeFormats,
                                    writeXtraAtomAlbumVariable, writeXtraAtomAlbumVideo,
                                    writeXtraAtomCategoriesVariable, writeXtraAtomCategoriesVideo,
                                    writeXtraAtomCommentVariable, writeXtraAtomCommentPicture, writeXtraAtomCommentVideo,
                                    writeXtraAtomKeywordsVariable, writeXtraAtomKeywordsVideo,
                                    writeXtraAtomRatingPicture, writeXtraAtomRatingVideo,
                                    writeXtraAtomSubjectVariable, writeXtraAtomSubjectPicture, wtraAtomSubjectVideo,
                                    writeXtraAtomSubtitleVariable, writeXtraAtomSubtitleVideo,
                                    writeXtraAtomArtistVariable, writeXtraAtomArtistVideo,
                                    out writeXtraAtomErrorMessageForFile);
                            }
                            #endregion

                            #region File Create date and Time attribute
                            if (!GlobalData.IsApplicationClosing)
                            {
                                foreach (Metadata metadata in queueSubsetMetadataToSave)
                                {
                                    if (metadata.TryParseDateTakenToUtc(out DateTime? dateTakenWithOffset))
                                    {
                                        if (metadata?.FileDateCreated != null &&
                                            metadata?.MediaDateTaken != null &&
                                            metadata?.MediaDateTaken < DateTime.Now &&
                                            Math.Abs(((DateTime)dateTakenWithOffset - (DateTime)metadata?.FileDateCreated).TotalSeconds) > 10) //No need to change
                                        {
                                            try
                                            {
                                                File.SetCreationTime(metadata.FileFullPath, (DateTime)dateTakenWithOffset);
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.Error("File.SetCreationTime failed...\r\n\r\n" + ex.Message);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Save Metadatas using Exiftool   
                            List<FileEntry> mediaFilesWithChangesWillBeUpdated = new List<FileEntry>();
                            string exiftoolErrorMessage = "";

                            if (!GlobalData.IsApplicationClosing)
                            {
                                try
                                {
                                    UpdateStatusAction("Batch update a subset of " + queueSubsetMetadataToSave.Count + " media files...");
                                    mediaFilesWithChangesWillBeUpdated = ExiftoolWriter.WriteMetadata(
                                        queueSubsetMetadataToSave, queueSubsetMetadataOrginalBeforeUserEdit, allowedFileNameDateTimeFormats,
                                        writeMetadataTagsVariable, writeMetadataKeywordDeleteVariable, writeMetadataKeywordAddVariable);
                                }
                                catch (Exception ex)
                                {
                                    exiftoolErrorMessage = ex.Message;
                                    Logger.Error("EXIFTOOL.EXE error...\r\n\r\n" + ex.Message);
                                }
                            }
                            #endregion

                            if (!GlobalData.IsApplicationClosing) ExiftoolWriter.WaitLockedFilesToBecomeUnlocked(queueSubsetMetadataToSave);

                            #region Check if all files was updated, if updated, add to verify queue
                            if (!GlobalData.IsApplicationClosing)
                            {
                                foreach (FileEntry fileSuposeToBeUpdated in mediaFilesWithChangesWillBeUpdated)
                                {

                                    bool failToSaveXtraAtom = false;
                                    //Check if writing Xtra Atom properties failed
                                    if (writeXtraAtomErrorMessageForFile.ContainsKey(fileSuposeToBeUpdated.FileFullPath))
                                    {
                                        failToSaveXtraAtom = true;
                                        AddError(fileSuposeToBeUpdated.Directory, fileSuposeToBeUpdated.FileName, fileSuposeToBeUpdated.LastWriteDateTime,
                                            AddErrorExiftooRegion, AddErrorExiftooCommandWrite, AddErrorExiftooParameterWrite, AddErrorExiftooParameterWrite,
                                            "Failed write Xtra Atom property to file: " + fileSuposeToBeUpdated.FileFullPath + "\r\n" +
                                            "Error message:" + writeXtraAtomErrorMessageForFile[fileSuposeToBeUpdated.FileFullPath]);
                                    }

                                    bool failToSaveUsingExiftool = false;
                                    DateTime currentLastWrittenDateTime = File.GetLastWriteTime(fileSuposeToBeUpdated.FileFullPath);
                                    DateTime lastestKnownLastWrittenDateTime = (DateTime)fileSuposeToBeUpdated.LastWriteDateTime;

                                    //Find last known writtenDate for file
                                    int index = FileEntry.FindIndex(filesUpdatedByWritePropertiesAndLastWriteTime, fileSuposeToBeUpdated);
                                    if (index > -1) lastestKnownLastWrittenDateTime = filesUpdatedByWritePropertiesAndLastWriteTime[index].LastWriteDateTime;
                                    //Check if file is updated, if file LastWrittenDateTime has changed, file is updated
                                    if (lastestKnownLastWrittenDateTime == currentLastWrittenDateTime)
                                    {
                                        failToSaveUsingExiftool = true;
                                        AddError(fileSuposeToBeUpdated.Directory, fileSuposeToBeUpdated.FileName, fileSuposeToBeUpdated.LastWriteDateTime,
                                                AddErrorExiftooRegion, AddErrorExiftooCommandWrite, AddErrorExiftooParameterWrite, AddErrorExiftooParameterWrite,
                                                "EXIFTOOL.EXE failed write to file:" + fileSuposeToBeUpdated.FileFullPath + "\r\n" +
                                                "Message return from Exiftool: " + exiftoolErrorMessage);
                                    }

                                    int indexInVerifyQueue = Metadata.FindFileEntryInList(queueSubsetMetadataToSave, fileSuposeToBeUpdated);
                                
                                    if (!failToSaveXtraAtom && !failToSaveUsingExiftool && indexInVerifyQueue > -1 && indexInVerifyQueue < queueSubsetMetadataToSave.Count)
                                    {
                                        Metadata currentMetadata = new Metadata(queueSubsetMetadataToSave[indexInVerifyQueue]);
                                        currentMetadata.FileDateModified = currentLastWrittenDateTime;
                                        AddQueueVerifyMetadata(currentMetadata);
                                        AddQueueMetadataReadToCacheOrUpdateFromSoruce(currentMetadata.FileEntryBroker);
                                        ImageListViewReloadThumbnailInvoke(imageListView1, fileSuposeToBeUpdated.FileFullPath);
                                    }
                                }
                            }
                            #endregion

                            //Clean up
                            queueSubsetMetadataToSave.Clear();
                            queueSubsetMetadataOrginalBeforeUserEdit.Clear();
                            mediaFilesWithChangesWillBeUpdated.Clear();

                            //Status updated for user
                            ShowExiftoolSaveProgressClear();
                            DisplayAllQueueStatus();

                            //Thread.Sleep(100); //Wait in case of loop
                        }

                        if (GlobalData.IsApplicationClosing)
                        {
                            lock (commonQueueSaveMetadataUpdatedByUserLock) commonQueueSaveMetadataUpdatedByUser.Clear();
                            lock (commonOrigialMetadataBeforeUserUpdateLock) commonOrigialMetadataBeforeUserUpdate.Clear();
                        }

                        StartThreads();
                        TriggerAutoResetEventQueueEmpty();
                        _ThreadSaveMetadata = null;
                    });

                
                    if (_ThreadSaveMetadata != null) _ThreadSaveMetadata.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadSaveMetadata.Start failed. " + ex.Message);
                    _ThreadSaveMetadata = null;
                }

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
        public void AddQueueMicrosoftPhotos(FileEntry fileEntry)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueReadMetadataFromMicrosoftPhotosLock)
            {
                if (!commonQueueReadMetadataFromMicrosoftPhotos.Contains(fileEntry)) commonQueueReadMetadataFromMicrosoftPhotos.Add(fileEntry);
            }
        }
        #endregion

        #region Thread - ThreadCollectMetadataWindowsLiveGallery
        public void ThreadCollectMetadataWindowsLiveGallery()
        {
            if ((_ThreadWindowsLiveGallery == null /*|| !_ThreadWindowsLiveGallery.IsAlive*/) && 
                CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() > 0)
            {
                try
                {
                    _ThreadWindowsLiveGallery = new Thread(() =>
                    {
                        //DataGridViewSuspendInvoke();

                        while (CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() > 0 && !GlobalData.IsApplicationClosing)
                        {
                            #region Common for ThreadCollectMetadataWindowsLiveGallery and ThreadCollectMetadataMicrosoftPhotos
                            MetadataDatabaseCache database = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            ImetadataReader databaseSourceReader = databaseWindowsLivePhotGallery;
                            MetadataBrokerType broker = MetadataBrokerType.WindowsLivePhotoGallery;

                            while (databaseSourceReader != null && !GlobalData.IsApplicationClosing && CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() > 0) //In case some more added to the queue
                            {
                                Metadata metadata;
                                FileEntry fileEntry;
                                lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) fileEntry = new FileEntry(commonQueueReadMetadataFromWindowsLivePhotoGallery[0]);

                                if (File.Exists(fileEntry.FileFullPath))
                                {
                                    metadata = databaseSourceReader.Read(broker, fileEntry.FileFullPath); //Read from broker as Microsoft Photos, Windows Live Photo Gallery (Using NamedPipes)
                                    if (metadata != null) // && broker != MetadataBrokerTypes.WindowsLivePhotoGallery)
                                    {
                                        //Windows Live Photo Gallery writes direclty to database from sepearte thread when found
                                        database.TransactionBeginBatch();
                                        database.Write(metadata);
                                        database.TransactionCommitBatch();
                                        AddQueueCreateRegionFromPoster(metadata);

                                        PopulateDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadata.FileFullPath, (DateTime)metadata.FileDateModified, FileEntryVersion.Current));
                                    }
                                }
                                else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                                lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) if (commonQueueReadMetadataFromWindowsLivePhotoGallery.Count > 0) commonQueueReadMetadataFromWindowsLivePhotoGallery.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar

                                DisplayAllQueueStatus();
                            }
                            #endregion 

                            DisplayAllQueueStatus();
                        }

                        if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) commonQueueReadMetadataFromWindowsLivePhotoGallery.Clear();

                        //DataGridViewResumeInvoke();

                        StartThreads();
                        TriggerAutoResetEventQueueEmpty();
                        _ThreadWindowsLiveGallery = null;
                    });
                
                    if (_ThreadWindowsLiveGallery != null) _ThreadWindowsLiveGallery.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadWindowsLiveGallery.Start failed. " + ex.Message);
                    _ThreadWindowsLiveGallery = null;
                }
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
        public void AddQueueWindowsLivePhotoGallery(FileEntry fileEntry)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock)
            {
                if (!commonQueueReadMetadataFromWindowsLivePhotoGallery.Contains(fileEntry)) commonQueueReadMetadataFromWindowsLivePhotoGallery.Add(fileEntry);
            }
        }
        #endregion

        #region Thread - ThreadCollectMetadataMicrosoftPhotos 
        public void ThreadCollectMetadataMicrosoftPhotos()
        {
            if ((_ThreadMicrosoftPhotos == null /*|| !_ThreadMicrosoftPhotos.IsAlive*/) && CommonQueueReadMetadataFromMicrosoftPhotosCountLock() > 0)
            {
                try
                {
                    _ThreadMicrosoftPhotos = new Thread(() =>
                    {
                        //DataGridViewSuspendInvoke();

                        while (CommonQueueReadMetadataFromMicrosoftPhotosCountLock() > 0 && !GlobalData.IsApplicationClosing)
                        {
                            #region Common for ThreadCollectMetadataWindowsLiveGallery and ThreadCollectMetadataMicrosoftPhotos
                            MetadataDatabaseCache database = databaseAndCacheMetadataMicrosoftPhotos;
                            ImetadataReader databaseSourceReader = databaseMicrosoftPhotos;
                            MetadataBrokerType broker = MetadataBrokerType.MicrosoftPhotos;

                            while (databaseSourceReader != null && !GlobalData.IsApplicationClosing && CommonQueueReadMetadataFromMicrosoftPhotosCountLock() > 0) //In case some more added to the queue
                            {
                                Metadata metadata;
                                FileEntry fileEntry;
                                lock (commonQueueReadMetadataFromMicrosoftPhotosLock) fileEntry = new FileEntry(commonQueueReadMetadataFromMicrosoftPhotos[0]);

                                if (File.Exists(fileEntry.FileFullPath))
                                {
                                    metadata = databaseSourceReader.Read(broker, fileEntry.FileFullPath); //Read from broker as Microsoft Photos, Windows Live Photo Gallery (Using NamedPipes)
                                    if (metadata != null) // && broker != MetadataBrokerTypes.WindowsLivePhotoGallery)
                                    {
                                        //Windows Live Photo Gallery writes direclty to database from sepearte thread when found
                                        database.TransactionBeginBatch();
                                        database.Write(metadata);
                                        database.TransactionCommitBatch();
                                        AddQueueCreateRegionFromPoster(metadata);

                                        PopulateDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(metadata.FileFullPath, (DateTime)metadata.FileDateModified, FileEntryVersion.Current));
                                    }
                                }
                                else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                                lock (commonQueueReadMetadataFromMicrosoftPhotosLock) if (commonQueueReadMetadataFromMicrosoftPhotos.Count > 0) commonQueueReadMetadataFromMicrosoftPhotos.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar

                                DisplayAllQueueStatus();
                            }
                            #endregion

                            DisplayAllQueueStatus();
                        }

                        if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromMicrosoftPhotosLock) commonQueueReadMetadataFromMicrosoftPhotos.Clear();

                        //DataGridViewResumeInvoke();

                        StartThreads();
                        TriggerAutoResetEventQueueEmpty();
                        _ThreadMicrosoftPhotos = null;
                    });

                    if (_ThreadMicrosoftPhotos != null) _ThreadMicrosoftPhotos.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadMicrosoftPhotos.Start failed. " + ex.Message);
                    _ThreadMicrosoftPhotos = null;
                }
            }
        }
        #endregion

        #endregion

        #region Region 

        #region AddQueue - AddQueueCreateRegionFromPoster(Metadata metadata)
        private void AddQueueCreateRegionFromPoster(Metadata metadata)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
            {
                if (!commonQueueReadPosterAndSaveFaceThumbnails.Contains(metadata)) commonQueueReadPosterAndSaveFaceThumbnails.Add(metadata);
            }
            StartThreads();
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
            if ((_ThreadThumbnailRegion == null /*|| !_ThreadThumbnailRegion.IsAlive*/) && CommonQueueReadPosterAndSaveFaceThumbnailsCountLock() > 0)
            {
                if (IsThreadRunningExcept_ThreadThumbnailRegion()) return; //Wait other thread to finnish first. Otherwise it will generate high load on disk use

                try
                {
                    _ThreadThumbnailRegion = new Thread(() =>
                    {      
                    
                        while (CommonQueueReadPosterAndSaveFaceThumbnailsCountLock() > 0 && !GlobalData.IsApplicationClosing && !IsThreadRunningExcept_ThreadThumbnailRegion()) //In case some more added to the queue
                        {

                            

                            try
                            {                            
                                FileEntry fileEntryRegion;
                                lock (commonQueueReadPosterAndSaveFaceThumbnailsLock) 
                                    { fileEntryRegion = new FileEntry(commonQueueReadPosterAndSaveFaceThumbnails[0].FileEntryBroker); }

                                bool fileFoundNeedCheckForMore;
                                int fileIndexFound;
                                bool fileFoundInList = false;

                                do //Remove all with same filename in the queue
                                {
                                    fileFoundNeedCheckForMore = false;
                                    fileIndexFound = -1;

                                    //Check Exiftool, Microsoft Phontos, Windows Live Photo Gallery in queue also

                                    Image image = null; //No image loaded

                                    int queueCount = CommonQueueReadPosterAndSaveFaceThumbnailsCountLock(); //Mark count that we will work with. 
                                    for (int thumbnailIndex = 0; thumbnailIndex < queueCount; thumbnailIndex++)
                                    {
                                        Metadata metadataActiveCopy = null;
                                        lock (commonQueueReadPosterAndSaveFaceThumbnailsLock) 
                                            { metadataActiveCopy = new Metadata(commonQueueReadPosterAndSaveFaceThumbnails[thumbnailIndex]); }

                                        //Find current file entry in queue, Exiftool, Microsoft Photos, Windows Live Gallery, etc...
                                        if (metadataActiveCopy.FileFullPath == fileEntryRegion.FileFullPath &&
                                            metadataActiveCopy.FileDateModified == fileEntryRegion.LastWriteDateTime)
                                        {
                                            fileFoundNeedCheckForMore = true;
                                            fileIndexFound = thumbnailIndex;
                                            fileFoundInList = true;
                                            //When found entry, check if has Face Regions to save
                                            if (metadataActiveCopy.PersonalRegionList.Count > 0)
                                            {
                                                ExiftoolWriter.WaitLockedFileToBecomeUnlocked(fileEntryRegion.FileFullPath);

                                                //Check if the current Metadata are same as newst file...
                                                //If not file exist anymore, date will become {01.01.1601 01:00:00}
                                                if (File.Exists(fileEntryRegion.FileFullPath) && File.GetLastWriteTime(fileEntryRegion.FileFullPath) == fileEntryRegion.LastWriteDateTime)
                                                {
                                                    if (image == null) image = LoadMediaCoverArtPoster(fileEntryRegion.FileFullPath, true); //Only load once when found

                                                    if (image != null) //If still Failed load cover art, often occur after filed is moved or deleted
                                                    {
                                                        databaseAndCacheThumbnail.TransactionBeginBatch(); //Only load image when regions found
                                                        //Metadata found and updated, updated DataGricView                                             
                                                        RegionThumbnailHandler.SaveThumbnailsForRegioList(databaseAndCacheMetadataExiftool, metadataActiveCopy, image);

                                                        fileFoundInList = true;
                                                        databaseAndCacheThumbnail.TransactionCommitBatch();

                                                        PopulateDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(fileEntryRegion, FileEntryVersion.Current)); //Updated Gridview
                                                    }
                                                    else
                                                    {
                                                        fileFoundInList = false;

                                                        string writeErrorDesciption = "Failed loading file. Was not able to update thumbnail for region for the file:" + fileEntryRegion.FileFullPath;
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
                                                //else Logger.Info("Don't load posters when request are with Diffrent LastWrittenDateTime:" + commonQueueReadPosterAndSaveFaceThumbnails[0].FileName);
                                            } 
                                    
                                            if (fileFoundNeedCheckForMore) break; //No need to search more.
                                        }
                                    }

                                    lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                                    {
                                        if (fileIndexFound > -1) commonQueueReadPosterAndSaveFaceThumbnails.RemoveAt(fileIndexFound);
                                    }


                                } while (fileFoundNeedCheckForMore);

                                if (!fileFoundInList)
                                {
                                    string writeErrorDesciption = "ThreadReadMediaPosterSaveRegions, file not found list for updated:" + fileEntryRegion.FileFullPath;
                                    Logger.Error(writeErrorDesciption);                                    
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Error("ThreadReadMediaPosterSaveRegions crashed" + e.Message);
                            }

                            DisplayAllQueueStatus();
                        }

                        if (GlobalData.IsApplicationClosing) lock (commonQueueReadPosterAndSaveFaceThumbnailsLock) commonQueueReadPosterAndSaveFaceThumbnails.Clear();
                        StartThreads();
                        TriggerAutoResetEventQueueEmpty();
                        _ThreadThumbnailRegion = null;
                    });
                
                    if (_ThreadThumbnailRegion != null) _ThreadThumbnailRegion.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadThumbnailRegion.Start failed. " + ex.Message);
                    _ThreadThumbnailRegion = null;
                }
            }

        }
        #endregion

        #endregion

        #region Rename

        #region Rename - AddQueueRename
        public void AddQueueRename (string fullFileName, string renameVariable)
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
            

            if ((_ThreadRenameMedafiles == null /*|| !_ThreadRenameMedafiles.IsAlive*/) && CommonQueueRenameCountLock() > 0)
            {
                _ThreadRenameMedafiles = new Thread(() =>
                {
                    while (CommonQueueRenameCountLock() > 0 && !GlobalData.IsApplicationClosing)
                    {
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

                                #region commonQueueReadPosterAndSaveFaceThumbnails
                                lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                                {
                                    foreach (Metadata metadata in commonQueueReadPosterAndSaveFaceThumbnails)
                                    {
                                        if (metadata.FileFullPath == fullFilename)
                                        {
                                            fileInUse = true;
                                            break;
                                        }
                                    }
                                }
                                #endregion 

                                #region commonQueueSaveThumbnailToDatabase
                                if (!fileInUse)
                                    lock (commonQueueSaveThumbnailToDatabaseLock)
                                    {
                                        foreach (FileEntryImage fileEntry in commonQueueSaveThumbnailToDatabase)
                                        {
                                            if (fileEntry.FileFullPath == fullFilename)
                                            {
                                                fileInUse = true;
                                                break;
                                            }
                                        }
                                    }
                                #endregion

                                #region commonQueueReadMetadataFromMicrosoftPhotos
                                if (!fileInUse) 
                                    lock(commonQueueReadMetadataFromMicrosoftPhotosLock)
                                    {
                                        foreach (FileEntryImage fileEntry in commonQueueReadMetadataFromMicrosoftPhotos)
                                        {
                                            if (fileEntry.FileFullPath == fullFilename)
                                            {
                                                fileInUse = true;
                                                break;
                                            }
                                        }
                                    }
                                #endregion

                                #region commonQueueReadMetadataFromWindowsLivePhotoGallery
                                if (!fileInUse)
                                    lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock)
                                    foreach (FileEntryImage fileEntry in commonQueueReadMetadataFromWindowsLivePhotoGallery)
                                    {
                                        if (fileEntry.FileFullPath == fullFilename)
                                        {
                                            fileInUse = true;
                                            break;
                                        }
                                    }
                                #endregion

                                #region commonQueueReadMetadataFromExiftool
                                if (!fileInUse)
                                    lock(commonQueueReadMetadataFromExiftoolLock)
                                    foreach (FileEntryImage fileEntry in commonQueueReadMetadataFromExiftool)
                                    {
                                        if (fileEntry.FileFullPath == fullFilename)
                                        {
                                            fileInUse = true;
                                            break;
                                        }
                                    }
                                #endregion

                                #region commonQueueSaveMetadataUpdatedByUser
                                if (!fileInUse)
                                    lock (commonQueueSaveMetadataUpdatedByUserLock)
                                    {
                                        foreach (Metadata metadata in commonQueueSaveMetadataUpdatedByUser)
                                        {
                                            if (metadata.FileFullPath == fullFilename)
                                            {
                                                fileInUse = true;
                                                break;
                                            }
                                        }
                                    }
                                #endregion
                            
                            }
                        }
                        #endregion 

                        #region Remove from qeueu
                        if (!fileInUse)
                        {
                            lock (commonQueueRenameLock)
                            {
                                if (commonQueueRename.ContainsKey(fullFilename)) commonQueueRename.Remove(fullFilename);
                            }
                        }
                        #endregion

                        #region Do the renameing process
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
                                string newRelativeFilename = Path.Combine(oldDirectory, newFilename);
                                string newFullFilename = Path.GetFullPath(newRelativeFilename);
                                #endregion 

                                MoveFile(folderTreeViewFolder, imageListView1, fullFilename, newFullFilename);

                            } else
                            {
                                Logger.Error("Was not able to read metadata after rename ");
                            }

                        }
                        #endregion

                        //Status updated for user
                        ShowExiftoolSaveProgressClear();
                        DisplayAllQueueStatus();

                        //Thread.Sleep(100); //Wait in case of loop
                    }

                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                    _ThreadRenameMedafiles = null;
                });
                try
                {
                    if (_ThreadRenameMedafiles != null) _ThreadRenameMedafiles.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadRenameMedafiles.Start failed. " + ex.Message);
                }
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

        private static FormMessageBox formMessageBox = null;
        private void timerShowErrorMessage_Tick(object sender, EventArgs e)
        {
            timerShowErrorMessage.Stop();
            if (hasWriteAndVerifyMetadataErrors)
            {
                string errors = listOfErrors;
                listOfErrors = "";
                hasWriteAndVerifyMetadataErrors = false;

                //MessageBox.Show(errors, "Warning or Errors has occured!", MessageBoxButtons.OK);
                if (formMessageBox == null || formMessageBox.IsDisposed) formMessageBox = new FormMessageBox(errors);
                else formMessageBox.AppendMessage(errors);
                formMessageBox.Owner = this;
                formMessageBox.Show();
            }
            try
            {
                timerShowErrorMessage.Start();
            }
            catch (Exception ex)
            {
                Logger.Error("timerShowErrorMessage.Start failed. " + ex.Message);
            }
        }
        #endregion

    }
}
