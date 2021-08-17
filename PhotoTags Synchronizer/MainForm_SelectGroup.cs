using System.Windows.Forms;
using Manina.Windows.Forms;
using MetadataLibrary;
using static Manina.Windows.Forms.ImageListView;
using System;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        private int lastGroupBaseIndex = int.MaxValue;
        private int lastGroupDirection = 0;

        private void GroupSelectionClear()
        {
            lastGroupDirection = 0;
            lastGroupBaseIndex = int.MaxValue;
        }

        private void SelectedGroupBySelections(ImageListView imageListView, int baseItemIndex, int direction,
            int maxSelectCount, int maxDayRange, bool fallbackOnFileCreated,
            bool checkLocationName, bool checkCity, bool checkDistrict, bool checkCountry)
        {
            using (new WaitCursor())
            {
                ImageListViewItemCollection imageListViewItems = imageListView.Items;
                if (baseItemIndex < imageListViewItems.Count && direction != 0)
                {
                    bool checkDayRange = maxDayRange > 0;

                    bool isMetadataNull;
                    string locationName = null;
                    string city = null;
                    string district = null;
                    string country = null;
                    DateTime? mediaDateTime = null;

                    ImageListViewItem imageListViewItem = imageListViewItems[baseItemIndex];
                    Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(imageListViewItem.FileFullPath, imageListViewItem.DateModified, MetadataBrokerType.ExifTool));
                    if (metadata != null)
                    {
                        locationName = metadata.LocationName;
                        city = metadata.LocationCity;
                        district = metadata.LocationState;
                        country = metadata.LocationCountry;
                        mediaDateTime = imageListViewItem.DateCreated;;
                        isMetadataNull = false;
                        if (metadata.MediaDateTaken != null && fallbackOnFileCreated) mediaDateTime = metadata.MediaDateTaken;
                    }
                    else isMetadataNull = true;


                    GlobalData.IsPopulatingButtonAction = true;
                    GlobalData.IsPopulatingImageListView = true; //Avoid one and one select item getting refreshed
                    GlobalData.DoNotRefreshDataGridViewWhileFileSelect = true;
                    folderTreeViewFolder.Enabled = false;
                    imageListView.Enabled = false;
                    toolStripButtonSelectPrevious.Enabled = false;
                    toolStripDropDownButtonSelectGroupBy.Enabled = false;
                    toolStripButtonSelectNext.Enabled = false;

                    ImageListViewSuspendLayoutInvoke(imageListView);

                    imageListView.ClearSelection();

                    int selectedCount = 0;
                    int itemIndex = baseItemIndex;

                    while (itemIndex > -1 && itemIndex < imageListViewItems.Count && selectedCount < maxSelectCount)
                    {
                        imageListViewItem = imageListViewItems[itemIndex];

                        //Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(new FileEntryBroker(imageListViewItem.FileFullPath, imageListViewItem.DateModified, MetadataBrokerType.ExifTool));
                        metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(imageListViewItem.FileFullPath, imageListViewItem.DateModified, MetadataBrokerType.ExifTool));

                        if (isMetadataNull && metadata == null)
                        {
                            imageListViewItem.Selected = true;
                            selectedCount++;
                        }
                        else if (isMetadataNull && metadata != null)
                        {
                            imageListViewItem.Selected = false;

                        }
                        else if (metadata != null)
                        {
                            bool isItemsEqual = true;
                            if (checkLocationName && locationName != metadata.LocationName) isItemsEqual = false;
                            if (checkCity && city == metadata.LocationCity) isItemsEqual = false;
                            if (checkDistrict && district == metadata.LocationState) isItemsEqual = false;
                            if (checkCountry && country == metadata.LocationCountry) isItemsEqual = false;

                            DateTime? metadataMediaDateTime = metadata.FileDateCreated;
                            if (metadata.MediaDateTaken != null && fallbackOnFileCreated) metadataMediaDateTime = metadata.MediaDateTaken;

                            if (checkDayRange && mediaDateTime != null && metadataMediaDateTime == null) isItemsEqual = false;
                            else if (checkDayRange && mediaDateTime == null && metadataMediaDateTime != null) isItemsEqual = false;
                            else if (mediaDateTime != null && metadataMediaDateTime != null &&
                                checkDayRange && Math.Abs(((DateTime)metadataMediaDateTime - (DateTime)mediaDateTime).TotalDays) > maxDayRange) isItemsEqual = false;

                            if (isItemsEqual)
                            {
                                selectedCount++;
                                imageListViewItem.Selected = true;
                            }
                        }

                        itemIndex += direction;
                    }

                    imageListView.EnsureVisible(itemIndex - direction);
                    imageListView.EnsureVisible(baseItemIndex);

                }

                toolStripButtonSelectPrevious.Enabled = true;
                toolStripDropDownButtonSelectGroupBy.Enabled = true;
                toolStripButtonSelectNext.Enabled = true;
                imageListView.Enabled = true;
                folderTreeViewFolder.Enabled = true;

                GlobalData.DoNotRefreshDataGridViewWhileFileSelect = false;
                GlobalData.IsPopulatingButtonAction = false;
                GlobalData.IsPopulatingImageListView = false;

                ImageListViewResumeLayoutInvoke(imageListView);
                imageListView.Focus();

                FilesSelected();
                lastGroupBaseIndex = baseItemIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageListView"></param>
        /// <param name="direction">-1 = backwards, 0 both directions (when direction not slected), 1 forward</param>
        /// <returns></returns>
        private int SelectedGroupFindBaseItemIndex(ImageListView imageListView, int direction)
        {
            using (new WaitCursor())
            {
                ImageListViewItemCollection imageListViewItems = imageListView.Items;

                int selectedCount = imageListView.SelectedItems.Count;
                int baseItemIndex;

                if (direction == 0)
                {
                    baseItemIndex = lastGroupBaseIndex; //-1 is unknown
                }
                else
                {
                    if (selectedCount == 0)
                    {
                        if (direction == 1) baseItemIndex = 0;
                        else baseItemIndex = imageListViewItems.Count - 1;
                    }
                    else
                    {
                        bool isSelecedFound = false;
                        bool isBaseItemFound = false;

                        if (direction == 1) baseItemIndex = 0;
                        else baseItemIndex = imageListViewItems.Count - 1;

                        while (!isBaseItemFound && baseItemIndex > -1 && baseItemIndex < imageListViewItems.Count)
                        {
                            ImageListViewItem imageListViewItem = imageListViewItems[baseItemIndex];



                            if (!isSelecedFound && imageListViewItem.Selected)
                            {
                                isSelecedFound = true;
                                if (selectedCount == 1 && lastGroupBaseIndex != baseItemIndex) isBaseItemFound = true;
                            }

                            if (isSelecedFound && !imageListViewItem.Selected) isBaseItemFound = true;
                            if (!isBaseItemFound) baseItemIndex += direction;
                        }
                    }
                }

                if (baseItemIndex > imageListViewItems.Count - 1) baseItemIndex = 0;
                if (baseItemIndex < 0) baseItemIndex = imageListViewItems.Count - 1;

                lastGroupBaseIndex = baseItemIndex;
                return baseItemIndex;
            }
        }
    }
}
