using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Exiftool;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;
using MetadataLibrary;
using FileHandeling;

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
        private Image GetThumbnailFromDatabaseUpdatedDatabaseIfNotExist(FileEntry fileEntry, bool dontReadFilesInCloud, bool isFileInCloud)
        {

            Image thumbnailImage;
            thumbnailImage = databaseAndCacheThumbnail.ReadThumbnailFromCacheOrDatabase(fileEntry);

            if (thumbnailImage == null) //Was not read from database or cache
            {
                if (isFileInCloud) UpdateStatusAction("File is in Cloud, check if windows has thumbnail: " + fileEntry.FileFullPath);
                else UpdateStatusAction("Read thumbnail from file: " + fileEntry.FileFullPath);

                thumbnailImage = LoadMediaCoverArtThumbnail(fileEntry.FileFullPath, ThumbnailSaveSize, dontReadFilesInCloud, isFileInCloud);

                if (thumbnailImage != null)
                {
                    databaseAndCacheThumbnail.ThumbnailCacheUpdate(fileEntry, new Bitmap(thumbnailImage)); //Remember the Thumbnail, before Save, for show in DataGridView etc., no need to load again                        
                    AddQueueLazyLoadingDataGridViewMetadataReadToCacheOrUpdateFromSoruce(fileEntry);
                    AddQueueSaveThumbnailMediaLock(new FileEntryImage(fileEntry, new Bitmap(thumbnailImage))); 

                    UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(new FileEntryAttribute(fileEntry, FileEntryVersion.Current), new Bitmap(thumbnailImage));
                    UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(new FileEntryAttribute(fileEntry, FileEntryVersion.Error), new Bitmap(thumbnailImage));
                }
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
                FileEntry fileEntry = new FileEntry(fullFileName, File.GetLastWriteTime(fullFileName));

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
                        FileEntryImage fileEntryImage = new FileEntryImage(posterCache[indexFound].FileEntry, posterCache[indexFound].Image /*new Bitmap(posterCache[indexFound].Image)*/);
                        posterCache.Add(fileEntryImage); //Add last
                        posterCache.RemoveAt(indexFound);
                        image = posterCache[posterCache.Count - 1].Image;
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
                    FileEntryImage fileEntryImage = new FileEntryImage(fullFilePath, File.GetLastWriteTime(fullFilePath), new Bitmap(image));
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
        private Image LoadMediaCoverArtPoster(string fullFilePath) //, bool dontReadFilesInCloud, bool isFileInCloud)
        {
            Image image = PosterCacheRead(fullFilePath);
            if (image != null) return image; //Found in cache

            try
            {
                FileHandler.WaitLockedFileToBecomeUnlocked(fullFilePath);
                if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilePath))
                {
                    var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

                    using (Stream memoryStream = new MemoryStream())
                    {
                        ffMpeg.GetVideoThumbnail(fullFilePath, memoryStream);

                        if (memoryStream.Length > 0) image = Image.FromStream(memoryStream);
                        else image = null;
                    }
                }
                else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
                {
                    image = ImageAndMovieFileExtentionsUtility.LoadImage(fullFilePath);
                    if (image == null) image = Utility.LoadImageWithoutLock(fullFilePath);
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
        private Image LoadMediaCoverArtThumbnail(string fullFilePath, Size maxSize, bool dontReadFilesInCloud, bool isFileInCloud = false)
        {
            Image image = null;
            try
            {
                bool doNotReadFullFileItsInCloud = false;
                
                if (isFileInCloud && dontReadFilesInCloud)
                {
                    doNotReadFullFileItsInCloud = true; 
                }

                if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilePath))
                {
                    WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();                    
                    image = windowsPropertyReader.GetThumbnail(fullFilePath);
                    
                    //DO NOT READ FROM FILE - IF NOT ALLOWED READ CLOUD FILES
                    if (image == null && !doNotReadFullFileItsInCloud)
                    {                        
                        FileHandler.WaitLockedFileToBecomeUnlocked(fullFilePath);
                        image = LoadMediaCoverArtPoster(fullFilePath);
                    }
                }
                else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
                {
                    WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();
                    image = windowsPropertyReader.GetThumbnail(fullFilePath);

                    if (image == null && !doNotReadFullFileItsInCloud)
                    {
                        FileHandler.WaitLockedFileToBecomeUnlocked(fullFilePath);
                        image = ImageAndMovieFileExtentionsUtility.ThumbnailFromFile(fullFilePath);
                    }

                    //DO NOT READ FROM FILE - IF NOT ALLOWED READ CLOUD FILES
                    if (image == null && !doNotReadFullFileItsInCloud)
                    {
                        FileHandler.WaitLockedFileToBecomeUnlocked(fullFilePath);
                        image = LoadMediaCoverArtPoster(fullFilePath); 
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.Error(ex, "LoadMediaCoverArtThumbnail");
            }

            if (image != null) image = Utility.ThumbnailFromImage(image, maxSize, Color.White, false);
            return image;
        }
        #endregion
    }
}
