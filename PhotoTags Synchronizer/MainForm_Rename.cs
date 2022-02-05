using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : KryptonForm
    {
        #region comboBoxRenameVariableList_SelectionChangeCommitted
        private void comboBoxRenameVariableList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            textBoxRenameNewName.Focus();
            var insertText = comboBoxRenameVariableList.Text;
            var selectionIndex = textBoxRenameNewName.SelectionStart;
            textBoxRenameNewName.Text = textBoxRenameNewName.Text.Remove(selectionIndex, textBoxRenameNewName.SelectionLength);
            textBoxRenameNewName.Text = textBoxRenameNewName.Text.Insert(selectionIndex, insertText);
            textBoxRenameNewName.SelectionStart = selectionIndex + insertText.Length;
        }
        #endregion

        #region buttonRenameUpdate_Click
        private void buttonRenameUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewHandlerRename.UpdateFilenames(dataGridViewRename, Properties.Settings.Default.RenameVariable, checkBoxRenameShowFullPath.Checked);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to updated name on files");
                KryptonMessageBox.Show("Was not able to updated name on files.\r\n" + ex.Message, "Update name on files failed.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region checkBoxRenameShowFullPath_CheckedChanged
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
                KryptonMessageBox.Show("Was not able to updated name on files.\r\n" + ex.Message, "Update name on files failed.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region SaveRename
        private void SaveRename()
        {

            if (GlobalData.IsApplicationClosing) return;
            try
            {
                if (IsFileInAnyQueueLock(imageListView1.SelectedItems))
                {
                    KryptonMessageBox.Show("Can't start rename process, files being updated, need wait files finished with updating.", "Can't start rename", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                }
                else
                {                    
                    Dictionary<string, string> renameSuccess;
                    Dictionary<string, RenameToNameAndResult> renameFailed;
                    
                    DataGridViewHandlerRename.Write(dataGridViewRename, out renameSuccess, out renameFailed, checkBoxRenameShowFullPath.Checked);
                    //GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
                    UpdateImageViewListeAfterRename(imageListView1, renameSuccess, renameFailed, true);
                    //GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;

                    //GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
                    ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
                    //GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to rename files");
                KryptonMessageBox.Show("Was not able to rename files.\r\n" + ex.Message, "Rename files failed.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region buttonRenameSave_Click
        private void buttonRenameSave_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Rename")) return;
            if (IsPopulatingAnything("Rename")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            GlobalData.IsPerformingAButtonAction = true;
            //GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
            SaveRename();
            //GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
            GlobalData.IsPerformingAButtonAction = false;
        }
        #endregion

        #region dataGridViewRename_CellEnter
        private void dataGridViewRename_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = dataGridViewRename;
            RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }
        #endregion

        #region dataGridViewRename_CellMouseClick
        private void dataGridViewRename_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (e.RowIndex == -1) RegionSelectorLoadAndSelect(dataGridView, e.RowIndex, e.ColumnIndex);
        }
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
