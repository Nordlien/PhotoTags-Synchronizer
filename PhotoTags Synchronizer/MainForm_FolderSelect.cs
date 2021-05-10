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
        private void PopulateImageListView(List<FileEntry> fileEntries, bool runPopulateFilter = true)
        {
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
                GlobalData.SearchFolder = true;
                folderTreeViewFolder.Enabled = false;

                ClearQueuePreloadningMetadata();
                ClearQueueExiftool();

                if (cacheFolderThumbnails || cacheFolderMetadatas || cacheFolderWebScraperDataSets) CacheFileEntries(fileEntries);
                if (runPopulateFilter) FilterVerifyer.ClearTreeViewNodes(treeViewFilter);

                ImageListViewAggregateWithMediaFiles(fileEntries);
                
                folderTreeViewFolder.Enabled = true;

                if (runPopulateFilter) PopulateTreeViewFolderFilterThread(fileEntries);
                AddQueueExiftoolLock(fileEntries);

                GlobalData.IsPopulatingFolderSelected = false;
            }

            FilesSelected(); //Even when 0 selected files, allocate data and flags, etc...
            DisplayAllQueueStatus();
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
            Properties.Settings.Default.Save();

            List<FileEntry> fileEntries = ImageAndMovieFileExtentionsUtility.ListAllMediaFileEntries(selectedFolder, recursive);            
            #endregion 

            PopulateImageListView(fileEntries, runPopulateFilter);
        }
        #endregion

        #region FolderSearchFilter - Populate DataGridView, ImageListView 
        private void PopulateImageListView_FromSearchTab(List<FileEntry> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;

            PopulateImageListView(searchFilterResult, runPopulateFilter);
        }
        #endregion 

        #endregion


    }
}

