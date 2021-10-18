using ApplicationAssociations;
using DataGridViewGeneric;
using Exiftool;
using ImageAndMovieFileExtentions;
using LocationNames;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Thumbnails;
using Krypton.Toolkit;
using Raccoom.Windows.Forms;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        #region FoldeTree

        #region FolderTree - Folder - Click
        private void treeViewFolderBrowser1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (GlobalData.IsPopulatingFolderTree) return;
                if (GlobalData.IsDragAndDropActive) return;

                if (GlobalData.DoNotRefreshImageListView) return;
                PopulateImageListView_FromFolderSelected(false, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #endregion

        #region Drag and Drop
        TreeNode currentNodeWhenStartDragging = null; //Updated by DragEnter
        private bool isInternalDrop = true;

        #region FolderTree - Drag and Drop - Detect Copy Or Move
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
                MemoryStream stream = (MemoryStream)obj.GetData("Preferred DropEffect", true);
                if (stream != null)
                {
                    int flag = stream.ReadByte();
                    if (flag != 2 && flag != 5) return DragDropEffects.None;

                    if (flag == 2) return DragDropEffects.Move;
                    if (flag == 5) return DragDropEffects.Copy;
                }

            } catch (Exception ex)
            {
                Logger.Error(ex, "Clipboard failed: ");
            }
            return DragDropEffects.None;
        }
        #endregion

        #region FolderTree - Drag and Drop - Files or Folders - ** Check if File is in ThreadQueue before accept move **
        private TreeNode TreeViewFolderBrowserFindNode(TreeNodeCollection treeNodeCollection, string directory)
        {
            TreeNode treeNodeFound = null;
            foreach (TreeNode treeNodeSearch in treeNodeCollection)
            {
                TreeNodePath treeNodePath = (TreeNodePath)treeNodeSearch;
                if (treeNodePath.Path == directory)
                {
                    treeNodeFound = treeNodeSearch;
                    break;
                }
                if (treeNodeSearch.Nodes != null) treeNodeFound = TreeViewFolderBrowserFindNode(treeNodeSearch.Nodes, directory);
            }
            return treeNodeFound;
        }

        private void CopyOrMove(DragDropEffects dragDropEffects, TreeNode targetNode, StringCollection fileDropList, string targetDirectory)
        {
            try
            {
                if (dragDropEffects == DragDropEffects.None)
                {
                    
                    KryptonMessageBox.Show("Was not able to detect if you select copy or cut object that was pasted or dropped");
                    return;
                }

                if (!Directory.Exists(targetDirectory))
                {
                    KryptonMessageBox.Show("Target folder is not a valid target folder.\r\nSelected system folder:" + targetNode?.FullPath == null ? "Unkown" : targetNode?.FullPath);
                    return;
                }

                if (IsFileInThreadQueueLock(fileDropList))
                {
                    KryptonMessageBox.Show("Can't " + dragDropEffects.ToString() + " files. Files are being used, you need wait until process is finished.");
                    return;
                }

                StringCollection files = new StringCollection();
                StringCollection directories = new StringCollection();
                StringCollection filesSameTarget = new StringCollection();
                StringCollection directoriesSameTaget = new StringCollection();

                int numberOfFilesAndFolders = 0;
                string copyFromFolders = "";
                int countFoldersSelected = 0;

                foreach (string clipbordSourceFileOrDirectory in fileDropList)
                {
                    if (File.Exists(clipbordSourceFileOrDirectory))
                    {
                        if (Path.GetDirectoryName(clipbordSourceFileOrDirectory) != targetDirectory)
                        {
                            files.Add(clipbordSourceFileOrDirectory);
                            numberOfFilesAndFolders++;
                        }
                        else filesSameTarget.Add(clipbordSourceFileOrDirectory);
                    }
                    else if (Directory.Exists(clipbordSourceFileOrDirectory))
                    {
                        if (clipbordSourceFileOrDirectory != targetDirectory)
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
                        else directoriesSameTaget.Add(clipbordSourceFileOrDirectory);
                    }
                }


                if (numberOfFilesAndFolders >= 1)
                {
                    if (numberOfFilesAndFolders <= 50 ||
                        (MessageBox.Show("You are about to " + dragDropEffects.ToString() + " " + (numberOfFilesAndFolders > 50 ? "over 50+" : numberOfFilesAndFolders.ToString()) + " files and/or folders.\r\n\r\n" +
                        "From:\r\n" + copyFromFolders + "\r\n\r\n" +
                        "To folder:\r\n" + targetDirectory + "\r\n\r\n" +
                        "Procced?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK))
                    {

                        if (dragDropEffects == DragDropEffects.Move)
                            MoveFiles(treeViewFolderBrowser1, imageListView1, files, targetDirectory);
                        else
                            CopyFiles(treeViewFolderBrowser1, files, targetDirectory);

                        foreach (string sourceDirectory in directories)
                        {
                            string newTagretDirectory = Path.Combine(targetDirectory, new DirectoryInfo(sourceDirectory).Name); //Target directory + dragged (drag&drop) direcotry
                                                                                                                                //TreeNode targetNode = folderTreeViewFolder.SelectedNode;
                            TreeNode sourceNode = TreeViewFolderBrowserFindNode(treeViewFolderBrowser1.Nodes, sourceDirectory);

                            if (dragDropEffects == DragDropEffects.Move)
                                MoveFolder(treeViewFolderBrowser1, sourceNode, targetNode, sourceDirectory, newTagretDirectory);
                            else
                                CopyFolder(treeViewFolderBrowser1, targetNode, sourceDirectory, newTagretDirectory);
                        }
                    }
                } else
                {
                    string fileMessage = "";
                    if (filesSameTarget.Count == 1) fileMessage = "Source file: "+ filesSameTarget[0] + "\r\n";
                    else if (filesSameTarget.Count >= 1) fileMessage = "File count: " + filesSameTarget.Count + "\r\n";

                    string folderMessage = "";
                    if (directoriesSameTaget.Count == 1) folderMessage = "";
                    else if (directoriesSameTaget.Count >= 1) folderMessage = "Folder count: " + directoriesSameTaget.Count + "\r\n";

                    KryptonMessageBox.Show("Can't " + dragDropEffects.ToString() + " files. \r\n" +
                        "Source and destiation are the same.\r\n\r\n" +
                        "Target folder: " + targetDirectory + "\r\n" +
                        fileMessage +
                        folderMessage);
                }
                treeViewFolderBrowser1.SelectedNode = currentNodeWhenStartDragging;
                filesCutCopyPasteDrag.RefeshFolderTree(treeViewFolderBrowser1, currentNodeWhenStartDragging);
                treeViewFolderBrowser1.Focus();

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        #region FolderTree - Rename Folder
        private void treeViewFolderBrowser1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null || e.Node.Parent == null) e.CancelEdit = true;
            if (e.CancelEdit == false)
            {
                string sourceDirectory = GetSelectedNodePath();
                DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectory);
                if (directoryInfo.Parent == null || sourceDirectory == null || !Directory.Exists(sourceDirectory))
                {
                        e.CancelEdit = true;
                        KryptonMessageBox.Show("Can't edit folder name. No valid folder selected.");
                    return;
                }
                           
            }
        }

        private void treeViewFolderBrowser1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            using (new WaitCursor())
            {
                treeViewFolderBrowser1.SuspendLayout();
                if (e.Label != null && e.Label != e.Node.Text)
                {
                    string sourceDirectory = GetSelectedNodePath();
                    string newTagretDirectory = Path.Combine((new DirectoryInfo(sourceDirectory).Parent).FullName, e.Label);
                    MoveFolder(treeViewFolderBrowser1, e.Node, e.Node.Parent, sourceDirectory, newTagretDirectory);
                }
                treeViewFolderBrowser1.ResumeLayout();
            }
        }
        #endregion

        #region FolderTree - Drag and Drop - Drop - Move/Copy Files - Move/Copy Folders
        private void treeViewFolderBrowser1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Point targetPoint = treeViewFolderBrowser1.PointToClient(new Point(e.X, e.Y)); // Retrieve the client coordinates of the drop location.                          
                TreeNode targetNode = treeViewFolderBrowser1.GetNodeAt(targetPoint); // Retrieve the node at the drop location.
                string targetDirectory = GetNodeFolderPath(targetNode as TreeNodePath);

                #region Move media files dropped to new folder from external source
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); //Check if files been dropped
                if (files != null)
                {
                    StringCollection fileCollection = new StringCollection();
                    fileCollection.AddRange(files);
                    CopyOrMove(e.Effect, targetNode, fileCollection, targetDirectory);
                }
                #endregion

                GlobalData.IsDragAndDropActive = false;
                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region FolderTree - Drag and Drop - Node Mouse Click - Set clickedNode
        private void treeViewFolderBrowser1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {        
            try
            {
                //clickedNode = e.Node;
                currentNodeWhenStartDragging = e.Node;

                if (e.Button == MouseButtons.Right)
                {
                    treeViewFolderBrowser1.SelectedNode = currentNodeWhenStartDragging;
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (((TreeViewFolderBrowser)sender).SelectedNode == e.Node) PopulateImageListView_FromFolderSelected(false, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderTree - Drag and Drop - Item Drag - Set Clipboard data to ** TreeViewFolder.Item ** | Move | Copy | Link |
        private DataObject SetDropDropFileList(string sourceDirectory, DragDropEffects dragDropEffects)
        {            
            var droplist = new StringCollection();
            droplist.Add(sourceDirectory);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Move);
            Clipboard.SetDataObject(data, true);
            //DragDropEffects dragDropEffects = treeViewFolderBrowser1.DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link); // Allowed effects
            return data;
        }
        private void treeViewFolderBrowser1_ItemDrag(object sender, ItemDragEventArgs e)
        {
         
            try
            {
                Clipboard.Clear();

                currentNodeWhenStartDragging = (TreeNode)e.Item;
                string sourceDirectory = GetNodeFolderPath(currentNodeWhenStartDragging as TreeNodePath);
                
                if (currentNodeWhenStartDragging != null && Directory.Exists(sourceDirectory))
                {
                    DataObject data = SetDropDropFileList(sourceDirectory, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link);
                    DragDropEffects dragDropEffects = treeViewFolderBrowser1.DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link); // Allowed effects
                    
                    if (!isInternalDrop)
                    {
                        if (dragDropEffects == DragDropEffects.Move) //Moved a folder to new location in eg. Windows Explorer
                        {
                            imageListView1.ClearSelection();

                            TreeNode sourceNode = currentNodeWhenStartDragging;
                            TreeNode parentNode = currentNodeWhenStartDragging.Parent;
                            if (parentNode == null) treeViewFolderBrowser1.SelectedNode = treeViewFolderBrowser1.Nodes[0];

                            //------ Update node tree -----
                            GlobalData.DoNotRefreshImageListView = true;
                            if (sourceNode != null) sourceNode.Remove();
                            filesCutCopyPasteDrag.RefeshFolderTree(treeViewFolderBrowser1, parentNode);
                            GlobalData.DoNotRefreshImageListView = false;

                            //----- Updated ImageListView with files ------
                            PopulateImageListView_FromFolderSelected(false, true);
                        }
                        else //Copied or NOT (cancel) a folder to new location in eg. Windows Explorer
                        {
                            GlobalData.DoNotRefreshImageListView = true;
                            treeViewFolderBrowser1.SelectedNode = currentNodeWhenStartDragging;
                            GlobalData.DoNotRefreshImageListView = false;
                        }
                    }
                }
                else
                {
                    SetDropDropFileList("", DragDropEffects.None); //Removes error message for wrong Data in Clipboard
                    treeViewFolderBrowser1.Focus();
                }
            }
            catch (Exception ex)
            {
                SetDropDropFileList("", DragDropEffects.None); //Removes error message for wrong Data in Clipboard

                Logger.Error(ex, "folderTreeViewFolder_ItemDrag, Failed create drag and drop transfer data.");
                KryptonMessageBox.Show("Failed create drag and drop transfer data. Error: " + ex.Message);
                treeViewFolderBrowser1.Focus();
            }
        }
        #endregion 

        #region FolderTree - Drag and Drop - Drag Leave - Set Clipboard data to ** FileDropList ** | Link |
        private void treeViewFolderBrowser1_DragLeave(object sender, EventArgs e)
        {
            try
            {
                isInternalDrop = false;

                GlobalData.IsDragAndDropActive = false;

                GlobalData.DoNotRefreshImageListView = true;
                treeViewFolderBrowser1.SelectedNode = currentNodeWhenStartDragging;
                GlobalData.DoNotRefreshImageListView = false;

                treeViewFolderBrowser1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region FolderTree - Drag and Drop - Drag Enter - update selected node
        private void treeViewFolderBrowser1_DragEnter(object sender, DragEventArgs e)
        {
            isInternalDrop = true;

            try
            {
                GlobalData.IsDragAndDropActive = true;
                currentNodeWhenStartDragging = treeViewFolderBrowser1.SelectedNode;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "folderTreeViewFolder_DragEnter");
            }
        }
        #endregion


        enum DragDropKeyStates
        {
            AltKey =32, //The ALT key is pressed.
            ControlKey = 8, //The control (CTRL) key is pressed.
            LeftMouseButton	= 1, //The left mouse button is pressed.
            MiddleMouseButton = 16, //The middle mouse button is pressed.
            None = 0,//No modifier keys or mouse buttons are pressed.
            RightMouseButton = 2, //The right mouse button is pressed.
            ShiftKey = 4 //The shift (SHIFT) key is pressed.
        }

        #region FolderTree - Drag and Drop - Drag Over - update folderTreeViewFolder.SelectedNode
        private void treeViewFolderBrowser1_DragOver(object sender, DragEventArgs e)
        {
            isInternalDrop = true;
            try
            {
                if (((DragDropKeyStates)e.KeyState & DragDropKeyStates.ShiftKey) == DragDropKeyStates.ShiftKey)
                    e.Effect = DragDropEffects.Move;
                else if (((DragDropKeyStates)e.KeyState & DragDropKeyStates.RightMouseButton) == DragDropKeyStates.RightMouseButton)
                    e.Effect = DragDropEffects.Copy;
                else if (((DragDropKeyStates)e.KeyState & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey)
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.Move;

                GlobalData.DoNotRefreshImageListView = true;
                // Retrieve the client coordinates of the mouse position.  
                Point targetPoint = treeViewFolderBrowser1.PointToClient(new Point(e.X, e.Y));

                // Select the node at the mouse position.  
                treeViewFolderBrowser1.SelectedNode = treeViewFolderBrowser1.GetNodeAt(targetPoint);
                GlobalData.DoNotRefreshImageListView = false;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "folderTreeViewFolder_DragOver");
            }
        }
        #endregion
        #endregion


    }
}
