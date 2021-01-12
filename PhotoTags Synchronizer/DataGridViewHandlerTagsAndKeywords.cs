using DataGridViewGeneric;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static Manina.Windows.Forms.ImageListView;

namespace PhotoTagsSynchronizer
{
    public static class DataGridViewHandlerTagsAndKeywords
    {
        public const string headerMedia = "Media";
        public const string headerMicrosoftPhotos = "Microsoft Photos";
        public const string headerFolder = "Folder";
        public const string headerWindowsLivePhotoGallery = "Windows Live Photo Gallery";
        public const string headerKeywords = "Keywords";
        public static readonly string[] sourceHeaders = {headerMedia, headerMicrosoftPhotos, headerFolder, headerWindowsLivePhotoGallery};
        public const string tagAlbum = "Album";
        public const string tagTitle = "Title";
        public const string tagDescription = "Description";
        public const string tagComments = "Comments";
        public const string tagRating = "Rating(1-5 stars)";
        public const string tagAuthor = "Author";

        public static MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataMicrosoftPhotos { get; set; }
        public static MetadataDatabaseCache DatabaseAndCacheMetadataWindowsLivePhotoGallery { get; set; }

        public static double MediaAiTagConfidence { get; set; }


        //Check what data has been updated by users
        public static void GetUserInputChanges(ref DataGridView dataGridView, Metadata metadata, FileEntry fileEntryColumn)
        {
            int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, headerKeywords);
            int keywordsEnds = DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, headerKeywords);

            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntryColumn);
            //DataGridViewHandler.ClearFileBeenUpdated(dataGridView, columnIndex);

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

        }

        

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, bool sort)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, sort);
        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        {
            //                          return AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, cellReadOnly));
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, cellReadOnly), false);
            return rowIndex;
        }

        private static int AddRowKeywords(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults, bool sort)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, dataGridViewGenericCellStatusDefaults, sort);
            return rowIndex;
        }
       

        private static void PopulateKeywords(DataGridView dataGridView, Metadata metadata, int columnIndex, MetadataBrokerTypes metadataBrokerType)
        {
            foreach (KeywordTag tag in metadata.PersonalKeywordTags)
            {
                if (tag.Confidence >= MediaAiTagConfidence)
                {
                    int rowIndex = AddRowKeywords(dataGridView, columnIndex, 
                        new DataGridViewGenericRow(headerKeywords, tag.Keyword, ReadWriteAccess.ForceCellToReadOnly), 
                        tag.Keyword, 
                        new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Undefine, true), true);

                    //Updated default cell status with new staus
                    DataGridViewGenericCellStatus dataGridViewGenericCellStatus = new DataGridViewGenericCellStatus(DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex));

                    dataGridViewGenericCellStatus.MetadataBrokerTypes |= metadataBrokerType;
                    if (dataGridViewGenericCellStatus.SwitchState == SwitchStates.Undefine)
                        dataGridViewGenericCellStatus.SwitchState = (dataGridViewGenericCellStatus.MetadataBrokerTypes & MetadataBrokerTypes.ExifTool) == MetadataBrokerTypes.ExifTool ? SwitchStates.On : SwitchStates.Off;
                    DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
                }
            }

            //If updated, check if any keyword are removed from the list
            int keywordsStarts = DataGridViewHandler.GetRowHeaderItemStarts(dataGridView, headerKeywords);
            int keywordsEnds = DataGridViewHandler.GetRowHeaderItemsEnds(dataGridView, headerKeywords);
            
            for (int rowIndex = keywordsStarts; rowIndex <= keywordsEnds; rowIndex++)
            {
                DataGridViewGenericCellStatus dataGridViewGenericCellStatus = DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex);
                if ((dataGridViewGenericCellStatus.MetadataBrokerTypes & metadataBrokerType) == metadataBrokerType)
                {                                        

                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);

                    if (!metadata.PersonalKeywordTags.Contains(new KeywordTag(dataGridViewGenericRow.RowName)))
                    {
                        if ((dataGridViewGenericCellStatus.MetadataBrokerTypes & metadataBrokerType) == MetadataBrokerTypes.ExifTool && dataGridViewGenericCellStatus.SwitchState == SwitchStates.On)
                            dataGridViewGenericCellStatus.SwitchState = SwitchStates.Undefine;

                        dataGridViewGenericCellStatus.MetadataBrokerTypes &= ~metadataBrokerType; //Remove flag if line deleted
                        DataGridViewHandler.SetCellStatus(dataGridView, columnIndex, rowIndex, dataGridViewGenericCellStatus);
                    }
                }
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
            DateTime? dateTimeNewest = null;
            if (dateTimeForEditableMediaFile == DataGridViewHandler.DateTimeForEditableMediaFile && fileVersionDates.Count > 0)
            {
                dateTimeNewest = FileEntryBroker.FindNewestDate(fileVersionDates); 
                fileVersionDates.Add(new FileEntryBroker(fullFilePath, DataGridViewHandler.DateTimeForEditableMediaFile, MetadataBrokerTypes.ExifTool));
            }

            foreach (FileEntryBroker fileEntryBroker in fileVersionDates)
            {
                //DataGridViewHandler.GetColumnCount(dataGridView); //Rememebr coulmn count before AddColumnOrUpdate

                FileEntryBroker fileEntryBrokerReadVersion = fileEntryBroker;

                Metadata metadata;
                //It's the edit column, edit column is a new column copy og last known data
                if (fileEntryBroker.LastWriteDateTime == DataGridViewHandler.DateTimeForEditableMediaFile)
                {
                    fileEntryBrokerReadVersion = new FileEntryBroker(fullFilePath, (DateTime)dateTimeNewest, MetadataBrokerTypes.ExifTool);
                    metadata = new Metadata(DatabaseAndCacheMetadataExiftool.MetadataCacheOnly(fileEntryBrokerReadVersion));
                    //metadata = new Metadata(DatabaseAndCacheMetadataExiftool.MetadataCacheReadOrDatabase(fileEntryBrokerReadVersion));
                }
                else
                {
                    metadata = DatabaseAndCacheMetadataExiftool.MetadataCacheOnly(fileEntryBrokerReadVersion);
                }
               

                int columnIndex = DataGridViewHandler.AddColumnOrUpdate(dataGridView,
                    new FileEntryImage(fileEntryBroker),                                                    /* This Column idenity                                      */
                    metadata,                                                                               /* Metadata will save on column                             */
                    dateTimeForEditableMediaFile,                                                           /* idenify what column that we can edit                     */
                    DataGridViewHandler.IsCurrentFile(fileEntryBroker, dateTimeForEditableMediaFile) ?      /* Metadata.FileEntry read date, fileEntryBroker fake future version */ 
                    ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly,            /* this will set histority columns as read only columns    */
                    showWhatColumns,                                                                        /* show Edit | Hisorical columns | Error columns            */
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Off, true));  /* New cells will have this value                           */
                if (columnIndex == -1) continue;

                //Media
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia), false);
                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagAlbum), metadata?.PersonalAlbum, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagTitle), metadata?.PersonalTitle, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagDescription), metadata?.PersonalDescription, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagComments), metadata?.PersonalComments, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagRating), metadata?.PersonalRating, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagAuthor), metadata?.PersonalAuthor, false);

                // Microsoft Phontos
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos), false);
                Metadata metadataMicrosoftPhotos = null;
                if (metadata != null) metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.MetadataCacheReadOrDatabase(new FileEntryBroker (fileEntryBrokerReadVersion, MetadataBrokerTypes.MicrosoftPhotos));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagAlbum), metadataMicrosoftPhotos?.PersonalAlbum, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagTitle), metadataMicrosoftPhotos?.PersonalTitle, true);
            
                // Folder path as Album
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerFolder), false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerFolder, tagAlbum), new DirectoryInfo(fileEntryBrokerReadVersion.Directory).Name, true);
                
                //Windows Live Photo Gallery
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery), false);
                Metadata metadataWindowsLivePhotoGallery = null;
                if (metadata != null) metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.MetadataCacheReadOrDatabase(new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.WindowsLivePhotoGallery));
               
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagTitle), metadata?.PersonalTitle, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagRating), metadata?.PersonalRating, true);
                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerKeywords), false);
                 
                if (metadata != null)
                {
                    PopulateKeywords(dataGridView, metadata, columnIndex, MetadataBrokerTypes.ExifTool);
                    
                    if (metadataMicrosoftPhotos != null)
                    {
                        PopulateKeywords(dataGridView, metadataMicrosoftPhotos, columnIndex, MetadataBrokerTypes.MicrosoftPhotos);
                    }

                    if (metadataWindowsLivePhotoGallery != null)
                    {
                        PopulateKeywords(dataGridView, metadataWindowsLivePhotoGallery, columnIndex, MetadataBrokerTypes.WindowsLivePhotoGallery);
                    }
                }

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
