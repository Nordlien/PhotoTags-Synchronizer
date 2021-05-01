using Exiftool;
using Furty.Windows.Forms;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Thumbnails;
using static Manina.Windows.Forms.ImageListView;

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

        #region FilesCutCopyPasteDrag - DeleteDirectory
        public void DeleteDirectory(string folder)
        {
            databaseAndCacheMetadataExiftool.DeleteDirectory(MetadataBrokerType.ExifTool, folder); //Also delete When (Broker & @Broker) = @Broker
            databaseAndCacheMetadataMicrosoftPhotos.DeleteDirectory(MetadataBrokerType.MicrosoftPhotos, folder);
            databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteDirectory(MetadataBrokerType.WindowsLivePhotoGallery, folder);
            databaseExiftoolData.DeleteDirectory(folder);
            databaseExiftoolWarning.DeleteDirectory(folder);
            databaseAndCacheThumbnail.DeleteDirectory(folder);
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteMetadataFileEntry
        public void DeleteMetadataFileEntry(FileEntry fileEntry)
        {

            databaseAndCacheMetadataExiftool.MetadataCacheRemove(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
            databaseAndCacheMetadataExiftool.DeleteFileEntry(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));  //Also delete When (Broker & @Broker) = @Broker

            databaseAndCacheMetadataMicrosoftPhotos.MetadataCacheRemove(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos));
            databaseAndCacheMetadataMicrosoftPhotos.DeleteFileEntry(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos));

            databaseAndCacheMetadataWindowsLivePhotoGallery.MetadataCacheRemove(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery));
            databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteFileEntry(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery));

            databaseExiftoolData.DeleteFileMediaExiftoolTags(fileEntry);
            databaseExiftoolWarning.DeleteFileEntry(fileEntry);
            databaseAndCacheThumbnail.DeleteThumbnail(fileEntry);
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteMetadataHirstory
        public void DeleteMetadataHirstory(string fullFilePath)
        {

            List<FileEntryBroker> fileEntryBrokers = databaseAndCacheMetadataExiftool.ListFileEntryBrokerDateVersions(MetadataBrokerType.ExifTool, fullFilePath);
            foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
            {
                databaseAndCacheMetadataExiftool.MetadataCacheRemove(fileEntryBroker);
                databaseAndCacheMetadataExiftool.DeleteFileEntry(fileEntryBroker);
            }

            fileEntryBrokers =
                databaseAndCacheMetadataMicrosoftPhotos.ListFileEntryBrokerDateVersions(MetadataBrokerType.MicrosoftPhotos, fullFilePath);
            foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
            {
                databaseAndCacheMetadataMicrosoftPhotos.MetadataCacheRemove(fileEntryBroker);
                databaseAndCacheMetadataMicrosoftPhotos.DeleteFileEntry(fileEntryBroker);
            }

            fileEntryBrokers =
                databaseAndCacheMetadataWindowsLivePhotoGallery.ListFileEntryBrokerDateVersions(MetadataBrokerType.WindowsLivePhotoGallery, fullFilePath);
            foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
            {
                databaseAndCacheMetadataWindowsLivePhotoGallery.MetadataCacheRemove(fileEntryBroker);
                databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteFileEntry(fileEntryBroker);
            }

            List<FileEntryAttribute> fileEntryAttributes;
            fileEntryAttributes = databaseExiftoolData.ListFileEntryDateVersions(fullFilePath);
            foreach (FileEntry fileEntry in fileEntryAttributes)
            {
                databaseExiftoolData.DeleteFileMediaExiftoolTags(fileEntry);
            }

            fileEntryAttributes = databaseExiftoolWarning.ListFileEntryDateVersions(fullFilePath);
            foreach (FileEntry fileEntry in fileEntryAttributes)
            {
                databaseExiftoolWarning.DeleteFileEntry(fileEntry);
            }

            List<FileEntry> fileEntries = databaseAndCacheThumbnail.ListFileEntryDateVersions(fullFilePath);
            foreach (FileEntry fileEntry in fileEntries)
            {
                databaseAndCacheThumbnail.DeleteThumbnail(fileEntry);
            }
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteSelectedFiles
        public void DeleteSelectedFiles(ImageListView imageListView)
        {
            GlobalData.IsPopulatingImageListView = true;
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;

            imageListView.SuspendLayout();

            foreach (ImageListViewItem imageListViewItem in imageListView.SelectedItems)
            {
                try
                {
                    this.DeleteMetadataHirstory(imageListViewItem.FileFullPath);
                    File.Delete(imageListViewItem.FileFullPath);
                    imageListView.Items.Remove(imageListViewItem);
                }
                catch
                {
                    MessageBox.Show("Was not able to delete the file: " + imageListViewItem.FileFullPath, "Deleting file failed", MessageBoxButtons.OK);
                }
            }

            imageListView.ResumeLayout();

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            GlobalData.IsPopulatingImageListView = false;
  
        }
        #endregion

        #region FilesCutCopyPasteDrag - RefeshFolderTree
        public void RefeshFolderTree(FolderTreeView folderTreeViewFolder, TreeNode targetNode)
        {
            if (targetNode != null)
            {
                targetNode.Nodes.Clear();

                TreeNode ntn = new TreeNode();
                ntn.Tag = "DUMMYNODE";
                targetNode.Nodes.Add(ntn); //Internal use of TreeView as sign that subfolders exists

                folderTreeViewFolder.SelectedNode = targetNode;
                targetNode.Collapse();
                targetNode.Expand();
            } else
            {
                //DEBUG: None node is selected
            }
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteFilesInFolder
        public void DeleteFilesInFolder(FolderTreeView folderTreeViewFolder, string folder)
        {
            string[] dirs = Directory.GetDirectories(folder + (folder.EndsWith(@"\") ? "" : @"\"), "*", SearchOption.AllDirectories);

            Directory.Delete(folder, true);

            foreach (string directory in dirs)
            {
                this.DeleteDirectory(directory);
            }
            this.DeleteDirectory(folder);

            TreeNode selectedNode = folderTreeViewFolder.SelectedNode;
            TreeNode parentNode = folderTreeViewFolder.SelectedNode.Parent;

            #region Update Node in TreeView
            GlobalData.DoNotRefreshImageListView = true;
            selectedNode.Remove();
            if (parentNode != null)
            {
                RefeshFolderTree(folderTreeViewFolder, parentNode);
            }
            GlobalData.DoNotRefreshImageListView = false;
            

            #endregion
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteFilesMetadataForReload
        public void ImageListViewReload(MainForm mainForm, ImageListView imageListView, ImageListViewItemCollection itemCollection, bool updatedOnlySelected)
        {            
            foreach (ImageListViewItem item in itemCollection)
            {
                if (!updatedOnlySelected || (updatedOnlySelected && item.Selected))
                {
                    item.Update();
                    mainForm.LoadDataGridViewProgerssAdd();
                }
            }
            
        }

        public void DeleteFilesMetadataBeforeReload(MainForm mainForm, ImageListView imageListView, ImageListViewItemCollection itemCollection, bool updatedOnlySelected)
        {
            foreach (ImageListViewItem item in itemCollection)
            {
                if (!updatedOnlySelected || (updatedOnlySelected && item.Selected)) this.DeleteMetadataFileEntry(new FileEntry(item.FileFullPath, item.DateModified));
                mainForm.LoadDataGridViewProgerssAdd();
            }
        }
        #endregion

        #region FilesCutCopyPasteDrag - ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory
        public void ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(FolderTreeView folderTreeViewFolder, ImageListView imageListView)
        {
            if (GlobalData.IsPopulatingAnything()) return;

            GlobalData.IsPopulatingButtonAction = true;

            folderTreeViewFolder.Enabled = false;
            imageListView.Enabled = false;
            imageListView.SuspendLayout();

            
            foreach (ImageListViewItem item in imageListView.SelectedItems)
            {
                this.DeleteMetadataHirstory(item.FileFullPath);
            }

            foreach (ImageListViewItem item in imageListView.SelectedItems)
            {
                item.Update();
                item.Selected = true;
            }
            

            folderTreeViewFolder.Enabled = true;

            GlobalData.IsPopulatingButtonAction = false;

            imageListView.ResumeLayout();
            imageListView.Enabled = true;
        }
        #endregion

        #region FilesCutCopyPasteDrag - MoveFile
        public void MoveFile(string sourceFullFilename, string targetFullFilename)
        {
            if (File.Exists(sourceFullFilename))
            {    
                
                string oldFilename = Path.GetFileName(sourceFullFilename);
                string oldDirectory = Path.GetDirectoryName(sourceFullFilename);

                string newFilename = Path.GetFileName(targetFullFilename);
                string newDirectory = Path.GetDirectoryName(targetFullFilename);

                Directory.CreateDirectory(newDirectory);
                File.Move(sourceFullFilename, targetFullFilename);
                databaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename);
            }
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
        public void CopyFile(string sourceFullFilename, string targetFullFilename)
        {
            if (File.Exists(sourceFullFilename))
            {
                string oldFilename = Path.GetFileName(sourceFullFilename);
                string oldDirectory = Path.GetDirectoryName(sourceFullFilename);

                string newFilename = Path.GetFileName(targetFullFilename);
                string newDirectory = Path.GetDirectoryName(targetFullFilename);

                Directory.CreateDirectory(newDirectory);
                File.Copy(sourceFullFilename, targetFullFilename);                
                File.SetCreationTime(targetFullFilename, File.GetCreationTime(sourceFullFilename));
                databaseAndCacheMetadataExiftool.Copy(oldDirectory, oldFilename, newDirectory, newFilename);
            }
        }
        #endregion
    }
}
