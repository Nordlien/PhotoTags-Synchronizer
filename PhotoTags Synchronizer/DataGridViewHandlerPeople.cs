using System.Drawing;
using System.Windows.Forms;
using MetadataLibrary;
using DataGridViewGeneric;
using System.Collections.Generic;
using System;
using Thumbnails;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerPeople 
    {
        public const string headerPeople = "People";
        public const string headerPeopleSuggestion = "Suggestion near date";
        public const string headerPeopleMostUsed = "Most used";
        public const string headerPeopleAdded = "Added";
        public static bool HasBeenInitialized { get; set; } = false;
        public static ThumbnailPosterDatabaseCache DatabaseAndCacheThumbnail { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }

        public static int SuggestRegionNameNearByTopMostCount { get; set; } = 10;
        public static int SuggestRegionNameNearByDays { get; set; } = 10;
        public static string RenameDateFormats { get; set; } = "";

        #region GetUserInputChanges
        public static void GetUserInputChanges(DataGridView dataGridView, ref Metadata metadata, FileEntryAttribute fileEntry)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndexUserInput(dataGridView, fileEntry);
            if (columnIndex == -1) return; //Column has not yet become aggregated or has already been removed
            if (!DataGridViewHandler.IsColumnPopulated(dataGridView, columnIndex)) return;

            metadata.PersonalRegionList.Clear();
            for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCount(dataGridView); rowIndex++)
            {
                SwitchStates switchStates = DataGridViewHandler.GetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex);
                if (switchStates == SwitchStates.On)
                {
                    RegionStructure regionStructure = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndex);
                    if (regionStructure != null) metadata.PersonalRegionListAddIfNotAreaAndNameExists(regionStructure);
                }               
            }
        }
        #endregion

        #region AddRowHeader
        private static int AddRowHeader(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, bool sort)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, sort);
        }
        #endregion

        #region SetCellDefault
        public static void SetCellDefault(DataGridView dataGridView, MetadataBrokerType metadataBrokerType, int columnIndex, int rowIndexUsed)
        {
            DataGridViewHandler.SetCellDefaultAfterUpdated(dataGridView, metadataBrokerType, columnIndex, rowIndexUsed);
        }
        #endregion

        #region AddRowPeople
        public static void AddRowPeople(DataGridView dataGridView, string regionName)
        {
            AddRowHeader(dataGridView, -1, new DataGridViewGenericRow(headerPeopleAdded, regionName), true);
        }
        #endregion

        #region AddRowRegion
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
                if (regionStructureInCell == null || regionStructureInCell?.Thumbnail == null || (metadata.Broker == MetadataBrokerType.ExifTool && regionStructureToAdd.Thumbnail != null))
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndexUsed, regionStructureToAdd, false); //Prioritize ExifTool
            } 
            else if (rowBlankFound) //Found row and but no cell with correct region
            {
                rowIndexUsed = firstBlankFound;
                RegionStructure regionStructureInCell = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndexUsed);
                if (regionStructureInCell == null || regionStructureInCell?.Thumbnail == null || (metadata.Broker == MetadataBrokerType.ExifTool && regionStructureToAdd.Thumbnail != null))
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndexUsed, regionStructureToAdd, false); //Prioritize ExifTool
            }
            else //No postion found, add on sorted location
            {
                //lastHeaderRowFound
                rowIndexUsed = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericRow,
                DataGridViewHandler.GetFavoriteList(dataGridView), regionStructureToAdd, dataGridViewGenericCellStatusDefaults, lastHeaderRowFound, true, true, true);
            }
            #endregion

            SetCellDefault(dataGridView, metadataBrokerType, columnIndex, rowIndexUsed); //No DirtyFlagSet

            DataGridViewHandler.SetCellRowHeight(dataGridView, rowIndexUsed, DataGridViewHandler.GetCellRowHeight(dataGridView));

            #region Delete from suggestion
            DataGridViewHandler.DeleteRow(dataGridView, headerPeopleSuggestion, regionStructureToAdd.Name);
            DataGridViewHandler.DeleteRow(dataGridView, headerPeopleMostUsed, regionStructureToAdd.Name);
            #endregion 

            return rowIndexUsed;
        }
        #endregion

        #region PopulatePeople
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
        #endregion

        #region PopulateFile
        public static int PopulateFile(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, ShowWhatColumns showWhatColumns, Metadata metadataAutoCorrected, bool onlyRefresh)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return -1;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return -1;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) return -1;  //In progress doing so

            //Check if file is in DataGridView, and needs updated
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fileEntryAttribute.FileFullPath)) return -1;

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
            
            FileEntryBroker fileEntryBrokerReadVersion = fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool);
            Image thumbnail = DatabaseAndCacheThumbnail.ReadThumbnailFromCacheOnly(fileEntryBrokerReadVersion);
            if (thumbnail == null && metadataAutoCorrected != null) thumbnail = DatabaseAndCacheThumbnail.ReadThumbnailFromCacheOnly(metadataAutoCorrected.FileEntry);

            Metadata metadataExiftool = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBrokerReadVersion);
            if (metadataExiftool != null) metadataExiftool = new Metadata(metadataExiftool);
            if (metadataAutoCorrected != null) metadataExiftool = metadataAutoCorrected; //If AutoCorrect is run, use AutoCorrect values. Needs to be after DataGridViewHandler.AddColumnOrUpdateNew, so orignal metadata stored will not be overwritten
            
            ReadWriteAccess readWriteAccessColumn =
                (FileEntryVersionHandler.IsReadOnlyColumnType(fileEntryAttribute.FileEntryVersion) ||
                metadataExiftool == null) ? ReadWriteAccess.ForceCellToReadOnly : ReadWriteAccess.AllowCellReadAndWrite;

            int columnIndex = DataGridViewHandler.AddColumnOrUpdateNew(
                dataGridView, fileEntryAttribute, thumbnail, metadataExiftool, readWriteAccessColumn, showWhatColumns,
                DataGridViewGenericCellStatus.DefaultEmpty(), out FileEntryVersionCompare fileEntryVersionCompareReason);

            
            //Chech if populated and new refresh data
            if (onlyRefresh && FileEntryVersionHandler.DoesCellsNeedUpdate(fileEntryVersionCompareReason) && !DataGridViewHandler.IsColumnPopulated(dataGridView, columnIndex))
                fileEntryVersionCompareReason = FileEntryVersionCompare.LostNoneEqualFound_ContinueSearch_Update_Nothing; //No need to populate
            //-----------------------------------------------------------------

            if (FileEntryVersionHandler.DoesCellsNeedUpdate(fileEntryVersionCompareReason))
            {
                DataGridViewHandler.SetDataGridViewAllowUserToAddRows(dataGridView, true);

                AddRowHeader(dataGridView, columnIndex, new DataGridViewGenericRow(headerPeople), false);

                //Remove column data, due to Populate People append data - 
                for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                {
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, null, false);
                    DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefault = new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, true);
                    DataGridViewHandler.SetCellDefaultAfterUpdated(dataGridView, dataGridViewGenericCellStatusDefault, columnIndex, rowIndex);
                }

                if (metadataExiftool != null)
                {
                    Metadata metadataCopy = new Metadata(metadataExiftool);
                    Metadata metadataWindowsLivePhotoGallery = (!GlobalData.doesWindowsLivePhotoGalleryExists ? null : 
                        DatabaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.WindowsLivePhotoGallery)));
                    Metadata metadataWindowsLivePhotoGalleryCopy = metadataWindowsLivePhotoGallery == null ? null : new Metadata(metadataWindowsLivePhotoGallery);

                    Metadata metadataMicrosoftPhotos = (!GlobalData.doesMircosoftPhotosExists ? null :  
                        DatabaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.MicrosoftPhotos)));
                    Metadata metadataMicrosoftPhotosCopy = metadataMicrosoftPhotos == null ? null : new Metadata(metadataMicrosoftPhotos);
                    
                    Metadata metadataWebScraping = DatabaseAndCacheMetadataExiftool.ReadWebScraperMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.WebScraping));
                    Metadata metadataWebScrapingCopy = metadataWebScraping == null ? null : new Metadata(metadataWebScraping);

                    //Remove doubles and add names where missing, only work with copy, don't change metadata in buffer.
                    //Don't remove region in ExifTool metadata when found other places 
                    
                    if (metadataWindowsLivePhotoGalleryCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataWindowsLivePhotoGalleryCopy.PersonalRegionList);
                    if (metadataMicrosoftPhotosCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataMicrosoftPhotosCopy.PersonalRegionList);
                    
                    if (metadataWebScrapingCopy != null)
                    {
                        metadataWebScrapingCopy.MediaHeight = metadataCopy.MediaHeight;
                        metadataWebScrapingCopy.MediaWidth = metadataCopy.MediaWidth;
                        metadataWebScrapingCopy.MediaOrientation = metadataCopy.MediaOrientation;
                        metadataWebScrapingCopy.MediaVideoLength = metadataCopy.MediaVideoLength;
                        
                        if (metadataCopy != null) metadataCopy.PersonalRegionSetRegionlessRegions(metadataWebScrapingCopy.PersonalRegionList);
                        if (metadataWindowsLivePhotoGalleryCopy != null) metadataWindowsLivePhotoGalleryCopy.PersonalRegionSetRegionlessRegions(metadataWebScrapingCopy.PersonalRegionList);
                        if (metadataMicrosoftPhotosCopy != null) metadataMicrosoftPhotosCopy.PersonalRegionSetRegionlessRegions(metadataWebScrapingCopy.PersonalRegionList);
                    }


                    //Populate 
                    PopulatePeople(dataGridView, metadataCopy, columnIndex, metadataCopy.Broker);
                    if (metadataWindowsLivePhotoGalleryCopy != null) PopulatePeople(dataGridView, metadataWindowsLivePhotoGalleryCopy, columnIndex, metadataWindowsLivePhotoGalleryCopy.Broker);
                    if (metadataMicrosoftPhotosCopy != null) PopulatePeople(dataGridView, metadataMicrosoftPhotosCopy, columnIndex, metadataMicrosoftPhotosCopy.Broker);
                    if (metadataWebScrapingCopy != null) PopulatePeople(dataGridView, metadataWebScrapingCopy, columnIndex, metadataWebScrapingCopy.Broker);

                    //Remember names added
                    foreach (RegionStructure regionStructure in metadataExiftool.PersonalRegionList)
                    {
                        if (!regionNamesAddedPeople.Contains(regionStructure.Name)) regionNamesAddedPeople.Add(regionStructure.Name);
                    }
                }

                DataGridViewHandler.SetColumnPopulatedFlag(dataGridView, columnIndex, true);
            }

            if (FileEntryVersionHandler.IsCurrenOrUpdatedVersion(fileEntryAttribute.FileEntryVersion) && metadataExiftool != null)
            {
                #region Suggestion of Names - Near date
                int columnIndexDummy = -1;
                List<string> regioNameSuggestions = null;
                DateTime? dateTimeSmartDate = metadataExiftool.FileSmartDate(RenameDateFormats);

                if (dateTimeSmartDate != null)
                {
                    DateTime date = new DateTime(((DateTime)dateTimeSmartDate).Year, ((DateTime)dateTimeSmartDate).Month, ((DateTime)dateTimeSmartDate).Day);
                    DateTime dateTimeFrom = date.AddDays(-SuggestRegionNameNearByDays);
                    DateTime dateTimeTo = date.AddDays(SuggestRegionNameNearByDays);

                    bool isHeaderPeopleSuggestionAdded = false;
                    regioNameSuggestions = DatabaseAndCacheMetadataExiftool.ListAllRegionNamesNearByCache(MetadataBrokerType.ExifTool, 
                        (DateTime)dateTimeFrom, (DateTime)dateTimeTo, SuggestRegionNameNearByTopMostCount);

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

                #region Suggestion of names - Top Most
                List<string> regioNamesTopMost = DatabaseAndCacheMetadataExiftool.ListAllPersonalRegionName(SuggestRegionNameNearByTopMostCount - regionNamesAddedTopMost.Count, regionNamesAddedPeople, regioNameSuggestions);
                if (regioNamesTopMost != null && regioNamesTopMost.Count > 0)
                {
                    AddRowHeader(dataGridView, columnIndexDummy, new DataGridViewGenericRow(headerPeopleMostUsed), false);
                    foreach (string regionName in regioNamesTopMost)
                    {
                        AddRowHeader(dataGridView, columnIndexDummy, new DataGridViewGenericRow(headerPeopleMostUsed, regionName), true);
                    }
                }
                #endregion

                AddRowHeader(dataGridView, columnIndexDummy, new DataGridViewGenericRow(headerPeopleAdded), false);
            }

            
            //DataGridViewHandler.Refresh(dataGridView);

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
            if (DataGridViewHandler.GetIsAgregated(dataGridView)) return;
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
            //AddRowHeader(dataGridView, -1, new DataGridViewGenericRow(headerPeople), false);

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
