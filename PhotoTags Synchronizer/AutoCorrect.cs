using Exiftool;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoTagsSynchronizer
{
 
    class AutoCorrect : ApplicationSettingsBase
    {
  

        public bool UseKeywordsFromWindowsLivePhotoGallery { get; set; } = true;
        public bool UseKeywordsFromMicrosoftPhotos { get; set; } = true;

        public bool UseFaceRegionFromWindowsLivePhotoGallery { get; set; } = true;
        public bool UseFaceRegionFromMicrosoftPhotos { get; set; } = true;
        public float KeywordTagConfidenceLevel { get; set; } = 0.9F;

        public bool UpdateTitle { get; set; } = true;
        public bool UpdateTitleWithFirstInPrioity { get; set; } = false;
        public List<MetadataBrokerTypes> TitlePriority { get; set; } = new List<MetadataBrokerTypes>();

        public bool UpdateAlbum { get; set; } = true;
        public bool UpdateAlbumWithFirstInPrioity { get; set; } = false;
        public List<MetadataBrokerTypes> albumPriority { get; set; } = new List<MetadataBrokerTypes>();


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
                foreach (MetadataBrokerTypes metadataBrokerType in albumPriority)
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
