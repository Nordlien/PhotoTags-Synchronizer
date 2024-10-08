﻿
using System;

namespace MetadataLibrary
{
    [Serializable]
    public enum MetadataBrokerType 
    {
        Empty                   = 0b_0000_0000,  // 0
        ExifTool                = 0b_0000_0001,  // 1
        ExifToolWriteError      = 0b_0000_0010,  // 2
        MicrosoftPhotos         = 0b_0000_0100,  // 4
        WindowsLivePhotoGallery = 0b_0000_1000,  // 8
        GoogleLocationHistory   = 0b_0001_0000,  // 16
        NominatimAPI            = 0b_0010_0000,  // 32
        FileSystem              = 0b_0100_0000,  // 64
        WebScraping             = 0b_1000_0000,  // 128
        UserSavedData           = 0b_1111_1111,  // 255
        Queue                   = 0b_1111_1110   // 254
    }
}

