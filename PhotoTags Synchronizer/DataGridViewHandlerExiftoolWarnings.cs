using System.Windows.Forms;
using System.Collections.Generic;
using Exiftool;
using MetadataLibrary;
using DataGridViewGeneric;
using static Manina.Windows.Forms.ImageListView;
using Manina.Windows.Forms;
using System;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerExiftoolWarnings
    {
        public static ExiftoolWarningDatabase DatabaseExiftoolWarning { get; set; }

        public static List<FileEntry> ListFileEntryDateVersions(string fullFileName)
        {
            return DatabaseExiftoolWarning.ListFileEntryDateVersions(fullFileName);
        }

        public static void PopulateFile(DataGridView dataGridView, string fullFilePath, ShowWhatColumns showWhatColumns, DateTime dateTimeForEditableMediaFile)

        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fullFilePath)) return;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);
            //-----------------------------------------------------------------

            List<FileEntry> fileEntries = DataGridViewHandlerExiftoolWarnings.ListFileEntryDateVersions(fullFilePath);
            foreach (FileEntry fileEntry in fileEntries)
            {
                List<ExiftoolWarningData> exifToolWarningDataList = DatabaseExiftoolWarning.Read(fileEntry);

                if (exifToolWarningDataList.Count > 0)
                {
                    int columnIndex = DataGridViewHandler.AddColumnOrUpdate(
                        dataGridView, new FileEntryImage(fileEntry), null, dateTimeForEditableMediaFile,
                        DataGridViewHandler.IsCurrentFile(fileEntry, dateTimeForEditableMediaFile) ? ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                        new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true));

                    /* Force updated, every time, new data arrives */
                    if (columnIndex == -1) columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntry);    
                    if (columnIndex == -1) continue;

                    //Clear old content, in case of new values are updated or deleted
                    for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++) DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, "");

                    string lastRegion = "";
                    foreach (ExiftoolWarningData exiftoolWarningData in exifToolWarningDataList)
                    {
                        if (lastRegion != exiftoolWarningData.NewExiftoolData.Region)
                        {
                            DataGridViewHandler.AddRow(dataGridView, columnIndex, 
                                new DataGridViewGenericRow(exiftoolWarningData.NewExiftoolData.Region),
                                null, 
                                new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true));
                            lastRegion = exiftoolWarningData.NewExiftoolData.Region;
                        }

                        DataGridViewHandler.AddRow(dataGridView, columnIndex, 
                            new DataGridViewGenericRow(exiftoolWarningData.NewExiftoolData.Region, exiftoolWarningData.NewExiftoolData.Command, true, null),
                            exiftoolWarningData.WarningMessage, 
                            new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true));
                    }
                }
            }

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }

        private static void AddRowsDefault(DataGridView dataGridView)
        {

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
            DataGridViewHandler.AddColumnSelectedFiles(dataGridView, imageListViewSelectItems, true, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true)); 
            //Add all default rows
            AddRowsDefault(dataGridView);
            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------


            //Populate one and one of selected files
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
