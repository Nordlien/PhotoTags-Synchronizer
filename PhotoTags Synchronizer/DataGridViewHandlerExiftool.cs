using System.Windows.Forms;
using System.Collections.Generic;
using MetadataLibrary;
using Exiftool;
using DataGridViewGeneric;
using static Manina.Windows.Forms.ImageListView;
using Manina.Windows.Forms;
using System;
using MetadataPriorityLibrary;
using System.Drawing;
using Thumbnails;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerExiftool
    {
        public static bool HasBeenInitialized { get; set; } = false;
        public static ThumbnailDatabaseCache DatabaseAndCacheThumbnail { get; set; }
        public static ExiftoolDataDatabase DatabaseExiftoolData { get; set; }
        public static ExiftoolReader exiftoolReader { get; set; }

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
            exiftoolReader.MetadataReadPrioity.ReadOnlyOnce();

            Image thumbnail = DatabaseAndCacheThumbnail.ReadThumbnailFromCacheOnlyClone(fileEntryAttribute);
            int columnIndex = DataGridViewHandler.AddColumnOrUpdateNew(
                dataGridView, fileEntryAttribute, thumbnail, null, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns, 
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), out FileEntryVersionCompare fileEntryVersionCompareReason);
          
            if (FileEntryVersionHandler.NeedUpdate(fileEntryVersionCompareReason))
            {
                //Clear old content, in case of new values are updated or deleted
                for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++) DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, null);

                List<ExiftoolData> exifToolDataList = DatabaseExiftoolData.Read(fileEntryAttribute);
                string lastRegion = "";
                foreach (ExiftoolData exiftoolData in exifToolDataList)
                {
                    if (lastRegion != exiftoolData.Region)
                    {
                        DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(exiftoolData.Region), null,
                            new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), false);
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
                        new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), true);

                    if (priorityKeyExisit)
                        DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, metadataPriorityGroup.ToString());
                    else
                        DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, "");
                }

                DataGridViewHandler.SetColumnPopulatedFlag(dataGridView, columnIndex, true);
            }

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
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
            DataGridViewHandlerCommon.AddColumnSelectedFiles(dataGridView, null, DatabaseAndCacheThumbnail, imageListViewSelectItems, true, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns, new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true)); 
            //Add all default rows
            //AddRowsDefault(dataGridView);
            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------
            List<FileEntryAttribute> allFileEntryAttributeDateVersions = new List<FileEntryAttribute>();
            foreach (FileEntry imageListViewItem in imageListViewSelectItems)
            {
                List<FileEntryAttribute> fileEntryAttributeDateVersions = DatabaseExiftoolData.ListFileEntryDateVersions(imageListViewItem.FileFullPath);
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
