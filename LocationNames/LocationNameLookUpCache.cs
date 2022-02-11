using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using SqliteDatabase;
using System;
using System.Collections.Generic;

namespace LocationNames
{
    public class LocationNameLookUpCache
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private SqliteDatabaseUtilities dbTools;
        public string PreferredLanguagesString { get; set; } = "en"; //"en;q=0.5,no;q=0.3,ru;q=0.2,*;q=0.1";
        public LocationNameLookUpCache(SqliteDatabaseUtilities databaseTools, string preferredLanguagesString)
        {
            dbTools = databaseTools;
            PreferredLanguagesString = preferredLanguagesString;
        }

        #region ReadLocationNames
        public Dictionary<LocationCoordinate, LocationDescription> ReadLocationNames()
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
            return locationNameCache.ReadLocationNames();
        }
        #endregion

        #region FindNewLocation
        public Dictionary<LocationCoordinate, LocationDescription> FindNewLocation()
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
            return locationNameCache.FindNewLocation();
        }
        #endregion

        #region LocationCoordinateInDatabase
        public LocationCoordinate LocationCoordinateInDatabase(LocationCoordinate locationCoordinateSearch, float locationAccuracyLatitude, float locationAccuracyLongitude)
        {
            LocationNameDatabase locationNameDatabase = new LocationNameDatabase(dbTools);
            LocationCoordinateAndDescription locationInDatbaseCoordinateAndDescription = locationNameDatabase.ReadLocationNameFromDatabaseOrCache(locationCoordinateSearch, locationAccuracyLatitude, locationAccuracyLongitude);
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
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
            locationNameCache.TransactionBeginBatch();
            locationNameCache.DeleteLocationName(locationCoordinateInDatabase: locationCoordinateInDatabase);
            locationNameCache.TransactionCommitBatch();
        }
        #endregion

        #region AddressUpdate
        public void AddressUpdate(LocationCoordinate locationCoordinateSearch, LocationCoordinateAndDescription locationCoordinateAndDescription)
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
            locationNameCache.TransactionBeginBatch();
            locationNameCache.UpdateLocationName(
                    locationCoordinateSearch: locationCoordinateSearch, 
                    locationCoordinateAndDescriptionInDatbase: locationCoordinateAndDescription);
            locationNameCache.TransactionCommitBatch();
        }
        #endregion

        #region AddressLookup

        //AddressLookupAndReverseGeocoder

        public LocationCoordinateAndDescription AddressLookupAndReverseGeocoder(
            LocationCoordinate locationCoordinateSearch, float locationAccuracyLatitude, float locationAccuracyLongitude, 
            bool onlyFromCache, bool canReverseGeocoder)
        {
            LocationNameDatabase locationNameDatabase = new LocationNameDatabase(dbTools);

            // Check if exist in cache
            if (onlyFromCache && !locationNameDatabase.LocationCoordinateAndDescriptionExsistInCache(locationCoordinateSearch)) return null;

            // Return location from Cache or Database
            LocationCoordinateAndDescription locationInDatbaseCoordinateAndDescription = locationNameDatabase.ReadLocationNameFromDatabaseOrCache(locationCoordinateSearch, locationAccuracyLatitude, locationAccuracyLongitude);
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

                        locationNameDatabase.TransactionBeginBatch();
                        locationNameDatabase.WriteLocationName(locationCoordinateSearch, locationInDatbaseCoordinateAndDescription);
                        locationNameDatabase.TransactionCommitBatch();
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
                //Update existing
                AddressUpdate(locationCoordinateAndDescriptionFromDatabase.Coordinate, locationCoordinateAndDescriptionSearch);
            }

        }
        #endregion
    }
}
