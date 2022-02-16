using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using SqliteDatabase;
using System;
using System.Collections.Generic;

namespace LocationNames
{
    public class LocationNameDatabaseAndLookUpCache
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private SqliteDatabaseUtilities dbTools;
        public string PreferredLanguagesString { get; set; } = "en"; //"en;q=0.5,no;q=0.3,ru;q=0.2,*;q=0.1";
        public LocationNameDatabaseAndLookUpCache(SqliteDatabaseUtilities databaseTools, string preferredLanguagesString)
        {
            dbTools = databaseTools;
            PreferredLanguagesString = preferredLanguagesString;
        }

        #region Database - WriteLocationName
        public void WriteLocationName(LocationCoordinate locationCoordinateSearch, LocationCoordinateAndDescription locationInDatabaseCoordinateAndDescription)
        {
            var sqlTransaction = dbTools.TransactionBegin();

            #region INSERT INTO LocationName 
            string sqlCommand =
                "INSERT INTO LocationName (Latitude, Longitude, Name, City, Province, Country) " +
                "Values (@Latitude, @Longitude, @Name, @City, @Province, @Country)";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
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
                int rowAffted = commandDatabase.ExecuteNonQuery();      // Execute the query
                LocationCoordinateAndDescriptionUpdate(locationCoordinateSearch, locationInDatabaseCoordinateAndDescription.Coordinate, locationInDatabaseCoordinateAndDescription.Description);
            }
            #endregion

            dbTools.TransactionCommit(sqlTransaction);
        }
        #endregion 

        #region Database - DeleteLocationName
        private void DeleteLocationName(LocationCoordinate locationCoordinateInDatabase, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            var sqlTransaction = dbTools.TransactionBegin();

            #region DELETE FROM LocationName 
            string sqlCommand = "DELETE FROM LocationName " +
                //"WHERE Latitude = @Latitude AND Longitude = @Longitude";
                "WHERE Latitude = (SELECT Latitude FROM (" +
                "SELECT Latitude, Name, Max(Abs(Latitude-@Latitude), Abs(Longitude - @Longitude)) AS Distance " +
                "FROM LocationName WHERE Latitude >= (@Latitude - @LocationAccuracyLatitude) AND Latitude <= (@Latitude + @LocationAccuracyLatitude) " +
                "AND Longitude >= (@Longitude - @LocationAccuracyLongitude) AND Longitude <= (@Longitude + @LocationAccuracyLongitude) " +
                "ORDER BY Distance DESC LIMIT 1 " +
                ")) AND " +
                "Longitude = (SELECT Longitude FROM (" +
                "SELECT Longitude, Name, Max(Abs(Latitude-@Latitude), Abs(Longitude - @Longitude)) AS Distance " +
                "FROM LocationName WHERE Latitude >= (@Latitude - @LocationAccuracyLatitude) AND Latitude <= (@Latitude + @LocationAccuracyLatitude) " +
                "AND Longitude >= (@Longitude - @LocationAccuracyLongitude) AND Longitude <= (@Longitude + @LocationAccuracyLongitude) " +
                "ORDER BY Distance DESC LIMIT 1))";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
            {
                //commandDatabase.Prepare();
                commandDatabase.Parameters.AddWithValue("@Latitude", locationCoordinateInDatabase.Latitude);
                commandDatabase.Parameters.AddWithValue("@Longitude", locationCoordinateInDatabase.Longitude);
                commandDatabase.Parameters.AddWithValue("@LocationAccuracyLatitude", locationAccuracyLatitude);
                commandDatabase.Parameters.AddWithValue("@LocationAccuracyLongitude", locationAccuracyLongitude);
                int affectedRows = commandDatabase.ExecuteNonQuery();      // Execute the query
                if (affectedRows <= 0)
                {
                    //DEBUG, This means problem with accurcy in number
                }
                LocationCoordinateAndDescriptionDelete(locationCoordinateInDatabase: locationCoordinateInDatabase);
            }
            #endregion

            dbTools.TransactionCommit(sqlTransaction);
        }
        #endregion 

        #region Database - UpdateLocationName
        private void UpdateLocationName(LocationCoordinate locationCoordinateInDatabase, LocationCoordinateAndDescription locationCoordinateAndDescriptionInDatbase, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            DeleteLocationName(locationCoordinateInDatabase, locationAccuracyLatitude, locationAccuracyLongitude);
            WriteLocationName(locationCoordinateInDatabase, locationCoordinateAndDescriptionInDatbase);
            //string sqlCommand =
            //    "UPDATE LocationName SET " +
            //    "Name = @Name, " +
            //    "City = @City, " +
            //    "Province = @Province, " +
            //    "Country = @Country " +
            //    //"WHERE Latitude = @Latitude AND Longitude = @Longitude";
            //    "WHERE Latitude = (SELECT Latitude FROM (" +
            //    "SELECT Latitude, Name, Max(Abs(Latitude-@Latitude), Abs(Longitude - @Longitude)) AS Distance " +
            //    "FROM LocationName WHERE Latitude >= (@Latitude - @LocationAccuracyLatitude) AND Latitude <= (@Latitude + @LocationAccuracyLatitude) " +
            //    "AND Longitude >= (@Longitude - @LocationAccuracyLongitude) AND Longitude <= (@Longitude + @LocationAccuracyLongitude) " +
            //    "ORDER BY Distance DESC LIMIT 1 " +
            //    ")) AND " +
            //    "Longitude = (SELECT Longitude FROM (" +
            //    "SELECT Longitude, Name, Max(Abs(Latitude-@Latitude), Abs(Longitude - @Longitude)) AS Distance " +
            //    "FROM LocationName WHERE Latitude >= (@Latitude - @LocationAccuracyLatitude) AND Latitude <= (@Latitude + @LocationAccuracyLatitude) " +
            //    "AND Longitude >= (@Longitude - @LocationAccuracyLongitude) AND Longitude <= (@Longitude + @LocationAccuracyLongitude) " +
            //    "ORDER BY Distance DESC LIMIT 1))";

            //using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransaction))
            //{
            //    locationCoordinateAndDescriptionInDatbase.Description.Name = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatbase.Description.Name) ? null : locationCoordinateAndDescriptionInDatbase.Description.Name;
            //    locationCoordinateAndDescriptionInDatbase.Description.Region = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatbase.Description.Region) ? null : locationCoordinateAndDescriptionInDatbase.Description.Region;
            //    locationCoordinateAndDescriptionInDatbase.Description.City = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatbase.Description.City) ? null : locationCoordinateAndDescriptionInDatbase.Description.City;
            //    locationCoordinateAndDescriptionInDatbase.Description.Country = string.IsNullOrEmpty(locationCoordinateAndDescriptionInDatbase.Description.Country) ? null : locationCoordinateAndDescriptionInDatbase.Description.Country;

            //    //commandDatabase.Prepare();
            //    commandDatabase.Parameters.AddWithValue("@Latitude", locationCoordinateInDatabase.Latitude);
            //    commandDatabase.Parameters.AddWithValue("@Longitude", locationCoordinateInDatabase.Longitude);
            //    commandDatabase.Parameters.AddWithValue("@Name", locationCoordinateAndDescriptionInDatbase.Description.Name);
            //    commandDatabase.Parameters.AddWithValue("@City", locationCoordinateAndDescriptionInDatbase.Description.City);
            //    commandDatabase.Parameters.AddWithValue("@Province", locationCoordinateAndDescriptionInDatbase.Description.Region);
            //    commandDatabase.Parameters.AddWithValue("@Country", locationCoordinateAndDescriptionInDatbase.Description.Country);
            //    commandDatabase.Parameters.AddWithValue("@LocationAccuracyLatitude", locationAccuracyLatitude);
            //    commandDatabase.Parameters.AddWithValue("@LocationAccuracyLongitude", locationAccuracyLongitude);
            //    int affetedRows = commandDatabase.ExecuteNonQuery();      // Execute the query
            //    if (affetedRows <= 0)
            //    {
            //        //DEBUG
            //    }
            //    LocationCoordinateAndDescriptionUpdate(locationCoordinateInDatabase, locationCoordinateAndDescriptionInDatbase.Coordinate, locationCoordinateAndDescriptionInDatbase.Description);
            //}
        }
        #endregion

        #region ReadLocationNameFromDatabaseOrCache
        public LocationCoordinateAndDescription ReadLocationNameFromDatabaseOrCache(LocationCoordinate locationCoordinate, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            if (locationCoordinate == null)
            {
                //DEBUG
            }
            if (LocationCoordinateAndDescriptionExsistInCache(locationCoordinate)) return LocationCoordinateAndDescriptionReadFromCache(locationCoordinate);
            return ReadLocationName(locationCoordinate, locationAccuracyLatitude, locationAccuracyLongitude);
        }
        #endregion

        #region Database - ReadLocationName
        private LocationCoordinateAndDescription ReadLocationName(LocationCoordinate locationCoordinateSearch, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            LocationCoordinateAndDescription locationCoordinateAndDescriptionInDatabase = null;

            var sqlTransactionSelect = dbTools.TransactionBeginSelect();

            #region SELECT FROM LocationName 
            string sqlCommand = "SELECT Latitude, Longitude, Name, City, Province, Country, " +
                "Max(Abs(Latitude - @Latitude), Abs(Longitude - @Longitude)) AS Distance " +
                "FROM LocationName WHERE Latitude >= (@Latitude - @LocationAccuracyLatitude) AND Latitude <= (@Latitude + @LocationAccuracyLatitude) " +
                "AND Longitude >= (@Longitude - @LocationAccuracyLongitude) AND Longitude <= (@Longitude + @LocationAccuracyLongitude) " +
                "ORDER BY Distance LIMIT 1";

            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
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
            #endregion

            dbTools.TransactionCommitSelect(sqlTransactionSelect);

            return locationCoordinateAndDescriptionInDatabase;
        }
        #endregion 

        #region Database - ReadAllLocationsData - Config
        public Dictionary<LocationCoordinate, LocationDescription> ReadAllLocationsData()
        {
            Dictionary<LocationCoordinate, LocationDescription> locations = new Dictionary<LocationCoordinate, LocationDescription>();

            var sqlTransactionSelect = dbTools.TransactionBeginSelect();

            #region SELECT Latitude, Longitude, Name, City, Province, Country FROM LocationName
            string sqlCommand = "SELECT Latitude, Longitude, Name, City, Province, Country FROM LocationName";
            using (CommonSqliteCommand commandDatabase = new CommonSqliteCommand(sqlCommand, dbTools.ConnectionDatabase, sqlTransactionSelect))
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
                        LocationCoordinateAndDescriptionUpdate(locationCoordinate, locationCoordinate, locationDescription);
                    }
                }
            }
            #endregion

            dbTools.TransactionCommitSelect(sqlTransactionSelect);

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
        private void LocationCoordinateAndDescriptionDelete(LocationCoordinate locationCoordinateInDatabase)
        {
            lock (locationCoordinateAndDescriptionCacheLock)
            {
                HashSet<LocationCoordinate> locationCoordinates = new HashSet<LocationCoordinate>();

                #region Fina all search locations points to this database location
                foreach (KeyValuePair<LocationCoordinate, LocationCoordinate> keyValuePairs in locationCoordinateConvertFromSearchToDatabaseCache)
                {
                    if (keyValuePairs.Value == locationCoordinateInDatabase) locationCoordinates.Add(keyValuePairs.Key);
                }
                #endregion

                #region Remove all found <Location Search> that points to <Location in Database>
                foreach (LocationCoordinate locationCoordinate in locationCoordinates)
                {
                    locationCoordinateConvertFromSearchToDatabaseCache.Remove(locationCoordinate);
                }
                #endregion

                if (locationCoordinateAndDescriptionInDatbaseCache.ContainsKey(locationCoordinateInDatabase))
                    locationCoordinateAndDescriptionInDatbaseCache.Remove(locationCoordinateInDatabase);
                else
                {
                    //DEBUG
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
                        return new LocationCoordinateAndDescription(
                            locationCoordinateConvertFromSearchToDatabaseCache[locationCoordinateSearch],
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
            if (locationCoordinateSearch == null)
            {
                return false;
                //DEBUG
            }
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

        #region DeleteLocation
        public void DeleteLocation(LocationCoordinate locationCoordinateInDatabase, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            DeleteLocationName(locationCoordinateInDatabase: locationCoordinateInDatabase, locationAccuracyLatitude, locationAccuracyLongitude);
        }
        #endregion

        #region AddressUpdate
        public void AddressUpdate(LocationCoordinate locationCoordinateInDatabase, LocationCoordinateAndDescription locationCoordinateAndDescription, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            UpdateLocationName(
                locationCoordinateInDatabase: locationCoordinateInDatabase, 
                locationCoordinateAndDescriptionInDatbase: locationCoordinateAndDescription, locationAccuracyLatitude, locationAccuracyLongitude);
        }
        #endregion

        #region AddressLookup
        public LocationCoordinateAndDescription AddressLookupAndReverseGeocoder(
            LocationCoordinate locationCoordinateSearch, float locationAccuracyLatitude, float locationAccuracyLongitude, 
            bool onlyFromCache, bool canReverseGeocoder, LocationDescription metadataLocationDescription, bool forceReloadUsingReverseGeocoder)
        {
            if (locationCoordinateSearch == null)
            {
                //DEBUG
                return null;
            }

            #region Only From Cache - Nothing in cache - ** return null **
            if (onlyFromCache && !LocationCoordinateAndDescriptionExsistInCache(locationCoordinateSearch)) return null;
            #endregion

            if (!forceReloadUsingReverseGeocoder)
            {
                #region If exist - ** Return ** location from Cache or Database
                LocationCoordinateAndDescription locationInDatbaseCoordinateAndDescription =
                    ReadLocationNameFromDatabaseOrCache(locationCoordinateSearch, locationAccuracyLatitude, locationAccuracyLongitude);
                if (locationInDatbaseCoordinateAndDescription != null) return locationInDatbaseCoordinateAndDescription;
                #endregion

                #region If not exist in Database, but Metadata has Location info, save it and ** return it ***
                if (metadataLocationDescription != null)
                {
                    locationInDatbaseCoordinateAndDescription = new LocationCoordinateAndDescription(
                        locationCoordinateSearch, metadataLocationDescription);
                    WriteLocationName(locationCoordinateSearch, locationInDatbaseCoordinateAndDescription);
                    return locationInDatbaseCoordinateAndDescription;
                }
                #endregion
            }
            else
            {
                #region When: Force to read from ReverseGeocode - Delete old data and continue to ReverseGeocode
                ReadLocationNameFromDatabaseOrCache(locationCoordinateSearch, locationAccuracyLatitude, locationAccuracyLongitude);
                DeleteLocation(locationCoordinateSearch, locationAccuracyLatitude, locationAccuracyLongitude);
                #endregion
            }

            if (canReverseGeocoder)
            {
                #region ReverseGeocoder
                LocationCoordinateAndDescription locationInDatbaseCoordinateAndDescription;                
                try
                {
                    var y = new ReverseGeocoder();
                    var r2 = y.ReverseGeocode(new ReverseGeocodeRequest
                    {
                        Longitude = locationCoordinateSearch.Longitude,
                        Latitude = locationCoordinateSearch.Latitude,

                        BreakdownAddressElements = true,
                        ShowExtraTags = true,
                        ShowAlternativeNames = true,
                        ShowGeoJSON = true,
                        PreferredLanguages = PreferredLanguagesString
                    });
                    r2.Wait();

                    if (r2.IsCompleted && !r2.IsFaulted && r2.Result != null && r2.Result.Address != null)
                    {
                        locationInDatbaseCoordinateAndDescription = new LocationCoordinateAndDescription();

                        locationInDatbaseCoordinateAndDescription.Coordinate.Latitude = locationCoordinateSearch.Latitude;
                        locationInDatbaseCoordinateAndDescription.Coordinate.Longitude = locationCoordinateSearch.Longitude;
                        locationInDatbaseCoordinateAndDescription.Description.City = (r2.Result.Address.City + " " + r2.Result.Address.Town + " " + r2.Result.Address.Village).Trim().Replace("  ", " ");
                        locationInDatbaseCoordinateAndDescription.Description.Country = r2.Result.Address.Country;
                        locationInDatbaseCoordinateAndDescription.Description.Name = (r2.Result.Address.Road + " " + r2.Result.Address.HouseNumber).Trim();
                        locationInDatbaseCoordinateAndDescription.Description.Region = (r2.Result.Address.State + " " + r2.Result.Address.County + " " + r2.Result.Address.Suburb + " " + r2.Result.Address.District + " " + r2.Result.Address.Hamlet).Trim().Replace("  ", " ");

                        locationInDatbaseCoordinateAndDescription.Description.Name = string.IsNullOrEmpty(locationInDatbaseCoordinateAndDescription.Description.Name) ? null : locationInDatbaseCoordinateAndDescription.Description.Name;
                        locationInDatbaseCoordinateAndDescription.Description.Region = string.IsNullOrEmpty(locationInDatbaseCoordinateAndDescription.Description.Region) ? null : locationInDatbaseCoordinateAndDescription.Description.Region;
                        locationInDatbaseCoordinateAndDescription.Description.City = string.IsNullOrEmpty(locationInDatbaseCoordinateAndDescription.Description.City) ? null : locationInDatbaseCoordinateAndDescription.Description.City;
                        locationInDatbaseCoordinateAndDescription.Description.Country = string.IsNullOrEmpty(locationInDatbaseCoordinateAndDescription.Description.Country) ? null : locationInDatbaseCoordinateAndDescription.Description.Country;

                        WriteLocationName(locationCoordinateSearch, locationInDatbaseCoordinateAndDescription);
                        return locationInDatbaseCoordinateAndDescription;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "AddressLookup");
                }
                #endregion
            }
            return null;
        }
        #endregion

    }
}
