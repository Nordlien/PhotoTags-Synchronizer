using System;
using System.IO;
using System.Collections.Generic;
using SqliteDatabase;
using SharpKml.Dom;
using SharpKml.Engine;
using SharpKml.Dom.GX;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace GoogleLocationHistory
{
    public class GoogleLocationHistoryKML
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public delegate void MyEvent(object sender, long locationsCount, long filePosition, long fileLength);
        public event EventHandler LocationFound;
        public event MyEvent LocationFoundParam;

        private SqliteDatabaseUtilities dbTools;
        GoogleLocationHistoryDatabaseCache googleLocationDatabaseCache;
        private string username;

        public GoogleLocationHistoryKML(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }

        public void ReadJsonAndWriteToCache(String filePath, String userName)
        {
            username = userName;

            googleLocationDatabaseCache = new GoogleLocationHistoryDatabaseCache(dbTools);
            googleLocationDatabaseCache.TransactionBeginBatch();

            googleLocationDatabaseCache.WriteLocationHistorySource(userName, filePath); //Need check exist
            googleLocationDatabaseCache.TransactionCommitBatch();

            /*
            XmlDataDocument xmldoc = new XmlDataDocument();            
            XElement elmRoot = XElement.Load(filePath);
            string longitude;
            string latitude;

            
            var v = (from page in elmRoot.Elements().Elements().Elements().Nodes() select page);
            if (v != null)
            {
                foreach (var item in v)
                {

                    if (((XElement)item).Name.LocalName == "longitude")
                    {
                        longitude = ((XElement)item).Value;
                    }

                    if (((XElement)item).Name.LocalName == "latitude")
                    {
                        latitude = ((XElement)item).Value;
                    }
                }
            }*/

            /*
            XDocument doc = XDocument.Load(filePath);
            XElement root = doc.Root;
            XNamespace ns = root.GetDefaultNamespace();

            List<XElement> simpleFields;

            List<XElement> extendedDatas = doc.Descendants(ns + "ExtendedData").ToList();

            foreach (XElement extendedData in extendedDatas)
            {
                simpleFields = extendedData.Descendants(ns + "SimpleData").ToList();
            }
            */
            
            TextReader reader = File.OpenText(filePath);
            KmlFile file = KmlFile.Load(reader);
            if (file == null)
            {
                return;
            }

            // It's good practice for the root element of the file to be a Kml element
            if (file.Root is Kml kml)
            {
                ExtractPlacemarks(kml.Feature);
            }
            
        }

        private void ExtractPlacemarks(Feature feature)
        {
            // Is the passed in value a Placemark?
            if (feature is Placemark placemark)
            {
                //placemarks.Add(placemark);
                //string name = placemark.Name;
                //string text = placemark.Description.Text;
                //SharpKml.Dom.Point point = placemark.Geometry as SharpKml.Dom.Point;

                SharpKml.Dom.GX.Track track = placemark.Geometry as SharpKml.Dom.GX.Track;
                SharpKml.Base.Vector[] vector = track.Coordinates.ToArray();
                DateTime[] whenElement =  track.When.ToArray();
                
                for (int i = 0; i < vector.Length; i++)
                {

                    GoogleJsonLocations googleJsonLocations = new GoogleJsonLocations();
                    googleJsonLocations.Accuracy = 0;
                    googleJsonLocations.Altitude = vector[i].Altitude == null ? 0 : (double)vector[i].Altitude;
                    googleJsonLocations.Latitude = vector[i].Latitude;
                    googleJsonLocations.Longitude = vector[i].Longitude;
                    googleJsonLocations.Timestamp = whenElement[i];

                    googleLocationDatabaseCache.WriteLocationHistory(username, googleJsonLocations);
                }
                
                /*
                if (point != null)
                {
                    double? altitude = point.Coordinate.Altitude;
                    double? latitude = point.Coordinate.Latitude;
                    double? longitude = point.Coordinate.Longitude;
                }*/
                //    TimePrimitive time =  placemark.Time;
                
                /*
                tempPlaceMarks[numOfPlaceMarks].Name = placemark.Name;
                tempPlaceMarks[numOfPlaceMarks].Description = placemark.Description.Text;
                tempPlaceMarks[numOfPlaceMarks].StyleUrl = placemark.StyleUrl;
                tempPlaceMarks[numOfPlaceMarks].point = placemark.Geometry as SharpKml.Dom.Point;
                tempPlaceMarks[numOfPlaceMarks].CoordinateX = tempPlaceMarks[numOfPlaceMarks].point.Coordinate.Longitude;
                tempPlaceMarks[numOfPlaceMarks].CoordinateY = tempPlaceMarks[numOfPlaceMarks].point.Coordinate.Latitude;
                tempPlaceMarks[numOfPlaceMarks].CoordinateZ = tempPlaceMarks[numOfPlaceMarks].point.Coordinate.Altitude;
                */
            }
            else
            {
                // Is it a Container, as the Container might have a child Placemark?
                if (feature is Container container)
                {                    
                    // Check each Feature to see if it's a Placemark or another Container
                    foreach (Feature f in container.Features)
                    {
                        ExtractPlacemarks(f);
                    }
                }
            }
        }

    }

    
}

