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
        private void FolderSelected(bool recursive, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingFolderSelected) 
            {
                ImageListViewClearAll(imageListView1);
                GlobalData.IsPopulatingFolderSelected = false;
            }
            if (GlobalData.IsPopulatingAnything()) return;

            using (new WaitCursor())
            {
                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
                GlobalData.SearchFolder = true;

                folderTreeViewFolder.Enabled = false;               
                ImageListViewAggregateWithFilesFromFolder(this.folderTreeViewFolder.GetSelectedNodePath(), recursive);
                folderTreeViewFolder.Enabled = true;
                if (runPopulateFilter) PopulateTreeViewFolderFilterThread(imageListView1.Items);
                PopulatePreloadMetadataQueue(imageListView1.Items);


                GlobalData.IsPopulatingFolderSelected = false;
            }

            FilesSelected(); //Even when 0 selected files, allocate data and flags, etc...

            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region FolderSearchFilter - Populate DataGridView, ImageListView 
        private void FolderSearchFilter(List<string> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
                GlobalData.SearchFolder = false;

                folderTreeViewFolder.Enabled = false;
                ImageListViewAggregateFromSearchFilter(searchFilterResult);
                folderTreeViewFolder.Enabled = true; //Avoid select folder while loading ImageListView
                if (runPopulateFilter) PopulateTreeViewFolderFilterThread(imageListView1.Items);

                GlobalData.IsPopulatingFolderSelected = false;
            }
            

            FilesSelected(); //Even when 0 selected files, allocate data and flags, etc...

            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region PopulatePreloadMetadataQueue
        private void PopulatePreloadMetadataQueue(ImageListView.ImageListViewItemCollection imageListViewItems)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewItems) AddQueuePreloadningMetadata(new FileEntryAttribute(imageListViewItem.FileFullPath, imageListViewItem.DateModified, FileEntryVersion.Current));
        }
        #endregion
        
        #endregion


    }
}

