using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using Raccoom.Windows.Forms;
using Manina.Windows.Forms;
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

            try
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;
                //ImageListViewHandler.SuspendLayout(imageListView1);

                using (new WaitCursor())
                {
                    try
                    {
                        bool directoryCreated = filesCutCopyPasteDrag.MoveFile(sourceFullFilename, targetFullFilename);

                        if (directoryCreated)
                        {
                            GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = true;
                            TreeViewFolderBrowserHandler.RefreshFolderWithName(folderTreeView, targetFullFilename, true);
                            GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = false;
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
                        #region Error Handling

                        FileStatus fileStatus = FileHandler.GetFileStatus(
                            sourceFullFilename, checkLockedStatus: true, hasErrorOccured: true, errorMessage: ex.Message);
                        ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatus);

                        FileStatus fileStatusTarget = FileHandler.GetFileStatus(
                            targetFullFilename, checkLockedStatus: true,
                            hasErrorOccured: true, errorMessage: ex.Message,
                            exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                        ImageListView_UpdateItemFileStatusInvoke(targetFullFilename, fileStatus);

                        AddError(
                            Path.GetDirectoryName(sourceFullFilename),
                            Path.GetFileName(sourceFullFilename),
                            fileStatus.LastWrittenDateTime,
                            AddErrorFileSystemRegion, AddErrorFileSystemMove, sourceFullFilename, targetFullFilename,
                            "Issue: Failed moving file.\r\n" +
                            "From File name : " + sourceFullFilename + "\r\n" +
                            "From File staus: " + fileStatus.ToString() + "\r\n" +
                            "To   File name : " + targetFullFilename + "\r\n" +
                            "To   File staus: " + fileStatusTarget.ToString() + "\r\n" +
                            "Error message: " + ex.Message);
                        Logger.Error(ex, "Error when move file. From: " + sourceFullFilename + " to:" + targetFullFilename);
                        #endregion 
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                //ImageListViewHandler.ResumeLayout(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
            }
            if (renameQueueCount == 0) 
                //To avoid selected files becomes added back to read queue, and also exist in rename queue,
                //that rename item can get removed after rename. With old name in read queue, and this file will then not exist when read
                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);

        }
        #endregion 

        #region Move files to new folder (no rename)
        private void MoveFilesNoRename_UpdateTreeViewFolderBrowser(TreeViewFolderBrowser folderTreeView, ImageListView imageListView, StringCollection files, string targetNodeDirectory, TreeNode treeNodeTarget)
        {
            if (GlobalData.IsApplicationClosing) return;

            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<TreeViewFolderBrowser, ImageListView, StringCollection, string, TreeNode>(MoveFilesNoRename_UpdateTreeViewFolderBrowser), folderTreeView, imageListView, files, targetNodeDirectory, treeNodeTarget);
                return;
            }

            if (GlobalData.IsApplicationClosing) return;

            if (GlobalData.DoNotTrigger_ImageListView_SelectionChanged) return;

            try
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;

                ImageListViewHandler.SuspendLayout(imageListView1);

                #region Do the work
                using (new WaitCursor())
                {
                    foreach (string oldPath in files) //Move all files to target directory 
                    {
                        string sourceFullFilename = oldPath;
                        string filename = Path.GetFileName(sourceFullFilename);
                        string targetFullFilename = Path.Combine(targetNodeDirectory, filename);
                        try
                        {
                            bool directoryCreated = filesCutCopyPasteDrag.MoveFile(sourceFullFilename, targetFullFilename);

                            //------ Update node tree -----
                            GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = true;

                            if (treeNodeTarget == null)
                            {
                                string targetFolder = Path.GetDirectoryName(targetFullFilename);
                                TreeViewFolderBrowserHandler.RemoveFolderWithName(folderTreeView, targetFolder);
                            }
                            else TreeViewFolderBrowserHandler.RefreshTreeNode(folderTreeView, treeNodeTarget);

                            GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = false;

                            ImageListViewItem foundItem = ImageListViewHandler.FindItem(imageListView.Items, sourceFullFilename);
                            if (foundItem != null) ImageListViewHandler.ImageListViewRemoveItem(imageListView, foundItem);
                        }
                        catch (Exception ex)
                        {
                            FileStatus fileStatus = FileHandler.GetFileStatus(
                                sourceFullFilename, checkLockedStatus: true, hasErrorOccured: true, errorMessage: ex.Message);
                            ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatus);

                            FileStatus fileStatusTarget = FileHandler.GetFileStatus(
                                targetFullFilename, checkLockedStatus: true,
                                hasErrorOccured: true, errorMessage: ex.Message,
                                exiftoolProcessStatus: ExiftoolProcessStatus.DoNotUpdate);
                            ImageListView_UpdateItemFileStatusInvoke(targetFullFilename, fileStatus);

                            AddError(
                                Path.GetDirectoryName(sourceFullFilename),
                                Path.GetFileName(sourceFullFilename),
                                fileStatus.LastWrittenDateTime,
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
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
                ImageListViewHandler.ResumeLayout(imageListView1);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
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
                    FileData[] fileDatas = FastDirectoryEnumerator.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);

                    #region Move all folder and files
                    Logger.Trace("Move folder from:" + sourceDirectory + " to: " + targetDirectory);
                    System.IO.Directory.Move(sourceDirectory, targetDirectory);
                    #endregion

                    #region Clear ImageListView
                    ImageListViewHandler.ClearAllAndCaches(imageListView1);
                    #endregion

                    #region Update node tree
                    GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = true;
                    TreeViewFolderBrowserHandler.RefreshFolderWithName(folderTreeView, sourceDirectory, true);                    
                    TreeViewFolderBrowserHandler.RemoveFolderWithName(folderTreeView, sourceDirectory);
                    TreeViewFolderBrowserHandler.RefreshFolderWithName(folderTreeView, targetDirectory, true);
                    GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = false;
                    #endregion

                    #region Update database
                    foreach (FileData oldFileData in fileDatas)
                    {
                        string oldFilename = Path.GetFileName(oldFileData.Path);
                        string newFullFilename = Path.Combine(targetDirectory, oldFilename);
                        Logger.Trace("Rename from:" + oldFileData.Path + " to: " + newFullFilename);

                        databaseAndCacheThumbnailPoster.Move(Path.GetDirectoryName(oldFileData.Path), Path.GetFileName(oldFileData.Path), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                        databaseAndCacheMetadataExiftool.Move(Path.GetDirectoryName(oldFileData.Path), Path.GetFileName(oldFileData.Path), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                    }
                    #endregion

                    DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirectory);                    
                    string targetFullFolderName = targetDirectory + directoryInfo.Parent.Name;

                    GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = true;
                    treeViewFolderBrowser1.Populate(targetDirectory);
                    GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = false;

                }
                //----- Updated ImageListView with files ------
                ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(false, true);
                treeViewFolderBrowser1.Focus();
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
                            GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = true;
                            TreeViewFolderBrowserHandler.RefreshTreeNode(folderTreeView, targetNode);
                            GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        FileStatus fileStatusSource = FileHandler.GetFileStatus(
                            sourceFullFilename, checkLockedStatus: true, hasErrorOccured: true, errorMessage: ex.Message);

                        ImageListView_UpdateItemFileStatusInvoke(sourceFullFilename, fileStatusSource);

                        FileStatus fileStatusTarget = FileHandler.GetFileStatus(
                            targetFullFilename, checkLockedStatus: true);

                        AddError(
                            Path.GetDirectoryName(sourceFullFilename), Path.GetFileName(sourceFullFilename), fileStatusSource.LastWrittenDateTime, 
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
            IEnumerable<FileData> allSourceFileDatas = ImageAndMovieFileExtentionsUtility.GetFilesByEnumerableFast(sourceDirectory, true);

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
                foreach (FileData sourceFileData in allSourceFileDatas)
                {
                    string sourceFilename = Path.GetFileName(sourceFileData.Path);
                    string targetFullFilename = Path.Combine(tagretDirectory, sourceFilename);
                    try
                    {
                        Logger.Trace("Copy from:" + sourceFileData.Path + " to: " + targetFullFilename);
                        File.Copy(sourceFileData.Path, sourceFileData.Path.Replace(sourceDirectory, tagretDirectory), false);

                        if (targetNode != null)
                            TreeViewFolderBrowserHandler.RefreshTreeNode(folderTreeView, targetNode);

                        databaseAndCacheMetadataExiftool.Copy(
                            Path.GetDirectoryName(sourceFileData.Path), Path.GetFileName(sourceFileData.Path),
                            Path.GetDirectoryName(sourceFileData.Path), Path.GetFileName(sourceFileData.Path));
                    }
                    catch (SystemException ex)
                    {
                        FileStatus fileStatusSource = FileHandler.GetFileStatus(
                            sourceFileData.Path, checkLockedStatus: true, hasErrorOccured: true, errorMessage: ex.Message);
                        ImageListView_UpdateItemFileStatusInvoke(sourceFileData.Path, fileStatusSource);

                        FileStatus fileStatusTarget = FileHandler.GetFileStatus(
                            targetFullFilename, checkLockedStatus: true);

                        AddError(
                            Path.GetDirectoryName(sourceFileData.Path), Path.GetFileName(sourceFileData.Path), fileStatusSource.LastWrittenDateTime,
                            AddErrorFileSystemRegion, AddErrorFileSystemCopy, sourceFileData.Path, targetFullFilename,
                            "Issue: Failed copying file.\r\n" +
                            "From File Name:  " + sourceFileData.Path + "\r\n" +
                            "From File Staus: " + fileStatusSource.ToString() + "\r\n" +
                            "To   File Name:  " + targetFullFilename + "\r\n" +
                            "To   File Staus: " + fileStatusTarget.ToString() + "\r\n" +
                            "Error message: " + ex.Message);
                    }
                }
            }

            //------ Update node tree -----
            GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = true;
            TreeViewFolderBrowserHandler.RefreshTreeNode(folderTreeView, targetNode);
            GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = false;
        }
        #endregion

        #region FixOneDriveIssues 
        public List<string> FixOneDriveIssues(HashSet<FileEntry> fileEntries, out List<string> notFixed, List<string> listOfNetworkNames,
            bool fixError = false, bool moveToRecycleBin = true,
            MetadataDatabaseCache metadataDatabaseCache = null)
        {
            List<string> foundOrRemovedFiles = new List<string>();
            notFixed = new List<string>();

            foreach (string networkName in listOfNetworkNames)
            {
                string machineName = "-" + networkName;
                int machineNameLength = machineName.Length;

                foreach (FileEntry fileEntryMaybeHasMachineName in fileEntries)
                {
                    bool machineNameFound = false;

                    string filenameWithoutExtension = Path.GetFileNameWithoutExtension(fileEntryMaybeHasMachineName.FileName);
                    int indexOfMachineName = filenameWithoutExtension.IndexOf(machineName, StringComparison.OrdinalIgnoreCase);
                    if (indexOfMachineName >= 0)
                    {
                        #region Get Filename without <-MachineName-xx>
                        int charsBehindMachineName = filenameWithoutExtension.Length - indexOfMachineName - machineNameLength;

                        if (charsBehindMachineName == 0) machineNameFound = true;
                        else if (charsBehindMachineName == 1) machineNameFound = false;
                        else if (charsBehindMachineName == 2)
                        {
                            if (filenameWithoutExtension[indexOfMachineName + machineNameLength] == '-' &&
                                char.IsDigit(filenameWithoutExtension[indexOfMachineName + machineNameLength + 1])) machineNameFound = true; //numberExtraCharBehind = 2;

                        }
                        else if (charsBehindMachineName == 3)
                        {
                            if (fileEntryMaybeHasMachineName.FileFullPath[indexOfMachineName + machineNameLength] == '-' &&
                                char.IsDigit(filenameWithoutExtension[indexOfMachineName + machineNameLength + 1]) &&
                                char.IsDigit(filenameWithoutExtension[indexOfMachineName + machineNameLength + 2])) machineNameFound = true; //numberExtraCharBehind = 3;
                        }
                        else
                        {
                            if (filenameWithoutExtension.IndexOf(machineName, indexOfMachineName, StringComparison.OrdinalIgnoreCase) != -1)
                                machineNameFound = true;
                            else
                                machineNameFound = false;
                        }
                        #endregion

                        #region pathWithoutMachineName
                        string pathWithoutMachineName = filenameWithoutExtension.Substring(0, indexOfMachineName);
                        FileEntry fileEntryWithoutMachineName = new FileEntry(
                            Path.Combine(
                                Path.GetDirectoryName(fileEntryMaybeHasMachineName.FileFullPath),
                            pathWithoutMachineName + Path.GetExtension(fileEntryMaybeHasMachineName.FileFullPath)),
                            fileEntryMaybeHasMachineName.LastWriteDateTime);
                        #endregion

                        if (machineNameFound && !fixError)
                        {
                            #region Add to Found files list
                            if (FileEntry.FullFileNameExist(fileEntries, fileEntryWithoutMachineName.FileFullPath))
                            {
                                if (!foundOrRemovedFiles.Contains(fileEntryWithoutMachineName.FileFullPath)) foundOrRemovedFiles.Add(fileEntryWithoutMachineName.FileFullPath);
                                foundOrRemovedFiles.Add(fileEntryMaybeHasMachineName.FileFullPath);
                            }
                            #endregion
                        }
                        else
                        if (fixError)
                        {
                            FileEntry fileEntryFoundfileWithoutMachineName = FileEntry.FindFileEntryByFullFileName(fileEntries, fileEntryWithoutMachineName.FileFullPath);
                            if (fileEntryFoundfileWithoutMachineName != null)
                            {
                                #region Create FileEntry Broker
                                FileEntryBroker fileEntryBrokerExiftoolWithoutMachineName = new FileEntryBroker(
                                    fileEntryFoundfileWithoutMachineName.FileFullPath, FileHandler.GetLastWriteTime(fileEntryFoundfileWithoutMachineName.FileFullPath, waitAndRetry: false), MetadataBrokerType.ExifTool);
                                FileEntryBroker fileEntryBrokerSavedWithoutMachineName = new FileEntryBroker(
                                    fileEntryFoundfileWithoutMachineName.FileFullPath, DateTime.MinValue, MetadataBrokerType.UserSavedData);

                                FileEntryBroker fileEntryBrokerExiftoolHasMachineName = new FileEntryBroker(
                                    fileEntryMaybeHasMachineName.FileFullPath, FileHandler.GetLastWriteTime(fileEntryMaybeHasMachineName.FileFullPath, waitAndRetry: false), MetadataBrokerType.ExifTool);
                                FileEntryBroker fileEntryBrokerSavedHasMachineName = new FileEntryBroker(
                                    fileEntryMaybeHasMachineName.FileFullPath, DateTime.MinValue, MetadataBrokerType.UserSavedData);
                                #endregion

                                #region Read Metadata
                                Metadata metadataExiftoolWithoutMachineName = metadataDatabaseCache.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftoolWithoutMachineName);
                                Metadata metadataSavedWithoutMachineName = metadataDatabaseCache.ReadMetadataFromCacheOrDatabase(fileEntryBrokerSavedWithoutMachineName);

                                Metadata metadataExiftoolHasMachineName = metadataDatabaseCache.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftoolHasMachineName);
                                Metadata metadataSavedHasMachineName = metadataDatabaseCache.ReadMetadataFromCacheOrDatabase(fileEntryBrokerSavedHasMachineName);
                                #endregion

                                if (metadataExiftoolWithoutMachineName != null && metadataExiftoolHasMachineName != null)
                                {
                                    #region Adjust Metadata
                                    Metadata metadataExiftoolWithoutMachineNameCopy = new Metadata(metadataExiftoolWithoutMachineName);
                                    metadataExiftoolWithoutMachineNameCopy.FileName = "";
                                    metadataExiftoolWithoutMachineNameCopy.FileDirectory = "";
                                    metadataExiftoolWithoutMachineNameCopy.Broker = MetadataBrokerType.Empty;
                                    metadataExiftoolWithoutMachineNameCopy.FileDateModified = DateTime.MinValue;
                                    metadataExiftoolWithoutMachineNameCopy.FileDateAccessed = DateTime.MinValue;
                                    metadataExiftoolWithoutMachineNameCopy.FileSize = 0;

                                    Metadata metadataExiftoolHasMachineNameCopy = new Metadata(metadataExiftoolHasMachineName);
                                    metadataExiftoolHasMachineNameCopy.FileName = "";
                                    metadataExiftoolHasMachineNameCopy.FileDirectory = "";
                                    metadataExiftoolHasMachineNameCopy.Broker = MetadataBrokerType.Empty;
                                    metadataExiftoolHasMachineNameCopy.FileDateModified = DateTime.MinValue;
                                    metadataExiftoolHasMachineNameCopy.FileDateAccessed = DateTime.MinValue;
                                    metadataExiftoolHasMachineNameCopy.FileSize = 0;

                                    Metadata metadataSavedWithoutMachineNameCopy = null;
                                    if (metadataSavedWithoutMachineName != null)
                                    {
                                        metadataSavedWithoutMachineNameCopy = new Metadata(metadataSavedWithoutMachineName);
                                        metadataSavedWithoutMachineNameCopy.FileName = "";
                                        metadataSavedWithoutMachineNameCopy.FileDirectory = "";
                                        metadataSavedWithoutMachineNameCopy.Broker = MetadataBrokerType.Empty;
                                        metadataSavedWithoutMachineNameCopy.FileDateModified = DateTime.MinValue;
                                        metadataSavedWithoutMachineNameCopy.FileDateAccessed = DateTime.MinValue;
                                        metadataSavedWithoutMachineNameCopy.FileSize = 0;
                                    }

                                    Metadata metadataSavedHasMachineNameCopy = null;
                                    if (metadataSavedHasMachineName != null)
                                    {
                                        metadataSavedHasMachineNameCopy = new Metadata(metadataSavedHasMachineName);
                                        metadataSavedHasMachineNameCopy.FileName = "";
                                        metadataSavedHasMachineNameCopy.FileDirectory = "";
                                        metadataSavedHasMachineNameCopy.Broker = MetadataBrokerType.Empty;
                                        metadataSavedHasMachineNameCopy.FileDateModified = DateTime.MinValue;
                                        metadataSavedHasMachineNameCopy.FileDateAccessed = DateTime.MinValue;
                                        metadataSavedHasMachineNameCopy.FileSize = 0;
                                    }
                                    #endregion

                                    bool winnerHasMachineName = false;
                                    bool winnerWithoutMachineName = false;

                                    #region Find a winner
                                    //Without Machine Name - Exifdata == Saved data???
                                    if (metadataExiftoolWithoutMachineNameCopy == metadataSavedWithoutMachineNameCopy) winnerWithoutMachineName = true;
                                    if (metadataExiftoolWithoutMachineNameCopy == metadataSavedHasMachineNameCopy) winnerWithoutMachineName = true;

                                    //--Has-- Machine Name - Exifdata == Saved data???
                                    if (metadataExiftoolHasMachineNameCopy == metadataSavedHasMachineNameCopy) winnerHasMachineName = true;
                                    if (metadataExiftoolHasMachineNameCopy == metadataSavedWithoutMachineNameCopy) winnerHasMachineName = true;



                                    //Both version is Equal
                                    if (metadataExiftoolHasMachineNameCopy == metadataExiftoolWithoutMachineNameCopy)
                                    {
                                        winnerHasMachineName = true;
                                        winnerWithoutMachineName = true;
                                    }
                                    else
                                    {
                                        if (winnerHasMachineName && winnerWithoutMachineName)
                                        {
                                            winnerHasMachineName = false;
                                            winnerWithoutMachineName = false;
                                        }
                                    }

                                    //If no winner, Find a winner, even when has error
                                    if (!winnerHasMachineName && !winnerWithoutMachineName)
                                    {
                                        if (metadataExiftoolWithoutMachineNameCopy != null && metadataSavedWithoutMachineNameCopy == null &&
                                            metadataExiftoolHasMachineNameCopy != null && metadataSavedHasMachineNameCopy != null) winnerHasMachineName = true;

                                        if (metadataExiftoolWithoutMachineNameCopy != null && metadataSavedWithoutMachineNameCopy != null &&
                                            metadataExiftoolHasMachineNameCopy != null && metadataSavedHasMachineNameCopy == null) winnerWithoutMachineName = true;
                                    }

                                    #endregion

                                    #region Delete loser

                                    if (winnerHasMachineName && winnerWithoutMachineName)
                                    {
                                        #region Both are Winner, Keep newest
                                        try
                                        {
                                            DateTime dateTimeWithoutMachineName = fileEntryWithoutMachineName.LastWriteDateTime;
                                            DateTime dateTimeHasMachineName = fileEntryMaybeHasMachineName.LastWriteDateTime;

                                            if (dateTimeHasMachineName > dateTimeWithoutMachineName)
                                            {
                                                try
                                                {
                                                    foundOrRemovedFiles.Add(fileEntryWithoutMachineName.FileFullPath);
                                                    filesCutCopyPasteDrag.DeleteFileAndHistory(fileEntryWithoutMachineName.FileFullPath);
                                                    filesCutCopyPasteDrag.MoveFile(fileEntryMaybeHasMachineName.FileFullPath, fileEntryWithoutMachineName.FileFullPath);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Logger.Error(ex);
                                                    KryptonMessageBox.Show(ex.Message + "\r\nWas trying to replace\r\n" + fileEntryWithoutMachineName.FileFullPath + "\r\n with\r\n" + fileEntryMaybeHasMachineName.FileFullPath,
                                                        "Was not able to remove duplicated file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                                }
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    foundOrRemovedFiles.Add(fileEntryMaybeHasMachineName.FileFullPath);
                                                    filesCutCopyPasteDrag.DeleteFile(fileEntryMaybeHasMachineName.FileFullPath, moveToRecycleBin);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Logger.Error(ex);
                                                    KryptonMessageBox.Show(ex.Message + "\r\n" + fileEntryMaybeHasMachineName.FileFullPath,
                                                        "Was not able to remove dubpliacted file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Error(ex);
                                            KryptonMessageBox.Show(ex.Message + "\r\n" + fileEntryWithoutMachineName.FileFullPath + "\r\n" + fileEntryMaybeHasMachineName.FileFullPath,
                                                "Was not able to remove the oldest of dubpliacted file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                        }
                                        #endregion
                                    }
                                    else if (winnerHasMachineName && !winnerWithoutMachineName)
                                    {
                                        #region HasMachineName wins, remove "original", replace with Other file
                                        try
                                        {
                                            foundOrRemovedFiles.Add(fileEntryWithoutMachineName.FileFullPath);                                            
                                            filesCutCopyPasteDrag.DeleteFile(fileEntryWithoutMachineName.FileFullPath);
                                            filesCutCopyPasteDrag.MoveFile(fileEntryMaybeHasMachineName.FileFullPath, fileEntryWithoutMachineName.FileFullPath);
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Error(ex);
                                            KryptonMessageBox.Show(ex.Message + "\r\nWas trying to replace\r\n" + fileEntryWithoutMachineName.FileFullPath + "\r\n with\r\n" + fileEntryMaybeHasMachineName.FileFullPath,
                                                "Was not able to remove duplicated file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                        }
                                        #endregion
                                    }
                                    else if (!winnerHasMachineName && winnerWithoutMachineName)
                                    {
                                        #region "Original" wins, delete "With Machine Name"
                                        try
                                        {
                                            foundOrRemovedFiles.Add(fileEntryMaybeHasMachineName.FileFullPath);
                                            filesCutCopyPasteDrag.DeleteFile(fileEntryMaybeHasMachineName.FileFullPath, moveToRecycleBin);
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.Error(ex);
                                            KryptonMessageBox.Show(ex.Message + "\r\n" + fileEntryMaybeHasMachineName.FileFullPath,
                                                "Was not able to remove dubpliacted file.", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region Report Not fixed
                                        notFixed.Add(fileEntryBrokerExiftoolWithoutMachineName.FileFullPath);
                                        notFixed.Add(fileEntryMaybeHasMachineName.FileFullPath);
                                        #endregion 
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //DEBUG - Didn't find original
                                }
                            }
                        }
                    }

                }
            }
            return foundOrRemovedFiles;
        }
        #endregion
    }
}
