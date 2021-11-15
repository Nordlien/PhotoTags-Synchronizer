using GoogleLocationHistory;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using DataGridViewGeneric;
using FileDateTime;
using TimeZone;
using LocationNames;
using MetadataLibrary;
using CefSharp.WinForms;
using CefSharp;

namespace PhotoTagsSynchronizer
{
    public partial class FormLocationHistoryAnalytics : KryptonForm
    {
        #region Properties
        public GoogleLocationHistoryDatabaseCache GoogleLocationHistoryDatabaseCache { get; set; }
        public KryptonPalette KryptonPalette { get; set; }
        public KryptonDataGridView DataGridView { get { return kryptonDataGridViewLocationHistory; } }
        public DateTime DefaultDateTimeFrom
        {
            get { return kryptonDateTimePickerDateFrom.Value; }
            set
            {
                kryptonDateTimePickerDateFrom.Value = value;
            }
        }

        public DateTime DefaultDateTimeTo
        {
            get { return kryptonDateTimePickerDateTo.Value; }
            set
            {
                kryptonDateTimePickerDateTo.Value = value;
            }
        }
        private readonly ChromiumWebBrowser browser;

        bool isInitProcessing = true;
        #endregion

        #region FormLocationHistoryAnalytics
        public FormLocationHistoryAnalytics()
        {
            isInitProcessing = true;
            InitializeComponent();

            this.kryptonContextMenuItemLocationNamesCopy.Click += KryptonContextMenuItemLocationNamesCopy_Click;
            this.kryptonContextMenuItemLocationNamesFind.Click += KryptonContextMenuItemLocationNamesFind_Click;
            //this.kryptonContextMenuSeparatorLocationNames3,
            this.kryptonContextMenuItemLocationNamesAddFavorite.Click += KryptonContextMenuItemLocationNamesAddFavorite_Click;
            this.kryptonContextMenuItemLocationNamesRemoveFavorite.Click += KryptonContextMenuItemLocationNamesRemoveFavorite_Click;
            this.kryptonContextMenuItemLocationNamesToggleFavorite.Click += KryptonContextMenuItemLocationNamesToggleFavorite_Click;
            //this.kryptonContextMenuSeparatorLocationNames4,
            this.kryptonContextMenuItemLocationNamesShowFavoriteRows.Click += KryptonContextMenuItemLocationNamesShowFavoriteRows_Click;
            //this.kryptonContextMenuSeparatorLocationNames6,
            this.kryptonContextMenuItemLocationNamesShowCoordinateOpenStreetMap.Click += KryptonContextMenuItemLocationNamesShowCoordinateOpenStreetMap_Click;
            this.kryptonContextMenuItemLocationNamesShowCoordinateGoogleMap.Click += KryptonContextMenuItemLocationNamesShowCoordinateGoogleMap_Click;

            comboBoxMapZoomLevel.SelectedIndex = Properties.Settings.Default.LocationAnalyticsZoomLevel;
            kryptonDateTimePickerDateFrom.Value = DefaultDateTimeFrom;
            kryptonDateTimePickerDateTo.Value = DefaultDateTimeTo;
            kryptonNumericUpDownDistance.Value = Properties.Settings.Default.LocationAnalyticsMinimumDistance;
            kryptonNumericUpDownTimeInterval.Value = Properties.Settings.Default.LocationAnalyticsMinimumTimeInterval;

            browser = new ChromiumWebBrowser("https://www.openstreetmap.org/")
            {
                Dock = DockStyle.Fill,
            };
            browser.BrowserSettings.Javascript = CefState.Enabled;
            //browser.BrowserSettings.WebSecurity = CefState.Enabled;
            browser.BrowserSettings.WebGl = CefState.Enabled;
            browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Disabled;
            browser.BrowserSettings.Plugins = CefState.Enabled;
            this.kryptonPanelBrowser.Controls.Add(this.browser);

            browser.AddressChanged += Browser_AddressChanged;
            isInitProcessing = false;

        }
        #endregion

        #region KryptonContextMenuItemLocationNamesCopy_Click
        private void KryptonContextMenuItemLocationNamesCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = kryptonDataGridViewLocationHistory;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region KryptonContextMenuItemLocationNamesFind_Click
        private void KryptonContextMenuItemLocationNamesFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = kryptonDataGridViewLocationHistory;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region KryptonContextMenuItemLocationNamesAddFavorite_Click
        private void KryptonContextMenuItemLocationNamesAddFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = kryptonDataGridViewLocationHistory;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region KryptonContextMenuItemLocationNamesRemoveFavorite_Click
        private void KryptonContextMenuItemLocationNamesRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = kryptonDataGridViewLocationHistory;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region KryptonContextMenuItemLocationNamesToggleFavorite_Click
        private void KryptonContextMenuItemLocationNamesToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = kryptonDataGridViewLocationHistory;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region KryptonContextMenuItemLocationNamesShowFavoriteRows_Click
        private void KryptonContextMenuItemLocationNamesShowFavoriteRows_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = kryptonDataGridViewLocationHistory;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            //DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region KryptonContextMenuItemLocationNamesShowCoordinateOpenStreetMap_Click
        private void KryptonContextMenuItemLocationNamesShowCoordinateOpenStreetMap_Click(object sender, EventArgs e)
        {
            GetLocationAndShow(MapProvider.OpenStreetMap);
        }
        #endregion

        #region KryptonContextMenuItemLocationNamesShowCoordinateGoogleMap_Click
        private void KryptonContextMenuItemLocationNamesShowCoordinateGoogleMap_Click(object sender, EventArgs e)
        {
            GetLocationAndShow(MapProvider.GoogleMap);
        }
        #endregion

        #region Browser_AddressChanged
        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, AddressChangedEventArgs>(Browser_AddressChanged), sender, e);
                return;
            }

            kryptonTextBoxUrl.Text = e.Address;
        }
        #endregion

        #region Columns
        private int columnIndexTimestamp = 0;
        private int columnIndexUserAccount = 1;        
        private int columnIndexCoordinate = 2;
        private int columnIndexAltitude = 4;
        private int columnIndexAccuracy = 5;
        #endregion

        #region Location names - PopulateMetadataLocationNames
        private void PopulateMetadataLocationNames(DataGridView dataGridView, HashSet<LocationsHistory> locationsHistories, int minimumIntervalSeconds, float minimumIntervalCoordinate)
        {
            DateTime? lastDateTime = null;
            LocationCoordinate lastLocationCoordinate = null;

            foreach (LocationsHistory locationsHistory in locationsHistories)
            {
                bool isNewFund = false;
                if (lastDateTime == null || lastLocationCoordinate == null)
                {
                    isNewFund = true;
                } 
                else
                {
                    bool isMinimumTimeOk = false;
                    bool isMinimumDistanceOk = false;
                    if (Math.Abs(((DateTime)lastDateTime - locationsHistory.Timestamp).TotalSeconds) > minimumIntervalSeconds) isMinimumTimeOk = true;
                    if (Math.Abs(locationsHistory.LocationCoordinate.Latitude - lastLocationCoordinate.Latitude) > minimumIntervalCoordinate) isMinimumDistanceOk = true;
                    if (Math.Abs(locationsHistory.LocationCoordinate.Longitude - lastLocationCoordinate.Longitude) > minimumIntervalCoordinate) isMinimumDistanceOk = true;
                    if (isMinimumTimeOk && isMinimumDistanceOk) isNewFund = true;
                }

                if (isNewFund)
                {
                    lastDateTime = locationsHistory.Timestamp;
                    lastLocationCoordinate = locationsHistory.LocationCoordinate;

                    string group = locationsHistory.Timestamp.ToShortDateString();
                    string rowId = locationsHistory.Timestamp.ToShortTimeString();
                    LocationCoordinate value = locationsHistory.LocationCoordinate;

                    DataGridViewHandler.AddRow(dataGridView, 0,
                        new DataGridViewGenericRow(group),
                        null, false, false);

                    DataGridViewHandler.AddRow(dataGridView, columnIndexTimestamp,
                        new DataGridViewGenericRow(group, rowId, value),
                        TimeZone.TimeZoneLibrary.ToStringExiftoolUTC(locationsHistory.Timestamp), true, false);

                    DataGridViewHandler.AddRow(dataGridView, columnIndexUserAccount,
                        new DataGridViewGenericRow(group, rowId, value),
                        locationsHistory.UserAccount, true, false);

                    DataGridViewHandler.AddRow(dataGridView, columnIndexCoordinate,
                        new DataGridViewGenericRow(group, rowId, value),
                        locationsHistory.LocationCoordinate.ToString(), true, false);

                    DataGridViewHandler.AddRow(dataGridView, columnIndexAltitude,
                        new DataGridViewGenericRow(group, rowId, value),
                        locationsHistory.Altitude, true, false);

                    DataGridViewHandler.AddRow(dataGridView, columnIndexAccuracy,
                        new DataGridViewGenericRow(group, rowId, value),
                        locationsHistory.Accuracy, true, false);
                }
            }

            //isCellValueUpdating = false;
        }
        #endregion 

        #region Location names - PopulateMetadataLocationNames
        public void PopulateMetadataLocationsClear(DataGridView dataGridView)
        {
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridView, KryptonPalette, "LocationDates", "Location dates", DataGridViewSize.ConfigSize);
            DataGridViewHandler.Clear(dataGridView, DataGridViewSize.ConfigSize);

            DateTime dateTimeEditable = DateTime.Now;

            columnIndexTimestamp = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Timestamp", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

            columnIndexUserAccount = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("UserAccount", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

            columnIndexCoordinate = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Coordinate", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

            columnIndexAltitude = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Altitude", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

            columnIndexAccuracy = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Accuracy", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));


            
        }
        #endregion

        public void PopulateMetadataLocationsAdd(DataGridView dataGridView, DateTime dateTimeFrom, DateTime dateTimeTo, int minimumTimeInterval, float minimumDistance)
        {
            HashSet<LocationsHistory> locationsHistories = GoogleLocationHistoryDatabaseCache.LoadLocationHistory(dateTimeFrom, dateTimeTo);
            PopulateMetadataLocationNames(dataGridView, locationsHistories, minimumTimeInterval, minimumDistance);
        }

        #region kryptonButtonSearch_Click
        private void kryptonButtonSearch_Click(object sender, EventArgs e)
        {
            PopulateMetadataLocationsClear(kryptonDataGridViewLocationHistory);
            PopulateMetadataLocationsAdd(kryptonDataGridViewLocationHistory, kryptonDateTimePickerDateFrom.Value, kryptonDateTimePickerDateTo.Value, (int)(kryptonNumericUpDownTimeInterval.Value * 60), (float)kryptonNumericUpDownDistance.Value);
        }
        #endregion

        #region GetZoomLevel
        private int GetZoomLevel()
        {
            return comboBoxMapZoomLevel.SelectedIndex + 1;
        }
        #endregion

        #region GetLocationAndShow
        private void GetLocationAndShow(MapProvider mapProvider)
        {
            List<LocationCoordinate> locationCoordinates = new List<LocationCoordinate>();

            DataGridView dataGridView = kryptonDataGridViewLocationHistory;
            foreach (int rowIndex in DataGridViewHandler.GetRowSelected(dataGridView))
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader && dataGridViewGenericRow.LocationCoordinate != null)
                if (!locationCoordinates.Contains(dataGridViewGenericRow.LocationCoordinate)) locationCoordinates.Add(dataGridViewGenericRow.LocationCoordinate);
            }
            ShowMediaOnMap.UpdatedBroswerMap(browser, locationCoordinates, GetZoomLevel(), mapProvider);
        }
        #endregion

        #region kryptonDataGridViewLocationHistory_SelectionChanged
        private void kryptonDataGridViewLocationHistory_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);

            GetLocationAndShow(MapProvider.OpenStreetMap);
        }
        #endregion 

        #region kryptonDataGridViewLocationHistory_CellPainting
        private void kryptonDataGridViewLocationHistory_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
            //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region kryptonTextBoxUrl_KeyPress
        private void kryptonTextBoxUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; //Handle the Keypress event (suppress the Beep)
                browser.Load(kryptonTextBoxUrl.Text);
            }
        }
        #endregion

        #region comboBoxMapZoomLevel_SelectedIndexChanged
        private void comboBoxMapZoomLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitProcessing) return;
            Properties.Settings.Default.LocationAnalyticsZoomLevel = (byte)comboBoxMapZoomLevel.SelectedIndex;
            GetLocationAndShow(MapProvider.OpenStreetMap);
        }
        #endregion

        #region kryptonNumericUpDownTimeInterval_ValueChanged
        private void kryptonNumericUpDownTimeInterval_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LocationAnalyticsMinimumTimeInterval = (int)kryptonNumericUpDownTimeInterval.Value;
        }
        #endregion

        #region kryptonNumericUpDownDistance_ValueChanged
        private void kryptonNumericUpDownDistance_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LocationAnalyticsMinimumDistance = kryptonNumericUpDownDistance.Value;
        }
        #endregion 
    }
}
