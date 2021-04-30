using MetadataLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        public static Color ColorReadOnly = SystemColors.GradientInactiveCaption;
        public static Color ColorError = Color.FromArgb(255, 192, 192);
        public static Color ColorFavourite = SystemColors.ControlLight;
        public static Color ColorReadOnlyFavourite = SystemColors.MenuHighlight;
        public static Color ColorHeader = SystemColors.Control;
        public static Color ColorCellEditable = SystemColors.ControlLightLight;
        public static Color ColorHeaderImage = Color.LightSteelBlue;
        public static Color ColorHeaderError = Color.Red;
        public static Color ColorHeaderWarning = Color.Yellow;
        public static Color ColorRegionFace = Color.White;


        private DataGridView dataGridView;

        private System.ComponentModel.IContainer components = null;

        private ContextMenuStrip contextMenuStripDataGridViewGeneric;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItemMapSave;
        private ToolStripMenuItem toggleRowsAsFavouriteToolStripMenuItem;
        private ToolStripMenuItem toggleShowFavouriteRowsToolStripMenuItem;
        private ToolStripMenuItem toggleHideEqualRowsValuesToolStripMenuItem;
        private ToolStripMenuItem markAsFavoriteToolStripMenuItem;
        private ToolStripMenuItem removeAsFavoriteToolStripMenuItem;


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
            //Commit changes ASAP, e.g. when SelectedIndexChanged will chnage the ValueChanges event be triggered
            dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            SetDataGridViewDirty(dataGridView, dataGridView.CurrentCell.ColumnIndex);
        }
        #endregion

        #region DataGridView events handling - IsDataGridViewDirty
        public static bool IsDataGridViewDirty(DataGridView dataGridView, int columnIndex)
        {
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null) return false;
            return dataGridViewGenericColumn.IsDirty;
        }
        #endregion

        #region DataGridView events handling - IsDataGridViewDirty
        public static bool IsDataGridViewDirty(DataGridView dataGridView)
        {
            for (int columnIndex = 0; columnIndex < GetColumnCount(dataGridView); columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericColumn != null) if (dataGridViewGenericColumn.IsDirty) return true;
            }
            return false;
        }
        #endregion

        #region DataGridView events handling - ClearDataGridViewDirty
        public static void ClearDataGridViewDirty(DataGridView dataGridView)
        {
            for (int columnIndex = 0; columnIndex < GetColumnCount(dataGridView); columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericColumn != null) dataGridViewGenericColumn.IsDirty = false;
            }
        }
        #endregion

        #region DataGridView events handling - SetDataGridViewDirty
        public static void SetDataGridViewDirty(DataGridView dataGridView, int columnIndex)
        {
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn != null) dataGridViewGenericColumn.IsDirty = true;
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

        public DataGridViewHandler(DataGridView dataGridView, string dataGridViewName, string topLeftHeaderCellName, DataGridViewSize cellSize)
        {
            this.dataGridView = dataGridView;

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

            DataGridViewGenericData dataGridViewGenricData = new DataGridViewGenericData();
            dataGridViewGenricData.TopCellName = topLeftHeaderCellName;
            dataGridViewGenricData.DataGridViewName = dataGridViewName;
            dataGridViewGenricData.FavoriteList = FavouriteRead(CreateFavoriteFilename(dataGridViewGenricData.DataGridViewName));
            dataGridViewGenricData.CellSize = cellSize;
            dataGridView.TopLeftHeaderCell.Tag = dataGridViewGenricData;

            dataGridView.CellValueNeeded += DataGridView_CellValueNeeded;
            dataGridView.CellValuePushed += DataGridView_CellValuePushed;
            dataGridView.NewRowNeeded += DataGridView_NewRowNeeded;
            dataGridView.RowValidated += DataGridView_RowValidated;
            dataGridView.RowDirtyStateNeeded += DataGridView_RowDirtyStateNeeded;
            dataGridView.CancelRowEdit += DataGridView_CancelRowEdit;
            dataGridView.UserDeletingRow += DataGridView_UserDeletingRow;
            dataGridView.CellMouseDown += DataGridView_CellMouseDown;
            dataGridView.CurrentCellDirtyStateChanged += DataGridView_CurrentCellDirtyStateChanged;


            if (dataGridView.ContextMenuStrip == null)
            {
                InitializeComponent(this.dataGridView);
                dataGridView.ContextMenuStrip = contextMenuStripDataGridViewGeneric;
            }
        }
        #endregion

        #region DataGridView Handling - Clear
        public static void Clear(DataGridView dataGridView, DataGridViewSize cellSize)
        {
            DataGridViewGenericData dataGridViewGenricData = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;

            ClipboardUtility.Clear(dataGridView);
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.TopLeftHeaderCell.Value = dataGridViewGenricData.TopCellName;
            dataGridView.EnableHeadersVisualStyles = false;

            dataGridView.ColumnHeadersHeight = GetTopColumnHeaderHeigth(cellSize);
            //dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView.RowHeadersWidth = GetFirstRowHeaderWidth(cellSize);
            //dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        }
        #endregion

        #region DataGridView Handling - InitializeComponent
        public void InitializeComponent(DataGridView dataGridView)
        {
            components = new System.ComponentModel.Container();
            contextMenuStripDataGridViewGeneric = new System.Windows.Forms.ContextMenuStrip(components);
            cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItemMapSave = new System.Windows.Forms.ToolStripMenuItem();
            toggleRowsAsFavouriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleShowFavouriteRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleHideEqualRowsValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            markAsFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            removeAsFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            contextMenuStripDataGridViewGeneric.SuspendLayout();
            // 
            // contextMenuStripDataGridViewGeneric
            // 
            contextMenuStripDataGridViewGeneric.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStripDataGridViewGeneric.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            cutToolStripMenuItem,
            copyToolStripMenuItem,
            pasteToolStripMenuItem,
            deleteToolStripMenuItem,
            undoToolStripMenuItem,
            redoToolStripMenuItem,
            findToolStripMenuItem,
            replaceToolStripMenuItem,
            toolStripMenuItemMapSave,
            markAsFavoriteToolStripMenuItem,
            removeAsFavoriteToolStripMenuItem,
            toggleRowsAsFavouriteToolStripMenuItem,
            toggleShowFavouriteRowsToolStripMenuItem,
            toggleHideEqualRowsValuesToolStripMenuItem});
            contextMenuStripDataGridViewGeneric.Name = "contextMenuStripMap";
            contextMenuStripDataGridViewGeneric.Size = new System.Drawing.Size(215, 370);
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Cut;
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            cutToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            cutToolStripMenuItem.Text = "Cut";
            cutToolStripMenuItem.Click += new System.EventHandler(cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Copy;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            copyToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += new System.EventHandler(copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Paste;
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            pasteToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += new System.EventHandler(pasteToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Delete;
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            deleteToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += new System.EventHandler(deleteToolStripMenuItem_Click);
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Undo;
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            undoToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            undoToolStripMenuItem.Text = "Undo";
            undoToolStripMenuItem.Click += new System.EventHandler(undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Redo;
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            redoToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            redoToolStripMenuItem.Text = "Redo";
            redoToolStripMenuItem.Click += new System.EventHandler(redoToolStripMenuItem_Click);
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Find;
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            findToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            findToolStripMenuItem.Text = "Find";
            findToolStripMenuItem.Click += new System.EventHandler(findToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            replaceToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.Replace;
            replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            replaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            replaceToolStripMenuItem.Size = new System.Drawing.Size(302, 26);
            replaceToolStripMenuItem.Text = "Replace";
            replaceToolStripMenuItem.Click += new System.EventHandler(replaceToolStripMenuItem_Click);
            // 
            // toolStripMenuItemMapSave
            // 
            this.toolStripMenuItemMapSave.Image = global::DataGridViewGeneric.Properties.Resources.Save;
            this.toolStripMenuItemMapSave.Name = "toolStripMenuItemMapSave";
            this.toolStripMenuItemMapSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemMapSave.Size = new System.Drawing.Size(240, 26);
            this.toolStripMenuItemMapSave.Text = "Save";
            this.toolStripMenuItemMapSave.Click += new System.EventHandler(this.toolStripMenuItemMapSave_Click);
            // 
            // toggleRowsAsFavouriteToolStripMenuItem
            // 
            toggleRowsAsFavouriteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.FavoriteToggle;
            toggleRowsAsFavouriteToolStripMenuItem.Name = "toggleRowsAsFavouriteToolStripMenuItem";
            toggleRowsAsFavouriteToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            toggleRowsAsFavouriteToolStripMenuItem.Text = "Toggle favorite";
            toggleRowsAsFavouriteToolStripMenuItem.Click += new System.EventHandler(toggleRowsAsFavouriteToolStripMenuItem_Click);
            // 
            // toggleShowFavouriteRowsToolStripMenuItem
            // 
            toggleShowFavouriteRowsToolStripMenuItem.Checked = false;
            toggleShowFavouriteRowsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Unchecked;
            toggleShowFavouriteRowsToolStripMenuItem.Name = "toggleShowFavouriteRowsToolStripMenuItem";
            toggleShowFavouriteRowsToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            toggleShowFavouriteRowsToolStripMenuItem.Text = "Show favorite rows";
            toggleShowFavouriteRowsToolStripMenuItem.Click += new System.EventHandler(toggleShowFavouriteRowsToolStripMenuItem_Click);
            // 
            // toggleHideEqualRowsValuesToolStripMenuItem
            // 
            toggleHideEqualRowsValuesToolStripMenuItem.Checked = false;
            toggleHideEqualRowsValuesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Unchecked;
            toggleHideEqualRowsValuesToolStripMenuItem.Name = "toggleHideEqualRowsValuesToolStripMenuItem";
            toggleHideEqualRowsValuesToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            toggleHideEqualRowsValuesToolStripMenuItem.Text = "Hide equal rows";
            toggleHideEqualRowsValuesToolStripMenuItem.Click += new System.EventHandler(toggleHideEqualRowsValuesToolStripMenuItem_Click);
            // 
            // markAsFavoriteToolStripMenuItem
            // 
            markAsFavoriteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.FavoriteSelect;
            markAsFavoriteToolStripMenuItem.Name = "markAsFavoriteToolStripMenuItem";
            markAsFavoriteToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            markAsFavoriteToolStripMenuItem.Text = "Mark as favorite";
            markAsFavoriteToolStripMenuItem.Click += new System.EventHandler(markAsFavoriteToolStripMenuItem_Click);
            // 
            // removeAsFavoriteToolStripMenuItem
            // 
            removeAsFavoriteToolStripMenuItem.Image = global::DataGridViewGeneric.Properties.Resources.FavoriteRemove;
            removeAsFavoriteToolStripMenuItem.Name = "removeAsFavoriteToolStripMenuItem";
            removeAsFavoriteToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            removeAsFavoriteToolStripMenuItem.Text = "Remove as favorite";
            removeAsFavoriteToolStripMenuItem.Click += new System.EventHandler(removeAsFavoriteToolStripMenuItem_Click);

            contextMenuStripDataGridViewGeneric.ResumeLayout();

        }
        #endregion

        #region DataGridView Handling - GetTopColumnHeaderHeigth
        //DataGridView Size for Column and Row Header, Row / Column size and resize 
        public static int GetTopColumnHeaderHeigth(DataGridViewSize size)
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
                    return 24; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    return 24; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    return 24; //Rename Grid*/
                case DataGridViewSize.ConfigSize:
                    return 24;
                default:
                    throw new Exception("Not implemented");
            }
        }
        #endregion

        #region DataGridView Handling - GetCellHeigth
        public static int GetCellHeigth(DataGridViewSize size)
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
                    return 24; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameConvertAndMergeSize:
                    return 24; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameConvertAndMergeSize:
                    return 24; //Rename Grid*/
                case DataGridViewSize.ConfigSize:
                    return 24;
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

        #region DataGridView Handling - GetCellColumnsWidth
        public static int GetCellColumnsWidth(DataGridView dataGridView)
        {
            return GetCellWidth(GetDataGridSizeLargeMediumSmall(dataGridView));
        }
        #endregion

        #region DataGridView Handling - GetCellRowHeight
        public static int GetCellRowHeight(DataGridView dataGridView)
        {
            return GetCellHeigth(GetDataGridSizeLargeMediumSmall(dataGridView));
        }
        #endregion

        #region DataGridView Handling - SetCellRowHeight
        public static void SetCellRowHeight(DataGridView dataGridView, int rowIndex, int height)
        {
            dataGridView.Rows[rowIndex].Height = height;
        }
        #endregion

        #region DataGridView Handling - GetDataGridSizeLargeMediumSmall
        public static DataGridViewSize GetDataGridSizeLargeMediumSmall(DataGridView dataGridView)
        {
            return GetDataGridViewGenericData(dataGridView)?.CellSize == null ? DataGridViewSize.Medium : GetDataGridViewGenericData(dataGridView).CellSize;
        }
        #endregion

        #region DataGridView Handling - SetCellSize
        public static void SetCellSize(DataGridView dataGridView, DataGridViewSize cellSize, bool changeCellRowsHeight)
        {
            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return;
            dataGridViewGenericData.CellSize = cellSize;

            dataGridView.ColumnHeadersHeight = GetTopColumnHeaderHeigth(cellSize);
            dataGridView.RowHeadersWidth = GetFirstRowHeaderWidth(cellSize);

            if (changeCellRowsHeight)
            {
                for (int rowIndex = 1; rowIndex < dataGridView.RowCount; rowIndex++) dataGridView.Rows[rowIndex].Height = GetCellHeigth(cellSize);
            }
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++) dataGridView.Columns[columnIndex].Width = GetCellWidth(cellSize);
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
        private static int suspendCount = 0;
        private static bool isSuspended = false;
        public static void SuspendLayout(DataGridView dataGridView, int queueSize)
        {
            suspendCount++;
            if (suspendCount > 1) return; //Already suspended


            if (!isSuspended)
            {
                dataGridView.SuspendLayout();

                dataGridViewAutoSizeRowsMode = dataGridView.AutoSizeRowsMode;
                dataGridViewAutoSizeColumnMode = dataGridView.AutoSizeColumnsMode;
                dataGridViewRowHeadersWidthSizeMode = dataGridView.RowHeadersWidthSizeMode;
                dataGridViewColumnHeadersHeightSizeMode = dataGridView.ColumnHeadersHeightSizeMode;

                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                dataGridView.RowHeadersVisible = false;

                SendMessage(dataGridView.Handle, WM_SETREDRAW, false, 0);

                isSuspended = true;
            }



        }
        #endregion 

        #region Suspend and Resume layout - ResumeLayout
        public static bool ResumeLayout(DataGridView dataGridView, int queueSize)
        {
            bool didResume = false;
            suspendCount--;
            if (suspendCount == 0 && isSuspended)
            {
                if (queueSize > 0) return didResume; //Wait new suspend
                dataGridView.AutoSizeRowsMode = dataGridViewAutoSizeRowsMode;
                dataGridView.AutoSizeColumnsMode = dataGridViewAutoSizeColumnMode;
                dataGridView.RowHeadersWidthSizeMode = dataGridViewRowHeadersWidthSizeMode;
                dataGridView.ColumnHeadersHeightSizeMode = dataGridViewColumnHeadersHeightSizeMode;

                dataGridView.RowHeadersVisible = true;
                dataGridView.ResumeLayout();

                SendMessage(dataGridView.Handle, WM_SETREDRAW, true, 0);
                dataGridView.Refresh();
                isSuspended = false;
                didResume = true;
            }
            else if (suspendCount < 0) suspendCount = 0;
            return didResume;
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

        #region Action Handling

        #region Action Handling - Cut
        public static void ActionCut(DataGridView dataGridView)
        {
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }
        #endregion

        #region Action Handling - Copy
        public static void ActionCopy(DataGridView dataGridView)
        {
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
        }
        #endregion

        #region Action Handling - Paste
        public static void ActionPaste(DataGridView dataGridView)
        {
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
        }
        #endregion

        #region Action Handling - Delete
        public static void ActionDelete(DataGridView dataGridView)
        {
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }
        #endregion

        #region Action Handling - Undo
        public static void ActionUndo(DataGridView dataGridView)
        {
            ClipboardUtility.UndoDataGridView(dataGridView);
        }
        #endregion

        #region Action Handling - Redo
        public static void ActionRedo(DataGridView dataGridView)
        {
            ClipboardUtility.RedoDataGridView(dataGridView);
        }
        #endregion

        #region Action Handling - ActionFindAndReplace
        static FindAndReplaceForm m_FindAndReplaceForm;

        public static void ActionFindAndReplace(DataGridView dataGridView, bool replaceTab)
        {
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

        #region Column handling - GetColumnSelected
        public static List<int> GetColumnSelected(DataGridView dataGridView)
        {
            List<int> selectedColumns = new List<int>();

            foreach (DataGridViewColumn dataGridViewColumn in dataGridView.SelectedColumns)
            {
                if (!selectedColumns.Contains(dataGridViewColumn.Index)) selectedColumns.Add(dataGridViewColumn.Index);
            }

            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                if (!selectedColumns.Contains(dataGridViewCell.ColumnIndex)) selectedColumns.Add(dataGridViewCell.ColumnIndex);
            }
            return selectedColumns;
        }
        #endregion

        #region Column handling - IstColumnSelected
        public static bool IsColumnSelected(DataGridView dataGridView, int columnIndex)
        {
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

        #region Column handling - GetColumnCount
        public static int GetColumnCount(DataGridView dataGridView)
        {
            return dataGridView.ColumnCount;
        }
        #endregion

        #region Column handling - DoesColumnFilenameExist
        public static bool DoesColumnFilenameExist(DataGridView dataGridView, string fullFilePath)
        {
            return GetColumnIndex(dataGridView, fullFilePath) != -1;
        }
        #endregion

        #region Column handling - GetColumnIndex - fullFilePath
        public static int GetColumnIndex(DataGridView dataGridView, string fullFilePath)
        {
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn column && column.FileEntryAttribute.FileFullPath == fullFilePath)
                {
                    return columnIndex;
                }
            }
            return -1; //Not found
        }
        #endregion

        #region Column handling - GetColumnIndex - fileEntry
        public static int GetColumnIndex(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute)
        {
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn column && column.FileEntryAttribute == fileEntryAttribute)
                {
                    return columnIndex;
                }
            }
            return -1; //Not found
        }
        #endregion

        #region Column handling - GetColumnDataGridViewGenericColumnList
        public static List<DataGridViewGenericColumn> GetColumnDataGridViewGenericColumnList(DataGridView dataGridView, bool onlyReadWriteAccessColumn)
        {
            List<DataGridViewGenericColumn> dataGridViewGenericColumnList = new List<DataGridViewGenericColumn>();

            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                if (dataGridViewGenericColumn != null)
                {
                    if (!onlyReadWriteAccessColumn || (onlyReadWriteAccessColumn && dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite))
                        dataGridViewGenericColumnList.Add(dataGridViewGenericColumn);
                }

            }
            return dataGridViewGenericColumnList;
        }
        #endregion

        #region Column handling - AddColumnOrUpdate 
        public static int AddColumnOrUpdateNew(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, Image thumbnail, Metadata metadata,
            ReadWriteAccess readWriteAccessForColumn, ShowWhatColumns showWhatColumns, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault)
        {
            int columnIndex = GetColumnIndex(dataGridView, fileEntryAttribute); //Find column Index for Filename and date last written
            bool isErrorColumn = fileEntryAttribute.FileEntryVersion == FileEntryVersion.Error;
            bool showErrorColumns = (showWhatColumns & ShowWhatColumns.ErrorColumns) > 0;
            bool isHistoryColumn = (fileEntryAttribute.FileEntryVersion == FileEntryVersion.Historical);
            bool showHirstoryColumns = (showWhatColumns & ShowWhatColumns.HistoryColumns) > 0;

            bool isMetadataAlreadyAgregated = false;

            if (columnIndex < 0) //Column not found, add a new column
            {
                //Do not add columns that is not visible //Check if error column first, can be historical, and error
                if (isErrorColumn)
                {
                    if (!showErrorColumns) return -1;
                } else if (!showHirstoryColumns && isHistoryColumn) return -1;


                DataGridViewColumn dataGridViewColumn = new DataGridViewColumn(new DataGridViewTextBoxCell());
                dataGridViewColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewColumn.MinimumWidth = 40;

                dataGridViewColumn.Width = GetCellColumnsWidth(dataGridView);

                dataGridViewColumn.ToolTipText = fileEntryAttribute.LastWriteDateTime.ToString() + "\r\n" + fileEntryAttribute.FileFullPath;
                dataGridViewColumn.Tag = new DataGridViewGenericColumn(fileEntryAttribute, thumbnail, metadata, readWriteAccessForColumn);

                dataGridViewColumn.Name = fileEntryAttribute.FileFullPath;
                dataGridViewColumn.HeaderText = fileEntryAttribute.FileFullPath;

                int columnIndexFilename = GetColumnIndex(dataGridView, fileEntryAttribute.FileFullPath);
                if (columnIndexFilename == -1) //Filename doesn't exist
                {
                    columnIndex = dataGridView.Columns.Add(dataGridViewColumn); //Filename doesn't exist add to end.
                }
                else
                {
                    //Short, newst always first
                    while (columnIndexFilename < dataGridView.Columns.Count &&
                        dataGridView.Columns[columnIndexFilename].Tag is DataGridViewGenericColumn column &&
                        column.FileEntryAttribute.FileFullPath == fileEntryAttribute.FileFullPath &&            //Correct filename on column
                        (fileEntryAttribute.FileEntryVersion != FileEntryVersion.Current &&                     //Current version added, then find correct postion
                        (column.FileEntryAttribute.FileEntryVersion == FileEntryVersion.Current ||              //Edit version, move to next column -> edit always first
                        column.FileEntryAttribute.LastWriteDateTime > fileEntryAttribute.LastWriteDateTime)     //Is older, move next -> Newst always frist
                        ))
                    {
                        columnIndexFilename += 1;
                    }

                    //Move error and historical version back
                    if (columnIndexFilename < dataGridView.Columns.Count - 1 &&
                        dataGridView.Columns[columnIndexFilename].Tag is DataGridViewGenericColumn column2 &&
                        column2.FileEntryAttribute.FileFullPath == fileEntryAttribute.FileFullPath &&           //Correct filename on column
                        (fileEntryAttribute.FileEntryVersion != FileEntryVersion.Current &&                     //History or Error column added, then find correct postion
                        (column2.FileEntryAttribute.FileEntryVersion == FileEntryVersion.Current ||             //Edit version, move to next column -> edit always first
                        column2.FileEntryAttribute.LastWriteDateTime > fileEntryAttribute.LastWriteDateTime)    //Is older, move next -> Newst always frist
                        )
                        )
                    {
                        columnIndexFilename += 1;
                    }

                    columnIndex = columnIndexFilename;

                    dataGridView.Columns.Insert(columnIndex, dataGridViewColumn);
                }

                SetCellStatusDefaultColumnWhenAdded(dataGridView, columnIndex, dataGridViewGenericCellStatusDefault);
                SetCellBackgroundColorForColumn(dataGridView, columnIndex);
            }
            else
            {
                DataGridViewGenericColumn currentDataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                if (currentDataGridViewGenericColumn == null)
                {
                    //Why do this happend
                    currentDataGridViewGenericColumn = new DataGridViewGenericColumn(fileEntryAttribute, thumbnail, metadata, readWriteAccessForColumn);
                    
                }
                //else if (currentDataGridViewGenericColumn.Metadata == null)
                //{
                //    currentDataGridViewGenericColumn.Metadata = metadata;
                //}
                //else
                //{
                //New data has arrived for Edit Column
                if (metadata != null && currentDataGridViewGenericColumn.Metadata != null && !isHistoryColumn)
                {
                    if (IsDataGridViewDirty(dataGridView, columnIndex)) //That means, data was changed by user and trying to make changes to "past"
                    {
                        //Check if old file, due to User click "reload metadata", then newest version has become older that current
                        if (metadata.FileDateModified <= currentDataGridViewGenericColumn.Metadata.FileDateModified)
                        {
                            isMetadataAlreadyAgregated = true; //Do not refresh, due to old file, do not overwrite
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warning needed
                        }
                        else if (metadata.FileDateModified == currentDataGridViewGenericColumn.Metadata.FileDateModified)
                        {
                            isMetadataAlreadyAgregated = true; //Do not refresh, same file is loaded, do not overwrite
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warning needed
                        }
                        else
                        {
                            isMetadataAlreadyAgregated = true; //Do not refresh, due to DataGrid are changed by user, do not overwrite
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = true; //Warn, new files can't be shown
                        }
                    }
                    else
                    {
                        //Check if old file, due to User click "reload metadata", then newest version has become older that current
                        if (metadata.FileDateModified < currentDataGridViewGenericColumn.Metadata.FileDateModified)
                        {
                            isMetadataAlreadyAgregated = true; //Do not refresh, due to old file, do not overwrite
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warning needed
                        }
                        else if (metadata.FileDateModified == currentDataGridViewGenericColumn.Metadata.FileDateModified)
                        {
                            isMetadataAlreadyAgregated = true; //Do not refresh, due to equal file, do not overwrite
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warning needed
                        }
                        else
                        {
                            isMetadataAlreadyAgregated = false; //Refresh with newst data
                            currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false; //No warnings needed, just updated datagrid with new data
                            currentDataGridViewGenericColumn.Metadata = metadata; //Keep newest version
                        }
                    }
                }
                //metadata != null && currentDataGridViewGenericColumn.Metadata != null && !isHistoryColumn
                else if (currentDataGridViewGenericColumn.Metadata != null && metadata != null)   
                {
                    isMetadataAlreadyAgregated = false;
                    currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false;
                    //currentDataGridViewGenericColumn.Metadata = metadata; //Keep this version
                }
                else if (currentDataGridViewGenericColumn.Metadata != null && metadata == null)
                {
                    isMetadataAlreadyAgregated = true;
                    currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false;
                    //currentDataGridViewGenericColumn.Metadata = metadata; //Keep this version
                }
                //When e.g. 
                else //if (currentDataGridViewGenericColumn.Metadata == null )
                {
                    isMetadataAlreadyAgregated = false;
                    currentDataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning = false;
                    currentDataGridViewGenericColumn.Metadata = metadata;
                    
                }

                if (currentDataGridViewGenericColumn.FileEntryAttribute != fileEntryAttribute) 
                    currentDataGridViewGenericColumn.FileEntryAttribute = fileEntryAttribute;
                                                                                               
                currentDataGridViewGenericColumn.Thumbnail = thumbnail;
                currentDataGridViewGenericColumn.ReadWriteAccess = readWriteAccessForColumn;
                dataGridView.Columns[columnIndex].Tag = currentDataGridViewGenericColumn;

                SetCellBackgroundColorForColumn(dataGridView, columnIndex);

                //Hide and show columns
                if (isErrorColumn) //Check if error column first, can be historical, and error
                {
                    if (showErrorColumns)
                        dataGridView.Columns[columnIndex].Visible = true;
                    else
                        dataGridView.Columns[columnIndex].Visible = false;
                }
                else if (isHistoryColumn)
                {
                    if (showHirstoryColumns)
                        dataGridView.Columns[columnIndex].Visible = true;
                    else
                        dataGridView.Columns[columnIndex].Visible = false;
                }
                else dataGridView.Columns[columnIndex].Visible = true;
            }

            if (isMetadataAlreadyAgregated) return -1;
            else return columnIndex;
        }
        #endregion

        #region Column handling - GetColumnDataGridViewGenericColumn
        public static DataGridViewGenericColumn GetColumnDataGridViewGenericColumn(DataGridView dataGridView, int columnIndex)
        {
            if (columnIndex < 0) return null;
            return dataGridView.Columns[columnIndex].Tag as DataGridViewGenericColumn;
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
                        var value = dataGridView[columnIndex, rowIndex].Value;
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
            if (dataGridView.AllowUserToAddRows)
                return dataGridView.RowCount - 1;
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
        public static int GetRowIndex(DataGridView dataGridView, DataGridViewGenericRow dataGridViewGenericRow)
        {
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                if (dataGridView.Rows[rowIndex].HeaderCell.Tag is DataGridViewGenericRow dataGridViewGenericRowCheck &&
                    dataGridViewGenericRowCheck == dataGridViewGenericRow) return rowIndex;
            }
            return -1;
        }
        #endregion

        #region Rows handling - GetRowIndex
        public static int GetRowIndex(DataGridView dataGridView, string headerName)
        {
            return GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName));
        }
        #endregion

        #region Rows handling - GetRowIndex
        public static int GetRowIndex(DataGridView dataGridView, string headerName, string rowName)
        {
            return GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
        }
        #endregion

        #region Row handling - DeleteRow
        public static bool DeleteRow(DataGridView dataGridView, string headerName, string rowName)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
            if (rowIndex > -1) dataGridView.Rows.RemoveAt(rowIndex);
            return rowIndex != -1;
        }
        #endregion

        #region Rows handling - AddRowAndValueList
        public static void AddRowAndValueList(DataGridView dataGridView, FileEntryAttribute fileEntryColumn, List<DataGridViewGenericRowAndValue> dataGridViewGenericRowAndValueList, bool sort)
        {
            int columnIndex = GetColumnIndex(dataGridView, fileEntryColumn);

            foreach (DataGridViewGenericRowAndValue dataGridViewGenericRowAndValue in dataGridViewGenericRowAndValueList)
            {
                int rowIndex = AddRow(dataGridView, columnIndex, dataGridViewGenericRowAndValue.DataGridViewGenericRow,
                    GetFavoriteList(dataGridView),
                    dataGridViewGenericRowAndValue.DataGridViewGenericCell.Value,
                    dataGridViewGenericRowAndValue.DataGridViewGenericCell.CellStatus, 0, true, sort);

                //if (rowIndex > -1) SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericRowAndValue.DataGridViewGenericCell.CellStatus);
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

        #region SetCellDefaultAfterUpdated
        public static void SetCellDefaultAfterUpdated(DataGridView dataGridView, DataGridViewGenericCellStatus dataGridViewGenericCellStatus, int columnIndex, int rowIndex)
        {
            DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
            DataGridViewHandler.SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
            //DataGridViewHandler.SetRowFavoriteFlag(dataGridView, rowIndexUsed, dataGridFavorites);
            DataGridViewHandler.SetCellBackGroundColor(dataGridView, columnIndex, rowIndex);
        }
        #endregion

        #region SetCellDefaultAfterUpdated
        public static void SetCellDefaultAfterUpdated(DataGridView dataGridView, MetadataBrokerType metadataBrokerType, int columnIndex, int rowIndex)
        {
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex)); //Remember current status, in case of updates
            dataGridViewGenericCellStatus.MetadataBrokerTypes |= metadataBrokerType;
            if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Disabled) dataGridViewGenericCellStatus.SwitchState = SwitchStates.Undefine;
            if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Undefine)
                dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerTypes & MetadataBrokerType.ExifTool) == MetadataBrokerType.ExifTool ? SwitchStates.On : SwitchStates.Off;
            dataGridViewGenericCellStatus.CellReadOnly = false;
            SetCellDefaultAfterUpdated(dataGridView, dataGridViewGenericCellStatus, columnIndex, rowIndex);
        }
        #endregion

        #region Rows handling - AddRow
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="columnIndex"></param>
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
                else
                {
                    //When dataGridView is still empty, or got cleaned: Why does thus occure
                }

            }

            bool isValueUpdated = false;
            //If a value row, set the value
            if (!dataGridViewGenericRow.IsHeader)
            {
                if (writeValue)
                {
                    if (dataGridView[columnIndex, rowIndex].Value != value)
                    {
                        isValueUpdated = true;
                        dataGridView[columnIndex, rowIndex].Value = value;
                    }
                }
                if (isValueUpdated && dataGridViewGenericRow.IsMultiLine) dataGridView.Columns[columnIndex].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            else dataGridViewGenericCellStatusDefault.CellReadOnly = true;

            SetRowFavoriteFlag(dataGridView, rowIndex, dataGridFavorites);

            if (columnIndex != -1) //When adding empty row without value in a given column
            {
                //It's only possible to update ReadOnly field
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
                if (dataGridViewGenericCellStatus != null) dataGridViewGenericCellStatus.CellReadOnly = dataGridViewGenericCellStatusDefault.CellReadOnly;
                SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
            }

            SetCellBackGroundColorForRow(dataGridView, rowIndex);


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
            return dataGridViewGenericRow == null ? "" : dataGridViewGenericRow.RowName;
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
            string jsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            if (!Directory.Exists(jsonPath)) Directory.CreateDirectory(jsonPath);
            return Path.Combine(jsonPath, "Favourite." + dataGridViewName + ".json");
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
                    firstValue = dataGridView[columnIndex, rowIndex].Value;
                    fistValueFound = true;
                }
                else
                {
                    if (firstValue == null && dataGridView[columnIndex, rowIndex].Value != null)
                        return true;
                    else if (firstValue != null && dataGridView[columnIndex, rowIndex].Value == null)
                        return true;
                    else if (firstValue == null && dataGridView[columnIndex, rowIndex].Value == null)
                    { } //Do nothing
                    else if (firstValue.ToString() != dataGridView[columnIndex, rowIndex].Value.ToString())
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
            return dataGridView[columnIndex, rowIndex];
        }
        #endregion

        #region Cell Handling - GetCellValue - int columnIndex, string headerName, string rowName
        public static object GetCellValue(DataGridView dataGridView, int columnIndex, string headerName, string rowName)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
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
            return GetCellValue(dataGridView[columnIndex, rowIndex]);
        }
        #endregion

        #region Cell Handling - GetCellValueNullOrStringTrim - int columnIndex, int rowIndex
        public static string GetCellValueNullOrStringTrim(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            if (columnIndex > -1 && rowIndex > -1)
            {
                string value = (dataGridView[columnIndex, rowIndex].Value == null ? null : dataGridView[columnIndex, rowIndex].Value.ToString().Trim());
                if (string.IsNullOrEmpty(value)) return null;
                else return value;
            }
            else return null;
            
        }
        #endregion

        #region Cell Handling - GetCellValueNullOrStringTrim - int columnIndex, string headerName, string rowName
        public static string GetCellValueNullOrStringTrim(DataGridView dataGridView, int columnIndex, string headerName, string rowName)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
            return GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex);
        }
        #endregion

        #region Cell Handling - IsCellNullOrWhiteSpace - int columnIndex, int rowIndex
        public static bool IsCellNullOrWhiteSpace(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView[columnIndex, rowIndex].Value == null || string.IsNullOrWhiteSpace(dataGridView[columnIndex, rowIndex].Value.ToString().Trim());
        }
        #endregion

        #region Cell Handling - SetCellValue - int columnIndex, string headerName, string rowName, object value
        public static void SetCellValue(DataGridView dataGridView, int columnIndex, string headerName, string rowName, object value)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
            SetCellValue(dataGridView, columnIndex, rowIndex, value);
        }
        #endregion

        #region Cell Handling - SetCellValue - int columnIndex, int rowIndex, object value
        public static void SetCellValue(DataGridView dataGridView, int columnIndex, int rowIndex, object value)
        {
            if (rowIndex > -1 && columnIndex > -1) dataGridView[columnIndex, rowIndex].Value = value;
        }
        #endregion

        #region Cell Handling - SetCellValue - int columnIndex, int rowIndex, object value
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
                dataGridViewCell.Value = value;
            }
            catch
            {
            }
        }
        #endregion

        #region Cell Handling - SetCellToolTipText
        public static void SetCellToolTipText(DataGridView dataGridView, int columnIndex, int rowIndex, string toolTipText)
        {
            dataGridView[columnIndex, rowIndex].ToolTipText = toolTipText;
        }
        #endregion

        #region Cell Handling - GetCellToolTipText
        public static string GetCellToolTipText(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView[columnIndex, rowIndex].ToolTipText;
        }
        #endregion

        #region Cell Handling - SetCellControlType
        public static void SetCellControlType(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewCell dataGridViewCell)
        {
            if (rowIndex > -1 && columnIndex > -1) dataGridView[columnIndex, rowIndex] = dataGridViewCell;
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
        public static void SetCellDataGridViewGenericCell(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewGenericCell dataGridViewGenericCell)
        {
            if (rowIndex > -1 && columnIndex > -1)
            {
                SetCellValue(dataGridView, columnIndex, rowIndex, dataGridViewGenericCell.Value);
                SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCell.CellStatus);
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
            return (dataGridView[columnIndex, rowIndex].Tag != null && dataGridView[columnIndex, rowIndex].Tag.GetType() == typeof(DataGridViewGenericCellStatus));
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
            return GetCellStatus(dataGridView[columnIndex, rowIndex]);
        }
        #endregion

        #region Cell Handling - SetCellStatus - int columnIndex, int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatus
        public static void SetCellStatus(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatus)
        {
            if (rowIndex > -1 && columnIndex > -1) dataGridView[columnIndex, rowIndex].Tag = dataGridViewGenericCellStatus;
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
                    SetCellStatus(dataGridView, dataGridCell.ColumnIndex, rowIndex, dataGridViewGenericCellStatusCopy);
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
                    SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatusCopy);
                    SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatusCopy);
                }
            }
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
            //DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
            if (dataGridViewGenericCellStatus != null)
                dataGridView[columnIndex, rowIndex].ReadOnly = dataGridViewGenericCellStatus.CellReadOnly;

            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.ForceCellToReadOnly)
                dataGridView[columnIndex, rowIndex].ReadOnly = true;

            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (dataGridViewGenericRow != null && (
                dataGridViewGenericRow.ReadWriteAccess == ReadWriteAccess.ForceCellToReadOnly))
                dataGridView[columnIndex, rowIndex].ReadOnly = true;
        }
        #endregion

        #region Cell Handling - SetCellBackGroundColor - int columnIndex, int rowIndex
        private static void SetCellBackGroundColor(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

            Color newColor = Color.Empty;

            if (dataGridViewGenericRow == null)
            {
                if (dataGridView[columnIndex, rowIndex].ReadOnly) newColor = ColorReadOnly;
                else newColor = ColorCellEditable;
            }
            else
            {
                if (dataGridViewGenericRow.IsFavourite && !dataGridView[columnIndex, rowIndex].ReadOnly) newColor = ColorFavourite;
                else if (!dataGridViewGenericRow.IsFavourite && dataGridView[columnIndex, rowIndex].ReadOnly)
                {
                    if (dataGridView[columnIndex, rowIndex].Style.BackColor != ColorReadOnly) newColor = ColorReadOnly;
                }
                else if (dataGridViewGenericRow.IsFavourite && dataGridView[columnIndex, rowIndex].ReadOnly) newColor = ColorReadOnlyFavourite;
                else newColor = ColorCellEditable;
            }

            if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion == FileEntryVersion.Error) newColor = ColorError;

            //if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.Metadata != null 
            //    && (dataGridViewGenericColumn.Metadata.Broker & MetadataBrokerType.ExifToolWriteError) == MetadataBrokerType.ExifToolWriteError) newColor = ColorError;

            if (newColor != Color.Empty && newColor != dataGridView[columnIndex, rowIndex].Style.BackColor) dataGridView[columnIndex, rowIndex].Style.BackColor = newColor;
        }
        #endregion

        #region Cell Handling - GetCellReadOnly - int columnIndex, int rowIndex
        public static bool GetCellReadOnly(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView[columnIndex, rowIndex].ReadOnly;
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
            return dataGridViewGenericCellStatus == null ? MetadataBrokerType.Empty : dataGridViewGenericCellStatus.MetadataBrokerTypes;
        }
        #endregion

        #endregion

        #region Copy Text within Grid
        #region Copy Text within Grid - CopyCellFromBrokerToMedia
        //Copy select Media Broker as Windows Life Photo Gallery, Microsoft Photots, Google Location History, etc... to correct Media File Tag
        private static Dictionary<CellLocation, DataGridViewGenericCell> CopyCellFromBrokerToMedia(DataGridView dataGridView, string targetHeader, int columnIndex, int rowIndex, bool doOwerwriteData)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            DataGridViewGenericRow dataGridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            int targetRowIndex = DataGridViewHandler.GetRowIndex(dataGridView, targetHeader, dataGridViewGenericDataRow.RowName);
            if (targetRowIndex != -1)
            {
                if (doOwerwriteData ||
                (!doOwerwriteData && (GetCellValue(dataGridView, columnIndex, targetRowIndex) == null || string.IsNullOrWhiteSpace(GetCellValue(dataGridView, columnIndex, targetRowIndex).ToString()))
                ))
                {
                    CellLocation cellLocation = new CellLocation(columnIndex, targetRowIndex);
                    if (!updatedCells.ContainsKey(cellLocation)) updatedCells.Add(cellLocation, DataGridViewHandler.CopyCellDataGridViewGenericCell(dataGridView, columnIndex, targetRowIndex));

                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, targetRowIndex,
                        DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex) );
                }
            }            
            return updatedCells;
        }
        #endregion

        #region Copy Text within Grid - CopySelectedCellFromBrokerToMedia
        public static void CopySelectedCellFromBrokerToMedia(DataGridView dataGridView, string targetHeader, bool overwrite)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = new Dictionary<CellLocation, DataGridViewGenericCell>();

            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, dataGridViewCell.ColumnIndex);
                if (dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                {
                    
                    Dictionary<CellLocation, DataGridViewGenericCell> updatedCellsDelta =
                        CopyCellFromBrokerToMedia(dataGridView, targetHeader, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex, overwrite);
                    

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
                SetDataGridViewDirty(dataGridView, columnIndex);
                //All click 
                if (gridViewGenericDataRow.IsHeader && columnIndex == -1)
                {
                    updatedCells = SetNextTriState(dataGridView, header, newState,
                        keywordHeaderStart, keywordsStarts, keywordsEnds,
                        0, keywordsStarts, 1, 1);
                }
                //Row click - click on a tristate buttons 
                else if (!gridViewGenericDataRow.IsHeader && columnIndex == -1) // 
                {
                    updatedCells = SetNextTriState(dataGridView, header, newState, keywordHeaderStart, keywordsStarts, keywordsEnds, 0, rowIndex, 1, 0);
                }
                //Column click - click on a tristate buttons 
                else if (gridViewGenericDataRow.IsHeader && columnIndex > -1) //Tag heading, on given column
                {
                    DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                    if (dataGridViewGenericDataColumn.Metadata != null)
                    {
                        updatedCells = SetNextTriState(dataGridView, header, newState, keywordHeaderStart, keywordsStarts, keywordsEnds, columnIndex, keywordHeaderStart, 0, 1);
                    }
                }
                //Cell - click on a tristate buttons 
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
            //Refresh(dataGridView);

            if (updatedCells != null && updatedCells.Count > 0)
            {                
                ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
            }

        }
        #endregion

        #endregion

        #region ToolStripMenuItem Handling 

        #region ToolStripMenuItem Handling - cut
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionCut(dataGridView);
        }
        #endregion

        #region ToolStripMenuItem Handling - copy
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionCopy(dataGridView);
        }
        #endregion

        #region ToolStripMenuItem Handling - paste
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionPaste(dataGridView);
        }
        #endregion

        #region ToolStripMenuItem Handling - delete
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionDelete(dataGridView);
        }
        #endregion

        #region ToolStripMenuItem Handling - undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionUndo(dataGridView);
        }
        #endregion

        #region ToolStripMenuItem Handling - redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionRedo(dataGridView);
        }
        #endregion

        #region ToolStripMenuItem Handling - find
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionFindAndReplace(dataGridView, false);
        }
        #endregion

        #region ToolStripMenuItem Handling - replace
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionFindAndReplace(dataGridView, true);
        }
        #endregion

        #region ToolStripMenuItem Handling - save
        private void toolStripMenuItemMapSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented");
        }
        #endregion

        #region ToolStripMenuItem Handling - toogle Favorite
        private void toggleRowsAsFavouriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ToolStripMenuItem Handling - mark Favorite
        private void markAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ToolStripMenuItem Handling - remove Favorite
        private void removeAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ToolStripMenuItem Handling - toogle hide/show equals
        private void toggleHideEqualRowsValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionToggleStripMenuItem(dataGridView, toggleHideEqualRowsValuesToolStripMenuItem);
            SetRowsVisbleStatus(dataGridView, toggleHideEqualRowsValuesToolStripMenuItem.Checked, toggleShowFavouriteRowsToolStripMenuItem.Checked);
        }
        #endregion

        #region ToolStripMenuItem Handling - toogle show Favorite
        private void toggleShowFavouriteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionToggleStripMenuItem(dataGridView, toggleShowFavouriteRowsToolStripMenuItem);
            SetRowsVisbleStatus(dataGridView, toggleHideEqualRowsValuesToolStripMenuItem.Checked, toggleShowFavouriteRowsToolStripMenuItem.Checked);
        }
        #endregion

        #region ToolStripMenuItem Handling - Toogle handler - For hide/show Favorite and Hide/Show Equals
        public static void ActionToggleStripMenuItem(DataGridView dataGridView, ToolStripMenuItem toolStripMenuItem)
        {
            if (!dataGridView.Enabled) return;
            if (toolStripMenuItem.Checked)
            {
                toolStripMenuItem.Checked = false;
                toolStripMenuItem.CheckState = CheckState.Unchecked;
            }
            else
            {
                toolStripMenuItem.Checked = true;
                toolStripMenuItem.CheckState = CheckState.Checked;
            }
        }
        #endregion

        #endregion

        #region KeyDownEventHandler

        #region KeyDownEventHandler - call none static KeyDownEventHandler
        public static void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            KeyDownEventHandler(dataGridView, e);
        }
        #endregion

        #region KeyDownEventHandler - KeyDownEventHandler
        public static void KeyDownEventHandler(DataGridView dataGridView, KeyEventArgs e)
        {

            if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.X) //Cut Ctrl-C
            {
                ActionCut(dataGridView);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.C) //Copy Ctrl-C
            {
                ActionCopy(dataGridView);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.V) //Paste  Ctrl-V
            {
                ActionPaste(dataGridView);
                e.SuppressKeyPress = true;
            }
            else if (!e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.Delete) //Delete
            {
                ActionDelete(dataGridView);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.Z) //Undo Ctrl-Z 
            {
                ActionUndo(dataGridView);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.Y) //Redo Ctrl-Y 
            {
                ActionRedo(dataGridView);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.F) //Find Ctrl-F 
            {
                ActionFindAndReplace(dataGridView, false);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.H) //Find and Replace Ctrl-H 
            {
                ActionFindAndReplace(dataGridView, true);
                e.SuppressKeyPress = true;
            }

            else if (!e.Control && !e.Shift && !e.Alt && e.KeyCode == Keys.Apps) //Context menu 
            {

            }
        }
        #endregion

        #endregion

        #region DataGridView - Refresh - InvalidateCell for MediaFullFilename
        public static void RefreshImageForMediaFullFilename(DataGridView dataGridView, string fullFilePath)
        {
            if (dataGridView == null) return;

            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn column && column.FileEntryAttribute.FileFullPath == fullFilePath)
                {
                    dataGridView.InvalidateCell(columnIndex, -1);
                }
            }
            //DataGridViewHandler.Refresh(dataGridView);
        }

        #endregion

        #region DataGridView - Update Image - for FileEntryImage
        public static void SetDataGridImageOnFileEntryAttribute(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, Image image)
        {
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingImage(dataGridView))
            {
                //return;  //In progress doing so, I think we can remove
            }

            DataGridViewHandler.SetIsPopulatingImage(dataGridView, true);
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn column && column.FileEntryAttribute == fileEntryAttribute)
                {
                    column.Thumbnail = image;
                    dataGridView.InvalidateCell(columnIndex, -1);
                }
            }            
            DataGridViewHandler.SetIsPopulatingImage(dataGridView, false);
        }
        #endregion 

        #region Cell Paint handling
        private const int roundedRadius = 8;

        #region Cell Paint handling - DrawImageAndSubText
        public static void DrawImageAndSubText(object sender, DataGridViewCellPaintingEventArgs e, Image image, string text, Color backgroundColor)
        {
            Rectangle rectangleRoundedCellBounds = CalulateCellRoundedRectangleCellBounds(e.CellBounds);
            if (image != null)
            {
                try
                {
                    Size thumbnailSize = CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);

                    if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                        e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.SelectionBackColor),
                        new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height));
                    else
                        e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor),
                         new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height));

                    Manina.Windows.Forms.Utility.FillRoundedRectangle(e.Graphics, new SolidBrush(backgroundColor),
                        new Rectangle(rectangleRoundedCellBounds.X, rectangleRoundedCellBounds.Y, rectangleRoundedCellBounds.Width, rectangleRoundedCellBounds.Height), roundedRadius);
                    Manina.Windows.Forms.Utility.DrawRoundedRectangle(e.Graphics, new Pen(Color.Black, 3),
                        new Rectangle(rectangleRoundedCellBounds.X, rectangleRoundedCellBounds.Y, rectangleRoundedCellBounds.Width, rectangleRoundedCellBounds.Height), roundedRadius);

                    e.Graphics.DrawImage(image, CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize));

                    e.Graphics.DrawLine(new Pen(Color.Silver),
                        e.CellBounds.Left,
                        e.CellBounds.Top + e.CellBounds.Height - 1,
                        e.CellBounds.Left + e.CellBounds.Width - 1,
                        e.CellBounds.Top + e.CellBounds.Height - 1);
                }
                catch (Exception ex)
                {
                    //Thumbnail was occupied in other thread
                    Logger.Warn("DrawImageAndSubText: " + ex.Message);
                }
            }

            if (text != null)
            {
                SizeF sizeF = e.Graphics.MeasureString(text, ((DataGridView)sender).Font, rectangleRoundedCellBounds.Size);

                var rectF = new RectangleF(
                    rectangleRoundedCellBounds.X + 2,
                    rectangleRoundedCellBounds.Y + rectangleRoundedCellBounds.Height - sizeF.Height - 2,
                    sizeF.Width, sizeF.Height);
                //Filling a 50% transparent rectangle before drawing the string.
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), rectF);
                e.Graphics.DrawString(text, ((DataGridView)sender).Font, new SolidBrush(Color.Black), rectF);
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
            return new Rectangle(rectangle.Left + 4, rectangle.Top + 4, rectangle.Width - 5, rectangle.Height - 5);
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

        public static bool UpdateSelectedCellsWithNewRegion(DataGridView dataGridView, int columnIndex, RectangleF region)
        {
            bool updated = false;

            ClipboardUtility.PushToUndoStack(dataGridView);

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
                                    new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.On, false)));

                                regionStructure = GetCellRegionStructure(dataGridView, cells.ColumnIndex, cells.RowIndex);
                                regionStructure.Name = dataGridViewGenericRow.RowName;
                            }
                        }
                    }

                    SetCellStatusSwichStatus(dataGridView, cells.ColumnIndex, cells.RowIndex, SwitchStates.On);
                    if (regionStructure != null)
                    {
                        regionStructure.AreaX = region.X;
                        regionStructure.AreaY = region.Y;
                        regionStructure.AreaWidth = region.Width;
                        regionStructure.AreaHeight = region.Height;
                        regionStructure.RegionStructureType = RegionStructureTypes.WindowsLivePhotoGallery;
                        updated = true;
                        regionStructure.Thumbnail = null;
                    }
                }
            }
            return updated;
        }

        #region Cell Paint handling - UpdateSelectedCellsWithNewMouseRegion 
        public static bool UpdateSelectedCellsWithNewMouseRegion(DataGridView dataGridView, int columnIndex, int x1, int y1, int x2, int y2)
        {
            bool updated = false;
            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null) return updated;
            if (dataGridViewGenericColumn.ReadWriteAccess != ReadWriteAccess.AllowCellReadAndWrite) return updated;

            Image image = dataGridViewGenericColumn.Thumbnail;

            Rectangle rectangleRoundedCellBounds = CalulateCellRoundedRectangleCellBounds(
                new Rectangle (0, 0, dataGridView.Columns[columnIndex].Width, dataGridView.ColumnHeadersHeight));
            Size thumbnailSize = CalulateCellImageSizeInRectagleWithUpScale(rectangleRoundedCellBounds, image.Size);
            Rectangle rectangleCenterThumbnail = CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);

            Rectangle rectangleMouse = GetRectangleFromMouseCoorinate(x1, y1, x2, y2);
            Rectangle rectangleMouseThumb = new Rectangle(rectangleMouse.X - rectangleCenterThumbnail.X, rectangleMouse.Y - rectangleCenterThumbnail.Y, rectangleMouse.Width, rectangleMouse.Height);
            RectangleF region = RegionStructure.CalculateImageRegionAbstarctRectangle(thumbnailSize, rectangleMouseThumb, RegionStructureTypes.WindowsLivePhotoGallery);

            updated = UpdateSelectedCellsWithNewRegion(dataGridView, columnIndex, region);

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
                Image image = dataGridViewGenericColumn.Thumbnail;
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
        #endregion

        #region Cell Paint handling - CellPaintingHandleDefault
        public static void CellPaintingHandleDefault(object sender, DataGridViewCellPaintingEventArgs e, bool paintHeaderRow)
        {
            try
            {                
                if (paintHeaderRow || e.ColumnIndex == -1 || e.RowIndex > -1) e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
            } catch (Exception ex)
            {
                Logger.Error(ex.Message);    
            }
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
                    DrawIcon16x16OnLeftSide(sender, e, global::DataGridViewGeneric.Properties.Resources.ToolTipsText);
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

                bool hasFileKnownErrors = (errorFileEntries.ContainsKey(fileEntryAttributeColumn.FileEntry.FileFullPath));

                string cellText = "";
                if (dataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning) cellText += "File updated!!\r\n";

                if (dataGridViewGenericColumn.Metadata != null)
                {
                    switch (GetDataGridSizeLargeMediumSmall(dataGridView))
                    {
                        case DataGridViewSize.Small:
                            cellText += fileEntryAttributeColumn.FileName;
                            break;
                        case DataGridViewSize.Medium:
                            cellText += dataGridViewGenericColumn.Metadata.FileDateModified.ToString() + "\r\n" + fileEntryAttributeColumn.FileName;
                            break;
                        case DataGridViewSize.Large:
                            cellText += dataGridViewGenericColumn.Metadata.FileDateModified.ToString() + "\r\n" + fileEntryAttributeColumn.FileFullPath;
                            break;
                        default:
                            throw new Exception("Not implemented");
                    }
                }
                else
                {
                    switch (GetDataGridSizeLargeMediumSmall(dataGridView))
                    {
                        case DataGridViewSize.Small: //Small
                            cellText += fileEntryAttributeColumn.FileName;
                            break;
                        case DataGridViewSize.Medium: //Medium
                            cellText += fileEntryAttributeColumn.LastWriteDateTime.ToString() + "\r\n" + fileEntryAttributeColumn.FileName;
                            break;
                        case DataGridViewSize.Large: //Large
                            cellText += fileEntryAttributeColumn.LastWriteDateTime.ToString() + "\r\n" + fileEntryAttributeColumn.FileFullPath;
                            break;
                        default:
                            throw new Exception("Not implemented");
                    }
                }

                Image image = dataGridViewGenericColumn.Thumbnail;
                if (image == null) image = (Image)Properties.Resources.load_image;
                if (hasFileKnownErrors) DrawImageAndSubText(sender, e, image, cellText, ColorHeaderError);
                else if (dataGridViewGenericColumn.HasFileBeenUpdatedGiveUserAwarning) DrawImageAndSubText(sender, e, image, cellText, ColorHeaderWarning);
                else DrawImageAndSubText(sender, e, image, cellText, ColorHeaderImage);

            }
            
        }
        #endregion

        #region Cell Paint handling - CellPaintingTriState
        public static void CellPaintingTriState(object sender, DataGridViewCellPaintingEventArgs e, DataGridView dataGridView, string header)
        {
            DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
            if (gridViewGenericDataRow == null) return; //Don't paint anything TriState on "New Empty Row" for "new Keywords"

            DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
            if (e.ColumnIndex > -1)
            {
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
