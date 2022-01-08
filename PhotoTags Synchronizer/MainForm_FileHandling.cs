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
        private void RenameFile_Thread_UpdateTreeViewFolderBrowser(TreeViewFolderBrowser folderTreeView, ImageListView imageListView, int renameQueueCount, string sourceFullFilename, string targetFullFilename)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<TreeViewFolderBrowser, ImageListView, int, string, string>(RenameFile_Thread_UpdateTreeViewFolderBrowser), folderTreeView, imageListView, renameQueueCount, sourceFullFilename, targetFullFilename);
                return;
            }

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            ImageListViewSuspendLayoutInvoke(imageListView1);

            try
            {
                bool isFileUnLockedAndExist = FileHandler.WaitLockedFileToBecomeUnlocked(sourceFullFilename, true, this);
                bool directoryCreated = filesCutCopyPasteDrag.MoveFile(sourceFullFilename, targetFullFilename);

                if (directoryCreated)
                {
                    GlobalData.DoNotRefreshImageListView = true;
                    TreeViewFolderBrowserHandler.RefreshFolderWithName(folderTreeView, targetFullFilename, true);
                    GlobalData.DoNotRefreshImageListView = false;
                }

                ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView.Items, sourceFullFilename);
                if (foundItem != null)
                {
                    ImageListViewHandler.ImageListViewRemoveItem(imageListView, foundItem);

                    #region Add new renames back to list
                    ImageListViewHandler.ImageListViewAddItem(imageListView, targetFullFilename);
                    #endregion

                    #region Select back all Items renamed
                    foundItem = ImageListViewHandler.FindItem(imageListView.Items, targetFullFilename);
                    if (foundItem != null) foundItem.Selected = true;
                    #endregion
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

                FileStatus fileStatus = FileHandler.GetFileStatus(sourceFullFilename, checkLockedStatus: true);
                ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatus);

                FileStatus fileStatusTarget = FileHandler.GetFileStatus(targetFullFilename, checkLockedStatus: true);
                ImageListView_UpdateItemFileStatusInvoke(targetFullFilename, fileStatus);

                AddError(
                    Path.GetDirectoryName(sourceFullFilename),
                    Path.GetFileName(sourceFullFilename),
                    dateTimeLastWriteTime,
                    AddErrorFileSystemRegion, AddErrorFileSystemMove, sourceFullFilename, targetFullFilename,
                    "Failed moving file.\r\n\r\n" +
                    "From:" + sourceFullFilename + "\r\n\r\n" +
                    "To: " + targetFullFilename + "\r\n\r\n" +
                    "Error message: " + ex.Message + "\r\n" +
                    "File staus:" + sourceFullFilename + "\r\n" + fileStatus.ToString() +
                    "File staus:" + targetFullFilename + "\r\n" + fileStatusTarget.ToString());
                Logger.Error(ex, "Error when move file.");
            }
            ImageListViewResumeLayoutInvoke(imageListView1);
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;

            if (renameQueueCount == 0) 
                //To avoid selected files becomes added back to read queue, and also exist in rename queue,
                //that rename item can get removed after rename. With old name in read queue, and this file will then not exist when read
                OnImageListViewSelect_FilesSelectedOrNoneSelected(false);

        }
        #endregion 

        #region Move files to new folder (no rename)
        private void MoveFilesNoRename_UpdateTreeViewFolderBrowser(TreeViewFolderBrowser folderTreeView, ImageListView imageListView, StringCollection files, string targetNodeDirectory, TreeNode treeNodeTarget)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<TreeViewFolderBrowser, ImageListView, StringCollection, string, TreeNode>(MoveFilesNoRename_UpdateTreeViewFolderBrowser), folderTreeView, imageListView, files, targetNodeDirectory, treeNodeTarget);
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

                        if (treeNodeTarget == null)
                        {
                            string targetFolder = Path.GetDirectoryName(targetFullFilename);
                            TreeViewFolderBrowserHandler.RemoveFolderWithName(folderTreeView, targetFolder);
                        }
                        else TreeViewFolderBrowserHandler.RefreshTreeNode(folderTreeView, treeNodeTarget);

                        GlobalData.DoNotRefreshImageListView = false;

                        ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView.Items, sourceFullFilename);
                        if (foundItem != null) ImageListViewHandler.ImageListViewRemoveItem(imageListView, foundItem);
                    }
                    catch (Exception ex)
                    {

                        DateTime dateTimeLastWriteTime = DateTime.Now;
                        try
                        {
                            dateTimeLastWriteTime = File.GetLastWriteTime(sourceFullFilename);
                        }
                        catch { }

                        FileStatus fileStatus = FileHandler.GetFileStatus(sourceFullFilename, checkLockedStatus: true);
                        ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatus);

                        FileStatus fileStatusTarget = FileHandler.GetFileStatus(targetFullFilename, checkLockedStatus: true);
                        ImageListView_UpdateItemFileStatusInvoke(targetFullFilename, fileStatus);

                        AddError(
                            Path.GetDirectoryName(sourceFullFilename),
                            Path.GetFileName(sourceFullFilename),
                            dateTimeLastWriteTime,
                            AddErrorFileSystemRegion, AddErrorFileSystemMove, sourceFullFilename, targetFullFilename,
                            "Failed moving file.\r\n\r\n" +
                            "From:" + sourceFullFilename + "\r\n\r\n" +
                            "To: " + targetFullFilename + "\r\n\r\n" +
                            "Error message: " + ex.Message + "\r\n" +
                            "File staus:" + sourceFullFilename + "\r\n" + fileStatus.ToString() +
                            "File staus:" + targetFullFilename + "\r\n" + fileStatusTarget.ToString());
                        Logger.Error(ex, "Error when move file.");
                    }
                }
                imageListView.ResumeLayout();
            }
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;

            OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
        }
        #endregion

        #region Move Folder
        private void MoveFolder_UpdateTreeViewFolderBrowser(TreeViewFolderBrowser folderTreeView, string sourceDirectory, string targetDirectory, TreeNode targetNode)
        {
            if (sourceDirectory == targetDirectory) return; //Can't move into itself. No need for error message

            try
            {
                using (new WaitCursor())
                {
                    string[] allSourceFullFilenames = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);

                    #region Move all folder and files
                    Logger.Trace("Move folder from:" + sourceDirectory + " to: " + targetDirectory);
                    System.IO.Directory.Move(sourceDirectory, targetDirectory);
                    #endregion

                    #region Clear ImageListView
                    ImageListViewHandler.ClearAllAndCaches(imageListView1);
                    #endregion

                    #region Update node tree
                    GlobalData.DoNotRefreshImageListView = true;
                    TreeViewFolderBrowserHandler.RefreshFolderWithName(folderTreeView, sourceDirectory, true);                    
                    TreeViewFolderBrowserHandler.RemoveFolderWithName(folderTreeView, sourceDirectory);
                    TreeViewFolderBrowserHandler.RefreshFolderWithName(folderTreeView, targetDirectory, true);
                    GlobalData.DoNotRefreshImageListView = false;
                    #endregion

                    #region Update database
                    foreach (string oldFullFilename in allSourceFullFilenames)
                    {
                        string oldFilename = Path.GetFileName(oldFullFilename);
                        string newFullFilename = Path.Combine(targetDirectory, oldFilename);
                        Logger.Trace("Rename from:" + oldFullFilename + " to: " + newFullFilename);

                        databaseAndCacheThumbnail.Move(Path.GetDirectoryName(oldFullFilename), Path.GetFileName(oldFullFilename), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                        databaseAndCacheMetadataExiftool.Move(Path.GetDirectoryName(oldFullFilename), Path.GetFileName(oldFullFilename), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                    }
                    #endregion

                    DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectory);                    
                    string targetFullFolderName = targetDirectory + directoryInfo.Parent.Name;
                    treeViewFolderBrowser1.Populate(targetDirectory);

                }
                //----- Updated ImageListView with files ------
                ImageListView_Aggregate_FromFolder(false, true);

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
                            TreeViewFolderBrowserHandler.RefreshTreeNode(folderTreeView, targetNode);
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

                        FileStatus fileStatus = FileHandler.GetFileStatus(sourceFullFilename, checkLockedStatus: true);
                        ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatus);

                        AddError(
                            Path.GetDirectoryName(sourceFullFilename),
                            Path.GetFileName(sourceFullFilename),
                            dateTimeLastWriteTime, sourceFullFilename, targetFullFilename,
                            AddErrorFileSystemRegion, AddErrorFileSystemCopy,
                            "Failed copying file.\r\n\r\n" +
                            "Error copy file from: " + sourceFullFilename + "\r\n\r\n" +
                            "To file: " + targetFullFilename + "\r\n\r\n" +
                            "Error message: " + ex.Message + "\r\n" +
                            "File staus:" + sourceFullFilename + "\r\n" + fileStatus.ToString());
                        Logger.Error(ex, "Error when copy file.");
                    }
                }
            }

            OnImageListViewSelect_FilesSelectedOrNoneSelected(false);
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
                            TreeViewFolderBrowserHandler.RefreshTreeNode(folderTreeView, targetNode);

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

                        FileStatus fileStatus = FileHandler.GetFileStatus(sourceFullFilename, checkLockedStatus: true);
                        ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatus);

                        FileStatus fileStatusTarger = FileHandler.GetFileStatus(targetFullFilename, checkLockedStatus: true);
                        ImageListView_UpdateItemFileStatusInvoke(targetFullFilename, fileStatus);

                        AddError(
                            Path.GetDirectoryName(sourceFullFilename),
                            Path.GetFileName(sourceFullFilename),
                            dateTimeLastWriteTime,
                            AddErrorFileSystemRegion, AddErrorFileSystemCopy, sourceFullFilename, targetFullFilename,
                            "Failed copying file.\r\n\r\n" +
                            "Error copy file from: " + sourceFullFilename + "\r\n\r\n" +
                            "To file: " + targetFullFilename + "\r\n\r\n" +
                            "Error message: " + ex.Message + "\r\n" +
                            "File staus:" + sourceFullFilename + "\r\n" + fileStatus.ToString() +
                            "File staus:" + targetFullFilename + "\r\n" + fileStatusTarger.ToString());
                    }
                }
            }

            //------ Update node tree -----
            GlobalData.DoNotRefreshImageListView = true;
            TreeViewFolderBrowserHandler.RefreshTreeNode(folderTreeView, targetNode);
            GlobalData.DoNotRefreshImageListView = false;
        }
        #endregion

    }
}
