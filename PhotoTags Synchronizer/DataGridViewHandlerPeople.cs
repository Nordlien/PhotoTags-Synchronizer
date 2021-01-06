using System.Drawing;
using System.Windows.Forms;
using MetadataLibrary;
using DataGridViewGeneric;
using static Manina.Windows.Forms.ImageListView;
using Manina.Windows.Forms;
using System.Collections.Generic;
using System;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerPeople 
    {
        public const string headerPeople = "People";
        public const string headerPeopleSuggestion = "Suggestion";
        public const string headerPeopleMostUsed = "Most used";

        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }

        public static int SuggestRegionNameTopMostCount { get; set; } = 10;
        public static int SuggestRegionNameNearbyDays { get; set; } = 10;

        public static void GetUserInputChanges(ref DataGridView dataGridView, Metadata metadata, FileEntry fileEntry)
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

        private static int AddRowRegion(DataGridView dataGridView, MetadataBrokerTypes metadataBrokerType, Metadata metadata, int columnIndex, DataGridViewGenericRow dataGridViewGenericRow, RegionStructure regionStructureToAdd, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
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
                object cellValue = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex);

                if (!dataGridViewGenericRowCheck.IsHeader &&
                        !dataGridViewGenericRow.IsHeader && //Is row
                        dataGridViewGenericRowCheck.HeaderName == dataGridViewGenericRow.HeaderName &&
                        dataGridViewGenericRowCheck.RowName == dataGridViewGenericRow.RowName)
                {
                    //Check if region match

                    if (regionStructureToAdd != null && cellValue != null && cellValue.GetType() == typeof(RegionStructure))
                    {
                        RegionStructure regionStructureInCell = (RegionStructure)cellValue;

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

                    if (cellValue == null)
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
                if (regionStructureInCell == null || metadata.Broker == MetadataBrokerTypes.ExifTool)
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndexUsed, regionStructureToAdd); //Prioritize ExifTool
            } 
            else if (rowBlankFound) //Found row and but no cell with correct region
            {
                rowIndexUsed = firstBlankFound;
                RegionStructure regionStructureInCell = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndexUsed);
                if (regionStructureInCell == null || metadata.Broker == MetadataBrokerTypes.ExifTool)
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndexUsed, regionStructureToAdd); //Prioritize ExifTool
            }
            else //No postion found, add on soreted location
            {
                //lastHeaderRowFound
                rowIndexUsed = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericRow,
                DataGridViewHandler.GetFavoriteList(dataGridView), regionStructureToAdd, dataGridViewGenericCellStatusDefaults, lastHeaderRowFound, true, true, true);
            }
            #endregion 

            #region Set default Cell status
            DataGridViewGenericCellStatus dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndexUsed)); //Remember current status, in case of updates
            dataGridViewGenericCellStatus.MetadataBrokerTypes |= metadataBrokerType;
            if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Undefine)
                dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerTypes & MetadataBrokerTypes.ExifTool) == MetadataBrokerTypes.ExifTool ? SwitchStates.On : SwitchStates.Off;
            DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndexUsed, dataGridViewGenericCellStatus);
            #endregion 

            DataGridViewHandler.SetCellRowHeight(dataGridView, rowIndexUsed, DataGridViewHandler.GetCellRowHeight(dataGridView));
            DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndexUsed, ""); //Clean first, avoid duplication

            #region Delete from suggestion
            DataGridViewHandler.DeleteRow(dataGridView, headerPeopleSuggestion, regionStructureToAdd.Name);
            DataGridViewHandler.DeleteRow(dataGridView, headerPeopleMostUsed, regionStructureToAdd.Name);
            #endregion 

            return rowIndexUsed;
        }

        private static void PopulatePeople(DataGridView dataGridView, Metadata metadata, int columnIndex, MetadataBrokerTypes metadataBrokerType) //, ref List<string> regionNames)
        {
            foreach (RegionStructure region in metadata.PersonalRegionList)
            {
                DataGridViewGenericRow dataGridViewGenericRow = new DataGridViewGenericRow(headerPeople, region.Name);

                AddRowRegion(dataGridView, metadataBrokerType, metadata, columnIndex, dataGridViewGenericRow,
                    new RegionStructure(region),//DataGridViewHandler.DeepCopy(region),
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Undefine, false)); //Other cell for this row will by default be Empty and disabled
            }
        }

        
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
            DateTime? dateTimeNewest = null;
            DateTime? dateTimeOldest = null;
            if (dateTimeForEditableMediaFile == DataGridViewHandler.DateTimeForEditableMediaFile && fileVersionDates.Count > 0)
            {
                dateTimeNewest = FileEntryBroker.FindNewestDate(fileVersionDates);
                dateTimeOldest = FileEntryBroker.FindNewestDate(fileVersionDates);
                fileVersionDates.Add(new FileEntryBroker(fullFilePath, DataGridViewHandler.DateTimeForEditableMediaFile, MetadataBrokerTypes.ExifTool));
            }

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

            foreach (FileEntryBroker fileEntryBroker in fileVersionDates)
            {
                DataGridViewHandler.GetColumnCount(dataGridView); //Rememebr coulmn count before AddColumnOrUpdate

                FileEntryBroker fileEntryBrokerReadVersion = fileEntryBroker;

                Metadata metadata;
                //It's the edit column, edit column is a new column copy og last known data
                if (fileEntryBroker.LastWriteDateTime == DataGridViewHandler.DateTimeForEditableMediaFile) 
                {
                    fileEntryBrokerReadVersion = new FileEntryBroker(fullFilePath, (DateTime)dateTimeNewest, MetadataBrokerTypes.ExifTool);
                    metadata = new Metadata(DatabaseAndCacheMetadataExiftool.MetadataCacheRead(fileEntryBrokerReadVersion));
                }
                else
                {
                    metadata = DatabaseAndCacheMetadataExiftool.MetadataCacheRead(fileEntryBrokerReadVersion);
                }
                //metadata = DatabaseAndCacheMetadataExiftool.ReadCache(fileEntryBrokerReadVersion);

                int columnIndex = DataGridViewHandler.AddColumnOrUpdate(dataGridView,
                    new FileEntryImage(fileEntryBroker),                                                    /* This Column idenity                                      */
                    metadata,                                                                               /* Metadata will save on column                             */
                    dateTimeForEditableMediaFile,                                                           /* idenify what column that we can edit                     */
                    DataGridViewHandler.IsCurrentFile(fileEntryBroker, dateTimeForEditableMediaFile) ?      /* Metadata.FileEntry read date, fileEntryBroker fake future version */
                    ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly,            /* this will set histority columns as read only columns    */
                    showWhatColumns,                                                                        /* show Edit | Hisorical columns | Error columns            */
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true));  /* New cells will have this value                           */

                columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntryBroker);    /* Force updated, every time, new data arrives */
                if (columnIndex == -1) continue;                                                         /* -1 - Don't need show column, due to hidden / do now show */

                AddRowHeader(dataGridView, columnIndex, new DataGridViewGenericRow(headerPeople), false);

                //Remove tooltips - Need remove, due to +=string and can get doublecated during updates 
                for (int rowIndex = 0; rowIndex < DataGridViewHandler.GetRowCountWithoutEditRow(dataGridView); rowIndex++)
                {
                    DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "");
                }

                if (metadata != null)
                {
                    Metadata metadataCopy = new Metadata(metadata);
                    Metadata metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.MetadataCacheRead(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.WindowsLivePhotoGallery));
                    Metadata metadataWindowsLivePhotoGalleryCopy = metadataWindowsLivePhotoGallery == null ? null : new Metadata(metadataWindowsLivePhotoGallery);
                    Metadata metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.MetadataCacheRead(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.MicrosoftPhotos));
                    Metadata metadataMicrosoftPhotosCopy = metadataMicrosoftPhotos == null ? null : new Metadata(metadataMicrosoftPhotos);

                    //Remove doubles and add names where missing, only work with copy, don't change metadata in buffer.
                    if (metadataWindowsLivePhotoGalleryCopy != null) metadataWindowsLivePhotoGalleryCopy.PersonalRegionRemoveNamelessDoubleRegions(metadataCopy.PersonalRegionList);
                    if (metadataMicrosoftPhotosCopy != null) metadataMicrosoftPhotosCopy.PersonalRegionRemoveNamelessDoubleRegions(metadataCopy.PersonalRegionList);
                    if (metadataWindowsLivePhotoGalleryCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataWindowsLivePhotoGalleryCopy.PersonalRegionList);
                    if (metadataMicrosoftPhotosCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataMicrosoftPhotosCopy.PersonalRegionList);

                    //Populate 
                    PopulatePeople(dataGridView, metadataCopy, columnIndex, MetadataBrokerTypes.ExifTool);
                    if (metadataWindowsLivePhotoGallery != null) PopulatePeople(dataGridView, metadataWindowsLivePhotoGalleryCopy, columnIndex, MetadataBrokerTypes.WindowsLivePhotoGallery);
                    if (metadataMicrosoftPhotos != null) PopulatePeople(dataGridView, metadataMicrosoftPhotosCopy, columnIndex, MetadataBrokerTypes.MicrosoftPhotos);

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
            if (dateTimeOldest != null && dateTimeNewest != null)
            {
                DateTime dateTimeFrom = ((DateTime)dateTimeNewest).AddDays(-SuggestRegionNameNearbyDays);
                DateTime dateTimeTo = ((DateTime)dateTimeNewest).AddDays(SuggestRegionNameNearbyDays);

                bool isHeaderPeopleSuggestionAdded = false;
                regioNameSuggestions = DatabaseAndCacheMetadataExiftool.ListAllRegionNamesCache(MetadataBrokerTypes.ExifTool, (DateTime)dateTimeFrom, (DateTime)dateTimeTo);
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
            List<string> regioNamesTopMost = DatabaseAndCacheMetadataExiftool.ListAllPersonalRegionNameNotInListCache(regionNamesAddedPeople, regioNameSuggestions, SuggestRegionNameTopMostCount - regionNamesAddedTopMost.Count);
            if (regioNamesTopMost != null && regioNamesTopMost.Count > 0)
            {
                AddRowHeader(dataGridView, columnIndexDummy, new DataGridViewGenericRow(headerPeopleMostUsed), false);
                foreach (string regionName in regioNamesTopMost)
                {

                    AddRowHeader(dataGridView, columnIndexDummy, new DataGridViewGenericRow(headerPeopleMostUsed, regionName), true);
                }
            }
            #endregion 
            
            DataGridViewHandler.Refresh(dataGridView);

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
                new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, true)); //ReadOnly untill data is read
            //Add all default rows
            //AddRowsDefault(dataGridView);
            //Tell data default columns and rows are agregated
            DataGridViewHandler.SetIsAgregated(dataGridView, true);
            //-----------------------------------------------------------------


            //Populate one and one of selected files, (new versions of files can be added)
            foreach (ImageListViewItem imageListViewItem in imageListViewSelectItems)
            {
                PopulateFile(dataGridView, imageListViewItem.FileFullPath, showWhatColumns, 
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
