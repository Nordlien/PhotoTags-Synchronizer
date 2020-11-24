using CameraOwners;
using Exiftool;
using FileDateTime;
using GoogleLocationHistory;
using LocationNames;
using MetadataLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TimeZone;

namespace PhotoTagsSynchronizer
{
    enum DateTimeSources
    {
        DateTaken,
        GPSDateAndTime,
        FirstDateFoundInFilename,
        LastDateFoundInFilename
    }
 
    class AutoCorrect
    {
        #region DateTaken
        [JsonProperty("UpdateDateTaken")]
        public bool UpdateDateTaken { get; set; } = true;
        
        [JsonProperty("UpdateDateTakenWithFirstInPrioity")]
        public bool UpdateDateTakenWithFirstInPrioity { get; set; } = false;

        [JsonProperty("DateTakenPriority")]
        public List<DateTimeSources> DateTakenPriority { get; set; } = new List<DateTimeSources>();




        #endregion

        #region GPS Location and Date Time

        [JsonProperty("LocationTimeZoneGuessHours")]
        public int LocationTimeZoneGuessHours { get; set; } = 24; //24 hours

        [JsonProperty("LocationFindMinutes")]
        public int LocationFindMinutes { get; set; } = 5; //Five minutes


        [JsonProperty("UpdateGPSLocation")]
        public bool UpdateGPSLocation { get; set; } = true;
        [JsonProperty("UpdateGPSDateTime")]
        public bool UpdateGPSDateTime { get; set; } = true;

        #endregion

        #region Keywords
        [JsonProperty("UseKeywordsFromWindowsLivePhotoGallery")]
        public bool UseKeywordsFromWindowsLivePhotoGallery { get; set; } = true;
        [JsonProperty("UseKeywordsFromMicrosoftPhotos")]
        public bool UseKeywordsFromMicrosoftPhotos { get; set; } = true;
        [JsonProperty("KeywordTagConfidenceLevel")]
        public double KeywordTagConfidenceLevel { get; set; } = 0.9;

        [JsonProperty("BackupDateTakenBeforeUpdate")]
        public bool BackupDateTakenBeforeUpdate { get; set; } = true;
        [JsonProperty("BackupDateTakenAfterUpdate")]
        public bool BackupDateTakenAfterUpdate { get; set; } = true;

        [JsonProperty("BackupGPGDateTimeUTCBeforeUpdate")]
        public bool BackupGPGDateTimeUTCBeforeUpdate { get; set; } = true;
        [JsonProperty("BackupGPGDateTimeUTCAfterUpdate")]
        public bool BackupGPGDateTimeUTCAfterUpdate { get; set; } = true;

        [JsonProperty("BackupRegionFaceNames")] 
        public bool BackupRegionFaceNames { get; set; } = true;

        [JsonProperty("BackupLocationName")]
        public bool BackupLocationName { get; set; } = true;        
        [JsonProperty("BackupLocationCity")]
        public bool BackupLocationCity { get; set; } = true;        
        [JsonProperty("BackupLocationState")]
        public bool BackupLocationState { get; set; } = true;       
        [JsonProperty("BackupLocationCountry")]
        public bool BackupLocationCountry { get; set; } = true;
        #endregion

        #region Face region
        [JsonProperty("UseFaceRegionFromWindowsLivePhotoGallery")]
        public bool UseFaceRegionFromWindowsLivePhotoGallery { get; set; } = true;
        [JsonProperty("UseFaceRegionFromMicrosoftPhotos")]
        public bool UseFaceRegionFromMicrosoftPhotos { get; set; } = true;
        #endregion

        #region Title
        [JsonProperty("UpdateTitle")]
        public bool UpdateTitle { get; set; } = true;
        [JsonProperty("UpdateTitleWithFirstInPrioity")]
        public bool UpdateTitleWithFirstInPrioity { get; set; } = false;
        [JsonProperty("TitlePriority")]
        public List<MetadataBrokerTypes> TitlePriority { get; set; } = new List<MetadataBrokerTypes>();
        #endregion 

        #region Album
        [JsonProperty("UpdateAlbum")]
        public bool UpdateAlbum { get; set; } = true;
        [JsonProperty("UpdateAlbumWithFirstInPrioity")]
        public bool UpdateAlbumWithFirstInPrioity { get; set; } = false;
        [JsonProperty("AlbumPriority")]
        public List<MetadataBrokerTypes> AlbumPriority { get; set; } = new List<MetadataBrokerTypes>();
        #endregion 

        #region Author
        [JsonProperty("UpdateAuthor")]
        public bool UpdateAuthor { get; set; } = true;
        [JsonProperty("UpdateAuthorOnlyWhenEmpty")]
        public bool UpdateAuthorOnlyWhenEmpty { get; set; } = true;
        #endregion

        #region Location
        [JsonProperty("UpdateLocation")]
        public bool UpdateLocation { get; set; } = true;
        [JsonProperty("UpdateLocationOnlyWhenEmpty")]
        public bool UpdateLocationOnlyWhenEmpty { get; set; } = true;


        [JsonProperty("UpdateLocationName")]
        public bool UpdateLocationName { get; set; } = true;
        [JsonProperty("UpdateLocationCity")]
        public bool UpdateLocationCity { get; set; } = true;
        [JsonProperty("UpdateLocationState")]
        public bool UpdateLocationState { get; set; } = true;
        [JsonProperty("UpdateLocationCountry")]
        public bool UpdateLocationCountry { get; set; } = true;
        #endregion

        #region Congig De- Serialization
        public string SerializeThis()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static AutoCorrect ConvertConfigValue(string configString)
        {
            return JsonConvert.DeserializeObject<AutoCorrect>(configString); 
        }
        #endregion

        #region FixAndSave
        public Metadata FixAndSave(FileEntry fileEntry,
            MetadataDatabaseCache metadataDatabaseCacheExiftool,
            MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos,
            MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery,
            CameraOwnersDatabaseCache cameraOwnersDatabaseCache,
            LocationNameLookUpCache locationNameLookUpCache,
            GoogleLocationHistoryDatabaseCache databaseGoogleLocationHistory
            )
        {
            FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileEntry, MetadataBrokerTypes.ExifTool);
            Metadata metadata = metadataDatabaseCacheExiftool.ReadCache(fileEntryBrokerExiftool);
            if (metadata == null) 
                return null; //DEBUG Why NULL - I manage to reproduce, select lot of files, select AutoCorrect many, many times.
            metadata = new Metadata(metadata); //Make a copy

            FileEntryBroker fileEntryBrokerMicrosoftPhotos = new FileEntryBroker(fileEntry, MetadataBrokerTypes.MicrosoftPhotos);            
            Metadata metadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos.ReadCache(fileEntryBrokerMicrosoftPhotos);

            FileEntryBroker fileEntryBrokerMWindowsLivePhotoGallery = new FileEntryBroker(fileEntry, MetadataBrokerTypes.WindowsLivePhotoGallery);            
            Metadata metadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadCache(fileEntryBrokerMWindowsLivePhotoGallery);

            #region Keywords backup
            if (BackupDateTakenBeforeUpdate && metadata?.MediaDateTaken != null)
                metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(TimeZone.TimeZoneLibrary.ToStringSortable(metadata?.MediaDateTaken)));            
            if (BackupGPGDateTimeUTCBeforeUpdate && metadata?.LocationDateTime != null)
                metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.LocationDateTime)));
            #endregion

            #region Find best guess on GPS Location Latitude Longitude
            
            //Don't have GPS locations, try Guess GPS location bases on what exist in Location History

            //If GPS Locations missing!
            //1. Check if GPS DateTime exist, if not then use DateTaken
            //2. Try find location in camera owner's history, ±24 hours
            //3. If location's found, find time zone
            //4. Adjust DateTaken with found time zone
            //5. Find new locations in camra owner's hirstory. ±1 hour
            if (metadata?.LocationLatitude == null || metadata?.LocationLongitude == null)
            {
                string cameraOwner = cameraOwnersDatabaseCache.GetOwenerForCameraMakeModel(metadata?.CameraMake, metadata?.CameraModel);
                if (!string.IsNullOrEmpty(cameraOwner))
                {
                    #region Find or Guess UTC time
                    DateTime? dateTimeUTC = null;

                    if (metadata?.LocationDateTime != null) //If has UTC time
                    {
                        dateTimeUTC = new DateTime(((DateTime)metadata?.LocationDateTime).Ticks, DateTimeKind.Utc);
                    }
                    else if (metadata?.MediaDateTaken != null) //Don't have UTC time, need try to Guess
                    {
                        DateTime mediaDateTimeUnspecified = new DateTime(((DateTime)metadata?.MediaDateTaken).Ticks, DateTimeKind.Unspecified);

                        //Try find a location nearby
                        Metadata metadataLocationTimeZone = databaseGoogleLocationHistory.FindLocationBasedOnTime(
                            cameraOwner, mediaDateTimeUnspecified, LocationTimeZoneGuessHours * 60 * 60);

                        //Found a location for time zone, however it's up to ±24 hours wrong
                        if (metadataLocationTimeZone != null && metadataLocationTimeZone?.LocationLatitude != null && metadataLocationTimeZone?.LocationLongitude != null)
                        {
                            TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation(
                                (double)metadataLocationTimeZone?.LocationLatitude, (double)metadataLocationTimeZone?.LocationLongitude);

                            dateTimeUTC = TimeZoneInfo.ConvertTimeToUtc(mediaDateTimeUnspecified, timeZoneInfo);
                        }
                    }
                    #endregion

                    #region Find best GPS location
                    if (dateTimeUTC != null) //UTC time found, guess location based on UTC time
                    {
                        Metadata metadataLocationBasedOnBestGuess = databaseGoogleLocationHistory.FindLocationBasedOnTime(
                            cameraOwner, dateTimeUTC, 60 * LocationFindMinutes);

                        //If location found, updated metadata with found location
                        if (metadataLocationBasedOnBestGuess != null)
                        {                            
                            //If allow update location, then updated metadata with found location
                            metadata.LocationDateTime = dateTimeUTC;    
                            metadata.LocationLatitude = metadataLocationBasedOnBestGuess.LocationLatitude;       
                            metadata.LocationLongitude = metadataLocationBasedOnBestGuess.LocationLongitude;     
                        }
                    }
                    #endregion
                }

            }
            else //Location is known
            {
                if (UpdateGPSDateTime)
                {
                    if (metadata?.MediaDateTaken != null)
                    {
                        DateTime mediaDateTimeUnspecified = new DateTime(((DateTime)metadata?.MediaDateTaken).Ticks, DateTimeKind.Unspecified);
                        TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadata?.LocationLatitude, (double)metadata?.LocationLongitude);
                        metadata.LocationDateTime = TimeZoneInfo.ConvertTimeToUtc(mediaDateTimeUnspecified, timeZoneInfo);
                    }
                    else if (metadata?.LocationDateTime != null)
                    {
                        //DateTime mediaDateUTC = new DateTime(((DateTime)metadata?.LocationDateTime).Ticks, DateTimeKind.Utc);
                        //TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadata?.LocationLatitude, (double)metadata?.LocationLongitude);
                        //metadata.MediaDateTaken = TimeZoneInfo.ConvertTimeToUtc(mediaDateUTC, timeZoneInfo);
                    }
                }
            }
            #endregion    

            #region DateAndTime Digitized
            if (UpdateDateTaken)
            {
                // Find first No empty date
                DateTime? newDateTime = null;
                foreach (DateTimeSources dateTimeSource in DateTakenPriority)
                {
                    switch (dateTimeSource)
                    {
                        case DateTimeSources.DateTaken:
                            newDateTime = metadata?.MediaDateTaken;
                            break;
                        case DateTimeSources.GPSDateAndTime:
                            if (metadata?.LocationLatitude != null && metadata?.LocationLongitude != null && metadata?.LocationDateTime != null)
                            {
                                TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadata?.LocationLatitude, (double)metadata?.LocationLongitude);
                                DateTime dateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(((DateTime)metadata?.LocationDateTime).Ticks, DateTimeKind.Utc), timeZoneInfo);
                                newDateTime = dateTimeLocal;
                            }
                            break;
                        case DateTimeSources.FirstDateFoundInFilename:
                            FileDateTimeReader fileDateTimeReader1 = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            List<DateTime> dates1 = fileDateTimeReader1.ListAllDateTimes(Path.GetFileNameWithoutExtension(metadata?.FileName));
                            if (dates1.Count > 0) newDateTime = dates1[0];
                            break;
                        case DateTimeSources.LastDateFoundInFilename:
                            FileDateTimeReader fileDateTimeReader2 = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            List<DateTime> dates2 = fileDateTimeReader2.ListAllDateTimes(Path.GetFileNameWithoutExtension(metadata?.FileName));
                            if (dates2.Count > 0) newDateTime = dates2[dates2.Count-1];
                            break;
                    }
                    if (UpdateTitleWithFirstInPrioity) break;
                    if (newDateTime != null) break;
                }
                if (newDateTime != null) metadata.MediaDateTaken = newDateTime;
                else { 

                }

            }
            #endregion

            #region Location name, city, state, country
            if (UpdateLocation && metadata?.LocationLatitude != null && metadata?.LocationLongitude != null)
            {
                if (!UpdateLocationOnlyWhenEmpty || 
                    string.IsNullOrWhiteSpace(metadata?.LocationName) ||
                    string.IsNullOrWhiteSpace(metadata?.LocationState) ||
                    string.IsNullOrWhiteSpace(metadata?.LocationCity) ||
                    string.IsNullOrWhiteSpace(metadata?.LocationCountry))
                {
                    Metadata locationData = locationNameLookUpCache.AddressLookup((float)metadata?.LocationLatitude, (float)metadata?.LocationLongitude);
                    if (locationData != null)
                    {
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadata?.LocationName))
                            if (UpdateLocationName) metadata.LocationName = locationData.LocationName;
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadata?.LocationState))
                            if (UpdateLocationState) metadata.LocationState = locationData.LocationState;
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadata?.LocationCity))
                            if (UpdateLocationCity) metadata.LocationCity = locationData.LocationCity;
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadata?.LocationCountry))
                            if (UpdateLocationCountry) metadata.LocationCountry = locationData.LocationCountry;
                    }
                }
            }
            #endregion

            #region Face region
            if (UseFaceRegionFromMicrosoftPhotos && metadataMicrosoftPhotos != null)
            {
                foreach (RegionStructure regionStructure in metadataMicrosoftPhotos.PersonalRegionList)
                {
                    metadata.PersonalRegionListAddIfNameNotExists(regionStructure);
                }
            }

            if (UseFaceRegionFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGallery != null)
            {
                foreach (RegionStructure regionStructure in metadataWindowsLivePhotoGallery.PersonalRegionList)
                {
                    metadata.PersonalRegionListAddIfNameNotExists(regionStructure);
                }
            }
            #endregion

            #region Keywords
            if (UseKeywordsFromMicrosoftPhotos && metadataMicrosoftPhotos != null)
            {
                foreach (KeywordTag keywordTag in metadataMicrosoftPhotos.PersonalKeywordTags)
                {
                    if (keywordTag.Confidence >= KeywordTagConfidenceLevel) metadata.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }

            if (UseKeywordsFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGallery != null)
            {
                foreach (KeywordTag keywordTag in metadataWindowsLivePhotoGallery.PersonalKeywordTags)
                {
                    if (keywordTag.Confidence >= KeywordTagConfidenceLevel) metadata.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }

            
            if (BackupDateTakenAfterUpdate && metadata?.MediaDateTaken != null)
                metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(TimeZone.TimeZoneLibrary.ToStringSortable(metadata?.MediaDateTaken)));
            if (BackupGPGDateTimeUTCAfterUpdate && metadata?.LocationDateTime != null)
                metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.LocationDateTime)));
            if (BackupRegionFaceNames)
            {
                foreach (RegionStructure regionStructure in metadata?.PersonalRegionList)
                {
                    metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(regionStructure.Name));
                }
            }
            if (BackupLocationName && !string.IsNullOrEmpty(metadata?.LocationName)) metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadata?.LocationName));
            if (BackupLocationCity && !string.IsNullOrEmpty(metadata?.LocationCity)) metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadata?.LocationCity));
            if (BackupLocationState && !string.IsNullOrEmpty(metadata?.LocationState)) metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadata?.LocationState));
            if (BackupLocationCountry && !string.IsNullOrEmpty(metadata?.LocationCountry)) metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadata?.LocationCountry));

            #endregion

            #region Title
            if (UpdateTitle)
            {

                // Find first No empty string
                string newTitle = null;
                foreach (MetadataBrokerTypes metadataBrokerType in TitlePriority)
                {
                    switch (metadataBrokerType)
                    {
                        case MetadataBrokerTypes.ExifTool:
                            newTitle = (!string.IsNullOrEmpty(metadata?.PersonalTitle) ? metadata?.PersonalTitle : newTitle);
                            break;
                        case MetadataBrokerTypes.MicrosoftPhotos:
                            newTitle = (!string.IsNullOrEmpty(metadataMicrosoftPhotos?.PersonalTitle) ? metadataMicrosoftPhotos?.PersonalTitle : newTitle);
                            break;
                        case MetadataBrokerTypes.WindowsLivePhotoGallery:
                            newTitle = (!string.IsNullOrEmpty(metadataWindowsLivePhotoGallery?.PersonalTitle) ? metadataWindowsLivePhotoGallery?.PersonalTitle : newTitle);
                            break;
                    }
                    if (UpdateTitleWithFirstInPrioity) break;
                    if (!string.IsNullOrWhiteSpace(newTitle)) break;
                }
                metadata.PersonalTitle = newTitle;

            }
            #endregion 

            #region Album
            if (UpdateAlbum)
            {

                // Find first No empty string
                string newAlbum = null;
                foreach (MetadataBrokerTypes metadataBrokerType in AlbumPriority)
                {
                    switch (metadataBrokerType)
                    {
                        case MetadataBrokerTypes.ExifTool:
                            newAlbum = (!string.IsNullOrEmpty(metadata?.PersonalAlbum) ? metadata?.PersonalAlbum : newAlbum);
                            break;
                        case MetadataBrokerTypes.MicrosoftPhotos:
                            newAlbum = (!string.IsNullOrEmpty(metadataMicrosoftPhotos?.PersonalAlbum) ? metadataMicrosoftPhotos?.PersonalAlbum : newAlbum);
                            break;
                        case MetadataBrokerTypes.FileSystem:
                            newAlbum = new DirectoryInfo(metadata.FileDirectory).Name;
                            break;
                    }
                    if (UpdateAlbumWithFirstInPrioity) break;
                    if (!string.IsNullOrWhiteSpace(newAlbum)) break;
                }
                metadata.PersonalAlbum = newAlbum;

            }
            #endregion

            #region Author
            if (UpdateAuthor)
            {
                if (!UpdateAuthorOnlyWhenEmpty || !string.IsNullOrWhiteSpace(metadata?.PersonalAuthor))
                {
                    string author = cameraOwnersDatabaseCache.GetOwenerForCameraMakeModel(metadata?.CameraMake, metadata?.CameraModel);
                    if (!string.IsNullOrWhiteSpace(author)) metadata.PersonalAuthor = author;
                }
            }
            #endregion

            return metadata;
        }
        #endregion 
    }
}
