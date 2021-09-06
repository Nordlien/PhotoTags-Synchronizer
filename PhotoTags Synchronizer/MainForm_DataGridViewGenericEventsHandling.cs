using System;
using System.Drawing;
using System.Windows.Forms;
using DataGridViewGeneric;
using System.Collections.Generic;
using MetadataLibrary;
using LocationNames;
using Krypton.Toolkit;

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

        private void kryptonContextMenuGeneric_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    this.kryptonContextMenuItemGenericCut.Visible = false;
                    this.kryptonContextMenuItemGenericCopy.Visible = false;
                    this.kryptonContextMenuItemGenericCopyText.Visible = false;
                    this.kryptonContextMenuItemGenericPaste.Visible = false;
                    this.kryptonContextMenuItemGenericDelete.Visible = false;
                    this.kryptonContextMenuItemGenericRename.Visible = false;
                    this.kryptonContextMenuItemGenericUndo.Visible = false;
                    this.kryptonContextMenuItemGenericRedo.Visible = false;
                    this.kryptonContextMenuItemGenericFind.Visible = false;
                    this.kryptonContextMenuItemGenericReplace.Visible = false;
                    this.kryptonContextMenuItemGenericSave.Visible = false;
                    this.kryptonContextMenuItemGenericFavoriteAdd.Visible = false;
                    this.kryptonContextMenuItemGenericFavoriteDelete.Visible = false;
                    this.kryptonContextMenuItemFavoriteToggle.Visible = false;
                    this.kryptonContextMenuItemGenericRowShowFavorite.Visible = false;
                    this.kryptonContextMenuItemGenericRowHideEqual.Visible = false;
                    this.kryptonContextMenuItemGenericTriStateOn.Visible = false;
                    this.kryptonContextMenuItemGenericTriStateOff.Visible = false;
                    this.kryptonContextMenuItemGenericTriStateToggle.Visible = false;
                    this.kryptonContextMenuItemGenericMediaViewAsPoster.Visible = false;
                    this.kryptonContextMenuItemGenericMediaViewAsFull.Visible = false;

                    this.kryptonContextMenuItemGenericOpen.Visible = false;
                    this.kryptonContextMenuItemGenericOpenWith.Visible = false;
                    //this.kryptonContextMenuItemsOpenWithAppList.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {this.kryptonContextMenuItemsOpenWithAppListExample});
                    //this.kryptonContextMenuItemsOpenWithAppListExample.Text = "Open with example";
                    this.kryptonContextMenuItemGenericOpenVerbEdit.Visible = false;
                    this.kryptonContextMenuItemGenericRunCommand.Visible = false;

                    this.kryptonContextMenuItemGenericAutoCorrectRun.Visible = false;
                    this.kryptonContextMenuItemGenericAutoCorrectForm.Visible = false;
                    this.kryptonContextMenuItemGenericRotate270.Visible = false;
                    this.kryptonContextMenuItemGenericRotate180.Visible = false;
                    this.kryptonContextMenuItemGenericRotate90.Visible = false;
                    this.kryptonContextMenuItemGenericMetadataRefreshLast.Visible = false;
                    this.kryptonContextMenuItemGenericMetadataDeleteHistory.Visible = false;

                    
                    
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    this.kryptonContextMenuItemGenericCut.Visible = true;
                    this.kryptonContextMenuItemGenericCopy.Visible = true;
                    this.kryptonContextMenuItemGenericCopyText.Visible = true;
                    this.kryptonContextMenuItemGenericPaste.Visible = true;
                    this.kryptonContextMenuItemGenericDelete.Visible = true;
                    this.kryptonContextMenuItemGenericRename.Visible = false;
                    this.kryptonContextMenuItemGenericUndo.Visible = false;
                    this.kryptonContextMenuItemGenericRedo.Visible = false;
                    this.kryptonContextMenuItemGenericFind.Visible = false;
                    this.kryptonContextMenuItemGenericReplace.Visible = false;
                    this.kryptonContextMenuItemGenericSave.Visible = false;
                    this.kryptonContextMenuItemGenericFavoriteAdd.Visible = false;
                    this.kryptonContextMenuItemGenericFavoriteDelete.Visible = false;
                    this.kryptonContextMenuItemFavoriteToggle.Visible = false;
                    this.kryptonContextMenuItemGenericRowShowFavorite.Visible = false;
                    this.kryptonContextMenuItemGenericRowHideEqual.Visible = false;
                    this.kryptonContextMenuItemGenericTriStateOn.Visible = false;
                    this.kryptonContextMenuItemGenericTriStateOff.Visible = false;
                    this.kryptonContextMenuItemGenericTriStateToggle.Visible = false;
                    this.kryptonContextMenuItemGenericMediaViewAsPoster.Visible = false;
                    this.kryptonContextMenuItemGenericMediaViewAsFull.Visible = false;
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
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
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }


        #region KryptonPages
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

        #region Ribbons - WorkspaceChanged
        private void WorkspaceChanged()
        {            
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = false;
                    kryptonRibbonGroupButtonHomeCut.Enabled = false;
                    kryptonRibbonGroupButtonHomePaste.Enabled = false;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = false;
                    kryptonRibbonGroupButtonRedo.Enabled = false;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = false;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = false;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;

                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;

                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = true;
                    kryptonRibbonGroupButtonHomePaste.Enabled = true;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = false;
                    kryptonRibbonGroupButtonRedo.Enabled = false;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = true;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Path";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = false;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = false;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = true;
                    kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Folder";
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "Folder";
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = true;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Folders";

                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = true;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = true;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = true;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = true;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = false;
                    kryptonRibbonGroupButtonHomeCut.Enabled = false;
                    kryptonRibbonGroupButtonHomePaste.Enabled = false;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = false;
                    kryptonRibbonGroupButtonRedo.Enabled = false;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = false;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = false;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = false;
                    kryptonRibbonGroupButtonHomeCut.Enabled = false;
                    kryptonRibbonGroupButtonHomePaste.Enabled = false;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = false;
                    kryptonRibbonGroupButtonRedo.Enabled = false;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = false;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = false;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = true;
                    kryptonRibbonGroupButtonHomePaste.Enabled = true;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = false;
                    kryptonRibbonGroupButtonRedo.Enabled = false;

                    //Home - Fast Copy text
                    if (imageListView1.SelectedItems.Count == 0)
                    {
                        kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Filenames";
                    }
                    else if (imageListView1.SelectedItems.Count == 1)
                    {
                        kryptonRibbonGroupButtonHomeCopyText.Enabled = true;
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Filename";
                    }
                    else
                    {
                        kryptonRibbonGroupButtonHomeCopyText.Enabled = true;
                        kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Filenames";
                    }

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = false;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = false;

                    //Home - FileSystem
                    if (imageListView1.SelectedItems.Count == 0)
                    {
                        kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "File(s)";
                        kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "File(s)";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Files";
                    }
                    else if (imageListView1.SelectedItems.Count == 1)
                    {
                        kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = true;
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "File";
                        kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = true;
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "File";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = true;
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Files";
                    } else
                    {
                        kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = true;
                        kryptonRibbonGroupButtonHomeFileSystemDelete.TextLine2 = "Files";
                        kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = true;
                        kryptonRibbonGroupButtonHomeFileSystemRename.TextLine2 = "Files";
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = true;
                        kryptonRibbonGroupButtonHomeFileSystemRefresh.TextLine2 = "Files";
                    }

                    if (imageListView1.SelectedItems.Count == 0)
                    {
                        kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                        kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                        kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                        kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;
                    }
                    else
                    {
                        kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = true;
                        kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = true;
                        kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = true;
                        kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = true;
                        kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = true;
                        kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = true;
                    }

                    //Home - Rotate
                    if (imageListView1.SelectedItems.Count == 0)
                    {
                        kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                        kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                        kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;
                    }
                    else
                    {
                        kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = true;
                        kryptonRibbonGroupButtonMediaFileRotate180.Enabled = true;
                        kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = true;
                    }

                    //Home - Metadata - AutoCorrect
                    if (imageListView1.SelectedItems.Count == 0)
                    {
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;
                    }
                    else
                    {
                        kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = true;
                        kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = true;
                    }

                    //Home - Metadata - Refresh/Reload
                    if (imageListView1.SelectedItems.Count == 0)
                    {
                        kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                        kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;
                    }
                    else
                    {
                        kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = true;
                        kryptonRibbonGroupButtonHomeMetadataReload.Enabled = true;
                    }

                    //Home - Metadata - 
                    if (imageListView1.SelectedItems.Count == 0)
                    {
                        kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                        kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                        kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    } 
                    else
                    {
                        kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = true;
                        kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = true;
                        kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = true;
                    }

                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = true;
                    kryptonRibbonGroupButtonHomePaste.Enabled = true;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = true;
                    kryptonRibbonGroupButtonRedo.Enabled = true;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = true;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = true;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = true;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = true;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = true;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = true;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = true;
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = true;
                    kryptonRibbonGroupButtonHomePaste.Enabled = true;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = true;
                    kryptonRibbonGroupButtonRedo.Enabled = true;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = true;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = true;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = true;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = true;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = true;
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = true;
                    kryptonRibbonGroupButtonHomePaste.Enabled = true;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = true;
                    kryptonRibbonGroupButtonRedo.Enabled = true;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = true;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = true;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = true;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = true;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = true;
                    kryptonRibbonGroupButtonHomePaste.Enabled = true;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = true;
                    kryptonRibbonGroupButtonRedo.Enabled = true;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = true;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = true;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = true;
                    kryptonRibbonGroupButtonHomePaste.Enabled = true;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = true;
                    kryptonRibbonGroupButtonRedo.Enabled = true;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = true;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = true;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = false;
                    kryptonRibbonGroupButtonHomePaste.Enabled = false;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = false;
                    kryptonRibbonGroupButtonRedo.Enabled = false;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = true;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = true;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = false;
                    kryptonRibbonGroupButtonHomePaste.Enabled = false;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = false;
                    kryptonRibbonGroupButtonRedo.Enabled = false;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = true;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = true;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = false;
                    kryptonRibbonGroupButtonHomePaste.Enabled = false;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = false;
                    kryptonRibbonGroupButtonRedo.Enabled = false;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = true;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = true;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    //Home - Clipboard
                    kryptonRibbonGroupButtonHomeCopy.Enabled = true;
                    kryptonRibbonGroupButtonHomeCut.Enabled = false;
                    kryptonRibbonGroupButtonHomePaste.Enabled = false;
                    kryptonRibbonGroupButtonHomeUndo.Enabled = false;
                    kryptonRibbonGroupButtonRedo.Enabled = false;

                    //Home - Fast Copy text
                    kryptonRibbonGroupButtonHomeCopyText.Enabled = false;
                    kryptonRibbonGroupButtonHomeCopyText.TextLine2 = "Text";

                    //Home - FastCopyNoOverwrite / Overwrite
                    kryptonRibbonGroupButtonHomeFastCopyNoOverwrite.Enabled = false;
                    kryptonRibbonGroupButtonHomeFastCopyOverwrite.Enabled = false;

                    //Home - Find and Replace
                    kryptonRibbonGroupButtonHomeFind.Enabled = true;
                    kryptonRibbonGroupButtonHomeReplace.Enabled = true;

                    //Home - FileSystem
                    kryptonRibbonGroupButtonHomeFileSystemDelete.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRename.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpen.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenWith.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemOpenAssociateDialog.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemOpenExplorer.Enabled = false;
                    kryptonRibbonGroupButtonFileSystemRunCommand.Enabled = false;
                    kryptonRibbonGroupButtonHomeFileSystemEdit.Enabled = false;

                    //Home - Rotate
                    kryptonRibbonGroupButtonMediaFileRotate90CCW.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate180.Enabled = false;
                    kryptonRibbonGroupButtonMediaFileRotate90CW.Enabled = false;

                    //Home - Metadata - AutoCorrect
                    kryptonRibbonGroupButtonHomeAutoCorrectRun.Enabled = false;
                    kryptonRibbonGroupButtonHomeAutoCorrectForm.Enabled = false;

                    //Home - Metadata - Refresh/Reload
                    kryptonRibbonGroupButtonHomeMetadataRefresh.Enabled = false;
                    kryptonRibbonGroupButtonHomeMetadataReload.Enabled = false;

                    //Home - Metadata - Tag Select
                    kryptonRibbonGroupButtonHomeTagSelectOn.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectToggle.Enabled = false;
                    kryptonRibbonGroupButtonHomeTagSelectOff.Enabled = false;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
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

        #region KryptonContextMenu - Actions
        private void KryptonContextMenuItemGenericCut_Click(object sender, EventArgs e)
        {
            ActionCut();
        }

        private void KryptonContextMenuItemGenericCopy_Click(object sender, EventArgs e)
        {
            
        }

        private void KryptonContextMenuItemGenericCopyText_Click(object sender, EventArgs e)
        {
            
        }

        private void KryptonContextMenuItemGenericPaste_Click(object sender, EventArgs e)
        {
            
        }

        private void KryptonContextMenuItemGenericDelete_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericRename_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericUndo_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericRedo_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericFind_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericReplace_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericSave_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericFavoriteAdd_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericFavoriteDelete_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemFavoriteToggle_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericRowShowFavorite_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericRowHideEqual_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericTriStateOn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericTriStateOff_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericTriStateToggle_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericMediaViewAsPoster_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KryptonContextMenuItemGenericMediaViewAsFull_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 


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
                    KeywordsCut_Click();
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

        //Convert and Merge
        #region Date
        private void DateCut_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion
        //Exiftool
        //ExiftoolWarning

        #region Map
        private void MapCut_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
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

        //Properties
        //Rename
        
        #region TagsAndKeywords
        private void KeywordsCut_Click()
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
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
        }
        #endregion

        #region Exiftool
        private void toolStripMenuItemExiftoolCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        //Properties
        //Rename

        #region TagsAndKeywords
        private void copyToolStripMenuTagsBrokerCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
        }
        #endregion

        #endregion

        #region Paste
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDatePaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        //Exiftool
        //ExiftoolWarning

        #region Map
        private void toolStripMenuItemMapPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeoplePaste_Click(object sender, EventArgs e)
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

        //Properties
        //Rename
        #region TagsAndKeywords
        private void pasteToolStripMenuTagsBrokerPaste_Click(object sender, EventArgs e)
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

        #region Delete
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 
        //Exiftool
        //ExiftoolWarning

        #region Map
        private void toolStripMenuItemMapDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void deleteToolStripMenuTagsBrokerDelete_Click(object sender, EventArgs e)
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
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.UndoDataGridView(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        //Exiftool
        //ExiftoolWarning
        #region Map
        private void toolStripMenuItemMapUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.UndoDataGridView(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleUndo_Click(object sender, EventArgs e)
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

        //Properties
        //Rename
        #region TagsAndKeywords
        private void undoToolStripMenuTags_Click(object sender, EventArgs e)
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
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        //Exiftool
        //ExiftoolWarning
        #region Map
        private void toolStripMenuItemMapRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleRedo_Click(object sender, EventArgs e)
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

        //Properties
        //Rename
        #region TagsAndKeywords
        private void redoToolStripMenuTags_Click(object sender, EventArgs e)
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
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleFind_Click(object sender, EventArgs e)
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

        //Properties
        //Rename

        #region TagsAndKeywords
        private void findToolStripMenuTag_Click(object sender, EventArgs e)
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

        #region Find and Replace - *** Need debug why not implemented ***
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //ExiftoolWarning
        #region Map
        private void toolStripMenuItemMapReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleReplace_Click(object sender, EventArgs e)
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

        //Properties
        //Rename
        #region TagsAndKeywords
        private void replaceToolStripMenuTag_Click(object sender, EventArgs e)
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


        #region Mark Favorite
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Exiftool
        private void toolStripMenuItemExiftoolMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        //Properties
        //Rename

        #region TagsAndKeywords
        private void markAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #endregion

        #region Remove Favorite
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void removeAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #endregion

        #region Toogle Favorite
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateToggleFavourite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void toggleFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #endregion

        #region Show only Favorite 

        private void UpdateBottonsEqualAndFavorite(bool hideEqualColumns, bool showFavouriteColumns)
        {
            kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Checked = hideEqualColumns;
            kryptonRibbonGroupButtonDataGridViewRowsFavorite.Checked = showFavouriteColumns;

            DataGridView dataGridView = GetActiveTabDataGridView();
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
        }


        #region Date
        private void toolStripMenuItemShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            
            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void showFavoriteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #endregion

        #region Hide Equal Rows
        #region  Date
        private void toolStripMenuItemDateHideEqualRows_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region Exiftool
        private void toolStripMenuItemExiftoolHideEqual_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningHideEqual_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
            DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.HideEqualColumns(dataGridView));
            kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Checked = DataGridViewHandler.HideEqualColumns(dataGridView);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapHideEqual_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
            DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.HideEqualColumns(dataGridView));
            kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Checked = DataGridViewHandler.HideEqualColumns(dataGridView);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleHideEqualRows_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void hideEqualRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
            UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #endregion


        #region TriState Buttons - Toogle and set state
        private void TagActionToggle(DataGridView dataGridView, string header, NewState newState)
        {
            DataGridViewHandler.ToggleSelected(dataGridView, header, newState);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region TriState Buttons - Toogle 
        private void toggleTagSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Toggle);
        }
        private void toolStripMenuItemPeopleTogglePeopleTag_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Toggle);
        }
        #endregion

        #region TriState Buttons - Set
        private void selectTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Set);
        }

        private void toolStripMenuItemPeopleSelectPeopleTag_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Set);
        }
        #endregion

        #region TriState Buttons - Remove
        private void removeTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords,NewState.Remove);
        }

        private void toolStripMenuItemPeopleRemovePeopleTag_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Remove);
        }
        #endregion


        #region Copy text to Media and overwrite
        private void toolStripMenuItemTagsCopyText_Click(object sender, EventArgs e)
        {
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, false);
        }

        private void toolStripMenuItemMapCopyNotOverwrite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridView, DataGridViewHandlerMap.headerMedia, false);
        }
        #endregion 

        #region Copy text to Media and NOT overwrite
        private void toolStripMenuItemTagsOverwriteText_Click(object sender, EventArgs e)
        {
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, true);
        }

        private void toolStripMenuItemMapCopyAndOverwrite_Click(object sender, EventArgs e)
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

        #region DataGridView Keydown

        #region Keydown - Convert And Merge
        private void dataGridViewConvertAndMerge_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Date
        private void dataGridViewDate_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Exiftool
        private void dataGridViewExifTool_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - ExiftoolWarning
        private void dataGridViewExifToolWarningData_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Map
        private void dataGridMap_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - People
        private void dataGridViewPeople_KeyDown(object sender, KeyEventArgs e)
        {
            triStateButtomClick = false;
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Properties
        private void dataGridViewRename_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Rename
        private void dataGridViewProperties_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
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
