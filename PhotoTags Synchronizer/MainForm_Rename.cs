using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Krypton.Toolkit;
using DataGridViewGeneric;
using FileHandeling;

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : KryptonForm
    {
        #region comboBoxRenameVariableList_SelectionChangeCommitted
        private void comboBoxRenameVariableList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            textBoxRenameNewName.Focus();
            Clipboard.SetText(comboBoxRenameVariableList.Text);
            textBoxRenameNewName.Paste();
        }
        #endregion

        #region buttonRenameUpdate_Click
        private void buttonRenameUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                DataGridViewHandlerRename.ShowFullPath = Properties.Settings.Default.RenameShowFullPath;
                DataGridViewHandlerRename.ComputerNames = new List<string>(oneDriveNetworkNames);
                DataGridViewHandlerRename.GPStag = Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix;
                DataGridViewHandlerRename.UpdateFilenames(dataGridViewRename, checkBoxRenameShowFullPath.Checked);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to updated name on files");
                KryptonMessageBox.Show("Was not able to updated name on files.\r\n" + ex.Message, "Update name on files failed.", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region checkBoxRenameShowFullPath_CheckedChanged
        private void checkBoxRenameShowFullPath_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.RenameShowFullPath = checkBoxRenameShowFullPath.Checked;
                DataGridViewHandlerRename.UpdateFilenames(dataGridViewRename, checkBoxRenameShowFullPath.Checked);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to updated name on files");
                KryptonMessageBox.Show("Was not able to updated name on files.\r\n" + ex.Message, "Update name on files failed.", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
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
                    DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;

                    using (new WaitCursor())
                    {
                        DataGridView dataGridView = dataGridViewRename;

                        int columnIndex = DataGridViewHandler.GetColumnIndexFirstFullFilePath(dataGridView, DataGridViewHandlerRename.headerNewFilename, false);
                        if (columnIndex == -1) return;

                        for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                        {
                            DataGridViewGenericCell cellGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, columnIndex, rowIndex);

                            if (!cellGridViewGenericCell.CellStatus.CellReadOnly)
                            {
                                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);

                                #region Get Old filename from grid
                                string oldFilename = dataGridViewGenericRow.RowName;
                                string oldDirectory = dataGridViewGenericRow.HeaderName;
                                string oldFullFilename = FileHandler.CombinePathAndName(oldDirectory, oldFilename);
                                #endregion

                                AddQueueRenameMediaFilesLock(oldFullFilename, DataGridViewHandlerRename.RenameVaribale); 
                            }
                        }
                    }
                }
                else
                {                    
                    Dictionary<string, string> renameSuccess;
                    Dictionary<string, RenameToNameAndResult> renameFailed;
                    HashSet<string> directoriesCreated;
                    
                    DataGridViewHandlerRename.Write(dataGridViewRename, out renameSuccess, out renameFailed, out directoriesCreated, checkBoxRenameShowFullPath.Checked);
                    ImageViewListeUpdateAfterRename(imageListView1, renameSuccess, renameFailed, true);

                    foreach (string newDirector in directoriesCreated)
                    {
                        GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = true;
                        TreeViewFolderBrowserHandler.RefreshFolderWithName(treeViewFolderBrowser1, newDirector, true);
                        GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = false;
                    }

                    ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Was not able to rename files");
                KryptonMessageBox.Show("Was not able to rename files.\r\n" + ex.Message, "Rename files failed.", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
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

            try
            {
                GlobalData.IsPerformingAButtonAction = true;
                SaveRename();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
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
