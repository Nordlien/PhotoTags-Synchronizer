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

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        private Thread _ThreadExiftool = null;
        private Thread _ThreadThumbnailMedia = null;
        private Thread _ThreadMicrosoftPhotos = null;
        private Thread _ThreadWindowsLiveGallery = null;
        private Thread _ThreadThumbnailRegion = null;

        private Thread _ThreadSaveMetadata = null;

        private List<string> mediaFilesNotInDatabase = new List<string>();

        //Read metadata
        private List<Metadata> commonQueueReadPosterAndSaveFaceThumbnails = new List<Metadata>();
        private static readonly Object commonQueueReadPosterAndSaveFaceThumbnailsLock = new Object();

        private List<FileEntryImage> commonQueueSaveThumbnailToDatabase = new List<FileEntryImage>();
        private static readonly Object commonQueueSaveThumbnailToDatabaseLock = new Object();

        private List<FileEntry> commonQueueReadMetadataFromMicrosoftPhotos = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromMicrosoftPhotosLock = new Object();
        private List<FileEntry> commonQueueReadMetadataFromWindowsLivePhotoGallery = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromWindowsLivePhotoGalleryLock = new Object();

        private List<FileEntry> commonQueueReadMetadataFromExiftool = new List<FileEntry>();
        private static readonly Object commonQueueReadMetadataFromExiftoolLock = new Object();
        private List<Metadata> commonQueueSaveMetadataUpdatedByUser = new List<Metadata>();
        private static readonly Object commonQueueSaveMetadataUpdatedByUserLock = new Object();
        private List<Metadata> commonOrigialMetadataBeforeUserUpdate = new List<Metadata>();
        private static readonly Object commonOrigialMetadataBeforeUserUpdateLock = new Object();
        private List<Metadata> commonQueueMetadataWrittenByExiftoolReadyToVerify = new List<Metadata>();
        private static readonly Object commonQueueMetadataWrittenByExiftoolReadyToVerifyLock = new Object();
        //Error handling
        private Dictionary<string, string> queueErrorQueue = new Dictionary<string, string>();
        private static readonly Object queueErrorQueueLock = new Object();


        #region Lock
        private int MediaFilesNotInDatabaseCount()
        {
            lock (mediaFilesNotInDatabase) return mediaFilesNotInDatabase.Count;
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

        #endregion

        #region Start Thread, IsAnyThreadRunning, Tell when all queues are empty
        private void timerStartThread_Tick(object sender, EventArgs e)
        {
            StartThreads();
            TriggerAutoResetEventQueueEmpty();
        }

        private void StartThreads()
        {
            ThreadCollectMetadataExiftool();
            ThreadCollectMetadataMicrosoftPhotos();
            ThreadCollectMetadataWindowsLiveGallery();
            ThreadSaveThumbnail();
            ThreadSaveMetadata();
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

                if (WaitCacheEmpty != null)
                {
                    lock (WaitCacheEmpty)
                    {
                        WaitCacheEmpty.Set();
                    }
                }
            }
        }
        #endregion

        #region AddQueue - GetThumbnailAndWriteNewToDatabase
        /// <summary>
        /// Load a Thumbnail for given file for last written datetime
        ///     1. Read first from database and return Thumbnail
        /// If not found, then read from file
        ///     1. Then try load from Cover Art
        ///     2. Then read full media and create thumbnail
        ///     3. ---- Add MediaFile to Thread Qeueu with image as Parameter ---
        ///     
        /// Error handling:
        ///     When faild loading, and error image will be return.
        /// </summary>
        /// <param name="fileEntry"></param>
        /// <returns></returns>
        private Image GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(FileEntry fileEntry)
        {

            Image thumbnailImage;
            try
            {                
                thumbnailImage = databaseAndCacheThumbnail.ReadCache(fileEntry);
             
                if (thumbnailImage == null)
                {
                    //Was not readed from database, need to cache to database
                    thumbnailImage = LoadMediaCoverArtThumbnail(fileEntry.FileFullPath, ThumbnailSaveSize);
                    
                    if (thumbnailImage != null)
                    {
                        Image cloneBitmap = new Bitmap(thumbnailImage); //Need create a clone, due to GDI + not thread safe
                        AddQueueAllUpadtedFileEntry(new FileEntryImage(fileEntry, cloneBitmap));
                        thumbnailImage = Manina.Windows.Forms.Utility.ThumbnailFromImage(thumbnailImage, ThumbnailMaxUpsize, Color.White, true);                        
                    }
                    else
                    {
                        Logger.Warn("Was not able to get thumbnail from file: " + fileEntry.FileFullPath);
                        thumbnailImage = (Image)Properties.Resources.load_image_error_general;
                    }
                }
            }
            catch (IOException ioe)
            {
                Logger.Warn("Load image error, OneDrive issues" + ioe.Message);
                thumbnailImage = (Image)Properties.Resources.load_image_error_onedrive;
            }
            catch (Exception e)
            {
                Logger.Warn("Load image error: " + e.Message);
                thumbnailImage = (Image)Properties.Resources.load_image_error_general;
            }
            return thumbnailImage;
        }

        private bool DataGridViewUpdateThumbnail(DataGridView dataGridView, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex != -1) return false;
            if (e.ColumnIndex < 0) return false;
            if (e.ColumnIndex >= dataGridView.ColumnCount) return false;

            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
            if (dataGridViewGenericColumn == null) return false;

            if (dataGridViewGenericColumn.FileEntryImage.Image == null)
            {
                FileEntry fileEntryReadImageFile;
                if (dataGridViewGenericColumn.Metadata != null && dataGridViewGenericColumn.Metadata.FileEntryBroker != null) 
                    fileEntryReadImageFile = dataGridViewGenericColumn.Metadata.FileEntryBroker; //Read FileEntry from Metadata 
                else 
                    fileEntryReadImageFile = dataGridViewGenericColumn.FileEntryImage; //Read FileEntry From column info

                dataGridViewGenericColumn.FileEntryImage.Image = databaseAndCacheThumbnail.ReadCache(fileEntryReadImageFile);
                //Was not in cache, add to queue for read
                if (dataGridViewGenericColumn.FileEntryImage.Image == null) AddQueueAllUpadtedFileEntry(new FileEntryImage(fileEntryReadImageFile, null));
            }
            if (databaseAndCacheThumbnail.DoesMetadataMissThumbnailInRegion(dataGridViewGenericColumn.Metadata)) AddQueueCreateRegionFromPoster(dataGridViewGenericColumn.Metadata);
            
            return true;
        }


        #endregion

        #region AddQueue - AddQueueMetadataConvertRegion
        public void AddQueueMetadataConvertRegion(List<Metadata> metadataListUpdatesByExiftool)
        {
            foreach (Metadata metadata in metadataListUpdatesByExiftool)
            {
                AddQueueCreateRegionFromPoster(metadata);
                //Update Global data for selected files, and refresh view
                lock (GlobalData.populateSelectedLock) //A PopulateSelectedGrid already in progress, wait untill complete
                {
                    PopulateMetadataOnFileOnActiveDataGrivViewInvoke(metadata.FileFullPath); //Metadata found and updated, updated DataGricView
                }
            }
            StartThreads();
            TriggerAutoResetEventQueueEmpty();
        }
        #endregion

        #region AddQueue - AddQueueThumbnailMedia
        public void AddQueueThumbnailMedia(FileEntryImage fileEntryImage)
        {
            //Need to add to the end, due due read queue read potion [0] end delete after, not thread safe
            lock (commonQueueSaveThumbnailToDatabaseLock)
            {
                if (!commonQueueSaveThumbnailToDatabase.Contains(fileEntryImage)) commonQueueSaveThumbnailToDatabase.Add(fileEntryImage);
                else
                if (fileEntryImage.Image != null)
                {
                    int index = commonQueueSaveThumbnailToDatabase.IndexOf(fileEntryImage);
                    if (index >= 0)
                        commonQueueSaveThumbnailToDatabase[index].Image = fileEntryImage.Image; //Image has been found, updated the entry, so image will not be needed to load again.
                }
            }
        }
        #endregion

        #region AddQueue - AddQueueAllUpadtedFileEntry(FileEntryImage fileEntryImage) - for read missing metadata
        public void AddQueueAllUpadtedFileEntry(FileEntryImage fileEntryImage)
        {
            //When file is DELETE, LastWriteDateTime become null
            if (fileEntryImage.LastWriteDateTime != null)
            {
                if (File.GetLastWriteTime(fileEntryImage.FileFullPath) == fileEntryImage.LastWriteDateTime) //Don't add old files in queue
                {
                    //If Metadata don't exisit in database, put it in read queue
                    Metadata metadata = databaseAndCacheMetadataExiftool.ReadCache(new FileEntryBroker(fileEntryImage, MetadataBrokerTypes.ExifTool));
                    if (metadata == null) AddQueueExiftool(fileEntryImage);

                    metadata = databaseAndCacheMetadataMicrosoftPhotos.ReadCache(new FileEntryBroker(fileEntryImage, MetadataBrokerTypes.MicrosoftPhotos));
                    if (metadata == null) AddQueueMicrosoftPhotos(fileEntryImage);

                    metadata = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadCache(new FileEntryBroker(fileEntryImage, MetadataBrokerTypes.WindowsLivePhotoGallery));
                    if (metadata == null) AddQueueWindowsLivePhotoGallery(fileEntryImage);
                }

                if (databaseAndCacheThumbnail.ReadCache(fileEntryImage) == null) AddQueueThumbnailMedia(fileEntryImage);
            } else
            {
                Debug.WriteLine("AddQueueAllUpadtedFileEntry was delete: (Check why), renmae of exiftool maybe, need back then... " + fileEntryImage.FileFullPath);
            }
            
            StartThreads();
            TriggerAutoResetEventQueueEmpty();
        }
        #endregion

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

        #region Thread - SaveThumbnail
        public void ThreadSaveThumbnail()
        {
            if (_ThreadThumbnailMedia == null || (!_ThreadThumbnailMedia.IsAlive && CommonQueueSaveThumbnailToDatabaseCountLock() > 0))
            {
                _ThreadThumbnailMedia = new Thread(() =>
                {
                    //_ThreadThumbnailMedia.Priority = ThreadPriority.Lowest;

                    while (CommonQueueSaveThumbnailToDatabaseCountLock() > 0 && !GlobalData.IsApplicationClosing) //In case some more added to the queue
                    {
                        try
                        {
                            FileEntryImage fileEntryImage;
                            lock (commonQueueSaveThumbnailToDatabaseLock)
                            {
                                fileEntryImage = new FileEntryImage(commonQueueSaveThumbnailToDatabase[0]);
                            }

                            if (fileEntryImage.Image == null)
                                fileEntryImage.Image = LoadMediaCoverArtThumbnail(fileEntryImage.FileFullPath, ThumbnailSaveSize);

                            if (fileEntryImage.Image != null && !databaseAndCacheThumbnail.DoesThumbnailExist(fileEntryImage))
                            {
                                databaseAndCacheThumbnail.TransactionBeginBatch();
                                databaseAndCacheThumbnail.WriteThumbnail(fileEntryImage, fileEntryImage.Image);
                                databaseAndCacheThumbnail.TransactionCommitBatch();
                                //A PopulateSelectedGrid already in progress, wait untill complete
                                lock (GlobalData.populateSelectedLock) PopulateImageOnFileEntryOnSelectedGrivViewInvoke(fileEntryImage);
                            }
                            else
                            {
                                //DEBUG, Manage to reproduce when select lot files and run log AutoCorrect Updates, Refresh
                            }

                            lock (commonQueueSaveThumbnailToDatabaseLock)
                            {
                                if (commonQueueSaveThumbnailToDatabase.Count > 0) commonQueueSaveThumbnailToDatabase.RemoveAt(0);
                            }
                        } catch (Exception ex)
                        {
                            Logger.Error("ThreadSaveThumbnail: " + ex.Message);
                        }
                        UpdateStatusAllQueueStatus();
                    }
                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });

                _ThreadThumbnailMedia.Start();
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
            if (_ThreadThumbnailRegion == null || (!_ThreadThumbnailRegion.IsAlive && CommonQueueReadPosterAndSaveFaceThumbnailsCountLock() > 0))
            {
                if (IsThreadRunningExcept_ThreadThumbnailRegion()) 
                    return; //Wait other thread to finnish first. Otherwise it will generate high load on disk use

                _ThreadThumbnailRegion = new Thread(() =>
                {                    
                    while (CommonQueueReadPosterAndSaveFaceThumbnailsCountLock() > 0 && !GlobalData.IsApplicationClosing && !IsThreadRunningExcept_ThreadThumbnailRegion()) //In case some more added to the queue
                    {
                        int queueCount = CommonQueueReadPosterAndSaveFaceThumbnailsCountLock(); //Mark count that we will work with. 

                        try
                        {
                            Image image = null;
                            FileEntry fileEntry;
                            lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                            {
                                fileEntry = new FileEntry(commonQueueReadPosterAndSaveFaceThumbnails[0].FileEntryBroker);                                
                                if (File.GetLastWriteTime(fileEntry.FileFullPath) == fileEntry.LastWriteDateTime) image = LoadMediaCoverArtPoster(fileEntry.FileFullPath);
                            }

                            if (image != null) databaseAndCacheMetadataExiftool.TransactionBeginBatch();
                            bool foundFile = false;
                            do //Remove all with same filename in the queue
                            {
                                foundFile = false;
                                //Check Exiftool, Microsoft Phontos, Windows Live Photo Gallery in queue also

                                lock (commonQueueReadPosterAndSaveFaceThumbnailsLock)
                                {
                                    for (int thumbnailIndex = 0; thumbnailIndex < Math.Min(queueCount, commonQueueReadPosterAndSaveFaceThumbnails.Count); thumbnailIndex++)
                                    {

                                        if (commonQueueReadPosterAndSaveFaceThumbnails[thumbnailIndex].FileFullPath == fileEntry.FileFullPath &&
                                            commonQueueReadPosterAndSaveFaceThumbnails[thumbnailIndex].FileDateModified == fileEntry.LastWriteDateTime)
                                        {
                                            if (image != null) //Failed load cover art, often occur after filed is moved or deleted
                                            {
                                                //Metadata found and updated, updated DataGricView                                             
                                                RegionThumbnailHandler.SaveThumbnailsForRegioList(databaseAndCacheMetadataExiftool, commonQueueReadPosterAndSaveFaceThumbnails[thumbnailIndex], image);
                                                foundFile = true;

                                            }
                                            else Logger.Error("ThreadReadMediaPosterSaveRegions failt to create 'face' region thumbails from file. Due to not exist anymore. File:" + commonQueueReadPosterAndSaveFaceThumbnails[0].FileName);

                                            queueCount--;
                                            commonQueueReadPosterAndSaveFaceThumbnails.RemoveAt(thumbnailIndex);
                                            if (foundFile) break; //No need to search more.
                                        }
                                    }
                                }
                                
                            } while (foundFile);


                            if (image != null)
                            {
                                databaseAndCacheMetadataExiftool.TransactionCommitBatch();
                                PopulateMetadataOnFileOnActiveDataGrivViewInvoke(fileEntry.FileFullPath); //Updated Gridview
                            }
                            image = null;

                        }

                        catch (Exception e)
                        {
                            Logger.Error("ThreadReadMediaPosterSaveRegions failt to create 'face' region thumbails" + e.Message);
                        }

                        UpdateStatusAllQueueStatus();
                    }
                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });

                _ThreadThumbnailRegion.Start();
            }

        }
        #endregion
       
        #region Thread - EmptyQueue(MetadataBrokerTypes broker, MetadataDatabaseCache database, ImetadataReader reader, List<FileEntry> metadataReadQueue)
        private void EmptyQueue(MetadataBrokerTypes broker, MetadataDatabaseCache database,
            ImetadataReader reader, List<FileEntry> metadataReadQueue)
        {
            //if (reader != null) //database reader becomes null when not open, e.g. due to database was locked when starting the app 
            //or application (Windows Live Photo Galelery or Microsot Photos) was not installed
            int metadataReadQueueCount;
            metadataReadQueueCount = metadataReadQueue.Count;

            while (metadataReadQueueCount > 0 && !GlobalData.IsApplicationClosing && reader != null) //In case some more added to the queue
            {
                FileEntry fileEntry;
                if (broker == MetadataBrokerTypes.MicrosoftPhotos) lock (commonQueueReadMetadataFromMicrosoftPhotosLock) fileEntry = new FileEntry(metadataReadQueue[0]);
                else lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) fileEntry = new FileEntry(metadataReadQueue[0]);
                Metadata metadata;

                if (File.Exists(fileEntry.FileFullPath))
                {
                    metadata = reader.Read(broker, fileEntry.FileFullPath); //Read from broker as Microsoft Photos, Windows Live Photo Gallery (Using NamedPipes)
                    if (metadata != null) // && broker != MetadataBrokerTypes.WindowsLivePhotoGallery)
                    {
                        //Windows Live Photo Gallery writes direclty to database from sepearte thread when found
                        database.TransactionBeginBatch();
                        database.Write(metadata);
                        AddQueueCreateRegionFromPoster(metadata);
                        database.TransactionCommitBatch();

                        //A PopulateSelectedGrid already in progress, wait untill complete, Metadata found and updated, updated DataGricView
                        lock (GlobalData.populateSelectedLock) PopulateMetadataOnFileOnActiveDataGrivViewInvoke(metadata.FileFullPath);                         
                    } /*else
                    {
                        metadata = new Metadata(broker);
                        metadata.FileEntryBroker.Broker = broker;
                        metadata.FileName = metadataReadQueue[indexWithImage].FileName;
                        metadata.FileDirectory = metadataReadQueue[indexWithImage].Directory;
                        metadata.FileDateModified = metadataReadQueue[indexWithImage].LastWriteDateTime;
                        database.TransactionBeginBatch();
                        database.Write(metadata);
                        database.TransactionCommitBatch();
                    }*/
                } 
                else Logger.Warn("File don't exsist anymore: " + fileEntry.FileFullPath);

                if (broker == MetadataBrokerTypes.MicrosoftPhotos) lock (commonQueueReadMetadataFromMicrosoftPhotosLock) metadataReadQueue.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar
                else lock (commonQueueReadMetadataFromWindowsLivePhotoGalleryLock) metadataReadQueue.RemoveAt(0); //Remove from queue after read. Otherwise wrong text in status bar

                UpdateStatusAllQueueStatus();                
                metadataReadQueueCount = metadataReadQueue.Count;
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
                        EmptyQueue(MetadataBrokerTypes.WindowsLivePhotoGallery, databaseAndCacheMetadataWindowsLivePhotoGallery, databaseWindowsLivePhotGallery, commonQueueReadMetadataFromWindowsLivePhotoGallery);
                        UpdateStatusAllQueueStatus();
                    }
                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });

                _ThreadWindowsLiveGallery.Start();

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
                        EmptyQueue(MetadataBrokerTypes.MicrosoftPhotos, databaseAndCacheMetadataMicrosoftPhotos, databaseWindowsPhotos, commonQueueReadMetadataFromMicrosoftPhotos);                        
                        UpdateStatusAllQueueStatus();
                    }
                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });

                _ThreadMicrosoftPhotos.Start();
            }
        }
        #endregion

        #region AddQueue - AddQueueSaveMetadataUpdatedByUser
        public void AddQueueSaveMetadataUpdatedByUser(Metadata metadataToSave, Metadata metadataOriginal)
        {
            lock (commonQueueSaveMetadataUpdatedByUserLock) commonQueueSaveMetadataUpdatedByUser.Add(metadataToSave);
            lock (commonOrigialMetadataBeforeUserUpdateLock) commonOrigialMetadataBeforeUserUpdate.Add(metadataOriginal);
            UpdateStatusAllQueueStatus();   
        }
        #endregion

        #region AddQueue - AddQueueVerifyMetadata(Metadata metadataToVerify)
        public void AddQueueVerifyMetadata(Metadata metadataToVerifyAfterSavedByExiftool)
        {            
            lock (commonQueueMetadataWrittenByExiftoolReadyToVerifyLock) commonQueueMetadataWrittenByExiftoolReadyToVerify.Add(metadataToVerifyAfterSavedByExiftool);
            UpdateStatusAllQueueStatus();
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
                    string writeMetadataTags = Properties.Settings.Default.WriteMetadataTags;
                    string writeMetadataKeywordDelete = Properties.Settings.Default.WriteMetadataKeywordDelete;
                    string writeMetadataKeywordAdd = Properties.Settings.Default.WriteMetadataKeywordAdd;

                    string writeXtraAtomAlbum = Properties.Settings.Default.XtraAtomAlbumVariable;
                    bool writeXtraAtomAlbumVideo = Properties.Settings.Default.XtraAtomAlbumVideo;

                    string writeXtraAtomCategories = Properties.Settings.Default.XtraAtomCategoriesVariable;
                    bool writeXtraAtomCategoriesVideo = Properties.Settings.Default.XtraAtomCategoriesVideo;

                    string writeXtraAtomComment = Properties.Settings.Default.XtraAtomCommentVariable;
                    bool writeXtraAtomCommentPicture = Properties.Settings.Default.XtraAtomCommentPicture;
                    bool writeXtraAtomCommentVideo = Properties.Settings.Default.XtraAtomCommentVideo;

                    string writeXtraAtomKeywords = Properties.Settings.Default.XtraAtomKeywordsVariable;
                    bool writeXtraAtomKeywordsVideo = Properties.Settings.Default.XtraAtomKeywordsVideo;

                    bool writeXtraAtomRatingPicture = Properties.Settings.Default.XtraAtomRatingPicture;
                    bool writeXtraAtomRatingVideo = Properties.Settings.Default.XtraAtomRatingVideo;

                    string writeXtraAtomSubject = Properties.Settings.Default.XtraAtomSubjectVariable;
                    bool writeXtraAtomSubjectPicture = Properties.Settings.Default.XtraAtomSubjectPicture;
                    bool wtraAtomSubjectVideo = Properties.Settings.Default.XtraAtomSubjectVideo;

                    string writeXtraAtomSubtitle = Properties.Settings.Default.XtraAtomSubtitleVariable;
                    bool writeXtraAtomSubtitleVideo = Properties.Settings.Default.XtraAtomSubtitleVideo;

                    string writeXtraAtomArtist = Properties.Settings.Default.XtraAtomArtistVariable;
                    bool writeXtraAtomArtistVideo = Properties.Settings.Default.XtraAtomArtistVideo;

                    List<string> allowedFileNameDateTimeFormats = FileDateTime.FileDateTimeReader.ConvertStringOfDatesToList(Properties.Settings.Default.RenameDateFormats);
                    #endregion 

                    while (CommonQueueSaveMetadataUpdatedByUserCountLock() > 0) // && !GlobalData.IsApplicationClosing)
                    {
                        ShowExiftoolSaveProgressClear();

                        int writeCount = CommonQueueSaveMetadataUpdatedByUserCountLock();
                        List<Metadata> queueSubsetSaveMetadata = new List<Metadata>();    //This new values for saving (changes done by user)
                        List<Metadata> queueSubsetOrginalMetadata = new List<Metadata>(); //Before updated by user, need this to check if any updates
                        List<Metadata> metadataUpdated = new List<Metadata>();
                        #region Creat a subset queue for writing
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

                            //If wait to be read, read first, write after, it's not the fastest logic but safer
    lock (commonQueueReadMetadataFromExiftoolLock)
    {
        if (commonQueueReadMetadataFromExiftool.Contains(metadataWrite.FileEntryBroker) ||
            mediaFilesNotInDatabase.Contains(metadataWrite.FileEntryBroker.FileFullPath))
        {
            lock (commonQueueSaveMetadataUpdatedByUserLock) commonQueueSaveMetadataUpdatedByUser.Add(metadataWrite);
            lock (commonOrigialMetadataBeforeUserUpdateLock) commonOrigialMetadataBeforeUserUpdate.Add(metadataOrginal);
        }
        else
        {
            if (!GlobalData.IsApplicationClosing)
            {
                AddWatcherShowExiftoolSaveProcessQueue(metadataWrite.FileEntryBroker.FileFullPath);
                queueSubsetSaveMetadata.Add(metadataWrite);
                queueSubsetOrginalMetadata.Add(metadataOrginal);
            }
            else Logger.Warn("Was not able to save all data before closing the application");
        }
    }
                        }
                        #endregion

                        if (!GlobalData.IsApplicationClosing) ExiftoolWriter.WaitLockedFilesToBecomeUnlocked(metadataUpdated); //E.g. some application writing to file, or OneDrive doing backup

                        #region Save Metadatas using Exiftool
                        bool didExiftoolCrash = false;
                        string exiftoolErrorMessage = "";
                        if (!GlobalData.IsApplicationClosing)
                        {
                            try
                            {
                                UpdateStatusAction("Batch update a subset of " + queueSubsetSaveMetadata.Count + " media files...");
                                metadataUpdated = ExiftoolWriter.WriteMetadata(queueSubsetSaveMetadata, queueSubsetOrginalMetadata,
                                allowedFileNameDateTimeFormats, writeMetadataTags, writeMetadataKeywordDelete, writeMetadataKeywordAdd,
                                writeXtraAtomAlbum, writeXtraAtomAlbumVideo,
                                writeXtraAtomCategories, writeXtraAtomCategoriesVideo,
                                writeXtraAtomComment, writeXtraAtomCommentPicture, writeXtraAtomCommentVideo,
                                writeXtraAtomKeywords, writeXtraAtomKeywordsVideo,
                                writeXtraAtomRatingPicture, writeXtraAtomRatingVideo,
                                writeXtraAtomSubject, writeXtraAtomSubjectPicture, wtraAtomSubjectVideo,
                                writeXtraAtomSubtitle, writeXtraAtomSubtitleVideo,
                                writeXtraAtomArtist, writeXtraAtomArtistVideo);                                
                            } catch (Exception ex) 
                            {
                                didExiftoolCrash = true;
                                exiftoolErrorMessage = ex.Message;
                                Logger.Error("EXIFTOOL.EXE error...\r\n\r\n" + ex.Message);
                            }
                        }
                        #endregion 

                        #region Check if all files was updated, if updated, add to verify queue
                        if (!GlobalData.IsApplicationClosing)
                        {
                            foreach (Metadata metadataWritten in queueSubsetSaveMetadata)
                            {
                                try
                                {
                                    DateTime dateTimeLastWriteTime = File.GetLastWriteTime(metadataWritten.FileFullPath);

                                    if (didExiftoolCrash || metadataWritten.FileDateModified == dateTimeLastWriteTime)
                                    {
                                        Metadata metadataError = new Metadata(metadataWritten);
                                        metadataError.FileDateModified = metadataWritten.FileDateModified;
                                        metadataError.Broker |= MetadataBrokerTypes.ExifToolWriteError;
                                        databaseAndCacheMetadataExiftool.TransactionBeginBatch();
                                        databaseAndCacheMetadataExiftool.Write(metadataError);
                                        databaseAndCacheMetadataExiftool.TransactionCommitBatch();

                                        AddError(metadataWritten.FileEntryBroker.Directory, metadataWritten.FileEntryBroker.FileName, metadataWritten.FileEntryBroker.LastWriteDateTime,
                                                AddErrorExiftooRegion, AddErrorExiftooCommandWrite, AddErrorExiftooParameterWrite, AddErrorExiftooParameterWrite,
                                                "EXIFTOOL.EXE failed write to file.\r\n" + metadataWritten.FileEntryBroker.FileFullPath + "\r\nMessage return from Exiftool:" + exiftoolErrorMessage);
                                    }
                                    else
                                    {
                                        metadataWritten.FileDateModified = dateTimeLastWriteTime;
                                        AddQueueVerifyMetadata(metadataWritten);
                                    }
                                }
                                catch { }
                            }
                        }
                        #endregion

                        if (!GlobalData.IsApplicationClosing) ExiftoolWriter.WaitLockedFilesToBecomeUnlocked(metadataUpdated); //E.g. OneDrive doing backup
                        
                        //Files are updated, need updated ImageListeView with new metadata, filesize, lastwritedate, etc...
                        UpdateThumbnailOnImageListViewItems(imageListView1, metadataUpdated);
                        UpdateMetadataOnSelectedFilesOnActiveDataGrivView(imageListView1.SelectedItems);

                        //Clean up
                        queueSubsetSaveMetadata.Clear();
                        queueSubsetOrginalMetadata.Clear();
                        metadataUpdated.Clear();

                        //Status updated for user
                        ShowExiftoolSaveProgressStop();
                        UpdateStatusAllQueueStatus();

                        Thread.Sleep(100); //Wait in case of loop
                    }
                });
                
                
                _ThreadSaveMetadata.Start();

            }
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

                        int rangeToRemove; //Remember how many in queue now

                        #region From the Write Queue - Create a List of All files that not already in database
                        lock (commonQueueReadMetadataFromExiftool)
                        {
                            rangeToRemove = Math.Min(commonQueueReadMetadataFromExiftool.Count, 30);
                            mediaFilesNotInDatabase.AddRange(databaseAndCacheMetadataExiftool.ListAllMissingFileEntries(MetadataBrokerTypes.ExifTool, commonQueueReadMetadataFromExiftool.GetRange(0, rangeToRemove)));
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

                            List<Metadata> metadataReadbackExiftoolAfterSaved = exiftoolReader.Read(MetadataBrokerTypes.ExifTool, useExiftoolOnThisSubsetOfFiles);

                            #region Verify in all files are read
                            if (useExiftoolOnThisSubsetOfFiles.Count != metadataReadbackExiftoolAfterSaved.Count)
                            {
                                string filesNotRead = "";
                                foreach (string fullFilePath in useExiftoolOnThisSubsetOfFiles)
                                {
                                    AddError(Path.GetDirectoryName(fullFilePath), Path.GetFileName(fullFilePath), DateTime.Now,
                                        AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                        AddErrorExiftooRegion, AddErrorExiftooCommandRead, AddErrorExiftooParameterRead,
                                        "The file was not read by exiftool", false);
                                    if (!Metadata.IsFullFilenameInList(metadataReadbackExiftoolAfterSaved, fullFilePath)) filesNotRead += filesNotRead + "; ";
                                }
                                Logger.Error("Exiftool fail with read all files. Files not read: " + filesNotRead);
                            }
                            #endregion 

                            AddQueueMetadataConvertRegion(metadataReadbackExiftoolAfterSaved);

                            //Verify                             
                            foreach (Metadata metadataRead in metadataReadbackExiftoolAfterSaved)
                            {
                                lock (commonQueueMetadataWrittenByExiftoolReadyToVerifyLock)
                                {
                                    if (ExiftoolWriter.HasWriteMetadataErrors(metadataRead,
                                    commonQueueMetadataWrittenByExiftoolReadyToVerify,
                                    out Metadata metadataUpdatedByUserCopy, out string writeErrorDesciption))
                                    {
                                        AddError(metadataUpdatedByUserCopy.FileEntryBroker.Directory, metadataUpdatedByUserCopy.FileEntryBroker.FileName, metadataUpdatedByUserCopy.FileEntryBroker.LastWriteDateTime,
                                            AddErrorExiftooRegion, AddErrorExiftooCommandVerify, AddErrorExiftooParameterVerify, AddErrorExiftooParameterVerify, writeErrorDesciption);

                                        Metadata metadataError = new Metadata(metadataUpdatedByUserCopy);
                                        metadataError.FileDateModified = DateTime.Now;
                                        metadataError.Broker |= MetadataBrokerTypes.ExifToolWriteError;
                                        databaseAndCacheMetadataExiftool.TransactionBeginBatch();
                                        databaseAndCacheMetadataExiftool.Write(metadataError);
                                        databaseAndCacheMetadataExiftool.TransactionCommitBatch();
                                    }
                                }
                            }


                            UpdateMetadataOnSelectedFilesOnActiveDataGrivView(imageListView1.SelectedItems);
                            RefreshImageOnSelectFilesOnActiveDataGrivView(imageListView1.SelectedItems);

                            mediaFilesNotInDatabase.RemoveRange(0, range); //Remove subset from queue before update status bar

                            UpdateStatusAllQueueStatus();
                        }
                    }

                    exiftoolReader.MetadataGroupPrioityWrite(); //Updated json config file if new tags found
                    StartThreads();
                    TriggerAutoResetEventQueueEmpty();
                });

                _ThreadExiftool.Start();
            }
        }
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
            } catch { }

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
            timerShowErrorMessage.Start();
        }
        #endregion

        #region ImageListView Invoke - UpdateItem
        private void ImageListViewSuspendLayoutInvoke(ImageListView imageListView)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListView>(ImageListViewSuspendLayoutInvoke), imageListView);
                return;
            }

            //imageListView.SuspendLayout(); //When this where, it crash, need debug why, this needed to avoid flashing
        }

        private void ImageListViewResumeLayoutInvoke(ImageListView imageListView)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListView>(ImageListViewResumeLayoutInvoke), imageListView);
                return;
            }

            //imageListView.ResumeLayout(); //When this where, it crash, need debug why, this needed to avoid flashing
        }

        private void ImageListViewUpdateItemThumbnailAndMetadataInvoke(ImageListViewItem item)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListViewItem>(ImageListViewUpdateItemThumbnailAndMetadataInvoke), item);
                return;
            }
            item.Update();
            item.Selected = true;
        }
        #endregion 

    }
}
