using System.Windows.Forms;
using MetadataLibrary;
using DataGridViewGeneric;
using Manina.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;
using System;
using System.IO;
using FileDateTime;
using System.Collections.Generic;
using NLog;
using ApplicationAssociations;
using System.Threading;
using System.Diagnostics;
using System.Text;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerConvertAndMerge
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static string headerConvertAndMergeFilename = "Filename";
        private static string headerConvertAndMergeInfo = "Drag and drop to re-order";

        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static FileDateTimeReader FileDateTimeFormats { get; set; }

        public static FilesCutCopyPasteDrag FilesCutCopyPasteDrag { get; set; }

        public static string RenameVaribale { get; set; }

        private static void ConvertImages(PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow, List<string> imagesFiles,
            string executeFile, string imageArgument,
            string arguFilename, string imageArguFile, int duration)
        {
            int exitCode = int.MinValue;

            if (imagesFiles.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(arguFilename, false, new UTF8Encoding(false)))
                {
                    foreach (string file in imagesFiles)
                    {                        
                        string arguFileLines = imageArguFile;
                        arguFileLines = arguFileLines.Replace("{ImageFileFullPath}", file);
                        arguFileLines = arguFileLines.Replace("{Duration}", duration.ToString());
                        sw.WriteLine(arguFileLines);
                    }
                    //(Due to a quirk, the last image has to be specified twice - the 2nd time without any duration directive)
                    sw.WriteLine("file '"+ imagesFiles[imagesFiles.Count-1] + "'");
                }

                String path = NativeMethods.GetFullPathOfFile(executeFile);
                
                
                string exiftoolOutput = "";

                formTerminalWindow.LogWarning("Command: " + path + " " + imageArgument + "\r\n");

                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = imageArgument,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        RedirectStandardInput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    }
                })
                {
                    formTerminalWindow.SetProcssToFollow(process);
                    bool result = process.Start();
                    string line;

                    while (!process.StandardError.EndOfStream)
                    {

                        line = process.StandardError.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        formTerminalWindow.LogError(line + "\r\n");
                        //if (!line.StartsWith("Warning")) hasExiftoolErrorMessage = true;
                        Logger.Error("EXIFTOOL WRITE ERROR: " + line);
                        Application.DoEvents();
                    }

                    while (!process.StandardOutput.EndOfStream)
                    {
                        line = process.StandardOutput.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        formTerminalWindow.LogInfo(line + "\r\n");
                        //if (line.StartsWith("Error")) hasExiftoolErrorMessage = true;
                        Logger.Info("EXIFTOOL WRITE OUTPUT: " + line);
                        Application.DoEvents();
                    }


                    process.WaitForExit();
                    if ((exitCode = process.ExitCode) != 0)
                    {
                        Logger.Info("process.WaitForExit() " + exitCode);
                    }

                    while (!process.HasExited) Thread.Sleep(100);

                    formTerminalWindow.SetProcssToFollow(null);
                    process.Close();
                    process.Dispose();
                }
                
                formTerminalWindow.LogWarning("Exitcode: " + exitCode + "\r\n");
            }
     
        }


        private static void MergeVideos(PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow, List<string> videoFiles,
            string executeFile, string videoArgument,
            string arguFilename, string videoArguFile)
        {
            int exitCode = -1;

            if (videoFiles.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(arguFilename, false, new UTF8Encoding(false)))
                {
                    foreach (string file in videoFiles)
                    {
                        
                        string arguFileLines = videoArguFile;
                        arguFileLines = arguFileLines.Replace("{VideoFileFullPath}", file);
                        sw.WriteLine(arguFileLines);
                    }
                }

                String path = NativeMethods.GetFullPathOfFile(executeFile);


                string exiftoolOutput = "";

                formTerminalWindow.LogWarning("Command: " + path + " " + videoArgument + "\r\n");

                using (var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = videoArgument,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        RedirectStandardInput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    }
                })
                {
                    formTerminalWindow.SetProcssToFollow(process);

                    bool result = process.Start();
                    string line;

                    while (!process.StandardError.EndOfStream)
                    {

                        line = process.StandardError.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        formTerminalWindow.LogError(line + "\r\n");
                        //if (!line.StartsWith("Warning")) hasExiftoolErrorMessage = true;
                        Logger.Error("EXIFTOOL WRITE ERROR: " + line);
                        Application.DoEvents();
                    }

                    while (!process.StandardOutput.EndOfStream)
                    {
                        line = process.StandardOutput.ReadLine();
                        exiftoolOutput += line + "\r\n";
                        formTerminalWindow.LogInfo(line + "\r\n");
                        //if (line.StartsWith("Error")) hasExiftoolErrorMessage = true;
                        Logger.Info("EXIFTOOL WRITE OUTPUT: " + line);
                        Application.DoEvents();
                    }

                    process.WaitForExit();
                    if ((exitCode = process.ExitCode) != 0)
                    {
                        Logger.Info("process.WaitForExit() " + exitCode);
                    }

                    while (!process.HasExited) Thread.Sleep(100);

                    formTerminalWindow.SetProcssToFollow(null);
                    process.Close();
                    process.Dispose();
                }
                formTerminalWindow.LogWarning("Exitcode: " + exitCode + "\r\n");
            }

        }

        public static void ConvertAndMerge(
            List<string> files, string executeFile, string musicFile, int duration, 
            string videoArgument, string videoArguFile, 
            string imageArgument, string imageArguFile, 
            string outputFile)
        {

            string arguFilename = Exiftool.ExiftoolWriter.GetTempArguFileFullPath("ffmpeg_arg.txt");
            string musicFileFullPath = NativeMethods.GetFullPathOfFile(musicFile);
            string outputFolder = Path.GetDirectoryName(outputFile);

            PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow = new PhotoTagsCommonComponets.FormTerminalWindow();
            formTerminalWindow.Show();

            
            List<string> tempFiles = new List<string>();
            List<string> videoFiles = new List<string>();

            int indexStartNext = 0;
            while (indexStartNext < files.Count && !formTerminalWindow.GetWasProcessKilled())
            {
                if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(files[indexStartNext]))
                {
                    //If video file add to merge queue
                    videoFiles.Add(files[indexStartNext]);
                    indexStartNext++;
                }
                else
                {
                    //If image, convert to video and and to merge queue
                    List<string> imagesFiles = new List<string>();
                    for (int indexMediaFile = indexStartNext; indexMediaFile < files.Count; indexMediaFile++)
                    {
                        indexStartNext = indexMediaFile + 1;
                        if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsImageFormat(files[indexMediaFile])) imagesFiles.Add(files[indexMediaFile]);
                        if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(files[indexMediaFile])) break;
                    }

                    string tempOutoutfile = Path.Combine(outputFolder, "temp_" + Guid.NewGuid().ToString() + ".mp4");
                    videoFiles.Add(tempOutoutfile);
                    tempFiles.Add(tempOutoutfile);

                    string imageArgumentReplace = imageArgument;
                    imageArgumentReplace = imageArgumentReplace.Replace("{ArgumentFileFullPath}", arguFilename);
                    imageArgumentReplace = imageArgumentReplace.Replace("{TempFileFullPath}", tempOutoutfile);
                    imageArgumentReplace = imageArgumentReplace.Replace("{AudioFileFullPath}", musicFileFullPath);
                    ConvertImages(formTerminalWindow, imagesFiles, executeFile, imageArgumentReplace, arguFilename, imageArguFile, duration);
                    
                }
            }

            if (!formTerminalWindow.GetWasProcessKilled())
            {
                if (tempFiles.Count == 1)
                {
                    try
                    {
                        File.Move(tempFiles[0], outputFile);
                        formTerminalWindow.LogInfo("Rename file from: " + tempFiles[0] + " to: " + outputFile + "\r\n");
                    }
                    catch (Exception ex)
                    {
                        formTerminalWindow.LogInfo(ex.Message + "\r\n");
                    }
                }
                else 
                { 
                    string videoArgumentReplace = videoArgument;
                    videoArgumentReplace = videoArgumentReplace.Replace("{ArgumentFileFullPath}", arguFilename);
                    videoArgumentReplace = videoArgumentReplace.Replace("{TempFileFullPath}", outputFile);
                    MergeVideos(formTerminalWindow, videoFiles, executeFile, videoArgumentReplace, arguFilename, videoArguFile);

                    foreach (string tempfile in tempFiles)
                    {
                        try
                        {
                            File.Delete(tempfile);
                            formTerminalWindow.LogInfo("Temp file deleted: " + tempfile + "\r\n");
                        }
                        catch (Exception ex)
                        {
                            formTerminalWindow.LogInfo(ex.Message + "\r\n");
                        }
                    }
                }
            }
        }

        #region Write
        public static void Write(DataGridView dataGridView,
            string executeFile, string musicFile, int duration,
            string videoArgument, string videoArguFile,
            string imageArgument, string imageArguFile,
            string outputFile)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, headerConvertAndMergeFilename);
            if (columnIndex == -1) return;

            List<string> files = new List<string>();

            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                //DataGridViewGenericCell cellGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, columnIndex, rowIndex);
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null && dataGridViewGenericRow.IsHeader == false) files.Add(dataGridViewGenericRow.RowName);
            }
            /*
-y -f concat -safe 0 -i "{ArgumentFileFullPath}" -preset fast -c:a aac -b:a 192k -ac 2 -vf fps=fps=30 -c:v libx264 -b:v 1024k -profile:v high -level 4.1 -crf -1 -pix_fmt yuv420p "{TempFileFullPath}"
-y -i "{AudioFileFullPath}" -f concat -safe 0 -i "{ArgumentFileFullPath}" -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1" -c:v libx264 -crf 14 -r 30 -pix_fmt yuv420p -shortest "{TempFileFullPath}"

-f concat -safe 0 -i "{ArgumentFileFullPath}" -c:a aac -b:a 192k -ac 2 -tune stillimage -preset fast -crf 23 -filter:v fps=fps=30 -profile:v high -c:v libx264 -pix_fmt yuv420p -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1" "{TempFileFullPath}" 
-y -i "{AudioFileFullPath}" -framerate 1/5 -f concat -safe 0 -i "{ArgumentFileFullPath}" -c:a aac -b:a 192k -ac 2 -tune stillimage -preset fast -crf 23 -filter:v fps=fps=30 -profile:v high -c:v libx264 -pix_fmt yuv420p -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1" -shortest "{TempFileFullPath}"

-y -f concat -safe 0 -i "{ArgumentFileFullPath}" -c:a aac -ar 48000 -b:a 128k -ac 2 -tune film -preset fast -profile:v high -c:v libx264 -pix_fmt yuv420p -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1,mpdecimate" -vsync 0  "{TempFileFullPath}"
-y -i "{AudioFileFullPath}" -f concat -safe 0 -i "{ArgumentFileFullPath}" -c:a aac -ar 48000 -b:a 128k -ac 2 -tune film -preset fast -profile:v high -c:v libx264 -pix_fmt yuv420p -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1,mpdecimate" -vsync 0 -shortest "{TempFileFullPath}"


            */
            ConvertAndMerge(files, executeFile, musicFile, duration, videoArgument, videoArguFile, imageArgument, imageArguFile, outputFile);
        }
        #endregion

        #region AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, bool sort)
        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, bool sort)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, sort);
        }
        #endregion

        #region AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, cellReadOnly));
        }
        #endregion

        #region AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, dataGridViewGenericCellStatusDefaults, true);
            return rowIndex;
        }
        #endregion 

        #region PopulateFile
        public static void PopulateFile(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, headerConvertAndMergeFilename)) return;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);
            //-----------------------------------------------------------------

            Metadata metadata = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool));

            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, headerConvertAndMergeFilename);

            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerConvertAndMergeInfo), true);

            if (columnIndex != -1)
            {
                string directory = fileEntryAttribute.Directory;
                string filename = fileEntryAttribute.FileName;
                

                //Media
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerConvertAndMergeInfo, fileEntryAttribute.FileFullPath, metadata), filename, true);
            }

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }
        #endregion 

        #region PopulateSelectedFiles
        public static void PopulateSelectedFiles(DataGridView dataGridView, ImageListViewSelectedItemCollection imageListViewSelectItems, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (DataGridViewHandler.GetIsAgregated(dataGridView)) return;
            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;
            //Tell that work in progress, can start a new before done.
            DataGridViewHandler.SetIsPopulating(dataGridView, true);
            //Clear current DataGridView
            DataGridViewHandler.Clear(dataGridView, dataGridViewSize);
            //Add Columns for all selected files, one column per select file
            
            DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute(headerConvertAndMergeFilename, DateTime.Now, FileEntryVersion.Current), null, null,
                ReadWriteAccess.AllowCellReadAndWrite, showWhatColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------

            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                PopulateFile(dataGridView, new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.DateModified, FileEntryVersion.Current));
            }

            //-----------------------------------------------------------------
            //Unlock
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //Tell that work is done
            DataGridViewHandler.SetIsPopulating(dataGridView, false);
            //-----------------------------------------------------------------
        }
        #endregion 
    }
}
