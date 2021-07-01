using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Manina.Windows.Forms;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using System.Threading;
using ApplicationAssociations;
using Exiftool;
using System.Collections.Generic;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{
    
    public partial class MainForm : Form
    {
        private AutoResetEvent ReadImageOutOfMemoryWillWaitCacheEmpty = null; //When out of memory, then wait for all data ready = new AutoResetEvent(false);
        private AutoResetEvent WaitThread_PopulateTreeViewFolderFilter_Stopped = null;

        #region ImageListView - Event - Retrieve Metadata
        private void imageListView1_RetrieveItemMetadataDetails(object sender, RetrieveItemMetadataDetailsEventArgs e)
        {
            Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(e.FileName, File.GetLastWriteTime(e.FileName), MetadataBrokerType.ExifTool));

            try
            {
                if (metadata == null || metadata.FileName == null)
                {
                    ExiftoolWriter.WaitLockedFileToBecomeUnlocked(e.FileName);
                    //Utility.ShellImageFileInfo shellImageFileInfo = ImageAndMovieFileExtentionsUtility.GetExif(e.FileName);
                    //e.FileMetadata = shellImageFileInfo;
                }
                else
                {
                    //e.FileMetadata = ImageAndMovieFileExtentionsUtility.GetExif(e.FileName);
                    //if (e.FileMetadata == null) 
                    e.FileMetadata = new Utility.ShellImageFileInfo();
                    
                    // Exif tags, Utility.ShellImageFileInfo()
                    e.FileMetadata.MediaDescription = metadata.PersonalDescription;
                    
                    if (metadata.MediaDateTaken != null) e.FileMetadata.MediaDateTaken = (DateTime)metadata.MediaDateTaken;
                    e.FileMetadata.MediaAuthor = metadata.PersonalAuthor;
                    e.FileMetadata.MediaComment = metadata.PersonalComments;

                    #region Provided by FileInfo
                    e.FileMetadata.FileDateCreated = (DateTime)metadata.FileDateCreated;
                    e.FileMetadata.FileDateModified = (DateTime)metadata.FileDateModified;
                    e.FileMetadata.FileSize = (long)metadata.FileSize;
                    e.FileMetadata.FileMimeType = metadata.FileMimeType;
                    e.FileMetadata.FileDirectory = metadata.FileDirectory;
                    #endregion

                    #region Provided by ShellImageFileInfo, MagickImage                                
                    e.FileMetadata.CameraMake = metadata.CameraMake;
                    e.FileMetadata.CameraModel = metadata.CameraModel;
                    if (metadata.MediaWidth != null && metadata.MediaHeight != null) e.FileMetadata.MediaDimensions = new Size((int)metadata.MediaWidth, (int)metadata.MediaHeight);
                    #endregion

                    #region Provided by MagickImage, Exiftool
                    e.FileMetadata.MediaTitle = metadata.PersonalTitle;
                    e.FileMetadata.MediaDescription = metadata.PersonalDescription;
                    e.FileMetadata.MediaComment = metadata.PersonalComments;
                    e.FileMetadata.MediaAuthor = metadata.PersonalAuthor;
                    e.FileMetadata.MediaRating = (byte)(metadata.PersonalRating == null ? 0 : metadata.PersonalRating);
                    #endregion

                    #region Provided by Exiftool
                    e.FileMetadata.MediaAlbum = metadata.PersonalAlbum;
                    e.FileMetadata.LocationName = metadata.LocationName;
                    e.FileMetadata.LocationRegionState = metadata.LocationState;
                    e.FileMetadata.LocationCity = metadata.LocationCity;
                    e.FileMetadata.LocationCountry = metadata.LocationCountry;
                    #endregion
                }
            } catch (Exception ex)
            {
                Logger.Error(ex.Message);
                e.FileMetadata.DisplayName = Path.GetFileName(e.FileName);
                e.FileMetadata.FileDirectory = Path.GetDirectoryName(e.FileName);
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
                if (File.Exists(e.FileName))
                {
                    FileEntry fileEntry = new FileEntry(e.FileName, File.GetLastWriteTime(e.FileName));
                    bool isFileInCloud = ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath); 
                    bool dontReadFileFromCloud = Properties.Settings.Default.AvoidOfflineMediaFiles;

                    lock (GlobalData.ReloadAllowedFromCloudLock)
                    {
                        if (GlobalData.ReloadAllowedFromCloud != null && GlobalData.ReloadAllowedFromCloud.Contains(fileEntry))
                        {
                            GlobalData.ReloadAllowedFromCloud.Remove(fileEntry);
                            dontReadFileFromCloud = false;
                        }
                    }

                    try
                    {
                        Image thumbnail = GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(fileEntry, dontReadFileFromCloud, isFileInCloud);
                        
                        if (thumbnail != null) //Add cloud icon if needed
                        {
                            Image thumbnailWithCloudIfFromCloud = Utility.ThumbnailFromImage(thumbnail, ThumbnailMaxUpsize, Color.White, true);
                            if (isFileInCloud) //If Media is in cloud, show Icon
                            {
                                using (Graphics g = Graphics.FromImage(thumbnailWithCloudIfFromCloud)) { g.DrawImage(Properties.Resources.FileInCloud, 0, 0); }
                            }
                            e.Thumbnail = thumbnailWithCloudIfFromCloud;
                        }
                        else
                        {
                            if (ExiftoolWriter.IsFileVirtual(fileEntry.FileFullPath)) e.Thumbnail = (Image)Properties.Resources.load_image_error_onedrive;
                            else if (isFileInCloud) e.Thumbnail = (Image)Properties.Resources.load_image_error_in_cloud;
                            else e.Thumbnail = (Image)Properties.Resources.load_image_error_thumbnail;
                        }
                    }
                    catch (IOException ioe)
                    {
                        Logger.Warn("Load image error, OneDrive issues" + ioe.Message);
                        e.Thumbnail = (Image)Properties.Resources.load_image_error_onedrive;
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Load image error: " + ex.Message);
                        e.Thumbnail = (Image)Properties.Resources.load_image_error_general;
                    }
                }
                else
                {
                    Logger.Warn("File not exist: " + e.FileName);
                    e.Thumbnail = (Image)Properties.Resources.load_image_error_file_not_exist;
                }
            } catch (Exception ex)
            {
                Logger.Warn("imageListView1_RetrieveItemThumbnail failed on: " + e.FileName + " " + ex.Message);
                e.Thumbnail = (Image)Properties.Resources.load_image_error_general;
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
                    Image fullSizeImage = LoadMediaCoverArtPoster(e.FullFilePath);
                    e.LoadedImage = fullSizeImage;
                    e.WasImageReadFromFile = true;
                    e.DidErrorOccourLoadMedia = false;
                }
                #region OutOfMemory, IOException (OneDrive issues) - Error handling
                //This is only error handling
                //1. When I was using 32bit version, I got lot of our of memory
                //2. When OneDrive had chrased, lot of stranger errors occured
                catch (OutOfMemoryException)
                {
                    e.LoadedImage = (Image)Properties.Resources.load_image_error_memory;
                    e.WasImageReadFromFile = false;
                    e.DidErrorOccourLoadMedia = true;
                    
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
                catch (IOException) //Set an error picture, OneDrive problems
                {
                    e.LoadedImage = (Image)Properties.Resources.load_image_error_onedrive;
                    e.WasImageReadFromFile = false;
                    e.DidErrorOccourLoadMedia = true;
                }
                catch (Exception)
                {
                    e.LoadedImage = (Image)Properties.Resources.load_image_error_general;
                    e.WasImageReadFromFile = false;
                    e.DidErrorOccourLoadMedia = true;
                }
                if (e.LoadedImage == null && ExiftoolWriter.IsFileInCloud(e.FullFilePath))
                {
                    e.LoadedImage = (Image)Properties.Resources.load_image_error_onedrive;
                    e.WasImageReadFromFile = false;
                    e.DidErrorOccourLoadMedia = true;
                }
                #endregion
            } while (retry);
            #endregion 

            try
            {
                foreach (ImageListViewItem imageListViewItem in imageListView1.Items)
                {
                    if (imageListViewItem.FileFullPath == e.FullFilePath) //Only to find DateTime Modified in the stored in the ImageListView
                    {
                        UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(new FileEntryAttribute(e.FullFilePath, imageListViewItem.DateModified, FileEntryVersion.Current), e.LoadedImage); //Also show error thumbnail
                        break;
                    }
                }
            } catch (Exception ex)
            {
                Logger.Warn("imageListView1_RetrieveImage failed on: " + e.FullFilePath + " " + ex.Message);
            }

        }
        #endregion 

        #region ImageListView - Event - SelectionChanged -> FileSelected
        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            //if (GlobalData.IsPopulatingAnything()) return; //E.g. Populate FolderSelect
            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;
            GroupSelectionClear();
            imageListView1.Enabled = false; //When Enabled = true, slection was cancelled during Updating the grid
            FilesSelected();
            imageListView1.Enabled = true;
            imageListView1.Focus();
        }
        #endregion

        #region ImageListView - ClearAll
        private void ImageListViewClearAll(ImageListView imageListeView)
        {
            imageListeView.ClearSelection();
            imageListeView.Items.Clear();
            imageListeView.ClearThumbnailCache();
            imageListeView.Refresh();
        }
        #endregion

        #region ImageListView - ClearThumbnailCache
        private void ImageListViewClearThumbnailCache(ImageListView imageListeView)
        {
            imageListeView.ClearThumbnailCache();
        }
        #endregion

        #region ImageListView - FindItem
        private ImageListViewItem FindItemInImageListView(ImageListViewItemCollection imageListViewItemCollection, string fullFilename)
        {
            ImageListViewItem foundItem = null;

            foreach (ImageListViewItem item in imageListViewItemCollection)
            {
                if (item.FileFullPath == fullFilename)
                {
                    foundItem = item;
                    break;
                }
            }
            return foundItem;
        }
        #endregion

        #region ImageListView - ReloadThumbnail - Filename - Invoke
        private void ImageListViewReloadThumbnailAndMetadataInvoke(ImageListView imageListView, string fullFileName)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListView, string>(ImageListViewReloadThumbnailAndMetadataInvoke), imageListView, fullFileName);
                return;
            }

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            try
            {
                ImageListViewItem item = FindItemInImageListView(imageListView1.Items, fullFileName);
                if (item != null) ImageListViewReloadThumbnailInvoke(item);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                //DID ImageListe update failed, because of thread???
            }
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            //return existAndUpdated;
        }
        #endregion

        #region ImageListView - ReloadThumbnail - ImageListViewItem - Invoke
        private void ImageListViewReloadThumbnailInvoke(ImageListViewItem imageListViewItem)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListViewItem>(ImageListViewReloadThumbnailInvoke), imageListViewItem);
                return;
            }

            imageListViewItem.BeginEdit();
            imageListViewItem.Update();
            imageListViewItem.EndEdit();
        }
        #endregion

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
        private void PopulateImageListViewOpenWithToolStripThread(ImageListViewSelectedItemCollection imageListViewSelectedItems)
        {
            
            List<FileEntry> imageListViewFileEntryCopy = new List<FileEntry>();
            try
            {
                foreach (ImageListViewItem imageListViewItem in imageListViewSelectedItems)
                {
                    imageListViewFileEntryCopy.Add(new FileEntry(imageListViewItem.FileFullPath, imageListViewItem.DateModified)); //Avoid crash when items gets updated
                }
            }
            catch { }

            Thread threadPopulateOpenWith = new Thread(() => { PopulateImageListViewOpenWithToolStripInvoke(imageListViewFileEntryCopy); });
            threadPopulateOpenWith.Start();
        }
        #endregion 

        #region ImageListView - Populate - OpenWith - Invoke
        private void PopulateImageListViewOpenWithToolStripInvoke(List<FileEntry> imageListViewSelectedFileEntryItems)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<List<FileEntry>>(PopulateImageListViewOpenWithToolStripInvoke), imageListViewSelectedFileEntryItems);
                return;
            }


            if (imageListViewSelectedFileEntryItems.Count == 1) openWithDialogToolStripMenuItem.Visible = true;
            else openWithDialogToolStripMenuItem.Visible = false;

            List<string> extentions = new List<string>();

            foreach (FileEntry fileEntry in imageListViewSelectedFileEntryItems)
            {
                string extention = Path.GetExtension(fileEntry.FileFullPath).ToLower();
                if (!extentions.Contains(extention)) extentions.Add(extention);
            }

            ApplicationAssociationsHandler applicationAssociationsHandler = new ApplicationAssociationsHandler();
            List<ApplicationData> listOfCommonOpenWith = applicationAssociationsHandler.OpenWithInCommon(extentions);

            openMediaFilesWithToolStripMenuItem.DropDownItems.Clear();
            if (listOfCommonOpenWith != null && listOfCommonOpenWith.Count > 0)
            {
                openMediaFilesWithToolStripMenuItem.Visible = true;
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

                        ToolStripMenuItem toolStripMenuItemOpenWith = new ToolStripMenuItem(singelVerbApplicationData.FriendlyAppName.Replace("&", "&&") + " - " + verbLink.Verb, singelVerbApplicationData.Icon.ToBitmap());
                        toolStripMenuItemOpenWith.Tag = singelVerbApplicationData;
                        toolStripMenuItemOpenWith.Click += ToolStripMenuItemOpenWith_Click;
                        openMediaFilesWithToolStripMenuItem.DropDownItems.Add(toolStripMenuItemOpenWith);
                    }
                }
            }
            else openMediaFilesWithToolStripMenuItem.Visible = false;
        }
        #endregion

        #region ImageListView - Add - Item
        private void ImageListViewAddItem(string fullFilename)
        {
            imageListView1.Items.Add(fullFilename);
        }
        #endregion

        #region ImageListViewGetSelected
        private List<FileEntry> ImageListViewGetSelected(ImageListView imageListView)
        {
            List<FileEntry> fileEntries = new List<FileEntry>();
            foreach (ImageListViewItem imageListViewItem in imageListView.SelectedItems)
            {
                fileEntries.Add(new FileEntry(imageListViewItem.FileFullPath, imageListViewItem.DateModified));
            }
            return fileEntries;
        }
        #endregion

        #region ImageListView - Aggregate - FromSearchFilter
        private void ImageListViewAggregateWithMediaFiles(List<FileEntry> fileEntries)
        {
            //if (cacheFolderThumbnails || cacheFolderMetadatas || cacheFolderWebScraperDataSets) CacheFolder(selectedFolder, filesFoundInDirectory, recursive);
            if (Properties.Settings.Default.ImageViewLoadThumbnailOnDemandMode) imageListView1.CacheMode = CacheMode.OnDemand;
            else imageListView1.CacheMode = CacheMode.Continuous;

            ImageListViewClearAll(imageListView1);

            imageListView1.Enabled = false;
            ImageListViewSuspendLayoutInvoke(imageListView1);

            FilterVerifyer filterVerifyerFolder = new FilterVerifyer();
            int valuesCountAdded = filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeViewFilter, FilterVerifyer.Root);

            foreach (FileEntry fileEntry in fileEntries)
            {
                if (File.Exists(fileEntry.FileFullPath))
                {
                    if (valuesCountAdded > 0) // no filter values added, no need read from database, this fjust for optimize speed
                    {
                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                        if (filterVerifyerFolder.VerifyMetadata(metadata)) imageListView1.Items.Add(fileEntry.FileFullPath);
                    }
                    else imageListView1.Items.Add(fileEntry.FileFullPath);
                }
            }

            imageListView1.Enabled = true;
            ImageListViewResumeLayoutInvoke(imageListView1);
            
            StartThreads();
        }
        #endregion
        
        #region ImageListView - Aggregate - Rename Items
        private void UpdateImageViewListeAfterRename(ImageListView imageListView, Dictionary<string, string> renameSuccess, Dictionary<string, string> renameFailed, bool onlyRenameAddbackToListView)
        {
            //GlobalData.DoNotRefreshImageListView = true;
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            ImageListViewSuspendLayoutInvoke(imageListView);

            #region Remove items with old names
            foreach (string filename in renameSuccess.Keys)
            {
                ImageListViewItem foundItem = FindItemInImageListView(imageListView.Items, filename);
                if (foundItem != null) imageListView.Items.Remove(foundItem);
            }
            #endregion

            #region Add new renames back to list
            if (onlyRenameAddbackToListView)
            {
                foreach (string filename in renameSuccess.Values) imageListView.Items.Add(filename);
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

                AddError(
                        Path.GetDirectoryName(filename),
                        Path.GetFileName(filename),
                        dateTimeLastWriteTime,
                        AddErrorFileSystemRegion, AddErrorFileSystemMove, filename, renameFailed[filename],
                        "Failed rename " + filename + " to : " + renameFailed[filename]);

                ImageListViewItem foundItem = FindItemInImageListView(imageListView.Items, filename);
                if (foundItem != null) foundItem.Selected = true; 
            }
            #endregion 

            #region Select back all Items renamed
            if (onlyRenameAddbackToListView)
            {
                foreach (string filename in renameSuccess.Values)
                {
                    ImageListViewItem foundItem = FindItemInImageListView(imageListView.Items, filename);
                    if (foundItem != null) foundItem.Selected = true;
                }
            }
            #endregion 

            ImageListViewResumeLayoutInvoke(imageListView);
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
        }
    
        #endregion 
    }
}
