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
        private AutoResetEvent WaitThread_PopulateTreeViewFolderFilter_Stopped = null;

        #region DataGridView - Populate File - For FileEntryAttribute missing Tag - Invoke
        private void DataGridView_Populate_FileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute>(DataGridView_Populate_FileEntryAttributeInvoke), fileEntryAttribute);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                string tag = GetActiveTabTag();
                if (!string.IsNullOrWhiteSpace(tag) && IsActiveDataGridViewAgregated(tag))
                {
                    DataGridView dataGridView = GetDataGridViewForTag(tag);
                    if (dataGridView != null) DataGridView_Populate_FileEntryAttribute(dataGridView, fileEntryAttribute, tag);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        
        private Dictionary<string, FileStatus> cacheFileStatus = new Dictionary<string, FileStatus>();
        private object cacheFileStatusLock = new object();

        #region ImageListViewItemPushFileStatus

        private void ImageListViewItemPushFileStatus(string fullFileName, FileStatus fileStatus)
        {
            lock (cacheFileStatusLock)
            {
                if (!cacheFileStatus.ContainsKey(fullFileName)) cacheFileStatus.Add(fullFileName, fileStatus);
                else cacheFileStatus[fullFileName] = fileStatus;
            }
        }
        #endregion

        #region ImageListViewItemUpdateFileStatus
        private void ImageListViewItemUpdateFileStatus(string fullFileName, FileStatus fileStatus)
        {
            lock (cacheFileStatusLock)
            {
                if (cacheFileStatus.ContainsKey(fullFileName)) cacheFileStatus[fullFileName] = fileStatus;
            }
        }
        #endregion

        #region ImageListViewItemPullFileStatus
        private FileStatus ImageListViewItemPullFileStatus(string fullFileName)
        {
            FileStatus fileStatus = null;
            lock (cacheFileStatusLock)
            {
                if (cacheFileStatus.ContainsKey(fullFileName))
                {
                    fileStatus = cacheFileStatus[fullFileName];
                    cacheFileStatus.Remove(fullFileName);
                }
            }
            return fileStatus;
        }
        #endregion

        #region ImageListView_UpdatedThumbnail_RefreshAll
        private void ImageListView_UpdatedThumbnail_RefreshAll(FileEntry fileEntry)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntry>(ImageListView_UpdatedThumbnail_RefreshAll), fileEntry);
                return;
            }

            ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fileEntry.FileFullPath);
            if (foundItem != null)
            {
                //if (!foundItem.IsItemDirty())
                {
                    ImageListViewItemPushFileStatus(fileEntry.FileFullPath, foundItem.FileStatus);
                    foundItem.Update();   
                }
                
            }
        }
        #endregion

        #region ImageListView - Update FileStatus - Invoke
        private void ImageListView_UpdateItemFileStatusInvoke(string fullFileName)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(ImageListView_UpdateItemFileStatusInvoke), fullFileName);
                return;
            }

            FileStatus fileStatus = FileHandler.GetFileStatus(fullFileName);
            ImageListView_UpdateItemFileStatusInvoke(fullFileName, fileStatus);
        }

        private void ImageListView_UpdateItemFileStatusInvoke(string fullFileName, FileStatus fileStatus)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string, FileStatus>(ImageListView_UpdateItemFileStatusInvoke), fullFileName, fileStatus);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fullFileName);
                if (foundItem != null)
                {
                    ImageListViewItemUpdateFileStatus(fullFileName, fileStatus);
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
        private void ImageListView_UpdateItemExiftoolMetadataInvoke(FileEntryAttribute fileEntryAttribute)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute>(ImageListView_UpdateItemExiftoolMetadataInvoke), fileEntryAttribute);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(
                        new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.ExifTool));
                if (metadata != null)
                {

                    ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fileEntryAttribute.FileFullPath);
                    if (foundItem != null)
                    {
                        
                        Utility.ShellImageFileInfo fileMetadata = new Utility.ShellImageFileInfo();
                        ConvertMetadataToShellImageFileInfo(ref fileMetadata, metadata);
                        if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.Error)
                        {
                            FileStatus fileStatus = FileHandler.GetFileStatus(fileEntryAttribute.FileFullPath, exiftoolProcessStatus: ExiftoolProcessStatus.FileInaccessibleOrError,
                                checkLockedStatus: true);
                            fileMetadata.FileStatus = fileStatus;
                        }

                        foundItem.UpdateDetails(fileMetadata);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

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

        #region Events 

        #region ImageListView - Event - Retrieve Metadata
        private void imageListView1_RetrieveItemMetadataDetails(object sender, RetrieveItemMetadataDetailsEventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (GlobalData.DoNotRefreshImageListView) return;
            if (imageListView1.IsDisposed) return;

            FileEntryBroker fileEntryBroker = new FileEntryBroker(e.FileName, File.GetLastWriteTime(e.FileName), MetadataBrokerType.ExifTool);
            Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBroker);

            //PS. Note: Make sure that the RetrieveItemMetadataDetails don't go in endless loop. Read data, flags as still dirty, read again, etc..
            try
            {
                #region Update FileStatus
                FileStatus fileStatus = ImageListViewItemPullFileStatus(e.FileName);
                if (fileStatus == null) fileStatus = FileHandler.GetFileStatus(e.FileName);
                #endregion

                if (metadata == null || metadata.FileName == null)
                {
                    FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(fileEntryBroker, FileEntryVersion.CurrentVersionInDatabase);

                    #region Check if has Record with Error - flag it with FileInaccessibleOrError
                    FileEntryBroker fileEntryBrokerError = new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError);
                    Metadata metadataError = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBrokerError);
                    if (metadataError != null) fileStatus.FileInaccessibleOrError = true;
                    #endregion

                    #region Add to read queue, when data missing and not marked as Error record
                    if (metadataError == null) AddQueueReadFromSourceIfMissing_AllSoruces(fileEntryAttribute);
                    #endregion

                    e.FileMetadata = new Utility.ShellImageFileInfo(); //Tell that data is create, all is good for internal void UpdateDetailsInternal(Utility.ShellImageFileInfo info)
                    e.FileMetadata.SetPropertyStatusOnAll(PropertyStatus.Requested); //All data will be read, it's in Lazy loading queue

                    fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.InExiftoolReadQueue;
                    string inCloudOrNotExistError = FileHandler.ConvertFileStatusToText(fileStatus);

                    #region Assign metadata

                    #region Provided by FileInfo
                    e.FileMetadata.FileDateCreated = DateTime.MinValue;
                    e.FileMetadata.FileDateModified = DateTime.MinValue;
                    e.FileMetadata.FileSmartDate = DateTime.MinValue;
                    e.FileMetadata.FileSize = long.MinValue;

                    if (fileStatus.FileExists)
                    {
                        try { e.FileMetadata.FileDateCreated = File.GetCreationTime(e.FileName); } catch { }
                        try { e.FileMetadata.FileDateModified = File.GetLastWriteTime(e.FileName); } catch { }
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
                    #endregion

                    #region Provided by ShellImageFileInfo, MagickImage                                
                    e.FileMetadata.CameraMake = inCloudOrNotExistError;
                    e.FileMetadata.CameraModel = inCloudOrNotExistError;
                    e.FileMetadata.MediaDimensions = new Size(0, 0);
                    #endregion

                    #region Provided by MagickImage, Exiftool
                    e.FileMetadata.MediaDateTaken = DateTime.MinValue;
                    e.FileMetadata.MediaTitle = inCloudOrNotExistError;
                    e.FileMetadata.MediaDescription = inCloudOrNotExistError;
                    e.FileMetadata.MediaComment = inCloudOrNotExistError;
                    e.FileMetadata.MediaAuthor = inCloudOrNotExistError;
                    e.FileMetadata.MediaRating = 0;
                    #endregion

                    #region Provided by Exiftool
                    e.FileMetadata.MediaAlbum = inCloudOrNotExistError;
                    e.FileMetadata.LocationDateTime = DateTime.MinValue;
                    e.FileMetadata.LocationTimeZone = inCloudOrNotExistError;
                    e.FileMetadata.LocationName = inCloudOrNotExistError;
                    e.FileMetadata.LocationRegionState = inCloudOrNotExistError;
                    e.FileMetadata.LocationCity = inCloudOrNotExistError;
                    e.FileMetadata.LocationCountry = inCloudOrNotExistError;
                    #endregion

                    #endregion
                
                    #region Provided by FileInfo
                    e.FileMetadata.DisplayName = Path.GetFileName(e.FileName);
                    //e.FileMetadata.Name= e.FileName;
                    e.FileMetadata.Extension = Path.GetExtension(e.FileName);
                    e.FileMetadata.FileAttributes = FileAttributes.Normal;
                    #endregion
                }
                else
                {
                    Utility.ShellImageFileInfo fileMetadata = new Utility.ShellImageFileInfo();
                    ConvertMetadataToShellImageFileInfo(ref fileMetadata, metadata);
                    e.FileMetadata = fileMetadata;
                    e.FileMetadata.FileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitAction;
                }

                e.FileMetadata.FileStatus = fileStatus;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "imageListView1_RetrieveItemMetadataDetails");
                if (e.FileMetadata == null) e.FileMetadata = new Utility.ShellImageFileInfo();
                e.FileMetadata.DisplayName = Path.GetFileName(e.FileName);
                e.FileMetadata.FileDirectory = Path.GetDirectoryName(e.FileName);

                if (!string.IsNullOrWhiteSpace(e.FileName))
                {
                    FileStatus fileStatus = FileHandler.GetFileStatus(
                        e.FileName, checkLockedStatus: true,
                        fileInaccessibleOrError: true, fileErrorMessage: ex.Message,
                        exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                    e.FileMetadata.FileStatus = fileStatus;
                    //No need - ImageListView_UpdateItemFileStatusInvoke(e.FileName, fileStatus);
                }
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
            if (GlobalData.DoNotRefreshImageListView) return;
            if (imageListView1.IsDisposed) return;

            try
            {
                FileStatus fileStatus = FileHandler.GetFileStatus(e.FileName);
                if (fileStatus.FileExists)
                {
                    FileEntry fileEntry = new FileEntry(e.FileName, File.GetLastWriteTime(e.FileName)); //Get last Write Time of Media file

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
                            else if (fileStatus.FileInaccessibleOrError) e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorGeneral;
                            else e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorNoThumbnail;
                        }
                    }
                    catch (IOException ioe)
                    {
                        if (!string.IsNullOrWhiteSpace(e.FileName))
                        {
                            FileStatus fileStatusError = FileHandler.GetFileStatus(
                                e.FileName, checkLockedStatus: true,
                                fileInaccessibleOrError: true, fileErrorMessage: ioe.Message,
                                exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
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
                                e.FileName, checkLockedStatus: true,
                                fileInaccessibleOrError: true, fileErrorMessage: ex.Message,
                                exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
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
                            e.FileName, checkLockedStatus: true,
                            fileInaccessibleOrError: true, fileErrorMessage: "File not exist: " + e.FileName,
                            exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
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
                        e.FileName, checkLockedStatus: true,
                        fileInaccessibleOrError: true, fileErrorMessage: ex.Message,
                        exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                    ImageListView_UpdateItemFileStatusInvoke(e.FileName, fileStatusError);
                }

                Logger.Warn(ex, "imageListView1_RetrieveItemThumbnail failed on: " + e.FileName);
                e.Thumbnail = (Image)Properties.Resources.ImageListViewLoadErrorGeneral;
            }
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
                            e.FullFilePath, checkLockedStatus: true,
                            fileInaccessibleOrError: true, fileErrorMessage: ioex.Message,
                            exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                        ImageListView_UpdateItemFileStatusInvoke(e.FullFilePath, fileStatusError);
                    }

                    e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorOneDriveNotRunning;
                }
                catch (Exception ex)
                {
                    FileStatus fileStatus = FileHandler.GetFileStatus(
                            e.FullFilePath, checkLockedStatus: true,
                            fileInaccessibleOrError: true, fileErrorMessage: ex.Message,
                            exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                    ImageListView_UpdateItemFileStatusInvoke(e.FullFilePath, fileStatus);
                    
                    e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorGeneral;                    
                    if (!fileStatus.FileExists) e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorFileNotExist; //File has become deleted
                    else if (fileStatus.IsVirtual) e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorOneDriveNotRunning;
                    else if (fileStatus.IsInCloudOrVirtualOrOffline) e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorFileInCloud;
                    else if (fileStatus.FileInaccessibleOrError) e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorGeneral;
                }

                
                if (e.LoadedImage == null)
                {
                    FileStatus fileStatus = FileHandler.GetFileStatus(
                        e.FullFilePath, checkLockedStatus: true,
                        fileInaccessibleOrError: true, fileErrorMessage: "Failed to load poster",
                        exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                    ImageListView_UpdateItemFileStatusInvoke(e.FullFilePath, fileStatus);

                    if (!fileStatus.FileExists) e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorFileNotExist; //File has become deleted
                    else if (fileStatus.IsVirtual) e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorOneDriveNotRunning;
                    else if (fileStatus.IsInCloudOrVirtualOrOffline) e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorFileInCloud;
                    else if (fileStatus.FileInaccessibleOrError) e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorGeneral;

                    e.LoadedImage = (Image)Properties.Resources.ImageListViewLoadErrorOneDriveNotRunning;
                }
                #endregion
            } while (retry);
            #endregion 
        }
        #endregion

        #region ImageListView - Event *** SelectionChanged -> OnImageListViewSelect_FilesSelectedOrNoneSelected ***
        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            ImageListViewHandler.ClearCacheFileEntriesSelectedItems(imageListView1);

            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;
            if (!GlobalData.IsPopulatingFolderSelected && !GlobalData.IsApplicationClosing) SaveBeforeContinue(false);

            GroupSelectionClear();
            ImageListViewSuspendLayoutInvoke(imageListView1); //When Enabled = true, selection was cancelled during Updating the grid

            OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
            ImageListViewResumeLayoutInvoke(imageListView1);
            MaximizeOrRestoreWorkspaceMainCellAndChilds();
        }
        #endregion

        #endregion

        #region ImageListView - Actions

        #region ImageListView - For Selected Files - Populate DataGridView, OpenWith...
        private void OnImageListViewSelect_FilesSelectedOrNoneSelected(bool allowUseCache)
        {
            if (GlobalData.IsPopulatingAnything()) return; //E.g. Populate FolderSelect
            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;

            using (new WaitCursor())
            {
                GlobalData.IsPopulatingImageListView = true;
                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                HashSet<FileEntry> fileEntries = ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, allowUseCache);
                ImageListView_RemoveNoneExistFilesFromSelectedFiles(ref fileEntries);
                DataGridView_Populate_SelectedItemsThread(fileEntries);
                PopulateImageListViewOpenWithToolStripThread(fileEntries, ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                UpdateRibbonsWhenWorkspaceChanged();

                GlobalData.IsPopulatingImageListView = false;
            }
        }
        #endregion

        #region ImageListView - Selection - RemoveNoneExistFilesFromSelectedFiles
        private void ImageListView_RemoveNoneExistFilesFromSelectedFiles(ref HashSet<FileEntry> fileEntries)
        {
            try
            {
                HashSet<FileEntry> filesDoesNotExist = new HashSet<FileEntry>();
                foreach (FileEntry fileEntry in fileEntries)
                {
                    if (!File.Exists(fileEntry.FileFullPath) && !filesDoesNotExist.Contains(fileEntry)) filesDoesNotExist.Add(fileEntry);
                }
                if (filesDoesNotExist.Count > 0)
                {
                    string listOfFiles = "";
                    int count = 0;
                    foreach (FileEntry fileEntry in filesDoesNotExist)
                    {
                        listOfFiles += fileEntry.FileFullPath + "\r\n";
                        if (count++ > 4)
                        {
                            listOfFiles += "and more....\r\n";
                            break;
                        }
                    }

                    if (KryptonMessageBox.Show(
                        (filesDoesNotExist.Count == 1 ? "File" : filesDoesNotExist.Count.ToString() + " files") + " doesn't exsist anymore\r\n" +
                        "The files will be removed from the list of media files and from the database.\r\n\r\n" +
                        "Example:\r\n" +
                        listOfFiles, "File(s) does'n exist...",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, showCtrlCopy: true) == DialogResult.OK)
                    {
                        using (new WaitCursor())
                        {
                            foreach (FileEntry fileEntry in filesDoesNotExist) fileEntries.Remove(fileEntry);
                            UpdateStatusAction("Deleing files and all record about files in database....");
                            filesCutCopyPasteDrag.DeleteSelectedFiles(this, imageListView1, filesDoesNotExist, false);
                            ImageListViewHandler.ClearCacheFileEntries(imageListView1);
                        }
                    }
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

        #region ImageListView - Populate

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
                            kryptonContextMenuItem.Click += KryptonContextMenuItemOpenWithSelectedVerb_Click;
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

        #region ImageListView - Aggregate - FromReadFolderOrFilterOrDatabase
        private HashSet<FileEntry> ImageListView_Aggregate_FromReadFolderOrFilterOrDatabase(IEnumerable<FileData> fileDatas, HashSet<FileEntry> fileEntries, string selectedFolder, bool runPopulateFilter = true)
        {
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice

                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, false);
                LoadingItemsImageListView(1, 6);
                UpdateStatusImageListView("Clear all old queues");
                ClearAllQueues();

                if (runPopulateFilter) FilterVerifyer.ClearTreeViewNodes(treeViewFilter);

                LoadingItemsImageListView(2, 6);
                UpdateStatusImageListView("Adding files to image list...");
                fileEntries = ImageListView_Populate_MediaFiles_WithFilter(fileDatas, fileEntries);
                ImageListViewHandler.SetFileEntriesNewCache(imageListView1, fileEntries);

                LoadingItemsImageListView(3, 6);
                UpdateStatusImageListView("Sorting...");
                ImageListViewSortByCheckedRadioButton(false);

                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, true);
                GlobalData.IsPopulatingFolderSelected = false;

                #region Read to cache
                if (cacheFolderThumbnails || cacheFolderMetadatas || cacheFolderWebScraperDataSets)
                {
                    LoadingItemsImageListView(4, 6);
                    UpdateStatusImageListView("Started the cache process...");
                    PreloadCacheFileEntries(fileEntries, selectedFolder);
                }
                #endregion

                #region PopulateFilter
                if (runPopulateFilter)
                {
                    LoadingItemsImageListView(6, 6);
                    UpdateStatusImageListView("Populate Filters...");
                    PopulateTreeViewFolderFilterThread(fileEntries);
                }
                #endregion

                OnImageListViewSelect_FilesSelectedOrNoneSelected(false); //Even when 0 selected files, allocate data and flags, etc...

                LoadingItemsImageListView(6, 6);
                UpdateStatusImageListView("Done populate " + fileEntries.Count + " media files...");
                treeViewFolderBrowser1.Focus();
                LoadingItemsImageListView(0, 0);
            }
            return fileEntries;
        }
        #endregion

        #region ImageListView - Aggregate - FromFolder
        private void ImageListView_Aggregate_FromFolder(bool recursive, bool runPopulateFilter)
        {

            if (GlobalData.IsPopulatingFolderSelected) //If in progress, then stop and reselect new
            {
                UpdateStatusImageListView("Remove old queues...");
                ImageListViewHandler.ClearAllAndCaches(imageListView1);
                GlobalData.IsPopulatingFolderSelected = false;
            }

            if (GlobalData.IsPopulatingAnything()) return;

            bool wasOneDriveDublicatedFoundAndremoved = false;
            do
            {
                #region Read folder files
                string selectedFolder = GetSelectedNodeFullRealPath();
                Properties.Settings.Default.LastFolder = GetSelectedNodeFullLinkPath();

                UpdateStatusImageListView("Read files in folder: " + selectedFolder);
                IEnumerable<FileData> fileDatas = GetFilesInSelectedFolder(selectedFolder, recursive);
                HashSet<FileEntry> fileEntries = ImageListView_Aggregate_FromReadFolderOrFilterOrDatabase(fileDatas, null, selectedFolder, runPopulateFilter);
                #endregion

                #region Check for OneDrive duplicate files in folder
                wasOneDriveDublicatedFoundAndremoved = false;
                UpdateStatusImageListView("Check for OneDrive duplicate files in folder: " + selectedFolder);
                if (FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, oneDriveNetworkNames, false))
                {
                    switch (KryptonMessageBox.Show("OneDrive duplicated files found.\r\n" +
                        "\r\n" +
                        "Will you replace older files with newest files\r\n" +
                        "Yes - keep the newest files\r\n" +
                        "No - delete OneDrive marked files regardless who is newest\r\n" +
                        "Cancel - Cancel the operation, Leave the files intact", "OneDrive duplicated files found.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, showCtrlCopy: true))
                    {
                        case DialogResult.Yes:
                            FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, oneDriveNetworkNames, true, true);
                            wasOneDriveDublicatedFoundAndremoved = false;
                            break;
                        case DialogResult.No:
                            FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, oneDriveNetworkNames, true, false);
                            wasOneDriveDublicatedFoundAndremoved = true;
                            break;
                    }
                }
                #endregion
            } while (wasOneDriveDublicatedFoundAndremoved);

        }
        #endregion

        #region ImageListView - Aggregate - FromDatabaseSearchResult
        private void ImageListView_Aggregate_FromDatabaseSearchResult(HashSet<FileEntry> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            HashSet<FileEntry> _ = ImageListView_Aggregate_FromReadFolderOrFilterOrDatabase(null, searchFilterResult, null, runPopulateFilter);
        }
        #endregion 

        #region ImageListView - Aggregate - FromSearchFilter
        private HashSet<FileEntry> ImageListView_Populate_MediaFiles_WithFilter(IEnumerable<FileData> fileDatasFromFolder, HashSet<FileEntry> fileEntriesFromDatabase)
        {

            HashSet<FileEntry> fileEntriesFound = new HashSet<FileEntry>();

            if (Properties.Settings.Default.ImageViewLoadThumbnailOnDemandMode) imageListView1.CacheMode = CacheMode.OnDemand;
            else imageListView1.CacheMode = CacheMode.Continuous;

            ImageListViewHandler.ClearAllAndCaches(imageListView1);

            ImageListViewHandler.Enable(imageListView1, false);
            ImageListViewSuspendLayoutInvoke(imageListView1);

            FilterVerifyer filterVerifyerFolder = new FilterVerifyer();
            int valuesCountAdded = filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeViewFilter, FilterVerifyer.Root); //Get filters

            if (fileDatasFromFolder != null)
            {
                foreach (FileData fileData in fileDatasFromFolder)
                {
                    if (ImageAndMovieFileExtentionsUtility.IsMediaFormat(fileData))
                    {
                        #region Add to ImageListView and check filter
                        FileEntry fileEntry = new FileEntry(fileData.Path, fileData.LastWriteTime);
                        fileEntriesFound.Add(fileEntry);

                        if (valuesCountAdded > 0) // no filter values added, no need read from database, this just for optimize speed
                        {
                            Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileData.Path, fileData.LastWriteTime, MetadataBrokerType.ExifTool));
                            if (filterVerifyerFolder.VerifyMetadata(metadata)) ImageListViewHandler.ImageListViewAddItem(imageListView1, fileData.Path);
                        }
                        else ImageListViewHandler.ImageListViewAddItem(imageListView1, fileData.Path);
                        #endregion
                    }
                }
            }

            if (fileEntriesFromDatabase != null)
            {
                foreach (FileEntry fileEntry in fileEntriesFromDatabase)
                {
                    #region Add to ImageListView and check filter
                    if (File.Exists(fileEntry.FileFullPath))
                    {
                        if (valuesCountAdded > 0) // no filter values added, no need read from database, this just for optimize speed
                        {
                            Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                            if (filterVerifyerFolder.VerifyMetadata(metadata)) ImageListViewHandler.ImageListViewAddItem(imageListView1, fileEntry.FileFullPath);
                        }
                        else ImageListViewHandler.ImageListViewAddItem(imageListView1, fileEntry.FileFullPath);
                    }
                    #endregion
                }
                fileEntriesFound = fileEntriesFromDatabase;
            }

            //UpdateStatusImageListView("Sorting...");
            //ImageListViewSortByCheckedRdioButton();

            ImageListViewHandler.Enable(imageListView1, true);
            ImageListViewResumeLayoutInvoke(imageListView1);
            UpdateStatusImageListView(fileEntriesFound.Count + " files added");

            StartThreads();

            return fileEntriesFound;
        }
        #endregion
        
        #region ImageListView - Aggregate - Rename Items
        private void UpdateImageViewListeAfterRename(ImageListView imageListView, Dictionary<string, string> renameSuccess, Dictionary<string, RenameToNameAndResult> renameFailed, bool onlyRenameAddbackToListView)
        {
            //GlobalData.DoNotRefreshImageListView = true;
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            ImageListViewSuspendLayoutInvoke(imageListView);

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
                foreach (string filename in renameSuccess.Values) ImageListViewHandler.ImageListViewAddItem(imageListView, filename);
            }
            #endregion 

            #region AddErrors to Error Queue - Also Select all previous selected Items 
            foreach (string filename in renameFailed.Keys)
            {
                DateTime dateTimeLastWriteTime = DateTime.Now;
                try
                {
                    dateTimeLastWriteTime = File.GetLastWriteTime(filename);
                }
                catch { }

                FileStatus fileStatus = FileHandler.GetFileStatus(
                    filename, checkLockedStatus: true,
                    fileInaccessibleOrError: true, fileErrorMessage: renameFailed[filename].ErrorMessage,
                    exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                ImageListView_UpdateItemFileStatusInvoke(filename, fileStatus);

                FileStatus fileStatusRenameFailed = FileHandler.GetFileStatus(
                    renameFailed[filename].NewFilename, checkLockedStatus: true);
                //ImageListView_UpdateItemFileStatusInvoke(renameFailed[filename].NewFilename, fileStatusRenameFailed);

                AddError(
                        Path.GetDirectoryName(filename), Path.GetFileName(filename), dateTimeLastWriteTime,
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

            ImageListViewResumeLayoutInvoke(imageListView);
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
        }

        #endregion

        #endregion
    }
}
