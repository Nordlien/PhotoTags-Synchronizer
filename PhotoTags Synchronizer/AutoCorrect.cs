using CameraOwners;
using Exiftool;
using FileDateTime;
using FileHandeling;
using GoogleLocationHistory;
using LocationNames;
using MetadataLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TimeZone;

namespace PhotoTagsSynchronizer
{
    public class AutoKeywordConvertion
    {
        public HashSet<string> LocationNames = new HashSet<string>();
        public HashSet<string> Titles = new HashSet<string>();
        public HashSet<string> Descriptions = new HashSet<string>();
        public HashSet<string> Comments = new HashSet<string>();
        public HashSet<string> Albums = new HashSet<string>();
        public HashSet<string> Keywords = new HashSet<string>();
        public List<string> NewKeywords = new List<string>();

        #region AddRangeToUpper
        public static void AddRangeToUpper(HashSet<string> list, string[] addThisArray)
        {
            foreach (string addThisItem in addThisArray)
            {
                if (!string.IsNullOrWhiteSpace(addThisItem)) 
                {
                    string itemToUpper = addThisItem.ToUpper();
                    if (!list.Contains(itemToUpper)) list.Add(itemToUpper);
                } 
            }
        }
        #endregion

        #region DoesWordExistInList
        private bool DoesWordExistInList(HashSet<string> list, string findInThisText)
        {
            if (string.IsNullOrWhiteSpace(findInThisText)) return false;
            findInThisText = findInThisText.ToUpper();

            foreach (string word in list)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    int findInThisTextLastChar = findInThisText.Length - 1;
                    int wordLength = word.Length;
                    int startSearchIndex = 0;
                    bool needSearchMore = true;
                    
                    while (startSearchIndex <= findInThisTextLastChar && needSearchMore)
                    {
                        int foundLocation = findInThisText.IndexOf(word, startSearchIndex);
                        if (foundLocation > -1)
                        {
                            bool isWhiteSpaceInFront = true;
                            int locationBefore = foundLocation - 1;
                            if (foundLocation > 0 && 
                                !char.IsWhiteSpace(findInThisText[locationBefore]) && 
                                !char.IsPunctuation(findInThisText[locationBefore])) isWhiteSpaceInFront = false;

                            bool isWhiteSpaceAfter = true;
                            if (isWhiteSpaceInFront) //No need to check
                            {
                                int locationAfter = foundLocation + wordLength;
                                if (locationAfter <= findInThisTextLastChar &&
                                    !char.IsWhiteSpace(findInThisText[locationAfter]) &&
                                    !char.IsPunctuation(findInThisText[locationAfter])) isWhiteSpaceAfter = false;
                            }

                            if (isWhiteSpaceInFront && isWhiteSpaceAfter) 
                                return true; //Found a word (and not whinin other words)
                            startSearchIndex = foundLocation + 1; //It was not a word, but text whithin, need continue search
                            needSearchMore = true;
                        }
                        else
                        {
                            needSearchMore = false; //Nothing found, don't need search for more
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region DoesWordExistInAnyList
        public bool DoesWordExistInAnyList(string locationName, string title, string album, string description, string comment, List<KeywordTag> keywordTags)
        {
            if (DoesWordExistInList(LocationNames, locationName)) return true;
            if (DoesWordExistInList(Titles, title)) return true;
            if (DoesWordExistInList(Albums, album)) return true;
            if (DoesWordExistInList(Descriptions, description)) return true;
            if (DoesWordExistInList(Comments, comment)) return true;
            if (keywordTags != null) foreach(KeywordTag keywordTag in keywordTags) if (DoesWordExistInList(Keywords, keywordTag.Keyword)) return true;
            return false;
        }
        #endregion 
    }
    class AutoKeywordHandler
    {
        #region Const
        const string Filename = "AutoKeywords.xml";
        const string DataSetName = "AutoKeywordsDataSet";
        const string TableName = "AutoKeywords";
        #endregion

        #region ReadDataSetFromXML
        public static DataSet ReadDataSetFromXML(string path)
        {
            DataSet dataSet = new DataSet();
            try
            {
                if (File.Exists(path)) dataSet.ReadXml(path);
            }
            catch { }
            return dataSet;
        }
        public static DataSet ReadDataSetFromXML()
        {
            string path = FileHandler.GetLocalApplicationDataPath(Filename, false, null);
            return ReadDataSetFromXML(path);
        }
        #endregion

        #region PopulateDataGridView
        public static void PopulateDataGridView(DataGridView dataGridView, DataSet dataSet)
        {
            dataGridView.Rows.Clear();
            
            if (dataSet.Tables.Count >= 1)
            {
                int index = dataSet.Tables.IndexOf(TableName);
                if (index >= 0) {

                    DataTable dataTable = dataSet.Tables[index];
                    dataGridView.Rows.Add(dataTable.Rows.Count);

                    for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                    {
                        dataGridView.Rows[rowIndex].HeaderCell.Value = (dataGridView.Rows[rowIndex].Index + 1).ToString();
                        
                        for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                        {
                            object[] dataItems = dataTable.Rows[rowIndex].ItemArray;
                            dataGridView.Rows[rowIndex].Cells[columnIndex].Value = dataItems[columnIndex];
                        }
                    }
                    dataGridView.Rows[dataGridView.Rows.Count - 1].HeaderCell.Value = "*" + (dataGridView.Rows[dataGridView.Rows.Count - 1].Index + 1).ToString();
                }
            }
        }
        #endregion

        #region ReadDataGridView
        public static DataSet ReadDataGridView(DataGridView dataGridView, bool IgnoreHideColumns = false)
        {
            try
            {
                if (dataGridView.ColumnCount == 0) return null;
                DataSet dataSet = new DataSet();

                DataTable dataTableSource = new DataTable();
                foreach (DataGridViewColumn dataGridViewColumn in dataGridView.Columns)
                {
                    if (IgnoreHideColumns & !dataGridViewColumn.Visible) continue;
                    if (dataGridViewColumn.Name == string.Empty) continue;
                    if (dataGridViewColumn.ValueType == null) dataGridViewColumn.ValueType = typeof(string);
                    dataTableSource.Columns.Add(dataGridViewColumn.Name, dataGridViewColumn.ValueType);
                    dataTableSource.Columns[dataGridViewColumn.Name].Caption = dataGridViewColumn.HeaderText;
                }
                if (dataTableSource.Columns.Count == 0) return null;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataRow drNewRow = dataTableSource.NewRow();
                        foreach (DataColumn col in dataTableSource.Columns)
                        {
                            drNewRow[col.ColumnName] = (row.Cells[col.ColumnName].Value == null ? "" : row.Cells[col.ColumnName].Value);
                        }
                        dataTableSource.Rows.Add(drNewRow);
                    }
                }
                dataSet.DataSetName = DataSetName;
                dataSet.Tables.Add(dataTableSource);
                dataSet.Tables[0].TableName = TableName;
                return dataSet;
            }
            catch { return null; }
        }
        #endregion

        #region PopulateList
        public static List<AutoKeywordConvertion> PopulateList(DataSet dataSet)
        {
            List<AutoKeywordConvertion> autoKeywordConvertions = new List<AutoKeywordConvertion>();

            if (dataSet.Tables.Count >= 1)
            {
                int index = dataSet.Tables.IndexOf(TableName);
                if (index >= 0)
                {
                    DataTable dataTable = dataSet.Tables[index];
                    for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                    {
                        AutoKeywordConvertion autoKeywordConvertion = new AutoKeywordConvertion();

                        object[] dataItems = dataTable.Rows[rowIndex].ItemArray;
                        
                        for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                        {
                            if (dataItems[columnIndex] is string)
                            {
                                string[] items = ((string)dataItems[columnIndex]).Replace("\r\n","\n").Split('\n');
                                switch (columnIndex)
                                {
                                    case 0:
                                        AutoKeywordConvertion.AddRangeToUpper(autoKeywordConvertion.LocationNames, items);
                                        break;
                                    case 1:
                                        AutoKeywordConvertion.AddRangeToUpper(autoKeywordConvertion.Titles, items);
                                        break;
                                    case 2:
                                        AutoKeywordConvertion.AddRangeToUpper(autoKeywordConvertion.Albums, items);
                                        break;
                                    case 3:
                                        AutoKeywordConvertion.AddRangeToUpper(autoKeywordConvertion.Descriptions, items);
                                        break;
                                    case 4:
                                        AutoKeywordConvertion.AddRangeToUpper(autoKeywordConvertion.Titles, items);
                                        break;
                                    case 5:
                                        AutoKeywordConvertion.AddRangeToUpper(autoKeywordConvertion.Keywords, items);
                                        break;
                                    case 6:
                                        autoKeywordConvertion.NewKeywords.AddRange(items); //Not ToUpper
                                        break;
                                }
                            }
                        }

                        autoKeywordConvertions.Add(autoKeywordConvertion);
                    }
                    
                }
            }
            return autoKeywordConvertions;
        }
        #endregion

        #region NewKeywords
        public static List<string> NewKeywords(List<AutoKeywordConvertion> autoKeywordConvertions, string locationName, string title, string album, string description, string comment, List<KeywordTag> keywordTags)
        {
            List<string> newKeywords = new List<string>();
            foreach (AutoKeywordConvertion autoKeywordConvertion in autoKeywordConvertions)
            {
                if (autoKeywordConvertion.DoesWordExistInAnyList(locationName, title, album, description, comment, keywordTags))
                {
                    foreach (string newKeyword in autoKeywordConvertion.NewKeywords)
                    {
                        if (!string.IsNullOrWhiteSpace(newKeyword) && !newKeywords.Contains(newKeyword)) newKeywords.Add(newKeyword);
                    }
                }
            }
            return newKeywords;
        }
        #endregion 
    }

    enum DateTimeSources
    {        
        DateTaken,
        SmartDate,
        GPSDateAndTime,
        FirstDateFoundInFilename,
        LastDateFoundInFilename,
        FileDate,
        FileCreateDate,
        FileModified
    }
 
    public class AutoCorrectFormVaraibles
    {
        public string Album { get; set; }
        public string Author { get; set; }
        public string Comments { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public List<string> Keywords { get; set; }

        public bool UseAlbum { get; set; }
        public bool UseAuthor { get; set; }
        public bool UseComments { get; set; }
        public bool UseDescription { get; set; }
        public bool WriteAlbumOnDescription { get; set; }
        public bool UseTitle { get; set; }

        #region UseAutoCorrectFormData
        public static void UseAutoCorrectFormData(ref Metadata metadataToSave, AutoCorrectFormVaraibles autoCorrectFormVaraibles)
        {
            if (autoCorrectFormVaraibles != null)
            {
                if (autoCorrectFormVaraibles.UseAlbum) metadataToSave.PersonalAlbum = autoCorrectFormVaraibles.Album;
                if (string.IsNullOrWhiteSpace(metadataToSave.PersonalAlbum)) metadataToSave.PersonalAlbum = null;

                if (autoCorrectFormVaraibles.UseAuthor) metadataToSave.PersonalAuthor = autoCorrectFormVaraibles.Author;
                if (string.IsNullOrWhiteSpace(metadataToSave.PersonalAuthor)) metadataToSave.PersonalAuthor = null;

                if (autoCorrectFormVaraibles.UseComments) metadataToSave.PersonalComments = autoCorrectFormVaraibles.Comments;
                if (string.IsNullOrWhiteSpace(metadataToSave.PersonalComments)) metadataToSave.PersonalComments = null;

                if (autoCorrectFormVaraibles.UseDescription) metadataToSave.PersonalDescription = autoCorrectFormVaraibles.Description;
                if (string.IsNullOrWhiteSpace(metadataToSave.PersonalDescription)) metadataToSave.PersonalDescription = null;

                if (autoCorrectFormVaraibles.UseTitle) metadataToSave.PersonalTitle = autoCorrectFormVaraibles.Title;
                if (string.IsNullOrWhiteSpace(metadataToSave.PersonalTitle)) metadataToSave.PersonalTitle = null;

                #region Description
                if (autoCorrectFormVaraibles.WriteAlbumOnDescription) metadataToSave.PersonalDescription = metadataToSave.PersonalAlbum;
                #endregion

                foreach (string keyword in autoCorrectFormVaraibles.Keywords)
                {
                    metadataToSave.PersonalKeywordTagsAddIfNotExists(new KeywordTag(keyword), false);
                }
            }
        }
        #endregion
    }

    class AutoCorrect
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region DateTaken
        [JsonProperty("UseSmartDate")]
        public bool UseSmartDate { get; set; } = true;
        #endregion

        #region DateTaken
        [JsonProperty("UpdateDateTaken")]
        public bool UpdateDateTaken { get; set; } = true;
        
        [JsonProperty("UpdateDateTakenWithFirstInPrioity")]
        public bool UpdateDateTakenWithFirstInPrioity { get; set; } = false;

        [JsonProperty("DateTakenPriority")]
        public List<DateTimeSources> DateTakenPriority { get; set; } = new List<DateTimeSources>();
        #endregion

        #region GPS Location and Date Time

        [JsonProperty("LocationTimeZoneGuessHours")]
        public int LocationTimeZoneGuessHours { get; set; } = 24; //24 hours

        [JsonProperty("LocationFindMinutes")]
        public int LocationFindMinutes { get; set; } = 5; //Five minutes

        [JsonProperty("LocationFindMinutesNearByMedia")]
        public int LocationFindMinutesNearByMedia { get; set; } = 60; //60 minutes


        [JsonProperty("UpdateGPSLocation")]
        public bool UpdateGPSLocation { get; set; } = true;
        
        [JsonProperty("UpdateGPSLocationNearByMedia")]
        public bool UpdateGPSLocationNearByMedia { get; set; } = true;

        [JsonProperty("UpdateGPSDateTime")]
        public bool UpdateGPSDateTime { get; set; } = true;

        #endregion

        #region Keywords
        [JsonProperty("UseKeywordsFromWindowsLivePhotoGallery")]
        public bool UseKeywordsFromWindowsLivePhotoGallery { get; set; } = true;
        [JsonProperty("UseKeywordsFromMicrosoftPhotos")]
        public bool UseKeywordsFromMicrosoftPhotos { get; set; } = true;
        [JsonProperty("KeywordTagConfidenceLevel")]
        public double KeywordTagConfidenceLevel { get; set; } = 0.9;

        [JsonProperty("UseKeywordsFromWebScraping")]
        public bool UseKeywordsFromWebScraping { get; set; } = true;

        [JsonProperty("UseAutoKeywords")]
        public bool UseAutoKeywords { get; set; } = true;

        [JsonProperty("BackupFileCreatedBeforeUpdate")]
        public bool BackupFileCreatedBeforeUpdate { get; set; } = true;
        [JsonProperty("BackupFileCreatedAfterUpdate")]
        public bool BackupFileCreatedAfterUpdate { get; set; } = true;

        [JsonProperty("BackupDateTakenBeforeUpdate")]
        public bool BackupDateTakenBeforeUpdate { get; set; } = true;
        [JsonProperty("BackupDateTakenAfterUpdate")]
        public bool BackupDateTakenAfterUpdate { get; set; } = true;

        [JsonProperty("BackupGPGDateTimeUTCBeforeUpdate")]
        public bool BackupGPGDateTimeUTCBeforeUpdate { get; set; } = true;
        [JsonProperty("BackupGPGDateTimeUTCAfterUpdate")]
        public bool BackupGPGDateTimeUTCAfterUpdate { get; set; } = true;

        [JsonProperty("BackupRegionFaceNames")] 
        public bool BackupRegionFaceNames { get; set; } = true;

        [JsonProperty("BackupLocationName")]
        public bool BackupLocationName { get; set; } = true;        
        [JsonProperty("BackupLocationCity")]
        public bool BackupLocationCity { get; set; } = true;        
        [JsonProperty("BackupLocationState")]
        public bool BackupLocationState { get; set; } = true;       
        [JsonProperty("BackupLocationCountry")]
        public bool BackupLocationCountry { get; set; } = true;
        #endregion

        #region Face region
        [JsonProperty("UseFaceRegionFromWindowsLivePhotoGallery")]
        public bool UseFaceRegionFromWindowsLivePhotoGallery { get; set; } = true;
        [JsonProperty("UseFaceRegionFromMicrosoftPhotos")]
        public bool UseFaceRegionFromMicrosoftPhotos { get; set; } = true;
        [JsonProperty("UseFaceRegionFromWebScraping")]
        public bool UseFaceRegionFromWebScraping { get; set; } = true;
        #endregion

        #region Title
        [JsonProperty("UpdateTitle")]
        public bool UpdateTitle { get; set; } = true;
        [JsonProperty("UpdateTitleWithFirstInPrioity")]
        public bool UpdateTitleWithFirstInPrioity { get; set; } = false;
        [JsonProperty("TitlePriority")]
        public List<MetadataBrokerType> TitlePriority { get; set; } = new List<MetadataBrokerType>();
        #endregion 

        #region Album
        [JsonProperty("UpdateAlbum")]
        public bool UpdateAlbum { get; set; } = true;
        [JsonProperty("UpdateAlbumWithFirstInPrioity")]
        public bool UpdateAlbumWithFirstInPrioity { get; set; } = false;
        [JsonProperty("AlbumPriority")]
        public List<MetadataBrokerType> AlbumPriority { get; set; } = new List<MetadataBrokerType>();
        #endregion

        #region Description 
        [JsonProperty("UpdateDescription")]
        public bool UpdateDescription { get; set; } = true;
        #endregion 

        #region Comments 
        [JsonProperty("TrackChangesInComments")]
        public bool TrackChangesInComments { get; set; } = false;
        #endregion 

        #region Author
        [JsonProperty("UpdateAuthor")]
        public bool UpdateAuthor { get; set; } = true;
        [JsonProperty("UpdateAuthorOnlyWhenEmpty")]
        public bool UpdateAuthorOnlyWhenEmpty { get; set; } = true;
        #endregion

        #region Location
        [JsonProperty("UpdateLocation")]
        public bool UpdateLocation { get; set; } = true;
        [JsonProperty("UpdateLocationOnlyWhenEmpty")]
        public bool UpdateLocationOnlyWhenEmpty { get; set; } = true;


        [JsonProperty("UpdateLocationName")]
        public bool UpdateLocationName { get; set; } = true;
        [JsonProperty("UpdateLocationCity")]
        public bool UpdateLocationCity { get; set; } = true;
        [JsonProperty("UpdateLocationState")]
        public bool UpdateLocationState { get; set; } = true;
        [JsonProperty("UpdateLocationCountry")]
        public bool UpdateLocationCountry { get; set; } = true;
        #endregion

        #region Rename
        [JsonProperty("RenameVariable")]
        public string RenameVariable { get; set; } = ".\\AutoCorrected\\%Trim%%MediaFileNow_DateTime% %FileNameWithoutDateTime%%Extension%";
        [JsonProperty("RenameAfterAutoCorrect")]
        public bool RenameAfterAutoCorrect { get; set; } = true;

        #endregion

        #region DateTakenFix
        public static void DateTakenFix(ref Metadata metadataCopy)
        {
            if (metadataCopy?.MediaDateTaken == null && metadataCopy?.LocationLatitude != null && metadataCopy?.LocationLongitude != null && metadataCopy?.LocationDateTime != null)
            {
                TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataCopy?.LocationLatitude, (double)metadataCopy?.LocationLongitude);
                if (timeZoneInfo != null)
                {
                    DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(((DateTime)metadataCopy?.LocationDateTime).Ticks, DateTimeKind.Utc), timeZoneInfo);
                    metadataCopy.MediaDateTaken = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Local);
                }
            }
        }
        #endregion

        #region Config De- Serialization
        public string SerializeThis()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static AutoCorrect ConvertConfigValue(string configString)
        {
            AutoCorrect autoCorrect = JsonConvert.DeserializeObject<AutoCorrect>(configString);
            if (autoCorrect == null) autoCorrect = new AutoCorrect();

            //Set defaults
            if (autoCorrect.DateTakenPriority == null) autoCorrect.DateTakenPriority = new List<DateTimeSources>();
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.GPSDateAndTime)) autoCorrect.DateTakenPriority.Add(DateTimeSources.GPSDateAndTime);
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.DateTaken)) autoCorrect.DateTakenPriority.Add(DateTimeSources.DateTaken);
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.SmartDate)) autoCorrect.DateTakenPriority.Add(DateTimeSources.SmartDate);
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.FirstDateFoundInFilename)) autoCorrect.DateTakenPriority.Add(DateTimeSources.FirstDateFoundInFilename);
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.LastDateFoundInFilename)) autoCorrect.DateTakenPriority.Add(DateTimeSources.LastDateFoundInFilename);
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.FileCreateDate)) autoCorrect.DateTakenPriority.Add(DateTimeSources.FileCreateDate);
            if (!autoCorrect.DateTakenPriority.Contains(DateTimeSources.FileModified)) autoCorrect.DateTakenPriority.Add(DateTimeSources.FileModified);

            if (autoCorrect.TitlePriority == null) autoCorrect.TitlePriority = new List<MetadataBrokerType>();
            if (!autoCorrect.TitlePriority.Contains(MetadataBrokerType.ExifTool)) autoCorrect.TitlePriority.Add(MetadataBrokerType.ExifTool);
            if (!autoCorrect.TitlePriority.Contains(MetadataBrokerType.MicrosoftPhotos)) autoCorrect.TitlePriority.Add(MetadataBrokerType.MicrosoftPhotos);
            if (!autoCorrect.TitlePriority.Contains(MetadataBrokerType.WindowsLivePhotoGallery)) autoCorrect.TitlePriority.Add(MetadataBrokerType.WindowsLivePhotoGallery);
            if (!autoCorrect.TitlePriority.Contains(MetadataBrokerType.WebScraping)) autoCorrect.TitlePriority.Add(MetadataBrokerType.WebScraping);

            if (autoCorrect.AlbumPriority == null) autoCorrect.AlbumPriority = new List<MetadataBrokerType>();

            if (!autoCorrect.AlbumPriority.Contains(MetadataBrokerType.ExifTool)) autoCorrect.AlbumPriority.Add(MetadataBrokerType.ExifTool);
            if (!autoCorrect.AlbumPriority.Contains(MetadataBrokerType.MicrosoftPhotos)) autoCorrect.AlbumPriority.Add(MetadataBrokerType.MicrosoftPhotos);
            if (!autoCorrect.AlbumPriority.Contains(MetadataBrokerType.WebScraping)) autoCorrect.AlbumPriority.Add(MetadataBrokerType.WebScraping);
            if (!autoCorrect.AlbumPriority.Contains(MetadataBrokerType.FileSystem)) autoCorrect.AlbumPriority.Add(MetadataBrokerType.FileSystem);
            return autoCorrect;
        }
        #endregion

        #region FixMetadata - CompatibilityCheckMetadata
        public static void CompatibilityCheckMetadata(ref Metadata metadata, bool fixDateTaken)
        {
            if (metadata == null) return;
            
            try
            {
                Metadata metadataCopy = new Metadata(metadata);
                bool changesFound;
                do
                {
                    changesFound = false;

                    foreach (KeywordTag keywordTag in metadataCopy.PersonalKeywordTags) //Read orginal and change the copy
                    {
                        #region Check Metadata.VariablePersonalKeywordsList() - special escape char used as splitter
                        char splitChar = ';';
                        if (!string.IsNullOrWhiteSpace(keywordTag.Keyword) && keywordTag.Keyword.Contains(splitChar.ToString()))
                        {
                            string[] keywords = keywordTag.Keyword.Split(splitChar);
                            foreach (string keyword in keywords)
                            {
                                KeywordTag newKeywordTag = new KeywordTag(keyword.Trim(), keywordTag.Confidence);
                                if (metadata.PersonalKeywordTagsAddIfNotExists(newKeywordTag)) changesFound = true;
                            }
                        }
                        #endregion

                        #region Check
                        splitChar = ',';
                        if (!string.IsNullOrWhiteSpace(keywordTag.Keyword) && keywordTag.Keyword.Contains(splitChar.ToString()))
                        {
                            string[] keywords = keywordTag.Keyword.Split(splitChar);
                            foreach (string keyword in keywords)
                            {
                                KeywordTag newKeywordTag = new KeywordTag(keyword.Trim(), keywordTag.Confidence);
                                if (metadata.PersonalKeywordTagsAddIfNotExists(newKeywordTag)) changesFound = true;
                            }
                        }
                        #endregion

                        #region Check Metadata.VariableKeywordCategories() - special escape char used as splitter
                        splitChar = '/';
                        if (!string.IsNullOrWhiteSpace(keywordTag.Keyword) && keywordTag.Keyword.Contains(splitChar.ToString()))
                        {
                            string mergedTrimmedKeywords = "";
                            string[] keywords = keywordTag.Keyword.Split(splitChar);
                            foreach (string keyword in keywords)
                            {
                                mergedTrimmedKeywords += (string.IsNullOrWhiteSpace(mergedTrimmedKeywords) ? "" : "/") + keyword.Trim();
                                KeywordTag newKeywordTag = new KeywordTag(keyword.Trim(), keywordTag.Confidence);
                                if (metadata.PersonalKeywordTagsAddIfNotExists(newKeywordTag)) changesFound = true;
                            }
                            KeywordTag mergedTrimmedKeywordTag = new KeywordTag(mergedTrimmedKeywords.Trim(), keywordTag.Confidence);
                            if (metadata.PersonalKeywordTagsAddIfNotExists(mergedTrimmedKeywordTag)) changesFound = true;
                        }
                        #endregion
                    }
                } while (changesFound);

                do
                {
                    changesFound = false;
                    for (int indexKeyword = 0; indexKeyword < metadataCopy.PersonalKeywordTags.Count; indexKeyword++)
                    {
                        if (metadataCopy.PersonalKeywordTags[indexKeyword].Keyword != null && 
                            metadataCopy.PersonalKeywordTags[indexKeyword].Keyword.Trim() != metadataCopy.PersonalKeywordTags[indexKeyword].Keyword)
                        {
                            KeywordTag trimmedKeywordTag = new KeywordTag(metadataCopy.PersonalKeywordTags[indexKeyword].Keyword.Trim(), metadataCopy.PersonalKeywordTags[indexKeyword].Confidence);
                            metadataCopy.PersonalKeywordTags.RemoveAt(indexKeyword);
                            metadataCopy.PersonalKeywordTags.Insert(indexKeyword, trimmedKeywordTag);
                            changesFound = true;
                            break;
                        }
                    }
                } while (changesFound);

                if (fixDateTaken) DateTakenFix(ref metadata);
            }
            catch { }
        }
        #endregion

        #region FixMetadata - RunAlgorithmReturnCopy
        public Metadata RunAlgorithmReturnCopy(Metadata metadata,
            MetadataDatabaseCache metadataAndCacheMetadataExiftool,
            MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos,
            MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery,
            CameraOwnersDatabaseCache cameraOwnersDatabaseCache,
            LocationNameDatabaseAndLookUpCache locationNameLookUpCache,
            GoogleLocationHistoryDatabaseCache databaseGoogleLocationHistory,
            float locationAccuracyLatitude,
            float locationAccuracyLongitude,
            int writeCreatedDateAndTimeAttributeTimeIntervalAccepted,
            List<AutoKeywordConvertion> autoKeywordConvertions,
            string allowedDateFormats
            )
        {
            if (metadata == null)
            {
                Logger.Warn("FixAndSave ended: metadata is null");
                return null; //DEBUG Why NULL - I manage to reproduce, select lot of files, select AutoCorrect many, many times.
            }

            Metadata metadataCopy = new Metadata(metadata); //Make a copy

            #region MicrosoftPhotos
            FileEntryBroker fileEntryBrokerMicrosoftPhotos = new FileEntryBroker(metadataCopy.FileEntry, MetadataBrokerType.MicrosoftPhotos);
            Metadata metadataMicrosoftPhotos = databaseAndCacheMetadataMicrosoftPhotos.ReadMetadataFromCacheOrDatabase(fileEntryBrokerMicrosoftPhotos);
            Metadata metadataMicrosoftPhotosCopy = metadataMicrosoftPhotos == null ? null : new Metadata(metadataMicrosoftPhotos);
            #endregion

            #region WindowsLivePhotoGallery
            FileEntryBroker fileEntryBrokerWindowsLivePhotoGallery = new FileEntryBroker(metadataCopy.FileEntry, MetadataBrokerType.WindowsLivePhotoGallery);
            Metadata metadataWindowsLivePhotoGallery = databaseAndCacheMetadataWindowsLivePhotoGallery.ReadMetadataFromCacheOrDatabase(fileEntryBrokerWindowsLivePhotoGallery);
            Metadata metadataWindowsLivePhotoGalleryCopy = metadataWindowsLivePhotoGallery == null ? null : new Metadata(metadataWindowsLivePhotoGallery);
            #endregion

            #region WebScraping
            FileEntryBroker fileEntryBrokerWebScraping = new FileEntryBroker(metadataCopy.FileEntry, MetadataBrokerType.WebScraping);
            Metadata metadataWebScraping = metadataAndCacheMetadataExiftool.ReadWebScraperMetadataFromCacheOrDatabase(fileEntryBrokerWebScraping);
            Metadata metadataWebScrapingCopy = metadataWebScraping == null ? null : new Metadata(metadataWebScraping);
            #endregion

            #region Find best guess on GPS Location Latitude Longitude
            if (UpdateGPSLocation || UpdateGPSLocationNearByMedia)
            {
                //Don't have GPS locations, try Guess GPS location bases on what exist in Location History

                //If GPS Locations missing!
                //1. Check if GPS DateTime exist, if not then use DateTaken
                //2. Try find location in camera owner's history, ±24 hours
                //3. If location's found, find time zone
                //4. Adjust DateTaken with found time zone
                //5. Find new locations in camra owner's hirstory. ±1 hour
                if (metadataCopy?.LocationLatitude == null || metadataCopy?.LocationLongitude == null)
                {
                    DateTime? dateTimeUTC = null;

                    #region Estimate Location Latitude Longitude (From Location Coordinates History Database)
                    string cameraOwner = cameraOwnersDatabaseCache.GetOwenerForCameraMakeModel(metadataCopy?.CameraMake, metadataCopy?.CameraModel);                   
                    if (!string.IsNullOrEmpty(cameraOwner))
                    {
                        Logger.Debug("FixAndSave: -Try Find or Guess UTC time-");
                        if (metadataCopy?.LocationDateTime != null) //If has UTC time
                        {
                            #region dateTimeUTC = LocationDateTime
                            dateTimeUTC = new DateTime(((DateTime)metadataCopy?.LocationDateTime).Ticks, DateTimeKind.Utc);
                            Logger.Debug("FixAndSave: Have UTC time, dateTimeUTC = LocationDateTime: " + dateTimeUTC.ToString());
                            #endregion
                        }
                        else if (metadataCopy?.MediaDateTaken != null) //Don't have UTC time, need try to Guess
                        {
                            Logger.Debug("FixAndSave: Don't have UTC time, need try to Guess");

                            #region Find NearBy Location using "MediaTaken DateTime" even with wrong TimeZone however (Can be ±24 hours wrong)
                            DateTime mediaDateTimeUnspecified = new DateTime(((DateTime)metadataCopy?.MediaDateTaken).Ticks, DateTimeKind.Unspecified);
                            Metadata metadataLocationTimeZone = databaseGoogleLocationHistory.FindLocationBasedOnTime(cameraOwner, mediaDateTimeUnspecified, LocationTimeZoneGuessHours * 60 * 60);
                            #endregion

                            #region Get TimeZone when found location
                            if (metadataLocationTimeZone != null && metadataLocationTimeZone?.LocationLatitude != null && metadataLocationTimeZone?.LocationLongitude != null)
                            {
                                TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataLocationTimeZone?.LocationLatitude, (double)metadataLocationTimeZone?.LocationLongitude);
                                if (timeZoneInfo != null)
                                {
                                    dateTimeUTC = TimeZoneInfo.ConvertTimeToUtc(mediaDateTimeUnspecified, timeZoneInfo);
                                    Logger.Debug("FixAndSave: Found a location (±24 hours), estimated dateTimeUTC: " + dateTimeUTC.ToString() + "for camera owner: " + cameraOwner);
                                }
                                else 
                                    Logger.Debug("FixAndSave: No time zone found for, invalid location");
                            }
                            else
                            {
                                Logger.Debug("FixAndSave: No location found (±24 hours) for camera owner: " + cameraOwner);
                            }
                            #endregion
                        }

                        #region Find best GPS location
                        if (dateTimeUTC != null) //UTC time found, guess location based on UTC time
                        {
                            Metadata metadataLocationBasedOnBestGuess = databaseGoogleLocationHistory.FindLocationBasedOnTime(
                                cameraOwner, dateTimeUTC, 60 * LocationFindMinutes);

                            //If location found, updated metadata with found location
                            if (metadataLocationBasedOnBestGuess != null)
                            {
                                //If allow update location, then updated metadata with found location
                                metadataCopy.LocationLatitude = metadataLocationBasedOnBestGuess.LocationLatitude;
                                metadataCopy.LocationLongitude = metadataLocationBasedOnBestGuess.LocationLongitude;

                                Logger.Debug("FixAndSave: Found a location (±" + LocationFindMinutes + " minutes), estimated dateTimeUTC: " + dateTimeUTC.ToString() + "for camera owner: " + cameraOwner);
                            } 
                            else
                            {
                                Logger.Debug("FixAndSave: No location found (±" + LocationFindMinutes + " minutes) for camera owner: " + cameraOwner);
                            }
                        }
                        #endregion
                    } else
                    {
                        Logger.Debug("FixAndSave: cameraOwner not found - can't search Location history");
                    }
                    #endregion

                    #region #region Estimate Location Latitude Longitude (From Location Near By Photos)
                    if (UpdateGPSLocationNearByMedia)
                    {
                        if (metadataCopy?.LocationLatitude == null || metadataCopy?.LocationLongitude == null)
                        {
                            Metadata metadataLocationBasedOnBestGuess = databaseGoogleLocationHistory.FindBestLocationBasedOtherMediaFiles(
                                dateTimeUTC, metadataCopy?.MediaDateTaken, 
                                UseSmartDate ? metadataCopy?.FileSmartDate(allowedDateFormats) : metadataCopy?.FileDate,
                                60 * LocationFindMinutesNearByMedia);

                            if (metadataLocationBasedOnBestGuess != null && metadataLocationBasedOnBestGuess.LocationLatitude != null && metadataLocationBasedOnBestGuess.LocationLongitude != null)
                            {
                                Logger.Debug("FixAndSave: Location found (±1 hours) using nearby mediafiles");

                                //If allow update location, then updated metadata with found location
                                //metadataCopy.LocationDateTime = dateTimeUTC;
                                metadataCopy.LocationLatitude = metadataLocationBasedOnBestGuess.LocationLatitude;
                                metadataCopy.LocationLongitude = metadataLocationBasedOnBestGuess.LocationLongitude;
                            } else
                            {
                                Logger.Debug("FixAndSave: No location found (±" + LocationFindMinutesNearByMedia + " minutes) using nearby mediafiles");
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region MediaDateTaken DateTime
            string sourceMediaDateTaken = null;
            if (UpdateDateTaken)
            {
                // Find first No empty date
                DateTime? newDateTime = null;
                foreach (DateTimeSources dateTimeSource in DateTakenPriority)
                {
                    switch (dateTimeSource)
                    {
                        case DateTimeSources.SmartDate:
                            newDateTime = metadataCopy?.FileSmartDate(allowedDateFormats);
                            sourceMediaDateTaken = "Smart Date";
                            break;
                        case DateTimeSources.DateTaken:
                            newDateTime = metadataCopy?.MediaDateTaken;
                            if (newDateTime != null) Logger.Debug("FixAndSave: DateAndTime Digitized was found at DateTimeSources.DateTaken: " + newDateTime.ToString());
                            sourceMediaDateTaken = "Media Date Taken";
                            break;
                        case DateTimeSources.GPSDateAndTime:
                            if (metadataCopy?.LocationLatitude != null && metadataCopy?.LocationLongitude != null && metadataCopy?.LocationDateTime != null)
                            {
                                TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataCopy?.LocationLatitude, (double)metadataCopy?.LocationLongitude);
                                if (timeZoneInfo != null)
                                {
                                    DateTime? dateTimeGPSLocal = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(((DateTime)metadataCopy?.LocationDateTime).Ticks, DateTimeKind.Utc), timeZoneInfo);
                                    newDateTime = dateTimeGPSLocal;
                                    if (newDateTime != null) Logger.Debug("FixAndSave: DateAndTime Digitized was found at DateTimeSources.GPSDateAndTime: " + newDateTime.ToString());
                                    sourceMediaDateTaken = "GPS Date And Time";
                                }
                            }                           
                            break;
                        case DateTimeSources.FirstDateFoundInFilename:
                            FileDateTimeReader fileDateTimeReader1 = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            List<DateTime> dates1 = fileDateTimeReader1.ListAllDateTimes(Path.GetFileNameWithoutExtension(metadataCopy?.FileName));
                            if (dates1.Count > 0)
                            {
                                newDateTime = dates1[0];
                                sourceMediaDateTaken = "1st Date in Filename";
                            }
                            break;
                        case DateTimeSources.LastDateFoundInFilename:
                            FileDateTimeReader fileDateTimeReader2 = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);
                            List<DateTime> dates2 = fileDateTimeReader2.ListAllDateTimes(Path.GetFileNameWithoutExtension(metadataCopy?.FileName));
                            if (dates2.Count > 0)
                            {
                                newDateTime = dates2[dates2.Count - 1];
                                sourceMediaDateTaken = "Last Date in Filename";
                            }
                            break;
                        case DateTimeSources.FileCreateDate:
                            newDateTime = metadataCopy?.FileDateCreated;
                            sourceMediaDateTaken = "File Date Created";
                            break;
                        case DateTimeSources.FileModified:
                            newDateTime = metadataCopy?.FileDateModified;
                            sourceMediaDateTaken = "File Date Modified";
                            break;
                    }
                    if (UpdateTitleWithFirstInPrioity) break;
                    if (newDateTime != null) break;
                }
                if (newDateTime != null)
                {
                    metadataCopy.MediaDateTaken = newDateTime;                    
                    Logger.Debug("FixAndSave: New DateAndTime Digitized was replaced: " + metadataCopy.MediaDateTaken.ToString() + " from source: " + sourceMediaDateTaken);
                }
                else
                {
                    Logger.Debug("FixAndSave: No new DateAndTime Digitized was found");
                }

            }
            #endregion

            #region UpdateGPSDateTime (only if location coordinates exists)
            if (UpdateGPSDateTime)
            {
                if (metadataCopy?.LocationDateTime == null && metadataCopy?.MediaDateTaken != null && metadataCopy?.LocationLatitude != null && metadataCopy?.LocationLongitude != null)
                {
                    DateTime mediaDateTimeUnspecified = new DateTime(((DateTime)metadataCopy?.MediaDateTaken).Ticks, DateTimeKind.Unspecified);
                    TimeZoneInfo timeZoneInfo = TimeZoneLibrary.GetTimeZoneInfoOnGeoLocation((double)metadataCopy?.LocationLatitude, (double)metadataCopy?.LocationLongitude);
                    if (timeZoneInfo != null) metadataCopy.LocationDateTime = TimeZoneInfo.ConvertTimeToUtc(mediaDateTimeUnspecified, timeZoneInfo);
                    if (metadataCopy?.LocationDateTime != null) Logger.Debug("FixAndSave: Location date time updated. Location was known." + metadataCopy.LocationDateTime.ToString());
                }
            }
            #endregion

            #region Location name, city, state, country
            if (UpdateLocation && metadataCopy?.LocationLatitude != null && metadataCopy?.LocationLongitude != null)
            {
                if (!UpdateLocationOnlyWhenEmpty ||
                    (UpdateLocationOnlyWhenEmpty && 
                    string.IsNullOrWhiteSpace(metadataCopy?.LocationName) &&
                    string.IsNullOrWhiteSpace(metadataCopy?.LocationState) &&
                    string.IsNullOrWhiteSpace(metadataCopy?.LocationCity) &&
                    string.IsNullOrWhiteSpace(metadataCopy?.LocationCountry))
                   )
                {
                    LocationCoordinateAndDescription locationData = locationNameLookUpCache.AddressLookupAndReverseGeocoder(
                        metadataCopy?.LocationCoordinate, locationAccuracyLatitude, locationAccuracyLongitude, onlyFromCache: false, canReverseGeocoder: true,
                        metadataLocationDescription: null, forceReloadUsingReverseGeocoder: false);
                    
                    if (locationData != null)
                    {
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadataCopy?.LocationName))
                            if (UpdateLocationName) metadataCopy.LocationName = locationData.Description.Name;
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadataCopy?.LocationState))
                            if (UpdateLocationState) metadataCopy.LocationState = locationData.Description.Region;
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadataCopy?.LocationCity))
                            if (UpdateLocationCity) metadataCopy.LocationCity = locationData.Description.City;
                        if (!UpdateLocationOnlyWhenEmpty || string.IsNullOrWhiteSpace(metadataCopy?.LocationCountry))
                            if (UpdateLocationCountry) metadataCopy.LocationCountry = locationData.Description.Country;
                    }
                }
            }
            #endregion

            #region Face region
            //If Exiftool Metadata missing names, find name in other sources
            if (UseFaceRegionFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null) metadataWindowsLivePhotoGalleryCopy.PersonalRegionSetNamelessRegions(metadataCopy.PersonalRegionList);
            if (UseFaceRegionFromMicrosoftPhotos && metadataMicrosoftPhotosCopy != null) metadataMicrosoftPhotosCopy.PersonalRegionSetNamelessRegions(metadataCopy.PersonalRegionList);

            //Remove doubles and add names where missing, (only chnage/update the metadata copy, don't change metadata in buffer).
            if (UseFaceRegionFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null) metadataWindowsLivePhotoGalleryCopy.PersonalRegionRemoveNamelessDoubleRegions(metadataCopy.PersonalRegionList);
            if (UseFaceRegionFromMicrosoftPhotos && metadataMicrosoftPhotosCopy != null) metadataMicrosoftPhotosCopy.PersonalRegionRemoveNamelessDoubleRegions(metadataCopy.PersonalRegionList);

            if (UseFaceRegionFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataWindowsLivePhotoGalleryCopy.PersonalRegionList);
            if (UseFaceRegionFromMicrosoftPhotos && metadataMicrosoftPhotosCopy != null) metadataCopy.PersonalRegionSetNamelessRegions(metadataMicrosoftPhotosCopy.PersonalRegionList);


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

            if (UseFaceRegionFromMicrosoftPhotos && metadataMicrosoftPhotosCopy != null)
            {
                foreach (RegionStructure regionStructure in metadataMicrosoftPhotosCopy.PersonalRegionList) metadataCopy.PersonalRegionListAddIfNameNotExists(regionStructure);
                
            }

            if (UseFaceRegionFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null)
            {
                foreach (RegionStructure regionStructure in metadataWindowsLivePhotoGalleryCopy.PersonalRegionList) metadataCopy.PersonalRegionListAddIfNameNotExists(regionStructure);
            }

            if (UseFaceRegionFromWebScraping && metadataWebScrapingCopy != null)
            {
                foreach (RegionStructure regionStructure in metadataWebScrapingCopy.PersonalRegionList) metadataCopy.PersonalRegionListAddIfNameNotExists(regionStructure);
                
            }
            #endregion

            #region Keywords
            if (UseKeywordsFromMicrosoftPhotos && metadataMicrosoftPhotosCopy != null)
            {
                foreach (KeywordTag keywordTag in metadataMicrosoftPhotosCopy.PersonalKeywordTags)
                {
                    if (!string.IsNullOrWhiteSpace(keywordTag.Keyword) && keywordTag.Confidence >= KeywordTagConfidenceLevel) 
                        metadataCopy.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }

            if (UseKeywordsFromWindowsLivePhotoGallery && metadataWindowsLivePhotoGalleryCopy != null)
            {
                foreach (KeywordTag keywordTag in metadataWindowsLivePhotoGalleryCopy.PersonalKeywordTags)
                {
                    if (!string.IsNullOrWhiteSpace(keywordTag.Keyword)) // && keywordTag.Confidence >= KeywordTagConfidenceLevel)
                        metadataCopy.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }

            if (UseKeywordsFromWebScraping && metadataWebScrapingCopy != null)
            {
                foreach (KeywordTag keywordTag in metadataWebScrapingCopy.PersonalKeywordTags)
                {
                    if (!string.IsNullOrWhiteSpace(keywordTag.Keyword)) // && keywordTag.Confidence >= KeywordTagConfidenceLevel)
                        metadataCopy.PersonalKeywordTagsAddIfNotExists(keywordTag);
                }
            }
            #endregion

            #region Backup/TrackChanges In Comments

            #region Convert FileDateCreated to UTC
            DateTime? newDateTimeFileCreated = metadata.FileDateCreated;            
            if (metadata.TryParseDateTakenToUtc(out DateTime? dateTakenWithOffset))
            {
                if (metadata?.FileDateCreated != null &&
                    metadata?.MediaDateTaken != null &&
                    metadata?.MediaDateTaken < DateTime.Now &&
                    Math.Abs(((DateTime)dateTakenWithOffset.Value.ToUniversalTime() - (DateTime)metadata?.FileDateCreated.Value.ToUniversalTime()).TotalSeconds) > writeCreatedDateAndTimeAttributeTimeIntervalAccepted) //No need to change
                {
                    newDateTimeFileCreated = (DateTime)dateTakenWithOffset;
                }
            }
            #endregion

            if (TrackChangesInComments)
            {
                if (newDateTimeFileCreated != metadataCopy?.FileDateCreated || 
                    metadata?.MediaDateTaken != metadataCopy?.MediaDateTaken ||
                    metadata?.LocationDateTime != metadataCopy?.LocationDateTime)
                {
                    metadataCopy.PersonalComments = 
                        "Source: " + sourceMediaDateTaken + " " +
                        "Date: " +
                        TimeZoneLibrary.ToStringW3CDTF_UTC(metadataCopy?.FileSmartDate(allowedDateFormats)) + " vs " +
                        TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.FileSmartDate(allowedDateFormats)) + " " +
                        "Taken: " +
                        (metadata?.MediaDateTaken == null ? "(empty)" : TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.MediaDateTaken)) + " vs " +
                        (metadataCopy?.MediaDateTaken == null ? "(empty)" : TimeZoneLibrary.ToStringW3CDTF_UTC(metadataCopy?.MediaDateTaken)) + " " +
                        "GPS: " +
                        (metadata?.LocationDateTime == null ? "(empty)" : TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.LocationDateTime)) + " vs " +
                        (metadataCopy?.LocationDateTime == null ? "(empty)" : TimeZoneLibrary.ToStringW3CDTF_UTC(metadataCopy?.LocationDateTime));
                }
            }
            #endregion

            #region Backup/TrackChanges In keywords
            int numberOfSecondsAccepted = 60;
            if (BackupFileCreatedBeforeUpdate && metadata?.FileDateCreated != null && 
                !TimeZoneLibrary.IsDateTimeEqualWithinOneSecond (metadata?.FileDateCreated, metadataCopy?.FileDateCreated, numberOfSecondsAccepted))
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("File created: " + TimeZoneLibrary.ToStringSortable(metadata?.FileDateCreated) + " before update"));

            if (BackupDateTakenBeforeUpdate && metadata?.MediaDateTaken != null &&
                !TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(metadata?.MediaDateTaken, metadataCopy?.MediaDateTaken, numberOfSecondsAccepted))
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("Media taken: " + TimeZoneLibrary.ToStringSortable(metadata?.MediaDateTaken) + " before update"));

            if (BackupGPGDateTimeUTCBeforeUpdate && metadata?.LocationDateTime != null &&
                !TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(metadata?.LocationDateTime, metadataCopy?.LocationDateTime, numberOfSecondsAccepted))
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("UTC time: " + TimeZoneLibrary.ToStringW3CDTF_UTC(metadata?.LocationDateTime) + " before update"));

            if (BackupFileCreatedAfterUpdate && metadataCopy?.FileDateCreated != null &&
                !TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(metadata?.FileDateCreated, metadataCopy?.FileDateCreated, numberOfSecondsAccepted))
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("File created: " + TimeZoneLibrary.ToStringSortable(metadataCopy?.FileDateCreated) + " after update"));

            if (BackupDateTakenAfterUpdate && metadataCopy?.MediaDateTaken != null &&
                !TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(metadata?.MediaDateTaken, metadataCopy?.MediaDateTaken, numberOfSecondsAccepted))
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("Media taken: " + TimeZoneLibrary.ToStringSortable(metadataCopy?.MediaDateTaken) + " after update"));

            if (BackupGPGDateTimeUTCAfterUpdate && metadataCopy?.LocationDateTime != null &&
                !TimeZoneLibrary.IsDateTimeEqualWithinOneSecond(metadata?.LocationDateTime, metadataCopy?.LocationDateTime, numberOfSecondsAccepted))
                metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag("UTC time: " + TimeZoneLibrary.ToStringW3CDTF_UTC(metadataCopy?.LocationDateTime) + " after update"));
            #endregion

            #region Backup Region Face Names
            if (BackupRegionFaceNames)
            {
                foreach (RegionStructure regionStructure in metadataCopy?.PersonalRegionList)
                {
                    if (!string.IsNullOrWhiteSpace(regionStructure.Name))
                    {
                        metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(regionStructure.Name), false);
                        Logger.Debug("AutoCorrect: " + regionStructure.Name);
                    } 
                }
            }
            #endregion

            #region Backup location Name, City, State/Region, Country
            if (BackupLocationName && !string.IsNullOrEmpty(metadataCopy?.LocationName)) metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadataCopy?.LocationName));
            if (BackupLocationCity && !string.IsNullOrEmpty(metadataCopy?.LocationCity)) metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadataCopy?.LocationCity));
            if (BackupLocationState && !string.IsNullOrEmpty(metadataCopy?.LocationState)) metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadataCopy?.LocationState));
            if (BackupLocationCountry && !string.IsNullOrEmpty(metadataCopy?.LocationCountry)) metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(metadataCopy?.LocationCountry));
            #endregion

            #region Title
            if (UpdateTitle)
            {
                // Find first No empty string
                string newTitle = null;
                foreach (MetadataBrokerType metadataBrokerType in TitlePriority)
                {
                    switch (metadataBrokerType)
                    {
                        case MetadataBrokerType.ExifTool:
                            newTitle = (!string.IsNullOrEmpty(metadataCopy?.PersonalTitle) ? metadataCopy?.PersonalTitle : newTitle);
                            break;
                        case MetadataBrokerType.MicrosoftPhotos:
                            newTitle = (!string.IsNullOrEmpty(metadataMicrosoftPhotos?.PersonalTitle) ? metadataMicrosoftPhotos?.PersonalTitle : newTitle);
                            break;
                        case MetadataBrokerType.WindowsLivePhotoGallery:
                            newTitle = (!string.IsNullOrEmpty(metadataWindowsLivePhotoGallery?.PersonalTitle) ? metadataWindowsLivePhotoGallery?.PersonalTitle : newTitle);
                            break;
                        case MetadataBrokerType.WebScraping:
                            newTitle = (!string.IsNullOrEmpty(metadataWebScraping?.PersonalTitle) ? metadataWebScraping?.PersonalTitle : newTitle);
                            break;
                    }
                    if (UpdateTitleWithFirstInPrioity) break;
                    if (!string.IsNullOrWhiteSpace(newTitle)) break;
                }
                metadataCopy.PersonalTitle = newTitle;
            }
            #endregion 

            #region Album
            if (UpdateAlbum)
            {
                // Find first No empty string
                string newAlbum = null;
                foreach (MetadataBrokerType metadataBrokerType in AlbumPriority)
                {
                    switch (metadataBrokerType)
                    {
                        case MetadataBrokerType.ExifTool:
                            newAlbum = (!string.IsNullOrEmpty(metadataCopy?.PersonalAlbum) ? metadataCopy?.PersonalAlbum : newAlbum);
                            break;
                        case MetadataBrokerType.MicrosoftPhotos:
                            newAlbum = (!string.IsNullOrEmpty(metadataMicrosoftPhotos?.PersonalAlbum) ? metadataMicrosoftPhotos?.PersonalAlbum : newAlbum);
                            break;
                        case MetadataBrokerType.FileSystem:
                            try
                            {
                                newAlbum = new DirectoryInfo(metadataCopy.FileDirectory).Name;
                            }
                            catch 
                            {
                                newAlbum = metadataCopy.FileDirectory;
                            }
                            break;
                        case MetadataBrokerType.WebScraping:
                            newAlbum = (!string.IsNullOrEmpty(metadataWebScraping?.PersonalAlbum) ? metadataWebScraping?.PersonalAlbum : newAlbum);
                            break;
                    }
                    if (UpdateAlbumWithFirstInPrioity) break;
                    if (!string.IsNullOrWhiteSpace(newAlbum)) break;
                }
                metadataCopy.PersonalAlbum = newAlbum;
            }
            #endregion

            #region Description
            if (UpdateDescription)
            {
                Logger.Debug("FixAndSave: Set Description as Album: " + (metadataCopy?.PersonalAlbum == null ? "null" : metadataCopy?.PersonalAlbum));
                metadataCopy.PersonalDescription = metadataCopy.PersonalAlbum;
            }
            #endregion

            #region Author
            if (UpdateAuthor)
            {
                if (!UpdateAuthorOnlyWhenEmpty || (UpdateAuthorOnlyWhenEmpty && string.IsNullOrWhiteSpace(metadataCopy?.PersonalAuthor)))
                {
                    string author = cameraOwnersDatabaseCache.GetOwenerForCameraMakeModel(metadataCopy?.CameraMake, metadataCopy?.CameraModel);
                    if (!string.IsNullOrWhiteSpace(author)) metadataCopy.PersonalAuthor = author;
                }
            }
            #endregion

            #region AutoKeywords
            if (UseAutoKeywords && metadataCopy != null)
            {
                List<string> newKeywords = AutoKeywordHandler.NewKeywords(autoKeywordConvertions, metadataCopy.LocationName, metadataCopy.PersonalTitle, 
                    metadataCopy.PersonalAlbum, metadataCopy.PersonalDescription, metadataCopy.PersonalComments, metadataCopy.PersonalKeywordTags);
                foreach (string keyword in newKeywords)
                {
                    metadataCopy.PersonalKeywordTagsAddIfNotExists(new KeywordTag(keyword), false);
                }
            }
            #endregion

            return metadataCopy;
        }
        #endregion 
    }
}
