using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DataGridViewGeneric;
using MetadataLibrary;

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
                    
                    if (DataGridViewHandler.GetCellReadOnly(dataGridView, e.ColumnIndex, selectedRow))
                    {
                        MessageBox.Show("The selected cell can't be changed, need select another cell.", "Wrong cell selected", MessageBoxButtons.OK);
                        return;
                    }                    
                }

                Image image = dataGridViewGenericColumn.FileEntryImage.Image;
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
                Image image = dataGridViewGenericColumn.FileEntryImage.Image;
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
                                Image imageCoverArt = LoadMediaCoverArtPoster(dataGridViewGenericColumn.FileEntryImage.FileFullPath);

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
                Image image = dataGridViewGenericColumn.FileEntryImage.Image;              
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

        //Refesh 
        private void dataGridViewPeople_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            Debug.WriteLine("Refresh");
            DataGridViewHandler.Refresh(dataGridView);
        }

        public AutoCompleteStringCollection ClientListDropDown()
        {
            List<string> regionNames = databaseAndCacheMetadataExiftool.ListAllRegionNamesCache(MetadataBrokerTypes.ExifTool, DateTime.Now.AddDays(-365), DateTime.Now);
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
    }
}
