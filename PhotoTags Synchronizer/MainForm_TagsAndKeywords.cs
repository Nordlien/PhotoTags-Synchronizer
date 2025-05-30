﻿using DataGridViewGeneric;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region Validation of cells and Rows (keyword tag added/changed validation)
        private void dataGridViewTagsAndKeywords_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);

                if (e.RowIndex < 0) return;
                if (GlobalData.IsApplicationClosing) return;
                if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;
                if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;

                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

                int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, header);
                int lastRowEdit = DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView);

                if (e.RowIndex >= keywordsStarts)
                {
                    //Update the row that become edit
                    DataGridViewHandler.SetCellStatusDefaultWhenRowAdded(dataGridView, lastRowEdit - 1,
                        new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, false));
                    DataGridViewHandler.SetCellBackGroundColorForRow(dataGridView, lastRowEdit - 1);

                    //Updated the new empty row added
                    DataGridViewHandler.SetCellStatusDefaultWhenRowAdded(dataGridView, lastRowEdit,
                                        new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, false));
                    DataGridViewHandler.SetCellBackGroundColorForRow(dataGridView, lastRowEdit);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private string ValidateAndReturnTag(DataGridView dataGridView, string header, string testThisTag)
        {
            string validTag = testThisTag;
            bool foundTagBefore;
            int number = 0;

            int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, header);
            int keywordsEnds = DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header);

            do
            {
                foundTagBefore = false;
                for (int rowIndex = keywordsStarts; rowIndex <= keywordsEnds; rowIndex++) // < Don't check new added line
                {
                    if (DataGridViewHandler.GetRowName(dataGridView, rowIndex) == (string)validTag) //Whithout ToString it was Object Compare and never become "true"
                    {
                        foundTagBefore = true;
                        number++;
                        validTag = testThisTag + number.ToString();
                        break;
                    }
                }
            } while (foundTagBefore);

            return validTag;
        }

        private void ValitedatePasteKeywords(DataGridView dataGridView, string header)
        {
            
            int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, header);

            int rowIndex = keywordsStarts;
            while (rowIndex < DataGridViewHandler.GetRowCount(dataGridView) - 1) 
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                
                //If is Keywords rows
                if (dataGridViewGenericRow == null || DataGridViewHandler.GetRowHeader(dataGridView, rowIndex).Equals(header))
                {

                    for (int column = 0; column < dataGridView.Columns.Count; column++)
                    {
                        if (dataGridViewGenericRow == null)
                        {
                            if (!DataGridViewHandler.IsCellNullOrWhiteSpace(dataGridView, column, rowIndex))
                            {
                                //Set a valid header tag to using current cell as base
                                
                                DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, header,
                                    ValidateAndReturnTag(dataGridView, header, DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, column, rowIndex)),
                                    ReadWriteAccess.AllowCellReadAndWrite);
                                dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                            }
                        }

                        if ( (!DataGridViewHandler.IsCellDataGridViewGenericCellStatus(dataGridView, column, rowIndex)) ||
                            (DataGridViewHandler.GetCellStatusSwichStatus(dataGridView, column, rowIndex) == SwitchStates.Undefine))
                        {
                            if (!DataGridViewHandler.IsCellNullOrWhiteSpace(dataGridView, column, rowIndex)) 
                                DataGridViewHandler.SetCellValue(dataGridView, column, rowIndex, DataGridViewHandler.GetRowName(dataGridView, rowIndex), true);
                            
                            DataGridViewHandler.SetCellStatus(dataGridView, column, rowIndex, 
                                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, 
                                DataGridViewHandler.IsCellNullOrWhiteSpace(dataGridView, column, rowIndex) ? SwitchStates.Off : SwitchStates.On, false), true);
                        } 
                    }

                    if (dataGridViewGenericRow == null)
                    {
                        DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, header, ValidateAndReturnTag(dataGridView, header, "(empty)"), ReadWriteAccess.AllowCellReadAndWrite);
                    }
                }

                rowIndex++;
            }
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region CellEnter
        private void dataGridViewTagsAndKeywords_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region TriState Click 
        private void dataGridViewTagsAndKeywords_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (e.RowIndex == -1) RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);

                Rectangle cellRectangle = dataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                if (e.X >= cellRectangle.Width - tristateButtonWidth && e.Y <= tristateBittonHight) triStateButtomClick = true;
                else triStateButtomClick = false;
                if (!triStateButtomClick) return;

                if (!dataGridView.Enabled) return;

                if (dataGridView.SelectedCells.Count < 1) return;

                DataGridViewSelectedCellCollection dataGridViewSelectedCellCollection = dataGridView.SelectedCells;
                if (dataGridViewSelectedCellCollection.Count < 1) return;

                Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = DataGridViewHandler.ToggleCells(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Toggle, e.ColumnIndex, e.RowIndex);
                DataGridViewHandler.Refresh(dataGridView);
                if (updatedCells != null && updatedCells.Count > 0)
                {
                    ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
                    DataGridView_UpdatedDirtyFlags(dataGridView);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region CellValueChanged
        private bool isDataGridViewTagsAndKeywords_CellValueChanging = false;
        private void dataGridViewTagsAndKeywords_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isDataGridViewTagsAndKeywords_CellValueChanging) return; //Avoid recursive isues
            if (GlobalData.IsApplicationClosing) return;
            if (ClipboardUtility.IsClipboardActive) return;
            if (GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress) return;
            if (e.ColumnIndex < 0) return;
            if (e.RowIndex < 0) return;

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;
            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;
            //if (IsPopulatingAnything("Tags and Keywords Cell value changed")) return;


            isDataGridViewTagsAndKeywords_CellValueChanging = true;
            try
            {
                DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);

                if (gridViewGenericDataRow == null || //new row 
                    gridViewGenericDataRow.HeaderName.Equals(DataGridViewHandlerTagsAndKeywords.headerKeywords)) //Check if one of Keywords row(s)
                {
                    if (DataGridViewHandler.IsCellNullOrWhiteSpace(dataGridView, e.ColumnIndex, e.RowIndex))
                    {
                        DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, e.ColumnIndex, e.RowIndex, SwitchStates.Off);
                    }
                    else
                    {
                        string newTag = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, e.ColumnIndex, e.RowIndex);
                        DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, e.ColumnIndex, e.RowIndex, SwitchStates.On);

                        if (DataGridViewHandler.GetRowName(dataGridView, e.RowIndex) == newTag)
                        {
                            isDataGridViewTagsAndKeywords_CellValueChanging = false;
                            return;
                        }

                        newTag = ValidateAndReturnTag(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, newTag);

                        DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, e.RowIndex,
                            new DataGridViewGenericRow(DataGridViewHandlerTagsAndKeywords.headerKeywords, newTag, ReadWriteAccess.AllowCellReadAndWrite));
                        //DataGridViewHandler.SetCellReadOnlyWhenForcedForRow(dataGridView, e.RowIndex);
                        DataGridViewHandler.SetRowFavoriteFlag(dataGridView, e.RowIndex);
                        DataGridViewHandler.SetCellBackGroundColorForRow(dataGridView, e.RowIndex);

                        //Update other tags in row with "newTag" if changed
                        for (int column = 0; column < dataGridViewTagsAndKeywords.Columns.Count; column++)
                        {
                            if ((!DataGridViewHandler.IsCellNullOrWhiteSpace(dataGridView, column, e.RowIndex)) &&
                                DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, column, e.RowIndex) != newTag)
                            {
                                DataGridViewHandler.SetCellValue(dataGridView, column, e.RowIndex, DataGridViewHandler.GetRowName(dataGridView, e.RowIndex), true);
                                DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, column, e.RowIndex, SwitchStates.On);
                            }
                        }
                    }

                    DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                isDataGridViewTagsAndKeywords_CellValueChanging = false;
            }
        }
        #endregion

        #region Populate DataGridView view when Text changed

        #region ActionChangeCommonPushToStack
        private void ActionChangeCommonPushToStack(DataGridView dataGridView, string header, string tag)
        {
            if (isFormLoading) return;
            if (GlobalData.IsApplicationClosing) return;
            if (isSettingDefaultComboxValues) return;
            if (GlobalData.IsPopulatingTags) return;

            try
            {
                int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, header, tag);
                Dictionary<CellLocation, DataGridViewGenericCell> pushToStack = new Dictionary<CellLocation, DataGridViewGenericCell>();
                for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
                {
                    DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    if (dataGridViewGenericDataColumn.Metadata != null && dataGridViewGenericDataColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                    {
                        CellLocation cellLocation = new CellLocation(columnIndex, rowIndex);
                        DataGridViewGenericCell dataGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, columnIndex, rowIndex);
                        pushToStack.Add(cellLocation, dataGridViewGenericCell);
                    }
                }
                if (pushToStack != null && pushToStack.Count > 0) ClipboardUtility.PushToUndoStack(dataGridView, pushToStack);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region ActionChangeCommon
        private void ActionChangeCommon(DataGridView dataGridView, string header, string tag, string newText)
        {
            if (isFormLoading) return;
            if (GlobalData.IsApplicationClosing) return;
            if (isSettingDefaultComboxValues) return;
            if (GlobalData.IsPopulatingTags) return;

            try
            {
                int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, header, tag);

                //Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

                for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
                {
                    CellLocation cellLocation = new CellLocation(columnIndex, rowIndex);
                    DataGridViewGenericCell dataGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, columnIndex, rowIndex);
                    //updatedCells.Add(cellLocation, dataGridViewGenericCell);

                    DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    if (dataGridViewGenericDataColumn.Metadata != null && dataGridViewGenericDataColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                    {
                        DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, newText, true);
                    }
                }
                
                DataGridView_UpdatedDirtyFlags(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region ChangeTitle
        private void ActionChangeTitle(string newText)
        {
            try {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagTitle, newText);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        private void ActionUpdateTitle()
        {
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxTitle);
            ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionTitle, comboBoxTitle.Text);
        }

        private void comboBoxTitle_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommonPushToStack(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagTitle);
        }

        private void comboBoxTitle_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
            ActionUpdateTitle();

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }

        private void comboBoxTitle_TextUpdate(object sender, EventArgs e)
        {

            ActionChangeTitle(((KryptonComboBox)sender).Text);
        }
        
        private void comboBoxTitle_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionChangeTitle((string)((KryptonComboBox)sender).SelectedItem);
            ActionUpdateTitle();
        }
        #endregion

        #region ChangeDescription
        private void ActionChangeDescription(string newText)
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagDescription, newText);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void ActionUpdateDescription()
        {
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxDescription);
            ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionDescription, comboBoxDescription.Text);
        }

        private void comboBoxDescription_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommonPushToStack(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagDescription);
        }

        private void comboBoxDescription_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
            ActionUpdateDescription();

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        private void comboBoxDescription_TextUpdate(object sender, EventArgs e)
        {
            ActionChangeDescription(((KryptonComboBox)sender).Text);
        }

        private void comboBoxDescription_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionChangeDescription((string)((KryptonComboBox)sender).SelectedItem);
            ActionUpdateDescription();
        }
        #endregion

        #region ChangeComments
        private void ActionChangeComments(string newText)
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagComments, newText);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void ActionUpdateComments()
        {
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxComments);
            ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionComments, comboBoxComments.Text);
        }

        private void comboBoxComments_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommonPushToStack(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagComments);
        }

        private void comboBoxComments_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
            ActionUpdateComments();

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }

        private void comboBoxComments_TextUpdate(object sender, EventArgs e)
        {
            ActionChangeComments(((KryptonComboBox)sender).Text);
        }
        private void comboBoxComments_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionChangeComments((string)((KryptonComboBox)sender).SelectedItem);
            ActionUpdateComments();
        }
        #endregion

        #region ChangeAlbum
        private void ActionChangeAlbum(string newText)
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagAlbum, newText);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void ActionUpdateAlbum()
        {
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxAlbum);
            ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionAlbum, comboBoxAlbum.Text);
        }

        private void comboBoxAlbum_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommonPushToStack(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagAlbum);
        }

        private void comboBoxAlbum_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
            ActionUpdateAlbum();

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }

        private void comboBoxAlbum_TextUpdate(object sender, EventArgs e)
        {
            ActionChangeAlbum(((KryptonComboBox)sender).Text);
        }

        private void comboBoxAlbum_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionChangeAlbum((string)((KryptonComboBox)sender).SelectedItem);
            ActionUpdateAlbum();
        }
        #endregion

        #region ChangeAuthor
        private void ActionChangeAuthor(string newText)
        {
            try
            {
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagAuthor, newText);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void ActionUpdateAuthor()
        {
            ComboBoxHandler.ComboBoxAddLastTextFirstInList(comboBoxAuthor);
            ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionAuthor, comboBoxAuthor.Text);
        }

        private void comboBoxAuthor_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommonPushToStack(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagAuthor);
        }

        private void comboBoxAuthor_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
            ActionUpdateAuthor();

            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }

        private void comboBoxAuthor_TextUpdate(object sender, EventArgs e)
        { 
            ActionChangeAuthor(((KryptonComboBox)sender).Text);
        }

        private void comboBoxAuthor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionChangeAuthor((string)((KryptonComboBox)sender).SelectedItem);
            ActionUpdateAuthor();
        }
        #endregion

        #endregion

        #region Control with focus (For Cut/Copy/Paste)
        private Control controlPasteWithFocusTag = null;

        private void dataGridViewTagsAndKeywords_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;
        }

        private void dataGridViewTagsAndKeywords_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
        }
        #endregion

        #region Rating

        #region RadioButtonRating_Common
        private void radioButtonRating_Common_CheckedChanged()
        {
            try 
            { 
                DataGridView dataGridView = dataGridViewTagsAndKeywords;
                ActionChangeCommonPushToStack(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagRating);

                byte? rating = null;
                if (radioButtonRating1.Checked) rating = 1;
                if (radioButtonRating2.Checked) rating = 2;
                if (radioButtonRating3.Checked) rating = 3;
                if (radioButtonRating4.Checked) rating = 4;
                if (radioButtonRating5.Checked) rating = 5;
            
                int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView,
                    DataGridViewHandlerTagsAndKeywords.headerMedia,
                    DataGridViewHandlerTagsAndKeywords.tagRating);

                for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
                {
                    DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    if (dataGridViewGenericDataColumn.Metadata != null)
                    {
                        DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, rating, true);
                    }
                }

                ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
                DataGridView_UpdatedDirtyFlags(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region Rating1_Click
        private void radioButtonRating1_Click(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }
        #endregion

        #region Rating2_Click
        private void radioButtonRating2_Click(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }
        #endregion

        #region Rating3_Click
        private void radioButtonRating3_Click(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }
        #endregion

        #region Rating4_Click
        private void radioButtonRating4_Click(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }

        #endregion

        #region Rating5_Click
        private void radioButtonRating5_Click(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }
        #endregion

        #endregion

        #region GetAiConfidence
        private double GetAiConfidence()
        {
            return (90 - comboBoxMediaAiConfidence.SelectedIndex * 10) / 100f;
        }

        private void comboBoxMediaAiConfidence_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (GlobalData.IsPopulatingTagsFile) return;
            Properties.Settings.Default.MediaAiConfidence = comboBoxMediaAiConfidence.SelectedIndex;
            DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();
            try
            {
                AddQueueLazyLoadning_AllSources_AllVersions_MetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region InitDetailViewTagsAndKeywords
        private void InitDetailViewTagsAndKeywords()
        {
            isSettingDefaultComboxValues = true;
            try
            {
                comboBoxMediaAiConfidence.SelectedIndex = Properties.Settings.Default.MediaAiConfidence;

                comboBoxAlbum.AutoCompleteMode = AutoCompleteMode.None;
                comboBoxAlbum.AutoCompleteSource = AutoCompleteSource.None;
                comboBoxTitle.AutoCompleteMode = AutoCompleteMode.None;
                comboBoxTitle.AutoCompleteSource = AutoCompleteSource.None;
                comboBoxDescription.AutoCompleteMode = AutoCompleteMode.None;
                comboBoxDescription.AutoCompleteSource = AutoCompleteSource.None;
                comboBoxComments.AutoCompleteMode = AutoCompleteMode.None;
                comboBoxComments.AutoCompleteSource = AutoCompleteSource.None;
                comboBoxAuthor.AutoCompleteMode = AutoCompleteMode.None;
                comboBoxAuthor.AutoCompleteSource = AutoCompleteSource.None;

                comboBoxAlbum.Items.Clear();
                comboBoxTitle.Items.Clear();
                comboBoxDescription.Items.Clear();
                comboBoxComments.Items.Clear();
                comboBoxAuthor.Items.Clear();

                lock (autoCompleteStringCollectionAlbumLock) autoCompleteStringCollectionAlbum = new AutoCompleteStringCollection();
                lock (autoCompleteStringCollectionTitleLock) autoCompleteStringCollectionTitle = new AutoCompleteStringCollection();
                lock (autoCompleteStringCollectionDescriptionLock) autoCompleteStringCollectionDescription = new AutoCompleteStringCollection();
                lock (autoCompleteStringCollectionCommentsLock) autoCompleteStringCollectionComments = new AutoCompleteStringCollection();
                lock (autoCompleteStringCollectionAuthorLock) autoCompleteStringCollectionAuthor = new AutoCompleteStringCollection();

                string[] arrayAlbum = ComboBoxHandler.ConvertStringToArray(Properties.Settings.Default.AutoCorrectFormAlbum);
                string[] arrayAuthor = ComboBoxHandler.ConvertStringToArray(Properties.Settings.Default.AutoCorrectFormAuthor);
                string[] arrayComments = ComboBoxHandler.ConvertStringToArray(Properties.Settings.Default.AutoCorrectFormComments);
                string[] arrayDescription = ComboBoxHandler.ConvertStringToArray(Properties.Settings.Default.AutoCorrectFormDescription);
                string[] arrayTitle = ComboBoxHandler.ConvertStringToArray(Properties.Settings.Default.AutoCorrectFormTitle);


                lock (autoCompleteStringCollectionAlbumLock)
                {
                    ComboBoxHandler.ComboBoxPopulateAppend(comboBoxAlbum, arrayAlbum, "");
                    ComboBoxHandler.AutoCompleteStringCollectionAppend(autoCompleteStringCollectionAlbum, arrayAlbum);
                }

                lock (autoCompleteStringCollectionTitleLock)
                {
                    ComboBoxHandler.ComboBoxPopulateAppend(comboBoxAuthor, arrayAuthor, "");
                    ComboBoxHandler.AutoCompleteStringCollectionAppend(autoCompleteStringCollectionAuthor, arrayAuthor);
                }

                lock (autoCompleteStringCollectionCommentsLock)
                {
                    ComboBoxHandler.ComboBoxPopulateAppend(comboBoxComments, arrayComments, "");
                    ComboBoxHandler.AutoCompleteStringCollectionAppend(autoCompleteStringCollectionComments, arrayComments);
                }

                lock (autoCompleteStringCollectionDescriptionLock)
                {
                    ComboBoxHandler.ComboBoxPopulateAppend(comboBoxDescription, arrayDescription, "");
                    ComboBoxHandler.AutoCompleteStringCollectionAppend(autoCompleteStringCollectionDescription, arrayDescription);
                }

                lock (autoCompleteStringCollectionAuthorLock)
                {
                    ComboBoxHandler.ComboBoxPopulateAppend(comboBoxTitle, arrayTitle, "");
                    ComboBoxHandler.AutoCompleteStringCollectionAppend(autoCompleteStringCollectionTitle, arrayTitle);
                }

                //groupBoxRating
                radioButtonRating1.Checked = false;
                radioButtonRating2.Checked = false;
                radioButtonRating3.Checked = false;
                radioButtonRating4.Checked = false;
                radioButtonRating5.Checked = false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                isSettingDefaultComboxValues = false;
            }
        }
        #endregion

        #region AddCellValueToKryptonComboBox
        private void AppendCellValueToKryptonComboBox(KryptonComboBox kryptonComboBox, object value)
        {
            if (value is string) ComboBoxHandler.ComboBoxPopulateAppend(kryptonComboBox, (string)value);            
        }
        #endregion

        #region AddCellValueToAutoCompleteStringCollection
        private void AppendCellValueToAutoCompleteStringCollection(AutoCompleteStringCollection autoCompleteStringCollection, object value)
        {
            if (value is string) ComboBoxHandler.AutoCompleteStringCollectionAppend(autoCompleteStringCollection, (string)value);
        }
        #endregion

        private AutoCompleteStringCollection autoCompleteStringCollectionTitle = new AutoCompleteStringCollection();
        private readonly object autoCompleteStringCollectionTitleLock = new object();
        private AutoCompleteStringCollection autoCompleteStringCollectionDescription = new AutoCompleteStringCollection();
        private readonly object autoCompleteStringCollectionDescriptionLock = new object();
        private AutoCompleteStringCollection autoCompleteStringCollectionComments = new AutoCompleteStringCollection();
        private readonly object autoCompleteStringCollectionCommentsLock = new object();
        private AutoCompleteStringCollection autoCompleteStringCollectionAlbum = new AutoCompleteStringCollection();
        private readonly object autoCompleteStringCollectionAlbumLock = new object();
        private AutoCompleteStringCollection autoCompleteStringCollectionAuthor = new AutoCompleteStringCollection();
        private readonly object autoCompleteStringCollectionAuthorLock = new object();

        #region PopulateDetailViewTagsAndKeywords(DataGridView dataGridView)
        private void PopulateDetailViewTagsAndKeywords(DataGridView dataGridView)
        {
            if (dataGridView == null) return;
            isSettingDefaultComboxValues = true;
            try
            {
                if (!string.IsNullOrWhiteSpace(comboBoxTitle.Text)) comboBoxTitle.Text = "";
                if (!string.IsNullOrWhiteSpace(comboBoxDescription.Text)) comboBoxDescription.Text = "";
                if (!string.IsNullOrWhiteSpace(comboBoxComments.Text)) comboBoxComments.Text = "";
                if (!string.IsNullOrWhiteSpace(comboBoxAlbum.Text)) comboBoxAlbum.Text = "";
                if (!string.IsNullOrWhiteSpace(comboBoxAuthor.Text)) comboBoxAuthor.Text = "";

                foreach (string sourceHeader in DataGridViewHandlerTagsAndKeywords.sourceHeaders)
                {
                    int rowIndexTitle = DataGridViewHandler.GetRowIndex(dataGridView, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagTitle);
                    int rowIndexDescription = DataGridViewHandler.GetRowIndex(dataGridView, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagDescription);
                    int rowIndexAlbum = DataGridViewHandler.GetRowIndex(dataGridView, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagAlbum);
                    int rowIndexComments = DataGridViewHandler.GetRowIndex(dataGridView, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagComments);
                    int rowIndexAuthor = DataGridViewHandler.GetRowIndex(dataGridView, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagAuthor);
                    
                    for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
                    {
                        object valueTitle = (rowIndexTitle == -1 ? null : DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndexTitle));
                        object valueDescription = (rowIndexDescription == -1 ? null : DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndexDescription));
                        object valueAlbum = (rowIndexAlbum == -1 ? null : DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndexAlbum));
                        object valueComments = (rowIndexComments == -1 ? null : DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndexComments));
                        object valueAuthor = (rowIndexAuthor == -1 ? null : DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndexAuthor));

                        try
                        {
                            lock (autoCompleteStringCollectionTitleLock)
                            {
                                AppendCellValueToKryptonComboBox(comboBoxTitle, valueTitle);
                                AppendCellValueToAutoCompleteStringCollection(autoCompleteStringCollectionTitle, valueTitle);
                            }

                            lock (autoCompleteStringCollectionDescriptionLock)
                            {
                                
                                AppendCellValueToKryptonComboBox(comboBoxDescription, valueDescription);
                                AppendCellValueToAutoCompleteStringCollection(autoCompleteStringCollectionDescription, valueDescription);

                                
                                AppendCellValueToKryptonComboBox(comboBoxDescription, valueAlbum);
                                AppendCellValueToAutoCompleteStringCollection(autoCompleteStringCollectionDescription, valueAlbum);
                            }

                            lock (autoCompleteStringCollectionCommentsLock)
                            {
                                AppendCellValueToKryptonComboBox(comboBoxComments, valueComments);
                                AppendCellValueToAutoCompleteStringCollection(autoCompleteStringCollectionComments, valueComments);
                            }

                            lock (autoCompleteStringCollectionAlbumLock)
                            {
                                AppendCellValueToAutoCompleteStringCollection(autoCompleteStringCollectionAlbum, valueAlbum);
                                AppendCellValueToAutoCompleteStringCollection(autoCompleteStringCollectionAlbum, valueDescription);
                            }

                            lock (autoCompleteStringCollectionAuthorLock)
                            {
                                AppendCellValueToKryptonComboBox(comboBoxAuthor, valueAuthor);
                                AppendCellValueToAutoCompleteStringCollection(autoCompleteStringCollectionAuthor, valueAuthor);
                            }
                        } catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                isSettingDefaultComboxValues = false;
            }
        }
        #endregion 

        #region EditingControlShowing
        private void dataGridViewTagsAndKeywords_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = (DataGridView)sender;


                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, dataGridView.CurrentCell.RowIndex);

                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                {
                    TextBox autoText = e.Control as TextBox;
                    if (autoText != null)
                    {
                        if (dataGridViewGenericRow.RowName == DataGridViewHandlerTagsAndKeywords.tagTitle)
                        {
                            autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                            autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            autoText.AutoCompleteCustomSource = autoCompleteStringCollectionTitle;
                        }
                        if (dataGridViewGenericRow.RowName == DataGridViewHandlerTagsAndKeywords.tagDescription)
                        {
                            autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                            autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            autoText.AutoCompleteCustomSource = autoCompleteStringCollectionDescription;
                        }
                        if (dataGridViewGenericRow.RowName == DataGridViewHandlerTagsAndKeywords.tagComments)
                        {
                            autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                            autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            autoText.AutoCompleteCustomSource = autoCompleteStringCollectionComments;
                        }
                        if (dataGridViewGenericRow.RowName == DataGridViewHandlerTagsAndKeywords.tagAlbum)
                        {
                            autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                            autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            autoText.AutoCompleteCustomSource = autoCompleteStringCollectionAlbum;
                        }
                        if (dataGridViewGenericRow.RowName == DataGridViewHandlerTagsAndKeywords.tagAuthor)
                        {
                            autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                            autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            autoText.AutoCompleteCustomSource = autoCompleteStringCollectionAuthor;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion
    }
}
