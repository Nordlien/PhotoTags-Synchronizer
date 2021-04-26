#define MonoSqlite
#if MonoSqlite
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
#endif
using SqliteDatabase;
using System.Collections.Generic;

namespace LocationNames
{
    class LocationNameDatabase
    {
        private SqliteDatabaseUtilities dbTools;
        public LocationNameDatabase(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }

        public void TransactionBeginBatch()
        {
            dbTools.TransactionBeginBatch();
        }

        public void TransactionCommitBatch()
        {
            dbTools.TransactionCommitBatch(false);
        }

        #region WriteLocationName
        public void WriteLocationName(LocationCoordinateAndDescription locationCoordinateAndDescription)
        {
            string sqlCommand =
                "INSERT INTO LocationName (Latitude, Longitude, Name, City, Province, Country) " +
                "Values (@Latitude, @Longitude, @Name, @City, @Province, @Country)";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                locationCoordinateAndDescription.Description.Name = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Name) ? null : locationCoordinateAndDescription.Description.Name;
                locationCoordinateAndDescription.Description.Region = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Region) ? null : locationCoordinateAndDescription.Description.Region;
                locationCoordinateAndDescription.Description.City = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.City) ? null : locationCoordinateAndDescription.Description.City;
                locationCoordinateAndDescription.Description.Country = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Country) ? null : locationCoordinateAndDescription.Description.Country;

                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Latitude", locationCoordinateAndDescription.Coordinate.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", locationCoordinateAndDescription.Coordinate.Longitude);
                commandDatabase.Parameters.AddWithValue("@Name", locationCoordinateAndDescription.Description.Name);
                commandDatabase.Parameters.AddWithValue("@City", locationCoordinateAndDescription.Description.City);
                commandDatabase.Parameters.AddWithValue("@Province", locationCoordinateAndDescription.Description.Region);
                commandDatabase.Parameters.AddWithValue("@Country", locationCoordinateAndDescription.Description.Country);
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }
        #endregion 

        #region DeleteLocationName
        public void DeleteLocationName(LocationCoordinate locationCoordinate)
        {
            string sqlCommand = "DELETE FROM LocationName WHERE Latitude = @Latitude AND Longitude = @Longitude";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Latitude", locationCoordinate.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", locationCoordinate.Longitude);
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }
        #endregion 

        #region UpdateLocationName
        public void UpdateLocationName(LocationCoordinateAndDescription locationCoordinateAndDescription)
        {
            string sqlCommand =
                "UPDATE LocationName SET " +
                "Name = @Name, " +
                "City = @City, " +
                "Province = @Province, " +
                "Country = @Country " +
                "WHERE Latitude = @Latitude AND Longitude = @Longitude";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                locationCoordinateAndDescription.Description.Name = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Name) ? null : locationCoordinateAndDescription.Description.Name;
                locationCoordinateAndDescription.Description.Region = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Region) ? null : locationCoordinateAndDescription.Description.Region;
                locationCoordinateAndDescription.Description.City = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.City) ? null : locationCoordinateAndDescription.Description.City;
                locationCoordinateAndDescription.Description.Country = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Country) ? null : locationCoordinateAndDescription.Description.Country;

                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Latitude", locationCoordinateAndDescription.Coordinate.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", locationCoordinateAndDescription.Coordinate.Longitude);
                commandDatabase.Parameters.AddWithValue("@Name", locationCoordinateAndDescription.Description.Name);
                commandDatabase.Parameters.AddWithValue("@City", locationCoordinateAndDescription.Description.City);
                commandDatabase.Parameters.AddWithValue("@Province", locationCoordinateAndDescription.Description.Region);
                commandDatabase.Parameters.AddWithValue("@Country", locationCoordinateAndDescription.Description.Country);
                commandDatabase.ExecuteNonQuery();      // Execute the query
            }
        }
        #endregion 

        #region ReadLocationName
        public LocationCoordinateAndDescription ReadLocationName(LocationCoordinate locationCoordinate, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            LocationCoordinateAndDescription locationCoordinateAndDescription = null;

            string sqlCommand = "SELECT MAX (ABS(Latitude - @Latitude), ABS(Longitude - @Longitude)) AS Distance, " +
                "Latitude, Longitude, Name, City, Province, Country FROM LocationName WHERE " +
                "Latitude > (@Latitude - @LocationAccuracyLatitude) AND (Latitude< @Latitude + @LocationAccuracyLatitude)" +
                "AND Longitude > (@Longitude - @LocationAccuracyLongitude) AND Longitude< (@Longitude + @LocationAccuracyLongitude) " +
                "ORDER BY Distance DESC";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Latitude", locationCoordinate.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", locationCoordinate.Longitude);
                commandDatabase.Parameters.AddWithValue("@LocationAccuracyLatitude", locationAccuracyLatitude);
                commandDatabase.Parameters.AddWithValue("@LocationAccuracyLongitude", locationAccuracyLongitude);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        locationCoordinateAndDescription = new LocationCoordinateAndDescription();
                        locationCoordinateAndDescription.Coordinate.Latitude = (float)dbTools.ConvertFromDBValFloat(reader["Latitude"]);
                        locationCoordinateAndDescription.Coordinate.Longitude = (float)dbTools.ConvertFromDBValFloat(reader["Longitude"]);
                        locationCoordinateAndDescription.Description.Name = dbTools.ConvertFromDBValString(reader["Name"]);
                        locationCoordinateAndDescription.Description.City = dbTools.ConvertFromDBValString(reader["City"]);
                        locationCoordinateAndDescription.Description.Region = dbTools.ConvertFromDBValString(reader["Province"]);
                        locationCoordinateAndDescription.Description.Country = dbTools.ConvertFromDBValString(reader["Country"]);

                        locationCoordinateAndDescription.Description.Name = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Name) ? null : locationCoordinateAndDescription.Description.Name;
                        locationCoordinateAndDescription.Description.Region = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Region) ? null : locationCoordinateAndDescription.Description.Region;
                        locationCoordinateAndDescription.Description.City = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.City) ? null : locationCoordinateAndDescription.Description.City;
                        locationCoordinateAndDescription.Description.Country = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Country) ? null : locationCoordinateAndDescription.Description.Country;
                    }
                }
            }
            return locationCoordinateAndDescription;
        }
        #endregion 

        #region ReadLocationNames
        public Dictionary<LocationCoordinate, LocationDescription> ReadLocationNames()
        {
            Dictionary<LocationCoordinate, LocationDescription> locations = new Dictionary<LocationCoordinate, LocationDescription>();

            string sqlCommand = "SELECT Latitude, Longitude, Name, City, Province, Country FROM LocationName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LocationCoordinate locationCoordinate = new LocationCoordinate(
                            (float)dbTools.ConvertFromDBValFloat(reader["Latitude"]), 
                            (float)dbTools.ConvertFromDBValFloat(reader["Longitude"]));

                        LocationDescription locationDescription = new LocationDescription(
                            dbTools.ConvertFromDBValString(reader["Name"]),
                            dbTools.ConvertFromDBValString(reader["City"]),
                            dbTools.ConvertFromDBValString(reader["Province"]),
                            dbTools.ConvertFromDBValString(reader["Country"]));

                        if (!locations.ContainsKey(locationCoordinate)) locations.Add(locationCoordinate, locationDescription);
                    }
                }
            }
            return locations;
        }
        #endregion 

        #region ReadLocationNames
        public Dictionary<LocationCoordinate, LocationDescription> FindNewLocation()
        {
            Dictionary<LocationCoordinate, LocationDescription> locations = new Dictionary<LocationCoordinate, LocationDescription>();

            string sqlCommand = "SELECT DISTINCT " +
                "Round(LocationLatitude, 5) AS LocationLatitude, " +
                "Round(LocationLongitude, 5) AS LocationLongitude, " +
                "LocationName, LocationCity, LocationState, LocationCountry FROM MediaMetadata";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();

                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        float? locationLatitude = dbTools.ConvertFromDBValFloat(reader["LocationLatitude"]);
                        float? LocationLongitude = dbTools.ConvertFromDBValFloat(reader["LocationLongitude"]);

                        if (locationLatitude != null && LocationLongitude != null)
                        {
                            LocationCoordinate locationCoordinate = new LocationCoordinate((float)locationLatitude, (float)LocationLongitude);

                            LocationDescription locationDescription = new LocationDescription(
                                dbTools.ConvertFromDBValString(reader["LocationName"]),
                                dbTools.ConvertFromDBValString(reader["LocationCity"]),
                                dbTools.ConvertFromDBValString(reader["LocationState"]),
                                dbTools.ConvertFromDBValString(reader["LocationCountry"]));

                            if (!locations.ContainsKey(locationCoordinate)) locations.Add(locationCoordinate, locationDescription);
                            else
                            {
                                if (locations[locationCoordinate] != locationDescription)
                                {
                                    locations[locationCoordinate].Name = null;
                                    locations[locationCoordinate].City = null;
                                    locations[locationCoordinate].Region = null;
                                    locations[locationCoordinate].Country = null;
                                }
                            }
                        }
                    }
                }
            }
            return locations;
        }
        #endregion 

    }


}
