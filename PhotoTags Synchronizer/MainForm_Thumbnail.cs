using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;
using MetadataLibrary;
using FileHandeling;
using Krypton.Toolkit;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
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
        private Image GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(FileEntry fileEntry, bool dontReadFilesInCloud, FileStatus fileStatus)
        {
            FileEntryVersion fileEntryVersion = FileEntryVersion.ExtractedNowUsingExiftool;

            Image thumbnailImage = null;
            try
            {
                thumbnailImage = databaseAndCacheThumbnailPoster.ReadThumbnailFromCacheOrDatabase(fileEntry);
                if (thumbnailImage != null) fileEntryVersion = FileEntryVersion.CurrentVersionInDatabase;

                if (thumbnailImage == null) //Was not read from database or cache
                {
                    if (fileStatus.IsInCloudOrVirtualOrOffline) UpdateStatusAction("File is in Cloud, check if windows has thumbnail: " + fileEntry.FileFullPath);
                    else UpdateStatusAction("Read thumbnail from file: " + fileEntry.FileFullPath);

                    thumbnailImage = LoadMediaCoverArtThumbnail(fileEntry.FileFullPath, ThumbnailSaveSize, fileStatus);

                    if (thumbnailImage != null)
                    {
                        databaseAndCacheThumbnailPoster.ThumbnailCacheUpdate(fileEntry, new Bitmap(thumbnailImage)); //Remember the Thumbnail, before Save, for show in DataGridView etc., no need to load again                        
                        DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute(new FileEntryAttribute(fileEntry, fileEntryVersion), new Bitmap(thumbnailImage));
                        DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute(new FileEntryAttribute(fileEntry, FileEntryVersion.Error), new Bitmap(thumbnailImage));
                    }
                    else
                    {
                        //Start downloading in background from OneDrive
                        if (!dontReadFilesInCloud && fileStatus.IsInCloudOrVirtualOrOffline)
                            FileHandler.TouchOfflineFileToGetFileOnline(fileEntry.FileFullPath);
                    }

                    AddQueueSaveToDatabaseMediaThumbnailLock(
                        new FileEntryImage(
                            fileEntry, thumbnailImage == null ? null : new Bitmap(thumbnailImage),
                            !dontReadFilesInCloud
                            ));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            return thumbnailImage;
        }
        #endregion

        #region Cache Poster
        private static List<FileEntryImage> posterCache = new List<FileEntryImage>();
        private static readonly Object posterCacheLock = new Object();

        #region Image PosterCacheRead
        private Image PosterCacheRead(string fullFileName)
        {
            Image image = null;
            try
            {
                int indexFound = -1;
                FileEntry fileEntry = new FileEntry(fullFileName, FileHandler.GetLastWriteTime(fullFileName, waitAndRetry: IsFileInTemporaryUnavailableLock(fullFileName)));

                lock (posterCacheLock)
                {
                    for (int index = 0; index < posterCache.Count; index++)
                    {
                        if (posterCache[index] == fileEntry)
                        {
                            indexFound = index;
                            break;
                        }
                    }

                    if (indexFound > -1)
                    {
                        //new new Bitmap to make it thraadsafe https://stackoverflow.com/questions/49679693/c-sharp-crashes-with-parameter-is-not-valid-when-setting-picturebox-image-to
                        FileEntryImage fileEntryImage = new FileEntryImage(posterCache[indexFound].FileEntry, posterCache[indexFound].Image);
                        posterCache.Add(fileEntryImage); //Add last
                        posterCache.RemoveAt(indexFound);
                        image = new Bitmap(posterCache[posterCache.Count - 1].Image);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "PosterCacheRead");
            }
            return image;
        }
        #endregion

        #region PosterCacheAdd
        private void PosterCacheAdd(string fullFilePath, Image image)
        {
            try
            {                
                lock (posterCacheLock)
                {
                    FileEntryImage fileEntryImage = new FileEntryImage(fullFilePath, FileHandler.GetLastWriteTime(fullFilePath, waitAndRetry: true), new Bitmap(image));
                    //new new Bitmap to make it thraadsafe https://stackoverflow.com/questions/49679693/c-sharp-crashes-with-parameter-is-not-valid-when-setting-picturebox-image-to
                    posterCache.Add(fileEntryImage); //Add last
                    if (posterCache.Count > 10) posterCache.RemoveAt(0); //Only remember last x images
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "PosterCacheAdd");
            }
        }
        #endregion

        #endregion 

        #region Thumbnail - LoadMediaCoverArtPoster
        private Image LoadMediaCoverArtPosterWithCache(string fullFilePath) //, bool dontReadFilesInCloud, bool isFileInCloud)
        {
            Image image = PosterCacheRead(fullFilePath);
            if (image != null) return image; //Found in cache

            try
            {
                if (File.Exists(fullFilePath)) //Files can always be moved, deleted or become change outside application (Also may occure when this Rename files)
                {
                    if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilePath))
                    {
                        //--- Get Video Thumbnail - Alternative 1
                        var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                        using (Stream memoryStream = new MemoryStream())
                        {
                            ffMpeg.GetVideoThumbnail(fullFilePath, memoryStream);

                            if (memoryStream.Length > 0) image = Image.FromStream(memoryStream);
                            else image = null;
                        }

                        //--- Get Video Thumbnail - Alternative 2
                        //if (!FileHandler.IsFileLockedForRead(fullFilePath, 100))
                        //    image = ImageAndMovieFileExtentionsUtility.GetVideoThumbnail(fullFilePath);

                    }
                    else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
                    {
                        bool wasFileLocked = false;
                        image = ImageAndMovieFileExtentionsUtility.LoadImage(fullFilePath, out wasFileLocked);
                        if (image == null && wasFileLocked && File.Exists(fullFilePath))
                        {
                            image = ImageAndMovieFileExtentionsUtility.LoadImage(fullFilePath, out wasFileLocked);
                        }
                        //if (image == null) image = Utility.LoadImageWithoutLock(fullFilePath);
                    }
                }
            } catch (Exception ex)
            {
                Logger.Warn(ex, "LoadMediaCoverArtPoster was not able to create poster of the file " + fullFilePath);
            }
            if (image != null) PosterCacheAdd(fullFilePath, image);
            return image;
        }
        #endregion 

        #region Thumbnail - LoadMediaCoverArtThumbnail
        static object windowsPropertyWindowsPropertyReaderLock = new object();
        private Image LoadMediaCoverArtThumbnail(string fullFilePath, Size maxSize, FileStatus fileStatus)
        {
            Image image = null;
            try
            {
                if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilePath))
                {
                    #region Load Video Thumbnail Poster
                    lock (windowsPropertyWindowsPropertyReaderLock)
                    {
                        try
                        {
                            WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();
                            image = windowsPropertyReader.GetThumbnail(fullFilePath);
                        }
                        catch { }
                    }

                    //DO NOT READ FROM FILE - WHEN NOT ALLOWED TO READ CLOUD FILES
                    if (image == null && !fileStatus.IsInCloudOrVirtualOrOffline) image = LoadMediaCoverArtPosterWithCache(fullFilePath);
                    #endregion
                }
                else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
                {
                    #region Load Picture Thumbnail Poster
                    WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();
                    image = windowsPropertyReader.GetThumbnail(fullFilePath);
                    
                    //DO NOT READ FROM FILE - WHEN NOT ALLOWED TO READ CLOUD FILES
                    if (image == null && !fileStatus.IsInCloudOrVirtualOrOffline)
                    {
                        image = ImageAndMovieFileExtentionsUtility.ThumbnailFromFile(fullFilePath); //Fast version - onlt load thumbnail from file
                        if (image == null ) image = LoadMediaCoverArtPosterWithCache(fullFilePath); //Slow loading, load full image
                    }
                    #endregion
                }
            }
            catch (Exception ex) 
            {
                Logger.Error(ex, "LoadMediaCoverArtThumbnail");
            }

            if (image != null) image = Utility.ConvertImageToThumbnail(image, maxSize, Color.White, false);
            return image;
        }
        #endregion
    }


}
