using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DataGridViewGeneric;
using MetadataLibrary;
using Thumbnails;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        #region Cell TriState Click
        bool triStateButtomClick = false;
        int tristateButtonWidth = 32;
        int tristateBittonHight = 20;

        private void dataGridViewPeople_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
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

        #region Cell Updated name
        private void dataGridViewPeople_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (!(sender is DataGridView)) return;
            
            object val = DataGridViewHandler.GetCellValue((DataGridView)sender, e.ColumnIndex, e.RowIndex);
            if (!(val is MetadataLibrary.RegionStructure)) return;
            MetadataLibrary.RegionStructure region = (MetadataLibrary.RegionStructure)val;

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

        #region Cell header - Face region - CellMouseUp

        private void dataGridViewPeople_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            drawingRegion = false;

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex == -1 && e.ColumnIndex == peopleMouseDownColumn)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
                if (dataGridViewGenericColumn == null) return;
                Image image = dataGridViewGenericColumn.Thumbnail;
                Rectangle rectangleRoundedCellBounds = DataGridViewHandler.CalulateCellRoundedRectangleCellBounds(
                    new Rectangle(0, 0, dataGridView.Columns[e.ColumnIndex].Width, dataGridView.ColumnHeadersHeight));
                Size thumbnailSize = DataGridViewHandler.CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);
                Rectangle rectangleCenterThumbnail = DataGridViewHandler.CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);

                if (DataGridViewHandler.IsMouseWithinRectangle(e.X, e.Y, rectangleCenterThumbnail))
                {
                
                    peopleMouseMoveX = e.X;
                    peopleMouseMoveY = e.Y;
                }

                dataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);

                if (Math.Abs(peopleMouseDownX - peopleMouseMoveX) > 1 && Math.Abs(peopleMouseMoveY - peopleMouseDownY) > 1)
                {
                    if (DataGridViewHandler.UpdateSelectedCellsWithNewMouseRegion(dataGridView, e.ColumnIndex, peopleMouseDownX, peopleMouseDownY, peopleMouseMoveX, peopleMouseMoveY))
                    {
                        foreach (DataGridViewCell cell in dataGridView.SelectedCells)
                        {
                            dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, cell.ColumnIndex);
                            if (dataGridViewGenericColumn != null)
                            {
                                Image imageCoverArt = LoadMediaCoverArtPoster(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath, false);

                                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, cell.RowIndex);
                                dataGridViewGenericRow.HeaderName = DataGridViewHandlerPeople.headerPeople;
                                DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, cell.RowIndex, dataGridViewGenericRow);
                                DataGridViewHandler.SetCellRowHeight(dataGridView, cell.RowIndex, DataGridViewHandler.GetCellRowHeight(dataGridView));

                                RegionStructure regionStructure = DataGridViewHandler.GetCellRegionStructure(dataGridView, cell.ColumnIndex, cell.RowIndex);
                                if (regionStructure != null)
                                {
                                    if (imageCoverArt != null) regionStructure.Thumbnail = RegionThumbnailHandler.CopyRegionFromImage(imageCoverArt, regionStructure);
                                    else regionStructure.Thumbnail = (Image)Properties.Resources.FaceLoading;
                                }
                            }                            
                        }

                        DataGridViewHandler.Refresh(dataGridView);
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
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex == -1 && e.ColumnIndex == peopleMouseDownColumn)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
                if (dataGridViewGenericColumn == null) return;
                Image image = dataGridViewGenericColumn.Thumbnail;              
                Rectangle rectangleRoundedCellBounds = DataGridViewHandler.CalulateCellRoundedRectangleCellBounds(
                    new Rectangle(0, 0, dataGridView.Columns[e.ColumnIndex].Width, dataGridView.ColumnHeadersHeight));
                Size thumbnailSize = DataGridViewHandler.CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);
                Rectangle rectangleCenterThumbnail = DataGridViewHandler.CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);

                if (DataGridViewHandler.IsMouseWithinRectangle (e.X, e.Y, rectangleCenterThumbnail))
                {
                    peopleMouseMoveX = e.X;
                    peopleMouseMoveY = e.Y;

                    dataGridView.InvalidateCell(e.ColumnIndex, e.RowIndex);
                }
            }
        }
        #endregion

        #endregion

        public AutoCompleteStringCollection ClientListDropDown()
        {
            List<string> regionNames = databaseAndCacheMetadataExiftool.ListAllRegionNamesCache(MetadataBrokerType.ExifTool, DateTime.Now.AddDays(-365), DateTime.Now);
            AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
            foreach (string regionName in regionNames)
            {
                if (!string.IsNullOrWhiteSpace(regionName)) autoCompleteStringCollection.Add(regionName);
            }
            return autoCompleteStringCollection;
        }


        private void dataGridViewPeople_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            //if (DataGridViewHandler.IsRow dataGridView.CurrentCell.ColumnIndex == 1)
            //if (DataGridViewHandler.IsRow dataGridView.CurrentCell.ColumnIndex == 1)
            //{
                TextBox prodCode = e.Control as TextBox;
                if (prodCode != null)
                {
                    prodCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    prodCode.AutoCompleteCustomSource = ClientListDropDown();
                    prodCode.AutoCompleteSource = AutoCompleteSource.CustomSource;

                }
            //}
            //else
            //{
            //    TextBox prodCode = e.Control as TextBox;
            //    if (prodCode != null)
            //    {
            //        prodCode.AutoCompleteMode = AutoCompleteMode.None;
            //    }
            //}
        }

        private List<string> lastUsedNames = new List<string>();
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

        private void SetPeopleStripToolMenu(ToolStripMenuItem toolStripMenuItem, int number, string name)
        {
            toolStripMenuItem.Tag = name;
            toolStripMenuItem.Text = "Rename #" + number + " " + name;

            Properties.Settings.Default.PeopleRename = string.Join("\r\n", lastUsedNames.ToArray());

        }

        public void PopulatePeopleToolStripMenuItems()
        {            
            toolStripMenuItemPeopleRenameFromAll.DropDownItems.Clear();
            toolStripMenuItemPeopleRenameFromMostUsed.DropDownItems.Clear();
            toolStripMenuItemPeopleRenameFromLast1.DropDownItems.Clear();

            List<string> regioNames;
            regioNames = databaseAndCacheMetadataExiftool.ListAllPersonalRegionNameTopCountCache(MetadataBrokerType.ExifTool, Properties.Settings.Default.SuggestRegionNameTopMostCount);
            foreach (string name in regioNames)
            {
                ToolStripMenuItem newTagSubItem = new ToolStripMenuItem();
                newTagSubItem.Name = name;
                newTagSubItem.Text = name;
                newTagSubItem.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameSelected_Click);
                toolStripMenuItemPeopleRenameFromMostUsed.DropDownItems.Add(newTagSubItem);
            }

            regioNames = databaseAndCacheMetadataExiftool.ListAllPersonalRegionsCache(MetadataBrokerType.ExifTool);
            foreach (string name in regioNames)
            {
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

        private void PeopleRenameSelected(DataGridView dataGridView, string nameSelected)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
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
            }

            if (updatedCells != null && updatedCells.Count > 0) 
                ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);

        }

        private void toolStripMenuItemPeopleRenameSelected_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            PeopleRenameSelected(dataGridView, ((ToolStripMenuItem)sender).Name);

            DataGridViewHandler.Refresh(dataGridView);
        }

        private void toolStripMenuItemPeopleRenameFromLast1_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            PeopleRenameSelected(dataGridView, (string)toolStripMenuItemPeopleRenameFromLast1.Tag);
            DataGridViewHandler.Refresh(dataGridView);
        }

        private void toolStripMenuItemPeopleRenameFromLast2_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            PeopleRenameSelected(dataGridView, (string)toolStripMenuItemPeopleRenameFromLast2.Tag);
            DataGridViewHandler.Refresh(dataGridView);
        }

        private void toolStripMenuItemPeopleRenameFromLast3_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            PeopleRenameSelected(dataGridView, (string)toolStripMenuItemPeopleRenameFromLast3.Tag);
            DataGridViewHandler.Refresh(dataGridView);
        }

        private void dataGridViewPeople_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true);
            for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
            {
                DataGridViewHandler.SetCellDefaultAfterUpdated(dataGridView, dataGridViewGenericCellStatus, columnIndex, e.RowIndex);
            }
            DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, e.RowIndex, 
                new DataGridViewGenericRow(DataGridViewHandlerPeople.headerPeople,
                dataGridView[e.ColumnIndex, e.RowIndex].Value == null ? "" : dataGridView[e.ColumnIndex, e.RowIndex].Value.ToString(), ReadWriteAccess.AllowCellReadAndWrite));
        }
    }
}
