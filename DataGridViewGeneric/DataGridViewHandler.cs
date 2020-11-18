using Manina.Windows.Forms;
using MetadataLibrary;
using MetadataPriorityLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;

namespace DataGridViewGeneric
{
    public enum DataGridViewSize
    {
        Large = 1,
        Medium = 2,
        Small = 3,
        RenameSize = 128,
        ConfigSize = 256
    }

    public partial class DataGridViewHandler
    {
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

        public static int GetSelectedCellCount(DataGridView dataGridView)
        {
            return dataGridView.SelectedCells.Count;
        }

        

        #region DataGridView events handling
        private void DataGridView_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void DataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DataGridView_CancelRowEdit(object sender, QuestionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DataGridView_RowDirtyStateNeeded(object sender, QuestionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DataGridView_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DataGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region InitializeComponent and DataGridView
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

        
        public static void Clear(DataGridView dataGridView, DataGridViewSize cellSize)
        {
            DataGridViewGenericData dataGridViewGenricData = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;

            ClipboardUtility.Clear(dataGridView);
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.TopLeftHeaderCell.Value = dataGridViewGenricData.TopCellName;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridViewGenricData.IsDirty = false;

            dataGridView.ColumnHeadersHeight = GetTopColumnHeaderHeigth(cellSize);
            //dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView.RowHeadersWidth = GetFirstRowHeaderWidth(cellSize);
            //dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            dataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
        }

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

        #region Event DataGridView_CurrentCellDirtyStateChanged
        private void DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //Commit changes ASAP, e.g. when SelectedIndexChanged will chnage the ValueChanges event be triggered
            dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);

            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return;
            dataGridViewGenericData.IsDirty = true;
        }

        public static bool IsDataGridViewDirty(DataGridView dataGridView)
        {
            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return false;
            return dataGridViewGenericData.IsDirty;
        }
        #endregion

        #region Select correct Cell when Right Click Mouse button
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
                else if (e.ColumnIndex == -1 && e.RowIndex == -1) //Column selected
                {
                    dataGridView.SelectAll();
                }
                else
                {
                    if (!dataGridView[e.ColumnIndex, e.RowIndex].Selected)
                    {
                        dataGridView.CurrentCell = dataGridView[e.ColumnIndex, e.RowIndex];
                    }
                }
            }
        }
        #endregion

        #region DataGridView Size for Column and Row Header, Row / Column size and resize 
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
                case DataGridViewSize.Small | DataGridViewSize.RenameSize:
                    return 24; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameSize:
                    return 24; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameSize:
                    return 24; //Rename Grid*/
                case DataGridViewSize.ConfigSize:
                    return 24;
                default:
                    throw new Exception("Not implemented");
            }
        }

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
                case DataGridViewSize.Small | DataGridViewSize.RenameSize:
                    return 24; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameSize:
                    return 24; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameSize:
                    return 24; //Rename Grid*/
                case DataGridViewSize.ConfigSize:
                    return 24;
                default:
                    throw new Exception("Not implemented");
            }
        }

        public static int GetFirstRowHeaderWidth(DataGridViewSize size)
        {
            switch (size)
            {
                case DataGridViewSize.Small:
                    return 200;
                case DataGridViewSize.Medium:
                    return 200;
                case DataGridViewSize.Large:
                    return 200;

                case DataGridViewSize.Small | DataGridViewSize.RenameSize:
                    return 200; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameSize:
                    return 400; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameSize:
                    return 600; //Rename Grid*/

                case DataGridViewSize.ConfigSize:
                    return 400;
                default:
                    throw new Exception("Not implemented");
            }
        }

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

                case DataGridViewSize.Small | DataGridViewSize.RenameSize:
                    return 200; //Rename Grid
                case DataGridViewSize.Medium | DataGridViewSize.RenameSize:
                    return 400; //Rename Grid
                case DataGridViewSize.Large | DataGridViewSize.RenameSize:
                    return 600; //Rename Grid

                case DataGridViewSize.ConfigSize:
                    return 200;
                default:
                    throw new Exception("Not implemented");
            }
        }


        public static int GetCellColumnsWidth(DataGridView dataGridView)
        {
            return GetCellWidth(GetDataGridSizeLargeMediumSmall(dataGridView));
        }

        public static int GetCellRowHeight(DataGridView dataGridView)
        {
            return GetCellHeigth(GetDataGridSizeLargeMediumSmall(dataGridView));
        }

        public static DataGridViewSize GetDataGridSizeLargeMediumSmall(DataGridView dataGridView)
        {
            return GetDataGridViewGenericData(dataGridView)?.CellSize == null ? DataGridViewSize.Medium : GetDataGridViewGenericData(dataGridView).CellSize;
        }

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

        private static DataGridViewAutoSizeRowsMode dataGridViewAutoSizeRowsMode;
        private static DataGridViewAutoSizeColumnsMode dataGridViewAutoSizeColumnMode;
        private static DataGridViewRowHeadersWidthSizeMode dataGridViewRowHeadersWidthSizeMode;
        private static DataGridViewColumnHeadersHeightSizeMode dataGridViewColumnHeadersHeightSizeMode;
        // *** API Declarations ***
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        public static void SuspendLayout(DataGridView dataGridView)
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
            
            // *** DataGridView population ***
            SendMessage(dataGridView.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeLayout(DataGridView dataGridView)
        {
            //dataGridView.EndEdit();
            dataGridView.AutoSizeRowsMode = dataGridViewAutoSizeRowsMode;
            dataGridView.AutoSizeColumnsMode = dataGridViewAutoSizeColumnMode;
            dataGridView.RowHeadersWidthSizeMode = dataGridViewRowHeadersWidthSizeMode;
            dataGridView.ColumnHeadersHeightSizeMode = dataGridViewColumnHeadersHeightSizeMode;

            dataGridView.RowHeadersVisible = true;
            dataGridView.ResumeLayout();

            SendMessage(dataGridView.Handle, WM_SETREDRAW, true, 0);
            dataGridView.Refresh();
        }

        public static bool IsCurrentFile(FileEntry fileEntry, DateTime lastWriteTime)
        {
            return (fileEntry.LastWriteDateTime == lastWriteTime);
        }


        #region IsPopulating IsAgregated handling

        public bool IsPopulating
        {
            get { return GetIsPopulating(dataGridView); }
            set { SetIsPopulating(dataGridView, value); }
        }

        public static bool GetIsPopulating(DataGridView dataGridView)
        {
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulating;
        }

        public static void SetIsPopulating(DataGridView dataGridView, bool isPopulating)
        {
            ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulating = isPopulating;
        }

        public bool IsPopulatingFile
        {
            get { return GetIsPopulatingFile(dataGridView); }
            set { SetIsPopulatingFile(dataGridView, value); }
        }

        public static bool GetIsPopulatingFile(DataGridView dataGridView)
        {
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulatingFile;
        }

        public static void SetIsPopulatingFile(DataGridView dataGridView, bool isPopulatingFile)
        {
            ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulatingFile = isPopulatingFile;
        }

        public bool IsPopulatingImage
        {
            get { return GetIsPopulatingImage(dataGridView); }
            set { SetIsPopulatingImage(dataGridView, value); }
        }

        public static bool GetIsPopulatingImage(DataGridView dataGridView)
        {
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulatingImage;
        }

        public static void SetIsPopulatingImage(DataGridView dataGridView, bool isPopulatingImage)
        {
            ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsPopulatingImage = isPopulatingImage;
        }

        public bool IsAgregated
        {
            get { return GetIsAgregated(dataGridView); }
            set { SetIsAgregated(dataGridView, value); }
        }

        public static bool GetIsAgregated(DataGridView dataGridView)
        {
            return ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsAgregated;
        }

        public static void SetIsAgregated(DataGridView dataGridView, bool isAgregated)
        {
            ((DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag).IsAgregated = isAgregated;
        }
        #endregion

        #region Actions
        public static void ActionCut(DataGridView dataGridView)
        {
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }

        public static void ActionCopy(DataGridView dataGridView)
        {
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
        }

        public static void ActionPaste(DataGridView dataGridView)
        {
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
        }

        public static void ActionDelete(DataGridView dataGridView)
        {
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }

        public static void ActionUndo(DataGridView dataGridView)
        {
            ClipboardUtility.UndoDataGridView(dataGridView);
        }

        public static void ActionRedo(DataGridView dataGridView)
        {
            ClipboardUtility.RedoDataGridView(dataGridView);
        }
        #endregion

        #region Action Find And Replace Form
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

        public static void BringToFrontFindAndReplace()
        {
            if (m_FindAndReplaceForm != null)
            {
                m_FindAndReplaceForm.BringToFront();
            }
        }

        private static void FindAndReplaceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_FindAndReplaceForm = null;
        }
        #endregion 

        #region DataGridViewGenericData handling
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

        public static string GetDataGridViewName(DataGridView dataGridView)
        {
            return GetDataGridViewGenericData(dataGridView)?.DataGridViewName;
        }


        #endregion

        #region Column handling
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

        public static int GetColumnCount(DataGridView dataGridView)
        {
            return dataGridView.ColumnCount;
        }

        public static bool DoesColumnFilenameExist(DataGridView dataGridView, string fullFilePath)
        {
            return GetColumnIndex(dataGridView, fullFilePath) != -1;
        }

        public static int GetColumnIndex(DataGridView dataGridView, string fullFilePath)
        {
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn &&
                    ((DataGridViewGenericColumn)dataGridView.Columns[columnIndex].Tag).FileEntryImage.FullFilePath == fullFilePath)
                {
                    return columnIndex;
                }
            }
            return -1; //Not found
        }

        public static int GetColumnIndex(DataGridView dataGridView, FileEntry fileEntry)
        {
            //TODO: Add cache dictonary
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn column &&
                    (FileEntry)column.FileEntryImage == (FileEntry)fileEntry)
                {
                    return columnIndex;
                }
            }
            return -1; //Not found
        }

        
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

        public static DateTime DateTimeForEditableMediaFile = DateTime.Now.AddYears(200);

        public static void AddColumnSelectedFiles(
            DataGridView dataGridView, ImageListViewSelectedItemCollection imageListViewItems, bool useCurrentFileLastWrittenDate, 
            ReadWriteAccess readWriteAccessForColumn, ShowWhatColumns showWhatColumns, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewItems)
            {
                AddColumnOrUpdate(dataGridView, 
                    new FileEntryImage(imageListViewItem.FullFileName, 
                    useCurrentFileLastWrittenDate ? imageListViewItem.DateModified : DateTimeForEditableMediaFile, //Use currentFile Last Written date, or future file for always keep same column for edit 
                    (Image)imageListViewItem.ThumbnailImage.Clone()),
                    null,                                                                   //No metadata yet 
                    imageListViewItem.DateModified,                                         //Last known file modified date
                    readWriteAccessForColumn,                                               //ForceReadOnly, AllowReadAndWrite
                    showWhatColumns,                                                        //Show Historical columns? Erros columns?
                    dataGridViewGenericCellStatusDefault);                                  //Default cell values
            }
        }

        /// <summary>
        /// Add a new column or find where column for FileEntry exists
        /// </summary>
        /// <param name="dataGridView">DataGridVIew to add / update column for</param>
        /// <param name="fileEntryImage"></param>
        /// <param name="metadata"></param>
        /// <param name="dateTimeForEditableMediaFile"></param>
        /// <param name="readWriteAccessForColumn"></param>
        /// <param name="showWhatColumns"></param>
        /// <param name="dataGridViewGenericCellStatusDefault"></param>
        /// <returns>== -1 - if column not added, or already aggregated, >= 0 for where column exists or was added</returns>
        public static int AddColumnOrUpdate(DataGridView dataGridView, 
            FileEntryImage fileEntryImage, Metadata metadata, DateTime dateTimeForEditableMediaFile,
            ReadWriteAccess readWriteAccessForColumn, ShowWhatColumns showWhatColumns, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault)
        {
            int columnIndex = GetColumnIndex(dataGridView, fileEntryImage); //Find column Index for Filename and date last written

            bool isErrorColumn = (metadata != null) && (metadata.Broker & MetadataBrokerTypes.ExifToolWriteError) > 0;
            bool showErrorColumns = (showWhatColumns & ShowWhatColumns.ErrorColumns) > 0;
            bool isHistoryColumn = (fileEntryImage.LastWriteDateTime < dateTimeForEditableMediaFile);
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
                
                dataGridViewColumn.ToolTipText = fileEntryImage.LastWriteDateTime.ToString() + "\r\n" + fileEntryImage.FullFilePath;                
                dataGridViewColumn.Tag = new DataGridViewGenericColumn(fileEntryImage, metadata, readWriteAccessForColumn);

                dataGridViewColumn.Name = fileEntryImage.FullFilePath;
                dataGridViewColumn.HeaderText = fileEntryImage.FullFilePath;
                
                int columnIndexFilename = GetColumnIndex(dataGridView, fileEntryImage.FullFilePath);
                if (columnIndexFilename == -1) //Not found
                {                
                    columnIndex = dataGridView.Columns.Add(dataGridViewColumn);
                }
                else
                {
                    //Short, newst always first
                    while (columnIndexFilename < dataGridView.Columns.Count &&
                        dataGridView.Columns[columnIndexFilename].Tag is DataGridViewGenericColumn column &&
                        column.FileEntryImage.FullFilePath == fileEntryImage.FullFilePath &&
                        column.FileEntryImage.LastWriteDateTime > fileEntryImage.LastWriteDateTime)
                    {
                        columnIndexFilename += 1;
                    }

                    if (columnIndexFilename < dataGridView.Columns.Count - 1 &&
                        dataGridView.Columns[columnIndexFilename].Tag is DataGridViewGenericColumn column2 &&
                        column2.FileEntryImage.FullFilePath == fileEntryImage.FullFilePath &&
                        column2.FileEntryImage.LastWriteDateTime > fileEntryImage.LastWriteDateTime)
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
                if (currentDataGridViewGenericColumn != null && currentDataGridViewGenericColumn.Metadata != null) 
                    isMetadataAlreadyAgregated = true;
                else
                    currentDataGridViewGenericColumn = new DataGridViewGenericColumn(fileEntryImage, metadata, readWriteAccessForColumn);

                if (metadata != null && !isHistoryColumn && !currentDataGridViewGenericColumn.HasFileBeenUpdated) 
                    currentDataGridViewGenericColumn.HasFileBeenUpdated = (metadata.FileDateModified > currentDataGridViewGenericColumn.Metadata.FileDateModified); //If edit Column
                
                if (metadata != null && metadata.FileDateModified > currentDataGridViewGenericColumn.Metadata.FileDateModified) 
                    currentDataGridViewGenericColumn.Metadata = metadata; //Keep newest version
                currentDataGridViewGenericColumn.ReadWriteAccess = readWriteAccessForColumn;
                dataGridView.Columns[columnIndex].Tag = currentDataGridViewGenericColumn;

                SetCellBackgroundColorForColumn(dataGridView, columnIndex);

                //Hide and show columns
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
            }

            if (isMetadataAlreadyAgregated) return -1;
            else return columnIndex;
        }

        public static bool IsColumnDataGridViewGenericColumn(DataGridView dataGridView, int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > dataGridView.ColumnCount) return false;
            return dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn;
        }

        public static DataGridViewGenericColumn GetColumnDataGridViewGenericColumn(DataGridView dataGridView, int columnIndex)
        {
            if (columnIndex < 0) return null;
            if (!IsColumnDataGridViewGenericColumn(dataGridView, columnIndex)) return null;
            return (DataGridViewGenericColumn)dataGridView.Columns[columnIndex].Tag;
        }
        #endregion 

        #region Rows handling
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

        public static bool IsRowDataGridViewGenericRow(DataGridView dataGridView, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > GetRowCount(dataGridView) - 1) return false;
            return dataGridView.Rows[rowIndex].HeaderCell.Tag is DataGridViewGenericRow;
        }

        public static DataGridViewGenericRow GetRowDataGridViewGenericRow(DataGridView dataGridView, int rowIndex)
        {
            if (rowIndex < 0) return null;
            if (!IsRowDataGridViewGenericRow(dataGridView, rowIndex)) return null;
            return (DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag;
        }

        public static int GetRowCountWithoutEditRow(DataGridView dataGridView)
        {
            if (dataGridView.AllowUserToAddRows)
                return dataGridView.RowCount - 1;
            return dataGridView.RowCount;
        }

        public static int GetRowCount(DataGridView dataGridView)
        {
            return dataGridView.RowCount;
        }

        public static int FindFileEntryRow(DataGridView dataGridView, DataGridViewGenericRow dataGridViewGenericRow, int startSearchRow)
        {
            int lastHeaderRowFound = -1;
            for (int rowIndex = startSearchRow; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                if (IsRowDataGridViewGenericRow(dataGridView, rowIndex))
                {
                    DataGridViewGenericRow dataGridViewGenericRowCheck = GetRowDataGridViewGenericRow(dataGridView, rowIndex);

                    if (dataGridViewGenericRow.IsHeader && dataGridViewGenericRowCheck.IsHeader && //It correct header
                       dataGridViewGenericRow.HeaderName == dataGridViewGenericRowCheck.HeaderName
                       )
                        return rowIndex;

                    if (!dataGridViewGenericRow.IsHeader && !dataGridViewGenericRowCheck.IsHeader && //It correct row
                        dataGridViewGenericRow.HeaderName == dataGridViewGenericRowCheck.HeaderName &&
                        dataGridViewGenericRow.RowName == dataGridViewGenericRowCheck.RowName
                        )
                        return rowIndex;

                    if (!dataGridViewGenericRow.IsHeader && //Remmeber last row found with same header name, regardless of hearer nor just value row
                        dataGridViewGenericRow.HeaderName == dataGridViewGenericRowCheck.HeaderName
                        )
                        lastHeaderRowFound = rowIndex;
                }

            }
            return lastHeaderRowFound;
        }

        public static int GetRowIndex(DataGridView dataGridView, DataGridViewGenericRow dataGridViewGenericRow)
        {
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                if (IsRowDataGridViewGenericRow(dataGridView, rowIndex) && GetRowDataGridViewGenericRow(dataGridView, rowIndex) == dataGridViewGenericRow)
                    return rowIndex;
            }
            return -1;
        }

        public static int GetRowIndex(DataGridView dataGridView, string headerName)
        {
            return GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName));
        }

        public static int GetRowIndex(DataGridView dataGridView, string headerName, string rowName)
        {
            return GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
        }

        public static void AddRowAndValueList(DataGridView dataGridView, FileEntryImage fileEntryColumn, List<DataGridViewGenericRowAndValue> dataGridViewGenericRowAndValueList)
        {
            int columnIndex = GetColumnIndex(dataGridView, fileEntryColumn);

            foreach (DataGridViewGenericRowAndValue dataGridViewGenericRowAndValue in dataGridViewGenericRowAndValueList)
            {
                int rowIndex = AddRow(dataGridView, columnIndex, dataGridViewGenericRowAndValue.DataGridViewGenericRow, 
                    GetFavoriteList(dataGridView),
                    dataGridViewGenericRowAndValue.DataGridViewGenericCell.Value,
                    dataGridViewGenericRowAndValue.DataGridViewGenericCell.CellStatus, 0, true);

                //if (rowIndex > -1) SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericRowAndValue.DataGridViewGenericCell.CellStatus);
            }
        }

        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericRow, GetFavoriteList(dataGridView), null,
                new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true), 0, false);
        }
        
        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow, object value, bool cellReadOnly)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericRow, GetFavoriteList(dataGridView), value,
                new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, cellReadOnly), 0, true);
        }

        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericRow, GetFavoriteList(dataGridView), value,
                dataGridViewGenericCellStatusDefaults, 0, true);
        }

        public static void FastAutoSizeRowsHeight(DataGridView dataGridView)
        {
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
            List<FavoriteRow> dataGridFavorites, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault, int startSearchRow, bool writeValue)
        {
            int rowIndex = FindFileEntryRow(dataGridView, dataGridViewGenericRow, startSearchRow);

            DataGridViewGenericRow dataGridViewGenericRowCheck = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (rowIndex == -1 ||
                dataGridViewGenericRowCheck.IsHeader != dataGridViewGenericRow.IsHeader ||
                dataGridViewGenericRowCheck.HeaderName != dataGridViewGenericRow.HeaderName ||
                dataGridViewGenericRowCheck.RowName != dataGridViewGenericRow.RowName)
            {
                //New row added
                if (rowIndex == -1) rowIndex = GetRowCountWithoutEditRow(dataGridView);
                else rowIndex++;

                dataGridView.Rows.Insert(rowIndex, 1);
                SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, dataGridViewGenericRow);
                SetCellStatusDefaultWhenRowAdded(dataGridView, rowIndex, dataGridViewGenericCellStatusDefault);
            }
            
            //If a value row, set the value
            if (!dataGridViewGenericRow.IsHeader)
            {
                if (writeValue) dataGridView[columnIndex, rowIndex].Value = value;                
                if (dataGridViewGenericRow.IsMultiLine)
                {
                    dataGridView.Columns[columnIndex].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
            } 
            else dataGridViewGenericCellStatusDefault.CellReadOnly = true;

            SetRowFavoriteFlag(dataGridView, rowIndex, dataGridFavorites);

            //It's only possible to update ReadOnly field 
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
            if (dataGridViewGenericCellStatus != null) dataGridViewGenericCellStatus.CellReadOnly = dataGridViewGenericCellStatusDefault.CellReadOnly;

            SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
            //SetCellStatusDefaults(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatusDefaults);

            SetCellBackGroundColorForRow(dataGridView, rowIndex);

            return rowIndex;
        }

        public static void SetRowHeaderNameAndFontStyle(DataGridView dataGridView, int rowIndex, string headerName, string rowName, ReadWriteAccess readWriteAccess)
        {
            SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, new DataGridViewGenericRow(headerName, rowName, readWriteAccess));
        }

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

        public static string GetRowName(DataGridView dataGridView, int rowIndex)
        {
            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            return dataGridViewGenericRow == null ? "" : dataGridViewGenericRow.RowName;
        }

        public static string GetRowHeader(DataGridView dataGridView, int rowIndex)
        {
            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            return dataGridViewGenericRow == null ? "" : dataGridViewGenericRow.HeaderName;
        }

        
        public static void SetRowToolTipText(DataGridView dataGridView, int rowIndex, string toolTipText)
        {
            dataGridView.Rows[rowIndex].HeaderCell.ToolTipText = toolTipText;
        }

        public static bool IsRowHeaderNameNullOrWhiteSpace(DataGridView dataGridView, int rowIndex)
        {
            return dataGridView.Rows[rowIndex].HeaderCell.Value == null || string.IsNullOrWhiteSpace(dataGridView.Rows[rowIndex].HeaderCell.Value.ToString());
        }

        

        #endregion      

        #region Row handling - Favorite handling

        public static List<FavoriteRow> GetFavoriteList(DataGridView dataGridView)
        {
            return GetDataGridViewGenericData(dataGridView)?.FavoriteList;
        }

        public static void SetFavoriteList(DataGridView dataGridView, List<FavoriteRow> dataGridFavoriteList)
        {
            DataGridViewGenericData dataGridViewGenericData = GetDataGridViewGenericData(dataGridView);
            if (dataGridViewGenericData == null) return;
            dataGridViewGenericData.FavoriteList = dataGridFavoriteList;
        }

        private static string CreateFavoriteFilename(string dataGridViewName)
        {
            string jsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
            if (!Directory.Exists(jsonPath)) Directory.CreateDirectory(jsonPath);
            return Path.Combine(jsonPath, "Favourite." + dataGridViewName + ".json");
        }

        public static List<FavoriteRow> FavouriteRead(string filename)
        {
            List<FavoriteRow> favouriteRows = new List<FavoriteRow>();
            if (File.Exists(filename))
            {
                favouriteRows = JsonConvert.DeserializeObject<List<FavoriteRow>>(File.ReadAllText(filename));
            }
            return favouriteRows;
        }

        public static void FavouriteWrite(string filename, List<FavoriteRow> favouriteRows)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(favouriteRows, Newtonsoft.Json.Formatting.Indented));
        }

        public static void FavouriteWrite(DataGridView dataGridView, List<FavoriteRow> favouriteRows)
        {
            FavouriteWrite(CreateFavoriteFilename(GetDataGridViewName(dataGridView)), favouriteRows);
        }


        public static void ActionSetRowsFavouriteState(DataGridView dataGridView, NewState newState)
        {
            List<FavoriteRow> favouriteRows = GetFavoriteList(dataGridView);
            List<int> selectedRows = GetRowSelected(dataGridView);

            foreach (int rowIndex in selectedRows)
            {
                if (IsRowDataGridViewGenericRow(dataGridView, rowIndex))
                {
                    dataGridView.InvalidateCell(dataGridView.Rows[rowIndex].HeaderCell);

                    DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);

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

        public static void SetRowFavoriteFlag(DataGridView dataGridView, int rowIndex)
        {
            SetRowFavoriteFlag(dataGridView, rowIndex, DataGridViewHandler.GetFavoriteList(dataGridView));
        }

        private static void SetRowFavoriteFlag(DataGridView dataGridView, int rowIndex, List<FavoriteRow> dataGridFavorites)
        {
            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);

            dataGridViewGenericRow.IsFavourite = dataGridFavorites.Contains(new FavoriteRow(dataGridViewGenericRow.HeaderName, dataGridViewGenericRow.RowName, dataGridViewGenericRow.IsHeader));
        }

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

            if (IsRowDataGridViewGenericRow(dataGridView, rowIndex))
            {
                DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
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

        #region Cell Handling

        #region Refreash
        public static CellLocation GetCellLocation(DataGridViewCell cell)
        {
            return new CellLocation(cell.ColumnIndex, cell.RowIndex);
        }

        public static void SetCurrentCellLocation(DataGridView dataGridView, CellLocation cell)
        {
            dataGridView.CurrentCell = dataGridView[cell.ColumnIndex, cell.RowIndex];
            if (dataGridView.CurrentCell is DataGridViewComboBoxCell) Refresh(dataGridView);
        }
        public static void Refresh(DataGridView dataGridView)
        {            
            dataGridView.Parent.Focus(); //Hack to refresh DataGridViewComboBoxCell, do to it will not refresh before changed cell / cell lost focus
            dataGridView.Focus();
            dataGridView.Refresh();
        }
        #endregion

        #region Deep Copy
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

        #region Cell Tag and Value

        

        public static DataGridViewCell GetCellDataGridViewCell(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView[columnIndex, rowIndex];
        }

        public static object GetCellValue(DataGridView dataGridView, int columnIndex, string headerName, string rowName)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
            if (columnIndex > -1 && rowIndex > -1) return GetCellValue(dataGridView,columnIndex, rowIndex);
            else return null;
        }

        public static object GetCellValue(DataGridViewCell dataGridViewCell)
        {
            return dataGridViewCell.Value;
        }

        public static object GetCellValue(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return GetCellValue(dataGridView[columnIndex, rowIndex]);
        }

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

        public static string GetCellValueNullOrStringTrim(DataGridView dataGridView, int columnIndex, string headerName, string rowName)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
            return GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex);
        }

        public static bool IsCellNullOrWhiteSpace(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView[columnIndex, rowIndex].Value == null || string.IsNullOrWhiteSpace(dataGridView[columnIndex, rowIndex].Value.ToString().Trim());
        }

        public static void SetCellValue(DataGridView dataGridView, int columnIndex, string headerName, string rowName, object value)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
            SetCellValue(dataGridView, columnIndex, rowIndex, value);
        }

        public static void SetCellValue(DataGridView dataGridView, int columnIndex, int rowIndex, object value)
        {
            if (rowIndex > -1 && columnIndex > -1) dataGridView[columnIndex, rowIndex].Value = value;
        }
        
        #endregion

        #region Cell ToolTipText

        public static void SetCellToolTipText(DataGridView dataGridView, int columnIndex, int rowIndex, string toolTipText)
        {
            dataGridView[columnIndex, rowIndex].ToolTipText = toolTipText;
        }

        public static string GetCellToolTipText(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView[columnIndex, rowIndex].ToolTipText;
        }
        #endregion

        #region Cell Contol Type
        public static void SetCellControlType(DataGridView dataGridView, int columnIndex, string headerName, string rowName, DataGridViewCell dataGridViewCell)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
            SetCellControlType(dataGridView, columnIndex, rowIndex, dataGridViewCell);
        }

        public static void SetCellControlType(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewCell dataGridViewCell)
        {
            if (rowIndex > -1 && columnIndex > -1) dataGridView[columnIndex, rowIndex] = dataGridViewCell;
        }
        #endregion

        #region Cell DataGridViewGenericCell
        public static DataGridViewGenericCell CopyCellDataGridViewGenericCell(DataGridViewCell dataGridViewCell)
        {
            return new DataGridViewGenericCell(DeepCopy(GetCellValue(dataGridViewCell)), DeepCopy(GetCellStatus(dataGridViewCell)));
        }

        public static DataGridViewGenericCell CopyCellDataGridViewGenericCell(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return new DataGridViewGenericCell(DeepCopy(GetCellValue(dataGridView, columnIndex, rowIndex)), DeepCopy(GetCellStatus(dataGridView, columnIndex, rowIndex)));
        }

        public static DataGridViewGenericCell GetCellDataGridViewGenericCell(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return new DataGridViewGenericCell(GetCellValue(dataGridView, columnIndex, rowIndex), GetCellStatus(dataGridView, columnIndex, rowIndex));
        }

        public static void SetCellDataGridViewGenericCell(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewGenericCell dataGridViewGenericCell)
        {
            if (rowIndex > -1 && columnIndex > -1)
            {
                SetCellValue(dataGridView, columnIndex, rowIndex, dataGridViewGenericCell.Value);
                SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCell.CellStatus);
            }
        }
        #endregion

        #region Cell Region Structure
        public static RegionStructure GetCellRegionStructure(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            var regionObject = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex);
            MetadataLibrary.RegionStructure region = null;
            if (regionObject is RegionStructure)
            {
                region = (MetadataLibrary.RegionStructure)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex);
            }
            return region;
        }
        #endregion 

        #region Cell Status
        public static bool IsCellDataGridViewGenericCellStatus(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return IsCellDataGridViewGenericCellStatus(dataGridView[columnIndex, rowIndex]);
        }

        public static DataGridViewGenericCellStatus GetCellStatus(DataGridViewCell dataGridViewCell)
        {
            if (!IsCellDataGridViewGenericCellStatus(dataGridViewCell)) return null;
            return (DataGridViewGenericCellStatus)dataGridViewCell.Tag;
        }

        public static bool IsCellDataGridViewGenericCellStatus(DataGridViewCell dataGridViewCell)
        {
            return (dataGridViewCell.Tag != null && dataGridViewCell.Tag.GetType() == typeof(DataGridViewGenericCellStatus));
        }

        public static DataGridViewGenericCellStatus GetCellStatus(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return GetCellStatus(dataGridView[columnIndex, rowIndex]);
        }

        public static void SetCellStatus(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatus)
        {
            if (rowIndex > -1 && columnIndex > -1)
            {
                dataGridView[columnIndex, rowIndex].Tag = dataGridViewGenericCellStatus;
            }
        }

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

        #region Cell ReadOnly and Color 

        public static void SetCellBackGroundColorForRow(DataGridView dataGridView, int rowIndex)
        {
            foreach (DataGridViewCell dataGridCell in dataGridView.Rows[rowIndex].Cells)
            {
                SetCellBackGroundColor(dataGridView, dataGridCell.ColumnIndex, rowIndex);
            }
        }

        public static void SetCellBackgroundColorForColumn(DataGridView dataGridView, int columnIndex)
        {
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                SetCellBackGroundColor(dataGridView, columnIndex, rowIndex);
            }
        }

        private static void SetCellReadOnlyDependingOfStatus(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewGenericCellStatus dataGridViewGenericCellStatus)
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

        private static void SetCellBackGroundColor(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            DataGridViewGenericRow dataGridViewGenericRow = GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);


            if (dataGridViewGenericRow == null)
            {
                if (dataGridView[columnIndex, rowIndex].ReadOnly)
                    dataGridView[columnIndex, rowIndex].Style.BackColor = ColorReadOnly;
                else
                    dataGridView[columnIndex, rowIndex].Style.BackColor = ColorCellEditable;
            }
            else
            {
                if (dataGridViewGenericRow.IsFavourite && !dataGridView[columnIndex, rowIndex].ReadOnly)
                    dataGridView[columnIndex, rowIndex].Style.BackColor = ColorFavourite;
                else if (!dataGridViewGenericRow.IsFavourite && dataGridView[columnIndex, rowIndex].ReadOnly)
                    dataGridView[columnIndex, rowIndex].Style.BackColor = ColorReadOnly;
                else if (dataGridViewGenericRow.IsFavourite && dataGridView[columnIndex, rowIndex].ReadOnly)
                    dataGridView[columnIndex, rowIndex].Style.BackColor = ColorReadOnlyFavourite;
                else
                    dataGridView[columnIndex, rowIndex].Style.BackColor = ColorCellEditable;
            }
            if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.Metadata != null && (dataGridViewGenericColumn.Metadata.Broker & MetadataBrokerTypes.ExifToolWriteError) == MetadataBrokerTypes.ExifToolWriteError)
                dataGridView[columnIndex, rowIndex].Style.BackColor = ColorError;
        }

        public static bool GetCellReadOnly(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView[columnIndex, rowIndex].ReadOnly;
        }
        #endregion

        #region Cell Switch Status
        public static SwitchStates GetCellStatusSwichStatus(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
            return dataGridViewGenericCellStatus == null ? SwitchStates.Undefine : dataGridViewGenericCellStatus.SwitchState;
        }

        public static void SetCellStatusSwichStatus(DataGridView dataGridView, int columnIndex, int rowIndex, SwitchStates switchState)
        {
            DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null || dataGridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.ForceCellToReadOnly) //History or Error column
                switchState = SwitchStates.Disabled;

            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
            if (dataGridViewGenericCellStatus == null) dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, switchState, true);
            dataGridViewGenericCellStatus.SwitchState = switchState;
        }
        #endregion

        #region Cell Metadata Broker Type
        public static MetadataBrokerTypes GetCellStatusMetadataBrokerType(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
            return dataGridViewGenericCellStatus == null ? MetadataBrokerTypes.Empty : dataGridViewGenericCellStatus.MetadataBrokerTypes;
        }

        public static void SetCellStatusMetadataBrokerType(DataGridView dataGridView, int columnIndex, int rowIndex, MetadataBrokerTypes metadataBrokerTypes)
        {
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = GetCellStatus(dataGridView, columnIndex, rowIndex);
            if (dataGridViewGenericCellStatus == null) dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(metadataBrokerTypes, SwitchStates.Off, true);
            dataGridViewGenericCellStatus.MetadataBrokerTypes = metadataBrokerTypes;
        }
        #endregion 

        #endregion

        #region Copy select Media Broker as Windows Life Photo Gallery, Microsoft Photots, Google Location History, etc... to correct Media File Tag
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
            
            
            if (updatedCells != null && updatedCells.Count > 0)
            {
                ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
            }
        }

        #endregion

        #region TriState handeling
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

        public static int GetRowHeadingItemStarts(DataGridView dataGridView, string header)
        {
            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, header);
            if (rowIndex > -1) return rowIndex + 1;
            return DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); ;
        }

        public static int GetRowHeadingItemsEnds(DataGridView dataGridView, string header)
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

                            if ((GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) & MetadataBrokerTypes.ExifTool) == 0)
                            {
                                isSomeAdded = true;
                            }
                        }
                        else
                        {
                            isAllAdded = false;
                            if ((GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) & MetadataBrokerTypes.ExifTool) != 0)
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

        public static TriState GetColumnTriState(DataGridView dataGridView, string header, int keywordsStarts, int columnIndex)
        {
            return GetTriState(dataGridView, header, columnIndex, keywordsStarts, 0, 1);
        }

        public static TriState GetRowTriState(DataGridView dataGridView, string header, int rowIndex)
        {
            return GetTriState(dataGridView, header, 0, rowIndex, 1, 0);
        }

        public static TriState GetAllTriState(DataGridView dataGridView, string header, int keywordsStarts)
        {
            return GetTriState(dataGridView, header, 0, keywordsStarts, 1, 1);
        }

        

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
                                if (GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) != MetadataBrokerTypes.Empty)
                                {
                                    SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.On);
                                }
                            }
                            break;
                        case TriState.AllDeleted:
                            SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, SwitchStates.Off);
                            break;
                        case TriState.Unchange:
                            if ((GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) & MetadataBrokerTypes.ExifTool) != 0)
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

        public static Dictionary<CellLocation, DataGridViewGenericCell> ToggleCells(DataGridView dataGridView, string header, NewState newState, int columnIndex, int rowIndex)
        {
            Dictionary<CellLocation, DataGridViewGenericCell> updatedCells = null;

            DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (gridViewGenericDataRow == null) return updatedCells; //Don't paint anything TriState on "New Empty Row" for "new Keywords"

            int keywordHeaderStart = DataGridViewHandler.GetRowHeadingIndex(dataGridView, header);
            int keywordsStarts = DataGridViewHandler.GetRowHeadingItemStarts(dataGridView, header);
            int keywordsEnds = DataGridViewHandler.GetRowHeadingItemsEnds(dataGridView, header);

            if (gridViewGenericDataRow.HeaderName.Equals(header))
            {
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
            Refresh(dataGridView);

            if (updatedCells != null && updatedCells.Count > 0)
            {
                ClipboardUtility.PushToUndoStack(dataGridView, updatedCells);
            }

        }
        #endregion 

        #region ToolStripMenuItem_Click
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionCut(dataGridView);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionCopy(dataGridView);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionPaste(dataGridView);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionDelete(dataGridView);
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionUndo(dataGridView);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionRedo(dataGridView);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionFindAndReplace(dataGridView, false);
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ActionFindAndReplace(dataGridView, true);
        }

        private void toolStripMenuItemMapSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented");
        }

        private void toggleRowsAsFavouriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }

        private void markAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }

        private void removeAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }


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


        private void toggleShowFavouriteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionToggleStripMenuItem(dataGridView, toggleShowFavouriteRowsToolStripMenuItem);
            SetRowsVisbleStatus(dataGridView, toggleHideEqualRowsValuesToolStripMenuItem.Checked, toggleShowFavouriteRowsToolStripMenuItem.Checked);
        }

        private void toggleHideEqualRowsValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionToggleStripMenuItem(dataGridView, toggleHideEqualRowsValuesToolStripMenuItem);
            SetRowsVisbleStatus(dataGridView, toggleHideEqualRowsValuesToolStripMenuItem.Checked, toggleShowFavouriteRowsToolStripMenuItem.Checked);
        }

        #endregion

        #region KeyDownEventHandler
        public static void KeyDownEventHandler(object sender, KeyEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            KeyDownEventHandler(dataGridView, e);
        }

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

        #region Image handling - Update Image on File

        public static void UpdateImageOnFile(DataGridView dataGridView, FileEntryImage fileEntryImage)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingImage(dataGridView)) return;  //In progress doing so
            DataGridViewHandler.SetIsPopulatingImage(dataGridView, true);
            //-----------------------------------------------------------------


            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn)
                {
                    if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn column && column.FileEntryImage == fileEntryImage)
                    {
                        lock (fileEntryImage.Image)
                        {
                            column.FileEntryImage.Image = fileEntryImage.Image;
                        }
                        dataGridView.InvalidateCell(columnIndex, -1);
                    }
                }
            }
            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingImage(dataGridView, false);
            //-----------------------------------------------------------------
        }

        #endregion 

        #region CellPainting
        private const int roundedRadius = 8;

        #region CellPainting Draw Functions 
        public static void DrawImageAndSubText(object sender, DataGridViewCellPaintingEventArgs e, Image image, string text, Color backgroundColor)
        {
            //lock (image)
            {
                Rectangle rectangleRoundedCellBounds = CalulateCellRoundedRectangleCellBounds(e.CellBounds);
                Size thumbnailSize = CalulateCellImageSizeInRectagle(rectangleRoundedCellBounds, image.Size);

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
        }

        public static void DrawImageOnRightSide(object sender, DataGridViewCellPaintingEventArgs e, Image image)
        {
            e.Graphics.DrawImage(image,
                e.CellBounds.Left + e.CellBounds.Width - image.Width - 1,
                e.CellBounds.Top + 1);
        }

        private static void DrawIcon16x16OnRightSide(object sender, DataGridViewCellPaintingEventArgs e, Image image)
        {
            e.Graphics.DrawImage(image,
                 e.CellBounds.Left + e.CellBounds.Width - 20 - 1,
                 e.CellBounds.Top + 1, 16, 16); // e.CellBounds.Width, e.CellBounds.Height);
        }


        private static void DrawIcon16x16OnLeftSide(object sender, DataGridViewCellPaintingEventArgs e, Image image)
        {
            e.Graphics.DrawImage(image,
                e.CellBounds.Left + 1,
                e.CellBounds.Top + 1, 16, 16); // e.CellBounds.Width, e.CellBounds.Height);
        }

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

        public static void DrawIconsMetadataBrokerTypes(object sender, DataGridViewCellPaintingEventArgs e, MetadataBrokerTypes metadataBrokerTypes)
        {
            if ((metadataBrokerTypes & MetadataBrokerTypes.ExifTool) != 0)
            {
                Image image = (Image)Properties.Resources.tag_source_exiftool;
                e.Graphics.DrawImage(image,
                           e.CellBounds.Left + e.CellBounds.Width - image.Width - 1 - 33,
                           e.CellBounds.Top + 1); // e.CellBounds.Width, e.CellBounds.Height);
            }

            if ((metadataBrokerTypes & MetadataBrokerTypes.WindowsLivePhotoGallery) != 0)
            {
                Image image = (Image)Properties.Resources.tag_source_windows_live_photo_gallery;
                e.Graphics.DrawImage(image,
                           e.CellBounds.Left + e.CellBounds.Width - image.Width - 1 - 33 - 21,
                           e.CellBounds.Top + 1); // e.CellBounds.Width, e.CellBounds.Height);
            }

            if ((metadataBrokerTypes & MetadataBrokerTypes.MicrosoftPhotos) != 0)
            {
                Image image = (Image)Properties.Resources.tag_source_microsoft_photos;
                e.Graphics.DrawImage(image,
                           e.CellBounds.Left + e.CellBounds.Width - image.Width - 1 - 33 - 21 - 21,
                           e.CellBounds.Top + 1); // e.CellBounds.Width, e.CellBounds.Height);
            }

        }
        #endregion

        #region Calculate Region Rectangle
        public static Rectangle CalulateCellRoundedRectangleCellBounds(Rectangle rectangle)
        {
            return new Rectangle(rectangle.Left + 4, rectangle.Top + 4, rectangle.Width - 5, rectangle.Height - 5);
        }

        public static Size CalulateCellImageSizeInRectagle(Rectangle rectangleRoundedCellBounds, Size imageSize)
        {
            int thumbnailSpace = 4;

            float f = System.Math.Max(
                    (float)imageSize.Width / ((float)rectangleRoundedCellBounds.Width - (thumbnailSpace * 2f)),
                    (float)imageSize.Height / ((float)rectangleRoundedCellBounds.Height - (thumbnailSpace * 2f)));

            if (f < 1.0f) f = 1.0f; // Do not upsize small images
            return new Size ( (int)System.Math.Round((float)imageSize.Width / f), (int)System.Math.Round((float)imageSize.Height / f));
        }    

        public static Rectangle CalulateCellImageCenterInRectagle(Rectangle rectangleRoundedCellBounds, Size thumbnailSize)
        {
            return new Rectangle(rectangleRoundedCellBounds.X + ((rectangleRoundedCellBounds.Width - thumbnailSize.Width) / 2),
                rectangleRoundedCellBounds.Y + ((rectangleRoundedCellBounds.Height - thumbnailSize.Height) / 2),
                thumbnailSize.Width, thumbnailSize.Height);
        }
        #endregion

        #region CellPainting Mouse rectangle
        public static Rectangle GetRectangleFromMouseCoorinate(int x1, int y1, int x2, int y2)
        {
            int x = Math.Min(x1, x2);
            int y = Math.Min(y1, y2);
            int width = Math.Max(x1, x2) - x;
            int height = Math.Max(y1, y2) - y;
            return new Rectangle(x, y, width, height);
        }

        public static bool IsMouseWithinRectangle(int x, int y, Rectangle rectangle)
        {
            return (x >= rectangle.X && y >= rectangle.Y &&
                    x <= rectangle.X + rectangle.Width && y <= rectangle.Y + rectangle.Height);
        }

        public static void CellPaintingColumnHeaderMouseRegion(object sender, DataGridViewCellPaintingEventArgs e, bool drawingRegion, int x1, int y1, int x2, int y2)
        {
            if (!drawingRegion) return;
            Rectangle rectangle = GetRectangleFromMouseCoorinate(x1, y1, x2, y2);
            e.Graphics.DrawRectangle(new Pen(Color.White, 1), e.CellBounds.X + rectangle.X, e.CellBounds.Y + rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static bool UpdateSelectedCellsWithNewMouseRegion(DataGridView dataGridView, int columnIndex, int x1, int y1, int x2, int y2)
        {
            bool updated = false;
            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn == null) return updated;
            if (dataGridViewGenericColumn.ReadWriteAccess != ReadWriteAccess.AllowCellReadAndWrite) return updated;

            Image image = dataGridViewGenericColumn.FileEntryImage.Image;

            Rectangle rectangleRoundedCellBounds = CalulateCellRoundedRectangleCellBounds(
                new Rectangle (0, 0, dataGridView.Columns[columnIndex].Width, dataGridView.ColumnHeadersHeight));
            Size thumbnailSize = CalulateCellImageSizeInRectagle(rectangleRoundedCellBounds, image.Size);
            Rectangle rectangleCenterThumbnail = CalulateCellImageCenterInRectagle(rectangleRoundedCellBounds, thumbnailSize);

            Rectangle rectangleMouse = GetRectangleFromMouseCoorinate(x1, y1, x2, y2);
            Rectangle rectangleMouseThumb = new Rectangle(rectangleMouse.X - rectangleCenterThumbnail.X, rectangleMouse.Y - rectangleCenterThumbnail.Y, rectangleMouse.Width, rectangleMouse.Height);
            RectangleF region = RegionStructure.CalculateImageRegionAbstarctRectangle(thumbnailSize, rectangleMouseThumb, RegionStructureTypes.WindowsLivePhotoGallery);

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
                                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.On, false)));

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
        #endregion

        #region CellPaintingColumnHeaderRegionsInThumbnail
        public static void CellPaintingColumnHeaderRegionsInThumbnail(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
                if (dataGridViewGenericColumn == null) return;
                Image image = dataGridViewGenericColumn.FileEntryImage.Image;

                Rectangle rectangleRoundedCellBounds = CalulateCellRoundedRectangleCellBounds(e.CellBounds);
                Size thumbnailSize = CalulateCellImageSizeInRectagle(rectangleRoundedCellBounds, image.Size);

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
        #endregion

        #region Cell Paint Column Header, Favorite 
        public static void CellPaintingHandleDefault(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //----------------------------------------------------
            //NB Call this first to load Thumbnail 
            //DataGridViewUpdateThumbnail(dataGridViewTags, e);
            //----------------------------------------------------
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (DataGridViewHandler.GetIsPopulatingImage(dataGridView)) return;  //In progress updated the picture, can cause crash

            e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
            e.Handled = true;
        }

        public static void CellPaintingFavoriteAndToolTipsIcon(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex > -1 && e.ColumnIndex == -1)
            {
                if (dataGridView.Rows[e.RowIndex].HeaderCell.Tag is DataGridViewGenericRow)
                {
                    DataGridViewGenericRow dataGridViewGenericRow = (DataGridViewGenericRow)dataGridView.Rows[e.RowIndex].HeaderCell.Tag;
                    if (dataGridViewGenericRow.IsFavourite)
                    {
                        DrawIcon16x16OnLeftSide(sender, e, global::DataGridViewGeneric.Properties.Resources.Favorite);
                    }
                }
                if (!string.IsNullOrWhiteSpace(dataGridView.Rows[e.RowIndex].HeaderCell.ToolTipText))
                {
                    DrawIcon16x16OnLeftSide(sender, e, global::DataGridViewGeneric.Properties.Resources.ToolTipsText);
                }
            }
        }       
        
        public static void CellPaintingColumnHeader(object sender, DataGridViewCellPaintingEventArgs e, Dictionary<string, string> errorFileEntries)
        {
            
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                if (!(dataGridView.Columns[e.ColumnIndex].Tag is DataGridViewGenericColumn)) return;
                
                FileEntryImage fileEntryColumn = ((DataGridViewGenericColumn)dataGridView.Columns[e.ColumnIndex].Tag).FileEntryImage;
                DataGridViewGenericColumn dataGridViewGenericColumn = GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);


                bool hasFileKnownErrors = (errorFileEntries.ContainsKey(fileEntryColumn.FileEntry.FullFilePath));

                string cellText = "";
                if (dataGridViewGenericColumn.HasFileBeenUpdated) cellText += "File updated!!\r\n";


                if (dataGridViewGenericColumn.Metadata != null)
                {
                    switch (GetDataGridSizeLargeMediumSmall(dataGridView))
                    {
                        case DataGridViewSize.Small: //Small DataGridViewSize.Small | DataGridViewSize.RenameSize:
                            cellText += fileEntryColumn.FileName;
                            break;
                        case DataGridViewSize.Medium: //Medium DataGridViewSize.Medium | DataGridViewSize.RenameSize:
                            cellText += dataGridViewGenericColumn.Metadata.FileDateModified.ToString() + "\r\n" + fileEntryColumn.FileName;
                            break;
                        case DataGridViewSize.Large: //Large DataGridViewSize.Large | DataGridViewSize.RenameSize:
                            cellText += dataGridViewGenericColumn.Metadata.FileDateModified.ToString() + "\r\n" + fileEntryColumn.FullFilePath;
                            break;
                        default: 
                            throw new Exception("Not implemented");
                    }
                } else
                {
                    switch (GetDataGridSizeLargeMediumSmall(dataGridView))
                    {
                        case DataGridViewSize.Small: //Small
                            cellText += fileEntryColumn.FileName;
                            break;
                        case DataGridViewSize.Medium: //Medium
                            cellText += fileEntryColumn.LastWriteDateTime.ToString() + "\r\n" + fileEntryColumn.FileName;
                            break;
                        case DataGridViewSize.Large: //Large
                            cellText += fileEntryColumn.LastWriteDateTime.ToString() + "\r\n" + fileEntryColumn.FullFilePath;
                            break;
                        default: 
                            throw new Exception("Not implemented");
                    }
                }

                if (hasFileKnownErrors)
                    DrawImageAndSubText(sender, e, fileEntryColumn.Image, cellText, ColorHeaderError);
                else if (dataGridViewGenericColumn.HasFileBeenUpdated)
                    DrawImageAndSubText(sender, e, fileEntryColumn.Image, cellText, ColorHeaderWarning);
                else
                    DrawImageAndSubText(sender, e, fileEntryColumn.Image, cellText, ColorHeaderImage);
                    
            }
            
        }

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
                int keywordsStarts = DataGridViewHandler.GetRowHeadingItemStarts(dataGridView, header);

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
                            if ((DataGridViewHandler.GetCellStatusMetadataBrokerType(dataGridView, e.ColumnIndex, e.RowIndex) & MetadataBrokerTypes.ExifTool) != 0)
                                DataGridViewHandler.DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_on);
                            else
                                DataGridViewHandler.DrawImageOnRightSide(sender, e, (Image)Properties.Resources.tri_state_switch_on_add);
                        }
                        else
                        {
                            if ((DataGridViewHandler.GetCellStatusMetadataBrokerType(dataGridView, e.ColumnIndex, e.RowIndex) & MetadataBrokerTypes.ExifTool) != 0)
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
