using FileDateTime;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using TimeZone;

namespace MetadataLibrary
{

    [Serializable]
    public class Metadata
    {
        //Fields size https://owl.phy.queensu.ca/~phil/exiftool/TagNames.pdf

        public String errors = "";
        private String fileDirectory;
        private Byte? personalRating;
        private Byte? personalRatingPercent;
        private List<RegionStructure> personalRegionList = new List<RegionStructure>();
        private List<KeywordTag> personalTagList = new List<KeywordTag>();

        //Media
        private DateTime? mediaDateTaken;
        private Int32? mediaWidth;
        private Int32? mediaHeight;
        private Int32? mediaOrientation;
        private Int32? mediaVideoLength;

        //Location
        private float? locationAltitude;
        private float? locationLatitude;
        private float? locationLongitude;
        private DateTime? locationDateTime;
        private String locationName;
        private String locationCountry;
        private String locationCity;
        private String locationState;

        #region Constructors
        public Metadata(MetadataBrokerTypes broker)
        {
            Broker = broker;
        }

        public Metadata(Metadata metadata)
        {
            errors = metadata.errors;

            //Broker
            Broker = metadata.Broker;

            //File
            FileName = metadata.FileName;
            fileDirectory = metadata.fileDirectory;
            FileSize = metadata.FileSize;
            FileDateCreated = metadata.FileDateCreated;
            FileDateModified = metadata.FileDateModified;
            FileLastAccessed = metadata.FileLastAccessed;
            FileMimeType = metadata.FileMimeType;

            //Personal
            PersonalTitle = metadata.PersonalTitle;
            PersonalDescription = metadata.PersonalDescription;
            PersonalComments = metadata.PersonalComments;
            personalRating = metadata.personalRating;
            personalRatingPercent = metadata.personalRatingPercent;
            PersonalAuthor = metadata.PersonalAuthor;
            PersonalAlbum = metadata.PersonalAlbum;
            foreach (RegionStructure region in metadata.personalRegionList) personalRegionList.Add(new RegionStructure(region));
            foreach (KeywordTag tag in metadata.personalTagList) personalTagList.Add(new KeywordTag(tag));

            //Camera
            CameraMake = metadata.CameraMake;
            CameraModel = metadata.CameraModel;

            //Media
            MediaDateTaken = metadata.mediaDateTaken;
            MediaWidth = metadata.mediaWidth;
            MediaHeight = metadata.mediaHeight;
            MediaOrientation = metadata.mediaOrientation;
            MediaVideoLength = metadata.mediaVideoLength;

            //Location
            LocationAltitude = metadata.locationAltitude;
            LocationLatitude = metadata.locationLatitude;
            LocationLongitude = metadata.locationLongitude;
            LocationDateTime = metadata.locationDateTime;
            LocationName = metadata.locationName;
            LocationCountry = metadata.locationCountry;
            LocationCity = metadata.locationCity;
            LocationState = metadata.locationState;

        }
        #endregion 

        #region Override
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return this == (Metadata)obj;
            }
        }

        private static bool VerifyRegionStructureList(List<RegionStructure> personalRegionList1, List<RegionStructure> personalRegionList2)
        {
            foreach (RegionStructure region in personalRegionList1) if (!region.DoesThisRectangleAndNameExistInList(personalRegionList2)) return false;
            foreach (RegionStructure region in personalRegionList2) if (!region.DoesThisRectangleAndNameExistInList(personalRegionList1)) return false;
            return true;
        }

        private static bool VerifyKeywordList(List<KeywordTag> personalKeywordTagList1, List<KeywordTag> personalKeywordTagList2)
        {
            if (personalKeywordTagList1.Count != personalKeywordTagList2.Count) return false;
            foreach (KeywordTag tag in personalKeywordTagList1) if (!personalKeywordTagList2.Contains(tag)) return false;
            foreach (KeywordTag tag in personalKeywordTagList2) if (!personalKeywordTagList1.Contains(tag)) return false;
            return true;
        }

        public static bool operator ==(Metadata m1, Metadata m2)
        {
            if (m1 is null && m2 is null) return true;
            if (m1 is null) return false;
            if (m2 is null) return false;
            if (ReferenceEquals(m1, m2)) return true;
            //return m1.GetHashCode() == m2.GetHashCode() ;

            //Broker
            if (m1.Broker != m2.Broker) return false;

            //File
            if (m1.FileName != m2.FileName) return false;
            if (m1.fileDirectory != m2.fileDirectory) return false;
            if (m1.FileSize != m2.FileSize) return false;
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.FileDateCreated, m2.FileDateCreated)) return false;
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.FileDateModified, m2.FileDateModified)) return false;
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.FileLastAccessed, m2.FileLastAccessed)) return false;
            if (m1.FileMimeType != m2.FileMimeType) return false;

            //Personal
            if (m1.PersonalTitle != m2.PersonalTitle) return false;
            if (m1.PersonalDescription != m2.PersonalDescription) return false;
            if (m1.PersonalComments != m2.PersonalComments) return false;
            if (m1.personalRating != m2.personalRating) return false;
            if (m1.personalRatingPercent != m2.personalRatingPercent) return false;
            if (m1.PersonalAuthor != m2.PersonalAuthor) return false;
            if (m1.PersonalAlbum != m2.PersonalAlbum) return false;

            if (VerifyRegionStructureList(m1.personalRegionList, m2.personalRegionList) == false) return false;
            if (VerifyKeywordList(m1.personalTagList, m2.personalTagList) == false) return false;

            //Camera
            if (m1.CameraMake != m2.CameraMake) return false;
            if (m1.CameraModel != m2.CameraModel) return false;

            //Media
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.mediaDateTaken, m2.mediaDateTaken)) return false;
            if (m1.mediaWidth != m2.mediaWidth) return false;
            if (m1.mediaHeight != m2.mediaHeight) return false;
            if (m1.mediaOrientation != m2.mediaOrientation) return false;
            if (m1.mediaVideoLength != m2.mediaVideoLength) return false;

            //Location
            if (m1.locationAltitude != m2.locationAltitude) return false;
            if (m1.locationLatitude != m2.locationLatitude) return false;
            if (m1.locationLongitude != m2.locationLongitude) return false;
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.locationDateTime, m2.locationDateTime)) return false;
            if (m1.locationName != m2.locationName) return false;
            if (m1.locationCountry != m2.locationCountry) return false;
            if (m1.locationCity != m2.locationCity) return false;
            if (m1.locationState != m2.locationState) return false;

            return true;

        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            //Broker
            //if (broker != null) hashCode += broker.GetHashCode();

            //File
            if (FileName != null) hashCode += FileName.GetHashCode();
            if (fileDirectory != null) hashCode += fileDirectory.GetHashCode();
            if (FileSize != null) hashCode += FileSize.GetHashCode();
            if (FileDateCreated != null) hashCode += FileDateCreated.GetHashCode();
            if (FileDateModified != null) hashCode += FileDateModified.GetHashCode();
            if (FileLastAccessed != null) hashCode += FileLastAccessed.GetHashCode();
            if (FileMimeType != null) hashCode += FileMimeType.GetHashCode();

            //Personal
            if (PersonalTitle != null) hashCode += PersonalTitle.GetHashCode();
            if (PersonalDescription != null) hashCode += PersonalDescription.GetHashCode();
            if (PersonalComments != null) hashCode += PersonalComments.GetHashCode();
            if (personalRating != null) hashCode += personalRating.GetHashCode();
            if (personalRatingPercent != null) hashCode += personalRatingPercent.GetHashCode();
            if (PersonalAuthor != null) hashCode += PersonalAuthor.GetHashCode();
            if (PersonalAlbum != null) hashCode += PersonalAlbum.GetHashCode();
            //Camera
            if (CameraMake != null) hashCode += CameraMake.GetHashCode();
            if (CameraModel != null) hashCode += CameraModel.GetHashCode();

            //Media
            if (mediaDateTaken != null) hashCode += mediaDateTaken.GetHashCode();
            if (mediaWidth != null) hashCode += mediaWidth.GetHashCode();
            if (mediaHeight != null) hashCode += mediaHeight.GetHashCode();
            if (mediaOrientation != null) hashCode += mediaOrientation.GetHashCode();
            if (mediaVideoLength != null) hashCode += mediaVideoLength.GetHashCode();

            //Location
            if (locationAltitude != null) hashCode += locationAltitude.GetHashCode();
            if (locationLatitude != null) hashCode += locationLatitude.GetHashCode();
            if (locationLongitude != null) hashCode += locationLongitude.GetHashCode();
            if (locationDateTime != null) hashCode += locationDateTime.GetHashCode();
            if (locationName != null) hashCode += locationName.GetHashCode();
            if (locationCountry != null) hashCode += locationCountry.GetHashCode();
            if (locationCity != null) hashCode += locationCity.GetHashCode();
            if (locationState != null) hashCode += locationState.GetHashCode();

            foreach (RegionStructure region in personalRegionList) hashCode += region.GetHashCode();
            foreach (KeywordTag tag in personalTagList) hashCode += tag.GetHashCode();

            return hashCode;
        }

        public static bool operator !=(Metadata m1, Metadata m2)
        {
            return !(m1 == m2);
        }
        #endregion

        #region Properties Helper - LocationCoordinates
        public LocationCoordinate LocationCoordinate
        {
            get
            {
                if (locationLatitude != null && locationLongitude != null)
                    return new LocationCoordinate((float)locationLatitude, (float)locationLongitude);
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    LocationLatitude = value.Latitude;
                    LocationLongitude = value.Longitude;
                }
                else
                {
                    LocationLatitude = null;
                    LocationLongitude = null;
                }
            }

        }
        #endregion

        #region Properties Helper - FileEntryBroker
        private FileEntryBroker fileEntryBroker = null;
        public FileEntryBroker FileEntryBroker
        {
            get
            {
                if (fileEntryBroker == null)
                    fileEntryBroker = new FileEntryBroker(Path.Combine(fileDirectory, FileName), (DateTime)FileDateModified, Broker);
                return fileEntryBroker;
            }
        }
        #endregion


        public static int FindFileEntryInList(List<Metadata> metadataListToCheck, FileEntry fileEntry)
        {
            if (metadataListToCheck == null) return -1;
            if (metadataListToCheck.Count == 0) return -1;

            try
            {
                for (int i = 0; i < metadataListToCheck.Count; i++)
                {
                    Metadata metadata = metadataListToCheck[i];
                    if (metadata != null && metadata.FileFullPath == fileEntry.FullFilePath && metadata.FileDateModified == fileEntry.LastWriteDateTime) return i;
                }
            }
            catch { }
            return -1;
        }

        #region Properties Helper - FindFullFilenameInList 
        public static int FindFullFilenameInList(List<Metadata> metadataListToCheck, string findThis)
        {
            if (metadataListToCheck == null) return -1;
            if (metadataListToCheck.Count == 0) return -1;

            try
            {
                for (int i = 0; i < metadataListToCheck.Count; i++)
                {
                    Metadata metadata = metadataListToCheck[i];
                    if (metadata != null && metadata.FileFullPath == findThis) return i;
                }
            } catch { }
            return -1;
        }

        public static int FindFullFilenameInList(List<Metadata> metadataListToCheck, Metadata findThis)
        {            
            return FindFullFilenameInList(metadataListToCheck, findThis.FileFullPath);
        }
        #endregion

        #region Properties Helper - IsFullFilenameInList
        public static bool IsFullFilenameInList(List<Metadata> queueSaveMetadataUpdatedByUser, string fullFilePath)
        {
            return FindFullFilenameInList(queueSaveMetadataUpdatedByUser, fullFilePath) >= 0;
        }
        #endregion 

        #region Errors
        private static string AddError(string text, object o1, object o2)
        {
            return text + ": '" + (o1 == null ? "" : o1.ToString()) + "' vs. '" + (o2 == null ? "" : o2.ToString()) + "'\r\n";
        }

        public static string GetErrors(Metadata m1, Metadata m2)
        {
            string errors = "";
            if (m1 is null && m2 is null) return "";
            if (m1 is null) return "Can't compare missing metadata for file 1.\r\n";
            if (m2 is null) return "Can't compare missing metadata for file 2.\r\n";
            if (ReferenceEquals(m1, m2)) return "";
            if (m1 == m2) return "";
            //if (m1.GetHashCode() == m2.GetHashCode()) return "";

            //File
            if (m1.Broker != m2.Broker) errors += "Broker\r\n";
            if (m1.FileName != m2.FileName) errors += AddError("File name", m1.FileName, m2.FileName);
            if (m1.fileDirectory != m2.fileDirectory) errors += AddError("File direcotry", m1.fileDirectory, m2.fileDirectory);
            if (m1.FileSize != m2.FileSize) errors += AddError("File size", m1.FileSize, m2.FileSize);
            if (m1.FileDateCreated != m2.FileDateCreated) errors += AddError("File Date Created", m1.FileDateCreated, m2.FileDateCreated);
            if (m1.FileDateModified != m2.FileDateModified) errors += AddError("File Date Modified", m1.FileDateModified, m2.FileDateModified);
            if (m1.FileLastAccessed != m2.FileLastAccessed) errors += AddError("File Last Accessed", m1.FileLastAccessed, m2.FileLastAccessed);
            if (m1.FileMimeType != m2.FileMimeType) errors += AddError("FileMimeType", m1.FileMimeType, m2.FileMimeType);

            //Personal
            if (m1.PersonalTitle != m2.PersonalTitle) errors += AddError("Title", m1.PersonalTitle, m2.PersonalTitle);
            if (m1.PersonalDescription != m2.PersonalDescription) errors += AddError("Description", m1.PersonalDescription, m2.PersonalDescription);
            if (m1.PersonalComments != m2.PersonalComments) errors += AddError("Comments", m1.PersonalComments, m2.PersonalComments);
            if (m1.personalRating != m2.personalRating) errors += AddError("Rating", m1.personalRating, m2.personalRating);
            if (m1.personalRatingPercent != m2.personalRatingPercent) errors += AddError("Rating Percent", m1.personalRatingPercent, m2.personalRatingPercent);
            if (m1.PersonalAuthor != m2.PersonalAuthor) errors += AddError("Author", m1.PersonalAuthor, m2.PersonalAuthor);
            if (m1.PersonalAlbum != m2.PersonalAlbum) errors += AddError("Album", m1.PersonalAlbum, m2.PersonalAlbum);

            //Camera
            if (m1.CameraMake != m2.CameraMake) return errors += AddError("Camera Make", m1.CameraMake, m2.CameraMake);
            if (m1.CameraModel != m2.CameraModel) return errors += AddError("Camra Model", m1.CameraModel, m2.CameraModel);

            //Media
            if (m1.mediaDateTaken != m2.mediaDateTaken) errors += AddError("Media DateTaken", m1.mediaDateTaken, m2.mediaDateTaken);
            if (m1.mediaWidth != m2.mediaWidth) errors += AddError("Media Width", m1.mediaWidth, m2.mediaWidth);
            if (m1.mediaHeight != m2.mediaHeight) errors += AddError("Media Hieght", m1.mediaHeight, m2.mediaHeight);
            if (m1.mediaOrientation != m2.mediaOrientation) errors += AddError("Media Orientation", m1.mediaOrientation, m2.mediaOrientation);
            if (m1.mediaVideoLength != m2.mediaVideoLength) errors += AddError("Media Video lenth", m1.mediaVideoLength, m2.mediaVideoLength);

            //Location
            if (m1.locationAltitude != m2.locationAltitude) errors += AddError("Location Altitude", m1.locationAltitude, m2.locationAltitude);
            if (m1.locationLatitude != m2.locationLatitude) errors += AddError("Location Latitude", m1.locationLatitude, m2.locationLatitude);
            if (m1.locationLongitude != m2.locationLongitude) errors += AddError("Location Longitude", m1.locationLongitude, m2.locationLongitude);
            if (m1.locationDateTime != m2.locationDateTime) errors += AddError("Location DateTime", m1.locationDateTime, m2.locationDateTime);
            if (m1.locationName != m2.locationName) errors += AddError("Location Name", m1.locationName, m2.locationName);
            if (m1.locationCountry != m2.locationCountry) errors += AddError("Location Country", m1.locationCountry, m2.locationCountry);
            if (m1.locationCity != m2.locationCity) errors += AddError("Location District", m1.locationCity, m2.locationCity);
            if (m1.locationState != m2.locationState) errors += AddError("Location Region", m1.locationState, m2.locationState);

            if (VerifyRegionStructureList(m1.personalRegionList, m2.personalRegionList) == false)
            {
                List<RegionStructure> allRegions = new List<RegionStructure>();
                foreach (RegionStructure region in m1.personalRegionList) if (!region.DoesThisRectangleAndNameExistInList(allRegions)) allRegions.Add(region);
                foreach (RegionStructure region in m2.personalRegionList) if (!region.DoesThisRectangleAndNameExistInList(allRegions)) allRegions.Add(region);

                errors += "\r\nRegion list\r\n";
                foreach (RegionStructure region in allRegions)
                    errors += "" + (region == null ? "" : region.ToErrorText()) +
                        (region.DoesThisRectangleAndNameExistInList(m1.personalRegionList) == (region.DoesThisRectangleAndNameExistInList(m2.personalRegionList)) ? " Verified" :
                        (region.DoesThisRectangleAndNameExistInList(m1.personalRegionList) ? " Not added" : " Not removed")) +
                        "\r\n";
            }

            if (VerifyKeywordList(m1.personalTagList, m2.personalTagList) == false)
            {

                List<KeywordTag> allTags = new List<KeywordTag>();
                foreach (KeywordTag tag in m1.PersonalKeywordTags) if (!allTags.Contains(tag)) allTags.Add(tag);
                foreach (KeywordTag tag in m2.PersonalKeywordTags) if (!allTags.Contains(tag)) allTags.Add(tag);

                errors += "\r\nKeyword list\r\n";
                foreach (KeywordTag tag in allTags)
                    errors += "" + (tag == null ? "" : tag.ToString()) +
                        (m1.PersonalKeywordTags.Contains(tag) == m2.PersonalKeywordTags.Contains(tag) ? " Verified OK" :
                        (m1.PersonalKeywordTags.Contains(tag) ? " Not add" : " Not removed")) +
                        "\r\n";
            }

            return errors;
        }

        public string Errors { get => errors; set => errors = value; }
        #endregion

        #region Properties MetadataBrokerTypes
        public MetadataBrokerTypes Broker { get; set; }
        #endregion

        #region Properties File
        public String FileName { get; set; }
        public String FileDirectory
        {
            get { return fileDirectory; }
            set
            {
                fileDirectory = null;
                if (value != null) fileDirectory = value.Replace("/", "\\"); //Convert to Windows format; 
            }
        }

        public String FileFullPath { get => Path.Combine(fileDirectory, FileName); }
        public Int64? FileSize { get; set; }
        public DateTime? FileDateCreated { get; set; }
        public DateTime? FileDateModified { get; set; }
        public DateTime? FileLastAccessed { get; set; }
        public string FileMimeType { get; set; }
        #endregion

        #region Properties Personal
        public String PersonalTitle { get; set; }
        public String PersonalDescription { get; set; }
        public string PersonalComments { get; set; }

        static public byte ConvertRatingStarsToRatingPercent(byte personalRating)
        {
            switch (personalRating)
            {
                case 1:
                    return 1;
                case 2:
                    return 25;
                case 3:
                    return 50;
                case 4:
                    return 75;
                case 5:
                    return 99;
                default:
                    return 0;
            }
        }

        public byte? PersonalRating
        {
            get { return personalRating; }
            set
            {
                personalRating = value;
                if (personalRating != null)
                {
                    if (personalRatingPercent != null)
                    {
                        //Don't change e.g. 89% to 75%
                        if (ConvertRatingPercentToRetingStars((byte)personalRatingPercent) != personalRating)
                            personalRatingPercent = ConvertRatingStarsToRatingPercent((byte)personalRating);
                    }
                    else personalRatingPercent = ConvertRatingStarsToRatingPercent((byte)personalRating);
                }
                else
                    personalRatingPercent = null;
            }
        }

        private byte ConvertRatingPercentToRetingStars(byte personalRatingPercent)
        {
            if (personalRatingPercent > 87)
            {
                return 5;
            }
            else if (personalRatingPercent > 62 && personalRatingPercent <= 87)
            {
                return 4;
            }
            else if (personalRatingPercent > 38 && personalRatingPercent <= 62)
            {
                return 3;
            }
            else if (personalRatingPercent > 12 && personalRatingPercent <= 37)
            {
                return 2;
            }
            else if (personalRatingPercent >= 1 && personalRatingPercent <= 12)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public byte? PersonalRatingPercent
        {
            get
            {
                return personalRatingPercent;
            }
            set
            {
                personalRatingPercent = value;
                if (personalRatingPercent != null)
                {
                    personalRating = ConvertRatingPercentToRetingStars((byte)personalRatingPercent);
                }
                else
                    personalRating = null;
            }
        }

        public String PersonalAuthor { get; set; }
        public string PersonalAlbum { get; set; }
        public List<RegionStructure> PersonalRegionList
        {
            get => personalRegionList;
            //set => personalRegionList = value; 
        }
        public void PersonalRegionListAddIfNotExists(RegionStructure regionStructure)
        {
            if (!personalRegionList.Contains(regionStructure))
                personalRegionList.Add(regionStructure);
        }

        public void PersonalRegionListAddIfNotAreaAndNameExists(RegionStructure regionStructure)
        {
            if (!regionStructure.DoesThisRectangleAndNameExistInList(personalRegionList)) personalRegionList.Add(regionStructure);
        }
        

        public void PersonalRegionListAddIfNameNotExists(RegionStructure regionStructure)
        {
            if (!regionStructure.DoesThisNameExistInList(personalRegionList)) personalRegionList.Add(regionStructure);
        }

        public List<KeywordTag> PersonalKeywordTags
        {
            get => personalTagList;
            //set => personalTagList = value; 
        }

        public void PersonalKeywordTagsAddIfNotExists(KeywordTag keywordTag)
        {
            if (!personalTagList.Contains(keywordTag)) personalTagList.Add(keywordTag);
        }

        public void PersonalTagListUpdateImage(RegionStructure updateRegion, Image thumbnail)
        {
            for (int i = 0; i < personalRegionList.Count; i++)
            {
                if (personalRegionList[i] == updateRegion)
                {
                    RegionStructure region = personalRegionList[i];
                    region.Thumbnail = thumbnail;
                    personalRegionList.RemoveAt(i);
                    personalRegionList.Insert(i, region);
                    break;
                }
            }
        }

        public Image PersonalTagListGetThumbnail(RegionStructure region)
        {
            if (personalRegionList.Contains(region))
            {
                return personalRegionList[personalRegionList.IndexOf(region)].Thumbnail;
            }
            return null;
        }
        #endregion

        #region Properties Camera
        public String CameraMake { get; set; }
        public String CameraModel { get; set; }
        #endregion

        #region Properties Media
        public DateTime? MediaDateTaken { get => mediaDateTaken; set => mediaDateTaken = value; }
        public Int32? MediaWidth { get => mediaWidth; set => mediaWidth = value; }
        public Int32? MediaHeight { get => mediaHeight; set => mediaHeight = value; }
        public Size MediaSize { get => new Size(mediaWidth == null ? 0 : (int)mediaWidth, mediaWidth == null ? 0 : (int)mediaHeight); }
        public int? MediaOrientation { get => mediaOrientation; set => mediaOrientation = value; }
        public int? MediaVideoLength { get => mediaVideoLength; set => mediaVideoLength = value; }
        #endregion 

        #region Properties Location
        public float? LocationAltitude
        {
            get => locationAltitude;
            set => locationAltitude = (value == null ? (float?)null : (float?)Math.Round((float)value, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimalsShort));
        }
        public float? LocationLatitude
        {
            get => locationLatitude;
            set => locationLatitude = (value == null ? (float?)null : (float?)Math.Round((float)value, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals));
        }
        public float? LocationLongitude
        {
            get => locationLongitude;
            set => locationLongitude = (value == null ? (float?)null : (float?)Math.Round((float)value, SqliteDatabase.SqliteDatabaseUtilities.NumberOfDecimals));
        }
        public DateTime? LocationDateTime { get => locationDateTime; set => locationDateTime = value; }
        public String LocationName { get => locationName; set => locationName = value; }
        public string LocationCountry { get => locationCountry; set => locationCountry = value; }
        public string LocationCity { get => locationCity; set => locationCity = value; }
        public string LocationState { get => locationState; set => locationState = value; }
        #endregion


        #region Vaiable Properties
        private static string[] arrayOfProperties = null;

        public static string[] ListOfProperties()
        {
            if (arrayOfProperties == null)
            {
                List<string> listOfProperties = new List<string>();

                //System
                listOfProperties.Add("{SystemDateTime}");
                listOfProperties.Add("{SystenDateTimeDateStamp}");
                listOfProperties.Add("{SystenDateTimeTimeStamp}");
                listOfProperties.Add("{SystemDateTime_yyyy}");
                listOfProperties.Add("{SystemDateTime_MM}");
                listOfProperties.Add("{SystemDateTime_dd}");
                listOfProperties.Add("{SystemDateTime_HH}");
                listOfProperties.Add("{SystemDateTime_mm}");
                listOfProperties.Add("{SystemDateTime_ss}");

                //Filesystem
                listOfProperties.Add("{FileName}");
                listOfProperties.Add("{FileNameWithoutExtension}");
                listOfProperties.Add("{FileNameWithoutDateTime}");
                listOfProperties.Add("{FileExtension}");
                listOfProperties.Add("{FileDirectory}");
                listOfProperties.Add("{FileSize}");
                
                listOfProperties.Add("{FileDateCreated}");
                listOfProperties.Add("{FileDateCreatedDateStamp}");
                listOfProperties.Add("{FileDateCreatedTimeStamp}");
                listOfProperties.Add("{FileDateCreated_yyyy}");
                listOfProperties.Add("{FileDateCreated_MM}");
                listOfProperties.Add("{FileDateCreated_dd}");
                listOfProperties.Add("{FileDateCreated_HH}");
                listOfProperties.Add("{FileDateCreated_mm}");
                listOfProperties.Add("{FileDateCreated_ss}");

                listOfProperties.Add("{FileDateModified}"); 
                listOfProperties.Add("{IfFileDateModifiedChanged}");

                listOfProperties.Add("{FileDateModifiedDateStamp}");
                listOfProperties.Add("{FileDateModifiedTimeStamp}");
                listOfProperties.Add("{FileDateModified_yyyy}");
                listOfProperties.Add("{FileDateModified_MM}");
                listOfProperties.Add("{FileDateModified_dd}");
                listOfProperties.Add("{FileDateModified_HH}");
                listOfProperties.Add("{FileDateModified_mm}");
                listOfProperties.Add("{FileDateModified_ss}");
                listOfProperties.Add("{FileLastAccessed}");
                listOfProperties.Add("{FileLastAccessedDateStamp}");
                listOfProperties.Add("{FileLastAccessedTimeStamp}");
                listOfProperties.Add("{FileLastAccessed_yyyy}");
                listOfProperties.Add("{FileLastAccessed_MM}");
                listOfProperties.Add("{FileLastAccessed_dd}");
                listOfProperties.Add("{FileLastAccessed_HH}");
                listOfProperties.Add("{FileLastAccessed_mm}");
                listOfProperties.Add("{FileLastAccessed_ss}");

                listOfProperties.Add("{FileMimeType}");

                //Personal
                listOfProperties.Add("{PersonalTitle}");
                listOfProperties.Add("{IfPersonalTitleChanged}");

                listOfProperties.Add("{PersonalDescription}");
                listOfProperties.Add("{IfPersonalDescriptionChanged}");

                listOfProperties.Add("{PersonalComments}");
                listOfProperties.Add("{IfPersonalCommentsChanged}");
                
                listOfProperties.Add("{PersonalRating}");
                listOfProperties.Add("{IfPersonalRatingChanged}");
                listOfProperties.Add("{PersonalRatingPercent}");

                listOfProperties.Add("{PersonalAuthor}");
                listOfProperties.Add("{IfPersonalAuthorChanged}");

                listOfProperties.Add("{PersonalAlbum}");
                listOfProperties.Add("{IfPersonalAlbumChanged}");

                //Region
                listOfProperties.Add("{PersonalRegionInfoMP}");
                listOfProperties.Add("{PersonalRegionInfo}");
                listOfProperties.Add("{IfPersonalRegionChanged}");

                //Keyword
                listOfProperties.Add("{PersonalKeywordsList}");
                listOfProperties.Add("{PersonalKeywordsXML}");
                listOfProperties.Add("{PersonalKeywordItems}");
                listOfProperties.Add("{IfPersonalKeywordsChanged}");

                //Camera
                listOfProperties.Add("{CameraMake}");
                listOfProperties.Add("{CameraModel}");

                //Media
                listOfProperties.Add("{MediaDateTaken}");
                listOfProperties.Add("{IfMediaDateTakenChanged}");

                listOfProperties.Add("{MediaDateTakenDateStamp}");
                listOfProperties.Add("{MediaDateTakenTimeStamp}");
                listOfProperties.Add("{MediaDateTaken_yyyy}");
                listOfProperties.Add("{MediaDateTaken_MM}");
                listOfProperties.Add("{MediaDateTaken_dd}");
                listOfProperties.Add("{MediaDateTaken_HH}");
                listOfProperties.Add("{MediaDateTaken_mm}");
                listOfProperties.Add("{MediaDateTaken_ss}");

                listOfProperties.Add("{MediaWidth}");
                listOfProperties.Add("{MediaHeight}");
                listOfProperties.Add("{MediaOrientation}");
                listOfProperties.Add("{MediaVideoLength}");

                //Location
                listOfProperties.Add("{LocationAltitude}");
                listOfProperties.Add("{IfLocationAltitudeChanged}");
                
                listOfProperties.Add("{LocationLatitude}");
                listOfProperties.Add("{IfLocationLatitudeChanged}");

                listOfProperties.Add("{LocationLongitude}");
                listOfProperties.Add("{IfLocationLongitudeChanged}");

                listOfProperties.Add("{LocationDateTime}");
                listOfProperties.Add("{IfLocationDateTimeChanged}");
                
                listOfProperties.Add("{LocationDateTimeUTC}");
                listOfProperties.Add("{LocationDateTimeDateStamp}");
                listOfProperties.Add("{LocationDateTimeTimeStamp}");
                listOfProperties.Add("{LocationDateTime_yyyy}");
                listOfProperties.Add("{LocationDateTime_MM}");
                listOfProperties.Add("{LocationDateTime_dd}");
                listOfProperties.Add("{LocationDateTime_HH}");
                listOfProperties.Add("{LocationDateTime_mm}");
                listOfProperties.Add("{LocationDateTime_ss}");

                listOfProperties.Add("{LocationName}");
                listOfProperties.Add("{IfLocationNameChanged}");

                listOfProperties.Add("{LocationCity}");
                listOfProperties.Add("{IfLocationCityChanged}");

                listOfProperties.Add("{LocationState}");
                listOfProperties.Add("{IfLocationStateChanged}");

                listOfProperties.Add("{LocationCountry}");
                listOfProperties.Add("{IfLocationCountryChanged}");
                //listOfProperties.Add("{LocationDistrict}");
                //listOfProperties.Add("{LocationRegion}");

                arrayOfProperties = listOfProperties.ToArray();
            }
            return arrayOfProperties;
        }

        public string GetPropertyValue(string variableName, bool useExifFormat, bool convertNullToBlank,
            List<string> allowedFileNameDateTimeFormats, 
            string personalRegionInfoMP, string personalRegionInfo, string personalKeywordList, string personalKeywordsXML, string personalKeywordItems)
        {
            string result = variableName;
            DateTime dateTimeSystem = DateTime.Now;
            FileDateTimeReader fileDateTimeFormats = new FileDateTimeReader(allowedFileNameDateTimeFormats);
            switch (variableName)
            {
                #region System
                case "{SystemDateTime}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(dateTimeSystem);
                    else result = TimeZoneLibrary.ToStringFilename(dateTimeSystem); 
                    break;
                case "{SystemDateTimeDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(dateTimeSystem);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(dateTimeSystem);
                    break;
                case "{SystemDateTimeTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(dateTimeSystem);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(dateTimeSystem);
                    break;
                case "{SystemDateTime_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(dateTimeSystem);
                    break;
                case "{SystemDateTime_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(dateTimeSystem);
                    break;
                case "{SystemDateTime_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(dateTimeSystem);
                    break;
                case "{SystemDateTime_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(dateTimeSystem);
                    break;
                case "{SystemDateTime_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(dateTimeSystem);
                    break;
                case "{SystemDateTime_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(dateTimeSystem);
                    break;
                #endregion 

                #region Filesystem
                case "{FileName}":
                    result = FileName; 
                    break;
                case "{FileNameWithoutExtension}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    break;
                case "{FileNameWithoutDateTime}":
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(FileName));
                    break;
                case "{FileExtension}":
                    result = Path.GetExtension(FileName);
                    break;
                case "{FileDirectory}":
                    result = FileDirectory;
                    break;
                case "{FileSize}": 
                    result = FileSize == null ? null : ((long)FileSize).ToString(CultureInfo.InvariantCulture); 
                    break;
                case "{FileDateCreated}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(FileDateCreated);
                    else result = TimeZoneLibrary.ToStringFilename(FileDateCreated);
                    break;
                case "{FileDateCreatedDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(FileDateCreated);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(FileDateCreated);
                    break;
                case "{FileDateCreatedTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(FileDateCreated);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(FileDateCreated);
                    break;
                case "{FileDateCreated_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(FileDateCreated);
                    break;
                case "{FileDateCreated_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(FileDateCreated); 
                    break;
                case "{FileDateCreated_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(FileDateCreated); 
                    break;
                case "{FileDateCreated_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(FileDateCreated); 
                    break;
                case "{FileDateCreated_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(FileDateCreated); 
                    break;
                case "{FileDateCreated_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(FileDateCreated); 
                    break;
                case "{FileDateModified}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(FileDateModified);
                    else result = TimeZoneLibrary.ToStringFilename(FileDateModified);
                    break;
                case "{FileDateModifiedDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(FileDateModified);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(FileDateModified);
                    break;
                case "{FileDateModifiedTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(FileDateModified);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(FileDateModified);
                    break;
                case "{FileDateModified_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(FileDateModified);
                    break;
                case "{FileDateModified_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(FileDateModified);
                    break;
                case "{FileDateModified_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(FileDateModified);
                    break;
                case "{FileDateModified_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(FileDateModified);
                    break;
                case "{FileDateModified_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(FileDateModified);
                    break;
                case "{FileDateModified_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(FileDateModified);
                    break;
                case "{FileLastAccessed}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(FileLastAccessed);
                    else result = TimeZoneLibrary.ToStringFilename(FileLastAccessed);
                    break;
                case "{FileLastAccessedDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(FileLastAccessed);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(FileLastAccessed);
                    break;
                case "{FileLastAccessedTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(FileLastAccessed);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(FileLastAccessed);
                    break;
                case "{FileLastAccessed_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(FileLastAccessed);
                    break;
                case "{FileLastAccessed_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(FileLastAccessed);
                    break;
                case "{FileLastAccessed_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(FileLastAccessed);
                    break;
                case "{FileLastAccessed_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(FileLastAccessed);
                    break;
                case "{FileLastAccessed_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(FileLastAccessed);
                    break;
                case "{FileLastAccessed_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(FileLastAccessed);
                    break;
                case "{FileMimeType}":
                    result = FileMimeType;
                    break;
                #endregion

                #region Personal
                case "{PersonalTitle}":
                    result = PersonalTitle;
                    break;
                case "{PersonalDescription}":
                    result = PersonalDescription;
                    break;
                case "{PersonalComments}":
                    result = PersonalComments;
                    break;
                case "{PersonalRating}":
                    result = PersonalRating == null ? null : ((byte)PersonalRating).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{PersonalRatingPercent}":
                    result = PersonalRatingPercent == null ? null : ((byte)PersonalRatingPercent).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{PersonalAuthor}":
                    result = PersonalAuthor;
                    break;
                case "{PersonalAlbum}":
                    result = PersonalAlbum;
                    break;
                #endregion

                #region Face Region
                case "{PersonalRegionInfoMP}":
                    result = personalRegionInfoMP;
                    break;
                case "{PersonalRegionInfo}":
                    result = personalRegionInfo;
                    break;

                //Keyword
                case "{PersonalKeywordsList}":
                    result = personalKeywordList;
                    break;
                case "{PersonalKeywordsXML}":
                    result = personalKeywordsXML;
                    break;
                case "{PersonalKeywordsItems}":
                    result = personalKeywordItems;
                    break;
                #endregion

                #region Camera
                case "{CameraMake}":
                    result = CameraMake;
                    break;
                case "{CameraModel}":
                    result = CameraModel;
                    break;
                #endregion

                #region Media
                case "{MediaDateTaken}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(MediaDateTaken);
                    else result = TimeZoneLibrary.ToStringFilename(MediaDateTaken);
                    break;
                case "{MediaDateTakenDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(MediaDateTaken);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(MediaDateTaken);
                    break;
                case "{MediaDateTakenTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(MediaDateTaken);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(MediaDateTaken);
                    break;
                case "{MediaDateTaken_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(MediaDateTaken);
                    break;
                case "{MediaDateTaken_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(MediaDateTaken);
                    break;
                case "{MediaDateTaken_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(MediaDateTaken);
                    break;
                case "{MediaDateTaken_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(MediaDateTaken);
                    break;
                case "{MediaDateTaken_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(MediaDateTaken);
                    break;
                case "{MediaDateTaken_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(MediaDateTaken);
                    break;
                case "{MediaWidth}": 
                    result = MediaWidth == null ? null : ((int)MediaWidth).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{MediaHeight}":
                    result = MediaHeight == null ? null : ((int)MediaHeight).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{MediaOrientation}":
                    result = MediaOrientation == null ? null : ((int)MediaOrientation).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{MediaVideoLength}":
                    result = MediaVideoLength == null ? null : ((int)MediaVideoLength).ToString(CultureInfo.InvariantCulture);
                    break;
                #endregion

                #region Location
                case "{LocationAltitude}":
                    result = LocationAltitude == null ? null : ((float)LocationAltitude).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{LocationLatitude}":
                    result = LocationLatitude == null ? null : ((float)LocationLatitude).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{LocationLongitude}":
                    result = LocationLongitude == null ? null : ((float)LocationLongitude).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{LocationDateTime}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(LocationDateTime);
                    else result = TimeZoneLibrary.ToStringFilename(LocationDateTime);
                    break;
                case "{LocationDateTimeUTC}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolUTC(LocationDateTime);
                    else result = TimeZoneLibrary.ToStringFilenameUTC(LocationDateTime);
                    break;
                case "{LocationDateTimeDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(LocationDateTime);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(LocationDateTime);                    
                    break;
                case "{LocationDateTimeTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(LocationDateTime);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(LocationDateTime);
                    break;            
                case "{LocationDateTime_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(LocationDateTime);
                    break;
                case "{LocationDateTime_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(LocationDateTime);
                    break;
                case "{LocationDateTime_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(LocationDateTime);
                    break;
                case "{LocationDateTime_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(LocationDateTime);
                    break;
                case "{LocationDateTime_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(LocationDateTime);
                    break;
                case "{LocationDateTime_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(LocationDateTime);
                    break;                
                case "{LocationName}":
                    result = LocationName;                     
                    break;
                case "{LocationCountry}":
                    result = LocationCountry; 
                    break;
                case "{LocationState}":
                    result = LocationState; 
                    break;
                case "{LocationCity}":
                    result = LocationCity; 
                    break;
                #endregion 
            }
            if (convertNullToBlank && result == null) result = "";
            return result;
        }

        #endregion

        #region Replace Variable with Propertiy values
        private bool HasValueChanged(string variable, Metadata metadata)
        {           
            bool result = false;
            switch (variable)
            {
                //System
                case "{IfFileDateModifiedChanged}":
                    if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(this.FileLastAccessed, metadata.FileLastAccessed)) result = true;
                    break;

                //Personal
                case "{IfPersonalTitleChanged}":
                    if (this.PersonalTitle != metadata.PersonalTitle) result = true;
                    break;
                case "{IfPersonalDescriptionChanged}":
                    if (this.PersonalDescription != metadata.PersonalDescription) result = true;
                    break;
                case "{IfPersonalCommentsChanged}":
                    if (this.PersonalComments != metadata.PersonalComments) result = true;
                    break;
                case "{IfPersonalRatingChanged}":
                    if (this.personalRating != metadata.personalRating) result = true;
                    if (this.personalRatingPercent != metadata.personalRatingPercent) result = true;
                    break;
                case "{IfPersonalAuthorChanged}":
                    if (this.PersonalAuthor != metadata.PersonalAuthor) result = true;
                    break;
                case "{IfPersonalAlbumChanged}":
                    if (this.PersonalAlbum != metadata.PersonalAlbum) result = true;
                    break;
                //Region
                case "{IfPersonalRegionChanged}":
                    if (VerifyRegionStructureList(this.personalRegionList, metadata.personalRegionList) == false) result = true;                    
                    break;
                //Keyword
                case "{IfPersonalKeywordsChanged}":
                    if (VerifyKeywordList(this.personalTagList, metadata.personalTagList) == false) result = true;
                    break;
                //Media
                case "{IfMediaDateTakenChanged}":
                    if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(this.MediaDateTaken, metadata.MediaDateTaken)) result = true;
                    break;
                //Location
                case "{IfLocationAltitudeChanged}":
                    if (this.locationAltitude != metadata.locationAltitude) result = true;
                    break;
                case "{IfLocationLatitudeChanged}":
                    if (this.locationLatitude != metadata.locationLatitude) result = true;
                    break;
                case "{IfLocationLongitudeChanged}":
                    if (this.locationLongitude != metadata.locationLongitude) result = true;
                    break;
                case "{IfLocationDateTimeChanged}":
                    if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(this.locationDateTime, metadata.locationDateTime)) result = true;
                    break;
                case "{IfLocationNameChanged}":
                    if (this.locationName != metadata.locationName) result = true;
                    break;
                case "{IfLocationCountryChanged}":
                    if (this.locationCountry != metadata.locationCountry) result = true;
                    break;
                case "{IfLocationCityChanged}":
                    if (this.locationCity != metadata.locationCity) result = true;
                    break;
                case "{IfLocationStateChanged}":
                    if (this.locationState != metadata.locationState) result = true;
                    break;
            }
            return result;
        }

        private string AddLine(string line, string variable, bool alwaysWrite, Metadata metadata, ref bool vaiableFound)
        {
            if (line.Contains(variable))
            {
                vaiableFound = true;
                line = line.Replace(variable, "");
                //If always write, then line will be added
                //If not always write, then check if vaiable is changed
                if (!alwaysWrite && !HasValueChanged(variable, metadata)) line = ""; 
            }

            return line;
        }

        public string RemoveLines(string stringWithVariables, Metadata metadata, bool alwaysWrite)
        {
            string result = "";
            string addLine;
            string[] lines = stringWithVariables.Replace("\r\n", "\n").Split('\n');
            
            foreach (string line in lines)
            {
                addLine = line;
                bool vaiableFound = false;
                do
                {
                    vaiableFound = false;
                    //File
                    addLine = AddLine(addLine, "{IfFileDateModifiedChanged}", alwaysWrite, metadata, ref vaiableFound);

                    //Personal
                    addLine = AddLine(addLine, "{IfPersonalTitleChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfPersonalDescriptionChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfPersonalCommentsChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfPersonalRatingChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfPersonalAuthorChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfPersonalAlbumChanged}", alwaysWrite, metadata, ref vaiableFound);

                    //Region
                    addLine = AddLine(addLine, "{IfPersonalRegionChanged}", alwaysWrite, metadata, ref vaiableFound);

                    //Keyword
                    addLine = AddLine(addLine, "{IfPersonalKeywordsChanged}", alwaysWrite, metadata, ref vaiableFound);
                    //Media
                    addLine = AddLine(addLine, "{IfMediaDateTakenChanged}", alwaysWrite, metadata, ref vaiableFound);

                    //Location
                    addLine = AddLine(addLine, "{IfLocationAltitudeChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfLocationLatitudeChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfLocationLongitudeChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfLocationDateTimeChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfLocationNameChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfLocationCityChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfLocationStateChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = AddLine(addLine, "{IfLocationCountryChanged}", alwaysWrite, metadata, ref vaiableFound);
                } while (vaiableFound);
                if (!string.IsNullOrWhiteSpace(addLine)) result +=  addLine + "\r\n";
            }
            return result;
        }

        public string ReplaceVariables(string stringWithVariables, bool useExifFormat, bool convertNullToBlank, List<string> allowedFileNameDateTimeFormats,
            string personalRegionInfoMP, string personalRegionInfo, string personalKeywordList, string personalKeywordsXML, string personalKeywordItems)
        {
            string result = stringWithVariables;
            string[] variables = Metadata.ListOfProperties();
            foreach (string variable in variables)
            {
                while (result.Contains(variable)) result = result.Replace(variable, GetPropertyValue(variable, useExifFormat, convertNullToBlank,
                    allowedFileNameDateTimeFormats, personalRegionInfoMP, personalRegionInfo, personalKeywordList, personalKeywordsXML, personalKeywordItems));
            }

            return result;
        }

        public string ReplaceKeywordItemVariables(string stringWithVariables, string keyword)
        {
            string result = stringWithVariables;
            while (result.Contains("{KeywordItem}")) result = result.Replace("{KeywordItem}", keyword);            
            return result;
        }
        #endregion 
    }
}

