using DataGridViewGeneric;
using Exiftool;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        #region About
        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            FormAbout form = new FormAbout();
            form.ShowDialog();
            form.Dispose();
        }
        #endregion

        #region Import Google Locations
        private void toolStripButtonImportGoogleLocation_Click(object sender, EventArgs e)
        {
            bool showLocationForm = true;
            if (IsAnyDataUnsaved())
            {
                if (MessageBox.Show("Will you continue, all unsaved data will be lost?", "You have unsaved data", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    showLocationForm = false;
            }
            
            if (showLocationForm)
            {
                LocationHistoryImportForm form = new LocationHistoryImportForm();
                form.databaseTools = databaseUtilitiesSqliteMetadata;
                form.databaseAndCahceCameraOwner = databaseAndCahceCameraOwner;
                form.Init();

                if (form.ShowDialog() == DialogResult.OK)
                {
                    databaseAndCahceCameraOwner.CameraMakeModelAndOwnerMakeDirty();
                    databaseAndCahceCameraOwner.MakeCameraOwnersDirty();
                    //Update DataGridViews
                    FilesSelected();
                }
            }
        }
        #endregion

        #region Save 
        private bool IsAnyDataUnsaved()
        {
            bool isAnyDataUnsaved = false;
            if (GlobalData.IsAgregatedTags) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewTagsAndKeywords);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;
            if (GlobalData.IsAgregatedMap) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewMap);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;
            if (GlobalData.IsAgregatedPeople) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewPeople);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;
            if (GlobalData.IsAgregatedDate) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewDate);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;
            if (GlobalData.IsAgregatedProperties) isAnyDataUnsaved = DataGridViewHandler.IsDataGridViewDirty(dataGridViewProperties);
            if (isAnyDataUnsaved) return isAnyDataUnsaved;
            
            GetDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView);

            //Find what columns are updated / changed by user
            List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
            return (listOfUpdates.Count > 0);
        }

        private void GetDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView)
        {
            metadataListOriginalExiftool = new List<Metadata>();
            metadataListFromDataGridView = new List<Metadata>();

            DataGridView dataGridView = GetActiveDataGridView();
            List<DataGridViewGenericColumn> dataGridViewGenericColumnList = DataGridViewHandler.GetColumnDataGridViewGenericColumnList(dataGridView, true);
            foreach (DataGridViewGenericColumn dataGridViewGenericColumn in dataGridViewGenericColumnList)
            {
                if (dataGridViewGenericColumn.Metadata == null) continue;

                Metadata metadataFromDataGridView = new Metadata(dataGridViewGenericColumn.Metadata);

                if (GlobalData.IsAgregatedTags)
                    DataGridViewHandlerTagsAndKeywords.GetUserInputChanges(ref dataGridViewTagsAndKeywords, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryImage);

                if (GlobalData.IsAgregatedMap)
                    DataGridViewHandlerMap.GetUserInputChanges(ref dataGridViewMap, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryImage);

                if (GlobalData.IsAgregatedPeople)
                    DataGridViewHandlerPeople.GetUserInputChanges(ref dataGridViewPeople, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryImage);

                if (GlobalData.IsAgregatedDate)
                    DataGridViewHandlerDate.GetUserInputChanges(ref dataGridViewDate, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryImage);

                metadataListFromDataGridView.Add(metadataFromDataGridView);
                metadataListOriginalExiftool.Add(dataGridViewGenericColumn.Metadata);
            }
        }

        private void SaveDataGridViewMetadata()
        {
            if (GlobalData.IsPopulatingAnything())
            {
                MessageBox.Show("Data is populating, please try a bit later.");
                return;
            }
            if (!GlobalData.IsAgredagedGridViewAny())
            {
                MessageBox.Show("No metadata are updated.");
                return;
            }

            GetDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView);

            //Find what columns are updated / changed by user
            List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
            if (listOfUpdates.Count == 0)
            {
                MessageBox.Show("Can't find any value that was changed. Nothing is saved...");
                return;
            }

            foreach (int updatedRecord in listOfUpdates)
            {
                //Add only metadata to save queue that that has changed by users
                AddQueueSaveMetadataUpdatedByUser(metadataListFromDataGridView[updatedRecord], metadataListOriginalExiftool[updatedRecord]);
            }
            ThreadSaveMetadata();

        }

        private void SaveProperties()
        {
            DataGridView dataGridView = dataGridViewProperties;
            int columnCount = DataGridViewHandler.GetColumnCount(dataGridView);
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericColumn != null)
                {
                    try
                    {
                        DataGridViewHandlerProperties.Write(dataGridView, columnIndex);
                    }
                    catch (Exception ex)
                    {
                        string writeErrorDesciption =
                            "Error writing properties to file.\r\n\r\n" +
                            "File: " + dataGridViewGenericColumn.FileEntryImage.FullFilePath + "\r\n\r\n" +
                            "Error message: " + ex.Message + "\r\n";

                        AddError(
                            dataGridViewGenericColumn.FileEntryImage.Directory,
                            dataGridViewGenericColumn.FileEntryImage.FileName,
                            dataGridViewGenericColumn.FileEntryImage.LastWriteDateTime,
                            AddErrorPropertiesRegion, AddErrorPropertiesCommandWrite, AddErrorPropertiesParameterWrite, AddErrorPropertiesParameterWrite,
                            writeErrorDesciption);
                        Logger.Error(ex.Message);
                    }
                }
            }

            GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
            UpdateThumbnailOnImageListViewItems(imageListView1, null);
            UpdateMetadataOnSelectedFilesOnActiveDataGrivView(imageListView1.SelectedItems);
            //GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
            FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems);
        }

        private void SaveActiveTabData()
        {
            if (GlobalData.IsPopulatingAnything()) return;
            if (GlobalData.IsSaveButtonPushed) return;
            GlobalData.IsSaveButtonPushed = true;
            this.Enabled = false;

            switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
            {
                case "ExifTool":
                case "Warning":
                    //Nothing
                    break;
                case "Tags":
                case "Map":
                case "People":
                case "Date":
                    SaveDataGridViewMetadata();
                    GlobalData.IsAgregatedProperties = false;
                    break;
                case "Properties":
                    SaveProperties();
                    break;
                case "Rename":
                    MessageBox.Show("Not implemented");
                    break;
            }
            GlobalData.IsSaveButtonPushed = false;
        }

        private void toolStripButtonSaveAllMetadata_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode
            SaveActiveTabData();
            this.Enabled = true;            
        }

        private void toolStripMenuItemPeopleSave_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode
            SaveActiveTabData();
            this.Enabled = true;
        }

        private void toolStripMenuTagsBrokerSave_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode
            SaveActiveTabData();
            this.Enabled = true;
        }

        private void toolStripMenuItemMapSave_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode
            SaveActiveTabData();
            this.Enabled = true;
        }

        #endregion

        #region Refresh - Folder tree
        private void toolStripMenuItemTreeViewFolderRefreshFolder_Click(object sender, EventArgs e)
        {
            GlobalData.DoNotRefreshImageListView = true;
            TreeNode selectedNode = folderTreeViewFolder.SelectedNode;
            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, selectedNode);
            GlobalData.DoNotRefreshImageListView = false;
            FolderSelected(false);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Refresh - Items in listview 
        private void toolStripMenuItemTreeViewFolderReadSubfolders_Click(object sender, EventArgs e)
        {
            FolderSelected(true);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Reload Metadata - Selected items
        private void toolStripMenuItemReloadThumbnailAndMetadata_Click(object sender, EventArgs e)
        {
            filesCutCopyPasteDrag.DeleteFilesMetadataForReload(folderTreeViewFolder, imageListView1, imageListView1.Items, true);
            imageListView1.Focus();
        }
        #endregion 

        #region Reload Metadata - Folder
        private void toolStripMenuItemTreeViewFolderReload_Click(object sender, EventArgs e)
        {
            filesCutCopyPasteDrag.DeleteFilesMetadataForReload(folderTreeViewFolder, imageListView1, imageListView1.Items, false);

            FilesSelected();
            UpdateStatusReadWriteStatus_NeedToBeUpated();
            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region Delete Metadata Hirstory - Selected Items
        private void toolStripMenuItemReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory_Click(object sender, EventArgs e)
        {
            filesCutCopyPasteDrag.ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(folderTreeViewFolder, imageListView1);

            FilesSelected();
            UpdateStatusReadWriteStatus_NeedToBeUpated();
        }
        #endregion

        #region Delete Metadata Hirstory - Directory

        private void toolStripMenuItemTreeViewFolderClearCache_Click(object sender, EventArgs e)
        {
            string folder = this.folderTreeViewFolder.GetSelectedNodePath();
            string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly).Take(51).ToArray();
            if (MessageBox.Show("Are you sure you will delete **ALL** metadata history in database store for " +
                (fileAndFolderEntriesCount.Length == 51 ? " over 50 + " : fileAndFolderEntriesCount.Length.ToString()) +
                "  number of files.\r\n\r\n" +
                "In the folder: " + folder,
                "You are going to delete all metadata in folder",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                filesCutCopyPasteDrag.DeleteDirectory(folder);
            }

            imageListView1.ClearThumbnailCache();
            imageListView1.Refresh();
            Application.DoEvents();
            FolderSelected_AggregateListViewWithFilesFromFolder(this.folderTreeViewFolder.GetSelectedNodePath(), false);
            UpdateStatusReadWriteStatus_NeedToBeUpated();
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Delete Files - Items
        private void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            folderTreeViewFolder.Enabled = false;
            imageListView1.Enabled = false;

            if (MessageBox.Show("Are you sure you will delete the files", "Files will be deleted!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                filesCutCopyPasteDrag.DeleteSelectedFiles(imageListView1);
                FilesSelected();
            }

            folderTreeViewFolder.Enabled = true;
            imageListView1.Enabled = true;
            
            UpdateStatusReadWriteStatus_NeedToBeUpated();

        }
        #endregion

        #region ImageListView - Delete Files - Directory
        private void toolStripMenuItemTreeViewFolderDelete_Click(object sender, EventArgs e)
        {
            string folder = folderTreeViewFolder.GetSelectedNodePath();
            try
            {

                string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories).Take(51).ToArray();
                if (MessageBox.Show("You are about to delete the folder:\r\n\r\n" +
                    folder + "\r\n\r\n" +
                    "There are " + (fileAndFolderEntriesCount.Length == 51 ? " over 50+" : fileAndFolderEntriesCount.Length.ToString()) + " files found.\r\n\r\n" +
                    "Procced?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    filesCutCopyPasteDrag.DeleteFilesInFolder(folderTreeViewFolder, folder);
                    FolderSelected(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error when delete folder." + ex.Message);
                
                AddError(
                    folder,
                    AddErrorFileSystemRegion, AddErrorFileSystemDeleteFolder, folder, folder,
                    "Was not able to delete folder with files and subfolder!\r\n\r\n" +
                    "From: " + folder + "\r\n\r\n" +
                    "Error message:\r\n" + ex.Message + "\r\n");
            }
            finally
            {
                GlobalData.DoNotRefreshImageListView = false;
            }
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region ImageListView - Cut Click
        private void toolStripMenuItemImageListViewCut_Click(object sender, EventArgs e)
        {
            var droplist = new StringCollection();
            foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FullFileName);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Move);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region ImageListView - Copy Click
        private void toolStripMenuItemImageListViewCopy_Click(object sender, EventArgs e)
        {
            StringCollection droplist = new StringCollection();
            foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FullFileName);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Copy);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region ImageListView - Paste Click
        private void toolStripMenuItemImageListViewPaste_Click(object sender, EventArgs e)
        {
            StringCollection files = Clipboard.GetFileDropList();
            foreach (string fullFilename in files)
            {
                bool fileFound = false;
                foreach (ImageListViewItem item in imageListView1.Items)
                {
                    if (item.FullFileName == fullFilename)
                    {
                        fileFound = true;
                        break;
                    }
                }
                if (!fileFound) imageListView1.Items.Add(fullFilename);
            }

        }
        #endregion 

        #region FolderTree - Cut Click
        private void toolStripMenuItemTreeViewFolderCut_Click(object sender, EventArgs e)
        {
            string folder = Furty.Windows.Forms.ShellOperations.GetFileDirectory(currentNodeWhenStartDragging); // folderTreeViewFolder.GetSelectedNodePath();
            var droplist = new StringCollection();
            droplist.Add(folder);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Move);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }
        #endregion

        #region FolderTree - Copy Click
        private void toolStripMenuItemTreeViewFolderCopy_Click(object sender, EventArgs e)
        {
            string folder = Furty.Windows.Forms.ShellOperations.GetFileDirectory(currentNodeWhenStartDragging); // folderTreeViewFolder.GetSelectedNodePath();
            StringCollection droplist = new StringCollection();
            droplist.Add(folder);

            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Copy);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region FolderTree - Paste Click
        private void toolStripMenuItemTreeViewFolderPaste_Click(object sender, EventArgs e)
        {
            DragDropEffects dragDropEffects = DetectCopyOrMove();
            CopyOrMove(dragDropEffects, currentNodeWhenStartDragging, Clipboard.GetFileDropList(), Furty.Windows.Forms.ShellOperations.GetFileDirectory(currentNodeWhenStartDragging));
        }
        #endregion

        #region Detect Copy Or Move
        private DragDropEffects DetectCopyOrMove()
        {
            try
            {
                IDataObject obj = Clipboard.GetDataObject();
                if (obj == null) return DragDropEffects.None;
                if (obj.GetData("Preferred DropEffect", true) is DragDropEffects effects)
                {
                    return effects;
                }

                obj.GetData(DataFormats.FileDrop);
                MemoryStream stream = (MemoryStream) obj.GetData("Preferred DropEffect", true);
                if (stream != null)
                {
                    int flag = stream.ReadByte();
                    if (flag != 2 && flag != 5) return DragDropEffects.None;

                    if (flag == 2) return DragDropEffects.Move;
                    if (flag == 5) return DragDropEffects.Copy;
                }
                
            } catch (Exception ex)
            {
                Logger.Error("Clipboard failed: " + ex.Message);
            }
            return DragDropEffects.None;
        }
        #endregion

        #region Copy files
        private void CopyFiles(TreeNode targetNode, StringCollection files, string targetNodeDirectory)
        {                       
            foreach (string oldPath in files) //Move all files to target directory 
            {
                string sourceFullFilename = oldPath;
                string filename = Path.GetFileName(sourceFullFilename);
                string targetFullFilename = Path.Combine(targetNodeDirectory, filename);

                try
                {
                    filesCutCopyPasteDrag.CopyFile(sourceFullFilename, targetFullFilename);
                }
                catch (Exception ex)
                {
                    DateTime dateTimeLastWriteTime = DateTime.Now;
                    try
                    {
                        dateTimeLastWriteTime = File.GetLastWriteTime(sourceFullFilename);
                    } catch { }

                    AddError(
                        Path.GetDirectoryName(sourceFullFilename),
                        Path.GetFileName(sourceFullFilename),
                        dateTimeLastWriteTime, sourceFullFilename, targetFullFilename,
                        AddErrorFileSystemRegion, AddErrorFileSystemCopy, 
                        "Failed copying file.\r\n\r\n" +
                        "Error copy file from: " + sourceFullFilename + "\r\n\r\n" +
                        "To file: " + targetFullFilename + "\r\n\r\n" +
                        "Error message: " + ex.Message + "\r\n");
                    Logger.Error("Error when copy file." + ex.Message);
                }
            }

            GlobalData.DoNotRefreshImageListView = true;
            folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
            GlobalData.DoNotRefreshImageListView = false;

            //FolderSelected();
            FilesSelected();
        }
        #endregion

        #region Move Files
        private void MoveFiles(TreeNode targetNode, StringCollection files, string targetNodeDirectory)
        {
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            imageListView1.SuspendLayout();

            foreach (string oldPath in files) //Move all files to target directory 
            {
                string sourceFullFilename = oldPath;
                string filename = Path.GetFileName(sourceFullFilename);
                string targetFullFilename = Path.Combine(targetNodeDirectory, filename);
                try
                {
                    filesCutCopyPasteDrag.MoveFile(sourceFullFilename, targetFullFilename);

                    ImageListViewItem foundItem = FindItemInImageListView(imageListView1.Items, sourceFullFilename);
                    if (foundItem != null) imageListView1.Items.Remove(foundItem);
                }
                catch (Exception ex)
                {

                    DateTime dateTimeLastWriteTime = DateTime.Now;
                    try
                    {
                        dateTimeLastWriteTime = File.GetLastWriteTime(sourceFullFilename);
                    }
                    catch { }
                    AddError(
                        Path.GetDirectoryName(sourceFullFilename),
                        Path.GetFileName(sourceFullFilename),
                        dateTimeLastWriteTime,
                        AddErrorFileSystemRegion, AddErrorFileSystemMove, sourceFullFilename, targetFullFilename,
                        "Failed moving file.\r\n\r\n" +
                        "From:" + sourceFullFilename + "\r\n\r\n" +
                        "To: " + targetFullFilename + "\r\n\r\n" +
                        "Error message: " + ex.Message + "\r\n");
                    Logger.Error("Error when move file." + ex.Message);
                }
            }
            imageListView1.ResumeLayout();
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;

            GlobalData.DoNotRefreshImageListView = true;
            folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
            GlobalData.DoNotRefreshImageListView = false;

            //FolderSelected();
            FilesSelected();
        }
        #endregion

        #region Move Folder
        private void MoveFolder(TreeNode sourceNode, TreeNode targetNode, string sourceDirectory, string targetDirectory)
        {
            if (sourceDirectory == targetDirectory) return; //Can't move into itself. No need for error message

            try
            {

                string[] allSourceFullFilenames = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);

                //----- Move all folder and files -----
                Logger.Trace("Move folder from:" + sourceDirectory + " to: " + targetDirectory);
                System.IO.Directory.Move(sourceDirectory, targetDirectory);

                //------ Clear ImageListView -----
                FolderSelectedNone();

                //------ Update node tree -----
                GlobalData.DoNotRefreshImageListView = true;
                if (sourceNode != null) sourceNode.Remove();
                filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, targetNode);
                GlobalData.DoNotRefreshImageListView = false;

                //------ Update database -----
                foreach (string oldFullFilename in allSourceFullFilenames)
                {
                    string oldFilename = Path.GetFileName(oldFullFilename);
                    string newFullFilename = Path.Combine(targetDirectory, oldFilename);
                    Logger.Trace("Rename from:" + oldFullFilename + " to: " + newFullFilename);
                    databaseAndCacheMetadataExiftool.Move(Path.GetDirectoryName(oldFullFilename), Path.GetFileName(oldFullFilename), Path.GetDirectoryName(newFullFilename), Path.GetFileName(newFullFilename));
                }

                //----- Updated ImageListView with files ------
                FolderSelected(false);

            }
            catch (Exception ex)
            {
                Logger.Error("Error when move folder." + ex.Message);

                AddError(
                    sourceDirectory,
                    AddErrorFileSystemRegion, AddErrorFileSystemMoveFolder, sourceDirectory, targetDirectory,
                    "Failed moving directory.\r\n\r\n" +
                    "From: " + sourceDirectory + "\r\n\r\n" +
                    "To: " + targetDirectory + "\r\n\r\n" +
                    "Error message:\r\n" + ex.Message);

            }
        }
        #endregion

        #region Copy Folder
        private void CopyFolder(TreeNode sourceNode, TreeNode targetNode, string sourceDirectory, string tagretDirectory)
        {

            string[] allSourceFullFilenames = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);

            //----- Create directories and sub-directories
            Directory.CreateDirectory(tagretDirectory);
            foreach (string dirPath in Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories))
            {                
                try
                {
                    Directory.CreateDirectory(dirPath.Replace(sourceDirectory, tagretDirectory));
                }
                catch (SystemException ex)
                {
                    Logger.Error("Error when create directory when copy all files from folder:" + ex.Message);
                    AddError(dirPath, 
                        AddErrorFileSystemRegion, AddErrorFileSystemCreateFolder, dirPath.Replace(sourceDirectory, tagretDirectory), dirPath.Replace(sourceDirectory, tagretDirectory),
                        "Failed create directory\r\n\r\n" + 
                        "Directory: " + dirPath + "\r\n\r\n" + 
                        "Error message: " + ex.Message + "\r\n");

                    
                }
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string sourceFullFilename in allSourceFullFilenames)
            {
                string sourceFilename = Path.GetFileName(sourceFullFilename);
                string targetFullFilename = Path.Combine(tagretDirectory, sourceFilename);
                try
                {
                    Logger.Trace("Copy from:" + sourceFullFilename + " to: " + targetFullFilename);
                    File.Copy(sourceFullFilename, sourceFullFilename.Replace(sourceDirectory, tagretDirectory), false);                    
                    
                    databaseAndCacheMetadataExiftool.Copy(
                        Path.GetDirectoryName(sourceFullFilename), Path.GetFileName(sourceFullFilename), 
                        Path.GetDirectoryName(targetFullFilename), Path.GetFileName(targetFullFilename));
                }
                catch (SystemException ex)
                {
                    DateTime dateTimeLastWriteTime = DateTime.Now;
                    try
                    {
                        dateTimeLastWriteTime = File.GetLastWriteTime(sourceFullFilename);
                    }
                    catch { }
                    AddError(
                        Path.GetDirectoryName(sourceFullFilename),
                        Path.GetFileName(sourceFullFilename),
                        dateTimeLastWriteTime,
                        AddErrorFileSystemRegion, AddErrorFileSystemCopy, sourceFullFilename, targetFullFilename,
                        "Failed copying file.\r\n\r\n" +
                        "Error copy file from: " + sourceFullFilename + "\r\n\r\n" +
                        "To file: " + targetFullFilename + "\r\n\r\n" +
                        "Error message: " + ex.Message + "\r\n");
                }
            }

            //------ Update node tree -----
            GlobalData.DoNotRefreshImageListView = true;
            //if (sourceNode != null) sourceNode.Remove();
            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, targetNode);
            GlobalData.DoNotRefreshImageListView = false;
        }
        #endregion

        #region Copy or Move - Files or Folders
        private void CopyOrMove(DragDropEffects dragDropEffects, TreeNode targetNode, StringCollection fileDropList, string targetDirectory)
        {
            if (dragDropEffects == DragDropEffects.None)
            {
                MessageBox.Show("Was not able to detect if you select copy or cut object that was pasted or dropped");
                return;
            }

            StringCollection files = new StringCollection();
            StringCollection directories = new StringCollection();

            int numberOfFilesAndFolders = 0;
            string copyFromFolders = ""; 
            int countFoldersSelected = 0;

            foreach (string clipbordSourceFileOrDirectory in fileDropList)
            {
                if (File.Exists(clipbordSourceFileOrDirectory))
                {
                    files.Add(clipbordSourceFileOrDirectory);
                    numberOfFilesAndFolders++;
                }
                else if (Directory.Exists(clipbordSourceFileOrDirectory))
                {
                    directories.Add(clipbordSourceFileOrDirectory);
                    if (numberOfFilesAndFolders <= 51)
                    {
                        string[] fileAndFolderEntriesCount = Directory.EnumerateFiles(clipbordSourceFileOrDirectory, "*", SearchOption.AllDirectories).Take(51).ToArray();
                        numberOfFilesAndFolders += fileAndFolderEntriesCount.Length;
                    }

                    countFoldersSelected++;
                    if (countFoldersSelected < 3)
                    {
                        copyFromFolders += clipbordSourceFileOrDirectory + "\r\n";
                        
                    }
                    else if (countFoldersSelected == 4) copyFromFolders += "and more directories...\r\n";
                }
            }

            if (numberOfFilesAndFolders <= 50 || 
                (MessageBox.Show("You are about to " + dragDropEffects.ToString() + " " + (numberOfFilesAndFolders > 50 ? "over 50+" : numberOfFilesAndFolders.ToString()) + " files and/or folders.\r\n\r\n" +
                "From:\r\n" + copyFromFolders + "\r\n\r\n" +
                "To folder:\r\n" + targetDirectory + "\r\n\r\n" +
                "Procced?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK))
            {

                if (dragDropEffects == DragDropEffects.Move)
                    MoveFiles(targetNode, files, targetDirectory);
                else
                    CopyFiles(targetNode, files, targetDirectory);

                foreach (string sourceDirectory in directories)
                {
                    string newTagretDirectory = Path.Combine(targetDirectory, new DirectoryInfo(sourceDirectory).Name); //Target directory + dragged (drag&drop) direcotry
                                                                                                                        //TreeNode targetNode = folderTreeViewFolder.SelectedNode;
                    TreeNode sourceNode = folderTreeViewFolder.FindFolder(sourceDirectory);

                    if (dragDropEffects == DragDropEffects.Move)
                        MoveFolder(sourceNode, targetNode, sourceDirectory, newTagretDirectory);                    
                    else
                        CopyFolder(sourceNode, targetNode, sourceDirectory, newTagretDirectory);
                }  
            }

            folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, currentNodeWhenStartDragging);
            folderTreeViewFolder.Focus();

        }
        #endregion


        TreeNode currentNodeWhenStartDragging = null; //Updated by DragEnter
        private bool isInternalDrop = true;

        #region FolderTree - Drag and Drop - Drop - Move/Copy Files - Move/Copy Folders
        private void folderTreeViewFolder_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = folderTreeViewFolder.PointToClient(new Point(e.X, e.Y)); // Retrieve the client coordinates of the drop location.                          
            TreeNode targetNode = folderTreeViewFolder.GetNodeAt(targetPoint); // Retrieve the node at the drop location.
            string targetDirectory = Furty.Windows.Forms.ShellOperations.GetFileDirectory(targetNode);
            
            #region Move media files dropped to new folder from external source
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); //Check if files been dropped
            if (files != null)
            {
                StringCollection fileCollection = new StringCollection();
                fileCollection.AddRange(files);
                CopyOrMove(e.Effect, targetNode, fileCollection, targetDirectory);
            }
            #endregion

            GlobalData.IsDragAndDropActive = false;
            folderTreeViewFolder.Focus();
            
        }
        #endregion

        #region FolderTree - Drag and Drop - ContainsNode?
        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            // Check the parent node of the second node.  
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            // If the parent node is not null or equal to the first node,   
            // call the ContainsNode method recursively using the parent of   
            // the second node.  
            return ContainsNode(node1, node2.Parent);
        }
        #endregion

        #region FolderTree - Drag and Drop - Node Mouse Click - Set clickedNode
        private void folderTreeViewFolder_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //clickedNode = e.Node;
            currentNodeWhenStartDragging = e.Node;

            if (e.Button == MouseButtons.Right)
            {               
                folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
            }
        }
        #endregion

        #region FolderTree - Drag and Drop - Item Drag - Set Clipboard data to ** TreeViewFolder.Item ** | Move | Copy | Link |
        private void folderTreeViewFolder_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                currentNodeWhenStartDragging = (TreeNode)e.Item;
                Clipboard.Clear();

                if (currentNodeWhenStartDragging != null)
                {
                    string sourceDirectory = Furty.Windows.Forms.ShellOperations.GetFileDirectory(currentNodeWhenStartDragging);
                    var droplist = new StringCollection();
                    droplist.Add(sourceDirectory);

                    DataObject data = new DataObject();
                    data.SetFileDropList(droplist);
                    data.SetData("Preferred DropEffect", DragDropEffects.Move);
                    Clipboard.SetDataObject(data, true);
                    DragDropEffects dragDropEffects = folderTreeViewFolder.DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link); // Allowed effects

                    if (!isInternalDrop)
                    {
                        if (dragDropEffects == DragDropEffects.Move) //Moved a folder to new location in eg. Windows Explorer
                        {
                            imageListView1.ClearSelection();

                            TreeNode sourceNode = currentNodeWhenStartDragging;
                            TreeNode parentNode = currentNodeWhenStartDragging.Parent;
                            if (parentNode == null) folderTreeViewFolder.SelectedNode = folderTreeViewFolder.Nodes[0];

                            //------ Update node tree -----
                            GlobalData.DoNotRefreshImageListView = true;
                            if (sourceNode != null) sourceNode.Remove();
                            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, parentNode);
                            GlobalData.DoNotRefreshImageListView = false;

                            //----- Updated ImageListView with files ------
                            FolderSelected(false);
                        }
                        else //Copied or NOT (cancel) a folder to new location in eg. Windows Explorer
                        {
                            GlobalData.DoNotRefreshImageListView = true;
                            folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging; 
                            GlobalData.DoNotRefreshImageListView = false;
                        } 
                    } 
                }
                else
                {
                    MessageBox.Show("No node folder was selected");
                    folderTreeViewFolder.Focus();
                }
            } 
            catch (Exception ex)
            {
                Logger.Error("Failed create drag and drop tarnsfer data. Error: " + ex.Message);
                MessageBox.Show("Failed create drag and drop tarnsfer data. Error: " + ex.Message);
            }
        }
        #endregion 

        #region FolderTree - Drag and Drop - Drag Leave - Set Clipboard data to ** FileDropList ** | Link |
        private void folderTreeViewFolder_DragLeave(object sender, EventArgs e)
        {
            isInternalDrop = false;

            GlobalData.IsDragAndDropActive = false;

            GlobalData.DoNotRefreshImageListView = true;
            folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
            GlobalData.DoNotRefreshImageListView = false;

            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region FolderTree - Drag and Drop - Drag Enter - update selected node
        private void folderTreeViewFolder_DragEnter(object sender, DragEventArgs e)
        {
            isInternalDrop = true;

            try
            {
                GlobalData.IsDragAndDropActive = true;
                currentNodeWhenStartDragging = folderTreeViewFolder.SelectedNode;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
        }
        #endregion

        #region FolderTree - Drag and Drop - Drag Over - update folderTreeViewFolder.SelectedNode
        private void folderTreeViewFolder_DragOver(object sender, DragEventArgs e)
        {
            isInternalDrop = true;
            try
            {
                if (((System.Windows.DragDropKeyStates)e.KeyState & System.Windows.DragDropKeyStates.ShiftKey) == System.Windows.DragDropKeyStates.ShiftKey)
                    e.Effect = DragDropEffects.Move;
                else if (((System.Windows.DragDropKeyStates)e.KeyState & System.Windows.DragDropKeyStates.RightMouseButton) == System.Windows.DragDropKeyStates.RightMouseButton)
                    e.Effect = DragDropEffects.Copy;
                else if (((System.Windows.DragDropKeyStates)e.KeyState & System.Windows.DragDropKeyStates.ControlKey) == System.Windows.DragDropKeyStates.ControlKey)
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.Move;

                GlobalData.DoNotRefreshImageListView = true;
                // Retrieve the client coordinates of the mouse position.  
                Point targetPoint = folderTreeViewFolder.PointToClient(new Point(e.X, e.Y));

                // Select the node at the mouse position.  
                folderTreeViewFolder.SelectedNode = folderTreeViewFolder.GetNodeAt(targetPoint);
                GlobalData.DoNotRefreshImageListView = false;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.Message);
            }
        }
        #endregion

        #region Refreh Folder
        private void toolStripMenuItemRefreshFolder_Click(object sender, EventArgs e)
        {
            FolderSelected(false);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region Select all - Items
        private void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            imageListView1.SelectAll();
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            FilesSelected();
        }
        #endregion

        #region Switch Renderers
        private struct RendererItem
        {
            public Type Type;

            public override string ToString()
            {
                return Type.Name;
            }

            public RendererItem(Type type)
            {
                Type = type;
            }
        }

        private void renderertoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isFormLoading) return;
            Properties.Settings.Default.RenderertoolStripComboBox = renderertoolStripComboBox.SelectedIndex;
            Properties.Settings.Default.Save();
            // Change the renderer
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            RendererItem item = (RendererItem)renderertoolStripComboBox.SelectedItem;
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(item.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }
        #endregion

        #region Switch View Modes
        private void thumbnailsToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Thumbnails;
            rendererToolStripLabel.Visible = true;
            renderertoolStripComboBox.Visible = true;
            toolStripSeparatorRenderer.Visible = true;

            renderertoolStripComboBox.SelectedIndex = Properties.Settings.Default.RenderertoolStripComboBox;
            renderertoolStripComboBox_SelectedIndexChanged(null, null);
        }

        private void galleryToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Gallery;
            rendererToolStripLabel.Visible = false;
            renderertoolStripComboBox.Visible = false;
            toolStripSeparatorRenderer.Visible = false;

            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(defaultRendererItem.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }

        private void paneToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Pane;
            rendererToolStripLabel.Visible = false;
            renderertoolStripComboBox.Visible = false;
            toolStripSeparatorRenderer.Visible = false;

            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(defaultRendererItem.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }

        private void detailsToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Details;
            rendererToolStripLabel.Visible = false;
            renderertoolStripComboBox.Visible = false;
            toolStripSeparatorRenderer.Visible = false;

            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(defaultRendererItem.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }
        #endregion

        #region Modify Column Headers
        private void columnsToolStripButton_Click(object sender, EventArgs e)
        {
            ChooseColumns form = new ChooseColumns();
            form.imageListView = imageListView1;
            form.ShowDialog();
        }
        #endregion

        #region Change Thumbnail Size

        private void toolStripButtonThumbnailSize1_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[4];
        }

        private void toolStripButtonThumbnailSize2_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[3];
        }

        private void toolStripButtonThumbnailSize3_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[2];
        }

        private void toolStripButtonThumbnailSize4_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[1];
        }

        private void toolStripButtonThumbnailSize5_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[0];
        }
        #endregion

        #region Rotate Selected Images
        private void rotateCCWToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
                "PhotoTagsSynchronizerApplication", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    item.BeginEdit();
                    using (Image img = Manina.Windows.Forms.Utility.LoadImageWithoutLock(item.FullFileName))
                    {
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        img.Save(item.FullFileName);
                    }
                    item.Update();
                    item.EndEdit();
                }
            }
        }

        private void rotateCWToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
                "PhotoTagsSynchronizerApplication", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    item.BeginEdit();
                    using (Image img = Manina.Windows.Forms.Utility.LoadImageWithoutLock(item.FullFileName))
                    {
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        img.Save(item.FullFileName);
                    }
                    item.Update();
                    item.EndEdit();
                }
            }
        }
        #endregion


        #region SetGridViewSize Small Medium Big
        private void SetGridViewSize(DataGridViewSize size)
        {
            switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
            {
                case "Tags":
                    DataGridViewHandler.SetCellSize(dataGridViewTagsAndKeywords, size, false);
                    Properties.Settings.Default.CellSizeKeywords = (int)size;
                    break;
                case "Map":
                    DataGridViewHandler.SetCellSize(dataGridViewMap, size, false);
                    Properties.Settings.Default.CellSizeMap = (int)size;
                    break;
                case "People":
                    DataGridViewHandler.SetCellSize(dataGridViewPeople, size, true);
                    Properties.Settings.Default.CellSizePeoples = (int)size;
                    break;
                case "Date":
                    DataGridViewHandler.SetCellSize(dataGridViewDate, size, false);
                    Properties.Settings.Default.CellSizeDates = (int)size;
                    break;
                case "ExifTool":
                    DataGridViewHandler.SetCellSize(dataGridViewExifTool, size, false);
                    Properties.Settings.Default.CellSizeExiftool = (int)size;
                    break;
                case "Warning":
                    DataGridViewHandler.SetCellSize(dataGridViewExifToolWarning, size, false);
                    Properties.Settings.Default.CellSizeWarnings = (int)size;
                    break;
                case "Properties":
                    DataGridViewHandler.SetCellSize(dataGridViewProperties, size, false);
                    Properties.Settings.Default.CellSizeProperties = (int)size;
                    break;
                case "Rename":
                    DataGridViewHandler.SetCellSize(dataGridViewRename, (size | DataGridViewSize.RenameSize), false);
                    Properties.Settings.Default.CellSizeRename = (int)size;
                    break;
                default:
                    throw new Exception("Not implemented");
            }
        }

        private void toolStripButtonGridBig_Click(object sender, EventArgs e)
        {
            SetGridViewSize(DataGridViewSize.Large);
        }

        private void toolStripButtonGridNormal_Click(object sender, EventArgs e)
        {
            SetGridViewSize(DataGridViewSize.Medium);
        }

        private void toolStripButtonGridSmall_Click(object sender, EventArgs e)
        {
            SetGridViewSize(DataGridViewSize.Small);
        }
        #endregion

        #region Show Config Window
        private void toolStripButtonConfig_Click(object sender, EventArgs e)
        {
            using (Config config = new Config())
            {
                exiftoolReader.MetadataReadPrioity.ReadOnlyOnce();
                config.MetadataReadPrioity = exiftoolReader.MetadataReadPrioity;
                config.ThumbnailSizes = thumbnailSizes;
                config.Init();
                config.ShowDialog();
                maxThumbnailSize = Properties.Settings.Default.ApplicationThumbnail;
            }
        }
        #endregion

        #region Show/Hide Historiy / Error Columns
        private void toolStripButtonHistortyColumns_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowHistortyColumns = toolStripButtonHistortyColumns.Checked;
            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);
            UpdateMetadataOnSelectedFilesOnActiveDataGrivView(imageListView1.SelectedItems);
        }

        private void toolStripButtonErrorColumns_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowErrorColumns = toolStripButtonErrorColumns.Checked;
            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);
            UpdateMetadataOnSelectedFilesOnActiveDataGrivView(imageListView1.SelectedItems);
        }
        #endregion

        #region AutoCorrect
        private void toolStripMenuItemTreeViewFolderAutoCorrectMetadata_Click(object sender, EventArgs e)
        {
            AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
            string selectedFolder = folderTreeViewFolder.GetSelectedNodePath();
            string[] files = Directory.GetFiles(selectedFolder, "*.*");
            foreach (string file in files)
            {
                Metadata metadataOriginal = new Metadata(MetadataBrokerTypes.Empty);
                Metadata metadataToSave = autoCorrect.FixAndSave(
                    new FileEntry(file, File.GetLastWriteTime(file)),
                    databaseAndCacheMetadataExiftool,
                    databaseAndCacheMetadataMicrosoftPhotos,
                    databaseAndCacheMetadataWindowsLivePhotoGallery,
                    databaseAndCahceCameraOwner,
                    databaseLocationAddress,
                    databaseGoogleLocationHistory);
                if (metadataToSave != null) AddQueueSaveMetadataUpdatedByUser(metadataToSave, metadataOriginal);
            }
            StartThreads();
        }


        private void toolStripMenuItemImageListViewAutoCorrect_Click(object sender, EventArgs e)
        {
            AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect); ;
            foreach (ImageListViewItem item in imageListView1.SelectedItems)
            {
                Metadata metadataOriginal = new Metadata(MetadataBrokerTypes.Empty);
                Metadata metadataToSave = autoCorrect.FixAndSave(
                    new FileEntry(item.FullFileName, item.DateModified), 
                    databaseAndCacheMetadataExiftool, 
                    databaseAndCacheMetadataMicrosoftPhotos, 
                    databaseAndCacheMetadataWindowsLivePhotoGallery,
                    databaseAndCahceCameraOwner,
                    databaseLocationAddress,
                    databaseGoogleLocationHistory);
                if (metadataToSave != null) AddQueueSaveMetadataUpdatedByUser(metadataToSave, metadataOriginal);
            }
            StartThreads();
        }
        #endregion

    }
}
