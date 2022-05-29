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
        private static ThreadPriority threadPriority = ThreadPriority.Lowest;

        #region Thread variables

        #region LazyLoading
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

        private static readonly Object _ThreadAutoCorrectLock = new Object();
        private static Thread _ThreadAutoCorrect = null; //
        #endregion

        #endregion

        #region Process Queues

        #region LazyLoading
        private static List<FileEntryBroker> commonQueueLazyLoadingSelectedFiles = new List<FileEntryBroker>();
        private static readonly Object commonQueueLazyLoadingSelectedFilesLock = new Object();

        private static List<FileEntryAttribute> commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails = new List<FileEntryAttribute>();
        private static readonly Object commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock = new Object();

        private static List<FileEntryAttribute> commonQueueLazyLoadingMediaThumbnail = new List<FileEntryAttribute>();
        private static readonly Object          commonQueueLazyLoadingMediaThumbnailLock = new Object();

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

        //AutoCorrect
        private static Dictionary<FileEntryBroker, AutoCorrectFormVaraibles> commonQueueAutoCorrect = new Dictionary<FileEntryBroker, AutoCorrectFormVaraibles>();
        private static readonly Object commonQueueAutoCorrectLock = new Object();
        #endregion

        #region Exiftool Save
        private static List<Metadata> exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser = new List<Metadata>();
        private static readonly Object exiftoolSave_QueueSaveUsingExiftool_MetadataUpdatedByUserLock = new Object();

        private static List<Metadata> exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate = new List<Metadata>();
        private static readonly Object exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUserLock = new Object();

        private static List<Metadata> exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify = new List<Metadata>();
        private static readonly Object exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock = new Object();

        private static List<FileEntry> exiftool_MediaFilesNotInDatabase = new List<FileEntry>(); //It's globale, just to manage to show count status
        private static readonly Object exiftool_MediaFilesNotInDatabaseLock = new Object();

        private static List<Metadata> exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser = new List<Metadata>();
        private static readonly Object exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock = new Object();
        #endregion

        #endregion

        #region Error handling
        private static Dictionary<string, string> queueErrorQueue = new Dictionary<string, string>();
        private static readonly Object queueErrorQueueLock = new Object();
        #endregion

        #region Get Count of items in Queue with Lock

        #region CommonQueueLazyLoadingMapNomnatatimLock()
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
        #endregion

        #region CommonQueueLazyLoadingAllSourcesAllMetadataAndThumbnailCountLock
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
        #endregion

        #region CommonQueueLazyLoadingThumbnailCountLock
        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueLazyLoadingThumbnailCountLock()
        {
            lock   (commonQueueLazyLoadingMediaThumbnailLock) return commonQueueLazyLoadingMediaThumbnail.Count;
        }

        private int CommonQueueLazyLoadingThumbnailCountDirty()
        {
            lock   (commonQueueLazyLoadingMediaThumbnailLock) return commonQueueLazyLoadingMediaThumbnail.Count;
        }
        #endregion

        #region CommonQueueSaveToDatabaseRegionAndThumbnailCountLock
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
        #endregion

        #region CommonQueueSaveToDatabaseMediaThumbnailCountLock
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
        #endregion

        #region CommonQueueReadMetadataFromMicrosoftPhotosCountLock
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
        #endregion

        #region CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountLock
        private int CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountLock()
        {
            lock   (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) return commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Count;
        }

        private int CommonQueueReadMetadataFromSourceWindowsLivePhotoGalleryCountDirty()
        {
            return  commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Count;
        }
        #endregion

        #region CommonQueueReadMetadataFromSourceExiftoolCountLock
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
        #endregion

        #region CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountLock
        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountLock()
        {
            lock   (exiftoolSave_QueueSaveUsingExiftool_MetadataUpdatedByUserLock) return exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.Count;
        }

        private int CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountDirty()
        {
            return exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.Count;
        }
        #endregion

        #region ExiftoolSave_MediaFilesNotInDatabaseCountLock
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
        #endregion

        #region CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty
        /// <summary>
        /// Get Count of items in Queue 
        /// </summary>
        /// <returns>Number of items in queue</returns>
        private int CommonQueueMetadataWrittenByExiftoolReadyToVerifyCountDirty()
        {
            return exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify.Count;
        }
        #endregion

        #region CommonQueueRenameCountLock
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

        #region CommonQueueAutoCorrectLock
        private int CommonQueueAutoCorrectLock()
        {
            lock (commonQueueAutoCorrectLock) return commonQueueAutoCorrect.Count;
        }

        private int CommonQueueAutoCorrectDirty()
        {
            return commonQueueAutoCorrect.Count;
        }
        #endregion

        #endregion

        #region Start Thread, IsAnyThreadRunning, Tell when all queues are empty

        #region timerStartThread_Tick
        /// <summary>
        /// Start all background thread every x ms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerStartThread_Tick(object sender, EventArgs e)
        {
            if (!GlobalData.IsApplicationClosing) StartThreads();
        }
        #endregion

        #region StartThreads()
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
            ThreadAutoCorrect();

            ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails();
            ThreadLazyLoadingMediaThumbnail();
            ThreadQueueLazyLoadingMapNomnatatim();            
        }
        #endregion

        #region IsAnyThreadRunning()
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
        #endregion

        #region IsThreadRunningExcept_ThreadSaveToDatabaseRegionAndThumbnail()
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
        #endregion

        #endregion

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

            StartThreads();

            MetadataDatabaseCache.StopCaching = false;
            ThumbnailPosterDatabaseCache.StopCaching = false;

            GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = false;
            GlobalData.IsStopAndEmptyThumbnailQueueRequest = false;

        }
        #endregion

        #region Keep control what Select Files has data loaded - To updated / refrersh screen when all loaded
        
        #region LazyLoading - Clear SelectedFiles
        public void ClearQueueLazyLoadningSelectedFilesLock()
        {
            lock (commonQueueLazyLoadingSelectedFilesLock)
            {
                commonQueueLazyLoadingSelectedFiles.Clear();
            }
        }
        #endregion

        #region LazyLoading - Add SelectedFiles
        public void AddQueueLazyLoadningSelectedFilesLock(FileEntryBroker fileEntryBroker)
        {
            lock (commonQueueLazyLoadingSelectedFilesLock)
            {
                if (ImageListViewHandler.DoesExistInSelectedFiles(imageListView1, fileEntryBroker.FileFullPath)) //JTN: Loop Starts an endless loop, due to checking DateModified
                {
                    if (!commonQueueLazyLoadingSelectedFiles.Contains(fileEntryBroker)) commonQueueLazyLoadingSelectedFiles.Add(fileEntryBroker);
                }
            }
        }
        #endregion

        #region LazyLoading - Remove SelectedFiles
        public void RemoveQueueLazyLoadningSelectedFilesLock(FileEntryBroker fileEntryBroker)
        {
            lock (commonQueueLazyLoadingSelectedFilesLock)
            {
                if (commonQueueLazyLoadingSelectedFiles.Contains(fileEntryBroker)) commonQueueLazyLoadingSelectedFiles.Remove(fileEntryBroker);
            }
        }
        #endregion

        #region LazyLoading - Count SelectedFiles
        public int CountQueueLazyLoadningSelectedFilesLock()
        {
            lock (commonQueueLazyLoadingSelectedFilesLock)
            {
                return commonQueueLazyLoadingSelectedFiles.Count;
            }
        }
        #endregion

        #endregion

        #region LazyLoading

        #region LazyLoading - Add - All Versions / All Sources / Metadata And Region Thumbnails - AddQueue - (Read order: Cache, Database, Source) - AfterPopulateSelectedFiles 
        private void AddQueueLazyLoadning_AllSources_AllVersions_MetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(HashSet<FileEntry> imageListViewSelectItems)
        {
            List<FileEntryAttribute> lazyLoadingAllExiftoolVersionOfMediaFile = new List<FileEntryAttribute>();

            foreach (FileEntry fileEntry in imageListViewSelectItems)
            {
                List<FileEntryAttribute> fileEntryAttributeDateVersions =
                    databaseAndCacheMetadataExiftool.ListFileEntryAttributesCache(MetadataBrokerType.ExifTool,
                    fileEntry.FileFullPath, FileHandler.GetLastWriteTime(fileEntry.FileFullPath, waitAndRetry: IsFileInTemporaryUnavailableLock(fileEntry.FileFullPath)));

                lazyLoadingAllExiftoolVersionOfMediaFile.AddRange(fileEntryAttributeDateVersions);

                FileStatus fileStaus = FileHandler.GetFileStatus(fileEntry.FileFullPath,
                    exiftoolProcessStatus: ExiftoolProcessStatus.InExiftoolReadQueue);

                FileHandler.RemoveOfflineFileTouched(fileEntry.FileFullPath);
                FileHandler.RemoveOfflineFileTouchedFailed(fileEntry.FileFullPath);

                ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStaus);
            }

            AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(lazyLoadingAllExiftoolVersionOfMediaFile);
            AddQueueLazyLoadningMediaThumbnailLock(lazyLoadingAllExiftoolVersionOfMediaFile);
            AddQueueLazyLoadingMapNomnatatimLock(lazyLoadingAllExiftoolVersionOfMediaFile, forceReloadUsingReverseGeocoder: false);
            StartThreads();
        }
        #endregion

        #region LazyLoading - Add - All Sources / No History / Metadata And Region Thumbnails - AddQueue - (Read order: Cache, Database, Source)
        public void AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(FileEntryAttribute fileEntryAttribute)
        {
            List<FileEntryAttribute> fileEntryAttributes = new List<FileEntryAttribute>();
            fileEntryAttributes.Add(fileEntryAttribute);
            AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttributes);
        }
        #endregion

        #region LazyLoading - Add - All Sources / Current File Written Date / Metadata And Region Thumbnails - AddQueue - (Read order: Cache, Database, Source)
        public void AddQueueLazyLoadning_AllSources_CurrentWrittenDate_UseCurrentFileDate_MetadataAndRegionThumbnailsLock(HashSet<FileEntry> fileEntries, FileEntryVersion fileEntryVersion)
        {
            List<FileEntryAttribute> fileEntryAttributes = new List<FileEntryAttribute>();
            foreach (FileEntry fileEntry in fileEntries)
            {
                #region Add to list, if file exist and use current written date 
                DateTime dateTime = FileHandler.GetLastWriteTime(fileEntry.FileFullPath, waitAndRetry: IsFileInTemporaryUnavailableLock(fileEntry.FileFullPath));
                if (dateTime > FileHandler.MinimumFileSystemDateTime) //If file not exist, written date becomes MinimumFileSystemDateTime
                {
                    if (!FileHandler.DidTouchedFileTimeoutDuringDownload(fileEntry.FileFullPath))
                    {
                        FileEntry fileEntryCurrent = new FileEntry(fileEntry.FileFullPath, dateTime);
                        if (fileEntry.LastWriteDateTime != dateTime) ImageListView_UpdateItemThumbnailUpdateAllInvoke(fileEntryCurrent);
                        fileEntryAttributes.Add(new FileEntryAttribute(fileEntryCurrent, fileEntryVersion));
                    }
                    else
                    {
                        //DEBUG
                    }
                } else
                {
                    //DEBUG - FIle not exist anymore
                }
                #endregion 
            }
            AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttributes);
        }
        #endregion

        #region LazyLoading - Add - All Sources / No History / Metadata And Region Thumbnails - AddQueue - (Read order: Cache, Database, Source)
        public void AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(List<FileEntryAttribute> fileEntryAttributes)
        {
            if (fileEntryAttributes == null) return;
            lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock)
            {
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes)
                {
                    AddQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.Queue));
                    if (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Contains(fileEntryAttribute))
                        commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Remove(fileEntryAttribute);
                    commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Insert(0, fileEntryAttribute);
                }
            }
        }
        #endregion

        #region LazyLoading - Thread - All Sources / Metadata And Region Thumbnails - Thread - **Populate** - (** Doesn't add Read from Source **)
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
                                
                                #region Check if need to read column
                                bool readColumn = false;
                                switch (fileEntryAttribute.FileEntryVersion)
                                {
                                    case FileEntryVersion.MetadataToSave:
                                    case FileEntryVersion.CompatibilityFixedAndAutoUpdated:
                                    case FileEntryVersion.CurrentVersionInDatabase:
                                    case FileEntryVersion.ExtractedNowUsingExiftool:
                                    case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:         //DataGridView Column
                                    case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:    //DataGridView Column
                                    case FileEntryVersion.ExtractedNowUsingExiftoolWithError:       //DataGridView Column

                                        readColumn = true;
                                        break;
                                    case FileEntryVersion.ExtractedNowUsingReadMediaFile:
                                    case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:
                                    case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:
                                    case FileEntryVersion.ExtractedNowUsingWebScraping:
                                        readColumn = true;
                                        break;
                                    case FileEntryVersion.Error:
                                        if ((showWhatColumns & ShowWhatColumns.ErrorColumns) > 0) readColumn = true;
                                        else DataGridView_SetColumnVisibleStatus_FileEntryAttributeInvoke(fileEntryAttribute, false);

                                        if (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails[0].FileEntry == fileEntryAttribute.FileEntry)
                                        {
                                            FileStatus fileStatus = FileHandler.GetFileStatus(fileEntryAttribute.FileFullPath, hasErrorOccured: true,
                                                errorMessage: "Exitfool failed", exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);
                                            
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
                                #endregion

                                bool isMetadataExiftoolFound = false;
                                bool isMetadataExiftoolErrorFound = false;

                                if (readColumn)
                                {
                                    FileStatus fileStatus = null;

                                    #region Exiftool with or without Error
                                    try
                                    {

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
                                                //RemoveQueueLazyLoadningSelectedFilesLock(fileEntryBrokerExiftool);
                                                if (!FileHandler.DidTouchedFileTimeoutDuringDownload(fileEntryAttribute.FileFullPath))
                                                    AddQueueReadFromSourceExiftoolLock(fileEntryAttribute); //Didn't exists in Database, need read from source
                                            }
                                            else
                                            {
                                                fileStatus = FileHandler.GetFileStatus(fileEntryBrokerExiftoolError.FileFullPath,
                                                    exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError,
                                                    errorMessage: "File has error recorded, you can retry read again", checkLockedStatus: true);
                                                ImageListView_UpdateItemFileStatusInvoke(fileEntryBrokerExiftoolError.FileFullPath, fileStatus);
                                                isMetadataExiftoolErrorFound = true;
                                            }
                                        }
                                        else
                                        {
                                            isMetadataExiftoolFound = true;
                                            //Check if Region Thumnbail missing, if yes, then create
                                            if (metadataExiftool.PersonalRegionIsThumbnailMissing())
                                                AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataExiftool);

                                            fileStatus = FileHandler.GetFileStatus(metadataExiftool.FileFullPath,
                                            exiftoolProcessStatus: ExiftoolProcessStatus.WaitAction);
                                            ImageListView_UpdateItemFileStatusInvoke(metadataExiftool.FileFullPath, fileStatus);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        KryptonMessageBox.Show("ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails crashed.\r\n" +
                                            "Read Exiftool data.\r\n" +
                                            "Exception message:" + ex.Message + "\r\n",
                                            "Lazy loading queue failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                        Logger.Error(ex, "ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails");
                                    }
                                    #endregion

                                    #region Microsoft Photos
                                    try
                                    {
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
                                    }
                                    catch (Exception ex)
                                    {
                                        KryptonMessageBox.Show("ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails crashed.\r\n" +
                                            "Read Microsoft Photos data.\r\n" +
                                            "Exception message:" + ex.Message + "\r\n",
                                            "Lazy loading queue failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                        Logger.Error(ex, "ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails");
                                    }
                                    #endregion

                                    #region WindowsLivePhotoGallery
                                    try
                                    {
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
                                    }
                                    catch (Exception ex)
                                    {
                                        KryptonMessageBox.Show("ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails crashed.\r\n" +
                                            "Read WindowsLiveGallery data.\r\n" +
                                            "Exception message:" + ex.Message + "\r\n",
                                            "Lazy loading queue failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                        Logger.Error(ex, "ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails");
                                    }
                                    #endregion

                                    #region WebScraper
                                    try
                                    {
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
                                    }
                                    catch (Exception ex)
                                    {
                                        KryptonMessageBox.Show("ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails crashed.\r\n" +
                                            "Read WebScaping data.\r\n" +
                                            "Exception message:" + ex.Message + "\r\n",
                                            "Lazy loading queue failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                        Logger.Error(ex, "ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails");
                                    }
                                    #endregion

                                    bool isCurrent = FileEntryVersionHandler.IsCurrenOrUpdatedVersion(fileEntryAttribute.FileEntryVersion);
                                    if (isCurrent && isMetadataExiftoolFound) //Don't care about historical or error columns
                                        ImageListView_UpdateItemExiftoolMetadataInvoke(fileEntryAttribute, fileStatus);

                                    if (isMetadataExiftoolFound || isMetadataExiftoolErrorFound) // No need update others before Exiftool has data || isMetadataExiftoolErrorFound || isMetadataOtherSourceFound) 
                                        DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute, MetadataBrokerType.Queue);
                                    
                                }
                                else
                                {
                                    RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntryAttribute.FileEntry, MetadataBrokerType.Queue));
                                    //LazyLoadingDataGridViewProgressUpdateStatus(DataGridView_GetCircleProgressCount(true, 0));
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
                            }
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails crashed.\r\n" +
                                "The 'commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails' was cleared.\r\n" + 
                                "Exception message:" + ex.Message + "\r\n",
                                "Lazy loading queue failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnailsLock) commonQueueLazyLoadingAllSourcesAllMetadataAndRegionThumbnails.Clear();  //Avoid loop, due to unknown error
                            Logger.Error(ex, "ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails");
                        }
                        finally
                        {
                            lock (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnailsLock) _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails = null;
                            Logger.Trace("ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails - ended");
                        }
                        #endregion
                    });

                    lock (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnailsLock) if (_ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails != null)
                    {
                        _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails.Priority = threadPriority;
                        _ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails.Start();                        
                    }
                    else Logger.Error("ThreadLazyLoadingAllSourcesMetadataAndRegionThumbnails was not able to start");
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

        #region Media Thumbnail - LazyLoading 

        #region Media Thumbnail - LazyLoading - Add Read Queue - (Read order: Cache, Database, Source)
        public void AddQueueLazyLoadningMediaThumbnailLock(List<FileEntryAttribute> fileEntryAttributes)
        {
            if (fileEntryAttributes == null) return;
            lock (commonQueueLazyLoadingMediaThumbnailLock)
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
            lock (commonQueueLazyLoadingMediaThumbnailLock)
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
                                    lock (commonQueueLazyLoadingMediaThumbnailLock) fileEntryAttribute = commonQueueLazyLoadingMediaThumbnail[0];

                                    if (!databaseAndCacheThumbnailPoster.DoesThumbnailExistInCache(fileEntryAttribute))
                                    {
                                        bool dontReadFileFromCloud = Properties.Settings.Default.AvoidOfflineMediaFiles;
                                        FileStatus fileStatus = FileHandler.GetFileStatus(fileEntryAttribute.FileFullPath);
                                        Image thumbnail = GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(fileEntryAttribute, dontReadFileFromCloud, fileStatus);                                       
                                    }
                                }
                                lock (commonQueueLazyLoadingMediaThumbnailLock) commonQueueLazyLoadingMediaThumbnail.RemoveAt(0);

                                if (GlobalData.IsApplicationClosing || GlobalData.IsStopAndEmptyThumbnailQueueRequest)
                                    lock (commonQueueLazyLoadingMediaThumbnailLock) commonQueueLazyLoadingMediaThumbnail.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("LazyLoadningThumbnail crashed." + 
                                "\r\nException message:" + ex.Message + "\r\n",
                                "LazyLoadningThumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (commonQueueLazyLoadingMediaThumbnailLock) commonQueueLazyLoadingMediaThumbnail.Clear(); //Avoid loop, due to unknown error
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
        public void AddQueueReadFromSourceExiftoolLock(FileEntry fileEntry, bool alsoAddToTheEnd = false)
        {
            if (FileHandler.DidTouchedFileTimeoutDuringDownload(fileEntry.FileFullPath))
            {
                //DEBUG
                return; 
            }
            lock (commonQueueReadMetadataFromSourceExiftoolLock)
            {
                AddQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));

                if (alsoAddToTheEnd)
                {
                    
                    if (commonQueueReadMetadataFromSourceExiftool.IndexOf(fileEntry) == commonQueueReadMetadataFromSourceExiftool.LastIndexOf(fileEntry))
                        commonQueueReadMetadataFromSourceExiftool.Add(fileEntry);
                }
                else
                {
                    bool existInQueue = IsFolderInQueueReadAndSaveMetadataFromSourceExiftoolLock(fileEntry.FileFullPath);
                    if (!existInQueue) commonQueueReadMetadataFromSourceExiftool.Add(fileEntry);
                }
            }
            RemoveError(fileEntry.FileFullPath);
        }
        #endregion

        #region Exiftool - ReadFromSource Metadata Exiftool - Thread - **Populate** - (Read order: Cache, Database, Source)
        public void ThreadReadFromSourceMetadataExiftool()
        {
            try
            {
                if (GlobalData.IsStopAndEmptyExiftoolReadQueueRequest || _ThreadReadMetadataFromSourceExiftool != null || CommonQueueReadMetadataFromSourceExiftoolCountDirty() <= 0) return;

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

                            lock (exiftool_MediaFilesNotInDatabaseLock) exiftool_MediaFilesNotInDatabase.Clear(); //Start empty, in case of crash

                            int count = CommonQueueReadMetadataFromSourceExiftoolCountLock();
                            while (count > 0 && !GlobalData.IsStopAndEmptyExiftoolReadQueueRequest && CommonQueueReadMetadataFromSourceExiftoolCountLock() > 0 && !GlobalData.IsApplicationClosing) //In case some more added to the queue
                            {
                                count--;
                                if (CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountLock() > 0) break; //Write first, read later on...

                                int rangeToRemove; //Remember how many in queue now
                                List<FileEntry> mediaFilesNotInDatabase_NeedCheckInCloud = new List<FileEntry>();
                                List<FileEntry> mediaFilesReadFromDatabase_NeedUpdated_DataGridView_ImageList = new List<FileEntry>();

                                //Input: commonQueueReadMetadataFromSourceExiftool (Main queue - Read this from Exiftool)
                                //Output: List<FileEntry> mediaFilesWasNotInCache                                       (Was not in database cache)
                                //Output: List<FileEntry> mediaFilesNotInDatabase_NeedCheckInCloud                      (Was not in database either)
                                //Output: List<FileEntry> mediaFilesReadFromDatabase_NeedUpdated_DataGridView_ImageList (Was alread in database)
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

                                //Input: List<FileEntry> mediaFilesNotInDatabase_NeedCheckInCloud
                                //Output: List<FileEntry> exiftool_MediaFilesNotInDatabase
                                //When file exist and ready to be processed:
                                //  Output: no changes
                                //When wait cloud file to be downloaded
                                //  Output: AddQueueReadFromSourceExiftoolLock(fileEntry); 
                                //When wait cloud file to be downloaded timeouted
                                //  Output: RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.Queue));
                                //  Output: RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                                #region Check if need avoid files in cloud, if yes, don't read files in cloud
                                if (Properties.Settings.Default.AvoidReadExifFromCloud)
                                {

                                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase_NeedCheckInCloud)
                                    {
                                        if (GlobalData.IsApplicationClosing) break;
                                        FileStatus fileStatus = FileHandler.GetFileStatus(fileEntry.FileFullPath);

                                        #region File not Exist, forget file 
                                        if (!fileStatus.FileExists)
                                        {
                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError;

                                            RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.Queue));
                                            RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                                            ImageListViewRemoveItemInvoke(fileEntry.FileFullPath);
                                            
                                            FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist);
                                            DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute, MetadataBrokerType.ExifTool);
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

                                            RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.Queue));
                                            RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                                            ImageListViewRemoveItemInvoke(fileEntry.FileFullPath);

                                            //When in cloud, and can't read, also need to populate dataGridView but will become with empty rows in column
                                            FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.NotAvailable);
                                            DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute, MetadataBrokerType.ExifTool);
                                        }
                                        #endregion
                                        
                                        ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                                    }
                                }
                                else
                                {
                                    foreach (FileEntry fileEntry in mediaFilesNotInDatabase_NeedCheckInCloud)
                                    {
                                        if (GlobalData.IsApplicationClosing) break;
                                        FileStatus fileStatus = FileHandler.GetFileStatus(fileEntry.FileFullPath);

                                        #region File not Exist, forget file
                                        if (!fileStatus.FileExists) 
                                        {
                                            fileStatus.FileExists = false;
                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError;

                                            RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.Queue));
                                            RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));

                                            FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist);
                                            DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute, MetadataBrokerType.ExifTool);
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
                                            #region Touched file failed download
                                            if (FileHandler.DidTouchedFileTimeoutDuringDownload(fileEntry.FileFullPath))
                                            {
                                                fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError; //TimeOut Error
                                                fileStatus.FileErrorMessage = "Failed download";

                                                RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.Queue));
                                                RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));

                                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.ExtractedNowUsingExiftoolTimeout);
                                                DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute, MetadataBrokerType.ExifTool);                                                
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

                                if (!GlobalData.IsApplicationClosing)
                                {
                                    
                                    while (!GlobalData.IsStopAndEmptyExiftoolReadQueueRequest && ExiftoolSave_MediaFilesNotInDatabaseCountLock() > 0 && !GlobalData.IsApplicationClosing)
                                    {
                                        //Input: List<FileEntry> exiftool_MediaFilesNotInDatabase ** This will get cleared after batch process done ** 
                                        //Output: List<String> useExiftoolOnThisSubsetOfFiles
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
                                            if (range > 100) range = 100;
                                            for (int index = 0; index < range; index++)
                                                useExiftoolOnThisSubsetOfFiles.Add(exiftool_MediaFilesNotInDatabase[index].FileFullPath);
                                        }
                                        #endregion

                                        //Input: List<filename> useExiftoolOnThisSubsetOfFiles
                                        //Output: List<Metadata> metadataReadFromExiftool
                                        //Output: Dictionary<string filename, string errormessage> exiftoolErrorMessageOnFiles
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

                                        #region CHECK IF READ OR NOT - Check if all files are read by Exiftool - Check against - metadataReadFromExiftool - if not read, no need to verify
                                        try
                                        {
                                            //Input: List<filename> useExiftoolOnThisSubsetOfFiles
                                            //Input: List<Metadata> metadataReadFromExiftool - List of files readby Exiftool
                                            //Input: Dictionary<filename, errormessage> exiftoolErrorMessageOnFiles[fullFilePath] - contains error messages from Exiftool

                                            //Output: List<Metadata> exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify (Remove from the verified queue)
                                            
                                            //Output: commonQueueReadMetadataFromSourceExiftool (Added back last in queue - when data was not read or failed to read)
                                            foreach (string fullFilePath in useExiftoolOnThisSubsetOfFiles)
                                            {
                                                #region Check if Files added for Read, but failed to be Read
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
                                                        hasErrorOccured: true, errorMessage: exiftoolErrorMessageForFile,
                                                        exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);

                                                    exiftoolErrorMessageForFile =
                                                        "Issue: EXIFTOOL.EXE failed to read metadata\r\n" +
                                                        "File name: " + fullFilePath + "\r\n" +
                                                        "File staus: " + fileStatus.ToString() + "\r\n" +
                                                        "Error Message: " + exiftoolErrorMessageForFile;
                                                    #endregion

                                                    //Output: FileEntry fileEntryMetadataNotRead - Filename read, add LastWrittenDateTime
                                                    #region Find FileEntry Information
                                                    FileEntry fileEntryMetadataNotRead;
                                                    lock (exiftool_MediaFilesNotInDatabaseLock)
                                                    {
                                                        int indexFileEntry = FileEntry.FindIndex(exiftool_MediaFilesNotInDatabase, fullFilePath, range);

                                                        if (indexFileEntry != -1)
                                                            fileEntryMetadataNotRead = new FileEntry(exiftool_MediaFilesNotInDatabase[indexFileEntry]);
                                                        else
                                                        {
                                                            fileEntryMetadataNotRead = new FileEntry(fullFilePath, FileHandler.GetLastWriteTime(fullFilePath, waitAndRetry: true));
                                                        }
                                                    }
                                                    #endregion

                                                    //Output: Metadata metadataErrorDummy
                                                    #region Create Metadata Error Dummy - MetadataBrokerType.ExifTool | ExifToolWriteError
                                                    Metadata metadataErrorDummy = new Metadata(MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError);
                                                    metadataErrorDummy.FileDateModified = fileEntryMetadataNotRead.LastWriteDateTime;
                                                    metadataErrorDummy.FileName = fileEntryMetadataNotRead.FileName;
                                                    metadataErrorDummy.FileDirectory = fileEntryMetadataNotRead.Directory;
                                                    metadataErrorDummy.FileMimeType = FormDatabaseCleaner.CorruptFile; //Also used
                                                    metadataErrorDummy.PersonalTitle = "Exiftool failed";
                                                    metadataErrorDummy.PersonalComments = FileHandler.ConvertFileStatusToText(fileStatus) + " " + lastKownExiftoolGenericErrorMessage;
                                                    metadataErrorDummy.FileDateCreated = fileEntryMetadataNotRead.LastWriteDateTime;
                                                    metadataErrorDummy.FileDateAccessed = DateTime.Now;
                                                    metadataErrorDummy.FileSize = 0;
                                                    try
                                                    {
                                                        if (File.Exists(fileEntryMetadataNotRead.FileFullPath))
                                                        {
                                                            FileInfo fileInfo = new FileInfo(fileEntryMetadataNotRead.FileFullPath);
                                                            metadataErrorDummy.FileDateCreated = fileInfo.CreationTime;
                                                            metadataErrorDummy.FileDateAccessed = fileInfo.LastAccessTime;
                                                            metadataErrorDummy.FileSize = fileInfo.Length;
                                                        }
                                                        else
                                                        {
                                                            metadataErrorDummy.PersonalComments = "File doesn't exist - Exiftool failed";
                                                        }
                                                    }
                                                    catch { }
                                                    databaseAndCacheMetadataExiftool.Write(metadataErrorDummy);
                                                    #endregion

                                                    //AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock (Error fileEntry)
                                                    #region Save Error, Update ImageListView Exiftool status
                                                    ImageListView_UpdateItemFileStatusInvoke(metadataErrorDummy.FileFullPath, fileStatus);

                                                    FileEntryAttribute fileEntryAttributeError = new FileEntryAttribute(
                                                        metadataErrorDummy.FileFullPath,
                                                        (DateTime)metadataErrorDummy.FileDateModified,
                                                        FileEntryVersion.Error);

                                                    AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttributeError);

                                                    AddError(
                                                        Path.GetDirectoryName(fullFilePath), Path.GetFileName(fullFilePath), DateTime.Now,
                                                        AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                                        AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                                        exiftoolErrorMessageForFile, false);
                                                    #endregion

                                                    //Output: exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify (Remove from the verified queue)
                                                    #region Remove from Verdify queue, no need verify 
                                                    lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock)
                                                    {
                                                        int verifyPosition = Metadata.FindFullFilenameInList(exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify, fullFilePath);
                                                        if (verifyPosition != -1) exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify.RemoveAt(verifyPosition);
                                                    }
                                                    #endregion
                                                }
                                                #endregion

                                                //Input: fullFilePath
                                                //Output: commonQueueReadMetadataFromSourceExiftool (Added back last in queue)
                                                #region Check if still missing data in Database, if yes, read again
                                                FileEntry fileEntry = new FileEntry(fullFilePath, FileHandler.GetLastWriteTime(fullFilePath, waitAndRetry: IsFileInTemporaryUnavailableLock(fullFilePath)));
                                                Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                                                if (metadata == null)
                                                {
                                                    metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError));
                                                    if (metadata == null) AddQueueReadFromSourceExiftoolLock(fileEntry, alsoAddToTheEnd: true);
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
                                                    //Input:  List<Metadata> metadataReadFromExiftool -> one by one - metadataRead 
                                                    //Input:  List<Metadata> *exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify (list of metadataUpdatedByUserCopy)*
                                                    //Output: List<Metadata> *exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify (will be removed after verified)*
                                                    //Outout: Metadata metadataUpdatedByUserCopy (found item from exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify[])
                                                    //If has error
                                                    //Output
                                                    //If Media file changed since read
                                                    //Output: commonQueueReadMetadataFromSourceExiftool - Added to queue
                                                    //If read, no errors
                                                    //Output: commonQueueSaveToDatabaseRegionAndThumbnail - Added to queue
                                                    #region Verify - Read back not Equal to saved - Then Add Error, Populate DataGridView and ImageListView
                                                    switch (ExiftoolWriter.HasWriteMetadataErrors(
                                                        metadataRead,
                                                        ref exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify,
                                                        out Metadata metadataUpdatedByUserCopy,
                                                        out string writeErrorDesciption))
                                                    {
                                                        case MetadataErrors.HasError:
                                                            #region Add Verified Error (Not read error)
                                                            AddError(
                                                                metadataUpdatedByUserCopy.FileEntryBroker.Directory, metadataUpdatedByUserCopy.FileEntryBroker.FileName, metadataUpdatedByUserCopy.FileEntryBroker.LastWriteDateTime,
                                                                AddErrorExiftooRegion, AddErrorExiftooCommandVerify,
                                                                AddErrorExiftooParameterVerify, AddErrorExiftooParameterVerify,
                                                                "Issue: Verified Metadata is not equal with written.\r\n" +
                                                                "File Name: " + metadataUpdatedByUserCopy.FileFullPath + "\r\n" +
                                                                "Error Message: " + writeErrorDesciption + "\r\n" +
                                                                "Reason: 3rd party applicatoin are doing correction on file, e.g OneDrive -> Microsoft Photos -> Synced back with changes\r\n" +
                                                                "Reason: There's tags from Manifactors or other applications store metadata same type of metadata.\r\n" +
                                                                "Solution: 1) Config Write Metadata to overwite old infomtation with new. 2) Config the tag with lower priority or ignor it.");
                                                            #endregion

                                                            #region Save Metadata with Broker Exiftool | WriteError 
                                                            Metadata metadataError = new Metadata(metadataUpdatedByUserCopy);
                                                            //metadataError.FileDateModified = DateTime.Now;
                                                            metadataError.Broker |= MetadataBrokerType.ExifToolWriteError;
                                                            databaseAndCacheMetadataExiftool.Write(metadataError);
                                                            #endregion

                                                            DataGridViewSetMetadataOnAllDataGridView(metadataRead);
                                                            DataGridViewSetDirtyFlagAfterSave(metadataRead, true, FileEntryVersion.MetadataToSave);

                                                            #region Populate - Current ImageListView Item - With erros info
                                                            FileStatus fileStatusVerify = FileHandler.GetFileStatus(
                                                                metadataError.FileFullPath, checkLockedStatus: true, checkLockStatusTimeout: FileHandler.GetFileLockedStatusTimeout,
                                                                hasErrorOccured: true, errorMessage: "Verify data failed",
                                                                exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);
                                                            
                                                            FileEntryAttribute fileEntryAttributeCurrent = new FileEntryAttribute(
                                                                metadataError.FileFullPath, (DateTime)metadataError.FileDateModified, FileEntryVersion.Error);
                                                            //ImageListView_UpdateItemFileStatusInvoke(metadataError.FileFullPath, fileStatusVerify);
                                                            ImageListView_UpdateItemExiftoolMetadataInvoke(fileEntryAttributeCurrent, fileStatusVerify);
                                                            #endregion

                                                            #region Populate - Current DataGridView Column - Add new Error Coloumn
                                                            FileEntryAttribute fileEntryAttributeError = new FileEntryAttribute(
                                                                metadataError.FileFullPath, (DateTime)metadataError.FileDateModified, FileEntryVersion.Error);
                                                            AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttributeError);
                                                            #endregion

                                                            #region Data was read, (even with errors), need to updated datagrid
                                                            AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataRead);
                                                            #endregion

                                                            break;
                                                        case MetadataErrors.WasUpdatedAfterRead:
                                                            //Output: List<Metadata> *exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify (was not found)*
                                                            //Was added back to queue - Check if still missing data in Database, if yes, read again
                                                            break;
                                                        case MetadataErrors.NoErrors:
                                                            #region Data was read, (even with errors), need to updated datagrid
                                                            AddQueueSaveToDatabaseRegionAndThumbnailLock(metadataRead);
                                                            #endregion

                                                            #region Populate - Current ImageListView Item - If no erros
                                                            FileStatus fileStatus = FileHandler.GetFileStatus(metadataRead.FileFullPath);
                                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitAction;
                                                            
                                                            FileEntryAttribute fileEntryAttributeExtractedNowUsingExiftool = new FileEntryAttribute(metadataRead.FileFullPath,
                                                                (DateTime)metadataRead.FileDateModified,
                                                                FileEntryVersion.ExtractedNowUsingExiftool);
                                                            ImageListView_UpdateItemExiftoolMetadataInvoke(fileEntryAttributeExtractedNowUsingExiftool, fileStatus);
                                                            #endregion

                                                            #region Populate - Current DataGridView Column - If no errors
                                                            AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttributeExtractedNowUsingExiftool);
                                                            #endregion 
                                                            break;
                                                        default:
                                                            throw new NotImplementedException();
                                                    }
                                                    #endregion

                                                    
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Error(ex, "ThreadCollectMetadataExiftool - Verify readback after saved failed.");
                                        }
                                        #endregion

                                        //Output: exiftool_MediaFilesNotInDatabase - Clear
                                        #region Clear the mediaFilesNotInDatabase
                                        try
                                        {
                                            lock (exiftool_MediaFilesNotInDatabaseLock)
                                            {
                                                foreach (FileEntry fileEntry in exiftool_MediaFilesNotInDatabase) RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                                                exiftool_MediaFilesNotInDatabase.RemoveRange(0, range); //Remove subset from queue before update status bar
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Error(ex, "ThreadCollectMetadataExiftool - mediaFilesNotInDatabase.RemoveRange(0, range) failed.");
                                        }
                                        #endregion

                                        //Input: List<Metadata> exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify
                                        //Output: List<FileEntry> commonQueueReadMetadataFromSourceExiftool 
                                        #region If still in verified queue, add it back to read list
                                        lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock)
                                        {
                                            foreach (Metadata metadataToBeVerified in exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify)
                                            {
                                                Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(metadataToBeVerified.FileEntry, MetadataBrokerType.ExifTool));
                                                if (metadata != null)
                                                {
                                                    #region Populate - Current ImageListView Item - If no erros
                                                    FileStatus fileStatus = FileHandler.GetFileStatus(metadataToBeVerified.FileFullPath);
                                                    fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitAction;

                                                    FileEntryAttribute fileEntryAttributeExtractedNowUsingExiftool = new FileEntryAttribute(metadataToBeVerified.FileFullPath,
                                                        (DateTime)metadataToBeVerified.FileDateModified,
                                                        FileEntryVersion.ExtractedNowUsingExiftool);
                                                    ImageListView_UpdateItemExiftoolMetadataInvoke(fileEntryAttributeExtractedNowUsingExiftool, fileStatus);
                                                    #endregion
                                                }
                                                else
                                                {
                                                    AddQueueReadFromSourceExiftoolLock(metadataToBeVerified.FileEntry, alsoAddToTheEnd: false);
                                                }
                                            }
                                        }
                                        #endregion 

                                    } //while (!GlobalData.IsStopAndEmptyExiftoolReadQueueRequest && ExiftoolSave_MediaFilesNotInDatabaseCountLock() > 0 && !GlobalData.IsApplicationClosing)

                                } //if (!GlobalData.IsApplicationClosing)

                                Thread.Sleep(10);
                            }

                            if (GlobalData.IsApplicationClosing || GlobalData.IsStopAndEmptyExiftoolReadQueueRequest)
                                lock (commonQueueReadMetadataFromSourceExiftoolLock) commonQueueReadMetadataFromSourceExiftool.Clear();
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
        
        #region Exiftool - VerifyMetadata - Add Queue
        public void AddQueueVerifyMetadataLock(Metadata metadataToVerifyAfterSavedByExiftool)
        {
            lock (exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerifyLock) exiftoolSave_QueueMetadataWrittenByExiftoolReadyToVerify.Add(metadataToVerifyAfterSavedByExiftool);
        }
        #endregion
        
        #endregion

        #region Exiftool - SaveMetadata

        #region Exiftool - SaveMetadata UpdatedByUser - Add Queue
        public void AddQueueSaveUsingExiftoolMetadataUpdatedByUserLock(Metadata metadataToSave, Metadata metadataOriginal)
        {
            lock (exiftoolSave_QueueSaveUsingExiftool_MetadataUpdatedByUserLock) exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.Add(metadataToSave);
            lock (exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUserLock) exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate.Add(metadataOriginal);
            DeleteError(metadataToSave.FileFullPath);
            DeleteError(metadataOriginal.FileFullPath);
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

                                string writeMetadataTagsConfiguration = Properties.Settings.Default.WriteMetadataTags;
                                string writeMetadataKeywordAddConfiguration = Properties.Settings.Default.WriteMetadataKeywordAdd;

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

                                List<string> networkNames = new List<string>();
                                string GPStag = Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix;
                                try
                                {
                                    networkNames = new List<string>(oneDriveNetworkNames);
                                }
                                catch { }
                                #endregion

                                #region Clear data
                                int writeCount = Math.Min(Properties.Settings.Default.ExiftoolMaximumWriteBach, CommonQueueSaveUsingExiftoolMetadataUpdatedByUserCountLock());
                                //List<Metadata> exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser -- Globale just to show status 
                                List<Metadata> exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate;

                                lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                                {
                                    exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser = new List<Metadata>();    //This new values for saving (changes done by user)
                                    exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate = new List<Metadata>(); //Before updated by user, need this to check if any updates
                                }
                                #endregion

                                //Queue in:     List<Metadata>exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser
                                //              List<Metadata>exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate
                                //          
                                //Result:       Dictonary<string filename, long size>fileSaveSizeWatcher (Just to show written size on media file)
                                //              List<Metadata>exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser
                                //              List<Metadata>exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate
                                #region Create a subset queue for writing
                                try
                                {
                                    for (int i = 0; i < writeCount; i++)
                                    {
                                        FileEntry fileEntryOriginal = exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate[0].FileEntry;

                                        #region Check file status
                                        FileStatus fileStatus = FileHandler.GetFileStatus(fileEntryOriginal.FileFullPath);

                                        #region Check file status - File not Exist, forget file
                                        if (!fileStatus.FileExists)
                                        {
                                            fileStatus.FileExists = false;
                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError;

                                            FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntryOriginal, FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist);
                                            RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntryOriginal, MetadataBrokerType.Queue));
                                            RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntryOriginal, MetadataBrokerType.ExifTool));
                                            ImageListViewRemoveItemInvoke(fileEntryOriginal.FileFullPath);
                                            DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute, MetadataBrokerType.ExifTool);
                                        }
                                        #endregion
                                        #region Check file status - File exist and not in clud, proceed
                                        else if (fileStatus.FileExists && !fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.InExiftoolReadQueue;
                                        }
                                        #endregion
                                        #region Check file status - File exist and offline, touch file and put back last in queue until Timeout
                                        else if (fileStatus.FileExists && fileStatus.IsInCloudOrVirtualOrOffline)
                                        {
                                            #region Touched file failed to Download
                                            if (FileHandler.DidTouchedFileTimeoutDuringDownload(fileEntryOriginal.FileFullPath))
                                            {
                                                fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError; //TimeOut Error
                                                fileStatus.FileErrorMessage = "Failed download";

                                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntryOriginal, FileEntryVersion.ExtractedNowUsingExiftoolTimeout);
                                                RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntryOriginal, MetadataBrokerType.Queue));
                                                RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntryOriginal, MetadataBrokerType.ExifTool));
                                                DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute, MetadataBrokerType.ExifTool);
                                            }
                                            #endregion
                                            else
                                            #region Touch file and put back in queue
                                            {
                                                FileHandler.TouchOfflineFileToGetFileOnline(fileEntryOriginal.FileFullPath);
                                                fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitOfflineBecomeLocal;
                                            }
                                            #endregion
                                        }
                                        #endregion
                                        #region Check file status - Updated status
                                        ImageListView_UpdateItemFileStatusInvoke(fileEntryOriginal.FileFullPath, fileStatus);
                                        #endregion

                                        #endregion

                                        //Remeber 
                                        Metadata metadataToWrite;
                                        Metadata metadataOrginal;

                                        lock (exiftoolSave_QueueSaveUsingExiftool_MetadataUpdatedByUserLock)
                                        {
                                            if (!fileStatus.FileExists || fileStatus.ExiftoolProcessStatus == ExiftoolProcessStatus.FileInaccessibleOrError)
                                            {
                                                #region File doesn't exist anymore, remove from queue, and remove from circle status queue
                                                exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.RemoveAt(0);
                                                exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate.RemoveAt(0);
                                                
                                                RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntryOriginal, MetadataBrokerType.Queue));
                                                RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntryOriginal, MetadataBrokerType.ExifTool));

                                                if (!fileStatus.FileExists) ImageListViewRemoveItemInvoke(fileEntryOriginal.FileFullPath);
                                                #endregion
                                            }
                                            else if (fileStatus.IsInCloudOrVirtualOrOffline)
                                            {
                                                #region When in cloud, move back in queue
                                                lock (exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUserLock)
                                                {
                                                    metadataToWrite = exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser[0];
                                                    metadataOrginal = exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate[0];
                                                
                                                    exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.RemoveAt(0);
                                                    exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate.RemoveAt(0);
                                                
                                                    exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.Add(metadataToWrite);
                                                    exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate.Add(metadataOrginal);
                                                }
                                                #endregion
                                            }
                                            else if (!fileStatus.IsInCloudOrVirtualOrOffline)
                                            {
                                                #region File ready to be written to, add to write queue
                                                
                                                lock (exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUserLock)
                                                {
                                                    metadataToWrite = exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser[0];
                                                    metadataOrginal = exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate[0];
                                                }

                                                lock (commonQueueReadMetadataFromSourceExiftoolLock)
                                                {
                                                    if (!GlobalData.IsApplicationClosing)
                                                    {
                                                        if (metadataOrginal != metadataToWrite) AddWatcherShowExiftoolSaveProcessQueue(metadataToWrite.FileEntryBroker.FileFullPath);

                                                        lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                                                        {
                                                            exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser.Add(metadataToWrite);
                                                            exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate.Add(metadataOrginal);
                                                        }
                                                    }
                                                }

                                                //Remove
                                                lock (exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUserLock)
                                                {
                                                    exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.RemoveAt(0);
                                                    exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate.RemoveAt(0);
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error(ex, "ThreadSaveMetadata - Create a subset queue for writing");
                                }
                                #endregion

                                //Queue in:     List<Metadata>exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser
                                //              List<Metadata>exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate
                                //    
                                //Result:       List<FileEntry>mediaFilesUpdatedByExiftool,
                                //              (When old and New metadata is diffrent, all when exiftool failed)
                                #region Save Metadatas using Exiftool  
                                List<FileEntry> mediaFilesUpdatedByExiftool = new List<FileEntry>();
                                string exiftoolErrorMessage = "";

                                if (!GlobalData.IsApplicationClosing)
                                {
                                    try
                                    {
                                        lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                                        {
                                            foreach (Metadata metadata in exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser)
                                            {
                                                Metadata metadataToSave = new Metadata(metadata);
                                                metadataToSave.Broker = MetadataBrokerType.UserSavedData;
                                                metadataToSave.FileDateModified = DateTime.MinValue;
                                                databaseAndCacheMetadataExiftool.DeleteFileEntry(metadataToSave.FileEntryBroker);
                                                databaseAndCacheMetadataExiftool.Write(metadataToSave);
                                            }
                                            if (exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate.Count > 0)
                                            {
                                                UpdateStatusAction("Batch update a subset of " + exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser.Count + " media files...");
                                                ExiftoolWriter.WriteMetadata(
                                                    exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser, exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate,
                                                    allowedFileNameDateTimeFormats, networkNames, GPStag,
                                                    writeMetadataTagsConfiguration, writeMetadataKeywordAddConfiguration, out mediaFilesUpdatedByExiftool,
                                                    showCliWindow, processPriorityClass);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Something did go wrong, need to tell users, what was saved and not, in general nothing was saved
                                        lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                                        {
                                            foreach (Metadata metadataCopy in exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser)
                                            {
                                                if (!FileEntry.Contains(mediaFilesUpdatedByExiftool, metadataCopy.FileEntry)) mediaFilesUpdatedByExiftool.Add(metadataCopy.FileEntry);
                                            }
                                        }

                                        exiftoolErrorMessage = ex.Message;
                                        Logger.Error(ex, "EXIFTOOL.EXE error...");
                                    }
                                }
                                #endregion

                                //Queue in:     List<Metadata>exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser
                                //              List<Metadata>exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate
                                //    
                                //Return:       List<FileEntry>writeXtraAtom_FilesUpdated
                                //              Dictionary<string fullFileName, string errorMessage>writeXtraAtom_ErrorMessageForFile
                                #region Write Xtra Atom properites
                                Dictionary<string, string> writeXtraAtom_ErrorMessageForFile = new Dictionary<string, string>();
                                List<FileEntry> writeXtraAtom_FilesUpdated = new List<FileEntry>();
                                try
                                {
                                    if (!GlobalData.IsApplicationClosing)
                                    {
                                        
                                        if (writeXtraAtomOnMediaFile && exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser.Count > 0)
                                        {
                                            UpdateStatusAction("Write Xtra Atom to " + exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser.Count + " media files...");
                                            writeXtraAtom_FilesUpdated = ExiftoolWriter.WriteXtraAtom(
                                                exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser, exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate, 
                                                allowedFileNameDateTimeFormats, networkNames, GPStag,
                                                writeXtraAtomAlbumVariable, writeXtraAtomAlbumVideo,
                                                writeXtraAtomCategoriesVariable, writeXtraAtomCategoriesVideo,
                                                writeXtraAtomCommentVariable, writeXtraAtomCommentPicture, writeXtraAtomCommentVideo,
                                                writeXtraAtomKeywordsVariable, writeXtraAtomKeywordsPicture, writeXtraAtomKeywordsVideo,
                                                writeXtraAtomRatingPicture, writeXtraAtomRatingVideo,
                                                writeXtraAtomSubjectVariable, writeXtraAtomSubjectPicture, wtraAtomSubjectVideo,
                                                writeXtraAtomSubtitleVariable, writeXtraAtomSubtitleVideo,
                                                writeXtraAtomArtistVariable, writeXtraAtomArtistVideo,
                                                out writeXtraAtom_ErrorMessageForFile);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.Error(ex, "ThreadSaveMetadata - Write Xtra Atom properites");
                                }
                                #endregion

                                //Queue in:     List<Metadata>exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser
                                #region File Create date and Time attribute
                                try
                                {
                                    if (!GlobalData.IsApplicationClosing)
                                    {
                                        if (writeCreatedDateAndTimeAttribute)
                                        {
                                            lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                                            {
                                                foreach (Metadata metadata in exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser)
                                                {
                                                    if (metadata.TryParseDateTakenToUtc(out DateTime? dateTakenWithOffset))
                                                    {
                                                        if (metadata?.FileDateCreated != null &&
                                                            metadata?.MediaDateTaken != null &&
                                                            metadata?.MediaDateTaken >=  FileHandler.MinimumFileSystemDateTime &&
                                                            metadata?.MediaDateTaken < DateTime.Now &&
                                                            Math.Abs(((DateTime)dateTakenWithOffset.Value.ToUniversalTime() - (DateTime)metadata?.FileDateCreated.Value.ToUniversalTime()).TotalSeconds) > writeCreatedDateAndTimeAttributeTimeIntervalAccepted) //No need to change
                                                        {
                                                            #region SetCreationTime and retry if locked
                                                            int retrySetCreationTime = 3;
                                                            do
                                                            {
                                                                try
                                                                {
                                                                    File.SetCreationTime(metadata.FileFullPath, (DateTime)dateTakenWithOffset);
                                                                    retrySetCreationTime = 0;
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Logger.Error(ex, "File.SetCreationTime failed... " + metadata.FileFullPath);
                                                                    FileStatus fileStatus = FileHandler.GetFileStatus(metadata.FileFullPath, checkLockedStatus: true);
                                                                    if (fileStatus.IsFileLockedReadAndWrite)
                                                                    {
                                                                        Thread.Sleep(1000);
                                                                        retrySetCreationTime--;
                                                                    }
                                                                    else retrySetCreationTime = 0;
                                                                }
                                                            } while (retrySetCreationTime > 0);
                                                            #endregion
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Logger.Warn("ThreadSaveMetadata - Was not able to convert time, missing UTC and/or MediaTaken time on file: " + metadata.FileFullPath);
                                                    }
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

                                //Queue in:     List<FileEntry>mediaFilesUpdatedByExiftool
                                //Remove from:  List<Metadata>exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser
                                //              List<Metadata>exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate
                                #region Check if all files was updated, if updated, add to verify queue
                                if (!GlobalData.IsApplicationClosing)
                                {
                                    foreach (FileEntry fileEntrySupposedToBeUpdated in mediaFilesUpdatedByExiftool)
                                    {
                                        try
                                        {
                                            string currentFilenameAfterUpdate = fileEntrySupposedToBeUpdated.FileFullPath;
                                            int indexExiftoolFailedOn = Metadata.FindFileEntryInList(exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser, fileEntrySupposedToBeUpdated);

                                            Metadata metadataSupposedToBeSaved = exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate[indexExiftoolFailedOn];

                                            //Output: currentFilenameAfterUpdate
                                            #region If not written to new Filename, use old filename.
                                            lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                                            {
                                                if (
                                                    fileEntrySupposedToBeUpdated.FileFullPath != exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate[indexExiftoolFailedOn].FileFullPath &&
                                                    !File.Exists(fileEntrySupposedToBeUpdated.FileFullPath) && //New rename to name doesn't exit
                                                    File.Exists(metadataSupposedToBeSaved.FileFullPath) //But old filename exit
                                                    )
                                                {
                                                    string oldFullFilename = fileEntrySupposedToBeUpdated.FileFullPath;
                                                    string newFullFilename = metadataSupposedToBeSaved.FileFullPath;

                                                    DataGridView_Rename_Invoke(oldFullFilename, newFullFilename);
                                                    ImageListView_Rename_Invoke(imageListView1, oldFullFilename, newFullFilename);

                                                    Database_Rename(
                                                        Path.GetDirectoryName(oldFullFilename), Path.GetFileName(oldFullFilename),
                                                        Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));

                                                    currentFilenameAfterUpdate = newFullFilename;
                                                }
                                                else
                                                {
                                                    currentFilenameAfterUpdate = fileEntrySupposedToBeUpdated.FileFullPath;
                                                }
                                            }
                                            #endregion

                                            //Output: writeXtraAtom_ErrorMessageForFile[fileEntrySupposedToBeUpdated.FileFullPath] on currentFilenameAfterUpdate
                                            #region AddError - When Write Xtra Atoms failed?
                                            if (writeXtraAtom_ErrorMessageForFile.ContainsKey(fileEntrySupposedToBeUpdated.FileFullPath))
                                            {
                                                FileStatus fileStatus = FileHandler.GetFileStatus(
                                                    currentFilenameAfterUpdate, checkLockedStatus: true,
                                                    hasErrorOccured: true, errorMessage: writeXtraAtom_ErrorMessageForFile[fileEntrySupposedToBeUpdated.FileFullPath],
                                                    exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);
                                                ImageListView_UpdateItemFileStatusInvoke(currentFilenameAfterUpdate, fileStatus);

                                                AddError(
                                                    fileEntrySupposedToBeUpdated.Directory, currentFilenameAfterUpdate, fileEntrySupposedToBeUpdated.LastWriteDateTime,
                                                    AddErrorExiftooRegion, AddErrorExiftooCommandWrite, AddErrorExiftooParameterWrite, AddErrorExiftooParameterWrite,
                                                    "Issue: Failed write Xtra Atom property to file.\r\n" +
                                                    "File Name:   " + currentFilenameAfterUpdate + "\r\n" +
                                                    "File Status: " + fileStatus.ToString() + "\r\n" +
                                                    "Error message:" + writeXtraAtom_ErrorMessageForFile[fileEntrySupposedToBeUpdated.FileFullPath]);
                                            }
                                            #endregion

                                            #region Remove from Circle Progressbar when file doesn't exist anymore
                                            if (!File.Exists(fileEntrySupposedToBeUpdated.FileFullPath))
                                            {
                                                RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntrySupposedToBeUpdated, MetadataBrokerType.Queue));
                                                RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntrySupposedToBeUpdated, MetadataBrokerType.ExifTool));
                                                ImageListViewRemoveItemInvoke(fileEntrySupposedToBeUpdated.FileFullPath);
                                            }
                                            #endregion

                                            #region Get file LastWritten date on file to check if updated (currentFilenameAfterUpdate)
                                            DateTime currentLastWrittenDateTime = FileHandler.GetLastWriteTime(currentFilenameAfterUpdate, waitAndRetry: false);
                                            DateTime previousLastWrittenDateTime = (DateTime)fileEntrySupposedToBeUpdated.LastWriteDateTime;
                                            #endregion

                                            //Find last known writtenDate for file
                                            //Check if file is updated, if file LastWrittenDateTime has changed, file is updated
                                            //If file not found, current will become 1601.01.01, ref. Microsoft documentation
                                            if (currentLastWrittenDateTime <= previousLastWrittenDateTime)
                                            {
                                                #region Remove from SubsetQueue (Exiftool failed to write, no need to verify data that hasn't been written)
                                                if (indexExiftoolFailedOn > -1 && indexExiftoolFailedOn < exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser.Count)
                                                {
                                                    lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                                                    {
                                                        exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser.RemoveAt(indexExiftoolFailedOn);
                                                        exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate.RemoveAt(indexExiftoolFailedOn);
                                                    }
                                                }
                                                #endregion

                                                #region Updated ImageListView Error Status (currentFilenameAfterUpdate)
                                                FileStatus fileStatus = FileHandler.GetFileStatus(
                                                    currentFilenameAfterUpdate, checkLockedStatus: true,
                                                    hasErrorOccured: true, errorMessage: exiftoolErrorMessage,
                                                    exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);
                                                ImageListView_UpdateItemFileStatusInvoke(currentFilenameAfterUpdate, fileStatus);
                                                #endregion

                                                #region AddError - Exiftool failed (currentFilenameAfterUpdate)
                                                string lockedByProcessesText = "";
                                                if (fileStatus.HasAnyLocks) lockedByProcessesText = FileHandler.GetLockedByText(fileEntrySupposedToBeUpdated.FileFullPath);

                                                AddError(
                                                    Path.GetDirectoryName(currentFilenameAfterUpdate), Path.GetFileName(currentFilenameAfterUpdate), fileEntrySupposedToBeUpdated.LastWriteDateTime,
                                                    AddErrorExiftooRegion, AddErrorExiftooCommandWrite, AddErrorExiftooParameterWrite, AddErrorExiftooParameterWrite,
                                                    "Issue: EXIFTOOL.EXE failed write to file." + "\r\n" +
                                                    "File Name:  " + currentFilenameAfterUpdate + "\r\n" +
                                                    "File Status: " + FileHandler.GetFileStatusText(currentFilenameAfterUpdate, checkLockedStatus: true, showLockedByProcess: true) + "\r\n" +
                                                    "Error Message: " + exiftoolErrorMessage);
                                                #endregion
                                            }

                                            #region Add to Verify queue, clear thumbnail on ImageListView (exiftoolSave_QueueSubsetMetadataToSave contains only written)
                                            //When Write With Exiftool Fail, then file was removed from "exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser"
                                            int indexInVerifyQueue = Metadata.FindFileEntryInList(exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser, fileEntrySupposedToBeUpdated);
                                            if (indexInVerifyQueue > -1 && indexInVerifyQueue < exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser.Count)
                                            {
                                                Metadata currentMetadata;
                                                lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                                                {
                                                    currentMetadata = new Metadata(exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser[indexInVerifyQueue]);
                                                }
                                                currentMetadata.FileDateModified = currentLastWrittenDateTime;

                                                #region Save the metatdata into DataGridView(s) - saved metadata should also be readed back, if not, verify will tell save failed
                                                DataGridViewSetMetadataOnAllDataGridView(currentMetadata); //PS: Sets only Metadata, not updates the content
                                                DataGridViewSetDirtyFlagAfterSave(currentMetadata, false, FileEntryVersion.MetadataToSave); //Not set Dirty flag is false, not edit by user (yet)
                                                #endregion

                                                #region If file was updated - Add to Verify queue
                                                if (File.Exists(currentMetadata.FileFullPath) && currentLastWrittenDateTime != previousLastWrittenDateTime) 
                                                    AddQueueVerifyMetadataLock(currentMetadata);
                                                #endregion

                                                FileEntryAttribute fileEntryAttribute = 
                                                    new FileEntryAttribute(currentMetadata.FileEntryBroker, FileEntryVersion.CurrentVersionInDatabase);
                                                AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttribute);
                                                ImageListView_UpdateItemThumbnailUpdateAllInvoke(currentMetadata.FileEntryBroker);

                                                FileEntryAttribute fileEntryAttributeHistorical = 
                                                    new FileEntryAttribute(fileEntrySupposedToBeUpdated, FileEntryVersion.Historical);
                                                AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttributeHistorical);
                                            }
                                            else
                                            {
                                                Logger.Warn("Was not able to removed from queue, didn't exist in queue anymore: " + fileEntrySupposedToBeUpdated);
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
                                lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                                {
                                    exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser.Clear();
                                    exiftoolSave_QueueSubset_MetadataOrigialBeforeUserUpdate.Clear();
                                }
                                mediaFilesUpdatedByExiftool.Clear();
                                #endregion

                                //Status updated for user
                                ShowExiftoolSaveProgressClear();
                                
                                Thread.Sleep(50);
                            }

                            if (GlobalData.IsApplicationClosing)
                            {
                                lock (exiftoolSave_QueueSaveUsingExiftool_MetadataUpdatedByUserLock) exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.Clear();
                                lock (exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUserLock) exiftoolSave_QueueSaveUsingExiftool_MetadataOrigialBeforeUserUpdate.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("Saving ThreadSaveMetadata crashed.\r\n" +
                                "The ThreadSaveMetadata queue was cleared. Nothing was saved.\r\n" +
                                "Exception message:" + ex.Message + "\r\n",
                                "Saving ThreadSaveMetadata failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock) exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser.Clear();
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

        #region Media Thumbnail - Save To Database

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
                                            else
                                            {
                                                FileStatus fileStatus = FileHandler.GetFileStatus(fileEntryImage.FileFullPath);
                                                ImageListView_UpdateItemThumbnailUpdateAll_OnlyIfFileStatusHasChangedInvoke(fileEntryImage, fileStatus);
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
                                    KryptonMessageBox.Show("Saving ThreadSaveThumbnail crashed.\r\n" +
                                        "The ThreadSaveThumbnail queue was cleared. Nothing was saved.\r\n" +
                                        "Exception message:" + ex.Message + "\r\n",
                                        "Saving ThreadSaveThumbnail failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                    lock (commonQueueSaveToDatabaseMediaThumbnailLock) { commonQueueSaveToDatabaseMediaThumbnail.Clear(); } //Avoid loop, due to unknown error
                                    Logger.Error(ex, "ThreadSaveThumbnail");
                                }
                            }
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
                AddQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos));
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
                                                AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttribute);
                                            }
                                        }
                                    }
                                    else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                                    RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos));
                                    lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) if (commonQueueReadMetadataFromSourceMicrosoftPhotos.Count > 0) commonQueueReadMetadataFromSourceMicrosoftPhotos.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar
                                }
                            }
                            #endregion

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) commonQueueReadMetadataFromSourceMicrosoftPhotos.Clear();
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
                AddQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery));
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
                                                AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttribute);
                                            }
                                        }
                                    }
                                    else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                                    RemoveQueueLazyLoadningSelectedFilesLock(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery));
                                    lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) if (commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Count > 0) commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar
                                }
                                #endregion
                            }

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromSourceWindowsLivePhotoGalleryLock) commonQueueReadMetadataFromSourceWindowsLivePhotoGallery.Clear();
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
                                                                FileHandler.GetLastWriteTime(current_FileEntryBrokerRegion.FileFullPath, waitAndRetry: IsFileInTemporaryUnavailableLock(current_FileEntryBrokerRegion.FileFullPath)) == current_FileEntryBrokerRegion.LastWriteDateTime) //Check if the current Metadata are same as newest file 
                                                                fileHasCorrectDate = true;
                                                            else 
                                                                fileHasCorrectDate = false;
                                                        }
                                                        else
                                                        {
                                                            fileExists = false;
                                                            fileHasCorrectDate = false;
                                                        }

                                                        if (fileExists)
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
                                                                    hasErrorOccured: true, errorMessage: exceptionError);

                                                                if (fileStatus.IsInCloudOrVirtualOrOffline)
                                                                {
                                                                    #region Download from cloud failed in time (Remove frome queue)
                                                                    if (FileHandler.DidTouchedFileTimeoutDuringDownload(current_FileEntryBrokerRegion.FileFullPath))
                                                                    {
                                                                        fileNeedRemoveFromList = true;
                                                                        didExceptionOccureWhenLoading = true; //Was not able to download in time
                                                                        exceptionError += (string.IsNullOrWhiteSpace(exceptionError) ? "" : "\r\n") + "Download from clound timeout.";
                                                                    }
                                                                    #endregion

                                                                    #region If file not touch by now (by this, Exiftool or other processes) - Give error message
                                                                    if (dontReadFilesInCloud &&
                                                                        !FileHandler.IsOfflineFileTouched(current_FileEntryBrokerRegion.FileFullPath))
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
                                                                    Logger.Warn(writeErrorDesciption);

                                                                    //AddError(
                                                                    //    current_FileEntryBrokerRegion.Directory,
                                                                    //    current_FileEntryBrokerRegion.FileName,
                                                                    //    current_FileEntryBrokerRegion.LastWriteDateTime,
                                                                    //    AddErrorFileSystemRegion, AddErrorFileSystemRead,
                                                                    //    AddErrorFileSystemRead, AddErrorFileSystemRead,
                                                                    //    writeErrorDesciption);
                                                                }
                                                                #endregion
                                                            } else 
                                                            {
                                                                #region Log warning about wrong file date
                                                                if (!fileHasCorrectDate)
                                                                {
                                                                    FileStatus fileStatus = FileHandler.GetFileStatus(
                                                                    current_FileEntryBrokerRegion.FileFullPath, checkLockedStatus: true,
                                                                    hasErrorOccured: true);

                                                                    fileStatus.FileErrorMessage +=
                                                                        "File date has changed. Need create thumbnail for different date. Requested: " +
                                                                        current_FileEntryBrokerRegion.LastWriteDateTime.ToString() + " vs. File date: " +
                                                                        FileHandler.GetLastWriteTime(current_FileEntryBrokerRegion.FileFullPath, waitAndRetry: false);

                                                                    string errorDesciption =
                                                                        "Issue: Issue while create thumbnails for regions.\r\n" +
                                                                        "File Name: " + current_FileEntryBrokerRegion.FileFullPath + "\r\n" +
                                                                        "File Status: " + FileHandler.ConvertFileStatusToText(fileStatus) + "\r\n" +
                                                                        "Error Message: " + fileStatus.FileErrorMessage;
                                                                    Logger.Warn(errorDesciption);
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
                                                                    hasErrorOccured: true);
                                                            fileStatus.FileErrorMessage = "File not found.";
                                                            
                                                            string errorDesciption =
                                                                "Issue: Issue while create thumbnails for regions.\r\n" +
                                                                "File Name: " + current_FileEntryBrokerRegion.FileFullPath + "\r\n" +
                                                                "File Status: " + FileHandler.ConvertFileStatusToText(fileStatus) + "\r\n" +
                                                                "Error Message: " + fileStatus.FileErrorMessage;

                                                            ImageListView_UpdateItemFileStatusInvoke(current_FileEntryBrokerRegion.FileFullPath, fileStatus);
                                                            
                                                            Logger.Warn(errorDesciption);

                                                            //AddError(
                                                            //    current_FileEntryBrokerRegion.Directory,
                                                            //    current_FileEntryBrokerRegion.FileName,
                                                            //    current_FileEntryBrokerRegion.LastWriteDateTime,
                                                            //    AddErrorFileSystemRegion, AddErrorFileSystemRead,
                                                            //    AddErrorFileSystemRead, AddErrorFileSystemRead,
                                                            //    errorDesciption);
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
                                                        DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttribute, MetadataBrokerType.Empty); //Updated Gridview

                                                        FileEntryAttribute fileEntryAttributeHistorical = new FileEntryAttribute(current_FileEntryBrokerRegion, FileEntryVersion.Historical);
                                                        DataGridView_Populate_FileEntryAttributeInvoke(fileEntryAttributeHistorical, MetadataBrokerType.Empty);
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
                lock (exiftoolSave_QueueSaveUsingExiftool_MetadataUpdatedByUserLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser)
                    {
                        if (metadata.FileFullPath.StartsWith(folder, comparisonType: StringComparison.OrdinalIgnoreCase))
                        {
                            folderInUse = true;
                            break;
                        }
                    }
            

            if (!folderInUse)
                lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser)
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

        #region Check ThreadQueues - IsFileInTemporaryUnavailableLock
        public bool IsFileInTemporaryUnavailableLock(string fullFilename)
        {
            bool fileInUse = false;
            if (!fileInUse)
                lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser)
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
                lock (exiftoolSave_QueueSaveUsingExiftool_MetadataUpdatedByUserLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser)
                    {
                        if (FilesCutCopyPasteDrag.IsFilenameEqual(metadata.FileFullPath, fullFilename))
                        {
                            fileInUse = true;
                            break;
                        }
                    }
            
            if (!fileInUse)
                lock (exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUserLock)
                    foreach (Metadata metadata in exiftoolSave_QueueSubset_MetadataToSaveUpdatedByUser)
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
                                        if (!fileInUse)
                                        {
                                            DateTime currentLastWrittenDateTime = FileHandler.GetLastWriteTime(fullFilename, waitAndRetry: IsFileInTemporaryUnavailableLock(fullFilename));
                                            if (currentLastWrittenDateTime > FileHandler.MinimumFileSystemDateTime)
                                            {
                                                FileEntry fileEntry = new FileEntry(fullFilename, currentLastWrittenDateTime);
                                                FileEntryBroker fileEntryBroker = new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool);
                                                Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBroker);
                                                if (metadata != null) break; //Metadata found -> contine to rename

                                                FileEntryBroker fileEntryBrokerError = new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError);
                                                Metadata metadataError = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerError);

                                                if (metadataError != null) break; //Metadata found -> contine to rename

                                                if (!FileHandler.DidTouchedFileTimeoutDuringDownload(fileEntry.FileFullPath))
                                                {
                                                    FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.CurrentVersionInDatabase);
                                                    AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttribute);
                                                    fileInUse = true;
                                                } else fileInUse = false; //Remove from queue, due to timeout, if rename fails it will give an error

                                            }
                                            break; //File not in use found, start rename it
                                        }
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
                                        DateTime currentLastWrittenDateTime = FileHandler.GetLastWriteTime(fullFilename, waitAndRetry: IsFileInTemporaryUnavailableLock(fullFilename));
                                        FileEntry fileEntry = new FileEntry(fullFilename, currentLastWrittenDateTime);

                                        FileEntryBroker fileEntryBroker = new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool);
                                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBroker);
                                        
                                        if (metadata != null)
                                        {
                                            DataGridViewHandlerRename.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                                            DataGridViewHandlerRename.RenameVaribale = renameVaiable;
                                            DataGridViewHandlerRename.ComputerNames = new List<string>(oneDriveNetworkNames);
                                            DataGridViewHandlerRename.GPStag = Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix;
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
                                            FileEntryBroker fileEntryBrokerError = new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError);
                                            Metadata metadataError = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerError);

                                            if (metadataError == null)
                                            {
                                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry, FileEntryVersion.CurrentVersionInDatabase);
                                                AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttribute);
                                            }
                                            else 
                                            {
                                                string error =
                                                    "Failed rename " + fullFilename + " to : New name is unknown(missing metadata)";
                                                Logger.Error(error);

                                                FileStatus fileStatus = FileHandler.GetFileStatus(
                                                    fullFilename, checkLockedStatus: true,
                                                    hasErrorOccured: true, errorMessage: error);
                                                ImageListView_UpdateItemFileStatusInvoke(fullFilename, fileStatus);

                                                error =
                                                    "Issue: Failed to rename file. Missing metadata for creating new filename\r\n" +
                                                    "File Name:   " + fullFilename + "\r\n" +
                                                    "File Status: " + fileStatus.ToString() + "\r\n" +
                                                    "Error Message: Missing Metatdata, can't create new filename";

                                                Logger.Error(error);

                                                AddError(
                                                    Path.GetDirectoryName(fullFilename), Path.GetFileName(fullFilename), fileStatus.LastWrittenDateTime,
                                                    AddErrorFileSystemRegion, AddErrorFileSystemMove, fullFilename, "New name is unknown (missing metadata)",
                                                    error);
                                            }
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


        #region AutoCorrect

        #region AutoCorrect - AddQueueAutoCorrectLock
        public void AddQueueAutoCorrectLock(FileEntryBroker fileEntryBroker, AutoCorrectFormVaraibles autoCorrectFormVaraibles)
        {
            lock (commonQueueAutoCorrectLock)
            {
                if (commonQueueAutoCorrect.ContainsKey(fileEntryBroker))
                {
                    commonQueueAutoCorrect[fileEntryBroker] = autoCorrectFormVaraibles;
                }
                else
                {
                    commonQueueAutoCorrect.Add(fileEntryBroker, autoCorrectFormVaraibles);
                }
            }
        }
        #endregion

        #region AutoCorrect - ThreadAutoCorrect
        public void ThreadAutoCorrect()
        {
            try
            {
                lock (_ThreadAutoCorrectLock) if (_ThreadAutoCorrect != null || CommonQueueAutoCorrectDirty() <= 0) return;

                lock (_ThreadAutoCorrectLock)
                {

                    _ThreadAutoCorrect = new Thread(() =>
                    {
                        try
                        {
                            #region AutoCorrect
                            Logger.Trace("ThreadCorrect - started");
                            int count = CommonQueueAutoCorrectLock();
                            while (count > 0 && CommonQueueAutoCorrectLock() > 0 && !GlobalData.IsApplicationClosing)
                            {
                                count--;

                                Dictionary<FileEntryBroker, AutoCorrectFormVaraibles> hasMetadataExiftool = new Dictionary<FileEntryBroker, AutoCorrectFormVaraibles>();
                                HashSet<FileEntryBroker> hasMetadataExiftoolError = new HashSet<FileEntryBroker>();

                                #region Find all that has metadata or error
                                lock (commonQueueAutoCorrectLock)
                                {
                                    foreach (KeyValuePair<FileEntryBroker, AutoCorrectFormVaraibles> keyValuePair in commonQueueAutoCorrect)
                                    {
                                        Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(keyValuePair.Key);
                                        if (metadataInCache != null) hasMetadataExiftool.Add(keyValuePair.Key, keyValuePair.Value);
                                        else
                                        {
                                            FileEntryBroker fileEntryBrokerError = new FileEntryBroker(keyValuePair.Key, MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError);
                                            Metadata metadataErrorInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerError);
                                            if (metadataErrorInCache != null) hasMetadataExiftoolError.Add(keyValuePair.Key);
                                        }
                                    } 
                                }
                                #endregion

                                #region Remove from Queue
                                lock (commonQueueAutoCorrectLock)
                                {
                                    foreach (FileEntryBroker fileEntryBroker in hasMetadataExiftoolError)
                                    {
                                        commonQueueAutoCorrect.Remove(fileEntryBroker);
                                    }
                                    foreach (FileEntryBroker fileEntryBroker in hasMetadataExiftool.Keys)
                                    {
                                        commonQueueAutoCorrect.Remove(fileEntryBroker);
                                    }
                                }
                                #endregion
                                

                                AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                                float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                                float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                                int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                                FileEntryVersion fileEntryVersion = FileEntryVersion.MetadataToSave;


                                foreach (KeyValuePair<FileEntryBroker, AutoCorrectFormVaraibles> keyValuePair in hasMetadataExiftool)
                                {
                                    FileEntryBroker fileEntryBroker = keyValuePair.Key;
                                    AutoCorrectFormVaraibles autoCorrectFormVaraibles = keyValuePair.Value;
                                    //autoCorrectFormVaraibles.WriteAlbumOnDescription = autoCorrect.UpdateDescription;

                                    Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBroker);
                                    if (metadataInCache != null)
                                    {
                                        Metadata metadata = new Metadata(metadataInCache);

                                        Metadata metadataToSave = autoCorrect.RunAlgorithmReturnCopy(metadata,
                                        databaseAndCacheMetadataExiftool,
                                        databaseAndCacheMetadataMicrosoftPhotos,
                                        databaseAndCacheMetadataWindowsLivePhotoGallery,
                                        databaseAndCahceCameraOwner,
                                        databaseLocationNameAndLookUp,
                                        databaseGoogleLocationHistory,
                                        locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                        autoKeywordConvertions,
                                        Properties.Settings.Default.RenameDateFormats);

                                        if (metadataToSave != null)
                                        {
                                            if (Properties.Settings.Default.WriteAutoKeywordsSynonyms) AutoKeywords(ref metadataToSave);
                                            if (autoCorrectFormVaraibles != null) AutoCorrectFormVaraibles.UseAutoCorrectFormData(ref metadataToSave, autoCorrectFormVaraibles);
                                            if (Properties.Settings.Default.WriteUsingCompatibilityCheck) AutoCorrect.CompatibilityCheckMetadata(ref metadataToSave, fixDateTaken: false);

                                            MicrosoftLocationHack(ref metadataToSave, metadata, Properties.Settings.Default.MicosoftOneDriveLocationHackUse, Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix);
                                            DataGridView_Populate_CompatibilityCheckedMetadataToSave(metadataToSave, fileEntryVersion);
                                            AddQueueSaveUsingExiftoolMetadataUpdatedByUserLock(metadataToSave, new Metadata(metadataInCache));
                                            //Need use metadataToSave.FullFilePath, Because When Exiftool output filename can be diffrent to input filename
                                            AddQueueRenameMediaFilesLock(metadata.FileFullPath, autoCorrect.RenameVariable);
                                        }
                                    }
                                }
                                Thread.Sleep(100);
                            }
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            KryptonMessageBox.Show("ThreadAutoCorrect crashed.\r\n" +
                                "The ThreadAutoCorrect queue was cleared. Nothing was saved.\r\n" +
                                "Exception message:" + ex.Message + "\r\n",
                                "Saving ThreadAutoCorrect failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                            lock (commonQueueAutoCorrectLock) commonQueueAutoCorrect.Clear();
                            Logger.Error(ex, "ThreadAutoCorrect");
                        }
                        finally
                        {
                            lock (_ThreadAutoCorrectLock) _ThreadAutoCorrect = null;
                            Logger.Trace("ThreadAutoCorrect - ended");
                        }
                    });

                    lock (_ThreadAutoCorrectLock) if (_ThreadAutoCorrect != null)
                        {
                            _ThreadAutoCorrect.Priority = threadPriority;
                            _ThreadAutoCorrect.Start();
                        }
                        else Logger.Error("_ThreadAutoCorrectLock was not able to start");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "_ThreadAutoCorrect.Start failed. ");
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
                    if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.CurrentVersionInDatabase && !commonLazyLoadingMapNomnatatim.ContainsKey(fileEntryAttribute)) 
                        commonLazyLoadingMapNomnatatim.Add(fileEntryAttribute, forceReloadUsingReverseGeocoder);
                }
            }
        }
        #endregion

        #region MapNomnatatim - LazyLoading - Thread 
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
                        #region Do the work
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
                                    KeyValuePair<FileEntryAttribute, bool> fileEntryAttributeAndAllowUseMetadataLocationInfo;

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
/*
if (FileHandler.DoesFileExists(fileEntryAttributeAndAllowUseMetadataLocationInfo.Key.FileFullPath))
{
    if (!isPopulated && fileEntryVersionCompare != FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing)
    {
        AddQueueLazyLoadingMapNomnatatimLock(
            fileEntryAttributeAndAllowUseMetadataLocationInfo.Key,
            fileEntryAttributeAndAllowUseMetadataLocationInfo.Value);
    }
}
else
{
    //DEBUG
}*/
                                }
                                Thread.Sleep(3);
                                #endregion
                            }

                            if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromSourceMicrosoftPhotosLock) commonQueueReadMetadataFromSourceMicrosoftPhotos.Clear();
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
