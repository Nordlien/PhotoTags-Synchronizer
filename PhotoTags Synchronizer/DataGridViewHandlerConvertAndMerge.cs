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

        /*
        Container: MP4
        -c:v libx264
        -c:v libx264rgb 		-> If your input files are RGB, it's the RGB to YUV color space conversion. Use -c:v libx264rgb instead
        -movflags +faststart 	-> YouTube ​recommends using faststart

        Audio bitrates : 
        -b:a 384k

	        Stereo	384 kbps
	        5.1	512 kbps
	
        Audio codec: AAC-LC 
        -c:a aac
        -ac 2
        -ar 48000

	        Channels: Stereo or Stereo + 5.1
	        Sample rate 96 khz or 48 khz

        Video codec: H.264
        -pix_fmt yuv420p -> 4:2:0 chroma subsampling


	        Progressive scan (no interlacing)
	        High Profile
        -profile:v baseline / high
	        2 consecutive B frames
	        Closed GOP. GOP of half the frame rate.
	        CABAC
	        Variable bitrate. No bitrate limit required, although we offer recommended bitrates below for reference
	        Chroma subsampling: 4:2:0
	
        Frame rate
	        Content should be encoded and uploaded using the same frame rate that was used during recording.
	        Common frame rates include: 24, 25, 30, 48, 50 and 60 frames per second (other frame rates are also acceptable).
	        Interlaced content should be deinterlaced before uploading. For example, 1080i60 content should be deinterlaced to 1080p30. 60 interlaced fields per second should be deinterlaced to 30 progressive frames per second.

        -vsync vfr -r 24
	        -filter:v fps=24
	        -crf 24 <- output	‘ntsc’ 30000/1001, ‘pal’ 25/1
	        -framerate is an input per-file option
	        -r can be either an input or output option. As an input option
	        fps filter allows one to alter a stream's framerate while filtering by dropping or duplicating frames to achieve the given rate
	
	
        Bitrate
        -maxrate 8M -bufsize 10M

	        The bitrates below are recommendations for uploads. Audio playback bitrate is not related to video resolution.	
	        Type		Video Bitrate, 		Video Bitrate, 
				        Standard Frame Rate	High Frame Rate
				        (24, 25, 30)		(48, 50, 60)
	        2160p (4K)	35–45 Mbps			53–68 Mbps
	        1440p (2K)	16 Mbps				24 Mbps
	        1080p		8 Mbps				12 Mbps
	        720p		5 Mbps				7.5 Mbps
	        480p		2.5 Mbps			4 Mbps
	        360p		1 Mbps				1.5 Mbps
	


        Resolution and aspect ratio
        -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2"
	        16:9
	        2160p: 3840 x 2160
	        1440p: 2560 x 1440
	        1080p: 1920 x 1080 -> ‘hd1080’
	        1920x1080
	        720p: 1280 x 720
	        480p: 854 x 480
	        360p: 640 x 360
	        240p: 426 x 240

        Colour space
	        BT.709	BT.709 (H.273 value: 1)	BT.709 (H.273 value 1)	BT.709 (H.273 value 1)


        -y 
        -i "{AudioFileFullPath}" 
        -f concat -safe 0 
        -i "{ArgumentFileFullPath}" 
        -c:v libx264rgb
        -movflags +faststart 
        -b:a 384k
        -c:a aac
        -ac 2
        -ar 48000
        -pix_fmt yuv420p
        -profile:v baseline
        -vsync vfr -r 24
        -maxrate 8M -bufsize 10M
        -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2"
        -shortest 
        "{TempFileFullPath}"

        Merge Images - Argument
        -y -i "{AudioFileFullPath}" -f concat -safe 0 -i "{ArgumentFileFullPath}" -c:v libx264 -movflags +faststart -b:a 384k -c:a aac -ac 2 -ar 48000 -pix_fmt yuv420p -profile:v baseline -vsync vfr -r 24 -maxrate 8M -bufsize 10M -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2" -shortest "{TempFileFullPath}"

        Merge Images - ArguFile
        file '{ImageFileFullPath}'
        duration {Duration}

        Convert video - Arguments
        -y -i "{VideoFileFullPath}" -c:v copy -c:v libx264 -movflags +faststart -b:a 384k -c:a aac -ac 2 -ar 48000 -pix_fmt yuv420p -profile:v baseline -vsync vfr -r 24 -maxrate 8M -bufsize 10M -vf "scale=1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2" "{TempFileFullPath}"

        Concat videos - Arguments 
        -y -f concat -safe 0 -i "{ArgumentFileFullPath}" -c:v copy "{TempFileFullPath}"

        Concat videos - ArguFile 
        file '{VideoFileFullPath}' 
        */
        public enum VariableListType
        {
            ExeArguments,
            ImagesListing,
            VideoListing
        }
        public static string[] ListVariables(VariableListType variableListType)
        {
            List<string> variables = new List<string>();
            switch (variableListType)
            {
                case VariableListType.ExeArguments:
                    variables.Add("{AudioFileFullPath}");
                    variables.Add("{ArgumentFileFullPath}");
                    variables.Add("{TempFileFullPath}");
                    break;
                case VariableListType.VideoListing:                    
                    variables.Add("{VideoFileFullPath}");
                    break;
                case VariableListType.ImagesListing:           
                    variables.Add("{ImageFileFullPath}");
                    variables.Add("{Duration}");
                    break;
            }
            return variables.ToArray();
        }

        public static string ReplaceVariablesInString(string stringWithVariables, string arguFilename, string musicFileFullPath, string tempOutputfile)
        {
            string variableReplaced = stringWithVariables;
            variableReplaced = variableReplaced.Replace("{ArgumentFileFullPath}", arguFilename);
            variableReplaced = variableReplaced.Replace("{AudioFileFullPath}", musicFileFullPath);
            variableReplaced = variableReplaced.Replace("{TempFileFullPath}", tempOutputfile);
            return variableReplaced;
        }

        public static string ReplaceVariablesInArguFile(string stringWithVariables, string file, int duration)
        {
            string variableReplaced = stringWithVariables;
            variableReplaced = variableReplaced.Replace("{VideoFileFullPath}", file);
            variableReplaced = variableReplaced.Replace("{ImageFileFullPath}", file);
            variableReplaced = variableReplaced.Replace("{Duration}", duration.ToString());
            return variableReplaced;
        }

        #region ConvertImages
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
                        string arguFileLines = ReplaceVariablesInArguFile(imageArguFile, file, duration);
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
        #endregion

        #region MergeVideos
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
                        
                        string arguFileLines = ReplaceVariablesInArguFile(videoArguFile, file, 0);
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
        #endregion 

        #region ConvertVideo
        private static void ConvertVideo(PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow, string executeFile, string videoConvertArgument)
        {
            int exitCode = -1;

            String path = NativeMethods.GetFullPathOfFile(executeFile);

            string exiftoolOutput = "";

            formTerminalWindow.LogWarning("Command: " + path + " " + videoConvertArgument + "\r\n");

            using (var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = videoConvertArgument,
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
        #endregion

        #region ConvertAndMerge
        public static void ConvertAndMerge(
            List<string> files, string executeFile, string musicFile, int duration, 
            string videoMergeArgument, string videoMergeArguFile, 
            string imageConcatArgument, string imageConcatArguFile,
            string videoCovertArgument, string outputFile)
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
                    bool converted = true;
                    string tempOutputfile = Path.Combine(outputFolder, "temp_" + Guid.NewGuid().ToString() + ".mp4");
                    string videoMergeArgumentReplaced = ReplaceVariablesInString(videoCovertArgument, "", musicFileFullPath, tempOutputfile);
                    videoMergeArgumentReplaced = ReplaceVariablesInArguFile(videoMergeArgumentReplaced, files[indexStartNext], 0);
                    ConvertVideo(formTerminalWindow, executeFile, videoMergeArgumentReplaced);

                    if (converted)
                    {
                        videoFiles.Add(tempOutputfile); //Add converted video file to merge queue
                        tempFiles.Add(tempOutputfile); //Add temp file to delete queue
                    }
                    else videoFiles.Add(files[indexStartNext]); //If NOT NEED not conveted video file add to merge queue

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

                    string tempOutputfile = Path.Combine(outputFolder, "temp_" + Guid.NewGuid().ToString() + ".mp4");
                    videoFiles.Add(tempOutputfile);
                    tempFiles.Add(tempOutputfile);

                    string imageArgumentReplace = ReplaceVariablesInString(imageConcatArgument, arguFilename, musicFileFullPath, tempOutputfile);
                    ConvertImages(formTerminalWindow, imagesFiles, executeFile, imageArgumentReplace, arguFilename, imageConcatArguFile, duration);
                    
                }
            }

            if (!formTerminalWindow.GetWasProcessKilled())
            {
                if (tempFiles.Count == 1)
                {
                    try
                    {
                        File.Delete(outputFile);
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
                    string videoArgumentReplace = ReplaceVariablesInString(videoMergeArgument, arguFilename, musicFileFullPath, outputFile);
                    MergeVideos(formTerminalWindow, videoFiles, executeFile, videoArgumentReplace, arguFilename, videoMergeArguFile);

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

            formTerminalWindow.LogInfo("\r\n\r\n\r\n\r\n\r\nThe process is complete, you can close the window.\r\n\r\n\r\n\r\n\r\n");
        }
        #endregion

        #region Write
        public static void Write(DataGridView dataGridView,
            string executeFile, string musicFile, int duration,
            string videoMergeArgument, string videoMergeArguFile,
            string imageConcatArgument, string imageConcatArguFile,
            string videoConvertArgument, string outputFile)
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
           
            ConvertAndMerge(files, executeFile, musicFile, duration, videoMergeArgument, videoMergeArguFile, imageConcatArgument, imageConcatArguFile, videoConvertArgument, outputFile);
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
