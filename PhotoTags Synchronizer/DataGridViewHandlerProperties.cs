﻿using System.Windows.Forms;
using System.Collections.Generic;
using MetadataLibrary;
using DataGridViewGeneric;
using Manina.Windows.Forms;
using WindowsProperty;
using static Manina.Windows.Forms.ImageListView;
using System.Drawing;
using System;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerProperties
    {
        public static bool HasBeenInitialized { get; set; } = false;
        public static WindowsPropertyReader WindowsPropertyReader { get; set; }

        #region Write
        public static void Write(DataGridView dataGridView, int columnIndex)
        {            
            WindowsPropertyReader.Write(dataGridView, columnIndex);
        }
        #endregion

        #region PopulateFile
        public static void PopulateFile(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, ShowWhatColumns showWhatColumns)

        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fileEntryAttribute.FileFullPath)) return;
            if (fileEntryAttribute.FileEntryVersion != FileEntryVersion.CurrentVersionInDatabase) return;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);
            //-----------------------------------------------------------------
            Image image = WindowsPropertyReader.GetThumbnail(fileEntryAttribute.FileFullPath);
            
            DataGridViewHandler.AddColumnOrUpdateNew( 
                dataGridView, fileEntryAttribute, image, null, ReadWriteAccess.DefaultReadOnly, showWhatColumns, 
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), out FileEntryVersionCompare fileEntryVersionCompareReason);
            
            DataGridViewHandler.AddRowAndValueList(dataGridView, fileEntryAttribute, WindowsPropertyReader.Read(fileEntryAttribute.FileFullPath), true);

            DataGridViewHandler.SetColumnPopulatedFlag(dataGridView, DataGridViewHandler.GetColumnIndexPriorities(dataGridView, fileEntryAttribute, out _), true);
            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }
        #endregion

        #region PopulateSelectedFiles
        public static void PopulateSelectedFiles(DataGridView dataGridView, HashSet<FileEntry> imageListViewSelectItems, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns)
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
            DataGridViewHandlerCommon.AddColumnSelectedFiles(dataGridView, null, imageListViewSelectItems, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true)); //ReadOnly until data is read 
            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------
            using (new WaitCursor())
            {
                GlobalData.ProcessCounterReadProperties = imageListViewSelectItems.Count;
                foreach (FileEntry imageListViewItem in imageListViewSelectItems)
                {
                    GlobalData.ProcessCounterReadProperties--;
                    PopulateFile(dataGridView, new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.LastWriteDateTime, FileEntryVersion.CurrentVersionInDatabase), showWhatColumns);
                }
                GlobalData.ProcessCounterReadProperties = 0;
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
