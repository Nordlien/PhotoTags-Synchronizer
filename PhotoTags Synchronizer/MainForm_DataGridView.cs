using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        #region DataGridView - DataGridViewSuspendInvoke
        private void DataGridViewSuspendInvoke()
        {
            if (!this.IsHandleCreated) return;
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(DataGridViewSuspendInvoke));
                return;
            }
            string tag = tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString();
            DataGridView dataGridView = GetDataGridViewForTag(tag);
            int threadLazyLoadingQueueSize = ThreadLazyLoadingQueueSize();
            DataGridViewHandler.SuspendLayout(dataGridView, threadLazyLoadingQueueSize);
            LoadDataGridViewProgerss(threadLazyLoadingQueueSize);
        }
        #endregion 

        #region DataGridView - DataGridViewResumeInvoke
        private void DataGridViewResumeInvoke()
        {
            if (!this.IsHandleCreated) return;
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(DataGridViewResumeInvoke));
                return;
            }
            string tag = tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString();
            DataGridView dataGridView = GetDataGridViewForTag(tag);

            int threadLazyLoadingQueueSize = ThreadLazyLoadingQueueSize();
            if (DataGridViewHandler.ResumeLayout(dataGridView, threadLazyLoadingQueueSize))
            {
                PopulateDataGridViewForSelectedItemsExtrasInvoke();
                LoadDataGridViewProgerss(threadLazyLoadingQueueSize);
            }
            
        }
        #endregion 

        #region DataGridView - Populate File - For FileEntryAttribute - Invoke
        private void PopulateDataGridViewForFileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute, int queueCount = 0)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute, int>(PopulateDataGridViewForFileEntryAttributeInvoke), fileEntryAttribute, queueCount);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;

            string tag = tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString();
            if (IsActiveDataGridViewAgregated(tag))
            {
                DataGridView dataGridView = GetDataGridViewForTag(tag);
                PopulateDataGrivViewForFileEntryAttributeAndTag(dataGridView, fileEntryAttribute, tag, queueCount);
            }
            
        }
        #endregion

        #region DataGridView - Populate File - For FileEntryAttribute and Tag
        private void PopulateDataGrivViewForFileEntryAttributeAndTag(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, string tag, int queueCount)
        {
            lock (GlobalData.populateSelectedLock)
            {
                SuspendLayout();

                LoadDataGridViewProgerss(ThreadLazyLoadingQueueSize(), queueCount);
                
                switch (tag)
                {
                    case "Tags":
                        DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        //PopulateDetailViewTagsAndKeywords(dataGridView);
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
                        break;
                    case "Warning":
                        DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        break;
                    case "Properties":
                        DataGridViewHandlerProperties.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                        break;
                    case "Rename":
                        break;
                }
                ResumeLayout();
            }
        }
        #endregion

        #region DataGridView - Populate File - For Selected Files 
        private void PopulateDataGridViewSelectedItemsWithMediaFileVersions(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            List<FileEntryAttribute> lazyLoadingAllVersionOfMediaFile = new List<FileEntryAttribute>();

            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                List<FileEntryAttribute> fileEntryAttributeDateVersions = databaseAndCacheMetadataExiftool.ListFileEntryAttributesCache(MetadataBrokerType.ExifTool, imageListViewItem.FileFullPath);
                lazyLoadingAllVersionOfMediaFile.AddRange(fileEntryAttributeDateVersions);
                //DataGridViewHandlerCommon.AddVisibleFiles(lazyLoadingAllVersionOfMediaFile, fileEntryAttributeDateVersions, showWhatColumns);
            }            
            AddQueueLazyLoadningMetadata(lazyLoadingAllVersionOfMediaFile);
            AddQueueLazyLoadningThumbnail(lazyLoadingAllVersionOfMediaFile);
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

        #region DataGridView - PopulateDataGridViewForSelectedItemsExtrasInvoke
        private void PopulateDataGridViewForSelectedItemsExtrasInvoke()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action(PopulateDataGridViewForSelectedItemsExtrasInvoke));
                return;
            }

            if (GlobalData.IsApplicationClosing) return;

            lock (GlobalData.populateSelectedLock)
            {

                DataGridView dataGridView = GetDataGridViewForTag(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
                switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
                {
                    case "Tags":

                        PopulateDetailViewTagsAndKeywords(dataGridView);
                        break;
                    case "Map":
                        break;
                    case "People":
                        break;
                    case "Date":
                        break;
                    case "ExifTool":
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, ThreadLazyLoadingQueueSize());
                        break;
                    case "Warning":
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, ThreadLazyLoadingQueueSize());
                        break;
                    case "Properties":
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, ThreadLazyLoadingQueueSize());
                        break;
                    case "Rename":
                        break;
                }
            }
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

                DataGridViewSuspendInvoke();

                switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
                {
                    case "Tags":
                        ClearDetailViewTagsAndKeywords();
                        DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords, showWhatColumns);
                        PopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);
                        break;
                    case "Map":
                        splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
                        DataGridViewHandlerMap.TimeZoneShift = GetTimeZoneShift();
                        DataGridViewHandlerMap.AccepedIntervalSecound = GetAccepedIntervalSecound();
                        DataGridViewHandlerMap.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerMap.DatabaseGoogleLocationHistory = databaseGoogleLocationHistory;
                        DataGridViewHandlerMap.DatabaseAndCacheLocationAddress = databaseLocationAddress;
                        DataGridViewHandlerMap.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                        DataGridViewHandlerMap.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeMap, showWhatColumns);
                        PopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);
                        break;
                    case "People":
                        DataGridViewHandlerPeople.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerPeople.SuggestRegionNameNearbyDays = Properties.Settings.Default.SuggestRegionNameNearbyDays;
                        DataGridViewHandlerPeople.SuggestRegionNameTopMostCount = Properties.Settings.Default.SuggestRegionNameTopMostCount;
                        DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizePeoples, showWhatColumns);
                        PopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);
                        break;
                    case "Date":
                        DataGridViewHandlerDate.DatabaseExiftoolData = databaseExiftoolData;
                        DataGridViewHandlerDate.DataGridViewMap = dataGridViewMap;
                        DataGridViewHandlerDate.DataGridViewMapHeaderMedia = DataGridViewHandlerMap.headerMedia;
                        DataGridViewHandlerDate.DataGridViewMapTagCoordinates = DataGridViewHandlerMap.tagCoordinates;
                        DataGridViewHandlerDate.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeDates, showWhatColumns);
                        PopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);
                        break;
                    case "ExifTool":
                        DataGridViewHandlerExiftool.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerExiftool.DatabaseExiftoolData = databaseExiftoolData;
                        DataGridViewHandlerExiftool.exiftoolReader = exiftoolReader;
                        lazyLoading = DataGridViewHandlerExiftool.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool, showWhatColumns);
                        AddQueueLazyLoadningMetadata(lazyLoading);
                        AddQueueLazyLoadningThumbnail(lazyLoading);
                        break;
                    case "Warning":
                        DataGridViewHandlerExiftoolWarnings.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                        DataGridViewHandlerExiftoolWarnings.DatabaseExiftoolWarning = databaseExiftoolWarning;
                        DataGridViewHandlerExiftoolWarnings.exiftoolReader = exiftoolReader;
                        lazyLoading = DataGridViewHandlerExiftoolWarnings.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings, showWhatColumns);
                        AddQueueLazyLoadningMetadata(lazyLoading);
                        AddQueueLazyLoadningThumbnail(lazyLoading);
                        break;
                    case "Properties":
                        DataGridViewHandlerProperties.WindowsPropertyReader = new WindowsPropertyReader();
                        DataGridViewHandlerProperties.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeProperties, showWhatColumns);
                        PopulateDataGridViewForSelectedItemsExtrasInvoke();
                        break;
                    case "Rename":
                        DataGridViewHandlerRename.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                        DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                        DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerRename.FilesCutCopyPasteDrag = filesCutCopyPasteDrag;
                        DataGridViewHandlerRename.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameSize), ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns);                        
                        break;
                }

                DataGridViewResumeInvoke();
            }
        }
        #endregion
    }
}
