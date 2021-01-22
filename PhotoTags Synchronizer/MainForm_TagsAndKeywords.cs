using DataGridViewGeneric;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : Form
    {
        #region Validation of cells and Rows (keyword tag added/changed validation)
        private void dataGridViewTagsAndKeywords_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
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
                    if (DataGridViewHandler.GetRowName(dataGridView, rowIndex) == validTag.ToString()) //Whithout ToString it was Object Compare and never become "true"
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

        private void ValitedatePaste(DataGridView dataGridView, string header)
        {
            //int keywordsHeadingIndex = DataGridViewHandler.GetRowHeadingIndex(dataGridView, header);
            int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, header);

            int rowIndex = keywordsStarts;
            while (rowIndex < DataGridViewHandler.GetRowCount(dataGridView) - 1) 
            {
                //If is Keywords rows
                if ( !DataGridViewHandler.IsRowDataGridViewGenericRow(dataGridView, rowIndex) || DataGridViewHandler.GetRowHeader(dataGridView, rowIndex).Equals(header))
                {
                    for (int column = 0; column < dataGridView.Columns.Count; column++)
                    {
                        if (!DataGridViewHandler.IsRowDataGridViewGenericRow(dataGridView, rowIndex))
                        {
                            if (!DataGridViewHandler.IsCellNullOrWhiteSpace(dataGridView, column, rowIndex))
                            {
                                //Set a valid header tag to using current cell as base
                                DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, header,
                                    ValidateAndReturnTag(dataGridView, header, DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, column, rowIndex)),
                                    ReadWriteAccess.AllowCellReadAndWrite);
                            }
                        }

                        if ( (!DataGridViewHandler.IsCellDataGridViewGenericCellStatus(dataGridView, column, rowIndex)) ||
                            (DataGridViewHandler.GetCellStatusSwichStatus(dataGridView, column, rowIndex) == SwitchStates.Undefine))
                        {
                            if (!DataGridViewHandler.IsCellNullOrWhiteSpace(dataGridView, column, rowIndex)) 
                                DataGridViewHandler.SetCellValue(dataGridView, column, rowIndex, 
                                    DataGridViewHandler.GetRowName(dataGridView, rowIndex));
                            
                            DataGridViewHandler.SetCellStatus(dataGridView, column, rowIndex, 
                                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, 
                                DataGridViewHandler.IsCellNullOrWhiteSpace(dataGridView, column, rowIndex) ? SwitchStates.Off : SwitchStates.On, false));
                        } 
                        
                    }

                    if (!DataGridViewHandler.IsRowDataGridViewGenericRow(dataGridView, rowIndex))
                    {
                        DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, header, ValidateAndReturnTag(dataGridView, header, "(empty)"), ReadWriteAccess.AllowCellReadAndWrite);
                    }
                }

                rowIndex++;
            }
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion 

        #region TriState Click / Begin Edit / End Edit
        private void dataGridViewTagsAndKeywords_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);

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
            if (updatedCells != null && updatedCells.Count > 0) ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
        }


        private bool isDataGridViewTagsAndKeywords_CellValueChanging = false;
        private void dataGridViewTagsAndKeywords_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isDataGridViewTagsAndKeywords_CellValueChanging) return; //Avoid recursive isues
            if (ClipboardUtility.IsClipboardActive) return;                    
            if (GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress) return;

            if (GlobalData.IsApplicationClosing) return;
            if (GlobalData.IsPopulatingTags || GlobalData.IsPopulatingTagsFile) return;
            if (e.ColumnIndex == -1) return; //Row added, this is not a cell value changed
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            isDataGridViewTagsAndKeywords_CellValueChanging = true;
            
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
                            DataGridViewHandler.SetCellValue(dataGridView, column, e.RowIndex, DataGridViewHandler.GetRowName(dataGridView, e.RowIndex));
                            DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, column, e.RowIndex, SwitchStates.On);
                        }
                    }
                }
            }
            //else DataGridViewHandler.Refresh(dataGridView); Hack to refreash dropdown combobox with new values
            isDataGridViewTagsAndKeywords_CellValueChanging = false;
        }
        #endregion

        #region Popelate DataGridView view when Text changed
        private void comboBoxTitle_TextChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;

            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView,
                DataGridViewHandlerTagsAndKeywords.headerMedia,
                DataGridViewHandlerTagsAndKeywords.tagTitle);

            for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericDataColumn.Metadata != null && dataGridViewGenericDataColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                {
                    if (DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex) != comboBoxTitle.Text) DataGridViewHandler.SetDataGridViewDirty(dataGridView, columnIndex);
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, comboBoxTitle.Text);
                    
                }
            }
        }

        private void comboBoxDescription_TextChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;

            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView,
                DataGridViewHandlerTagsAndKeywords.headerMedia,
                DataGridViewHandlerTagsAndKeywords.tagDescription);

            for (int columnIndex = 0; columnIndex < dataGridViewTagsAndKeywords.Columns.Count; columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericDataColumn.Metadata != null && dataGridViewGenericDataColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                {
                    if (DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex) != comboBoxDescription.Text) DataGridViewHandler.SetDataGridViewDirty(dataGridView, columnIndex);
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, comboBoxDescription.Text);
                }
            }
        }

        private void comboBoxComments_TextChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;

            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView,
                DataGridViewHandlerTagsAndKeywords.headerMedia,
                DataGridViewHandlerTagsAndKeywords.tagComments);

            for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericDataColumn.Metadata != null && dataGridViewGenericDataColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                {
                    if (DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex) != comboBoxComments.Text) DataGridViewHandler.SetDataGridViewDirty(dataGridView, columnIndex);
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, comboBoxComments.Text);
                }
            }
        }

        private void comboBoxAlbum_TextChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;

            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView,
                DataGridViewHandlerTagsAndKeywords.headerMedia,
                DataGridViewHandlerTagsAndKeywords.tagAlbum);

            for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericDataColumn.Metadata != null && dataGridViewGenericDataColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                {
                    if (DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex) != comboBoxAlbum.Text) DataGridViewHandler.SetDataGridViewDirty(dataGridView, columnIndex);
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, comboBoxAlbum.Text);
                }
            }
        }

        private void comboBoxAuthor_TextChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;

            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, 
                DataGridViewHandlerTagsAndKeywords.headerMedia,
                DataGridViewHandlerTagsAndKeywords.tagAuthor);

            for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericDataColumn.Metadata != null && dataGridViewGenericDataColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                {
                    if (DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex) != comboBoxAuthor.Text) DataGridViewHandler.SetDataGridViewDirty(dataGridView, columnIndex);

                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, comboBoxAuthor.Text);
                }
            }
        }

        private void radioButtonRating_Common_CheckedChanged()
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
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
                    if (DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex) != (rating == null ? "" :rating.ToString()) ) DataGridViewHandler.SetDataGridViewDirty(dataGridView, columnIndex);
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, rating);
                }
            }
        }

        private void radioButtonRating1_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }

        private void radioButtonRating2_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }

        private void radioButtonRating3_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }

        private void radioButtonRating4_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }

        private void radioButtonRating5_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }

        #endregion

        #region GetAiConfidence
        private double GetAiConfidence()
        {
            return (90 - comboBoxMediaAiConfidence.SelectedIndex * 10) / 100f;
        }

        private void comboBoxMediaAiConfidence_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (GlobalData.IsPopulatingTagsFile) return;
            Properties.Settings.Default.MediaAiConfidence = comboBoxMediaAiConfidence.SelectedIndex;
            DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();

            PopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListView1.SelectedItems);
        }
        #endregion

        #region PupulateDetalView
        private void ClearDetailViewTagsAndKeywords()
        {
            comboBoxMediaAiConfidence.SelectedIndex = Properties.Settings.Default.MediaAiConfidence;
            comboBoxTitle.Items.Clear();
            comboBoxDescription.Items.Clear();
            comboBoxComments.Items.Clear();
            comboBoxAlbum.Items.Clear();

            //groupBoxRating
            radioButtonRating1.Checked = false;
            radioButtonRating2.Checked = false;
            radioButtonRating3.Checked = false;
            radioButtonRating4.Checked = false;
            radioButtonRating5.Checked = false;
        }

        private void AddToListBox(DataGridView dataGridView, ComboBox comboBox, int columnIndex, string sourceHeader, string rowTag)
        {
            int rowIndex;
            object value;
            rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, sourceHeader, rowTag);
            if (rowIndex > -1)
            {
                value = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex);
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()) && !comboBox.Items.Contains(value.ToString()))
                    comboBox.Items.Add(value.ToString());
            }
        }

        private void PopulateDetailViewTagsAndKeywords(DataGridView dataGridView)
        {
            if (dataGridView == null) return;

            for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
            {
                foreach (string sourceHeader in DataGridViewHandlerTagsAndKeywords.sourceHeaders)
                {

                    AddToListBox(dataGridView, comboBoxTitle, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagTitle);
                    AddToListBox(dataGridView, comboBoxTitle, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagDescription);

                    AddToListBox(dataGridView, comboBoxDescription, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagDescription);
                    AddToListBox(dataGridView, comboBoxDescription, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagTitle);

                    AddToListBox(dataGridView, comboBoxComments, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagComments);
                    AddToListBox(dataGridView, comboBoxAlbum, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagAlbum);
                    AddToListBox(dataGridView, comboBoxAuthor, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagAuthor);
                }
            }
        }


        #endregion

    }
}
