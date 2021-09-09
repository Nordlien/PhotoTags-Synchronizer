using ApplicationAssociations;
using DataGridViewGeneric;
using Exiftool;
using ImageAndMovieFileExtentions;
using LocationNames;
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
using Thumbnails;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region DataGridViewHandler - ShowMediaPosterWindowToolStripMenuItemSelectedEvent
        private void DataGridViewHandlerConvertAndMerge_ShowMediaPosterWindowToolStripMenuItemSelectedEvent(object sender, EventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);

                OpenRegionSelector();
                RegionSelectorLoadAndSelect(dataGridView, dataGridView.CurrentCell.RowIndex, dataGridView.CurrentCell.ColumnIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion 

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

            DataGridView dataGridView = GetActiveTabDataGridView();
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
                AddQueueSaveMetadataUpdatedByUserLock(metadataListFromDataGridView[updatedRecord], metadataListOriginalExiftool[updatedRecord]);
            }
            ThreadSaveMetadata();
        }
        #endregion

        #region Save - SaveProperties
        private void SaveProperties()
        {
            using (new WaitCursor())
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
                            Logger.Error(ex, "SaveProperties");
                        }
                    }
                }

                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
                //ImageListViewReloadThumbnailInvoke(imageListView1, null); //Why null
                LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListView1.SelectedItems);
                FilesSelected();
            }
        }
        #endregion

        #region Save - SaveConvertAndMerge
        private void SaveConvertAndMerge()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.InitialDirectory = @"C:\";      
            saveFileDialog1.Title = "Where to save converted and merged video file";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "mp4";
            saveFileDialog1.Filter = "Video file (*.mp4)|*.mp4|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (new WaitCursor())
                {
                    string outputFile = saveFileDialog1.FileName;

                    DataGridView dataGridView = dataGridViewConvertAndMerge;
                    DataGridViewHandlerConvertAndMerge.Write(dataGridView,
                        Properties.Settings.Default.ConvertAndMergeExecute,
                        Properties.Settings.Default.ConvertAndMergeMusic,
                        (int)Properties.Settings.Default.ConvertAndMergeImageDuration,
                        (int)Properties.Settings.Default.ConvertAndMergeOutputWidth,
                        (int)Properties.Settings.Default.ConvertAndMergeOutputHeight,
                        Properties.Settings.Default.ConvertAndMergeOutputTempfileExtension,

                        Properties.Settings.Default.ConvertAndMergeConcatVideosArguments,
                        Properties.Settings.Default.ConvertAndMergeConcatVideosArguFile,

                        Properties.Settings.Default.ConvertAndMergeConcatImagesArguments,
                        Properties.Settings.Default.ConvertAndMergeConcatImagesArguFile,

                        Properties.Settings.Default.ConvertAndMergeConvertVideosArguments,
                        outputFile);

                    GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                    bool found = false;
                    try
                    {
                        if (File.Exists(outputFile) && new FileInfo(outputFile).Length > 0) found = true;
                    }
                    catch
                    {
                    }
                    if (found) ImageListViewAddItem(outputFile);
                }
            }


        }
        #endregion

        

        #endregion


        #region ToolStrip - AutoCorrect - Folder - Click
        private void toolStripMenuItemTreeViewFolderAutoCorrectMetadata_Click(object sender, EventArgs e)
        {
            try
            {
                AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                string selectedFolder = folderTreeViewFolder.GetSelectedNodePath();
                string[] files = Directory.GetFiles(selectedFolder, "*.*");
                foreach (string file in files)
                {
                    Metadata metadataToSave = autoCorrect.FixAndSave(
                        new FileEntry(file, File.GetLastWriteTime(file)),
                        databaseAndCacheMetadataExiftool,
                        databaseAndCacheMetadataMicrosoftPhotos,
                        databaseAndCacheMetadataWindowsLivePhotoGallery,
                        databaseAndCahceCameraOwner,
                        databaseLocationAddress,
                        databaseGoogleLocationHistory, locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
                        autoKeywordConvertions);
                    if (metadataToSave != null)
                    {
                        AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                        AddQueueRenameLock(file, autoCorrect.RenameVariable); //Properties.Settings.Default.AutoCorrect.)
                    }
                }
                StartThreads();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - AutoCorrect - Selected files - Click
        private void toolStripMenuItemImageListViewAutoCorrectForm_Click(object sender, EventArgs e)
        {
            try
            {
                FormAutoCorrect formAutoCorrect = new FormAutoCorrect();
                if (formAutoCorrect.ShowDialog() == DialogResult.OK)
                {

                    string album = formAutoCorrect.Album;
                    string author = formAutoCorrect.Author;
                    string comments = formAutoCorrect.Comments;
                    string description = formAutoCorrect.Description;
                    string title = formAutoCorrect.Title;
                    List<string> keywords = formAutoCorrect.Keywords;

                    bool useAlbum = formAutoCorrect.UseAlbum;
                    bool useAuthor = formAutoCorrect.UseAuthor;
                    bool useComments = formAutoCorrect.UseComments;
                    bool uselDescription = formAutoCorrect.UseDescription;
                    bool useTitle = formAutoCorrect.UseTitle;
                    

                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    bool writeAlbumOnDescription = autoCorrect.UpdateDescription;

                    foreach (ImageListViewItem item in imageListView1.SelectedItems)
                    {
                        Metadata metadataToSave = autoCorrect.FixAndSave(
                            new FileEntry(item.FileFullPath, item.DateModified),
                            databaseAndCacheMetadataExiftool,
                            databaseAndCacheMetadataMicrosoftPhotos,
                            databaseAndCacheMetadataWindowsLivePhotoGallery,
                            databaseAndCahceCameraOwner,
                            databaseLocationAddress,
                            databaseGoogleLocationHistory,
                            locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted, autoKeywordConvertions);

                        if (metadataToSave != null)
                        {
                            if (useAlbum) metadataToSave.PersonalAlbum = album;
                            if (!useAlbum || string.IsNullOrWhiteSpace(metadataToSave.PersonalAlbum)) metadataToSave.PersonalAlbum = null;

                            if (useAuthor) metadataToSave.PersonalAuthor = author;
                            if (!useAuthor || string.IsNullOrWhiteSpace(metadataToSave.PersonalAuthor)) metadataToSave.PersonalAuthor = null;

                            if (useComments) metadataToSave.PersonalComments = comments;
                            if (!useComments || string.IsNullOrWhiteSpace(metadataToSave.PersonalComments)) metadataToSave.PersonalComments = null;

                            if (uselDescription) metadataToSave.PersonalDescription = description;
                            if (!uselDescription || string.IsNullOrWhiteSpace(metadataToSave.PersonalDescription)) metadataToSave.PersonalDescription = null;

                            if (useTitle) metadataToSave.PersonalTitle = title;
                            if (!useTitle || string.IsNullOrWhiteSpace(metadataToSave.PersonalTitle)) metadataToSave.PersonalTitle = null;

                            #region Description
                            if (writeAlbumOnDescription)
                            {
                                Logger.Debug("AutoCorrectForm: Set Description as Album: " + (metadataToSave?.PersonalAlbum == null ? "null" : metadataToSave?.PersonalAlbum));
                                metadataToSave.PersonalDescription = metadataToSave.PersonalAlbum;
                            }
                            #endregion

                            foreach (string keyword in keywords)
                            {
                                metadataToSave.PersonalKeywordTagsAddIfNotExists(new KeywordTag(keyword), false);
                            }

                            AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                            AddQueueRenameLock(item.FileFullPath, autoCorrect.RenameVariable);
                        }
                    }

                    StartThreads();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemImageListViewAutoCorrect_Click(object sender, EventArgs e)
        {
            try
            {
                AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    Metadata metadataToSave = autoCorrect.FixAndSave(
                        new FileEntry(item.FileFullPath, item.DateModified),
                        databaseAndCacheMetadataExiftool,
                        databaseAndCacheMetadataMicrosoftPhotos,
                        databaseAndCacheMetadataWindowsLivePhotoGallery,
                        databaseAndCahceCameraOwner,
                        databaseLocationAddress,
                        databaseGoogleLocationHistory,
                        locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted, autoKeywordConvertions);
                    if (metadataToSave != null)
                    {
                        AddQueueSaveMetadataUpdatedByUserLock(metadataToSave, new Metadata(MetadataBrokerType.Empty));
                        AddQueueRenameLock(item.FileFullPath, autoCorrect.RenameVariable); //Properties.Settings.Default.AutoCorrect.)
                    }
                }

                StartThreads();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - About - Click
        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            try
            {
                FormAbout form = new FormAbout();
                form.ShowDialog();
                form.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Import Google Locations - Click
        private void toolStripButtonImportGoogleLocation_Click(object sender, EventArgs e)
        {
            try
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
                    using (new WaitCursor())
                    {
                        form.databaseTools = databaseUtilitiesSqliteMetadata;
                        form.databaseAndCahceCameraOwner = databaseAndCahceCameraOwner;
                        form.Init();
                    }
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        databaseAndCahceCameraOwner.CameraMakeModelAndOwnerMakeDirty();
                        databaseAndCahceCameraOwner.MakeCameraOwnersDirty();
                        //Update DataGridViews
                        FilesSelected();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Refreh Folder - Click
        private void toolStripMenuItemRefreshFolder_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateImageListView_FromFolderSelected(false, true);
                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Select all Items - Click
        private void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
                using (new WaitCursor())
                {
                    imageListView1.SelectAll();
                }
                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                FilesSelected();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

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

        #region ToolStrip - InageListView - Switch Renderers 
        private void SetImageListViewRender(Manina.Windows.Forms.View imageListViewViewMode, RendererItem selectedRender)
        {
            try
            {
                Properties.Settings.Default.ImageListViewRendererName = selectedRender.Type.Name;
                Properties.Settings.Default.ImageListViewViewMode = (int)imageListViewViewMode;
                
                imageListView1.View = imageListViewViewMode;                
                Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
                ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(selectedRender.Type.FullName) as ImageListView.ImageListViewRenderer;
                imageListView1.SetRenderer(renderer);
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void KryptonContextMenuItem_Click(object sender, EventArgs e)
        {
            KryptonContextMenuItem kryptonContextMenuItem = (KryptonContextMenuItem)sender;
            SetImageListViewRender(Manina.Windows.Forms.View.Thumbnails, (RendererItem)kryptonContextMenuItem.Tag);           
        }

        #endregion

        #region ToolStrip - ImageListView - Switch View Modes
        
        private void kryptonRibbonGroupButtonImageListViewModeGallery_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Gallery, imageListViewSelectedRenderer);
        }

        private void kryptonRibbonGroupButtonImageListViewModeDetails_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Details, imageListViewSelectedRenderer);
        }

        private void kryptonRibbonGroupButtonImageListViewModePane_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Pane, imageListViewSelectedRenderer);
        }

        private void kryptonRibbonGroupButtonImageListViewModeThumbnails_Click(object sender, EventArgs e)
        {
            SetImageListViewRender(Manina.Windows.Forms.View.Thumbnails, imageListViewSelectedRenderer);
        }

        
        #endregion

        #region ToolStrip - ImageListView - Modify Column Headers - Click
        private void kryptonRibbonGroupButtonImageListViewDetailviewColumns_Click(object sender, EventArgs e)
        {
            try
            {
                FormChooseColumns form = new FormChooseColumns();
                form.imageListView = imageListView1;
                int index = 0;
                if (imageListView1.View == Manina.Windows.Forms.View.Thumbnails) index = 1;
                form.Populate(index);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Change Thumbnail Size - Click

        private void SetThumbnailSize (int size)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[size];
            Properties.Settings.Default.ThumbmailViewSizeIndex = size;
            kryptonRibbonGroupButtonThumbnailSizeXLarge.Checked = (size == 4);
            kryptonRibbonGroupButtonThumbnailSizeLarge.Checked = (size == 3);
            kryptonRibbonGroupButtonThumbnailSizeMedium.Checked = (size == 2);
            kryptonRibbonGroupButtonThumbnailSizeSmall.Checked = (size == 1);
            kryptonRibbonGroupButtonThumbnailSizeXSmall.Checked = (size == 0);
        }

        
        private void kryptonRibbonGroupButtonThumbnailSizeXLarge_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(4);            
        }

        private void kryptonRibbonGroupButtonThumbnailSizeLarge_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(3);           
        }

        private void kryptonRibbonGroupButtonThumbnailSizeMedium_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(2);
        }

        private void kryptonRibbonGroupButtonThumbnailSizeSmall_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(1);
        }

        private void kryptonRibbonGroupButtonThumbnailSizeXSmall_Click(object sender, EventArgs e)
        {
            SetThumbnailSize(0);
        }
        #endregion

        #region ToolStrip - Rotate Selected Images - 90 degrees - Click
        private void rotateCWToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                RotateInit(imageListView1, 90);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rotateCW90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateInit(imageListView1, 90);
        }
        #endregion

        #region ToolStrip - Rotate Selected Images - 180 degrees - Click

        private void rotate180ToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                RotateInit(imageListView1, 180);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rotate180ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RotateInit(imageListView1, 180);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Rotate Selected Images - 270 degrees - Click
        private void rotateCCWToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                RotateInit(imageListView1, 270);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ratateCCW270ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RotateInit(imageListView1, 270);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ToolStrip - WebScraper_Click
        private void toolStripButtonWebScraper_Click(object sender, EventArgs e)
        {
            try
            {
                FormWebScraper formWebScraper = new FormWebScraper();
                formWebScraper.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                formWebScraper.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Media Preview
        private static string imageListViewHoverItem = "";
        private void imageListView1_ItemHover(object sender, ItemHoverEventArgs e)
        {
            try
            {
                if (e.Item != null) imageListViewHoverItem = e.Item.FileFullPath;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MediaPreviewFoldeOrFilterResult(string selectedMediaFilePullPath)
        {
            List<string> listOfMediaFiles = new List<string>();
            for (int itemIndex = 0; itemIndex < imageListView1.SelectedItems.Count; itemIndex++) listOfMediaFiles.Add(imageListView1.SelectedItems[itemIndex].FileFullPath);
            MediaPreviewInit(listOfMediaFiles, selectedMediaFilePullPath);
        }

        private void toolStripButtonPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (imageListView1.SelectedItems.Count > 1) MediaPreviewFoldeOrFilterResult("");
                else
                {
                    List<string> listOfMediaFiles = new List<string>();
                    for (int itemIndex = 0; itemIndex < imageListView1.Items.Count; itemIndex++) listOfMediaFiles.Add(imageListView1.Items[itemIndex].FileFullPath);
                    string selectedMediaFilePullPath = "";
                    if (imageListView1.SelectedItems.Count == 1) selectedMediaFilePullPath = imageListView1.SelectedItems[0].FileFullPath;
                    MediaPreviewInit(listOfMediaFiles, selectedMediaFilePullPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mediaPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MediaPreviewFoldeOrFilterResult(imageListViewHoverItem);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showMediaPosterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = GetActiveTabDataGridView();
            OpenRegionSelector();
            RegionSelectorLoadAndSelect(dataGridView);
        }

        private void MediaPreviewSelectedInDataGridView(DataGridView dataGridView)
        {
            List<string> listOfMediaFiles = new List<string>();

            List<int> selectedColumns = DataGridViewHandler.GetColumnSelected(dataGridView);

            if (selectedColumns.Count <= 1)
            {
                int columnCount = DataGridViewHandler.GetColumnCount(dataGridView);
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    if (dataGridViewGenericColumn?.Metadata?.FileFullPath != null) listOfMediaFiles.Add(dataGridViewGenericColumn?.Metadata?.FileFullPath);
                }
            }
            else
            {
                foreach (int columnIndex in selectedColumns)
                {
                    DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    if (dataGridViewGenericColumn?.Metadata?.FileFullPath != null) listOfMediaFiles.Add(dataGridViewGenericColumn?.Metadata?.FileFullPath);
                }
            }

            string selectedMediaFilePullPath = "";
            DataGridViewCell dataGridViewCell = DataGridViewHandler.GetCellCurrent(dataGridView);
            DataGridViewGenericColumn dataGridViewGenericColumnCurrent = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, dataGridViewCell.ColumnIndex);
            if (dataGridViewGenericColumnCurrent?.Metadata?.FileFullPath != null) selectedMediaFilePullPath = dataGridViewGenericColumnCurrent?.Metadata?.FileFullPath;

            MediaPreviewInit(listOfMediaFiles, selectedMediaFilePullPath);
        }

        private void toolStripMenuItemTagsAndKeywordMediaPreview_Click(object sender, EventArgs e)
        {
            try
            {
                MediaPreviewSelectedInDataGridView(GetActiveTabDataGridView());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemDateMediaPreview_Click(object sender, EventArgs e)
        {
            try
            {
                MediaPreviewSelectedInDataGridView(GetActiveTabDataGridView());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemPeopleMediaPreview_Click(object sender, EventArgs e)
        {
            try
            {
                MediaPreviewSelectedInDataGridView(GetActiveTabDataGridView());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemMapMediaPreview_Click(object sender, EventArgs e)
        {
            try
            {
                MediaPreviewSelectedInDataGridView(GetActiveTabDataGridView());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemMediaPreview_Click(object sender, EventArgs e)
        {
            try
            {
                MediaPreviewSelectedInDataGridView(GetActiveTabDataGridView());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemExiftoolWarningMediaPreview_Click(object sender, EventArgs e)
        {
            try
            {
                MediaPreviewSelectedInDataGridView(GetActiveTabDataGridView());
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        #endregion

        #region ImageListView Sort
        private void ImageListViewSortColumn(ImageListView imageListView, ColumnType columnToSort)
        {
            if (imageListView.SortColumn == columnToSort)
            {
                if (imageListView.SortOrder == SortOrder.Descending) imageListView.SortOrder = SortOrder.Ascending;
                else imageListView.SortOrder = SortOrder.Descending;
            }
            else
            {
                imageListView.SortColumn = columnToSort;
                imageListView.SortOrder = SortOrder.Ascending;
            }
        }

        private void ToolStripMenuItemSortByFilename_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.FileName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByFileCreatedDate_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.FileDateCreated);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByFileModifiedDate_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.FileDateModified);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaDateTaken_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaDateTaken);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaAlbum_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaAlbum);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaTitle_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaTitle);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaDescription_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaDescription);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaComments_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaComment);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaAuthor_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaAuthor);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByMediaRating_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.MediaRating);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByLocationName_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.LocationName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByLocationRegionState_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.LocationRegionState);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByLocationCity_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.LocationCity);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemSortByLocationCountry_Click(object sender, EventArgs e)
        {
            try
            {
                ImageListViewSortColumn(imageListView1, ColumnType.LocationCountry);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - SetGridViewSize Small Medium Big - Click
        private void SetRibbonDataGridViewSizeBottons(DataGridViewSize size, bool enabled)
        {
            switch (size)
            {
                case DataGridViewSize.ConfigSize:
                    break;
                case DataGridViewSize.RenameConvertAndMergeSize:
                    break;
                case DataGridViewSize.Large:
                    kryptonRibbonGroupButtonDataGridViewCellSizeBig.Checked = true;
                    kryptonRibbonGroupButtonDataGridViewCellSizeMedium.Checked = false;
                    kryptonRibbonGroupButtonDataGridViewCellSizeSmall.Checked = false;
                    break;
                case DataGridViewSize.Medium:
                    kryptonRibbonGroupButtonDataGridViewCellSizeBig.Checked = false;
                    kryptonRibbonGroupButtonDataGridViewCellSizeMedium.Checked = true;
                    kryptonRibbonGroupButtonDataGridViewCellSizeSmall.Checked = false;
                    break;
                case DataGridViewSize.Small:
                    kryptonRibbonGroupButtonDataGridViewCellSizeBig.Checked = false;
                    kryptonRibbonGroupButtonDataGridViewCellSizeMedium.Checked = false;
                    kryptonRibbonGroupButtonDataGridViewCellSizeSmall.Checked = true;
                    break;
            }
            kryptonRibbonGroupButtonDataGridViewCellSizeBig.Enabled = enabled;
            kryptonRibbonGroupButtonDataGridViewCellSizeMedium.Enabled = enabled;
            kryptonRibbonGroupButtonDataGridViewCellSizeSmall.Enabled = enabled;
        }

        private void SetGridViewSize(DataGridViewSize size)
        {
            SetRibbonDataGridViewSizeBottons(size, true);

            switch (GetActiveTabTag())
            {
                case LinkTabAndDataGridViewNameTags:
                    DataGridViewHandler.SetCellSize(dataGridViewTagsAndKeywords, size, false);
                    Properties.Settings.Default.CellSizeKeywords = (int)size;
                    break;
                case LinkTabAndDataGridViewNameMap:
                    DataGridViewHandler.SetCellSize(dataGridViewMap, size, false);
                    Properties.Settings.Default.CellSizeMap = (int)size;
                    break;
                case LinkTabAndDataGridViewNamePeople:
                    DataGridViewHandler.SetCellSize(dataGridViewPeople, size, true);
                    Properties.Settings.Default.CellSizePeoples = (int)size;
                    break;
                case LinkTabAndDataGridViewNameDates:
                    DataGridViewHandler.SetCellSize(dataGridViewDate, size, false);
                    Properties.Settings.Default.CellSizeDates = (int)size;
                    break;
                case LinkTabAndDataGridViewNameExiftool:
                    DataGridViewHandler.SetCellSize(dataGridViewExiftool, size, false);
                    Properties.Settings.Default.CellSizeExiftool = (int)size;
                    break;
                case LinkTabAndDataGridViewNameWarnings:
                    DataGridViewHandler.SetCellSize(dataGridViewExiftoolWarning, size, false);
                    Properties.Settings.Default.CellSizeWarnings = (int)size;
                    break;
                case LinkTabAndDataGridViewNameProperties:
                    DataGridViewHandler.SetCellSize(dataGridViewProperties, size, false);
                    Properties.Settings.Default.CellSizeProperties = (int)size;
                    break;
                case LinkTabAndDataGridViewNameRename:
                    DataGridViewHandler.SetCellSize(dataGridViewRename, (size | DataGridViewSize.RenameConvertAndMergeSize), false);
                    Properties.Settings.Default.CellSizeRename = (int)size;
                    break;
                case LinkTabAndDataGridViewNameConvertAndMerge:
                    DataGridViewHandler.SetCellSize(dataGridViewRename, (size | DataGridViewSize.RenameConvertAndMergeSize), false);
                    Properties.Settings.Default.CellSizeConvertAndMerge= (int)size;
                    break;
                default: throw new Exception("Not implemented");
            }
        }


        private void kryptonRibbonGroupButtonDataGridViewCellSizeBig_Click(object sender, EventArgs e)
        {
            try
            {
                SetGridViewSize(DataGridViewSize.Large);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kryptonRibbonGroupButtonDataGridViewCellSizeMedium_Click(object sender, EventArgs e)
        {
            try
            {
                SetGridViewSize(DataGridViewSize.Medium);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kryptonRibbonGroupButtonDataGridViewCellSizeSmall_Click(object sender, EventArgs e)
        {
            try
            {
                SetGridViewSize(DataGridViewSize.Small);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ToolStrip - Show Config Window - Click
        private void toolStripButtonConfig_Click(object sender, EventArgs e)
        {
            try
            {
                using (FormConfig config = new FormConfig())
                {
                    using (new WaitCursor())
                    {
                        LocationNameLookUpCache databaseLocationNames = new LocationNameLookUpCache(databaseUtilitiesSqliteMetadata, Properties.Settings.Default.ApplicationPreferredLanguages);

                        exiftoolReader.MetadataReadPrioity.ReadOnlyOnce();
                        config.MetadataReadPrioity = exiftoolReader.MetadataReadPrioity;
                        config.ThumbnailSizes = thumbnailSizes;
                        config.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                        config.DatabaseLocationNames = databaseLocationNames;
                        config.DatabaseAndCacheLocationAddress = databaseLocationAddress;
                        config.DatabaseUtilitiesSqliteMetadata = databaseUtilitiesSqliteMetadata;
                        config.Init();
                    }
                    if (config.ShowDialog() != DialogResult.Cancel)
                    {
                        ThumbnailSaveSize = Properties.Settings.Default.ApplicationThumbnail;
                        RegionThumbnailHandler.FaceThumbnailSize = Properties.Settings.Default.ApplicationRegionThumbnail;

                        databaseLocationAddress.PreferredLanguagesString = Properties.Settings.Default.ApplicationPreferredLanguages;
                        RegionStructure.SetAcceptRegionMissmatchProcent((float)Properties.Settings.Default.RegionMissmatchProcent);

                        autoKeywordConvertions = AutoKeywordHandler.PopulateList(AutoKeywordHandler.ReadDataSetFromXML());
                        //Cache config
                        cacheNumberOfPosters = (int)Properties.Settings.Default.CacheNumberOfPosters;
                        cacheAllMetadatas = Properties.Settings.Default.CacheAllMetadatas;
                        cacheAllThumbnails = Properties.Settings.Default.CacheAllThumbnails;
                        cacheAllWebScraperDataSets = Properties.Settings.Default.CacheAllWebScraperDataSets;
                        cacheFolderMetadatas = Properties.Settings.Default.CacheFolderMetadatas;
                        cacheFolderThumbnails = Properties.Settings.Default.CacheFolderThumbnails;
                        cacheFolderWebScraperDataSets = Properties.Settings.Default.CacheFolderWebScraperDataSets;

                        //
                        folderTreeViewFolder.Enabled = false;
                        imageListView1.Enabled = false;
                        FilesSelected();
                        folderTreeViewFolder.Enabled = true;
                        imageListView1.Enabled = true;
                        imageListView1.Focus();

                        UpdateColorControls(this, Properties.Settings.Default.ApplicationDarkMode);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Show/Hide Historiy / Error Columns - Click
        private void SetRibbonDataGridViewShowWhatColumns(ShowWhatColumns showWhatColumns, bool enabled = true)
        {
            SetRibbonGridViewColumnsButtonsHistoricalAndError(ShowWhatColumnHandler.ShowHirstoryColumns(showWhatColumns), ShowWhatColumnHandler.ShowErrorColumns(showWhatColumns));
            kryptonRibbonGroupButtonDataGridViewColumnsHistory.Enabled = enabled;
            kryptonRibbonGroupButtonDataGridViewColumnsErrors.Enabled = enabled;
        }

        private void SetRibbonGridViewColumnsButtonsHistoricalAndError(bool showHistorical, bool showErrors)
        {
            kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked = showHistorical;
            kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked = showErrors;            
        }

        private void kryptonRibbonGroupButtonDataGridViewColumnsHistory_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.ShowHistortyColumns = kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked;
                showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked, kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked);
                LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListView1.SelectedItems);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kryptonRibbonGroupButtonDataGridViewColumnsErrors_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.ShowErrorColumns = kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked;
                showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked, kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked);
                LazyLoadPopulateDataGridViewSelectedItemsWithMediaFileVersions(imageListView1.SelectedItems);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - OpenWith Dialog - Click
        private void openWithDialogToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
                {
                    ApplicationActivation.ShowOpenWithDialog(imageListViewItem.FileFullPath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - OpenWith / Run - Advance Dialog - Click
        private void runSelectedLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
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
                    AutoCorrect autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
                    if (autoCorrect == null) MessageBox.Show("AutoCorrect: " + Properties.Settings.Default.AutoCorrect);
                    float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                    float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                    int writeCreatedDateAndTimeAttributeTimeIntervalAccepted = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;

                    List<Metadata> metadataListEmpty = new List<Metadata>();
                    List<Metadata> metadataListFromDataGridViewAutoCorrect = new List<Metadata>();

                    foreach (ImageListViewItem item in imageListView1.SelectedItems)
                    {
                        Metadata metadataToSave = autoCorrect.FixAndSave(
                            new FileEntry(item.FileFullPath, item.DateModified),
                            databaseAndCacheMetadataExiftool,
                            databaseAndCacheMetadataMicrosoftPhotos,
                            databaseAndCacheMetadataWindowsLivePhotoGallery,
                            databaseAndCahceCameraOwner,
                            databaseLocationAddress,
                            databaseGoogleLocationHistory,
                            locationAccuracyLatitude, locationAccuracyLongitude, writeCreatedDateAndTimeAttributeTimeIntervalAccepted, autoKeywordConvertions);

                        if (metadataToSave != null) metadataListFromDataGridViewAutoCorrect.Add(new Metadata(metadataToSave));
                        else
                        {
                            Logger.Warn("Metadata was not loaded for file, check if file is only in cloud:" + item.FileFullPath);
                        }
                        metadataListEmpty.Add(new Metadata(MetadataBrokerType.Empty));
                    }


                    ExiftoolWriter.CreateExiftoolArguFileText(
                        metadataListFromDataGridViewAutoCorrect, metadataListEmpty, allowedFileNameDateTimeFormats,
                        writeMetadataTagsVariable, writeMetadataKeywordDeleteVariable, writeMetadataKeywordAddVariable,
                        true, out string exiftoolAutoCorrectFileText);
                    #endregion

                    using (FormRunCommand runCommand = new FormRunCommand())
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
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Refresh - Folder tree - Click
        private void toolStripMenuItemTreeViewFolderRefreshFolder_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalData.DoNotRefreshImageListView = true;
                TreeNode selectedNode = folderTreeViewFolder.SelectedNode;
                filesCutCopyPasteDrag.RefeshFolderTree(folderTreeViewFolder, selectedNode);
                GlobalData.DoNotRefreshImageListView = false;
                PopulateImageListView_FromFolderSelected(false, true);
                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Refresh - Items in listview 
        private void toolStripMenuItemTreeViewFolderReadSubfolders_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateImageListView_FromFolderSelected(true, true);
                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Reload Metadata - Delete Last Mediadata And Reload
        void DeleteLastMediadataAndReload(ImageListView imageListView, bool updatedOnlySelected)
        {
            try
            {
                if (GlobalData.IsPopulatingAnything()) return;

                using (new WaitCursor())
                {
                    GlobalData.IsPopulatingButtonAction = true;
                    GlobalData.IsPopulatingImageListView = true; //Avoid one and one select item getting refreshed
                    GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
                    folderTreeViewFolder.Enabled = false;
                    ImageListViewSuspendLayoutInvoke(imageListView);

                    //Clean up ImageListView and other queues
                    ImageListViewClearThumbnailCache(imageListView1);
                    //imageListView1.Refresh();
                    ClearAllQueues();

                    UpdateStatusAction("Delete all data and files...");
                    lock (GlobalData.ReloadAllowedFromCloudLock)
                    {
                        GlobalData.ReloadAllowedFromCloud = filesCutCopyPasteDrag.DeleteFileEntriesBeforeReload(imageListView.Items, updatedOnlySelected);
                    }
                    filesCutCopyPasteDrag.ImageListViewReload(imageListView.Items, updatedOnlySelected);

                    folderTreeViewFolder.Enabled = true;
                    ImageListViewResumeLayoutInvoke(imageListView);
                    GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                    GlobalData.IsPopulatingButtonAction = false;
                    GlobalData.IsPopulatingImageListView = false;

                    FilesSelected();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region ToolStrip - Reload Metadata - Selected items - Click
        private void toolStripMenuItemReloadThumbnailAndMetadata_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteLastMediadataAndReload(imageListView1, true);

                DataGridView dataGridView = GetActiveTabDataGridView();
                if (dataGridView != null) DataGridViewHandler.Focus(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region ToolStrip - Reload Metadata - Folder - Click
        private void toolStripMenuItemTreeViewFolderReload_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteLastMediadataAndReload(imageListView1, false);
                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region ToolStrip - Delete Metadata Hirstory - Selected Items - Click
        private void toolStripMenuItemReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory_Click(object sender, EventArgs e)
        {
            try
            {
                using (new WaitCursor())
                {
                    filesCutCopyPasteDrag.ReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory(this, folderTreeViewFolder, imageListView1);
                    FilesSelected();
                    DisplayAllQueueStatus();
                }
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region ToolStrip - Delete Metadata Hirstory - Directory - Click
        private void toolStripMenuItemTreeViewFolderClearCache_Click(object sender, EventArgs e)
        {
            try
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
                    using (new WaitCursor())
                    {
                        //Clean up ImageListView and other queues
                        ImageListViewClearThumbnailCache(imageListView1);
                        imageListView1.Refresh();
                        ClearAllQueues();

                        UpdateStatusAction("Delete all record about files in database....");
                        GlobalData.ProcessCounterDelete = FilesCutCopyPasteDrag.DeleteDirectoryAndHistorySize;
                        int recordAffected = filesCutCopyPasteDrag.DeleteDirectoryAndHistory(ref FilesCutCopyPasteDrag.DeleteDirectoryAndHistorySize, folder);
                        GlobalData.ProcessCounterDelete = 0;
                        UpdateStatusAction(recordAffected + " records was delete from database....");

                        string selectedFolder = this.folderTreeViewFolder.GetSelectedNodePath();
                        HashSet<FileEntry> fileEntries = ImageAndMovieFileExtentionsUtility.ListAllMediaFileEntries(selectedFolder, false);
                        PopulateImageListView(fileEntries, selectedFolder, false);
                    }
                }
                DisplayAllQueueStatus();
                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        

        #region ToolStrip - OpenWith - Click
        private void ToolStripMenuItemOpenWith_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        #region ImageListView

        #region ImageListView - Cut - Click
        private void toolStripMenuItemImageListViewCut_Click(object sender, EventArgs e)
        {
            try
            {
                var droplist = new StringCollection();
                using (new WaitCursor())
                {
                    foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FileFullPath);

                    DataObject data = new DataObject();
                    data.SetFileDropList(droplist);
                    data.SetData("Preferred DropEffect", DragDropEffects.Move);

                    Clipboard.Clear();
                    Clipboard.SetDataObject(data, true);
                }
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region ImageListView - Copy - Click
        private void toolStripMenuItemImageListViewCopy_Click(object sender, EventArgs e)
        {
            try
            {
                StringCollection droplist = new StringCollection();
                using (new WaitCursor())
                {
                    foreach (ImageListViewItem item in imageListView1.SelectedItems) droplist.Add(item.FileFullPath);

                    DataObject data = new DataObject();
                    data.SetFileDropList(droplist);
                    data.SetData("Preferred DropEffect", DragDropEffects.Copy);

                    Clipboard.Clear();
                    Clipboard.SetDataObject(data, true);
                }
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region ImageListView - Paste - Click
        private void toolStripMenuItemImageListViewPaste_Click(object sender, EventArgs e)
        {
            try
            {
                StringCollection files = Clipboard.GetFileDropList();
                using (new WaitCursor())
                {
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
                imageListView1.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            using (new WaitCursor())
            {
                droplist.Add(folder);

                DataObject data = new DataObject();
                data.SetFileDropList(droplist);
                data.SetData("Preferred DropEffect", DragDropEffects.Move);

                Clipboard.Clear();
                Clipboard.SetDataObject(data, true);
            }
            folderTreeViewFolder.Focus();
        }
        #endregion

        #region FolderTree - Copy - Click
        private void toolStripMenuItemTreeViewFolderCopy_Click(object sender, EventArgs e)
        {
            try
            {
                string folder = Furty.Windows.Forms.ShellOperations.GetFileDirectory(currentNodeWhenStartDragging); // folderTreeViewFolder.GetSelectedNodePath();
                StringCollection droplist = new StringCollection();
                using (new WaitCursor())
                {
                    droplist.Add(folder);

                    DataObject data = new DataObject();
                    data.SetFileDropList(droplist);
                    data.SetData("Preferred DropEffect", DragDropEffects.Copy);

                    Clipboard.Clear();
                    Clipboard.SetDataObject(data, true);
                }
                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        #endregion

        #region FolderTree - Copy - FolderNameToClipboard_Click
        private void toolStripMenuItemTreeViewFolderCopyFolderNameToClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                string folder = Furty.Windows.Forms.ShellOperations.GetFileDirectory(currentNodeWhenStartDragging); // folderTreeViewFolder.GetSelectedNodePath();
                Clipboard.SetText(folder);
            } catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region FolderTree - Paste - Click
        private void toolStripMenuItemTreeViewFolderPaste_Click(object sender, EventArgs e)
        {
            try
            {
                DragDropEffects dragDropEffects = DetectCopyOrMove();
                using (new WaitCursor())
                {
                    CopyOrMove(dragDropEffects, currentNodeWhenStartDragging, Clipboard.GetFileDropList(), Furty.Windows.Forms.ShellOperations.GetFileDirectory(currentNodeWhenStartDragging));
                    folderTreeViewFolder.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderTree - Folder - Click
        private void folderTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (GlobalData.IsPopulatingFolderTree) return;
                if (GlobalData.IsDragAndDropActive) return;

                if (GlobalData.DoNotRefreshImageListView) return;
                PopulateImageListView_FromFolderSelected(false, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                MemoryStream stream = (MemoryStream)obj.GetData("Preferred DropEffect", true);
                if (stream != null)
                {
                    int flag = stream.ReadByte();
                    if (flag != 2 && flag != 5) return DragDropEffects.None;

                    if (flag == 2) return DragDropEffects.Move;
                    if (flag == 5) return DragDropEffects.Copy;
                }

            } catch (Exception ex)
            {
                Logger.Error(ex, "Clipboard failed: ");
            }
            return DragDropEffects.None;
        }
        #endregion

        #region FolderTree - Drag and Drop - Files or Folders - ** Check if File is in ThreadQueue before accept move **
        private void CopyOrMove(DragDropEffects dragDropEffects, TreeNode targetNode, StringCollection fileDropList, string targetDirectory)
        {
            try
            {
                if (dragDropEffects == DragDropEffects.None)
                {
                    MessageBox.Show("Was not able to detect if you select copy or cut object that was pasted or dropped");
                    return;
                }

                if (dragDropEffects == DragDropEffects.None)
                {
                    if (IsFileInThreadQueueLock(fileDropList))
                    {
                        MessageBox.Show("Can't move files. Files are being used, you need wait until process is finished.");
                        return;
                    }
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
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region FolderTree - Drag and Drop - Drop - Move/Copy Files - Move/Copy Folders
        private void folderTreeViewFolder_DragDrop(object sender, DragEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
            try
            {
                //clickedNode = e.Node;
                currentNodeWhenStartDragging = e.Node;

                if (e.Button == MouseButtons.Right)
                {
                    folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (((Furty.Windows.Forms.FolderTreeView)sender).SelectedNode == e.Node) PopulateImageListView_FromFolderSelected(false, true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            PopulateImageListView_FromFolderSelected(false, true);
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
                Logger.Error(ex, "folderTreeViewFolder_ItemDrag, Failed create drag and drop tarnsfer data.");
                MessageBox.Show("Failed create drag and drop tarnsfer data. Error: " + ex.Message);
                folderTreeViewFolder.Focus();
            }
        }
        #endregion 

        #region FolderTree - Drag and Drop - Drag Leave - Set Clipboard data to ** FileDropList ** | Link |
        private void folderTreeViewFolder_DragLeave(object sender, EventArgs e)
        {
            try
            {
                isInternalDrop = false;

                GlobalData.IsDragAndDropActive = false;

                GlobalData.DoNotRefreshImageListView = true;
                folderTreeViewFolder.SelectedNode = currentNodeWhenStartDragging;
                GlobalData.DoNotRefreshImageListView = false;

                folderTreeViewFolder.Focus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                Logger.Error(ex, "folderTreeViewFolder_DragEnter");
            }
        }
        #endregion


        enum DragDropKeyStates
        {
            AltKey =32, //The ALT key is pressed.
            ControlKey = 8, //The control (CTRL) key is pressed.
            LeftMouseButton	= 1, //The left mouse button is pressed.
            MiddleMouseButton = 16, //The middle mouse button is pressed.
            None = 0,//No modifier keys or mouse buttons are pressed.
            RightMouseButton = 2, //The right mouse button is pressed.
            ShiftKey = 4 //The shift (SHIFT) key is pressed.
        }

        #region FolderTree - Drag and Drop - Drag Over - update folderTreeViewFolder.SelectedNode
        private void folderTreeViewFolder_DragOver(object sender, DragEventArgs e)
        {
            isInternalDrop = true;
            try
            {
                if (((DragDropKeyStates)e.KeyState & DragDropKeyStates.ShiftKey) == DragDropKeyStates.ShiftKey)
                    e.Effect = DragDropEffects.Move;
                else if (((DragDropKeyStates)e.KeyState & DragDropKeyStates.RightMouseButton) == DragDropKeyStates.RightMouseButton)
                    e.Effect = DragDropEffects.Copy;
                else if (((DragDropKeyStates)e.KeyState & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey)
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
                Logger.Warn(ex, "folderTreeViewFolder_DragOver");
            }
        }
        #endregion
        #endregion

        #region Select Group

        #region Select Group - Populate ToolStripMenuItem
        private void PopulateSelectGroupToolStripMenuItems()
        {
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 1) toolStripMenuItemSelectSameDay.Checked = true;
            else toolStripMenuItemSelectSameDay.Checked = false;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 3) toolStripMenuItemSelectSame3Day.Checked = true;
            else toolStripMenuItemSelectSame3Day.Checked = false;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 7) toolStripMenuItemSelectSameWeek.Checked = true;
            else toolStripMenuItemSelectSameWeek.Checked = false;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 14) toolStripMenuItemSelectSame2week.Checked = true;
            else toolStripMenuItemSelectSame2week.Checked = false;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 30) toolStripMenuItemSelectSameMonth.Checked = true;
            else toolStripMenuItemSelectSameMonth.Checked = false;

            if (Properties.Settings.Default.SelectGroupMaxCount == 10) toolStripMenuItemSelectMax10items.Checked = true;
            else toolStripMenuItemSelectMax10items.Checked = false;
            if (Properties.Settings.Default.SelectGroupMaxCount == 30) toolStripMenuItemSelectMax30items.Checked = true;
            else toolStripMenuItemSelectMax30items.Checked = false;
            if (Properties.Settings.Default.SelectGroupMaxCount == 50) toolStripMenuItemSelectMax50items.Checked = true;
            else toolStripMenuItemSelectMax50items.Checked = false;
            if (Properties.Settings.Default.SelectGroupMaxCount == 100) toolStripMenuItemSelectMax100items.Checked = true;
            else toolStripMenuItemSelectMax100items.Checked = false;

            toolStripMenuItemSelectFallbackOnFileCreated.Checked = Properties.Settings.Default.SelectGroupFileCreatedFallback;
            toolStripMenuItemSelectSameLocationName.Checked = Properties.Settings.Default.SelectGroupSameLocationName;
            toolStripMenuItemSelectSameCity.Checked = Properties.Settings.Default.SelectGroupSameCity;
            toolStripMenuItemSelectSameDistrict.Checked = Properties.Settings.Default.SelectGroupSameDistrict;
            toolStripMenuItemSelectSameCountry.Checked = Properties.Settings.Default.SelectGroupSameCountry;

            GroupSelectionClear();
        }
        #endregion 

        #region Select Group - Previous
        private void toolStripButtonSelectPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                lastGroupDirection = -1;
                int baseItemIndex = SelectedGroupFindBaseItemIndex(imageListView1, lastGroupDirection);

                SelectedGroupBySelections(imageListView1, baseItemIndex, lastGroupDirection,
                    Properties.Settings.Default.SelectGroupMaxCount,
                    Properties.Settings.Default.SelectGroupNumberOfDays,
                    Properties.Settings.Default.SelectGroupFileCreatedFallback,
                    Properties.Settings.Default.SelectGroupSameLocationName,
                    Properties.Settings.Default.SelectGroupSameCity,
                    Properties.Settings.Default.SelectGroupSameDistrict,
                    Properties.Settings.Default.SelectGroupSameCountry);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Next
        private void toolStripButtonSelectNext_Click(object sender, EventArgs e)
        {
            try
            {
                lastGroupDirection = 1;
                int baseItemIndex = SelectedGroupFindBaseItemIndex(imageListView1, lastGroupDirection);

                SelectedGroupBySelections(imageListView1, baseItemIndex, lastGroupDirection,
                    Properties.Settings.Default.SelectGroupMaxCount,
                    Properties.Settings.Default.SelectGroupNumberOfDays,
                    Properties.Settings.Default.SelectGroupFileCreatedFallback,
                    Properties.Settings.Default.SelectGroupSameLocationName,
                    Properties.Settings.Default.SelectGroupSameCity,
                    Properties.Settings.Default.SelectGroupSameDistrict,
                    Properties.Settings.Default.SelectGroupSameCountry);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Select Group - Select Last
        public void GroupSelectLast()
        {
            int baseItemIndex = SelectedGroupFindBaseItemIndex(imageListView1, 0);

            SelectedGroupBySelections(imageListView1, baseItemIndex, lastGroupDirection,
                Properties.Settings.Default.SelectGroupMaxCount,
                Properties.Settings.Default.SelectGroupNumberOfDays,
                Properties.Settings.Default.SelectGroupFileCreatedFallback,
                Properties.Settings.Default.SelectGroupSameLocationName,
                Properties.Settings.Default.SelectGroupSameCity,
                Properties.Settings.Default.SelectGroupSameDistrict,
                Properties.Settings.Default.SelectGroupSameCountry);
        }
        #endregion 

        #region Select Group - Fallback on File Created
        private void toolStripMenuItemSelectFallbackOnFileCreated_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
                Properties.Settings.Default.SelectGroupFileCreatedFallback = !toolStripMenuItem.Checked;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same day
        private void toolStripMenuItemSelectSameDay_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSameDay.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 1;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same 3 days
        private void toolStripMenuItemSelectSame3Day_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSame3Day.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 3;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same week
        private void toolStripMenuItemSelectSameWeek_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSameWeek.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 7;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same 2 weeks
        private void toolStripMenuItemSelectSame2week_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSame2week.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 14;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Select Group - Same month
        private void toolStripMenuItemSelectSameMonth_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemSelectSameMonth.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = -1;
                else Properties.Settings.Default.SelectGroupNumberOfDays = 30;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Max 10 items
        private void toolStripMenuItemSelectMax10items_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SelectGroupMaxCount = 10;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Max 30 items
        private void toolStripMenuItemSelectMax30items_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SelectGroupMaxCount = 30;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - 50 items
        private void toolStripMenuItemSelectMax50items_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SelectGroupMaxCount = 50;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - 100 items
        private void toolStripMenuItemSelectMax100items_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.SelectGroupMaxCount = 100;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same Location Name
        private void toolStripMenuItemSelectSameLocationName_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
                Properties.Settings.Default.SelectGroupSameLocationName = !toolStripMenuItem.Checked;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region Select Group - Same City
        private void toolStripMenuItemSelectSameCity_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
                Properties.Settings.Default.SelectGroupSameCity = !toolStripMenuItem.Checked;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Select Group - District
        private void toolStripMenuItemSelectSameDistrict_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
            Properties.Settings.Default.SelectGroupSameDistrict = !toolStripMenuItem.Checked;
            PopulateSelectGroupToolStripMenuItems();
            GroupSelectLast();
        }
        #endregion 

        #region Select Group - Same Country
        private void toolStripMenuItemSelectSameCountry_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)(sender);
                Properties.Settings.Default.SelectGroupSameCountry = !toolStripMenuItem.Checked;
                PopulateSelectGroupToolStripMenuItems();
                GroupSelectLast();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                MessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #endregion 
    }
}
