using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MetadataPriorityLibrary
{
    public class CompositeTags
    {
        public const string Ignore = "Ignore";
        public const string NotDefined = "Not defined";
        public const string FileName = "Filename";
        public const string Directory = "Directory";
        public const string FileModificationDateTime = "File Modification Date/Time";
        public const string FileAccessDateTime = "File Access Date/Time";
        public const string FileCreationDateTime = "File Creation Date/Time";
        public const string FileSize = "File Size";
        public const string MIMEType = "MIME Type";
        public const string CameraModelMake = "Camera Model Make";
        public const string CameraModelName = "Camera Model Name";
        public const string MediaHeight = "Media Height";
        public const string MediaWidth = "Media Width";
        public const string DateTimeDigitized = "Date Time Digitized";
        public const string Arthur = "Arthur";
        public const string ArthurStruct = "Arthurs list";
        public const string Description = "Description";
        public const string Title = "Title";
        public const string Comment = "Comment";
        public const string Album = "Album";
        public const string Rating = "Rating";
        public const string RatingPercent = "RatingPercent";
        public const string Location = "Location";
        public const string LocationStruct = "Location Struct";
        public const string City = "City";
        public const string State = "State";
        public const string Country = "Country";
        public const string GPSAltitude = "GPS Altitude";
        public const string GPSLatitude = "GPS Latitude";
        public const string GPSLongitude = "GPS Longitude";
        public const string GPSCoordinates = "GPS Coordinates";
        public const string GPSCoordinatesLatitude = "GPS Coordinates Latitude";
        public const string GPSCoordinatesLongitude = "GPS Coordinates Longitude";
        public const string GPSDateTime = "GPS DateTime";
        public const string FaceRegionIPTC = "Face Region IPTC";
        public const string FaceRegionMicrosoft = "Face Region Microsoft";
        public const string FaceRegionMWG = "Face Region Metadata Working Group";
        public const string KeywordsXML = "Keywords XML";
        public const string KeywordsStruct = "Keywords Struct";
        public const string KeywordsMicrosoft = "Keywords Microsoft";

        //Categories

        public SortedDictionary<string, string> ListAllTags()
        {
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            CompositeTags compositeTags = new CompositeTags();
            foreach (var prop in compositeTags.GetType().GetFields())
            {
                dictionary.Add(prop.Name, (string)prop.GetValue(this));
            }
            return dictionary;
        }
    }
}