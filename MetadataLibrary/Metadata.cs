using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;

namespace MetadataLibrary
{

    [Serializable]
    public class Metadata
    {
        //Fields size https://owl.phy.queensu.ca/~phil/exiftool/TagNames.pdf

        public String errors = "";
        private String fileDirectory;
        private Byte? personalRating;               
        private Single? personalRatingPercent;
        private List<RegionStructure> personalRegionList = new List<RegionStructure>();
        private List<KeywordTag> personalTagList = new List<KeywordTag>();

        //Media
        private DateTime? mediaDateTaken;           
        private Int32? mediaWidth;           
        private Int32? mediaHeight;            
        private Int32? mediaOrientation;
        private Int32? mediaVideoLength;

        //Location
        private Double? locationAltitude;           
        private Double? locationLatitude;           
        private Double? locationLongitude;          
        private DateTime? locationDateTime;         
        private String locationName;
        private String locationCountry;
        private String locationDistrict;
        private String locationRegion;

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
            mediaDateTaken = metadata.mediaDateTaken;
            mediaWidth = metadata.mediaWidth;
            mediaHeight = metadata.mediaHeight;
            mediaOrientation = metadata.mediaOrientation;
            mediaVideoLength = metadata.mediaVideoLength;

            //Location
            locationAltitude = metadata.locationAltitude;
            locationLatitude = metadata.locationLatitude;
            locationLongitude = metadata.locationLongitude;
            locationDateTime = metadata.locationDateTime;
            locationName = metadata.locationName;
            locationCountry = metadata.locationCountry;
            locationDistrict = metadata.locationDistrict;
            locationRegion = metadata.locationRegion;

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
            if (m1.FileDateCreated != m2.FileDateCreated) return false;
            if (m1.FileDateModified != m2.FileDateModified) return false;
            if (m1.FileLastAccessed != m2.FileLastAccessed) return false;
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
            if (m1.mediaDateTaken != m2.mediaDateTaken) return false;
            if (m1.mediaWidth != m2.mediaWidth) return false;
            if (m1.mediaHeight != m2.mediaHeight) return false;
            if (m1.mediaOrientation != m2.mediaOrientation) return false;
            if (m1.mediaVideoLength != m2.mediaVideoLength) return false;

            //Location
            if (m1.locationAltitude != m2.locationAltitude) return false;
            if (m1.locationLatitude != m2.locationLatitude) return false;
            if (m1.locationLongitude != m2.locationLongitude) return false;
            if (m1.locationDateTime != m2.locationDateTime) return false;
            if (m1.locationName != m2.locationName) return false;
            if (m1.locationCountry != m2.locationCountry) return false;
            if (m1.locationDistrict != m2.locationDistrict) return false;
            if (m1.locationRegion != m2.locationRegion) return false;

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
            if (locationDistrict != null) hashCode += locationDistrict.GetHashCode();
            if (locationRegion != null) hashCode += locationRegion.GetHashCode();

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
                    return new LocationCoordinate((double)locationLatitude, (double)locationLongitude);
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    locationLatitude = value.Latitude;
                    locationLongitude = value.Longitude;
                }
                else
                {
                    locationLatitude = null;
                    locationLongitude = null;
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

        #region Properties Helper - FindMetadataInList of Metadatas using full filename
        public static int FindMetadataInList(List<Metadata> metadataListToCheck, Metadata findThis)
        {
            if (metadataListToCheck == null) return -1;
            if (metadataListToCheck.Count == 0) return -1;

            for (int i = 0; i < metadataListToCheck.Count; i++)
            {
                Metadata metadata = metadataListToCheck[i];
                if (metadata != null &&
                    metadata.FileName == findThis.FileName &&
                    metadata.FileDirectory == findThis.FileDirectory)
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion

        #region Properties Helper - IsFullFilePathInList
        public static bool IsFullFilePathInList(List<Metadata> queueSaveMetadataUpdatedByUser, string fullFilePath)
        {
            try
            {
                foreach (Metadata metatdata in queueSaveMetadataUpdatedByUser)
                {
                    if (metatdata.FileFullPath == fullFilePath) return true;
                }
            }
            catch { }
            return false;
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
            if (m1.locationDistrict != m2.locationDistrict) errors += AddError("Location District", m1.locationDistrict, m2.locationDistrict);
            if (m1.locationRegion != m2.locationRegion) errors += AddError("Location Region", m1.locationRegion, m2.locationRegion);

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
                        (m1.PersonalKeywordTags.Contains(tag) ? " Not add":" Not removed")) +                        
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

        static public float ConvertRatingStarsToRatingPercent(byte personalRating)
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

        public Byte? PersonalRating
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
                        if (ConvertRatingPercentToRetingStars((float)personalRatingPercent) != personalRating)
                            personalRatingPercent = ConvertRatingStarsToRatingPercent((byte)personalRating);
                    }
                    else personalRatingPercent = ConvertRatingStarsToRatingPercent((byte)personalRating);
                }
                else
                    personalRatingPercent = null;
            }
        }

        private byte ConvertRatingPercentToRetingStars(float personalRatingPercent)
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

        public float? PersonalRatingPercent
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
                    personalRating = ConvertRatingPercentToRetingStars((float)personalRatingPercent);
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

        public void PersonalRegionListAddIfNameNotExists(RegionStructure regionStructure)
        {            
            bool doesNameExists = false;
            foreach (RegionStructure regionStructureSearch in personalRegionList)
            {
                if (regionStructure.Name == regionStructureSearch.Name) doesNameExists = true;
            }
            if (!doesNameExists) personalRegionList.Add(regionStructure);
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
        public Double? LocationAltitude
        {
            get => locationAltitude;
            set => locationAltitude = (value == null ? (double?)null : (double?)Math.Round((double)value, SqliteDatabase.SqliteDatabaseUtilities.FloatAndDoubleNumberOfDecimalsShort));            
        }
        public Double? LocationLatitude
        {
            get => locationLatitude;           
            set => locationLatitude = (value == null ? (double?)null : (double?)Math.Round((double)value, SqliteDatabase.SqliteDatabaseUtilities.FloatAndDoubleNumberOfDecimals));
        }
        public Double? LocationLongitude
        {
            get => locationLongitude;
            set => locationLongitude = (value == null ? (double?)null : (double?)Math.Round((double)value, SqliteDatabase.SqliteDatabaseUtilities.FloatAndDoubleNumberOfDecimals));
        }
        public DateTime? LocationDateTime { get => locationDateTime; set => locationDateTime = value; }
        public String LocationName { get => locationName; set => locationName = value; }
        public string LocationCountry { get => locationCountry; set => locationCountry = value; }
        public string LocationCity { get => locationDistrict; set => locationDistrict = value; }
        public string LocationState { get => locationRegion; set => locationRegion = value; }
        #endregion 

        
    }
}

