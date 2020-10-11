using System;
using System.Windows.Forms;
using Manina.Windows.Forms;
using MetadataLibrary;
using DataGridViewGeneric;
using WindowsProperty;
using static Manina.Windows.Forms.ImageListView;
using System.IO;
using System.Collections.Generic;
using FileDateTime;

namespace PhotoTagsSynchronizer
{
    
    public partial class MainForm : Form
    {
        private void FilesSelected()
        {
            if (GlobalData.IsPopulatingAnything()) return;
            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;

            GlobalData.IsPopulatingImageListView = true;

            //imageListView1.Enabled = false;
            PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems); //GlobalData.isPopulatingSelectedFiles start with true end with false;
            //imageListView1.Enabled = true;

            GlobalData.IsPopulatingImageListView = false;
            imageListView1.Focus();
        }


        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(imageListView1_SelectionChanged), sender, e);
                return;
            }

            FilesSelected();
        }

        private DataGridView GetActiveDataGridView()
        {
            try
            {
                switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
                {
                    case "Tags":
                        return dataGridViewTagsAndKeywords;
                    case "Map":
                        return dataGridViewMap;
                    case "People":
                        return dataGridViewPeople;
                    case "Date":
                        return dataGridViewDate;
                    case "ExifTool":
                        return dataGridViewExifTool;
                    case "Warning":
                        return dataGridViewExifToolWarning;
                    case "Properties":
                        return dataGridViewProperties;
                    case "Rename":
                        return dataGridViewRename;
                    default:
                        return null;
                }
            } catch (Exception e)
            {
                //Why do this been called from another thread
                Logger.Error("GetActiveDataGridView: " + e.Message);
            }
            return null;
        }


        private void PopulateImageOnFileEntryOnSelectedGrivViewInvoke(FileEntryImage fileEntryImage)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryImage>(PopulateImageOnFileEntryOnSelectedGrivViewInvoke), fileEntryImage);
                return;
            }
            
            if (GlobalData.IsPopulatingAnything()) return;

            DataGridView dataGridView = GetActiveDataGridView();
            DataGridViewHandler.UpdateImageOnFile(dataGridView, fileEntryImage);
        }

        private void RefreshImageOnFullFilePathOnGrivView(string fullFilePath)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(RefreshImageOnFullFilePathOnGrivView), fullFilePath);
                return;
            }

            if (GlobalData.IsPopulatingAnything()) return;

            DataGridView dataGridView = GetActiveDataGridView();
            if (dataGridView == null) return;
            
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn)
                {
                    if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn column && column.FileEntryImage.FullFilePath == fullFilePath)
                    {
                        dataGridView.InvalidateCell(columnIndex, -1);
                    }
                }
            }
            DataGridViewHandler.Refresh(dataGridView);
        }

        private void RefreshImageOnSelectFilesOnActiveDataGrivView(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                RefreshImageOnFullFilePathOnGrivView(imageListViewItem.FullFileName);
            }
        }


        private void UpdateMetadataOnSelectedFilesOnActiveDataGrivView(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                PopulateMetadataOnFileOnActiveDataGrivViewInvoke(imageListViewItem.FullFileName);
            }
        }

        private void PopulateMetadataOnFileOnActiveDataGrivViewInvoke(string fullFilePath)
        {           
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(PopulateMetadataOnFileOnActiveDataGrivViewInvoke), fullFilePath);
                return;
            }

            if (GlobalData.IsApplicationClosing) return;

            DataGridView dataGridView = GetActiveDataGridView();

            lock (GlobalData.populateSelectedLock)
            {
                DateTime dateTimeForEditableMediaFile = DataGridViewHandler.DateTimeForEditableMediaFile; //Else File.GetLastWriteTime(fullFilePath)

                switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
                {
                    case "Tags":
                        DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        PopulateDetailViewTagsAndKeywords(dataGridView);
                        break;
                    case "People":
                        DataGridViewHandlerPeople.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                    case "Map":
                        DataGridViewHandlerMap.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                    case "Date":
                        DataGridViewHandlerDate.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                    case "ExifTool":
                        dateTimeForEditableMediaFile = File.GetLastWriteTime(fullFilePath);
                        DataGridViewHandlerExiftool.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                    case "Warning":
                        dateTimeForEditableMediaFile = File.GetLastWriteTime(fullFilePath);
                        DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                    case "Properties":
                        dateTimeForEditableMediaFile = File.GetLastWriteTime(fullFilePath);
                        DataGridViewHandlerProperties.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                    case "Rename":
                        DataGridViewHandlerRename.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                }
                //if (dataGridView != null) dataGridView.Refresh();
            }
        }

        private void UpdateThumbnailOnImageListViewItems(ImageListView imageListView, List<Metadata> updatedMetadata)
        {
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            try
            {
                ImageListViewSuspendLayoutInvoke(imageListView);
                foreach (ImageListViewItem item in imageListView.SelectedItems)
                {
                    if (updatedMetadata == null) ImageListViewUpdateItemThumbnailAndMetadataInvoke(item);
                    else if (Metadata.IsFileInList(updatedMetadata, item.FullFileName)) ImageListViewUpdateItemThumbnailAndMetadataInvoke(item);
                }
                ImageListViewResumeLayoutInvoke(imageListView);
            }
            catch
            {
                //DID ImageListe update failed, because of thread???
            }
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
        }

        

        private void PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            lock (GlobalData.populateSelectedLock)
            {
                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
                PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewInvoke(imageListViewSelectItems);
                UpdateStatusReadWriteStatus_NeedToBeUpated();
            }
        }

        /// <summary>
        /// Populate Active DataGridView with Seleted Files from ImageListView
        /// PS. When selected new files, all DataGridViews are maked as dirty.
        /// </summary>
        /// <param name="imageListViewSelectItems"></param>
        private void PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewInvoke(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            if (InvokeRequired)
            {
                _ = this.BeginInvoke(new Action<ImageListViewSelectedItemCollection>(UpdateMetadataOnSelectedFilesOnActiveDataGrivView), imageListViewSelectItems);
                return;
            }

            if (GlobalData.IsApplicationClosing) return;

            lock (GlobalData.populateSelectedLock)
            {
                foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
                {
                    AddQueueAllUpadtedFileEntry(new FileEntryImage(imageListViewItem.FullFileName, imageListViewItem.DateModified));
                    
                }
                StartThreads();

                DataGridView dataGridView = GetActiveDataGridView();
                switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
                {
                    case "Tags":
                        ClearDetailViewTagsAndKeywords();
                        DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();
                        
                        dataGridView.SuspendLayout();
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state

                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords, showWhatColumns);

                        PopulateDetailViewTagsAndKeywords(dataGridView);
                        dataGridView.Enabled = true;
                        dataGridView.ResumeLayout();

                        break;
                    case "Map":
                        splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();

                        DataGridViewHandlerMap.TimeZoneShift = GetTimeZoneShift();
                        DataGridViewHandlerMap.AccepedIntervalSecound = GetAccepedIntervalSecound();
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerMap.DatabaseGoogleLocationHistory = databaseGoogleLocationHistory;
                        DataGridViewHandlerMap.DatabaseLocationAddress = databaseLocationAddress;
                        DataGridViewHandlerMap.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                        DataGridViewHandlerMap.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizeMap, showWhatColumns);

                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;

                        break;
                    case "People":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();

                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizePeoples, showWhatColumns);

                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "Date":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();
                        DataGridViewHandlerDate.DatabaseExiftoolData = databaseExiftoolData;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizeDates, showWhatColumns);

                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "ExifTool":                        
                        //DataGridViewHandler.SuspendLayout(dataGridView); //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandlerExiftool.DatabaseExiftoolData = databaseExiftoolData;
                        DataGridViewHandlerExiftool.exiftoolReader = exiftoolReader;
                        DataGridViewHandlerExiftool.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool, showWhatColumns);
                        //DataGridViewHandler.ResumeLayout(dataGridView);
                        break;
                    case "Warning":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();
                        DataGridViewHandlerExiftoolWarnings.DatabaseExiftoolWarning = databaseExiftoolWarning;
                        DataGridViewHandlerExiftoolWarnings.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings, showWhatColumns);

                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "Properties":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();
                        DataGridViewHandlerProperties.WindowsPropertyReader = new WindowsPropertyReader();
                        DataGridViewHandlerProperties.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, (DataGridViewSize)Properties.Settings.Default.CellSizeProperties, showWhatColumns);
                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "Rename":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();
                        DataGridViewHandlerRename.FileDateTimeFormats =new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                        DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                        DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        
                        DataGridViewHandlerRename.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameSize), ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns);
                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                }
            }
        }

        

    }
}
