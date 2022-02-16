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
        //public event EventHandler LocationFound;
        public event MyEvent LocationFoundParam;

        private SqliteDatabaseUtilities dbTools;
        private static GoogleLocationHistoryDatabaseCache googleLocationDatabaseCache;
        private string username;

        public GoogleLocationHistoryKML(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }

        #region ReadJsonAndWriteToCache
        public void ReadJsonAndWriteToCache(String filePath, String userName)
        {
            username = userName;

            googleLocationDatabaseCache = new GoogleLocationHistoryDatabaseCache(dbTools);
            googleLocationDatabaseCache.WriteLocationHistorySource(userName, filePath); //Need check exist
            
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
        #endregion

        #region ExtractPlacemarks
        private void ExtractPlacemarks(Feature feature)
        {
            // Is the passed in value a Placemark?
            if (feature is Placemark placemark)
            {
                SharpKml.Dom.GX.Track track = placemark.Geometry as SharpKml.Dom.GX.Track;
                SharpKml.Base.Vector[] vector = track.Coordinates.ToArray();
                DateTime[] whenElement =  track.When.ToArray();

                var sqlTransactionBatch = googleLocationDatabaseCache.TransactionBegin();
                for (int i = 0; i < vector.Length; i++)
                {
                    if (i % 5000 == 0)
                    {
                        googleLocationDatabaseCache.TransactionCommit(sqlTransactionBatch);
                        sqlTransactionBatch = googleLocationDatabaseCache.TransactionBegin();
                    }
                    LocationFoundParam?.Invoke(this, i, i, vector.Length);

                    GoogleJsonLocations googleJsonLocations = new GoogleJsonLocations();
                    googleJsonLocations.Accuracy = 0;
                    googleJsonLocations.Altitude = vector[i].Altitude == null ? 0 : (float)vector[i].Altitude;
                    googleJsonLocations.Latitude = (float)vector[i].Latitude;
                    googleJsonLocations.Longitude = (float)vector[i].Longitude;
                    googleJsonLocations.Timestamp = whenElement[i];
                    googleLocationDatabaseCache.WriteLocationHistory(username, googleJsonLocations);
                }
                googleLocationDatabaseCache.TransactionCommit(sqlTransactionBatch);
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
        #endregion 
    }


}

