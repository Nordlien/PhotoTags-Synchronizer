using System.Windows.Forms;
using System.Collections.Generic;
using Exiftool;
using MetadataLibrary;
using DataGridViewGeneric;
using static Manina.Windows.Forms.ImageListView;
using Manina.Windows.Forms;
using System;
using MetadataPriorityLibrary;
using Thumbnails;
using System.Drawing;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerExiftoolWarnings
    {
        public static bool HasBeenInitialized { get; set; } = false;
        public static ThumbnailDatabaseCache DatabaseAndCacheThumbnail { get; set; }
        public static ExiftoolWarningDatabase DatabaseExiftoolWarning { get; set; }
        public static ExiftoolReader exiftoolReader { get; set; }

        public static List<FileEntryAttribute> ListFileEntryDateVersions(string fullFileName)
        {
            return DatabaseExiftoolWarning.ListFileEntryDateVersions(fullFileName);
        }

        public static void PopulateFile(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, ShowWhatColumns showWhatColumns)

        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fileEntryAttribute.FileFullPath)) return;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);
            //-----------------------------------------------------------------
            List<ExiftoolWarningData> exifToolWarningDataList = DatabaseExiftoolWarning.Read(fileEntryAttribute);

            if (exifToolWarningDataList.Count > 0)
            {
                Image thumbnail = DatabaseAndCacheThumbnail.ReadThumbnailFromCacheOnlyClone(fileEntryAttribute);

                int columnIndex = DataGridViewHandler.AddColumnOrUpdateNew(
                    dataGridView, fileEntryAttribute, thumbnail, null, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), out FileEntryVersionCompare fileEntryVersionCompareReason);

                if (FileEntryVersionHandler.NeedUpdate(fileEntryVersionCompareReason))
                {
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
                                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), true);
                            lastRegion = exiftoolWarningData.NewExiftoolData.Region;
                        }

                        MetadataPriorityKey metadataPriorityKey = new MetadataPriorityKey(exiftoolWarningData.NewExiftoolData.Region, exiftoolWarningData.NewExiftoolData.Command);
                        MetadataPriorityGroup metadataPriorityGroup = null;

                        bool priorityKeyExisit = exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary.ContainsKey(metadataPriorityKey);
                        if (priorityKeyExisit)
                        {
                            metadataPriorityGroup = new MetadataPriorityGroup(metadataPriorityKey, exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary[metadataPriorityKey]);
                            if (metadataPriorityGroup.MetadataPriorityValues.Composite == CompositeTags.NotDefined) priorityKeyExisit = false;
                        }

                        int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex,
                            new DataGridViewGenericRow(exiftoolWarningData.NewExiftoolData.Region, exiftoolWarningData.NewExiftoolData.Command, true, metadataPriorityKey),
                            exiftoolWarningData.WarningMessage,
                            new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), true);

                        if (priorityKeyExisit)
                            DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, metadataPriorityGroup.ToString());
                        else
                            DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, "");
                    }

                    DataGridViewHandler.SetColumnPopulatedFlag(dataGridView, columnIndex, true);
                }
            }

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }

        private static void AddRowsDefault(DataGridView dataGridView)
        {

        }

        public static List<FileEntryAttribute> PopulateSelectedFiles(DataGridView dataGridView, HashSet<FileEntry> imageListViewSelectItems, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return null;
            if (DataGridViewHandler.GetIsAgregated(dataGridView)) return null;
            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return null;
            //Tell that work in progress, can start a new before done.
            DataGridViewHandler.SetIsPopulating(dataGridView, true);
            //Clear current DataGridView
            DataGridViewHandler.Clear(dataGridView, dataGridViewSize);
            //Add Columns for all selected files, one column per select file
            DataGridViewHandlerCommon.AddColumnSelectedFiles(dataGridView, null, DatabaseAndCacheThumbnail, imageListViewSelectItems, true, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true)); 
            //Add all default rows
            AddRowsDefault(dataGridView);
            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------

            List<FileEntryAttribute> allFileEntryAttributeDateVersions = new List<FileEntryAttribute>();
            //Populate one and one of selected files
            foreach (FileEntry imageListViewItem in imageListViewSelectItems)
            {
                List<FileEntryAttribute> fileEntryAttributeDateVersions = DataGridViewHandlerExiftoolWarnings.ListFileEntryDateVersions(imageListViewItem.FileFullPath);
                allFileEntryAttributeDateVersions.AddRange(fileEntryAttributeDateVersions);
            }

            //-----------------------------------------------------------------
            //Unlock
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //Tell that work is done
            DataGridViewHandler.SetIsPopulating(dataGridView, false);
            //-----------------------------------------------------------------

            return allFileEntryAttributeDateVersions;
        }

    }
    
}
