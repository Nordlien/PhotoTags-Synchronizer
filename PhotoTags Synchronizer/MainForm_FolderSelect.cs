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

        #region FolderSelected - Populate DataGridView, ImageListView 
        private void PopulateImageListViewBasedOnSelectedFolderAndOrFilter(bool recursive, bool runPopulateFilter)
        {
            if (GlobalData.IsPopulatingFolderSelected) //If in progress, then stop and reselect new
            {
                ImageListViewClearAll(imageListView1);
                GlobalData.IsPopulatingFolderSelected = false;
            }

            if (GlobalData.IsPopulatingAnything()) return;

            using (new WaitCursor())
            {
                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
                GlobalData.SearchFolder = true;

                if (runPopulateFilter)
                {
                    FilterVerifyer.ClearTreeViewNodes(treeViewFilter);
                }

                folderTreeViewFolder.Enabled = false;

                ClearQueuePreloadningMetadata();
                ClearQueueExiftool();
                List<FileEntry> imageListViewFileEntryItems = ImageListViewAggregateWithFilesFromFolder(this.folderTreeViewFolder.GetSelectedNodePath(), recursive);
                AddQueueExiftoolLock(imageListViewFileEntryItems);
                
                folderTreeViewFolder.Enabled = true;
                
                if (runPopulateFilter) PopulateTreeViewFolderFilterThread(imageListViewFileEntryItems);
                

                GlobalData.IsPopulatingFolderSelected = false;
            }

            FilesSelected(); //Even when 0 selected files, allocate data and flags, etc...
            
            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region FolderSearchFilter - Populate DataGridView, ImageListView 
        private void PopulateImageisteViedBasedOnSearchResult(List<FileEntry> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
                GlobalData.SearchFolder = false;

                folderTreeViewFolder.Enabled = false;

                if (cacheFolderThumbnails || cacheFolderMetadatas || cacheFolderWebScraperDataSets) CacheSelected(searchFilterResult);

                ImageListViewAggregateFromSearchFilter(searchFilterResult);
                folderTreeViewFolder.Enabled = true; //Avoid select folder while loading ImageListView
                if (runPopulateFilter) PopulateTreeViewFolderFilterThread(searchFilterResult);

                AddQueueExiftoolLock(searchFilterResult);
                GlobalData.IsPopulatingFolderSelected = false;
            }
            

            FilesSelected(); //Even when 0 selected files, allocate data and flags, etc...

            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }
        #endregion 

        #endregion


    }
}

