using Nominatim.API.Address;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using MetadataLibrary;
using SqliteDatabase;
using System.Diagnostics;

namespace LocationNames
{
    public class LocationNameLookUpCache
    {

        private SqliteDatabaseUtilities dbTools;
        public string PreferredLanguagesString { get; set; } = "en"; //"en;q=0.5,no;q=0.3,ru;q=0.2,*;q=0.1";
        public LocationNameLookUpCache(SqliteDatabaseUtilities databaseTools, string preferredLanguagesString)
        {
            dbTools = databaseTools;
            PreferredLanguagesString = preferredLanguagesString;
        }



        public Metadata AddressLookup(float latitude, float longitude)
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);

            Metadata metadata = locationNameCache.ReadLocationName(latitude, longitude);
            if (metadata != null) return metadata;
      
            var y = new ReverseGeocoder();
            var r2 = y.ReverseGeocode(new ReverseGeocodeRequest
            {
                Longitude = longitude, 
                Latitude = latitude,  

                BreakdownAddressElements = true,
                ShowExtraTags = true,
                ShowAlternativeNames = true,
                ShowGeoJSON = true,
                PreferredLanguages = PreferredLanguagesString
            }); 
            r2.Wait();

            if (r2.IsCompleted && !r2.IsFaulted && r2.Result != null)
            {
                metadata = new Metadata(MetadataBrokerTypes.NominatimAPI);

                metadata.LocationCity = (r2.Result.Address.City + " " + r2.Result.Address.Town + " " + r2.Result.Address.Village).Trim().Replace("  ", " ");
                metadata.LocationCountry = r2.Result.Address.Country;
                metadata.LocationLatitude = latitude;
                metadata.LocationLongitude = longitude;
                metadata.LocationName = (r2.Result.Address.Road + " " + r2.Result.Address.HouseNumber).Trim();
                metadata.LocationState = (r2.Result.Address.State + " " + r2.Result.Address.County + " " + r2.Result.Address.Suburb + " " + r2.Result.Address.District + " " + r2.Result.Address.Hamlet).Trim().Replace("  ", " ");

                metadata.LocationName = string.IsNullOrEmpty(metadata.LocationName) ? null : metadata.LocationName;
                metadata.LocationState = string.IsNullOrEmpty(metadata.LocationState) ? null : metadata.LocationState;
                metadata.LocationCity = string.IsNullOrEmpty(metadata.LocationCity) ? null : metadata.LocationCity;
                metadata.LocationCountry = string.IsNullOrEmpty(metadata.LocationCountry) ? null : metadata.LocationCountry;

                locationNameCache.TransactionBeginBatch();
                locationNameCache.WriteLocationName(metadata);
                locationNameCache.TransactionCommitBatch();
                return metadata;

            }
            else
            {
                return null;
            }

            
        }

        public void DeleteLocation(float mediaLatitude, float mediaLongitude)
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
            locationNameCache.TransactionBeginBatch();
            locationNameCache.DeleteLocationName(mediaLatitude, mediaLongitude);
            locationNameCache.TransactionCommitBatch();
        }

        public void AddressUpdate(float mediaLatitude, float mediaLongitude, string locationName, string locationCity, string locationState, string locationCountry)
        {
            Metadata metadata = AddressLookup(mediaLatitude, mediaLongitude);
            if (metadata != null)
            {
                metadata.LocationCity = locationCity;
                metadata.LocationCountry = locationCountry;
                metadata.LocationName = locationName;
                metadata.LocationState = locationState;

                LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);
                locationNameCache.TransactionBeginBatch();
                locationNameCache.UpdateLocationName(metadata);
                locationNameCache.TransactionCommitBatch();
            }

        }

    }
}
