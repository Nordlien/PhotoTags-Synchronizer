using ApplicationAssociations;
using DataGridViewGeneric;
using Exiftool;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        #region Save

        #region Save - IsAnyDataUnsaved
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
        #endregion

        #region Save - ClearDataGridDirtyFlag
        private void ClearDataGridDirtyFlag()
        {
            if (GlobalData.IsAgregatedTags) DataGridViewHandler.ClearDataGridViewDirty(dataGridViewTagsAndKeywords);
            if (GlobalData.IsAgregatedMap) DataGridViewHandler.ClearDataGridViewDirty(dataGridViewMap);
            if (GlobalData.IsAgregatedPeople) DataGridViewHandler.ClearDataGridViewDirty(dataGridViewPeople);
            if (GlobalData.IsAgregatedDate) DataGridViewHandler.ClearDataGridViewDirty(dataGridViewDate);
        }
        #endregion

        #region Save - GetDataGridViewData
        private void GetDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView)
        {
            metadataListOriginalExiftool = new List<Metadata>();
            metadataListFromDataGridView = new List<Metadata>();

            DataGridView dataGridView = GetDataGridViewForTag(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            List<DataGridViewGenericColumn> dataGridViewGenericColumnList = DataGridViewHandler.GetColumnDataGridViewGenericColumnList(dataGridView, true);
            foreach (DataGridViewGenericColumn dataGridViewGenericColumn in dataGridViewGenericColumnList)
            {
                if (dataGridViewGenericColumn.Metadata == null) continue;

                Metadata metadataFromDataGridView = new Metadata(dataGridViewGenericColumn.Metadata);

                if (GlobalData.IsAgregatedTags) DataGridViewHandlerTagsAndKeywords.GetUserInputChanges(ref dataGridViewTagsAndKeywords, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryAttribute);                
                if (GlobalData.IsAgregatedMap) DataGridViewHandlerMap.GetUserInputChanges(ref dataGridViewMap, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryAttribute);               
                if (GlobalData.IsAgregatedPeople) DataGridViewHandlerPeople.GetUserInputChanges(ref dataGridViewPeople, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryAttribute);                
                if (GlobalData.IsAgregatedDate) DataGridViewHandlerDate.GetUserInputChanges(ref dataGridViewDate, metadataFromDataGridView, dataGridViewGenericColumn.FileEntryAttribute);
                
                metadataListFromDataGridView.Add(new Metadata(metadataFromDataGridView));
                metadataListOriginalExiftool.Add(new Metadata(dataGridViewGenericColumn.Metadata));

                dataGridViewGenericColumn.Metadata = metadataFromDataGridView; //Updated the column with Metadata according to the user input
            }
        }
        #endregion

        #region Save - SaveDataGridViewMetadata
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

            ClearDataGridDirtyFlag(); //Clear before save; To track if become dirty during save process
            foreach (int updatedRecord in listOfUpdates)
            {
                //Add only metadata to save queue that that has changed by users
                AddQueueSaveMetadataUpdatedByUser(metadataListFromDataGridView[updatedRecord], metadataListOriginalExiftool[updatedRecord]);
            }
            ThreadSaveMetadata();
        }
        #endregion

        #region Save - SaveProperties
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
                            "File: " + dataGridViewGenericColumn.FileEntryAttribute.FileFullPath + "\r\n\r\n" +
                            "Error message: " + ex.Message + "\r\n";

                        AddError(
                            dataGridViewGenericColumn.FileEntryAttribute.Directory,
                            dataGridViewGenericColumn.FileEntryAttribute.FileName,
                            dataGridViewGenericColumn.FileEntryAttribute.LastWriteDateTime,
                            AddErrorPropertiesRegion, AddErrorPropertiesCommandWrite, AddErrorPropertiesParameterWrite, AddErrorPropertiesParameterWrite,
                            writeErrorDesciption);
                        Logger.Error(ex.Message);
                    }
                }
            }

            GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
            ImageListViewReloadThumbnailInvoke(imageListView1, null);
            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListView1.SelectedItems);
            //GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
            FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems);
        }
        #endregion

        #region Save - SaveActiveTabData
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
        #endregion

        #endregion

        #region ToolStrip - Save
        #region ToolStrip - Save All Metadata - Click 
        private void toolStripButtonSaveAllMetadata_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode
            SaveActiveTabData();
            this.Enabled = true;
        }
        #endregion

        #region ToolStrip - Save People - Click
        private void toolStripMenuItemPeopleSave_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode
            SaveActiveTabData();
            this.Enabled = true;
        }
        #endregion

        #region ToolStrip - Save Tags - Click
        private void toolStripMenuTagsBrokerSave_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode
            SaveActiveTabData();
            this.Enabled = true;
        }
        #endregion

        #region ToolStrip - Save Map - Click
        private void toolStripMenuItemMapSave_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode
            SaveActiveTabData();
            this.Enabled = true;
        }
        #endregion

        #endregion

        #region ToolStrip - About - Click
        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {

            FormAbout form = new FormAbout();
            form.ShowDialog();
            form.Dispose();
        }
        #endregion

        #region ToolStrip - Import Google Locations - Click
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

        #region ToolStrip - Refreh Folder - Click
        private void toolStripMenuItemRefreshFolder_Click(object sender, EventArgs e)
        {
            PopulateImageListViewBasedOnSelectedFolderAndOrFilter(false, true);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region ToolStrip - Select all Items - Click
        private void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            imageListView1.SelectAll();
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            FilesSelected();
        }
        #endregion

        #region ToolStrip - Switch Renderers - Click
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

        #region ToolStrip - Switch View Modes - Click
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
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(defaultImageListViewRenderer.Type.FullName) as ImageListView.ImageListViewRenderer;
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
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(defaultImageListViewRenderer.Type.FullName) as ImageListView.ImageListViewRenderer;
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
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(defaultImageListViewRenderer.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }
        #endregion

        #region ToolStrip - Modify Column Headers - Click
        private void columnsToolStripButton_Click(object sender, EventArgs e)
        {
            ChooseColumns form = new ChooseColumns();
            form.imageListView = imageListView1;
            form.ShowDialog();
        }
        #endregion

        #region ToolStrip - Change Thumbnail Size - Click

        private void toolStripButtonThumbnailSize1_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[4];
            Properties.Settings.Default.ThumbmailViewSizeIndex = 4;
            Properties.Settings.Default.Save();
        }

        private void toolStripButtonThumbnailSize2_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[3];
            Properties.Settings.Default.ThumbmailViewSizeIndex = 3;
            Properties.Settings.Default.Save();
        }

        private void toolStripButtonThumbnailSize3_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[2];
            Properties.Settings.Default.ThumbmailViewSizeIndex = 2;
            Properties.Settings.Default.Save();
        }

        private void toolStripButtonThumbnailSize4_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[1];
            Properties.Settings.Default.ThumbmailViewSizeIndex = 1;
            Properties.Settings.Default.Save();
        }

        private void toolStripButtonThumbnailSize5_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[0];
            Properties.Settings.Default.ThumbmailViewSizeIndex = 0;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region ToolStrip - Rotate Selected Images - Click
        private void rotateCCWToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
                "PhotoTagsSynchronizerApplication", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                
                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    item.BeginEdit();
                    using (Image img = Manina.Windows.Forms.Utility.LoadImageWithoutLock(item.FileFullPath))
                    {
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        img.Save(item.FileFullPath);
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
                    using (Image img = Manina.Windows.Forms.Utility.LoadImageWithoutLock(item.FileFullPath))
                    {
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        img.Save(item.FileFullPath);
                    }
                    item.Update();
                    item.EndEdit();
                }
                
            }
        }
        #endregion

        #region ToolStrip - SetGridViewSize Small Medium Big - Click
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

        #region ToolStrip - Show Config Window - Click
        private void toolStripButtonConfig_Click(object sender, EventArgs e)
        {
            using (Config config = new Config())
            {
                exiftoolReader.MetadataReadPrioity.ReadOnlyOnce();
                config.MetadataReadPrioity = exiftoolReader.MetadataReadPrioity;
                config.ThumbnailSizes = thumbnailSizes;
                config.Init();
                config.ShowDialog();
                ThumbnailSaveSize = Properties.Settings.Default.ApplicationThumbnail;
                databaseLocationAddress.PreferredLanguagesString = Properties.Settings.Default.ApplicationPreferredLanguages;
                RegionStructure.SetAcceptRegionMissmatchProcent((float)Properties.Settings.Default.RegionMissmatchProcent);
            }
        }
        #endregion

        #region ToolStrip - Show/Hide Historiy Columns - Click
        private void toolStripButtonHistortyColumns_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowHistortyColumns = toolStripButtonHistortyColumns.Checked;
            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);
            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListView1.SelectedItems);
        }
        #endregion

        #region ToolStrip - Show/Hide Error Columns - Click
        private void toolStripButtonErrorColumns_CheckedChanged(object sender, EventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Properties.Settings.Default.ShowErrorColumns = toolStripButtonErrorColumns.Checked;
            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);
            LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListView1.SelectedItems);
            stopWatch.Stop();
            Debug.WriteLine("_CheckedChanged:" + stopWatch.Elapsed.ToString());
        }
        #endregion

        #region ToolStrip - AutoCorrect - Folder - Click
        private void toolStripMenuItemTreeViewFolderAutoCorrectMetadata_Click(object sender, EventArgs e)
        {
            AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
            string selectedFolder = folderTreeViewFolder.GetSelectedNodePath();
            string[] files = Directory.GetFiles(selectedFolder, "*.*");
            foreach (string file in files)
            {
                Metadata metadataOriginal = new Metadata(MetadataBrokerType.Empty);
                Metadata metadataToSave = autoCorrect.FixAndSave(
                    new FileEntry(file, File.GetLastWriteTime(file)),
                    databaseAndCacheMetadataExiftool,
                    databaseAndCacheMetadataMicrosoftPhotos,
                    databaseAndCacheMetadataWindowsLivePhotoGallery,
                    databaseAndCahceCameraOwner,
                    databaseLocationAddress,
                    databaseGoogleLocationHistory);
                if (metadataToSave != null)
                {
                    AddQueueSaveMetadataUpdatedByUser(metadataToSave, metadataOriginal);
                    AddQueueRename(file, autoCorrect.RenameVariable); //Properties.Settings.Default.AutoCorrect.)
                }
            }
            StartThreads();
        }
        #endregion 

        #region ToolStrip - AutoCorrect - Selected files - Click
        private void toolStripMenuItemImageListViewAutoCorrect_Click(object sender, EventArgs e)
        {
            AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
 
            foreach (ImageListViewItem item in imageListView1.SelectedItems)
            {
                Metadata metadataOriginal = new Metadata(MetadataBrokerType.Empty);
                Metadata metadataToSave = autoCorrect.FixAndSave(
                    new FileEntry(item.FileFullPath, item.DateModified),
                    databaseAndCacheMetadataExiftool,
                    databaseAndCacheMetadataMicrosoftPhotos,
                    databaseAndCacheMetadataWindowsLivePhotoGallery,
                    databaseAndCahceCameraOwner,
                    databaseLocationAddress,
                    databaseGoogleLocationHistory);
                if (metadataToSave != null)
                {
                    AddQueueSaveMetadataUpdatedByUser(metadataToSave, metadataOriginal);
                    AddQueueRename(item.FileFullPath, autoCorrect.RenameVariable); //Properties.Settings.Default.AutoCorrect.)
                }
            }
            
            StartThreads();
        }
        #endregion

        #region ToolStrip - OpenWith Dialog - Click
        private void openWithDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
            {
                ApplicationActivation.ShowOpenWithDialog(imageListViewItem.FileFullPath);
            }
        
        }
        #endregion

        #region ToolStrip - OpenWith Associated Application - Click
        private void openFileWithAssociatedApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errorMessage = "";

            foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
            {
                try
                {
                    ApplicationActivation.ProcessRunOpenFile(imageListViewItem.FileFullPath);
                }
                catch (Exception ex) { errorMessage += (errorMessage == "" ? "" : "\r\n") + ex.Message; }
            }
           
            if (errorMessage != "") MessageBox.Show(errorMessage, "Failed to start application process...", MessageBoxButtons.OK);
        }
        #endregion

        #region ToolStrip - EditWith Associated Application - Click
        private void editFileWithAssociatedApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errorMessage = "";

            foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
            {
                try
                {
                    ApplicationActivation.ProcessRunEditFile(imageListViewItem.FileFullPath);
                }
                catch (Exception ex) { errorMessage += (errorMessage == "" ? "" : "\r\n") + ex.Message; }
            }

            if (errorMessage != "") MessageBox.Show(errorMessage, "Failed to start application process...", MessageBoxButtons.OK);
        }
        #endregion

        #region ToolStrip - Open File Location - Click
        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errorMessage = "";

            foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
            {
                try
                {
                    ApplicationActivation.ShowFileInExplorer(imageListViewItem.FileFullPath);
                }
                catch (Exception ex) { errorMessage += (errorMessage == "" ? "" : "\r\n") + ex.Message; }
            }
            
            if (errorMessage != "") MessageBox.Show(errorMessage, "Failed to start application process...", MessageBoxButtons.OK);
        }
        #endregion

        #region ToolStrip - Open Folder Location - Click
        private void openFolderLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationActivation.ShowFolderInEplorer(folderTreeViewFolder.GetSelectedNodePath());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Failed to start application process...", MessageBoxButtons.OK); }
        }
        #endregion

        #region ToolStrip - Copy Filenames to Clipboard - Click
        private void copyFileNamesToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageListView1.SelectedItems.Count > 0)
            {
                string text = "";

                foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
                {
                    text = text + (text == "" ? "" : "\r\n") + imageListViewItem.FileFullPath;
                }
                
                Clipboard.SetText(text);
            }
        }
        #endregion

        #region ToolStrip - OpenWith / Run - Advance Dialog - Click
        private void runSelectedLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageListView1.SelectedItems.Count > 0)
            {
                string writeMetadataTagsVariable = Properties.Settings.Default.WriteMetadataTags;
                string writeMetadataKeywordDeleteVariable = Properties.Settings.Default.WriteMetadataKeywordDelete;
                string writeMetadataKeywordAddVariable = Properties.Settings.Default.WriteMetadataKeywordAdd;

                List<string> allowedFileNameDateTimeFormats = FileDateTime.FileDateTimeReader.ConvertStringOfDatesToList(Properties.Settings.Default.RenameDateFormats);

                #region Create ArgumentFile file
                GetDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView);

                ExiftoolWriter.CreateExiftoolArguFileText(
                    metadataListFromDataGridView, metadataListOriginalExiftool, allowedFileNameDateTimeFormats, writeMetadataTagsVariable, writeMetadataKeywordDeleteVariable, writeMetadataKeywordAddVariable,
                    true, out string exiftoolAgruFileText);
                #endregion 

                #region AutoCorrect
                AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect); ;

                List<Metadata> metadataListEmpty = new List<Metadata>();
                List<Metadata> metadataListFromDataGridViewAutoCorrect = new List<Metadata>();

                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    Metadata metadataOriginal = new Metadata(MetadataBrokerType.Empty);
                    Metadata metadataToSave = autoCorrect.FixAndSave(
                        new FileEntry(item.FileFullPath, item.DateModified),
                        databaseAndCacheMetadataExiftool,
                        databaseAndCacheMetadataMicrosoftPhotos,
                        databaseAndCacheMetadataWindowsLivePhotoGallery,
                        databaseAndCahceCameraOwner,
                        databaseLocationAddress,
                        databaseGoogleLocationHistory);

                    metadataListFromDataGridViewAutoCorrect.Add(new Metadata(metadataToSave));
                    metadataListEmpty.Add(new Metadata(metadataOriginal));
                }
                

                ExiftoolWriter.CreateExiftoolArguFileText(
                    metadataListFromDataGridViewAutoCorrect, metadataListEmpty, allowedFileNameDateTimeFormats,
                    writeMetadataTagsVariable, writeMetadataKeywordDeleteVariable, writeMetadataKeywordAddVariable,
                    true, out string exiftoolAutoCorrectFileText);
                #endregion 

                using (RunCommand runCommand = new RunCommand())
                {
                    runCommand.ArguFile = exiftoolAgruFileText;
                    runCommand.ArguFileAutoCorrect = exiftoolAutoCorrectFileText;
                    runCommand.MetadatasGridView = metadataListFromDataGridView;
                    runCommand.MetadatasOriginal = metadataListOriginalExiftool;
                    runCommand.MetadatasEmpty = metadataListEmpty;
                    runCommand.AllowedFileNameDateTimeFormats = allowedFileNameDateTimeFormats;
                    runCommand.MetadataPrioity = exiftoolReader.MetadataReadPrioity;

                    runCommand.Init();
                    runCommand.ShowDialog();
                }


            }
        }
        #endregion

        #region ToolStrip - Refresh - Folder tree - Click
        private void toolStripMenuItemTreeViewFolderRefreshFolder_Click(object sender, EventArgs e)
        {
            GlobalData.DoNotRefreshImageListView = true;
            TreeNode selectedNode = folderTreeViewFolder.SelectedNode;
            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, selectedNode);
            GlobalData.DoNotRefreshImageListView = false;
            PopulateImageListViewBasedOnSelectedFolderAndOrFilter(false, true);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region ToolStrip - Refresh - Items in listview 
        private void toolStripMenuItemTreeViewFolderReadSubfolders_Click(object sender, EventArgs e)
        {
            PopulateImageListViewBasedOnSelectedFolderAndOrFilter(true, true);
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region ToolStrip - Reload Metadata - Selected items - Click
        private void toolStripMenuItemReloadThumbnailAndMetadata_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            //if (GlobalData.IsAgredagedGridViewAny()) return;
            GlobalData.IsPopulatingButtonAction = true;
            GlobalData.IsPopulatingImageListView = true; //Avoid one and one select item getting refreshed
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;

            folderTreeViewFolder.Enabled = false;

            ImageListViewSuspendLayoutInvoke(imageListView1);
            filesCutCopyPasteDrag.DeleteFilesMetadataBeforeReload(folderTreeViewFolder, imageListView1, imageListView1.Items, true);
            filesCutCopyPasteDrag.ImageListViewReload(folderTreeViewFolder, imageListView1, imageListView1.Items, true);

            folderTreeViewFolder.Enabled = true;

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            GlobalData.IsPopulatingButtonAction = false;
            GlobalData.IsPopulatingImageListView = false;

            FilesSelected();
            ImageListViewResumeLayoutInvoke(imageListView1);

            DataGridView dataGridView = GetDataGridViewForTag(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            if (dataGridView != null) DataGridViewHandler.Focus(dataGridView);

        }
        #endregion 

        #region ToolStrip - Reload Metadata - Folder - Click
        private void toolStripMenuItemTreeViewFolderReload_Click(object sender, EventArgs e)
        {
            if (GlobalData.IsPopulatingAnything()) return;
            //if (GlobalData.IsAgredagedGridViewAny()) return;
            GlobalData.IsPopulatingButtonAction = true;
            GlobalData.IsPopulatingImageListView = true; //Avoid one and one select item getting refreshed
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;

            folderTreeViewFolder.Enabled = false;

            ImageListViewSuspendLayoutInvoke(imageListView1);
            filesCutCopyPasteDrag.DeleteFilesMetadataBeforeReload(folderTreeViewFolder, imageListView1, imageListView1.Items, false);
            filesCutCopyPasteDrag.ImageListViewReload(folderTreeViewFolder, imageListView1, imageListView1.Items, false);

            folderTreeViewFolder.Enabled = true;

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            GlobalData.IsPopulatingButtonAction = false;
            GlobalData.IsPopulatingImageListView = false;

            FilesSelected();
            ImageListViewResumeLayoutInvoke(imageListView1);

            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region ToolStrip - Delete Metadata Hirstory - Selected Items - Click
        private void toolStripMenuItemReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory_Click(object sender, EventArgs e)
        {
            filesCutCopyPasteDrag.ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(folderTreeViewFolder, imageListView1);
            FilesSelected();
            DisplayAllQueueStatus();
        }
        #endregion

        #region ToolStrip - Delete Metadata Hirstory - Directory - Click
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

            ImageListViewClearThumbnailCache(imageListView1);
            imageListView1.Refresh();

            Application.DoEvents();
            _ = ImageListViewAggregateWithFilesFromFolder(this.folderTreeViewFolder.GetSelectedNodePath(), false);

            DisplayAllQueueStatus();
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region ToolStrip - Delete Files - Items - Click
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
            
            DisplayAllQueueStatus();

        }
        #endregion

        #region ToolStrip - OpenWith - Click
        private void ToolStripMenuItemOpenWith_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            if (toolStripMenuItem.Tag is ApplicationData applicationData)
            {

                foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
                {
                    try
                    {
                        if (applicationData.VerbLinks.Count >= 1) ApplicationActivation.ProcessRun(applicationData.VerbLinks[0].Command, applicationData.ApplicationId, imageListViewItem.FileFullPath, applicationData.VerbLinks[0].Verb, false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Can execute file. Error: " + ex.Message, "Can execute file", MessageBoxButtons.OK);
                    }
                }
                
            }
        }
        #endregion 


        #region ImageListView
        
        #region ImageListView - Delete Files - Directory - Click
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
                    PopulateImageListViewBasedOnSelectedFolderAndOrFilter(false, true);
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

        #region ImageListView - Cut - Click
        private void toolStripMenuItemImageListViewCut_Click(object sender, EventArgs e)
        {
            var droplist = new StringCollection();

            foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FileFullPath);
            
            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Move);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region ImageListView - Copy - Click
        private void toolStripMenuItemImageListViewCopy_Click(object sender, EventArgs e)
        {
            StringCollection droplist = new StringCollection();
            foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FileFullPath);
            
            DataObject data = new DataObject();
            data.SetFileDropList(droplist);
            data.SetData("Preferred DropEffect", DragDropEffects.Copy);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);

            folderTreeViewFolder.Focus();
        }
        #endregion 

        #region ImageListView - Paste - Click
        private void toolStripMenuItemImageListViewPaste_Click(object sender, EventArgs e)
        {
            StringCollection files = Clipboard.GetFileDropList();
            foreach (string fullFilename in files)
            {
                bool fileFound = false;


                foreach (ImageListViewItem item in imageListView1.Items)
                {
                    if (item.FileFullPath == fullFilename)
                    {
                        fileFound = true;
                        break;
                    }
                }

                if (!fileFound) imageListView1.Items.Add(fullFilename);
            }

        }
        #endregion

        #region ImageListView - DoubleClick
        private void imageListView1_ItemDoubleClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ApplicationActivation.ProcessRunOpenFile(e.Item.FileFullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed to start application process...", MessageBoxButtons.OK);
            }
        }
        #endregion 
        
        #endregion 

        #region FoldeTree
        
        #region FolderTree - Cut - Click
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

        #region FolderTree - Copy - Click
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

        #region FolderTree - Paste - Click
        private void toolStripMenuItemTreeViewFolderPaste_Click(object sender, EventArgs e)
        {
            DragDropEffects dragDropEffects = DetectCopyOrMove();
            CopyOrMove(dragDropEffects, currentNodeWhenStartDragging, Clipboard.GetFileDropList(), Furty.Windows.Forms.ShellOperations.GetFileDirectory(currentNodeWhenStartDragging));
        }
        #endregion

        #region FolderTree - Folder - Click
        private void folderTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (GlobalData.IsDragAndDropActive) return;
            if (GlobalData.DoNotRefreshImageListView) return;

            PopulateImageListViewBasedOnSelectedFolderAndOrFilter(false, true);
        }
        #endregion 

        #endregion

        #region Drag and Drop
        TreeNode currentNodeWhenStartDragging = null; //Updated by DragEnter
        private bool isInternalDrop = true;

        #region FolderTree - Drag and Drop - Detect Copy Or Move
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

        #region FolderTree - Drag and Drop -  Files or Folders
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
                    MoveFiles(folderTreeViewFolder, imageListView1, files, targetDirectory);
                else
                    CopyFiles(folderTreeViewFolder, files, targetDirectory);

                foreach (string sourceDirectory in directories)
                {
                    string newTagretDirectory = Path.Combine(targetDirectory, new DirectoryInfo(sourceDirectory).Name); //Target directory + dragged (drag&drop) direcotry
                                                                                                                        //TreeNode targetNode = folderTreeViewFolder.SelectedNode;
                    TreeNode sourceNode = folderTreeViewFolder.FindFolder(sourceDirectory);

                    if (dragDropEffects == DragDropEffects.Move)
                        MoveFolder(folderTreeViewFolder, sourceNode, targetNode, sourceDirectory, newTagretDirectory);
                    else
                        CopyFolder(folderTreeViewFolder, targetNode, sourceDirectory, newTagretDirectory);
                }
            }

            folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
            filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, currentNodeWhenStartDragging);
            folderTreeViewFolder.Focus();

        }
        #endregion

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
                            PopulateImageListViewBasedOnSelectedFolderAndOrFilter(false, true);
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
        #endregion 
    }
}
