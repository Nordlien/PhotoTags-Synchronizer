using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DataGridViewGeneric;
using Exiftool;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;
using MetadataLibrary;
using TimeZone;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
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

                bool isFileInCloud = ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath);
                if (thumbnailImage == null)
                {
                    try
                    {
                        if (isFileInCloud) UpdateStatusAction("Read thumbnail from Cloud: " + fileEntry.FileFullPath);
                        else UpdateStatusAction("Read thumbnail from file: " + fileEntry.FileFullPath);
                    }
                    catch { }

                    //Was not readed from database, need to cache to database
                    thumbnailImage = LoadMediaCoverArtThumbnail(fileEntry.FileFullPath, ThumbnailSaveSize, true, isFileInCloud);

                    if (thumbnailImage != null)
                    {
                        Image cloneBitmap = Utility.ThumbnailFromImage(thumbnailImage, ThumbnailMaxUpsize, Color.White, true); //Need create a clone, due to GDI + not thread safe

                        AddQueueMetadataReadToCacheOrUpdateFromSoruce(fileEntry);
                        AddQueueSaveThumbnailMedia(new FileEntryImage(fileEntry, cloneBitmap));
                        thumbnailImage = Utility.ThumbnailFromImage(cloneBitmap, imageListView1.ThumbnailSize, Color.White, true, true);
                        
                    }
                    else
                    {
                        if (isFileInCloud) thumbnailImage = (Image)Properties.Resources.load_image_error_in_cloud;
                        else thumbnailImage = (Image)Properties.Resources.load_image_error_thumbnail;
                        return thumbnailImage;
                    }
                }
                
                if (isFileInCloud)
                {
                    //Bitmap bitmap = new Bitmap(imageListView1.ThumbnailSize.Width, imageListView1.ThumbnailSize.Height);
                    using (Graphics g = Graphics.FromImage(thumbnailImage))
                    {
                        g.DrawImage(Properties.Resources.FileInCloud, 0, 0);
                    }
                    //e.Graphics.DrawImage(image, e.CellBounds.Left + e.CellBounds.Width - image.Width - 1, e.CellBounds.Top + 1);
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

        public void Test()
        {
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

            //NReco.VideoConverter.FFMpegInput ffMpegInput;
            //fFMpegInput.CustomInputArgs
            //ffMpeg.ConvertMedia()
        }



        #region Thumbnail - LoadMediaCoverArtPoster
        private Image LoadMediaCoverArtPoster(string fullFilePath, bool checkIfCloudFile)
        {
            if (checkIfCloudFile && Properties.Settings.Default.AvoidOfflineMediaFiles)
            {
                if (ExiftoolWriter.IsFileInCloud(fullFilePath)) return null;
            }

            ExiftoolWriter.WaitLockedFileToBecomeUnlocked(fullFilePath);
            if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilePath))
            {

                
                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

                using (Stream memoryStream = new MemoryStream())
                {
                    ffMpeg.GetVideoThumbnail(fullFilePath, memoryStream);

                    if (memoryStream.Length > 0) return Image.FromStream(memoryStream);
                    else return null;
                }
            }
            else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
            {
                Image image = ImageAndMovieFileExtentionsUtility.LoadImage(fullFilePath);
                if (image == null) image = Utility.LoadImageWithoutLock(fullFilePath);
                return image;
            }
            
            return null;
        }
        #endregion 

        #region Thumbnail - LoadMediaCoverArtThumbnail
        private Image LoadMediaCoverArtThumbnail(string fullFilePath, Size maxSize, bool checkIfCloudFile, bool isFileInCloud = false)
        {
            try
            {
                bool doNotReadFullFileIfInCloud = false;
                //isFileInCloud = ExiftoolWriter.IsFileInCloud(fullFilePath);
                if (checkIfCloudFile && Properties.Settings.Default.AvoidOfflineMediaFiles)
                {
                    if (isFileInCloud) doNotReadFullFileIfInCloud = true; ;
                }

                if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilePath))
                {
                    WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();
                    Image image = windowsPropertyReader.GetThumbnail(fullFilePath);
                    if (image != null) return image;
                    if (doNotReadFullFileIfInCloud) return image; //Don't read from file

                    ExiftoolWriter.WaitLockedFileToBecomeUnlocked(fullFilePath);
                    return Utility.ThumbnailFromImage(LoadMediaCoverArtPoster(fullFilePath, checkIfCloudFile), maxSize, Color.White, false);
                }
                else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
                {
                    WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();
                    Image image = windowsPropertyReader.GetThumbnail(fullFilePath);
                    if (doNotReadFullFileIfInCloud) return image; //Don't read from file

                    ExiftoolWriter.WaitLockedFileToBecomeUnlocked(fullFilePath);
                    if (image == null) image = Utility.ThumbnailFromImage(ImageAndMovieFileExtentionsUtility.ThumbnailFromFile(fullFilePath, maxSize, false), maxSize, Color.White, false);
                    if (image == null) image = Utility.ThumbnailFromFile(fullFilePath, maxSize, UseEmbeddedThumbnails.Auto, Color.White, false);
                    return image;
                }
            }
            catch { }

            return null;
        }
        #endregion
    }
}
