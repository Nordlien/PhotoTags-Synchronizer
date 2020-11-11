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
        public LocationNameLookUpCache(SqliteDatabaseUtilities databaseTools)
        {
            dbTools = databaseTools;
        }



        public Metadata AddressLookup(double latitude, double longitude)
        {
            LocationNameDatabase locationNameCache = new LocationNameDatabase(dbTools);

            Metadata metadata = locationNameCache.ReadLocationName(latitude, longitude);
            if (metadata != null)
            {               
                return metadata;
            }

            var y = new ReverseGeocoder();
            var r2 = y.ReverseGeocode(new ReverseGeocodeRequest
            {
                Longitude = longitude,  //Longitude = 10.7542963027778
                Latitude = latitude, //Latitude = 59.9028396605556, 

                BreakdownAddressElements = true,
                ShowExtraTags = true,
                ShowAlternativeNames = true,
                ShowGeoJSON = true//,
                //PreferredLanguages = "en-US;q=0.8,en;q=0.5,nb;q=0.4,no;q=0.3,ru;q=0.2;*;q=0.1",

            }); 
            r2.Wait();

            if (r2.IsCompleted && !r2.IsFaulted && r2.Result != null)
            {
                metadata = new Metadata(MetadataBrokerTypes.NominatimAPI);

                metadata.LocationCity = (r2.Result.Address.PostCode + " " + r2.Result.Address.City + " " + r2.Result.Address.Town + " " + r2.Result.Address.Village).Trim();
                metadata.LocationCountry = r2.Result.Address.Country;
                metadata.LocationLatitude = latitude;
                metadata.LocationLongitude = longitude;
                metadata.LocationName = (r2.Result.Address.Road + " " + r2.Result.Address.HouseNumber).Trim();
                metadata.LocationState = (r2.Result.Address.State + " " + r2.Result.Address.County + " " + r2.Result.Address.Suburb + " " + r2.Result.Address.District + " " + r2.Result.Address.Hamlet).Trim();

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

        public void AddressUpdate(CommonDatabaseTransaction commonDatabaseTransaction, double mediaLatitude, double mediaLongitude, string locationName, string locationCity, string locationState, string locationCountry)
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
