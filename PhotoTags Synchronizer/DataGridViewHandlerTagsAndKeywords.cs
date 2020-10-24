using DataGridViewGeneric;
using Manina.Windows.Forms;
using MetadataLibrary;
using System;
using System.Collections.Generic;
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
            int keywordsStarts = DataGridViewHandler.GetRowHeadingItemStarts(dataGridView, headerKeywords);
            int keywordsEnds = DataGridViewHandler.GetRowHeadingItemsEnds(dataGridView, headerKeywords);

            int columnIndex = DataGridViewHandler.GetColumnIndex(dataGridView, fileEntryColumn);
                
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

        

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow)
        {
            return DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow);
        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, bool cellReadOnly)
        {
            return AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, cellReadOnly));
        }

        private static int AddRow(DataGridView dataGridView, int columnIndex, DataGridViewGenericRow dataGridViewGenericDataRow, object value, DataGridViewGenericCellStatus dataGridViewGenericCellStatusDefaults)
        {
            int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndex, dataGridViewGenericDataRow, value, dataGridViewGenericCellStatusDefaults);
            return rowIndex;
        }
       

        private static void PopulateKeywords(DataGridView dataGridView, Metadata metadata, int columnIndex, MetadataBrokerTypes metadataBrokerType)
        {
            foreach (KeywordTag tag in metadata.PersonalKeywordTags)
            {
                if (tag.Confidence >= MediaAiTagConfidence)
                {
                    int rowIndex = AddRow(dataGridView, columnIndex, 
                        new DataGridViewGenericRow(headerKeywords, tag.Keyword, ReadWriteAccess.ForceCellToReadOnly), 
                        tag.Keyword, 
                        new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Off, true));

                    //Updated default cell status with new staus
                    DataGridViewHandler.SetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex,
                        DataGridViewHandler.GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) | metadataBrokerType);
                    DataGridViewHandler.SetCellStatusSwichStatus(dataGridView, columnIndex, rowIndex, 
                        (DataGridViewHandler.GetCellStatusMetadataBrokerType(dataGridView, columnIndex, rowIndex) & MetadataBrokerTypes.ExifTool) == MetadataBrokerTypes.ExifTool ? SwitchStates.On : SwitchStates.Off);

                    DataGridViewGenericCellStatus dataGridViewGenericCellStatus = DataGridViewHandler.GetCellStatus(dataGridView, columnIndex, rowIndex);
                    string toolTipText = DataGridViewHandler.GetCellToolTipText(dataGridView, columnIndex, rowIndex);
                    toolTipText = "" + toolTipText + dataGridViewGenericCellStatus.CellReadOnly + " " + 
                        dataGridViewGenericCellStatus.SwitchState.ToString() + " " + 
                        dataGridViewGenericCellStatus.MetadataBrokerTypes.ToString() + "\r\n";
                    DataGridViewHandler.SetCellToolTipText(dataGridView, columnIndex, rowIndex, toolTipText);
                    
                    DataGridViewHandler.SetCellStatusDefaultWhenRowAdded(dataGridView, DataGridViewHandler.GetRowCount(dataGridView) - 1, 
                        new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Disabled, false));
                    DataGridViewHandler.SetCellBackGroundColorForRow(dataGridView, DataGridViewHandler.GetRowCount(dataGridView) - 1);
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
            if (dateTimeForEditableMediaFile == DataGridViewHandler.DateTimeForEditableMediaFile && fileVersionDates.Count > 0)
            {                
                fileVersionDates.Add(new FileEntryBroker(fullFilePath, DataGridViewHandler.DateTimeForEditableMediaFile, MetadataBrokerTypes.ExifTool));
            }

            foreach (FileEntryBroker fileEntryBroker in fileVersionDates)
            {
                int columnCountBeforeAddOrUpdate = DataGridViewHandler.GetColumnCount(dataGridView); //Rememebr coulmn count before AddColumnOrUpdate

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
                
                int columnIndex = DataGridViewHandler.AddColumnOrUpdate(dataGridView, new FileEntryImage(fileEntryBrokerColumnVersion), metadata, dateTimeForEditableMediaFile,
                    DataGridViewHandler.IsCurrentFile(fileEntryBrokerColumnVersion, dateTimeForEditableMediaFile) ? ReadWriteAccess.AllowCellReadAndWrite : ReadWriteAccess.ForceCellToReadOnly, showWhatColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Off, true));
                if (columnIndex == -1) continue; // -1 = Do not show

                //Media
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia));
                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagAlbum), metadata?.PersonalAlbum, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagTitle), metadata?.PersonalTitle, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagDescription), metadata?.PersonalDescription, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagComments), metadata?.PersonalComments, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagRating), metadata?.PersonalRating, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, tagAuthor), metadata?.PersonalAuthor, false);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMedia, "Altitude"), metadata?.LocationAltitude, false);

                // Microsoft Phontos
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos));
                Metadata metadataMicrosoftPhotos = null;
                if (metadata != null) metadataMicrosoftPhotos = DatabaseAndCacheMetadataMicrosoftPhotos.ReadCache(
                    new FileEntryBroker (fileEntryBrokerReadVersion, MetadataBrokerTypes.MicrosoftPhotos));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagAlbum), metadataMicrosoftPhotos?.PersonalAlbum, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerMicrosoftPhotos, tagTitle), metadataMicrosoftPhotos?.PersonalTitle, true);
            
                // Folder path as Album
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerFolder));
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerFolder, tagAlbum), Path.GetFileName(fileEntryBrokerReadVersion.Directory), true);

                //Windows Live Photo Gallery
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery));
                Metadata metadataWindowsLivePhotoGallery = null;
                if (metadata != null) metadataWindowsLivePhotoGallery = DatabaseAndCacheMetadataWindowsLivePhotoGallery.ReadCache(
                    new FileEntryBroker(fileEntryBrokerReadVersion, MetadataBrokerTypes.WindowsLivePhotoGallery));
               
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagTitle), metadata?.PersonalTitle, true);
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerWindowsLivePhotoGallery, tagRating), metadata?.PersonalRating, true);
                
                AddRow(dataGridView, columnIndex, new DataGridViewGenericRow(headerKeywords));
                 
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
