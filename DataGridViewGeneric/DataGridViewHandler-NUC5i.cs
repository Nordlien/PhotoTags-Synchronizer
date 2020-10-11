using Manina.Windows.Forms;
using MetadataLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;

namespace DataGridViewGeneric
{
    public enum NewState
    {
        Set,
        Remove,
        Toggle
    }

    public class DataGridViewHandler
    {
        public static Color ColorReadOnly = System.Drawing.SystemColors.GradientInactiveCaption;
        public static Color ColorFavourite = System.Drawing.SystemColors.ControlLight;
        public static Color ColorReadOnlyFavourite = System.Drawing.SystemColors.MenuHighlight;
        public static Color ColorControl = SystemColors.Control;
        
        private DataGridView dataGridView;
        private string dataGridViewName = "";

        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ContextMenuStrip contextMenuStripDataGridViewGeneric;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem toggleRowsAsFavouriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleShowFavouriteRowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleHideEqualRowsValuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markAsFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAsFavoriteToolStripMenuItem;


        #region InitializeComponent and DataGridView

        public DataGridViewHandler(DataGridView dataGridView, string dataGridViewName, string topLeftHeaderCellName)
        {
            this.dataGridView = dataGridView;
            this.dataGridViewName = dataGridViewName;

            DataGridViewGenericData dataGridViewGenricData = new DataGridViewGenericData();
            dataGridView.TopLeftHeaderCell.Tag = dataGridViewGenricData;
            dataGridViewGenricData.Name = topLeftHeaderCellName;
            dataGridViewGenricData.FavoriteList = FavouriteRead(CreateFileName(this.dataGridViewName));
            dataGridView.TopLeftHeaderCell.Tag = dataGridViewGenricData;

            //ClipboardUtility.SetUndoAndRedoDataGridView(dataGridView);

            if (dataGridView.ContextMenuStrip == null)
            {
                InitializeComponent(this.dataGridView);
                dataGridView.ContextMenuStrip = contextMenuStripDataGridViewGeneric;
            }

        }

        public static void Clear(DataGridView dataGridView)
        {
            DataGridViewGenericData dataGridViewGenricData = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;

            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.TopLeftHeaderCell.Tag = dataGridViewGenricData;
            dataGridView.TopLeftHeaderCell.Value = dataGridViewGenricData.Name;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersHeight = 200;
            dataGridView.RowHeadersWidth = 230;
            dataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
            typeof(DataGridView).InvokeMember(
                   "DoubleBuffered",
                   BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                   null,
                   dataGridView,
                   new object[] { true });
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

        public static bool HasColumnFilename(DataGridView dataGridView, string fullFilePath)
        {
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn &&
                    ((DataGridViewGenericColumn)dataGridView.Columns[columnIndex].Tag).FileEntryImage.FullFilePath == fullFilePath)
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetColumnIndex(DataGridView dataGridView, FileEntry fileEntry)
        {
            //TODO: Add cache dictonary
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn &&
                    ((DataGridViewGenericColumn)dataGridView.Columns[columnIndex].Tag).FileEntryImage == fileEntry)
                {
                    return columnIndex;
                }
            }
            return -1; //Not found
        }

        

        public static List<FileEntryImage> GetColumns(DataGridView dataGridView)
        {
            List<FileEntryImage> fileEntryImages = new List<FileEntryImage>();

            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn)
                {
                    fileEntryImages.Add(((DataGridViewGenericColumn)dataGridView.Columns[columnIndex].Tag).FileEntryImage);
                }
            }

            return fileEntryImages;
        }

        public static void AddColumnOrUpdate(DataGridView dataGridView, ImageListViewSelectedItemCollection imageListViewItems)
        {
            AddColumnSelectedFiles(dataGridView, imageListViewItems, false);
        }

        public static void AddColumnSelectedFiles(DataGridView dataGridView, ImageListViewSelectedItemCollection imageListViewItems, bool forceReadOnly)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewItems)
            {
                AddColumnOrUpdate(dataGridView, new FileEntryImage(imageListViewItem.FullFileName, imageListViewItem.DateModified,
                    (Image)imageListViewItem.ThumbnailImage.Clone()), null, forceReadOnly);
            }
        }

        public static int AddColumnOrUpdate(DataGridView dataGridView, FileEntryImage fileEntryImage)
        {
            return AddColumnOrUpdate(dataGridView, fileEntryImage, null, false);
        }

        public static int AddColumnOrUpdate(DataGridView dataGridView, FileEntryImage fileEntryImage, Metadata metadata, bool readOnly)
        {
            int columnIndex = GetColumnIndex(dataGridView, fileEntryImage);

            if (columnIndex < 0) //Column not found, add a new column
            {
                columnIndex = dataGridView.Columns.Add(fileEntryImage.FullFilePath, fileEntryImage.FullFilePath);
                dataGridView.Columns[columnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView.Columns[columnIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView.Columns[columnIndex].Width = 200;
            }

            //Updated column information
            if (readOnly)
            {
                dataGridView.Columns[columnIndex].ReadOnly = readOnly;
                dataGridView.Columns[columnIndex].DefaultCellStyle.BackColor = ColorReadOnly;
            }
            else
            {
                dataGridView.Columns[columnIndex].ReadOnly = readOnly;
                dataGridView.Columns[columnIndex].DefaultCellStyle.BackColor = ColorControl;
            }
            dataGridView.Columns[columnIndex].Tag = new DataGridViewGenericColumn(fileEntryImage, metadata, readOnly);
            return columnIndex;
        }

        public static bool IsColumnDataGridViewGenericColumn(DataGridView dataGridView, int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > dataGridView.ColumnCount) return false;
            return dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn;
        }

        public static DataGridViewGenericColumn GetColumnDataGridViewGenericColumn(DataGridView dataGridView, int columnIndex)
        {
            if (columnIndex < 0) return null;
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

        public static bool IsDataGridViewGenericRow(DataGridView dataGridView, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex > dataGridView.RowCount) return false;
            return dataGridView.Rows[rowIndex].HeaderCell.Tag is DataGridViewGenericRow;
        }

        public static DataGridViewGenericRow GetRowDataGridViewGenericRow(DataGridView dataGridView, int rowIndex)
        {
            if (rowIndex < 0) return null;
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

        public static int FindFileEntryRow(DataGridView dataGridView, DataGridViewGenericRow dataGridViewGenericRow)
        {
            int lastHeaderRowFound = -1;
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                if (dataGridView.Rows[rowIndex].HeaderCell.Tag is DataGridViewGenericRow)
                {

                    if (dataGridViewGenericRow.IsHeader && //It correct header
                       dataGridViewGenericRow.HeaderName == ((DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag).HeaderName
                       )
                        return rowIndex;

                    if (!dataGridViewGenericRow.IsHeader && //It correct row
                        dataGridViewGenericRow.HeaderName == ((DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag).HeaderName &&
                        dataGridViewGenericRow.RowName == ((DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag).RowName
                        )
                        return rowIndex;

                    if (!dataGridViewGenericRow.IsHeader && //Remmeber last header
                        dataGridViewGenericRow.HeaderName == ((DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag).HeaderName
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
                if (dataGridView.Rows[rowIndex].HeaderCell.Tag is DataGridViewGenericRow &&
                    (DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag == dataGridViewGenericRow)
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



        public static void AddRows(DataGridView dataGridView, FileEntryImage fileEntryColumn, List<DataGridViewGenericRow> dataGridViewGenericRows)
        {
            AddRows(dataGridView, fileEntryColumn, dataGridViewGenericRows, GetFavoriteList(dataGridView));
        }

        public static void AddRows(DataGridView dataGridView, FileEntryImage fileEntryColumn, List<DataGridViewGenericRow> dataGridViewGenericRows, List<FavoriteRow> dataGridFavorites)
        {
            int columnIndex = GetColumnIndex(dataGridView, fileEntryColumn);

            foreach (DataGridViewGenericRow dataGridViewGenericRow in dataGridViewGenericRows)
            {
                AddRow(dataGridView, columnIndex, dataGridViewGenericRow, dataGridFavorites, false, false);
            }
        }

        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow, bool changeCellFlag, bool cellReadOnly)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericRow, GetFavoriteList(dataGridView), changeCellFlag, cellReadOnly);
        }

        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericRow, GetFavoriteList(dataGridView), false, false);
        }


        public static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow,
            List<FavoriteRow> dataGridFavorites, bool changeCellFlag, bool cellReadOnly)
        {
            int rowIndex = FindFileEntryRow(dataGridView, dataGridViewGenericRow);

            if (rowIndex == -1 ||
                ((DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag).HeaderName != dataGridViewGenericRow.HeaderName ||
                ((DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag).RowName != dataGridViewGenericRow.RowName)
            {
                //New row added
                if (rowIndex == -1)
                    rowIndex = GetRowCountWithoutEditRow(dataGridView);
                else
                    rowIndex++;

                dataGridView.Rows.Insert(rowIndex, 1);

                UpdateRowDataGridViewGenericDataRow(dataGridView, rowIndex, dataGridViewGenericRow);

            }

            //If a value row, set the value
            if (!dataGridViewGenericRow.IsHeader)
            {
                dataGridView[columnIndex, rowIndex].Value = dataGridViewGenericRow.Value;
            }
            //if (changeCellFlag)
            
            dataGridView[columnIndex, rowIndex].ReadOnly = cellReadOnly;
            SetCellColor(dataGridView, columnIndex, rowIndex);
            
            

            //SetRowEqualFlag(dataGridView, rowIndex);
            SetRowFavoriteFlag(dataGridView, rowIndex, dataGridFavorites);
            SetRowHeaderColor(dataGridView.Rows[rowIndex]);

            return rowIndex;
        }

        public static void UpdateRowDataGridViewGenericDataRow(DataGridView dataGridView, int rowIndex, DataGridViewGenericRow dataGridViewGenericRow)
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
            }

            if (dataGridViewGenericRow.ReadOnly)
            {
                dataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = ColorReadOnly;
                dataGridView.Rows[rowIndex].ReadOnly = true;
            }
        }

        #endregion

        #region Row handling - Favorite handling
        public static List<FavoriteRow> GetFavoriteList(DataGridView dataGridView)
        {
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return null;

            DataGridViewGenericData dataGridViewGenricData = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
            return dataGridViewGenricData.FavoriteList;
        }

        public static void SetFavoriteList(DataGridView dataGridView, List<FavoriteRow> dataGridFavoriteList)
        {
            if (dataGridView.TopLeftHeaderCell.Tag.GetType() != typeof(DataGridViewGenericData)) return;

            DataGridViewGenericData dataGridViewGenricData = (DataGridViewGenericData)dataGridView.TopLeftHeaderCell.Tag;
            dataGridViewGenricData.FavoriteList = dataGridFavoriteList;
        }

        private static string CreateFileName(string dataGridViewName)
        {
            //string jsonPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
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

        

        public static void ActionSetRowsFavouriteState(DataGridView dataGridView, string dataGridViewName, NewState newState)
        {
            List<FavoriteRow> favouriteRows = new List<FavoriteRow>();
            List<int> selectedRows = GetRowSelected(dataGridView);

            foreach (int rowIndex in selectedRows)
            {
                if (dataGridView.Rows[rowIndex].HeaderCell.Tag is DataGridViewGenericRow)
                {
                    dataGridView.InvalidateCell(dataGridView.Rows[rowIndex].HeaderCell);

                    DataGridViewGenericRow dataGridViewGenericRow = (DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag;


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

                    FavoriteRow favouriteRow = ((DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag).GetFavouriteRow();
                    if (dataGridViewGenericRow.IsFavourite)
                    {
                        if (!favouriteRows.Contains(favouriteRow)) favouriteRows.Add(favouriteRow);
                    }
                    else
                    {
                        if (favouriteRows.Contains(favouriteRow)) favouriteRows.Remove(favouriteRow);
                    }
                    SetRowHeaderColor(dataGridView.Rows[rowIndex]);
                }
            }

            FavouriteWrite(CreateFileName(dataGridViewName), favouriteRows);
        }

        private static void SetRowFavoriteFlag(DataGridView dataGridView, int rowIndex, List<FavoriteRow> dataGridFavorites)
        {
            DataGridViewGenericRow dataGridViewGenericRow = (DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag;
            dataGridViewGenericRow.IsFavourite =
                dataGridFavorites.Contains(new FavoriteRow(dataGridViewGenericRow.HeaderName, dataGridViewGenericRow.RowName, dataGridViewGenericRow.IsHeader));
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
                    /*
                    if (!(firstValue == null && dataGridView[columnIndex, rowIndex].Value == null))
                    {
                        if ((firstValue == null && dataGridView[columnIndex, rowIndex].Value != null) ||
                            (firstValue != null && dataGridView[columnIndex, rowIndex].Value == null) ||
                            (firstValue.ToString() != dataGridView[columnIndex, rowIndex].Value.ToString())
                            )
                        {
                            return true;
                        }
                    }*/
                }
            }
            return false;
        }

        private static void SetRowEqualFlag(DataGridView dataGridView, int rowIndex)
        {
            ((DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag).IsEqual = !IsRowValuesDifferent(dataGridView, rowIndex);
        }
        #endregion

        #region Row handling - Header color and Visible Flag
        private static void SetRowHeaderColor(DataGridViewRow dataGridViewRow)
        {
            if (dataGridViewRow.HeaderCell.Tag is DataGridViewGenericRow)
            {
                DataGridViewGenericRow dataGridViewGenericRow = (DataGridViewGenericRow)dataGridViewRow.HeaderCell.Tag;
                if (dataGridViewGenericRow.IsFavourite && !dataGridViewGenericRow.ReadOnly)
                    dataGridViewRow.DefaultCellStyle.BackColor = ColorFavourite;
                else if (!dataGridViewGenericRow.IsFavourite && dataGridViewGenericRow.ReadOnly)
                    dataGridViewRow.DefaultCellStyle.BackColor = ColorReadOnly;
                else if (dataGridViewGenericRow.IsFavourite && dataGridViewGenericRow.ReadOnly)
                    dataGridViewRow.DefaultCellStyle.BackColor = ColorReadOnlyFavourite;
                else
                    dataGridViewRow.DefaultCellStyle.BackColor = Color.Empty;
            }
        }

        private static void SetRowVisbleStatus(DataGridView dataGridView, int rowIndex, bool showOnlyDiffrentValues, bool showOnlyFavorite)
        {
            SetRowEqualFlag(dataGridView, rowIndex);
            //SetRowFavoriteFlag(dataGridView, rowIndex);

            if (dataGridView.Rows[rowIndex].HeaderCell.Tag is DataGridViewGenericRow)
            {
                DataGridViewGenericRow dataGridViewGenericRow = (DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag;
                bool visble = true;

                if (!dataGridViewGenericRow.IsHeader && dataGridViewGenericRow.IsEqual && showOnlyDiffrentValues) visble = false;
                if (!dataGridViewGenericRow.IsFavourite && showOnlyFavorite) visble = false;

                dataGridView.Rows[rowIndex].Visible = visble;
            }
        }

        private static void SetRowsVisbleStatus(DataGridView dataGridView, bool showOnlyEqual, bool showOnlyFavorite)
        {
            for (int rowIndex = 0; rowIndex < GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                SetRowVisbleStatus(dataGridView, rowIndex, showOnlyEqual, showOnlyFavorite);
            }
        }
        #endregion

        #region Cell Handling
        private static void SetCellColor(DataGridView dataGridView, int columnIndex, int rowIndex)
        {

            DataGridViewGenericRow dataGridViewGenericRow = (DataGridViewGenericRow)dataGridView.Rows[rowIndex].HeaderCell.Tag;

            if (dataGridViewGenericRow.IsFavourite && !dataGridView[columnIndex, rowIndex].ReadOnly)
                dataGridView[columnIndex, rowIndex].Style.BackColor = ColorFavourite;
            else if (!dataGridViewGenericRow.IsFavourite && dataGridView[columnIndex, rowIndex].ReadOnly)
                dataGridView[columnIndex, rowIndex].Style.BackColor = ColorReadOnly;
            else if (dataGridViewGenericRow.IsFavourite && dataGridView[columnIndex, rowIndex].ReadOnly)
                dataGridView[columnIndex, rowIndex].Style.BackColor = ColorReadOnlyFavourite;
            else
                dataGridView[columnIndex, rowIndex].Style.BackColor = Color.Empty;
        }

        public static object GetCellValue(DataGridView dataGridView, FileEntry columnFileEntry, string headerName, string rowName)
        {
            int columnIndex = GetColumnIndex(dataGridView, columnFileEntry);
            return GetCellValue(dataGridView, columnIndex, headerName, rowName);
        }

        public static object GetCellValue(DataGridView dataGridView, int columnIndex, string headerName, string rowName)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
            if (columnIndex > -1 && rowIndex > -1)
                return GetCellValue(dataGridView,columnIndex, rowIndex);
            else
                return null;
        }
        public static object GetCellValue(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView[columnIndex, rowIndex].Value;
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

        public static void SetCell(DataGridView dataGridView, int columnIndex, string headerName, string rowName, DataGridViewCell dataGridViewCell)
        {
            int rowIndex = GetRowIndex(dataGridView, new DataGridViewGenericRow(headerName, rowName));
            SetCell(dataGridView, columnIndex, rowIndex, dataGridViewCell);
        }
        public static void SetCell(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewCell dataGridViewCell)
        {
            if (rowIndex > -1 && columnIndex > -1) dataGridView[columnIndex, rowIndex] = dataGridViewCell;
        }


        public static object GetCellTag(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return dataGridView[columnIndex, rowIndex].Tag;
        }

        public static void SetCellTag(DataGridView dataGridView, int columnIndex, int rowIndex, object value)
        {
            if (rowIndex > -1 && columnIndex > -1) dataGridView[columnIndex, rowIndex].Tag = value;
        }

        public static DataGridViewGenericCell GetCellDataGridViewGenericCell(DataGridView dataGridView, int columnIndex, int rowIndex)
        {
            return new DataGridViewGenericCell(
                GetCellValue(dataGridView, columnIndex, rowIndex), 
                GetCellTag(dataGridView, columnIndex, rowIndex));
        }

        public static void SetCellDataGridViewGenericCell(DataGridView dataGridView, int columnIndex, int rowIndex, DataGridViewGenericCell dataGridViewGenericCell)
        {
            if (rowIndex > -1 && columnIndex > -1)
            {
                SetCellValue(dataGridView, columnIndex, rowIndex, dataGridViewGenericCell.Value);
                SetCellTag(dataGridView, columnIndex, rowIndex, dataGridViewGenericCell.Tag);
            }
        }
        #endregion

        #region Copy select Media Broker as Windows Life Photo Gallery, Microsoft Photots, Google Location History, etc... to correct Media File Tag
        private static Dictionary<CellLocation, object> CopyCellFromBrokerToMedia(DataGridView dataGridView, string targetHeader, int columnIndex, int rowIndex, bool doOwerwriteData)
        {
            Dictionary<CellLocation, object> updatedCells = new Dictionary<CellLocation, object>();

            DataGridViewGenericRow dataGridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            int targetRowIndex = DataGridViewHandler.GetRowIndex(dataGridView, targetHeader, dataGridViewGenericDataRow.RowName);
            if (targetRowIndex != -1)
            {
                if (doOwerwriteData ||
                (!doOwerwriteData && (GetCellValue(dataGridView, columnIndex, targetRowIndex) == null || string.IsNullOrWhiteSpace(GetCellValue(dataGridView, columnIndex, targetRowIndex).ToString()))
                ))
                {
                    CellLocation cellLocation = new CellLocation(columnIndex, targetRowIndex);
                    if (!updatedCells.ContainsKey(cellLocation)) updatedCells.Add(cellLocation, DataGridViewHandler.GetCellValue(dataGridView, columnIndex, targetRowIndex));

                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, targetRowIndex,
                        DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex) );
                }
            }            
            return updatedCells;
        }

        public static void CopySelectedCellFromBrokerToMedia(DataGridView dataGridView, string targetHeader, bool overwrite)
        {
            Dictionary<CellLocation, object> updatedCells = new Dictionary<CellLocation, object>();

            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                Dictionary<CellLocation, object> updatedCellsDelta = 
                    CopyCellFromBrokerToMedia(dataGridView, targetHeader, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex, overwrite);

                if (updatedCellsDelta != null && updatedCellsDelta.Count > 0)
                {
                    foreach (CellLocation cellLocation in updatedCellsDelta.Keys)
                    {
                        if (!updatedCells.ContainsKey(cellLocation)) updatedCells.Add(cellLocation, updatedCellsDelta[cellLocation]);
                    }
                }
            }

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

        private void toggleRowsAsFavouriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionSetRowsFavouriteState(dataGridView, dataGridViewName, NewState.Toggle);
        }

        private void markAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionSetRowsFavouriteState(dataGridView, dataGridViewName, NewState.Set);
        }

        private void removeAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionSetRowsFavouriteState(dataGridView, dataGridViewName, NewState.Remove);
        }


        public static void ActionToggleShowFavouriteRows(DataGridView dataGridView, ToolStripMenuItem toolStripMenuItem)
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
            ////////////////////////////////////////////////////////////////////////
            SetRowsVisbleStatus(dataGridView, toolStripMenuItem.Checked, toolStripMenuItem.Checked);

        }

        public static void ActionToggleHideEqualRowsValues(DataGridView dataGridView, ToolStripMenuItem toolStripMenuItem)
        {
            //DataGridView dataGridView = ((DataGridView)sender);
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
            SetRowsVisbleStatus(dataGridView, toolStripMenuItem.Checked, toolStripMenuItem.Checked);
        }

        private void toggleShowFavouriteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionToggleShowFavouriteRows(dataGridView, toggleShowFavouriteRowsToolStripMenuItem);
        }

        private void toggleHideEqualRowsValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!dataGridView.Enabled) return;
            ActionToggleHideEqualRowsValues(dataGridView, toggleHideEqualRowsValuesToolStripMenuItem);
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
                    if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn &&
                        ((DataGridViewGenericColumn)dataGridView.Columns[columnIndex].Tag).FileEntryImage == fileEntryImage)
                    {
                        lock (fileEntryImage.Image)
                        {
                            ((DataGridViewGenericColumn)dataGridView.Columns[columnIndex].Tag).FileEntryImage.Image = fileEntryImage.Image;
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
        public static void CellPaintingThumbnailAndText(object sender, DataGridViewCellPaintingEventArgs e, Image image)
        {            
            lock (image)
            {
                int roundedLeft = e.CellBounds.Left + 4;
                int roundedTop = e.CellBounds.Top + 4;
                int roudnedWidth = e.CellBounds.Width - 5;
                int roundedHeight = e.CellBounds.Height - 5;
                int roundedRadius = 8;
                int thumbnailSpace = 4;


                float f = System.Math.Max(
                        (float)image.Width / ((float)roudnedWidth - (thumbnailSpace * 2f)),
                        (float)image.Height / ((float)roundedHeight - (thumbnailSpace * 2f)));

                if (f < 1.0f) f = 1.0f; // Do not upsize small images
                int thumbnailWidth = (int)System.Math.Round((float)image.Width / f);
                int thumbnailHeight = (int)System.Math.Round((float)image.Height / f);


                e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor),
                    new Rectangle(e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Width, e.CellBounds.Height));

                Manina.Windows.Forms.Utility.FillRoundedRectangle(e.Graphics, new SolidBrush(Color.LightSteelBlue),
                    new Rectangle(roundedLeft, roundedTop, roudnedWidth, roundedHeight), roundedRadius);
                Manina.Windows.Forms.Utility.DrawRoundedRectangle(e.Graphics, new Pen(Color.Black, 3),
                    new Rectangle(roundedLeft, roundedTop, roudnedWidth, roundedHeight), roundedRadius);
                e.Graphics.DrawImage(image,
                    roundedLeft + ((roudnedWidth - thumbnailWidth) / 2),
                    roundedTop + ((roundedHeight - thumbnailHeight) / 2),
                    thumbnailWidth, thumbnailHeight);
                e.Graphics.DrawLine(new Pen(Color.Silver),
                    e.CellBounds.Left,
                    e.CellBounds.Top + e.CellBounds.Height - 1,
                    e.CellBounds.Left + e.CellBounds.Width - 1,
                    e.CellBounds.Top + e.CellBounds.Height - 1);

                SizeF sizeF = e.Graphics.MeasureString(e.Value.ToString(),
                    ((DataGridView)sender).Font,
                    new SizeF(roudnedWidth, roundedHeight));

                var rectF = new RectangleF(roundedLeft + 2, roundedTop + 2, sizeF.Width, sizeF.Height);
                //Filling a 50% transparent rectangle before drawing the string.
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), rectF);

                e.Graphics.DrawString(
                    e.Value.ToString(),
                    ((DataGridView)sender).Font,
                    new SolidBrush(Color.Black),
                    rectF);
            }
        }

        public static void CellPaintingMetadataBrokerTypes(object sender, DataGridViewCellPaintingEventArgs e, MetadataBrokerTypes metadataBrokerTypes)
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
        private static void CellPaintingImageOnRightSide(object sender, DataGridViewCellPaintingEventArgs e, Image image)
        {
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All); 

            e.Graphics.DrawImage(image,
                       e.CellBounds.Left + e.CellBounds.Width - 20 - 1,
                       e.CellBounds.Top + 1, 16, 16); // e.CellBounds.Width, e.CellBounds.Height);
        }

        public static void CellPaintingFavorite(object sender, DataGridViewCellPaintingEventArgs e)
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
                        CellPaintingImageOnRightSide(sender, e, global::DataGridViewGeneric.Properties.Resources.Favorite);
                        e.Handled = true;
                    }
                }
            }
        }

        public static void CellPaintingColumnHeader(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                if (!(dataGridView.Columns[e.ColumnIndex].Tag is DataGridViewGenericColumn)) return;
                
                FileEntryImage fileEntryColumn = ((DataGridViewGenericColumn)dataGridView.Columns[e.ColumnIndex].Tag).FileEntryImage;
                CellPaintingThumbnailAndText(sender, e, fileEntryColumn.Image);
                e.Handled = true;
            }
        }

        public static void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //----------------------------------------------------
            //NB Call this first to load Thumbnail 
            //DataGridViewUpdateThumbnail(dataGridViewTags, e);
            //----------------------------------------------------

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            if (DataGridViewHandler.GetIsPopulatingImage(dataGridView))
                return;  //In progress updated the picture, can cause crash
            

            CellPaintingFavorite(sender, e);
            CellPaintingColumnHeader(sender, e);            
        }
        #endregion

    }
}
