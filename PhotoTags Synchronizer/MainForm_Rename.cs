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

        private void textBoxRenameNewName_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.RenameVariable = textBoxRenameNewName.Text;
        }

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
                MessageBox.Show("Was not able to updated name on files.\r\n" + ex.Message, "Update name on files failed.");
            }
        }

        private void checkBoxRenameShowFullPath_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridViewHandlerRename.UpdateFilenames(dataGridViewRename, Properties.Settings.Default.RenameVariable, checkBoxRenameShowFullPath.Checked);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to updated name on files");
                MessageBox.Show("Was not able to updated name on files.\r\n" + ex.Message, "Update name on files failed.");
            }
        }
        private void buttonRenameSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsFileInThreadQueueLock(imageListView1.SelectedItems))
                {
                    MessageBox.Show("Can't start rename process, files being updated, need wait files finished with updating.", "Can't start rename");
                }
                else
                {
                    Dictionary<string, string> renameSuccess;
                    Dictionary<string, string> renameFailed;

                    DataGridViewHandlerRename.Write(dataGridViewRename, out renameSuccess, out renameFailed, checkBoxRenameShowFullPath.Checked);

                    UpdateImageViewListeAfterRename(imageListView1, renameSuccess, renameFailed, true);

                    FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems);
                }
            } catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to rename files");
                MessageBox.Show("Was not able to rename files.\r\n" + ex.Message, "Rename files failed.");
            }
        }

        #region Painting
        #endregion
    }
}
