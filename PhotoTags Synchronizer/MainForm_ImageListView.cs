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

namespace PhotoTagsSynchronizer
{
    
    public partial class MainForm : Form
    {
        private bool IsFileInCloud(string fullFileName)
        {
            /*
            FILE_ATTRIBUTE_RECALL_ON_DATA_ACCESS
            4194304 (0x400000)
            When this attribute is set, it means that the file or directory is not fully present locally. For a file that means that not all of its data is on local storage (e.g. it may be sparse with some data still in remote storage). For a directory it means that some of the directory contents are being virtualized from another location. Reading the file / enumerating the directory will be more expensive than normal, e.g. it will cause at least some of the file/directory content to be fetched from a remote store. Only kernel-mode callers can set this bit.
    
            1048576 (0x100000) Unknown flag
            */
            try
            {
                FileAttributes fileAttributes = File.GetAttributes(fullFileName);
                if ((((int)fileAttributes) & 0x000400000) == 0x000400000)
                    return true; 
            } catch { }
            return false; 
        }

        #region Load Media Full Cover Art Poster or Thumbnail
        private Image LoadMediaCoverArtPoster(string fullFilePath, bool checkIfCloudFile)
        {
            //bool doNotReadFullFile = false;
            if (checkIfCloudFile && Properties.Settings.Default.AvoidOfflineMediaFiles)
            {
                if (IsFileInCloud(fullFilePath)) return null;
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

        private Image LoadMediaCoverArtThumbnail(string fullFilePath, Size maxSize, bool checkIfCloudFile)
        {
            try
            {
                bool doNotReadFullFile = false;
                if (checkIfCloudFile && Properties.Settings.Default.AvoidOfflineMediaFiles)
                {
                    if (Properties.Settings.Default.AvoidOfflineMediaFiles)
                    {
                        if (IsFileInCloud(fullFilePath)) doNotReadFullFile = true; ;
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

        #region Load Item Metadata Details
        private void imageListView1_RetrieveItemMetadataDetails(object sender, RetrieveItemMetadataDetailsEventArgs e)
        {
            Metadata metadata = databaseAndCacheMetadataExiftool.MetadataCacheRead(
                new FileEntryBroker(e.FileName, File.GetLastWriteTime(e.FileName), MetadataBrokerTypes.ExifTool));

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


        #region Load Images and create Thumbnail from picture and videos


        private AutoResetEvent WaitCacheEmpty = null; //When out of mempry, then wait for all data ready = new AutoResetEvent(false);

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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                try
                {
                    Image fullSizeImage = LoadMediaCoverArtPoster(e.FullFilePath, true);
                    e.LoadedImage = fullSizeImage;
                    e.WasImageReadFromFile = true;
                    e.DidErrorOccourLoadMedia = false;
                }
                #region Error handling
                //This is only error handling
                //1. When I was using 32bit version, I got lot of our of memory
                //2. When OneDrive had chrased, lot of stranger errors occured
                catch (OutOfMemoryException)
                {
                    e.LoadedImage = (Image)Properties.Resources.load_image_error_memory;
                    e.WasImageReadFromFile = false;
                    e.DidErrorOccourLoadMedia = true;
                    
                    WaitCacheEmpty = new AutoResetEvent(false);
                    WaitCacheEmpty.WaitOne(10000);
                    lock (WaitCacheEmpty)
                    {
                        WaitCacheEmpty = null;
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
                if (e.LoadedImage == null && IsFileInCloud(e.FullFilePath))
                {
                    e.LoadedImage = (Image)Properties.Resources.load_image_error_onedrive;
                    e.WasImageReadFromFile = false;
                    e.DidErrorOccourLoadMedia = true;
                }
                #endregion
            } while (retry);

            if (stopwatch.ElapsedMilliseconds > 10) Logger.Info("imageListView1_RetrieveImage {0}ms ", stopwatch.ElapsedMilliseconds);

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

        /// <summary>
        /// Occures when ImageListView need to "load" thumbnail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageListView1_RetrieveItemThumbnail(object sender, RetrieveItemThumbnailEventArgs e)
        {
            //Console.WriteLine("imageListView1_RetrieveItemThumbnail: " + (GlobalData.IsApplicationClosing ? "AppClosing" : "") + (GlobalData.IsDragAndDropActive ? "IsDragAndDropActive" : ""));

            if (GlobalData.IsApplicationClosing) return;
            if (GlobalData.DoNotRefreshImageListView) return;
            if (imageListView1.IsDisposed) return;
            GlobalData.retrieveThumbnailCount++; //Counter to keey track of active threads. Can't quit application before thread empty

            Console.WriteLine("imageListView1_RetrieveItemThumbnail " + GlobalData.retrieveThumbnailCount + " " + e.FileName);
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

        private void imageListView1_ItemDoubleClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ApplicationActivation.ProcessRunOpenFile(e.Item.FileFullPath);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to start application process...", MessageBoxButtons.OK);
            }
        }

    }
}
