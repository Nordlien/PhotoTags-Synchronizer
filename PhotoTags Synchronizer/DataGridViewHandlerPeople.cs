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

        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }

        public static void GetUserInputChanges(ref DataGridView dataGridView, Metadata metadata, FileEntry fileEntry)
        {
            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntry);
            DataGridViewHandler.ClearFileBeenUpdated(dataGridView, columnIndex);

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


        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow);
        }

        private static int AddRow(DataGridView dataGridView, Metadata metadata, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        {
            int startSearchRow = 0;
            bool notFound;
            int rowIndex;
            do
            {
                rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow,
                    DataGridViewHandler.GetFavoriteList(dataGridView), (object)null, dataGridViewGenericCellStatusDefaults, startSearchRow, false);
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

                        if (RegionStructure.RectangleEqual (mediaRectangleInput, mediaRectangleCell)) notFound = false;
                        else notFound = true;
                    }
                    else notFound = false;
                }
                else notFound = false;
                startSearchRow = rowIndex + 1;

            } while (notFound);

            RegionStructure regionStructure = DataGridViewHandler.GetCellRegionStructure(dataGridView, columnIndex, rowIndex);
            if (regionStructure == null || metadata.Broker == MetadataBrokerTypes.ExifTool) 
                DataGridViewHandler.SetCellValue(dataGridView, columnIndex, rowIndex, value); //Prioritize ExifTool
            return rowIndex;
        }

        private static void PopulatePeople(DataGridView dataGridView, Metadata metadata, int columnIndex, MetadataBrokerTypes metadataBrokerType)
        {            

            foreach (RegionStructure region in metadata.PersonalRegionList)
            {
                DataGridViewGenericRow dataGridViewGenericRow = new DataGridViewGenericRow(headerPeople, region.Name ?? "");

                int rowIndex = DataGridViewHandler.GetRowIndex(dataGridView, dataGridViewGenericRow);
                
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus; 
                if (rowIndex == -1) dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, false); //By default, empty and disabled
                else dataGridViewGenericCellStatus = DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex); //Remember current status, in case of updates

                rowIndex = AddRow(dataGridView, metadata, columnIndex, dataGridViewGenericRow, DataGridViewHandler.DeepCopy(region),
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, false)); //Other cell for thisrow will by default be Empty and disabled

                dataGridViewGenericCellStatus.MetadataBrokerTypes |= metadataBrokerType;
                if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Disabled || dataGridViewGenericCellStatus.SwitchState == SwitchStates.Undefine)
                    dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerTypes & MetadataBrokerTypes.ExifTool) == MetadataBrokerTypes.ExifTool ? SwitchStates.On : SwitchStates.Off;
                DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);

                dataGridView.Rows[rowIndex].Height = DataGridViewHandler.GetCellRowHeight(dataGridView);

                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, ""); //Clean first, avoid duplication
            }
        }

        private static void PopulatePeopleTooltip(DataGridView dataGridView, Metadata metadata, int columnIndex, MetadataBrokerTypes metadataBrokerType)
        {

            foreach (RegionStructure region in metadata.PersonalRegionList)
            {

                int rowIndex = AddRow(dataGridView, metadata, columnIndex, new DataGridViewGenericRow(headerPeople, region.Name ?? ""), DataGridViewHandler.DeepCopy(region),
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
            if (dateTimeForEditableMediaFile == DataGridViewHandler.DateTimeForEditableMediaFile && fileVersionDates.Count > 0)
            {
                dateTimeNewest = FileEntryBroker.FindNewestDate(fileVersionDates); 
                fileVersionDates.Add(new FileEntryBroker(fullFilePath, DataGridViewHandler.DateTimeForEditableMediaFile, MetadataBrokerTypes.ExifTool));
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
                    metadata = new Metadata(DatabaseAndCacheMetadataExiftool.ReadCache(fileEntryBrokerReadVersion));
                }
                else
                {
                    metadata = DatabaseAndCacheMetadataExiftool.ReadCache(fileEntryBrokerReadVersion);
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

                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerPeople));

                if (metadata != null)
                {
                    //Exif tool
                    PopulatePeople(dataGridView, metadata, columnIndex, MetadataBrokerTypes.ExifTool);

                    //Windows Live Gallery
                    Metadata metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.ReadCache(
                        new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.WindowsLivePhotoGallery));                
                    if (metadataWindowsLivePhotoGallery != null) PopulatePeople(dataGridView, metadataWindowsLivePhotoGallery, columnIndex, MetadataBrokerTypes.WindowsLivePhotoGallery);

                    //Microsoft Photos
                    Metadata metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.ReadCache(
                        new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.MicrosoftPhotos));
                    if (metadataMicrosoftPhotos != null) PopulatePeople(dataGridView, metadataMicrosoftPhotos, columnIndex, MetadataBrokerTypes.MicrosoftPhotos);

                    PopulatePeopleTooltip(dataGridView, metadata, columnIndex, MetadataBrokerTypes.ExifTool);
                    if (metadataWindowsLivePhotoGallery != null) PopulatePeopleTooltip(dataGridView, metadataWindowsLivePhotoGallery, columnIndex, MetadataBrokerTypes.WindowsLivePhotoGallery);
                    if (metadataMicrosoftPhotos != null) PopulatePeopleTooltip(dataGridView, metadataMicrosoftPhotos, columnIndex, MetadataBrokerTypes.MicrosoftPhotos);
                }
            }
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
