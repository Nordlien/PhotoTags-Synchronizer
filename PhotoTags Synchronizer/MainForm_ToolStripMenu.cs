using DataGridViewGeneric;
using Exiftool;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        #region About
        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            FormAbout form = new FormAbout();
            form.ShowDialog();
            form.Dispose();
        }
        #endregion

        #region Import Google Locations
        private void toolStripButtonImportGoogleLocation_Click(object sender, EventArgs e)
        {
            LocationHistoryImportForm form = new LocationHistoryImportForm();
            form.databaseTools = databaseUtilitiesSqliteMetadata;
            if (form.ShowDialog() == DialogResult.OK)
            {
                databaseAndCahceCameraOwner.CameraMakeModelAndOwnerMakeDirty();
//Update DataGridViews
            }
        }
        #endregion

        #region Save Metadata
        private void SaveDataGridViewMetadata()
        {
            if (GlobalData.IsPopulatingAnything())
            {
                MessageBox.Show("Data is populating, please try a bit later.");
                return;
            }
            if (!GlobalData.IsAgredagedGridViewAny())
            {
                MessageBox.Show("No metadata are updated.");
                return;
            }

            
            List<Metadata> metadataListOriginalExiftool = new List<Metadata>();
            List<Metadata> metadataListFromDataGridView = new List<Metadata>();

            DataGridView dataGridView = GetActiveDataGridView();
            List<DataGridViewGenericColumn> dataGridViewGenericColumnList = DataGridViewHandler.GetColumnsMetadata(dataGridView, true);
            foreach (DataGridViewGenericColumn dataGridViewGenericColumn in dataGridViewGenericColumnList)
            {
                if (dataGridViewGenericColumn.Metadata == null) continue;
                
                Metadata metadataFromDataGridView = new Metadata(dataGridViewGenericColumn.Metadata);

                if (GlobalData.IsAgregatedTags)
                    DataGridViewHandlerTagsAndKeywords.GetUserInputChanges(ref dataGridViewTagsAndKeywords, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryImage);

                if (GlobalData.IsAgregatedMap)
                    DataGridViewHandlerMap.GetUserInputChanges(ref dataGridViewMap, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryImage);

                if (GlobalData.IsAgregatedPeople)
                    DataGridViewHandlerPeople.GetUserInputChanges(ref dataGridViewPeople, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryImage);

                metadataListFromDataGridView.Add(metadataFromDataGridView);
                metadataListOriginalExiftool.Add(dataGridViewGenericColumn.Metadata);   
            }

            //Find what columns are updated / changed by user
            List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
            if (listOfUpdates.Count == 0)
            {
                MessageBox.Show("Can't find any value that was changed. Nothing is saved...");
                return;
            }

            foreach (int updatedRecord in listOfUpdates)
            {
                //Add only metadata to save queue that that has changed by users
                AddQueueSaveMetadataUpdatedByUser(metadataListFromDataGridView[updatedRecord], metadataListOriginalExiftool[updatedRecord]);
            }
            ThreadSaveMetadata();

        }

        private void toolStripButtonSaveAllMetadata_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode

            if (GlobalData.IsPopulatingAnything()) return;
            if (GlobalData.IsSaveButtonPushed) return;
            GlobalData.IsSaveButtonPushed = true;
            this.Enabled = false;

            switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
            {
                case "ExifTool":
                case "Warning":
                    //Nothing
                    break;
                case "Tags":
                case "Map":
                case "People":
                case "Date":
                    SaveDataGridViewMetadata();
                    GlobalData.IsAgregatedProperties = false;
                    break;                
                case "Properties":
                    DataGridView dataGridView = dataGridViewProperties;
                    int columnCount = DataGridViewHandler.GetColumnCount(dataGridView);
                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                    {
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                        if (dataGridViewGenericColumn != null)
                        {
                            try
                            {
                                DataGridViewHandlerProperties.Write(dataGridView, columnIndex);
                            }
                            catch (Exception ex)
                            {
                                string writeErrorDesciption = "Error writing properties for file: " + dataGridViewGenericColumn.FileEntryImage.FullFilePath + " Message: " + ex.Message + "\r\n";
                                AddError(dataGridViewGenericColumn.FileEntryImage.FullFilePath, writeErrorDesciption);
                                Logger.Error(ex.Message);
                            }
                        }
                    }

                    GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
                    UpdateThumbnailOnImageListViewItems(imageListView1, null);
                    UpdateMetadataOnSelectedFilesOnActiveDataGrivView(imageListView1.SelectedItems);
                    //GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
                    FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems);

                    break;
                case "Rename":
                    MessageBox.Show("Not implemented");
                    break;
            }

            this.Enabled = true;
            GlobalData.IsSaveButtonPushed = false;
        }
        #endregion

        #region Refresh - Folder tree
        private void toolStripMenuItemTreeViewFolderRefreshFolder_Click(object sender, EventArgs e)
        {
            GlobalData.DoNotRefreshImageListView = true;
            TreeNode selectedNode = folderTreeViewFolder.SelectedNode;
            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, selectedNode);
            GlobalData.DoNotRefreshImageListView = false;
            FolderSelected(false);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Refresh - Items in listview 
        private void toolStripMenuItemTreeViewFolderReadSubfolders_Click(object sender, EventArgs e)
        {
            FolderSelected(true);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Reload Metadata - Selected items
        private void toolStripMenuItemReloadThumbnailAndMetadata_Click(object sender, EventArgs e)
        {
            filesCutCopyPasteDrag.DeleteFilesMetadataForReload(folderTreeViewFolder, imageListView1, imageListView1.Items, true);
            imageListView1.Focus();
        }
        #endregion 

        #region Reload Metadata - Folder
        private void toolStripMenuItemTreeViewFolderReload_Click(object sender, EventArgs e)
        {
            filesCutCopyPasteDrag.DeleteFilesMetadataForReload(folderTreeViewFolder, imageListView1, imageListView1.Items, false);

            FilesSelected();
            UpdateStatusReadWriteStatus_NeedToBeUpated();
            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region Delete Metadata Hirstory - Selected Items
        private void toolStripMenuItemReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory_Click(object sender, EventArgs e)
        {
            filesCutCopyPasteDrag.ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(folderTreeViewFolder, imageListView1);

            FilesSelected();
            UpdateStatusReadWriteStatus_NeedToBeUpated();
        }
        #endregion

        #region Delete Metadata Hirstory - Directory

        private void toolStripMenuItemTreeViewFolderClearCache_Click(object sender, EventArgs e)
        {
            string folder = this.folderTreeViewFolder.GetSelectedNodePath();
            string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly).Take(51).ToArray();
            if (MessageBox.Show("Are you sure you will delete **ALL** metadata history in database store for " +
                (fileAndFolderEntriesCount.Length == 51 ? " over 50 + " : fileAndFolderEntriesCount.Length.ToString()) +
                "  number of files.\r\n\r\n" +
                "In the folder: " + folder,
                "You are going to delete all metadata in folder",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                filesCutCopyPasteDrag.DeleteDirectory(folder);
            }

            imageListView1.ClearThumbnailCache();
            imageListView1.Refresh();
            Application.DoEvents();
            FolderSelected_AggregateListViewWithFilesFromFolder(this.folderTreeViewFolder.GetSelectedNodePath(), false);
            UpdateStatusReadWriteStatus_NeedToBeUpated();
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Delete Files - Items
        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            folderTreeViewFolder.Enabled = false;
            imageListView1.Enabled = false;

            if (MessageBox.Show("Are you sure you will delete the files", "Files will be deleted!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                filesCutCopyPasteDrag.DeleteSelectedFiles(imageListView1);
                FilesSelected();
            }

            folderTreeViewFolder.Enabled = true;
            imageListView1.Enabled = true;
            
            UpdateStatusReadWriteStatus_NeedToBeUpated();

        }
        #endregion

        #region Delete Files - Directory
        private void toolStripMenuItemTreeViewFolderDelete_Click(object sender, EventArgs e)
        {
            string folder = folderTreeViewFolder.GetSelectedNodePath();
            try
            {

                string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories).Take(51).ToArray();
                if (MessageBox.Show("You are about to delete the folder:\r\n\r\n" +
                    folder + "\r\n\r\n" +
                    "There are " + (fileAndFolderEntriesCount.Length == 51 ? " over 50+" : fileAndFolderEntriesCount.Length.ToString()) + " files found.\r\n\r\n" +
                    "Procced?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    filesCutCopyPasteDrag.DeleteFilesInFolder(folderTreeViewFolder, folder);
                    FolderSelected(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error when move folder." + ex.Message);
                MessageBox.Show("Was not able to delete folder with files and subfolder!\r\n\r\n" +
                    "From:\r\n" + folder +
                    "Error message:\r\n" + ex.Message, "Error, was not able to move folder!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GlobalData.DoNotRefreshImageListView = false;
            }
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Cut Copy Paste - ImageListView
        private void toolStripMenuItemImageListViewCut_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItemImageListViewCopy_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItemImageListViewPaste_Click(object sender, EventArgs e)
        {

        }
        #endregion 

        #region Cut Copy Paste - FolderTreeView
        private void toolStripMenuItemTreeViewFolderCut_Click(object sender, EventArgs e)
        {
            string folder = folderTreeViewFolder.GetSelectedNodePath();
            var droplist = new StringCollection();
            droplist.Add(folder);

            byte[] moveEffect = new byte[] { 2, 0, 0, 0 };
            MemoryStream dropEffect = new MemoryStream();
            dropEffect.Write(moveEffect, 0, moveEffect.Length);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", dropEffect);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }

        private void toolStripMenuItemTreeViewFolderCopy_Click(object sender, EventArgs e)
        {
            //string[] dirs = Directory.GetDirectories(folder + (folder.EndsWith(@"\") ? folder : folder + @"\"), "*", SearchOption.AllDirectories);


            string folder = folderTreeViewFolder.GetSelectedNodePath();
            StringCollection paths = new StringCollection();
            paths.Add(folder);
            Clipboard.SetFileDropList(paths);

            folderTreeViewFolder.Focus();
        }

        private void toolStripMenuItemTreeViewFolderPaste_Click(object sender, EventArgs e)
        {
            string selectedDirectory = folderTreeViewFolder.GetSelectedNodePath();


            foreach (string clipbordFileOrDirectory in Clipboard.GetFileDropList())
            {
                if (File.Exists(clipbordFileOrDirectory))
                {
                    try
                    {
                        File.Copy(clipbordFileOrDirectory, Path.Combine(selectedDirectory, Path.GetFileName(clipbordFileOrDirectory)));
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (Directory.Exists(clipbordFileOrDirectory))
                {
                    try
                    {
                        string newTagretDirectory = Path.Combine(selectedDirectory, new DirectoryInfo(clipbordFileOrDirectory).Name); //Target directory + dragged (drag&drop) direcotry
                        Directory.CreateDirectory(newTagretDirectory);

                        //Now Create all of the directories
                        foreach (string dirPath in Directory.GetDirectories(clipbordFileOrDirectory, "*", SearchOption.AllDirectories))
                        {
                            //string newTagretDirectory = Path.Combine(selectedDirectory, new DirectoryInfo(dirPath).Name); //Target directory + dragged (drag&drop) direcotry

                            Console.WriteLine(dirPath.Replace(clipbordFileOrDirectory, newTagretDirectory));
                            Directory.CreateDirectory(dirPath.Replace(clipbordFileOrDirectory, newTagretDirectory));
                        }


                        //Copy all the files & Replaces any files with the same name
                        foreach (string file in Directory.GetFiles(clipbordFileOrDirectory, "*.*", SearchOption.AllDirectories))
                        {
                            File.Copy(file, file.Replace(clipbordFileOrDirectory, newTagretDirectory), true);
                        }
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, folderTreeViewFolder.SelectedNode);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Drag and Drop - FolderTreeView
        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            // Check the parent node of the second node.  
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            // If the parent node is not null or equal to the first node,   
            // call the ContainsNode method recursively using the parent of   
            // the second node.  
            return ContainsNode(node1, node2.Parent);
        }

        private void folderTreeViewFolder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            GlobalData.IsDragAndDropActive = true;

            // Move the dragged node when the left mouse button is used.  
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }

            // Copy the dragged node when the right mouse button is used.  
            else if (e.Button == MouseButtons.Right)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void folderTreeViewFolder_DragDrop(object sender, DragEventArgs e)
        {

            Point targetPoint = folderTreeViewFolder.PointToClient(new Point(e.X, e.Y)); // Retrieve the client coordinates of the drop location.                          
            TreeNode targetNode = folderTreeViewFolder.GetNodeAt(targetPoint); // Retrieve the node at the drop location.

            #region Move media files dropped to new folder
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); //Check if files been dropped
            if (files != null)
            {
                try
                {
                    string targetNodeDirectory = Furty.Windows.Forms.ShellOperations.GetFileDirectory(targetNode);
                    Dictionary<string, string> renameSuccess = new Dictionary<string, string>();
                    Dictionary<string, string> renameFailed = new Dictionary<string, string>();

                    foreach (string oldFullFilename in files) //Move all files to target directory 
                    {
                        string oldFilename = Path.GetFileName(oldFullFilename);
                        string newFullFilename = Path.Combine(targetNodeDirectory, oldFilename);
                        DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerRename.RenameFile(oldFullFilename, newFullFilename, ref renameSuccess, ref renameFailed);
                    }

                    GlobalData.DoNotRefreshImageListView = true;
                    folderTreeViewFolder.SelectedNode = currentNode;
                    GlobalData.DoNotRefreshImageListView = false;
                    //GlobalData.DoNotRefreshImageListView = true;
                    UpdateImageViewListeAfterRename(renameSuccess, renameFailed, false);
                    //GlobalData.DoNotRefreshImageListView = false;

                    //FolderSelected();
                    FilesSelected();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error when move folder." + ex.Message);
                }
                finally
                {
                    GlobalData.IsDragAndDropActive = false;
                }
                return;
            }
            #endregion

            #region Get information about dragged, if unknown then return

            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode)); // Retrieve the node that was dragged.  
            if (draggedNode == null)
            {
                GlobalData.IsDragAndDropActive = false;
                return;
            }
            #endregion 

            // Confirm that the node at the drop location is not the dragged node or a descendant of the dragged node.  
            if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
            {
                // If it is a move operation, remove the node from its current location and add it to the node at the drop location.  
                if (e.Effect == DragDropEffects.Move)
                {
                    string targetNodeDirectory = Furty.Windows.Forms.ShellOperations.GetFileDirectory(targetNode);
                    string nodeSourceDirectory = Furty.Windows.Forms.ShellOperations.GetFileDirectory(draggedNode);
                    string newTagretDirectory = Path.Combine(targetNodeDirectory, new DirectoryInfo(nodeSourceDirectory).Name); //Target directory + dragged (drag&drop) direcotry

                    try
                    {

                        if (nodeSourceDirectory == newTagretDirectory) return; //Can't move into itself. No need for error message

                        string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(nodeSourceDirectory, "*", SearchOption.AllDirectories).Take(51).ToArray();
                        if (MessageBox.Show("You are about to move " + (fileAndFolderEntriesCount.Length == 51 ? " over 50+" : fileAndFolderEntriesCount.Length.ToString()) + " files found.\r\n\r\n" +
                            "From folder:\r\n" + nodeSourceDirectory + "\r\n\r\n" +
                            "To folder:\r\n" + newTagretDirectory + "\r\n\r\n\r\n" +
                            "Procced?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            string[] allfiles = Directory.GetFiles(nodeSourceDirectory, "*.*", SearchOption.AllDirectories);

                            Logger.Trace("Move folder from:" + nodeSourceDirectory + " to: " + newTagretDirectory);
                            System.IO.Directory.Move(nodeSourceDirectory, newTagretDirectory);

                            FolderSelectedNone();

                            #region Update Node in TreeView
                            GlobalData.DoNotRefreshImageListView = true;
                            draggedNode.Remove();

                            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, targetNode);

                            GlobalData.DoNotRefreshImageListView = false;
                            #endregion

                            foreach (string oldFullFilename in allfiles)
                            {
                                string oldFilename = Path.GetFileName(oldFullFilename);
                                string newFullFilename = Path.Combine(newTagretDirectory, oldFilename);
                                Logger.Trace("Rename from:" + oldFullFilename + " to: " + newFullFilename);
                                databaseAndCacheMetadataExiftool.Rename(Path.GetDirectoryName(oldFullFilename), Path.GetFileName(oldFullFilename), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                            }

                            FolderSelected(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error when move folder." + ex.Message);
                        MessageBox.Show("Was not able to move directory with files and folder!\r\n\r\n" +
                            "From:\r\n" + nodeSourceDirectory +
                            "To:\r\n" + targetNodeDirectory + "\r\n\r\n" +
                            "Error message:\r\n" + ex.Message, "Error, was not able to move folder!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        GlobalData.IsDragAndDropActive = false;
                    }
                }

                // If it is a copy operation, clone the dragged node   
                // and add it to the node at the drop location.  
                else if (e.Effect == DragDropEffects.Copy)
                {
                    //targetNode.Nodes.Add((TreeNode)draggedNode.Clone());
                }

                // Expand the node at the location   
                // to show the dropped node.  

            }
            GlobalData.IsDragAndDropActive = false;
            folderTreeViewFolder.Focus();
        }

        TreeNode currentNode = null;
        private void folderTreeViewFolder_DragEnter(object sender, DragEventArgs e)
        {
            GlobalData.IsDragAndDropActive = true;
            e.Effect = DragDropEffects.Move; //e.AllowedEffect;
            currentNode = folderTreeViewFolder.SelectedNode;
        }

        private void folderTreeViewFolder_DragLeave(object sender, EventArgs e)
        {

        }

        private void folderTreeViewFolder_DragOver(object sender, DragEventArgs e)
        {
            GlobalData.DoNotRefreshImageListView = true;
            // Retrieve the client coordinates of the mouse position.  
            Point targetPoint = folderTreeViewFolder.PointToClient(new Point(e.X, e.Y));

            // Select the node at the mouse position.  
            folderTreeViewFolder.SelectedNode = folderTreeViewFolder.GetNodeAt(targetPoint);
            GlobalData.DoNotRefreshImageListView = false;
        }
        #endregion

        private void folderTreeViewFolder_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                folderTreeViewFolder.SelectedNode = e.Node;
            }
        }

        #region Refreh Folder
        private void toolStripMenuItemRefreshFolder_Click(object sender, EventArgs e)
        {
            FolderSelected(false);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Select all - Items
        private void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            imageListView1.SelectAll();
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            FilesSelected();
        }
        #endregion

        
    }
}
