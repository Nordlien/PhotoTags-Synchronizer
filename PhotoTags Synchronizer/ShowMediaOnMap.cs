using System;
using System.Globalization;
using LocationNames;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CefSharp.WinForms;
using FileHandeling;

namespace PhotoTagsSynchronizer
{
    public enum MapProvider
    {
        OpenStreetMap,
        GoogleMap
    }
    public class ShowMediaOnMap
    {
        public static MapProvider GetMapProvider(string url)
        {
            if (url.ToLower().StartsWith("https://www.google.com/maps/")) return MapProvider.GoogleMap;
            else return MapProvider.OpenStreetMap;
        }

        private static void CalulateLocationDistanceAndCenterPoints(List<LocationCoordinate> locationCoordinates,
            out LocationCoordinate locationCoordinateMap, out LocationCoordinate locationCoordinateMin, out LocationCoordinate locationCoordinateMax)
        {
            bool isDefaultSet = false;
            float maxLongitude = 0;
            float minLongitude = 0;
            float maxLatitude = 0;
            float minLatitude = 0;

            foreach (LocationCoordinate locationCoordinate in locationCoordinates)
            {
                if (!isDefaultSet)
                {
                    isDefaultSet = true;
                    maxLongitude = locationCoordinate.Longitude;
                    minLongitude = locationCoordinate.Longitude;
                    maxLatitude = locationCoordinate.Latitude;
                    minLatitude = locationCoordinate.Latitude;
                }
                if (locationCoordinate.Longitude > maxLongitude) maxLongitude = locationCoordinate.Longitude;
                if (locationCoordinate.Longitude < minLongitude) minLongitude = locationCoordinate.Longitude;
                if (locationCoordinate.Latitude > maxLatitude) maxLatitude = locationCoordinate.Latitude;
                if (locationCoordinate.Latitude < minLatitude) minLatitude = locationCoordinate.Latitude;
            }

            float mapLongitude = (maxLongitude + minLongitude) / 2;
            float mapLatitude = (maxLatitude + minLatitude) / 2;

            locationCoordinateMin = new LocationCoordinate(minLatitude, minLongitude);
            locationCoordinateMax = new LocationCoordinate(maxLatitude, maxLongitude);
            locationCoordinateMap = new LocationCoordinate(mapLatitude, mapLongitude);

        }

        private static int CalulateZoomLevel(LocationCoordinate locationCoordinateMin, LocationCoordinate locationCoordinateMax)
        {
            float distanceLatitude;
            if (locationCoordinateMax.Latitude < 0 && locationCoordinateMin.Latitude < 0)           // -75 <-> -50 = 25 
                distanceLatitude = Math.Abs(locationCoordinateMax.Latitude - locationCoordinateMin.Latitude);
            else if (locationCoordinateMax.Latitude < 0 && locationCoordinateMin.Latitude >= 0)     // -50 <-> 50 = 100
                distanceLatitude = Math.Abs(locationCoordinateMax.Latitude) + locationCoordinateMin.Latitude;
            else if (locationCoordinateMax.Latitude >= 0 && locationCoordinateMin.Latitude < 0)     //  50 <-> -50 = 100
                distanceLatitude = locationCoordinateMax.Latitude + Math.Abs(locationCoordinateMin.Latitude);
            else                                                                                    //  50 <-> 75 = 25
                distanceLatitude = locationCoordinateMax.Latitude - locationCoordinateMin.Latitude;

            float distanceLongitude;
            if (locationCoordinateMax.Longitude < 0 && locationCoordinateMin.Longitude < 0)         // -75 <-> -50 = 25 
                distanceLongitude = Math.Abs(locationCoordinateMax.Longitude - locationCoordinateMin.Longitude);
            else if (locationCoordinateMax.Longitude < 0 && locationCoordinateMin.Longitude >= 0)   // -50 <-> 50 = 100
                distanceLongitude = Math.Abs(locationCoordinateMax.Longitude) + locationCoordinateMin.Longitude;
            else if (locationCoordinateMax.Longitude >= 0 && locationCoordinateMin.Longitude < 0)   //  50 <-> -50 = 100
                distanceLongitude = locationCoordinateMax.Longitude + Math.Abs(locationCoordinateMin.Longitude);
            else                                                                                    //  50 <-> 75 = 25
                distanceLongitude = locationCoordinateMax.Longitude - locationCoordinateMin.Longitude;

            float distanceMax = Math.Max(distanceLatitude, distanceLongitude);

            int zoomLevel = 1;
            if (distanceMax < 0.01) zoomLevel = 15; //OK
            else if (distanceMax < 00.02)
                zoomLevel = 13;
            else if (distanceMax < 00.03) zoomLevel = 12; //OK
            else if (distanceMax < 00.40)
                zoomLevel = 11;
            else if (distanceMax < 00.50)
                zoomLevel = 10;
            else if (distanceMax < 00.60) zoomLevel = 9; //0.54
            else if (distanceMax < 00.80)
                zoomLevel = 8;
            else if (distanceMax < 01.10) zoomLevel = 7; //OK
            else if (distanceMax < 05.00) zoomLevel = 6; //OK; Test 2.83
            else if (distanceMax < 10.00) zoomLevel = 3;
            else if (distanceMax < 18.00) zoomLevel = 3; //OK
            else if (distanceMax < 25.00) zoomLevel = 2; //OK
            return zoomLevel;
        }

        public static int ConvertZoomLevelToGoogle(int zoomLevel)
        {
            return zoomLevel ;
        }

        public static void UpdateBrowserMap(ChromiumWebBrowser chromiumWebBrowser, LocationCoordinate locationCoordinate, int zoomLevel, MapProvider mapProvider)
        {
            switch (mapProvider)
            {
                case MapProvider.OpenStreetMap:
                    //https://www.google.com/maps/search/59.902056,+10.743139/@59.902056,10.7409503,17z
                    //https://www.google.com/maps/search/59.902056,10.743139
                    //https://www.latlong.net/c/?lat=59.902827&long=10.754396
                    //https://www.openstreetmap.org/?mlat=51.510772705078125&mlon=0.054931640625#map=13/51.5147/0.0494

                    chromiumWebBrowser.Load("https://www.openstreetmap.org/?mlat=" +
                        locationCoordinate.Latitude.ToString(CultureInfo.InvariantCulture) +
                        "&mlon=" + locationCoordinate.Longitude.ToString(CultureInfo.InvariantCulture) +
                        "#map=" + zoomLevel.ToString() +
                        "/" + locationCoordinate.Latitude.ToString(CultureInfo.InvariantCulture) +
                        "/" + locationCoordinate.Longitude.ToString(CultureInfo.InvariantCulture));
                    break;
                case MapProvider.GoogleMap:
                    //https://www.google.com/maps/@53.8576492,27.7043077,15z
                    /*chromiumWebBrowser.Load("https://www.google.com/maps/@" +
                        locationCoordinate.Latitude.ToString(CultureInfo.InvariantCulture) +
                        "," + locationCoordinate.Longitude.ToString(CultureInfo.InvariantCulture) +
                        "," + ConvertZoomLevelToGoogle(zoomLevel).ToString() + "z");*/
                    //https://www.google.com/maps/search/?api=1&query=59.97775%2C10.67877
                    chromiumWebBrowser.Load("https://www.google.com/maps/search/?api=1&query=" +
                        locationCoordinate.Latitude.ToString(CultureInfo.InvariantCulture) +
                        "%2C" + locationCoordinate.Longitude.ToString(CultureInfo.InvariantCulture));
                    break;
            }
        }

        

        public static void UpdatedBroswerMap(ChromiumWebBrowser chromiumWebBrowser, List<LocationCoordinate> locationCoordinates, int zoomLevelSiglePointView, MapProvider mapProvider)
        {
            if (locationCoordinates.Count == 1) UpdateBrowserMap(chromiumWebBrowser, locationCoordinates[0], zoomLevelSiglePointView, mapProvider);
            else
            {
                LocationCoordinate locationCoordinateMax;
                LocationCoordinate locationCoordinateMin;
                LocationCoordinate locationCoordinateMap;
                CalulateLocationDistanceAndCenterPoints(locationCoordinates, out locationCoordinateMap, out locationCoordinateMin, out locationCoordinateMax);
                int zoomLevel = CalulateZoomLevel(locationCoordinateMin, locationCoordinateMax);

                switch (mapProvider)
                {
                    case MapProvider.OpenStreetMap:
                        string coordinates = "";
                        
                        foreach (LocationCoordinate locationCoordinate in locationCoordinates)
                        {
                            if (coordinates != "") coordinates += ", ";
                            coordinates +=
                                "[" +
                                (locationCoordinate.Longitude).ToString(CultureInfo.InvariantCulture) + ", " +
                                (locationCoordinate.Latitude).ToString(CultureInfo.InvariantCulture) +
                                ", 'Face']";
                        }

                        string locationMap = "";
                        locationMap = locationCoordinateMap.Longitude.ToString(CultureInfo.InvariantCulture) + ", " + locationCoordinateMap.Latitude.ToString(CultureInfo.InvariantCulture);

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
                            "    var lonLat = new OpenLayers.LonLat(" + locationMap + ").transform(epsg4326, projectTo);" + "\r\n" +
                            "    var zoom = " + zoomLevel.ToString() + ";" + "\r\n" +
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
                        string exiftoolArgFile = FileHandler.GetLocalApplicationDataPath("openstreetmap.html", true, null);
                        if (File.Exists(exiftoolArgFile)) FileHandler.Delete(exiftoolArgFile, false);

                        using (StreamWriter sw = new StreamWriter(exiftoolArgFile, false, Encoding.UTF8))
                        {
                            sw.WriteLine(htmlPage);
                        }
                        chromiumWebBrowser.Load(exiftoolArgFile);
                        break;

                    case MapProvider.GoogleMap:
                        //https://www.google.com/maps/dir/33.93729,-106.85761/33.91629,-106.866761/33.98729,-106.85861//33.91629,-106.866761,11z/@33.9338371,-106.9191602,12z
                        /*
                        https://www.google.com/maps/dir
                          /33.93729,-106.85761
                          /33.91629,-106.866761
                          /33.98729,-106.85861
                          //33.91629,-106.866761,11z
                          /@33.9338371,-106.9191602,12z
                        */
                        /*
                        https://www.google.com/maps/dir
                        /59.97613,10.67896
                        /59.97614,10.67897
                        /59.97615,10.67898
                        /59.976044,10.6788366
                        /@59.9766696,10.676853,17z/data=!3m1!4b1!4m2!4m1!3e0

                        https://www.google.com/maps/dir/59.97615,10.67896/59.97615,10.67896/59.97615,10.67896/59.976044,10.6788366/@59.9766696,10.676853,17z/data=!3m1!4b1!4m2!4m1!3e0
                         */

                        //LocationCoordinate locationCoordinateMap = CalulateSenterOfLocationPoints(locationCoordinates);
                        string googleUrl = "https://www.google.com/maps/dir";
                        foreach (LocationCoordinate locationCoordinate in locationCoordinates)
                        {
                            googleUrl +=
                                "/" + locationCoordinate.Latitude.ToString(CultureInfo.InvariantCulture) +
                                "," + locationCoordinate.Longitude.ToString(CultureInfo.InvariantCulture);
                        
                        }
                        googleUrl += 
                            "/@" + locationCoordinateMap.Latitude.ToString(CultureInfo.InvariantCulture) +
                            "," + locationCoordinateMap.Longitude.ToString(CultureInfo.InvariantCulture) +
                            "," + ConvertZoomLevelToGoogle(zoomLevel).ToString() + "z";

                        chromiumWebBrowser.Load(googleUrl);

                        break;
                }
            }
        }
    }
}
