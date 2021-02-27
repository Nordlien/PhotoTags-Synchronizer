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

        public static void Test(
            List<string> files, string executeFile, string musicFile, int duration, 
            string videoArgument, string videoArguFile, 
            string imageArgument, string imageArguFile, 
            string outputFile)
        {

            string arguFilename = Exiftool.ExiftoolWriter.GetTempArguFileFullPath("ffmpeg_arg.txt");
            string musicFileFullPath = NativeMethods.GetFullPathOfFile(musicFile);
            string outputFolder = Path.GetDirectoryName(outputFile);

            string tempOutoutfile = Path.Combine(outputFolder, "temp.mp4");

            videoArgument = videoArgument.Replace("{ArgumentFileFullPath}", arguFilename);
            videoArgument = videoArgument.Replace("{TempFileFullPath}", tempOutoutfile);
            //videoArguFile = videoArguFile.Replace("{VideoFileFullPath}", files);

            imageArgument = imageArgument.Replace("{ArgumentFileFullPath}", arguFilename);
            imageArgument = imageArgument.Replace("{TempFileFullPath}", tempOutoutfile);
            imageArgument = imageArgument.Replace("{AudioFileFullPath}", musicFileFullPath);


            using (StreamWriter sw = new StreamWriter(arguFilename, false, new UTF8Encoding(false)))
            {
                foreach (string file in files)
                {
                    string arguFileLines = imageArguFile;
                    arguFileLines = arguFileLines.Replace("{ImageFileFullPath}", file);
                    arguFileLines = arguFileLines.Replace("{Duration}", duration.ToString());
                    sw.WriteLine(arguFileLines);
                }
            }

            PhotoTagsCommonComponets.FormTerminalWindow formTerminalWindow = new PhotoTagsCommonComponets.FormTerminalWindow();
            formTerminalWindow.Show();

            String path = NativeMethods.GetFullPathOfFile(executeFile);
            string arguments = imageArgument;
                /* "-y -i \"c:\\Users\\nordl\\OneDrive\\Pictures JTNs OneDrive\\TestTags\\audio.wav\" -f concat -safe 0 -i \"" +
                NativeMethods.ShortFileName(tempFilename) + "\" -framerate 1/2 -vf \"scale =1080:720:force_original_aspect_ratio=decrease,pad=1080:720:(ow-iw)/2:(oh-ih)/2,setsar=1\" -c:v libx264 -crf 14 -r 25 -pix_fmt yuv420p -shortest \"c:\\Users\\nordl\\OneDrive\\Pictures JTNs OneDrive\\TestTags\\test.mp4\"";*/

            int exitCode = -1;
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

                process.Close();
                process.Dispose();
            }

            formTerminalWindow.LogWarningo("Exitcode: " + exitCode + "\r\n");
            //if (hasExiftoolErrorMessage) //MessageBox.Show(exiftoolOutput);

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

            Test(files, executeFile, musicFile, duration, videoArgument, videoArguFile, imageArgument, imageArguFile, outputFile);
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
