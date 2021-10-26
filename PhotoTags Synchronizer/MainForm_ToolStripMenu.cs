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
        private void CopyOrMove_UpdatedBrowserTreeView(DragDropEffects dragDropEffects, StringCollection sourceFilesAndFolders, string targetFolder, TreeNode targetNode)
        {
            try
            {
                if (dragDropEffects == DragDropEffects.None)
                {                    
                    KryptonMessageBox.Show("Was not able to detect if you select copy or cut object that was pasted or dropped");
                    return;
                }

                if (!Directory.Exists(targetFolder))
                {
                    KryptonMessageBox.Show("Target folder is not a valid target folder.\r\nSelected system folder:" + targetNode?.FullPath == null ? "Unkown" : targetNode?.FullPath);
                    return;
                }

                if (IsFileInThreadQueueLock(sourceFilesAndFolders))
                {
                    KryptonMessageBox.Show("Can't " + dragDropEffects.ToString() + " files. Files are being used, you need wait until process is finished.");
                    return;
                }

                StringCollection sourceFiles = new StringCollection();
                StringCollection sourceFolders = new StringCollection();
                StringCollection sourceFilesSameAsTargetFiles = new StringCollection();
                StringCollection sourceFoldersSameAsTagetFolders = new StringCollection();

                int numberOfFilesAndFolders = 0;
                string informationTextCopyFromFolders = "";
                int countFoldersSelected = 0;

                foreach (string sourceFileOrFolder in sourceFilesAndFolders)
                {
                    if (File.Exists(sourceFileOrFolder)) //Check if is a file
                    {
                        if (Path.GetDirectoryName(sourceFileOrFolder) != targetFolder)
                        {
                            sourceFiles.Add(sourceFileOrFolder);
                            numberOfFilesAndFolders++;
                        }
                        else sourceFilesSameAsTargetFiles.Add(sourceFileOrFolder);
                    }
                    else if (Directory.Exists(sourceFileOrFolder)) //If not file, check if folder and still exists
                    {
                        if (sourceFileOrFolder != targetFolder)
                        {
                            sourceFolders.Add(sourceFileOrFolder);
                            numberOfFilesAndFolders++;

                            if (numberOfFilesAndFolders <= 51) //Check if lot of files being processed
                            {
                                string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(sourceFileOrFolder, "*", SearchOption.AllDirectories).Take(51).ToArray();
                                numberOfFilesAndFolders += fileAndFolderEntriesCount.Length;
                            }

                            countFoldersSelected++;
                            if (countFoldersSelected < 3) informationTextCopyFromFolders += sourceFileOrFolder + "\r\n";                            
                            else if (countFoldersSelected == 4) informationTextCopyFromFolders += "and more directories...\r\n";
                        }
                        else sourceFoldersSameAsTagetFolders.Add(sourceFileOrFolder);
                    }
                }

                if (numberOfFilesAndFolders >= 1)
                {
                    if (numberOfFilesAndFolders <= 50 ||
                        (MessageBox.Show("You are about to " + dragDropEffects.ToString() + " " + (numberOfFilesAndFolders > 50 ? "over 50+" : numberOfFilesAndFolders.ToString()) + " files and/or folders.\r\n\r\n" +
                        "From:\r\n" + informationTextCopyFromFolders + "\r\n\r\n" +
                        "To folder:\r\n" + targetFolder + "\r\n\r\n" +
                        "Procced?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK))
                    {

                        if (dragDropEffects == DragDropEffects.Move)
                            MoveFilesNoRename_UpdateTreeViewFolderBrowser(treeViewFolderBrowser1, imageListView1, sourceFiles, targetFolder, targetNode);
                        else
                            CopyFiles_UpdateTreeViewFolderBrowser(treeViewFolderBrowser1,                 sourceFiles, targetFolder, targetNode); 

                        foreach (string sourceDirectory in sourceFolders)
                        {
                            string newTagretDirectory = Path.Combine(targetFolder, new DirectoryInfo(sourceDirectory).Name); //Target directory + dragged (drag&drop) direcotry

                            if (dragDropEffects == DragDropEffects.Move)
                                MoveFolder_UpdateTreeViewFolderBrowser(treeViewFolderBrowser1, sourceDirectory, newTagretDirectory, targetNode);
                            else
                                CopyFolder_UpdateTreeViewFolderBrowser(treeViewFolderBrowser1, sourceDirectory, newTagretDirectory, targetNode);

                        }


                        treeViewFolderBrowser1.SelectedNode = targetNode;
                        treeViewFolderBrowser1.SelectedNode.Expand();
                        PopulateImageListView_FromFolderSelected(false, true);
                        treeViewFolderBrowser1.Focus();
                    }
                } 
                else
                {
                    string fileMessage = "";
                    if (sourceFilesSameAsTargetFiles.Count == 1) fileMessage = "Source file: "+ sourceFilesSameAsTargetFiles[0] + "\r\n";
                    else if (sourceFilesSameAsTargetFiles.Count >= 1) fileMessage = "File count: " + sourceFilesSameAsTargetFiles.Count + "\r\n";

                    string folderMessage = "";
                    if (sourceFoldersSameAsTagetFolders.Count == 1) folderMessage = "";
                    else if (sourceFoldersSameAsTagetFolders.Count >= 1) folderMessage = "Folder count: " + sourceFoldersSameAsTagetFolders.Count + "\r\n";

                    KryptonMessageBox.Show("Can't " + dragDropEffects.ToString() + " files. \r\n" +
                        "Source and destiation are the same.\r\n\r\n" +
                        "Target folder: " + targetFolder + "\r\n" +
                        fileMessage +
                        folderMessage);
                }
                
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AfterAfterLabelEdit(TreeNode node, string newLabel, string oldLabel)
        {
            try
            {
                using (new WaitCursor())
                {
                    treeViewFolderBrowser1.SuspendLayout();
                    string sourceDirectory = GetSelectedNodePath();
                    if (Directory.Exists(sourceDirectory))
                    {
                        string newTagretDirectory = Path.Combine((new DirectoryInfo(sourceDirectory).Parent).FullName, newLabel);
                        TreeNode treeNodeParent = node.Parent;
                        treeNodeParent.Collapse();

                        MoveFolder_UpdateTreeViewFolderBrowser(treeViewFolderBrowser1, sourceDirectory, newTagretDirectory, treeNodeParent);

                        filesCutCopyPasteDrag.TreeViewFolderBrowserRefreshTreeNode(treeViewFolderBrowser1, treeNodeParent); //Need refresh, don't know why yet, it should already been done
                        //Set Selected Node back to the node that was renamed
                        foreach (TreeNode treeNode in treeNodeParent.Nodes)
                        {
                            if (treeNode.Text == newLabel || treeNode.Text == oldLabel) //Select the node with new name or old name, in case rename failed
                            {
                                treeNode.TreeView.SelectedNode = treeNode;
                                treeNode.Expand();
                                break;
                            }
                        }
                    }
                    else
                    {
                        KryptonMessageBox.Show("Can't edit folder name. No valid folder selected.");
                    }
                    treeViewFolderBrowser1.ResumeLayout();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void treeViewFolderBrowser1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                if (e.Label != null && e.Label != e.Node.Text)
                {
                    this.BeginInvoke(new Action<TreeNode, string, string>(AfterAfterLabelEdit), e.Node, e.Label, e.Node.Text);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                #region Copy or Move media files dropped to new folder from external source
                string[] filesAndFolders = (string[])e.Data.GetData(DataFormats.FileDrop); //Check if files been dropped
                if (filesAndFolders != null && filesAndFolders.Length > 0)
                {
                    StringCollection fileAndFolders = new StringCollection();
                    fileAndFolders.AddRange(filesAndFolders);
                    CopyOrMove_UpdatedBrowserTreeView(e.Effect, fileAndFolders, targetDirectory, targetNode);
                }
                #endregion

                GlobalData.IsDragAndDropActive = false;
                
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
                if (e.Button == MouseButtons.Right)
                {
                    treeViewFolderBrowser1.SelectedNode = e.Node;
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

        #region FolderTree - Drag and Drop

        #region FolderTree - Drag and Drop - SetDropDropFileList
        private DataObject SetDropDropFileList(string sourceDirectory)
        {           
            var droplist = new StringCollection();
            droplist.Add(sourceDirectory);
            return SetDropDropFileList(droplist);
        }

        private DataObject SetDropDropFileList(StringCollection sourceFilesOrDirectory)
        {
            DataObject data = new DataObject();
            data.SetFileDropList(sourceFilesOrDirectory);
            data.SetData("Preferred DropEffect", DragDropEffects.Move);
            Clipboard.SetDataObject(data, true);
            return data;
        }
        #endregion

        #region FolderTree - Drag and Drop - treeViewFolderBrowser1_ItemDrag
        private void treeViewFolderBrowser1_ItemDrag(object sender, ItemDragEventArgs e)
        {
         
            try
            {
                TreeNode currentNode = (TreeNode)e.Item;
                string sourceDirectory = GetNodeFolderPath(currentNode as TreeNodePath);
                
                if (currentNode != null && Directory.Exists(sourceDirectory))
                {
                    DataObject data = SetDropDropFileList(sourceDirectory);
                    DragDropEffects dragDropEffects = treeViewFolderBrowser1.DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link); // Allowed effects
              
                }
                else
                {
                    SetDropDropFileList(""); //Removes error message for wrong Data in Clipboard
                    treeViewFolderBrowser1.Focus();
                }
            }
            catch (Exception ex)
            {
                SetDropDropFileList(""); //Removes error message for wrong Data in Clipboard

                Logger.Error(ex, "folderTreeViewFolder_ItemDrag, Failed create drag and drop transfer data.");
                KryptonMessageBox.Show("Failed create drag and drop transfer data. Error: " + ex.Message);
                treeViewFolderBrowser1.Focus();
            }
        }
        #endregion

        #endregion

        #region FolderTree - Drag and Drop - Drag Leave - Set Clipboard data to ** FileDropList ** | Link |
        private void treeViewFolderBrowser1_DragLeave(object sender, EventArgs e)
        {
            try
            {
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

        #region FolderTree - Drag and Drop - Drag Enter - update selected node
        private void treeViewFolderBrowser1_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                GlobalData.IsDragAndDropActive = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "folderTreeViewFolder_DragEnter");
            }
        }
        #endregion

        #region DragDropKeyStates
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
        #endregion

        #region FolderTree - Drag and Drop - Drag Over - update folderTreeViewFolder.SelectedNode
        private void treeViewFolderBrowser1_DragOver(object sender, DragEventArgs e)
        {
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
