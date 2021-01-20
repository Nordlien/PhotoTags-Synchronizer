using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Manina.Windows.Forms;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using System.Diagnostics;
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

        #region ImageListView - Event - Retrieve Metadata
        private void imageListView1_RetrieveItemMetadataDetails(object sender, RetrieveItemMetadataDetailsEventArgs e)
        {
            Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(e.FileName, File.GetLastWriteTime(e.FileName), MetadataBrokerType.ExifTool));

            Application.DoEvents();
            try
            {
                if (metadata == null || metadata.FileName == null)
                {
                    Utility.ShellImageFileInfo shellImageFileInfo = new Utility.ShellImageFileInfo();
                    shellImageFileInfo.ReadShellImageFileInfo(e.FileName);
                    e.FileMetadata = shellImageFileInfo;

                }
                else
                {
                    e.FileMetadata = new Utility.ShellImageFileInfo();
                    e.FileMetadata.LastAccessTime = (DateTime)metadata.FileLastAccessed;
                    e.FileMetadata.CreationTime = (DateTime)metadata.FileDateCreated;
                    e.FileMetadata.LastWriteTime = (DateTime)metadata.FileDateModified;
                    e.FileMetadata.Size = (long)metadata.FileSize;
                    e.FileMetadata.TypeName = metadata.FileMimeType;
                    e.FileMetadata.DirectoryName = metadata.FileDirectory;
                    if (metadata.MediaWidth != null && metadata.MediaHeight != null) e.FileMetadata.Dimensions = new Size((int)metadata.MediaWidth, (int)metadata.MediaHeight);

                    // Exif tags
                    e.FileMetadata.ImageDescription = metadata.PersonalDescription;
                    e.FileMetadata.EquipmentModel = metadata.CameraModel;
                    if (metadata.MediaDateTaken != null) e.FileMetadata.DateTaken = (DateTime)metadata.MediaDateTaken;
                    e.FileMetadata.Artist = metadata.PersonalAuthor;
                    e.FileMetadata.UserComment = metadata.PersonalComments;
                }
            } catch 
            {
                e.FileMetadata.DisplayName = Path.GetFileName(e.FileName);
                e.FileMetadata.DirectoryName = Path.GetDirectoryName(e.FileName);
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
            GlobalData.retrieveThumbnailCount++; //Counter to keey track of active threads. Can't quit application before thread empty

            if (File.Exists(e.FileName))
            {
                FileEntry fileEntry = new FileEntry(e.FileName, File.GetLastWriteTime(e.FileName));
                if (e.Thumbnail == null) e.Thumbnail = new Bitmap(GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(fileEntry));
            }
            else
            {
                Logger.Warn("File not exist: " + e.FileName);
            }

            Application.DoEvents();

            GlobalData.retrieveThumbnailCount--;
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

            GlobalData.retrieveImageCount++; //Counter to keey track of active threads. Can't quit application before thread empty
            bool retry = false;
            int retryCount = 3; //In case of waiting for OneDrive to load and timeout 

            do
            {
                try
                {
                    Image fullSizeImage = LoadMediaCoverArtPoster(e.FullFilePath, true);
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

            foreach (ImageListViewItem imageListViewItem in imageListView1.Items)
            {
                if (imageListViewItem.FileFullPath == e.FullFilePath) //Only to find DateTime Modified in the stored in the ImageListView
                {
                    UpdateImageOnFileEntryOnSelectedGrivViewInvoke(new FileEntryAttribute(e.FullFilePath, imageListViewItem.DateModified, FileEntryVersion.Current), e.LoadedImage); //Also show error thumbnail
                    break;
                }
            }
            GlobalData.retrieveImageCount--;
        }
        #endregion 

        #region ImageListView - Event - SelectionChanged -> FileSelected
        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            imageListView1.Enabled = false; //When Enabled = true, slection was cancelled during Updating the grid
            FilesSelected();
            imageListView1.Enabled = true;
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
        private void ImageListViewReloadThumbnailInvoke(ImageListView imageListView, string fullFileName)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListView, string>(ImageListViewReloadThumbnailInvoke), imageListView, fullFileName);
                return;
            }

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            try
            {
                ImageListViewItem item = FindItemInImageListView(imageListView1.Items, fullFileName);
                if (item != null) ImageListViewReloadThumbnailInvoke(item);
            }
            catch
            {
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

            imageListView.SuspendLayout(); //When this where, it crash, need debug why, this needed to avoid flashing
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

            //imageListView.ResumeLayout(); //When this where, it crash, need debug why, this needed to avoid flashing
        }
        #endregion

        #region ImageListView - Populate - OpenWith - Thread
        private void PopulateImageListViewOpenWithToolStripThread(ImageListViewSelectedItemCollection imageListViewSelectedItems)
        {
            Thread threadPopulateOpenWith = new Thread(() => { PopulateImageListViewOpenWithToolStripInvoke(imageListView1.SelectedItems); });
            threadPopulateOpenWith.Start();
        }
        #endregion 

        #region ImageListView - Populate - OpenWith - Invoke
        private void PopulateImageListViewOpenWithToolStripInvoke(ImageListViewSelectedItemCollection imageListViewSelectedItems)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListViewSelectedItemCollection>(PopulateImageListViewOpenWithToolStripInvoke), imageListViewSelectedItems);
                return;
            }

            if (imageListViewSelectedItems.Count == 1) openWithDialogToolStripMenuItem.Visible = true;
            else openWithDialogToolStripMenuItem.Visible = false;

            List<string> extentions = new List<string>();
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectedItems)
            {
                string extention = Path.GetExtension(imageListViewItem.FileFullPath).ToLower();
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

        #region ImageListView - Aggregate - FromSearchFilter
        private void ImageListViewAggregateFromSearchFilter(List<string> searchFilterResult)
        {
            ImageListViewClearAll(imageListView1);

            imageListView1.Enabled = false;
            ImageListViewSuspendLayoutInvoke(imageListView1);

            //bool isAndBetweenFieldTagsFolder = treeViewFilter.Nodes[FilterVerifyer.Root].Checked;
            FilterVerifyer filterVerifyerFolder = new FilterVerifyer();
            int valuesCountAdded = filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeViewFilter, FilterVerifyer.Root);

            foreach (string fileFullPath in searchFilterResult)
            {
                if (File.Exists(fileFullPath))
                {
                    if (valuesCountAdded > 0) // no filter values added, no need read from database, this fjust for optimize speed
                    {
                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileFullPath, File.GetLastWriteTime(fileFullPath), MetadataBrokerType.ExifTool));
                        if (filterVerifyerFolder.VerifyMetadata(metadata)) imageListView1.Items.Add(fileFullPath);
                    }
                    else imageListView1.Items.Add(fileFullPath);
                }
            }
            ImageListViewResumeLayoutInvoke(imageListView1);
            imageListView1.Enabled = true;

            StartThreads();
        }
        #endregion

        #region ImageListView - Aggregate - WithFilesFromFolder
        //Folder selected after Form load/init, click new folder and clear cache and re-read folder
        private void ImageListViewAggregateWithFilesFromFolder(string selectedFolder, bool recursive)
        {
            if (Directory.Exists(selectedFolder))
            {
                ClearQueueExiftool();

                //-------- FolderSelected_AddFilesImageListView -------------
                Properties.Settings.Default.LastFolder = selectedFolder;
                Properties.Settings.Default.Save();
                FileEntryImage[] filesFoundInDirectory;

                FilterVerifyer filterVerifyerFolder = new FilterVerifyer();
                int valuesCountAdded = filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeViewFilter, FilterVerifyer.Root);

                filesFoundInDirectory = ImageAndMovieFileExtentionsUtility.ListAllMediaFiles(selectedFolder, recursive);

                if (Properties.Settings.Default.ImageViewLoadThumbnailOnDemandMode) imageListView1.CacheMode = CacheMode.OnDemand;
                imageListView1.CacheMode = CacheMode.Continuous;

                ImageListViewClearAll(imageListView1);

                imageListView1.Enabled = false;
                ImageListViewSuspendLayoutInvoke(imageListView1);

                for (int fileNumber = 0; fileNumber < filesFoundInDirectory.Length; fileNumber++)
                {
                    if (valuesCountAdded > 0) // no filter values added, no need read from database, this just for optimize speed
                    {
                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(filesFoundInDirectory[fileNumber], MetadataBrokerType.ExifTool));
                        if (filterVerifyerFolder.VerifyMetadata(metadata)) imageListView1.Items.Add(filesFoundInDirectory[fileNumber].FileFullPath);
                    }
                    else imageListView1.Items.Add(filesFoundInDirectory[fileNumber].FileFullPath);
                }

                ImageListViewResumeLayoutInvoke(imageListView1);
                imageListView1.Enabled = true;
                //-------- FolderSelected_AddFilesImageListView -------------

                GlobalData.lastReadFolderWasRecursive = recursive;

                StartThreads();
            }
        }
        #endregion

        #region ImageListView - Rename Items
        private void UpdateImageViewListeAfterRename(ImageListView imageListView, Dictionary<string, string> renameSuccess, Dictionary<string, string> renameFailed, bool onlyRenameAddbackToListView)
        {
            /*if (InvokeRequired)
            {
                this.BeginInvoke(new Action<Dictionary<string, string>, Dictionary<string, string>, bool>(UpdateImageViewListeAfterRename), renameSuccess, renameFailed, onlyRenameAddbackToListView);
                return;
            }*/

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

        #region Thumbnail - Get Thumbnail And WriteNewToDatabase - AddQueueAllUpadtedFileEntry
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
                thumbnailImage = databaseAndCacheThumbnail.ReadThumbnailFromCacheOrDatabase(fileEntry);

                if (thumbnailImage == null)
                {
                    try
                    {
                        if (ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath)) UpdateStatusAction("Cloud file: " + fileEntry.FileFullPath);
                        else UpdateStatusAction("Read thumbnail for file: " + fileEntry.FileFullPath);
                    }
                    catch { }

                    //Was not readed from database, need to cache to database
                    thumbnailImage = LoadMediaCoverArtThumbnail(fileEntry.FileFullPath, ThumbnailSaveSize, true);

                    if (thumbnailImage != null)
                    {
                        Image cloneBitmap = new Bitmap(thumbnailImage); //Need create a clone, due to GDI + not thread safe
                        AddQueueAllUpdatedFileEntry(new FileEntryImage(fileEntry, cloneBitmap));
                        thumbnailImage = Manina.Windows.Forms.Utility.ThumbnailFromImage(thumbnailImage, ThumbnailMaxUpsize, Color.White, true);
                    }
                    else
                    {
                        if (ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath)) thumbnailImage = (Image)Properties.Resources.load_image_error_onedrive;
                        else
                        {
                            Logger.Warn("Was not able to get thumbnail from file: " + fileEntry.FileFullPath);
                            thumbnailImage = (Image)Properties.Resources.load_image_error_general;
                        }
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
        #endregion

        #region Thumbnail - LoadMediaCoverArtPoster
        private Image LoadMediaCoverArtPoster(string fullFilePath, bool checkIfCloudFile)
        {
            if (checkIfCloudFile && Properties.Settings.Default.AvoidOfflineMediaFiles)
            {
                if (ExiftoolWriter.IsFileInCloud(fullFilePath)) return null;
            }

            if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilePath))
            {
                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                //ffMpeg.ConvertProgress += FfMpeg_ConvertProgress;
                //ffMpeg.LogReceived += FfMpeg_LogReceived;
                Stream memoryStream = new MemoryStream();
                ffMpeg.GetVideoThumbnail(fullFilePath, memoryStream);

                if (memoryStream.Length > 0)
                    return Image.FromStream(memoryStream);
                else
                    return null;
            }
            else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
            {
                return Manina.Windows.Forms.Utility.LoadImageWithoutLock(fullFilePath);
            }
            else
                return null;
        }
        #endregion 

        #region Thumbnail - LoadMediaCoverArtThumbnail
        private Image LoadMediaCoverArtThumbnail(string fullFilePath, Size maxSize, bool checkIfCloudFile)
        {
            try
            {
                bool doNotReadFullFile = false;
                if (checkIfCloudFile && Properties.Settings.Default.AvoidOfflineMediaFiles)
                {
                    if (Properties.Settings.Default.AvoidOfflineMediaFiles)
                    {
                        if (ExiftoolWriter.IsFileInCloud(fullFilePath)) doNotReadFullFile = true; ;
                    }
                }

                if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilePath))
                {
                    WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();
                    Image image = windowsPropertyReader.GetThumbnail(fullFilePath);
                    if (image != null) return image;

                    if (doNotReadFullFile) return image;

                    return Utility.ThumbnailFromImage(LoadMediaCoverArtPoster(fullFilePath, checkIfCloudFile), maxSize, Color.White, false);
                }
                else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
                {
                    WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();
                    Image image = windowsPropertyReader.GetThumbnail(fullFilePath);
                    if (image != null) return image;

                    if (doNotReadFullFile) return image;
                    return Utility.ThumbnailFromFile(fullFilePath, maxSize, UseEmbeddedThumbnails.Auto, Color.White);
                }
            }
            catch { }

            return null;
        }
        #endregion

    }
}
