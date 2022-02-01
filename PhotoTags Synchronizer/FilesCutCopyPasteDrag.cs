using Exiftool;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Thumbnails;
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
        private ThumbnailPosterDatabaseCache databaseAndCacheThumbnail;
        private ExiftoolDataDatabase databaseExiftoolData;
        private ExiftoolWarningDatabase databaseExiftoolWarning;

        #region IsFilenameEqual
        public static bool IsFilenameEqual(string fullFileName1, string fullFileName2)
        {
            if (fullFileName1 == null && fullFileName2 != null) return false;
            if (fullFileName1 != null && fullFileName2 == null) return false;
            if (fullFileName1 == null && fullFileName2 == null) return true;
            return String.Compare(fullFileName1, fullFileName2, comparisonType: StringComparison.OrdinalIgnoreCase) == 0;
        }
        #endregion

        #region FilesCutCopyPasteDrag - Constructor
        public FilesCutCopyPasteDrag(MetadataDatabaseCache databaseAndCacheMetadataExiftool, 
            MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery, 
            MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos, 
            ThumbnailPosterDatabaseCache databaseAndCacheThumbnail, 
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
        public int DeleteDirectoryAndHistory(string folder)
        {
            int rowsAffected = 0;
            rowsAffected += databaseAndCacheMetadataExiftool.DeleteDirectoryAndHistory(MetadataBrokerType.ExifTool, folder); //Also delete When Broker = @Broker
            rowsAffected += databaseAndCacheMetadataMicrosoftPhotos.DeleteDirectoryAndHistory(MetadataBrokerType.MicrosoftPhotos, folder);
            rowsAffected += databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteDirectoryAndHistory(MetadataBrokerType.WindowsLivePhotoGallery, folder);
            rowsAffected += databaseExiftoolData.DeleteDirectoryAndHistory(folder);
            rowsAffected += databaseExiftoolWarning.DeleteDirectoryAndHistory(folder);
            rowsAffected += databaseAndCacheThumbnail.DeleteDirectoryAndHistory(folder);
            return rowsAffected;
        }
        #endregion

        #region FilesCutCopyPasteDrag - DeleteFileEntry
        public void DeleteFileEntry(FileEntry fileEntry)
        {
            HashSet<FileEntry> fileEntries = new HashSet<FileEntry>();
            fileEntries.Add(fileEntry);
            DeleteFileEntries(fileEntries);
        }
        public void DeleteFileEntries(HashSet<FileEntry> fileEntries)
        {

            List<FileEntry> fileEntriesList = new List<FileEntry>(fileEntries);
            List<FileEntryBroker> fileEntryBrokersExifTool = new List<FileEntryBroker>();
            List<FileEntryBroker> fileEntryBrokersMicrosoftPhotos = new List<FileEntryBroker>();
            List<FileEntryBroker> fileEntryBrokersWindowsPhotoGallary = new List<FileEntryBroker>();

            foreach (FileEntry fileEntry in fileEntries)
            {
                fileEntryBrokersExifTool.Add(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool));
                fileEntryBrokersExifTool.Add(new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool | MetadataBrokerType.ExifToolWriteError));
                fileEntryBrokersMicrosoftPhotos.Add(new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos));
                fileEntryBrokersWindowsPhotoGallary.Add(new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery));
            }

            databaseAndCacheMetadataExiftool.MetadataCacheRemove(fileEntryBrokersExifTool);
            databaseAndCacheMetadataExiftool.DeleteFileEntries(fileEntryBrokersExifTool);  //Also delete When Broker = @Broker

            databaseAndCacheMetadataMicrosoftPhotos.MetadataCacheRemove(fileEntryBrokersMicrosoftPhotos);
            databaseAndCacheMetadataMicrosoftPhotos.DeleteFileEntries(fileEntryBrokersMicrosoftPhotos);

            databaseAndCacheMetadataWindowsLivePhotoGallery.MetadataCacheRemove(fileEntryBrokersWindowsPhotoGallary);
            databaseAndCacheMetadataWindowsLivePhotoGallery.DeleteFileEntries(fileEntryBrokersWindowsPhotoGallary);

            databaseExiftoolData.DeleteFileEntriesFromMediaExiftoolTags(fileEntriesList);
            databaseExiftoolWarning.DeleteFileEntriesFromMediaExiftoolTagsWarning(fileEntriesList);
            databaseAndCacheThumbnail.DeleteThumbnails(fileEntriesList);
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
        public void DeleteSelectedFiles(MainForm mainForm, ImageListView imageListView, HashSet<FileEntry> fileEntries, bool deleteFromFileSystemAlso)
        {

            GlobalData.IsPopulatingImageListView = true;
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;

            imageListView.SuspendLayout();

            foreach (FileEntry fileEntry in fileEntries)
            {
                try
                {
                    mainForm.UpdateStatusAction("Deleting the file " + fileEntry.FileFullPath + " and records in database");
                    if (deleteFromFileSystemAlso) File.Delete(fileEntry.FileFullPath);
                    this.DeleteFileAndHistory(fileEntry.FileFullPath);
                    imageListView.Items.Remove(ImageListViewHandler.FindItem(imageListView.Items, fileEntry.FileFullPath));
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show("Was not able to delete the file: " + fileEntry.FileFullPath + "\r\n\r\n" + ex.Message, 
                        "Deleting file failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            string[] subFolders = Directory.GetDirectories(folder + (folder.EndsWith(@"\") ? "" : @"\"), "*", SearchOption.AllDirectories);

            Directory.Delete(folder, true);

            int recordAffected = 0;
            
            foreach (string directory in subFolders)
            {
                mainForm.UpdateStatusAction("Delete all data and files from folder: " + directory);
                recordAffected += this.DeleteDirectoryAndHistory(directory);
            }

            mainForm.UpdateStatusAction("Delete all data and files from folder: " + folder);
            recordAffected += this.DeleteDirectoryAndHistory(folder);
            
            TreeNode selectedNode = folderTreeViewFolder.SelectedNode;
            TreeNode parentNode = folderTreeViewFolder.SelectedNode.Parent;

            #region Update Node in TreeView
            GlobalData.DoNotRefreshImageListView = true;
            
            TreeViewFolderBrowserHandler.RemoveTreeNode (folderTreeViewFolder, selectedNode);
            if (parentNode != null)
            {
                TreeViewFolderBrowserHandler.RefreshTreeNode(folderTreeViewFolder, parentNode);
            }
            GlobalData.DoNotRefreshImageListView = false;
            #endregion

            return recordAffected;
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
                if (!IsFilenameEqual(sourceFullFilename, targetFullFilename))
                {
                    File.Move(sourceFullFilename, targetFullFilename);
                    
                    databaseAndCacheThumbnail.Move(oldDirectory, oldFilename, newDirectory, newFilename);
                    if (!databaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename))
                    {
                        DeleteFileAndHistory(Path.Combine(newDirectory, newFilename));
                        
                        databaseAndCacheThumbnail.Move(oldDirectory, oldFilename, newDirectory, newFilename);
                        databaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename);
                    }
                }
            }
            return directoryCreated;
        }
        #endregion

        #region FilesCutCopyPasteDrag - RenameFile
        public void RenameFile(string oldFullFilename, string newFullFilename, ref Dictionary<string, string> renameSuccess, ref Dictionary<string, RenameToNameAndResult> renameFailed)
        {
            try
            {
                MoveFile(oldFullFilename, newFullFilename);
                if (renameSuccess != null) renameSuccess.Add(oldFullFilename, newFullFilename);
            }
            catch (Exception ex)
            {
                string errorMessage = "Rename file failed: " + oldFullFilename + " to :" + newFullFilename + " " + ex.Message;
                if (renameFailed != null) renameFailed.Add(oldFullFilename, new RenameToNameAndResult(newFullFilename, errorMessage));
                Logger.Error(errorMessage);
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

    #region RenameToNameAndResult
    public class RenameToNameAndResult
    {
        public RenameToNameAndResult(string newFilename, string errorMessage)
        {
            NewFilename = newFilename;
            ErrorMessage = errorMessage;
        }

        public string NewFilename { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
    }
    #endregion 
}
