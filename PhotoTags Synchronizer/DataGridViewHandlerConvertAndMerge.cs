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
    public static class DataGridViewHandlerConvertAndMerge
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static string headerConvertAndMergePath = "Full Path";
        private static string headerConvertAndMergeFilename = "Filename";
        private static string headerConvertAndMergeInfo = "Drag and drop to re-order";

        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static FileDateTimeReader FileDateTimeFormats { get; set; }

        public static FilesCutCopyPasteDrag FilesCutCopyPasteDrag { get; set; }

        public static string RenameVaribale { get; set; }


        #region Write
        public static void Write(DataGridView dataGridView, out Dictionary<string, string> renameSuccess, out Dictionary<string, string> renameFailed)
        {
            renameSuccess = new Dictionary<string, string>();
            renameFailed = new Dictionary<string, string>();

            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, headerConvertAndMergeFilename);
            if (columnIndex == -1) return;

            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericCell cellGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, columnIndex, rowIndex);

                if (!cellGridViewGenericCell.CellStatus.CellReadOnly)
                {
                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                    /*
                    #region Get Old filename from grid
                    string oldFilename = dataGridViewGenericRow.RowName;
                    string oldDirectory = dataGridViewGenericRow.HeaderName;
                    string oldFullFilename = Path.Combine(oldDirectory, oldFilename);
                    #endregion

                    #region Get New filename from grid
                    string newRelativeFilename = Path.Combine(oldDirectory, DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex));
                    string newFullFilename = Path.GetFullPath(newRelativeFilename);
                    #endregion 

                    FilesCutCopyPasteDrag.RenameFile(oldFullFilename, newFullFilename, ref renameSuccess, ref renameFailed);
                    */
                }
            }
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
