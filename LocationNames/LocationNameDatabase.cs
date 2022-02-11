#define MonoSqlite
#define noMicrosoftDataSqlite

#if MonoSqlite
using Mono.Data.Sqlite;
#elif MicrosoftDataSqlite
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

using SqliteDatabase;
using System;
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
        public void WriteLocationName(LocationCoordinate locationCoordinateSearch, LocationCoordinateAndDescription locationInDatabaseCoordinateAndDescription)
        {
            string sqlCommand =
                "INSERT INTO LocationName (Latitude, Longitude, Name, City, Province, Country) " +
                "Values (@Latitude, @Longitude, @Name, @City, @Province, @Country)";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                locationInDatabaseCoordinateAndDescription.Description.Name = string.IsNullOrEmpty(locationInDatabaseCoordinateAndDescription.Description.Name) ? null : locationInDatabaseCoordinateAndDescription.Description.Name;
                locationInDatabaseCoordinateAndDescription.Description.Region = string.IsNullOrEmpty(locationInDatabaseCoordinateAndDescription.Description.Region) ? null : locationInDatabaseCoordinateAndDescription.Description.Region;
                locationInDatabaseCoordinateAndDescription.Description.City = string.IsNullOrEmpty(locationInDatabaseCoordinateAndDescription.Description.City) ? null : locationInDatabaseCoordinateAndDescription.Description.City;
                locationInDatabaseCoordinateAndDescription.Description.Country = string.IsNullOrEmpty(locationInDatabaseCoordinateAndDescription.Description.Country) ? null : locationInDatabaseCoordinateAndDescription.Description.Country;

                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Latitude", locationInDatabaseCoordinateAndDescription.Coordinate.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", locationInDatabaseCoordinateAndDescription.Coordinate.Longitude);
                commandDatabase.Parameters.AddWithValue("@Name", locationInDatabaseCoordinateAndDescription.Description.Name);
                commandDatabase.Parameters.AddWithValue("@City", locationInDatabaseCoordinateAndDescription.Description.City);
                commandDatabase.Parameters.AddWithValue("@Province", locationInDatabaseCoordinateAndDescription.Description.Region);
                commandDatabase.Parameters.AddWithValue("@Country", locationInDatabaseCoordinateAndDescription.Description.Country);
                commandDatabase.ExecuteNonQuery();      // Execute the query
                LocationCoordinateAndDescriptionUpdate(locationCoordinateSearch, locationInDatabaseCoordinateAndDescription.Coordinate, locationInDatabaseCoordinateAndDescription.Description);
            }
        }
        #endregion 

        #region DeleteLocationName
        public void DeleteLocationName(LocationCoordinate locationCoordinateInDatabase)
        {
            string sqlCommand = "DELETE FROM LocationName WHERE Latitude = @Latitude AND Longitude = @Longitude";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Latitude", locationCoordinateInDatabase.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", locationCoordinateInDatabase.Longitude);
                int rows = commandDatabase.ExecuteNonQuery();      // Execute the query
                LocationCoordinateAndDescriptionDelete(locationCoordinateSearch: locationCoordinateInDatabase);
            }
        }
        #endregion 

        #region UpdateLocationName
        public void UpdateLocationName(LocationCoordinate locationCoordinateSearch, LocationCoordinateAndDescription locationCoordinateAndDescriptionInDatbase)
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
                locationCoordinateAndDescriptionInDatbase.Description.Name = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatbase.Description.Name) ? null : locationCoordinateAndDescriptionInDatbase.Description.Name;
                locationCoordinateAndDescriptionInDatbase.Description.Region = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatbase.Description.Region) ? null : locationCoordinateAndDescriptionInDatbase.Description.Region;
                locationCoordinateAndDescriptionInDatbase.Description.City = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatbase.Description.City) ? null : locationCoordinateAndDescriptionInDatbase.Description.City;
                locationCoordinateAndDescriptionInDatbase.Description.Country = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatbase.Description.Country) ? null : locationCoordinateAndDescriptionInDatbase.Description.Country;

                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Latitude", locationCoordinateAndDescriptionInDatbase.Coordinate.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", locationCoordinateAndDescriptionInDatbase.Coordinate.Longitude);
                commandDatabase.Parameters.AddWithValue("@Name", locationCoordinateAndDescriptionInDatbase.Description.Name);
                commandDatabase.Parameters.AddWithValue("@City", locationCoordinateAndDescriptionInDatbase.Description.City);
                commandDatabase.Parameters.AddWithValue("@Province", locationCoordinateAndDescriptionInDatbase.Description.Region);
                commandDatabase.Parameters.AddWithValue("@Country", locationCoordinateAndDescriptionInDatbase.Description.Country);
                commandDatabase.ExecuteNonQuery();      // Execute the query
                LocationCoordinateAndDescriptionUpdate(locationCoordinateSearch, locationCoordinateAndDescriptionInDatbase.Coordinate, locationCoordinateAndDescriptionInDatbase.Description);
            }
        }
        #endregion

        #region ReadLocationNameFromDatabaseOrCache
        public LocationCoordinateAndDescription ReadLocationNameFromDatabaseOrCache(LocationCoordinate locationCoordinate, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            if (LocationCoordinateAndDescriptionExsistInCache(locationCoordinate)) return LocationCoordinateAndDescriptionReadFromCache(locationCoordinate);
            return ReadLocationName(locationCoordinate, locationAccuracyLatitude, locationAccuracyLongitude);
        }
        #endregion

        #region ReadLocationName
        public LocationCoordinateAndDescription ReadLocationName(LocationCoordinate locationCoordinateSearch, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            LocationCoordinateAndDescription locationCoordinateAndDescriptionInDatabase = null;

            string sqlCommand = "SELECT MAX (ABS(Latitude - @Latitude), ABS(Longitude - @Longitude)) AS Distance, " +
                "Latitude, Longitude, Name, City, Province, Country FROM LocationName WHERE " +
                "Latitude > (@Latitude - @LocationAccuracyLatitude) AND Latitude < (@Latitude + @LocationAccuracyLatitude) AND " +
                "Longitude > (@Longitude - @LocationAccuracyLongitude) AND Longitude < (@Longitude + @LocationAccuracyLongitude) " +
                "ORDER BY Distance DESC";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Latitude", locationCoordinateSearch.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", locationCoordinateSearch.Longitude);
                commandDatabase.Parameters.AddWithValue("@LocationAccuracyLatitude", locationAccuracyLatitude);
                commandDatabase.Parameters.AddWithValue("@LocationAccuracyLongitude", locationAccuracyLongitude);
                
                using (CommonSqliteDataReader reader = commandDatabase.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        locationCoordinateAndDescriptionInDatabase = new LocationCoordinateAndDescription();
                        locationCoordinateAndDescriptionInDatabase.Coordinate.Latitude = (float)dbTools.ConvertFromDBValFloat(reader["Latitude"]);
                        locationCoordinateAndDescriptionInDatabase.Coordinate.Longitude = (float)dbTools.ConvertFromDBValFloat(reader["Longitude"]);
                        locationCoordinateAndDescriptionInDatabase.Description.Name = dbTools.ConvertFromDBValString(reader["Name"]);
                        locationCoordinateAndDescriptionInDatabase.Description.City = dbTools.ConvertFromDBValString(reader["City"]);
                        locationCoordinateAndDescriptionInDatabase.Description.Region = dbTools.ConvertFromDBValString(reader["Province"]);
                        locationCoordinateAndDescriptionInDatabase.Description.Country = dbTools.ConvertFromDBValString(reader["Country"]);

                        locationCoordinateAndDescriptionInDatabase.Description.Name = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatabase.Description.Name) ? null : locationCoordinateAndDescriptionInDatabase.Description.Name;
                        locationCoordinateAndDescriptionInDatabase.Description.Region = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatabase.Description.Region) ? null : locationCoordinateAndDescriptionInDatabase.Description.Region;
                        locationCoordinateAndDescriptionInDatabase.Description.City = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatabase.Description.City) ? null : locationCoordinateAndDescriptionInDatabase.Description.City;
                        locationCoordinateAndDescriptionInDatabase.Description.Country = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatabase.Description.Country) ? null : locationCoordinateAndDescriptionInDatabase.Description.Country;
                        LocationCoordinateAndDescriptionUpdate(locationCoordinateSearch, locationCoordinateAndDescriptionInDatabase.Coordinate, locationCoordinateAndDescriptionInDatabase.Description);
                    }
                }
            }
            return locationCoordinateAndDescriptionInDatabase;
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

        private static Dictionary<LocationCoordinate, LocationDescription> locationCoordinateAndDescriptionInDatbaseCache = new Dictionary<LocationCoordinate, LocationDescription>();
        private static Dictionary<LocationCoordinate, LocationCoordinate> locationCoordinateConvertFromSearchToDatabaseCache = new Dictionary<LocationCoordinate, LocationCoordinate>();
        private static readonly Object locationCoordinateAndDescriptionCacheLock = new Object();

        #region Cache AddressLookup - Updated    
        private void LocationCoordinateAndDescriptionUpdate(LocationCoordinate locationCoordinateSearch, LocationCoordinate locationCoordinateInDatabase, LocationDescription locationDescription)
        {
            lock (locationCoordinateAndDescriptionCacheLock)
            {
                if (!locationCoordinateConvertFromSearchToDatabaseCache.ContainsKey(locationCoordinateSearch))
                    locationCoordinateConvertFromSearchToDatabaseCache.Add(locationCoordinateSearch, locationCoordinateInDatabase);
                else
                    locationCoordinateConvertFromSearchToDatabaseCache[locationCoordinateSearch] = locationCoordinateInDatabase;

                if (!locationCoordinateAndDescriptionInDatbaseCache.ContainsKey(locationCoordinateInDatabase))
                    locationCoordinateAndDescriptionInDatbaseCache.Add(locationCoordinateInDatabase, locationDescription);
                else
                    locationCoordinateAndDescriptionInDatbaseCache[locationCoordinateInDatabase] = locationDescription;
            }
        }
        #endregion

        #region Cache AddressLookup - Delete from Cache 
        private void LocationCoordinateAndDescriptionDelete(LocationCoordinate locationCoordinateSearch)
        {
            lock (locationCoordinateAndDescriptionCacheLock)
            {   
                if (locationCoordinateConvertFromSearchToDatabaseCache.ContainsKey(locationCoordinateSearch))
                {
                    if (locationCoordinateAndDescriptionInDatbaseCache.ContainsKey(locationCoordinateConvertFromSearchToDatabaseCache[locationCoordinateSearch]))
                        locationCoordinateAndDescriptionInDatbaseCache.Remove(locationCoordinateConvertFromSearchToDatabaseCache[locationCoordinateSearch]);

                    locationCoordinateConvertFromSearchToDatabaseCache.Remove(locationCoordinateSearch);
                }
            }
        }
        #endregion

        #region Cache AddressLookup - Read from Cache 
        private LocationCoordinateAndDescription LocationCoordinateAndDescriptionReadFromCache(LocationCoordinate locationCoordinateSearch)
        {
            lock (locationCoordinateAndDescriptionCacheLock)
            {
                //Return if exist in Database cache
                if (locationCoordinateAndDescriptionInDatbaseCache.ContainsKey(locationCoordinateSearch))
                    return new LocationCoordinateAndDescription(locationCoordinateSearch, locationCoordinateAndDescriptionInDatbaseCache[locationCoordinateSearch]);

                //Check can convert from search to In database Location
                if (locationCoordinateConvertFromSearchToDatabaseCache.ContainsKey(locationCoordinateSearch)) 
                {
                    if (locationCoordinateAndDescriptionInDatbaseCache.ContainsKey(locationCoordinateConvertFromSearchToDatabaseCache[locationCoordinateSearch]))
                    {
                        return new LocationCoordinateAndDescription(locationCoordinateSearch,
                            locationCoordinateAndDescriptionInDatbaseCache[locationCoordinateConvertFromSearchToDatabaseCache[locationCoordinateSearch]]);
                    }
                } 
            }
            return null;
        }
        #endregion

        #region Cache AddressLookup - Exisit in Cache    
        public bool LocationCoordinateAndDescriptionExsistInCache(LocationCoordinate locationCoordinateSearch)
        {
            lock (locationCoordinateAndDescriptionCacheLock)
            {
                if (locationCoordinateAndDescriptionInDatbaseCache.ContainsKey(locationCoordinateSearch)) return true;

                if (locationCoordinateConvertFromSearchToDatabaseCache.ContainsKey(locationCoordinateSearch) &&
                    locationCoordinateAndDescriptionInDatbaseCache.ContainsKey(locationCoordinateConvertFromSearchToDatabaseCache[locationCoordinateSearch]))
                    return true;
            }
            return false;
        }
        #endregion
    }


}
