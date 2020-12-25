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
        private void FolderSearchFilter(List<string> searchFilterResult, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            GlobalData.SearchFolder = false;

            GlobalData.IsPopulatingFolderSelected = true; //Don't start twice

            Application.UseWaitCursor = true;

            folderTreeViewFolder.Enabled = false;
            FolderSelected_AggregateListViewWithFilesFromSearchFilter(searchFilterResult);
            folderTreeViewFolder.Enabled = true; //Avoid select folder while loading ImageListView

            if (runPopulateFilter) PopulateTreeViewFolderFilter(imageListView1.Items);

            Application.UseWaitCursor = false;
            GlobalData.IsPopulatingFolderSelected = false;

            FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems); //Even when 0 selected files, allocate data and flags, etc...


            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }

        #region FolderSelcted Updated views
        private void FolderSelected(bool recursive, bool runPopulateFilter = true)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
            GlobalData.SearchFolder = true;
            Application.UseWaitCursor = true;

            folderTreeViewFolder.Enabled = false;
            FolderSelected_AggregateListViewWithFilesFromFolder(this.folderTreeViewFolder.GetSelectedNodePath(), recursive);
            folderTreeViewFolder.Enabled = true; //Avoid select folder while loading ImageListView
            
            if (runPopulateFilter) PopulateTreeViewFolderFilter(imageListView1.Items);

            Application.UseWaitCursor = false;
            GlobalData.IsPopulatingFolderSelected = false;

            FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems); //Even when 0 selected files, allocate data and flags, etc...
            

            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }

        private void folderTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (GlobalData.IsDragAndDropActive) return;
            if (GlobalData.DoNotRefreshImageListView) return;
            FolderSelected(false);
        }


        private void FolderSelected_AggregateListViewWithFilesFromSearchFilter(List<string> searchFilterResult)
        {
            imageListView1.ClearSelection();
            imageListView1.Items.Clear();
            imageListView1.Enabled = false;
            imageListView1.SuspendLayout();

            bool isAndBetweenFieldTagsFolder = treeViewFilter.Nodes[rootNodeFolder].Checked;
            FilterVerifyer filterVerifyerFolder = new FilterVerifyer(isAndBetweenFieldTagsFolder);
            int valuesCountAdded = filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeViewFilter, rootNodeFolder);

            foreach (string fileFullPath in searchFilterResult)
            {
                if (File.Exists(fileFullPath))
                {                    
                    if (valuesCountAdded > 0) // no filter values added, no need read from database, this fjust for optimize speed
                    {
                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadCache(new FileEntryBroker(fileFullPath, File.GetLastWriteTime(fileFullPath), MetadataBrokerTypes.ExifTool));
                        if (filterVerifyerFolder.VerifyMetadata(metadata)) imageListView1.Items.Add(fileFullPath);
                    }
                    else imageListView1.Items.Add(fileFullPath);                    
                }
            }
            imageListView1.ResumeLayout(true);
            imageListView1.Enabled = true;

            StartThreads();

        }

        //Folder selected after Form load/init, click new folder and clear cache and re-read folder
        private void FolderSelected_AggregateListViewWithFilesFromFolder(string selectedFolder, bool recursive)
        {
            if (Directory.Exists(selectedFolder))
            {
                //fileSystemWatcher.EnableRaisingEvents = false;

                FolderSelected_AddFilesImageListView(selectedFolder, recursive);
                GlobalData.lastReadFolderWasRecursive = recursive;
                StartThreads();
                /*
                fileSystemWatcher.BeginInit();
                fileSystemWatcher.Path = this.folderTreeView1.GetSelectedNodePath();
                fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                fileSystemWatcher.Filter = "*.*";
                fileSystemWatcher.EndInit();

                fileSystemWatcher.EnableRaisingEvents = true;
                */
            }
        }

        private void FolderSelectedNone()
        {
            //imageListView1.ClearSelection();
            imageListView1.Items.Clear();
            imageListView1.Refresh();
        }

        private void FolderSelected_AddFilesImageListView(string selectedFolder, bool recursive)
        {
            Properties.Settings.Default.LastFolder = selectedFolder;
            Properties.Settings.Default.Save();
            FileEntryImage[] filesFoundInDirectory;

            bool isAndBetweenFieldTagsFolder = treeViewFilter.Nodes[rootNodeFolder].Checked;
            FilterVerifyer filterVerifyerFolder = new FilterVerifyer(isAndBetweenFieldTagsFolder);
            int valuesCountAdded = filterVerifyerFolder.ReadValuesFromRootNodesWithChilds(treeViewFilter, rootNodeFolder);

            filesFoundInDirectory = ImageAndMovieFileExtentionsUtility.ListAllMediaFiles(selectedFolder, recursive);

            imageListView1.ClearSelection();
            imageListView1.Items.Clear();
            imageListView1.Enabled = false;
            imageListView1.SuspendLayout();
            for (int fileNumber = 0; fileNumber < filesFoundInDirectory.Length; fileNumber++)
            {
                if (valuesCountAdded > 0) // no filter values added, no need read from database, this fjust for optimize speed
                {
                    Metadata metadata = databaseAndCacheMetadataExiftool.ReadCache(new FileEntryBroker(filesFoundInDirectory[fileNumber], MetadataBrokerTypes.ExifTool));
                    if (filterVerifyerFolder.VerifyMetadata(metadata)) imageListView1.Items.Add(filesFoundInDirectory[fileNumber].FileFullPath);
                }
                else imageListView1.Items.Add(filesFoundInDirectory[fileNumber].FileFullPath);
            }

            imageListView1.ResumeLayout(true);
            imageListView1.Enabled = true;
        }

        #endregion


    }
}

