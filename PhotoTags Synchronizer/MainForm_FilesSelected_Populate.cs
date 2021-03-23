
using System.Windows.Forms;


namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        #region FilesSelected - Populate DataGridVIew, OpenWith...
        private void FilesSelected()
        {
            if (GlobalData.IsPopulatingAnything()) return; //E.g. Populate FolderSelect
            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;

            if (imageListView1.SelectedItems.Count == 0)
            {
                toolStripMenuItemImageListViewCut.Enabled = false;
                toolStripMenuItemImageListViewCopy.Enabled = false;
                copyFileNamesToClipboardToolStripMenuItem.Enabled = false;
                toolStripMenuItemImageListViewPaste.Enabled = false;
                toolStripMenuItemImageListViewDelete.Enabled = false;
                toolStripMenuItemImageListViewRefreshFolder.Enabled = true;
                toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Enabled = false;
                toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Enabled = false;
                toolStripMenuItemImageListViewSelectAll.Enabled = false;
                toolStripMenuItemImageListViewAutoCorrect.Enabled = false;
                openFileWithAssociatedApplicationToolStripMenuItem.Enabled = false;
                openMediaFilesWithToolStripMenuItem.Enabled = false;
                editFileWithAssociatedApplicationToolStripMenuItem.Enabled = false;
                runSelectedToolStripMenuItem1.Enabled = false;
                openWithDialogToolStripMenuItem.Enabled = false;
                openFileLocationToolStripMenuItem.Enabled = false;

                toolStripButtonPreview.Enabled = false;
                mediaPreviewToolStripMenuItem.Enabled = false;
                rotateCCWToolStripButton.Enabled = false;
                ratateCCW270ToolStripMenuItem.Enabled = false;
                rotate180ToolStripButton.Enabled = false;
                rotate180ToolStripMenuItem.Enabled = false;
                rotateCWToolStripButton.Enabled = false;
                rotateCW90ToolStripMenuItem.Enabled = false;
            }
            else
            {

                toolStripMenuItemImageListViewCut.Enabled = true;
                toolStripMenuItemImageListViewCopy.Enabled = true;
                copyFileNamesToClipboardToolStripMenuItem.Enabled = true;
                toolStripMenuItemImageListViewPaste.Enabled = true;
                toolStripMenuItemImageListViewDelete.Enabled = true;
                toolStripMenuItemImageListViewRefreshFolder.Enabled = true;
                toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Enabled = true;
                toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Enabled = true;
                toolStripMenuItemImageListViewSelectAll.Enabled = true;
                toolStripMenuItemImageListViewAutoCorrect.Enabled = true;
                openFileWithAssociatedApplicationToolStripMenuItem.Enabled = true;
                openMediaFilesWithToolStripMenuItem.Enabled = true;
                editFileWithAssociatedApplicationToolStripMenuItem.Enabled = true;
                runSelectedToolStripMenuItem1.Enabled = true;
                openWithDialogToolStripMenuItem.Enabled = true;
                openFileLocationToolStripMenuItem.Enabled = true;

                toolStripButtonPreview.Enabled = true;
                mediaPreviewToolStripMenuItem.Enabled = true;
                rotateCCWToolStripButton.Enabled = true;
                ratateCCW270ToolStripMenuItem.Enabled = true;
                rotate180ToolStripButton.Enabled = true;
                rotate180ToolStripMenuItem.Enabled = true;
                rotateCWToolStripButton.Enabled = true;
                rotateCW90ToolStripMenuItem.Enabled = true;
            }

            
            
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingImageListView = true;
                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                PopulateDataGridViewForSelectedItemsInvoke(imageListView1.SelectedItems);
                PopulateImageListViewOpenWithToolStripThread(imageListView1.SelectedItems);

                DisplayAllQueueStatus();
                GlobalData.IsPopulatingImageListView = false;
            }
        }
        #endregion
    }
}
