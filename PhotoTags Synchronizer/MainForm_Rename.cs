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
            DataGridViewHandlerRename.UpdateFilenames(dataGridViewRename, Properties.Settings.Default.RenameVariable);
        }
        

        private ImageListViewItem FindItemInImageListView(ImageListViewItemCollection imageListViewItemCollection, string fullFilename)
        {            
            ImageListViewItem foundItem = null;
            foreach (ImageListViewItem item in imageListViewItemCollection)
            {
                if (item.FullFileName == fullFilename)
                {
                    foundItem = item;
                    break;
                }
            }
            return foundItem;
        }

        private void UpdateImageViewListeAfterRename(Dictionary<string, string> renameSuccess, Dictionary<string, string> renameFailed, bool onlyRenameAddbackToListView)
        {            
            //GlobalData.DoNotRefreshImageListView = true;
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;

            //Remove items with old names
            imageListView1.SuspendLayout();
            foreach (string filename in renameSuccess.Keys)
            {
                ImageListViewItem foundItem = FindItemInImageListView(imageListView1.Items, filename);
                if (foundItem != null) imageListView1.Items.Remove(foundItem);
            }

            //Add new renames back to list
            if (onlyRenameAddbackToListView)
            {
                foreach (string filename in renameSuccess.Values)
                {
                    imageListView1.Items.Add(filename);
                }
            }
            
            #region Also select items that didn't got renamed due to error in the ImageListView 
            foreach (string filename in renameFailed.Keys)
            {
                AddError(filename, "Failed rename " + filename + " to : " + renameFailed[filename]);
                ImageListViewItem foundItem = FindItemInImageListView(imageListView1.Items, filename);
                if (foundItem != null) foundItem.Selected = true;
            }
            imageListView1.ResumeLayout();
            #endregion

            

            if (onlyRenameAddbackToListView)
            {
                foreach (string filename in renameSuccess.Values)
                {
                    ImageListViewItem foundItem = FindItemInImageListView(imageListView1.Items, filename);
                    if (foundItem != null) foundItem.Selected = true;
                }
            }
            
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;


            FilesSelected();
            
            
        }

        private void buttonRenameSave_Click(object sender, EventArgs e)
        {
            
            Dictionary<string, string> renameSuccess;
            Dictionary<string, string> renameFailed;
            DataGridViewHandlerRename.Write(dataGridViewRename, out renameSuccess, out renameFailed);

            UpdateImageViewListeAfterRename(renameSuccess, renameFailed, true);

            FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems);
        }

        #region Painting
        
    
        #endregion
    }
}
