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

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        private Thread _ThreadLazyLoadingMetadata = null;
        private Thread _ThreadLazyLoadingThumbnail = null;

        private Thread _ThreadExiftool = null;
        private Thread _ThreadThumbnailMedia = null;
        private Thread _ThreadMicrosoftPhotos = null;
        private Thread _ThreadWindowsLiveGallery = null;
        private Thread _ThreadThumbnailRegion = null;

        private Thread _ThreadSaveMetadata = null;
        private Thread _ThreadRenameMedafiles = null;

        List<string> mediaFilesNotInDatabase = new List<string>(); //It's globale, just to manage to show count status

        private List<FileEntryAttribute> commonQueueLazyLoadingMetadata = new List<FileEntryAttribute>();
        private static readonly Object commonQueueLazyLoadingMetadataLock = new Object();

        private List<FileEntryAttribute> commonQueueLazyLoadingThumbnail = new List<FileEntryAttribute>();
        private static readonly Object commonQueueLazyLoadingThumbnailLock = new Object();

        //Region "Face" thumbnails
        private List<Metadata>         commonQueueReadPosterAndSaveFaceThumbnails = new List<Metadata>();
        private static readonly Object commonQueueReadPosterAndSaveFaceThumbnailsLock = new Object();

        //Thumbnail
        private List<FileEntryImage>   commonQueueSaveThumbnailToDatabase = new List<FileEntryImage>();
        private static readonly Object commonQueueSaveThumbnailToDatabaseLock = new Object();

        //Microsoft Photos
        private List<FileEntry>        commonQueueReadMetadataFromMicrosoftPhotos = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromMicrosoftPhotosLock = new Object();

        //Windows Live Photo Gallery
        private List<FileEntry>        commonQueueReadMetadataFromWindowsLivePhotoGallery = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromWindowsLivePhotoGalleryLock = new Object();

        //Exif
        private List<FileEntry>        commonQueueReadMetadataFromExiftool = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromExiftoolLock = new Object();
        private List<Metadata>         commonQueueSaveMetadataUpdatedByUser = new List<Metadata>();
        private static readonly Object commonQueueSaveMetadataUpdatedByUserLock = new Object();
        private List<Metadata>         commonOrigialMetadataBeforeUserUpdate = new List<Metadata>();
        private static readonly Object commonOrigialMetadataBeforeUserUpdateLock = new Object();
        private List<Metadata>         commonQueueMetadataWrittenByExiftoolReadyToVerify = new List<Metadata>();
        private static readonly Object commonQueueMetadataWrittenByExiftoolReadyToVerifyLock = new Object();

        //Rename
        private static Dictionary<string, string> commonQueueRename = new Dictionary<string, string>();
        private static readonly Object            commonQueueRenameLock = new Object();

        //Error handling
        private Dictionary<string, string> queueErrorQueue = new Dictionary<string, string>();
        private static readonly Object     queueErrorQueueLock = new Object();


        #region Lock
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
            ThreadSaveThumbnail();
            ThreadSaveMetadata(); 
            ThreadRename();
            ThreadLazyLoadningMetadata();
            ThreadLazyLoadningThumbnail();
        }

        private bool IsAnyThreadRunning()
        {
            return (
                (_ThreadThumbnailMedia == null || _ThreadThumbnailMedia.IsAlive) ||
                (_ThreadExiftool == null || _ThreadExiftool.IsAlive) ||
                (_ThreadWindowsLiveGallery == null || _ThreadWindowsLiveGallery.IsAlive) ||
                (_ThreadMicrosoftPhotos == null || _ThreadMicrosoftPhotos.IsAlive) ||
                (_ThreadThumbnailRegion == null || _ThreadThumbnailRegion.IsAlive) ||
                (_ThreadSaveMetadata == null || _ThreadSaveMetadata.IsAlive)
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

        #region LazyLoadning - Metadata

        #region LazyLoadning - Metadata - AddQueue - Read from Cache, then Database, then Source and Save
        public void AddQueueMetadataReadToCacheOrUpdateFromSoruce(FileEntry fileEntry)
        {
            //When file is DELETE, LastWriteDateTime become null
            if (fileEntry.LastWriteDateTime != null)
            {
                if (File.GetLastWriteTime(fileEntry.FileFullPath) == fileEntry.LastWriteDateTime) //Don't add old files in queue
                {
                    //If Metadata don't exisit in database, put it in read queue
                    Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                    if (metadata == null) AddQueueExiftool(fileEntry);

                    metadata = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos));
                    if (metadata == null) AddQueueMicrosoftPhotos(fileEntry);

                    metadata = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery));
                    if (metadata == null) AddQueueWindowsLivePhotoGallery(fileEntry);
                }
                //if (databaseAndCacheThumbnail.ReadThumbnailFromCacheOrDatabase(fileEntryImage) == null) AddQueueSaveThumbnail(fileEntryImage);
            }
            else
            {
                Debug.WriteLine("AddQueueAllUpadtedFileEntry was delete: (Check why), renname of exiftool maybe, need back then... " + fileEntry.FileFullPath);
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
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes) if (!commonQueueLazyLoadingMetadata.Contains(fileEntryAttribute)) commonQueueLazyLoadingMetadata.Add(fileEntryAttribute); 
            }
            StartThreads();
        }
        #endregion

        #region LazyLoadning - Metadata - Thread 
        public void ThreadLazyLoadningMetadata()
        {
            if (_ThreadLazyLoadingMetadata == null || (!_ThreadLazyLoadingMetadata.IsAlive && CommonQueueLazyLoadingMetadataCountLock() > 0))
            {
                _ThreadLazyLoadingMetadata = new Thread(() =>
                {
                    while (CommonQueueLazyLoadingMetadataCountLock() > 0 && !GlobalData.IsApplicationClosing)
                    {
                        int queueCount = CommonQueueLazyLoadingMetadataCountLock();

                        for (int queueIndex = 0; queueIndex < queueCount; queueIndex++)
                        {
                            Metadata metadata;
                            FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(commonQueueLazyLoadingMetadata[queueIndex]);

                            if (databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.ExifTool)) == null)
                            {
                                metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.ExifTool));
                               // if (metadata == null) AddQueueExiftool(fileEntry);                                 
                            }
                            
                            if (databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.MicrosoftPhotos)) == null)
                            {
                                metadata = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.MicrosoftPhotos));
                                //if (metadata == null) AddQueueMicrosoftPhotos(fileEntry);
                            }

                            if (databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.WindowsLivePhotoGallery)) == null)
                            {
                                metadata = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.WindowsLivePhotoGallery));
                                //if (metadata == null) AddQueueWindowsLivePhotoGallery(fileEntry);
                            }

                            PopulateDataGridViewForFileEntryAttributeInvoke(fileEntryAttribute);                            
                        }

                        lock (commonQueueLazyLoadingMetadataLock)
                        {
                            for (int queueIndex = 0; queueIndex < queueCount; queueIndex++) commonQueueLazyLoadingMetadata.RemoveAt(0);                            
                        }

                        Application.DoEvents();
                    }

                    StartThreads();
                });
                try
                {
                    _ThreadLazyLoadingMetadata.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadLazyLoading.Start failed. " + ex.Message);
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
                foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes) if (!commonQueueLazyLoadingThumbnail.Contains(fileEntryAttribute)) commonQueueLazyLoadingThumbnail.Add(fileEntryAttribute);
            }
            StartThreads();
        }
        #endregion

        #region LazyLoadning - Thread LazyLoadning
        public void ThreadLazyLoadningThumbnail()
        {
            if (_ThreadLazyLoadingThumbnail == null || (!_ThreadLazyLoadingThumbnail.IsAlive && CommonQueueLazyLoadingThumbnailCountLock() > 0))
            {
                _ThreadLazyLoadingThumbnail = new Thread(() =>
                {
                    while (CommonQueueLazyLoadingThumbnailCountLock() > 0 && !GlobalData.IsApplicationClosing)
                    {
                        int queueCount = CommonQueueLazyLoadingThumbnailCountLock();

                        for (int queueIndex = 0; queueIndex < queueCount; queueIndex++)
                        {
                            FileEntryAttribute fileEntryAttribute = commonQueueLazyLoadingThumbnail[queueIndex];

                            if (databaseAndCacheThumbnail.ReadThumbnailFromCacheOnly(fileEntryAttribute) == null)
                            {
                                Image image = databaseAndCacheThumbnail.ReadThumbnailFromCacheOrDatabase(fileEntryAttribute);
                                if (image != null) UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(fileEntryAttribute, image);
                            }
                        }

                        lock (commonQueueLazyLoadingThumbnailLock)
                        {
                            for (int queueIndex = 0; queueIndex < queueCount; queueIndex++) commonQueueLazyLoadingThumbnail.RemoveAt(0);
                        }

                        Application.DoEvents();
                    }

                    StartThreads();
                });
                try
                {
                    _ThreadLazyLoadingThumbnail.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadLazyLoading.Start failed. " + ex.Message);
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
            if (_ThreadThumbnailMedia == null || (!_ThreadThumbnailMedia.IsAlive && CommonQueueSaveThumbnailToDatabaseCountLock() > 0))
            {
                _ThreadThumbnailMedia = new Thread(() =>
                {
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
                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });

                try
                {
                    _ThreadThumbnailMedia.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadThumbnailMedia failed to start. " + ex.Message);
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
            if (_ThreadExiftool == null || (!_ThreadExiftool.IsAlive && CommonQueueReadMetadataFromExiftoolCountLock() > 0))
            {
                _ThreadExiftool = new Thread(() =>
                {
                    Thread.Sleep(300); //Wait more to become updated;

                    while (CommonQueueReadMetadataFromExiftoolCountLock() > 0 && !GlobalData.IsApplicationClosing) //In case some more added to the queue
                    {
                        if (CommonQueueSaveMetadataUpdatedByUserCountLock() > 0) break; //Write first, read later on...

                        int rangeToRemove; //Remember how many in queue now
                        mediaFilesNotInDatabase = new List<string>();

                        #region From the Read Queue - Find files not alread in database
                        lock (commonQueueReadMetadataFromExiftoolLock)
                        {
                            rangeToRemove = commonQueueReadMetadataFromExiftool.Count;
                            mediaFilesNotInDatabase.AddRange(databaseAndCacheMetadataExiftool.ListAllMissingFileEntries(MetadataBrokerType.ExifTool, commonQueueReadMetadataFromExiftool.GetRange(0, rangeToRemove)));
                            commonQueueReadMetadataFromExiftool.RemoveRange(0, rangeToRemove);
                        }
                        #endregion

                        while (mediaFilesNotInDatabase.Count > 0 && !GlobalData.IsApplicationClosing)
                        {
                            #region Create a subset of files to Read using Exiftool command line with parameters, and remove subset from queue
                            int range = 0;
                            //On computers running Microsoft Windows XP or later, the maximum length of the string that you can 
                            //use at the command prompt is 8191 characters. On computers running Microsoft Windows 2000 or 
                            //Windows NT 4.0, the maximum length of the string that you can use at the command prompt is 2047 
                            //characters.

                            int argumnetLength = 80; //Init command length;
                            while (argumnetLength < 2047 && range < mediaFilesNotInDatabase.Count)
                            {
                                argumnetLength += mediaFilesNotInDatabase[range].Length + 3; //+3 = space and 2x"
                                range++;
                            }
                            if (argumnetLength > 2047) range--;

                            List<String> useExiftoolOnThisSubsetOfFiles = mediaFilesNotInDatabase.GetRange(0, range);
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
                                    }
                                }
                                AddQueueCreateRegionFromPoster(metadataRead);
                                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(metadataRead.FileFullPath, (DateTime)metadataRead.FileDateModified, FileEntryVersion.Current);
                                PopulateDataGridViewForFileEntryAttributeInvoke(fileEntryAttribute);
                                RefreshHeaderImageAndRegionsOnActiveDataGridView(fileEntryAttribute);
                            }
                            #endregion

                            mediaFilesNotInDatabase.RemoveRange(0, range); //Remove subset from queue before update status bar
                            DisplayAllQueueStatus();
                        }
                    }

                    exiftoolReader.MetadataGroupPrioityWrite(); //Updated json config file if new tags found

                    if (GlobalData.IsApplicationClosing) lock (commonQueueReadMetadataFromExiftoolLock) commonQueueReadMetadataFromExiftool.Clear();
                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });

                try
                {
                    _ThreadExiftool.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadExiftool.Start failed. " + ex.Message);
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
            if (_ThreadSaveMetadata == null || (!_ThreadSaveMetadata.IsAlive && CommonQueueSaveMetadataUpdatedByUserCountLock() > 0))
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

                                Metadata currentMetadata = new Metadata(queueSubsetMetadataToSave[Metadata.FindFileEntryInList(queueSubsetMetadataToSave, fileSuposeToBeUpdated)]);

                                if (!failToSaveXtraAtom && !failToSaveUsingExiftool)
                                {
                                    currentMetadata.FileDateModified = currentLastWrittenDateTime;
                                    AddQueueVerifyMetadata(currentMetadata);

                                    //DEBUG JTN 
                                    //bool wasUpdated = ImageListViewReloadThumbnail(imageListView1, fileSuposeToBeUpdated.FileFullPath);
                                    //ImageListView will refresh Thumbnail and Force Metadata to be read again. Then don't need put in queue again

                                    //if (!wasUpdated) 
                                    AddQueueMetadataReadToCacheOrUpdateFromSoruce(currentMetadata.FileEntryBroker);

                                    ImageListViewReloadThumbnailInvoke(imageListView1, fileSuposeToBeUpdated.FileFullPath);
                                }
                                // Errors will be found when verify read
                                //{                                    
                                //    currentMetadata.FileDateModified = metadataError.FileDateModified = DateTime.Now; //fileSuposeToBeUpdated.LastWriteDateTime; //Can create duplicates
                                //    currentMetadata.Broker |= MetadataBrokerTypes.ExifToolWriteError;
                                //    databaseAndCacheMetadataExiftool.TransactionBeginBatch();
                                //    databaseAndCacheMetadataExiftool.Write(currentMetadata);
                                //    databaseAndCacheMetadataExiftool.TransactionCommitBatch();
                                //}
                            }
                        }
                        #endregion

                        //Clean up
                        queueSubsetMetadataToSave.Clear();
                        queueSubsetMetadataOrginalBeforeUserEdit.Clear();
                        mediaFilesWithChangesWillBeUpdated.Clear();

                        //Status updated for user
                        ShowExiftoolSaveProgressStop();
                        DisplayAllQueueStatus();

                        Thread.Sleep(100); //Wait in case of loop
                    }

                    if (GlobalData.IsApplicationClosing)
                    {
                        lock (commonQueueSaveMetadataUpdatedByUserLock) commonQueueSaveMetadataUpdatedByUser.Clear();
                        lock (commonOrigialMetadataBeforeUserUpdateLock) commonOrigialMetadataBeforeUserUpdate.Clear();
                    }

                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });

                try
                {
                    _ThreadSaveMetadata.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadSaveMetadata.Start failed. " + ex.Message);
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
            if (_ThreadWindowsLiveGallery == null || (!_ThreadWindowsLiveGallery.IsAlive && CommonQueueReadMetadataFromWindowsLivePhotoGalleryCountLock() > 0))
            {
                _ThreadWindowsLiveGallery = new Thread(() =>
                {
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
                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });
                try
                {
                    _ThreadWindowsLiveGallery.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadWindowsLiveGallery.Start failed. " + ex.Message);
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
            if (_ThreadMicrosoftPhotos == null || (!_ThreadMicrosoftPhotos.IsAlive && CommonQueueReadMetadataFromMicrosoftPhotosCountLock() > 0))
            {
                _ThreadMicrosoftPhotos = new Thread(() =>
                {
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
                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });

                try
                {
                    _ThreadMicrosoftPhotos.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadMicrosoftPhotos.Start failed. " + ex.Message);
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
            ThreadReadMediaPosterSaveRegions();
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
            if (_ThreadThumbnailRegion == null || (!_ThreadThumbnailRegion.IsAlive && CommonQueueReadPosterAndSaveFaceThumbnailsCountLock() > 0))
            {
                if (IsThreadRunningExcept_ThreadThumbnailRegion()) return; //Wait other thread to finnish first. Otherwise it will generate high load on disk use

                _ThreadThumbnailRegion = new Thread(() =>
                {                    
                    while (CommonQueueReadPosterAndSaveFaceThumbnailsCountLock() > 0 && !GlobalData.IsApplicationClosing && !IsThreadRunningExcept_ThreadThumbnailRegion()) //In case some more added to the queue
                    {
                        int queueCount = CommonQueueReadPosterAndSaveFaceThumbnailsCountLock(); //Mark count that we will work with. 

                        try
                        {                            
                            FileEntry fileEntryRegion;
                            lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                            {
                                fileEntryRegion = new FileEntry(commonQueueReadPosterAndSaveFaceThumbnails[0].FileEntryBroker);                                                                
                            }

                            bool foundFile = false;
                            do //Remove all with same filename in the queue
                            {
                                foundFile = false;
                                //Check Exiftool, Microsoft Phontos, Windows Live Photo Gallery in queue also

                                lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                                {
                                    Image image = null; //No image loaded
                                    for (int thumbnailIndex = 0; thumbnailIndex < queueCount; thumbnailIndex++)
                                    {

                                        //Find current file entry in queue, Exiftool, Microsoft Photos, Windows Live Gallery, etc...
                                        if (commonQueueReadPosterAndSaveFaceThumbnails[thumbnailIndex].FileFullPath == fileEntryRegion.FileFullPath &&
                                            commonQueueReadPosterAndSaveFaceThumbnails[thumbnailIndex].FileDateModified == fileEntryRegion.LastWriteDateTime)
                                        {
                                            //When found entry, check if has Face Regions to save
                                            if (commonQueueReadPosterAndSaveFaceThumbnails[thumbnailIndex].PersonalRegionList.Count > 0)
                                            {
                                                ExiftoolWriter.WaitLockedFileToBecomeUnlocked(fileEntryRegion.FileFullPath);
                                                
                                                //Check if the current Metadata are same as newst file...
                                                if (File.GetLastWriteTime(fileEntryRegion.FileFullPath) == fileEntryRegion.LastWriteDateTime)
                                                {

                                                    if (image == null) image = LoadMediaCoverArtPoster(fileEntryRegion.FileFullPath, true); //Only load once when found

                                                    if (image != null) //If still Failed load cover art, often occur after filed is moved or deleted
                                                    {
                                                        databaseAndCacheMetadataExiftool.TransactionBeginBatch(); //Only load image when regions found
                                                        //Metadata found and updated, updated DataGricView                                             
                                                        RegionThumbnailHandler.SaveThumbnailsForRegioList(databaseAndCacheMetadataExiftool, commonQueueReadPosterAndSaveFaceThumbnails[thumbnailIndex], image);
                                                        foundFile = true;

                                                        PopulateDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(fileEntryRegion, FileEntryVersion.Current)); //Updated Gridview
                                                    }
                                                    else Logger.Error("ThreadReadMediaPosterSaveRegions failed to create 'face' region thumbails from file. Due to mediafile do not exist anymore. File:" + commonQueueReadPosterAndSaveFaceThumbnails[0].FileName);
                                                }
                                                else Logger.Info("Don't load posters when request are with Diffrent LastWrittenDateTime:" + commonQueueReadPosterAndSaveFaceThumbnails[0].FileName);
                                            }

                                            queueCount--;
                                            commonQueueReadPosterAndSaveFaceThumbnails.RemoveAt(thumbnailIndex);
                                            if (foundFile) break; //No need to search more.
                                        }
                                    }
                                }
                                
                            } while (foundFile);

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
                });
                try { 
                    _ThreadThumbnailRegion.Start();
                }
                catch (Exception ex)
                {
                    Logger.Error("_ThreadThumbnailRegion.Start failed. " + ex.Message);
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
            

            if (_ThreadRenameMedafiles == null || (!_ThreadRenameMedafiles.IsAlive && CommonQueueRenameCountLock() > 0))
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
                        ShowExiftoolSaveProgressStop();
                        DisplayAllQueueStatus();

                        Thread.Sleep(100); //Wait in case of loop
                    }

                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });
                try
                {
                    _ThreadRenameMedafiles.Start();
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
        private string listOfErrors = "";
        private bool hasWriteAndVerifyMetadataErrors = false;

        const string AddErrorFileSystemRegion = "FileSystem";
        const string AddErrorFileSystemCopy = "Copy";
        const string AddErrorFileSystemMove = "Move";
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

        FormMessageBox formMessageBox = null;
        private void timerShowErrorMessage_Tick(object sender, EventArgs e)
        {
            timerShowErrorMessage.Stop();
            if (hasWriteAndVerifyMetadataErrors)
            {
                string errors = listOfErrors;
                listOfErrors = "";
                hasWriteAndVerifyMetadataErrors = false;

                //MessageBox.Show(errors, "Warning or Errors has occured!", MessageBoxButtons.OK);
                if (formMessageBox == null) formMessageBox = new FormMessageBox(errors);
                else formMessageBox.AppendMessage(errors);
                formMessageBox.ShowDialog();
                formMessageBox = null;
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
