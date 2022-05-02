using System.Windows.Forms;
using MetadataLibrary;
using DataGridViewGeneric;
using System;
using System.IO;
using FileDateTime;
using System.Collections.Generic;
using FileHandeling;

namespace PhotoTagsSynchronizer
{


    public static class DataGridViewHandlerRename
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static string headerNewFilename = "New filename";
        public static bool HasBeenInitialized { get; set; } = false;
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static FileDateTimeReader FileDateTimeFormats { get; set; }
        public static FilesCutCopyPasteDrag FilesCutCopyPasteDrag { get; set; }
        public static string RenameVaribale { get; set; }
        public static bool ShowFullPath { get; set; } = false;

        public static List<string> ComputerNames = new List<string>();
        public static string GPStag = "";

        #region RemoveComputerNames
        public static string RemoveComputerNames(string filename, List<string> computerNames)
        {
            foreach (string computerName in computerNames) filename = filename.Replace(computerName, "");
            return filename;
        }
        #endregion

        #region RemoveGPStag
        public static string RemoveGPStag(string filename, string GPStag)
        {
            return filename.Replace(GPStag, "");
        }
        #endregion

        public static string[] ListOfRenameVariables = new string[]
            {
            //%Trim%%MediaFileNow_DateTime% %FileNameWithoutDateTime%%Extension%
            "%Trim%",
            "%FileName%",
            "%FileNameWithoutExtension%",
            "%FileNameWithoutExtensionDateTime%",
            "%FileNameWithoutExtensionDateTimeComputerName%",
            "%FileNameWithoutExtensionDateTimeGPStag%",
            "%FileNameWithoutExtensionDateTimeComputerNameGPStag%",
            "%FileNameWithoutExtensionComputerName%",
            "%FileNameWithoutExtensionComputerNameGPStag%",
            "%FileNameWithoutExtensionGPStag%",
            "%FileNameWithoutDateTime%",
            "%FileNameWithoutDateTimeComputerName%",
            "%FileNameWithoutDateTimeGPStag%",
            "%FileNameWithoutDateTimeComputerNameGPStag%",
            "%FileNameWithoutComputerName%",
            "%FileNameWithoutComputerNameGPStag%",
            "%FileNameWithoutGPStag%",
            "%FileExtension%",
            "%MediaFileNow_DateTime%",
            "%Media_DateTime%",
            "%Media_yyyy%",
            "%Media_MM%",
            "%Media_dd%",
            "%Media_HH%",
            "%Media_mm%",
            "%Media_ss%",
            "%File_DateTime%",
            "%File_yyyy%",
            "%File_MM%",
            "%File_dd%",
            "%File_HH%",
            "%File_mm%",
            "%File_ss%",
            "%Now_DateTime%",
            "%Now_yyyy%",
            "%Now_MM",
            "%Now_dd%",
            "%Now_HH%",
            "%Now_mm%",
            "%Now_ss%",
            "%GPS_DateTimeUTC%",
            "%MediaAlbum%",
            "%MediaTitle%",
            "%MediaDescription%",
            "%MediaAuthor%",
            "%LocationName%",
            "%LocationCountry%",
            "%LocationRegion%"
            };
        

        #region CreateNewFilename
        public static string CreateNewFilename(string newFilenameVariable, string oldFilename, Metadata metadata)
        {
            #region List of vaiables - that can be used in Rename tool
            
            #endregion

            #region Filename
            string newFilename = newFilenameVariable;
            newFilename = newFilename.Replace("%FileName%", Path.GetFileNameWithoutExtension(oldFilename));

            #region Without Extension
            newFilename = newFilename.Replace("%FileNameWithoutExtension%", Path.GetFileNameWithoutExtension(oldFilename));

            newFilename = newFilename.Replace("%FileNameWithoutExtensionDateTime%", 
                FileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(oldFilename)));

            newFilename = newFilename.Replace("%FileNameWithoutExtensionDateTimeComputerName%",
                FileDateTimeFormats.RemoveAllDateTimes(RemoveComputerNames(RemoveGPStag(Path.GetFileNameWithoutExtension(oldFilename), GPStag: GPStag), computerNames: ComputerNames)));
            
            newFilename = newFilename.Replace("%FileNameWithoutExtensionDateTimeGPStag%",
                FileDateTimeFormats.RemoveAllDateTimes(RemoveGPStag(Path.GetFileNameWithoutExtension(oldFilename), GPStag: GPStag)));
            
            newFilename = newFilename.Replace("%FileNameWithoutExtensionDateTimeComputerNameGPStag%",
                FileDateTimeFormats.RemoveAllDateTimes(RemoveComputerNames(RemoveGPStag(Path.GetFileNameWithoutExtension(oldFilename), GPStag: GPStag), computerNames: ComputerNames)));
            
            newFilename = newFilename.Replace("%FileNameWithoutExtensionComputerName%",
                FileDateTimeFormats.RemoveAllDateTimes(RemoveComputerNames(Path.GetFileNameWithoutExtension(oldFilename), computerNames: ComputerNames)));
            
            newFilename = newFilename.Replace("%FileNameWithoutExtensionComputerNameGPStag%",
                RemoveComputerNames(RemoveGPStag(Path.GetFileNameWithoutExtension(oldFilename), GPStag: GPStag), computerNames: ComputerNames));
            
            newFilename = newFilename.Replace("%FileNameWithoutExtensionGPStag%",
                RemoveGPStag(Path.GetFileNameWithoutExtension(oldFilename), GPStag: GPStag));
            #endregion

            #region DateTime
            newFilename = newFilename.Replace("%FileNameWithoutDateTime%",
                FileDateTimeFormats.RemoveAllDateTimes(oldFilename));
            newFilename = newFilename.Replace("%FileNameWithoutDateTimeComputerName%",
                FileDateTimeFormats.RemoveAllDateTimes(RemoveComputerNames(RemoveGPStag(oldFilename, GPStag: GPStag), computerNames: ComputerNames)));

            newFilename = newFilename.Replace("%FileNameWithoutDateTimeGPStag%",
                FileDateTimeFormats.RemoveAllDateTimes(RemoveGPStag(oldFilename, GPStag: GPStag)));

            newFilename = newFilename.Replace("%FileNameWithoutDateTimeComputerNameGPStag%",
                FileDateTimeFormats.RemoveAllDateTimes(RemoveComputerNames(RemoveGPStag(oldFilename, GPStag: GPStag), computerNames: ComputerNames)));
            #endregion

            #region WithoutComputerName
            newFilename = newFilename.Replace("%FileNameWithoutComputerName%",
                RemoveComputerNames(oldFilename, computerNames: ComputerNames));
            newFilename = newFilename.Replace("%FileNameWithoutComputerNameGPStag%",
                RemoveComputerNames(RemoveGPStag(oldFilename, GPStag: GPStag), computerNames: ComputerNames));
            #endregion

            #region WithoutGPStag
             newFilename = newFilename.Replace("%FileNameWithoutGPStag%",
                RemoveGPStag(oldFilename, GPStag: GPStag));
            #endregion

            newFilename = newFilename.Replace("%Extension%", Path.GetExtension(oldFilename));
            #endregion

            #region DataTime
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

            newFilename = newFilename.Replace("%File_DateTime%", (metadata == null || metadata.FileDate == null) ? "" : ((DateTime)metadata.FileDate).ToString("yyyy-MM-dd HH-mm-ss"));
            newFilename = newFilename.Replace("%File_yyyy%", (metadata == null || metadata.FileDate == null) ? "" : ((DateTime)metadata.FileDate).ToString("yyyy"));
            newFilename = newFilename.Replace("%File_MM%", (metadata == null || metadata.FileDate == null) ? "" : ((DateTime)metadata.FileDate).ToString("MM"));
            newFilename = newFilename.Replace("%File_dd%", (metadata == null || metadata.FileDate == null) ? "" : ((DateTime)metadata.FileDate).ToString("dd"));
            newFilename = newFilename.Replace("%File_HH%", (metadata == null || metadata.FileDate == null) ? "" : ((DateTime)metadata.FileDate).ToString("HH"));
            newFilename = newFilename.Replace("%File_mm%", (metadata == null || metadata.FileDate == null) ? "" : ((DateTime)metadata.FileDate).ToString("mm"));
            newFilename = newFilename.Replace("%File_ss%", (metadata == null || metadata.FileDate == null) ? "" : ((DateTime)metadata.FileDate).ToString("ss"));

            newFilename = newFilename.Replace("%Now_DateTime%", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            newFilename = newFilename.Replace("%Now_yyyy%", DateTime.Now.ToString("yyyy"));
            newFilename = newFilename.Replace("%Now_MM%", DateTime.Now.ToString("MM"));
            newFilename = newFilename.Replace("%Now_dd%", DateTime.Now.ToString("dd"));
            newFilename = newFilename.Replace("%Now_HH%", DateTime.Now.ToString("HH"));
            newFilename = newFilename.Replace("%Now_mm%", DateTime.Now.ToString("mm"));
            newFilename = newFilename.Replace("%Now_ss%", DateTime.Now.ToString("ss"));

            newFilename = newFilename.Replace("%GPS_DateTimeUTC%", (metadata == null || metadata.LocationDateTime == null) ? "" : ((DateTime)metadata.LocationDateTime).ToString("u").Replace(":", "-"));
            #endregion

            #region Media Text
            newFilename = newFilename.Replace("%MediaAlbum%", (metadata == null || metadata.PersonalAlbum == null) ? "" : metadata.PersonalAlbum);
            newFilename = newFilename.Replace("%MediaTitle%", (metadata == null || metadata.PersonalTitle == null) ? "" : metadata.PersonalTitle);
            newFilename = newFilename.Replace("%MediaDescription%", (metadata == null || metadata.PersonalDescription == null) ? "" : metadata.PersonalDescription);
            newFilename = newFilename.Replace("%MediaAuthor%", (metadata == null || metadata.PersonalAuthor == null) ? "" : metadata.PersonalAuthor);
            #endregion

            #region Location
            newFilename = newFilename.Replace("%LocationName%", (metadata == null || metadata.LocationName == null) ? "" : metadata.LocationName);
            newFilename = newFilename.Replace("%LocationCountry%", (metadata == null || metadata.LocationCountry == null) ? "" : metadata.LocationCountry);
            newFilename = newFilename.Replace("%LocationRegion%", (metadata == null || metadata.LocationState == null) ? "" : metadata.LocationState);
            newFilename = newFilename.Replace("%LocationCity%", (metadata == null || metadata.LocationCity == null) ? "" : metadata.LocationCity);
            #endregion

            #region Trim WhiteSpace 
            if (newFilename.Contains("%Trim%"))
            {
                int indexOfSplit = newFilename.IndexOf("%Trim%");
                string beforeSplit = newFilename.Substring(0, indexOfSplit);
                string afterSplit = newFilename.Substring(indexOfSplit + ("%Trim%").Length);
                afterSplit = FileHandler.TrimFolderName(afterSplit, "  ", " ");
                afterSplit = FileHandler.TrimFolderName(afterSplit, "_ ", "_");
                afterSplit = FileHandler.TrimFolderName(afterSplit, " -", "-");
                afterSplit = FileHandler.TrimFolderName(afterSplit, "- ", "-");
                afterSplit = FileHandler.TrimFolderName(afterSplit, " .", ".");
                afterSplit = FileHandler.TrimFolderName(afterSplit, ". ", ".");
                afterSplit = FileHandler.TrimFolderName(afterSplit, "\\ ", "\\");
                afterSplit = FileHandler.TrimFolderName(afterSplit, " \\", "\\"); 
                newFilename = (beforeSplit + afterSplit).Replace("%Trim%", "").Trim(); //If contains more %Trim%, just remove them
            }
            #endregion

            #region Trim Leading space Folder Names
            newFilename = FileHandler.TrimFolderName(newFilename); //https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.createdirectory?view=net-5.0
            #endregion

            #region Remove Not allowed Chars
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
                  .Replace("|", "")
                  .Replace("?", "")
                  .Replace("*", "")
                  .Replace("/", "")
                  .Replace("\"", "");
            #endregion

            #region Handle Drive letters A:\ a:\ B:\ b:\ C:\ and remove other :
            if (newFilename.Length >= 3 && Char.IsLetter(newFilename[0]) && newFilename[2] == '\\') //x:\
            {
                newFilename = newFilename.Substring(0, 3) + newFilename.Substring(3).Replace(":", "");
            }
            else newFilename = newFilename.Replace(":", "");
            #endregion

            return newFilename;
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

        #region Write
        public static void Write(DataGridView dataGridView, out Dictionary<string, string> renameSuccess, out Dictionary<string, RenameToNameAndResult> renameFailed, out HashSet<string> directoryCreated, bool showFullPathIsUsed)
        {
            using (new WaitCursor())
            {
                renameSuccess = new Dictionary<string, string>();
                renameFailed = new Dictionary<string, RenameToNameAndResult>();
                directoryCreated = new HashSet<string>();

                int columnIndex = DataGridViewHandler.GetColumnIndexFirstFullFilePath(dataGridView, headerNewFilename, false);
                if (columnIndex == -1) return;

                for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                {
                    DataGridViewGenericCell cellGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, columnIndex, rowIndex);

                    if (!cellGridViewGenericCell.CellStatus.CellReadOnly)
                    {
                        DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);

                        #region Get Old filename from grid
                        string oldFilename = dataGridViewGenericRow.RowName;
                        string oldDirectory = dataGridViewGenericRow.HeaderName;
                        string oldFullFilename = FileHandler.CombinePathAndName(oldDirectory, oldFilename);
                        #endregion

                        #region Get New filename from grid and rename
                        if (dataGridViewGenericRow.Metadata != null)
                        {
                            string newFullFilename = FileHandler.CombinePathAndName(oldDirectory, DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex));

                            string newDirectory = Path.GetDirectoryName(newFullFilename);
                            if (!Directory.Exists(newDirectory) && !directoryCreated.Contains(newDirectory)) directoryCreated.Add(newDirectory);
                            FilesCutCopyPasteDrag.RenameFile(oldFullFilename, newFullFilename, ref renameSuccess, ref renameFailed);
                        }
                        #endregion
                    }
                }
            }
        }
        #endregion

        #region GetShortOrFullFilename depend of checkbox
        private static string GetShortOrFullFilename(string newFilenameVariable, Metadata metadata, bool showFullFilePath, string oldFolder, string filename)
        {
            string newFilename = CreateNewFilename(newFilenameVariable, filename, metadata);
            if (showFullFilePath) return FileHandler.CombinePathAndName(oldFolder, newFilename);
            else return newFilename;            
        }
        #endregion

        #region UpdateFilenames(DataGridView dataGridView, string newFilenameVariable)
        public static void UpdateFilenames(DataGridView dataGridView, bool showFullPath)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndexFirstFullFilePath(dataGridView, headerNewFilename, false);
            if (columnIndex == -1) return;

            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);

                if (!dataGridViewGenericRow.IsHeader)
                {
                    PopulateFile(dataGridView, dataGridViewGenericRow.FileEntryAttribute, showFullPath, null);
                }
            }
        }
        #endregion

        #region PopulateFile
        public static void PopulateFile(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, bool showFullPath, Metadata metadataAutoCorrected)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, headerNewFilename)) return;
            if (!FileEntryVersionHandler.IsCurrenOrUpdatedVersion(fileEntryAttribute.FileEntryVersion)) return;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);
            //-----------------------------------------------------------------

            
            int columnIndex = DataGridViewHandler.GetColumnIndexFirstFullFilePath(dataGridView, headerNewFilename, false);
            if (columnIndex != -1)
            {
                //DateTime? dateTime = FileHandler.GetFileStatus(fileEntryAttribute)
                Metadata metadata;
                if (metadataAutoCorrected != null) metadata = metadataAutoCorrected;
                else metadata = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool));

                #region Folder header
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(fileEntryAttribute.Directory), true);
                #endregion

                #region Filename
                string newShortOrFullFilename;
                if (metadata == null) newShortOrFullFilename = "Waiting metadata to be loaded";
                else newShortOrFullFilename = GetShortOrFullFilename(RenameVaribale, metadata, showFullPath, fileEntryAttribute.Directory, fileEntryAttribute.FileName);
                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(
                    fileEntryAttribute.Directory, fileEntryAttribute.FileName, metadata, fileEntryAttribute), newShortOrFullFilename, metadata == null);
                #endregion
            }

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }
        #endregion 

        #region PopulateSelectedFiles
        public static void PopulateSelectedFiles(DataGridView dataGridView, HashSet<FileEntry> imageListViewSelectItems, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns, bool showFullPath)
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
            DataGridViewHandler.SetDataGridViewAllowUserToAddRows(dataGridView, false);
            //Add Columns for all selected files, one column per select file

            DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute(headerNewFilename, DateTime.Now, FileEntryVersion.CurrentVersionInDatabase), null, null, ReadWriteAccess.AllowCellReadAndWrite, showWhatColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out FileEntryVersionCompare fileEntryVersionCompareReason);

            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------

            foreach (FileEntry imageListViewItem in imageListViewSelectItems)
            {
                PopulateFile(dataGridView, 
                    new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.LastWriteDateTime, FileEntryVersion.CurrentVersionInDatabase), showFullPath, null);
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
