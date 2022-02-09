using DataGridViewGeneric;
using Exiftool;
using FileDateTime;
using LocationNames;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Thumbnails;
using TimeZone;
using static Manina.Windows.Forms.ImageListView;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerDate
    {
        public const string headerMedia = "Date&Time digitized";
        public const string tagMediaDateTaken = "Local time";
        public const string tagGPSLocationDateTime = "UTC media take";
        public const string tagLocationOffsetTimeZone = "GPS Time Zone";
        public const string tagCalulatedOffsetZimeZone = "Estimated Time Zone";

        public const string headerSuggestion = "Correction suggestion";
        public const string tagSuggestedLocationTime = "Suggestion from GPS";
        public const string tagWhenUsedHomeClock = "Home clock when travel";
        public const string tagTravelClockAtHome = "Tavel clock when Home";

        public const string headerLocationComputer = "Digitized Local time";
        public const string headerLocationGPS = "Digitized on location";
        public const string headerMetadataDates = "Windows filesystem";
        public const string headerDatesTimeInFilename = "Scraping filename";

        public const string tagFileDate = "File Date";
        public const string tagFileSmartDate = "File Smart Date";
        public const string tagFileDateCreated = "File Date Created";
        public const string tagFileDateModified = "File Date Modified";
        public const string tagFileLastAccessed = "File Last Accessed";

        public const string tagDatesFoundInFilename = "Found ";

        public static bool HasBeenInitialized { get; set; } = false;
        public static DataGridView DataGridViewMap { get; set; }
        public static string DataGridViewMapHeaderMedia { get; set; }
        public static string DataGridViewMapTagCoordinates { get; set; }

        public static ThumbnailPosterDatabaseCache DatabaseAndCacheThumbnail { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }
        public static ExiftoolDataDatabase DatabaseExiftoolData { get; set; }

        #region GetDateTaken
        public static DateTime? GetUserInputDateTaken(DataGridView dataGridView, int? columnIndex, FileEntryAttribute fileEntryAttribute)
        {
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return null;
            if (columnIndex == null) columnIndex = DataGridViewHandler.GetColumnIndexUserInput(dataGridView, fileEntryAttribute);
            if (columnIndex == -1) return null;
            if (!DataGridViewHandler.IsColumnPopulated(dataGridView, (int)columnIndex)) return null;

            string dateTimeStringMediaTaken = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, (int)columnIndex, headerMedia, tagMediaDateTaken);
            return TimeZoneLibrary.ParseDateTimeAsLocal(dateTimeStringMediaTaken);
        }
        #endregion

        #region GetLocationDate
        public static DateTime? GetUserInputLocationDate(DataGridView dataGridView, int? columnIndex, FileEntryAttribute fileEntryAttribute)
        {
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return null;
            if (columnIndex == null) columnIndex = DataGridViewHandler.GetColumnIndexUserInput(dataGridView, fileEntryAttribute);
            if (columnIndex == -1) return null;
            if (!DataGridViewHandler.IsColumnPopulated(dataGridView, (int)columnIndex)) return null;

            string dateTimeStringLocation = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, (int)columnIndex, headerMedia, tagGPSLocationDateTime);
            DateTime? date = TimeZoneLibrary.ParseDateTimeAsUTC(dateTimeStringLocation);
            return date;
        }
        #endregion

        #region GetUserInputChanges
        //Check what data has been updated by users
        public static void GetUserInputChanges(ref KryptonDataGridView dataGridView, Metadata metadata, FileEntryAttribute fileEntryColumn)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndexUserInput(dataGridView, fileEntryColumn);
            if (columnIndex == -1) return; //Column has not yet become aggregated or has already been removed
            if (!DataGridViewHandler.IsColumnPopulated(dataGridView, columnIndex)) return;

            //Get Date and Time for DataGridView
            metadata.MediaDateTaken = GetUserInputDateTaken(dataGridView, columnIndex, null);
            metadata.LocationDateTime = GetUserInputLocationDate(dataGridView, columnIndex, null);
            if (metadata.LocationDateTime != null) metadata.LocationDateTime = new DateTime(((DateTime)metadata.LocationDateTime).Ticks, DateTimeKind.Local);
        }
        #endregion

        #region PopulateTimeZone
        public static void PopulateTimeZone(DataGridView dataGridViewDateTime, int? columnIndexDateTime, FileEntryAttribute fileEntryAttribute)
        {
            #region Check if all data IsAgregated, //need this check, due to Maps tab also updated this, when coordinates has been updated
            if (!DataGridViewHandler.GetIsAgregated(dataGridViewDateTime)) return; 
            if (columnIndexDateTime == null) columnIndexDateTime = DataGridViewHandler.GetColumnIndexUserInput(dataGridViewDateTime, fileEntryAttribute);
            if (columnIndexDateTime == -1) return;
            int columnIndex = (int)columnIndexDateTime;
            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridViewDateTime, columnIndex);
            if (dataGridViewGenericColumn == null) return;
            #endregion

            #region Get Media Date&Time and GPS Location Date&time from DataGridView or use Metadata
            //Get Date and Time for DataGridView
            DateTime? metadataMediaDateTaken = GetUserInputDateTaken(dataGridViewDateTime, columnIndex, null); 
            DateTime? metadataLocationDateTime = GetUserInputLocationDate(dataGridViewDateTime, columnIndex, null);
            if (metadataMediaDateTaken == null) metadataMediaDateTaken = dataGridViewGenericColumn?.Metadata?.MediaDateTaken;
            if (metadataLocationDateTime == null) metadataLocationDateTime = dataGridViewGenericColumn?.Metadata?.LocationDateTime;
            #endregion

            #region Get GPS Coorindates - 1. DataGridViewMap user input, 2. Metadata record 3. null 
            //Get Media GPS Coordinates from DataGridViewMap is exist or use Metadata coordinates
            double ? metadataLocationLatitude;
            double? metadataLocationLongitude;

            //If DataGridViewMap is agregated then pick up coordinates from what user have entered
            LocationCoordinate locationCoordinate = DataGridViewHandlerMap.GetUserInputLocationCoordinate(DataGridViewMap, null, dataGridViewGenericColumn.FileEntryAttribute);
            if (locationCoordinate != null)
            {
                metadataLocationLatitude = locationCoordinate.Latitude;
                metadataLocationLongitude = locationCoordinate.Longitude;
            } 
            else 
            {
                metadataLocationLatitude = dataGridViewGenericColumn?.Metadata?.LocationLatitude;
                metadataLocationLongitude = dataGridViewGenericColumn?.Metadata?.LocationLongitude;
            }
            #endregion 


            //------------------------------------
            DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion), false);


            if (metadataLocationLatitude != null && metadataLocationLongitude != null)
            {
                TimeZoneInfo timeZoneInfoGPSLocation = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataLocationLatitude, (double)metadataLocationLongitude);

                DateTime findOffsettDateTime;
                if (metadataLocationDateTime != null) findOffsettDateTime = (DateTime)metadataLocationDateTime;
                else if (metadataLocationDateTime != null) findOffsettDateTime = (DateTime)metadataMediaDateTaken;
                else findOffsettDateTime = DateTime.Now;

                //Media header
                DateTime findOffsettDateTimeUTC = findOffsettDateTime.ToUniversalTime();
                DateTimeOffset locationOffset = new DateTimeOffset(findOffsettDateTimeUTC.Ticks, timeZoneInfoGPSLocation.GetUtcOffset(findOffsettDateTimeUTC));

                DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerMedia, tagLocationOffsetTimeZone),
                        TimeZoneLibrary.ToStringOffset(locationOffset.Offset) + " " + TimeZoneLibrary.TimeZoneNameStandarOrDaylight(timeZoneInfoGPSLocation, findOffsettDateTimeUTC), true, false);

                DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerMedia, tagCalulatedOffsetZimeZone), "", true, false);

                //
                if (metadataLocationDateTime != null)
                {
                    DateTime locationDateTimeUTC = ((DateTime)metadataLocationDateTime).ToUniversalTime();
                    DateTime dateTimeFromGPS = new DateTime(locationDateTimeUTC.Ticks).Add(locationOffset.Offset);
                    DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagSuggestedLocationTime),
                        TimeZoneLibrary.ToStringSortable(dateTimeFromGPS) + TimeZoneLibrary.ToStringOffset(locationOffset.Offset, false), true, false);
                }
                else
                {
                    DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagSuggestedLocationTime), "No GPS time found", true, false);
                }

                if (metadataMediaDateTaken != null)
                {
                    DateTime mediaTakenDateTimeUTC = ((DateTime)metadataMediaDateTaken).ToUniversalTime();
                    DateTimeOffset mediaTakenDateTimeOffsetUTC = new DateTimeOffset(mediaTakenDateTimeUTC.Ticks, timeZoneInfoGPSLocation.GetUtcOffset(mediaTakenDateTimeUTC));

                    TimeSpan timeZoneDifferenceLocalAndLocation = timeZoneInfoGPSLocation.BaseUtcOffset - TimeZoneInfo.Local.BaseUtcOffset;
                    DateTime dateTimeUsedHomeClockOnTravel = new DateTime(((DateTime)metadataMediaDateTaken).Ticks).Add(timeZoneDifferenceLocalAndLocation);

                    DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagWhenUsedHomeClock),
                        TimeZoneLibrary.ToStringSortable(dateTimeUsedHomeClockOnTravel) + TimeZoneLibrary.ToStringOffset(mediaTakenDateTimeOffsetUTC.Offset, false), true, false);

                    timeZoneDifferenceLocalAndLocation = TimeZoneInfo.Local.BaseUtcOffset - timeZoneInfoGPSLocation.BaseUtcOffset;
                    dateTimeUsedHomeClockOnTravel = new DateTime(((DateTime)metadataMediaDateTaken).Ticks).Add(timeZoneDifferenceLocalAndLocation);
                    DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagTravelClockAtHome),
                        TimeZoneLibrary.ToStringSortable(dateTimeUsedHomeClockOnTravel) + TimeZoneLibrary.ToStringOffset(mediaTakenDateTimeOffsetUTC.Offset, false), true, false);
                }
                else
                {
                    DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagWhenUsedHomeClock), "Can't find local time", true, false);
                    DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagTravelClockAtHome), "Can't find local time", true, false);
                }

            }
            else
            {
                //Media header
                DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerMedia, tagLocationOffsetTimeZone), "No GPS location found", true, false);
                DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerMedia, tagCalulatedOffsetZimeZone), "No GPS location found", true, false);
                //Suggestion header
                DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagSuggestedLocationTime), "No GPS location found", true, false);
                DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagWhenUsedHomeClock), "No GPS location found", true, false);
                DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagTravelClockAtHome), "No GPS location found", true, false);
            }

            // -------------------------------------------------------
            string timeSpanString = "(±??:??)";
            TimeSpan? timeSpan = TimeZoneLibrary.CalulateTimeDiffrentWithoutTimeZone(metadataMediaDateTaken, metadataLocationDateTime);
            string prefredTimeZoneName = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridViewDateTime, columnIndex, headerMedia, tagLocationOffsetTimeZone);
            DateTime? dateTimeLocation = null;
            if (metadataMediaDateTaken != null) dateTimeLocation = new DateTime(((DateTime)metadataMediaDateTaken).Ticks, DateTimeKind.Utc);

            string timeZoneName = TimeZoneLibrary.GetTimeZoneName(timeSpan, dateTimeLocation, prefredTimeZoneName, out string timeZoneAlternatives);

            if (timeSpan != null) timeSpanString = TimeZoneLibrary.ToStringOffset((TimeSpan)timeSpan);
            
            
            int rowIndex = DataGridViewHandler.AddRow(dataGridViewDateTime, columnIndex,
                                new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagCalulatedOffsetZimeZone),
                                timeSpanString + " " + timeZoneName, true, false);

            DataGridViewHandler.SetCellToolTipText(dataGridViewDateTime, columnIndex, rowIndex, timeZoneAlternatives);

        }
        #endregion

        #region PopulateFile
        public static int PopulateFile(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, ShowWhatColumns showWhatColumns, Metadata metadataAutoCorrected, bool onlyRefresh)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return -1;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return -1;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return -1;  //In progress doing so
            
            //Check if file is in DataGridView
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fileEntryAttribute.FileFullPath)) return -1;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);

            //-----------------------------------------------------------------
            Image thumbnail = DatabaseAndCacheThumbnail.ReadThumbnailFromCacheOnly(fileEntryAttribute);
            FileEntryBroker fileEntryBrokerReadVersion = fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool);

            Metadata metadataExiftool = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBrokerReadVersion);
            if (metadataExiftool != null) metadataExiftool = new Metadata(metadataExiftool);
            if (metadataAutoCorrected != null) metadataExiftool = metadataAutoCorrected; //If AutoCorrect is run, use AutoCorrect values. Needs to be after DataGridViewHandler.AddColumnOrUpdateNew, so orignal metadata stored will not be overwritten
            
            ReadWriteAccess readWriteAccessColumn =
                (FileEntryVersionHandler.IsReadOnlyType(fileEntryAttribute.FileEntryVersion) ||
                metadataExiftool == null) ? ReadWriteAccess.ForceCellToReadOnly : ReadWriteAccess.AllowCellReadAndWrite;

            int columnIndex = DataGridViewHandler.AddColumnOrUpdateNew(
                dataGridView, fileEntryAttribute, thumbnail, metadataExiftool, readWriteAccessColumn, showWhatColumns,
                DataGridViewGenericCellStatus.DefaultEmpty(), out FileEntryVersionCompare fileEntryVersionCompareReason);

            //Chech if populated and new refresh data
            if (onlyRefresh && FileEntryVersionHandler.NeedUpdate(fileEntryVersionCompareReason) && !DataGridViewHandler.IsColumnPopulated(dataGridView, columnIndex))
                fileEntryVersionCompareReason = FileEntryVersionCompare.LostNoneEqualFound; //No need to populate
            //-----------------------------------------------------------------

            if (FileEntryVersionHandler.NeedUpdate(fileEntryVersionCompareReason))
            {
                //Media
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia), false);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagMediaDateTaken), TimeZoneLibrary.ToStringSortable(metadataExiftool?.MediaDateTaken), false, false);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagGPSLocationDateTime), TimeZoneLibrary.ToStringW3CDTF_UTC_Convert(metadataExiftool?.LocationDateTime), false, false);

                //Suggestion header
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion), false);

                //Dates and/or time in filename
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerDatesTimeInFilename), false);

                FileDateTimeReader fileDateTimeReader = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                List<DateTime> dates = fileDateTimeReader.ListAllDateTimes(Path.GetFileNameWithoutExtension(fileEntryAttribute.FileFullPath));
                for (int i = 0; i < dates.Count; i++)
                {
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerDatesTimeInFilename, tagDatesFoundInFilename + (i + 1).ToString()), TimeZoneLibrary.ToStringSortable(dates[i]), true, false);
                }

                //Metadata
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates), false);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileDate), TimeZoneLibrary.ToStringW3CDTF(metadataExiftool?.FileDate), true, false);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileSmartDate), TimeZoneLibrary.ToStringW3CDTF(metadataExiftool?.FileSmartDate(Properties.Settings.Default.RenameDateFormats)), true, false);

                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileDateCreated), TimeZoneLibrary.ToStringW3CDTF(metadataExiftool?.FileDateCreated), true, false);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileDateModified), TimeZoneLibrary.ToStringW3CDTF(metadataExiftool?.FileDateModified), true, false);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileLastAccessed), TimeZoneLibrary.ToStringW3CDTF(metadataExiftool?.FileDateAccessed), true, false);

                //Exiftool data
                List<ExiftoolData> exifToolDataList = DatabaseExiftoolData.Read(fileEntryBrokerReadVersion);
                string lastRegion = "";
                foreach (ExiftoolData exiftoolData in exifToolDataList)
                {

                    if (lastRegion != exiftoolData.Region)
                    {
                        DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(exiftoolData.Region), null,
                            new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), false);
                        lastRegion = exiftoolData.Region;
                    }

                    if (exiftoolData.Command.Contains("Date") || exiftoolData.Command.Contains("Time") || exiftoolData.Command.Contains("UTC"))
                    {
                        DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(exiftoolData.Region, exiftoolData.Command), exiftoolData.Parameter,
                            new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true), true);
                    }
                }

                PopulateTimeZone(dataGridView, columnIndex, null);

                DataGridViewHandler.SetColumnPopulatedFlag(dataGridView, columnIndex, true);
            }

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
            return columnIndex;
        }
        #endregion

        #region PopulateSelectedFiles
        public static void PopulateSelectedFiles(DataGridView dataGridView, HashSet<FileEntry> imageListViewSelectItems, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (DataGridViewHandler.GetIsAgregated(dataGridView))      
            {
                //Normaly return if already if *Agregated*, but if, but we use data from Maps Tab, therfor update DataGridViewDate with new data from DataGridViewMap                
                for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
                {
                    PopulateTimeZone(dataGridView, columnIndex, null);
                }                
                return;
            }

            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;
            //Tell that work in progress, can start a new before done.
            DataGridViewHandler.SetIsPopulating(dataGridView, true);
            //Clear current DataGridView
            DataGridViewHandler.Clear(dataGridView, dataGridViewSize);
            DataGridViewHandler.SetDataGridViewAllowUserToAddRows(dataGridView, false);
            //Add Columns for all selected files, one column per select file
            DataGridViewHandlerCommon.AddColumnSelectedFiles(dataGridView, DatabaseAndCacheThumbnail, imageListViewSelectItems, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true)); //ReadOnly until data is read 
            //Add all default rows
            //AddRowsDefault(dataGridView);
            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------

            //Tell that work is done
            DataGridViewHandler.SetIsPopulating(dataGridView, false);
            //-----------------------------------------------------------------
        }
        #endregion
    }
}
