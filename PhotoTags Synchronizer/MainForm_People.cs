using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DataGridViewGeneric;
using MetadataLibrary;
using Thumbnails;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        #region CellMouseClick
        private void dataGridViewPeople_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Rectangle cellRectangle = ((DataGridView)sender).GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            if (e.X >= cellRectangle.Width - tristateButtonWidth && e.Y <= tristateBittonHight) triStateButtomClick = true;
            else triStateButtomClick = false;
            if (!triStateButtomClick) return;

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (dataGridView.SelectedCells.Count < 1) return;

            DataGridViewSelectedCellCollection dataGridViewSelectedCellCollection = dataGridView.SelectedCells;
            if (dataGridViewSelectedCellCollection.Count < 1) return;

            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = DataGridViewHandler.ToggleCells(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Toggle, e.ColumnIndex, e.RowIndex);
            DataGridViewHandler.Refresh(dataGridView);
            if (updatedCells != null && updatedCells.Count > 0) ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
        }
        #endregion 

        #region Cell TriState Click
        bool triStateButtomClick = false;
        int tristateButtonWidth = 32;
        int tristateBittonHight = 20;
        #endregion

        #region Cell Updated name
        private void dataGridViewPeople_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (!(sender is DataGridView)) return;
            
            object val = DataGridViewHandler.GetCellValue((DataGridView)sender, e.ColumnIndex, e.RowIndex);
            if (!(val is RegionStructure)) return;
            RegionStructure region = (RegionStructure)val;

            if (region == null) return;
            region.Name = (string)e.Value;
            e.Value = region;
            e.ParsingApplied = true;
            PeopleAddNewLastUseName(region.Name);
        }
        #endregion

        #region Cell header - Face region
        private int peopleMouseDownX = -1;
        private int peopleMouseDownY = -1;
        private int peopleMouseMoveX = -1;
        private int peopleMouseMoveY = -1;
        private int peopleMouseDownColumn = int.MinValue;
        private bool drawingRegion = false;

        #region Cell header - Face region - CellMouseDown
        private void dataGridViewPeople_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            drawingRegion = false;
            if (e.Button != MouseButtons.Left) return;

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                if (!DataGridViewHandler.IsColumnSelected(dataGridView, e.ColumnIndex))
                {
                    MessageBox.Show("You need to select a name cell for current media file.", "Missing selection on media file", MessageBoxButtons.OK);
                    return;
                }

                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
                if (dataGridViewGenericColumn == null || dataGridViewGenericColumn.ReadWriteAccess != ReadWriteAccess.AllowCellReadAndWrite)
                {                    
                    MessageBox.Show("You can only change region on current version on media file, not on historical or error log.", "Not correct column type", MessageBoxButtons.OK);
                    return;
                }

                List<int> selectedRows = DataGridViewHandler.GetRowSelected(dataGridView);
                if (selectedRows.Count != 1)
                {
                    MessageBox.Show("You can only create a region for one name cell at once.", "Wrong number of selection", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    int selectedRow = selectedRows[0];
                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, selectedRow);

                    if (dataGridViewGenericRow == null || dataGridViewGenericRow.IsHeader)
                    {
                        MessageBox.Show("The selected cell can't be changed, need select another cell.", "Wrong cell selected", MessageBoxButtons.OK);
                        return;
                    } 
                }

                Image image = dataGridViewGenericColumn.Thumbnail;
                if (image == null)
                {
                    MessageBox.Show("No media has been load, please wait or reload the media to fetch thumbnail image.", "Not media has been loaded", MessageBoxButtons.OK);
                    return;
                }

                Rectangle rectangleRoundedCellBounds = DataGridViewHandler.CalulateCellRoundedRectangleCellBounds(
                    new Rectangle(0, 0, dataGridView.Columns[e.ColumnIndex].Width, dataGridView.ColumnHeadersHeight));
                Size thumbnailSize = DataGridViewHandler.CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);
                Rectangle rectangleCenterThumbnail = DataGridViewHandler.CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);

                if (DataGridViewHandler.IsMouseWithinRectangle(e.X, e.Y, rectangleCenterThumbnail))
                {
                    peopleMouseDownX = e.X;
                    peopleMouseDownY = e.Y;
                    peopleMouseDownColumn = e.ColumnIndex;

                    drawingRegion = true;
                }

            }
        }
        #endregion

        #region Cell header - Face region - CellMouseLeave
        private void dataGridViewPeople_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (drawingRegion)
            {
                drawingRegion = false;
                peopleMouseDownColumn = int.MinValue;

                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;
                DataGridViewHandler.Refresh(dataGridView);
            }
        }
        #endregion

        #region Cell header - UpdateRegionThumbnail
        private void UpdateRegionThumbnail(DataGridView dataGridView)
        {
            try
            {
                foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
                {

                    DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, dataGridViewCell.ColumnIndex);
                    if (dataGridViewGenericColumn != null)
                    {

                        Image imageCoverArt = LoadMediaCoverArtPoster(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath);
                        if (imageCoverArt != null)
                        {
                            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, dataGridViewCell.RowIndex);
                            dataGridViewGenericRow.HeaderName = DataGridViewHandlerPeople.headerPeople;
                            DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, dataGridViewCell.RowIndex, dataGridViewGenericRow);
                            DataGridViewHandler.SetCellRowHeight(dataGridView, dataGridViewCell.RowIndex, DataGridViewHandler.GetCellRowHeight(dataGridView));

                            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = DataGridViewHandler.GetCellStatus(dataGridViewCell);
                            dataGridViewGenericCellStatus.CellReadOnly = false;
                            DataGridViewHandler.SetCellReadOnlyDependingOfStatus(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex, dataGridViewGenericCellStatus);
                            //DataGridViewHandler.SetCellStatus(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex, dataGridViewGenericCellStatus);
                            DataGridViewHandler.SetCellDefaultAfterUpdated(dataGridView, dataGridViewGenericCellStatus, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
  
                            RegionStructure regionStructure = DataGridViewHandler.GetCellRegionStructure(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                            if (regionStructure != null)
                            {
                                if (imageCoverArt != null) regionStructure.Thumbnail = RegionThumbnailHandler.CopyRegionFromImage(imageCoverArt, regionStructure);
                                else regionStructure.Thumbnail = (Image)Properties.Resources.FaceLoading;
                            }
                        } else
                        {
                            Logger.Error("Was not able to updated the region thumbnail. Poster was failed to load.");
                            MessageBox.Show("Was not able to updated the region thumbnail.\r\nPoster was failed to load.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "UpdateRegionThumbnail");
                MessageBox.Show("Was not able to updated the region thumbnail.\r\n\r\n" + ex.Message);
            }
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion 

        #region Cell header - Face region - CellMouseUp
        private void dataGridViewPeople_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            drawingRegion = false;
            if (e.Button != MouseButtons.Left) return;

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex == -1 && e.ColumnIndex == peopleMouseDownColumn)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
                if (dataGridViewGenericColumn == null) return;

                lock (dataGridViewGenericColumn._ThumbnailLock)
                {
                    Image image = dataGridViewGenericColumn.thumbnailUnlock;
                    Rectangle rectangleRoundedCellBounds = DataGridViewHandler.CalulateCellRoundedRectangleCellBounds(
                        new Rectangle(0, 0, dataGridView.Columns[e.ColumnIndex].Width, dataGridView.ColumnHeadersHeight));
                    Size thumbnailSize = DataGridViewHandler.CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);
                    Rectangle rectangleCenterThumbnail = DataGridViewHandler.CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);

                    if (DataGridViewHandler.IsMouseWithinRectangle(e.X, e.Y, rectangleCenterThumbnail))
                    {
                        peopleMouseMoveX = e.X;
                        peopleMouseMoveY = e.Y;
                    }
                }

                dataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);

                if (Math.Abs(peopleMouseDownX - peopleMouseMoveX) > 1 && Math.Abs(peopleMouseMoveY - peopleMouseDownY) > 1)
                {
                    if (DataGridViewHandler.UpdateSelectedCellsWithNewMouseRegion(dataGridView, e.ColumnIndex, peopleMouseDownX, peopleMouseDownY, peopleMouseMoveX, peopleMouseMoveY))
                    {
                        UpdateRegionThumbnail(dataGridView);
                    }
                } else
                {
                    MessageBox.Show("Couldn't create a region. No region selection was made.", "No region selected", MessageBoxButtons.OK);
                    peopleMouseDownColumn = int.MinValue;
                }
                
            }
            peopleMouseDownColumn = int.MinValue;
        }
        #endregion

        #region Cell header - Face region - CellMouseMove
        private void dataGridViewPeople_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex == -1 && e.ColumnIndex == peopleMouseDownColumn)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
                if (dataGridViewGenericColumn == null) return;

                lock (dataGridViewGenericColumn._ThumbnailLock)
                {
                    Image image = dataGridViewGenericColumn.thumbnailUnlock;
                    Rectangle rectangleRoundedCellBounds = DataGridViewHandler.CalulateCellRoundedRectangleCellBounds(
                        new Rectangle(0, 0, dataGridView.Columns[e.ColumnIndex].Width, dataGridView.ColumnHeadersHeight));
                    Size thumbnailSize = DataGridViewHandler.CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);
                    Rectangle rectangleCenterThumbnail = DataGridViewHandler.CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);

                    if (DataGridViewHandler.IsMouseWithinRectangle(e.X, e.Y, rectangleCenterThumbnail))
                    {
                        peopleMouseMoveX = e.X;
                        peopleMouseMoveY = e.Y;

                        dataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
                    }
                }
            }
        }
        #endregion

        #endregion

        #region AutoComplete

        #region AutoComplete - ClientListDropDown
        public AutoCompleteStringCollection ClientListDropDown()
        {
            //List<string> regionName1 = databaseAndCacheMetadataExiftool.ListAllRegionNamesCache(MetadataBrokerType.Empty, null, null);
            List<string> regionNames = databaseAndCacheMetadataExiftool.ListAllPersonalRegionsCache();
            AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
            foreach (string regionName in regionNames)
            {
                if (!string.IsNullOrWhiteSpace(regionName)) autoCompleteStringCollection.Add(regionName);
            }
            return autoCompleteStringCollection;
        }
        #endregion

        #region AutoComplete - EditingControlShowing
        private void dataGridViewPeople_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            TextBox prodCode = e.Control as TextBox;
            if (prodCode != null)
            {
                prodCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                prodCode.AutoCompleteCustomSource = ClientListDropDown();
                prodCode.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }            
        }
        #endregion

        #endregion

        #region People name suggestion  

        private List<string> lastUsedNames = new List<string>();

        #region People name suggestion - PeopleAddNewLastUseName
        private void PeopleAddNewLastUseName(string name)
        {
            if (lastUsedNames.Contains(name)) lastUsedNames.Remove(name);
            lastUsedNames.Insert(0, name);
            while (lastUsedNames.Count > 3) lastUsedNames.RemoveAt(3);

            if (lastUsedNames.Count > 0)
            {
                SetPeopleStripToolMenu(toolStripMenuItemPeopleRenameFromLast1, 1, lastUsedNames[0]);
                toolStripMenuItemPeopleRenameFromLast1.Visible = true;
            }
            else toolStripMenuItemPeopleRenameFromLast1.Visible = false;

            if (lastUsedNames.Count > 1)
            {
                SetPeopleStripToolMenu(toolStripMenuItemPeopleRenameFromLast2, 2, lastUsedNames[1]);
                toolStripMenuItemPeopleRenameFromLast2.Visible = true;
            } else toolStripMenuItemPeopleRenameFromLast2.Visible = false;

            if (lastUsedNames.Count > 2)
            {
                SetPeopleStripToolMenu(toolStripMenuItemPeopleRenameFromLast3, 3, lastUsedNames[2]);
                toolStripMenuItemPeopleRenameFromLast3.Visible = true;
            }
            else toolStripMenuItemPeopleRenameFromLast3.Visible = false;
        }
        #endregion

        #region People name suggestion - SetPeopleStripToolMenu
        private void SetPeopleStripToolMenu(ToolStripMenuItem toolStripMenuItem, int number, string name)
        {
            toolStripMenuItem.Tag = name;
            toolStripMenuItem.Text = "Rename #" + number + " " + name;
            Properties.Settings.Default.PeopleRename = string.Join("\r\n", lastUsedNames.ToArray());
        }
        #endregion 

        private static HashSet<string> regionNamesRenameFromAllAdded = new HashSet<string>();
        private static HashSet<string> regionNamesRenameFromTopCoundAdded = new HashSet<string>();

        #region People name suggestion - PopulatePeopleToolStripMenuItems
        public void PopulatePeopleToolStripMenuItems()
        {            
            toolStripMenuItemPeopleRenameFromAll.DropDownItems.Clear();
            toolStripMenuItemPeopleRenameFromMostUsed.DropDownItems.Clear();
            toolStripMenuItemPeopleRenameFromLast1.DropDownItems.Clear();
            toolStripMenuItemPeopleRenameFromLast2.DropDownItems.Clear();
            toolStripMenuItemPeopleRenameFromLast3.DropDownItems.Clear();

            List<string> regioNames;
            regioNames = databaseAndCacheMetadataExiftool.ListAllPersonalRegionNameTopCountCache(MetadataBrokerType.ExifTool, Properties.Settings.Default.SuggestRegionNameTopMostCount);
            foreach (string name in regioNames)
            {
                regionNamesRenameFromTopCoundAdded.Add(name);
                ToolStripMenuItem newTagSubItem = new ToolStripMenuItem();
                newTagSubItem.Name = name;
                newTagSubItem.Text = name;
                newTagSubItem.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameSelected_Click);
                toolStripMenuItemPeopleRenameFromMostUsed.DropDownItems.Add(newTagSubItem);
            }

            regioNames = databaseAndCacheMetadataExiftool.ListAllPersonalRegionsCache();
            foreach (string name in regioNames)
            {
                regionNamesRenameFromAllAdded.Add(name);
                ToolStripMenuItem newTagSubItem = new ToolStripMenuItem();
                newTagSubItem.Name = name;
                newTagSubItem.Text = name;
                newTagSubItem.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameSelected_Click);
                toolStripMenuItemPeopleRenameFromAll.DropDownItems.Add(newTagSubItem);
            }

            string[] renameNames = Properties.Settings.Default.PeopleRename.Replace("\r", "").Split('\n');

            for (int i = renameNames.Length - 1; i >= 0; i--)
            {
                PeopleAddNewLastUseName(renameNames[i]);
            }
        }
        #endregion

        #endregion

        #region People rename
        
        private bool PeopleRenameCell(DataGridView dataGridView, DataGridViewCell cell, string nameSelected, Dictionary<CellLocation, DataGridViewGenericCell> updatedCells)
        {
            bool cellUpdated = false;

            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, cell.ColumnIndex);
            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, cell.RowIndex);

            if (dataGridViewGenericColumn != null && dataGridViewGenericRow != null &&
                dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite &&
                dataGridViewGenericRow.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite &&
                !dataGridViewGenericRow.IsHeader)
            {
                DataGridViewGenericCell dataGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, cell.ColumnIndex, cell.RowIndex);
                if (!dataGridViewGenericCell.CellStatus.CellReadOnly)
                {
                    CellLocation cellLocation = new CellLocation(cell.ColumnIndex, cell.RowIndex);
                    if (!updatedCells.ContainsKey(cellLocation)) updatedCells.Add(cellLocation, new DataGridViewGenericCell(dataGridViewGenericCell));
                    cellUpdated = true;
                    
                    if (dataGridViewGenericCell.Value is RegionStructure)
                    {
                        RegionStructure region = (RegionStructure)dataGridViewGenericCell.Value;
                        if (region != null)
                        {
                            region.Name = nameSelected;
                            PeopleAddNewLastUseName(nameSelected);
                            DataGridViewHandler.SetCellValue(dataGridView, cell.ColumnIndex, cell.RowIndex, region);
                        }
                    }
                    DataGridViewHandlerPeople.SetCellDefault(dataGridView, MetadataBrokerType.Empty, cell.ColumnIndex, cell.RowIndex);
                }

            }
            else if (dataGridViewGenericRow == null) //new row
            {
                DataGridViewHandlerPeople.AddRowPeople(dataGridView, nameSelected);
            }
            return cellUpdated;
        }

        #region People rename - PeopleRenameSelected
        private void PeopleRenameSelected(DataGridView dataGridView, string nameSelected)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                PeopleRenameCell(dataGridView, cell, nameSelected, updatedCells);                
            }
            
            if (updatedCells != null && updatedCells.Count > 0) ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
        }
        #endregion

        #region People rename - PeopleRenameSelected_Click
        private void toolStripMenuItemPeopleRenameSelected_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            PeopleRenameSelected(dataGridView, ((ToolStripMenuItem)sender).Name);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region People rename - PeopleRenameFromLast1_Click
        private void toolStripMenuItemPeopleRenameFromLast1_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            PeopleRenameSelected(dataGridView, (string)toolStripMenuItemPeopleRenameFromLast1.Tag);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region People rename - PeopleRenameFromLast2_Click
        private void toolStripMenuItemPeopleRenameFromLast2_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            PeopleRenameSelected(dataGridView, (string)toolStripMenuItemPeopleRenameFromLast2.Tag);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region People rename - PeopleRenameFromLast3_Click
        private void toolStripMenuItemPeopleRenameFromLast3_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            PeopleRenameSelected(dataGridView, (string)toolStripMenuItemPeopleRenameFromLast3.Tag);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #endregion

        #region CheckRowAndSetDefaults
        private void CheckRowAndSetDefaults(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            for (int columnIndexCheck = 0; columnIndexCheck < DataGridViewHandler.GetColumnCount(dataGridView); columnIndexCheck++)
            {
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus = DataGridViewHandler.GetCellStatus(dataGridView, columnIndexCheck, rowIndex);
                if (dataGridViewGenericCellStatus == null) dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true);

                DataGridViewHandler.SetCellDefaultAfterUpdated(dataGridView, dataGridViewGenericCellStatus, columnIndexCheck, rowIndex);
            }

            #region Set Row defaults
            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            string dataGridViewGenericRowHeaderName = (dataGridViewGenericRow != null ? dataGridViewGenericRow.HeaderName : DataGridViewHandlerPeople.headerPeopleAdded);
            DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, rowIndex,
                new DataGridViewGenericRow(dataGridViewGenericRowHeaderName,
                dataGridView[columnIndex, rowIndex].Value == null ? "" : dataGridView[columnIndex, rowIndex].Value.ToString(), ReadWriteAccess.AllowCellReadAndWrite));
            #endregion
        }
        #endregion 

        #region CellEndEdit        
        private void dataGridViewPeople_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            CheckRowAndSetDefaults(dataGridView, e.ColumnIndex, e.RowIndex);
        }
        #endregion

        #region FormRegionSelect
        FormRegionSelect formRegionSelect = new FormRegionSelect();

        #region FormRegionSelect - OpenRegionSelector
        private void OpenRegionSelector()
        {
            if (formRegionSelect == null || formRegionSelect.IsDisposed) formRegionSelect = new FormRegionSelect();
            formRegionSelect.OnRegionSelected -= FormRegionSelect_OnRegionSelected;
            formRegionSelect.OnRegionSelected += FormRegionSelect_OnRegionSelected;
            formRegionSelect.Owner = this;
            if (formRegionSelect.WindowState == FormWindowState.Minimized) formRegionSelect.WindowState = FormWindowState.Normal;
            formRegionSelect.BringToFront();
            formRegionSelect.Show();
        }
        #endregion 

        #region FormRegionSelect - RegionSelectorLoadAndSelect
        private void RegionSelectorLoadAndSelect(DataGridView dataGridView, int rowSelected = -1, int columnSelected = -1)
        {
            if (dataGridView == null) return;
            if (formRegionSelect == null) return;
            if (formRegionSelect.Visible == false) return;
            try
            {
                formRegionSelect.SetSelectionNone();
                if (!dataGridView.Enabled) { formRegionSelect.SetImageNone(); return; }

                string dataGridViewName = DataGridViewHandler.GetDataGridViewName(dataGridView);
                if (dataGridViewName == LinkTabAndDataGridViewNameRename)
                {
                    //Only one row can be selected, only one file can be shown
                    if (rowSelected == -1)
                    {
                        List<int> selectedRows = DataGridViewHandler.GetColumnSelected(dataGridView);
                        if (selectedRows.Count != 1) { formRegionSelect.SetImageNone(); return; } //Can only show one poster
                        rowSelected = selectedRows[0];
                    }

                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowSelected);
                    if (dataGridViewGenericRow == null) { formRegionSelect.SetImageNone(); return; }
                    if (dataGridViewGenericRow.IsHeader) { formRegionSelect.SetImageNone(); return; }

                    formRegionSelect.SetImageText(Path.Combine(dataGridViewGenericRow.HeaderName, dataGridViewGenericRow.RowName));
                    Image image = LoadMediaCoverArtPoster(Path.Combine(dataGridViewGenericRow.HeaderName, dataGridViewGenericRow.RowName));
                    formRegionSelect.SetImage(image, "Showing: " + dataGridViewGenericRow.RowName);
                }
                else if (dataGridViewName == LinkTabAndDataGridViewNameConvertAndMerge)
                {
                    //Only one row can be selected, only one file can be shown
                    if (rowSelected == -1)
                    {
                        List<int> selectedRows = DataGridViewHandler.GetColumnSelected(dataGridView);
                        if (selectedRows.Count != 1) { formRegionSelect.SetImageNone(); return; } //Can only show one poster
                        rowSelected = selectedRows[0];
                    }

                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowSelected);
                    if (dataGridViewGenericRow == null) { formRegionSelect.SetImageNone(); return; }
                    if (dataGridViewGenericRow.IsHeader) { formRegionSelect.SetImageNone(); return; }

                    formRegionSelect.SetImageText(dataGridViewGenericRow.RowName);
                    Image image = LoadMediaCoverArtPoster(dataGridViewGenericRow.RowName);
                    formRegionSelect.SetImage(image, "Showing: " + dataGridViewGenericRow.RowName);
                }
                else
                {
                    //Only one column can be selected, only one file can be shown
                    if (columnSelected == -1)
                    {
                        List<int> selectedColumns = DataGridViewHandler.GetColumnSelected(dataGridView);
                        if (selectedColumns.Count != 1) { formRegionSelect.SetImageNone(); return; } //Can only show one poster
                        columnSelected = selectedColumns[0];
                    }

                    DataGridViewSelectedCellCollection cellSelected = DataGridViewHandler.GetCellSelected(dataGridView);

                    if (DataGridViewHandler.GetCellSelectedCount(dataGridView) != 1 || dataGridViewName != LinkTabAndDataGridViewNamePeople)
                    {
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnSelected);
                        formRegionSelect.SetImageText(dataGridViewGenericColumn.FileEntryAttribute.FileName);
                        Image image = LoadMediaCoverArtPoster(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath);
                        formRegionSelect.SetImage(image, "Showing: " + dataGridViewGenericColumn.FileEntryAttribute.FileName);
                    }
                    else
                    {
                        int rowIndex = cellSelected[0].RowIndex;
                        int columnIndex = cellSelected[0].ColumnIndex;
                        if (rowIndex < 0 || columnIndex < 0) { formRegionSelect.SetImageNone(); return; }

                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                        if (dataGridViewGenericColumn == null || dataGridViewGenericColumn.ReadWriteAccess != ReadWriteAccess.AllowCellReadAndWrite) return;
                        //MessageBox.Show("You can only change region on current version on media file, not on historical or error log.", "Not correct column type", MessageBoxButtons.OK);

                        List<int> selectedRows = DataGridViewHandler.GetRowSelected(dataGridView);
                        if (selectedRows.Count != 1) { formRegionSelect.SetImageNone(); return; }
                        //MessageBox.Show("You can only create a region for one name cell at once.", "Wrong number of selection", MessageBoxButtons.OK);

                        int selectedRow = selectedRows[0];
                        DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, selectedRow);
                        if (dataGridViewGenericRow == null) { formRegionSelect.SetImageNone(); return; }
                        if (dataGridViewGenericRow.IsHeader) { formRegionSelect.SetImageNone(); return; }
                        //MessageBox.Show("The selected cell can't be changed, need select another cell.", "Wrong cell selected", MessageBoxButtons.OK);
                        if (dataGridViewGenericColumn.Metadata == null) { formRegionSelect.SetImageNone(); return; }

                        formRegionSelect.SetImageText(dataGridViewGenericColumn.Metadata.FileFullPath);
                        Image image = LoadMediaCoverArtPoster(dataGridViewGenericColumn.Metadata.FileFullPath);
                        if (image != null)
                        {
                            RegionStructure region = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndex);
                            if (region != null)
                            {
                                formRegionSelect.SetImage(image, "Select region: " + region.Name, columnIndex, rowIndex);

                                Rectangle rectangleInImage = region.GetImageRegionPixelRectangle(image.Size);
                                RectangleF rectangleFInImage = new RectangleF((float)rectangleInImage.X, (float)rectangleInImage.Y, (float)rectangleInImage.Width, (float)rectangleInImage.Height);
                                formRegionSelect.SetSelection(rectangleFInImage);
                            } else
                            {
                                formRegionSelect.SetImage(image, "Select region: create a new region", columnIndex, rowIndex);
                            }
                        }
                        else
                        {
                            Logger.Warn("Region selector was not able to load poster.");
                            MessageBox.Show("Region selector was not able to load poster.");
                        }

                    }
                    
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "RegionSelectorLoadAndSelect");
                MessageBox.Show("Region selector was not able to start.\r\n\r\n" + ex.Message);
            }
        }
        #endregion 

        #region FormRegionSelect - PeopleShowRegionSelector_Click
        private void toolStripMenuItemPeopleShowRegionSelector_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            OpenRegionSelector();
            RegionSelectorLoadAndSelect(dataGridView);
        }
        #endregion 

        #region FormRegionSelect - FormRegionSelect_OnRegionSelected
        private void FormRegionSelect_OnRegionSelected(object sender, RegionSelectedEventArgs e)
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            if (!dataGridView.Enabled) { formRegionSelect.SetImageNone(); return; }
            

            RectangleF region = RegionStructure.CalculateImageRegionAbstarctRectangle(e.ImageSize, 
                new Rectangle((int)e.Selection.X, (int)e.Selection.Y, (int)e.Selection.Width, (int)e.Selection.Height), 
                RegionStructureTypes.WindowsLivePhotoGallery);
            if (DataGridViewHandler.UpdateSelectedCellsWithNewRegion(dataGridView, e.ColumnIndex, region))
            {
                foreach (DataGridViewCell cell in dataGridView.SelectedCells)
                {
                    UpdateRegionThumbnail(dataGridView);
                }

                DataGridViewHandler.Refresh(dataGridView);
            }
        }
        #endregion

        #region FormRegionSelect - CellEnter
        private void dataGridViewPeople_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }
        #endregion

        #endregion

        #region ValitedatePastePeople
        private void ValitedatePastePeople(DataGridView dataGridView, string header)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();
            DataGridViewSelectedCellCollection dataGridViewSelectedCellCollection = DataGridViewHandler.GetCellSelected(dataGridView);
            foreach (DataGridViewCell dataGridViewCell in dataGridViewSelectedCellCollection)
            {
                //

                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, dataGridViewCell.ColumnIndex);
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, dataGridViewCell.RowIndex);

                if (dataGridViewGenericColumn == null)
                {
                    //CheckRowAndSetDefaults(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                }
                else if (dataGridViewGenericRow == null)
                {
                    CheckRowAndSetDefaults(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                }
                else 
                {
                    if (dataGridViewCell.Value != null) PeopleRenameCell(dataGridView, dataGridViewCell, dataGridViewCell.Value.ToString(), updatedCells);
                }
            }
            
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion
    }
}
