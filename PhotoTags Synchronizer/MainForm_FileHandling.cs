using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using Exiftool;
using Raccoom.Windows.Forms;
using Manina.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;
using FileHandeling;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region RenameFileProcess from thread queue
        private void RenameFileProcess_UpdateTreeViewFolderBroswer(TreeViewFolderBrowser folderTreeView, ImageListView imageListView, string sourceFullFilename, string targetFullFilename)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<TreeViewFolderBrowser, ImageListView, string, string>(RenameFileProcess_UpdateTreeViewFolderBroswer), folderTreeView, imageListView, sourceFullFilename, targetFullFilename);
                return;
            }

            StringCollection files = new StringCollection();
            files.Add(sourceFullFilename);
            MoveFiles_UpdateTreeViewFolderBrowser(folderTreeView, imageListView, files, targetFullFilename, null);
        }
        #endregion 

        #region Move Files to target folder
        private void MoveFiles_UpdateTreeViewFolderBrowser(TreeViewFolderBrowser folderTreeView, ImageListView imageListView, StringCollection files, string targetNodeDirectory, TreeNode treeNodeTarget)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<TreeViewFolderBrowser, ImageListView, StringCollection, string, TreeNode>(MoveFiles_UpdateTreeViewFolderBrowser), folderTreeView, imageListView, files, targetNodeDirectory, treeNodeTarget);
                return;
            }

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            using (new WaitCursor())
            {
                imageListView.SuspendLayout();

                foreach (string oldPath in files) //Move all files to target directory 
                {
                    string sourceFullFilename = oldPath;
                    string filename = Path.GetFileName(sourceFullFilename);
                    string targetFullFilename = Path.Combine(targetNodeDirectory, filename);
                    try
                    {
                        bool directoryCreated = filesCutCopyPasteDrag.MoveFile(sourceFullFilename, targetFullFilename);

                        //------ Update node tree -----
                        GlobalData.DoNotRefreshImageListView = true;

                        string sourceFolder = Path.GetDirectoryName(sourceFullFilename);
                        List<TreeNode> sourceNodes = filesCutCopyPasteDrag.TreeViewFolderBrowserFindAllNodes(treeViewFolderBrowser1.Nodes, sourceFolder);
                        foreach (TreeNode sourceTreeNode in sourceNodes)
                        {
                            if (sourceTreeNode != null && sourceTreeNode != treeNodeTarget) filesCutCopyPasteDrag.TreeViewFolderBrowserRemoveTreeNode(folderTreeView, sourceTreeNode);
                        }

                        if (treeNodeTarget == null)
                        {
                            string targetFolder = Path.GetDirectoryName(targetFullFilename);
                            List<TreeNode> targetNodes = filesCutCopyPasteDrag.TreeViewFolderBrowserFindAllNodes(treeViewFolderBrowser1.Nodes, targetFolder);
                            foreach (TreeNode targetNode in targetNodes)
                            {
                                filesCutCopyPasteDrag.TreeViewFolderBrowserRemoveTreeNode(folderTreeView, targetNode);
                            }
                        }
                        else filesCutCopyPasteDrag.TreeViewFolderBrowserRefreshTreeNode(folderTreeView, treeNodeTarget);

                        GlobalData.DoNotRefreshImageListView = false;

                        ImageListViewItem foundItem = FindItemInImageListView(imageListView.Items, sourceFullFilename);
                        if (foundItem != null) imageListView.Items.Remove(foundItem);
                    }
                    catch (Exception ex)
                    {

                        DateTime dateTimeLastWriteTime = DateTime.Now;
                        try
                        {
                            dateTimeLastWriteTime = File.GetLastWriteTime(sourceFullFilename);
                        }
                        catch { }
                        AddError(
                            Path.GetDirectoryName(sourceFullFilename),
                            Path.GetFileName(sourceFullFilename),
                            dateTimeLastWriteTime,
                            AddErrorFileSystemRegion, AddErrorFileSystemMove, sourceFullFilename, targetFullFilename,
                            "Failed moving file.\r\n\r\n" +
                            "From:" + sourceFullFilename + "\r\n\r\n" +
                            "To: " + targetFullFilename + "\r\n\r\n" +
                            "Error message: " + ex.Message + "\r\n");
                        Logger.Error(ex, "Error when move file.");
                    }
                }
                imageListView.ResumeLayout();
            }
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;

            FilesSelected();
        }
        #endregion

        #region Move Folder
        private void MoveFolder_UpdateTreeViewFolderBrowser(TreeViewFolderBrowser folderTreeView, string sourceDirectory, string targetDirectory, TreeNode targetNode)
        {
            if (sourceDirectory == targetDirectory) return; //Can't move into itself. No need for error message

            try
            {
                string[] allSourceFullFilenames = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);
                using (new WaitCursor())
                {
                    //----- Move all folder and files -----
                    Logger.Trace("Move folder from:" + sourceDirectory + " to: " + targetDirectory);
                    System.IO.Directory.Move(sourceDirectory, targetDirectory);

                    //------ Clear ImageListView -----
                    ImageListViewClearAll(imageListView1);

                    //------ Update node tree -----
                    GlobalData.DoNotRefreshImageListView = true;

                    List<TreeNode> sourceNodes = filesCutCopyPasteDrag.TreeViewFolderBrowserFindAllNodes(treeViewFolderBrowser1.Nodes, sourceDirectory);
                    foreach (TreeNode sourceTreeNode in sourceNodes)
                    {
                        if (sourceTreeNode != null && sourceTreeNode != targetNode) filesCutCopyPasteDrag.TreeViewFolderBrowserRemoveTreeNode(folderTreeView, sourceTreeNode);
                    }
                    if (targetNode != null) filesCutCopyPasteDrag.TreeViewFolderBrowserRefreshTreeNode(folderTreeView, targetNode);
                    
                    GlobalData.DoNotRefreshImageListView = false;

                    //------ Update database -----
                    foreach (string oldFullFilename in allSourceFullFilenames)
                    {
                        string oldFilename = Path.GetFileName(oldFullFilename);
                        string newFullFilename = Path.Combine(targetDirectory, oldFilename);
                        Logger.Trace("Rename from:" + oldFullFilename + " to: " + newFullFilename);
                        databaseAndCacheMetadataExiftool.Move(Path.GetDirectoryName(oldFullFilename), Path.GetFileName(oldFullFilename), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                    }
                }
                //----- Updated ImageListView with files ------
                PopulateImageListView_FromFolderSelected(false, true);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error when move folder.");

                AddError(
                    sourceDirectory,
                    AddErrorFileSystemRegion, AddErrorFileSystemMoveFolder, sourceDirectory, targetDirectory,
                    "Failed moving directory.\r\n\r\n" +
                    "From: " + sourceDirectory + "\r\n\r\n" +
                    "To: " + targetDirectory + "\r\n\r\n" +
                    "Error message:\r\n" + ex.Message);

            }
        }
        #endregion

        #region Copy files
        private void CopyFiles_UpdateTreeViewFolderBrowser(TreeViewFolderBrowser folderTreeView, StringCollection files, string targetNodeDirectory, TreeNode targetNode)
        {
            using (new WaitCursor())
            {
                foreach (string oldPath in files) //Move all files to target directory 
                {
                    string sourceFullFilename = oldPath;
                    string filename = Path.GetFileName(sourceFullFilename);
                    string targetFullFilename = Path.Combine(targetNodeDirectory, filename);

                    try
                    {                        
                        bool directoryCreated = filesCutCopyPasteDrag.CopyFile(sourceFullFilename, targetFullFilename);

                        if (directoryCreated)
                        {
                            GlobalData.DoNotRefreshImageListView = true;
                            filesCutCopyPasteDrag.TreeViewFolderBrowserRefreshTreeNode(folderTreeView, targetNode);
                            GlobalData.DoNotRefreshImageListView = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        DateTime dateTimeLastWriteTime = DateTime.Now;
                        try
                        {
                            dateTimeLastWriteTime = File.GetLastWriteTime(sourceFullFilename);
                        }
                        catch { }

                        AddError(
                            Path.GetDirectoryName(sourceFullFilename),
                            Path.GetFileName(sourceFullFilename),
                            dateTimeLastWriteTime, sourceFullFilename, targetFullFilename,
                            AddErrorFileSystemRegion, AddErrorFileSystemCopy,
                            "Failed copying file.\r\n\r\n" +
                            "Error copy file from: " + sourceFullFilename + "\r\n\r\n" +
                            "To file: " + targetFullFilename + "\r\n\r\n" +
                            "Error message: " + ex.Message + "\r\n");
                        Logger.Error(ex, "Error when copy file.");
                    }
                }
            }

            FilesSelected();
        }
        #endregion

        #region Copy Folder
        private void CopyFolder_UpdateTreeViewFolderBrowser(TreeViewFolderBrowser folderTreeView, string sourceDirectory, string tagretDirectory, TreeNode targetNode)
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
                    Logger.Error(ex, "Error when create directory when copy all files from folder");
                    AddError(dirPath,
                        AddErrorFileSystemRegion, AddErrorFileSystemCreateFolder, dirPath.Replace(sourceDirectory, tagretDirectory), dirPath.Replace(sourceDirectory, tagretDirectory),
                        "Failed create directory\r\n\r\n" +
                        "Directory: " + dirPath + "\r\n\r\n" +
                        "Error message: " + ex.Message + "\r\n");
                }
            }
            using (new WaitCursor())
            {
                //Copy all the files & Replaces any files with the same name
                foreach (string sourceFullFilename in allSourceFullFilenames)
                {
                    string sourceFilename = Path.GetFileName(sourceFullFilename);
                    string targetFullFilename = Path.Combine(tagretDirectory, sourceFilename);
                    try
                    {
                        Logger.Trace("Copy from:" + sourceFullFilename + " to: " + targetFullFilename);
                        File.Copy(sourceFullFilename, sourceFullFilename.Replace(sourceDirectory, tagretDirectory), false);

                        if (targetNode != null) 
                            filesCutCopyPasteDrag.TreeViewFolderBrowserRefreshTreeNode(folderTreeView, targetNode);

                        databaseAndCacheMetadataExiftool.Copy(
                            Path.GetDirectoryName(sourceFullFilename), Path.GetFileName(sourceFullFilename),
                            Path.GetDirectoryName(targetFullFilename), Path.GetFileName(targetFullFilename));
                    }
                    catch (SystemException ex)
                    {
                        DateTime dateTimeLastWriteTime = DateTime.Now;
                        try
                        {
                            dateTimeLastWriteTime = File.GetLastWriteTime(sourceFullFilename);
                        }
                        catch { }
                        AddError(
                            Path.GetDirectoryName(sourceFullFilename),
                            Path.GetFileName(sourceFullFilename),
                            dateTimeLastWriteTime,
                            AddErrorFileSystemRegion, AddErrorFileSystemCopy, sourceFullFilename, targetFullFilename,
                            "Failed copying file.\r\n\r\n" +
                            "Error copy file from: " + sourceFullFilename + "\r\n\r\n" +
                            "To file: " + targetFullFilename + "\r\n\r\n" +
                            "Error message: " + ex.Message + "\r\n");
                    }
                }
            }

            //------ Update node tree -----
            GlobalData.DoNotRefreshImageListView = true;
            filesCutCopyPasteDrag.TreeViewFolderBrowserRefreshTreeNode(folderTreeView, targetNode);
            GlobalData.DoNotRefreshImageListView = false;
        }
        #endregion

    }
}
