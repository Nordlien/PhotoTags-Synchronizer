using DataGridViewGeneric;
using Manina.Windows.Forms;
using MetadataLibrary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Thumbnails;
using static Manina.Windows.Forms.ImageListView;
using Krypton.Toolkit;
using System.Threading;

namespace PhotoTagsSynchronizer
{

    public static class DataGridViewHandlerTagsAndKeywords
    {
        public const string headerMedia = "Media";
        public const string headerMicrosoftPhotos = "Microsoft Photos";
        public const string headerFolder = "Folder";
        public const string headerWindowsLivePhotoGallery = "Windows Live Photo Gallery";
        public const string headerKeywords = "Keywords";
        public const string headerWebScraping = "WebScraper";
        public static readonly string[] sourceHeaders = {headerMedia, headerMicrosoftPhotos, headerFolder, headerWindowsLivePhotoGallery, headerWebScraping};
        public const string tagAlbum = "Album";
        public const string tagTitle = "Title";
        public const string tagDescription = "Description";
        public const string tagComments = "Comments";
        public const string tagRating = "Rating(1-5 stars)";
        public const string tagAuthor = "Author";

        public static bool HasBeenInitialized { get; set; } = false;
        public static ThumbnailPosterDatabaseCache DatabaseAndCacheThumbnail { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }
        public static List<AutoKeywordConvertion> AutoKeywordConvertions { get; set; }

        public static double MediaAiTagConfidence { get; set; }

        #region GetUserInputChanges
        //Check what data has been updated by users
        public static void GetUserInputChanges(DataGridView dataGridView, ref Metadata metadata, FileEntryAttribute fileEntryColumn)
        {

            int columnIndex = DataGridViewHandler.GetColumnIndexUserInput(dataGridView, fileEntryColumn);
            if (columnIndex == -1) return; //Column has not yet become aggregated or has already been removed
            if (!DataGridViewHandler.IsColumnPopulated(dataGridView, columnIndex)) return;
            

            int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, headerKeywords);
            int keywordsEnds = DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, headerKeywords);

            metadata.PersonalAlbum = (string)DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, headerMedia, tagAlbum);
            if (metadata.PersonalAlbum != null) metadata.PersonalAlbum = metadata.PersonalAlbum.Trim();
            
            metadata.PersonalTitle = (string)DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, headerMedia, tagTitle);
            if (metadata.PersonalTitle != null) metadata.PersonalTitle = metadata.PersonalTitle.Trim();

            metadata.PersonalDescription = (string)DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, headerMedia, tagDescription);
            if (metadata.PersonalDescription != null) metadata.PersonalDescription = metadata.PersonalDescription.Trim();

            metadata.PersonalComments = (string)DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, headerMedia, tagComments);
            if (metadata.PersonalComments != null) metadata.PersonalComments = metadata.PersonalComments.Trim();

            metadata.PersonalAuthor = (string)DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndex, headerMedia, tagAuthor);
            if (metadata.PersonalAuthor != null) metadata.PersonalAuthor = metadata.PersonalAuthor.Trim();

            byte rating = 255; //empty
            var ratingValue = DataGridViewHandler.GetCellValue(dataGridView, columnIndex, headerMedia, tagRating);
            if (ratingValue != null && !byte.TryParse(ratingValue.ToString(), out rating)) rating = 255;
            
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

        }
        #endregion

        #region AddRow
        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, bool sort)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, sort);
        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Disabled, cellReadOnly), false);
            return rowIndex;
        }
        #endregion

        #region AddRowKeywords
        private static int AddRowKeywords(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults, bool sort)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, dataGridViewGenericCellStatusDefaults, sort);
            return rowIndex;
        }
        #endregion

        #region PopulateKeywords
        private static void PopulateKeywords(DataGridView dataGridView, Metadata metadata, int columnIndex, MetadataBrokerType metadataBrokerType, FileEntryAttribute fileEntryAttribute)
        {
            foreach (KeywordTag tag in metadata.PersonalKeywordTags)
            {
                if (tag.Confidence >= MediaAiTagConfidence)
                {
                    int rowIndex = AddRowKeywords(dataGridView, columnIndex, 
                        new DataGridViewGenericRow(headerKeywords, tag.Keyword, ReadWriteAccess.ForceCellToReadOnly), 
                        tag.Keyword, 
                        new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Undefine, true), true);

                    List<KeywordTag> keywordTags = new List<KeywordTag>();
                    keywordTags.Add(new KeywordTag(tag.Keyword));

                    List<string> newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, null, null, null, null, null, keywordTags);
                    DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);

                    //Updated default cell status with new staus
                    DataGridViewGenericCellStatus dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex));

                    dataGridViewGenericCellStatus.MetadataBrokerType |= metadataBrokerType;
                    if (fileEntryAttribute.FileEntryVersion != FileEntryVersion.CompatibilityFixedAndAutoUpdated &&
                        fileEntryAttribute.FileEntryVersion != FileEntryVersion.MetadataToSave)
                    {
                        if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Undefine)
                            dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerType & MetadataBrokerType.ExifTool) == MetadataBrokerType.ExifTool ? SwitchStates.On : SwitchStates.Off;
                    }
                    else
                    {
                        dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerType & MetadataBrokerType.ExifTool) == MetadataBrokerType.ExifTool ? SwitchStates.On : SwitchStates.Off;
                        DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);

                        if (dataGridViewGenericRow != null && dataGridViewGenericRow.ReadWriteAccess == ReadWriteAccess.AllowCellReadAndWrite)
                        {
                            dataGridViewGenericRow.ReadWriteAccess = ReadWriteAccess.ForceCellToReadOnly;
                            for (int columnIndexUpdate = 0; columnIndexUpdate < DataGridViewHandler.GetColumnCount(dataGridView); columnIndexUpdate++)
                                DataGridViewHandler.SetCellReadOnlyDependingOfStatus(dataGridView, columnIndexUpdate, rowIndex, dataGridViewGenericCellStatus);

                        }
                    }
                    DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus, false);
                }
            }

            //If updated, check if any keyword are removed from the list
            int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, headerKeywords);
            int keywordsEnds = DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, headerKeywords);
            
            for (int rowIndex = keywordsStarts; rowIndex <= keywordsEnds; rowIndex++)
            {
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus = DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex);
                if ((dataGridViewGenericCellStatus.MetadataBrokerType & metadataBrokerType) == metadataBrokerType)
                {                                        

                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);

                    if (!metadata.PersonalKeywordTags.Contains(new KeywordTag(dataGridViewGenericRow.RowName)))
                    {
                        if ((dataGridViewGenericCellStatus.MetadataBrokerType & metadataBrokerType) == MetadataBrokerType.ExifTool && dataGridViewGenericCellStatus.SwitchState == SwitchStates.On)
                            dataGridViewGenericCellStatus.SwitchState = SwitchStates.Undefine;

                        dataGridViewGenericCellStatus.MetadataBrokerType &= ~metadataBrokerType; //Remove flag if line deleted
                        DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus, false);
                    }
                }
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

            //Check if file is in DataGridView
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fileEntryAttribute.FileFullPath)) return -1;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);

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
                //Media
                int rowIndex;
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia), false);

                rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagAlbum), metadataExiftool?.PersonalAlbum, false);
                List<string> newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, null, null, metadataExiftool?.PersonalAlbum, null, null, null);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);

                rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagTitle), metadataExiftool?.PersonalTitle, false);
                newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, null, metadataExiftool?.PersonalTitle, null, null, null, null);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);

                rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagDescription), metadataExiftool?.PersonalDescription, false);
                newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, null, null, null, metadataExiftool?.PersonalDescription, null, null);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);

                rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagComments), metadataExiftool?.PersonalComments, false);
                newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, null, null, null, null, metadataExiftool?.PersonalComments, null);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);
                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagRating), metadataExiftool?.PersonalRating, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagAuthor), metadataExiftool?.PersonalAuthor, false);

                // Microsoft Phontos
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos), false);
                Metadata metadataMicrosoftPhotos = null;
                if (metadataExiftool != null) metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.MicrosoftPhotos));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagAlbum), metadataMicrosoftPhotos?.PersonalAlbum, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagTitle), metadataMicrosoftPhotos?.PersonalTitle, true);

                // Folder path as Album
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerFolder), false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerFolder, tagAlbum), new DirectoryInfo(fileEntryBrokerReadVersion.Directory).Name, true);

                //Windows Live Photo Gallery
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery), false);
                Metadata metadataWindowsLivePhotoGallery = null;
                if (metadataExiftool != null) metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.WindowsLivePhotoGallery));

                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagTitle), metadataWindowsLivePhotoGallery?.PersonalTitle, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagRating), metadataWindowsLivePhotoGallery?.PersonalRating, true);

                // WebScarping
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWebScraping), false);
                Metadata metadataWebScraping = null;
                if (metadataExiftool != null) metadataWebScraping = DatabaseAndCacheMetadataExiftool.ReadWebScraperMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.WebScraping));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWebScraping, tagAlbum), metadataWebScraping?.PersonalAlbum, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWebScraping, tagTitle), metadataWebScraping?.PersonalTitle, true);


                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerKeywords), false);

                if (fileEntryAttribute.FileEntryVersion == FileEntryVersion.CompatibilityFixedAndAutoUpdated ||
                    fileEntryAttribute.FileEntryVersion == FileEntryVersion.MetadataToSave)
                {
                    int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, headerKeywords);
                    int keywordsEnds = DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, headerKeywords);
                    for (int rowIndexToClean = keywordsStarts; rowIndexToClean <= keywordsEnds; rowIndexToClean++)
                    {
                        DataGridViewGenericCellStatus dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Undefine, true);
                        DataGridViewHandler.SetCellReadOnlyDependingOfStatus(dataGridView, columnIndex, rowIndexToClean, dataGridViewGenericCellStatus);
                        DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndexToClean, dataGridViewGenericCellStatus, false);
                    }
                }

                if (metadataExiftool != null) PopulateKeywords(dataGridView, metadataExiftool, columnIndex, metadataExiftool.Broker, fileEntryAttribute);
                if (metadataMicrosoftPhotos != null) PopulateKeywords(dataGridView, metadataMicrosoftPhotos, columnIndex, metadataMicrosoftPhotos.Broker, fileEntryAttribute);
                if (metadataWindowsLivePhotoGallery != null) PopulateKeywords(dataGridView, metadataWindowsLivePhotoGallery, columnIndex, metadataWindowsLivePhotoGallery.Broker, fileEntryAttribute);
                if (metadataWebScraping != null) PopulateKeywords(dataGridView, metadataWebScraping, columnIndex, metadataWebScraping.Broker, fileEntryAttribute);


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
