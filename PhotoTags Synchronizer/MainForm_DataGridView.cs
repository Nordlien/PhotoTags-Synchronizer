using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DataGridViewGeneric;
using FileDateTime;
using MetadataLibrary;
using WindowsProperty;
using Krypton.Toolkit;
using Exiftool;
using System.IO;
using FileHandeling;
using Manina.Windows.Forms;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region DataGridView - UpdateColumnThumbnail - OnFileEntryAttribute - OnSelectedGrivView
        private void DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute(FileEntryAttribute fileEntryAttribute, Image image)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute, Image>(DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute), fileEntryAttribute, image);
                return;
            }

            DataGridView dataGridView = GetActiveTabDataGridView();
            if (dataGridView == null) return;

            lock (GlobalData.populateSelectedLock) DataGridViewHandler.SetColumnHeaderThumbnail(dataGridView, fileEntryAttribute, image);
        }
        #endregion

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
                        return dataGridViewExiftool;
                    case LinkTabAndDataGridViewNameWarnings:
                        return dataGridViewExiftoolWarning;
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
            if (kryptonWorkspaceCellToolbox.SelectedPage == null)
                return null;

            return kryptonWorkspaceCellToolbox.SelectedPage.Tag.ToString();
        }
        #endregion

        #region DataGridView - GetActiveTabDataGridView()
        private DataGridView GetActiveTabDataGridView()
        {
            return GetDataGridViewForTag(GetActiveTabTag());
        }
        #endregion 

        #region DataGridView - GetAnyTabDataGridView()
        private DataGridView GetAnyAgregatedDataGridView()
        {
            if (DataGridViewHandler.GetIsAgregated(dataGridViewTagsAndKeywords)) return dataGridViewTagsAndKeywords;
            if (DataGridViewHandler.GetIsAgregated(dataGridViewPeople)) return dataGridViewPeople;
            if (DataGridViewHandler.GetIsAgregated(dataGridViewMap)) return dataGridViewMap;
            if (DataGridViewHandler.GetIsAgregated(dataGridViewDate)) return dataGridViewDate;
            return dataGridViewTagsAndKeywords; //Also if empty
        }
        #endregion 

        #region DataGridView - IsActiveDataGridViewAgregated
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

        #region DataGridView - Populate - ExtrasAsDropdownAndColumnSizesInvoke (Populate DataGridView Extras)
        private void DataGridView_Populate_ExtrasAsDropdownAndColumnSizesInvoke()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action(DataGridView_Populate_ExtrasAsDropdownAndColumnSizesInvoke));
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
                        List<DataGridViewGenericColumn> dataGridViewGenericColumns = DataGridViewHandler.GetColumnsDataGridViewGenericColumnCurrentOrAutoCorrect(dataGridView, false);
                        PopulatePeopleToolStripMenuItems(dataGridViewGenericColumns,
                                Properties.Settings.Default.SuggestRegionNameNearbyDays,
                                //Properties.Settings.Default.SuggestRegionNameNearByCount,
                                Properties.Settings.Default.SuggestRegionNameNearByContextMenuCount,
                                Properties.Settings.Default.SuggestRegionNameMostUsedContextMenuCount,
                                Properties.Settings.Default.ApplicationSizeOfRegionNamesGroup,
                                Properties.Settings.Default.RenameDateFormats);

                        break;
                    case LinkTabAndDataGridViewNameDates:
                        break;
                    case LinkTabAndDataGridViewNameExiftool:
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, GetDataGridView_ColumnsEntriesInReadQueues_Count());
                        break;
                    case LinkTabAndDataGridViewNameWarnings:
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, GetDataGridView_ColumnsEntriesInReadQueues_Count());
                        break;
                    case LinkTabAndDataGridViewNameProperties:
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, GetDataGridView_ColumnsEntriesInReadQueues_Count());
                        break;
                    case LinkTabAndDataGridViewNameRename:
                        break;
                    case LinkTabAndDataGridViewNameConvertAndMerge:
                        break;
                    default: throw new NotImplementedException();
                }

                
            }
        }
        #endregion

        #region DataGridView - Populate File - For FileEntryAttribute missing Tag - Invoke
        private void DataGridView_Populate_FileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute>(DataGridView_Populate_FileEntryAttributeInvoke), fileEntryAttribute);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                string tag = GetActiveTabTag();
                if (!string.IsNullOrWhiteSpace(tag) && IsActiveDataGridViewAgregated(tag))
                {
                    DataGridView dataGridView = GetDataGridViewForTag(tag);
                    if (dataGridView != null) DataGridView_Populate_FileEntryAttribute(dataGridView, fileEntryAttribute, tag);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region DataGridView - Populate File - For FileEntryAttribute missing Tag - Invoke
        private void DataGridView_SetColumnVisibleStatus_FileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute, bool visible)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute, bool>(DataGridView_SetColumnVisibleStatus_FileEntryAttributeInvoke), fileEntryAttribute, visible);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                DataGridViewHandler.SetColumnVisibleStatus(dataGridViewTagsAndKeywords, fileEntryAttribute, visible);
                DataGridViewHandler.SetColumnVisibleStatus(dataGridViewPeople, fileEntryAttribute, visible);
                DataGridViewHandler.SetColumnVisibleStatus(dataGridViewMap, fileEntryAttribute, visible);
                DataGridViewHandler.SetColumnVisibleStatus(dataGridViewDate, fileEntryAttribute, visible);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region DataGridView - Populate - Metadata
        private void DataGridView_Populate_Metadata(Metadata metadataToSave)
        {
            DataGridView_Populate_FileEntryAttribute(GetActiveTabDataGridView(),
                new FileEntryAttribute(metadataToSave.FileEntry, FileEntryVersion.AutoCorrect),
                GetActiveTabTag(), metadataToSave);
        }
        #endregion 

        #region DataGridView - Populate - FileEntryAttribute -> PopulateDataGridViewForSelectedItemsExtrasDelayed();
        private void DataGridView_Populate_FileEntryAttribute(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, string tabTag, Metadata metadataAutoCorrect = null)
        {
            lock (GlobalData.populateSelectedLock)
            {
                #region isFileInDataGridView
                bool isFilSelectedInImageListView = ImageListViewHandler.DoesExistInSelectedFiles(imageListView1, fileEntryAttribute.FileFullPath);
                #endregion

                if (isFilSelectedInImageListView)
                {
                    DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, isFilSelectedInImageListView); //Will not suspend when Column Don't exist, but counter will increase

                    #region Popuate File
                    switch (tabTag)
                    {
                        case LinkTabAndDataGridViewNameTags:
                            DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);
                            //if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNamePeople:
                            DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNameMap:
                            DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNameDates:
                            DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);
                            break;

                        case LinkTabAndDataGridViewNameExiftool:
                            DataGridViewHandlerExiftool.PopulateFile(dataGridViewExiftool, fileEntryAttribute, showWhatColumns);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridViewExiftoolWarning, fileEntryAttribute, showWhatColumns);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNameProperties:
                            DataGridViewHandlerProperties.PopulateFile(dataGridViewProperties, fileEntryAttribute, showWhatColumns);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNameRename:
                            DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridViewConvertAndMerge, fileEntryAttribute);
                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    #endregion

                    #region Check if got thumbnail, if not, push to read queue
                    Image thumbnailImage = databaseAndCacheThumbnailPoster.ReadThumbnailFromCacheOnly(fileEntryAttribute);
                    if (thumbnailImage == null)
                        AddQueueLazyLoadningMediaThumbnailLock(fileEntryAttribute);
                    #endregion

                    int queueCount = GetDataGridView_ColumnsEntriesInReadQueues_Count();
                    LazyLoadingDataGridViewProgressUpdateStatus(GetCircleProgressCount(true, 0));
                    if (queueCount == 0) DataGridView_Populate_ExtrasAsDropdownAndColumnSizesInvoke();

                    DataGridViewHandler.ResumeLayoutDelayed(dataGridView); //Will resume when counter reach 0
                }
            }
        }
        #endregion

        #region DataGridView - LazyLoad - AfterPopulateSelectedFilesLazyLoadOtherFileVersions 
        private void DataGridView_AfterPopulateSelectedFiles_LazyLoadFromDatabaseThenSourceAllVersions(HashSet<FileEntry> imageListViewSelectItems)
        {
            List<FileEntryAttribute> lazyLoadingAllExiftoolVersionOfMediaFile = new List<FileEntryAttribute>();

            foreach (FileEntry fileEntry in imageListViewSelectItems)
            {
                List<FileEntryAttribute> fileEntryAttributeDateVersions =
                    databaseAndCacheMetadataExiftool.ListFileEntryAttributesCache(MetadataBrokerType.ExifTool, 
                    fileEntry.FileFullPath, File.GetLastWriteTime(fileEntry.FileFullPath));
                lazyLoadingAllExiftoolVersionOfMediaFile.AddRange(fileEntryAttributeDateVersions);

                FileStatus fileStaus = FileHandler.GetFileStatus(fileEntry.FileFullPath,
                    exiftoolProcessStatus: ExiftoolProcessStatus.InExiftoolReadQueue);

                FileHandler.RemoveOfflineFileTouched(fileEntry.FileFullPath);
                FileHandler.RemoveOfflineFileTouchedFailed(fileEntry.FileFullPath);

                ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStaus);
            }

            AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(lazyLoadingAllExiftoolVersionOfMediaFile);
            AddQueueLazyLoadningMediaThumbnailLock(lazyLoadingAllExiftoolVersionOfMediaFile);
            AddQueueLazyLoadingMapNomnatatimLock(lazyLoadingAllExiftoolVersionOfMediaFile);
            StartThreads();
        }
        #endregion 

        #region DataGridView - Populate Selected Files - OnActiveDataGridView - Thread
        private void DataGridView_Populate_SelectedItemsThread(HashSet<FileEntry> imageListViewSelectItems)
        {
            LazyLoadingDataGridViewProgressUpdateStatus(GetCircleProgressCount(true, 5));

            Thread threadPopulateDataGridView = new Thread(() => {
                DataGridView_Populate_SelectedItemsInvoke(imageListViewSelectItems);
            });

            threadPopulateDataGridView.Start();
        }
        #endregion

        #region DataGridView - GetCircleProgressCount
        private int GetCircleProgressCount(bool showProgressCircle, int populateProgress)
        {
            if (!showProgressCircle) return 0; //imageListViewSelectItems.Count
            //If Lazy Loading queue is 0, means that Data is mising. This occure when was not able to read Exiftool data due e.g. locked file.
            int queueCount;
            if (CommonQueueLazyLoadingAllSourcesAllMetadataAndThumbnailCountLock() == 0) queueCount = 0;
            else queueCount = GetDataGridView_ColumnsEntriesInReadQueues_Count() + populateProgress;

            return queueCount + populateProgress;
        }
        #endregion

        #region DataGridView - CleanAll
        private void DataGridView_CleanAll()
        {
            DataGridViewHandler.SetIsAgregated(dataGridViewTagsAndKeywords, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewPeople, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewMap, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewDate, false);

            DataGridViewHandler.SetIsAgregated(dataGridViewExiftool, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewExiftoolWarning, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewProperties, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewRename, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewConvertAndMerge, false);

        }
        #endregion

        #region DataGridView - Populate - MapLocation
        private void DataGridView_Populate_MapLocation(FileEntryAttribute fileEntryAttribute)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action<FileEntryAttribute>(DataGridView_Populate_MapLocation), fileEntryAttribute);
                return;
            }
            DataGridView dataGridView = dataGridViewMap;

            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;
            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;

            int columnIndex = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridView, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompare);
            if (columnIndex != -1)
            {
                LocationNames.LocationCoordinate locationCoordinate = DataGridViewHandlerMap.GetUserInputLocationCoordinate(dataGridView, columnIndex, fileEntryAttribute);
                DataGridViewHandlerMap.PopulateGrivViewMapNomnatatim(dataGridView, columnIndex, locationCoordinate, false);
            }
        }
        #endregion

        #region DataGridView - Populate Selected Files - OnActiveDataGridView - Invoke 
        /// <summary>
        /// Populate Active DataGridView with Seleted Files from ImageListView
        /// PS. When selected new files, all DataGridViews are maked as dirty.
        /// </summary>
        /// <param name="imageListViewSelectItems"></param>
        private void DataGridView_Populate_SelectedItemsInvoke(HashSet<FileEntry> imageListViewSelectItems)
        {
            
            if (this.InvokeRequired)
            {
                LazyLoadingDataGridViewProgressUpdateStatus(GetCircleProgressCount(true, 4));
                BeginInvoke(new Action<HashSet<FileEntry>>(DataGridView_Populate_SelectedItemsInvoke), imageListViewSelectItems);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;

            LazyLoadingDataGridViewProgressUpdateStatus(GetCircleProgressCount(true, 3));

            lock (GlobalData.populateSelectedLock)
            {
                using (new WaitCursor())
                {
                    DataGridView dataGridView = GetActiveTabDataGridView();

                    #region Layout - Ribbon / Favorite / Equal / Size
                    kryptonRibbonGroupButtonDataGridViewRowsFavorite.Checked = DataGridViewHandler.ShowFavouriteColumns(dataGridView);
                    kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Checked = DataGridViewHandler.HideEqualColumns(dataGridView);

                    DataGridViewSize dataGridViewSize;
                    ShowWhatColumns showWhatColumnsForTab;
                    bool showProgressCircle = true;
                    bool isSizeEnabled = ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true).Count > 0;
                    bool isColumnsEnabled = isSizeEnabled;
                    switch (GetActiveTabTag())
                    {
                        case LinkTabAndDataGridViewNameTags:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameMap:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeMap;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNamePeople:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizePeoples;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameDates:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeDates;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameExiftool:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameProperties:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeProperties;
                            showWhatColumnsForTab = ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns;
                            //isSizeEnabled = false;
                            isColumnsEnabled = false;
                            break;
                        case LinkTabAndDataGridViewNameRename:
                            dataGridViewSize = ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize);
                            showWhatColumnsForTab = ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns;
                            showProgressCircle = false;
                            //isSizeEnabled = false;
                            isColumnsEnabled = false;
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            dataGridViewSize = ((DataGridViewSize)Properties.Settings.Default.CellSizeConvertAndMerge | DataGridViewSize.RenameConvertAndMergeSize);
                            showWhatColumnsForTab = ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns;
                            showProgressCircle = false;
                            //isSizeEnabled = false;
                            isColumnsEnabled = false;
                            break;
                        default: throw new NotImplementedException();
                    }
                    SetRibbonDataGridViewSizeBottons(dataGridViewSize, isSizeEnabled);
                    SetRibbonDataGridViewShowWhatColumns(showWhatColumns, isColumnsEnabled);
                    #endregion

                    if (dataGridView == null || DataGridViewHandler.GetIsAgregated(dataGridView))
                    {
                        LazyLoadingDataGridViewProgressUpdateStatus(GetCircleProgressCount(true, 0));
                        return;
                    }

                    LazyLoadingDataGridViewProgressUpdateStatus(GetCircleProgressCount(true, 2));
                    List<FileEntryAttribute> lazyLoading;
                    DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, true);

                   

                    #region PopulateSelectedFiles
                    switch (GetActiveTabTag())
                    {
                        case LinkTabAndDataGridViewNameTags:
                            InitDetailViewTagsAndKeywords();
                            DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerTagsAndKeywords.AutoKeywordConvertions = autoKeywordConvertions;
                            DataGridViewHandlerTagsAndKeywords.HasBeenInitialized = true;
                            DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadFromDatabaseThenSourceAllVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNameMap:
                            //splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
                            DataGridViewHandlerMap.TimeZoneShift = GetTimeZoneShift();
                            DataGridViewHandlerMap.AccepedIntervalSecound = GetAccepedIntervalSecound();
                            DataGridViewHandlerMap.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                            DataGridViewHandlerMap.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerMap.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerMap.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerMap.DatabaseGoogleLocationHistory = databaseGoogleLocationHistory;
                            DataGridViewHandlerMap.AutoKeywordConvertions = autoKeywordConvertions;
                            DataGridViewHandlerMap.DatabaseAndCacheLocationAddress = databaseLocationAddress;
                            DataGridViewHandlerMap.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                            DataGridViewHandlerMap.HasBeenInitialized = true;
                            DataGridViewHandlerMap.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadFromDatabaseThenSourceAllVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNamePeople:
                            DataGridViewHandlerPeople.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                            DataGridViewHandlerPeople.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerPeople.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerPeople.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerPeople.SuggestRegionNameNearByDays = Properties.Settings.Default.SuggestRegionNameNearbyDays;
                            DataGridViewHandlerPeople.SuggestRegionNameNearByTopMostCount = Properties.Settings.Default.SuggestRegionNameNearByCount;
                            DataGridViewHandlerPeople.RenameDateFormats = Properties.Settings.Default.RenameDateFormats;
                            DataGridViewHandlerPeople.HasBeenInitialized = true;
                            DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            
                            PopulatePeopleToolStripMenuItems(null,
                                Properties.Settings.Default.SuggestRegionNameNearbyDays,
                                //Properties.Settings.Default.SuggestRegionNameNearByCount,
                                Properties.Settings.Default.SuggestRegionNameNearByContextMenuCount,
                                Properties.Settings.Default.SuggestRegionNameMostUsedContextMenuCount,
                                Properties.Settings.Default.ApplicationSizeOfRegionNamesGroup,
                                Properties.Settings.Default.RenameDateFormats);
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadFromDatabaseThenSourceAllVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNameDates:
                            DataGridViewHandlerDate.DatabaseExiftoolData = databaseExiftoolData;
                            DataGridViewHandlerDate.DataGridViewMap = dataGridViewMap;
                            DataGridViewHandlerDate.DataGridViewMapHeaderMedia = DataGridViewHandlerMap.headerMedia;
                            DataGridViewHandlerDate.DataGridViewMapTagCoordinates = DataGridViewHandlerMap.tagMediaCoordinates;
                            DataGridViewHandlerDate.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                            DataGridViewHandlerDate.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerDate.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerDate.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerDate.HasBeenInitialized = true;
                            DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadFromDatabaseThenSourceAllVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNameExiftool:
                            DataGridViewHandlerExiftool.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                            DataGridViewHandlerExiftool.DatabaseExiftoolData = databaseExiftoolData;
                            DataGridViewHandlerExiftool.exiftoolReader = exiftoolReader;
                            DataGridViewHandlerExiftool.HasBeenInitialized = true;
                            lazyLoading = DataGridViewHandlerExiftool.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(lazyLoading);
                            AddQueueLazyLoadningMediaThumbnailLock(lazyLoading);
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            DataGridViewHandlerExiftoolWarnings.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                            DataGridViewHandlerExiftoolWarnings.DatabaseExiftoolWarning = databaseExiftoolWarning;
                            DataGridViewHandlerExiftoolWarnings.exiftoolReader = exiftoolReader;
                            DataGridViewHandlerExiftoolWarnings.HasBeenInitialized = true;
                            lazyLoading = DataGridViewHandlerExiftoolWarnings.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(lazyLoading);
                            AddQueueLazyLoadningMediaThumbnailLock(lazyLoading);
                            break;
                        case LinkTabAndDataGridViewNameProperties:
                            DataGridViewHandlerProperties.WindowsPropertyReader = new WindowsPropertyReader();
                            DataGridViewHandlerProperties.HasBeenInitialized = true;
                            DataGridViewHandlerProperties.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            DataGridView_Populate_ExtrasAsDropdownAndColumnSizesInvoke();
                            break;
                        case LinkTabAndDataGridViewNameRename:
                            DataGridViewHandlerRename.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                            DataGridViewHandlerRename.ShowFullPath = Properties.Settings.Default.RenameShowFullPath;
                            DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerRename.FilesCutCopyPasteDrag = filesCutCopyPasteDrag;
                            checkBoxRenameShowFullPath.Checked = DataGridViewHandlerRename.ShowFullPath;
                            DataGridViewHandlerRename.HasBeenInitialized = true;
                            DataGridViewHandlerRename.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab, DataGridViewHandlerRename.ShowFullPath);
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadFromDatabaseThenSourceAllVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            DataGridViewHandlerConvertAndMerge.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            DataGridViewHandlerConvertAndMerge.RenameVaribale = Properties.Settings.Default.RenameVariable;
                            DataGridViewHandlerConvertAndMerge.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerConvertAndMerge.FilesCutCopyPasteDrag = filesCutCopyPasteDrag;
                            DataGridViewHandlerConvertAndMerge.HasBeenInitialized = true;
                            DataGridViewHandlerConvertAndMerge.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            break;
                        default: throw new NotImplementedException();

                    }
                    #endregion

                    DataGridViewHandler.ResumeLayoutDelayed(dataGridView);
                    
                    LazyLoadingDataGridViewProgressUpdateStatus(GetCircleProgressCount(showProgressCircle, 0));
                } //Cursor
                
                if (imageListViewSelectItems.Count == 0) LazyLoadingDataGridViewProgressUpdateStatus(-1);
            }
            StartThreads();
        }
        #endregion

        #region DataGridView - SaveBeforeContinue
        private DialogResult SaveBeforeContinue(bool canCancel)
        {
            DialogResult dialogResult = DialogResult.No;
            try
            {
                if (IsAnyDataUnsaved())
                {

                    dialogResult = KryptonMessageBox.Show(
                        "Do you want to save before continue?\r\n" +
                        "Yes - Save without AutoCorrect and continue\r\n" +
                        "No - Don't save and continue without save." +
                        (canCancel ? "\r\nCancel - Cancel the opeation and continue where you left." : ""),
                        "Warning, unsaved data! Save before continue?",
                        (canCancel ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.YesNo), MessageBoxIcon.Warning, showCtrlCopy: true);

                    if (dialogResult == DialogResult.Yes)
                    {
                        //ActionSave(false);
                        SaveDataGridViewMetadata(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return dialogResult;
        }
        #endregion

        #region DataGridView - IsAnyDataUnsaved
        private bool IsAnyDataUnsaved()
        {
            if (GlobalData.IsApplicationClosing) return false;
            bool isAnyDataUnsaved = false;

            try
            {
                int listOfUpdatesCount = 0;

                CollectMetadataFromAllDataGridViewData(out List <Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView, false);
                //Find what columns are updated / changed by user
                List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
                listOfUpdatesCount = listOfUpdates.Count;

                isAnyDataUnsaved = (listOfUpdatesCount > 0);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return isAnyDataUnsaved;
        }
        #endregion

        #region DataGridView - IsDataGridViewColumnDirty
        private bool IsDataGridViewColumnDirty(DataGridView dataGridView, int columnIndex, out string differences)
        {
            if (columnIndex == -1)
            {
                differences = "Column not found";
                return false;
            }
            differences = "";

            int listOfUpdatesCount = 0;
            try
            {
                List<Metadata> metadataListOriginalExiftool = new List<Metadata>();
                List<Metadata> metadataListFromDataGridView = new List<Metadata>();

                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                if (dataGridViewGenericColumn.IsPopulated)
                {
                    if (dataGridViewGenericColumn.Metadata != null) //throw new Exception("Missing needed metadata"); //This should not happen. Means it's not aggregated 
                    {
                        if (!FileEntryVersionHandler.IsReadOnlyType(dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion)) //Only check columns User can edit
                        {
                            Metadata metadataFromDataGridView = new Metadata(dataGridViewGenericColumn.Metadata);
                            CollectedMetadataFromAllDataGridView(dataGridViewGenericColumn.FileEntryAttribute, ref metadataFromDataGridView);
                            metadataListOriginalExiftool.Add(new Metadata(dataGridViewGenericColumn.Metadata));
                            metadataListFromDataGridView.Add(new Metadata(metadataFromDataGridView));
                        }
                        else return false; //Was not a column a user can edit, can not be dirty
                    }
                    else
                    {
                        return false; 
                    }
                }
                else
                {
                    return false;
                }
            

                //Find what columns are updated / changed by user
                List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
                differences = Metadata.GetErrors(metadataListFromDataGridView[0], metadataListOriginalExiftool[0], true);
                listOfUpdatesCount = listOfUpdates.Count;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return (listOfUpdatesCount > 0);
        }
        #endregion

        #region DataGridView - UpdatedDirtyFlags
        private void DataGridView_UpdatedDirtyFlags(DataGridView dataGridView)
        {
            try
            {
                if (DataGridViewHandler.GetIsAgregated(dataGridView))
                {
                    for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
                    {
                        DataGridViewHandler.SetColumnDirtyFlag(dataGridView, columnIndex, IsDataGridViewColumnDirty(dataGridView, columnIndex, out string differences), differences);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - CollectMetadataFromAllDataGridViewData - FileEntry
        private void CollectedMetadataFromAllDataGridView(FileEntryAttribute fileEntryAttribute, ref Metadata metadataFromDataGridView)
        {
            try
            {
                if (GlobalData.IsAgregatedTags) DataGridViewHandlerTagsAndKeywords.GetUserInputChanges(ref dataGridViewTagsAndKeywords, metadataFromDataGridView, fileEntryAttribute);
                if (GlobalData.IsAgregatedMap) DataGridViewHandlerMap.GetUserInputChanges(ref dataGridViewMap, metadataFromDataGridView, fileEntryAttribute);
                if (GlobalData.IsAgregatedPeople) DataGridViewHandlerPeople.GetUserInputChanges(ref dataGridViewPeople, metadataFromDataGridView, fileEntryAttribute);
                if (GlobalData.IsAgregatedDate) DataGridViewHandlerDate.GetUserInputChanges(ref dataGridViewDate, metadataFromDataGridView, fileEntryAttribute);
                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - CollectMetadataFromAllDataGridViewData - All
        private void CollectMetadataFromAllDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView, bool clearDirtyFlagAndUpdatedMetadata)
        {
            metadataListOriginalExiftool = new List<Metadata>();
            metadataListFromDataGridView = new List<Metadata>();
            try
            {
                DataGridView dataGridView = GetAnyAgregatedDataGridView();
                List<DataGridViewGenericColumn> dataGridViewGenericColumnList = DataGridViewHandler.GetColumnsDataGridViewGenericColumnCurrentOrAutoCorrect(dataGridView, true);
                foreach (DataGridViewGenericColumn dataGridViewGenericColumn in dataGridViewGenericColumnList)
                {
                    if (dataGridViewGenericColumn.IsPopulated)
                    {
                        if (dataGridViewGenericColumn.Metadata == null)
                        {
                            throw new Exception("Missing needed metadata"); //This should not happen. Means it's nt aggregated 
                        }

                        Metadata metadataFromDataGridView = new Metadata(dataGridViewGenericColumn.Metadata);

                        CollectedMetadataFromAllDataGridView(dataGridViewGenericColumn.FileEntryAttribute, ref metadataFromDataGridView);

                        metadataListOriginalExiftool.Add(new Metadata(dataGridViewGenericColumn.Metadata));
                        metadataListFromDataGridView.Add(new Metadata(metadataFromDataGridView));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - MakeEqualBetweenMetadataAndDataGridViewContent - FileEntry
        private void DataGridViewSetMetadataOnAllDataGridView(Metadata metadataFixedAndCorrected)
        {
            try
            {
                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(metadataFixedAndCorrected.FileEntry, FileEntryVersion.AutoCorrect);

                int debugColumn = -1;
                int columnIndexTagsAndKeywords = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridViewConvertAndMerge, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompareTagsAndKeywords);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewTagsAndKeywords, columnIndexTagsAndKeywords))
                    DataGridViewHandler.SetColumnHeaderMetadata(dataGridViewTagsAndKeywords, metadataFixedAndCorrected, columnIndexTagsAndKeywords);

                if (columnIndexTagsAndKeywords != -1) debugColumn = columnIndexTagsAndKeywords;
                if (debugColumn != -1 && columnIndexTagsAndKeywords != -1 && debugColumn != columnIndexTagsAndKeywords)
                {
                    //DEBUG
                    Logger.Warn("Column updated between user action and thread");
                }

                int columnIndexPeople = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridViewPeople, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionComparePeople);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewPeople, columnIndexPeople))
                    DataGridViewHandler.SetColumnHeaderMetadata(dataGridViewPeople, metadataFixedAndCorrected, columnIndexPeople);
                if (columnIndexPeople != -1) debugColumn = columnIndexPeople;
                if (debugColumn != -1 && columnIndexPeople != -1 && debugColumn != columnIndexPeople)
                {
                    //DEBUG
                    Logger.Warn("Column updated between user action and thread");
                }

                int columnIndexMap = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridViewMap, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompareMap);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewMap, columnIndexMap))
                    DataGridViewHandler.SetColumnHeaderMetadata(dataGridViewMap, metadataFixedAndCorrected, columnIndexMap);
                if (columnIndexMap != -1) debugColumn = columnIndexMap;
                if (debugColumn != -1 && columnIndexMap != -1 && debugColumn != columnIndexMap)
                {
                    //DEBUG
                    Logger.Warn("Column updated between user action and thread");
                }

                int columnIndexDate = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridViewDate, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompareDate);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewDate, columnIndexDate))
                    DataGridViewHandler.SetColumnHeaderMetadata(dataGridViewDate, metadataFixedAndCorrected, columnIndexDate);
                if (columnIndexDate != -1) debugColumn = columnIndexDate;
                if (debugColumn != -1 && columnIndexDate != -1 && debugColumn != columnIndexDate)
                {
                    //DEBUG 
                    Logger.Warn("Column updated between user action and thread");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - MakeEqualBetweenMetadataAndDataGridViewContent - FileEntry
        private void DataGridViewSetDirtyFlagAfterSave(Metadata metadataFixedAndCorrected, bool isDirty)
        {
            try
            {
                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(metadataFixedAndCorrected.FileEntry, FileEntryVersion.AutoCorrect);
                int columnIndex = DataGridViewHandler.GetColumnIndexWhenAddColumn(GetAnyAgregatedDataGridView(), fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompare);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewTagsAndKeywords, columnIndex))
                    DataGridViewHandler.SetColumnDirtyFlag(dataGridViewTagsAndKeywords, columnIndex, isDirty);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewPeople, columnIndex))
                    DataGridViewHandler.SetColumnDirtyFlag(dataGridViewPeople, columnIndex, isDirty);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewMap, columnIndex))
                    DataGridViewHandler.SetColumnDirtyFlag(dataGridViewMap, columnIndex, isDirty);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewDate, columnIndex))
                    DataGridViewHandler.SetColumnDirtyFlag(dataGridViewDate, columnIndex, isDirty);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - GetSelectedFilesFromActiveDataGridView
        private HashSet<FileEntry> DataGridView_GetSelectedFilesFromActive()
        {
            HashSet<FileEntry> files = new HashSet<FileEntry>();
            try
            {
                DataGridView dataGridView;
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    case KryptonPages.kryptonPageMediaFiles:
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                    case KryptonPages.kryptonPageToolboxPeople:
                    case KryptonPages.kryptonPageToolboxMap:
                    case KryptonPages.kryptonPageToolboxDates:
                    case KryptonPages.kryptonPageToolboxExiftool:
                    case KryptonPages.kryptonPageToolboxWarnings:
                    case KryptonPages.kryptonPageToolboxProperties:
                        dataGridView = GetActiveTabDataGridView();
                        if (dataGridView != null)
                        {
                            foreach (int columnIndex in DataGridViewHandler.GetColumnSelected(GetActiveTabDataGridView()))
                            {
                                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(GetActiveTabDataGridView(), columnIndex);
                                if (dataGridViewGenericColumn != null && !files.Contains(dataGridViewGenericColumn.FileEntryAttribute.FileEntry)) files.Add(dataGridViewGenericColumn.FileEntryAttribute.FileEntry);
                            }
                        }
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        dataGridView = GetActiveTabDataGridView();
                        if (dataGridView != null)
                        {
                            foreach (int rowIndex in DataGridViewHandler.GetRowSelected(GetActiveTabDataGridView()))
                            {
                                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(GetActiveTabDataGridView(), rowIndex);
                                FileEntry fileEntry = dataGridViewGenericRow.FileEntryAttribute.FileEntry;
                                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader && !files.Contains(fileEntry)) files.Add(fileEntry);
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return files;
        }
        #endregion

    }
}
