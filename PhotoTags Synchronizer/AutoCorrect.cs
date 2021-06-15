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
        [JsonProperty("UseKeywordsFromWebScraping")]
        public bool UseKeywordsFromWebScraping { get; set; } = true;

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
        [JsonProperty("UseFaceRegionFromWebScraping")]
        public bool UseFaceRegionFromWebScraping { get; set; } = true;
        #endregion

        #region Title
        [JsonProperty("UpdateTitle")]
        public bool UpdateTitle { get; set; } = true;
        [JsonProperty("UpdateTitleWithFirstInPrioity")]
        public bool UpdateTitleWithFirstInPrioity { get; set; } = false;
        [JsonProperty("TitlePriority")]
        public List<MetadataBrokerType> TitlePriority { get; set; } = new List<MetadataBrokerType>();
        #endregion 

        #region Album
        [JsonProperty("UpdateAlbum")]
        public bool UpdateAlbum { get; set; } = true;
        [JsonProperty("UpdateAlbumWithFirstInPrioity")]
        public bool UpdateAlbumWithFirstInPrioity { get; set; } = false;
        [JsonProperty("AlbumPriority")]
        public List<MetadataBrokerType> AlbumPriority { get; set; } = new List<MetadataBrokerType>();
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

        #region Rename
        [JsonProperty("RenameVariable")]
        public string RenameVariable { get; set; } = ".\\AutoCorrected\\%Trim%%MediaFileNow_DateTime% %FileNameWithoutDateTime%%Extension%";
        [JsonProperty("RenameAfterAutoCorrect")]
        public bool RenameAfterAutoCorrect { get; set; } = true;

        #endregion

        #region Congig De- Serialization
        public string SerializeThis()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static AutoCorrect ConvertConfigValue(string configString)
        {
            AutoCorrect autoCorrect = JsonConvert.DeserializeObject<AutoCorrect>(configString);
            if (autoCorrect == null) autoCorrect = new AutoCorrect();
            return autoCorrect;
        }
        #endregion

        #region FixAndSave
        public Metadata FixAndSave(FileEntry fileEntry,
            MetadataDatabaseCache metadataAndCacheMetadataExiftool,
            MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos,
            MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery,
            CameraOwnersDatabaseCache cameraOwnersDatabaseCache,
            LocationNameLookUpCache locationNameLookUpCache,
            GoogleLocationHistoryDatabaseCache databaseGoogleLocationHistory,
            float locationAccuracyLatitude,
            float locationAccuracyLongitude
            )
        {
            FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool);
            Metadata metadata = metadataAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
            if (metadata == null) 
                return null; //DEBUG Why NULL - I manage to reproduce, select lot of files, select AutoCorrect many, many times.
            Metadata metadataCopy = new Metadata(metadata); //Make a copy

            

            #region Keywords backup
            if (BackupDateTakenBeforeUpdate && metadataCopy?.MediaDateTaken != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(TimeZone.TimeZoneLibrary.ToStringSortable(metadataCopy?.MediaDateTaken)));            
            if (BackupGPGDateTimeUTCBeforeUpdate && metadataCopy?.LocationDateTime != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadataCopy?.LocationDateTime)));
            #endregion

            #region Find best guess on GPS Location Latitude Longitude
            
            //Don't have GPS locations, try Guess GPS location bases on what exist in Location History

            //If GPS Locations missing!
            //1. Check if GPS DateTime exist, if not then use DateTaken
            //2. Try find location in camera owner's history, ±24 hours
            //3. If location's found, find time zone
            //4. Adjust DateTaken with found time zone
            //5. Find new locations in camra owner's hirstory. ±1 hour
            if (metadataCopy?.LocationLatitude == null || metadataCopy?.LocationLongitude == null)
            {
                string cameraOwner = cameraOwnersDatabaseCache.GetOwenerForCameraMakeModel(metadataCopy?.CameraMake, metadataCopy?.CameraModel);
                if (!string.IsNullOrEmpty(cameraOwner))
                {
                    #region Find or Guess UTC time
                    DateTime? dateTimeUTC = null;

                    if (metadataCopy?.LocationDateTime != null) //If has UTC time
                    {
                        dateTimeUTC = new DateTime(((DateTime)metadataCopy?.LocationDateTime).Ticks, DateTimeKind.Utc);
                    }
                    else if (metadataCopy?.MediaDateTaken != null) //Don't have UTC time, need try to Guess
                    {
                        DateTime mediaDateTimeUnspecified = new DateTime(((DateTime)metadataCopy?.MediaDateTaken).Ticks, DateTimeKind.Unspecified);

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
                            metadataCopy.LocationDateTime = dateTimeUTC;    
                            metadataCopy.LocationLatitude = metadataLocationBasedOnBestGuess.LocationLatitude;       
                            metadataCopy.LocationLongitude = metadataLocationBasedOnBestGuess.LocationLongitude;     
                        }
                    }
                    #endregion
                }

            }
            else //Location is known
            {
                if (UpdateGPSDateTime)
                {
                    if (metadataCopy?.MediaDateTaken != null)
                    {
                        DateTime mediaDateTimeUnspecified = new DateTime(((DateTime)metadataCopy?.MediaDateTaken).Ticks, DateTimeKind.Unspecified);
                        TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataCopy?.LocationLatitude, (double)metadataCopy?.LocationLongitude);
                        metadataCopy.LocationDateTime = TimeZoneInfo.ConvertTimeToUtc(mediaDateTimeUnspecified, timeZoneInfo);
                    }
                    else if (metadataCopy?.LocationDateTime != null)
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
                            newDateTime = metadataCopy?.MediaDateTaken;
                            break;
                        case DateTimeSources.GPSDateAndTime:
                            if (metadataCopy?.LocationLatitude != null && metadataCopy?.LocationLongitude != null && metadataCopy?.LocationDateTime != null)
                            {
                                TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataCopy?.LocationLatitude, (double)metadataCopy?.LocationLongitude);
                                DateTime dateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(((DateTime)metadataCopy?.LocationDateTime).Ticks, DateTimeKind.Utc), timeZoneInfo);
                                newDateTime = dateTimeLocal;
                            }
                            break;
                        case DateTimeSources.FirstDateFoundInFilename:
                            FileDateTimeReader fileDateTimeReader1 = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            List<DateTime> dates1 = fileDateTimeReader1.ListAllDateTimes(Path.GetFileNameWithoutExtension(metadataCopy?.FileName));
                            if (dates1.Count > 0) newDateTime = dates1[0];
                            break;
                        case DateTimeSources.LastDateFoundInFilename:
                            FileDateTimeReader fileDateTimeReader2 = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            List<DateTime> dates2 = fileDateTimeReader2.ListAllDateTimes(Path.GetFileNameWithoutExtension(metadataCopy?.FileName));
                            if (dates2.Count > 0) newDateTime = dates2[dates2.Count-1];
                            break;
                    }
                    if (UpdateTitleWithFirstInPrioity) break;
                    if (newDateTime != null) break;
                }
                if (newDateTime != null) metadataCopy.MediaDateTaken = newDateTime;
                else { 

                }

            }
            #endregion

            #region Location name, city, state, country
            if (UpdateLocation && metadataCopy?.LocationLatitude != null && metadataCopy?.LocationLongitude != null)
            {
                if (!UpdateLocationOnlyWhenEmpty || 
                    string.IsNullOrWhiteSpace(metadataCopy?.LocationName) ||
                    string.IsNullOrWhiteSpace(metadataCopy?.LocationState) ||
                    string.IsNullOrWhiteSpace(metadataCopy?.LocationCity) ||
                    string.IsNullOrWhiteSpace(metadataCopy?.LocationCountry))
                {
                    LocationCoordinateAndDescription locationData = locationNameLookUpCache.AddressLookup(metadataCopy?.LocationCoordinate, locationAccuracyLatitude, locationAccuracyLongitude);
                    if (locationData != null)
                    {
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadataCopy?.LocationName))
                            if (UpdateLocationName) metadataCopy.LocationName = locationData.Description.Name;
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadataCopy?.LocationState))
                            if (UpdateLocationState) metadataCopy.LocationState = locationData.Description.Region;
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadataCopy?.LocationCity))
                            if (UpdateLocationCity) metadataCopy.LocationCity = locationData.Description.City;
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadataCopy?.LocationCountry))
                            if (UpdateLocationCountry) metadataCopy.LocationCountry = locationData.Description.Country;
                    }
                }
            }
            #endregion

            //MicrosoftPhotos
            FileEntryBroker fileEntryBrokerMicrosoftPhotos = new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos);
            Metadata metadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(fileEntryBrokerMicrosoftPhotos);
            Metadata metadataMicrosoftPhotosCopy = metadataMicrosoftPhotos == null ? null : new Metadata(metadataMicrosoftPhotos);

            //WindowsLivePhotoGallery
            FileEntryBroker fileEntryBrokerWindowsLivePhotoGallery = new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery);
            Metadata metadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(fileEntryBrokerWindowsLivePhotoGallery);
            Metadata metadataWindowsLivePhotoGalleryCopy = metadataWindowsLivePhotoGallery == null ? null : new Metadata(metadataWindowsLivePhotoGallery);

            //WebScraping
            FileEntryBroker fileEntryBrokerWebScraping = new FileEntryBroker(fileEntry, MetadataBrokerType.WebScraping);
            Metadata metadataWebScraping = metadataAndCacheMetadataExiftool.ReadWebScraperMetadataFromCacheOrDatabase(fileEntryBrokerWebScraping);
            Metadata metadataWebScrapingCopy = metadataWebScraping == null ? null : new Metadata(metadataWebScraping);

            #region Face region

            //Remove doubles and add names where missing, only work with copy, don't change metadata in buffer.
            if (UseFaceRegionFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null) metadataWindowsLivePhotoGalleryCopy.PersonalRegionRemoveNamelessDoubleRegions(metadataCopy.PersonalRegionList);
            if (UseFaceRegionFromMicrosoftPhotos && metadataMicrosoftPhotosCopy != null) metadataMicrosoftPhotosCopy.PersonalRegionRemoveNamelessDoubleRegions(metadataCopy.PersonalRegionList);

            if (UseFaceRegionFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataWindowsLivePhotoGalleryCopy.PersonalRegionList);
            if (UseFaceRegionFromMicrosoftPhotos && metadataMicrosoftPhotosCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataMicrosoftPhotosCopy.PersonalRegionList);

            if (metadataWebScrapingCopy != null)
            {
                metadataWebScrapingCopy.MediaHeight = metadataCopy.MediaHeight;
                metadataWebScrapingCopy.MediaWidth = metadataCopy.MediaWidth;
                metadataWebScrapingCopy.MediaOrientation = metadataCopy.MediaOrientation;
                metadataWebScrapingCopy.MediaSize = metadataCopy.MediaSize;
                metadataWebScrapingCopy.MediaVideoLength = metadataCopy.MediaVideoLength;

                if (metadataCopy != null) metadataCopy.PersonalRegionSetRegionlessRegions(metadataWebScrapingCopy.PersonalRegionList);
                if (metadataWindowsLivePhotoGalleryCopy != null) metadataWindowsLivePhotoGalleryCopy.PersonalRegionSetRegionlessRegions(metadataWebScrapingCopy.PersonalRegionList);
                if (metadataMicrosoftPhotosCopy != null) metadataMicrosoftPhotosCopy.PersonalRegionSetRegionlessRegions(metadataWebScrapingCopy.PersonalRegionList);
            }

            if (UseFaceRegionFromMicrosoftPhotos && metadataMicrosoftPhotosCopy != null)
            {
                foreach (RegionStructure regionStructure in metadataMicrosoftPhotosCopy.PersonalRegionList) metadataCopy.PersonalRegionListAddIfNameNotExists(regionStructure);
                
            }

            if (UseFaceRegionFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null)
            {
                foreach (RegionStructure regionStructure in metadataWindowsLivePhotoGalleryCopy.PersonalRegionList) metadataCopy.PersonalRegionListAddIfNameNotExists(regionStructure);
            }

            if (UseFaceRegionFromWebScraping && metadataWebScrapingCopy != null)
            {
                foreach (RegionStructure regionStructure in metadataWebScrapingCopy.PersonalRegionList) metadataCopy.PersonalRegionListAddIfNameNotExists(regionStructure);
                
            }
            #endregion

            #region Keywords
            if (UseKeywordsFromMicrosoftPhotos && metadataMicrosoftPhotosCopy != null)
            {
                foreach (KeywordTag keywordTag in metadataMicrosoftPhotosCopy.PersonalKeywordTags)
                {
                    if (keywordTag.Confidence >= KeywordTagConfidenceLevel) metadataCopy.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }

            if (UseKeywordsFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null)
            {
                foreach (KeywordTag keywordTag in metadataWindowsLivePhotoGalleryCopy.PersonalKeywordTags)
                {
                    if (keywordTag.Confidence >= KeywordTagConfidenceLevel) metadataCopy.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }

            if (UseKeywordsFromWebScraping && metadataWebScrapingCopy != null)
            {
                 foreach (KeywordTag keywordTag in metadataWebScrapingCopy.PersonalKeywordTags)
                {
                    if (keywordTag.Confidence >= KeywordTagConfidenceLevel) metadataCopy.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }

            if (BackupDateTakenAfterUpdate && metadataCopy?.MediaDateTaken != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(TimeZone.TimeZoneLibrary.ToStringSortable(metadataCopy?.MediaDateTaken)));
            if (BackupGPGDateTimeUTCAfterUpdate && metadataCopy?.LocationDateTime != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadataCopy?.LocationDateTime)));
            if (BackupRegionFaceNames)
            {
                foreach (RegionStructure regionStructure in metadataCopy?.PersonalRegionList)
                {
                    metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(regionStructure.Name));
                }
            }
            if (BackupLocationName && !string.IsNullOrEmpty(metadataCopy?.LocationName)) metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadataCopy?.LocationName));
            if (BackupLocationCity && !string.IsNullOrEmpty(metadataCopy?.LocationCity)) metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadataCopy?.LocationCity));
            if (BackupLocationState && !string.IsNullOrEmpty(metadataCopy?.LocationState)) metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadataCopy?.LocationState));
            if (BackupLocationCountry && !string.IsNullOrEmpty(metadataCopy?.LocationCountry)) metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadataCopy?.LocationCountry));

            #endregion

            #region Title
            if (UpdateTitle)
            {
                // Find first No empty string
                string newTitle = null;
                foreach (MetadataBrokerType metadataBrokerType in TitlePriority)
                {
                    switch (metadataBrokerType)
                    {
                        case MetadataBrokerType.ExifTool:
                            newTitle = (!string.IsNullOrEmpty(metadataCopy?.PersonalTitle) ? metadataCopy?.PersonalTitle : newTitle);
                            break;
                        case MetadataBrokerType.MicrosoftPhotos:
                            newTitle = (!string.IsNullOrEmpty(metadataMicrosoftPhotos?.PersonalTitle) ? metadataMicrosoftPhotos?.PersonalTitle : newTitle);
                            break;
                        case MetadataBrokerType.WindowsLivePhotoGallery:
                            newTitle = (!string.IsNullOrEmpty(metadataWindowsLivePhotoGallery?.PersonalTitle) ? metadataWindowsLivePhotoGallery?.PersonalTitle : newTitle);
                            break;
                        case MetadataBrokerType.WebScraping:
                            newTitle = (!string.IsNullOrEmpty(metadataWebScraping?.PersonalTitle) ? metadataWebScraping?.PersonalTitle : newTitle);
                            break;
                    }
                    if (UpdateTitleWithFirstInPrioity) break;
                    if (!string.IsNullOrWhiteSpace(newTitle)) break;
                }
                metadataCopy.PersonalTitle = newTitle;
            }
            #endregion 

            #region Album
            if (UpdateAlbum)
            {
                // Find first No empty string
                string newAlbum = null;
                foreach (MetadataBrokerType metadataBrokerType in AlbumPriority)
                {
                    switch (metadataBrokerType)
                    {
                        case MetadataBrokerType.ExifTool:
                            newAlbum = (!string.IsNullOrEmpty(metadataCopy?.PersonalAlbum) ? metadataCopy?.PersonalAlbum : newAlbum);
                            break;
                        case MetadataBrokerType.MicrosoftPhotos:
                            newAlbum = (!string.IsNullOrEmpty(metadataMicrosoftPhotos?.PersonalAlbum) ? metadataMicrosoftPhotos?.PersonalAlbum : newAlbum);
                            break;
                        case MetadataBrokerType.FileSystem:
                            try
                            {
                                newAlbum = new DirectoryInfo(metadataCopy.FileDirectory).Name;
                            }
                            catch 
                            {
                                newAlbum = metadataCopy.FileDirectory;
                            }
                            break;
                        case MetadataBrokerType.WebScraping:
                            newAlbum = (!string.IsNullOrEmpty(metadataWebScraping?.PersonalAlbum) ? metadataWebScraping?.PersonalAlbum : newAlbum);
                            break;
                    }
                    if (UpdateAlbumWithFirstInPrioity) break;
                    if (!string.IsNullOrWhiteSpace(newAlbum)) break;
                }
                metadataCopy.PersonalAlbum = newAlbum;
            }
            #endregion

            #region Author
            if (UpdateAuthor)
            {
                if (!UpdateAuthorOnlyWhenEmpty || !string.IsNullOrWhiteSpace(metadataCopy?.PersonalAuthor))
                {
                    string author = cameraOwnersDatabaseCache.GetOwenerForCameraMakeModel(metadataCopy?.CameraMake, metadataCopy?.CameraModel);
                    if (!string.IsNullOrWhiteSpace(author)) metadataCopy.PersonalAuthor = author;
                }
            }
            #endregion

            return metadataCopy;
        }
        #endregion 
    }
}
