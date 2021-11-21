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

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region Workspace - FindWorkspaceCell
        private Krypton.Workspace.KryptonWorkspaceCell FindWorkspaceCell(Krypton.Workspace.KryptonWorkspace kryptonWorkspace, string name)
        {
            Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell = kryptonWorkspace.FirstVisibleCell();
            while (kryptonWorkspaceCell != null)
            {
                if (kryptonWorkspaceCell.Name == name)
                {
                    return kryptonWorkspaceCell;
                }
                kryptonWorkspaceCell = kryptonWorkspace.NextVisibleCell(kryptonWorkspaceCell);
            }
            return null;
        }
        #endregion

        #region Workspace - ActionMaximumWorkspaceCell
        private void ActionMaximumWorkspaceCell(Krypton.Workspace.KryptonWorkspace kryptonWorkspace, Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell)
        {
            kryptonWorkspace.MaximizedCell = kryptonWorkspaceCell;
        }
        #endregion

        #region WorkspaceCellToolboxTags - MaximizeRestore

        #region WorkspaceCellToolboxTags - MaximizeOrRestore()
        private void WorkspaceCellToolboxTagsMaximizeOrRestore()
        {
            switch (GetActiveTabTag())
            {
                case LinkTabAndDataGridViewNameTags:
                    Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell = FindWorkspaceCell(kryptonWorkspaceToolboxTags, Properties.Settings.Default.WorkspaceToolboxTagsMaximizedCell);
                    ActionMaximumWorkspaceCell(kryptonWorkspaceToolboxTags, kryptonWorkspaceCell);
                    break;
                case LinkTabAndDataGridViewNameMap:
                case LinkTabAndDataGridViewNamePeople:
                case LinkTabAndDataGridViewNameDates:
                case LinkTabAndDataGridViewNameExiftool:
                case LinkTabAndDataGridViewNameWarnings:
                case LinkTabAndDataGridViewNameProperties:
                case LinkTabAndDataGridViewNameRename:
                case LinkTabAndDataGridViewNameConvertAndMerge:
                    break;
                default: throw new NotImplementedException();
            }
        }
        #endregion 

        #region WorkspaceCellToolboxTags - MaximizeRestore - Click
        private void ActionMaximizeRestoreWorkspaceCellToolboxTags()
        {
            Properties.Settings.Default.WorkspaceToolboxTagsMaximizedCell = kryptonWorkspaceToolboxTags?.MaximizedCell?.Name ?? "";
        }
        #endregion

        #region WorkspacToolboxTags - MaximizeRestore - Click
        private void kryptonWorkspaceCellToolboxTagsDetails_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            ActionMaximizeRestoreWorkspaceCellToolboxTags();
        }

        private void kryptonWorkspaceCellToolboxTagsKeywords_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            ActionMaximizeRestoreWorkspaceCellToolboxTags();
        }
        #endregion

        #endregion

        #region WorkspaceMain - MaximizeRestore

        #region WorkspaceMain - MaximizeWorkspaceMainCell
        private void MaximizeOrRestoreWorkspaceMainCellAndChilds()
        {
            Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell = FindWorkspaceCell(kryptonWorkspaceMain, Properties.Settings.Default.WorkspaceMainMaximizedCell);
            ActionMaximumWorkspaceCell(kryptonWorkspaceMain, kryptonWorkspaceCell);
            WorkspaceCellToolboxTagsMaximizeOrRestore();
        }
        #endregion

        #region WorkspaceMain - ActionMaximizeRestoreWorkspaceMain
        private void ActionMaximizeRestoreWorkspaceMain()
        {
            Properties.Settings.Default.WorkspaceMainMaximizedCell = kryptonWorkspaceMain?.MaximizedCell?.Name ?? "";
            WorkspaceCellToolboxTagsMaximizeOrRestore(); //Not restore this but childs
        }
        #endregion

        #region WorkspaceMain - MaximizeRestore - Click
        private void kryptonWorkspaceCellFolderSearchFilter_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            SetNavigatorModeSearch(NavigatorMode.OutlookFull);
            ActionMaximizeRestoreWorkspaceMain();
        }

        private void kryptonWorkspaceCellMediaFiles_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            ActionMaximizeRestoreWorkspaceMain();
        }

        private void kryptonWorkspaceCellToolbox_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            ActionMaximizeRestoreWorkspaceMain();
        }
        #endregion

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


        #region -- SelectedPageChanged --
        private void kryptonWorkspaceCellToolbox_SelectedPageChanged(object sender, EventArgs e)
        {
            if (isFormLoading) return;
            try
            {
                ActionMaximumWorkspaceCell(kryptonWorkspaceMain, FindWorkspaceCell(kryptonWorkspaceMain, Properties.Settings.Default.WorkspaceMainMaximizedCell)); //Need to be in front of ActionMaximumWorkspaceCell(kryptonWorkspaceToolboxTags, kryptonWorkspaceToolboxTagPrevious);

                switch (GetActiveTabTag())
                {
                    case LinkTabAndDataGridViewNameTags:
                        ActionMaximumWorkspaceCell(kryptonWorkspaceToolboxTags, FindWorkspaceCell(kryptonWorkspaceToolboxTags, Properties.Settings.Default.WorkspaceToolboxTagsMaximizedCell));
                        break;
                    case LinkTabAndDataGridViewNameMap:
                    case LinkTabAndDataGridViewNamePeople:
                    case LinkTabAndDataGridViewNameDates:
                    case LinkTabAndDataGridViewNameExiftool:
                    case LinkTabAndDataGridViewNameWarnings:
                    case LinkTabAndDataGridViewNameProperties:
                    case LinkTabAndDataGridViewNameRename:
                    case LinkTabAndDataGridViewNameConvertAndMerge:
                        break;
                    default: throw new NotImplementedException();
                }
                PopulateDataGridViewForSelectedItemsThread(GetSelectedFileEntriesImageListView());
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to to populate data grid view");
                Logger.Error(ex);
            }
        }
        #endregion

        #region DataGridView - Populate DataGridView, OpenWith...
        private void FilesSelectedOrNoneSelected()
        {
            if (GlobalData.IsPopulatingAnything()) return; //E.g. Populate FolderSelect
            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;

            using (new WaitCursor())
            {
                GlobalData.IsPopulatingImageListView = true;
                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                HashSet<FileEntry> fileEntries = GetSelectedFileEntriesImageListView();
                PopulateDataGridViewForSelectedItemsThread(fileEntries);
                PopulateImageListViewOpenWithToolStripThread(fileEntries, imageListView1.Items);
                UpdateRibbonsWhenWorkspaceChanged();


                GlobalData.IsPopulatingImageListView = false;
            }
        }
        #endregion

        #region DataGridView - Populate File - For FileEntryAttribute missing Tag - Invoke
        private void PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute>(PopulateImageListVieAndDataGridViewForFileEntryAttributeInvoke), fileEntryAttribute);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                string tag = GetActiveTabTag();
                if (!string.IsNullOrWhiteSpace(tag) && IsActiveDataGridViewAgregated(tag))
                {
                    DataGridView dataGridView = GetDataGridViewForTag(tag);
                    if (dataGridView != null) PopulateDataGrivViewForFileEntryAttributeAndTag(dataGridView, fileEntryAttribute, tag);
                }
            
                ImageListViewItem foundItem = FindItemInImageListView(imageListView1.Items, fileEntryAttribute.FileFullPath);
                if (foundItem != null)
                {
                    if (foundItem.IsPropertyRequested()) foundItem.Update(); //imageListView1.Items.Remove(foundItem);
                }
            } catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region DataGridView - Populate File - For FileEntryAttribute and Tag
        private void PopulateDataGrivViewForFileEntryAttributeAndTag(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, string tag)
        {
            lock (GlobalData.populateSelectedLock)
            {
                #region isFileInDataGridView
                bool isFilSelectedInImageListView = DoesExistInSelectedFileEntriesImageListView(fileEntryAttribute.FileFullPath);
                #endregion

                DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, isFilSelectedInImageListView); //Will not suspend when Column Don't exist, but counter will increase
                if (isFilSelectedInImageListView)
                {
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
                    switch (tag)
                    {
                        case LinkTabAndDataGridViewNameTags:
                            DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            //if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNamePeople:
                            DataGridViewHandlerPeople.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNameMap:
                            DataGridViewHandlerMap.PopulateFile(dataGridView, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            break;
                        case LinkTabAndDataGridViewNameDates:
                            DataGridViewHandlerDate.PopulateFile(dataGridView, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            //if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
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
                            DataGridViewHandlerRename.PopulateFile(dataGridView, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath);
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridView, fileEntryAttribute);
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

                    int queueCount = GetDataGridViewWatingToBePopulatedCount();
                    LazyLoadingDataGridViewProgressUpdateStatus(queueCount); //Update progressbar when File In DataGridView
                    if (queueCount == 0) 
                        PopulateDataGridViewForSelectedItemsExtrasDelayed();
                }

                DataGridViewHandler.ResumeLayoutDelayed(dataGridView); //Will resume when counter reach 0
            }
        }
        #endregion

        #region DataGridView - LazyLoad - Populate File - For Selected Files 
        private void LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(HashSet<FileEntry> imageListViewSelectItems)
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

        #region DataGridView - PopulateDataGridViewForSelectedItemsExtrasInvoke (Populate DataGridView Extras)

        private void PopulateDataGridViewForSelectedItemsExtrasDelayed()
        {
            PopulateDataGridViewForSelectedItemsExtrasInvoke();
        }

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

        #region DataGridView - Populate Selected Files - OnActiveDataGridView - Thread
        private void PopulateDataGridViewForSelectedItemsThread(HashSet<FileEntry> imageListViewSelectItems)
        {
            LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 5);
            Thread threadPopulateDataGridView = new Thread(() => {
                PopulateDataGridViewForSelectedItemsInvoke(imageListViewSelectItems);
            });

            threadPopulateDataGridView.Start();
        }
        #endregion

        #region DataGridView - Populate Selected Files - OnActiveDataGridView - Invoke -> PopulateDataGridViewForSelectedItemsExtrasDelayed();
        /// <summary>
        /// Populate Active DataGridView with Seleted Files from ImageListView
        /// PS. When selected new files, all DataGridViews are maked as dirty.
        /// </summary>
        /// <param name="imageListViewSelectItems"></param>
        private void PopulateDataGridViewForSelectedItemsInvoke(HashSet<FileEntry> imageListViewSelectItems)
        {
            
            if (this.InvokeRequired)
            {
                LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 4);
                BeginInvoke(new Action<HashSet<FileEntry>>(PopulateDataGridViewForSelectedItemsInvoke), imageListViewSelectItems);
                return;
            }

            LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 3);
            if (GlobalData.IsApplicationClosing) return;

            lock (GlobalData.populateSelectedLock)
            {
                using (new WaitCursor())
                {
                    DataGridView dataGridView = GetActiveTabDataGridView();

                    #region RibbonGroupButtonDataGridViewRows Favorite and Equal
                    kryptonRibbonGroupButtonDataGridViewRowsFavorite.Checked = DataGridViewHandler.ShowFavouriteColumns(dataGridView);
                    kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Checked = DataGridViewHandler.HideEqualColumns(dataGridView);

                    DataGridViewSize dataGridViewSize;
                    ShowWhatColumns showWhatColumnsForTab;
                    bool isSizeEnabled = GetSelectedFilesImageListView().Count > 0;
                    bool isColumnsEnabled = GetSelectedFilesImageListView().Count > 0;
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
                            //isSizeEnabled = false;
                            isColumnsEnabled = false;
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            dataGridViewSize = ((DataGridViewSize)Properties.Settings.Default.CellSizeConvertAndMerge | DataGridViewSize.RenameConvertAndMergeSize);
                            showWhatColumnsForTab = ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns;
                            //isSizeEnabled = false;
                            isColumnsEnabled = false;
                            break;
                        default: throw new NotImplementedException();
                    }
                    SetRibbonDataGridViewSizeBottons(dataGridViewSize, isSizeEnabled);
                    SetRibbonDataGridViewShowWhatColumns(showWhatColumns, isColumnsEnabled);
                    #endregion

                    if (dataGridView == null || DataGridViewHandler.GetIsAgregated(dataGridView)) return;

                    LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 2);
                    List<FileEntryAttribute> lazyLoading;
                    DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, true);

                    switch (GetActiveTabTag())
                    {
                        case LinkTabAndDataGridViewNameTags:
                            ClearDetailViewTagsAndKeywords();
                            DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheThumbnail = databaseAndCacheThumbnail;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                            DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                            DataGridViewHandlerTagsAndKeywords.AutoKeywordConvertions = autoKeywordConvertions;
                            DataGridViewHandlerTagsAndKeywords.HasBeenInitialized = true;
                            DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);
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
                            DataGridViewHandlerPeople.HasBeenInitialized = true;
                            DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
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
                            DataGridViewHandlerDate.HasBeenInitialized = true;
                            DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListViewSelectItems);
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
                            PopulateDataGridViewForSelectedItemsExtrasDelayed();
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
                    DataGridViewHandler.ResumeLayoutDelayed(dataGridView);
                }
                LazyLoadingDataGridViewProgressUpdateStatus(imageListViewSelectItems.Count + 1);
            }
            StartThreads();
        }
        #endregion
    }
}
