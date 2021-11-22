using DataGridViewGeneric;
using Krypton.Toolkit;
using LocationNames;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Exiftool;
using MetadataLibrary;
using Thumbnails;
using Manina.Windows.Forms;
using ApplicationAssociations;
using System.Collections.Specialized;
using ImageAndMovieFileExtentions;
using System.Reflection;
using MetadataPriorityLibrary;
using Raccoom.Windows.Forms;
using FileDateTime;
using System.Threading;
using System.Diagnostics;
using ColumnNamesAndWidth;

/*
Ctrl+X				T	Cut								Home / Organise
Ctrl+C 				C	Copy							Home / Organise
Ctrl+Shift+T		MCT	Copy Text						Home / Organise
Ctrl+O				MCN	Fast Copy No Overwrite			Home / Organise
Ctrl+Shift+O		MCO	Fast Copy With Overwrite		Home / Organise
Ctrl+V				P 	Paste							Home / Organise
Del					OD	Delete file/folder				Home / Organise
F2					ON	Rename file/folder				Home / Organise
Ctrl+Z				U	Undo							Home / Organise
Ctrl+Y				R	Redo							Home / Organise
Ctrl+F				MF	Find							Home / Organise
Ctrl+H				MR	Replace							Home / Organise
Ctrl+S				S	Save							Home / Save
F5					OF	Refresh file list/folder tree	Home / Organise
Ctrl+R 					Add subfolders					**
Ctrl+E				OL	Open Location in Explorer		Home / Organise
Ctrl+Enter			OP	Open							Home / Organise
					OW	Open with...					Home / Organise
Ctrl+Shift+Enter 	OA	Open with associate				Home / Organise
F4					OE	Open for Edit					Home / Organise
F6	 				OR	Run								Home / Organise
F7	 				AR	AutoCorrect Run					Home / Metadata
F8		 			AF	AutoCorrect Form				Home / Metadata
Shift+F11			AP	View media as Poster			Home / Metadata	QAT
Shift+F5 			MER	Refresh Metadata				Home / Metadata
Ctrl+F5 			MEL	Reload Metadata 				Home / Metadata
Ctrl+Shift+Space	MSS	Select tags						Home / Metadata
Ctrl+Del 			MSR	Remove tags						Home / Metadata
Ctrl+Space 			MST	Toggle tags						Home / Metadata
Ctrl+9 				9	Rotate 90						Home / Rotate
Ctrl+1				1	Rotate 180						Home / Rotate
Ctrl+2 				2	Rotate 270						Home / Rotate
Ctrl+D				FM	Mark as Favorite				View / Favorite
Ctrl+Shift+D		FR	Remove as Favorite				View / Favorite
Ctrl+T				FT	Toggle as Favorite				View / Favorite
Ctrl+0		 		RF	Show Favorite rows				View / Rows 
Ctrl+Shift+0		RE	Hide equal rows					View / Rows



F11					PF	View full media					Preview / Preview	QAT 
					PF	Preview							Preview / Preview
					NSP	Skip Preview					Preview / Navigate
					NSN	SKip Next						Preview / Navigate
					NPL	Play							Preview / Navigate
					NPA	Pause							Preview / Navigate
					NR	Rewind							Preview / Navigate
					NF	Forward							Preview / Navigate
					NS	Stop							Preview / Navigate
					S	Slider							Preview / Navigate
					2	270								Preview / Rotate
					1	180								Preview / Rotate
					9	90								Preview / Rotate
					SS	Start slideshow					Preview / Slideshow
					SM	Select Media					Preview / Slideshow
					ST	Time Interval					Preview / Slideshow
					C	Chromecast						Preview / Slideshow
						
Ctrl+M					Show Coordinate on OpenStreetView
Ctrl+Shift+M			Show Coordinate on Google Maps
Ctrl+L 					Reload Location information using Nominatim

Ctrl+Alt+Left		SB	Select Previous group			Select / Select Group	QAT
Ctrl+Alt+Right		SF	Select next group				Select / Select Group	QAT
Ctrl+Shift+I		SM	Select all match				Select / Select Group	QAT
Ctrl+A  			SA	Select all 						Select / Select Group	QAT
Ctrl+Shift+A 		SN	Select None						Select / Select Group	QAT
Ctrl+I				SI	Invert Select					Select / Select Group 	QAT
					DD	Date							Select / Date range
					DT	Media taken						Select / Date range
					DA	Match all						Select / Date range
					D1	1 day							Select / Date range
					D3	3 days							Select / Date range
					DW	Week							Select / Date range
					D2	2 weeks							Select / Date range
					DM	Month 							Select / Date range
					LN	Location name					Select / Location
					LI	City							Select / Location
					LS	State/Region					Select / Location
					LC	Country							Select / Location
					LM	Match all 						Select / Location
					
					I	Import Locations				Tools / Import
Ctrl+Q				L	Location Analytics				Tools / Import
					W	Web Scraping					Tools / Import
						
					C	Config							Tools / Help
					A	About							Tools / Help
						
					T	Task list						Tools / Status 
*/

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
        #endregion

        #region Ribbons - WorkspaceChanged - Enable / Disable ribbons buttons

        private void RibbonsQTAVisiable(bool saveVisible = true, bool mediaSelectVisible = true, bool mediaPlayerVisible = false)
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
            kryptonRibbonGroupButtonHomeFileSystemOpenAssociateDialog.Enabled = enabledOpenWithEdit;
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

        private void RibbonGroupButtonHomeMetadata(bool enabledAutoCorrect = false, bool enabledDeleteHistoryRefresh = false, bool enabledTriState = false, bool enablePreviewPoster = false)
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
        #endregion

        #region UpdateRibbonsWhenWorkspaceChanged()
        private void UpdateRibbonsWhenWorkspaceChanged()
        {
            bool isSomethingSelected = (GetSelectedFilesImageListView().Count >= 1);
            bool isMoreThatOneSelected = (GetSelectedFilesImageListView().Count > 1);

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
                    break;
                default:
                    throw new NotImplementedException();
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
        private void ContextMenuGenericAssignCompositeTag (bool visible)
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

        // -----------------------------------------------------------------------
        #region Cut

        #region Cut - Click Events Sources       
        private void kryptonRibbonGroupButtonHomeCut_Click(object sender, EventArgs e)
        {
            ActionCut();
        }

        private void KryptonContextMenuItemGenericCut_Click(object sender, EventArgs e)
        {
            ActionCut();
        }
        #endregion

        #region ActionCut()
        private void ActionCut()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderCut_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesCut_Click();
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

        #region MediaFilesCut_Click
        private void MediaFilesCut_Click()
        {
            try
            {
                var droplist = new StringCollection();
                using (new WaitCursor())
                {
                    foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FileFullPath);
                    SetDropDropFileList(droplist);
                }
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderCut_Click
        private void FolderCut_Click()
        {
            try
            {
                string folder = GetNodeFolderPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
                if (!Directory.Exists (folder))
                {
                    KryptonMessageBox.Show("Not a valid folder selected. Try select anoter folder.\r\nSelected system folder: " + (string.IsNullOrWhiteSpace(folder) ? "Unknown" : folder));
                    return;
                }

                using (new WaitCursor())
                {
                    SetDropDropFileList(folder);
                }
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region Copy - Click Events Sources
        private void kryptonRibbonGroupButtonHomeCopy_Click(object sender, EventArgs e)
        {
            ActionCopy();
        }

        private void KryptonContextMenuItemGenericCopy_Click(object sender, EventArgs e)
        {
            ActionCopy();
        }
        #endregion

        #region ActionCopy()
        private void ActionCopy()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderCopy_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesCopy_Click();
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

        #region MediaFilesCopy_Click
        private void MediaFilesCopy_Click()
        {
            try
            {
                StringCollection droplist = new StringCollection();
                using (new WaitCursor())
                {
                    foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FileFullPath);
                    SetDropDropFileList(droplist);
                }
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderTree - Copy - Click
        private void FolderCopy_Click()
        {
            try
            {
                string folder = GetNodeFolderPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
                if (!Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Not a valid folder selected. Try select anoter folder.\r\nSelected system folder: " + (string.IsNullOrWhiteSpace(folder) ? "Unknown" : folder));
                    return;
                }

                using (new WaitCursor())
                {
                    SetDropDropFileList(folder);
                    
                }
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region Paste - Click Events Sources
        private void kryptonRibbonGroupButtonHomePaste_Click(object sender, EventArgs e)
        {
            ActionPaste();
        }
        private void KryptonContextMenuItemGenericPaste_Click(object sender, EventArgs e)
        {
            ActionPaste();
        }
        #endregion 

        #region ActionPaste
        private void ActionPaste()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderPaste_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesPaste_Click();
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

        #region MediaFilesPaste_Click
        private void MediaFilesPaste_Click()
        {
            try
            {
                StringCollection files = Clipboard.GetFileDropList();
                using (new WaitCursor())
                {
                    foreach (string fullFilename in files)
                    {
                        bool fileFound = false;


                        foreach (ImageListViewItem item in imageListView1.Items)
                        {
                            if (item.FileFullPath == fullFilename)
                            {
                                fileFound = true;
                                break;
                            }
                        }

                        if (!fileFound) imageListView1.Items.Add(fullFilename);
                    }
                }
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderPaste_Click
        private void FolderPaste_Click()
        {
            try
            {
                DragDropEffects dragDropEffects = DetectCopyOrMove();
                using (new WaitCursor())
                {
                    TreeNode targetTreeNode = treeViewFolderBrowser1.SelectedNode;
                    string targetFolder = GetNodeFolderPath(targetTreeNode as TreeNodePath);

                    if (!Directory.Exists(targetFolder))
                    {
                        KryptonMessageBox.Show("Not a valid target folder selected. Try select anoter folder.\r\nSelected system folder: " + (string.IsNullOrWhiteSpace(targetFolder) ? "Unknown" : targetFolder));
                        return;
                    }

                    StringCollection filesOrFolders = Clipboard.GetFileDropList();
                    CopyOrMove_UpdatedBrowserTreeView(dragDropEffects, filesOrFolders, targetFolder, treeViewFolderBrowser1.SelectedNode);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (controlPasteWithFocusRename?.Name == textBoxRenameNewName.Name)
            {
                textBoxRenameNewName.Paste();
            }
            if (controlPasteWithFocusRename?.Name == dataGridViewRename.Name)
            { 
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericPaste(dataGridView);
            }
        }
        #endregion

        #region TagsAndKeywords
        private void TagsAndKeywordsPaste_Click()
        {
            if (controlPasteWithFocusTag?.Name == comboBoxAlbum.Name ||
                controlPasteWithFocusTag?.Name == comboBoxTitle.Name ||
                controlPasteWithFocusTag?.Name == comboBoxDescription.Name ||
                controlPasteWithFocusTag?.Name == comboBoxComments.Name ||
                controlPasteWithFocusTag?.Name == comboBoxAuthor.Name
                )
            {
                KryptonComboBox kryptonComboBox = (KryptonComboBox)controlPasteWithFocusTag;
                string insertText = Clipboard.GetText();
                int selectionStart = kryptonComboBox.SelectionStart;
                kryptonComboBox.Text = kryptonComboBox.Text.Remove(selectionStart, kryptonComboBox.SelectionLength);
                kryptonComboBox.Text = kryptonComboBox.Text.Insert(selectionStart, insertText);
                kryptonComboBox.SelectionStart = selectionStart + insertText.Length;
            }

            if (controlPasteWithFocusTag?.Name == dataGridViewTagsAndKeywords.Name)
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
        }
        #endregion

        #endregion
        
        #region Delete

        #region Delete - Click Events Sources
        private void kryptonRibbonGroupButtonHomeFileSystemDelete_Click(object sender, EventArgs e)
        {
            ActionGridCellAndFileSystemDelete();
        }

        private void KryptonContextMenuItemGenericFileSystemDelete_Click(object sender, EventArgs e)
        {
            ActionGridCellAndFileSystemDelete();
        }
        #endregion 

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
                if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

                treeViewFolderBrowser1.Enabled = false;
                imageListView1.Enabled = false;

                try
                {
                    if (IsFileInThreadQueueLock(imageListView1))
                    {
                        KryptonMessageBox.Show("Can't delete files. Files are being used, you need wait until process is finished.");
                        return;
                    }

                    if (MessageBox.Show("Are you sure you will delete the files", "Files will be deleted!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (new WaitCursor())
                        {
                            UpdateStatusAction("Deleing files and all record about files in database....");
                            filesCutCopyPasteDrag.DeleteSelectedFiles(this, imageListView1);
                            FilesSelectedOrNoneSelected();
                        }
                    }
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message);
                }

                treeViewFolderBrowser1.Enabled = true;
                imageListView1.Enabled = true;
                imageListView1.Focus();
                DisplayAllQueueStatus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region FilSystemFolderDelete_Click
        private void FilSystemFolderDelete_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                string folder = GetSelectedNodePath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't delete folder. No valid folder selected.");
                    return;
                }

                if (IsFolderInThreadQueueLock(folder))
                {
                    KryptonMessageBox.Show("Can't delete folder. Files in folder is been used, you need wait until process is finished.");
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
                            int recordAffected = filesCutCopyPasteDrag.DeleteFilesInFolder(this, treeViewFolderBrowser1, folder);
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
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #region Undo - Click Events Sources
        private void kryptonRibbonGroupButtonHomeUndo_Click(object sender, EventArgs e)
        {
            ActionUndo();
        }

        private void KryptonContextMenuItemGenericUndo_Click(object sender, EventArgs e)
        {
            ActionUndo();
        }
        #endregion 

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

        #region Redo - Click Events Sources
        private void kryptonRibbonGroupButtonHomeRedo_Click(object sender, EventArgs e)
        {
            ActionRedo();
        }
        private void KryptonContextMenuItemGenericRedo_Click(object sender, EventArgs e)
        {
            ActionRedo();
        }
        #endregion

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

        #region Find - Click Events Sources
        private void kryptonRibbonGroupButtonHomeFind_Click(object sender, EventArgs e)
        {
            ActionFind();
        }
        private void KryptonContextMenuItemGenericFind_Click(object sender, EventArgs e)
        {
            ActionFind();
        }
        #endregion 

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

        #region FolderSearchFilterFolderFind_Click
        private void FolderSearchFilterFolderFind_Click()
        {
            kryptonWorkspaceCellFolderSearchFilter.SelectedPage = kryptonPageFolderSearchFilterSearch;
            kryptonTextBoxSearchDirectory.Text = GetSelectedNodePath() == null ? "" : GetSelectedNodePath();
        }
        #endregion

        #region FolderSearchFilterSearchFind_Click
        private void FolderSearchFilterSearchFind_Click()
        {
            buttonFilterSearch_Click();
        }
        #endregion

        #region MediaFilesFind_Click
        private void MediaFilesFind_Click()
        {
            kryptonWorkspaceCellFolderSearchFilter.SelectedPage = kryptonPageFolderSearchFilterSearch;
            kryptonTextBoxSearchDirectory.Text = GetSelectedNodePath() == null ? "" : GetSelectedNodePath();
            if (imageListView1.SelectedItems.Count == 1) kryptonTextBoxSearchFilename.Text = imageListView1.SelectedItems[0].Text;
        }
        #endregion

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

        #region FindAndReplace - Click Events Sources
        private void kryptonRibbonGroupButtonHomeReplace_Click(object sender, EventArgs e)
        {
            ActionFindAndReplace();
        }
        private void KryptonContextMenuItemGenericReplace_Click(object sender, EventArgs e)
        {
            ActionFindAndReplace();
        }
        #endregion

        #region ActionFindAndReplace
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
        
        #region Favorite Add

        #region Favorite Add - Click Events Sources
        private void KryptonContextMenuItemGenericFavoriteAdd_Click(object sender, EventArgs e) //---------------------------------------
        {
            ActionFavoriteAdd();
        }
        #endregion 

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
        
        #region Favorite Remove 

        #region Favorite Remove - Click Events Sources
        private void KryptonContextMenuItemGenericFavoriteDelete_Click(object sender, EventArgs e)
        {
            ActionFavoriteDelete();
        }
        #endregion 

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
        
        #region Favorite Toogle 

        #region FavoriteToggle - Click Events Sources
        private void KryptonContextMenuItemFavoriteToggle_Click(object sender, EventArgs e)
        {
            ActionFavoriteToggle();
        }
        #endregion 

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

        #region RowsShowFavoriteToggle - Click Events Sources
        private void kryptonRibbonGroupButtonDataGridViewRowsFavorite_Click(object sender, EventArgs e)
        {
            ActionRowsShowFavoriteToggle();
        }

        private void KryptonContextMenuItemGenericRowShowFavorite_Click(object sender, EventArgs e)
        {
            ActionRowsShowFavoriteToggle();
        }
        #endregion

        #region UpdateBottonsEqualAndFavorite
        private void UpdateBottonsEqualAndFavorite(bool hideEqualColumns, bool showFavouriteColumns)
        {
            kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Checked = hideEqualColumns;
            kryptonRibbonGroupButtonDataGridViewRowsFavorite.Checked = showFavouriteColumns;
            DataGridView dataGridView = GetActiveTabDataGridView();
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

        #region RowsHideEqualToggle - Click Events Sources
        private void KryptonContextMenuItemGenericRowHideEqual_Click(object sender, EventArgs e)
        {
            ActionRowsHideEqualToggle();
        }

        private void kryptonRibbonGroupButtonDataGridViewRowsHideEqual_Click(object sender, EventArgs e)
        {
            ActionRowsHideEqualToggle();
        }
        #endregion

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
        
        #region CopyText

        #region CopyText - Click Events Sources
        private void kryptonRibbonGroupButtonHomeCopyText_Click(object sender, EventArgs e)
        {
            ActionCopyText();
        }

        private void KryptonContextMenuItemGenericCopyText_Click(object sender, EventArgs e)
        {
            ActionCopyText();
        }
        #endregion

        #region ActionCopyText
        private void ActionCopyText()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderCopyNameToClipboard_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesCopyNameToClipboard_Click();
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

        #region FolderCopyFolderNameToClipboard_Click
        private void FolderCopyNameToClipboard_Click()
        {
            try
            {
                string folder = GetSelectedNodePath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't copy folder name. Not a valid folder selected.");
                    return;
                }
                Clipboard.SetText(folder);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region MediaFilesCopyNameToClipboard_Click
        private void MediaFilesCopyNameToClipboard_Click()
        {
            try
            {
                if (imageListView1.SelectedItems.Count > 0)
                {
                    string filenames = "";
                    foreach (ImageListViewItem item in imageListView1.SelectedItems) filenames += (string.IsNullOrWhiteSpace(filenames) ? "" : "\r\n") + item.FileFullPath;
                    Clipboard.SetText(filenames);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region TriStateToggle

        #region TriStateToggle - Click Events Sources
        private void kryptonRibbonGroupButtonHomeTriStateToggle_Click(object sender, EventArgs e)
        {
            ActionTriStateToggle();
        }

        private void KryptonContextMenuItemGenericTriStateToggle_Click(object sender, EventArgs e)
        {
            ActionTriStateToggle();
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

        #region DataGridViewGenericTagActionToggle
        private void DataGridViewGenericTagActionToggle(DataGridView dataGridView, string header, NewState newState)
        {
            if (!dataGridView.Enabled) return;
            DataGridViewHandler.ToggleSelected(dataGridView, header, newState);
            ValitedatePasteKeywords(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
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
        
        #region TriStateOn

        #region TriStateOn - Click Events Sources
        private void kryptonRibbonGroupButtonHomeTriStateOn_Click(object sender, EventArgs e)
        {
            ActionTriStateOn();
        }
        private void KryptonContextMenuItemGenericTriStateOn_Click(object sender, EventArgs e)
        {
            ActionTriStateOn();
        }
        #endregion

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
        
        #region TriStateOff

        #region TriStateOff - Click Events Sources
        private void kryptonRibbonGroupButtonHomeTriStateOff_Click(object sender, EventArgs e)
        {
            ActionTriStateOff();
        }
        private void KryptonContextMenuItemGenericTriStateOff_Click(object sender, EventArgs e)
        {
            ActionTriStateOff();
        }
        #endregion

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

        #region TagsAndKeywordsTriStateOff_Click
        private void TagsAndKeywordsTriStateOff_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Remove);
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

        

        #region Save and AutoCorrect
        private void kryptonRibbonGroupButtonHomeSaveAutoCorrectAndSave_Click(object sender, EventArgs e)
        {
            ActionSave(true);
        }
        #endregion

        #region Save

        #region Save - Click Events Sources
        private void kryptonRibbonQATButtonSave_Click(object sender, EventArgs e)
        {
            ActionSave(false);
        }

        private void kryptonRibbonGroupButtonHomeSaveSave_Click(object sender, EventArgs e)
        {
            ActionSave(false);
        }

        private void KryptonContextMenuItemGenericSave_Click(object sender, EventArgs e)
        {
            ActionSave(false);
        }
        #endregion 

        #region ActionSave
        private void ActionSave(bool useAutoCorrect)
        {
            try
            {
                this.Activate();
                this.Validate(); //Get the latest changes, that are text in edit mode

                if (GlobalData.IsPopulatingAnything()) return;
                if (GlobalData.IsSaveButtonPushed) return;
                GlobalData.IsSaveButtonPushed = true;
                this.Enabled = false;
                using (new WaitCursor())
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
                            SaveDataGridViewMetadata(useAutoCorrect);
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
                            SaveRename();
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
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region bool SaveBeforeContinue
        private DialogResult SaveBeforeContinue(bool canCancel)
        {
            DialogResult dialogResult = DialogResult.No;
            if (IsAnyDataUnsaved())
            {       
                
                dialogResult = KryptonMessageBox.Show(
                    "Do you want to save and contine.\r\n" +
                    "Yes - Save without AutoCorrect and continue\r\n" +
                    "No - Don't save and continue without save." +
                    (canCancel ? "\r\nCancel - Cancel the opeation and continue where you left." : ""), 
                    "Warning, unsaved data", 
                    (canCancel ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.YesNo), MessageBoxIcon.Warning);
                
                if (dialogResult == DialogResult.Yes)
                {
                    ActionSave(false);
                }
            }
            return dialogResult;
        }
        #endregion

        #region Save - IsAnyDataUnsaved
        private bool IsAnyDataUnsaved()
        {
            if (isClosing) return false;

            bool isAnyDataUnsaved = false;
            if (GlobalData.IsAgregatedTags) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewTagsAndKeywords);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;
            if (GlobalData.IsAgregatedMap) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewMap);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;
            if (GlobalData.IsAgregatedPeople) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewPeople);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;
            if (GlobalData.IsAgregatedDate) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewDate);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;
            if (GlobalData.IsAgregatedProperties) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewProperties);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;

            GetDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView);

            //Find what columns are updated / changed by user
            List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
            return (listOfUpdates.Count > 0);
        }
        #endregion

        #region Save - ClearDataGridDirtyFlag
        private void ClearDataGridDirtyFlag()
        {
            if (GlobalData.IsAgregatedTags) DataGridViewHandler.ClearDataGridViewDirty(dataGridViewTagsAndKeywords);
            if (GlobalData.IsAgregatedMap) DataGridViewHandler.ClearDataGridViewDirty(dataGridViewMap);
            if (GlobalData.IsAgregatedPeople) DataGridViewHandler.ClearDataGridViewDirty(dataGridViewPeople);
            if (GlobalData.IsAgregatedDate) DataGridViewHandler.ClearDataGridViewDirty(dataGridViewDate);
        }
        #endregion

        #region GetDataGridViewData - FileEntry
        private void UpdateMetadataFromDataGridView (FileEntryAttribute fileEntryAttribute, ref Metadata metadataFromDataGridView)
        {
            if (GlobalData.IsAgregatedTags) DataGridViewHandlerTagsAndKeywords.GetUserInputChanges(ref dataGridViewTagsAndKeywords, metadataFromDataGridView, fileEntryAttribute);
            if (GlobalData.IsAgregatedMap) DataGridViewHandlerMap.GetUserInputChanges(ref dataGridViewMap, metadataFromDataGridView, fileEntryAttribute);
            if (GlobalData.IsAgregatedPeople) DataGridViewHandlerPeople.GetUserInputChanges(ref dataGridViewPeople, metadataFromDataGridView, fileEntryAttribute);
            if (GlobalData.IsAgregatedDate) DataGridViewHandlerDate.GetUserInputChanges(ref dataGridViewDate, metadataFromDataGridView, fileEntryAttribute);
        }
        #endregion 

        #region GetDataGridViewData - All
        private void GetDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView)
        {
            metadataListOriginalExiftool = new List<Metadata>();
            metadataListFromDataGridView = new List<Metadata>();

            DataGridView dataGridView = GetActiveTabDataGridView();
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

                    UpdateMetadataFromDataGridView(dataGridViewGenericColumn.FileEntryAttribute, ref metadataFromDataGridView);

                    metadataListOriginalExiftool.Add(new Metadata(dataGridViewGenericColumn.Metadata));
                    metadataListFromDataGridView.Add(new Metadata(metadataFromDataGridView));
                } 
            }
        }
        #endregion

        #region Save - SaveDataGridViewMetadata
        private void SaveDataGridViewMetadata(bool useAutoCorrect)
        {
            if (GlobalData.IsPopulatingAnything())
            {
                KryptonMessageBox.Show("Data is populating, please try a bit later.");
                return;
            }
            if (!GlobalData.IsAgredagedGridViewAny())
            {
                KryptonMessageBox.Show("No metadata are updated.");
                return;
            }

            GetDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView);

            //Find what columns are updated / changed by user
            List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
            if (listOfUpdates.Count == 0)
            {
                KryptonMessageBox.Show("Can't find any value that was changed. Nothing is saved...");
                return;
            }

            ClearDataGridDirtyFlag(); //Clear before save; To track if become dirty during save process
            GlobalData.ListOfAutoCorrectFilesClear();

            foreach (int updatedRecord in listOfUpdates)
            {
                if (useAutoCorrect)
                {
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    Metadata metadataToSave = autoCorrect.FixAndSave(
                        metadataListFromDataGridView[updatedRecord].FileEntry,
                        metadataListFromDataGridView[updatedRecord],
                        databaseAndCacheMetadataExiftool,
                        databaseAndCacheMetadataMicrosoftPhotos,
                        databaseAndCacheMetadataWindowsLivePhotoGallery,
                        databaseAndCahceCameraOwner,
                        databaseLocationAddress,
                        databaseGoogleLocationHistory,
                        locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                        autoKeywordConvertions,
                        Properties.Settings.Default.RenameDateFormats);
                    if (metadataToSave != null)
                    {
                        AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, metadataListOriginalExiftool[updatedRecord]);
                    }

                }
                else
                {
                    //Add only metadata to save queue that that has changed by users
                    AddQueueSaveMetadataUpdatedByUserLock(metadataListFromDataGridView[updatedRecord], metadataListOriginalExiftool[updatedRecord]);
                }
            }
            ThreadSaveMetadata();
        }
        #endregion

        #region Save - SaveProperties
        private void SaveProperties()
        {
            using (new WaitCursor())
            {
                DataGridView dataGridView = dataGridViewProperties;
                int columnCount = DataGridViewHandler.GetColumnCount(dataGridView);
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    if (dataGridViewGenericColumn != null)
                    {
                        try
                        {
                            DataGridViewHandlerProperties.Write(dataGridView, columnIndex);
                        }
                        catch (Exception ex)
                        {
                            string writeErrorDesciption =
                                "Error writing properties to file.\r\n\r\n" +
                                "File: " + dataGridViewGenericColumn.FileEntryAttribute.FileFullPath + "\r\n\r\n" +
                                "Error message: " + ex.Message + "\r\n";

                            AddError(
                                dataGridViewGenericColumn.FileEntryAttribute.Directory,
                                dataGridViewGenericColumn.FileEntryAttribute.FileName,
                                dataGridViewGenericColumn.FileEntryAttribute.LastWriteDateTime,
                                AddErrorPropertiesRegion, AddErrorPropertiesCommandWrite, AddErrorPropertiesParameterWrite, AddErrorPropertiesParameterWrite,
                                writeErrorDesciption);
                            Logger.Error(ex, "SaveProperties");
                        }
                    }
                }

                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
                //ImageListViewReloadThumbnailInvoke(imageListView1, null); //Why null
                LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(GetSelectedFileEntriesImageListView());
                FilesSelectedOrNoneSelected();
            }
        }
        #endregion

        #region Save - SaveConvertAndMerge
        private void SaveConvertAndMerge()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.InitialDirectory = @"C:\";      
            saveFileDialog1.Title = "Where to save converted and merged video file";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "mp4";
            saveFileDialog1.Filter = "Video file (*.mp4)|*.mp4|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (new WaitCursor())
                {
                    string outputFile = saveFileDialog1.FileName;

                    DataGridView dataGridView = dataGridViewConvertAndMerge;
                    DataGridViewHandlerConvertAndMerge.Write(dataGridView,
                        Properties.Settings.Default.ConvertAndMergeExecute,
                        Properties.Settings.Default.ConvertAndMergeMusic,
                        (int)Properties.Settings.Default.ConvertAndMergeImageDuration,
                        (int)Properties.Settings.Default.ConvertAndMergeOutputWidth,
                        (int)Properties.Settings.Default.ConvertAndMergeOutputHeight,
                        Properties.Settings.Default.ConvertAndMergeOutputTempfileExtension,

                        Properties.Settings.Default.ConvertAndMergeConcatVideosArguments,
                        Properties.Settings.Default.ConvertAndMergeConcatVideosArguFile,

                        Properties.Settings.Default.ConvertAndMergeConcatImagesArguments,
                        Properties.Settings.Default.ConvertAndMergeConcatImagesArguFile,

                        Properties.Settings.Default.ConvertAndMergeConvertVideosArguments,
                        outputFile);

                    GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                    bool found = false;
                    try
                    {
                        if (File.Exists(outputFile) && new FileInfo(outputFile).Length > 0) found = true;
                    }
                    catch
                    {
                    }
                    if (found) ImageListViewAddItem(outputFile);
                }
            }


        }
        #endregion

        #endregion
        
        #region FastCopyNoOverwrite

        #region FastCopyNoOverwrite - Click Events Sources
        private void kryptonRibbonGroupButtonHomeFastCopyNoOverwrite_Click(object sender, EventArgs e)
        {
            ActionFastCopyNoOverwrite();
        }

        private void KryptonContextMenuItemGenericFastCopyNoOverwrite_Click(object sender, EventArgs e)
        {
            ActionFastCopyNoOverwrite();
        }
        #endregion 

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
        
        #region FastCopyOverwrite

        #region FastCopyOverwrite - Click Events Sources
        private void kryptonRibbonGroupButtonHomeFastCopyOverwrite_Click(object sender, EventArgs e)
        {
            ActionFastCopyOverwrite();
        }

        private void KryptonContextMenuItemGenericFastCopyWithOverwrite_Click(object sender, EventArgs e)
        {
            ActionFastCopyOverwrite();
        }
        #endregion

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

        #region Rename

        #region Rename - Click Events Sources
        private void kryptonRibbonGroupButtonHomeFileSystemRename_Click(object sender, EventArgs e)
        {
            ActionFileSystemRename();
        }
        private void KryptonContextMenuItemGenericFileSystemRename_Click(object sender, EventArgs e)
        {
            ActionFileSystemRename();
        }
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

        #region CellRename
        private void CellRename()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            dataGridView.BeginEdit(true);
        }
        #endregion 

        #region MediafilesRename
        private void MediafilesRename()
        {
            kryptonWorkspaceCellToolbox.SelectedPage = kryptonPageToolboxRename;
        }
        #endregion 

        #region FolderRename
        private void FolderRename()
        {
            treeViewFolderBrowser1.SelectedNode.BeginEdit();
        }
        #endregion

        #endregion
        
        #region Rotate270

        #region ActionRotate270
        private void kryptonRibbonGroupButtonMediaFileRotate90CCW_Click(object sender, EventArgs e)
        {
            ActionRotate270();
        }
        private void KryptonContextMenuItemGenericRotate270_Click(object sender, EventArgs e)
        {
            ActionRotate270();
        }
        #endregion

        #region ActionRotate270
        private void ActionRotate270()
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
                    MediaRotate270_Click();
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
        #endregion

        #region MediaRotate270_Click
        private void MediaRotate270_Click()
        {
            try
            {
                RotateInit(imageListView1, 270);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion
        
        #region Rotate180

        #region Rotate180 - Click Events Sources
        private void KryptonContextMenuItemGenericRotate180_Click(object sender, EventArgs e)
        {
            ActionRotate180();
        }

        private void kryptonRibbonGroupButtonMediaFileRotate180_Click(object sender, EventArgs e)
        {
            ActionRotate180();
        }

        #endregion

        #region ActionRotate180
        private void ActionRotate180()
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
                    MediaRotate180_Click();
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
        #endregion

        #region MediaRotate180_Click
        private void MediaRotate180_Click()
        {
            try
            {
                RotateInit(imageListView1, 180);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion
        
        #region Rotate90

        #region Rotate90 - Click Events Sources
        private void kryptonRibbonGroupButtonMediaFileRotate90CW_Click(object sender, EventArgs e)
        {
            ActionRotate90();
        }
        private void KryptonContextMenuItemGenericRotate90_Click(object sender, EventArgs e)
        {
            ActionRotate90();
        }
        #endregion

        #region ActionRotate90
        private void ActionRotate90()
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
                    MediaRotate90_Click();
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
        #endregion

        #region MediaRotate90_Click
        private void MediaRotate90_Click()
        {
            try
            {
                RotateInit(imageListView1, 90);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion
        
        #region MediaViewAsPoster

        #region MediaViewAsPoster - Click Events Sources
        private void KryptonContextMenuItemGenericMediaViewAsPoster_Click(object sender, EventArgs e)
        {
            ActionMediaViewAsPoster();
        }

        private void kryptonRibbonQATButtonMediaPoster_Click(object sender, EventArgs e)
        {
            ActionMediaViewAsPoster();
        }

        private void kryptonRibbonGroupButtonDatGridShowPoster_Click(object sender, EventArgs e)
        {
            ActionMediaViewAsPoster();
        }
        private void kryptonRibbonGroupButtonPreviewPoster_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region ActionMediaViewAsPoster
        private void ActionMediaViewAsPoster()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    TagsAndKeywordsMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ExiftoolWarningMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameMediaViewAsPoster_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeMediaViewAsPoster_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region MediaViewAsPoster_Click
        private void GenericMediaViewAsPoster_Click(DataGridView dataGridView)
        {
            try
            {
                OpenRegionSelector();
                RegionSelectorLoadAndSelect(dataGridView);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region FolderMediaViewAsPoster_Click
        private void FolderMediaViewAsPoster_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaViewAsPoster_Click(dataGridView);
        }
        #endregion 

        #region MediaFilesMediaViewAsPoster_Click
        private void MediaFilesMediaViewAsPoster_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaViewAsPoster_Click(dataGridView);
        }
        #endregion 

        #region DateMediaViewAsPoster_Click
        private void DateMediaViewAsPoster_Click()
        {
            DataGridView dataGridView = dataGridViewDate;
            GenericMediaViewAsPoster_Click(dataGridView);
        }
        #endregion

        #region ExiftoolMediaViewAsPoster_Click
        private void ExiftoolMediaViewAsPoster_Click()
        {
            DataGridView dataGridView = dataGridViewExiftool;
            GenericMediaViewAsPoster_Click(dataGridView);
        }
        #endregion

        #region ExiftoolWarningMediaViewAsPoster_Click
        private void ExiftoolWarningMediaViewAsPoster_Click()
        {
            DataGridView dataGridView = dataGridViewExiftoolWarning;
            GenericMediaViewAsPoster_Click(dataGridView);
        }
        #endregion

        #region MapMediaViewAsPoster_Click
        private void MapMediaViewAsPoster_Click()
        {
            DataGridView dataGridView = dataGridViewMap;
            GenericMediaViewAsPoster_Click(dataGridView);
        }
        #endregion

        #region PeopleMediaViewAsPoster_Click
        private void PeopleMediaViewAsPoster_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            GenericMediaViewAsPoster_Click(dataGridView);
        }
        #endregion

        #region TagsAndKeywordsMediaViewAsPoster_Click
        private void TagsAndKeywordsMediaViewAsPoster_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            GenericMediaViewAsPoster_Click(dataGridView);
        }
        #endregion

        #region PropertiesMediaViewAsPoster_Click
        private void PropertiesMediaViewAsPoster_Click()
        {
            DataGridView dataGridView = dataGridViewProperties;
            GenericMediaViewAsPoster_Click(dataGridView);
        }
        #endregion

        #region RenameMediaViewAsPoster_Click
        private void RenameMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                OpenRegionSelector();
                RegionSelectorLoadAndSelect(dataGridView, dataGridView.CurrentCell.RowIndex, dataGridView.CurrentCell.ColumnIndex);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ConvertAndMergeMediaViewAsPoster_Click
        private void ConvertAndMergeMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                OpenRegionSelector();
                RegionSelectorLoadAndSelect(dataGridView, dataGridView.CurrentCell.RowIndex, dataGridView.CurrentCell.ColumnIndex);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message);
            }
        }
        #endregion

        #endregion
        
        #region MediaViewAsFull

        #region MediaViewAsFull - Click Events Sources
        private void KryptonContextMenuItemGenericMediaViewAsFull_Click(object sender, EventArgs e)
        {
            ActionMediaViewAsFull();
        }

        private void kryptonRibbonQATButtonMediaPreview_Click(object sender, EventArgs e)
        {
            ActionMediaViewAsFull();
        }
        #endregion 

        #region ActionMediaViewAsFull
        private void ActionMediaViewAsFull()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderMediaViewAsFull_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaviewHoveredItemMediaViewAsFull_Click();
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    TagsAndKeywordMediaPreview_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    PeopleMediaPreview_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MapMediaPreview_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    DateMediaPreview_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ExiftoolMediaPreview_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    WarningsMediaPreview_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    PropertiesMediaPreview_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    RenameMediaPreview_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    ConvertAndMergeMediaPreview_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region Media Preview - imageListView1_ItemHover
        private static string imageListViewHoverItem = "";
        private void imageListView1_ItemHover(object sender, ItemHoverEventArgs e)
        {
            try
            {
                if (e.Item != null) imageListViewHoverItem = e.Item.FileFullPath;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region  GenericMediaPreviewFoldeOrMediaList
        private void GenericMediaPreviewFoldeOrMediaList(string selectedMediaFilePullPath)
        {
            List<string> listOfMediaFiles = new List<string>();
            for (int itemIndex = 0; itemIndex < imageListView1.SelectedItems.Count; itemIndex++) listOfMediaFiles.Add(imageListView1.SelectedItems[itemIndex].FileFullPath);
            MediaPreviewInit(listOfMediaFiles, selectedMediaFilePullPath);
        }
        #endregion

        #region PreviewPreviewOpen / Close


        #region ActionPreviewPreviewOpen
        private void ActionPreviewPreviewOpen()
        {
            try
            {
                //SetPreviewRibbonEnabledStatus(previewStartEnabled: true, enabled: true);
                //SetPreviewRibbonPreviewButtonChecked(true);
                if (imageListView1.SelectedItems.Count > 1) GenericMediaPreviewFoldeOrMediaList("");
                else
                {
                    List<string> listOfMediaFiles = new List<string>();
                    for (int itemIndex = 0; itemIndex < imageListView1.Items.Count; itemIndex++) listOfMediaFiles.Add(imageListView1.Items[itemIndex].FileFullPath);
                    string selectedMediaFilePullPath = "";
                    if (imageListView1.SelectedItems.Count == 1) selectedMediaFilePullPath = imageListView1.SelectedItems[0].FileFullPath;
                    MediaPreviewInit(listOfMediaFiles, selectedMediaFilePullPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ActionPreviewPreviewClose
        private void ActionPreviewPreviewClose()
        {
            SetPreviewRibbonEnabledStatus(previewStartEnabled: true, enabled: false);
            SetPreviewRibbonPreviewButtonChecked(false);
            timerFindGoogleCast.Stop();
            ActionPreviewStop();
            panelMediaPreview.Visible = false;
        }
        #endregion

        #region PreviewPreviewOpenClose - Click
        private void kryptonRibbonGroupButtonPreviewPreview_Click(object sender, EventArgs e)
        {
            if (kryptonRibbonGroupButtonPreviewPreview.Checked) ActionPreviewPreviewOpen(); else ActionPreviewPreviewClose();
        }

        private void FolderMediaViewAsFull_Click()
        {
            ActionPreviewPreviewOpen();
        }

        private string lastSelectedTab = "";
        private void kryptonRibbonMain_SelectedTabChanged(object sender, EventArgs e)
        {
            if (kryptonRibbonMain.SelectedTab != null) lastSelectedTab = kryptonRibbonMain.SelectedTab.Text;
    
            if (lastSelectedTab == kryptonRibbonTabPreview.Text)
            {
                RibbonsQTAVisiable(saveVisible: false, mediaSelectVisible: false, mediaPlayerVisible: true);
            }
            else
            {
                RibbonsQTAVisiable(saveVisible: true, mediaSelectVisible: true, mediaPlayerVisible: false);
                ActionPreviewPreviewClose();
            }
        }
        #endregion 

        #endregion

        #region MediaviewHoveredItemMediaViewAsFull_Click
        private void MediaviewHoveredItemMediaViewAsFull_Click()
        {
            try
            {
                GenericMediaPreviewFoldeOrMediaList(imageListViewHoverItem);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region GenericMediaPreviewSelectedInDataGridView
        private void GenericMediaPreviewSelectedInDataGridView(DataGridView dataGridView)
        {
            try
            {
                List<string> listOfMediaFiles = new List<string>();

                List<int> selectedColumns = DataGridViewHandler.GetColumnSelected(dataGridView);

                if (selectedColumns.Count <= 1)
                {
                    int columnCount = DataGridViewHandler.GetColumnCount(dataGridView);
                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                    {
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                        if (dataGridViewGenericColumn?.Metadata?.FileFullPath != null) listOfMediaFiles.Add(dataGridViewGenericColumn?.Metadata?.FileFullPath);
                    }
                }
                else
                {
                    foreach (int columnIndex in selectedColumns)
                    {
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                        if (dataGridViewGenericColumn?.Metadata?.FileFullPath != null) listOfMediaFiles.Add(dataGridViewGenericColumn?.Metadata?.FileFullPath);
                    }
                }

                string selectedMediaFilePullPath = "";
                DataGridViewCell dataGridViewCell = DataGridViewHandler.GetCellCurrent(dataGridView);
                DataGridViewGenericColumn dataGridViewGenericColumnCurrent = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, dataGridViewCell.ColumnIndex);
                if (dataGridViewGenericColumnCurrent?.Metadata?.FileFullPath != null) selectedMediaFilePullPath = dataGridViewGenericColumnCurrent?.Metadata?.FileFullPath;

                MediaPreviewInit(listOfMediaFiles, selectedMediaFilePullPath);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region TagsAndKeywordMediaPreview_Click
        private void TagsAndKeywordMediaPreview_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaPreviewSelectedInDataGridView(dataGridView);
        }
        #endregion

        #region DateMediaPreview_Click
        private void DateMediaPreview_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaPreviewSelectedInDataGridView(dataGridView);
        }
        #endregion

        #region PeopleMediaPreview_Click
        private void PeopleMediaPreview_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaPreviewSelectedInDataGridView(dataGridView);
        }
        #endregion

        #region MapMediaPreview_Click
        private void MapMediaPreview_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaPreviewSelectedInDataGridView(dataGridView);
        }
        #endregion

        #region ExiftoolMediaPreview_Click
        private void ExiftoolMediaPreview_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaPreviewSelectedInDataGridView(dataGridView);
        }
        #endregion

        #region WarningsMediaPreview_Click
        private void WarningsMediaPreview_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaPreviewSelectedInDataGridView(dataGridView);
        }
        #endregion

        #region PropertiesMediaPreview_Click
        private void PropertiesMediaPreview_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaPreviewSelectedInDataGridView(dataGridView);
        }
        #endregion

        #region RenameMediaPreview_Click
        private void RenameMediaPreview_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaPreviewSelectedInDataGridView(dataGridView);
        }
        #endregion

        #region ConvertAndMergeMediaPreview_Click
        private void ConvertAndMergeMediaPreview_Click()
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            GenericMediaPreviewSelectedInDataGridView(dataGridView);
        }
        #endregion


        #endregion
        
        #region RefreshFolderAndFiles

        #region ActionRefreshFolderAndFiles
        private void ActionRefreshFolderAndFiles()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderRefresh_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesRefresh_Click();
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
        #endregion

        #region RefreshFolderAndFiles - Click Events Sources
        private void kryptonRibbonGroupButtonHomeFileSystemRefreshFolder_Click(object sender, EventArgs e)
        {
            ActionRefreshFolderAndFiles();
        }

        private void KryptonContextMenuItemGenericFileSystemRefreshFolder_Click(object sender, EventArgs e)
        {
            ActionRefreshFolderAndFiles();
        }
        #endregion

        #region FolderRefresh_Click
        private void FolderRefresh_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.DoNotRefreshImageListView = true;
                TreeNodePath selectedNode = (TreeNodePath)treeViewFolderBrowser1.SelectedNode;
                filesCutCopyPasteDrag.TreeViewFolderBrowserRefreshTreeNode(treeViewFolderBrowser1, selectedNode);
                GlobalData.DoNotRefreshImageListView = false;
                PopulateImageListView_FromFolderSelected(false, true);
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region MediaFilesRefresh_Click
        private void MediaFilesRefresh_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                PopulateImageListView_FromFolderSelected(false, true);
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion
        
        #region ReadSubfolders

        #region ActionReadSubfolders
        private void ActionReadSubfolders()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderReadSubfolders_Click();
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
        #endregion

        #region ReadSubfolders - Click Events Sources
        private void KryptonContextMenuItemGenericReadSubfolders_Click(object sender, EventArgs e)
        {
            ActionReadSubfolders();
        }
        #endregion

        #region ToolStrip - Refresh - Items in listview 
        private void FolderReadSubfolders_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                PopulateImageListView_FromFolderSelected(true, true);
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region GetSelectedFilesFromActiveDataGridView
        private List<string> GetSelectedFilesFromActiveDataGridView()
        {

            List<string> files = new List<string>();
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
                            if (dataGridViewGenericColumn != null && !files.Contains(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath)) files.Add(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath);
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
                            if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader &&
                                !files.Contains(dataGridViewGenericRow.RowName)) files.Add(dataGridViewGenericRow.RowName);
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return files;
        }
        #endregion

        #region GetSelectedFilesImageListView
        private List<string> GetSelectedFilesImageListView()
        {
            List<string> files = new List<string>();
            try
            {
                foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
                {
                    if (!files.Contains(imageListViewItem.FileFullPath)) files.Add(imageListViewItem.FileFullPath);
                }
            }
            catch { }
            return files;
        }
        #endregion

        #region GetSelectedFilesImageListView
        private HashSet<FileEntry> selectedFileEntriesImageListViewCache = null;
        private HashSet<string> selectedFullFileNameImageListViewCache = null;
        private void SelectedFileEntriesImageListViewCacheClear()
        {
            selectedFileEntriesImageListViewCache = null;
        }

        private bool DoesExistInSelectedFileEntriesImageListView(string fullFileName)
        {
            if (selectedFullFileNameImageListViewCache == null) return false;
            return selectedFullFileNameImageListViewCache.Contains(fullFileName);
        }

        private HashSet<FileEntry> GetSelectedFileEntriesImageListView()
        {
            if (selectedFileEntriesImageListViewCache != null) return selectedFileEntriesImageListViewCache;
            
            HashSet<FileEntry> fileEntries = new HashSet<FileEntry>();
            HashSet<string> fullFilePaths = new HashSet<string>();
            try
            {
                int queueCount = imageListView1.SelectedItems.Count;
                int queueSize = queueCount;


                LoadingItemsImageListView(0, queueCount);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
                {
                    LoadingItemsImageListView(--queueSize, queueCount);
                    FileEntry fileEntry = new FileEntry(imageListViewItem.FileFullPath, imageListViewItem.DateModified);
                    if (!fileEntries.Contains(fileEntry)) fileEntries.Add(fileEntry);
                    if (!fullFilePaths.Contains(fileEntry.FileFullPath)) fullFilePaths.Add(fileEntry.FileFullPath);
                    
                    if (stopwatch.ElapsedMilliseconds > 100)
                    {
                        kryptonWorkspaceCellMediaFiles.Refresh();
                        stopwatch.Restart();
                    }
                }

                LoadingItemsImageListView(0, 0);

                selectedFileEntriesImageListViewCache = fileEntries;
                selectedFullFileNameImageListViewCache = fullFilePaths;
            }
            catch { }
            return fileEntries;
        }
        #endregion

        #region GetSelectedFilesInFolder
        private List<string> GetSelectedFilesInFolder()
        {
            List<string> files = new List<string>();
            try
            {
                string folder = GetSelectedNodePath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't reach the folder. Not a valid folder selected.");
                    return files;
                }

                HashSet<FileEntry> fileEntries = ImageAndMovieFileExtentionsUtility.ListAllMediaFileEntries(folder, false);
                foreach (FileEntry fileEntry in fileEntries)
                {
                    if (!files.Contains(fileEntry.FileFullPath)) files.Add(fileEntry.FileFullPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
            }
            return files;
        }
        #endregion 

        #region OpenExplorerLocation

        #region OpenExplorerLocation
        private void ActionOpenExplorerLocation()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderOpenExplorerLocation_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesImageListView());
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    MediaFilesOpenExplorerLocation_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region FileSystemOpenExplorerLocation - Click Events Sources
        private void kryptonRibbonGroupButtonHomeFileSystemOpenExplorerLocation_Click(object sender, EventArgs e)
        {
            ActionOpenExplorerLocation();
        }

        private void KryptonContextMenuItemGenericOpenExplorerLocation_Click(object sender, EventArgs e)
        {
            ActionOpenExplorerLocation();
        }
        #endregion

        #region MediaFilesOpenExplorerLocation_Click
        private void MediaFilesOpenExplorerLocation_Click(List<string> files)
        {
            string errorMessage = "";

            foreach (string fileFullPath in files)
            {
                try
                {
                    ApplicationActivation.ShowFileInExplorer(fileFullPath);
                }
                catch (Exception ex) { errorMessage += (errorMessage == "" ? "" : "\r\n") + ex.Message; }
            }

            if (errorMessage != "") KryptonMessageBox.Show(errorMessage, "Failed to start application process...", MessageBoxButtons.OK);
        }
        #endregion

        #region FolderOpenExplorerLocation_Click
        private void FolderOpenExplorerLocation_Click()
        {
            try
            {
                string folder = GetSelectedNodePath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't open folder location. Not a valid folder selected.");
                    return;
                }
                ApplicationActivation.ShowFolderInEplorer(folder);
            }
            catch (Exception ex) { KryptonMessageBox.Show(ex.Message, "Failed to start application process...", MessageBoxButtons.OK); }
        }
        #endregion

        #endregion

        #region FileSystemVerbOpen (Files in Folder, ImageListView, Grid)

        #region ActionFileSystemOpen
        private void ActionFileSystemVerbOpen(int? columnIndex, int? rowIndex)
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    MediaFilesVerbOpen_Click(GetSelectedFilesInFolder());
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesVerbOpen_Click(GetSelectedFilesImageListView());
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                case KryptonPages.kryptonPageToolboxPeople:
                case KryptonPages.kryptonPageToolboxMap:
                case KryptonPages.kryptonPageToolboxDates:
                case KryptonPages.kryptonPageToolboxExiftool:
                case KryptonPages.kryptonPageToolboxWarnings:
                case KryptonPages.kryptonPageToolboxProperties:
                    if (columnIndex == null || rowIndex == null) MediaFilesVerbOpen_Click(GetSelectedFilesFromActiveDataGridView());
                    else if (columnIndex > -1 && rowIndex <= 0) //Head and first row allowd
                    {
                        List<string> files = new List<string>();
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(GetActiveTabDataGridView(), (int)columnIndex);
                        if (dataGridViewGenericColumn != null)
                        {
                            files.Add(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath);
                            MediaFilesVerbOpen_Click(files);
                        }
                    }
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    if (columnIndex == null || rowIndex == null) MediaFilesVerbOpen_Click(GetSelectedFilesFromActiveDataGridView());
                    else if (rowIndex > 0)
                    {
                        List<string> files = new List<string>();
                        DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(GetActiveTabDataGridView(), (int)rowIndex);
                        if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                        {
                            files.Add(dataGridViewGenericRow.Metadata.FileEntry.FileFullPath);
                            MediaFilesVerbOpen_Click(files);
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region FileSystemVerbOpen - Click Events Sources
        private void imageListView1_ItemDoubleClick(object sender, ItemClickEventArgs e)
        {
            ActionFileSystemVerbOpen(null, null);
        }

        private void kryptonRibbonGroupButtonHomeFileSystemOpen_Click(object sender, EventArgs e)
        {
            ActionFileSystemVerbOpen(null, null);
        }

        private void KryptonContextMenuItemGenericOpen_Click(object sender, EventArgs e)
        {
            ActionFileSystemVerbOpen(null, null);
        }
        #endregion

        #region MediaFileOpen_DoubleClick Events Source

        private void dataGridViewTagsAndKeywords_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ActionFileSystemVerbOpen(e.ColumnIndex, e.RowIndex);
        }

        private void dataGridViewPeople_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ActionFileSystemVerbOpen(e.ColumnIndex, e.RowIndex);
        }

        //private void dataGridViewMap_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    ActionFileSystemVerbOpen();
        //}

        private void dataGridViewDate_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ActionFileSystemVerbOpen(e.ColumnIndex, e.RowIndex);
        }

        

        private void dataGridViewConvertAndMerge_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ActionFileSystemVerbOpen(e.ColumnIndex, e.RowIndex);
        }

        private void dataGridViewRename_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ActionFileSystemVerbOpen(e.ColumnIndex, e.RowIndex);
        }


        private void dataGridViewProperties_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ActionFileSystemVerbOpen(e.ColumnIndex, e.RowIndex);
        }

        private void dataGridViewExiftoolWarning_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ActionFileSystemVerbOpen(e.ColumnIndex, e.RowIndex);
        }

        private void dataGridViewExiftool_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ActionFileSystemVerbOpen(e.ColumnIndex, e.RowIndex);
        }


        


        #endregion

        #region MediaFilesVerbOpen_Click
        private void MediaFilesVerbOpen_Click(List<string> files)
        {
            string errorMessage = "";

            foreach (string fullFileName in files)
            {
                try
                {
                    ApplicationActivation.ProcessRunOpenFile(fullFileName);
                }
                catch (Exception ex) { errorMessage += (errorMessage == "" ? "" : "\r\n") + ex.Message; }
            }

            if (errorMessage != "") KryptonMessageBox.Show(errorMessage, "Failed to start application process...", MessageBoxButtons.OK);
        }
        #endregion


        #endregion

        #region OpenWith - Selected Verb (Files in Folder, ImageListView, Grid)

        #region ActionFileSystemOpenWith
        private void ActionFileSystemOpenWith(ApplicationData applicationData)
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesInFolder());
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesImageListView());
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    OpenWithSelectedVerb(applicationData, GetSelectedFilesFromActiveDataGridView());
                    break;
                default:
                    throw new NotImplementedException();
            } 
        }
        #endregion

        #region OpenWithSelectedVerb
        private void OpenWithSelectedVerb(ApplicationData applicationData, List<string> files)
        {          
            foreach (string fileFullPath in files)
            {
                try
                {
                    if (applicationData.VerbLinks.Count >= 1) ApplicationActivation.ProcessRun(applicationData.VerbLinks[0].Command, applicationData.ApplicationId, fileFullPath, applicationData.VerbLinks[0].Verb, false);
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show("Can execute file. Error: " + ex.Message, "Can execute file", MessageBoxButtons.OK);
                }
            }   
        }
        #endregion 

        #region KryptonContextMenuItemOpenWithSelectedVerb_Click
        private void KryptonContextMenuItemOpenWithSelectedVerb_Click(object sender, EventArgs e)
        {
            try
            {
                Krypton.Toolkit.KryptonContextMenuItem kryptonContextMenuItem = (Krypton.Toolkit.KryptonContextMenuItem)sender;
                if (kryptonContextMenuItem.Tag is ApplicationData applicationData) ActionFileSystemOpenWith(applicationData);                
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region OpenAndAssociateWithDialog (Files in Folder, ImageListView, Grid)

        #region ActionOpenAndAssociateWithDialog
        private void ActionOpenAndAssociateWithDialog()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesInFolder());
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesImageListView());
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    MediaFilesOpenAndAssociateWithDialog_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region ActionOpenAndAssociateWithDialog - Click Events Sources
        private void KryptonContextMenuItemOpenAndAssociateWithDialog_Click(object sender, EventArgs e)
        {
            ActionOpenAndAssociateWithDialog();
        }

        private void kryptonRibbonGroupButtonFileSystemOpenAssociateDialog_Click(object sender, EventArgs e)
        {
            ActionOpenAndAssociateWithDialog();
        }
        #endregion

        #region MediaFilesOpenWithDialog_Click()
        private void MediaFilesOpenAndAssociateWithDialog_Click(List<string> files)
        {
            try
            {
                foreach (string fileFullPath in files)
                {
                    ApplicationActivation.ShowOpenWithDialog(fileFullPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #endregion

        #region FileSystemVerbEdit

        #region ActionFileSystemVerbEdit (Files in Folder, ImageListView, Grid)
        private void ActionFileSystemVerbEdit()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    MediaFilesVerbEdit_Click(GetSelectedFilesInFolder());
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesVerbEdit_Click(GetSelectedFilesImageListView());
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    MediaFilesVerbEdit_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    MediaFilesVerbEdit_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MediaFilesVerbEdit_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    MediaFilesVerbEdit_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    MediaFilesVerbEdit_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    MediaFilesVerbEdit_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    MediaFilesVerbEdit_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    MediaFilesVerbEdit_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    MediaFilesVerbEdit_Click(GetSelectedFilesFromActiveDataGridView());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region FileSystemVerbEdit - Click Events Sources
        private void kryptonRibbonGroupButtonHomeFileSystemVerbEdit_Click(object sender, EventArgs e)
        {
            ActionFileSystemVerbEdit();
        }
        private void KryptonContextMenuItemGenericFileSystemVerbEdit_Click(object sender, EventArgs e)
        {
            ActionFileSystemVerbEdit();
        }
        #endregion

        #region MediaFilesVerbEdit()
        private void MediaFilesVerbEdit_Click(List<string> files)
        {
            string errorMessage = "";

            foreach (string fileFullPath in files)
            {
                try
                {
                    ApplicationActivation.ProcessRunEditFile(fileFullPath);
                }
                catch (Exception ex) { errorMessage += (errorMessage == "" ? "" : "\r\n") + ex.Message; }
            }

            if (errorMessage != "") KryptonMessageBox.Show(errorMessage, "Failed to start application process...", MessageBoxButtons.OK);
        }
        #endregion

        #endregion

        #region FileSystemRunCommand (Files in Folder, ImageListView, Grid)

        #region ActionFileSystemRunCommand
        private void ActionFileSystemRunCommand()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    MediaFilesRunCommand(GetSelectedFilesInFolder());
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesRunCommand(GetSelectedFilesImageListView());
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    MediaFilesRunCommand(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    MediaFilesRunCommand(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    MediaFilesRunCommand(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    MediaFilesRunCommand(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    MediaFilesRunCommand(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    MediaFilesRunCommand(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    MediaFilesRunCommand(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    MediaFilesRunCommand(GetSelectedFilesFromActiveDataGridView());
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    MediaFilesRunCommand(GetSelectedFilesFromActiveDataGridView());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region FileSystemRunCommand - Click Events Sources
        private void kryptonRibbonGroupButtonFileSystemRunCommand_Click(object sender, EventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            ActionFileSystemRunCommand();
        }

        private void KryptonContextMenuItemGenericFileSystemRunCommand_Click(object sender, EventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            ActionFileSystemRunCommand();
        }
        #endregion

        #region MediaFilesRunCommand
        private void MediaFilesRunCommand(List<string> files)
        {
            try
            {
                if (imageListView1.SelectedItems.Count > 0)
                {
                    FormSplash.ShowSplashScreen("Populate the 'Advance: Run Command' form...", imageListView1.SelectedItems.Count + 3, false, false);

                    string writeMetadataTagsVariable = Properties.Settings.Default.WriteMetadataTags;
                    string writeMetadataKeywordAddVariable = Properties.Settings.Default.WriteMetadataKeywordAdd;

                    List<string> allowedFileNameDateTimeFormats = FileDateTime.FileDateTimeReader.ConvertStringOfDatesToList(Properties.Settings.Default.RenameDateFormats);

                    FormSplash.UpdateStatus("Create argument file...");
                    #region Create ArgumentFile file
                    GetDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView);

                    ExiftoolWriter.CreateExiftoolArguFileText(
                        metadataListFromDataGridView, metadataListOriginalExiftool, allowedFileNameDateTimeFormats, writeMetadataTagsVariable, writeMetadataKeywordAddVariable,
                        true, out string exiftoolAgruFileText);
                    #endregion

                    FormSplash.UpdateStatus("Create AutoCorrect file...");
                    #region AutoCorrect
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    if (autoCorrect == null) KryptonMessageBox.Show("AutoCorrect: " + Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    List<Metadata> metadataListEmpty = new List<Metadata>();
                    List<Metadata> metadataListFromDataGridViewAutoCorrect = new List<Metadata>();

                    foreach (ImageListViewItem item in imageListView1.SelectedItems)
                    {
                        
                        if (files.Contains(item.FileFullPath))
                        {
                            FormSplash.UpdateStatus("Create AutoCorrect file..." + item.Text);
                            Metadata metadataToSave = autoCorrect.FixAndSave(
                                new FileEntry(item.FileFullPath, item.DateModified),
                                null,
                                databaseAndCacheMetadataExiftool,
                                databaseAndCacheMetadataMicrosoftPhotos,
                                databaseAndCacheMetadataWindowsLivePhotoGallery,
                                databaseAndCahceCameraOwner,
                                databaseLocationAddress,
                                databaseGoogleLocationHistory,
                                locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                autoKeywordConvertions,
                                Properties.Settings.Default.RenameDateFormats);

                            if (metadataToSave != null) metadataListFromDataGridViewAutoCorrect.Add(new Metadata(metadataToSave));
                            else
                            {
                                Logger.Warn("Metadata was not loaded for file, check if file is only in cloud:" + item.FileFullPath);
                            }
                            metadataListEmpty.Add(new Metadata(MetadataBrokerType.Empty));
                        } else FormSplash.UpdateStatus("Skip AutoCorrect for file..." + item.Text);
                    }


                    ExiftoolWriter.CreateExiftoolArguFileText(
                        metadataListFromDataGridViewAutoCorrect, metadataListEmpty, allowedFileNameDateTimeFormats,
                        writeMetadataTagsVariable, writeMetadataKeywordAddVariable,
                        true, out string exiftoolAutoCorrectFileText);
                    #endregion

                    using (FormRunCommand runCommand = new FormRunCommand())
                    {
                        runCommand.ArguFile = exiftoolAgruFileText;
                        runCommand.ArguFileAutoCorrect = exiftoolAutoCorrectFileText;
                        runCommand.MetadatasGridView = metadataListFromDataGridView;
                        runCommand.MetadatasOriginal = metadataListOriginalExiftool;
                        runCommand.MetadatasEmpty = metadataListEmpty;
                        runCommand.AllowedFileNameDateTimeFormats = allowedFileNameDateTimeFormats;
                        runCommand.MetadataPrioity = exiftoolReader.MetadataReadPrioity;

                        FormSplash.UpdateStatus("Populate applications and icons...");
                        runCommand.Init();
                        FormSplash.CloseForm();

                        runCommand.ShowDialog();
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region AutoCorrectRun (Files in Folder, ImageListView, NOT: Grid)

        #region ActionAutoCorrectRun 
        private void ActionAutoCorrectRun()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    AutoCorrectRunFolder_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    AutoCorrectRunMediaFiles_Click();
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    AutoCorrectRunDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    AutoCorrectRunDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    AutoCorrectRunDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    AutoCorrectRunDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    AutoCorrectRunDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    AutoCorrectRunDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    AutoCorrectRunDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    AutoCorrectRunDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    AutoCorrectRunDataGridView_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region AutoCorrectRun - Click Events Sources
        private void KryptonContextMenuItemGenericAutoCorrectRun_Click(object sender, EventArgs e)
        {
            ActionAutoCorrectRun();
        }
        private void kryptonRibbonGroupButtonHomeAutoCorrectRun_Click(object sender, EventArgs e)
        {
            ActionAutoCorrectRun();
        }
        #endregion

        #region MediaFilesAutoCorrectRun_Click
        private void AutoCorrectRunMediaFiles_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    Metadata metadataToSave = autoCorrect.FixAndSave(
                        new FileEntry(item.FileFullPath, item.DateModified),
                        null,
                        databaseAndCacheMetadataExiftool,
                        databaseAndCacheMetadataMicrosoftPhotos,
                        databaseAndCacheMetadataWindowsLivePhotoGallery,
                        databaseAndCahceCameraOwner,
                        databaseLocationAddress,
                        databaseGoogleLocationHistory,
                        locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted, 
                        autoKeywordConvertions,
                        Properties.Settings.Default.RenameDateFormats);
                    if (metadataToSave != null)
                    {
                        AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                        AddQueueRenameLock(item.FileFullPath, autoCorrect.RenameVariable); //Properties.Settings.Default.AutoCorrect.)
                    }
                }

                StartThreads();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderAutoCorrectRun_Click
        private void AutoCorrectRunFolder_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                string selectedFolder = GetSelectedNodePath();
                if (selectedFolder == null || !Directory.Exists(selectedFolder))
                {
                    KryptonMessageBox.Show("Can't run AutoCorrect. Not a valid folder selected.");
                    return;
                }
                string[] files = Directory.GetFiles(selectedFolder, "*.*");
                foreach (string file in files)
                {
                    Metadata metadataToSave = autoCorrect.FixAndSave(
                        new FileEntry(file, File.GetLastWriteTime(file)),
                        null,
                        databaseAndCacheMetadataExiftool,
                        databaseAndCacheMetadataMicrosoftPhotos,
                        databaseAndCacheMetadataWindowsLivePhotoGallery,
                        databaseAndCahceCameraOwner,
                        databaseLocationAddress,
                        databaseGoogleLocationHistory, locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                        autoKeywordConvertions,
                        Properties.Settings.Default.RenameDateFormats);
                    if (metadataToSave != null)
                    {
                        AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                        AddQueueRenameLock(file, autoCorrect.RenameVariable); //Properties.Settings.Default.AutoCorrect.)
                    }
                }
                StartThreads();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region AutoCorrectRunDataGridView_Click
        private void AutoCorrectRunDataGridView_Click()
        {
            if (GlobalData.IsPopulatingAnything())
            {
                KryptonMessageBox.Show("Data is populating, please try a bit later.");
                return;
            }
            if (!GlobalData.IsAgredagedGridViewAny())
            {
                KryptonMessageBox.Show("No metadata are updated.");
                return;
            }
            DataGridView dataGridView = GetActiveTabDataGridView();

            try
            {
                List<FileEntryAttribute> fileEntryAttributes = new List<FileEntryAttribute>();
                
                foreach (int columIndex in DataGridViewHandler.GetColumnSelected(dataGridView))
                {
                    DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columIndex);
                    if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.Metadata != null)
                    {
                        GlobalData.ListOfAutoCorrectFilesAdd(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath, null);
                        fileEntryAttributes.Add(new FileEntryAttribute(
                            dataGridViewGenericColumn.Metadata.FileEntry.FileFullPath,
                            dataGridViewGenericColumn.Metadata.FileEntry.LastWriteDateTime,
                            FileEntryVersion.AutoCorrect));
                    }
                }
                AddQueueLazyLoadningDataGridViewMetadataLock(fileEntryAttributes);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            ThreadSaveMetadata();
        }
        #endregion

        #endregion

        #region AutoCorrectFrom (Files in Folder, ImageListView,  Grid)

        #region ActionAutoCorrectForm
        private void ActionAutoCorrectForm()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    AutoCorrectFormFolder_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    AutoCorrectFormMediaFiles_Click();
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                    AutoCorrectFormDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    AutoCorrectFormDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    AutoCorrectFormDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    AutoCorrectFormDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    AutoCorrectFormDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    AutoCorrectFormDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxProperties:
                    AutoCorrectFormDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    AutoCorrectFormDataGridView_Click();
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    AutoCorrectFormDataGridView_Click();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region AutoCorrectForm - Click Events Sources
        private void kryptonRibbonGroupButtonHomeAutoCorrectForm_Click(object sender, EventArgs e)
        {
            ActionAutoCorrectForm();
        }
        private void KryptonContextMenuItemGenericAutoCorrectForm_Click(object sender, EventArgs e)
        {
            ActionAutoCorrectForm();
        }
        #endregion

        #region MediaFilesAutoCorrectForm_Click
        private void AutoCorrectFormMediaFiles_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                FormAutoCorrect formAutoCorrect = new FormAutoCorrect();
                if (formAutoCorrect.ShowDialog() == DialogResult.OK)
                {

                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    AutoCorrectFormVaraibles autoCorrectFormVaraibles = formAutoCorrect.AutoCorrectFormVaraibles;
                    autoCorrectFormVaraibles.WriteAlbumOnDescription = autoCorrect.UpdateDescription;

                    foreach (ImageListViewItem item in imageListView1.SelectedItems)
                    {
                        Metadata metadataToSave = autoCorrect.FixAndSave(
                            new FileEntry(item.FileFullPath, item.DateModified),
                            null,
                            databaseAndCacheMetadataExiftool,
                            databaseAndCacheMetadataMicrosoftPhotos,
                            databaseAndCacheMetadataWindowsLivePhotoGallery,
                            databaseAndCahceCameraOwner,
                            databaseLocationAddress,
                            databaseGoogleLocationHistory,
                            locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted, 
                            autoKeywordConvertions,
                            Properties.Settings.Default.RenameDateFormats);

                        if (metadataToSave != null)
                        {
                            AutoCorrectFormVaraibles.UpdateMetaData(ref metadataToSave, autoCorrectFormVaraibles);
                            AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                            AddQueueRenameLock(item.FileFullPath, autoCorrect.RenameVariable);
                        }
                    }

                    StartThreads();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderAutoCorrectForm_Click
        private void AutoCorrectFormFolder_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                FormAutoCorrect formAutoCorrect = new FormAutoCorrect();
                if (formAutoCorrect.ShowDialog() == DialogResult.OK)
                {

                    
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    AutoCorrectFormVaraibles autoCorrectFormVaraibles = formAutoCorrect.AutoCorrectFormVaraibles;
                    autoCorrectFormVaraibles.WriteAlbumOnDescription = autoCorrect.UpdateDescription;

                    string selectedFolder = GetSelectedNodePath();
                    if (selectedFolder == null || !Directory.Exists(selectedFolder))
                    {
                        KryptonMessageBox.Show("Can't run AutoCorrect. Not a valid folder selected.");
                        return;
                    }
                    string[] files = Directory.GetFiles(selectedFolder, "*.*");
                    foreach (string file in files)
                    {
                        Metadata metadataToSave = autoCorrect.FixAndSave(
                            new FileEntry(file, File.GetLastWriteTime(file)),
                            null,
                            databaseAndCacheMetadataExiftool,
                            databaseAndCacheMetadataMicrosoftPhotos,
                            databaseAndCacheMetadataWindowsLivePhotoGallery,
                            databaseAndCahceCameraOwner,
                            databaseLocationAddress,
                            databaseGoogleLocationHistory,
                            locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                            autoKeywordConvertions,
                            Properties.Settings.Default.RenameDateFormats);

                        if (metadataToSave != null)
                        {
                            AutoCorrectFormVaraibles.UpdateMetaData(ref metadataToSave, autoCorrectFormVaraibles);
                            AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                            AddQueueRenameLock(file, autoCorrect.RenameVariable);
                        }
                    }

                    StartThreads();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region AutoCorrectFormDataGridView_Click
        private void AutoCorrectFormDataGridView_Click()
        {

            if (GlobalData.IsPopulatingAnything())
            {
                KryptonMessageBox.Show("Data is populating, please try a bit later.");
                return;
            }
            if (!GlobalData.IsAgredagedGridViewAny())
            {
                KryptonMessageBox.Show("No metadata are updated.");
                return;
            }
            DataGridView dataGridView = GetActiveTabDataGridView();

            try
            {
                FormAutoCorrect formAutoCorrect = new FormAutoCorrect();
                if (formAutoCorrect.ShowDialog() == DialogResult.OK)
                {

                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    AutoCorrectFormVaraibles autoCorrectFormVaraibles = formAutoCorrect.AutoCorrectFormVaraibles;
                    autoCorrectFormVaraibles.WriteAlbumOnDescription = autoCorrect.UpdateDescription;

                    List<FileEntryAttribute> fileEntryAttributes = new List<FileEntryAttribute>();
                    foreach (int columIndex in DataGridViewHandler.GetColumnSelected(dataGridView))
                    {
                        
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columIndex);
                        if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.Metadata != null)
                        {
                            GlobalData.ListOfAutoCorrectFilesAdd(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath, autoCorrectFormVaraibles);
                            fileEntryAttributes.Add(new FileEntryAttribute(
                                dataGridViewGenericColumn.Metadata.FileEntry.FileFullPath,
                                dataGridViewGenericColumn.Metadata.FileEntry.LastWriteDateTime,
                                FileEntryVersion.AutoCorrect));
                        }
                    }
                    AddQueueLazyLoadningDataGridViewMetadataLock(fileEntryAttributes);
                    StartThreads();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region MetadataRefreshLast (Files in Folder, ImageListView, NOT Grid)

        #region ActionMetadataRefreshLast
        private void ActionMetadataRefreshLast()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderMetadataRefreshLast_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:                    
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesMetadataRefreshLast_Click();
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
        #endregion

        #region MetadataRefreshLast - Click Events Sources
        private void kryptonRibbonGroupButtonHomeMetadataRefreshLast_Click(object sender, EventArgs e)
        {
            ActionMetadataRefreshLast();
        }

        private void KryptonContextMenuItemGenericMetadataRefreshLast_Click(object sender, EventArgs e)
        {
            ActionMetadataRefreshLast();
        }
        #endregion

        #region DeleteLastMediadataAndReload
        void DeleteLastMediadataAndReload(ImageListView imageListView, bool updatedOnlySelected)
        {
            try
            {
                if (GlobalData.IsPopulatingAnything()) return;

                using (new WaitCursor())
                {
                    GlobalData.IsPopulatingButtonAction = true;
                    GlobalData.IsPopulatingImageListView = true; //Avoid one and one select item getting refreshed
                    GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
                    treeViewFolderBrowser1.Enabled = false;
                    ImageListViewSuspendLayoutInvoke(imageListView);

                    //Clean up ImageListView and other queues
                    ImageListViewClearThumbnailCache(imageListView1);
                    //imageListView1.Refresh();
                    ClearAllQueues();

                    UpdateStatusAction("Delete all data and files...");
                    lock (GlobalData.ReloadAllowedFromCloudLock)
                    {
                        GlobalData.ReloadAllowedFromCloud = filesCutCopyPasteDrag.DeleteFileEntriesBeforeReload(imageListView.Items, updatedOnlySelected);
                    }
                    filesCutCopyPasteDrag.ImageListViewReload(imageListView.Items, updatedOnlySelected);

                    treeViewFolderBrowser1.Enabled = true;
                    ImageListViewResumeLayoutInvoke(imageListView);
                    GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                    GlobalData.IsPopulatingButtonAction = false;
                    GlobalData.IsPopulatingImageListView = false;

                    FilesSelectedOrNoneSelected();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region MediaFilesMetadataRefreshLast_Click
        private void MediaFilesMetadataRefreshLast_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                DeleteLastMediadataAndReload(imageListView1, true);

                DataGridView dataGridView = GetActiveTabDataGridView();
                if (dataGridView != null) DataGridViewHandler.Focus(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region FolderMetadataRefreshLast_Click
        private void FolderMetadataRefreshLast_Click()
        {
            try
            {
                if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
                DeleteLastMediadataAndReload(imageListView1, false);
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region MetadataReloadDeleteHistory (Files in Folder, ImageListView, NOT Grid)

        #region ActionMetadataReloadDeleteHistory
        private void ActionMetadataReloadDeleteHistory()
        {
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                    FolderMetadataReloadDeleteHistory_Click();
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                    break;
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                    break;
                case KryptonPages.kryptonPageMediaFiles:
                    MediaFilesMetadataReloadDeleteHistory_Click();
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
        #endregion

        #region MetadataReloadDeleteHistory - Click Events Sources
        private void kryptonRibbonGroupButtonHomeMetadataReloadDeleteHistory_Click(object sender, EventArgs e)
        {
            ActionMetadataReloadDeleteHistory();
        }
        private void KryptonContextMenuItemGenericMetadataReloadDeleteHistory_Click(object sender, EventArgs e)
        {
            ActionMetadataReloadDeleteHistory();
        }
        #endregion

        #region MediaFilesMetadataReloadDeleteHistory_Click
        private void MediaFilesMetadataReloadDeleteHistory_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                using (new WaitCursor())
                {
                    filesCutCopyPasteDrag.ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(this, treeViewFolderBrowser1, imageListView1);
                    FilesSelectedOrNoneSelected();
                    DisplayAllQueueStatus();
                }
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderMetadataReloadDeleteHistory_Click
        private void FolderMetadataReloadDeleteHistory_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                string folder = GetSelectedNodePath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't reload folder. Not a valid folder selected.");
                    return;
                }

                string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly).Take(51).ToArray();
                if (KryptonMessageBox.Show("Are you sure you will delete **ALL** metadata history in database store for " +
                    (fileAndFolderEntriesCount.Length == 51 ? " over 50 + " : fileAndFolderEntriesCount.Length.ToString()) +
                    "  number of files.\r\n\r\n" +
                    "In the folder: " + folder,
                    "You are going to delete all metadata in folder",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using (new WaitCursor())
                    {
                        //Clean up ImageListView and other queues
                        ImageListViewClearThumbnailCache(imageListView1);
                        imageListView1.Refresh();
                        ClearAllQueues();

                        UpdateStatusAction("Delete all record about files in database....");
                        GlobalData.ProcessCounterDelete = FilesCutCopyPasteDrag.DeleteDirectoryAndHistorySize;
                        int recordAffected = filesCutCopyPasteDrag.DeleteDirectoryAndHistory(ref FilesCutCopyPasteDrag.DeleteDirectoryAndHistorySize, folder);
                        GlobalData.ProcessCounterDelete = 0;
                        UpdateStatusAction(recordAffected + " records was delete from database....");

                        string selectedFolder = GetSelectedNodePath();
                        HashSet<FileEntry> fileEntries = ImageAndMovieFileExtentionsUtility.ListAllMediaFileEntries(selectedFolder, false);

                        PopulateImageListView(fileEntries, selectedFolder, false);
                    }
                }
                DisplayAllQueueStatus();
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        //---- Tools
        
        #region ImportLocations

        #region ImportLocations - Click Events Sources
        private void kryptonRibbonGroupButtonToolsImportLocations_Click(object sender, EventArgs e)
        {
            ImportLocations_Click();
        }
        #endregion

        #region ImportLocations_Click
        private void ImportLocations_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                FormLocationHistoryImport form = new FormLocationHistoryImport();
                using (new WaitCursor())
                {
                    form.databaseTools = databaseUtilitiesSqliteMetadata;
                    form.databaseAndCahceCameraOwner = databaseAndCahceCameraOwner;
                    form.Init();
                }
                if (form.ShowDialog() == DialogResult.OK)
                {
                    databaseAndCahceCameraOwner.CameraMakeModelAndOwnerMakeDirty();
                    databaseAndCahceCameraOwner.MakeCameraOwnersDirty();
                    //Update DataGridViews
                    FilesSelectedOrNoneSelected();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion
        
        #region WebScraper

        #region WebScraper - Click Events Sources
        private void kryptonRibbonGroupButtonToolsWebScraping_Click(object sender, EventArgs e)
        {
            WebScraper_Click();
        }
        #endregion 

        #region WebScraper_Click
        private void WebScraper_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                FormWebScraper formWebScraper = new FormWebScraper();
                formWebScraper.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                formWebScraper.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion
        
        #region Config

        #region Config - Click Events Sources
        private void kryptonRibbonGroupButtonToolsConfig_Click(object sender, EventArgs e)
        {
            Config_Click();
        }
        #endregion 

        #region Config_Click
        private void Config_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                using (FormConfig config = new FormConfig(kryptonManager1, imageListView1))
                {
                    using (new WaitCursor())
                    {
                        LocationNameLookUpCache databaseLocationNames = new LocationNameLookUpCache(databaseUtilitiesSqliteMetadata, Properties.Settings.Default.ApplicationPreferredLanguages);

                        exiftoolReader.MetadataReadPrioity.ReadOnlyOnce();
                        config.MetadataReadPrioity = exiftoolReader.MetadataReadPrioity;
                        config.ThumbnailSizes = thumbnailSizes;
                        config.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                        config.DatabaseLocationNames = databaseLocationNames;
                        config.DatabaseAndCacheLocationAddress = databaseLocationAddress;
                        config.DatabaseUtilitiesSqliteMetadata = databaseUtilitiesSqliteMetadata;
                        config.Init();
                    }
                    if (config.ShowDialog() != DialogResult.Cancel)
                    {
                        //Thumbnail
                        ThumbnailSaveSize = Properties.Settings.Default.ApplicationThumbnail;
                        RegionThumbnailHandler.FaceThumbnailSize = Properties.Settings.Default.ApplicationRegionThumbnail;
                        fileDateTimeReader = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);

                        databaseLocationAddress.PreferredLanguagesString = Properties.Settings.Default.ApplicationPreferredLanguages;
                        RegionStructure.SetAcceptRegionMissmatchProcent((float)Properties.Settings.Default.RegionMissmatchProcent);

                        autoKeywordConvertions = AutoKeywordHandler.PopulateList(AutoKeywordHandler.ReadDataSetFromXML());
                        //Cache config
                        cacheNumberOfPosters = (int)Properties.Settings.Default.CacheNumberOfPosters;
                        cacheAllMetadatas = Properties.Settings.Default.CacheAllMetadatas;
                        cacheAllThumbnails = Properties.Settings.Default.CacheAllThumbnails;
                        cacheAllWebScraperDataSets = Properties.Settings.Default.CacheAllWebScraperDataSets;
                        cacheFolderMetadatas = Properties.Settings.Default.CacheFolderMetadatas;
                        cacheFolderThumbnails = Properties.Settings.Default.CacheFolderThumbnails;
                        cacheFolderWebScraperDataSets = Properties.Settings.Default.CacheFolderWebScraperDataSets;

                        //
                        treeViewFolderBrowser1.Enabled = false;
                        imageListView1.Enabled = false;
                        FilesSelectedOrNoneSelected();
                        treeViewFolderBrowser1.Enabled = true;
                        imageListView1.Enabled = true;
                        imageListView1.Focus();
                    }
                    //Palette
                    if (config.IsKryptonManagerChanged)
                    {
                        KryptonPalette kryptonPalette = KryptonPaletteHandler.Load(Properties.Settings.Default.KryptonPaletteFullFilename, Properties.Settings.Default.KryptonPaletteName);
                        KryptonPaletteHandler.SetPalette(this, kryptonManager1, kryptonPalette, KryptonPaletteHandler.IsSystemPalette, Properties.Settings.Default.KryptonPaletteDropShadow);                        
                    }
                    KryptonPaletteHandler.SetDataGridViewPalette(kryptonManager1, dataGridViewConvertAndMerge);
                    KryptonPaletteHandler.SetDataGridViewPalette(kryptonManager1, dataGridViewDate);
                    KryptonPaletteHandler.SetDataGridViewPalette(kryptonManager1, dataGridViewExiftool);
                    KryptonPaletteHandler.SetDataGridViewPalette(kryptonManager1, dataGridViewExiftoolWarning);
                    KryptonPaletteHandler.SetDataGridViewPalette(kryptonManager1, dataGridViewMap);
                    KryptonPaletteHandler.SetDataGridViewPalette(kryptonManager1, dataGridViewPeople);
                    KryptonPaletteHandler.SetDataGridViewPalette(kryptonManager1, dataGridViewProperties);
                    KryptonPaletteHandler.SetDataGridViewPalette(kryptonManager1, dataGridViewRename);
                    KryptonPaletteHandler.SetDataGridViewPalette(kryptonManager1, dataGridViewTagsAndKeywords);
                    
                    KryptonPaletteHandler.SetImageListViewPalettes(kryptonManager1, imageListView1);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region DatabaseCleaner
        private void kryptonRibbonGroupButtonToolsDatabaseCleaner_Click(object sender, EventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                using (FormDatabaseCleaner formDatabaseCleaner = new FormDatabaseCleaner())
                {
                    using (new WaitCursor())
                    {
                        formDatabaseCleaner.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                    
                    }
                    if (formDatabaseCleaner.ShowDialog() != DialogResult.Cancel)
                    {
                    }   
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region About

        #region About - Click Event Sources
        private void kryptonRibbonGroupButtonToolsAbout_Click(object sender, EventArgs e)
        {
            About_Click();
        }
        #endregion 

        #region About_Click
        private void About_Click()
        {
            try
            {
                FormAbout form = new FormAbout();
                form.ShowDialog();
                form.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region LocationAnalytics
        private void kryptonRibbonGroupButtonToolsLocationAnalytics_Click(object sender, EventArgs e)
        {
            ShowFormLocationHistoryAnalytics();
        }

        private void KryptonContextMenuItemToolLocationAnalytics_Click(object sender, EventArgs e)
        {
            ShowFormLocationHistoryAnalytics();
        }
        #endregion
        
        //----
        #region AssignCompositeTag 

        #region PopulateExiftoolToolStripMenuItems
        public void PopulateExiftoolToolStripMenuItems()
        {
            kryptonContextMenuItemsAssignCompositeTagList.Items.Clear();
            //toolStripMenuItemExiftoolAssignCompositeTag.DropDownItems.Clear();

            SortedDictionary<string, string> listAllTags = new CompositeTags().ListAllTags();
            foreach (KeyValuePair<string, string> tag in listAllTags.OrderBy(key => key.Value))
            {
                KryptonContextMenuItem kryptonContextMenuItemAssignCompositeTag;
                KryptonContextMenuItems kryptonContextMenuItemAssignCompositeTagSubList;


                switch (tag.Value)
                {
                    case CompositeTags.NotDefined:
                        kryptonContextMenuItemAssignCompositeTag = new KryptonContextMenuItem();
                        kryptonContextMenuItemAssignCompositeTag.Text = tag.Value;
                        kryptonContextMenuItemAssignCompositeTag.Tag = new MetadataPriorityValues(tag.Value, 25);
                        kryptonContextMenuItemAssignCompositeTag.Click += KryptonContextMenuItemAssignCompositeTag_Click;
                        kryptonContextMenuItemsAssignCompositeTagList.Items.Add(kryptonContextMenuItemAssignCompositeTag);
                        break;
                    case CompositeTags.Ignore:
                        kryptonContextMenuItemAssignCompositeTag = new KryptonContextMenuItem();
                        kryptonContextMenuItemAssignCompositeTag.Text = tag.Value;
                        kryptonContextMenuItemAssignCompositeTag.Tag = new MetadataPriorityValues(tag.Value, 25);
                        kryptonContextMenuItemAssignCompositeTag.Click += KryptonContextMenuItemAssignCompositeTag_Click;
                        kryptonContextMenuItemsAssignCompositeTagList.Items.Add(kryptonContextMenuItemAssignCompositeTag);

                        break;
                    default:
                        kryptonContextMenuItemAssignCompositeTagSubList = new KryptonContextMenuItems();

                        kryptonContextMenuItemAssignCompositeTag = new KryptonContextMenuItem();
                        kryptonContextMenuItemAssignCompositeTag.Text = tag.Value;
                        kryptonContextMenuItemsAssignCompositeTagList.Items.Add(kryptonContextMenuItemAssignCompositeTag);
                        kryptonContextMenuItemAssignCompositeTag.Items.Add(kryptonContextMenuItemAssignCompositeTagSubList);


                        kryptonContextMenuItemAssignCompositeTag = new KryptonContextMenuItem();
                        kryptonContextMenuItemAssignCompositeTag.Text = "Prioity low - 25";
                        kryptonContextMenuItemAssignCompositeTag.Tag = new MetadataPriorityValues(tag.Value, 25);
                        kryptonContextMenuItemAssignCompositeTag.Click += KryptonContextMenuItemAssignCompositeTag_Click;
                        kryptonContextMenuItemAssignCompositeTagSubList.Items.Add(kryptonContextMenuItemAssignCompositeTag);

                        kryptonContextMenuItemAssignCompositeTag = new KryptonContextMenuItem();
                        kryptonContextMenuItemAssignCompositeTag.Text = "Prioity medium low - 50";
                        kryptonContextMenuItemAssignCompositeTag.Tag = new MetadataPriorityValues(tag.Value, 50);
                        kryptonContextMenuItemAssignCompositeTag.Click += KryptonContextMenuItemAssignCompositeTag_Click;
                        kryptonContextMenuItemAssignCompositeTagSubList.Items.Add(kryptonContextMenuItemAssignCompositeTag);

                        kryptonContextMenuItemAssignCompositeTag = new KryptonContextMenuItem();
                        kryptonContextMenuItemAssignCompositeTag.Text = "Prioity normal - 100";
                        kryptonContextMenuItemAssignCompositeTag.Tag = new MetadataPriorityValues(tag.Value, 100);
                        kryptonContextMenuItemAssignCompositeTag.Click += KryptonContextMenuItemAssignCompositeTag_Click;
                        kryptonContextMenuItemAssignCompositeTagSubList.Items.Add(kryptonContextMenuItemAssignCompositeTag);

                        kryptonContextMenuItemAssignCompositeTag = new KryptonContextMenuItem();
                        kryptonContextMenuItemAssignCompositeTag.Text = "Prioity medium high - 150";
                        kryptonContextMenuItemAssignCompositeTag.Tag = new MetadataPriorityValues(tag.Value, 150);
                        kryptonContextMenuItemAssignCompositeTag.Click += KryptonContextMenuItemAssignCompositeTag_Click;
                        kryptonContextMenuItemAssignCompositeTagSubList.Items.Add(kryptonContextMenuItemAssignCompositeTag);

                        kryptonContextMenuItemAssignCompositeTag = new KryptonContextMenuItem();
                        kryptonContextMenuItemAssignCompositeTag.Text = "Prioity high - 200";
                        kryptonContextMenuItemAssignCompositeTag.Tag = new MetadataPriorityValues(tag.Value, 200);
                        kryptonContextMenuItemAssignCompositeTag.Click += KryptonContextMenuItemAssignCompositeTag_Click;
                        kryptonContextMenuItemAssignCompositeTagSubList.Items.Add(kryptonContextMenuItemAssignCompositeTag);
                        break;
                }
            }
        }
        #endregion

        #region ActionAssignCompositeTag
        private void ActionAssignCompositeTag(DataGridView dataGridView, MetadataPriorityValues metadataPriorityValues)
        {           
            List<int> rows = DataGridViewHandler.GetRowSelected(dataGridView);
            foreach (int rowIndex in rows)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader && dataGridViewGenericRow.MetadataPriorityKey != null)
                {
                    exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary[dataGridViewGenericRow.MetadataPriorityKey] = metadataPriorityValues;

                    MetadataPriorityGroup metadataPriorityGroup = new MetadataPriorityGroup(
                        dataGridViewGenericRow.MetadataPriorityKey,
                        exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary[dataGridViewGenericRow.MetadataPriorityKey]);

                    bool priorityKeyExisit = exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary.ContainsKey(dataGridViewGenericRow.MetadataPriorityKey);
                    if (exiftoolReader.MetadataReadPrioity.MetadataPrioityDictionary[dataGridViewGenericRow.MetadataPriorityKey].Composite == CompositeTags.NotDefined) priorityKeyExisit = false;

                    if (priorityKeyExisit)
                        DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, metadataPriorityGroup.ToString());
                    else
                        DataGridViewHandler.SetRowToolTipText(dataGridView, rowIndex, "");

                    DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, rowIndex,
                        new DataGridViewGenericRow(
                            dataGridViewGenericRow.HeaderName,
                            dataGridViewGenericRow.RowName,
                            dataGridViewGenericRow.IsMultiLine,
                            dataGridViewGenericRow.MetadataPriorityKey));
                    DataGridViewHandler.SetRowFavoriteFlag(dataGridView, rowIndex);
                    DataGridViewHandler.SetCellBackGroundColorForRow(dataGridView, rowIndex);
                }
            }
            DataGridViewHandler.Refresh(dataGridView);
            exiftoolReader.MetadataReadPrioity.WriteAlways();
        }
        #endregion

        #region KryptonContextMenuItemAssignCompositeTag_Click
        private void KryptonContextMenuItemAssignCompositeTag_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            MetadataPriorityValues metadataPriorityValues = (MetadataPriorityValues)(((KryptonContextMenuItem)sender).Tag);
            
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
                    break;
                case KryptonPages.kryptonPageToolboxPeople:
                    break;
                case KryptonPages.kryptonPageToolboxMap:
                    break;
                case KryptonPages.kryptonPageToolboxDates:
                    break;
                case KryptonPages.kryptonPageToolboxExiftool:
                    ActionAssignCompositeTag(dataGridView, metadataPriorityValues);
                    break;
                case KryptonPages.kryptonPageToolboxWarnings:
                    ActionAssignCompositeTag(dataGridView, metadataPriorityValues);
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

        #endregion 

        //----
        #region DataGridView - SetGridViewSize Small / Medium / Big - Click

        #region SetRibbonDataGridViewSizeBottons
        private void SetRibbonDataGridViewSizeBottons(DataGridViewSize size, bool enabled)
        {
            switch (size)
            {
                case DataGridViewSize.ConfigSize:
                    break;
                case DataGridViewSize.Large:
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    kryptonRibbonGroupButtonDataGridViewCellSizeBig.Checked = true;
                    kryptonRibbonGroupButtonDataGridViewCellSizeMedium.Checked = false;
                    kryptonRibbonGroupButtonDataGridViewCellSizeSmall.Checked = false;
                    break;
                case DataGridViewSize.Medium:
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    kryptonRibbonGroupButtonDataGridViewCellSizeBig.Checked = false;
                    kryptonRibbonGroupButtonDataGridViewCellSizeMedium.Checked = true;
                    kryptonRibbonGroupButtonDataGridViewCellSizeSmall.Checked = false;
                    break;
                case DataGridViewSize.Small:
                case DataGridViewSize.Small | DataGridViewSize.RenameConvertAndMergeSize:
                    kryptonRibbonGroupButtonDataGridViewCellSizeBig.Checked = false;
                    kryptonRibbonGroupButtonDataGridViewCellSizeMedium.Checked = false;
                    kryptonRibbonGroupButtonDataGridViewCellSizeSmall.Checked = true;
                    break;
            }
            kryptonRibbonGroupButtonDataGridViewCellSizeBig.Enabled = enabled;
            kryptonRibbonGroupButtonDataGridViewCellSizeMedium.Enabled = enabled;
            kryptonRibbonGroupButtonDataGridViewCellSizeSmall.Enabled = enabled;
        }
        #endregion

        #region SetGridViewSize
        private void SetGridViewSize(DataGridViewSize size)
        {
            SetRibbonDataGridViewSizeBottons(size, true);

            switch (GetActiveTabTag())
            {
                case LinkTabAndDataGridViewNameTags:
                    DataGridViewHandler.SetCellSize(dataGridViewTagsAndKeywords, size, false);
                    Properties.Settings.Default.CellSizeKeywords = (int)size;
                    break;
                case LinkTabAndDataGridViewNameMap:
                    DataGridViewHandler.SetCellSize(dataGridViewMap, size, false);
                    Properties.Settings.Default.CellSizeMap = (int)size;
                    break;
                case LinkTabAndDataGridViewNamePeople:
                    DataGridViewHandler.SetCellSize(dataGridViewPeople, size, true);
                    Properties.Settings.Default.CellSizePeoples = (int)size;
                    break;
                case LinkTabAndDataGridViewNameDates:
                    DataGridViewHandler.SetCellSize(dataGridViewDate, size, false);
                    Properties.Settings.Default.CellSizeDates = (int)size;
                    break;
                case LinkTabAndDataGridViewNameExiftool:
                    DataGridViewHandler.SetCellSize(dataGridViewExiftool, size, false);
                    Properties.Settings.Default.CellSizeExiftool = (int)size;
                    break;
                case LinkTabAndDataGridViewNameWarnings:
                    DataGridViewHandler.SetCellSize(dataGridViewExiftoolWarning, size, false);
                    Properties.Settings.Default.CellSizeWarnings = (int)size;
                    break;
                case LinkTabAndDataGridViewNameProperties:
                    DataGridViewHandler.SetCellSize(dataGridViewProperties, size, false);
                    Properties.Settings.Default.CellSizeProperties = (int)size;
                    break;
                case LinkTabAndDataGridViewNameRename:
                    DataGridViewHandler.SetCellSize(dataGridViewRename, (size | DataGridViewSize.RenameConvertAndMergeSize), false);
                    Properties.Settings.Default.CellSizeRename = (int)size;
                    break;
                case LinkTabAndDataGridViewNameConvertAndMerge:
                    DataGridViewHandler.SetCellSize(dataGridViewConvertAndMerge, (size | DataGridViewSize.RenameConvertAndMergeSize), false);
                    Properties.Settings.Default.CellSizeConvertAndMerge = (int)size;
                    break;
                default: throw new Exception("Not implemented");
            }
        }
        #endregion

        #region DataGridViewCellSizeXYZ_Click
        private void kryptonRibbonGroupButtonDataGridViewCellSizeBig_Click(object sender, EventArgs e)
        {
            try
            {
                SetGridViewSize(DataGridViewSize.Large);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kryptonRibbonGroupButtonDataGridViewCellSizeMedium_Click(object sender, EventArgs e)
        {
            try
            {
                SetGridViewSize(DataGridViewSize.Medium);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kryptonRibbonGroupButtonDataGridViewCellSizeSmall_Click(object sender, EventArgs e)
        {
            try
            {
                SetGridViewSize(DataGridViewSize.Small);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region ColumnWidthChanged

        #region ColumnWidthChanged - imageListView1_ColumnWidthChanged
        private void imageListView1_ColumnWidthChanged(object sender, ColumnEventArgs e)
        {
            Properties.Settings.Default.ColumnNameAndWithsImageListView = ColumnNamesAndWidthHandler.ConvertColumnNameAndWidthsToConfigString(ColumnNamesAndWidthHandler.GetColumnNameAndWidths(imageListView1));
        }
        #endregion

        #region ColumnWidthChanged - dataGridViewRename_ColumnWidthChanged
        private void dataGridViewRename_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;
            if (DataGridViewHandler.GetIsPopulationgCellSize(dataGridView)) return;

            DataGridViewHandler.UpdatedCacheColumnsWidth(dataGridView);
            DataGridViewSize dataGridViewSize = DataGridViewHandler.GetDataGridSizeLargeMediumSmall(dataGridView);
            List<ColumnNameAndWidth> columnNameAndWidths = DataGridViewHandler.GetColumnNameAndWidths(dataGridView, dataGridViewSize);
            string configXml = ColumnNamesAndWidthHandler.ConvertColumnNameAndWidthsToConfigString(columnNameAndWidths);
            switch (dataGridViewSize)
            {
                case DataGridViewSize.Small:
                case DataGridViewSize.Small | DataGridViewSize.RenameConvertAndMergeSize:
                    Properties.Settings.Default.ColumnNameAndWithsRenameSmall = configXml;
                    break;
                case DataGridViewSize.Medium:
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    Properties.Settings.Default.ColumnNameAndWithsRenameMedium = configXml;
                    break;
                case DataGridViewSize.Large:
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    Properties.Settings.Default.ColumnNameAndWithsRenameLarge = configXml;
                    break;
                case DataGridViewSize.ConfigSize:
                    break;
                default:
                    throw new Exception("Not implemented");
            }

        }
        #endregion

        #region ColumnWidthChanged - dataGridViewConvertAndMerge_ColumnWidthChanged
        private void dataGridViewConvertAndMerge_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;
            if (DataGridViewHandler.GetIsPopulationgCellSize(dataGridView)) return;

            DataGridViewHandler.UpdatedCacheColumnsWidth(dataGridView);
            DataGridViewSize dataGridViewSize = DataGridViewHandler.GetDataGridSizeLargeMediumSmall(dataGridView);
            List<ColumnNameAndWidth> columnNameAndWidths = DataGridViewHandler.GetColumnNameAndWidths(dataGridView, dataGridViewSize);
            string configXml = ColumnNamesAndWidthHandler.ConvertColumnNameAndWidthsToConfigString(columnNameAndWidths);
            switch (dataGridViewSize)
            {
                case DataGridViewSize.Small:
                case DataGridViewSize.Small | DataGridViewSize.RenameConvertAndMergeSize:
                    Properties.Settings.Default.ColumnNameAndWithsConvertAndMergeSmall = configXml;
                    break;
                case DataGridViewSize.Medium:
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    Properties.Settings.Default.ColumnNameAndWithsConvertAndMergeMedium = configXml;
                    break;
                case DataGridViewSize.Large:
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    Properties.Settings.Default.ColumnNameAndWithsConvertAndMergeLarge = configXml;
                    break;
                case DataGridViewSize.ConfigSize:
                    break;
                default:
                    throw new Exception("Not implemented");
            }
        }
        #endregion

        #endregion

        #region DataGridView - Show/Hide Historiy / Error Columns - Click

        #region SetRibbonDataGridViewShowWhatColumns
        private void SetRibbonDataGridViewShowWhatColumns(ShowWhatColumns showWhatColumns, bool enabled = true)
        {
            SetRibbonGridViewColumnsButtonsHistoricalAndError(ShowWhatColumnHandler.ShowHirstoryColumns(showWhatColumns), ShowWhatColumnHandler.ShowErrorColumns(showWhatColumns));
            kryptonRibbonGroupButtonDataGridViewColumnsHistory.Enabled = enabled;
            kryptonRibbonGroupButtonDataGridViewColumnsErrors.Enabled = enabled;
        }
        #endregion

        #region SetRibbonGridViewColumnsButtonsHistoricalAndError
        private void SetRibbonGridViewColumnsButtonsHistoricalAndError(bool showHistorical, bool showErrors)
        {
            kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked = showHistorical;
            kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked = showErrors;
        }
        #endregion

        #region DataGridViewColumnsXyz_Click
        private void kryptonRibbonGroupButtonDataGridViewColumnsHistory_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.ShowHistortyColumns = kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked;
                showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked, kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked);
                LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(GetSelectedFileEntriesImageListView());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kryptonRibbonGroupButtonDataGridViewColumnsErrors_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.ShowErrorColumns = kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked;
                showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked, kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked);
                LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(GetSelectedFileEntriesImageListView());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region ImageListView - Switch Renderers 

        #region RendererItem
        private struct RendererItem
        {
            public Type Type;
            public override string ToString()
            {
                return Type.Name;
            }

            public RendererItem(Type type)
            {
                Type = type;
            }
        }
        #endregion

        #region SetImageListViewRender
        private void SetImageListViewRender(Manina.Windows.Forms.View imageListViewViewMode, RendererItem selectedRender)
        {
            try
            {
                Properties.Settings.Default.ImageListViewRendererName = selectedRender.Type.Name;
                Properties.Settings.Default.ImageListViewViewMode = (int)imageListViewViewMode;

                imageListView1.View = imageListViewViewMode;
                Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
                ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(selectedRender.Type.FullName) as ImageListView.ImageListViewRenderer;
                imageListView1.SetRenderer(renderer);
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        #endregion

        #region SwitchRenderers_Click
        private void KryptonContextMenuItemRenderers_Click(object sender, EventArgs e)
        {
            KryptonContextMenuItem kryptonContextMenuItem = (KryptonContextMenuItem)sender;
            SetImageListViewRender(Manina.Windows.Forms.View.Thumbnails, (RendererItem)kryptonContextMenuItem.Tag);
        }

        private void kryptonRibbonGroupButtonImageListViewModeGallery_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Gallery, imageListViewSelectedRenderer);
        }

        private void kryptonRibbonGroupButtonImageListViewModeDetails_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Details, imageListViewSelectedRenderer);
        }

        private void kryptonRibbonGroupButtonImageListViewModePane_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Pane, imageListViewSelectedRenderer);
        }

        private void kryptonRibbonGroupButtonImageListViewModeThumbnails_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Thumbnails, imageListViewSelectedRenderer);
        }

        #endregion

        #endregion

        #region ImageListView - ChooseColumns
        private void kryptonRibbonGroupButtonImageListViewDetailviewColumns_Click(object sender, EventArgs e)
        {
            try
            {
                FormChooseColumns form = new FormChooseColumns();
                form.imageListView = imageListView1;
                int index = 0;
                if (imageListView1.View == Manina.Windows.Forms.View.Thumbnails) index = 1;
                form.Populate(index);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ImageListView - Change Thumbnail Size 

        #region SetThumbnailSize
        private void SetThumbnailSize(int size)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[size];
            Properties.Settings.Default.ThumbmailViewSizeIndex = size;
            kryptonRibbonGroupButtonThumbnailSizeXLarge.Checked = (size == 4);
            kryptonRibbonGroupButtonThumbnailSizeLarge.Checked = (size == 3);
            kryptonRibbonGroupButtonThumbnailSizeMedium.Checked = (size == 2);
            kryptonRibbonGroupButtonThumbnailSizeSmall.Checked = (size == 1);
            kryptonRibbonGroupButtonThumbnailSizeXSmall.Checked = (size == 0);
        }
        #endregion

        #region ThumbnailSizeZYZ_Click
        private void kryptonRibbonGroupButtonThumbnailSizeXLarge_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(4);
        }

        private void kryptonRibbonGroupButtonThumbnailSizeLarge_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(3);
        }

        private void kryptonRibbonGroupButtonThumbnailSizeMedium_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(2);
        }

        private void kryptonRibbonGroupButtonThumbnailSizeSmall_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(1);
        }

        private void kryptonRibbonGroupButtonThumbnailSizeXSmall_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(0);
        }
        #endregion

        #endregion

        #region ImageListView - Sort

        #region ImageListViewSortColumn
        private void ImageListViewSortColumn(ImageListView imageListView, ColumnType columnToSort)
        {
            try
            {
                if (imageListView.SortColumn == columnToSort)
                {
                    if (imageListView.SortOrder == SortOrder.Descending) imageListView.SortOrder = SortOrder.Ascending;
                    else imageListView.SortOrder = SortOrder.Descending;
                }
                else
                {
                    imageListView.SortColumn = columnToSort;
                    imageListView.SortOrder = SortOrder.Ascending;
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FileSystemColumnSortXyz_Click
        private void KryptonContextMenuRadioButtonFileSystemColumnSortFilename_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.FileName);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.FileSmartDate);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFileDate_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.FileDate);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.FileDateCreated);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.FileDateModified);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.MediaDateTaken);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.MediaAlbum);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.MediaTitle);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.MediaDescription);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaComments_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.MediaComment);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.MediaAuthor);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaRating_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.MediaRating);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationName_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.LocationName);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.LocationRegionState);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationCity_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.LocationCity);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry_Click(object sender, EventArgs e)
        {
            ImageListViewSortColumn(imageListView1, ColumnType.LocationCountry);
        }
        #endregion

        #endregion

        //--
        #region ImageListView - Select all 

        #region ActionSelectAll
        private void ActionSelectAll()
        {
            try
            {
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
                using (new WaitCursor())
                {
                    imageListView1.SelectAll();
                }
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                FilesSelectedOrNoneSelected();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region SelectAll_Click
        private void kryptonRibbonQATButtonSelectAll_Click(object sender, EventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            ActionSelectAll();
        }

        private void kryptonRibbonGroupButtonSelectAll_Click(object sender, EventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            ActionSelectAll();
        }
        #endregion

        #endregion 

        #region ImageListView - Select None

        #region ActionSelectNone
        private void ActionSelectNone()
        {
            try
            {
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
                using (new WaitCursor())
                {
                    imageListView1.ClearSelection();
                }
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                FilesSelectedOrNoneSelected();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region SelectNone_Click
        private void kryptonRibbonQATButtonSelectNone_Click(object sender, EventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            ActionSelectNone();
        }

        private void kryptonRibbonGroupButtonSelectNone_Click(object sender, EventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            ActionSelectNone();
        }
        #endregion

        #endregion

        #region ImageListView - Select Toggle

        #region ActionSelectToggle
        private void ActionSelectToggle()
        {
            try
            {
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
                using (new WaitCursor())
                {
                    imageListView1.SuspendLayout();
                    foreach (ImageListViewItem imageListViewItem in imageListView1.Items) imageListViewItem.Selected = !imageListViewItem.Selected;
                    imageListView1.ResumeLayout();
                }
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                FilesSelectedOrNoneSelected();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region SelectToggle_Click
        private void kryptonRibbonGroupButtonSelectToggle_Click(object sender, EventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            ActionSelectToggle();
        }

        private void kryptonRibbonQATButtonSelectToggle_Click(object sender, EventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            ActionSelectToggle();
        }
        #endregion

        #endregion

        //---- Tools Progress and Status 
        #region StatusActionText
        private string StatusActionText
        {
            get { return kryptonRibbonGroupLabelToolsCurrentActions.TextLine1; }
            set { kryptonRibbonGroupLabelToolsCurrentActions.TextLine1 = value; }
        }
        #endregion

        #region ProgressbarSaveAndConvertProgress

        #region ProgressbarSaveAndConvertProgress(bool enabled, int value)
        private void ProgressbarSaveAndConvertProgress(bool enabled, int value)
        {
            progressBarSaveConvert.Value = value;
            ProgressbarSaveProgress(enabled);
        }
        #endregion

        #region ProgressbarSaveProgress(bool enabled, int value, int minimum, int maximum, string descrption)
        private void ProgressbarSaveAndConvertProgress(bool enabled, int value, int minimum, int maximum, string descrption)
        {
            progressBarSaveConvert.Minimum = minimum;
            progressBarSaveConvert.Maximum = maximum;
            progressBarSaveConvert.Value = value;
            ProgressbarSaveProgress(enabled);
        }
        #endregion

        #region ProgressbarSaveProgress(bool visible)
        private void ProgressbarSaveProgress(bool visible)
        {
            kryptonRibbonGroupTripleProgressStatusSave.Visible = visible;
            kryptonRibbonGroupCustomControlToolsProgressSave.Visible = visible;
            kryptonRibbonGroupLabelToolsProgressSave.Visible = visible;
            kryptonRibbonGroupLabelToolsProgressSaveText.Visible = visible;
        }
        #endregion

        #endregion

        

        #region Progress Laxy Loading for DataGridView

        #region SeeProcessQueue_Clcik
        private void kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus_Click(object sender, EventArgs e)
        {
            ActionSeeProcessQueue();
        }

        private void buttonSpecNavigatorDataGridViewProgressCircle_Click(object sender, EventArgs e)
        {
            ActionSeeProcessQueue();
        }

        private void buttonSpecNavigatorImageListViewLoadStatus_Click(object sender, EventArgs e)
        {
            ActionSeeProcessQueue();
        }
        #endregion


        #endregion

        //----
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

            if (((KryptonDataGridView)sender)[e.ColumnIndex, e.RowIndex].Value is RegionStructure regionStructure) regionStructure.ShowNameInToString = true; //Just a hack so KryptonDataGridView don't print name alse

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

            DataGridViewGenericColumn dataGridViewGenericDataColumn = null;
            if (e.ColumnIndex > -1)
            {
                dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
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
                    DataGridViewHandler.DrawImageAndSubText(sender, e, regionThumbnail, ((RegionStructure)e.Value).Name);

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
