using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DataGridViewGeneric;
using FileDateTime;
using Manina.Windows.Forms;
using MetadataLibrary;
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
                    case LinkTabAndDataGridViewNameTags:
                        return dataGridViewTagsAndKeywords;
                    case LinkTabAndDataGridViewNameMap:
                        return dataGridViewMap;
                    case LinkTabAndDataGridViewNamePeople:
                        return dataGridViewPeople;
                    case LinkTabAndDataGridViewNameDates:
                        return dataGridViewDate;
                    case LinkTabAndDataGridViewNameExiftool:
                        return dataGridViewExifTool;
                    case LinkTabAndDataGridViewNameWarnings:
                        return dataGridViewExifToolWarning;
                    case LinkTabAndDataGridViewNameProperties:
                        return dataGridViewProperties;
                    case LinkTabAndDataGridViewNameRename:
                        return dataGridViewRename;
                    case LinkTabAndDataGridViewNameConvertAndMerge:
                        return dataGridViewConvertAndMerge; 
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                //Why do this been called from another thread
                Logger.Error(ex, "GetActiveDataGridView");
            }
            return null;
        }
        #endregion

        #region DataGridView - GetActiveTabTag()
        private string GetActiveTabTag()
        {
            return tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString();
        }
        #endregion

        #region DataGridView - GetActiveTabDataGridView()
        private DataGridView GetActiveTabDataGridView()
        {
            return GetDataGridViewForTag(GetActiveTabTag());
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

            DataGridView dataGridView = GetActiveTabDataGridView();
            if (dataGridView == null) return;

            lock (GlobalData.populateSelectedLock) DataGridViewHandler.SetDataGridImageOnFileEntryAttribute(dataGridView, fileEntryAttribute, image);
        }
        #endregion

        #region DataGridVIew - IsActiveDataGridViewAgregated
        private bool IsActiveDataGridViewAgregated(string tag)
        {
            bool isAgregated = false;
            switch (tag)
            {
                case LinkTabAndDataGridViewNameTags:
                    isAgregated = GlobalData.IsAgregatedTags;
                    break;
                case LinkTabAndDataGridViewNamePeople:
                    isAgregated = GlobalData.IsAgregatedPeople;
                    break;
                case LinkTabAndDataGridViewNameMap:
                    isAgregated = GlobalData.IsAgregatedMap;
                    break;
                case LinkTabAndDataGridViewNameDates:
                    isAgregated = GlobalData.IsAgregatedDate;
                    break;
                case LinkTabAndDataGridViewNameExiftool:
                    isAgregated = GlobalData.IsAgregatedExiftoolTags;
                    break;
                case LinkTabAndDataGridViewNameWarnings:
                    isAgregated = GlobalData.IsAgregatedExiftoolWarning;
                    break;
                case LinkTabAndDataGridViewNameProperties:
                    isAgregated = GlobalData.IsAgregatedProperties;
                    break;
                case LinkTabAndDataGridViewNameRename:
                    isAgregated = GlobalData.IsAgregatedRename;
                    break;
                case LinkTabAndDataGridViewNameConvertAndMerge:
                    isAgregated = GlobalData.IsAgregatedConvertAndMerge;
                    break;
            }
            return isAgregated;
        }
        #endregion

        #region DataGridView - Populate File - For FileEntryAttribute missing Tag - Invoke
        private void PopulateDataGridViewForFileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute>(PopulateDataGridViewForFileEntryAttributeInvoke), fileEntryAttribute);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;

            string tag = GetActiveTabTag();
            if (IsActiveDataGridViewAgregated(tag))
            {
                DataGridView dataGridView = GetDataGridViewForTag(tag);
                PopulateDataGrivViewForFileEntryAttributeAndTag(dataGridView, fileEntryAttribute, tag);
            }

        }
        #endregion

        #region DataGridView - Populate File - For FileEntryAttribute and Tag
        private void PopulateDataGrivViewForFileEntryAttributeAndTag(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, string tag)
        {
            lock (GlobalData.populateSelectedLock)
            {
                bool isFileInDataGridView = DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fileEntryAttribute.FileFullPath);
                    
                DataGridViewHandler.SuspendLayout(dataGridView, isFileInDataGridView);

                if (isFileInDataGridView)
                {
                    switch (tag)
                    {
                        case LinkTabAndDataGridViewNameTags:
                            DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                            break;
                        case LinkTabAndDataGridViewNamePeople:
                            DataGridViewHandlerPeople.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                            break;
                        case LinkTabAndDataGridViewNameMap:
                            DataGridViewHandlerMap.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                            break;
                        case LinkTabAndDataGridViewNameDates:
                            DataGridViewHandlerDate.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                            break;
                        case LinkTabAndDataGridViewNameExiftool:
                            DataGridViewHandlerExiftool.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                            break;
                        case LinkTabAndDataGridViewNameProperties:
                            DataGridViewHandlerProperties.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);
                            break;
                        case LinkTabAndDataGridViewNameRename:
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                #region PopulateTreeViewFolderFilter
                if (!IsPopulateTreeViewFolderFilterThreadRunning)
                {
                    PopulateTreeViewFolderFilterAdd(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.ExifTool));
                    PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke();
                }
                #endregion

                int queueCount = ThreadLazyLoadingDataGridViewQueueSizeDirty();
                if (isFileInDataGridView) LazyLoadingDataGridViewProgressUpdateStatus(queueCount); //Update progressbar when File In DataGridView

                DataGridViewHandler.ResumeLayout(dataGridView);
                
                if (queueCount == 0)
                {
                    //LazyLoadMissingLock();
                    if (isFileInDataGridView) PopulateDataGridViewForSelectedItemsExtrasInvoke();
                    LazyLoadingDataGridViewProgressEndReached();
                }
            }
        }
        #endregion

        #region DataGridView - LazyLoad - Populate File - For Selected Files 
        private void LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            List<FileEntryAttribute> lazyLoadingAllExiftoolVersionOfMediaFile = new List<FileEntryAttribute>();

            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                List<FileEntryAttribute> fileEntryAttributeDateVersions = databaseAndCacheMetadataExiftool.ListFileEntryAttributesCache(MetadataBrokerType.ExifTool, imageListViewItem.FileFullPath);
                //When list is 0, then Metadata was not readed from mediafile and needs put back in read queue
                if (fileEntryAttributeDateVersions.Count == 0)
                {
                    AddQueueLazyLoadingDataGridViewMetadataReadToCacheOrUpdateFromSoruce(new FileEntry(imageListViewItem.FileFullPath, imageListViewItem.DateModified));
                }
                lazyLoadingAllExiftoolVersionOfMediaFile.AddRange(fileEntryAttributeDateVersions);
            }

            AddQueueLazyLoadningDataGridViewMetadataLock(lazyLoadingAllExiftoolVersionOfMediaFile);
            AddQueueLazyLoadningDataGridViewThumbnailLock(lazyLoadingAllExiftoolVersionOfMediaFile);

            StartThreads();
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
                DataGridView dataGridView = GetActiveTabDataGridView();
                switch (GetActiveTabTag())
                {
                    case LinkTabAndDataGridViewNameTags:
                        PopulateDetailViewTagsAndKeywords(dataGridView);
                        break;
                    case LinkTabAndDataGridViewNameMap:
                        break;
                    case LinkTabAndDataGridViewNamePeople:
                        break;
                    case LinkTabAndDataGridViewNameDates:
                        break;
                    case LinkTabAndDataGridViewNameExiftool:
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, ThreadLazyLoadingDataGridViewQueueSizeDirty());
                        break;
                    case LinkTabAndDataGridViewNameWarnings:
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, ThreadLazyLoadingDataGridViewQueueSizeDirty());
                        break;
                    case LinkTabAndDataGridViewNameProperties:
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, ThreadLazyLoadingDataGridViewQueueSizeDirty());
                        break;
                    case LinkTabAndDataGridViewNameRename:
                        break;
                    case LinkTabAndDataGridViewNameConvertAndMerge:
                        break;
                    default:
                        throw new NotImplementedException();
                }
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
                DataGridView dataGridView = GetActiveTabDataGridView();

                if (DataGridViewHandler.GetIsAgregated(dataGridView)) return;

                using (new WaitCursor())
                {
                    List<FileEntryAttribute> lazyLoading;
                    DataGridViewHandler.SuspendLayout(dataGridView, true);


                    switch (GetActiveTabTag())
                    {
                        case LinkTabAndDataGridViewNameTags:
                            ClearDetailViewTagsAndKeywords();
                            DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords, showWhatColumns);
                            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);

                            break;
                        case LinkTabAndDataGridViewNameMap:
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
                            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNamePeople:
                            PopulatePeopleToolStripMenuItems();
                            DataGridViewHandlerPeople.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerPeople.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerPeople.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerPeople.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerPeople.SuggestRegionNameNearbyDays = Properties.Settings.Default.SuggestRegionNameNearbyDays;
                            DataGridViewHandlerPeople.SuggestRegionNameTopMostCount = Properties.Settings.Default.SuggestRegionNameTopMostCount;
                            DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizePeoples, showWhatColumns);
                            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNameDates:
                            DataGridViewHandlerDate.DatabaseExiftoolData = databaseExiftoolData;
                            DataGridViewHandlerDate.DataGridViewMap = dataGridViewMap;
                            DataGridViewHandlerDate.DataGridViewMapHeaderMedia = DataGridViewHandlerMap.headerMedia;
                            DataGridViewHandlerDate.DataGridViewMapTagCoordinates = DataGridViewHandlerMap.tagCoordinates;
                            DataGridViewHandlerDate.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerDate.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerDate.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerDate.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeDates, showWhatColumns);
                            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNameExiftool:
                            DataGridViewHandlerExiftool.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerExiftool.DatabaseExiftoolData = databaseExiftoolData;
                            DataGridViewHandlerExiftool.exiftoolReader = exiftoolReader;
                            lazyLoading = DataGridViewHandlerExiftool.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool, showWhatColumns);
                            AddQueueLazyLoadningDataGridViewMetadataLock(lazyLoading);
                            AddQueueLazyLoadningDataGridViewThumbnailLock(lazyLoading);
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            DataGridViewHandlerExiftoolWarnings.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerExiftoolWarnings.DatabaseExiftoolWarning = databaseExiftoolWarning;
                            DataGridViewHandlerExiftoolWarnings.exiftoolReader = exiftoolReader;
                            lazyLoading = DataGridViewHandlerExiftoolWarnings.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings, showWhatColumns);
                            AddQueueLazyLoadningDataGridViewMetadataLock(lazyLoading);
                            AddQueueLazyLoadningDataGridViewThumbnailLock(lazyLoading);
                            break;
                        case LinkTabAndDataGridViewNameProperties:
                            DataGridViewHandlerProperties.WindowsPropertyReader = new WindowsPropertyReader();
                            DataGridViewHandlerProperties.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, (DataGridViewSize)Properties.Settings.Default.CellSizeProperties, showWhatColumns);
                            PopulateDataGridViewForSelectedItemsExtrasInvoke();
                            break;
                        case LinkTabAndDataGridViewNameRename:
                            DataGridViewHandlerRename.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                            DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerRename.FilesCutCopyPasteDrag = filesCutCopyPasteDrag;
                            DataGridViewHandlerRename.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize), ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns);
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            DataGridViewHandlerConvertAndMerge.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            DataGridViewHandlerConvertAndMerge.RenameVaribale = Properties.Settings.Default.RenameVariable;
                            DataGridViewHandlerConvertAndMerge.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerConvertAndMerge.FilesCutCopyPasteDrag = filesCutCopyPasteDrag;
                            DataGridViewHandlerConvertAndMerge.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize), ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns);
                            break;
                        default:
                            throw new NotImplementedException();

                    }
                    DataGridViewHandler.ResumeLayout(dataGridView);
                }
            }
            StartThreads();
        }
        #endregion
    }
}
