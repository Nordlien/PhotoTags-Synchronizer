﻿using Krypton.Toolkit;
using MetadataLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FileHandeling;
using ColumnNamesAndWidth;
using System.Runtime.CompilerServices;

namespace DataGridViewGeneric
{
    public enum DataGridViewSize
    {
        Large = 1,
        Medium = 2,
        Small = 3,
        RenameConvertAndMergeSize = 128,
        ConfigSize = 256
    }

    public partial class DataGridViewHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region IsFilenameEqual
        public static bool IsFilenameEqual(string fullFileName1, string fullFileName2)
        {
            if (fullFileName1 == null && fullFileName2 != null) return false;
            if (fullFileName1 != null && fullFileName2 == null) return false;
            if (fullFileName1 == null && fullFileName2 == null) return true;
            return String.Compare(fullFileName1, fullFileName2, comparisonType: StringComparison.OrdinalIgnoreCase) == 0;
        }
        #endregion

        #region Palette Colors

        #region Color - Cell - Normal
        public static Color ColorBackCellNormal(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlLightLight;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateNormal.DataCell.Back.Color1;
        }


        public static Color ColorTextCellNormal(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateNormal.DataCell.Content.Color1;
        }
        #endregion

        #region Color - Cell - Favorite
        public static Color ColorBackCellFavorite(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlLight;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateNormal.DataCell.Back.Color2;
        }

        public static Color ColorTextCellFavorite(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateNormal.DataCell.Content.Color2;
        }
        #endregion

        #region Color - Cell - ReadOnly
        public static Color ColorBackCellReadOnly(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.GradientInactiveCaption;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateDisabled.DataCell.Back.Color1;
        }

        public static Color ColorTextCellReadOnly(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateDisabled.DataCell.Content.Color1;
        }
        #endregion

        #region Color - Cell - Favorite - ReadOnly
        public static Color ColorBackCellFavoriteReadOnly(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.MenuHighlight;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateDisabled.DataCell.Back.Color2;
        }

        public static Color ColorTextCellFavoriteReadOnly(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateDisabled.DataCell.Content.Color2;
        }
        #endregion

        #region Color - Cell - Error
        public static Color ColorBackCellError(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return Color.FromArgb(255, 192, 192);
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom2.StateNormal.DataCell.Back.Color1;
        }

        public static Color ColorTextCellError(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom2.StateNormal.DataCell.Content.Color1;
        }
        #endregion

        #region Color - Cell - Image
        public static Color ColorBackCellImage(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return Color.White;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom3.StateNormal.DataCell.Back.Color1;
        }

        public static Color ColorTextCellImage(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom3.StateNormal.DataCell.Content.Color1;
        }
        #endregion

        #region Color - Header - Normal 
        public static Color ColorBackHeaderNormal(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.Control;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color1;
        }

        public static Color ColorTextHeaderNormal(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCommon.StateCommon.HeaderColumn.Content.Color1;
        }
        #endregion

        #region Color - Header - Warning
        public static Color ColorBackHeaderWarning(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return Color.Yellow;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom1.StateCommon.HeaderColumn.Back.Color1;
        }

        public static Color ColorTextHeaderWarning(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom1.StateCommon.HeaderColumn.Content.Color1;
        }
        #endregion

        #region Color - Header - Error
        public static Color ColorBackHeaderError(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return Color.Red;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom2.StateCommon.HeaderColumn.Back.Color1;
        }

        public static Color ColorTextHeaderError(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom2.StateCommon.HeaderColumn.Content.Color1;
        }
        #endregion

        #region Color - Header - Image
        public static Color ColorBackHeaderImage(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return Color.LightSteelBlue;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom3.StateCommon.HeaderColumn.Back.Color1;
        }

        public static Color ColorTextHeaderImage(DataGridView dataGridView)
        {
            if (dataGridView == null || dataGridView.TopLeftHeaderCell.Tag == null) return SystemColors.ControlText;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).KryptonCustomPaletteBase.GridStyles.GridCustom3.StateCommon.HeaderColumn.Content.Color1;
        }
        #endregion



        #endregion

        private DataGridView dataGridView;

        #region DataGridView events handling

        #region DataGridView events handling - RowValidated - Speed hack
        private void DataGridView_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            //Do nothing, this speed up the DataGridView a lot
            //throw new NotImplementedException();
        }
        #endregion 

        #region DataGridView events handling - UserDeletingRow - NotImplementedException
        private void DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region DataGridView events handling - CancelRowEdit - NotImplementedException
        private void DataGridView_CancelRowEdit(object sender, QuestionEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region DataGridView events handling - RowDirtyStateNeeded - NotImplementedException
        private void DataGridView_RowDirtyStateNeeded(object sender, QuestionEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region DataGridView events handling - NewRowNeeded - NotImplementedException
        private void DataGridView_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region DataGridView events handling - CellValuePushed - NotImplementedException
        private void DataGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region DataGridView events handling - CellValueNeeded - NotImplementedException
        private void DataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region DataGridView events handling - CurrentCellDirtyStateChanged
        private void DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //Commit changes ASAP, e.g. when SelectedIndexChanged will change the ValueChanges event be triggered
            dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            SetColumnDirtyFlag(dataGridView, dataGridView.CurrentCell.ColumnIndex, true);
        }
        #endregion

        #region DataGridView events handling - KeyDown 
        private void DataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            Krypton.Toolkit.KryptonDataGridView kryptonDataGridView = (Krypton.Toolkit.KryptonDataGridView)sender;

            // If we have a defined context menu then need to check for matching shortcut
            if (kryptonDataGridView.KryptonContextMenu != null)
            {
                if (kryptonDataGridView.KryptonContextMenu.ProcessShortcut(e.KeyData))
                    e.Handled = true;
            }
        }
        #endregion

        #region DataGridView events handling - CellMouseDown - Select correct Cell when Right Click Mouse button
        private void DataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex == -1 && e.RowIndex >= 0) //Row selected
                {
                    if (!dataGridView.Rows[e.RowIndex].Selected)
                    {
                        dataGridView.ClearSelection();
                        dataGridView.Rows[e.RowIndex].Selected = true;
                    }
                }
                else if (e.ColumnIndex >= 0 && e.RowIndex == -1) //Column selected
                {
                    bool isColumnSelected = true;
                    for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                    {
                        if (!dataGridView[e.ColumnIndex, rowIndex].Selected)
                        {
                            isColumnSelected = false;
                            break;
                        }
                    }

                    if (!isColumnSelected)
                    {
                        dataGridView.ClearSelection();
                        for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                        {
                            dataGridView[e.ColumnIndex, rowIndex].Selected = true;
                        }
                    }
                }
                else if (e.ColumnIndex == -1 && e.RowIndex == -1) //All Columns and Rows selected
                {
                    dataGridView.SelectAll();
                }
                else
                {
                    if (!dataGridView[e.ColumnIndex, e.RowIndex].Selected)
                    {
                        dataGridView.CurrentCell = dataGridView[e.ColumnIndex, e.RowIndex];
                    }
                    else 
                    {
                        //Hack to set CurrentCell, due to When set CurrentCell all Selected cells get removed
                        dataGridView.SuspendLayout();
                        List<DataGridViewCell> dataGridCells = new List<DataGridViewCell>();
                        foreach (DataGridViewCell dataGridViewCelll in dataGridView.SelectedCells) dataGridCells.Add(dataGridViewCelll);
                        dataGridView.CurrentCell = dataGridView[e.ColumnIndex, e.RowIndex];
                        foreach (DataGridViewCell dataGridViewCell in dataGridCells) dataGridViewCell.Selected = true; //dataGridView[dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex].Selected = true;
                        dataGridView.ResumeLayout();
                    }
                }
            }
        }
        #endregion

        #endregion 

        #region DataGridView Handling 

        #region DataGridView Handling - Constructor
        

        #region DataGridViewInit
        public static void DataGridViewInit(DataGridView dataGridView, bool allowUserToAddRows)
        {
            //Increase speed
            typeof(DataGridView).InvokeMember(
                   "DoubleBuffered",
                   BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                   null,
                   dataGridView,
                   new object[] { true });

            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;

            dataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridView.AllowUserToResizeColumns = true;
            dataGridView.AllowUserToResizeRows = true;
            dataGridView.ShowCellErrors = false;
            dataGridView.ShowRowErrors = false;
            dataGridView.ShowCellToolTips = true;

            dataGridView.AllowUserToAddRows = allowUserToAddRows;
        }
        #endregion

        #region DataGridViewInitGenericData
        private static void DataGridViewInitGenericData(
            DataGridView dataGridView, KryptonCustomPaletteBase palette, string dataGridViewName, string topLeftHeaderCellName, DataGridViewSize cellSize, 
            List<ColumnNameAndWidth> columnNameAndWidthsLarge, List<ColumnNameAndWidth> columnNameAndWidthsMedium, List<ColumnNameAndWidth> columnNameAndWidthsSmall)
        {
            DataGridViewGenericData dataGridViewGenericData = new DataGridViewGenericData();
            dataGridViewGenericData.KryptonCustomPaletteBase = palette;
            dataGridViewGenericData.TopCellName = topLeftHeaderCellName;
            dataGridViewGenericData.DataGridViewName = dataGridViewName;
            dataGridViewGenericData.FavoriteList = FavouriteRead(CreateFavoriteFilename(dataGridViewGenericData.DataGridViewName));
            dataGridViewGenericData.CellSize = cellSize;
            dataGridViewGenericData.ColumnNameAndWidthsLarge = columnNameAndWidthsLarge;
            dataGridViewGenericData.ColumnNameAndWidthsMedium = columnNameAndWidthsMedium;
            dataGridViewGenericData.ColumnNameAndWidthsSmall = columnNameAndWidthsSmall;

            dataGridViewGenericData.CellHeight = CalculateCellHeightBaseOnFont(dataGridView); 
            dataGridView.TopLeftHeaderCell.Value = topLeftHeaderCellName;
            dataGridView.TopLeftHeaderCell.Tag = dataGridViewGenericData;
        }
        #endregion

        #region DataGridViewInitEvents
        private void DataGridViewInitEvents(DataGridView dataGridView)
        {
            dataGridView.CellValueNeeded += DataGridView_CellValueNeeded;
            dataGridView.CellValuePushed += DataGridView_CellValuePushed;
            dataGridView.NewRowNeeded += DataGridView_NewRowNeeded;
            dataGridView.RowValidated += DataGridView_RowValidated;
            dataGridView.RowDirtyStateNeeded += DataGridView_RowDirtyStateNeeded;
            dataGridView.CancelRowEdit += DataGridView_CancelRowEdit;
            dataGridView.UserDeletingRow += DataGridView_UserDeletingRow;
            dataGridView.CellMouseDown += DataGridView_CellMouseDown;
            dataGridView.CurrentCellDirtyStateChanged += DataGridView_CurrentCellDirtyStateChanged;
            dataGridView.KeyDown += DataGridView_KeyDown;
        }
        #endregion

        public DataGridViewHandler(DataGridView dataGridView, KryptonCustomPaletteBase palette, string dataGridViewName, string topLeftHeaderCellName,
            DataGridViewSize cellSize, bool allowUserToAddRow) : this
            (dataGridView, palette, dataGridViewName, topLeftHeaderCellName, cellSize, null, null, null, allowUserToAddRow) {
        }

        public DataGridViewHandler(DataGridView dataGridView, KryptonCustomPaletteBase palette, string dataGridViewName, string topLeftHeaderCellName, 
            DataGridViewSize cellSize, List<ColumnNameAndWidth> columnNameAndWidthsLarge, List<ColumnNameAndWidth> columnNameAndWidthsMedium, 
            List<ColumnNameAndWidth> columnNameAndWidthsSmall, bool allowUserToAddRow)
        {
            this.dataGridView = dataGridView;

            DataGridViewInit(dataGridView, allowUserToAddRow);
            DataGridViewInitGenericData(dataGridView, palette, dataGridViewName, topLeftHeaderCellName, cellSize,
                columnNameAndWidthsLarge, columnNameAndWidthsMedium, columnNameAndWidthsSmall);
            DataGridViewInitEvents(dataGridView);
        }

        
        #endregion

        #region DataGridView Handling - Clear
        public static void Clear(DataGridView dataGridView, DataGridViewSize cellSize)
        {
            DataGridViewGenericData dataGridViewGenericData = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;

            ClipboardUtility.Clear(dataGridView);

            dataGridView.DefaultCellStyle.BackColor = ColorBackCellNormal(dataGridView);
            dataGridView.DefaultCellStyle.ForeColor = ColorTextCellNormal(dataGridView);

            //dataGridViewGenericData.CellHeight = (dataGridView.DefaultCellStyle.Font.Height <= 24 ? 24 : dataGridView.DefaultCellStyle.Font.Height + 2);
            dataGridViewGenericData.CellHeight = CalculateCellHeightBaseOnFont(dataGridView);

            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.TopLeftHeaderCell.Value = dataGridViewGenericData.TopCellName;
            dataGridView.EnableHeadersVisualStyles = false;

            dataGridView.ColumnHeadersHeight = GetTopColumnHeaderHeigth(cellSize, dataGridViewGenericData.CellHeight);
            dataGridView.RowHeadersWidth = GetFirstRowHeaderWidth(cellSize);
            dataGridView.RowTemplate.Height = dataGridViewGenericData.CellHeight;
            dataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        }
        #endregion

        #region DataGridView Handling - AllowUserToAddRows
        public static void SetDataGridViewAllowUserToAddRows(DataGridView dataGridView, bool allowUserToAddRows)
        {
            dataGridView.AllowUserToAddRows = allowUserToAddRows;
        }
        #endregion

        #region DataGridView Handling - GetTopColumnHeaderHeigth
        //DataGridView Size for Column and Row Header, Row / Column size and resize 
        public static int GetTopColumnHeaderHeigth(DataGridViewSize size, int cellHight)
        {
            switch (size)
            {
                case DataGridViewSize.Small:
                    return 100;
                case DataGridViewSize.Medium:
                    return 200;
                case DataGridViewSize.Large:
                    return 300;
                case DataGridViewSize.Small | DataGridViewSize.RenameConvertAndMergeSize:
                    return cellHight; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    return cellHight; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    return cellHight; //Rename Grid*/
                case DataGridViewSize.ConfigSize:
                    return cellHight;
                default:
                    throw new Exception("Not implemented");
            }
        }
        #endregion

        #region DataGridView Handling - GetCellHeigth
        public static int GetCellHeigth(DataGridViewSize size, int cellHight)
        {
            switch (size)
            {
                case DataGridViewSize.Small:
                    return 100;
                case DataGridViewSize.Large:
                    return 200;
                case DataGridViewSize.Medium:
                    return 200;
                case DataGridViewSize.Small | DataGridViewSize.RenameConvertAndMergeSize:
                    return cellHight; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    return cellHight; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    return cellHight; //Rename Grid*/
                case DataGridViewSize.ConfigSize:
                    return cellHight; 
                default:
                    throw new Exception("Not implemented");
            }
        }
        #endregion

        #region DataGridView Handling - GetFirstRowHeaderWidth
        public static int GetFirstRowHeaderWidth(DataGridViewSize size)
        {
            switch (size)
            {
                case DataGridViewSize.Small:
                    return 230;
                case DataGridViewSize.Medium:
                    return 230;
                case DataGridViewSize.Large:
                    return 230;

                case DataGridViewSize.Small | DataGridViewSize.RenameConvertAndMergeSize:
                    return 230; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    return 400; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    return 600; //Rename Grid*/

                case DataGridViewSize.ConfigSize:
                    return 400;
                default:
                    throw new Exception("Not implemented");
            }
        }
        #endregion

        #region Populating handling - GetColumnNameAndWidths
        public static List<ColumnNameAndWidth> GetColumnNameAndWidths(DataGridView dataGridView, DataGridViewSize dataGridViewSize)
        {
            switch (dataGridViewSize)
            {
                case DataGridViewSize.Small:
                case DataGridViewSize.Small | DataGridViewSize.RenameConvertAndMergeSize:
                    return GetDataGridViewGenericData(dataGridView)?.ColumnNameAndWidthsSmall;
                case DataGridViewSize.Medium:
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    return GetDataGridViewGenericData(dataGridView)?.ColumnNameAndWidthsMedium;
                case DataGridViewSize.Large:
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    return GetDataGridViewGenericData(dataGridView)?.ColumnNameAndWidthsLarge;
                case DataGridViewSize.ConfigSize:
                    return null;
                default:
                    throw new Exception("Not implemented");
            }
        }
        #endregion

        #region DataGridView Handling - GetCellColumnsWidth
        public static int GetCellColumnsWidth(DataGridView dataGridView, string columnName)
        {
            List<ColumnNameAndWidth> columnNameAndWidths = GetColumnNameAndWidths(dataGridView, GetDataGridSizeLargeMediumSmall(dataGridView));            
            return ColumnNamesAndWidthHandler.GetColumnWidth(columnNameAndWidths, columnName, GetCellWidth(GetDataGridSizeLargeMediumSmall(dataGridView))); 
        }
        #endregion

        #region DataGridView Handling - UpdatedCacheColumnsWidth
        public static void UpdatedCacheColumnsWidth(DataGridView dataGridView)
        {
            List<ColumnNameAndWidth> columnNameAndWidths = new List<ColumnNameAndWidth>();
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++) 
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericColumn != null)
                {
                    columnNameAndWidths.Add(new ColumnNameAndWidth(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath, dataGridView.Columns[columnIndex].Width));
                }
            }

            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            DataGridViewSize dataGridViewSize = GetDataGridSizeLargeMediumSmall(dataGridView);

            switch (dataGridViewSize)
            {
                case DataGridViewSize.Small:
                case DataGridViewSize.Small | DataGridViewSize.RenameConvertAndMergeSize:
                    dataGridViewGenericData.ColumnNameAndWidthsSmall = columnNameAndWidths;
                    break;
                case DataGridViewSize.Medium:
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    dataGridViewGenericData.ColumnNameAndWidthsMedium = columnNameAndWidths;
                    break;
                case DataGridViewSize.Large:
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    dataGridViewGenericData.ColumnNameAndWidthsLarge = columnNameAndWidths;
                    break;
                case DataGridViewSize.ConfigSize:
                    break;
                default:
                    throw new Exception("Not implemented");
            }

        }
        #endregion

        #region DataGridView Handling - GetCellWidth
        public static int GetCellWidth(DataGridViewSize size)
        {
            switch (size)
            {
                case DataGridViewSize.Small:
                    return 130;
                case DataGridViewSize.Medium:
                    return 230;
                case DataGridViewSize.Large:
                    return 330;

                case DataGridViewSize.Small | DataGridViewSize.RenameConvertAndMergeSize:
                    return 200; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    return 400; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    return 600; //Rename Grid

                case DataGridViewSize.ConfigSize:
                    return 200;
                default:
                    throw new Exception("Not implemented");
            }
        }
        #endregion

        #region DataGridView Handling - GetDataGridSizeLargeMediumSmall
        public static DataGridViewSize GetDataGridSizeLargeMediumSmall(DataGridView dataGridView)
        {
            return GetDataGridViewGenericData(dataGridView)?.CellSize == null ? DataGridViewSize.Medium : GetDataGridViewGenericData(dataGridView).CellSize;
        }
        #endregion

        private static int CalculateCellHeightBaseOnFont(DataGridView dataGridView) 
        {
            Font font = dataGridView.DefaultCellStyle.Font;
            
            // 18.398438 = 16.0 * 2355 / 2048
            float lineSpacing = font.FontFamily.GetLineSpacing(font.Style);
            int lineSpacingPixel = (int)(font.Size * (lineSpacing / font.FontFamily.GetEmHeight(font.Style)));

            ////infoString = "The line spacing is " + lineSpacing + " design units, " +   lineSpacingPixel + " pixels.";


            //int ascent = font.FontFamily.GetCellAscent(font.Style);

            //// 14.484375 = 16.0 * 1854 / 2048
            //int ascentPixel = (int)(font.Size * ascent / font.FontFamily.GetEmHeight(font.Style));

            //int descent = font.FontFamily.GetCellDescent(font.Style);

            //// 3.390625 = 16.0 * 434 / 2048
            //int descentPixel =
            //   (int)(font.Size * descent / font.FontFamily.GetEmHeight(font.Style));

            float deviceDpi = 96;
            Graphics g = dataGridView.Parent.CreateGraphics();
            try {
                deviceDpi = g.DpiX;
            }
            finally {
                g.Dispose();
            }

            //int deviceDpi = (int)(dx / 96);

            int cellHeight = (int)((lineSpacingPixel + 9) * (deviceDpi / 96));
            return (cellHeight <= 24 ? 24 : cellHeight);
        }

        #region DataGridView Handling - GetCellRowHeight
        public static int GetCellRowHeight(DataGridView dataGridView)
        {
            DataGridViewGenericData dataGridViewGenericData = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;            
            return GetCellHeigth(GetDataGridSizeLargeMediumSmall(dataGridView), (dataGridViewGenericData == null ? 
                CalculateCellHeightBaseOnFont(dataGridView) : dataGridViewGenericData.CellHeight));
        }
        #endregion

        #region DataGridView Handling - SetCellRowHeight
        public static void SetCellRowHeight(DataGridView dataGridView, int rowIndex, int height)
        {
            dataGridView.Rows[rowIndex].Height = height;
        }
        #endregion

        #region DataGridView Handling - Set/ShowFavouriteColumns / Set/HideEqualColumns 
        public static bool ShowFavouriteColumns(DataGridView dataGridView)
        {
            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return false;
            return dataGridViewGenericData.ShowFavouriteColumns;
        }

        public static bool HideEqualColumns(DataGridView dataGridView)
        {
            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return false;
            return dataGridViewGenericData.HideEqualColumns;
        }

        public static void SetShowFavouriteColumns(DataGridView dataGridView, bool showFavouriteColumns)
        {
            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return;
            dataGridViewGenericData.ShowFavouriteColumns = showFavouriteColumns;
        }

        public static void SetHideEqualColumns(DataGridView dataGridView, bool hideEqualColumns)
        {
            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return;
            dataGridViewGenericData.HideEqualColumns = hideEqualColumns;
        }
        #endregion 

        #region DataGridView Handling - SetCellSize
        public static void SetCellSize(DataGridView dataGridView, DataGridViewSize cellSize, bool changeCellRowsHeight)
        {
            if (DataGridViewHandler.GetIsPopulationgCellSize(dataGridView)) return;

            DataGridViewHandler.SetIsPopulationgCellSize(dataGridView, true);
            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return;
            dataGridViewGenericData.CellSize = cellSize;

            dataGridView.ColumnHeadersHeight = GetTopColumnHeaderHeigth(cellSize, dataGridViewGenericData.CellHeight);
            dataGridView.RowHeadersWidth = GetFirstRowHeaderWidth(cellSize);

            if (changeCellRowsHeight)
            {
                for (int rowIndex = 1; rowIndex < dataGridView.RowCount; rowIndex++) dataGridView.Rows[rowIndex].Height = GetCellHeigth(cellSize, dataGridViewGenericData.CellHeight);
            }
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++) dataGridView.Columns[columnIndex].Width = 
                    GetCellColumnsWidth(dataGridView, GetColumnDataGridViewGenericColumn(dataGridView, columnIndex)?.FileEntryAttribute?.FileFullPath);
            DataGridViewHandler.SetIsPopulationgCellSize(dataGridView, false);
        }
        #endregion

        #endregion

        #region Suspend and Resume layout

        #region Suspend and Resume layout - Local variables
        private static DataGridViewAutoSizeRowsMode dataGridViewAutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        private static DataGridViewAutoSizeColumnsMode dataGridViewAutoSizeColumnMode = DataGridViewAutoSizeColumnsMode.None;
        private static DataGridViewRowHeadersWidthSizeMode dataGridViewRowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        private static DataGridViewColumnHeadersHeightSizeMode dataGridViewColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        // *** API Declarations ***
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;
        #endregion 

        #region Suspend and Resume layout - SuspendLayout
        public static void SuspendLayoutSetDelay(DataGridView dataGridView, bool doesColumnFilenameExist)
        {
        }
        #endregion 

        #region Suspend and Resume layout - ResumeLayout
        public static bool ResumeLayoutDelayed(DataGridView dataGridView)
        {
            
            return true;
        }
        #endregion

        #region Focus
        public static void Focus(DataGridView dataGridView)
        {
            dataGridView.Focus();
        }
        #endregion 

        #endregion

        #region Populating handling

        #region Populating handling - IsPopulating
        public bool IsPopulating
        {
            get { return GetIsPopulating(dataGridView); }
            set { SetIsPopulating(dataGridView, value); }
        }
        #endregion

        #region Populating handling - GetIsPopulating
        public static bool GetIsPopulating(DataGridView dataGridView)
        {
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulating;
        }
        #endregion

        #region Populating handling - SetIsPopulating
        public static void SetIsPopulating(DataGridView dataGridView, bool isPopulating)
        {
            ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulating = isPopulating;
        }
        #endregion

        #region Populating handling - GetIsPopulating
        public static bool GetIsPopulationgCellSize(DataGridView dataGridView)
        {
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulationgCellSize;
        }
        #endregion

        #region Populating handling - SetIsPopulating
        public static void SetIsPopulationgCellSize(DataGridView dataGridView, bool isPopulationgCellSize)
        {
            ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulationgCellSize = isPopulationgCellSize;
        }
        #endregion

        #region Populating handling - IsPopulatingFile
        public bool IsPopulatingFile
        {
            get { return GetIsPopulatingFile(dataGridView); }
            set { SetIsPopulatingFile(dataGridView, value); }
        }
        #endregion

        #region Populating handling - GetIsPopulatingFile
        public static bool GetIsPopulatingFile(DataGridView dataGridView)
        {
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulatingFile;
        }
        #endregion

        #region Populating handling - SetIsPopulatingFile
        public static void SetIsPopulatingFile(DataGridView dataGridView, bool isPopulatingFile)
        {
            ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulatingFile = isPopulatingFile;
        }
        #endregion

        #region Populating handling - IsPopulatingImage
        public bool IsPopulatingImage
        {
            get { return GetIsPopulatingImage(dataGridView); }
            set { SetIsPopulatingImage(dataGridView, value); }
        }
        #endregion

        #region Populating handling - GetIsPopulatingImage
        public static bool GetIsPopulatingImage(DataGridView dataGridView)
        {
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulatingImage;
        }
        #endregion

        #region Populating handling - SetIsPopulatingImage
        public static void SetIsPopulatingImage(DataGridView dataGridView, bool isPopulatingImage)
        {
            ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulatingImage = isPopulatingImage;
        }
        #endregion

        #endregion

        #region Agregate handling

        #region Agregate handling - IsAgregated
        public bool IsAgregated
        {
            get { return GetIsAgregated(dataGridView); }
            set { SetIsAgregated(dataGridView, value); }
        }
        #endregion

        #region Agregate handling - GetIsAgregated
        public static bool GetIsAgregated(DataGridView dataGridView)
        {
            if (dataGridView.TopLeftHeaderCell.Tag == null) return false;
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsAgregated;
        }
        #endregion

        #region Agregate handling - SetIsAgregated
        public static void SetIsAgregated(DataGridView dataGridView, bool isAgregated)
        {
            ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsAgregated = isAgregated;
        }
        #endregion

        #endregion
       
        #region Action Handling - Find And Replace 
        
        #region Action Handling - ActionFindAndReplace
        static FindAndReplaceForm m_FindAndReplaceForm;

        public static void ActionFindAndReplace(DataGridView dataGridView, bool replaceTab)
        {
            if (dataGridView == null) return;
            if (dataGridView.ColumnCount == 0 || dataGridView.RowCount == 0) return;
            if (m_FindAndReplaceForm == null)
            {
                m_FindAndReplaceForm = new FindAndReplaceForm(dataGridView, replaceTab);
                m_FindAndReplaceForm.FormClosed += FindAndReplaceForm_FormClosed; //new FormClosedEventHandler(FindAndReplaceForm_FormClosed);
                m_FindAndReplaceForm.Show();
            }
            else
            {
                m_FindAndReplaceForm.InitializeForm(dataGridView, replaceTab);
                m_FindAndReplaceForm.BringToFront();
            }
        }
        #endregion

        #region Action Handling - ActionFindAndReplace- BringToFront
        public static void BringToFrontFindAndReplace()
        {
            if (m_FindAndReplaceForm != null)
            {
                m_FindAndReplaceForm.BringToFront();
            }
        }
        #endregion

        #region Action Handling - ActionFindAndReplace - Close
        private static void FindAndReplaceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_FindAndReplaceForm = null;
        }
        #endregion 

        #endregion 
        
        #region DataGridViewGenericData handling

        #region DataGridViewGenericData handling - GetDataGridViewGenericData
        public static DataGridViewGenericData GetDataGridViewGenericData(DataGridView dataGridView)
        {
            if (dataGridView.TopLeftHeaderCell.Tag == null)
            {
                //This should never happen
                return null;
            }
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return null;
            return (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
        }
        #endregion

        #region DataGridViewGenericData handling - GetDataGridViewName
        public static string GetDataGridViewName(DataGridView dataGridView)
        {
            return GetDataGridViewGenericData(dataGridView)?.DataGridViewName;
        }
        #endregion

        #endregion

        #region Column handling 

        #region Column handling - GetSelectFileEntriesMode
        public enum GetSelectFileEntriesMode
        {
            None,
            Columns,
            Rows
        }
        #endregion

        #region Column handling - GetSelectFileEntries
        public static HashSet<FileEntry> GetSelectFileEntries(DataGridView dataGridView, GetSelectFileEntriesMode mode)
        {
            HashSet<FileEntry> files = new HashSet<FileEntry>();
            if (dataGridView != null)
            {
                switch (mode)
                {
                    case GetSelectFileEntriesMode.Columns:
                        foreach (int columnIndex in DataGridViewHandler.GetColumnSelected(dataGridView))
                        {
                            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                            if (dataGridViewGenericColumn != null && !files.Contains(dataGridViewGenericColumn.FileEntryAttribute.FileEntry)) files.Add(dataGridViewGenericColumn.FileEntryAttribute.FileEntry);
                        }
                        break;
                    case GetSelectFileEntriesMode.Rows:
                        foreach (int rowIndex in DataGridViewHandler.GetRowSelected(dataGridView))
                        {
                            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                            FileEntry fileEntry = dataGridViewGenericRow?.FileEntryAttribute?.FileEntry;
                            if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader && !files.Contains(fileEntry)) files.Add(fileEntry);
                        }
                        break;
                }
            }
            return files;
        }
        #endregion

        #region Column handling - SetColumnDirtyFlag
        public static void SetColumnDirtyFlag(DataGridView dataGridView, int columnIndex, bool isDirty, string diffrences = null)
        {
            if (columnIndex == -1) return;
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn != null)
            {
                dataGridViewGenericColumn.IsDirty = isDirty;
                dataGridView.Columns[columnIndex].ToolTipText = diffrences;
                if (!isDirty) dataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false;
            }
            InvalidateCellColumnHeader(dataGridView, columnIndex);
        }
        #endregion

        #region Column handling - IsColumnDirty
        public static bool IsColumnDirty(DataGridView dataGridView, int columnIndex)
        {
            if (!GetIsAgregated(dataGridView))
                return false; //Should not occure
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null) return false;
            return dataGridViewGenericColumn.IsDirty;
        }
        #endregion

        #region Column handling - IsColumnPopulated
        public static bool IsColumnPopulated(DataGridView dataGridView, int columnIndex)
        {
            if (!GetIsAgregated(dataGridView)) return false; 
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null) return false;
            return dataGridViewGenericColumn.IsPopulated;
        }
        #endregion

        #region Column handling - SetColumnPopulatedFlag
        public static void SetColumnPopulatedFlag(DataGridView dataGridView, int columnIndex, bool newFlag)
        {
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null) return;
            dataGridViewGenericColumn.IsPopulated = newFlag;
        }
        #endregion 

        #region Column handling - GetColumnSelected
        public static List<int> GetColumnSelected(DataGridView dataGridView)
        {
            List<int> selectedColumns = new List<int>();

            if (GetIsAgregated(dataGridView))
            {
                foreach (DataGridViewColumn dataGridViewColumn in dataGridView.SelectedColumns)
                {
                    if (!selectedColumns.Contains(dataGridViewColumn.Index)) selectedColumns.Add(dataGridViewColumn.Index);
                }

                foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
                {
                    if (!selectedColumns.Contains(dataGridViewCell.ColumnIndex)) selectedColumns.Add(dataGridViewCell.ColumnIndex);
                }
            }
            return selectedColumns;
        }
        #endregion

        #region Column handling - SelectColumnRows
        public static void SelectColumnRows(DataGridView dataGridView, int selectColumnIndex, bool selected = true)
        {
            for (int rowIndex = 0; rowIndex < dataGridView.RowCount; rowIndex++) dataGridView[selectColumnIndex, rowIndex].Selected = selected;
        }
        #endregion

        #region Column handling - IsColumnSelected
        public static bool IsColumnSelected(DataGridView dataGridView, int columnIndex)
        {
            if (!GetIsAgregated(dataGridView))
                return false; //Should not occure
            foreach (DataGridViewColumn dataGridViewColumn in dataGridView.SelectedColumns)
            {
                if (dataGridViewColumn.Index == columnIndex) return true;
            }

            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                if (dataGridViewCell.ColumnIndex == columnIndex) return true;
            }
            return false;
        }
        #endregion

        #region Column handling - SetColumnHeaderThumbnail
        public static void SetColumnHeaderThumbnail(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, Image image)
        {
            if (!GetIsAgregated(dataGridView)) return; 

            DataGridViewHandler.SetIsPopulatingImage(dataGridView, true);

            int columnIndex = GetColumnIndexPriorities(dataGridView, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompare);
            if (columnIndex >= 0)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                dataGridViewGenericColumn.Thumbnail = new Bitmap(image);
                dataGridView.InvalidateCell(columnIndex, -1);
            }
            DataGridViewHandler.SetIsPopulatingImage(dataGridView, false);
        }
        #endregion

        #region Column handling - SetColumnHeaderMetadata
        public static void SetColumnHeaderMetadata(DataGridView dataGridView, Metadata newMetadata, int columnIndex)
        {
            if (!GetIsAgregated(dataGridView))
                return; //Should not occure

            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.IsPopulated &&
                IsFilenameEqual(dataGridViewGenericColumn.FileEntryAttribute.FileFullPath, newMetadata.FileEntry.FileFullPath))
            {
                dataGridViewGenericColumn.Metadata = new Metadata(newMetadata);
            } else
            {
                //DEBUG - Why has columns changed? If this happen debug
            }
        }
        #endregion 

        #region Column handling - GetColumnCount
        public static int GetColumnCount(DataGridView dataGridView)
        {
            if (!GetIsAgregated(dataGridView)) return 0; 
            return dataGridView.ColumnCount;
        }
        #endregion

        #region Column handling - DoesColumnFilenameExist
        public static bool DoesColumnFilenameExist(DataGridView dataGridView, string fullFilePath)
        {
            if (!GetIsAgregated(dataGridView))
                return false; //Should not occure
            return GetColumnIndexFirstFullFilePath(dataGridView, fullFilePath, false) != -1;
        }
        #endregion

        #region Column handling - GetColumnIndexFirst - fullFilePath        
        public static int GetColumnIndexFirstFullFilePath(DataGridView dataGridView, string fullFilePath, bool checkIsAgregated)
        {
            if (checkIsAgregated && !GetIsAgregated(dataGridView))
                return -1; 
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                DataGridViewGenericColumn column = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (column != null && IsFilenameEqual(column.FileEntryAttribute.FileFullPath, fullFilePath))
                {
                    return columnIndex;
                }
            }
            return -1; //Not found
        }
        #endregion

        #region Column handling - GetColumnIndex - FileEntryAttribute    
        private static Dictionary<FileEntryAttribute, int> columnIndexCache = new Dictionary<FileEntryAttribute, int>();

        public static int GetColumnIndexUserInput(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute)
        {
            if (!GetIsAgregated(dataGridView)) 
                return -1;
            FileEntryAttribute fileEntryAttributeSearch = new FileEntryAttribute(fileEntryAttribute.FileEntry, 
                FileEntryVersionHandler.ConvertCurrentErrorHistorical(fileEntryAttribute.FileEntryVersion));

            #region Cache logic
            if (columnIndexCache.TryGetValue(fileEntryAttributeSearch, out int columnIndexFound))
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndexFound);
                if (columnIndexFound < dataGridView.ColumnCount && dataGridViewGenericColumn != null)
                {
                    FileEntryAttribute fileEntryAttributeCompare = new FileEntryAttribute(dataGridViewGenericColumn.FileEntryAttribute.FileEntry,
                        FileEntryVersionHandler.ConvertCurrentErrorHistorical(dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion));
                    if (fileEntryAttributeCompare == fileEntryAttributeSearch) 
                        return columnIndexFound; //It's still same file in column, (means not column has been added in front)
                    else
                        columnIndexCache.Clear(); //It's a diffrent file, means a column has been added in front, need rebuid cache
                }
                 
            }
            #endregion

            #region Find column index
            if (FileEntryVersionHandler.IsCurrenOrUpdatedVersion(fileEntryAttributeSearch.FileEntryVersion))
            {
                for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
                {
                    DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    
                    FileEntryAttribute fileEntryAttributeCache = new FileEntryAttribute(dataGridViewGenericColumn.FileEntryAttribute.FileEntry,
                            FileEntryVersionHandler.ConvertCurrentErrorHistorical(dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion));
                    if (!columnIndexCache.ContainsKey(fileEntryAttributeCache)) columnIndexCache.Add(fileEntryAttributeCache, columnIndex);

                    if (dataGridViewGenericColumn != null)
                    {
                        switch (fileEntryAttribute.FileEntryVersion)
                        {
                            case FileEntryVersion.ExtractedNowUsingExiftool: //is used in in DataGridView Column
                            case FileEntryVersion.ExtractedNowUsingExiftoolTimeout:
                            case FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist:
                            case FileEntryVersion.ExtractedNowUsingExiftoolWithError:

                            case FileEntryVersion.ExtractedNowUsingReadMediaFile:
                            case FileEntryVersion.ExtractedNowUsingWindowsLivePhotoGallery:
                            case FileEntryVersion.ExtractedNowUsingMicrosoftPhotos:
                            case FileEntryVersion.ExtractedNowUsingWebScraping:
                            case FileEntryVersion.CompatibilityFixedAndAutoUpdated:
                            case FileEntryVersion.MetadataToSave:

                            case FileEntryVersion.CurrentVersionInDatabase:
                                if (FileEntryVersionHandler.IsCurrenOrUpdatedVersion(dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion) &&
                                    IsFilenameEqual(fileEntryAttribute.FileFullPath, dataGridViewGenericColumn.FileEntryAttribute.FileFullPath) &&
                                    dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite //Is Edit Column (User Input Column)
                                    )
                                    //fileEntryAttribute.LastWriteDateTime == dataGridViewGenericColumn.FileEntryAttribute.LastWriteDateTime)
                                    return columnIndex;                                
                                break;
                            case FileEntryVersion.Error:
                            case FileEntryVersion.Historical:
                                if (dataGridViewGenericColumn.FileEntryAttribute == fileEntryAttributeSearch) return columnIndex;                                
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }
            #endregion

            return -1; //Not found
        }

        private static Dictionary<FileEntryAttribute, int> columnIndexCachePriorities = new Dictionary<FileEntryAttribute, int>();
        private static object columnIndexCachePrioritiesLock = new object();
        public static int GetColumnIndexPriorities(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute2, out FileEntryVersionCompare fileEntryVersionPriorityReason, bool checkIsAgregated = true)
        {
            if (!checkIsAgregated || GetIsAgregated(dataGridView)) //Don't check checkIsAgregated when create new columns
            {
                FileEntryAttribute fileEntryAttributeSearch = new FileEntryAttribute(fileEntryAttribute2.FileEntry,
                    FileEntryVersionHandler.ConvertCurrentErrorHistorical(fileEntryAttribute2.FileEntryVersion));


                int startColumn = 0;
                #region Cache logic
                lock (columnIndexCachePrioritiesLock)
                {
                    if (columnIndexCachePriorities.TryGetValue(fileEntryAttributeSearch, out int columnIndex))
                    { 
                        DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                        if (dataGridViewGenericColumn != null)
                        {
                            FileEntryAttribute fileEntryAttributeCompare = new FileEntryAttribute(dataGridViewGenericColumn.FileEntryAttribute.FileEntry,
                            FileEntryVersionHandler.ConvertCurrentErrorHistorical(dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion));
                            if (fileEntryAttributeCompare == fileEntryAttributeSearch)
                                startColumn = columnIndex; //It's still same file in column, (means not column has been added in front)
                            else
                                columnIndexCachePriorities.Clear(); //It's a diffrent file, means a column has been added in front, need rebuid cache
                        }
                    }
                }
                #endregion


                for (int columnIndex = startColumn; columnIndex < dataGridView.ColumnCount; columnIndex++)
                {
                    DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    if (dataGridViewGenericColumn != null)
                    {
                        FileEntryAttribute fileEntryAttributeCache = new FileEntryAttribute(dataGridViewGenericColumn.FileEntryAttribute.FileEntry,
                            FileEntryVersionHandler.ConvertCurrentErrorHistorical(dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion));

                        lock (columnIndexCachePrioritiesLock)
                        {
                            if (!columnIndexCachePriorities.ContainsKey(fileEntryAttributeCache)) columnIndexCachePriorities.Add(fileEntryAttributeCache, columnIndex);
                        }

                        fileEntryVersionPriorityReason = FileEntryVersionHandler.CompareFileEntryAttribute(dataGridViewGenericColumn.FileEntryAttribute, fileEntryAttribute2);
                        switch (fileEntryVersionPriorityReason)
                        {
                            case FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing:
                                break; //Continue search
                            case FileEntryVersionCompare.LostWasOlder_Updated_Nothing:
                                //break; //Continue search
                                return columnIndex;
                            case FileEntryVersionCompare.Update_Status_Metadata_WriteFailed:
                            case FileEntryVersionCompare.Update_Status_FileNotFound:
                            case FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView:
                                return columnIndex;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }
            else
            {
                //DEBUG
            }
            fileEntryVersionPriorityReason = FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing;
            return -1; //Not found
        }

        public static int GetColumnIndexWhenAddColumn(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompareReason)
        {
            return GetColumnIndexPriorities(dataGridView, fileEntryAttribute, out fileEntryVersionCompareReason, false); 
        }
        #endregion

        #region Column handling - GetColumnDataGridViewGenericColumnList
        public static List<DataGridViewGenericColumn> GetColumnsDataGridViewGenericColumnCurrentOrAutoCorrect(DataGridView dataGridView, bool onlyReadWriteAccessColumn)
        {
            List<DataGridViewGenericColumn> dataGridViewGenericColumnList = new List<DataGridViewGenericColumn>();

            if (GetIsAgregated(dataGridView))
            {
                for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
                {
                    DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                    if (dataGridViewGenericColumn != null)
                    {
                        if (FileEntryVersionHandler.IsCurrenOrUpdatedVersion(dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion))
                        {
                            if (!onlyReadWriteAccessColumn || (onlyReadWriteAccessColumn && dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)) //Check if loaded
                                dataGridViewGenericColumnList.Add(dataGridViewGenericColumn);
                        }
                    }

                }
            }
            return dataGridViewGenericColumnList;
        }
        #endregion

        #region Column handling - AddColumnOrUpdate 
        public static int AddColumnOrUpdateNew(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, Image thumbnail, Metadata metadata,
            ReadWriteAccess readWriteAccessForColumn, ShowWhatColumns showWhatColumns, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault,
            out FileEntryVersionCompare fileEntryVersionCompareReason)
        {
            int columnIndex = GetColumnIndexWhenAddColumn(dataGridView, fileEntryAttribute, out fileEntryVersionCompareReason); //Find column Index for Filename and date last written, Prioritize

            bool isErrorColumn = fileEntryAttribute.FileEntryVersion == FileEntryVersion.Error;
            bool showErrorColumns = ShowWhatColumnHandler.ShowErrorColumns(showWhatColumns);
            bool isHistoryColumn = (fileEntryAttribute.FileEntryVersion == FileEntryVersion.Historical);
            bool showHirstoryColumns = ShowWhatColumnHandler.ShowHirstoryColumns(showWhatColumns);
            bool isCurrenOrUpdatedColumn = FileEntryVersionHandler.IsCurrenOrUpdatedVersion(fileEntryAttribute.FileEntryVersion);
            bool isErrorOrHistoricalColumn = FileEntryVersionHandler.IsErrorOrHistoricalVersion(fileEntryAttribute.FileEntryVersion);

            if (fileEntryVersionCompareReason == FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing) //Column not found, add a new column
            {
                //Do not add columns that is not visible //Check if error column first, can be historical, and error
                if (isErrorColumn && !showErrorColumns) return -1; //FileEntryVersionCompare.LostNoneEqualFound
                if (!showHirstoryColumns && isHistoryColumn) return -1; //FileEntryVersionCompare.LostNoneEqualFound

                #region Create a column - Set default Columns attributes
                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn(new DataGridViewTextBoxCell());
                dataGridViewColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;  //For optimize speed
                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;   //For optimize speed
                dataGridViewColumn.MinimumWidth = 40;                                   //For optimize speed, and layout
                dataGridViewColumn.FillWeight = 0.1f;                                   //Expand limit of 65535 witdth
                dataGridViewColumn.Width = GetCellColumnsWidth(dataGridView, fileEntryAttribute.FileFullPath);          //Layout
                dataGridViewColumn.ToolTipText = fileEntryAttribute.LastWriteDateTime.ToString() + "\r\n" + fileEntryAttribute.FileFullPath;
                dataGridViewColumn.HeaderText = fileEntryAttribute.FileFullPath;
                dataGridViewColumn.Tag = new DataGridViewGenericColumn(fileEntryAttribute, thumbnail, metadata, readWriteAccessForColumn);
                #endregion

                #region Find where to add *NEW* column
                int columnIndexFilename = GetColumnIndexFirstFullFilePath(dataGridView, fileEntryAttribute.FileFullPath, false); //Find first Column with equal name
                if (columnIndexFilename == -1) //Filename doesn't exist
                {
                    #region Filename doesn't exist, add last
                    columnIndex = dataGridView.Columns.Add(dataGridViewColumn); //Filename doesn't exist add to end.
                    #endregion
                }
                else
                {
                    #region Sort, newst always first
                    //No need to check: "Current, Read from Database and AutoCorrect" they are always first
                    if (isErrorOrHistoricalColumn) 
                    {
                        while (columnIndexFilename < dataGridView.Columns.Count &&
                            dataGridView.Columns[columnIndexFilename].Tag is DataGridViewGenericColumn column &&
                            IsFilenameEqual(column.FileEntryAttribute.FileFullPath, fileEntryAttribute.FileFullPath) &&                //Correct filename on column
                            (
                                FileEntryVersionHandler.IsCurrenOrUpdatedVersion(column.FileEntryAttribute.FileEntryVersion) || //Move behind Current, AutoCorrect and Database 
                                column.FileEntryAttribute.LastWriteDateTime > fileEntryAttribute.LastWriteDateTime)     //ALso move behind new dates
                            )
                        {
                            columnIndexFilename += 1;
                        }
                    }
                    #endregion

                    #region Move error and historical version back
                    //No need to check: "Current, Read from Database and AutoCorrect" they are always first
                    if (isErrorOrHistoricalColumn)  //History or Error column added, then find correct postion
                    {
                        if (columnIndexFilename < dataGridView.Columns.Count && dataGridView.Columns[columnIndexFilename].Tag is DataGridViewGenericColumn column2 &&
                            IsFilenameEqual(column2.FileEntryAttribute.FileFullPath, fileEntryAttribute.FileFullPath) &&           //Correct filename on column
                            (
                            FileEntryVersionHandler.IsCurrenOrUpdatedVersion(column2.FileEntryAttribute.FileEntryVersion) || //Edit version, move to next column -> edit always first
                            column2.FileEntryAttribute.LastWriteDateTime > fileEntryAttribute.LastWriteDateTime)    //Is older, move next -> Newst always frist
                            )
                        {
                            columnIndexFilename += 1;
                            fileEntryVersionCompareReason = FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView;

                        }
                    }
                    #endregion

                    columnIndex = columnIndexFilename;

                    #region Check if need create new column
                    bool createNewColumn = true;
                    if (isCurrenOrUpdatedColumn &&
                        columnIndex >= 0 &&  columnIndex < dataGridView.ColumnCount && 
                        dataGridView.Columns[columnIndexFilename].Tag is DataGridViewGenericColumn columnCheck)
                    {
                        if (FileEntryVersionHandler.IsCurrenOrUpdatedVersion(columnCheck.FileEntryAttribute.FileEntryVersion) &&
                            IsFilenameEqual(columnCheck.FileEntryAttribute.FileFullPath, fileEntryAttribute.FileFullPath)) createNewColumn = false;
                    }
                    #endregion

                    if (createNewColumn)
                    {
                        dataGridView.Columns.Insert(columnIndex, dataGridViewColumn);
                        if (isErrorColumn || showErrorColumns) fileEntryVersionCompareReason = FileEntryVersionCompare.CreateColumnHistoricalOrError_CreateColumn;
                    }
                }
                #endregion

                SetCellStatusDefaultColumnWhenAdded(dataGridView, columnIndex, dataGridViewGenericCellStatusDefault);
                SetCellBackgroundColorForColumn(dataGridView, columnIndex);
            }
            else
            {
                DataGridViewGenericColumn currentDataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                #region Updated - When new, No updated when Equal or older
                if (IsColumnDirty(dataGridView, columnIndex)) //That means, data was changed by user and trying to make changes to "past"
                {
                    #region Check if data will overwrite user changes
                    //Check if old file, due to User click "reload metadata", then newest version has become older that current

                    switch (fileEntryVersionCompareReason)
                    {
                        case FileEntryVersionCompare.Update_Status_Metadata_WriteFailed:
                        case FileEntryVersionCompare.Update_Status_FileNotFound:
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = true; //Write failed, data not equal
                            currentDataGridViewGenericColumn.Metadata = metadata; //Write back orginal, due to data was not saved
                            break;
                        case FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView:
                            if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.MetadataToSave)
                            {
                                currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warning needed, expected behaviour
                                currentDataGridViewGenericColumn.Metadata = metadata; //This need to saved, to check if correct read back
                            }
                            else if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.CompatibilityFixedAndAutoUpdated)
                            {
                                currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warning needed, expected behaviour
                                //currentDataGridViewGenericColumn.Metadata = metadata; //Don't updated Metadata with AutoCorrect, needs to know orginal to check for changes
                            }
                            else 
                            { 
                                fileEntryVersionCompareReason = FileEntryVersionCompare.LostOverUserInput_Update_Status;
                                if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.CurrentVersionInDatabase)
                                    //|| fileEntryAttribute.FileEntryVersion == FileEntryVersion.ExtractedNowFromExternalSource ||
                                    // fileEntryAttribute.FileEntryVersion == FileEntryVersion.ExtractedNowFromMediaFile)
                                {
                                    //Example when this will occure. When change tabs and DataGridView was not created, other tabs/DataGridView
                                    //will also get refreshed
                                    currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No need to Warn, same version as in past
                                    currentDataGridViewGenericColumn.Metadata = metadata; //Keep newest version, PS All columns get added with empty Metadata
                                }
                                else
                                {
                                    currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = true; //Warn, new files can't be shown
                                }
                            }
                            break;
                        case FileEntryVersionCompare.LostWasOlder_Updated_Nothing:
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warning needed
                            break;
                        case FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing:
                            break;
                        //case FileEntryVersionCompare.LostOverUserInput:
                        default:
                            throw new NotImplementedException();
                    }                    
                    #endregion 
                }
                else
                {
                    #region Check if need to reload/refresh dataGridView
                    switch (fileEntryVersionCompareReason)
                    {
                        case FileEntryVersionCompare.Update_Status_FileNotFound:
                        case FileEntryVersionCompare.Update_Status_Metadata_WriteFailed:
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = true; //Write failed
                            currentDataGridViewGenericColumn.Metadata = metadata; //Return orginal version, data was not saved, user data will not be overwritten
                            break;

                        case FileEntryVersionCompare.Update_Status_DataGridView_LostOverUserData:
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = true; 
                            currentDataGridViewGenericColumn.Metadata = metadata; 
                            break;

                        case FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView:
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warnings needed, just updated datagrid with new data
                            if (fileEntryAttribute.FileEntryVersion != FileEntryVersion.CompatibilityFixedAndAutoUpdated) //Don't overwrite user data
                                currentDataGridViewGenericColumn.Metadata = metadata; //Keep newest version, PS All columns get added with empty Metadata
                            break;
                            
                        case FileEntryVersionCompare.LostWasOlder_Updated_Nothing:
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warning needed
                            break;
                        case FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing:
                            break;
                        //case FileEntryVersionCompare.LostOverUserInput:
                        default:
                            throw new NotImplementedException();
                    }
                    #endregion
                }
                #endregion

                #region Updated - currentDataGridViewGenericColumn
                switch (fileEntryVersionCompareReason)
                {
                    case FileEntryVersionCompare.Update_Status_FileNotFound:
                        SetCellReadOnlyForColumn(dataGridView, columnIndex, true);
                        currentDataGridViewGenericColumn.FileEntryAttribute = fileEntryAttribute; //Updated from FromSource, Database or AutoCorrect                
                        currentDataGridViewGenericColumn.Thumbnail = (thumbnail == null ? null : new Bitmap(thumbnail)); //Avoid thread issues
                        currentDataGridViewGenericColumn.ReadWriteAccess = readWriteAccessForColumn;
                        dataGridView.Columns[columnIndex].Tag = currentDataGridViewGenericColumn;
                        SetCellBackgroundColorForColumn(dataGridView, columnIndex);
                        break;
                    case FileEntryVersionCompare.Update_Status_DataGridView_LostOverUserData:
                    case FileEntryVersionCompare.Update_Status_Metadata_WriteFailed:
                    case FileEntryVersionCompare.Won_Update_Satuts_Metdata_DataGridView:                    
                        currentDataGridViewGenericColumn.FileEntryAttribute = fileEntryAttribute; //Updated from FromSource, Database or AutoCorrect                
                        currentDataGridViewGenericColumn.Thumbnail = (thumbnail == null ? null : new Bitmap(thumbnail)); //Avoid thread issues
                        currentDataGridViewGenericColumn.ReadWriteAccess = readWriteAccessForColumn;
                        dataGridView.Columns[columnIndex].Tag = currentDataGridViewGenericColumn;
                        //If this needed, updated what's already updated...
//SetCellBackgroundColorForColumn(dataGridView, columnIndex);
                        break;
                    case FileEntryVersionCompare.LostOverUserInput_Update_Status:
                    case FileEntryVersionCompare.LostWasOlder_Updated_Nothing:
                    case FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing:
                        break;
                    default:
                        throw new NotImplementedException();
                }
                #endregion

                #region Hide and show columns, accoring to user config
                if (isErrorColumn) //Check if error column first, can be historical, and error
                {
                    if (showErrorColumns) dataGridView.Columns[columnIndex].Visible = true;
                    else dataGridView.Columns[columnIndex].Visible = false;
                }
                else if (isHistoryColumn)
                {
                    if (showHirstoryColumns) dataGridView.Columns[columnIndex].Visible = true;
                    else dataGridView.Columns[columnIndex].Visible = false;
                }
                else dataGridView.Columns[columnIndex].Visible = true;
                #endregion 
            }
            return columnIndex;
        }
        #endregion

        #region Column Handling - SetColumnVisibleStatus

        public static void SetColumnVisibleStatus(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, bool visible)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridView, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompare);
            if (columnIndex != -1) SetColumnVisibleStatus(dataGridView, columnIndex, visible);
        }

        public static void SetColumnVisibleStatus(DataGridView dataGridView, int columnIndex, bool visible)
        {
            if (dataGridView.Columns[columnIndex].Visible != visible) dataGridView.Columns[columnIndex].Visible = visible;
        }
        #endregion 

        #region Column handling - GetColumnDataGridViewGenericColumn
        public static DataGridViewGenericColumn GetColumnDataGridViewGenericColumn(DataGridView dataGridView, int columnIndex)
        {
            DataGridViewGenericColumn dataGridViewGenericColumn = null;
            try
            {
                if (columnIndex < 0) return null;
                if (columnIndex >= dataGridView.ColumnCount) return null; //This can happen when using cache and switch between tabs
                dataGridViewGenericColumn = dataGridView.Columns[columnIndex].Tag as DataGridViewGenericColumn;
            } catch
            {
            }
            return dataGridViewGenericColumn;
        }
        #endregion

        #region Column handling - GetColumnDataGridViewName
        public static string GetColumnDataGridViewName(DataGridView dataGridView, int columnIndex)
        {
            if (columnIndex < 0) return null;
            if (columnIndex >= dataGridView.ColumnCount) return null; //This can happen when using cache and switch between tabs
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            return dataGridView.Columns[columnIndex].HeaderText;
        }
        #endregion

        #region Column handling - SetColumnDataGridViewName
        public static void SetColumnDataGridViewName(DataGridView dataGridView, int columnIndex, string headerText)
        {
            if (columnIndex < 0) return;
            if (columnIndex >= dataGridView.ColumnCount) return; //This can happen when using cache and switch between tabs
            dataGridView.Columns[columnIndex].HeaderText = headerText;
        }
        #endregion

        #endregion

        #region Rows handling
        #region Rows handling - FastAutoSizeRowsHeight
        public static void FastAutoSizeRowsHeight(DataGridView dataGridView, int queueSize)
        {
            if (queueSize > 0) return;

            // Create a graphics object from the target grid. Used for measuring text size.
            using (var gfx = dataGridView.CreateGraphics())
            {
                for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                {
                    //if ()
                    //string longestString = "";
                    int maxHeight = 0;
                    for (int columnIndex = 0; columnIndex < GetColumnCount(dataGridView); columnIndex++)
                    {
                        var value = dataGridView.Rows[rowIndex].Cells[columnIndex].Value;
                        if (value != null) // && value.ToString().Length > longestString.Length)
                        {//longestString = value.ToString();
                            SizeF size = gfx.MeasureString(value.ToString(), dataGridView.Font, dataGridView.Columns[0].HeaderCell.Size.Width - 4);
                            int height = (int)size.Height + 3;
                            if (height > maxHeight) maxHeight = height;
                        }
                    }

                    if (GetColumnCount(dataGridView) >= 1) //No need to resize if no columns
                    {
                        // Use the graphics object to measure the string size.


                        // If the calculated width is larger than the column header width, set the new column width.
                        if (maxHeight > dataGridView.Rows[rowIndex].Height)
                        {
                            dataGridView.Rows[rowIndex].Height = maxHeight;
                        }
                    }
                }
            }
        }
        #endregion

        #region Rows handling - GetRowSelected
        public static List<int> GetRowSelected(DataGridView dataGridView)
        {
            List<int> selectedRows = new List<int>();

            foreach (DataGridViewRow dataGridViewRow in dataGridView.SelectedRows)
            {
                if (!selectedRows.Contains(dataGridViewRow.Index)) selectedRows.Add(dataGridViewRow.Index);
            }

            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                if (!selectedRows.Contains(cell.RowIndex)) selectedRows.Add(cell.RowIndex);
            }
            return selectedRows;
        }
        #endregion

        #region Rows handling - GetRowDataGridViewGenericRow
        public static DataGridViewGenericRow GetRowDataGridViewGenericRow(DataGridView dataGridView, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > GetRowCount(dataGridView) - 1) return null;
            return dataGridView.Rows[rowIndex].HeaderCell.Tag as DataGridViewGenericRow;
        }
        #endregion

        #region Rows handling - GetRowCountWithoutEditRow
        public static int GetRowCountWithoutEditRow(DataGridView dataGridView)
        {
            if (dataGridView.AllowUserToAddRows) return dataGridView.RowCount == 0 ? 0 /* Meens just been cleared */ : dataGridView.RowCount - 1;
            return dataGridView.RowCount;
        }
        #endregion

        #region Rows handling - GetRowCount
        public static int GetRowCount(DataGridView dataGridView)
        {
            return dataGridView.RowCount;
        }
        #endregion

        #region Rows handling - FindFileEntryRow
        public static int FindFileEntryRow(DataGridView dataGridView, DataGridViewGenericRow dataGridViewGenericRow, int startSearchRow, bool sort, out bool rowFound)
        {
            rowFound = false;
            int lastHeaderRowFound = -1;

            int rowIndexFound = GetRowIndex(dataGridView, dataGridViewGenericRow.RowIndenifier);
            if (rowIndexFound >= startSearchRow)
            {
                rowFound = true;
                return rowIndexFound;
            }


            for (int rowIndex = startSearchRow; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericRowCheck = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRowCheck != null)
                {
                    if (
                       dataGridViewGenericRowCheck.IsHeader &&
                       dataGridViewGenericRow.IsHeader && //It correct header
                       dataGridViewGenericRow.HeaderName == dataGridViewGenericRowCheck.HeaderName
                       )
                    {
                        rowFound = true;
                        return rowIndex;
                    }

                    if (!dataGridViewGenericRowCheck.IsHeader &&
                        !dataGridViewGenericRow.IsHeader && //It correct row
                        dataGridViewGenericRowCheck.HeaderName == dataGridViewGenericRow.HeaderName &&
                        dataGridViewGenericRowCheck.RowName == dataGridViewGenericRow.RowName)
                    {
                        rowFound = true;
                        return rowIndex;
                    }

                    if (sort)
                    {

                        if (dataGridViewGenericRow.IsHeader && //A normal row is add (not header)
                                                               //dataGridViewGenericRowCheck.IsHeader &&  //If header, then check if same header name
                            dataGridViewGenericRow.HeaderName.CompareTo(dataGridViewGenericRowCheck.HeaderName) >= 0)
                            lastHeaderRowFound = rowIndex; //Remember head row found

                        //Add sorted
                        if (!dataGridViewGenericRow.IsHeader && //A normal row is add (not header)
                            dataGridViewGenericRowCheck.IsHeader &&  //If header, then check if same header name
                            dataGridViewGenericRowCheck.HeaderName == dataGridViewGenericRow.HeaderName)
                            lastHeaderRowFound = rowIndex; //Remember head row found

                        if (!dataGridViewGenericRow.IsHeader && //A normal row is add (not header)
                            !dataGridViewGenericRowCheck.IsHeader &&  //If header, then check if same header name
                            dataGridViewGenericRowCheck.HeaderName == dataGridViewGenericRow.HeaderName &&
                            dataGridViewGenericRow.RowName.CompareTo(dataGridViewGenericRowCheck.RowName) >= 0)
                            lastHeaderRowFound = rowIndex; //If lower or eaual, remeber last
                    }
                    else
                    {
                        //Add last
                        if (!dataGridViewGenericRow.IsHeader && //Remmeber last row found with same header name, regardless of header or just value row
                                                                //dataGridViewGenericRowCheck.IsHeader && - No need to check
                            dataGridViewGenericRowCheck.HeaderName == dataGridViewGenericRow.HeaderName
                            )
                            lastHeaderRowFound = rowIndex;
                    }


                }

            }
            rowFound = false;
            return lastHeaderRowFound;
        }
        #endregion

        #region Rows handling - GetRowIndex
        private static Dictionary<RowIndenifier, int> rowIndexCache = new Dictionary<RowIndenifier, int>();

        public static int GetRowIndex(DataGridView dataGridView, RowIndenifier rowIndenifier)
        {
            #region Cache logic
            if (rowIndexCache.TryGetValue(rowIndenifier, out int rowIndexFound))
            {
                DataGridViewGenericRow dataGridViewGenericRowCheck = GetRowDataGridViewGenericRow(dataGridView, rowIndexFound);

                if (dataGridViewGenericRowCheck != null && 
                    dataGridViewGenericRowCheck.RowIndenifier == rowIndenifier) 
                    return rowIndexFound;
                else 
                    rowIndexCache.Clear();
            }
            #endregion

            int foundRow = -1;
            #region Find row
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericRowCheck = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRowCheck != null)
                {
                    if (!rowIndexCache.ContainsKey(dataGridViewGenericRowCheck.RowIndenifier)) rowIndexCache.Add(dataGridViewGenericRowCheck.RowIndenifier, rowIndex);
                    if (dataGridViewGenericRowCheck.RowIndenifier == rowIndenifier) foundRow = rowIndex;                    
                }
            }
            #endregion

            return foundRow;
        }
        #endregion

        #region Rows handling - GetRowIndex
        public static int GetRowIndex(DataGridView dataGridView, string headerName)
        {
            return GetRowIndex(dataGridView, new RowIndenifier(headerName));
        }
        #endregion

        #region Rows handling - GetRowIndex
        public static int GetRowIndex(DataGridView dataGridView, string headerName, string rowName)
        {
            return GetRowIndex(dataGridView, new RowIndenifier(headerName, rowName));
        }
        #endregion

        #region Row handling - DeleteRow
        public static bool DeleteRow(DataGridView dataGridView, string headerName, string rowName)
        {
            int rowIndex = GetRowIndex(dataGridView, new RowIndenifier(headerName, rowName));
            if (rowIndex > -1) dataGridView.Rows.RemoveAt(rowIndex);
            return rowIndex != -1;
        }
        #endregion

        #region Rows handling - AddRowAndValueList
        public static void AddRowAndValueList(DataGridView dataGridView, FileEntryAttribute fileEntryColumn, List<DataGridViewGenericRowAndValue> dataGridViewGenericRowAndValueList, bool sort)
        {
            int columnIndex = GetColumnIndexPriorities(dataGridView, fileEntryColumn, out FileEntryVersionCompare fileEntryVersionCompareReason);

            foreach (DataGridViewGenericRowAndValue dataGridViewGenericRowAndValue in dataGridViewGenericRowAndValueList)
            {
                AddRow(dataGridView, columnIndex, dataGridViewGenericRowAndValue.DataGridViewGenericRow,
                    GetFavoriteList(dataGridView),
                    dataGridViewGenericRowAndValue.DataGridViewGenericCell.Value,
                    dataGridViewGenericRowAndValue.DataGridViewGenericCell.CellStatus, 0, true, sort);
            }
        }
        #endregion

        #region Rows handling - AddRow
        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow, bool sort)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericRow, GetFavoriteList(dataGridView), null,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), 0, false, sort);
        }
        #endregion

        #region Rows handling - AddRow
        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow, object value, bool cellReadOnly, bool sort)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericRow, GetFavoriteList(dataGridView), value,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, cellReadOnly), 0, true, sort);
        }
        #endregion

        #region Rows handling - AddRow
        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults, bool sort)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericRow, GetFavoriteList(dataGridView), value,
                dataGridViewGenericCellStatusDefaults, 0, true, sort);
        }
        #endregion

        #region Rows handling - AddRow
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="columnIndex">If column == -1, that means new row without values in columns</param>
        /// <param name="dataGridViewGenericRow"></param>
        /// <param name="dataGridFavorites"></param>
        /// <param name="value">Set value for the field, check writeValue if the field can/will be updated</param>
        /// <param name="dataGridViewGenericCellStatusDefault">When not exists, write this, ReadOnly field will always be updated</param>
        /// <param name="startSearchRow"></param>
        /// <param name="writeValue"></param>
        /// <returns></returns>
        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow,
            List<FavoriteRow> dataGridFavorites, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault, int startSearchRow, bool writeValue, bool sort, bool forceAddAfterStartSearchRow = false)
        {
            bool rowFound;
            int rowIndex;
            //column -1 is ok

            #region Find row, if not find where, sort or not sort
            if (forceAddAfterStartSearchRow)
            {
                rowIndex = startSearchRow;
                rowFound = false;
            }
            else rowIndex = FindFileEntryRow(dataGridView, dataGridViewGenericRow, startSearchRow, sort, out rowFound);

            if (!rowFound) //If not found, add a new row
            {
                if (rowIndex == -1)
                {
                    if (sort) rowIndex = startSearchRow; //if sorting, add in begging of search
                    else rowIndex = GetRowCountWithoutEditRow(dataGridView); //If not sorting, add last line
                }
                else rowIndex++; //add row after found line

                if (rowIndex != -1)
                {
                    dataGridView.Rows.Insert(rowIndex, 1);
                    SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, dataGridViewGenericRow);
                    SetCellStatusDefaultWhenRowAdded(dataGridView, rowIndex, dataGridViewGenericCellStatusDefault);
                }
            }
            #endregion

            bool isValueUpdated = false;
            //If a value row, set the value
            if (!dataGridViewGenericRow.IsHeader)
            {
                if (writeValue)
                {
                    if (dataGridViewGenericRow.FileEntryAttribute != null &&
                        dataGridView.Rows[rowIndex].HeaderCell.Tag is DataGridViewGenericRow dataGridViewGenericRowDataGridView)
                    {
                        FileEntryVersionCompare fileEntryVersionCompareReason =
                            FileEntryVersionHandler.CompareFileEntryAttribute(dataGridViewGenericRowDataGridView.FileEntryAttribute, dataGridViewGenericRow.FileEntryAttribute);

                        if (FileEntryVersionHandler.DoesCellsNeedUpdate(fileEntryVersionCompareReason))
                            dataGridView.Rows[rowIndex].HeaderCell.Tag = dataGridViewGenericRow;
                    }

                    if (dataGridView.Rows[rowIndex].Cells[columnIndex].Value != value)
                    {
                        isValueUpdated = true;
                        dataGridView.Rows[rowIndex].Cells[columnIndex].Value = value;
                    }
                }
                if (isValueUpdated && dataGridViewGenericRow.IsMultiLine) dataGridView.Columns[columnIndex].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            else dataGridViewGenericCellStatusDefault.CellReadOnly = true;

            bool isRowCreated = !rowFound;
            
            if (columnIndex != -1) //When adding empty row without value in a given column
            {
                //It's only{possible to update ReadOnly field
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
                if (dataGridViewGenericCellStatus != null)
                {
                    //if (dataGridViewGenericCellStatus.CellReadOnly != dataGridViewGenericCellStatusDefault.CellReadOnly)
                    {                    
                        dataGridViewGenericCellStatus.CellReadOnly = dataGridViewGenericCellStatusDefault.CellReadOnly;
                        SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
                        SetCellBackGroundColor(dataGridView, columnIndex, rowIndex);
                    }
                }

            }
            if (isRowCreated)
            {
                SetRowFavoriteFlag(dataGridView, rowIndex, dataGridFavorites);
                SetCellBackGroundColorForRow(dataGridView, rowIndex);
            }

            return rowIndex;
        }
        #endregion

        #region Rows handling - SetRowHeaderNameAndFontStyle
        public static void SetRowHeaderNameAndFontStyle(DataGridView dataGridView, int rowIndex, string headerName, string rowName, ReadWriteAccess readWriteAccess)
        {
            SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, new DataGridViewGenericRow(headerName, rowName, readWriteAccess));
        }
        #endregion

        #region Rows handling - SetRowHeaderNameAndFontStyle
        public static void SetRowHeaderNameAndFontStyle(DataGridView dataGridView, int rowIndex, DataGridViewGenericRow dataGridViewGenericRow)
        {
            dataGridView.Rows[rowIndex].HeaderCell.Tag = dataGridViewGenericRow;

            if (dataGridViewGenericRow.IsHeader)
            {
                dataGridView.Rows[rowIndex].HeaderCell.Value = dataGridViewGenericRow.HeaderName;
                dataGridView.Rows[rowIndex].HeaderCell.Style.Font = new Font(dataGridView.Font, FontStyle.Bold);
            }
            else
            {
                dataGridView.Rows[rowIndex].HeaderCell.Value = dataGridViewGenericRow.RowName;
                dataGridView.Rows[rowIndex].HeaderCell.Style.Font = new Font(dataGridView.Font, FontStyle.Regular);
            }
        }
        #endregion

        #region Rows handling - GetRowName
        public static string GetRowName(DataGridView dataGridView, int rowIndex)
        {
            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            return dataGridViewGenericRow == null ? "" : (dataGridViewGenericRow.IsHeader ? dataGridViewGenericRow.HeaderName : dataGridViewGenericRow.RowName);
        }
        #endregion

        #region Rows handling - GetRowValue
        public static string GetRowValue(DataGridView dataGridView, int rowIndex)
        {
            return (string)dataGridView.Rows[rowIndex].HeaderCell.Value;
        }
        #endregion

        #region Rows handling - GetRowHeader
        public static string GetRowHeader(DataGridView dataGridView, int rowIndex)
        {
            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            return dataGridViewGenericRow == null ? "" : dataGridViewGenericRow.HeaderName;
        }
        #endregion

        #region Rows handling - SetRowToolTipText
        public static void SetRowToolTipText(DataGridView dataGridView, int rowIndex, string toolTipText)
        {
            dataGridView.Rows[rowIndex].HeaderCell.ToolTipText = toolTipText;
        }
        #endregion

        #region Row handling - Favorite handling

        #region Row handling - Favorite handling - GetFavoriteList
        public static List<FavoriteRow> GetFavoriteList(DataGridView dataGridView)
        {
            return GetDataGridViewGenericData(dataGridView)?.FavoriteList;
        }
        #endregion

        #region Row handling - Favorite handling - SetFavoriteList
        public static void SetFavoriteList(DataGridView dataGridView, List<FavoriteRow> dataGridFavoriteList)
        {
            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return;
            dataGridViewGenericData.FavoriteList = dataGridFavoriteList;
        }
        #endregion

        #region Row handling - Favorite handling - CreateFavoriteFilename
        private static string CreateFavoriteFilename(string dataGridViewName)
        {
            return FileHandler.GetLocalApplicationDataPath("Favourite." + dataGridViewName + ".json", deleteOldTempFile: false);
        }
        #endregion

        #region Row handling - Favorite handling - FavouriteRead
        public static List<FavoriteRow> FavouriteRead(string filename)
        {
            List<FavoriteRow> favouriteRows = new List<FavoriteRow>();
            if (File.Exists(filename))
            {
                favouriteRows = JsonConvert.DeserializeObject<List<FavoriteRow>>(File.ReadAllText(filename));
            }
            return favouriteRows;
        }
        #endregion

        #region Row handling - Favorite handling - FavouriteWrite
        public static void FavouriteWrite(string filename, List<FavoriteRow> favouriteRows)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(favouriteRows, Newtonsoft.Json.Formatting.Indented));
        }
        #endregion

        #region Row handling - Favorite handling - FavouriteWrite
        public static void FavouriteWrite(DataGridView dataGridView, List<FavoriteRow> favouriteRows)
        {
            FavouriteWrite(CreateFavoriteFilename(GetDataGridViewName(dataGridView)), favouriteRows);
        }
        #endregion

        #region Row handling - Favorite handling - ActionSetRowsFavouriteState
        public static void ActionSetRowsFavouriteState(DataGridView dataGridView, NewState newState)
        {
            List<FavoriteRow> favouriteRows = GetFavoriteList(dataGridView);
            List<int> selectedRows = GetRowSelected(dataGridView);

            foreach (int rowIndex in selectedRows)
            {
                DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null)
                {
                    dataGridView.InvalidateCell(dataGridView.Rows[rowIndex].HeaderCell);

                    switch (newState)
                    {
                        case NewState.Set:
                            dataGridViewGenericRow.IsFavourite = true;
                            break;
                        case NewState.Remove:
                            dataGridViewGenericRow.IsFavourite = false;
                            break;
                        case NewState.Toggle:
                            dataGridViewGenericRow.IsFavourite = !dataGridViewGenericRow.IsFavourite;
                            break;
                    }

                    FavoriteRow favouriteRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex).GetFavouriteRow();
                    if (dataGridViewGenericRow.IsFavourite)
                    {
                        if (!favouriteRows.Contains(favouriteRow)) favouriteRows.Add(favouriteRow);
                    }
                    else
                    {
                        if (favouriteRows.Contains(favouriteRow)) favouriteRows.Remove(favouriteRow);
                    }

                    SetCellBackGroundColorForRow(dataGridView, rowIndex);
                }
            }

            SetFavoriteList(dataGridView, favouriteRows);

        }
        #endregion

        #region Row handling - Favorite handling - SetRowFavoriteFlag
        public static void SetRowFavoriteFlag(DataGridView dataGridView, int rowIndex)
        {
            SetRowFavoriteFlag(dataGridView, rowIndex, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Row handling - Favorite handling - SetRowFavoriteFlag
        private static void SetRowFavoriteFlag(DataGridView dataGridView, int rowIndex, List<FavoriteRow> dataGridFavorites)
        {
            if (rowIndex >= 0)
            {
                DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                dataGridViewGenericRow.IsFavourite = dataGridFavorites.Contains(new FavoriteRow(dataGridViewGenericRow.HeaderName, dataGridViewGenericRow.RowName, dataGridViewGenericRow.IsHeader));
            } 
        }
        #endregion

        #endregion

        #region Row handling - Is values equal
        private static bool IsRowValuesDifferent(DataGridView dataGridView, int rowIndex)
        {
            bool fistValueFound = false;
            object firstValue = null;
            for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count; columnIndex++)
            {
                if (!fistValueFound)
                {
                    firstValue = dataGridView.Rows[rowIndex].Cells[columnIndex].Value;
                    fistValueFound = true;
                }
                else
                {
                    if (firstValue == null && dataGridView.Rows[rowIndex].Cells[columnIndex].Value != null)
                        return true;
                    else if (firstValue != null && dataGridView.Rows[rowIndex].Cells[columnIndex].Value == null)
                        return true;
                    else if (firstValue == null && dataGridView.Rows[rowIndex].Cells[columnIndex].Value == null)
                    { } //Do nothing
                    else if (firstValue.ToString() != dataGridView.Rows[rowIndex].Cells[columnIndex].Value.ToString())
                        return true;
                }
            }
            return false;
        }

        private static void SetRowEqualFlag(DataGridView dataGridView, int rowIndex)
        {
            GetRowDataGridViewGenericRow(dataGridView, rowIndex).IsEqual = !IsRowValuesDifferent(dataGridView, rowIndex);
        }
        #endregion

        #region Row handling - Header color and Visible Flag

        private static void SetRowVisbleStatus(DataGridView dataGridView, int rowIndex, bool showOnlyDiffrentValues, bool showOnlyFavorite)
        {
            SetRowEqualFlag(dataGridView, rowIndex);

            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (dataGridViewGenericRow != null)
            {                
                bool visble = true;

                if (!dataGridViewGenericRow.IsHeader && dataGridViewGenericRow.IsEqual && showOnlyDiffrentValues) visble = false;
                if (!dataGridViewGenericRow.IsFavourite && showOnlyFavorite) visble = false;

                dataGridView.Rows[rowIndex].Visible = visble;
            }
        }

        public static void SetRowsVisbleStatus(DataGridView dataGridView, bool showOnlyEqual, bool showOnlyFavorite)
        {
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                SetRowVisbleStatus(dataGridView, rowIndex, showOnlyEqual, showOnlyFavorite);
            }
        }
        #endregion

        #endregion

        #region Clipboard - Get and Set Cell location
        #region Refresh - GetCurrentCellLocation
        public static CellLocation GetCurrentCellLocation(DataGridViewCell cell)
        {
            return new CellLocation(cell.ColumnIndex, cell.RowIndex);
        }
        #endregion

        #region Refresh - SetCurrentCellLocation
        public static void SetCurrentCellLocation(DataGridView dataGridView, CellLocation cell)
        {
            dataGridView.CurrentCell = dataGridView[cell.ColumnIndex, cell.RowIndex];
            if (dataGridView.CurrentCell.GetType() == typeof(DataGridViewComboBoxCell)) Refresh(dataGridView);
        }
        #endregion
        #endregion

        #region Refresh - Refresh
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();

        private static Control GetFocusedControl()
        {
            Control focusedControl = null;
            // To get hold of the focused control:
            IntPtr focusedHandle = GetFocus();
            if (focusedHandle != IntPtr.Zero)
                // Note that if the focused Control is not a .Net control, then this will return null.
                focusedControl = Control.FromHandle(focusedHandle);
            return focusedControl;
        }

        public static void Refresh(DataGridView dataGridView)
        {
            Control controlInFocus = GetFocusedControl();
            dataGridView.Parent.Focus(); //Hack to refresh DataGridViewComboBoxCell, do to it will not refresh before changed cell / cell lost focus
            dataGridView.Focus();
            dataGridView.Refresh(); //This created Thread failure // Region????
            if (controlInFocus != null) controlInFocus.Focus();
        }
        #endregion

        #region Cell Handling

        #region Cell Handling - SetCellDefaultAfterUpdated
        public static void SetCellDefaultAfterUpdated(DataGridView dataGridView, DataGridViewGenericCellStatus dataGridViewGenericCellStatus, int columnIndex, int rowIndex)
        {
            DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus, false);
            DataGridViewHandler.SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
            DataGridViewHandler.SetCellBackGroundColor(dataGridView, columnIndex, rowIndex);
        }
        #endregion

        #region Cell Handling - SetCellDefaultAfterUpdated, No DirtyFlag need to be Set
        public static void SetCellDefaultAfterUpdated(DataGridView dataGridView, MetadataBrokerType metadataBrokerType, int columnIndex, int rowIndex)
        {
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex)); //Remember current status, in case of updates
            dataGridViewGenericCellStatus.MetadataBrokerType |= metadataBrokerType;
            if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Disabled) dataGridViewGenericCellStatus.SwitchState = SwitchStates.Undefine;
            if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Undefine)
                dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerType & MetadataBrokerType.ExifTool) == MetadataBrokerType.ExifTool ? SwitchStates.On : SwitchStates.Off;
            dataGridViewGenericCellStatus.CellReadOnly = false;
            SetCellDefaultAfterUpdated(dataGridView, dataGridViewGenericCellStatus, columnIndex, rowIndex);
        }
        #endregion

        #region Cell handling - InvalidateCellColumnHeader
        public static void InvalidateCellColumnHeader(DataGridView dataGridView, int columnIndex)
        {
            dataGridView.InvalidateCell(dataGridView.Columns[columnIndex].HeaderCell);
        }
        #endregion

        #region Cell handling - InvalidateCell
        public static void InvalidateCell(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            dataGridView.InvalidateCell(columnIndex, rowIndex);
        }
        #endregion

        #region Cell Handling - Deep Copy
        public static T DeepCopy<T>(T obj)
        {
            if (obj == null) return (T)(object)null;
            return (T)Process(obj);
        }

        static object Process(object obj)
        {
            if (obj == null)
                return null;
            Type type = obj.GetType();
            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            else if (type.IsArray)
            {
                Type elementType = Type.GetType(
                     type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(Process(array.GetValue(i)), i);
                }
                return Convert.ChangeType(copied, obj.GetType());
            }
            else if (type.IsClass)
            {
                if (type == typeof(Bitmap))
                {
                    return obj; //This should copy Bitmap pixel by pixel, but for this application don't need, just return refrance
                }
                else
                {
                    object toret = Activator.CreateInstance(obj.GetType());
                    FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach (FieldInfo field in fields)
                    {
                        object fieldValue = field.GetValue(obj);
                        if (fieldValue == null)
                            continue;
                        field.SetValue(toret, Process(fieldValue));
                    }
                    return toret;
                }

            }
            else
                throw new ArgumentException("Unknown type");
        }
        #endregion

        #region Cell Handling - GetSelectedCellCount
        public static int GetCellSelectedCount(DataGridView dataGridView)
        {
            return dataGridView.SelectedCells.Count;
        }
        #endregion 

        #region Cell Handling - GetCellSelected
        public static DataGridViewSelectedCellCollection GetCellSelected(DataGridView dataGridView)
        {
            return dataGridView.SelectedCells;
        }
        #endregion 

        #region Cell Handling - GetCellCurrent
        public static DataGridViewCell GetCellCurrent(DataGridView dataGridView)
        {
            return dataGridView.CurrentCell;
        }
        #endregion 

        #region Cell Handling - GetCellDataGridViewCell -  int columnIndex, int rowIndex
        public static DataGridViewCell GetCellDataGridViewCell(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView.Rows[rowIndex].Cells[columnIndex];
        }
        #endregion

        #region Cell Handling - GetCellValue - int columnIndex, string headerName, string rowName
        public static object GetCellValue(DataGridView dataGridView, int columnIndex, string headerName, string rowName)
        {
            int rowIndex = GetRowIndex(dataGridView, new RowIndenifier(headerName, rowName));
            if (columnIndex > -1 && rowIndex > -1) return GetCellValue(dataGridView,columnIndex, rowIndex);
            else return null;
        }
        #endregion

        #region Cell Handling - GetCellValue - dataGridViewCell
        public static object GetCellValue(DataGridViewCell dataGridViewCell)
        {
            return dataGridViewCell.Value;
        }
        #endregion

        #region Cell Handling - GetCellValue - int columnIndex, int rowIndex
        public static object GetCellValue(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return GetCellValue(dataGridView.Rows[rowIndex].Cells[columnIndex]);
        }
        #endregion

        #region Cell Handling - GetCellValueNullOrStringTrim - int columnIndex, int rowIndex
        public static string GetCellValueNullOrStringTrim(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            string value = null;
            try
            {
                value = (dataGridView.Rows[rowIndex].Cells[columnIndex].Value == null ? null : dataGridView.Rows[rowIndex].Cells[columnIndex].Value.ToString().Trim());
                if (string.IsNullOrEmpty(value)) value = null;
            }
            catch (Exception ex) 
            { 
                Logger.Error(ex); 
            }
            return value;
            
        }
        #endregion

        #region Cell Handling - GetCellValueNullOrStringTrim - int columnIndex, string headerName, string rowName
        public static string GetCellValueNullOrStringTrim(DataGridView dataGridView, int columnIndex, string headerName, string rowName)
        {
            int rowIndex = GetRowIndex(dataGridView, new RowIndenifier(headerName, rowName));
            return GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex);
        }
        #endregion

        #region Cell Handling - IsCellNullOrWhiteSpace - int columnIndex, int rowIndex
        public static bool IsCellNullOrWhiteSpace(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView.Rows[rowIndex].Cells[columnIndex].Value == null || string.IsNullOrWhiteSpace(dataGridView.Rows[rowIndex].Cells[columnIndex].Value.ToString().Trim());
        }
        #endregion

        #region Cell Handling - SetCellValue - int columnIndex, string headerName, string rowName, object value
        public static void SetCellValue(DataGridView dataGridView, int columnIndex, string headerName, string rowName, object value)
        {
            int rowIndex = GetRowIndex(dataGridView, new RowIndenifier(headerName, rowName));
            SetCellValue(dataGridView, columnIndex, rowIndex, value, false);
        }
        #endregion

        #region Cell Handling - SetCellValue - int columnIndex, int rowIndex, object value
        public static void SetCellValue(DataGridView dataGridView, int columnIndex, int rowIndex, object value, bool setDirtyFalgWhenValueChanged)
        {
            if (rowIndex > -1 && columnIndex > -1)
            {
                if (setDirtyFalgWhenValueChanged && dataGridView.Rows[rowIndex].Cells[columnIndex].Value != value) 
                    SetColumnDirtyFlag(dataGridView, columnIndex, true); 
                dataGridView.Rows[rowIndex].Cells[columnIndex].Value = value;
            }
        }
        #endregion

        #region Cell Handling - GetCellStatus - dataGridViewCell
        public static DataGridViewGenericCellStatus GetCellStatus(DataGridViewCell dataGridViewCell)
        {
            return dataGridViewCell.Tag as DataGridViewGenericCellStatus;
        }
        #endregion

        #region Cell Handling - GetCellStatus - int columnIndex, int rowIndex
        public static DataGridViewGenericCellStatus GetCellStatus(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return GetCellStatus(dataGridView.Rows[rowIndex].Cells[columnIndex]);
        }
        #endregion

        #region Cell Handling - SetCellStatus - int columnIndex, int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatus
        public static void SetCellStatus(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatus, bool setDirtyFalgWhenValueChanged)
        {
            if (rowIndex > -1 && columnIndex > -1)
            {
                if (setDirtyFalgWhenValueChanged)
                {
                    SwitchStates switchStates = GetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex);
                    if (switchStates != SwitchStates.Undefine && switchStates != dataGridViewGenericCellStatus.SwitchState)
                        SetColumnDirtyFlag(dataGridView, columnIndex, true);
                }
                dataGridView.Rows[rowIndex].Cells[columnIndex].Tag = dataGridViewGenericCellStatus;
            }
        }
        #endregion

        #region Cell Handling - SetCellStatusDefaultWhenRowAdded - int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault
        public static void SetCellStatusDefaultWhenRowAdded(DataGridView dataGridView, int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault)
        {
            foreach (DataGridViewCell dataGridCell in dataGridView.Rows[rowIndex].Cells)
            {
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, dataGridCell.ColumnIndex, dataGridCell.RowIndex);
                if (dataGridViewGenericCellStatus == null)
                {
                    DataGridViewGenericCellStatus dataGridViewGenericCellStatusCopy = new DataGridViewGenericCellStatus(dataGridViewGenericCellStatusDefault);
                    SetCellStatus(dataGridView, dataGridCell.ColumnIndex, rowIndex, dataGridViewGenericCellStatusCopy, false);
                    SetCellReadOnlyDependingOfStatus(dataGridView, dataGridCell.ColumnIndex, rowIndex, dataGridViewGenericCellStatusCopy);
                }

            }
        }
        #endregion

        #region Cell Handling - SetCellStatusDefaultColumnWhenAdded - int columnIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault
        private static void SetCellStatusDefaultColumnWhenAdded(DataGridView dataGridView, int columnIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault)
        {
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
                if (dataGridViewGenericCellStatus == null)
                {
                    DataGridViewGenericCellStatus dataGridViewGenericCellStatusCopy = new DataGridViewGenericCellStatus(dataGridViewGenericCellStatusDefault);
                    SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatusCopy, false);
                    SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatusCopy);
                }
            }
        }
        #endregion

        #region Cell Handling - SetCellValue - value
        public static void SetCellValue(DataGridView dataGridView, DataGridViewCell dataGridViewCell, object value)
        {
            try
            {
                DataGridViewComboBoxCell cell = dataGridViewCell as DataGridViewComboBoxCell;                
                if (cell != null && value != null && !cell.Items.Contains(value))
                {
                    cell.Items.Insert(0, value);
                    if (dataGridView.IsCurrentCellDirty) dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);                    
                }
                if (dataGridViewCell.Value is string)
                {
                    if ((string)dataGridViewCell.Value != (string)value) DataGridViewHandler.SetColumnDirtyFlag(dataGridView, dataGridViewCell.ColumnIndex, true);
                    dataGridViewCell.Value = value;
                }
                else if (dataGridViewCell.Value is LocationNames.LocationCoordinate)
                {
                    if (dataGridViewCell.Value != value) DataGridViewHandler.SetColumnDirtyFlag(dataGridView, dataGridViewCell.ColumnIndex, true);
                    dataGridViewCell.Value = value;
                }
                else if (dataGridViewCell.Value is RegionStructure cellRegionStructure)
                {
                    if (cellRegionStructure.Name != (string)value) DataGridViewHandler.SetColumnDirtyFlag(dataGridView, dataGridViewCell.ColumnIndex, true);
                    cellRegionStructure.Name = (string)value;
                }
                else
                {
                    if ((dataGridViewCell.Value != null && value != null) &&
                        ((dataGridViewCell.Value == null && value != null) ||
                        (dataGridViewCell.Value != null && value == null) ||
                        dataGridViewCell.Value.ToString() != value.ToString())) DataGridViewHandler.SetColumnDirtyFlag(dataGridView, dataGridViewCell.ColumnIndex, true);
                    dataGridViewCell.Value = value;
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Cell Handling - SetCellToolTipText
        public static void SetCellToolTipText(DataGridView dataGridView, int columnIndex, int rowIndex, string toolTipText)
        {
            dataGridView.Rows[rowIndex].Cells[columnIndex].ToolTipText = toolTipText;
        }
        #endregion

        #region Cell Handling - SetCellToolTipText
        public static void SetCellToolTipText(DataGridView dataGridView, int columnIndex, int rowIndex, string heading, List<string> autokeywords)
        {
            string tooltip = "";
            foreach (string keyword in autokeywords) tooltip = (string.IsNullOrWhiteSpace(tooltip) ? "" : tooltip + "\r\n") + keyword;
            if (!string.IsNullOrWhiteSpace(tooltip)) tooltip = heading + "\r\n" + tooltip;
            SetCellToolTipText(dataGridView, columnIndex, rowIndex, tooltip);
        }
        #endregion

        #region Cell Handling - SetCellControlType
        public static void SetCellControlType(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewCell dataGridViewCell)
        {
            if (rowIndex > -1 && columnIndex > -1) dataGridView.Rows[rowIndex].Cells[columnIndex] = dataGridViewCell;
        }
        #endregion

        #region Cell Handling - CopyCellDataGridViewGenericCell - dataGridViewCell
        public static DataGridViewGenericCell CopyCellDataGridViewGenericCell(DataGridViewCell dataGridViewCell)
        {
            return new DataGridViewGenericCell(DeepCopy(GetCellValue(dataGridViewCell)), DeepCopy(GetCellStatus(dataGridViewCell)));
        }
        #endregion

        #region Cell Handling - CopyCellDataGridViewGenericCell - int columnIndex, int rowIndex
        public static DataGridViewGenericCell CopyCellDataGridViewGenericCell(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return new DataGridViewGenericCell(DeepCopy(GetCellValue(dataGridView, columnIndex, rowIndex)), DeepCopy(GetCellStatus(dataGridView, columnIndex, rowIndex)));
        }
        #endregion

        #region Cell Handling - GetCellDataGridViewGenericCell - int columnIndex, int rowIndex
        public static DataGridViewGenericCell GetCellDataGridViewGenericCellCopy(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return new DataGridViewGenericCell(GetCellValue(dataGridView, columnIndex, rowIndex), GetCellStatus(dataGridView, columnIndex, rowIndex));
        }
        #endregion

        #region Cell Handling - SetCellDataGridViewGenericCell - int columnIndex, int rowIndex, DataGridViewGenericCell dataGridViewGenericCell
        public static void SetCellDataGridViewGenericCell(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewGenericCell dataGridViewGenericCell, bool setDirtyFalgWhenValueChanged)
        {
            if (rowIndex > -1 && columnIndex > -1)
            {
                SetCellValue(dataGridView, columnIndex, rowIndex, dataGridViewGenericCell.Value, setDirtyFalgWhenValueChanged);
                SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCell.CellStatus, setDirtyFalgWhenValueChanged);
            }
        }
        #endregion

        #region Cell Handling - GetCellRegionStructure - int columnIndex, int rowIndex
        public static RegionStructure GetCellRegionStructure(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex) as MetadataLibrary.RegionStructure;
        }
        #endregion 

        #region Cell Handling - IsCellDataGridViewGenericCellStatus - int columnIndex, int rowIndex
        public static bool IsCellDataGridViewGenericCellStatus(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return (dataGridView.Rows[rowIndex].Cells[columnIndex].Tag != null && dataGridView.Rows[rowIndex].Cells[columnIndex].Tag.GetType() == typeof(DataGridViewGenericCellStatus));
        }
        #endregion

        #region Cell Handling - SetCellBackGroundColorForRow - int rowIndex
        public static void SetCellBackGroundColorForRow(DataGridView dataGridView, int rowIndex)
        {
            if (rowIndex >= 0)
            {
                foreach (DataGridViewCell dataGridCell in dataGridView.Rows[rowIndex].Cells)
                {
                    SetCellBackGroundColor(dataGridView, dataGridCell.ColumnIndex, rowIndex);
                }
            }
        }
        #endregion

        #region Cell Handling - SetCellReadOnlyForColumn - int columnIndex
        public static void SetCellReadOnlyForColumn(DataGridView dataGridView, int columnIndex, bool readOnly)
        {
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
                if (dataGridViewGenericCellStatus != null)
                {
                    dataGridViewGenericCellStatus.CellReadOnly = readOnly;
                    SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus, false);
                    SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
                }
            }
        }
        #endregion

        #region Cell Handling - SetCellBackgroundColorForColumn - int columnIndex
        public static void SetCellBackgroundColorForColumn(DataGridView dataGridView, int columnIndex)
        {
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                SetCellBackGroundColor(dataGridView, columnIndex, rowIndex);
            }
        }
        #endregion

        #region Cell Handling - SetCellReadOnlyDependingOfStatus -  int columnIndex, int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatus
        public static void SetCellReadOnlyDependingOfStatus(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatus)
        {
            if (dataGridViewGenericCellStatus != null &&
                dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly != dataGridViewGenericCellStatus.CellReadOnly) //Why check, because changing takes lot of CPU time
                dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly = dataGridViewGenericCellStatus.CellReadOnly;

            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

            if (dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.ForceCellToReadOnly &&
                dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly != true)
                dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly = true;

            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (dataGridViewGenericRow != null && (
                dataGridViewGenericRow.ReadWriteAccess == ReadWriteAccess.ForceCellToReadOnly) &&
                dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly != true)
                dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly = true;
        }
        #endregion

        #region Cell Handling - SetCellBackGroundColor - int columnIndex, int rowIndex
        private static void SetCellBackGroundColor(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            
            Color backColor = Color.Empty;
            Color textColor = Color.Empty;

            if (dataGridViewGenericRow == null)
            {
                if (dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly)
                {
                    backColor = ColorBackCellReadOnly(dataGridView);
                    textColor = ColorTextCellReadOnly(dataGridView);
                }
                else
                {
                    backColor = ColorBackCellNormal(dataGridView);
                    textColor = ColorTextCellNormal(dataGridView);
                }
            }
            else
            {
                if (dataGridViewGenericRow.IsFavourite && 
                    !dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly)
                {
                    backColor = ColorBackCellFavorite(dataGridView);
                    textColor = ColorTextCellFavorite(dataGridView);
                }
                else if (!dataGridViewGenericRow.IsFavourite && 
                    dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly)
                {
                    backColor = ColorBackCellReadOnly(dataGridView);
                    textColor = ColorTextCellReadOnly(dataGridView);
                }
                else if (dataGridViewGenericRow.IsFavourite && dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly)
                {
                    backColor = ColorBackCellFavoriteReadOnly(dataGridView);
                    textColor = ColorTextCellFavoriteReadOnly(dataGridView);
                }
                else
                {
                    backColor = ColorBackCellNormal(dataGridView);
                    textColor = ColorTextCellNormal(dataGridView);
                }
            }

            if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion == FileEntryVersion.Error)
            {
                backColor = ColorBackCellError(dataGridView);
                textColor = ColorTextCellError(dataGridView);
            }

            if (backColor != Color.Empty &&
                (backColor.A != dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor.A ||
                backColor.B != dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor.B ||
                backColor.G != dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor.G ||
                backColor.R != dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor.R))
            {
                if (dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor.A != backColor.A ||
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor.B != backColor.B ||
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor.G != backColor.G ||
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor.R != backColor.R
                    ) //Why check? Update DataGridView takes time
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style.BackColor = backColor;

                if (dataGridView.Rows[rowIndex].Cells[columnIndex].Style.ForeColor.A != textColor.A ||
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style.ForeColor.B != textColor.B ||
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style.ForeColor.G != textColor.G ||
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style.ForeColor.R != textColor.R

                    ) //Why check? Update DataGridView takes time
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Style.ForeColor = textColor;
            }
        }
        #endregion

        #region Cell Handling - GetCellReadOnly - int columnIndex, int rowIndex
        public static bool GetCellReadOnly(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView.Rows[rowIndex].Cells[columnIndex].ReadOnly;
        }
        #endregion

        #region Cell Handling - GetCellStatusSwichStatus - int columnIndex, int rowIndex
        public static SwitchStates GetCellStatusSwichStatus(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
            return dataGridViewGenericCellStatus == null ? SwitchStates.Undefine : dataGridViewGenericCellStatus.SwitchState;
        }
        #endregion

        #region Cell Handling - SetCellStatusSwichStatus - int columnIndex, int rowIndex, SwitchStates switchState
        public static void SetCellStatusSwichStatus(DataGridView dataGridView, int columnIndex, int rowIndex, SwitchStates switchState)
        {
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null || dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.ForceCellToReadOnly) //History or Error column
                switchState = SwitchStates.Disabled;

            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
            if (dataGridViewGenericCellStatus == null) dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, switchState, true);
            dataGridViewGenericCellStatus.SwitchState = switchState;
        }
        #endregion

        #region Cell Handling - GetCellStatusMetadataBrokerType - int columnIndex, int rowIndex
        public static MetadataBrokerType GetCellStatusMetadataBrokerType(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
            return dataGridViewGenericCellStatus == null ? MetadataBrokerType.Empty : dataGridViewGenericCellStatus.MetadataBrokerType;
        }
        #endregion

        #endregion

        #region Copy Text within Grid
        #region Copy Text within Grid - CopyCellFromBrokerToMedia
        //Copy select Media Broker as Windows Life Photo Gallery, Microsoft Photots, Google Location History, etc... to correct Media File Tag
        public static Dictionary<CellLocation, DataGridViewGenericCell> CopyCellFromBrokerToMedia(DataGridView dataGridView, string targetHeader, string targetRowNameStartWithAndTrunc, int columnIndex, int rowIndex, bool doOwerwriteData)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            DataGridViewGenericRow dataGridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);

            string foundTargetRowName;
            if (targetRowNameStartWithAndTrunc == null)
            {
                foundTargetRowName = dataGridViewGenericDataRow.RowName;
            }
            else
            {
                if (dataGridViewGenericDataRow.RowName.StartsWith(targetRowNameStartWithAndTrunc)) foundTargetRowName = targetRowNameStartWithAndTrunc;
                else foundTargetRowName = dataGridViewGenericDataRow.RowName;
            }
            int targetRowIndex = DataGridViewHandler.GetRowIndex(dataGridView, targetHeader, foundTargetRowName);
            if (targetRowIndex != -1)
            {
                if (doOwerwriteData ||
                (!doOwerwriteData && (GetCellValue(dataGridView, columnIndex, targetRowIndex) == null || string.IsNullOrWhiteSpace(GetCellValue(dataGridView, columnIndex, targetRowIndex).ToString()))
                ))
                {
                    CellLocation cellLocation = new CellLocation(columnIndex, targetRowIndex);
                    DataGridViewGenericCell dataGridViewGenericCellTarget = DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, columnIndex, targetRowIndex);
                    DataGridViewGenericCell dataGridViewGenericCellCopyFrom = DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, columnIndex, rowIndex);

                    if (dataGridViewGenericCellTarget != dataGridViewGenericCellCopyFrom)
                    {
                        if (!updatedCells.ContainsKey(cellLocation)) updatedCells.Add(cellLocation, DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, columnIndex, targetRowIndex));
                        DataGridViewHandler.SetCellValue(dataGridView, columnIndex, targetRowIndex, DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex), true);
                    }
                }
            }            
            return updatedCells;
        }
        #endregion

        #region Copy Text within Grid - CopySelectedCellFromBrokerToMedia
        public static void CopySelectedCellFromBrokerToMedia(DataGridView dataGridView, string targetHeader, string targetRowName, bool overwrite)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, dataGridViewCell.ColumnIndex);
                if (dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                {
                    
                    Dictionary<CellLocation, DataGridViewGenericCell> updatedCellsDelta =
                        CopyCellFromBrokerToMedia(dataGridView, targetHeader, targetRowName, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex, overwrite);
                    
                    if (updatedCellsDelta != null && updatedCellsDelta.Count > 0)
                    {
                        foreach (CellLocation cellLocation in updatedCellsDelta.Keys)
                        {
                            if (!updatedCells.ContainsKey(cellLocation)) updatedCells.Add(cellLocation, updatedCellsDelta[cellLocation]);
                        }
                    }
                }
            }
            
            if (updatedCells != null && updatedCells.Count > 0) ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
        }
        #endregion
        #endregion 

        #region TriState handeling

        #region TriState handeling - GetRowHeadingIndex
        public static int GetRowHeadingIndex(DataGridView dataGridView, string header)
        {
            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCount(dataGridView); rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericDataRow != null &&
                    dataGridViewGenericDataRow.IsHeader &&
                    dataGridViewGenericDataRow.HeaderName.Equals(header))
                {
                    return rowIndex;
                }
            }
            return dataGridView.RowCount;
        }
        #endregion

        #region TriState handeling - GetRowHeaderItemStarts
        public static int GetRowHeaderItemStarts(DataGridView dataGridView, string header)
        {
            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, header);
            if (rowIndex > -1) return rowIndex + 1;
            return DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); ;
        }
        #endregion

        #region TriState handeling - GetRowHeadingItemsEnds
        public static int GetRowHeaderItemsEnds(DataGridView dataGridView, string header)
        {
            for (int rowIndex = DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView) - 1; rowIndex > -1 ; rowIndex--)
            {
                DataGridViewGenericRow dataGridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericDataRow != null &&
                    !dataGridViewGenericDataRow.IsHeader &&
                    dataGridViewGenericDataRow.HeaderName.Equals(header))
                {
                    if (dataGridView.AllowUserToAddRows && rowIndex == dataGridView.RowCount)
                        return rowIndex - 1;
                    else
                        return rowIndex;
                }
            }

            return -1;
        }
        #endregion

        #region TriState handeling - GetRowHeaderLast
        public static int GetRowHeaderLast(DataGridView dataGridView, string header)
        {
            for (int rowIndex = DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView) - 1; rowIndex > -1; rowIndex--)
            {
                DataGridViewGenericRow dataGridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericDataRow != null &&
                    dataGridViewGenericDataRow.HeaderName.Equals(header))
                {
                    if (dataGridView.AllowUserToAddRows && rowIndex == dataGridView.RowCount)
                        return rowIndex - 1;
                    else
                        return rowIndex;
                }
            }

            return -1;
        }
        #endregion

        #region TriState handeling - GetTriState
        private static TriState GetTriState(DataGridView dataGridView, string header, int columnStart, int rowStart, int columnIncrement, int rowIncrement)
        {
            //Find the corrent state
            bool isAllAdded = true;
            bool isAllDeleted = true;

            bool isSomeAdded = false;
            bool isSomeDeleted = false;

            int rowIndex = rowStart;
            int columnIndex = columnStart;

            bool checkAll = rowIncrement == 1 && columnIncrement == 1;

            DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            while (columnIndex < GetColumnCount(dataGridView)  && rowIndex < GetRowCount(dataGridView))
            {
                if (gridViewGenericDataRow != null && gridViewGenericDataRow.HeaderName.Equals(header))
                {

                    DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                    if (dataGridViewGenericDataColumn.Metadata != null) //Don't care about Files not loaded yet, 
                    {
                        if (GetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex) == SwitchStates.Disabled)
                        {
                            //Do nothing
                        }
                        else if (GetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex) == SwitchStates.On)
                        {
                            isAllDeleted = false;

                            if ((GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) & MetadataBrokerType.ExifTool) == 0)
                            {
                                isSomeAdded = true;
                            }
                        }
                        else
                        {
                            isAllAdded = false;
                            if ((GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) & MetadataBrokerType.ExifTool) != 0)
                            {
                                isSomeDeleted = true;
                            }
                        }

                    }

                }

                columnIndex += columnIncrement;
                if (checkAll)
                {
                    if (columnIndex > dataGridView.ColumnCount - 1)
                    {
                        columnIndex = 0;
                        rowIndex += rowIncrement;
                    }
                }
                else rowIndex += rowIncrement;

                gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            }

            TriState columnState;
            if (isSomeAdded && isSomeDeleted) //Something added and something deleted
                columnState = TriState.SomethingAddedAndDeleted;
            else if (!isSomeAdded && !isSomeDeleted) //Unchanged
                columnState = TriState.Unchange;
            else if (isSomeAdded && !isAllAdded)
                columnState = TriState.SomethingAdded;
            else if (isSomeDeleted && !isAllDeleted)
                columnState = TriState.SomethingDeleded;
            else if (isAllAdded)
                columnState = TriState.AllAdded;
            else if (isAllDeleted)
                columnState = TriState.AllDeleted;
            else
                columnState = TriState.Unchange;
            return columnState;
        }
        #endregion

        #region TriState handeling - GetColumnTriState
        public static TriState GetColumnTriState(DataGridView dataGridView, string header, int keywordsStarts, int columnIndex)
        {
            return GetTriState(dataGridView, header, columnIndex, keywordsStarts, 0, 1);
        }
        #endregion

        #region TriState handeling - GetRowTriState
        public static TriState GetRowTriState(DataGridView dataGridView, string header, int rowIndex)
        {
            return GetTriState(dataGridView, header, 0, rowIndex, 1, 0);
        }
        #endregion

        #region TriState handeling - GetAllTriState
        public static TriState GetAllTriState(DataGridView dataGridView, string header, int keywordsStarts)
        {
            return GetTriState(dataGridView, header, 0, keywordsStarts, 1, 1);
        }
        #endregion

        #region TriState handeling - SetNewTriStateValue
        private static Dictionary<CellLocation, DataGridViewGenericCell> SetNewTriStateValue(DataGridView dataGridView, TriState newTriState,
            int keywordHeaderIndex, int keywordsStarts, int keywordsEnds,
            int columnStart, int rowStart, int columnIncrement, int rowIncrement)
        {
            int rowIndex = rowStart;
            int columnIndex = columnStart;
            bool checkAll = rowIncrement == 1 && columnIncrement == 1;

            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            while (rowIndex <= keywordsEnds && columnIndex < dataGridView.ColumnCount)
            {
                updatedCells.Add(new CellLocation(columnIndex, rowIndex), DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, columnIndex, rowIndex));
                DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                if (dataGridViewGenericDataColumn.Metadata != null && GetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex) != SwitchStates.Disabled)
                {
                    switch (newTriState)
                    {
                        case TriState.AllAdded:
                            //if (dataGridViewGenericDataColumn.Metadata != null)
                            {
                                SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.On);
                            }
                            break;
                        case TriState.SomethingAdded:
                            //if (dataGridViewGenericDataColumn.Metadata != null)
                            {
                                if (GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) != MetadataBrokerType.Empty)
                                {
                                    SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.On);
                                }
                            }
                            break;
                        case TriState.AllDeleted:
                            SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.Off);
                            break;
                        case TriState.Unchange:
                            if ((GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) & MetadataBrokerType.ExifTool) != 0)
                                SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.On);
                            else
                                SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.Off);
                            break;
                    }
                }

                columnIndex += columnIncrement;
                if (checkAll)
                {
                    if (columnIndex > GetColumnCount(dataGridView) - 1)
                    {
                        columnIndex = 0;
                        rowIndex += rowIncrement;
                    }
                }
                else rowIndex += rowIncrement;
            }

            //if (checkAll)  
            dataGridView.InvalidateCell(-1, keywordHeaderIndex); //Force cell to check If need to change status

            if (columnIncrement == 1)
            {
                for (columnIndex = 1; columnIndex < GetColumnCount(dataGridView); columnIndex++)
                    dataGridView.InvalidateCell(columnIndex, keywordHeaderIndex);
            }

            if (rowIncrement == 1)
            {
                for (rowIndex = keywordsStarts; rowIndex <= keywordsEnds; rowIndex++)
                    dataGridView.InvalidateCell(-1, rowIndex);
            }

            return updatedCells;
        }
        #endregion

        #region TriState handeling - SetNextTriState
        private static Dictionary<CellLocation, DataGridViewGenericCell> SetNextTriState(DataGridView dataGridView, string header, NewState newState, int keywordHeaderIndex, int keywordsStarts, int keywordsEnds, int columnStart, int rowStart, int columnIncrement, int rowIncrement)
        {
            bool checkAll = rowIncrement == 1 && columnIncrement == 1;

            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = null;

            switch (newState)
            {
                case NewState.Remove:
                    updatedCells = SetNewTriStateValue(dataGridView, TriState.AllDeleted, keywordHeaderIndex, keywordsStarts, keywordsEnds, columnStart, rowStart, columnIncrement, rowIncrement);
                    break;
                case NewState.Set:
                    updatedCells = SetNewTriStateValue(dataGridView, TriState.AllAdded, keywordHeaderIndex, keywordsStarts, keywordsEnds, columnStart, rowStart, columnIncrement, rowIncrement);
                    break;
                case NewState.Toggle:
                    TriState currentTriState;

                    if (checkAll) currentTriState = GetAllTriState(dataGridView, header, keywordsStarts);
                    else if (rowIncrement == 1) currentTriState = GetColumnTriState(dataGridView, header, keywordsStarts, columnStart);
                    else currentTriState = GetRowTriState(dataGridView, header, rowStart);

                    switch (currentTriState)
                    {
                        case TriState.Unchange: // --> First AllAdded, if can't do all delete
                            updatedCells = SetNewTriStateValue(dataGridView, TriState.SomethingAdded, keywordHeaderIndex, keywordsStarts, keywordsEnds, columnStart, rowStart, columnIncrement, rowIncrement);

                            TriState afterTriState;
                            if (checkAll) afterTriState = GetAllTriState(dataGridView, header, keywordsStarts);
                            else if (rowIncrement == 1) afterTriState = GetColumnTriState(dataGridView, header, keywordsStarts, columnStart);
                            else afterTriState = GetRowTriState(dataGridView, header, rowStart);

                            if (afterTriState == TriState.Unchange)
                                updatedCells = SetNewTriStateValue(dataGridView, TriState.AllAdded, keywordHeaderIndex, keywordsStarts, keywordsEnds, columnStart, rowStart, columnIncrement, rowIncrement);

                            if (checkAll) afterTriState = GetAllTriState(dataGridView, header, keywordsStarts);
                            else if (rowIncrement == 1) afterTriState = GetColumnTriState(dataGridView, header, keywordsStarts, columnStart);
                            else afterTriState = GetRowTriState(dataGridView, header, rowStart);

                            if (afterTriState == TriState.Unchange)
                                updatedCells = SetNewTriStateValue(dataGridView, TriState.AllDeleted, keywordHeaderIndex, keywordsStarts, keywordsEnds, columnStart, rowStart, columnIncrement, rowIncrement);
                            break;
                        case TriState.SomethingAdded: // --> All added
                            updatedCells = SetNewTriStateValue(dataGridView, TriState.AllAdded, keywordHeaderIndex, keywordsStarts, keywordsEnds, columnStart, rowStart, columnIncrement, rowIncrement);
                            break;
                        case TriState.SomethingDeleded:
                        case TriState.AllAdded: // --> All delete
                            updatedCells = SetNewTriStateValue(dataGridView, TriState.AllDeleted, keywordHeaderIndex, keywordsStarts, keywordsEnds, columnStart, rowStart, columnIncrement, rowIncrement);
                            break;
                        case TriState.SomethingAddedAndDeleted:
                        case TriState.AllDeleted: //--> Unchanged
                        default: //Only to remove warning
                            updatedCells = SetNewTriStateValue(dataGridView, TriState.Unchange, keywordHeaderIndex, keywordsStarts, keywordsEnds, columnStart, rowStart, columnIncrement, rowIncrement);
                            break;
                    }
                    break;
            }
            return updatedCells;
        }
        #endregion

        #region TriState handeling - ToggleCells
        public static Dictionary<CellLocation, DataGridViewGenericCell> ToggleCells(DataGridView dataGridView, string header, NewState newState, int columnIndex, int rowIndex)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = null;

            DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (gridViewGenericDataRow == null) return updatedCells; //Don't paint anything TriState on "New Empty Row" for "new Keywords"

            int keywordHeaderStart = DataGridViewHandler.GetRowHeadingIndex(dataGridView, header);
            int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, header);
            int keywordsEnds = DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header);

            if (gridViewGenericDataRow.HeaderName.Equals(header))
            {
                #region All click 
                if (gridViewGenericDataRow.IsHeader && columnIndex == -1)
                {
                    updatedCells = SetNextTriState(dataGridView, header, newState,
                        keywordHeaderStart, keywordsStarts, keywordsEnds,
                        0, keywordsStarts, 1, 1);
                }
                #endregion
                #region Row click - click on a tristate buttons 
                else if (!gridViewGenericDataRow.IsHeader && columnIndex == -1) // 
                {
                    updatedCells = SetNextTriState(dataGridView, header, newState, keywordHeaderStart, keywordsStarts, keywordsEnds, 0, rowIndex, 1, 0);
                }
                #endregion
                #region Column click - click on a tristate buttons 
                else if (gridViewGenericDataRow.IsHeader && columnIndex > -1) //Tag heading, on given column
                {
                    DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                    if (dataGridViewGenericDataColumn.Metadata != null)
                    {
                        updatedCells = SetNextTriState(dataGridView, header, newState, keywordHeaderStart, keywordsStarts, keywordsEnds, columnIndex, keywordHeaderStart, 0, 1);
                    }
                }
                #endregion
                #region Cell - click on a tristate buttons 
                else if (!gridViewGenericDataRow.IsHeader && columnIndex > -1)
                {
                    DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                    if (dataGridViewGenericDataColumn.Metadata != null && GetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex) != SwitchStates.Disabled)
                    {
                        updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();
                        updatedCells.Add(new CellLocation(columnIndex, rowIndex), DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, columnIndex, rowIndex));

                        switch (newState)
                        {
                            case NewState.Remove:
                                SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.Off);
                                break;
                            case NewState.Set:
                                SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.On);
                                break;
                            case NewState.Toggle:
                                if (GetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex) == SwitchStates.Off)
                                    SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.On);
                                else
                                    SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.Off);
                                break;
                        }
                    }
                }
                #endregion
            }

            return updatedCells;
        }
        #endregion

        #region TriState handeling - ToggleSelected
        public static void ToggleSelected(DataGridView dataGridView, string header, NewState newState)
        {
            DataGridViewSelectedCellCollection dataGridViewSelectedCellCollection = dataGridView.SelectedCells;
            if (dataGridViewSelectedCellCollection.Count < 1) return;

            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            foreach (DataGridViewCell dataGridViewCell in dataGridViewSelectedCellCollection)
            {
                DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, dataGridViewCell.RowIndex);

                if (gridViewGenericDataRow != null && gridViewGenericDataRow.HeaderName.Equals(header) && dataGridViewCell.ColumnIndex >= 0)
                {
                    Dictionary<CellLocation, DataGridViewGenericCell> updatedCellsDelta = ToggleCells(dataGridView, header, newState, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);

                    if (updatedCellsDelta != null && updatedCellsDelta.Count > 0)
                    {
                        foreach (CellLocation cellLocation in updatedCellsDelta.Keys)
                        {
                            if (!updatedCells.ContainsKey(cellLocation)) updatedCells.Add(cellLocation, updatedCellsDelta[cellLocation]);
                        }
                    }
                }
            }

            if (updatedCells != null && updatedCells.Count > 0)
            {
                ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
            }

        }
        #endregion

        #endregion

        #region Cell Paint handling
        private const int roundedRadius = 8;

        #region Cell Paint handling - DrawImageAndSubText
        public static void DrawImageAndSubText(object sender, DataGridViewCellPaintingEventArgs e, Image image, string textTop, string textBottom)
        {
            Rectangle rectangleRoundedCellBounds = CalulateCellRoundedRectangleCellBounds(e.CellBounds);
            if (image != null)
            {
                try
                {
                    Size thumbnailSize = CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);
                    Rectangle f = CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);
                    e.Graphics.DrawImage(image, f);
                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(64, Color.White), 3), f);                    
                }
                catch (Exception ex)
                {
                    //Thumbnail was occupied in other thread
                    Logger.Error(ex, "DrawImageAndSubText - Thumbnail was occupied in other thread. Text: " + textTop + textBottom);
                }
            }

            if (!string.IsNullOrWhiteSpace(textTop))
            {
                SizeF sizeF = e.Graphics.MeasureString(textTop, ((DataGridView)sender).Font, rectangleRoundedCellBounds.Size);

                var rectF = new RectangleF(
                    rectangleRoundedCellBounds.X + 2,
                    rectangleRoundedCellBounds.Y + 2,
                    sizeF.Width, sizeF.Height);
                //Filling a 50% transparent rectangle before drawing the string.
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), rectF);
                e.Graphics.DrawString(textTop, ((DataGridView)sender).Font, new SolidBrush(Color.Black), rectF);
            }

            if (!string.IsNullOrWhiteSpace(textBottom))
            {
                SizeF sizeF = e.Graphics.MeasureString(textBottom, ((DataGridView)sender).Font, rectangleRoundedCellBounds.Size);

                var rectF = new RectangleF(
                    rectangleRoundedCellBounds.X + 2,
                    rectangleRoundedCellBounds.Y + rectangleRoundedCellBounds.Height - sizeF.Height - 2,
                    sizeF.Width, sizeF.Height);
                //Filling a 50% transparent rectangle before drawing the string.
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), rectF);
                e.Graphics.DrawString(textBottom, ((DataGridView)sender).Font, new SolidBrush(Color.Black), rectF);
            }
            
        }
        #endregion

        #region Cell Paint handling - DrawImageOnRightSide
        public static void DrawImageOnRightSide(object sender, DataGridViewCellPaintingEventArgs e, Image image)
        {
            e.Graphics.DrawImage(image,
                e.CellBounds.Left + e.CellBounds.Width - image.Width - 1,
                e.CellBounds.Top + 1);
        }
        #endregion

        #region Cell Paint handling - DrawIcon16x16OnLeftSide
        private static void DrawIcon16x16OnLeftSide(object sender, DataGridViewCellPaintingEventArgs e, Image image)
        {
            e.Graphics.DrawImage(image,
                e.CellBounds.Left + 1,
                e.CellBounds.Top + 1, 16, 16); // e.CellBounds.Width, e.CellBounds.Height);
        }
        #endregion

        #region Cell Paint handling - DrawIcon16x16OnRightSide
        private static void DrawIcon16x16OnRightSide(object sender, DataGridViewCellPaintingEventArgs e, Image image)
        {
            e.Graphics.DrawImage(image,
                e.CellBounds.Left + e.CellBounds.Width - 17, 
                e.CellBounds.Top + 1, 16, 16);
        }
        #endregion

        #region Cell Paint handling - DrawTriStateButton
        public static void DrawTriStateButton(object sender, DataGridViewCellPaintingEventArgs e, TriState triState)
        {
            switch (triState)
            {
                case TriState.Unchange:
                    DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_partial);
                    break;
                case TriState.SomethingAddedAndDeleted:
                    DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_add_delete);
                    break;
                case TriState.AllAdded:
                    DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_on_add_all);
                    break;
                case TriState.AllDeleted:
                    DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_on_delete_all);
                    break;
                case TriState.SomethingAdded:
                    DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_on_add);
                    break;
                case TriState.SomethingDeleded:
                    DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_off_remove);
                    break;
            }
        }
        #endregion

        #region Cell Paint handling - DrawIconsMetadataBrokerTypes
        public static void DrawIconsMetadataBrokerTypes(object sender, DataGridViewCellPaintingEventArgs e, MetadataBrokerType metadataBrokerTypes)
        {
            if ((metadataBrokerTypes & MetadataBrokerType.ExifTool) != 0)
            {
                Image image = (Image)Properties.Resources.tag_source_exiftool;
                e.Graphics.DrawImage(image,
                           e.CellBounds.Left + e.CellBounds.Width - image.Width - 1 - 33,
                           e.CellBounds.Top + 1); // e.CellBounds.Width, e.CellBounds.Height);
            }

            if ((metadataBrokerTypes & MetadataBrokerType.WindowsLivePhotoGallery) != 0)
            {
                Image image = (Image)Properties.Resources.tag_source_windows_live_photo_gallery;
                e.Graphics.DrawImage(image,
                           e.CellBounds.Left + e.CellBounds.Width - image.Width - 1 - 33 - 21,
                           e.CellBounds.Top + 1); // e.CellBounds.Width, e.CellBounds.Height);
            }

            if ((metadataBrokerTypes & MetadataBrokerType.MicrosoftPhotos) != 0)
            {
                Image image = (Image)Properties.Resources.tag_source_microsoft_photos;
                e.Graphics.DrawImage(image,
                           e.CellBounds.Left + e.CellBounds.Width - image.Width - 1 - 33 - 21 - 21,
                           e.CellBounds.Top + 1); // e.CellBounds.Width, e.CellBounds.Height);
            }

            if ((metadataBrokerTypes & MetadataBrokerType.WebScraping) != 0)
            {
                Image image = (Image)Properties.Resources.tag_source_webscraping;
                e.Graphics.DrawImage(image,
                           e.CellBounds.Left + e.CellBounds.Width - image.Width - 1 - 33 - 21 - 21 - 21,
                           e.CellBounds.Top + 1); // e.CellBounds.Width, e.CellBounds.Height);
            }

        }
        #endregion

        #region Cell Paint handling - CalulateCellRoundedRectangleCellBounds
        public static Rectangle CalulateCellRoundedRectangleCellBounds(Rectangle rectangle)
        {
            return new Rectangle(rectangle.Left + 2, rectangle.Top + 2, rectangle.Width - 8, rectangle.Height - 6);
        }
        #endregion

        #region Cell Paint handling - CalulateCellImageSizeInRectagleWithUpScale
        public static Size CalulateCellImageSizeInRectagleWithUpScale(Rectangle rectangleRoundedCellBounds, Size imageSize)
        {
            
            int thumbnailSpace = 4;

            float f = System.Math.Max(
                    (float)imageSize.Width / ((float)rectangleRoundedCellBounds.Width - (thumbnailSpace * 2f)),
                    (float)imageSize.Height / ((float)rectangleRoundedCellBounds.Height - (thumbnailSpace * 2f)));

            //if (f < 1.0f) f = 1.0f; // Do not upsize small images
            return new Size ( (int)System.Math.Round((float)imageSize.Width / f), (int)System.Math.Round((float)imageSize.Height / f));
        }
        #endregion

        #region Cell Paint handling - CalulateCellImageCenterInRectagle
        public static Rectangle CalulateCellImageCenterInRectagle(Rectangle rectangleRoundedCellBounds, Size thumbnailSize)
        {
            return new Rectangle(rectangleRoundedCellBounds.X + ((rectangleRoundedCellBounds.Width - thumbnailSize.Width) / 2),
                rectangleRoundedCellBounds.Y + ((rectangleRoundedCellBounds.Height - thumbnailSize.Height) / 2),
                thumbnailSize.Width, thumbnailSize.Height);
        }
        #endregion

        #region Cell Paint handling - GetRectangleFromMouseCoorinate
        public static Rectangle GetRectangleFromMouseCoorinate(int x1, int y1, int x2, int y2)
        {
            int x = Math.Min(x1, x2);
            int y = Math.Min(y1, y2);
            int width = Math.Max(x1, x2) - x;
            int height = Math.Max(y1, y2) - y;
            return new Rectangle(x, y, width, height);
        }
        #endregion

        #region Cell Paint handling - IsMouseWithinRectangle 
        public static bool IsMouseWithinRectangle(int x, int y, Rectangle rectangle)
        {
            return (x >= rectangle.X && y >= rectangle.Y &&
                    x <= rectangle.X + rectangle.Width && y <= rectangle.Y + rectangle.Height);
        }
        #endregion

        #region Cell Paint handling - CellPaintingColumnHeaderMouseRegion
        public static void CellPaintingColumnHeaderMouseRegion(object sender, DataGridViewCellPaintingEventArgs e, bool drawingRegion, int x1, int y1, int x2, int y2)
        {
            if (!drawingRegion) return;
            Rectangle rectangle = GetRectangleFromMouseCoorinate(x1, y1, x2, y2);
            e.Graphics.DrawRectangle(new Pen(Color.White, 1), e.CellBounds.X + rectangle.X, e.CellBounds.Y + rectangle.Y, rectangle.Width, rectangle.Height);
        }
        #endregion

        #region Cell Paint Handling - UpdateSelectedCellsWithNewRegion
        public static bool UpdateSelectedCellsWithNewRegion(DataGridView dataGridView, int columnIndex, RectangleF region)
        {
            bool updated = false;

            ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);

            foreach (DataGridViewCell cells in dataGridView.SelectedCells)
            {
                if (cells.ColumnIndex == columnIndex)
                {
                    RegionStructure regionStructure = GetCellRegionStructure(dataGridView, cells.ColumnIndex, cells.RowIndex);
                    if (regionStructure == null)
                    {
                        DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, cells.RowIndex);
                        if (dataGridViewGenericRow != null)
                        {
                            if (!dataGridViewGenericRow.IsHeader)
                            {

                                SetCellDataGridViewGenericCell(dataGridView, cells.ColumnIndex, cells.RowIndex,
                                    new DataGridViewGenericCell(new RegionStructure(),
                                    new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.On, false)), false);

                                regionStructure = GetCellRegionStructure(dataGridView, cells.ColumnIndex, cells.RowIndex);
                                regionStructure.Name = dataGridViewGenericRow.RowName;
                            }
                        }
                    }

                    SetCellStatusSwichStatus(dataGridView, cells.ColumnIndex, cells.RowIndex, SwitchStates.On);
                    if (regionStructure != null)
                    {
                        regionStructure = new RegionStructure(regionStructure);
                        regionStructure.AreaX = region.X;
                        regionStructure.AreaY = region.Y;
                        regionStructure.AreaWidth = region.Width;
                        regionStructure.AreaHeight = region.Height;
                        regionStructure.RegionStructureType = RegionStructureTypes.WindowsLivePhotoGallery;
                        updated = true;
                        regionStructure.Thumbnail = null;
                    }
                    SetCellValue(dataGridView, cells.ColumnIndex, cells.RowIndex, regionStructure, true); //This will set IsDirty flag
                }
            }
            
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);

            return updated;
        }
        #endregion 

        #region Cell Paint handling - UpdateSelectedCellsWithNewMouseRegion 
        public static bool UpdateSelectedCellsWithNewMouseRegion(DataGridView dataGridView, int columnIndex, int x1, int y1, int x2, int y2)
        {
            bool updated = false;
            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null) return updated;
            if (dataGridViewGenericColumn.ReadWriteAccess != ReadWriteAccess.AllowCellReadAndWrite) return updated;

            lock (dataGridViewGenericColumn._ThumbnailLock)
            {
                Image image = dataGridViewGenericColumn.thumbnailUnlock;

                Rectangle rectangleRoundedCellBounds = CalulateCellRoundedRectangleCellBounds(
                    new Rectangle(0, 0, dataGridView.Columns[columnIndex].Width, dataGridView.ColumnHeadersHeight));
                Size thumbnailSize = CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);
                Rectangle rectangleCenterThumbnail = CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);

                Rectangle rectangleMouse = GetRectangleFromMouseCoorinate(x1, y1, x2, y2);
                Rectangle rectangleMouseThumb = new Rectangle(rectangleMouse.X - rectangleCenterThumbnail.X, rectangleMouse.Y - rectangleCenterThumbnail.Y, rectangleMouse.Width, rectangleMouse.Height);
                RectangleF region = RegionStructure.CalculateImageRegionAbstarctRectangle(thumbnailSize, rectangleMouseThumb, RegionStructureTypes.WindowsLivePhotoGallery);

                updated = UpdateSelectedCellsWithNewRegion(dataGridView, columnIndex, region);

            }
            return updated;
        }
        #endregion

        #region Cell Paint handling - CellPaintingColumnHeaderRegionsInThumbnail 
        public static void CellPaintingColumnHeaderRegionsInThumbnail(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
                if (dataGridViewGenericColumn == null) return;

                lock (dataGridViewGenericColumn._ThumbnailLock)
                {
                    Image image = dataGridViewGenericColumn.thumbnailUnlock;
                    if (image != null)
                    {

                        Rectangle rectangleRoundedCellBounds = CalulateCellRoundedRectangleCellBounds(e.CellBounds);
                        Size thumbnailSize = CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);

                        for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                        {
                            MetadataLibrary.RegionStructure region = DataGridViewHandler.GetCellRegionStructure(dataGridView, e.ColumnIndex, rowIndex);
                            if (region != null)
                            {
                                Rectangle rectangleCenterThumbnail = CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);
                                e.Graphics.DrawRectangle(new Pen(Color.Black, 1), rectangleCenterThumbnail.X, rectangleCenterThumbnail.Y, rectangleCenterThumbnail.Width, rectangleCenterThumbnail.Height);

                                Rectangle rectangleRegion = region.GetImageRegionPixelRectangle(thumbnailSize);

                                e.Graphics.DrawRectangle(new Pen(Color.White, 1), rectangleCenterThumbnail.X + rectangleRegion.X, rectangleCenterThumbnail.Y + rectangleRegion.Y, rectangleRegion.Width, rectangleRegion.Height);

                                if (dataGridView[e.ColumnIndex, rowIndex].Selected) e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)),
                                    rectangleCenterThumbnail.X + rectangleRegion.X, rectangleCenterThumbnail.Y + rectangleRegion.Y, rectangleRegion.Width, rectangleRegion.Height);

                            }

                        }
                    }
                }
            }
        }
        #endregion

        #region Cell Paint handling - CellPaintingHandleDefault
        public static void CellPaintingHandleDefault(object sender, DataGridViewCellPaintingEventArgs e, bool paintHeaderRow)
        {
            //try
            //{                
            //    //It's already paited of Krypton if (paintHeaderRow || e.ColumnIndex == -1 || e.RowIndex > -1) e.Paint(e.ClipBounds, DataGridViewPaintParts.All); 
            //} catch (Exception ex)
            //{
            //    Logger.Error(ex.Message);    
            //}
            e.Handled = true;
        }
        #endregion

        #region Cell Paint handling - CellPaintingFavoriteAndToolTipsIcon
        public static void CellPaintingFavoriteAndToolTipsIcon(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex > -1 && e.ColumnIndex == -1)
            {
                if (dataGridView.Rows[e.RowIndex].HeaderCell.Tag is DataGridViewGenericRow dataGridViewGenericRow && dataGridViewGenericRow.IsFavourite)
                {
                    DrawIcon16x16OnLeftSide(sender, e, global::DataGridViewGeneric.Properties.Resources.Favorite);
                }
                if (!string.IsNullOrWhiteSpace(dataGridView.Rows[e.RowIndex].HeaderCell.ToolTipText))
                {
                    DrawIcon16x16OnRightSide(sender, e, global::DataGridViewGeneric.Properties.Resources.ToolTipsText);
                }
            } else if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (!string.IsNullOrWhiteSpace(dataGridView[e.ColumnIndex, e.RowIndex].ToolTipText))
                {
                    DrawIcon16x16OnRightSide(sender, e, global::DataGridViewGeneric.Properties.Resources.ToolTipsText);
                }
            }
            
        }
        #endregion

        #region Cell Paint handling - CellPaintingColumnHeader
        public static void CellPaintingColumnHeader(object sender, DataGridViewCellPaintingEventArgs e, Dictionary<string, string> errorFileEntries)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            if (e.RowIndex == -1 && e.ColumnIndex >= 0 && dataGridView.Columns[e.ColumnIndex].Tag is DataGridViewGenericColumn dataGridViewGenericColumn)
            {
                FileEntryAttribute fileEntryAttributeColumn = dataGridViewGenericColumn.FileEntryAttribute;

                string cellTextTop = "";
                string cellTextBottom = "";
                bool hasFileKnownErrors = errorFileEntries.ContainsKey(fileEntryAttributeColumn.FileEntry.FileFullPath);

                if (hasFileKnownErrors)
                {
                    dataGridView.Columns[e.ColumnIndex].ToolTipText = errorFileEntries[fileEntryAttributeColumn.FileEntry.FileFullPath] + "\r\n";
                    dataGridView.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = ColorBackHeaderError(dataGridView);
                }
                else if (dataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning)
                {
                    dataGridView.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = ColorBackHeaderWarning(dataGridView);
                }
                else
                {
                    dataGridView.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = ColorBackHeaderImage(dataGridView);
                }

                if (dataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning)
                    cellTextTop += "File updated!!\r\n";

                switch (GetDataGridSizeLargeMediumSmall(dataGridView))
                {
                    case DataGridViewSize.Small: //Small
                        cellTextTop += fileEntryAttributeColumn.LastWriteDateTime.ToString() + "\r\n";
                        cellTextTop += FileEntryVersionHandler.ToStringShort(fileEntryAttributeColumn.FileEntryVersion);
                        cellTextBottom += fileEntryAttributeColumn.FileName;
                        break;
                    case DataGridViewSize.Medium: //Medium                        
                        cellTextBottom += fileEntryAttributeColumn.LastWriteDateTime.ToString() + "\r\n" + fileEntryAttributeColumn.FileName;
                        cellTextTop += FileEntryVersionHandler.ToStringShort(fileEntryAttributeColumn.FileEntryVersion);
                        break;
                    case DataGridViewSize.Large: //Large
                        cellTextBottom += fileEntryAttributeColumn.LastWriteDateTime.ToString() + "\r\n" + fileEntryAttributeColumn.FileFullPath;
                        cellTextTop += FileEntryVersionHandler.ToStringShort(fileEntryAttributeColumn.FileEntryVersion);
                        break;
                    default:
                        throw new Exception("Not implemented");
                }
                
                lock (dataGridViewGenericColumn._ThumbnailLock)
                {
                    Image image = dataGridViewGenericColumn.thumbnailUnlock;
                    if (image == null) image = (Image)Properties.Resources.load_image;
                    DrawImageAndSubText(sender, e, image, cellTextTop, cellTextBottom);
                    if (dataGridViewGenericColumn.IsDirty) DrawIcon16x16OnLeftSide(sender, e, (Image)Properties.Resources.EditPencil);
                }
            }
            
        }
        #endregion

        #region Cell Paint handling - CellPaintingTriState
        public static void CellPaintingTriState(object sender, DataGridViewCellPaintingEventArgs e, DataGridView dataGridView, string header)
        {
            DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
            if (gridViewGenericDataRow == null) return; //Don't paint anything TriState on "New Empty Row" for "new Keywords"
            if (e.ColumnIndex >= dataGridView.ColumnCount) 
                return; //DEBUG - this should not happen

            DataGridViewGenericColumn dataGridViewGenericDataColumn = null;
            if (e.ColumnIndex > -1)
            {
                dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
                if (dataGridViewGenericDataColumn == null) return; //Data is not set, no point to check more.
                if (dataGridViewGenericDataColumn.Metadata == null) return; //Don't paint TriState button when MetaData is null (data not loaded)
            }
           
            //If keywords row
            if (gridViewGenericDataRow.HeaderName.Equals(header))
            {
                int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, header);

                if (e.ColumnIndex > -1) //Row title clicked, Don't show icon over inputbox
                {
                    if (!gridViewGenericDataRow.IsHeader) DataGridViewHandler.DrawIconsMetadataBrokerTypes(sender, e, DataGridViewHandler.GetCellStatusMetadataBrokerType(dataGridView, e.ColumnIndex, e.RowIndex));
                    // Don't paint TriState botton on read only files (hirstorical files)
                    if (dataGridViewGenericDataColumn.ReadWriteAccess == ReadWriteAccess.ForceCellToReadOnly) return;
                }


                //Tag name column - paint - do not nothing on input text boxes
                if (gridViewGenericDataRow.IsHeader && e.ColumnIndex == -1) //Row title clicked, Don't show icon over inputbox
                {
                    DataGridViewHandler.DrawTriStateButton(sender, e, DataGridViewHandler.GetAllTriState(dataGridView, header, keywordsStarts));
                    e.Handled = true;
                }
                //Tag name column - paint a tristate buttons 
                else if (!gridViewGenericDataRow.IsHeader && e.ColumnIndex == -1)
                {
                    DataGridViewHandler.DrawTriStateButton(sender, e, DataGridViewHandler.GetRowTriState(dataGridView, header, e.RowIndex));
                    e.Handled = true;
                }
                //Tag heading - paint a tristate buttons 
                else if (gridViewGenericDataRow.IsHeader && e.ColumnIndex > -1) //Tag heading, on given column
                {

                    DataGridViewHandler.DrawTriStateButton(sender, e, DataGridViewHandler.GetColumnTriState(dataGridView, header, keywordsStarts, e.ColumnIndex));
                    e.Handled = true;
                }
                //Cell - Paint a tristate buttons 
                else if (!gridViewGenericDataRow.IsHeader && e.ColumnIndex > -1)
                {
                    if (dataGridView.RowCount - 1 == e.RowIndex &&
                        dataGridView.AllowUserToAddRows) //Check if ready to add a new line without text
                    {
                        DataGridViewHandler.DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_partial);
                    }
                    else
                    {
                        //CellPaintingSelectedTriState
                        if (dataGridViewGenericDataColumn.Metadata == null)
                        {
                            DataGridViewHandler.DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_partial);
                        }
                        else if (DataGridViewHandler.GetCellStatusSwichStatus(dataGridView, e.ColumnIndex, e.RowIndex) == SwitchStates.Disabled)
                        {
                            //DataGridViewHandler.CellPaintingSelectedTriState(sender, e, (Image)Properties.Resources.tri_state_switch_partial);
                        }
                        else if (DataGridViewHandler.GetCellStatusSwichStatus(dataGridView, e.ColumnIndex, e.RowIndex) == SwitchStates.On)
                        {
                            if ((DataGridViewHandler.GetCellStatusMetadataBrokerType(dataGridView, e.ColumnIndex, e.RowIndex) & MetadataBrokerType.ExifTool) != 0)
                                DataGridViewHandler.DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_on);
                            else
                                DataGridViewHandler.DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_on_add);
                        }
                        else
                        {
                            if ((DataGridViewHandler.GetCellStatusMetadataBrokerType(dataGridView, e.ColumnIndex, e.RowIndex) & MetadataBrokerType.ExifTool) != 0)
                                DataGridViewHandler.DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_off_remove);
                            else
                                DataGridViewHandler.DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_off);
                        }

                        
                    }

                    e.Handled = true;
                }
            }

        }
        #endregion

        #endregion

    }
}
