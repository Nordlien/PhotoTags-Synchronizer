using GoogleLocationHistory;
using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DataGridViewGeneric;
using LocationNames;
using MetadataLibrary;
using CefSharp.WinForms;
using CefSharp;
using TimeZone;

namespace PhotoTagsSynchronizer
{
    public partial class FormLocationHistoryAnalytics : KryptonForm
    {
        #region Properties
        public GoogleLocationHistoryDatabaseCache GoogleLocationHistoryDatabaseCache { get; set; }
        public KryptonCustomPaletteBase KryptonCustomPaletteBase { get; set; }
        public KryptonDataGridView DataGridViewLocationHistory { get { return kryptonDataGridViewLocationHistory; } }
        public DataGridView DataGridViewDateTime { get; set; }
        public DataGridView ActiveDataGridView { get; set; }
        public DataGridViewHandler.GetSelectFileEntriesMode ActiveDataGridViewSelectedFilesMode { get; set; }

        private List<int> selectedRowsSorted = new List<int>();
        private int visibleRowIndex = 0;
        private bool selectionChanged = true;
        private bool isPopulatingSelectRows = false;

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
            SetButtonStatus(false);

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

            if (!GlobalData.isRunningWinSmode)
            {
                browser = new ChromiumWebBrowser("https://www.openstreetmap.org/")
                {
                    Dock = DockStyle.Fill,
                };
                browser.BrowserSettings.Javascript = CefState.Enabled;
                //browser.BrowserSettings.WebSecurity = CefState.Enabled;
                browser.BrowserSettings.WebGl = CefState.Enabled;
                //browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Disabled;
                //browser.BrowserSettings.Plugins = CefState.Enabled;
                this.kryptonPanelBrowser.Controls.Add(this.browser);

                browser.AddressChanged += Browser_AddressChanged;
            }

            isInitProcessing = false;

            DataGridViewHandler.DataGridViewInit(kryptonDataGridViewLocationHistory, false);
        }
        #endregion

        #region KryptonContextMenuItemLocationNamesCopy_Click
        private void KryptonContextMenuItemLocationNamesCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = kryptonDataGridViewLocationHistory;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
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

            DataGridViewHandler.SuspendLayoutSetDelay(dataGridView, true);
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
                        TimeZone.TimeZoneLibrary.ToStringSortableUTC(locationsHistory.Timestamp), true, false);

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
            DataGridViewHandler.ResumeLayoutDelayed(dataGridView);
            //isCellValueUpdating = false;
        }
        #endregion 

        #region Location names - PopulateMetadataLocations - Clear
        public void PopulateMetadataLocationsClear(DataGridView dataGridView)
        {
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridView, KryptonCustomPaletteBase, "LocationDates", "Location dates", DataGridViewSize.ConfigSize, allowUserToAddRow: false);
            DataGridViewHandler.Clear(dataGridView, DataGridViewSize.ConfigSize);
            DataGridViewHandler.SetIsAgregated(dataGridView, true);

            DateTime dateTimeEditable = DateTime.Now;

            columnIndexTimestamp = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Timestamp", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

            columnIndexUserAccount = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("UserAccount", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

            columnIndexCoordinate = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Coordinate", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

            columnIndexAltitude = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Altitude", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

            columnIndexAccuracy = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Accuracy", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.DefaultReadOnly, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);


            
        }
        #endregion

        #region Location names - PopulateMetadataLocations - Add
        public void PopulateMetadataLocationsAdd(DataGridView dataGridView, DateTime dateTimeFrom, DateTime dateTimeTo, int minimumTimeInterval, float minimumDistance)
        {
            HashSet<LocationsHistory> locationsHistories = GoogleLocationHistoryDatabaseCache.LoadLocationHistory(dateTimeFrom, dateTimeTo);
            PopulateMetadataLocationNames(dataGridView, locationsHistories, minimumTimeInterval, minimumDistance);
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
        }
        #endregion

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
            try
            {
                if (isPopulatingSelectRows) return;

                List<LocationCoordinate> locationCoordinates = new List<LocationCoordinate>();

                DataGridView dataGridView = kryptonDataGridViewLocationHistory;
                foreach (int rowIndex in DataGridViewHandler.GetRowSelected(dataGridView))
                {
                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                    if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader && dataGridViewGenericRow.LocationCoordinate != null)
                        if (!locationCoordinates.Contains(dataGridViewGenericRow.LocationCoordinate)) locationCoordinates.Add(dataGridViewGenericRow.LocationCoordinate);
                }
                try
                {
                    ShowMediaOnMap.UpdatedBroswerMap(browser, locationCoordinates, GetZoomLevel(), mapProvider);
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(
                        (GlobalData.isRunningWinSmode ? "Your Windows is running Windows 10 S / 11 S mode.\r\n" +
                        "The Chromium Web Browser doesn't support this mode.\r\n\r\n" : "") +
                        ex.Message, "Syntax Error", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                }
            }
            catch (Exception ex)
            {
                //Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                
            }
        }
        #endregion

        #region kryptonDataGridViewLocationHistory_SelectionChanged
        private void kryptonDataGridViewLocationHistory_SelectionChanged(object sender, EventArgs e)
        {
            selectionChanged = true;
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

        #region FindAndSelect
        private void FindAndSelect(DateTime? dateTimeTaken, DateTime? dateTimeLocationUtc, DateTime? dateTimeSuggestionFromGPSDateTime)
        {
            try
            {
                isPopulatingSelectRows = true;
                DataGridView dataGridView = DataGridViewLocationHistory;

                if (dateTimeSuggestionFromGPSDateTime != null) dateTimeTaken = dateTimeSuggestionFromGPSDateTime;
                int columnIndex = columnIndexTimestamp;
                bool dateTimeTakenFound = dateTimeTaken == null;
                bool dateTimeLocationUtcFound = dateTimeLocationUtc == null;

                for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                {
                    string dateTimeUtcOnRowString = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex);
                    DateTime? dateTimeOnRow = TimeZoneLibrary.ParseDateTimeAsUTC(dateTimeUtcOnRowString);
                    if (!dateTimeTakenFound && dateTimeOnRow != null && dateTimeTaken != null && dateTimeOnRow > dateTimeTaken)
                    {
                        if (rowIndex > 0) dataGridView.Rows[rowIndex - 1].Selected = true;
                        dataGridView.Rows[rowIndex].Selected = true;
                        dateTimeTakenFound = true;
                    }

                    if (!dateTimeLocationUtcFound && dateTimeOnRow != null && dateTimeLocationUtc != null && dateTimeOnRow > dateTimeLocationUtc)
                    {
                        if (rowIndex > 0) dataGridView.Rows[rowIndex - 1].Selected = true;
                        dataGridView.Rows[rowIndex].Selected = true;
                        dateTimeLocationUtcFound = true;                        
                    }

                    if (dateTimeTakenFound && dateTimeLocationUtcFound)
                    {     
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                isPopulatingSelectRows = false;
            }
        }
        #endregion

        #region MarkRows
        private void ActionMarkRows()
        {
            DataGridView dataGridView = DataGridViewLocationHistory;
            dataGridView.ClearSelection();

            HashSet<FileEntry> files = DataGridViewHandler.GetSelectFileEntries(ActiveDataGridView, ActiveDataGridViewSelectedFilesMode);
            for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(DataGridViewDateTime); columnIndex++)
            {
                DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(DataGridViewDateTime, columnIndex);
                if (dataGridViewGenericColumn != null)
                {
                    if (files.Contains(dataGridViewGenericColumn.FileEntryAttribute.FileEntry))
                    {
                        DateTime? dataTimeTaken = DataGridViewHandlerDate.GetUserInputDateTaken(DataGridViewDateTime, columnIndex, null);
                        DateTime? dataTimeLocationUtc = DataGridViewHandlerDate.GetUserInputLocationDate(DataGridViewDateTime, columnIndex, null);
                        DateTime? dateTimeSuggestionFromGPSDateTime = DataGridViewHandlerDate.GetSuggestionFromGPSDate(DataGridViewDateTime, columnIndex, null);
                        FindAndSelect(dataTimeTaken, dataTimeLocationUtc, dateTimeSuggestionFromGPSDateTime);
                    }
                }
            }


            if (dataGridView.SelectedRows.Count > 0)
            {
                selectedRowsSorted.Clear();
                foreach (DataGridViewRow dataGridViewRow in dataGridView.SelectedRows) selectedRowsSorted.Add(dataGridViewRow.Index);
                selectedRowsSorted.Sort();

                SetRowVisbible(visibleRowIndex);
                SetButtonStatus(enabledPreviousNext: selectedRowsSorted.Count > 0);
            }
            else
            {
                selectedRowsSorted.Clear();
                SetButtonStatus(enabledPreviousNext: selectedRowsSorted.Count > 0);
            }
            Properties.Settings.Default.LocationAnalyticsZoomLevel = (byte)comboBoxMapZoomLevel.SelectedIndex;
            GetLocationAndShow(MapProvider.OpenStreetMap);
            selectionChanged = false;
        }
        #endregion

        #region kryptonButtonMarkRows_Click
        private void kryptonButtonMarkRows_Click(object sender, EventArgs e)
        {
            visibleRowIndex = 0;
            ActionMarkRows();
        }
        #endregion

        #region SetRowVisbible
        private void SetRowVisbible(int rowIndex, bool markTheRow = false)
        {
            DataGridView dataGridView = DataGridViewLocationHistory;
            dataGridView.FirstDisplayedScrollingRowIndex = selectedRowsSorted[rowIndex];
            //dataGridView.CurrentCell = dataGridView[0, dataGridView.SelectedRows[0].Index];
            if (markTheRow) dataGridView.Rows[selectedRowsSorted[rowIndex]].Selected = true;
            kryptonLabelRowsSelected.Text = "Selected: " + (rowIndex + 1) + " / " + selectedRowsSorted.Count;
        }
        #endregion 

        #region SetButtonStatus
        public void SetButtonStatus(bool enabledPreviousNext)
        {
            kryptonButtonBrowseRowPrevious.Enabled = enabledPreviousNext;
            kryptonButtonBrowseRowNext.Enabled = enabledPreviousNext;
        }
        #endregion

        #region kryptonButtonBrowseRowPrevious_Click
        private void kryptonButtonBrowseRowPrevious_Click(object sender, EventArgs e)
        {
            if (selectionChanged) ActionMarkRows();
            if (selectedRowsSorted.Count == 0) SetButtonStatus(enabledPreviousNext: selectedRowsSorted.Count > 0);
            else
            {
                visibleRowIndex--;
                if (visibleRowIndex < 0) visibleRowIndex = selectedRowsSorted.Count - 1;
                SetRowVisbible(visibleRowIndex, markTheRow: true);
            }
            
            
        }
        #endregion

        #region kryptonButtonBrowseRowNext_Click
        private void kryptonButtonBrowseRowNext_Click(object sender, EventArgs e)
        {
            if (selectionChanged) ActionMarkRows();
            if (selectedRowsSorted.Count == 0) SetButtonStatus(enabledPreviousNext: selectedRowsSorted.Count > 0);
            else
            {
                visibleRowIndex++;
                if (visibleRowIndex > selectedRowsSorted.Count - 1) visibleRowIndex = 0;
                SetRowVisbible(visibleRowIndex, markTheRow: true);
            }
        }
        #endregion 

        #region AddDatesFound
        private void AddDatesFound(DateTime dateTime, ref List<DateTime> dates)
        {
            DateTime dateTimeFound = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
            if (!dates.Contains(dateTimeFound.AddDays(-1))) dates.Add(dateTimeFound.AddDays(-1));
            if (!dates.Contains(dateTimeFound)) dates.Add(dateTimeFound);
            if (!dates.Contains(dateTimeFound.AddDays(1))) dates.Add(dateTimeFound.AddDays(1));
        }
        #endregion

        #region ShowFormLocationHistoryAnalytics
        private void ShowFormLocationHistoryAnalytics(DataGridView dataGridViewLocationHistory, DataGridView dataGridViewDate, DataGridView dataGridViewActive)
        {
            using (new WaitCursor())
            {
                SetButtonStatus(false);

                List<DateTime> datesFound = new List<DateTime>();
                DateTime? dateTimeFrom = null;
                DateTime? dateTimeTo = null;

                if (DataGridViewHandler.GetIsAgregated(dataGridViewActive))
                {
                    //ShowFormLocationHistoryAnalyticsInit();
                    PopulateMetadataLocationsClear(dataGridViewLocationHistory);

                    foreach (int columnIndex in DataGridViewHandler.GetColumnSelected(dataGridViewActive))
                    {
                        DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridViewActive, columnIndex);

                        if (dataGridViewGenericColumn != null)
                        {
                            DateTime? date = DataGridViewHandlerDate.GetUserInputDateTaken(dataGridViewDate, null, dataGridViewGenericColumn.FileEntryAttribute);
                            if (date != null)
                            {
                                AddDatesFound((DateTime)date, ref datesFound);
                                if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                            }

                            date = DataGridViewHandlerDate.GetUserInputLocationDate(dataGridViewDate, null, dataGridViewGenericColumn.FileEntryAttribute);
                            if (date != null)
                            {
                                AddDatesFound((DateTime)date, ref datesFound);
                                if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                            }

                            if (dataGridViewGenericColumn != null && dataGridViewGenericColumn.Metadata != null)
                            {
                                date = dataGridViewGenericColumn.Metadata.MediaDateTaken;
                                if (date != null)
                                {
                                    AddDatesFound((DateTime)date, ref datesFound);
                                    if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                    if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                                }

                                date = dataGridViewGenericColumn.Metadata.LocationDateTime;
                                if (date != null)
                                {
                                    AddDatesFound((DateTime)date, ref datesFound);
                                    if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                    if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                                }

                                //date = dataGridViewGenericColumn.Metadata.FileDateCreated;
                                //if (date != null)
                                //{
                                //    if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                //    if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                                //}

                                //date = dataGridViewGenericColumn.Metadata.FileDateModified;
                                //if (date != null)
                                //{
                                //    if (dateTimeFrom == null || date < dateTimeFrom) dateTimeFrom = date;
                                //    if (dateTimeTo == null || date > dateTimeTo) dateTimeTo = date;
                                //}

                                if (dataGridViewGenericColumn.Metadata.FileDateCreated != null && dataGridViewGenericColumn.Metadata.FileDateModified != null)
                                {
                                    date = (dataGridViewGenericColumn.Metadata.FileDateCreated < dataGridViewGenericColumn.Metadata.FileDateModified ? dataGridViewGenericColumn.Metadata.FileDateCreated : dataGridViewGenericColumn.Metadata.FileDateModified);
                                    AddDatesFound((DateTime)date, ref datesFound);
                                }
                            }

                        }
                    }

                    DateTime? dateTimeFoundFrom = null;
                    DateTime? dateTimeFoundTo = null;
                    for (int index = 0; index < datesFound.Count - 1; index++)
                    {
                        if (index == 0) dateTimeFoundFrom = datesFound[index];
                        if (datesFound[index].AddDays(1) != datesFound[index + 1]) dateTimeFoundTo = datesFound[index];
                        if (index == datesFound.Count - 2) dateTimeFoundTo = datesFound[index + 1];
                        if (dateTimeFoundFrom != null && dateTimeFoundTo == null) dateTimeFoundTo = ((DateTime)dateTimeFoundFrom).AddDays(1);
                        if (dateTimeFoundFrom == null && dateTimeFoundTo != null) dateTimeFoundFrom = ((DateTime)dateTimeFoundTo).AddDays(-1);

                        if (dateTimeFoundFrom != null && dateTimeFoundTo != null)
                        {
                            PopulateMetadataLocationsAdd(dataGridViewLocationHistory, (DateTime)dateTimeFoundFrom, ((DateTime)dateTimeFoundTo).AddDays(1).AddMilliseconds(-1), Properties.Settings.Default.LocationAnalyticsMinimumTimeInterval * 60, (float)Properties.Settings.Default.LocationAnalyticsMinimumDistance);
                            dateTimeFoundFrom = datesFound[index + 1];
                            dateTimeFoundTo = null;
                        }
                    }

                }

                if (dateTimeFrom != null) DefaultDateTimeFrom = new DateTime(((DateTime)dateTimeFrom).Year, ((DateTime)dateTimeFrom).Month, ((DateTime)dateTimeFrom).Day, 0, 0, 0, DateTimeKind.Utc);
                if (dateTimeTo != null) DefaultDateTimeTo = new DateTime(((DateTime)dateTimeTo).Year, ((DateTime)dateTimeTo).Month, ((DateTime)dateTimeTo).Day, 0, 0, 0, DateTimeKind.Utc).AddDays(1);
            }
        }
        #endregion

        #region ShowFormLocationHistoryAnalytics
        public void ShowFormLocationHistoryAnalytics()
        {
            ShowFormLocationHistoryAnalytics(kryptonDataGridViewLocationHistory, DataGridViewDateTime, ActiveDataGridView);
        }
        #endregion

        #region kryptonButtonSearchFitCells_Click
        private void kryptonButtonSearchFitCells_Click(object sender, EventArgs e)
        {
            ShowFormLocationHistoryAnalytics();
        }
        #endregion
    }
}
