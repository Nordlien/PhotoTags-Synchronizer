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
        public static void PushToUndoStack(DataGridView dataGridView)
        {
            if (IsDoingUndoRedo) return;
            if (dataGridView.TopLeftHeaderCell.Tag == null)
                dataGridView.TopLeftHeaderCell.Tag = new DataGridViewGenericData();

            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return;

            PushToUndoStack(dataGridView, GetSelectedCells(dataGridView));
        }
        #endregion

        #region PushToUndoStack
        public static void PushToUndoStack(DataGridView dataGridView, Dictionary<CellLocation, DataGridViewGenericCell> cells)
        {
            if (IsDoingUndoRedo) return;
            if (dataGridView.TopLeftHeaderCell.Tag == null) dataGridView.TopLeftHeaderCell.Tag = new DataGridViewGenericData();
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return;
            
            DataGridViewGenericData undoAndRedo = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
            undoAndRedo.UndoCellsStack.Push(cells);
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
                    DataGridViewHandler.SetCellDataGridViewGenericCell(dataGridView, cell.Key.ColumnIndex, cell.Key.RowIndex, cell.Value);
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
                    DataGridViewHandler.SetCellDataGridViewGenericCell(dataGridView, cell.Key.ColumnIndex, cell.Key.RowIndex, cell.Value);
                }
                NuberOfItemsToEdit = 0;
                IsClipboardActive = false;
            }

            DataGridViewHandler.SetCurrentCellLocation(dataGridView, currentCell);
            IsDoingUndoRedo = false;
        }
        #endregion

        #region CopyDataGridViewSelectedCellsToClipboard
        public static void CopyDataGridViewSelectedCellsToClipboard(DataGridView dataGridView)
        {
            try
            {
                DataGridViewCell cell = dataGridView.CurrentCell;
                if (cell != null)
                {
                    if (cell is DataGridViewTextBoxCell || cell is DataGridViewComboBoxCell)
                    {
                        if (cell.IsInEditMode)
                        {
                            TextBox txt = dataGridView.EditingControl as TextBox;
                            if (txt != null) Clipboard.SetText(txt.SelectedText);
                        }
                        else
                        { 
                            dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                            DataObject dataObj = dataGridView.GetClipboardContent();
                            if (dataObj != null) Clipboard.SetDataObject(dataObj, true);
                        }
                    }
                }           
            }
            catch
            {
                MessageBox.Show("Can't copy cells to clipboard", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region DeleteDataGridViewSelectedCells
        public static void DeleteDataGridViewSelectedCells(DataGridView dataGridView)
        {
            DeleteDataGridViewSelectedCells(dataGridView, -1, -1, -1, -1, true);
        }
        #endregion

        #region DeleteDataGridViewSelectedCells
        public static void DeleteDataGridViewSelectedCells(DataGridView dataGridView, int leftColumnOverwrite, int rightColumnOverwrite, int topRowOverwrite, int buttomRowOverwrite, bool removeTag)
        {
            DataGridViewSelectedCellCollection dataGridViewSelectedCellCollection = dataGridView.SelectedCells;
            if (dataGridViewSelectedCellCollection.Count < 1) return;

            PushToUndoStack(dataGridView);

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
            IsClipboardActive = false;
        }
        #endregion

        #region PasteDataGridViewSelectedCellsFromClipboard
        public static void PasteDataGridViewSelectedCellsFromClipboard(DataGridView dataGridView)
        {
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, -1, -1, -1, -1, true);
        }
        #endregion

        #region PasteDataGridViewSelectedCellsFromClipboard
        public static void PasteDataGridViewSelectedCellsFromClipboard(DataGridView dataGridView, int leftColumnOverwrite, int rightColumnOverwrite, int topRowOverwrite, int buttomRowOverwrite, bool removeTag)
        {
             
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

                    bool anyCellFound = false;
                    while (!String.IsNullOrWhiteSpace(trMatch.Value))
                    {
                        anyCellFound = true;
                        int rowStart = trMatch.Index + trMatch.Length;
                        int rowEnd = HtmlFormat.IndexOf("</tr>", rowStart, StringComparison.InvariantCultureIgnoreCase);
                        System.Text.RegularExpressions.Match tdMatch = TDregex.Match(HtmlFormat, rowStart, rowEnd - rowStart);
                        List<string> rowContent = new List<string>();
                        while (!String.IsNullOrWhiteSpace(tdMatch.Value))
                        {
                            int cellStart = tdMatch.Index + tdMatch.Length;
                            int cellEnd = HtmlFormat.IndexOf("</td>", cellStart, StringComparison.InvariantCultureIgnoreCase);
                            String cellContent = HtmlFormat.Substring(cellStart, cellEnd - cellStart);
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

            // -----------------------------------------------------------------------------
            // Put the feach data to cells
            // -----------------------------------------------------------------------------

            Stack<CellLocation> selectedCells = new Stack<CellLocation>();

            #region Find topRow and leftColumn
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
            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                if (!columnsSelected.Contains(dataGridViewCell.ColumnIndex)) columnsSelected.Add(dataGridViewCell.ColumnIndex);
                if (!rowsSelected.Contains(dataGridViewCell.RowIndex)) rowsSelected.Add(dataGridViewCell.RowIndex);
            }
            #endregion 

            int columnConentsCount = 0;
            if (rowContents.Count > 0) columnConentsCount = rowContents[0].Count;

            //Paste one clipboard "cell/text" to all selected (Only one text to more than one selected cell)
            if (rowContents.Count == 0 && columnConentsCount == 0)
            { 
                //Nothing found
            }
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
            //Paste one row from clipboard to multiple rows
            else if (
                rowContents.Count == 1 && columnConentsCount > 1 && //One row, and multimple columns
                columnsSelected.Count > 1 && rowsSelected.Count >= 1)  //Only cell selected 
            {
                
                if (columnConentsCount != columnsSelected.Count)
                {
                    MessageBox.Show("Can't paste selection. Can only paste selection when have selected equal numbers of columns.\r\n" +
                        "Columns selected for copy: " + columnConentsCount + "\r\n" +
                        "Columns selected for paste: " + columnsSelected.Count,
                        "Can't paste selected text", MessageBoxButtons.OK);
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
            //Paste one column from clipboard to multiple columns
            else if (rowContents.Count > 1 && columnConentsCount == 1 && //One column, and multimple rows
                !(columnsSelected.Count == 1 && rowsSelected.Count == 1)) //Only cell selected
            {
                
                if (rowContents.Count != rowsSelected.Count)
                {
                    MessageBox.Show("Can't paste selection. Can only paste selection when have selected equal numbers of rows.\r\n" +
                        "Rows selected for copy: " + rowContents.Count + "\r\n" +
                        "Rows selected for paste: " + rowsSelected.Count,
                        "Can't paste selected text", MessageBoxButtons.OK);
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
            else
            {
                NuberOfItemsToEdit = columnsSelected.Count * rowsSelected.Count;
                IsClipboardActive = true;

                foreach (List<String> rowContent in rowContents)
                {
                    int iCol = leftColumn;
                    foreach (String cellContent in rowContent)
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

            foreach (CellLocation cellLocation in undoCells.Keys) dataGridView[cellLocation.ColumnIndex, cellLocation.RowIndex].Selected = true;
            PushToUndoStack (dataGridView, undoCells);
        }
        #endregion

    }
}


