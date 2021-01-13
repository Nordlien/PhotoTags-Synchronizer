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

        #region ImageListView - Event - imageListView1_RetrieveItemMetadataDetails - Load Item Metadata Details
        private void imageListView1_RetrieveItemMetadataDetails(object sender, RetrieveItemMetadataDetailsEventArgs e)
        {
            Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(e.FileName, File.GetLastWriteTime(e.FileName), MetadataBrokerTypes.ExifTool));

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

        #region ImageListView - Event - imageListView1_RetrieveImage Event - Load Images / Poster
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
                    UpdateImageOnFileEntryOnSelectedGrivViewInvoke(new FileEntryImage(e.FullFilePath, imageListViewItem.DateModified, e.LoadedImage)); //Also show error thumbnail
                    break;
                }
            }
            GlobalData.retrieveImageCount--;
        }
        #endregion 

        #region ImageListView - Event - imageListView1_RetrieveItemThumbnail 
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
                if (e.Thumbnail == null) e.Thumbnail = (Image)GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(fileEntry).Clone();
            } else
            {
                Logger.Warn("File not exist: " + e.FileName);
            }

            Application.DoEvents();

            GlobalData.retrieveThumbnailCount--;
        }
        #endregion

        #region ImageListView - LoadMediaCoverArtPoster
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

        #region ImageListView - LoadMediaCoverArtThumbnail
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
                ImageListViewSuspendLayoutInvoke(imageListView);
                foreach (ImageListViewItem item in imageListView.SelectedItems)
                {
                    if (item.FileFullPath == fullFileName)
                    {
                        //existAndUpdated = true;
                        ImageListViewReloadThumbnailInvoke(item);
                        break;
                    }
                }
                ImageListViewResumeLayoutInvoke(imageListView);
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

            //imageListView.SuspendLayout(); //When this where, it crash, need debug why, this needed to avoid flashing
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

        #region ImageListeView - Populate - OpenWith - Thread

        private void PopulateImageListeViewOpenWithToolStripThread(ImageListViewSelectedItemCollection imageListViewSelectedItems)
        {
            Thread threadPopulateOpenWith = new Thread(() => { PopulateImageListeViewOpenWithToolStripInvoke(imageListView1.SelectedItems); });
            threadPopulateOpenWith.Start();
        }
        #endregion 

        #region ImageListeView - Populate - OpenWith - Invoke
        private void PopulateImageListeViewOpenWithToolStripInvoke(ImageListViewSelectedItemCollection imageListViewSelectedItems)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ImageListViewSelectedItemCollection>(PopulateImageListeViewOpenWithToolStripInvoke), imageListViewSelectedItems);
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
    }
}
