using DataGridViewExtended;
using PhotoTagsCommonComponets;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    partial class FormConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfig));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelApplicationRegionThumbnailSize = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperJavaScriptExecuteTimeoutDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperWebScrapingRetryDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperWebScrapringDelayDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperWebScrapingDelayDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperPageStartLoadingTimeoutDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperPageLoadedTimeoutDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperNumberOfPageDownDescription = new Krypton.Toolkit.KryptonLabel();
            this.numericUpDownWebScrapingPageDownCount = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWaitEventPageLoadedTimeout = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWaitEventPageStartLoadingTimeout = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWebScrapingDelayInPageScriptToRun = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWebScrapingDelayOurScriptToRun = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWebScrapingRetry = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownJavaScriptExecuteTimeout = new System.Windows.Forms.NumericUpDown();
            this.labelWebScraperNumberOfPageDown = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperPageLoadedTimeout = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperPageStartLoadingTimeout = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperWebScrapingDelay = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperWebScrapringDelay = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperJavaScriptExecuteTimeout = new Krypton.Toolkit.KryptonLabel();
            this.labelWebScraperWebScrapingRetry = new Krypton.Toolkit.KryptonLabel();
            this.textBoxWebScrapingStartPages = new Krypton.Toolkit.KryptonTextBox();
            this.fastColoredTextBoxConfigFilenameDateFormats = new FastColoredTextBoxNS.FastColoredTextBox();
            this.textBoxApplicationDateTimeFormatHelp = new Krypton.Toolkit.KryptonTextBox();
            this.textBoxRenameTo = new Krypton.Toolkit.KryptonTextBox();
            this.label10 = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxRename = new Krypton.Toolkit.KryptonCheckBox();
            this.label9 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxRenameVariables = new Krypton.Toolkit.KryptonComboBox();
            this.checkBoxFaceRegionAddWebScraping = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxFaceRegionAddMicrosoftPhotos = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxFaceRegionAddWindowsMediaPhotoGallery = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxAutoCorrectTrackChanges = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordBackupFileCreatedBefore = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordBackupFileCreatedAfter = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordBackupDateTakenBefore = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordBackupLocationCountry = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordBackupGPSDateTimeUTCBefore = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordBackupLocationState = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordBackupDateTakenAfter = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordBackupLocationCity = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordBackupGPSDateTimeUTCAfter = new Krypton.Toolkit.KryptonCheckBox();
            this.labelAutoCorrectBackupOfTags = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxKeywordBackupLocationName = new Krypton.Toolkit.KryptonCheckBox();
            this.label17 = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxKeywordBackupRegionFaceNames = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordsAddAutoKeywords = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordsAddWebScraping = new Krypton.Toolkit.KryptonCheckBox();
            this.label4 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxKeywordsAiConfidence = new Krypton.Toolkit.KryptonComboBox();
            this.checkBoxKeywordsAddMicrosoftPhotos = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxKeywordsAddWindowsMediaPhotoGallery = new Krypton.Toolkit.KryptonCheckBox();
            this.radioButtonAuthorAlwaysChange = new Krypton.Toolkit.KryptonRadioButton();
            this.label13 = new Krypton.Toolkit.KryptonLabel();
            this.radioButtonAuthorChangeWhenEmpty = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonAuthorDoNotChange = new Krypton.Toolkit.KryptonRadioButton();
            this.checkBoxDublicateAlbumAsDescription = new Krypton.Toolkit.KryptonCheckBox();
            this.label19 = new Krypton.Toolkit.KryptonLabel();
            this.imageListViewOrderAlbum = new PhotoTagsCommonComponets.ImageListViewOrder();
            this.radioButtonAlbumUseFirst = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonAlbumChangeWhenEmpty = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonAlbumDoNotChange = new Krypton.Toolkit.KryptonRadioButton();
            this.labelAutoCorrectTitlePrioritySource = new Krypton.Toolkit.KryptonLabel();
            this.imageListViewOrderTitle = new PhotoTagsCommonComponets.ImageListViewOrder();
            this.radioButtonTitleUseFirst = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonTitleChangeWhenEmpty = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonTitleDoNotChange = new Krypton.Toolkit.KryptonRadioButton();
            this.checkBoxUpdateLocationCountry = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxUpdateLocationState = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxUpdateLocationCity = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxUpdateLocationName = new Krypton.Toolkit.KryptonCheckBox();
            this.label15 = new Krypton.Toolkit.KryptonLabel();
            this.labelAutoCorrectLocationInformationDescription = new Krypton.Toolkit.KryptonLabel();
            this.radioButtonLocationNameChangeAlways = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonLocationNameChangeWhenEmpty = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonLocationNameDoNotChange = new Krypton.Toolkit.KryptonRadioButton();
            this.label87 = new Krypton.Toolkit.KryptonLabel();
            this.numericUpDownLocationAccurateIntervalNearByMediaFile = new System.Windows.Forms.NumericUpDown();
            this.checkBoxGPSUpdateLocationNearByMedia = new Krypton.Toolkit.KryptonCheckBox();
            this.label23 = new Krypton.Toolkit.KryptonLabel();
            this.label22 = new Krypton.Toolkit.KryptonLabel();
            this.numericUpDownLocationAccurateInterval = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLocationGuessInterval = new System.Windows.Forms.NumericUpDown();
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing = new Krypton.Toolkit.KryptonLabel();
            this.labelLocationTimeZoneAccurate = new Krypton.Toolkit.KryptonLabel();
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1 = new Krypton.Toolkit.KryptonLabel();
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing = new Krypton.Toolkit.KryptonLabel();
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4 = new Krypton.Toolkit.KryptonLabel();
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3 = new Krypton.Toolkit.KryptonLabel();
            this.labelLocationTimeZoneGuess = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxGPSUpdateDateTime = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxGPSUpdateLocation = new Krypton.Toolkit.KryptonCheckBox();
            this.labelAutoCorrectPrioritySourceOrder = new Krypton.Toolkit.KryptonLabel();
            this.radioButtonDateTakenUseFirst = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonDateTakenChangeWhenEmpty = new Krypton.Toolkit.KryptonRadioButton();
            this.radioButtonDateTakenDoNotChange = new Krypton.Toolkit.KryptonRadioButton();
            this.imageListViewOrderDateTaken = new PhotoTagsCommonComponets.ImageListViewOrder();
            this.textBoxHelpAutoCorrect = new Krypton.Toolkit.KryptonTextBox();
            this.dataGridViewAutoKeywords = new System.Windows.Forms.DataGridView();
            this.LocationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Album = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Keywords = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddKeywords = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripAutoKeyword = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAutoKeywordCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAutoKeywordCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAutoKeywordPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAutoKeywordDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAutoKeywordUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox3 = new Krypton.Toolkit.KryptonTextBox();
            this.dataGridViewCameraOwner = new System.Windows.Forms.DataGridView();
            this.dataGridViewLocationNames = new System.Windows.Forms.DataGridView();
            this.contextMenuStripLocationNames = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemMapCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapShowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapHideEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowCoordinateOnMap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowCoordinateOnGoogleMap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapReloadLocationUsingNominatim = new System.Windows.Forms.ToolStripMenuItem();
            this.searchForNewLocationsInMediaFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxBrowserURL = new Krypton.Toolkit.KryptonTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBoxMapZoomLevel = new Krypton.Toolkit.KryptonComboBox();
            this.panelBrowser = new Krypton.Toolkit.KryptonPanel();
            this.buttonLocationImport = new Krypton.Toolkit.KryptonButton();
            this.buttonLocationExport = new Krypton.Toolkit.KryptonButton();
            this.textBox4 = new Krypton.Toolkit.KryptonTextBox();
            this.labelApplicationGPSLocationAccuracy = new Krypton.Toolkit.KryptonLabel();
            this.label65 = new Krypton.Toolkit.KryptonLabel();
            this.label64 = new Krypton.Toolkit.KryptonLabel();
            this.numericUpDownLocationAccuracyLongitude = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLocationAccuracyLatitude = new System.Windows.Forms.NumericUpDown();
            this.checkBoxApplicationAvoidReadExifFromCloud = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxApplicationImageListViewCacheModeOnDemand = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxApplicationAvoidReadMediaFromCloud = new Krypton.Toolkit.KryptonCheckBox();
            this.labelApplicationRegionAccuracyDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelApplicationRegionAccuracyHelp = new Krypton.Toolkit.KryptonLabel();
            this.labelApplicationRegionAccuracy = new Krypton.Toolkit.KryptonLabel();
            this.numericUpDownRegionMissmatchProcent = new System.Windows.Forms.NumericUpDown();
            this.labelApplicationNumberOfMostCommonDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelApplicationNumberOfDaysDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelApplicationNumberOfMostCommon = new Krypton.Toolkit.KryptonLabel();
            this.labelApplicationNumberOfDays = new Krypton.Toolkit.KryptonLabel();
            this.numericUpDownPeopleSuggestNameTopMost = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPeopleSuggestNameDaysInterval = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownApplicationMaxRowsInSearchResult = new System.Windows.Forms.NumericUpDown();
            this.labelApplicationSearch = new Krypton.Toolkit.KryptonLabel();
            this.labelApplicationNominatimPreferredLanguagesHelp = new Krypton.Toolkit.KryptonLabel();
            this.labelApplicationNominatimTitle = new Krypton.Toolkit.KryptonLabel();
            this.labelApplicationPreferredLanguages = new Krypton.Toolkit.KryptonLabel();
            this.textBoxApplicationPreferredLanguages = new Krypton.Toolkit.KryptonTextBox();
            this.comboBoxApplicationLanguages = new Krypton.Toolkit.KryptonComboBox();
            this.labelApplicationRegionThumbnailSizeDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelApplicationPosterThumbnailSizeDescription = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxApplicationRegionThumbnailSizes = new Krypton.Toolkit.KryptonComboBox();
            this.labelApplicationThumbnailSizeHelp = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxApplicationThumbnailSizes = new Krypton.Toolkit.KryptonComboBox();
            this.labelApplicationPosterThumbnailSize = new Krypton.Toolkit.KryptonLabel();
            this.dataGridViewMetadataReadPriority = new System.Windows.Forms.DataGridView();
            this.contextMenuStripMetadataRead = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemMetadataReadMove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMetadataReadShowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.labelMetadataFileCreateDateTimDiffrentDescription = new Krypton.Toolkit.KryptonLabel();
            this.labelMetadataFileCreateDateTimDiffrent = new Krypton.Toolkit.KryptonLabel();
            this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted = new System.Windows.Forms.NumericUpDown();
            this.checkBoxWriteFileAttributeCreatedDate = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxWriteMetadataAddAutoKeywords = new Krypton.Toolkit.KryptonCheckBox();
            this.comboBoxMetadataWriteKeywordAdd = new Krypton.Toolkit.KryptonComboBox();
            this.fastColoredTextBoxMetadataWriteKeywordAdd = new FastColoredTextBoxNS.FastColoredTextBox();
            this.fastColoredTextBoxMetadataWriteKeywordDelete = new FastColoredTextBoxNS.FastColoredTextBox();
            this.comboBoxMetadataWriteKeywordDelete = new Krypton.Toolkit.KryptonComboBox();
            this.labelMetadataForeachDeletedKeyword = new Krypton.Toolkit.KryptonLabel();
            this.labelMetadataForeachNewKeyword = new Krypton.Toolkit.KryptonLabel();
            this.textBoxWriteXtraAtomArtist = new Krypton.Toolkit.KryptonTextBox();
            this.checkBoxWriteXtraAtomArtistVideo = new Krypton.Toolkit.KryptonCheckBox();
            this.label37 = new Krypton.Toolkit.KryptonLabel();
            this.labelMetadataWriteOnVideoAndPictureFilesVariables = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxWriteXtraAtomVariables = new Krypton.Toolkit.KryptonComboBox();
            this.textBoxWriteXtraAtomComment = new Krypton.Toolkit.KryptonTextBox();
            this.textBoxWriteXtraAtomSubject = new Krypton.Toolkit.KryptonTextBox();
            this.textBoxWriteXtraAtomSubtitle = new Krypton.Toolkit.KryptonTextBox();
            this.textBoxWriteXtraAtomAlbum = new Krypton.Toolkit.KryptonTextBox();
            this.textBoxWriteXtraAtomCategories = new Krypton.Toolkit.KryptonTextBox();
            this.textBoxWriteXtraAtomKeywords = new Krypton.Toolkit.KryptonTextBox();
            this.labelMetadataWriteOnVideoAndPictureFiles = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxWriteXtraAtomRatingPicture = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxWriteXtraAtomRatingVideo = new Krypton.Toolkit.KryptonCheckBox();
            this.label34 = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxWriteXtraAtomCommentPicture = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxWriteXtraAtomSubjectPicture = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxWriteXtraAtomCommentVideo = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxWriteXtraAtomSubjectVideo = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxWriteXtraAtomSubtitleVideo = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxWriteXtraAtomAlbumVideo = new Krypton.Toolkit.KryptonCheckBox();
            this.label33 = new Krypton.Toolkit.KryptonLabel();
            this.label32 = new Krypton.Toolkit.KryptonLabel();
            this.label31 = new Krypton.Toolkit.KryptonLabel();
            this.label30 = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxWriteXtraAtomCategoriesVideo = new Krypton.Toolkit.KryptonCheckBox();
            this.label29 = new Krypton.Toolkit.KryptonLabel();
            this.label28 = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxWriteXtraAtomKeywordsVideo = new Krypton.Toolkit.KryptonCheckBox();
            this.fastColoredTextBoxMetadataWriteTags = new FastColoredTextBoxNS.FastColoredTextBox();
            this.comboBoxMetadataWriteStandardTags = new Krypton.Toolkit.KryptonComboBox();
            this.labelMetadataForeachKeyword = new Krypton.Toolkit.KryptonLabel();
            this.textBoxMetadataWriteHelpText = new Krypton.Toolkit.KryptonTextBox();
            this.buttonConfigSave = new Krypton.Toolkit.KryptonButton();
            this.buttonConfigCancel = new Krypton.Toolkit.KryptonButton();
            this.panelAvoidResizeIssues = new Krypton.Toolkit.KryptonPanel();
            this.tabControlConfig = new System.Windows.Forms.TabControl();
            this.tabPageApplication = new System.Windows.Forms.TabPage();
            this.panelApplication = new Krypton.Toolkit.KryptonPanel();
            this.tabPageConvertAndMerge = new System.Windows.Forms.TabPage();
            this.panelConvertAndMerge = new Krypton.Toolkit.KryptonPanel();
            this.groupBoxConvertAndMergeConvertVideo = new Krypton.Toolkit.KryptonGroupBox();
            this.label53 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxConvertAndMergeConvertVideoFilesVariables = new Krypton.Toolkit.KryptonComboBox();
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument = new FastColoredTextBoxNS.FastColoredTextBox();
            this.groupBoxConvertAndMergeMergeVideos = new Krypton.Toolkit.KryptonGroupBox();
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxConvertAndMergeConcatVideoFilesVariables = new Krypton.Toolkit.KryptonComboBox();
            this.label52 = new Krypton.Toolkit.KryptonLabel();
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile = new FastColoredTextBoxNS.FastColoredTextBox();
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument = new FastColoredTextBoxNS.FastColoredTextBox();
            this.label50 = new Krypton.Toolkit.KryptonLabel();
            this.groupBoxConvertAndMergeCommand = new Krypton.Toolkit.KryptonGroupBox();
            this.label56 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxConvertAndMergeTempfileExtension = new Krypton.Toolkit.KryptonComboBox();
            this.label55 = new Krypton.Toolkit.KryptonLabel();
            this.label54 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxConvertAndMergeOutputSize = new Krypton.Toolkit.KryptonComboBox();
            this.buttonConvertAndMergeBrowseBackgroundMusic = new Krypton.Toolkit.KryptonButton();
            this.buttonConvertAndMergeBrowseFFmpeg = new Krypton.Toolkit.KryptonButton();
            this.label49 = new Krypton.Toolkit.KryptonLabel();
            this.numericUpDownConvertAndMergeImageDuration = new System.Windows.Forms.NumericUpDown();
            this.label48 = new Krypton.Toolkit.KryptonLabel();
            this.textBoxConvertAndMergeBackgroundMusic = new Krypton.Toolkit.KryptonTextBox();
            this.label47 = new Krypton.Toolkit.KryptonLabel();
            this.textBoxConvertAndMergeFFmpeg = new Krypton.Toolkit.KryptonTextBox();
            this.label46 = new Krypton.Toolkit.KryptonLabel();
            this.groupBoxConvertAndMergeArgumentFileVariables = new Krypton.Toolkit.KryptonGroupBox();
            this.label51 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables = new Krypton.Toolkit.KryptonComboBox();
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument = new FastColoredTextBoxNS.FastColoredTextBox();
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables = new Krypton.Toolkit.KryptonComboBox();
            this.label45 = new Krypton.Toolkit.KryptonLabel();
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile = new FastColoredTextBoxNS.FastColoredTextBox();
            this.kryptonManager1 = new Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonWorkspaceConfig = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageApplication = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceConfigApplication = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageApplicationThumbnail = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCell2 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageApplicationNominatim = new Krypton.Navigator.KryptonPage();
            this.kryptonPageApplicationSearch = new Krypton.Navigator.KryptonPage();
            this.kryptonPageApplicationRegionSuggestion = new Krypton.Navigator.KryptonPage();
            this.kryptonPageRegionAccuracy = new Krypton.Navigator.KryptonPage();
            this.kryptonPageApplicationCloudAndVirtualFiles = new Krypton.Navigator.KryptonPage();
            this.kryptonPageApplicationGPSLocationAccuracy = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp = new Krypton.Navigator.KryptonPage();
            this.kryptonPageApplicationDateAndTimeInFilenames = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCell1 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageMetadata = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceConfigMetadata = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageMetadataReadHelp = new Krypton.Navigator.KryptonPage();
            this.kryptonTextBoxMetadataReadHelpText = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonWorkspaceCell3 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageMetadataReadPriority = new Krypton.Navigator.KryptonPage();
            this.kryptonPageMetadataWriteHelp = new Krypton.Navigator.KryptonPage();
            this.kryptonPageMetadataWriteWindowsXtraProperties = new Krypton.Navigator.KryptonPage();
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated = new Krypton.Navigator.KryptonPage();
            this.kryptonPageMetadataExiftoolHelp = new Krypton.Navigator.KryptonPage();
            this.kryptonPageMetadataExiftoolForEachNewKeyword = new Krypton.Navigator.KryptonPage();
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword = new Krypton.Navigator.KryptonPage();
            this.kryptonPageMetadataExiftoolForEachKeyword = new Krypton.Navigator.KryptonPage();
            this.kryptonPageWebScraper = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceWebScraper = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageWebScraperWebScrapingSettings = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCell4 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageWebScraperWebScrapingStartPages = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrect = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceAutoCorrect = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageAutoCorrectAutoKeywords = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCell5 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageAutoCorrectAutoCorrectHelp = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectDateAndTimeDigitized = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectGPSLocationAndDateTime = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectLocationInformation = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectTitle = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectAlbum = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectAuthor = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectKeywordTags = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectBackupOfTags = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectFaceRegionFields = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectRename = new Krypton.Navigator.KryptonPage();
            this.kryptonPageAutoCorrectKeywordsHelp = new Krypton.Navigator.KryptonPage();
            this.kryptonPageLocation = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceLocation = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPage42 = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceLocationLocationNames = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageLocationLocationNameNames = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCellLocationLocationNameNames = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonWorkspaceCellLocationLocationNameMap = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageLocationLocationNameMap = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceCell6 = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPage38 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage39 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage41 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage40 = new Krypton.Navigator.KryptonPage();
            this.kryptonPageConvertAndMerge = new Krypton.Navigator.KryptonPage();
            this.kryptonPageChromecast = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceChromecast = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageChromecastVLCstreamConfig = new Krypton.Navigator.KryptonPage();
            this.comboBoxChromecastVideoCodec = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxChromecastUrl = new Krypton.Toolkit.KryptonComboBox();
            this.label61 = new Krypton.Toolkit.KryptonLabel();
            this.label58 = new Krypton.Toolkit.KryptonLabel();
            this.label60 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxChromecastAgruments = new Krypton.Toolkit.KryptonComboBox();
            this.label59 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxChromecastAudioCodec = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonWorkspaceCellChromecast = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageChromecastImage = new Krypton.Navigator.KryptonPage();
            this.comboBoxChromecastImageFormat = new Krypton.Toolkit.KryptonComboBox();
            this.label57 = new Krypton.Toolkit.KryptonLabel();
            this.label63 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxChromecastImageResolution = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonPageChromecastVideo = new Krypton.Navigator.KryptonPage();
            this.label66 = new Krypton.Toolkit.KryptonLabel();
            this.comboBoxChromecastVideoTransporter = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonPageLog = new Krypton.Navigator.KryptonPage();
            this.kryptonWorkspaceLog = new Krypton.Workspace.KryptonWorkspace();
            this.kryptonPageLogApplication = new Krypton.Navigator.KryptonPage();
            this.fastColoredTextBoxShowLog = new FastColoredTextBoxNS.FastColoredTextBox();
            this.kryptonWorkspaceCellLog = new Krypton.Workspace.KryptonWorkspaceCell();
            this.kryptonPageLogPipe = new Krypton.Navigator.KryptonPage();
            this.fastColoredTextBoxShowPipe32Log = new FastColoredTextBoxNS.FastColoredTextBox();
            this.groupBox12 = new Krypton.Toolkit.KryptonGroupBox();
            this.numericUpDownCacheNumberOfPosters = new System.Windows.Forms.NumericUpDown();
            this.label83 = new Krypton.Toolkit.KryptonLabel();
            this.checkBoxCacheAllMetadatas = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxCacheAllThumbnails = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxCacheAllWebScraperDataSets = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxCacheFolderMetadatas = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxCacheFolderThumbnails = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxCacheFolderWebScraperDataSets = new Krypton.Toolkit.KryptonCheckBox();
            this.groupBox13 = new Krypton.Toolkit.KryptonGroupBox();
            this.checkBoxApplicationExiftoolReadShowCliWindow = new Krypton.Toolkit.KryptonCheckBox();
            this.checkBoxApplicationExiftoolWriteShowCliWindow = new Krypton.Toolkit.KryptonCheckBox();
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity = new Krypton.Toolkit.KryptonComboBox();
            this.comboBoxApplicationDebugBackgroundThreadPrioity = new Krypton.Toolkit.KryptonComboBox();
            this.label90 = new Krypton.Toolkit.KryptonLabel();
            this.label91 = new Krypton.Toolkit.KryptonLabel();
            this.label92 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.checkBoxApplicationDarkMode = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonComboBoxThemes = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonPage23 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage44 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage45 = new Krypton.Navigator.KryptonPage();
            this.kryptonPage47 = new Krypton.Navigator.KryptonPage();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebScrapingPageDownCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaitEventPageLoadedTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaitEventPageStartLoadingTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebScrapingDelayInPageScriptToRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebScrapingDelayOurScriptToRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebScrapingRetry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownJavaScriptExecuteTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConfigFilenameDateFormats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRenameVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxKeywordsAiConfidence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationAccurateIntervalNearByMediaFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationAccurateInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationGuessInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAutoKeywords)).BeginInit();
            this.contextMenuStripAutoKeyword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCameraOwner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLocationNames)).BeginInit();
            this.contextMenuStripLocationNames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMapZoomLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBrowser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationAccuracyLongitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationAccuracyLatitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRegionMissmatchProcent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeopleSuggestNameTopMost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeopleSuggestNameDaysInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownApplicationMaxRowsInSearchResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationLanguages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationRegionThumbnailSizes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationThumbnailSizes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMetadataReadPriority)).BeginInit();
            this.contextMenuStripMetadataRead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMetadataWriteKeywordAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxMetadataWriteKeywordAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxMetadataWriteKeywordDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMetadataWriteKeywordDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxWriteXtraAtomVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxMetadataWriteTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMetadataWriteStandardTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelAvoidResizeIssues)).BeginInit();
            this.panelAvoidResizeIssues.SuspendLayout();
            this.tabControlConfig.SuspendLayout();
            this.tabPageApplication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelApplication)).BeginInit();
            this.tabPageConvertAndMerge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelConvertAndMerge)).BeginInit();
            this.panelConvertAndMerge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeConvertVideo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeConvertVideo.Panel)).BeginInit();
            this.groupBoxConvertAndMergeConvertVideo.Panel.SuspendLayout();
            this.groupBoxConvertAndMergeConvertVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConvertVideoFilesVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeMergeVideos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeMergeVideos.Panel)).BeginInit();
            this.groupBoxConvertAndMergeMergeVideos.Panel.SuspendLayout();
            this.groupBoxConvertAndMergeMergeVideos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConcatVideosArguFileVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConcatVideoFilesVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConcatVideoArgument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeCommand.Panel)).BeginInit();
            this.groupBoxConvertAndMergeCommand.Panel.SuspendLayout();
            this.groupBoxConvertAndMergeCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeTempfileExtension)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeOutputSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConvertAndMergeImageDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeArgumentFileVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeArgumentFileVariables.Panel)).BeginInit();
            this.groupBoxConvertAndMergeArgumentFileVariables.Panel.SuspendLayout();
            this.groupBoxConvertAndMergeArgumentFileVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceConfig)).BeginInit();
            this.kryptonWorkspaceConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplication)).BeginInit();
            this.kryptonPageApplication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceConfigApplication)).BeginInit();
            this.kryptonWorkspaceConfigApplication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationThumbnail)).BeginInit();
            this.kryptonPageApplicationThumbnail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell2)).BeginInit();
            this.kryptonWorkspaceCell2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationNominatim)).BeginInit();
            this.kryptonPageApplicationNominatim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationSearch)).BeginInit();
            this.kryptonPageApplicationSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationRegionSuggestion)).BeginInit();
            this.kryptonPageApplicationRegionSuggestion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageRegionAccuracy)).BeginInit();
            this.kryptonPageRegionAccuracy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationCloudAndVirtualFiles)).BeginInit();
            this.kryptonPageApplicationCloudAndVirtualFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationGPSLocationAccuracy)).BeginInit();
            this.kryptonPageApplicationGPSLocationAccuracy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp)).BeginInit();
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationDateAndTimeInFilenames)).BeginInit();
            this.kryptonPageApplicationDateAndTimeInFilenames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell1)).BeginInit();
            this.kryptonWorkspaceCell1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadata)).BeginInit();
            this.kryptonPageMetadata.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceConfigMetadata)).BeginInit();
            this.kryptonWorkspaceConfigMetadata.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataReadHelp)).BeginInit();
            this.kryptonPageMetadataReadHelp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell3)).BeginInit();
            this.kryptonWorkspaceCell3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataReadPriority)).BeginInit();
            this.kryptonPageMetadataReadPriority.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataWriteHelp)).BeginInit();
            this.kryptonPageMetadataWriteHelp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataWriteWindowsXtraProperties)).BeginInit();
            this.kryptonPageMetadataWriteWindowsXtraProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataWriteFileAttributeDateTimeCreated)).BeginInit();
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataExiftoolHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataExiftoolForEachNewKeyword)).BeginInit();
            this.kryptonPageMetadataExiftoolForEachNewKeyword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataExiftoolForEachDeletedKeyword)).BeginInit();
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataExiftoolForEachKeyword)).BeginInit();
            this.kryptonPageMetadataExiftoolForEachKeyword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageWebScraper)).BeginInit();
            this.kryptonPageWebScraper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceWebScraper)).BeginInit();
            this.kryptonWorkspaceWebScraper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageWebScraperWebScrapingSettings)).BeginInit();
            this.kryptonPageWebScraperWebScrapingSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell4)).BeginInit();
            this.kryptonWorkspaceCell4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageWebScraperWebScrapingStartPages)).BeginInit();
            this.kryptonPageWebScraperWebScrapingStartPages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrect)).BeginInit();
            this.kryptonPageAutoCorrect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceAutoCorrect)).BeginInit();
            this.kryptonWorkspaceAutoCorrect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectAutoKeywords)).BeginInit();
            this.kryptonPageAutoCorrectAutoKeywords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell5)).BeginInit();
            this.kryptonWorkspaceCell5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectAutoCorrectHelp)).BeginInit();
            this.kryptonPageAutoCorrectAutoCorrectHelp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectDateAndTimeDigitized)).BeginInit();
            this.kryptonPageAutoCorrectDateAndTimeDigitized.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectGPSLocationAndDateTime)).BeginInit();
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectLocationInformation)).BeginInit();
            this.kryptonPageAutoCorrectLocationInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectTitle)).BeginInit();
            this.kryptonPageAutoCorrectTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectAlbum)).BeginInit();
            this.kryptonPageAutoCorrectAlbum.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectAuthor)).BeginInit();
            this.kryptonPageAutoCorrectAuthor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectKeywordTags)).BeginInit();
            this.kryptonPageAutoCorrectKeywordTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectBackupOfTags)).BeginInit();
            this.kryptonPageAutoCorrectBackupOfTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectFaceRegionFields)).BeginInit();
            this.kryptonPageAutoCorrectFaceRegionFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectRename)).BeginInit();
            this.kryptonPageAutoCorrectRename.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectKeywordsHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLocation)).BeginInit();
            this.kryptonPageLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceLocation)).BeginInit();
            this.kryptonWorkspaceLocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage42)).BeginInit();
            this.kryptonPage42.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceLocationLocationNames)).BeginInit();
            this.kryptonWorkspaceLocationLocationNames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLocationLocationNameNames)).BeginInit();
            this.kryptonPageLocationLocationNameNames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellLocationLocationNameNames)).BeginInit();
            this.kryptonWorkspaceCellLocationLocationNameNames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellLocationLocationNameMap)).BeginInit();
            this.kryptonWorkspaceCellLocationLocationNameMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLocationLocationNameMap)).BeginInit();
            this.kryptonPageLocationLocationNameMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell6)).BeginInit();
            this.kryptonWorkspaceCell6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage38)).BeginInit();
            this.kryptonPage38.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage39)).BeginInit();
            this.kryptonPage39.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage41)).BeginInit();
            this.kryptonPage41.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage40)).BeginInit();
            this.kryptonPage40.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageConvertAndMerge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChromecast)).BeginInit();
            this.kryptonPageChromecast.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceChromecast)).BeginInit();
            this.kryptonWorkspaceChromecast.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChromecastVLCstreamConfig)).BeginInit();
            this.kryptonPageChromecastVLCstreamConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastVideoCodec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastUrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastAgruments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastAudioCodec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellChromecast)).BeginInit();
            this.kryptonWorkspaceCellChromecast.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChromecastImage)).BeginInit();
            this.kryptonPageChromecastImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastImageFormat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastImageResolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChromecastVideo)).BeginInit();
            this.kryptonPageChromecastVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastVideoTransporter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLog)).BeginInit();
            this.kryptonPageLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceLog)).BeginInit();
            this.kryptonWorkspaceLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLogApplication)).BeginInit();
            this.kryptonPageLogApplication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxShowLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLogPipe)).BeginInit();
            this.kryptonPageLogPipe.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxShowPipe32Log)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox12.Panel)).BeginInit();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCacheNumberOfPosters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox13.Panel)).BeginInit();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationDebugExiftoolReadThreadPrioity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationDebugExiftoolWriteThreadPrioity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationDebugBackgroundThreadPrioity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBoxThemes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage44)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage45)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage47)).BeginInit();
            this.SuspendLayout();
            // 
            // labelApplicationRegionThumbnailSize
            // 
            this.labelApplicationRegionThumbnailSize.Location = new System.Drawing.Point(3, 27);
            this.labelApplicationRegionThumbnailSize.Name = "labelApplicationRegionThumbnailSize";
            this.labelApplicationRegionThumbnailSize.Size = new System.Drawing.Size(136, 20);
            this.labelApplicationRegionThumbnailSize.TabIndex = 4;
            this.labelApplicationRegionThumbnailSize.Values.Text = "Region Thumbnail size:";
            // 
            // labelWebScraperJavaScriptExecuteTimeoutDescription
            // 
            this.labelWebScraperJavaScriptExecuteTimeoutDescription.Location = new System.Drawing.Point(429, 4);
            this.labelWebScraperJavaScriptExecuteTimeoutDescription.Name = "labelWebScraperJavaScriptExecuteTimeoutDescription";
            this.labelWebScraperJavaScriptExecuteTimeoutDescription.Size = new System.Drawing.Size(265, 20);
            this.labelWebScraperJavaScriptExecuteTimeoutDescription.TabIndex = 20;
            this.labelWebScraperJavaScriptExecuteTimeoutDescription.Values.Text = "ms. WebScraping script will timeout after x ms.";
            // 
            // labelWebScraperWebScrapingRetryDescription
            // 
            this.labelWebScraperWebScrapingRetryDescription.Location = new System.Drawing.Point(429, 54);
            this.labelWebScraperWebScrapingRetryDescription.Name = "labelWebScraperWebScrapingRetryDescription";
            this.labelWebScraperWebScrapingRetryDescription.Size = new System.Drawing.Size(290, 20);
            this.labelWebScraperWebScrapingRetryDescription.TabIndex = 19;
            this.labelWebScraperWebScrapingRetryDescription.Values.Text = "When you data not received, wait and retry x times.";
            // 
            // labelWebScraperWebScrapringDelayDescription
            // 
            this.labelWebScraperWebScrapringDelayDescription.Location = new System.Drawing.Point(429, 27);
            this.labelWebScraperWebScrapringDelayDescription.Name = "labelWebScraperWebScrapringDelayDescription";
            this.labelWebScraperWebScrapringDelayDescription.Size = new System.Drawing.Size(286, 20);
            this.labelWebScraperWebScrapringDelayDescription.TabIndex = 18;
            this.labelWebScraperWebScrapringDelayDescription.Values.Text = "ms. Give \"ScrapingScript\" x ms to run, before retry.";
            // 
            // labelWebScraperWebScrapingDelayDescription
            // 
            this.labelWebScraperWebScrapingDelayDescription.Location = new System.Drawing.Point(429, 79);
            this.labelWebScraperWebScrapingDelayDescription.Name = "labelWebScraperWebScrapingDelayDescription";
            this.labelWebScraperWebScrapingDelayDescription.Size = new System.Drawing.Size(321, 20);
            this.labelWebScraperWebScrapingDelayDescription.TabIndex = 17;
            this.labelWebScraperWebScrapingDelayDescription.Values.Text = "ms. Provide x ms to OnSite scripte to run before scraping";
            // 
            // labelWebScraperPageStartLoadingTimeoutDescription
            // 
            this.labelWebScraperPageStartLoadingTimeoutDescription.Location = new System.Drawing.Point(429, 106);
            this.labelWebScraperPageStartLoadingTimeoutDescription.Name = "labelWebScraperPageStartLoadingTimeoutDescription";
            this.labelWebScraperPageStartLoadingTimeoutDescription.Size = new System.Drawing.Size(248, 20);
            this.labelWebScraperPageStartLoadingTimeoutDescription.TabIndex = 16;
            this.labelWebScraperPageStartLoadingTimeoutDescription.Values.Text = "ms. Wait max x ms for page to start loading";
            // 
            // labelWebScraperPageLoadedTimeoutDescription
            // 
            this.labelWebScraperPageLoadedTimeoutDescription.Location = new System.Drawing.Point(429, 131);
            this.labelWebScraperPageLoadedTimeoutDescription.Name = "labelWebScraperPageLoadedTimeoutDescription";
            this.labelWebScraperPageLoadedTimeoutDescription.Size = new System.Drawing.Size(203, 20);
            this.labelWebScraperPageLoadedTimeoutDescription.TabIndex = 15;
            this.labelWebScraperPageLoadedTimeoutDescription.Values.Text = "ms. Wait max x ms for page to load";
            // 
            // labelWebScraperNumberOfPageDownDescription
            // 
            this.labelWebScraperNumberOfPageDownDescription.Location = new System.Drawing.Point(429, 157);
            this.labelWebScraperNumberOfPageDownDescription.Name = "labelWebScraperNumberOfPageDownDescription";
            this.labelWebScraperNumberOfPageDownDescription.Size = new System.Drawing.Size(318, 20);
            this.labelWebScraperNumberOfPageDownDescription.TabIndex = 14;
            this.labelWebScraperNumberOfPageDownDescription.Values.Text = "If no new information found, then accept as end of page";
            // 
            // numericUpDownWebScrapingPageDownCount
            // 
            this.numericUpDownWebScrapingPageDownCount.Location = new System.Drawing.Point(303, 157);
            this.numericUpDownWebScrapingPageDownCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWebScrapingPageDownCount.Name = "numericUpDownWebScrapingPageDownCount";
            this.numericUpDownWebScrapingPageDownCount.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWebScrapingPageDownCount.TabIndex = 6;
            this.numericUpDownWebScrapingPageDownCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numericUpDownWaitEventPageLoadedTimeout
            // 
            this.numericUpDownWaitEventPageLoadedTimeout.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownWaitEventPageLoadedTimeout.Location = new System.Drawing.Point(303, 131);
            this.numericUpDownWaitEventPageLoadedTimeout.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numericUpDownWaitEventPageLoadedTimeout.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownWaitEventPageLoadedTimeout.Name = "numericUpDownWaitEventPageLoadedTimeout";
            this.numericUpDownWaitEventPageLoadedTimeout.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWaitEventPageLoadedTimeout.TabIndex = 5;
            this.numericUpDownWaitEventPageLoadedTimeout.Value = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            // 
            // numericUpDownWaitEventPageStartLoadingTimeout
            // 
            this.numericUpDownWaitEventPageStartLoadingTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownWaitEventPageStartLoadingTimeout.Location = new System.Drawing.Point(303, 106);
            this.numericUpDownWaitEventPageStartLoadingTimeout.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numericUpDownWaitEventPageStartLoadingTimeout.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDownWaitEventPageStartLoadingTimeout.Name = "numericUpDownWaitEventPageStartLoadingTimeout";
            this.numericUpDownWaitEventPageStartLoadingTimeout.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWaitEventPageStartLoadingTimeout.TabIndex = 4;
            this.numericUpDownWaitEventPageStartLoadingTimeout.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // numericUpDownWebScrapingDelayInPageScriptToRun
            // 
            this.numericUpDownWebScrapingDelayInPageScriptToRun.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownWebScrapingDelayInPageScriptToRun.Location = new System.Drawing.Point(303, 80);
            this.numericUpDownWebScrapingDelayInPageScriptToRun.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownWebScrapingDelayInPageScriptToRun.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownWebScrapingDelayInPageScriptToRun.Name = "numericUpDownWebScrapingDelayInPageScriptToRun";
            this.numericUpDownWebScrapingDelayInPageScriptToRun.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWebScrapingDelayInPageScriptToRun.TabIndex = 3;
            this.numericUpDownWebScrapingDelayInPageScriptToRun.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // numericUpDownWebScrapingDelayOurScriptToRun
            // 
            this.numericUpDownWebScrapingDelayOurScriptToRun.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownWebScrapingDelayOurScriptToRun.Location = new System.Drawing.Point(303, 29);
            this.numericUpDownWebScrapingDelayOurScriptToRun.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownWebScrapingDelayOurScriptToRun.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownWebScrapingDelayOurScriptToRun.Name = "numericUpDownWebScrapingDelayOurScriptToRun";
            this.numericUpDownWebScrapingDelayOurScriptToRun.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWebScrapingDelayOurScriptToRun.TabIndex = 1;
            this.numericUpDownWebScrapingDelayOurScriptToRun.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // numericUpDownWebScrapingRetry
            // 
            this.numericUpDownWebScrapingRetry.Location = new System.Drawing.Point(303, 54);
            this.numericUpDownWebScrapingRetry.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownWebScrapingRetry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWebScrapingRetry.Name = "numericUpDownWebScrapingRetry";
            this.numericUpDownWebScrapingRetry.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWebScrapingRetry.TabIndex = 2;
            this.numericUpDownWebScrapingRetry.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numericUpDownJavaScriptExecuteTimeout
            // 
            this.numericUpDownJavaScriptExecuteTimeout.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownJavaScriptExecuteTimeout.Location = new System.Drawing.Point(303, 3);
            this.numericUpDownJavaScriptExecuteTimeout.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numericUpDownJavaScriptExecuteTimeout.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownJavaScriptExecuteTimeout.Name = "numericUpDownJavaScriptExecuteTimeout";
            this.numericUpDownJavaScriptExecuteTimeout.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownJavaScriptExecuteTimeout.TabIndex = 0;
            this.numericUpDownJavaScriptExecuteTimeout.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // labelWebScraperNumberOfPageDown
            // 
            this.labelWebScraperNumberOfPageDown.Location = new System.Drawing.Point(2, 157);
            this.labelWebScraperNumberOfPageDown.Name = "labelWebScraperNumberOfPageDown";
            this.labelWebScraperNumberOfPageDown.Size = new System.Drawing.Size(188, 20);
            this.labelWebScraperNumberOfPageDown.TabIndex = 6;
            this.labelWebScraperNumberOfPageDown.Values.Text = "Number of PageDown keystroke";
            // 
            // labelWebScraperPageLoadedTimeout
            // 
            this.labelWebScraperPageLoadedTimeout.Location = new System.Drawing.Point(3, 131);
            this.labelWebScraperPageLoadedTimeout.Name = "labelWebScraperPageLoadedTimeout";
            this.labelWebScraperPageLoadedTimeout.Size = new System.Drawing.Size(130, 20);
            this.labelWebScraperPageLoadedTimeout.TabIndex = 5;
            this.labelWebScraperPageLoadedTimeout.Values.Text = "Page Loaded Timeout ";
            // 
            // labelWebScraperPageStartLoadingTimeout
            // 
            this.labelWebScraperPageStartLoadingTimeout.Location = new System.Drawing.Point(3, 106);
            this.labelWebScraperPageStartLoadingTimeout.Name = "labelWebScraperPageStartLoadingTimeout";
            this.labelWebScraperPageStartLoadingTimeout.Size = new System.Drawing.Size(162, 20);
            this.labelWebScraperPageStartLoadingTimeout.TabIndex = 4;
            this.labelWebScraperPageStartLoadingTimeout.Values.Text = "Page Start Loading Timeout ";
            // 
            // labelWebScraperWebScrapingDelay
            // 
            this.labelWebScraperWebScrapingDelay.Location = new System.Drawing.Point(3, 80);
            this.labelWebScraperWebScrapingDelay.Name = "labelWebScraperWebScrapingDelay";
            this.labelWebScraperWebScrapingDelay.Size = new System.Drawing.Size(235, 20);
            this.labelWebScraperWebScrapingDelay.TabIndex = 3;
            this.labelWebScraperWebScrapingDelay.Values.Text = "Web Scraping Delay (InPageScriptToRun)";
            // 
            // labelWebScraperWebScrapringDelay
            // 
            this.labelWebScraperWebScrapringDelay.Location = new System.Drawing.Point(3, 29);
            this.labelWebScraperWebScrapringDelay.Name = "labelWebScraperWebScrapringDelay";
            this.labelWebScraperWebScrapringDelay.Size = new System.Drawing.Size(228, 20);
            this.labelWebScraperWebScrapringDelay.TabIndex = 2;
            this.labelWebScraperWebScrapringDelay.Values.Text = "Web Scraping Delay (Our Script To Run)";
            // 
            // labelWebScraperJavaScriptExecuteTimeout
            // 
            this.labelWebScraperJavaScriptExecuteTimeout.Location = new System.Drawing.Point(3, 3);
            this.labelWebScraperJavaScriptExecuteTimeout.Name = "labelWebScraperJavaScriptExecuteTimeout";
            this.labelWebScraperJavaScriptExecuteTimeout.Size = new System.Drawing.Size(158, 20);
            this.labelWebScraperJavaScriptExecuteTimeout.TabIndex = 1;
            this.labelWebScraperJavaScriptExecuteTimeout.Values.Text = "JavaScript Execute Timeout";
            // 
            // labelWebScraperWebScrapingRetry
            // 
            this.labelWebScraperWebScrapingRetry.Location = new System.Drawing.Point(3, 54);
            this.labelWebScraperWebScrapingRetry.Name = "labelWebScraperWebScrapingRetry";
            this.labelWebScraperWebScrapingRetry.Size = new System.Drawing.Size(118, 20);
            this.labelWebScraperWebScrapingRetry.TabIndex = 0;
            this.labelWebScraperWebScrapingRetry.Values.Text = "Web Scraping Retry ";
            // 
            // textBoxWebScrapingStartPages
            // 
            this.textBoxWebScrapingStartPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxWebScrapingStartPages.Location = new System.Drawing.Point(0, 0);
            this.textBoxWebScrapingStartPages.Multiline = true;
            this.textBoxWebScrapingStartPages.Name = "textBoxWebScrapingStartPages";
            this.textBoxWebScrapingStartPages.Size = new System.Drawing.Size(555, 789);
            this.textBoxWebScrapingStartPages.TabIndex = 0;
            // 
            // fastColoredTextBoxConfigFilenameDateFormats
            // 
            this.fastColoredTextBoxConfigFilenameDateFormats.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxConfigFilenameDateFormats.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.fastColoredTextBoxConfigFilenameDateFormats.BackBrush = null;
            this.fastColoredTextBoxConfigFilenameDateFormats.CharHeight = 14;
            this.fastColoredTextBoxConfigFilenameDateFormats.CharWidth = 8;
            this.fastColoredTextBoxConfigFilenameDateFormats.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxConfigFilenameDateFormats.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxConfigFilenameDateFormats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastColoredTextBoxConfigFilenameDateFormats.IsReplaceMode = false;
            this.fastColoredTextBoxConfigFilenameDateFormats.Location = new System.Drawing.Point(0, 0);
            this.fastColoredTextBoxConfigFilenameDateFormats.Name = "fastColoredTextBoxConfigFilenameDateFormats";
            this.fastColoredTextBoxConfigFilenameDateFormats.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxConfigFilenameDateFormats.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxConfigFilenameDateFormats.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxConfigFilenameDateFormats.ServiceColors")));
            this.fastColoredTextBoxConfigFilenameDateFormats.Size = new System.Drawing.Size(564, 789);
            this.fastColoredTextBoxConfigFilenameDateFormats.TabIndex = 5;
            this.fastColoredTextBoxConfigFilenameDateFormats.Zoom = 100;
            // 
            // textBoxApplicationDateTimeFormatHelp
            // 
            this.textBoxApplicationDateTimeFormatHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxApplicationDateTimeFormatHelp.Location = new System.Drawing.Point(0, 0);
            this.textBoxApplicationDateTimeFormatHelp.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxApplicationDateTimeFormatHelp.Multiline = true;
            this.textBoxApplicationDateTimeFormatHelp.Name = "textBoxApplicationDateTimeFormatHelp";
            this.textBoxApplicationDateTimeFormatHelp.ReadOnly = true;
            this.textBoxApplicationDateTimeFormatHelp.Size = new System.Drawing.Size(576, 789);
            this.textBoxApplicationDateTimeFormatHelp.TabIndex = 4;
            this.textBoxApplicationDateTimeFormatHelp.Text = "When renaming media files. Date and time can be removed. This is list of date and" +
    " time formats, that will be removed from filename during rename tool. ";
            // 
            // textBoxRenameTo
            // 
            this.textBoxRenameTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRenameTo.Location = new System.Drawing.Point(118, 46);
            this.textBoxRenameTo.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxRenameTo.Name = "textBoxRenameTo";
            this.textBoxRenameTo.Size = new System.Drawing.Size(425, 23);
            this.textBoxRenameTo.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(3, 48);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 20);
            this.label10.TabIndex = 3;
            this.label10.Values.Text = "Rename file(s) to:";
            // 
            // checkBoxRename
            // 
            this.checkBoxRename.Location = new System.Drawing.Point(3, 3);
            this.checkBoxRename.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxRename.Name = "checkBoxRename";
            this.checkBoxRename.Size = new System.Drawing.Size(276, 20);
            this.checkBoxRename.TabIndex = 0;
            this.checkBoxRename.Values.Text = "Rename media file(s) after/during AutoCorrect";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 24);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 20);
            this.label9.TabIndex = 1;
            this.label9.Values.Text = "Rename variables:";
            // 
            // comboBoxRenameVariables
            // 
            this.comboBoxRenameVariables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRenameVariables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxRenameVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRenameVariables.DropDownWidth = 558;
            this.comboBoxRenameVariables.FormattingEnabled = true;
            this.comboBoxRenameVariables.IntegralHeight = false;
            this.comboBoxRenameVariables.Items.AddRange(new object[] {
            "%Trim%",
            "%FileName%",
            "%FileNameWithoutDateTime%",
            "%Extension%",
            "%MediaFileNow_DateTime%",
            "%Media_DateTime%",
            "%Media_yyyy%",
            "%Media_MM%",
            "%Media_dd%",
            "%Media_HH%",
            "%Media_mm%",
            "%Media_ss%",
            "%File_DateTime%",
            "%File_yyyy%",
            "%File_MM%",
            "%File_dd%",
            "%File_HH%",
            "%File_mm%",
            "%File_ss%",
            "%Now_DateTime%",
            "%Now_yyyy%",
            "%Now_MM%",
            "%Now_dd%",
            "%Now_HH%",
            "%Now_mm%",
            "%Now_ss%",
            "%GPS_DateTimeUTC%",
            "%MediaAlbum%",
            "%MediaTitle%",
            "%MediaDescription%",
            "%MediaAuthor%",
            "%LocationName%",
            "%LocationCountry%",
            "%LocationState%"});
            this.comboBoxRenameVariables.Location = new System.Drawing.Point(118, 23);
            this.comboBoxRenameVariables.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxRenameVariables.Name = "comboBoxRenameVariables";
            this.comboBoxRenameVariables.Size = new System.Drawing.Size(425, 21);
            this.comboBoxRenameVariables.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxRenameVariables.TabIndex = 2;
            this.comboBoxRenameVariables.SelectionChangeCommitted += new System.EventHandler(this.comboBoxRenameVariables_SelectionChangeCommitted);
            // 
            // checkBoxFaceRegionAddWebScraping
            // 
            this.checkBoxFaceRegionAddWebScraping.Checked = true;
            this.checkBoxFaceRegionAddWebScraping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFaceRegionAddWebScraping.Location = new System.Drawing.Point(3, 45);
            this.checkBoxFaceRegionAddWebScraping.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxFaceRegionAddWebScraping.Name = "checkBoxFaceRegionAddWebScraping";
            this.checkBoxFaceRegionAddWebScraping.Size = new System.Drawing.Size(267, 20);
            this.checkBoxFaceRegionAddWebScraping.TabIndex = 2;
            this.checkBoxFaceRegionAddWebScraping.Values.Text = "Add none existing names from WebScraping";
            // 
            // checkBoxFaceRegionAddMicrosoftPhotos
            // 
            this.checkBoxFaceRegionAddMicrosoftPhotos.Checked = true;
            this.checkBoxFaceRegionAddMicrosoftPhotos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFaceRegionAddMicrosoftPhotos.Location = new System.Drawing.Point(3, 24);
            this.checkBoxFaceRegionAddMicrosoftPhotos.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxFaceRegionAddMicrosoftPhotos.Name = "checkBoxFaceRegionAddMicrosoftPhotos";
            this.checkBoxFaceRegionAddMicrosoftPhotos.Size = new System.Drawing.Size(287, 20);
            this.checkBoxFaceRegionAddMicrosoftPhotos.TabIndex = 1;
            this.checkBoxFaceRegionAddMicrosoftPhotos.Values.Text = "Add none existing names from Microsoft Photos";
            // 
            // checkBoxFaceRegionAddWindowsMediaPhotoGallery
            // 
            this.checkBoxFaceRegionAddWindowsMediaPhotoGallery.Checked = true;
            this.checkBoxFaceRegionAddWindowsMediaPhotoGallery.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFaceRegionAddWindowsMediaPhotoGallery.Location = new System.Drawing.Point(3, 3);
            this.checkBoxFaceRegionAddWindowsMediaPhotoGallery.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxFaceRegionAddWindowsMediaPhotoGallery.Name = "checkBoxFaceRegionAddWindowsMediaPhotoGallery";
            this.checkBoxFaceRegionAddWindowsMediaPhotoGallery.Size = new System.Drawing.Size(359, 20);
            this.checkBoxFaceRegionAddWindowsMediaPhotoGallery.TabIndex = 0;
            this.checkBoxFaceRegionAddWindowsMediaPhotoGallery.Values.Text = "Add none existing names from Windows Media Photo Gallery";
            // 
            // checkBoxAutoCorrectTrackChanges
            // 
            this.checkBoxAutoCorrectTrackChanges.Location = new System.Drawing.Point(3, 150);
            this.checkBoxAutoCorrectTrackChanges.Name = "checkBoxAutoCorrectTrackChanges";
            this.checkBoxAutoCorrectTrackChanges.Size = new System.Drawing.Size(342, 20);
            this.checkBoxAutoCorrectTrackChanges.TabIndex = 7;
            this.checkBoxAutoCorrectTrackChanges.Values.Text = "Append text that keep track of date changes in comments.";
            // 
            // checkBoxKeywordBackupFileCreatedBefore
            // 
            this.checkBoxKeywordBackupFileCreatedBefore.Location = new System.Drawing.Point(3, 21);
            this.checkBoxKeywordBackupFileCreatedBefore.Name = "checkBoxKeywordBackupFileCreatedBefore";
            this.checkBoxKeywordBackupFileCreatedBefore.Size = new System.Drawing.Size(231, 20);
            this.checkBoxKeywordBackupFileCreatedBefore.TabIndex = 1;
            this.checkBoxKeywordBackupFileCreatedBefore.Values.Text = "Store original FileCreated in Keywords";
            // 
            // checkBoxKeywordBackupFileCreatedAfter
            // 
            this.checkBoxKeywordBackupFileCreatedAfter.Location = new System.Drawing.Point(3, 45);
            this.checkBoxKeywordBackupFileCreatedAfter.Name = "checkBoxKeywordBackupFileCreatedAfter";
            this.checkBoxKeywordBackupFileCreatedAfter.Size = new System.Drawing.Size(235, 20);
            this.checkBoxKeywordBackupFileCreatedAfter.TabIndex = 2;
            this.checkBoxKeywordBackupFileCreatedAfter.Values.Text = "Store updated FileCreated in Keywords";
            // 
            // checkBoxKeywordBackupDateTakenBefore
            // 
            this.checkBoxKeywordBackupDateTakenBefore.Location = new System.Drawing.Point(3, 66);
            this.checkBoxKeywordBackupDateTakenBefore.Name = "checkBoxKeywordBackupDateTakenBefore";
            this.checkBoxKeywordBackupDateTakenBefore.Size = new System.Drawing.Size(227, 20);
            this.checkBoxKeywordBackupDateTakenBefore.TabIndex = 3;
            this.checkBoxKeywordBackupDateTakenBefore.Values.Text = "Store original DateTaken in Keywords";
            // 
            // checkBoxKeywordBackupLocationCountry
            // 
            this.checkBoxKeywordBackupLocationCountry.Location = new System.Drawing.Point(3, 297);
            this.checkBoxKeywordBackupLocationCountry.Name = "checkBoxKeywordBackupLocationCountry";
            this.checkBoxKeywordBackupLocationCountry.Size = new System.Drawing.Size(115, 20);
            this.checkBoxKeywordBackupLocationCountry.TabIndex = 13;
            this.checkBoxKeywordBackupLocationCountry.Values.Text = "Location country";
            // 
            // checkBoxKeywordBackupGPSDateTimeUTCBefore
            // 
            this.checkBoxKeywordBackupGPSDateTimeUTCBefore.Location = new System.Drawing.Point(3, 129);
            this.checkBoxKeywordBackupGPSDateTimeUTCBefore.Name = "checkBoxKeywordBackupGPSDateTimeUTCBefore";
            this.checkBoxKeywordBackupGPSDateTimeUTCBefore.Size = new System.Drawing.Size(305, 20);
            this.checkBoxKeywordBackupGPSDateTimeUTCBefore.TabIndex = 6;
            this.checkBoxKeywordBackupGPSDateTimeUTCBefore.Values.Text = "Store updated GPS UTC Date and Time in Keywords";
            // 
            // checkBoxKeywordBackupLocationState
            // 
            this.checkBoxKeywordBackupLocationState.Location = new System.Drawing.Point(3, 276);
            this.checkBoxKeywordBackupLocationState.Name = "checkBoxKeywordBackupLocationState";
            this.checkBoxKeywordBackupLocationState.Size = new System.Drawing.Size(100, 20);
            this.checkBoxKeywordBackupLocationState.TabIndex = 12;
            this.checkBoxKeywordBackupLocationState.Values.Text = "Location state";
            // 
            // checkBoxKeywordBackupDateTakenAfter
            // 
            this.checkBoxKeywordBackupDateTakenAfter.Location = new System.Drawing.Point(3, 87);
            this.checkBoxKeywordBackupDateTakenAfter.Name = "checkBoxKeywordBackupDateTakenAfter";
            this.checkBoxKeywordBackupDateTakenAfter.Size = new System.Drawing.Size(232, 20);
            this.checkBoxKeywordBackupDateTakenAfter.TabIndex = 4;
            this.checkBoxKeywordBackupDateTakenAfter.Values.Text = "Store updated DateTaken in Keywords";
            // 
            // checkBoxKeywordBackupLocationCity
            // 
            this.checkBoxKeywordBackupLocationCity.Location = new System.Drawing.Point(3, 255);
            this.checkBoxKeywordBackupLocationCity.Name = "checkBoxKeywordBackupLocationCity";
            this.checkBoxKeywordBackupLocationCity.Size = new System.Drawing.Size(92, 20);
            this.checkBoxKeywordBackupLocationCity.TabIndex = 11;
            this.checkBoxKeywordBackupLocationCity.Values.Text = "Location city";
            // 
            // checkBoxKeywordBackupGPSDateTimeUTCAfter
            // 
            this.checkBoxKeywordBackupGPSDateTimeUTCAfter.Location = new System.Drawing.Point(3, 108);
            this.checkBoxKeywordBackupGPSDateTimeUTCAfter.Name = "checkBoxKeywordBackupGPSDateTimeUTCAfter";
            this.checkBoxKeywordBackupGPSDateTimeUTCAfter.Size = new System.Drawing.Size(300, 20);
            this.checkBoxKeywordBackupGPSDateTimeUTCAfter.TabIndex = 5;
            this.checkBoxKeywordBackupGPSDateTimeUTCAfter.Values.Text = "Store original GPS UTC Date and Time in Keywords";
            // 
            // labelAutoCorrectBackupOfTags
            // 
            this.labelAutoCorrectBackupOfTags.Location = new System.Drawing.Point(3, 192);
            this.labelAutoCorrectBackupOfTags.Name = "labelAutoCorrectBackupOfTags";
            this.labelAutoCorrectBackupOfTags.Size = new System.Drawing.Size(421, 20);
            this.labelAutoCorrectBackupOfTags.TabIndex = 8;
            this.labelAutoCorrectBackupOfTags.Values.Text = "Backup data and make fields more easier to for search in some applications";
            // 
            // checkBoxKeywordBackupLocationName
            // 
            this.checkBoxKeywordBackupLocationName.Location = new System.Drawing.Point(3, 234);
            this.checkBoxKeywordBackupLocationName.Name = "checkBoxKeywordBackupLocationName";
            this.checkBoxKeywordBackupLocationName.Size = new System.Drawing.Size(104, 20);
            this.checkBoxKeywordBackupLocationName.TabIndex = 10;
            this.checkBoxKeywordBackupLocationName.Values.Text = "Location name";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(3, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(213, 20);
            this.label17.TabIndex = 0;
            this.label17.Values.Text = "Keey track for dates and date history:";
            // 
            // checkBoxKeywordBackupRegionFaceNames
            // 
            this.checkBoxKeywordBackupRegionFaceNames.Location = new System.Drawing.Point(3, 213);
            this.checkBoxKeywordBackupRegionFaceNames.Name = "checkBoxKeywordBackupRegionFaceNames";
            this.checkBoxKeywordBackupRegionFaceNames.Size = new System.Drawing.Size(144, 20);
            this.checkBoxKeywordBackupRegionFaceNames.TabIndex = 9;
            this.checkBoxKeywordBackupRegionFaceNames.Values.Text = "All Region face names";
            // 
            // checkBoxKeywordsAddAutoKeywords
            // 
            this.checkBoxKeywordsAddAutoKeywords.Location = new System.Drawing.Point(3, 87);
            this.checkBoxKeywordsAddAutoKeywords.Name = "checkBoxKeywordsAddAutoKeywords";
            this.checkBoxKeywordsAddAutoKeywords.Size = new System.Drawing.Size(450, 20);
            this.checkBoxKeywordsAddAutoKeywords.TabIndex = 5;
            this.checkBoxKeywordsAddAutoKeywords.Values.Text = "Add AutoKeyword synonym(s), when found trigger keysin AutoKeywords table ";
            // 
            // checkBoxKeywordsAddWebScraping
            // 
            this.checkBoxKeywordsAddWebScraping.Checked = true;
            this.checkBoxKeywordsAddWebScraping.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxKeywordsAddWebScraping.Location = new System.Drawing.Point(3, 66);
            this.checkBoxKeywordsAddWebScraping.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxKeywordsAddWebScraping.Name = "checkBoxKeywordsAddWebScraping";
            this.checkBoxKeywordsAddWebScraping.Size = new System.Drawing.Size(283, 20);
            this.checkBoxKeywordsAddWebScraping.TabIndex = 4;
            this.checkBoxKeywordsAddWebScraping.Values.Text = "Add none existing keywords from WebScraping";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 45);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 20);
            this.label4.TabIndex = 2;
            this.label4.Values.Text = "Required AI Confidence";
            // 
            // comboBoxKeywordsAiConfidence
            // 
            this.comboBoxKeywordsAiConfidence.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxKeywordsAiConfidence.DropDownWidth = 150;
            this.comboBoxKeywordsAiConfidence.FormattingEnabled = true;
            this.comboBoxKeywordsAiConfidence.IntegralHeight = false;
            this.comboBoxKeywordsAiConfidence.Items.AddRange(new object[] {
            "90% Confidence",
            "80% Confidence",
            "70% Confidence",
            "60% Confidence",
            "50% Confidence",
            "40% Confidence",
            "30% Confidence",
            "20% Confidence",
            "10% Confidence"});
            this.comboBoxKeywordsAiConfidence.Location = new System.Drawing.Point(144, 43);
            this.comboBoxKeywordsAiConfidence.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxKeywordsAiConfidence.Name = "comboBoxKeywordsAiConfidence";
            this.comboBoxKeywordsAiConfidence.Size = new System.Drawing.Size(150, 21);
            this.comboBoxKeywordsAiConfidence.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxKeywordsAiConfidence.TabIndex = 3;
            // 
            // checkBoxKeywordsAddMicrosoftPhotos
            // 
            this.checkBoxKeywordsAddMicrosoftPhotos.Checked = true;
            this.checkBoxKeywordsAddMicrosoftPhotos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxKeywordsAddMicrosoftPhotos.Location = new System.Drawing.Point(3, 21);
            this.checkBoxKeywordsAddMicrosoftPhotos.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxKeywordsAddMicrosoftPhotos.Name = "checkBoxKeywordsAddMicrosoftPhotos";
            this.checkBoxKeywordsAddMicrosoftPhotos.Size = new System.Drawing.Size(303, 20);
            this.checkBoxKeywordsAddMicrosoftPhotos.TabIndex = 1;
            this.checkBoxKeywordsAddMicrosoftPhotos.Values.Text = "Add none existing keywords from Microsoft Photos";
            // 
            // checkBoxKeywordsAddWindowsMediaPhotoGallery
            // 
            this.checkBoxKeywordsAddWindowsMediaPhotoGallery.Checked = true;
            this.checkBoxKeywordsAddWindowsMediaPhotoGallery.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxKeywordsAddWindowsMediaPhotoGallery.Location = new System.Drawing.Point(3, 3);
            this.checkBoxKeywordsAddWindowsMediaPhotoGallery.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxKeywordsAddWindowsMediaPhotoGallery.Name = "checkBoxKeywordsAddWindowsMediaPhotoGallery";
            this.checkBoxKeywordsAddWindowsMediaPhotoGallery.Size = new System.Drawing.Size(375, 20);
            this.checkBoxKeywordsAddWindowsMediaPhotoGallery.TabIndex = 0;
            this.checkBoxKeywordsAddWindowsMediaPhotoGallery.Values.Text = "Add none existing keywords from Windows Media Photo Gallery";
            // 
            // radioButtonAuthorAlwaysChange
            // 
            this.radioButtonAuthorAlwaysChange.Location = new System.Drawing.Point(3, 45);
            this.radioButtonAuthorAlwaysChange.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAuthorAlwaysChange.Name = "radioButtonAuthorAlwaysChange";
            this.radioButtonAuthorAlwaysChange.Size = new System.Drawing.Size(354, 20);
            this.radioButtonAuthorAlwaysChange.TabIndex = 2;
            this.radioButtonAuthorAlwaysChange.Values.Text = "Always change Author text to Camra model and make owner";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(3, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(459, 20);
            this.label13.TabIndex = 3;
            this.label13.Values.Text = "If Camra Make/Model are not configurated with \"Owner\" no changes will be done.";
            // 
            // radioButtonAuthorChangeWhenEmpty
            // 
            this.radioButtonAuthorChangeWhenEmpty.Location = new System.Drawing.Point(3, 24);
            this.radioButtonAuthorChangeWhenEmpty.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAuthorChangeWhenEmpty.Name = "radioButtonAuthorChangeWhenEmpty";
            this.radioButtonAuthorChangeWhenEmpty.Size = new System.Drawing.Size(438, 20);
            this.radioButtonAuthorChangeWhenEmpty.TabIndex = 1;
            this.radioButtonAuthorChangeWhenEmpty.Values.Text = "Change Author text to Camra model and make owner when Author is empty";
            // 
            // radioButtonAuthorDoNotChange
            // 
            this.radioButtonAuthorDoNotChange.Location = new System.Drawing.Point(3, 3);
            this.radioButtonAuthorDoNotChange.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAuthorDoNotChange.Name = "radioButtonAuthorDoNotChange";
            this.radioButtonAuthorDoNotChange.Size = new System.Drawing.Size(161, 20);
            this.radioButtonAuthorDoNotChange.TabIndex = 0;
            this.radioButtonAuthorDoNotChange.Values.Text = "Don\'t change Author text";
            // 
            // checkBoxDublicateAlbumAsDescription
            // 
            this.checkBoxDublicateAlbumAsDescription.Location = new System.Drawing.Point(3, 66);
            this.checkBoxDublicateAlbumAsDescription.Name = "checkBoxDublicateAlbumAsDescription";
            this.checkBoxDublicateAlbumAsDescription.Size = new System.Drawing.Size(195, 20);
            this.checkBoxDublicateAlbumAsDescription.TabIndex = 7;
            this.checkBoxDublicateAlbumAsDescription.Values.Text = "Duplicate Album as Description";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(3, 109);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(124, 20);
            this.label19.TabIndex = 6;
            this.label19.Values.Text = "Priority source order:";
            // 
            // imageListViewOrderAlbum
            // 
            this.imageListViewOrderAlbum.AllowReorder = true;
            this.imageListViewOrderAlbum.LineColor = System.Drawing.Color.Red;
            this.imageListViewOrderAlbum.Location = new System.Drawing.Point(3, 133);
            this.imageListViewOrderAlbum.Margin = new System.Windows.Forms.Padding(2);
            this.imageListViewOrderAlbum.Name = "imageListViewOrderAlbum";
            this.imageListViewOrderAlbum.Size = new System.Drawing.Size(540, 110);
            this.imageListViewOrderAlbum.TabIndex = 3;
            // 
            // radioButtonAlbumUseFirst
            // 
            this.radioButtonAlbumUseFirst.Location = new System.Drawing.Point(3, 45);
            this.radioButtonAlbumUseFirst.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAlbumUseFirst.Name = "radioButtonAlbumUseFirst";
            this.radioButtonAlbumUseFirst.Size = new System.Drawing.Size(302, 20);
            this.radioButtonAlbumUseFirst.TabIndex = 2;
            this.radioButtonAlbumUseFirst.Values.Text = "Change Album text to first prioritized text in source";
            // 
            // radioButtonAlbumChangeWhenEmpty
            // 
            this.radioButtonAlbumChangeWhenEmpty.Location = new System.Drawing.Point(3, 24);
            this.radioButtonAlbumChangeWhenEmpty.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAlbumChangeWhenEmpty.Name = "radioButtonAlbumChangeWhenEmpty";
            this.radioButtonAlbumChangeWhenEmpty.Size = new System.Drawing.Size(387, 20);
            this.radioButtonAlbumChangeWhenEmpty.TabIndex = 1;
            this.radioButtonAlbumChangeWhenEmpty.Values.Text = "Change Album text to first none empty text from prioritized source";
            // 
            // radioButtonAlbumDoNotChange
            // 
            this.radioButtonAlbumDoNotChange.Location = new System.Drawing.Point(3, 3);
            this.radioButtonAlbumDoNotChange.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonAlbumDoNotChange.Name = "radioButtonAlbumDoNotChange";
            this.radioButtonAlbumDoNotChange.Size = new System.Drawing.Size(159, 20);
            this.radioButtonAlbumDoNotChange.TabIndex = 0;
            this.radioButtonAlbumDoNotChange.Values.Text = "Don\'t change Album text";
            // 
            // labelAutoCorrectTitlePrioritySource
            // 
            this.labelAutoCorrectTitlePrioritySource.Location = new System.Drawing.Point(3, 83);
            this.labelAutoCorrectTitlePrioritySource.Name = "labelAutoCorrectTitlePrioritySource";
            this.labelAutoCorrectTitlePrioritySource.Size = new System.Drawing.Size(124, 20);
            this.labelAutoCorrectTitlePrioritySource.TabIndex = 4;
            this.labelAutoCorrectTitlePrioritySource.Values.Text = "Priority source order:";
            // 
            // imageListViewOrderTitle
            // 
            this.imageListViewOrderTitle.AllowReorder = true;
            this.imageListViewOrderTitle.LineColor = System.Drawing.Color.Red;
            this.imageListViewOrderTitle.Location = new System.Drawing.Point(3, 105);
            this.imageListViewOrderTitle.Margin = new System.Windows.Forms.Padding(2);
            this.imageListViewOrderTitle.Name = "imageListViewOrderTitle";
            this.imageListViewOrderTitle.Size = new System.Drawing.Size(544, 100);
            this.imageListViewOrderTitle.TabIndex = 3;
            // 
            // radioButtonTitleUseFirst
            // 
            this.radioButtonTitleUseFirst.Location = new System.Drawing.Point(3, 45);
            this.radioButtonTitleUseFirst.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonTitleUseFirst.Name = "radioButtonTitleUseFirst";
            this.radioButtonTitleUseFirst.Size = new System.Drawing.Size(289, 20);
            this.radioButtonTitleUseFirst.TabIndex = 2;
            this.radioButtonTitleUseFirst.Values.Text = "Change Title text to first prioritized text in source";
            // 
            // radioButtonTitleChangeWhenEmpty
            // 
            this.radioButtonTitleChangeWhenEmpty.Location = new System.Drawing.Point(3, 24);
            this.radioButtonTitleChangeWhenEmpty.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonTitleChangeWhenEmpty.Name = "radioButtonTitleChangeWhenEmpty";
            this.radioButtonTitleChangeWhenEmpty.Size = new System.Drawing.Size(374, 20);
            this.radioButtonTitleChangeWhenEmpty.TabIndex = 1;
            this.radioButtonTitleChangeWhenEmpty.Values.Text = "Change Title text to first none empty text from prioritized source";
            // 
            // radioButtonTitleDoNotChange
            // 
            this.radioButtonTitleDoNotChange.Location = new System.Drawing.Point(3, 3);
            this.radioButtonTitleDoNotChange.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonTitleDoNotChange.Name = "radioButtonTitleDoNotChange";
            this.radioButtonTitleDoNotChange.Size = new System.Drawing.Size(146, 20);
            this.radioButtonTitleDoNotChange.TabIndex = 0;
            this.radioButtonTitleDoNotChange.Values.Text = "Don\'t change Title text";
            // 
            // checkBoxUpdateLocationCountry
            // 
            this.checkBoxUpdateLocationCountry.Location = new System.Drawing.Point(3, 178);
            this.checkBoxUpdateLocationCountry.Name = "checkBoxUpdateLocationCountry";
            this.checkBoxUpdateLocationCountry.Size = new System.Drawing.Size(303, 20);
            this.checkBoxUpdateLocationCountry.TabIndex = 8;
            this.checkBoxUpdateLocationCountry.Values.Text = "Updated Location Country when updates are found";
            // 
            // checkBoxUpdateLocationState
            // 
            this.checkBoxUpdateLocationState.Location = new System.Drawing.Point(3, 159);
            this.checkBoxUpdateLocationState.Name = "checkBoxUpdateLocationState";
            this.checkBoxUpdateLocationState.Size = new System.Drawing.Size(288, 20);
            this.checkBoxUpdateLocationState.TabIndex = 7;
            this.checkBoxUpdateLocationState.Values.Text = "Updated Location State when updates are found";
            // 
            // checkBoxUpdateLocationCity
            // 
            this.checkBoxUpdateLocationCity.Location = new System.Drawing.Point(3, 140);
            this.checkBoxUpdateLocationCity.Name = "checkBoxUpdateLocationCity";
            this.checkBoxUpdateLocationCity.Size = new System.Drawing.Size(281, 20);
            this.checkBoxUpdateLocationCity.TabIndex = 6;
            this.checkBoxUpdateLocationCity.Values.Text = "Updated Location City when updates are found";
            // 
            // checkBoxUpdateLocationName
            // 
            this.checkBoxUpdateLocationName.Location = new System.Drawing.Point(3, 120);
            this.checkBoxUpdateLocationName.Name = "checkBoxUpdateLocationName";
            this.checkBoxUpdateLocationName.Size = new System.Drawing.Size(293, 20);
            this.checkBoxUpdateLocationName.TabIndex = 5;
            this.checkBoxUpdateLocationName.Values.Text = "Updated Location Name when updates are found";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(3, 94);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(6, 2);
            this.label15.TabIndex = 4;
            this.label15.Values.Text = "";
            // 
            // labelAutoCorrectLocationInformationDescription
            // 
            this.labelAutoCorrectLocationInformationDescription.Location = new System.Drawing.Point(3, 83);
            this.labelAutoCorrectLocationInformationDescription.Name = "labelAutoCorrectLocationInformationDescription";
            this.labelAutoCorrectLocationInformationDescription.Size = new System.Drawing.Size(424, 36);
            this.labelAutoCorrectLocationInformationDescription.TabIndex = 3;
            this.labelAutoCorrectLocationInformationDescription.Values.Text = "Location name, region, city and country are fetch from local database first.\r\nIf " +
    "not exisit in local database it will be fetch from Internet via Nominatim.API";
            // 
            // radioButtonLocationNameChangeAlways
            // 
            this.radioButtonLocationNameChangeAlways.Location = new System.Drawing.Point(2, 45);
            this.radioButtonLocationNameChangeAlways.Name = "radioButtonLocationNameChangeAlways";
            this.radioButtonLocationNameChangeAlways.Size = new System.Drawing.Size(319, 20);
            this.radioButtonLocationNameChangeAlways.TabIndex = 2;
            this.radioButtonLocationNameChangeAlways.Values.Text = "Always change location name, region, city and country";
            // 
            // radioButtonLocationNameChangeWhenEmpty
            // 
            this.radioButtonLocationNameChangeWhenEmpty.Location = new System.Drawing.Point(3, 24);
            this.radioButtonLocationNameChangeWhenEmpty.Name = "radioButtonLocationNameChangeWhenEmpty";
            this.radioButtonLocationNameChangeWhenEmpty.Size = new System.Drawing.Size(351, 20);
            this.radioButtonLocationNameChangeWhenEmpty.TabIndex = 1;
            this.radioButtonLocationNameChangeWhenEmpty.Values.Text = "Change location name, region, city and country when empty";
            // 
            // radioButtonLocationNameDoNotChange
            // 
            this.radioButtonLocationNameDoNotChange.Location = new System.Drawing.Point(3, 3);
            this.radioButtonLocationNameDoNotChange.Name = "radioButtonLocationNameDoNotChange";
            this.radioButtonLocationNameDoNotChange.Size = new System.Drawing.Size(311, 20);
            this.radioButtonLocationNameDoNotChange.TabIndex = 0;
            this.radioButtonLocationNameDoNotChange.Values.Text = "Don\'t change location name, city, region and country";
            // 
            // label87
            // 
            this.label87.Location = new System.Drawing.Point(436, 179);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(54, 20);
            this.label87.TabIndex = 13;
            this.label87.Values.Text = "minutes";
            // 
            // numericUpDownLocationAccurateIntervalNearByMediaFile
            // 
            this.numericUpDownLocationAccurateIntervalNearByMediaFile.Location = new System.Drawing.Point(351, 177);
            this.numericUpDownLocationAccurateIntervalNearByMediaFile.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numericUpDownLocationAccurateIntervalNearByMediaFile.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLocationAccurateIntervalNearByMediaFile.Name = "numericUpDownLocationAccurateIntervalNearByMediaFile";
            this.numericUpDownLocationAccurateIntervalNearByMediaFile.Size = new System.Drawing.Size(79, 20);
            this.numericUpDownLocationAccurateIntervalNearByMediaFile.TabIndex = 12;
            this.numericUpDownLocationAccurateIntervalNearByMediaFile.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // checkBoxGPSUpdateLocationNearByMedia
            // 
            this.checkBoxGPSUpdateLocationNearByMedia.Location = new System.Drawing.Point(3, 178);
            this.checkBoxGPSUpdateLocationNearByMedia.Name = "checkBoxGPSUpdateLocationNearByMedia";
            this.checkBoxGPSUpdateLocationNearByMedia.Size = new System.Drawing.Size(337, 20);
            this.checkBoxGPSUpdateLocationNearByMedia.TabIndex = 11;
            this.checkBoxGPSUpdateLocationNearByMedia.Values.Text = "Use GPS location from other media files, if have locations.";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(436, 113);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(54, 20);
            this.label23.TabIndex = 9;
            this.label23.Values.Text = "minutes";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(436, 49);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(41, 20);
            this.label22.TabIndex = 7;
            this.label22.Values.Text = "hours";
            // 
            // numericUpDownLocationAccurateInterval
            // 
            this.numericUpDownLocationAccurateInterval.Location = new System.Drawing.Point(351, 113);
            this.numericUpDownLocationAccurateInterval.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numericUpDownLocationAccurateInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLocationAccurateInterval.Name = "numericUpDownLocationAccurateInterval";
            this.numericUpDownLocationAccurateInterval.Size = new System.Drawing.Size(79, 20);
            this.numericUpDownLocationAccurateInterval.TabIndex = 8;
            this.numericUpDownLocationAccurateInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownLocationAccurateInterval.ValueChanged += new System.EventHandler(this.numericUpDownLocationAccurateInterval_ValueChanged);
            // 
            // numericUpDownLocationGuessInterval
            // 
            this.numericUpDownLocationGuessInterval.Location = new System.Drawing.Point(351, 49);
            this.numericUpDownLocationGuessInterval.Maximum = new decimal(new int[] {
            168,
            0,
            0,
            0});
            this.numericUpDownLocationGuessInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLocationGuessInterval.Name = "numericUpDownLocationGuessInterval";
            this.numericUpDownLocationGuessInterval.Size = new System.Drawing.Size(79, 20);
            this.numericUpDownLocationGuessInterval.TabIndex = 6;
            this.numericUpDownLocationGuessInterval.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDownLocationGuessInterval.ValueChanged += new System.EventHandler(this.numericUpDownLocationGuessInterval_ValueChanged);
            // 
            // labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing
            // 
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing.Location = new System.Drawing.Point(3, 211);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing.Name = "labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing";
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing.Size = new System.Drawing.Size(164, 20);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing.TabIndex = 14;
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing.Values.Text = "GPS Date and Time missing!";
            // 
            // labelLocationTimeZoneAccurate
            // 
            this.labelLocationTimeZoneAccurate.Location = new System.Drawing.Point(3, 115);
            this.labelLocationTimeZoneAccurate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLocationTimeZoneAccurate.Name = "labelLocationTimeZoneAccurate";
            this.labelLocationTimeZoneAccurate.Size = new System.Drawing.Size(339, 20);
            this.labelLocationTimeZoneAccurate.TabIndex = 5;
            this.labelLocationTimeZoneAccurate.Values.Text = "5. Find new locations in camra owner\'s hirstory. ±60 minutes";
            // 
            // labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1
            // 
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1.Location = new System.Drawing.Point(3, 27);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1.Name = "labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1";
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1.Size = new System.Drawing.Size(322, 20);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1.TabIndex = 1;
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1.Values.Text = "1. Check if GPS DateTime exist, if not then use DateTaken";
            // 
            // labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing
            // 
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing.Location = new System.Drawing.Point(3, 3);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing.Name = "labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing";
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing.Size = new System.Drawing.Size(136, 20);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing.TabIndex = 0;
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing.Values.Text = "GPS Locations missing!";
            // 
            // labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4
            // 
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4.Location = new System.Drawing.Point(3, 92);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4.Name = "labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4";
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4.Size = new System.Drawing.Size(239, 20);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4.TabIndex = 4;
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4.Values.Text = "4. Adjust DateTaken with found time zone";
            // 
            // labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3
            // 
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3.Location = new System.Drawing.Point(3, 70);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3.Name = "labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3";
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3.Size = new System.Drawing.Size(206, 20);
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3.TabIndex = 3;
            this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3.Values.Text = "3. If location\'s found, find time zone";
            // 
            // labelLocationTimeZoneGuess
            // 
            this.labelLocationTimeZoneGuess.Location = new System.Drawing.Point(3, 49);
            this.labelLocationTimeZoneGuess.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLocationTimeZoneGuess.Name = "labelLocationTimeZoneGuess";
            this.labelLocationTimeZoneGuess.Size = new System.Drawing.Size(316, 20);
            this.labelLocationTimeZoneGuess.TabIndex = 2;
            this.labelLocationTimeZoneGuess.Values.Text = "2. Try find location in camera owner\'s history, ±24 hours";
            // 
            // checkBoxGPSUpdateDateTime
            // 
            this.checkBoxGPSUpdateDateTime.Location = new System.Drawing.Point(3, 229);
            this.checkBoxGPSUpdateDateTime.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxGPSUpdateDateTime.Name = "checkBoxGPSUpdateDateTime";
            this.checkBoxGPSUpdateDateTime.Size = new System.Drawing.Size(495, 20);
            this.checkBoxGPSUpdateDateTime.TabIndex = 15;
            this.checkBoxGPSUpdateDateTime.Values.Text = "Update GPS DateTime when missing using DateTaken and Time Zone for GPS Location ";
            // 
            // checkBoxGPSUpdateLocation
            // 
            this.checkBoxGPSUpdateLocation.Location = new System.Drawing.Point(3, 153);
            this.checkBoxGPSUpdateLocation.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxGPSUpdateLocation.Name = "checkBoxGPSUpdateLocation";
            this.checkBoxGPSUpdateLocation.Size = new System.Drawing.Size(401, 20);
            this.checkBoxGPSUpdateLocation.TabIndex = 10;
            this.checkBoxGPSUpdateLocation.Values.Text = "Update GPS Location when missing using algorithm described above.";
            // 
            // labelAutoCorrectPrioritySourceOrder
            // 
            this.labelAutoCorrectPrioritySourceOrder.Location = new System.Drawing.Point(3, 85);
            this.labelAutoCorrectPrioritySourceOrder.Name = "labelAutoCorrectPrioritySourceOrder";
            this.labelAutoCorrectPrioritySourceOrder.Size = new System.Drawing.Size(124, 20);
            this.labelAutoCorrectPrioritySourceOrder.TabIndex = 10;
            this.labelAutoCorrectPrioritySourceOrder.Values.Text = "Priority source order:";
            // 
            // radioButtonDateTakenUseFirst
            // 
            this.radioButtonDateTakenUseFirst.Location = new System.Drawing.Point(3, 51);
            this.radioButtonDateTakenUseFirst.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonDateTakenUseFirst.Name = "radioButtonDateTakenUseFirst";
            this.radioButtonDateTakenUseFirst.Size = new System.Drawing.Size(307, 20);
            this.radioButtonDateTakenUseFirst.TabIndex = 2;
            this.radioButtonDateTakenUseFirst.Values.Text = "Change DateTaken to first prioritized date&time in list";
            // 
            // radioButtonDateTakenChangeWhenEmpty
            // 
            this.radioButtonDateTakenChangeWhenEmpty.Location = new System.Drawing.Point(3, 27);
            this.radioButtonDateTakenChangeWhenEmpty.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonDateTakenChangeWhenEmpty.Name = "radioButtonDateTakenChangeWhenEmpty";
            this.radioButtonDateTakenChangeWhenEmpty.Size = new System.Drawing.Size(399, 20);
            this.radioButtonDateTakenChangeWhenEmpty.TabIndex = 1;
            this.radioButtonDateTakenChangeWhenEmpty.Values.Text = "Change DateTaken to first existing date&&time from prioritized source";
            // 
            // radioButtonDateTakenDoNotChange
            // 
            this.radioButtonDateTakenDoNotChange.Location = new System.Drawing.Point(3, 3);
            this.radioButtonDateTakenDoNotChange.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonDateTakenDoNotChange.Name = "radioButtonDateTakenDoNotChange";
            this.radioButtonDateTakenDoNotChange.Size = new System.Drawing.Size(184, 20);
            this.radioButtonDateTakenDoNotChange.TabIndex = 0;
            this.radioButtonDateTakenDoNotChange.Values.Text = "Don\'t change DateTaken field";
            // 
            // imageListViewOrderDateTaken
            // 
            this.imageListViewOrderDateTaken.AllowReorder = true;
            this.imageListViewOrderDateTaken.LineColor = System.Drawing.Color.Red;
            this.imageListViewOrderDateTaken.Location = new System.Drawing.Point(3, 108);
            this.imageListViewOrderDateTaken.Margin = new System.Windows.Forms.Padding(2);
            this.imageListViewOrderDateTaken.Name = "imageListViewOrderDateTaken";
            this.imageListViewOrderDateTaken.Size = new System.Drawing.Size(544, 122);
            this.imageListViewOrderDateTaken.TabIndex = 3;
            // 
            // textBoxHelpAutoCorrect
            // 
            this.textBoxHelpAutoCorrect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHelpAutoCorrect.Location = new System.Drawing.Point(0, 0);
            this.textBoxHelpAutoCorrect.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxHelpAutoCorrect.Multiline = true;
            this.textBoxHelpAutoCorrect.Name = "textBoxHelpAutoCorrect";
            this.textBoxHelpAutoCorrect.ReadOnly = true;
            this.textBoxHelpAutoCorrect.Size = new System.Drawing.Size(555, 789);
            this.textBoxHelpAutoCorrect.TabIndex = 0;
            this.textBoxHelpAutoCorrect.Text = resources.GetString("textBoxHelpAutoCorrect.Text");
            // 
            // dataGridViewAutoKeywords
            // 
            this.dataGridViewAutoKeywords.AllowUserToOrderColumns = true;
            this.dataGridViewAutoKeywords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewAutoKeywords.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridViewAutoKeywords.CausesValidation = false;
            this.dataGridViewAutoKeywords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAutoKeywords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LocationName,
            this.Title,
            this.Album,
            this.Description,
            this.Comments,
            this.Keywords,
            this.AddKeywords});
            this.dataGridViewAutoKeywords.ContextMenuStrip = this.contextMenuStripAutoKeyword;
            this.dataGridViewAutoKeywords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAutoKeywords.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewAutoKeywords.Name = "dataGridViewAutoKeywords";
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle33.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle33.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            dataGridViewCellStyle33.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle33.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle33.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewAutoKeywords.RowHeadersDefaultCellStyle = dataGridViewCellStyle33;
            this.dataGridViewAutoKeywords.RowHeadersWidth = 80;
            this.dataGridViewAutoKeywords.RowTemplate.Height = 24;
            this.dataGridViewAutoKeywords.Size = new System.Drawing.Size(555, 789);
            this.dataGridViewAutoKeywords.TabIndex = 0;
            this.dataGridViewAutoKeywords.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewAutoKeywords_CellBeginEdit);
            this.dataGridViewAutoKeywords.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewAutoKeywords_RowsAdded);
            this.dataGridViewAutoKeywords.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewAutoKeywords_KeyDown);
            // 
            // LocationName
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LocationName.DefaultCellStyle = dataGridViewCellStyle8;
            this.LocationName.HeaderText = "LocationName";
            this.LocationName.MinimumWidth = 6;
            this.LocationName.Name = "LocationName";
            this.LocationName.Width = 101;
            // 
            // Title
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Title.DefaultCellStyle = dataGridViewCellStyle20;
            this.Title.HeaderText = "Title";
            this.Title.MinimumWidth = 6;
            this.Title.Name = "Title";
            this.Title.Width = 52;
            // 
            // Album
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Album.DefaultCellStyle = dataGridViewCellStyle21;
            this.Album.HeaderText = "Album";
            this.Album.MinimumWidth = 6;
            this.Album.Name = "Album";
            this.Album.Width = 61;
            // 
            // Description
            // 
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Description.DefaultCellStyle = dataGridViewCellStyle29;
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 6;
            this.Description.Name = "Description";
            this.Description.Width = 85;
            // 
            // Comments
            // 
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Comments.DefaultCellStyle = dataGridViewCellStyle30;
            this.Comments.HeaderText = "Comments";
            this.Comments.MinimumWidth = 6;
            this.Comments.Name = "Comments";
            this.Comments.Width = 81;
            // 
            // Keywords
            // 
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Keywords.DefaultCellStyle = dataGridViewCellStyle31;
            this.Keywords.HeaderText = "Keywords";
            this.Keywords.MinimumWidth = 6;
            this.Keywords.Name = "Keywords";
            this.Keywords.Width = 78;
            // 
            // AddKeywords
            // 
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AddKeywords.DefaultCellStyle = dataGridViewCellStyle32;
            this.AddKeywords.HeaderText = "AddKeywords";
            this.AddKeywords.MinimumWidth = 6;
            this.AddKeywords.Name = "AddKeywords";
            this.AddKeywords.Width = 97;
            // 
            // contextMenuStripAutoKeyword
            // 
            this.contextMenuStripAutoKeyword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStripAutoKeyword.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripAutoKeyword.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAutoKeywordCut,
            this.toolStripMenuItemAutoKeywordCopy,
            this.toolStripMenuItemAutoKeywordPaste,
            this.toolStripMenuItemAutoKeywordDelete,
            this.toolStripMenuItemAutoKeywordUndo,
            this.toolStripMenuItemRedo,
            this.toolStripMenuItemFind,
            this.toolStripMenuItemReplace});
            this.contextMenuStripAutoKeyword.Name = "contextMenuStripMap";
            this.contextMenuStripAutoKeyword.Size = new System.Drawing.Size(163, 212);
            // 
            // toolStripMenuItemAutoKeywordCut
            // 
            this.toolStripMenuItemAutoKeywordCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.Cut;
            this.toolStripMenuItemAutoKeywordCut.Name = "toolStripMenuItemAutoKeywordCut";
            this.toolStripMenuItemAutoKeywordCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemAutoKeywordCut.Size = new System.Drawing.Size(162, 26);
            this.toolStripMenuItemAutoKeywordCut.Text = "Cut";
            this.toolStripMenuItemAutoKeywordCut.Click += new System.EventHandler(this.toolStripMenuItemAutoKeywordCut_Click);
            // 
            // toolStripMenuItemAutoKeywordCopy
            // 
            this.toolStripMenuItemAutoKeywordCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemAutoKeywordCopy.Name = "toolStripMenuItemAutoKeywordCopy";
            this.toolStripMenuItemAutoKeywordCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemAutoKeywordCopy.Size = new System.Drawing.Size(162, 26);
            this.toolStripMenuItemAutoKeywordCopy.Text = "Copy";
            this.toolStripMenuItemAutoKeywordCopy.Click += new System.EventHandler(this.toolStripMenuItemAutoKeywordCopy_Click);
            // 
            // toolStripMenuItemAutoKeywordPaste
            // 
            this.toolStripMenuItemAutoKeywordPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.Paste;
            this.toolStripMenuItemAutoKeywordPaste.Name = "toolStripMenuItemAutoKeywordPaste";
            this.toolStripMenuItemAutoKeywordPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.toolStripMenuItemAutoKeywordPaste.Size = new System.Drawing.Size(162, 26);
            this.toolStripMenuItemAutoKeywordPaste.Text = "Paste";
            this.toolStripMenuItemAutoKeywordPaste.Click += new System.EventHandler(this.toolStripMenuItemAutoKeywordPaste_Click);
            // 
            // toolStripMenuItemAutoKeywordDelete
            // 
            this.toolStripMenuItemAutoKeywordDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.Delete;
            this.toolStripMenuItemAutoKeywordDelete.Name = "toolStripMenuItemAutoKeywordDelete";
            this.toolStripMenuItemAutoKeywordDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemAutoKeywordDelete.Size = new System.Drawing.Size(162, 26);
            this.toolStripMenuItemAutoKeywordDelete.Text = "Delete";
            this.toolStripMenuItemAutoKeywordDelete.Click += new System.EventHandler(this.toolStripMenuItemAutoKeywordDelete_Click);
            // 
            // toolStripMenuItemAutoKeywordUndo
            // 
            this.toolStripMenuItemAutoKeywordUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Undo;
            this.toolStripMenuItemAutoKeywordUndo.Name = "toolStripMenuItemAutoKeywordUndo";
            this.toolStripMenuItemAutoKeywordUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemAutoKeywordUndo.Size = new System.Drawing.Size(162, 26);
            this.toolStripMenuItemAutoKeywordUndo.Text = "Undo";
            this.toolStripMenuItemAutoKeywordUndo.Click += new System.EventHandler(this.toolStripMenuItemAutoKeywordUndo_Click);
            // 
            // toolStripMenuItemRedo
            // 
            this.toolStripMenuItemRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Redo;
            this.toolStripMenuItemRedo.Name = "toolStripMenuItemRedo";
            this.toolStripMenuItemRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemRedo.Size = new System.Drawing.Size(162, 26);
            this.toolStripMenuItemRedo.Text = "Redo";
            this.toolStripMenuItemRedo.Click += new System.EventHandler(this.toolStripMenuItemRedo_Click);
            // 
            // toolStripMenuItemFind
            // 
            this.toolStripMenuItemFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.Find;
            this.toolStripMenuItemFind.Name = "toolStripMenuItemFind";
            this.toolStripMenuItemFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemFind.Size = new System.Drawing.Size(162, 26);
            this.toolStripMenuItemFind.Text = "Find";
            this.toolStripMenuItemFind.Click += new System.EventHandler(this.toolStripMenuItemFind_Click);
            // 
            // toolStripMenuItemReplace
            // 
            this.toolStripMenuItemReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.Replace;
            this.toolStripMenuItemReplace.Name = "toolStripMenuItemReplace";
            this.toolStripMenuItemReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemReplace.Size = new System.Drawing.Size(162, 26);
            this.toolStripMenuItemReplace.Text = "Replace";
            this.toolStripMenuItemReplace.Click += new System.EventHandler(this.toolStripMenuItemReplace_Click);
            // 
            // textBox3
            // 
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Location = new System.Drawing.Point(0, 0);
            this.textBox3.Margin = new System.Windows.Forms.Padding(2);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(511, 626);
            this.textBox3.TabIndex = 4;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // dataGridViewCameraOwner
            // 
            this.dataGridViewCameraOwner.AllowDrop = true;
            this.dataGridViewCameraOwner.AllowUserToAddRows = false;
            this.dataGridViewCameraOwner.AllowUserToDeleteRows = false;
            this.dataGridViewCameraOwner.ColumnHeadersHeight = 29;
            this.dataGridViewCameraOwner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCameraOwner.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCameraOwner.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCameraOwner.Name = "dataGridViewCameraOwner";
            this.dataGridViewCameraOwner.RowHeadersWidth = 51;
            this.dataGridViewCameraOwner.RowTemplate.Height = 24;
            this.dataGridViewCameraOwner.Size = new System.Drawing.Size(511, 626);
            this.dataGridViewCameraOwner.TabIndex = 1;
            this.dataGridViewCameraOwner.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewCameraOwner_CellBeginEdit);
            this.dataGridViewCameraOwner.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewCameraOwner_CellPainting);
            this.dataGridViewCameraOwner.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewCameraOwner_CellValidating);
            this.dataGridViewCameraOwner.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewCameraOwner_EditingControlShowing);
            this.dataGridViewCameraOwner.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewCameraOwner_KeyDown);
            // 
            // dataGridViewLocationNames
            // 
            this.dataGridViewLocationNames.AllowDrop = true;
            this.dataGridViewLocationNames.AllowUserToAddRows = false;
            this.dataGridViewLocationNames.AllowUserToDeleteRows = false;
            this.dataGridViewLocationNames.ColumnHeadersHeight = 29;
            this.dataGridViewLocationNames.ContextMenuStrip = this.contextMenuStripLocationNames;
            this.dataGridViewLocationNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLocationNames.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewLocationNames.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewLocationNames.Name = "dataGridViewLocationNames";
            this.dataGridViewLocationNames.RowHeadersWidth = 51;
            this.dataGridViewLocationNames.RowTemplate.Height = 24;
            this.dataGridViewLocationNames.Size = new System.Drawing.Size(509, 345);
            this.dataGridViewLocationNames.TabIndex = 5;
            this.dataGridViewLocationNames.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewLocationNames_CellBeginEdit);
            this.dataGridViewLocationNames.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewLocationNames_CellMouseDoubleClick);
            this.dataGridViewLocationNames.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewLocationNames_CellPainting);
            this.dataGridViewLocationNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewLocationNames_KeyDown);
            // 
            // contextMenuStripLocationNames
            // 
            this.contextMenuStripLocationNames.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripLocationNames.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemMapCut,
            this.toolStripMenuItemMapCopy,
            this.toolStripMenuItemMapPaste,
            this.toolStripMenuItemMapDelete,
            this.toolStripMenuItemMapUndo,
            this.toolStripMenuItemMapRedo,
            this.toolStripMenuItemMapFind,
            this.toolStripMenuItemMapReplace,
            this.toolStripMenuItemMapMarkFavorite,
            this.toolStripMenuItemMapRemoveFavorite,
            this.toolStripMenuItemMapToggleFavorite,
            this.toolStripMenuItemMapShowFavorite,
            this.toolStripMenuItemMapHideEqual,
            this.toolStripMenuItemShowCoordinateOnMap,
            this.toolStripMenuItemShowCoordinateOnGoogleMap,
            this.toolStripMenuItemMapReloadLocationUsingNominatim,
            this.searchForNewLocationsInMediaFilesToolStripMenuItem});
            this.contextMenuStripLocationNames.Name = "contextMenuStripMap";
            this.contextMenuStripLocationNames.Size = new System.Drawing.Size(366, 446);
            // 
            // toolStripMenuItemMapCut
            // 
            this.toolStripMenuItemMapCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.Cut;
            this.toolStripMenuItemMapCut.Name = "toolStripMenuItemMapCut";
            this.toolStripMenuItemMapCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemMapCut.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapCut.Text = "Cut";
            this.toolStripMenuItemMapCut.Click += new System.EventHandler(this.toolStripMenuItemMapCut_Click);
            // 
            // toolStripMenuItemMapCopy
            // 
            this.toolStripMenuItemMapCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemMapCopy.Name = "toolStripMenuItemMapCopy";
            this.toolStripMenuItemMapCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemMapCopy.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapCopy.Text = "Copy";
            this.toolStripMenuItemMapCopy.Click += new System.EventHandler(this.toolStripMenuItemMapCopy_Click);
            // 
            // toolStripMenuItemMapPaste
            // 
            this.toolStripMenuItemMapPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.Paste;
            this.toolStripMenuItemMapPaste.Name = "toolStripMenuItemMapPaste";
            this.toolStripMenuItemMapPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.toolStripMenuItemMapPaste.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapPaste.Text = "Paste";
            this.toolStripMenuItemMapPaste.Click += new System.EventHandler(this.toolStripMenuItemMapPaste_Click);
            // 
            // toolStripMenuItemMapDelete
            // 
            this.toolStripMenuItemMapDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.Delete;
            this.toolStripMenuItemMapDelete.Name = "toolStripMenuItemMapDelete";
            this.toolStripMenuItemMapDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemMapDelete.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapDelete.Text = "Delete";
            this.toolStripMenuItemMapDelete.Click += new System.EventHandler(this.toolStripMenuItemMapDelete_Click);
            // 
            // toolStripMenuItemMapUndo
            // 
            this.toolStripMenuItemMapUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Undo;
            this.toolStripMenuItemMapUndo.Name = "toolStripMenuItemMapUndo";
            this.toolStripMenuItemMapUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemMapUndo.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapUndo.Text = "Undo";
            this.toolStripMenuItemMapUndo.Click += new System.EventHandler(this.toolStripMenuItemMapUndo_Click);
            // 
            // toolStripMenuItemMapRedo
            // 
            this.toolStripMenuItemMapRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Redo;
            this.toolStripMenuItemMapRedo.Name = "toolStripMenuItemMapRedo";
            this.toolStripMenuItemMapRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemMapRedo.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapRedo.Text = "Redo";
            this.toolStripMenuItemMapRedo.Click += new System.EventHandler(this.toolStripMenuItemMapRedo_Click);
            // 
            // toolStripMenuItemMapFind
            // 
            this.toolStripMenuItemMapFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.Find;
            this.toolStripMenuItemMapFind.Name = "toolStripMenuItemMapFind";
            this.toolStripMenuItemMapFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.toolStripMenuItemMapFind.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapFind.Text = "Find";
            this.toolStripMenuItemMapFind.Click += new System.EventHandler(this.toolStripMenuItemMapFind_Click);
            // 
            // toolStripMenuItemMapReplace
            // 
            this.toolStripMenuItemMapReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.Replace;
            this.toolStripMenuItemMapReplace.Name = "toolStripMenuItemMapReplace";
            this.toolStripMenuItemMapReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemMapReplace.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapReplace.Text = "Replace";
            this.toolStripMenuItemMapReplace.Click += new System.EventHandler(this.toolStripMenuItemMapReplace_Click);
            // 
            // toolStripMenuItemMapMarkFavorite
            // 
            this.toolStripMenuItemMapMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect;
            this.toolStripMenuItemMapMarkFavorite.Name = "toolStripMenuItemMapMarkFavorite";
            this.toolStripMenuItemMapMarkFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemMapMarkFavorite.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapMarkFavorite.Text = "Mark as favorite";
            this.toolStripMenuItemMapMarkFavorite.Click += new System.EventHandler(this.toolStripMenuItemMapMarkFavorite_Click);
            // 
            // toolStripMenuItemMapRemoveFavorite
            // 
            this.toolStripMenuItemMapRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove;
            this.toolStripMenuItemMapRemoveFavorite.Name = "toolStripMenuItemMapRemoveFavorite";
            this.toolStripMenuItemMapRemoveFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemMapRemoveFavorite.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapRemoveFavorite.Text = "Remove as favorite";
            this.toolStripMenuItemMapRemoveFavorite.Click += new System.EventHandler(this.toolStripMenuItemMapRemoveFavorite_Click);
            // 
            // toolStripMenuItemMapToggleFavorite
            // 
            this.toolStripMenuItemMapToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle;
            this.toolStripMenuItemMapToggleFavorite.Name = "toolStripMenuItemMapToggleFavorite";
            this.toolStripMenuItemMapToggleFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemMapToggleFavorite.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapToggleFavorite.Text = "Toggle favorite";
            this.toolStripMenuItemMapToggleFavorite.Click += new System.EventHandler(this.toolStripMenuItemMapToggleFavorite_Click);
            // 
            // toolStripMenuItemMapShowFavorite
            // 
            this.toolStripMenuItemMapShowFavorite.Name = "toolStripMenuItemMapShowFavorite";
            this.toolStripMenuItemMapShowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemMapShowFavorite.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapShowFavorite.Text = "Show favorite rows";
            this.toolStripMenuItemMapShowFavorite.Click += new System.EventHandler(this.toolStripMenuItemMapShowFavorite_Click);
            // 
            // toolStripMenuItemMapHideEqual
            // 
            this.toolStripMenuItemMapHideEqual.Name = "toolStripMenuItemMapHideEqual";
            this.toolStripMenuItemMapHideEqual.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemMapHideEqual.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapHideEqual.Text = "Hide equal rows";
            this.toolStripMenuItemMapHideEqual.Click += new System.EventHandler(this.toolStripMenuItemMapHideEqual_Click);
            // 
            // toolStripMenuItemShowCoordinateOnMap
            // 
            this.toolStripMenuItemShowCoordinateOnMap.Image = global::PhotoTagsSynchronizer.Properties.Resources.ShowLocation;
            this.toolStripMenuItemShowCoordinateOnMap.Name = "toolStripMenuItemShowCoordinateOnMap";
            this.toolStripMenuItemShowCoordinateOnMap.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemShowCoordinateOnMap.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemShowCoordinateOnMap.Text = "Show Coordinate on OpenStreetMap";
            this.toolStripMenuItemShowCoordinateOnMap.Click += new System.EventHandler(this.toolStripMenuItemShowCoordinateOnMap_Click);
            // 
            // toolStripMenuItemShowCoordinateOnGoogleMap
            // 
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Image = global::PhotoTagsSynchronizer.Properties.Resources.ShowLocation;
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Name = "toolStripMenuItemShowCoordinateOnGoogleMap";
            this.toolStripMenuItemShowCoordinateOnGoogleMap.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Text = "Show Coordinate on Google Map";
            this.toolStripMenuItemShowCoordinateOnGoogleMap.Click += new System.EventHandler(this.toolStripMenuItemShowCoordinateOnGoogleMap_Click);
            // 
            // toolStripMenuItemMapReloadLocationUsingNominatim
            // 
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Image = global::PhotoTagsSynchronizer.Properties.Resources.LocationReload;
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Name = "toolStripMenuItemMapReloadLocationUsingNominatim";
            this.toolStripMenuItemMapReloadLocationUsingNominatim.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Size = new System.Drawing.Size(365, 26);
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Text = "Reload Location Information using Nominatim";
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Click += new System.EventHandler(this.toolStripMenuItemMapReloadLocationUsingNominatim_Click);
            // 
            // searchForNewLocationsInMediaFilesToolStripMenuItem
            // 
            this.searchForNewLocationsInMediaFilesToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.LocationSearch;
            this.searchForNewLocationsInMediaFilesToolStripMenuItem.Name = "searchForNewLocationsInMediaFilesToolStripMenuItem";
            this.searchForNewLocationsInMediaFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.searchForNewLocationsInMediaFilesToolStripMenuItem.Size = new System.Drawing.Size(365, 26);
            this.searchForNewLocationsInMediaFilesToolStripMenuItem.Text = "Search for new Locations in Media files";
            this.searchForNewLocationsInMediaFilesToolStripMenuItem.Click += new System.EventHandler(this.searchForNewLocationsInMediaFilesToolStripMenuItem_Click);
            // 
            // textBoxBrowserURL
            // 
            this.textBoxBrowserURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBrowserURL.Location = new System.Drawing.Point(150, 187);
            this.textBoxBrowserURL.Name = "textBoxBrowserURL";
            this.textBoxBrowserURL.Size = new System.Drawing.Size(353, 23);
            this.textBoxBrowserURL.TabIndex = 17;
            this.textBoxBrowserURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxBrowserURL_KeyPress);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(7, 186);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 26);
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // comboBoxMapZoomLevel
            // 
            this.comboBoxMapZoomLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxMapZoomLevel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxMapZoomLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMapZoomLevel.DropDownWidth = 97;
            this.comboBoxMapZoomLevel.FormattingEnabled = true;
            this.comboBoxMapZoomLevel.IntegralHeight = false;
            this.comboBoxMapZoomLevel.Items.AddRange(new object[] {
            "Zoom 1",
            "Zoom 2",
            "Zoom 3",
            "Zoom 4",
            "Zoom 5",
            "Zoom 6",
            "Zoom 7",
            "Zoom 8",
            "Zoom 9",
            "Zoom 10",
            "Zoom 11",
            "Zoom 12",
            "Zoom 13",
            "Zoom 14",
            "Zoom 15",
            "Zoom 16",
            "Zoom 17",
            "Zoom 18"});
            this.comboBoxMapZoomLevel.Location = new System.Drawing.Point(47, 187);
            this.comboBoxMapZoomLevel.Name = "comboBoxMapZoomLevel";
            this.comboBoxMapZoomLevel.Size = new System.Drawing.Size(97, 21);
            this.comboBoxMapZoomLevel.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxMapZoomLevel.TabIndex = 18;
            this.comboBoxMapZoomLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxMapZoomLevel_SelectedIndexChanged);
            // 
            // panelBrowser
            // 
            this.panelBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBrowser.Location = new System.Drawing.Point(3, 3);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(504, 178);
            this.panelBrowser.TabIndex = 7;
            // 
            // buttonLocationImport
            // 
            this.buttonLocationImport.Location = new System.Drawing.Point(101, 5);
            this.buttonLocationImport.Name = "buttonLocationImport";
            this.buttonLocationImport.Size = new System.Drawing.Size(91, 26);
            this.buttonLocationImport.TabIndex = 8;
            this.buttonLocationImport.Values.Text = "Import";
            this.buttonLocationImport.Click += new System.EventHandler(this.buttonLocationImport_Click);
            // 
            // buttonLocationExport
            // 
            this.buttonLocationExport.Location = new System.Drawing.Point(4, 5);
            this.buttonLocationExport.Name = "buttonLocationExport";
            this.buttonLocationExport.Size = new System.Drawing.Size(91, 25);
            this.buttonLocationExport.TabIndex = 7;
            this.buttonLocationExport.Values.Text = "Export";
            this.buttonLocationExport.Click += new System.EventHandler(this.buttonLocationExport_Click);
            // 
            // textBox4
            // 
            this.textBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox4.Location = new System.Drawing.Point(0, 0);
            this.textBox4.Margin = new System.Windows.Forms.Padding(2);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(511, 626);
            this.textBox4.TabIndex = 6;
            this.textBox4.Text = "Location names is to set names automaticly on location based un GPS coordinates. " +
    "E.g. Home, Zoo Park, Cabin, Parents Home, etc.";
            // 
            // labelApplicationGPSLocationAccuracy
            // 
            this.labelApplicationGPSLocationAccuracy.Location = new System.Drawing.Point(3, 3);
            this.labelApplicationGPSLocationAccuracy.Name = "labelApplicationGPSLocationAccuracy";
            this.labelApplicationGPSLocationAccuracy.Size = new System.Drawing.Size(537, 20);
            this.labelApplicationGPSLocationAccuracy.TabIndex = 4;
            this.labelApplicationGPSLocationAccuracy.Values.Text = "When lookup location from Database, threat numbers as eaual using this parameters" +
    " as accuracy";
            // 
            // label65
            // 
            this.label65.Location = new System.Drawing.Point(3, 56);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(66, 20);
            this.label65.TabIndex = 3;
            this.label65.Values.Text = "Longitude";
            // 
            // label64
            // 
            this.label64.Location = new System.Drawing.Point(3, 30);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(57, 20);
            this.label64.TabIndex = 2;
            this.label64.Values.Text = "Latitude:";
            // 
            // numericUpDownLocationAccuracyLongitude
            // 
            this.numericUpDownLocationAccuracyLongitude.DecimalPlaces = 4;
            this.numericUpDownLocationAccuracyLongitude.Increment = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            this.numericUpDownLocationAccuracyLongitude.Location = new System.Drawing.Point(118, 56);
            this.numericUpDownLocationAccuracyLongitude.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.numericUpDownLocationAccuracyLongitude.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            this.numericUpDownLocationAccuracyLongitude.Name = "numericUpDownLocationAccuracyLongitude";
            this.numericUpDownLocationAccuracyLongitude.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownLocationAccuracyLongitude.TabIndex = 1;
            this.numericUpDownLocationAccuracyLongitude.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // numericUpDownLocationAccuracyLatitude
            // 
            this.numericUpDownLocationAccuracyLatitude.DecimalPlaces = 4;
            this.numericUpDownLocationAccuracyLatitude.Increment = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            this.numericUpDownLocationAccuracyLatitude.Location = new System.Drawing.Point(118, 30);
            this.numericUpDownLocationAccuracyLatitude.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.numericUpDownLocationAccuracyLatitude.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            262144});
            this.numericUpDownLocationAccuracyLatitude.Name = "numericUpDownLocationAccuracyLatitude";
            this.numericUpDownLocationAccuracyLatitude.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownLocationAccuracyLatitude.TabIndex = 0;
            this.numericUpDownLocationAccuracyLatitude.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // checkBoxApplicationAvoidReadExifFromCloud
            // 
            this.checkBoxApplicationAvoidReadExifFromCloud.Location = new System.Drawing.Point(3, 22);
            this.checkBoxApplicationAvoidReadExifFromCloud.Name = "checkBoxApplicationAvoidReadExifFromCloud";
            this.checkBoxApplicationAvoidReadExifFromCloud.Size = new System.Drawing.Size(229, 20);
            this.checkBoxApplicationAvoidReadExifFromCloud.TabIndex = 1;
            this.checkBoxApplicationAvoidReadExifFromCloud.Values.Text = "Avoid use exiftool on files from Cloud";
            // 
            // checkBoxApplicationImageListViewCacheModeOnDemand
            // 
            this.checkBoxApplicationImageListViewCacheModeOnDemand.Location = new System.Drawing.Point(3, 42);
            this.checkBoxApplicationImageListViewCacheModeOnDemand.Name = "checkBoxApplicationImageListViewCacheModeOnDemand";
            this.checkBoxApplicationImageListViewCacheModeOnDemand.Size = new System.Drawing.Size(517, 20);
            this.checkBoxApplicationImageListViewCacheModeOnDemand.TabIndex = 2;
            this.checkBoxApplicationImageListViewCacheModeOnDemand.Values.Text = "Load Image Thumbnails On Demand (what\'s visible on screen). Unchecked load all at" +
    " once.";
            // 
            // checkBoxApplicationAvoidReadMediaFromCloud
            // 
            this.checkBoxApplicationAvoidReadMediaFromCloud.Location = new System.Drawing.Point(3, 3);
            this.checkBoxApplicationAvoidReadMediaFromCloud.Name = "checkBoxApplicationAvoidReadMediaFromCloud";
            this.checkBoxApplicationAvoidReadMediaFromCloud.Size = new System.Drawing.Size(210, 20);
            this.checkBoxApplicationAvoidReadMediaFromCloud.TabIndex = 0;
            this.checkBoxApplicationAvoidReadMediaFromCloud.Values.Text = "Avoid read media files from Cloud";
            // 
            // labelApplicationRegionAccuracyDescription
            // 
            this.labelApplicationRegionAccuracyDescription.Location = new System.Drawing.Point(301, 30);
            this.labelApplicationRegionAccuracyDescription.Name = "labelApplicationRegionAccuracyDescription";
            this.labelApplicationRegionAccuracyDescription.Size = new System.Drawing.Size(163, 20);
            this.labelApplicationRegionAccuracyDescription.TabIndex = 3;
            this.labelApplicationRegionAccuracyDescription.Values.Text = "when diffrence are less than";
            // 
            // labelApplicationRegionAccuracyHelp
            // 
            this.labelApplicationRegionAccuracyHelp.Location = new System.Drawing.Point(3, 3);
            this.labelApplicationRegionAccuracyHelp.Name = "labelApplicationRegionAccuracyHelp";
            this.labelApplicationRegionAccuracyHelp.Size = new System.Drawing.Size(504, 20);
            this.labelApplicationRegionAccuracyHelp.TabIndex = 2;
            this.labelApplicationRegionAccuracyHelp.Values.Text = "When region from diffrent AI engines find e.g. face, they don\'t have excat same r" +
    "egion size";
            // 
            // labelApplicationRegionAccuracy
            // 
            this.labelApplicationRegionAccuracy.Location = new System.Drawing.Point(3, 29);
            this.labelApplicationRegionAccuracy.Name = "labelApplicationRegionAccuracy";
            this.labelApplicationRegionAccuracy.Size = new System.Drawing.Size(135, 20);
            this.labelApplicationRegionAccuracy.TabIndex = 1;
            this.labelApplicationRegionAccuracy.Values.Text = "Threat region as equal:";
            // 
            // numericUpDownRegionMissmatchProcent
            // 
            this.numericUpDownRegionMissmatchProcent.DecimalPlaces = 2;
            this.numericUpDownRegionMissmatchProcent.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDownRegionMissmatchProcent.Location = new System.Drawing.Point(175, 30);
            this.numericUpDownRegionMissmatchProcent.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            this.numericUpDownRegionMissmatchProcent.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDownRegionMissmatchProcent.Name = "numericUpDownRegionMissmatchProcent";
            this.numericUpDownRegionMissmatchProcent.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownRegionMissmatchProcent.TabIndex = 0;
            this.numericUpDownRegionMissmatchProcent.Value = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            // 
            // labelApplicationNumberOfMostCommonDescription
            // 
            this.labelApplicationNumberOfMostCommonDescription.Location = new System.Drawing.Point(294, 29);
            this.labelApplicationNumberOfMostCommonDescription.Name = "labelApplicationNumberOfMostCommonDescription";
            this.labelApplicationNumberOfMostCommonDescription.Size = new System.Drawing.Size(259, 20);
            this.labelApplicationNumberOfMostCommonDescription.TabIndex = 11;
            this.labelApplicationNumberOfMostCommonDescription.Values.Text = "Show most common names not already listed";
            // 
            // labelApplicationNumberOfDaysDescription
            // 
            this.labelApplicationNumberOfDaysDescription.Location = new System.Drawing.Point(294, 3);
            this.labelApplicationNumberOfDaysDescription.Name = "labelApplicationNumberOfDaysDescription";
            this.labelApplicationNumberOfDaysDescription.Size = new System.Drawing.Size(314, 20);
            this.labelApplicationNumberOfDaysDescription.TabIndex = 10;
            this.labelApplicationNumberOfDaysDescription.Values.Text = "Show names used in other media files with date interval";
            // 
            // labelApplicationNumberOfMostCommon
            // 
            this.labelApplicationNumberOfMostCommon.Location = new System.Drawing.Point(3, 29);
            this.labelApplicationNumberOfMostCommon.Name = "labelApplicationNumberOfMostCommon";
            this.labelApplicationNumberOfMostCommon.Size = new System.Drawing.Size(155, 20);
            this.labelApplicationNumberOfMostCommon.TabIndex = 3;
            this.labelApplicationNumberOfMostCommon.Values.Text = "Number of most common:";
            // 
            // labelApplicationNumberOfDays
            // 
            this.labelApplicationNumberOfDays.Location = new System.Drawing.Point(3, 3);
            this.labelApplicationNumberOfDays.Name = "labelApplicationNumberOfDays";
            this.labelApplicationNumberOfDays.Size = new System.Drawing.Size(101, 20);
            this.labelApplicationNumberOfDays.TabIndex = 2;
            this.labelApplicationNumberOfDays.Values.Text = "Number of days:";
            // 
            // numericUpDownPeopleSuggestNameTopMost
            // 
            this.numericUpDownPeopleSuggestNameTopMost.Location = new System.Drawing.Point(167, 29);
            this.numericUpDownPeopleSuggestNameTopMost.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownPeopleSuggestNameTopMost.Name = "numericUpDownPeopleSuggestNameTopMost";
            this.numericUpDownPeopleSuggestNameTopMost.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPeopleSuggestNameTopMost.TabIndex = 1;
            this.numericUpDownPeopleSuggestNameTopMost.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDownPeopleSuggestNameDaysInterval
            // 
            this.numericUpDownPeopleSuggestNameDaysInterval.Location = new System.Drawing.Point(168, 3);
            this.numericUpDownPeopleSuggestNameDaysInterval.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numericUpDownPeopleSuggestNameDaysInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPeopleSuggestNameDaysInterval.Name = "numericUpDownPeopleSuggestNameDaysInterval";
            this.numericUpDownPeopleSuggestNameDaysInterval.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPeopleSuggestNameDaysInterval.TabIndex = 0;
            this.numericUpDownPeopleSuggestNameDaysInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDownApplicationMaxRowsInSearchResult
            // 
            this.numericUpDownApplicationMaxRowsInSearchResult.Location = new System.Drawing.Point(152, 3);
            this.numericUpDownApplicationMaxRowsInSearchResult.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownApplicationMaxRowsInSearchResult.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownApplicationMaxRowsInSearchResult.Name = "numericUpDownApplicationMaxRowsInSearchResult";
            this.numericUpDownApplicationMaxRowsInSearchResult.Size = new System.Drawing.Size(172, 20);
            this.numericUpDownApplicationMaxRowsInSearchResult.TabIndex = 0;
            this.numericUpDownApplicationMaxRowsInSearchResult.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // labelApplicationSearch
            // 
            this.labelApplicationSearch.Location = new System.Drawing.Point(3, 3);
            this.labelApplicationSearch.Name = "labelApplicationSearch";
            this.labelApplicationSearch.Size = new System.Drawing.Size(151, 20);
            this.labelApplicationSearch.TabIndex = 0;
            this.labelApplicationSearch.Values.Text = "Max rows in search result:";
            // 
            // labelApplicationNominatimPreferredLanguagesHelp
            // 
            this.labelApplicationNominatimPreferredLanguagesHelp.Location = new System.Drawing.Point(152, 78);
            this.labelApplicationNominatimPreferredLanguagesHelp.Name = "labelApplicationNominatimPreferredLanguagesHelp";
            this.labelApplicationNominatimPreferredLanguagesHelp.Size = new System.Drawing.Size(432, 36);
            this.labelApplicationNominatimPreferredLanguagesHelp.TabIndex = 6;
            this.labelApplicationNominatimPreferredLanguagesHelp.Values.Text = "Nominatim location look-up will only be preformed once per location.\r\nWhen locati" +
    "on infomration is found, this data will be cached in local database";
            // 
            // labelApplicationNominatimTitle
            // 
            this.labelApplicationNominatimTitle.Location = new System.Drawing.Point(152, 3);
            this.labelApplicationNominatimTitle.Name = "labelApplicationNominatimTitle";
            this.labelApplicationNominatimTitle.Size = new System.Drawing.Size(300, 20);
            this.labelApplicationNominatimTitle.TabIndex = 5;
            this.labelApplicationNominatimTitle.Values.Text = "Language for city and country on Nominatim Lookup";
            // 
            // labelApplicationPreferredLanguages
            // 
            this.labelApplicationPreferredLanguages.Location = new System.Drawing.Point(3, 54);
            this.labelApplicationPreferredLanguages.Name = "labelApplicationPreferredLanguages";
            this.labelApplicationPreferredLanguages.Size = new System.Drawing.Size(122, 20);
            this.labelApplicationPreferredLanguages.TabIndex = 2;
            this.labelApplicationPreferredLanguages.Values.Text = "PreferredLanguages:";
            // 
            // textBoxApplicationPreferredLanguages
            // 
            this.textBoxApplicationPreferredLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApplicationPreferredLanguages.Location = new System.Drawing.Point(152, 51);
            this.textBoxApplicationPreferredLanguages.Name = "textBoxApplicationPreferredLanguages";
            this.textBoxApplicationPreferredLanguages.Size = new System.Drawing.Size(416, 23);
            this.textBoxApplicationPreferredLanguages.TabIndex = 1;
            // 
            // comboBoxApplicationLanguages
            // 
            this.comboBoxApplicationLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxApplicationLanguages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxApplicationLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApplicationLanguages.DropDownWidth = 504;
            this.comboBoxApplicationLanguages.FormattingEnabled = true;
            this.comboBoxApplicationLanguages.IntegralHeight = false;
            this.comboBoxApplicationLanguages.Items.AddRange(new object[] {
            "af - Afrikaans",
            "sq - Albanian",
            "ar - Arabic (Standard)",
            "ar-dz - Arabic (Algeria)",
            "ar-bh - Arabic (Bahrain)",
            "ar-eg - Arabic (Egypt)",
            "ar-iq - Arabic (Iraq)",
            "ar-jo - Arabic (Jordan)",
            "ar-kw - Arabic (Kuwait)",
            "ar-lb - Arabic (Lebanon)",
            "ar-ly - Arabic (Libya)",
            "ar-ma - Arabic (Morocco)",
            "ar-om - Arabic (Oman)",
            "ar-qa - Arabic (Qatar)",
            "ar-sa - Arabic (Saudi Arabia)",
            "ar-sy - Arabic (Syria)",
            "ar-tn - Arabic (Tunisia)",
            "ar-ae - Arabic (U.A.E.)",
            "ar-ye - Arabic (Yemen)",
            "ar - Aragonese",
            "hy - Armenian",
            "as - Assamese",
            "ast - Asturian",
            "az - Azerbaijani",
            "eu - Basque",
            "bg - Bulgarian",
            "be - Belarusian",
            "bn - Bengali",
            "bs - Bosnian",
            "br - Breton",
            "bg - Bulgarian",
            "my - Burmese",
            "ca - Catalan",
            "ch - Chamorro",
            "ce - Chechen",
            "zh - Chinese",
            "zh-hk - Chinese (Hong Kong)",
            "zh-cn - Chinese (PRC)",
            "zh-sg - Chinese (Singapore)",
            "zh-tw - Chinese (Taiwan)",
            "cv - Chuvash",
            "co - Corsican",
            "cr - Cree",
            "hr - Croatian",
            "cs - Czech",
            "da - Danish",
            "nl - Dutch (Standard)",
            "nl-be - Dutch (Belgian)",
            "en - English",
            "en-au - English (Australia)",
            "en-bz - English (Belize)",
            "en-ca - English (Canada)",
            "en-ie - English (Ireland)",
            "en-jm - English (Jamaica)",
            "en-nz - English (New Zealand)",
            "en-ph - English (Philippines)",
            "en-za - English (South Africa)",
            "en-tt - English (Trinidad & Tobago)",
            "en-gb - English (United Kingdom)",
            "en-us - English (United States)",
            "en-zw - English (Zimbabwe)",
            "eo - Esperanto",
            "et - Estonian",
            "fo - Faeroese",
            "fa - Farsi",
            "fj - Fijian",
            "fi - Finnish",
            "fr - French (Standard)",
            "fr-be - French (Belgium)",
            "fr-ca - French (Canada)",
            "fr-fr - French (France)",
            "fr-lu - French (Luxembourg)",
            "fr-mc - French (Monaco)",
            "fr-ch - French (Switzerland)",
            "fy - Frisian",
            "fur - Friulian",
            "gd - Gaelic (Scots)",
            "gd-ie - Gaelic (Irish)",
            "gl - Galacian",
            "ka - Georgian",
            "de - German (Standard)",
            "de-at - German (Austria)",
            "de-de - German (Germany)",
            "de-li - German (Liechtenstein)",
            "de-lu - German (Luxembourg)",
            "de-ch - German (Switzerland)",
            "el - Greek",
            "gu - Gujurati",
            "ht - Haitian",
            "he - Hebrew",
            "hi - Hindi",
            "hu - Hungarian",
            "is - Icelandic",
            "id - Indonesian",
            "iu - Inuktitut",
            "ga - Irish",
            "it - Italian (Standard)",
            "it-ch - Italian (Switzerland)",
            "ja - Japanese",
            "kn - Kannada",
            "ks - Kashmiri",
            "kk - Kazakh",
            "km - Khmer",
            "ky - Kirghiz",
            "tlh - Klingon",
            "ko - Korean",
            "ko-kp - Korean (North Korea)",
            "ko-kr - Korean (South Korea)",
            "la - Latin",
            "lv - Latvian",
            "lt - Lithuanian",
            "lb - Luxembourgish",
            "mk - FYRO Macedonian",
            "ms - Malay",
            "ml - Malayalam",
            "mt - Maltese",
            "mi - Maori",
            "mr - Marathi",
            "mo - Moldavian",
            "nv - Navajo",
            "ng - Ndonga",
            "ne - Nepali",
            "no - Norwegian",
            "nb - Norwegian (Bokmal)",
            "nn - Norwegian (Nynorsk)",
            "oc - Occitan",
            "or - Oriya",
            "om - Oromo",
            "fa - Persian",
            "fa-ir - Persian/Iran",
            "pl - Polish",
            "pt - Portuguese",
            "pt-br - Portuguese (Brazil)",
            "pa - Punjabi",
            "pa-in - Punjabi (India)",
            "pa-pk - Punjabi (Pakistan)",
            "qu - Quechua",
            "rm - Rhaeto-Romanic",
            "ro - Romanian",
            "ro-mo - Romanian (Moldavia)",
            "ru - Russian",
            "ru-mo - Russian (Moldavia)",
            "sz - Sami (Lappish)",
            "sg - Sango",
            "sa - Sanskrit",
            "sc - Sardinian",
            "gd - Scots Gaelic",
            "sd - Sindhi",
            "si - Singhalese",
            "sr - Serbian",
            "sk - Slovak",
            "sl - Slovenian",
            "so - Somani",
            "sb - Sorbian",
            "es - Spanish",
            "es-ar - Spanish (Argentina)",
            "es-bo - Spanish (Bolivia)",
            "es-cl - Spanish (Chile)",
            "es-co - Spanish (Colombia)",
            "es-cr - Spanish (Costa Rica)",
            "es-do - Spanish (Dominican Republic)",
            "es-ec - Spanish (Ecuador)",
            "es-sv - Spanish (El Salvador)",
            "es-gt - Spanish (Guatemala)",
            "es-hn - Spanish (Honduras)",
            "es-mx - Spanish (Mexico)",
            "es-ni - Spanish (Nicaragua)",
            "es-pa - Spanish (Panama)",
            "es-py - Spanish (Paraguay)",
            "es-pe - Spanish (Peru)",
            "es-pr - Spanish (Puerto Rico)",
            "es-es - Spanish (Spain)",
            "es-uy - Spanish (Uruguay)",
            "es-ve - Spanish (Venezuela)",
            "sx - Sutu",
            "sw - Swahili",
            "sv - Swedish",
            "sv-fi - Swedish (Finland)",
            "sv-sv - Swedish (Sweden)",
            "ta - Tamil",
            "tt - Tatar",
            "te - Teluga",
            "th - Thai",
            "tig - Tigre",
            "ts - Tsonga",
            "tn - Tswana",
            "tr - Turkish",
            "tk - Turkmen",
            "uk - Ukrainian",
            "hsb - Upper Sorbian",
            "ur - Urdu",
            "ve - Venda",
            "vi - Vietnamese",
            "vo - Volapuk",
            "wa - Walloon",
            "cy - Welsh",
            "xh - Xhosa",
            "ji - Yiddish",
            "zu - Zulu"});
            this.comboBoxApplicationLanguages.Location = new System.Drawing.Point(152, 24);
            this.comboBoxApplicationLanguages.Name = "comboBoxApplicationLanguages";
            this.comboBoxApplicationLanguages.Size = new System.Drawing.Size(416, 21);
            this.comboBoxApplicationLanguages.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxApplicationLanguages.TabIndex = 0;
            this.comboBoxApplicationLanguages.SelectionChangeCommitted += new System.EventHandler(this.comboBoxApplicationLanguages_SelectionChangeCommitted);
            // 
            // labelApplicationRegionThumbnailSizeDescription
            // 
            this.labelApplicationRegionThumbnailSizeDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelApplicationRegionThumbnailSizeDescription.Location = new System.Drawing.Point(330, 28);
            this.labelApplicationRegionThumbnailSizeDescription.Name = "labelApplicationRegionThumbnailSizeDescription";
            this.labelApplicationRegionThumbnailSizeDescription.Size = new System.Drawing.Size(256, 20);
            this.labelApplicationRegionThumbnailSizeDescription.TabIndex = 7;
            this.labelApplicationRegionThumbnailSizeDescription.Values.Text = "Region can Face or other regions with names";
            // 
            // labelApplicationPosterThumbnailSizeDescription
            // 
            this.labelApplicationPosterThumbnailSizeDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelApplicationPosterThumbnailSizeDescription.Location = new System.Drawing.Point(330, 4);
            this.labelApplicationPosterThumbnailSizeDescription.Name = "labelApplicationPosterThumbnailSizeDescription";
            this.labelApplicationPosterThumbnailSizeDescription.Size = new System.Drawing.Size(266, 20);
            this.labelApplicationPosterThumbnailSizeDescription.TabIndex = 6;
            this.labelApplicationPosterThumbnailSizeDescription.Values.Text = "Poster means; first frame of video or the image";
            // 
            // comboBoxApplicationRegionThumbnailSizes
            // 
            this.comboBoxApplicationRegionThumbnailSizes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxApplicationRegionThumbnailSizes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApplicationRegionThumbnailSizes.DropDownWidth = 172;
            this.comboBoxApplicationRegionThumbnailSizes.FormattingEnabled = true;
            this.comboBoxApplicationRegionThumbnailSizes.IntegralHeight = false;
            this.comboBoxApplicationRegionThumbnailSizes.Location = new System.Drawing.Point(152, 28);
            this.comboBoxApplicationRegionThumbnailSizes.Name = "comboBoxApplicationRegionThumbnailSizes";
            this.comboBoxApplicationRegionThumbnailSizes.Size = new System.Drawing.Size(172, 21);
            this.comboBoxApplicationRegionThumbnailSizes.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxApplicationRegionThumbnailSizes.TabIndex = 1;
            // 
            // labelApplicationThumbnailSizeHelp
            // 
            this.labelApplicationThumbnailSizeHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelApplicationThumbnailSizeHelp.Location = new System.Drawing.Point(3, 53);
            this.labelApplicationThumbnailSizeHelp.Name = "labelApplicationThumbnailSizeHelp";
            this.labelApplicationThumbnailSizeHelp.Size = new System.Drawing.Size(421, 36);
            this.labelApplicationThumbnailSizeHelp.TabIndex = 2;
            this.labelApplicationThumbnailSizeHelp.Values.Text = "This is the size of thumbnail saved in the local cache database. \r\nSmaler size = " +
    "faster and much smaller database, but more blury thumbnails";
            // 
            // comboBoxApplicationThumbnailSizes
            // 
            this.comboBoxApplicationThumbnailSizes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxApplicationThumbnailSizes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApplicationThumbnailSizes.DropDownWidth = 172;
            this.comboBoxApplicationThumbnailSizes.FormattingEnabled = true;
            this.comboBoxApplicationThumbnailSizes.IntegralHeight = false;
            this.comboBoxApplicationThumbnailSizes.Location = new System.Drawing.Point(152, 3);
            this.comboBoxApplicationThumbnailSizes.Name = "comboBoxApplicationThumbnailSizes";
            this.comboBoxApplicationThumbnailSizes.Size = new System.Drawing.Size(172, 21);
            this.comboBoxApplicationThumbnailSizes.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxApplicationThumbnailSizes.TabIndex = 0;
            // 
            // labelApplicationPosterThumbnailSize
            // 
            this.labelApplicationPosterThumbnailSize.Location = new System.Drawing.Point(3, 3);
            this.labelApplicationPosterThumbnailSize.Name = "labelApplicationPosterThumbnailSize";
            this.labelApplicationPosterThumbnailSize.Size = new System.Drawing.Size(132, 20);
            this.labelApplicationPosterThumbnailSize.TabIndex = 0;
            this.labelApplicationPosterThumbnailSize.Values.Text = "Poster Thumbnail size:";
            // 
            // dataGridViewMetadataReadPriority
            // 
            this.dataGridViewMetadataReadPriority.AllowDrop = true;
            this.dataGridViewMetadataReadPriority.AllowUserToAddRows = false;
            this.dataGridViewMetadataReadPriority.AllowUserToDeleteRows = false;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMetadataReadPriority.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle26;
            this.dataGridViewMetadataReadPriority.ColumnHeadersHeight = 29;
            this.dataGridViewMetadataReadPriority.ContextMenuStrip = this.contextMenuStripMetadataRead;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle34.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewMetadataReadPriority.DefaultCellStyle = dataGridViewCellStyle34;
            this.dataGridViewMetadataReadPriority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewMetadataReadPriority.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMetadataReadPriority.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewMetadataReadPriority.Name = "dataGridViewMetadataReadPriority";
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle35.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle35.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMetadataReadPriority.RowHeadersDefaultCellStyle = dataGridViewCellStyle35;
            this.dataGridViewMetadataReadPriority.RowHeadersWidth = 51;
            this.dataGridViewMetadataReadPriority.RowTemplate.Height = 24;
            this.dataGridViewMetadataReadPriority.Size = new System.Drawing.Size(556, 789);
            this.dataGridViewMetadataReadPriority.TabIndex = 0;
            this.dataGridViewMetadataReadPriority.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewMetadataReadPriority_CellBeginEdit);
            this.dataGridViewMetadataReadPriority.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewMetadataReadPriority_CellPainting);
            this.dataGridViewMetadataReadPriority.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMetadataReadPriority_CellValueChanged);
            this.dataGridViewMetadataReadPriority.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewMetadataReadPriority_DragDrop);
            this.dataGridViewMetadataReadPriority.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewMetadataReadPriority_DragOver);
            this.dataGridViewMetadataReadPriority.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewMetadataReadPriority_KeyDown);
            this.dataGridViewMetadataReadPriority.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewMetadataReadPriority_MouseDown);
            this.dataGridViewMetadataReadPriority.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewMetadataReadPriority_MouseMove);
            // 
            // contextMenuStripMetadataRead
            // 
            this.contextMenuStripMetadataRead.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripMetadataRead.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemMetadataReadMove,
            this.toolStripMenuItemMetadataReadCut,
            this.toolStripMenuItemMetadataReadCopy,
            this.toolStripMenuItemMetadataReadPaste,
            this.toolStripMenuItemMetadataReadDelete,
            this.toolStripMenuItemMetadataReadUndo,
            this.toolStripMenuItemMetadataReadRedo,
            this.toolStripMenuItemMetadataReadFind,
            this.toolStripMenuItemMetadataReadReplace,
            this.toolStripMenuItemMetadataReadMarkFavorite,
            this.toolStripMenuItemMetadataReadRemoveFavorite,
            this.toolStripMenuItemMetadataReadToggleFavorite,
            this.toolStripMenuItemMetadataReadShowFavorite});
            this.contextMenuStripMetadataRead.Name = "contextMenuStripMap";
            this.contextMenuStripMetadataRead.Size = new System.Drawing.Size(179, 342);
            // 
            // toolStripMenuItemMetadataReadMove
            // 
            this.toolStripMenuItemMetadataReadMove.Name = "toolStripMenuItemMetadataReadMove";
            this.toolStripMenuItemMetadataReadMove.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadMove.Text = "Assign to tag";
            // 
            // toolStripMenuItemMetadataReadCut
            // 
            this.toolStripMenuItemMetadataReadCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.Cut;
            this.toolStripMenuItemMetadataReadCut.Name = "toolStripMenuItemMetadataReadCut";
            this.toolStripMenuItemMetadataReadCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemMetadataReadCut.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadCut.Text = "Cut";
            this.toolStripMenuItemMetadataReadCut.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadCut_Click);
            // 
            // toolStripMenuItemMetadataReadCopy
            // 
            this.toolStripMenuItemMetadataReadCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemMetadataReadCopy.Name = "toolStripMenuItemMetadataReadCopy";
            this.toolStripMenuItemMetadataReadCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemMetadataReadCopy.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadCopy.Text = "Copy";
            this.toolStripMenuItemMetadataReadCopy.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadCopy_Click);
            // 
            // toolStripMenuItemMetadataReadPaste
            // 
            this.toolStripMenuItemMetadataReadPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.Paste;
            this.toolStripMenuItemMetadataReadPaste.Name = "toolStripMenuItemMetadataReadPaste";
            this.toolStripMenuItemMetadataReadPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.toolStripMenuItemMetadataReadPaste.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadPaste.Text = "Paste";
            this.toolStripMenuItemMetadataReadPaste.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadPaste_Click);
            // 
            // toolStripMenuItemMetadataReadDelete
            // 
            this.toolStripMenuItemMetadataReadDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.Delete;
            this.toolStripMenuItemMetadataReadDelete.Name = "toolStripMenuItemMetadataReadDelete";
            this.toolStripMenuItemMetadataReadDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemMetadataReadDelete.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadDelete.Text = "Delete";
            this.toolStripMenuItemMetadataReadDelete.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadDelete_Click);
            // 
            // toolStripMenuItemMetadataReadUndo
            // 
            this.toolStripMenuItemMetadataReadUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Undo;
            this.toolStripMenuItemMetadataReadUndo.Name = "toolStripMenuItemMetadataReadUndo";
            this.toolStripMenuItemMetadataReadUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemMetadataReadUndo.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadUndo.Text = "Undo";
            this.toolStripMenuItemMetadataReadUndo.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadUndo_Click);
            // 
            // toolStripMenuItemMetadataReadRedo
            // 
            this.toolStripMenuItemMetadataReadRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Redo;
            this.toolStripMenuItemMetadataReadRedo.Name = "toolStripMenuItemMetadataReadRedo";
            this.toolStripMenuItemMetadataReadRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemMetadataReadRedo.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadRedo.Text = "Redo";
            this.toolStripMenuItemMetadataReadRedo.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadRedo_Click);
            // 
            // toolStripMenuItemMetadataReadFind
            // 
            this.toolStripMenuItemMetadataReadFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.Find;
            this.toolStripMenuItemMetadataReadFind.Name = "toolStripMenuItemMetadataReadFind";
            this.toolStripMenuItemMetadataReadFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemMetadataReadFind.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadFind.Text = "Find";
            this.toolStripMenuItemMetadataReadFind.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadFind_Click);
            // 
            // toolStripMenuItemMetadataReadReplace
            // 
            this.toolStripMenuItemMetadataReadReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.Replace;
            this.toolStripMenuItemMetadataReadReplace.Name = "toolStripMenuItemMetadataReadReplace";
            this.toolStripMenuItemMetadataReadReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemMetadataReadReplace.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadReplace.Text = "Replace";
            this.toolStripMenuItemMetadataReadReplace.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadReplace_Click);
            // 
            // toolStripMenuItemMetadataReadMarkFavorite
            // 
            this.toolStripMenuItemMetadataReadMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect;
            this.toolStripMenuItemMetadataReadMarkFavorite.Name = "toolStripMenuItemMetadataReadMarkFavorite";
            this.toolStripMenuItemMetadataReadMarkFavorite.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadMarkFavorite.Text = "Mark as favorite";
            this.toolStripMenuItemMetadataReadMarkFavorite.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadMarkFavorite_Click);
            // 
            // toolStripMenuItemMetadataReadRemoveFavorite
            // 
            this.toolStripMenuItemMetadataReadRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove;
            this.toolStripMenuItemMetadataReadRemoveFavorite.Name = "toolStripMenuItemMetadataReadRemoveFavorite";
            this.toolStripMenuItemMetadataReadRemoveFavorite.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadRemoveFavorite.Text = "Remove as favorite";
            this.toolStripMenuItemMetadataReadRemoveFavorite.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadRemoveFavorite_Click);
            // 
            // toolStripMenuItemMetadataReadToggleFavorite
            // 
            this.toolStripMenuItemMetadataReadToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle;
            this.toolStripMenuItemMetadataReadToggleFavorite.Name = "toolStripMenuItemMetadataReadToggleFavorite";
            this.toolStripMenuItemMetadataReadToggleFavorite.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadToggleFavorite.Text = "Toggle favorite";
            this.toolStripMenuItemMetadataReadToggleFavorite.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadToggleFavorite_Click);
            // 
            // toolStripMenuItemMetadataReadShowFavorite
            // 
            this.toolStripMenuItemMetadataReadShowFavorite.Name = "toolStripMenuItemMetadataReadShowFavorite";
            this.toolStripMenuItemMetadataReadShowFavorite.Size = new System.Drawing.Size(178, 26);
            this.toolStripMenuItemMetadataReadShowFavorite.Text = "Show favorite rows";
            this.toolStripMenuItemMetadataReadShowFavorite.Click += new System.EventHandler(this.toolStripMenuItemMetadataReadShowFavorite_Click);
            // 
            // labelMetadataFileCreateDateTimDiffrentDescription
            // 
            this.labelMetadataFileCreateDateTimDiffrentDescription.Location = new System.Drawing.Point(258, 39);
            this.labelMetadataFileCreateDateTimDiffrentDescription.Name = "labelMetadataFileCreateDateTimDiffrentDescription";
            this.labelMetadataFileCreateDateTimDiffrentDescription.Size = new System.Drawing.Size(380, 20);
            this.labelMetadataFileCreateDateTimDiffrentDescription.TabIndex = 4;
            this.labelMetadataFileCreateDateTimDiffrentDescription.Values.Text = "sec. Accepted time diffrence between Media taken and File Created.";
            // 
            // labelMetadataFileCreateDateTimDiffrent
            // 
            this.labelMetadataFileCreateDateTimDiffrent.Location = new System.Drawing.Point(11, 39);
            this.labelMetadataFileCreateDateTimDiffrent.Name = "labelMetadataFileCreateDateTimDiffrent";
            this.labelMetadataFileCreateDateTimDiffrent.Size = new System.Drawing.Size(91, 20);
            this.labelMetadataFileCreateDateTimDiffrent.TabIndex = 3;
            this.labelMetadataFileCreateDateTimDiffrent.Values.Text = "Time diffrence:";
            // 
            // numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted
            // 
            this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted.Location = new System.Drawing.Point(129, 40);
            this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted.Maximum = new decimal(new int[] {
            7200,
            0,
            0,
            0});
            this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted.Name = "numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted";
            this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted.TabIndex = 1;
            this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // checkBoxWriteFileAttributeCreatedDate
            // 
            this.checkBoxWriteFileAttributeCreatedDate.Location = new System.Drawing.Point(17, 8);
            this.checkBoxWriteFileAttributeCreatedDate.Name = "checkBoxWriteFileAttributeCreatedDate";
            this.checkBoxWriteFileAttributeCreatedDate.Size = new System.Drawing.Size(507, 20);
            this.checkBoxWriteFileAttributeCreatedDate.TabIndex = 0;
            this.checkBoxWriteFileAttributeCreatedDate.Values.Text = "Update File Created Date/Time when Media taken exists and time zone can be estima" +
    "ted";
            // 
            // checkBoxWriteMetadataAddAutoKeywords
            // 
            this.checkBoxWriteMetadataAddAutoKeywords.Location = new System.Drawing.Point(4, 29);
            this.checkBoxWriteMetadataAddAutoKeywords.Name = "checkBoxWriteMetadataAddAutoKeywords";
            this.checkBoxWriteMetadataAddAutoKeywords.Size = new System.Drawing.Size(448, 20);
            this.checkBoxWriteMetadataAddAutoKeywords.TabIndex = 6;
            this.checkBoxWriteMetadataAddAutoKeywords.Values.Text = "Add AutoKeyword synonym(s), when found trigger key in AutoKeywords table ";
            // 
            // comboBoxMetadataWriteKeywordAdd
            // 
            this.comboBoxMetadataWriteKeywordAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMetadataWriteKeywordAdd.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxMetadataWriteKeywordAdd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMetadataWriteKeywordAdd.DropDownWidth = 352;
            this.comboBoxMetadataWriteKeywordAdd.FormattingEnabled = true;
            this.comboBoxMetadataWriteKeywordAdd.IntegralHeight = false;
            this.comboBoxMetadataWriteKeywordAdd.Location = new System.Drawing.Point(372, 3);
            this.comboBoxMetadataWriteKeywordAdd.Name = "comboBoxMetadataWriteKeywordAdd";
            this.comboBoxMetadataWriteKeywordAdd.Size = new System.Drawing.Size(174, 21);
            this.comboBoxMetadataWriteKeywordAdd.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxMetadataWriteKeywordAdd.TabIndex = 3;
            this.comboBoxMetadataWriteKeywordAdd.SelectionChangeCommitted += new System.EventHandler(this.comboBoxMetadataWriteKeywordAdd_SelectionChangeCommitted);
            // 
            // fastColoredTextBoxMetadataWriteKeywordAdd
            // 
            this.fastColoredTextBoxMetadataWriteKeywordAdd.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxMetadataWriteKeywordAdd.AutoScrollMinSize = new System.Drawing.Size(179, 14);
            this.fastColoredTextBoxMetadataWriteKeywordAdd.BackBrush = null;
            this.fastColoredTextBoxMetadataWriteKeywordAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxMetadataWriteKeywordAdd.CharHeight = 14;
            this.fastColoredTextBoxMetadataWriteKeywordAdd.CharWidth = 8;
            this.fastColoredTextBoxMetadataWriteKeywordAdd.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxMetadataWriteKeywordAdd.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxMetadataWriteKeywordAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fastColoredTextBoxMetadataWriteKeywordAdd.IsReplaceMode = false;
            this.fastColoredTextBoxMetadataWriteKeywordAdd.Location = new System.Drawing.Point(0, 30);
            this.fastColoredTextBoxMetadataWriteKeywordAdd.Name = "fastColoredTextBoxMetadataWriteKeywordAdd";
            this.fastColoredTextBoxMetadataWriteKeywordAdd.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxMetadataWriteKeywordAdd.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxMetadataWriteKeywordAdd.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxMetadataWriteKeywordAdd.ServiceColors")));
            this.fastColoredTextBoxMetadataWriteKeywordAdd.Size = new System.Drawing.Size(556, 759);
            this.fastColoredTextBoxMetadataWriteKeywordAdd.TabIndex = 2;
            this.fastColoredTextBoxMetadataWriteKeywordAdd.Text = "fastColoredTextBox1";
            this.fastColoredTextBoxMetadataWriteKeywordAdd.Zoom = 100;
            this.fastColoredTextBoxMetadataWriteKeywordAdd.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxMetadataWriteKeywordAdd_TextChanged);
            this.fastColoredTextBoxMetadataWriteKeywordAdd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fastColoredTextBoxMetadataWriteKeywordAdd_KeyDown);
            // 
            // fastColoredTextBoxMetadataWriteKeywordDelete
            // 
            this.fastColoredTextBoxMetadataWriteKeywordDelete.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxMetadataWriteKeywordDelete.AutoScrollMinSize = new System.Drawing.Size(179, 14);
            this.fastColoredTextBoxMetadataWriteKeywordDelete.BackBrush = null;
            this.fastColoredTextBoxMetadataWriteKeywordDelete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxMetadataWriteKeywordDelete.CharHeight = 14;
            this.fastColoredTextBoxMetadataWriteKeywordDelete.CharWidth = 8;
            this.fastColoredTextBoxMetadataWriteKeywordDelete.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxMetadataWriteKeywordDelete.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxMetadataWriteKeywordDelete.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fastColoredTextBoxMetadataWriteKeywordDelete.IsReplaceMode = false;
            this.fastColoredTextBoxMetadataWriteKeywordDelete.Location = new System.Drawing.Point(0, 53);
            this.fastColoredTextBoxMetadataWriteKeywordDelete.Name = "fastColoredTextBoxMetadataWriteKeywordDelete";
            this.fastColoredTextBoxMetadataWriteKeywordDelete.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxMetadataWriteKeywordDelete.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxMetadataWriteKeywordDelete.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxMetadataWriteKeywordDelete.ServiceColors")));
            this.fastColoredTextBoxMetadataWriteKeywordDelete.Size = new System.Drawing.Size(556, 736);
            this.fastColoredTextBoxMetadataWriteKeywordDelete.TabIndex = 0;
            this.fastColoredTextBoxMetadataWriteKeywordDelete.Text = "fastColoredTextBox1";
            this.fastColoredTextBoxMetadataWriteKeywordDelete.Zoom = 100;
            this.fastColoredTextBoxMetadataWriteKeywordDelete.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxMetadataWriteKeywordDelete_TextChanged);
            this.fastColoredTextBoxMetadataWriteKeywordDelete.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fastColoredTextBoxMetadataWriteKeywordDelete_KeyDown);
            // 
            // comboBoxMetadataWriteKeywordDelete
            // 
            this.comboBoxMetadataWriteKeywordDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMetadataWriteKeywordDelete.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxMetadataWriteKeywordDelete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMetadataWriteKeywordDelete.DropDownWidth = 352;
            this.comboBoxMetadataWriteKeywordDelete.FormattingEnabled = true;
            this.comboBoxMetadataWriteKeywordDelete.IntegralHeight = false;
            this.comboBoxMetadataWriteKeywordDelete.Location = new System.Drawing.Point(372, 3);
            this.comboBoxMetadataWriteKeywordDelete.Name = "comboBoxMetadataWriteKeywordDelete";
            this.comboBoxMetadataWriteKeywordDelete.Size = new System.Drawing.Size(174, 21);
            this.comboBoxMetadataWriteKeywordDelete.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxMetadataWriteKeywordDelete.TabIndex = 1;
            this.comboBoxMetadataWriteKeywordDelete.SelectionChangeCommitted += new System.EventHandler(this.comboBoxMetadataWriteKeywordDelete_SelectionChangeCommitted);
            // 
            // labelMetadataForeachDeletedKeyword
            // 
            this.labelMetadataForeachDeletedKeyword.Location = new System.Drawing.Point(3, 3);
            this.labelMetadataForeachDeletedKeyword.Name = "labelMetadataForeachDeletedKeyword";
            this.labelMetadataForeachDeletedKeyword.Size = new System.Drawing.Size(159, 20);
            this.labelMetadataForeachDeletedKeyword.TabIndex = 5;
            this.labelMetadataForeachDeletedKeyword.Values.Text = "For each deleted keywords:";
            // 
            // labelMetadataForeachNewKeyword
            // 
            this.labelMetadataForeachNewKeyword.Location = new System.Drawing.Point(3, 3);
            this.labelMetadataForeachNewKeyword.Name = "labelMetadataForeachNewKeyword";
            this.labelMetadataForeachNewKeyword.Size = new System.Drawing.Size(262, 20);
            this.labelMetadataForeachNewKeyword.TabIndex = 4;
            this.labelMetadataForeachNewKeyword.Values.Text = "For every keyword deleted and before adding:";
            // 
            // textBoxWriteXtraAtomArtist
            // 
            this.textBoxWriteXtraAtomArtist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWriteXtraAtomArtist.Location = new System.Drawing.Point(370, 239);
            this.textBoxWriteXtraAtomArtist.Name = "textBoxWriteXtraAtomArtist";
            this.textBoxWriteXtraAtomArtist.Size = new System.Drawing.Size(174, 23);
            this.textBoxWriteXtraAtomArtist.TabIndex = 16;
            this.textBoxWriteXtraAtomArtist.Enter += new System.EventHandler(this.textBoxWriteXtraAtomArtist_Enter);
            // 
            // checkBoxWriteXtraAtomArtistVideo
            // 
            this.checkBoxWriteXtraAtomArtistVideo.Location = new System.Drawing.Point(142, 241);
            this.checkBoxWriteXtraAtomArtistVideo.Name = "checkBoxWriteXtraAtomArtistVideo";
            this.checkBoxWriteXtraAtomArtistVideo.Size = new System.Drawing.Size(55, 20);
            this.checkBoxWriteXtraAtomArtistVideo.TabIndex = 15;
            this.checkBoxWriteXtraAtomArtistVideo.Values.Text = "Video";
            // 
            // label37
            // 
            this.label37.Location = new System.Drawing.Point(12, 242);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(72, 20);
            this.label37.TabIndex = 26;
            this.label37.Values.Text = "Write Artist";
            // 
            // labelMetadataWriteOnVideoAndPictureFilesVariables
            // 
            this.labelMetadataWriteOnVideoAndPictureFilesVariables.Location = new System.Drawing.Point(293, 12);
            this.labelMetadataWriteOnVideoAndPictureFilesVariables.Name = "labelMetadataWriteOnVideoAndPictureFilesVariables";
            this.labelMetadataWriteOnVideoAndPictureFilesVariables.Size = new System.Drawing.Size(63, 20);
            this.labelMetadataWriteOnVideoAndPictureFilesVariables.TabIndex = 25;
            this.labelMetadataWriteOnVideoAndPictureFilesVariables.Values.Text = "Variables:";
            // 
            // comboBoxWriteXtraAtomVariables
            // 
            this.comboBoxWriteXtraAtomVariables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxWriteXtraAtomVariables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxWriteXtraAtomVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWriteXtraAtomVariables.DropDownWidth = 352;
            this.comboBoxWriteXtraAtomVariables.FormattingEnabled = true;
            this.comboBoxWriteXtraAtomVariables.IntegralHeight = false;
            this.comboBoxWriteXtraAtomVariables.Location = new System.Drawing.Point(370, 9);
            this.comboBoxWriteXtraAtomVariables.Name = "comboBoxWriteXtraAtomVariables";
            this.comboBoxWriteXtraAtomVariables.Size = new System.Drawing.Size(174, 21);
            this.comboBoxWriteXtraAtomVariables.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxWriteXtraAtomVariables.TabIndex = 0;
            this.comboBoxWriteXtraAtomVariables.SelectionChangeCommitted += new System.EventHandler(this.comboBoxWriteXtraAtomVariables_SelectionChangeCommitted);
            // 
            // textBoxWriteXtraAtomComment
            // 
            this.textBoxWriteXtraAtomComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWriteXtraAtomComment.Location = new System.Drawing.Point(370, 205);
            this.textBoxWriteXtraAtomComment.Name = "textBoxWriteXtraAtomComment";
            this.textBoxWriteXtraAtomComment.Size = new System.Drawing.Size(174, 23);
            this.textBoxWriteXtraAtomComment.TabIndex = 14;
            this.textBoxWriteXtraAtomComment.Enter += new System.EventHandler(this.textBoxWriteXtraAtomComment_Enter);
            // 
            // textBoxWriteXtraAtomSubject
            // 
            this.textBoxWriteXtraAtomSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWriteXtraAtomSubject.Location = new System.Drawing.Point(370, 172);
            this.textBoxWriteXtraAtomSubject.Name = "textBoxWriteXtraAtomSubject";
            this.textBoxWriteXtraAtomSubject.Size = new System.Drawing.Size(174, 23);
            this.textBoxWriteXtraAtomSubject.TabIndex = 11;
            this.textBoxWriteXtraAtomSubject.Enter += new System.EventHandler(this.textBoxWriteXtraAtomSubject_Enter);
            // 
            // textBoxWriteXtraAtomSubtitle
            // 
            this.textBoxWriteXtraAtomSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWriteXtraAtomSubtitle.Location = new System.Drawing.Point(370, 139);
            this.textBoxWriteXtraAtomSubtitle.Name = "textBoxWriteXtraAtomSubtitle";
            this.textBoxWriteXtraAtomSubtitle.Size = new System.Drawing.Size(174, 23);
            this.textBoxWriteXtraAtomSubtitle.TabIndex = 8;
            this.textBoxWriteXtraAtomSubtitle.Enter += new System.EventHandler(this.textBoxWriteXtraAtomSubtitle_Enter);
            // 
            // textBoxWriteXtraAtomAlbum
            // 
            this.textBoxWriteXtraAtomAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWriteXtraAtomAlbum.Location = new System.Drawing.Point(370, 106);
            this.textBoxWriteXtraAtomAlbum.Name = "textBoxWriteXtraAtomAlbum";
            this.textBoxWriteXtraAtomAlbum.Size = new System.Drawing.Size(174, 23);
            this.textBoxWriteXtraAtomAlbum.TabIndex = 6;
            this.textBoxWriteXtraAtomAlbum.Enter += new System.EventHandler(this.textBoxWriteXtraAtomAlbum_Enter);
            // 
            // textBoxWriteXtraAtomCategories
            // 
            this.textBoxWriteXtraAtomCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWriteXtraAtomCategories.Location = new System.Drawing.Point(370, 73);
            this.textBoxWriteXtraAtomCategories.Name = "textBoxWriteXtraAtomCategories";
            this.textBoxWriteXtraAtomCategories.Size = new System.Drawing.Size(174, 23);
            this.textBoxWriteXtraAtomCategories.TabIndex = 4;
            this.textBoxWriteXtraAtomCategories.Enter += new System.EventHandler(this.textBoxWriteXtraAtomCategories_Enter);
            // 
            // textBoxWriteXtraAtomKeywords
            // 
            this.textBoxWriteXtraAtomKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWriteXtraAtomKeywords.Location = new System.Drawing.Point(370, 40);
            this.textBoxWriteXtraAtomKeywords.Name = "textBoxWriteXtraAtomKeywords";
            this.textBoxWriteXtraAtomKeywords.Size = new System.Drawing.Size(174, 23);
            this.textBoxWriteXtraAtomKeywords.TabIndex = 2;
            this.textBoxWriteXtraAtomKeywords.Enter += new System.EventHandler(this.textBoxWriteXtraAtomKeywords_Enter);
            // 
            // labelMetadataWriteOnVideoAndPictureFiles
            // 
            this.labelMetadataWriteOnVideoAndPictureFiles.Location = new System.Drawing.Point(12, 12);
            this.labelMetadataWriteOnVideoAndPictureFiles.Name = "labelMetadataWriteOnVideoAndPictureFiles";
            this.labelMetadataWriteOnVideoAndPictureFiles.Size = new System.Drawing.Size(196, 20);
            this.labelMetadataWriteOnVideoAndPictureFiles.TabIndex = 17;
            this.labelMetadataWriteOnVideoAndPictureFiles.Values.Text = "Write on video and or picture files";
            // 
            // checkBoxWriteXtraAtomRatingPicture
            // 
            this.checkBoxWriteXtraAtomRatingPicture.Location = new System.Drawing.Point(246, 288);
            this.checkBoxWriteXtraAtomRatingPicture.Name = "checkBoxWriteXtraAtomRatingPicture";
            this.checkBoxWriteXtraAtomRatingPicture.Size = new System.Drawing.Size(61, 20);
            this.checkBoxWriteXtraAtomRatingPicture.TabIndex = 18;
            this.checkBoxWriteXtraAtomRatingPicture.Values.Text = "Picture";
            // 
            // checkBoxWriteXtraAtomRatingVideo
            // 
            this.checkBoxWriteXtraAtomRatingVideo.Location = new System.Drawing.Point(142, 288);
            this.checkBoxWriteXtraAtomRatingVideo.Name = "checkBoxWriteXtraAtomRatingVideo";
            this.checkBoxWriteXtraAtomRatingVideo.Size = new System.Drawing.Size(55, 20);
            this.checkBoxWriteXtraAtomRatingVideo.TabIndex = 17;
            this.checkBoxWriteXtraAtomRatingVideo.Values.Text = "Video";
            // 
            // label34
            // 
            this.label34.Location = new System.Drawing.Point(12, 289);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(79, 20);
            this.label34.TabIndex = 14;
            this.label34.Values.Text = "Write Rating";
            // 
            // checkBoxWriteXtraAtomCommentPicture
            // 
            this.checkBoxWriteXtraAtomCommentPicture.Location = new System.Drawing.Point(246, 207);
            this.checkBoxWriteXtraAtomCommentPicture.Name = "checkBoxWriteXtraAtomCommentPicture";
            this.checkBoxWriteXtraAtomCommentPicture.Size = new System.Drawing.Size(61, 20);
            this.checkBoxWriteXtraAtomCommentPicture.TabIndex = 13;
            this.checkBoxWriteXtraAtomCommentPicture.Values.Text = "Picture";
            // 
            // checkBoxWriteXtraAtomSubjectPicture
            // 
            this.checkBoxWriteXtraAtomSubjectPicture.Location = new System.Drawing.Point(246, 176);
            this.checkBoxWriteXtraAtomSubjectPicture.Name = "checkBoxWriteXtraAtomSubjectPicture";
            this.checkBoxWriteXtraAtomSubjectPicture.Size = new System.Drawing.Size(61, 20);
            this.checkBoxWriteXtraAtomSubjectPicture.TabIndex = 10;
            this.checkBoxWriteXtraAtomSubjectPicture.Values.Text = "Picture";
            // 
            // checkBoxWriteXtraAtomCommentVideo
            // 
            this.checkBoxWriteXtraAtomCommentVideo.Location = new System.Drawing.Point(142, 207);
            this.checkBoxWriteXtraAtomCommentVideo.Name = "checkBoxWriteXtraAtomCommentVideo";
            this.checkBoxWriteXtraAtomCommentVideo.Size = new System.Drawing.Size(55, 20);
            this.checkBoxWriteXtraAtomCommentVideo.TabIndex = 12;
            this.checkBoxWriteXtraAtomCommentVideo.Values.Text = "Video";
            // 
            // checkBoxWriteXtraAtomSubjectVideo
            // 
            this.checkBoxWriteXtraAtomSubjectVideo.Location = new System.Drawing.Point(142, 176);
            this.checkBoxWriteXtraAtomSubjectVideo.Name = "checkBoxWriteXtraAtomSubjectVideo";
            this.checkBoxWriteXtraAtomSubjectVideo.Size = new System.Drawing.Size(55, 20);
            this.checkBoxWriteXtraAtomSubjectVideo.TabIndex = 9;
            this.checkBoxWriteXtraAtomSubjectVideo.Values.Text = "Video";
            // 
            // checkBoxWriteXtraAtomSubtitleVideo
            // 
            this.checkBoxWriteXtraAtomSubtitleVideo.Location = new System.Drawing.Point(142, 141);
            this.checkBoxWriteXtraAtomSubtitleVideo.Name = "checkBoxWriteXtraAtomSubtitleVideo";
            this.checkBoxWriteXtraAtomSubtitleVideo.Size = new System.Drawing.Size(55, 20);
            this.checkBoxWriteXtraAtomSubtitleVideo.TabIndex = 7;
            this.checkBoxWriteXtraAtomSubtitleVideo.Values.Text = "Video";
            // 
            // checkBoxWriteXtraAtomAlbumVideo
            // 
            this.checkBoxWriteXtraAtomAlbumVideo.Location = new System.Drawing.Point(142, 108);
            this.checkBoxWriteXtraAtomAlbumVideo.Name = "checkBoxWriteXtraAtomAlbumVideo";
            this.checkBoxWriteXtraAtomAlbumVideo.Size = new System.Drawing.Size(55, 20);
            this.checkBoxWriteXtraAtomAlbumVideo.TabIndex = 5;
            this.checkBoxWriteXtraAtomAlbumVideo.Values.Text = "Video";
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(12, 208);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(97, 20);
            this.label33.TabIndex = 7;
            this.label33.Values.Text = "Write Comment";
            // 
            // label32
            // 
            this.label32.Location = new System.Drawing.Point(12, 177);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(84, 20);
            this.label32.TabIndex = 6;
            this.label32.Values.Text = "Write Subject";
            // 
            // label31
            // 
            this.label31.Location = new System.Drawing.Point(12, 142);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(85, 20);
            this.label31.TabIndex = 5;
            this.label31.Values.Text = "Write Subtitle";
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(12, 109);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(79, 20);
            this.label30.TabIndex = 4;
            this.label30.Values.Text = "Write Album";
            // 
            // checkBoxWriteXtraAtomCategoriesVideo
            // 
            this.checkBoxWriteXtraAtomCategoriesVideo.Location = new System.Drawing.Point(142, 75);
            this.checkBoxWriteXtraAtomCategoriesVideo.Name = "checkBoxWriteXtraAtomCategoriesVideo";
            this.checkBoxWriteXtraAtomCategoriesVideo.Size = new System.Drawing.Size(55, 20);
            this.checkBoxWriteXtraAtomCategoriesVideo.TabIndex = 3;
            this.checkBoxWriteXtraAtomCategoriesVideo.Values.Text = "Video";
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(12, 76);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(101, 20);
            this.label29.TabIndex = 2;
            this.label29.Values.Text = "Write Categories";
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(12, 43);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(96, 20);
            this.label28.TabIndex = 1;
            this.label28.Values.Text = "Write Keywords";
            // 
            // checkBoxWriteXtraAtomKeywordsVideo
            // 
            this.checkBoxWriteXtraAtomKeywordsVideo.Location = new System.Drawing.Point(142, 42);
            this.checkBoxWriteXtraAtomKeywordsVideo.Name = "checkBoxWriteXtraAtomKeywordsVideo";
            this.checkBoxWriteXtraAtomKeywordsVideo.Size = new System.Drawing.Size(55, 20);
            this.checkBoxWriteXtraAtomKeywordsVideo.TabIndex = 1;
            this.checkBoxWriteXtraAtomKeywordsVideo.Values.Text = "Video";
            // 
            // fastColoredTextBoxMetadataWriteTags
            // 
            this.fastColoredTextBoxMetadataWriteTags.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxMetadataWriteTags.AutoIndent = false;
            this.fastColoredTextBoxMetadataWriteTags.AutoIndentChars = false;
            this.fastColoredTextBoxMetadataWriteTags.AutoIndentExistingLines = false;
            this.fastColoredTextBoxMetadataWriteTags.AutoScrollMinSize = new System.Drawing.Size(179, 14);
            this.fastColoredTextBoxMetadataWriteTags.BackBrush = null;
            this.fastColoredTextBoxMetadataWriteTags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxMetadataWriteTags.CharHeight = 14;
            this.fastColoredTextBoxMetadataWriteTags.CharWidth = 8;
            this.fastColoredTextBoxMetadataWriteTags.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxMetadataWriteTags.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxMetadataWriteTags.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fastColoredTextBoxMetadataWriteTags.IsReplaceMode = false;
            this.fastColoredTextBoxMetadataWriteTags.Location = new System.Drawing.Point(0, 30);
            this.fastColoredTextBoxMetadataWriteTags.Name = "fastColoredTextBoxMetadataWriteTags";
            this.fastColoredTextBoxMetadataWriteTags.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxMetadataWriteTags.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxMetadataWriteTags.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxMetadataWriteTags.ServiceColors")));
            this.fastColoredTextBoxMetadataWriteTags.Size = new System.Drawing.Size(556, 759);
            this.fastColoredTextBoxMetadataWriteTags.TabIndex = 0;
            this.fastColoredTextBoxMetadataWriteTags.Text = "fastColoredTextBox1";
            this.fastColoredTextBoxMetadataWriteTags.Zoom = 100;
            this.fastColoredTextBoxMetadataWriteTags.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxMetadataWriteTags_TextChanged);
            this.fastColoredTextBoxMetadataWriteTags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fastColoredTextBoxMetadataWriteTags_KeyDown);
            // 
            // comboBoxMetadataWriteStandardTags
            // 
            this.comboBoxMetadataWriteStandardTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMetadataWriteStandardTags.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxMetadataWriteStandardTags.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMetadataWriteStandardTags.DropDownWidth = 352;
            this.comboBoxMetadataWriteStandardTags.FormattingEnabled = true;
            this.comboBoxMetadataWriteStandardTags.IntegralHeight = false;
            this.comboBoxMetadataWriteStandardTags.Location = new System.Drawing.Point(372, 3);
            this.comboBoxMetadataWriteStandardTags.Name = "comboBoxMetadataWriteStandardTags";
            this.comboBoxMetadataWriteStandardTags.Size = new System.Drawing.Size(174, 21);
            this.comboBoxMetadataWriteStandardTags.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxMetadataWriteStandardTags.TabIndex = 1;
            this.comboBoxMetadataWriteStandardTags.SelectionChangeCommitted += new System.EventHandler(this.comboBoxMetadataWriteStandardTags_SelectionChangeCommitted);
            // 
            // labelMetadataForeachKeyword
            // 
            this.labelMetadataForeachKeyword.Location = new System.Drawing.Point(3, 3);
            this.labelMetadataForeachKeyword.Name = "labelMetadataForeachKeyword";
            this.labelMetadataForeachKeyword.Size = new System.Drawing.Size(113, 20);
            this.labelMetadataForeachKeyword.TabIndex = 7;
            this.labelMetadataForeachKeyword.Values.Text = "For every keyword:";
            // 
            // textBoxMetadataWriteHelpText
            // 
            this.textBoxMetadataWriteHelpText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMetadataWriteHelpText.Location = new System.Drawing.Point(0, 0);
            this.textBoxMetadataWriteHelpText.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMetadataWriteHelpText.Multiline = true;
            this.textBoxMetadataWriteHelpText.Name = "textBoxMetadataWriteHelpText";
            this.textBoxMetadataWriteHelpText.ReadOnly = true;
            this.textBoxMetadataWriteHelpText.Size = new System.Drawing.Size(556, 789);
            this.textBoxMetadataWriteHelpText.TabIndex = 3;
            this.textBoxMetadataWriteHelpText.Text = resources.GetString("textBoxMetadataWriteHelpText.Text");
            // 
            // buttonConfigSave
            // 
            this.buttonConfigSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigSave.Location = new System.Drawing.Point(644, 828);
            this.buttonConfigSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConfigSave.Name = "buttonConfigSave";
            this.buttonConfigSave.Size = new System.Drawing.Size(117, 31);
            this.buttonConfigSave.TabIndex = 1;
            this.buttonConfigSave.Values.Text = "Save";
            this.buttonConfigSave.Click += new System.EventHandler(this.buttonConfigSave_Click);
            // 
            // buttonConfigCancel
            // 
            this.buttonConfigCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigCancel.Location = new System.Drawing.Point(514, 828);
            this.buttonConfigCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConfigCancel.Name = "buttonConfigCancel";
            this.buttonConfigCancel.Size = new System.Drawing.Size(117, 31);
            this.buttonConfigCancel.TabIndex = 2;
            this.buttonConfigCancel.Values.Text = "Cancel";
            this.buttonConfigCancel.Click += new System.EventHandler(this.buttonConfigCancel_Click);
            // 
            // panelAvoidResizeIssues
            // 
            this.panelAvoidResizeIssues.Controls.Add(this.tabControlConfig);
            this.panelAvoidResizeIssues.Location = new System.Drawing.Point(777, 0);
            this.panelAvoidResizeIssues.Name = "panelAvoidResizeIssues";
            this.panelAvoidResizeIssues.Size = new System.Drawing.Size(766, 833);
            this.panelAvoidResizeIssues.TabIndex = 3;
            // 
            // tabControlConfig
            // 
            this.tabControlConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlConfig.Controls.Add(this.tabPageApplication);
            this.tabControlConfig.Controls.Add(this.tabPageConvertAndMerge);
            this.tabControlConfig.Location = new System.Drawing.Point(2, 2);
            this.tabControlConfig.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlConfig.MinimumSize = new System.Drawing.Size(739, 777);
            this.tabControlConfig.Name = "tabControlConfig";
            this.tabControlConfig.SelectedIndex = 0;
            this.tabControlConfig.Size = new System.Drawing.Size(762, 777);
            this.tabControlConfig.TabIndex = 0;
            // 
            // tabPageApplication
            // 
            this.tabPageApplication.Controls.Add(this.panelApplication);
            this.tabPageApplication.Location = new System.Drawing.Point(4, 22);
            this.tabPageApplication.Name = "tabPageApplication";
            this.tabPageApplication.Size = new System.Drawing.Size(754, 751);
            this.tabPageApplication.TabIndex = 5;
            this.tabPageApplication.Text = "Application";
            this.tabPageApplication.UseVisualStyleBackColor = true;
            // 
            // panelApplication
            // 
            this.panelApplication.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelApplication.AutoScroll = true;
            this.panelApplication.BackColor = System.Drawing.Color.Transparent;
            this.panelApplication.Location = new System.Drawing.Point(17, 30);
            this.panelApplication.Name = "panelApplication";
            this.panelApplication.Size = new System.Drawing.Size(610, 615);
            this.panelApplication.TabIndex = 0;
            // 
            // tabPageConvertAndMerge
            // 
            this.tabPageConvertAndMerge.Controls.Add(this.panelConvertAndMerge);
            this.tabPageConvertAndMerge.Location = new System.Drawing.Point(4, 22);
            this.tabPageConvertAndMerge.Name = "tabPageConvertAndMerge";
            this.tabPageConvertAndMerge.Size = new System.Drawing.Size(754, 751);
            this.tabPageConvertAndMerge.TabIndex = 8;
            this.tabPageConvertAndMerge.Text = "Convert and Merge";
            this.tabPageConvertAndMerge.UseVisualStyleBackColor = true;
            // 
            // panelConvertAndMerge
            // 
            this.panelConvertAndMerge.Controls.Add(this.groupBoxConvertAndMergeConvertVideo);
            this.panelConvertAndMerge.Controls.Add(this.groupBoxConvertAndMergeMergeVideos);
            this.panelConvertAndMerge.Controls.Add(this.groupBoxConvertAndMergeCommand);
            this.panelConvertAndMerge.Controls.Add(this.groupBoxConvertAndMergeArgumentFileVariables);
            this.panelConvertAndMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConvertAndMerge.Location = new System.Drawing.Point(0, 0);
            this.panelConvertAndMerge.Name = "panelConvertAndMerge";
            this.panelConvertAndMerge.Size = new System.Drawing.Size(754, 751);
            this.panelConvertAndMerge.TabIndex = 0;
            // 
            // groupBoxConvertAndMergeConvertVideo
            // 
            this.groupBoxConvertAndMergeConvertVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConvertAndMergeConvertVideo.Location = new System.Drawing.Point(3, 417);
            this.groupBoxConvertAndMergeConvertVideo.Name = "groupBoxConvertAndMergeConvertVideo";
            // 
            // groupBoxConvertAndMergeConvertVideo.Panel
            // 
            this.groupBoxConvertAndMergeConvertVideo.Panel.Controls.Add(this.label53);
            this.groupBoxConvertAndMergeConvertVideo.Panel.Controls.Add(this.comboBoxConvertAndMergeConvertVideoFilesVariables);
            this.groupBoxConvertAndMergeConvertVideo.Panel.Controls.Add(this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument);
            this.groupBoxConvertAndMergeConvertVideo.Size = new System.Drawing.Size(742, 113);
            this.groupBoxConvertAndMergeConvertVideo.TabIndex = 3;
            this.groupBoxConvertAndMergeConvertVideo.Values.Heading = "Convert video files";
            // 
            // label53
            // 
            this.label53.Location = new System.Drawing.Point(3, 7);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(176, 20);
            this.label53.TabIndex = 31;
            this.label53.Values.Text = "Convert video files arguments:";
            // 
            // comboBoxConvertAndMergeConvertVideoFilesVariables
            // 
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.DropDownWidth = 352;
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.FormattingEnabled = true;
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.IntegralHeight = false;
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.Location = new System.Drawing.Point(1034, 21);
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.Name = "comboBoxConvertAndMergeConvertVideoFilesVariables";
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.Size = new System.Drawing.Size(352, 21);
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.TabIndex = 0;
            this.comboBoxConvertAndMergeConvertVideoFilesVariables.SelectionChangeCommitted += new System.EventHandler(this.comboBoxConvertAndMergeConvertVideoFilesVariables_SelectionChangeCommitted);
            // 
            // fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument
            // 
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.AutoScrollMinSize = new System.Drawing.Size(0, 14);
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.BackBrush = null;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.CharHeight = 14;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.CharWidth = 8;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.IsReplaceMode = false;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Location = new System.Drawing.Point(6, 34);
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Multiline = false;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Name = "fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument";
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.ServiceColors")));
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.ShowCaretWhenInactive = true;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.ShowScrollBars = false;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Size = new System.Drawing.Size(729, 48);
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.TabIndex = 1;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Text = "file \'{ImageFullFilename}\'";
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.WordWrap = true;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Zoom = 100;
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument_TextChanged);
            this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument_KeyDown);
            // 
            // groupBoxConvertAndMergeMergeVideos
            // 
            this.groupBoxConvertAndMergeMergeVideos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConvertAndMergeMergeVideos.Location = new System.Drawing.Point(3, 536);
            this.groupBoxConvertAndMergeMergeVideos.Name = "groupBoxConvertAndMergeMergeVideos";
            // 
            // groupBoxConvertAndMergeMergeVideos.Panel
            // 
            this.groupBoxConvertAndMergeMergeVideos.Panel.Controls.Add(this.comboBoxConvertAndMergeConcatVideosArguFileVariables);
            this.groupBoxConvertAndMergeMergeVideos.Panel.Controls.Add(this.comboBoxConvertAndMergeConcatVideoFilesVariables);
            this.groupBoxConvertAndMergeMergeVideos.Panel.Controls.Add(this.label52);
            this.groupBoxConvertAndMergeMergeVideos.Panel.Controls.Add(this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile);
            this.groupBoxConvertAndMergeMergeVideos.Panel.Controls.Add(this.fastColoredTextBoxConvertAndMergeConcatVideoArgument);
            this.groupBoxConvertAndMergeMergeVideos.Panel.Controls.Add(this.label50);
            this.groupBoxConvertAndMergeMergeVideos.Size = new System.Drawing.Size(742, 226);
            this.groupBoxConvertAndMergeMergeVideos.TabIndex = 2;
            this.groupBoxConvertAndMergeMergeVideos.Values.Heading = "Merge video files:";
            // 
            // comboBoxConvertAndMergeConcatVideosArguFileVariables
            // 
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.DropDownWidth = 352;
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.FormattingEnabled = true;
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.IntegralHeight = false;
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.Location = new System.Drawing.Point(1034, 116);
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.Name = "comboBoxConvertAndMergeConcatVideosArguFileVariables";
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.Size = new System.Drawing.Size(352, 21);
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.TabIndex = 2;
            this.comboBoxConvertAndMergeConcatVideosArguFileVariables.SelectionChangeCommitted += new System.EventHandler(this.comboBoxConvertAndMergeConcatVideosArguFileVariables_SelectionChangeCommitted);
            // 
            // comboBoxConvertAndMergeConcatVideoFilesVariables
            // 
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.DropDownWidth = 352;
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.FormattingEnabled = true;
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.IntegralHeight = false;
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.Location = new System.Drawing.Point(1034, 16);
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.Name = "comboBoxConvertAndMergeConcatVideoFilesVariables";
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.Size = new System.Drawing.Size(352, 21);
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.TabIndex = 0;
            this.comboBoxConvertAndMergeConcatVideoFilesVariables.SelectionChangeCommitted += new System.EventHandler(this.comboBoxConvertAndMergeConcatVideoFilesVariables_SelectionChangeCommitted);
            // 
            // label52
            // 
            this.label52.Location = new System.Drawing.Point(1, 91);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(178, 20);
            this.label52.TabIndex = 3;
            this.label52.Values.Text = "For each video file in Argu File:";
            // 
            // fastColoredTextBoxConvertAndMergeConcatVideoArguFile
            // 
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.AutoScrollMinSize = new System.Drawing.Size(0, 14);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.BackBrush = null;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.CharHeight = 14;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.CharWidth = 8;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.IsReplaceMode = false;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Location = new System.Drawing.Point(1, 115);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Name = "fastColoredTextBoxConvertAndMergeConcatVideoArguFile";
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxConvertAndMergeConcatVideoArguFile.ServiceColors")));
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.ShowCaretWhenInactive = true;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Size = new System.Drawing.Size(732, 59);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.TabIndex = 3;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Text = "$ cat mylist.txt";
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.WordWrap = true;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Zoom = 100;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile_TextChanged);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile_KeyDown);
            // 
            // fastColoredTextBoxConvertAndMergeConcatVideoArgument
            // 
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.AutoScrollMinSize = new System.Drawing.Size(0, 28);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.BackBrush = null;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.CharHeight = 14;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.CharWidth = 8;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.IsReplaceMode = false;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.Location = new System.Drawing.Point(6, 30);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.Multiline = false;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.Name = "fastColoredTextBoxConvertAndMergeConcatVideoArgument";
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxConvertAndMergeConcatVideoArgument.ServiceColors")));
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.ShowCaretWhenInactive = true;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.ShowScrollBars = false;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.Size = new System.Drawing.Size(729, 59);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.TabIndex = 1;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.Text = "ffmpeg -i opening.mkv -i episode.mkv -i ending.mkv -filter_complex \"[0:v] [0:a] [" +
    "1:v] [1:a] [2:v] [2:a] concat=n=3:v=1:a=1 [v] [a]\" -map \"[v]\" -map \"[a]\" output." +
    "mkv";
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.WordWrap = true;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.Zoom = 100;
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxConvertAndMergeConcatVideoArgument_TextChanged);
            this.fastColoredTextBoxConvertAndMergeConcatVideoArgument.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fastColoredTextBoxConvertAndMergeConcatVideoArgument_KeyDown);
            // 
            // label50
            // 
            this.label50.Location = new System.Drawing.Point(5, 3);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(107, 20);
            this.label50.TabIndex = 0;
            this.label50.Values.Text = "Concat video files ";
            // 
            // groupBoxConvertAndMergeCommand
            // 
            this.groupBoxConvertAndMergeCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConvertAndMergeCommand.Location = new System.Drawing.Point(3, 3);
            this.groupBoxConvertAndMergeCommand.Name = "groupBoxConvertAndMergeCommand";
            // 
            // groupBoxConvertAndMergeCommand.Panel
            // 
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.label56);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.comboBoxConvertAndMergeTempfileExtension);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.label55);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.label54);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.comboBoxConvertAndMergeOutputSize);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.buttonConvertAndMergeBrowseBackgroundMusic);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.buttonConvertAndMergeBrowseFFmpeg);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.label49);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.numericUpDownConvertAndMergeImageDuration);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.label48);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.textBoxConvertAndMergeBackgroundMusic);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.label47);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.textBoxConvertAndMergeFFmpeg);
            this.groupBoxConvertAndMergeCommand.Panel.Controls.Add(this.label46);
            this.groupBoxConvertAndMergeCommand.Size = new System.Drawing.Size(742, 176);
            this.groupBoxConvertAndMergeCommand.TabIndex = 1;
            this.groupBoxConvertAndMergeCommand.Values.Heading = "Convert and Merge Command variables:";
            // 
            // label56
            // 
            this.label56.Location = new System.Drawing.Point(226, 117);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(311, 20);
            this.label56.TabIndex = 8;
            this.label56.Values.Text = "mp4 continer doesn\'t support concatenate files directly";
            // 
            // comboBoxConvertAndMergeTempfileExtension
            // 
            this.comboBoxConvertAndMergeTempfileExtension.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxConvertAndMergeTempfileExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConvertAndMergeTempfileExtension.DropDownWidth = 77;
            this.comboBoxConvertAndMergeTempfileExtension.FormattingEnabled = true;
            this.comboBoxConvertAndMergeTempfileExtension.IntegralHeight = false;
            this.comboBoxConvertAndMergeTempfileExtension.Location = new System.Drawing.Point(143, 114);
            this.comboBoxConvertAndMergeTempfileExtension.Name = "comboBoxConvertAndMergeTempfileExtension";
            this.comboBoxConvertAndMergeTempfileExtension.Size = new System.Drawing.Size(77, 21);
            this.comboBoxConvertAndMergeTempfileExtension.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxConvertAndMergeTempfileExtension.TabIndex = 7;
            // 
            // label55
            // 
            this.label55.Location = new System.Drawing.Point(3, 117);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(119, 20);
            this.label55.TabIndex = 11;
            this.label55.Values.Text = "Temp file extension:";
            // 
            // label54
            // 
            this.label54.Location = new System.Drawing.Point(3, 87);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(110, 20);
            this.label54.TabIndex = 10;
            this.label54.Values.Text = "Output resolution:";
            // 
            // comboBoxConvertAndMergeOutputSize
            // 
            this.comboBoxConvertAndMergeOutputSize.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxConvertAndMergeOutputSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConvertAndMergeOutputSize.DropDownWidth = 157;
            this.comboBoxConvertAndMergeOutputSize.FormattingEnabled = true;
            this.comboBoxConvertAndMergeOutputSize.IntegralHeight = false;
            this.comboBoxConvertAndMergeOutputSize.Items.AddRange(new object[] {
            "2160p: 3840 x 2160",
            "1440p: 2560 x 1440",
            "1080p: 1920 x 1080",
            "720p: 1280 x 720",
            "480p: 854 x 480",
            "360p: 640 x 360",
            "240p: 426 x 240"});
            this.comboBoxConvertAndMergeOutputSize.Location = new System.Drawing.Point(143, 84);
            this.comboBoxConvertAndMergeOutputSize.Name = "comboBoxConvertAndMergeOutputSize";
            this.comboBoxConvertAndMergeOutputSize.Size = new System.Drawing.Size(157, 21);
            this.comboBoxConvertAndMergeOutputSize.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxConvertAndMergeOutputSize.TabIndex = 6;
            // 
            // buttonConvertAndMergeBrowseBackgroundMusic
            // 
            this.buttonConvertAndMergeBrowseBackgroundMusic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvertAndMergeBrowseBackgroundMusic.Location = new System.Drawing.Point(1285, 49);
            this.buttonConvertAndMergeBrowseBackgroundMusic.Name = "buttonConvertAndMergeBrowseBackgroundMusic";
            this.buttonConvertAndMergeBrowseBackgroundMusic.Size = new System.Drawing.Size(101, 25);
            this.buttonConvertAndMergeBrowseBackgroundMusic.TabIndex = 3;
            this.buttonConvertAndMergeBrowseBackgroundMusic.Values.Text = "Browse...";
            this.buttonConvertAndMergeBrowseBackgroundMusic.Click += new System.EventHandler(this.buttonConvertAndMergeBrowseBackgroundMusic_Click);
            // 
            // buttonConvertAndMergeBrowseFFmpeg
            // 
            this.buttonConvertAndMergeBrowseFFmpeg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvertAndMergeBrowseFFmpeg.Location = new System.Drawing.Point(1285, 21);
            this.buttonConvertAndMergeBrowseFFmpeg.Name = "buttonConvertAndMergeBrowseFFmpeg";
            this.buttonConvertAndMergeBrowseFFmpeg.Size = new System.Drawing.Size(101, 25);
            this.buttonConvertAndMergeBrowseFFmpeg.TabIndex = 1;
            this.buttonConvertAndMergeBrowseFFmpeg.Values.Text = "Browse...";
            this.buttonConvertAndMergeBrowseFFmpeg.Click += new System.EventHandler(this.buttonConvertAndMergeBrowseFFmpeg_Click);
            // 
            // label49
            // 
            this.label49.Location = new System.Drawing.Point(226, 58);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(394, 20);
            this.label49.TabIndex = 5;
            this.label49.Values.Text = "in seconds for each image, when merge image files to video slideshow";
            // 
            // numericUpDownConvertAndMergeImageDuration
            // 
            this.numericUpDownConvertAndMergeImageDuration.Location = new System.Drawing.Point(143, 56);
            this.numericUpDownConvertAndMergeImageDuration.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownConvertAndMergeImageDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownConvertAndMergeImageDuration.Name = "numericUpDownConvertAndMergeImageDuration";
            this.numericUpDownConvertAndMergeImageDuration.Size = new System.Drawing.Size(77, 20);
            this.numericUpDownConvertAndMergeImageDuration.TabIndex = 4;
            this.numericUpDownConvertAndMergeImageDuration.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label48
            // 
            this.label48.Location = new System.Drawing.Point(3, 58);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(97, 20);
            this.label48.TabIndex = 4;
            this.label48.Values.Text = "Image duration:";
            // 
            // textBoxConvertAndMergeBackgroundMusic
            // 
            this.textBoxConvertAndMergeBackgroundMusic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConvertAndMergeBackgroundMusic.Location = new System.Drawing.Point(143, 29);
            this.textBoxConvertAndMergeBackgroundMusic.Name = "textBoxConvertAndMergeBackgroundMusic";
            this.textBoxConvertAndMergeBackgroundMusic.Size = new System.Drawing.Size(592, 23);
            this.textBoxConvertAndMergeBackgroundMusic.TabIndex = 2;
            this.textBoxConvertAndMergeBackgroundMusic.Text = "silent.wav";
            // 
            // label47
            // 
            this.label47.Location = new System.Drawing.Point(3, 32);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(113, 20);
            this.label47.TabIndex = 2;
            this.label47.Values.Text = "Background music:";
            // 
            // textBoxConvertAndMergeFFmpeg
            // 
            this.textBoxConvertAndMergeFFmpeg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxConvertAndMergeFFmpeg.Location = new System.Drawing.Point(143, 3);
            this.textBoxConvertAndMergeFFmpeg.Name = "textBoxConvertAndMergeFFmpeg";
            this.textBoxConvertAndMergeFFmpeg.Size = new System.Drawing.Size(592, 23);
            this.textBoxConvertAndMergeFFmpeg.TabIndex = 0;
            this.textBoxConvertAndMergeFFmpeg.Text = "ffmpeg.exe";
            // 
            // label46
            // 
            this.label46.Location = new System.Drawing.Point(3, 6);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(116, 20);
            this.label46.TabIndex = 0;
            this.label46.Values.Text = "Path to ffmpeg.exe:";
            // 
            // groupBoxConvertAndMergeArgumentFileVariables
            // 
            this.groupBoxConvertAndMergeArgumentFileVariables.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConvertAndMergeArgumentFileVariables.Location = new System.Drawing.Point(3, 185);
            this.groupBoxConvertAndMergeArgumentFileVariables.Name = "groupBoxConvertAndMergeArgumentFileVariables";
            // 
            // groupBoxConvertAndMergeArgumentFileVariables.Panel
            // 
            this.groupBoxConvertAndMergeArgumentFileVariables.Panel.Controls.Add(this.label51);
            this.groupBoxConvertAndMergeArgumentFileVariables.Panel.Controls.Add(this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables);
            this.groupBoxConvertAndMergeArgumentFileVariables.Panel.Controls.Add(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument);
            this.groupBoxConvertAndMergeArgumentFileVariables.Panel.Controls.Add(this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables);
            this.groupBoxConvertAndMergeArgumentFileVariables.Panel.Controls.Add(this.label45);
            this.groupBoxConvertAndMergeArgumentFileVariables.Panel.Controls.Add(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile);
            this.groupBoxConvertAndMergeArgumentFileVariables.Size = new System.Drawing.Size(742, 226);
            this.groupBoxConvertAndMergeArgumentFileVariables.TabIndex = 0;
            this.groupBoxConvertAndMergeArgumentFileVariables.Values.Heading = "Convert and Merge Images to Video format:";
            // 
            // label51
            // 
            this.label51.Location = new System.Drawing.Point(0, 123);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(179, 20);
            this.label51.TabIndex = 28;
            this.label51.Values.Text = "For each image file in Argu file:";
            // 
            // comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables
            // 
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.DropDownWidth = 352;
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.FormattingEnabled = true;
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.IntegralHeight = false;
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.Location = new System.Drawing.Point(1034, 21);
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.Name = "comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables";
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.Size = new System.Drawing.Size(352, 21);
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.TabIndex = 1;
            this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.SelectionChangeCommitted += new System.EventHandler(this.comboBoxConvertAndMergeConcatImageAsVideoCommandVariables_SelectionChangeCommitted);
            // 
            // fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument
            // 
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.AutoScrollMinSize = new System.Drawing.Size(0, 42);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.BackBrush = null;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.CharHeight = 14;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.CharWidth = 8;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.IsReplaceMode = false;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Location = new System.Drawing.Point(3, 34);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Multiline = false;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Name = "fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument";
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.ServiceColors")));
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.ShowCaretWhenInactive = true;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.ShowScrollBars = false;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Size = new System.Drawing.Size(732, 80);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.TabIndex = 2;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Text = resources.GetString("fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Text");
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.WordWrap = true;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Zoom = 100;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoCommand_TextChanged);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoCommand_KeyDown);
            // 
            // comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables
            // 
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.DropDownWidth = 352;
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.FormattingEnabled = true;
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.IntegralHeight = false;
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.Location = new System.Drawing.Point(1034, 137);
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.Name = "comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables";
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.Size = new System.Drawing.Size(352, 21);
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.TabIndex = 3;
            this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.SelectionChangeCommitted += new System.EventHandler(this.comboBoxConvertAndMergeConcatImageAsVideoCommandArgumentVariables_SelectionChangeCommitted);
            // 
            // label45
            // 
            this.label45.Location = new System.Drawing.Point(0, 7);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(173, 20);
            this.label45.TabIndex = 0;
            this.label45.Values.Text = "Merge image files agruments:";
            // 
            // fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile
            // 
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.AutoScrollMinSize = new System.Drawing.Size(0, 28);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.BackBrush = null;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.CharHeight = 14;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.CharWidth = 8;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.IsReplaceMode = false;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Location = new System.Drawing.Point(3, 150);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Name = "fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile";
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.ServiceColors")));
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.ShowCaretWhenInactive = true;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Size = new System.Drawing.Size(732, 45);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.TabIndex = 4;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Text = "file \'{ImageFullFilename}\'\r\nduration {Duration}";
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.WordWrap = true;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Zoom = 100;
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoCommandArgument_TextChanged);
            this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoCommandArgument_KeyDown);
            // 
            // kryptonWorkspaceConfig
            // 
            this.kryptonWorkspaceConfig.ActivePage = this.kryptonPageAutoCorrect;
            this.kryptonWorkspaceConfig.Location = new System.Drawing.Point(3, 2);
            this.kryptonWorkspaceConfig.Name = "kryptonWorkspaceConfig";
            // 
            // 
            // 
            this.kryptonWorkspaceConfig.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCell1});
            this.kryptonWorkspaceConfig.Root.UniqueName = "74f3e76ef6f44751a76c1ae766bf778a";
            this.kryptonWorkspaceConfig.Root.WorkspaceControl = this.kryptonWorkspaceConfig;
            this.kryptonWorkspaceConfig.Size = new System.Drawing.Size(759, 818);
            this.kryptonWorkspaceConfig.TabIndex = 4;
            this.kryptonWorkspaceConfig.TabStop = true;
            // 
            // kryptonPageApplication
            // 
            this.kryptonPageApplication.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageApplication.Controls.Add(this.kryptonWorkspaceConfigApplication);
            this.kryptonPageApplication.Flags = 65534;
            this.kryptonPageApplication.LastVisibleSet = true;
            this.kryptonPageApplication.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageApplication.Name = "kryptonPageApplication";
            this.kryptonPageApplication.Size = new System.Drawing.Size(757, 791);
            this.kryptonPageApplication.Text = "Application";
            this.kryptonPageApplication.ToolTipTitle = "Page ToolTip";
            this.kryptonPageApplication.UniqueName = "c215c1585f354e12bb77a1549ead7e7c";
            // 
            // kryptonWorkspaceConfigApplication
            // 
            this.kryptonWorkspaceConfigApplication.ActivePage = this.kryptonPageApplicationThumbnail;
            this.kryptonWorkspaceConfigApplication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceConfigApplication.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceConfigApplication.Name = "kryptonWorkspaceConfigApplication";
            // 
            // 
            // 
            this.kryptonWorkspaceConfigApplication.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCell2});
            this.kryptonWorkspaceConfigApplication.Root.UniqueName = "521b7cae8fa74ef7b9e0bd987fac84ec";
            this.kryptonWorkspaceConfigApplication.Root.WorkspaceControl = this.kryptonWorkspaceConfigApplication;
            this.kryptonWorkspaceConfigApplication.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceConfigApplication.Size = new System.Drawing.Size(757, 791);
            this.kryptonWorkspaceConfigApplication.TabIndex = 0;
            this.kryptonWorkspaceConfigApplication.TabStop = true;
            // 
            // kryptonPageApplicationThumbnail
            // 
            this.kryptonPageApplicationThumbnail.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageApplicationThumbnail.Controls.Add(this.labelApplicationRegionThumbnailSize);
            this.kryptonPageApplicationThumbnail.Controls.Add(this.labelApplicationRegionThumbnailSizeDescription);
            this.kryptonPageApplicationThumbnail.Controls.Add(this.labelApplicationPosterThumbnailSizeDescription);
            this.kryptonPageApplicationThumbnail.Controls.Add(this.labelApplicationPosterThumbnailSize);
            this.kryptonPageApplicationThumbnail.Controls.Add(this.comboBoxApplicationRegionThumbnailSizes);
            this.kryptonPageApplicationThumbnail.Controls.Add(this.comboBoxApplicationThumbnailSizes);
            this.kryptonPageApplicationThumbnail.Controls.Add(this.labelApplicationThumbnailSizeHelp);
            this.kryptonPageApplicationThumbnail.Flags = 65534;
            this.kryptonPageApplicationThumbnail.LastVisibleSet = true;
            this.kryptonPageApplicationThumbnail.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageApplicationThumbnail.Name = "kryptonPageApplicationThumbnail";
            this.kryptonPageApplicationThumbnail.Size = new System.Drawing.Size(576, 789);
            this.kryptonPageApplicationThumbnail.Text = "Thumbnail";
            this.kryptonPageApplicationThumbnail.ToolTipTitle = "Page ToolTip";
            this.kryptonPageApplicationThumbnail.UniqueName = "533a958d219a47959b59a1e3163ef8df";
            // 
            // kryptonWorkspaceCell2
            // 
            this.kryptonWorkspaceCell2.AllowPageDrag = true;
            this.kryptonWorkspaceCell2.AllowTabFocus = false;
            this.kryptonWorkspaceCell2.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Left;
            this.kryptonWorkspaceCell2.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.FixedTop;
            this.kryptonWorkspaceCell2.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCell2.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCell2.Name = "kryptonWorkspaceCell2";
            this.kryptonWorkspaceCell2.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageApplicationThumbnail,
            this.kryptonPageApplicationNominatim,
            this.kryptonPageApplicationSearch,
            this.kryptonPageApplicationRegionSuggestion,
            this.kryptonPageRegionAccuracy,
            this.kryptonPageApplicationCloudAndVirtualFiles,
            this.kryptonPageApplicationGPSLocationAccuracy,
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp,
            this.kryptonPageApplicationDateAndTimeInFilenames});
            this.kryptonWorkspaceCell2.SelectedIndex = 0;
            this.kryptonWorkspaceCell2.UniqueName = "509b106994a8437cbec6056b8eeed6d9";
            // 
            // kryptonPageApplicationNominatim
            // 
            this.kryptonPageApplicationNominatim.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageApplicationNominatim.Controls.Add(this.labelApplicationNominatimTitle);
            this.kryptonPageApplicationNominatim.Controls.Add(this.labelApplicationNominatimPreferredLanguagesHelp);
            this.kryptonPageApplicationNominatim.Controls.Add(this.comboBoxApplicationLanguages);
            this.kryptonPageApplicationNominatim.Controls.Add(this.textBoxApplicationPreferredLanguages);
            this.kryptonPageApplicationNominatim.Controls.Add(this.labelApplicationPreferredLanguages);
            this.kryptonPageApplicationNominatim.Flags = 65534;
            this.kryptonPageApplicationNominatim.LastVisibleSet = true;
            this.kryptonPageApplicationNominatim.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageApplicationNominatim.Name = "kryptonPageApplicationNominatim";
            this.kryptonPageApplicationNominatim.Size = new System.Drawing.Size(576, 789);
            this.kryptonPageApplicationNominatim.Text = "Nominatim look-up";
            this.kryptonPageApplicationNominatim.ToolTipTitle = "Page ToolTip";
            this.kryptonPageApplicationNominatim.UniqueName = "73c2d6e844994988959c446a2bf05e6b";
            // 
            // kryptonPageApplicationSearch
            // 
            this.kryptonPageApplicationSearch.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageApplicationSearch.Controls.Add(this.numericUpDownApplicationMaxRowsInSearchResult);
            this.kryptonPageApplicationSearch.Controls.Add(this.labelApplicationSearch);
            this.kryptonPageApplicationSearch.Flags = 65534;
            this.kryptonPageApplicationSearch.LastVisibleSet = true;
            this.kryptonPageApplicationSearch.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageApplicationSearch.Name = "kryptonPageApplicationSearch";
            this.kryptonPageApplicationSearch.Size = new System.Drawing.Size(576, 789);
            this.kryptonPageApplicationSearch.Text = "Search";
            this.kryptonPageApplicationSearch.ToolTipTitle = "Page ToolTip";
            this.kryptonPageApplicationSearch.UniqueName = "27a2ac3290f04f38a1a52296e9f1903b";
            // 
            // kryptonPageApplicationRegionSuggestion
            // 
            this.kryptonPageApplicationRegionSuggestion.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageApplicationRegionSuggestion.Controls.Add(this.labelApplicationNumberOfMostCommonDescription);
            this.kryptonPageApplicationRegionSuggestion.Controls.Add(this.labelApplicationNumberOfDays);
            this.kryptonPageApplicationRegionSuggestion.Controls.Add(this.labelApplicationNumberOfDaysDescription);
            this.kryptonPageApplicationRegionSuggestion.Controls.Add(this.numericUpDownPeopleSuggestNameDaysInterval);
            this.kryptonPageApplicationRegionSuggestion.Controls.Add(this.labelApplicationNumberOfMostCommon);
            this.kryptonPageApplicationRegionSuggestion.Controls.Add(this.numericUpDownPeopleSuggestNameTopMost);
            this.kryptonPageApplicationRegionSuggestion.Flags = 65534;
            this.kryptonPageApplicationRegionSuggestion.LastVisibleSet = true;
            this.kryptonPageApplicationRegionSuggestion.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageApplicationRegionSuggestion.Name = "kryptonPageApplicationRegionSuggestion";
            this.kryptonPageApplicationRegionSuggestion.Size = new System.Drawing.Size(576, 789);
            this.kryptonPageApplicationRegionSuggestion.Text = "Region suggestion ";
            this.kryptonPageApplicationRegionSuggestion.ToolTipTitle = "Page ToolTip";
            this.kryptonPageApplicationRegionSuggestion.UniqueName = "91d103da57554355ad508c89fe569a44";
            // 
            // kryptonPageRegionAccuracy
            // 
            this.kryptonPageRegionAccuracy.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageRegionAccuracy.Controls.Add(this.labelApplicationRegionAccuracyDescription);
            this.kryptonPageRegionAccuracy.Controls.Add(this.labelApplicationRegionAccuracyHelp);
            this.kryptonPageRegionAccuracy.Controls.Add(this.numericUpDownRegionMissmatchProcent);
            this.kryptonPageRegionAccuracy.Controls.Add(this.labelApplicationRegionAccuracy);
            this.kryptonPageRegionAccuracy.Flags = 65534;
            this.kryptonPageRegionAccuracy.LastVisibleSet = true;
            this.kryptonPageRegionAccuracy.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageRegionAccuracy.Name = "kryptonPageRegionAccuracy";
            this.kryptonPageRegionAccuracy.Size = new System.Drawing.Size(576, 789);
            this.kryptonPageRegionAccuracy.Text = "Region accuracy";
            this.kryptonPageRegionAccuracy.ToolTipTitle = "Page ToolTip";
            this.kryptonPageRegionAccuracy.UniqueName = "eb580b8b963e42cc8c659a3987162706";
            // 
            // kryptonPageApplicationCloudAndVirtualFiles
            // 
            this.kryptonPageApplicationCloudAndVirtualFiles.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageApplicationCloudAndVirtualFiles.Controls.Add(this.checkBoxApplicationAvoidReadExifFromCloud);
            this.kryptonPageApplicationCloudAndVirtualFiles.Controls.Add(this.checkBoxApplicationAvoidReadMediaFromCloud);
            this.kryptonPageApplicationCloudAndVirtualFiles.Controls.Add(this.checkBoxApplicationImageListViewCacheModeOnDemand);
            this.kryptonPageApplicationCloudAndVirtualFiles.Flags = 65534;
            this.kryptonPageApplicationCloudAndVirtualFiles.LastVisibleSet = true;
            this.kryptonPageApplicationCloudAndVirtualFiles.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageApplicationCloudAndVirtualFiles.Name = "kryptonPageApplicationCloudAndVirtualFiles";
            this.kryptonPageApplicationCloudAndVirtualFiles.Size = new System.Drawing.Size(576, 789);
            this.kryptonPageApplicationCloudAndVirtualFiles.Text = "Cloud and Virual files";
            this.kryptonPageApplicationCloudAndVirtualFiles.ToolTipTitle = "Page ToolTip";
            this.kryptonPageApplicationCloudAndVirtualFiles.UniqueName = "0267042819a846f3ab4e9b312d1e09bc";
            // 
            // kryptonPageApplicationGPSLocationAccuracy
            // 
            this.kryptonPageApplicationGPSLocationAccuracy.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageApplicationGPSLocationAccuracy.Controls.Add(this.labelApplicationGPSLocationAccuracy);
            this.kryptonPageApplicationGPSLocationAccuracy.Controls.Add(this.numericUpDownLocationAccuracyLongitude);
            this.kryptonPageApplicationGPSLocationAccuracy.Controls.Add(this.label65);
            this.kryptonPageApplicationGPSLocationAccuracy.Controls.Add(this.label64);
            this.kryptonPageApplicationGPSLocationAccuracy.Controls.Add(this.numericUpDownLocationAccuracyLatitude);
            this.kryptonPageApplicationGPSLocationAccuracy.Flags = 65534;
            this.kryptonPageApplicationGPSLocationAccuracy.LastVisibleSet = true;
            this.kryptonPageApplicationGPSLocationAccuracy.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageApplicationGPSLocationAccuracy.Name = "kryptonPageApplicationGPSLocationAccuracy";
            this.kryptonPageApplicationGPSLocationAccuracy.Size = new System.Drawing.Size(576, 789);
            this.kryptonPageApplicationGPSLocationAccuracy.Text = "GPS Location Accuracy";
            this.kryptonPageApplicationGPSLocationAccuracy.ToolTipTitle = "Page ToolTip";
            this.kryptonPageApplicationGPSLocationAccuracy.UniqueName = "ee40c1ac2a5048c386af9eac94b1a48d";
            // 
            // kryptonPageAppliactionDateTimeFormatsInFilenamesHelp
            // 
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.Controls.Add(this.textBoxApplicationDateTimeFormatHelp);
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.Flags = 65534;
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.LastVisibleSet = true;
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.Name = "kryptonPageAppliactionDateTimeFormatsInFilenamesHelp";
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.Size = new System.Drawing.Size(576, 789);
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.Text = "DateTime formats help";
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.UniqueName = "6e2d844a426a4ac6beb646b2829aaf0e";
            // 
            // kryptonPageApplicationDateAndTimeInFilenames
            // 
            this.kryptonPageApplicationDateAndTimeInFilenames.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageApplicationDateAndTimeInFilenames.Controls.Add(this.fastColoredTextBoxConfigFilenameDateFormats);
            this.kryptonPageApplicationDateAndTimeInFilenames.Flags = 65534;
            this.kryptonPageApplicationDateAndTimeInFilenames.LastVisibleSet = true;
            this.kryptonPageApplicationDateAndTimeInFilenames.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageApplicationDateAndTimeInFilenames.Name = "kryptonPageApplicationDateAndTimeInFilenames";
            this.kryptonPageApplicationDateAndTimeInFilenames.Size = new System.Drawing.Size(564, 789);
            this.kryptonPageApplicationDateAndTimeInFilenames.Text = "DateTime formats in filenames";
            this.kryptonPageApplicationDateAndTimeInFilenames.ToolTipTitle = "Page ToolTip";
            this.kryptonPageApplicationDateAndTimeInFilenames.UniqueName = "9005463b04cf40ed9c256d2baa999bba";
            // 
            // kryptonWorkspaceCell1
            // 
            this.kryptonWorkspaceCell1.AllowPageDrag = true;
            this.kryptonWorkspaceCell1.AllowTabFocus = false;
            this.kryptonWorkspaceCell1.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCell1.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCell1.Name = "kryptonWorkspaceCell1";
            this.kryptonWorkspaceCell1.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageApplication,
            this.kryptonPageMetadata,
            this.kryptonPageWebScraper,
            this.kryptonPageAutoCorrect,
            this.kryptonPageLocation,
            this.kryptonPageConvertAndMerge,
            this.kryptonPageChromecast,
            this.kryptonPageLog});
            this.kryptonWorkspaceCell1.SelectedIndex = 3;
            this.kryptonWorkspaceCell1.UniqueName = "6c55e441207441569a9cebe15f1d63b7";
            // 
            // kryptonPageMetadata
            // 
            this.kryptonPageMetadata.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadata.Controls.Add(this.kryptonWorkspaceConfigMetadata);
            this.kryptonPageMetadata.Flags = 65534;
            this.kryptonPageMetadata.LastVisibleSet = true;
            this.kryptonPageMetadata.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadata.Name = "kryptonPageMetadata";
            this.kryptonPageMetadata.Size = new System.Drawing.Size(757, 791);
            this.kryptonPageMetadata.Text = "Metadata";
            this.kryptonPageMetadata.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadata.UniqueName = "d5cb9d54763045a6ac874ae78ebe0c03";
            // 
            // kryptonWorkspaceConfigMetadata
            // 
            this.kryptonWorkspaceConfigMetadata.ActivePage = this.kryptonPageMetadataReadHelp;
            this.kryptonWorkspaceConfigMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceConfigMetadata.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceConfigMetadata.Name = "kryptonWorkspaceConfigMetadata";
            // 
            // 
            // 
            this.kryptonWorkspaceConfigMetadata.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCell3});
            this.kryptonWorkspaceConfigMetadata.Root.UniqueName = "e7f6d4a4944e4f5ab606df51ef6d45b8";
            this.kryptonWorkspaceConfigMetadata.Root.WorkspaceControl = this.kryptonWorkspaceConfigMetadata;
            this.kryptonWorkspaceConfigMetadata.Size = new System.Drawing.Size(757, 791);
            this.kryptonWorkspaceConfigMetadata.TabIndex = 0;
            this.kryptonWorkspaceConfigMetadata.TabStop = true;
            // 
            // kryptonPageMetadataReadHelp
            // 
            this.kryptonPageMetadataReadHelp.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadataReadHelp.Controls.Add(this.kryptonTextBoxMetadataReadHelpText);
            this.kryptonPageMetadataReadHelp.Flags = 65534;
            this.kryptonPageMetadataReadHelp.LastVisibleSet = true;
            this.kryptonPageMetadataReadHelp.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadataReadHelp.Name = "kryptonPageMetadataReadHelp";
            this.kryptonPageMetadataReadHelp.Size = new System.Drawing.Size(556, 789);
            this.kryptonPageMetadataReadHelp.Text = "Read help text";
            this.kryptonPageMetadataReadHelp.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadataReadHelp.UniqueName = "eec060a13b4e45ebbc18d3f53eab1529";
            // 
            // kryptonTextBoxMetadataReadHelpText
            // 
            this.kryptonTextBoxMetadataReadHelpText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonTextBoxMetadataReadHelpText.Location = new System.Drawing.Point(0, 0);
            this.kryptonTextBoxMetadataReadHelpText.Multiline = true;
            this.kryptonTextBoxMetadataReadHelpText.Name = "kryptonTextBoxMetadataReadHelpText";
            this.kryptonTextBoxMetadataReadHelpText.Size = new System.Drawing.Size(556, 789);
            this.kryptonTextBoxMetadataReadHelpText.TabIndex = 0;
            this.kryptonTextBoxMetadataReadHelpText.Text = resources.GetString("kryptonTextBoxMetadataReadHelpText.Text");
            // 
            // kryptonWorkspaceCell3
            // 
            this.kryptonWorkspaceCell3.AllowPageDrag = true;
            this.kryptonWorkspaceCell3.AllowTabFocus = false;
            this.kryptonWorkspaceCell3.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Left;
            this.kryptonWorkspaceCell3.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.FixedTop;
            this.kryptonWorkspaceCell3.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCell3.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCell3.Name = "kryptonWorkspaceCell3";
            this.kryptonWorkspaceCell3.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageMetadataReadHelp,
            this.kryptonPageMetadataReadPriority,
            this.kryptonPageMetadataWriteHelp,
            this.kryptonPageMetadataWriteWindowsXtraProperties,
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated,
            this.kryptonPageMetadataExiftoolHelp,
            this.kryptonPageMetadataExiftoolForEachNewKeyword,
            this.kryptonPageMetadataExiftoolForEachKeyword,
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword});
            this.kryptonWorkspaceCell3.SelectedIndex = 0;
            this.kryptonWorkspaceCell3.UniqueName = "dc14c7222a5b4f1988263e23e6ff262d";
            // 
            // kryptonPageMetadataReadPriority
            // 
            this.kryptonPageMetadataReadPriority.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadataReadPriority.Controls.Add(this.dataGridViewMetadataReadPriority);
            this.kryptonPageMetadataReadPriority.Flags = 65534;
            this.kryptonPageMetadataReadPriority.LastVisibleSet = true;
            this.kryptonPageMetadataReadPriority.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadataReadPriority.Name = "kryptonPageMetadataReadPriority";
            this.kryptonPageMetadataReadPriority.Size = new System.Drawing.Size(556, 789);
            this.kryptonPageMetadataReadPriority.Text = "Read priority";
            this.kryptonPageMetadataReadPriority.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadataReadPriority.UniqueName = "4d859c3a564943e8805ced2985a13756";
            // 
            // kryptonPageMetadataWriteHelp
            // 
            this.kryptonPageMetadataWriteHelp.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadataWriteHelp.Controls.Add(this.textBoxMetadataWriteHelpText);
            this.kryptonPageMetadataWriteHelp.Flags = 65534;
            this.kryptonPageMetadataWriteHelp.LastVisibleSet = true;
            this.kryptonPageMetadataWriteHelp.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadataWriteHelp.Name = "kryptonPageMetadataWriteHelp";
            this.kryptonPageMetadataWriteHelp.Size = new System.Drawing.Size(556, 789);
            this.kryptonPageMetadataWriteHelp.Text = "Write help text";
            this.kryptonPageMetadataWriteHelp.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadataWriteHelp.UniqueName = "79b2d292fc224d60b32cbd5059a64081";
            // 
            // kryptonPageMetadataWriteWindowsXtraProperties
            // 
            this.kryptonPageMetadataWriteWindowsXtraProperties.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.textBoxWriteXtraAtomArtist);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.labelMetadataWriteOnVideoAndPictureFiles);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomArtistVideo);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomKeywordsVideo);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.label37);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.label28);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.labelMetadataWriteOnVideoAndPictureFilesVariables);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.label29);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.comboBoxWriteXtraAtomVariables);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomCategoriesVideo);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.textBoxWriteXtraAtomComment);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.label30);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.textBoxWriteXtraAtomSubject);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.label31);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.textBoxWriteXtraAtomSubtitle);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.label32);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.textBoxWriteXtraAtomAlbum);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.label33);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.textBoxWriteXtraAtomCategories);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomAlbumVideo);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.textBoxWriteXtraAtomKeywords);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomSubtitleVideo);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomSubjectVideo);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomRatingPicture);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomCommentVideo);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomRatingVideo);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomSubjectPicture);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.label34);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Controls.Add(this.checkBoxWriteXtraAtomCommentPicture);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Flags = 65534;
            this.kryptonPageMetadataWriteWindowsXtraProperties.LastVisibleSet = true;
            this.kryptonPageMetadataWriteWindowsXtraProperties.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Name = "kryptonPageMetadataWriteWindowsXtraProperties";
            this.kryptonPageMetadataWriteWindowsXtraProperties.Size = new System.Drawing.Size(556, 789);
            this.kryptonPageMetadataWriteWindowsXtraProperties.Text = "Write Winows Xtra properties";
            this.kryptonPageMetadataWriteWindowsXtraProperties.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadataWriteWindowsXtraProperties.UniqueName = "6b72f4451e6b436d99f25058fd1c1672";
            // 
            // kryptonPageMetadataWriteFileAttributeDateTimeCreated
            // 
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.Controls.Add(this.labelMetadataFileCreateDateTimDiffrentDescription);
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.Controls.Add(this.labelMetadataFileCreateDateTimDiffrent);
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.Controls.Add(this.checkBoxWriteFileAttributeCreatedDate);
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.Controls.Add(this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted);
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.Flags = 65534;
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.LastVisibleSet = true;
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.Name = "kryptonPageMetadataWriteFileAttributeDateTimeCreated";
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.Text = "Update files Create Date and Time";
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.UniqueName = "db2d5ffa4b884a8fb83555535360f119";
            // 
            // kryptonPageMetadataExiftoolHelp
            // 
            this.kryptonPageMetadataExiftoolHelp.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadataExiftoolHelp.Flags = 65534;
            this.kryptonPageMetadataExiftoolHelp.LastVisibleSet = true;
            this.kryptonPageMetadataExiftoolHelp.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadataExiftoolHelp.Name = "kryptonPageMetadataExiftoolHelp";
            this.kryptonPageMetadataExiftoolHelp.Size = new System.Drawing.Size(556, 789);
            this.kryptonPageMetadataExiftoolHelp.Text = "Exiftool help text";
            this.kryptonPageMetadataExiftoolHelp.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadataExiftoolHelp.UniqueName = "411a97c64b304462ad916c57ea93841b";
            // 
            // kryptonPageMetadataExiftoolForEachNewKeyword
            // 
            this.kryptonPageMetadataExiftoolForEachNewKeyword.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadataExiftoolForEachNewKeyword.Controls.Add(this.checkBoxWriteMetadataAddAutoKeywords);
            this.kryptonPageMetadataExiftoolForEachNewKeyword.Controls.Add(this.fastColoredTextBoxMetadataWriteKeywordDelete);
            this.kryptonPageMetadataExiftoolForEachNewKeyword.Controls.Add(this.labelMetadataForeachNewKeyword);
            this.kryptonPageMetadataExiftoolForEachNewKeyword.Controls.Add(this.comboBoxMetadataWriteKeywordDelete);
            this.kryptonPageMetadataExiftoolForEachNewKeyword.Flags = 65534;
            this.kryptonPageMetadataExiftoolForEachNewKeyword.LastVisibleSet = true;
            this.kryptonPageMetadataExiftoolForEachNewKeyword.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadataExiftoolForEachNewKeyword.Name = "kryptonPageMetadataExiftoolForEachNewKeyword";
            this.kryptonPageMetadataExiftoolForEachNewKeyword.Size = new System.Drawing.Size(556, 789);
            this.kryptonPageMetadataExiftoolForEachNewKeyword.Text = "For each new keyword";
            this.kryptonPageMetadataExiftoolForEachNewKeyword.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadataExiftoolForEachNewKeyword.UniqueName = "63c73fc6dac348b78f574a6515bac69f";
            // 
            // kryptonPageMetadataExiftoolForEachDeletedKeyword
            // 
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.Controls.Add(this.comboBoxMetadataWriteKeywordAdd);
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.Controls.Add(this.fastColoredTextBoxMetadataWriteKeywordAdd);
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.Controls.Add(this.labelMetadataForeachDeletedKeyword);
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.Flags = 65534;
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.LastVisibleSet = true;
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.Name = "kryptonPageMetadataExiftoolForEachDeletedKeyword";
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.Size = new System.Drawing.Size(556, 789);
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.Text = "For each deleted keyword";
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.UniqueName = "00f51afa468a4bc68202d53cf7819c1e";
            // 
            // kryptonPageMetadataExiftoolForEachKeyword
            // 
            this.kryptonPageMetadataExiftoolForEachKeyword.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMetadataExiftoolForEachKeyword.Controls.Add(this.fastColoredTextBoxMetadataWriteTags);
            this.kryptonPageMetadataExiftoolForEachKeyword.Controls.Add(this.comboBoxMetadataWriteStandardTags);
            this.kryptonPageMetadataExiftoolForEachKeyword.Controls.Add(this.labelMetadataForeachKeyword);
            this.kryptonPageMetadataExiftoolForEachKeyword.Flags = 65534;
            this.kryptonPageMetadataExiftoolForEachKeyword.LastVisibleSet = true;
            this.kryptonPageMetadataExiftoolForEachKeyword.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageMetadataExiftoolForEachKeyword.Name = "kryptonPageMetadataExiftoolForEachKeyword";
            this.kryptonPageMetadataExiftoolForEachKeyword.Size = new System.Drawing.Size(556, 789);
            this.kryptonPageMetadataExiftoolForEachKeyword.Text = "For every keyword";
            this.kryptonPageMetadataExiftoolForEachKeyword.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMetadataExiftoolForEachKeyword.UniqueName = "b02b2f3292bb4233aeabddbf663b823a";
            // 
            // kryptonPageWebScraper
            // 
            this.kryptonPageWebScraper.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageWebScraper.Controls.Add(this.kryptonWorkspaceWebScraper);
            this.kryptonPageWebScraper.Flags = 65534;
            this.kryptonPageWebScraper.LastVisibleSet = true;
            this.kryptonPageWebScraper.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageWebScraper.Name = "kryptonPageWebScraper";
            this.kryptonPageWebScraper.Size = new System.Drawing.Size(757, 791);
            this.kryptonPageWebScraper.Text = "WebScraper";
            this.kryptonPageWebScraper.ToolTipTitle = "Page ToolTip";
            this.kryptonPageWebScraper.UniqueName = "ed4ced01f1e442d090efd8da6eee1139";
            // 
            // kryptonWorkspaceWebScraper
            // 
            this.kryptonWorkspaceWebScraper.ActivePage = this.kryptonPageWebScraperWebScrapingSettings;
            this.kryptonWorkspaceWebScraper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceWebScraper.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceWebScraper.Name = "kryptonWorkspaceWebScraper";
            // 
            // 
            // 
            this.kryptonWorkspaceWebScraper.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCell4});
            this.kryptonWorkspaceWebScraper.Root.UniqueName = "1205cc50e85d4bb2a77e1b84daa27be4";
            this.kryptonWorkspaceWebScraper.Root.WorkspaceControl = this.kryptonWorkspaceWebScraper;
            this.kryptonWorkspaceWebScraper.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceWebScraper.Size = new System.Drawing.Size(757, 791);
            this.kryptonWorkspaceWebScraper.TabIndex = 0;
            this.kryptonWorkspaceWebScraper.TabStop = true;
            // 
            // kryptonPageWebScraperWebScrapingSettings
            // 
            this.kryptonPageWebScraperWebScrapingSettings.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperJavaScriptExecuteTimeoutDescription);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperJavaScriptExecuteTimeout);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperWebScrapingRetryDescription);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperWebScrapingRetry);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperWebScrapringDelayDescription);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperWebScrapringDelay);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperWebScrapingDelayDescription);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperWebScrapingDelay);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperPageStartLoadingTimeoutDescription);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperPageStartLoadingTimeout);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperPageLoadedTimeoutDescription);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperPageLoadedTimeout);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperNumberOfPageDownDescription);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.labelWebScraperNumberOfPageDown);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.numericUpDownWebScrapingPageDownCount);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.numericUpDownJavaScriptExecuteTimeout);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.numericUpDownWaitEventPageLoadedTimeout);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.numericUpDownWebScrapingRetry);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.numericUpDownWaitEventPageStartLoadingTimeout);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.numericUpDownWebScrapingDelayOurScriptToRun);
            this.kryptonPageWebScraperWebScrapingSettings.Controls.Add(this.numericUpDownWebScrapingDelayInPageScriptToRun);
            this.kryptonPageWebScraperWebScrapingSettings.Flags = 65534;
            this.kryptonPageWebScraperWebScrapingSettings.LastVisibleSet = true;
            this.kryptonPageWebScraperWebScrapingSettings.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageWebScraperWebScrapingSettings.Name = "kryptonPageWebScraperWebScrapingSettings";
            this.kryptonPageWebScraperWebScrapingSettings.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageWebScraperWebScrapingSettings.Text = "Web Scraping settings";
            this.kryptonPageWebScraperWebScrapingSettings.ToolTipTitle = "Page ToolTip";
            this.kryptonPageWebScraperWebScrapingSettings.UniqueName = "c7483a73647b426f9f71d583cd42e1b4";
            // 
            // kryptonWorkspaceCell4
            // 
            this.kryptonWorkspaceCell4.AllowPageDrag = true;
            this.kryptonWorkspaceCell4.AllowTabFocus = false;
            this.kryptonWorkspaceCell4.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Left;
            this.kryptonWorkspaceCell4.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.FixedTop;
            this.kryptonWorkspaceCell4.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCell4.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCell4.Name = "kryptonWorkspaceCell4";
            this.kryptonWorkspaceCell4.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageWebScraperWebScrapingSettings,
            this.kryptonPageWebScraperWebScrapingStartPages});
            this.kryptonWorkspaceCell4.SelectedIndex = 0;
            this.kryptonWorkspaceCell4.UniqueName = "c17b77f2ada7440682e51549724c089a";
            // 
            // kryptonPageWebScraperWebScrapingStartPages
            // 
            this.kryptonPageWebScraperWebScrapingStartPages.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageWebScraperWebScrapingStartPages.Controls.Add(this.textBoxWebScrapingStartPages);
            this.kryptonPageWebScraperWebScrapingStartPages.Flags = 65534;
            this.kryptonPageWebScraperWebScrapingStartPages.LastVisibleSet = true;
            this.kryptonPageWebScraperWebScrapingStartPages.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageWebScraperWebScrapingStartPages.Name = "kryptonPageWebScraperWebScrapingStartPages";
            this.kryptonPageWebScraperWebScrapingStartPages.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageWebScraperWebScrapingStartPages.Text = "Start pages for Web Scraping categories";
            this.kryptonPageWebScraperWebScrapingStartPages.ToolTipTitle = "Page ToolTip";
            this.kryptonPageWebScraperWebScrapingStartPages.UniqueName = "b781a220c40540408e12e0d66ffe3d79";
            // 
            // kryptonPageAutoCorrect
            // 
            this.kryptonPageAutoCorrect.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrect.Controls.Add(this.kryptonWorkspaceAutoCorrect);
            this.kryptonPageAutoCorrect.Flags = 65534;
            this.kryptonPageAutoCorrect.LastVisibleSet = true;
            this.kryptonPageAutoCorrect.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrect.Name = "kryptonPageAutoCorrect";
            this.kryptonPageAutoCorrect.Size = new System.Drawing.Size(757, 791);
            this.kryptonPageAutoCorrect.Text = "AutoCorrect";
            this.kryptonPageAutoCorrect.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrect.UniqueName = "219866b69b4d4edbb734a59b455eb354";
            // 
            // kryptonWorkspaceAutoCorrect
            // 
            this.kryptonWorkspaceAutoCorrect.ActivePage = this.kryptonPageAutoCorrectRename;
            this.kryptonWorkspaceAutoCorrect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceAutoCorrect.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceAutoCorrect.Name = "kryptonWorkspaceAutoCorrect";
            // 
            // 
            // 
            this.kryptonWorkspaceAutoCorrect.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCell5});
            this.kryptonWorkspaceAutoCorrect.Root.UniqueName = "0280942664974b158e62a1090e6c0fc9";
            this.kryptonWorkspaceAutoCorrect.Root.WorkspaceControl = this.kryptonWorkspaceAutoCorrect;
            this.kryptonWorkspaceAutoCorrect.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceAutoCorrect.Size = new System.Drawing.Size(757, 791);
            this.kryptonWorkspaceAutoCorrect.TabIndex = 0;
            this.kryptonWorkspaceAutoCorrect.TabStop = true;
            // 
            // kryptonPageAutoCorrectAutoKeywords
            // 
            this.kryptonPageAutoCorrectAutoKeywords.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectAutoKeywords.Controls.Add(this.dataGridViewAutoKeywords);
            this.kryptonPageAutoCorrectAutoKeywords.Flags = 65534;
            this.kryptonPageAutoCorrectAutoKeywords.LastVisibleSet = true;
            this.kryptonPageAutoCorrectAutoKeywords.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectAutoKeywords.Name = "kryptonPageAutoCorrectAutoKeywords";
            this.kryptonPageAutoCorrectAutoKeywords.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectAutoKeywords.Text = "Keyword synonyms";
            this.kryptonPageAutoCorrectAutoKeywords.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectAutoKeywords.UniqueName = "370125c5671e4d168de35cf939a4e3aa";
            // 
            // kryptonWorkspaceCell5
            // 
            this.kryptonWorkspaceCell5.AllowPageDrag = true;
            this.kryptonWorkspaceCell5.AllowTabFocus = false;
            this.kryptonWorkspaceCell5.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Left;
            this.kryptonWorkspaceCell5.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.FixedTop;
            this.kryptonWorkspaceCell5.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCell5.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCell5.Name = "kryptonWorkspaceCell5";
            this.kryptonWorkspaceCell5.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageAutoCorrectAutoCorrectHelp,
            this.kryptonPageAutoCorrectDateAndTimeDigitized,
            this.kryptonPageAutoCorrectGPSLocationAndDateTime,
            this.kryptonPageAutoCorrectLocationInformation,
            this.kryptonPageAutoCorrectTitle,
            this.kryptonPageAutoCorrectAlbum,
            this.kryptonPageAutoCorrectAuthor,
            this.kryptonPageAutoCorrectKeywordTags,
            this.kryptonPageAutoCorrectBackupOfTags,
            this.kryptonPageAutoCorrectFaceRegionFields,
            this.kryptonPageAutoCorrectKeywordsHelp,
            this.kryptonPageAutoCorrectRename,
            this.kryptonPageAutoCorrectAutoKeywords});
            this.kryptonWorkspaceCell5.SelectedIndex = 11;
            this.kryptonWorkspaceCell5.UniqueName = "ac9d0e4214674cdeb45441915a7be906";
            // 
            // kryptonPageAutoCorrectAutoCorrectHelp
            // 
            this.kryptonPageAutoCorrectAutoCorrectHelp.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectAutoCorrectHelp.Controls.Add(this.textBoxHelpAutoCorrect);
            this.kryptonPageAutoCorrectAutoCorrectHelp.Flags = 65534;
            this.kryptonPageAutoCorrectAutoCorrectHelp.LastVisibleSet = true;
            this.kryptonPageAutoCorrectAutoCorrectHelp.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectAutoCorrectHelp.Name = "kryptonPageAutoCorrectAutoCorrectHelp";
            this.kryptonPageAutoCorrectAutoCorrectHelp.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectAutoCorrectHelp.Text = "AutoCorrect help";
            this.kryptonPageAutoCorrectAutoCorrectHelp.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectAutoCorrectHelp.UniqueName = "0892d4403d444998aa1f77cc6dd73b02";
            // 
            // kryptonPageAutoCorrectDateAndTimeDigitized
            // 
            this.kryptonPageAutoCorrectDateAndTimeDigitized.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectDateAndTimeDigitized.Controls.Add(this.labelAutoCorrectPrioritySourceOrder);
            this.kryptonPageAutoCorrectDateAndTimeDigitized.Controls.Add(this.radioButtonDateTakenDoNotChange);
            this.kryptonPageAutoCorrectDateAndTimeDigitized.Controls.Add(this.radioButtonDateTakenUseFirst);
            this.kryptonPageAutoCorrectDateAndTimeDigitized.Controls.Add(this.imageListViewOrderDateTaken);
            this.kryptonPageAutoCorrectDateAndTimeDigitized.Controls.Add(this.radioButtonDateTakenChangeWhenEmpty);
            this.kryptonPageAutoCorrectDateAndTimeDigitized.Flags = 65534;
            this.kryptonPageAutoCorrectDateAndTimeDigitized.LastVisibleSet = true;
            this.kryptonPageAutoCorrectDateAndTimeDigitized.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectDateAndTimeDigitized.Name = "kryptonPageAutoCorrectDateAndTimeDigitized";
            this.kryptonPageAutoCorrectDateAndTimeDigitized.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectDateAndTimeDigitized.Text = "Date and Time Digitized";
            this.kryptonPageAutoCorrectDateAndTimeDigitized.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectDateAndTimeDigitized.UniqueName = "61afeb3badf74e579b6047fd1e028f1a";
            // 
            // kryptonPageAutoCorrectGPSLocationAndDateTime
            // 
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.label87);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.numericUpDownLocationAccurateIntervalNearByMediaFile);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.checkBoxGPSUpdateLocation);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.checkBoxGPSUpdateLocationNearByMedia);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.checkBoxGPSUpdateDateTime);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.label23);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.labelLocationTimeZoneGuess);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.label22);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.numericUpDownLocationAccurateInterval);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.numericUpDownLocationGuessInterval);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Controls.Add(this.labelLocationTimeZoneAccurate);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Flags = 65534;
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.LastVisibleSet = true;
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Name = "kryptonPageAutoCorrectGPSLocationAndDateTime";
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.Text = "GPS Location and GPS Date and Time";
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.UniqueName = "8b5cacf15fb44e0d83ae4e4cc81cb637";
            // 
            // kryptonPageAutoCorrectLocationInformation
            // 
            this.kryptonPageAutoCorrectLocationInformation.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectLocationInformation.Controls.Add(this.checkBoxUpdateLocationCountry);
            this.kryptonPageAutoCorrectLocationInformation.Controls.Add(this.radioButtonLocationNameDoNotChange);
            this.kryptonPageAutoCorrectLocationInformation.Controls.Add(this.checkBoxUpdateLocationState);
            this.kryptonPageAutoCorrectLocationInformation.Controls.Add(this.radioButtonLocationNameChangeWhenEmpty);
            this.kryptonPageAutoCorrectLocationInformation.Controls.Add(this.checkBoxUpdateLocationCity);
            this.kryptonPageAutoCorrectLocationInformation.Controls.Add(this.radioButtonLocationNameChangeAlways);
            this.kryptonPageAutoCorrectLocationInformation.Controls.Add(this.checkBoxUpdateLocationName);
            this.kryptonPageAutoCorrectLocationInformation.Controls.Add(this.labelAutoCorrectLocationInformationDescription);
            this.kryptonPageAutoCorrectLocationInformation.Controls.Add(this.label15);
            this.kryptonPageAutoCorrectLocationInformation.Flags = 65534;
            this.kryptonPageAutoCorrectLocationInformation.LastVisibleSet = true;
            this.kryptonPageAutoCorrectLocationInformation.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectLocationInformation.Name = "kryptonPageAutoCorrectLocationInformation";
            this.kryptonPageAutoCorrectLocationInformation.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectLocationInformation.Text = "Location information";
            this.kryptonPageAutoCorrectLocationInformation.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectLocationInformation.UniqueName = "ed170dbd7192495491c0508f3766d678";
            // 
            // kryptonPageAutoCorrectTitle
            // 
            this.kryptonPageAutoCorrectTitle.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectTitle.Controls.Add(this.labelAutoCorrectTitlePrioritySource);
            this.kryptonPageAutoCorrectTitle.Controls.Add(this.imageListViewOrderTitle);
            this.kryptonPageAutoCorrectTitle.Controls.Add(this.radioButtonTitleDoNotChange);
            this.kryptonPageAutoCorrectTitle.Controls.Add(this.radioButtonTitleUseFirst);
            this.kryptonPageAutoCorrectTitle.Controls.Add(this.radioButtonTitleChangeWhenEmpty);
            this.kryptonPageAutoCorrectTitle.Flags = 65534;
            this.kryptonPageAutoCorrectTitle.LastVisibleSet = true;
            this.kryptonPageAutoCorrectTitle.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectTitle.Name = "kryptonPageAutoCorrectTitle";
            this.kryptonPageAutoCorrectTitle.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectTitle.Text = "Title";
            this.kryptonPageAutoCorrectTitle.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectTitle.UniqueName = "88062a9b3a214707ac7a7f9bd5f209cb";
            // 
            // kryptonPageAutoCorrectAlbum
            // 
            this.kryptonPageAutoCorrectAlbum.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectAlbum.Controls.Add(this.checkBoxDublicateAlbumAsDescription);
            this.kryptonPageAutoCorrectAlbum.Controls.Add(this.radioButtonAlbumDoNotChange);
            this.kryptonPageAutoCorrectAlbum.Controls.Add(this.label19);
            this.kryptonPageAutoCorrectAlbum.Controls.Add(this.radioButtonAlbumChangeWhenEmpty);
            this.kryptonPageAutoCorrectAlbum.Controls.Add(this.imageListViewOrderAlbum);
            this.kryptonPageAutoCorrectAlbum.Controls.Add(this.radioButtonAlbumUseFirst);
            this.kryptonPageAutoCorrectAlbum.Flags = 65534;
            this.kryptonPageAutoCorrectAlbum.LastVisibleSet = true;
            this.kryptonPageAutoCorrectAlbum.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectAlbum.Name = "kryptonPageAutoCorrectAlbum";
            this.kryptonPageAutoCorrectAlbum.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectAlbum.Text = "Album";
            this.kryptonPageAutoCorrectAlbum.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectAlbum.UniqueName = "bbc413572ab74df5b2f3d12631c1434a";
            // 
            // kryptonPageAutoCorrectAuthor
            // 
            this.kryptonPageAutoCorrectAuthor.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectAuthor.Controls.Add(this.radioButtonAuthorAlwaysChange);
            this.kryptonPageAutoCorrectAuthor.Controls.Add(this.radioButtonAuthorDoNotChange);
            this.kryptonPageAutoCorrectAuthor.Controls.Add(this.label13);
            this.kryptonPageAutoCorrectAuthor.Controls.Add(this.radioButtonAuthorChangeWhenEmpty);
            this.kryptonPageAutoCorrectAuthor.Flags = 65534;
            this.kryptonPageAutoCorrectAuthor.LastVisibleSet = true;
            this.kryptonPageAutoCorrectAuthor.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectAuthor.Name = "kryptonPageAutoCorrectAuthor";
            this.kryptonPageAutoCorrectAuthor.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectAuthor.Text = "Author";
            this.kryptonPageAutoCorrectAuthor.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectAuthor.UniqueName = "83843b43f8574351930c8a2c792fa080";
            // 
            // kryptonPageAutoCorrectKeywordTags
            // 
            this.kryptonPageAutoCorrectKeywordTags.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectKeywordTags.Controls.Add(this.checkBoxKeywordsAddAutoKeywords);
            this.kryptonPageAutoCorrectKeywordTags.Controls.Add(this.checkBoxKeywordsAddWindowsMediaPhotoGallery);
            this.kryptonPageAutoCorrectKeywordTags.Controls.Add(this.checkBoxKeywordsAddWebScraping);
            this.kryptonPageAutoCorrectKeywordTags.Controls.Add(this.checkBoxKeywordsAddMicrosoftPhotos);
            this.kryptonPageAutoCorrectKeywordTags.Controls.Add(this.label4);
            this.kryptonPageAutoCorrectKeywordTags.Controls.Add(this.comboBoxKeywordsAiConfidence);
            this.kryptonPageAutoCorrectKeywordTags.Flags = 65534;
            this.kryptonPageAutoCorrectKeywordTags.LastVisibleSet = true;
            this.kryptonPageAutoCorrectKeywordTags.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectKeywordTags.Name = "kryptonPageAutoCorrectKeywordTags";
            this.kryptonPageAutoCorrectKeywordTags.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectKeywordTags.Text = "Keyword tags";
            this.kryptonPageAutoCorrectKeywordTags.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectKeywordTags.UniqueName = "6477c437f8b742e7a1c93518aaf2230d";
            // 
            // kryptonPageAutoCorrectBackupOfTags
            // 
            this.kryptonPageAutoCorrectBackupOfTags.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxAutoCorrectTrackChanges);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.label17);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupFileCreatedBefore);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupRegionFaceNames);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupFileCreatedAfter);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupLocationName);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupDateTakenBefore);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.labelAutoCorrectBackupOfTags);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupLocationCountry);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupGPSDateTimeUTCAfter);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupGPSDateTimeUTCBefore);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupLocationCity);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupLocationState);
            this.kryptonPageAutoCorrectBackupOfTags.Controls.Add(this.checkBoxKeywordBackupDateTakenAfter);
            this.kryptonPageAutoCorrectBackupOfTags.Flags = 65534;
            this.kryptonPageAutoCorrectBackupOfTags.LastVisibleSet = true;
            this.kryptonPageAutoCorrectBackupOfTags.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectBackupOfTags.Name = "kryptonPageAutoCorrectBackupOfTags";
            this.kryptonPageAutoCorrectBackupOfTags.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectBackupOfTags.Text = "Backup of tags";
            this.kryptonPageAutoCorrectBackupOfTags.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectBackupOfTags.UniqueName = "6ce0b01b17584495a7f601f029317d51";
            // 
            // kryptonPageAutoCorrectFaceRegionFields
            // 
            this.kryptonPageAutoCorrectFaceRegionFields.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectFaceRegionFields.Controls.Add(this.checkBoxFaceRegionAddWebScraping);
            this.kryptonPageAutoCorrectFaceRegionFields.Controls.Add(this.checkBoxFaceRegionAddWindowsMediaPhotoGallery);
            this.kryptonPageAutoCorrectFaceRegionFields.Controls.Add(this.checkBoxFaceRegionAddMicrosoftPhotos);
            this.kryptonPageAutoCorrectFaceRegionFields.Flags = 65534;
            this.kryptonPageAutoCorrectFaceRegionFields.LastVisibleSet = true;
            this.kryptonPageAutoCorrectFaceRegionFields.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectFaceRegionFields.Name = "kryptonPageAutoCorrectFaceRegionFields";
            this.kryptonPageAutoCorrectFaceRegionFields.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectFaceRegionFields.Text = "Face region fields";
            this.kryptonPageAutoCorrectFaceRegionFields.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectFaceRegionFields.UniqueName = "f0f9cba9f5e2405f8371aee9bf7870fc";
            // 
            // kryptonPageAutoCorrectRename
            // 
            this.kryptonPageAutoCorrectRename.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectRename.Controls.Add(this.textBoxRenameTo);
            this.kryptonPageAutoCorrectRename.Controls.Add(this.comboBoxRenameVariables);
            this.kryptonPageAutoCorrectRename.Controls.Add(this.label10);
            this.kryptonPageAutoCorrectRename.Controls.Add(this.label9);
            this.kryptonPageAutoCorrectRename.Controls.Add(this.checkBoxRename);
            this.kryptonPageAutoCorrectRename.Flags = 65534;
            this.kryptonPageAutoCorrectRename.LastVisibleSet = true;
            this.kryptonPageAutoCorrectRename.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectRename.Name = "kryptonPageAutoCorrectRename";
            this.kryptonPageAutoCorrectRename.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectRename.Text = "Rename media file(s)";
            this.kryptonPageAutoCorrectRename.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectRename.UniqueName = "6311de12deb94ffcaadf56e77a4de6ea";
            // 
            // kryptonPageAutoCorrectKeywordsHelp
            // 
            this.kryptonPageAutoCorrectKeywordsHelp.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageAutoCorrectKeywordsHelp.Flags = 65534;
            this.kryptonPageAutoCorrectKeywordsHelp.LastVisibleSet = true;
            this.kryptonPageAutoCorrectKeywordsHelp.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageAutoCorrectKeywordsHelp.Name = "kryptonPageAutoCorrectKeywordsHelp";
            this.kryptonPageAutoCorrectKeywordsHelp.Size = new System.Drawing.Size(555, 789);
            this.kryptonPageAutoCorrectKeywordsHelp.Text = "Keyword synonyms help";
            this.kryptonPageAutoCorrectKeywordsHelp.ToolTipTitle = "Page ToolTip";
            this.kryptonPageAutoCorrectKeywordsHelp.UniqueName = "eb5b617c8db5410ea3da890c88c4e226";
            // 
            // kryptonPageLocation
            // 
            this.kryptonPageLocation.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageLocation.Controls.Add(this.kryptonWorkspaceLocation);
            this.kryptonPageLocation.Flags = 65534;
            this.kryptonPageLocation.LastVisibleSet = true;
            this.kryptonPageLocation.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageLocation.Name = "kryptonPageLocation";
            this.kryptonPageLocation.Size = new System.Drawing.Size(757, 791);
            this.kryptonPageLocation.Text = "Location";
            this.kryptonPageLocation.ToolTipTitle = "Page ToolTip";
            this.kryptonPageLocation.UniqueName = "0ce8d2c0eed042deb2c8ebd325782fca";
            // 
            // kryptonWorkspaceLocation
            // 
            this.kryptonWorkspaceLocation.ActivePage = this.kryptonPage42;
            this.kryptonWorkspaceLocation.Location = new System.Drawing.Point(17, 18);
            this.kryptonWorkspaceLocation.Name = "kryptonWorkspaceLocation";
            // 
            // 
            // 
            this.kryptonWorkspaceLocation.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCell6});
            this.kryptonWorkspaceLocation.Root.UniqueName = "5a9a35e7f6b342ba82e7d7f237742591";
            this.kryptonWorkspaceLocation.Root.WorkspaceControl = this.kryptonWorkspaceLocation;
            this.kryptonWorkspaceLocation.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceLocation.Size = new System.Drawing.Size(713, 628);
            this.kryptonWorkspaceLocation.TabIndex = 0;
            this.kryptonWorkspaceLocation.TabStop = true;
            // 
            // kryptonPage42
            // 
            this.kryptonPage42.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage42.Controls.Add(this.kryptonWorkspaceLocationLocationNames);
            this.kryptonPage42.Flags = 65534;
            this.kryptonPage42.LastVisibleSet = true;
            this.kryptonPage42.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage42.Name = "kryptonPage42";
            this.kryptonPage42.Size = new System.Drawing.Size(511, 626);
            this.kryptonPage42.Text = "Location names";
            this.kryptonPage42.ToolTipTitle = "Page ToolTip";
            this.kryptonPage42.UniqueName = "ee498799ed034130abeb6e66f41bedda";
            // 
            // kryptonWorkspaceLocationLocationNames
            // 
            this.kryptonWorkspaceLocationLocationNames.ActivePage = this.kryptonPageLocationLocationNameNames;
            this.kryptonWorkspaceLocationLocationNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceLocationLocationNames.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceLocationLocationNames.Name = "kryptonWorkspaceLocationLocationNames";
            // 
            // 
            // 
            this.kryptonWorkspaceLocationLocationNames.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCellLocationLocationNameNames,
            this.kryptonWorkspaceCellLocationLocationNameMap});
            this.kryptonWorkspaceLocationLocationNames.Root.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonWorkspaceLocationLocationNames.Root.StarSize = "75*,25*";
            this.kryptonWorkspaceLocationLocationNames.Root.UniqueName = "2a34af3700cd4deb9926f1ef754a54f4";
            this.kryptonWorkspaceLocationLocationNames.Root.WorkspaceControl = this.kryptonWorkspaceLocationLocationNames;
            this.kryptonWorkspaceLocationLocationNames.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceLocationLocationNames.Size = new System.Drawing.Size(511, 626);
            this.kryptonWorkspaceLocationLocationNames.TabIndex = 0;
            this.kryptonWorkspaceLocationLocationNames.TabStop = true;
            // 
            // kryptonPageLocationLocationNameNames
            // 
            this.kryptonPageLocationLocationNameNames.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageLocationLocationNameNames.Controls.Add(this.dataGridViewLocationNames);
            this.kryptonPageLocationLocationNameNames.Flags = 65534;
            this.kryptonPageLocationLocationNameNames.LastVisibleSet = true;
            this.kryptonPageLocationLocationNameNames.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageLocationLocationNameNames.Name = "kryptonPageLocationLocationNameNames";
            this.kryptonPageLocationLocationNameNames.Size = new System.Drawing.Size(509, 345);
            this.kryptonPageLocationLocationNameNames.Text = "kryptonPage43";
            this.kryptonPageLocationLocationNameNames.ToolTipTitle = "Page ToolTip";
            this.kryptonPageLocationLocationNameNames.UniqueName = "cee3980d1c8f4592932ab094964184d4";
            // 
            // kryptonWorkspaceCellLocationLocationNameNames
            // 
            this.kryptonWorkspaceCellLocationLocationNameNames.AllowPageDrag = true;
            this.kryptonWorkspaceCellLocationLocationNameNames.AllowTabFocus = false;
            this.kryptonWorkspaceCellLocationLocationNameNames.Name = "kryptonWorkspaceCellLocationLocationNameNames";
            this.kryptonWorkspaceCellLocationLocationNameNames.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageLocationLocationNameNames});
            this.kryptonWorkspaceCellLocationLocationNameNames.SelectedIndex = 0;
            this.kryptonWorkspaceCellLocationLocationNameNames.StarSize = "50*,75*";
            this.kryptonWorkspaceCellLocationLocationNameNames.UniqueName = "e526156333ef49c0ad35d30c16294283";
            // 
            // kryptonWorkspaceCellLocationLocationNameMap
            // 
            this.kryptonWorkspaceCellLocationLocationNameMap.AllowPageDrag = true;
            this.kryptonWorkspaceCellLocationLocationNameMap.AllowTabFocus = false;
            this.kryptonWorkspaceCellLocationLocationNameMap.Name = "kryptonWorkspaceCellLocationLocationNameMap";
            this.kryptonWorkspaceCellLocationLocationNameMap.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageLocationLocationNameMap});
            this.kryptonWorkspaceCellLocationLocationNameMap.SelectedIndex = 0;
            this.kryptonWorkspaceCellLocationLocationNameMap.UniqueName = "39016dfb221c43729d91de8983a898eb";
            // 
            // kryptonPageLocationLocationNameMap
            // 
            this.kryptonPageLocationLocationNameMap.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageLocationLocationNameMap.Controls.Add(this.textBoxBrowserURL);
            this.kryptonPageLocationLocationNameMap.Controls.Add(this.pictureBox1);
            this.kryptonPageLocationLocationNameMap.Controls.Add(this.panelBrowser);
            this.kryptonPageLocationLocationNameMap.Controls.Add(this.comboBoxMapZoomLevel);
            this.kryptonPageLocationLocationNameMap.Flags = 65534;
            this.kryptonPageLocationLocationNameMap.LastVisibleSet = true;
            this.kryptonPageLocationLocationNameMap.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageLocationLocationNameMap.Name = "kryptonPageLocationLocationNameMap";
            this.kryptonPageLocationLocationNameMap.Size = new System.Drawing.Size(509, 222);
            this.kryptonPageLocationLocationNameMap.Text = "kryptonPage46";
            this.kryptonPageLocationLocationNameMap.ToolTipTitle = "Page ToolTip";
            this.kryptonPageLocationLocationNameMap.UniqueName = "2572f50833e04b1eb4343bacc77d2108";
            // 
            // kryptonWorkspaceCell6
            // 
            this.kryptonWorkspaceCell6.AllowPageDrag = true;
            this.kryptonWorkspaceCell6.AllowTabFocus = false;
            this.kryptonWorkspaceCell6.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Left;
            this.kryptonWorkspaceCell6.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.FixedTop;
            this.kryptonWorkspaceCell6.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCell6.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCell6.Name = "kryptonWorkspaceCell6";
            this.kryptonWorkspaceCell6.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPage38,
            this.kryptonPage39,
            this.kryptonPage41,
            this.kryptonPage40,
            this.kryptonPage42});
            this.kryptonWorkspaceCell6.SelectedIndex = 4;
            this.kryptonWorkspaceCell6.UniqueName = "b1d9a40ef8a64139b04217e840cf2455";
            // 
            // kryptonPage38
            // 
            this.kryptonPage38.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage38.Controls.Add(this.textBox3);
            this.kryptonPage38.Flags = 65534;
            this.kryptonPage38.LastVisibleSet = true;
            this.kryptonPage38.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage38.Name = "kryptonPage38";
            this.kryptonPage38.Size = new System.Drawing.Size(511, 626);
            this.kryptonPage38.Text = "Camera Owner help";
            this.kryptonPage38.ToolTipTitle = "Page ToolTip";
            this.kryptonPage38.UniqueName = "0a562f543413447e916b5bdb7f95a6da";
            // 
            // kryptonPage39
            // 
            this.kryptonPage39.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage39.Controls.Add(this.dataGridViewCameraOwner);
            this.kryptonPage39.Flags = 65534;
            this.kryptonPage39.LastVisibleSet = true;
            this.kryptonPage39.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage39.Name = "kryptonPage39";
            this.kryptonPage39.Size = new System.Drawing.Size(511, 626);
            this.kryptonPage39.Text = "Camera owner";
            this.kryptonPage39.ToolTipTitle = "Page ToolTip";
            this.kryptonPage39.UniqueName = "898c05621f8e4af1b3b4faf224fd28ef";
            // 
            // kryptonPage41
            // 
            this.kryptonPage41.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage41.Controls.Add(this.textBox4);
            this.kryptonPage41.Flags = 65534;
            this.kryptonPage41.LastVisibleSet = true;
            this.kryptonPage41.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage41.Name = "kryptonPage41";
            this.kryptonPage41.Size = new System.Drawing.Size(511, 626);
            this.kryptonPage41.Text = "Location names - help";
            this.kryptonPage41.ToolTipTitle = "Page ToolTip";
            this.kryptonPage41.UniqueName = "ed82dc85818a43c18e0eb421001c448a";
            // 
            // kryptonPage40
            // 
            this.kryptonPage40.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage40.Controls.Add(this.buttonLocationImport);
            this.kryptonPage40.Controls.Add(this.buttonLocationExport);
            this.kryptonPage40.Flags = 65534;
            this.kryptonPage40.LastVisibleSet = true;
            this.kryptonPage40.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage40.Name = "kryptonPage40";
            this.kryptonPage40.Size = new System.Drawing.Size(511, 626);
            this.kryptonPage40.Text = "Location names Import and Export";
            this.kryptonPage40.ToolTipTitle = "Page ToolTip";
            this.kryptonPage40.UniqueName = "d3e4db5922324857ab48af92e79d74bf";
            // 
            // kryptonPageConvertAndMerge
            // 
            this.kryptonPageConvertAndMerge.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageConvertAndMerge.Flags = 65534;
            this.kryptonPageConvertAndMerge.LastVisibleSet = true;
            this.kryptonPageConvertAndMerge.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageConvertAndMerge.Name = "kryptonPageConvertAndMerge";
            this.kryptonPageConvertAndMerge.Size = new System.Drawing.Size(757, 791);
            this.kryptonPageConvertAndMerge.Text = "Convert and Merge";
            this.kryptonPageConvertAndMerge.ToolTipTitle = "Page ToolTip";
            this.kryptonPageConvertAndMerge.UniqueName = "e754400f44cb4e8da8725d1ed5002ada";
            // 
            // kryptonPageChromecast
            // 
            this.kryptonPageChromecast.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageChromecast.Controls.Add(this.kryptonWorkspaceChromecast);
            this.kryptonPageChromecast.Flags = 65534;
            this.kryptonPageChromecast.LastVisibleSet = true;
            this.kryptonPageChromecast.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageChromecast.Name = "kryptonPageChromecast";
            this.kryptonPageChromecast.Size = new System.Drawing.Size(757, 791);
            this.kryptonPageChromecast.Text = "Chromecast";
            this.kryptonPageChromecast.ToolTipTitle = "Page ToolTip";
            this.kryptonPageChromecast.UniqueName = "97b0d2538d0a4fbca85fc1bb76fe8047";
            // 
            // kryptonWorkspaceChromecast
            // 
            this.kryptonWorkspaceChromecast.ActivePage = this.kryptonPageChromecastVLCstreamConfig;
            this.kryptonWorkspaceChromecast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceChromecast.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceChromecast.Name = "kryptonWorkspaceChromecast";
            // 
            // 
            // 
            this.kryptonWorkspaceChromecast.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCellChromecast});
            this.kryptonWorkspaceChromecast.Root.UniqueName = "3acbc3a7a96349b783ee98b2d7809764";
            this.kryptonWorkspaceChromecast.Root.WorkspaceControl = this.kryptonWorkspaceChromecast;
            this.kryptonWorkspaceChromecast.SeparatorStyle = Krypton.Toolkit.SeparatorStyle.HighProfile;
            this.kryptonWorkspaceChromecast.Size = new System.Drawing.Size(757, 791);
            this.kryptonWorkspaceChromecast.TabIndex = 0;
            this.kryptonWorkspaceChromecast.TabStop = true;
            // 
            // kryptonPageChromecastVLCstreamConfig
            // 
            this.kryptonPageChromecastVLCstreamConfig.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageChromecastVLCstreamConfig.Controls.Add(this.comboBoxChromecastVideoCodec);
            this.kryptonPageChromecastVLCstreamConfig.Controls.Add(this.comboBoxChromecastUrl);
            this.kryptonPageChromecastVLCstreamConfig.Controls.Add(this.label61);
            this.kryptonPageChromecastVLCstreamConfig.Controls.Add(this.label58);
            this.kryptonPageChromecastVLCstreamConfig.Controls.Add(this.label60);
            this.kryptonPageChromecastVLCstreamConfig.Controls.Add(this.comboBoxChromecastAgruments);
            this.kryptonPageChromecastVLCstreamConfig.Controls.Add(this.label59);
            this.kryptonPageChromecastVLCstreamConfig.Controls.Add(this.comboBoxChromecastAudioCodec);
            this.kryptonPageChromecastVLCstreamConfig.Flags = 65534;
            this.kryptonPageChromecastVLCstreamConfig.LastVisibleSet = true;
            this.kryptonPageChromecastVLCstreamConfig.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageChromecastVLCstreamConfig.Name = "kryptonPageChromecastVLCstreamConfig";
            this.kryptonPageChromecastVLCstreamConfig.Size = new System.Drawing.Size(635, 789);
            this.kryptonPageChromecastVLCstreamConfig.Text = "VLC Stream config";
            this.kryptonPageChromecastVLCstreamConfig.ToolTipTitle = "Page ToolTip";
            this.kryptonPageChromecastVLCstreamConfig.UniqueName = "32c9610eb6e64de1b04f0e5b35152ed6";
            // 
            // comboBoxChromecastVideoCodec
            // 
            this.comboBoxChromecastVideoCodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxChromecastVideoCodec.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxChromecastVideoCodec.DropDownWidth = 605;
            this.comboBoxChromecastVideoCodec.FormattingEnabled = true;
            this.comboBoxChromecastVideoCodec.IntegralHeight = false;
            this.comboBoxChromecastVideoCodec.Items.AddRange(new object[] {
            "For MP4 Container:",
            "vcodec=mp4v,fps=24,venc=x264{preset=ultrafast,crf=21},maxwidth=1280,maxheight=720" +
                "",
            "vcodec=h264,fps=24,venc=x264{cfr=16},scale=1",
            "vcodec=h264,fps=24,venc=x264{cfr=40},scale=1",
            "vcodec=h264,fps=24,venc=x264{preset=ultrafast,crf=21},maxwidth=1920,maxheight=108" +
                "0",
            "vcodec=h264,fps=24,venc=x264{preset=ultrafast,crf=21},maxwidth=1280,maxheight=720" +
                "",
            "",
            "For OGG Container:",
            "vcodec=theo,venc=theora{quality=9},scale=1",
            "vcodec=theo,venc=theora{quality=4},scale=1",
            "",
            "For WEBM Container:",
            "vcodec=VP80,vb=2000,scale=1",
            "vcodec=VP80,vb=1000,scale=1",
            "",
            "OTHER:",
            "vcodec=mp1v",
            "vcodec=mp2v",
            "vcodec=mp4v",
            "vcodec=WMV1",
            "vcodec=WMV2",
            "vcodec=WMV3",
            "vcodec=H263",
            "vcodec=HEVC",
            "vcodec=CYUV",
            "vcodec=HFYU",
            "vcodec=vp31",
            "vcodec=vp62",
            "vcodec=vp90"});
            this.comboBoxChromecastVideoCodec.Location = new System.Drawing.Point(149, 16);
            this.comboBoxChromecastVideoCodec.Name = "comboBoxChromecastVideoCodec";
            this.comboBoxChromecastVideoCodec.Size = new System.Drawing.Size(478, 21);
            this.comboBoxChromecastVideoCodec.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxChromecastVideoCodec.TabIndex = 0;
            // 
            // comboBoxChromecastUrl
            // 
            this.comboBoxChromecastUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxChromecastUrl.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxChromecastUrl.DropDownWidth = 605;
            this.comboBoxChromecastUrl.FormattingEnabled = true;
            this.comboBoxChromecastUrl.IntegralHeight = false;
            this.comboBoxChromecastUrl.Items.AddRange(new object[] {
            ":{port}/output.MP2T",
            "{ipaddress}:{port}/output.MP2T",
            ":{port}/output.MP4",
            "{ipaddress}:{port}/output.MP4",
            ":{port}/output.OGG",
            "{ipaddress}:{port}/output.OGG",
            ":{port}/output.WebM",
            "{ipaddress}:{port}/output.WebM",
            ""});
            this.comboBoxChromecastUrl.Location = new System.Drawing.Point(149, 70);
            this.comboBoxChromecastUrl.Name = "comboBoxChromecastUrl";
            this.comboBoxChromecastUrl.Size = new System.Drawing.Size(478, 21);
            this.comboBoxChromecastUrl.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxChromecastUrl.TabIndex = 2;
            // 
            // label61
            // 
            this.label61.Location = new System.Drawing.Point(9, 99);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(108, 20);
            this.label61.TabIndex = 18;
            this.label61.Values.Text = "Container(Muxer):";
            // 
            // label58
            // 
            this.label58.Location = new System.Drawing.Point(9, 71);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(90, 20);
            this.label58.TabIndex = 19;
            this.label58.Values.Text = "Url (Port:Path):";
            // 
            // label60
            // 
            this.label60.Location = new System.Drawing.Point(9, 44);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(81, 20);
            this.label60.TabIndex = 16;
            this.label60.Values.Text = "Audio codec:";
            // 
            // comboBoxChromecastAgruments
            // 
            this.comboBoxChromecastAgruments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxChromecastAgruments.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxChromecastAgruments.DropDownWidth = 605;
            this.comboBoxChromecastAgruments.FormattingEnabled = true;
            this.comboBoxChromecastAgruments.IntegralHeight = false;
            this.comboBoxChromecastAgruments.Items.AddRange(new object[] {
            "ogg - Xiph.org\'s ogg container format. ",
            "matroska - For codec H264",
            "webm - For codec VP8, VP9, VORBIT, OPUS ",
            "",
            "sout=#transcode{{vcodec},{acodec}}:standard{access=http{mime=video/ogg},mux=ogg,d" +
                "st={url}}",
            "sout=#transcode{{vcodec},{acodec}}:standard{access=http{mime=video/matroska},mux=" +
                "ogg,dst={url}}",
            "sout=#transcode{{vcodec},{acodec}}:standard{access=http{mime=video/webm},mux=webm" +
                ",dst={url}}",
            "sout=#transcode{{vcodec},{acodec}}:standard{access=http{mime=video/webm},mux=ffmp" +
                "eg{mux=webm},dst=\'{url}\'}",
            "",
            "sout=#transcode{{vcodec},{acodec}}:std{access=http,mux=ogg,url={url}}",
            "sout=#transcode{{vcodec},{acodec},mux=ogg}:http{dst={url}}",
            "sout=#transcode{{vcodec},{acodec},mux=webm}:http{dst={url}}",
            "sout=#transcode{{vcodec},{acodec}}:std{access=http,mux=ogg,url={url}}",
            "sout=#transcode{{vcodec},{acodec}}:std{access=http,mux=ffmpeg{mux=webm},url={url}" +
                "}",
            "sout=#transcode{{vcodec},{acodec}}:chromecast-proxy:std{mux=avformat{mux=webm,opt" +
                "ions={live=1},reset-ts},access=chromecast-http}",
            "sout=#transcode{{vcodec},{acodec}}:chromecast-proxy:std{mux=avformat{mux=matroska" +
                ",options={live=1},reset-ts},access=chromecast-http}",
            "sout=#transcode{{vcodec},{acodec},mux=avi}:http{dst={url}}",
            "sout=#transcode{{vcodec},{acodec},mux=asf}:http{dst={url}}",
            "sout=#transcode{{vcodec},{acodec},mux=ogg}:http{dst={url}}",
            "sout=#transcode{{vcodec},{acodec},mux=ts}:http{dst={url}}",
            "sout=#transcode{{vcodec},{acodec},mux=ps}:http{dst={url}}",
            "sout=#transcode{{vcodec},{acodec},mux=mp4}:http{dst={url}}",
            "sout=#transcode{{vcodec},{acodec},mux=mpeg1}:http{dst={url}}"});
            this.comboBoxChromecastAgruments.Location = new System.Drawing.Point(149, 98);
            this.comboBoxChromecastAgruments.Name = "comboBoxChromecastAgruments";
            this.comboBoxChromecastAgruments.Size = new System.Drawing.Size(478, 21);
            this.comboBoxChromecastAgruments.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxChromecastAgruments.TabIndex = 3;
            // 
            // label59
            // 
            this.label59.Location = new System.Drawing.Point(9, 17);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(81, 20);
            this.label59.TabIndex = 14;
            this.label59.Values.Text = "Video codec:";
            // 
            // comboBoxChromecastAudioCodec
            // 
            this.comboBoxChromecastAudioCodec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxChromecastAudioCodec.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxChromecastAudioCodec.DropDownWidth = 605;
            this.comboBoxChromecastAudioCodec.FormattingEnabled = true;
            this.comboBoxChromecastAudioCodec.IntegralHeight = false;
            this.comboBoxChromecastAudioCodec.Items.AddRange(new object[] {
            "MP4:",
            "acodec=mp4a,channels=2,ab=160,samplerate=44100",
            "acodec=mp4a,channels=2,ab=128,samplerate=44100",
            "acodec=mp4a,channels=2,ab=96,samplerate=44100",
            "acodec=mp3,channels=2,ab=128",
            "OGG/WEBM:",
            "acodec=vorb,channels=2,aenc=vorbis{quality=9}",
            "acodec=vorb,channels=2,aenc=vorbis{quality=4}",
            "acodec=vorb,channels=2,aenc=vorbis{quality=1}",
            "acodec=vorb,channels=2,ab=160,samplerate=44100",
            "acodec=vorb,channels=2,ab=128,samplerate=44100",
            "acodec=vorb,channels=2,ab=96,samplerate=44100",
            "OTHER:",
            "acodec=a52,channels=2",
            "acodec=opus,channels=2",
            "acodec=spx,channels=2",
            "acodec=flac,channels=2"});
            this.comboBoxChromecastAudioCodec.Location = new System.Drawing.Point(149, 43);
            this.comboBoxChromecastAudioCodec.Name = "comboBoxChromecastAudioCodec";
            this.comboBoxChromecastAudioCodec.Size = new System.Drawing.Size(478, 21);
            this.comboBoxChromecastAudioCodec.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxChromecastAudioCodec.TabIndex = 1;
            // 
            // kryptonWorkspaceCellChromecast
            // 
            this.kryptonWorkspaceCellChromecast.AllowPageDrag = true;
            this.kryptonWorkspaceCellChromecast.AllowTabFocus = false;
            this.kryptonWorkspaceCellChromecast.Bar.BarOrientation = Krypton.Toolkit.VisualOrientation.Left;
            this.kryptonWorkspaceCellChromecast.Bar.ItemOrientation = Krypton.Toolkit.ButtonOrientation.FixedTop;
            this.kryptonWorkspaceCellChromecast.Button.CloseButtonAction = Krypton.Navigator.CloseButtonAction.None;
            this.kryptonWorkspaceCellChromecast.Button.CloseButtonDisplay = Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonWorkspaceCellChromecast.Name = "kryptonWorkspaceCellChromecast";
            this.kryptonWorkspaceCellChromecast.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageChromecastImage,
            this.kryptonPageChromecastVideo,
            this.kryptonPageChromecastVLCstreamConfig});
            this.kryptonWorkspaceCellChromecast.SelectedIndex = 2;
            this.kryptonWorkspaceCellChromecast.UniqueName = "c9537e2ae93a48f39a7f8e7825615bdf";
            // 
            // kryptonPageChromecastImage
            // 
            this.kryptonPageChromecastImage.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageChromecastImage.Controls.Add(this.comboBoxChromecastImageFormat);
            this.kryptonPageChromecastImage.Controls.Add(this.label57);
            this.kryptonPageChromecastImage.Controls.Add(this.label63);
            this.kryptonPageChromecastImage.Controls.Add(this.comboBoxChromecastImageResolution);
            this.kryptonPageChromecastImage.Flags = 65534;
            this.kryptonPageChromecastImage.LastVisibleSet = true;
            this.kryptonPageChromecastImage.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageChromecastImage.Name = "kryptonPageChromecastImage";
            this.kryptonPageChromecastImage.Size = new System.Drawing.Size(690, 557);
            this.kryptonPageChromecastImage.Text = "Image";
            this.kryptonPageChromecastImage.ToolTipTitle = "Page ToolTip";
            this.kryptonPageChromecastImage.UniqueName = "0604ea7ef67d448182cab39dc7efc07d";
            // 
            // comboBoxChromecastImageFormat
            // 
            this.comboBoxChromecastImageFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxChromecastImageFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChromecastImageFormat.DropDownWidth = 157;
            this.comboBoxChromecastImageFormat.FormattingEnabled = true;
            this.comboBoxChromecastImageFormat.IntegralHeight = false;
            this.comboBoxChromecastImageFormat.Items.AddRange(new object[] {
            ".BMP",
            ".GIF",
            ".JPEG",
            ".PNG",
            ".WEBP"});
            this.comboBoxChromecastImageFormat.Location = new System.Drawing.Point(146, 40);
            this.comboBoxChromecastImageFormat.Name = "comboBoxChromecastImageFormat";
            this.comboBoxChromecastImageFormat.Size = new System.Drawing.Size(157, 21);
            this.comboBoxChromecastImageFormat.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxChromecastImageFormat.TabIndex = 1;
            // 
            // label57
            // 
            this.label57.Location = new System.Drawing.Point(6, 13);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(110, 20);
            this.label57.TabIndex = 12;
            this.label57.Values.Text = "Output resolution:";
            // 
            // label63
            // 
            this.label63.Location = new System.Drawing.Point(6, 43);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(52, 20);
            this.label63.TabIndex = 13;
            this.label63.Values.Text = "Format:";
            // 
            // comboBoxChromecastImageResolution
            // 
            this.comboBoxChromecastImageResolution.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxChromecastImageResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChromecastImageResolution.DropDownWidth = 157;
            this.comboBoxChromecastImageResolution.FormattingEnabled = true;
            this.comboBoxChromecastImageResolution.IntegralHeight = false;
            this.comboBoxChromecastImageResolution.Items.AddRange(new object[] {
            "Original",
            "2160p: 3840 x 2160",
            "1440p: 2560 x 1440",
            "1080p: 1920 x 1080",
            "720p: 1280 x 720",
            "480p: 854 x 480",
            "360p: 640 x 360",
            "240p: 426 x 240"});
            this.comboBoxChromecastImageResolution.Location = new System.Drawing.Point(146, 10);
            this.comboBoxChromecastImageResolution.Name = "comboBoxChromecastImageResolution";
            this.comboBoxChromecastImageResolution.Size = new System.Drawing.Size(157, 21);
            this.comboBoxChromecastImageResolution.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxChromecastImageResolution.TabIndex = 0;
            // 
            // kryptonPageChromecastVideo
            // 
            this.kryptonPageChromecastVideo.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageChromecastVideo.Controls.Add(this.label66);
            this.kryptonPageChromecastVideo.Controls.Add(this.comboBoxChromecastVideoTransporter);
            this.kryptonPageChromecastVideo.Flags = 65534;
            this.kryptonPageChromecastVideo.LastVisibleSet = true;
            this.kryptonPageChromecastVideo.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageChromecastVideo.Name = "kryptonPageChromecastVideo";
            this.kryptonPageChromecastVideo.Size = new System.Drawing.Size(690, 557);
            this.kryptonPageChromecastVideo.Text = "Video";
            this.kryptonPageChromecastVideo.ToolTipTitle = "Page ToolTip";
            this.kryptonPageChromecastVideo.UniqueName = "9b5002b092f74b48a2928ff940229d09";
            // 
            // label66
            // 
            this.label66.Location = new System.Drawing.Point(7, 16);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(109, 20);
            this.label66.TabIndex = 26;
            this.label66.Values.Text = "Video transporter:";
            // 
            // comboBoxChromecastVideoTransporter
            // 
            this.comboBoxChromecastVideoTransporter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxChromecastVideoTransporter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChromecastVideoTransporter.DropDownWidth = 332;
            this.comboBoxChromecastVideoTransporter.FormattingEnabled = true;
            this.comboBoxChromecastVideoTransporter.IntegralHeight = false;
            this.comboBoxChromecastVideoTransporter.Items.AddRange(new object[] {
            "HTTP - Simple HTTP server, send video as is",
            "VLC-Render - Use VLC own Chromecast stack",
            "VLC-Stream - Use VLC stream and own config"});
            this.comboBoxChromecastVideoTransporter.Location = new System.Drawing.Point(147, 13);
            this.comboBoxChromecastVideoTransporter.Name = "comboBoxChromecastVideoTransporter";
            this.comboBoxChromecastVideoTransporter.Size = new System.Drawing.Size(332, 21);
            this.comboBoxChromecastVideoTransporter.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxChromecastVideoTransporter.TabIndex = 0;
            // 
            // kryptonPageLog
            // 
            this.kryptonPageLog.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageLog.Controls.Add(this.kryptonWorkspaceLog);
            this.kryptonPageLog.Flags = 65534;
            this.kryptonPageLog.LastVisibleSet = true;
            this.kryptonPageLog.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageLog.Name = "kryptonPageLog";
            this.kryptonPageLog.Size = new System.Drawing.Size(757, 791);
            this.kryptonPageLog.Text = "Log";
            this.kryptonPageLog.ToolTipTitle = "Page ToolTip";
            this.kryptonPageLog.UniqueName = "db92d8f583a2414d95c003b48b78ddb3";
            // 
            // kryptonWorkspaceLog
            // 
            this.kryptonWorkspaceLog.ActivePage = this.kryptonPageLogApplication;
            this.kryptonWorkspaceLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonWorkspaceLog.Location = new System.Drawing.Point(0, 0);
            this.kryptonWorkspaceLog.Name = "kryptonWorkspaceLog";
            // 
            // 
            // 
            this.kryptonWorkspaceLog.Root.Children.AddRange(new System.ComponentModel.Component[] {
            this.kryptonWorkspaceCellLog});
            this.kryptonWorkspaceLog.Root.UniqueName = "e3ea08e1b3944ecc9ad20137a3847788";
            this.kryptonWorkspaceLog.Root.WorkspaceControl = this.kryptonWorkspaceLog;
            this.kryptonWorkspaceLog.Size = new System.Drawing.Size(757, 791);
            this.kryptonWorkspaceLog.TabIndex = 0;
            this.kryptonWorkspaceLog.TabStop = true;
            // 
            // kryptonPageLogApplication
            // 
            this.kryptonPageLogApplication.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageLogApplication.Controls.Add(this.fastColoredTextBoxShowLog);
            this.kryptonPageLogApplication.Flags = 65534;
            this.kryptonPageLogApplication.LastVisibleSet = true;
            this.kryptonPageLogApplication.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageLogApplication.Name = "kryptonPageLogApplication";
            this.kryptonPageLogApplication.Size = new System.Drawing.Size(755, 739);
            this.kryptonPageLogApplication.Text = "Application";
            this.kryptonPageLogApplication.ToolTipTitle = "Page ToolTip";
            this.kryptonPageLogApplication.UniqueName = "faa1f4e9d6944471862cf75c62c08d8b";
            // 
            // fastColoredTextBoxShowLog
            // 
            this.fastColoredTextBoxShowLog.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxShowLog.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.fastColoredTextBoxShowLog.BackBrush = null;
            this.fastColoredTextBoxShowLog.CharHeight = 14;
            this.fastColoredTextBoxShowLog.CharWidth = 8;
            this.fastColoredTextBoxShowLog.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxShowLog.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxShowLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastColoredTextBoxShowLog.HighlightingRangeType = FastColoredTextBoxNS.HighlightingRangeType.VisibleRange;
            this.fastColoredTextBoxShowLog.IsReplaceMode = false;
            this.fastColoredTextBoxShowLog.Location = new System.Drawing.Point(0, 0);
            this.fastColoredTextBoxShowLog.Name = "fastColoredTextBoxShowLog";
            this.fastColoredTextBoxShowLog.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxShowLog.ReadOnly = true;
            this.fastColoredTextBoxShowLog.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxShowLog.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxShowLog.ServiceColors")));
            this.fastColoredTextBoxShowLog.Size = new System.Drawing.Size(755, 739);
            this.fastColoredTextBoxShowLog.TabIndex = 0;
            this.fastColoredTextBoxShowLog.WordWrap = true;
            this.fastColoredTextBoxShowLog.WordWrapIndent = 3;
            this.fastColoredTextBoxShowLog.WordWrapMode = FastColoredTextBoxNS.WordWrapMode.Custom;
            this.fastColoredTextBoxShowLog.Zoom = 100;
            this.fastColoredTextBoxShowLog.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxShowLog_TextChanged);
            this.fastColoredTextBoxShowLog.TextChangedDelayed += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxShowLog_TextChangedDelayed);
            this.fastColoredTextBoxShowLog.VisibleRangeChangedDelayed += new System.EventHandler(this.fastColoredTextBoxShowLog_VisibleRangeChangedDelayed);
            this.fastColoredTextBoxShowLog.WordWrapNeeded += new System.EventHandler<FastColoredTextBoxNS.WordWrapNeededEventArgs>(this.fastColoredTextBoxShowLog_WordWrapNeeded);
            // 
            // kryptonWorkspaceCellLog
            // 
            this.kryptonWorkspaceCellLog.AllowPageDrag = true;
            this.kryptonWorkspaceCellLog.AllowTabFocus = false;
            this.kryptonWorkspaceCellLog.Name = "kryptonWorkspaceCellLog";
            this.kryptonWorkspaceCellLog.NavigatorMode = Krypton.Navigator.NavigatorMode.StackCheckButtonGroup;
            this.kryptonWorkspaceCellLog.Pages.AddRange(new Krypton.Navigator.KryptonPage[] {
            this.kryptonPageLogApplication,
            this.kryptonPageLogPipe});
            this.kryptonWorkspaceCellLog.SelectedIndex = 0;
            this.kryptonWorkspaceCellLog.UniqueName = "bef22f6226f54736a9d48cde984d4266";
            // 
            // kryptonPageLogPipe
            // 
            this.kryptonPageLogPipe.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageLogPipe.Controls.Add(this.fastColoredTextBoxShowPipe32Log);
            this.kryptonPageLogPipe.Flags = 65534;
            this.kryptonPageLogPipe.LastVisibleSet = true;
            this.kryptonPageLogPipe.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageLogPipe.Name = "kryptonPageLogPipe";
            this.kryptonPageLogPipe.Size = new System.Drawing.Size(569, 415);
            this.kryptonPageLogPipe.Text = "Windows Live Photo Gallery Pipe Server";
            this.kryptonPageLogPipe.ToolTipTitle = "Page ToolTip";
            this.kryptonPageLogPipe.UniqueName = "9088412512e941e1b5bc312c3206ff6e";
            // 
            // fastColoredTextBoxShowPipe32Log
            // 
            this.fastColoredTextBoxShowPipe32Log.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBoxShowPipe32Log.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.fastColoredTextBoxShowPipe32Log.BackBrush = null;
            this.fastColoredTextBoxShowPipe32Log.CharHeight = 14;
            this.fastColoredTextBoxShowPipe32Log.CharWidth = 8;
            this.fastColoredTextBoxShowPipe32Log.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBoxShowPipe32Log.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBoxShowPipe32Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastColoredTextBoxShowPipe32Log.HighlightingRangeType = FastColoredTextBoxNS.HighlightingRangeType.VisibleRange;
            this.fastColoredTextBoxShowPipe32Log.IsReplaceMode = false;
            this.fastColoredTextBoxShowPipe32Log.Location = new System.Drawing.Point(0, 0);
            this.fastColoredTextBoxShowPipe32Log.Name = "fastColoredTextBoxShowPipe32Log";
            this.fastColoredTextBoxShowPipe32Log.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBoxShowPipe32Log.ReadOnly = true;
            this.fastColoredTextBoxShowPipe32Log.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBoxShowPipe32Log.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBoxShowPipe32Log.ServiceColors")));
            this.fastColoredTextBoxShowPipe32Log.Size = new System.Drawing.Size(569, 415);
            this.fastColoredTextBoxShowPipe32Log.TabIndex = 1;
            this.fastColoredTextBoxShowPipe32Log.WordWrap = true;
            this.fastColoredTextBoxShowPipe32Log.WordWrapIndent = 3;
            this.fastColoredTextBoxShowPipe32Log.WordWrapMode = FastColoredTextBoxNS.WordWrapMode.Custom;
            this.fastColoredTextBoxShowPipe32Log.Zoom = 100;
            this.fastColoredTextBoxShowPipe32Log.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxShowPipe32Log_TextChanged);
            this.fastColoredTextBoxShowPipe32Log.TextChangedDelayed += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.fastColoredTextBoxShowPipe32Log_TextChangedDelayed);
            this.fastColoredTextBoxShowPipe32Log.VisibleRangeChangedDelayed += new System.EventHandler(this.fastColoredTextBoxShowPipe32Log_VisibleRangeChangedDelayed);
            this.fastColoredTextBoxShowPipe32Log.WordWrapNeeded += new System.EventHandler<FastColoredTextBoxNS.WordWrapNeededEventArgs>(this.fastColoredTextBoxShowPipe32Log_WordWrapNeeded);
            // 
            // groupBox12
            // 
            this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox12.Location = new System.Drawing.Point(0, 806);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(697, 221);
            this.groupBox12.TabIndex = 13;
            this.groupBox12.Values.Heading = "Cache logic";
            // 
            // numericUpDownCacheNumberOfPosters
            // 
            this.numericUpDownCacheNumberOfPosters.Location = new System.Drawing.Point(207, 7);
            this.numericUpDownCacheNumberOfPosters.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCacheNumberOfPosters.Name = "numericUpDownCacheNumberOfPosters";
            this.numericUpDownCacheNumberOfPosters.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCacheNumberOfPosters.TabIndex = 0;
            this.numericUpDownCacheNumberOfPosters.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label83
            // 
            this.label83.Location = new System.Drawing.Point(9, 3);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(113, 20);
            this.label83.TabIndex = 1;
            this.label83.Values.Text = "Number of Posters";
            // 
            // checkBoxCacheAllMetadatas
            // 
            this.checkBoxCacheAllMetadatas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCacheAllMetadatas.Location = new System.Drawing.Point(207, 29);
            this.checkBoxCacheAllMetadatas.Name = "checkBoxCacheAllMetadatas";
            this.checkBoxCacheAllMetadatas.Size = new System.Drawing.Size(259, 20);
            this.checkBoxCacheAllMetadatas.TabIndex = 1;
            this.checkBoxCacheAllMetadatas.Values.Text = "Cache All Metadatas on Application startup";
            // 
            // checkBoxCacheAllThumbnails
            // 
            this.checkBoxCacheAllThumbnails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCacheAllThumbnails.Location = new System.Drawing.Point(207, 56);
            this.checkBoxCacheAllThumbnails.Name = "checkBoxCacheAllThumbnails";
            this.checkBoxCacheAllThumbnails.Size = new System.Drawing.Size(265, 20);
            this.checkBoxCacheAllThumbnails.TabIndex = 2;
            this.checkBoxCacheAllThumbnails.Values.Text = "Cache All Thumbnails on Application startup";
            // 
            // checkBoxCacheAllWebScraperDataSets
            // 
            this.checkBoxCacheAllWebScraperDataSets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCacheAllWebScraperDataSets.Location = new System.Drawing.Point(207, 83);
            this.checkBoxCacheAllWebScraperDataSets.Name = "checkBoxCacheAllWebScraperDataSets";
            this.checkBoxCacheAllWebScraperDataSets.Size = new System.Drawing.Size(320, 20);
            this.checkBoxCacheAllWebScraperDataSets.TabIndex = 3;
            this.checkBoxCacheAllWebScraperDataSets.Values.Text = "Cache All WebScraper DataSets on Application startup";
            // 
            // checkBoxCacheFolderMetadatas
            // 
            this.checkBoxCacheFolderMetadatas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCacheFolderMetadatas.Location = new System.Drawing.Point(207, 110);
            this.checkBoxCacheFolderMetadatas.Name = "checkBoxCacheFolderMetadatas";
            this.checkBoxCacheFolderMetadatas.Size = new System.Drawing.Size(267, 20);
            this.checkBoxCacheFolderMetadatas.TabIndex = 4;
            this.checkBoxCacheFolderMetadatas.Values.Text = "Cache Folder Metadatas after folder selected";
            // 
            // checkBoxCacheFolderThumbnails
            // 
            this.checkBoxCacheFolderThumbnails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCacheFolderThumbnails.Location = new System.Drawing.Point(207, 137);
            this.checkBoxCacheFolderThumbnails.Name = "checkBoxCacheFolderThumbnails";
            this.checkBoxCacheFolderThumbnails.Size = new System.Drawing.Size(273, 20);
            this.checkBoxCacheFolderThumbnails.TabIndex = 5;
            this.checkBoxCacheFolderThumbnails.Values.Text = "Cache Folder Thumbnails after folder selected";
            // 
            // checkBoxCacheFolderWebScraperDataSets
            // 
            this.checkBoxCacheFolderWebScraperDataSets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCacheFolderWebScraperDataSets.Location = new System.Drawing.Point(207, 161);
            this.checkBoxCacheFolderWebScraperDataSets.Name = "checkBoxCacheFolderWebScraperDataSets";
            this.checkBoxCacheFolderWebScraperDataSets.Size = new System.Drawing.Size(327, 20);
            this.checkBoxCacheFolderWebScraperDataSets.TabIndex = 6;
            this.checkBoxCacheFolderWebScraperDataSets.Values.Text = "Cache Folder WebScraper DataSets after folder selected";
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox13.Location = new System.Drawing.Point(0, 1033);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(697, 245);
            this.groupBox13.TabIndex = 7;
            this.groupBox13.Values.Heading = "Application debug:";
            // 
            // checkBoxApplicationExiftoolReadShowCliWindow
            // 
            this.checkBoxApplicationExiftoolReadShowCliWindow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxApplicationExiftoolReadShowCliWindow.Location = new System.Drawing.Point(207, 3);
            this.checkBoxApplicationExiftoolReadShowCliWindow.Name = "checkBoxApplicationExiftoolReadShowCliWindow";
            this.checkBoxApplicationExiftoolReadShowCliWindow.Size = new System.Drawing.Size(304, 20);
            this.checkBoxApplicationExiftoolReadShowCliWindow.TabIndex = 0;
            this.checkBoxApplicationExiftoolReadShowCliWindow.Values.Text = "Show Exiftool read in own window CLI (Not hidden)";
            // 
            // checkBoxApplicationExiftoolWriteShowCliWindow
            // 
            this.checkBoxApplicationExiftoolWriteShowCliWindow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxApplicationExiftoolWriteShowCliWindow.Location = new System.Drawing.Point(207, 30);
            this.checkBoxApplicationExiftoolWriteShowCliWindow.Name = "checkBoxApplicationExiftoolWriteShowCliWindow";
            this.checkBoxApplicationExiftoolWriteShowCliWindow.Size = new System.Drawing.Size(309, 20);
            this.checkBoxApplicationExiftoolWriteShowCliWindow.TabIndex = 1;
            this.checkBoxApplicationExiftoolWriteShowCliWindow.Values.Text = "Show Exiftool Write in own window CLI (Not hidden)";
            // 
            // comboBoxApplicationDebugExiftoolReadThreadPrioity
            // 
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.DropDownWidth = 175;
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.FormattingEnabled = true;
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.IntegralHeight = false;
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.Items.AddRange(new object[] {
            "Idle",
            "Below Normal",
            "Normal",
            "Above Normal",
            "High",
            "RealTime"});
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.Location = new System.Drawing.Point(207, 59);
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.Name = "comboBoxApplicationDebugExiftoolReadThreadPrioity";
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.Size = new System.Drawing.Size(175, 21);
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxApplicationDebugExiftoolReadThreadPrioity.TabIndex = 2;
            // 
            // comboBoxApplicationDebugExiftoolWriteThreadPrioity
            // 
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.DropDownWidth = 175;
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.FormattingEnabled = true;
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.IntegralHeight = false;
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.Items.AddRange(new object[] {
            "Idle",
            "Below Normal",
            "Normal",
            "Above Normal",
            "High",
            "RealTime"});
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.Location = new System.Drawing.Point(207, 90);
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.Name = "comboBoxApplicationDebugExiftoolWriteThreadPrioity";
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.Size = new System.Drawing.Size(175, 21);
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxApplicationDebugExiftoolWriteThreadPrioity.TabIndex = 3;
            // 
            // comboBoxApplicationDebugBackgroundThreadPrioity
            // 
            this.comboBoxApplicationDebugBackgroundThreadPrioity.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxApplicationDebugBackgroundThreadPrioity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApplicationDebugBackgroundThreadPrioity.DropDownWidth = 175;
            this.comboBoxApplicationDebugBackgroundThreadPrioity.FormattingEnabled = true;
            this.comboBoxApplicationDebugBackgroundThreadPrioity.IntegralHeight = false;
            this.comboBoxApplicationDebugBackgroundThreadPrioity.Items.AddRange(new object[] {
            "Lowest",
            "Below Normal",
            "Normal",
            "Above Normal",
            "Highest"});
            this.comboBoxApplicationDebugBackgroundThreadPrioity.Location = new System.Drawing.Point(207, 120);
            this.comboBoxApplicationDebugBackgroundThreadPrioity.Name = "comboBoxApplicationDebugBackgroundThreadPrioity";
            this.comboBoxApplicationDebugBackgroundThreadPrioity.Size = new System.Drawing.Size(175, 21);
            this.comboBoxApplicationDebugBackgroundThreadPrioity.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.comboBoxApplicationDebugBackgroundThreadPrioity.TabIndex = 4;
            // 
            // label90
            // 
            this.label90.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label90.Location = new System.Drawing.Point(9, 59);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(127, 20);
            this.label90.TabIndex = 5;
            this.label90.Values.Text = "Exiftool Read priority:";
            // 
            // label91
            // 
            this.label91.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label91.Location = new System.Drawing.Point(9, 89);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(129, 20);
            this.label91.TabIndex = 6;
            this.label91.Values.Text = "Exiftool Write priority:";
            // 
            // label92
            // 
            this.label92.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label92.Location = new System.Drawing.Point(9, 119);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(121, 20);
            this.label92.TabIndex = 7;
            this.label92.Values.Text = "Background priority:";
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 1280);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            this.kryptonGroupBox1.Size = new System.Drawing.Size(742, 127);
            this.kryptonGroupBox1.TabIndex = 14;
            // 
            // checkBoxApplicationDarkMode
            // 
            this.checkBoxApplicationDarkMode.Location = new System.Drawing.Point(171, 65);
            this.checkBoxApplicationDarkMode.Name = "checkBoxApplicationDarkMode";
            this.checkBoxApplicationDarkMode.Size = new System.Drawing.Size(166, 20);
            this.checkBoxApplicationDarkMode.TabIndex = 8;
            this.checkBoxApplicationDarkMode.Values.Text = "Dark mode - Experimental";
            // 
            // kryptonComboBoxThemes
            // 
            this.kryptonComboBoxThemes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.kryptonComboBoxThemes.DropDownWidth = 185;
            this.kryptonComboBoxThemes.IntegralHeight = false;
            this.kryptonComboBoxThemes.Location = new System.Drawing.Point(171, 22);
            this.kryptonComboBoxThemes.Name = "kryptonComboBoxThemes";
            this.kryptonComboBoxThemes.Size = new System.Drawing.Size(185, 21);
            this.kryptonComboBoxThemes.StateCommon.ComboBox.Content.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.kryptonComboBoxThemes.TabIndex = 9;
            this.kryptonComboBoxThemes.SelectionChangeCommitted += new System.EventHandler(this.kryptonComboBoxThemes_SelectionChangeCommitted);
            // 
            // kryptonPage23
            // 
            this.kryptonPage23.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage23.Flags = 65534;
            this.kryptonPage23.LastVisibleSet = true;
            this.kryptonPage23.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage23.Name = "kryptonPage23";
            this.kryptonPage23.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage23.Text = "kryptonPage23";
            this.kryptonPage23.ToolTipTitle = "Page ToolTip";
            this.kryptonPage23.UniqueName = "ac22c9e4618d4fccb727def573932f0e";
            // 
            // kryptonPage44
            // 
            this.kryptonPage44.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage44.Flags = 65534;
            this.kryptonPage44.LastVisibleSet = true;
            this.kryptonPage44.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage44.Name = "kryptonPage44";
            this.kryptonPage44.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage44.Text = "kryptonPage44";
            this.kryptonPage44.ToolTipTitle = "Page ToolTip";
            this.kryptonPage44.UniqueName = "810aa985b2e34ea79b5fb75758bdbc09";
            // 
            // kryptonPage45
            // 
            this.kryptonPage45.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage45.Flags = 65534;
            this.kryptonPage45.LastVisibleSet = true;
            this.kryptonPage45.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage45.Name = "kryptonPage45";
            this.kryptonPage45.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage45.Text = "kryptonPage45";
            this.kryptonPage45.ToolTipTitle = "Page ToolTip";
            this.kryptonPage45.UniqueName = "f33e2c78eb4f436c8b51c02822d4d825";
            // 
            // kryptonPage47
            // 
            this.kryptonPage47.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage47.Flags = 65534;
            this.kryptonPage47.LastVisibleSet = true;
            this.kryptonPage47.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage47.Name = "kryptonPage47";
            this.kryptonPage47.Size = new System.Drawing.Size(100, 100);
            this.kryptonPage47.Text = "kryptonPage47";
            this.kryptonPage47.ToolTipTitle = "Page ToolTip";
            this.kryptonPage47.UniqueName = "3be8aa04f8f54de38ad507e79ebe42a3";
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1555, 863);
            this.Controls.Add(this.buttonConfigSave);
            this.Controls.Add(this.kryptonWorkspaceConfig);
            this.Controls.Add(this.buttonConfigCancel);
            this.Controls.Add(this.panelAvoidResizeIssues);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(616, 405);
            this.Name = "FormConfig";
            this.Text = "Config";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Config_FormClosing);
            this.Load += new System.EventHandler(this.Config_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebScrapingPageDownCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaitEventPageLoadedTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaitEventPageStartLoadingTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebScrapingDelayInPageScriptToRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebScrapingDelayOurScriptToRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWebScrapingRetry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownJavaScriptExecuteTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConfigFilenameDateFormats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxRenameVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxKeywordsAiConfidence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationAccurateIntervalNearByMediaFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationAccurateInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationGuessInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAutoKeywords)).EndInit();
            this.contextMenuStripAutoKeyword.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCameraOwner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLocationNames)).EndInit();
            this.contextMenuStripLocationNames.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMapZoomLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelBrowser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationAccuracyLongitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLocationAccuracyLatitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRegionMissmatchProcent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeopleSuggestNameTopMost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeopleSuggestNameDaysInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownApplicationMaxRowsInSearchResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationLanguages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationRegionThumbnailSizes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationThumbnailSizes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMetadataReadPriority)).EndInit();
            this.contextMenuStripMetadataRead.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMetadataWriteKeywordAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxMetadataWriteKeywordAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxMetadataWriteKeywordDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMetadataWriteKeywordDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxWriteXtraAtomVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxMetadataWriteTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxMetadataWriteStandardTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelAvoidResizeIssues)).EndInit();
            this.panelAvoidResizeIssues.ResumeLayout(false);
            this.tabControlConfig.ResumeLayout(false);
            this.tabPageApplication.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelApplication)).EndInit();
            this.tabPageConvertAndMerge.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelConvertAndMerge)).EndInit();
            this.panelConvertAndMerge.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeConvertVideo.Panel)).EndInit();
            this.groupBoxConvertAndMergeConvertVideo.Panel.ResumeLayout(false);
            this.groupBoxConvertAndMergeConvertVideo.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeConvertVideo)).EndInit();
            this.groupBoxConvertAndMergeConvertVideo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConvertVideoFilesVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeMergeVideos.Panel)).EndInit();
            this.groupBoxConvertAndMergeMergeVideos.Panel.ResumeLayout(false);
            this.groupBoxConvertAndMergeMergeVideos.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeMergeVideos)).EndInit();
            this.groupBoxConvertAndMergeMergeVideos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConcatVideosArguFileVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConcatVideoFilesVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConcatVideoArguFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConcatVideoArgument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeCommand.Panel)).EndInit();
            this.groupBoxConvertAndMergeCommand.Panel.ResumeLayout(false);
            this.groupBoxConvertAndMergeCommand.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeCommand)).EndInit();
            this.groupBoxConvertAndMergeCommand.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeTempfileExtension)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeOutputSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownConvertAndMergeImageDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeArgumentFileVariables.Panel)).EndInit();
            this.groupBoxConvertAndMergeArgumentFileVariables.Panel.ResumeLayout(false);
            this.groupBoxConvertAndMergeArgumentFileVariables.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxConvertAndMergeArgumentFileVariables)).EndInit();
            this.groupBoxConvertAndMergeArgumentFileVariables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceConfig)).EndInit();
            this.kryptonWorkspaceConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplication)).EndInit();
            this.kryptonPageApplication.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceConfigApplication)).EndInit();
            this.kryptonWorkspaceConfigApplication.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationThumbnail)).EndInit();
            this.kryptonPageApplicationThumbnail.ResumeLayout(false);
            this.kryptonPageApplicationThumbnail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell2)).EndInit();
            this.kryptonWorkspaceCell2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationNominatim)).EndInit();
            this.kryptonPageApplicationNominatim.ResumeLayout(false);
            this.kryptonPageApplicationNominatim.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationSearch)).EndInit();
            this.kryptonPageApplicationSearch.ResumeLayout(false);
            this.kryptonPageApplicationSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationRegionSuggestion)).EndInit();
            this.kryptonPageApplicationRegionSuggestion.ResumeLayout(false);
            this.kryptonPageApplicationRegionSuggestion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageRegionAccuracy)).EndInit();
            this.kryptonPageRegionAccuracy.ResumeLayout(false);
            this.kryptonPageRegionAccuracy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationCloudAndVirtualFiles)).EndInit();
            this.kryptonPageApplicationCloudAndVirtualFiles.ResumeLayout(false);
            this.kryptonPageApplicationCloudAndVirtualFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationGPSLocationAccuracy)).EndInit();
            this.kryptonPageApplicationGPSLocationAccuracy.ResumeLayout(false);
            this.kryptonPageApplicationGPSLocationAccuracy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp)).EndInit();
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.ResumeLayout(false);
            this.kryptonPageAppliactionDateTimeFormatsInFilenamesHelp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageApplicationDateAndTimeInFilenames)).EndInit();
            this.kryptonPageApplicationDateAndTimeInFilenames.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell1)).EndInit();
            this.kryptonWorkspaceCell1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadata)).EndInit();
            this.kryptonPageMetadata.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceConfigMetadata)).EndInit();
            this.kryptonWorkspaceConfigMetadata.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataReadHelp)).EndInit();
            this.kryptonPageMetadataReadHelp.ResumeLayout(false);
            this.kryptonPageMetadataReadHelp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell3)).EndInit();
            this.kryptonWorkspaceCell3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataReadPriority)).EndInit();
            this.kryptonPageMetadataReadPriority.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataWriteHelp)).EndInit();
            this.kryptonPageMetadataWriteHelp.ResumeLayout(false);
            this.kryptonPageMetadataWriteHelp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataWriteWindowsXtraProperties)).EndInit();
            this.kryptonPageMetadataWriteWindowsXtraProperties.ResumeLayout(false);
            this.kryptonPageMetadataWriteWindowsXtraProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataWriteFileAttributeDateTimeCreated)).EndInit();
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.ResumeLayout(false);
            this.kryptonPageMetadataWriteFileAttributeDateTimeCreated.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataExiftoolHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataExiftoolForEachNewKeyword)).EndInit();
            this.kryptonPageMetadataExiftoolForEachNewKeyword.ResumeLayout(false);
            this.kryptonPageMetadataExiftoolForEachNewKeyword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataExiftoolForEachDeletedKeyword)).EndInit();
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.ResumeLayout(false);
            this.kryptonPageMetadataExiftoolForEachDeletedKeyword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMetadataExiftoolForEachKeyword)).EndInit();
            this.kryptonPageMetadataExiftoolForEachKeyword.ResumeLayout(false);
            this.kryptonPageMetadataExiftoolForEachKeyword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageWebScraper)).EndInit();
            this.kryptonPageWebScraper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceWebScraper)).EndInit();
            this.kryptonWorkspaceWebScraper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageWebScraperWebScrapingSettings)).EndInit();
            this.kryptonPageWebScraperWebScrapingSettings.ResumeLayout(false);
            this.kryptonPageWebScraperWebScrapingSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell4)).EndInit();
            this.kryptonWorkspaceCell4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageWebScraperWebScrapingStartPages)).EndInit();
            this.kryptonPageWebScraperWebScrapingStartPages.ResumeLayout(false);
            this.kryptonPageWebScraperWebScrapingStartPages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrect)).EndInit();
            this.kryptonPageAutoCorrect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceAutoCorrect)).EndInit();
            this.kryptonWorkspaceAutoCorrect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectAutoKeywords)).EndInit();
            this.kryptonPageAutoCorrectAutoKeywords.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell5)).EndInit();
            this.kryptonWorkspaceCell5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectAutoCorrectHelp)).EndInit();
            this.kryptonPageAutoCorrectAutoCorrectHelp.ResumeLayout(false);
            this.kryptonPageAutoCorrectAutoCorrectHelp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectDateAndTimeDigitized)).EndInit();
            this.kryptonPageAutoCorrectDateAndTimeDigitized.ResumeLayout(false);
            this.kryptonPageAutoCorrectDateAndTimeDigitized.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectGPSLocationAndDateTime)).EndInit();
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.ResumeLayout(false);
            this.kryptonPageAutoCorrectGPSLocationAndDateTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectLocationInformation)).EndInit();
            this.kryptonPageAutoCorrectLocationInformation.ResumeLayout(false);
            this.kryptonPageAutoCorrectLocationInformation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectTitle)).EndInit();
            this.kryptonPageAutoCorrectTitle.ResumeLayout(false);
            this.kryptonPageAutoCorrectTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectAlbum)).EndInit();
            this.kryptonPageAutoCorrectAlbum.ResumeLayout(false);
            this.kryptonPageAutoCorrectAlbum.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectAuthor)).EndInit();
            this.kryptonPageAutoCorrectAuthor.ResumeLayout(false);
            this.kryptonPageAutoCorrectAuthor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectKeywordTags)).EndInit();
            this.kryptonPageAutoCorrectKeywordTags.ResumeLayout(false);
            this.kryptonPageAutoCorrectKeywordTags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectBackupOfTags)).EndInit();
            this.kryptonPageAutoCorrectBackupOfTags.ResumeLayout(false);
            this.kryptonPageAutoCorrectBackupOfTags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectFaceRegionFields)).EndInit();
            this.kryptonPageAutoCorrectFaceRegionFields.ResumeLayout(false);
            this.kryptonPageAutoCorrectFaceRegionFields.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectRename)).EndInit();
            this.kryptonPageAutoCorrectRename.ResumeLayout(false);
            this.kryptonPageAutoCorrectRename.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageAutoCorrectKeywordsHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLocation)).EndInit();
            this.kryptonPageLocation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceLocation)).EndInit();
            this.kryptonWorkspaceLocation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage42)).EndInit();
            this.kryptonPage42.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceLocationLocationNames)).EndInit();
            this.kryptonWorkspaceLocationLocationNames.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLocationLocationNameNames)).EndInit();
            this.kryptonPageLocationLocationNameNames.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellLocationLocationNameNames)).EndInit();
            this.kryptonWorkspaceCellLocationLocationNameNames.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellLocationLocationNameMap)).EndInit();
            this.kryptonWorkspaceCellLocationLocationNameMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLocationLocationNameMap)).EndInit();
            this.kryptonPageLocationLocationNameMap.ResumeLayout(false);
            this.kryptonPageLocationLocationNameMap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCell6)).EndInit();
            this.kryptonWorkspaceCell6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage38)).EndInit();
            this.kryptonPage38.ResumeLayout(false);
            this.kryptonPage38.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage39)).EndInit();
            this.kryptonPage39.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage41)).EndInit();
            this.kryptonPage41.ResumeLayout(false);
            this.kryptonPage41.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage40)).EndInit();
            this.kryptonPage40.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageConvertAndMerge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChromecast)).EndInit();
            this.kryptonPageChromecast.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceChromecast)).EndInit();
            this.kryptonWorkspaceChromecast.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChromecastVLCstreamConfig)).EndInit();
            this.kryptonPageChromecastVLCstreamConfig.ResumeLayout(false);
            this.kryptonPageChromecastVLCstreamConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastVideoCodec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastUrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastAgruments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastAudioCodec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellChromecast)).EndInit();
            this.kryptonWorkspaceCellChromecast.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChromecastImage)).EndInit();
            this.kryptonPageChromecastImage.ResumeLayout(false);
            this.kryptonPageChromecastImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastImageFormat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastImageResolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageChromecastVideo)).EndInit();
            this.kryptonPageChromecastVideo.ResumeLayout(false);
            this.kryptonPageChromecastVideo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxChromecastVideoTransporter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLog)).EndInit();
            this.kryptonPageLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceLog)).EndInit();
            this.kryptonWorkspaceLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLogApplication)).EndInit();
            this.kryptonPageLogApplication.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxShowLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonWorkspaceCellLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageLogPipe)).EndInit();
            this.kryptonPageLogPipe.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBoxShowPipe32Log)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox12.Panel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox12)).EndInit();
            this.groupBox12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCacheNumberOfPosters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox13.Panel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox13)).EndInit();
            this.groupBox13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationDebugExiftoolReadThreadPrioity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationDebugExiftoolWriteThreadPrioity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxApplicationDebugBackgroundThreadPrioity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBoxThemes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage44)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage45)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage47)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewMetadataReadPriority;
        private Krypton.Toolkit.KryptonButton buttonConfigSave;
        private Krypton.Toolkit.KryptonButton buttonConfigCancel;
        private Krypton.Toolkit.KryptonTextBox textBoxMetadataWriteHelpText;
        private Krypton.Toolkit.KryptonTextBox textBoxApplicationDateTimeFormatHelp;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMetadataRead;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadMove;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadRedo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadReplace;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMetadataReadShowFavorite;
        private Krypton.Toolkit.KryptonTextBox textBoxHelpAutoCorrect;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordsAddMicrosoftPhotos;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordsAddWindowsMediaPhotoGallery;
        private Krypton.Toolkit.KryptonCheckBox checkBoxFaceRegionAddMicrosoftPhotos;
        private Krypton.Toolkit.KryptonCheckBox checkBoxFaceRegionAddWindowsMediaPhotoGallery;
        private Krypton.Toolkit.KryptonLabel label4;
        private Krypton.Toolkit.KryptonComboBox comboBoxKeywordsAiConfidence;
        private Krypton.Toolkit.KryptonTextBox textBoxRenameTo;
        private Krypton.Toolkit.KryptonLabel label10;
        private Krypton.Toolkit.KryptonCheckBox checkBoxRename;
        private Krypton.Toolkit.KryptonLabel label9;
        private Krypton.Toolkit.KryptonComboBox comboBoxRenameVariables;
        private Krypton.Toolkit.KryptonLabel labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep4;
        private Krypton.Toolkit.KryptonLabel labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep3;
        private Krypton.Toolkit.KryptonLabel labelLocationTimeZoneGuess;
        private Krypton.Toolkit.KryptonCheckBox checkBoxGPSUpdateDateTime;
        private Krypton.Toolkit.KryptonCheckBox checkBoxGPSUpdateLocation;
        private Krypton.Toolkit.KryptonRadioButton radioButtonAuthorChangeWhenEmpty;
        private Krypton.Toolkit.KryptonRadioButton radioButtonAuthorDoNotChange;
        private Krypton.Toolkit.KryptonRadioButton radioButtonAuthorAlwaysChange;
        private Krypton.Toolkit.KryptonLabel label13;
        private Krypton.Toolkit.KryptonLabel labelAutoCorrectLocationInformationDescription;
        private Krypton.Toolkit.KryptonRadioButton radioButtonLocationNameChangeAlways;
        private Krypton.Toolkit.KryptonRadioButton radioButtonLocationNameChangeWhenEmpty;
        private Krypton.Toolkit.KryptonRadioButton radioButtonLocationNameDoNotChange;
        private Krypton.Toolkit.KryptonLabel label15;
        private Krypton.Toolkit.KryptonLabel label19;
        private ImageListViewOrder imageListViewOrderAlbum;
        private Krypton.Toolkit.KryptonRadioButton radioButtonAlbumUseFirst;
        private Krypton.Toolkit.KryptonRadioButton radioButtonAlbumChangeWhenEmpty;
        private Krypton.Toolkit.KryptonRadioButton radioButtonAlbumDoNotChange;
        private Krypton.Toolkit.KryptonLabel labelAutoCorrectTitlePrioritySource;
        private ImageListViewOrder imageListViewOrderTitle;
        private Krypton.Toolkit.KryptonRadioButton radioButtonTitleUseFirst;
        private Krypton.Toolkit.KryptonRadioButton radioButtonTitleChangeWhenEmpty;
        private Krypton.Toolkit.KryptonRadioButton radioButtonTitleDoNotChange;
        private Krypton.Toolkit.KryptonLabel labelAutoCorrectPrioritySourceOrder;
        private Krypton.Toolkit.KryptonRadioButton radioButtonDateTakenUseFirst;
        private Krypton.Toolkit.KryptonRadioButton radioButtonDateTakenChangeWhenEmpty;
        private Krypton.Toolkit.KryptonRadioButton radioButtonDateTakenDoNotChange;
        private ImageListViewOrder imageListViewOrderDateTaken;
        private Krypton.Toolkit.KryptonLabel labelAutoCorrectGPSLocationAndGPSDateTimeGPSStep1;
        private Krypton.Toolkit.KryptonLabel labelAutoCorrectGPSLocationAndGPSDateTimeGPSLocationMissing;
        private Krypton.Toolkit.KryptonCheckBox checkBoxUpdateLocationCountry;
        private Krypton.Toolkit.KryptonCheckBox checkBoxUpdateLocationState;
        private Krypton.Toolkit.KryptonCheckBox checkBoxUpdateLocationCity;
        private Krypton.Toolkit.KryptonCheckBox checkBoxUpdateLocationName;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupLocationName;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupRegionFaceNames;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupGPSDateTimeUTCAfter;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupDateTakenAfter;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupGPSDateTimeUTCBefore;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupDateTakenBefore;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupLocationCountry;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupLocationState;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupLocationCity;
        private Krypton.Toolkit.KryptonLabel labelAutoCorrectBackupOfTags;
        private Krypton.Toolkit.KryptonLabel label17;
        private Krypton.Toolkit.KryptonLabel labelLocationTimeZoneAccurate;
        private Krypton.Toolkit.KryptonLabel labelAutoCorrectGPSLocationAndGPSDateTimeGPSDateTimeMissing;
        private Krypton.Toolkit.KryptonLabel label23;
        private Krypton.Toolkit.KryptonLabel label22;
        private System.Windows.Forms.NumericUpDown numericUpDownLocationAccurateInterval;
        private System.Windows.Forms.NumericUpDown numericUpDownLocationGuessInterval;
        private Krypton.Toolkit.KryptonComboBox comboBoxMetadataWriteStandardTags;
        private Krypton.Toolkit.KryptonLabel labelMetadataForeachKeyword;
        private Krypton.Toolkit.KryptonComboBox comboBoxApplicationThumbnailSizes;
        private Krypton.Toolkit.KryptonLabel labelApplicationPosterThumbnailSize;
        private Krypton.Toolkit.KryptonLabel labelApplicationPreferredLanguages;
        private Krypton.Toolkit.KryptonTextBox textBoxApplicationPreferredLanguages;
        private Krypton.Toolkit.KryptonComboBox comboBoxApplicationLanguages;
        private Krypton.Toolkit.KryptonLabel labelApplicationNominatimTitle;
        private Krypton.Toolkit.KryptonLabel labelApplicationNominatimPreferredLanguagesHelp;
        private Krypton.Toolkit.KryptonLabel labelApplicationThumbnailSizeHelp;
        private Krypton.Toolkit.KryptonLabel labelMetadataForeachDeletedKeyword;
        private Krypton.Toolkit.KryptonLabel labelMetadataForeachNewKeyword;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomKeywordsVideo;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomCategoriesVideo;
        private Krypton.Toolkit.KryptonLabel label29;
        private Krypton.Toolkit.KryptonLabel label28;
        private Krypton.Toolkit.KryptonLabel label30;
        private Krypton.Toolkit.KryptonLabel label32;
        private Krypton.Toolkit.KryptonLabel label31;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomCommentVideo;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomSubjectVideo;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomSubtitleVideo;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomAlbumVideo;
        private Krypton.Toolkit.KryptonLabel label33;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomCommentPicture;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomSubjectPicture;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomRatingVideo;
        private Krypton.Toolkit.KryptonLabel label34;
        private Krypton.Toolkit.KryptonLabel labelMetadataWriteOnVideoAndPictureFilesVariables;
        private Krypton.Toolkit.KryptonComboBox comboBoxWriteXtraAtomVariables;
        private Krypton.Toolkit.KryptonTextBox textBoxWriteXtraAtomComment;
        private Krypton.Toolkit.KryptonTextBox textBoxWriteXtraAtomSubject;
        private Krypton.Toolkit.KryptonTextBox textBoxWriteXtraAtomSubtitle;
        private Krypton.Toolkit.KryptonTextBox textBoxWriteXtraAtomAlbum;
        private Krypton.Toolkit.KryptonTextBox textBoxWriteXtraAtomCategories;
        private Krypton.Toolkit.KryptonTextBox textBoxWriteXtraAtomKeywords;
        private Krypton.Toolkit.KryptonLabel labelMetadataWriteOnVideoAndPictureFiles;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomRatingPicture;
        private Krypton.Toolkit.KryptonTextBox textBoxWriteXtraAtomArtist;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteXtraAtomArtistVideo;
        private Krypton.Toolkit.KryptonLabel label37;
        private Krypton.Toolkit.KryptonComboBox comboBoxMetadataWriteKeywordDelete;
        private Krypton.Toolkit.KryptonPanel panelAvoidResizeIssues;
        private System.Windows.Forms.NumericUpDown numericUpDownApplicationMaxRowsInSearchResult;
        private Krypton.Toolkit.KryptonLabel labelApplicationSearch;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxConfigFilenameDateFormats;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxMetadataWriteKeywordDelete;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxMetadataWriteKeywordAdd;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxMetadataWriteTags;
        private Krypton.Toolkit.KryptonComboBox comboBoxMetadataWriteKeywordAdd;
        private Krypton.Toolkit.KryptonLabel labelApplicationNumberOfMostCommonDescription;
        private Krypton.Toolkit.KryptonLabel labelApplicationNumberOfDaysDescription;
        private Krypton.Toolkit.KryptonLabel labelApplicationNumberOfMostCommon;
        private Krypton.Toolkit.KryptonLabel labelApplicationNumberOfDays;
        private System.Windows.Forms.NumericUpDown numericUpDownPeopleSuggestNameTopMost;
        private System.Windows.Forms.NumericUpDown numericUpDownPeopleSuggestNameDaysInterval;
        private Krypton.Toolkit.KryptonLabel labelApplicationRegionAccuracyHelp;
        private Krypton.Toolkit.KryptonLabel labelApplicationRegionAccuracy;
        private System.Windows.Forms.NumericUpDown numericUpDownRegionMissmatchProcent;
        private Krypton.Toolkit.KryptonCheckBox checkBoxApplicationAvoidReadMediaFromCloud;
        private Krypton.Toolkit.KryptonCheckBox checkBoxApplicationImageListViewCacheModeOnDemand;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteFileAttributeCreatedDate;
        private System.Windows.Forms.DataGridView dataGridViewCameraOwner;
        private Krypton.Toolkit.KryptonTextBox textBox3;
        private Krypton.Toolkit.KryptonTextBox textBox4;
        private System.Windows.Forms.DataGridView dataGridViewLocationNames;
        private Krypton.Toolkit.KryptonPanel panelBrowser;
        private Krypton.Toolkit.KryptonTextBox textBoxBrowserURL;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Krypton.Toolkit.KryptonComboBox comboBoxMapZoomLevel;
        private Krypton.Toolkit.KryptonLabel labelApplicationGPSLocationAccuracy;
        private Krypton.Toolkit.KryptonLabel label65;
        private Krypton.Toolkit.KryptonLabel label64;
        private System.Windows.Forms.NumericUpDown numericUpDownLocationAccuracyLongitude;
        private System.Windows.Forms.NumericUpDown numericUpDownLocationAccuracyLatitude;
        private Krypton.Toolkit.KryptonLabel labelApplicationRegionAccuracyDescription;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLocationNames;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapRedo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapReplace;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapShowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapHideEqual;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowCoordinateOnMap;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapReloadLocationUsingNominatim;
        private System.Windows.Forms.ToolStripMenuItem searchForNewLocationsInMediaFilesToolStripMenuItem;
        private Krypton.Toolkit.KryptonCheckBox checkBoxFaceRegionAddWebScraping;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordsAddWebScraping;
        private Krypton.Toolkit.KryptonLabel labelWebScraperJavaScriptExecuteTimeout;
        private Krypton.Toolkit.KryptonTextBox textBoxWebScrapingStartPages;
        private System.Windows.Forms.NumericUpDown numericUpDownWebScrapingPageDownCount;
        private System.Windows.Forms.NumericUpDown numericUpDownWaitEventPageLoadedTimeout;
        private System.Windows.Forms.NumericUpDown numericUpDownWaitEventPageStartLoadingTimeout;
        private System.Windows.Forms.NumericUpDown numericUpDownWebScrapingDelayInPageScriptToRun;
        private System.Windows.Forms.NumericUpDown numericUpDownWebScrapingDelayOurScriptToRun;
        private System.Windows.Forms.NumericUpDown numericUpDownWebScrapingRetry;
        private System.Windows.Forms.NumericUpDown numericUpDownJavaScriptExecuteTimeout;
        private Krypton.Toolkit.KryptonLabel labelWebScraperNumberOfPageDown;
        private Krypton.Toolkit.KryptonLabel labelWebScraperPageLoadedTimeout;
        private Krypton.Toolkit.KryptonLabel labelWebScraperPageStartLoadingTimeout;
        private Krypton.Toolkit.KryptonLabel labelWebScraperWebScrapingDelay;
        private Krypton.Toolkit.KryptonLabel labelWebScraperWebScrapringDelay;
        private Krypton.Toolkit.KryptonLabel labelWebScraperWebScrapingRetry;
        private Krypton.Toolkit.KryptonLabel labelWebScraperJavaScriptExecuteTimeoutDescription;
        private Krypton.Toolkit.KryptonLabel labelWebScraperWebScrapingRetryDescription;
        private Krypton.Toolkit.KryptonLabel labelWebScraperWebScrapringDelayDescription;
        private Krypton.Toolkit.KryptonLabel labelWebScraperWebScrapingDelayDescription;
        private Krypton.Toolkit.KryptonLabel labelWebScraperPageStartLoadingTimeoutDescription;
        private Krypton.Toolkit.KryptonLabel labelWebScraperPageLoadedTimeoutDescription;
        private Krypton.Toolkit.KryptonLabel labelWebScraperNumberOfPageDownDescription;
        private Krypton.Toolkit.KryptonLabel labelApplicationRegionThumbnailSizeDescription;
        private Krypton.Toolkit.KryptonLabel labelApplicationPosterThumbnailSizeDescription;
        private Krypton.Toolkit.KryptonComboBox comboBoxApplicationRegionThumbnailSizes;
        private Krypton.Toolkit.KryptonLabel labelApplicationRegionThumbnailSize;
        private Krypton.Toolkit.KryptonButton buttonLocationImport;
        private Krypton.Toolkit.KryptonButton buttonLocationExport;
        private Krypton.Toolkit.KryptonCheckBox checkBoxApplicationAvoidReadExifFromCloud;
        private Krypton.Toolkit.KryptonLabel label87;
        private System.Windows.Forms.NumericUpDown numericUpDownLocationAccurateIntervalNearByMediaFile;
        private Krypton.Toolkit.KryptonCheckBox checkBoxGPSUpdateLocationNearByMedia;
        private Krypton.Toolkit.KryptonLabel labelMetadataFileCreateDateTimDiffrentDescription;
        private Krypton.Toolkit.KryptonLabel labelMetadataFileCreateDateTimDiffrent;
        private System.Windows.Forms.NumericUpDown numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted;
        private Krypton.Toolkit.KryptonCheckBox checkBoxDublicateAlbumAsDescription;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowCoordinateOnGoogleMap;
        private Krypton.Toolkit.KryptonCheckBox checkBoxAutoCorrectTrackChanges;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupFileCreatedBefore;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordBackupFileCreatedAfter;
        private System.Windows.Forms.DataGridView dataGridViewAutoKeywords;
        private Krypton.Toolkit.KryptonCheckBox checkBoxKeywordsAddAutoKeywords;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAutoKeyword;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAutoKeywordCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAutoKeywordCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAutoKeywordPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAutoKeywordDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAutoKeywordUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRedo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemReplace;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Album;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comments;
        private System.Windows.Forms.DataGridViewTextBoxColumn Keywords;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddKeywords;
        private Krypton.Toolkit.KryptonCheckBox checkBoxWriteMetadataAddAutoKeywords;
        private KryptonManager kryptonManager1;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceConfig;
        private Krypton.Navigator.KryptonPage kryptonPageApplication;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceConfigApplication;
        private Krypton.Navigator.KryptonPage kryptonPageApplicationThumbnail;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell2;
        private Krypton.Navigator.KryptonPage kryptonPageApplicationNominatim;
        private Krypton.Navigator.KryptonPage kryptonPageApplicationSearch;
        private Krypton.Navigator.KryptonPage kryptonPageRegionAccuracy;
        private Krypton.Navigator.KryptonPage kryptonPageApplicationCloudAndVirtualFiles;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell1;
        private Krypton.Navigator.KryptonPage kryptonPageMetadata;
        private Krypton.Navigator.KryptonPage kryptonPageWebScraper;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrect;
        private Krypton.Navigator.KryptonPage kryptonPageLocation;
        private Krypton.Navigator.KryptonPage kryptonPageConvertAndMerge;
        private Krypton.Navigator.KryptonPage kryptonPageChromecast;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceConfigMetadata;
        private Krypton.Navigator.KryptonPage kryptonPageMetadataExiftoolForEachNewKeyword;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell3;
        private Krypton.Navigator.KryptonPage kryptonPageMetadataReadHelp;
        private Krypton.Navigator.KryptonPage kryptonPageMetadataReadPriority;
        private Krypton.Navigator.KryptonPage kryptonPageMetadataWriteHelp;
        private Krypton.Navigator.KryptonPage kryptonPageMetadataWriteWindowsXtraProperties;
        private Krypton.Navigator.KryptonPage kryptonPageMetadataWriteFileAttributeDateTimeCreated;
        private Krypton.Navigator.KryptonPage kryptonPageMetadataExiftoolHelp;
        private Krypton.Navigator.KryptonPage kryptonPageMetadataExiftoolForEachDeletedKeyword;
        private Krypton.Navigator.KryptonPage kryptonPageMetadataExiftoolForEachKeyword;
        private Krypton.Navigator.KryptonPage kryptonPageApplicationGPSLocationAccuracy;
        private KryptonTextBox kryptonTextBoxMetadataReadHelpText;
        private KryptonGroupBox groupBox12;
        private System.Windows.Forms.NumericUpDown numericUpDownCacheNumberOfPosters;
        private KryptonLabel label83;
        private KryptonCheckBox checkBoxCacheAllMetadatas;
        private KryptonCheckBox checkBoxCacheAllThumbnails;
        private KryptonCheckBox checkBoxCacheAllWebScraperDataSets;
        private KryptonCheckBox checkBoxCacheFolderMetadatas;
        private KryptonCheckBox checkBoxCacheFolderThumbnails;
        private KryptonCheckBox checkBoxCacheFolderWebScraperDataSets;
        private KryptonGroupBox groupBox13;
        private KryptonCheckBox checkBoxApplicationExiftoolReadShowCliWindow;
        private KryptonCheckBox checkBoxApplicationExiftoolWriteShowCliWindow;
        private KryptonComboBox comboBoxApplicationDebugExiftoolReadThreadPrioity;
        private KryptonComboBox comboBoxApplicationDebugExiftoolWriteThreadPrioity;
        private KryptonComboBox comboBoxApplicationDebugBackgroundThreadPrioity;
        private KryptonLabel label90;
        private KryptonLabel label91;
        private KryptonLabel label92;
        private KryptonGroupBox kryptonGroupBox1;
        private KryptonCheckBox checkBoxApplicationDarkMode;
        private KryptonComboBox kryptonComboBoxThemes;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceWebScraper;
        private Krypton.Navigator.KryptonPage kryptonPageWebScraperWebScrapingStartPages;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell4;
        private Krypton.Navigator.KryptonPage kryptonPageWebScraperWebScrapingSettings;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceAutoCorrect;
        private Krypton.Navigator.KryptonPage kryptonPage23;
        private Krypton.Navigator.KryptonPage kryptonPageApplicationDateAndTimeInFilenames;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectDateAndTimeDigitized;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell5;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectGPSLocationAndDateTime;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectLocationInformation;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectTitle;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectAlbum;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectAuthor;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectKeywordTags;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectBackupOfTags;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectFaceRegionFields;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectRename;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectAutoCorrectHelp;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectAutoKeywords;
        private Krypton.Navigator.KryptonPage kryptonPageAutoCorrectKeywordsHelp;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceLocation;
        private Krypton.Navigator.KryptonPage kryptonPage40;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCell6;
        private Krypton.Navigator.KryptonPage kryptonPage38;
        private Krypton.Navigator.KryptonPage kryptonPage39;
        private Krypton.Navigator.KryptonPage kryptonPage41;
        private Krypton.Navigator.KryptonPage kryptonPage42;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceLocationLocationNames;
        private Krypton.Navigator.KryptonPage kryptonPageLocationLocationNameNames;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellLocationLocationNameNames;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellLocationLocationNameMap;
        private Krypton.Navigator.KryptonPage kryptonPageLocationLocationNameMap;
        private Krypton.Navigator.KryptonPage kryptonPage44;
        private Krypton.Navigator.KryptonPage kryptonPage45;
        private Krypton.Navigator.KryptonPage kryptonPage47;
        private System.Windows.Forms.TabControl tabControlConfig;
        private System.Windows.Forms.TabPage tabPageApplication;
        private KryptonPanel panelApplication;
        private System.Windows.Forms.TabPage tabPageConvertAndMerge;
        private KryptonPanel panelConvertAndMerge;
        private KryptonGroupBox groupBoxConvertAndMergeConvertVideo;
        private KryptonLabel label53;
        private KryptonComboBox comboBoxConvertAndMergeConvertVideoFilesVariables;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument;
        private KryptonGroupBox groupBoxConvertAndMergeMergeVideos;
        private KryptonComboBox comboBoxConvertAndMergeConcatVideosArguFileVariables;
        private KryptonComboBox comboBoxConvertAndMergeConcatVideoFilesVariables;
        private KryptonLabel label52;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxConvertAndMergeConcatVideoArguFile;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxConvertAndMergeConcatVideoArgument;
        private KryptonLabel label50;
        private KryptonGroupBox groupBoxConvertAndMergeCommand;
        private KryptonLabel label56;
        private KryptonComboBox comboBoxConvertAndMergeTempfileExtension;
        private KryptonLabel label55;
        private KryptonLabel label54;
        private KryptonComboBox comboBoxConvertAndMergeOutputSize;
        private KryptonButton buttonConvertAndMergeBrowseBackgroundMusic;
        private KryptonButton buttonConvertAndMergeBrowseFFmpeg;
        private KryptonLabel label49;
        private System.Windows.Forms.NumericUpDown numericUpDownConvertAndMergeImageDuration;
        private KryptonLabel label48;
        private KryptonTextBox textBoxConvertAndMergeBackgroundMusic;
        private KryptonLabel label47;
        private KryptonTextBox textBoxConvertAndMergeFFmpeg;
        private KryptonLabel label46;
        private KryptonGroupBox groupBoxConvertAndMergeArgumentFileVariables;
        private KryptonLabel label51;
        private KryptonComboBox comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument;
        private KryptonComboBox comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables;
        private KryptonLabel label45;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile;
        private KryptonComboBox comboBoxChromecastUrl;
        private KryptonLabel label58;
        private KryptonComboBox comboBoxChromecastVideoCodec;
        private KryptonLabel label59;
        private KryptonLabel label61;
        private KryptonComboBox comboBoxChromecastAudioCodec;
        private KryptonComboBox comboBoxChromecastAgruments;
        private KryptonLabel label60;
        private KryptonLabel label66;
        private KryptonComboBox comboBoxChromecastVideoTransporter;
        private KryptonComboBox comboBoxChromecastImageFormat;
        private KryptonLabel label63;
        private KryptonLabel label57;
        private KryptonComboBox comboBoxChromecastImageResolution;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxShowLog;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBoxShowPipe32Log;
        private Krypton.Navigator.KryptonPage kryptonPageLog;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceLog;
        private Krypton.Navigator.KryptonPage kryptonPageLogApplication;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellLog;
        private Krypton.Navigator.KryptonPage kryptonPageLogPipe;
        private Krypton.Workspace.KryptonWorkspace kryptonWorkspaceChromecast;
        private Krypton.Navigator.KryptonPage kryptonPageChromecastVLCstreamConfig;
        private Krypton.Workspace.KryptonWorkspaceCell kryptonWorkspaceCellChromecast;
        private Krypton.Navigator.KryptonPage kryptonPageChromecastImage;
        private Krypton.Navigator.KryptonPage kryptonPageChromecastVideo;
        private Krypton.Navigator.KryptonPage kryptonPageApplicationRegionSuggestion;
        private Krypton.Navigator.KryptonPage kryptonPageAppliactionDateTimeFormatsInFilenamesHelp;
    }
}