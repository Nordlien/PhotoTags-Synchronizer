using System;
using System.Windows.Forms;
using Krypton.Toolkit;
using Krypton.Navigator;
using Raccoom.Windows.Forms;

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

        #region KryptonWorkspaceCell Click - Media files
        private void kryptonWorkspaceCellMediaFiles_Click(object sender, EventArgs e)
        {
            ((Krypton.Workspace.KryptonWorkspaceCell)sender).Focus();
        }
        #endregion

        #region KryptonWorkspaceCell Click - Keywords and Tags
        private void kryptonWorkspaceCellToolboxTagsDetails_Click(object sender, EventArgs e)
        {
            ((Krypton.Workspace.KryptonWorkspaceCell)sender).Focus();
        }

        private void kryptonWorkspaceCellToolboxTagsKeywords_Click(object sender, EventArgs e)
        {
            ((Krypton.Workspace.KryptonWorkspaceCell)sender).Focus();
        }
        #endregion 

        #region KryptonWorkspaceCell Click - Folder,Search, Filter
        private void kryptonWorkspaceCellFolderSearchFilter_Click(object sender, EventArgs e)
        {
            ((Krypton.Workspace.KryptonWorkspaceCell)sender).Focus();
        }
        #endregion 

        #region ActivePageChanged - PopulateDatabaseFilter / Add TextTitle + *
        private void kryptonWorkspaceMain_ActivePageChanged(object sender, Krypton.Workspace.ActivePageChangedEventArgs e)
        {
            if (e.NewPage == kryptonPageFolderSearchFilterSearch) PopulateDatabaseFilter();
            e.OldPage.Text = e.OldPage.TextTitle = e.OldPage.Text.TrimEnd('*');
            e.NewPage.Text = e.NewPage.TextTitle = e.NewPage.Text.TrimEnd('*') + "*";
        }
        #endregion

        #region Workspace -- Selected DataGrivView tab - Changed --
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
                DataGridView_Populate_SelectedItemsThread(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to to populate data grid view", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                Logger.Error(ex);
            }
        }
        #endregion

        #region kryptonContextMenuGenericBase_Opening
        private void kryptonContextMenuGenericBase_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (sender is Krypton.Toolkit.KryptonContextMenu)
                {
                    if (((Krypton.Toolkit.KryptonContextMenu)sender).Caller is Krypton.Toolkit.KryptonDataGridView)
                    {
                        DataGridView dataGridView = (Krypton.Toolkit.KryptonDataGridView)((Krypton.Toolkit.KryptonContextMenu)sender).Caller;
                        if (dataGridView.Name == nameDataGridViewConvertAndMerge) ActiveKryptonPage = KryptonPages.kryptonPageToolboxConvertAndMerge;
                        if (dataGridView.Name == nameDataGridViewDate) ActiveKryptonPage = KryptonPages.kryptonPageToolboxDates;
                        if (dataGridView.Name == nameDataGridViewExifTool) ActiveKryptonPage = KryptonPages.kryptonPageToolboxExiftool;
                        if (dataGridView.Name == nameDataGridViewExifToolWarning) ActiveKryptonPage = KryptonPages.kryptonPageToolboxWarnings;
                        if (dataGridView.Name == nameDataGridViewMap) ActiveKryptonPage = KryptonPages.kryptonPageToolboxMap;
                        if (dataGridView.Name == nameDataGridViewPeople) ActiveKryptonPage = KryptonPages.kryptonPageToolboxPeople;
                        if (dataGridView.Name == nameDataGridViewProperties) ActiveKryptonPage = KryptonPages.kryptonPageToolboxProperties;
                        if (dataGridView.Name == nameDataGridViewRename) ActiveKryptonPage = KryptonPages.kryptonPageToolboxRename;
                        if (dataGridView.Name == nameDataGridViewTagsAndKeywords) ActiveKryptonPage = KryptonPages.kryptonPageToolboxTags;
                    }
                    else if (((Krypton.Toolkit.KryptonContextMenu)sender).Caller is TreeViewFolderBrowser)
                    {
                        TreeViewFolderBrowser folderTreeView = (TreeViewFolderBrowser)((Krypton.Toolkit.KryptonContextMenu)sender).Caller;
                        if (folderTreeView.Name == nameFolderTreeViewFolder) ActiveKryptonPage = KryptonPages.kryptonPageFolderSearchFilterFolder;
                    }
                    else if (((Krypton.Toolkit.KryptonContextMenu)sender).Caller is Manina.Windows.Forms.ImageListView)
                    {
                        Manina.Windows.Forms.ImageListView imageListView = (Manina.Windows.Forms.ImageListView)((Krypton.Toolkit.KryptonContextMenu)sender).Caller;
                        if (imageListView.Name == nameImageListView) ActiveKryptonPage = KryptonPages.kryptonPageMediaFiles;
                    }
                    else throw new NotImplementedException();

                }

                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(false);
                        ContextMenuGenericFileSystem(false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(false);
                        ContextMenuGenericShowHideRows(false);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(false);
                        ContextMenuGenericMap(false, false);

                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: true, visibleUndoRedo: false, visibleCopyText: true,
                            visibleFind: true, visibleReplace: false,
                            visibleDelete: true, visibleRenameEdit: true, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: true, visibleReadSubfolders: true, visibleOpenBrowserOnLocation: true, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(true);
                        ContextMenuGenericRotate(true);
                        ContextMenuGenericFavorite(false);
                        ContextMenuGenericShowHideRows(false);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, false);
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: false, visibleCutPaste: false, visibleUndoRedo: false, visibleCopyText: false,
                            visibleFind: true, visibleReplace: false,
                            visibleDelete: false, visibleRenameEdit: false, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(false);
                        ContextMenuGenericShowHideRows(false);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(false);
                        ContextMenuGenericMap(false, false);
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(false);
                        ContextMenuGenericFileSystem(false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(false);
                        ContextMenuGenericShowHideRows(false);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(false);
                        ContextMenuGenericMap(false, false);
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: true, visibleUndoRedo: false, visibleCopyText: true,
                            visibleFind: true, visibleReplace: false,
                            visibleDelete: true, visibleRenameEdit: true, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: true, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: true, visibleOpenRunEdit: true);
                        ContextMenuGenericMetadata(true);
                        ContextMenuGenericRotate(true);
                        ContextMenuGenericFavorite(false);
                        ContextMenuGenericShowHideRows(false);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, false);
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                            visibleFind: true, visibleReplace: true,
                            visibleDelete: true, visibleRenameEdit: true, visibleSave: false, visibleFastCopy: true);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(true);
                        ContextMenuGenericShowHideRows(true);
                        ContextMenuGenericTriState(true);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, true);
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(true);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                            visibleFind: true, visibleReplace: true,
                            visibleDelete: true, visibleRenameEdit: true, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(true);
                        ContextMenuGenericShowHideRows(true);
                        ContextMenuGenericTriState(true);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, true);
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                            visibleFind: true, visibleReplace: true,
                            visibleDelete: true, visibleRenameEdit: true, visibleSave: false, visibleFastCopy: true);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(true);
                        ContextMenuGenericShowHideRows(true);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(true, true);
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                            visibleFind: true, visibleReplace: true,
                            visibleDelete: true, visibleRenameEdit: true, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(true);
                        ContextMenuGenericShowHideRows(true);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, true);
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ContextMenuGenericAssignCompositeTag(true);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: false, visibleUndoRedo: true, visibleCopyText: false,
                            visibleFind: true, visibleReplace: false,
                            visibleDelete: false, visibleRenameEdit: false, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(true);
                        ContextMenuGenericShowHideRows(true);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, false);
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ContextMenuGenericAssignCompositeTag(true);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: false, visibleUndoRedo: true, visibleCopyText: false,
                            visibleFind: true, visibleReplace: false,
                            visibleDelete: false, visibleRenameEdit: false, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(true);
                        ContextMenuGenericShowHideRows(true);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, false);
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                            visibleFind: true, visibleReplace: false,
                            visibleDelete: true, visibleRenameEdit: true, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(true);
                        ContextMenuGenericShowHideRows(true);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, false);
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                            visibleFind: true, visibleReplace: false,
                            visibleDelete: true, visibleRenameEdit: true, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(true);
                        ContextMenuGenericShowHideRows(true);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, false);
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ContextMenuGenericAssignCompositeTag(false);
                        ContextMenuGenericRegionNameRename(false);
                        ContextMenuGenericClipboard(
                            visibleCopy: true, visibleCutPaste: false, visibleUndoRedo: false, visibleCopyText: false,
                            visibleFind: true, visibleReplace: false,
                            visibleDelete: false, visibleRenameEdit: false, visibleSave: false, visibleFastCopy: false);
                        ContextMenuGenericFileSystem(
                            visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                        ContextMenuGenericMetadata(false);
                        ContextMenuGenericRotate(false);
                        ContextMenuGenericFavorite(true);
                        ContextMenuGenericShowHideRows(true);
                        ContextMenuGenericTriState(false);
                        ContextMenuGenericMediaView(true);
                        ContextMenuGenericMap(false, false);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error starting kryptonContextMenuGenericBase_Opening...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Ribbons - WorkspaceChanged - Enable / Disable ribbons buttons
        private void RibbonsQTAVisiable(bool saveVisible = true, bool mediaSelectVisible = true, bool mediaPlayerVisible = false)
        {
            try
            {
                kryptonRibbonQATButtonSave.Visible = saveVisible;
                kryptonRibbonQATButtonMediaPreview.Visible = mediaSelectVisible;
                kryptonRibbonQATButtonMediaPoster.Visible = mediaSelectVisible;
                kryptonRibbonQATButtonSelectPrevius.Visible = mediaSelectVisible;
                kryptonRibbonQATButtonSelectNext.Visible = mediaSelectVisible;
                kryptonRibbonQATButtonSelectEqual.Visible = mediaSelectVisible;
                kryptonRibbonQATButtonSelectAll.Visible = mediaSelectVisible;
                kryptonRibbonQATButtonSelectNone.Visible = mediaSelectVisible;
                kryptonRibbonQATButtonSelectToggle.Visible = mediaSelectVisible;

                kryptonRibbonQATButtonMediaPlayerPrevious.Visible = mediaPlayerVisible;
                kryptonRibbonQATButtonMediaPlayerNext.Visible = mediaPlayerVisible;
                kryptonRibbonQATButtonMediaPlayerPlay.Visible = mediaPlayerVisible;
                kryptonRibbonQATButtonMediaPlayerPause.Visible = mediaPlayerVisible;
                kryptonRibbonQATButtonMediaPlayerStop.Visible = mediaPlayerVisible;
                kryptonRibbonQATButtonMediaPlayerFastBackwards.Visible = mediaPlayerVisible;
                kryptonRibbonQATButtonMediaPlayerFastForward.Visible = mediaPlayerVisible;
                kryptonRibbonQATButtonMediaPlayerSlideshowPlay.Visible = mediaPlayerVisible;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void RibbonGroupButtonHomeClipboard(bool enabledCopy = false, bool enabledCutPaste = false, bool enabledUndoRedo = false)
        {
            try
            {
                //Home - Clipboard
                kryptonRibbonGroupButtonHomeCopy.Enabled = enabledCopy;
                kryptonRibbonGroupButtonHomeCut.Enabled = enabledCutPaste;
                kryptonRibbonGroupButtonHomePaste.Enabled = enabledCutPaste;
                kryptonRibbonGroupButtonHomeUndo.Enabled = enabledUndoRedo;
                kryptonRibbonGroupButtonRedo.Enabled = enabledUndoRedo;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void RibbonGroupButtonHomeFileSystem(bool enabled)
        {
            RibbonGroupButtonHomeFileSystem(enabled, enabled, enabled, enabled);
        }
        private void RibbonGroupButtonHomeFileSystem(bool enabledDelete = false, bool enabledRename = false, bool enabledRefresh = false, bool enabledOpenWithEdit = false)
        {
            try
            {
                //Home - FileSystem
                kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = enabledDelete;
                kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = enabledRename;

                kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = enabledRefresh;

                kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = enabledOpenWithEdit;
                kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = enabledOpenWithEdit;
                kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.Enabled = enabledOpenWithEdit;
                kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = enabledOpenWithEdit;
                kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = enabledOpenWithEdit;
                kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = enabledOpenWithEdit;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void RibbonGroupButtonHomeFastCopytext(bool enabledFastCopyPathText = false, bool enabledFastCopyGridOverwrite = false)
        {
            try
            {
                kryptonRibbonGroupButtonHomeCopyText.Enabled = enabledFastCopyPathText;
                kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = enabledFastCopyGridOverwrite;
                kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = enabledFastCopyGridOverwrite;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void RibbonGroupButtonHomeFineAndReplace(bool enabledFind = false, bool enabledRplace = false)
        {
            try
            {
                kryptonRibbonGroupButtonHomeFind.Enabled = enabledFind;
                kryptonRibbonGroupButtonHomeReplace.Enabled = enabledRplace;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void RibbonGroupButtonHomeRotate(bool enabled = false)
        {
            try
            {
                kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = enabled;
                kryptonRibbonGroupButtonMediaFileRotate180.Enabled = enabled;
                kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = enabled;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void RibbonGroupButtonHomeMetadata(bool enabledAutoCorrect = false, bool enabledDeleteHistoryRefresh = false, bool enabledTriState = false, bool enablePreviewPoster = false)
        {
            try
            {
                //Home - Metadata - AutoCorrect
                kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = enabledAutoCorrect;
                kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = enabledAutoCorrect;

                //Home - Metadata - Refresh/Reload
                kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = enabledDeleteHistoryRefresh;
                kryptonRibbonGroupButtonHomeMetadataReload.Enabled = enabledDeleteHistoryRefresh;

                //Home - Metadata - Tag Select
                kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = enabledTriState;
                kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = enabledTriState;
                kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = enabledTriState;

                kryptonRibbonGroupButtonDatGridShowPoster.Enabled = enablePreviewPoster;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region UpdateRibbonsWhenWorkspaceChanged()
        private void UpdateRibbonsWhenWorkspaceChanged()
        {
            try
            {

                bool isSomethingSelected = (ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true).Count >= 1);
                bool isMoreThatOneSelected = (ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true).Count > 1);

                SetPreviewRibbonEnabledStatus(previewStartEnabled: isSomethingSelected, enabled: false);

                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: false, enabledCutPaste: false, enabledUndoRedo: false);
                        //Home - Fast copy text
                        RibbonGroupButtonHomeFastCopytext(false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "TxT";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: false, enabledRplace: false);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: false, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "";

                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: false);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "(Inactive)";
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: true, enabledCutPaste: true, enabledUndoRedo: false);
                        //Home - Fast copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: true, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Folder path";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: true, enabledRplace: false);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: true, enabledRename: true, enabledRefresh: true, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Folder";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "Folder";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Folders";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Files in folder)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Files in folder)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Files in folder)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Folder)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Files in folder)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Files in folder)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: true, enabledTriState: false, enablePreviewPoster: false);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Folder)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Folder)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Inactive)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "(Inactive)";
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: false, enabledCutPaste: false, enabledUndoRedo: false);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: true, enabledRplace: false);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: false, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Inactive)";

                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: false);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Inactive)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "(Inactive)";
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: false, enabledCutPaste: false, enabledUndoRedo: false);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: true, enabledRplace: false);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: false, enabledRename: true, enabledRefresh: false, enabledOpenWithEdit: false);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Inactive)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: false);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Inactive)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "(Inactive)";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "(Inactive)";
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: isSomethingSelected, enabledUndoRedo: false);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: isSomethingSelected, enabledFastCopyGridOverwrite: false);
                        if (isMoreThatOneSelected) kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Filenames";
                        else if (isSomethingSelected) kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Filename";
                        else kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Filenames";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: true, enabledRplace: false);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(
                            enabledDelete: isSomethingSelected, enabledRename: isSomethingSelected,
                            enabledRefresh: isSomethingSelected, enabledOpenWithEdit: isSomethingSelected);

                        if (isMoreThatOneSelected)
                        {
                            kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Files";
                            kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "Files";
                            kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Files";

                            kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Files)";
                        }
                        else if (isSomethingSelected)
                        {
                            kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "File";
                            kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "File";
                            kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Files";

                            kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(File)";
                            kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(File)";
                            kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(File)";
                            kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(File)";
                            kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(File)";
                            kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(File)";
                        }
                        else
                        {
                            kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "File(s)";
                            kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "File(s)";
                            kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Files";

                            kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Files)";
                            kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Files)";
                        }

                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: isSomethingSelected);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: isSomethingSelected, enabledDeleteHistoryRefresh: isSomethingSelected, enabledTriState: false, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Files)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Files)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Files)";

                        this.kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select Previous group of media files. Using the properties set in this ribbon.";
                        this.kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select Next group of media files. Using the properties set in this ribbon.";
                        this.kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all media files that match criterias from selected files.";
                        this.kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all media files";
                        this.kryptonRibbonQATButtonSelectNone.ToolTipBody = "Select none of the media files";
                        this.kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of the media files.";
                        break;

                    case KryptonPages.kryptonPageToolboxTags:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: isSomethingSelected, enabledUndoRedo: isSomethingSelected);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: isSomethingSelected, enabledRplace: isSomethingSelected);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Cells)";

                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: false, enabledTriState: isSomethingSelected, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Cells)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select all cells in Previous Column.";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select all cells in Next Column.";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all cells match current cell.";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all cells";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "Select no cells";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of cells.";
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: isSomethingSelected, enabledUndoRedo: isSomethingSelected);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: isSomethingSelected, enabledRplace: isSomethingSelected);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Cells)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: false, enabledTriState: isSomethingSelected, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Cells)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select all cells in Previous Column.";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select all cells in Next Column.";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all cells match current cell.";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all cells";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "Select no cells";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of cells.";
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: isSomethingSelected, enabledUndoRedo: isSomethingSelected);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: isSomethingSelected, enabledRplace: isSomethingSelected);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Cells)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Cells)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select all cells in Previous Column.";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select all cells in Next Column.";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all cells match current cell.";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all cells";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "Select no cells";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of cells.";
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: isSomethingSelected, enabledUndoRedo: isSomethingSelected);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: isSomethingSelected, enabledRplace: isSomethingSelected);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Cells)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Cells)";
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: false, enabledUndoRedo: false);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: isSomethingSelected, enabledRplace: false);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: false, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Cells)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Cells)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select all cells in Previous Column.";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select all cells in Next Column.";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all cells match current cell.";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all cells";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "Select no cells";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of cells.";
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: false, enabledUndoRedo: false);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: isSomethingSelected, enabledRplace: false);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: false, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Cells)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Cells)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select all cells in Previous Column.";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select all cells in Next Column.";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all cells match current cell.";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all cells";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "Select no cells";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of cells.";
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: false, enabledUndoRedo: false);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: isSomethingSelected, enabledRplace: isSomethingSelected);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Cells)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Cells)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select all cells in Previous Column.";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select all cells in Next Column.";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all cells match current cell.";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all cells";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "Select no cells";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of cells.";
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: isSomethingSelected, enabledUndoRedo: isSomethingSelected);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: isSomethingSelected, enabledRplace: isSomethingSelected);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Cells)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Cells)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select all cells in Previous Column.";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select all cells in Next Column.";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all cells match current cell.";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all cells";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "Select no cells";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of cells.";
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        //Home - Clipboard
                        RibbonGroupButtonHomeClipboard(enabledCopy: isSomethingSelected, enabledCutPaste: isSomethingSelected, enabledUndoRedo: isSomethingSelected);
                        //Home - Fast Copy text
                        RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";
                        //Home - Find and Replace
                        RibbonGroupButtonHomeFineAndReplace(enabledFind: isSomethingSelected, enabledRplace: isSomethingSelected);
                        //Home - FileSystem
                        RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: true, enabledRefresh: false, enabledOpenWithEdit: true);
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "(Inactive)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "(Inactive)";

                        kryptonRibbonGroupButtonHomeFileSystemOpen.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonFileSystemRunCommand.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeFileSystemEdit.TextLine2 = "(Cells)";
                        //Home - Rotate
                        RibbonGroupButtonHomeRotate(enabled: false);
                        //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                        RibbonGroupButtonHomeMetadata(enabledAutoCorrect: true, enabledDeleteHistoryRefresh: false, enabledTriState: false, enablePreviewPoster: isSomethingSelected);
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.TextLine2 = "(Cells)";
                        kryptonRibbonGroupButtonDatGridShowPoster.TextLine2 = "(Cells)";

                        kryptonRibbonQATButtonSelectPrevius.ToolTipBody = "Select all cells in Previous Column.";
                        kryptonRibbonQATButtonSelectNext.ToolTipBody = "Select all cells in Next Column.";
                        kryptonRibbonQATButtonSelectEqual.ToolTipBody = "Select all cells match current cell.";
                        kryptonRibbonQATButtonSelectAll.ToolTipBody = "Select all cells";
                        kryptonRibbonQATButtonSelectNone.ToolTipBody = "Select no cells";
                        kryptonRibbonQATButtonSelectToggle.ToolTipBody = "Invert selection of cells.";
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
        }
        #endregion

        #region UpdateDataGridViewDirtyFlagsWhenPageActivated()
        private void ActionUpdateDataGridViewDirtyFlagsWhenPageActivated()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                DataGridView_UpdatedDirtyFlags(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        private void UpdateDataGridViewDirtyFlagsWhenPageActivated()
        {
            try
            {
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                    case KryptonPages.kryptonPageToolboxPeople:
                    case KryptonPages.kryptonPageToolboxMap:
                    case KryptonPages.kryptonPageToolboxDates:
                        ActionUpdateDataGridViewDirtyFlagsWhenPageActivated();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
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
        }
        #endregion

        #region KryptonPages - Remeber what's active page
        private KryptonPages activeKryptonPage = KryptonPages.None;
        private KryptonPages ActiveKryptonPage
        {
            get
            {
                return activeKryptonPage;
            }
            set
            {
                activeKryptonPage = value;
                UpdateRibbonsWhenWorkspaceChanged();
                UpdateDataGridViewDirtyFlagsWhenPageActivated();
            }
        }

        private void kryptonPageFolderSearchFilterFolder_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageFolderSearchFilterFolder;
        }

        private void kryptonPageFolderSearchFilterSearch_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageFolderSearchFilterSearch;            
        }

        private void kryptonPageFolderSearchFilterFilter_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageFolderSearchFilterFilter;
        }

        private void kryptonPageMediaFiles_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageMediaFiles;
        }

        private void kryptonPageToolboxTags_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageToolboxTags;
        }

        private void kryptonPageToolboxPeople_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageToolboxPeople;
        }

        private void kryptonPageToolboxMap_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageToolboxMap;
        }

        private void kryptonPageToolboxDates_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageToolboxDates;
        }

        private void kryptonPageToolboxExiftool_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageToolboxExiftool;
        }

        private void kryptonPageToolboxWarnings_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageToolboxWarnings;
        }

        private void kryptonPageToolboxProperties_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageToolboxProperties;
        }

        private void kryptonPageToolboxRename_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageToolboxRename;
        }

        private void kryptonPageToolboxConvertAndMerge_Enter(object sender, EventArgs e)
        {
            ActiveKryptonPage = KryptonPages.kryptonPageToolboxConvertAndMerge;
        }
        #endregion

        #region ContextMenuGeneric - Turn on / off

        #region  AssignCompositeTag
        private void ContextMenuGenericAssignCompositeTag(bool visible)
        {
            this.kryptonContextMenuItemAssignCompositeTag.Visible = visible;
        }
        #endregion

        #region  Region Rename
        private void ContextMenuGenericRegionNameRename(bool visible)
        {

            this.kryptonContextMenuItemGenericRegionRename1.Visible = visible;
            this.kryptonContextMenuItemGenericRegionRename2.Visible = visible;
            this.kryptonContextMenuItemGenericRegionRename3.Visible = visible;
            this.kryptonContextMenuItemGenericRegionRename4.Visible = visible;
            this.kryptonContextMenuItemGenericRegionRename5.Visible = visible;
            this.kryptonContextMenuItemGenericRegionRenameFromNearBy.Visible = visible;

            this.kryptonContextMenuItemGenericRegionRenameMostUsed.Visible = visible;
            //this.kryptonContextMenuItemGenericRegionRenameMostUsedList
            //this.kryptonContextMenuItemGenericRegionRenameMostUsedExample

            //this.kryptonContextMenuItemsGenericRegionRenameFromLastUsedList.Visible = visible; //GenericRegionRenameFromLastUsed
            //this.kryptonContextMenuItemGenericRegionRenameFormLastUsedExample.Visible = visible; //GenericRegionRenameFromLastUsed
            this.kryptonContextMenuItemGenericRegionRenameListAll.Visible = visible;
            //this.kryptonContextMenuItemsGenericRegionRenameListAllList.Visible = visible; //GenericRegionRenameListAll
            //this.kryptonContextMenuItemGenericRegionRenameListAllExample.Visible = visible; //GenericRegionRenameListAll
            this.kryptonContextMenuSeparatorGenericEndOfRegionRename.Visible = visible;
        }
        #endregion

        #region Clipboard
        private void ContextMenuGenericClipboard(bool visible)
        {
            ContextMenuGenericClipboard(visible, visible, visible, visible, visible, visible, visible, visible);
        }

        private void ContextMenuGenericClipboard(bool visibleCopy = false, bool visibleCutPaste = false, bool visibleUndoRedo = false, bool visibleCopyText = false, bool visibleFind = false, bool visibleReplace = false,
             bool visibleDelete = false, bool visibleRenameEdit = false, bool visibleSave = false, bool visibleFastCopy = false)
        {
            this.kryptonContextMenuItemGenericCut.Visible = visibleCutPaste;
            this.kryptonContextMenuItemGenericCopy.Visible = visibleCopy;
            this.kryptonContextMenuItemGenericPaste.Visible = visibleCutPaste;
            this.kryptonContextMenuItemGenericCopyText.Visible = visibleCopyText;
            this.kryptonContextMenuItemGenericFastCopyNoOverwrite.Visible = visibleFastCopy;
            this.kryptonContextMenuItemGenericFastCopyWithOverwrite.Visible = visibleFastCopy;
            this.kryptonContextMenuItemGenericDelete.Visible = visibleDelete;
            this.kryptonContextMenuItemGenericRename.Visible = visibleRenameEdit;
            this.kryptonContextMenuItemGenericUndo.Visible = visibleUndoRedo;
            this.kryptonContextMenuItemGenericRedo.Visible = visibleUndoRedo;
            this.kryptonContextMenuItemGenericFind.Visible = visibleFind;
            this.kryptonContextMenuItemGenericReplace.Visible = visibleReplace;
            this.kryptonContextMenuItemGenericSave.Visible = visibleSave;
            this.kryptonContextMenuSeparatorGenericEndOfClipboard.Visible =
                visibleCopy || visibleCutPaste || visibleUndoRedo || visibleFind || visibleReplace || visibleCopyText || visibleDelete || visibleRenameEdit || visibleSave || visibleFastCopy;
        }
        #endregion

        #region FileSystem
        private void ContextMenuGenericFileSystem(bool visible)
        {
            ContextMenuGenericFileSystem(visible, visible, visible, visible);
        }

        private void ContextMenuGenericFileSystem(bool visibleRefreshFolder = false, bool visibleReadSubfolders = false, bool visibleOpenBrowserOnLocation = false, bool visibleOpenRunEdit = false)
        {
            this.kryptonContextMenuItemGenericRefreshFolder.Visible = visibleRefreshFolder;
            this.kryptonContextMenuItemGenericReadSubfolders.Visible = visibleReadSubfolders;
            this.kryptonContextMenuItemGenericOpenFolderLocation.Visible = visibleOpenBrowserOnLocation;
            this.kryptonContextMenuItemGenericOpen.Visible = visibleOpenRunEdit;
            this.kryptonContextMenuItemGenericOpenWith.Visible = visibleOpenRunEdit;
            this.kryptonContextMenuItemOpenAndAssociateWithDialog.Visible = visibleOpenRunEdit;
            //this.kryptonContextMenuItemsGenericOpenWithAppList.Visible = visibleOpenRunEdit; //GenericOpenWith
            //this.kryptonContextMenuItemsGenericOpenWithAppListExample.Visible = visibleOpenRunEdit; //GenericOpenWith
            this.kryptonContextMenuItemGenericOpenVerbEdit.Visible = visibleOpenRunEdit;
            this.kryptonContextMenuItemGenericRunCommand.Visible = visibleOpenRunEdit;
            this.kryptonContextMenuSeparatorGenericEndOfFileSystem.Visible = visibleRefreshFolder || visibleReadSubfolders || visibleOpenBrowserOnLocation || visibleOpenRunEdit;
        }
        #endregion

        #region Metadata
        private void ContextMenuGenericMetadata(bool visible)
        {
            this.kryptonContextMenuItemGenericAutoCorrectRun.Visible = visible;
            this.kryptonContextMenuItemGenericAutoCorrectForm.Visible = visible;
            this.kryptonContextMenuItemGenericMetadataRefreshLast.Visible = visible;
            this.kryptonContextMenuItemGenericMetadataDeleteHistory.Visible = visible;
            this.kryptonContextMenuSeparatorGenericEndOfMetadata.Visible = visible;
        }
        #endregion

        #region Rotate
        private void ContextMenuGenericRotate(bool visible)
        {
            this.kryptonContextMenuItemGenericRotate270.Visible = visible;
            this.kryptonContextMenuItemGenericRotate180.Visible = visible;
            this.kryptonContextMenuItemGenericRotate90.Visible = visible;
            this.kryptonContextMenuSeparatorEndOfRotate.Visible = visible;
        }
        #endregion

        #region Favorite
        private void ContextMenuGenericFavorite(bool visible)
        {
            this.kryptonContextMenuItemGenericFavoriteAdd.Visible = visible;
            this.kryptonContextMenuItemGenericFavoriteDelete.Visible = visible;
            this.kryptonContextMenuItemGenericFavoriteToggle.Visible = visible;
            this.kryptonContextMenuSeparatorGenericEndOfFavorite.Visible = visible;
        }
        #endregion

        #region Show&Hide rows
        private void ContextMenuGenericShowHideRows(bool visible)
        {
            this.kryptonContextMenuItemGenericRowShowFavorite.Visible = visible;
            this.kryptonContextMenuItemGenericRowHideEqual.Visible = visible;
            this.kryptonContextMenuSeparatorGenericEndOfShowHideRows.Visible = visible;
        }
        #endregion

        #region TriState
        private void ContextMenuGenericTriState(bool visible)
        {
            this.kryptonContextMenuItemGenericTriStateOn.Visible = visible;
            this.kryptonContextMenuItemGenericTriStateOff.Visible = visible;
            this.kryptonContextMenuItemGenericTriStateToggle.Visible = visible;
            this.kryptonContextMenuSeparatorGenericEndOfTriState.Visible = visible;
        }
        #endregion

        #region MediaView
        private void ContextMenuGenericMediaView(bool visible)
        {
            this.kryptonContextMenuItemGenericMediaViewAsPoster.Visible = visible;
            this.kryptonContextMenuItemGenericMediaViewAsFull.Visible = visible;
            this.kryptonContextMenuSeparatorGenericEndOfMediaView.Visible = visible;
        }
        #endregion

        #region Map
        private void ContextMenuGenericMap(bool visible, bool locationAnalyticsVisible)
        {
            this.kryptonContextMenuItemMapShowCoordinateOnOpenStreetMap.Visible = visible;
            this.kryptonContextMenuItemMapShowCoordinateOnGoogleMap.Visible = visible;
            this.kryptonContextMenuItemMapReloadUsingNominatim.Visible = visible;
            this.kryptonContextMenuItemToolLocationAnalytics.Visible = locationAnalyticsVisible;
        }
        #endregion

        #endregion

    }
}
