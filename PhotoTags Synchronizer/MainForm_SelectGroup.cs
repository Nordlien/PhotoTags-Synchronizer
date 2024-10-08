﻿using System.Windows.Forms;
using Manina.Windows.Forms;
using MetadataLibrary;
using static Manina.Windows.Forms.ImageListView;
using System;
using Krypton.Toolkit;
using System.Diagnostics;
using System.Collections.Generic;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
    {

        private int lastGroupBaseIndex = int.MaxValue;
        private int lastGroupDirection = 0;

        #region GroupSelectionClear
        private void FastGroupSelection_Clear()
        {
            lastGroupDirection = 0;
            lastGroupBaseIndex = int.MaxValue;
        }
        #endregion 

        #region SelectedGroupBySelections
        private void SelectedGroupByMatch(ImageListView imageListView,
            bool checkFileCreated, bool checkMediaTaken, bool checkAllDates, int maxDayRange,
            bool checkLocationName, bool checkLocationCity, bool checkLocationDistrict, bool checkLocationCountry, bool checkAllLocations)
        {

            try
            {

                ImageListViewHandler.SuspendLayout(imageListView1);
                ImageListViewHandler.Enable(imageListView, false);
                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, false);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;

                #region Do the work
                using (new WaitCursor())
                {
                    ImageListViewItemCollection imageListViewItems = imageListView.Items;

                    List<GroupMacth> groupMacthSourceList = new List<GroupMacth>();


                    #region Init
                    foreach (ImageListViewItem imageListViewItem in imageListView.SelectedItems)
                    {
                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(imageListViewItem.FileFullPath, imageListViewItem.DateModified, MetadataBrokerType.ExifTool));

                        GroupMacth groupMacthSource = new GroupMacth();
                        groupMacthSource.IsMetadataNull = (metadata == null);
                        groupMacthSource.FileDate = imageListViewItem.Date;
                        groupMacthSource.MediaTaken = metadata?.MediaDateTaken;
                        groupMacthSource.LocationName = metadata?.LocationName;
                        groupMacthSource.LocationCity = metadata?.LocationCity;
                        groupMacthSource.LocationDistrict = metadata?.LocationState;
                        groupMacthSource.LocationCountry = metadata?.LocationCountry;
                        groupMacthSourceList.Add(groupMacthSource);
                    }
                    #endregion


                    imageListView.ClearSelection();

                    #region Find and Select
                    foreach (ImageListViewItem imageListViewItem in imageListView.Items)
                    {
                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(imageListViewItem.FileFullPath, imageListViewItem.DateModified, MetadataBrokerType.ExifTool));

                        GroupMacth groupMacthCheckWith = new GroupMacth();
                        groupMacthCheckWith.IsMetadataNull = (metadata == null);
                        groupMacthCheckWith.FileDate = imageListViewItem.Date;
                        groupMacthCheckWith.MediaTaken = metadata?.MediaDateTaken;
                        groupMacthCheckWith.LocationName = metadata?.LocationName;
                        groupMacthCheckWith.LocationCity = metadata?.LocationCity;
                        groupMacthCheckWith.LocationDistrict = metadata?.LocationState;
                        groupMacthCheckWith.LocationCountry = metadata?.LocationCountry;

                        bool isItemsEqual = false;

                        foreach (GroupMacth groupMacthSource in groupMacthSourceList)
                        {
                            isItemsEqual = GroupMacth.IsMatch(groupMacthSource, groupMacthCheckWith, checkFileCreated, checkMediaTaken,
                                checkAllDates, maxDayRange,
                                checkLocationName, checkLocationCity, checkLocationDistrict, checkLocationCountry, checkAllLocations);
                            if (isItemsEqual) break;
                        }

                        if (isItemsEqual) imageListViewItem.Selected = true; else imageListViewItem.Selected = false;
                    }
                    #endregion

                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, true);
                ImageListViewHandler.Enable(imageListView, true);
                ImageListViewHandler.ResumeLayout(imageListView1);

                imageListView.Focus();

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
        }
        #endregion

        #region SelectedGroupBySelections
        private void SelectedGroupBySelections(ImageListView imageListView, int baseItemIndex, int direction, int maxSelectCount,
            bool checkFileCreated, bool checkMediaTaken, bool checkAllDates, int maxDayRange,
            bool checkLocationName, bool checkLocationCity, bool checkLocationDistrict, bool checkLocationCountry, bool checkAllLocations)
        {
            try
            {
                ImageListViewHandler.SuspendLayout(imageListView1);
                ImageListViewHandler.Enable(imageListView, false);
                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, false);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;

                #region Do the work
                using (new WaitCursor())
                {
                    ImageListViewItemCollection imageListViewItems = imageListView.Items;
                    if (baseItemIndex < imageListViewItems.Count && direction != 0)
                    {
                        #region Init Variables
                        GroupMacth groupMacthSource = new GroupMacth();

                        ImageListViewItem imageListViewItem = imageListViewItems[baseItemIndex];
                        Metadata metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(imageListViewItem.FileFullPath, imageListViewItem.DateModified, MetadataBrokerType.ExifTool));

                        groupMacthSource.IsMetadataNull = (metadata == null);
                        groupMacthSource.FileDate = imageListViewItem.Date;
                        groupMacthSource.MediaTaken = metadata?.MediaDateTaken;
                        groupMacthSource.LocationName = metadata?.LocationName;
                        groupMacthSource.LocationCity = metadata?.LocationCity;
                        groupMacthSource.LocationDistrict = metadata?.LocationState;
                        groupMacthSource.LocationCountry = metadata?.LocationCountry;
                        #endregion

                        imageListView.ClearSelection();

                        #region Find and Select
                        int selectedCount = 0;
                        int itemIndex = baseItemIndex;
                        while (itemIndex > -1 && itemIndex < imageListViewItems.Count && selectedCount < maxSelectCount)
                        {
                            imageListViewItem = imageListViewItems[itemIndex];
                            metadata = databaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(new FileEntryBroker(imageListViewItem.FileFullPath, imageListViewItem.DateModified, MetadataBrokerType.ExifTool));

                            GroupMacth groupMacthCheckWith = new GroupMacth();
                            groupMacthCheckWith.IsMetadataNull = (metadata == null);
                            groupMacthCheckWith.FileDate = imageListViewItem.Date;
                            groupMacthCheckWith.MediaTaken = metadata?.MediaDateTaken;
                            groupMacthCheckWith.LocationName = metadata?.LocationName;
                            groupMacthCheckWith.LocationCity = metadata?.LocationCity;
                            groupMacthCheckWith.LocationDistrict = metadata?.LocationState;
                            groupMacthCheckWith.LocationCountry = metadata?.LocationCountry;


                            bool isItemsEqual = GroupMacth.IsMatch(groupMacthSource, groupMacthCheckWith, checkFileCreated, checkMediaTaken,
                                checkAllDates, maxDayRange,
                                checkLocationName, checkLocationCity, checkLocationDistrict, checkLocationCountry, checkAllLocations);

                            if (isItemsEqual)
                            {
                                selectedCount++;
                                imageListViewItem.Selected = true;
                            }
                            else imageListViewItem.Selected = false;

                            itemIndex += direction;
                        }
                        #endregion

                        imageListView.EnsureVisible(itemIndex - direction);
                        imageListView.EnsureVisible(baseItemIndex);
                    }
                    lastGroupBaseIndex = baseItemIndex;
                    
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, true);
                ImageListViewHandler.Enable(imageListView, true);
                ImageListViewHandler.ResumeLayout(imageListView1);

                imageListView.Focus();
                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
        }
        #endregion 

        #region SelectedGroupFindBaseItemIndex
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
        #endregion

        #region ImageListViewSelectNext
        private void ImageListViewSelectNext()
        {
            try
            {
                lastGroupDirection = 1;
                int baseItemIndex = SelectedGroupFindBaseItemIndex(imageListView1, lastGroupDirection);

                SelectGroupGetProperties();
                SelectedGroupBySelections(imageListView1, baseItemIndex, lastGroupDirection,
                    Properties.Settings.Default.SelectGroupMaxCount,
                    Properties.Settings.Default.SelectGroupFileCreated,
                    Properties.Settings.Default.SelectGroupMediaTaken,
                    Properties.Settings.Default.SelectGroupCheckAllDates,
                    Properties.Settings.Default.SelectGroupNumberOfDays,
                    Properties.Settings.Default.SelectGroupSameLocationName,
                    Properties.Settings.Default.SelectGroupSameCity,
                    Properties.Settings.Default.SelectGroupSameDistrict,
                    Properties.Settings.Default.SelectGroupSameCountry,
                    Properties.Settings.Default.SelectGroupCheckAllLocations);
                DisplayAllQueueStatus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ImageListViewSelectPrevious
        private void ImageListViewSelectPrevious()
        {
            try
            {
                lastGroupDirection = -1;
                int baseItemIndex = SelectedGroupFindBaseItemIndex(imageListView1, lastGroupDirection);

                SelectGroupGetProperties();
                SelectedGroupBySelections(imageListView1, baseItemIndex, lastGroupDirection,
                    Properties.Settings.Default.SelectGroupMaxCount,
                    Properties.Settings.Default.SelectGroupFileCreated,
                    Properties.Settings.Default.SelectGroupMediaTaken,
                    Properties.Settings.Default.SelectGroupCheckAllDates,
                    Properties.Settings.Default.SelectGroupNumberOfDays,
                    Properties.Settings.Default.SelectGroupSameLocationName,
                    Properties.Settings.Default.SelectGroupSameCity,
                    Properties.Settings.Default.SelectGroupSameDistrict,
                    Properties.Settings.Default.SelectGroupSameCountry,
                    Properties.Settings.Default.SelectGroupCheckAllLocations);
                DisplayAllQueueStatus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ImageListViewSelectMatch
        private void ImageListViewSelectMatch()
        {
            try
            {
                SelectGroupGetProperties();
                SelectedGroupByMatch(imageListView1,
                    kryptonRibbonGroupCheckBoxSelectFileCreated.Checked,
                    kryptonRibbonGroupCheckBoxSelectMediaTaken.Checked,
                    kryptonRibbonGroupCheckBoxSelectCheckAllDates.Checked,
                    Properties.Settings.Default.SelectGroupNumberOfDays,
                    Properties.Settings.Default.SelectGroupSameLocationName,
                    Properties.Settings.Default.SelectGroupSameCity,
                    Properties.Settings.Default.SelectGroupSameDistrict,
                    Properties.Settings.Default.SelectGroupSameCountry,
                    kryptonRibbonGroupCheckBoxSelectCheckAllLocations.Checked);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "");
                KryptonMessageBox.Show("Following error occured: \r\n" + ex.Message, "Was not able to complete operation", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region ImageListViewWildcardsSelectMatch
        private void ImageListViewWildcardsSelectMatch(ImageListView imageListView, string wildcards)
        {
            try
            {
                ImageListViewHandler.SuspendLayout(imageListView1);
                ImageListViewHandler.Enable(imageListView, false);
                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, false);
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = true;

                imageListView.ClearSelection();

                string[] wildcardsArray = wildcards.Split(new string[] { System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator, "\r\n", ";", ",", "\n", "\r", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string pattern in wildcardsArray)
                {
                    foreach (ImageListViewItem imageListViewItem in imageListView.Items)
                    {
                        if (FileHandeling.FileHandler.FilenameMatchesPattern(imageListViewItem.Text, pattern)) imageListViewItem.Selected = true;
                    }
                }     
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.DoNotTrigger_ImageListView_SelectionChanged = false;
                TreeViewFolderBrowserHandler.Enabled(treeViewFolderBrowser1, true);
                ImageListViewHandler.Enable(imageListView, true);
                ImageListViewHandler.ResumeLayout(imageListView1);

                imageListView.Focus();

                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);
            }
        }
        #endregion

        #region Select Group - Getvalues
        private void SelectGroupGetProperties()
        {
            if (kryptonRibbonGroupRadioButtonSelectDateRange1.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = 1;
            if (kryptonRibbonGroupRadioButtonSelectDateRange3.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = 3;
            if (kryptonRibbonGroupRadioButtonSelectDateRange7.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = 7;
            if (kryptonRibbonGroupRadioButtonSelectDateRange14.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = 14;
            if (kryptonRibbonGroupRadioButtonSelectDateRange30.Checked) Properties.Settings.Default.SelectGroupNumberOfDays = 30;

            if (kryptonRibbonGroupRadioButtonSelectMediaAmount10.Checked) Properties.Settings.Default.SelectGroupMaxCount = 10;
            if (kryptonRibbonGroupRadioButtonSelectMediaAmount30.Checked) Properties.Settings.Default.SelectGroupMaxCount = 30;
            if (kryptonRibbonGroupRadioButtonSelectMediaAmount50.Checked) Properties.Settings.Default.SelectGroupMaxCount = 50;
            if (kryptonRibbonGroupRadioButtonSelectMediaAmount100.Checked) Properties.Settings.Default.SelectGroupMaxCount = 100;

            Properties.Settings.Default.SelectGroupFileCreated = kryptonRibbonGroupCheckBoxSelectFileCreated.Checked;
            Properties.Settings.Default.SelectGroupMediaTaken = kryptonRibbonGroupCheckBoxSelectMediaTaken.Checked;
            Properties.Settings.Default.SelectGroupCheckAllDates = kryptonRibbonGroupCheckBoxSelectCheckAllDates.Checked;
            Properties.Settings.Default.SelectGroupSameLocationName = kryptonRibbonGroupCheckBoxSelectLocationName.Checked;
            Properties.Settings.Default.SelectGroupSameCity = kryptonRibbonGroupCheckBoxSelectLocationCity.Checked;
            Properties.Settings.Default.SelectGroupSameDistrict = kryptonRibbonGroupCheckBoxSelectLocationStateRegion.Checked;
            Properties.Settings.Default.SelectGroupSameCountry = kryptonRibbonGroupCheckBoxSelectLocationCountry.Checked;
            Properties.Settings.Default.SelectGroupCheckAllLocations = kryptonRibbonGroupCheckBoxSelectCheckAllLocations.Checked;
        }
        #endregion 

        #region Select Group - Populate ToolStripMenuItem
        private void PopulateSelectGroupToolStripMenuItems()
        {
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 1) kryptonRibbonGroupRadioButtonSelectDateRange1.Checked = true;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 3) kryptonRibbonGroupRadioButtonSelectDateRange3.Checked = true;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 7) kryptonRibbonGroupRadioButtonSelectDateRange7.Checked = true;
            if (Properties.Settings.Default.SelectGroupNumberOfDays == 14) kryptonRibbonGroupRadioButtonSelectDateRange14.Checked = true;

            if (Properties.Settings.Default.SelectGroupMaxCount == 10) kryptonRibbonGroupRadioButtonSelectMediaAmount10.Checked = true;
            if (Properties.Settings.Default.SelectGroupMaxCount == 30) kryptonRibbonGroupRadioButtonSelectMediaAmount30.Checked = true;
            if (Properties.Settings.Default.SelectGroupMaxCount == 50) kryptonRibbonGroupRadioButtonSelectMediaAmount50.Checked = true;
            if (Properties.Settings.Default.SelectGroupMaxCount == 100) kryptonRibbonGroupRadioButtonSelectMediaAmount100.Checked = true;

            kryptonRibbonGroupCheckBoxSelectFileCreated.Checked = Properties.Settings.Default.SelectGroupFileCreated;
            kryptonRibbonGroupCheckBoxSelectMediaTaken.Checked = Properties.Settings.Default.SelectGroupMediaTaken;
            kryptonRibbonGroupCheckBoxSelectCheckAllDates.Checked = Properties.Settings.Default.SelectGroupCheckAllDates;
            kryptonRibbonGroupCheckBoxSelectLocationName.Checked = Properties.Settings.Default.SelectGroupSameLocationName;
            kryptonRibbonGroupCheckBoxSelectLocationCity.Checked = Properties.Settings.Default.SelectGroupSameCity;
            kryptonRibbonGroupCheckBoxSelectLocationStateRegion.Checked = Properties.Settings.Default.SelectGroupSameDistrict;
            kryptonRibbonGroupCheckBoxSelectLocationCountry.Checked = Properties.Settings.Default.SelectGroupSameCountry;
            kryptonRibbonGroupCheckBoxSelectCheckAllLocations.Checked = Properties.Settings.Default.SelectGroupCheckAllLocations;
            FastGroupSelection_Clear();
        }
        #endregion

        #region Select Group - GroupSelectLast
        public void GroupSelectLast()
        {
            if (GlobalData.IsApplicationClosing) return;
            if (IsPerforminAButtonAction("Select Last")) return;
            if (IsPopulatingAnything("Select Last")) return;
            if (SaveBeforeContinue(true) == DialogResult.Cancel) return;

            try
            {
                GlobalData.IsPerformingAButtonAction = true;

                int baseItemIndex = SelectedGroupFindBaseItemIndex(imageListView1, 0);

                SelectGroupGetProperties();
                SelectedGroupBySelections(imageListView1, baseItemIndex, lastGroupDirection,
                    Properties.Settings.Default.SelectGroupMaxCount,
                    Properties.Settings.Default.SelectGroupFileCreated,
                    Properties.Settings.Default.SelectGroupMediaTaken,
                    Properties.Settings.Default.SelectGroupCheckAllDates,
                    Properties.Settings.Default.SelectGroupNumberOfDays,
                    Properties.Settings.Default.SelectGroupSameLocationName,
                    Properties.Settings.Default.SelectGroupSameCity,
                    Properties.Settings.Default.SelectGroupSameDistrict,
                    Properties.Settings.Default.SelectGroupSameCountry,
                    Properties.Settings.Default.SelectGroupCheckAllLocations);
                DisplayAllQueueStatus();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                KryptonMessageBox.Show("Unexpected error occur.\r\nException message:" + ex.Message + "\r\n",
                    "Unexpected error occur", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                GlobalData.IsPerformingAButtonAction = false;
            }
        }
        #endregion 

        #region Select Group - GroupSelectLast
        private void kryptonRibbonGroupCheckBoxSelectFileCreated_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupCheckBoxSelectCheckAllDates_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupCheckBoxSelectCheckAllLocations_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }


        private void kryptonRibbonGroupRadioButtonSelectDateRange1_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupRadioButtonSelectDateRange3_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupRadioButtonSelectDateRange7_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupRadioButtonSelectDateRange14_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupRadioButtonSelectDateRange30_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupRadioButtonSelectMediaAmount10_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }
        private void kryptonRibbonGroupRadioButtonSelectMediaAmount30_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupRadioButtonSelectMediaAmount50_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupRadioButtonSelectMediaAmount100_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupCheckBoxSelectLocationName_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupCheckBoxSelectLocationCity_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        private void kryptonRibbonGroupCheckBoxSelectLocationStateRegion_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }
        private void kryptonRibbonGroupCheckBoxSelectLocationCountry_Click(object sender, EventArgs e)
        {
            GroupSelectLast();
        }

        #endregion
    }

    #region class GroupMacth
    public class GroupMacth
    {
        public bool IsMetadataNull { get; set; } = true;
        public DateTime? FileDate { get; set; } = null;
        public DateTime? MediaTaken { get; set; } = null;        
        public string LocationName { get; set; } = null;
        public string LocationCity { get; set; } = null;
        public string LocationDistrict { get; set; } = null;
        public string LocationCountry { get; set; } = null;


        #region DateRangeAbs
        private static int DateRangeAbs(DateTime? dateTime1, DateTime? dateTime2)
        {
            if (dateTime1 == null && dateTime2 == null) return 0;
            if (dateTime1 != null && dateTime2 == null) return int.MaxValue;
            if (dateTime1 == null && dateTime2 != null) return int.MaxValue;

            return (int)Math.Abs((
                new DateTime(((DateTime)dateTime1).Year, ((DateTime)dateTime1).Month, ((DateTime)dateTime1).Day) -
                new DateTime(((DateTime)dateTime2).Year, ((DateTime)dateTime2).Month, ((DateTime)dateTime2).Day)
                ).TotalDays);
        }
        #endregion

        #region IsMatch
        public static bool IsMatch(GroupMacth groupMacthSource, GroupMacth groupMacthCheckWith, 
            bool checkFileCreated, bool checkMediaTaken, bool checkAllDates, int maxDayRange,
            bool checkLocationName, bool checkLocationCity, bool checkLocationDistrict, bool checkLocationCountry, bool checkAllLocations)
        {
            bool isItemsEqual = true;

            if (groupMacthSource.IsMetadataNull != groupMacthCheckWith.IsMetadataNull) isItemsEqual = false;

            bool didAnyCheckedDatesMissmatch = false;
            bool didAnyCheckedDatesMatch = false;
            if (checkFileCreated) if (DateRangeAbs(groupMacthCheckWith.FileDate, groupMacthSource.FileDate) > maxDayRange) didAnyCheckedDatesMissmatch = true; else didAnyCheckedDatesMatch = true;
            if (checkMediaTaken) if (DateRangeAbs(groupMacthCheckWith.MediaTaken, groupMacthSource.MediaTaken) > maxDayRange) didAnyCheckedDatesMissmatch = true; else didAnyCheckedDatesMatch = true;
            if (checkAllDates && didAnyCheckedDatesMissmatch) isItemsEqual = false;
            if (checkFileCreated || checkMediaTaken) if (!checkAllDates && !didAnyCheckedDatesMatch) isItemsEqual = false;

            bool didAnyCheckedLocationMissmatch = false;
            bool didAnyCheckedLocationMatch = false;
            if (checkLocationName) if (groupMacthCheckWith.LocationName != groupMacthSource.LocationName) didAnyCheckedLocationMissmatch = true; else didAnyCheckedLocationMatch = true;
            if (checkLocationCity) if (groupMacthCheckWith.LocationCity != groupMacthSource.LocationCity) didAnyCheckedLocationMissmatch = true; else didAnyCheckedLocationMatch = true;
            if (checkLocationDistrict) if (groupMacthCheckWith.LocationDistrict != groupMacthSource.LocationDistrict) didAnyCheckedLocationMissmatch = true; else didAnyCheckedLocationMatch = true;
            if (checkLocationCountry) if (groupMacthCheckWith.LocationCountry != groupMacthSource.LocationCountry) didAnyCheckedLocationMissmatch = true; else didAnyCheckedLocationMatch = true;
            if (checkAllLocations && didAnyCheckedLocationMissmatch) isItemsEqual = false;
            if (checkLocationName || checkLocationCity || checkLocationDistrict || checkLocationCountry) if (!checkAllLocations && !didAnyCheckedLocationMatch) isItemsEqual = false;

            return isItemsEqual;
        }
        #endregion
    }
    #endregion 
}
