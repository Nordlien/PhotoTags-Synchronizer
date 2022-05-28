using DataGridViewGeneric;
using Exiftool;
using FileDateTime;
using Krypton.Toolkit;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WindowsProperty;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        //ConvertAndMerge
        //Date
        //Exiftool
        //ExiftoolWarning
        //Map
        //People
        //Properties
        //Rename
        //TagsAndKeywords
        #region DirtyFlag - DataGridView - CellLeave - UpdatedDirtyFlag
        private void dataGridViewTagsAndKeywords_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewConvertAndMerge_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewProperties_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewDate_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewMap_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }

        private void dataGridViewPeople_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
        }
        #endregion

        
        #region Cell BeginEdit

        #region Cell BeginEdit - Date
        private void dataGridViewDate_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void dataGridViewDate_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Exiftool
        private void dataGridViewExifTool_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void dataGridViewExiftool_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - ExiftoolWarning
        private void dataGridViewExifToolWarning_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void dataGridViewExiftoolWarning_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Map
        private void dataGridViewMap_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void dataGridViewMap_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - People
        private void dataGridViewPeople_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (((KryptonDataGridView)sender)[e.ColumnIndex, e.RowIndex].Value is RegionStructure regionStructure) regionStructure.ShowNameInToString = true; //Just a hack so KryptonDataGridView don't print name alse

                if (triStateButtomClick)
                {
                    e.Cancel = true;
                    return;
                }

                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;


                ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void dataGridViewPeople_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (((KryptonDataGridView)sender)[e.ColumnIndex, e.RowIndex].Value is RegionStructure regionStructure) regionStructure.ShowNameInToString = false; //Just a hack so KryptonDataGridView don't print name also
                DataGridView dataGridView = dataGridViewPeople;
                CheckRowAndSetDefaults(dataGridView, e.ColumnIndex, e.RowIndex);                
                if (!dataGridView.Enabled) return;
                
                if (!ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView))
                {
                    #region Find New names
                    Dictionary<CellLocation, DataGridViewGenericCell> peek = ClipboardUtility.PeekUndoStack(dataGridView);

                    foreach (CellLocation cellLocation in peek.Keys)
                    {
                        DataGridViewGenericCell dataGridViewGenericCell = DataGridViewHandler.GetCellDataGridViewGenericCellCopy(dataGridView, cellLocation.ColumnIndex, cellLocation.RowIndex);
                        if (dataGridViewGenericCell.Value is RegionStructure regionStructureForPeek)
                        {
                            if (!string.IsNullOrWhiteSpace(regionStructureForPeek.Name)) PeopleAddNewLastUseName(regionStructureForPeek.Name);
                        }
                        else
                        {
                            //DEBUG
                        }
                    }
                    #endregion
                }

                DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex, out string diffrences), diffrences);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region Cell BeginEdit - Properties
        private void dataGridViewProperties_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void dataGridViewProperties_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Rename
        private void dataGridViewRename_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void dataGridViewRename_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (triStateButtomClick)
                {
                    e.Cancel = true;
                    return;
                }

                DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;

                ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private bool cellEndEditInProcess = false;
        private void dataGridViewTagsAndKeywords_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (cellEndEditInProcess) return;
            cellEndEditInProcess = true;
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
                if (gridViewGenericDataRow != null)
                {
                    string newValue = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, e.ColumnIndex, e.RowIndex);
                    if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagTitle)
                    {
                        ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionTitle, newValue);
                        Properties.Settings.Default.AutoCorrectFormTitle = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionTitle);
                    }
                    if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagDescription)
                    {
                        ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionDescription, newValue);
                        Properties.Settings.Default.AutoCorrectFormDescription = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionDescription);
                    }
                    if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagComments)
                    {
                        ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionComments, newValue);
                        Properties.Settings.Default.AutoCorrectFormComments = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionComments);
                    }
                    if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagAlbum)
                    {
                        ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionAlbum, newValue);
                        Properties.Settings.Default.AutoCorrectFormAlbum = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionAlbum);
                    }
                    if (gridViewGenericDataRow.RowName == DataGridViewHandlerTagsAndKeywords.tagAuthor)
                    {
                        ComboBoxHandler.AddLastTextFirstInAutoCompleteStringCollection(autoCompleteStringCollectionAuthor, newValue);
                        Properties.Settings.Default.AutoCorrectFormAuthor = ComboBoxHandler.AutoCompleteStringCollectionToString(autoCompleteStringCollectionAuthor);
                    }
                }

                //DataGridView dataGridView = ((DataGridView)sender);
                if (!dataGridView.Enabled) return;
                ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                cellEndEditInProcess = false;
            }
        }

        #endregion

        #endregion

        #region Cell Painting

        #region Cell Painting - Convert and Merge
        private void dataGridViewConvertAndMerge_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
                //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);

                //Draw red line for drag and drop
                DataGridView dataGridView = (DataGridView)sender;
                if (e.RowIndex == dragdropcurrentIndex && e.RowIndex > -1 && dragdropcurrentIndex < DataGridViewHandler.GetRowCount(dataGridView))
                {
                    Pen p = new Pen(Color.Red, 2);
                    e.Graphics.DrawLine(p, e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height - 1, e.CellBounds.Right, e.CellBounds.Top + e.CellBounds.Height - 1);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region Cell Painting - Date
        private void dataGridViewDate_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - Exiftool
        private void dataGridViewExifTool_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - ExiftoolWarning
        private void dataGridViewExifToolWarning_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - Map
        private void dataGridViewMap_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - People
        private void dataGridViewPeople_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                string header = DataGridViewHandlerPeople.headerPeople;

                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                DataGridViewHandler.CellPaintingColumnHeaderRegionsInThumbnail(sender, e);
                DataGridViewHandler.CellPaintingColumnHeaderMouseRegion(sender, e, drawingRegion, peopleMouseDownX, peopleMouseDownY, peopleMouseMoveX, peopleMouseMoveY);

                DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
                if (gridViewGenericDataRow == null) return; //Don't paint anything TriState on "New Empty Row" for "new Keywords"

                DataGridViewGenericColumn dataGridViewGenericDataColumn = null;
                if (e.ColumnIndex > -1)
                {
                    dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
                    if (dataGridViewGenericDataColumn == null) return; //Data is not set, no point to check more.
                    if (dataGridViewGenericDataColumn.Metadata == null) return; //Don't paint TriState button when MetaData is null (data not loaded)
                }

                //If people region row
                if (gridViewGenericDataRow.HeaderName.Equals(DataGridViewHandlerPeople.headerPeople))
                {
                    if (!gridViewGenericDataRow.IsHeader && e.ColumnIndex > -1)
                    {
                        MetadataLibrary.RegionStructure region = DataGridViewHandler.GetCellRegionStructure(dataGridView, e.ColumnIndex, e.RowIndex);
                        Image regionThumbnail = (Image)Properties.Resources.RegionLoading;
                        if (region == null)
                        {
                            e.Handled = false;
                            return;
                        }
                        else if (region.Thumbnail != null) regionThumbnail = region.Thumbnail;
                        DataGridViewHandler.DrawImageAndSubText(sender, e, regionThumbnail, null, ((RegionStructure)e.Value).Name);

                        e.Handled = true;
                    }

                    DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                }
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - Properties
        private void dataGridViewProperties_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - Rename
        private void dataGridViewRename_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
                //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Cell Painting - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                DataGridView dataGridView = ((DataGridView)sender);
                string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

                DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
                DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
                DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
                DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                //KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #endregion

        #region DataGridView - UpdateColumnThumbnail - OnFileEntryAttribute - OnSelectedGrivView
        private void DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute(FileEntryAttribute fileEntryAttribute, Image image)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute, Image>(DataGridView_UpdateColumnThumbnail_OnFileEntryAttribute), fileEntryAttribute, image);
                return;
            }

            DataGridView dataGridView = GetActiveTabDataGridView();
            if (dataGridView == null) return;

            lock (GlobalData.populateSelectedLock)
            {
                //DataGridViewHandler.SetColumnHeaderThumbnail(dataGridView, fileEntryAttribute, image);
                if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandler.SetColumnHeaderThumbnail(dataGridViewTagsAndKeywords, fileEntryAttribute, image);
                if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandler.SetColumnHeaderThumbnail(dataGridViewPeople, fileEntryAttribute, image);
                if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandler.SetColumnHeaderThumbnail(dataGridViewMap, fileEntryAttribute, image);
                if (DataGridViewHandlerDate.HasBeenInitialized) DataGridViewHandler.SetColumnHeaderThumbnail(dataGridViewDate, fileEntryAttribute, image);
                if (DataGridViewHandlerExiftool.HasBeenInitialized) DataGridViewHandler.SetColumnHeaderThumbnail(dataGridViewExiftool, fileEntryAttribute, image);
                if (DataGridViewHandlerExiftoolWarnings.HasBeenInitialized) DataGridViewHandler.SetColumnHeaderThumbnail(dataGridViewExiftoolWarning, fileEntryAttribute, image);
                //if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandler.SetColumnHeaderThumbnail(dataGridViewRename, fileEntryAttribute, image);
                //if (DataGridViewHandlerConvertAndMerge.HasBeenInitialized) DataGridViewHandler.SetColumnHeaderThumbnail(dataGridViewConvertAndMerge, fileEntryAttribute, image);
            }
        }
        #endregion

        #region DataGridView - GetDataGridViewForTag
        private DataGridView GetDataGridViewForTag(string tag)
        {
            try
            {
                switch (tag)
                {
                    case LinkTabAndDataGridViewNameTags:
                        return dataGridViewTagsAndKeywords;
                    case LinkTabAndDataGridViewNameMap:
                        return dataGridViewMap;
                    case LinkTabAndDataGridViewNamePeople:
                        return dataGridViewPeople;
                    case LinkTabAndDataGridViewNameDates:
                        return dataGridViewDate;
                    case LinkTabAndDataGridViewNameExiftool:
                        return dataGridViewExiftool;
                    case LinkTabAndDataGridViewNameWarnings:
                        return dataGridViewExiftoolWarning;
                    case LinkTabAndDataGridViewNameProperties:
                        return dataGridViewProperties;
                    case LinkTabAndDataGridViewNameRename:
                        return dataGridViewRename;
                    case LinkTabAndDataGridViewNameConvertAndMerge:
                        return dataGridViewConvertAndMerge;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                //Why do this been called from another thread
                Logger.Error(ex, "GetActiveDataGridView");
            }
            return null;
        }
        #endregion

        #region DataGridView - GetActiveTabTag()
        private string GetActiveTabTag()
        {
            if (kryptonWorkspaceCellToolbox.SelectedPage == null)
                return null;

            return kryptonWorkspaceCellToolbox.SelectedPage.Tag.ToString();
        }
        #endregion

        #region DataGridView - GetActiveTabDataGridView()
        private DataGridView GetActiveTabDataGridView()
        {
            return GetDataGridViewForTag(GetActiveTabTag());
        }
        #endregion 

        #region DataGridView - GetAnyTabDataGridView()
        private DataGridView GetAnyAgregatedDataGridView()
        {
            if (DataGridViewHandler.GetIsAgregated(dataGridViewTagsAndKeywords)) return dataGridViewTagsAndKeywords;
            if (DataGridViewHandler.GetIsAgregated(dataGridViewPeople)) return dataGridViewPeople;
            if (DataGridViewHandler.GetIsAgregated(dataGridViewMap)) return dataGridViewMap;
            if (DataGridViewHandler.GetIsAgregated(dataGridViewDate)) return dataGridViewDate;
            return dataGridViewTagsAndKeywords; //Also if empty
        }
        #endregion 

        #region DataGridView - IsActiveDataGridViewAgregated
        private bool IsActiveDataGridViewAgregated(string tag)
        {
            bool isAgregated = false;
            switch (tag)
            {
                case LinkTabAndDataGridViewNameTags:
                    isAgregated = GlobalData.IsAgregatedTags;
                    break;
                case LinkTabAndDataGridViewNamePeople:
                    isAgregated = GlobalData.IsAgregatedPeople;
                    break;
                case LinkTabAndDataGridViewNameMap:
                    isAgregated = GlobalData.IsAgregatedMap;
                    break;
                case LinkTabAndDataGridViewNameDates:
                    isAgregated = GlobalData.IsAgregatedDate;
                    break;
                case LinkTabAndDataGridViewNameExiftool:
                    isAgregated = GlobalData.IsAgregatedExiftoolTags;
                    break;
                case LinkTabAndDataGridViewNameWarnings:
                    isAgregated = GlobalData.IsAgregatedExiftoolWarning;
                    break;
                case LinkTabAndDataGridViewNameProperties:
                    isAgregated = GlobalData.IsAgregatedProperties;
                    break;
                case LinkTabAndDataGridViewNameRename:
                    isAgregated = GlobalData.IsAgregatedRename;
                    break;
                case LinkTabAndDataGridViewNameConvertAndMerge:
                    isAgregated = GlobalData.IsAgregatedConvertAndMerge;
                    break;
            }
            return isAgregated;
        }
        #endregion

        #region DataGridView - Populate Extras - AsDropdownAndColumnSizesInvoke (Populate DataGridView Extras)
        private void DataGridView_Populate_ExtrasAsDropdownAndColumnSizesInvoke()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action(DataGridView_Populate_ExtrasAsDropdownAndColumnSizesInvoke));
                return;
            }

            if (GlobalData.IsApplicationClosing) return;

            lock (GlobalData.populateSelectedLock)
            {
                DataGridView dataGridView = GetActiveTabDataGridView();
                switch (GetActiveTabTag())
                {
                    case LinkTabAndDataGridViewNameTags:
                        PopulateDetailViewTagsAndKeywords(dataGridView);
                        break;
                    case LinkTabAndDataGridViewNameMap:
                        break;
                    case LinkTabAndDataGridViewNamePeople:
                        List<DataGridViewGenericColumn> dataGridViewGenericColumns = DataGridViewHandler.GetColumnsDataGridViewGenericColumnCurrentOrAutoCorrect(dataGridView, false);
                        PopulatePeopleToolStripMenuItems(dataGridViewGenericColumns,
                                Properties.Settings.Default.SuggestRegionNameNearbyDays,
                                //Properties.Settings.Default.SuggestRegionNameNearByCount,
                                Properties.Settings.Default.SuggestRegionNameNearByContextMenuCount,
                                Properties.Settings.Default.SuggestRegionNameMostUsedContextMenuCount,
                                Properties.Settings.Default.ApplicationSizeOfRegionNamesGroup,
                                Properties.Settings.Default.RenameDateFormats);

                        break;
                    case LinkTabAndDataGridViewNameDates:
                        break;
                    case LinkTabAndDataGridViewNameExiftool:
                        //DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, true);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, DataGridView_GetCountQueueLazyLoadningSelectedFilesLock());
                        //DataGridViewHandler.ResumeLayoutDelayed(dataGridView);
                        break;
                    case LinkTabAndDataGridViewNameWarnings:
                        //DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, true);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, DataGridView_GetCountQueueLazyLoadningSelectedFilesLock());
                        //DataGridViewHandler.ResumeLayoutDelayed(dataGridView); 
                        break;
                    case LinkTabAndDataGridViewNameProperties:
                        //DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, true);
                        DataGridViewHandler.FastAutoSizeRowsHeight(dataGridView, DataGridView_GetCountQueueLazyLoadningSelectedFilesLock());
                        //DataGridViewHandler.ResumeLayoutDelayed(dataGridView);
                        break;
                    case LinkTabAndDataGridViewNameRename:
                        break;
                    case LinkTabAndDataGridViewNameConvertAndMerge:
                        break;
                    default: throw new NotImplementedException();
                }

                
            }
        }
        #endregion

        #region DataGridView - CountQueueLazyLoadningSelectedFilesLock
        public int DataGridView_GetCountQueueLazyLoadningSelectedFilesLock()
        {
            return CountQueueLazyLoadningSelectedFilesLock() + countInvokeCalls;
        }
        #endregion

        #region DataGridView - SetColumnVisibleStatus - FileEntryAttributeInvoke - Invoke
        private void DataGridView_SetColumnVisibleStatus_FileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute, bool visible)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<FileEntryAttribute, bool>(DataGridView_SetColumnVisibleStatus_FileEntryAttributeInvoke), fileEntryAttribute, visible);
                return;
            }
            if (GlobalData.IsApplicationClosing) return;
            try
            {
                DataGridViewHandler.SetColumnVisibleStatus(dataGridViewTagsAndKeywords, fileEntryAttribute, visible);
                DataGridViewHandler.SetColumnVisibleStatus(dataGridViewPeople, fileEntryAttribute, visible);
                DataGridViewHandler.SetColumnVisibleStatus(dataGridViewMap, fileEntryAttribute, visible);
                DataGridViewHandler.SetColumnVisibleStatus(dataGridViewDate, fileEntryAttribute, visible);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region DataGridView - CleanAll
        private void DataGridView_CleanAll()
        {
            DataGridViewHandler.SetIsAgregated(dataGridViewTagsAndKeywords, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewPeople, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewMap, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewDate, false);

            DataGridViewHandler.SetIsAgregated(dataGridViewExiftool, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewExiftoolWarning, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewProperties, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewRename, false);
            DataGridViewHandler.SetIsAgregated(dataGridViewConvertAndMerge, false);

        }
        #endregion


        #region DataGridView - Populate File - FileEntryVersion - CompatibilityCheckedMetadataToSave
        private void DataGridView_Populate_CompatibilityCheckedMetadataToSave(Metadata metadataToSave, FileEntryVersion fileEntryVersion)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<Metadata, FileEntryVersion>(DataGridView_Populate_CompatibilityCheckedMetadataToSave), metadataToSave, fileEntryVersion);
                return;
            }

            DataGridView_Populate_FileEntryAttribute(GetActiveTabDataGridView(),
                new FileEntryAttribute(metadataToSave.FileFullPath, DateTime.Now, fileEntryVersion), GetActiveTabTag(), MetadataBrokerType.Empty, metadataToSave);
        }
        #endregion

        #region DataGridView - Populate File - FileEntryAttribute  + Invoke + Control the Queue 
        private HashSet<FileEntryAttribute> dataGridView_Populate_FileEntryAttribute_Invoked = new HashSet<FileEntryAttribute>();
        private readonly Object dataGridView_Populate_FileEntryAttribute_InvokedLock = new Object();
        int countInvokeCalls = 0;
        private void DataGridView_Populate_FileEntryAttributeInvoke(FileEntryAttribute fileEntryAttribute, MetadataBrokerType metadataBrokerType, bool isInvokedCall = false, bool wasAlreadyInInvokedQueue = false)
        {
            if (GlobalData.IsApplicationClosing) return;


            if (InvokeRequired)
            {
                countInvokeCalls++;
                bool isAlreadyInvoked = false;

                lock (dataGridView_Populate_FileEntryAttribute_InvokedLock)
                {
                    if (!dataGridView_Populate_FileEntryAttribute_Invoked.Contains(fileEntryAttribute))
                    {
                        dataGridView_Populate_FileEntryAttribute_Invoked.Add(fileEntryAttribute);
                    }
                    else isAlreadyInvoked = true;
                }

                this.BeginInvoke(new Action<FileEntryAttribute, MetadataBrokerType, bool, bool>(DataGridView_Populate_FileEntryAttributeInvoke),
                    fileEntryAttribute, metadataBrokerType, true, isAlreadyInvoked);
                return;
            }

            try
            {
                if (isInvokedCall) countInvokeCalls--;

                #region Call - DataGridView_Populate_FileEntryAttribute - only once, if in queue, don't process all in queue
                if (!wasAlreadyInInvokedQueue)
                {
                    string tag = GetActiveTabTag();
                    if (!string.IsNullOrWhiteSpace(tag) && IsActiveDataGridViewAgregated(tag))
                    {
                        DataGridView dataGridView = GetDataGridViewForTag(tag);
                        if (dataGridView != null) DataGridView_Populate_FileEntryAttribute(dataGridView, fileEntryAttribute, tag, metadataBrokerType);
                    }
                }
                #endregion

                #region Remove from Invoked Queue
                lock (dataGridView_Populate_FileEntryAttribute_InvokedLock)
                {
                    if (dataGridView_Populate_FileEntryAttribute_Invoked.Contains(fileEntryAttribute)) dataGridView_Populate_FileEntryAttribute_Invoked.Remove(fileEntryAttribute);
                }
                #endregion

                FileEntryBroker fileEntryBroker = new FileEntryBroker(fileEntryAttribute.FileEntry, metadataBrokerType);
                RemoveQueueLazyLoadningSelectedFilesLock(fileEntryBroker);

                LazyLoadingDataGridViewProgressUpdateStatus(DataGridView_GetCountQueueLazyLoadningSelectedFilesLock());

                if (countInvokeCalls == 0)
                {
                    int queueCount = CountQueueLazyLoadningSelectedFilesLock();
                    if (queueCount == 0 && countInvokeCalls == 0)
                        DataGridView_Populate_ExtrasAsDropdownAndColumnSizesInvoke();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #region DataGridView - Populate File - FileEntryAttribute
        private void DataGridView_Populate_FileEntryAttribute(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, string tabTag, MetadataBrokerType metadataBrokerType, Metadata metadataAutoCorrect = null)
        {            
            lock (GlobalData.populateSelectedLock)
            {
                #region isFilSelectedInImageListView 
                Manina.Windows.Forms.ImageListViewItem imageListViewItem = ImageListViewHandler.FindItem(imageListView1.Items, fileEntryAttribute.FileFullPath);
                bool isFilSelectedInImageListView = (imageListViewItem != null);
                #endregion

                #region Hack until find reason for ImageListView wasn't updated. With new LastWrittenDateTime
                if (imageListViewItem != null &&
                    FileEntryVersionHandler.IsCurrenFileVersion(fileEntryAttribute.FileEntryVersion) &&
                    imageListViewItem.FileDateModifiedPropertyStatus == Manina.Windows.Forms.PropertyStatus.IsSet)
                {
                    DateTime dateTimeFile = FileHandeling.FileHandler.GetLastWriteTime(fileEntryAttribute.FileFullPath);
                    if (imageListViewItem.DateModified != fileEntryAttribute.LastWriteDateTime)
                        ImageListView_UpdateItemExiftoolMetadataInvoke(fileEntryAttribute, null);
                        //ImageListView_UpdateItemThumbnailUpdateAllInvoke(fileEntryAttribute);
                }
                #endregion


                if (isFilSelectedInImageListView || fileEntryAttribute.FileEntryVersion == FileEntryVersion.ExtractedNowUsingExiftoolFileNotExist)
                {
                    DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, isFilSelectedInImageListView); //Will not suspend when Column Don't exist, but counter will increase

                    DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                    DataGridViewHandlerRename.ShowFullPath = Properties.Settings.Default.RenameShowFullPath;
                    DataGridViewHandlerRename.ComputerNames = new List<string>(oneDriveNetworkNames);
                    DataGridViewHandlerRename.GPStag = Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix;
                    DataGridViewHandlerConvertAndMerge.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                    DataGridViewHandlerConvertAndMerge.RenameVaribale = Properties.Settings.Default.RenameVariable;
                    DataGridViewHandlerPeople.SuggestRegionNameNearByDays = Properties.Settings.Default.SuggestRegionNameNearbyDays;
                    DataGridViewHandlerPeople.SuggestRegionNameNearByTopMostCount = Properties.Settings.Default.SuggestRegionNameNearByCount;
                    DataGridViewHandlerPeople.RenameDateFormats = Properties.Settings.Default.RenameDateFormats;

                    #region Popuate File
                    switch (tabTag)
                    {
                        case LinkTabAndDataGridViewNameTags:
                        case LinkTabAndDataGridViewNamePeople:
                        case LinkTabAndDataGridViewNameMap:
                        case LinkTabAndDataGridViewNameDates:
                            DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);
                            DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);
                            DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);
                            //Map need to be populated after Date
                            DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, false);
                            BeginInvoke(new Action<DataGridView, FileEntryAttribute>(DataGridViewHandlerDate.PopulateExiftoolData), dataGridViewDate, fileEntryAttribute);

                            if (DataGridViewHandlerExiftool.HasBeenInitialized) DataGridViewHandlerExiftool.PopulateFile(dataGridViewExiftool, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerExiftoolWarnings.HasBeenInitialized) DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridViewExiftoolWarning, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect);
                            if (DataGridViewHandlerConvertAndMerge.HasBeenInitialized) DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridViewConvertAndMerge, fileEntryAttribute);

                            //IsDataGridViewColumnDirty
                            if (metadataAutoCorrect != null) 
                            {
                                int columnIndexDirtyFlagToFix = DataGridViewHandler.GetColumnIndexWhenAddColumn(GetAnyAgregatedDataGridView(), fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompare);
                                if (columnIndexDirtyFlagToFix != -1) DataGridViewHandler.SetColumnDirtyFlag(dataGridView, columnIndexDirtyFlagToFix, IsDataGridViewColumnDirty(dataGridView, columnIndexDirtyFlagToFix, out string diffrences), diffrences);
                                //DataGridViewSetDirtyFlagAfterSave(metadataAutoCorrect, true, FileEntryVersion.CurrentVersionInDatabase);
                            }
                            break;
                        case LinkTabAndDataGridViewNameExiftool:
                            DataGridViewHandlerExiftool.PopulateFile(dataGridViewExiftool, fileEntryAttribute, showWhatColumns);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized)
                            {
                                DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                                BeginInvoke(new Action<DataGridView, FileEntryAttribute>(DataGridViewHandlerDate.PopulateExiftoolData), dataGridViewDate, fileEntryAttribute);
                            }
                            //Map need to be populated after Date
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true); 

                            //if (DataGridViewHandlerExiftool.HasBeenInitialized) DataGridViewHandlerExiftool.PopulateFile(dataGridViewExiftool, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerExiftoolWarnings.HasBeenInitialized) DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridViewExiftoolWarning, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect);
                            if (DataGridViewHandlerConvertAndMerge.HasBeenInitialized) DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridViewConvertAndMerge, fileEntryAttribute);
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridViewExiftoolWarning, fileEntryAttribute, showWhatColumns);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized)
                            {
                                DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                                BeginInvoke(new Action<DataGridView, FileEntryAttribute>(DataGridViewHandlerDate.PopulateExiftoolData), dataGridViewDate, fileEntryAttribute);
                            }
                            //Map need to be populated after Date
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);

                            if (DataGridViewHandlerExiftool.HasBeenInitialized) DataGridViewHandlerExiftool.PopulateFile(dataGridViewExiftool, fileEntryAttribute, showWhatColumns);
                            //if (DataGridViewHandlerExiftoolWarnings.HasBeenInitialized) DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridViewExiftoolWarning, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect);
                            if (DataGridViewHandlerConvertAndMerge.HasBeenInitialized) DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridViewConvertAndMerge, fileEntryAttribute);
                            break;
                        case LinkTabAndDataGridViewNameProperties:
                            DataGridViewHandlerProperties.PopulateFile(dataGridViewProperties, fileEntryAttribute, showWhatColumns);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized)
                            {
                                DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                                BeginInvoke(new Action<DataGridView, FileEntryAttribute>(DataGridViewHandlerDate.PopulateExiftoolData), dataGridViewDate, fileEntryAttribute);
                            }
                            //Map need to be populated after Date
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);

                            if (DataGridViewHandlerExiftool.HasBeenInitialized) DataGridViewHandlerExiftool.PopulateFile(dataGridViewExiftool, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerExiftoolWarnings.HasBeenInitialized) DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridViewExiftoolWarning, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect);
                            if (DataGridViewHandlerConvertAndMerge.HasBeenInitialized) DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridViewConvertAndMerge, fileEntryAttribute);
                            break;
                        case LinkTabAndDataGridViewNameRename:
                            DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized)
                            {
                                DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                                BeginInvoke(new Action<DataGridView, FileEntryAttribute>(DataGridViewHandlerDate.PopulateExiftoolData), dataGridViewDate, fileEntryAttribute);
                            }
                            //Map need to be populated after Date
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);

                            if (DataGridViewHandlerExiftool.HasBeenInitialized) DataGridViewHandlerExiftool.PopulateFile(dataGridViewExiftool, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerExiftoolWarnings.HasBeenInitialized) DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridViewExiftoolWarning, fileEntryAttribute, showWhatColumns);
                            //if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect, true);
                            if (DataGridViewHandlerConvertAndMerge.HasBeenInitialized) DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridViewConvertAndMerge, fileEntryAttribute);
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridViewConvertAndMerge, fileEntryAttribute);

                            if (DataGridViewHandlerTagsAndKeywords.HasBeenInitialized) DataGridViewHandlerTagsAndKeywords.PopulateFile(dataGridViewTagsAndKeywords, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerPeople.HasBeenInitialized) DataGridViewHandlerPeople.PopulateFile(dataGridViewPeople, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                            if (DataGridViewHandlerDate.HasBeenInitialized)
                            {
                                DataGridViewHandlerDate.PopulateFile(dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);
                                BeginInvoke(new Action<DataGridView, FileEntryAttribute>(DataGridViewHandlerDate.PopulateExiftoolData), dataGridViewDate, fileEntryAttribute);
                            }
                            //Map need to be populated after Date
                            if (DataGridViewHandlerMap.HasBeenInitialized) DataGridViewHandlerMap.PopulateFile(dataGridViewMap, dataGridViewDate, fileEntryAttribute, showWhatColumns, metadataAutoCorrect, true);

                            if (DataGridViewHandlerExiftool.HasBeenInitialized) DataGridViewHandlerExiftool.PopulateFile(dataGridViewExiftool, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerExiftoolWarnings.HasBeenInitialized) DataGridViewHandlerExiftoolWarnings.PopulateFile(dataGridViewExiftoolWarning, fileEntryAttribute, showWhatColumns);
                            if (DataGridViewHandlerRename.HasBeenInitialized) DataGridViewHandlerRename.PopulateFile(dataGridViewRename, fileEntryAttribute, DataGridViewHandlerRename.ShowFullPath, metadataAutoCorrect);
                            //if (DataGridViewHandlerConvertAndMerge.HasBeenInitialized) DataGridViewHandlerConvertAndMerge.PopulateFile(dataGridViewConvertAndMerge, fileEntryAttribute);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    #endregion

                    #region Check if got thumbnail, if not, push to read queue
                    int columnIndex = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridView, fileEntryAttribute, out FileEntryVersionCompare _);
                    if (columnIndex != -1)
                    {
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                        if (dataGridViewGenericColumn?.Thumbnail == null) AddQueueLazyLoadningMediaThumbnailLock(fileEntryAttribute);
                    }
                    #endregion

                    LazyLoadingDataGridViewProgressUpdateStatus(DataGridView_GetCountQueueLazyLoadningSelectedFilesLock());
                    DataGridViewHandler.ResumeLayoutDelayed(dataGridView); //Will resume when counter reach 0
                }
            }
        }
        #endregion

        #region DataGridView - Populate Selected Files - OnActiveDataGridView - Thread
        private void DataGridView_Populate_SelectedItemsThread(HashSet<FileEntry> imageListViewSelectItems)
        {
            DataGridView_Populate_SelectedItemsInvoke(imageListViewSelectItems);

            //Thread threadPopulateDataGridView = new Thread(() =>
            //{
            //    DataGridView_Populate_SelectedItemsInvoke(imageListViewSelectItems);
            //});

            //threadPopulateDataGridView.Start();
        }
        #endregion

        #region DataGridView - Populate Selected Files - OnActiveDataGridView - Invoke 
        /// <summary>
        /// Populate Active DataGridView with Seleted Files from ImageListView
        /// PS. When selected new files, all DataGridViews are maked as dirty.
        /// </summary>
        /// <param name="imageListViewSelectItems"></param>
        private void DataGridView_Populate_SelectedItemsInvoke(HashSet<FileEntry> imageListViewSelectItems)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action<HashSet<FileEntry>>(DataGridView_Populate_SelectedItemsInvoke), imageListViewSelectItems);
                return;
            }
            
            lock (GlobalData.populateSelectedLock)
            {
                using (new WaitCursor())
                {
                    DataGridView dataGridView = GetActiveTabDataGridView();

                    #region Updated Layout - Ribbon / Favorite / Equal / Size
                    kryptonRibbonGroupButtonDataGridViewRowsFavorite.Checked = DataGridViewHandler.ShowFavouriteColumns(dataGridView);
                    kryptonRibbonGroupButtonDataGridViewRowsHideEqual.Checked = DataGridViewHandler.HideEqualColumns(dataGridView);

                    DataGridViewSize dataGridViewSize;
                    ShowWhatColumns showWhatColumnsForTab;
                    bool isSizeEnabled = ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true).Count > 0;
                    bool isColumnsEnabled = isSizeEnabled;

                    switch (GetActiveTabTag())
                    {
                        case LinkTabAndDataGridViewNameTags:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameMap:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeMap;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNamePeople:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizePeoples;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameDates:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeDates;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameExiftool:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings;
                            showWhatColumnsForTab = showWhatColumns;
                            break;
                        case LinkTabAndDataGridViewNameProperties:
                            dataGridViewSize = (DataGridViewSize)Properties.Settings.Default.CellSizeProperties;
                            showWhatColumnsForTab = ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns;
                            //isSizeEnabled = false;
                            isColumnsEnabled = false;
                            break;
                        case LinkTabAndDataGridViewNameRename:
                            dataGridViewSize = ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize);
                            showWhatColumnsForTab = ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns;
                            //isSizeEnabled = false;
                            isColumnsEnabled = false;
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            dataGridViewSize = ((DataGridViewSize)Properties.Settings.Default.CellSizeConvertAndMerge | DataGridViewSize.RenameConvertAndMergeSize);
                            showWhatColumnsForTab = ShowWhatColumns.HistoryColumns | ShowWhatColumns.ErrorColumns;
                            //isSizeEnabled = false;
                            isColumnsEnabled = false;
                            break;
                        default: throw new NotImplementedException();
                    }
                    SetRibbonDataGridViewSizeBottons(dataGridViewSize, isSizeEnabled);
                    SetRibbonDataGridViewShowWhatColumns(showWhatColumns, isColumnsEnabled);
                    #endregion

                    #region Check if agregated, if not stop
                    if (dataGridView == null || DataGridViewHandler.GetIsAgregated(dataGridView))
                    {
                        LazyLoadingDataGridViewProgressUpdateStatus(-1);
                        return;
                    }
                    #endregion

                    List<FileEntryAttribute> lazyLoading;
                    DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, true);

                    #region PopulateSelectedFiles
                    switch (GetActiveTabTag())
                    {
                        case LinkTabAndDataGridViewNameTags:
                        case LinkTabAndDataGridViewNamePeople:
                        case LinkTabAndDataGridViewNameMap:
                        case LinkTabAndDataGridViewNameDates:
                            using (new WaitCursor())
                            {
                                #region dataGridViewTagsAndKeywords
                                dataGridView = dataGridViewTagsAndKeywords;
                                InitDetailViewTagsAndKeywords();
                                DataGridViewHandlerTagsAndKeywords.MediaAiTagConfidence = GetAiConfidence();
                                DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                                DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                                DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                                DataGridViewHandlerTagsAndKeywords.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                                DataGridViewHandlerTagsAndKeywords.AutoKeywordConvertions = autoKeywordConvertions;
                                DataGridViewHandlerTagsAndKeywords.HasBeenInitialized = true;
                                DataGridViewHandlerTagsAndKeywords.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                                //AddQueueLazyLoadningAllVersionsAllSourcesMetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(imageListViewSelectItems);
                                #endregion

                                #region dataGridViewPeople
                                dataGridView = dataGridViewPeople;
                                DataGridViewHandlerPeople.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                                DataGridViewHandlerPeople.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                                DataGridViewHandlerPeople.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                                DataGridViewHandlerPeople.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                                DataGridViewHandlerPeople.SuggestRegionNameNearByDays = Properties.Settings.Default.SuggestRegionNameNearbyDays;
                                DataGridViewHandlerPeople.SuggestRegionNameNearByTopMostCount = Properties.Settings.Default.SuggestRegionNameNearByCount;
                                DataGridViewHandlerPeople.RenameDateFormats = Properties.Settings.Default.RenameDateFormats;
                                DataGridViewHandlerPeople.HasBeenInitialized = true;
                                DataGridViewHandlerPeople.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);

                                PopulatePeopleToolStripMenuItems(null,
                                    Properties.Settings.Default.SuggestRegionNameNearbyDays,
                                    //Properties.Settings.Default.SuggestRegionNameNearByCount,
                                    Properties.Settings.Default.SuggestRegionNameNearByContextMenuCount,
                                    Properties.Settings.Default.SuggestRegionNameMostUsedContextMenuCount,
                                    Properties.Settings.Default.ApplicationSizeOfRegionNamesGroup,
                                    Properties.Settings.Default.RenameDateFormats);
                                //AddQueueLazyLoadningAllVersionsAllSourcesMetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(imageListViewSelectItems);
                                #endregion

                                #region dataGridViewMap
                                dataGridView = dataGridViewMap;
                                DataGridViewHandlerMap.TimeZoneShift = GetTimeZoneShift();
                                DataGridViewHandlerMap.AccepedIntervalSecound = GetAccepedIntervalSecound();
                                DataGridViewHandlerMap.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                                DataGridViewHandlerMap.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                                DataGridViewHandlerMap.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                                DataGridViewHandlerMap.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                                DataGridViewHandlerMap.DatabaseGoogleLocationHistory = databaseGoogleLocationHistory;
                                DataGridViewHandlerMap.AutoKeywordConvertions = autoKeywordConvertions;
                                DataGridViewHandlerMap.DatabaseAndCacheLocationAddress = databaseLocationNameAndLookUp;
                                DataGridViewHandlerMap.DatabaseAndCacheCameraOwner = databaseAndCahceCameraOwner;
                                DataGridViewHandlerMap.HasBeenInitialized = true;
                                DataGridViewHandlerMap.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                                //AddQueueLazyLoadningAllVersionsAllSourcesMetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(imageListViewSelectItems);
                                #endregion

                                #region dataGridViewDate
                                dataGridView = dataGridViewDate;
                                DataGridViewHandlerDate.DatabaseExiftoolData = databaseExiftoolData;
                                DataGridViewHandlerDate.DataGridViewMap = dataGridViewMap;
                                DataGridViewHandlerDate.DataGridViewMapHeaderMedia = DataGridViewHandlerMap.headerMedia;
                                DataGridViewHandlerDate.DataGridViewMapTagCoordinates = DataGridViewHandlerMap.tagMediaCoordinates;
                                DataGridViewHandlerDate.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                                DataGridViewHandlerDate.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                                DataGridViewHandlerDate.DatabaseAndCacheMetadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery;
                                DataGridViewHandlerDate.DatabaseAndCacheMetadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos;
                                DataGridViewHandlerDate.HasBeenInitialized = true;
                                DataGridViewHandlerDate.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                                //AddQueueLazyLoadning_AllSources_AllVersions_MetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(imageListViewSelectItems);
                                #endregion
                            }
                            AddQueueLazyLoadning_AllSources_AllVersions_MetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(imageListViewSelectItems);

                            break;
                        case LinkTabAndDataGridViewNameExiftool:
                            #region dataGridViewExiftool
                            dataGridView = dataGridViewExiftool;
                            DataGridViewHandlerExiftool.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                            DataGridViewHandlerExiftool.DatabaseExiftoolData = databaseExiftoolData;
                            DataGridViewHandlerExiftool.exiftoolReader = exiftoolReader;
                            DataGridViewHandlerExiftool.HasBeenInitialized = true;
                            using (new WaitCursor())
                            {
                                lazyLoading = DataGridViewHandlerExiftool.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            }
                            AddQueueLazyLoadning_AllSources_NoHistory_MetadataAndRegionThumbnailsLock(lazyLoading);
                            AddQueueLazyLoadningMediaThumbnailLock(lazyLoading);
                            #endregion
                            break;
                        case LinkTabAndDataGridViewNameWarnings:
                            #region dataGridViewExiftoolWarning
                            dataGridView = dataGridViewExiftoolWarning;
                            DataGridViewHandlerExiftoolWarnings.DatabaseAndCacheThumbnail = databaseAndCacheThumbnailPoster;
                            DataGridViewHandlerExiftoolWarnings.DatabaseExiftoolWarning = databaseExiftoolWarning;
                            DataGridViewHandlerExiftoolWarnings.exiftoolReader = exiftoolReader;
                            DataGridViewHandlerExiftoolWarnings.HasBeenInitialized = true;
                            using (new WaitCursor())
                            {
                                lazyLoading = DataGridViewHandlerExiftoolWarnings.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                                //AddQueueLazyLoadningAllSourcesMetadataAndRegionThumbnailsLock(lazyLoading);
                                //AddQueueLazyLoadningMediaThumbnailLock(lazyLoading);
                            }
                            AddQueueLazyLoadning_AllSources_AllVersions_MetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(imageListViewSelectItems);
                            #endregion
                            break;
                        case LinkTabAndDataGridViewNameProperties:
                            #region dataGridViewProperties
                            dataGridView = dataGridViewProperties;
                            DataGridViewHandlerProperties.WindowsPropertyReader = new WindowsPropertyReader();
                            DataGridViewHandlerProperties.HasBeenInitialized = true;
                            using (new WaitCursor())
                            {
                                DataGridViewHandlerProperties.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                                DataGridView_Populate_ExtrasAsDropdownAndColumnSizesInvoke();
                            }
                            #endregion
                            break;
                        case LinkTabAndDataGridViewNameRename:
                            #region dataGridViewRename
                            dataGridView = dataGridViewRename;
                            DataGridViewHandlerRename.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            DataGridViewHandlerRename.RenameVaribale = Properties.Settings.Default.RenameVariable;
                            DataGridViewHandlerRename.ShowFullPath = Properties.Settings.Default.RenameShowFullPath;
                            DataGridViewHandlerRename.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerRename.FilesCutCopyPasteDrag = filesCutCopyPasteDrag;
                            DataGridViewHandlerRename.ComputerNames = new List<string>(oneDriveNetworkNames);
                            DataGridViewHandlerRename.GPStag = Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix;
                            checkBoxRenameShowFullPath.Checked = DataGridViewHandlerRename.ShowFullPath;
                            DataGridViewHandlerRename.HasBeenInitialized = true;
                            DataGridViewHandlerRename.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab, DataGridViewHandlerRename.ShowFullPath);
                            AddQueueLazyLoadning_AllSources_CurrentWrittenDate_UseCurrentFileDate_MetadataAndRegionThumbnailsLock(imageListViewSelectItems, FileEntryVersion.CurrentVersionInDatabase);
                            //AddQueueLazyLoadning_AllSources_AllVersions_MetadataAndRegionThumbnailsLock_AfterPopulateSelectedFiles(imageListViewSelectItems);
                            #endregion
                            break;
                        case LinkTabAndDataGridViewNameConvertAndMerge:
                            #region dataGridViewConvertAndMerge
                            dataGridView = dataGridViewConvertAndMerge;
                            DataGridViewHandlerConvertAndMerge.FileDateTimeFormats = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            DataGridViewHandlerConvertAndMerge.RenameVaribale = Properties.Settings.Default.RenameVariable;
                            DataGridViewHandlerConvertAndMerge.DatabaseAndCacheMetadataExiftool = databaseAndCacheMetadataExiftool;
                            DataGridViewHandlerConvertAndMerge.FilesCutCopyPasteDrag = filesCutCopyPasteDrag;
                            DataGridViewHandlerConvertAndMerge.HasBeenInitialized = true;
                            DataGridViewHandlerConvertAndMerge.PopulateSelectedFiles(dataGridView, imageListViewSelectItems, dataGridViewSize, showWhatColumnsForTab);
                            #endregion
                            break;
                        default: throw new NotImplementedException();

                    }
                    #endregion

                    DataGridViewHandler.ResumeLayoutDelayed(dataGridView);
                    
                } //Cursor

            }
            StartThreads();
        }
        #endregion

        #region DataGridView - Populate - MapLocation
        private void DataGridView_Populate_MapLocation(FileEntryAttribute fileEntryAttribute, bool forceReloadUsingReverseGeocoder)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (this.InvokeRequired)
            {
                BeginInvoke(new Action<FileEntryAttribute, bool>(DataGridView_Populate_MapLocation), fileEntryAttribute, forceReloadUsingReverseGeocoder);
                return;
            }
            DataGridView dataGridView = dataGridViewMap;

            if (!DataGridViewHandler.GetIsAgregated(dataGridView))
                return;
            if (DataGridViewHandler.GetIsPopulating(dataGridView))
                return;

            int columnIndex = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridView, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompare);
            if (columnIndex != -1)
            {
                LocationNames.LocationCoordinate locationCoordinate = DataGridViewHandlerMap.GetUserInputLocationCoordinate(dataGridView, columnIndex, fileEntryAttribute);
                bool createNewAccurateLocationUsingSearchLocation = DataGridViewHandlerMap.GetUserInputIsCreateNewAccurateLocationUsingSearchLocation(dataGridView, columnIndex, fileEntryAttribute);

                if (locationCoordinate != null)
                {
                    DataGridViewHandlerMap.PopulateGrivViewMapNomnatatim(dataGridView, columnIndex, locationCoordinate,
                        onlyFromCache: false, canReverseGeocoder: true, forceReloadUsingReverseGeocoder: forceReloadUsingReverseGeocoder,
                        createNewAccurateLocationUsingSearchLocation: createNewAccurateLocationUsingSearchLocation);
                }
            }
        }
        #endregion


        #region IsDragAndDropActive
        private bool IsDragAndDropActive()
        {
            if (!GlobalData.IsApplicationClosing && GlobalData.IsDragAndDropActive)
            {
                bool retry = true;
                Thread.Sleep(1000);
                while (GlobalData.IsDragAndDropActive && retry)
                {
                    if (
                        KryptonMessageBox.Show(
                        "Drag and Drop activites is in progress. Do you want to retry?\r\n\r\n" +
                        "Retry - Yes, I have already waited, please retry.\r\n" +
                        "Cacnel - No worries, I'll try later.\r\n",
                        "Opps, you was a little to quick on your hands.",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Exclamation, showCtrlCopy: true) == DialogResult.Retry)
                    {
                        retry = true;
                        Application.DoEvents();
                    }
                    else retry = false;
                }
            }
            return GlobalData.IsDragAndDropActive;
        }
        #endregion

        #region IsPopulatingButtonAction
        private string previousAction = "";
        private bool IsPerforminAButtonAction(string nameOfAction = "")
        {
            if (GlobalData.IsPerformingAButtonAction)
            {
                bool retry = true;
                Thread.Sleep(1000);
                while (GlobalData.IsPerformingAButtonAction && retry)
                {
                    if (
                        KryptonMessageBox.Show(
                        "Ongoing action in progress. " + previousAction + "\r\n" +
                        "Do you want to retry" + (string.IsNullOrWhiteSpace(nameOfAction) ? "" : " " + nameOfAction) + "?\r\n\r\n" +
                        "Retry - Yes, I have already waited, please retry.\r\n" +
                        "Cancel - No worries, I'll try later.\r\n",
                        "Opps, you was a little to quick on your hands.",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Exclamation, showCtrlCopy: true) == DialogResult.Retry)
                    {
                        retry = true;
                        Application.DoEvents();
                    }
                    else retry = false;
                }
            }
            else previousAction = nameOfAction;
            return GlobalData.IsPerformingAButtonAction;
        }
        #endregion 

        #region IsPopulatingAnything
        private bool IsPopulatingAnything(string nameOfAction = "")
        {
            if (!GlobalData.IsApplicationClosing && GlobalData.IsPopulatingAnything())
            {
                bool retry = true;
                Thread.Sleep(1000);
                while (GlobalData.IsPopulatingAnything() && retry)
                {
                    if (
                        KryptonMessageBox.Show(
                        "Populationg data in progress.\r\n" + GlobalData.WhatsPopulating() + "\r\n" +
                        "Do you want to wait? " + (string.IsNullOrWhiteSpace(nameOfAction) ? "" : " " + nameOfAction) + "?\r\n\r\n" +
                        "Retry - Yes, wait and check if process finished.\r\n" +
                        "Cancel - No worries, I'll try later.\r\n",
                        "Opps, you was a little to quick on your hands.",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Exclamation, showCtrlCopy: true) == DialogResult.Retry)
                    {
                        retry = true;
                        Application.DoEvents();
                    }
                    else retry = false;
                }
            }
            return GlobalData.IsPopulatingAnything();
        }
        #endregion 

        #region DataGridView - SaveBeforeContinue
        private DialogResult SaveBeforeContinue(bool canCancel, bool useAutoSave = false, string reason = null)
        {
            this.Activate();
            this.Validate(); //Get the latest changes, that are text in edit mode

            DialogResult dialogResult = DialogResult.No;
            try
            {
                if (IsAnyDataUnsaved())
                {

                    dialogResult = KryptonMessageBox.Show(
                        "Do you want to save before continue?\r\n\r\n" +
                        (string.IsNullOrEmpty(reason) ? "" : reason + "\r\n\r\n") +
                        "Yes - Save using " + (useAutoSave ? "" : "without ") +  "AutoCorrect and continue\r\n" +
                        "No - Don't save and continue without save." +
                        (canCancel ? "\r\nCancel - Cancel the opeation and continue where you left." : ""),
                        "Warning, unsaved data! Save before continue?",
                        (canCancel ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.YesNo), MessageBoxIcon.Warning, showCtrlCopy: true);

                    if (dialogResult == DialogResult.Yes)
                    {
                        //ActionSave(false);
                        SaveDataGridViewMetadata(useAutoSave);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return dialogResult;
        }
        #endregion

        #region DataGridView - IsAnyDataUnsaved
        private bool IsAnyDataUnsaved()
        {
            if (GlobalData.IsApplicationClosing) return false;
            bool isAnyDataUnsaved = false;

            try
            {
                int listOfUpdatesCount = 0;

                CollectMetadataFromAllDataGridViewData(out List <Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView, false);
                //Find what columns are updated / changed by user
                List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
                listOfUpdatesCount = listOfUpdates.Count;

                isAnyDataUnsaved = (listOfUpdatesCount > 0);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return isAnyDataUnsaved;
        }
        #endregion

        #region DataGridView - IsDataGridViewColumnDirty
        private bool IsDataGridViewColumnDirty(DataGridView dataGridView, int columnIndex, out string differences)
        {
            if (columnIndex == -1)
            {
                differences = "Column not found";
                return false;
            }
            differences = "";

            int listOfUpdatesCount = 0;
            try
            {
                List<Metadata> metadataListOriginalExiftool = new List<Metadata>();
                List<Metadata> metadataListFromDataGridView = new List<Metadata>();

                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                if (dataGridViewGenericColumn.IsPopulated)
                {
                    if (dataGridViewGenericColumn.Metadata != null) //throw new Exception("Missing needed metadata"); //This should not happen. Means it's not aggregated 
                    {
                        if (!FileEntryVersionHandler.IsReadOnlyColumnType(dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion)) //Only check columns User can edit
                        {
                            Metadata metadataFromDataGridView = new Metadata(dataGridViewGenericColumn.Metadata);
                            CollectedMetadataFromAllDataGridView(dataGridViewGenericColumn.FileEntryAttribute, ref metadataFromDataGridView);
                            metadataListOriginalExiftool.Add(new Metadata(dataGridViewGenericColumn.Metadata));
                            metadataListFromDataGridView.Add(new Metadata(metadataFromDataGridView));
                        }
                        else return false; //Was not a column a user can edit, can not be dirty
                    }
                    else
                    {
                        return false; 
                    }
                }
                else
                {
                    return false;
                }
            

                //Find what columns are updated / changed by user
                List<int> listOfUpdates = ExiftoolWriter.GetListOfMetadataChangedByUser(metadataListOriginalExiftool, metadataListFromDataGridView);
                differences = Metadata.GetErrors(metadataListFromDataGridView[0], metadataListOriginalExiftool[0], true);
                listOfUpdatesCount = listOfUpdates.Count;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return (listOfUpdatesCount > 0);
        }
        #endregion


        #region DataGridView - Updated Filenames

        #region DataGridView - UpdatedFilename - RenameRows
        private void DataGridViewUpdatedFilenameRenameRows(DataGridView dataGridView, string oldFullFileName, string newFullFileName)
        {
            string headerNewFilename = DataGridViewHandlerRename.headerNewFilename;
            
            try
            {
                int columnIndex = DataGridViewHandler.GetColumnIndexFirstFullFilePath(dataGridView, headerNewFilename, false);
                if (columnIndex != -1)
                {
                    for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCount(dataGridView); rowIndex++)
                    {
                        DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                        if (!dataGridViewGenericRow.IsHeader)
                        {
                            if (dataGridViewGenericRow.FileEntryAttribute != null)
                            {
                                if (dataGridViewGenericRow.FileEntryAttribute.FileFullPath == oldFullFileName)
                                {
                                    dataGridViewGenericRow.FileEntryAttribute = new FileEntryAttribute(
                                        newFullFileName,
                                        dataGridViewGenericRow.FileEntryAttribute.LastWriteDateTime,
                                        dataGridViewGenericRow.FileEntryAttribute.FileEntryVersion);
                                    dataGridViewGenericRow.RowName = dataGridViewGenericRow.FileEntryAttribute.FileName;
                                    DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, dataGridViewGenericRow);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - UpdatedFilename - ConvertAndMergeRows
        private void DataGridViewUpdatedFilenameConvertAndMergeRows(DataGridView dataGridView, string oldFullFileName, string newFullFileName)
        {
            string headerDirectory = DataGridViewHandlerConvertAndMerge.headerConvertAndMergeInfo;
            string headerNewFilename = DataGridViewHandlerConvertAndMerge.headerConvertAndMergeFilename;

            try
            {
                int columnIndex = DataGridViewHandler.GetColumnIndexFirstFullFilePath(dataGridView, headerNewFilename, false);
                if (columnIndex != -1)
                {
                    for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCount(dataGridView); rowIndex++)
                    {
                        DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                        if (!dataGridViewGenericRow.IsHeader)
                        {
                            if (dataGridViewGenericRow.FileEntryAttribute != null)
                            {
                                if (dataGridViewGenericRow.FileEntryAttribute.FileFullPath == oldFullFileName)
                                {
                                    dataGridViewGenericRow.FileEntryAttribute = new FileEntryAttribute(
                                        newFullFileName,
                                        dataGridViewGenericRow.FileEntryAttribute.LastWriteDateTime,
                                        dataGridViewGenericRow.FileEntryAttribute.FileEntryVersion);
                                    dataGridViewGenericRow.RowName = newFullFileName;
                                    DataGridViewHandler.SetRowHeaderNameAndFontStyle(dataGridView, rowIndex, dataGridViewGenericRow);
                                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, dataGridViewGenericRow.FileEntryAttribute.FileName, false);
                                }
                            }
                        }
                    }
                }
                DataGridView_UpdatedDirtyFlags(dataGridView);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - UpdatedFilename - Columns
        private void DataGridViewUpdatedFilenameColumns(DataGridView dataGridView, string oldFullFileName, string newFullFileName)
        {
            try
            {
                for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
                {
                    DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    if (dataGridViewGenericColumn.IsPopulated)
                    {
                        if (dataGridViewGenericColumn.FileEntryAttribute != null)
                        {
                            if (dataGridViewGenericColumn.FileEntryAttribute.FileFullPath == oldFullFileName)
                            {
                                dataGridViewGenericColumn.FileEntryAttribute = new FileEntryAttribute(
                                    newFullFileName,
                                    dataGridViewGenericColumn.FileEntryAttribute.LastWriteDateTime,
                                    dataGridViewGenericColumn.FileEntryAttribute.FileEntryVersion);

                                DataGridViewHandler.SetColumnDataGridViewName(dataGridView, columnIndex, dataGridViewGenericColumn.FileEntryAttribute.FileFullPath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion
        
        #endregion

        #region DataGridView - CollectMetadataFromAllDataGridViewData - FileEntry
        private void CollectedMetadataFromAllDataGridView(FileEntryAttribute fileEntryAttribute, ref Metadata metadataFromDataGridView)
        {
            try
            {
                if (GlobalData.IsAgregatedTags) DataGridViewHandlerTagsAndKeywords.GetUserInputChanges(dataGridViewTagsAndKeywords, ref metadataFromDataGridView, fileEntryAttribute);
                if (GlobalData.IsAgregatedMap) DataGridViewHandlerMap.GetUserInputChanges(dataGridViewMap, ref metadataFromDataGridView, fileEntryAttribute);
                if (GlobalData.IsAgregatedPeople) DataGridViewHandlerPeople.GetUserInputChanges(dataGridViewPeople, ref metadataFromDataGridView, fileEntryAttribute);
                if (GlobalData.IsAgregatedDate) DataGridViewHandlerDate.GetUserInputChanges(dataGridViewDate, ref metadataFromDataGridView, fileEntryAttribute);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - CollectMetadataFromAllDataGridViewData - All
        private void CollectMetadataFromAllDataGridViewData(out List<Metadata> metadataListOriginalExiftool, out List<Metadata> metadataListFromDataGridView, bool clearDirtyFlagAndUpdatedMetadata)
        {
            metadataListOriginalExiftool = new List<Metadata>();
            metadataListFromDataGridView = new List<Metadata>();
            try
            {
                DataGridView dataGridView = GetAnyAgregatedDataGridView();
                List<DataGridViewGenericColumn> dataGridViewGenericColumnList = DataGridViewHandler.GetColumnsDataGridViewGenericColumnCurrentOrAutoCorrect(dataGridView, true);
                foreach (DataGridViewGenericColumn dataGridViewGenericColumn in dataGridViewGenericColumnList)
                {
                    if (dataGridViewGenericColumn.IsPopulated)
                    {
                        if (dataGridViewGenericColumn.Metadata == null)
                        {
                            throw new Exception("Missing needed metadata"); //This should not happen. Means it's nt aggregated 
                        }

                        Metadata metadataFromDataGridView = new Metadata(dataGridViewGenericColumn.Metadata);

                        CollectedMetadataFromAllDataGridView(dataGridViewGenericColumn.FileEntryAttribute, ref metadataFromDataGridView);

                        metadataListOriginalExiftool.Add(new Metadata(dataGridViewGenericColumn.Metadata));
                        metadataListFromDataGridView.Add(new Metadata(metadataFromDataGridView));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - DataGridViewSetMetadataOnAllDataGridView - Metadata
        private void DataGridViewSetMetadataOnAllDataGridView(Metadata metadataFixedAndCorrected)
        {
            try
            {
                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(metadataFixedAndCorrected.FileEntry, FileEntryVersion.MetadataToSave);

                int debugColumn = -1;

                #region TagsAndKeywords
                int columnIndexTagsAndKeywords = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridViewTagsAndKeywords, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompareTagsAndKeywords);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewTagsAndKeywords, columnIndexTagsAndKeywords))
                    DataGridViewHandler.SetColumnHeaderMetadata(dataGridViewTagsAndKeywords, metadataFixedAndCorrected, columnIndexTagsAndKeywords);

                if (columnIndexTagsAndKeywords != -1) debugColumn = columnIndexTagsAndKeywords;
                if (debugColumn != -1 && columnIndexTagsAndKeywords != -1 && debugColumn != columnIndexTagsAndKeywords)
                {
                    //DEBUG
                    Logger.Warn("Column updated between user action and thread");
                }
                #endregion

                #region People
                int columnIndexPeople = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridViewPeople, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionComparePeople);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewPeople, columnIndexPeople))
                    DataGridViewHandler.SetColumnHeaderMetadata(dataGridViewPeople, metadataFixedAndCorrected, columnIndexPeople);
                if (columnIndexPeople != -1) debugColumn = columnIndexPeople;
                if (debugColumn != -1 && columnIndexPeople != -1 && debugColumn != columnIndexPeople)
                {
                    //DEBUG
                    Logger.Warn("Column updated between user action and thread");
                }
                #endregion

                #region Map
                int columnIndexMap = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridViewMap, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompareMap);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewMap, columnIndexMap))
                    DataGridViewHandler.SetColumnHeaderMetadata(dataGridViewMap, metadataFixedAndCorrected, columnIndexMap);
                if (columnIndexMap != -1) debugColumn = columnIndexMap;
                if (debugColumn != -1 && columnIndexMap != -1 && debugColumn != columnIndexMap)
                {
                    //DEBUG
                    Logger.Warn("Column updated between user action and thread");
                }
                #endregion

                #region Date
                int columnIndexDate = DataGridViewHandler.GetColumnIndexWhenAddColumn(dataGridViewDate, fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompareDate);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewDate, columnIndexDate))
                    DataGridViewHandler.SetColumnHeaderMetadata(dataGridViewDate, metadataFixedAndCorrected, columnIndexDate);
                if (columnIndexDate != -1) debugColumn = columnIndexDate;
                if (debugColumn != -1 && columnIndexDate != -1 && debugColumn != columnIndexDate)
                {
                    //DEBUG 
                    Logger.Warn("Column updated between user action and thread");
                }
                #endregion

                #region Rename

                #endregion

                #region ConvertAndMerge

                #endregion

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - UpdatedDirtyFlags
        private void DataGridView_UpdatedDirtyFlags(DataGridView dataGridView)
        {
            try
            {
                if (DataGridViewHandler.GetIsAgregated(dataGridView))
                {
                    for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
                    {
                        DataGridViewHandler.SetColumnDirtyFlag(dataGridView, columnIndex, IsDataGridViewColumnDirty(dataGridView, columnIndex, out string differences), differences);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - DataGridViewSetDirtyFlagAfterSave - Metadata
        private void DataGridViewSetDirtyFlagAfterSave(Metadata metadataFixedAndCorrected, bool isDirty, FileEntryVersion fileEntryVersion)
        {
            try
            {
                FileEntryAttribute fileEntryAttribute = new FileEntryAttribute(metadataFixedAndCorrected.FileEntry, fileEntryVersion);
                int columnIndex = DataGridViewHandler.GetColumnIndexWhenAddColumn(GetAnyAgregatedDataGridView(), fileEntryAttribute, out FileEntryVersionCompare fileEntryVersionCompare);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewTagsAndKeywords, columnIndex))
                    DataGridViewHandler.SetColumnDirtyFlag(dataGridViewTagsAndKeywords, columnIndex, isDirty);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewPeople, columnIndex))
                    DataGridViewHandler.SetColumnDirtyFlag(dataGridViewPeople, columnIndex, isDirty);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewMap, columnIndex))
                    DataGridViewHandler.SetColumnDirtyFlag(dataGridViewMap, columnIndex, isDirty);
                if (DataGridViewHandler.IsColumnPopulated(dataGridViewDate, columnIndex))
                    DataGridViewHandler.SetColumnDirtyFlag(dataGridViewDate, columnIndex, isDirty);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region DataGridView - GetSelectedFilesFromActiveDataGridView
        private DataGridViewHandler.GetSelectFileEntriesMode DataGridView_GetSelectedFilesModeFromActive()
        {
            DataGridViewHandler.GetSelectFileEntriesMode mode = DataGridViewHandler.GetSelectFileEntriesMode.None;
            switch (ActiveKryptonPage)
            {
                case KryptonPages.None:
                case KryptonPages.kryptonPageFolderSearchFilterFolder:
                case KryptonPages.kryptonPageFolderSearchFilterSearch:
                case KryptonPages.kryptonPageFolderSearchFilterFilter:
                case KryptonPages.kryptonPageMediaFiles:
                    mode = DataGridViewHandler.GetSelectFileEntriesMode.None;
                    break;
                case KryptonPages.kryptonPageToolboxTags:
                case KryptonPages.kryptonPageToolboxPeople:
                case KryptonPages.kryptonPageToolboxMap:
                case KryptonPages.kryptonPageToolboxDates:
                case KryptonPages.kryptonPageToolboxExiftool:
                case KryptonPages.kryptonPageToolboxWarnings:
                case KryptonPages.kryptonPageToolboxProperties:
                    mode = DataGridViewHandler.GetSelectFileEntriesMode.Columns;
                    break;
                case KryptonPages.kryptonPageToolboxRename:
                case KryptonPages.kryptonPageToolboxConvertAndMerge:
                    mode = DataGridViewHandler.GetSelectFileEntriesMode.Rows;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return mode;
        }

        private HashSet<FileEntry> DataGridView_GetSelectedFilesFromActive()
        {
            HashSet<FileEntry> files = new HashSet<FileEntry>();
            try
            {
                DataGridViewHandler.GetSelectFileEntriesMode mode = DataGridView_GetSelectedFilesModeFromActive();
                files = DataGridViewHandler.GetSelectFileEntries(GetActiveTabDataGridView(), mode);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            return files;
        }
        #endregion

        #region DataGridView - Select DataGridView - All UsingFileEntry

        private void SelectDataGridViewAllUsingFileEntry(HashSet<FileEntry> selectedFileEntries)
        {
            SelectAndMatchDataGridViewRows(dataGridViewConvertAndMerge, selectedFileEntries);
            SelectDataGridViewColumn(dataGridViewDate, selectedFileEntries);
            SelectDataGridViewColumn(dataGridViewExiftool, selectedFileEntries);
            SelectDataGridViewColumn(dataGridViewExiftoolWarning, selectedFileEntries);
            SelectDataGridViewColumn(dataGridViewMap, selectedFileEntries);
            SelectDataGridViewColumn(dataGridViewPeople, selectedFileEntries);
            SelectDataGridViewColumn(dataGridViewProperties, selectedFileEntries);
            SelectAndMatchDataGridViewRows(dataGridViewRename, selectedFileEntries);
            SelectDataGridViewColumn(dataGridViewTagsAndKeywords, selectedFileEntries);
        }

        private void SelectDataGridViewColumn(DataGridView dataGridView, HashSet<FileEntry> selectedFileEntries)
        {
            dataGridView.ClearSelection();
            for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                if (dataGridViewGenericColumn != null && selectedFileEntries.Contains(dataGridViewGenericColumn.FileEntryAttribute.FileEntry))
                    DataGridViewHandler.SelectColumnRows(dataGridView, columnIndex, true);
                else
                    DataGridViewHandler.SelectColumnRows(dataGridView, columnIndex, false);
            }
        }

        private void SelectAndMatchDataGridViewRows(DataGridView dataGridView, HashSet<FileEntry> selectedFileEntries)
        {
            dataGridView.ClearSelection();
            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow?.FileEntryAttribute != null && selectedFileEntries.Contains(dataGridViewGenericRow?.FileEntryAttribute.FileEntry))
                    dataGridView.Rows[rowIndex].Selected = true;
                else
                    dataGridView.Rows[rowIndex].Selected = false;
            }
        }

        #endregion

        #region DataGridView - Rename Header - Database
        private void Database_Rename(string oldDirectory, string oldFilename, string newDirectory, string newFilename)
        {
            databaseAndCacheThumbnailPoster.Move(oldDirectory, oldFilename, newDirectory, newFilename);
            if (!databaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename))
            {
                filesCutCopyPasteDrag.DeleteFileAndHistory(oldFilename);
                databaseAndCacheThumbnailPoster.Move(oldDirectory, oldFilename, newDirectory, newFilename);
                databaseAndCacheMetadataExiftool.Move(oldDirectory, oldFilename, newDirectory, newFilename);
            }
        }
        #endregion

        #region DataGridView - Rename Header
        private void DataGridView_Rename_Invoke(string oldFullFilename, string newFullFilename)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string, string>(DataGridView_Rename_Invoke), oldFullFilename, newFullFilename);
                return;
            }

            ImageListViewHandler.ClearCacheFileEntries(imageListView1);
            ImageListViewHandler.ClearCacheFileEntriesSelectedItems(imageListView1);
            try
            {
                if (GlobalData.IsAgregatedTags) DataGridViewUpdatedFilenameColumns(dataGridViewTagsAndKeywords, oldFullFilename, newFullFilename);
                if (GlobalData.IsAgregatedMap) DataGridViewUpdatedFilenameColumns(dataGridViewMap, oldFullFilename, newFullFilename);
                if (GlobalData.IsAgregatedPeople) DataGridViewUpdatedFilenameColumns(dataGridViewPeople, oldFullFilename, newFullFilename);
                if (GlobalData.IsAgregatedDate) DataGridViewUpdatedFilenameColumns(dataGridViewDate, oldFullFilename, newFullFilename);

                if (GlobalData.IsAgregatedExiftoolTags) DataGridViewUpdatedFilenameColumns(dataGridViewExiftool, oldFullFilename, newFullFilename);
                if (GlobalData.IsAgregatedExiftoolWarning) DataGridViewUpdatedFilenameColumns(dataGridViewExiftoolWarning, oldFullFilename, newFullFilename);
                if (GlobalData.IsAgregatedProperties) DataGridViewUpdatedFilenameColumns(dataGridViewProperties, oldFullFilename, newFullFilename);

                if (GlobalData.IsAgregatedConvertAndMerge) DataGridViewUpdatedFilenameConvertAndMergeRows(dataGridViewConvertAndMerge, oldFullFilename, newFullFilename);
                if (GlobalData.IsAgregatedRename) DataGridViewUpdatedFilenameRenameRows(dataGridViewRename, oldFullFilename, newFullFilename);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show(ex.Message, "Syntax error...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion
    }
}
