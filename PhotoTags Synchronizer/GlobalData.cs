using DataGridViewGeneric;
using MetadataLibrary;
using System.Collections.Generic;

namespace PhotoTagsSynchronizer
{
    public static class GlobalData
    {
        public static readonly object populateSelectedLock = new object(); //Avoid gridview to update while updateing
        public static readonly object metadataUpdateLock = new object();

        public static int ProcessCounterReadProperties = 0;

        #region 
        public static HashSet<FileEntry> SerachFilterResult { get; set; }
        public static bool SearchFolder { get; set; } = true;
        public static bool lastReadFolderWasRecursive { get; set; } = false;
        #endregion

        public static bool DoNotTrigger_ImageListView_ItemUpdate { get; set; } = false;
        public static bool DoNotTrigger_ImageListView_SelectionChanged { get; set; } = false;
        public static bool DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect { get; set; } = false; //To avoid Populate twice
        public static bool DoNotTrigger_TreeViewFilter_BeforeAndAfterCheck { get; set; } = false; //To avoid double click / start process twice

        
        #region Avoid double click / start process twice
        public static bool IsPerformingAButtonAction { get; set; } = false;
        public static bool IsDragAndDropActive { get; set; } = false; //To avoid enter rutine when in process        
        public static bool IsPopulatingImageListViewFromFolderOrDatabaseList { get; set; } = false; //When started twice, stop current, continue with new
        public static bool IsDataGridViewCutPasteDeleteFindReplaceInProgress { get; set; } = false; //To avoid trigger "Changes Value Cell"
        #endregion

        #region Stop onging process, other task requested
        public static bool IsApplicationClosing { get; set; } = false; //Tell ongoing process, the application is closing, just termiate the ongoing process
        public static bool IsStopAndEmptyExiftoolReadQueueRequest { get; set; } = false;
        public static bool IsStopAndEmptyThumbnailQueueRequest { get; set; } = false;
        #endregion


        #region DataGridViewHandler
        public static DataGridViewHandler dataGridViewHandlerTags = null;
        public static DataGridViewHandler dataGridViewHandlerMap = null;
        public static DataGridViewHandler dataGridViewHandlerPeople = null;
        public static DataGridViewHandler dataGridViewHandlerDates = null;
        public static DataGridViewHandler dataGridViewHandlerExiftoolTags = null;
        public static DataGridViewHandler dataGridViewHandlerExiftoolWarning = null;
        public static DataGridViewHandler dataGridViewHandlerProperties = null;
        public static DataGridViewHandler dataGridViewHandlerRename = null;
        public static DataGridViewHandler dataGridViewHandlerConvertAndMerge = null;
        #endregion

        #region DataGridView - Tags & Keywords
        public static bool IsPopulatingTags { get => dataGridViewHandlerTags.IsPopulating; set => dataGridViewHandlerTags.IsPopulating = value; }
        public static bool IsPopulatingTagsFile { get => dataGridViewHandlerTags.IsPopulatingFile; set => dataGridViewHandlerTags.IsPopulatingFile = value; }
        public static bool IsPopulatingTagsImage { get => dataGridViewHandlerTags.IsPopulatingImage; set => dataGridViewHandlerTags.IsPopulatingImage = value; }
        public static bool IsAgregatedTags { get => dataGridViewHandlerTags.IsAgregated; set => dataGridViewHandlerTags.IsAgregated = value; }
        #endregion

        #region DataGridView - Map
        public static bool IsPopulatingMap { get => dataGridViewHandlerMap.IsPopulating; set => dataGridViewHandlerMap.IsPopulating = value; }
        public static bool IsPopulatingMapFile { get => dataGridViewHandlerMap.IsPopulatingFile; set => dataGridViewHandlerMap.IsPopulatingFile = value; }
        public static bool IsPopulatingMapImage { get => dataGridViewHandlerMap.IsPopulatingImage; set => dataGridViewHandlerMap.IsPopulatingImage = value; }
        public static bool IsAgregatedMap { get => dataGridViewHandlerMap.IsAgregated; set => dataGridViewHandlerMap.IsAgregated = value; }
        #endregion

        #region DataGridView - People
        public static bool IsPopulatingPeople { get => dataGridViewHandlerPeople.IsPopulating; set => dataGridViewHandlerPeople.IsPopulating = value; }
        public static bool IsPopulatingPeopleFile { get => dataGridViewHandlerPeople.IsPopulatingFile; set => dataGridViewHandlerPeople.IsPopulatingFile = value; }
        public static bool IsPopulatingPeopleImage { get => dataGridViewHandlerPeople.IsPopulatingImage; set => dataGridViewHandlerPeople.IsPopulatingImage = value; }
        public static bool IsAgregatedPeople { get => dataGridViewHandlerPeople.IsAgregated; set => dataGridViewHandlerPeople.IsAgregated = value; }
        #endregion

        #region DataGridView - Date
        public static bool IsPopulatingDate { get => dataGridViewHandlerDates.IsPopulating; set => dataGridViewHandlerDates.IsPopulating = value; }
        public static bool IsPopulatingDateFile { get => dataGridViewHandlerDates.IsPopulatingFile; set => dataGridViewHandlerDates.IsPopulatingFile = value; }
        public static bool IsPopulatingDateImage { get => dataGridViewHandlerDates.IsPopulatingImage; set => dataGridViewHandlerDates.IsPopulatingImage = value; }
        public static bool IsAgregatedDate { get => dataGridViewHandlerDates.IsAgregated; set => dataGridViewHandlerDates.IsAgregated = value; }
        #endregion

        #region DataGridView - Exiftool tags
        public static bool IsPopulatingExiftoolTags { get => dataGridViewHandlerExiftoolTags.IsPopulating; set => dataGridViewHandlerExiftoolTags.IsPopulating = value; }
        public static bool IsPopulatingExiftoolTagsFile { get => dataGridViewHandlerExiftoolTags.IsPopulatingFile; set => dataGridViewHandlerExiftoolTags.IsPopulatingFile = value; }
        public static bool IsPopulatingExiftoolTagsImage { get => dataGridViewHandlerExiftoolTags.IsPopulatingImage; set => dataGridViewHandlerExiftoolTags.IsPopulatingImage = value; }
        public static bool IsAgregatedExiftoolTags { get => dataGridViewHandlerExiftoolTags.IsAgregated; set => dataGridViewHandlerExiftoolTags.IsAgregated = value; }
        #endregion

        #region DataGridView - Exiftool warning
        public static bool IsPopulatingExiftoolWarning { get => dataGridViewHandlerExiftoolWarning.IsPopulating; set => dataGridViewHandlerExiftoolWarning.IsPopulating = value; }
        public static bool IsPopulatingExiftoolWarningFile { get => dataGridViewHandlerExiftoolWarning.IsPopulatingFile; set => dataGridViewHandlerExiftoolWarning.IsPopulatingFile = value; }
        public static bool IsPopulatingExiftoolWarningImage { get => dataGridViewHandlerExiftoolWarning.IsPopulatingImage; set => dataGridViewHandlerExiftoolWarning.IsPopulatingImage = value; }
        public static bool IsAgregatedExiftoolWarning { get => dataGridViewHandlerExiftoolWarning.IsAgregated; set => dataGridViewHandlerExiftoolWarning.IsAgregated = value; }
        #endregion

        #region DataGridView - Properties
        public static bool IsPopulatingProperties { get => dataGridViewHandlerProperties.IsPopulating; set => dataGridViewHandlerProperties.IsPopulating = value; }
        public static bool IsPopulatingPropertiesFile { get => dataGridViewHandlerProperties.IsPopulatingFile; set => dataGridViewHandlerProperties.IsPopulatingFile = value; }
        public static bool IsPopulatingPropertiesImage { get => dataGridViewHandlerProperties.IsPopulatingImage; set => dataGridViewHandlerProperties.IsPopulatingImage = value; }
        public static bool IsAgregatedProperties { get => dataGridViewHandlerProperties.IsAgregated; set => dataGridViewHandlerProperties.IsAgregated = value; }
        #endregion

        #region DataGridView - Rename
        public static bool IsPopulatingRename { get => dataGridViewHandlerRename.IsPopulating; set => dataGridViewHandlerRename.IsPopulating = value; }
        public static bool IsPopulatingRenameFile { get => dataGridViewHandlerRename.IsPopulatingFile; set => dataGridViewHandlerRename.IsPopulatingFile = value; }
        public static bool IsPopulatingRenameImage { get => dataGridViewHandlerRename.IsPopulatingImage; set => dataGridViewHandlerRename.IsPopulatingImage = value; }
        public static bool IsAgregatedRename { get => dataGridViewHandlerRename.IsAgregated; set => dataGridViewHandlerRename.IsAgregated = value; }
        #endregion

        #region DataGridView - Convert and Merge
        public static bool IsPopulatingConvertAndMerge { get => dataGridViewHandlerConvertAndMerge.IsPopulating; set => dataGridViewHandlerConvertAndMerge.IsPopulating = value; }
        public static bool IsPopulatingConvertAndMergeFile { get => dataGridViewHandlerConvertAndMerge.IsPopulatingFile; set => dataGridViewHandlerConvertAndMerge.IsPopulatingFile = value; }
        public static bool IsPopulatingConvertAndMergeImage { get => dataGridViewHandlerConvertAndMerge.IsPopulatingImage; set => dataGridViewHandlerConvertAndMerge.IsPopulatingImage = value; }
        public static bool IsAgregatedConvertAndMerge { get => dataGridViewHandlerConvertAndMerge.IsAgregated; set => dataGridViewHandlerConvertAndMerge.IsAgregated = value; }
        #endregion

        #region SetDataNotAgreegatedOnGridViewForAnyTabs
        public static void SetDataNotAgreegatedOnGridViewForAnyTabs()
        {
            IsAgregatedTags = false;            
            IsAgregatedMap = false;
            IsAgregatedPeople = false;
            IsAgregatedDate = false;
            IsAgregatedExiftoolTags = false;
            IsAgregatedExiftoolWarning = false;
            IsAgregatedProperties = false;
            IsAgregatedRename = false;
            IsAgregatedConvertAndMerge = false;
        }
        #endregion

        #region IsAgredagedGridViewAny
        public static bool HasDataGridViewAggregatedAny()
        {
            return (IsAgregatedTags ||
                IsAgregatedMap ||
                IsAgregatedPeople ||
                IsAgregatedDate ||
                IsPopulatingExiftoolTags ||
                IsPopulatingExiftoolWarning ||
                IsAgregatedProperties ||
                IsAgregatedRename || 
                IsAgregatedConvertAndMerge);
        }
        #endregion

        #region WhatsPopulating
        public static string WhatsPopulating()
        {
            return
                (IsApplicationClosing ? "Application is closing\r\n" : "") +
                (IsPopulatingImageListViewFromFolderOrDatabaseList ? "Populating ImageListView List\r\n" : "") +
                
                //Acton button
                (IsPerformingAButtonAction ? "Action Button\r\n" : "") +

                //Keywords
                (IsPopulatingTags ? "Populating Tags and Keywords\r\n" : "") +
                (IsPopulatingTagsFile ? "Populating Tags and Keywords File\r\n" : "") +
                (IsPopulatingTagsImage ? "Populating Tags and Keywords Image\r\n" : "") +

                //Map
                (IsPopulatingMap ? "Populating Map\r\n" : "") +
                (IsPopulatingMapFile ? "Populating Map File\r\n" : "") +
                (IsPopulatingMapImage ? "Populating Map Image\r\n" : "") +

                //People
                (IsPopulatingPeople ? "Populating People\r\n" : "") +
                (IsPopulatingPeopleFile ? "Populating People File\r\n" : "") +
                (IsPopulatingPeopleImage ? "Populating People Image\r\n" : "") +

                //Date
                (IsPopulatingDate ? "PopulatingDate\r\n" : "") +
                (IsPopulatingDateFile ? "PopulatingDate File\r\n" : "") +
                (IsPopulatingDateImage ? "PopulatingDate Image\r\n" : "") +

                //Exiftool tags
                (IsPopulatingExiftoolTags ? "PopulatingExiftoolTags\r\n" : "") +
                (IsPopulatingExiftoolTagsFile ? "PopulatingExiftoolTags File\r\n" : "") +
                (IsPopulatingExiftoolTagsImage ? "PopulatingExiftoolTags Image\r\n" : "") +

                //Exiftool warning
                (IsPopulatingExiftoolWarning ? "Populating ExiftoolWarning\r\n" : "") +
                (IsPopulatingExiftoolWarningFile ? "Populating ExiftoolWarning File\r\n" : "") +
                (IsPopulatingExiftoolWarningImage ? "Populating ExiftoolWarning Image\r\n" : "") +

                //Properties
                (IsPopulatingProperties ? "Populating Properties\r\n" : "") +
                (IsPopulatingPropertiesFile ? "Populating Properties File\r\n" : "") +
                (IsPopulatingPropertiesImage ? "Populating Properties Image\r\n" : "") +

                //Rename
                (IsPopulatingRename ? "Populating Rename\r\n" : "") +
                (IsPopulatingRenameFile ? "Populating Rename File\r\n" : "") +
                (IsPopulatingRenameImage ? "Populating Rename Image\r\n" : "") +

                //Convert and Merge
                (IsPopulatingConvertAndMerge ? "Populating ConvertAndMerge\r\n" : "") +
                (IsPopulatingConvertAndMergeFile ? "Populating ConvertAndMerge File\r\n" : "") +
                (IsPopulatingConvertAndMergeImage ? "Populating ConvertAndMerge Image\r\n" : "");
        }
        #endregion

        #region IsPopulatingAnything
        public static bool IsPopulatingAnything()
        {    
            bool result = (
                IsApplicationClosing ||
                IsPopulatingImageListViewFromFolderOrDatabaseList ||

                //Acton button
                IsPerformingAButtonAction ||

                //Keywords
                IsPopulatingTags ||
                IsPopulatingTagsFile ||
                IsPopulatingTagsImage ||

                //Map
                IsPopulatingMap ||
                IsPopulatingMapFile ||
                IsPopulatingMapImage ||

                //People
                IsPopulatingPeople ||
                IsPopulatingPeopleFile ||
                IsPopulatingPeopleImage ||

                //Date
                IsPopulatingDate ||
                IsPopulatingDateFile ||
                IsPopulatingDateImage ||

                //Exiftool tags
                IsPopulatingExiftoolTags ||
                IsPopulatingExiftoolTagsFile ||
                IsPopulatingExiftoolTagsImage ||

                //Exiftool warning
                IsPopulatingExiftoolWarning ||
                IsPopulatingExiftoolWarningFile ||
                IsPopulatingExiftoolWarningImage ||

                //Properties
                IsPopulatingProperties ||
                IsPopulatingPropertiesFile ||
                IsPopulatingPropertiesImage ||

                //Rename
                IsPopulatingRename ||
                IsPopulatingRenameFile ||
                IsPopulatingRenameImage ||

                //Convert and Merge
                IsPopulatingConvertAndMerge ||
                IsPopulatingConvertAndMergeFile ||
                IsPopulatingConvertAndMergeImage
                );
            return result;
        }
        #endregion
    }
}
