using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ApplicationAssociations;
using DataGridViewGeneric;
using Exiftool;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;
using MetadataLibrary;
using NReco.VideoConverter;
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

        public void Test(List<string> files, string outputFile)
        {


            //Slideshow documentation: https://trac.ffmpeg.org/wiki/Slideshow 
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

            //ffMpeg.ConvertMedia(this.Video, null, videoSalida1 + ".mp4", null, 
            //  new NReco.VideoConverter.ConvertSettings() { 
            // AudioCodec = "mp3 -ab 128k", 
            // VideoCodec = "libx264", 
            // CustomInputArgs = "-filter_complex \"[0] yadif=0:-1:0,scale=iw*sar:ih,scale='if(gt(a,16/9),1080,-2)':'if(gt(a,16/9),-2,720)'[scaled];[scaled] pad=1080:720:(ow-iw)/2:(oh-ih)/2:black \"" });

            //ffmpeg -y -i audio.wav -f concat -i input.txt -framerate 1/2 -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1" -c:v libx264 -crf 14 -r 25 -pix_fmt yuv420p -shortest output3.mp4

            /*
            ffMpeg.ConvertMedia(this.Video //+ ".mov"/, 
                null, // autodetect by input file extension 
                outPutVideo1 + ".mp4", 
                null, // autodetect by output file extension 
                new NReco.VideoConverter.ConvertSettings()
                {
                    CustomOutputArgs = " -filter_complex \"[0] yadif=0:-1:0,scale=iw*sar:ih,scale='if(gt(a,16/9),1280,-2)':'if(gt(a,16/9),-2,720)'[scaled];[scaled] pad=1280:720:(ow-iw)/2:(oh-ih)/2:black \" -c:v libx264 -c:a mp3 -ab 128k "
                }
            );
            */

            //ffmpeg -y -i audio.wav -f concat -i input.txt -framerate 1/2 -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1" -c:v libx264 -crf 14 -r 25 -pix_fmt yuv420p
            
            /*
            var ffMpegConverter = new FFMpegConverter();
            try
            {
                ffMpegConverter.ConcatMedia(files.ToArray(), outputFile, NReco.VideoConverter.Format.mp4, new ConcatSettings()
                {
                    CustomOutputArgs = "-i \"c:\\Users\\nordl\\OneDrive\\Pictures JTNs OneDrive\\TestTags\\audio.wav\" concat -i input.txt -framerate 1/2 -vf \"scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1\" -c:v libx264 -crf 14 -r 25 -pix_fmt yuv420p"
                    //CustomOutputArgs = "-framerate 1/2 -vf \"scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1\" -c:v libx264 -crf 14 -r 25 -pix_fmt yuv420p"
                }
                ); 
            } catch { }
            */

            string tempFilename = Exiftool.ExiftoolWriter.GetTempArguFileFullPath();

            using (StreamWriter sw = new StreamWriter(tempFilename, false, new UTF8Encoding(false)))
            {
                foreach (string file in files)
                {
                    sw.WriteLine("file '" + file + "'");
                    sw.WriteLine("duration 2");
                }
            }

            #region ffMpeg Write
            String path = NativeMethods.GetFullPathOfExeFile("ffmpeg.exe");
            string arguments = "-y -i \"c:\\Users\\nordl\\OneDrive\\Pictures JTNs OneDrive\\TestTags\\audio.wav\" -f concat -safe 0 -i \"" + 
                NativeMethods.ShortFileName(tempFilename) + "\" -framerate 1/2 -vf \"scale =1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1\" -c:v libx264 -crf 14 -r 25 -pix_fmt yuv420p -shortest \"c:\\Users\\nordl\\OneDrive\\Pictures JTNs OneDrive\\TestTags\\test.mp4\"";
            
            bool hasExiftoolErrorMessage = false;
            string exiftoolOutput = "";

            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    StandardOutputEncoding = Encoding.UTF8
                }
            })
            {
                bool result = process.Start();
                string line;

                while (!process.StandardError.EndOfStream)
                {
                    line = process.StandardError.ReadLine();
                    exiftoolOutput += line + "\r\n";
                    if (!line.StartsWith("Warning")) hasExiftoolErrorMessage = true;
                    Logger.Error("EXIFTOOL WRITE ERROR: " + line);
                }

                while (!process.StandardOutput.EndOfStream)
                {
                    line = process.StandardOutput.ReadLine();
                    exiftoolOutput += line + "\r\n";
                    if (line.StartsWith("Error")) hasExiftoolErrorMessage = true;
                    Logger.Info("EXIFTOOL WRITE OUTPUT: " + line);
                }

                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    hasExiftoolErrorMessage = true;
                    Logger.Info("process.WaitForExit() " + process.ExitCode);
                }

                while (!process.HasExited) Thread.Sleep(100);

                process.Close();
                process.Dispose();
            }
            if (hasExiftoolErrorMessage) MessageBox.Show(exiftoolOutput);
            #endregion 

            /*
            //foreach 
            string inputFile = files[0];
            
            ffMpeg.ConvertMedia(inputFile,
                null, // autodetect by input file extension 
                outputFile,
                null, // autodetect by output file extension 
                new NReco.VideoConverter.ConvertSettings()
                {
                    CustomOutputArgs = " -filter_complex \"[0] yadif=0:-1:0,scale=iw*sar:ih,scale='if(gt(a,16/9),1280,-2)':'if(gt(a,16/9),-2,720)'[scaled];[scaled] pad=1280:720:(ow-iw)/2:(oh-ih)/2:black \" -c:v libx264 -c:a mp3 -ab 128k "
                }
            );
            */

            /*
            if (files.Count > 1)
            {
                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

                NReco.VideoConverter.FFMpegInput[] ffMpegInputs = new NReco.VideoConverter.FFMpegInput[files.Count];

                for (int i = 0; i < files.Count; i++)
                {
                    var ffMpegInputs = new NReco.VideoConverter.FFMpegInput(files);
                    ffMpegInputs[i] = ffMpegInputs;
                }


                ConvertSettings csettings = new ConvertSettings();
                csettings.SetVideoFrameSize((int)MovieWidth.Value, (int)MovieHeight.Value);
                csettings.VideoFrameCount = listImages.Items.Count;
                csettings.VideoFrameRate = (int)FPS.Value;

               
                if (FormatChooser.SelectedIndex == 0)
                {
                    //This just takes the first picture and converts that single frame into a movie.
                    ffMpeg.ConvertMedia(ffMpegInputs, @"Converted.avi", Format.avi, csettings);
                    //ffMpeg.ConvertMedia(listImages.Items[0].SubItems[2].Text.ToString(), ffMpegInput.Format, @"Converted.avi", Format.avi, csettings);
                }
            }
            else
            {
                MessageBox.Show("You need at least two images to make a movie.", Title);
            }*/
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
