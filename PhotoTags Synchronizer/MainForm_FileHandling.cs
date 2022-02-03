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
using MetadataLibrary;
using ImageAndMovieFileExtentions;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        #region Files In Folder Helper - GetFilesInSelectedFolderCached
        private string cachedFolder = "";
        private HashSet<FileEntry> fileEntriesFolderCached = new HashSet<FileEntry>();
        private HashSet<FileEntry> GetFilesInSelectedFolderCached()
        {

            try
            {
                string folder = GetSelectedNodeFullRealPath();
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't reach the folder. Not a valid folder selected.", "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    cachedFolder = "";
                    fileEntriesFolderCached = new HashSet<FileEntry>();
                    return fileEntriesFolderCached;
                }

                if (cachedFolder != folder) //Need updated cache
                {
                    IEnumerable<FileData> fileDatas = ImageAndMovieFileExtentionsUtility.GetFilesByEnumerableFast(folder, false);
                    HashSet<FileEntry> fileEntriesFolder = new HashSet<FileEntry>();
                    foreach (FileData fileData in fileDatas)
                    {
                        if (ImageAndMovieFileExtentionsUtility.IsMediaFormat(fileData)) fileEntriesFolder.Add(new FileEntry(fileData.Path, fileData.LastWriteTime));
                    }
                    fileEntriesFolderCached = fileEntriesFolder;
                    cachedFolder = folder;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
            }

            return fileEntriesFolderCached;
        }
        #endregion

        #region Files In Folder Helper - GetFilesInSelectedFolder
        private IEnumerable<FileData> GetFilesInSelectedFolder(string folder, bool recursive = false)
        {
            IEnumerable<FileData> fileDatas = null;
            try
            {
                if (folder == null || !Directory.Exists(folder))
                {
                    KryptonMessageBox.Show("Can't reach the folder. Not a valid folder selected.", "Invalid folder...", MessageBoxButtons.OK, MessageBoxIcon.Warning, showCtrlCopy: true);
                    return fileDatas;
                }
                fileDatas = ImageAndMovieFileExtentionsUtility.GetFilesByEnumerableFast(folder, recursive);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
            }
            return fileDatas;
        }
        #endregion

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
                    lock (keepTrackOfLoadedMetadataLock)
                    {
                        ImageListViewHandler.ImageListViewAddItem(imageListView1, targetFullFilename, ref hasTriggerLoadAllMetadataActions, ref keepTrackOfLoadedMetadata);
                    }
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

                FileStatus fileStatus = FileHandler.GetFileStatus(
                    sourceFullFilename, checkLockedStatus: true, fileInaccessibleOrError: true, fileErrorMessage: ex.Message);
                ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatus);

                FileStatus fileStatusTarget = FileHandler.GetFileStatus(
                    targetFullFilename, checkLockedStatus: true,
                    fileInaccessibleOrError: true, fileErrorMessage: ex.Message,
                    exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                ImageListView_UpdateItemFileStatusInvoke(targetFullFilename, fileStatus);

                AddError(
                    Path.GetDirectoryName(sourceFullFilename),
                    Path.GetFileName(sourceFullFilename),
                    dateTimeLastWriteTime,
                    AddErrorFileSystemRegion, AddErrorFileSystemMove, sourceFullFilename, targetFullFilename,
                    "Issue: Failed moving file.\r\n" +
                    "From File name : " + sourceFullFilename + "\r\n" +
                    "From File staus: " + fileStatus.ToString() + "\r\n" +
                    "To   File name : " + targetFullFilename + "\r\n" +
                    "To   File staus: " + fileStatusTarget.ToString() + "\r\n" +
                    "Error message: " + ex.Message);
                Logger.Error(ex, "Error when move file.");
            }
            ImageListViewResumeLayoutInvoke(imageListView1);
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;

            if (renameQueueCount == 0) 
                //To avoid selected files becomes added back to read queue, and also exist in rename queue,
                //that rename item can get removed after rename. With old name in read queue, and this file will then not exist when read
                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);

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

                        FileStatus fileStatus = FileHandler.GetFileStatus(
                            sourceFullFilename,  checkLockedStatus: true, fileInaccessibleOrError: true, fileErrorMessage: ex.Message);
                        ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatus);

                        FileStatus fileStatusTarget = FileHandler.GetFileStatus(
                            targetFullFilename, checkLockedStatus: true,
                            fileInaccessibleOrError: true, fileErrorMessage: ex.Message,
                            exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                        ImageListView_UpdateItemFileStatusInvoke(targetFullFilename, fileStatus);

                        AddError(
                            Path.GetDirectoryName(sourceFullFilename),
                            Path.GetFileName(sourceFullFilename),
                            dateTimeLastWriteTime,
                            AddErrorFileSystemRegion, AddErrorFileSystemMove, sourceFullFilename, targetFullFilename,
                            "Issue: Failed moving file.\r\n" +
                            "From File name : " + sourceFullFilename + "\r\n" +
                            "From File staus: " + fileStatus.ToString() + "\r\n" +
                            "To   File name : " + targetFullFilename + "\r\n" +
                            "To   File staus: " + fileStatusTarget.ToString() + "\r\n" +
                            "Error message: " + ex.Message);

                        Logger.Error(ex, "Error when move file.");
                    }
                }
                imageListView.ResumeLayout();
            }
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;

            ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
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

                        databaseAndCacheThumbnailPoster.Move(Path.GetDirectoryName(oldFullFilename), Path.GetFileName(oldFullFilename), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                        databaseAndCacheMetadataExiftool.Move(Path.GetDirectoryName(oldFullFilename), Path.GetFileName(oldFullFilename), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                    }
                    #endregion

                    DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectory);                    
                    string targetFullFolderName = targetDirectory + directoryInfo.Parent.Name;
                    treeViewFolderBrowser1.Populate(targetDirectory);

                }
                //----- Updated ImageListView with files ------
                ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(false, true);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error when move folder.");

                AddError(
                    sourceDirectory,
                    AddErrorFileSystemRegion, AddErrorFileSystemMoveFolder, sourceDirectory, targetDirectory,
                    "Issue: Failed moving directory.\r\n" +
                    "From Directory: " + sourceDirectory + "\r\n" +
                    "To Directory: " + targetDirectory + "\r\n" +
                    "Error message: " + ex.Message);

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

                        FileStatus fileStatusSource = FileHandler.GetFileStatus(
                            sourceFullFilename, checkLockedStatus: true, fileInaccessibleOrError: true, fileErrorMessage: ex.Message);

                        ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatusSource);

                        FileStatus fileStatusTarget = FileHandler.GetFileStatus(
                            targetFullFilename, checkLockedStatus: true);

                        AddError(
                            Path.GetDirectoryName(sourceFullFilename), Path.GetFileName(sourceFullFilename), dateTimeLastWriteTime, 
                            sourceFullFilename, targetFullFilename, AddErrorFileSystemRegion, AddErrorFileSystemCopy,
                            "Issue: Failed copying the file.\r\n" +
                            "From File Name:  " + sourceFullFilename + "\r\n" +
                            "From File Staus: " + fileStatusSource.ToString() + "\r\n" +
                            "To   File Name:  " + targetFullFilename + "\r\n" +
                            "To   File Staus: " + fileStatusTarget.ToString() + "\r\n" +
                            "Error message: " + ex.Message);
                        Logger.Error(ex, "Error when copy file.");
                    }
                }
            }

            ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
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
                    AddError(
                        dirPath, AddErrorFileSystemRegion, AddErrorFileSystemCreateFolder, dirPath.Replace(sourceDirectory, tagretDirectory), dirPath.Replace(sourceDirectory, tagretDirectory),
                        "Issue: Failed create directory\r\n" +
                        "Directory: " + dirPath + "\r\n" +
                        "Error message: " + ex.Message);
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

                        FileStatus fileStatusSource = FileHandler.GetFileStatus(
                            sourceFullFilename, checkLockedStatus: true, fileInaccessibleOrError: true, fileErrorMessage: ex.Message);
                        ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatusSource);

                        FileStatus fileStatusTarget = FileHandler.GetFileStatus(
                            targetFullFilename, checkLockedStatus: true);
                        //ImageListView_UpdateItemFileStatusInvoke(targetFullFilename, fileStatusTarget);

                        AddError(
                            Path.GetDirectoryName(sourceFullFilename), Path.GetFileName(sourceFullFilename), dateTimeLastWriteTime,
                            AddErrorFileSystemRegion, AddErrorFileSystemCopy, sourceFullFilename, targetFullFilename,
                            "Issue: Failed copying file.\r\n" +
                            "From File Name:  " + sourceFullFilename + "\r\n" +
                            "From File Staus: " + fileStatusSource.ToString() + "\r\n" +
                            "To   File Name:  " + targetFullFilename + "\r\n" +
                            "To   File Staus: " + fileStatusTarget.ToString() + "\r\n" +
                            "Error message: " + ex.Message);
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
