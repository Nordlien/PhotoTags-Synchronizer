using System;

namespace MetadataLibrary
{
    [Serializable]
    public enum RegionStructureTypes
    {
        MetadataWorkingGroup = 1,           //MWG RegionInfo
        WindowsLivePhotoGallery = 2,        //Microsoft RegionInfoMP structure
        IptcRelative = 3,                   //IPTC Relative ImageRegion
        IptcPixel = 4,                      //IPTC Pixel ImageRegion
        MicrosoftPhotosDatabase = 5,        //From Microsoft Photos database
        WindowsLivePhotoGalleryDatabase = 6 //From Windows Live Photo Gallery
    }
}
