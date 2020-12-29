using DataGridViewGeneric;
using FastColoredTextBoxNS;
using MetadataLibrary;
using MetadataPriorityLibrary;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{


    public partial class Config : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public MetadataReadPrioity MetadataReadPrioity { get; set; } //= new MetadataReadPrioity();
        public Size[] ThumbnailSizes { get; set; }

        private Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionaryCopy = new Dictionary<MetadataPriorityKey, MetadataPriorityValues>();
        private AutoCorrect autoCorrect = new AutoCorrect();

        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeywordAdd = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeuwordDelete = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeywordWriteTags = null;

        private bool isPopulation = false;

        public Config()
        {
            InitializeComponent();

        }

        #region All tabs - Init - Save - Close
        public void Init()
        {
            isPopulation = true;
            //PopulateApplication()
            PopulateApplication();

            //Metadata Filename Date formats
            fastColoredTextBoxConfigFilenameDateFormats.Text = Properties.Settings.Default.RenameDateFormats;

            //Metadata Read
            PopulateMetadataReadToolStripMenu();
            CopyMetadataReadPrioity(MetadataReadPrioity.MetadataPrioityDictionary, metadataPrioityDictionaryCopy);
            PopulateMetadataRead(dataGridViewMetadataReadPriority);

            //AutoCorrect
            autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
            if (autoCorrect == null) autoCorrect = new AutoCorrect();
            PopulateAutoCorrectPoperties();

            //Metadata Write
            fastColoredTextBoxHandlerKeywordAdd = new FastColoredTextBoxHandler(fastColoredTextBoxMetadataWriteKeywordAdd, true, MetadataReadPrioity.MetadataPrioityDictionary);
            fastColoredTextBoxHandlerKeuwordDelete = new FastColoredTextBoxHandler(fastColoredTextBoxMetadataWriteKeywordDelete, true, MetadataReadPrioity.MetadataPrioityDictionary);
            fastColoredTextBoxHandlerKeywordWriteTags = new FastColoredTextBoxHandler(fastColoredTextBoxMetadataWriteTags, false, MetadataReadPrioity.MetadataPrioityDictionary);
            PopulateMetadataWritePoperties();
            isPopulation = false;

            //Show log
            string logFilename = GetLogFileName("logfile");
            if (string.IsNullOrWhiteSpace(logFilename)) logFilename = "PhotoTagsSynchronizer_Log.txt";

            if (File.Exists(logFilename))
            {
                fastColoredTextBoxShowLog.OpenBindingFile(logFilename, Encoding.UTF8);
                fastColoredTextBoxShowLog.IsChanged = false;
                fastColoredTextBoxShowLog.ClearUndo();
                GC.Collect();
                GC.GetTotalMemory(true);
            }

            logFilename = "Pipe\\WindowsLivePhotoGalleryServer_Log.txt";
            if (File.Exists(logFilename))
            {
                fastColoredTextBoxShowPipe32Log.OpenBindingFile(logFilename, Encoding.UTF8);
                fastColoredTextBoxShowPipe32Log.IsChanged = false;
                fastColoredTextBoxShowPipe32Log.ClearUndo();
                GC.Collect();
                GC.GetTotalMemory(true);
            }
        }

        #region Log - GetLogFileName(string targetName)
        private string GetLogFileName(string targetName)
        {
            string fileName = null;

            if (LogManager.Configuration != null && LogManager.Configuration.ConfiguredNamedTargets.Count != 0)
            {
                Target target = LogManager.Configuration.FindTargetByName(targetName);
                if (target == null)
                {
                    return null;
                    //throw new Exception("Could not find target named: " + targetName);
                }

                FileTarget fileTarget = null;
                WrapperTargetBase wrapperTarget = target as WrapperTargetBase;

                // Unwrap the target if necessary.
                if (wrapperTarget == null)
                {
                    fileTarget = target as FileTarget;
                }
                else
                {
                    fileTarget = wrapperTarget.WrappedTarget as FileTarget;
                }

                if (fileTarget == null)
                {
                    return null;
                    //throw new Exception("Could not get a FileTarget from " + target.GetType());
                }

                var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
                fileName = fileTarget.FileName.Render(logEventInfo);
            }
            else
            {
                return null;
                //throw new Exception("LogManager contains no Configuration or there are no named targets");
            }

            /*if (!File.Exists(fileName))
            {
                throw new Exception("File " + fileName + " does not exist");
            }*/

            return fileName;
        }
        #endregion 

        private void Config_Load(object sender, EventArgs e)
        {
            dataGridViewMetadataReadPriority.Focus();
        }

        private void buttonConfigSave_Click(object sender, EventArgs e)
        {
            //Application
            Properties.Settings.Default.ApplicationThumbnail = ThumbnailSizes[comboBoxApplicationThumbnailSizes.SelectedIndex];
            Properties.Settings.Default.ApplicationPreferredLanguages = textBoxApplicationPreferredLanguages.Text;
            Properties.Settings.Default.MaxRowsInSearchResult = (int)numericUpDownApplicationMaxRowsInSearchResult.Value;

            //AutoCorrect
            GetAutoCorrectPoperties();
            Properties.Settings.Default.AutoCorrect = autoCorrect.SerializeThis();

            //Metadata Write
            Properties.Settings.Default.WriteMetadataTags = fastColoredTextBoxMetadataWriteTags.Text;
            Properties.Settings.Default.WriteMetadataKeywordAdd = fastColoredTextBoxMetadataWriteKeywordAdd.Text;
            Properties.Settings.Default.WriteMetadataKeywordDelete = fastColoredTextBoxMetadataWriteKeywordDelete.Text;

            Properties.Settings.Default.XtraAtomAlbumVideo = checkBoxWriteXtraAtomAlbumVideo.Checked;
            Properties.Settings.Default.XtraAtomCategoriesVideo = checkBoxWriteXtraAtomCategoriesVideo.Checked;
            Properties.Settings.Default.XtraAtomCommentPicture = checkBoxWriteXtraAtomCommentPicture.Checked;
            Properties.Settings.Default.XtraAtomCommentVideo = checkBoxWriteXtraAtomCommentVideo.Checked;
            Properties.Settings.Default.XtraAtomKeywordsVideo = checkBoxWriteXtraAtomKeywordsVideo.Checked;
            Properties.Settings.Default.XtraAtomRatingPicture = checkBoxWriteXtraAtomRatingPicture.Checked;
            Properties.Settings.Default.XtraAtomRatingVideo = checkBoxWriteXtraAtomRatingVideo.Checked;
            Properties.Settings.Default.XtraAtomSubjectPicture = checkBoxWriteXtraAtomSubjectPicture.Checked;
            Properties.Settings.Default.XtraAtomSubjectVideo = checkBoxWriteXtraAtomSubjectVideo.Checked;
            Properties.Settings.Default.XtraAtomSubtitleVideo = checkBoxWriteXtraAtomSubtitleVideo.Checked;
            Properties.Settings.Default.XtraAtomArtistVideo = checkBoxWriteXtraAtomArtistVideo.Checked;

            Properties.Settings.Default.XtraAtomArtistVariable = textBoxWriteXtraAtomArtist.Text;
            Properties.Settings.Default.XtraAtomAlbumVariable = textBoxWriteXtraAtomAlbum.Text;
            Properties.Settings.Default.XtraAtomCategoriesVariable = textBoxWriteXtraAtomCategories.Text;
            Properties.Settings.Default.XtraAtomCommentVariable = textBoxWriteXtraAtomComment.Text;
            Properties.Settings.Default.XtraAtomKeywordsVariable = textBoxWriteXtraAtomKeywords.Text;
            Properties.Settings.Default.XtraAtomSubjectVariable = textBoxWriteXtraAtomSubject.Text;
            Properties.Settings.Default.XtraAtomSubtitleVariable = textBoxWriteXtraAtomSubtitle.Text;


            //Filename date formates
            Properties.Settings.Default.RenameDateFormats = fastColoredTextBoxConfigFilenameDateFormats.Text;

            //Save config file
            Properties.Settings.Default.Save();

            //Metadata Read
            MetadataReadPrioity.MetadataPrioityDictionary = metadataPrioityDictionaryCopy;
            MetadataReadPrioity.WriteAlways();
            this.Close();
        }

        private void buttonConfigCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion 

        public void PopulateApplication()
        {
            for (int i = 0; i < ThumbnailSizes.Length; i++)
                comboBoxApplicationThumbnailSizes.Items.Add(ThumbnailSizes[i].ToString());

            comboBoxApplicationThumbnailSizes.Text = Properties.Settings.Default.ApplicationThumbnail.ToString();
            textBoxApplicationPreferredLanguages.Text = Properties.Settings.Default.ApplicationPreferredLanguages;
            numericUpDownApplicationMaxRowsInSearchResult.Value = Properties.Settings.Default.MaxRowsInSearchResult;
        }


        #region AutoCorrect - Populate and Save
        private void PopulateAutoCorrectListOrder(ImageListViewOrder imageListViewOrder, List<MetadataBrokerTypes> listPriority)
        {
            ListViewItem listViewItem;

            imageListViewOrder.Items.Clear();
            foreach (MetadataBrokerTypes metadataBroker in listPriority)
            {
                switch (metadataBroker)
                {
                    case MetadataBrokerTypes.ExifTool:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Exiftool";
                        listViewItem.Tag = MetadataBrokerTypes.ExifTool;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerTypes.MicrosoftPhotos:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "MicrosoftPhotos";
                        listViewItem.Tag = MetadataBrokerTypes.MicrosoftPhotos;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerTypes.WindowsLivePhotoGallery:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Windows Live Photo Gallery";
                        listViewItem.Tag = MetadataBrokerTypes.WindowsLivePhotoGallery;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerTypes.FileSystem:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Subfolder name";
                        listViewItem.Tag = MetadataBrokerTypes.FileSystem;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                }
            }
            imageListViewOrder.AutoResize();
        }

        private void PopulateAutoCorrectDateTakenPriority(ImageListViewOrder imageListViewOrder, List<DateTimeSources> listPriority)
        {
            ListViewItem listViewItem;

            imageListViewOrder.Items.Clear();
            foreach (DateTimeSources dateTimeSource in listPriority)
            {
                switch (dateTimeSource)
                {
                    case DateTimeSources.DateTaken:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Date&Time Taken";
                        listViewItem.Tag = DateTimeSources.DateTaken;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case DateTimeSources.GPSDateAndTime:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "GPS UTC DateTime";
                        listViewItem.Tag = DateTimeSources.GPSDateAndTime;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case DateTimeSources.FirstDateFoundInFilename:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "First Date&Time found in Filename";
                        listViewItem.Tag = DateTimeSources.FirstDateFoundInFilename;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case DateTimeSources.LastDateFoundInFilename:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Last Date&Time found in Filename";
                        listViewItem.Tag = DateTimeSources.LastDateFoundInFilename;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                }
            }
            imageListViewOrder.AutoResize();
        }

        private void PopulateAutoCorrectPoperties()
        {
            #region Date Taken
            if (autoCorrect.DateTakenPriority == null || autoCorrect.DateTakenPriority.Count == 0)
            {
                autoCorrect.DateTakenPriority.Add(DateTimeSources.DateTaken);
                autoCorrect.DateTakenPriority.Add(DateTimeSources.GPSDateAndTime);
                autoCorrect.DateTakenPriority.Add(DateTimeSources.FirstDateFoundInFilename);
                autoCorrect.DateTakenPriority.Add(DateTimeSources.LastDateFoundInFilename);
            }

            PopulateAutoCorrectDateTakenPriority(imageListViewOrderDateTaken, autoCorrect.DateTakenPriority);

            if (autoCorrect.UpdateDateTaken)
            {
                if (autoCorrect.UpdateDateTakenWithFirstInPrioity)
                    radioButtonDateTakenUseFirst.Checked = true;
                else
                    radioButtonDateTakenChangeWhenEmpty.Checked = true;
            }
            else radioButtonDateTakenDoNotChange.Checked = true;

            #endregion

            #region Title            
            if (autoCorrect.TitlePriority == null || autoCorrect.TitlePriority.Count == 0)
            {
                autoCorrect.TitlePriority.Add(MetadataBrokerTypes.ExifTool);
                autoCorrect.TitlePriority.Add(MetadataBrokerTypes.MicrosoftPhotos);
                autoCorrect.TitlePriority.Add(MetadataBrokerTypes.WindowsLivePhotoGallery);
            }

            PopulateAutoCorrectListOrder(imageListViewOrderTitle, autoCorrect.TitlePriority);

            if (autoCorrect.UpdateTitle)
            {
                if (autoCorrect.UpdateTitleWithFirstInPrioity)
                    radioButtonTitleUseFirst.Checked = true;
                else
                    radioButtonTitleChangeWhenEmpty.Checked = true;
            }
            else radioButtonTitleDoNotChange.Checked = true;

            #endregion

            #region Album
            if (autoCorrect.AlbumPriority == null || autoCorrect.AlbumPriority.Count == 0)
            {
                autoCorrect.AlbumPriority.Add(MetadataBrokerTypes.ExifTool);
                autoCorrect.AlbumPriority.Add(MetadataBrokerTypes.MicrosoftPhotos);
                autoCorrect.AlbumPriority.Add(MetadataBrokerTypes.FileSystem);
            }
            PopulateAutoCorrectListOrder(imageListViewOrderAlbum, autoCorrect.AlbumPriority);

            if (autoCorrect.UpdateAlbum)
            {
                if (autoCorrect.UpdateAlbumWithFirstInPrioity)
                    radioButtonAlbumUseFirst.Checked = true;
                else
                    radioButtonAlbumChangeWhenEmpty.Checked = true;
            }
            else radioButtonAlbumDoNotChange.Checked = true;

            #endregion

            #region Keywords
            checkBoxKeywordsAddMicrosoftPhotos.Checked = autoCorrect.UseKeywordsFromMicrosoftPhotos;
            checkBoxKeywordsAddWindowsMediaPhotoGallery.Checked = autoCorrect.UseKeywordsFromWindowsLivePhotoGallery;
            comboBoxKeywordsAiConfidence.SelectedIndex = 9 - (int)(autoCorrect.KeywordTagConfidenceLevel * 10);

            checkBoxKeywordBackupDateTakenAfter.Checked = autoCorrect.BackupDateTakenAfterUpdate;
            checkBoxKeywordBackupDateTakenBefore.Checked = autoCorrect.BackupDateTakenBeforeUpdate;
            checkBoxKeywordBackupGPSDateTimeUTCAfter.Checked = autoCorrect.BackupGPGDateTimeUTCAfterUpdate;
            checkBoxKeywordBackupGPSDateTimeUTCBefore.Checked = autoCorrect.BackupGPGDateTimeUTCBeforeUpdate;
            checkBoxKeywordBackupLocationCity.Checked = autoCorrect.BackupLocationCity;
            checkBoxKeywordBackupLocationCountry.Checked = autoCorrect.BackupLocationCountry;
            checkBoxKeywordBackupLocationName.Checked = autoCorrect.BackupLocationName;
            checkBoxKeywordBackupLocationState.Checked = autoCorrect.BackupLocationState;
            checkBoxKeywordBackupRegionFaceNames.Checked = autoCorrect.BackupRegionFaceNames;
            #endregion

            #region Region Faces
            checkBoxFaceRegionAddMicrosoftPhotos.Checked = autoCorrect.UseFaceRegionFromMicrosoftPhotos;
            checkBoxFaceRegionAddWindowsMediaPhotoGallery.Checked = autoCorrect.UseFaceRegionFromWindowsLivePhotoGallery;
            #endregion

            #region Author
            if (!autoCorrect.UpdateAuthor) radioButtonAuthorDoNotChange.Checked = true;
            else if (autoCorrect.UpdateAuthorOnlyWhenEmpty) radioButtonAuthorChangeWhenEmpty.Checked = true;
            else radioButtonAuthorAlwaysChange.Checked = true;
            #endregion

            #region GPS Location and Date&Time
            checkBoxGPSUpdateLocation.Checked = autoCorrect.UpdateGPSLocation;
            checkBoxGPSUpdateDateTime.Checked = autoCorrect.UpdateGPSDateTime;
            numericUpDownLocationGuessInterval.Value = autoCorrect.LocationTimeZoneGuessHours;
            numericUpDownLocationAccurateInterval.Value = autoCorrect.LocationFindMinutes;
            #endregion

            #region Location Name, State, City, Country
            if (!autoCorrect.UpdateLocation) radioButtonLocationNameDoNotChange.Checked = true;
            else if (autoCorrect.UpdateLocationOnlyWhenEmpty) radioButtonLocationNameChangeWhenEmpty.Checked = true;
            else radioButtonLocationNameChangeAlways.Checked = true;

            checkBoxUpdateLocationName.Checked = autoCorrect.UpdateLocationName;
            checkBoxUpdateLocationCity.Checked = autoCorrect.UpdateLocationCity;
            checkBoxUpdateLocationState.Checked = autoCorrect.UpdateLocationState;
            checkBoxUpdateLocationCountry.Checked = autoCorrect.UpdateLocationCountry;
            #endregion
        }

        private void GetAutoCorrectPoperties()
        {
            #region DateTaken
            autoCorrect.DateTakenPriority.Clear();
            foreach (ListViewItem item in imageListViewOrderDateTaken.Items)
            {
                autoCorrect.DateTakenPriority.Add((DateTimeSources)item.Tag);
            }

            if (radioButtonDateTakenDoNotChange.Checked)
            {
                autoCorrect.UpdateDateTaken = false;
                autoCorrect.UpdateDateTakenWithFirstInPrioity = false;
            }
            else
            {
                autoCorrect.UpdateTitle = true;

                if (radioButtonDateTakenUseFirst.Checked)
                    autoCorrect.UpdateDateTakenWithFirstInPrioity = true;
                else
                    autoCorrect.UpdateDateTakenWithFirstInPrioity = false;
            }
            #endregion 

            #region Title
            autoCorrect.TitlePriority.Clear();
            foreach (ListViewItem item in imageListViewOrderTitle.Items)
            {
                autoCorrect.TitlePriority.Add((MetadataBrokerTypes)item.Tag);
            }

            if (radioButtonTitleDoNotChange.Checked)
            {
                autoCorrect.UpdateTitle = false;
                autoCorrect.UpdateTitleWithFirstInPrioity = false;
            }
            else
            {
                autoCorrect.UpdateTitle = true;

                if (radioButtonTitleUseFirst.Checked)
                    autoCorrect.UpdateTitleWithFirstInPrioity = true;
                else
                    autoCorrect.UpdateTitleWithFirstInPrioity = false;
            }
            #endregion

            #region Album
            autoCorrect.AlbumPriority.Clear();
            foreach (ListViewItem item in imageListViewOrderAlbum.Items)
            {
                autoCorrect.AlbumPriority.Add((MetadataBrokerTypes)item.Tag);
            }

            if (radioButtonAlbumDoNotChange.Checked)
            {
                autoCorrect.UpdateAlbum = false;
                autoCorrect.UpdateAlbumWithFirstInPrioity = false;
            }
            else
            {
                autoCorrect.UpdateAlbum = true;

                if (radioButtonTitleUseFirst.Checked)
                    autoCorrect.UpdateAlbumWithFirstInPrioity = true;
                else
                    autoCorrect.UpdateAlbumWithFirstInPrioity = false;
            }

            autoCorrect.KeywordTagConfidenceLevel = (90 - comboBoxKeywordsAiConfidence.SelectedIndex * 10) / 100.0;
            #endregion

            #region Keywords
            autoCorrect.UseKeywordsFromMicrosoftPhotos = checkBoxKeywordsAddMicrosoftPhotos.Checked;
            autoCorrect.UseKeywordsFromWindowsLivePhotoGallery = checkBoxKeywordsAddWindowsMediaPhotoGallery.Checked;

            autoCorrect.BackupDateTakenAfterUpdate = checkBoxKeywordBackupDateTakenAfter.Checked;
            autoCorrect.BackupDateTakenBeforeUpdate = checkBoxKeywordBackupDateTakenBefore.Checked;
            autoCorrect.BackupGPGDateTimeUTCAfterUpdate = checkBoxKeywordBackupGPSDateTimeUTCAfter.Checked;
            autoCorrect.BackupGPGDateTimeUTCBeforeUpdate = checkBoxKeywordBackupGPSDateTimeUTCBefore.Checked;
            autoCorrect.BackupLocationCity = checkBoxKeywordBackupLocationCity.Checked;
            autoCorrect.BackupLocationCountry = checkBoxKeywordBackupLocationCountry.Checked;
            autoCorrect.BackupLocationName = checkBoxKeywordBackupLocationName.Checked;
            autoCorrect.BackupLocationState = checkBoxKeywordBackupLocationState.Checked;
            autoCorrect.BackupRegionFaceNames = checkBoxKeywordBackupRegionFaceNames.Checked;
            #endregion

            #region Region Faces
            autoCorrect.UseFaceRegionFromMicrosoftPhotos = checkBoxFaceRegionAddMicrosoftPhotos.Checked;
            autoCorrect.UseFaceRegionFromWindowsLivePhotoGallery = checkBoxFaceRegionAddWindowsMediaPhotoGallery.Checked;
            #endregion

            #region Author
            if (radioButtonAuthorDoNotChange.Checked)
            {
                autoCorrect.UpdateAuthor = false;
                autoCorrect.UpdateAuthorOnlyWhenEmpty = false;
            }
            else
            {
                autoCorrect.UpdateAuthor = true;

                if (radioButtonAuthorChangeWhenEmpty.Checked)
                    autoCorrect.UpdateAuthorOnlyWhenEmpty = true;
                else
                    autoCorrect.UpdateAuthorOnlyWhenEmpty = false;
            }
            #endregion

            #region GPS Location and Date&Time
            autoCorrect.UpdateGPSLocation = checkBoxGPSUpdateLocation.Checked;
            autoCorrect.UpdateGPSDateTime = checkBoxGPSUpdateDateTime.Checked;
            autoCorrect.LocationTimeZoneGuessHours = (int)numericUpDownLocationGuessInterval.Value;
            autoCorrect.LocationFindMinutes = (int)numericUpDownLocationAccurateInterval.Value;
            #endregion

            #region Location            
            if (radioButtonLocationNameDoNotChange.Checked)
            {
                autoCorrect.UpdateLocation = false;
                autoCorrect.UpdateLocationOnlyWhenEmpty = false;
            }
            else
            {
                autoCorrect.UpdateLocation = true;

                if (radioButtonLocationNameChangeWhenEmpty.Checked)
                    autoCorrect.UpdateLocationOnlyWhenEmpty = true;
                else
                    autoCorrect.UpdateLocationOnlyWhenEmpty = false;
            }

            autoCorrect.UpdateLocationName = checkBoxUpdateLocationName.Checked;
            autoCorrect.UpdateLocationCity = checkBoxUpdateLocationCity.Checked;
            autoCorrect.UpdateLocationState = checkBoxUpdateLocationState.Checked;
            autoCorrect.UpdateLocationCountry = checkBoxUpdateLocationCountry.Checked;
            #endregion
        }
        #endregion

        #region Metadata Read - Populate
        private void PopulateMetadataReadToolStripMenu()
        {
            SortedDictionary<string, string> listAllTags = new CompositeTags().ListAllTags();
            foreach (KeyValuePair<string, string> tag in listAllTags.OrderBy(key => key.Value))
            {
                ToolStripMenuItem newTagItem = new ToolStripMenuItem();
                newTagItem.Name = tag.Value;
                newTagItem.Size = new System.Drawing.Size(224, 26);
                newTagItem.Text = tag.Value;
                newTagItem.Click += new System.EventHandler(this.ToolStripMenuItemMoveAndAssign_Click);
                this.toolStripMenuItemMetadataReadMove.DropDownItems.Add(newTagItem);
            }
        }

        public void CopyMetadataReadPrioity(Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionarySource,
            Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionaryDestination)
        {
            metadataPrioityDictionaryCopy = new Dictionary<MetadataPriorityKey, MetadataPriorityValues>();
            foreach (KeyValuePair<MetadataPriorityKey, MetadataPriorityValues> keyValuePair in metadataPrioityDictionarySource)
            {
                metadataPrioityDictionaryCopy.Add(new MetadataPriorityKey(keyValuePair.Key), new MetadataPriorityValues(keyValuePair.Value));
            }
        }


        private void PopulateMetadataRead(DataGridView dataGridView)
        {
            isCellValueUpdating = true;
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridView, "Name", "Tags", DataGridViewSize.ConfigSize);
            DataGridViewHandler.Clear(dataGridView, DataGridViewSize.ConfigSize);
            //contextMenuStripMetadataRead contextMenuStripMetadataRead

            DateTime dateTimeEditable = DateTime.Now;

            int columnIndex1 = DataGridViewHandler.AddColumnOrUpdate(dataGridView,
                new FileEntryImage("Priority", dateTimeEditable), //Heading
                    null, dateTimeEditable,
                    ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerTypes.Empty, SwitchStates.Off, true));

            List<string> compositeList = new List<string>();

            List<MetadataPriorityGroup> metadataPrioityGroupSortedList = new List<MetadataPriorityGroup>();
            foreach (MetadataPriorityKey metadataPriorityKey in metadataPrioityDictionaryCopy.Keys)
            {
                metadataPrioityGroupSortedList.Add(new MetadataPriorityGroup(metadataPriorityKey, metadataPrioityDictionaryCopy[metadataPriorityKey]));
            }
            metadataPrioityGroupSortedList.Sort(); // (x, y) => x.CompareTo(y));

            foreach (MetadataPriorityGroup metadataPrioityGroup in metadataPrioityGroupSortedList)
            {
                if (!compositeList.Contains(metadataPrioityGroup.MetadataPriorityValues.Composite))
                {
                    compositeList.Add(metadataPrioityGroup.MetadataPriorityValues.Composite);
                    DataGridViewHandler.AddRow(dataGridView, columnIndex1, new DataGridViewGenericRow(metadataPrioityGroup.MetadataPriorityValues.Composite));
                }
            }

            foreach (MetadataPriorityGroup metadataPrioityGroup in metadataPrioityGroupSortedList)
            {
                DataGridViewHandler.AddRow(dataGridView, columnIndex1, new DataGridViewGenericRow(
                    metadataPrioityGroup.MetadataPriorityValues.Composite,
                    metadataPrioityGroup.MetadataPriorityKey.Region + " | " + metadataPrioityGroup.MetadataPriorityKey.Tag,
                    metadataPrioityGroup.MetadataPriorityKey),
                    metadataPrioityGroup.MetadataPriorityValues.Priority, false);
            }
            isCellValueUpdating = false;
        }
        #endregion

        #region Metadata Read - DataGridView Assign when ToolStripMenuItemMoveAndAssign_Click
        public void AssignSelectedToNewTag(DataGridView dataGridView, Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionary, string composite)
        {
            List<int> rowSelected = DataGridViewHandler.GetRowSelected(dataGridView);

            foreach (int rowIndex in rowSelected)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                {
                    MetadataPriorityValues metadataPriorityValues = metadataPrioityDictionary[dataGridViewGenericRow.MetadataPriorityKey];
                    metadataPriorityValues.Composite = composite;
                }
            }
        }

        private void ToolStripMenuItemMoveAndAssign_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tagSender = (ToolStripMenuItem)sender;
            //DataGridViewHandler.
            AssignSelectedToNewTag(dataGridViewMetadataReadPriority, metadataPrioityDictionaryCopy, tagSender.Text);
            PopulateMetadataRead(dataGridViewMetadataReadPriority);
        }
        #endregion

        #region Metadata Read - Drag and Drop
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private void dataGridViewMetadataReadPriority_MouseMove(object sender, MouseEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dataGridView.DoDragDrop(dataGridView.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
        }

        private void dataGridViewMetadataReadPriority_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dataGridView.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndexFromMouseDown);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                {
                    // Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // Create a rectangle using the DragSize, with the mouse position being at the center of the rectangle.
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
                else
                {
                    rowIndexFromMouseDown = -1;
                    dragBoxFromMouseDown = Rectangle.Empty;
                }
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dataGridViewMetadataReadPriority_DragDrop(object sender, DragEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop = dataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {


                DataGridViewGenericRow dataGridViewGenericRowFrom = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndexFromMouseDown);
                DataGridViewGenericRow dataGridViewGenericRowTo = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndexOfItemUnderMouseToDrop);

                if (dataGridViewGenericRowFrom != null && !dataGridViewGenericRowFrom.IsHeader &&
                    dataGridViewGenericRowTo != null && dataGridViewGenericRowTo.IsHeader)
                {

                    MetadataPriorityValues metadataPriorityValues = metadataPrioityDictionaryCopy[dataGridViewGenericRowFrom.MetadataPriorityKey];
                    metadataPriorityValues.Composite = dataGridViewGenericRowTo.HeaderName;
                }
                int toRowIndex = rowIndexOfItemUnderMouseToDrop + (rowIndexFromMouseDown < rowIndexOfItemUnderMouseToDrop ? 0 : 1);

                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridView.Rows.RemoveAt(rowIndexFromMouseDown);
                dataGridView.Rows.Insert(toRowIndex, rowToMove);
                dataGridView.CurrentCell = DataGridViewHandler.GetCellDataGridViewCell(dataGridView, 0, toRowIndex);
            }

        }

        private void dataGridViewMetadataReadPriority_DragOver(object sender, DragEventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            Point clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));
            // Get the row index of the item the mouse is below. 
            int rowIndex = dataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (dataGridViewGenericRow != null && dataGridViewGenericRow.IsHeader)
            {
                e.Effect = DragDropEffects.Move;
            }
            else e.Effect = DragDropEffects.None;
        }
        #endregion

        #region Metadata Read - Cell Changed
        private bool isCellValueUpdating = false;
        private void dataGridViewMetadataReadPriority_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isCellValueUpdating) return;
            DataGridView dataGridView = (DataGridView)sender;
            string value = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, e.ColumnIndex, e.RowIndex);
            if (int.TryParse(value.ToString(), out int priority))
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader)
                {
                    MetadataPriorityValues metadataPriorityValues = metadataPrioityDictionaryCopy[dataGridViewGenericRow.MetadataPriorityKey];
                    metadataPriorityValues.Priority = priority;
                }
            }
            else
            {
                isCellValueUpdating = true;
                DataGridViewHandler.SetCellValue(dataGridView, e.ColumnIndex, e.RowIndex, 100);
                isCellValueUpdating = false;
            }
        }
        #endregion

        #region Metadata Read - Keydown and Item Click, Clipboard
        private void dataGridViewMetadataReadPriority_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridViewHandler.KeyDownEventHandler(sender, e);
        }

        private void toolStripMenuItemMetadataReadCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }

        private void toolStripMenuItemMetadataReadCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView);
        }

        private void toolStripMenuItemMetadataReadPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
        }

        private void toolStripMenuItemMetadataReadDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }

        private void toolStripMenuItemMetadataReadUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.UndoDataGridView(dataGridView);
        }

        private void toolStripMenuItemMetadataReadRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.RedoDataGridView(dataGridView);
        }

        private void toolStripMenuItemMetadataReadFind_Click(object sender, EventArgs e)
        {
            //string header = DataGridViewHandlerX.headerKeywords;
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }

        private void toolStripMenuItemMetadataReadReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }

        private void toolStripMenuItemMetadataReadMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }

        private void toolStripMenuItemMetadataReadRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }

        private void toolStripMenuItemMetadataReadToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }

        private void toolStripMenuItemMetadataReadShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionToggleStripMenuItem(dataGridView, toolStripMenuItemMetadataReadShowFavorite);
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, false, toolStripMenuItemMetadataReadShowFavorite.Checked);
        }

        private void dataGridViewMetadataReadPriority_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #region Metadata Read - CellPaining 
        private void dataGridViewMetadataReadPriority_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;

            //DataGridViewUpdateThumbnail(dataGridView, e);
            DataGridViewHandler.CellPaintingHandleDefault(sender, e);
            //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region AutoCorrect - Updated label when value changes
        private void numericUpDownLocationGuessInterval_ValueChanged(object sender, EventArgs e)
        {
            labelLocationTimeZoneGuess.Text = "2. Try find location in camera owner's history, ±" + (int)numericUpDownLocationGuessInterval.Value + " hours";
        }

        private void numericUpDownLocationAccurateInterval_ValueChanged(object sender, EventArgs e)
        {
            labelLocationTimeZoneAccurate.Text = "5. Find new locations in camra owner's hirstory. ±" + (int)numericUpDownLocationAccurateInterval.Value + " minutes";
        }
        #endregion

        #region Metadata Write - Populate Window
        private void PopulateMetadataWritePoperties()
        {
            comboBoxMetadataWriteStandardTags.Items.AddRange(Metadata.ListOfProperties(false));
            comboBoxWriteXtraAtomVariables.Items.AddRange(Metadata.ListOfProperties(false));
            comboBoxMetadataWriteKeywordDelete.Items.AddRange(Metadata.ListOfProperties(true));
            comboBoxMetadataWriteKeywordAdd.Items.AddRange(Metadata.ListOfProperties(true));

            fastColoredTextBoxMetadataWriteTags.Text = Properties.Settings.Default.WriteMetadataTags;
            fastColoredTextBoxMetadataWriteKeywordAdd.Text = Properties.Settings.Default.WriteMetadataKeywordAdd;
            fastColoredTextBoxMetadataWriteKeywordDelete.Text = Properties.Settings.Default.WriteMetadataKeywordDelete;

            checkBoxWriteXtraAtomAlbumVideo.Checked = Properties.Settings.Default.XtraAtomAlbumVideo;
            checkBoxWriteXtraAtomCategoriesVideo.Checked = Properties.Settings.Default.XtraAtomCategoriesVideo;
            checkBoxWriteXtraAtomCommentPicture.Checked = Properties.Settings.Default.XtraAtomCommentPicture;
            checkBoxWriteXtraAtomCommentVideo.Checked = Properties.Settings.Default.XtraAtomCommentVideo;
            checkBoxWriteXtraAtomKeywordsVideo.Checked = Properties.Settings.Default.XtraAtomKeywordsVideo;
            checkBoxWriteXtraAtomRatingPicture.Checked = Properties.Settings.Default.XtraAtomRatingPicture;
            checkBoxWriteXtraAtomRatingVideo.Checked = Properties.Settings.Default.XtraAtomRatingVideo;
            checkBoxWriteXtraAtomSubjectPicture.Checked = Properties.Settings.Default.XtraAtomSubjectPicture;
            checkBoxWriteXtraAtomSubjectVideo.Checked = Properties.Settings.Default.XtraAtomSubjectVideo;
            checkBoxWriteXtraAtomSubtitleVideo.Checked = Properties.Settings.Default.XtraAtomSubtitleVideo;
            checkBoxWriteXtraAtomArtistVideo.Checked = Properties.Settings.Default.XtraAtomArtistVideo;

            textBoxWriteXtraAtomArtist.Text = Properties.Settings.Default.XtraAtomArtistVariable;
            textBoxWriteXtraAtomAlbum.Text = Properties.Settings.Default.XtraAtomAlbumVariable;
            textBoxWriteXtraAtomCategories.Text = Properties.Settings.Default.XtraAtomCategoriesVariable;
            textBoxWriteXtraAtomComment.Text = Properties.Settings.Default.XtraAtomCommentVariable;
            textBoxWriteXtraAtomKeywords.Text = Properties.Settings.Default.XtraAtomKeywordsVariable;
            textBoxWriteXtraAtomSubject.Text = Properties.Settings.Default.XtraAtomSubjectVariable;
            textBoxWriteXtraAtomSubtitle.Text = Properties.Settings.Default.XtraAtomSubtitleVariable;
        }
        #endregion

        #region Metadata Write - Insert Variable from after Selected in Combobox
        private void comboBoxMetadataWriteStandardTags_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxMetadataWriteTags, comboBoxMetadataWriteStandardTags.Text);
        }

        private void comboBoxMetadataWriteKeywordAdd_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxMetadataWriteKeywordAdd, comboBoxMetadataWriteKeywordAdd.Text);
        }

        private void comboBoxMetadataWriteKeywordDelete_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxMetadataWriteKeywordDelete, comboBoxMetadataWriteKeywordDelete.Text);
        }

        private void comboBoxApplicationLanguages_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var insertText = comboBoxApplicationLanguages.Text.Split(' ', '\t')[0];
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(textBoxApplicationPreferredLanguages, insertText);
        }

        private void comboBoxWriteXtraAtomVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(activeXtraAtomTextbox, comboBoxWriteXtraAtomVariables.Text);
        }
        #endregion

        #region Metadata Write - Set Active XtraAtom Textbox
        TextBox activeXtraAtomTextbox = null;
        private void textBoxWriteXtraAtomKeywords_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (TextBox)sender;
        }

        private void textBoxWriteXtraAtomCategories_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (TextBox)sender;
        }

        private void textBoxWriteXtraAtomAlbum_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (TextBox)sender;
        }

        private void textBoxWriteXtraAtomSubtitle_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (TextBox)sender;
        }

        private void textBoxWriteXtraAtomSubject_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (TextBox)sender;
        }

        private void textBoxWriteXtraAtomComment_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (TextBox)sender;
        }

        private void textBoxWriteXtraAtomArtist_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (TextBox)sender;
        }
        #endregion

        #region FastColoredTextBox - events

        private void fastColoredTextBoxMetadataWriteKeywordDelete_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeuwordDelete != null) fastColoredTextBoxHandlerKeuwordDelete.SyntaxHighlightProperties(sender, e);
        }

        private void fastColoredTextBoxMetadataWriteKeywordAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordAdd != null) fastColoredTextBoxHandlerKeywordAdd.SyntaxHighlightProperties(sender, e);
        }

        private void fastColoredTextBoxMetadataWriteTags_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordWriteTags != null) fastColoredTextBoxHandlerKeywordWriteTags.SyntaxHighlightProperties(sender, e);
        }

        private void fastColoredTextBoxMetadataWriteKeywordDelete_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeuwordDelete != null) fastColoredTextBoxHandlerKeuwordDelete.KeyDown(sender, e);
        }

        private void fastColoredTextBoxMetadataWriteKeywordAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordAdd != null) fastColoredTextBoxHandlerKeywordAdd.KeyDown(sender, e);
        }

        private void fastColoredTextBoxMetadataWriteTags_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordWriteTags != null) fastColoredTextBoxHandlerKeywordWriteTags.KeyDown(sender, e);
        }
        #endregion 

        #region Log - App

        private void fastColoredTextBoxShowLog_WordWrapNeeded(object sender, FastColoredTextBoxNS.WordWrapNeededEventArgs e)
        {
            FastColoredTextBoxHandler.WordWrapNeededLog(sender, e);
        }

        private void fastColoredTextBoxShowLog_TextChanged(object sender, TextChangedEventArgs e)
        {
            FastColoredTextBoxHandler.SyntaxHighlightLog(fastColoredTextBoxShowLog);
        }

        private void fastColoredTextBoxShowLog_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            FastColoredTextBoxHandler.SyntaxHighlightLog(fastColoredTextBoxShowLog);
        }

        private void fastColoredTextBoxShowLog_VisibleRangeChangedDelayed(object sender, EventArgs e)
        {
            FastColoredTextBoxHandler.SyntaxHighlightLog(fastColoredTextBoxShowLog);
        }
        #endregion

        #region Log - Pipe 32
        private void fastColoredTextBoxShowPipe32Log_TextChanged(object sender, TextChangedEventArgs e)
        {
            FastColoredTextBoxHandler.SyntaxHighlightLog(fastColoredTextBoxShowPipe32Log);
        }

        private void fastColoredTextBoxShowPipe32Log_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            FastColoredTextBoxHandler.SyntaxHighlightLog(fastColoredTextBoxShowPipe32Log);
        }

        private void fastColoredTextBoxShowPipe32Log_VisibleRangeChangedDelayed(object sender, EventArgs e)
        {
            FastColoredTextBoxHandler.SyntaxHighlightLog(fastColoredTextBoxShowPipe32Log);
        }

        private void fastColoredTextBoxShowPipe32Log_WordWrapNeeded(object sender, WordWrapNeededEventArgs e)
        {
            FastColoredTextBoxHandler.WordWrapNeededLog(sender, e);
        }
        #endregion 
    }
}

/*
-Keywords-={KeywordItem}
-Subject-={KeywordItem}
-TagsList-={KeywordItem}
-CatalogSets-={KeywordItem}

-Keywords+={KeywordItem}
-Subject+={KeywordItem}
-TagsList+={KeywordItem}
-CatalogSets+={KeywordItem}


-charset
filename=UTF8
-overwrite_original
-m
-F
{IfLocationDateTimeChanged}-XMP-exif:GPSDateTime={LocationDateTimeUTC}
{IfLocationDateTimeChanged}-XMP:GPSDateTime={LocationDateTimeUTC}
{IfLocationDateTimeChanged}-GPS:GPSDateStamp={LocationDateTimeDateStamp}
{IfLocationDateTimeChanged}-GPS:GPSTimeStamp={LocationDateTimeTimeStamp}
{IfLocationDateTimeChanged}-GPSDateStamp={LocationDateTimeDateStamp}
{IfLocationDateTimeChanged}-GPSTimeStamp={LocationDateTimeTimeStamp}
{IfMediaDateTakenChanged}-Composite:SubSecCreateDate={MediaDateTaken}
{IfMediaDateTakenChanged}-EXIF:CreateDate={MediaDateTaken}
{IfMediaDateTakenChanged}-XMP-xmp:CreateDate={MediaDateTaken}
{IfMediaDateTakenChanged}-XMP:CreateDate={MediaDateTaken}
{IfMediaDateTakenChanged}-XMP:DateTimeOriginal={MediaDateTaken}
{IfMediaDateTakenChanged}-IPTC:DigitalCreationDate={MediaDateTakenDateStamp}
{IfMediaDateTakenChanged}-IPTC:DigitalCreationTime={MediaDateTakenTimeStamp}
{IfMediaDateTakenChanged}-Composite:SubSecDateTimeOriginal={MediaDateTaken}
{IfMediaDateTakenChanged}-ExifIFD:DateTimeOriginal={MediaDateTaken}
{IfMediaDateTakenChanged}-EXIF:DateTimeOriginal={MediaDateTaken}
{IfMediaDateTakenChanged}-XMP-photoshop:DateCreated={MediaDateTaken}
{IfMediaDateTakenChanged}-IPTC:DateCreated={MediaDateTakenDateStamp}
{IfMediaDateTakenChanged}-IPTC:TimeCreated={MediaDateTakenTimeStamp}
{IfMediaDateTakenChanged}-CreateDate={MediaDateTaken}
{IfPersonalAlbumChanged}-XMP-xmpDM:Album={PersonalAlbum}
{IfPersonalAlbumChanged}-XMP:Album={PersonalAlbum}
{IfPersonalAlbumChanged}-IPTC:Headline={PersonalAlbum}
{IfPersonalAlbumChanged}-XMP-photoshop:Headline={PersonalAlbum}
{IfPersonalAlbumChanged}-ItemList:Album={PersonalAlbum}
{IfPersonalAuthorChanged}-EXIF:Artist={PersonalAuthor}
{IfPersonalAuthorChanged}-IPTC:By-line={PersonalAuthor}
{IfPersonalAuthorChanged}-EXIF:XPAuthor={PersonalAuthor}
{IfPersonalAuthorChanged}-ItemList:Author={PersonalAuthor}
{IfPersonalAuthorChanged}-Creator={PersonalAuthor}
{IfPersonalCommentsChanged}-File:Comment={PersonalComments}
{IfPersonalCommentsChanged}-ExifIFD:UserComment={PersonalComments}
{IfPersonalCommentsChanged}-EXIF:UserComment={PersonalComments}
{IfPersonalCommentsChanged}-EXIF:XPComment={PersonalComments}
{IfPersonalCommentsChanged}-XMP-album:Notes={PersonalComments}
{IfPersonalCommentsChanged}-XMP-acdsee:Notes={PersonalComments}
{IfPersonalCommentsChanged}-XMP:UserComment={PersonalComments}
{IfPersonalCommentsChanged}-XMP:Notes={PersonalComments}
{IfPersonalCommentsChanged}-ItemList:Comment={PersonalComments}
{IfPersonalDescriptionChanged}-EXIF:ImageDescription={PersonalDescription}
{IfPersonalDescriptionChanged}-XMP:ImageDescription={PersonalDescription}
{IfPersonalDescriptionChanged}-XMP-dc:Description={PersonalDescription}
{IfPersonalDescriptionChanged}-XMP:Description={PersonalDescription}
{IfPersonalDescriptionChanged}-IPTC:Caption-Abstract={PersonalDescription}
{IfPersonalDescriptionChanged}-ItemList:Description={PersonalDescription}
{IfPersonalDescriptionChanged}-Description={PersonalDescription}
{IfPersonalRatingChanged}-XMP-microsoft:RatingPercent={PersonalRatingPercent}
{IfPersonalRatingChanged}-XMP:RatingPercent={PersonalRatingPercent}
{IfPersonalRatingChanged}-EXIF:RatingPercent={PersonalRatingPercent}
{IfPersonalRatingChanged}-XMP-xmp:Rating={PersonalRating}
{IfPersonalRatingChanged}-XMP:Rating={PersonalRating}
{IfPersonalRatingChanged}-XMP-acdsee:Rating={PersonalRating}
{IfPersonalRatingChanged}-EXIF:Rating={PersonalRating}
{IfPersonalRatingChanged}-Rating={PersonalRating}
{IfPersonalTitleChanged}-ItemList:Title={PersonalTitle}
{IfPersonalTitleChanged}-EXIF:XPTitle={PersonalTitle}
{IfPersonalTitleChanged}-XMP-dc:Title={PersonalTitle}
{IfPersonalTitleChanged}-XMP:Title={PersonalTitle}
{IfPersonalTitleChanged}-ItemList:Title={PersonalTitle}
{IfLocationLatitudeChanged}-EXIF:GPSLatitude={LocationLatitude}
{IfLocationLatitudeChanged}-XMP-exif:GPSLatitude={LocationLatitude}
{IfLocationLatitudeChanged}-XMP:GPSLatitude={LocationLatitude}
{IfLocationLatitudeChanged}-GPS:GPSLatitude={LocationLatitude}
{IfLocationLatitudeChanged}-GPSLatitude={LocationLatitude}
{IfLocationLongitudeChanged}-EXIF:GPSLongitude={LocationLongitude}
{IfLocationLongitudeChanged}-XMP-exif:GPSLongitude={LocationLongitude}
{IfLocationLongitudeChanged}-XMP:GPSLongitude={LocationLongitude}
{IfLocationLongitudeChanged}-GPS:GPSLongitude={LocationLongitude}
{IfLocationLongitudeChanged}-GPSLongitude={LocationLongitude}
{IfLocationNameChanged}-XMP:Location={LocationName}
{IfLocationNameChanged}-XMP-iptcCore:Location={LocationName}
{IfLocationNameChanged}-XMP-iptcExt:LocationShownSublocation={LocationName}
{IfLocationNameChanged}-XMP:LocationCreatedSublocation={LocationName}
{IfLocationNameChanged}-IPTC:Sub-location={LocationName}
{IfLocationNameChanged}-Sub-location={LocationName}
{IfLocationNameChanged}-Location={LocationName}
{IfLocationStateChanged}-XMP-iptcExt:LocationShownProvinceState={LocationState}
{IfLocationStateChanged}-XMP-photoshop:State={LocationState}
{IfLocationStateChanged}-IPTC:Province-State={LocationState}
{IfLocationStateChanged}-XMP:State={LocationState}
{IfLocationStateChanged}-State={LocationState}
{IfLocationCityChanged}-XMP-photoshop:City={LocationCity}
{IfLocationCityChanged}-XMP-iptcExt:LocationShownCity={LocationCity}
{IfLocationCityChanged}-IPTC:City={LocationCity}
{IfLocationCityChanged}-XMP:City={LocationCity}
{IfLocationCityChanged}-City={LocationCity}
{IfLocationCountryChanged}-IPTC:Country-PrimaryLocationName={LocationCountry}
{IfLocationCountryChanged}-XMP-photoshop:Country={LocationCountry}
{IfLocationCountryChanged}-XMP-iptcExt:LocationShownCountryName={LocationCountry}
{IfLocationCountryChanged}-XMP:Country={LocationCountry}
{IfLocationCountryChanged}-Country={LocationCountry}
{IfPersonalRegionChanged}-ImageRegion=
{IfPersonalRegionChanged}-RegionInfoMP={PersonalRegionInfoMP}
{IfPersonalRegionChanged}-RegionInfo={PersonalRegionInfo}
{IfPersonalKeywordsChanged}-Subject=
{IfPersonalKeywordsChanged}-Keyword=
{IfPersonalKeywordsChanged}-Keywords=
{IfPersonalKeywordsChanged}-XPKeywords=
{IfPersonalKeywordsChanged}-Category=
{IfPersonalKeywordsChanged}-Categories=
{IfPersonalKeywordsChanged}-CatalogSets=
{IfPersonalKeywordsChanged}-HierarchicalKeywords=
{IfPersonalKeywordsChanged}-HierarchicalSubject=
{IfPersonalKeywordsChanged}-LastKeywordXMP=
{IfPersonalKeywordsChanged}-LastKeywordIPTC=
{IfPersonalKeywordsChanged}-TagsList=
{IfPersonalKeywordsChanged}{PersonalKeywordItemsDelete}
{IfPersonalKeywordsChanged}{PersonalKeywordItemsAdd}
{IfPersonalKeywordsChanged}-Categories={PersonalKeywordsXML}
{IfPersonalKeywordsChanged}-XPKeywords={PersonalKeywordsList}
{FileFullPath}
-execute
*/

