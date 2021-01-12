using DataGridViewGeneric;
using FileDateTime;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WindowsProperty;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        #region imageListView1 SelectionChanged -> FileSelected
        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            imageListView1.Enabled = false; //When Enabled = true, slection was cancelled during Updating the grid
            FilesSelected();
            imageListView1.Enabled = true;
        }
        #endregion

        #region FilesSelected
        private void FilesSelected()
        {
            if (GlobalData.IsPopulatingAnything()) return;
            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;
            
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingImageListView = true;
                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewThread(imageListView1.SelectedItems);
                PopulateImageListeViewOpenWithToolStripThread(imageListView1.SelectedItems);

                DisplayAllQueueStatus();
                GlobalData.IsPopulatingImageListView = false;
            }
            
            imageListView1.Focus();
        }
        #endregion

        #region DataGridView - GetDataGridViewForTag
        private DataGridView GetDataGridViewForTag(string tag)
        {
            try
            {
                switch (tag)
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
        #endregion

        #region DataGridView - Update Image - OnFileEntry - OnSelectedGrivView
        private void UpdateImageOnFileEntryOnSelectedGrivViewInvoke(FileEntryImage fileEntryImage)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryImage>(UpdateImageOnFileEntryOnSelectedGrivViewInvoke), fileEntryImage);
                return;
            }
            
            if (GlobalData.IsPopulatingAnything()) return;

            DataGridView dataGridView = GetDataGridViewForTag(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            if (dataGridView == null) return;

            DataGridViewHandler.UpdateImageOnFile(dataGridView, new FileEntryImage(fileEntryImage.FileFullPath, DataGridViewHandler.DateTimeForEditableMediaFile, fileEntryImage.Image)); 
        }
        #endregion

        #region DataGridView - Refresh Header Image And Regions - OnActiveDataGridView
        private void RefreshHeaderImageAndRegionsOnActiveDataGridView(string fullFilePath)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(RefreshHeaderImageAndRegionsOnActiveDataGridView), fullFilePath);
                return;
            }

            if (GlobalData.IsPopulatingAnything()) return;
            DataGridView dataGridView = GetDataGridViewForTag(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            DataGridViewHandler.RefreshImageForMediaFullFilename(dataGridView, fullFilePath);
        }
        #endregion


        #region Populate - Metadata - OnFile - OnActive DataGrivView - Invoke
        private void PopulateMetadataOnFileOnActiveDataGrivViewInvoke(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                System.Diagnostics.Debug.WriteLine("UpdateMetadataOnSelectedFilesOnActiveDataGrivView: imageListViewItem.FullFileName" + imageListViewItem.FileFullPath);
                PopulateMetadataOnFileOnActiveDataGrivViewInvoke(imageListViewItem.FileFullPath);
            }
        }
        #endregion 

        #region Populate - Metadata - OnFile - OnActive DataGrivView
        private void PopulateMetadataOnFileOnActiveDataGrivView(DataGridView dataGridView, string fullFilePath, string tag)
        {
            lock (GlobalData.populateSelectedLock)
            {
                DateTime dateTimeForEditableMediaFile = DataGridViewHandler.DateTimeForEditableMediaFile; //Else File.GetLastWriteTime(fullFilePath)

                switch (tag)
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
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        break;
                    case "Warning":
                        dateTimeForEditableMediaFile = File.GetLastWriteTime(fullFilePath);
                        DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        break;
                    case "Properties":
                        dateTimeForEditableMediaFile = File.GetLastWriteTime(fullFilePath);
                        DataGridViewHandlerProperties.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        break;
                    case "Rename":
                        DataGridViewHandlerRename.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                }
            }
        }
        #endregion

        #region Populate - Metadata - OnFile - OnActiveDataGrivView Invoke
        private void PopulateMetadataOnFileOnActiveDataGrivViewInvoke(string fullFilePath)
        {           
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(PopulateMetadataOnFileOnActiveDataGrivViewInvoke), fullFilePath);
                return;
            }

            if (GlobalData.IsApplicationClosing) return;
            DataGridView dataGridView;

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "Tags" || GlobalData.IsAgregatedTags)
            {
                dataGridView = GetDataGridViewForTag("Tags");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }
            
            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "Map" || GlobalData.IsAgregatedMap)
            {
                dataGridView = GetDataGridViewForTag("Map");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "People" || GlobalData.IsAgregatedMap)
            {
                dataGridView = GetDataGridViewForTag("People");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "Date" || GlobalData.IsAgregatedPeople)
            {
                dataGridView = GetDataGridViewForTag("Date");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "ExifTool" || GlobalData.IsAgregatedPeople)
            {
                dataGridView = GetDataGridViewForTag("ExifTool");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "Warning" || GlobalData.IsAgregatedPeople)
            {
                dataGridView = GetDataGridViewForTag("Warning");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }
        }
        #endregion


        private void PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewThread(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            Thread threadPopulateDataGridView = new Thread(() => {
                PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewInvoke(imageListView1.SelectedItems);
            });
            threadPopulateDataGridView.Start();
        }

        #region Populate - Details - OnSelected ImageListViewItems - OnActiveDataGridViewInvoke
        /// <summary>
        /// Populate Active DataGridView with Seleted Files from ImageListView
        /// PS. When selected new files, all DataGridViews are maked as dirty.
        /// </summary>
        /// <param name="imageListViewSelectItems"></param>
        private void PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewInvoke(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            
            if (this.InvokeRequired)
            {
                //DEBUG:JTN
                _ = this.BeginInvoke(new Action<ImageListViewSelectedItemCollection>(PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewInvoke), imageListViewSelectItems);
                return;
            }

            if (GlobalData.IsApplicationClosing) return;

            Debug.WriteLine("---lock (GlobalData.populateSelectedLock)");
            lock (GlobalData.populateSelectedLock)
            {                
                foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
                {
                    AddQueueAllUpadtedFileEntry(new FileEntryImage(imageListViewItem.FileFullPath, imageListViewItem.DateModified));
                }
                StartThreads();

                DataGridView dataGridView = GetDataGridViewForTag(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
                switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
                {
                    case "Tags":
                        ClearDetailViewTagsAndKeywords();
                        DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();

                        DataGridViewHandler.SuspendLayout(dataGridView);
                        //dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords, showWhatColumns);
                        PopulateDetailViewTagsAndKeywords(dataGridView);
                        //dataGridView.Enabled = true;
                        DataGridViewHandler.ResumeLayout(dataGridView);

                        break;
                    case "Map":
                        splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);

                        DataGridViewHandlerMap.TimeZoneShift = GetTimeZoneShift();
                        DataGridViewHandlerMap.AccepedIntervalSecound = GetAccepedIntervalSecound();
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerMap.DatabaseGoogleLocationHistory = databaseGoogleLocationHistory;
                        DataGridViewHandlerMap.DatabaseAndCacheLocationAddress = databaseLocationAddress;
                        DataGridViewHandlerMap.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                        DataGridViewHandlerMap.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizeMap, showWhatColumns);

                        DataGridViewHandler.ResumeLayout(dataGridView);
                        dataGridView.Enabled = true;

                        break;
                    case "People":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);

                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerPeople.SuggestRegionNameNearbyDays = Properties.Settings.Default.SuggestRegionNameNearbyDays;
                        DataGridViewHandlerPeople.SuggestRegionNameTopMostCount = Properties.Settings.Default.SuggestRegionNameTopMostCount;

                        DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizePeoples, showWhatColumns);

                        DataGridViewHandler.ResumeLayout(dataGridView);
                        dataGridView.Enabled = true;
                        break;
                    case "Date":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);

                        DataGridViewHandlerDate.DatabaseExiftoolData = databaseExiftoolData;
                        DataGridViewHandlerDate.DataGridViewMap = dataGridViewMap;
                        DataGridViewHandlerDate.DataGridViewMapHeaderMedia = DataGridViewHandlerMap.headerMedia;
                        DataGridViewHandlerDate.DataGridViewMapTagCoordinates = DataGridViewHandlerMap.tagCoordinates;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizeDates, showWhatColumns);

                        DataGridViewHandler.ResumeLayout(dataGridView);
                        dataGridView.Enabled = true;
                        break;
                    case "ExifTool":                        
                        //DataGridViewHandler.SuspendLayout(dataGridView); //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandlerExiftool.DatabaseExiftoolData = databaseExiftoolData;
                        DataGridViewHandlerExiftool.exiftoolReader = exiftoolReader;
                        DataGridViewHandlerExiftool.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        //DataGridViewHandler.ResumeLayout(dataGridView);
                        break;
                    case "Warning":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);
                        DataGridViewHandlerExiftoolWarnings.DatabaseExiftoolWarning = databaseExiftoolWarning;
                        DataGridViewHandlerExiftoolWarnings.exiftoolReader = exiftoolReader;
                        DataGridViewHandlerExiftoolWarnings.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "Properties":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);
                        DataGridViewHandlerProperties.WindowsPropertyReader = new WindowsPropertyReader();
                        DataGridViewHandlerProperties.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, (DataGridViewSize)Properties.Settings.Default.CellSizeProperties, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        DataGridViewHandler.ResumeLayout(dataGridView);
                        dataGridView.Enabled = true;
                        break;
                    case "Rename":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);
                        DataGridViewHandlerRename.FileDateTimeFormats =new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                        DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                        DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerRename.FilesCutCopyPasteDrag = filesCutCopyPasteDrag;

                        DataGridViewHandlerRename.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameSize), ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns);
                        DataGridViewHandler.ResumeLayout(dataGridView);
                        dataGridView.Enabled = true;
                        break;
                }
            }
            Debug.WriteLine("---unlock (GlobalData.populateSelectedLock)");
        }
        #endregion


    }
}
