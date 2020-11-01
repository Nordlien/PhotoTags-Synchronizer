using CameraOwners;
using DataGridViewGeneric;
using GoogleLocationHistory;
using LocationNames;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{


    public static class DataGridViewHandlerMap
    {
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }
        public static LocationNameLookUpCache DatabaseLocationAddress { get; set; }
        public static CameraOwnersDatabaseCache DatabaseAndCacheCameraOwner { get; set; }
        public static GoogleLocationHistoryDatabaseCache DatabaseGoogleLocationHistory {get; set; }
        public static int TimeZoneShift { get; set; } = 0;
        public static int AccepedIntervalSecound { get; set; } = 3600;

        public const string headerMedia = "Media";
        public const string headerMicrosoftPhotos = "Microsoft Photos";
        public const string headerWindowsLivePhotoGallery = "Windows Live Photo Gallery";
        public const string headerGoogleLocations = "Google Locations";
        public const string headerNominatim = "Nominatim";
        public const string headerBrowser = "Browser map";

        public const string tagCoordinates = "Coordinates";
        public const string tagCameraMakeModel = "Camera make/model";
        public const string tagCameraOwner = "Camera owner";
        public const string tagLocationName = "Location name";
        public const string tagCity = "City";
        public const string tagProvince = "Province";
        public const string tagCountry = "Country";

        public static void GetUserInputChanges(ref DataGridView dataGridView, Metadata metadata, FileEntry fileEntry)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntry);

            LocationCoordinate.TryParse(DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagCoordinates).ToString(), out LocationCoordinate locationCoordinate);
            metadata.LocationCoordinate = locationCoordinate;
            metadata.LocationName = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagLocationName);
            metadata.LocationCity= (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagCity);
            metadata.LocationState = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagProvince);
            metadata.LocationCountry = (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagCountry);            
        }

        #region Help functions
        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow);
        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value,
                new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, cellReadOnly));            
            return rowIndex;
        }
        #endregion 

        public static string GetCameraOwner(DataGridView dataGridView, int columnIndex)
        {
            return (string)DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerGoogleLocations, tagCameraOwner);
        }

        public static void SetCameraOwner(DataGridView dataGridView, int columnIndex, string value)
        {
            DataGridViewHandler.SetCellValue(dataGridView, columnIndex, headerGoogleLocations, tagCameraOwner, value);
        }

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

        #region PopulateGrivViewMapGoogle
        public static void PopulateGoogleHistoryCoordinate(DataGridView dataGridView, int columnIndex, 
            int timeZoneShift, int accepedIntervalSecound)
        {
            DataGridViewGenericColumn dataGridViewGenericColumn = DataGridViewHandler.GetColumnDataGridViewGenericColumn(dataGridView, columnIndex);
            
            if (dataGridViewGenericColumn.Metadata == null)
            {
                DataGridViewHandler.SetCellValue(dataGridView, columnIndex, headerGoogleLocations, tagCoordinates, "No metadata loaded");
                return;
            }
            if (dataGridViewGenericColumn.Metadata.MediaDateTaken == null)
            {
                DataGridViewHandler.SetCellValue(dataGridView, columnIndex, headerGoogleLocations, tagCoordinates, "Missing Date Taken");                
                return;
            }

            DateTime mediaCreateUTC = ((DateTime)dataGridViewGenericColumn.Metadata.MediaDateTaken).ToUniversalTime();
            mediaCreateUTC = mediaCreateUTC.AddHours(timeZoneShift);


            string cameraOwner = GetCameraOwner(dataGridView, columnIndex);

            if (string.IsNullOrWhiteSpace(cameraOwner))
            {
                DataGridViewHandler.SetCellValue(dataGridView, columnIndex, headerGoogleLocations, tagCoordinates, "Need select camera owner");
                return;
            }


            Metadata metadataLocation = DatabaseGoogleLocationHistory.FindLocationBasedOnTime(cameraOwner, mediaCreateUTC, accepedIntervalSecound);

            if (metadataLocation != null)
            {
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCoordinates), metadataLocation.LocationCoordinate, 
                    true);
            }
            else
            {
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCoordinates), 
                    mediaCreateUTC.ToShortDateString() + " " + mediaCreateUTC.ToShortTimeString() + " +/- " + accepedIntervalSecound + "secounds not found", 
                    true);                    
            }
        }
        #endregion

        #region PopulateGrivViewMapNomnatatim
        public static void PopulateGrivViewMapNomnatatim(DataGridView dataGridView, int columnIndex, LocationCoordinate locationCoordinate)
        {
            Metadata metadataLocation = null;
            if (locationCoordinate != null) metadataLocation = DatabaseLocationAddress.AddressLookup(locationCoordinate.Latitude, locationCoordinate.Longitude);
            
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim, tagLocationName), metadataLocation?.LocationName, false); 
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim, tagCity), metadataLocation?.LocationCity, false); 
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim, tagProvince), metadataLocation?.LocationState, false); 
            AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim, tagCountry), metadataLocation?.LocationCountry, false); 
           
        }
        #endregion 


        public static void PopulateFile(DataGridView dataGridView, string fullFilePath, ShowWhatColumns showWhatColumns, DateTime dateTimeForEditableMediaFile)

        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
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
                int columnCount = dataGridView.ColumnCount; //Rememebr coulmn count before AddColumnOrUpdate

                ReadWriteAccess readWriteAccessColumn = DataGridViewHandler.IsCurrentFile(fileEntryBroker, dateTimeForEditableMediaFile) ? ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly;
                
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

                int columnIndex = DataGridViewHandler.AddColumnOrUpdate(dataGridView, new FileEntryImage(fileEntryBrokerColumnVersion), metadata, dateTimeForEditableMediaFile, readWriteAccessColumn, showWhatColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Off, true));
                if (columnIndex == -1) continue;

                //Media
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCoordinates), metadata?.LocationCoordinate, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagLocationName), metadata?.LocationName, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCity), metadata?.LocationCity, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagProvince), metadata?.LocationState, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCountry), metadata?.LocationCountry, false);

                //Google location history
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations));

                if (metadata != null)
                {
                    CameraOwner cameraOwnerPrint = new CameraOwner(metadata.CameraMake, metadata.CameraModel, "");
                    AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCameraMakeModel), cameraOwnerPrint, true);
                
                    int rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCameraOwner), "Owner???", false);
                    PopulateCameraOwner(dataGridView, columnIndex, readWriteAccessColumn, metadata.CameraMake, metadata.CameraModel);

                    //DataGridViewHandler.SetCellReadOnlyStatus(dataGridView, columnIndex, rowIndex, false);

                }
                else 
                {
                    AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagCameraMakeModel), "", false);
                    AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCameraOwner), "Select Camera owner/locations", true);
                }

                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerGoogleLocations, tagCoordinates), metadata?.LocationCoordinate, true);
                PopulateGoogleHistoryCoordinate(dataGridView, columnIndex, TimeZoneShift, AccepedIntervalSecound);

                //Nominatim.API
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerNominatim));
                PopulateGrivViewMapNomnatatim(dataGridView, columnIndex, metadata?.LocationCoordinate);
                
                //Microsoft Photos Locations
                Metadata metadataMicrosoftPhotos = null;
                if (metadata != null) metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.ReadCache(
                    new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.MicrosoftPhotos));
                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagCoordinates), metadataMicrosoftPhotos?.LocationCoordinate, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagLocationName), metadataMicrosoftPhotos?.LocationName, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagCity), metadataMicrosoftPhotos?.LocationCity, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagProvince), metadataMicrosoftPhotos?.LocationState, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagCountry), metadataMicrosoftPhotos?.LocationCountry, true);
                
                //Windows Live Photo Gallary Locations
                Metadata metadataWindowsLivePhotoGallery = null;
                if (metadata != null) metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.ReadCache(
                    new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.WindowsLivePhotoGallery));

                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagCoordinates), metadataWindowsLivePhotoGallery?.LocationCoordinate, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagLocationName), metadataWindowsLivePhotoGallery?.LocationName, true);

                //Browser
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerBrowser));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerBrowser, tagCoordinates), "", true);
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
            DataGridViewHandler.AddColumnSelectedFiles(dataGridView, imageListViewSelectItems, false, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns, new DataGridViewGenericCellStatus());
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
