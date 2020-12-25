using System;
using System.Windows.Forms;
using Manina.Windows.Forms;
using MetadataLibrary;
using DataGridViewGeneric;
using WindowsProperty;
using static Manina.Windows.Forms.ImageListView;
using System.IO;
using System.Collections.Generic;
using FileDateTime;
using System.Threading;
using ApplicationAssociations;

namespace PhotoTagsSynchronizer
{
    
    public partial class MainForm : Form
    {
        private void FilesSelected()
        {
            if (GlobalData.IsPopulatingAnything()) return;
            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;

            GlobalData.IsPopulatingImageListView = true;
            Application.UseWaitCursor = true;
            //imageListView1.Enabled = false;
            PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems); //GlobalData.isPopulatingSelectedFiles start with true end with false;
            //imageListView1.Enabled = true;
            PopulateImageListeViewToolStrip(imageListView1.SelectedItems);

            Application.UseWaitCursor = false;
            GlobalData.IsPopulatingImageListView = false;
            imageListView1.Focus();
        }

        


        private void PopulateImageListeViewToolStrip(ImageListViewSelectedItemCollection imageListViewSelectedItems)
        {
            if (imageListViewSelectedItems.Count == 1) openWithDialogToolStripMenuItem.Visible = true;
            else openWithDialogToolStripMenuItem.Visible = false;

            List<string> extentions = new List<string>();
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectedItems)
            {
                string extention = Path.GetExtension(imageListViewItem.FileFullPath).ToLower();
                if (!extentions.Contains(extention)) extentions.Add(extention);
            }

            ApplicationAssociationsHandler applicationAssociationsHandler = new ApplicationAssociationsHandler();
            List<ApplicationData> listOfCommonOpenWith = applicationAssociationsHandler.OpenWithInCommon(extentions);

            openMediaFilesWithToolStripMenuItem.DropDownItems.Clear();
            if (listOfCommonOpenWith != null && listOfCommonOpenWith.Count > 0)
            {
                openMediaFilesWithToolStripMenuItem.Visible = true;
                foreach (ApplicationData data in listOfCommonOpenWith)
                {
                    foreach (VerbLink verbLink in data.VerbLinks)
                    {
                        ApplicationData singelVerbApplicationData = new ApplicationData();
                        singelVerbApplicationData.AppIconReference = data.AppIconReference;
                        singelVerbApplicationData.ApplicationId = data.ApplicationId;
                        singelVerbApplicationData.Command = data.Command;
                        singelVerbApplicationData.FriendlyAppName = data.FriendlyAppName;
                        singelVerbApplicationData.Icon = data.Icon;
                        singelVerbApplicationData.ProgId = data.ProgId;
                        singelVerbApplicationData.AddVerb(verbLink.Verb, verbLink.Command);

                        ToolStripMenuItem toolStripMenuItemOpenWith = new ToolStripMenuItem(singelVerbApplicationData.FriendlyAppName.Replace("&", "&&") + " - " + verbLink.Verb, singelVerbApplicationData.Icon.ToBitmap());
                        toolStripMenuItemOpenWith.Tag = singelVerbApplicationData;
                        toolStripMenuItemOpenWith.Click += ToolStripMenuItemOpenWith_Click;
                        openMediaFilesWithToolStripMenuItem.DropDownItems.Add(toolStripMenuItemOpenWith);
                    }
                }
            }
            else openMediaFilesWithToolStripMenuItem.Visible = false;

        }

        private void ToolStripMenuItemOpenWith_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            if (toolStripMenuItem.Tag is ApplicationData applicationData)
            {
                foreach (ImageListViewItem imageListViewItem in imageListView1.SelectedItems)
                {
                    if (applicationData.VerbLinks.Count >= 1) ApplicationActivation.ProcessRun(applicationData.VerbLinks[0].Command, applicationData.ApplicationId, imageListViewItem.FileFullPath, applicationData.VerbLinks[0].Verb, false);                    
                }
            }

        }

        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(imageListView1_SelectionChanged), sender, e);
                return;
            }

            FilesSelected();
        }

        //tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString()
        private DataGridView GetActiveDataGridView(string tag)
        {
            try
            {
                switch (tag)
                {
                    case "Tags":
                        return dataGridViewTagsAndKeywords;
                    case "Map":
                        return dataGridViewMap;
                    case "People":
                        return dataGridViewPeople;
                    case "Date":
                        return dataGridViewDate;
                    case "ExifTool":
                        return dataGridViewExifTool;
                    case "Warning":
                        return dataGridViewExifToolWarning;
                    case "Properties":
                        return dataGridViewProperties;
                    case "Rename":
                        return dataGridViewRename;
                    default:
                        return null;
                }
            } catch (Exception e)
            {
                //Why do this been called from another thread
                Logger.Error("GetActiveDataGridView: " + e.Message);
            }
            return null;
        }


        private void PopulateImageOnFileEntryOnSelectedGrivViewInvoke(FileEntryImage fileEntryImage)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryImage>(PopulateImageOnFileEntryOnSelectedGrivViewInvoke), fileEntryImage);
                return;
            }
            
            if (GlobalData.IsPopulatingAnything()) return;

            DataGridView dataGridView = GetActiveDataGridView(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            DataGridViewHandler.UpdateImageOnFile(dataGridView, fileEntryImage);
        }

        private void RefreshHeaderImageOnActiveDataGrivView(string fullFilePath)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(RefreshHeaderImageOnActiveDataGrivView), fullFilePath);
                return;
            }

            if (GlobalData.IsPopulatingAnything()) return;

            DataGridView dataGridView = GetActiveDataGridView(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            if (dataGridView == null) return;
            
            for (int columnIndex = 0; columnIndex < dataGridView.ColumnCount; columnIndex++)
            {
                if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn)
                {
                    if (dataGridView.Columns[columnIndex].Tag is DataGridViewGenericColumn column && column.FileEntryImage.FileFullPath == fullFilePath)
                    {
                        dataGridView.InvalidateCell(columnIndex, -1);
                    }
                }
            }
            DataGridViewHandler.Refresh(dataGridView);
        }

        /*
        private void RefreshHeaderImageOnActiveDataGrivView(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                RefreshHeaderImageOnActiveDataGrivView(imageListViewItem.FullFileName);
            }
        }
        */

        private void PopulateMetadataOnFileOnActiveDataGrivViewInvoke(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                System.Diagnostics.Debug.WriteLine("UpdateMetadataOnSelectedFilesOnActiveDataGrivView: imageListViewItem.FullFileName" + imageListViewItem.FileFullPath);
                PopulateMetadataOnFileOnActiveDataGrivViewInvoke(imageListViewItem.FileFullPath);
            }
        }

        private void PopulateMetadataOnFileOnActiveDataGrivView(DataGridView dataGridView, string fullFilePath, string tag)
        {
            lock (GlobalData.populateSelectedLock)
            {
                DateTime dateTimeForEditableMediaFile = DataGridViewHandler.DateTimeForEditableMediaFile; //Else File.GetLastWriteTime(fullFilePath)

                switch (tag)
                {
                    case "Tags":
                        DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        PopulateDetailViewTagsAndKeywords(dataGridView);
                        break;
                    case "People":
                        DataGridViewHandlerPeople.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                    case "Map":
                        DataGridViewHandlerMap.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                    case "Date":
                        DataGridViewHandlerDate.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                    case "ExifTool":
                        dateTimeForEditableMediaFile = File.GetLastWriteTime(fullFilePath);
                        DataGridViewHandlerExiftool.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        break;
                    case "Warning":
                        dateTimeForEditableMediaFile = File.GetLastWriteTime(fullFilePath);
                        DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        break;
                    case "Properties":
                        dateTimeForEditableMediaFile = File.GetLastWriteTime(fullFilePath);
                        DataGridViewHandlerProperties.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        break;
                    case "Rename":
                        DataGridViewHandlerRename.PopulateFile(dataGridView, fullFilePath, showWhatColumns, dateTimeForEditableMediaFile);
                        break;
                }
            }
        }
       
        private void PopulateMetadataOnFileOnActiveDataGrivViewInvoke(string fullFilePath)
        {           
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(PopulateMetadataOnFileOnActiveDataGrivViewInvoke), fullFilePath);
                return;
            }

            if (GlobalData.IsApplicationClosing) return;
            DataGridView dataGridView;

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "Tags" || GlobalData.IsAgregatedTags)
            {
                dataGridView = GetActiveDataGridView("Tags");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }
            
            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "Map" || GlobalData.IsAgregatedMap)
            {
                dataGridView = GetActiveDataGridView("Map");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "People" || GlobalData.IsAgregatedMap)
            {
                dataGridView = GetActiveDataGridView("People");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "Date" || GlobalData.IsAgregatedPeople)
            {
                dataGridView = GetActiveDataGridView("Date");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "ExifTool" || GlobalData.IsAgregatedPeople)
            {
                dataGridView = GetActiveDataGridView("ExifTool");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }

            if (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString() == "Warning" || GlobalData.IsAgregatedPeople)
            {
                dataGridView = GetActiveDataGridView("Warning");
                PopulateMetadataOnFileOnActiveDataGrivView(dataGridView, fullFilePath, tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
            }
        }

        private bool ImageListViewForceThumbnailRefreshAndThreads(ImageListView imageListView, string fullFileName)
        {
            bool existAndUpdated = false;

            if (GlobalData.retrieveImageCount > 0) 
            {
                Thread.Sleep(100); //Wait until all ImageListView events are removed
                //DEBGUG BREAK
            }

            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
            try
            {
                ImageListViewSuspendLayoutInvoke(imageListView);
                foreach (ImageListViewItem item in imageListView.SelectedItems)
                {
                    if (item.FileFullPath == fullFileName)
                    {
                        existAndUpdated = true;
                        ImageListViewUpdateItemThumbnailAndMetadataInvoke(item);
                        break;
                    }
                }
                ImageListViewResumeLayoutInvoke(imageListView);
            }
            catch
            {
                //DID ImageListe update failed, because of thread???
            }
            GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
            return existAndUpdated;
        }


        private void PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            lock (GlobalData.populateSelectedLock)
            {
                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();
                PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewInvoke(imageListViewSelectItems);
                DisplayAllQueueStatus();
            }
        }

        /// <summary>
        /// Populate Active DataGridView with Seleted Files from ImageListView
        /// PS. When selected new files, all DataGridViews are maked as dirty.
        /// </summary>
        /// <param name="imageListViewSelectItems"></param>
        private void PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewInvoke(ImageListViewSelectedItemCollection imageListViewSelectItems)
        {
            if (InvokeRequired)
            {
                _ = this.BeginInvoke(new Action<ImageListViewSelectedItemCollection>(PopulateMetadataOnFileOnActiveDataGrivViewInvoke), imageListViewSelectItems);
                return;
            }

            if (GlobalData.IsApplicationClosing) return;

            lock (GlobalData.populateSelectedLock)
            {
                foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
                {
                    AddQueueAllUpadtedFileEntry(new FileEntryImage(imageListViewItem.FileFullPath, imageListViewItem.DateModified));
           
                }
                StartThreads();

                DataGridView dataGridView = GetActiveDataGridView(tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString());
                switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
                {
                    case "Tags":
                        ClearDetailViewTagsAndKeywords();
                        DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();
                        
                        dataGridView.SuspendLayout();
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state

                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords, showWhatColumns);

                        PopulateDetailViewTagsAndKeywords(dataGridView);
                        dataGridView.Enabled = true;
                        dataGridView.ResumeLayout();

                        break;
                    case "Map":
                        splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();

                        DataGridViewHandlerMap.TimeZoneShift = GetTimeZoneShift();
                        DataGridViewHandlerMap.AccepedIntervalSecound = GetAccepedIntervalSecound();
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerMap.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerMap.DatabaseGoogleLocationHistory = databaseGoogleLocationHistory;
                        DataGridViewHandlerMap.DatabaseAndCacheLocationAddress = databaseLocationAddress;
                        DataGridViewHandlerMap.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                        DataGridViewHandlerMap.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizeMap, showWhatColumns);

                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;

                        break;
                    case "People":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();

                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerPeople.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizePeoples, showWhatColumns);

                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "Date":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();

                        DataGridViewHandlerDate.DatabaseExiftoolData = databaseExiftoolData;
                        DataGridViewHandlerDate.DataGridViewMap = dataGridViewMap;
                        DataGridViewHandlerDate.DataGridViewMapHeaderMedia = DataGridViewHandlerMap.headerMedia;
                        DataGridViewHandlerDate.DataGridViewMapTagCoordinates = DataGridViewHandlerMap.tagCoordinates;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                        DataGridViewHandlerDate.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                        DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, false, (DataGridViewSize)Properties.Settings.Default.CellSizeDates, showWhatColumns);

                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "ExifTool":                        
                        //DataGridViewHandler.SuspendLayout(dataGridView); //Remember datagrid_CellPainting will be triggered when change Enable state
                        DataGridViewHandlerExiftool.DatabaseExiftoolData = databaseExiftoolData;
                        DataGridViewHandlerExiftool.exiftoolReader = exiftoolReader;
                        DataGridViewHandlerExiftool.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        //DataGridViewHandler.ResumeLayout(dataGridView);
                        break;
                    case "Warning":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();
                        DataGridViewHandlerExiftoolWarnings.DatabaseExiftoolWarning = databaseExiftoolWarning;
                        DataGridViewHandlerExiftoolWarnings.exiftoolReader = exiftoolReader;
                        DataGridViewHandlerExiftoolWarnings.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "Properties":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();                        
                        DataGridViewHandlerProperties.WindowsPropertyReader = new WindowsPropertyReader();
                        DataGridViewHandlerProperties.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, (DataGridViewSize)Properties.Settings.Default.CellSizeProperties, showWhatColumns);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView);
                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                    case "Rename":
                        dataGridView.Enabled = false; //Remember datagrid_CellPainting will be triggered when change Enable state
                        dataGridView.SuspendLayout();
                        DataGridViewHandlerRename.FileDateTimeFormats =new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                        DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                        DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                        
                        DataGridViewHandlerRename.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, true, ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameSize), ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns);
                        dataGridView.ResumeLayout();
                        dataGridView.Enabled = true;
                        break;
                }
            }
        }

        

    }
}
