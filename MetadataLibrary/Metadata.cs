using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using TimeZone;
using LocationNames;
using FileDateTime;
using ApplicationAssociations;
using System.Linq;

namespace MetadataLibrary
{

    [Serializable]
    public class Metadata
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region Local variables
        public String errors = "";
        private String fileDirectory;
        private Byte? personalRating;
        private Byte? personalRatingPercent;
        private List<RegionStructure> personalRegionList = new List<RegionStructure>();
        private List<KeywordTag> personalTagList = new List<KeywordTag>();

        //Location
        private float? locationAltitude;
        private float? locationLatitude;
        private float? locationLongitude;
        private DateTime? locationDateTime;
        private String locationName;
        private String locationCountry;
        private String locationCity;
        private String locationState;

        public bool Readonly { get; set; } //Used to debug, changes of original Metadata
        #endregion

        #region MergeMetadatas
        public static Metadata MergeMetadatas (Metadata metadataWinner, Metadata metadataLoser)
        {
            Metadata metadata = new Metadata(metadataWinner);

            if (metadataWinner.Readonly)
            {
                //DEBUG - Create to find where orginal is changes
            }
            //Broker
            //metadata.Broker = metadata.Broker;

            //JTN: MediaFileAttributes
            //File
            if (metadata.FileName == null) metadata.FileName = metadataLoser.FileName;
            if (metadata.fileDirectory == null) metadata.fileDirectory = metadataLoser.fileDirectory;
            if (metadata.FileSize == null) metadata.FileSize = metadataLoser.FileSize;

            if (metadata.FileDateCreated == null) metadata.FileDateCreated = metadataLoser.FileDateCreated;
            if (metadata.FileDateModified == null) metadata.FileDateModified = metadataLoser.FileDateModified;
            if (metadata.FileDateAccessed == null) metadata.FileDateAccessed = metadataLoser.FileDateAccessed;
            if (metadata.FileMimeType == null) metadata.FileMimeType = metadataLoser.FileMimeType;

            //Personal
            if (metadata.PersonalTitle == null) metadata.PersonalTitle = metadataLoser.PersonalTitle;
            if (metadata.PersonalDescription == null) metadata.PersonalDescription = metadataLoser.PersonalDescription;
            if (metadata.PersonalComments == null) metadata.PersonalComments = metadataLoser.PersonalComments;
            if (metadata.personalRating == null) metadata.personalRating = metadataLoser.personalRating;
            if (metadata.personalRatingPercent == null) metadata.personalRatingPercent = metadataLoser.personalRatingPercent;
            if (metadata.PersonalAuthor == null) metadata.PersonalAuthor = metadataLoser.PersonalAuthor;
            if (metadata.PersonalAlbum == null) metadata.PersonalAlbum = metadataLoser.PersonalAlbum;
            foreach (RegionStructure region in metadataLoser.personalRegionList) metadata.PersonalRegionListAddIfNameNotExists(new RegionStructure(region));
            foreach (KeywordTag tag in metadataLoser.personalTagList) metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(tag));
            
            //Camera
            if (metadata.CameraMake == null) metadata.CameraMake = metadataLoser.CameraMake;
            if (metadata.CameraModel == null) metadata.CameraModel = metadataLoser.CameraModel;

            //Media
            if (metadata.MediaDateTaken == null) metadata.MediaDateTaken = metadataLoser.mediaDateTaken;
            if (metadata.MediaWidth == null) metadata.MediaWidth = metadataLoser.mediaWidth;
            if (metadata.MediaHeight == null) metadata.MediaHeight = metadataLoser.mediaHeight;
            if (metadata.MediaOrientation == null) metadata.MediaOrientation = metadataLoser.mediaOrientation;
            if (metadata.MediaVideoLength == null) metadata.MediaVideoLength = metadataLoser.mediaVideoLength;

            //Location
            if (metadata.LocationAltitude == null) metadata.LocationAltitude = metadataLoser.locationAltitude;
            if (metadata.LocationLatitude == null) metadata.LocationLatitude = metadataLoser.locationLatitude;
            if (metadata.LocationLongitude == null) metadata.LocationLongitude = metadataLoser.locationLongitude;
            if (metadata.LocationDateTime == null) metadata.LocationDateTime = metadataLoser.locationDateTime;
            if (metadata.LocationCountry == null) metadata.LocationCountry = metadataLoser.locationName;
            if (metadata.LocationCountry == null) metadata.LocationCountry = metadataLoser.locationCountry;
            if (metadata.LocationCity == null) metadata.LocationCity = metadataLoser.locationCity;
            if (metadata.LocationState == null) metadata.LocationState = metadataLoser.locationState;

            return metadata;
        }
        #endregion

        #region Constructors
        public Metadata(MetadataBrokerType broker)
        {
            Broker = broker;
        }

        public Metadata(Metadata metadata)
        {    
            errors = metadata.errors;

            Readonly = false; //Reason for copy, is not to edit original 
            //Broker
            Broker = metadata.Broker;

            //File
            FileName = metadata.FileName;
            fileDirectory = metadata.fileDirectory;
            FileSize = metadata.FileSize;
            FileDateCreated = metadata.FileDateCreated;
            FileDateModified = metadata.FileDateModified;
            FileDateAccessed = metadata.FileDateAccessed;
            FileMimeType = metadata.FileMimeType;

            //Personal
            PersonalTitle = metadata.PersonalTitle;
            PersonalDescription = metadata.PersonalDescription;
            PersonalComments = metadata.PersonalComments;
            personalRating = metadata.personalRating;
            personalRatingPercent = metadata.personalRatingPercent;
            PersonalAuthor = metadata.PersonalAuthor;
            PersonalAlbum = metadata.PersonalAlbum;
            try
            {
                foreach (RegionStructure region in metadata.personalRegionList)
                {
                    if (region != null) personalRegionList.Add(new RegionStructure(region));
                }
            } catch (Exception ex)
            {
                Logger.Error(ex.Message); //If not thread safe, but don't crash and save error
            }
            try
            {
                foreach (KeywordTag tag in metadata.personalTagList) personalTagList.Add(new KeywordTag(tag));
            } catch (Exception ex)
            {
                Logger.Error(ex.Message); //If not thread safe, but don't crash and save error
            }
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

        #region Override - Equals
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
        #endregion

        #region operator ==
        public static bool operator ==(Metadata m1, Metadata m2)
        {
            if (m1 is null && m2 is null) return true;
            if (m1 is null) return false;
            if (m2 is null) return false;
            if (ReferenceEquals(m1, m2)) return true;
            //return m1.GetHashCode() == m2.GetHashCode() ;

            #region  Broker
            if (m1.Broker != m2.Broker) return false;
            #endregion

            #region File
            if (String.Compare(m1.FileName, m2.FileName, comparisonType: StringComparison.OrdinalIgnoreCase) != 0) return false;
            if (String.Compare(m1.fileDirectory, m2.fileDirectory, comparisonType: StringComparison.OrdinalIgnoreCase) != 0) return false;
            if (m1.FileSize != m2.FileSize) return false;
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.FileDateCreated, m2.FileDateCreated)) return false;
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.FileDateModified, m2.FileDateModified)) return false;
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.FileDateAccessed, m2.FileDateAccessed)) return false;
            if (m1.FileMimeType != m2.FileMimeType) return false;
            #endregion

            #region Personal
            if (!(string.IsNullOrWhiteSpace(m1.PersonalTitle) && string.IsNullOrWhiteSpace(m2.PersonalTitle)) && 
                m1.PersonalTitle != m2.PersonalTitle) return false;
            if (!(string.IsNullOrWhiteSpace(m1.PersonalDescription) && string.IsNullOrWhiteSpace(m2.PersonalDescription)) &&
                m1.PersonalDescription != m2.PersonalDescription) return false;
            if (!(string.IsNullOrWhiteSpace(m1.PersonalComments) && string.IsNullOrWhiteSpace(m2.PersonalComments)) &&
                m1.PersonalComments != m2.PersonalComments) return false;
            if (m1.personalRating != m2.personalRating) return false;
            if (m1.personalRatingPercent != m2.personalRatingPercent) return false;
            if (!(string.IsNullOrWhiteSpace(m1.PersonalAuthor) && string.IsNullOrWhiteSpace(m2.PersonalAuthor)) &&
                m1.PersonalAuthor != m2.PersonalAuthor) return false;
            if (!(string.IsNullOrWhiteSpace(m1.PersonalAlbum) && string.IsNullOrWhiteSpace(m2.PersonalAlbum)) &&
                m1.PersonalAlbum != m2.PersonalAlbum) return false;
            #endregion

            #region RegionStructureList
            if (VerifyRegionStructureList(m1.personalRegionList, m2.personalRegionList) == false) return false;
            #endregion

            #region KeywordList
            if (VerifyKeywordList(m1.personalTagList, m2.personalTagList) == false) return false;
            #endregion

            #region Camera
            if (m1.CameraMake != m2.CameraMake) return false;
            if (m1.CameraModel != m2.CameraModel) return false;
            #endregion

            #region Media
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.mediaDateTaken, m2.mediaDateTaken)) return false;
            if (m1.mediaWidth != m2.mediaWidth) return false;
            if (m1.mediaHeight != m2.mediaHeight) return false;
            if (m1.mediaOrientation != m2.mediaOrientation) return false;
            if (m1.mediaVideoLength != m2.mediaVideoLength) return false;
            #endregion

            #region Location
            if (m1.locationAltitude != m2.locationAltitude) return false;
            if (m1.locationLatitude != m2.locationLatitude) return false;
            if (m1.locationLongitude != m2.locationLongitude) return false;
            if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(m1.locationDateTime, m2.locationDateTime)) return false;
            if (m1.locationName != m2.locationName) return false;
            if (m1.locationCountry != m2.locationCountry) return false;
            if (m1.locationCity != m2.locationCity) return false;
            if (m1.locationState != m2.locationState) return false;
            #endregion 
            return true;
        }
        #endregion

        #region operator !=
        public static bool operator !=(Metadata m1, Metadata m2)
        {
            return !(m1 == m2);
        }
        #endregion

        #region GetHashCode
        public override int GetHashCode()
        {
            int hashCode = 0;
            //Broker
            //if (broker != null) hashCode += broker.GetHashCode();

            #region File
            if (FileName != null) hashCode += FileName.GetHashCode();
            if (fileDirectory != null) hashCode += fileDirectory.GetHashCode();
            if (FileSize != null) hashCode += FileSize.GetHashCode();
            if (FileDateCreated != null) hashCode += FileDateCreated.GetHashCode();
            if (FileDateModified != null) hashCode += FileDateModified.GetHashCode();
            if (FileDateAccessed != null) hashCode += FileDateAccessed.GetHashCode();
            if (FileMimeType != null) hashCode += FileMimeType.GetHashCode();
            #endregion

            #region Personal
            if (PersonalTitle != null) hashCode += PersonalTitle.GetHashCode();
            if (PersonalDescription != null) hashCode += PersonalDescription.GetHashCode();
            if (PersonalComments != null) hashCode += PersonalComments.GetHashCode();
            if (personalRating != null) hashCode += personalRating.GetHashCode();
            if (personalRatingPercent != null) hashCode += personalRatingPercent.GetHashCode();
            if (PersonalAuthor != null) hashCode += PersonalAuthor.GetHashCode();
            if (PersonalAlbum != null) hashCode += PersonalAlbum.GetHashCode();
            #endregion

            #region Camera
            if (CameraMake != null) hashCode += CameraMake.GetHashCode();
            if (CameraModel != null) hashCode += CameraModel.GetHashCode();
            #endregion

            #region Media
            if (mediaDateTaken != null) hashCode += mediaDateTaken.GetHashCode();
            if (mediaWidth != null) hashCode += mediaWidth.GetHashCode();
            if (mediaHeight != null) hashCode += mediaHeight.GetHashCode();
            if (mediaOrientation != null) hashCode += mediaOrientation.GetHashCode();
            if (mediaVideoLength != null) hashCode += mediaVideoLength.GetHashCode();
            #endregion

            #region Location
            if (locationAltitude != null) hashCode += locationAltitude.GetHashCode();
            if (locationLatitude != null) hashCode += locationLatitude.GetHashCode();
            if (locationLongitude != null) hashCode += locationLongitude.GetHashCode();
            if (locationDateTime != null) hashCode += locationDateTime.GetHashCode();
            if (locationName != null) hashCode += locationName.GetHashCode();
            if (locationCountry != null) hashCode += locationCountry.GetHashCode();
            if (locationCity != null) hashCode += locationCity.GetHashCode();
            if (locationState != null) hashCode += locationState.GetHashCode();
            #endregion

            #region PersonalRegionList
            foreach (RegionStructure region in personalRegionList) hashCode += region.GetHashCode();
            #endregion

            #region PersonalTagList
            foreach (KeywordTag tag in personalTagList) hashCode += tag.GetHashCode();
            #endregion

            return hashCode;
        }
        #endregion

        #region private - VerifyRegionStructureList
        private static bool VerifyRegionStructureList(List<RegionStructure> personalRegionList1, List<RegionStructure> personalRegionList2)
        {
            foreach (RegionStructure region in personalRegionList1) if (!region.DoesThisRectangleAndNameExistInList(personalRegionList2)) return false;
            foreach (RegionStructure region in personalRegionList2) if (!region.DoesThisRectangleAndNameExistInList(personalRegionList1)) return false;
            return true;
        }
        #endregion

        #region private - VerifyKeywordList
        private static bool VerifyKeywordList(List<KeywordTag> personalKeywordTagList1, List<KeywordTag> personalKeywordTagList2)
        {
            if (personalKeywordTagList1.Count != personalKeywordTagList2.Count) return false;
            foreach (KeywordTag tag in personalKeywordTagList1) if (!personalKeywordTagList2.Contains(tag)) return false;
            foreach (KeywordTag tag in personalKeywordTagList2) if (!personalKeywordTagList1.Contains(tag)) return false;
            return true;
        }
        #endregion

        #region Properties Helper - FileDate
        public DateTime? FileDate
        {
            get
            {
                if (FileDateCreated != null && FileDateModified != null) return FileDateCreated < FileDateModified ? FileDateCreated : FileDateModified;
                else if (FileDateCreated == null && FileDateModified != null) return FileDateModified;
                else if (FileDateCreated != null && FileDateModified == null) return FileDateCreated;
                else return null;
            }
        }
        #endregion

        #region Properties Helper - FileDate
        public DateTime? FileSmartDate(string allowedDateFormats)
        {
            FileDateTimeReader fileDateTimeReader = new FileDateTimeReader(allowedDateFormats);
            return fileDateTimeReader.SmartDateTime(FileName, FileDateCreated, FileDateModified);
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

        #region Properties Helper - FileEntry
        private FileEntry fileEntry = null;
        public FileEntry FileEntry
        {
            get
            {
                if (fileEntry == null) fileEntry = new FileEntry(Path.Combine(fileDirectory, FileName), (DateTime)FileDateModified);
                return fileEntry;
            }
        }
        #endregion

        #region Properties Helper - FindFileEntryInList
        public static int FindFileEntryInList(List<Metadata> metadataListToCheck, FileEntry fileEntry)
        {
            if (metadataListToCheck == null) return -1;
            if (metadataListToCheck.Count == 0) return -1;

            try
            {
                for (int i = 0; i < metadataListToCheck.Count; i++)
                {
                    Metadata metadata = metadataListToCheck[i];                  
                    if (metadata != null && (string.Compare(metadata.FileFullPath, fileEntry.FileFullPath, true) == 0) && fileEntry.LastWriteDateTime == metadata.FileDateModified) 
                        return i;
                }
            }
            catch { }
            return -1;
        }
        #endregion 

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
                    if (metadata != null && (string.Compare(metadata.FileFullPath, findThis) == 0)) return i;
                }
            }
            catch { }
            return -1;
        }
        #endregion

        #region Properties Helper - IsFullFilenameInList
        public static bool IsFullFilenameInList(List<Metadata> queueSaveMetadataUpdatedByUser, string fullFilePath)
        {
            return FindFullFilenameInList(queueSaveMetadataUpdatedByUser, fullFilePath) >= 0;
        }
        #endregion 

        #region Properties - MetadataBrokerTypes
        public MetadataBrokerType Broker { get; set; }
        #endregion

        #region Properties - File
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

        public string FileFullPath 
        { get 
            {
                if (fileDirectory == null || FileName == null)
                {
                    //DEBUG
                    return null;
                }
                return Path.Combine(fileDirectory, FileName); 
            }
        }
        public Int64? FileSize { get; set; }
        public DateTime? FileDateCreated { get; set; }
        public DateTime? FileDateModified { get; set; }
        public DateTime? FileDateAccessed { get; set; }
        public string FileMimeType { get; set; }
        #endregion

        #region Properties - Personal
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

        public static byte ConvertRatingPercentToRetingStars(byte personalRatingPercent)
        {
            ////starRating = rating == 0 ? 0 : (uint)Math.Round((double)rating / 25.0) + 1;
            if (personalRatingPercent > 87)
            {
                return 5;
            }
            else if (personalRatingPercent > 62 && personalRatingPercent <= 87)
            {
                return 4;
            }
            else if (personalRatingPercent > 37 && personalRatingPercent <= 62)
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

        public Point RotatePoint90(int x, int y, int pageWidth, int pageHeight, double degrees)
        {
            switch (degrees)
            {
                case 0: return new Point(x, y);
                case 270: return new Point(y, pageWidth - x);
                case 180: return new Point(pageWidth - x, pageHeight - y);
                case 90: return new Point(pageHeight - y, x);
                default: throw new NotImplementedException();
            }
        }

        public Size RotateSize(Size size, double degrees)
        {
            switch (degrees)
            {
                case 0: return size;
                case 90: return new Size(size.Height, size.Width);
                case 180: return size;
                case 270: return new Size(size.Height, size.Width);
                default: throw new NotImplementedException();
            }
        }

        public void PersonalRegionRotate(double rotateDegrees)
        {
            if (this.Readonly)
            {
                //DEBUG - Create to find where orginal is changes
            }
            List<RegionStructure> regionStructuresCopy = new List<RegionStructure>();
            foreach (RegionStructure regionStructure in personalRegionList)
            {
                RegionStructure regionStructureCopy = new RegionStructure(regionStructure);
                Rectangle rectangleInPixel = regionStructure.GetImageRegionPixelRectangle(MediaSize);

                ///RotatePoint X = 224 Y = 140 Width = 2813 Height = 2025
                //Point pointTopLeft = RotatePoint(MediaSize, rectangleInPixel.Location, rotateDegrees);
                Point newPoint1 = RotatePoint90(rectangleInPixel.X, rectangleInPixel.Y, (int)MediaWidth, (int)MediaHeight, rotateDegrees);
                Point newPoint2 = RotatePoint90(
                    rectangleInPixel.X + rectangleInPixel.Width, 
                    rectangleInPixel.Y + rectangleInPixel.Height, (int)MediaWidth, (int)MediaHeight, rotateDegrees);
                Rectangle rectangleTransformed = new Rectangle(
                    Math.Min(newPoint1.X, newPoint2.X), 
                    Math.Min(newPoint1.Y, newPoint2.Y),
                    Math.Max(newPoint1.X, newPoint2.X) - Math.Min(newPoint1.X, newPoint2.X),
                    Math.Max(newPoint1.Y, newPoint2.Y) - Math.Min(newPoint1.Y, newPoint2.Y));
                MediaSize = RotateSize(MediaSize, rotateDegrees);
                
                RectangleF rectangleF = RegionStructure.CalculateImageRegionAbstarctRectangle(
                    MediaSize, rectangleTransformed, regionStructureCopy.RegionStructureType);
                regionStructureCopy.AreaX = rectangleF.X;
                regionStructureCopy.AreaY = rectangleF.Y;
                regionStructureCopy.AreaWidth = rectangleF.Width;
                regionStructureCopy.AreaHeight = rectangleF.Height;
                regionStructuresCopy.Add(regionStructureCopy);
            }
            personalRegionList = regionStructuresCopy;
        }

        public void PersonalRegionListAddIfNotExists(RegionStructure regionStructure)
        {
            if (regionStructure == null)
            {
                //DEBUG - should not happen
                //throw new NotImplementedException();
            }
            if (this.Readonly)
            {
                //DEBUG - Create to find where orginal is changes
            }
            if (!personalRegionList.Contains(regionStructure)) personalRegionList.Add(regionStructure);
        }

        public bool PersonalRegionIsThumbnailMissing()
        {
            foreach (RegionStructure regionStructureCheckMissingThumbnail in PersonalRegionList)
            {
                if (regionStructureCheckMissingThumbnail?.Thumbnail == null) return true;
            }
            return false;
        }

        public void PersonalRegionListAddIfNotAreaAndNameExists(RegionStructure regionStructure)
        {
            if (this.Readonly)
            {
                //DEBUG - Create to find where orginal is changes
            }
            if (!regionStructure.DoesThisRectangleAndNameExistInList(personalRegionList)) personalRegionList.Add(regionStructure);
        }

        public void PersonalRegionRemoveNamelessDoubleRegions(List<RegionStructure> removeFromThisRegionStructures)
        {            
            bool foundRegion;
            do
            {
                foundRegion = false;
                int indexChecking = -1;
                int indexFoundLocal = -1;

                for (int indexSearch = 0; indexSearch < removeFromThisRegionStructures.Count; indexSearch++)
                {
                    if (string.IsNullOrWhiteSpace(removeFromThisRegionStructures[indexSearch].Name))
                    {
                        indexFoundLocal = removeFromThisRegionStructures[indexSearch].IndexOfRectangleInList(PersonalRegionList);

                        if (indexFoundLocal != -1 && !string.IsNullOrWhiteSpace(PersonalRegionList[indexFoundLocal].Name))
                        {
                            indexChecking = indexSearch;
                            break;
                        }
                    }
                }

                if (indexChecking != -1 && indexFoundLocal != -1)
                {
                    removeFromThisRegionStructures.RemoveAt(indexChecking);
                    foundRegion = true;
                }
            } while (foundRegion);

        }

        public void PersonalRegionSetNamelessRegions(List<RegionStructure> updateNameOnThisRegionStructures)
        {
            bool foundRegion;
            do
            {
                foundRegion = false;
                int indexChecking = -1;
                int indexFoundLocal = -1;

                for (int indexSearch = 0; indexSearch < updateNameOnThisRegionStructures.Count; indexSearch++)
                {
                    if (string.IsNullOrWhiteSpace(updateNameOnThisRegionStructures[indexSearch].Name))
                    {
                        indexFoundLocal = updateNameOnThisRegionStructures[indexSearch].IndexOfRectangleInList(PersonalRegionList);

                        if (indexFoundLocal != -1 && !string.IsNullOrWhiteSpace(PersonalRegionList[indexFoundLocal].Name))
                        {
                            indexChecking = indexSearch;
                            break;
                        }
                    }
                }

                if (indexChecking != -1 && indexFoundLocal != -1)
                {
                    updateNameOnThisRegionStructures[indexChecking].Name = PersonalRegionList[indexFoundLocal].Name;
                    foundRegion = true;
                }
            } while (foundRegion);
        }

        public void PersonalRegionSetRegionlessRegions(List<RegionStructure> updateNameOnThisRegionStructures)
        {
            bool foundRegion;
            do
            {
                foundRegion = false;
                int indexChecking = -1;
                int indexFoundLocal = -1;

                for (int indexSearch = 0; indexSearch < updateNameOnThisRegionStructures.Count; indexSearch++)
                {
                    if (updateNameOnThisRegionStructures[indexSearch].AreaX == 0 &&
                        updateNameOnThisRegionStructures[indexSearch].AreaY == 0 &&
                        updateNameOnThisRegionStructures[indexSearch].AreaWidth == 1 &&
                        updateNameOnThisRegionStructures[indexSearch].AreaHeight == 1 
                        )
                    {
                        
                        indexFoundLocal = updateNameOnThisRegionStructures[indexSearch].IndexOfNameInList(PersonalRegionList);

                        if (indexFoundLocal != -1 && 
                            PersonalRegionList[indexFoundLocal].AreaX != 0 &&
                            PersonalRegionList[indexFoundLocal].AreaY != 0 &&
                            PersonalRegionList[indexFoundLocal].AreaWidth != 1 &&
                            PersonalRegionList[indexFoundLocal].AreaHeight != 1)
                        {
                            indexChecking = indexSearch;
                            break;
                        }
                    }
                }

                if (indexChecking != -1 && indexFoundLocal != -1)
                {
                    updateNameOnThisRegionStructures[indexChecking].AreaX = PersonalRegionList[indexFoundLocal].AreaX;
                    updateNameOnThisRegionStructures[indexChecking].AreaY = PersonalRegionList[indexFoundLocal].AreaY;
                    updateNameOnThisRegionStructures[indexChecking].AreaHeight = PersonalRegionList[indexFoundLocal].AreaHeight;
                    updateNameOnThisRegionStructures[indexChecking].AreaWidth = PersonalRegionList[indexFoundLocal].AreaWidth;
                    updateNameOnThisRegionStructures[indexChecking].Type = PersonalRegionList[indexFoundLocal].Type;
                    updateNameOnThisRegionStructures[indexChecking].RegionStructureType = PersonalRegionList[indexFoundLocal].RegionStructureType;
                    updateNameOnThisRegionStructures[indexChecking].Thumbnail = PersonalRegionList[indexFoundLocal].Thumbnail;
                    foundRegion = true;
                }
            } while (foundRegion);
        }

        public void PersonalRegionListAddIfNameNotExists(RegionStructure regionStructure)
        {
            if (this.Readonly)
            {
                //DEBUG - Create to find where orginal is changes
            }
            if (!regionStructure.DoesThisNameExistInList(personalRegionList)) personalRegionList.Add(regionStructure);
        }

        public List<KeywordTag> PersonalKeywordTags
        {
            get => personalTagList;
        }

        public bool PersonalKeywordTagsAddIfNotExists(KeywordTag keywordTag, bool caseSencetive = true)
        {
            if (this.Readonly)
            {
                //DEBUG - Create to find where orginal is changes
            }
            bool keywordWasAdded = false;
            if (!caseSencetive)
            {
                foreach (KeywordTag keywordTagToCkeck in personalTagList)
                {
                    if (string.Equals(keywordTagToCkeck.Keyword, keywordTag.Keyword, StringComparison.OrdinalIgnoreCase)) return true;
                }
                personalTagList.Add(keywordTag);
                keywordWasAdded = true;
            }
            else
            {
                if (!personalTagList.Contains(keywordTag))
                {
                    personalTagList.Add(keywordTag);
                    keywordWasAdded = true;
                }
            }
            return keywordWasAdded;
        }

        //public void PersonalTagListUpdateImage(RegionStructure updateRegion, Image thumbnail)
        //{
        //    for (int i = 0; i < personalRegionList.Count; i++)
        //    {
        //        if (personalRegionList[i] == updateRegion)
        //        {
        //            RegionStructure region = personalRegionList[i];
        //            region.Thumbnail = thumbnail;
        //            personalRegionList.RemoveAt(i);
        //            personalRegionList.Insert(i, region);
        //            break;
        //        }
        //    }
        //}

        //public Image PersonalTagListGetThumbnail(RegionStructure region)
        //{
        //    if (personalRegionList.Contains(region))
        //    {
        //        return personalRegionList[personalRegionList.IndexOf(region)].Thumbnail;
        //    }
        //    return null;
        //}
        #endregion

        #region Properties - Camera
        public String CameraMake { get; set; }
        public String CameraModel { get; set; }
        #endregion

        #region MediaTaken UTC - TryParseDateTakenToUtc
        public bool TryParseDateTakenToUtc(out DateTime? dateTime)
        {
            dateTime = null;

            if (MediaDateTaken == null) return false;
            if (LocationDateTime == null) return false;
            
            

            DateTime locationDateTomeUtc = ((DateTime)LocationDateTime).ToUniversalTime();

            DateTime mediaTakenUtc;

            if (LocationLatitude != null && LocationLongitude != null)
            {
                TimeZoneInfo timeZoneInfoGPSLocation = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)LocationLatitude, (double)LocationLongitude);
                if (timeZoneInfoGPSLocation != null)
                {
                    DateTimeOffset locationOffset = new DateTimeOffset(locationDateTomeUtc.Ticks, timeZoneInfoGPSLocation.GetUtcOffset(locationDateTomeUtc));
                    mediaTakenUtc = new DateTime(((DateTime)MediaDateTaken).Add(-locationOffset.Offset).Ticks, DateTimeKind.Utc);
                }
                else
                {
                    TimeSpan diff = (new DateTime(((DateTime)MediaDateTaken).Ticks, DateTimeKind.Utc)) - locationDateTomeUtc;
                    mediaTakenUtc = new DateTime(((DateTime)MediaDateTaken).Add(-diff).Ticks, DateTimeKind.Utc);
                }
            } else
            {
                TimeSpan diff = (new DateTime( ((DateTime)MediaDateTaken).Ticks, DateTimeKind.Utc)) - locationDateTomeUtc;
                mediaTakenUtc = new DateTime(((DateTime)MediaDateTaken).Add(-diff).Ticks, DateTimeKind.Utc);
            }
            
            TimeSpan? timeDiffrence = TimeZoneLibrary.CalulateTimeDiffrentWithoutTimeZone(mediaTakenUtc, (DateTime)LocationDateTime);
            if (timeDiffrence == null || Math.Abs(((TimeSpan)timeDiffrence).TotalSeconds) > 30*60) 
                return false; //Accept GPS to use time to find time
            
            dateTime = mediaTakenUtc;

            return true;
        }
        #endregion

        #region Properties Helper - LocationTimeZone 
        public string LocationTimeZoneDescription
        {
            get
            {
                TimeSpan? timeSpan = TimeZoneLibrary.CalulateTimeDiffrentWithoutTimeZone(MediaDateTaken, LocationDateTime);

                string prefredTimeZoneName = "";

                string timeZoneName = TimeZoneLibrary.GetTimeZoneName(timeSpan, LocationDateTime, prefredTimeZoneName, out string timeZoneAlternatives);
                string timeSpanString = "(±??:??)";
                if (timeSpan != null) timeSpanString = TimeZoneLibrary.ToStringOffset((TimeSpan)timeSpan);


                string timeZone = timeSpanString + " " + timeZoneName;

                //Fine time zone using GPS Location
                if (LocationLatitude != null && LocationLongitude != null)
                {
                    TimeZoneInfo timeZoneInfoGPSLocation = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)LocationLatitude, (double)LocationLongitude);
                    if (timeZoneInfoGPSLocation == null) return null;
                    
                    DateTime findOffsettDateTime;
                    if (LocationDateTime != null) findOffsettDateTime = (DateTime)LocationDateTime;
                    else if (LocationDateTime != null) findOffsettDateTime = (DateTime)MediaDateTaken;
                    else findOffsettDateTime = DateTime.Now;

                    DateTime findOffsettDateTimeUTC = findOffsettDateTime.ToUniversalTime();
                    DateTimeOffset locationOffset = new DateTimeOffset(findOffsettDateTimeUTC.Ticks, timeZoneInfoGPSLocation.GetUtcOffset(findOffsettDateTimeUTC));

                    return TimeZoneLibrary.ToStringOffset(locationOffset.Offset) + " " + TimeZoneLibrary.TimeZoneNameStandarOrDaylight(timeZoneInfoGPSLocation, findOffsettDateTimeUTC);
                }

                return timeZone;
            }
        }
        #endregion

        #region Properties - Media
        private DateTime? mediaDateTaken;
        public DateTime? MediaDateTaken { get => mediaDateTaken; set => mediaDateTaken = value; }
        
        private Int32? mediaWidth;        
        public Int32? MediaWidth { get => mediaWidth; set => mediaWidth = value; }

        private Int32? mediaHeight;        
        public Int32? MediaHeight { get => mediaHeight; set => mediaHeight = value; }
        public Size MediaSize { get => new Size(mediaWidth == null ? 0 : (int)mediaWidth, mediaWidth == null ? 0 : (int)mediaHeight);
            set {
                MediaWidth = value.Width;
                MediaHeight = value.Height;
            }
        }

        private Int32? mediaOrientation;
        public int? MediaOrientation { get => mediaOrientation; set => mediaOrientation = value; }

        private Int32? mediaVideoLength;
        public int? MediaVideoLength { get => mediaVideoLength; set => mediaVideoLength = value; }
        #endregion 

        #region Properties - Location
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

        #region Errors handler

        #region Errors handler - Property
        public string Errors { get => errors; set => errors = value; }
        #endregion

        #region Errors handler - AddError
        private static string AddError(string text, object o1, object o2)
        {
            return text + ": '" + (o1 == null ? "" : o1.ToString()) + "' vs. '" + (o2 == null ? "" : o2.ToString()) + "'\r\n";
        }
        #endregion

        #region Errors handler - GetErrors
        public static string GetErrors(Metadata metadataOriginal, Metadata metadataChangeInto, bool whatsAddedChanged = false)
        {
            string errors = "";
            if (metadataOriginal is null && metadataChangeInto is null) return "";
            if (metadataOriginal is null) return "Can't compare missing metadata for file 1.\r\n";
            if (metadataChangeInto is null) return "Can't compare missing metadata for file 2.\r\n";
            if (ReferenceEquals(metadataOriginal, metadataChangeInto)) return "";
            if (metadataOriginal == metadataChangeInto) return "";

            string fileInformation =
                (whatsAddedChanged ?
                "Edited by user vs Media file\r\n" + "File: " + metadataChangeInto.FileFullPath + "\r\n" :
                "Original (saved) vs. Read back after saved\r\n" + "File: " + metadataChangeInto.FileFullPath + "\r\n");

            #region File
            if (metadataOriginal.Broker != metadataChangeInto.Broker) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Broker", metadataOriginal.Broker, metadataChangeInto.Broker);
            if (String.Compare(metadataOriginal.FileName, metadataChangeInto.FileName, comparisonType: StringComparison.OrdinalIgnoreCase) != 0) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("File name", metadataOriginal.FileName, metadataChangeInto.FileName);
            if (String.Compare(metadataOriginal.fileDirectory, metadataChangeInto.fileDirectory, comparisonType: StringComparison.OrdinalIgnoreCase) != 0) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("File direcotry", metadataOriginal.fileDirectory, metadataChangeInto.fileDirectory);
            if (metadataOriginal.FileSize != metadataChangeInto.FileSize) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("File size", metadataOriginal.FileSize, metadataChangeInto.FileSize);
            if (metadataOriginal.FileDateCreated != metadataChangeInto.FileDateCreated) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("File Date Created", metadataOriginal.FileDateCreated, metadataChangeInto.FileDateCreated);
            if (metadataOriginal.FileDateModified != metadataChangeInto.FileDateModified) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("File Date Modified", metadataOriginal.FileDateModified, metadataChangeInto.FileDateModified);
            if (metadataOriginal.FileDateAccessed != metadataChangeInto.FileDateAccessed) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("File Last Accessed", metadataOriginal.FileDateAccessed, metadataChangeInto.FileDateAccessed);
            if (metadataOriginal.FileMimeType != metadataChangeInto.FileMimeType) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("FileMimeType", metadataOriginal.FileMimeType, metadataChangeInto.FileMimeType);
            #endregion

            #region Personal
            if (metadataOriginal.PersonalTitle != metadataChangeInto.PersonalTitle) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Title", metadataOriginal.PersonalTitle, metadataChangeInto.PersonalTitle);
            if (metadataOriginal.PersonalDescription != metadataChangeInto.PersonalDescription) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Description", metadataOriginal.PersonalDescription, metadataChangeInto.PersonalDescription);
            if (metadataOriginal.PersonalComments != metadataChangeInto.PersonalComments) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Comments", metadataOriginal.PersonalComments, metadataChangeInto.PersonalComments);
            if (metadataOriginal.personalRating != metadataChangeInto.personalRating) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Rating", metadataOriginal.personalRating, metadataChangeInto.personalRating);
            if (metadataOriginal.personalRatingPercent != metadataChangeInto.personalRatingPercent) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Rating Percent", metadataOriginal.personalRatingPercent, metadataChangeInto.personalRatingPercent);
            if (metadataOriginal.PersonalAuthor != metadataChangeInto.PersonalAuthor) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Author", metadataOriginal.PersonalAuthor, metadataChangeInto.PersonalAuthor);
            if (metadataOriginal.PersonalAlbum != metadataChangeInto.PersonalAlbum) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Album", metadataOriginal.PersonalAlbum, metadataChangeInto.PersonalAlbum);
            #endregion

            #region Camera
            if (metadataOriginal.CameraMake != metadataChangeInto.CameraMake) return errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Camera Make", metadataOriginal.CameraMake, metadataChangeInto.CameraMake);
            if (metadataOriginal.CameraModel != metadataChangeInto.CameraModel) return errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Camra Model", metadataOriginal.CameraModel, metadataChangeInto.CameraModel);
            #endregion

            #region Media
            if (metadataOriginal.mediaDateTaken != metadataChangeInto.mediaDateTaken) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Media DateTaken", metadataOriginal.mediaDateTaken, metadataChangeInto.mediaDateTaken);
            if (metadataOriginal.mediaWidth != metadataChangeInto.mediaWidth) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Media Width", metadataOriginal.mediaWidth, metadataChangeInto.mediaWidth);
            if (metadataOriginal.mediaHeight != metadataChangeInto.mediaHeight) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Media Hieght", metadataOriginal.mediaHeight, metadataChangeInto.mediaHeight);
            if (metadataOriginal.mediaOrientation != metadataChangeInto.mediaOrientation) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Media Orientation", metadataOriginal.mediaOrientation, metadataChangeInto.mediaOrientation);
            if (metadataOriginal.mediaVideoLength != metadataChangeInto.mediaVideoLength) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Media Video lenth", metadataOriginal.mediaVideoLength, metadataChangeInto.mediaVideoLength);
            #endregion

            #region Location
            if (metadataOriginal.locationAltitude != metadataChangeInto.locationAltitude) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Location Altitude", metadataOriginal.locationAltitude, metadataChangeInto.locationAltitude);
            if (metadataOriginal.locationLatitude != metadataChangeInto.locationLatitude) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Location Latitude", metadataOriginal.locationLatitude, metadataChangeInto.locationLatitude);
            if (metadataOriginal.locationLongitude != metadataChangeInto.locationLongitude) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Location Longitude", metadataOriginal.locationLongitude, metadataChangeInto.locationLongitude);
            if (metadataOriginal.locationDateTime != metadataChangeInto.locationDateTime) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Location DateTime", metadataOriginal.locationDateTime, metadataChangeInto.locationDateTime);
            if (metadataOriginal.locationName != metadataChangeInto.locationName) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Location Name", metadataOriginal.locationName, metadataChangeInto.locationName);
            if (metadataOriginal.locationCountry != metadataChangeInto.locationCountry) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Location Country", metadataOriginal.locationCountry, metadataChangeInto.locationCountry);
            if (metadataOriginal.locationCity != metadataChangeInto.locationCity) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Location District", metadataOriginal.locationCity, metadataChangeInto.locationCity);
            if (metadataOriginal.locationState != metadataChangeInto.locationState) errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + AddError("Location Region", metadataOriginal.locationState, metadataChangeInto.locationState);
            #endregion

            #region personalRegionList
            string notAdded = (whatsAddedChanged ? " - Was added by user" : " - Was not added the metadata in file");
            string notRemoved = (whatsAddedChanged ? " - Was removed by user" : " - Not not removed metadata in file");
            string vierfied = " Verified OK";
            if (VerifyRegionStructureList(metadataOriginal.personalRegionList, metadataChangeInto.personalRegionList) == false)
            {
                List<RegionStructure> allRegions = new List<RegionStructure>();
                foreach (RegionStructure region in metadataOriginal.personalRegionList) if (!region.DoesThisRectangleAndNameExistInList(allRegions)) allRegions.Add(region);
                foreach (RegionStructure region in metadataChangeInto.personalRegionList) if (!region.DoesThisRectangleAndNameExistInList(allRegions)) allRegions.Add(region);

                errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + "\r\nRegion list\r\n";
                foreach (RegionStructure region in allRegions)
                    if (!whatsAddedChanged || (whatsAddedChanged && (!region.DoesThisRectangleAndNameExistInList(metadataOriginal.personalRegionList) || !region.DoesThisRectangleAndNameExistInList(metadataChangeInto.personalRegionList))))
                    errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + (region == null ? "" : region.ToErrorText()) +
                        (region.DoesThisRectangleAndNameExistInList(metadataOriginal.personalRegionList) && region.DoesThisRectangleAndNameExistInList(metadataChangeInto.personalRegionList) ? vierfied :
                        (region.DoesThisRectangleAndNameExistInList(metadataOriginal.personalRegionList) ? notAdded : notRemoved)) +
                        "\r\n";
            }
            #endregion

            #region personalTagList
            if (VerifyKeywordList(metadataOriginal.personalTagList, metadataChangeInto.personalTagList) == false)
            {

                List<KeywordTag> allTags = new List<KeywordTag>();
                foreach (KeywordTag tag in metadataOriginal.PersonalKeywordTags) if (!allTags.Contains(tag)) allTags.Add(tag);
                foreach (KeywordTag tag in metadataChangeInto.PersonalKeywordTags) if (!allTags.Contains(tag)) allTags.Add(tag);

                errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + "\r\nKeyword list\r\n";
                foreach (KeywordTag tag in allTags)
                    if (!whatsAddedChanged || (whatsAddedChanged && (!metadataOriginal.PersonalKeywordTags.Contains(tag) || !metadataChangeInto.PersonalKeywordTags.Contains(tag))))
                    errors += (string.IsNullOrWhiteSpace(errors) ? fileInformation : "") + (tag == null ? "" : tag.ToString()) +
                        (metadataOriginal.PersonalKeywordTags.Contains(tag) && metadataChangeInto.PersonalKeywordTags.Contains(tag) ? vierfied :
                        (metadataOriginal.PersonalKeywordTags.Contains(tag) ? notAdded : notRemoved)) +
                        "\r\n";
            }
            #endregion

            return errors;
        }
        #endregion

        #endregion Errors handler


        #region ExiftoolWriterBuilder - AddLine
        private string ExiftoolWriterBuilderAddLine(string line, string variable, bool alwaysWrite, Metadata metadata, ref bool vaiableFound)
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
        #endregion

        #region ExiftoolWriterBuilder - RemoveLinesNotChanged
        public string ExiftoolWriterBuilderRemoveLinesNotChanged(string stringWithVariables, Metadata metadata, bool alwaysWrite)
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
                    #region File
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfFileDateModifiedChanged}", alwaysWrite, metadata, ref vaiableFound);
                    #endregion

                    #region FileName/Folder/Path
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfFilePathChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfFileNameChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfFileDirectoryChanged}", alwaysWrite, metadata, ref vaiableFound);
                    #endregion

                    #region Personal
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfPersonalTitleChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfPersonalDescriptionChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfPersonalCommentsChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfPersonalRatingChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfPersonalAuthorChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfPersonalAlbumChanged}", alwaysWrite, metadata, ref vaiableFound);
                    #endregion

                    #region  Region
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfPersonalRegionChanged}", alwaysWrite, metadata, ref vaiableFound);
                    #endregion

                    #region Keyword
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfPersonalKeywordsChanged}", alwaysWrite, metadata, ref vaiableFound);
                    #endregion

                    #region Media
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfMediaDateTakenChanged}", alwaysWrite, metadata, ref vaiableFound);
                    #endregion

                    #region Location
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfLocationChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfLocationAltitudeChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfLocationLatitudeChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfLocationLongitudeChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfLocationDateTimeChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfLocationNameChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfLocationCityChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfLocationStateChanged}", alwaysWrite, metadata, ref vaiableFound);
                    addLine = ExiftoolWriterBuilderAddLine(addLine, "{IfLocationCountryChanged}", alwaysWrite, metadata, ref vaiableFound);
                    #endregion
                } while (vaiableFound);
                if (!string.IsNullOrWhiteSpace(addLine)) result += addLine + "\r\n";
            }
            return result;
        }
        #endregion

        #region Variables - List if Variable 
        public static string[] ListOfPropertiesCombined(bool addKeywordItems)
        {
            List<string> listOfPropertiesWrittenByUser = ListOfPropertiesWrittenByUser(addKeywordItems);
            List<string> listOfPropertiesOriginal = ListOfPropertiesOriginal(addKeywordItems);
            return listOfPropertiesWrittenByUser.Union(listOfPropertiesOriginal).ToArray();
        }

        public static List<string> ListOfPropertiesWrittenByUser(bool addKeywordItems)
        {
            List<string> listOfProperties = new List<string>();

            #region SystemDateTime
            listOfProperties.Add("{SystemDateTime}");
            listOfProperties.Add("{SystemDateTimeDateStamp}");
            listOfProperties.Add("{SystemDateTimeTimeStamp}");
            listOfProperties.Add("{SystemDateTime_yyyy}");
            listOfProperties.Add("{SystemDateTime_MM}");
            listOfProperties.Add("{SystemDateTime_dd}");
            listOfProperties.Add("{SystemDateTime_HH}");
            listOfProperties.Add("{SystemDateTime_mm}");
            listOfProperties.Add("{SystemDateTime_ss}");
            #endregion

            #region FileName/Folder/Path
            listOfProperties.Add("{IfFileNameChanged}");
            listOfProperties.Add("{FileName}");
            listOfProperties.Add("{IfFilePathChanged}");
            listOfProperties.Add("{FileFullPath}");
            listOfProperties.Add("{FileFullPath.8.3}");

            listOfProperties.Add("{FileNameWithoutExtension}");
            listOfProperties.Add("{FileNameWithoutExtensionDateTime}");
            listOfProperties.Add("{FileNameWithoutExtensionDateTimeComputerName}");
            listOfProperties.Add("{FileNameWithoutExtensionDateTimeGPStag}");
            listOfProperties.Add("{FileNameWithoutExtensionDateTimeComputerNameGPStag}");
            listOfProperties.Add("{FileNameWithoutExtensionComputerName}");
            listOfProperties.Add("{FileNameWithoutExtensionComputerNameGPStag}");
            listOfProperties.Add("{FileNameWithoutExtensionGPStag}");
            listOfProperties.Add("{FileNameWithoutDateTime}");
            listOfProperties.Add("{FileNameWithoutDateTimeComputerName}");
            listOfProperties.Add("{FileNameWithoutDateTimeGPStag}");
            listOfProperties.Add("{FileNameWithoutDateTimeComputerNameGPStag}");
            listOfProperties.Add("{FileNameWithoutComputerName}");
            listOfProperties.Add("{FileNameWithoutComputerNameGPStag}");
            listOfProperties.Add("{FileNameWithoutGPStag}");

            listOfProperties.Add("{FileExtension}");
            listOfProperties.Add("{IfFileDirectoryChanged}");
            listOfProperties.Add("{FileDirectory}");
            #endregion

            #region FileAttributes
            listOfProperties.Add("{FileSize}");
            listOfProperties.Add("{FileMimeType}");
            #endregion

            #region FileDate
            listOfProperties.Add("{FileDate}");
            listOfProperties.Add("{FileDateDateStamp}");
            listOfProperties.Add("{FileDateTimeStamp}");
            listOfProperties.Add("{FileDate_yyyy}");
            listOfProperties.Add("{FileDate_MM}");
            listOfProperties.Add("{FileDate_dd}");
            listOfProperties.Add("{FileDate_HH}");
            listOfProperties.Add("{FileDate_mm}");
            listOfProperties.Add("{FileDate_ss}");
            #endregion

            #region FileDateCreated
            listOfProperties.Add("{FileDateCreated}");
            listOfProperties.Add("{FileDateCreatedDateStamp}");
            listOfProperties.Add("{FileDateCreatedTimeStamp}");
            listOfProperties.Add("{FileDateCreated_yyyy}");
            listOfProperties.Add("{FileDateCreated_MM}");
            listOfProperties.Add("{FileDateCreated_dd}");
            listOfProperties.Add("{FileDateCreated_HH}");
            listOfProperties.Add("{FileDateCreated_mm}");
            listOfProperties.Add("{FileDateCreated_ss}");
            #endregion

            #region FileDateModified
            listOfProperties.Add("{IfFileDateModifiedChanged}");
            listOfProperties.Add("{FileDateModified}");
            listOfProperties.Add("{FileDateModifiedDateStamp}");
            listOfProperties.Add("{FileDateModifiedTimeStamp}");
            listOfProperties.Add("{FileDateModified_yyyy}");
            listOfProperties.Add("{FileDateModified_MM}");
            listOfProperties.Add("{FileDateModified_dd}");
            listOfProperties.Add("{FileDateModified_HH}");
            listOfProperties.Add("{FileDateModified_mm}");
            listOfProperties.Add("{FileDateModified_ss}");
            #endregion 

            #region Media MediaDateTaken
            listOfProperties.Add("{IfMediaDateTakenChanged}");
            listOfProperties.Add("{MediaDateTaken}");
            listOfProperties.Add("{MediaDateTakenDateStamp}");
            listOfProperties.Add("{MediaDateTakenTimeStamp}");
            listOfProperties.Add("{MediaDateTaken_yyyy}");
            listOfProperties.Add("{MediaDateTaken_MM}");
            listOfProperties.Add("{MediaDateTaken_dd}");
            listOfProperties.Add("{MediaDateTaken_HH}");
            listOfProperties.Add("{MediaDateTaken_mm}");
            listOfProperties.Add("{MediaDateTaken_ss}");
            listOfProperties.Add("{MediaHeight}");
            listOfProperties.Add("{MediaOrientation}");
            listOfProperties.Add("{MediaVideoLength}");
            #endregion

            #region Personal
            listOfProperties.Add("{IfPersonalTitleChanged}");
            listOfProperties.Add("{PersonalTitle}");            
            listOfProperties.Add("{IfPersonalDescriptionChanged}");
            listOfProperties.Add("{PersonalDescription}");
            listOfProperties.Add("{IfPersonalCommentsChanged}");
            listOfProperties.Add("{PersonalComments}");
            listOfProperties.Add("{IfPersonalRatingChanged}");
            listOfProperties.Add("{PersonalRating}");            
            listOfProperties.Add("{PersonalRatingPercent}");
            listOfProperties.Add("{PersonalAuthor}");
            listOfProperties.Add("{IfPersonalAuthorChanged}");
            listOfProperties.Add("{IfPersonalAlbumChanged}");
            listOfProperties.Add("{PersonalAlbum}");            
            #endregion

            #region Region
            listOfProperties.Add("{IfPersonalRegionChanged}");
            listOfProperties.Add("{PersonalRegionInfoMP}");
            listOfProperties.Add("{PersonalRegionInfo}");
            #endregion

            #region Keyword
            if (addKeywordItems) listOfProperties.Add("{KeywordItem}");
            listOfProperties.Add("{IfPersonalKeywordsChanged}");
            listOfProperties.Add("{PersonalKeywordsList}");
            listOfProperties.Add("{PersonalKeywordsXML}");
            listOfProperties.Add("{PersonalKeywordItemsDelete}");
            listOfProperties.Add("{PersonalKeywordItemsAdd}");
            #endregion

            #region Camera
            listOfProperties.Add("{CameraMake}");
            listOfProperties.Add("{CameraModel}");
            #endregion

            #region Location
            listOfProperties.Add("{IfLocationChanged}");
            listOfProperties.Add("{IfLocationAltitudeChanged}");
            listOfProperties.Add("{LocationAltitude}");
            listOfProperties.Add("{IfLocationLatitudeChanged}"); 
            listOfProperties.Add("{LocationLatitude}");
            listOfProperties.Add("{IfLocationLongitudeChanged}");
            listOfProperties.Add("{LocationLongitude}");
            listOfProperties.Add("{IfLocationDateTimeChanged}"); 
            listOfProperties.Add("{LocationDateTime}");
            listOfProperties.Add("{LocationDateTimeUTC}");
            listOfProperties.Add("{LocationDateTimeDateStamp}");
            listOfProperties.Add("{LocationDateTimeTimeStamp}");
            listOfProperties.Add("{LocationDateTime_yyyy}");
            listOfProperties.Add("{LocationDateTime_MM}");
            listOfProperties.Add("{LocationDateTime_dd}");
            listOfProperties.Add("{LocationDateTime_HH}");
            listOfProperties.Add("{LocationDateTime_mm}");
            listOfProperties.Add("{LocationDateTime_ss}");
            listOfProperties.Add("{IfLocationNameChanged}");
            listOfProperties.Add("{LocationName}");
            listOfProperties.Add("{IfLocationCityChanged}");
            listOfProperties.Add("{LocationCity}");
            listOfProperties.Add("{IfLocationStateChanged}");
            listOfProperties.Add("{LocationState}");
            listOfProperties.Add("{IfLocationCountryChanged}");
            listOfProperties.Add("{LocationCountry}");
            #endregion

            return listOfProperties; 
        }

        public static List<string> ListOfPropertiesOriginal(bool addKeywordItems, bool addIf = true)
        {
            List<string> listOfProperties = new List<string>();

            #region FileName/Folder/Path
            if (addIf) listOfProperties.Add("{IfFileNameChanged}");
            listOfProperties.Add("{OriginalFileName}");

            if (addIf) listOfProperties.Add("{IfFilePathChanged}");
            listOfProperties.Add("{OriginalFileFullPath}");
            listOfProperties.Add("{OriginalFileFullPath.8.3}");

            listOfProperties.Add("{OriginalFileNameWithoutExtension}");
            listOfProperties.Add("{OriginalFileNameWithoutExtensionDateTime}");
            listOfProperties.Add("{OriginalFileNameWithoutExtensionDateTimeComputerName}");
            listOfProperties.Add("{OriginalFileNameWithoutExtensionDateTimeGPStag}");
            listOfProperties.Add("{OriginalFileNameWithoutExtensionDateTimeComputerNameGPStag}");
            listOfProperties.Add("{OriginalFileNameWithoutExtensionComputerName}");
            listOfProperties.Add("{OriginalFileNameWithoutExtensionComputerNameGPStag}");
            listOfProperties.Add("{OriginalFileNameWithoutExtensionGPStag}");
            listOfProperties.Add("{OriginalFileNameWithoutDateTime}");
            listOfProperties.Add("{OriginalFileNameWithoutDateTimeComputerName}");
            listOfProperties.Add("{OriginalFileNameWithoutDateTimeGPStag}");
            listOfProperties.Add("{OriginalFileNameWithoutDateTimeComputerNameGPStag}");
            listOfProperties.Add("{OriginalFileNameWithoutComputerName}");
            listOfProperties.Add("{OriginalFileNameWithoutComputerNameGPStag}");
            listOfProperties.Add("{OriginalFileNameWithoutGPStag}");

            listOfProperties.Add("{OriginalFileExtension}");

            if (addIf) listOfProperties.Add("{IfFileDirectoryChanged}");
            listOfProperties.Add("{OriginalFileDirectory}");
            #endregion

            #region FileAttributes
            
            #endregion

            #region FileDate
            listOfProperties.Add("{OriginalFileDate}");
            listOfProperties.Add("{OriginalFileDateDateStamp}");
            listOfProperties.Add("{OriginalFileDateTimeStamp}");
            listOfProperties.Add("{OriginalFileDate_yyyy}");
            listOfProperties.Add("{OriginalFileDate_MM}");
            listOfProperties.Add("{OriginalFileDate_dd}");
            listOfProperties.Add("{OriginalFileDate_HH}");
            listOfProperties.Add("{OriginalFileDate_mm}");
            listOfProperties.Add("{OriginalFileDate_ss}");
            #endregion

            #region FileDateCreated
            listOfProperties.Add("{OriginalFileDateCreated}");
            listOfProperties.Add("{OriginalFileDateCreatedDateStamp}");
            listOfProperties.Add("{OriginalFileDateCreatedTimeStamp}");
            listOfProperties.Add("{OriginalFileDateCreated_yyyy}");
            listOfProperties.Add("{OriginalFileDateCreated_MM}");
            listOfProperties.Add("{OriginalFileDateCreated_dd}");
            listOfProperties.Add("{OriginalFileDateCreated_HH}");
            listOfProperties.Add("{OriginalFileDateCreated_mm}");
            listOfProperties.Add("{OriginalFileDateCreated_ss}");
            #endregion

            #region FileDateModified
            if (addIf) listOfProperties.Add("{IfFileDateModifiedChanged}");
            listOfProperties.Add("{OriginalFileDateModified}");
            listOfProperties.Add("{OriginalFileDateModifiedDateStamp}");
            listOfProperties.Add("{OriginalFileDateModifiedTimeStamp}");
            listOfProperties.Add("{OriginalFileDateModified_yyyy}");
            listOfProperties.Add("{OriginalFileDateModified_MM}");
            listOfProperties.Add("{OriginalFileDateModified_dd}");
            listOfProperties.Add("{OriginalFileDateModified_HH}");
            listOfProperties.Add("{OriginalFileDateModified_mm}");
            listOfProperties.Add("{OriginalFileDateModified_ss}");
            #endregion 

            #region Media MediaDateTaken
            if (addIf) listOfProperties.Add("{IfMediaDateTakenChanged}");
            listOfProperties.Add("{OriginalMediaDateTaken}");
            listOfProperties.Add("{OriginalMediaDateTakenDateStamp}");
            listOfProperties.Add("{OriginalMediaDateTakenTimeStamp}");
            listOfProperties.Add("{OriginalMediaDateTaken_yyyy}");
            listOfProperties.Add("{OriginalMediaDateTaken_MM}");
            listOfProperties.Add("{OriginalMediaDateTaken_dd}");
            listOfProperties.Add("{OriginalMediaDateTaken_HH}");
            listOfProperties.Add("{OriginalMediaDateTaken_mm}");
            listOfProperties.Add("{OriginalMediaDateTaken_ss}");
            listOfProperties.Add("{OriginalMediaWidth}");
            listOfProperties.Add("{OriginalMediaHeight}");
            listOfProperties.Add("{OriginalMediaOrientation}");
            listOfProperties.Add("{OriginalMediaVideoLength}");
            #endregion

            #region Personal
            if (addIf) listOfProperties.Add("{IfPersonalTitleChanged}");
            listOfProperties.Add("{OriginalPersonalTitle}");

            if (addIf) listOfProperties.Add("{IfPersonalDescriptionChanged}");
            listOfProperties.Add("{OriginalPersonalDescription}");

            if (addIf) listOfProperties.Add("{IfPersonalCommentsChanged}");
            listOfProperties.Add("{OriginalPersonalComments}");

            if (addIf) listOfProperties.Add("{IfPersonalRatingChanged}");
            listOfProperties.Add("{OriginalPersonalRating}"); ;
            listOfProperties.Add("{OriginalPersonalRatingPercent}");

            if (addIf) listOfProperties.Add("{IfPersonalAuthorChanged}");
            listOfProperties.Add("{OriginalPersonalAuthor}");

            if (addIf) listOfProperties.Add("{IfPersonalAlbumChanged}");
            listOfProperties.Add("{OriginalPersonalAlbum}");
            #endregion

            #region Region
            if (addIf) listOfProperties.Add("{IfPersonalRegionChanged}");
            listOfProperties.Add("{OriginalPersonalRegionInfoMP}");
            listOfProperties.Add("{OriginalPersonalRegionInfo}");
            #endregion

            #region Keyword
            if (addKeywordItems) listOfProperties.Add("{OriginalKeywordItem}");
            if (addIf) listOfProperties.Add("{IfPersonalKeywordsChanged}");
            listOfProperties.Add("{OriginalPersonalKeywordsList}");
            listOfProperties.Add("{OriginalPersonalKeywordsXML}");
            listOfProperties.Add("{OriginalPersonalKeywordItemsDelete}");
            listOfProperties.Add("{OriginalPersonalKeywordItemsAdd}");
            #endregion

            #region Camera
            listOfProperties.Add("{OriginalCameraMake}");
            listOfProperties.Add("{OriginalCameraModel}");
            #endregion

            #region Location
            if (addIf) listOfProperties.Add("{IfLocationChanged}");

            if (addIf) listOfProperties.Add("{IfLocationAltitudeChanged}");
            listOfProperties.Add("{OriginalLocationAltitude}");

            if (addIf) listOfProperties.Add("{IfLocationLatitudeChanged}");
            listOfProperties.Add("{OriginalLocationLatitude}");

            if (addIf) listOfProperties.Add("{IfLocationLongitudeChanged}");
            listOfProperties.Add("{OriginalLocationLongitude}");

            if (addIf) listOfProperties.Add("{IfLocationDateTimeChanged}");
            listOfProperties.Add("{OriginalLocationDateTime}");

            if (addIf) listOfProperties.Add("{OriginalLocationDateTimeUTC}");
            listOfProperties.Add("{OriginalLocationDateTimeDateStamp}");
            listOfProperties.Add("{OriginalLocationDateTimeTimeStamp}");
            listOfProperties.Add("{OriginalLocationDateTime_yyyy}");
            listOfProperties.Add("{OriginalLocationDateTime_MM}");
            listOfProperties.Add("{OriginalLocationDateTime_dd}");
            listOfProperties.Add("{OriginalLocationDateTime_HH}");
            listOfProperties.Add("{OriginalLocationDateTime_mm}");
            listOfProperties.Add("{OriginalLocationDateTime_ss}");

            if (addIf) listOfProperties.Add("{IfLocationNameChanged}");
            listOfProperties.Add("{OriginalLocationName}");

            if (addIf) listOfProperties.Add("{IfLocationCityChanged}");
            listOfProperties.Add("{OriginalLocationCity}");

            if (addIf) listOfProperties.Add("{IfLocationStateChanged}");
            listOfProperties.Add("{OriginalLocationState}");

            if (addIf) listOfProperties.Add("{IfLocationCountryChanged}");
            listOfProperties.Add("{OriginalLocationCountry}");
            #endregion

            return listOfProperties; 
        }
        #endregion Variables - List if Variable

        #region Variables - GetPropertyValueWrittenByUser
        public string GetPropertyValueWrittenByUser(string variableName, bool useExifFormat, bool convertNullToBlank,
            List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag,
            string personalRegionInfoMP, string personalRegionInfo, string personalKeywordList, string personalKeywordsXML, string personalKeywordItemsAdd)
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
                case "{FileFullPath}":
                    result = FileFullPath;
                    break;
                case "{FileFullPath.8.3}":
                    result = NativeMethods.ShortFileName(FileFullPath);
                    break;

                #region WithoutExtension
                case "{FileNameWithoutExtension}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    break;
                case "{FileNameWithoutExtensionDateTime}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    break;
                case "{FileNameWithoutExtensionDateTimeComputerName}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{FileNameWithoutExtensionDateTimeGPStag}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    result = result.Replace(GPStag, "");
                    break;
                case "{FileNameWithoutExtensionDateTimeComputerNameGPStag}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    result = result.Replace(GPStag, "");
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{FileNameWithoutExtensionComputerName}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{FileNameWithoutExtensionComputerNameGPStag}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = result.Replace(GPStag, "");
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{FileNameWithoutExtensionGPStag}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = result.Replace(GPStag, "");
                    break;
                #endregion

                #region WithoutDateTime
                case "{FileNameWithoutDateTime}":
                    result = FileName;
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    break;
                case "{FileNameWithoutDateTimeComputerName}":
                    result = FileName;
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{FileNameWithoutDateTimeGPStag}":
                    result = FileName;
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    result = result.Replace(GPStag, "");
                    break;
                case "{FileNameWithoutDateTimeComputerNameGPStag}":
                    result = FileName;
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    result = result.Replace(GPStag, "");
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                #endregion

                #region WithoutComputerName
                case "{FileNameWithoutComputerName}":
                    result = FileName;
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{FileNameWithoutComputerNameGPStag}":
                    result = FileName;
                    result = result.Replace(GPStag, "");
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                #endregion

                #region WithoutGPStag
                case "{FileNameWithoutGPStag}":
                    result = FileName;
                    result = result.Replace(GPStag, "");
                    break;
                #endregion

                case "{FileExtension}":
                    result = Path.GetExtension(FileName);
                    break;
                case "{FileDirectory}":
                    result = FileDirectory;
                    break;
                case "{FileSize}":
                    result = FileSize == null ? null : ((long)FileSize).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{FileDate}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(FileDate);
                    else result = TimeZoneLibrary.ToStringFilename(FileDate);
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
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(FileDateAccessed);
                    else result = TimeZoneLibrary.ToStringFilename(FileDateAccessed);
                    break;
                case "{FileLastAccessedDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(FileDateAccessed);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(FileDateAccessed);
                    break;
                case "{FileLastAccessedTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(FileDateAccessed);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(FileDateAccessed);
                    break;
                case "{FileLastAccessed_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(FileDateAccessed);
                    break;
                case "{FileLastAccessed_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(FileDateAccessed);
                    break;
                case "{FileLastAccessed_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(FileDateAccessed);
                    break;
                case "{FileLastAccessed_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(FileDateAccessed);
                    break;
                case "{FileLastAccessed_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(FileDateAccessed);
                    break;
                case "{FileLastAccessed_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(FileDateAccessed);
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
                #endregion

                #region Keyword
                case "{PersonalKeywordsList}":
                    result = personalKeywordList;
                    break;
                case "{PersonalKeywordsXML}":
                    result = personalKeywordsXML;
                    break;
                //case "{PersonalKeywordItemsDelete}":
                case "{PersonalKeywordItemsAdd}":
                    result = personalKeywordItemsAdd;
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
                default:
                    throw new Exception("Theres a been use on none existing variable in config for write metadata: '"+ variableName + "' that is not implemented");
                    
            }
            if (convertNullToBlank && result == null) result = "";
            return result;
        }
        #endregion Variables - GetPropertyValueWrittenByUser

        #region Variables - GetPropertyValueOriginal
        public string GetPropertyValueOriginal(string variableName, bool useExifFormat, bool convertNullToBlank,
            List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag,
            string personalRegionInfoMP, string personalRegionInfo, string personalKeywordList, string personalKeywordsXML, string personalKeywordItemsAdd)
        {
            string result = variableName;
            DateTime dateTimeSystem = DateTime.Now;
            FileDateTimeReader fileDateTimeFormats = new FileDateTimeReader(allowedFileNameDateTimeFormats);
            switch (variableName)
            {
                #region System
                case "{OriginalSystemDateTime}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(dateTimeSystem);
                    else result = TimeZoneLibrary.ToStringFilename(dateTimeSystem);
                    break;
                case "{OriginalSystemDateTimeDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(dateTimeSystem);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(dateTimeSystem);
                    break;
                case "{OriginalSystemDateTimeTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(dateTimeSystem);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(dateTimeSystem);
                    break;
                case "{OriginalSystemDateTime_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(dateTimeSystem);
                    break;
                case "{OriginalSystemDateTime_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(dateTimeSystem);
                    break;
                case "{OriginalSystemDateTime_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(dateTimeSystem);
                    break;
                case "{OriginalSystemDateTime_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(dateTimeSystem);
                    break;
                case "{OriginalSystemDateTime_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(dateTimeSystem);
                    break;
                case "{OriginalSystemDateTime_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(dateTimeSystem);
                    break;
                #endregion System

                #region Filesystem
                case "{OriginalFileName}":
                    result = FileName;
                    break;
                case "{OriginalFileFullPath}":
                    result = FileFullPath;
                    break;
                case "{OriginalFileFullPath.8.3}":
                    result = NativeMethods.ShortFileName(FileFullPath);
                    break;

                #region WithoutExtension
                case "{OriginalFileNameWithoutExtension}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    break;
                case "{OriginalFileNameWithoutExtensionDateTime}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    break;
                case "{OriginalFileNameWithoutExtensionDateTimeComputerName}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{OriginalFileNameWithoutExtensionDateTimeGPStag}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    result = result.Replace(GPStag, "");
                    break;
                case "{OriginalFileNameWithoutExtensionDateTimeComputerNameGPStag}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    result = result.Replace(GPStag, "");
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{OriginalFileNameWithoutExtensionComputerName}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{OriginalFileNameWithoutExtensionComputerNameGPStag}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = result.Replace(GPStag, "");
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{OriginalFileNameWithoutExtensionGPStag}":
                    result = Path.GetFileNameWithoutExtension(FileName);
                    result = result.Replace(GPStag, "");
                    break;
                #endregion

                #region WithoutDateTime
                case "{OriginalFileNameWithoutDateTime}":
                    result = FileName;
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    break;
                case "{OriginalFileNameWithoutDateTimeComputerName}":
                    result = FileName;
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{OriginalFileNameWithoutDateTimeGPStag}":
                    result = FileName;
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    result = result.Replace(GPStag, "");
                    break;
                case "{OriginalFileNameWithoutDateTimeComputerNameGPStag}":
                    result = FileName;
                    result = fileDateTimeFormats.RemoveAllDateTimes(Path.GetFileNameWithoutExtension(result));
                    result = result.Replace(GPStag, "");
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                #endregion

                #region WithoutComputerName
                case "{OriginalFileNameWithoutComputerName}":
                    result = FileName;
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                case "{OriginalFileNameWithoutComputerNameGPStag}":
                    result = FileName;
                    result = result.Replace(GPStag, "");
                    foreach (string computerName in computerNames) result = result.Replace(computerName, "");
                    break;
                #endregion

                #region WithoutGPStag
                case "{OriginalFileNameWithoutGPStag}":
                    result = FileName;
                    result = result.Replace(GPStag, "");
                    break;
                #endregion

 
                case "{OriginalFileExtension}":
                    result = Path.GetExtension(FileName);
                    break;
                case "{OriginalFileDirectory}":
                    result = FileDirectory;
                    break;
                case "{OriginalFileSize}":
                    result = FileSize == null ? null : ((long)FileSize).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{FileDate}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(FileDate);
                    else result = TimeZoneLibrary.ToStringFilename(FileDate);
                    break;
                case "{OriginalFileDateCreated}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(FileDateCreated);
                    else result = TimeZoneLibrary.ToStringFilename(FileDateCreated);
                    break;
                case "{OriginalFileDateCreatedDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(FileDateCreated);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(FileDateCreated);
                    break;
                case "{OriginalFileDateCreatedTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(FileDateCreated);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(FileDateCreated);
                    break;
                case "{OriginalFileDateCreated_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(FileDateCreated);
                    break;
                case "{OriginalFileDateCreated_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(FileDateCreated);
                    break;
                case "{OriginalFileDateCreated_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(FileDateCreated);
                    break;
                case "{OriginalFileDateCreated_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(FileDateCreated);
                    break;
                case "{OriginalFileDateCreated_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(FileDateCreated);
                    break;
                case "{OriginalFileDateCreated_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(FileDateCreated);
                    break;
                case "{OriginalFileDateModified}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(FileDateModified);
                    else result = TimeZoneLibrary.ToStringFilename(FileDateModified);
                    break;
                case "{OriginalFileDateModifiedDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(FileDateModified);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(FileDateModified);
                    break;
                case "{OriginalFileDateModifiedTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(FileDateModified);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(FileDateModified);
                    break;
                case "{OriginalFileDateModified_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(FileDateModified);
                    break;
                case "{OriginalFileDateModified_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(FileDateModified);
                    break;
                case "{OriginalFileDateModified_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(FileDateModified);
                    break;
                case "{OriginalFileDateModified_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(FileDateModified);
                    break;
                case "{OriginalFileDateModified_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(FileDateModified);
                    break;
                case "{OriginalFileDateModified_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(FileDateModified);
                    break;
                case "{OriginalFileLastAccessed}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(FileDateAccessed);
                    else result = TimeZoneLibrary.ToStringFilename(FileDateAccessed);
                    break;
                case "{OriginalFileLastAccessedDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(FileDateAccessed);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(FileDateAccessed);
                    break;
                case "{OriginalFileLastAccessedTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(FileDateAccessed);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(FileDateAccessed);
                    break;
                case "{OriginalFileLastAccessed_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(FileDateAccessed);
                    break;
                case "{OriginalFileLastAccessed_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(FileDateAccessed);
                    break;
                case "{OriginalFileLastAccessed_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(FileDateAccessed);
                    break;
                case "{OriginalFileLastAccessed_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(FileDateAccessed);
                    break;
                case "{OriginalFileLastAccessed_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(FileDateAccessed);
                    break;
                case "{OriginalFileLastAccessed_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(FileDateAccessed);
                    break;
                case "{OriginalFileMimeType}":
                    result = FileMimeType;
                    break;
                #endregion Filesystem

                #region Personal
                case "{OriginalPersonalTitle}":
                    result = PersonalTitle;
                    break;
                case "{OriginalPersonalDescription}":
                    result = PersonalDescription;
                    break;
                case "{OriginalPersonalComments}":
                    result = PersonalComments;
                    break;
                case "{OriginalPersonalRating}":
                    result = PersonalRating == null ? null : ((byte)PersonalRating).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{OriginalPersonalRatingPercent}":
                    result = PersonalRatingPercent == null ? null : ((byte)PersonalRatingPercent).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{OriginalPersonalAuthor}":
                    result = PersonalAuthor;
                    break;
                case "{OriginalPersonalAlbum}":
                    result = PersonalAlbum;
                    break;
                #endregion Personal

                #region Face Region
                case "{OriginalPersonalRegionInfoMP}":
                    result = personalRegionInfoMP;
                    break;
                case "{OriginalPersonalRegionInfo}":
                    result = personalRegionInfo;
                    break;
                #endregion Face Region

                #region Keyword
                case "{OriginalPersonalKeywordsList}":
                    result = personalKeywordList;
                    break;
                case "{OriginalPersonalKeywordsXML}":
                    result = personalKeywordsXML;
                    break;
                case "{OriginalPersonalKeywordItemsAdd}":
                    result = personalKeywordItemsAdd;
                    break;
                #endregion Keyword

                #region Camera
                case "{OriginalCameraMake}":
                    result = CameraMake;
                    break;
                case "{OriginalCameraModel}":
                    result = CameraModel;
                    break;
                #endregion Camera

                #region Media
                case "{OriginalMediaDateTaken}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(MediaDateTaken);
                    else result = TimeZoneLibrary.ToStringFilename(MediaDateTaken);
                    break;
                case "{OriginalMediaDateTakenDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(MediaDateTaken);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(MediaDateTaken);
                    break;
                case "{OriginalMediaDateTakenTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(MediaDateTaken);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(MediaDateTaken);
                    break;
                case "{OriginalMediaDateTaken_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(MediaDateTaken);
                    break;
                case "{OriginalMediaDateTaken_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(MediaDateTaken);
                    break;
                case "{OriginalMediaDateTaken_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(MediaDateTaken);
                    break;
                case "{OriginalMediaDateTaken_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(MediaDateTaken);
                    break;
                case "{OriginalMediaDateTaken_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(MediaDateTaken);
                    break;
                case "{OriginalMediaDateTaken_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(MediaDateTaken);
                    break;
                case "{OriginalMediaWidth}":
                    result = MediaWidth == null ? null : ((int)MediaWidth).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{OriginalMediaHeight}":
                    result = MediaHeight == null ? null : ((int)MediaHeight).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{OriginalMediaOrientation}":
                    result = MediaOrientation == null ? null : ((int)MediaOrientation).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{OriginalMediaVideoLength}":
                    result = MediaVideoLength == null ? null : ((int)MediaVideoLength).ToString(CultureInfo.InvariantCulture);
                    break;
                #endregion Media

                #region Location
                case "{OriginalLocationAltitude}":
                    result = LocationAltitude == null ? null : ((float)LocationAltitude).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{OriginalLocationLatitude}":
                    result = LocationLatitude == null ? null : ((float)LocationLatitude).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{OriginalLocationLongitude}":
                    result = LocationLongitude == null ? null : ((float)LocationLongitude).ToString(CultureInfo.InvariantCulture);
                    break;
                case "{OriginalLocationDateTime}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftool(LocationDateTime);
                    else result = TimeZoneLibrary.ToStringFilename(LocationDateTime);
                    break;
                case "{OriginalLocationDateTimeUTC}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolUTC(LocationDateTime);
                    else result = TimeZoneLibrary.ToStringFilenameUTC(LocationDateTime);
                    break;
                case "{OriginalLocationDateTimeDateStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolDateStamp(LocationDateTime);
                    else result = TimeZoneLibrary.ToStringFilenameDateStamp(LocationDateTime);
                    break;
                case "{OriginalLocationDateTimeTimeStamp}":
                    if (useExifFormat) result = TimeZoneLibrary.ToStringExiftoolTimeStamp(LocationDateTime);
                    else result = TimeZoneLibrary.ToStringFilenameTimeStamp(LocationDateTime);
                    break;
                case "{OriginalLocationDateTime_yyyy}":
                    result = TimeZoneLibrary.ToStringDateTime_yyyy(LocationDateTime);
                    break;
                case "{OriginalLocationDateTime_MM}":
                    result = TimeZoneLibrary.ToStringDateTime_MM(LocationDateTime);
                    break;
                case "{OriginalLocationDateTime_dd}":
                    result = TimeZoneLibrary.ToStringDateTime_dd(LocationDateTime);
                    break;
                case "{OriginalLocationDateTime_HH}":
                    result = TimeZoneLibrary.ToStringDateTime_HH(LocationDateTime);
                    break;
                case "{OriginalLocationDateTime_mm}":
                    result = TimeZoneLibrary.ToStringDateTime_mm(LocationDateTime);
                    break;
                case "{OriginalLocationDateTime_ss}":
                    result = TimeZoneLibrary.ToStringDateTime_ss(LocationDateTime);
                    break;
                case "{OriginalLocationName}":
                    result = LocationName;
                    break;
                case "{OriginalLocationCountry}":
                    result = LocationCountry;
                    break;
                case "{OriginalLocationState}":
                    result = LocationState;
                    break;
                case "{OriginalLocationCity}":
                    result = LocationCity;
                    break;
                #endregion Location
                default:
                    throw new Exception("Theres a been use on none existing variable in config for write metadata: '" + variableName + "' that is not implemented");

            }
            if (convertNullToBlank && result == null) result = "";
            return result;
        }
        #endregion Variables - GetPropertyValueOriginal

        #region Variables - HasValueChanged
        private bool HasValueChanged(string variable, Metadata metadata)
        {
            bool result = false;
            switch (variable)
            {
                #region File System
                case "{IfFileDateModifiedChanged}":
                    if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(this.FileDateAccessed, metadata.FileDateAccessed)) result = true;
                    break;
                #endregion

                #region FileName/Folder/Path
                case "{IfFilePathChanged}":
                    if (this.FileFullPath != metadata.FileFullPath) result = true;
                    break;
                case "{IfFileNameChanged}":
                    if (this.FileName != metadata.FileName) 
                        result = true;
                    break;
                case "{IfFileDirectoryChanged}":
                    if (this.FileDirectory != metadata.FileDirectory) result = true;
                    break;
                #endregion

                #region Personal
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
                #endregion

                #region Region
                case "{IfPersonalRegionChanged}":
                    if (VerifyRegionStructureList(this.personalRegionList, metadata.personalRegionList) == false) result = true;
                    break;
                #endregion

                #region Keyword
                case "{IfPersonalKeywordsChanged}":
                    if (VerifyKeywordList(this.personalTagList, metadata.personalTagList) == false) result = true;
                    break;
                #endregion

                #region Media
                case "{IfMediaDateTakenChanged}":
                    if (!TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(this.MediaDateTaken, metadata.MediaDateTaken)) result = true;
                    break;
                #endregion

                #region Location
                case "{IfLocationChanged}":
                    if (this.locationLatitude != metadata.locationLatitude ||
                        this.LocationLongitude != metadata.locationLongitude) result = true;
                    break;
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
                #endregion
            }
            return result;

        }
        #endregion 

        #region Variables - ReplaceVariablesWrittenByUser
        public string ReplaceVariablesWrittenByUser(string stringWithVariables, bool useExifFormat, bool convertNullToBlank, 
            List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag,
            string personalRegionInfoMP, string personalRegionInfo, string personalKeywordList, string personalKeywordsXML, string personalKeywordItemsAdd)
        {
            string result = stringWithVariables;
            List<string> variables = Metadata.ListOfPropertiesWrittenByUser(false);
            foreach (string variable in variables)
            {
                while (result.Contains(variable))
                    result = result.Replace(variable, GetPropertyValueWrittenByUser(variable, useExifFormat, convertNullToBlank,
                    allowedFileNameDateTimeFormats, computerNames, GPStag,
                    personalRegionInfoMP, personalRegionInfo, personalKeywordList, personalKeywordsXML, personalKeywordItemsAdd));
            }
            result = result.Replace("\r\n\r\n", "\r\n");
            return result;
        }
        #endregion

        #region Variables - ReplaceVariablesOriginal
        public string ReplaceVariablesOriginal(string stringWithVariables, bool useExifFormat, bool convertNullToBlank, 
            List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag, 
            string personalRegionInfoMP, string personalRegionInfo, string personalKeywordList, string personalKeywordsXML, string personalKeywordItemsAdd)
        {
            string result = stringWithVariables;
            List<string> variables = Metadata.ListOfPropertiesOriginal(false);
            foreach (string variable in variables)
            {
                while (result.Contains(variable))
                    result = result.Replace(variable, GetPropertyValueOriginal(variable, useExifFormat, convertNullToBlank,
                    allowedFileNameDateTimeFormats, computerNames, GPStag, 
                    personalRegionInfoMP, personalRegionInfo, personalKeywordList, personalKeywordsXML, personalKeywordItemsAdd));
            }
            result = result.Replace("\r\n\r\n", "\r\n");
            return result;
        }
        #endregion

        #region Variables - ReplaceKeywordItemVariablesWrittenByUser
        public string ReplaceKeywordItemVariablesWrittenByUser(string stringWithVariables, string keyword)
        {
            string result = stringWithVariables;
            while (result.Contains("{KeywordItem}")) result = result.Replace("{KeywordItem}", keyword);
            return result;
        }
        #endregion

        #region Variables - ReplaceKeywordItemVariablesOriginal
        public string ReplaceKeywordItemVariablesOriginal(string stringWithVariables, string keyword)
        {
            string result = stringWithVariables;
            while (result.Contains("{KeywordItemOriginal}")) result = result.Replace("{KeywordItemOriginal}", keyword);
            return result;
        }
        #endregion

        #region Variables - ReplaceVariablesWrittenByUser
        public string ReplaceVariablesWrittenByUser(string stringWithVariables, List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag)
        {
            return ReplaceVariablesWrittenByUser(stringWithVariables, true, true, allowedFileNameDateTimeFormats, computerNames, GPStag,
                VariablePersonalRegionInfoMP(), VariablePersonalRegionInfo(), VariablePersonalKeywordsList(), VariableKeywordCategories(), 
                "");
        }
        #endregion

        #region Variables - ReplaceVariablesOriginal
        public string ReplaceVariablesOriginal(string stringWithVariables, List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag)
        {
            return ReplaceVariablesOriginal(stringWithVariables, true, true, allowedFileNameDateTimeFormats, computerNames, GPStag,
                VariablePersonalRegionInfoMP(), VariablePersonalRegionInfo(), VariablePersonalKeywordsList(), VariableKeywordCategories(), 
                "");
        }
        #endregion

        #region Variables - ReplaceVariablesWrittenByUser
        public string ReplaceVariablesWrittenByUser(string stringWithVariables, List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag, string personalKeywordItemsAdd)
        {
            return ReplaceVariablesWrittenByUser(stringWithVariables, true, true, allowedFileNameDateTimeFormats, computerNames, GPStag,
                VariablePersonalRegionInfoMP(), VariablePersonalRegionInfo(), VariablePersonalKeywordsList(), VariableKeywordCategories(), 
                personalKeywordItemsAdd);
        }
        #endregion 

        #region Variables - ReplaceVariablesOriginal
        public string ReplaceVariablesOriginal(string stringWithVariables, List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag, string personalKeywordItemsAdd)
        {
            return ReplaceVariablesOriginal(stringWithVariables, true, true, 
                allowedFileNameDateTimeFormats, computerNames, GPStag,
                VariablePersonalRegionInfoMP(), VariablePersonalRegionInfo(), VariablePersonalKeywordsList(), VariableKeywordCategories(), 
                personalKeywordItemsAdd);
        }
        #endregion 

        #region Variables - Create Variable -XPKeywords={PersonalKeywordsList}
        public string VariablePersonalKeywordsList()
        {
            string personalKeywordsList = "";
            foreach (KeywordTag keywordTag in this.PersonalKeywordTags)
            {
                if (keywordTag.Keyword.Contains(";")) 
                {
                    string[] keywords = keywordTag.Keyword.Split(';');
                    foreach (string keyword in keywords) if (keyword != null) personalKeywordsList += (personalKeywordsList.Length > 0 ? ";" : "") + keyword.Trim();
                }
                else personalKeywordsList += (personalKeywordsList.Length > 0 ? ";" : "") + keywordTag; //personalKeywordsList
            }
            return personalKeywordsList;
        }
        #endregion

        #region Variables - Create Variable -Categories={PersonalKeywordsXML}
        public string VariableKeywordCategories()
        {
            string keywordCategories = "<Categories>";
            foreach (KeywordTag tagHierarchy in this.PersonalKeywordTags)
            {
                try
                {
                    if (tagHierarchy != null && tagHierarchy.Keyword != null)
                    {
                        string[] tagHierarchyList = tagHierarchy.Keyword.Split('/');
                        for (int tagNumber = 0; tagNumber < tagHierarchyList.Length; tagNumber++)
                        {
                            if (tagNumber == tagHierarchyList.Length - 1) keywordCategories += "<Category Assigned=\"1\">";
                            else keywordCategories += "<Category Assigned=\"0\">";

                            keywordCategories += tagHierarchyList[tagNumber].Trim();
                        }
                        for (int tagNumber = 0; tagNumber < tagHierarchyList.Length; tagNumber++) keywordCategories += "</Category>";
                    }
                } catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            }
            keywordCategories += "</Categories>";
            return keywordCategories;
        }
        #endregion

        #region Variables - Remove duplicates Name and Area regions (Don't care about source)
        private List<RegionStructure> VariableRegionWriteListWithoutDuplicate()
        {
            List<RegionStructure> regionWriteListWithoutDuplicate = new List<RegionStructure>();
            foreach (RegionStructure regionStructure in this.PersonalRegionList)
                if (!regionStructure.DoesThisRectangleAndNameExistInList(regionWriteListWithoutDuplicate)) regionWriteListWithoutDuplicate.Add(regionStructure);
            return regionWriteListWithoutDuplicate;
        }
        #endregion

        #region Variables - Create Variable -ImageRegion= (IPTC region tags - ImageRegion)
        #endregion

        #region Variables - Create Variable -RegionInfoMP={PersonalRegionInfoMP} - Microsoft region tags 
        public string VariablePersonalRegionInfoMP()
        {
            List<RegionStructure> regionWriteListWithoutDuplicate = VariableRegionWriteListWithoutDuplicate();

            string personalRegionInfoMP = "";
            if (regionWriteListWithoutDuplicate.Count > 0)
            {
                bool needComma = false;
                personalRegionInfoMP += "{Regions=[";
                foreach (RegionStructure region in regionWriteListWithoutDuplicate)
                {
                    RectangleF rectangleF = region.GetRegionInfoMPRectangleF(this.MediaSize);
                    if (needComma) personalRegionInfoMP += ",";
                    personalRegionInfoMP += "{PersonDisplayName=" + region.Name + ",Rectangle=" +
                        rectangleF.X.ToString(CultureInfo.InvariantCulture) + "|," +
                        rectangleF.Y.ToString(CultureInfo.InvariantCulture) + "|," +
                        rectangleF.Width.ToString(CultureInfo.InvariantCulture) + "|," +
                        rectangleF.Height.ToString(CultureInfo.InvariantCulture) + "}";
                    needComma = true;
                }
                personalRegionInfoMP += "]}";
            }
            return personalRegionInfoMP;
        }
        #endregion

        #region Variables - Create Variable -RegionInfo={PersonalRegionInfo} - MWG Regions Tags 
        public string VariablePersonalRegionInfo()
        {
            List<RegionStructure> regionWriteListWithoutDuplicate = VariableRegionWriteListWithoutDuplicate();

            string personalRegionInfo = "";
            if (regionWriteListWithoutDuplicate.Count > 0)
            {
                bool needComma = false;
                personalRegionInfo += "{AppliedToDimensions={W=" + this.MediaWidth +
                    ",H=" + this.MediaHeight +
                    ",Unit=pixel}," +
                    "RegionList=[";
                foreach (RegionStructure region in regionWriteListWithoutDuplicate)
                {
                    RectangleF rectangleF = region.GetRegionInfoRectangleF(this.MediaSize);
                    if (needComma) personalRegionInfo += ",";
                    personalRegionInfo += "{Area={W=" + rectangleF.Width.ToString(CultureInfo.InvariantCulture) +
                        ",H=" + rectangleF.Height.ToString(CultureInfo.InvariantCulture) +
                        ",X=" + rectangleF.X.ToString(CultureInfo.InvariantCulture) +
                        ",Y=" + rectangleF.Y.ToString(CultureInfo.InvariantCulture) +
                        ",Unit=normalized},Name=" + region.Name + "}";
                    needComma = true;
                }
                personalRegionInfo += "]}";
            }
            return personalRegionInfo;
        }
        #endregion

        #region Variables - Create Variable - Keyword items - ***Loop of keyword items***
        public string VariablePersonalKeywordsWrittenByUser(string stringWithVariables, List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag)
        {
            string personalRegionInfoMP = VariablePersonalRegionInfoMP();
            string personalRegionInfo = VariablePersonalRegionInfo();
            string personalKeywordsList = VariablePersonalKeywordsList();
            string keywordCategories = VariableKeywordCategories();

            string personalKeywordAdd = "";
            foreach (KeywordTag keywordTag in this.PersonalKeywordTags)
            {
                string keywordItemToWrite = this.ReplaceVariablesWrittenByUser(stringWithVariables, true, true, allowedFileNameDateTimeFormats, computerNames, GPStag,
                    personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, "");
                keywordItemToWrite = this.ReplaceKeywordItemVariablesWrittenByUser(keywordItemToWrite, keywordTag.Keyword);
                if (!string.IsNullOrWhiteSpace(keywordItemToWrite))
                {
                    if (!keywordItemToWrite.EndsWith("\r\n") || !keywordItemToWrite.EndsWith("\r") || !keywordItemToWrite.EndsWith("\n"))
                        keywordItemToWrite += "\r\n";

                    personalKeywordAdd += keywordItemToWrite;
                } 
            }
            return personalKeywordAdd;
        }
        #endregion

        #region Variables - Create Variable - Keyword items - ***Loop of keyword items***
        public string VariablePersonalKeywordsOriginal(string stringWithVariables, List<string> allowedFileNameDateTimeFormats, List<string> computerNames, string GPStag)
        {
            string personalRegionInfoMP = VariablePersonalRegionInfoMP();
            string personalRegionInfo = VariablePersonalRegionInfo();
            string personalKeywordsList = VariablePersonalKeywordsList();
            string keywordCategories = VariableKeywordCategories();

            string personalKeywordAdd = "";
            foreach (KeywordTag keywordTag in this.PersonalKeywordTags)
            {
                string keywordItemToWrite = this.ReplaceVariablesOriginal(stringWithVariables, true, true, allowedFileNameDateTimeFormats, computerNames, GPStag,
                    personalRegionInfoMP, personalRegionInfo, personalKeywordsList, keywordCategories, "");
                keywordItemToWrite = this.ReplaceKeywordItemVariablesOriginal(keywordItemToWrite, keywordTag.Keyword);
                if (!string.IsNullOrWhiteSpace(keywordItemToWrite))
                {
                    if (!keywordItemToWrite.EndsWith("\r\n") || !keywordItemToWrite.EndsWith("\r") || !keywordItemToWrite.EndsWith("\n"))
                        keywordItemToWrite += "\r\n";

                    personalKeywordAdd += keywordItemToWrite;
                }
            }
            return personalKeywordAdd;
        }
        #endregion

    }
}

