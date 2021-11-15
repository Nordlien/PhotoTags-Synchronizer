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
        public static ThumbnailDatabaseCache DatabaseAndCacheThumbnail { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }
        public static List<AutoKeywordConvertion> AutoKeywordConvertions { get; set; }

        public static double MediaAiTagConfidence { get; set; }

        #region GetUserInputChanges
        //Check what data has been updated by users
        public static void GetUserInputChanges(ref KryptonDataGridView dataGridView, Metadata metadata, FileEntryAttribute fileEntryColumn)
        {

            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntryColumn);
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
                    DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords); ;

                    //Updated default cell status with new staus
                    DataGridViewGenericCellStatus dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex));

                    dataGridViewGenericCellStatus.MetadataBrokerType |= metadataBrokerType;
                    if (fileEntryAttribute.FileEntryVersion != FileEntryVersion.AutoCorrect)
                    {
                        if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Undefine)
                            dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerType & MetadataBrokerType.ExifTool) == MetadataBrokerType.ExifTool ? SwitchStates.On : SwitchStates.Off;
                    }
                    else
                    {
                        dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerType & MetadataBrokerType.ExifTool) == MetadataBrokerType.ExifTool ? SwitchStates.On : SwitchStates.Off;
                    }
                    DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
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
                        DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
                    }
                }
            }
        }
        #endregion

        #region PopulateFile
        public static void PopulateFile(DataGridView dataGridView, FileEntryAttribute fileEntryAttribute, ShowWhatColumns showWhatColumns, Metadata metadataAutoCorrected, bool onlyRefresh)
        {
            //-----------------------------------------------------------------
            //Chech if need to stop
            if (GlobalData.IsApplicationClosing) return;
            if (!DataGridViewHandler.GetIsAgregated(dataGridView)) return;      //Not default columns or rows added
            if (DataGridViewHandler.GetIsPopulatingFile(dataGridView)) 
                return;  //In progress doing so

            //Check if file is in DataGridView
            if (!DataGridViewHandler.DoesColumnFilenameExist(dataGridView, fileEntryAttribute.FileFullPath)) return;

            //When file found, Tell it's populating file, avoid two process updates
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, true);

            //-----------------------------------------------------------------
            Image thumbnail = DatabaseAndCacheThumbnail.ReadThumbnailFromCacheOnlyClone(fileEntryAttribute);
            FileEntryBroker fileEntryBrokerReadVersion = fileEntryAttribute.GetFileEntryBroker(MetadataBrokerType.ExifTool);

            Metadata metadata = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBrokerReadVersion);            
            
            //It's the edit column, make a copy do edit in dataGridView updated the origianal metadata
            if ((fileEntryAttribute.FileEntryVersion == FileEntryVersion.AutoCorrect || fileEntryAttribute.FileEntryVersion == FileEntryVersion.Current)
                && metadata != null) metadata = new Metadata(metadata);
            ReadWriteAccess readWriteAccessColumn =
                (fileEntryAttribute.FileEntryVersion == FileEntryVersion.AutoCorrect || fileEntryAttribute.FileEntryVersion == FileEntryVersion.Current) &&
                metadata != null ? ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly; 
            
            int columnIndex = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView, fileEntryAttribute, thumbnail, metadata, readWriteAccessColumn, showWhatColumns, DataGridViewGenericCellStatus.DefaultEmpty());
            //-----------------------------------------------------------------
            if (metadataAutoCorrected != null) 
                metadata = metadataAutoCorrected; //If AutoCorrect is run, use AutoCorrect values. Needs to be after DataGridViewHandler.AddColumnOrUpdateNew, so orignal metadata stored will not be overwritten
            if (columnIndex == -1) columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntryAttribute); //Find column Index for Filename and date last written

            //Chech if populated and new refresh data
            if (onlyRefresh && columnIndex != -1 && !DataGridViewHandler.IsColumnPopulated(dataGridView, columnIndex)) columnIndex = -1; //No refresh needed

            if (columnIndex != -1)
            {
                //Media
                int rowIndex;
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia), false);

                rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagAlbum), metadata?.PersonalAlbum, false);
                List<string> newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, null, null, metadata?.PersonalAlbum, null, null, null);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);

                rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagTitle), metadata?.PersonalTitle, false);
                newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, null, metadata?.PersonalTitle, null, null, null, null);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);

                rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagDescription), metadata?.PersonalDescription, false);
                newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, null, null, null, metadata?.PersonalDescription, null, null);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);

                rowIndex = AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagComments), metadata?.PersonalComments, false);
                newKeywords = AutoKeywordHandler.NewKeywords(AutoKeywordConvertions, null, null, null, null, metadata?.PersonalComments, null);
                DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, "Running AutoCorrect will add these keywords", newKeywords);
                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagRating), metadata?.PersonalRating, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagAuthor), metadata?.PersonalAuthor, false);

                // Microsoft Phontos
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos), false);
                Metadata metadataMicrosoftPhotos = null;
                if (metadata != null) metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.MicrosoftPhotos));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagAlbum), metadataMicrosoftPhotos?.PersonalAlbum, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagTitle), metadataMicrosoftPhotos?.PersonalTitle, true);

                // Folder path as Album
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerFolder), false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerFolder, tagAlbum), new DirectoryInfo(fileEntryBrokerReadVersion.Directory).Name, true);

                //Windows Live Photo Gallery
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery), false);
                Metadata metadataWindowsLivePhotoGallery = null;
                if (metadata != null) metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.WindowsLivePhotoGallery));

                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagTitle), metadata?.PersonalTitle, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagRating), metadata?.PersonalRating, true);

                // WebScarping
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWebScraping), false);
                Metadata metadataWebScraping = null;
                if (metadata != null) metadataWebScraping = DatabaseAndCacheMetadataExiftool.ReadWebScraperMetadataFromCacheOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerType.WebScraping));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWebScraping, tagAlbum), metadataWebScraping?.PersonalAlbum, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWebScraping, tagTitle), metadataWebScraping?.PersonalTitle, true);


                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerKeywords), false);

                if (metadata != null) PopulateKeywords(dataGridView, metadata, columnIndex, metadata.Broker, fileEntryAttribute);
                if (metadataMicrosoftPhotos != null) PopulateKeywords(dataGridView, metadataMicrosoftPhotos, columnIndex, metadataMicrosoftPhotos.Broker, fileEntryAttribute);
                if (metadataWindowsLivePhotoGallery != null) PopulateKeywords(dataGridView, metadataWindowsLivePhotoGallery, columnIndex, metadataWindowsLivePhotoGallery.Broker, fileEntryAttribute);
                if (metadataWebScraping != null) PopulateKeywords(dataGridView, metadataWebScraping, columnIndex, metadataWebScraping.Broker, fileEntryAttribute);

                DataGridViewHandler.SetColumnPopulatedFlag(dataGridView, columnIndex, true);
            }
            //-----------------------------------------------------------------
            DataGridViewHandler.SetIsPopulatingFile(dataGridView, false);
            //-----------------------------------------------------------------
        }
        #endregion

        #region PopulateSelectedFiles
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
