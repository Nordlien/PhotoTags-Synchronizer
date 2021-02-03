using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using MetadataLibrary;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : Form
    {
        #region FolderSelected or FilterSearch clicked

        #region FolderSelected - Populate DataGridView, ImageListView 
        private void PopulateImageListViewBasedOnSleectedFolderAndOrFilter(bool recursive, bool runPopulateFilter)
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
                    ClearQueuePreloadningMetadata();
                }

                folderTreeViewFolder.Enabled = false;
                List<FileEntry> imageListViewFileEntryItems = ImageListViewAggregateWithFilesFromFolder(this.folderTreeViewFolder.GetSelectedNodePath(), recursive);
                
                folderTreeViewFolder.Enabled = true;
                if (runPopulateFilter) PopulateTreeViewFolderFilterThread(imageListViewFileEntryItems);
                PopulatePreloadMetadataQueue(imageListViewFileEntryItems);

                GlobalData.IsPopulatingFolderSelected = false;
            }

            FilesSelected(); //Even when 0 selected files, allocate data and flags, etc...

            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region FolderSearchFilter - Populate DataGridView, ImageListView 
        private void FolderSearchFilter(List<FileEntry> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
                GlobalData.SearchFolder = false;

                folderTreeViewFolder.Enabled = false;
                ImageListViewAggregateFromSearchFilter(searchFilterResult);
                folderTreeViewFolder.Enabled = true; //Avoid select folder while loading ImageListView
                if (runPopulateFilter) PopulateTreeViewFolderFilterThread(searchFilterResult);

                GlobalData.IsPopulatingFolderSelected = false;
            }
            

            FilesSelected(); //Even when 0 selected files, allocate data and flags, etc...

            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region PopulatePreloadMetadataQueue
        private void PopulatePreloadMetadataQueue(List<FileEntry> imageListViewFileEntryItems)
        {
            foreach (FileEntry imageListViewItemFileEntryItem in imageListViewFileEntryItems) 
                AddQueuePreloadningMetadata(new FileEntryAttribute(imageListViewItemFileEntryItem, FileEntryVersion.Current));
        }
        #endregion
        
        #endregion


    }
}

