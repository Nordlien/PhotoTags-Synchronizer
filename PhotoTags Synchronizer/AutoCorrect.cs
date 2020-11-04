using CameraOwners;
using Exiftool;
using FileDateTime;
using LocationNames;
using MetadataLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


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

        #region GPS Location

        #endregion

        #region Keywords
        [JsonProperty("UseKeywordsFromWindowsLivePhotoGallery")]
        public bool UseKeywordsFromWindowsLivePhotoGallery { get; set; } = true;
        [JsonProperty("UseKeywordsFromMicrosoftPhotos")]
        public bool UseKeywordsFromMicrosoftPhotos { get; set; } = true;
        [JsonProperty("KeywordTagConfidenceLevel")]
        public float KeywordTagConfidenceLevel { get; set; } = 0.9F;
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


        public string SerializeThis()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static AutoCorrect ConvertConfigValue(string configString)
        {
            return JsonConvert.DeserializeObject<AutoCorrect>(configString); 
        }

        public Metadata FixAndSave(FileEntry fileEntry,
            MetadataDatabaseCache metadataDatabaseCacheExiftool,
            MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos,
            MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery,
            CameraOwnersDatabaseCache cameraOwnersDatabaseCache,
            LocationNameLookUpCache locationNameLookUpCache
            )
        {
            FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileEntry, MetadataBrokerTypes.ExifTool);            
            Metadata metadata = metadataDatabaseCacheExiftool.ReadCache(fileEntryBrokerExiftool);

            FileEntryBroker fileEntryBrokerMicrosoftPhotos = new FileEntryBroker(fileEntry, MetadataBrokerTypes.MicrosoftPhotos);            
            Metadata metadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos.ReadCache(fileEntryBrokerMicrosoftPhotos);

            FileEntryBroker fileEntryBrokerMWindowsLivePhotoGallery = new FileEntryBroker(fileEntry, MetadataBrokerTypes.WindowsLivePhotoGallery);            
            Metadata metadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadCache(fileEntryBrokerMWindowsLivePhotoGallery);


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
                            newDateTime = metadata?.LocationDateTime;
//TimeZone
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
                metadata.MediaDateTaken = newDateTime;

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

            #region Location
            if (UpdateLocation)
            {
                if (!UpdateLocationOnlyWhenEmpty || !string.IsNullOrWhiteSpace(metadata?.LocationName))
                {
                    Metadata locationData = locationNameLookUpCache.AddressLookup((double)metadata?.LocationLatitude, (double)metadata?.LocationLongitude);
                    if (locationData != null)
                    {
                        if (UpdateLocationName) metadata.LocationName = locationData.LocationName;
                        if (UpdateLocationState) metadata.LocationState = locationData.LocationState;
                        if (UpdateLocationCity) metadata.LocationCity = locationData.LocationCity;
                        if (UpdateLocationCountry) metadata.LocationCountry = locationData.LocationCountry;
                    }
                }
            }
            #endregion

            return metadata;
        }
    }
}
