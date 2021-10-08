using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DataGridViewGeneric;
using MetadataLibrary;
using Thumbnails;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
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
                                else regionStructure.Thumbnail = (Image)Properties.Resources.RegionLoading;
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
                SetPeopleStripToolMenu(kryptonContextMenuItemGenericRegionRename1, 1, lastUsedNames[0]);
                kryptonContextMenuItemGenericRegionRename1.Visible = true;
            }
            else kryptonContextMenuItemGenericRegionRename1.Visible = false;

            if (lastUsedNames.Count > 1)
            {
                SetPeopleStripToolMenu(kryptonContextMenuItemGenericRegionRename2, 2, lastUsedNames[1]);
                kryptonContextMenuItemGenericRegionRename2.Visible = true;
            } else kryptonContextMenuItemGenericRegionRename2.Visible = false;

            if (lastUsedNames.Count > 2)
            {
                SetPeopleStripToolMenu(kryptonContextMenuItemGenericRegionRename3, 3, lastUsedNames[2]);
                kryptonContextMenuItemGenericRegionRename3.Visible = true;
            }
            else kryptonContextMenuItemGenericRegionRename3.Visible = false;
        }
        #endregion

        #region People name suggestion - SetPeopleStripToolMenu
        private void SetPeopleStripToolMenu(KryptonContextMenuItem toolStripMenuItem, int number, string name)
        {
            toolStripMenuItem.Tag = name;
            toolStripMenuItem.Text = "Rename #" + number;
            toolStripMenuItem.ExtraText = name;
            Properties.Settings.Default.PeopleRename = string.Join("\r\n", lastUsedNames.ToArray());
        }
        #endregion 

        private static HashSet<string> regionNamesRenameFromAllAdded = new HashSet<string>();
        private static HashSet<string> regionNamesRenameFromTopCoundAdded = new HashSet<string>();

        #region People name suggestion - PopulatePeopleToolStripMenuItems
        private int FindFirstUnequal(string text1, string text2)
        {
            int index = 0;
            while (true)
            {
                if (index >= text1.Length) return index; 
                if (index >= text2.Length) return index;
                if (text1[index] != text2[index]) return index;
                //     abc   index = 0    
                //
                //abc  abc
                //012  012   index = 3
                //
                //abc1 abc2
                //0123 0123  index = 3
                //               
                index++;
            }
        }

        private string GetSubStringIndex(string text, int index)
        {
            return text.Substring(0, Math.Min(index, text.Length));
        }

        public void PopulatePeopleToolStripMenuItems()
        {
            kryptonContextMenuItemsGenericRegionRenameListAllList.Items.Clear();
            kryptonContextMenuItemsGenericRegionRenameFromLastUsedList.Items.Clear();

            List<string> regioNames;
            regioNames = databaseAndCacheMetadataExiftool.ListAllPersonalRegionNameTopCountCache(MetadataBrokerType.ExifTool, Properties.Settings.Default.SuggestRegionNameTopMostCount);
            foreach (string name in regioNames)
            {
                regionNamesRenameFromTopCoundAdded.Add(name);
                KryptonContextMenuItem kryptonContextMenuItemGenericRegionRenameFromLastUsed = new KryptonContextMenuItem();
                kryptonContextMenuItemGenericRegionRenameFromLastUsed.Tag = name;
                kryptonContextMenuItemGenericRegionRenameFromLastUsed.Text = name;
                kryptonContextMenuItemGenericRegionRenameFromLastUsed.Click += KryptonContextMenuItemRegionRenameGeneric_Click;
                this.kryptonContextMenuItemsGenericRegionRenameFromLastUsedList.Items.Add(kryptonContextMenuItemGenericRegionRenameFromLastUsed);
            }


            int groupSize = 30;

            regioNames = databaseAndCacheMetadataExiftool.ListAllPersonalRegionsCache();
            if (regioNames.Count <= groupSize)
            {
                foreach (string name in regioNames)
                {
                    regionNamesRenameFromAllAdded.Add(name);
                    KryptonContextMenuItem kryptonContextMenuItemGenericRegionRenameFromListAll = new KryptonContextMenuItem();
                    kryptonContextMenuItemGenericRegionRenameFromListAll.Tag = name;
                    kryptonContextMenuItemGenericRegionRenameFromListAll.Text = name;
                    kryptonContextMenuItemGenericRegionRenameFromListAll.Click += KryptonContextMenuItemRegionRenameGeneric_Click;
                    kryptonContextMenuItemsGenericRegionRenameListAllList.Items.Add(kryptonContextMenuItemGenericRegionRenameFromListAll);
                }
            }
            else
            {
                KryptonContextMenuItem kryptonContextMenuItemGenericGroupName = null;
                KryptonContextMenuItem kryptonContextMenuItemGenericGroupNamePrevious = null;
                KryptonContextMenuItems kryptonContextMenuItemGenericGroupList = null;
                
                string firstNameInGroupCurrent = "";
                string lastNameInGroupCurrent = "";
                string lastNameInGroupPrevious = "";
                
                string firstNameInGroubSubPrevious = "";
                string lastNameInGroupSubPrevious = "";

                string firstGroupNamePrevious = "";

                int indexName = 0;
                bool nameFixed = false;

                foreach (string name in regioNames)
                {
                    if (kryptonContextMenuItemGenericGroupName == null)
                    {
                        kryptonContextMenuItemGenericGroupName = new KryptonContextMenuItem();
                        kryptonContextMenuItemsGenericRegionRenameListAllList.Items.Add(kryptonContextMenuItemGenericGroupName);
                        kryptonContextMenuItemGenericGroupList = new KryptonContextMenuItems();
                        kryptonContextMenuItemGenericGroupName.Items.Add(kryptonContextMenuItemGenericGroupList);
                        nameFixed = false;
                    }

                    regionNamesRenameFromAllAdded.Add(name);
                    KryptonContextMenuItem kryptonContextMenuItemGenericRegionRenameFromListAll = new KryptonContextMenuItem();
                    kryptonContextMenuItemGenericRegionRenameFromListAll.Tag = name;
                    kryptonContextMenuItemGenericRegionRenameFromListAll.Text = name;
                    Image image = databaseAndCacheMetadataExiftool.ReadRandomThumbnailFromCacheOrDatabase(name);
                    if (image != null) kryptonContextMenuItemGenericRegionRenameFromListAll.Image = image;
                    kryptonContextMenuItemGenericRegionRenameFromListAll.Click += KryptonContextMenuItemRegionRenameGeneric_Click;
                    kryptonContextMenuItemGenericGroupList.Items.Add(kryptonContextMenuItemGenericRegionRenameFromListAll);

                    if (indexName == 0) firstNameInGroupCurrent = name;
                    if (indexName >= groupSize - 1)
                    {
                        lastNameInGroupCurrent = name;

                        int indexNotEqualPrevious = FindFirstUnequal(firstGroupNamePrevious, lastNameInGroupPrevious) + 1;
                        int indexNotEqualPreviousAndCurrent = FindFirstUnequal(lastNameInGroupPrevious, firstNameInGroupCurrent) + 1;
                        int indexNotEqualCurrent = FindFirstUnequal(firstNameInGroupCurrent, lastNameInGroupCurrent) + 1;
                        
                        if (kryptonContextMenuItemGenericGroupNamePrevious != null)
                        {
                            lastNameInGroupSubPrevious = GetSubStringIndex(lastNameInGroupPrevious, Math.Max(indexNotEqualPrevious, indexNotEqualPreviousAndCurrent));
                            kryptonContextMenuItemGenericGroupNamePrevious.Text = firstNameInGroubSubPrevious + " - " + lastNameInGroupSubPrevious;
                        }

                        string currentFirstSubName = GetSubStringIndex(firstNameInGroupCurrent, Math.Max(indexNotEqualCurrent, indexNotEqualPreviousAndCurrent));
                        string currentLastSubName = GetSubStringIndex(lastNameInGroupCurrent, indexNotEqualCurrent);
                        kryptonContextMenuItemGenericGroupName.Text = currentFirstSubName + " - " + currentLastSubName;


                        firstGroupNamePrevious = firstNameInGroupCurrent; 
                        lastNameInGroupPrevious = lastNameInGroupCurrent;

                        firstNameInGroubSubPrevious = currentFirstSubName;
                        lastNameInGroupSubPrevious = currentLastSubName;

                        //Get ready for new group
                        indexName = 0;
                        kryptonContextMenuItemGenericGroupNamePrevious = kryptonContextMenuItemGenericGroupName;
                        kryptonContextMenuItemGenericGroupName = null;
                        kryptonContextMenuItemGenericGroupList = null;
                        nameFixed = true;
                    } else indexName++;

                    

                    
                }

                if (!nameFixed)
                {
                    int indexNotEqual = FindFirstUnequal(firstNameInGroupCurrent, lastNameInGroupCurrent) + 1;
                    kryptonContextMenuItemGenericGroupName.Text = GetSubStringIndex(firstNameInGroupCurrent, indexNotEqual) + " - " + GetSubStringIndex(lastNameInGroupCurrent, indexNotEqual);
                }
            }
            string[] renameNames = Properties.Settings.Default.PeopleRename.Replace("\r", "").Split('\n');

            for (int i = renameNames.Length - 1; i >= 0; i--)
            {
                PeopleAddNewLastUseName(renameNames[i]);
            }
        }

        

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
        private void KryptonContextMenuItemRegionRenameGeneric_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            PeopleRenameSelected(dataGridView, (string)((KryptonContextMenuItem)sender).Tag);
            DataGridViewHandler.Refresh(dataGridView);
        }

        #endregion

        #region ActionRegionRename
        private void ActionRegionRename(string name)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            PeopleRenameSelected(dataGridView, name);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region RegionRename1, 2, 3 - Click Events Sources
        private void KryptonContextMenuItemGenericRegionRenameGeneric_Click(object sender, EventArgs e)
        {
            ActionRegionRename((string)((KryptonContextMenuItem)sender).Tag);
        }
        #endregion


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
            if (((KryptonDataGridView)sender)[e.ColumnIndex, e.RowIndex].Value is RegionStructure regionStructure) regionStructure.ShowNameInToString = false; //Just a hack so KryptonDataGridView don't print name alse
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

            Cyotek.Windows.Forms.ImageBoxSelectionMode imageBoxSelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Zoom;
            try
            {
                formRegionSelect.SetSelectionNone();
                if (!dataGridView.Enabled) { formRegionSelect.SetImageNone("No valid media file is selected, and no data loaded."); return; }

                string dataGridViewName = DataGridViewHandler.GetDataGridViewName(dataGridView);
                if (dataGridViewName == LinkTabAndDataGridViewNameRename || dataGridViewName == LinkTabAndDataGridViewNameConvertAndMerge)
                {
                    string errorMessag = "No valid media file is selected.\r\nSelect a row or a cell within a row to present a poster of media file.";


                    //Only one row can be selected, only one file can be shown
                    if (rowSelected == -1)
                    {
                        List<int> selectedRows = DataGridViewHandler.GetColumnSelected(dataGridView);
                        if (selectedRows.Count != 1) { formRegionSelect.SetImageNone(errorMessag); return; } //Can only show one poster
                        rowSelected = selectedRows[0];
                    }

                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowSelected);
                    if (dataGridViewGenericRow == null) { formRegionSelect.SetImageNone(errorMessag); return; }
                    if (dataGridViewGenericRow.IsHeader) { formRegionSelect.SetImageNone(errorMessag); return; }

                    formRegionSelect.SetImageText(Path.Combine(dataGridViewGenericRow.HeaderName, dataGridViewGenericRow.RowName));
                    Image image = LoadMediaCoverArtPoster(Path.Combine(dataGridViewGenericRow.HeaderName, dataGridViewGenericRow.RowName));
                    formRegionSelect.SetImage(image, "Showing: " + dataGridViewGenericRow.RowName, imageBoxSelectionMode);
                }/*
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
                }*/
                else
                {
                    string errorMessag;
                    if (dataGridViewName != LinkTabAndDataGridViewNamePeople) errorMessag = "No valid media file is selected.\r\nSelect a column or a cell within a column to present a poster of media file.";
                    else
                    {
                        errorMessag = "No valid media file is selected.\r\n" +
                          "You need to select a 'region name cell'.\r\n" +
                          "Then you can drag and drop to create a region square for select cell.\r\n" +
                          "The region will be added to selected cell and will become named.";
                        imageBoxSelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Rectangle;
                    }
                    //Only one column can be selected, only one file can be shown
                    if (columnSelected == -1)
                    {
                        List<int> selectedColumns = DataGridViewHandler.GetColumnSelected(dataGridView);
                        if (selectedColumns.Count != 1) { formRegionSelect.SetImageNone(errorMessag); return; } //Can only show one poster
                        columnSelected = selectedColumns[0];
                    }

                    DataGridViewSelectedCellCollection cellSelected = DataGridViewHandler.GetCellSelected(dataGridView);

                    if (DataGridViewHandler.GetCellSelectedCount(dataGridView) != 1 || dataGridViewName != LinkTabAndDataGridViewNamePeople)
                    {
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnSelected);
                        formRegionSelect.SetImageText(dataGridViewGenericColumn.FileEntryAttribute.FileName);
                        Image image = LoadMediaCoverArtPoster(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath);
                        formRegionSelect.SetImage(image, "Showing: " + dataGridViewGenericColumn.FileEntryAttribute.FileName, imageBoxSelectionMode);
                    }
                    else
                    {
                        
                        int rowIndex = cellSelected[0].RowIndex;
                        int columnIndex = cellSelected[0].ColumnIndex;
                        if (rowIndex < 0 || columnIndex < 0) { formRegionSelect.SetImageNone(errorMessag); return; }

                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                        if (dataGridViewGenericColumn == null || dataGridViewGenericColumn.ReadWriteAccess != ReadWriteAccess.AllowCellReadAndWrite) return;
                        //MessageBox.Show("You can only change region on current version on media file, not on historical or error log.", "Not correct column type", MessageBoxButtons.OK);

                        List<int> selectedRows = DataGridViewHandler.GetRowSelected(dataGridView);
                        

                        int selectedRow = selectedRows[0];
                        DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, selectedRow);


                        if (selectedRows.Count != 1) { formRegionSelect.SetImageNone(errorMessag); return; }
                        //MessageBox.Show("You can only create a region for one name cell at once.", "Wrong number of selection", MessageBoxButtons.OK);
                        if (dataGridViewGenericRow == null) { formRegionSelect.SetImageNone(errorMessag); return; }
                        if (dataGridViewGenericRow.IsHeader) { formRegionSelect.SetImageNone(errorMessag); return; }
                        //MessageBox.Show("The selected cell can't be changed, need select another cell.", "Wrong cell selected", MessageBoxButtons.OK);
                        if (dataGridViewGenericColumn.Metadata == null) { formRegionSelect.SetImageNone(errorMessag); return; }
                         

                        formRegionSelect.SetImageText(dataGridViewGenericColumn.Metadata.FileFullPath);
                        Image image = LoadMediaCoverArtPoster(dataGridViewGenericColumn.Metadata.FileFullPath);
                        if (image != null)
                        {
                            RegionStructure region = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndex);
                            if (region != null)
                            {
                                formRegionSelect.SetImage(image, "Select region: " + region.Name, imageBoxSelectionMode, columnIndex, rowIndex);

                                Rectangle rectangleInImage = region.GetImageRegionPixelRectangle(image.Size);
                                RectangleF rectangleFInImage = new RectangleF((float)rectangleInImage.X, (float)rectangleInImage.Y, (float)rectangleInImage.Width, (float)rectangleInImage.Height);
                                formRegionSelect.SetSelection(rectangleFInImage);
                            } else
                            {
                                formRegionSelect.SetImage(image, "Select region: create a new region", imageBoxSelectionMode, columnIndex, rowIndex);
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

        #region FormRegionSelect - FormRegionSelect_OnRegionSelected
        private void FormRegionSelect_OnRegionSelected(object sender, RegionSelectedEventArgs e)
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            if (!dataGridView.Enabled) { formRegionSelect.SetImageNone("No valid media file is selected, and no data loaded."); return; }
            

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
