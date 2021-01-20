using DataGridViewGeneric;
using FileDateTime;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WindowsProperty;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        #region FilesSelected - Populate DataGridVIew, OpenWith...
        private void FilesSelected()
        {
            if (GlobalData.IsPopulatingAnything()) return;
            if (GlobalData.DoNotRefreshDataGridViewWhileFileSelect) return;
            
            using (new WaitCursor())
            {
                GlobalData.IsPopulatingImageListView = true;
                GlobalData.SetDataNotAgreegatedOnGridViewForAnyTabs();

                PopulateDataGridViewForSelectedItemsThread(imageListView1.SelectedItems);
                PopulateImageListViewOpenWithToolStripThread(imageListView1.SelectedItems);

                DisplayAllQueueStatus();
                GlobalData.IsPopulatingImageListView = false;
            }
            
            imageListView1.Focus();
        }
        #endregion
    }
}
