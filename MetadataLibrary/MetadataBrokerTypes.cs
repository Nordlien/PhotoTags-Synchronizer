
using System;

namespace MetadataLibrary
{
    [Serializable]
    public enum MetadataBrokerTypes 
    {
        Empty                   = 0b_0000_0000,  // 0
        ExifTool                = 0b_0000_0001,  // 1
        ExifToolWriteError      = 0b_0000_0010,  // 2
        MicrosoftPhotos         = 0b_0000_0100,  // 4
        WindowsLivePhotoGallery = 0b_0000_1000,  // 8
        GoogleLocationHistory   = 0b_0001_0000,  // 16
        NominatimAPI            = 0b_0010_0000,  // 32
        FileSystem              = 0b_0100_0000   // 64
    }
}

