using System;
using System.Drawing;
using System.Windows.Forms;
using DataGridViewGeneric;
using System.Collections.Generic;
using MetadataLibrary;
using LocationNames;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        //Convert and Merge
        //Date
        //Exiftool
        //ExiftoolWarning
        //Map
        //People
        //Properties
        //Rename
        //TagsAndKeywords

        #region Cut
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion
        //Exiftool
        //ExiftoolWarning

        #region Map
        private void toolStripMenuItemMapCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //Properties
        //Rename
        
        #region TagsAndKeywords
        private void cutToolStripMenuTagsBrokerCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, 0, dataGridView.Columns.Count - 1,
                DataGridViewHandler.GetRowHeadingIndex(dataGridView, header),
                DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header), true);
            ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Copy
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
        }
        #endregion

        #region Exiftool
        private void toolStripMenuItemExiftoolCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        //Properties
        //Rename

        #region TagsAndKeywords
        private void copyToolStripMenuTagsBrokerCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
        }
        #endregion

        #endregion

        #region Paste
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDatePaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        //Exiftool
        //ExiftoolWarning

        #region Map
        private void toolStripMenuItemMapPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeoplePaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void pasteToolStripMenuTagsBrokerPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(
                dataGridView, 0, dataGridView.Columns.Count - 1,
                DataGridViewHandler.GetRowHeadingIndex(dataGridView, header),
                DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header), true);
            ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Delete
        //Convert and Merge
        //Date
        private void toolStripMenuItemDateDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        //Exiftool
        //ExiftoolWarning

        #region Map
        private void toolStripMenuItemMapDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void deleteToolStripMenuTagsBrokerDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, 0, dataGridView.Columns.Count - 1,
                DataGridViewHandler.GetRowHeadingIndex(dataGridView, header),
                DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, header), true);
            ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Undo
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.UndoDataGridView(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        //Exiftool
        //ExiftoolWarning
        #region Map
        private void toolStripMenuItemMapUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.UndoDataGridView(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            ClipboardUtility.UndoDataGridView(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void undoToolStripMenuTags_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.UndoDataGridView(dataGridView);
            ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Redo
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        //Exiftool
        //ExiftoolWarning
        #region Map
        private void toolStripMenuItemMapRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void redoToolStripMenuTags_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Find
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //Properties
        //Rename

        #region TagsAndKeywords
        private void findToolStripMenuTag_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion

        #region Find and Replace - *** Need debug why not implemented ***
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //ExiftoolWarning
        #region Map
        private void toolStripMenuItemMapReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void replaceToolStripMenuTag_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;

            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #endregion


        #region Mark Favorite
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Exiftool
        private void toolStripMenuItemExiftoolMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        //Properties
        //Rename

        #region TagsAndKeywords
        private void markAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #endregion

        #region Remove Favorite
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void removeAsFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #endregion

        #region Toogle Favorite
        //Convert and Merge
        #region Date
        private void toolStripMenuItemDateToggleFavourite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void toggleFavoriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #endregion

        #region Show only Favorite 
        
        #region Date
        private void toolStripMenuItemShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemDateShowFavorite);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemDateHideEqualRows.Checked, toolStripMenuItemDateShowFavorite.Checked);
        }
        #endregion 

        #region Exiftool
        private void toolStripMenuItemExiftoolShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemExiftoolSHowFavorite);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemExiftoolHideEqual.Checked, toolStripMenuItemExiftoolSHowFavorite.Checked);
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemExiftoolWarningShowFavorite);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemExiftoolWarningHideEqual.Checked, toolStripMenuItemExiftoolWarningShowFavorite.Checked);
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemMapShowFavorite);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemMapHideEqual.Checked, toolStripMenuItemMapShowFavorite.Checked);
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemPeopleShowFavorite);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemPeopleHideEqualRows.Checked, toolStripMenuItemPeopleShowFavorite.Checked);
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void showFavoriteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemKeywordsShowFavoriteRows);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemKeywordsHideEqualRows.Checked, toolStripMenuItemKeywordsShowFavoriteRows.Checked);
        }
        #endregion

        #endregion

        #region Hide Equal Rows
        #region  Date
        private void toolStripMenuItemDateHideEqualRows_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewDate;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemDateHideEqualRows);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemDateHideEqualRows.Checked, toolStripMenuItemDateShowFavorite.Checked);
        }
        #endregion

        #region Exiftool
        private void toolStripMenuItemExiftoolHideEqual_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifTool;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemExiftoolHideEqual);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemExiftoolHideEqual.Checked, toolStripMenuItemExiftoolSHowFavorite.Checked);
        }
        #endregion

        #region ExiftoolWarning
        private void toolStripMenuItemExiftoolWarningHideEqual_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewExifToolWarning;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemExiftoolWarningHideEqual);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemExiftoolWarningHideEqual.Checked, toolStripMenuItemExiftoolWarningShowFavorite.Checked);
        }
        #endregion

        #region Map
        private void toolStripMenuItemMapHideEqual_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemMapHideEqual);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemMapHideEqual.Checked, toolStripMenuItemMapShowFavorite.Checked);
        }
        #endregion

        #region People
        private void toolStripMenuItemPeopleHideEqualRows_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemPeopleHideEqualRows);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemPeopleHideEqualRows.Checked, toolStripMenuItemPeopleShowFavorite.Checked);
        }
        #endregion

        //Properties
        //Rename
        #region TagsAndKeywords
        private void hideEqualRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemKeywordsHideEqualRows);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, toolStripMenuItemKeywordsHideEqualRows.Checked, toolStripMenuItemKeywordsShowFavoriteRows.Checked);
        }
        #endregion

        #endregion


        #region TriState Buttons - Toogle and set state
        private void TagActionToggle(DataGridView dataGridView, string header, NewState newState)
        {
            DataGridViewHandler.ToggleSelected(dataGridView, header, newState);
            ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region TriState Buttons - Toogle 
        private void toggleTagSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Toggle);
        }
        private void toolStripMenuItemPeopleTogglePeopleTag_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Toggle);
        }
        #endregion

        #region TriState Buttons - Set
        private void selectTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords, NewState.Set);
        }

        private void toolStripMenuItemPeopleSelectPeopleTag_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Set);
        }
        #endregion

        #region TriState Buttons - Remove
        private void removeTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewTagsAndKeywords;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerTagsAndKeywords.headerKeywords,NewState.Remove);
        }

        private void toolStripMenuItemPeopleRemovePeopleTag_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewPeople;
            if (!dataGridView.Enabled) return;

            TagActionToggle(dataGridView, DataGridViewHandlerPeople.headerPeople, NewState.Remove);
        }
        #endregion


        #region Copy text to Media and overwrite
        private void toolStripMenuItemTagsCopyText_Click(object sender, EventArgs e)
        {
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, false);
        }

        private void toolStripMenuItemMapCopyNotOverwrite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridView, DataGridViewHandlerMap.headerMedia, false);
        }
        #endregion 

        #region Copy text to Media and NOT overwrite
        private void toolStripMenuItemTagsOverwriteText_Click(object sender, EventArgs e)
        {
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridViewTagsAndKeywords, DataGridViewHandlerTagsAndKeywords.headerMedia, true);
        }

        private void toolStripMenuItemMapCopyAndOverwrite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            DataGridViewHandler.CopySelectedCellFromBrokerToMedia(dataGridView, DataGridViewHandlerMap.headerMedia, true);


            List<int> columnUpdated = new List<int>();

            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {
                if (!columnUpdated.Contains(dataGridViewCell.ColumnIndex))
                {
                    DataGridViewGenericColumn gridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, dataGridViewCell.ColumnIndex);
                    if (gridViewGenericColumn.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                    {
                        DataGridViewGenericRow gridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, dataGridViewCell.RowIndex);
                        //gridViewGenericRow.HeaderName.Equals(DataGridViewHandlerMap.headerMedia) &&

                        if (!gridViewGenericRow.HeaderName.Equals(DataGridViewHandlerMap.headerMedia) &&
                            gridViewGenericRow.RowName.Equals(DataGridViewHandlerMap.tagCoordinates))
                        {
                            object cellValue = DataGridViewHandler.GetCellValue(dataGridViewMap, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex);
                            if (cellValue != null)
                            {
                                string coordinate = cellValue.ToString();
                                //UpdateBrowserMap(coordinate);
                                DataGridViewHandlerMap.PopulateGrivViewMapNomnatatim(dataGridView, dataGridViewCell.ColumnIndex, LocationCoordinate.Parse(coordinate));
                                columnUpdated.Add(dataGridViewCell.ColumnIndex);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region DataGridView Keydown

        #region Keydown - Convert And Merge
        private void dataGridViewConvertAndMerge_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Date
        private void dataGridViewDate_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Exiftool
        private void dataGridViewExifTool_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - ExiftoolWarning
        private void dataGridViewExifToolWarningData_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Map
        private void dataGridMap_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - People
        private void dataGridViewPeople_KeyDown(object sender, KeyEventArgs e)
        {
            triStateButtomClick = false;
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Properties
        private void dataGridViewRename_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Keydown - Rename
        private void dataGridViewProperties_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion 

        #region Keydown - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_KeyDown(object sender, KeyEventArgs e)
        {
            triStateButtomClick = false; 
        }
        #endregion

        #endregion


        #region Cell BeginEdit

        #region Cell BeginEdit - Date
        private void dataGridViewDate_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Exiftool
        private void dataGridViewExifTool_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - ExiftoolWarning
        private void dataGridViewExifToolWarning_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Map
        private void dataGridViewMap_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - People
        private void dataGridViewPeople_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (triStateButtomClick)
            {
                e.Cancel = true;
                return;
            }

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Properties
        private void dataGridViewProperties_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - Rename
        private void dataGridViewRename_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Cell BeginEdit - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (triStateButtomClick)
            {
                e.Cancel = true;
                return;
            }

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #endregion

        #region Cell Painting

        #region Cell Painting - Convert and Merge
        private void dataGridViewConvertAndMerge_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
        #endregion 

        #region Cell Painting - Date
        private void dataGridViewDate_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - Exiftool
        private void dataGridViewExifTool_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - ExiftoolWarning
        private void dataGridViewExifToolWarning_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - Map
        private void dataGridViewMap_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - People
        private void dataGridViewPeople_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            string header = DataGridViewHandlerPeople.headerPeople;

            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            DataGridViewHandler.CellPaintingColumnHeaderRegionsInThumbnail(sender, e);
            DataGridViewHandler.CellPaintingColumnHeaderMouseRegion(sender, e, drawingRegion, peopleMouseDownX, peopleMouseDownY, peopleMouseMoveX, peopleMouseMoveY);

            DataGridViewGenericRow gridViewGenericDataRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
            if (gridViewGenericDataRow == null) return; //Don't paint anything TriState on "New Empty Row" for "new Keywords"

            DataGridViewGenericColumn dataGridViewGenericDataColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
            if (e.ColumnIndex > -1)
            {
                if (dataGridViewGenericDataColumn == null) return; //Data is not set, no point to check more.
                if (dataGridViewGenericDataColumn.Metadata == null) return; //Don't paint TriState button when MetaData is null (data not loaded)
            }

            //If people region row
            if (gridViewGenericDataRow.HeaderName.Equals(DataGridViewHandlerPeople.headerPeople))
            {
                if (!gridViewGenericDataRow.IsHeader && e.ColumnIndex > -1)
                {
                    MetadataLibrary.RegionStructure region = DataGridViewHandler.GetCellRegionStructure(dataGridView, e.ColumnIndex, e.RowIndex);
                    Image regionThumbnail = (Image)Properties.Resources.FaceLoading;
                    if (region == null)
                    {
                        e.Handled = false;
                        return;
                    }
                    else if (region.Thumbnail != null) regionThumbnail = region.Thumbnail;
                    DataGridViewHandler.DrawImageAndSubText(sender, e, regionThumbnail, e.Value.ToString(), DataGridViewHandler.ColorHeaderImage);

                    e.Handled = true;
                }

                DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            }
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            
        }
        #endregion

        #region Cell Painting - Properties
        private void dataGridViewProperties_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - Rename
        private void dataGridViewRename_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
            //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Cell Painting - TagsAndKeywords
        private void dataGridViewTagsAndKeywords_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            DataGridViewHandler.CellPaintingHandleDefault(sender, e, false);
            DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
            
        }
        #endregion

        #endregion

        

        //ConvertAndMerge
        //Date
        //Exiftool
        //ExiftoolWarning
        //Map
        //People
        //Properties
        //Rename
        //TagsAndKeywords


    }
}
