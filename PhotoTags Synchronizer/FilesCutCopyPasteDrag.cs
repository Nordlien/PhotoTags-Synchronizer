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
        private MetadataDatabaseCache databaseAndCacheMetadataExiftool;
        private MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery;
        private MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos;
        private ThumbnailDatabaseCache databaseAndCacheThumbnail;
        private ExiftoolDataDatabase databaseExiftoolData;
        private ExiftoolWarningDatabase databaseExiftoolWarning;

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


        public void DeleteDirectory(string folder)
        {
            databaseAndCacheMetadataExiftool.DeleteDirectory(MetadataBrokerTypes.ExifTool, folder);
            databaseAndCacheMetadataMicrosoftPhotos.DeleteDirectory(MetadataBrokerTypes.MicrosoftPhotos, folder);
            databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteDirectory(MetadataBrokerTypes.WindowsLivePhotoGallery, folder);
            databaseExiftoolData.DeleteDirectory(folder);
            databaseExiftoolWarning.DeleteDirectory(folder);
            databaseAndCacheThumbnail.DeleteDirectory(folder);
        }

        public void DeleteMetadataFileEntry(FileEntry fileEntry)
        {

            databaseAndCacheMetadataExiftool.CacheRemove(new FileEntryBroker(fileEntry, MetadataBrokerTypes.ExifTool));
            databaseAndCacheMetadataExiftool.DeleteFileEntry(new FileEntryBroker(fileEntry, MetadataBrokerTypes.ExifTool));

            databaseAndCacheMetadataMicrosoftPhotos.CacheRemove(new FileEntryBroker(fileEntry, MetadataBrokerTypes.MicrosoftPhotos));
            databaseAndCacheMetadataMicrosoftPhotos.DeleteFileEntry(new FileEntryBroker(fileEntry, MetadataBrokerTypes.MicrosoftPhotos));

            databaseAndCacheMetadataWindowsLivePhotoGallery.CacheRemove(new FileEntryBroker(fileEntry, MetadataBrokerTypes.WindowsLivePhotoGallery));
            databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteFileEntry(new FileEntryBroker(fileEntry, MetadataBrokerTypes.WindowsLivePhotoGallery));

            databaseExiftoolData.DeleteFileMediaExiftoolTags(fileEntry);
            databaseExiftoolWarning.DeleteFileEntry(fileEntry);
            databaseAndCacheThumbnail.DeleteThumbnail(fileEntry);
        }

        public void DeleteMetadataHirstory(string fullFilePath)
        {

            List<FileEntryBroker> fileEntryBrokers = databaseAndCacheMetadataExiftool.ListFileEntryDateVersions(MetadataBrokerTypes.ExifTool, fullFilePath);
            foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
            {
                databaseAndCacheMetadataExiftool.CacheRemove(fileEntryBroker);
                databaseAndCacheMetadataExiftool.DeleteFileEntry(fileEntryBroker);
            }

            fileEntryBrokers =
                databaseAndCacheMetadataMicrosoftPhotos.ListFileEntryDateVersions(MetadataBrokerTypes.MicrosoftPhotos, fullFilePath);
            foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
            {
                databaseAndCacheMetadataMicrosoftPhotos.CacheRemove(fileEntryBroker);
                databaseAndCacheMetadataMicrosoftPhotos.DeleteFileEntry(fileEntryBroker);
            }

            fileEntryBrokers =
                databaseAndCacheMetadataWindowsLivePhotoGallery.ListFileEntryDateVersions(MetadataBrokerTypes.WindowsLivePhotoGallery, fullFilePath);
            foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
            {
                databaseAndCacheMetadataWindowsLivePhotoGallery.CacheRemove(fileEntryBroker);
                databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteFileEntry(fileEntryBroker);
            }

            List<FileEntry> fileEntries;
            fileEntries = databaseExiftoolData.ListFileEntryDateVersions(fullFilePath);
            foreach (FileEntry fileEntry in fileEntries)
            {
                databaseExiftoolData.DeleteFileMediaExiftoolTags(fileEntry);
            }

            fileEntries = databaseExiftoolWarning.ListFileEntryDateVersions(fullFilePath);
            foreach (FileEntry fileEntry in fileEntries)
            {
                databaseExiftoolWarning.DeleteFileEntry(fileEntry);
            }

            fileEntries = databaseAndCacheThumbnail.ListFileEntryDateVersions(fullFilePath);
            foreach (FileEntry fileEntry in fileEntries)
            {
                databaseAndCacheThumbnail.DeleteThumbnail(fileEntry);
            }
        }

        public void DeleteSelectedFiles(ImageListView imageListView)
        {
            GlobalData.IsPopulatingImageListView = true;
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;

            imageListView.SuspendLayout();
            foreach (ImageListViewItem imageListViewItem in imageListView.SelectedItems)
            {
                try
                {
                    this.DeleteMetadataHirstory(imageListViewItem.FullFileName);
                    File.Delete(imageListViewItem.FullFileName);
                    imageListView.Items.Remove(imageListViewItem);
                }
                catch
                {
                    MessageBox.Show("Was not able to delete the file: " + imageListViewItem.FullFileName, "Deleting file failed", MessageBoxButtons.OK);
                }                
            }
            imageListView.ResumeLayout();

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            GlobalData.IsPopulatingImageListView = false;
  
        }

        public void RefeshFolderTree(FolderTreeView folderTreeViewFolder, TreeNode targetNode)
        {
            targetNode.Nodes.Clear();

            TreeNode ntn = new TreeNode();
            ntn.Tag = "DUMMYNODE";
            targetNode.Nodes.Add(ntn); //Internal use of TreeView as sign that subfolders exists

            folderTreeViewFolder.SelectedNode = targetNode;
            targetNode.Collapse();
            targetNode.Expand();
        }

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

        public void DeleteFilesMetadataForReload(FolderTreeView folderTreeViewFolder, ImageListView imageListView, ImageListViewItemCollection itemCollection, bool updatedOnlySelected)
        {

            if (GlobalData.IsPopulatingAnything()) return;
            //if (GlobalData.IsAgredagedGridViewAny()) return;

            GlobalData.IsPopulatingButtonAction = true;
            GlobalData.IsPopulatingExiftoolTagsImage = true;

            folderTreeViewFolder.Enabled = false;
            imageListView.Enabled = false;
            imageListView.SuspendLayout();

            foreach (ImageListViewItem item in itemCollection)
            {
                if (!updatedOnlySelected || (updatedOnlySelected && item.Selected))
                    this.DeleteMetadataFileEntry(new FileEntry(item.FullFileName, item.DateModified));
            }

            foreach (ImageListViewItem item in itemCollection)
            {
                if (!updatedOnlySelected || (updatedOnlySelected && item.Selected))
                {
                    item.Update();
                }
            }

            folderTreeViewFolder.Enabled = true;

            GlobalData.IsPopulatingButtonAction = false;
            GlobalData.IsPopulatingExiftoolTagsImage = false;

            imageListView.ResumeLayout();
            imageListView.Enabled = true;
            
        }

        public void ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(FolderTreeView folderTreeViewFolder, ImageListView imageListView)
        {
            if (GlobalData.IsPopulatingAnything()) return;

            GlobalData.IsPopulatingButtonAction = true;

            folderTreeViewFolder.Enabled = false;
            imageListView.Enabled = false;
            imageListView.SuspendLayout();

            foreach (ImageListViewItem item in imageListView.SelectedItems)
            {
                this.DeleteMetadataHirstory(item.FullFileName);
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
    }
}
