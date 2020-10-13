using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using MetadataLibrary;
using System.Threading;
using System.Drawing;
using ImageAndMovieFileExtentions;
using Manina.Windows.Forms;
using Exiftool;

namespace PhotoTagsSynchronizer
{
    
    public partial class MainForm : Form
    {
        

        private object folderSecletionLock = new object();
        private const int sleepThread = 30;


        #region FolderSelcted Updated views
        private void FolderSelected(bool recursive)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            
            GlobalData.IsPopulatingFolderSelected = true; //Don't start twice
            
            folderTreeViewFolder.Enabled = false;
            FolderSelected_AggregateListViewWithFilesFromFolder(this.folderTreeViewFolder.GetSelectedNodePath(), recursive);
            folderTreeViewFolder.Enabled = true; //Avoid select folder while loading ImageListView

            GlobalData.IsPopulatingFolderSelected = false;

            FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems); //Even when 0 selected files, allocate data and flags, etc...

            UpdateStatusReadWriteStatus_NeedToBeUpated();
            folderTreeViewFolder.Focus();
        }

        private void folderTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (GlobalData.IsDragAndDropActive) return;
            if (GlobalData.DoNotRefreshImageListView) return;
            FolderSelected(false);
        }

        //Folder selected after Form load/init, click new folder and clear cache and re-read folder
        private void FolderSelected_AggregateListViewWithFilesFromFolder(string selectedFolder, bool recursive)
        {
            if (Directory.Exists(selectedFolder))
            {
                //fileSystemWatcher.EnableRaisingEvents = false;

                FolderSelected_AddFilesImageListView(selectedFolder, recursive);
                
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

            filesFoundInDirectory = ImageAndMovieFileExtentionsUtility.ListAllMediaFiles(selectedFolder, recursive);
            
            imageListView1.ClearSelection();
            imageListView1.Items.Clear();
            imageListView1.Enabled = false;
            imageListView1.SuspendLayout();
            for (int fileNumber = 0; fileNumber < filesFoundInDirectory.Length; fileNumber++)
            {
                imageListView1.Items.Add(filesFoundInDirectory[fileNumber].FullFilePath);
            }

            imageListView1.ResumeLayout(true);
            imageListView1.Enabled = true;
        }

        #endregion


    }
}

