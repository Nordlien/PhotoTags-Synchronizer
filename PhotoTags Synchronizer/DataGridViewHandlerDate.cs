using DataGridViewGeneric;
using Exiftool;
using FileDateTime;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using TimeZone;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerDate
    {
        public const string headerMedia = "Date&Time digitized";
        public const string tagMediaDateTaken = "Locaction Local time";
        public const string tagGPSLocationDateTime = "UCT media take";
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
        
        public const string tagFileDateCreated = "File Date Created";
        public const string tagFileDateModified = "File Date Modified";
        public const string tagFileLastAccessed = "File Last Accessed";

        public const string tagDatesFoundInFilename = "Found ";

        public static DataGridView DataGridViewMap { get; set; }
        public static string DataGridViewMapHeaderMedia { get; set; }
        public static string DataGridViewMapTagCoordinates { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }
        public static ExiftoolDataDatabase DatabaseExiftoolData { get; set; }

        public static double MediaAiTagConfidence { get; set; }


        //Check what data has been updated by users
        public static void GetUserInputChanges(ref DataGridView dataGridView, Metadata metadata, FileEntry fileEntryColumn)
        {
            /*
            int keywordsStarts = DataGridViewHandler.GetRowHeadingItemStarts(dataGridView, headerKeywords);
            int keywordsEnds = DataGridViewHandler.GetRowHeadingItemsEnds(dataGridView, headerKeywords);

            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntryColumn);
                
            metadata.PersonalAlbum = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagAlbum); 
            metadata.PersonalTitle = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagTitle);
            metadata.PersonalDescription = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagDescription);
            metadata.PersonalComments = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagComments);
            metadata.PersonalAuthor = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagAuthor);

            byte rating = 255; //empty
            var ratingValue = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagRating);
            if (ratingValue == null) { }
            else if (ratingValue.GetType() == typeof(byte)) rating = (byte)ratingValue;
            else if (ratingValue.GetType() == typeof(int)) rating = (byte)(int)ratingValue;
            else if (ratingValue.GetType() == typeof(string)) byte.TryParse((string)ratingValue, out rating);

            if (rating >= 0 && rating <= 5)
                metadata.PersonalRating = rating;
            else
                metadata.PersonalRating = null;
                
            for (int rowIndex = keywordsStarts; rowIndex <= keywordsEnds; rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                    
                SwitchStates switchStates = DataGridViewHandler.GetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex);
                if (switchStates == SwitchStates.On)
                {
                    //Add tag
                    if (!metadata.PersonalKeywordTags.Contains(new KeywordTag(dataGridViewGenericRow.RowName)))
                        metadata.PersonalKeywordTags.Add(new KeywordTag(dataGridViewGenericRow.RowName));
                }
                else
                {
                    //Remove tag
                    if (metadata.PersonalKeywordTags.Contains(new KeywordTag(dataGridViewGenericRow.RowName)))
                        metadata.PersonalKeywordTags.Remove(new KeywordTag(dataGridViewGenericRow.RowName));
                }
                
            }
            */
        }

         

        public static void PopulateTimeZone(DataGridView dataGridView, int columnIndex)
        {
            #region Get Media Date&Timeand GPS Location Date&time 
            //Get Date and Time for DataGridView
            string dateTimeStringMediaTaken = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagMediaDateTaken).ToString().Trim();
            string dateTimeStringLocation = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagGPSLocationDateTime).ToString().Trim();
            DateTime? metadataMediaDateTaken = TimeZoneLibrary.ParseDateTimeAsUTC(dateTimeStringMediaTaken); 
            DateTime? metadataLocationDateTime = TimeZoneLibrary.ParseDateTimeAsUTC(dateTimeStringMediaTaken);
            #endregion

            #region Get GPS Coorindates - 1. DataGridViewMap user input, 2. Metadata record 3. null 
            //Get Media GPS Coordinates from DataGridViewMap is exist or use Metadata coordinates
            double? metadataLocationLatitude = null;
            double? metadataLocationLongitude = null;
            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            if (dataGridViewGenericColumn.Metadata != null)
            {
                metadataLocationLatitude = dataGridViewGenericColumn.Metadata?.LocationLatitude;
                metadataLocationLongitude = dataGridViewGenericColumn.Metadata?.LocationLongitude;
            }

            //If DataGridViewMap is agregated then pick up coordnates from what user have entered
            if (DataGridViewHandler.GetIsAgregated(DataGridViewMap))
            {
                string locationCoordinateString = (string)DataGridViewHandler.GetCellValueStringTrim(DataGridViewMap, columnIndex, DataGridViewMapHeaderMedia, DataGridViewMapTagCoordinates);
                LocationCoordinate locationCoordinate = LocationCoordinate.Parse(locationCoordinateString);

                if (locationCoordinate != null)
                {
                    metadataLocationLatitude = locationCoordinate.Latitude;
                    metadataLocationLongitude = locationCoordinate.Longitude;
                }
            }
            #endregion 


            //------------------------------------
            DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion));


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

                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagLocationOffsetTimeZone),
                        TimeZoneLibrary.ToStringOffset(locationOffset.Offset) + " " + TimeZoneLibrary.TimeZoneNameStandarOrDaylight(timeZoneInfoGPSLocation, findOffsettDateTimeUTC), true);

                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCalulatedOffsetZimeZone), "", true);

                //
                if (metadataLocationDateTime != null)
                {
                    DateTime locationDateTimeUTC = ((DateTime)metadataLocationDateTime).ToUniversalTime();
                    DateTime dateTimeFromGPS = new DateTime(locationDateTimeUTC.Ticks).Add(locationOffset.Offset);
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagSuggestedLocationTime),
                        TimeZoneLibrary.ToStringDateTimeSortable(dateTimeFromGPS) + TimeZoneLibrary.ToStringOffset(locationOffset.Offset, false), true);
                }
                else
                {
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagSuggestedLocationTime), "No GPS time found", true);
                }

                if (metadataMediaDateTaken != null)
                {
                    DateTime mediaTakenDateTimeUTC = ((DateTime)metadataMediaDateTaken).ToUniversalTime();
                    DateTimeOffset mediaTakenDateTimeOffsetUTC = new DateTimeOffset(mediaTakenDateTimeUTC.Ticks, timeZoneInfoGPSLocation.GetUtcOffset(mediaTakenDateTimeUTC));

                    TimeSpan timeZoneDifferenceLocalAndLocation = timeZoneInfoGPSLocation.BaseUtcOffset - TimeZoneInfo.Local.BaseUtcOffset;
                    DateTime dateTimeUsedHomeClockOnTravel = new DateTime(((DateTime)metadataMediaDateTaken).Ticks).Add(timeZoneDifferenceLocalAndLocation);

                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagWhenUsedHomeClock),
                        TimeZoneLibrary.ToStringDateTimeSortable(dateTimeUsedHomeClockOnTravel) + TimeZoneLibrary.ToStringOffset(mediaTakenDateTimeOffsetUTC.Offset, false), true);

                    timeZoneDifferenceLocalAndLocation = TimeZoneInfo.Local.BaseUtcOffset - timeZoneInfoGPSLocation.BaseUtcOffset;
                    dateTimeUsedHomeClockOnTravel = new DateTime(((DateTime)metadataMediaDateTaken).Ticks).Add(timeZoneDifferenceLocalAndLocation);
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagTravelClockAtHome),
                        TimeZoneLibrary.ToStringDateTimeSortable(dateTimeUsedHomeClockOnTravel) + TimeZoneLibrary.ToStringOffset(mediaTakenDateTimeOffsetUTC.Offset, false), true);
                }
                else
                {
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagWhenUsedHomeClock), "Can't find local time", true);
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagTravelClockAtHome), "Can't find local time", true);
                }

            }
            else
            {
                //Media header
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagLocationOffsetTimeZone), "No GPS location found", true);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCalulatedOffsetZimeZone), "No GPS location found", true);
                //Suggestion header
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagSuggestedLocationTime), "No GPS location found", true);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagWhenUsedHomeClock), "No GPS location found", true);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerSuggestion, tagTravelClockAtHome), "No GPS location found", true);
            }

            // -------------------------------------------------------


            TimeSpan? timeSpan = TimeZoneLibrary.CalulateTimeDiffrent(dateTimeStringMediaTaken, dateTimeStringLocation);

            string prefredTimeZoneName = DataGridViewHandler.GetCellValueStringTrim(dataGridView, columnIndex, headerMedia, tagLocationOffsetTimeZone);
            DateTime? dateTimeLocation = TimeZoneLibrary.ParseDateTimeAsUTC(dateTimeStringMediaTaken);
            
            string timeZoneName = TimeZoneLibrary.GetTimeZoneName(timeSpan, dateTimeLocation, prefredTimeZoneName, out string timeZoneAlternatives);
            string timeSpanString = "(±??:??)";
            if (timeSpan != null) timeSpanString = TimeZoneLibrary.ToStringOffset((TimeSpan)timeSpan);
            
            
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex,
                                new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagCalulatedOffsetZimeZone),
                                timeSpanString + " " + timeZoneName, true);

            DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, timeZoneAlternatives);

        }

        public static void PopulateFile(DataGridView dataGridView, string fullFilePath, ShowWhatColumns showWhatColumns, DateTime dateTimeForEditableMediaFile)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;  //In progress doing so

            //Check if file is in DataGridView
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fullFilePath)) return;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);
            //-----------------------------------------------------------------

            List<FileEntryBroker> fileVersionDates = DatabaseAndCacheMetadataExiftool.ListFileEntryDateVersions(MetadataBrokerTypes.ExifTool, fullFilePath);

            //If create a dummy column for newst, add this "dummy file" to queue
            if (dateTimeForEditableMediaFile == DataGridViewHandler.DateTimeForEditableMediaFile && fileVersionDates.Count > 0)
            {                
                fileVersionDates.Add(new FileEntryBroker(fullFilePath, DataGridViewHandler.DateTimeForEditableMediaFile, MetadataBrokerTypes.ExifTool));
            }

            foreach (FileEntryBroker fileEntryBroker in fileVersionDates)
            {
                int columnCountBeforeAddOrUpdate = DataGridViewHandler.GetColumnCount(dataGridView); //Rememebr coulmn count before AddColumnOrUpdate

                //If create a dummy column for newst, add the news meta data to it

                FileEntryBroker fileEntryBrokerReadVersion = fileEntryBroker;
                FileEntryBroker fileEntryBrokerColumnVersion = fileEntryBroker;

                Metadata metadata;
                if (fileEntryBroker.LastWriteDateTime == DataGridViewHandler.DateTimeForEditableMediaFile)
                {
                    //Find news version
                    DateTime newestDate = fileVersionDates[0].LastWriteDateTime;
                    foreach (FileEntryBroker fileEntryBrokerFindNewest in fileVersionDates)
                    {
                        if (fileEntryBrokerFindNewest.Broker == MetadataBrokerTypes.ExifTool && fileEntryBrokerFindNewest.LastWriteDateTime > newestDate && fileEntryBrokerFindNewest.LastWriteDateTime != DataGridViewHandler.DateTimeForEditableMediaFile)
                            newestDate = fileEntryBrokerFindNewest.LastWriteDateTime;
                    }

                    fileEntryBrokerReadVersion = new FileEntryBroker(fullFilePath, newestDate, MetadataBrokerTypes.ExifTool);
                    metadata = new Metadata(DatabaseAndCacheMetadataExiftool.ReadCache(fileEntryBrokerReadVersion));
                }
                else metadata = DatabaseAndCacheMetadataExiftool.ReadCache(fileEntryBrokerReadVersion);

                int columnIndex = DataGridViewHandler.AddColumnOrUpdate(dataGridView, new FileEntryImage(fileEntryBrokerColumnVersion),
                    metadata, dateTimeForEditableMediaFile,
                    DataGridViewHandler.IsCurrentFile(fileEntryBrokerColumnVersion, dateTimeForEditableMediaFile) ? ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Off, true));
                if (columnIndex == -1) continue;

                //Media
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia));
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagMediaDateTaken), TimeZoneLibrary.ToStringDateTimeSortable(metadata?.MediaDateTaken), false);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagGPSLocationDateTime), TimeZoneLibrary.ToStringW3CDTF_UTC_Convert(metadata?.LocationDateTime), false);

                //Dates and/or time in filename
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerDatesTimeInFilename));

                FileDateTimeReader fileDateTimeReader = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                List<string> dates = fileDateTimeReader.ListAllDateTimesFound(Path.GetFileNameWithoutExtension(fullFilePath));
                for (int i = 0; i < dates.Count; i++)
                {
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerDatesTimeInFilename, tagDatesFoundInFilename + (i + 1).ToString()), dates[i], true);
                }

                //Metadata
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates));
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileDateCreated), TimeZoneLibrary.ToStringW3CDTF(metadata?.FileDateCreated), true);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileDateModified), TimeZoneLibrary.ToStringW3CDTF(metadata?.FileDateModified), true);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileLastAccessed), TimeZoneLibrary.ToStringW3CDTF(metadata?.FileLastAccessed), true);



                //Exiftool data
                List<ExiftoolData> exifToolDataList = DatabaseExiftoolData.Read(fileEntryBrokerReadVersion);
                string lastRegion = "";
                foreach (ExiftoolData exiftoolData in exifToolDataList)
                {

                    if (lastRegion != exiftoolData.Region)
                    {
                        DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(exiftoolData.Region), null,
                            new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true));
                        lastRegion = exiftoolData.Region;
                    }

                    if (exiftoolData.Command.Contains("Date") || exiftoolData.Command.Contains("Time") || exiftoolData.Command.Contains("UTC"))
                    {
                        DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(exiftoolData.Region, exiftoolData.Command), exiftoolData.Parameter,
                            new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true));
                    }
                }

                PopulateTimeZone(dataGridView, columnIndex);
            }

            

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }


        public static void PopulateSelectedFiles(DataGridView dataGridView, ImageListViewSelectedItemCollection imageListViewSelectItems, bool useCurrentFileLastWrittenDate, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (DataGridViewHandler.GetIsAgregated(dataGridView))      
            {
                //Normaly return if already if *Agregated*, but if, but we use data from Maps Tab, therfor update DataGridViewDate with new data from DataGridVIewMap
                if (DataGridViewHandler.GetIsAgregated(DataGridViewMap))
                {
                    for (int columnIndex = 0; columnIndex < DataGridViewHandler.GetColumnCount(dataGridView); columnIndex++)
                    {
                        PopulateTimeZone(dataGridView, columnIndex);
                    }
                }
                return;
            }
            if (DataGridViewHandler.GetIsPopulating(dataGridView)) return;
            //Tell that work in progress, can start a new before done.
            DataGridViewHandler.SetIsPopulating(dataGridView, true);
            //Clear current DataGridView
            DataGridViewHandler.Clear(dataGridView, dataGridViewSize);
            //Add Columns for all selected files, one column per select file
            DataGridViewHandler.AddColumnSelectedFiles(dataGridView, imageListViewSelectItems, false, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns, 
                new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Off, true)); //ReadOnly until data is read
            //Add all default rows
            //AddRowsDefault(dataGridView);
            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------


            //Populate one and one of selected files, (new versions of files can be added)
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                PopulateFile(dataGridView, imageListViewItem.FullFileName, showWhatColumns, 
                    useCurrentFileLastWrittenDate ? imageListViewItem.DateModified : DataGridViewHandler.DateTimeForEditableMediaFile);
            }

            //-----------------------------------------------------------------
            //Unlock
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //Tell that work is done
            DataGridViewHandler.SetIsPopulating(dataGridView, false);
            //-----------------------------------------------------------------

        }
    }
}
