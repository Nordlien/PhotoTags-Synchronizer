using Exiftool;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Thumbnails;
using static Manina.Windows.Forms.ImageListView;
using Raccoom.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public class FilesCutCopyPasteDrag
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private MetadataDatabaseCache databaseAndCacheMetadataExiftool;
        private MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery;
        private MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos;
        private ThumbnailDatabaseCache databaseAndCacheThumbnail;
        private ExiftoolDataDatabase databaseExiftoolData;
        private ExiftoolWarningDatabase databaseExiftoolWarning;

        #region TreeViewFolderBrowser - Remove TreeNode
        public void TreeViewFolderBrowserRemoveTreeNode(TreeViewFolderBrowser folderTreeViewFolder, TreeNode treeNode)
        {
            if (treeNode != null)
            {
                TreeNodePath node = (TreeNodePath)treeNode;
                Raccoom.Win32.ShellItem folderItem = ((Raccoom.Win32.ShellItem)node.Tag);
                folderItem.ClearFolders();
                node.Remove();
            }
        }
        #endregion

        #region TreeViewFolderBrowser - Refresh TreeNode
        public void TreeViewFolderBrowserRefreshTreeNode(TreeViewFolderBrowser folderTreeViewFolder, TreeNode treeNode)
        {
            if (treeNode != null)
            {
                
                TreeNodePath node = (TreeNodePath)treeNode;
                Raccoom.Win32.ShellItem folderItem = ((Raccoom.Win32.ShellItem)node.Tag);
                folderItem.ClearFolders();
                node.Refresh();
                folderTreeViewFolder.UseWaitCursor = true;
                folderTreeViewFolder.BeginUpdate();
                treeNode.Collapse();
                treeNode.Expand();
                folderTreeViewFolder.EndUpdate();
                folderTreeViewFolder.UseWaitCursor = false;
            }
        }
        #endregion

        #region
        public void TreeViewFOlderBrowserRefreshFolderWithName(TreeViewFolderBrowser folderTreeViewFolder, string folder)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            TreeViewFolderBrowserFindAllNodesRecursive(folderTreeViewFolder.Nodes, folder, ref treeNodes);
            foreach (TreeNode treeNode in treeNodes) TreeViewFolderBrowserRefreshTreeNode(folderTreeViewFolder, treeNode);
        }
        #endregion

        #region TreeViewFolderBrowser - FindAllNodes
        public List<TreeNode> TreeViewFolderBrowserFindAllNodes(TreeNodeCollection treeNodeCollection, string directory)
        {
            List<TreeNode> treeNodeFound = new List<TreeNode>();
            TreeViewFolderBrowserFindAllNodesRecursive(treeNodeCollection, directory, ref treeNodeFound);
            return treeNodeFound;
        }

        public void TreeViewFolderBrowserFindAllNodesRecursive(TreeNodeCollection treeNodeCollection, string directory, ref List<TreeNode> treeNodeFound)
        {
            foreach (TreeNode treeNodeSearch in treeNodeCollection)
            {
                TreeNodePath treeNodePath = (TreeNodePath)treeNodeSearch;
                if (treeNodePath.Path == directory)
                {
                    treeNodeFound.Add(treeNodeSearch);
                }
                if (treeNodeSearch.Nodes != null) TreeViewFolderBrowserFindAllNodesRecursive(treeNodeSearch.Nodes, directory, ref treeNodeFound);
            }
        }
        #endregion

        #region FilesCutCopyPasteDrag - Constructor
        public FilesCutCopyPasteDrag(MetadataDatabaseCache databaseAndCacheMetadataExiftool, 
            MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery, 
            MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos, 
            ThumbnailDatabaseCache databaseAndCacheThumbnail, 
            ExiftoolDataDatabase databaseExiftoolData, 
            ExiftoolWarningDatabase databaseExiftoolWarning)
        {
            this.databaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool ?? throw new ArgumentNullException(nameof(databaseAndCacheMetadataExiftool));
            this.databaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery ?? throw new ArgumentNullException(nameof(databaseAndCacheMetadataWindowsLivePhotoGallery));
            this.databaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos ?? throw new ArgumentNullException(nameof(databaseAndCacheMetadataMicrosoftPhotos));
            this.databaseAndCacheThumbnail = databaseAndCacheThumbnail ?? throw new ArgumentNullException(nameof(databaseAndCacheThumbnail));
            this.databaseExiftoolData = databaseExiftoolData ?? throw new ArgumentNullException(nameof(databaseExiftoolData));
            this.databaseExiftoolWarning = databaseExiftoolWarning ?? throw new ArgumentNullException(nameof(databaseExiftoolWarning));
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteDirectoryAndHistory
        public static int DeleteDirectoryAndHistorySize = 6;
        public int DeleteDirectoryAndHistory(ref int queueSize, string folder)
        {
            int rowsAffected = 0;
            
            rowsAffected += databaseAndCacheMetadataExiftool.DeleteDirectoryAndHistory(MetadataBrokerType.ExifTool, folder); //Also delete When (Broker & @Broker) = @Broker
            queueSize--; //1

            rowsAffected += databaseAndCacheMetadataMicrosoftPhotos.DeleteDirectoryAndHistory(MetadataBrokerType.MicrosoftPhotos, folder);
            queueSize--; //2

            rowsAffected += databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteDirectoryAndHistory(MetadataBrokerType.WindowsLivePhotoGallery, folder);
            queueSize--; //3

            rowsAffected += databaseExiftoolData.DeleteDirectoryAndHistory(folder);
            queueSize--; //4

            rowsAffected += databaseExiftoolWarning.DeleteDirectoryAndHistory(folder);
            queueSize--; //5

            rowsAffected += databaseAndCacheThumbnail.DeleteDirectoryAndHistory(folder);
            queueSize--; //6
            return rowsAffected;
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteFileEntry
        public void DeleteFileEntries(List<FileEntry> fileEntries)
        {
            List<FileEntryBroker> fileEntryBrokersExifTool = new List<FileEntryBroker>();
            List<FileEntryBroker> fileEntryBrokersMicrosoftPhotos = new List<FileEntryBroker>();
            List<FileEntryBroker> fileEntryBrokersWindowsPhotoGallary = new List<FileEntryBroker>();

            foreach (FileEntry fileEntry in fileEntries)
            {
                fileEntryBrokersExifTool.Add(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                fileEntryBrokersMicrosoftPhotos.Add(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos));
                fileEntryBrokersWindowsPhotoGallary.Add(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery));
            }

            databaseAndCacheMetadataExiftool.MetadataCacheRemove(fileEntryBrokersExifTool);
            databaseAndCacheMetadataExiftool.DeleteFileEntries(fileEntryBrokersExifTool);  //Also delete When (Broker & @Broker) = @Broker

            databaseAndCacheMetadataMicrosoftPhotos.MetadataCacheRemove(fileEntryBrokersMicrosoftPhotos);
            databaseAndCacheMetadataMicrosoftPhotos.DeleteFileEntries(fileEntryBrokersMicrosoftPhotos);

            databaseAndCacheMetadataWindowsLivePhotoGallery.MetadataCacheRemove(fileEntryBrokersWindowsPhotoGallary);
            databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteFileEntries(fileEntryBrokersWindowsPhotoGallary);

            databaseExiftoolData.DeleteFileEntriesFromMediaExiftoolTags(fileEntries);
            databaseExiftoolWarning.DeleteFileEntriesFromMediaExiftoolTagsWarning(fileEntries);
            databaseAndCacheThumbnail.DeleteThumbnails(fileEntries);
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteFileAndHistory
        public void DeleteFileAndHistory(string fullFilePath)
        {
            List<FileEntryBroker> fileEntryBrokers = databaseAndCacheMetadataExiftool.ListFileEntryBrokerDateVersions(MetadataBrokerType.ExifTool, fullFilePath);

            databaseAndCacheMetadataExiftool.MetadataCacheRemove(fileEntryBrokers);
            databaseAndCacheMetadataExiftool.DeleteFileEntries(fileEntryBrokers);

            fileEntryBrokers = databaseAndCacheMetadataMicrosoftPhotos.ListFileEntryBrokerDateVersions(MetadataBrokerType.MicrosoftPhotos, fullFilePath);
            databaseAndCacheMetadataMicrosoftPhotos.MetadataCacheRemove(fileEntryBrokers);
            databaseAndCacheMetadataMicrosoftPhotos.DeleteFileEntries(fileEntryBrokers);

            fileEntryBrokers = databaseAndCacheMetadataWindowsLivePhotoGallery.ListFileEntryBrokerDateVersions(MetadataBrokerType.WindowsLivePhotoGallery, fullFilePath);
            databaseAndCacheMetadataWindowsLivePhotoGallery.MetadataCacheRemove(fileEntryBrokers);
            databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteFileEntries(fileEntryBrokers);

            List<FileEntryAttribute> fileEntryAttributes;
            List<FileEntry> fileEntrys = new List<FileEntry>();

            fileEntryAttributes = databaseExiftoolData.ListFileEntryDateVersions(fullFilePath);
            fileEntrys.Clear();
            foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes) fileEntrys.Add(fileEntryAttribute);
            databaseExiftoolData.DeleteFileEntriesFromMediaExiftoolTags(fileEntrys);
            
            fileEntryAttributes = databaseExiftoolWarning.ListFileEntryDateVersions(fullFilePath);
            fileEntrys.Clear();
            foreach (FileEntryAttribute fileEntryAttribute in fileEntryAttributes) fileEntrys.Add(fileEntryAttribute);
            databaseExiftoolWarning.DeleteFileEntriesFromMediaExiftoolTagsWarning(fileEntrys);

            List<FileEntry> fileEntries = databaseAndCacheThumbnail.ListFileEntryDateVersions(fullFilePath);
            databaseAndCacheThumbnail.DeleteThumbnails(fileEntries);
            
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteSelectedFiles
        public void DeleteSelectedFiles(MainForm mainForm, ImageListView imageListView)
        {

            GlobalData.IsPopulatingImageListView = true;
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;

            imageListView.SuspendLayout();

            foreach (ImageListViewItem imageListViewItem in imageListView.SelectedItems)
            {
                try
                {
                    mainForm.UpdateStatusAction("Deleting the file " + imageListViewItem.FileFullPath + " and records in database");
                    File.Delete(imageListViewItem.FileFullPath);
                    this.DeleteFileAndHistory(imageListViewItem.FileFullPath);
                    imageListView.Items.Remove(imageListViewItem);
                }
                catch
                {
                    KryptonMessageBox.Show("Was not able to delete the file: " + imageListViewItem.FileFullPath, "Deleting file failed", MessageBoxButtons.OK);
                }
            }

            imageListView.ResumeLayout();

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            GlobalData.IsPopulatingImageListView = false;
  
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteFilesInFolder
        public int DeleteFilesInFolder(MainForm mainForm, TreeViewFolderBrowser folderTreeViewFolder, string folder)
        {
            string[] dirs = Directory.GetDirectories(folder + (folder.EndsWith(@"\") ? "" : @"\"), "*", SearchOption.AllDirectories);

            Directory.Delete(folder, true);

            int recordAffected = 0;
            GlobalData.ProcessCounterDelete = (dirs.Length + 1) * FilesCutCopyPasteDrag.DeleteDirectoryAndHistorySize;
            
            foreach (string directory in dirs)
            {
                mainForm.UpdateStatusAction("Delete all data and files from folder: " + directory);
                recordAffected += this.DeleteDirectoryAndHistory(ref GlobalData.ProcessCounterDelete, directory);
            }
            mainForm.UpdateStatusAction("Delete all data and files from folder: " + folder);
            recordAffected += this.DeleteDirectoryAndHistory(ref GlobalData.ProcessCounterDelete, folder);
            GlobalData.ProcessCounterDelete = 0;

            TreeNode selectedNode = folderTreeViewFolder.SelectedNode;
            TreeNode parentNode = folderTreeViewFolder.SelectedNode.Parent;

            #region Update Node in TreeView
            GlobalData.DoNotRefreshImageListView = true;
            
            TreeViewFolderBrowserRemoveTreeNode (folderTreeViewFolder, selectedNode);
            if (parentNode != null)
            {
                TreeViewFolderBrowserRefreshTreeNode(folderTreeViewFolder, parentNode);
            }
            GlobalData.DoNotRefreshImageListView = false;
            #endregion

            return recordAffected;
        }
        #endregion

        #region FilesCutCopyPasteDrag - ImageListViewReload
        public void ImageListViewReload(ImageListViewItemCollection itemCollection, bool updatedOnlySelected)
        {            
            foreach (ImageListViewItem item in itemCollection)
            {
                if (!updatedOnlySelected || (updatedOnlySelected && item.Selected)) item.Update();                
            }
        }
        #endregion 

        #region FilesCutCopyPasteDrag - DeleteSelectedFilesBeforeReload
        public List<FileEntry> DeleteFileEntriesBeforeReload(ImageListViewItemCollection itemCollection, bool updatedOnlySelected)
        {
            List<FileEntry> fileEntries = new List<FileEntry>();
            foreach (ImageListViewItem item in itemCollection)
            {
                if (!updatedOnlySelected || (updatedOnlySelected && item.Selected)) fileEntries.Add(new FileEntry(item.FileFullPath, item.DateModified));
            }

            this.DeleteFileEntries(fileEntries);
            return fileEntries;
        }
        #endregion

        #region FilesCutCopyPasteDrag - ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory
        public void ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(MainForm mainForm, TreeViewFolderBrowser folderTreeViewFolder, ImageListView imageListView)
        {
            if (GlobalData.IsPopulatingAnything()) return;

            GlobalData.IsPopulatingButtonAction = true;

            folderTreeViewFolder.Enabled = false;
            imageListView.Enabled = false;
            imageListView.SuspendLayout();

            GlobalData.ProcessCounterDelete = imageListView.SelectedItems.Count;             
            foreach (ImageListViewItem imageListViewItem in imageListView.SelectedItems)
            {
                mainForm.UpdateStatusAction("Refreshing database for " + imageListViewItem.FileFullPath);
                this.DeleteFileAndHistory(imageListViewItem.FileFullPath);
                GlobalData.ProcessCounterDelete--;
            }
            GlobalData.ProcessCounterDelete = 0;

            GlobalData.ProcessCounterRefresh = imageListView.SelectedItems.Count;
            foreach (ImageListViewItem item in imageListView.SelectedItems)
            {
                item.Update();
                item.Selected = true;
                GlobalData.ProcessCounterRefresh--;
            }
            GlobalData.ProcessCounterRefresh = 0;


            folderTreeViewFolder.Enabled = true;

            GlobalData.IsPopulatingButtonAction = false;

            imageListView.ResumeLayout();
            imageListView.Enabled = true;
        }
        #endregion

        #region FilesCutCopyPasteDrag - MoveFile
        public bool MoveFile(string sourceFullFilename, string targetFullFilename)
        {
            bool directoryCreated = false;
            if (File.Exists(sourceFullFilename))
            {    
                
                string oldFilename = Path.GetFileName(sourceFullFilename);
                string oldDirectory = Path.GetDirectoryName(sourceFullFilename);

                string newFilename = Path.GetFileName(targetFullFilename);
                string newDirectory = Path.GetDirectoryName(targetFullFilename);

                if (!Directory.Exists(newDirectory))
                {
                    Directory.CreateDirectory(newDirectory);
                    directoryCreated = true;
                }

                File.Move(sourceFullFilename, targetFullFilename);
                if (!databaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename))
                {
                    if (KryptonMessageBox.Show(
                        "Already exist a file in the database with same name: " + Path.Combine(newDirectory, newFilename) + "\r\n" +
                        "This can occure after manully moving files outside application\r\n\r\n" +
                        "Do you want do delete the old histoty and replace with new file?", 
                        "Can move content in database!", 
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DeleteFileAndHistory(Path.Combine(newDirectory, newFilename));
                        databaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename);
                    } 
                }
            }
            return directoryCreated;
        }
        #endregion

        #region FilesCutCopyPasteDrag - RenameFile
        public void RenameFile(string oldFullFilename, string newFullFilename, ref Dictionary<string, string> renameSuccess, ref Dictionary<string, string> renameFailed)
        {
            try
            {
                MoveFile(oldFullFilename, newFullFilename);
                if (renameSuccess != null) renameSuccess.Add(oldFullFilename, newFullFilename);
            }
            catch (Exception ex)
            {
                if (renameFailed != null) renameFailed.Add(oldFullFilename, newFullFilename);
                Logger.Error("Rename file failed: " + oldFullFilename + " to :" + newFullFilename + " " + ex.Message);
            }
        }
        #endregion

        #region FilesCutCopyPasteDrag - CopyFile
        public bool CopyFile(string sourceFullFilename, string targetFullFilename)
        {
            bool directoryCreated = false;
            if (File.Exists(sourceFullFilename))
            {
                string oldFilename = Path.GetFileName(sourceFullFilename);
                string oldDirectory = Path.GetDirectoryName(sourceFullFilename);

                string newFilename = Path.GetFileName(targetFullFilename);
                string newDirectory = Path.GetDirectoryName(targetFullFilename);

                if (!Directory.Exists(newDirectory))
                {
                    Directory.CreateDirectory(newDirectory);
                    directoryCreated = true;
                }

                File.Copy(sourceFullFilename, targetFullFilename);                
                File.SetCreationTime(targetFullFilename, File.GetCreationTime(sourceFullFilename));
                databaseAndCacheMetadataExiftool.Copy(oldDirectory, oldFilename, newDirectory, newFilename);
            }
            return directoryCreated;
        }
        #endregion
    }
}
