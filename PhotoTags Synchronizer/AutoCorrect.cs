using Exiftool;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoTagsSynchronizer
{
    class AutoCorrect
    {
        public Metadata FixAndSave(FileEntry fileEntry,
            MetadataDatabaseCache metadataDatabaseCacheExiftool,
            MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos,
            MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery
            )
        {
            FileEntryBroker fileEntryBrokerExiftool = new FileEntryBroker(fileEntry, MetadataBrokerTypes.ExifTool);
            Metadata metadataExiftool = metadataDatabaseCacheExiftool.ReadCache(fileEntryBrokerExiftool);
            return metadataExiftool;
        }
    }
}
