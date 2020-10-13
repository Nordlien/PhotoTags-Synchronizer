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

namespace PhotoTagsSynchronizer
{
    

    public static class DataGridViewHandlerRename
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static string headerNewFilename = "new filename";
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static FileDateTimeReader FileDateTimeFormats { get; set; }
        public static string RenameVaribale { get; set; }

        private static string CreateNewFilename(string newFilenameVariable, string oldFilename, Metadata metadata)
        {
            /*
            %Trim%%MediaFileNow_DateTime% %FileNameWithoutDateTime%%Extension%

            %Trim%
            %FileName%
            %FileNameWithoutDateTime%
            %Extension%
            %MediaFileNow_DateTime%
            %Media_DateTime%
            %Media_yyyy%
            %Media_MM%
            %Media_dd%
            %Media_HH%
            %Media_mm%
            %Media_ss%
            %File_DateTime%
            %File_yyyy%
            %File_MM%
            %File_dd%
            %File_HH%
            %File_mm%
            %File_ss%
            %Now_DateTime%
            %Now_yyyy%
            %Now_MM%
            %Now_dd%
            %Now_HH%
            %Now_mm%
            %Now_ss%
            %GPS_DateTimeUTC%
            %MediaAlbum%
            %MediaTitle%
            %MediaDescription%
            %MediaAuthor%
            %LocationName%
            %LocationCountry%
            %LocationState%
            */
            string newFilename = newFilenameVariable;
            newFilename = newFilename.Replace("%FileName%", Path.GetFileNameWithoutExtension(oldFilename));
            newFilename = newFilename.Replace("%FileNameWithoutDateTime%", FileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(oldFilename)));
            //newFilename = newFilename.Replace("%FileNameWithoutDateTrim%", FileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(oldFilename))).Trim().Replace("  ", " ").Replace(" _", "_").Replace("_ ", "_");

            newFilename = newFilename.Replace("%Extension%", Path.GetExtension(oldFilename));
            DateTime dateTime;
            if ((metadata != null && metadata.MediaDateTaken != null)) dateTime = (DateTime)metadata.MediaDateTaken;
            else if (metadata != null && metadata.FileDateCreated != null) dateTime = (DateTime)metadata.FileDateCreated;
            else dateTime = DateTime.Now;

            newFilename = newFilename.Replace("%MediaFileNow_DateTime%", dateTime.ToString("yyyy-MM-dd HH-mm-ss"));

            newFilename = newFilename.Replace("%Media_DateTime%", (metadata == null || metadata.MediaDateTaken == null) ? "" : ((DateTime)metadata.MediaDateTaken).ToString("yyyy-MM-dd HH-mm-ss"));
            newFilename = newFilename.Replace("%Media_yyyy%", (metadata == null || metadata.MediaDateTaken == null) ? "" : ((DateTime)metadata.MediaDateTaken).ToString("yyyy"));
            newFilename = newFilename.Replace("%Media_MM%", (metadata == null || metadata.MediaDateTaken == null) ? "" : ((DateTime)metadata.MediaDateTaken).ToString("MM"));
            newFilename = newFilename.Replace("%Media_dd%", (metadata == null || metadata.MediaDateTaken == null) ? "" : ((DateTime)metadata.MediaDateTaken).ToString("dd"));
            newFilename = newFilename.Replace("%Media_HH%", (metadata == null || metadata.MediaDateTaken == null) ? "" : ((DateTime)metadata.MediaDateTaken).ToString("HH"));
            newFilename = newFilename.Replace("%Media_mm%", (metadata == null || metadata.MediaDateTaken == null) ? "" : ((DateTime)metadata.MediaDateTaken).ToString("mm"));
            newFilename = newFilename.Replace("%Media_ss%", (metadata == null || metadata.MediaDateTaken == null) ? "" : ((DateTime)metadata.MediaDateTaken).ToString("ss"));

            newFilename = newFilename.Replace("%File_DateTime%", (metadata == null || metadata.FileDateCreated == null) ? "" : ((DateTime)metadata.FileDateCreated).ToString("yyyy-MM-dd HH-mm-ss"));
            newFilename = newFilename.Replace("%File_yyyy%", (metadata == null || metadata.FileDateCreated == null) ? "" : ((DateTime)metadata.FileDateCreated).ToString("yyyy"));
            newFilename = newFilename.Replace("%File_MM%", (metadata == null || metadata.FileDateCreated == null) ? "" : ((DateTime)metadata.FileDateCreated).ToString("MM"));
            newFilename = newFilename.Replace("%File_dd%", (metadata == null || metadata.FileDateCreated == null) ? "" : ((DateTime)metadata.FileDateCreated).ToString("dd"));
            newFilename = newFilename.Replace("%File_HH%", (metadata == null || metadata.FileDateCreated == null) ? "" : ((DateTime)metadata.FileDateCreated).ToString("HH"));
            newFilename = newFilename.Replace("%File_mm%", (metadata == null || metadata.FileDateCreated == null) ? "" : ((DateTime)metadata.FileDateCreated).ToString("mm"));
            newFilename = newFilename.Replace("%File_ss%", (metadata == null || metadata.FileDateCreated == null) ? "" : ((DateTime)metadata.FileDateCreated).ToString("ss"));

            newFilename = newFilename.Replace("%Now_DateTime%", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            newFilename = newFilename.Replace("%Now_yyyy%", DateTime.Now.ToString("yyyy"));
            newFilename = newFilename.Replace("%Now_MM%", DateTime.Now.ToString("MM"));
            newFilename = newFilename.Replace("%Now_dd%", DateTime.Now.ToString("dd"));
            newFilename = newFilename.Replace("%Now_HH%", DateTime.Now.ToString("HH"));
            newFilename = newFilename.Replace("%Now_mm%", DateTime.Now.ToString("mm"));
            newFilename = newFilename.Replace("%Now_ss%", DateTime.Now.ToString("ss"));

            newFilename = newFilename.Replace("%GPS_DateTimeUTC%", (metadata == null || metadata.LocationDateTime == null) ? "" : ((DateTime)metadata.LocationDateTime).ToString("u").Replace(":", "-"));
            
            newFilename = newFilename.Replace("%MediaAlbum%", (metadata == null || metadata.PersonalAlbum == null) ? "" : metadata.PersonalAlbum);
            newFilename = newFilename.Replace("%MediaTitle%", (metadata == null || metadata.PersonalTitle == null) ? "" : metadata.PersonalTitle);
            newFilename = newFilename.Replace("%MediaDescription%", (metadata == null || metadata.PersonalDescription == null) ? "" : metadata.PersonalDescription);
            newFilename = newFilename.Replace("%MediaAuthor%", (metadata == null || metadata.PersonalAuthor == null) ? "" : metadata.PersonalAuthor);

            newFilename = newFilename.Replace("%LocationName%", (metadata == null || metadata.LocationName == null) ? "" : metadata.LocationName);
            newFilename = newFilename.Replace("%LocationCountry%", (metadata == null || metadata.LocationCountry == null) ? "" : metadata.LocationCountry);
            newFilename = newFilename.Replace("%LocationState%", (metadata == null || metadata.LocationState == null) ? "" : metadata.LocationState);

            if (newFilename.Contains("%Trim%")) newFilename = newFilename.Replace("%Trim%", "").Trim().Replace("  ", " ").Replace("_ ", "_").Replace(" -", "-").Replace("- ", "-").Replace(" .", ".");

            newFilename = newFilename
                .Replace("\u0000", "")
                .Replace("\u0001", "")
                .Replace("\u0002", "")
                .Replace("\u0003", "")
                .Replace("\u0004", "")
                .Replace("\u0005", "")
                .Replace("\u0006", "")
                .Replace("\u0007", "")
                .Replace("\u0008", "")
                .Replace("\u0009", "")
                .Replace("\u000A", "")
                .Replace("\u000B", "")
                .Replace("\u000C", "")
                .Replace("\u000D", "")
                .Replace("\u000E", "")
                .Replace("\u000F", "")
                .Replace("\u0010", "")
                .Replace("\u0011", "")
                .Replace("\u0012", "")
                .Replace("\u0013", "")
                .Replace("\u0014", "")
                .Replace("\u0015", "")
                .Replace("\u0016", "")
                .Replace("\u0017", "")
                .Replace("\u0018", "")
                .Replace("\u0019", "")
                .Replace("\u001A", "")
                .Replace("\u001B", "")
                .Replace("\u001C", "")
                .Replace("\u001D", "")
                .Replace("\u001E", "")
                .Replace("\u001F", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace(":", "")
                .Replace("|", "")
                .Replace("?", "")
                .Replace("*", "")
                .Replace("/", "")
                .Replace("\\", "")
                .Replace("\"", "");
            return newFilename;
        }

        public static void UpdateFilenames(DataGridView dataGridView, string newFilenameVariable)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, new FileEntryImage(headerNewFilename, dateTimeEditable));
            if (columnIndex == -1) return;

            //WindowsPropertyReader.Write(dataGridView, columnIndex);
            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                DataGridViewGenericCell cellGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCell(dataGridView, columnIndex, rowIndex);
                
                if (!cellGridViewGenericCell.CellStatus.CellReadOnly)
                {
                    //FileEntryBroker fileEntryBroker = new FileEntryBroker(fullFilePath, dateTimeForEditableMediaFile, MetadataBrokerTypes.ExifTool);
                    //Metadata metadata = DatabaseAndCacheMetadataExiftool.ReadCache(fileEntryBroker);
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, CreateNewFilename(newFilenameVariable, dataGridViewGenericRow.RowName, dataGridViewGenericRow.Metadata));
                }
            }

        }

        public static void RenameFile(string oldFullFilename, string newFullFilename, ref Dictionary<string, string> renameSuccess, ref Dictionary<string, string> renameFailed)
        {
            try
            {
                string oldFilename = Path.GetFileName(oldFullFilename);
                string oldDirectory = Path.GetDirectoryName(oldFullFilename);

                string newFilename = Path.GetFileName(newFullFilename);
                string newDirectory = Path.GetDirectoryName(newFullFilename);

                Directory.CreateDirectory(newDirectory);  
                File.Move(oldFullFilename, newFullFilename);
                DatabaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename);
                renameSuccess.Add(oldFullFilename, newFullFilename);
            }
            catch (Exception ex)
            {
                renameFailed.Add(oldFullFilename, newFullFilename);
                Logger.Error("Rename file failed: " + oldFullFilename + " to :" + newFullFilename + " " + ex.Message);
            }
        }

        public static void Write(DataGridView dataGridView, out Dictionary<string, string> renameSuccess, out Dictionary<string, string> renameFailed)
        {
            renameSuccess = new Dictionary<string, string>();
            renameFailed = new Dictionary<string, string>();

            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, new FileEntryImage(headerNewFilename, dateTimeEditable));
            if (columnIndex == -1) return;

            //WindowsPropertyReader.Write(dataGridView, columnIndex);
            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericCell cellGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCell(dataGridView, columnIndex, rowIndex);

                if (!cellGridViewGenericCell.CellStatus.CellReadOnly)
                {
                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);

                    string oldFilename = dataGridViewGenericRow.RowName;
                    string oldDirectory = dataGridViewGenericRow.HeaderName;
                    string oldFullFilename = Path.Combine(oldDirectory, oldFilename);

                    string newRelativeFilename = Path.Combine(oldDirectory, DataGridViewHandler.GetCellValueStringTrim(dataGridView, columnIndex, rowIndex));
                    string newFullFilename = Path.GetFullPath(newRelativeFilename);
                    
                    RenameFile(oldFullFilename, newFullFilename, ref renameSuccess, ref renameFailed);
                    

                }
            }

        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow);
        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, cellReadOnly));
        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, dataGridViewGenericCellStatusDefaults);
            return rowIndex;
        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults, Metadata metadata)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, dataGridViewGenericCellStatusDefaults);
            return rowIndex;
        }

        private static DateTime dateTimeEditable = DataGridViewHandler.DateTimeForEditableMediaFile;
        
        public static void PopulateFile(DataGridView dataGridView, string fullFilePath, ShowWhatColumns showWhatColumns, DateTime dateTimeForEditableMediaFile)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, headerNewFilename)) return;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);
            //-----------------------------------------------------------------

            
            FileEntryBroker fileEntryBroker = new FileEntryBroker(fullFilePath, dateTimeForEditableMediaFile, MetadataBrokerTypes.ExifTool);
            Metadata metadata = DatabaseAndCacheMetadataExiftool.ReadCache(fileEntryBroker);

            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, new FileEntryImage(headerNewFilename, dateTimeEditable));
            if (columnIndex == -1) return;
            
            string directory = Path.GetDirectoryName(fullFilePath);
            string filename = Path.GetFileName(fullFilePath);

            //Media
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(directory));
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(directory, filename, metadata), CreateNewFilename(RenameVaribale, filename, metadata), false);


            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }


        public static void PopulateSelectedFiles(DataGridView dataGridView, ImageListViewSelectedItemCollection imageListViewSelectItems, bool useCurrentFileLastWrittenDate, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns)
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
            //DataGridViewHandler.AddColumnSelectedFiles(dataGridView, imageListViewSelectItems, true, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
            //  new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true)); 
            
            int columnIndex = DataGridViewHandler.AddColumnOrUpdate(dataGridView,
                new FileEntryImage(headerNewFilename, dateTimeEditable), //Heading
                    null, dateTimeEditable,
                    ReadWriteAccess.AllowCellReadAndWrite, 
                    showWhatColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Off, true));

            //Add all default rows
            //AddRowsDefault(dataGridView);
            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------

            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                PopulateFile(dataGridView, imageListViewItem.FullFileName, showWhatColumns, 
                    useCurrentFileLastWrittenDate ? imageListViewItem.DateModified : DataGridViewHandler.DateTimeForEditableMediaFile);
            }

            //-----------------------------------------------------------------
            //Unlock
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //Tell that work is done
            DataGridViewHandler.SetIsPopulating(dataGridView, false);
            //-----------------------------------------------------------------
        }
    }
}
