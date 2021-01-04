using System.Windows.Forms;
using System.Collections.Generic;
using MetadataLibrary;
using Exiftool;
using DataGridViewGeneric;
using static Manina.Windows.Forms.ImageListView;
using Manina.Windows.Forms;
using System;
using MetadataPriorityLibrary;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerExiftool
    {
        public static ExiftoolDataDatabase DatabaseExiftoolData { get; set; }
        public static ExiftoolReader exiftoolReader { get; set; }

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

            DataGridViewHandler.SuspendLayout(dataGridView);
            //-----------------------------------------------------------------
            List<FileEntry> fileEntries = DatabaseExiftoolData.ListFileEntryDateVersions(fullFilePath);
            exiftoolReader.MetadataReadPrioity.ReadOnlyOnce();

            foreach (FileEntry fileEntry in fileEntries)
            {         
                int columnIndex = DataGridViewHandler.AddColumnOrUpdate(
                    dataGridView, new FileEntryImage(fileEntry), null, dateTimeForEditableMediaFile,
                    DataGridViewHandler.IsCurrentFile(fileEntry, dateTimeForEditableMediaFile) ? ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true));

                /* Force updated, every time, new data arrives */
                if (columnIndex == -1) columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntry);
                if (columnIndex == -1) continue;

                //Clear old content, in case of new values are updated or deleted
                for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++) DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, null);

                List<ExiftoolData> exifToolDataList = DatabaseExiftoolData.Read(fileEntry);
                string lastRegion = "";
                foreach (ExiftoolData exiftoolData in exifToolDataList)
                {
                    if (lastRegion != exiftoolData.Region)
                    {
                        DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(exiftoolData.Region), null,
                            new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true), false);
                        lastRegion = exiftoolData.Region;
                    }

                    MetadataPriorityKey metadataPriorityKey = new MetadataPriorityKey(exiftoolData.Region, exiftoolData.Command);
                    MetadataPriorityGroup metadataPriorityGroup = null; 
                    bool priorityKeyExisit = exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary.ContainsKey(metadataPriorityKey);
                    if (priorityKeyExisit)
                    {
                        metadataPriorityGroup = new MetadataPriorityGroup(metadataPriorityKey, exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary[metadataPriorityKey]);
                        if (metadataPriorityGroup.MetadataPriorityValues.Composite == CompositeTags.NotDefined) priorityKeyExisit = false;
                    }
                    
                    int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex,
                        new DataGridViewGenericRow(exiftoolData.Region, exiftoolData.Command, true, metadataPriorityKey),
                        exiftoolData.Parameter,
                        new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true), true);

                    if (priorityKeyExisit)
                        DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, metadataPriorityGroup.ToString());
                    else
                        DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, "");
                }
            }

            //-----------------------------------------------------------------
            DataGridViewHandler.ResumeLayout(dataGridView);
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
            DataGridViewHandler.AddColumnSelectedFiles(dataGridView, imageListViewSelectItems, true, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true)); 
            //Add all default rows
            //AddRowsDefault(dataGridView);
            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------

            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                PopulateFile(dataGridView, imageListViewItem.FileFullPath, showWhatColumns, 
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
