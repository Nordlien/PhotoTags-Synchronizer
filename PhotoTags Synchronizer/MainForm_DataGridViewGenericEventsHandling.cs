using DataGridViewGeneric;
using Krypton.Toolkit;
using LocationNames;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    enum KryptonPages
    {
        None,
        kryptonPageFolderSearchFilterFolder,
        kryptonPageFolderSearchFilterSearch,
        kryptonPageFolderSearchFilterFilter,
        kryptonPageMediaFiles,
        kryptonPageToolboxTags,
        kryptonPageToolboxPeople,
        kryptonPageToolboxMap,
        kryptonPageToolboxDates,
        kryptonPageToolboxExiftool,
        kryptonPageToolboxWarnings,
        kryptonPageToolboxProperties,
        kryptonPageToolboxRename,
        kryptonPageToolboxConvertAndMerge
    }

    public partial class MainForm : KryptonForm
    {
        #region kryptonContextMenuGenericBase_Opening
        private void kryptonContextMenuGenericBase_Opening(object sender, System.ComponentModel.CancelEventArgs e)
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
                else if (((Krypton.Toolkit.KryptonContextMenu)sender).Caller is Furty.Windows.Forms.FolderTreeView)
                {
                    Furty.Windows.Forms.FolderTreeView folderTreeView = (Furty.Windows.Forms.FolderTreeView)((Krypton.Toolkit.KryptonContextMenu)sender).Caller;
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
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(false);
                    ContextMenuGenericFileSystem(false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(false);
                    ContextMenuGenericShowHideRows(false);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(false);

                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: false, visibleCopyText: true,
                        visibleFind: true, visibleReplace: false,
                        visibleDelete: true, visibleRename: true, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: true, visibleReadSubfolders: true, visibleOpenBrowserOnLocation: true, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(true);
                    ContextMenuGenericRotate(true);
                    ContextMenuGenericFavorite(false);
                    ContextMenuGenericShowHideRows(false);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(true);

                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: false, visibleUndoRedo: false, visibleCopyText: false,
                        visibleFind: true, visibleReplace: false,
                        visibleDelete: false, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(false);
                    ContextMenuGenericShowHideRows(false);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(false);
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(false);
                    ContextMenuGenericFileSystem(false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(false);
                    ContextMenuGenericShowHideRows(false);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(false);
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: false, visibleCopyText: true,
                        visibleFind: true, visibleReplace: false,
                        visibleDelete: true, visibleRename: true, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: true, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: true, visibleOpenRunEdit: true);
                    ContextMenuGenericMetadata(true);
                    ContextMenuGenericRotate(true);
                    ContextMenuGenericFavorite(false);
                    ContextMenuGenericShowHideRows(false);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(true);
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                        visibleFind: true, visibleReplace: true,
                        visibleDelete: true, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(true);
                    ContextMenuGenericShowHideRows(true);
                    ContextMenuGenericTriState(true);
                    ContextMenuGenericMediaView(true);
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    ContextMenuGenericRename(true);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                        visibleFind: true, visibleReplace: true,
                        visibleDelete: true, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(true);
                    ContextMenuGenericShowHideRows(true);
                    ContextMenuGenericTriState(true);
                    ContextMenuGenericMediaView(true);
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                        visibleFind: true, visibleReplace: true,
                        visibleDelete: true, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(true);
                    ContextMenuGenericShowHideRows(true);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(true);
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                        visibleFind: true, visibleReplace: true,
                        visibleDelete: true, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(true);
                    ContextMenuGenericShowHideRows(true);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(true);
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                        visibleFind: true, visibleReplace: false,
                        visibleDelete: false, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(true);
                    ContextMenuGenericShowHideRows(true);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(true);
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                        visibleFind: true, visibleReplace: false,
                        visibleDelete: false, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(true);
                    ContextMenuGenericShowHideRows(true);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(true);
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                        visibleFind: true, visibleReplace: false,
                        visibleDelete: true, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(true);
                    ContextMenuGenericShowHideRows(true);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(true);
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                        visibleFind: true, visibleReplace: false,
                        visibleDelete: true, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(true);
                    ContextMenuGenericShowHideRows(true);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(true);
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ContextMenuGenericRename(false);
                    ContextMenuGenericClipboard(
                        visibleCutCopyPaste: true, visibleUndoRedo: true, visibleCopyText: false,
                        visibleFind: true, visibleReplace: false,
                        visibleDelete: true, visibleRename: false, visibleSave: false);
                    ContextMenuGenericFileSystem(
                        visibleRefreshFolder: false, visibleReadSubfolders: false, visibleOpenBrowserOnLocation: false, visibleOpenRunEdit: false);
                    ContextMenuGenericMetadata(false);
                    ContextMenuGenericRotate(false);
                    ContextMenuGenericFavorite(true);
                    ContextMenuGenericShowHideRows(true);
                    ContextMenuGenericTriState(false);
                    ContextMenuGenericMediaView(true);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region Ribbons - WorkspaceChanged - Enable / Disable ribbons buttons

        private void RibbonGroupButtonHomeClipboard(bool enabled)
        {
            RibbonGroupButtonHomeClipboard(enabled, enabled);
        }
        
        private void RibbonGroupButtonHomeClipboard(bool enabledCopy = false, bool enabledCutPaste = false, bool enabledUndoRedo = false)
        {
            //Home - Clipboard
            kryptonRibbonGroupButtonHomeCopy.Enabled = enabledCopy;
            kryptonRibbonGroupButtonHomeCut.Enabled = enabledCutPaste;
            kryptonRibbonGroupButtonHomePaste.Enabled = enabledCutPaste;
            kryptonRibbonGroupButtonHomeUndo.Enabled = enabledUndoRedo;
            kryptonRibbonGroupButtonRedo.Enabled = enabledUndoRedo;
        }

        private void RibbonGroupButtonHomeFileSystem(bool enabled)
        {
            RibbonGroupButtonHomeFileSystem(enabled, enabled, enabled, enabled);
        }
        private void RibbonGroupButtonHomeFileSystem(bool enabledDelete = false, bool enabledRename = false, bool enabledRefresh = false, bool enabledOpenWithEdit = false)
        {
            //Home - FileSystem
            kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = enabledDelete;
            kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = enabledRename;

            kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = enabledRefresh;

            kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = enabledOpenWithEdit;
            kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = enabledOpenWithEdit;
            kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = enabledOpenWithEdit;
            kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = enabledOpenWithEdit;
            kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = enabledOpenWithEdit;
            kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = enabledOpenWithEdit;
        }

        private void RibbonGroupButtonHomeFastCopytext(bool enabledFastCopyPathText = false, bool enabledFastCopyGridOverwrite = false)
        {
            kryptonRibbonGroupButtonHomeCopyText.Enabled = enabledFastCopyPathText;
            kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = enabledFastCopyGridOverwrite;
            kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = enabledFastCopyGridOverwrite;
        }

        private void RibbonGroupButtonHomeFineAndReplace(bool enabledFind = false, bool enabledRplace = false)
        {
            kryptonRibbonGroupButtonHomeFind.Enabled = enabledFind;
            kryptonRibbonGroupButtonHomeReplace.Enabled = enabledRplace;
        }

        private void RibbonGroupButtonHomeRotate(bool enabled = false)
        {
            kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = enabled;
            kryptonRibbonGroupButtonMediaFileRotate180.Enabled = enabled;
            kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = enabled;
        }

        private void RibbonGroupButtonHomeMetadata(bool enabledAutoCorrect = false, bool enabledDeleteHistoryRefresh = false, bool enabledTriState = false)
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
        }
        #endregion

        #region WorkspaceChanged()
        private void WorkspaceChanged()
        {
            bool isSomethingSelected = (imageListView1.SelectedItems.Count >= 1);
            bool isMoreThatOneSelected = (imageListView1.SelectedItems.Count > 1);

            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    //Home - Clipboard
                    RibbonGroupButtonHomeClipboard(enabledCopy: false, enabledCutPaste: false, enabledUndoRedo: false);
                    //Home - Fast copy text
                    RibbonGroupButtonHomeFastCopytext(false);
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "TxT";
                    //Home - Find and Replace
                    RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                    //Home - FileSystem
                    RibbonGroupButtonHomeFileSystem(enabledDelete: false, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "";
                    kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "";
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    //Home - Clipboard
                    RibbonGroupButtonHomeClipboard(enabledCopy: true, enabledCutPaste: true, enabledUndoRedo: false);
                    //Home - Fast copy text
                    RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: false, enabledFastCopyGridOverwrite: false);
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Path";
                    //Home - Find and Replace
                    RibbonGroupButtonHomeFastCopytext(enabledFastCopyPathText: true, enabledFastCopyGridOverwrite: false);
                    //Home - FileSystem
                    RibbonGroupButtonHomeFileSystem(enabledDelete: true, enabledRename: true, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Folder";
                    kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "Folder";
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Folders";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: true, enabledTriState: false);
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
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
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
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
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
                    RibbonGroupButtonHomeFineAndReplace(enabledFind: false, enabledRplace: false);
                    RibbonGroupButtonHomeFileSystem(
                        enabledDelete: isSomethingSelected, enabledRename: isSomethingSelected,
                        enabledRefresh: isSomethingSelected, enabledOpenWithEdit: isSomethingSelected);

                    if (isMoreThatOneSelected)
                    {
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Files";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "Files";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Files";
                    }
                    else if (isSomethingSelected)
                    {
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "File";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "File";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Files";
                    }
                    else
                    {
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "File(s)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "File(s)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Files";
                    }

                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: isSomethingSelected);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: isSomethingSelected, enabledDeleteHistoryRefresh: isSomethingSelected, enabledTriState: false);

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
                    RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Cell(s)";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: isSomethingSelected, enabledDeleteHistoryRefresh: false, enabledTriState: isSomethingSelected);
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
                    RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Cell(s)";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: isSomethingSelected);
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
                    RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Cell(s)";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
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
                    RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Cell(s)";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
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
                    RibbonGroupButtonHomeFileSystem(enabledDelete: false, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Cell(s)";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
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
                    RibbonGroupButtonHomeFileSystem(enabledDelete: false, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Cell(s)";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
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
                    RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Cell(s)";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
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
                    RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: false, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Cell(s)";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
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
                    RibbonGroupButtonHomeFileSystem(enabledDelete: isSomethingSelected, enabledRename: true, enabledRefresh: false, enabledOpenWithEdit: false);
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Cell(s)";
                    //Home - Rotate
                    RibbonGroupButtonHomeRotate(enabled: false);
                    //Home - Metadata - AutoCorrect - Refresh/Reload - TriState/Tag Select
                    RibbonGroupButtonHomeMetadata(enabledAutoCorrect: false, enabledDeleteHistoryRefresh: false, enabledTriState: false);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region Actions triggers

        #region Context Menu Only - ActoinRegionRename1
        private void KryptonContextMenuItemGenericRegionRename1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Context Menu Only - ActoinRegionRename2
        private void KryptonContextMenuItemGenericRegionRename2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Context Menu Only - ActoinRegionRename3
        private void KryptonContextMenuItemGenericRegionRename3_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Context Menu Only - ActoinRegionRenameFromLastUsed
        private void KryptonContextMenuItemGenericRegionRenameFromLastUsed_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Context Menu Only - ActoinRegionRenameListAll
        private void KryptonContextMenuItemGenericRegionRenameListAll_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region ActionCut        
        private void kryptonRibbonGroupButtonHomeCut_Click(object sender, EventArgs e)
        {
            ActionCut();
        }

        private void KryptonContextMenuItemGenericCut_Click(object sender, EventArgs e)
        {
            ActionCut();
        }
        #endregion

        #region ActionCopy
        private void kryptonRibbonGroupButtonHomeCopy_Click(object sender, EventArgs e)
        {
            ActionCopy();
        }

        private void KryptonContextMenuItemGenericCopy_Click(object sender, EventArgs e)
        {
            ActionCopy();
        }
        #endregion

        #region ActionCopyText
        private void kryptonRibbonGroupButtonHomeCopyText_Click(object sender, EventArgs e)
        {
            ActionCopyText();
        }

        private void KryptonContextMenuItemGenericCopyText_Click(object sender, EventArgs e)
        {
            ActionCopyText();
        }
        #endregion

        #region ActionPaste
        private void kryptonRibbonGroupButtonHomePaste_Click(object sender, EventArgs e)
        {
            ActionPaste();
        }
        private void KryptonContextMenuItemGenericPaste_Click(object sender, EventArgs e)
        {
            ActionPaste();
        }
        #endregion 

        #region ActionFileSystemDelete
        private void kryptonRibbonGroupButtonHomeFileSystemDelete_Click(object sender, EventArgs e)
        {
            ActionGridCellAndFileSystemDelete();
        }

        private void KryptonContextMenuItemGenericFileSystemDelete_Click(object sender, EventArgs e)
        {
            ActionGridCellAndFileSystemDelete();
        }
        #endregion 

        #region ActionFileSystemRename
        private void kryptonRibbonGroupButtonHomeFileSystemRename_Click(object sender, EventArgs e)
        {
            ActionFileSystemRename();
        }
        private void KryptonContextMenuItemGenericFileSystemRename_Click(object sender, EventArgs e)
        {
            ActionFileSystemRename();
        }
        #endregion 


        #region ActionUndo
        private void kryptonRibbonGroupButtonHomeUndo_Click(object sender, EventArgs e)
        {
            ActionUndo();
        }

        private void KryptonContextMenuItemGenericUndo_Click(object sender, EventArgs e)
        {
            ActionUndo();
        }
        #endregion 

        #region ActionRedo
        private void kryptonRibbonGroupButtonHomeRedo_Click(object sender, EventArgs e)
        {
            ActionRedo();
        }
        private void KryptonContextMenuItemGenericRedo_Click(object sender, EventArgs e)
        {
            ActionRedo();
        }
        #endregion 

        #region ActionFind
        private void kryptonRibbonGroupButtonHomeFind_Click(object sender, EventArgs e)
        {
            ActionFind();
        }
        private void KryptonContextMenuItemGenericFind_Click(object sender, EventArgs e)
        {
            ActionFind();
        }
        #endregion 

        #region ActionFindAndReplace
        private void kryptonRibbonGroupButtonHomeReplace_Click(object sender, EventArgs e)
        {
            ActionFindAndReplace();
        }
        private void KryptonContextMenuItemGenericReplace_Click(object sender, EventArgs e)
        {
            ActionFindAndReplace();
        }
        #endregion

        #region ActionSave
        
        private void kryptonRibbonQATButtonSave_Click(object sender, EventArgs e)
        {
            ActionSave();            
        }

        private void KryptonContextMenuItemGenericSave_Click(object sender, EventArgs e)
        {
            ActionSave();
        }
        #endregion 


        #region KryptonContextMenuItemGenericFavoriteAdd_Click
        private void KryptonContextMenuItemGenericFavoriteAdd_Click(object sender, EventArgs e) //---------------------------------------
        {
            ActionFavoriteAdd();
        }
        #endregion 

        #region KryptonContextMenuItemGenericFavoriteDelete_Click
        private void KryptonContextMenuItemGenericFavoriteDelete_Click(object sender, EventArgs e)
        {
            ActionFavoriteDelete();
        }
        #endregion 

        #region KryptonContextMenuItemFavoriteToggle_Click
        private void KryptonContextMenuItemFavoriteToggle_Click(object sender, EventArgs e)
        {
            ActionFavoriteToggle();
        }
        #endregion 


        #region ActionRowsShowFavoriteToggle
        private void kryptonRibbonGroupButtonDataGridViewRowsFavorite_Click(object sender, EventArgs e)
        {
            ActionRowsShowFavoriteToggle();
        }

        private void KryptonContextMenuItemGenericRowShowFavorite_Click(object sender, EventArgs e)
        {
            ActionRowsShowFavoriteToggle();
        }
        #endregion 

        #region ActionRowsHideEqualToggle
        private void KryptonContextMenuItemGenericRowHideEqual_Click(object sender, EventArgs e)
        {
            ActionRowsHideEqualToggle();
        }

        private void kryptonRibbonGroupButtonDataGridViewRowsHideEqual_Click(object sender, EventArgs e)
        {
            ActionRowsHideEqualToggle();
        }
        #endregion 


        #region ActionTriStateOn
        private void kryptonRibbonGroupButtonHomeTriStateOn_Click(object sender, EventArgs e)
        {
            ActionTriStateOn();
        }
        private void KryptonContextMenuItemGenericTriStateOn_Click(object sender, EventArgs e)
        {
            ActionTriStateOn();
        }
        #endregion

        #region ActionTriStateOff
        private void kryptonRibbonGroupButtonHomeTriStateOff_Click(object sender, EventArgs e)
        {
            ActionTriStateOff();
        }
        private void KryptonContextMenuItemGenericTriStateOff_Click(object sender, EventArgs e)
        {
            ActionTriStateOff();
        }
        #endregion 

        #region ActionTriStateToggle
        private void kryptonRibbonGroupButtonHomeTriStateToggle_Click(object sender, EventArgs e)
        {
            ActionTriStateToggle();
        }

        private void KryptonContextMenuItemGenericTriStateToggle_Click(object sender, EventArgs e)
        {
            ActionTriStateToggle();
        }
        #endregion

        
       
        //KryptonContextMenuItemGenericRefreshFolder_Click;
        //kryptonRibbonGroupButtonHomeFileSystemRefresh_Click
        
        private void kryptonRibbonGroupButtonFileSystemOpenAssociateDialog_Click(object sender, EventArgs e)
        {

        }


        #region ActionFileSystemRefreshFolder
        private void kryptonRibbonGroupButtonHomeFileSystemRefreshFolder_Click(object sender, EventArgs e)
        {

        }
        private void KryptonContextMenuItemGenericFileSystemRefreshFolder_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Context Menu Only - ReadSubfolders
        private void KryptonContextMenuItemGenericReadSubfolders_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region ActionFileSystemOpenExplorerLocation
        private void kryptonRibbonGroupButtonHomeFileSystemOpenExplorerLocation_Click(object sender, EventArgs e)
        {

        }

        private void KryptonContextMenuItemGenericOpenExplorerLocation_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ActionFileSystemOpen
        private void kryptonRibbonGroupButtonHomeFileSystemOpen_Click(object sender, EventArgs e)
        {

        }

        private void KryptonContextMenuItemGenericOpen_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ActionFileSystemOpenWith
        private void kryptonRibbonGroupButtonHomeFileSystemOpenWith_Click(object sender, EventArgs e)
        {

        }
        private void KryptonContextMenuItemGenericOpenWith_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ActionFileSystemVerbEdit
        private void kryptonRibbonGroupButtonHomeFileSystemVerbEdit_Click(object sender, EventArgs e)
        {

        }
        private void KryptonContextMenuItemGenericFileSystemVerbEdit_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region FileSystemRunCommand
        private void kryptonRibbonGroupButtonFileSystemRunCommand_Click(object sender, EventArgs e)
        {

        }

        private void KryptonContextMenuItemGenericFileSystemRunCommand_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ActionAutoCorrectRun
        private void KryptonContextMenuItemGenericAutoCorrectRun_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void kryptonRibbonGroupButtonHomeAutoCorrectRun_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region ActionAutoCorrectFrom
        private void kryptonRibbonGroupButtonHomeAutoCorrectForm_Click(object sender, EventArgs e)
        {

        }
        private void KryptonContextMenuItemGenericAutoCorrectForm_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ActionMetadataRefreshLast
        private void kryptonRibbonGroupButtonHomeMetadataRefreshLast_Click(object sender, EventArgs e)
        {

        }
        private void KryptonContextMenuItemGenericMetadataRefreshLast_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ActionMetadataReloadDeleteHistory
        private void kryptonRibbonGroupButtonHomeMetadataReloadDeleteHistory_Click(object sender, EventArgs e)
        {

        }
        private void KryptonContextMenuItemGenericMetadataReloadDeleteHistory_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region ActionRotate270
        private void kryptonRibbonGroupButtonMediaFileRotate90CCW_Click(object sender, EventArgs e)
        {

        }
        private void KryptonContextMenuItemGenericRotate270_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ActionRotate180
        private void KryptonContextMenuItemGenericRotate180_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void kryptonRibbonGroupButtonMediaFileRotate180_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region ActionRotate90
        private void kryptonRibbonGroupButtonMediaFileRotate90CW_Click(object sender, EventArgs e)
        {

        }
        private void KryptonContextMenuItemGenericRotate90_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Ribbon Only - ActionFastCopyNoOverwrite
        private void kryptonRibbonGroupButtonHomeFastCopyNoOverwrite_Click(object sender, EventArgs e)
        {
            ActionFastCopyNoOverwrite();
        }
        #endregion 

        #region Ribbon Only - ActionFastCopyOverwrite
        private void kryptonRibbonGroupButtonHomeFastCopyOverwrite_Click(object sender, EventArgs e)
        {
            ActionFastCopyOverwrite();
        }
        #endregion

        #region Context Menu Only - KryptonContextMenuItemGenericMediaViewAsPoster_Click
        private void KryptonContextMenuItemGenericMediaViewAsPoster_Click(object sender, EventArgs e)
        {
            ActionMediaViewAsPoster();
        }
        #endregion 

        #region Context Menu Only - KryptonContextMenuItemGenericMediaViewAsFull_Click
        private void KryptonContextMenuItemGenericMediaViewAsFull_Click(object sender, EventArgs e)
        {
            ActionMediaViewAsFull();
        }
        #endregion 

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
                WorkspaceChanged(); 
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

        #region ContextMenuGenericRename - Turn on / off

        #region  Region Rename
        private void ContextMenuGenericRename(bool visible)
        {
            
            this.kryptonContextMenuItemGenericRegionRename1.Visible = visible;
            this.kryptonContextMenuItemGenericRegionRename2.Visible = visible;
            this.kryptonContextMenuItemGenericRegionRename3.Visible = visible;
            this.kryptonContextMenuItemGenericRegionRenameFromLastUsed.Visible = visible;
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

        private void ContextMenuGenericClipboard(bool visibleCutCopyPaste = false, bool visibleUndoRedo = false, bool visibleCopyText = false, bool visibleFind = false, bool visibleReplace = false,
             bool visibleDelete = false, bool visibleRename = false, bool visibleSave = false)
        {            
            this.kryptonContextMenuItemGenericCut.Visible = visibleCutCopyPaste;
            this.kryptonContextMenuItemGenericCopy.Visible = visibleCutCopyPaste;
            this.kryptonContextMenuItemGenericCopyText.Visible = visibleCopyText;
            this.kryptonContextMenuItemGenericPaste.Visible = visibleCutCopyPaste;
            this.kryptonContextMenuItemGenericDelete.Visible = visibleDelete;
            this.kryptonContextMenuItemGenericRename.Visible = visibleRename;
            this.kryptonContextMenuItemGenericUndo.Visible = visibleUndoRedo;
            this.kryptonContextMenuItemGenericRedo.Visible = visibleUndoRedo;
            this.kryptonContextMenuItemGenericFind.Visible = visibleFind;
            this.kryptonContextMenuItemGenericReplace.Visible = visibleReplace;
            this.kryptonContextMenuItemGenericSave.Visible = visibleSave;
            this.kryptonContextMenuSeparatorGenericEndOfClipboard.Visible =
                visibleCutCopyPaste || visibleUndoRedo || visibleFind || visibleReplace || visibleCopyText || visibleDelete || visibleRename || visibleSave;            
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
        }
        #endregion

        #endregion

        

        //Convert and Merge
        //Date
        //Exiftool
        //ExiftoolWarning
        //Map
        //People
        //Properties
        //Rename
        //TagsAndKeywords


        #region Cut

        #region ActionCut()
        private void ActionCut()
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
                    TagsAndKeywordsCut_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleCut_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapCut_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateCut_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolCut_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningCut_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesCut_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameCut_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeCut_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridGeneric_Cut
        private void DataGridGeneric_Cut(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region ConvertAndMergeCut_Click
        private void ConvertAndMergeCut_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridGeneric_Cut(dataGridView);
        }
        #endregion

        #region DateCut_Click
        private void DateCut_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridGeneric_Cut(dataGridView);
        }
        #endregion

        #region ExiftoolCut_Click
        private void ExiftoolCut_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridGeneric_Cut(dataGridView);
        }
        #endregion

        #region ExiftoolWarningCut_Click
        private void ExiftoolWarningCut_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridGeneric_Cut(dataGridView);
        }
        #endregion

        #region MapCut_Click
        private void MapCut_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridGeneric_Cut(dataGridView);
        }
        #endregion

        #region PeopleCut_Click
        private void PeopleCut_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region PropertiesCut_Click
        private void PropertiesCut_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridGeneric_Cut(dataGridView);
        }
        #endregion

        #region RenameCut_Click
        private void RenameCut_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridGeneric_Cut(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsCut_Click
        private void TagsAndKeywordsCut_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            if (dataGridView.CurrentCell.IsInEditMode)  
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, 0, dataGridView.Columns.Count - 1,
                DataGridViewHandler.GetRowHeadingIndex(dataGridView, header),
                DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header), true);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Copy

        #region ActionCopy()
        private void ActionCopy()
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
                    TagsAndKeywordsCopy_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleCopy_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapCopy_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateCopy_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolCopy_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningCopy_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesCopy_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameCopy_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeCopy_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridGeneric_Copy
        private void DataGridGeneric_Copy(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region ConvertAndMergeCopy_Click
        private void ConvertAndMergeCopy_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridGeneric_Copy(dataGridView);
        }
        #endregion

        #region DateCopy_Click
        private void DateCopy_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridGeneric_Copy(dataGridView);
        }
        #endregion

        #region ExiftoolCopy_Click
        private void ExiftoolCopy_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridGeneric_Copy(dataGridView);
        }
        #endregion

        #region ExiftoolWarningCopy_Click
        private void ExiftoolWarningCopy_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridGeneric_Copy(dataGridView);
        }
        #endregion

        #region MapCopy_Click
        private void MapCopy_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridGeneric_Copy(dataGridView);
        }
        #endregion

        #region PeopleCopy_Click
        private void PeopleCopy_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridGeneric_Copy(dataGridView);  
        }
        #endregion

        #region PropertiesCopy_Click
        private void PropertiesCopy_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridGeneric_Copy(dataGridView);
        }
        #endregion

        #region RenameCopy_Click
        private void RenameCopy_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridGeneric_Copy(dataGridView);
        }
        #endregion 

        #region TagsAndKeywords_Click
        private void TagsAndKeywordsCopy_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridGeneric_Copy(dataGridView);
        }
        #endregion

        #endregion

        #region Paste

        #region ActionPaste
        private void ActionPaste()
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
                    TagsAndKeywordsPaste_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeoplePaste_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapPaste_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DatePaste_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolPaste_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningPaste_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesPaste_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenamePaste_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergePaste_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridViewGenrericPaste
        private void DataGridViewGenrericPaste(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region ConvertAndMerge
        private void ConvertAndMergePaste_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericPaste(dataGridView);
        }
        #endregion

        #region Date
        private void DatePaste_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericPaste(dataGridView);
        }
        #endregion

        #region Exiftool
        private void ExiftoolPaste_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericPaste(dataGridView);
        }
        #endregion

        #region ExiftoolWarning
        private void ExiftoolWarningPaste_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericPaste(dataGridView);
        }
        #endregion

        #region Map
        private void MapPaste_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericPaste(dataGridView);
        }
        #endregion

        #region People
        private void PeoplePaste_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, -1, -1, -1, -1, false);
            ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Properties
        private void PropertiesPaste_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericPaste(dataGridView);
        }
        #endregion
 
        #region Rename
        private void RenamePaste_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericPaste(dataGridView);
        }
        #endregion

        #region TagsAndKeywords
        private void TagsAndKeywordsPaste_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(
                dataGridView, 0, dataGridView.Columns.Count - 1,
                DataGridViewHandler.GetRowHeadingIndex(dataGridView, header),
                DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header), true);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region ActionDelete

        #region ActionDelete
        private void ActionGridCellAndFileSystemDelete()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FilSystemFolderDelete_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    FilSystemSelectedFilesDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    TagsAndKeywordsDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeDelete_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region FilSystemSelectedFilesDelete_Click
        private void FilSystemSelectedFilesDelete_Click()
        {
            try
            {
                if (GlobalData.IsPopulatingAnything()) return;
                folderTreeViewFolder.Enabled = false;
                imageListView1.Enabled = false;

                try
                {
                    if (IsFileInThreadQueueLock(imageListView1))
                    {
                        MessageBox.Show("Can't delete files. Files are being used, you need wait until process is finished.");
                        return;
                    }

                    if (MessageBox.Show("Are you sure you will delete the files", "Files will be deleted!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (new WaitCursor())
                        {
                            UpdateStatusAction("Deleing files and all record about files in database....");
                            filesCutCopyPasteDrag.DeleteSelectedFiles(this, imageListView1);
                            FilesSelected();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                folderTreeViewFolder.Enabled = true;
                imageListView1.Enabled = true;
                imageListView1.Focus();
                DisplayAllQueueStatus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region FilSystemFolderDelete_Click
        private void FilSystemFolderDelete_Click()
        {
            try
            {
                string folder = folderTreeViewFolder.GetSelectedNodePath();

                if (IsFolderInThreadQueueLock(folder))
                {
                    MessageBox.Show("Can't delete folder. Files in folder is been used, you need wait until process is finished.");
                    return;
                }
                try
                {
                    string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories).Take(51).ToArray();
                    if (MessageBox.Show("You are about to delete the folder:\r\n\r\n" +
                        folder + "\r\n\r\n" +
                        "There are " + (fileAndFolderEntriesCount.Length == 51 ? " over 50+" : fileAndFolderEntriesCount.Length.ToString()) + " files found.\r\n\r\n" +
                        "Procced?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        using (new WaitCursor())
                        {
                            UpdateStatusAction("Delete all record about files in database....");
                            int recordAffected = filesCutCopyPasteDrag.DeleteFilesInFolder(this, folderTreeViewFolder, folder);
                            UpdateStatusAction(recordAffected + " records was delete from database....");
                            PopulateImageListView_FromFolderSelected(false, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error when delete folder.");

                    AddError(
                        folder,
                        AddErrorFileSystemRegion, AddErrorFileSystemDeleteFolder, folder, folder,
                        "Was not able to delete folder with files and subfolder!\r\n\r\n" +
                        "From: " + folder + "\r\n\r\n" +
                        "Error message:\r\n" + ex.Message + "\r\n");
                }
                finally
                {
                    GlobalData.DoNotRefreshImageListView = false;
                }
                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region DataGridViewGenrericDelete
        private void DataGridViewGenrericDelete(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region ConvertAndMergeDelete_Click
        private void ConvertAndMergeDelete_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericDelete(dataGridView);
        }
        #endregion 

        #region DateDelete_Click
        private void DateDelete_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericDelete(dataGridView);
        }
        #endregion 

        #region ExiftoolDelete_Click
        private void ExiftoolDelete_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericDelete(dataGridView);
        }
        #endregion 

        #region ExiftoolWarningDelete_Click
        private void ExiftoolWarningDelete_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericDelete(dataGridView);
        }
        #endregion 

        #region MapDelete_Click
        private void MapDelete_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericDelete(dataGridView);
        }
        #endregion

        #region PeopleDelete_Click
        private void PeopleDelete_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region PropertiesDelete_Click
        private void PropertiesDelete_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericDelete(dataGridView);
        }
        #endregion
        
        #region RenameDelete_Click
        private void RenameDelete_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericDelete(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsDelete_Click()
        private void TagsAndKeywordsDelete_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, 0, dataGridView.Columns.Count - 1,
                DataGridViewHandler.GetRowHeadingIndex(dataGridView, header),
                DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header), true);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Undo

        #region ActionUndo
        private void ActionUndo()
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
                    TagsAndKeywordsUndo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleUndo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapUndo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateUndo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolUndo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningUndo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesUndo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameUndo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeUndo_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridViewGenrericUndo
        private void DataGridViewGenrericUndo(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.UndoDataGridView(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region ConvertAndMergeUndo_Click
        private void ConvertAndMergeUndo_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericUndo(dataGridView);
        }
        #endregion

        #region DateUndo_Click
        private void DateUndo_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericUndo(dataGridView);
        }
        #endregion 

        #region ExiftoolUndo_Click
        private void ExiftoolUndo_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericUndo(dataGridView);
        }
        #endregion

        #region ExiftoolWarningUndo_Click
        private void ExiftoolWarningUndo_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericUndo(dataGridView);
        }
        #endregion

        #region Map
        private void MapUndo_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericUndo(dataGridView);
        }
        #endregion

        #region PeopleUndo_Click
        private void PeopleUndo_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.UndoDataGridView(dataGridView);
            ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region PropertiesUndo_Click
        private void PropertiesUndo_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericUndo(dataGridView);
        }
        #endregion
        
        #region RenameUndo_Click
        private void RenameUndo_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericUndo(dataGridView);
        }
        #endregion
        
        #region TagsAndKeywords
        private void TagsAndKeywordsUndo_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.UndoDataGridView(dataGridView);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Redo

        #region ActionRedo
        private void ActionRedo()
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
                    TagsAndKeywordsRedo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleRedo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapRedo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateRedo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolRedo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningRedo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesRedo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameRedo_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeRedo_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridViewGenrericRedo
        private void DataGridViewGenrericRedo(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region ConvertAndMergepRedo_Click
        private void ConvertAndMergeRedo_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericRedo(dataGridView);
        }
        #endregion

        #region DateRedo_Click
        private void DateRedo_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericRedo(dataGridView);
        }
        #endregion 

        #region ExiftoolRedo_Click
        private void ExiftoolRedo_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericRedo(dataGridView);
        }
        #endregion

        #region ExiftoolWarningRedo_Click
        private void ExiftoolWarningRedo_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericRedo(dataGridView);
        }
        #endregion

        #region MapRedo_Click
        private void MapRedo_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericRedo(dataGridView);
        }
        #endregion

        #region PeopleRedo_Click
        private void PeopleRedo_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.RedoDataGridView(dataGridView);
            ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region PropertiesRedo_Click
        private void PropertiesRedo_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericRedo(dataGridView);
        }
        #endregion

        #region RenameRedo_Click
        private void RenameRedo_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericRedo(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsRedo_Click
        private void TagsAndKeywordsRedo_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Find

        #region ActionFind
        private void ActionFind()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderSearchFilterFolderFind_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    FolderSearchFilterSearchFind_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesFind_Click();
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    TagsAndKeywordsFind_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleFind_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapFind_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateFind_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolFind_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningFind_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesFind_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameFind_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeFind_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        private void FolderSearchFilterFolderFind_Click()
        {
            //Open search, add folder name
        }

        private void FolderSearchFilterSearchFind_Click()
        {
            //Click search
        }

        private void MediaFilesFind_Click()
        {

        }


        #region DataGridViewGenrericFind
        private void DataGridViewGenrericFind(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region ConvertAndMergeFind_Click
        private void ConvertAndMergeFind_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericFind(dataGridView);
        }
        #endregion

        #region DateFind_Click
        private void DateFind_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericFind(dataGridView);
        }
        #endregion 

        #region ExiftoolFind_Click
        private void ExiftoolFind_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericFind(dataGridView);
        }
        #endregion

        #region ExiftoolWarningFind_Click
        private void ExiftoolWarningFind_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericFind(dataGridView);
        }
        #endregion

        #region MapFind_Click
        private void MapFind_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericFind(dataGridView);
        }
        #endregion

        #region PeopleFind_Click
        private void PeopleFind_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region PropertiesFind_Click
        private void PropertiesFind_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericFind(dataGridView);
        }
        #endregion

        #region RenameFind_Click
        private void RenameFind_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericFind(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsFind_Click
        private void TagsAndKeywordsFind_Click()
        {            
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region FindAndReplace

        #region ActionFind
        private void ActionFindAndReplace()
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
                    TagsAndKeywordsFindAndReplace_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleFindAndReplace_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapFindAndReplace_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateFindAndReplace_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolFindAndReplace_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningFindAndReplace_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesFindAndReplace_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameFindAndReplace_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeFindAndReplace_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridViewGenrericFindAndReplace
        private void DataGridViewGenrericFindAndReplace(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region ConvertAndMergeFindAndReplace_Click
        private void ConvertAndMergeFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericFindAndReplace(dataGridView);
        }
        #endregion 

        #region DateFindAndReplace_Click
        private void DateFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericFindAndReplace(dataGridView);
        }
        #endregion 

        #region ExiftoolFindAndReplace_Click
        private void ExiftoolFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericFindAndReplace(dataGridView);
        }
        #endregion

        #region ExiftoolWarningFindAndReplace_Click
        private void ExiftoolWarningFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericFindAndReplace(dataGridView);
        }
        #endregion 

        #region MapFindAndReplace_Click
        private void MapFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericFindAndReplace(dataGridView);
        }
        #endregion

        #region PeopleFindAndReplace_Click
        private void PeopleFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region PropertiesFindAndReplace_Click
        private void PropertiesFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericFindAndReplace(dataGridView);
        }
        #endregion

        #region RenameFindAndReplace_Click
        private void RenameFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericFindAndReplace(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsFindAndReplace_Click
        private void TagsAndKeywordsFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region FavoriteAdd

        #region ActionFavoriteAdd
        private void ActionFavoriteAdd()
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
                    TagsAndKeywordsFavoriteAdd_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleFavoriteAdd_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapFavoriteAdd_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateFavoriteAdd_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolFavoriteAdd_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningFavoriteAdd_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesFavoriteAdd_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameFavoriteAdd_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeFavoriteAdd_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteAdd
        private void DataGridViewGenrericFavoriteAdd(DataGridView dataGridView)
        {
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ConvertAndMergeFavoriteAdd_Click
        private void ConvertAndMergeFavoriteAdd_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericFavoriteAdd(dataGridView);
        }
        #endregion

        #region DateFavoriteAdd_Click
        private void DateFavoriteAdd_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericFavoriteAdd(dataGridView);
        }
        #endregion

        #region ExiftoolFavoriteAdd_Click
        private void ExiftoolFavoriteAdd_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericFavoriteAdd(dataGridView);
        }
        #endregion

        #region ExiftoolWarningFavoriteAdd_Click
        private void ExiftoolWarningFavoriteAdd_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericFavoriteAdd(dataGridView);
        }
        #endregion

        #region MapFavoriteAdd_Click
        private void MapFavoriteAdd_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericFavoriteAdd(dataGridView);
        }
        #endregion

        #region PeopleFavoriteAdd_Click
        private void PeopleFavoriteAdd_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewGenrericFavoriteAdd(dataGridView);
        }
        #endregion

        #region PropertiesFavoriteAdd_Click
        private void PropertiesFavoriteAdd_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericFavoriteAdd(dataGridView);
        }
        #endregion

        #region RenameFavoriteAdd_Click
        private void RenameFavoriteAdd_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericFavoriteAdd(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsFavoriteAdd_Click
        private void TagsAndKeywordsFavoriteAdd_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewGenrericFavoriteAdd(dataGridView);
        }
        #endregion

        #endregion

        #region Remove Favorite

        #region ActionFavoriteDelete
        private void ActionFavoriteDelete()
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
                    TagsAndKeywordsFavoriteDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleFavoriteDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapFavoriteDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateFavoriteDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolFavoriteDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningFavoriteDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesFavoriteDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameFavoriteDelete_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeFavoriteDelete_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteDelete
        private void DataGridViewGenrericFavoriteDelete(DataGridView dataGridView)
        {
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ConvertAndMergeFavoriteDelete_Click
        private void ConvertAndMergeFavoriteDelete_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericFavoriteDelete(dataGridView);
        }
        #endregion

        #region DateFavoriteDelete_Click
        private void DateFavoriteDelete_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericFavoriteDelete(dataGridView);
        }
        #endregion 

        #region ExiftoolFavoriteDelete_Click
        private void ExiftoolFavoriteDelete_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericFavoriteDelete(dataGridView);
        }
        #endregion

        #region ExiftoolWarningFavoriteDelete_Click
        private void ExiftoolWarningFavoriteDelete_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericFavoriteDelete(dataGridView);
        }
        #endregion

        #region MapFavoriteDelete_Click
        private void MapFavoriteDelete_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericFavoriteDelete(dataGridView);
        }
        #endregion

        #region PeopleFavoriteDelete_Click
        private void PeopleFavoriteDelete_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewGenrericFavoriteDelete(dataGridView);
        }
        #endregion

        #region PropertiesFavoriteDelete_Click
        private void PropertiesFavoriteDelete_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericFavoriteDelete(dataGridView);
        }
        #endregion
        
        #region RenameFavoriteDelete_Click
        private void RenameFavoriteDelete_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericFavoriteDelete(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsFavoriteDelete_Click
        private void TagsAndKeywordsFavoriteDelete_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewGenrericFavoriteDelete(dataGridView);
        }
        #endregion

        #endregion

        #region Toogle Favorite

        #region ActionFavoriteToogle
        private void ActionFavoriteToggle()
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
                    TagsAndKeywordsFavoriteToogle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleFavoriteToogle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapFavoriteToogle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateFavoriteToogle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolFavoriteToogle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningFavoriteToogle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesFavoriteToogle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameFavoriteToogle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeFavoriteToogle_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteToogle
        private void DataGridViewGenrericFavoriteToogle(DataGridView dataGridView)
        {
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ConvertAndMergeFavoriteToogle_Click
        private void ConvertAndMergeFavoriteToogle_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericFavoriteToogle(dataGridView);
        }
        #endregion

        #region DateFavoriteToogle_Click
        private void DateFavoriteToogle_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericFavoriteToogle(dataGridView);
        }
        #endregion 

        #region ExiftoolFavoriteToogle_Click
        private void ExiftoolFavoriteToogle_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericFavoriteToogle(dataGridView);
        }
        #endregion

        #region ExiftoolWarningFavoriteToogle_Click
        private void ExiftoolWarningFavoriteToogle_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericFavoriteToogle(dataGridView);
        }
        #endregion

        #region MapFavoriteToogle_Click
        private void MapFavoriteToogle_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericFavoriteToogle(dataGridView);
        }
        #endregion

        #region PeopleFavoriteToogle_Click
        private void PeopleFavoriteToogle_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewGenrericFavoriteToogle(dataGridView);
        }
        #endregion

        #region PropertiesFavoriteToogle_Click
        private void PropertiesFavoriteToogle_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericFavoriteToogle(dataGridView);
        }
        #endregion
        
        #region RenameFavoriteToogle_Click
        private void RenameFavoriteToogle_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericFavoriteToogle(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsFavoriteToogle_Click
        private void TagsAndKeywordsFavoriteToogle_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewGenrericFavoriteToogle(dataGridView);
        }
        #endregion

        #endregion

        #region ActionRowsShowFavoriteToggle 

        #region UpdateBottonsEqualAndFavorite
        private void UpdateBottonsEqualAndFavorite(bool hideEqualColumns, bool showFavouriteColumns)
        {
            kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Checked = hideEqualColumns;
            kryptonRibbonGroupButtonDataGridViewRowsFavorite.Checked = showFavouriteColumns;

            DataGridView dataGridView = GetActiveTabDataGridView();
            /*
            switch (GetActiveTabTag())
            {
                
                case LinkTabAndDataGridViewNameTags:
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemKeywordsHideEqualRows, DataGridViewHandler.HideEqualColumns(dataGridView));
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemKeywordsShowFavoriteRows, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
                    break;
                case LinkTabAndDataGridViewNameMap:
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemMapHideEqual, DataGridViewHandler.HideEqualColumns(dataGridView));
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemMapShowFavorite, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
                    break;
                case LinkTabAndDataGridViewNamePeople:
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemPeopleHideEqualRows, DataGridViewHandler.HideEqualColumns(dataGridView));
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemPeopleShowFavorite, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
                    break;
                case LinkTabAndDataGridViewNameDates:
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemDateHideEqualRows, DataGridViewHandler.HideEqualColumns(dataGridView));
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemDateShowFavorite, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
                    break;
                case LinkTabAndDataGridViewNameExiftool:
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemExiftoolHideEqual, DataGridViewHandler.HideEqualColumns(dataGridView));
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemExiftoolSHowFavorite, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
                    break;
                case LinkTabAndDataGridViewNameWarnings:
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemExiftoolWarningHideEqual, DataGridViewHandler.HideEqualColumns(dataGridView));
                    DataGridViewHandler.UpdatedStripMenuItem(dataGridView, toolStripMenuItemExiftoolWarningShowFavorite, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
                    break;
                case LinkTabAndDataGridViewNameProperties:
                    //ee.Checked = hideEqualColumns;
                    //ff.Checked = hideEqualColumns;
                    throw new NotImplementedException();
                    break;
                case LinkTabAndDataGridViewNameRename:
                    //ee.Checked = hideEqualColumns;
                    //ff.Checked = hideEqualColumns;
                    throw new NotImplementedException();
                    break;
                case LinkTabAndDataGridViewNameConvertAndMerge:
                    //ee.Checked = hideEqualColumns;
                    //ff.Checked = hideEqualColumns;
                    throw new NotImplementedException();
                    break;
                default: throw new NotImplementedException();
            }
            */
        }
        #endregion 

        #region ActionRowsShowFavoriteToggle
        private void ActionRowsShowFavoriteToggle()
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
                    TagsAndKeywordsRowsShowFavoriteToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleRowsShowFavoriteToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapRowsShowFavoriteToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateRowsShowFavoriteToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolRowsShowFavoriteToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningRowsShowFavoriteToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesRowsShowFavoriteToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameRowsShowFavoriteToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeRowsShowFavoriteToggle_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteToogle
        private void DataGridViewGenrericShowFavoriteToogle(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region ConvertAndMergeShowFavoriteToggle_Click
        private void ConvertAndMergeRowsShowFavoriteToggle_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericShowFavoriteToogle(dataGridView);
        }
        #endregion 

        #region DateShowFavoriteToggle_Click
        private void DateRowsShowFavoriteToggle_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericShowFavoriteToogle(dataGridView);
        }
        #endregion 

        #region ExiftoolShowFavoriteToggle_Click
        private void ExiftoolRowsShowFavoriteToggle_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericShowFavoriteToogle(dataGridView);
        }
        #endregion

        #region ExiftoolWarningShowFavoriteToggle_Click
        private void ExiftoolWarningRowsShowFavoriteToggle_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericShowFavoriteToogle(dataGridView);
        }
        #endregion

        #region MapShowFavoriteToggle_Click
        private void MapRowsShowFavoriteToggle_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericShowFavoriteToogle(dataGridView);
        }
        #endregion

        #region PeopleShowFavoriteToggle_Click
        private void PeopleRowsShowFavoriteToggle_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewGenrericShowFavoriteToogle(dataGridView);
        }
        #endregion

        #region PropertiesShowFavoriteToggle_Click
        private void PropertiesRowsShowFavoriteToggle_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericShowFavoriteToogle(dataGridView);
        }
        #endregion

        #region RenameShowFavoriteToggle_Click
        private void RenameRowsShowFavoriteToggle_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericShowFavoriteToogle(dataGridView);
        }
        #endregion

        #region TagsAndKeywords
        private void TagsAndKeywordsRowsShowFavoriteToggle_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewGenrericShowFavoriteToogle(dataGridView);
        }
        #endregion

        #endregion

        #region ActionRowsHideEqualToggle

        #region ActionRowsHideEqualToggle
        private void ActionRowsHideEqualToggle()
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
                    TagsAndKeywordsRowsHideEqualToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleRowsHideEqualToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapRowsHideEqualToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateRowsHideEqualToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolRowsHideEqualToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningRowsHideEqualToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesRowsHideEqualToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameRowsHideEqualToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeRowsHideEqualToggle_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteToogle
        private void DataGridViewGenrericRowsHideEqualToogle(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region ConvertAndMergeRowsHideEqualToggle_Click
        private void ConvertAndMergeRowsHideEqualToggle_Click()
        {
            DataGridView dataGridView = dataGridViewConvertAndMerge;
            DataGridViewGenrericRowsHideEqualToogle(dataGridView);
        }
        #endregion

        #region DateRowsHideEqualToggle_Click
        private void DateRowsHideEqualToggle_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewGenrericRowsHideEqualToogle(dataGridView);
        }
        #endregion

        #region ExiftoolRowsHideEqualToggle_Click
        private void ExiftoolRowsHideEqualToggle_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            DataGridViewGenrericRowsHideEqualToogle(dataGridView);
        }
        #endregion

        #region ExiftoolWarningRowsHideEqualToggle_Click
        private void ExiftoolWarningRowsHideEqualToggle_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            DataGridViewGenrericRowsHideEqualToogle(dataGridView);
        }
        #endregion

        #region MapRowsHideEqualToggle_Click
        private void MapRowsHideEqualToggle_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewGenrericRowsHideEqualToogle(dataGridView);
        }
        #endregion

        #region PeopleRowsHideEqualToggle_Click
        private void PeopleRowsHideEqualToggle_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewGenrericRowsHideEqualToogle(dataGridView);
        }
        #endregion

        #region PropertiesRowsHideEqualToggle_Click
        private void PropertiesRowsHideEqualToggle_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            DataGridViewGenrericRowsHideEqualToogle(dataGridView);
        }
        #endregion

        #region RenameRowsHideEqualToggle_Click
        private void RenameRowsHideEqualToggle_Click()
        {
            DataGridView dataGridView = dataGridViewRename;
            DataGridViewGenrericRowsHideEqualToogle(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsRowsHideEqualToggle_Click
        private void TagsAndKeywordsRowsHideEqualToggle_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewGenrericRowsHideEqualToogle(dataGridView);
        }
        #endregion

        #endregion

        #region ActionTriStateToggle

        #region DataGridViewGenericTagActionToggle
        private void DataGridViewGenericTagActionToggle(DataGridView dataGridView, string header, NewState newState)
        {
            if (!dataGridView.Enabled) return;
            DataGridViewHandler.ToggleSelected(dataGridView, header, newState);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region ActionTriStateToggle
        private void ActionTriStateToggle()
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
                    TagsAndKeywordsTriStateToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleTriStateToggle_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    SaveProperties();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    MessageBox.Show("Not implemented");
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    SaveConvertAndMerge();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion 

        #region TagsAndKeywordsTriStateToggle_Click 
        private void TagsAndKeywordsTriStateToggle_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;            
            DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Toggle);
        }
        #endregion

        #region PeopleTriStateToggle_Click
        private void PeopleTriStateToggle_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Toggle);
        }
        #endregion

        #endregion 

        #region ActionTriStateOn

        #region ActionTriStateOn
        private void ActionTriStateOn()
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
                    TagsAndKeywordsTriStateOn_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleTriStateOn_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    SaveProperties();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    MessageBox.Show("Not implemented");
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    SaveConvertAndMerge();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion 

        #region TagsAndKeywordsTriStateOn_Click
        private void TagsAndKeywordsTriStateOn_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Set);
        }
        #endregion

        #region PeopleTriStateOn_Click
        private void PeopleTriStateOn_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Set);
        }
        #endregion

        #endregion 

        #region ActionTriStateOff

        #region ActionTriStateOff

        private void ActionTriStateOff()
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
                    TagsAndKeywordsTriStateOff_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleTriStateOff_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    SaveProperties();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    MessageBox.Show("Not implemented");
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    SaveConvertAndMerge();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region TagsAndKeywordsTriStateOff_Click
        private void TagsAndKeywordsTriStateOff_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords,NewState.Remove);
        }
        #endregion

        #region PeopleTriStateOff_Click
        private void PeopleTriStateOff_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Remove);
        }
        #endregion
        
        #endregion 


        #region ActionSave
        private void ActionSave()
        {
            try
            {
                this.Activate();
                this.Validate(); //Get the latest changes, that are text in edit mode
                SaveActiveTabData();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveActiveTabData()
        {
            if (GlobalData.IsPopulatingAnything()) return;
            if (GlobalData.IsSaveButtonPushed) return;
            GlobalData.IsSaveButtonPushed = true;
            this.Enabled = false;
            using (new WaitCursor())
            {
                /*
                switch (GetActiveTabTag())
                {
                    case LinkTabAndDataGridViewNameTags:
                        break;
                    case LinkTabAndDataGridViewNameMap:
                        break;
                    case LinkTabAndDataGridViewNamePeople:
                        break;
                    case LinkTabAndDataGridViewNameDates:
                        break;
                    case LinkTabAndDataGridViewNameExiftool:
                        break;
                    case LinkTabAndDataGridViewNameWarnings:
                        break;
                    case LinkTabAndDataGridViewNameProperties:
                        break;
                    case LinkTabAndDataGridViewNameRename:
                        break;
                    case LinkTabAndDataGridViewNameConvertAndMerge:
                        break;
                    default: throw new NotImplementedException();
                }*/

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
                        SaveDataGridViewMetadata();
                        GlobalData.IsAgregatedProperties = false;
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        SaveProperties();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        MessageBox.Show("Not implemented");
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        SaveConvertAndMerge();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            GlobalData.IsSaveButtonPushed = false;
            this.Enabled = true;
        }
        #endregion


        #region ActionCopyText
        private void ActionCopyText()
        {
            
        }
        #endregion

        #region ActionFastCopyNoOverwrite

        #region ActionFastCopyNoOverwrite
        private void ActionFastCopyNoOverwrite()
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
                    TagsAndKeywordsFastCopyTextNoOverwrite_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapFastCopyTextNoOverwrite_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
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
        #endregion

        #region TagsAndKeywordsFastCopyTextNoOverwrite_Click
        private void TagsAndKeywordsFastCopyTextNoOverwrite_Click()
        {
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, false);
        }
        #endregion

        #region MapFastCopyTextNoOverwrite_Click
        private void MapFastCopyTextNoOverwrite_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridView, DataGridViewHandlerMap.headerMedia, false);
        }
        #endregion

        #endregion 

        #region ActionFastCopyOverwrite

        #region ActionFastCopyOverwrite
        private void ActionFastCopyOverwrite()
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
                    TagsAndKeywordsFastCopyTextOverwrite_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapFastCopyTextAndOverwrite_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
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
        #endregion

        #region TagsAndKeywordsFastCopyTextOverwrite_Click
        private void TagsAndKeywordsFastCopyTextOverwrite_Click()
        {
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, true);
        }
        #endregion

        #region MapFastCopyTextAndOverwrite_Click       
        private void MapFastCopyTextAndOverwrite_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridView, DataGridViewHandlerMap.headerMedia, true);


            List<int> columnUpdated = new List<int>();

            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                if (!columnUpdated.Contains(dataGridViewCell.ColumnIndex))
                {
                    DataGridViewGenericColumn gridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, dataGridViewCell.ColumnIndex);
                    if (gridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                    {
                        DataGridViewGenericRow gridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, dataGridViewCell.RowIndex);
                        //gridViewGenericRow.HeaderName.Equals(DataGridViewHandlerMap.headerMedia) &&

                        if (!gridViewGenericRow.HeaderName.Equals(DataGridViewHandlerMap.headerMedia) &&
                            gridViewGenericRow.RowName.Equals(DataGridViewHandlerMap.tagCoordinates))
                        {
                            object cellValue = DataGridViewHandler.GetCellValue(dataGridViewMap, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                            if (cellValue != null)
                            {
                                string coordinate = cellValue.ToString();
                                //UpdateBrowserMap(coordinate);
                                DataGridViewHandlerMap.PopulateGrivViewMapNomnatatim(dataGridView, dataGridViewCell.ColumnIndex, LocationCoordinate.Parse(coordinate));
                                columnUpdated.Add(dataGridViewCell.ColumnIndex);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region ActionRename
        private void ActionFileSystemRename()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderRename();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediafilesRename();
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    CellRename();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    CellRename();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    CellRename();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    CellRename();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    CellRename();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    CellRename();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    CellRename();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    CellRename();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region 
        private void CellRename()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            dataGridView.BeginEdit(true);
        }
        #endregion 

        #region 
        private void MediafilesRename()
        {
            kryptonWorkspaceCellToolbox.SelectedPage = kryptonPageToolboxRename;
        }
        #endregion 

        #region 
        private void FolderRename()
        {
            folderTreeViewFolder.SelectedNode.BeginEdit();
        }
        #endregion 


        private void ActionMediaViewAsPoster()
        {

        }

        private void ActionMediaViewAsFull()
        {
        }

        #region DataGridView Keydown

        #region Keydown - People
        private void dataGridViewPeople_KeyDown(object sender, KeyEventArgs e)
        {
            triStateButtomClick = false;            
        }
        #endregion

        #region Keydown - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_KeyDown(object sender, KeyEventArgs e)
        {            
            triStateButtomClick = false; 
        }
        #endregion

        #endregion


        #region Cell BeginEdit

        #region Cell BeginEdit - Date
        private void dataGridViewDate_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Exiftool
        private void dataGridViewExifTool_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - ExiftoolWarning
        private void dataGridViewExifToolWarning_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Map
        private void dataGridViewMap_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - People
        private void dataGridViewPeople_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (triStateButtomClick)
            {
                e.Cancel = true;
                return;
            }

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Properties
        private void dataGridViewProperties_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Rename
        private void dataGridViewRename_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (triStateButtomClick)
            {
                e.Cancel = true;
                return;
            }

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #endregion

        #region Cell Painting

        #region Cell Painting - Convert and Merge
        private void dataGridViewConvertAndMerge_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
            //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);

            //Draw red line for drag and drop
            DataGridView dataGridView = (DataGridView)sender;
            if (e.RowIndex == dragdropcurrentIndex && e.RowIndex > -1 && dragdropcurrentIndex < DataGridViewHandler.GetRowCount(dataGridView))
            {
                Pen p = new Pen(Color.Red, 2);
                e.Graphics.DrawLine(p, e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height - 1, e.CellBounds.Right, e.CellBounds.Top + e.CellBounds.Height - 1);
            }
        }
        #endregion 

        #region Cell Painting - Date
        private void dataGridViewDate_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - Exiftool
        private void dataGridViewExifTool_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - ExiftoolWarning
        private void dataGridViewExifToolWarning_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - Map
        private void dataGridViewMap_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - People
        private void dataGridViewPeople_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            string header = DataGridViewHandlerPeople.headerPeople;

            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            DataGridViewHandler.CellPaintingColumnHeaderRegionsInThumbnail(sender, e);
            DataGridViewHandler.CellPaintingColumnHeaderMouseRegion(sender, e, drawingRegion, peopleMouseDownX, peopleMouseDownY, peopleMouseMoveX, peopleMouseMoveY);

            DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
            if (gridViewGenericDataRow == null) return; //Don't paint anything TriState on "New Empty Row" for "new Keywords"

            DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
            if (e.ColumnIndex > -1)
            {
                if (dataGridViewGenericDataColumn == null) return; //Data is not set, no point to check more.
                if (dataGridViewGenericDataColumn.Metadata == null) return; //Don't paint TriState button when MetaData is null (data not loaded)
            }

            //If people region row
            if (gridViewGenericDataRow.HeaderName.Equals(DataGridViewHandlerPeople.headerPeople))
            {
                if (!gridViewGenericDataRow.IsHeader && e.ColumnIndex > -1)
                {
                    MetadataLibrary.RegionStructure region = DataGridViewHandler.GetCellRegionStructure(dataGridView, e.ColumnIndex, e.RowIndex);
                    Image regionThumbnail = (Image)Properties.Resources.RegionLoading;
                    if (region == null)
                    {
                        e.Handled = false;
                        return;
                    }
                    else if (region.Thumbnail != null) regionThumbnail = region.Thumbnail;
                    DataGridViewHandler.DrawImageAndSubText(sender, e, regionThumbnail, e.Value.ToString(), DataGridViewHandler.ColorHeaderImage);

                    e.Handled = true;
                }

                DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            }
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            
        }
        #endregion

        #region Cell Painting - Properties
        private void dataGridViewProperties_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - Rename
        private void dataGridViewRename_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
            //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            
        }
        #endregion

        #endregion

        

        //ConvertAndMerge
        //Date
        //Exiftool
        //ExiftoolWarning
        //Map
        //People
        //Properties
        //Rename
        //TagsAndKeywords


    }
}
