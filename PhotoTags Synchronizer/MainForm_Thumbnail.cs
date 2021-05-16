using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Exiftool;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;
using MetadataLibrary;
using NReco.VideoConverter;

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
                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Restart();
                thumbnailImage = databaseAndCacheThumbnail.ReadThumbnailFromCacheOrDatabase(fileEntry);
                         Logger.Trace("GetThumbnail - from database:  " + " " + stopwatch.ElapsedMilliseconds + "ms. " + (stopwatch.ElapsedMilliseconds > 500 ? " SLOW " : "") + (thumbnailImage == null ? " Found" : "") + fileEntry.FileFullPath);

                bool isFileInCloud = ExiftoolWriter.IsFileInCloud(fileEntry.FileFullPath);
                if (thumbnailImage == null)
                {
                    try
                    {
                        if (isFileInCloud) UpdateStatusAction("File is in Cloud, check if windows has thumbnail: " + fileEntry.FileFullPath);
                        else UpdateStatusAction("Read thumbnail from file: " + fileEntry.FileFullPath);
                    }
                    catch { }

                    //Was not readed from database, need to cache to database
                    stopwatch.Restart();
                    thumbnailImage = LoadMediaCoverArtThumbnail(fileEntry.FileFullPath, ThumbnailSaveSize, true, isFileInCloud);
                         Logger.Trace("GetThumbnail - from CoverArt:       " + " " + stopwatch.ElapsedMilliseconds + "ms. " + (stopwatch.ElapsedMilliseconds > 500 ? " SLOW " : "") + (thumbnailImage == null ? " Found" : "") + fileEntry.FileFullPath);

                    if (thumbnailImage != null)
                    {
                        stopwatch.Restart();
                        Image cloneBitmap = Utility.ThumbnailFromImage(thumbnailImage, ThumbnailMaxUpsize, Color.White, true); //Need create a clone, due to GDI + not thread safe
                        Logger.Trace("GetThumbnail - from read from file:  " + " " + stopwatch.ElapsedMilliseconds + "ms. " + (stopwatch.ElapsedMilliseconds > 500 ? " SLOW " : "") + (thumbnailImage == null ? " Found" : "") + fileEntry.FileFullPath);

                        AddQueueMetadataReadToCacheOrUpdateFromSoruce(fileEntry);
                        AddQueueSaveThumbnailMediaLock(new FileEntryImage(fileEntry, cloneBitmap));
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

        private static List<FileEntryImage> posterCache = new List<FileEntryImage>();
        private static readonly Object posterCacheLock = new Object();

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
                        FileEntryImage fileEntryImage = new FileEntryImage(posterCache[indexFound].FileEntry, new Bitmap(posterCache[indexFound].Image));
                        posterCache.Add(fileEntryImage); //Add last
                        posterCache.RemoveAt(indexFound);
                        image = posterCache[posterCache.Count - 1].Image;
                    }
                }
            }
            catch 
            { 
            }
            return image;
        }

        private void PosterCacheAdd(string fullFilePath, Image image)
        {
            try
            {                
                lock (posterCacheLock)
                {
                    FileEntryImage fileEntryImage = new FileEntryImage(fullFilePath, File.GetLastWriteTime(fullFilePath), image);
                    posterCache.Add(fileEntryImage); //Add last
                    if (posterCache.Count > 10) posterCache.RemoveAt(0); //Only remember last x images
                }
            }
            catch
            {
            }
        }

        #region Thumbnail - LoadMediaCoverArtPoster
        private Image LoadMediaCoverArtPoster(string fullFilePath, bool checkIfCloudFile)
        {
            Image image = PosterCacheRead(fullFilePath);
            if (image != null) return image; //Found in cache

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

                    if (memoryStream.Length > 0) image = Image.FromStream(memoryStream);
                    else image = null;
                }
            }
            else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
            {
                image = ImageAndMovieFileExtentionsUtility.LoadImage(fullFilePath);
                if (image == null) image = Utility.LoadImageWithoutLock(fullFilePath);
            }

            if (image != null) PosterCacheAdd(fullFilePath, image);
            return image;
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

                Stopwatch stopwatch = new Stopwatch();
                if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilePath))
                {
                    stopwatch.Restart();
                    WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();
                    Logger.Trace("LoadMediaCoverArtThumbnail - Init WindowsPropertyReader:  " + stopwatch.ElapsedMilliseconds + "ms. ");

                    Image image = windowsPropertyReader.GetThumbnail(fullFilePath);
                    Logger.Trace("LoadMediaCoverArtThumbnail - Windows Property:           " + " " + stopwatch.ElapsedMilliseconds + "ms. " + (stopwatch.ElapsedMilliseconds > 500 ? " SLOW " : "") + (image == null ? " Found" : "") + fullFilePath);

                    if (image != null) return image;
                    //DO NOT READ FROM FILE - IF NOT ALLOWED READ CLOUD FILES
                    if (doNotReadFullFileIfInCloud) return image; //Don't read from file
                    //DO NOT READ FROM FILE - IF NOT ALLOWED READ CLOUD FILES

                    stopwatch.Restart();
                    ExiftoolWriter.WaitLockedFileToBecomeUnlocked(fullFilePath);
                    Logger.Trace("LoadMediaCoverArtThumbnail - Wait unlock:  " + stopwatch.ElapsedMilliseconds + "ms. ");

                    image = Utility.ThumbnailFromImage(LoadMediaCoverArtPoster(fullFilePath, checkIfCloudFile), maxSize, Color.White, false);
                    Logger.Trace("LoadMediaCoverArtThumbnail - Windows Property:           " + " " + stopwatch.ElapsedMilliseconds + "ms. " + (stopwatch.ElapsedMilliseconds > 500 ? " SLOW " : "") + (image == null ? " Found" : "") + fullFilePath);
                    
                    return image;
                }
                else if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilePath))
                {
                    stopwatch.Restart();
                    WindowsProperty.WindowsPropertyReader windowsPropertyReader = new WindowsProperty.WindowsPropertyReader();
                    Logger.Trace("LoadMediaCoverArtThumbnail - Init WindowsPropertyReader:  " + stopwatch.ElapsedMilliseconds + "ms. "); 
                    
                    Image image = windowsPropertyReader.GetThumbnail(fullFilePath);
                    Logger.Trace("LoadMediaCoverArtThumbnail - Windows Property:           " + " " + stopwatch.ElapsedMilliseconds + "ms. " + (stopwatch.ElapsedMilliseconds > 500 ? " SLOW " : "") + (image == null ? " Found" : "") + fullFilePath);

                    //DO NOT READ FROM FILE - IF NOT ALLOWED READ CLOUD FILES
                    if (doNotReadFullFileIfInCloud) return image; //Don't read from file
                    //DO NOT READ FROM FILE

                    stopwatch.Restart();
                    ExiftoolWriter.WaitLockedFileToBecomeUnlocked(fullFilePath);
                    Logger.Trace("LoadMediaCoverArtThumbnail - Wait unlock:  " + stopwatch.ElapsedMilliseconds + "ms. ");
                    
                    if (image == null)
                    {
                        image = Utility.ThumbnailFromImage(ImageAndMovieFileExtentionsUtility.ThumbnailFromFile(fullFilePath, maxSize, true), maxSize, Color.White, false);
                        Logger.Trace("LoadMediaCoverArtThumbnail - MagickGeometry:             " + " " + stopwatch.ElapsedMilliseconds + "ms. " + (stopwatch.ElapsedMilliseconds > 500 ? " SLOW " : "") + (image == null ? " Found" : "") + fullFilePath);
                    }

                    stopwatch.Restart();
                    if (image == null)
                    {
                        image = Utility.ThumbnailFromFile(fullFilePath, maxSize, UseEmbeddedThumbnails.Auto, Color.White, false);
                        Logger.Trace("LoadMediaCoverArtThumbnail - Utility.ThumbnailFromFile:  " + stopwatch.ElapsedMilliseconds + "ms. " + (stopwatch.ElapsedMilliseconds > 500 ? " SLOW " : "") + (image == null ? " Found" : "") + " " + fullFilePath);
                    }
                    return image;
                }
            }
            catch (Exception ex) 
            {
                Logger.Warn(ex.Message);
            }

            return null;
        }
        #endregion
    }
}
