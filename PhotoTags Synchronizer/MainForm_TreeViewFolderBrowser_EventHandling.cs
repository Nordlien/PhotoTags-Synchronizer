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
        #region FolderSelected or FilterSearch clicked

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
                PopulateImageListView_FromFolderSelected(false, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region PopulateImageListView
        private void PopulateImageListView(IEnumerable<FileData> fileDatas, HashSet<FileEntry> fileEntries, string selectedFolder, bool runPopulateFilter = true)
        {

            using (new WaitCursor())
            {
                #region Krypton Sort menu - set to unchecked
                for (int index = 0; index < kryptonContextMenuFileSystemColumnSort.Items.Count; index++)
                {
                    if (kryptonContextMenuFileSystemColumnSort.Items[index] is KryptonContextMenuRadioButton radioButton) radioButton.Checked = false;
                }
                #endregion

                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
                
                treeViewFolderBrowser1.Enabled = false;
                LoadingItemsImageListView(1, 5);
                UpdateStatusImageListView("Clear all old queues");
                ClearAllQueues();

                if (runPopulateFilter) FilterVerifyer.ClearTreeViewNodes(treeViewFilter);

                LoadingItemsImageListView(2, 5);
//UpdateStatusImageListView("Adding files to image list: " + fileData.Count);
                fileEntries = ImageListViewAggregateWithMediaFiles(fileDatas, fileEntries);

                treeViewFolderBrowser1.Enabled = true;
                GlobalData.IsPopulatingFolderSelected = false;

                #region Read to cache
                if (cacheFolderThumbnails || cacheFolderMetadatas || cacheFolderWebScraperDataSets)
                {
                    LoadingItemsImageListView(3, 5);
                    UpdateStatusImageListView("Started the cache process...");
                    CacheFileEntries(fileEntries, selectedFolder);
                }
                #endregion

                #region PopulateFilter
                if (runPopulateFilter)
                {
                    LoadingItemsImageListView(4, 5);
                    UpdateStatusImageListView("Populate Filters");
                    PopulateTreeViewFolderFilterThread(fileEntries);
                }
                #endregion
            }
            FilesSelectedOrNoneSelected(); //Even when 0 selected files, allocate data and flags, etc...

            LoadingItemsImageListView(5, 5);
            UpdateStatusImageListView("Done populate " + fileEntries.Count + " media files...");
            treeViewFolderBrowser1.Focus();
            LoadingItemsImageListView(0, 0);
        }
        #endregion

        #region GetNodeFolderPath / GetSelectedNodeFullPath
        private string GetNodeFolderPath(TreeNodePath treeNodePath)
        {
            string folder = treeNodePath?.Path == null ? "" : treeNodePath?.Path;
            return Directory.Exists(folder) ? folder : "";
        }

        private string GetSelectedNodePath()
        {
            return GetNodeFolderPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
        }

        private string GetNodeFolderFullPath(TreeNodePath treeNodePath)
        {
            return treeNodePath?.FullPath == null ? "" : treeNodePath?.FullPath;
        }

        private string GetSelectedNodeFullPath()
        {
            return GetNodeFolderFullPath(treeViewFolderBrowser1.SelectedNode as TreeNodePath);
        }
        #endregion 

        #region FolderSelected - Populate DataGridView, ImageListView 
        private void PopulateImageListView_FromFolderSelected(bool recursive, bool runPopulateFilter)
        {
            #region Read folder files
            if (GlobalData.IsPopulatingFolderSelected) //If in progress, then stop and reselect new
            {
                UpdateStatusImageListView("Remove old queues...");
                ImageListViewClearAll(imageListView1);
                GlobalData.IsPopulatingFolderSelected = false;
            }

            if (GlobalData.IsPopulatingAnything()) return;

            string selectedFolder = GetSelectedNodePath();
            Properties.Settings.Default.LastFolder = GetSelectedNodeFullPath();
            
            UpdateStatusImageListView("Read files in folder: " + selectedFolder);
            IEnumerable<FileData> fileDatas = GetFilesInSelectedFolder(selectedFolder, recursive);
            PopulateImageListView(fileDatas, null, selectedFolder, runPopulateFilter);

            //UpdateStatusImageListView("Check for OneDrive duplicate files in folder: " + selectedFolder);
            //if (FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, oneDriveNetworkNames, false))
            //{
            //    switch (KryptonMessageBox.Show("OneDrive duplicated files found.\r\n" +
            //        "\r\n"+
            //        "Will you replace older files with newest files\r\n" +
            //        "Yes - keep the newest files\r\n" +
            //        "No - delete OneDrive marked files regardless who is newest\r\n" + 
            //        "Cancel - Cancel the operation, Leave the files intact", "OneDrive duplicated files found.", MessageBoxButtons.YesNoCancel))
            //    {
            //        case DialogResult.Yes:
            //            FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, oneDriveNetworkNames, true, true);
            //            fileEntries = ImageAndMovieFileExtentionsUtility.ListAllMediaFileEntries(selectedFolder, recursive);
            //            break;
            //        case DialogResult.No:
            //            FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, oneDriveNetworkNames, true, false);
            //            fileEntries = ImageAndMovieFileExtentionsUtility.ListAllMediaFileEntries(selectedFolder, recursive);
            //            break;
            //    }
            //}
            #endregion

        }
        #endregion

        #region FolderSearchFilter - Populate DataGridView, ImageListView 
        private void PopulateImageListView_FromSearchTab(HashSet<FileEntry> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            PopulateImageListView(null, searchFilterResult, null, runPopulateFilter);
        }
        #endregion 

        #endregion


    }
}

