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
using FileHandeling;
using PhotoTagsSynchronizer.Properties;

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
    public partial class MainForm : KryptonForm
    {
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region FolderCut_Click
        private void FolderCut_Click()
        {
            try
            {
                string folder = GetNodeFolderRealPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
                if (!Directory.Exists (folder))
                {
                    KryptonMessageBox.Show("Not a valid folder selected. Try select anoter folder.\r\nSelected system folder: " + (string.IsNullOrWhiteSpace(folder) ? "Unknown" : folder), "Invalid folder selection...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
}
        #endregion

        #region DataGridGeneric_Cut
        private void DataGridGeneric_Cut(DataGridView dataGridView)
        {
            try
            {
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true);
                ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
                DataGridViewHandler.Refresh(dataGridView);
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ConvertAndMergeCut_Click
        private void ConvertAndMergeCut_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridGeneric_Cut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateCut_Click
        private void DateCut_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridGeneric_Cut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolCut_Click
        private void ExiftoolCut_Click()
        {
            try {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridGeneric_Cut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningCut_Click
        private void ExiftoolWarningCut_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridGeneric_Cut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapCut_Click
        private void MapCut_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridGeneric_Cut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleCut_Click
        private void PeopleCut_Click()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesCut_Click
        private void PropertiesCut_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridGeneric_Cut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameCut_Click
        private void RenameCut_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridGeneric_Cut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsCut_Click
        private void TagsAndKeywordsCut_Click()
        {
            try
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
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region FolderTree - Copy - Click
        private void FolderCopy_Click()
        {
            try
            {
                string folder = GetNodeFolderRealPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
                if (!Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Not a valid folder selected. Try select anoter folder.\r\nSelected system folder: " + (string.IsNullOrWhiteSpace(folder) ? "Unknown" : folder), "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }

        }
        #endregion

        #region DataGridGeneric_Copy
        private void DataGridGeneric_Copy(DataGridView dataGridView)
        {
            try
            {
                if (!dataGridView.Enabled) return;
                ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ConvertAndMergeCopy_Click
        private void ConvertAndMergeCopy_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridGeneric_Copy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateCopy_Click
        private void DateCopy_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridGeneric_Copy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolCopy_Click
        private void ExiftoolCopy_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridGeneric_Copy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningCopy_Click
        private void ExiftoolWarningCopy_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridGeneric_Copy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapCopy_Click
        private void MapCopy_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridGeneric_Copy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleCopy_Click
        private void PeopleCopy_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                DataGridGeneric_Copy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
}
        #endregion

        #region PropertiesCopy_Click
        private void PropertiesCopy_Click()
        {
            try { 
            DataGridView dataGridView = dataGridViewProperties;
            DataGridGeneric_Copy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameCopy_Click
        private void RenameCopy_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridGeneric_Copy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region TagsAndKeywords_Click
        private void TagsAndKeywordsCopy_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                DataGridGeneric_Copy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                            if (FilesCutCopyPasteDrag.IsFilenameEqual(item.FileFullPath, fullFilename))
                            {
                                fileFound = true;
                                break;
                            }
                        }

                        if (!fileFound) ImageListViewHandler.ImageListViewAddItem(imageListView1, fullFilename);
                    }
                }
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                    string targetFolder = GetNodeFolderRealPath(targetTreeNode as TreeNodePath);

                    if (!Directory.Exists(targetFolder))
                    {
                        KryptonMessageBox.Show("Not a valid target folder selected. Try select anoter folder.\r\nSelected system folder: " + (string.IsNullOrWhiteSpace(targetFolder) ? "Unknown" : targetFolder), "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                        return;
                    }

                    StringCollection filesOrFolders = Clipboard.GetFileDropList();
                    CopyOrMove_UpdatedBrowserTreeView(dragDropEffects, filesOrFolders, targetFolder, treeViewFolderBrowser1.SelectedNode);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericPaste
        private void DataGridViewGenrericPaste(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion 

        #region ConvertAndMerge
        private void ConvertAndMergePaste_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericPaste(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Date
        private void DatePaste_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericPaste(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Exiftool
        private void ExiftoolPaste_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericPaste(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarning
        private void ExiftoolWarningPaste_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericPaste(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Map
        private void MapPaste_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericPaste(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region People
        private void PeoplePaste_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, -1, -1, -1, -1, false);
                ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region Properties
        private void PropertiesPaste_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericPaste(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Rename
        private void RenamePaste_Click()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywords
        private void TagsAndKeywordsPaste_Click()
        {
            try
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
                    try
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
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                    }
                    finally
                    {
                        GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
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
            try
            {
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        FileSystemFolderDelete_Click();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        FileSystemSelectedFilesDelete_Click();
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region FileSystemSelectedFilesDelete_Click
        private void FileSystemSelectedFilesDelete_Click()
        {            
            try
            {
                if (GlobalData.IsPopulatingAnything()) return;
                if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, false);
                ImageListViewHandler.Enable(imageListView1, false);

                try
                {
                    if (IsFileInThreadQueueLock(imageListView1))
                    {
                        KryptonMessageBox.Show("Can't delete files. Files are being used, you need wait until process is finished.", "File in queue...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                        return;
                    }

                    if (KryptonMessageBox.Show("Are you sure you will delete the files", "Files will be deleted!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, showCtrlCopy: true) == DialogResult.Yes)
                    {
                        using (new WaitCursor())
                        {
                            UpdateStatusAction("Deleing files and all record about files in database....");
                            HashSet<FileEntry> fileEntries = ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true);
                            filesCutCopyPasteDrag.DeleteSelectedFiles(this, imageListView1, fileEntries, true);
                            ImageListViewHandler.ClearCacheFileEntries(imageListView1);
                            OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                }

                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, true);
                ImageListViewHandler.Enable(imageListView1, true);
                imageListView1.Focus();
                DisplayAllQueueStatus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }

        }
        #endregion

        #region FileSystemFolderDelete_Click
        private void FileSystemFolderDelete_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                string folder = GetSelectedNodeFullRealPath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't delete folder. No valid folder selected.", "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }

                if (IsFolderInThreadQueueLock(folder))
                {
                    KryptonMessageBox.Show("Can't delete folder. Files in folder is been used, you need wait until process is finished.", "File are in queue....", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                    return;
                }
                try
                {
                    string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories).Take(51).ToArray();

                    if (
                        (fileAndFolderEntriesCount.Length == 0) || //No need to ask, when zero files
                        (KryptonMessageBox.Show("You are about to delete the folder:\r\n\r\n" +
                        folder + "\r\n\r\n" +
                        "There are " + (fileAndFolderEntriesCount.Length == 51 ? " over 50+" : fileAndFolderEntriesCount.Length.ToString()) + " files found.\r\n\r\n" +
                        "Procced?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, showCtrlCopy: true) == DialogResult.OK)
                        )
                    {
                        using (new WaitCursor())
                        {
                            UpdateStatusAction("Delete all record about files in database....");
                            int recordAffected = filesCutCopyPasteDrag.DeleteFilesInFolder(this, treeViewFolderBrowser1, folder);
                            UpdateStatusAction(recordAffected + " records was delete from database....");
                            ImageListView_Aggregate_FromFolder(false, true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericDelete
        private void DataGridViewGenrericDelete(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region ConvertAndMergeDelete_Click
        private void ConvertAndMergeDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region DateDelete_Click
        private void DateDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ExiftoolDelete_Click
        private void ExiftoolDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ExiftoolWarningDelete_Click
        private void ExiftoolWarningDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region MapDelete_Click
        private void MapDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleDelete_Click
        private void PeopleDelete_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region PropertiesDelete_Click
        private void PropertiesDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameDelete_Click
        private void RenameDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsDelete_Click()
        private void TagsAndKeywordsDelete_Click()
        {
            try
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
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericUndo
        private void DataGridViewGenrericUndo(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.UndoDataGridView(dataGridView);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region ConvertAndMergeUndo_Click
        private void ConvertAndMergeUndo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericUndo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateUndo_Click
        private void DateUndo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericUndo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ExiftoolUndo_Click
        private void ExiftoolUndo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericUndo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningUndo_Click
        private void ExiftoolWarningUndo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericUndo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Map
        private void MapUndo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericUndo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleUndo_Click
        private void PeopleUndo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.UndoDataGridView(dataGridView);
                ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region PropertiesUndo_Click
        private void PropertiesUndo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericUndo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameUndo_Click
        private void RenameUndo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericUndo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywords
        private void TagsAndKeywordsUndo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
                ClipboardUtility.UndoDataGridView(dataGridView);
                ValitedatePasteKeywords(dataGridView, header);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericRedo
        private void DataGridViewGenrericRedo(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
                ClipboardUtility.RedoDataGridView(dataGridView);
                //ValitedatePaste(dataGridView, header);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region ConvertAndMergepRedo_Click
        private void ConvertAndMergeRedo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericRedo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateRedo_Click
        private void DateRedo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericRedo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ExiftoolRedo_Click
        private void ExiftoolRedo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericRedo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningRedo_Click
        private void ExiftoolWarningRedo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericRedo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapRedo_Click
        private void MapRedo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericRedo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleRedo_Click
        private void PeopleRedo_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.RedoDataGridView(dataGridView);
                ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region PropertiesRedo_Click
        private void PropertiesRedo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericRedo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameRedo_Click
        private void RenameRedo_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericRedo(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsRedo_Click
        private void TagsAndKeywordsRedo_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
                ClipboardUtility.RedoDataGridView(dataGridView);
                ValitedatePasteKeywords(dataGridView, header);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region FolderSearchFilterFolderFind_Click
        private void FolderSearchFilterFolderFind_Click()
        {
            try
            {
                kryptonWorkspaceCellFolderSearchFilter.SelectedPage = kryptonPageFolderSearchFilterSearch;
                kryptonTextBoxSearchDirectory.Text = GetSelectedNodeFullRealPath() == null ? "" : GetSelectedNodeFullRealPath();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            try
            {
                kryptonWorkspaceCellFolderSearchFilter.SelectedPage = kryptonPageFolderSearchFilterSearch;
                
                if (imageListView1.SelectedItems.Count == 1)
                {
                    PopulateDatabaseFilter();

                    kryptonTextBoxSearchDirectory.Text = imageListView1.SelectedItems[0].FileDirectory;
                    kryptonTextBoxSearchFilename.Text = imageListView1.SelectedItems[0].Text;

                    dateTimePickerSearchDateFrom.Value = DateTime.Now;
                    dateTimePickerSearchDateTo.Value = DateTime.Now;

                    Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(
                        new FileEntryBroker(imageListView1.SelectedItems[0].FileFullPath, imageListView1.SelectedItems[0].DateModified, MetadataBrokerType.ExifTool));
                    if (metadata != null && metadata.MediaDateTaken != null)
                    {
                        dateTimePickerSearchDateFrom.Value = (DateTime)metadata.MediaDateTaken;
                        dateTimePickerSearchDateTo.Value = (DateTime)metadata.MediaDateTaken;
                    }

                }
                else if (imageListView1.SelectedItems.Count >= 1)
                {
                    kryptonTextBoxSearchDirectory.Text = "";
                    kryptonTextBoxSearchFilename.Text = "";
                    DateTime? dateTimeFrom = null;
                    DateTime? dateTimeTo = null;
                    HashSet<FileEntry> fileEntries = ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true);
                    foreach (FileEntry fileEntry in fileEntries)
                    {
                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                        if (metadata != null && metadata.MediaDateTaken != null)
                        {
                            if (dateTimeFrom == null) dateTimeFrom = metadata.MediaDateTaken;
                            else if (metadata.MediaDateTaken < dateTimeFrom) dateTimeFrom = metadata.MediaDateTaken;
                            if (dateTimeTo == null) dateTimeTo = metadata.MediaDateTaken;
                            else if (metadata.MediaDateTaken > dateTimeTo) dateTimeTo = metadata.MediaDateTaken;
                        }
                    }
                    if (dateTimeFrom != null) dateTimePickerSearchDateFrom.Value = (DateTime)dateTimeFrom;
                    if (dateTimeTo != null) dateTimePickerSearchDateTo.Value = (DateTime)dateTimeTo;

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericFind
        private void DataGridViewGenrericFind(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
                //ValitedatePaste(dataGridView, header);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region ConvertAndMergeFind_Click
        private void ConvertAndMergeFind_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericFind(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateFind_Click
        private void DateFind_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericFind(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ExiftoolFind_Click
        private void ExiftoolFind_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericFind(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningFind_Click
        private void ExiftoolWarningFind_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericFind(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapFind_Click
        private void MapFind_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericFind(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleFind_Click
        private void PeopleFind_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
                ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
                DataGridViewHandler.Refresh(dataGridView);
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesFind_Click
        private void PropertiesFind_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericFind(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameFind_Click
        private void RenameFind_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericFind(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsFind_Click
        private void TagsAndKeywordsFind_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
                DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
                ValitedatePasteKeywords(dataGridView, header);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericFindAndReplace
        private void DataGridViewGenrericFindAndReplace(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
                //ValitedatePaste(dataGridView, header);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region ConvertAndMergeFindAndReplace_Click
        private void ConvertAndMergeFindAndReplace_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericFindAndReplace(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region DateFindAndReplace_Click
        private void DateFindAndReplace_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericFindAndReplace(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ExiftoolFindAndReplace_Click
        private void ExiftoolFindAndReplace_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericFindAndReplace(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningFindAndReplace_Click
        private void ExiftoolWarningFindAndReplace_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericFindAndReplace(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region MapFindAndReplace_Click
        private void MapFindAndReplace_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericFindAndReplace(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleFindAndReplace_Click
        private void PeopleFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
                ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
        }
        #endregion

        #region PropertiesFindAndReplace_Click
        private void PropertiesFindAndReplace_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericFindAndReplace(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameFindAndReplace_Click
        private void RenameFindAndReplace_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericFindAndReplace(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsFindAndReplace_Click
        private void TagsAndKeywordsFindAndReplace_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
                DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
                ValitedatePasteKeywords(dataGridView, header);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteAdd
        private void DataGridViewGenrericFavoriteAdd(DataGridView dataGridView)
        {
            try
            {
                DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
                DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ConvertAndMergeFavoriteAdd_Click
        private void ConvertAndMergeFavoriteAdd_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericFavoriteAdd(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateFavoriteAdd_Click
        private void DateFavoriteAdd_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericFavoriteAdd(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolFavoriteAdd_Click
        private void ExiftoolFavoriteAdd_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericFavoriteAdd(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningFavoriteAdd_Click
        private void ExiftoolWarningFavoriteAdd_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericFavoriteAdd(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapFavoriteAdd_Click
        private void MapFavoriteAdd_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericFavoriteAdd(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleFavoriteAdd_Click
        private void PeopleFavoriteAdd_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                DataGridViewGenrericFavoriteAdd(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesFavoriteAdd_Click
        private void PropertiesFavoriteAdd_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericFavoriteAdd(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameFavoriteAdd_Click
        private void RenameFavoriteAdd_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericFavoriteAdd(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsFavoriteAdd_Click
        private void TagsAndKeywordsFavoriteAdd_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                DataGridViewGenrericFavoriteAdd(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteDelete
        private void DataGridViewGenrericFavoriteDelete(DataGridView dataGridView)
        {
            try
            {
                DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
                DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
}
        #endregion

        #region ConvertAndMergeFavoriteDelete_Click
        private void ConvertAndMergeFavoriteDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericFavoriteDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateFavoriteDelete_Click
        private void DateFavoriteDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericFavoriteDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ExiftoolFavoriteDelete_Click
        private void ExiftoolFavoriteDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericFavoriteDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningFavoriteDelete_Click
        private void ExiftoolWarningFavoriteDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericFavoriteDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapFavoriteDelete_Click
        private void MapFavoriteDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericFavoriteDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleFavoriteDelete_Click
        private void PeopleFavoriteDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                DataGridViewGenrericFavoriteDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesFavoriteDelete_Click
        private void PropertiesFavoriteDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericFavoriteDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameFavoriteDelete_Click
        private void RenameFavoriteDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericFavoriteDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsFavoriteDelete_Click
        private void TagsAndKeywordsFavoriteDelete_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                DataGridViewGenrericFavoriteDelete(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteToogle
        private void DataGridViewGenrericFavoriteToogle(DataGridView dataGridView)
        {
            try
            {
                DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
                DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ConvertAndMergeFavoriteToogle_Click
        private void ConvertAndMergeFavoriteToogle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateFavoriteToogle_Click
        private void DateFavoriteToogle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ExiftoolFavoriteToogle_Click
        private void ExiftoolFavoriteToogle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningFavoriteToogle_Click
        private void ExiftoolWarningFavoriteToogle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapFavoriteToogle_Click
        private void MapFavoriteToogle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleFavoriteToogle_Click
        private void PeopleFavoriteToogle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                DataGridViewGenrericFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesFavoriteToogle_Click
        private void PropertiesFavoriteToogle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameFavoriteToogle_Click
        private void RenameFavoriteToogle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsFavoriteToogle_Click
        private void TagsAndKeywordsFavoriteToogle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                DataGridViewGenrericFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            try
            {
                kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Checked = hideEqualColumns;
                kryptonRibbonGroupButtonDataGridViewRowsFavorite.Checked = showFavouriteColumns;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ActionRowsShowFavoriteToggle
        private void ActionRowsShowFavoriteToggle()
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteToogle
        private void DataGridViewGenrericShowFavoriteToogle(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            try
            {
                DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
                UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
                DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ConvertAndMergeShowFavoriteToggle_Click
        private void ConvertAndMergeRowsShowFavoriteToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericShowFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region DateShowFavoriteToggle_Click
        private void DateRowsShowFavoriteToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericShowFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region ExiftoolShowFavoriteToggle_Click
        private void ExiftoolRowsShowFavoriteToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericShowFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningShowFavoriteToggle_Click
        private void ExiftoolWarningRowsShowFavoriteToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericShowFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapShowFavoriteToggle_Click
        private void MapRowsShowFavoriteToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericShowFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleShowFavoriteToggle_Click
        private void PeopleRowsShowFavoriteToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                DataGridViewGenrericShowFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesShowFavoriteToggle_Click
        private void PropertiesRowsShowFavoriteToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericShowFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameShowFavoriteToggle_Click
        private void RenameRowsShowFavoriteToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericShowFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywords
        private void TagsAndKeywordsRowsShowFavoriteToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                DataGridViewGenrericShowFavoriteToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewGenrericFavoriteToogle
        private void DataGridViewGenrericRowsHideEqualToogle(DataGridView dataGridView)
        {
            if (!dataGridView.Enabled) return;
            try {
                DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
                UpdateBottonsEqualAndFavorite(DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
                DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ConvertAndMergeRowsHideEqualToggle_Click
        private void ConvertAndMergeRowsHideEqualToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridViewGenrericRowsHideEqualToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateRowsHideEqualToggle_Click
        private void DateRowsHideEqualToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridViewGenrericRowsHideEqualToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolRowsHideEqualToggle_Click
        private void ExiftoolRowsHideEqualToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridViewGenrericRowsHideEqualToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningRowsHideEqualToggle_Click
        private void ExiftoolWarningRowsHideEqualToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridViewGenrericRowsHideEqualToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapRowsHideEqualToggle_Click
        private void MapRowsHideEqualToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewGenrericRowsHideEqualToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleRowsHideEqualToggle_Click
        private void PeopleRowsHideEqualToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                DataGridViewGenrericRowsHideEqualToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesRowsHideEqualToggle_Click
        private void PropertiesRowsHideEqualToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridViewGenrericRowsHideEqualToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameRowsHideEqualToggle_Click
        private void RenameRowsHideEqualToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridViewGenrericRowsHideEqualToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsRowsHideEqualToggle_Click
        private void TagsAndKeywordsRowsHideEqualToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                DataGridViewGenrericRowsHideEqualToogle(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region FolderCopyFolderNameToClipboard_Click
        private void FolderCopyNameToClipboard_Click()
        {
            try
            {
                string folder = GetSelectedNodeFullRealPath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't copy folder name. Not a valid folder selected.", "Invalid folder", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                    return;
                }
                Clipboard.SetText(folder);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region TriStateButtonClick - DataGridView - Keydown 

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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region DataGridViewGenericTagActionToggle
        private void DataGridViewGenericTagActionToggle(DataGridView dataGridView, string header, NewState newState)
        {
            if (!dataGridView.Enabled) return;
            try
            {
                DataGridViewHandler.ToggleSelected(dataGridView, header, newState);
                ValitedatePasteKeywords(dataGridView, header);
                DataGridViewHandler.Refresh(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsTriStateToggle_Click 
        private void TagsAndKeywordsTriStateToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Toggle);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleTriStateToggle_Click
        private void PeopleTriStateToggle_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Toggle);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region TagsAndKeywordsTriStateOn_Click
        private void TagsAndKeywordsTriStateOn_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            try
            {
                DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Set);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleTriStateOn_Click
        private void PeopleTriStateOn_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            try
            {
                DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Set);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsTriStateOff_Click
        private void TagsAndKeywordsTriStateOff_Click()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            try
            {
                DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Remove);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleTriStateOff_Click
        private void PeopleTriStateOff_Click()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            try
            {
                DataGridViewGenericTagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Remove);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Save - SaveDataGridViewMetadata
        private void SaveDataGridViewMetadata(bool useAutoCorrect)
        {
            if (GlobalData.IsPopulatingAnything())
            {
                KryptonMessageBox.Show("Data is populating, please try a bit later.", "Need wait...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                return;
            }
            if (!GlobalData.IsAgredagedGridViewAny())
            {
                KryptonMessageBox.Show("Nothing has been agredaged, nothing to do.", "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                return;
            }

            try
            {
                CollectMetadataFromAllDataGridViewData(out List <Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView, true);

                //Find what columns are updated / changed by user
                List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
                if (listOfUpdates.Count == 0)
                {
                    KryptonMessageBox.Show("Can't find any value that was changed.", "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }
                using (new WaitCursor())
                {
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    foreach (int updatedRecord in listOfUpdates)
                    {
                        if (useAutoCorrect)
                        {
                            Metadata metadataToSave = autoCorrect.RunAlgorithm(metadataListFromDataGridView[updatedRecord],
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
                                //1. Run CompatibilityCheckMetadata, 2. Update DataGridView(s) with fixed metadata, 3. Add to Save queue, 4. Clear dirty flags
                                metadataToSave = AutoCorrect.CompatibilityCheckMetadata(metadataToSave, out bool isUpdated);
                                bool isDirty = isUpdated = metadataToSave != metadataListFromDataGridView[updatedRecord];
                                MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, isUpdated, isDirty);
                                AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, metadataListOriginalExiftool[updatedRecord]);
                            }
                        }
                        else
                        {
                            //Add only metadata to save queue that that has changed by users
                            //1. Run CompatibilityCheckMetadata, 2. Update DataGridView(s) with fixed metadata, 3. Add to Save queue, 4. Clear dirty flags
                            Metadata metadataToSave = AutoCorrect.CompatibilityCheckMetadata(metadataListFromDataGridView[updatedRecord], out bool isUpdated);
                            bool isDirty = isUpdated = metadataToSave!= metadataListFromDataGridView[updatedRecord];
                            string diff = Metadata.GetErrors(metadataToSave, metadataListFromDataGridView[updatedRecord]);
                            MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, isUpdated, isDirty);
                            AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, metadataListOriginalExiftool[updatedRecord]);
                        }
                    }
                }
                ThreadSaveMetadata();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Save - SaveProperties
        private void SaveProperties()
        {
            try
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
                                    "Error message: " + ex.Message + "\r\n" +
                                    "File staus:" + dataGridViewGenericColumn.FileEntryAttribute.FileFullPath + "\r\n" + FileHandler.FileStatusText(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath);

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
                    DataGridView_AfterPopulateSelectedFiles_LazyLoadOtherFileVersions(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                    OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Save - SaveConvertAndMerge
        private void SaveConvertAndMerge()
        {
            try
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
                        if (found) ImageListViewHandler.ImageListViewAddItem(imageListView1, outputFile);
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsFastCopyTextNoOverwrite_Click
        private void TagsAndKeywordsFastCopyTextNoOverwrite_Click()
        {
            try
            {
                DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapFastCopyTextNoOverwrite_Click
        private void MapFastCopyTextNoOverwrite_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridView, DataGridViewHandlerMap.headerMedia, false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsFastCopyTextOverwrite_Click
        private void TagsAndKeywordsFastCopyTextOverwrite_Click()
        {
            try
            {
                DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapFastCopyTextAndOverwrite_Click       
        private void MapFastCopyTextAndOverwrite_Click()
        {
            try
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
                                gridViewGenericRow.RowName.Equals(DataGridViewHandlerMap.tagMediaCoordinates))
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region CellRename
        private void CellRename()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                dataGridView.BeginEdit(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region MediafilesRename
        private void MediafilesRename()
        {
            try
            {
                kryptonWorkspaceCellToolbox.SelectedPage = kryptonPageToolboxRename;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region FolderRename
        private void FolderRename()
        {
            try
            {
                treeViewFolderBrowser1.SelectedNode.BeginEdit();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region FolderMediaViewAsPoster_Click
        private void FolderMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaViewAsPoster_Click(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region MediaFilesMediaViewAsPoster_Click
        private void MediaFilesMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaViewAsPoster_Click(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region DateMediaViewAsPoster_Click
        private void DateMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                GenericMediaViewAsPoster_Click(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolMediaViewAsPoster_Click
        private void ExiftoolMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                GenericMediaViewAsPoster_Click(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningMediaViewAsPoster_Click
        private void ExiftoolWarningMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                GenericMediaViewAsPoster_Click(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapMediaViewAsPoster_Click
        private void MapMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                GenericMediaViewAsPoster_Click(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleMediaViewAsPoster_Click
        private void PeopleMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                GenericMediaViewAsPoster_Click(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsMediaViewAsPoster_Click
        private void TagsAndKeywordsMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                GenericMediaViewAsPoster_Click(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesMediaViewAsPoster_Click
        private void PropertiesMediaViewAsPoster_Click()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                GenericMediaViewAsPoster_Click(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region  GenericMediaPreviewFoldeOrMediaList
        private void GenericMediaPreviewFoldeOrMediaList(string selectedMediaFilePullPath)
        {
            try
            {
                List<string> listOfMediaFiles = new List<string>();
                for (int itemIndex = 0; itemIndex < imageListView1.SelectedItems.Count; itemIndex++) listOfMediaFiles.Add(imageListView1.SelectedItems[itemIndex].FileFullPath);
                MediaPreviewInit(listOfMediaFiles, selectedMediaFilePullPath);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ActionPreviewPreviewClose
        private void ActionPreviewPreviewClose()
        {
            try
            {
                SetPreviewRibbonEnabledStatus(previewStartEnabled: true, enabled: false);
                SetPreviewRibbonPreviewButtonChecked(false);
                timerFindGoogleCast.Stop();
                ActionPreviewStop();
                panelMediaPreview.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PreviewPreviewOpenClose - Click
        private void kryptonRibbonGroupButtonPreviewPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (kryptonRibbonGroupButtonPreviewPreview.Checked) ActionPreviewPreviewOpen(); else ActionPreviewPreviewClose();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void FolderMediaViewAsFull_Click()
        {
            ActionPreviewPreviewOpen();
        }

        private string lastSelectedTab = "";
        private void kryptonRibbonMain_SelectedTabChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaPreviewSelectedInDataGridView(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateMediaPreview_Click
        private void DateMediaPreview_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaPreviewSelectedInDataGridView(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleMediaPreview_Click
        private void PeopleMediaPreview_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaPreviewSelectedInDataGridView(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapMediaPreview_Click
        private void MapMediaPreview_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaPreviewSelectedInDataGridView(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolMediaPreview_Click
        private void ExiftoolMediaPreview_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaPreviewSelectedInDataGridView(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
}
        #endregion

        #region WarningsMediaPreview_Click
        private void WarningsMediaPreview_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaPreviewSelectedInDataGridView(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesMediaPreview_Click
        private void PropertiesMediaPreview_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaPreviewSelectedInDataGridView(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameMediaPreview_Click
        private void RenameMediaPreview_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaPreviewSelectedInDataGridView(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ConvertAndMergeMediaPreview_Click
        private void ConvertAndMergeMediaPreview_Click()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaPreviewSelectedInDataGridView(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion


        #endregion
        
        #region RefreshFolderAndFiles

        #region ActionRefreshFolderAndFiles
        private void ActionRefreshFolderAndFiles()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                TreeViewFolderBrowserHandler.RefreshTreeNode(treeViewFolderBrowser1, selectedNode);
                GlobalData.DoNotRefreshImageListView = false;
                ImageListView_Aggregate_FromFolder(false, true);
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MediaFilesRefresh_Click
        private void MediaFilesRefresh_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                ImageListView_Aggregate_FromFolder(false, true);
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion
        
        #region ReadSubfolders

        #region ActionReadSubfolders
        private void ActionReadSubfolders()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                ImageListView_Aggregate_FromFolder(true, true);
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region OpenExplorerLocation

        #region OpenExplorerLocation
        private void ActionOpenExplorerLocation()
        {
            try
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
                        MediaFilesOpenExplorerLocation_Click(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        MediaFilesOpenExplorerLocation_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        MediaFilesOpenExplorerLocation_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MediaFilesOpenExplorerLocation_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        MediaFilesOpenExplorerLocation_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        MediaFilesOpenExplorerLocation_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        MediaFilesOpenExplorerLocation_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        MediaFilesOpenExplorerLocation_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        MediaFilesOpenExplorerLocation_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        MediaFilesOpenExplorerLocation_Click(DataGridView_GetSelectedFilesFromActive());
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
        private void MediaFilesOpenExplorerLocation_Click(HashSet<FileEntry> files)
        {
            try {
                string errorMessage = "";

                foreach (FileEntry fileEntry in files)
                {
                    try
                    {
                        ApplicationActivation.ShowFileInExplorer(fileEntry.FileFullPath);
                    }
                    catch (Exception ex) { errorMessage += (errorMessage == "" ? "" : "\r\n") + ex.Message; }
                }

                if (errorMessage != "") KryptonMessageBox.Show(errorMessage, "Failed to start application process...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region FolderOpenExplorerLocation_Click
        private void FolderOpenExplorerLocation_Click()
        {
            try
            {
                string folder = GetSelectedNodeFullRealPath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't open folder location. Not a valid folder selected.", "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }
                ApplicationActivation.ShowFolderInEplorer(folder);
            }
            catch (Exception ex) { 
                KryptonMessageBox.Show(ex.Message, "Failed to start application process...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region FileSystemVerbOpen (Files in Folder, ImageListView, Grid)

        #region ActionFileSystemOpen
        private void ActionFileSystemVerbOpen(int? columnIndex, int? rowIndex)
        {
            try
            {
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        MediaFilesVerbOpen_Click(GetFilesInSelectedFolderCached());
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesVerbOpen_Click(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                    case KryptonPages.kryptonPageToolboxPeople:
                    case KryptonPages.kryptonPageToolboxMap:
                    case KryptonPages.kryptonPageToolboxDates:
                    case KryptonPages.kryptonPageToolboxExiftool:
                    case KryptonPages.kryptonPageToolboxWarnings:
                    case KryptonPages.kryptonPageToolboxProperties:
                        if (columnIndex == null || rowIndex == null) MediaFilesVerbOpen_Click(DataGridView_GetSelectedFilesFromActive());
                        else if (columnIndex > -1 && rowIndex <= 0) //Head and first row allowd
                        {
                            HashSet<FileEntry> files = new HashSet<FileEntry>();
                            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(GetActiveTabDataGridView(), (int)columnIndex);
                            if (dataGridViewGenericColumn != null)
                            {
                                files.Add(dataGridViewGenericColumn.FileEntryAttribute);
                                MediaFilesVerbOpen_Click(files);
                            }
                        }
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        if (columnIndex == null || rowIndex == null) MediaFilesVerbOpen_Click(DataGridView_GetSelectedFilesFromActive());
                        else if (rowIndex > 0)
                        {
                            HashSet<FileEntry> files = new HashSet<FileEntry>();
                            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(GetActiveTabDataGridView(), (int)rowIndex);
                            if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                            {
                                files.Add(dataGridViewGenericRow.FileEntryAttribute.FileEntry);
                                MediaFilesVerbOpen_Click(files);
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
                KryptonMessageBox.Show(ex.Message, "Syntax error starting ActionFileSystemVerbOpen...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
        private void MediaFilesVerbOpen_Click(HashSet<FileEntry> files)
        {
            try
            {
                string errorMessage = "";

                foreach (FileEntry fileEntry in files)
                {
                    try
                    {
                        ApplicationActivation.ProcessRunOpenFile(fileEntry.FileFullPath);
                    }
                    catch (Exception ex) { errorMessage += (errorMessage == "" ? "" : "\r\n") + ex.Message; }
                }

                if (errorMessage != "") KryptonMessageBox.Show(errorMessage, "Failed to start application process...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error starting ActionFileSystemVerbOpen...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion


        #endregion

        #region OpenWith - Selected Verb (Files in Folder, ImageListView, Grid)

        #region ActionFileSystemOpenWith
        private void ActionFileSystemOpenWith(ApplicationData applicationData)
        {
            try
            {
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        OpenWithSelectedVerb(applicationData, GetFilesInSelectedFolderCached());
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        OpenWithSelectedVerb(applicationData, ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        OpenWithSelectedVerb(applicationData, DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        OpenWithSelectedVerb(applicationData, DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        OpenWithSelectedVerb(applicationData, DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        OpenWithSelectedVerb(applicationData, DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        OpenWithSelectedVerb(applicationData, DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        OpenWithSelectedVerb(applicationData, DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        OpenWithSelectedVerb(applicationData, DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        OpenWithSelectedVerb(applicationData, DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        OpenWithSelectedVerb(applicationData, DataGridView_GetSelectedFilesFromActive());
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

        #region OpenWithSelectedVerb
        private void OpenWithSelectedVerb(ApplicationData applicationData, HashSet<FileEntry> files)
        {          
            foreach (FileEntry fileEntry in files)
            {
                try
                {
                    if (applicationData.VerbLinks.Count >= 1) ApplicationActivation.ProcessRun(applicationData.VerbLinks[0].Command, applicationData.ApplicationId, fileEntry.FileFullPath, applicationData.VerbLinks[0].Verb, false);
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show("Can execute file. Error: " + ex.Message, "Can execute file", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region OpenAndAssociateWithDialog (Files in Folder, ImageListView, Grid)

        #region ActionOpenAndAssociateWithDialog
        private void ActionOpenAndAssociateWithDialog()
        {
            try
            {
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        MediaFilesOpenAndAssociateWithDialog_Click(GetFilesInSelectedFolderCached());
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesOpenAndAssociateWithDialog_Click(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        MediaFilesOpenAndAssociateWithDialog_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        MediaFilesOpenAndAssociateWithDialog_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MediaFilesOpenAndAssociateWithDialog_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        MediaFilesOpenAndAssociateWithDialog_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        MediaFilesOpenAndAssociateWithDialog_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        MediaFilesOpenAndAssociateWithDialog_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        MediaFilesOpenAndAssociateWithDialog_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        MediaFilesOpenAndAssociateWithDialog_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        MediaFilesOpenAndAssociateWithDialog_Click(DataGridView_GetSelectedFilesFromActive());
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
        private void MediaFilesOpenAndAssociateWithDialog_Click(HashSet<FileEntry> files)
        {
            try
            {
                foreach (FileEntry fileEntry in files)
                {
                    ApplicationActivation.ShowOpenWithDialog(fileEntry.FileFullPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }

        }
        #endregion

        #endregion

        #region FileSystemVerbEdit

        #region ActionFileSystemVerbEdit (Files in Folder, ImageListView, Grid)
        private void ActionFileSystemVerbEdit()
        {
            try
            {
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        MediaFilesVerbEdit_Click(GetFilesInSelectedFolderCached());
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesVerbEdit_Click(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        MediaFilesVerbEdit_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        MediaFilesVerbEdit_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MediaFilesVerbEdit_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        MediaFilesVerbEdit_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        MediaFilesVerbEdit_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        MediaFilesVerbEdit_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        MediaFilesVerbEdit_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        MediaFilesVerbEdit_Click(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        MediaFilesVerbEdit_Click(DataGridView_GetSelectedFilesFromActive());
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
        private void MediaFilesVerbEdit_Click(HashSet<FileEntry> files)
        {
            try {
                string errorMessage = "";

                foreach (FileEntry fileEntry in files)
                {
                    try
                    {
                        ApplicationActivation.ProcessRunEditFile(fileEntry.FileFullPath);
                    }
                    catch (Exception ex) { errorMessage += (errorMessage == "" ? "" : "\r\n") + ex.Message; }
                }

                if (errorMessage != "") KryptonMessageBox.Show(errorMessage, "Failed to start application process...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region FileSystemRunCommand (Files in Folder, ImageListView, Grid)

        #region ActionFileSystemRunCommand
        private void ActionFileSystemRunCommand()
        {
            try
            {
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        MediaFilesRunCommand(GetFilesInSelectedFolderCached());
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesRunCommand(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        MediaFilesRunCommand(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        MediaFilesRunCommand(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MediaFilesRunCommand(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        MediaFilesRunCommand(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        MediaFilesRunCommand(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        MediaFilesRunCommand(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        MediaFilesRunCommand(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        MediaFilesRunCommand(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        MediaFilesRunCommand(DataGridView_GetSelectedFilesFromActive());
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
        private void MediaFilesRunCommand(HashSet<FileEntry> files)
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
                    CollectMetadataFromAllDataGridViewData(out List <Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView, false);

                    ExiftoolWriter.CreateExiftoolArguFileText(
                        metadataListFromDataGridView, metadataListOriginalExiftool, allowedFileNameDateTimeFormats, writeMetadataTagsVariable, writeMetadataKeywordAddVariable,
                        true, out string exiftoolAgruFileText);
                    #endregion

                    FormSplash.UpdateStatus("Create AutoCorrect file...");
                    #region AutoCorrect
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    if (autoCorrect == null) KryptonMessageBox.Show("AutoCorrect: " + Properties.Settings.Default.AutoCorrect, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    List<Metadata> metadataListEmpty = new List<Metadata>();
                    List<Metadata> metadataListFromDataGridViewAutoCorrect = new List<Metadata>();

                    foreach (FileEntry fileEntry in files)  
                    {
                        FormSplash.UpdateStatus("Create AutoCorrect file..." + fileEntry.FileName);

                        FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool);
                        Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
                        if (metadataInCache != null)
                        {
                            Metadata metadata = new Metadata(metadataInCache);

                            Metadata metadataToSave = autoCorrect.RunAlgorithm(metadata,
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
                                Logger.Warn("Metadata was not loaded for file, check if file is only in cloud:" + fileEntry.FileFullPath);
                            }
                            metadataListEmpty.Add(new Metadata(MetadataBrokerType.Empty));
                        }
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region AutoCorrectRun (Files in Folder, ImageListView, NOT: Grid)

        #region ActionAutoCorrectRun 
        private void ActionAutoCorrectRun()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                using (new WaitCursor())
                {
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    foreach (ImageListViewItem item in imageListView1.SelectedItems)
                    {
                        FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(item.FileFullPath, item.DateModified, MetadataBrokerType.ExifTool);
                        Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
                        if (metadataInCache != null)
                        {
                            Metadata metadata = new Metadata(metadataInCache);

                            Metadata metadataToSave = autoCorrect.RunAlgorithm(metadata,
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
                                //1. Run CompatibilityCheckMetadata, 2. Update DataGridView(s) with fixed metadata, 3. Add to Save queue, 4. Clear dirty flags
                                metadataToSave = AutoCorrect.CompatibilityCheckMetadata(metadataToSave, out bool isUpdated);
                                bool isDirty = isUpdated = metadata != metadataToSave;
                                MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, true, isDirty);
                                AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                                AddQueueRenameLock(item.FileFullPath, autoCorrect.RenameVariable); //Properties.Settings.Default.AutoCorrect.)
                            }
                        }
                    }
                }
                StartThreads();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region FolderAutoCorrectRun_Click
        private void AutoCorrectRunFolder_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                using (new WaitCursor())
                {
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    string selectedFolder = GetSelectedNodeFullRealPath();
                    if (selectedFolder == null || !Directory.Exists(selectedFolder))
                    {
                        KryptonMessageBox.Show("Can't run AutoCorrect. Not a valid folder selected.", "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                        return;
                    }
                    string[] files = Directory.GetFiles(selectedFolder, "*.*");
                    foreach (string file in files)
                    {
                        FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(file, File.GetLastWriteTime(file), MetadataBrokerType.ExifTool);
                        Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
                        if (metadataInCache != null)
                        {
                            Metadata metadata = new Metadata(metadataInCache);

                            Metadata metadataToSave = autoCorrect.RunAlgorithm(metadata,
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
                                //1. Run CompatibilityCheckMetadata, 2. Update DataGridView(s) with fixed metadata, 3. Add to Save queue, 4. Clear dirty flags
                                metadataToSave = AutoCorrect.CompatibilityCheckMetadata(metadataToSave, out bool isUpdated);
                                bool isDirty = isUpdated = metadataToSave != metadata;
                                MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, true, isDirty);
                                AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                                AddQueueRenameLock(file, autoCorrect.RenameVariable); //Properties.Settings.Default.AutoCorrect.)
                            }
                        }
                    }
                }
                StartThreads();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region AutoCorrectRunDataGridView_Click
        private void AutoCorrectRunDataGridView_Click()
        {
            if (GlobalData.IsPopulatingAnything())
            {
                KryptonMessageBox.Show("Data is populating, please try a bit later.", "Warning...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                return;
            }
            if (!GlobalData.IsAgredagedGridViewAny())
            {
                KryptonMessageBox.Show("No metadata are updated.", "Warning...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                return;
            }
            DataGridView dataGridView = GetActiveTabDataGridView();

            try
            {
                using (new WaitCursor())
                {
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    List<FileEntryAttribute> fileEntryAttributes = new List<FileEntryAttribute>();

                    foreach (int columIndex in DataGridViewHandler.GetColumnSelected(dataGridView))
                    {
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columIndex);
                        
                        if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.Metadata != null)
                        {
                            FileEntryAttribute fileEntryAttribute = dataGridViewGenericColumn.FileEntryAttribute;
                            Metadata metadataFromDataGridView = new Metadata(dataGridViewGenericColumn.Metadata);
                            CollectedMetadataFromAllDataGridView(fileEntryAttribute, ref metadataFromDataGridView);

                            Metadata metadataToSave = autoCorrect.RunAlgorithm(metadataFromDataGridView,
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
                                //1. Run CompatibilityCheckMetadata, 2. Update DataGridView(s) with fixed metadata,  4. Clear dirty flags
                                metadataToSave = AutoCorrect.CompatibilityCheckMetadata(metadataToSave, out bool isUpdated);
                                bool isDirty = isUpdated = metadataToSave != metadataFromDataGridView;
                                MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, isUpdated, isDirty);
                            }
                        }
                    }
                    AddQueueLazyLoadningDataGridViewMetadataLock(fileEntryAttributes);
                }
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                    using (new WaitCursor())
                    {
                        AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                        float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                        float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                        int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                        AutoCorrectFormVaraibles autoCorrectFormVaraibles = formAutoCorrect.AutoCorrectFormVaraibles;
                        autoCorrectFormVaraibles.WriteAlbumOnDescription = autoCorrect.UpdateDescription;

                        foreach (ImageListViewItem item in imageListView1.SelectedItems)
                        {
                            FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(item.FileFullPath, item.DateModified, MetadataBrokerType.ExifTool);
                            Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
                            if (metadataInCache != null)
                            {
                                Metadata metadata = new Metadata(metadataInCache);
                                
                                AutoCorrectFormVaraibles.UseAutoCorrectFormData(ref metadata, autoCorrectFormVaraibles);

                                Metadata metadataToSave = autoCorrect.RunAlgorithm(metadata,
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
                                    //1. Run CompatibilityCheckMetadata, 2. Update DataGridView(s) with fixed metadata, 3. Add to Save queue, 4. Clear dirty flags
                                    metadataToSave = AutoCorrect.CompatibilityCheckMetadata(metadataToSave, out bool isUpdated);
                                    bool isDirty = isUpdated = metadataToSave != metadata;
                                    MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, isUpdated, isDirty);
                                    AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                                    AddQueueRenameLock(item.FileFullPath, autoCorrect.RenameVariable);
                                }
                            }
                        }
                    }
                    StartThreads();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                    using (new WaitCursor())
                    {
                        AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                        float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                        float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                        int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                        AutoCorrectFormVaraibles autoCorrectFormVaraibles = formAutoCorrect.AutoCorrectFormVaraibles;
                        autoCorrectFormVaraibles.WriteAlbumOnDescription = autoCorrect.UpdateDescription;

                        string selectedFolder = GetSelectedNodeFullRealPath();
                        if (selectedFolder == null || !Directory.Exists(selectedFolder))
                        {
                            KryptonMessageBox.Show("Can't run AutoCorrect. Not a valid folder selected.", "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                            return;
                        }
                        string[] files = Directory.GetFiles(selectedFolder, "*.*");
                        foreach (string file in files)
                        {
                            FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(file, File.GetLastWriteTime(file), MetadataBrokerType.ExifTool);
                            Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
                            if (metadataInCache != null)
                            {
                                Metadata metadata = new Metadata(metadataInCache);

                                Metadata metadataToSave = autoCorrect.RunAlgorithm(metadata,
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
                                    AutoCorrectFormVaraibles.UseAutoCorrectFormData(ref metadataToSave, autoCorrectFormVaraibles);

                                    //1. Run CompatibilityCheckMetadata, 2. Update DataGridView(s) with fixed metadata, 3. Add to Save queue, 4. Clear dirty flags
                                    metadataToSave = AutoCorrect.CompatibilityCheckMetadata(metadataToSave, out bool isUpdated);
                                    bool isDirty = isUpdated = metadataToSave != metadata;
                                    MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, isUpdated, isDirty);
                                    AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                                    AddQueueRenameLock(file, autoCorrect.RenameVariable);
                                }
                            }
                        }
                    }
                    StartThreads();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region AutoCorrectFormDataGridView_Click
        private void AutoCorrectFormDataGridView_Click()
        {

            if (GlobalData.IsPopulatingAnything())
            {
                KryptonMessageBox.Show("Data is populating, please try a bit later.", "Warning...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                return;
            }
            if (!GlobalData.IsAgredagedGridViewAny())
            {
                KryptonMessageBox.Show("No metadata are updated.", "Warning...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                return;
            }
            DataGridView dataGridView = GetActiveTabDataGridView();

            try
            {
                FormAutoCorrect formAutoCorrect = new FormAutoCorrect();
                if (formAutoCorrect.ShowDialog() == DialogResult.OK)
                {
                    using (new WaitCursor())
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
                                FileEntryAttribute fileEntryAttribute = dataGridViewGenericColumn.FileEntryAttribute;
                                Metadata metadataFromDataGridView = new Metadata(dataGridViewGenericColumn.Metadata);
                                
                                CollectedMetadataFromAllDataGridView(fileEntryAttribute, ref metadataFromDataGridView);
                                AutoCorrectFormVaraibles.UseAutoCorrectFormData(ref metadataFromDataGridView, autoCorrectFormVaraibles);
                                
                                Metadata metadataToSave = autoCorrect.RunAlgorithm(metadataFromDataGridView,
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
                                    //1. Run CompatibilityCheckMetadata, 2. Update DataGridView(s) with fixed metadata,  4. Clear dirty flags
                                    metadataToSave = AutoCorrect.CompatibilityCheckMetadata(metadataToSave, out bool isUpdated);
                                    bool isDirty = isUpdated = metadataToSave != metadataFromDataGridView;
                                    MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, isUpdated, isDirty);
                                }
                            }
                        }
                        AddQueueLazyLoadningDataGridViewMetadataLock(fileEntryAttributes);
                    }
                    StartThreads();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region MetadataRefreshLast (Files in Folder, ImageListView, NOT Grid)

        #region ActionMetadataRefreshLast
        private void ActionMetadataRefreshLast()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                    TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, false);

                    ClearAllQueues();

                    UpdateStatusAction("Delete all data and files...");
                    lock (GlobalData.ReloadAllowedFromCloudLock)
                    {
                        GlobalData.ReloadAllowedFromCloud = filesCutCopyPasteDrag.DeleteFileEntriesBeforeReload(imageListView.Items, updatedOnlySelected);
                    }
                    filesCutCopyPasteDrag.ImageListViewReload(imageListView.Items, updatedOnlySelected);

                    TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, true);
                    //ImageListViewResumeLayoutInvoke(imageListView);
                    GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                    GlobalData.IsPopulatingButtonAction = false;
                    GlobalData.IsPopulatingImageListView = false;

                    OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region MetadataReloadDeleteHistory (Files in Folder, ImageListView, NOT Grid)

        #region ActionMetadataReloadDeleteHistory
        private void ActionMetadataReloadDeleteHistory()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                    OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
                    DisplayAllQueueStatus();
                }
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region FolderMetadataReloadDeleteHistory_Click
        private void FolderMetadataReloadDeleteHistory_Click()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                string folder = GetSelectedNodeFullRealPath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't reload folder. Not a valid folder selected.", "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }

                string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly).Take(51).ToArray();
                if (KryptonMessageBox.Show("Are you sure you will delete **ALL** metadata history in database store for " +
                    (fileAndFolderEntriesCount.Length == 51 ? " over 50 + " : fileAndFolderEntriesCount.Length.ToString()) +
                    "  number of files.\r\n\r\n" +
                    "In the folder: " + folder,
                    "You are going to delete all metadata in folder",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, showCtrlCopy: true) == DialogResult.Yes)
                {
                    using (new WaitCursor())
                    {
                        //Clean up ImageListView and other queues
                        ImageListViewHandler.ClearThumbnailCache(imageListView1);
                        imageListView1.Refresh();
                        ClearAllQueues();

                        UpdateStatusAction("Delete all record about files in database....");
                        GlobalData.ProcessCounterDelete = FilesCutCopyPasteDrag.DeleteDirectoryAndHistorySize;
                        int recordAffected = filesCutCopyPasteDrag.DeleteDirectoryAndHistory(ref FilesCutCopyPasteDrag.DeleteDirectoryAndHistorySize, folder);
                        GlobalData.ProcessCounterDelete = 0;
                        UpdateStatusAction(recordAffected + " records was delete from database....");

                        //string selectedFolder = GetSelectedNodePath();
                        //IEnumerable<FileData> fileDatas = GetFilesInSelectedFolder(selectedFolder, false);
                        //ImageListView_Aggregate_FromReadFolderOrFilterOrDatabase(fileDatas, null, selectedFolder, false);
                        ImageListView_Aggregate_FromFolder(false, true);
                    }
                }
                DisplayAllQueueStatus();
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                    OnImageListViewSelect_FilesSelectedOrNoneSelected(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                        TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, false);
                        ImageListViewHandler.Enable(imageListView1, false);
                        
                        OnImageListViewSelect_FilesSelectedOrNoneSelected(true);

                        ImageListViewHandler.Enable(imageListView1, true);
                        TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ActionAssignCompositeTag
        private void ActionAssignCompositeTag(DataGridView dataGridView, MetadataPriorityValues metadataPriorityValues)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region KryptonContextMenuItemAssignCompositeTag_Click
        private void KryptonContextMenuItemAssignCompositeTag_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion 

        //----
        #region DataGridView - SetGridViewSize Small / Medium / Big - Click

        #region SetRibbonDataGridViewSizeBottons
        private void SetRibbonDataGridViewSizeBottons(DataGridViewSize size, bool enabled)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region SetGridViewSize
        private void SetGridViewSize(DataGridViewSize size)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region ColumnWidthChanged

        #region ColumnWidthChanged - imageListView1_ColumnWidthChanged
        private void imageListView1_ColumnWidthChanged(object sender, ColumnEventArgs e)
        {
            try
            {
                Properties.Settings.Default.ColumnNameAndWithsImageListView = ColumnNamesAndWidthHandler.ConvertColumnNameAndWidthsToConfigString(ColumnNamesAndWidthHandler.GetColumnNameAndWidths(imageListView1));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ColumnWidthChanged - dataGridViewRename_ColumnWidthChanged
        private void dataGridViewRename_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ColumnWidthChanged - dataGridViewConvertAndMerge_ColumnWidthChanged
        private void dataGridViewConvertAndMerge_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region DataGridView - Show/Hide Historiy / Error Columns - Click

        #region SetRibbonDataGridViewShowWhatColumns
        private void SetRibbonDataGridViewShowWhatColumns(ShowWhatColumns showWhatColumns, bool enabled = true)
        {
            try
            {
                SetRibbonGridViewColumnsButtonsHistoricalAndError(ShowWhatColumnHandler.ShowHirstoryColumns(showWhatColumns), ShowWhatColumnHandler.ShowErrorColumns(showWhatColumns));
                kryptonRibbonGroupButtonDataGridViewColumnsHistory.Enabled = enabled;
                kryptonRibbonGroupButtonDataGridViewColumnsErrors.Enabled = enabled;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region SetRibbonGridViewColumnsButtonsHistoricalAndError
        private void SetRibbonGridViewColumnsButtonsHistoricalAndError(bool showHistorical, bool showErrors)
        {
            try
            {
                kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked = showHistorical;
                kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked = showErrors;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewColumnsXyz_Click
        private void kryptonRibbonGroupButtonDataGridViewColumnsHistory_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.ShowHistortyColumns = kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked;
                showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked, kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked);
                DataGridView_AfterPopulateSelectedFiles_LazyLoadOtherFileVersions(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void kryptonRibbonGroupButtonDataGridViewColumnsErrors_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.ShowErrorColumns = kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked;
                showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked, kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked);
                DataGridView_AfterPopulateSelectedFiles_LazyLoadOtherFileVersions(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);

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
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ImageListView - Change Thumbnail Size 

        #region SetThumbnailSize
        private void SetThumbnailSize(int size)
        {
            try
            {
                imageListView1.ThumbnailSize = thumbnailSizes[size];
                Properties.Settings.Default.ThumbmailViewSizeIndex = size;
                kryptonRibbonGroupButtonThumbnailSizeXLarge.Checked = (size == 4);
                kryptonRibbonGroupButtonThumbnailSizeLarge.Checked = (size == 3);
                kryptonRibbonGroupButtonThumbnailSizeMedium.Checked = (size == 2);
                kryptonRibbonGroupButtonThumbnailSizeSmall.Checked = (size == 1);
                kryptonRibbonGroupButtonThumbnailSizeXSmall.Checked = (size == 0);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
        private void ImageListViewSortColumn(ImageListView imageListView, KryptonContextMenuRadioButton kryptonContextMenuRadioButton, ColumnType columnToSort, SortOrder sortOrder)
        {
            try
            {
                if (imageListView.SortColumn != columnToSort) imageListView.SortColumn = columnToSort;
                if (imageListView.SortOrder != sortOrder) imageListView.SortOrder = sortOrder;

                if (sortOrder != SortOrder.None) //If has sorting, force read all fields before sorting. To avoid colltection get updaed and sort will give error
                foreach (Manina.Windows.Forms.ImageListViewItem imageListViewItem in imageListView.Items)
                {
                    object _;

                    switch (columnToSort)
                    {
                        case ColumnType.FileName:
                            //var _ = imageListViewItem.Text;
                            break;
                        case ColumnType.FileDate:
                            _ = imageListViewItem.Date;
                            break;
                        case ColumnType.FileSmartDate:
                            _ = imageListViewItem.SmartDate;
                            break;
                        case ColumnType.FileDateCreated:
                            _ = imageListViewItem.DateCreated;
                            break;
                        case ColumnType.FileDateModified:
                            _ = imageListViewItem.DateModified;
                            break;
                        case ColumnType.FileType:
                            _ = imageListViewItem.FileType;
                            break;
                        case ColumnType.FileFullPath:
                            _ = imageListViewItem.FileFullPath;
                            break;
                        case ColumnType.FileDirectory:
                            _ = imageListViewItem.FileDirectory;
                            break;
                        case ColumnType.FileSize:
                            _ = imageListViewItem.FileSize;
                            break;
                        case ColumnType.MediaDimensions:
                            _ = imageListViewItem.Dimensions;
                            break;
                        case ColumnType.CameraMake:
                            _ = imageListViewItem.CameraMake;
                            break;
                        case ColumnType.CameraModel:
                            _ = imageListViewItem.CameraModel;
                            break;
                        case ColumnType.MediaDateTaken:
                            _ = imageListViewItem.DateTaken;
                            break;
                        case ColumnType.MediaAlbum:
                            _ = imageListViewItem.MediaAlbum;
                            break;
                        case ColumnType.MediaTitle:
                            _ = imageListViewItem.MediaTitle;
                            break;
                        case ColumnType.MediaDescription:
                            _ = imageListViewItem.MediaDescription;
                            break;
                        case ColumnType.MediaComment:
                            _ = imageListViewItem.MediaComment;
                            break;
                        case ColumnType.MediaAuthor:
                            _ = imageListViewItem.MediaAuthor;
                            break;
                        case ColumnType.MediaRating:
                            _ = imageListViewItem.MediaRating;
                            break;
                        case ColumnType.LocationDateTime:
                            _ = imageListViewItem.LocationDateTime;
                            break;
                        case ColumnType.LocationTimeZone:
                            _ = imageListViewItem.LocationTimeZone;
                            break;
                        case ColumnType.LocationName:
                            _ = imageListViewItem.LocationName;
                            break;
                        case ColumnType.LocationRegionState:
                            _ = imageListViewItem.LocationRegionState;
                            break;
                        case ColumnType.LocationCity:
                            _ = imageListViewItem.LocationCity;
                            break;
                        case ColumnType.LocationCountry:
                            _ = imageListViewItem.LocationCountry;
                            break;
                        default:
                            throw new NotImplementedException();

                    }
                }
                imageListView.Sort();

                if (kryptonContextMenuRadioButton != null) kryptonContextMenuRadioButton.ExtraText = imageListView.SortOrder.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region SetImageListViewSortByRadioButton
        private void SetImageListViewSortByRadioButton(ImageListView imageListView, ColumnType columnType, SortOrder sortOrder)
        {
            try
            {
                if (columnType != imageListView1.SortColumn) imageListView1.SortColumn = columnType;
                if (sortOrder != imageListView1.SortOrder) imageListView1.SortOrder = sortOrder;

                #region Clear RadioButton Checked = false
                this.kryptonContextMenuRadioButtonFileSystemColumnSortFilename.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortFileDate.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationName.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity.Checked = false;
                this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry.Checked = false;
                #endregion

                #region Set Correct RadioButton to Checked 
                if (sortOrder != SortOrder.None)
                {
                    switch (columnType)
                    {
                        case ColumnType.FileName:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortFilename.Checked = true;
                            break;
                        case ColumnType.FileSmartDate:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate.Checked = true;
                            break;
                        case ColumnType.FileDate:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileDate.Checked = true;
                            break;
                        case ColumnType.FileDateCreated:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate.Checked = true;
                            break;
                        case ColumnType.FileDateModified:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate.Checked = true;
                            break;
                        case ColumnType.MediaDateTaken:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken.Checked = true;
                            break;
                        case ColumnType.MediaAlbum:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum.Checked = true;
                            break;
                        case ColumnType.MediaTitle:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle.Checked = true;
                            break;
                        case ColumnType.MediaDescription:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription.Checked = true;
                            break;
                        case ColumnType.MediaComment:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments.Checked = true;
                            break;
                        case ColumnType.MediaAuthor:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor.Checked = true;
                            break;
                        case ColumnType.MediaRating:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating.Checked = true;
                            break;
                        case ColumnType.LocationName:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationName.Checked = true;
                            break;
                        case ColumnType.LocationRegionState:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState.Checked = true;
                            break;
                        case ColumnType.LocationCity:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity.Checked = true;
                            break;
                        case ColumnType.LocationCountry:
                            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry.Checked = true;
                            break;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ImageListViewSortByCheckedRadioButton
        private SortOrder lastUsedSortOrder = SortOrder.Ascending;
        private void ImageListViewSortByCheckedRadioButton(bool toogle)
        {
            try
            {
                #region Clear all ExtraText
                this.kryptonContextMenuRadioButtonFileSystemColumnSortFilename.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortFileDate.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationName.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity.ExtraText = "";
                this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry.ExtraText = "";
                #endregion

                #region Find correct KryptonContextMenuRadioButton and ColumnType
                ColumnType columnType = ColumnType.FileSmartDate;
                KryptonContextMenuRadioButton kryptonContextMenuRadioButton = null;
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortFilename.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortFilename;
                    columnType = ColumnType.FileName;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate;
                    columnType = ColumnType.FileSmartDate;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortFileDate.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortFileDate;
                    columnType = ColumnType.FileDate;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate;
                    columnType = ColumnType.FileDateCreated;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate;
                    columnType = ColumnType.FileDateModified;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken;
                    columnType = ColumnType.MediaDateTaken;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum;
                    columnType = ColumnType.MediaAlbum;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle;
                    columnType = ColumnType.MediaTitle;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription;
                    columnType = ColumnType.MediaDescription;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments;
                    columnType = ColumnType.MediaComment;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor;
                    columnType = ColumnType.MediaAuthor;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating;
                    columnType = ColumnType.MediaRating;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationName.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortLocationName;
                    columnType = ColumnType.LocationName;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState;
                    columnType = ColumnType.LocationRegionState;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity;
                    columnType = ColumnType.LocationCity;
                }
                if (this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry.Checked)
                {
                    kryptonContextMenuRadioButton = kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry;
                    columnType = ColumnType.LocationCountry;
                }
                #endregion

                SortOrder sortOrder = imageListView1.SortOrder;
                if (toogle)
                {
                    if (imageListView1.SortColumn == columnType || sortOrder == SortOrder.None) //Only change Ascending <-> Descending when select same type again
                    {
                        if (sortOrder == SortOrder.Ascending) sortOrder = SortOrder.Descending;
                        else if (sortOrder == SortOrder.Descending) sortOrder = SortOrder.Ascending;
                        else sortOrder = lastUsedSortOrder;
                    }
                }
                Properties.Settings.Default.ImageListViewSortingColumn = (int)columnType;
                Properties.Settings.Default.ImageListViewSortingOrder = (int)sortOrder;
                if (sortOrder != SortOrder.None) lastUsedSortOrder = sortOrder;

                ImageListViewSortColumn(imageListView1, kryptonContextMenuRadioButton, columnType, sortOrder);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region KryptonContextMenuRadioButtonFileSystemColumnSortXYZ_Click
        private void KryptonContextMenuItemFileSystemColumnSortClear_Click(object sender, EventArgs e)
        {
            SetImageListViewSortByRadioButton(imageListView1, imageListView1.SortColumn, SortOrder.None);
            ImageListViewSortByCheckedRadioButton(false);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFilename_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFileDate_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaComments_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaRating_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationName_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationCity_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry_Click(object sender, EventArgs e)
        {
            ImageListViewSortByCheckedRadioButton(true);
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
                OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                    ImageListViewSuspendLayoutInvoke(imageListView1);
                    foreach (ImageListViewItem imageListViewItem in imageListView1.Items) imageListViewItem.Selected = !imageListViewItem.Selected;
                    ImageListViewResumeLayoutInvoke(imageListView1);
                }
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            try
            {
                progressBarSaveConvert.Minimum = minimum;
                progressBarSaveConvert.Maximum = maximum;
                progressBarSaveConvert.Value = value;
                ProgressbarSaveProgress(enabled);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ProgressbarSaveProgress(bool visible)
        private void ProgressbarSaveProgress(bool visible)
        {
            try
            {
                kryptonRibbonGroupTripleProgressStatusSave.Visible = visible;
                kryptonRibbonGroupCustomControlToolsProgressSave.Visible = visible;
                kryptonRibbonGroupLabelToolsProgressSave.Visible = visible;
                kryptonRibbonGroupLabelToolsProgressSaveText.Visible = visible;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

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


        //----
        

        #region DirtyFlag - DataGridView - CellLeave - UpdatedDirtyFlag
        private void dataGridViewTagsAndKeywords_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewConvertAndMerge_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewProperties_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewDate_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewMap_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewPeople_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }
        #endregion

        #region Cell BeginEdit

        #region Cell BeginEdit - Date
        private void dataGridViewDate_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell BeginEdit - Exiftool
        private void dataGridViewExifTool_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell BeginEdit - ExiftoolWarning
        private void dataGridViewExifToolWarning_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell BeginEdit - Map
        private void dataGridViewMap_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell BeginEdit - People
        private void dataGridViewPeople_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell BeginEdit - Properties
        private void dataGridViewProperties_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell BeginEdit - Rename
        private void dataGridViewRename_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        #endregion

        #region Cell BeginEdit - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region Cell Painting

        #region Cell Painting - Convert and Merge
        private void dataGridViewConvertAndMerge_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region Cell Painting - Date
        private void dataGridViewDate_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - Exiftool
        private void dataGridViewExifTool_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - ExiftoolWarning
        private void dataGridViewExifToolWarning_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - Map
        private void dataGridViewMap_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - People
        private void dataGridViewPeople_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - Properties
        private void dataGridViewProperties_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - Rename
        private void dataGridViewRename_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
                //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
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
