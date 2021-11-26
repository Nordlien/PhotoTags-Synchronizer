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
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        

        private void comboBoxRenameVariableList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            textBoxRenameNewName.Focus();
            var insertText = comboBoxRenameVariableList.Text;
            var selectionIndex = textBoxRenameNewName.SelectionStart;
            textBoxRenameNewName.Text = textBoxRenameNewName.Text.Remove(selectionIndex, textBoxRenameNewName.SelectionLength);
            textBoxRenameNewName.Text = textBoxRenameNewName.Text.Insert(selectionIndex, insertText);
            textBoxRenameNewName.SelectionStart = selectionIndex + insertText.Length;
        }

        private void buttonRenameUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewHandlerRename.UpdateFilenames(dataGridViewRename, Properties.Settings.Default.RenameVariable, checkBoxRenameShowFullPath.Checked);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to updated name on files");
                KryptonMessageBox.Show("Was not able to updated name on files.\r\n" + ex.Message, "Update name on files failed.");
            }
        }

        private void checkBoxRenameShowFullPath_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.RenameShowFullPath = checkBoxRenameShowFullPath.Checked;
                DataGridViewHandlerRename.UpdateFilenames(dataGridViewRename, Properties.Settings.Default.RenameVariable, checkBoxRenameShowFullPath.Checked);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to updated name on files");
                KryptonMessageBox.Show("Was not able to updated name on files.\r\n" + ex.Message, "Update name on files failed.");
            }
        }

        private void SaveRename()
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;
            try
            {
                if (IsFileInThreadQueueLock(imageListView1.SelectedItems))
                {
                    KryptonMessageBox.Show("Can't start rename process, files being updated, need wait files finished with updating.", "Can't start rename");
                }
                else
                {                    
                    Dictionary<string, string> renameSuccess;
                    Dictionary<string, string> renameFailed;

                    DataGridViewHandlerRename.Write(dataGridViewRename, out renameSuccess, out renameFailed, checkBoxRenameShowFullPath.Checked);

                    UpdateImageViewListeAfterRename(imageListView1, renameSuccess, renameFailed, true);

                    OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to rename files");
                KryptonMessageBox.Show("Was not able to rename files.\r\n" + ex.Message, "Rename files failed.");
            }
        }

        private void buttonRenameSave_Click(object sender, EventArgs e)
        {
            SaveRename();
        }

        private void dataGridViewRename_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = dataGridViewRename;
            RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }

        private void dataGridViewRename_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (e.RowIndex == -1) RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }

        #region Painting
        #endregion

        #region Control with focus (For Cut/Copy/Paste)

        private Control controlPasteWithFocusRename = null;
        private void textBoxRenameNewName_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.RenameVariable = textBoxRenameNewName.Text;
            controlPasteWithFocusRename = null;
        }

        private void textBoxRenameNewName_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusRename = (Control)sender;
        }

        private void dataGridViewRename_Leave(object sender, EventArgs e)
        {
            controlPasteWithFocusRename = null;
        }

        private void dataGridViewRename_Enter(object sender, EventArgs e)
        {
            controlPasteWithFocusRename = (Control)sender;
        }
        #endregion

    }
}
