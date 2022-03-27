using System.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        private FormLocationHistoryAnalytics formLocationHistoryAnalytics = new FormLocationHistoryAnalytics();

        #region ShowFormLocationHistoryAnalyticsInit
        private void ShowFormLocationHistoryAnalyticsInit()
        {
            if (formLocationHistoryAnalytics == null || formLocationHistoryAnalytics.IsDisposed) formLocationHistoryAnalytics = new FormLocationHistoryAnalytics();
            formLocationHistoryAnalytics.Owner = this;
            formLocationHistoryAnalytics.GoogleLocationHistoryDatabaseCache = databaseGoogleLocationHistory;
            formLocationHistoryAnalytics.KryptonPalette = (KryptonPalette)kryptonManager1.GlobalPalette;

            if (formLocationHistoryAnalytics.WindowState == FormWindowState.Minimized) formLocationHistoryAnalytics.WindowState = FormWindowState.Normal;
            formLocationHistoryAnalytics.BringToFront();
            formLocationHistoryAnalytics.Show();
        }
        #endregion

        #region SetDataGridViewForLocationAnalytics
        private void SetDataGridViewForLocationAnalytics()
        {
            if (formLocationHistoryAnalytics != null)
            {
                formLocationHistoryAnalytics.DataGridViewDateTime = dataGridViewDate;
                formLocationHistoryAnalytics.ActiveDataGridViewSelectedFilesMode = DataGridView_GetSelectedFilesModeFromActive();
                formLocationHistoryAnalytics.ActiveDataGridView = GetActiveTabDataGridView();
            }
        }
        #endregion

        #region ShowFormLocationHistoryAnalytics
        public void ShowFormLocationHistoryAnalytics()
        {
            ShowFormLocationHistoryAnalyticsInit();
            SetDataGridViewForLocationAnalytics();
            formLocationHistoryAnalytics.ShowFormLocationHistoryAnalytics();
        }
        #endregion
    }
}
