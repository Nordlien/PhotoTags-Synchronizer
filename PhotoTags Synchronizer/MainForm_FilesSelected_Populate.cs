
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
            
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingImageListView = true;
                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                PopulateDataGridViewForSelectedItemsInvoke(imageListView1.SelectedItems);
                PopulateImageListViewOpenWithToolStripThread(imageListView1.SelectedItems);

                DisplayAllQueueStatus();
                GlobalData.IsPopulatingImageListView = false;
            }
            
            //imageListView1.Focus();
        }
        #endregion
    }
}
