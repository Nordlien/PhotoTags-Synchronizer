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
        public const string headerMedia = "Media";
        public const string headerLocationComputer = "Computer time zone";
        public const string headerLocationGPS = "GPS Location time zone";
        public const string headerMetadataDates = "Windows filesystem";
        public const string headerDatesTimeInFilename = "Scraping filename";
        
        public const string tagMediaDateTaken = "MediaDateTaken";
        public const string tagFileDateCreated = "FileDateCreated";
        public const string tagFileDateModified = "FileDateModified";
        public const string tagFileLastAccessed = "FileLastAccessed";
        public const string tagGPSLocationDateTime = "GPS UTC DateTime";
        public const string tagTimeZone = "Time Zone offset";

        public const string tagDatesFoundInFilename = "Found ";

        public const string tagCalulateTimeZone = "TimeZone";
        public const string tagCalulateStadardDaylight = "Standard/Daylight";
        public const string tagCalulateDateTime = "Date and Time";

        

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
            DateTime? dateTimeLocation = TimeZoneLibrary.ParseDateTimeAsUTC(DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagMediaDateTaken).ToString().Trim());
            DateTime? dateTimeUTC = TimeZoneLibrary.ParseDateTimeAsUTC(DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagGPSLocationDateTime).ToString().Trim());

            string prefredTimeZoneName = DataGridViewHandler.GetCellValueStringTrim(dataGridView, columnIndex, headerLocationGPS, tagCalulateTimeZone);


            if (dateTimeLocation != null && dateTimeUTC != null)
            {
                
                //Remove time zone and location information so we can substract  
                TimeSpan timeSpan = 
                    new DateTime(
                        ((DateTime)dateTimeLocation).Year, ((DateTime)dateTimeLocation).Month, ((DateTime)dateTimeLocation).Day, 
                        ((DateTime)dateTimeLocation).Hour, ((DateTime)dateTimeLocation).Minute, ((DateTime)dateTimeLocation).Second, ((DateTime)dateTimeLocation).Millisecond) -
                    new DateTime(
                        ((DateTime)dateTimeUTC).Year, ((DateTime)dateTimeUTC).Month, ((DateTime)dateTimeUTC).Day,
                        ((DateTime)dateTimeUTC).Hour, ((DateTime)dateTimeUTC).Minute, ((DateTime)dateTimeUTC).Second, ((DateTime)dateTimeUTC).Millisecond);

                string timeZoneAlternatives;

                string timeZoneName = TimeZoneLibrary.GetTimeZoneName(timeSpan, (DateTime)dateTimeLocation, prefredTimeZoneName, out timeZoneAlternatives);
                int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex,
                                    new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagTimeZone),
                                    timeSpan.ToString().Substring(0, timeSpan.ToString().Length-3) + " " + timeZoneName, true);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, timeZoneAlternatives);
                
            }
            else
            {
                int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex,
                    new DataGridViewGenericRow(DataGridViewHandlerDate.headerMedia, DataGridViewHandlerDate.tagTimeZone),
                    "Error", false);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "");
            } 
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
                

                //Metadata
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates));
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileDateCreated), TimeZoneLibrary.ToStringW3CDTF(metadata?.FileDateCreated), true);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileDateModified), TimeZoneLibrary.ToStringW3CDTF(metadata?.FileDateModified), true);
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMetadataDates, tagFileLastAccessed), TimeZoneLibrary.ToStringW3CDTF(metadata?.FileLastAccessed), true);

                //Dates and/or time in filename
                DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerDatesTimeInFilename));

                FileDateTimeReader fileDateTimeReader = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                List<string> dates = fileDateTimeReader.ListAllDateTimesFound(Path.GetFileNameWithoutExtension(fullFilePath));
                for (int i = 0; i < dates.Count; i++)
                {
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerDatesTimeInFilename, tagDatesFoundInFilename + (i + 1).ToString()), dates[i], true);
                }

                DateTime localTime = metadata?.MediaDateTaken != null ? (DateTime)metadata?.MediaDateTaken : DateTime.Now;
                if (metadata?.MediaDateTaken != null && metadata?.LocationLatitude != null && metadata?.LocationLongitude != null)
                {
                    DateTime mediaDateTaken = (DateTime)metadata?.MediaDateTaken;

                    TimeZoneInfo timeZoneInfoComputerLocation = TimeZoneInfo.Local;
                    TimeZoneInfo timeZoneInfoGPSLocation = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadata?.LocationLatitude, (double)metadata?.LocationLongitude);

                    DateTimeOffset dateTimeUTC = new DateTimeOffset(mediaDateTaken, timeZoneInfoGPSLocation.GetUtcOffset(mediaDateTaken));

                    DateTime mediaDateTakeComputerLocation = TimeZoneInfo.ConvertTime(dateTimeUTC.UtcDateTime, TimeZoneInfo.Utc, timeZoneInfoComputerLocation);
                    DateTime mediaDateTakenGPSLocation = TimeZoneInfo.ConvertTime(dateTimeUTC.UtcDateTime, TimeZoneInfo.Utc, timeZoneInfoGPSLocation);

                    //"Calculation"

                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerLocationComputer));
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerLocationComputer, tagCalulateTimeZone),
                        timeZoneInfoComputerLocation.DisplayName, true);

                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerLocationComputer, tagCalulateStadardDaylight),
                        TimeZoneLibrary.TimeZoneNameStandarOrDaylight(timeZoneInfoComputerLocation, mediaDateTakeComputerLocation),
                        //timeZoneInfoComputerLocation.IsDaylightSavingTime(mediaDateTakeComputerLocation) ? timeZoneInfoComputerLocation.DaylightName : timeZoneInfoComputerLocation.StandardName, 
                        true);

                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerLocationComputer, tagCalulateDateTime),
                        TimeZoneLibrary.ToStringW3CDTF(mediaDateTakeComputerLocation), true);

                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerLocationGPS));
                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerLocationGPS, tagCalulateTimeZone),
                        timeZoneInfoGPSLocation.DisplayName, true);

                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerLocationGPS, tagCalulateStadardDaylight),
                        TimeZoneLibrary.TimeZoneNameStandarOrDaylight(timeZoneInfoGPSLocation, mediaDateTakenGPSLocation),
                        //timeZoneInfoGPSLocation.IsDaylightSavingTime(mediaDateTakenGPSLocation) ? timeZoneInfoGPSLocation.DaylightName : timeZoneInfoGPSLocation.StandardName, 
                        true);

                    DataGridViewHandler.AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerLocationGPS, tagCalulateDateTime),
                        TimeZoneLibrary.ToStringW3CDTF(mediaDateTakenGPSLocation), true);
                }
                

                //Exiftool data
                List<ExiftoolData> exifToolDataList = DatabaseExiftoolData.ExifToolData_Read(fileEntryBrokerReadVersion);
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
            if (DataGridViewHandler.GetIsAgregated(dataGridView)) return;
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
