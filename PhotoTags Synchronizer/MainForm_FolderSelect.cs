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

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : Form
    {
        #region FolderSelected or FilterSearch clicked

        #region PopulateImageListView
        private void PopulateImageListView(HashSet<FileEntry> fileEntries, string selectedFolder, bool runPopulateFilter = true)
        {
            using (new WaitCursor())
            {
                UpdateStatusAction("Adding files to image list: " + fileEntries.Count);

                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
                GlobalData.SearchFolder = true;
                folderTreeViewFolder.Enabled = false;

                ClearAllQueues();

                if (cacheFolderThumbnails || cacheFolderMetadatas || cacheFolderWebScraperDataSets) CacheFileEntries(fileEntries, selectedFolder);
                if (runPopulateFilter) FilterVerifyer.ClearTreeViewNodes(treeViewFilter);

                ImageListViewAggregateWithMediaFiles(fileEntries);
                
                folderTreeViewFolder.Enabled = true;

                if (runPopulateFilter) PopulateTreeViewFolderFilterThread(fileEntries);

                GlobalData.IsPopulatingFolderSelected = false;
            }

            FilesSelected(); //Even when 0 selected files, allocate data and flags, etc...
            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region FolderSelected - Populate DataGridView, ImageListView 
        private void PopulateImageListView_FromFolderSelected(bool recursive, bool runPopulateFilter)
        {
            #region Read folder files
            if (GlobalData.IsPopulatingFolderSelected) //If in progress, then stop and reselect new
            {
                ImageListViewClearAll(imageListView1);
                GlobalData.IsPopulatingFolderSelected = false;
            }

            if (GlobalData.IsPopulatingAnything()) return;

            string selectedFolder = this.folderTreeViewFolder.GetSelectedNodePath();
            Properties.Settings.Default.LastFolder = selectedFolder;
            try
            {
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Can't save settings");
            }

            UpdateStatusAction("Read files in folder: " + selectedFolder);
            HashSet<FileEntry> fileEntries = ImageAndMovieFileExtentionsUtility.ListAllMediaFileEntries(selectedFolder, recursive);
            if (FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, false))
            {
                switch (MessageBox.Show("OneDrive duplicated files found.\r\n" +
                    "\r\n"+
                    "Will you replace older files with newest files\r\n" +
                    "Yes - keep the newest files\r\n" +
                    "No - delete OneDrive marked files regardless who is newest\r\n" + 
                    "Cancel - Cancel the operation, Leave the files intact", "OneDrive duplicated files found.", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, true, true);
                        fileEntries = ImageAndMovieFileExtentionsUtility.ListAllMediaFileEntries(selectedFolder, recursive);
                        break;
                    case DialogResult.No:
                        FileHandeling.FileHandler.FixOneDriveIssues(fileEntries, this, true, false);
                        fileEntries = ImageAndMovieFileExtentionsUtility.ListAllMediaFileEntries(selectedFolder, recursive);
                        break;
                }
            }
            #endregion

            PopulateImageListView(fileEntries, selectedFolder, runPopulateFilter);
        }
        #endregion

        #region FolderSearchFilter - Populate DataGridView, ImageListView 
        private void PopulateImageListView_FromSearchTab(HashSet<FileEntry> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;

            PopulateImageListView(searchFilterResult, null, runPopulateFilter);
        }
        #endregion 

        #endregion


    }
}

