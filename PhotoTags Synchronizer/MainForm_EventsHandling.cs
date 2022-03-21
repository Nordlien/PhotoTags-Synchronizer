using ApplicationAssociations;
using ColumnNamesAndWidth;
using DataGridViewGeneric;
using Exiftool;
using FileDateTime;
using FileHandeling;
using ImageAndMovieFileExtentions;
using Krypton.Toolkit;
using Manina.Windows.Forms;
using MetadataLibrary;
using MetadataPriorityLibrary;
using Raccoom.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Thumbnails;

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
        #region Select All 

        #region SelectAll - Click Events Sources
        private void kryptonRibbonQATButtonSelectAll_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select all")) return;
            if (IsPopulatingAnything("Select all")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try { 
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectAll();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonSelectAll_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select all")) return;
            if (IsPopulatingAnything("Select all")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectAll();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ActionSelectAll
        private void ActionSelectAll()
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
                        ImageListViewSelectAll();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        if (controlPasteWithFocusTag is KryptonComboBox) ComboBoxSelectAll();
                        if (controlPasteWithFocusTag is KryptonDataGridView) DataGridViewSelectAll(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        DataGridViewSelectAll(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        DataGridViewSelectAll(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DataGridViewSelectAll(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        DataGridViewSelectAll(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        DataGridViewSelectAll(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        DataGridViewSelectAll(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        DataGridViewSelectAll(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        DataGridViewSelectAll(GetActiveTabDataGridView());
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

        #region ImageListViewSelectAll
        private void ImageListViewSelectAll()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
                ImageListViewHandler.SuspendLayout(imageListView1);

                using (new WaitCursor())
                {
                    imageListView1.SelectAll();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                ImageListViewHandler.ResumeLayout(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
        }
        #endregion

        #region ComboBoxSelectAll
        private void ComboBoxSelectAll()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                if (controlPasteWithFocusTag is KryptonComboBox)
                {
                    ((KryptonComboBox)controlPasteWithFocusTag).SelectAll();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewSelectAll
        private void DataGridViewSelectAll(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.ColumnCount == 0 || dataGridView.RowCount == 0) return;
            dataGridView.SelectAll();
        }
        #endregion

        #endregion

        #region Select None

        #region SelectNone - Click Events Sources
        private void kryptonRibbonQATButtonSelectNone_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select none")) return;
            if (IsPopulatingAnything("Select none")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectNone();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void kryptonRibbonGroupButtonSelectNone_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select none")) return;
            if (IsPopulatingAnything("Select none")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectNone();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ActionSelectNone
        private void ActionSelectNone()
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
                        ImageListViewSelectNone();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:                        
                        if (controlPasteWithFocusTag is KryptonComboBox) ComboBoxSelectNone();
                        if (controlPasteWithFocusTag is KryptonDataGridView) DataGridViewSelectNone(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        DataGridViewSelectNone(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        DataGridViewSelectNone(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DataGridViewSelectNone(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        DataGridViewSelectNone(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        DataGridViewSelectNone(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        DataGridViewSelectNone(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        DataGridViewSelectNone(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        DataGridViewSelectNone(GetActiveTabDataGridView());
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

        #region ImageListViewSelectNone
        private void ImageListViewSelectNone()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
                ImageListViewHandler.SuspendLayout(imageListView1);

                using (new WaitCursor())
                {
                    imageListView1.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                ImageListViewHandler.ResumeLayout(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
        }
        #endregion

        #region ComboBoxSelectNone
        private void ComboBoxSelectNone()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                if (controlPasteWithFocusTag is KryptonComboBox)
                {
                    ((KryptonComboBox)controlPasteWithFocusTag).SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewSelectNone
        private void DataGridViewSelectNone(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.ColumnCount == 0 || dataGridView.RowCount == 0) return;
            dataGridView.ClearSelection();
        }
        #endregion

        #endregion

        #region Select Toggle

        #region Select Toggle - Click Events Sources
        private void kryptonRibbonGroupButtonSelectToggle_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select toggle")) return;
            if (IsPopulatingAnything("Select toggle")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectToggle();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonQATButtonSelectToggle_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select toggle")) return;
            if (IsPopulatingAnything("Select toggle")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectToggle();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ActionSelectToggle
        private void ActionSelectToggle()
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
                        ImageListViewSelectToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:                        
                        if (controlPasteWithFocusTag is KryptonComboBox) ComboBoxSelectToggle();
                        if (controlPasteWithFocusTag is KryptonDataGridView) DataGridViewSelectToggle(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        DataGridViewSelectToggle(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        DataGridViewSelectToggle(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DataGridViewSelectToggle(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        DataGridViewSelectToggle(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        DataGridViewSelectToggle(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        DataGridViewSelectToggle(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        DataGridViewSelectToggle(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        DataGridViewSelectToggle(GetActiveTabDataGridView());
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

        #region ImageListViewSelectToggle
        private void ImageListViewSelectToggle()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
                ImageListViewHandler.SuspendLayout(imageListView1);

                using (new WaitCursor())
                {
                    ImageListViewSuspendLayoutInvoke(imageListView1);
                    foreach (ImageListViewItem imageListViewItem in imageListView1.Items) imageListViewItem.Selected = !imageListViewItem.Selected;
                    ImageListViewResumeLayoutInvoke(imageListView1);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                ImageListViewHandler.ResumeLayout(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
        }
        #endregion

        #region ComboBoxSelectToggle
        private void ComboBoxSelectToggle()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                if (controlPasteWithFocusTag is KryptonComboBox)
                {
                    KryptonComboBox kryptonComboBox = (KryptonComboBox)controlPasteWithFocusTag;                    
                    if (kryptonComboBox.SelectionLength == 0)
                    {
                        //|---- -> #####
                        kryptonComboBox.SelectAll();
                    }
                    else if (kryptonComboBox.SelectionStart == 0 && kryptonComboBox.SelectionLength > 0)
                    {
                        //##--- -> --###
                        int selectionStart = kryptonComboBox.SelectionLength;
                        int selectionLength = kryptonComboBox.Text.Length - selectionStart;
                        kryptonComboBox.Select(selectionStart, selectionLength);
                    }
                    else if (kryptonComboBox.SelectionStart > 0 && kryptonComboBox.SelectionLength > 0)
                    {
                        //--##- -> ##---
                        //--### -> ##---
                        int selectionStart = 0;
                        int selectionLength = kryptonComboBox.SelectionStart;
                        kryptonComboBox.Select(selectionStart, selectionLength);
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

        #region DataGridViewSelectNone
        private void DataGridViewSelectToggle(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.ColumnCount == 0 || dataGridView.RowCount == 0) return;
            foreach (DataGridViewRow dataGridViewRow in dataGridView.Rows)
            {
                foreach (DataGridViewCell dataGridViewCell in dataGridViewRow.Cells) dataGridViewCell.Selected = !dataGridViewCell.Selected;
            }
        }
        #endregion

        #endregion

        #region Select Next

        #region Select Next - Click Events Sources
        private void kryptonRibbonGroupButtonSelectForwards_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select Forwards")) return;
            if (IsPopulatingAnything("Select Forwards")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectNext();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonQATButtonSelectNext_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select Next")) return;
            if (IsPopulatingAnything("Select Next")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectNext();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ActionSelectNext
        private void ActionSelectNext()
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
                        ImageListViewSelectNext();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        if (controlPasteWithFocusTag is KryptonComboBox) ComboBoxSelectNext();
                        if (controlPasteWithFocusTag is KryptonDataGridView) DataGridViewSelectNextColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        DataGridViewSelectNextColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        DataGridViewSelectNextColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DataGridViewSelectNextColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        DataGridViewSelectNextColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        DataGridViewSelectNextColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        DataGridViewSelectNextColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        DataGridViewSelectNextRow(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        DataGridViewSelectNextRow(GetActiveTabDataGridView());
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

        #region ComboBoxSelectNext
        private void ComboBoxSelectNext()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                if (controlPasteWithFocusTag is KryptonComboBox)
                {
                    KryptonComboBox kryptonComboBox = (KryptonComboBox)controlPasteWithFocusTag;

                    string text = kryptonComboBox.Text;

                    #region Cursor Position
                    int selectionCursorPositon = kryptonComboBox.SelectionStart + kryptonComboBox.SelectionLength;
                    if (selectionCursorPositon == text.Length) selectionCursorPositon = 0;
                    #endregion

                    #region Find Start Position - Jump over space - In Front
                    int newStartingPosition = selectionCursorPositon + 1;
                    while (newStartingPosition < text.Length - 1 && text.IndexOf(" ", newStartingPosition) == newStartingPosition)
                        newStartingPosition = newStartingPosition + 1;

                    if (newStartingPosition == text.Length - 1) newStartingPosition = 0;
                    #endregion

                    #region Find End Position
                    int selectionEndPoint = text.IndexOf(" ", newStartingPosition);
                    if (selectionEndPoint == -1) selectionEndPoint = text.Length;
                    #endregion

                    #region Selection Start and Length
                    int selectionStart = newStartingPosition;
                    int selectionLength = selectionEndPoint - selectionStart;
                    #endregion

                    kryptonComboBox.Select(selectionStart, selectionLength);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewSelectNextColumn
        private void DataGridViewSelectNextColumn(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.ColumnCount == 0 || dataGridView.RowCount == 0) return;
            int selectColumnIndex = dataGridView.CurrentCell.ColumnIndex + 1;
            if (selectColumnIndex > dataGridView.ColumnCount - 1) selectColumnIndex = 0;
            dataGridView.CurrentCell = dataGridView[selectColumnIndex, 0];
            DataGridViewHandler.SelectColumnRows(dataGridView, selectColumnIndex);
        }
        #endregion

        #region DataGridViewSelectNextRow
        private void DataGridViewSelectNextRow(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.ColumnCount == 0 || dataGridView.RowCount == 0) return;
            int selectRowIndex = dataGridView.CurrentCell.RowIndex + 1;
            if (selectRowIndex > dataGridView.RowCount - 1) selectRowIndex = 0;
            dataGridView.ClearSelection();
            dataGridView.CurrentCell = dataGridView[0, selectRowIndex];
            dataGridView.Rows[selectRowIndex].Selected = true;
        }
        #endregion

        #endregion 

        #region Select Match

        #region Select Match - Click Events Sources
        private void kryptonRibbonGroupButtonSelectEqual_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select Equal")) return;
            if (IsPopulatingAnything("Select Equal")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectMatch();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonQATButtonSelectEqual_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select Equal")) return;
            if (IsPopulatingAnything("Select Equal")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectMatch();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ActionSelectMatch
        private void ActionSelectMatch()
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
                        ImageListViewSelectMatch();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        if (controlPasteWithFocusTag is KryptonComboBox) ComboBoxSelectMatch();
                        if (controlPasteWithFocusTag is KryptonDataGridView) DataGridViewSelectMatch(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        DataGridViewSelectMatch(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        DataGridViewSelectMatch(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DataGridViewSelectMatch(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        DataGridViewSelectMatch(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        DataGridViewSelectMatch(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        DataGridViewSelectMatch(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        DataGridViewSelectMatch(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        DataGridViewSelectMatch(GetActiveTabDataGridView());
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

        #region ComboBoxSelectMatch
        private void ComboBoxSelectMatch()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                if (controlPasteWithFocusTag is KryptonComboBox)
                {
                    KryptonComboBox kryptonComboBox = (KryptonComboBox)controlPasteWithFocusTag;

                    string text = kryptonComboBox.Text;
                    int selectionStartingPoint = kryptonComboBox.SelectionStart;
                    int selectionEnd = text.IndexOf(" ", selectionStartingPoint);
                    int selectionStart = text.LastIndexOf(" ", selectionStartingPoint) + 1;
                    int selectionLength = selectionEnd - selectionStart;
                    kryptonComboBox.Select(selectionStart, selectionLength);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewSelectMatch
        private void DataGridViewSelectMatch(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.ColumnCount == 0 || dataGridView.RowCount == 0) return;
            string selectMatch = (string)dataGridView.CurrentCell.Value;
            dataGridView.ClearSelection();
            foreach (DataGridViewRow dataGridViewRow in dataGridView.Rows)
            {
                foreach (DataGridViewCell dataGridViewCell in dataGridViewRow.Cells)
                {
                    if ((string)dataGridViewCell.Value == selectMatch) dataGridViewCell.Selected = true;
                }
            }
        }
        #endregion


        #endregion

        #region Select Previous

        #region Select Previous - Click Events Sources
        private void kryptonRibbonQATButtonSelectPrevious_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select Previous")) return;
            if (IsPopulatingAnything("Select Backwards")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectPrevious();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonSelectBackwards_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select Backwards")) return;
            if (IsPopulatingAnything("Select Backwards")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectPrevious();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ActionSelectPrevious
        private void ActionSelectPrevious()
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
                        ImageListViewSelectPrevious();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        if (controlPasteWithFocusTag is KryptonComboBox) ComboBoxSelectPrevious();
                        if (controlPasteWithFocusTag is KryptonDataGridView) DataGridViewSelectPreviousColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        DataGridViewSelectPreviousColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        DataGridViewSelectPreviousColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DataGridViewSelectPreviousColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        DataGridViewSelectPreviousColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        DataGridViewSelectPreviousColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        DataGridViewSelectPreviousColumn(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        DataGridViewSelectPreviousRow(GetActiveTabDataGridView());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        DataGridViewSelectPreviousRow(GetActiveTabDataGridView());
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

        #region ComboBoxSelectPrevious
        private void ComboBoxSelectPrevious()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                if (controlPasteWithFocusTag is KryptonComboBox)
                {
                    KryptonComboBox kryptonComboBox = (KryptonComboBox)controlPasteWithFocusTag;

                    string text = kryptonComboBox.Text;

                    #region Cursor Position
                    int selectionCursorPositon = kryptonComboBox.SelectionStart; // + kryptonComboBox.SelectionLength;
                    if (selectionCursorPositon == 0) selectionCursorPositon = text.Length - 1;
                    #endregion

                    #region Find End Position - Jump over space - From Back
                    int newEndPosition = selectionCursorPositon;
                    while (newEndPosition > 0 && text.LastIndexOf(" ", newEndPosition - 1) == newEndPosition - 1)
                        newEndPosition = newEndPosition - 1;

                    if (newEndPosition == 0) newEndPosition = text.Length - 1;
                    #endregion

                    #region Find Start Position
                    int selectionStartPoint = text.LastIndexOf(" ", newEndPosition - 1) + 1;
                    if (selectionStartPoint == text.Length - 1) selectionStartPoint = 0;
                    #endregion

                    #region Selection Start and Length
                    int selectionStart = selectionStartPoint;
                    int selectionLength = newEndPosition - selectionStart;
                    #endregion

                    kryptonComboBox.Select(selectionStart, selectionLength);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridViewSelectPreviousColumn
        private void DataGridViewSelectPreviousColumn(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.ColumnCount == 0 || dataGridView.RowCount == 0) return;
            int selectColumnIndex = dataGridView.CurrentCell.ColumnIndex - 1;
            if (selectColumnIndex < 0) selectColumnIndex = dataGridView.ColumnCount - 1;
            dataGridView.ClearSelection();
            dataGridView.CurrentCell = dataGridView[selectColumnIndex, 0];
            DataGridViewHandler.SelectColumnRows(dataGridView, selectColumnIndex);
        }
        #endregion

        #region DataGridViewSelectPreviousRow
        private void DataGridViewSelectPreviousRow(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.ColumnCount == 0 || dataGridView.RowCount == 0) return;
            int selectRowIndex = dataGridView.CurrentCell.RowIndex - 1;
            if (selectRowIndex < 0) selectRowIndex = dataGridView.RowCount - 1;
            dataGridView.ClearSelection();
            dataGridView.CurrentCell = dataGridView[0, selectRowIndex];
            dataGridView.Rows[selectRowIndex].Selected = true;
        }
        #endregion

        #endregion

        #region Cut

        #region Cut - Click Events Sources       
        private void kryptonRibbonGroupButtonHomeCut_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Cut")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionCut();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericCut_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Cut")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionCut();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        FolderCut();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesCut();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        TagsAndKeywordsCut();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleCut();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapCut();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateCut();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolCut();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningCut();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesCut();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameCut();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeCut();
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

        #region MediaFilesCut
        private void MediaFilesCut()
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

        #region FolderCut
        private void FolderCut()
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

        #region DataGridGenericCut
        private void DataGridGenericCut(DataGridView dataGridView)
        {
            try
            {
                if (dataGridView == null || dataGridView.RowCount == 0 || dataGridView.ColumnCount == 0) return;
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
                if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ConvertAndMergeCut
        private void ConvertAndMergeCut()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridGenericCut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateCut
        private void DateCut()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridGenericCut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolCut
        private void ExiftoolCut()
        {
            try {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridGenericCut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningCut
        private void ExiftoolWarningCut()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridGenericCut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapCut
        private void MapCut()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridGenericCut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleCut
        private void PeopleCut()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
                ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
                if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesCut
        private void PropertiesCut()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                DataGridGenericCut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameCut
        private void RenameCut()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridGenericCut(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsCut
        private void TagsAndKeywordsCut()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                if (dataGridView == null || dataGridView.RowCount == 0 || dataGridView.ColumnCount == 0) return;
                if (!dataGridView.Enabled) return;

                if (dataGridView.CurrentCell.IsInEditMode)
                    GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

                ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, 0, dataGridView.Columns.Count - 1,
                    DataGridViewHandler.GetRowHeadingIndex(dataGridView, header),
                    DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header), true, 
                    out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
                if (!textBoxSelectionCanRestore) ValitedatePasteKeywords(dataGridView, header);
                if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Copy")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionCopy();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericCopy_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Copy")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionCopy();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        FolderCopy();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        TagsAndKeywordsCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeCopy();
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

        #region MediaFilesCopy
        private void MediaFilesCopy()
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

        #region FolderCopy
        private void FolderCopy()
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
        private void DataGridGenericCopy(DataGridView dataGridView)
        {
            try
            {
                if (!dataGridView.Enabled) return;
                ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ConvertAndMergeCopy
        private void ConvertAndMergeCopy()
        {
            try
            {
                DataGridView dataGridView = dataGridViewConvertAndMerge;
                DataGridGenericCopy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DateCopy
        private void DateCopy()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                DataGridGenericCopy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolCopy
        private void ExiftoolCopy()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                DataGridGenericCopy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningCopy
        private void ExiftoolWarningCopy()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                DataGridGenericCopy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapCopy
        private void MapCopy()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridGenericCopy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleCopy
        private void PeopleCopy()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                DataGridGenericCopy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
}
        #endregion

        #region PropertiesCopy
        private void PropertiesCopy()
        {
            try { 
            DataGridView dataGridView = dataGridViewProperties;
            DataGridGenericCopy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameCopy
        private void RenameCopy()
        {
            try
            {
                DataGridView dataGridView = dataGridViewRename;
                DataGridGenericCopy(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region TagsAndKeywordsCopy
        private void TagsAndKeywordsCopy()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                DataGridGenericCopy(dataGridView);
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Paste")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionPaste();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void KryptonContextMenuItemGenericPaste_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Paste")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionPaste();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        FolderPaste();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesPaste();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        TagsAndKeywordsPaste();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeoplePaste();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapPaste();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DatePaste();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolPaste();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningPaste();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesPaste();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenamePaste();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergePaste();
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

        #region MediaFilesPaste
        private void MediaFilesPaste()
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

                        if (!fileFound)
                        {
                            lock (keepTrackOfLoadedMetadataLock)
                            {
                                ImageListViewHandler.ImageListViewAddItem(imageListView1, fullFilename, ref hasTriggerLoadAllMetadataActions, ref keepTrackOfLoadedMetadata);
                            }
                        }
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

        #region FolderPaste
        private void FolderPaste()
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
                ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
                DataGridView_UpdatedDirtyFlags(dataGridView);
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

        #region ConvertAndMergePaste
        private void ConvertAndMergePaste()
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

        #region DatePaste
        private void DatePaste()
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

        #region ExiftoolPaste
        private void ExiftoolPaste()
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

        #region ExiftoolWarningPaste
        private void ExiftoolWarningPaste()
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

        #region MapPaste
        private void MapPaste()
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

        #region PeoplePaste
        private void PeoplePaste()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, -1, -1, -1, -1, false, 
                    out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                //ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
                if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
                DataGridView_UpdatedDirtyFlags(dataGridView);
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

        #region PropertiesPaste
        private void PropertiesPaste()
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

        #region RenamePaste
        private void RenamePaste()
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

        #region TagsAndKeywordsPaste
        private void TagsAndKeywordsPaste()
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

                        //DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, true);
                        ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(
                            dataGridView, 0, dataGridView.Columns.Count - 1,
                            DataGridViewHandler.GetRowHeadingIndex(dataGridView, header),
                            DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header), true,
                            out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                        if (!textBoxSelectionCanRestore) ValitedatePasteKeywords(dataGridView, header);
                        if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                        ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
                        DataGridView_UpdatedDirtyFlags(dataGridView);
                        //DataGridViewHandler.ResumeLayoutDelayed(dataGridView);
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Delete")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionGridCellAndFileSystemDelete();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericFileSystemDelete_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Delete")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionGridCellAndFileSystemDelete();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        FileSystemFolderDelete();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        FileSystemSelectedFilesDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        TagsAndKeywordsDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeDelete();
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

        #region FileSystemSelectedFilesDelete
        private void FileSystemSelectedFilesDelete()
        {
            if (GlobalData.IsApplicationClosing) return;            
            try
            {
                try
                {
                    HashSet<string> selectedFiles = ImageListViewHandler.GetFilesSelectedItemsCache(imageListView1);
                    if (IsFileInAnyQueueLock(selectedFiles))
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
                            ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Syntax error", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }

            imageListView1.Focus();
            DisplayAllQueueStatus();
        }
        #endregion

        #region FileSystemFolderDelete
        private void FileSystemFolderDelete()
        {
            if (GlobalData.IsApplicationClosing) return;            
            try
            {
                string folder = GetSelectedNodeFullRealPath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't delete folder. No valid folder selected.", "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }

                if (IsFolderInAnyQueueLock(folder))
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
                            ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(false, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Error when delete folder.");

                    AddError(
                        folder,
                        AddErrorFileSystemRegion, AddErrorFileSystemDeleteFolder, folder, folder,
                        "Issue: Was not able to delete folder with files and subfolder!\r\n" +
                        "From Folder: " + folder + "\r\n" +
                        "Error message: " + ex.Message);
                }
                finally
                {
                    GlobalData.DoNotTrigger_ImageListView_ItemUpdate = false;
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
                ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
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

        #region ConvertAndMergeDelete
        private void ConvertAndMergeDelete()
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

        #region DateDelete
        private void DateDelete()
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

        #region ExiftoolDelete
        private void ExiftoolDelete()
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

        #region ExiftoolWarningDelete
        private void ExiftoolWarningDelete()
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

        #region MapDelete
        private void MapDelete()
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

        #region PeopleDelete
        private void PeopleDelete()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
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

        #region PropertiesDelete
        private void PropertiesDelete()
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

        #region RenameDelete
        private void RenameDelete()
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

        #region TagsAndKeywordsDelete()
        private void TagsAndKeywordsDelete()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

                ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, 0, dataGridView.Columns.Count - 1,
                    DataGridViewHandler.GetRowHeadingIndex(dataGridView, header),
                    DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header), true,
                    out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
                ValitedatePasteKeywords(dataGridView, header);
                if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
                ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Undo")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionUndo();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericUndo_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Undo")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionUndo();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        TagsAndKeywordsUndo();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleUndo();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapUndo();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateUndo();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolUndo();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningUndo();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesUndo();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameUndo();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeUndo();
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
                DataGridView_UpdatedDirtyFlags(dataGridView);
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

        #region ConvertAndMergeUndo
        private void ConvertAndMergeUndo()
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

        #region DateUndo
        private void DateUndo()
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

        #region ExiftoolUndo
        private void ExiftoolUndo()
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

        #region ExiftoolWarningUndo
        private void ExiftoolWarningUndo()
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

        #region MapUndo
        private void MapUndo()
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

        #region PeopleUndo
        private void PeopleUndo()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.UndoDataGridView(dataGridView);
                //ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
                DataGridView_UpdatedDirtyFlags(dataGridView);
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

        #region PropertiesUndo
        private void PropertiesUndo()
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

        #region RenameUndo
        private void RenameUndo()
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

        #region TagsAndKeywordsUndo
        private void TagsAndKeywordsUndo()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                if (!dataGridView.Enabled) return;
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
                ClipboardUtility.UndoDataGridView(dataGridView);
                ValitedatePasteKeywords(dataGridView, header);
                DataGridView_UpdatedDirtyFlags(dataGridView);
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Redo")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRedo();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void KryptonContextMenuItemGenericRedo_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Redo")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRedo();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        TagsAndKeywordsRedo();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleRedo();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapRedo();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateRedo();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolRedo();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningRedo();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesRedo();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameRedo();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeRedo();
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
                DataGridView_UpdatedDirtyFlags(dataGridView);
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

        #region ConvertAndMergeRedo
        private void ConvertAndMergeRedo()
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

        #region DateRedo
        private void DateRedo()
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

        #region ExiftoolRedo
        private void ExiftoolRedo()
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

        #region ExiftoolWarningRedo
        private void ExiftoolWarningRedo()
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

        #region MapRedo
        private void MapRedo()
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

        #region PeopleRedo
        private void PeopleRedo()
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            try
            {
                GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
                ClipboardUtility.RedoDataGridView(dataGridView);
                //ValitedatePastePeople(dataGridView, DataGridViewHandlerPeople.headerPeople);
                DataGridView_UpdatedDirtyFlags(dataGridView);
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

        #region PropertiesRedo
        private void PropertiesRedo()
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

        #region RenameRedo
        private void RenameRedo()
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

        #region TagsAndKeywordsRedo
        private void TagsAndKeywordsRedo()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Find")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFind();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void KryptonContextMenuItemGenericFind_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Find")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFind();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        FolderSearchFilterFolderFind();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        FolderSearchFilterSearchFind();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesFind();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        TagsAndKeywordsFind();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleFind();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapFind();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateFind();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolFind();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningFind();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesFind();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameFind();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeFind();
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

        #region FolderSearchFilterFolderFind
        private void FolderSearchFilterFolderFind()
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

        #region FolderSearchFilterSearchFind
        private void FolderSearchFilterSearchFind()
        {
            ImageListView_FetchListOfMediaFiles_FromDatabase_and_Aggregate();
        }
        #endregion

        #region MediaFilesFind
        private void MediaFilesFind()
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

        #region ConvertAndMergeFind
        private void ConvertAndMergeFind()
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

        #region DateFind
        private void DateFind()
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

        #region ExiftoolFind
        private void ExiftoolFind()
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

        #region ExiftoolWarningFind
        private void ExiftoolWarningFind()
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

        #region MapFind
        private void MapFind()
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

        #region PeopleFind
        private void PeopleFind()
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

        #region PropertiesFind
        private void PropertiesFind()
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

        #region RenameFind
        private void RenameFind()
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

        #region TagsAndKeywordsFind
        private void TagsAndKeywordsFind()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Replace")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFindAndReplace();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void KryptonContextMenuItemGenericReplace_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Replace")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFindAndReplace();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        TagsAndKeywordsFindAndReplace();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleFindAndReplace();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapFindAndReplace();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateFindAndReplace();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolFindAndReplace();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningFindAndReplace();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesFindAndReplace();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameFindAndReplace();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeFindAndReplace();
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

        #region ConvertAndMergeFindAndReplace
        private void ConvertAndMergeFindAndReplace()
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

        #region DateFindAndReplace
        private void DateFindAndReplace()
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

        #region ExiftoolFindAndReplace
        private void ExiftoolFindAndReplace()
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

        #region ExiftoolWarningFindAndReplace
        private void ExiftoolWarningFindAndReplace()
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

        #region MapFindAndReplace
        private void MapFindAndReplace()
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

        #region PeopleFindAndReplace
        private void PeopleFindAndReplace()
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

        #region PropertiesFindAndReplace
        private void PropertiesFindAndReplace()
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

        #region RenameFindAndReplace
        private void RenameFindAndReplace()
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

        #region TagsAndKeywordsFindAndReplace
        private void TagsAndKeywordsFindAndReplace()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("FavoriteAdd")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFavoriteAdd();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        TagsAndKeywordsFavoriteAdd();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleFavoriteAdd();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapFavoriteAdd();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateFavoriteAdd();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolFavoriteAdd();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningFavoriteAdd();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesFavoriteAdd();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameFavoriteAdd();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeFavoriteAdd();
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

        #region ConvertAndMergeFavoriteAdd
        private void ConvertAndMergeFavoriteAdd()
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

        #region DateFavoriteAdd
        private void DateFavoriteAdd()
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

        #region ExiftoolFavoriteAdd
        private void ExiftoolFavoriteAdd()
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

        #region ExiftoolWarningFavoriteAdd
        private void ExiftoolWarningFavoriteAdd()
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

        #region MapFavoriteAdd
        private void MapFavoriteAdd()
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

        #region PeopleFavoriteAdd
        private void PeopleFavoriteAdd()
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

        #region PropertiesFavoriteAdd
        private void PropertiesFavoriteAdd()
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

        #region RenameFavoriteAdd
        private void RenameFavoriteAdd()
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

        #region TagsAndKeywordsFavoriteAdd
        private void TagsAndKeywordsFavoriteAdd()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("FavoriteDelete")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFavoriteDelete();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        TagsAndKeywordsFavoriteDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleFavoriteDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapFavoriteDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateFavoriteDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolFavoriteDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningFavoriteDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesFavoriteDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameFavoriteDelete();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeFavoriteDelete();
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

        #region ConvertAndMergeFavoriteDelete
        private void ConvertAndMergeFavoriteDelete()
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

        #region DateFavoriteDelete
        private void DateFavoriteDelete()
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

        #region ExiftoolFavoriteDelete
        private void ExiftoolFavoriteDelete()
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

        #region ExiftoolWarningFavoriteDelete
        private void ExiftoolWarningFavoriteDelete()
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

        #region MapFavoriteDelete
        private void MapFavoriteDelete()
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

        #region PeopleFavoriteDelete
        private void PeopleFavoriteDelete()
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

        #region PropertiesFavoriteDelete
        private void PropertiesFavoriteDelete()
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

        #region RenameFavoriteDelete
        private void RenameFavoriteDelete()
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

        #region TagsAndKeywordsFavoriteDelete
        private void TagsAndKeywordsFavoriteDelete()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("FavoriteToggle")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFavoriteToggle();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        TagsAndKeywordsFavoriteToogle();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleFavoriteToogle();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapFavoriteToogle();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateFavoriteToogle();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolFavoriteToogle();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningFavoriteToogle();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesFavoriteToogle();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameFavoriteToogle();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeFavoriteToogle();
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

        #region ConvertAndMergeFavoriteToogle
        private void ConvertAndMergeFavoriteToogle()
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

        #region DateFavoriteToogle
        private void DateFavoriteToogle()
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

        #region ExiftoolFavoriteToogle
        private void ExiftoolFavoriteToogle()
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

        #region ExiftoolWarningFavoriteToogle
        private void ExiftoolWarningFavoriteToogle()
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

        #region MapFavoriteToogle
        private void MapFavoriteToogle()
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

        #region PeopleFavoriteToogle
        private void PeopleFavoriteToogle()
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

        #region PropertiesFavoriteToogle
        private void PropertiesFavoriteToogle()
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

        #region RenameFavoriteToogle
        private void RenameFavoriteToogle()
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

        #region TagsAndKeywordsFavoriteToogle
        private void TagsAndKeywordsFavoriteToogle()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("ShowFavorite")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRowsShowFavoriteToggle();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericRowShowFavorite_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("ShowFavorite")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRowsShowFavoriteToggle();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        TagsAndKeywordsRowsShowFavoriteToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleRowsShowFavoriteToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapRowsShowFavoriteToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateRowsShowFavoriteToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolRowsShowFavoriteToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningRowsShowFavoriteToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesRowsShowFavoriteToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameRowsShowFavoriteToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeRowsShowFavoriteToggle();
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

        #region ConvertAndMergeRowsShowFavoriteToggle
        private void ConvertAndMergeRowsShowFavoriteToggle()
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

        #region DateRowsShowFavoriteToggle
        private void DateRowsShowFavoriteToggle()
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

        #region ExiftoolRowsShowFavoriteToggle
        private void ExiftoolRowsShowFavoriteToggle()
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

        #region ExiftoolWarningRowsShowFavoriteToggle
        private void ExiftoolWarningRowsShowFavoriteToggle()
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

        #region MapRowsShowFavoriteToggle
        private void MapRowsShowFavoriteToggle()
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

        #region PeopleRowsShowFavoriteToggle
        private void PeopleRowsShowFavoriteToggle()
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

        #region PropertiesRowsShowFavoriteToggle
        private void PropertiesRowsShowFavoriteToggle()
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

        #region RenameRowsShowFavoriteToggle
        private void RenameRowsShowFavoriteToggle()
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

        #region TagsAndKeywordsRowsShowFavoriteToggle
        private void TagsAndKeywordsRowsShowFavoriteToggle()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("HideEqual")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRowsHideEqualToggle();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonDataGridViewRowsHideEqual_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("HideEqual")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRowsHideEqualToggle();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        TagsAndKeywordsRowsHideEqualToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleRowsHideEqualToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapRowsHideEqualToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateRowsHideEqualToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolRowsHideEqualToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningRowsHideEqualToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesRowsHideEqualToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameRowsHideEqualToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeRowsHideEqualToggle();
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

        #region DataGridViewGenrericRowsHideEqualToogle
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

        #region ConvertAndMergeRowsHideEqualToggle
        private void ConvertAndMergeRowsHideEqualToggle()
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

        #region DateRowsHideEqualToggle
        private void DateRowsHideEqualToggle()
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

        #region ExiftoolRowsHideEqualToggle
        private void ExiftoolRowsHideEqualToggle()
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

        #region ExiftoolWarningRowsHideEqualToggle
        private void ExiftoolWarningRowsHideEqualToggle()
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

        #region MapRowsHideEqualToggle
        private void MapRowsHideEqualToggle()
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

        #region PeopleRowsHideEqualToggle
        private void PeopleRowsHideEqualToggle()
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

        #region PropertiesRowsHideEqualToggle
        private void PropertiesRowsHideEqualToggle()
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

        #region RenameRowsHideEqualToggle
        private void RenameRowsHideEqualToggle()
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

        #region TagsAndKeywordsRowsHideEqualToggle
        private void TagsAndKeywordsRowsHideEqualToggle()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("CopyText")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionCopyText();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericCopyText_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("CopyText")) return;
            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionCopyText();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        TagsAndKeywordsCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameCopy();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeCopy();
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
                        TagsAndKeywordsTriStateToggle();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleTriStateToggle();
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
                DataGridView_UpdatedDirtyFlags(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsTriStateToggle 
        private void TagsAndKeywordsTriStateToggle()
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

        #region PeopleTriStateToggle
        private void PeopleTriStateToggle()
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
                        TagsAndKeywordsTriStateOn();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleTriStateOn();
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

        #region TagsAndKeywordsTriStateOn
        private void TagsAndKeywordsTriStateOn()
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

        #region PeopleTriStateOn
        private void PeopleTriStateOn()
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
                        TagsAndKeywordsTriStateOff();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleTriStateOff();
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

        #region TagsAndKeywordsTriStateOff
        private void TagsAndKeywordsTriStateOff()
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

        #region PeopleTriStateOff
        private void PeopleTriStateOff()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Save")) return;
            if (IsPopulatingAnything("Save")) return;
            //if (SaveBeforeContinue(true, useAutoSave: true) == DialogResult.Cancel) return;
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                case KryptonPages.kryptonPageMediaFiles:
                case KryptonPages.kryptonPageToolboxTags:
                case KryptonPages.kryptonPageToolboxPeople:
                case KryptonPages.kryptonPageToolboxMap:
                case KryptonPages.kryptonPageToolboxDates:
                case KryptonPages.kryptonPageToolboxExiftool:
                case KryptonPages.kryptonPageToolboxWarnings:
                case KryptonPages.kryptonPageToolboxProperties:
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    if (SaveBeforeContinue(true, useAutoSave: true) == DialogResult.Cancel) return;
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    break;
                default:
                    throw new NotImplementedException();
            }

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSave(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region Save

        #region Save - Click Events Sources
        private void kryptonRibbonQATButtonSave_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Save")) return;
            if (IsPopulatingAnything("Save")) return;
            //if (SaveBeforeContinue(true, useAutoSave: true) == DialogResult.Cancel) return;
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                case KryptonPages.kryptonPageMediaFiles:
                case KryptonPages.kryptonPageToolboxTags:
                case KryptonPages.kryptonPageToolboxPeople:
                case KryptonPages.kryptonPageToolboxMap:
                case KryptonPages.kryptonPageToolboxDates:
                case KryptonPages.kryptonPageToolboxExiftool:
                case KryptonPages.kryptonPageToolboxWarnings:
                case KryptonPages.kryptonPageToolboxProperties:
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    if (SaveBeforeContinue(true, useAutoSave: false) == DialogResult.Cancel) return;
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    break;
                default:
                    throw new NotImplementedException();
            }

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSave(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonHomeSaveSave_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Save")) return;
            if (IsPopulatingAnything("Save")) return;
            //if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                case KryptonPages.kryptonPageMediaFiles:
                case KryptonPages.kryptonPageToolboxTags:
                case KryptonPages.kryptonPageToolboxPeople:
                case KryptonPages.kryptonPageToolboxMap:
                case KryptonPages.kryptonPageToolboxDates:
                case KryptonPages.kryptonPageToolboxExiftool:
                case KryptonPages.kryptonPageToolboxWarnings:
                case KryptonPages.kryptonPageToolboxProperties:
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    if (SaveBeforeContinue(true, useAutoSave: false) == DialogResult.Cancel) return;
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    break;
                default:
                    throw new NotImplementedException();
            }

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSave(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericSave_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Save")) return;
            if (IsPopulatingAnything("Save")) return;
            //if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                case KryptonPages.kryptonPageMediaFiles:
                case KryptonPages.kryptonPageToolboxTags:
                case KryptonPages.kryptonPageToolboxPeople:
                case KryptonPages.kryptonPageToolboxMap:
                case KryptonPages.kryptonPageToolboxDates:
                case KryptonPages.kryptonPageToolboxExiftool:
                case KryptonPages.kryptonPageToolboxWarnings:
                case KryptonPages.kryptonPageToolboxProperties:
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                    if (SaveBeforeContinue(true, useAutoSave: false) == DialogResult.Cancel) return;
                    break;
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    break;
                default:
                    throw new NotImplementedException();
            }

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSave(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion 

        #region ActionSave
        private void ActionSave(bool useAutoCorrect)
        {
            if (GlobalData.IsApplicationClosing) return;

            try
            {
                this.Activate();
                this.Validate(); //Get the latest changes, that are text in edit mode

                this.Enabled = false;
                using (new WaitCursor())
                {
                    switch (ActiveKryptonPage)
                    {
                        case KryptonPages.None:
                            KryptonMessageBox.Show("No active View is selected. You need to select a view so application knows what View to save.", "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                            break;
                        case KryptonPages.kryptonPageFolderSearchFilterFolder:
                            KryptonMessageBox.Show("'Folders view' is avtive. You need to select diffrent view so application knows what View to save.", "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                            break;
                        case KryptonPages.kryptonPageFolderSearchFilterSearch:
                            KryptonMessageBox.Show("'Search view' is avtive. You need to select diffrent view so application knows what View to save.", "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);

                            break;
                        case KryptonPages.kryptonPageFolderSearchFilterFilter:
                            KryptonMessageBox.Show("'Filter view' is avtive. You need to select diffrent view so application knows what View to save.", "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                            break;
                        case KryptonPages.kryptonPageMediaFiles:
                            KryptonMessageBox.Show("'Show Media Files view' is avtive. You need to select diffrent view so application knows what View to save.", "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                            break;
                        case KryptonPages.kryptonPageToolboxTags:
                        case KryptonPages.kryptonPageToolboxPeople:
                        case KryptonPages.kryptonPageToolboxMap:
                        case KryptonPages.kryptonPageToolboxDates:
                            SaveDataGridViewMetadata(useAutoCorrect);
                            GlobalData.IsAgregatedProperties = false;
                            break;
                        case KryptonPages.kryptonPageToolboxExiftool:
                            KryptonMessageBox.Show("'Exiftool view' is avtive. You need to select diffrent view so application knows what View to save.", "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                            break;
                        case KryptonPages.kryptonPageToolboxWarnings:
                            KryptonMessageBox.Show("'Exiftool Warnings' view is avtive. You need to select diffrent view so application knows what View to save.", "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
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
                
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Save - Add AutoKeywords to Metadata Keywords
        private void AutoKeywords(ref Metadata metadataCopy)
        {
            bool writeAddAutokeywords = Properties.Settings.Default.WriteMetadataAddAutoKeywords;
            if (writeAddAutokeywords && metadataCopy != null)
            {
                List<string> newKeywords = AutoKeywordHandler.NewKeywords(autoKeywordConvertions, metadataCopy.LocationName, metadataCopy.PersonalTitle,
                    metadataCopy.PersonalAlbum, metadataCopy.PersonalDescription, metadataCopy.PersonalComments, metadataCopy.PersonalKeywordTags);
                foreach (string keyword in newKeywords)
                {
                    metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(keyword), false);
                }
            }
        }
        #endregion

        #region MicrosoftLocationHack
        private void MicrosoftLocationHack(ref Metadata metadataToSave, Metadata metadataListOriginal, bool useMicrosoftLocationHack, string filenamePostfix)
        {
            if (useMicrosoftLocationHack)
            {

                if (Properties.Settings.Default.MicosoftOneDriveLocationHackUse)
                {
                    if (metadataToSave.LocationCoordinate != metadataListOriginal.LocationCoordinate)
                    {
                        string oldDirectory = metadataToSave.FileDirectory;
                        string oldFilename = metadataToSave.FileName;
                        string oldFullFilename = metadataToSave.FileFullPath;
                        string newDirectory = metadataToSave.FileDirectory;
                        string newFilename = Path.GetFileNameWithoutExtension(oldFullFilename) + filenamePostfix + Path.GetExtension(oldFullFilename);
                        string newFullFilename = Path.Combine(newDirectory, newFilename);
                        metadataToSave.FileName = newFilename;

                        ImageListViewItem imageListViewItem = ImageListViewHandler.FindItem(imageListView1.Items, oldFullFilename);
                        if (imageListViewItem != null) imageListViewItem.FileFullPath = newFullFilename;
                        else
                        {
                            //DEBUG
                        }
                        //ImageListViewHandler.ClearAllAndCaches(imageListView1);
                        ImageListViewHandler.ClearCacheFileEntries(imageListView1);
                        ImageListViewHandler.ClearCacheFileEntriesSelectedItems(imageListView1);
                        try
                        {
                            if (GlobalData.IsAgregatedTags) DataGridViewUpdatedFilenameColumns(dataGridViewTagsAndKeywords, oldFullFilename, newFullFilename);
                            if (GlobalData.IsAgregatedMap) DataGridViewUpdatedFilenameColumns(dataGridViewMap, oldFullFilename, newFullFilename);
                            if (GlobalData.IsAgregatedPeople) DataGridViewUpdatedFilenameColumns(dataGridViewPeople, oldFullFilename, newFullFilename);
                            if (GlobalData.IsAgregatedDate) DataGridViewUpdatedFilenameColumns(dataGridViewDate, oldFullFilename, newFullFilename);

                            if (GlobalData.IsAgregatedExiftoolTags) DataGridViewUpdatedFilenameColumns(dataGridViewExiftool, oldFullFilename, newFullFilename);
                            if (GlobalData.IsAgregatedExiftoolWarning) DataGridViewUpdatedFilenameColumns(dataGridViewExiftoolWarning, oldFullFilename, newFullFilename);
                            if (GlobalData.IsAgregatedProperties) DataGridViewUpdatedFilenameColumns(dataGridViewProperties, oldFullFilename, newFullFilename);

                            if (GlobalData.IsAgregatedConvertAndMerge) DataGridViewUpdatedFilenameConvertAndMergeRows(dataGridViewConvertAndMerge, oldFullFilename, newFullFilename);
                            if (GlobalData.IsAgregatedRename) DataGridViewUpdatedFilenameRenameRows(dataGridViewRename, oldFullFilename, newFullFilename);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                            KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                        }


                        databaseAndCacheThumbnailPoster.Move(oldDirectory, oldFilename, newDirectory, newFilename);
                        if (!databaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename))
                        {
                            filesCutCopyPasteDrag.DeleteFileAndHistory(oldFilename);
                            databaseAndCacheThumbnailPoster.Move(oldDirectory, oldFilename, newDirectory, newFilename);
                            databaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename);
                        }
                    }
                }
            }
        }
        #endregion

        #region Save - SaveDataGridViewMetadata
        private void SaveDataGridViewMetadata(bool useAutoCorrect)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (!HasDataGridViewAggregatedAny()) return;

            try
            {
                CollectMetadataFromAllDataGridViewData(out List <Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView, true);

                if (metadataListOriginalExiftool.Count != metadataListFromDataGridView.Count)
                {
                    KryptonMessageBox.Show("Error occure", "Can't read and macth the data...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }
                
                bool changesFound = false;
                using (new WaitCursor())
                {
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;


                    for (int index = 0; index < metadataListOriginalExiftool.Count; index++)
                    {
                        Metadata metadataFromDataGridView = metadataListFromDataGridView[index];

                        Metadata metadataToSave;
                        if (useAutoCorrect)
                        {
                            metadataToSave = autoCorrect.RunAlgorithmReturnCopy(metadataFromDataGridView,
                                databaseAndCacheMetadataExiftool,
                                databaseAndCacheMetadataMicrosoftPhotos,
                                databaseAndCacheMetadataWindowsLivePhotoGallery,
                                databaseAndCahceCameraOwner,
                                databaseLocationNameAndLookUp,
                                databaseGoogleLocationHistory,
                                locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                autoKeywordConvertions,
                                Properties.Settings.Default.RenameDateFormats);

                            if (Properties.Settings.Default.WriteAutoKeywordsSynonyms) AutoKeywords(ref metadataToSave);
                            if (Properties.Settings.Default.WriteUsingCompatibilityCheck) AutoCorrect.CompatibilityCheckMetadata(ref metadataToSave, fixDateTaken: false);
                        }
                        else
                        {
                            metadataToSave = new Metadata(metadataFromDataGridView);
                            if (Properties.Settings.Default.WriteUsingCompatibilityCheck) AutoCorrect.CompatibilityCheckMetadata(ref metadataToSave, fixDateTaken: true);
                        }

                        if (metadataToSave != metadataListOriginalExiftool[index])
                        {
                            changesFound = true;

                            MicrosoftLocationHack(ref metadataToSave, metadataListOriginalExiftool[index], Properties.Settings.Default.MicosoftOneDriveLocationHackUse, Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix);
                            DataGridView_Populate_CompatibilityCheckedMetadataToSave(metadataToSave, FileEntryVersion.MetadataToSave);
                            AddQueueSaveUsingExiftoolMetadataUpdatedByUserLock(metadataToSave, metadataListOriginalExiftool[index]);
                        }
                    }
                }

                if (!changesFound)
                {
                    KryptonMessageBox.Show(
                        "Can't find any value that was changed.\r\n" +
                        "Compatibility Check and Fix can replace values back to original values.\t\n" +
                        (useAutoCorrect ? "Please note AutoCorrect can have changed back information." : ""),
                        "Nothing to save...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }
                ThreadSaveUsingExiftoolToMedia();
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
            if (GlobalData.IsApplicationClosing) return;
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
                                FileStatus fileStatus = FileHandler.GetFileStatus(
                                    dataGridViewGenericColumn.FileEntryAttribute.FileFullPath, checkLockedStatus: true,
                                    hasErrorOccured: true, errorMessage: ex.Message);
                                ImageListView_UpdateItemFileStatusInvoke(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath, fileStatus);

                                string writeErrorDesciption =
                                    "Issue: Error writing properties to file.\r\n" +
                                    "File name:  " + dataGridViewGenericColumn.FileEntryAttribute.FileFullPath + "\r\n" +
                                    "File staus: " + fileStatus.ToString() + "\r\n" +
                                    "Error message: " + ex.Message;

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
                    
                    AddQueueLazyLoadningAllVersionsAllSourcesMetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));

                    ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
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
            if (GlobalData.IsApplicationClosing) return;
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
                        if (found)
                        {
                            lock (keepTrackOfLoadedMetadataLock)
                            {
                                ImageListViewHandler.ImageListViewAddItem(imageListView1, outputFile, ref hasTriggerLoadAllMetadataActions, ref keepTrackOfLoadedMetadata);
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
                        TagsAndKeywordsFastCopyTextNoOverwrite();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapFastCopyTextNoOverwrite();
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

        #region TagsAndKeywordsFastCopyTextNoOverwrite
        private void TagsAndKeywordsFastCopyTextNoOverwrite()
        {
            try
            {
                DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, null, false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapFastCopyTextNoOverwrite
        private void MapFastCopyTextNoOverwrite()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridView, DataGridViewHandlerMap.headerMedia, DataGridViewHandlerMap.tagMediaCoordinates, false);
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
                        TagsAndKeywordsFastCopyTextOverwrite();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapFastCopyTextAndOverwrite();
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
        private void TagsAndKeywordsFastCopyTextOverwrite()
        {
            try
            {
                DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, null, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapFastCopyTextAndOverwrite_Click       
        private void MapFastCopyTextAndOverwrite()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridView, DataGridViewHandlerMap.headerMedia, DataGridViewHandlerMap.tagMediaCoordinates, true);
                
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
                if (dataGridView == null || dataGridView.SelectedCells.Count == 0) return; 
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Rotate270")) return;
            if (IsPopulatingAnything("Rotate270")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRotate270();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void KryptonContextMenuItemGenericRotate270_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Rotate270")) return;
            if (IsPopulatingAnything("Rotate270")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRotate270();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        MediaRotate270();
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

        #region MediaRotate270
        private void MediaRotate270()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Rotate180")) return;
            if (IsPopulatingAnything("Rotate180")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRotate180();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonMediaFileRotate180_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Rotate180")) return;
            if (IsPopulatingAnything("Rotate180")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRotate180();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        MediaRotate180();
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

        #region MediaRotate180
        private void MediaRotate180()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Rotate90")) return;
            if (IsPopulatingAnything("Rotate90")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRotate90();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void KryptonContextMenuItemGenericRotate90_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Rotate90")) return;
            if (IsPopulatingAnything("Rotate90")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRotate90();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        MediaRotate90();
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

        #region MediaRotate90
        private void MediaRotate90()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("ViewAsPoster")) return;
            if (IsPopulatingAnything("ViewAsPoster")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionMediaViewAsPoster();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonQATButtonMediaPoster_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("ViewAsPoster")) return;
            if (IsPopulatingAnything("ViewAsPoster")) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionMediaViewAsPoster();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonDatGridShowPoster_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("ViewAsPoster")) return;
            if (IsPopulatingAnything("ViewAsPoster")) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionMediaViewAsPoster();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        FolderMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        TagsAndKeywordsMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        ExiftoolWarningMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameMediaViewAsPoster();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeMediaViewAsPoster();
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

        #region GenericMediaViewAsPoster
        private void GenericMediaViewAsPoster(DataGridView dataGridView)
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

        #region FolderMediaViewAsPoster
        private void FolderMediaViewAsPoster()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaViewAsPoster(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region MediaFilesMediaViewAsPoster
        private void MediaFilesMediaViewAsPoster()
        {
            try
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                GenericMediaViewAsPoster(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region DateMediaViewAsPoster
        private void DateMediaViewAsPoster()
        {
            try
            {
                DataGridView dataGridView = dataGridViewDate;
                GenericMediaViewAsPoster(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolMediaViewAsPoster
        private void ExiftoolMediaViewAsPoster()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftool;
                GenericMediaViewAsPoster(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ExiftoolWarningMediaViewAsPoster
        private void ExiftoolWarningMediaViewAsPoster()
        {
            try
            {
                DataGridView dataGridView = dataGridViewExiftoolWarning;
                GenericMediaViewAsPoster(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region MapMediaViewAsPoster
        private void MapMediaViewAsPoster()
        {
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                GenericMediaViewAsPoster(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PeopleMediaViewAsPoster
        private void PeopleMediaViewAsPoster()
        {
            try
            {
                DataGridView dataGridView = dataGridViewPeople;
                GenericMediaViewAsPoster(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TagsAndKeywordsMediaViewAsPoster
        private void TagsAndKeywordsMediaViewAsPoster()
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                GenericMediaViewAsPoster(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region PropertiesMediaViewAsPoster
        private void PropertiesMediaViewAsPoster()
        {
            try
            {
                DataGridView dataGridView = dataGridViewProperties;
                GenericMediaViewAsPoster(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region RenameMediaViewAsPoster
        private void RenameMediaViewAsPoster()
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

        #region ConvertAndMergeMediaViewAsPoster
        private void ConvertAndMergeMediaViewAsPoster()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("MediaViewAsFull")) return;
            if (IsPopulatingAnything("MediaViewAsFull")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionMediaViewAsFull();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonQATButtonMediaPreview_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("MediaViewAsFull")) return;
            if (IsPopulatingAnything("MediaViewAsFull")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionMediaViewAsFull();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                        FolderMediaViewAsFull();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaviewHoveredItemMediaViewAsFull();
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        TagsAndKeywordMediaPreview();
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        PeopleMediaPreview();
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MapMediaPreview();
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        DateMediaPreview();
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        ExiftoolMediaPreview();
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        WarningsMediaPreview();
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        PropertiesMediaPreview();
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        RenameMediaPreview();
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        ConvertAndMergeMediaPreview();
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

        private void FolderMediaViewAsFull()
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

        #region MediaviewHoveredItemMediaViewAsFull
        private void MediaviewHoveredItemMediaViewAsFull()
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
                if (dataGridView == null || dataGridView.RowCount == 0 || dataGridView.ColumnCount == 0) return;

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

        #region TagsAndKeywordMediaPreview
        private void TagsAndKeywordMediaPreview()
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

        #region DateMediaPreview
        private void DateMediaPreview()
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

        #region PeopleMediaPreview
        private void PeopleMediaPreview()
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

        #region MapMediaPreview
        private void MapMediaPreview()
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

        #region ExiftoolMediaPreview
        private void ExiftoolMediaPreview()
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

        #region WarningsMediaPreview
        private void WarningsMediaPreview()
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

        #region PropertiesMediaPreview
        private void PropertiesMediaPreview()
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

        #region RenameMediaPreview
        private void RenameMediaPreview()
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

        #region ConvertAndMergeMediaPreview
        private void ConvertAndMergeMediaPreview()
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
                        FolderRefresh();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesRefresh();
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Refresh Folder")) return;
            if (IsPopulatingAnything("Refresh Folder")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRefreshFolderAndFiles();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericFileSystemRefreshFolder_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Refresh Folder")) return;
            if (IsPopulatingAnything("Refresh Folder")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionRefreshFolderAndFiles();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region FolderRefresh
        private void FolderRefresh()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = true;
                TreeNodePath selectedNode = (TreeNodePath)treeViewFolderBrowser1.SelectedNode;
                TreeViewFolderBrowserHandler.RefreshTreeNode(treeViewFolderBrowser1, selectedNode);
                GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = false;
                ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(false, true);
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
        private void MediaFilesRefresh()
        {
            if (GlobalData.IsApplicationClosing) return;            
            try
            {
                ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(false, true);
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
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        FolderReadSubfolders();
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("ReadSubfolders")) return;
            if (IsPopulatingAnything("ReadSubfolders")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionReadSubfolders();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region FolderReadSubfolders
        private void FolderReadSubfolders()
        {
            if (GlobalData.IsApplicationClosing) return;

            try
            {
                ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(true, true);
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

        #region ActionOpenExplorerLocation
        private void ActionOpenExplorerLocation()
        {
            try
            {
                switch (ActiveKryptonPage)
                {
                    case KryptonPages.None:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFolder:
                        FolderOpenExplorerLocation();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesOpenExplorerLocation(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        MediaFilesOpenExplorerLocation(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        MediaFilesOpenExplorerLocation(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MediaFilesOpenExplorerLocation(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        MediaFilesOpenExplorerLocation(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        MediaFilesOpenExplorerLocation(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        MediaFilesOpenExplorerLocation(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        MediaFilesOpenExplorerLocation(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        MediaFilesOpenExplorerLocation(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        MediaFilesOpenExplorerLocation(DataGridView_GetSelectedFilesFromActive());
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

        #region OpenExplorerLocation - Click Events Sources
        private void kryptonRibbonGroupButtonHomeFileSystemOpenExplorerLocation_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("OpenExplorerLocation")) return;
            if (IsPopulatingAnything("OpenExplorerLocation")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionOpenExplorerLocation();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericOpenExplorerLocation_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("OpenExplorerLocation")) return;
            if (IsPopulatingAnything("OpenExplorerLocation")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionOpenExplorerLocation();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region MediaFilesOpenExplorerLocation
        private void MediaFilesOpenExplorerLocation(HashSet<FileEntry> files)
        {
            if (GlobalData.IsApplicationClosing) return;
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

        #region FolderOpenExplorerLocation
        private void FolderOpenExplorerLocation()
        {
            if (GlobalData.IsApplicationClosing) return;
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("FileSystemOpen")) return;
            if (IsPopulatingAnything("FileSystemOpen")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                //GlobalData.IsPerformingAButtonAction = true;
                ActionFileSystemVerbOpen(null, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                //GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonHomeFileSystemOpen_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("FileSystemOpen")) return;
            if (IsPopulatingAnything("FileSystemOpen")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFileSystemVerbOpen(null, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericOpen_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("FileSystemOpen")) return;
            if (IsPopulatingAnything("FileSystemOpen")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFileSystemVerbOpen(null, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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

        #region KryptonContextMenuItemOpenWithSelectedVerb
        private void KryptonContextMenuItemOpenWithSelectedVerb(object sender, EventArgs e)
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
                        MediaFilesOpenAndAssociateWithDialog(GetFilesInSelectedFolderCached());
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesOpenAndAssociateWithDialog(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        MediaFilesOpenAndAssociateWithDialog(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        MediaFilesOpenAndAssociateWithDialog(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MediaFilesOpenAndAssociateWithDialog(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        MediaFilesOpenAndAssociateWithDialog(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        MediaFilesOpenAndAssociateWithDialog(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        MediaFilesOpenAndAssociateWithDialog(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        MediaFilesOpenAndAssociateWithDialog(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        MediaFilesOpenAndAssociateWithDialog(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        MediaFilesOpenAndAssociateWithDialog(DataGridView_GetSelectedFilesFromActive());
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("OpenAndAssociateWithDialog")) return;
            if (IsPopulatingAnything("OpenAndAssociateWithDialog")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionOpenAndAssociateWithDialog();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonFileSystemOpenAssociateDialog_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("OpenAndAssociateWithDialog")) return;
            if (IsPopulatingAnything("OpenAndAssociateWithDialog")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try { 
            GlobalData.IsPerformingAButtonAction = true;
            ActionOpenAndAssociateWithDialog();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region MediaFilesOpenWithDialog
        private void MediaFilesOpenAndAssociateWithDialog(HashSet<FileEntry> files)
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
                        MediaFilesVerbEdit(GetFilesInSelectedFolderCached());
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesVerbEdit(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        MediaFilesVerbEdit(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        MediaFilesVerbEdit(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        MediaFilesVerbEdit(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        MediaFilesVerbEdit(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        MediaFilesVerbEdit(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        MediaFilesVerbEdit(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        MediaFilesVerbEdit(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        MediaFilesVerbEdit(DataGridView_GetSelectedFilesFromActive());
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        MediaFilesVerbEdit(DataGridView_GetSelectedFilesFromActive());
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("FileSystemVerbEdit")) return;
            if (IsPopulatingAnything("FileSystemVerbEdit")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFileSystemVerbEdit();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void KryptonContextMenuItemGenericFileSystemVerbEdit_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("FileSystemVerbEdit")) return;
            if (IsPopulatingAnything("FileSystemVerbEdit")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFileSystemVerbEdit();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region MediaFilesVerbEdit
        private void MediaFilesVerbEdit(HashSet<FileEntry> files)
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("RunCommand")) return;
            if (IsPopulatingAnything("RunCommand")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFileSystemRunCommand();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericFileSystemRunCommand_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("RunCommand")) return;
            if (IsPopulatingAnything("RunCommand")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionFileSystemRunCommand();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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

                    string writeMetadataTagsConfiguration = Properties.Settings.Default.WriteMetadataTags;
                    string writeMetadataKeywordAddConfiguration = Properties.Settings.Default.WriteMetadataKeywordAdd;

                    List<string> allowedFileNameDateTimeFormats = FileDateTime.FileDateTimeReader.ConvertStringOfDatesToList(Properties.Settings.Default.RenameDateFormats);

                    FormSplash.UpdateStatus("Create argument file...");
                    #region Create ArgumentFile file
                    CollectMetadataFromAllDataGridViewData(out List <Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView, false);

                    ExiftoolWriter.CreateExiftoolArguFileText(
                        metadataListFromDataGridView, metadataListOriginalExiftool, allowedFileNameDateTimeFormats, 
                        writeMetadataTagsConfiguration, writeMetadataKeywordAddConfiguration,
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

                            Metadata metadataToSave = autoCorrect.RunAlgorithmReturnCopy(metadata,
                                databaseAndCacheMetadataExiftool,
                                databaseAndCacheMetadataMicrosoftPhotos,
                                databaseAndCacheMetadataWindowsLivePhotoGallery,
                                databaseAndCahceCameraOwner,
                                databaseLocationNameAndLookUp,
                                databaseGoogleLocationHistory,
                                locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                autoKeywordConvertions,
                                Properties.Settings.Default.RenameDateFormats);

                            if (metadataToSave != null) metadataListFromDataGridViewAutoCorrect.Add(new Metadata(metadataToSave));
                            else
                            {
                                Logger.Warn("Metadata was not loaded for file, check if file is only in cloud:" + fileEntry.FileFullPath);
                            }
                            metadataListEmpty.Add(new Metadata(metadataInCache));
                        }
                    }


                    ExiftoolWriter.CreateExiftoolArguFileText(
                        metadataListFromDataGridViewAutoCorrect, metadataListEmpty, allowedFileNameDateTimeFormats,
                        writeMetadataTagsConfiguration, writeMetadataKeywordAddConfiguration,
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
                        AutoCorrectRunFolder(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        AutoCorrectRunMediaFiles(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        AutoCorrectRunDataGridView(FileEntryVersion.CompatibilityFixedAndAutoUpdated);
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        AutoCorrectRunDataGridView(FileEntryVersion.CompatibilityFixedAndAutoUpdated);
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        AutoCorrectRunDataGridView(FileEntryVersion.CompatibilityFixedAndAutoUpdated);
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        AutoCorrectRunDataGridView(FileEntryVersion.CompatibilityFixedAndAutoUpdated);
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        AutoCorrectRunDataGridView(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        AutoCorrectRunDataGridView(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        AutoCorrectRunDataGridView(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        AutoCorrectRunDataGridView(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        AutoCorrectRunDataGridView(FileEntryVersion.MetadataToSave);
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("AutoCorrectRun")) return;
            if (IsPopulatingAnything("AutoCorrectRun")) return;

            switch (ActiveKryptonPage)
            {            
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                case KryptonPages.kryptonPageMediaFiles:
                case KryptonPages.kryptonPageToolboxRename:
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
                    break;
            }

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionAutoCorrectRun();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void kryptonRibbonGroupButtonHomeAutoCorrectRun_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("AutoCorrectRun")) return;
            if (IsPopulatingAnything("AutoCorrectRun")) return;

            switch (ActiveKryptonPage)
            {
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                case KryptonPages.kryptonPageMediaFiles:
                case KryptonPages.kryptonPageToolboxRename:
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
                    break;
            }

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionAutoCorrectRun();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region AutoCorrectRunMediaFiles
        private void AutoCorrectRunMediaFiles(FileEntryVersion fileEntryVersion)
        {
            if (GlobalData.IsApplicationClosing) return;
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
                            Metadata metadataToSave = autoCorrect.RunAlgorithmReturnCopy(metadataInCache,
                                databaseAndCacheMetadataExiftool,
                                databaseAndCacheMetadataMicrosoftPhotos,
                                databaseAndCacheMetadataWindowsLivePhotoGallery,
                                databaseAndCahceCameraOwner,
                                databaseLocationNameAndLookUp,
                                databaseGoogleLocationHistory,
                                locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                autoKeywordConvertions,
                                Properties.Settings.Default.RenameDateFormats);
                            
                            if (metadataToSave != null)
                            {
                                if (Properties.Settings.Default.WriteAutoKeywordsSynonyms) AutoKeywords(ref metadataToSave);
                                if (Properties.Settings.Default.WriteUsingCompatibilityCheck) AutoCorrect.CompatibilityCheckMetadata(ref metadataToSave, fixDateTaken: false);

                                MicrosoftLocationHack(ref metadataToSave, metadataInCache, Properties.Settings.Default.MicosoftOneDriveLocationHackUse, Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix);
                                DataGridView_Populate_CompatibilityCheckedMetadataToSave(metadataToSave, fileEntryVersion);
                                AddQueueSaveUsingExiftoolMetadataUpdatedByUserLock(metadataToSave, new Metadata(metadataInCache));
                                //Need use metadataToSave.FullFilePath, Because When Exiftool output filename can be diffrent to input filename
                                AddQueueRenameMediaFilesLock(metadataToSave.FileFullPath, autoCorrect.RenameVariable);  
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

        #region AutoCorrectRunFolder
        private void AutoCorrectRunFolder(FileEntryVersion fileEntryVersion)
        {
            if (GlobalData.IsApplicationClosing) return;
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

                    IEnumerable<FileData> fileDatas = ImageAndMovieFileExtentionsUtility.GetFilesByEnumerableFast(selectedFolder, false);
                    foreach (FileData file in fileDatas)
                    {
                        FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(file.Path, FileHandler.GetLastWriteTime(file.Path), MetadataBrokerType.ExifTool);
                        Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
                        if (metadataInCache != null)
                        {
                            Metadata metadata = new Metadata(metadataInCache);

                            Metadata metadataToSave = autoCorrect.RunAlgorithmReturnCopy(metadata,
                            databaseAndCacheMetadataExiftool,
                            databaseAndCacheMetadataMicrosoftPhotos,
                            databaseAndCacheMetadataWindowsLivePhotoGallery,
                            databaseAndCahceCameraOwner,
                            databaseLocationNameAndLookUp,
                            databaseGoogleLocationHistory, locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                            autoKeywordConvertions,
                            Properties.Settings.Default.RenameDateFormats);
                            if (metadataToSave != null)
                            {
                                if (Properties.Settings.Default.WriteAutoKeywordsSynonyms) AutoKeywords(ref metadataToSave);
                                if (Properties.Settings.Default.WriteUsingCompatibilityCheck) AutoCorrect.CompatibilityCheckMetadata(ref metadataToSave, fixDateTaken: false);

                                MicrosoftLocationHack(ref metadataToSave, metadata, Properties.Settings.Default.MicosoftOneDriveLocationHackUse, Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix);
                                DataGridView_Populate_CompatibilityCheckedMetadataToSave(metadataToSave, fileEntryVersion);
                                AddQueueSaveUsingExiftoolMetadataUpdatedByUserLock(metadataToSave, new Metadata(metadataInCache));
                                //Need use metadataToSave.FullFilePath, Because When Exiftool output filename can be diffrent to input filename
                                AddQueueRenameMediaFilesLock(metadataToSave.FileFullPath, autoCorrect.RenameVariable); 
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

        #region AutoCorrectRunDataGridView
        private void AutoCorrectRunDataGridView(FileEntryVersion fileEntryVersion)
        {
            if (GlobalData.IsApplicationClosing) return;;

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

                            Metadata metadataToSave = autoCorrect.RunAlgorithmReturnCopy(metadataFromDataGridView,
                                databaseAndCacheMetadataExiftool,
                                databaseAndCacheMetadataMicrosoftPhotos,
                                databaseAndCacheMetadataWindowsLivePhotoGallery,
                                databaseAndCahceCameraOwner,
                                databaseLocationNameAndLookUp,
                                databaseGoogleLocationHistory,
                                locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                autoKeywordConvertions,
                                Properties.Settings.Default.RenameDateFormats);
                            
                            if (metadataToSave != null)
                            {
                                if (Properties.Settings.Default.WriteAutoKeywordsSynonyms) AutoKeywords(ref metadataToSave);
                                if (Properties.Settings.Default.WriteUsingCompatibilityCheck) AutoCorrect.CompatibilityCheckMetadata(ref metadataToSave, fixDateTaken: false);

                                //MicrosoftLocationHack(ref metadataToSave, metadataListOriginalExiftool[index], Properties.Settings.Default.MicosoftOneDriveLocationHackUse, Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix);
                                DataGridView_Populate_CompatibilityCheckedMetadataToSave(metadataToSave, fileEntryVersion);
                                //MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, isDirty);
                            }
                        }
                    }
                    //AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttributes);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            ThreadSaveUsingExiftoolToMedia();
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
                        AutoCorrectFormFolder(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        AutoCorrectFormMediaFiles(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxTags:
                        AutoCorrectFormDataGridView(FileEntryVersion.CompatibilityFixedAndAutoUpdated);
                        break;
                    case KryptonPages.kryptonPageToolboxPeople:
                        AutoCorrectFormDataGridView(FileEntryVersion.CompatibilityFixedAndAutoUpdated);
                        break;
                    case KryptonPages.kryptonPageToolboxMap:
                        AutoCorrectFormDataGridView(FileEntryVersion.CompatibilityFixedAndAutoUpdated);
                        break;
                    case KryptonPages.kryptonPageToolboxDates:
                        AutoCorrectFormDataGridView(FileEntryVersion.CompatibilityFixedAndAutoUpdated);
                        break;
                    case KryptonPages.kryptonPageToolboxExiftool:
                        AutoCorrectFormDataGridView(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxWarnings:
                        AutoCorrectFormDataGridView(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxProperties:
                        AutoCorrectFormDataGridView(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxRename:
                        AutoCorrectFormDataGridView(FileEntryVersion.MetadataToSave);
                        break;
                    case KryptonPages.kryptonPageToolboxConvertAndMerge:
                        AutoCorrectFormDataGridView(FileEntryVersion.MetadataToSave);
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("AutoCorrectForm")) return;
            if (IsPopulatingAnything("AutoCorrectForm")) return;
            switch (ActiveKryptonPage)
            {
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                case KryptonPages.kryptonPageMediaFiles:
                case KryptonPages.kryptonPageToolboxRename:
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
                    break;
            }

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionAutoCorrectForm();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void KryptonContextMenuItemGenericAutoCorrectForm_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("AutoCorrectForm")) return;
            if (IsPopulatingAnything("AutoCorrectForm")) return;
            switch (ActiveKryptonPage)
            {
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                case KryptonPages.kryptonPageMediaFiles:
                case KryptonPages.kryptonPageToolboxRename:
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
                    break;
            }

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionAutoCorrectForm();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region AutoCorrectFormMediaFiles
        private void AutoCorrectFormMediaFiles(FileEntryVersion fileEntryVersion)
        {
            if (GlobalData.IsApplicationClosing) return;
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

                                Metadata metadataToSave = autoCorrect.RunAlgorithmReturnCopy(metadata,
                                    databaseAndCacheMetadataExiftool,
                                    databaseAndCacheMetadataMicrosoftPhotos,
                                    databaseAndCacheMetadataWindowsLivePhotoGallery,
                                    databaseAndCahceCameraOwner,
                                    databaseLocationNameAndLookUp,
                                    databaseGoogleLocationHistory,
                                    locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                    autoKeywordConvertions,
                                    Properties.Settings.Default.RenameDateFormats);

                                if (metadataToSave != null)
                                {
                                    if (Properties.Settings.Default.WriteAutoKeywordsSynonyms) AutoKeywords(ref metadataToSave);
                                    if (Properties.Settings.Default.WriteUsingCompatibilityCheck) AutoCorrect.CompatibilityCheckMetadata(ref metadataToSave, fixDateTaken: false);

                                    MicrosoftLocationHack(ref metadataToSave, metadata, Properties.Settings.Default.MicosoftOneDriveLocationHackUse, Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix);
                                    DataGridView_Populate_CompatibilityCheckedMetadataToSave(metadataToSave, fileEntryVersion);
                                    AddQueueSaveUsingExiftoolMetadataUpdatedByUserLock(metadataToSave, new Metadata(metadataInCache));
                                    //Need use metadataToSave.FullFilePath, Because When Exiftool output filename can be diffrent to input filename
                                    AddQueueRenameMediaFilesLock(metadataToSave.FileFullPath, autoCorrect.RenameVariable);
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

        #region AutoCorrectFormFolder
        private void AutoCorrectFormFolder(FileEntryVersion fileEntryVersion)
        {
            if (GlobalData.IsApplicationClosing) return;
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
                        IEnumerable<FileData> fileDatas = ImageAndMovieFileExtentionsUtility.GetFilesByEnumerableFast(selectedFolder, false);
                        foreach (FileData fileData in fileDatas)
                        {
                            FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileData.Path, FileHandler.GetLastWriteTime(fileData.Path), MetadataBrokerType.ExifTool);
                            Metadata metadataInCache = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
                            if (metadataInCache != null)
                            {
                                Metadata metadata = new Metadata(metadataInCache);

                                Metadata metadataToSave = autoCorrect.RunAlgorithmReturnCopy(metadata,
                                databaseAndCacheMetadataExiftool,
                                databaseAndCacheMetadataMicrosoftPhotos,
                                databaseAndCacheMetadataWindowsLivePhotoGallery,
                                databaseAndCahceCameraOwner,
                                databaseLocationNameAndLookUp,
                                databaseGoogleLocationHistory,
                                locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                autoKeywordConvertions,
                                Properties.Settings.Default.RenameDateFormats);

                                if (metadataToSave != null)
                                {
                                    if (Properties.Settings.Default.WriteAutoKeywordsSynonyms) AutoKeywords(ref metadataToSave);
                                    AutoCorrectFormVaraibles.UseAutoCorrectFormData(ref metadataToSave, autoCorrectFormVaraibles);
                                    if (Properties.Settings.Default.WriteUsingCompatibilityCheck) AutoCorrect.CompatibilityCheckMetadata(ref metadataToSave, fixDateTaken: false);

                                    MicrosoftLocationHack(ref metadataToSave, metadata, Properties.Settings.Default.MicosoftOneDriveLocationHackUse, Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix);
                                    DataGridView_Populate_CompatibilityCheckedMetadataToSave(metadataToSave, fileEntryVersion);
                                    AddQueueSaveUsingExiftoolMetadataUpdatedByUserLock(metadataToSave, new Metadata(metadataInCache));
                                    //Need use metadataToSave.FullFilePath, Because When Exiftool output filename can be diffrent to input filename
                                    AddQueueRenameMediaFilesLock(metadata.FileFullPath, autoCorrect.RenameVariable);
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

        #region AutoCorrectFormDataGridView
        private void AutoCorrectFormDataGridView(FileEntryVersion fileEntryVersion)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (!HasDataGridViewAggregatedAny()) return;

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
                                
                                Metadata metadataToSave = autoCorrect.RunAlgorithmReturnCopy(metadataFromDataGridView,
                                    databaseAndCacheMetadataExiftool,
                                    databaseAndCacheMetadataMicrosoftPhotos,
                                    databaseAndCacheMetadataWindowsLivePhotoGallery,
                                    databaseAndCahceCameraOwner,
                                    databaseLocationNameAndLookUp,
                                    databaseGoogleLocationHistory,
                                    locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                                    autoKeywordConvertions,
                                    Properties.Settings.Default.RenameDateFormats);

                                if (metadataToSave != null)
                                {
                                    if (Properties.Settings.Default.WriteAutoKeywordsSynonyms) AutoKeywords(ref metadataToSave);
                                    if (Properties.Settings.Default.WriteUsingCompatibilityCheck) AutoCorrect.CompatibilityCheckMetadata(ref metadataToSave, fixDateTaken: false);
                                    //MicrosoftLocationHack(ref metadataToSave, metadataListOriginalExiftool[index], Properties.Settings.Default.MicosoftOneDriveLocationHackUse, Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix);
                                    DataGridView_Populate_CompatibilityCheckedMetadataToSave(metadataToSave, fileEntryVersion);
                                    //MakeEqualBetweenMetadataAndDataGridViewContent(metadataToSave, isDirty);
                                }
                            }
                        }
                        //AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(fileEntryAttributes);
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

        #region TouchFiles
        private void TouchFiles(HashSet<FileEntry> fileEntries)
        {
            #region Touch Offline files so they get downloaded
            bool dontReadFileFromCloud = Properties.Settings.Default.AvoidOfflineMediaFiles;
            if (!dontReadFileFromCloud)
            {
                foreach (FileEntry fileEntry in fileEntries)
                {
                    FileStatus fileStatus = FileHandler.GetFileStatus(
                        fileEntry.FileFullPath,
                        checkLockedStatus: true,
                        exiftoolProcessStatus: ExiftoolProcessStatus.InExiftoolReadQueue);

                    if (fileStatus.IsInCloudOrVirtualOrOffline)
                    {
                        fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitOfflineBecomeLocal;
                        FileHandler.TouchOfflineFileToGetFileOnline(fileEntry.FileFullPath);
                    }

                    ImageListView_UpdateItemThumbnailUpdateAllInvoke(fileEntry);
                    //ImageListView_UpdateItemFileStatusInvoke(fileEntry.FileFullPath, fileStatus);
                }
            }
            #endregion
        }
        #endregion

        #region Metadata - Refresh - Last (Files in Folder, ImageListView, NOT Grid)

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
                        FolderMetadataRefreshLast();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesMetadataRefreshLast();
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("MetadataRefreshLast")) return;
            if (IsPopulatingAnything("MetadataRefreshLast")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionMetadataRefreshLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemGenericMetadataRefreshLast_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("MetadataRefreshLast")) return;
            if (IsPopulatingAnything("MetadataRefreshLast")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionMetadataRefreshLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region DeleteLastMediadataAndReload
        void DeleteLastMetadataAndReloadThread(HashSet<FileEntry> fileEntries)
        {
            if (GlobalData.IsApplicationClosing) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                
                ClearAllQueues();

                TouchFiles(fileEntries);
                
                foreach (FileEntry fileEntry in fileEntries)
                {
                    UpdateStatusAction("Delete all data in database for file: " + fileEntry.FileFullPath);
                    filesCutCopyPasteDrag.DeleteFileEntry(fileEntry);                    
                    ImageListView_UpdateItemThumbnailUpdateAllInvoke(fileEntry);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            } 
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion 

        #region MediaFilesMetadataRefreshLast
        private void MediaFilesMetadataRefreshLast()
        {
            if (GlobalData.IsApplicationClosing) return;

            try
            {
                HashSet<FileEntry> fileEntries = ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true);
                Thread thread = new Thread(() =>
                {
                    DeleteLastMetadataAndReloadThread(fileEntries);
                });
                thread.Start();

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
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

        #region FolderMetadataRefreshLast
        private void FolderMetadataRefreshLast()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                HashSet<FileEntry> fileEntries = ImageListViewHandler.GetFileEntriesItems(imageListView1);
                Thread thread = new Thread(() =>
                {
                    DeleteLastMetadataAndReloadThread(fileEntries);
                });
                thread.Start();

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
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

        #region Metadata - Reload - DeleteHistory (Files in Folder, ImageListView, NOT Grid)

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
                        FolderMetadataReloadDeleteHistoryk();
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterSearch:
                        break;
                    case KryptonPages.kryptonPageFolderSearchFilterFilter:
                        break;
                    case KryptonPages.kryptonPageMediaFiles:
                        MediaFilesMetadataReloadDeleteHistoryk();
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("ReloadDeleteHistory")) return;
            if (IsPopulatingAnything("ReloadDeleteHistory")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionMetadataReloadDeleteHistory();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        private void KryptonContextMenuItemGenericMetadataReloadDeleteHistory_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("ReloadDeleteHistory")) return;
            if (IsPopulatingAnything("ReloadDeleteHistory")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionMetadataReloadDeleteHistory();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory
        public void ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(HashSet<FileEntry> fileEntries)
        {
            if (GlobalData.IsApplicationClosing) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;

                foreach (FileEntry fileEntry in fileEntries)
                {

                    UpdateStatusAction("Refreshing database for " + fileEntry.FileFullPath);
                    TouchFiles(fileEntries);
                    filesCutCopyPasteDrag.DeleteFileAndHistory(fileEntry.FileFullPath);
                    ImageListView_UpdateItemThumbnailUpdateAllInvoke(fileEntry);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }            
        }
        #endregion

        #region MediaFilesMetadataReloadDeleteHistory
        private void MediaFilesMetadataReloadDeleteHistoryk()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                HashSet<FileEntry> fileEntries = ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true);
                Thread thread = new Thread(() =>
                {
                    ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(fileEntries);
                });
                thread.Start();

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
                DisplayAllQueueStatus();
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
        private void FolderMetadataReloadDeleteHistoryThread(string[] files, string folder)
        {
            //Clean up ImageListView and other queues
            ClearAllQueues();

            UpdateStatusAction("Delete all record about files in database....");
            
            #region Touch Offline files so they get downloaded
            foreach (string fullFileName in files)
            {
                FileStatus fileStatus = FileHandler.GetFileStatus(
                fullFileName, checkLockedStatus: true,
                exiftoolProcessStatus: ExiftoolProcessStatus.ExiftoolWillNotProcessingFileInCloud);

                if (fileStatus.IsInCloudOrVirtualOrOffline)
                {
                    fileStatus.ExiftoolProcessStatus = ExiftoolProcessStatus.WaitOfflineBecomeLocal;
                    FileHandler.TouchOfflineFileToGetFileOnline(fullFileName);
                }

                ImageListView_UpdateItemFileStatusInvoke(fullFileName, fileStatus);
            }
            #endregion

            int recordAffected = filesCutCopyPasteDrag.DeleteDirectoryAndHistory(folder);

            UpdateStatusAction(recordAffected + " records was delete from database....");
        }
        private void FolderMetadataReloadDeleteHistoryk()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                string folder = GetSelectedNodeFullRealPath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't reload folder. Not a valid folder selected.", "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }

                string[] files = Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly);

                int filesCount = files.Length;

                int foldersCount = 0;
                if (filesCount == 0)
                {
                    string[] folders = Directory.GetDirectories(folder);
                    foldersCount = folders.Length;
                }

                if (filesCount + foldersCount > 0)
                {
                    if (KryptonMessageBox.Show(
                        "Are you sure you will delete **ALL** metadata history in database\r\n" +
                        "For files in folder: " + folder + "\r\n\r\n" +
                        (filesCount == 0 ? "" : "Files count: " + filesCount + "\r\n") +
                        (foldersCount == 0 ? "" : "Folders count: " + foldersCount + "\r\n"),
                        "You are going to delete all metadata in folder",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, showCtrlCopy: true) == DialogResult.Yes)
                    {
                        
                        Thread thread = new Thread(() =>
                        {
                            FolderMetadataReloadDeleteHistoryThread(files, folder);
                        });
                        thread.Start();
                        ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(false, true);
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("MetadataRefreshLast")) return;
            if (IsPopulatingAnything("MetadataRefreshLast")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImportLocations();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ImportLocations
        private void ImportLocations()
        {
            if (GlobalData.IsApplicationClosing) return;
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
                    ImageListView_SelectionChanged_Action_ImageListView_DataGridView(true);
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("MetadataRefreshLast")) return;
            if (IsPopulatingAnything("MetadataRefreshLast")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                Config();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion 

        #region Config
        private void Config()
        {
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                using (FormConfig config = new FormConfig(kryptonManager1, imageListView1))
                {
                    using (new WaitCursor())
                    {
                        exiftoolReader.MetadataReadPrioity.ReadOnlyOnce();
                        config.MetadataReadPrioity = exiftoolReader.MetadataReadPrioity;
                        config.ThumbnailSizes = thumbnailSizes;
                        config.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                        config.DatabaseAndCacheLocationAddress = databaseLocationNameAndLookUp;
                        config.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        config.Init();
                    }
                    if (config.ShowDialog() != DialogResult.Cancel)
                    {
                        //Thumbnail
                        ThumbnailSaveSize = Properties.Settings.Default.ApplicationThumbnail;
                        ThumbnailRegionHandler.FaceThumbnailSize = Properties.Settings.Default.ApplicationRegionThumbnail;
                        fileDateTimeReader = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);

                        databaseLocationNameAndLookUp.PreferredLanguagesString = Properties.Settings.Default.ApplicationPreferredLanguages;
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
                        
                        ImageListView_SelectionChanged_Action_ImageListView_DataGridView(true);

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
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                using (FormDatabaseCleaner formDatabaseCleaner = new FormDatabaseCleaner())
                {
                    using (new WaitCursor())
                    {
                        formDatabaseCleaner.databaseUtilitiesSqliteMetadata = databaseUtilitiesSqliteMetadata;
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("MetadataRefreshLast")) return;
            if (IsPopulatingAnything("MetadataRefreshLast")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                About();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion 

        #region About
        private void About()
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("LocationAnalytics")) return;
            if (IsPopulatingAnything("LocationAnalytics")) return;
            //if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ShowFormLocationHistoryAnalytics();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuItemToolLocationAnalytics_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("LocationAnalytics")) return;
            if (IsPopulatingAnything("LocationAnalytics")) return;
            //if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try 
            { 
                GlobalData.IsPerformingAButtonAction = true;
                ShowFormLocationHistoryAnalytics();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region Action_CheckAndFixOneDriveIssues_ReturnWasFoundAndRemoved
        private void Action_CheckAndFixOneDriveIssues_ReturnWasFoundAndRemoved()
        {
            HashSet<FileEntry> fileEntries = ImageListViewHandler.GetFileEntriesItems(imageListView1);
            List<string> deletedFiles = FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, out List<string> notFixed, oneDriveNetworkNames, fixError: true,
                moveToRecycleBin: Properties.Settings.Default.MoveToRecycleBin, databaseAndCacheMetadataExiftool);

            #region Remove delete files form ImageListView
            foreach (string fullFileName in deletedFiles)
            {
                ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fullFileName);
                if (foundItem != null) ImageListViewHandler.ImageListViewRemoveItem(imageListView1, foundItem);
            }
            #endregion 

            if (deletedFiles.Count > 0 || notFixed.Count > 0)
            {
                string filesNotFixed = "";
                for (int fileIndex = 0; fileIndex < Math.Min(notFixed.Count, 5); fileIndex++)
                {
                    filesNotFixed += (string.IsNullOrWhiteSpace(filesNotFixed) ? "" : "\r\n") + notFixed[fileIndex];
                }

                string filesDeleted = "";
                for (int fileIndex = 0; fileIndex < Math.Min(deletedFiles.Count, 5); fileIndex++)
                {
                    filesDeleted += (string.IsNullOrWhiteSpace(filesDeleted) ? "" : "\r\n") + deletedFiles[fileIndex];
                }

                if (
                    KryptonMessageBox.Show("Result after run OneDrive duplicated tool.\r\n\r\n" +

                    "Delete files: " + deletedFiles.Count + "\r\n" +
                    (deletedFiles.Count == 0 ? "\r\n" :
                    "Example files:\r\n" +
                    filesDeleted + "\r\n\r\n") +

                    "Files not fixed: " + notFixed.Count + "\r\n" +
                    (
                        notFixed.Count == 0 ? "\r\n" :
                        "Example files:\r\n" + filesNotFixed + "\r\n\r\n" +
                        "Select the files not fixed?\r\n" +
                        "Yes - Media files will be select in ImageListView\r\n" +
                        "No - No changes in selections"
                    ),
                    "Result running OneDrive duplicated tool.",
                    (notFixed.Count == 0 ? MessageBoxButtons.OK : MessageBoxButtons.YesNo),
                    MessageBoxIcon.Question, showCtrlCopy: true) == DialogResult.Yes)
                {
                    ImageListView_SelectFiles(notFixed);
                }
            }
            else
            {
                KryptonMessageBox.Show("Result after run OneDrive duplicated tool.\r\n\r\n" +

                "Delete files: 0 \r\n" +
                "Files not fixed: 0\r\n\r\n" +
                "No files found, was searching for <FileNames><-MachineName<-xx>>.ext\r\n",
                "Result running OneDrive duplicated tool.",
                MessageBoxButtons.OK);
            }
        }
        #endregion

        #region kryptonRibbonGroupButtonToolsRemoveOneDriveDuplicates_Click
        private void kryptonRibbonGroupButtonToolsRemoveOneDriveDuplicates_Click(object sender, EventArgs e)
        {
            Action_CheckAndFixOneDriveIssues_ReturnWasFoundAndRemoved();
        }
        #endregion 

        #region ActionSelectMediaFilesMatchCells
        private void ActionSelectMediaFilesMatchCells()
        {

            if (GlobalData.IsApplicationClosing) return;
            try
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
                ImageListViewHandler.SuspendLayout(imageListView1);

                using (new WaitCursor())
                {
                    ImageListViewSuspendLayoutInvoke(imageListView1);
                    imageListView1.ClearSelection();
                    HashSet<FileEntry> selectedFileEntries = DataGridView_GetSelectedFilesFromActive();
                    foreach (FileEntry fileEntry in selectedFileEntries)
                    {
                        ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView1.Items, fileEntry.FileFullPath);
                        if (foundItem != null) foundItem.Selected = true;
                    }
                    ImageListViewResumeLayoutInvoke(imageListView1);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                ImageListViewHandler.ResumeLayout(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
        }
        #endregion

        #region kryptonRibbonGroupButtonToolsReselectFilesMatchDataGridView_Click_1
        private void kryptonRibbonGroupButtonToolsReselectFilesMatchDataGridView_Click_1(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select Media Files Match Cells")) return;
            if (IsPopulatingAnything("Select Media Files Match Cells")) return;
            //if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectMediaFilesMatchCells();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        #region ActionSelectMediaFilesMatchCells
        private void SelectDataGridViewColumn(DataGridView dataGridView, HashSet<FileEntry> selectedFileEntries)
        {
            dataGridView.ClearSelection();
            for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericColumn != null && selectedFileEntries.Contains(dataGridViewGenericColumn.FileEntryAttribute.FileEntry))
                    DataGridViewHandler.SelectColumnRows(dataGridView, columnIndex, true);
                else
                    DataGridViewHandler.SelectColumnRows(dataGridView, columnIndex, false);
            }
        }

        private void SelectAndMatchDataGridViewRows(DataGridView dataGridView, HashSet<FileEntry> selectedFileEntries)
        {
            dataGridView.ClearSelection();
            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow?.FileEntryAttribute != null && selectedFileEntries.Contains(dataGridViewGenericRow?.FileEntryAttribute.FileEntry))
                    dataGridView.Rows[rowIndex].Selected = true;
                else
                    dataGridView.Rows[rowIndex].Selected = false;
            }
        }

        private void ActionSelectDataGridViewMatchCells()
        {

            if (GlobalData.IsApplicationClosing) return;
            try
            {
                using (new WaitCursor())
                {
                    HashSet<FileEntry> selectedFileEntries = DataGridView_GetSelectedFilesFromActive();
                    SelectAndMatchDataGridViewRows(dataGridViewConvertAndMerge, selectedFileEntries);
                    SelectDataGridViewColumn(dataGridViewDate, selectedFileEntries);
                    SelectDataGridViewColumn(dataGridViewExiftool, selectedFileEntries);
                    SelectDataGridViewColumn(dataGridViewExiftoolWarning, selectedFileEntries);
                    SelectDataGridViewColumn(dataGridViewMap, selectedFileEntries);
                    SelectDataGridViewColumn(dataGridViewPeople, selectedFileEntries);
                    SelectDataGridViewColumn(dataGridViewProperties, selectedFileEntries);
                    SelectAndMatchDataGridViewRows(dataGridViewRename, selectedFileEntries);
                    SelectDataGridViewColumn(dataGridViewTagsAndKeywords, selectedFileEntries);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region kryptonRibbonGroupButtonToolsReselectDataGridVIewMatchDataGridView_Click
        private void kryptonRibbonGroupButtonToolsReselectDataGridVIewMatchDataGridView_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select DataGridView Match Cells")) return;
            if (IsPopulatingAnything("Select DataGridView Match Cells")) return;
            //if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSelectDataGridViewMatchCells();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

        //----  ----
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
                AddQueueLazyLoadningAllVersionsAllSourcesMetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
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
                AddQueueLazyLoadningAllVersionsAllSourcesMetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
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
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set thumbnail size")) return;
            if (IsPopulatingAnything("Set thumbnail size")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                SetThumbnailSize(4);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonThumbnailSizeLarge_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set thumbnail size")) return;
            if (IsPopulatingAnything("Set thumbnail size")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                SetThumbnailSize(3);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonThumbnailSizeMedium_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set thumbnail size")) return;
            if (IsPopulatingAnything("Set thumbnail size")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                SetThumbnailSize(2);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonThumbnailSizeSmall_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set thumbnail size")) return;
            if (IsPopulatingAnything("Set thumbnail size")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                SetThumbnailSize(1);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void kryptonRibbonGroupButtonThumbnailSizeXSmall_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set thumbnail size")) return;
            if (IsPopulatingAnything("Set thumbnail size")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                SetThumbnailSize(0);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
                Logger.Error(ex);
                //KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
        private bool isSortingImageListView = false;
        private void ImageListViewSortByCheckedRadioButton(bool toogle)
        {
            try
            {
                if (isSortingImageListView) return;
                isSortingImageListView = true;
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
            } finally
            {
                isSortingImageListView = false;
            }
        }
        #endregion

        #region KryptonContextMenuRadioButtonFileSystemColumnSortXYZ_Click
        private void KryptonContextMenuItemFileSystemColumnSortClear_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                SetImageListViewSortByRadioButton(imageListView1, imageListView1.SortColumn, SortOrder.None);
                ImageListViewSortByCheckedRadioButton(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFilename_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFileDate_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaComments_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortMediaRating_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try { 
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationName_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationCity_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            { 
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void KryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Set Column Sort Order")) return;
            if (IsPopulatingAnything("Set Column Sort Order")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ImageListViewSortByCheckedRadioButton(true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion 

        #endregion

        //--
        

        #region SeeProcessQueue_Clcik
        private void kryptonRibbonGroupButtonToolsProgressLazyloadingShowStatus_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("See Process Queue")) return;
            if (IsPopulatingAnything("See Process Queue")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSeeProcessQueue();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void buttonSpecNavigatorDataGridViewProgressCircle_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("See Process Queue")) return;
            if (IsPopulatingAnything("See Process Queue")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSeeProcessQueue();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }

        private void buttonSpecNavigatorImageListViewLoadStatus_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("See Process Queue")) return;
            if (IsPopulatingAnything("See Process Queue")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                ActionSeeProcessQueue();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion

    }
}
