using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using SqliteDatabase;
using DataGridViewGeneric;
using FileDateTime;
using System.Collections.Generic;
using Manina.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;
using System.Linq;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {

        #region Convert and Merge - Drag and Drop
        private Rectangle convertAndMergeDragBoxFromMouseDown;
        private int convertAndMergeRowIndexFromMouseDown;
        private int convertAndMergeRowIndexOfItemUnderMouseToDrop;
        private int oldIndex = -2;
        private int dragdropcurrentIndex = -1;

        #region Convert and Merge - Drag and Drop - General - Mouse Move
        private void dataGridViewConvertAndMerge_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (convertAndMergeDragBoxFromMouseDown != Rectangle.Empty && !convertAndMergeDragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dataGridView.DoDragDrop(dataGridView.Rows[convertAndMergeRowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
        }
        #endregion

        #region Convert and Merge - Drag and Drop - General - Mouse Down
        private void dataGridViewConvertAndMerge_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            
            convertAndMergeRowIndexFromMouseDown = dataGridView.HitTest(e.X, e.Y).RowIndex; // Get the index of the item the mouse is below.
            if (convertAndMergeRowIndexFromMouseDown != -1)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, convertAndMergeRowIndexFromMouseDown);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                {
                    // Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;
                    // Create a rectangle using the DragSize, with the mouse position being at the center of the rectangle.
                    convertAndMergeDragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
                else
                {
                    convertAndMergeRowIndexFromMouseDown = -1;
                    convertAndMergeDragBoxFromMouseDown = Rectangle.Empty;
                }
            }
            // Reset the rectangle if the mouse is not over an item in the ListBox.
            else convertAndMergeDragBoxFromMouseDown = Rectangle.Empty;
        }
        #endregion 

        #region Convert and Merge - Drag and Drop - Drag Over
        
        private void dataGridViewConvertAndMerge_DragOver(object sender, DragEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            Point clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));
            // Get the row index of the item the mouse is below. 
            int rowIndex = dataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (dataGridViewGenericRow != null /*&& dataGridViewGenericRow.IsHeader*/) e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;


            if (e.Effect == DragDropEffects.Move) dragdropcurrentIndex = dataGridView.HitTest(dataGridView.PointToClient(new Point(e.X, e.Y)).X, dataGridView.PointToClient(new Point(e.X, e.Y)).Y).RowIndex;
            else dragdropcurrentIndex = -1;

            if (oldIndex != dragdropcurrentIndex)
            {
                dataGridView.Invalidate();
                oldIndex = dragdropcurrentIndex;
            }
        }
        #endregion 

        #region Convert and Merge - Drag and Drop - General - Drag Drop
        private void dataGridViewConvertAndMerge_DragDrop(object sender, DragEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            // The mouse locations are relative to the screen, so they must be converted to client coordinates.
            Point clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            convertAndMergeRowIndexOfItemUnderMouseToDrop = dataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                int toRowIndex = convertAndMergeRowIndexOfItemUnderMouseToDrop + (convertAndMergeRowIndexFromMouseDown < convertAndMergeRowIndexOfItemUnderMouseToDrop ? 0 : 1);

                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridView.Rows.RemoveAt(convertAndMergeRowIndexFromMouseDown);
                dataGridView.Rows.Insert(toRowIndex, rowToMove);
                dataGridView.CurrentCell = DataGridViewHandler.GetCellDataGridViewCell(dataGridView, 0, toRowIndex);
            }

            dragdropcurrentIndex = -1;
            dataGridView.Invalidate();
        }
        #endregion

        #endregion


        private void dataGridViewConvertAndMerge_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }

        private void dataGridViewConvertAndMerge_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (e.RowIndex == -1) RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }
    }
}
