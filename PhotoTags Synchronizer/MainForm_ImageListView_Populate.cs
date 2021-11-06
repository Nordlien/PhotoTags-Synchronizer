
using System.Diagnostics;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
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
                
                GlobalData.IsPopulatingImageListView = false;
            }
        }
        #endregion
    }
}
