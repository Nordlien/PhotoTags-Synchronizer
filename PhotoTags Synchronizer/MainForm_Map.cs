using System;
using System.Windows.Forms;
using CefSharp;
using System.Globalization;
using CameraOwners;
using SqliteDatabase;
using LocationNames;
using DataGridViewGeneric;
using System.Collections.Generic;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {        
        #region LocationCoordinate ParseCoordinateFromURL(string text)
        private LocationCoordinate ParseCoordinateFromURL(string text)
        {
            /*
            Normal:                                                     #map=15/59.9415/10.6785&layers=N
            Note:       note/new?   lat=59.9419&lon=10.6814             #map=15/59.9415/10.6861&layers=N
            Center:     query?      lat=59.9420&lon=10.6847             #map=15/59.9421/10.6805&layers=N
            Address:    search?whereami=1&  query=59.9417%2C10.6740     #map=15/59.9417/10.6740&layers=N
            Search:     search?             query=59.9417%2C10.6740     #map=15/59.9417/10.6740&layers=N
            */
            LocationCoordinate locationCoordinate = null;

            string url = text;
            string string_lon;
            string string_lat;
            int index_sperator;
            int index_lat = url.IndexOf("lat=");
            if (index_lat >= 0)
            {
                index_sperator = url.IndexOf("&", index_lat);
                if (index_sperator <= index_lat) index_sperator = url.IndexOf("%2C", index_lat);
                if (index_sperator <= index_lat) return locationCoordinate; //Not found, can't find values

                string_lat = url.Substring(index_lat + 4, index_sperator - index_lat - 4);

                int index_lon = url.IndexOf("lon=");
                if (index_lon <= index_lat) return locationCoordinate;
                index_sperator = url.IndexOf("#map", index_lon);
                if (index_sperator == -1) index_sperator = url.Length; //Not found, can't find values
                string_lon = url.Substring(index_lon + 4, index_sperator - index_lon - 4);
            }
            else
            {
                const int coordLength = 11;
                index_lat = url.IndexOf("query=") + 6;
                if (index_lat >= 0 && index_lat + coordLength <= url.Length)
                {
                    index_sperator = url.IndexOf("%2C", index_lat, coordLength);
                    if (index_sperator == -1) index_sperator = url.IndexOf("&", index_lat, 9);
                    if (index_sperator == -1) return locationCoordinate; //Not found, can't find seperator
                    string_lat = url.Substring(index_lat, index_sperator - index_lat);

                    int index_lon = -1;
                    if (url.IndexOf("%2C", index_lat, 11) != -1) index_lon = index_sperator + 3;
                    if (url.IndexOf("&", index_lat, 9) != -1) index_lon = index_sperator + 1;
                    if (index_lon == -1) return locationCoordinate;

                    index_sperator = url.IndexOf("#map", index_lon);
                    if (index_sperator == -1) index_sperator = url.Length;

                    string_lon = url.Substring(index_lon, index_sperator - index_lon);
                }
                else return locationCoordinate;
            }

            float lat;
            float lon;
            if (float.TryParse(string_lon, NumberStyles.Float, CultureInfo.InvariantCulture, out lon) &&
                float.TryParse(string_lat, NumberStyles.Float, CultureInfo.InvariantCulture, out lat))
            {
                locationCoordinate = new LocationCoordinate(lat, lon);
            }
            return locationCoordinate;
        }
        #endregion 

        #region Browser event handling
        delegate void SetTextBoxURLCallback(string text);

        private void textBoxBrowserURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; //Handle the Keypress event (suppress the Beep)
                browser.Load(textBoxBrowserURL.Text);
            }
        }

        private void PopulateDataGridViewMapWithBrowserCoordinate(DataGridView dataGridView, string text)
        {
            if (dataGridView.CurrentCell == null && dataGridView.SelectedCells.Count < 1) return;

            LocationCoordinate locationCoordinate = ParseCoordinateFromURL(text);


            if (locationCoordinate != null)
            {
                List<int> selectedColumns = DataGridViewHandler.GetColumnSelected(dataGridView);

                foreach (int columnIndex in selectedColumns)
                {
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex,
                        DataGridViewHandlerMap.headerBrowser, DataGridViewHandlerMap.tagCoordinates, locationCoordinate.ToString());

                }

            }
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs e)
        {
            SetTextBoxBrowserURLText(e.Address);
        }

        private void SetTextBoxBrowserURLText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            try
            {
                DataGridView dataGridView = dataGridViewMap;
                if (this.textBoxBrowserURL.InvokeRequired)
                {
                    SetTextBoxURLCallback d = new SetTextBoxURLCallback(SetTextBoxBrowserURLText);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.textBoxBrowserURL.Text = text;
                    PopulateDataGridViewMapWithBrowserCoordinate(dataGridView, text);
                }
            }
            catch (Exception ex)
            {
                Logger.Warn("PopulateGrivViewMapMediaFile." + ex.Message);
                //throw; Windows closing, this will cause error. But ok...
            }
        }
        
        private int GetZoomLevel()
        {
            return comboBoxMapZoomLevel.SelectedIndex + 1;
        }

        LocationCoordinate locationCoordinateRememberForZooming = null;
        #region UpdateBrowserMap - Only when one cell selected
        private void UpdateBrowserMap(string combinedCorordinateString)
        {
            if (ClipboardUtility.IsClipboardActive && ClipboardUtility.NuberOfItemsToEdit>1) return;
            DataGridView dataGridView = dataGridViewMap;
            if (DataGridViewHandler.GetCellSelectedCount(dataGridView) == 1) //Only updated the Browser Map when one cell are updated
            {
                LocationCoordinate locationCoordinate = LocationCoordinate.Parse(combinedCorordinateString);

                if (locationCoordinate != null)
                {
                    locationCoordinateRememberForZooming = new LocationCoordinate(locationCoordinate.Latitude, locationCoordinate.Longitude);

                    ShowMediaOnMap.UpdateBrowserMap(browser, locationCoordinate, GetZoomLevel());
                }
            }
        }
        #endregion

        #region ShowCoordinateOnMap_Click
        private void toolStripMenuItemShowCoordinateOnMap_Click(object sender, EventArgs e)
        {
            List<LocationCoordinate> locationCoordinates = new List<LocationCoordinate>();

            DataGridView dataGridView = dataGridViewMap;
            foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
            {

                if (LocationCoordinate.TryParse(DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex), out LocationCoordinate locationCoordinate))
                {
                    locationCoordinates.Add(locationCoordinate);
                }
            }
            ShowMediaOnMap.UpdatedBroswerMap(browser, locationCoordinates, GetZoomLevel());
        }
        #endregion 

        

        
        #endregion

        #region UpdateGoodleHistoryCoordinate after Select new TimeZoneShift or AccepedIntervalSecound
        private int GetAccepedIntervalSecound()
        {
            int accepedIntervalSecound;
            switch (comboBoxGoogleLocationInterval.SelectedIndex)
            {
                case 0:
                    accepedIntervalSecound = 60; // 1 minute
                    break;
                case 1:
                    accepedIntervalSecound = 60 * 5; //5 minutes
                    break;
                case 2:
                    accepedIntervalSecound = 60 * 30; //30 minutes
                    break;
                case 3:
                    accepedIntervalSecound = 60 * 60; //1 hour
                    break;
                case 4:
                    accepedIntervalSecound = 60 * 60 * 24; //1 day
                    break;
                case 5:
                    accepedIntervalSecound = 60 * 60 * 24 * 2; //2 days
                    break;
                case 6:
                    accepedIntervalSecound = 60 * 60 * 24 * 10; //2 days
                    break;
                default:
                    accepedIntervalSecound = 3600;
                    break;
            }
            return accepedIntervalSecound;
        }

        private int GetTimeZoneShift()
        {
            return comboBoxGoogleTimeZoneShift.SelectedIndex - 12;
        }

        private void UpdateGoodleHistoryCoordinate()
        {
            DataGridView dataGridView = dataGridViewMap;

            DataGridViewHandlerMap.TimeZoneShift = GetTimeZoneShift();
            DataGridViewHandlerMap.AccepedIntervalSecound = GetAccepedIntervalSecound();

            for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
            {
                DataGridViewHandlerMap.PopulateGoogleHistoryCoordinate(dataGridView, columnIndex,
                    DataGridViewHandlerMap.TimeZoneShift,
                    DataGridViewHandlerMap.AccepedIntervalSecound);
            }
            
        }
        #endregion 

        #region User control handling
        private void comboBoxGoogleTimeZoneShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (isSettingDefaultComboxValues) return;
            if (GlobalData.IsPopulatingMap) return;

            Properties.Settings.Default.ComboBoxGoogleTimeZoneShift = comboBoxGoogleTimeZoneShift.SelectedIndex;
            Properties.Settings.Default.Save();
            UpdateGoodleHistoryCoordinate();

            if (dataGridViewMap.CurrentCell != null && dataGridViewMap.CurrentCell.Value != null) UpdateBrowserMap(dataGridViewMap.CurrentCell.Value.ToString());

        }

        private void comboBoxGoogleLocationInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (isSettingDefaultComboxValues) return;
            if (GlobalData.IsPopulatingMap) return;

            Properties.Settings.Default.ComboBoxGoogleLocationInterval = comboBoxGoogleLocationInterval.SelectedIndex;    //30 minutes Index 2
            Properties.Settings.Default.Save();
            UpdateGoodleHistoryCoordinate();

            if (dataGridViewMap.CurrentCell.Value != null) UpdateBrowserMap(dataGridViewMap.CurrentCell.Value.ToString());
        }

        private void comboBoxMapZoomLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (isSettingDefaultComboxValues) return;
            if (GlobalData.IsPopulatingMap) return;

            Properties.Settings.Default.ComboBoxMapZoomLevel = comboBoxMapZoomLevel.SelectedIndex;
            Properties.Settings.Default.Save();

            if (locationCoordinateRememberForZooming != null) ShowMediaOnMap.UpdateBrowserMap(browser, locationCoordinateRememberForZooming, GetZoomLevel()); //Use last valid coordinates clicked

        }

        #endregion

        #region DataGridMap Enter Cell with GPS location, update map


        #region DataGridView - Map - Cell Mouse *Double* Click
        private void dataGridViewMap_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            
            if (!dataGridView.Enabled) return;
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dataGridView.SelectedCells.Count > 1) return;

            if (dataGridViewMap[e.ColumnIndex, e.RowIndex].Value != null)
            {
                UpdateBrowserMap(DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, e.ColumnIndex, e.RowIndex));
            }
        }
        #endregion

        #region DataGridView - Map - CellValueChanged
        private bool isDataGridViewMaps_CellValueChanging = false;
        private void dataGridViewMap_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isDataGridViewMaps_CellValueChanging) return; //Avoid requirng isues
            if (GlobalData.IsApplicationClosing) return;
            if (GlobalData.IsPopulatingAnything()) return;

            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            DataGridViewGenericColumn gridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, e.ColumnIndex);
            if (gridViewGenericColumn.Metadata == null) return;
            DataGridViewGenericRow gridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);

            isDataGridViewMaps_CellValueChanging = true;
            ///////////////////////////////////////////////////////////////////////////
            /// Coordinate changes, updated Nomnatatim address 
            ///////////////////////////////////////////////////////////////////////////
            if (gridViewGenericRow.HeaderName.Equals(DataGridViewHandlerMap.headerMedia) &&
                gridViewGenericRow.RowName.Equals(DataGridViewHandlerMap.tagCoordinates))
            {
                string coordinate = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridViewMap, e.ColumnIndex, e.RowIndex);
                UpdateBrowserMap(coordinate);
                DataGridViewHandlerMap.PopulateGrivViewMapNomnatatim(dataGridView, e.ColumnIndex, LocationCoordinate.Parse(coordinate));                
            }

            ///////////////////////////////////////////////////////////////////////////
            /// Camera make and model owner changed, upated all fields
            ///////////////////////////////////////////////////////////////////////////

            else if (gridViewGenericRow.HeaderName.Equals(DataGridViewHandlerMap.headerGoogleLocations) &&
                gridViewGenericRow.RowName.Equals(DataGridViewHandlerMap.tagCameraOwner))
            {
                string selectedCameraOwner = DataGridViewHandlerMap.GetCameraOwner(dataGridView, e.ColumnIndex);
                DataGridViewHandlerMap.SetCameraOwner(dataGridView, e.ColumnIndex, selectedCameraOwner);
                if (!string.IsNullOrWhiteSpace(selectedCameraOwner))
                {
                    if (gridViewGenericColumn.Metadata != null)
                    {
                        CameraOwner cameraOwner = new CameraOwner(
                            gridViewGenericColumn.Metadata.CameraMake,
                            gridViewGenericColumn.Metadata.CameraModel,
                            selectedCameraOwner);

                        CommonDatabaseTransaction commonDatabaseTransaction = databaseUtilitiesSqliteMetadata.TransactionBegin(CommonDatabaseTransaction.TransactionReadCommitted);
                        databaseAndCahceCameraOwner.SaveCameraMakeModelAndOwner(commonDatabaseTransaction, cameraOwner);
                        databaseAndCahceCameraOwner.CameraMakeModelAndOwnerMakeDirty();
                        databaseUtilitiesSqliteMetadata.TransactionCommit(commonDatabaseTransaction);

                        for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridViewMap); columnIndex++)
                        {
                            DataGridViewGenericColumn gridViewGenericColumnCheck = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);

                            if (gridViewGenericColumnCheck?.Metadata?.CameraMake == gridViewGenericColumn.Metadata.CameraMake &&
                                gridViewGenericColumnCheck?.Metadata?.CameraModel == gridViewGenericColumn.Metadata.CameraModel)
                            {
                                //DataGridViewHandlerMap.SetCameraOwner(dataGridView, columnIndex, selectedCameraOwner);

                                DataGridViewHandlerMap.PopulateCameraOwner(dataGridView, columnIndex, gridViewGenericColumnCheck.ReadWriteAccess,
                                    gridViewGenericColumnCheck.Metadata.CameraMake, gridViewGenericColumnCheck.Metadata.CameraModel);
                                DataGridViewHandlerMap.PopulateGoogleHistoryCoordinate(dataGridView, columnIndex, GetTimeZoneShift(), GetAccepedIntervalSecound());
                            }
                        }
                    }
                }
            }

            ///////////////////////////////////////////////////////////////////////////
            /// Nomnatatim
            ///////////////////////////////////////////////////////////////////////////
            float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
            float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
            if (gridViewGenericRow.HeaderName.Equals(DataGridViewHandlerMap.headerNominatim))
            {
                string coordinate = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridViewMap, e.ColumnIndex, DataGridViewHandlerMap.headerMedia, DataGridViewHandlerMap.tagCoordinates);
                LocationCoordinate locationCoordinateNomnatatim = LocationCoordinate.Parse(coordinate);

                if (locationCoordinateNomnatatim != null)
                {
                    LocationNameLookUpCache locationAddress = new LocationNameLookUpCache(databaseUtilitiesSqliteMetadata, Properties.Settings.Default.ApplicationPreferredLanguages);

                    CommonDatabaseTransaction commonDatabaseTransaction = databaseUtilitiesSqliteMetadata.TransactionBegin(CommonDatabaseTransaction.TransactionReadCommitted);
                    locationAddress.AddressLookupNearestAndUpdate(
                        new LocationCoordinateAndDescription(
                            new LocationCoordinate( 
                                (float)gridViewGenericColumn.Metadata.LocationLatitude,
                                (float)gridViewGenericColumn.Metadata.LocationLongitude),
                            new LocationDescription(
                                (string)DataGridViewHandler.GetCellValue(dataGridView, e.ColumnIndex, DataGridViewHandlerMap.headerNominatim, DataGridViewHandlerMap.tagLocationName), //Name
                                (string)DataGridViewHandler.GetCellValue(dataGridView, e.ColumnIndex, DataGridViewHandlerMap.headerNominatim, DataGridViewHandlerMap.tagCity), //City
                                (string)DataGridViewHandler.GetCellValue(dataGridView, e.ColumnIndex, DataGridViewHandlerMap.headerNominatim, DataGridViewHandlerMap.tagProvince), //State
                                (string)DataGridViewHandler.GetCellValue(dataGridView, e.ColumnIndex, DataGridViewHandlerMap.headerNominatim, DataGridViewHandlerMap.tagCountry)) //Country
                            ),
                            locationAccuracyLatitude, locationAccuracyLongitude
                        );
                    databaseUtilitiesSqliteMetadata.TransactionCommit(commonDatabaseTransaction);

                    for (int columnIndex = 0; columnIndex < dataGridViewMap.ColumnCount; columnIndex++)
                    {
                        string locationCoordinateString = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridViewMap, columnIndex, DataGridViewHandlerMap.headerMedia, DataGridViewHandlerMap.tagCoordinates);
                        LocationCoordinate locationCoordinate = LocationCoordinate.Parse(locationCoordinateString);
                        DataGridViewHandlerMap.PopulateGrivViewMapNomnatatim(dataGridView, columnIndex, locationCoordinate);
                    }
                }
            }

            isDataGridViewMaps_CellValueChanging = false;
        }
        #endregion 

        #endregion

        private void toolStripMenuItemMapReloadLocationUsingNominatim_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            List<int> selectedColumns = DataGridViewHandler.GetColumnSelected(dataGridView);
            if (selectedColumns.Count == 0) return;

            isDataGridViewMaps_CellValueChanging = true;
            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, DataGridViewHandlerMap.headerMedia, DataGridViewHandlerMap.tagCoordinates);

            //Delete from database cache
            foreach (int columnIndex in selectedColumns)
            {
                string coordinate = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridViewMap, columnIndex, rowIndex);
                DataGridViewHandlerMap.DeleteMapNomnatatim(LocationCoordinate.Parse(coordinate));
            }

            //Reload data from Nomnatatim or if from database in case equal
            foreach (int columnIndex in selectedColumns)
            {
                string coordinate = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridViewMap, columnIndex, rowIndex);
                DataGridViewHandlerMap.PopulateGrivViewMapNomnatatim(dataGridView, columnIndex, LocationCoordinate.Parse(coordinate));
            }
            isDataGridViewMaps_CellValueChanging = false;
        }
    }
}
