using System;
using System.Windows.Forms;
using MetadataLibrary;
using CefSharp;
using System.Globalization;
using CameraOwners;
using SqliteDatabase;
using LocationNames;
using DataGridViewGeneric;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

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
        

        double _latitudeRememberForZooming = 0;
        double _longitudeRememberForZooming = 0;
        private void UpdateBrowserMap(string comdinedCorordinates)
        {
            if (ClipboardUtility.IsClipboardActive && ClipboardUtility.NuberOfItemsToEdit>1) return;
            DataGridView dataGridView = dataGridViewMap;
            if (DataGridViewHandler.GetSelectedCellCount(dataGridView) == 1) //Only updated the Browser Map when one cell are updated
            {
                LocationCoordinate locationCoordinate = LocationCoordinate.Parse(comdinedCorordinates);

                if (locationCoordinate != null)
                {
                    _latitudeRememberForZooming = locationCoordinate.Latitude;
                    _longitudeRememberForZooming = locationCoordinate.Longitude;

                    UpdateBrowserMap(locationCoordinate.Latitude, locationCoordinate.Longitude);
                }
            }
        }

        private void UpdateBrowserMap(double latitide, double longitude)
        {
            //https://www.google.com/maps/search/59.902056,+10.743139/@59.902056,10.7409503,17z
            //https://www.google.com/maps/search/59.902056,10.743139
            //https://www.latlong.net/c/?lat=59.902827&long=10.754396
            //https://www.openstreetmap.org/?mlat=51.510772705078125&mlon=0.054931640625#map=13/51.5147/0.0494

            browser.Load("https://www.openstreetmap.org/?mlat=" +
                latitide.ToString(CultureInfo.InvariantCulture) +
                "&mlon=" + longitude.ToString(CultureInfo.InvariantCulture) +
                "#map=" + (comboBoxMapZoomLevel.SelectedIndex + 1).ToString() +
                "/" + latitide.ToString(CultureInfo.InvariantCulture) +
                "/" + longitude.ToString(CultureInfo.InvariantCulture));
        }
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

            UpdateBrowserMap(_latitudeRememberForZooming, _longitudeRememberForZooming); //Use last valid coordinates clicked

        }

        #endregion

        #region Cell begin edit and end edit
        #endregion 

        #region DataGridMap Enter Cell with GPS location, update map
        public void GPSCoordinatedClicked(DataGridView dataGridView, int columnIndex, int rowIndex)
        {            
            if (!dataGridView.Enabled) return;
            if (columnIndex < 0 || rowIndex < 0) return;
            if (dataGridView.SelectedCells.Count > 1) return;

            if (dataGridViewMap[columnIndex, rowIndex].Value != null)
            {
                UpdateBrowserMap(DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, rowIndex));
            }
        }

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

                            if (gridViewGenericColumnCheck.Metadata.CameraMake == gridViewGenericColumn.Metadata.CameraMake &&
                                gridViewGenericColumnCheck.Metadata.CameraModel == gridViewGenericColumn.Metadata.CameraModel)
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
            if (gridViewGenericRow.HeaderName.Equals(DataGridViewHandlerMap.headerNominatim))
            {
                if (gridViewGenericColumn.Metadata.LocationLatitude == null || gridViewGenericColumn.Metadata.LocationLongitude == null) 
                {
                    isDataGridViewMaps_CellValueChanging = false;
                    return; 
                }

                LocationNameLookUpCache locationAddress = new LocationNameLookUpCache(databaseUtilitiesSqliteMetadata, Properties.Settings.Default.ApplicationPreferredLanguages);

                CommonDatabaseTransaction commonDatabaseTransaction = databaseUtilitiesSqliteMetadata.TransactionBegin(CommonDatabaseTransaction.TransactionReadCommitted);
                locationAddress.AddressUpdate(
                    (float)gridViewGenericColumn.Metadata.LocationLatitude,
                    (float)gridViewGenericColumn.Metadata.LocationLongitude,
                    (string)DataGridViewHandler.GetCellValue(dataGridView, e.ColumnIndex, DataGridViewHandlerMap.headerNominatim, DataGridViewHandlerMap.tagLocationName), //Name
                    (string)DataGridViewHandler.GetCellValue(dataGridView, e.ColumnIndex, DataGridViewHandlerMap.headerNominatim, DataGridViewHandlerMap.tagCity), //City
                    (string)DataGridViewHandler.GetCellValue(dataGridView, e.ColumnIndex, DataGridViewHandlerMap.headerNominatim, DataGridViewHandlerMap.tagProvince), //State
                    (string)DataGridViewHandler.GetCellValue(dataGridView, e.ColumnIndex, DataGridViewHandlerMap.headerNominatim, DataGridViewHandlerMap.tagCountry)); //Country
                databaseUtilitiesSqliteMetadata.TransactionCommit(commonDatabaseTransaction);

                for (int columnIndex = 0; columnIndex < dataGridViewMap.ColumnCount; columnIndex++)
                {
                    string locationCoordinateString = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, e.RowIndex);
                    LocationCoordinate locationCoordinate = LocationCoordinate.Parse(locationCoordinateString);
                    DataGridViewHandlerMap.PopulateGrivViewMapNomnatatim(dataGridView, columnIndex, locationCoordinate);
                }

            }

            isDataGridViewMaps_CellValueChanging = false;
        }

        private void dataGridViewMap_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            GPSCoordinatedClicked(dataGridView, e.ColumnIndex, e.RowIndex);
        }

        private void toolStripMenuItemShowCoordinateOnMap_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMap;
            if (dataGridView.SelectedCells.Count == 1)
                GPSCoordinatedClicked(dataGridView, dataGridView.CurrentCell.ColumnIndex, dataGridView.CurrentCell.RowIndex);
            else
            {
                string coordinates = "";
                /*
                coordinates =
                        "      [9.2, 48.8, 'Cities1']," +
                        "      [8.4, 49.0, 'Cities1']," +
                        "      [6.2, 48.7, 'Cities2']," +
                        "      [2.5, 48.9, 'Cities2']," +
                        "      [-1.4, 50.9, 'Cities3']," +
                        "      [6.6, 51.8, 'Cities3']," +
                        "      [8.6, 49.4, 'Cities4']," +
                        "      [11.6, 48.1, 'Cities4']";
                */
                string locationMap = "";
                foreach (DataGridViewCell dataGridViewCell in dataGridView.SelectedCells)
                {
                    
                    if (LocationCoordinate.TryParse(DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex), out LocationCoordinate locationCoordinate))
                    {
                        locationMap = (locationCoordinate.Longitude).ToString(CultureInfo.InvariantCulture) + ", " +
                            (locationCoordinate.Latitude).ToString(CultureInfo.InvariantCulture);

                        if (coordinates != "") coordinates += ", ";
                        coordinates += 
                            "[" +
                            (locationCoordinate.Longitude).ToString(CultureInfo.InvariantCulture) + ", " +
                            (locationCoordinate.Latitude).ToString(CultureInfo.InvariantCulture) +
                            ", 'Face']";
                    }                    
                    
                }
                string htmlPage =
                    "<html>" + "\r\n" +
                    "<head>" + "\r\n" +
                    "  <title>Openlayers Marker Array Multilayer Example</title>" + "\r\n" +
                    "</head>" + "\r\n" +
                    "<body>" + "\r\n" +
                    "  <div id=\"mapdiv\"></div>" + "\r\n" +
                    "  <script src=\"http://www.openlayers.org/api/OpenLayers.js\"></script>" + "\r\n" +
                    "  <script>" + "\r\n" +
                    "    // Adapted from: harrywood.co.uk" + "\r\n" +
                    "    epsg4326 = new OpenLayers.Projection(\"EPSG:4326\")" + "\r\n" +
                    "    map = new OpenLayers.Map({" + "\r\n" +
                    "      div: \"mapdiv\"," + "\r\n" +
                    "      displayProjection: epsg4326   // With this setting, lat and lon are displayed correctly in MousePosition and permanent anchor" + "\r\n" +
                    "    });" + "\r\n" +
                    "    //   map = new OpenLayers.Map(\"mapdiv\");" + "\r\n" +
                    "    map.addLayer(new OpenLayers.Layer.OSM());" + "\r\n" +
                    "    map.addLayer(new OpenLayers.Layer.OSM(\"Wikimedia\"," + "\r\n" +
                    "      [\"https://maps.wikimedia.org/osm-intl/${z}/${x}/${y}.png\"]," + "\r\n" +
                    "      {" + "\r\n" +
                    "        attribution: \"&copy; <a href='http://www.openstreetmap.org/'>OpenStreetMap</a> and contributors, under an <a href='http://www.openstreetmap.org/copyright' title='ODbL'>open license</a>. <a href='https://www.mediawiki.org/wiki/Maps'>Wikimedia's new style (beta)</a>\"," + "\r\n" +
                    "        \"tileOptions\": { \"crossOriginKeyword\": null }" + "\r\n" +
                    "      })" + "\r\n" +
                    "   );" + "\r\n" +
                    "    // See https://wiki.openstreetmap.org/wiki/Tile_servers for other OSM-based layers" + "\r\n" +
                    "    map.addControls([" + "\r\n" +
                    "      new OpenLayers.Control.MousePosition()," + "\r\n" +
                    "      new OpenLayers.Control.ScaleLine()," + "\r\n" +
                    "      new OpenLayers.Control.LayerSwitcher()," + "\r\n" +
                    "      new OpenLayers.Control.Permalink({ anchor: true })" + "\r\n" +
                    "    ]);" + "\r\n" +
                    "    projectTo = map.getProjectionObject(); //The map projection (Spherical Mercator)" + "\r\n" +
                    "    var lonLat = new OpenLayers.LonLat(" + locationMap +").transform(epsg4326, projectTo);" + "\r\n" +
                    "    var zoom = 7;" + "\r\n" +
                    "    if (!map.getCenter()) {" + "\r\n" +
                    "      map.setCenter(lonLat, zoom);" + "\r\n" +
                    "    }" + "\r\n" +
                    "    // Put your point-definitions here" + "\r\n" +
                    "    var markers = [" + "\r\n" +
                         coordinates + "\r\n" +
                    "    ];" + "\r\n" +
                    "    var colorList = [\"red\", \"blue\", \"yellow\"];" + "\r\n" +
                    "    var layerName = [markers[0][2]];" + "\r\n" +
                    "    var styleArray = [new OpenLayers.StyleMap({ pointRadius: 6, fillColor: colorList[0], fillOpacity: 0.5 })];" + "\r\n" +
                    "    var vectorLayer = [new OpenLayers.Layer.Vector(layerName[0], { styleMap: styleArray[0] })];		// First element defines first Layer" + "\r\n" +
                    "    var j = 0;" + "\r\n" +
                    "    for (var i = 1; i < markers.length; i++) {" + "\r\n" +
                    "      if (!layerName.includes(markers[i][2])) {" + "\r\n" +
                    "        j++;" + "\r\n" +
                    "        layerName.push(markers[i][2]);															// If new layer name found it is created" + "\r\n" +
                    "        styleArray.push(new OpenLayers.StyleMap({ pointRadius: 6, fillColor: colorList[j % colorList.length], fillOpacity: 0.5 }));" + "\r\n" +
                    "        vectorLayer.push(new OpenLayers.Layer.Vector(layerName[j], { styleMap: styleArray[j] }));" + "\r\n" +
                    "      }" + "\r\n" +
                    "    }" + "\r\n" +
                    "    //Loop through the markers array" + "\r\n" +
                    "    for (var i = 0; i < markers.length; i++) {" + "\r\n" +
                    "      var lon = markers[i][0];" + "\r\n" +
                    "      var lat = markers[i][1];" + "\r\n" +
                    "      var feature = new OpenLayers.Feature.Vector(" + "\r\n" +
                    "        new OpenLayers.Geometry.Point(lon, lat).transform(epsg4326, projectTo)," + "\r\n" +
                    "        { description: \"marker number \" + i }" + "\r\n" +
                    "        // see http://dev.openlayers.org/docs/files/OpenLayers/Feature/Vector-js.html#OpenLayers.Feature.Vector.Constants for more options" + "\r\n" +
                    "      );" + "\r\n" +
                    "      vectorLayer[layerName.indexOf(markers[i][2])].addFeatures(feature);" + "\r\n" +
                    "    }" + "\r\n" +
                    "    for (var i = 0; i < layerName.length; i++) {" + "\r\n" +
                    "      map.addLayer(vectorLayer[i]);" + "\r\n" +
                    "    }" + "\r\n" +
                    "  </script>" + "\r\n" +
                    "</body>" + "\r\n" +
                    "</html>" + "\r\n";

                //Create directory, filename and remove old arg file
                string exiftoolArgPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
                if (!Directory.Exists(exiftoolArgPath)) Directory.CreateDirectory(exiftoolArgPath);
                string exiftoolArgFile = Path.Combine(exiftoolArgPath, "openstreetmap.html");
                if (File.Exists(exiftoolArgFile)) File.Delete(exiftoolArgFile);

                using (StreamWriter sw = new StreamWriter(exiftoolArgFile, false, Encoding.UTF8))
                {
                    sw.WriteLine(htmlPage);
                }
                browser.Load(exiftoolArgFile);
            }
        }
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
