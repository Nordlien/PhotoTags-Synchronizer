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

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        

        

        

        #region ToolStrip - Select all Items - Click
        private void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
                using (new WaitCursor())
                {
                    imageListView1.SelectAll();
                }
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                FilesSelected();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion



        #region ToolStrip - InageListView - Switch Renderers 
        private struct RendererItem
        {
            public Type Type;
            public override string ToString()
            {
                return Type.Name;
            }

            public RendererItem(Type type)
            {
                Type = type;
            }
        }
        private void SetImageListViewRender(Manina.Windows.Forms.View imageListViewViewMode, RendererItem selectedRender)
        {
            try
            {
                Properties.Settings.Default.ImageListViewRendererName = selectedRender.Type.Name;
                Properties.Settings.Default.ImageListViewViewMode = (int)imageListViewViewMode;
                
                imageListView1.View = imageListViewViewMode;                
                Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
                ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(selectedRender.Type.FullName) as ImageListView.ImageListViewRenderer;
                imageListView1.SetRenderer(renderer);
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void KryptonContextMenuItemRenderers_Click(object sender, EventArgs e)
        {
            KryptonContextMenuItem kryptonContextMenuItem = (KryptonContextMenuItem)sender;
            SetImageListViewRender(Manina.Windows.Forms.View.Thumbnails, (RendererItem)kryptonContextMenuItem.Tag);           
        }

        #endregion

        #region ToolStrip - ImageListView - Switch View Modes
        
        private void kryptonRibbonGroupButtonImageListViewModeGallery_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Gallery, imageListViewSelectedRenderer);
        }

        private void kryptonRibbonGroupButtonImageListViewModeDetails_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Details, imageListViewSelectedRenderer);
        }

        private void kryptonRibbonGroupButtonImageListViewModePane_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Pane, imageListViewSelectedRenderer);
        }

        private void kryptonRibbonGroupButtonImageListViewModeThumbnails_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Thumbnails, imageListViewSelectedRenderer);
        }

        
        #endregion

        #region ToolStrip - ImageListView - Modify Column Headers - Click
        private void kryptonRibbonGroupButtonImageListViewDetailviewColumns_Click(object sender, EventArgs e)
        {
            try
            {
                FormChooseColumns form = new FormChooseColumns();
                form.imageListView = imageListView1;
                int index = 0;
                if (imageListView1.View == Manina.Windows.Forms.View.Thumbnails) index = 1;
                form.Populate(index);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Change Thumbnail Size - Click

        private void SetThumbnailSize (int size)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[size];
            Properties.Settings.Default.ThumbmailViewSizeIndex = size;
            kryptonRibbonGroupButtonThumbnailSizeXLarge.Checked = (size == 4);
            kryptonRibbonGroupButtonThumbnailSizeLarge.Checked = (size == 3);
            kryptonRibbonGroupButtonThumbnailSizeMedium.Checked = (size == 2);
            kryptonRibbonGroupButtonThumbnailSizeSmall.Checked = (size == 1);
            kryptonRibbonGroupButtonThumbnailSizeXSmall.Checked = (size == 0);
        }

        
        private void kryptonRibbonGroupButtonThumbnailSizeXLarge_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(4);            
        }

        private void kryptonRibbonGroupButtonThumbnailSizeLarge_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(3);           
        }

        private void kryptonRibbonGroupButtonThumbnailSizeMedium_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(2);
        }

        private void kryptonRibbonGroupButtonThumbnailSizeSmall_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(1);
        }

        private void kryptonRibbonGroupButtonThumbnailSizeXSmall_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(0);
        }
        #endregion

        #region ImageListView Sort
        private void ImageListViewSortColumn(ImageListView imageListView, ColumnType columnToSort)
        {
            if (imageListView.SortColumn == columnToSort)
            {
                if (imageListView.SortOrder == SortOrder.Descending) imageListView.SortOrder = SortOrder.Ascending;
                else imageListView.SortOrder = SortOrder.Descending;
            }
            else
            {
                imageListView.SortColumn = columnToSort;
                imageListView.SortOrder = SortOrder.Ascending;
            }
        }

        private void ToolStripMenuItemSortByFilename_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByFileCreatedDate_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.FileDateCreated);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByFileModifiedDate_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.FileDateModified);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaDateTaken_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaDateTaken);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaAlbum_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaAlbum);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaTitle_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaTitle);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaDescription_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaDescription);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaComments_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaComment);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaAuthor_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaAuthor);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaRating_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaRating);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByLocationName_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.LocationName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByLocationRegionState_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.LocationRegionState);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByLocationCity_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.LocationCity);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByLocationCountry_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.LocationCountry);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        
        

        #region ToolStrip - Reload Metadata - Delete Last Mediadata And Reload
        void DeleteLastMediadataAndReload(ImageListView imageListView, bool updatedOnlySelected)
        {
            try
            {
                if (GlobalData.IsPopulatingAnything()) return;

                using (new WaitCursor())
                {
                    GlobalData.IsPopulatingButtonAction = true;
                    GlobalData.IsPopulatingImageListView = true; //Avoid one and one select item getting refreshed
                    GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
                    folderTreeViewFolder.Enabled = false;
                    ImageListViewSuspendLayoutInvoke(imageListView);

                    //Clean up ImageListView and other queues
                    ImageListViewClearThumbnailCache(imageListView1);
                    //imageListView1.Refresh();
                    ClearAllQueues();

                    UpdateStatusAction("Delete all data and files...");
                    lock (GlobalData.ReloadAllowedFromCloudLock)
                    {
                        GlobalData.ReloadAllowedFromCloud = filesCutCopyPasteDrag.DeleteFileEntriesBeforeReload(imageListView.Items, updatedOnlySelected);
                    }
                    filesCutCopyPasteDrag.ImageListViewReload(imageListView.Items, updatedOnlySelected);

                    folderTreeViewFolder.Enabled = true;
                    ImageListViewResumeLayoutInvoke(imageListView);
                    GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                    GlobalData.IsPopulatingButtonAction = false;
                    GlobalData.IsPopulatingImageListView = false;

                    FilesSelected();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

       
       


        #region ImageListView

        

        #endregion

        #region FoldeTree

        #region FolderTree - Folder - Click
        private void folderTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
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
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void CopyOrMove(DragDropEffects dragDropEffects, TreeNode targetNode, StringCollection fileDropList, string targetDirectory)
        {
            try
            {
                if (dragDropEffects == DragDropEffects.None)
                {
                    MessageBox.Show("Was not able to detect if you select copy or cut object that was pasted or dropped");
                    return;
                }

                if (dragDropEffects == DragDropEffects.None)
                {
                    if (IsFileInThreadQueueLock(fileDropList))
                    {
                        MessageBox.Show("Can't move files. Files are being used, you need wait until process is finished.");
                        return;
                    }
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
                        MoveFiles(folderTreeViewFolder, imageListView1, files, targetDirectory);
                    else
                        CopyFiles(folderTreeViewFolder, files, targetDirectory);

                    foreach (string sourceDirectory in directories)
                    {
                        string newTagretDirectory = Path.Combine(targetDirectory, new DirectoryInfo(sourceDirectory).Name); //Target directory + dragged (drag&drop) direcotry
                                                                                                                            //TreeNode targetNode = folderTreeViewFolder.SelectedNode;
                        TreeNode sourceNode = folderTreeViewFolder.FindFolder(sourceDirectory);

                        if (dragDropEffects == DragDropEffects.Move)
                            MoveFolder(folderTreeViewFolder, sourceNode, targetNode, sourceDirectory, newTagretDirectory);
                        else
                            CopyFolder(folderTreeViewFolder, targetNode, sourceDirectory, newTagretDirectory);
                    }
                }

                folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
                filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, currentNodeWhenStartDragging);
                folderTreeViewFolder.Focus();

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderTree - Drag and Drop - Drop - Move/Copy Files - Move/Copy Folders
        private void folderTreeViewFolder_DragDrop(object sender, DragEventArgs e)
        {
            try
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
                }
                #endregion

                GlobalData.IsDragAndDropActive = false;
                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region FolderTree - Drag and Drop - ContainsNode?
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
        #endregion

        #region FolderTree - Drag and Drop - Node Mouse Click - Set clickedNode
        private void folderTreeViewFolder_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                //clickedNode = e.Node;
                currentNodeWhenStartDragging = e.Node;

                if (e.Button == MouseButtons.Right)
                {
                    folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (((Furty.Windows.Forms.FolderTreeView)sender).SelectedNode == e.Node) PopulateImageListView_FromFolderSelected(false, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderTree - Drag and Drop - Item Drag - Set Clipboard data to ** TreeViewFolder.Item ** | Move | Copy | Link |
        private void folderTreeViewFolder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                currentNodeWhenStartDragging = (TreeNode)e.Item;
                Clipboard.Clear();

                if (currentNodeWhenStartDragging != null)
                {
                    string sourceDirectory = Furty.Windows.Forms.ShellOperations.GetFileDirectory(currentNodeWhenStartDragging);
                    var droplist = new StringCollection();
                    droplist.Add(sourceDirectory);

                    DataObject data = new DataObject();
                    data.SetFileDropList(droplist);
                    data.SetData("Preferred DropEffect", DragDropEffects.Move);
                    Clipboard.SetDataObject(data, true);
                    DragDropEffects dragDropEffects = folderTreeViewFolder.DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link); // Allowed effects

                    if (!isInternalDrop)
                    {
                        if (dragDropEffects == DragDropEffects.Move) //Moved a folder to new location in eg. Windows Explorer
                        {
                            imageListView1.ClearSelection();

                            TreeNode sourceNode = currentNodeWhenStartDragging;
                            TreeNode parentNode = currentNodeWhenStartDragging.Parent;
                            if (parentNode == null) folderTreeViewFolder.SelectedNode = folderTreeViewFolder.Nodes[0];

                            //------ Update node tree -----
                            GlobalData.DoNotRefreshImageListView = true;
                            if (sourceNode != null) sourceNode.Remove();
                            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, parentNode);
                            GlobalData.DoNotRefreshImageListView = false;

                            //----- Updated ImageListView with files ------
                            PopulateImageListView_FromFolderSelected(false, true);
                        }
                        else //Copied or NOT (cancel) a folder to new location in eg. Windows Explorer
                        {
                            GlobalData.DoNotRefreshImageListView = true;
                            folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
                            GlobalData.DoNotRefreshImageListView = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No node folder was selected");
                    folderTreeViewFolder.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "folderTreeViewFolder_ItemDrag, Failed create drag and drop tarnsfer data.");
                MessageBox.Show("Failed create drag and drop tarnsfer data. Error: " + ex.Message);
                folderTreeViewFolder.Focus();
            }
        }
        #endregion 

        #region FolderTree - Drag and Drop - Drag Leave - Set Clipboard data to ** FileDropList ** | Link |
        private void folderTreeViewFolder_DragLeave(object sender, EventArgs e)
        {
            try
            {
                isInternalDrop = false;

                GlobalData.IsDragAndDropActive = false;

                GlobalData.DoNotRefreshImageListView = true;
                folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
                GlobalData.DoNotRefreshImageListView = false;

                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region FolderTree - Drag and Drop - Drag Enter - update selected node
        private void folderTreeViewFolder_DragEnter(object sender, DragEventArgs e)
        {
            isInternalDrop = true;

            try
            {
                GlobalData.IsDragAndDropActive = true;
                currentNodeWhenStartDragging = folderTreeViewFolder.SelectedNode;
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
        private void folderTreeViewFolder_DragOver(object sender, DragEventArgs e)
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
                Point targetPoint = folderTreeViewFolder.PointToClient(new Point(e.X, e.Y));

                // Select the node at the mouse position.  
                folderTreeViewFolder.SelectedNode = folderTreeViewFolder.GetNodeAt(targetPoint);
                GlobalData.DoNotRefreshImageListView = false;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "folderTreeViewFolder_DragOver");
            }
        }
        #endregion
        #endregion

        #region Select Group

        #region Select Group - Populate ToolStripMenuItem
        private void PopulateSelectGroupToolStripMenuItems()
        {
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 1) toolStripMenuItemSelectSameDay.Checked = true;
            else toolStripMenuItemSelectSameDay.Checked = false;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 3) toolStripMenuItemSelectSame3Day.Checked = true;
            else toolStripMenuItemSelectSame3Day.Checked = false;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 7) toolStripMenuItemSelectSameWeek.Checked = true;
            else toolStripMenuItemSelectSameWeek.Checked = false;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 14) toolStripMenuItemSelectSame2week.Checked = true;
            else toolStripMenuItemSelectSame2week.Checked = false;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 30) toolStripMenuItemSelectSameMonth.Checked = true;
            else toolStripMenuItemSelectSameMonth.Checked = false;

            if (Properties.Settings.Default.SelectGroupMaxCount == 10) toolStripMenuItemSelectMax10items.Checked = true;
            else toolStripMenuItemSelectMax10items.Checked = false;
            if (Properties.Settings.Default.SelectGroupMaxCount == 30) toolStripMenuItemSelectMax30items.Checked = true;
            else toolStripMenuItemSelectMax30items.Checked = false;
            if (Properties.Settings.Default.SelectGroupMaxCount == 50) toolStripMenuItemSelectMax50items.Checked = true;
            else toolStripMenuItemSelectMax50items.Checked = false;
            if (Properties.Settings.Default.SelectGroupMaxCount == 100) toolStripMenuItemSelectMax100items.Checked = true;
            else toolStripMenuItemSelectMax100items.Checked = false;

            toolStripMenuItemSelectFallbackOnFileCreated.Checked = Properties.Settings.Default.SelectGroupFileCreatedFallback;
            toolStripMenuItemSelectSameLocationName.Checked = Properties.Settings.Default.SelectGroupSameLocationName;
            toolStripMenuItemSelectSameCity.Checked = Properties.Settings.Default.SelectGroupSameCity;
            toolStripMenuItemSelectSameDistrict.Checked = Properties.Settings.Default.SelectGroupSameDistrict;
            toolStripMenuItemSelectSameCountry.Checked = Properties.Settings.Default.SelectGroupSameCountry;

            GroupSelectionClear();
        }
        #endregion 

        #region Select Group - Previous
        private void toolStripButtonSelectPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                lastGroupDirection = -1;
                int baseItemIndex = SelectedGroupFindBaseItemIndex(imageListView1, lastGroupDirection);

                SelectedGroupBySelections(imageListView1, baseItemIndex, lastGroupDirection,
                    Properties.Settings.Default.SelectGroupMaxCount,
                    Properties.Settings.Default.SelectGroupNumberOfDays,
                    Properties.Settings.Default.SelectGroupFileCreatedFallback,
                    Properties.Settings.Default.SelectGroupSameLocationName,
                    Properties.Settings.Default.SelectGroupSameCity,
                    Properties.Settings.Default.SelectGroupSameDistrict,
                    Properties.Settings.Default.SelectGroupSameCountry);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Next
        private void toolStripButtonSelectNext_Click(object sender, EventArgs e)
        {
            try
            {
                lastGroupDirection = 1;
                int baseItemIndex = SelectedGroupFindBaseItemIndex(imageListView1, lastGroupDirection);

                SelectedGroupBySelections(imageListView1, baseItemIndex, lastGroupDirection,
                    Properties.Settings.Default.SelectGroupMaxCount,
                    Properties.Settings.Default.SelectGroupNumberOfDays,
                    Properties.Settings.Default.SelectGroupFileCreatedFallback,
                    Properties.Settings.Default.SelectGroupSameLocationName,
                    Properties.Settings.Default.SelectGroupSameCity,
                    Properties.Settings.Default.SelectGroupSameDistrict,
                    Properties.Settings.Default.SelectGroupSameCountry);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Select Group - Select Last
        public void GroupSelectLast()
        {
            int baseItemIndex = SelectedGroupFindBaseItemIndex(imageListView1, 0);

            SelectedGroupBySelections(imageListView1, baseItemIndex, lastGroupDirection,
                Properties.Settings.Default.SelectGroupMaxCount,
                Properties.Settings.Default.SelectGroupNumberOfDays,
                Properties.Settings.Default.SelectGroupFileCreatedFallback,
                Properties.Settings.Default.SelectGroupSameLocationName,
                Properties.Settings.Default.SelectGroupSameCity,
                Properties.Settings.Default.SelectGroupSameDistrict,
                Properties.Settings.Default.SelectGroupSameCountry);
        }
        #endregion 

        #region Select Group - Fallback on File Created
        private void toolStripMenuItemSelectFallbackOnFileCreated_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
                Properties.Settings.Default.SelectGroupFileCreatedFallback = !toolStripMenuItem.Checked;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same day
        private void toolStripMenuItemSelectSameDay_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSameDay.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 1;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same 3 days
        private void toolStripMenuItemSelectSame3Day_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSame3Day.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 3;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same week
        private void toolStripMenuItemSelectSameWeek_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSameWeek.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 7;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same 2 weeks
        private void toolStripMenuItemSelectSame2week_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSame2week.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 14;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Select Group - Same month
        private void toolStripMenuItemSelectSameMonth_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSameMonth.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 30;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Max 10 items
        private void toolStripMenuItemSelectMax10items_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SelectGroupMaxCount = 10;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Max 30 items
        private void toolStripMenuItemSelectMax30items_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SelectGroupMaxCount = 30;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - 50 items
        private void toolStripMenuItemSelectMax50items_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SelectGroupMaxCount = 50;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - 100 items
        private void toolStripMenuItemSelectMax100items_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SelectGroupMaxCount = 100;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same Location Name
        private void toolStripMenuItemSelectSameLocationName_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
                Properties.Settings.Default.SelectGroupSameLocationName = !toolStripMenuItem.Checked;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same City
        private void toolStripMenuItemSelectSameCity_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
                Properties.Settings.Default.SelectGroupSameCity = !toolStripMenuItem.Checked;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Select Group - District
        private void toolStripMenuItemSelectSameDistrict_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
            Properties.Settings.Default.SelectGroupSameDistrict = !toolStripMenuItem.Checked;
            PopulateSelectGroupToolStripMenuItems();
            GroupSelectLast();
        }
        #endregion 

        #region Select Group - Same Country
        private void toolStripMenuItemSelectSameCountry_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
                Properties.Settings.Default.SelectGroupSameCountry = !toolStripMenuItem.Checked;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #endregion 
    }
}
