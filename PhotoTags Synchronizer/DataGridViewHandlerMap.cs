using CameraOwners;
using DataGridViewGeneric;
using GoogleLocationHistory;
using LocationNames;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Thumbnails;
using static Manina.Windows.Forms.ImageListView;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{


    public static class DataGridViewHandlerMap
    {
        public static bool HasBeenInitialized { get; set; } = false;
        public static ThumbnailDatabaseCache DatabaseAndCacheThumbnail { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }
        public static LocationNameLookUpCache DatabaseAndCacheLocationAddress { get; set; }
        public static CameraOwnersDatabaseCache DatabaseAndCacheCameraOwner { get; set; }
        public static GoogleLocationHistoryDatabaseCache DatabaseGoogleLocationHistory {get; set; }
        public static List<AutoKeywordConvertion> AutoKeywordConvertions { get; set; }
        public static int TimeZoneShift { get; set; } = 0;
        public static int AccepedIntervalSecound { get; set; } = 3600;

        public const string headerMedia = "Media";
        public const string headerMicrosoftPhotos = "Microsoft Photos";
        public const string headerWindowsLivePhotoGallery = "Windows Live Photo Gallery";
        public const string headerGoogleLocations = "Google Locations";
        public const string headerNearByLocations = "Near by photos";
        public const string headerWebScraping = "WebScraper";
        public const string headerNominatim = "Nominatim";        
        public const string headerBrowser = "Browser map";

        public const string tagCoordinates = "Coordinates";
        public const string tagCoordinatesNearByPhotos = "Coordinates";
        public const string tagCameraMakeModel = "Camera make/model";
        public const string tagCameraOwner = "Camera owner";
        public const string tagLocationName = "Location name";
        public const string tagCity = "City";
        public const string tagProvince = "Province";
        public const string tagCountry = "Country";

        #region GetUserInputChanges
        public static void GetUserInputChanges(ref KryptonDataGridView dataGridView, Metadata metadata, FileEntryAttribute fileEntryAttribute)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndexUserInput(dataGridView, fileEntryAttribute);
            if (columnIndex == -1) return; //Column has not yet become aggregated or has already been removed
            if (!DataGridViewHandler.IsColumnPopulated(dataGridView, columnIndex)) return;

            LocationCoordinate.TryParse(DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, headerMedia, tagCoordinates), out LocationCoordinate locationCoordinate);
            metadata.LocationCoordinate = locationCoordinate;

            metadata.LocationName = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagLocationName);
            if (metadata.LocationName != null) metadata.LocationName = metadata.LocationName.Trim();
            if (string.IsNullOrWhiteSpace(metadata.LocationName)) metadata.LocationName = null;

            metadata.LocationCity = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagCity);
            if (metadata.LocationCity != null) metadata.LocationCity = metadata.LocationCity.Trim();
            if (string.IsNullOrWhiteSpace(metadata.LocationCity)) metadata.LocationCity = null;

            metadata.LocationState = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagProvince);
            if (metadata.LocationState != null) metadata.LocationState = metadata.LocationState.Trim();
            if (string.IsNullOrWhiteSpace(metadata.LocationState)) metadata.LocationState = null;

            metadata.LocationCountry = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagCountry);
            if (metadata.LocationCountry != null) metadata.LocationCountry = metadata.LocationCountry.Trim();
            if (string.IsNullOrWhiteSpace(metadata.LocationCountry)) metadata.LocationCountry = null;
        }
        #endregion 

        #region Add Row
        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, false);
        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, cellReadOnly), false);            
            return rowIndex;
        }
        #endregion

        #region GetCameraOwner
        public static string GetUserInputCameraOwner(DataGridView dataGridViewMap, int? columnIndex, FileEntryAttribute fileEntryAttribute)
        {
            if (!DataGridViewHandler.GetIsAgregated(dataGridViewMap)) return null;
            if (columnIndex == null) columnIndex = DataGridViewHandler.GetColumnIndexUserInput(dataGridViewMap, fileEntryAttribute);
            if (columnIndex == -1) return null;
            if (!DataGridViewHandler.IsColumnPopulated(dataGridViewMap, (int)columnIndex)) return null;

            string cameraOwner = (string)DataGridViewHandler.GetCellValue(dataGridViewMap, (int)columnIndex, headerGoogleLocations, tagCameraOwner);
            if (cameraOwner == CameraOwnersDatabaseCache.MissingLocationsOwners) cameraOwner = null;
            return cameraOwner;
        }
        #endregion

        #region SetCameraOwner
        public static void SetCameraOwner(DataGridView dataGridView, int columnIndex, string value)
        {
            DataGridViewHandler.SetCellValue(dataGridView, columnIndex, headerGoogleLocations, tagCameraOwner, value);
        }
        #endregion

        #region GetLocationCoordinate
        public static LocationCoordinate GetUserInputLocationCoordinate(DataGridView dataGridViewMap, int? columnIndex, FileEntryAttribute fileEntryAttribute)
        {
            if (!DataGridViewHandler.GetIsAgregated(dataGridViewMap)) return null;
            if (columnIndex == null) columnIndex = DataGridViewHandler.GetColumnIndexUserInput(dataGridViewMap, fileEntryAttribute);
            if (columnIndex == -1) return null;
            if (!DataGridViewHandler.IsColumnPopulated(dataGridViewMap, (int)columnIndex)) return null;
            
            LocationCoordinate locationCoordinate = null;            
            string locationCoordinateString = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridViewMap, (int)columnIndex, headerMedia, tagCoordinates);
            locationCoordinate = LocationCoordinate.Parse(locationCoordinateString);
            return locationCoordinate;
        }
        #endregion 

        #region PopulateGrivViewMapCameraOwner
        private static DataGridViewComboBoxCell dataGridViewComboBoxCellCameraOwners = null;

        public static void PopulateCameraOwner(DataGridView dataGridView, int columnIndex, ReadWriteAccess readWriteAccessColumn, string cameraMake, string cameraModel)
        {
            int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, headerGoogleLocations, tagCameraOwner);
            string cameraOwner = DatabaseAndCacheCameraOwner.GetOwenerForCameraMakeModel(cameraMake, cameraModel);

            if (dataGridViewComboBoxCellCameraOwners == null || DatabaseAndCacheCameraOwner.IsCameraMakeModelAndOwnerDirty())
            {
                //Create or updated common dropbox for all cells with "List of camera owners"
                dataGridViewComboBoxCellCameraOwners = null;
                dataGridViewComboBoxCellCameraOwners = new DataGridViewComboBoxCell();
                dataGridViewComboBoxCellCameraOwners.FlatStyle = FlatStyle.Flat;
                dataGridViewComboBoxCellCameraOwners.Items.AddRange(DatabaseAndCacheCameraOwner.ReadCameraOwners().ToArray());
            }

            if (readWriteAccessColumn == ReadWriteAccess.AllowCellReadAndWrite)
            {                
                DataGridViewHandler.SetCellControlType(dataGridView, columnIndex, rowIndex, dataGridViewComboBoxCellCameraOwners);
               
                if (!string.IsNullOrWhiteSpace(cameraOwner) && dataGridViewComboBoxCellCameraOwners.Items.Contains(cameraOwner))
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, cameraOwner);
                else
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, null);
            }
            else
                DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, cameraOwner);




        }
        #endregion

        #region PopulateNearbyCoordinate
        public static void PopulateNearbyCoordinate(DataGridView dataGridView, int columnIndex,
            int timeZoneShift, int accepedIntervalSecound, DateTime date, DateTime? dateTaken, DateTime? locationDate)
        {
            List<Metadata> metadatasLocationBasedOnBestGuess = DatabaseGoogleLocationHistory.FindLocationBasedOtherMediaFiles
                (locationDate, dateTaken, date, //UseSmartDate ? metadata?.FileSmartDate(allowedDateFormats) : metadata?.FileDate,
                    AccepedIntervalSecound);

            int count = 0;
            foreach (Metadata metadataLocationBasedOnBestGuess in metadatasLocationBasedOnBestGuess)
            {
                string tag = tagCoordinatesNearByPhotos + (count == 0 ? "" : " " + count.ToString());
                if (metadataLocationBasedOnBestGuess != null && metadataLocationBasedOnBestGuess.LocationLatitude != null && metadataLocationBasedOnBestGuess.LocationLongitude != null)
                {
                    int rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNearByLocations, tag), metadataLocationBasedOnBestGuess.LocationCoordinate, true);
                    DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex,
                        "Date: " + (date == null ? "(Empty)" : date.ToString()) + "\r\n" +
                        (metadataLocationBasedOnBestGuess.FileDateCreated == null ? "" : "Date: " + metadataLocationBasedOnBestGuess.FileDateCreated.ToString() + " on found media\r\n") +

                        "MediaTaken: " + (dateTaken == null ? "(Empty)" : dateTaken.ToString()) + "\r\n" +
                        (metadataLocationBasedOnBestGuess.MediaDateTaken == null ? "" : "MediaTaken: " + metadataLocationBasedOnBestGuess.MediaDateTaken.ToString() + " on found media\r\n") +

                        "LocationDate: " + (locationDate == null ? "(Empty)" : locationDate.ToString()) + "\r\n" +
                        (metadataLocationBasedOnBestGuess.LocationDateTime == null ? "" : "LocationDate: " + metadataLocationBasedOnBestGuess.LocationDateTime.ToString() + " on found media\r\n")
                        );
                    dataGridView.ShowCellToolTips = true;
                }
                else
                {
                    AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNearByLocations, tag), "Not found", true);
                }
                count++;
            }
        }
        #endregion 

        #region PopulateGrivViewMapGoogle
        public static void PopulateGoogleHistoryCoordinate(DataGridView dataGridViewMap, int columnIndexMap, 
            int timeZoneShift, int accepedIntervalSecound, DateTime mediaCreateUTC)
        {
            string cameraOwner = GetUserInputCameraOwner(dataGridViewMap, columnIndexMap, null);
            if (string.IsNullOrWhiteSpace(cameraOwner))
            {
                DataGridViewHandler.SetCellValue(dataGridViewMap, columnIndexMap, headerGoogleLocations, tagCoordinates, "Need select camera owner");
                return;
            }

            Metadata metadataLocation = DatabaseGoogleLocationHistory.FindLocationBasedOnTime(cameraOwner, mediaCreateUTC, accepedIntervalSecound);
            if (metadataLocation != null)
                AddRow(dataGridViewMap, columnIndexMap, new DataGridViewGenericRow(headerGoogleLocations, tagCoordinates), metadataLocation.LocationCoordinate, true);
            else
                AddRow(dataGridViewMap, columnIndexMap, new DataGridViewGenericRow(headerGoogleLocations, tagCoordinates), 
                    "Not found: Coordinates timestamp " + mediaCreateUTC.ToShortDateString() + " " + mediaCreateUTC.ToShortTimeString() 
                    + " +/- " + accepedIntervalSecound + " secounds not found", true);                    
            
        }
        #endregion

        #region PopulateGoogleHistoryCoordinateAndNearby
        public static void PopulateGoogleHistoryCoordinateAndNearby(DataGridView dataGridViewMap, DataGridView dataGridViewDate, int columnIndexMap, int timeZoneShift, int accepedIntervalSecound)
        {
            #region Check if Aggegated
            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridViewMap, columnIndexMap);
            if (dataGridViewGenericColumn == null) return;

            Metadata metadata = dataGridViewGenericColumn.Metadata;
            if (metadata == null)
            {
                DataGridViewHandler.SetCellValue(dataGridViewMap, columnIndexMap, headerGoogleLocations, tagCoordinates, "No metadata loaded");
                return;
            }
            #endregion 


            DateTime? dateTaken = DataGridViewHandlerDate.GetUserInputDateTaken(dataGridViewDate, null, dataGridViewGenericColumn.FileEntryAttribute);
            DateTime? locationDate = DataGridViewHandlerDate.GetUserInputLocationDate(dataGridViewDate, null, dataGridViewGenericColumn.FileEntryAttribute);

            if (dateTaken == null) dateTaken = metadata.MediaDateTaken;
            if (locationDate == null) locationDate = metadata.LocationDateTime;

            if (dateTaken == null && locationDate == null)
            {
                DataGridViewHandler.SetCellValue(dataGridViewMap, columnIndexMap, headerGoogleLocations, tagCoordinates, "Missing Dates");
                return;
            }


            DateTime mediaCreateUTC;
            if (locationDate != null)
                mediaCreateUTC = (DateTime)locationDate;
            else 
                mediaCreateUTC = new DateTime( ((DateTime)dateTaken).Ticks, DateTimeKind.Utc).AddHours(timeZoneShift);
            
            PopulateGoogleHistoryCoordinate(
                dataGridViewMap, columnIndexMap, timeZoneShift, accepedIntervalSecound, mediaCreateUTC);
            PopulateNearbyCoordinate(
                dataGridViewMap, columnIndexMap, timeZoneShift, accepedIntervalSecound, (DateTime)metadata.FileDate, dateTaken, locationDate);
        }
        #endregion

        #region PopulateGrivViewMapNomnatatim
        public static void DeleteMapNomnatatim(LocationCoordinate locationCoordinate)
        {
            if (locationCoordinate != null) DatabaseAndCacheLocationAddress.DeleteLocation(locationCoordinate);
        }

        public static void PopulateGrivViewMapNomnatatim(DataGridView dataGridView, int columnIndex, LocationCoordinate locationCoordinate)
        {
            LocationCoordinateAndDescription locationCoordinateAndDescription = null;

            float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
            float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;

            if (locationCoordinate != null) locationCoordinateAndDescription = DatabaseAndCacheLocationAddress.AddressLookup(locationCoordinate, locationAccuracyLatitude, locationAccuracyLongitude);
            bool isReadOnly = (locationCoordinateAndDescription == null);
            //isReadOnly = false;
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim, tagLocationName, ReadWriteAccess.AllowCellReadAndWrite), 
                locationCoordinateAndDescription?.Description.Name, isReadOnly); 
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim, tagCity, ReadWriteAccess.AllowCellReadAndWrite), 
                locationCoordinateAndDescription?.Description.City, isReadOnly); 
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim, tagProvince, ReadWriteAccess.AllowCellReadAndWrite), 
                locationCoordinateAndDescription?.Description.Region, isReadOnly); 
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim, tagCountry, ReadWriteAccess.AllowCellReadAndWrite), 
                locationCoordinateAndDescription?.Description.Country, isReadOnly); 
           
        }
        #endregion

        
        #region PopulateFile
        public static void PopulateFile(DataGridView dataGridView, DataGridView dataGridViewDate, FileEntryAttribute fileEntryAttribute, ShowWhatColumns showWhatColumns, Metadata metadataAutoCorrected, bool onlyRefresh)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fileEntryAttribute.FileFullPath)) return;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);

            //-----------------------------------------------------------------
            Image thumbnail = DatabaseAndCacheThumbnail.ReadThumbnailFromCacheOnlyClone(fileEntryAttribute);
            FileEntryBroker fileEntryBrokerReadVersion = fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool);

            Metadata metadataExiftool = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBrokerReadVersion);
            if (metadataExiftool != null) metadataExiftool = new Metadata(metadataExiftool);
            ReadWriteAccess readWriteAccessColumn = metadataExiftool != null ? ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly;

            int columnIndex = DataGridViewHandler.AddColumnOrUpdateNew(
                dataGridView, fileEntryAttribute, thumbnail, metadataExiftool, readWriteAccessColumn, showWhatColumns,
                DataGridViewGenericCellStatus.DefaultEmpty(), out FileEntryVersionCompare fileEntryVersionCompareReason);

            if (metadataAutoCorrected != null) metadataExiftool = metadataAutoCorrected; //If AutoCorrect is run, use AutoCorrect values. Needs to be after DataGridViewHandler.AddColumnOrUpdateNew, so orignal metadata stored will not be overwritten

            //Chech if populated and new refresh data
            if (onlyRefresh && FileEntryVersionHandler.NeedUpdate(fileEntryVersionCompareReason) && !DataGridViewHandler.IsColumnPopulated(dataGridView, columnIndex))
                fileEntryVersionCompareReason = FileEntryVersionCompare.NotEqualFound; //No need to populate
            //-----------------------------------------------------------------

            if (FileEntryVersionHandler.NeedUpdate(fileEntryVersionCompareReason))
            {
                //Media
                int rowIndex;
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCoordinates), metadataExiftool?.LocationCoordinate, false);
                rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagLocationName), metadataExiftool?.LocationName, false);
                List<string> newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, metadataExiftool?.LocationName, null, null, null, null, null);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);
                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCity), metadataExiftool?.LocationCity, false);                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagProvince), metadataExiftool?.LocationState, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCountry), metadataExiftool?.LocationCountry, false);

                //List<string> newKeywords = AutoKeywordHandler.NewKeywords(autoKeywordConvertions, metadataCopy.LocationName, metadataCopy.PersonalTitle,
                //  metadataCopy.PersonalAlbum, metadataCopy.PersonalDescription, metadataCopy.PersonalComments, metadataCopy.PersonalKeywordTags);
                
                //Google location history
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations));

                if (metadataExiftool != null)
                {
                    CameraOwner cameraOwnerPrint = new CameraOwner(metadataExiftool.CameraMake, metadataExiftool.CameraModel, "");
                    AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCameraMakeModel), cameraOwnerPrint, true);
                    AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCameraOwner), "Owner???", false);

                    DataGridViewGenericColumn gridViewGenericColumnCheck = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
                    PopulateCameraOwner(dataGridView, columnIndex, readWriteAccessColumn, metadataExiftool.CameraMake, metadataExiftool.CameraModel);
                }
                else
                {
                    AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCameraMakeModel), "", false);
                    AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCameraOwner), "Select Camera owner/locations", true);
                }

                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCoordinates), metadataExiftool?.LocationCoordinate, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNearByLocations));

                PopulateGoogleHistoryCoordinateAndNearby(dataGridView, dataGridViewDate, columnIndex, TimeZoneShift, AccepedIntervalSecound);

                //Nominatim.API
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim));
                PopulateGrivViewMapNomnatatim(dataGridView, columnIndex, metadataExiftool?.LocationCoordinate);

                //WebScraper
                //headerWebScraping = "WebScraper";
                // WebScarping
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWebScraping));
                Metadata metadataWebScraping = null;
                if (metadataExiftool != null) metadataWebScraping = DatabaseAndCacheMetadataExiftool.ReadWebScraperMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.WebScraping));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWebScraping, tagLocationName), metadataWebScraping?.LocationName, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWebScraping, tagCountry), metadataWebScraping?.LocationCountry, true);

                //Microsoft Photos Locations
                Metadata metadataMicrosoftPhotos = null;
                if (metadataExiftool != null) metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(
                    new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.MicrosoftPhotos));

                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagCoordinates), metadataMicrosoftPhotos?.LocationCoordinate, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagLocationName), metadataMicrosoftPhotos?.LocationName, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagCity), metadataMicrosoftPhotos?.LocationCity, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagProvince), metadataMicrosoftPhotos?.LocationState, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagCountry), metadataMicrosoftPhotos?.LocationCountry, true);

                //Windows Live Photo Gallary Locations
                Metadata metadataWindowsLivePhotoGallery = null;
                if (metadataExiftool != null) metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(
                    new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.WindowsLivePhotoGallery));

                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagCoordinates), metadataWindowsLivePhotoGallery?.LocationCoordinate, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagLocationName), metadataWindowsLivePhotoGallery?.LocationName, true);

                //Browser
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerBrowser));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerBrowser, tagCoordinates), "", true);

                DataGridViewHandler.SetColumnPopulatedFlag(dataGridView, columnIndex, true);
            }

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }
        #endregion

        #region PopulateSelectedFiles
        public static void PopulateSelectedFiles(DataGridView dataGridView, HashSet<FileEntry> imageListViewSelectItems, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns)
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
            DataGridViewHandlerCommon.AddColumnSelectedFiles(dataGridView, DatabaseAndCacheMetadataExiftool, DatabaseAndCacheThumbnail, imageListViewSelectItems,  false, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns, new DataGridViewGenericCellStatus());
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