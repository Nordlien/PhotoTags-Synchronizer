using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;
using System.Threading;
using Thumbnails;
using System.Diagnostics;
using Krypton.Toolkit;
using Raccoom.Windows.Forms;
using FileHandeling;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
    
        #region FolderTree - BeforeSelect - Click
        private void treeViewFolderBrowser1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (SaveBeforeContinue(true) == DialogResult.Cancel) e.Cancel = true;
        }
        #endregion

        #region FolderTree - AfterSelect - Click
        private void treeViewFolderBrowser1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (GlobalData.IsPopulatingFolderTree) return;
                if (GlobalData.IsDragAndDropActive) return;
                if (GlobalData.DoNotRefreshImageListView) return;
                GlobalData.SearchFolder = true;
                ImageListView_Aggregate_FromFolder(false, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
            }
        }
        #endregion


        #region PopulateImageListView
        private HashSet<FileEntry> ImageListView_Aggregate_FromReadFolderOrFilterOrDatabase(IEnumerable<FileData> fileDatas, HashSet<FileEntry> fileEntries, string selectedFolder, bool runPopulateFilter = true)
        {
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice

                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, false);
                LoadingItemsImageListView(1, 6);
                UpdateStatusImageListView("Clear all old queues");
                ClearAllQueues();

                if (runPopulateFilter) FilterVerifyer.ClearTreeViewNodes(treeViewFilter);

                LoadingItemsImageListView(2, 6);
                UpdateStatusImageListView("Adding files to image list...");
                fileEntries = ImageListView_Populate_MediaFiles_WithFilter(fileDatas, fileEntries);

                SetImageListViewFileEntriesCache(fileEntries);
                LoadingItemsImageListView(3, 6);
                UpdateStatusImageListView("Sorting...");
                ImageListViewSortByCheckedRadioButton(false);

                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, true);
                GlobalData.IsPopulatingFolderSelected = false;

                #region Read to cache
                if (cacheFolderThumbnails || cacheFolderMetadatas || cacheFolderWebScraperDataSets)
                {
                    LoadingItemsImageListView(4, 6);
                    UpdateStatusImageListView("Started the cache process...");
                    CacheFileEntries(fileEntries, selectedFolder);
                }
                #endregion

                #region PopulateFilter
                if (runPopulateFilter)
                {
                    LoadingItemsImageListView(6, 6);
                    UpdateStatusImageListView("Populate Filters...");
                    PopulateTreeViewFolderFilterThread(fileEntries);
                }
                #endregion

                OnImageListViewSelect_FilesSelectedOrNoneSelected(false); //Even when 0 selected files, allocate data and flags, etc...

                LoadingItemsImageListView(6, 6);
                UpdateStatusImageListView("Done populate " + fileEntries.Count + " media files...");
                treeViewFolderBrowser1.Focus();
                LoadingItemsImageListView(0, 0);
            }
            return fileEntries;
        }
        #endregion

        #region GetNodeFolderPath / GetSelectedNodeFullPath
        private string GetNodeFolderRealPath(TreeNodePath treeNodePath)
        {
            string folder = treeNodePath?.Path == null ? "" : treeNodePath?.Path; //"C:\\Users\\nordl\\OneDrive\\Skrivebord"
            if (folder == "Desktop") return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (folder == "Documents") return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (folder == "Music") return Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            if (folder == "Pictures") return Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (folder == "Videos") return Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            //if (folder == "Downloads") return Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            return Directory.Exists(folder) ? folder : "";
        }
        private string GetSelectedNodeFullRealPath() 
        {
            return GetNodeFolderRealPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
        }

        private string GetNodeFolderFullLinkPath(TreeNodePath treeNodePath)
        {
            return treeNodePath?.FullPath == null ? "" : treeNodePath?.FullPath; //"Desktop"
            //Path     "C:\\Users\\nordl\\OneDrive\\Pictures JTNs OneDrive\\a-- PhotoTags Synchronizer --a"
            //FullPath "Desktop\\This PC\\Pictures\\a-- PhotoTags Synchronizer --a"
        }

        private string GetSelectedNodeFullLinkPath() 
        {
            return GetNodeFolderFullLinkPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
        }
        #endregion 

        #region FolderSelected - Populate DataGridView, ImageListView 
        private void ImageListView_Aggregate_FromFolder(bool recursive, bool runPopulateFilter)
        {
            
            if (GlobalData.IsPopulatingFolderSelected) //If in progress, then stop and reselect new
            {
                UpdateStatusImageListView("Remove old queues...");
                ImageListViewClearAll(imageListView1);
                GlobalData.IsPopulatingFolderSelected = false;
            }

            if (GlobalData.IsPopulatingAnything()) return;

            bool wasOneDriveDublicatedFoundAndremoved = false;
            do
            {
                #region Read folder files
                string selectedFolder = GetSelectedNodeFullRealPath();
                Properties.Settings.Default.LastFolder = GetSelectedNodeFullLinkPath();

                UpdateStatusImageListView("Read files in folder: " + selectedFolder);
                IEnumerable<FileData> fileDatas = GetFilesInSelectedFolder(selectedFolder, recursive);
                HashSet<FileEntry> fileEntries = ImageListView_Aggregate_FromReadFolderOrFilterOrDatabase(fileDatas, null, selectedFolder, runPopulateFilter);
                #endregion

                #region Check for OneDrive duplicate files in folder
                wasOneDriveDublicatedFoundAndremoved = false;
                UpdateStatusImageListView("Check for OneDrive duplicate files in folder: " + selectedFolder);
                if (FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, oneDriveNetworkNames, false))
                {
                    switch (KryptonMessageBox.Show("OneDrive duplicated files found.\r\n" +
                        "\r\n" +
                        "Will you replace older files with newest files\r\n" +
                        "Yes - keep the newest files\r\n" +
                        "No - delete OneDrive marked files regardless who is newest\r\n" +
                        "Cancel - Cancel the operation, Leave the files intact", "OneDrive duplicated files found.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, true))
                    {
                        case DialogResult.Yes:
                            FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, oneDriveNetworkNames, true, true);
                            wasOneDriveDublicatedFoundAndremoved = false;
                            break;
                        case DialogResult.No:
                            FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, oneDriveNetworkNames, true, false);
                            wasOneDriveDublicatedFoundAndremoved = true;
                            break;
                    }
                }
                #endregion
            } while (wasOneDriveDublicatedFoundAndremoved);

        }
        #endregion

        #region ImageListView_Aggregate_FromDatabaseSearchResult
        private void ImageListView_Aggregate_FromDatabaseSearchResult(HashSet<FileEntry> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            HashSet<FileEntry> _ = ImageListView_Aggregate_FromReadFolderOrFilterOrDatabase(null, searchFilterResult, null, runPopulateFilter);
        }
        #endregion 

    }
}

