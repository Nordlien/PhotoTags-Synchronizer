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
using Krypton.Toolkit;
using Krypton.Navigator;
using System.Diagnostics;
using System.IO;

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

        #region DataGridView - ImageListView - Populate File - For FileEntryAttribute missing Tag - Invoke
        private void DataGridView_ImageListView_Populate_FileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute>(DataGridView_ImageListView_Populate_FileEntryAttributeInvoke), fileEntryAttribute);
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
            
                ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fileEntryAttribute.FileFullPath);
                if (foundItem != null)
                {
                    if (foundItem.IsPropertyRequested()) foundItem.Update();
                }
            } catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region DataGridView - Populate - SelectedExtrasDropdownAndColumnSizesInvoke (Populate DataGridView Extras)
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
                        break;
                    case LinkTabAndDataGridViewNameDates:
                        break;
                    case LinkTabAndDataGridViewNameExiftool:
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, GetDataGridViewWatingToBePopulatedCount());
                        break;
                    case LinkTabAndDataGridViewNameWarnings:
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, GetDataGridViewWatingToBePopulatedCount());
                        break;
                    case LinkTabAndDataGridViewNameProperties:
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, GetDataGridViewWatingToBePopulatedCount());
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

        #region DataGridView - Populate - FileEntryAttribute -> PopulateDataGridViewForSelectedItemsExtrasDelayed();
        private void DataGridView_Populate_FileEntryAttribute(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, string tabTag)
        {
            lock (GlobalData.populateSelectedLock)
            {
                #region isFileInDataGridView
                bool isFilSelectedInImageListView = ImageListViewHandler.DoesExistInSelectedFiles(imageListView1, fileEntryAttribute.FileFullPath);
                #endregion

                if (isFilSelectedInImageListView)
                {
                    DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, isFilSelectedInImageListView); //Will not suspend when Column Don't exist, but counter will increase

                    #region AutoCorrect
                    Metadata metadataAutoCorrect = null;
                    if (isFilSelectedInImageListView && (fileEntryAttribute.FileEntryVersion == FileEntryVersion.AutoCorrect || GlobalData.ListOfAutoCorrectFilesContains(fileEntryAttribute.FileFullPath)))
                    {
                        Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool));
                        Metadata metadataUpdatedFromGrid = (metadataInCache == null ? null : new Metadata(metadataInCache));

                        if (metadataUpdatedFromGrid != null)
                        {
                            AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                            float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                            float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                            int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                            UpdateMetadataFromDataGridView(fileEntryAttribute, ref metadataUpdatedFromGrid);

                            metadataAutoCorrect = autoCorrect.FixAndSave(
                                fileEntryAttribute.FileEntry,
                                metadataUpdatedFromGrid,
                                databaseAndCacheMetadataExiftool,
                                databaseAndCacheMetadataMicrosoftPhotos,
                                databaseAndCacheMetadataWindowsLivePhotoGallery,
                                databaseAndCahceCameraOwner,
                                databaseLocationAddress,
                                databaseGoogleLocationHistory,
                                locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                autoKeywordConvertions,
                                Properties.Settings.Default.RenameDateFormats);
                            AutoCorrectFormVaraibles autoCorrectFormVaraibles = GlobalData.GetAutoCorrectVariablesForFile(fileEntryAttribute.FileFullPath);
                            AutoCorrectFormVaraibles.UpdateMetaData(ref metadataAutoCorrect, autoCorrectFormVaraibles);

                        }
                    }
                    #endregion

                    #region Popuate File
                    switch (tabTag)
                    {
                        case LinkTabAndDataGridViewNameTags:
                            DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            //if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;
                        case LinkTabAndDataGridViewNamePeople:
                            DataGridViewHandlerPeople.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;
                        case LinkTabAndDataGridViewNameMap:
                            DataGridViewHandlerMap.PopulateFile(dataGridView, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;
                        case LinkTabAndDataGridViewNameDates:
                            DataGridViewHandlerDate.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;

                        case LinkTabAndDataGridViewNameExiftool:
                            DataGridViewHandlerExiftool.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;
                        case LinkTabAndDataGridViewNameProperties:
                            DataGridViewHandlerProperties.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;
                        case LinkTabAndDataGridViewNameRename:
                            DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridView, fileEntryAttribute);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    #endregion

                    #region PopulateTreeViewFolderFilter
                    if (!IsPopulateTreeViewFolderFilterThreadRunning)
                    {
                        PopulateTreeViewFolderFilterAdd(new FileEntryBroker(fileEntryAttribute, MetadataBrokerType.ExifTool));
                        PopulateTreeViewFolderFilterUpdatedTreeViewFilterInvoke();
                    }
                    #endregion

                    #region Check if got thumbnail, if not, push to read queue
                    Image thumbnailImage = databaseAndCacheThumbnail.ReadThumbnailFromCacheOnly(fileEntryAttribute);
                    if (thumbnailImage == null)
                        AddQueueLazyLoadningDataGridViewThumbnailLock(fileEntryAttribute);
                    #endregion

                    int queueCount = GetDataGridViewWatingToBePopulatedCount();
                    LazyLoadingDataGridViewProgressUpdateStatus(queueCount); //Update progressbar when File In DataGridView
                    if (queueCount == 0) DataGridView_Populate_ExtrasAsDropdownAndColumnSizesInvoke();

                    DataGridViewHandler.ResumeLayoutDelayed(dataGridView); //Will resume when counter reach 0
                }
            }
        }
        #endregion

        #region DataGridView - LazyLoad - AfterPopulateSelectedFilesLazyLoadOtherFileVersions 
        private void DataGridView_AfterPopulateSelectedFiles_LazyLoadOtherFileVersions(HashSet<FileEntry> imageListViewSelectItems)
        {
            List<FileEntryAttribute> lazyLoadingAllExiftoolVersionOfMediaFile = new List<FileEntryAttribute>();

            foreach (FileEntry imageListViewItem in imageListViewSelectItems)
            {
                List<FileEntryAttribute> fileEntryAttributeDateVersions = databaseAndCacheMetadataExiftool.ListFileEntryAttributesCache(MetadataBrokerType.ExifTool, imageListViewItem.FileFullPath);
                //When list is 0, then Metadata was not readed from mediafile and needs put back in read queue
                if (fileEntryAttributeDateVersions.Count == 0)
                {
                    AddQueueLazyLoadingDataGridViewMetadataReadToCacheOrUpdateFromSoruce(new FileEntry(imageListViewItem.FileFullPath, imageListViewItem.LastWriteDateTime));
                }
                lazyLoadingAllExiftoolVersionOfMediaFile.AddRange(fileEntryAttributeDateVersions);
            }

            AddQueueLazyLoadningDataGridViewMetadataLock(lazyLoadingAllExiftoolVersionOfMediaFile);
            AddQueueLazyLoadningDataGridViewThumbnailLock(lazyLoadingAllExiftoolVersionOfMediaFile);

            StartThreads();
        }
        #endregion 

        #region DataGridView - Populate Selected Files - OnActiveDataGridView - Thread
        private void DataGridView_Populate_SelectedItemsThread(HashSet<FileEntry> imageListViewSelectItems)
        {
            LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 5);
            Thread threadPopulateDataGridView = new Thread(() => {
                DataGridView_Populate_SelectedItemsInvoke(imageListViewSelectItems);
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
        private void DataGridView_Populate_SelectedItemsInvoke(HashSet<FileEntry> imageListViewSelectItems)
        {
            
            if (this.InvokeRequired)
            {
                LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 4);
                BeginInvoke(new Action<HashSet<FileEntry>>(DataGridView_Populate_SelectedItemsInvoke), imageListViewSelectItems);
                return;
            }

            LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 3);
            if (GlobalData.IsApplicationClosing) return;

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
                        LazyLoadingDataGridViewProgressUpdateStatus(0);
                        return;
                    }

                    LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 2);
                    List<FileEntryAttribute> lazyLoading;
                    DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, true);

                    #region PopulateSelectedFiles
                    switch (GetActiveTabTag())
                    {
                        case LinkTabAndDataGridViewNameTags:
                            InitDetailViewTagsAndKeywords();
                            DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerTagsAndKeywords.AutoKeywordConvertions = autoKeywordConvertions;
                            DataGridViewHandlerTagsAndKeywords.HasBeenInitialized = true;
                            DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadOtherFileVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNameMap:
                            //splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
                            DataGridViewHandlerMap.TimeZoneShift = GetTimeZoneShift();
                            DataGridViewHandlerMap.AccepedIntervalSecound = GetAccepedIntervalSecound();
                            DataGridViewHandlerMap.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerMap.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerMap.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerMap.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerMap.DatabaseGoogleLocationHistory = databaseGoogleLocationHistory;
                            DataGridViewHandlerMap.AutoKeywordConvertions = autoKeywordConvertions;
                            DataGridViewHandlerMap.DatabaseAndCacheLocationAddress = databaseLocationAddress;
                            DataGridViewHandlerMap.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                            DataGridViewHandlerMap.HasBeenInitialized = true;
                            DataGridViewHandlerMap.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadOtherFileVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNamePeople:
                            PopulatePeopleToolStripMenuItems();
                            DataGridViewHandlerPeople.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerPeople.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerPeople.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerPeople.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerPeople.SuggestRegionNameNearbyDays = Properties.Settings.Default.SuggestRegionNameNearbyDays;
                            DataGridViewHandlerPeople.SuggestRegionNameTopMostCount = Properties.Settings.Default.SuggestRegionNameTopMostCount;
                            DataGridViewHandlerPeople.HasBeenInitialized = true;
                            DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadOtherFileVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNameDates:
                            DataGridViewHandlerDate.DatabaseExiftoolData = databaseExiftoolData;
                            DataGridViewHandlerDate.DataGridViewMap = dataGridViewMap;
                            DataGridViewHandlerDate.DataGridViewMapHeaderMedia = DataGridViewHandlerMap.headerMedia;
                            DataGridViewHandlerDate.DataGridViewMapTagCoordinates = DataGridViewHandlerMap.tagMediaCoordinates;
                            DataGridViewHandlerDate.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerDate.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerDate.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerDate.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerDate.HasBeenInitialized = true;
                            DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadOtherFileVersions(imageListViewSelectItems);
                            break;
                        case LinkTabAndDataGridViewNameExiftool:
                            DataGridViewHandlerExiftool.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerExiftool.DatabaseExiftoolData = databaseExiftoolData;
                            DataGridViewHandlerExiftool.exiftoolReader = exiftoolReader;
                            DataGridViewHandlerExiftool.HasBeenInitialized = true;
                            lazyLoading = DataGridViewHandlerExiftool.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            AddQueueLazyLoadningDataGridViewMetadataLock(lazyLoading);
                            AddQueueLazyLoadningDataGridViewThumbnailLock(lazyLoading);
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            DataGridViewHandlerExiftoolWarnings.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerExiftoolWarnings.DatabaseExiftoolWarning = databaseExiftoolWarning;
                            DataGridViewHandlerExiftoolWarnings.exiftoolReader = exiftoolReader;
                            DataGridViewHandlerExiftoolWarnings.HasBeenInitialized = true;
                            lazyLoading = DataGridViewHandlerExiftoolWarnings.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            AddQueueLazyLoadningDataGridViewMetadataLock(lazyLoading);
                            AddQueueLazyLoadningDataGridViewThumbnailLock(lazyLoading);
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
                            DataGridView_AfterPopulateSelectedFiles_LazyLoadOtherFileVersions(imageListViewSelectItems);
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
                    LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 1);
                    if (!showProgressCircle) LazyLoadingDataGridViewProgressUpdateStatus(0);
                } //Cursor
                
                if (imageListViewSelectItems.Count == 0) LazyLoadingDataGridViewProgressUpdateStatus(0);
            }
            StartThreads();
        }
        #endregion
    }
}
