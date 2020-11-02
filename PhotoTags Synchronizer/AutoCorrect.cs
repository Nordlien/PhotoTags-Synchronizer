using Exiftool;
using MetadataLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoTagsSynchronizer
{
 
    class AutoCorrect
    {
        [JsonProperty("UseKeywordsFromWindowsLivePhotoGallery")]
        public bool UseKeywordsFromWindowsLivePhotoGallery { get; set; } = true;
        [JsonProperty("UseKeywordsFromMicrosoftPhotos")]
        public bool UseKeywordsFromMicrosoftPhotos { get; set; } = true;

        [JsonProperty("UseFaceRegionFromWindowsLivePhotoGallery")]
        public bool UseFaceRegionFromWindowsLivePhotoGallery { get; set; } = true;
        [JsonProperty("UseFaceRegionFromMicrosoftPhotos")]
        public bool UseFaceRegionFromMicrosoftPhotos { get; set; } = true;
        [JsonProperty("KeywordTagConfidenceLevel")]
        public float KeywordTagConfidenceLevel { get; set; } = 0.9F;

        [JsonProperty("UpdateTitle")]
        public bool UpdateTitle { get; set; } = true;
        [JsonProperty("UpdateTitleWithFirstInPrioity")]
        public bool UpdateTitleWithFirstInPrioity { get; set; } = false;
        [JsonProperty("TitlePriority")]
        public List<MetadataBrokerTypes> TitlePriority { get; set; } = new List<MetadataBrokerTypes>();

        [JsonProperty("UpdateAlbum")]
        public bool UpdateAlbum { get; set; } = true;
        [JsonProperty("UpdateAlbumWithFirstInPrioity")]
        public bool UpdateAlbumWithFirstInPrioity { get; set; } = false;
        [JsonProperty("AlbumPriority")]
        public List<MetadataBrokerTypes> AlbumPriority { get; set; } = new List<MetadataBrokerTypes>();

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
            MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery
            )
        {
            FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileEntry, MetadataBrokerTypes.ExifTool);            
            Metadata metadata = metadataDatabaseCacheExiftool.ReadCache(fileEntryBrokerExiftool);

            FileEntryBroker fileEntryBrokerMicrosoftPhotos = new FileEntryBroker(fileEntry, MetadataBrokerTypes.MicrosoftPhotos);            
            Metadata metadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos.ReadCache(fileEntryBrokerMicrosoftPhotos);

            FileEntryBroker fileEntryBrokerMWindowsLivePhotoGallery = new FileEntryBroker(fileEntry, MetadataBrokerTypes.WindowsLivePhotoGallery);            
            Metadata metadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadCache(fileEntryBrokerMWindowsLivePhotoGallery);

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
                string newTitle = "";
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
                string newAlbum = "";
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

            return metadata;
        }
    }
}
