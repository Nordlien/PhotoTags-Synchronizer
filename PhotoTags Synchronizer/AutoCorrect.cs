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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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

        [JsonProperty("LocationFindMinutesNearByMedia")]
        public int LocationFindMinutesNearByMedia { get; set; } = 60; //60 minutes


        [JsonProperty("UpdateGPSLocation")]
        public bool UpdateGPSLocation { get; set; } = true;
        
        [JsonProperty("UpdateGPSLocationNearByMedia")]
        public bool UpdateGPSLocationNearByMedia { get; set; } = true;

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

        [JsonProperty("BackupFileCreatedBeforeUpdate")]
        public bool BackupFileCreatedBeforeUpdate { get; set; } = true;
        [JsonProperty("BackupFileCreatedAfterUpdate")]
        public bool BackupFileCreatedAfterUpdate { get; set; } = true;

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

        #region Description 
        [JsonProperty("UpdateDescription")]
        public bool UpdateDescription { get; set; } = true;
        #endregion 

        #region Comments 
        [JsonProperty("TrackChangesInComments")]
        public bool TrackChangesInComments { get; set; } = true;
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

            //Set defaults
            if (autoCorrect.DateTakenPriority == null) autoCorrect.DateTakenPriority = new List<DateTimeSources>();
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.DateTaken)) autoCorrect.DateTakenPriority.Add(DateTimeSources.DateTaken);
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.GPSDateAndTime)) autoCorrect.DateTakenPriority.Add(DateTimeSources.GPSDateAndTime);
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.FirstDateFoundInFilename)) autoCorrect.DateTakenPriority.Add(DateTimeSources.FirstDateFoundInFilename);
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.LastDateFoundInFilename)) autoCorrect.DateTakenPriority.Add(DateTimeSources.LastDateFoundInFilename);

            if (autoCorrect.TitlePriority == null) autoCorrect.TitlePriority = new List<MetadataBrokerType>();
            if (!autoCorrect.TitlePriority.Contains(MetadataBrokerType.ExifTool)) autoCorrect.TitlePriority.Add(MetadataBrokerType.ExifTool);
            if (!autoCorrect.TitlePriority.Contains(MetadataBrokerType.MicrosoftPhotos)) autoCorrect.TitlePriority.Add(MetadataBrokerType.MicrosoftPhotos);
            if (!autoCorrect.TitlePriority.Contains(MetadataBrokerType.WindowsLivePhotoGallery)) autoCorrect.TitlePriority.Add(MetadataBrokerType.WindowsLivePhotoGallery);
            if (!autoCorrect.TitlePriority.Contains(MetadataBrokerType.WebScraping)) autoCorrect.TitlePriority.Add(MetadataBrokerType.WebScraping);

            if (autoCorrect.AlbumPriority == null) autoCorrect.AlbumPriority = new List<MetadataBrokerType>();

            if (!autoCorrect.AlbumPriority.Contains(MetadataBrokerType.ExifTool)) autoCorrect.AlbumPriority.Add(MetadataBrokerType.ExifTool);
            if (!autoCorrect.AlbumPriority.Contains(MetadataBrokerType.MicrosoftPhotos)) autoCorrect.AlbumPriority.Add(MetadataBrokerType.MicrosoftPhotos);
            if (!autoCorrect.AlbumPriority.Contains(MetadataBrokerType.WebScraping)) autoCorrect.AlbumPriority.Add(MetadataBrokerType.WebScraping);
            if (!autoCorrect.AlbumPriority.Contains(MetadataBrokerType.FileSystem)) autoCorrect.AlbumPriority.Add(MetadataBrokerType.FileSystem);
            return autoCorrect;
        }
        #endregion

        public DateTime? FindBestTimeZone(GoogleLocationHistoryDatabaseCache databaseGoogleLocationHistory, Metadata metadataCopy, string cameraOwner)
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
            return dateTimeUTC;
        }


        #region FixAndSave
        public Metadata FixAndSave(FileEntry fileEntry,
            MetadataDatabaseCache metadataAndCacheMetadataExiftool,
            MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos,
            MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery,
            CameraOwnersDatabaseCache cameraOwnersDatabaseCache,
            LocationNameLookUpCache locationNameLookUpCache,
            GoogleLocationHistoryDatabaseCache databaseGoogleLocationHistory,
            float locationAccuracyLatitude,
            float locationAccuracyLongitude,
            int writeCreatedDateAndTimeAttributeTimeIntervalAccepted
            )
        {
            Logger.Debug("FixAndSave started:" + fileEntry.FileFullPath);

            FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileEntry, MetadataBrokerType.ExifTool);
            Metadata metadata = metadataAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBrokerExiftool);
            if (metadata == null)
            {
                Logger.Warn("FixAndSave ended: metadata is null");
                return null; //DEBUG Why NULL - I manage to reproduce, select lot of files, select AutoCorrect many, many times.
            }

            Metadata metadataCopy = new Metadata(metadata); //Make a copy

            //MicrosoftPhotos
            FileEntryBroker fileEntryBrokerMicrosoftPhotos = new FileEntryBroker(fileEntry, MetadataBrokerType.MicrosoftPhotos);
            Metadata metadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(fileEntryBrokerMicrosoftPhotos);
            Metadata metadataMicrosoftPhotosCopy = metadataMicrosoftPhotos == null ? null : new Metadata(metadataMicrosoftPhotos);
            if (metadataMicrosoftPhotosCopy == null) Logger.Debug("FixAndSave: metadataWindowsLivePhotoGalleryCopy is null");

            //WindowsLivePhotoGallery
            FileEntryBroker fileEntryBrokerWindowsLivePhotoGallery = new FileEntryBroker(fileEntry, MetadataBrokerType.WindowsLivePhotoGallery);
            Metadata metadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(fileEntryBrokerWindowsLivePhotoGallery);
            Metadata metadataWindowsLivePhotoGalleryCopy = metadataWindowsLivePhotoGallery == null ? null : new Metadata(metadataWindowsLivePhotoGallery);
            if (metadataWindowsLivePhotoGalleryCopy == null) Logger.Debug("FixAndSave: metadataWindowsLivePhotoGalleryCopy is null");

            //WebScraping
            FileEntryBroker fileEntryBrokerWebScraping = new FileEntryBroker(fileEntry, MetadataBrokerType.WebScraping);
            Metadata metadataWebScraping = metadataAndCacheMetadataExiftool.ReadWebScraperMetadataFromCacheOrDatabase(fileEntryBrokerWebScraping);
            Metadata metadataWebScrapingCopy = metadataWebScraping == null ? null : new Metadata(metadataWebScraping);
            if (metadataWebScrapingCopy == null) Logger.Debug("FixAndSave: metadataWebScrapingCopy is null");

            #region GPS Location Latitude Longitude, Check if Exist in other sources first 
            if (UpdateGPSLocation && metadataCopy != null) 
            {
                if (metadataCopy?.LocationLatitude == null || metadataCopy?.LocationLongitude == null)
                {
                    metadataCopy.LocationLatitude = metadataWindowsLivePhotoGalleryCopy?.LocationLatitude; 
                    metadataCopy.LocationLongitude = metadataWindowsLivePhotoGalleryCopy?.LocationLongitude;
                }

                if (metadataCopy?.LocationLatitude == null || metadataCopy?.LocationLongitude == null)
                {
                    if (metadataMicrosoftPhotosCopy?.LocationLatitude != 0 && metadataMicrosoftPhotosCopy?.LocationLongitude != 0)
                    {
                        metadataCopy.LocationLatitude = metadataMicrosoftPhotosCopy?.LocationLatitude;
                        metadataCopy.LocationLongitude = metadataMicrosoftPhotosCopy?.LocationLongitude;
                    }
                }

                if (metadataCopy?.LocationLatitude == null || metadataCopy?.LocationLongitude == null)
                {
                    metadataCopy.LocationLatitude = metadataWebScrapingCopy?.LocationLatitude;
                    metadataCopy.LocationLongitude = metadataWebScrapingCopy?.LocationLongitude;
                }
            }
            Logger.Debug("FixAndSave: GPS coordinates:" + (metadataCopy.LocationCoordinate == null ? "null" : metadataCopy.LocationCoordinate.ToString()));
            #endregion

            #region Find best guess on GPS Location Latitude Longitude
            if (UpdateGPSLocation || UpdateGPSLocationNearByMedia)
            {
                //Don't have GPS locations, try Guess GPS location bases on what exist in Location History

                //If GPS Locations missing!
                //1. Check if GPS DateTime exist, if not then use DateTaken
                //2. Try find location in camera owner's history, ±24 hours
                //3. If location's found, find time zone
                //4. Adjust DateTaken with found time zone
                //5. Find new locations in camra owner's hirstory. ±1 hour
                if (metadataCopy?.LocationLatitude == null || metadataCopy?.LocationLongitude == null)
                {
                    DateTime? dateTimeUTC = null;
                    string cameraOwner = cameraOwnersDatabaseCache.GetOwenerForCameraMakeModel(metadataCopy?.CameraMake, metadataCopy?.CameraModel);
                    
                    if (!string.IsNullOrEmpty(cameraOwner))
                    {
                        #region Find or Guess UTC time   
                        Logger.Debug("FixAndSave: -Try Find or Guess UTC time-");
                        if (metadataCopy?.LocationDateTime != null) //If has UTC time
                        {
                            dateTimeUTC = new DateTime(((DateTime)metadataCopy?.LocationDateTime).Ticks, DateTimeKind.Utc);
                            Logger.Debug("FixAndSave: Have UTC time, dateTimeUTC = LocationDateTime: " + dateTimeUTC.ToString());
                        }
                        else if (metadataCopy?.MediaDateTaken != null) //Don't have UTC time, need try to Guess
                        {
                            Logger.Debug("FixAndSave: Don't have UTC time, need try to Guess");

                            DateTime mediaDateTimeUnspecified = new DateTime(((DateTime)metadataCopy?.MediaDateTaken).Ticks, DateTimeKind.Unspecified);

                            //Try find a location nearby
                            Metadata metadataLocationTimeZone = databaseGoogleLocationHistory.FindLocationBasedOnTime(cameraOwner, mediaDateTimeUnspecified, LocationTimeZoneGuessHours * 60 * 60);

                            //Found a location for time zone, however it's up to ±24 hours wrong
                            if (metadataLocationTimeZone != null && metadataLocationTimeZone?.LocationLatitude != null && metadataLocationTimeZone?.LocationLongitude != null)
                            {
                                TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataLocationTimeZone?.LocationLatitude, (double)metadataLocationTimeZone?.LocationLongitude);
                                dateTimeUTC = TimeZoneInfo.ConvertTimeToUtc(mediaDateTimeUnspecified, timeZoneInfo);

                                Logger.Debug("FixAndSave: Found a location (±24 hours), estimated dateTimeUTC: " + dateTimeUTC.ToString() + "for camera owner: " + cameraOwner);
                            }
                            else
                            {
                                Logger.Debug("FixAndSave: No location found (±24 hours) for camera owner: " + cameraOwner);
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

                                Logger.Debug("FixAndSave: Found a location (±1 hours), estimated dateTimeUTC: " + dateTimeUTC.ToString() + "for camera owner: " + cameraOwner);
                            } else
                            {
                                Logger.Debug("FixAndSave: No location found (±1 hours) for camera owner: " + cameraOwner);
                            }
                        }
                        #endregion
                    } else
                    {
                        Logger.Debug("FixAndSave: cameraOwner not found - can't search Location history");
                    }

                    #region UpdateGPSLocationNearByMedia - if still missing location
                    if (UpdateGPSLocationNearByMedia)
                    {
                        if (metadataCopy?.LocationLatitude == null || metadataCopy?.LocationLongitude == null)
                        {
                            Metadata metadataLocationBasedOnBestGuess = databaseGoogleLocationHistory.FindLocationBasedOtherMediaFiles(
                                dateTimeUTC, metadataCopy?.MediaDateTaken, metadataCopy?.FileDateCreated, 60 * LocationFindMinutesNearByMedia);

                            if (metadataLocationBasedOnBestGuess != null && metadataLocationBasedOnBestGuess.LocationLatitude != null && metadataLocationBasedOnBestGuess.LocationLongitude != null)
                            {
                                Logger.Debug("FixAndSave: Location found (±1 hours) using nearby mediafiles");

                                //If allow update location, then updated metadata with found location
                                //metadataCopy.LocationDateTime = dateTimeUTC;
                                metadataCopy.LocationLatitude = metadataLocationBasedOnBestGuess.LocationLatitude;
                                metadataCopy.LocationLongitude = metadataLocationBasedOnBestGuess.LocationLongitude;

                                //Found timezone if missing 
                                if (metadataCopy != null && metadataCopy?.LocationLatitude != null && metadataCopy?.LocationLongitude != null && metadataCopy?.LocationDateTime == null)
                                {
                                    TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataCopy?.LocationLatitude, (double)metadataCopy?.LocationLongitude);
                                    DateTime mediaDateTimeUnspecified;
                                    if (metadataCopy?.MediaDateTaken != null)
                                        mediaDateTimeUnspecified = new DateTime(((DateTime)metadataCopy.MediaDateTaken).Ticks, DateTimeKind.Unspecified);
                                    else
                                        mediaDateTimeUnspecified = new DateTime(((DateTime)metadataCopy.FileDateCreated).Ticks, DateTimeKind.Unspecified);

                                    metadataCopy.LocationDateTime = TimeZoneInfo.ConvertTimeToUtc(mediaDateTimeUnspecified, timeZoneInfo);
                                    if (metadataCopy?.LocationDateTime != null) Logger.Debug("FixAndSave: Location date time was updated to:" + metadataCopy.LocationDateTime.ToString());
                                } 
                                else
                                {
                                    Logger.Debug("FixAndSave: Location date time was not updated");
                                }

                            } else
                            {
                                Logger.Debug("FixAndSave: No location found (±1 hours) using nearby mediafiles");
                            }
                        }
                    }
                    #endregion
                }
                else //Location is known
                {
                    if (UpdateGPSDateTime)
                    {
                        if (metadataCopy?.LocationDateTime != null)
                        {
                            Logger.Debug("FixAndSave: Location date time before update: " + metadataCopy.LocationDateTime.ToString());
                        }

                        if (metadataCopy?.MediaDateTaken != null)
                        {
                            DateTime mediaDateTimeUnspecified = new DateTime(((DateTime)metadataCopy?.MediaDateTaken).Ticks, DateTimeKind.Unspecified);
                            TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataCopy?.LocationLatitude, (double)metadataCopy?.LocationLongitude);
                            metadataCopy.LocationDateTime = TimeZoneInfo.ConvertTimeToUtc(mediaDateTimeUnspecified, timeZoneInfo);
                            if (metadataCopy?.LocationDateTime != null) Logger.Debug("FixAndSave: Location date time updated. Location was known." + metadataCopy.LocationDateTime.ToString());
                        }
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
                            if (newDateTime != null) Logger.Debug("FixAndSave: DateAndTime Digitized was found at DateTimeSources.DateTaken: " + newDateTime.ToString());
                            break;
                        case DateTimeSources.GPSDateAndTime:
                            if (metadataCopy?.LocationLatitude != null && metadataCopy?.LocationLongitude != null && metadataCopy?.LocationDateTime != null)
                            {
                                TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataCopy?.LocationLatitude, (double)metadataCopy?.LocationLongitude);
                                DateTime dateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(((DateTime)metadataCopy?.LocationDateTime).Ticks, DateTimeKind.Utc), timeZoneInfo);
                                newDateTime = dateTimeLocal;
                                if (newDateTime != null) Logger.Debug("FixAndSave: DateAndTime Digitized was found at DateTimeSources.GPSDateAndTime: " + newDateTime.ToString());
                            }
                            
                            break;
                        case DateTimeSources.FirstDateFoundInFilename:
                            FileDateTimeReader fileDateTimeReader1 = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            List<DateTime> dates1 = fileDateTimeReader1.ListAllDateTimes(Path.GetFileNameWithoutExtension(metadataCopy?.FileName));
                            if (dates1.Count > 0)
                            {
                                newDateTime = dates1[0];
                                Logger.Debug("FixAndSave: DateAndTime Digitized was found at DateTimeSources.FirstDateFoundInFilename: " + newDateTime.ToString());
                            }
                            break;
                        case DateTimeSources.LastDateFoundInFilename:
                            FileDateTimeReader fileDateTimeReader2 = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            List<DateTime> dates2 = fileDateTimeReader2.ListAllDateTimes(Path.GetFileNameWithoutExtension(metadataCopy?.FileName));
                            if (dates2.Count > 0)
                            {
                                newDateTime = dates2[dates2.Count - 1];
                                Logger.Debug("FixAndSave: DateAndTime Digitized was found at DateTimeSources.LastDateFoundInFilename: " + newDateTime.ToString());
                            }
                            break;
                    }
                    if (UpdateTitleWithFirstInPrioity) break;
                    if (newDateTime != null) break;
                }
                if (newDateTime != null)
                {
                    metadataCopy.MediaDateTaken = newDateTime;
                    Logger.Debug("FixAndSave: New DateAndTime Digitized was replaced: " + metadataCopy.MediaDateTaken.ToString());
                }
                else
                {
                    Logger.Debug("FixAndSave: No new DateAndTime Digitized was found");
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
                    if (!string.IsNullOrWhiteSpace(keywordTag.Keyword) && keywordTag.Confidence >= KeywordTagConfidenceLevel) 
                        metadataCopy.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }

            if (UseKeywordsFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null)
            {
                foreach (KeywordTag keywordTag in metadataWindowsLivePhotoGalleryCopy.PersonalKeywordTags)
                {
                    if (!string.IsNullOrWhiteSpace(keywordTag.Keyword)) // && keywordTag.Confidence >= KeywordTagConfidenceLevel)
                        metadataCopy.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }

            if (UseKeywordsFromWebScraping && metadataWebScrapingCopy != null)
            {
                foreach (KeywordTag keywordTag in metadataWebScrapingCopy.PersonalKeywordTags)
                {
                    if (!string.IsNullOrWhiteSpace(keywordTag.Keyword)) // && keywordTag.Confidence >= KeywordTagConfidenceLevel)
                        metadataCopy.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }
            #endregion

            #region TrackChangesInComments
            DateTime? newDateTimeFileCreated = metadata.FileDateCreated;
            
            if (metadata.TryParseDateTakenToUtc(out DateTime? dateTakenWithOffset))
            {
                if (metadata?.FileDateCreated != null &&
                    metadata?.MediaDateTaken != null &&
                    metadata?.MediaDateTaken < DateTime.Now &&
                    Math.Abs(((DateTime)dateTakenWithOffset.Value.ToUniversalTime() - (DateTime)metadata?.FileDateCreated.Value.ToUniversalTime()).TotalSeconds) > writeCreatedDateAndTimeAttributeTimeIntervalAccepted) //No need to change
                {
                    newDateTimeFileCreated = (DateTime)dateTakenWithOffset;
                }
            }

            if (TrackChangesInComments)
            {
                if (newDateTimeFileCreated != metadataCopy?.FileDateCreated)
                {
                    metadataCopy.PersonalComments += (string.IsNullOrEmpty(metadataCopy.PersonalComments) ? "" : " ") + "File Created: " +
                        "Old: " + (metadata?.FileDateCreated == null ? "(empty)" : TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.FileDateCreated)) + " " +
                        "New: " + (newDateTimeFileCreated == null ? "(empty)" : TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(newDateTimeFileCreated));
                }

                if (metadata?.MediaDateTaken != metadataCopy?.MediaDateTaken)
                {
                    metadataCopy.PersonalComments += (string.IsNullOrEmpty(metadataCopy.PersonalComments) ? "" : " ") + "DateTaken: " +
                        "Old: " + (metadata?.MediaDateTaken == null ? "(empty)" : TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.MediaDateTaken)) + " " +
                        "New: " + (metadataCopy?.MediaDateTaken == null ? "(empty)" : TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadataCopy?.MediaDateTaken));
                }

                if (metadata?.LocationDateTime != metadataCopy?.LocationDateTime)
                {
                    metadataCopy.PersonalComments += (string.IsNullOrEmpty(metadataCopy.PersonalComments) ? "" : " ") + "UTC date and time: " +
                        "Old: " + (metadata?.LocationDateTime == null ? "(empty)" : TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.LocationDateTime)) + " " +
                        "New: " + (metadataCopy?.LocationDateTime == null ? "(empty)" : TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadataCopy?.LocationDateTime));
                }
            }
            #endregion

            #region Backup dates after changes in keywords
            if (BackupFileCreatedBeforeUpdate && metadata?.FileDateCreated != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("File created: " + TimeZone.TimeZoneLibrary.ToStringSortable(metadata?.FileDateCreated)));

            if (BackupDateTakenBeforeUpdate && metadata?.MediaDateTaken != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("Media taken: " + TimeZone.TimeZoneLibrary.ToStringSortable(metadata?.MediaDateTaken)));

            if (BackupGPGDateTimeUTCBeforeUpdate && metadata?.LocationDateTime != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("UTC time: " + TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.LocationDateTime)));
            #endregion

            #region Backup dates after changes in keywords
            if (BackupFileCreatedAfterUpdate && metadataCopy?.FileDateCreated != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("File created: " + TimeZone.TimeZoneLibrary.ToStringSortable(metadataCopy?.FileDateCreated)));

            if (BackupDateTakenAfterUpdate && metadataCopy?.MediaDateTaken != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("Media taken: " + TimeZone.TimeZoneLibrary.ToStringSortable(metadataCopy?.MediaDateTaken)));

            if (BackupGPGDateTimeUTCAfterUpdate && metadataCopy?.LocationDateTime != null)
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("UTC time: " + TimeZone.TimeZoneLibrary.ToStringW3CDTF_UTC(metadataCopy?.LocationDateTime)));
            #endregion

            #region Backup Region Face Names
            if (BackupRegionFaceNames)
            {
                foreach (RegionStructure regionStructure in metadataCopy?.PersonalRegionList)
                {
                    if (!string.IsNullOrWhiteSpace(regionStructure.Name)) metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(regionStructure.Name));
                }
            }
            #endregion

            #region Backup location Name, City, State/Region, Country
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

            #region Description
            if (UpdateDescription)
            {  
                metadataCopy.PersonalDescription = metadataCopy.PersonalAlbum;
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
