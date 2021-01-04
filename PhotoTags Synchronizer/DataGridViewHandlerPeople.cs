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

        private static int AddRowRegion(DataGridView dataGridView, Metadata metadata, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        {
            int startSearchRow = 0;
            bool regionFound;
            int rowIndex;
            do
            {
                //Find first row that fits with Region Name, then in region area not fit, is in use, find next
                rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow,
                    DataGridViewHandler.GetFavoriteList(dataGridView), (object)null, dataGridViewGenericCellStatusDefaults, startSearchRow, false, false);
                object cellValue = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, rowIndex);
                
                if (value != null && cellValue != null &&
                    value.GetType() == typeof(RegionStructure) &&
                    cellValue.GetType() == typeof(RegionStructure))
                {
                    RegionStructure regionStructureInput = (RegionStructure)value;
                    RegionStructure regionStructureCell = (RegionStructure)cellValue;

                    if (metadata != null && metadata.MediaHeight != null && metadata.MediaWidth != null)
                    {
                        Size imageSize = new Size((int)metadata.MediaWidth, (int)metadata.MediaHeight);
                        Rectangle mediaRectangleInput = regionStructureInput.GetImageRegionPixelRectangle(imageSize);
                        Rectangle mediaRectangleCell = regionStructureCell.GetImageRegionPixelRectangle(imageSize);

                        if (RegionStructure.RectangleEqual (mediaRectangleInput, mediaRectangleCell)) regionFound = true;
                        else regionFound = false;
                    }
                    else regionFound = true;
                }
                else regionFound = true;
                startSearchRow = rowIndex + 1;

            } while (!regionFound);

            RegionStructure regionStructure = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndex);
            if (regionStructure == null || metadata.Broker == MetadataBrokerTypes.ExifTool) 
                DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, value); //Prioritize ExifTool
            return rowIndex;
        }

        private static void PopulatePeople(DataGridView dataGridView, Metadata metadata, int columnIndex, MetadataBrokerTypes metadataBrokerType, ref List<string> regionNames)
        {            
            foreach (RegionStructure region in metadata.PersonalRegionList)
            {
                DataGridViewGenericRow dataGridViewGenericRow = new DataGridViewGenericRow(headerPeople, region.Name);

                int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, dataGridViewGenericRow);
                
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus; 
                if (rowIndex == -1) dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Undefine, false); //By default, empty and disabled
                else dataGridViewGenericCellStatus = DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex); //Remember current status, in case of updates

                rowIndex = AddRowRegion(dataGridView, metadata, columnIndex, dataGridViewGenericRow, DataGridViewHandler.DeepCopy(region),
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Undefine, false)); //Other cell for this row will by default be Empty and disabled

                DataGridViewHandler.DeleteRow(dataGridView, headerPeopleSuggestion, region.Name);
                DataGridViewHandler.DeleteRow(dataGridView, headerPeopleMostUsed, region.Name);
                
                if (!regionNames.Contains(region.Name)) regionNames.Add(region.Name);
                
                dataGridViewGenericCellStatus.MetadataBrokerTypes |= metadataBrokerType;
                if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Undefine)
                    dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerTypes & MetadataBrokerTypes.ExifTool) == MetadataBrokerTypes.ExifTool ? SwitchStates.On : SwitchStates.Off;
                DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);

                dataGridView.Rows[rowIndex].Height = DataGridViewHandler.GetCellRowHeight(dataGridView);

                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, ""); //Clean first, avoid duplication
            }
        }

        private static void PopulatePeopleTooltip(DataGridView dataGridView, Metadata metadata, int columnIndex)
        {

            foreach (RegionStructure region in metadata.PersonalRegionList)
            {
                int rowIndex = AddRowRegion(dataGridView, metadata, columnIndex, new DataGridViewGenericRow(headerPeople, region.Name), DataGridViewHandler.DeepCopy(region),
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, false));
                
                string toolTipText = DataGridViewHandler.GetCellToolTipText(dataGridView, columnIndex, rowIndex);
                toolTipText = "" + toolTipText + region.ToolTipText(metadata.MediaSize) + "\r\n";
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, toolTipText);
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

                
                if (metadata != null)
                {
                    
                    //Exif tool
                    PopulatePeople(dataGridView, metadata, columnIndex, MetadataBrokerTypes.ExifTool, ref regionNamesAddedPeople);

                    //Windows Live Gallery
                    Metadata metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.MetadataCacheRead(
                        new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.WindowsLivePhotoGallery));                
                    if (metadataWindowsLivePhotoGallery != null) PopulatePeople(dataGridView, metadataWindowsLivePhotoGallery, columnIndex, MetadataBrokerTypes.WindowsLivePhotoGallery, ref regionNamesAddedPeople);

                    //Microsoft Photos
                    Metadata metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.MetadataCacheRead(
                        new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.MicrosoftPhotos));
                    if (metadataMicrosoftPhotos != null) PopulatePeople(dataGridView, metadataMicrosoftPhotos, columnIndex, MetadataBrokerTypes.MicrosoftPhotos, ref regionNamesAddedPeople);

                    PopulatePeopleTooltip(dataGridView, metadata, columnIndex);
                    if (metadataWindowsLivePhotoGallery != null) PopulatePeopleTooltip(dataGridView, metadataWindowsLivePhotoGallery, columnIndex);
                    if (metadataMicrosoftPhotos != null) PopulatePeopleTooltip(dataGridView, metadataMicrosoftPhotos, columnIndex);
                }
            }

            #region Suggestion of Names - Near date
            int columnIndexDummy = -1;
            List<string> regioNameSuggestions = null;
            if (dateTimeOldest != null && dateTimeNewest != null)
            {
                DateTime dateTimeFrom = ((DateTime)dateTimeOldest).AddDays(-SuggestRegionNameNearbyDays);
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
