using DataGridViewGeneric;
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

        private void ValitedatePasteKeywords(DataGridView dataGridView, string header)
        {
            //int keywordsHeadingIndex = DataGridViewHandler.GetRowHeadingIndex(dataGridView, header);
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
                                DataGridViewHandler.SetCellValue(dataGridView, column, rowIndex, 
                                    DataGridViewHandler.GetRowName(dataGridView, rowIndex));
                            
                            DataGridViewHandler.SetCellStatus(dataGridView, column, rowIndex, 
                                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, 
                                DataGridViewHandler.IsCellNullOrWhiteSpace(dataGridView, column, rowIndex) ? SwitchStates.Off : SwitchStates.On, false));
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
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }
        #endregion

        #region TriState Click 
        private void dataGridViewTagsAndKeywords_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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
            if (updatedCells != null && updatedCells.Count > 0) ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
        }
        #endregion

        private bool cellEndEditInProcess = false;
        private void dataGridViewTagsAndKeywords_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (cellEndEditInProcess) return;
            cellEndEditInProcess = true;
            DataGridView dataGridView = ((DataGridView)sender);
            DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
            if (gridViewGenericDataRow != null)
            {
                string newValue = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, e.ColumnIndex, e.RowIndex);
                if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagTitle)
                {
                    ComboBoxHandler.AddAutoCompleteStringCollection(autoCompleteStringCollectionTitle, newValue);
                    Properties.Settings.Default.AutoCorrectFormTitle = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionTitle);
                }
                if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagDescription)
                {
                    ComboBoxHandler.AddAutoCompleteStringCollection(autoCompleteStringCollectionDescription, newValue);
                    Properties.Settings.Default.AutoCorrectFormDescription = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionDescription);
                }
                if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagComments)
                {
                    ComboBoxHandler.AddAutoCompleteStringCollection(autoCompleteStringCollectionComments, newValue);
                    Properties.Settings.Default.AutoCorrectFormComments = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionComments);
                }
                if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagAlbum)
                {
                    ComboBoxHandler.AddAutoCompleteStringCollection(autoCompleteStringCollectionAlbum, newValue);
                    Properties.Settings.Default.AutoCorrectFormAlbum = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionAlbum);
                }
                if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagAuthor)
                {
                    ComboBoxHandler.AddAutoCompleteStringCollection(autoCompleteStringCollectionAuthor, newValue);
                    Properties.Settings.Default.AutoCorrectFormAuthor = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionAuthor);
                }
            }
            cellEndEditInProcess = false;
        }

        #region CellValueChanged
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

            
            isDataGridViewTagsAndKeywords_CellValueChanging = false;
        }
        #endregion

        #region Populate DataGridView view when Text changed

        #region ActionChangeCommon
        private void ActionChangeCommon(DataGridView dataGridView, string header, string tag, string newText)
        {
            if (isFormLoading) return;
            if (GlobalData.IsApplicationClosing) return;
            if (isSettingDefaultComboxValues) return;
            if (GlobalData.IsPopulatingTags) return;

            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, header, tag);

            for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericDataColumn.Metadata != null && dataGridViewGenericDataColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                {
                    if (DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex) != comboBoxAlbum.Text) DataGridViewHandler.SetDataGridViewDirty(dataGridView, columnIndex);
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, newText);
                }
            }
        }
        #endregion 

        #region ChangeTitle
        private void ActionChangeTitle(string newText)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagTitle, newText);
        }

        private void comboBoxTitle_TextUpdate(object sender, EventArgs e)
        {
            ActionChangeTitle(((KryptonComboBox)sender).Text);
        }
        
        private void comboBoxTitle_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionChangeTitle((string)((KryptonComboBox)sender).SelectedItem);
        }
        #endregion

        #region ChangeDescription
        private void ActionChangeDescription(string newText)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagDescription, newText);
        }

        private void comboBoxDescription_TextUpdate(object sender, EventArgs e)
        {
            ActionChangeDescription(((KryptonComboBox)sender).Text);
        }

        private void comboBoxDescription_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionChangeDescription((string)((KryptonComboBox)sender).SelectedItem);
        }
        #endregion

        #region ChangeComments
        private void ActionChangeComments(string newText)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagComments, newText);
        }

        private void comboBoxComments_TextUpdate(object sender, EventArgs e)
        {
            ActionChangeComments(((KryptonComboBox)sender).Text);
        }
        private void comboBoxComments_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionChangeComments((string)((KryptonComboBox)sender).SelectedItem);
        }
        #endregion

        #region ChangeAlbum
        private void ActionChangeAlbum(string newText)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagAlbum, newText);
        }

        private void comboBoxAlbum_TextUpdate(object sender, EventArgs e)
        {
            ActionChangeAlbum(((KryptonComboBox)sender).Text);
        }

        private void comboBoxAlbum_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionChangeAlbum((string)((KryptonComboBox)sender).SelectedItem);
        }
        #endregion

        #region ChangeAuthor
        private void ActionAuthor(string newText)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            ActionChangeCommon(dataGridView, DataGridViewHandlerTagsAndKeywords.headerMedia, DataGridViewHandlerTagsAndKeywords.tagAuthor, newText);
        }
        private void comboBoxAuthor_TextUpdate(object sender, EventArgs e)
        { 
            ActionAuthor(((KryptonComboBox)sender).Text);
        }

        private void comboBoxAuthor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ActionAuthor((string)((KryptonComboBox)sender).SelectedItem);
        }
        #endregion

        #endregion

        #region Populate DataGridView view when Stars changed

        #region radioButtonRating_Common_CheckedChanged
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
        #endregion

        #region Rating1_CheckedChanged
        private void radioButtonRating1_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }
        #endregion

        #region Rating2_CheckedChanged
        private void radioButtonRating2_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }
        #endregion

        #region Rating3_CheckedChanged
        private void radioButtonRating3_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }
        #endregion

        #region Rating4_CheckedChanged
        private void radioButtonRating4_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonRating_Common_CheckedChanged();
        }
        #endregion

        #region Rating5_CheckedChanged
        private void radioButtonRating5_CheckedChanged(object sender, EventArgs e)
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

            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(GetSelectedFileEntriesImageListView());
        }
        #endregion

        #region ClearDetailViewTagsAndKeywords
        private void ClearDetailViewTagsAndKeywords()
        {
            isSettingDefaultComboxValues = true;

            EnableDetailViewTagsAndKeywords(false);

            comboBoxMediaAiConfidence.SelectedIndex = Properties.Settings.Default.MediaAiConfidence;
            
            comboBoxTitle.Items.Clear();
            comboBoxDescription.Items.Clear();
            comboBoxComments.Items.Clear();
            comboBoxAlbum.Items.Clear();

            autoCompleteStringCollectionAlbum = new AutoCompleteStringCollection();
            autoCompleteStringCollectionTitle = new AutoCompleteStringCollection();
            autoCompleteStringCollectionDescription = new AutoCompleteStringCollection();
            autoCompleteStringCollectionComments = new AutoCompleteStringCollection();
            autoCompleteStringCollectionAuthor = new AutoCompleteStringCollection();

            string[] arrayAlbum = ComboBoxHandler.ConvertToArray(Properties.Settings.Default.AutoCorrectFormAlbum);
            string[] arrayAuthor = ComboBoxHandler.ConvertToArray(Properties.Settings.Default.AutoCorrectFormAuthor);
            string[] arrayComments = ComboBoxHandler.ConvertToArray(Properties.Settings.Default.AutoCorrectFormComments);
            string[] arrayDescription = ComboBoxHandler.ConvertToArray(Properties.Settings.Default.AutoCorrectFormDescription);
            string[] arrayTitle = ComboBoxHandler.ConvertToArray(Properties.Settings.Default.AutoCorrectFormTitle);

            ComboBoxHandler.ComboBoxPopulate(comboBoxAlbum, arrayAlbum, "");
            autoCompleteStringCollectionAlbum.AddRange(arrayAlbum);

            ComboBoxHandler.ComboBoxPopulate(comboBoxAuthor, arrayAuthor, "");
            autoCompleteStringCollectionAuthor.AddRange(arrayAuthor);

            ComboBoxHandler.ComboBoxPopulate(comboBoxComments, arrayComments, "");
            autoCompleteStringCollectionComments.AddRange(arrayComments);

            ComboBoxHandler.ComboBoxPopulate(comboBoxDescription, arrayDescription, "");
            autoCompleteStringCollectionDescription.AddRange(arrayDescription);

            ComboBoxHandler.ComboBoxPopulate(comboBoxTitle, arrayTitle, "");
            autoCompleteStringCollectionTitle.AddRange(arrayTitle);

            //groupBoxRating
            radioButtonRating1.Checked = false;
            radioButtonRating2.Checked = false;
            radioButtonRating3.Checked = false;
            radioButtonRating4.Checked = false;
            radioButtonRating5.Checked = false;
            isSettingDefaultComboxValues = false;

        }
        #endregion

        #region EnableDetailViewTagsAndKeywords(bool enable)
        private void EnableDetailViewTagsAndKeywords(bool enable)
        {
            if (!enable)
            {
                comboBoxMediaAiConfidence.SuspendLayout();
                comboBoxTitle.SuspendLayout();
                comboBoxDescription.SuspendLayout();
                comboBoxComments.SuspendLayout();
                comboBoxAlbum.SuspendLayout();
                comboBoxAuthor.SuspendLayout();
                radioButtonRating1.SuspendLayout();
                radioButtonRating2.SuspendLayout();
                radioButtonRating3.SuspendLayout();
                radioButtonRating4.SuspendLayout();
                radioButtonRating5.SuspendLayout();
            }
            comboBoxMediaAiConfidence.Enabled = enable;

            if (enable) comboBoxTitle.Enabled = enable; //To avoid flashing
            comboBoxTitle.DropDownStyle = enable ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList; //To avoid white inputbox
            comboBoxTitle.Enabled = enable;//Also to avoid flashing, need set false after change DropDownStyle

            if (enable) comboBoxDescription.Enabled = enable;//To avoid flashing
            comboBoxDescription.DropDownStyle = enable ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList; //To avoid white inputbox
            comboBoxDescription.Enabled = enable;//Also to avoid flashing, need set false after change DropDownStyle

            if (enable) comboBoxComments.Enabled = enable;//To avoid flashing
            comboBoxComments.DropDownStyle = enable ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;//To avoid white inputbox
            comboBoxComments.Enabled = enable;//Also to avoid flashing, need set false after change DropDownStyle

            if (enable) comboBoxAlbum.Enabled = enable;//To avoid flashing
            comboBoxAlbum.DropDownStyle = enable ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;//To avoid white inputbox
            comboBoxAlbum.Enabled = enable;//Also to avoid flashing, need set false after change DropDownStyle

            if (enable) comboBoxAuthor.Enabled = enable;//To avoid flashing
            comboBoxAuthor.DropDownStyle = enable ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;//To avoid white inputbox
            comboBoxAuthor.Enabled = enable; //Also to avoid flashing, need set false after change DropDownStyle


            radioButtonRating1.Enabled = enable;
            radioButtonRating2.Enabled = enable;
            radioButtonRating3.Enabled = enable;
            radioButtonRating4.Enabled = enable;
            radioButtonRating5.Enabled = enable;

            if (enable)
            {
                comboBoxMediaAiConfidence.ResumeLayout();
                comboBoxTitle.ResumeLayout();
                comboBoxDescription.ResumeLayout();
                comboBoxComments.ResumeLayout();
                comboBoxAlbum.ResumeLayout();
                comboBoxAuthor.ResumeLayout();
                radioButtonRating1.ResumeLayout();
                radioButtonRating2.ResumeLayout();
                radioButtonRating3.ResumeLayout();
                radioButtonRating4.ResumeLayout();
                radioButtonRating5.ResumeLayout();
            } 
            
            
        }
        #endregion

        #region AddToListBox
        private void AddToListBox(DataGridView dataGridView, KryptonComboBox comboBox, int columnIndex, string sourceHeader, string rowTag)
        {
            int rowIndex;
            object value;
            rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, sourceHeader, rowTag);
            if (rowIndex > -1)
            {
                value = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex);
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()) && !comboBox.Items.Contains(value.ToString())) comboBox.Items.Add(value.ToString());
            }
        }
        #endregion

        #region AddToCollection
        private void AddToCollection(DataGridView dataGridView, AutoCompleteStringCollection autoCompleteStringCollection, int columnIndex, string sourceHeader, string rowTag)
        {
            int rowIndex;
            object value;
            rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, sourceHeader, rowTag);
            if (rowIndex > -1)
            {
                value = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex);
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()) && !autoCompleteStringCollection.Contains(value.ToString())) autoCompleteStringCollection.Add(value.ToString());
            }
        }
        #endregion

        private AutoCompleteStringCollection autoCompleteStringCollectionTitle = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection autoCompleteStringCollectionDescription = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection autoCompleteStringCollectionComments = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection autoCompleteStringCollectionAlbum = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection autoCompleteStringCollectionAuthor = new AutoCompleteStringCollection();

        #region PopulateDetailViewTagsAndKeywords(DataGridView dataGridView)
        private void PopulateDetailViewTagsAndKeywords(DataGridView dataGridView)
        {
            if (dataGridView == null) return;
            isSettingDefaultComboxValues = true;

            comboBoxTitle.Text = "";
            comboBoxDescription.Text = "";
            comboBoxComments.Text = "";
            comboBoxAlbum.Text = "";
            comboBoxAuthor.Text = "";

            for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
            {
                foreach (string sourceHeader in DataGridViewHandlerTagsAndKeywords.sourceHeaders)
                {

                    AddToListBox(dataGridView, comboBoxTitle, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagTitle);
                    AddToCollection(dataGridView, autoCompleteStringCollectionTitle, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagTitle);
                    
                    AddToListBox(dataGridView, comboBoxDescription, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagDescription);
                    AddToListBox(dataGridView, comboBoxDescription, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagAlbum);                    
                    AddToCollection(dataGridView, autoCompleteStringCollectionDescription, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagDescription);
                    AddToCollection(dataGridView, autoCompleteStringCollectionDescription, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagAlbum);

                    AddToListBox(dataGridView, comboBoxComments, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagComments);
                    AddToCollection(dataGridView, autoCompleteStringCollectionComments, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagComments);

                    AddToListBox(dataGridView, comboBoxAlbum, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagAlbum);
                    AddToListBox(dataGridView, comboBoxAlbum, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagDescription);
                    AddToCollection(dataGridView, autoCompleteStringCollectionAlbum, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagAlbum);
                    AddToCollection(dataGridView, autoCompleteStringCollectionAlbum, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagDescription);

                    AddToListBox(dataGridView, comboBoxAuthor, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagAuthor);
                    AddToCollection(dataGridView, autoCompleteStringCollectionAuthor, columnIndex, sourceHeader, DataGridViewHandlerTagsAndKeywords.tagTitle);
                }
            }

            EnableDetailViewTagsAndKeywords(true);
            isSettingDefaultComboxValues = false;

        }
        #endregion 

        #region Control with focus (For Cut/Copy/Paste)

        private Control controlPasteWithFocusTag = null;
        private void comboBoxAlbum_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;
        }

        private void comboBoxAlbum_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
        }

        private void comboBoxTitle_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;
        }

        private void comboBoxTitle_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
        }

        private void comboBoxDescription_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;
        }

        private void comboBoxDescription_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
        }

        private void comboBoxComments_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;
        }

        private void comboBoxComments_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
        }

        private void comboBoxAuthor_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;
        }

        private void comboBoxAuthor_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
        }

        private void dataGridViewTagsAndKeywords_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = (Control)sender;
        }

        private void dataGridViewTagsAndKeywords_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusTag = null;
        }
        #endregion

        #region EditingControlShowing
        private void dataGridViewTagsAndKeywords_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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
        #endregion
    }
}
