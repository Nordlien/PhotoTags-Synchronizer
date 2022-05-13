using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Manina.Windows.Forms;
using MetadataLibrary;
using System.Threading;
using ApplicationAssociations;
using System.Collections.Generic;
using FileHandeling;
using Krypton.Toolkit;
using ImageAndMovieFileExtentions;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        private AutoResetEvent ReadImageOutOfMemoryWillWaitCacheEmpty = null; //When out of memory, then wait for all data ready = new AutoResetEvent(false);

        #region ImageListView - Item updates

        #region ImageListView - Update Thumbnail - UpdateAll - Invoke (ImageListViewItem.Update)
        private void ImageListView_UpdateItemThumbnailUpdateAllInvoke(FileEntry fileEntry)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntry>(ImageListView_UpdateItemThumbnailUpdateAllInvoke), fileEntry);
                return;
            }

            FileStatus fileStatus = FileHandler.GetFileStatus(fileEntry.FileFullPath);
            if (fileStatus.FileExists)
            {
                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntry.FileFullPath, fileStatus.LastWrittenDateTime, FileEntryVersion.CurrentVersionInDatabase);
                AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttribute);
            }

            ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fileEntry.FileFullPath);
            if (foundItem != null) foundItem.Update();   
        }
        #endregion

        #region ImageListView - Update Thumbnail - UpdateAll - Only when need - Invoke (ImageListViewItem.Update)
        private void ImageListView_UpdateItemThumbnailUpdateAll_OnlyIfFileStatusHasChangedInvoke(FileEntry fileEntry, FileStatus fileStatus)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntry, FileStatus>(ImageListView_UpdateItemThumbnailUpdateAll_OnlyIfFileStatusHasChangedInvoke), fileEntry, fileStatus);
                return;
            }

            ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fileEntry.FileFullPath);
            if (foundItem != null)
            {
                if (foundItem.FileStatus.IsInCloud != fileStatus.IsInCloud ||
                    foundItem.FileStatus.LastWrittenDateTime != fileStatus.LastWrittenDateTime) 
                    foundItem.Update();
            } 
        }
        #endregion

        #region ImageListView - Update FileStatus - Invoke
        private void ImageListView_UpdateItemFileStatusInvoke(string fullFileName, FileStatus fileStatus)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string, FileStatus>(ImageListView_UpdateItemFileStatusInvoke), fullFileName, fileStatus);
                return;
            }
            
            try
            {
                ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fullFileName);
                if (foundItem != null)
                {
                    foundItem.FileStatus = fileStatus;
                    foundItem.Invalidate();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion      

        #region ImageListView - Update Exiftool Metadata - Invoke
        private void ImageListView_UpdateItemExiftoolMetadataInvoke(FileEntryAttribute fileEntryAttribute, FileStatus fileStatus)
        {   
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute, FileStatus>(ImageListView_UpdateItemExiftoolMetadataInvoke), fileEntryAttribute, fileStatus);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(
                        new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.ExifTool));

                if (metadata != null)
                {
                    PopulateTreeViewFolderFilterAdd(metadata);

                    ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fileEntryAttribute.FileFullPath);
                    if (foundItem != null)
                    {
                        Utility.ShellImageFileInfo fileMetadata = new Utility.ShellImageFileInfo();
                        ConvertMetadataToShellImageFileInfo(ref fileMetadata, metadata);

                        if (fileStatus == null)
                        {
                            fileStatus = FileHandler.GetFileStatus(fileEntryAttribute.FileFullPath,
                            exiftoolProcessStatus: ExiftoolProcessStatus.WaitAction, //Metadata is found
                            checkLockedStatus: true);
                        }

                        if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.Error)
                            fileMetadata.FileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError; //Error Metadata found

                        foundItem.UpdateDetails(fileMetadata);
                        KeepTrackOfMetadataLoadedRemoveFromList(fileEntryAttribute.FileFullPath);
                    }
                }
                else
                {
                    //DEBUG - FIle is become deleted
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region ImageListView - MetadataLoadedRemoveFromList - Trigger Action when all metadata Loaded
        private HashSet<string> keepTrackOfLoadedMetadata = new HashSet<string>();
        private readonly object keepTrackOfLoadedMetadataLock = new object();
        private bool hasTriggerLoadAllMetadataActions = false;

        private void KeepTrackOfMetadataLoadedClearList()
        {
            lock (keepTrackOfLoadedMetadataLock)
            {
                hasTriggerLoadAllMetadataActions = false;
                keepTrackOfLoadedMetadata = new HashSet<string>();
            }
        }

        private Thread threadFilter = null;
        private void KeepTrackOfMetadataLoadedRemoveFromList(string fullFileName)
        {
            if (!hasTriggerLoadAllMetadataActions)
            {
                bool isAllMetadataLoaded;
                lock (keepTrackOfLoadedMetadataLock)
                {
                    if (keepTrackOfLoadedMetadata.Contains(fullFileName)) keepTrackOfLoadedMetadata.Remove(fullFileName);
                    isAllMetadataLoaded = keepTrackOfLoadedMetadata.Count == 0;
                }
                if (isAllMetadataLoaded)
                {
                    hasTriggerLoadAllMetadataActions = true;
                    ImageListViewSortByCheckedRadioButton(false);

                    #region PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke
                    try
                    {
                        FilterVerifyer.StopPopulate = true;
                        if (threadFilter != null)
                        {
                            while (threadFilter.IsAlive) Application.DoEvents();
                        }
                        FilterVerifyer.StopPopulate = false;
                    }
                    catch { }

                    threadFilter = new Thread(() =>
                    {
                        try
                        {
                            PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke();
                        }
                        catch { }
                        finally
                        {
                            threadFilter = null;
                        }
                    }
                    );
                    threadFilter.Start();
                    #endregion
                }
            }
        }
        #endregion

        #endregion

        #region ImageListView - Events

        #region ConvertMetadataToShellImageFileInfo
        private void ConvertMetadataToShellImageFileInfo(ref Utility.ShellImageFileInfo fileMetadata, Metadata metadata)
        {
            #region Provided by FileInfo
            fileMetadata.FileDateCreated = (DateTime)metadata.FileDateCreated;
            fileMetadata.FileDateModified = (DateTime)metadata.FileDateModified;

            #region SmartDate
            DateTime? fileSmartDate = fileDateTimeReader.SmartDateTime(metadata.FileFullPath, fileMetadata.FileDateCreated, fileMetadata.FileDateModified);
            fileMetadata.FileSmartDate = (fileSmartDate == null ? DateTime.MinValue : (DateTime)fileSmartDate);
            #endregion

            #region FileSize
            if (metadata.FileSize != null) fileMetadata.FileSize = (long)metadata.FileSize;
            else
            {
                try
                {
                    if (File.Exists(metadata.FileFullPath)) fileMetadata.FileSize = new FileInfo(metadata.FileFullPath).Length;
                }
                catch
                {
                    fileMetadata.FileSize = long.MinValue;
                }
            }
            #endregion

            fileMetadata.FileMimeType = metadata.FileMimeType;
            fileMetadata.FileDirectory = metadata.FileDirectory;
            #endregion

            #region Provided by ShellImageFileInfo, MagickImage                                
            fileMetadata.CameraMake = metadata.CameraMake;
            fileMetadata.CameraModel = metadata.CameraModel;
            if (metadata.MediaWidth != null && metadata.MediaHeight != null) fileMetadata.MediaDimensions = new Size((int)metadata.MediaWidth, (int)metadata.MediaHeight);
            else fileMetadata.MediaDimensions = new Size(0, 0);
            #endregion

            #region Provided by MagickImage, Exiftool
            if (metadata.MediaDateTaken != null) fileMetadata.MediaDateTaken = (DateTime)metadata.MediaDateTaken;
            else fileMetadata.MediaDateTaken = DateTime.MinValue;
            fileMetadata.MediaTitle = metadata.PersonalTitle;
            fileMetadata.MediaDescription = metadata.PersonalDescription;
            fileMetadata.MediaComment = metadata.PersonalComments;
            fileMetadata.MediaAuthor = metadata.PersonalAuthor;
            fileMetadata.MediaRating = (byte)(metadata.PersonalRating == null ? 0 : metadata.PersonalRating);
            #endregion

            #region Provided by Exiftool
            fileMetadata.MediaAlbum = metadata.PersonalAlbum;
            if (metadata.LocationDateTime != null) fileMetadata.LocationDateTime = (DateTime)metadata.LocationDateTime;
            else fileMetadata.LocationDateTime = DateTime.MinValue;
            fileMetadata.LocationTimeZone = metadata.LocationTimeZoneDescription;
            fileMetadata.LocationName = metadata.LocationName;
            fileMetadata.LocationRegionState = metadata.LocationState;
            fileMetadata.LocationCity = metadata.LocationCity;
            fileMetadata.LocationCountry = metadata.LocationCountry;
            #endregion
        }

        #endregion 

        #region ImageListView - Event - Retrieve Metadata
        private void imageListView1_RetrieveItemMetadataDetails(object sender, RetrieveItemMetadataDetailsEventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (GlobalData.DoNotTrigger_ImageListView_ItemUpdate) return;
            if (imageListView1.IsDisposed) return;

            //PS. Note: Make sure that the RetrieveItemMetadataDetails don't go in endless loop. Read data, flags as still dirty, read again, etc..
            try
            {
                FileStatus fileStatus = FileHandler.GetFileStatus(e.FileName, exiftoolProcessStatus: ExiftoolProcessStatus.WaitAction);
                FileEntryBroker fileEntryBroker = new FileEntryBroker(e.FileName, fileStatus.LastWrittenDateTime, MetadataBrokerType.ExifTool);
                
                #region Assign Metadata - If file exist
                Metadata metadata = null;
                if (fileStatus.FileExists) metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBroker);
                #endregion

                if (metadata == null || metadata.FileName == null)
                {
                    #region Init data
                    e.FileMetadata = new Utility.ShellImageFileInfo(); //Tell that data is create, all is good for internal void UpdateDetailsInternal(Utility.ShellImageFileInfo info)
                    e.FileMetadata.SetPropertyStatusOnAll(PropertyStatus.Requested); //All data will be read, it's in Lazy loading queue
                    #endregion

                    #region Error message
                    string inCloudOrNotExistError = FileHandler.ConvertFileStatusToText(fileStatus);
                    if (string.IsNullOrWhiteSpace(inCloudOrNotExistError)) inCloudOrNotExistError = "Info Requested";
                    #endregion

                    #region Assign metadata (what exist, mostly emty)

                    #region Provided by FileInfo
                    e.FileMetadata.FileDateCreated = DateTime.MinValue;
                    e.FileMetadata.FileDateModified = DateTime.MinValue;
                    e.FileMetadata.FileSmartDate = DateTime.MinValue;
                    e.FileMetadata.FileSize = long.MinValue;

                    if (fileStatus.FileExists)
                    {
                        try { e.FileMetadata.FileDateCreated = FileHandler.GetCreationTime(e.FileName); } catch { }
                        try { e.FileMetadata.FileDateModified = fileStatus.LastWrittenDateTime; } catch { }
                        try
                        {
                            DateTime? fileSmartDate = fileDateTimeReader.SmartDateTime(e.FileName, e.FileMetadata.FileDateCreated, e.FileMetadata.FileDateModified);
                            e.FileMetadata.FileSmartDate = (fileSmartDate == null ? DateTime.MinValue : (DateTime)fileSmartDate);
                        }
                        catch { }
                        try { e.FileMetadata.FileSize = new FileInfo(e.FileName).Length; } catch { }
                    }

                    e.FileMetadata.FileMimeType = Path.GetExtension(e.FileName);
                    e.FileMetadata.FileDirectory = Path.GetDirectoryName(e.FileName);
                    
                    e.FileMetadata.DisplayName = Path.GetFileName(e.FileName);
                    //e.FileMetadata.Name= e.FileName;
                    e.FileMetadata.Extension = Path.GetExtension(e.FileName);
                    e.FileMetadata.FileAttributes = FileAttributes.Normal;
                    #endregion

                    #region Empty - Normally Provided by ShellImageFileInfo, MagickImage                             
                    e.FileMetadata.CameraMake = inCloudOrNotExistError;
                    e.FileMetadata.CameraModel = inCloudOrNotExistError;
                    e.FileMetadata.MediaDimensions = new Size(0, 0);
                    #endregion

                    #region Empty - Normally Provided by MagickImage, Exiftool
                    e.FileMetadata.MediaDateTaken = DateTime.MinValue;
                    e.FileMetadata.MediaTitle = inCloudOrNotExistError;
                    e.FileMetadata.MediaDescription = inCloudOrNotExistError;
                    e.FileMetadata.MediaComment = inCloudOrNotExistError;
                    e.FileMetadata.MediaAuthor = inCloudOrNotExistError;
                    e.FileMetadata.MediaRating = 0;
                    #endregion

                    #region Empty - Normally Provided by Exiftool
                    e.FileMetadata.MediaAlbum = inCloudOrNotExistError;
                    e.FileMetadata.LocationDateTime = DateTime.MinValue;
                    e.FileMetadata.LocationTimeZone = inCloudOrNotExistError;
                    e.FileMetadata.LocationName = inCloudOrNotExistError;
                    e.FileMetadata.LocationRegionState = inCloudOrNotExistError;
                    e.FileMetadata.LocationCity = inCloudOrNotExistError;
                    e.FileMetadata.LocationCountry = inCloudOrNotExistError;
                    #endregion

                    #endregion

                    FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntryBroker, FileEntryVersion.CurrentVersionInDatabase);
                    FileEntryBroker fileEntryBrokerError = new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError);

                    #region ReadMetadata (BrokerError) Check if has Record with Error - flag it with FileInaccessibleOrError
                    Metadata metadataError = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBrokerError);
                    if (metadataError != null)
                    {
                        fileStatus.HasErrorOccured = true;
                        fileStatus.FileErrorMessage = "Error record in database";
                        fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.FileInaccessibleOrError;
                    }
                    #endregion

                    #region Add to read queue, when data missing and not marked as Error record
                    if (metadataError == null && fileStatus.FileExists)
                    {
                        AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(fileEntryAttribute); //JTN: If Check ImmageListViewItems attributes, will create Loop
                    }
                    #endregion
                }
                else
                {
                    #region Return Metadata found
                    PopulateTreeViewFolderFilterAdd(metadata);
                    KeepTrackOfMetadataLoadedRemoveFromList(metadata.FileFullPath);
                    Utility.ShellImageFileInfo fileMetadata = new Utility.ShellImageFileInfo();
                    ConvertMetadataToShellImageFileInfo(ref fileMetadata, metadata);
                    e.FileMetadata = fileMetadata;
                    fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitAction;
                    #endregion
                }
                e.FileMetadata.FileStatus = fileStatus;

            }
            catch (Exception ex)
            {
                #region Exception Handling
                Logger.Error(ex, "imageListView1_RetrieveItemMetadataDetails");
                if (e.FileMetadata == null) e.FileMetadata = new Utility.ShellImageFileInfo();
                e.FileMetadata.DisplayName = Path.GetFileName(e.FileName);
                e.FileMetadata.FileDirectory = Path.GetDirectoryName(e.FileName);

                if (!string.IsNullOrWhiteSpace(e.FileName))
                {
                    FileStatus fileStatus = FileHandler.GetFileStatus(
                        e.FileName, checkLockedStatus: true,
                        hasErrorOccured: true, errorMessage: ex.Message,
                        exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError);
                    e.FileMetadata.FileStatus = fileStatus;
                    e.FileMetadata.SetPropertyStatusOnAll(PropertyStatus.IsSet);
                }
                #endregion
            }
        }
        #endregion

        #region ImageListView - Event - Retrieve Thumbnail 
        /// <summary>
        /// Occures when ImageListView need to "load" thumbnail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageListView1_RetrieveItemThumbnail(object sender, RetrieveItemThumbnailEventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (GlobalData.DoNotTrigger_ImageListView_ItemUpdate) return;
            if (imageListView1.IsDisposed) return;

            try
            {
                FileStatus fileStatus = FileHandler.GetFileStatus(e.FileName);
                if (fileStatus.FileExists)
                {
                    FileEntry fileEntry = new FileEntry(e.FileName, fileStatus.LastWrittenDateTime); //Get last Write Time of Media file

                    bool dontReadFileFromCloud = Properties.Settings.Default.AvoidOfflineMediaFiles;
                    try
                    {
                        Image thumbnail = GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(fileEntry, dontReadFileFromCloud, fileStatus);

                        if (thumbnail != null) 
                        {
                            Image thumbnailWithCloudIfFromCloud = Utility.ConvertImageToThumbnail(thumbnail, ThumbnailMaxUpsize, Color.White, true);
                            e.Thumbnail = thumbnailWithCloudIfFromCloud;
                        }

                        if (thumbnail == null)
                        {
                            //if (!fileStatus.FileExists) //It's already checked
                            if (fileStatus.IsVirtual) e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorOneDriveNotRunning;
                            else if (fileStatus.IsInCloudOrVirtualOrOffline) e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorFileInCloud;
                            else if (fileStatus.HasErrorOccured) e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorGeneral;
                            else e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorNoThumbnail;
                        }
                    }
                    catch (IOException ioe)
                    {
                        if (!string.IsNullOrWhiteSpace(e.FileName))
                        {
                            FileStatus fileStatusError = FileHandler.GetFileStatus(
                                e.FileName, checkLockedStatus: true, hasErrorOccured: true, errorMessage: ioe.Message);
                            ImageListView_UpdateItemFileStatusInvoke(e.FileName, fileStatusError);
                        }
                        
                        Logger.Error(ioe, "Load image error, OneDrive issues");
                        e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorOneDriveNotRunning;
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrWhiteSpace(e.FileName))
                        {
                            FileStatus fileStatusError = FileHandler.GetFileStatus(
                                e.FileName, checkLockedStatus: true, hasErrorOccured: true, errorMessage: ex.Message);
                            ImageListView_UpdateItemFileStatusInvoke(e.FileName, fileStatusError);
                        }

                        Logger.Warn(ex, "Load image error");
                        e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorGeneral;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(e.FileName))
                    {
                        FileStatus fileStatusError = FileHandler.GetFileStatus(
                            e.FileName, checkLockedStatus: true, hasErrorOccured: true, errorMessage: "File not exist: " + e.FileName);
                        ImageListView_UpdateItemFileStatusInvoke(e.FileName, fileStatusError);
                    }

                    Logger.Warn("File not exist: " + e.FileName);
                    e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorFileNotExist;
                }
            } catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(e.FileName))
                {
                    FileStatus fileStatusError = FileHandler.GetFileStatus(
                        e.FileName, checkLockedStatus: true, hasErrorOccured: true, errorMessage: ex.Message);
                    ImageListView_UpdateItemFileStatusInvoke(e.FileName, fileStatusError);
                }

                Logger.Warn(ex, "imageListView1_RetrieveItemThumbnail failed on: " + e.FileName);
                e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorGeneral;
            }
        }
        #endregion

        #region ImageListView - Event - GetFileStatusIcon
        private Image GetFileStatusIcon(FileStatus fileStatus)
        {
            if (!fileStatus.FileExists) return (Image)Properties.Resources.ImageListViewLoadErrorFileNotExist; //File has become deleted
            else if (fileStatus.IsVirtual) return (Image)Properties.Resources.ImageListViewLoadErrorOneDriveNotRunning;
            else if (fileStatus.IsInCloudOrVirtualOrOffline) return (Image)Properties.Resources.ImageListViewLoadErrorFileInCloud;
            else if (fileStatus.HasErrorOccured) return (Image)Properties.Resources.ImageListViewLoadErrorGeneral;
            return (Image)Properties.Resources.ImageListViewLoadErrorOneDriveNotRunning;
        }
        #endregion 

        #region ImageListView - Event - Retrieve Image 
        /// <summary>
        /// RetrieveImage occures when ImageListView will show bigger picture than Thumbnail
        /// </summary>
        /// <param name="sender">ImageListView sender</param>
        /// <param name="e">ImageListView events parameter</param>
        private void imageListView1_RetrieveImage(object sender, RetrieveItemImageEventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (imageListView1.IsDisposed) return;

            bool retry = false;
            int retryCount = 3; //In case of waiting for OneDrive to load and timeout 

            #region For 32-bit OS, where very little memory, and need to clean up
            do
            {
                try
                {
                    Image fullSizeImage = LoadMediaCoverArtPosterWithCache(e.FullFilePath);
                    e.LoadedImage = fullSizeImage;
                }
                #region OutOfMemory, IOException (OneDrive issues) - Error handling
                //This is only error handling
                //1. When I was using 32bit version, I got lot of our of memory
                //2. When OneDrive had chrased, lot of stranger errors occured
                catch (OutOfMemoryException)
                {
                    e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorOutOfMemory;
                    
                    ReadImageOutOfMemoryWillWaitCacheEmpty = new AutoResetEvent(false);
                    ReadImageOutOfMemoryWillWaitCacheEmpty.WaitOne(10000);
                    lock (ReadImageOutOfMemoryWillWaitCacheEmpty)
                    {
                        ReadImageOutOfMemoryWillWaitCacheEmpty = null;
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    
                    if (retryCount-- > 0) 
                        retry = true; 
                    else 
                        retry = false;
                }
                catch (IOException ioex) //Set an error picture, OneDrive problems
                {
                    if (!string.IsNullOrWhiteSpace(e.FullFilePath))
                    {
                        FileStatus fileStatusError = FileHandler.GetFileStatus(
                            e.FullFilePath, checkLockedStatus: true, hasErrorOccured: true, errorMessage: ioex.Message);
                        ImageListView_UpdateItemFileStatusInvoke(e.FullFilePath, fileStatusError);
                    }

                    e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorOneDriveNotRunning;
                }
                catch (Exception ex)
                {
                    FileStatus fileStatus = FileHandler.GetFileStatus(
                            e.FullFilePath, checkLockedStatus: true, hasErrorOccured: true, errorMessage: ex.Message);
                    ImageListView_UpdateItemFileStatusInvoke(e.FullFilePath, fileStatus);
                    e.LoadedImage = GetFileStatusIcon(fileStatus);
                }

                
                if (e.LoadedImage == null)
                {
                    FileStatus fileStatus = FileHandler.GetFileStatus(
                        e.FullFilePath, checkLockedStatus: true, hasErrorOccured: true, errorMessage: "Failed to load poster");
                    ImageListView_UpdateItemFileStatusInvoke(e.FullFilePath, fileStatus);
                    e.LoadedImage = GetFileStatusIcon(fileStatus);
                }
                #endregion
            } while (retry);
            #endregion 
        }
        #endregion

        #region ImageListView - Event - SelectionChanged 

        #region ImageListView - Event - SelectionChanged - *** imageListView1_SelectionChanged ***
        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (GlobalData.DoNotTrigger_ImageListView_SelectionChanged) return;
            if (IsPerforminAButtonAction("Selection Changed for Media files")) return;
            if (IsPopulatingAnything("Selection Changed for Media files")) return;
            if (!GlobalData.IsPopulatingImageListViewFromFolderOrDatabaseList) SaveBeforeContinue(false);

            GlobalData.IsPerformingAButtonAction = true;

            try
            {
                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ImageListView - SelectionChanged Action - For Selected Files - Populate DataGridView, OpenWith...
        private void ImageListView_SelectionChanged_Action_ImageListView_DataGridView(bool allowUseCache)
        {
            if (GlobalData.IsApplicationClosing) return;

            try
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
                ImageListViewSuspendLayoutInvoke(imageListView1);

                using (new WaitCursor())
                {
                    ImageListViewHandler.ClearCacheFileEntriesSelectedItems(imageListView1);
                    FastGroupSelection_Clear();
                    SetDataGridViewForLocationAnalytics();
                    ClearQueueLazyLoadningSelectedFilesLock();

                    GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                    HashSet<FileEntry> fileEntries = ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, allowUseCache);
                    ImageListView_SelectionChanged_Action_CheckFileStatusRemoveNoneExistFilesFromSelectedFiles(ref fileEntries);
                    DataGridView_CleanAll();
                    foreach (FileEntry fileEntry in fileEntries)
                    {
                        FileHandler.RemoveOfflineFileTouchedFailed(fileEntry.FileFullPath);
                        FileHandler.RemoveOfflineFileTouched(fileEntry.FileFullPath);
                    }
                    DataGridView_Populate_SelectedItemsThread(fileEntries);

                    PopulateImageListViewOpenWithToolStripThread(fileEntries, ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                    UpdateRibbonsWhenWorkspaceChanged();

                    MaximizeOrRestoreWorkspaceMainCellAndChilds();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                ImageListViewResumeLayoutInvoke(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
            }
        }
        #endregion

        #region ImageListView - SelectionChanged Action - CheckFileStatusRemoveNoneExistFilesFromSelectedFiles
        private void ImageListView_SelectionChanged_Action_CheckFileStatusRemoveNoneExistFilesFromSelectedFiles(ref HashSet<FileEntry> fileEntries)
        {
            try
            {
                HashSet<FileEntry> filesDoesNotExist = new HashSet<FileEntry>();
                foreach (FileEntry fileEntry in fileEntries)
                {
                    DateTime dateTime = FileHandler.GetLastWriteTime(fileEntry.FileFullPath, waitAndRetry: false);
                    #region Mark file not exist anymore
                    if (dateTime <= FileHandler.MinimumFileSystemDateTime && !filesDoesNotExist.Contains(fileEntry)) filesDoesNotExist.Add(fileEntry);
                    #endregion

                    #region Force refesh if file has changed
                    if (dateTime > FileHandler.MinimumFileSystemDateTime && dateTime != fileEntry.LastWriteDateTime) 
                        ImageListView_UpdateItemThumbnailUpdateAllInvoke(fileEntry);
                    #endregion
                }

                if (filesDoesNotExist.Count > 0)
                {
                    #region Remove from ImageListView
                    using (new WaitCursor())
                    {
                        foreach (FileEntry fileEntry in filesDoesNotExist) fileEntries.Remove(fileEntry);
                        UpdateStatusAction("Deleing files and all record about files in database....");
                        filesCutCopyPasteDrag.DeleteSelectedFiles(imageListView1, filesDoesNotExist, false);
                        ImageListViewHandler.ClearCacheFileEntries(imageListView1);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region ImageListView - Suspend / Resume

        #region ImageListView - SuspendLayout - Invoke
        private void ImageListViewSuspendLayoutInvoke(ImageListView imageListView)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListView>(ImageListViewSuspendLayoutInvoke), imageListView);
                return;
            }

            imageListView.SuspendLayout(); 
        }
        #endregion

        #region ImageListView - ResumeLayout - Invoke
        private void ImageListViewResumeLayoutInvoke(ImageListView imageListView)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListView>(ImageListViewResumeLayoutInvoke), imageListView);
                return;
            }
            imageListView.ResumeLayout(); 
        }
        #endregion

        #endregion
        #endregion

        #region ImageListView - OpenWith

        #region ImageListView - Populate - OpenWith - Thread
        private void PopulateImageListViewOpenWithToolStripThread(HashSet<FileEntry> imageListViewSelectedItems, HashSet<FileEntry> imageListViewItems)
        {
            bool addOnlySelectedItems = true;
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                case KryptonPages.kryptonPageFolderSearchFilterFilter:                
                case KryptonPages.kryptonPageToolboxTags:
                case KryptonPages.kryptonPageToolboxPeople:
                case KryptonPages.kryptonPageToolboxMap:
                case KryptonPages.kryptonPageToolboxDates:
                case KryptonPages.kryptonPageToolboxExiftool:
                case KryptonPages.kryptonPageToolboxWarnings:
                case KryptonPages.kryptonPageToolboxProperties:
                case KryptonPages.kryptonPageToolboxRename:
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    addOnlySelectedItems = false;
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    addOnlySelectedItems = true;
                    break;
                default:
                    throw new NotImplementedException();
            }

            HashSet<FileEntry> imageListViewFileEntryCopy = new HashSet<FileEntry>();
            try
            {
                if (addOnlySelectedItems && imageListViewSelectedItems != null) imageListViewFileEntryCopy = new HashSet<FileEntry>(imageListViewSelectedItems);
                if (!addOnlySelectedItems && imageListViewItems != null) imageListViewFileEntryCopy = new HashSet<FileEntry>(imageListViewItems);
            }
            catch { }

            Thread threadPopulateOpenWith = new Thread(() => { PopulateImageListViewOpenWithToolStripInvoke(imageListViewFileEntryCopy); });
            threadPopulateOpenWith.Start();
        }
        #endregion 

        #region ImageListView - Populate - OpenWith - Invoke
        private void PopulateImageListViewOpenWithToolStripInvoke(HashSet<FileEntry> imageListViewSelectedFileEntryItems)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<HashSet<FileEntry>>(PopulateImageListViewOpenWithToolStripInvoke), imageListViewSelectedFileEntryItems);
                return;
            }

            try
            {
                List<string> extentions = new List<string>();

                foreach (FileEntry fileEntry in imageListViewSelectedFileEntryItems)
                {
                    string extention = Path.GetExtension(fileEntry.FileFullPath).ToLower();
                    if (!extentions.Contains(extention)) extentions.Add(extention);
                }

                ApplicationAssociationsHandler applicationAssociationsHandler = new ApplicationAssociationsHandler();
                List<ApplicationData> listOfCommonOpenWith = applicationAssociationsHandler.OpenWithInCommon(extentions);

                KryptonContextMenu kryptonContextMenu = new KryptonContextMenu();
                KryptonContextMenuItems kryptonContextMenuItems = new KryptonContextMenuItems();
                kryptonContextMenu.Items.Add(kryptonContextMenuItems);

                kryptonContextMenuItemsGenericOpenWithAppList.Items.Clear();
                kryptonRibbonGroupButtonHomeFileSystemOpenWith.KryptonContextMenu = null;

                if (listOfCommonOpenWith != null && listOfCommonOpenWith.Count > 0)
                {
                    foreach (ApplicationData data in listOfCommonOpenWith)
                    {
                        foreach (VerbLink verbLink in data.VerbLinks)
                        {
                            ApplicationData singelVerbApplicationData = new ApplicationData();
                            singelVerbApplicationData.AppIconReference = data.AppIconReference;
                            singelVerbApplicationData.ApplicationId = data.ApplicationId;
                            singelVerbApplicationData.Command = data.Command;
                            singelVerbApplicationData.FriendlyAppName = data.FriendlyAppName;
                            singelVerbApplicationData.Icon = data.Icon;
                            singelVerbApplicationData.ProgId = data.ProgId;
                            singelVerbApplicationData.AddVerb(verbLink.Verb, verbLink.Command);

                            Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem = new Krypton.Toolkit.KryptonContextMenuItem();
                            kryptonContextMenuItem.Text = singelVerbApplicationData.FriendlyAppName.Replace("&", "&&") + " - " + verbLink.Verb;
                            kryptonContextMenuItem.Image = new Bitmap(singelVerbApplicationData.Icon.ToBitmap(), new Size(32, 32));
                            kryptonContextMenuItem.Tag = singelVerbApplicationData;
                            kryptonContextMenuItem.Click += KryptonContextMenuItemOpenWithSelectedVerb;
                            kryptonContextMenuItemsGenericOpenWithAppList.Items.Add(kryptonContextMenuItem);
                            kryptonContextMenuItems.Items.Add(kryptonContextMenuItem);
                        }
                    }
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.KryptonContextMenu = kryptonContextMenu;
                }
            } catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        #endregion

        #endregion

        #region ImageListView - Aggregate / Populate 

        #region Database Search - click
        private void buttonFilterSearch_Click(object sender, EventArgs e)
        {
            ImageListView_FetchListOfMediaFiles_FromDatabase_and_Aggregate();
        }
        #endregion

        #region ImageListView - Fetch List Of Media Files - FromDatabase and Aggregate
        private void ImageListView_FetchListOfMediaFiles_FromDatabase_and_Aggregate()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            ClearAllQueues();
            //GlobalData.SerachFilterResult = databaseAndCacheMetadataExiftool.ListAllSearch
            //GlobalData.SearchFolder = false;

            #region DateTaken
            bool useMediaTakenFrom = dateTimePickerSearchDateFrom.Checked;
            DateTime mediaTakenFrom = new DateTime(dateTimePickerSearchDateFrom.Value.Year, dateTimePickerSearchDateFrom.Value.Month, dateTimePickerSearchDateFrom.Value.Day);
            bool useMediaTakenTo = dateTimePickerSearchDateTo.Checked;
            DateTime mediaTakenTo = new DateTime(dateTimePickerSearchDateTo.Value.Year, dateTimePickerSearchDateTo.Value.Month, dateTimePickerSearchDateTo.Value.Day).AddDays(1);
            bool isMediaTakenNull = checkBoxSearchMediaTakenIsNull.Checked;
            #endregion 

            #region Text tags
            bool usePersonalAlbum = !string.IsNullOrWhiteSpace(comboBoxSearchAlbum.Text);
            string personalAlbum = comboBoxSearchAlbum.SelectedIndex == 0 ? null : comboBoxSearchAlbum.Text;
            bool usePersonalTitle = !string.IsNullOrWhiteSpace(comboBoxSearchTitle.Text);
            string personalTitle = comboBoxSearchTitle.SelectedIndex == 0 ? null : comboBoxSearchTitle.Text;
            bool usePersonalComments = !string.IsNullOrWhiteSpace(comboBoxSearchComments.Text);
            string personalComments = comboBoxSearchComments.SelectedIndex == 0 ? null : comboBoxSearchComments.Text;
            bool usePersonalDescription = !string.IsNullOrWhiteSpace(comboBoxSearchDescription.Text);
            string personalDescription = comboBoxSearchDescription.SelectedIndex == 0 ? null : comboBoxSearchDescription.Text;
            bool useLocationName = !string.IsNullOrWhiteSpace(comboBoxSearchLocationName.Text);
            string locationName = comboBoxSearchLocationName.SelectedIndex == 0 ? null : comboBoxSearchLocationName.Text;
            bool useLocationCity = !string.IsNullOrWhiteSpace(comboBoxSearchLocationCity.Text);
            string locationCity = comboBoxSearchLocationCity.SelectedIndex == 0 ? null : comboBoxSearchLocationCity.Text;
            bool useLocationState = !string.IsNullOrWhiteSpace(comboBoxSearchLocationState.Text);
            string locationState = comboBoxSearchLocationState.SelectedIndex == 0 ? null : comboBoxSearchLocationState.Text;
            bool useLocationCountry = !string.IsNullOrWhiteSpace(comboBoxSearchLocationCountry.Text);
            string locationCountry = comboBoxSearchLocationCountry.SelectedIndex == 0 ? null : comboBoxSearchLocationCountry.Text;
            bool useAndBetweenTextTagFields = checkBoxSearchUseAndBetweenTextTagFields.Checked;
            #endregion 

            #region Rating
            bool isRatingNull = checkBoxSearchRatingEmpty.Checked;
            bool hasRating0 = checkBoxSearchRating0.Checked;
            bool hasRating1 = checkBoxSearchRating1.Checked;
            bool hasRating2 = checkBoxSearchRating2.Checked;
            bool hasRating3 = checkBoxSearchRating3.Checked;
            bool hasRating4 = checkBoxSearchRating4.Checked;
            bool hasRating5 = checkBoxSearchRating5.Checked;
            #endregion

            #region Region Names
            bool useRegionNameList = checkedListBoxSearchPeople.CheckedItems.Count > 0;
            bool needAllRegionNames = checkBoxSearchNeedAllNames.Checked;
            bool withoutRegions = checkBoxSearchWithoutRegions.Checked;

            List<string> regionNameList = new List<string>();
            for (int index = 0; index < checkedListBoxSearchPeople.Items.Count; index++)
            {
                if (checkedListBoxSearchPeople.GetItemChecked(index))
                {
                    if (index == 0) regionNameList.Add(null);
                    else
                        regionNameList.Add(checkedListBoxSearchPeople.Items[index].ToString());
                }
            }
            #endregion

            #region Keywords
            bool useKeywordList = !string.IsNullOrWhiteSpace(comboBoxSearchKeyword.Text);
            bool needAllKeywords = checkBoxSearchNeedAllKeywords.Checked;
            bool withoutKeywords = checkBoxSearchWithoutKeyword.Checked;

            List<string> keywords = new List<string>();
            keywords.AddRange(comboBoxSearchKeyword.Text.Split(';'));
            #endregion

            #region Warning
            bool checkIfHasExifWarning = checkBoxSearchHasWarning.Checked;
            #endregion

            #region Between Groups
            bool useAndBetweenGroups = checkBoxSerachFitsAllValues.Checked;
            #endregion

            LoadingItemsImageListView(1, 6);
            UpdateStatusImageListView("Searhing for match in database...");

            #region Read from Database
            string searchDirectory = kryptonTextBoxSearchDirectory.Text;
            bool useSearchDirectory = !string.IsNullOrWhiteSpace(searchDirectory);
            string searchFilename = kryptonTextBoxSearchFilename.Text;
            bool useSearchFilename = !string.IsNullOrWhiteSpace(searchFilename);

            bool useRegEx = kryptonCheckBoxSearchUseRegEx.Checked;

            int maxRowsInResult = Properties.Settings.Default.MaxRowsInSearchResult;

            GlobalData.SerachFilterResult = databaseAndCacheMetadataExiftool.ListAllSearch(MetadataBrokerType.ExifTool,
                useAndBetweenGroups, useRegEx,
                useMediaTakenFrom, mediaTakenFrom, useMediaTakenTo, mediaTakenTo, isMediaTakenNull,
                useAndBetweenTextTagFields,
                usePersonalAlbum, personalAlbum,
                usePersonalTitle, personalTitle,
                usePersonalComments, personalComments,
                usePersonalDescription, personalDescription,
                isRatingNull, hasRating0, hasRating1, hasRating2, hasRating3, hasRating4, hasRating5,
                useLocationName, locationName,
                useLocationCity, locationCity,
                useLocationState, locationState,
                useLocationCountry, locationCountry,
                useRegionNameList, needAllRegionNames, regionNameList, withoutRegions,
                useKeywordList, needAllKeywords, keywords, withoutKeywords,
                checkIfHasExifWarning, maxRowsInResult,
                useSearchDirectory, searchDirectory,
                useSearchFilename, searchFilename);
            GlobalData.SearchFolder = false;
            #endregion 

            ImageListView_Aggregate_FromDatabaseSearchResult_and_Aggregate(GlobalData.SerachFilterResult, true); //True = New search result needs to aggregate and poplate filters
        }
        #endregion

        #region ImageListView - SelectFiles
        private void ImageListView_SelectFiles(List<string> listOfFileFullPath, bool clearSelection = true)
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
                ImageListViewHandler.SuspendLayout(imageListView1);

                using (new WaitCursor())
                {

                    if (clearSelection) ImageListViewSuspendLayoutInvoke(imageListView1);

                    imageListView1.ClearSelection();
                    foreach (string fullFilename in listOfFileFullPath)
                    {
                        ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fullFilename);
                        if (foundItem != null) foundItem.Selected = true;
                    }
                    ImageListViewResumeLayoutInvoke(imageListView1);
                }
                DisplayAllQueueStatus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                ImageListViewHandler.ResumeLayout(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
        }
        #endregion

        #region ImageListView - Fetch List of Media Files - FromFolder and Aggregate
        private void ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(bool recursive, bool runPopulateFilter)
        {
            if (GlobalData.IsApplicationClosing) return;

            #region If already in progress, then stop and re-select new
            if (GlobalData.IsPopulatingImageListViewFromFolderOrDatabaseList) //Remove old process and continue with new process
            {
                UpdateStatusImageListView("Remove old queues...");
                ImageListViewHandler.ClearAllAndCaches(imageListView1);
                GlobalData.IsPopulatingImageListViewFromFolderOrDatabaseList = false;
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
            }
            #endregion

            FileHandler.ClearOfflineFileTouched();
            FileHandler.ClearOfflineFileTouchedFailed();

            #region Read from Folder
            string selectedFolder = GetSelectedNodeFullRealPath();
            Properties.Settings.Default.LastFolder = GetSelectedNodeFullLinkPath();

            LoadingItemsImageListView(1, 6);
            UpdateStatusImageListView("Read files in folder: " + selectedFolder);

            #region Read folder files - and - Populate ImageListView
            IEnumerable<FileData> fileDatas = GetFilesInSelectedFolder(selectedFolder, recursive);
            HashSet<FileEntry> fileEntries = ImageListView_Populate_ListOfMediaFiles_AddFilter(fileDatas, null, selectedFolder, runPopulateFilter);
            #endregion

            UpdateStatusImageListView("Check for OneDrive duplicate files in folder: " + selectedFolder);
            #region Check for OneDrive duplicate files in folder

            
            List<string> dublicatedFound = FixOneDriveIssues(fileEntries, out List<string> notFixed, oneDriveNetworkNames, fixError: false,
                moveToRecycleBin: Properties.Settings.Default.MoveToRecycleBin, databaseAndCacheMetadataExiftool);


            if (dublicatedFound.Count > 0)
            {
                string filesExampleFound = "";
                for (int fileIndex = 0; fileIndex < Math.Min(dublicatedFound.Count, 5); fileIndex++)
                {
                    filesExampleFound += (string.IsNullOrWhiteSpace(filesExampleFound) ? "" : "\r\n") + dublicatedFound[fileIndex];
                }
                if (KryptonMessageBox.Show(dublicatedFound.Count + " OneDrive duplicated files found.\r\n" +
                    "You can use the Tool: Remove OneDrive Duplicates.\r\n\r\n" +
                    "Examples files:\r\n\r\n" + filesExampleFound + "\r\n\r\n" +
                    "Select files where dulicates found?",
                    "OneDrive duplicated files found.", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, showCtrlCopy: true) == DialogResult.OK)
                {
                    ImageListView_SelectFiles(dublicatedFound);
                }
            }

            #endregion

            #endregion
        }
        #endregion

        #region ImageListView - Aggregate - Existing List of Media files - FromDatabaseSearchResult
        private void ImageListView_Aggregate_FromDatabaseSearchResult_and_Aggregate(HashSet<FileEntry> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsApplicationClosing) return;
            HashSet<FileEntry> _ = ImageListView_Populate_ListOfMediaFiles_AddFilter(null, searchFilterResult, null, runPopulateFilter);
        }
        #endregion

        #region ImageListView - Aggregate - UsingFiltersOnExistingFiles
        private void ImageListView_Aggregate_UsingFiltersOnExistingFiles(KryptonTreeView treeView)
        {
            if (treeView.Nodes == null) return;
            if (treeView.Nodes[FilterVerifyer.Root] == null) return;

            FilterVerifyer filterVerifyerFolder = new FilterVerifyer();
            filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeView, FilterVerifyer.Root);

            if (GlobalData.SearchFolder)
                ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(GlobalData.lastReadFolderWasRecursive, false); //False = No need populate filter, we are using filter
            else
                ImageListView_Aggregate_FromDatabaseSearchResult_and_Aggregate(GlobalData.SerachFilterResult, false); //False = No need populate filter, we are using filter
            imageListView1.Focus();
        }
        #endregion

        #region ImageListView - Populate - List of MediaFiles - Add Filter
        private HashSet<FileEntry> ImageListView_Populate_ListOfMediaFiles_AddFilter(IEnumerable<FileData> fileDatasFromFolder, HashSet<FileEntry> fileEntriesFromDatabase, string selectedFolder, bool runPopulateFilter = true)
        {
            GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
            GlobalData.IsPopulatingImageListViewFromFolderOrDatabaseList = true;
          
            HashSet<FileEntry> fileEntriesFound = new HashSet<FileEntry>();
            try 
            { 
                using (new WaitCursor())
                {
                    LoadingItemsImageListView(2, 6);
                    UpdateStatusImageListView("Clear old data...");

                    #region Init - Clear Old data and Caches
                    if (Properties.Settings.Default.ImageViewLoadThumbnailOnDemandMode) imageListView1.CacheMode = CacheMode.OnDemand;
                    else imageListView1.CacheMode = CacheMode.Continuous;

                    //GlobalData.DoNotTrigger_ImageListView_ItemUpdate = true;
                    if (runPopulateFilter) FilterVerifyer.ClearTreeViewNodes(treeViewFilter);
                    //GlobalData.DoNotTrigger_ImageListView_ItemUpdate = false;

                    UpdateStatusImageListView("Clear old data...");
                    KeepTrackOfMetadataLoadedClearList();
                    RemoveErrors();
                    ClearAllQueues();
                    ImageListViewHandler.ClearAllAndCaches(imageListView1);
                    #endregion

                    #region Suspend / Resume
                    TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, false);
                    ImageListViewHandler.Enable(imageListView1, false);
                    ImageListViewSuspendLayoutInvoke(imageListView1);
                    #endregion

                    LoadingItemsImageListView(3, 6);
                    UpdateStatusImageListView("Checking filter...");
                    #region Get Filter
                    FilterVerifyer filterVerifyerFolder = new FilterVerifyer();
                    int valuesCountFiltersAdded = filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeViewFilter, FilterVerifyer.Root); //Get filters
                    #endregion

                    LoadingItemsImageListView(4, 6);
                    UpdateStatusImageListView("Populating...");

                    #region Populate ImageListView
                    #region Mediafiles Fetched from Folder
                    if (fileDatasFromFolder != null)
                    {
                        foreach (FileData fileData in fileDatasFromFolder)
                        {
                            if (ImageAndMovieFileExtentionsUtility.IsMediaFormat(fileData))
                            {
                                #region Add to ImageListView and check filter
                                FileEntry fileEntry = new FileEntry(fileData.Path, fileData.LastWriteTime);
                                fileEntriesFound.Add(fileEntry);

                                if (valuesCountFiltersAdded > 0) // no filter values added, no need read from database, this just for optimize speed
                                {
                                    Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileData.Path, fileData.LastWriteTime, MetadataBrokerType.ExifTool));
                                    if (filterVerifyerFolder.VerifyMetadata(metadata))
                                    {
                                        lock (keepTrackOfLoadedMetadataLock)
                                        {
                                            ImageListViewHandler.ImageListViewAddItem(imageListView1, fileData.Path, ref hasTriggerLoadAllMetadataActions, ref keepTrackOfLoadedMetadata);
                                        }
                                    }
                                }
                                else
                                {
                                    lock (keepTrackOfLoadedMetadataLock)
                                    {
                                        ImageListViewHandler.ImageListViewAddItem(imageListView1, fileData.Path, ref hasTriggerLoadAllMetadataActions, ref keepTrackOfLoadedMetadata);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion

                    #region Mediafiles Fetched from Database
                    if (fileEntriesFromDatabase != null)
                    {
                        foreach (FileEntry fileEntry in fileEntriesFromDatabase)
                        {
                            #region Add to ImageListView and check filter
                            if (File.Exists(fileEntry.FileFullPath))
                            {
                                if (valuesCountFiltersAdded > 0) // no filter values added, no need read from database, this just for optimize speed
                                {
                                    Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                                    if (filterVerifyerFolder.VerifyMetadata(metadata))
                                    {
                                        lock (keepTrackOfLoadedMetadataLock)
                                        {
                                            ImageListViewHandler.ImageListViewAddItem(imageListView1, fileEntry.FileFullPath, ref hasTriggerLoadAllMetadataActions, ref keepTrackOfLoadedMetadata);
                                        }
                                    }
                                }
                                else
                                {
                                    lock (keepTrackOfLoadedMetadataLock)
                                    {
                                        ImageListViewHandler.ImageListViewAddItem(imageListView1, fileEntry.FileFullPath, ref hasTriggerLoadAllMetadataActions, ref keepTrackOfLoadedMetadata);
                                    }
                                }
                            }
                            #endregion
                        }
                        fileEntriesFound = fileEntriesFromDatabase;
                    }
                    #endregion

                    #endregion

                    #region Suspend / Resume
                    ImageListViewHandler.Enable(imageListView1, true);
                    ImageListViewResumeLayoutInvoke(imageListView1);
                    TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, true);
                    #endregion

                    LoadingItemsImageListView(5, 6);
                    UpdateStatusImageListView(fileEntriesFound.Count + " files added");

                    ///////////////////////////////////

                    ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false); //Even when 0 selected files, allocate data and flags, etc...
                    #region Read to cache
                    if (cacheFolderThumbnails || cacheFolderMetadatas || cacheFolderWebScraperDataSets)
                    {
                        LoadingItemsImageListView(4, 6);
                        UpdateStatusImageListView("Started the cache process...");
                    }
                    #endregion
                    StartThreads();


                    LoadingItemsImageListView(6, 6);
                    UpdateStatusImageListView("Done populate " + fileEntriesFound.Count + " media files...");

                    //treeViewFolderBrowser1.Focus();
                    //imageListView1.Focus();
                    LoadingItemsImageListView(0, 0);
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
                GlobalData.IsPopulatingImageListViewFromFolderOrDatabaseList = false;
            }

            return fileEntriesFound;
        }
        #endregion

        #endregion

        #region ImageListView - Rename 

        #region ImageListView - Remove Item - Invoke
        private void ImageListViewRemoveItemInvoke(string filename)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(ImageListViewRemoveItemInvoke), filename);
                return;
            }

            try
            {
                //ImageListViewHandler.SuspendLayout(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;

                #region Remove items with old names
                ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, filename);
                if (foundItem != null) ImageListViewHandler.ImageListViewRemoveItem(imageListView1, foundItem);
                #endregion

                DisplayAllQueueStatus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
                //ImageListViewHandler.ResumeLayout(imageListView1);
            }
        }
        #endregion

        #region ImageListView - Rename Item - Invoke
        private void ImageListView_Rename_Invoke(string oldFullFilename, string newFullFilename)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string, string>(ImageListView_Rename_Invoke), oldFullFilename, newFullFilename);
                return;
            }
            ImageListViewItem imageListViewItem = ImageListViewHandler.FindItem(imageListView1.Items, oldFullFilename);
            if (imageListViewItem != null) imageListViewItem.FileFullPath = newFullFilename;
        }
        #endregion

        #region ImageListView - Rename Item - UpdateImageViewListeAfterRename
        private void ImageViewListeUpdateAfterRename(ImageListView imageListView, string oldFilename, string newFilename)
        {
            Dictionary<string, string> renameSuccess = new Dictionary<string, string>();
            Dictionary<string, RenameToNameAndResult> renameFailed = new Dictionary<string, RenameToNameAndResult>();
            renameSuccess.Add(oldFilename, newFilename);
            ImageViewListeUpdateAfterRename(imageListView, renameSuccess, renameFailed, true);
        }
        #endregion 

        #region ImageListView - Rename Items - Aggregate 
        private void ImageViewListeUpdateAfterRename(ImageListView imageListView, Dictionary<string, string> renameSuccess, Dictionary<string, RenameToNameAndResult> renameFailed, bool onlyRenameAddbackToListView)
        {
            try 
            { 
                ImageListViewHandler.SuspendLayout(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;

                using (new WaitCursor())
                {
                    #region Remove items with old names
                    foreach (string filename in renameSuccess.Keys)
                    {
                        ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView.Items, filename);
                        if (foundItem != null) ImageListViewHandler.ImageListViewRemoveItem(imageListView, foundItem);
                    }
                    #endregion

                    #region Add new renames back to list
                    if (onlyRenameAddbackToListView)
                    {
                        lock (keepTrackOfLoadedMetadataLock)
                        {
                            foreach (string filename in renameSuccess.Values)
                            {
                                ImageListViewHandler.ImageListViewAddItem(imageListView, filename, ref hasTriggerLoadAllMetadataActions, ref keepTrackOfLoadedMetadata);
                            }
                        }
                    }
                    #endregion

                    #region AddErrors to Error Queue - Also Select all previous selected Items 
                    foreach (string filename in renameFailed.Keys)
                    {
                        FileStatus fileStatus = FileHandler.GetFileStatus(
                            filename, checkLockedStatus: true,
                            hasErrorOccured: true, errorMessage: renameFailed[filename].ErrorMessage);
                        ImageListView_UpdateItemFileStatusInvoke(filename, fileStatus);

                        FileStatus fileStatusRenameFailed = FileHandler.GetFileStatus(
                            renameFailed[filename].NewFilename, checkLockedStatus: true);

                        AddError(
                                Path.GetDirectoryName(filename), Path.GetFileName(filename), fileStatus.LastWrittenDateTime,
                                AddErrorFileSystemRegion, AddErrorFileSystemMove, filename, renameFailed[filename].NewFilename,
                                "Issue: Failed to rename file.\r\n" +
                                "From File Name:  " + filename + "\r\n" +
                                "From File Staus: " + fileStatus.ToString() + "\r\n" +
                                "To   File Name:  " + renameFailed[filename].NewFilename + "\r\n" +
                                "To   File Staus: " + fileStatusRenameFailed.ToString() + "\r\n" +
                                "Error message: " + renameFailed[filename].ErrorMessage);

                        ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView.Items, filename);
                        if (foundItem != null) foundItem.Selected = true;
                    }
                    #endregion

                    #region Select back all Items renamed
                    if (onlyRenameAddbackToListView)
                    {
                        foreach (string filename in renameSuccess.Values)
                        {
                            ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView.Items, filename);
                            if (foundItem != null) foundItem.Selected = true;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
                ImageListViewHandler.ResumeLayout(imageListView1);
            }
            
        }

        #endregion

        #endregion
    }
}
