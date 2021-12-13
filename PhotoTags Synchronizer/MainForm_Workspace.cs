using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DataGridViewGeneric;
using FileDateTime;
using Manina.Windows.Forms;
using MetadataLibrary;
using WindowsProperty;
using static Manina.Windows.Forms.ImageListView;
using Krypton.Toolkit;
using Krypton.Navigator;
using System.Diagnostics;
using System.IO;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region Workspace - FindWorkspaceCell
        private Krypton.Workspace.KryptonWorkspaceCell FindWorkspaceCell(Krypton.Workspace.KryptonWorkspace kryptonWorkspace, string name)
        {
            Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell = kryptonWorkspace.FirstVisibleCell();
            while (kryptonWorkspaceCell != null)
            {
                if (kryptonWorkspaceCell.Name == name)
                {
                    return kryptonWorkspaceCell;
                }
                kryptonWorkspaceCell = kryptonWorkspace.NextVisibleCell(kryptonWorkspaceCell);
            }
            return null;
        }
        #endregion

        #region Workspace - ActionMaximumWorkspaceCell
        private void ActionMaximumWorkspaceCell(Krypton.Workspace.KryptonWorkspace kryptonWorkspace, Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell)
        {
            kryptonWorkspace.MaximizedCell = kryptonWorkspaceCell;
        }
        #endregion

        #region WorkspaceCellToolboxTags - MaximizeRestore

        #region WorkspaceCellToolboxTags - MaximizeOrRestore()
        private void WorkspaceCellToolboxTagsMaximizeOrRestore()
        {
            switch (GetActiveTabTag())
            {
                case LinkTabAndDataGridViewNameTags:
                    Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell = FindWorkspaceCell(kryptonWorkspaceToolboxTags, Properties.Settings.Default.WorkspaceToolboxTagsMaximizedCell);
                    ActionMaximumWorkspaceCell(kryptonWorkspaceToolboxTags, kryptonWorkspaceCell);
                    break;
                case LinkTabAndDataGridViewNameMap:
                case LinkTabAndDataGridViewNamePeople:
                case LinkTabAndDataGridViewNameDates:
                case LinkTabAndDataGridViewNameExiftool:
                case LinkTabAndDataGridViewNameWarnings:
                case LinkTabAndDataGridViewNameProperties:
                case LinkTabAndDataGridViewNameRename:
                case LinkTabAndDataGridViewNameConvertAndMerge:
                    break;
                default: throw new NotImplementedException();
            }
        }
        #endregion 

        #region WorkspaceCellToolboxTags - MaximizeRestore - Click
        private void ActionMaximizeRestoreWorkspaceCellToolboxTags()
        {
            Properties.Settings.Default.WorkspaceToolboxTagsMaximizedCell = kryptonWorkspaceToolboxTags?.MaximizedCell?.Name ?? "";
        }
        #endregion

        #region WorkspacToolboxTags - MaximizeRestore - Click
        private void kryptonWorkspaceCellToolboxTagsDetails_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            ActionMaximizeRestoreWorkspaceCellToolboxTags();
        }

        private void kryptonWorkspaceCellToolboxTagsKeywords_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            ActionMaximizeRestoreWorkspaceCellToolboxTags();
        }
        #endregion

        #endregion

        #region WorkspaceMain - MaximizeRestore

        #region WorkspaceMain - MaximizeWorkspaceMainCell
        private void MaximizeOrRestoreWorkspaceMainCellAndChilds()
        {
            Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell = FindWorkspaceCell(kryptonWorkspaceMain, Properties.Settings.Default.WorkspaceMainMaximizedCell);
            ActionMaximumWorkspaceCell(kryptonWorkspaceMain, kryptonWorkspaceCell);
            WorkspaceCellToolboxTagsMaximizeOrRestore();
        }
        #endregion

        #region WorkspaceMain - ActionMaximizeRestoreWorkspaceMain
        private void ActionMaximizeRestoreWorkspaceMain()
        {
            Properties.Settings.Default.WorkspaceMainMaximizedCell = kryptonWorkspaceMain?.MaximizedCell?.Name ?? "";
            WorkspaceCellToolboxTagsMaximizeOrRestore(); //Not restore this but childs
        }
        #endregion

        #region WorkspaceMain - MaximizeRestore - Click
        private void kryptonWorkspaceCellFolderSearchFilter_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            SetNavigatorModeSearch(NavigatorMode.OutlookFull);
            ActionMaximizeRestoreWorkspaceMain();
        }

        private void kryptonWorkspaceCellMediaFiles_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            ActionMaximizeRestoreWorkspaceMain();
        }

        private void kryptonWorkspaceCellToolbox_MaximizeRestoreClicked(object sender, EventArgs e)
        {
            ActionMaximizeRestoreWorkspaceMain();
        }
        #endregion

        #endregion

        #region Workspace -- SelectedPageChanged --
        private void kryptonWorkspaceCellToolbox_SelectedPageChanged(object sender, EventArgs e)
        {
            if (isFormLoading) return;
            try
            {
                ActionMaximumWorkspaceCell(kryptonWorkspaceMain, FindWorkspaceCell(kryptonWorkspaceMain, Properties.Settings.Default.WorkspaceMainMaximizedCell)); //Need to be in front of ActionMaximumWorkspaceCell(kryptonWorkspaceToolboxTags, kryptonWorkspaceToolboxTagPrevious);

                switch (GetActiveTabTag())
                {
                    case LinkTabAndDataGridViewNameTags:
                        ActionMaximumWorkspaceCell(kryptonWorkspaceToolboxTags, FindWorkspaceCell(kryptonWorkspaceToolboxTags, Properties.Settings.Default.WorkspaceToolboxTagsMaximizedCell));
                        break;
                    case LinkTabAndDataGridViewNameMap:
                    case LinkTabAndDataGridViewNamePeople:
                    case LinkTabAndDataGridViewNameDates:
                    case LinkTabAndDataGridViewNameExiftool:
                    case LinkTabAndDataGridViewNameWarnings:
                    case LinkTabAndDataGridViewNameProperties:
                    case LinkTabAndDataGridViewNameRename:
                    case LinkTabAndDataGridViewNameConvertAndMerge:
                        break;
                    default: throw new NotImplementedException();
                }
                DataGridView_Populate_SelectedItemsThread(ImageListViewHandler.GetFileEntriesSelectedItemsCache(imageListView1, true));
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to to populate data grid view", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
                Logger.Error(ex);
            }
        }
        #endregion

    }
}
