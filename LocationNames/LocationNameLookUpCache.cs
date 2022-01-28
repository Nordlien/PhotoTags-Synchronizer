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

        public Dictionary<LocationCoordinate, LocationDescription> ReadLocationNames()
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
            return locationNameCache.ReadLocationNames();
        }

        public Dictionary<LocationCoordinate, LocationDescription> FindNewLocation()
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
            return locationNameCache.FindNewLocation();
        }


        

        public LocationCoordinateAndDescription AddressLookup(LocationCoordinate locationCoordinate, float locationAccuracyLatitude, float locationAccuracyLongitude, bool onlyFromCache)
        {
            LocationNameDatabase locationNameDatabase = new LocationNameDatabase(dbTools);

            if (onlyFromCache && !locationNameDatabase.LocationCoordinateAndDescriptionExsistInCache(locationCoordinate)) return null;

            LocationCoordinateAndDescription locationCoordinateAndDescription = locationNameDatabase.ReadLocationNameFromDatabaseOrCache(locationCoordinate, locationAccuracyLatitude, locationAccuracyLongitude);
            if (locationCoordinateAndDescription != null) return locationCoordinateAndDescription;

            try
            {
                var y = new ReverseGeocoder();
                var r2 = y.ReverseGeocode(new ReverseGeocodeRequest
                {
                    Longitude = locationCoordinate.Longitude,
                    Latitude = locationCoordinate.Latitude,

                    BreakdownAddressElements = true,
                    ShowExtraTags = true,
                    ShowAlternativeNames = true,
                    ShowGeoJSON = true,
                    PreferredLanguages = PreferredLanguagesString
                });
                r2.Wait();

                if (r2.IsCompleted && !r2.IsFaulted && r2.Result != null && r2.Result.Address != null)
                {
                    locationCoordinateAndDescription = new LocationCoordinateAndDescription();

                    locationCoordinateAndDescription.Coordinate.Latitude = locationCoordinate.Latitude;
                    locationCoordinateAndDescription.Coordinate.Longitude = locationCoordinate.Longitude;
                    locationCoordinateAndDescription.Description.City = (r2.Result.Address.City + " " + r2.Result.Address.Town + " " + r2.Result.Address.Village).Trim().Replace("  ", " ");
                    locationCoordinateAndDescription.Description.Country = r2.Result.Address.Country;
                    locationCoordinateAndDescription.Description.Name = (r2.Result.Address.Road + " " + r2.Result.Address.HouseNumber).Trim();
                    locationCoordinateAndDescription.Description.Region = (r2.Result.Address.State + " " + r2.Result.Address.County + " " + r2.Result.Address.Suburb + " " + r2.Result.Address.District + " " + r2.Result.Address.Hamlet).Trim().Replace("  ", " ");

                    locationCoordinateAndDescription.Description.Name = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Name) ? null : locationCoordinateAndDescription.Description.Name;
                    locationCoordinateAndDescription.Description.Region = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Region) ? null : locationCoordinateAndDescription.Description.Region;
                    locationCoordinateAndDescription.Description.City = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.City) ? null : locationCoordinateAndDescription.Description.City;
                    locationCoordinateAndDescription.Description.Country = string.IsNullOrEmpty(locationCoordinateAndDescription.Description.Country) ? null : locationCoordinateAndDescription.Description.Country;

                    locationNameDatabase.TransactionBeginBatch();
                    locationNameDatabase.WriteLocationName(locationCoordinateAndDescription);
                    locationNameDatabase.TransactionCommitBatch();
                    return locationCoordinateAndDescription;

                }
                else
                {
                    return null;
                }
            } catch (Exception ex)
            {
                Logger.Error(ex, "AddressLookup");
            }
            return null;
            
        }

        #region DeleteLocation
        public void DeleteLocation(LocationCoordinate locationCoordinate)
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
            locationNameCache.TransactionBeginBatch();
            locationNameCache.DeleteLocationName(locationCoordinate);
            locationNameCache.TransactionCommitBatch();
        }
        #endregion

        #region AddressUpdate
        public void AddressUpdate(LocationCoordinateAndDescription locationCoordinateAndDescription)
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
            locationNameCache.TransactionBeginBatch();
            locationNameCache.UpdateLocationName(locationCoordinateAndDescription);
            locationNameCache.TransactionCommitBatch();
        }
        #endregion

        #region AddressLookupNearestAndUpdate
        public void AddressLookupNearestAndUpdate(LocationCoordinateAndDescription locationCoordinateAndDescription, float locationAccuracyLatitude, float locationAccuracyLongitude, bool onlyFromCache)
        {
            LocationCoordinateAndDescription locationCoordinateAndDescriptionLookupNearest = 
                AddressLookup(locationCoordinateAndDescription.Coordinate, locationAccuracyLatitude, locationAccuracyLongitude, onlyFromCache);
            if (locationCoordinateAndDescriptionLookupNearest != null)
            {
                locationCoordinateAndDescription.Coordinate = locationCoordinateAndDescriptionLookupNearest.Coordinate;
            }
            AddressUpdate(locationCoordinateAndDescription);
        }
        #endregion
    }
}
