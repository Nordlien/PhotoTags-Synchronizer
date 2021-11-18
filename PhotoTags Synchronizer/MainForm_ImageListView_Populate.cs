
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Krypton.Toolkit;
using MetadataLibrary;

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

                HashSet<FileEntry> fileEntries = GetSelectedFileEntriesImageListView();
                PopulateDataGridViewForSelectedItemsThread(fileEntries);
                PopulateImageListViewOpenWithToolStripThread(fileEntries, imageListView1.Items);
                
                GlobalData.IsPopulatingImageListView = false;
            }
        }
        #endregion
    }
}
