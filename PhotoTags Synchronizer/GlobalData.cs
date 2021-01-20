using DataGridViewGeneric;
using System.Collections.Generic;

namespace PhotoTagsSynchronizer
{
    public static class GlobalData
    {
        private static object isGlobalDataBeenUpdated = new object();
        public static readonly object populateSelectedLock = new object(); //Avoid gridview to update while updateing
        public static readonly object metadataUpdateLock = new object();

        public static List<string> SerachFilterResult { get; set; }
        public static bool SearchFolder { get; set; } = true;

        public static DataGridViewHandler dataGridViewHandlerTags = null;
        public static DataGridViewHandler dataGridViewHandlerMap = null;       
        public static DataGridViewHandler dataGridViewHandlerPeople = null;
        public static DataGridViewHandler dataGridViewHandlerDates = null;
        public static DataGridViewHandler dataGridViewHandlerExiftoolTags = null;        
        public static DataGridViewHandler dataGridViewHandlerExiftoolWarning = null;       
        public static DataGridViewHandler dataGridViewHandlerProperties = null;
        public static DataGridViewHandler dataGridViewHandlerRename = null;

        public static bool lastReadFolderWasRecursive { get; set; } = false;

        public static int retrieveImageCount = 0;
        public static int retrieveThumbnailCount = 0;

        //
        public static bool IsApplicationClosing { get; set; } = false;
        public static bool IsPopulatingFolderTree { get; set; } = true;
        public static bool IsPopulatingImageListView { get; set; } = false;

        public static readonly object ImageListViewForEachLock = new object();
        public static bool IsImageListViewForEachInProgressRequestStop { get; set; } = false;

        public static bool IsPopulatingFolderSelected { get; set; } = false;
        public static bool IsDataGridViewCutPasteDeleteFindReplaceInProgress { get; set; } = false;


        //Keywords
        public static bool IsPopulatingTags { get => dataGridViewHandlerTags.IsPopulating; set => dataGridViewHandlerTags.IsPopulating = value; }
        public static bool IsPopulatingTagsFile { get => dataGridViewHandlerTags.IsPopulatingFile; set => dataGridViewHandlerTags.IsPopulatingFile = value; }
        public static bool IsPopulatingTagsImage { get => dataGridViewHandlerTags.IsPopulatingImage; set => dataGridViewHandlerTags.IsPopulatingImage = value; }
        public static bool IsAgregatedTags { get => dataGridViewHandlerTags.IsAgregated; set => dataGridViewHandlerTags.IsAgregated = value; }
        
        //Map
        public static bool IsPopulatingMap { get => dataGridViewHandlerMap.IsPopulating; set => dataGridViewHandlerMap.IsPopulating = value; }
        public static bool IsPopulatingMapFile { get => dataGridViewHandlerMap.IsPopulatingFile; set => dataGridViewHandlerMap.IsPopulatingFile = value; }
        public static bool IsPopulatingMapImage { get => dataGridViewHandlerMap.IsPopulatingImage; set => dataGridViewHandlerMap.IsPopulatingImage = value; }
        public static bool IsAgregatedMap { get => dataGridViewHandlerMap.IsAgregated; set => dataGridViewHandlerMap.IsAgregated = value; }
        
        //People
        public static bool IsPopulatingPeople { get => dataGridViewHandlerPeople.IsPopulating; set => dataGridViewHandlerPeople.IsPopulating = value; }
        public static bool IsPopulatingPeopleFile { get => dataGridViewHandlerPeople.IsPopulatingFile; set => dataGridViewHandlerPeople.IsPopulatingFile = value; }
        public static bool IsPopulatingPeopleImage { get => dataGridViewHandlerPeople.IsPopulatingImage; set => dataGridViewHandlerPeople.IsPopulatingImage = value; }
        public static bool IsAgregatedPeople { get => dataGridViewHandlerPeople.IsAgregated; set => dataGridViewHandlerPeople.IsAgregated = value; }

        //Date
        public static bool IsPopulatingDate { get => dataGridViewHandlerDates.IsPopulating; set => dataGridViewHandlerDates.IsPopulating = value; }
        public static bool IsPopulatingDateFile { get => dataGridViewHandlerDates.IsPopulatingFile; set => dataGridViewHandlerDates.IsPopulatingFile = value; }
        public static bool IsPopulatingDateImage { get => dataGridViewHandlerDates.IsPopulatingImage; set => dataGridViewHandlerDates.IsPopulatingImage = value; }
        public static bool IsAgregatedDate { get => dataGridViewHandlerDates.IsAgregated; set => dataGridViewHandlerDates.IsAgregated = value; }

        //Exiftool tags
        public static bool IsPopulatingExiftoolTags { get => dataGridViewHandlerExiftoolTags.IsPopulating; set => dataGridViewHandlerExiftoolTags.IsPopulating = value; }
        public static bool IsPopulatingExiftoolTagsFile { get => dataGridViewHandlerExiftoolTags.IsPopulatingFile; set => dataGridViewHandlerExiftoolTags.IsPopulatingFile = value; }
        public static bool IsPopulatingExiftoolTagsImage { get => dataGridViewHandlerExiftoolTags.IsPopulatingImage; set => dataGridViewHandlerExiftoolTags.IsPopulatingImage = value; }
        public static bool IsAgregatedExiftoolTags { get => dataGridViewHandlerExiftoolTags.IsAgregated; set => dataGridViewHandlerExiftoolTags.IsAgregated = value; }
        
        //Exiftool warning
        public static bool IsPopulatingExiftoolWarning { get => dataGridViewHandlerExiftoolWarning.IsPopulating; set => dataGridViewHandlerExiftoolWarning.IsPopulating = value; }
        public static bool IsPopulatingExiftoolWarningFile { get => dataGridViewHandlerExiftoolWarning.IsPopulatingFile; set => dataGridViewHandlerExiftoolWarning.IsPopulatingFile = value; }
        public static bool IsPopulatingExiftoolWarningImage { get => dataGridViewHandlerExiftoolWarning.IsPopulatingImage; set => dataGridViewHandlerExiftoolWarning.IsPopulatingImage = value; }
        public static bool IsAgregatedExiftoolWarning { get => dataGridViewHandlerExiftoolWarning.IsAgregated; set => dataGridViewHandlerExiftoolWarning.IsAgregated = value; }
        
        //Properties
        public static bool IsPopulatingProperties { get => dataGridViewHandlerProperties.IsPopulating; set => dataGridViewHandlerProperties.IsPopulating = value; }
        public static bool IsPopulatingPropertiesFile { get => dataGridViewHandlerProperties.IsPopulatingFile; set => dataGridViewHandlerProperties.IsPopulatingFile = value; }
        public static bool IsPopulatingPropertiesImage { get => dataGridViewHandlerProperties.IsPopulatingImage; set => dataGridViewHandlerProperties.IsPopulatingImage = value; }
        public static bool IsAgregatedProperties { get => dataGridViewHandlerProperties.IsAgregated; set => dataGridViewHandlerProperties.IsAgregated = value; }

        //Rename
        public static bool IsPopulatingRename { get => dataGridViewHandlerRename.IsPopulating; set => dataGridViewHandlerRename.IsPopulating = value; }
        public static bool IsPopulatingRenameFile { get => dataGridViewHandlerRename.IsPopulatingFile; set => dataGridViewHandlerRename.IsPopulatingFile = value; }
        public static bool IsPopulatingRenameImage { get => dataGridViewHandlerRename.IsPopulatingImage; set => dataGridViewHandlerRename.IsPopulatingImage = value; }
        public static bool IsAgregatedRename { get => dataGridViewHandlerRename.IsAgregated; set => dataGridViewHandlerRename.IsAgregated = value; }

        //Acton button
        public static bool IsPopulatingButtonAction { get; set; } = false;

        public static bool IsSaveButtonPushed { get; set; } = false;

        public static bool DoNotRefreshDataGridViewWhileFileSelect { get; set; } = false;
        public static bool DoNotRefreshImageListView { get; set; } = false;
        public static bool IsDragAndDropActive { get; set; } = false;
        public static bool IsPopulatingFilter { get; set; } = false;

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
        }        

        //Data stored in DataGridView
        public static bool IsAgredagedGridViewAny()
        {
            return (IsAgregatedTags ||
                IsAgregatedMap ||
                IsAgregatedPeople ||
                IsAgregatedDate ||
                IsPopulatingExiftoolTags ||
                IsPopulatingExiftoolWarning ||
                IsAgregatedProperties ||
                IsAgregatedRename);
        }

        public static bool IsPopulatingAnything()
        {            
            return (
                IsApplicationClosing ||
                IsPopulatingFolderTree ||
                IsPopulatingImageListView ||
                IsPopulatingFolderSelected ||
                //IsSaveButtonPushed ||

                //Acton button
                IsPopulatingButtonAction ||

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
                IsPopulatingRenameImage
                );

        }

    }
}
