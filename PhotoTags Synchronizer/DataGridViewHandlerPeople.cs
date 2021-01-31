using System.Drawing;
using System.Windows.Forms;
using MetadataLibrary;
using DataGridViewGeneric;
using static Manina.Windows.Forms.ImageListView;
using Manina.Windows.Forms;
using System.Collections.Generic;
using System;
using Thumbnails;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerPeople 
    {
        public const string headerPeople = "People";
        public const string headerPeopleSuggestion = "Suggestion";
        public const string headerPeopleMostUsed = "Most used";

        public static ThumbnailDatabaseCache DatabaseAndCacheThumbnail { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }

        public static int SuggestRegionNameTopMostCount { get; set; } = 10;
        public static int SuggestRegionNameNearbyDays { get; set; } = 10;

        public static void GetUserInputChanges(ref DataGridView dataGridView, Metadata metadata, FileEntryAttribute fileEntry)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntry);
            //DataGridViewHandler.ClearFileBeenUpdated(dataGridView, columnIndex);

            metadata.PersonalRegionList.Clear();
            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCount(dataGridView); rowIndex++)
            {
                SwitchStates switchStates = DataGridViewHandler.GetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex);
                if (switchStates == SwitchStates.On)
                {
                    RegionStructure regionStructure = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndex);
                    metadata.PersonalRegionListAddIfNotAreaAndNameExists(regionStructure);
                }               
            }
        }


        private static int AddRowHeader(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, bool sort)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, sort);
        }

        public static void SetCellDefault(DataGridView dataGridView, MetadataBrokerType metadataBrokerType, int columnIndex, int rowIndexUsed)
        {
            DataGridViewHandler.SetCellDefaultAfterUpdated(dataGridView, metadataBrokerType, columnIndex, rowIndexUsed);
        }

        private static int AddRowSuggestion(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, bool sort)
        {
            DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault = new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true);
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, null, dataGridViewGenericCellStatusDefault, sort);
        }

        private static int AddRowRegion(DataGridView dataGridView, MetadataBrokerType metadataBrokerType, Metadata metadata, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow, RegionStructure regionStructureToAdd, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        {
            #region Find Row to Edit or Where to add
            bool rowFound = false;
            int lastHeaderRowFound = -1;

            bool rowBlankFound = false;
            int firstBlankFound = -1;
            int rowIndexRowFound = -1;

            int startSearchRow = 0;
            for (int rowIndex = startSearchRow; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericRowCheck = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
               

                if (!dataGridViewGenericRowCheck.IsHeader &&
                        !dataGridViewGenericRow.IsHeader && //Is row
                        dataGridViewGenericRowCheck.HeaderName == dataGridViewGenericRow.HeaderName &&
                        dataGridViewGenericRowCheck.RowName == dataGridViewGenericRow.RowName)
                {
                    //Check if region match
                    RegionStructure regionStructureInCell = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex) as RegionStructure;

                    if (regionStructureToAdd != null && regionStructureInCell != null)
                    {
                        if (metadata != null && metadata.MediaHeight != null && metadata.MediaWidth != null)
                        {
                            Size imageSize = new Size((int)metadata.MediaWidth, (int)metadata.MediaHeight);
                            Rectangle mediaRectangleToAdd = regionStructureToAdd.GetImageRegionPixelRectangle(imageSize);
                            Rectangle mediaRectangleCell = regionStructureInCell.GetImageRegionPixelRectangle(imageSize);

                            if (RegionStructure.RectangleEqual(mediaRectangleToAdd, mediaRectangleCell))
                            {
                                rowFound = true;
                                rowIndexRowFound = rowIndex;
                                break; // return rowIndex;
                            }
                        }
                    }

                    if (regionStructureInCell == null)
                    {
                        rowBlankFound = true;
                        if (firstBlankFound == -1) firstBlankFound = rowIndex;
                    }
                }

                #region Sorting
                if (dataGridViewGenericRow.IsHeader && //A normal row is add (not header)
                                                       //dataGridViewGenericRowCheck.IsHeader &&  //If header, then check if same header name
                    dataGridViewGenericRow.HeaderName.CompareTo(dataGridViewGenericRowCheck.HeaderName) >= 0)
                    lastHeaderRowFound = rowIndex; //Remember head row found

                //Add sorted
                if (!dataGridViewGenericRow.IsHeader && //A normal row is add (not header)
                    dataGridViewGenericRowCheck.IsHeader &&  //If header, then check if same header name
                    dataGridViewGenericRowCheck.HeaderName == dataGridViewGenericRow.HeaderName)
                    lastHeaderRowFound = rowIndex; //Remember head row found

                if (!dataGridViewGenericRow.IsHeader && //A normal row is add (not header)
                    !dataGridViewGenericRowCheck.IsHeader &&  //If header, then check if same header name
                    dataGridViewGenericRowCheck.HeaderName == dataGridViewGenericRow.HeaderName &&
                    dataGridViewGenericRow.RowName.CompareTo(dataGridViewGenericRowCheck.RowName) >= 0)
                    lastHeaderRowFound = rowIndex; //If lower or eaual, remeber last
                #endregion 
            }
            #endregion

            #region Update row or Add
            int rowIndexUsed;
            if (rowFound) //Found row and cell with correct region
            {
                rowIndexUsed = rowIndexRowFound;
                RegionStructure regionStructureInCell = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndexUsed);
                if (regionStructureInCell == null || metadata.Broker == MetadataBrokerType.ExifTool)
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndexUsed, regionStructureToAdd); //Prioritize ExifTool
            } 
            else if (rowBlankFound) //Found row and but no cell with correct region
            {
                rowIndexUsed = firstBlankFound;
                RegionStructure regionStructureInCell = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndexUsed);
                if (regionStructureInCell == null || metadata.Broker == MetadataBrokerType.ExifTool)
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndexUsed, regionStructureToAdd); //Prioritize ExifTool
            }
            else //No postion found, add on soreted location
            {
                //lastHeaderRowFound
                rowIndexUsed = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericRow,
                DataGridViewHandler.GetFavoriteList(dataGridView), regionStructureToAdd, dataGridViewGenericCellStatusDefaults, lastHeaderRowFound, true, true, true);
            }
            #endregion

            SetCellDefault(dataGridView, metadataBrokerType, columnIndex, rowIndexUsed);

            DataGridViewHandler.SetCellRowHeight(dataGridView, rowIndexUsed, DataGridViewHandler.GetCellRowHeight(dataGridView));

            #region Delete from suggestion
            DataGridViewHandler.DeleteRow(dataGridView, headerPeopleSuggestion, regionStructureToAdd.Name);
            DataGridViewHandler.DeleteRow(dataGridView, headerPeopleMostUsed, regionStructureToAdd.Name);
            #endregion 

            return rowIndexUsed;
        }

        private static void PopulatePeople(DataGridView dataGridView, Metadata metadata, int columnIndex, MetadataBrokerType metadataBrokerType) //, ref List<string> regionNames)
        {
            foreach (RegionStructure region in metadata.PersonalRegionList)
            {
                DataGridViewGenericRow dataGridViewGenericRow = new DataGridViewGenericRow(headerPeople, region.Name, ReadWriteAccess.AllowCellReadAndWrite);

                AddRowRegion(dataGridView, metadataBrokerType, metadata, columnIndex, dataGridViewGenericRow,
                    new RegionStructure(region),//DataGridViewHandler.DeepCopy(region),
                    new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Undefine, true)); //Other cell for this row will by default be Empty and disabled
            }
        }

        
        public static void PopulateFile(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, ShowWhatColumns showWhatColumns)

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

            List<string> regionNamesAddedPeople = new List<string>();
            List<string> regionNamesAddedTopMost = new List<string>();
            List<string> regionNamesAddedDateInterval = new List<string>();

            
            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                {
                    if (dataGridViewGenericRow.HeaderName == headerPeople && !regionNamesAddedPeople.Contains(dataGridViewGenericRow.RowName)) regionNamesAddedPeople.Add(dataGridViewGenericRow.RowName);
                    else if (dataGridViewGenericRow.HeaderName == headerPeopleSuggestion && !regionNamesAddedDateInterval.Contains(dataGridViewGenericRow.RowName)) regionNamesAddedDateInterval.Add(dataGridViewGenericRow.RowName);
                    else if (dataGridViewGenericRow.HeaderName == headerPeopleMostUsed && !regionNamesAddedTopMost.Contains(dataGridViewGenericRow.RowName)) regionNamesAddedTopMost.Add(dataGridViewGenericRow.RowName);
                }
            }

            

            //-----------------------------------------------------------------
            Image thumbnail = new Bitmap(DatabaseAndCacheThumbnail.ReadThumbnailFromCacheOnlyClone(fileEntryAttribute));
            FileEntryBroker fileEntryBrokerReadVersion = fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool);
            Metadata metadata = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBrokerReadVersion);
            if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.Current && metadata != null) metadata = new Metadata(metadata); //It's the edit column, make a copy do edit in dataGridView updated the origianal metadata
            ReadWriteAccess readWriteAccessColumn = fileEntryAttribute.FileEntryVersion == FileEntryVersion.Current && metadata != null ? ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly;
            int columnIndex = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView, fileEntryAttribute, thumbnail, metadata, readWriteAccessColumn, showWhatColumns, DataGridViewGenericCellStatus.DefaultEmpty());
            //-----------------------------------------------------------------


            if (columnIndex != -1)
            {

                AddRowHeader(dataGridView, columnIndex, new DataGridViewGenericRow(headerPeople), false);

                //Remove column data, due to Populate People append data - 
                for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                {
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, null);
                    //DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatusDefault);
                    //DataGridViewHandler.SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatusDefault);
                    DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault = new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true);
                    DataGridViewHandler.SetCellDefaultAfterUpdated(dataGridView, dataGridViewGenericCellStatusDefault, columnIndex, rowIndex);
                }

                if (metadata != null)
                {
                    Metadata metadataCopy = new Metadata(metadata);
                    Metadata metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.WindowsLivePhotoGallery));
                    Metadata metadataWindowsLivePhotoGalleryCopy = metadataWindowsLivePhotoGallery == null ? null : new Metadata(metadataWindowsLivePhotoGallery);
                    Metadata metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.MicrosoftPhotos));
                    Metadata metadataMicrosoftPhotosCopy = metadataMicrosoftPhotos == null ? null : new Metadata(metadataMicrosoftPhotos);

                    //Remove doubles and add names where missing, only work with copy, don't change metadata in buffer.
                    if (metadataWindowsLivePhotoGalleryCopy != null) metadataWindowsLivePhotoGalleryCopy.PersonalRegionRemoveNamelessDoubleRegions(metadataCopy.PersonalRegionList);
                    if (metadataMicrosoftPhotosCopy != null) metadataMicrosoftPhotosCopy.PersonalRegionRemoveNamelessDoubleRegions(metadataCopy.PersonalRegionList);
                    if (metadataWindowsLivePhotoGalleryCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataWindowsLivePhotoGalleryCopy.PersonalRegionList);
                    if (metadataMicrosoftPhotosCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataMicrosoftPhotosCopy.PersonalRegionList);

                    //Populate 
                    PopulatePeople(dataGridView, metadataCopy, columnIndex, MetadataBrokerType.ExifTool);
                    if (metadataWindowsLivePhotoGallery != null) PopulatePeople(dataGridView, metadataWindowsLivePhotoGalleryCopy, columnIndex, MetadataBrokerType.WindowsLivePhotoGallery);
                    if (metadataMicrosoftPhotos != null) PopulatePeople(dataGridView, metadataMicrosoftPhotosCopy, columnIndex, MetadataBrokerType.MicrosoftPhotos);

                    //Remember names added
                    foreach (RegionStructure regionStructure in metadata.PersonalRegionList)
                    {
                        if (!regionNamesAddedPeople.Contains(regionStructure.Name)) regionNamesAddedPeople.Add(regionStructure.Name);
                    }
                }
            }

            #region Suggestion of Names - Near date
            int columnIndexDummy = -1;
            List<string> regioNameSuggestions = null;
            DateTime? dateTimeMediaTaken = metadata?.MediaDateTaken;
            if (dateTimeMediaTaken != null)
            {
                DateTime dateTimeFrom = ((DateTime)dateTimeMediaTaken).AddDays(-SuggestRegionNameNearbyDays);
                DateTime dateTimeTo = ((DateTime)dateTimeMediaTaken).AddDays(SuggestRegionNameNearbyDays);

                bool isHeaderPeopleSuggestionAdded = false;
                regioNameSuggestions = DatabaseAndCacheMetadataExiftool.ListAllRegionNamesCache(MetadataBrokerType.ExifTool, (DateTime)dateTimeFrom, (DateTime)dateTimeTo);
                if (regioNameSuggestions != null && regioNameSuggestions.Count > 0)
                {
                    foreach (string regionName in regioNameSuggestions)
                    {
                        if (!string.IsNullOrWhiteSpace(regionName))
                        {
                            if (regionNamesAddedTopMost.Contains(regionName))
                            {
                                DataGridViewHandler.DeleteRow(dataGridView, headerPeopleMostUsed, regionName);
                                regionNamesAddedTopMost.Remove(regionName);
                            }

                            if (!regionNamesAddedPeople.Contains(regionName))
                            {
                                if (!isHeaderPeopleSuggestionAdded)
                                {
                                    AddRowHeader(dataGridView, columnIndexDummy, new DataGridViewGenericRow(headerPeopleSuggestion), false);
                                    isHeaderPeopleSuggestionAdded = true;
                                }
                                AddRowHeader(dataGridView, columnIndexDummy, new DataGridViewGenericRow(headerPeopleSuggestion, regionName), true);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Sugegstion of names - Top Most
            List<string> regioNamesTopMost = DatabaseAndCacheMetadataExiftool.ListAllPersonalRegionNameNotInListCache(MetadataBrokerType.ExifTool, regionNamesAddedPeople, regioNameSuggestions, SuggestRegionNameTopMostCount - regionNamesAddedTopMost.Count);
            if (regioNamesTopMost != null && regioNamesTopMost.Count > 0)
            {
                AddRowHeader(dataGridView, columnIndexDummy, new DataGridViewGenericRow(headerPeopleMostUsed), false);
                foreach (string regionName in regioNamesTopMost)
                {
                    AddRowHeader(dataGridView, columnIndexDummy, new DataGridViewGenericRow(headerPeopleMostUsed, regionName), true);
                }
            }
            #endregion 
            
            //DataGridViewHandler.Refresh(dataGridView);

            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }

        public static void PopulateSelectedFiles(DataGridView dataGridView, ImageListViewSelectedItemCollection imageListViewSelectItems, DataGridViewSize dataGridViewSize, ShowWhatColumns showWhatColumns)
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
            DataGridViewHandlerCommon.AddColumnSelectedFiles(dataGridView, DatabaseAndCacheMetadataExiftool, DatabaseAndCacheThumbnail, imageListViewSelectItems, false, ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true)); //ReadOnly untill data is read
            
            //Add all default rows
            AddRowHeader(dataGridView, -1, new DataGridViewGenericRow(headerPeople), false);

            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------
         
            //Tell that work is done
            DataGridViewHandler.SetIsPopulating(dataGridView, false);
            //-----------------------------------------------------------------
        }
    }
}
