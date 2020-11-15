using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MetadataLibrary;
using MetadataPriorityLibrary;
using TimeZone;

namespace Exiftool
{
    

    public class ExiftoolReader : ImetadataReader
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private const string _ExiftoolTabSeperator = "{7ZAJ6A5GHJ}";
        private string[] _ExiftoolTabSeperators = new string[] { _ExiftoolTabSeperator };

        private MetadataDatabaseCache metadataDatabaseCache;
        private ExiftoolDataDatabase metadataExiftoolDatabase;
        private ExiftoolWarningDatabase metadataExiftoolWarningDatabase;

        public ExiftoolReader(
            MetadataDatabaseCache metadataDatabaseCache,
            ExiftoolDataDatabase metadataExiftoolDatabase,
            ExiftoolWarningDatabase metadataExiftoolWarningDatabase)
        {
            this.metadataDatabaseCache = metadataDatabaseCache;
            this.metadataExiftoolDatabase = metadataExiftoolDatabase;
            this.metadataExiftoolWarningDatabase = metadataExiftoolWarningDatabase;
        }

        public delegate void AfterNewMediaFound(FileEntry fileEntry);
        public event AfterNewMediaFound afterNewMediaFoundEvent;
        public MetadataReadPrioity MetadataReadPrioity { get; set; } = new MetadataReadPrioity();

        #region ExiftoolData old - Used to check new data equal with old data that was read

        private List<string> oldKeywordList = null;
        private ExiftoolData oldExifToolKeywords = new ExiftoolData();

        private List<RegionStructure> oldRegionList = null;
        private ExiftoolData oldExifToolRegion = new ExiftoolData();

        private ExiftoolData oldExifToolFileName = new ExiftoolData();
        private ExiftoolData oldExifToolFilePath = new ExiftoolData();
        private ExiftoolData oldExifToolFileModifyDate = new ExiftoolData();
        private ExiftoolData oldExifToolFileAccessDate = new ExiftoolData();
        private ExiftoolData oldExifToolFileCreateDate = new ExiftoolData();
        private ExiftoolData oldExifToolFileSize = new ExiftoolData();
        private ExiftoolData oldExifToolMediaWidth = new ExiftoolData();
        private ExiftoolData oldExifToolMediaHeight = new ExiftoolData();
        private ExiftoolData oldExifToolMIMEType = new ExiftoolData();
        private ExiftoolData oldExifToolMake = new ExiftoolData();
        private ExiftoolData oldExifToolModel = new ExiftoolData();
        private ExiftoolData oldExifToolCreateDate = new ExiftoolData();
        private ExiftoolData oldExifToolAuthor = new ExiftoolData();
        private ExiftoolData oldExifToolAlbum = new ExiftoolData();
        private ExiftoolData oldExifToolDescription = new ExiftoolData();
        private ExiftoolData oldExifToolTitle = new ExiftoolData();
        private ExiftoolData oldExifToolComment = new ExiftoolData();
        private ExiftoolData oldExifToolRating = new ExiftoolData();
        private ExiftoolData oldExifToolRatingPercent = new ExiftoolData();
        private ExiftoolData oldExifToolLocationName = new ExiftoolData();
        private ExiftoolData oldExifToolLocationCity = new ExiftoolData();
        private ExiftoolData oldExifToolLocationState = new ExiftoolData();
        private ExiftoolData oldExifToolLocationCountry = new ExiftoolData();
        private ExiftoolData oldExifToolGPSAltitude = new ExiftoolData();
        private ExiftoolData oldExifToolGPSLatitude = new ExiftoolData();
        private ExiftoolData oldExifToolGPSLongitude = new ExiftoolData();
        private ExiftoolData oldExifToolGPSDateTime = new ExiftoolData();

        private void CleanAllOldExiftoolDataForReadNewFile()
        {
            oldKeywordList = null;
            oldExifToolKeywords = new ExiftoolData();

            oldRegionList = null;
            oldExifToolRegion = new ExiftoolData();

            oldExifToolFileName = new ExiftoolData();
            oldExifToolFilePath = new ExiftoolData();
            oldExifToolFileModifyDate = new ExiftoolData();
            oldExifToolFileAccessDate = new ExiftoolData();
            oldExifToolFileCreateDate = new ExiftoolData();
            oldExifToolFileSize = new ExiftoolData();
            oldExifToolMediaWidth = new ExiftoolData();
            oldExifToolMediaHeight = new ExiftoolData();
            oldExifToolMIMEType = new ExiftoolData();
            oldExifToolMake = new ExiftoolData();
            oldExifToolModel = new ExiftoolData();
            oldExifToolCreateDate = new ExiftoolData();
            oldExifToolAuthor = new ExiftoolData();
            oldExifToolAlbum = new ExiftoolData();
            oldExifToolDescription = new ExiftoolData();
            oldExifToolTitle = new ExiftoolData();
            oldExifToolComment = new ExiftoolData();
            oldExifToolRating = new ExiftoolData();
            oldExifToolRatingPercent = new ExiftoolData();
            oldExifToolLocationName = new ExiftoolData();
            oldExifToolLocationCity = new ExiftoolData();
            oldExifToolLocationState = new ExiftoolData();
            oldExifToolLocationCountry = new ExiftoolData();
            oldExifToolGPSAltitude = new ExiftoolData();
            oldExifToolGPSLatitude = new ExiftoolData();
            oldExifToolGPSLongitude = new ExiftoolData();
            oldExifToolGPSDateTime = new ExiftoolData();
        }
        #endregion

        #region Is Application Closing
        private bool isClosing = false;
        public void Close()
        {
            isClosing = true;
        }
        #endregion 

        #region MetadataGroupPrioity
        public void MetadataGroupPrioityWrite()
        {
            MetadataReadPrioity.Write();
        }
        public void MetadataGroupPrioityRead()
        {
            MetadataReadPrioity.ReadOnlyOnce();
        }
        #endregion ExiftoolTagsWarning_Write

        #region ExiftoolTagsWarning_Write
        private void ExiftoolTagsWarning_Write(ExiftoolData exifToolDataPrevious, ExiftoolData exifToolDataConvertThis, string warning)
        {
            metadataExiftoolWarningDatabase.Write(exifToolDataPrevious, exifToolDataConvertThis, warning);
        }
        #endregion 

        #region ConvertAndCheck(Format)FromString
        public bool isDateTimeEqual(DateTime c1, DateTime c2)
        {
            TimeSpan t = ((DateTime)c1).Subtract((DateTime)c2);
            if (System.Math.Abs(t.TotalSeconds) < 1)
            {
                return true;
            }
            return false;
        }

        private void CheckTimeZoom(Metadata metadata, ref String error)
        {
            string verificationRegion = "Verification";
            string verificationMediaTaken = "MediaTaken";
            string verificationTimeZone = "TimeZone";
            string verificationLocationDateTime = "GPSDateTime";
            string verificationLocationCoordinates = "GPSCoordinates";


            if (metadata.MediaDateTaken == null)
            {
                ExiftoolData exifToolData = new ExiftoolData(metadata.FileName, metadata.FileDirectory, (DateTime)metadata.FileDateModified,
                verificationRegion, verificationMediaTaken, "Check Date and Time has correct time");

                string warning = "Warning! Missing metadata tag " + CompositeTags.DateTimeDigitized + "\r\n";
                error += warning;
                ExiftoolTagsWarning_Write(exifToolData, exifToolData, warning);
            }
            else if (metadata.LocationLatitude == null || metadata.LocationLongitude == null)
            {
                //Missing GPS ccorinate
                ExiftoolData exifToolData = new ExiftoolData(metadata.FileName, metadata.FileDirectory, (DateTime)metadata.FileDateModified,
                    verificationRegion, verificationLocationCoordinates, "Check Date and Time has correct time");

                string warning = "Warning! Missing metadata tags " + CompositeTags.GPSCoordinatesLatitude + " and " + CompositeTags.GPSCoordinatesLongitude + "\r\n";
                error += warning;
                ExiftoolTagsWarning_Write(exifToolData, exifToolData, warning);
            }
            else if (metadata.LocationDateTime == null)
            {
                ExiftoolData exifToolData = new ExiftoolData(metadata.FileName, metadata.FileDirectory, (DateTime)metadata.FileDateModified,
                        verificationRegion, verificationLocationDateTime, "Check Date and Time has correct time");

                string warning = "Warning! Missing metadata tag " + CompositeTags.GPSDateTime + "\r\n";
                    error += warning;
                    ExiftoolTagsWarning_Write(exifToolData, exifToolData, warning);
            }
            else
            {
                if (!TimeZoneLibrary.VerifyTimeZoon(
                    (double)metadata.LocationLatitude, (double)metadata.LocationLongitude,
                    (DateTime)metadata.LocationDateTime, (DateTime)metadata.MediaDateTaken))
                {
                    ExiftoolData exifToolData = new ExiftoolData(metadata.FileName, metadata.FileDirectory, (DateTime)metadata.FileDateModified,
                        verificationRegion, verificationTimeZone, "Check Date and Time has correct time");

                    string warning = "Warning! Metadata has mismatch between " + CompositeTags.DateTimeDigitized + " and " + CompositeTags.GPSDateTime + "\r\n";
                    error += warning;
                    ExiftoolTagsWarning_Write(exifToolData, exifToolData, warning);

                }
            }
        }

        private void CheckKeywordList(List<string> oldKeywordList, List<string> newKeywordList, ExiftoolData exifToolDataConvertThis, ExiftoolData exifToolDataPrevious, String compositeTag, ref String error)
        {
            if (oldKeywordList == null) return;
            if (newKeywordList == null) return;
            bool isListEqual = true;

            foreach (string keyword in newKeywordList)
            {
                if (!oldKeywordList.Contains(keyword))
                {
                    isListEqual = false;
                    break;
                }
            }
            if (isListEqual)
            {
                foreach (string keyword in oldKeywordList)
                {
                    if (!newKeywordList.Contains(keyword))
                    {
                        isListEqual = false;
                        break;
                    }
                }
            }

            if (!isListEqual)
            {
                string warning = "Warning! Metadata missmatching between two metadata values.\r\nComposite tag:" + compositeTag + "\r\n";

                warning += string.Format(CultureInfo.InvariantCulture,
                        "\r\nRegion: {0} Command: {1}\r\nValues:'\r\n", exifToolDataConvertThis.Region, exifToolDataConvertThis.Command);
                foreach (string keyword in newKeywordList) warning += keyword + "\r\n";

                warning += string.Format(CultureInfo.InvariantCulture,
                        "\r\nRegion: {0} Command: {1}\r\nValue\r\n", exifToolDataPrevious.Region, exifToolDataPrevious.Command);
                foreach (string keyword in oldKeywordList) warning += keyword + "\r\n";

                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                //Select the list that has highst priority
                //if (MetadataReadPrioity.Get(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag) > MetadataReadPrioity.GetCompositeTagsHighestPrioity(compositeTag))
            }
        }

        private void CheckRegionList(List<RegionStructure> oldRegionList, List<RegionStructure> newRegionList, Size? mediaSize, ExiftoolData exifToolDataConvertThis, ExiftoolData exifToolDataPrevious, String compositeTag, ref String error)
        {
            if (oldRegionList == null) return;
            if (newRegionList == null) return;
            bool isListEqual = true;

            foreach (RegionStructure regionStructure in newRegionList)
            {                
                if (!regionStructure.DoesThisRectangleAndNameExistInList(oldRegionList))
                {
                    isListEqual = false;
                    break;
                }
            }
            if (isListEqual)
            {
                foreach (RegionStructure regionStructure in oldRegionList)
                {
                    if (!regionStructure.DoesThisRectangleAndNameExistInList(newRegionList))
                    {
                        isListEqual = false;
                        break;
                    }
                }
            }

            if (!isListEqual)
            {
                string warning = "Warning! Metadata missmatching between two metadata values.\r\nComposite tag:" + compositeTag + "\r\n";

                if (mediaSize == null) mediaSize = new Size(1000, 1000); 

                warning += string.Format(CultureInfo.InvariantCulture,
                        "\r\nRegion: {0} Command: {1}\r\nValues:'\r\n", exifToolDataConvertThis.Region, exifToolDataConvertThis.Command);
                foreach (RegionStructure regionStructure in newRegionList) warning += regionStructure.RegionErrorText((Size)mediaSize) + "\r\n";

                warning += string.Format(CultureInfo.InvariantCulture,
                        "\r\nRegion: {0} Command: {1}\r\nValue\r\n", exifToolDataPrevious.Region, exifToolDataPrevious.Command);
                foreach (RegionStructure regionStructure in oldRegionList) warning += regionStructure.RegionErrorText((Size)mediaSize) + "\r\n";

                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                //Select the list that has highst priority
                //if (MetadataReadPrioity.Get(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag) > MetadataReadPrioity.GetCompositeTagsHighestPrioity(compositeTag))
            }
        }

        public DateTime? ConvertDateTimeLocalFromString(String dateTimeToConvert)
        {
            DateTimeOffset pictureTime;
            DateTimeOffset localTimeZone;
            String[] dateFormats = { "yyyy:MM:dd HH:mm:ssZ", "yyyy:MM:dd HH:mm", "yyyy:MM:dd HH:mm:ss", "yyyy:MM:dd HH:mm:sszzz", "yyyy:MM:dd HH:mm:ss.fff", "yyyy:MM:dd HH:mm:ss.ff", "yyyy:MM:dd HH:mm:ss.ffff", "yyyy:MM:dd HH:mm:ss.fffff", "yyyy:MM:dd HH:mm:ss.ffffff" };
            String[] convertThisDateZone = dateTimeToConvert.Split('+');
            
            try
            {
                if (convertThisDateZone.Length > 1)
                {
                    if (DateTimeOffset.TryParseExact(dateTimeToConvert, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out pictureTime))
                    {                        
                        localTimeZone = TimeZoneInfo.ConvertTime(pictureTime, TimeZoneInfo.Local);
                        return new DateTime(localTimeZone.Ticks, DateTimeKind.Local);
                    }
                    else return null;
                }
                else if (convertThisDateZone[0][convertThisDateZone[0].Length - 1] == 'Z')
                {
                    if (DateTimeOffset.TryParseExact(dateTimeToConvert, "yyyy:MM:dd HH:mm:ssZ", CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out pictureTime))
                    {                        
                        localTimeZone = TimeZoneInfo.ConvertTime(pictureTime, TimeZoneInfo.Utc);
                        return new DateTime(localTimeZone.Ticks, DateTimeKind.Utc);
                    }
                    else return null;
                }
                else
                {
                    if (DateTimeOffset.TryParseExact(dateTimeToConvert, dateFormats, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out pictureTime))
                    {
                        localTimeZone = TimeZoneInfo.ConvertTime(pictureTime, TimeZoneInfo.Local);
                        return new DateTime(localTimeZone.Ticks, DateTimeKind.Local);
                    }
                    else return null;
                }
            } catch (Exception e)
            {
                Logger.Warn(dateTimeToConvert + " " + e.Message); //TODO: Need to fix problems with date formats
                return null; 
            }

        }
    
        private DateTime? ConvertAndCheckDateFromString(DateTime? oldValue, ExiftoolData exifToolDataConvertThis, ExiftoolData exifToolDataPrevious, String compositeTag, ref String error)
        {
            MetadataReadPrioity.Add(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag);
            DateTime? newValue = ConvertDateTimeLocalFromString(exifToolDataConvertThis.Parameter);

            if (!newValue.HasValue)
            {

                string warning = string.Format(CultureInfo.InvariantCulture, "Warning: Error in date and time formating value Region: {0} Command: {1} Value: '{2}': ",
                        exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, exifToolDataConvertThis.Parameter);
                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                return null;
            }

            if (oldValue.HasValue && !isDateTimeEqual((DateTime)newValue, (DateTime)oldValue))
            {
                string warning = string.Format(CultureInfo.InvariantCulture, 
                        "Warning! Metadata missmatching between two metadata values.\r\n" +
                        "Region: {0} Command: {1}\r\nValue '{2}'\r\n" +
                        "Region: {3} Command: {4}\r\nValue '{5}'",
                        exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, exifToolDataConvertThis.Parameter,
                        exifToolDataPrevious.Region, exifToolDataPrevious.Command, exifToolDataPrevious.Parameter);                
                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                //return oldDate; //Do to bug in composite Date Time and this comming last in list
                if (MetadataReadPrioity.Get(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag) > MetadataReadPrioity.GetCompositeTagsHighestPrioity(compositeTag))
                    return newValue;
                else
                    return oldValue;
            }

            return newValue;
            
        }

        private Byte? ConvertAndCheckByteFromString(Byte? oldValue, ExiftoolData exifToolDataConvertThis, ExiftoolData exifToolDataPrevious, String compositeTag, ref String error)
        {
            MetadataReadPrioity.Add(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag);
            Byte? newValue = Byte.Parse(exifToolDataConvertThis.Parameter, CultureInfo.InvariantCulture);

            if (oldValue.HasValue && newValue != oldValue)
            {
                string warning = string.Format(CultureInfo.InvariantCulture,
                        "Warning! Metadata missmatching between two metadata values.\r\n" +
                        "Region: {0} Command: {1}\r\nValue '{2}'\r\n" +
                        "Region: {3} Command: {4}\r\nValue '{5}'",
                        exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, exifToolDataConvertThis.Parameter,
                        exifToolDataPrevious.Region, exifToolDataPrevious.Command, exifToolDataPrevious.Parameter);
                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                if (MetadataReadPrioity.Get(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag) > MetadataReadPrioity.GetCompositeTagsHighestPrioity(compositeTag))
                    return newValue;
                else
                    return oldValue;
            }

            return newValue;
        }

        private int? ConvertAndCheckIntFromString(int? oldValue, ExiftoolData exifToolDataConvertThis, ExiftoolData exifToolDataPrevious, String compositeTag, ref String error)
        {
            MetadataReadPrioity.Add(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag);
            int? newValue = int.Parse(exifToolDataConvertThis.Parameter, CultureInfo.InvariantCulture);

            if (oldValue.HasValue && newValue != oldValue)
            {
                string warning = string.Format(CultureInfo.InvariantCulture,
                        "Warning! Metadata missmatching between two metadata values.\r\n" +
                        "Region: {0} Command: {1}\r\nValue '{2}'\r\n" +
                        "Region: {3} Command: {4}\r\nValue '{5}'",
                        exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, exifToolDataConvertThis.Parameter,
                        exifToolDataPrevious.Region, exifToolDataPrevious.Command, exifToolDataPrevious.Parameter);
                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                if (MetadataReadPrioity.Get(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag) > MetadataReadPrioity.GetCompositeTagsHighestPrioity(compositeTag))
                    return newValue;
                else
                    return oldValue;
            }

            return newValue;
        }

        private long? ConvertAndCheckLongFromString(long? oldValue, ExiftoolData exifToolDataConvertThis, ExiftoolData exifToolDataPrevious, String compositeTag, ref String error)
        {
            MetadataReadPrioity.Add(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag);
            long? newValue = long.Parse(exifToolDataConvertThis.Parameter, CultureInfo.InvariantCulture);

            if (oldValue.HasValue && newValue != oldValue)
            {
                string warning = string.Format(CultureInfo.InvariantCulture,
                        "Warning! Metadata missmatching between two metadata values.\r\n" +
                        "Region: {0} Command: {1}\r\nValue '{2}'\r\n" +
                        "Region: {3} Command: {4}\r\nValue '{5}'",
                        exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, exifToolDataConvertThis.Parameter,
                        exifToolDataPrevious.Region, exifToolDataPrevious.Command, exifToolDataPrevious.Parameter);
                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                if (MetadataReadPrioity.Get(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag) > MetadataReadPrioity.GetCompositeTagsHighestPrioity(compositeTag))
                    return newValue;
                else
                    return oldValue;
            }

            return newValue;
        }

        private float? ConvertAndCheckFloatFromString(float? oldValue, ExiftoolData exifToolDataConvertThis, ExiftoolData exifToolDataPrevious, String compositeTag, ref String error)
        {
            MetadataReadPrioity.Add(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag);
            float? newValue = float.Parse(exifToolDataConvertThis.Parameter, CultureInfo.InvariantCulture);

            if (newValue.HasValue) newValue = (float)Math.Round((float)newValue, SqliteDatabase.SqliteDatabaseUtilities.FloatAndDoubleNumberOfDecimals);

            if (oldValue.HasValue && Math.Abs((float)newValue - (float)oldValue) >= 0.000000000001) //Due not not excant numbers in float
            {
                string warning = string.Format(CultureInfo.InvariantCulture,
                        "Warning! Metadata missmatching between two metadata values.\r\n" +
                        "Region: {0} Command: {1}\r\nValue '{2}'\r\n" +
                        "Region: {3} Command: {4}\r\nValue '{5}'",
                        exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, exifToolDataConvertThis.Parameter,
                        exifToolDataPrevious.Region, exifToolDataPrevious.Command, exifToolDataPrevious.Parameter);
                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                if (MetadataReadPrioity.Get(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag) > MetadataReadPrioity.GetCompositeTagsHighestPrioity(compositeTag))
                    return newValue;
                else
                    return oldValue;
            }

            return newValue;
        }

        private Double? ConvertAndCheckDoubleFromString(Double? oldValue, ExiftoolData exifToolDataConvertThis, ExiftoolData exifToolDataPrevious, String compositeTag, ref String error)
        {
            MetadataReadPrioity.Add(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag);
            Double? newValue = Double.Parse(exifToolDataConvertThis.Parameter, CultureInfo.InvariantCulture);
            if (newValue.HasValue) newValue = (double)Math.Round((double)newValue, SqliteDatabase.SqliteDatabaseUtilities.FloatAndDoubleNumberOfDecimals);

            if (oldValue.HasValue && Math.Abs((double)newValue - (double)oldValue) >= 0.000000000001) //Due not not excant numbers in float
            {
                string warning = string.Format(CultureInfo.InvariantCulture,
                    "Warning! Metadata missmatching between two metadata values.\r\n" +
                    "Region: {0} Command: {1}\r\nValue '{2}'\r\n" +
                    "Region: {3} Command: {4}\r\nValue '{5}'",
                    exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, exifToolDataConvertThis.Parameter,
                    exifToolDataPrevious.Region, exifToolDataPrevious.Command, exifToolDataPrevious.Parameter);
                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                if (MetadataReadPrioity.Get(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag) > MetadataReadPrioity.GetCompositeTagsHighestPrioity(compositeTag))
                    return newValue;
                else
                    return oldValue;
            }

            return newValue;        
        }

        private String ConvertAndCheckStringFromString(String oldValue, ExiftoolData exifToolDataConvertThis, ExiftoolData exifToolDataPrevious, String compositeTag, ref String error)
        {
            MetadataReadPrioity.Add(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag);
            String newValue = exifToolDataConvertThis.Parameter;

            if (oldValue != null && newValue != oldValue)
            {
                string warning = string.Format(CultureInfo.InvariantCulture,
                        "Warning! Metadata missmatching between two metadata values.\r\n" +
                        "Region: {0} Command: {1}\r\nValue '{2}'\r\n" +
                        "Region: {3} Command: {4}\r\nValue '{5}'",
                        exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, exifToolDataConvertThis.Parameter,
                        exifToolDataPrevious.Region, exifToolDataPrevious.Command, exifToolDataPrevious.Parameter);
                error += warning;
                ExiftoolTagsWarning_Write(exifToolDataPrevious, exifToolDataConvertThis, warning);

                if (MetadataReadPrioity.Get(exifToolDataConvertThis.Region, exifToolDataConvertThis.Command, compositeTag) > MetadataReadPrioity.GetCompositeTagsHighestPrioity(compositeTag))
                    return newValue;
                else
                    return oldValue;
            }

            return newValue;
        }

        #endregion

        #region Convert Region
        private void ConvertRegion(List<RegionStructure> regionStructures, string tempRegionRectangle, string tempRegionPersonDisplayName)
        {
            string[] regionRectangles = string.IsNullOrWhiteSpace(tempRegionRectangle) ? null : tempRegionRectangle.Split(_ExiftoolTabSeperators, StringSplitOptions.None);
            string[] regionPersonDisplayNames = string.IsNullOrWhiteSpace(tempRegionPersonDisplayName) ? null : tempRegionPersonDisplayName.Split(_ExiftoolTabSeperators, StringSplitOptions.None);

            if (regionRectangles == null) return;


            for (int regionNumber = 0; regionNumber < regionRectangles.Length; regionNumber++)
            {
                RegionStructure regionStructure = new RegionStructure();
                regionStructure.RegionStructureType = RegionStructureTypes.WindowsLivePhotoGallery;
                regionStructure.Type = "Face";

                //   0       1       2       3        4      5       
                //{Rect}, {Rect}, {Rect}, {Rect}, {Rect}, {Rect} = 6
                // Name1,  Name2,  Name3,  Name4,  Name5,  Name6 = 6 -> Offset 0
                // Null,    Null,  Null,   Null,   Name1,  Name2 = 2 -> Offset 4
                //                                 5-4=0   5-4= 1

                int nameOffset = regionRectangles.Length - regionPersonDisplayNames.Length;
                int namePosition = regionNumber - nameOffset;

                /*if (regionNumber < regionPersonDisplayNames.Length)
                    regionStructure.Name = regionPersonDisplayNames[regionNumber];
                else
                    regionStructure.Name = "(Unknow)";*/

                if (namePosition >= 0 && namePosition < regionPersonDisplayNames.Length)
                    regionStructure.Name = regionPersonDisplayNames[namePosition];
                else
                    regionStructure.Name = null; // "(Unknow)";

                if (regionRectangles != null && regionNumber < regionRectangles.Length)
                {
                    string[] valueArray = regionRectangles[regionNumber].Split(',');
                    if (valueArray.Length == 4)
                    {
                        regionStructure.AreaX = float.Parse(valueArray[0], CultureInfo.InvariantCulture);
                        regionStructure.AreaY = float.Parse(valueArray[1], CultureInfo.InvariantCulture);
                        regionStructure.AreaWidth = float.Parse(valueArray[2], CultureInfo.InvariantCulture);
                        regionStructure.AreaHeight = float.Parse(valueArray[3], CultureInfo.InvariantCulture);
                    }
                }
                regionStructures.Add(regionStructure);
            }

        }

        private void ConvertRegion(List<RegionStructure> regionStructures, string tempRegionType, string tempRegionName,
            string tempRegionAreaH, string tempRegionAreaW, string tempRegionAreaX, string tempRegionAreaY)
        {
            string[] regionType = string.IsNullOrWhiteSpace(tempRegionType) ? null : tempRegionType.Split(_ExiftoolTabSeperators, StringSplitOptions.None);
            string[] regionAreaName = string.IsNullOrWhiteSpace(tempRegionName) ? null : tempRegionName.Split(_ExiftoolTabSeperators, StringSplitOptions.None);
            string[] regionAreaH = string.IsNullOrWhiteSpace(tempRegionAreaH) ? null : tempRegionAreaH.Split(_ExiftoolTabSeperators, StringSplitOptions.None);
            string[] regionAreaW = string.IsNullOrWhiteSpace(tempRegionAreaW) ? null : tempRegionAreaW.Split(_ExiftoolTabSeperators, StringSplitOptions.None);
            string[] regionAreaX = string.IsNullOrWhiteSpace(tempRegionAreaX) ? null : tempRegionAreaX.Split(_ExiftoolTabSeperators, StringSplitOptions.None);
            string[] regionAreaY = string.IsNullOrWhiteSpace(tempRegionAreaY) ? null : tempRegionAreaY.Split(_ExiftoolTabSeperators, StringSplitOptions.None);

            int maxCount = 0;
            if (regionType != null && regionType.Length > maxCount) maxCount = regionType.Length;
            if (regionAreaName != null && regionAreaName.Length > maxCount) maxCount = regionAreaName.Length;
            if (regionAreaH != null && regionAreaH.Length > maxCount) maxCount = regionAreaH.Length;
            if (regionAreaW != null && regionAreaW.Length > maxCount) maxCount = regionAreaW.Length;
            if (regionAreaX != null && regionAreaX.Length > maxCount) maxCount = regionAreaX.Length;
            if (regionAreaY != null && regionAreaY.Length > maxCount) maxCount = regionAreaY.Length;

            if (maxCount == 0) return;

            for (int regionNumber = 0; regionNumber < maxCount; regionNumber++)
            {
                RegionStructure regionStructure = new RegionStructure();

                regionStructure.RegionStructureType = RegionStructureTypes.MetadataWorkingGroup;

                if (regionType != null && regionNumber < regionType.Length)
                    regionStructure.Type = regionType[regionNumber];
                else
                    regionStructure.Type = "Face";

                if (regionType != null && regionNumber < regionAreaName.Length)
                    regionStructure.Name = regionAreaName[regionNumber];
                else
                    regionStructure.Name = null; // "(Unknown)";

                regionStructure.AreaHeight = float.Parse(regionAreaH[regionNumber], CultureInfo.InvariantCulture);
                regionStructure.AreaWidth = float.Parse(regionAreaW[regionNumber], CultureInfo.InvariantCulture);
                regionStructure.AreaX = float.Parse(regionAreaX[regionNumber], CultureInfo.InvariantCulture);
                regionStructure.AreaY = float.Parse(regionAreaY[regionNumber], CultureInfo.InvariantCulture);
                regionStructures.Add(regionStructure);
            }



        }
        #endregion

        #region Read Metadata

        #region Read fullFilePath
        public Metadata Read(MetadataBrokerTypes broker, string fullFilePath)
        {
            List<String> files = new List<String>();
            files.Add(fullFilePath);

            List<Metadata> metaDataCollections = Read(broker, files);
            if (metaDataCollections.Count == 1)
            {
                return metaDataCollections[0];
            }
            else
            {
                return null;
            }
        }
        #endregion



        

        #region Read List of files

        public List<Metadata> Read(MetadataBrokerTypes broker, List<String> files)
        {
            List<Metadata> metaDataCollections = new List<Metadata>();
            if (files == null) return metaDataCollections;
            if (files.Count == 0) return metaDataCollections;

            #region Find path to exiftool
            String path = NativeMethods.GetFullPathOfExeFile("exiftool.exe");
            if (path == null)
            {
                String path2 = NativeMethods.GetFullPathOfExeFile("exiftool(-k).exe");
                if (path2 != null)
                {
                    throw new InvalidOperationException("Found exiftool(-k).exe. You need ro rename from exiftool(-k).exe to exiftool.exe. " + Path.Combine(path2, "exiftool(-k).exe"));
                }
            }
            #endregion 

            #region Build exiftool.exe argument parameters
            //-charset filename=cp65001 -charset exiftool=cp65001
            string arguments = "-t -a -G0:1 -s -n -P -struct ";  
            //NEED USE "exiftools -struct" otherwise 'bug' in exiftool, can match region name and region rectangle, eg. list of 4 recgle and list of one name
            //can't be matched.  List names: Name1, List rectangles: R1, R2, R3, R4, where should Name1 go??? https://exiftool.org/struct.html 

            foreach (string file in files)
            {
                Logger.Info("ReadMetadata: " + file + " " + File.GetLastWriteTime(file).ToString());
                arguments += "\"" + NativeMethods.ShortFileName(file) + "\" ";  //DOS Workaround for with UTF-8 filenames, like æøåÆØÅ
            }
            #endregion 

            using (var process = new Process())
            {
                #region Start Exiftool process
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,

                    //Extra 
                    RedirectStandardInput = true,
                    StandardOutputEncoding = Encoding.UTF8 //,
                };
                process.Start();
                #endregion

                #region Init all data for start "first/each file"
                CleanAllOldExiftoolDataForReadNewFile();
                Metadata metadata = null; //= new MetaData("ExifTool.exe");
                List<ExiftoolData> exiftoolDatas = new List<ExiftoolData>();
                string tempRegionRectangle = "";
                string tempRegionPersonDisplayName = "";
                string tempRegionType = null;
                string tempRegionAreaH = null;
                string tempRegionAreaW = null;
                string tempRegionAreaX = null;
                string tempRegionAreaY = null;
                string tempRegionName = null;
                #endregion

                int fileNumber = 0;

                #region while (!process.StandardOutput.EndOfStream)
                while (!process.StandardOutput.EndOfStream)
                {
                    if (isClosing) break; //If user closeing the application, stop continue reading
                    
                    String readedLineFromExiftool = process.StandardOutput.ReadLine();
                    Int32 foundToSeperatorInLine = readedLineFromExiftool.IndexOf("\t");
                    ExiftoolData exifToolData;

                    if (readedLineFromExiftool.Substring(0, 9) == "======== ") //This occures only when multiple files found
                    {
                        if (metadata != null) //New file found, save all metadata found. 
                        {

                            CheckTimeZoom(metadata, ref metadata.errors);

                            #region Write Metadata and trigger Event afterNewMediaFoundEvent
                            ConvertRegion(metadata.PersonalRegionList, tempRegionRectangle, tempRegionPersonDisplayName);
                            ConvertRegion(metadata.PersonalRegionList, tempRegionType, tempRegionName,
                                tempRegionAreaH, tempRegionAreaW, tempRegionAreaX, tempRegionAreaY);

                            metadataDatabaseCache.TransactionBeginBatch();
                            metadataDatabaseCache.Write(metadata);
                            metadataDatabaseCache.TransactionCommitBatch();                            
                            
                            if (afterNewMediaFoundEvent != null) afterNewMediaFoundEvent.Invoke(new FileEntry(Path.Combine(metadata.FileDirectory, metadata.FileName), (DateTime)metadata.FileDateModified));
                            #endregion

                            metaDataCollections.Add(metadata); //Add list of metadatas read

                            #region Clean up temporary data - get ready for new media file
                            CleanAllOldExiftoolDataForReadNewFile();
                            metadata = null; //Start with new empty data when new file found
                            exiftoolDatas = new List<ExiftoolData>();
                            tempRegionRectangle = "";
                            tempRegionPersonDisplayName = "";
                            tempRegionType = null;
                            tempRegionAreaH = null;
                            tempRegionAreaW = null;
                            tempRegionAreaX = null;
                            tempRegionAreaY = null;
                            tempRegionName = null;
                            #endregion 
                        }
                    }

                    #region Init metadata with Init known data as directory, filename and last modyfied time
                    if (metadata == null)  //After loop we also check if metaData not null, to save the last file found
                    {
                        fileNumber++;
                        metadata = new Metadata(MetadataBrokerTypes.ExifTool); //Get ready to read a meta data
                        
                        //Don't use exif file name, problem with UTF8 filenames therefor short windows 8 short names used instead
                        metadata.FileName = Path.GetFileName(files[fileNumber - 1]);
                        metadata.FileDirectory = Path.GetDirectoryName(files[fileNumber - 1]);
                        metadata.FileDateModified = File.GetLastWriteTime(Path.Combine(metadata.FileDirectory, metadata.FileName));
                        metadata.FileDateCreated = File.GetCreationTime(Path.Combine(metadata.FileDirectory, metadata.FileName));

                        //For debugging
                        exifToolData = new ExiftoolData();
                        exifToolData.FileName = metadata.FileName;
                        exifToolData.FileDirectory = metadata.FileDirectory;
                        exifToolData.FileDateModified = (DateTime)metadata.FileDateModified;
                        exifToolData.Region = "FileSystem";
                        exifToolData.Command = "FileName";
                        exifToolData.Parameter = metadata.FileName;
                        ConvertAndCheckStringFromString(metadata.FileName, exifToolData, oldExifToolFileName,
                                    CompositeTags.FileName, ref metadata.errors);
                        oldExifToolFileName = new ExiftoolData(exifToolData);

                        exifToolData = new ExiftoolData();
                        exifToolData.FileName = metadata.FileName;
                        exifToolData.FileDirectory = metadata.FileDirectory;
                        exifToolData.FileDateModified = (DateTime)metadata.FileDateModified;
                        exifToolData.Region = "FileSystem";
                        exifToolData.Command = "Directory";
                        exifToolData.Parameter = metadata.FileDirectory;
                        ConvertAndCheckStringFromString(metadata.FileDirectory, exifToolData, oldExifToolFilePath,
                                    CompositeTags.Directory, ref metadata.errors);
                        oldExifToolFilePath = new ExiftoolData(exifToolData);

                        exifToolData = new ExiftoolData();
                        exifToolData.FileName = metadata.FileName;
                        exifToolData.FileDirectory = metadata.FileDirectory;
                        exifToolData.FileDateModified = (DateTime)metadata.FileDateModified;
                        exifToolData.Region = "FileSystem";
                        exifToolData.Command = "FileModifyDate";
                        exifToolData.Parameter = ((DateTime)metadata.FileDateModified).ToString("yyyy:MM:dd HH:mm:ss", CultureInfo.CurrentCulture);
                        ConvertAndCheckDateFromString(metadata.FileDateModified, exifToolData, oldExifToolFileName,
                            CompositeTags.FileModificationDateTime, ref metadata.errors);
                        oldExifToolFilePath = new ExiftoolData(exifToolData);
                    }
                    #endregion 

                    if (foundToSeperatorInLine >= 0)
                    {
                        #region Fill in exifToolData struct/class
                        String[] parameters = readedLineFromExiftool.Split('\t');
                        String regionType = parameters[0];
                        String command = parameters[1];
                        String parameter = parameters[2];

                        exifToolData = new ExiftoolData();
                        exifToolData.FileName = metadata.FileName;
                        exifToolData.FileDirectory = metadata.FileDirectory;
                        exifToolData.FileDateModified = (DateTime)metadata.FileDateModified;
                        exifToolData.Region = regionType;
                        exifToolData.Command = command;
                        exifToolData.Parameter = parameter.Replace(_ExiftoolTabSeperator, System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                        #endregion 

                        #region Log all Exiftool Region, Command and Paramters
                        //Make the Command unique, due to exif delivers not unique valies 
                        metadataExiftoolDatabase.TransactionBeginBatch();
                        string tempCommad = exifToolData.Command;
                        int uniqueNumber = 1;
                        while (exiftoolDatas.Contains(exifToolData)) exifToolData.Command = tempCommad + "(" + (uniqueNumber++) + ")";
                        exiftoolDatas.Add(exifToolData);    //Remover what's been saved to avoid 2 equal rows in database
                        metadataExiftoolDatabase.Write(exifToolData);
                        exifToolData.Command = tempCommad;  //Put original values back, after stores into database as Unique
                        metadataExiftoolDatabase.TransactionCommitBatch();
                        #endregion

                        #region Find Composite Tag and Priority
                        string commandPriority = exifToolData.Command;
                        MetadataPriorityKey metadataPriorityKey = new MetadataPriorityKey(exifToolData.Region, exifToolData.Command);
                        if (MetadataReadPrioity.MetadataPrioityDictionary.ContainsKey(metadataPriorityKey))
                        {
                            if (MetadataReadPrioity.MetadataPrioityDictionary[metadataPriorityKey].Composite != CompositeTags.NotDefined)
                                commandPriority = MetadataReadPrioity.MetadataPrioityDictionary[metadataPriorityKey].Composite;
                            else
                                MetadataReadPrioity.MetadataPrioityDictionary.Remove(metadataPriorityKey); //Not defind, add it back later with default values 
                        }
                        #endregion 

                        switch (commandPriority)
                        {
                            #region File
                            case CompositeTags.FileName:
                            case "FileName":
                            case "File Name":
                                if (metadata.FileName != parameter) //Convert short windows 8 filename to windows NT long filename
                                {
                                    exifToolData.Parameter = Path.GetFileName(NativeMethods.LongFileName(Path.Combine(metadata.FileDirectory, parameter)));
                                    if (metadata.FileName != exifToolData.Parameter) 
                                        throw new Exception("Filename doesn't match between Exiftool and reading of file, that means something went wrong with using exif tool and therefor crash");
                                }
                                ConvertAndCheckStringFromString(metadata.FileName, exifToolData, oldExifToolFileName,
                                    CompositeTags.FileName, ref metadata.errors);
                                oldExifToolFileName = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.Directory:
                                if (metadata.FileDirectory != parameter) //Convert short windows 8 directory name to windows NT long filename
                                {
                                    exifToolData.Parameter = Path.GetDirectoryName(NativeMethods.LongFileName(Path.Combine(parameter, metadata.FileName)));
                                    if (metadata.FileDirectory != exifToolData.Parameter) throw new Exception("Directory doesn't match between Exiftool and reading of file, that means something went wrong with using exif tool and therefor crash");
                                }
                                ConvertAndCheckStringFromString(metadata.FileDirectory, exifToolData, oldExifToolFileName,
                                    CompositeTags.Directory, ref metadata.errors);
                                oldExifToolFileName = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.FileModificationDateTime:
                            case "FileModifyDate":
                                DateTime tempDateTimeSoNotOverwriteMilliscounds = (DateTime)metadata.FileDateModified;
                                ConvertAndCheckDateFromString(tempDateTimeSoNotOverwriteMilliscounds, exifToolData, oldExifToolFileModifyDate,
                                    CompositeTags.FileModificationDateTime, ref metadata.errors);
                                oldExifToolFileModifyDate = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.FileAccessDateTime:
                            case "FileAccessDate":
                                metadata.FileLastAccessed = ConvertAndCheckDateFromString(
                                    metadata.FileLastAccessed, exifToolData, oldExifToolFileAccessDate,
                                    CompositeTags.FileAccessDateTime, ref metadata.errors);
                                oldExifToolFileAccessDate = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.FileCreationDateTime:
                            case "FileCreationDate":
                            case "FileCreateDate":
                                DateTime temp2DateTimeSoNotOverwriteMilliscounds = (DateTime)metadata.FileDateCreated;
                                metadata.FileDateCreated = ConvertAndCheckDateFromString(temp2DateTimeSoNotOverwriteMilliscounds,
                                    exifToolData, oldExifToolFileCreateDate,
                                    CompositeTags.FileCreationDateTime, ref metadata.errors);

                                oldExifToolFileCreateDate = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.FileSize:
                            case "FileSize":
                                metadata.FileSize = ConvertAndCheckLongFromString(metadata.FileSize,
                                    exifToolData, oldExifToolFileSize,
                                    CompositeTags.FileSize, ref metadata.errors);
                                oldExifToolFileSize = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.MIMEType:
                            case "MIMEType":
                                metadata.FileMimeType = ConvertAndCheckStringFromString(metadata.FileMimeType,
                                    exifToolData, oldExifToolMIMEType,
                                    CompositeTags.MIMEType, ref metadata.errors);
                                oldExifToolMIMEType = new ExiftoolData(exifToolData);
                                break;
                            #endregion

                            #region Camra
                            case CompositeTags.CameraModelMake:
                            case "Make":
                                metadata.CameraMake = ConvertAndCheckStringFromString(metadata.CameraMake, exifToolData, oldExifToolMake,
                                    CompositeTags.CameraModelMake, ref metadata.errors);
                                oldExifToolMake = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.CameraModelName:
                            case "Model":
                                metadata.CameraModel = ConvertAndCheckStringFromString(metadata.CameraModel, exifToolData, oldExifToolModel,
                                    CompositeTags.CameraModelName, ref metadata.errors);
                                oldExifToolModel = new ExiftoolData(exifToolData);
                                break;
                            #endregion

                            #region Media
                            case CompositeTags.MediaHeight:
                            case "ImageHeight":
                            case "ExifImageHeight":
                            case "SourceImageHeight":
                                if (regionType == "EXIF:IFD1")
                                {
                                    MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.Ignore);
                                    break;
                                } 
                                metadata.MediaHeight = ConvertAndCheckIntFromString(metadata.MediaHeight, exifToolData, oldExifToolMediaHeight,
                                    CompositeTags.MediaHeight, ref metadata.errors);
                                oldExifToolMediaHeight = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.MediaWidth:
                            case "ImageWidth":
                            case "ExifImageWidth":
                            case "SourceImageWidth":
                                if (regionType == "EXIF:IFD1")
                                {
                                    MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.Ignore);
                                    break;
                                }
                                metadata.MediaWidth = ConvertAndCheckIntFromString(metadata.MediaWidth, exifToolData, oldExifToolMediaWidth,
                                    CompositeTags.MediaWidth, ref metadata.errors);
                                oldExifToolMediaWidth = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.DateTimeDigitized:
                            case "Date Time Original":
                            case "Digital Creation Date/Time":
                            case "Date/Time Created":
                            case "CreationTime":
                            case "DateTimeOriginal":
                            case "CreateDate":
                            case "SubSecCreateDate":
                            case "SubSecDateTimeOriginal":
                                if (regionType == "IPTC" && command == "DateCreated")
                                {
                                    MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.Ignore);
                                    break; //IPTC:DateCreated + IPTC:TimeCreated = EXIF:CreateDate --> Only date without time, so don't read it
                                }
                                    
                                metadata.MediaDateTaken = ConvertAndCheckDateFromString(metadata.MediaDateTaken, exifToolData, oldExifToolCreateDate,
                                    CompositeTags.DateTimeDigitized, ref metadata.errors);
                                oldExifToolCreateDate = new ExiftoolData(exifToolData);
                                break;
                            #endregion

                            #region Personal
                            case CompositeTags.ArthurStruct:
                            case "Creator":         //XMP:XMP-dc	Creator	        [Nokia Imaging SDK]  
                                MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.ArthurStruct);
                                List<string> arthurListStruct = StructDeSerialization.GetListOfValues(parameter);

                                string arthur = string.Join(";", arthurListStruct.ToArray());

                                exifToolData.Parameter = arthur;
                                metadata.PersonalAuthor = ConvertAndCheckStringFromString(metadata.PersonalAuthor, exifToolData, oldExifToolAuthor,
                                    CompositeTags.Arthur, ref metadata.errors);
                                oldExifToolAuthor = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.Arthur:
                            case "By-line":         //IPTC	        By-line	        string[0,32]+
                            case "XPAuthor":        //EXIF:IFD0	    XPAuthor	    Wrong size
                                
                                metadata.PersonalAuthor = ConvertAndCheckStringFromString(metadata.PersonalAuthor, exifToolData, oldExifToolAuthor,
                                    CompositeTags.Arthur, ref metadata.errors);
                                oldExifToolAuthor = new ExiftoolData(exifToolData);
                                break;

                            case CompositeTags.Description:
                            case "ImageDescription":
                            case "Caption-Abstract":
                                metadata.PersonalDescription = ConvertAndCheckStringFromString(metadata.PersonalDescription, exifToolData, oldExifToolDescription,
                                    CompositeTags.Description, ref metadata.errors);
                                oldExifToolDescription = new ExiftoolData(exifToolData);
                                break;

                            case CompositeTags.Title:
                            case "XP Title":
                            case "XPTitle":
                                metadata.PersonalTitle = ConvertAndCheckStringFromString(metadata.PersonalTitle, exifToolData, oldExifToolTitle,
                                    CompositeTags.Title, ref metadata.errors);
                                oldExifToolTitle = new ExiftoolData(exifToolData);
                                break;

                            case CompositeTags.Comment:
                            case "User Comment":
                            case "XP Comment":
                            case "UserComment":
                            case "Notes":
                                metadata.PersonalComments = ConvertAndCheckStringFromString(metadata.PersonalComments, exifToolData, oldExifToolComment,
                                    CompositeTags.Comment, ref metadata.errors);
                                oldExifToolComment = new ExiftoolData(exifToolData);
                                break;

                            case CompositeTags.Album:
                            case "Headline":
                                metadata.PersonalAlbum = ConvertAndCheckStringFromString(metadata.PersonalAlbum, exifToolData, oldExifToolAlbum,
                                    CompositeTags.Album, ref metadata.errors);
                                oldExifToolAlbum = new ExiftoolData(exifToolData);
                                break;
                            #endregion

                            #region Personal Rating
                            /*
                            Notes:
                            RatingPercent - Setting: 5 Stars = 99, 4 stars = 75, 3 stars = 50, 2 stars = 25, 1 star = 1, 0 stars = removes tags.  
                            Reading: 88-100+ = 5 Stars, 63-87 = 4 stars, 38-62 = 3 stars, 13-37 = 2 stars, 1-12 = 1 star, no tag or 0 = 0 star
                            */
                            case CompositeTags.Rating:
                                metadata.PersonalRating = ConvertAndCheckByteFromString(metadata.PersonalRating, exifToolData, oldExifToolRating,
                                    CompositeTags.Rating, ref metadata.errors);
                                oldExifToolRating = new ExiftoolData(exifToolData);

                                oldExifToolRatingPercent = new ExiftoolData(exifToolData); //Use this Rating as old. To keep sync
                                exifToolData.Parameter = ((float)metadata.PersonalRatingPercent).ToString(CultureInfo.InvariantCulture);
                                metadata.PersonalRatingPercent = ConvertAndCheckByteFromString(metadata.PersonalRatingPercent, exifToolData, oldExifToolRatingPercent,
                                    CompositeTags.RatingPercent, ref metadata.errors);
                                oldExifToolRatingPercent = new ExiftoolData(exifToolData);

                                break;
                            case CompositeTags.RatingPercent:
                                //Reading: 88-100+ = 5 Stars, 63-87 = 4 stars, 38-62 = 3 stars, 13-37 = 2 stars, 1-12 = 1 star, no tag or 0 = 0 star
                                metadata.PersonalRatingPercent = ConvertAndCheckByteFromString(metadata.PersonalRatingPercent,
                                    exifToolData, oldExifToolRatingPercent,
                                    CompositeTags.RatingPercent, ref metadata.errors);
                                oldExifToolRatingPercent = new ExiftoolData(exifToolData);

                                oldExifToolRating = new ExiftoolData(exifToolData); //Use this RatingPercent as old. To keep sync
                                exifToolData.Parameter = ((byte)metadata.PersonalRating).ToString(CultureInfo.InvariantCulture);
                                metadata.PersonalRating = ConvertAndCheckByteFromString(metadata.PersonalRating, exifToolData, oldExifToolRating,
                                    CompositeTags.Rating, ref metadata.errors);
                                oldExifToolRating = new ExiftoolData(exifToolData);

                                break;
                            #endregion

                            #region De-Serialization structed - Region (with 'exiftool -struct')
                            case CompositeTags.FaceRegionIPTC:
                            case "MyRegion":        //Struct ImageRegion IPTC ImageRegion list
                                MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.FaceRegionIPTC);
                                RegionStructure regionIPTC = new RegionStructure();
                                regionIPTC.RegionStructureType = RegionStructureTypes.IptcRelative; //Relative or pixel
                                regionIPTC.Type = "Face";
                                //{ 'RbX','RbY','RbW','RbH'} if ($$rgn{ RegionBoundary} { RbUnit} eq 'pixel')  'relative' {
                                throw new Exception("Not implemented yet");
                            //break;
                            case CompositeTags.FaceRegionMicrosoft:
                            case CompositeTags.FaceRegionMWG:
                            case "RegionInfo":      //MWG RegionInfo
                            case "RegionInfoMP":    //Microsoft RegionInfoMP
                                                    //De-Serialization 

                                List<RegionStructure> readRegionList = new List<RegionStructure>();
                                
                                if (parameter == "{Regions=}") break; //Empty list, skip
                                if (parameter == "{RegionList=}") break; //Empty list, skip

                                string currentRegionCompositeTag;
                                RegionStructureTypes currentExiftoolRegionStructureType;
                                switch (exifToolData.Command)
                                {
                                    case "RegionInfoMP":
                                        currentRegionCompositeTag = CompositeTags.FaceRegionMicrosoft;
                                        currentExiftoolRegionStructureType = RegionStructureTypes.WindowsLivePhotoGallery;
                                        MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, currentRegionCompositeTag);
                                        
                                        break;
                                    case "RegionInfo":
                                        currentRegionCompositeTag = CompositeTags.FaceRegionMWG;
                                        currentExiftoolRegionStructureType = RegionStructureTypes.MetadataWorkingGroup;
                                        MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, currentRegionCompositeTag);

                                        break;
                                    default:
                                        throw new Exception("Not implemented yet");
                                }

                                RegionStructure region = null;
                                StructDeSerialization structDeSerialization = new StructDeSerialization(parameter);
                                StructObject structObject;
                                string lastKnownFieldName = "";

                                while (structDeSerialization.Read(out structObject))
                                {
                                    switch (structObject.Type)
                                    {
                                        case StructTypes.EOF:
                                            break;
                                        case StructTypes.OpeningSquareBrackets:
                                            break;
                                        case StructTypes.ClosingSquareBrackets:
                                            break;
                                        case StructTypes.FieldName:
                                            lastKnownFieldName = structObject.Value;
                                            break;
                                        case StructTypes.OpeningCurlyBracket:
                                            if (structObject.IsList && region == null)
                                            {
                                                region = new RegionStructure();
                                                region.RegionStructureType = currentExiftoolRegionStructureType;
                                            }
                                            break;
                                        case StructTypes.ClosingCurlyBracket:
                                            //break;
                                            goto case StructTypes.Value;
                                        case StructTypes.Value:

                                            switch (lastKnownFieldName)
                                            {
                                                #region WindowsLivePhotoGallery
                                                case "Regions":
                                                    break;
                                                case "PersonDisplayName": //WindowsLivePhotoGallery
                                                    region.Name = structObject.Value;
                                                    break;

                                                case "Rectangle": //WindowsLivePhotoGallery --> 0.086957|, 0.125926|, 0.136364|, 0.102222
                                                    try
                                                    {
                                                        string[] valueArray = structObject.Value.Split(',');
                                                        region.AreaX = float.Parse(valueArray[0], CultureInfo.InvariantCulture);
                                                        region.AreaY = float.Parse(valueArray[1], CultureInfo.InvariantCulture);
                                                        region.AreaWidth = float.Parse(valueArray[2], CultureInfo.InvariantCulture);
                                                        region.AreaHeight = float.Parse(valueArray[3], CultureInfo.InvariantCulture);
                                                    }
                                                    catch
                                                    {
                                                        region.AreaX = 0;
                                                        region.AreaY = 0;
                                                        region.AreaWidth = 1;
                                                        region.AreaHeight = 1;
                                                    }
                                                    break;
                                                #endregion

                                                #region MetadataWorkingGroup
                                                case "AppliedToDimensions":
                                                    break;
                                                case "RegionList":
                                                    break;
                                                case "Area":
                                                    break;
                                                case "X":
                                                case "Y":
                                                case "H":
                                                case "W":
                                                    if (structObject.IsList)
                                                    {
                                                        float coordinate = float.Parse(structObject.Value, CultureInfo.InvariantCulture);
                                                        switch (lastKnownFieldName)
                                                        {
                                                            case "X":
                                                                region.AreaX = coordinate;
                                                                break;
                                                            case "Y":
                                                                region.AreaY = coordinate;
                                                                break;
                                                            case "H":
                                                                region.AreaHeight = coordinate;
                                                                break;
                                                            case "W":
                                                                region.AreaWidth = coordinate;
                                                                break;
                                                        }
                                                    }
                                                    break;
                                                case "Type":
                                                    region.Type = structObject.Value;
                                                    break;
                                                case "Name":
                                                    region.Name = structObject.Value;
                                                    break;
                                                #endregion

                                                default:
                                                    break; //Closing curves
                                            }
                                            lastKnownFieldName = "";

                                            #region Add to Region List
                                            switch (currentExiftoolRegionStructureType)
                                            {
                                                case RegionStructureTypes.WindowsLivePhotoGallery:
                                                    if (structObject.Type == StructTypes.ClosingCurlyBracket)
                                                    {
                                                        if (region != null)
                                                        {
                                                            if (String.IsNullOrWhiteSpace(region.Type)) region.Type = "Face";

                                                            if (!readRegionList.Contains(region)) readRegionList.Add(region);
                                                            metadata.PersonalRegionListAddIfNotExists(region);
                                                            region = null;
                                                        }

                                                    }
                                                    break;
                                                case RegionStructureTypes.MetadataWorkingGroup:
                                                    if (structObject.Level == 0 &&
                                                        structObject.IsList &&
                                                        structObject.Type == StructTypes.ClosingCurlyBracket)
                                                    {
                                                        if (region != null)
                                                        {
                                                            if (String.IsNullOrWhiteSpace(region.Type)) region.Type = "Face";
                                                            if (!readRegionList.Contains(region)) readRegionList.Add(region);
                                                            metadata.PersonalRegionListAddIfNotExists(region);
                                                            region = null;
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    throw new Exception("Not implemented yet");
                                            }
                                            #endregion
                                            break;
                                        default:
                                            break;

                                    }

                                }

                                if (structDeSerialization.Level != -1) throw new Exception("Error in Exiftool seralization. Missing closing curves");

                                CheckRegionList(oldRegionList, readRegionList, metadata.MediaSize, exifToolData, oldExifToolKeywords, currentRegionCompositeTag, ref metadata.errors);
                                oldRegionList = readRegionList;
                                oldExifToolRegion = new ExiftoolData(exifToolData);

                                break;
                            #endregion

                            #region De-Serialization structed - PersonalKeywordTags 
                            case CompositeTags.KeywordsXML:
                            case "Categories": //XML
                                #region Read XML Categories
                                MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.KeywordsXML);

                                List<string> keywordListXML = new List<string>();
                                XmlReader xmlReader = XmlReader.Create(new StringReader(parameter));
                                string tagHierarchy = "";
                                bool assigned = false;
                                while (xmlReader.Read())
                                {
                                    switch (xmlReader.NodeType)
                                    {
                                        case XmlNodeType.Element:
                                            if (xmlReader.GetAttribute("Assigned") == "1") assigned = true;
                                            else assigned = false;
                                            break;
                                        case XmlNodeType.Text:
                                            if (assigned)
                                            {
                                                tagHierarchy += xmlReader.Value;
                                                if (!keywordListXML.Contains(tagHierarchy)) keywordListXML.Add(tagHierarchy);
                                                KeywordTag keywordTag = new KeywordTag(tagHierarchy);
                                                metadata.PersonalKeywordTagsAddIfNotExists(keywordTag);
                                                tagHierarchy = "";
                                            } else
                                            {
                                                tagHierarchy += xmlReader.Value + "/";
                                            }
                                            break;
                                        case XmlNodeType.EndElement:
                                            break;
                                    }
                                }


                                CheckKeywordList(oldKeywordList, keywordListXML, exifToolData, oldExifToolKeywords, CompositeTags.KeywordsXML, ref metadata.errors);
                                oldKeywordList = keywordListXML;
                                oldExifToolKeywords = new ExiftoolData(exifToolData);

                                #endregion
                                break;

                            case CompositeTags.KeywordsStruct:
                            case "Subject":
                            case "TagsList":
                            case "LastKeywordIPTC":
                            case "LastKeywordXMP":
                            case "HierarchicalSubject":
                            case "Category":    //Home made for store in QuickTime
                            case "Keyword":     //Home made for store in QuickTime
                            case "Keywords":
                            case "CatalogSets":

                                MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.KeywordsStruct);
                                List<string> keywordListStruct = StructDeSerialization.GetListOfValues(parameter);
                                
                                foreach (String tag in keywordListStruct)
                                {
                                    KeywordTag keywordTag = new KeywordTag(tag);
                                    metadata.PersonalKeywordTagsAddIfNotExists(keywordTag);
                                }

                                CheckKeywordList(oldKeywordList, keywordListStruct, exifToolData, oldExifToolKeywords, CompositeTags.KeywordsXML, ref metadata.errors);
                                oldKeywordList = keywordListStruct;
                                oldExifToolKeywords = new ExiftoolData(exifToolData);

                                break;
                            case CompositeTags.KeywordsMicrosoft:
                            case "XPKeywords": //Always ; sepearated list | EXIF:IFD0 XPKeywords Keyword1;Keyword2;Keyword3
                                MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.KeywordsMicrosoft);
                                List<string> keywordListXP = parameter.Split(';').ToList();
                                
                                foreach (String tag in keywordListXP)
                                {
                                    KeywordTag keywordTag = new KeywordTag(tag);
                                    metadata.PersonalKeywordTagsAddIfNotExists(keywordTag);
                                }

                                CheckKeywordList(oldKeywordList, keywordListXP, exifToolData, oldExifToolKeywords, CompositeTags.KeywordsXML, ref metadata.errors);
                                oldKeywordList = keywordListXP;
                                oldExifToolKeywords = new ExiftoolData(exifToolData);
                                break;
                            #endregion

                            #region Location
                            case CompositeTags.LocationStruct:
                            case "LocationCreated": //		LocationCreated		[{Sublocation=sdfsfdf}]
                                #region LocationCreated
                                if (parameter == "{Sublocation=}") break; //Empty list, skip

                                StructDeSerialization structDeSerializationLocation = new StructDeSerialization(parameter);
                                StructObject structObjectLocation;
                                string lastKnownFieldNameLocation = "";

                                while (structDeSerializationLocation.Read(out structObjectLocation))
                                {
                                    switch (structObjectLocation.Type)
                                    {
                                        case StructTypes.EOF:
                                            break;
                                        case StructTypes.OpeningSquareBrackets:
                                            break;
                                        case StructTypes.ClosingSquareBrackets:
                                            break;
                                        case StructTypes.FieldName:
                                            lastKnownFieldNameLocation = structObjectLocation.Value;
                                            break;
                                        case StructTypes.OpeningCurlyBracket:
                                            break;
                                        case StructTypes.ClosingCurlyBracket:
                                            //break;
                                            goto case StructTypes.Value;
                                        case StructTypes.Value:
                                            switch (lastKnownFieldNameLocation)
                                            {
                                                case "Sublocation":
                                                    exifToolData.Parameter = structObjectLocation.Value;
                                                    metadata.LocationName = ConvertAndCheckStringFromString(metadata.LocationName, exifToolData, oldExifToolLocationName,
                                                        CompositeTags.Location, ref metadata.errors);
                                                    oldExifToolLocationName = new ExiftoolData(exifToolData);
                                                    break;
                                            }
                                            lastKnownFieldNameLocation = ""; 
                                            break;
                                        default:
                                            break;

                                    }

                                }

                                if (structDeSerializationLocation.Level != -1) throw new Exception("Error in Exiftool seralization. Missing closing curves");
                                #endregion

                                break;
                            case CompositeTags.Location:
                            case "Sub-location":
                                metadata.LocationName = ConvertAndCheckStringFromString(metadata.LocationName, exifToolData, oldExifToolLocationName,
                                    CompositeTags.Location, ref metadata.errors);
                                oldExifToolLocationName = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.City:
                                metadata.LocationCity = ConvertAndCheckStringFromString(metadata.LocationCity, exifToolData, oldExifToolLocationCity,
                                    CompositeTags.City, ref metadata.errors);
                                oldExifToolLocationCity = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.State:
                            case "Province-State":
                                metadata.LocationState = ConvertAndCheckStringFromString(metadata.LocationState, exifToolData, oldExifToolLocationState,
                                    CompositeTags.State, ref metadata.errors);
                                oldExifToolLocationState = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.Country:
                            case "Country-PrimaryLocationName":
                                metadata.LocationCountry = ConvertAndCheckStringFromString(metadata.LocationCountry, exifToolData, oldExifToolLocationCountry,
                                    CompositeTags.Country, ref metadata.errors);
                                oldExifToolLocationCountry = new ExiftoolData(exifToolData);
                                break;
                            #endregion

                            #region GPS Location
                            
                            case "GPSAltitude":
                            case CompositeTags.GPSAltitude:
                                double? newAltitudeValue = ConvertAndCheckDoubleFromString(metadata.LocationAltitude, exifToolData, oldExifToolGPSAltitude,
                                    CompositeTags.GPSAltitude, ref metadata.errors);

                                if (metadata.LocationAltitude == null) metadata.LocationAltitude = newAltitudeValue;
                                oldExifToolGPSAltitude = new ExiftoolData(exifToolData);
                                break;
                            case "GPSLatitude":
                            case CompositeTags.GPSLatitude:
                                double? newLatitudeValue = ConvertAndCheckDoubleFromString(metadata.LocationLatitude,
                                    exifToolData, oldExifToolGPSLatitude,
                                    CompositeTags.GPSLatitude, ref metadata.errors);

                                if (metadata.LocationLatitude == null) metadata.LocationLatitude = newLatitudeValue;
                                if (regionType == "XMP:XMP-exif" && newLatitudeValue != null) metadata.LocationLatitude = newLatitudeValue;
                                oldExifToolGPSLatitude = new ExiftoolData(exifToolData);
                                break;
                            case "GPSLongitude":
                            case CompositeTags.GPSLongitude:
                                //BUG In ExifTool data, some postition is truncated in EXIF:GPS
                                //EXIF:GPS          GPSLongitude    27
                                //XMP:XMP-exif      GPSLongitude    27.4822166666667
                                double? newLongitudeValue = ConvertAndCheckDoubleFromString(metadata.LocationLongitude, exifToolData, oldExifToolGPSLongitude,
                                    CompositeTags.GPSLongitude, ref metadata.errors);
                                if (metadata.LocationLongitude == null) metadata.LocationLongitude = newLongitudeValue; //Override only if null value
                                oldExifToolGPSLongitude = new ExiftoolData(exifToolData);
                                break;
                            case CompositeTags.GPSCoordinates:
                            case "GPS Position":
                            case "GPSPosition":
                                MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.GPSCoordinates);
                                String[] coordinates = exifToolData.Parameter.Split(' ');

                                ExiftoolData tempExiftoolData = new ExiftoolData(exifToolData);
                                tempExiftoolData.Parameter = coordinates[0];
                                double? newLocationLatitude = ConvertAndCheckDoubleFromString(metadata.LocationLatitude, tempExiftoolData, oldExifToolGPSLatitude,
                                    CompositeTags.GPSCoordinatesLatitude, ref metadata.errors);
                                if (newLocationLatitude == null) metadata.LocationLatitude = newLocationLatitude;
                                oldExifToolGPSLatitude = new ExiftoolData(exifToolData);

                                tempExiftoolData.Parameter = coordinates[1];
                                double? newLocationLongitude = ConvertAndCheckDoubleFromString(metadata.LocationLongitude, tempExiftoolData, oldExifToolGPSLongitude,
                                    CompositeTags.GPSCoordinatesLongitude, ref metadata.errors);
                                if (newLocationLongitude == null) metadata.LocationLongitude = newLocationLongitude;
                                oldExifToolGPSLongitude = new ExiftoolData(exifToolData);

                                break;
                            case CompositeTags.GPSDateTime:
                            case "GPSDateTime":
                                //if (regionType == "XMP:XMP-exif" && command == "GPSDateTime")
                                //{                                   
                                    if (!exifToolData.Parameter.EndsWith("Z", true, CultureInfo.InvariantCulture)) 
                                        exifToolData.Parameter += "Z"; //GPS Time needs to be UTC
                                //}

                                metadata.LocationDateTime = ConvertAndCheckDateFromString(metadata.LocationDateTime,
                                    exifToolData, oldExifToolGPSDateTime,
                                    CompositeTags.GPSDateTime, ref metadata.errors);
                                
                                oldExifToolGPSDateTime = new ExiftoolData(exifToolData);
                                break;
                                #endregion
                        }
                        MetadataReadPrioity.Add(exifToolData.Region, exifToolData.Command, CompositeTags.NotDefined);

                    }
                    #endregion

                    
                }
              
                process.WaitForExit(1500);

                #region Write last Metadata and trigger Event afterNewMediaFoundEvent
                if (metadata != null) //Save the last one, remover we save everytime, when new file found
                {
                    CheckTimeZoom(metadata, ref metadata.errors);

                    ConvertRegion(metadata.PersonalRegionList, tempRegionRectangle, tempRegionPersonDisplayName);
                    ConvertRegion(metadata.PersonalRegionList, tempRegionType, tempRegionName,
                        tempRegionAreaH, tempRegionAreaW, tempRegionAreaX, tempRegionAreaY);

                    metaDataCollections.Add(metadata);

                    metadataDatabaseCache.TransactionBeginBatch();
                    metadataDatabaseCache.Write(metadata);
                    metadataDatabaseCache.TransactionCommitBatch();

                    if (afterNewMediaFoundEvent != null) afterNewMediaFoundEvent.Invoke(new FileEntry(Path.Combine(metadata.FileDirectory, metadata.FileName), (DateTime)metadata.FileDateModified));
                }
                #endregion 

                #region Log error
                string error = "";
                while (!process.StandardError.EndOfStream)
                {
                    error += process.StandardError.ReadLine() + "\r\n";
                    Logger.Error("ERROR: " + error);
                    throw new Exception(error);
                }
                #endregion 

            } //Using process
            return metaDataCollections;
        }
        #endregion

        #endregion

    }
}