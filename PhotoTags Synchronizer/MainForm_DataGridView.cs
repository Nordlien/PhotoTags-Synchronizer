using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DataGridViewGeneric;
using FileDateTime;
using Manina.Windows.Forms;
using MetadataLibrary;
using TimeZone;
using WindowsProperty;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
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
            }
            catch (Exception e)
            {
                //Why do this been called from another thread
                Logger.Error("GetActiveDataGridView: " + e.Message);
            }
            return null;
        }
        #endregion

        #region DataGridView - Update Image - OnFileEntry - OnSelectedGrivView
        private void UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke(FileEntryAttribute fileEntryAttribute, Image image)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute, Image>(UpdateImageOnFileEntryAttributeOnSelectedGrivViewInvoke), fileEntryAttribute, image);
                return;
            }
            //if (GlobalData.IsPopulatingAnything()) return;

            DataGridView dataGridView = GetDataGridViewForTag(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            if (dataGridView == null) return;

            lock (GlobalData.populateSelectedLock) DataGridViewHandler.SetDataGridImageOnFileEntryAttribute(dataGridView, fileEntryAttribute, image);
        }
        #endregion

        #region DataGridView - Refresh Header Image And Regions - OnActiveDataGridView
        private void RefreshHeaderImageAndRegionsOnActiveDataGridView(FileEntryAttribute fileEntryAttribute)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute>(RefreshHeaderImageAndRegionsOnActiveDataGridView), fileEntryAttribute);
                return;
            }

            if (GlobalData.IsPopulatingAnything()) return;
            DataGridView dataGridView = GetDataGridViewForTag(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            DataGridViewHandler.RefreshImageForMediaFullFilename(dataGridView, fileEntryAttribute.FileFullPath);
        }
        #endregion

        #region DataGridVIew - IsActiveDataGridViewAgregated
        private bool IsActiveDataGridViewAgregated(string tag)
        {
            bool isAgregated = false;
            switch (tag)
            {
                case "Tags":
                    isAgregated = GlobalData.IsAgregatedTags;
                    break;
                case "People":
                    isAgregated = GlobalData.IsAgregatedPeople;
                    break;
                case "Map":
                    isAgregated = GlobalData.IsAgregatedMap;
                    break;
                case "Date":
                    isAgregated = GlobalData.IsAgregatedDate;
                    break;
                case "ExifTool":
                    isAgregated = GlobalData.IsAgregatedExiftoolTags;
                    break;
                case "Warning":
                    isAgregated = GlobalData.IsAgregatedExiftoolWarning;
                    break;
                case "Properties":
                    isAgregated = GlobalData.IsAgregatedProperties;
                    break;
                case "Rename":
                    isAgregated = GlobalData.IsAgregatedRename;
                    break;
            }
            return isAgregated;
        }
        #endregion

        #region DataGridView - Populate File - For FileEntryAttribute and Tag
        private void PopulateDataGrivViewForFileEntryAttributeAndTag(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, string tag)
        {
            lock (GlobalData.populateSelectedLock)
            {
                DataGridViewHandler.SuspendLayout(dataGridView);
                switch (tag)
                {
                    case "Tags":
                        DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        PopulateDetailViewTagsAndKeywords(dataGridView);
                        break;
                    case "People":
                        DataGridViewHandlerPeople.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        break;
                    case "Map":
                        DataGridViewHandlerMap.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        break;
                    case "Date":
                        DataGridViewHandlerDate.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        break;
                    case "ExifTool":
                        DataGridViewHandlerExiftool.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        break;
                    case "Warning":
                        DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        break;
                    case "Properties":
                        DataGridViewHandlerProperties.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        break;
                    case "Rename":
                        //DataGridViewHandlerRename.PopulateFile(dataGridView, fileVersionAttribute, showWhatColumns);
                        break;
                }
                DataGridViewHandler.ResumeLayout(dataGridView);
            }
        }
        #endregion

        #region DataGridView - Populate File - For FileEntryAttribute - Invoke
        private void PopulateDataGridViewForFileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute>(PopulateDataGridViewForFileEntryAttributeInvoke), fileEntryAttribute);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;

            string tag = tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString();
            if (IsActiveDataGridViewAgregated(tag))
            {
                DataGridView dataGridView = GetDataGridViewForTag(tag);
                PopulateDataGrivViewForFileEntryAttributeAndTag(dataGridView, fileEntryAttribute, tag);
            }
        }
        #endregion

        #region DataGridView - Populate File - For Selected Files 
        private void PopulateDataGridVIewForSelectedItemsInvoke(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                PopulateDataGridViewForFileEntryAttributeInvoke(new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.DateModified, FileEntryVersion.Current));
            }
        }
        #endregion 

        #region DataGridView - Populate Selected Files - OnActiveDataGridView - Thread
        private void PopulateDataGridViewForSelectedItemsThread(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            Thread threadPopulateDataGridView = new Thread(() => {
                PopulateDataGridViewForSelectedItemsInvoke(imageListView1.SelectedItems);
            });

            threadPopulateDataGridView.Start();
        }
        #endregion 

        #region DataGridView - Populate Selected Files - OnActiveDataGridView - Invoke
        /// <summary>
        /// Populate Active DataGridView with Seleted Files from ImageListView
        /// PS. When selected new files, all DataGridViews are maked as dirty.
        /// </summary>
        /// <param name="imageListViewSelectItems"></param>
        private void PopulateDataGridViewForSelectedItemsInvoke(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action<ImageListViewSelectedItemCollection>(PopulateDataGridViewForSelectedItemsInvoke), imageListViewSelectItems);
                return;
            }

            if (GlobalData.IsApplicationClosing) return;

            lock (GlobalData.populateSelectedLock)
            {
                StartThreads();

                DataGridView dataGridView = GetDataGridViewForTag(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
                List<FileEntryAttribute> lazyLoading;

                switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
                {
                    case "Tags":
                        ClearDetailViewTagsAndKeywords();
                        DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();

                        DataGridViewHandler.SuspendLayout(dataGridView);
                        //dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        
                        lazyLoading = DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords, showWhatColumns);
                        AddQueueLazyLoadningMetadata(lazyLoading);
                        AddQueueLazyLoadningThumbnail(lazyLoading);

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

                        DataGridViewHandlerMap.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerMap.DatabaseGoogleLocationHistory = databaseGoogleLocationHistory;
                        DataGridViewHandlerMap.DatabaseAndCacheLocationAddress = databaseLocationAddress;
                        DataGridViewHandlerMap.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                        lazyLoading = DataGridViewHandlerMap.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeMap, showWhatColumns);
                        AddQueueLazyLoadningMetadata(lazyLoading);
                        AddQueueLazyLoadningThumbnail(lazyLoading);


                        DataGridViewHandler.ResumeLayout(dataGridView);
                        dataGridView.Enabled = true;

                        break;
                    case "People":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);

                        DataGridViewHandlerPeople.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerPeople.SuggestRegionNameNearbyDays = Properties.Settings.Default.SuggestRegionNameNearbyDays;
                        DataGridViewHandlerPeople.SuggestRegionNameTopMostCount = Properties.Settings.Default.SuggestRegionNameTopMostCount;

                        lazyLoading = DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizePeoples, showWhatColumns);
                        AddQueueLazyLoadningMetadata(lazyLoading);
                        AddQueueLazyLoadningThumbnail(lazyLoading);


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

                        DataGridViewHandlerDate.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        lazyLoading = DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeDates, showWhatColumns);
                        AddQueueLazyLoadningMetadata(lazyLoading);
                        AddQueueLazyLoadningThumbnail(lazyLoading);

                        DataGridViewHandler.ResumeLayout(dataGridView);
                        dataGridView.Enabled = true;
                        break;
                    case "ExifTool":
                        //DataGridViewHandler.SuspendLayout(dataGridView); //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandlerExiftool.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerExiftool.DatabaseExiftoolData = databaseExiftoolData;
                        DataGridViewHandlerExiftool.exiftoolReader = exiftoolReader;
                        lazyLoading = DataGridViewHandlerExiftool.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        AddQueueLazyLoadningMetadata(lazyLoading);
                        AddQueueLazyLoadningThumbnail(lazyLoading);

                        //DataGridViewHandler.ResumeLayout(dataGridView);
                        break;
                    case "Warning":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);
                        DataGridViewHandlerExiftoolWarnings.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerExiftoolWarnings.DatabaseExiftoolWarning = databaseExiftoolWarning;
                        DataGridViewHandlerExiftoolWarnings.exiftoolReader = exiftoolReader;
                        lazyLoading = DataGridViewHandlerExiftoolWarnings.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        AddQueueLazyLoadningMetadata(lazyLoading);
                        AddQueueLazyLoadningThumbnail(lazyLoading);

                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "Properties":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);
                        //DataGridViewHandlerProperties.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerProperties.WindowsPropertyReader = new WindowsPropertyReader();
                        DataGridViewHandlerProperties.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeProperties, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        DataGridViewHandler.ResumeLayout(dataGridView);
                        dataGridView.Enabled = true;
                        break;
                    case "Rename":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandler.SuspendLayout(dataGridView);
                        DataGridViewHandlerRename.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                        DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                        DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerRename.FilesCutCopyPasteDrag = filesCutCopyPasteDrag;
                        DataGridViewHandlerRename.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameSize), ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns);                        
                        DataGridViewHandler.ResumeLayout(dataGridView);
                        dataGridView.Enabled = true;
                        break;
                }
            }
        }
        #endregion
    }
}
