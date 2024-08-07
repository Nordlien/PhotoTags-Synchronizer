﻿using Krypton.Toolkit;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DataGridViewGeneric
{
    public class ClipboardUtility
    {
        #region Clear
        public static void Clear(DataGridView dataGridView)
        {
            if (dataGridView.TopLeftHeaderCell.Tag == null)
                dataGridView.TopLeftHeaderCell.Tag = new DataGridViewGenericData();
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return;

            DataGridViewGenericData dataGridViewGenericData = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
            dataGridViewGenericData.RedoCellsStack.Clear();
            dataGridViewGenericData.UndoCellsStack.Clear();
        }
        #endregion

        #region PushToUndoStack
        public static void PushSelectedCellsToUndoStack(DataGridView dataGridView)
        {
            if (IsDoingUndoRedo) return;
            if (dataGridView.TopLeftHeaderCell.Tag == null)
                dataGridView.TopLeftHeaderCell.Tag = new DataGridViewGenericData();

            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return;

            PushToUndoStack(dataGridView, GetSelectedCells(dataGridView));
        }
        #endregion

        //DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        public static void PushNotEqualToUndoStack(DataGridView dataGridView, Dictionary<CellLocation, DataGridViewGenericCell> cells)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> peek = ClipboardUtility.PeekUndoStack(dataGridView);

            //foreach (CellLocation cellLocation in peek.Keys)
            //{
            //    DataGridViewGenericCell dataGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, cellLocation.ColumnIndex, cellLocation.RowIndex);
            //    if (dataGridViewGenericCell.Value is RegionStructure regionStructureForPeek)
            //    {
            //        if (!string.IsNullOrWhiteSpace(regionStructureForPeek.Name)) PeopleAddNewLastUseName(regionStructureForPeek.Name);
            //    }
            //    else
            //    {
            //        //DEBUG
            //    }
            //}
        }

        #region PushToUndoStack
        public static void PushToUndoStack(DataGridView dataGridView, Dictionary<CellLocation, DataGridViewGenericCell> cells)
        {
            if (IsDoingUndoRedo) return;
            if (dataGridView.TopLeftHeaderCell.Tag == null) dataGridView.TopLeftHeaderCell.Tag = new DataGridViewGenericData();
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return;
            
            DataGridViewGenericData undoAndRedo = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
            Dictionary<CellLocation, DataGridViewGenericCell> peekCells = undoAndRedo.UndoCellsStack.Count == 0 ? null : undoAndRedo.UndoCellsStack.Peek();
            undoAndRedo.UndoCellsStack.Push(cells);
            
        }
        #endregion

        #region PushToUndoStack
        public static Dictionary<CellLocation, DataGridViewGenericCell> PeekUndoStack(DataGridView dataGridView)
        {
            if (IsDoingUndoRedo) return null;
            if (dataGridView.TopLeftHeaderCell.Tag == null) dataGridView.TopLeftHeaderCell.Tag = new DataGridViewGenericData();
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return null;

            DataGridViewGenericData undoAndRedo = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
            return undoAndRedo.UndoCellsStack.Count == 0 ? null : undoAndRedo.UndoCellsStack.Peek();
        }
        #endregion

        #region PushToUndoStack
        public static bool CancelPushUndoStackIfNoChanges(DataGridView dataGridView)
        {
            if (IsDoingUndoRedo) return true;
            if (dataGridView.TopLeftHeaderCell.Tag == null) dataGridView.TopLeftHeaderCell.Tag = new DataGridViewGenericData();
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return true;

            DataGridViewGenericData undoAndRedo = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
            Dictionary<CellLocation, DataGridViewGenericCell> peekCells = undoAndRedo.UndoCellsStack.Count == 0 ? null : undoAndRedo.UndoCellsStack.Peek();

            bool isEqual = true;
            if (peekCells == null) isEqual = false;

            if (isEqual)
            {
                foreach (CellLocation cellLocation in peekCells.Keys)
                {
                    DataGridViewGenericCell dataGridViewGenericCell = 
                        DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, cellLocation.ColumnIndex, cellLocation.RowIndex);

                    if (peekCells[cellLocation] != dataGridViewGenericCell)
                    {
                        isEqual = false;
                        break;
                    }
                }
            }
            if (isEqual) undoAndRedo.UndoCellsStack.Pop();

            return isEqual;
        }
        #endregion

        #region PushToRedoStack
        private static void PushToRedoStack(DataGridView dataGridView, Dictionary<CellLocation, DataGridViewGenericCell> cells)
        {
            if (IsDoingUndoRedo) return;
            if (dataGridView.TopLeftHeaderCell.Tag == null) dataGridView.TopLeftHeaderCell.Tag = new DataGridViewGenericData();
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return;

            DataGridViewGenericData undoAndRedo = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
            undoAndRedo.RedoCellsStack.Push(cells);
        }
        #endregion

        #region GetCellsFromStack
        private static Dictionary<CellLocation, DataGridViewGenericCell> GetCellsFromStack(DataGridView dataGridView, Dictionary<CellLocation, DataGridViewGenericCell> undoCells)
        {            
            Dictionary<CellLocation, DataGridViewGenericCell> redoCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            foreach (CellLocation cellLocation in undoCells.Keys)
            {
                CellLocation cellPosition = new CellLocation(cellLocation.ColumnIndex, cellLocation.RowIndex);
                redoCells.Add(cellPosition, DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, cellPosition.ColumnIndex, cellPosition.RowIndex));
            }

            return redoCells;
        }
        #endregion

        #region GetSelectedCells
        private static Dictionary<CellLocation, DataGridViewGenericCell> GetSelectedCells(DataGridView dataGridView)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> selectedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                CellLocation cellPosition = new CellLocation(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                selectedCells.Add(cellPosition, DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, cellPosition.ColumnIndex, cellPosition.RowIndex));
            }

            return selectedCells;
        }
        #endregion

        public static bool IsDoingUndoRedo { get; set; }  = false;
        public static bool IsClipboardActive { get; set; } = false;
        public static int NuberOfItemsToEdit { get; set; } = 0;

        #region UndoDataGridView
        public static void UndoDataGridView(DataGridView dataGridView)
        {
            if (IsDoingUndoRedo) return;
            if (dataGridView.CurrentCell == null) return;

            CellLocation currentCell = DataGridViewHandler.GetCurrentCellLocation(dataGridView.CurrentCell);
            
            if (dataGridView.TopLeftHeaderCell.Tag == null) return;
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return;

            DataGridViewGenericData undoAndRedo = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
            
            if (undoAndRedo.UndoCellsStack.Count > 0)
            {
                Dictionary<CellLocation, DataGridViewGenericCell> undoCells = undoAndRedo.UndoCellsStack.Pop(); 
                
                Dictionary<CellLocation, DataGridViewGenericCell> redoCells = GetCellsFromStack(dataGridView, undoCells);
                PushToRedoStack(dataGridView, redoCells);

                IsDoingUndoRedo = true; //needs to be after PushToRedoStack or it will return

                NuberOfItemsToEdit = undoCells.Count;
                IsClipboardActive = true;
                foreach (var cell in undoCells)
                {
                    DataGridViewHandler.SetCellDataGridViewGenericCell(dataGridView, cell.Key.ColumnIndex, cell.Key.RowIndex, cell.Value, true);
                }
                NuberOfItemsToEdit = 0;
                IsClipboardActive = false;
            }

            DataGridViewHandler.SetCurrentCellLocation(dataGridView, currentCell);

            IsDoingUndoRedo = false;
        }
        #endregion

        #region RedoDataGridView
        public static void RedoDataGridView(DataGridView dataGridView)
        {
            if (IsDoingUndoRedo) return;
            if (dataGridView.CurrentCell == null) return;

            CellLocation currentCell = DataGridViewHandler.GetCurrentCellLocation(dataGridView.CurrentCell);           

            if (dataGridView.TopLeftHeaderCell.Tag == null) return;
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return;
            DataGridViewGenericData undoAndRedo = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;

            
            if (undoAndRedo.RedoCellsStack.Count > 0)
            {
                Dictionary<CellLocation, DataGridViewGenericCell> redoCells = undoAndRedo.RedoCellsStack.Pop();
                
                Dictionary<CellLocation, DataGridViewGenericCell> undoCells = GetCellsFromStack(dataGridView, redoCells);
                PushToUndoStack(dataGridView, undoCells);

                IsDoingUndoRedo = true; //needs to be after PushToUndoStack or it will return
                NuberOfItemsToEdit = redoCells.Count;
                IsClipboardActive = true;
                foreach (var cell in redoCells)
                {
                    //dataGridView.CurrentCell = dataGridView[cell.Key.Column, cell.Key.Row];
                    DataGridViewHandler.SetCellDataGridViewGenericCell(dataGridView, cell.Key.ColumnIndex, cell.Key.RowIndex, cell.Value, true);
                }
                NuberOfItemsToEdit = 0;
                IsClipboardActive = false;
            }

            DataGridViewHandler.SetCurrentCellLocation(dataGridView, currentCell);
            IsDoingUndoRedo = false;
        }
        #endregion

        #region IsCurrentCellTextBoxAndInEditMode
        private static bool IsCurrentCellTextBoxAndInEditMode(DataGridView dataGridView)
        {
            try
            {
                DataGridViewCell cell = dataGridView.CurrentCell;
                if (cell != null)
                {
                    if (cell is DataGridViewTextBoxCell || cell is DataGridViewComboBoxCell)
                    {
                        if (cell.IsInEditMode) return true;
                    }
                }
            }
            catch { }
            return false;
        }
        #endregion

        #region TextBoxGetSelction
        public static void TextBoxGetSelction(TextBox textBox, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength)
        {

            if (textBox != null)
            {
                textBoxSelectionStart = textBox.SelectionStart;
                textBoxSelectionLength = textBox.SelectionLength;
                textBoxSelectionCanRestore = true;
            } else
            {
                textBoxSelectionStart = 0;
                textBoxSelectionLength = 0;
                textBoxSelectionCanRestore = false;
            }
        }
        #endregion

        #region DataGridViewRestoreEditMode
        public static void DataGridViewRestoreEditMode(DataGridView dataGridView, bool textBoxSelectionCanRestore, int textBoxSelectionStart, int textBoxSelectionLength)
        {
            if (textBoxSelectionCanRestore)
            {
                if (!IsCurrentCellTextBoxAndInEditMode(dataGridView)) dataGridView.BeginEdit(true);
                TextBox textBox = dataGridView.EditingControl as TextBox;

                if (textBox != null)
                {
                    textBox.SelectionStart = textBoxSelectionStart;
                    textBox.SelectionLength = textBoxSelectionLength;
                }
            }
        }
        #endregion

        #region CopyDataGridViewSelectedCellsToClipboard
        public static void CopyDataGridViewSelectedCellsToClipboard(DataGridView dataGridView, bool doCut, 
            out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength)
        {
            textBoxSelectionStart = 0;
            textBoxSelectionLength = 0;
            textBoxSelectionCanRestore = false;

            try
            {
                if (IsCurrentCellTextBoxAndInEditMode(dataGridView))
                {
                    TextBox textBox = dataGridView.EditingControl as TextBox;
                    if (textBox != null)
                    {
                        if (doCut) textBox.Cut(); else textBox.Copy();
                        TextBoxGetSelction(textBox, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
                        textBoxSelectionCanRestore = true;
                    }                   
                }                
                else
                {
                    dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                    DataGridView dataGridViewCopy = new DataGridView();
                    DataGridViewHandler.DataGridViewInit(dataGridViewCopy, allowUserToAddRows: false);
                    dataGridViewCopy.TopLeftHeaderCell.Value = dataGridView.TopLeftHeaderCell.Value;

                    #region Add Columns
                    List<int> selectedColumns = DataGridViewHandler.GetColumnSelected(dataGridView);
                    selectedColumns.Sort();
                    foreach (int columnIndex in selectedColumns)
                    {
                        string columnName = DataGridViewHandler.GetColumnDataGridViewName(dataGridView, columnIndex);
                        dataGridViewCopy.Columns.Add(columnName, columnName);
                    }
                    #endregion

                    List<int> selectedRows = DataGridViewHandler.GetRowSelected(dataGridView);
                    selectedRows.Sort();
                    foreach (int rowIndex in selectedRows)
                    {
                        string rowName = DataGridViewHandler.GetRowValue(dataGridView, rowIndex);
                        int destinationRow = dataGridViewCopy.Rows.Add();
                        dataGridViewCopy.Rows[destinationRow].HeaderCell.Value = rowName;

                        int destinationColumn = 0;
                        foreach (int columnIndex in selectedColumns)
                        {
                            var cellValue = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex);
                            if (cellValue is RegionStructure)
                                DataGridViewHandler.SetCellValue(dataGridViewCopy, destinationColumn, destinationRow, ((RegionStructure)cellValue).Name, true);
                            else
                                DataGridViewHandler.SetCellValue(dataGridViewCopy, destinationColumn, destinationRow, cellValue, true);
                            destinationColumn++;
                        }
                    }

                    dataGridViewCopy.SelectAll();
                    dataGridViewCopy.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                    DataObject dataObj = dataGridViewCopy.GetClipboardContent();
                    if (dataObj != null) Clipboard.SetDataObject(dataObj, true);
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Can't copy cells to clipboard. Reason:\r\n\r\n" + ex.Message, "Warning!", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DeleteDataGridViewSelectedCells
        public static void DeleteDataGridViewSelectedCells(DataGridView dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength)
        {
            DeleteDataGridViewSelectedCells(dataGridView, -1, -1, -1, -1, true, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
        }
        #endregion

        #region DeleteDataGridViewSelectedCells
        public static void DeleteDataGridViewSelectedCells(DataGridView dataGridView, int leftColumnOverwrite, int rightColumnOverwrite, int topRowOverwrite, int buttomRowOverwrite, bool removeTag,
            out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength)
        {
            textBoxSelectionStart = 0;
            textBoxSelectionLength = 0;
            textBoxSelectionCanRestore = false;

            DataGridViewSelectedCellCollection dataGridViewSelectedCellCollection = dataGridView.SelectedCells;
            if (dataGridViewSelectedCellCollection.Count < 1) return;
            if (IsCurrentCellTextBoxAndInEditMode(dataGridView))
            {
                TextBox textBox = dataGridView.EditingControl as TextBox;
                if (textBox != null)
                {
                    TextBoxGetSelction(textBox, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
                    textBox.Text =  textBox.Text.Substring(0, textBox.SelectionStart) + textBox.Text.Substring(textBox.SelectionStart + textBox.SelectedText.Length);
                    textBox.SelectionStart = textBoxSelectionStart;
                    textBoxSelectionLength = textBox.SelectionLength = 0;
                    textBoxSelectionCanRestore = true;
                }
                return;
            }
            PushSelectedCellsToUndoStack(dataGridView);

            NuberOfItemsToEdit = dataGridViewSelectedCellCollection.Count;
            IsClipboardActive = false;
            foreach (DataGridViewCell dataGridViewCell in dataGridViewSelectedCellCollection)
            {
                if (!dataGridViewCell.ReadOnly ||
                    (dataGridViewCell.ColumnIndex >= leftColumnOverwrite && dataGridViewCell.ColumnIndex <= rightColumnOverwrite &&
                    dataGridViewCell.RowIndex >= topRowOverwrite && dataGridViewCell.RowIndex <= buttomRowOverwrite)
                    )
                {
                    DataGridViewHandler.SetCellValue(dataGridView, dataGridViewCell, dataGridViewCell.DefaultNewRowValue);
                    if (removeTag) DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex, SwitchStates.Undefine);
                }
            }
            NuberOfItemsToEdit = 0;

            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
            IsClipboardActive = false;
        }
        #endregion

        #region PasteDataGridViewSelectedCellsFromClipboard
        public static void PasteDataGridViewSelectedCellsFromClipboard(DataGridView dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength)
        {
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, -1, -1, -1, -1, true,
                    out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);            
        }
        #endregion

        #region PasteDataGridViewSelectedCellsFromClipboard      
        public static void PasteDataGridViewSelectedCellsFromClipboard(
            DataGridView dataGridView, int leftColumnOverwrite, int rightColumnOverwrite, int topRowOverwrite, int buttomRowOverwrite, bool removeTag,
            out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength)
        {
            textBoxSelectionStart = 0;
            textBoxSelectionLength = 0;
            textBoxSelectionCanRestore = false;

            #region Html Format
            // Try to process as html format (data from excel) since it keeps the row information intact, instead of assuming
            // a new row for every new line if we just process it as text
            String HtmlFormat = Clipboard.GetData("HTML Format") as String;
            List<List<string>> rowContents = new List<List<string>>();
            if (HtmlFormat != null)
            {
                try
                {
                    // Remove html tags to just extract row information and store it in rowContents
                    System.Text.RegularExpressions.Regex TRregex = new System.Text.RegularExpressions.Regex(@"<( )*tr([^>])*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.Regex TDregex = new System.Text.RegularExpressions.Regex(@"<( )*td([^>])*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.Match trMatch = TRregex.Match(HtmlFormat);

                    while (!string.IsNullOrWhiteSpace(trMatch.Value))
                    {
                        int rowStart = trMatch.Index + trMatch.Length;
                        int rowEnd = HtmlFormat.IndexOf("</tr>", rowStart, StringComparison.InvariantCultureIgnoreCase);
                        System.Text.RegularExpressions.Match tdMatch = TDregex.Match(HtmlFormat, rowStart, rowEnd - rowStart);
                        List<string> rowContent = new List<string>();
                        while (!string.IsNullOrWhiteSpace(tdMatch.Value))
                        {
                            int cellStart = tdMatch.Index + tdMatch.Length;
                            int cellEnd = HtmlFormat.IndexOf("</td>", cellStart, StringComparison.InvariantCultureIgnoreCase);
                            string cellContent = HtmlFormat.Substring(cellStart, cellEnd - cellStart);
                            cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*br( )*>", "\r\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*li( )*>", "\r\n - ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*div([^>])*>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*code([^>])*>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*/code([^>])*>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            cellContent = System.Text.RegularExpressions.Regex.Replace(cellContent, @"<( )*p([^>])*>", "\r\n\r\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            if (!cellContent.StartsWith("<B>")) //Don't paste Row Header
                            {
                                //cellContent = cellContent.Replace("&nbsp;", " ");
                                cellContent = System.Net.WebUtility.HtmlDecode(cellContent);
                                rowContent.Add(cellContent);
                            }
                            tdMatch = tdMatch.NextMatch();
                        }
                        if (rowContent.Count > 0)
                        {
                            rowContents.Add(rowContent);
                        }
                        trMatch = trMatch.NextMatch();
                    }
                } catch
                {
                    rowContents.Clear();
                }
            }
            #endregion

            #region Text format
            if (rowContents.Count == 0)
            {
                // Clipboard is not in html format, read as text
                String CopiedText = Clipboard.GetText();
                String[] lines = CopiedText.Split('\n');
                foreach (string line in lines)
                {
                    List<string> rowContent = new List<string>(line.Split('\t'));
                    if (rowContent.Count > 0)
                    {
                        rowContents.Add(rowContent);
                    }
                }
            }
            #endregion

            #region Paste into Edit cell 
            if (IsCurrentCellTextBoxAndInEditMode(dataGridView))
            {
                if (rowContents.Count >= 1)
                {
                    string clipboardText = "";
                    foreach (List<string> textsInLine in rowContents)
                    {
                        string clipboardLine = "";
                        foreach (string text in textsInLine) clipboardLine = clipboardLine + (string.IsNullOrWhiteSpace(clipboardLine) ? "" : " ") + text;                        
                        clipboardText = clipboardText + (string.IsNullOrWhiteSpace(clipboardText) ? "" : "\r\n") + clipboardLine;
                    }

                    Clipboard.SetText(clipboardText);
                }
                TextBox textBox = dataGridView.EditingControl as TextBox;
                if (textBox != null)
                {
                    textBox.Paste();
                    TextBoxGetSelction(textBox, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
                    textBoxSelectionCanRestore = true;
                }
                return; //Can return - don't need push to stach, Edit cell did push to stacj
            }
            #endregion 

            // -----------------------------------------------------------------------------
            // Put the feach data to cells
            // -----------------------------------------------------------------------------

            Stack<CellLocation> selectedCells = new Stack<CellLocation>();

            #region Find all - SelectedCells - and - topRow and leftColumn -
            int topRow = int.MaxValue;
            int leftColumn = int.MaxValue;
            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                topRow = Math.Min(topRow, dataGridViewCell.RowIndex);
                leftColumn = Math.Min(leftColumn, dataGridViewCell.ColumnIndex);

                selectedCells.Push(new CellLocation(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex));
            }
            int iRow = topRow;
            #endregion

            #region Add Rows if needed
            if (dataGridView.AllowUserToAddRows)
            {
                // DataGridView's rowCount has one extra row (the temporary new row)
                if (iRow + rowContents.Count > dataGridView.Rows.Count - 1)
                {
                    int iNumNewRows = iRow + rowContents.Count - dataGridView.Rows.Count + 1;
                    // Simply add to a new row to the datagridview if it is not binded to a datasource
                    if (dataGridView.DataSource == null)
                    {
                        //dataGridView.Rows.Add(iNumNewRows); //This will change the selected cell
                        for (int i = 0; i < iNumNewRows; i++)
                        {
                            dataGridView.Rows.Add();
                        }
                        dataGridView.ClearSelection(); //Remove the selection and select it back after
                    }
                    // Otherwise, add rows to binded data source
                    else
                    {
                        try
                        {
                            BindingSource bindingSource = dataGridView.DataSource as BindingSource;
                            if (bindingSource != null)
                            {
                                // This is important!!  
                                // Cancel Edit before adding new entries into bindingsource
                                // If the UI is currently adding a new line (you have your cursor on the last time)
                                // You will System.InvalidOperationException                                
                                bindingSource.CancelEdit();
                                for (int i = 0; i < iNumNewRows; i++)
                                {
                                    Object obj = bindingSource.AddNew();
                                    dataGridView.ClearSelection();
                                }
                            }
                        }
                        catch
                        {
                            // failed adding row to binding data source
                            // It was okay for my application to ignore the error
                        }
                    }
                }
            }
            #endregion 

            #region New rows added, and selection forgoten. Need to reselect
            if (dataGridView.SelectedCells.Count == 0)
            {
                while (selectedCells.Count > 0)
                {
                    CellLocation cell = selectedCells.Pop();
                    dataGridView[cell.ColumnIndex, cell.RowIndex].Selected = true;
                }
            }
            #endregion 

            //Paste one clipboard "cell/text" to all selected (Only one text to more than one selected cell)
            //Paste many clipbaord "cell/text" to selected fields (Paste to more than one selected cell)
            //Paste many clipbaord "cell/text" bases on current location (Paste to one selected cell)

            Dictionary<CellLocation, DataGridViewGenericCell> undoCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            #region Count rows and columns in selctions
            List<int> columnsSelected = new List<int>();
            List<int> rowsSelected = new List<int>();
            foreach (CellLocation cellLocationCount in selectedCells)
            {
                if (!columnsSelected.Contains(cellLocationCount.ColumnIndex)) columnsSelected.Add(cellLocationCount.ColumnIndex);
                if (!rowsSelected.Contains(cellLocationCount.RowIndex)) rowsSelected.Add(cellLocationCount.RowIndex);
            }
            #endregion 

            int columnConentsCount = 0;
            if (rowContents.Count > 0) columnConentsCount = rowContents[0].Count;

            #region Paste one clipboard "cell/text" to all selected (Only one text to more than one selected cell)
            #region Paste - Source: Nothing selected - nothing to do
            if (rowContents.Count == 0 && columnConentsCount == 0)
            { 
                //Nothing found
            }
            #endregion
            #region Paste - Sourec: One Cell Selected (rowContents.Count == 1 && columnConentsCount == 1)
            else if (rowContents.Count == 1 && columnConentsCount == 1) 
            {
                NuberOfItemsToEdit = dataGridView.SelectedCells.Count;
                IsClipboardActive = true;
                foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
                {
                    String cellContent = rowContents[0][0];
                    try
                    {
                        if (!dataGridViewCell.ReadOnly ||
                            (dataGridViewCell.ColumnIndex >= leftColumnOverwrite && dataGridViewCell.ColumnIndex <= rightColumnOverwrite &&
                            dataGridViewCell.RowIndex >= topRowOverwrite && dataGridViewCell.RowIndex <= buttomRowOverwrite)
                            )
                        {
                            //Rememebr in the current value in cell before changed so we can "ReDo" in current DataGridView 
                            CellLocation cellPosition = new CellLocation(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                            undoCells.Add(cellPosition, DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView[dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex]));

                            DataGridViewHandler.SetCellValue(dataGridView, dataGridViewCell, Convert.ChangeType(cellContent, dataGridViewCell.ValueType));
                            if (removeTag) DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex, SwitchStates.Undefine);
                            dataGridView.InvalidateCell(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);                            
                        }       
                    } catch { }
                }
                NuberOfItemsToEdit = 0;
                IsClipboardActive = false;
            }
            #endregion
            #region Paste - One row, and multimple columns, Only cell selected / one row from clipboard to multiple rows
            else if (
                rowContents.Count == 1 && columnConentsCount > 1 && //One row, and multimple columns
                columnsSelected.Count > 1 && rowsSelected.Count >= 1)  //Only cell selected 
            {
                
                if (columnConentsCount != columnsSelected.Count)
                {
                    KryptonMessageBox.Show("Can't paste selection. Can only paste selection when have selected equal numbers of columns.\r\n" +
                        "Columns selected for copy: " + columnConentsCount + "\r\n" +
                        "Columns selected for paste: " + columnsSelected.Count,
                        "Can't paste selected text", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }

                NuberOfItemsToEdit = columnsSelected.Count * rowsSelected.Count;
                IsClipboardActive = true;

                columnsSelected.Sort();
                for (int columnIndex = 0; columnIndex < rowContents[0].Count; columnIndex++)
                {
                    
                    String cellContent = rowContents[0][columnIndex]; //Row content will always be 1
                    int columnIndexPaste = columnsSelected[columnIndex];

                    foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
                    {
                        
                        if (dataGridViewCell.ColumnIndex == columnIndexPaste)
                        {

                            try
                            {
                                if (!dataGridViewCell.ReadOnly ||
                                    (dataGridViewCell.ColumnIndex >= leftColumnOverwrite && dataGridViewCell.ColumnIndex <= rightColumnOverwrite &&
                                    dataGridViewCell.RowIndex >= topRowOverwrite && dataGridViewCell.RowIndex <= buttomRowOverwrite)
                                    )
                                {
                                    //Rememebr in the current value in cell before changed so we can "ReDo" in current DataGridView 
                                    CellLocation cellPosition = new CellLocation(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                                    undoCells.Add(cellPosition, DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridViewCell));

                                    DataGridViewHandler.SetCellValue(dataGridView, dataGridViewCell, Convert.ChangeType(cellContent, dataGridViewCell.ValueType));
                                    if (removeTag) DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex, SwitchStates.Undefine);
                                    dataGridView.InvalidateCell(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                                }
                            }
                            catch { }
                        }
                    }
                }
                NuberOfItemsToEdit = 0;
                IsClipboardActive = false;
            }
            #endregion
            #region Paste - one column from clipboard to multiple columns / Only cell selected
            else if (rowContents.Count > 1 && columnConentsCount == 1 && //One column, and multimple rows
                !(columnsSelected.Count == 1 && rowsSelected.Count == 1)) //Only cell selected
            {
                
                if (rowContents.Count != rowsSelected.Count)
                {
                    KryptonMessageBox.Show("Can't paste selection. Can only paste selection when have selected equal numbers of rows.\r\n" +
                        "Rows selected for copy: " + rowContents.Count + "\r\n" +
                        "Rows selected for paste: " + rowsSelected.Count,
                        "Can't paste selected text", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Warning, showCtrlCopy: true);
                    return;
                }

                NuberOfItemsToEdit = columnsSelected.Count * rowsSelected.Count;
                IsClipboardActive = true;
                rowsSelected.Sort();
                for (int rowIndex = 0; rowIndex < rowContents.Count; rowIndex++)
                {

                    String cellContent = rowContents[rowIndex][0];

                    foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
                    {
                        int columnIndexPaste = rowsSelected[rowIndex];

                        if (dataGridViewCell.RowIndex == columnIndexPaste)
                        {

                            try
                            {
                                if (!dataGridViewCell.ReadOnly ||
                                    (dataGridViewCell.ColumnIndex >= leftColumnOverwrite && dataGridViewCell.ColumnIndex <= rightColumnOverwrite &&
                                    dataGridViewCell.RowIndex >= topRowOverwrite && dataGridViewCell.RowIndex <= buttomRowOverwrite)
                                    )
                                {
                                    //Rememebr in the current value in cell before changed so we can "ReDo" in current DataGridView 
                                    CellLocation cellPosition = new CellLocation(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                                    undoCells.Add(cellPosition, DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridViewCell));

                                    DataGridViewHandler.SetCellValue(dataGridView, dataGridViewCell, Convert.ChangeType(cellContent, dataGridViewCell.ValueType));
                                    if (removeTag) DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex, SwitchStates.Undefine);
                                    dataGridView.InvalidateCell(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                                }
                            }
                            catch { }
                        }
                    }
                }
                NuberOfItemsToEdit = 0;
                IsClipboardActive = false;
            }
            #endregion
            #region Paste - rest
            else
            {
                NuberOfItemsToEdit = columnsSelected.Count * rowsSelected.Count;
                IsClipboardActive = true;

                foreach (List<string> rowContent in rowContents)
                {
                    int iCol = leftColumn;
                    foreach (string cellContent in rowContent)
                    {
                        try
                        {
                            bool cellOk = false;
                            if (dataGridView.SelectedCells.Count == 1)
                            {
                                cellOk = true;
                            }
                            else
                            {
                                foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
                                {
                                    if (dataGridViewCell.ColumnIndex == iCol && dataGridViewCell.RowIndex == iRow) cellOk = true;
                                }
                            }

                            if (cellOk)
                            {
                                if (iCol < dataGridView.Columns.Count)
                                {
                                    

                                    DataGridViewCell cell = dataGridView[iCol, iRow];
                                    //if (!cell.ReadOnly)
                                    if (!cell.ReadOnly ||
                                        (cell.ColumnIndex >= leftColumnOverwrite && cell.ColumnIndex <= rightColumnOverwrite &&
                                        cell.RowIndex >= topRowOverwrite && cell.RowIndex <= buttomRowOverwrite)
                                        )
                                    {
                                        //Rememebr in the current value in cell before changed so we can "ReDo" in current DataGridView 
                                        CellLocation cellPosition = new CellLocation(iCol, iRow);
                                        undoCells.Add(cellPosition, DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, iCol, iRow));
                                        DataGridViewHandler.SetCellValue(dataGridView, cell, Convert.ChangeType(cellContent, cell.ValueType));
                                        if (removeTag) DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, cell.ColumnIndex, cell.RowIndex, SwitchStates.Undefine);
                                        dataGridView.InvalidateCell(iCol, iRow);
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                        iCol++;
                    }
                    iRow++;
                    if (iRow >= dataGridView.Rows.Count)
                    {
                        break;
                    }
                }
                NuberOfItemsToEdit = 0;
                IsClipboardActive = false;
                dataGridView.ResumeLayout();
            }
            #endregion
            #endregion
            
            //CAN BE REMOVED foreach (CellLocation cellLocation in undoCells.Keys) dataGridView[cellLocation.ColumnIndex, cellLocation.RowIndex].Selected = true; //Already done, can be removed after check
            PushToUndoStack (dataGridView, undoCells);
        }
        #endregion

    }
}


