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

        public void TransactionBeginBatch()
        {
            dbTools.TransactionBeginBatch();
        }

        public void TransactionCommitBatch()
        {
            dbTools.TransactionCommitBatch(false);
        }

        #region Database - WriteLocationName
        private void WriteLocationName(LocationCoordinate locationCoordinateSearch, LocationCoordinateAndDescription locationInDatabaseCoordinateAndDescription)
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

        #region Database - DeleteLocationName
        private void DeleteLocationName(LocationCoordinate locationCoordinateInDatabase)
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

        #region Database - UpdateLocationName
        private void UpdateLocationName(LocationCoordinate locationCoordinateSearch, LocationCoordinateAndDescription locationCoordinateAndDescriptionInDatbase)
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

        #region Database - ReadLocationName
        private LocationCoordinateAndDescription ReadLocationName(LocationCoordinate locationCoordinateSearch, float locationAccuracyLatitude, float locationAccuracyLongitude)
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

        #region Database - ReadAllLocationsData - Config
        public Dictionary<LocationCoordinate, LocationDescription> ReadAllLocationsData()
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

        #region LocationCoordinateInDatabase
        public LocationCoordinate LocationCoordinateInDatabase(LocationCoordinate locationCoordinateSearch, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            LocationCoordinateAndDescription locationInDatbaseCoordinateAndDescription = ReadLocationNameFromDatabaseOrCache(locationCoordinateSearch, locationAccuracyLatitude, locationAccuracyLongitude);
            return locationInDatbaseCoordinateAndDescription.Coordinate;
        }
        #endregion



        #region DeleteLocation
        public void DeleteLocationBySearch(LocationCoordinate locationCoordinateBySearch, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            LocationCoordinateAndDescription locationCoordinateAndDescriptionFromDatabase = AddressLookupAndReverseGeocoder(
                locationCoordinateBySearch, locationAccuracyLatitude, locationAccuracyLongitude,
                onlyFromCache: false, canReverseGeocoder: false);
            if (locationCoordinateAndDescriptionFromDatabase != null) 
                DeleteLocation(locationCoordinateAndDescriptionFromDatabase.Coordinate);
            else
                DeleteLocation(locationCoordinateBySearch);
        }

        public void DeleteLocation(LocationCoordinate locationCoordinateInDatabase)
        {
            TransactionBeginBatch();
            DeleteLocationName(locationCoordinateInDatabase: locationCoordinateInDatabase);
            TransactionCommitBatch();
        }
        #endregion

        #region AddressUpdate
        public void AddressUpdate(LocationCoordinate locationCoordinateSearch, LocationCoordinateAndDescription locationCoordinateAndDescription)
        {
            TransactionBeginBatch();
            UpdateLocationName(
                    locationCoordinateSearch: locationCoordinateSearch, 
                    locationCoordinateAndDescriptionInDatbase: locationCoordinateAndDescription);
            TransactionCommitBatch();
        }
        #endregion

        #region AddressLookup

        //AddressLookupAndReverseGeocoder

        public LocationCoordinateAndDescription AddressLookupAndReverseGeocoder(
            LocationCoordinate locationCoordinateSearch, float locationAccuracyLatitude, float locationAccuracyLongitude, 
            bool onlyFromCache, bool canReverseGeocoder)
        {
            // Check if exist in cache
            if (onlyFromCache && !LocationCoordinateAndDescriptionExsistInCache(locationCoordinateSearch)) return null;

            // Return location from Cache or Database
            LocationCoordinateAndDescription locationInDatbaseCoordinateAndDescription = ReadLocationNameFromDatabaseOrCache(locationCoordinateSearch, locationAccuracyLatitude, locationAccuracyLongitude);
            if (locationInDatbaseCoordinateAndDescription != null) return locationInDatbaseCoordinateAndDescription;

            if (canReverseGeocoder)
            {
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

                        TransactionBeginBatch();
                        WriteLocationName(locationCoordinateSearch, locationInDatbaseCoordinateAndDescription);
                        TransactionCommitBatch();
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
            }
            return null;
        }
        #endregion

        #region AddressLookupNearestAndUpdate
        public void AddressUpdateBySearchLocation(
            LocationCoordinateAndDescription locationCoordinateAndDescriptionSearch, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            LocationCoordinateAndDescription locationCoordinateAndDescriptionFromDatabase = AddressLookupAndReverseGeocoder(
                locationCoordinateAndDescriptionSearch.Coordinate, locationAccuracyLatitude, locationAccuracyLongitude,
                onlyFromCache: false, canReverseGeocoder: false);

            if (locationCoordinateAndDescriptionFromDatabase == null)
            {
                //Didn't exist in database, new will be created
                AddressUpdate(locationCoordinateAndDescriptionSearch.Coordinate, locationCoordinateAndDescriptionSearch);
            } else
            {
                //Update existing using nearby Coordinate 
                AddressUpdate(locationCoordinateAndDescriptionFromDatabase.Coordinate, locationCoordinateAndDescriptionSearch);
            }

        }
        #endregion
    }
}
