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
                Logger.Error("Error when delete folder." + ex.Message);
                AddError(folder, "Was not able to delete folder with files and subfolder!\r\n\r\n" +
                    "From:\r\n" + folder +
                    "Error message:\r\n" + ex.Message);
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
            var droplist = new StringCollection();
            foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FullFileName);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Move);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }

        private void toolStripMenuItemImageListViewCopy_Click(object sender, EventArgs e)
        {
            StringCollection droplist = new StringCollection();
            foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FullFileName);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Copy);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }

        private void toolStripMenuItemImageListViewPaste_Click(object sender, EventArgs e)
        {
            StringCollection files = Clipboard.GetFileDropList();
            foreach (string fullFilename in files)
            {
                bool fileFound = false;
                foreach (ImageListViewItem item in imageListView1.Items)
                {
                    if (item.FullFileName == fullFilename)
                    {
                        fileFound = true;
                        break;
                    }
                }
                if (!fileFound) imageListView1.Items.Add(fullFilename);
            }

        }
        #endregion 

        #region Cut Copy Paste - FolderTreeView
        private void toolStripMenuItemTreeViewFolderCut_Click(object sender, EventArgs e)
        {
            string folder = folderTreeViewFolder.GetSelectedNodePath();
            var droplist = new StringCollection();
            droplist.Add(folder);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Move);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }

        private void toolStripMenuItemTreeViewFolderCopy_Click(object sender, EventArgs e)
        {            
            string folder = folderTreeViewFolder.GetSelectedNodePath();
            StringCollection droplist = new StringCollection();
            droplist.Add(folder);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Copy);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }

        private DragDropEffects DetectCopyOrMove()
        {
            try
            {
                IDataObject obj = Clipboard.GetDataObject();
                if (obj == null) return DragDropEffects.None;
                if (obj.GetData("Preferred DropEffect", true) is DragDropEffects effects)
                {
                    return effects;
                }

                obj.GetData(DataFormats.FileDrop);
                MemoryStream stream = (MemoryStream) obj.GetData("Preferred DropEffect", true);
                if (stream != null)
                {
                    int flag = stream.ReadByte();
                    if (flag != 2 && flag != 5) return DragDropEffects.None;

                    if (flag == 2) return DragDropEffects.Move;
                    if (flag == 5) return DragDropEffects.Copy;
                }
                
            } catch (Exception ex)
            {
                Logger.Error("Clipboard failed: " + ex.Message);
            }
            return DragDropEffects.None;
        }


        private void CopyFile(TreeNode targetNode, StringCollection files, string targetNodeDirectory)
        {                       
            foreach (string oldPath in files) //Move all files to target directory 
            {
                string sourceFullFilename = oldPath;
                string filename = Path.GetFileName(sourceFullFilename);
                string targetFullFilename = Path.Combine(targetNodeDirectory, filename);

                try
                {
                    filesCutCopyPasteDrag.CopyFile(sourceFullFilename, targetFullFilename);
                }
                catch (Exception ex)
                {
                    AddError(oldPath, "Error copy file from:" + sourceFullFilename + " to: " + targetFullFilename);
                    Logger.Error("Error when copy file." + ex.Message);
                }
            }

            GlobalData.DoNotRefreshImageListView = true;
            folderTreeViewFolder.SelectedNode = currentNode;
            GlobalData.DoNotRefreshImageListView = false;

            //FolderSelected();
            FilesSelected();
        }

        private void MoveFiles(TreeNode targetNode, StringCollection files, string targetNodeDirectory)
        {
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            imageListView1.SuspendLayout();

            foreach (string oldPath in files) //Move all files to target directory 
            {
                string sourceFullFilename = oldPath;
                string filename = Path.GetFileName(sourceFullFilename);
                string targetFullFilename = Path.Combine(targetNodeDirectory, filename);
                try
                {
                    filesCutCopyPasteDrag.MoveFile(sourceFullFilename, targetFullFilename);

                    ImageListViewItem foundItem = FindItemInImageListView(imageListView1.Items, sourceFullFilename);
                    if (foundItem != null) imageListView1.Items.Remove(foundItem);
                }
                catch (Exception ex)
                {
                    AddError(oldPath, "Error move file from:" + sourceFullFilename + " to: "+ targetFullFilename);
                    Logger.Error("Error when move file." + ex.Message);
                }
            }
            imageListView1.ResumeLayout();
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;

            GlobalData.DoNotRefreshImageListView = true;
            folderTreeViewFolder.SelectedNode = currentNode;
            GlobalData.DoNotRefreshImageListView = false;

            //FolderSelected();
            FilesSelected();

        }

        private void MoveFolder(TreeNode sourceNode, TreeNode targetNode, string sourceDirectory, string targetDirectory)
        {
            if (sourceDirectory == targetDirectory) return; //Can't move into itself. No need for error message

            try
            {

                string[] allSourceFullFilenames = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);

                //----- Move all folder and files -----
                Logger.Trace("Move folder from:" + sourceDirectory + " to: " + targetDirectory);
                System.IO.Directory.Move(sourceDirectory, targetDirectory);

                //------ Clear ImageListView -----
                FolderSelectedNone();

                //------ Update node tree -----
                GlobalData.DoNotRefreshImageListView = true;
                if (sourceNode != null) sourceNode.Remove();
                filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, targetNode);
                GlobalData.DoNotRefreshImageListView = false;

                //------ Update database -----
                foreach (string oldFullFilename in allSourceFullFilenames)
                {
                    string oldFilename = Path.GetFileName(oldFullFilename);
                    string newFullFilename = Path.Combine(targetDirectory, oldFilename);
                    Logger.Trace("Rename from:" + oldFullFilename + " to: " + newFullFilename);
                    databaseAndCacheMetadataExiftool.Move(Path.GetDirectoryName(oldFullFilename), Path.GetFileName(oldFullFilename), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                }

                //----- Updated ImageListView with files ------
                FolderSelected(false);

            }
            catch (Exception ex)
            {
                Logger.Error("Error when move folder." + ex.Message);
                AddError(sourceDirectory, "Was not able to move directory with files and folder!\r\n\r\n" +
                    "From:\r\n" + sourceDirectory +
                    "To:\r\n" + targetDirectory + "\r\n\r\n" +
                    "Error message:\r\n" + ex.Message);
            }
        }

        private void CopyFolder(TreeNode sourceNode, TreeNode targetNode, string sourceDirectory, string tagretDirectory)
        {

            string[] allSourceFullFilenames = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);

            //----- Create directories and sub-directories
            Directory.CreateDirectory(tagretDirectory);
            foreach (string dirPath in Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories))
            {                
                try
                {
                    Directory.CreateDirectory(dirPath.Replace(sourceDirectory, tagretDirectory));
                }
                catch (SystemException ex)
                {
                    Logger.Error("Error when create directory when copy all files from folder:" + ex.Message);
                    AddError(dirPath,
                        "Was not able to create directory " + dirPath + "\r\n" + ex.Message);
                }
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string sourceFullFilename in allSourceFullFilenames)
            {
                string sourceFilename = Path.GetFileName(sourceFullFilename);
                string targetFullFilename = Path.Combine(tagretDirectory, sourceFilename);
                try
                {
                    Logger.Trace("Copy from:" + sourceFullFilename + " to: " + targetFullFilename);
                    File.Copy(sourceFullFilename, sourceFullFilename.Replace(sourceDirectory, tagretDirectory), true);                    
                    
                    databaseAndCacheMetadataExiftool.Copy(
                        Path.GetDirectoryName(sourceFullFilename), Path.GetFileName(sourceFullFilename), 
                        Path.GetDirectoryName(targetFullFilename), Path.GetFileName(targetFullFilename));
                }
                catch (SystemException ex)
                {
                    Logger.Error("Error when copy file when copy all files from folder:" + ex.Message);
                    AddError(sourceFullFilename,
                        "Was not able to copy file from:" + sourceFullFilename +
                        " to: " + tagretDirectory + "\r\n" +
                        ex.Message);
                }
            }

            //------ Update node tree -----
            GlobalData.DoNotRefreshImageListView = true;
            //if (sourceNode != null) sourceNode.Remove();
            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, targetNode);
            GlobalData.DoNotRefreshImageListView = false;
        }

        private void CopyOrMove(DragDropEffects dragDropEffects, TreeNode targetNode, StringCollection fileDropList, string targetDirectory)
        {
            if (dragDropEffects == DragDropEffects.None)
            {
                MessageBox.Show("Was not able to detect if you select copy or cut object that was pasted or dropped");
                return;
            }

            StringCollection files = new StringCollection();
            StringCollection directories = new StringCollection();

            int numberOfFilesAndFolders = 0;
            string copyFromFolders = ""; 
            int countFoldersSelected = 0;

            foreach (string clipbordSourceFileOrDirectory in fileDropList)
            {
                if (File.Exists(clipbordSourceFileOrDirectory))
                {
                    files.Add(clipbordSourceFileOrDirectory);
                    numberOfFilesAndFolders++;
                }
                else if (Directory.Exists(clipbordSourceFileOrDirectory))
                {
                    directories.Add(clipbordSourceFileOrDirectory);
                    if (numberOfFilesAndFolders <= 51)
                    {
                        string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(clipbordSourceFileOrDirectory, "*", SearchOption.AllDirectories).Take(51).ToArray();
                        numberOfFilesAndFolders += fileAndFolderEntriesCount.Length;
                    }

                    countFoldersSelected++;
                    if (countFoldersSelected < 3)
                    {
                        copyFromFolders += clipbordSourceFileOrDirectory + "\r\n";
                        
                    }
                    else if (countFoldersSelected == 4) copyFromFolders += "and more directories...\r\n";
                }
            }

            if (numberOfFilesAndFolders <= 50 || 
                (MessageBox.Show("You are about to " + dragDropEffects.ToString() + " " + (numberOfFilesAndFolders > 50 ? "over 50+" : numberOfFilesAndFolders.ToString()) + " files and/or folders.\r\n\r\n" +
                "From:\r\n" + copyFromFolders + "\r\n\r\n" +
                "To folder:\r\n" + targetDirectory + "\r\n\r\n" +
                "Procced?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK))
            {

                if (dragDropEffects == DragDropEffects.Move)
                    MoveFiles(targetNode, files, targetDirectory);
                else
                    CopyFile(targetNode, files, targetDirectory);

                foreach (string sourceDirectory in directories)
                {
                    string newTagretDirectory = Path.Combine(targetDirectory, new DirectoryInfo(sourceDirectory).Name); //Target directory + dragged (drag&drop) direcotry
                                                                                                                        //TreeNode targetNode = folderTreeViewFolder.SelectedNode;
                    TreeNode sourceNode = folderTreeViewFolder.FindFolder(sourceDirectory);

                    if (dragDropEffects == DragDropEffects.Move)
                        MoveFolder(sourceNode, targetNode, sourceDirectory, newTagretDirectory);                    
                    else
                        CopyFolder(sourceNode, targetNode, sourceDirectory, newTagretDirectory);
                }
            }
            folderTreeViewFolder.SelectedNode = targetNode;
            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, folderTreeViewFolder.SelectedNode);
            folderTreeViewFolder.Focus();
        }

        private void toolStripMenuItemTreeViewFolderPaste_Click(object sender, EventArgs e)
        {
            DragDropEffects dragDropEffects = DetectCopyOrMove();
            CopyOrMove(dragDropEffects, folderTreeViewFolder.SelectedNode, Clipboard.GetFileDropList(), folderTreeViewFolder.GetSelectedNodePath());            
        }
        #endregion


        #region Drop - FolderTreeView
        private void folderTreeViewFolder_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = folderTreeViewFolder.PointToClient(new Point(e.X, e.Y)); // Retrieve the client coordinates of the drop location.                          
            TreeNode targetNode = folderTreeViewFolder.GetNodeAt(targetPoint); // Retrieve the node at the drop location.
            string targetDirectory = Furty.Windows.Forms.ShellOperations.GetFileDirectory(targetNode);
            
            #region Move media files dropped to new folder from external source
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); //Check if files been dropped
            if (files != null)
            {
                StringCollection fileCollection = new StringCollection();
                fileCollection.AddRange(files);
                CopyOrMove(e.Effect, targetNode, fileCollection, targetDirectory);
                
                GlobalData.IsDragAndDropActive = false;                
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
                string nodeSourceDirectory = Furty.Windows.Forms.ShellOperations.GetFileDirectory(draggedNode);
                string newTagretDirectory = Path.Combine(targetDirectory, new DirectoryInfo(nodeSourceDirectory).Name); //Target directory + dragged (drag&drop) direcotry

                // If it is a move operation, remove the node from its current location and add it to the node at the drop location.  
                if (e.Effect == DragDropEffects.Move)
                    MoveFolder(draggedNode, targetNode, nodeSourceDirectory, newTagretDirectory);
                else if(e.Effect == DragDropEffects.Copy)
                    CopyFolder(draggedNode, targetNode, nodeSourceDirectory, newTagretDirectory);                                
            }

            GlobalData.IsDragAndDropActive = false;
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Drag - FolderTreeView
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
            try
            {
                GlobalData.IsDragAndDropActive = true;

                // Move the dragged node when the left mouse button is used.  
                if (e.Button == MouseButtons.Left)
                    DoDragDrop(e.Item, DragDropEffects.Move);
                // Copy the dragged node when the right mouse button is used.
                else if (e.Button == MouseButtons.Right)
                    DoDragDrop(e.Item, DragDropEffects.Copy);
            } catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
        }

        TreeNode currentNode = null;
        private void folderTreeViewFolder_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                GlobalData.IsDragAndDropActive = true;
                //e.Effect = e.AllowedEffect;
                //e.KeyStates == DragDropKeyStates.RightMouseButton
                if (((System.Windows.DragDropKeyStates)e.KeyState & System.Windows.DragDropKeyStates.ShiftKey) == System.Windows.DragDropKeyStates.ShiftKey)
                    e.Effect = DragDropEffects.Move;
                else if (((System.Windows.DragDropKeyStates)e.KeyState & System.Windows.DragDropKeyStates.RightMouseButton) == System.Windows.DragDropKeyStates.RightMouseButton)
                    e.Effect = DragDropEffects.Copy;
                else if (((System.Windows.DragDropKeyStates)e.KeyState & System.Windows.DragDropKeyStates.ControlKey) == System.Windows.DragDropKeyStates.ControlKey)
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.Move;

                currentNode = folderTreeViewFolder.SelectedNode;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
        }

        private void folderTreeViewFolder_DragLeave(object sender, EventArgs e)
        {
            GlobalData.IsDragAndDropActive = false;
        }

        private void folderTreeViewFolder_DragOver(object sender, DragEventArgs e)
        {
            try
            {

                if (((System.Windows.DragDropKeyStates)e.KeyState & System.Windows.DragDropKeyStates.ShiftKey) == System.Windows.DragDropKeyStates.ShiftKey)
                    e.Effect = DragDropEffects.Move;
                else if (((System.Windows.DragDropKeyStates)e.KeyState & System.Windows.DragDropKeyStates.RightMouseButton) == System.Windows.DragDropKeyStates.RightMouseButton)
                    e.Effect = DragDropEffects.Copy;
                else if (((System.Windows.DragDropKeyStates)e.KeyState & System.Windows.DragDropKeyStates.ControlKey) == System.Windows.DragDropKeyStates.ControlKey)
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.Move;

                GlobalData.DoNotRefreshImageListView = true;
                // Retrieve the client coordinates of the mouse position.  
                Point targetPoint = folderTreeViewFolder.PointToClient(new Point(e.X, e.Y));

                // Select the node at the mouse position.  
                folderTreeViewFolder.SelectedNode = folderTreeViewFolder.GetNodeAt(targetPoint);
                GlobalData.DoNotRefreshImageListView = false;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
        }        

        private void folderTreeViewFolder_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                folderTreeViewFolder.SelectedNode = e.Node;
            }
        }
        #endregion

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
