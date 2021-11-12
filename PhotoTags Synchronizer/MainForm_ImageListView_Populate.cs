
using System.Diagnostics;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {
        #region FilesSelected - Populate DataGridVIew, OpenWith...
        private void FilesSelectedOrNoneSelected()
        {
            if (GlobalData.IsPopulatingAnything()) return; //E.g. Populate FolderSelect
            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;

            using (new WaitCursor())
            {
                GlobalData.IsPopulatingImageListView = true;
                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                PopulateDataGridViewForSelectedItemsThread(imageListView1.SelectedItems);
                PopulateImageListViewOpenWithToolStripThread(imageListView1.SelectedItems, imageListView1.Items);
                
                GlobalData.IsPopulatingImageListView = false;
            }
        }
        #endregion
    }
}
