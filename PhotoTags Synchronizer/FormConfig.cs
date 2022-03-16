using CameraOwners;
using CefSharp;
using CefSharp.WinForms;
using DataGridViewGeneric;
using FastColoredTextBoxNS;
using LocationNames;
using MetadataLibrary;
using MetadataPriorityLibrary;
using Newtonsoft.Json;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileHandeling;
using System.Data;
using System.Reflection;
using PhotoTagsCommonComponets;
using Krypton.Toolkit;
using System.Xml.Linq;

namespace PhotoTagsSynchronizer
{
    public partial class FormConfig : KryptonForm
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public MetadataReadPrioity MetadataReadPrioity { get; set; }
        public CameraOwnersDatabaseCache DatabaseAndCacheCameraOwner { get; set; }
        public LocationNameDatabaseAndLookUpCache DatabaseAndCacheLocationAddress { get; set; }
        public MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }

        private readonly ChromiumWebBrowser browser;

        public Size[] ThumbnailSizes { get; set; }

        private Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionaryCopy = new Dictionary<MetadataPriorityKey, MetadataPriorityValues>();
        private AutoCorrect autoCorrect = new AutoCorrect();

        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeywordAdd = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeywordWriteTags = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArgument = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArguFile = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConcatVideoArgument = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConcatVideoArguFile = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConvertVideoArgument = null;
        private bool isPopulation = false;

        private KryptonManager kryptonManager1;
        private Manina.Windows.Forms.ImageListView imageListView1;

        public bool IsKryptonManagerChanged {get; set;} = false;

        
        #region FormConfig(KryptonManager kryptonManager, Manina.Windows.Forms.ImageListView imageListView)
        public FormConfig(KryptonManager kryptonManager, Manina.Windows.Forms.ImageListView imageListView)
        {
            InitializeComponent();

            dataGridViewMetadataReadPriority.ContextMenu = null;
            dataGridViewMetadataReadPriority.KryptonContextMenu = kryptonContextMenuMetadataRead;
            
            dataGridViewLocationNames.ContextMenu = null;
            dataGridViewLocationNames.KryptonContextMenu = kryptonContextMenuLocationNames;

            dataGridViewCameraOwner.ContextMenu = null;
            dataGridViewCameraOwner.KryptonContextMenu = kryptonContextMenuCameraOwner;

            dataGridViewAutoKeywords.ContextMenu = null;
            dataGridViewAutoKeywords.KryptonContextMenu = kryptonContextMenuAutoKeyword;

            #region CameraOwner
            this.kryptonContextMenuItemCameraOwnerCut.Click += KryptonContextMenuItemCameraOwnerCut_Click;
            this.kryptonContextMenuItemCameraOwnerCopy.Click += KryptonContextMenuItemCameraOwnerCopy_Click;
            this.kryptonContextMenuItemCameraOwnerPaste.Click += KryptonContextMenuItemCameraOwnerPaste_Click;
            this.kryptonContextMenuItemCameraOwnerDelete.Click += KryptonContextMenuItemCameraOwnerDelete_Click;
            this.kryptonContextMenuItemCameraOwnerUndo.Click += KryptonContextMenuItemCameraOwnerUndo_Click;
            this.kryptonContextMenuItemCameraOwnerRedo.Click += KryptonContextMenuItemCameraOwnerRedo_Click;
            this.kryptonContextMenuItemCameraOwnerFind.Click += KryptonContextMenuItemCameraOwnerFind_Click;
            this.kryptonContextMenuItemCameraOwnerReplace.Click += KryptonContextMenuItemCameraOwnerReplace_Click;
            //this.kryptonContextMenuSeparator1,
            this.kryptonContextMenuItemCameraOwnerMarkFavorite.Click += KryptonContextMenuItemCameraOwnerMarkFavorite_Click;
            this.kryptonContextMenuItemCameraOwnerRemoveFavorite.Click += KryptonContextMenuItemCameraOwnerRemoveFavorite_Click;
            this.kryptonContextMenuItemToggleFavorite.Click += KryptonContextMenuItemToggleFavorite_Click;
            //this.kryptonContextMenuSeparator2,
            this.kryptonContextMenuItemCameraOwnerShowOnlyFavoriteRows.Click += KryptonContextMenuItemCameraOwnerShowOnlyFavoriteRows_Click;
            #endregion

            #region AutoKeyword
            this.kryptonContextMenuItemAutoKeywordCut.Click += KryptonContextMenuItemAutoKeywordCut_Click;
            this.kryptonContextMenuItemAutoKeywordCopy.Click += KryptonContextMenuItemAutoKeywordCopy_Click;
            this.kryptonContextMenuItemAutoKeywordPaste.Click += KryptonContextMenuItemAutoKeywordPaste_Click;
            this.kryptonContextMenuItemAutoKeywordDelete.Click += KryptonContextMenuItemAutoKeywordDelete_Click;
            this.kryptonContextMenuItemAutoKeywordUndo.Click += KryptonContextMenuItemAutoKeywordUndo_Click;
            this.kryptonContextMenuItemAutoKeywordRedo.Click += KryptonContextMenuItemAutoKeywordRedo_Click;
            this.kryptonContextMenuItemAutoKeywordFind.Click += KryptonContextMenuItemAutoKeywordFind_Click;
            this.kryptonContextMenuItemAutoKeywordReplace.Click += KryptonContextMenuItemAutoKeywordReplace_Click;
            #endregion

            #region MetadataRead
            //this.kryptonContextMenuItemMetadataReadAssignToTag,
            //this.kryptonContextMenuSeparatorMetadataRead5,
            this.kryptonContextMenuItemMetadataReadCut.Click += KryptonContextMenuItemMetadataReadCut_Click;
            this.kryptonContextMenuItemMetadataReadCopy.Click += KryptonContextMenuItemMetadataReadCopy_Click;
            this.kryptonContextMenuItemMetadataReadPaste.Click += KryptonContextMenuItemMetadataReadPaste_Click;
            this.kryptonContextMenuItemMetadataReadDelete.Click += KryptonContextMenuItemMetadataReadDelete_Click;
            this.kryptonContextMenuItemMetadataReadUndo.Click += KryptonContextMenuItemMetadataReadUndo_Click;
            this.kryptonContextMenuItemMetadataReadRedo.Click += KryptonContextMenuItemMetadataReadRedo_Click;
            this.kryptonContextMenuItemMetadataReadFind.Click += KryptonContextMenuItemMetadataReadFind_Click;
            this.kryptonContextMenuItemMetadataReadReplace.Click += KryptonContextMenuItemMetadataReadReplace_Click;
            //this.kryptonContextMenuSeparatorMetadataRead1,
            this.kryptonContextMenuItemMetadataReadMarkFavorite.Click += KryptonContextMenuItemMetadataReadMarkFavorite_Click;
            this.kryptonContextMenuItemMetadataReadRemoveFavorite.Click += KryptonContextMenuItemMetadataReadRemoveFavorite_Click;
            this.kryptonContextMenuItemMetadataReadToggleFavorite.Click += KryptonContextMenuItemMetadataReadToggleFavorite_Click;
            //this.kryptonContextMenuSeparatorMetadataRead2,
            this.kryptonContextMenuItemMetadataReadShowFavorite.Click += KryptonContextMenuItemMetadataReadShowFavorite_Click;
            #endregion

            #region LocationNames
            this.kryptonContextMenuItemLocationNamesCut.Click += KryptonContextMenuItemLocationNamesCut_Click;
            this.kryptonContextMenuItemLocationNamesCopy.Click += KryptonContextMenuItemLocationNamesCopy_Click;
            this.kryptonContextMenuItemLocationNamesPaste.Click += KryptonContextMenuItemLocationNamesPaste_Click;
            this.kryptonContextMenuItemLocationNamesDelete.Click += KryptonContextMenuItemLocationNamesDelete_Click;
            this.kryptonContextMenuItemLocationNamesUndo.Click += KryptonContextMenuItemLocationNamesUndo_Click;
            this.kryptonContextMenuItemLocationNamesRedo.Click += KryptonContextMenuItemLocationNamesRedo_Click;
            this.kryptonContextMenuItemLocationNamesFind.Click += KryptonContextMenuItemLocationNamesFind_Click;
            this.kryptonContextMenuItemLocationNamesReplace.Click += KryptonContextMenuItemLocationNamesReplace_Click;
            //this.kryptonContextMenuSeparatorLocationNames3,
            this.kryptonContextMenuItemLocationNamesAddFavorite.Click += KryptonContextMenuItemLocationNamesAddFavorite_Click;
            this.kryptonContextMenuItemLocationNamesRemoveFavorite.Click += KryptonContextMenuItemLocationNamesRemoveFavorite_Click;
            this.kryptonContextMenuItemLocationNamesToggleFavorite.Click += KryptonContextMenuItemLocationNamesToggleFavorite_Click;
            //this.kryptonContextMenuSeparatorLocationNames4,
            this.kryptonContextMenuItemLocationNamesShowFavoriteRows.Click += KryptonContextMenuItemLocationNamesShowFavoriteRows_Click;
            this.kryptonContextMenuItemLocationNamesHideEqualRows.Click += KryptonContextMenuItemLocationNamesHideEqualRows_Click;
            //this.kryptonContextMenuSeparatorLocationNames6,
            this.kryptonContextMenuItemLocationNamesShowCoordinateOpenStreetMap.Click += KryptonContextMenuItemLocationNamesShowCoordinateOpenStreetMap_Click;
            this.kryptonContextMenuItemLocationNamesShowCoordinateGoogleMap.Click += KryptonContextMenuItemLocationNamesShowCoordinateGoogleMap_Click;
            this.kryptonContextMenuItemLocationNamesReloadUsingNominatim.Click += KryptonContextMenuItemLocationNamesReloadUsingNominatim_Click;
            
            this.kryptonContextMenuItemLocationNamesSearchInMediaFiles.Click += KryptonContextMenuItemLocationNamesSearchInMediaFiles_Click;
            #endregion 


            kryptonManager1 = kryptonManager;
            imageListView1 = imageListView;

            LoadPaletteSettings();
            AddDummyDataPaletteDataGridView();

            propertyGrid.SelectedObject = kryptonManager1.GlobalPalette;

            browser = new ChromiumWebBrowser("https://www.openstreetmap.org/")
            {
                Dock = DockStyle.Fill,
            };
            browser.BrowserSettings.Javascript = CefState.Enabled;
            //browser.BrowserSettings.WebSecurity = CefState.Enabled;
            browser.BrowserSettings.WebGl = CefState.Enabled;
            //browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Disabled;
            browser.BrowserSettings.Plugins = CefState.Enabled;
            this.panelBrowser.Controls.Add(this.browser);

            browser.AddressChanged += Browser_AddressChanged;

            typeof(DataGridView).InvokeMember(
                   "DoubleBuffered",
                   BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                   null,
                   dataGridViewAutoKeywords,
                   new object[] { true });

            isConfigClosing = false;
        }
        #endregion

        #region Combobox Helper

        #region Combobox - Select Best Match Combobox
        private void SelectBestMatchCombobox(KryptonComboBox comboBox, string text)
        {
            int foundItemIndex = -1;
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.GetItemText(comboBox.Items[i]).StartsWith(text))
                {
                    foundItemIndex = i;
                    break;
                }
            }

            if (foundItemIndex == -1)
            {
                comboBox.Items.Insert(0, text);
                comboBox.SelectedIndex = 0;
            }
            else
                comboBox.SelectedIndex = foundItemIndex;
        }
        #endregion

        #region Combobox - Select Best Match Combobox Reselution
        private void SelectBestMatchComboboxReselution(KryptonComboBox comboBox, int width, int height)
        {
            if (width < 1 && height < 1)
                SelectBestMatchCombobox(comboBox, "Original");
            else
                SelectBestMatchCombobox(comboBox, width + " x " + height);
        }
        #endregion

        #region Combobox - Get Combobox String Value
        private String GetComboboxValue(KryptonComboBox comboBox)
        {
            return comboBox.Text.Split(' ')[0];
        }
        #endregion

        #region Combobox - Get Combobox Int Value
        private int GetComboboxIntValue(KryptonComboBox comboBox)
        {
            if (int.TryParse(comboBox.Text.Split(' ')[0], out int result))
                return result;
            else
                return -1;
        }
        #endregion

        #region Combobox - Set Res From Combox
        private void SetResFromCombox(string value, ref int width, ref int height)
        {
            switch (value)
            {
                case "Original":
                    width = -1;
                    height = -1;
                    break;
                case "2160p:":
                    width = 3840;
                    height = 2160;
                    break;
                case "1440p:":
                    width = 2560;
                    height = 1440;
                    break;
                case "1080p:":
                    width = 1920;
                    height = 1080;
                    break;
                case "720p:":
                    width = 1280;
                    height = 720;
                    break;
                case "480p:": // 854 x 480
                    width = 854;
                    height = 480;
                    break;
                case "360p:": // 640 x 360
                    width = 640;
                    height = 360;
                    break;
                case "240p:": // 426 x 240
                    width = 426;
                    height = 240;
                    break;
            }
        }
        #endregion

        #endregion

        #region All tabs - Init - Save - Close

        #region Init

        public void Init()
        {
            DialogResult = DialogResult.Cancel;

            isPopulation = true;
            PopulateApplication();

            //Metadata Filename Date formats
            fastColoredTextBoxConfigFilenameDateFormats.Text = Properties.Settings.Default.RenameDateFormats;

            //Metadata Read
            PopulateMetadataReadToolStripMenu();
            CopyMetadataReadPrioity(MetadataReadPrioity.MetadataPrioityDictionary, metadataPrioityDictionaryCopy);
            PopulateMetadataRead(dataGridViewMetadataReadPriority);

            //WebScraping
            numericUpDownWaitEventPageLoadedTimeout.Value = Properties.Settings.Default.WaitEventPageLoadedTimeout;
            numericUpDownWaitEventPageStartLoadingTimeout.Value = Properties.Settings.Default.WaitEventPageStartLoadingTimeout;
            //textBox.Text = Properties.Settings.Default.WebScraperScript;
            textBoxWebScrapingStartPages.Text = Properties.Settings.Default.WebScraperStartPages;
            numericUpDownWebScrapingDelayInPageScriptToRun.Value = Properties.Settings.Default.WebScrapingDelayInPageScriptToRun;
            numericUpDownWebScrapingDelayOurScriptToRun.Value = Properties.Settings.Default.WebScrapingDelayOurScriptToRun;
            numericUpDownWebScrapingPageDownCount.Value = Properties.Settings.Default.WebScrapingPageDownCount;
            numericUpDownWebScrapingRetry.Value = Properties.Settings.Default.WebScrapingRetry;
            numericUpDownJavaScriptExecuteTimeout.Value = Properties.Settings.Default.JavaScriptExecuteTimeout;

            //Camera Owner 
            PopulateMetadataCameraOwner(dataGridViewCameraOwner);

            //Location Names
            PopulateMetadataLocationNames(dataGridViewLocationNames);
            isSettingDefaultComboxValuesZoomLevel = true;
            comboBoxMapZoomLevel.SelectedIndex = Properties.Settings.Default.SettingLocationZoomLevel;
            isSettingDefaultComboxValuesZoomLevel = false;

            //AutoCorrect
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridViewAutoKeywords, (KryptonPalette)kryptonManager1.GlobalPalette, "AutoKeywords", "AutoKeywords", DataGridViewSize.ConfigSize);
            autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
            PopulateAutoCorrectPoperties();


            //AutoKeywords
            LoadAutoKeywords();

            //Metadata Write
            fastColoredTextBoxHandlerKeywordAdd = new FastColoredTextBoxHandler(fastColoredTextBoxMetadataWriteKeywordAdd, true, MetadataReadPrioity.MetadataPrioityDictionary);
            fastColoredTextBoxHandlerKeywordWriteTags = new FastColoredTextBoxHandler(fastColoredTextBoxMetadataWriteTags, false, MetadataReadPrioity.MetadataPrioityDictionary);

            //Convert and Merge
            PopulateConvertAndMerge();

            PopulateMetadataWritePoperties();
            isPopulation = false;

            //Chromecast

            SelectBestMatchCombobox(comboBoxChromecastImageFormat, Properties.Settings.Default.ChromecastImageOutputFormat); //E.g. .JPEG
            SelectBestMatchComboboxReselution(comboBoxChromecastImageResolution, Properties.Settings.Default.ChromecastImageOutputResolutionWidth, Properties.Settings.Default.ChromecastImageOutputResolutionHeight);

            SelectBestMatchCombobox(comboBoxChromecastVideoTransporter, Properties.Settings.Default.ChromecastTransporter);

            comboBoxChromecastAgruments.Text = Properties.Settings.Default.ChromecastAgruments;
            comboBoxChromecastUrl.Text = Properties.Settings.Default.ChromecastUrl;
            comboBoxChromecastAudioCodec.Text = Properties.Settings.Default.ChromecastAudioCodec;
            comboBoxChromecastVideoCodec.Text = Properties.Settings.Default.ChromecastVideoCodec;


            //Show log
            ShowLogs();
        }
        #endregion 

        #region Config - Load
        private void Config_Load(object sender, EventArgs e)
        {
            dataGridViewMetadataReadPriority.Focus();
        }
        #endregion

        #region Config - Save 
        private void buttonConfigSave_Click(object sender, EventArgs e)
        {
            try
            {
                SavePaletteSettings();

                //Application
                Properties.Settings.Default.ApplicationThumbnail = ThumbnailSizes[comboBoxApplicationThumbnailSizes.SelectedIndex];
                Properties.Settings.Default.ApplicationRegionThumbnail = ThumbnailSizes[comboBoxApplicationRegionThumbnailSizes.SelectedIndex];
                
                Properties.Settings.Default.ApplicationPreferredLanguages = textBoxApplicationPreferredLanguages.Text;
                Properties.Settings.Default.MaxRowsInSearchResult = (int)numericUpDownApplicationMaxRowsInSearchResult.Value;
                Properties.Settings.Default.SuggestRegionNameNearbyDays = (int)numericUpDownPeopleSuggestNearByDaysInterval.Value;
                Properties.Settings.Default.SuggestRegionNameNearByCount = (int)numericUpDownPeopleSuggestNameNearBy.Value;
                Properties.Settings.Default.SuggestRegionNameNearByContextMenuCount = (int)numericUpDownSuggestRegionNameNearByContextMenuCount.Value;
                Properties.Settings.Default.SuggestRegionNameMostUsedContextMenuCount= (int)numericUpDownSuggestRegionNameMostUsedContextMenuCount.Value;
                Properties.Settings.Default.ApplicationSizeOfRegionNamesGroup = (int)kryptonNumericUpDownApplicationGroupSizeRenameNames.Value;

                Properties.Settings.Default.RegionMissmatchProcent = (float)numericUpDownRegionMissmatchProcent.Value;
                Properties.Settings.Default.LocationAccuracyLatitude = (float)numericUpDownLocationAccuracyLatitude.Value;
                Properties.Settings.Default.LocationAccuracyLongitude = (float)numericUpDownLocationAccuracyLongitude.Value;
                Properties.Settings.Default.AvoidOfflineMediaFiles = checkBoxApplicationAvoidReadMediaFromCloud.Checked;
                Properties.Settings.Default.AvoidReadExifFromCloud = checkBoxApplicationAvoidReadExifFromCloud.Checked;
                Properties.Settings.Default.ImageViewLoadThumbnailOnDemandMode = checkBoxApplicationImageListViewCacheModeOnDemand.Checked;
                Properties.Settings.Default.MoveToRecycleBin = kryptonCheckBoxFileMoveToRecycleBin.Checked;

                Properties.Settings.Default.CacheNumberOfPosters = (int)numericUpDownCacheNumberOfPosters.Value;
                Properties.Settings.Default.CacheAllMetadatas = checkBoxCacheAllMetadatas.Checked;
                Properties.Settings.Default.CacheAllThumbnails = checkBoxCacheAllThumbnails.Checked;
                Properties.Settings.Default.CacheAllWebScraperDataSets = checkBoxCacheAllWebScraperDataSets.Checked;
                Properties.Settings.Default.CacheFolderMetadatas = checkBoxCacheFolderMetadatas.Checked;
                Properties.Settings.Default.CacheFolderThumbnails = checkBoxCacheFolderThumbnails.Checked;
                Properties.Settings.Default.CacheFolderWebScraperDataSets = checkBoxCacheFolderWebScraperDataSets.Checked;

                Properties.Settings.Default.ExiftoolMaximumWriteBach = (int)kryptonNumericUpDownMaximumWriteBachExiftool.Value;
                //Config Debug
                Properties.Settings.Default.ApplicationDebugExiftoolReadShowCliWindow = checkBoxApplicationExiftoolReadShowCliWindow.Checked;
                Properties.Settings.Default.ApplicationDebugExiftoolWriteShowCliWindow = checkBoxApplicationExiftoolWriteShowCliWindow.Checked;
                Properties.Settings.Default.ApplicationDebugExiftoolReadThreadPrioity = ConvertIndexToProcessPriorityClass(comboBoxApplicationDebugExiftoolReadThreadPrioity.SelectedIndex);
                Properties.Settings.Default.ApplicationDebugExiftoolWriteThreadPrioity = ConvertIndexToProcessPriorityClass(comboBoxApplicationDebugExiftoolWriteThreadPrioity.SelectedIndex);
                Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity = comboBoxApplicationDebugBackgroundThreadPrioity.SelectedIndex;
                //Layout
                //Properties.Settings.Default.ApplicationDarkMode = checkBoxApplicationDarkMode.Checked;

                //AutoCorrect
                GetAutoCorrectPoperties();
                Properties.Settings.Default.AutoCorrect = autoCorrect.SerializeThis();

                //AutoKeywords
                SaveAutoKeywords();

                //WebScraping
                Properties.Settings.Default.WaitEventPageLoadedTimeout = (int)numericUpDownWaitEventPageLoadedTimeout.Value;
                Properties.Settings.Default.WaitEventPageStartLoadingTimeout = (int)numericUpDownWaitEventPageStartLoadingTimeout.Value;
                //Properties.Settings.Default.WebScraperScript = textBox.Text;
                Properties.Settings.Default.WebScraperStartPages = textBoxWebScrapingStartPages.Text;
                Properties.Settings.Default.WebScrapingDelayInPageScriptToRun = (int)numericUpDownWebScrapingDelayInPageScriptToRun.Value;
                Properties.Settings.Default.WebScrapingDelayOurScriptToRun = (int)numericUpDownWebScrapingDelayOurScriptToRun.Value;
                Properties.Settings.Default.WebScrapingPageDownCount = (int)numericUpDownWebScrapingPageDownCount.Value;
                Properties.Settings.Default.WebScrapingRetry = (int)numericUpDownWebScrapingRetry.Value;
                Properties.Settings.Default.JavaScriptExecuteTimeout = (int)numericUpDownJavaScriptExecuteTimeout.Value;

                //Metadata Write
                Properties.Settings.Default.WriteUsingCompatibilityCheck = kryptonCheckBoxWriteMetadataCompatibilityCheckAndFix.Checked;
                Properties.Settings.Default.WriteAutoKeywordsSynonyms = kryptonCheckBoxWriteAutoKeywordsSynonyms.Checked;
                Properties.Settings.Default.WriteMetadataTags = fastColoredTextBoxMetadataWriteTags.Text;
                Properties.Settings.Default.WriteMetadataKeywordAdd = fastColoredTextBoxMetadataWriteKeywordAdd.Text;

                Properties.Settings.Default.XtraAtomWriteOnFile = checkBoxWriteXtraAtomOnMediaFile.Checked;
                Properties.Settings.Default.XtraAtomAlbumVideo = checkBoxWriteXtraAtomAlbumVideo.Checked;
                Properties.Settings.Default.XtraAtomCategoriesVideo = checkBoxWriteXtraAtomCategoriesVideo.Checked;
                Properties.Settings.Default.XtraAtomCommentPicture = checkBoxWriteXtraAtomCommentPicture.Checked;
                Properties.Settings.Default.XtraAtomCommentVideo = checkBoxWriteXtraAtomCommentVideo.Checked;
                Properties.Settings.Default.XtraAtomKeywordsPicture = checkBoxWriteXtraAtomKeywordsPicture.Checked;
                Properties.Settings.Default.XtraAtomKeywordsVideo = checkBoxWriteXtraAtomKeywordsVideo.Checked;
                Properties.Settings.Default.XtraAtomRatingPicture = checkBoxWriteXtraAtomRatingPicture.Checked;
                Properties.Settings.Default.XtraAtomRatingVideo = checkBoxWriteXtraAtomRatingVideo.Checked;
                Properties.Settings.Default.WriteMetadataCreatedDateFileAttribute = checkBoxWriteFileAttributeCreatedDate.Checked;
                Properties.Settings.Default.WriteMetadataAddAutoKeywords = checkBoxWriteMetadataAddAutoKeywords.Checked;
                Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted = (int)numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted.Value;
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

                Properties.Settings.Default.MicosoftOneDriveLocationHackUse = kryptonCheckBoxMicrosoftPhotosLocationHack.Checked;
                Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix = 
                    (string.IsNullOrWhiteSpace(kryptonTextBoxMicrosoftPhotosLocationHackPostfix.Text.Trim()) ? "-GPS-" :
                    kryptonTextBoxMicrosoftPhotosLocationHackPostfix.Text);
                //Camera Owner 
                SaveMetadataCameraOwner(dataGridViewCameraOwner);

                //Location Names
                SaveMetadataLocation(dataGridViewLocationNames);

                //Filename date formates
                Properties.Settings.Default.RenameDateFormats = fastColoredTextBoxConfigFilenameDateFormats.Text;

                //Convert and Merge
                Properties.Settings.Default.ConvertAndMergeExecute = textBoxConvertAndMergeFFmpeg.Text;
                Properties.Settings.Default.ConvertAndMergeMusic = textBoxConvertAndMergeBackgroundMusic.Text;
                Properties.Settings.Default.ConvertAndMergeImageDuration = (int)numericUpDownConvertAndMergeImageDuration.Value;
                Properties.Settings.Default.ConvertAndMergeOutputTempfileExtension = comboBoxConvertAndMergeTempfileExtension.Text;

                int width = Properties.Settings.Default.ConvertAndMergeOutputWidth;
                int height = Properties.Settings.Default.ConvertAndMergeOutputHeight;
                SetResFromCombox(GetComboboxValue(comboBoxConvertAndMergeOutputSize), ref width, ref height);
                Properties.Settings.Default.ConvertAndMergeOutputWidth = width;
                Properties.Settings.Default.ConvertAndMergeOutputHeight = height;

                Properties.Settings.Default.ConvertAndMergeConcatVideosArguments = fastColoredTextBoxConvertAndMergeConcatVideoArgument.Text;
                Properties.Settings.Default.ConvertAndMergeConcatVideosArguFile = fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Text;

                Properties.Settings.Default.ConvertAndMergeConcatImagesArguments = fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Text;
                Properties.Settings.Default.ConvertAndMergeConcatImagesArguFile = fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Text;

                Properties.Settings.Default.ConvertAndMergeConvertVideosArguments = fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Text;


                //Chromecast
                width = Properties.Settings.Default.ChromecastImageOutputResolutionWidth;
                height = Properties.Settings.Default.ChromecastImageOutputResolutionHeight;
                SetResFromCombox(GetComboboxValue(comboBoxChromecastImageResolution), ref width, ref height);
                Properties.Settings.Default.ChromecastImageOutputResolutionWidth = width;
                Properties.Settings.Default.ChromecastImageOutputResolutionHeight = height;
                Properties.Settings.Default.ChromecastImageOutputFormat = GetComboboxValue(comboBoxChromecastImageFormat); //E.g. .JPEG

                Properties.Settings.Default.ChromecastTransporter = GetComboboxValue(comboBoxChromecastVideoTransporter);

                Properties.Settings.Default.ChromecastAgruments = comboBoxChromecastAgruments.Text;
                Properties.Settings.Default.ChromecastUrl = comboBoxChromecastUrl.Text;
                Properties.Settings.Default.ChromecastAudioCodec = comboBoxChromecastAudioCodec.Text;
                Properties.Settings.Default.ChromecastVideoCodec = comboBoxChromecastVideoCodec.Text;

                //Save config file
                Properties.Settings.Default.Save();

                //Metadata Read
                MetadataReadPrioity.MetadataPrioityDictionary = metadataPrioityDictionaryCopy;

                MetadataReadPrioity.WriteAlways();
            } catch (Exception ex)
            {
                KryptonMessageBox.Show("Failed to save config.\r\n\r\n" + ex.Message, "Failed to save config", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                _ = this.BeginInvoke(new Action<Exception, string>(Logger.Error), ex, "buttonConfigSave_Click failed saving config.");
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion

        #region Config - Cancel
        private void buttonConfigCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #endregion

        #region ConvertPriorityClassToIndex
        private int ConvertPriorityClassToIndex(ProcessPriorityClass processPriorityClass)
        {
            switch (processPriorityClass)
            {
                case ProcessPriorityClass.Idle: return 0;
                case ProcessPriorityClass.BelowNormal: return 1;
                case ProcessPriorityClass.Normal: return 2;
                case ProcessPriorityClass.AboveNormal: return 3;
                case ProcessPriorityClass.High: return 4;
                case ProcessPriorityClass.RealTime: return 5;
            }
            return 1;

        }
        #endregion

        #region ConvertIndexToProcessPriorityClass
        private int ConvertIndexToProcessPriorityClass(int index)
        {
            switch (index)
            {
                case 0: return 64; //Idle = 64,
                case 1: return 16384; // BelowNormal = 16384,        
                case 2: return 32; // Normal = 32,
                case 3: return 32768; // AboveNormal = 32768
                case 4: return 128; //High = 128,
                case 5: return 256; //RealTime = 256 
            }
            return 16384; // BelowNormal = 16384
        }
        #endregion 


        #region PopulateApplication()
        public void PopulateApplication()
        {
            for (int i = 0; i < ThumbnailSizes.Length; i++)
            {
                comboBoxApplicationThumbnailSizes.Items.Add(ThumbnailSizes[i].ToString());
                comboBoxApplicationRegionThumbnailSizes.Items.Add(ThumbnailSizes[i].ToString());
            }

            comboBoxApplicationThumbnailSizes.Text = Properties.Settings.Default.ApplicationThumbnail.ToString();
            comboBoxApplicationRegionThumbnailSizes.Text = Properties.Settings.Default.ApplicationRegionThumbnail.ToString();
            
            textBoxApplicationPreferredLanguages.Text = Properties.Settings.Default.ApplicationPreferredLanguages;
            numericUpDownApplicationMaxRowsInSearchResult.Value = Properties.Settings.Default.MaxRowsInSearchResult;
            
            numericUpDownPeopleSuggestNearByDaysInterval.Value = Properties.Settings.Default.SuggestRegionNameNearbyDays;
            numericUpDownPeopleSuggestNameNearBy.Value = Properties.Settings.Default.SuggestRegionNameNearByCount;
            numericUpDownSuggestRegionNameNearByContextMenuCount.Value = Properties.Settings.Default.SuggestRegionNameNearByContextMenuCount;
            numericUpDownSuggestRegionNameMostUsedContextMenuCount.Value = Properties.Settings.Default.SuggestRegionNameMostUsedContextMenuCount;
            kryptonNumericUpDownApplicationGroupSizeRenameNames.Value = Properties.Settings.Default.ApplicationSizeOfRegionNamesGroup;

            numericUpDownRegionMissmatchProcent.Value = (decimal)Properties.Settings.Default.RegionMissmatchProcent;

            numericUpDownLocationAccuracyLatitude.Value = (decimal)Properties.Settings.Default.LocationAccuracyLatitude;
            numericUpDownLocationAccuracyLongitude.Value = (decimal)Properties.Settings.Default.LocationAccuracyLongitude;

            checkBoxApplicationAvoidReadMediaFromCloud.Checked = Properties.Settings.Default.AvoidOfflineMediaFiles;
            checkBoxApplicationAvoidReadExifFromCloud.Checked = Properties.Settings.Default.AvoidReadExifFromCloud;
            checkBoxApplicationImageListViewCacheModeOnDemand.Checked = Properties.Settings.Default.ImageViewLoadThumbnailOnDemandMode;
            kryptonCheckBoxFileMoveToRecycleBin.Checked = Properties.Settings.Default.MoveToRecycleBin;
            //Cache
            numericUpDownCacheNumberOfPosters.Value = (int)Properties.Settings.Default.CacheNumberOfPosters;
            checkBoxCacheAllMetadatas.Checked = Properties.Settings.Default.CacheAllMetadatas;
            checkBoxCacheAllThumbnails.Checked = Properties.Settings.Default.CacheAllThumbnails;
            checkBoxCacheAllWebScraperDataSets.Checked = Properties.Settings.Default.CacheAllWebScraperDataSets;
            checkBoxCacheFolderMetadatas.Checked = Properties.Settings.Default.CacheFolderMetadatas;
            checkBoxCacheFolderThumbnails.Checked = Properties.Settings.Default.CacheFolderThumbnails;
            checkBoxCacheFolderWebScraperDataSets.Checked = Properties.Settings.Default.CacheFolderWebScraperDataSets;

            kryptonNumericUpDownMaximumWriteBachExiftool.Value = Properties.Settings.Default.ExiftoolMaximumWriteBach;
            //Config Debug
            checkBoxApplicationExiftoolReadShowCliWindow.Checked = Properties.Settings.Default.ApplicationDebugExiftoolReadShowCliWindow;
            checkBoxApplicationExiftoolWriteShowCliWindow.Checked = Properties.Settings.Default.ApplicationDebugExiftoolWriteShowCliWindow;
            comboBoxApplicationDebugExiftoolReadThreadPrioity.SelectedIndex = ConvertPriorityClassToIndex((ProcessPriorityClass)Properties.Settings.Default.ApplicationDebugExiftoolReadThreadPrioity);
            comboBoxApplicationDebugExiftoolWriteThreadPrioity.SelectedIndex = ConvertPriorityClassToIndex((ProcessPriorityClass)Properties.Settings.Default.ApplicationDebugExiftoolWriteThreadPrioity);
            comboBoxApplicationDebugBackgroundThreadPrioity.SelectedIndex = Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity;
            //Layout
            //checkBoxApplicationDarkMode.Checked = Properties.Settings.Default.ApplicationDarkMode;
        }
        #endregion 

        #region AutoCorrect - Populate and Save
        private void PopulateAutoCorrectListOrder(ImageListViewOrder imageListViewOrder, List<MetadataBrokerType> listPriority)
        {
            ListViewItem listViewItem;

            imageListViewOrder.Items.Clear();
            foreach (MetadataBrokerType metadataBroker in listPriority)
            {
                switch (metadataBroker)
                {
                    case MetadataBrokerType.ExifTool:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Exiftool";
                        listViewItem.Tag = MetadataBrokerType.ExifTool;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerType.MicrosoftPhotos:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "MicrosoftPhotos";
                        listViewItem.Tag = MetadataBrokerType.MicrosoftPhotos;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerType.WindowsLivePhotoGallery:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Windows Live Photo Gallery";
                        listViewItem.Tag = MetadataBrokerType.WindowsLivePhotoGallery;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerType.FileSystem:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Subfolder name";
                        listViewItem.Tag = MetadataBrokerType.FileSystem;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case MetadataBrokerType.WebScraping:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "WebScraping";
                        listViewItem.Tag = MetadataBrokerType.WebScraping;
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
                    case DateTimeSources.SmartDate:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "Smart Date";
                        listViewItem.Tag = DateTimeSources.SmartDate;
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
                    case DateTimeSources.FileCreateDate:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "File created";
                        listViewItem.Tag = DateTimeSources.FileCreateDate;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                    case DateTimeSources.FileModified:
                        listViewItem = new ListViewItem();
                        listViewItem.Text = "File modified";
                        listViewItem.Tag = DateTimeSources.FileModified;
                        imageListViewOrder.Items.Add(listViewItem);
                        break;
                }
            }
            imageListViewOrder.AutoResize();
        }

        private void PopulateAutoCorrectPoperties()
        {
            #region Date Taken
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
            PopulateAutoCorrectListOrder(imageListViewOrderAlbum, autoCorrect.AlbumPriority);

            if (autoCorrect.UpdateAlbum)
            {
                if (autoCorrect.UpdateAlbumWithFirstInPrioity)
                    radioButtonAlbumUseFirst.Checked = true;
                else
                    radioButtonAlbumChangeWhenEmpty.Checked = true;
            }
            else radioButtonAlbumDoNotChange.Checked = true;

            checkBoxDublicateAlbumAsDescription.Checked = autoCorrect.UpdateDescription;
            #endregion

            #region Keywords
            checkBoxKeywordsAddMicrosoftPhotos.Checked = autoCorrect.UseKeywordsFromMicrosoftPhotos;
            checkBoxKeywordsAddWindowsMediaPhotoGallery.Checked = autoCorrect.UseKeywordsFromWindowsLivePhotoGallery;
            comboBoxKeywordsAiConfidence.SelectedIndex = 9 - (int)(autoCorrect.KeywordTagConfidenceLevel * 10);
            checkBoxKeywordsAddWebScraping.Checked = autoCorrect.UseKeywordsFromWebScraping;
            checkBoxKeywordsAddAutoKeywords.Checked = autoCorrect.UseAutoKeywords;

            checkBoxAutoCorrectTrackChanges.Checked = autoCorrect.TrackChangesInComments;
            checkBoxKeywordBackupFileCreatedAfter.Checked = autoCorrect.BackupFileCreatedAfterUpdate;
            checkBoxKeywordBackupFileCreatedBefore.Checked = autoCorrect.BackupFileCreatedBeforeUpdate;
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
            checkBoxFaceRegionAddWebScraping.Checked = autoCorrect.UseFaceRegionFromWebScraping;
            #endregion

            #region Author
            if (!autoCorrect.UpdateAuthor) radioButtonAuthorDoNotChange.Checked = true;
            else if (autoCorrect.UpdateAuthorOnlyWhenEmpty) radioButtonAuthorChangeWhenEmpty.Checked = true;
            else radioButtonAuthorAlwaysChange.Checked = true;
            #endregion

            #region GPS Location and Date&Time
            checkBoxGPSUpdateLocation.Checked = autoCorrect.UpdateGPSLocation;
            checkBoxGPSUpdateDateTime.Checked = autoCorrect.UpdateGPSDateTime;
            checkBoxGPSUpdateLocationNearByMedia.Checked = autoCorrect.UpdateGPSLocationNearByMedia;
            kryptonCheckBoxAutoCorrectUseSmartDate.Checked = autoCorrect.UseSmartDate;

            numericUpDownLocationGuessInterval.Value = autoCorrect.LocationTimeZoneGuessHours;
            numericUpDownLocationAccurateInterval.Value = autoCorrect.LocationFindMinutes;
            numericUpDownLocationAccurateIntervalNearByMediaFile.Value = autoCorrect.LocationFindMinutesNearByMedia;
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

            #region Rename
            textBoxRenameTo.Text = autoCorrect.RenameVariable;
            checkBoxRename.Checked = autoCorrect.RenameAfterAutoCorrect;
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

            for (int index = 0; index < imageListViewOrderTitle.Items.Count; index++)
            {
                if (!autoCorrect.TitlePriority.Contains((MetadataBrokerType)imageListViewOrderTitle.Items[index].Tag)) autoCorrect.TitlePriority.Add((MetadataBrokerType)imageListViewOrderTitle.Items[index].Tag);
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
            //foreach (ListViewItem item in imageListViewOrderAlbum.Items)
            for (int index = 0; index < imageListViewOrderAlbum.Items.Count; index++)
            {
                if (!autoCorrect.AlbumPriority.Contains((MetadataBrokerType)imageListViewOrderAlbum.Items[index].Tag)) autoCorrect.AlbumPriority.Add((MetadataBrokerType)imageListViewOrderAlbum.Items[index].Tag);
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

            autoCorrect.UpdateDescription = checkBoxDublicateAlbumAsDescription.Checked;
            #endregion

            #region Keywords
            autoCorrect.UseKeywordsFromMicrosoftPhotos = checkBoxKeywordsAddMicrosoftPhotos.Checked;
            autoCorrect.UseKeywordsFromWindowsLivePhotoGallery = checkBoxKeywordsAddWindowsMediaPhotoGallery.Checked;
            autoCorrect.UseKeywordsFromWebScraping = checkBoxKeywordsAddWebScraping.Checked;
            autoCorrect.UseAutoKeywords = checkBoxKeywordsAddAutoKeywords.Checked;

            autoCorrect.TrackChangesInComments = checkBoxAutoCorrectTrackChanges.Checked;
            autoCorrect.BackupFileCreatedAfterUpdate = checkBoxKeywordBackupFileCreatedAfter.Checked;
            autoCorrect.BackupFileCreatedBeforeUpdate = checkBoxKeywordBackupFileCreatedBefore.Checked;
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
            autoCorrect.UseFaceRegionFromWebScraping = checkBoxFaceRegionAddWebScraping.Checked;
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
            autoCorrect.UpdateGPSLocationNearByMedia = checkBoxGPSUpdateLocationNearByMedia.Checked;
            autoCorrect.UseSmartDate = kryptonCheckBoxAutoCorrectUseSmartDate.Checked;
            autoCorrect.LocationTimeZoneGuessHours = (int)numericUpDownLocationGuessInterval.Value;
            autoCorrect.LocationFindMinutes = (int)numericUpDownLocationAccurateInterval.Value;
            autoCorrect.LocationFindMinutesNearByMedia = (int)numericUpDownLocationAccurateIntervalNearByMediaFile.Value;
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

            #region Rename
            autoCorrect.RenameVariable = textBoxRenameTo.Text;
            autoCorrect.RenameAfterAutoCorrect = checkBoxRename.Checked;
            #endregion
        }
        #endregion

        #region AutoCorrect - Rename
        private void comboBoxRenameVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(textBoxRenameTo, comboBoxRenameVariables.Text);
        }
        #endregion

        #region AutoKeywords - Save        
        private void SaveAutoKeywords()
        {
            try
            {
                //DataSet dataSet = (DataSet)dataGridViewAutoKeywords.DataSource;
                if (dataGridViewAutoKeywords.Rows.Count > 1)
                {
                    DataSet dataSet = AutoKeywordHandler.ReadDataGridView(dataGridViewAutoKeywords);
                    if (dataSet != null) dataSet.WriteXml(FileHandler.GetLocalApplicationDataPath("AutoKeywords.xml", false, this));
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "AutoKeywords failed to saved", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                _ = this.BeginInvoke(new Action<Exception>(Logger.Error), ex); 
            }
        }
        #endregion

        #region AutoKeyword - Load
        void LoadAutoKeywords()
        {
            try
            {
                DataSet dataSet = AutoKeywordHandler.ReadDataSetFromXML();
                AutoKeywordHandler.PopulateDataGridView(dataGridViewAutoKeywords, dataSet);
                DataGridViewHandler.SetIsAgregated(dataGridViewAutoKeywords, true);
                DataGridViewHandler.FastAutoSizeRowsHeight(dataGridViewAutoKeywords, 0);
            } catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "AutoKeywords failed to saved", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                _ = this.BeginInvoke(new Action<Exception, string>(Logger.Error), ex, "LoadAutoKeywords");
            }
        }
        #endregion

        #region Camera owner 

        private int columnIndexOwner = 0;
        #region Camera owner - PopulateMetadataCameraOwner
        private void PopulateMetadataCameraOwner(DataGridView dataGridView)
        {
            isCellValueUpdating = true;
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridView, (KryptonPalette)kryptonManager1.GlobalPalette, "CameraMakeModelOwner", "Camera Make/Model", DataGridViewSize.ConfigSize);
            DataGridViewHandler.Clear(dataGridView, DataGridViewSize.ConfigSize);
            DataGridViewHandler.SetIsAgregated(dataGridView, true);

            DateTime dateTimeEditable = DateTime.Now;

            columnIndexOwner = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Owner", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out FileEntryVersionCompare fileEntryVersionCompareReason);

            List<CameraOwner> cameraOwners = DatabaseAndCacheCameraOwner.ReadCameraMakeModelAndOwners();
            DatabaseAndCacheCameraOwner.ReadCameraMakeModelAndOwnersThatNotExist(cameraOwners); //Add this to Thread

            foreach (CameraOwner cameraOwner in cameraOwners)
            {
                DataGridViewHandler.AddRow(dataGridView, columnIndexOwner, 
                    new DataGridViewGenericRow(cameraOwner.Make),
                    cameraOwner.Owner, false, false);

                int rowIndex = DataGridViewHandler.AddRow(dataGridView, columnIndexOwner, 
                    new DataGridViewGenericRow(cameraOwner.Make, cameraOwner.Model),
                    cameraOwner.Owner, false, false);
            }
            isCellValueUpdating = false;
        }
        #endregion

        #region Camera owner - SaveMetadataCameraOwner
        private void SaveMetadataCameraOwner(DataGridView dataGridView)
        {
            int rowCount = DataGridViewHandler.GetRowCount(dataGridView);
            for (int row = 0; row < rowCount; row++) 
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, row);
                if (!dataGridViewGenericRow.IsHeader)
                {
                    string camerMakeModelOwner = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexOwner, row);
                    if (camerMakeModelOwner == CameraOwnersDatabaseCache.MissingLocationsOwners) camerMakeModelOwner = null;
                    CameraOwner cameraOwner = new CameraOwner(dataGridViewGenericRow.HeaderName, dataGridViewGenericRow.RowName, camerMakeModelOwner);
                    DatabaseAndCacheCameraOwner.SaveCameraMakeModelAndOwner(cameraOwner);
                }
            }
            DatabaseAndCacheCameraOwner.CameraMakeModelAndOwnerMakeDirty();
        }
        #endregion 

        #region Camera owner - CellBeginEdit
        private void dataGridViewCameraOwner_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
        }

        private void dataGridViewCameraOwner_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion

        #region Camera owner - CellPainting
        private void dataGridViewCameraOwner_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
            //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Camera owner - GetCameraOwnerAutoComplete 
        AutoCompleteStringCollection autoCompleteStringCollectionCameraOwner = null;
        public AutoCompleteStringCollection GetCameraOwnerAutoComplete()
        {
            if (autoCompleteStringCollectionCameraOwner == null) 
            {
                List<string> cameraOwners = DatabaseAndCacheCameraOwner.ReadCameraOwners();
                AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
                foreach (string cameraOwner in cameraOwners)
                {
                    if (!string.IsNullOrWhiteSpace(cameraOwner)) autoCompleteStringCollection.Add(cameraOwner);
                }
                autoCompleteStringCollectionCameraOwner = autoCompleteStringCollection;
            }
            return autoCompleteStringCollectionCameraOwner;
        }

        public void AddCameraOwnerAutoComplete(string autoCompleteName)
        {
            AutoCompleteStringCollection autoCompleteStringCollection = GetCameraOwnerAutoComplete();            
            if (!string.IsNullOrWhiteSpace(autoCompleteName) && !autoCompleteStringCollection.Contains(autoCompleteName)) autoCompleteStringCollection.Add(autoCompleteName);
        }
        #endregion

        #region Camera owner - EditingControlShowing
        private void dataGridViewCameraOwner_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            TextBox prodCode = e.Control as TextBox;
            if (prodCode != null)
            {
                prodCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                prodCode.AutoCompleteCustomSource = GetCameraOwnerAutoComplete();
                prodCode.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
        }
        #endregion

        #region Camera owner - CellValidating
        private void dataGridViewCameraOwner_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            try
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
                if (!dataGridViewGenericRow.IsHeader)
                {
                    AddCameraOwnerAutoComplete((string)e.FormattedValue);
                    
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        #endregion

        #endregion

        #region Location names
        bool isSettingDefaultComboxValuesZoomLevel = false;
        LocationCoordinate locationCoordinateRememberForZooming = null;
        private Dictionary<LocationCoordinate, LocationDescription> locationNames = new Dictionary<LocationCoordinate, LocationDescription>();
        private int columnIndexName = 0;
        private int columnIndexCity = 1;
        private int columnIndexRegion = 2;
        private int columnIndexCountry = 3;

        #region Location names - PopulateMetadataLocationNames
        private void PopulateMetadataLocationNames(DataGridView dataGridView, Dictionary<LocationCoordinate, LocationDescription> locationNames)
        {
            foreach (KeyValuePair<LocationCoordinate, LocationDescription> locationKeyValuePair in locationNames)
            {
                string group = 
                    (locationKeyValuePair.Value.Country == null ? "(Country)" : locationKeyValuePair.Value.Country) + " \\ "+
                    (locationKeyValuePair.Value.City == null ? "(City)" : locationKeyValuePair.Value.City);
                string rowId = locationKeyValuePair.Key.ToString();

                DataGridViewHandler.AddRow(dataGridView, 0,
                    new DataGridViewGenericRow(group),
                    null, false, true);

                DataGridViewHandler.AddRow(dataGridView, columnIndexName,
                    new DataGridViewGenericRow(group, rowId, locationKeyValuePair.Key),
                    locationKeyValuePair.Value.Name, false, true);

                DataGridViewHandler.AddRow(dataGridView, columnIndexCity,
                    new DataGridViewGenericRow(group, rowId, locationKeyValuePair.Key),
                    locationKeyValuePair.Value.City, false, true);

                DataGridViewHandler.AddRow(dataGridView, columnIndexRegion,
                    new DataGridViewGenericRow(group, rowId, locationKeyValuePair.Key),
                    locationKeyValuePair.Value.Region, false, true);

                DataGridViewHandler.AddRow(dataGridView, columnIndexCountry,
                    new DataGridViewGenericRow(group, rowId, locationKeyValuePair.Key),
                    locationKeyValuePair.Value.Country, false, true);
            }

            isCellValueUpdating = false;
        }
        #endregion 

        #region Location names - PopulateMetadataLocationNames
        private void PopulateMetadataLocationNames(DataGridView dataGridView)
        {
            isCellValueUpdating = true;
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridView, (KryptonPalette)kryptonManager1.GlobalPalette, "LocationNames", "Location names", DataGridViewSize.ConfigSize);
            DataGridViewHandler.Clear(dataGridView, DataGridViewSize.ConfigSize);
            DataGridViewHandler.SetIsAgregated(dataGridView, true);

            DateTime dateTimeEditable = DateTime.Now;

            columnIndexName = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Name", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

            columnIndexCity = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("City", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

            columnIndexRegion = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Region", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

            columnIndexCountry = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Country", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

            locationNames = DatabaseAndCacheLocationAddress.ReadAllLocationsData();
            PopulateMetadataLocationNames(dataGridView, locationNames);
        }
        #endregion

        #region Location names - LocationRecord
        private class LocationRecord
        {
            public LocationRecord(LocationCoordinate locationCoordinate, LocationDescription locationDescription)
            {
                LocationCoordinate = locationCoordinate;
                LocationDescription = locationDescription;
            }

            public LocationCoordinate LocationCoordinate { get; set; } = new LocationCoordinate();
            public LocationDescription LocationDescription { get; set; } = new LocationDescription();
        }
        #endregion

        #region Location names - LocationExport Click
        private void buttonLocationExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                //saveFileDialog1.InitialDirectory = @ "C:\";      
                saveFileDialog1.Title = "Save locations as JSON";
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = false;
                saveFileDialog1.DefaultExt = "json";
                saveFileDialog1.Filter = "JSON files (*.json)|*.json";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    DataGridView dataGridView = dataGridViewLocationNames;

                    int rowCount = DataGridViewHandler.GetRowCount(dataGridView);
                    List<LocationRecord> locationRecords = new List<LocationRecord>();

                    for (int row = 0; row < rowCount; row++)
                    {
                        DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, row);
                        if (!dataGridViewGenericRow.IsHeader)
                        {
                            LocationCoordinate locationCoordinate = dataGridViewGenericRow.LocationCoordinate;
                            LocationDescription locationDescription = new LocationDescription(
                                DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexName, row),
                                DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexCity, row),
                                DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexRegion, row),
                                DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexCountry, row));

                            locationRecords.Add(new LocationRecord(locationCoordinate, locationDescription));
                        }
                        
                    }
                    string output = JsonConvert.SerializeObject(locationRecords);
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, output, Encoding.UTF8);

                    KryptonMessageBox.Show(locationRecords.Count.ToString() + " locations exported", "Location file exported", MessageBoxButtons.OK, MessageBoxIcon.Information, showCtrlCopy: true);
                }
            } catch (Exception ex)
            {
                KryptonMessageBox.Show("Error saving JSON file!\r\n\r\n" + ex.Message, "Was not able to save JSON file", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Location names - LocationImport_Click
        private void buttonLocationImport_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;

            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    Title = "Browse JSON Files",
                    CheckFileExists = true,
                    CheckPathExists = true,

                    DefaultExt = "json",
                    Filter = "json files (*.json)|*.json",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    ReadOnlyChecked = false,
                    ShowReadOnly = true
                };

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string input = System.IO.File.ReadAllText(openFileDialog1.FileName, Encoding.UTF8);
                    List<LocationRecord> readResult = JsonConvert.DeserializeObject<List<LocationRecord>>(input);
                    Dictionary<LocationCoordinate, LocationDescription> locationNames = new Dictionary<LocationCoordinate, LocationDescription>();
                    foreach (LocationRecord locationRecord in readResult) locationNames.Add(locationRecord.LocationCoordinate, locationRecord.LocationDescription);
                    PopulateMetadataLocationNames(dataGridView, locationNames);
                    KryptonMessageBox.Show(locationNames.Count.ToString() + " locations imported", "Location file imported", MessageBoxButtons.OK, MessageBoxIcon.Information, showCtrlCopy: true);
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error loading JSON file!\r\n\r\n" + ex.Message, "Was not able to load JSON file", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Location names - SaveMetadataLocation
        private void SaveMetadataLocation(DataGridView dataGridView)
        {
            int rowCount = DataGridViewHandler.GetRowCount(dataGridView);

            float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
            float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;

            for (int row = 0; row < rowCount; row++)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, row);
                if (!dataGridViewGenericRow.IsHeader)
                {
                    LocationCoordinate locationCoordinateSearch = dataGridViewGenericRow.LocationCoordinate;
                    LocationDescription locationDescription = new LocationDescription(
                        DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexName, row),
                        DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexCity, row),
                        DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexRegion, row),
                        DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexCountry, row));

                    if (locationNames.ContainsKey(locationCoordinateSearch) && locationNames[locationCoordinateSearch] != locationDescription)
                    {
                        LocationCoordinateAndDescription locationCoordinateAndDescriptionFromDatabase =
                            DatabaseAndCacheLocationAddress.ReadLocationNameFromDatabaseOrCache(locationCoordinateSearch, locationAccuracyLatitude, locationAccuracyLongitude);

                        DatabaseAndCacheLocationAddress.AddressUpdate(
                            locationCoordinateInDatabase: locationCoordinateAndDescriptionFromDatabase.Coordinate, 
                            locationCoordinateAndDescription: new LocationCoordinateAndDescription(locationCoordinateSearch, locationDescription), 
                            locationAccuracyLatitude, locationAccuracyLongitude);
                    }
                }
            }
            DatabaseAndCacheCameraOwner.CameraMakeModelAndOwnerMakeDirty();
        }
        #endregion 

        #region Location names - DataGridView - CellBeginEdit
        private void dataGridViewLocationNames_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
        }

        private void dataGridViewLocationNames_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion 

        #region Location names - CellPainting
        private void dataGridViewLocationNames_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
            //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);
        }
        #endregion

        #region Location names - Browser - AddressChanged
        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, AddressChangedEventArgs>(Browser_AddressChanged), sender, e);
                return;
            }

            textBoxBrowserURL.Text = e.Address;
        }
        #endregion 
        
        #region Location names - BrowserURL_KeyPress
        private void textBoxBrowserURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; //Handle the Keypress event (suppress the Beep)
                browser.Load(textBoxBrowserURL.Text);
            }
        }
        #endregion

        #region Location names - GetZoomLevel 
        private int GetZoomLevel()
        {
            return comboBoxMapZoomLevel.SelectedIndex + 1;
        }
        #endregion

        #region Location names - DataGridView - Cell Double Click
        private void dataGridViewLocationNames_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);

            if (!dataGridView.Enabled) return;
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dataGridView.SelectedCells.Count > 1) return;

            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, e.RowIndex);
            locationCoordinateRememberForZooming = dataGridViewGenericRow?.LocationCoordinate;
            if (locationCoordinateRememberForZooming != null) ShowMediaOnMap.UpdateBrowserMap(browser, locationCoordinateRememberForZooming, GetZoomLevel(), GetMapProvider()); //Use last valid coordinates clicked
        }
        #endregion 

        #region Location names - Zoom Level - SelectedIndexChanged
        private void comboBoxMapZoomLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalData.IsApplicationClosing) return;
            if (isSettingDefaultComboxValuesZoomLevel) return;
            if (GlobalData.IsPopulatingMap) return;
            Properties.Settings.Default.SettingLocationZoomLevel = (byte)comboBoxMapZoomLevel.SelectedIndex;
            if (locationCoordinateRememberForZooming != null) ShowMediaOnMap.UpdateBrowserMap(browser, locationCoordinateRememberForZooming, GetZoomLevel(), GetMapProvider()); //Use last valid coordinates clicked
        }
        #endregion

        #region Location names - ShowCoordinateOnMap_Click

        #region MapProvider GetMapProvider
        private MapProvider GetMapProvider()
        {
            return ShowMediaOnMap.GetMapProvider(textBoxBrowserURL.Text);
        }
        #endregion

        #region GetLocationAndShow(MapProvider mapProvider)
        private void GetLocationAndShow(MapProvider mapProvider)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            List<int> rowsSelected = DataGridViewHandler.GetRowSelected(dataGridView);
            List<LocationCoordinate> locationCoordinates = new List<LocationCoordinate>();
            foreach (int rowIndex in rowsSelected)
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader && dataGridViewGenericRow?.LocationCoordinate != null)
                {
                    if (!locationCoordinates.Contains((LocationCoordinate)dataGridViewGenericRow?.LocationCoordinate)) locationCoordinates.Add(dataGridViewGenericRow.LocationCoordinate);
                }
            }
            ShowMediaOnMap.UpdatedBroswerMap(browser, locationCoordinates, GetZoomLevel(), mapProvider);
        }
        #endregion

        #endregion

        #region Location names - Cut_Click
        private void KryptonContextMenuItemLocationNamesCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
            DataGridViewHandler.Refresh(dataGridView);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Location names - Copy_Click
        private void KryptonContextMenuItemLocationNamesCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
        }
        #endregion

        #region Location names - Paste_Click
        private void KryptonContextMenuItemLocationNamesPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Location names - Delete_Click
        private void KryptonContextMenuItemLocationNamesDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Location names - Undo_Click
        private void KryptonContextMenuItemLocationNamesUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.UndoDataGridView(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Location names - Redo_Click
        private void KryptonContextMenuItemLocationNamesRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Location names - Find_Click
        private void KryptonContextMenuItemLocationNamesFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Location names - Replace_Click
        private void KryptonContextMenuItemLocationNamesReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Location names - MarkFavorite_Click
        private void KryptonContextMenuItemLocationNamesAddFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Location names - RemoveFavorite_Click
        private void KryptonContextMenuItemLocationNamesRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion 

        #region Location names - ToggleFavorite_Click
        private void KryptonContextMenuItemLocationNamesToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Location names - ShowFavorite_Click
        private void KryptonContextMenuItemLocationNamesShowFavoriteRows_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            //DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region Location names - HideEqual_Click
        private void KryptonContextMenuItemLocationNamesHideEqualRows_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
            //DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.HideEqualColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region Location names - ShowCoordinateOnMap_Click
        private void KryptonContextMenuItemLocationNamesShowCoordinateOpenStreetMap_Click(object sender, EventArgs e)
        {
            GetLocationAndShow(MapProvider.OpenStreetMap);
        }
        #endregion

        #region Location names - ShowCoordinateOnGoogleMap_Click
        private void KryptonContextMenuItemLocationNamesShowCoordinateGoogleMap_Click(object sender, EventArgs e)
        {
            GetLocationAndShow(MapProvider.GoogleMap);
        }
        #endregion

        #region Location names - ReloadUsingNominatim_Click 
        Thread threadReloadLocationUsingNominatim = null;
        bool isConfigClosing = false;

        private void KryptonContextMenuItemLocationNamesReloadUsingNominatim_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;

            //Add to thread, if window close, stop

            threadReloadLocationUsingNominatim = new Thread(() => {

                List<int> rowsSelected = DataGridViewHandler.GetRowSelected(dataGridView);
                if (rowsSelected.Count == 0) return;

                float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
                float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
                foreach (int rowIndex in rowsSelected)
                {
                    if (isConfigClosing) break;
                    DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
                    if (dataGridViewGenericRow != null && !dataGridViewGenericRow.IsHeader && dataGridViewGenericRow?.LocationCoordinate != null)
                    {
                        LocationCoordinateAndDescription locationCoordinateAndDescription = 
                            DatabaseAndCacheLocationAddress.AddressLookupAndReverseGeocoder(
                            dataGridViewGenericRow?.LocationCoordinate, locationAccuracyLatitude, locationAccuracyLongitude, 
                            onlyFromCache: false, canReverseGeocoder: true, metadataLocationDescription: null, forceReloadUsingReverseGeocoder: true);

                        DataGridViewHandler.SetCellValue(dataGridView, columnIndexName, rowIndex, locationCoordinateAndDescription?.Description.Name, false);
                        DataGridViewHandler.SetCellValue(dataGridView, columnIndexCity, rowIndex, locationCoordinateAndDescription?.Description.City, false);
                        DataGridViewHandler.SetCellValue(dataGridView, columnIndexRegion, rowIndex, locationCoordinateAndDescription?.Description.Region, false);
                        DataGridViewHandler.SetCellValue(dataGridView, columnIndexCountry, rowIndex, locationCoordinateAndDescription?.Description.Country, false);
                    }

                }
            });

            threadReloadLocationUsingNominatim.Start();
        }
        #endregion

        #region Location names - SearchInMediaFiles_Click
        private void KryptonContextMenuItemLocationNamesSearchInMediaFiles_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            Dictionary<LocationCoordinate, LocationDescription> locationFound = DatabaseAndCacheMetadataExiftool.FindNewLocationFromMediaMetadata();
            Dictionary<LocationCoordinate, LocationDescription> locationNotFound = new Dictionary<LocationCoordinate, LocationDescription>();

            float locationAccuracyLatitude = Properties.Settings.Default.LocationAccuracyLatitude;
            float locationAccuracyLongitude = Properties.Settings.Default.LocationAccuracyLongitude;
            foreach (LocationCoordinate locationCoordinate in locationFound.Keys)
            {
                bool foundLocation = false;
                foreach (LocationCoordinate locationCoordinateSearch in locationNames.Keys)
                {
                    if (locationCoordinateSearch.Latitude < (locationCoordinate.Latitude + locationAccuracyLatitude) &&
                        locationCoordinateSearch.Latitude > (locationCoordinate.Latitude - locationAccuracyLatitude) &&
                        locationCoordinateSearch.Longitude < (locationCoordinate.Longitude + locationAccuracyLongitude) &&
                        locationCoordinateSearch.Longitude > (locationCoordinate.Longitude - locationAccuracyLongitude))
                    {
                        foundLocation = true;
                        break;
                    }

                }
                if (!foundLocation) locationNotFound.Add(locationCoordinate, locationFound[locationCoordinate]);
            }
            PopulateMetadataLocationNames(dataGridView, locationNotFound);
        }
        #endregion

        #endregion

        #region CameraOwner
        
        #region Camera owner - Cut_Click
        private void KryptonContextMenuItemCameraOwnerCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
            if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Camera owner - Copy_Click
        private void KryptonContextMenuItemCameraOwnerCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
        }
        #endregion

        #region Camera owner - Paste_Click
        private void KryptonContextMenuItemCameraOwnerPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Camera owner - Delete_Click
        private void KryptonContextMenuItemCameraOwnerDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            if (!textBoxSelectionCanRestore) DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Camera owner - Undo_Click
        private void KryptonContextMenuItemCameraOwnerUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.UndoDataGridView(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Camera owner - Redo_Click
        private void KryptonContextMenuItemCameraOwnerRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Camera owner - Find_Click
        private void KryptonContextMenuItemCameraOwnerFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Camera owner - Replace_Click
        private void KryptonContextMenuItemCameraOwnerReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Camera owner - MarkFavorite_Click
        private void KryptonContextMenuItemCameraOwnerMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Camera owner - RemoveFavorite_Click
        private void KryptonContextMenuItemCameraOwnerRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion 

        #region Camera owner - ToggleFavorite_Click
        private void KryptonContextMenuItemToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Camera owner - ShowFavorite_Click
        private void KryptonContextMenuItemCameraOwnerShowOnlyFavoriteRows_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewCameraOwner;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            //DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #endregion

        #region Metadata Read - Populate
        private void PopulateMetadataReadToolStripMenu()
        {
            kryptonContextMenuItemsMetadataReadAssignToTagList.Items.Clear();
            SortedDictionary<string, string> listAllTags = new CompositeTags().ListAllTags();
            foreach (KeyValuePair<string, string> tag in listAllTags.OrderBy(key => key.Value))
            {
                KryptonContextMenuItem kryptonContextMenuItemMetadataReadPriority = new KryptonContextMenuItem();
                kryptonContextMenuItemMetadataReadPriority.Tag = tag.Value;
                kryptonContextMenuItemMetadataReadPriority.Text = tag.Value;
                kryptonContextMenuItemMetadataReadPriority.Click += KryptonContextMenuItemMetadataReadPriority_Click;
                kryptonContextMenuItemsMetadataReadAssignToTagList.Items.Add(kryptonContextMenuItemMetadataReadPriority);
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
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridView, (KryptonPalette)kryptonManager1.GlobalPalette, "MetadataRead", "Tags", DataGridViewSize.ConfigSize);
            DataGridViewHandler.Clear(dataGridView, DataGridViewSize.ConfigSize);
            DataGridViewHandler.SetIsAgregated(dataGridView, true);

            DateTime dateTimeEditable = DateTime.Now;

            int columnIndex1 = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Priority", dateTimeEditable, FileEntryVersion.CurrentVersionInDatabase), //Heading
                    null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true), out _);

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
                    DataGridViewHandler.AddRow(dataGridView, columnIndex1, new DataGridViewGenericRow(metadataPrioityGroup.MetadataPriorityValues.Composite), false);
                }
            }

            foreach (MetadataPriorityGroup metadataPrioityGroup in metadataPrioityGroupSortedList)
            {
                DataGridViewHandler.AddRow(dataGridView, columnIndex1, new DataGridViewGenericRow(
                    metadataPrioityGroup.MetadataPriorityValues.Composite,
                    metadataPrioityGroup.MetadataPriorityKey.Region + " | " + metadataPrioityGroup.MetadataPriorityKey.Tag,
                    metadataPrioityGroup.MetadataPriorityKey),
                    metadataPrioityGroup.MetadataPriorityValues.Priority, false, false);
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

        private void KryptonContextMenuItemMetadataReadPriority_Click(object sender, EventArgs e)
        {
            KryptonContextMenuItem tagSender = (KryptonContextMenuItem)sender;
            AssignSelectedToNewTag(dataGridViewMetadataReadPriority, metadataPrioityDictionaryCopy, tagSender.Text);
            PopulateMetadataRead(dataGridViewMetadataReadPriority);
        }

        #endregion

        #region Metadata Read - Drag and Drop
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private int oldIndex = -2;
        private int dragdropcurrentIndex = -1;

        #region Metadata Read - Drag and Drop - General - Mouse Move
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
        #endregion

        #region Metadata Read - Drag and Drop - General - Mouse Down
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
        #endregion

        #region Metadata Read - Drag and Drop - Drag Over
        private void dataGridViewMetadataReadPriority_DragOver(object sender, DragEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            Point clientPoint = dataGridView.PointToClient(new Point(e.X, e.Y));
            // Get the row index of the item the mouse is below. 
            int rowIndex = dataGridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, rowIndex);
            if (dataGridViewGenericRow != null && dataGridViewGenericRow.IsHeader) e.Effect = DragDropEffects.Move;
            else e.Effect = DragDropEffects.None;

            if (e.Effect == DragDropEffects.Move) dragdropcurrentIndex = dataGridView.HitTest(dataGridView.PointToClient(new Point(e.X, e.Y)).X, dataGridView.PointToClient(new Point(e.X, e.Y)).Y).RowIndex;
            else dragdropcurrentIndex = -1;

            if (oldIndex != dragdropcurrentIndex)
            {
                dataGridView.Invalidate();
                oldIndex = dragdropcurrentIndex;
            }
        }
        #endregion

        #region Metadata Read - Drag and Drop - Drag Drop 
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

            dragdropcurrentIndex = -1;
            dataGridView.Invalidate();
        }
        #endregion

        #endregion

        #region Metadata Read - Cell Changed
        private bool isCellValueUpdating = false;
        private void dataGridViewMetadataReadPriority_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isCellValueUpdating) return;
            DataGridView dataGridView = (DataGridView)sender;
            string value = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, e.ColumnIndex, e.RowIndex);
            if (value != null && int.TryParse(value.ToString(), out int priority))
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
                DataGridViewHandler.SetCellValue(dataGridView, e.ColumnIndex, e.RowIndex, 100, true);
                isCellValueUpdating = false;
            }
        }
        #endregion

        #region AutoKeywords

        #region AutoKeywords - RowAdded
        private void dataGridViewAutoKeywords_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            dataGridView.Rows[e.RowIndex].HeaderCell.Value = "*" + (dataGridView.Rows[e.RowIndex].Index + 1).ToString();
        }
        #endregion

        #region AutoKeywords - Cut
        private void KryptonContextMenuItemAutoKeywordCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
        }
        #endregion

        #region AutoKeywords - Copy
        private void KryptonContextMenuItemAutoKeywordCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
        }
        #endregion

        #region AutoKeywords - Paste
        private void KryptonContextMenuItemAutoKeywordPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
        }
        #endregion

        #region AutoKeywords - Delete
        private void KryptonContextMenuItemAutoKeywordDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
        }
        #endregion

        #region AutoKeywords - Undo
        private void KryptonContextMenuItemAutoKeywordUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.UndoDataGridView(dataGridView);
        }
        #endregion

        #region AutoKeywords - Redo
        private void KryptonContextMenuItemAutoKeywordRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
        }
        #endregion

        #region AutoKeywords - Find
        private void KryptonContextMenuItemAutoKeywordFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region AutoKeywords - Replace
        private void KryptonContextMenuItemAutoKeywordReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region AutoKeywords - KeyDown

        #endregion

        #region AutoKeywords - CellBeginEdit
        private void dataGridViewAutoKeywords_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
        }

        private void dataGridViewAutoKeywords_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion

        #endregion

        #region Metadata Read - Keydown and Item Click, Clipboard

        #region Metadata Read - Cut
        private void KryptonContextMenuItemMetadataReadCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out textBoxSelectionCanRestore, out textBoxSelectionStart, out textBoxSelectionLength);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
        }
        #endregion

        #region Metadata Read - Copy
        private void KryptonContextMenuItemMetadataReadCopy_Click(object sender, EventArgs e)
        {

            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
        }
        #endregion

        #region Metadata Read - Paste
        private void KryptonContextMenuItemMetadataReadPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
        }
        #endregion

        #region Metadata Read - Delete
        private void KryptonContextMenuItemMetadataReadDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView, out bool textBoxSelectionCanRestore, out int textBoxSelectionStart, out int textBoxSelectionLength);
            ClipboardUtility.DataGridViewRestoreEditMode(dataGridView, textBoxSelectionCanRestore, textBoxSelectionStart, textBoxSelectionLength);
        }
        #endregion

        #region Metadata Read - Undo
        private void KryptonContextMenuItemMetadataReadUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.UndoDataGridView(dataGridView);
        }
        #endregion

        #region Metadata Read - Redo
        private void KryptonContextMenuItemMetadataReadRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.RedoDataGridView(dataGridView);
        }
        #endregion

        #region Metadata Read - Find
        private void KryptonContextMenuItemMetadataReadFind_Click(object sender, EventArgs e)
        {
            //string header = DataGridViewHandlerX.headerKeywords;
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region Metadata Read - Replace
        private void KryptonContextMenuItemMetadataReadReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region Metadata Read - MarkFavorite
        private void KryptonContextMenuItemMetadataReadMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Metadata Read - RemoveFavorite
        private void KryptonContextMenuItemMetadataReadRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Metadata Read - ToggleFavorite
        private void KryptonContextMenuItemMetadataReadToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Metadata Read - ShowFavorite
        private void KryptonContextMenuItemMetadataReadShowFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            //DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region Metadata Read - CellBeginEdit
        private void dataGridViewMetadataReadPriority_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushSelectedCellsToUndoStack(dataGridView);
        }

        private void dataGridViewMetadataReadPriority_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;
            ClipboardUtility.CancelPushUndoStackIfNoChanges(dataGridView);
        }
        #endregion

        #endregion

        #region Metadata Read - CellPaining 
        private void dataGridViewMetadataReadPriority_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHandler.CellPaintingHandleDefault(sender, e, true);
            //DataGridViewHandler.CellPaintingColumnHeader(sender, e, queueErrorQueue);
            //DataGridViewHandler.CellPaintingTriState(sender, e, dataGridView, header);
            DataGridViewHandler.CellPaintingFavoriteAndToolTipsIcon(sender, e);

            //Draw red line for drag and drop
            DataGridView dataGridView = (DataGridView)sender;
            if (e.RowIndex == dragdropcurrentIndex && e.RowIndex > -1 && dragdropcurrentIndex < DataGridViewHandler.GetRowCount(dataGridView))
            {
                Pen p = new Pen(Color.Red, 2);
                e.Graphics.DrawLine(p, e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height - 1, e.CellBounds.Right, e.CellBounds.Top + e.CellBounds.Height - 1);
            }
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
            comboBoxMetadataWriteStandardTags.Items.AddRange(Metadata.ListOfPropertiesCombined(false));
            comboBoxWriteXtraAtomVariables.Items.AddRange(Metadata.ListOfPropertiesCombined(false));
            comboBoxMetadataWriteKeywordAdd.Items.AddRange(Metadata.ListOfPropertiesCombined(true));

            kryptonCheckBoxWriteMetadataCompatibilityCheckAndFix.Checked = Properties.Settings.Default.WriteUsingCompatibilityCheck;
            kryptonCheckBoxWriteAutoKeywordsSynonyms.Checked = Properties.Settings.Default.WriteAutoKeywordsSynonyms;
            fastColoredTextBoxMetadataWriteTags.Text = Properties.Settings.Default.WriteMetadataTags;
            fastColoredTextBoxMetadataWriteKeywordAdd.Text = Properties.Settings.Default.WriteMetadataKeywordAdd;

            checkBoxWriteXtraAtomOnMediaFile.Checked = Properties.Settings.Default.XtraAtomWriteOnFile;
            checkBoxWriteXtraAtomAlbumVideo.Checked = Properties.Settings.Default.XtraAtomAlbumVideo;
            checkBoxWriteXtraAtomCategoriesVideo.Checked = Properties.Settings.Default.XtraAtomCategoriesVideo;
            checkBoxWriteXtraAtomCommentPicture.Checked = Properties.Settings.Default.XtraAtomCommentPicture;
            checkBoxWriteXtraAtomCommentVideo.Checked = Properties.Settings.Default.XtraAtomCommentVideo;
            checkBoxWriteXtraAtomKeywordsPicture.Checked = Properties.Settings.Default.XtraAtomKeywordsPicture;
            checkBoxWriteXtraAtomKeywordsVideo.Checked = Properties.Settings.Default.XtraAtomKeywordsVideo;            
            checkBoxWriteXtraAtomRatingPicture.Checked = Properties.Settings.Default.XtraAtomRatingPicture;
            checkBoxWriteXtraAtomRatingVideo.Checked = Properties.Settings.Default.XtraAtomRatingVideo;
            checkBoxWriteFileAttributeCreatedDate.Checked = Properties.Settings.Default.WriteMetadataCreatedDateFileAttribute;
            checkBoxWriteMetadataAddAutoKeywords.Checked = Properties.Settings.Default.WriteMetadataAddAutoKeywords;
            numericUpDownWriteFileAttributeCreatedDateTimeIntervalAccepted.Value = Properties.Settings.Default.WriteFileAttributeCreatedDateTimeIntervalAccepted;
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

            kryptonCheckBoxMicrosoftPhotosLocationHack.Checked = Properties.Settings.Default.MicosoftOneDriveLocationHackUse;
            kryptonTextBoxMicrosoftPhotosLocationHackPostfix.Text = Properties.Settings.Default.MicosoftOneDriveLocationHackPostfix;
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
        KryptonTextBox activeXtraAtomTextbox = null;
        private void textBoxWriteXtraAtomKeywords_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (KryptonTextBox)sender;
        }

        private void textBoxWriteXtraAtomCategories_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (KryptonTextBox)sender;
        }

        private void textBoxWriteXtraAtomAlbum_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (KryptonTextBox)sender;
        }

        private void textBoxWriteXtraAtomSubtitle_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (KryptonTextBox)sender;
        }

        private void textBoxWriteXtraAtomSubject_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (KryptonTextBox)sender;
        }

        private void textBoxWriteXtraAtomComment_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (KryptonTextBox)sender;
        }

        private void textBoxWriteXtraAtomArtist_Enter(object sender, EventArgs e)
        {
            activeXtraAtomTextbox = (KryptonTextBox)sender;
        }
        #endregion

        #region FastColoredTextBox - events

        private void fastColoredTextBoxMetadataWriteKeywordAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordAdd != null) fastColoredTextBoxHandlerKeywordAdd.SyntaxHighlightProperties(sender, e);
        }


        private void fastColoredTextBoxMetadataWriteTags_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerKeywordWriteTags != null) fastColoredTextBoxHandlerKeywordWriteTags.SyntaxHighlightProperties(sender, e);
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
            }

            return fileName;
        }
        #endregion

        #region Log - GetLogFilenameApplication
        private string GetLogFilenameApplication()
        {
            string logFilename = "";
            try
            {
                logFilename = GetLogFileName("logfile");
                if (string.IsNullOrWhiteSpace(logFilename)) logFilename = "PhotoTagsSynchronizer_Log.txt";
                if (!File.Exists(logFilename)) FileHandler.CombineApplicationPathWithFilename(logFilename);
            }
            catch { }

            return logFilename;
        }
        #endregion

        #region Log - GetLogFilenameServer
        private string GetLogFilenameServer()
        {

            string logFilename = "";

            try
            {
                string configFilename = FileHandler.CombineApplicationPathWithFilename("Pipe\\NLog.config");
                try
                {
                    if (!File.Exists(configFilename)) configFilename = FileHandler.CombineApplicationPathWithFilename("Pipe\\net48\\NLog.config");
                    if (!File.Exists(configFilename)) configFilename = FileHandler.CombineApplicationPathWithFilename("Pipe\\WindowsLivePhotoGalleryServer.exe.NLog");
                    if (!File.Exists(configFilename)) configFilename = FileHandler.CombineApplicationPathWithFilename("Pipe\\net48\\WindowsLivePhotoGalleryServer.exe.NLog");
                }
                catch { }

                string xmlFilename = "";
                if (File.Exists(configFilename))
                {
                    var doc = XDocument.Load(configFilename);
                    xmlFilename = doc.Descendants().Where(e => e.Name.LocalName == "target").First().Attribute("fileName").Value;
                }
                logFilename = xmlFilename;
                if (!File.Exists(logFilename)) logFilename = FileHandler.CombineApplicationPathWithFilename("Pipe\\" + xmlFilename);
                if (!File.Exists(logFilename)) logFilename = FileHandler.CombineApplicationPathWithFilename("Pipe\\net48\\" + xmlFilename);
            }
            catch { }

            if (!File.Exists(logFilename)) logFilename = FileHandler.CombineApplicationPathWithFilename("Pipe\\WindowsLivePhotoGalleryServer_Log.txt");            
            if (!File.Exists(logFilename)) logFilename = FileHandler.CombineApplicationPathWithFilename("Pipe\\net48\\WindowsLivePhotoGalleryServer_Log.txt");            
            return logFilename;
        }
        #endregion 

        #region Log - Load and show files
        private void ShowLogs()
        {
            string logFilename = GetLogFilenameApplication();
            try
            {
                if (File.Exists(logFilename))
                {
                    fastColoredTextBoxShowLog.OpenFile(logFilename, Encoding.UTF8); //OpenBindingFile stopped to work, started to encounter: The output char buffer is too small to contain the decoded characters, encoding 'Unicode (UTF-8)' fallback 'System.Text.DecoderReplacementFallback'.
                    fastColoredTextBoxShowLog.IsChanged = false;
                    fastColoredTextBoxShowLog.ClearUndo();
                    GC.Collect();
                    GC.GetTotalMemory(true);
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Was not able top open the log file.\r\n\r\n" + ex.Message, "Can't open file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }

            try
            {
                logFilename = GetLogFilenameServer(); 
                
                if (File.Exists(logFilename))
                {
                    fastColoredTextBoxShowPipe32Log.OpenFile(logFilename, Encoding.UTF8); //OpenBindingFile stopped to work, started to encounter: The output char buffer is too small to contain the decoded characters, encoding 'Unicode (UTF-8)' fallback 'System.Text.DecoderReplacementFallback'.
                    fastColoredTextBoxShowPipe32Log.IsChanged = false;
                    fastColoredTextBoxShowPipe32Log.ClearUndo();
                    GC.Collect();
                    GC.GetTotalMemory(true);
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Was not able to open the log file.\r\n\r\n" + ex.Message, "Can open file...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Log - Delete log files
        private void kryptonButtonLogDeleteLogFiles_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(GetLogFilenameApplication())) FileHandler.Delete(GetLogFilenameApplication(), Properties.Settings.Default.MoveToRecycleBin);
                fastColoredTextBoxShowLog.Clear();

                if (File.Exists(GetLogFilenameServer())) FileHandler.Delete(GetLogFilenameServer(), Properties.Settings.Default.MoveToRecycleBin);
                fastColoredTextBoxShowPipe32Log.Clear();
            } catch (Exception ex)
            {
                KryptonMessageBox.Show("Was not able to delete the log files.\r\n\r\n" + ex.Message, "Can't delete the files...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion 

        #region Convert and Merge

        #region Convert and Merge - PopulateConvertAndMerge()
        public void PopulateConvertAndMerge()
        {
            comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.Items.Clear();
            comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.Items.AddRange(
                DataGridViewHandlerConvertAndMerge.ListVariables(DataGridViewHandlerConvertAndMerge.VariableListType.ExeConcatFilesArguments));
            
            comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.Items.Clear();
            comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.Items.AddRange(
                DataGridViewHandlerConvertAndMerge.ListVariables(DataGridViewHandlerConvertAndMerge.VariableListType.ImageArgumentFileListing));
            
            comboBoxConvertAndMergeConcatVideoFilesVariables.Items.Clear();
            comboBoxConvertAndMergeConcatVideoFilesVariables.Items.AddRange(
                DataGridViewHandlerConvertAndMerge.ListVariables(DataGridViewHandlerConvertAndMerge.VariableListType.ExeConcatFilesArguments));
            
            comboBoxConvertAndMergeConcatVideosArguFileVariables.Items.Clear();
            comboBoxConvertAndMergeConcatVideosArguFileVariables.Items.AddRange(
                DataGridViewHandlerConvertAndMerge.ListVariables(DataGridViewHandlerConvertAndMerge.VariableListType.VideoArgumentFileListing));
            
            comboBoxConvertAndMergeConvertVideoFilesVariables.Items.Clear();
            comboBoxConvertAndMergeConvertVideoFilesVariables.Items.AddRange(
                DataGridViewHandlerConvertAndMerge.ListVariables(DataGridViewHandlerConvertAndMerge.VariableListType.ExeConvertFileArguments));
            
            fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArgument = 
                new FastColoredTextBoxHandler(fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument, comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables, 
                DataGridViewHandlerConvertAndMerge.parameterAgruments, DataGridViewHandlerConvertAndMerge.parameterValues);
            fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArguFile = 
                new FastColoredTextBoxHandler(fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile, comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables,
                DataGridViewHandlerConvertAndMerge.parameterAgruments, DataGridViewHandlerConvertAndMerge.parameterValues);
            fastColoredTextBoxHandlerConvertAndMergeConcatVideoArgument = 
                new FastColoredTextBoxHandler(fastColoredTextBoxConvertAndMergeConcatVideoArgument, comboBoxConvertAndMergeConcatVideoFilesVariables,
                DataGridViewHandlerConvertAndMerge.parameterAgruments, DataGridViewHandlerConvertAndMerge.parameterValues);
            fastColoredTextBoxHandlerConvertAndMergeConcatVideoArguFile = 
                new FastColoredTextBoxHandler(fastColoredTextBoxConvertAndMergeConcatVideoArguFile, comboBoxConvertAndMergeConcatVideosArguFileVariables,
                DataGridViewHandlerConvertAndMerge.parameterAgruments, DataGridViewHandlerConvertAndMerge.parameterValues);
            fastColoredTextBoxHandlerConvertAndMergeConvertVideoArgument = 
                new FastColoredTextBoxHandler(fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument, comboBoxConvertAndMergeConvertVideoFilesVariables,
                DataGridViewHandlerConvertAndMerge.parameterAgruments, DataGridViewHandlerConvertAndMerge.parameterValues);
            
            textBoxConvertAndMergeFFmpeg.Text = Properties.Settings.Default.ConvertAndMergeExecute;
            textBoxConvertAndMergeBackgroundMusic.Text = Properties.Settings.Default.ConvertAndMergeMusic;
            numericUpDownConvertAndMergeImageDuration.Value = (int)Properties.Settings.Default.ConvertAndMergeImageDuration;

            comboBoxConvertAndMergeTempfileExtension.Items.AddRange(ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.videoFormats.ToArray());
            comboBoxConvertAndMergeTempfileExtension.Text = Properties.Settings.Default.ConvertAndMergeOutputTempfileExtension;
          
            SelectBestMatchComboboxReselution(comboBoxConvertAndMergeOutputSize, Properties.Settings.Default.ConvertAndMergeOutputWidth, Properties.Settings.Default.ConvertAndMergeOutputHeight);
            fastColoredTextBoxConvertAndMergeConcatVideoArgument.Text = Properties.Settings.Default.ConvertAndMergeConcatVideosArguments;
            fastColoredTextBoxConvertAndMergeConcatVideoArguFile.Text = Properties.Settings.Default.ConvertAndMergeConcatVideosArguFile;

            fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument.Text = Properties.Settings.Default.ConvertAndMergeConcatImagesArguments;
            fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile.Text = Properties.Settings.Default.ConvertAndMergeConcatImagesArguFile;

            fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument.Text = Properties.Settings.Default.ConvertAndMergeConvertVideosArguments;

        }
        #endregion

        #region Convert and Merge - BrowseFFmpeg_Click
        private void buttonConvertAndMergeBrowseFFmpeg_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select exe file you want to use for Video files.";
            openFileDialog.Filter = "Executeable file (*.exe)|*.exe";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;
            openFileDialog.ReadOnlyChecked = false;
            openFileDialog.ShowReadOnly = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxConvertAndMergeFFmpeg.Text = openFileDialog.FileName;
            }
        }
        #endregion

        #region Convert and Merge - BackgroundMusic_Click
        private void buttonConvertAndMergeBrowseBackgroundMusic_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select sound file you want to use when convert images to video file.";
            openFileDialog.Filter = "Sound file (*.wav)|*.wav";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;
            openFileDialog.ReadOnlyChecked = false;
            openFileDialog.ShowReadOnly = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxConvertAndMergeBackgroundMusic.Text = openFileDialog.FileName;
            }
        }
        #endregion

        #region Convert and Merge - ConcatImageAsVideoCommandVariables_SelectionChangeCommitted
        private void comboBoxConvertAndMergeConcatImageAsVideoCommandVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArgument, comboBoxConvertAndMergeConcatImageAsVideoArgumentVariables.Text);
        }
        #endregion

        #region Convert and Merge - ConcatImageAsVideoCommandArgumentVariables_SelectionChangeCommitted
        private void comboBoxConvertAndMergeConcatImageAsVideoCommandArgumentVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile, comboBoxConvertAndMergeConcatImageAsVideoArguFileVariables.Text);
        }
        #endregion

        #region Convert and Merge - ConcatImagesAsVideoCommand_KeyDown
        private void fastColoredTextBoxConvertAndMergeConcatImagesAsVideoCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArgument != null) fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArgument.KeyDown(sender, e);
        }
        #endregion

        #region Convert and Merge - ConcatImagesAsVideoCommandArgument_KeyDown
        private void fastColoredTextBoxConvertAndMergeConcatImagesAsVideoCommandArgument_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArguFile != null) fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArguFile.KeyDown(sender, e);
        }
        #endregion

        #region Convert and Merge - ConcatImagesAsVideoCommand_TextChanged
        private void fastColoredTextBoxConvertAndMergeConcatImagesAsVideoCommand_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArgument != null) fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArgument.SyntaxHighlightProperties(sender, e);
        }
        #endregion

        #region Convert and Merge - ConcatImagesAsVideoCommandArgument_TextChanged
        private void fastColoredTextBoxConvertAndMergeConcatImagesAsVideoCommandArgument_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArguFile != null) fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArguFile.SyntaxHighlightProperties(sender, e);
        }
        #endregion

        #region Convert and Merge - ConcatVideoFilesVariables_SelectionChangeCommitted
        private void comboBoxConvertAndMergeConcatVideoFilesVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxConvertAndMergeConcatVideoArgument, comboBoxConvertAndMergeConcatVideoFilesVariables.Text);
        }
        #endregion

        #region Convert and Merge - ConcatVideosArguFileVariables_SelectionChangeCommitte
        private void comboBoxConvertAndMergeConcatVideosArguFileVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxConvertAndMergeConcatVideoArguFile, comboBoxConvertAndMergeConcatVideosArguFileVariables.Text);
        }
        #endregion

        #region Convert and Merge - ConcatVideoArgument_TextChanged
        private void fastColoredTextBoxConvertAndMergeConcatVideoArgument_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConcatVideoArgument != null) fastColoredTextBoxHandlerConvertAndMergeConcatVideoArgument.SyntaxHighlightProperties(sender, e);
        }
        #endregion

        #region Convert and Merge - ConcatImagesAsVideoArguFile_TextChange
        private void fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConcatVideoArguFile != null) fastColoredTextBoxHandlerConvertAndMergeConcatVideoArguFile.SyntaxHighlightProperties(sender, e);
        }
        #endregion

        #region Convert and Merge - VideoArgument_KeyDown
        private void fastColoredTextBoxConvertAndMergeConcatVideoArgument_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConcatVideoArgument != null) fastColoredTextBoxHandlerConvertAndMergeConcatVideoArgument.KeyDown(sender, e);
        }
        #endregion

        #region Convert and Merge - tImagesAsVideoArguFile_KeyDown
        private void fastColoredTextBoxConvertAndMergeConcatImagesAsVideoArguFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConcatVideoArguFile != null) fastColoredTextBoxHandlerConvertAndMergeConcatVideoArguFile.KeyDown(sender, e);
        }
        #endregion

        #region Convert and Merge - VideoFilesArgument_KeyDown
        private void fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument_KeyDown(object sender, KeyEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConvertVideoArgument != null) fastColoredTextBoxHandlerConvertAndMergeConvertVideoArgument.KeyDown(sender, e);
        }
        #endregion

        #region Convert and Merge - VideoFilesArgument_TextChanged
        private void fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fastColoredTextBoxHandlerConvertAndMergeConvertVideoArgument != null) fastColoredTextBoxHandlerConvertAndMergeConvertVideoArgument.SyntaxHighlightProperties(sender, e);
        }
        #endregion

        #region Convert and Merge - VideoFilesVariables_SelectionChangeCommitted
        private void comboBoxConvertAndMergeConvertVideoFilesVariables_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!isPopulation) ComboBoxHandler.SelectionChangeCommitted(fastColoredTextBoxConvertAndMergeConvertVideoFilesArgument, comboBoxConvertAndMergeConvertVideoFilesVariables.Text);
        }




        #endregion

        #endregion

        #region ForClosing - Stop background processes
        private void Config_FormClosing(object sender, FormClosingEventArgs e)
        {
            isConfigClosing = true;
            if (threadReloadLocationUsingNominatim != null && threadReloadLocationUsingNominatim.IsAlive) 
            {
                while (
                    threadReloadLocationUsingNominatim.ThreadState != System.Threading.ThreadState.Stopped &&
                    threadReloadLocationUsingNominatim.ThreadState != System.Threading.ThreadState.Aborted) Task.Delay(50).Wait(); 
                //threadReloadLocationUsingNominatim.Abort();
            }
        }






        #endregion

        #region Themes

        #region AddDummyDataPaletteDataGridView
        DataGridViewHandler dataGridViewHandlerPalette;
        private void AddDummyDataPaletteDataGridView()
        {
            kryptonDataGridViewShowPalette.Rows.Clear();
            dataGridViewHandlerPalette = new DataGridViewHandler(kryptonDataGridViewShowPalette, (KryptonPalette)kryptonManager1.GlobalPalette, "Palette", "Test", DataGridViewSize.Small);

            //Header
            kryptonDataGridViewShowPalette.Columns[0].HeaderCell.Style.BackColor = DataGridViewHandler.ColorBackHeaderNormal(kryptonDataGridViewShowPalette);            
            kryptonDataGridViewShowPalette.Columns[0].HeaderCell.Style.ForeColor = DataGridViewHandler.ColorTextHeaderNormal(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Columns[0].HeaderCell.ToolTipText =
                "Normal header (StateCommon=Normal)\r\n" +
                "Back color: GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color1\r\n" +
                "Fade color: GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color2\r\n" +
                "Text color: GridStyles.GridCommon.StateCommon.HeaderColumn.Content.Color1";


            kryptonDataGridViewShowPalette.Columns[1].HeaderCell.Style.BackColor = DataGridViewHandler.ColorBackHeaderNormal(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Columns[1].HeaderCell.Style.ForeColor = DataGridViewHandler.ColorTextHeaderNormal(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Columns[1].HeaderCell.ToolTipText =
                "Normal header (StateCommon=Normal)\r\n" +
                "Back color: GridStyles.GridCommon.StateCommon.HeaderColumn.Back.Color1\r\n" +
                "Text color: GridStyles.GridCommon.StateCommon.HeaderColumn.Content.Color1";

            kryptonDataGridViewShowPalette.Columns[2].HeaderCell.Style.BackColor = DataGridViewHandler.ColorBackHeaderError(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Columns[2].HeaderCell.Style.ForeColor = DataGridViewHandler.ColorTextHeaderError(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Columns[2].HeaderCell.ToolTipText =
                "Error header (GridCustom2=Error)\r\n" +
                "Back color: GridStyles.GridCustom2.StateCommon.HeaderColumn.Back.Color1\r\n" +
                "Text color: GridStyles.GridCustom2.StateCommon.HeaderColumn.Content.Color";

            kryptonDataGridViewShowPalette.Columns[3].HeaderCell.Style.BackColor = DataGridViewHandler.ColorBackHeaderWarning(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Columns[3].HeaderCell.Style.ForeColor = DataGridViewHandler.ColorTextHeaderWarning(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Columns[3].HeaderCell.ToolTipText =
                "Warning header (GridCustom1=Warning)\r\n" +
                "Back color: GridStyles.GridCustom1.StateCommon.HeaderColumn.Back.Color1\r\n" +
                "Text color: GridStyles.GridCustom1.StateCommon.HeaderColumn.Content.Color1";

            kryptonDataGridViewShowPalette.Columns[4].HeaderCell.Style.BackColor = DataGridViewHandler.ColorBackHeaderImage(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Columns[4].HeaderCell.Style.ForeColor = DataGridViewHandler.ColorTextHeaderImage(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Columns[4].HeaderCell.ToolTipText =
                "Image header (GridCustom3=Image)\r\n" +
                "Back color: GridStyles.GridCustom3.StateCommon.HeaderColumn.Back.Color1\r\n" +
                "Text color: GridStyles.GridCustom3.StateCommon.HeaderColumn.Content.Color1";

            //Row 1 
            int row = kryptonDataGridViewShowPalette.Rows.Add("Edit", "ReadOnly", "Error", "Edit", "Edit");
            kryptonDataGridViewShowPalette.Rows[row].HeaderCell.Value = "Normal";

            kryptonDataGridViewShowPalette.Rows[row].Cells[0].Style.BackColor = DataGridViewHandler.ColorBackCellNormal(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[0].Style.ForeColor = DataGridViewHandler.ColorTextCellNormal(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[0].ToolTipText =
                "Normal cell (GridCommon=Normal/StateNormal=Editable)\r\n" +
                "Background color: GridStyles.GridCommon.StateNormal.DataCell.Back.Color1\r\n" +
                "Text color: GridStyles.GridCommon.StateNormal.DataCell.Content.Color1";

            kryptonDataGridViewShowPalette.Rows[row].Cells[1].ReadOnly = true;
            kryptonDataGridViewShowPalette.Rows[row].Cells[1].Style.BackColor = DataGridViewHandler.ColorBackCellReadOnly(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[1].Style.ForeColor = DataGridViewHandler.ColorTextCellReadOnly(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[1].ToolTipText =
                "Normal but readonly cell (GridCommon=Normal/StateDisabled=Readonly)\r\n" +
                "Background color: GridStyles.GridCommon.StateDisabled.DataCell.Back.Color1\r\n" +
                "Text color: GridStyles.GridCommon.StateDisabled.DataCell.Content.Color1";

            kryptonDataGridViewShowPalette.Rows[row].Cells[2].Style.BackColor = DataGridViewHandler.ColorBackCellError(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[2].Style.ForeColor = DataGridViewHandler.ColorTextCellError(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[2].ToolTipText =
                "Error cell (GridCustom2=Error)\r\n" +
                "Back color: GridStyles.GridCustom2.StateNormal.DataCell.Back.Color1\r\n" +
                "Text color: GridStyles.GridCustom2.StateNormal.DataCell.Content.Color1";

            kryptonDataGridViewShowPalette.Rows[row].Cells[3].Style.BackColor = DataGridViewHandler.ColorBackCellNormal(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[3].Style.ForeColor = DataGridViewHandler.ColorTextCellNormal(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[3].ToolTipText =
                "Normal cell (GridCommon=Normal/StateNormal=Editable)\r\n" +
                "Back color: GridStyles.GridCommon.StateNormal.DataCell.Back.Color1\r\n" +
                "Text color: GridStyles.GridCommon.StateNormal.DataCell.Content.Color1";

            kryptonDataGridViewShowPalette.Rows[row].Cells[4].Style.BackColor = DataGridViewHandler.ColorBackCellNormal(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[4].Style.ForeColor = DataGridViewHandler.ColorTextCellNormal(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[4].ToolTipText =
                "Normal cell (GridCommon=Normal/StateNormal=Editable)\r\n" +
                "Back color: GridStyles.GridCommon.StateNormal.DataCell.Back.Color1\r\n" +
                "Text color: GridStyles.GridCommon.StateNormal.DataCell.Content.Color1";

            //Row 2
            row = kryptonDataGridViewShowPalette.Rows.Add("EditFavorite", "ReadOnlyFavorite", "Error", "EditFavorite", "EditFavorite");
            kryptonDataGridViewShowPalette.Rows[row].HeaderCell.Value = "Favorite";
            kryptonDataGridViewShowPalette.Rows[row].Cells[0].Style.BackColor = DataGridViewHandler.ColorBackCellFavorite(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[0].Style.ForeColor = DataGridViewHandler.ColorTextCellFavorite(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[0].ToolTipText =
                "Favorite cell (GridCommon=Normal,Color2=Favorite)\r\n" +
                "Back color: GridStyles.GridCommon.StateNormal.DataCell.Back.Color2\r\n" +
                "Text color: GridStyles.GridCommon.StateNormal.DataCell.Content.Color2";

            kryptonDataGridViewShowPalette.Rows[row].Cells[1].ReadOnly = true;
            kryptonDataGridViewShowPalette.Rows[row].Cells[1].Style.BackColor = DataGridViewHandler.ColorBackCellFavoriteReadOnly(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[1].Style.ForeColor = DataGridViewHandler.ColorTextCellFavoriteReadOnly(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[1].ToolTipText =
                "Favorite and ReadOnly cell (GridCommon=Normal,StateDisabled=ReadOnly,Color2=Favorite)\r\n" +
                "Back color: GridStyles.GridCommon.StateDisabled.DataCell.Back.Color2\r\n" +
                "Text color: GridStyles.GridCommon.StateDisabled.DataCell.Content.Color2";

            kryptonDataGridViewShowPalette.Rows[row].Cells[2].Style.BackColor = DataGridViewHandler.ColorBackCellError(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[2].Style.ForeColor = DataGridViewHandler.ColorTextCellError(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[2].ToolTipText =
                "Error cell (GridCustom2=Error)\r\n" +
                "Back color: GridStyles.GridCustom2.StateNormal.DataCell.Back.Color1\r\n" +
                "Text color: GridStyles.GridCustom2.StateNormal.DataCell.Content.Color1";

            kryptonDataGridViewShowPalette.Rows[row].Cells[3].Style.BackColor = DataGridViewHandler.ColorBackCellFavorite(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[3].Style.ForeColor = DataGridViewHandler.ColorTextCellFavorite(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[3].ToolTipText =
                "Favorite cell (GridCommon=Normal,Color2=Favorite)\r\n" +
                "Back color: GridStyles.GridCommon.StateNormal.DataCell.Back.Color2\r\n" +
                "Text color: GridStyles.GridCommon.StateNormal.DataCell.Content.Color2";

            kryptonDataGridViewShowPalette.Rows[row].Cells[4].Style.BackColor = DataGridViewHandler.ColorBackCellFavorite(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[4].Style.ForeColor = DataGridViewHandler.ColorTextCellFavorite(kryptonDataGridViewShowPalette);
            kryptonDataGridViewShowPalette.Rows[row].Cells[4].ToolTipText =
                "Favorite cell (GridCommon=Normal,Color2=Favorite)\r\n" +
                "Back color: GridStyles.GridCommon.StateNormal.DataCell.Back.Color2\r\n" +
                "Text color: GridStyles.GridCommon.StateNormal.DataCell.Content.Color2";
        }
        #endregion

        #region SavePaletteSettings
        private void SavePaletteSettings()
        {
            if (isPaletteProperyChanged)
            {
                string paletteProperyFile = FileHandeling.FileHandler.GetLocalApplicationDataPath("Palette.xml", true, this);
                ((KryptonPalette)kryptonManager1.GlobalPalette).Export(paletteProperyFile, false);
                KryptonPaletteHandler.PaletteFilename = paletteProperyFile;
            }
            Properties.Settings.Default.KryptonPaletteDropShadow = KryptonPaletteHandler.UseDropShadow;
            Properties.Settings.Default.KryptonPaletteFullFilename = KryptonPaletteHandler.PaletteFilename;
            Properties.Settings.Default.KryptonPaletteName = KryptonPaletteHandler.PaletteName;

        }
        #endregion

        #region LoadPaletteSettings
        private void LoadPaletteSettings()
        {
            KryptonPaletteHandler.PaletteFilename = Properties.Settings.Default.KryptonPaletteFullFilename;
            KryptonPaletteHandler.PaletteName = Properties.Settings.Default.KryptonPaletteName;
            KryptonPaletteHandler.UseDropShadow = Properties.Settings.Default.KryptonPaletteDropShadow;
        }
        #endregion

        #region SetPalette
        private void SetPalette(IPalette newKryptonPalette, bool isSystemPalette, bool enableDropShadow)
        {
            isPaletteProperyChanged = false;
            IsKryptonManagerChanged = true;
            KryptonPaletteHandler.SetPalette(this, kryptonManager1, newKryptonPalette, isSystemPalette, enableDropShadow);
            propertyGrid.SelectedObject = kryptonManager1.GlobalPalette;
            AddDummyDataPaletteDataGridView();
            KryptonPaletteHandler.SetImageListViewPalettes(kryptonManager1, imageListView1);
        }
        #endregion

        #region buttonPalette_Click
        private void buttonOffice2010Blue_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Blue)), true, true);
        }

        private void buttonOffice2010Silver_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Silver)), true, true);
        }

        private void buttonOffice2010Black_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010Black)), true, true);
        }

        private void buttonOffice2010White_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2010White)), true, true);
        }

        private void buttonOffice2007Blue_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Blue)), true, true);
        }

        private void buttonOffice2007Silver_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Silver)), true, true);
        }

        private void buttonOffice2007Black_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007Black)), true, true);
        }

        private void buttonOffice2007White_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2007White)), true, true);
        }

        private void buttonOffice2003_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.ProfessionalOffice2003)), true, true);
        }
      

        private void buttonSystem_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.ProfessionalSystem)), true, false);
        }

        private void buttonSparkleBlue_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparkleBlue)), true, true);
        }

        private void buttonSparkleOrange_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparkleOrange)), true, true);
        }

        private void buttonSparklePurple_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.SparklePurple)), true, true);
        }

        private void buttonOffice2013White_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office2013White)), true, true);
        }

        private void buttonDarkMode_Click(object sender, EventArgs e)
        {
            string darkModeFilename = FileHandler.CombineApplicationPathWithFilename("Themes\\PhotoTags Synchronizer Dark mode.xml");
            SetPalette(KryptonPaletteHandler.Load(darkModeFilename, ""), false, true);
        }

        private void buttonOffice365Black_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Black)), true, true);
        }

        private void buttonOffice365Blue_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Blue)), true, true);
        }

        private void buttonOffice365Silver_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365Silver)), true, true);
        }

        private void buttonOffice365White_Click(object sender, EventArgs e)
        {
            SetPalette(KryptonPaletteHandler.Load("", ThemeManager.ReturnPaletteModeAsString(PaletteMode.Office365White)), true, true);
        }
        #endregion

        #region kryptonButtonApplicationThemesImport_Click
        private void kryptonButtonApplicationThemesImport_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    // Palette files are just XML documents
                    dialog.CheckFileExists = true;
                    dialog.CheckPathExists = true;
                    dialog.DefaultExt = @"xml";
                    dialog.Filter = @"Palette files (*.xml)|*.xml|All files (*.*)|(*.*)";
                    dialog.Title = @"Load Palette";

                    // Get the actual file selected by the user
                    if (dialog.ShowDialog() == DialogResult.OK && File.Exists(dialog.FileName))
                    {
                        KryptonPalette kryptonPalette = KryptonPaletteHandler.Load(dialog.FileName, "");
                        SetPalette(kryptonPalette, KryptonPaletteHandler.IsSystemPalette, true);
                    }
                    else
                    {
                        KryptonMessageBox.Show("Was not able to import Theme Palette\r\n" + dialog.FileName,
                                    "Loading Palette for Theme failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                    }

                }

                
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Was not able to import Theme Palette\n\n Error:" + ex.Message,
                                    "Loading Palette for Theme failed", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region kryptonButtonApplicationThemesExport_Click
        private void kryptonButtonApplicationThemesExport_Click(object sender, EventArgs e)
        {
            string paletteFilename = "(unknown.xml)";
            try
            {                
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    // Palette files are just xml documents
                    dialog.OverwritePrompt = true;
                    dialog.DefaultExt = @"xml";
                    dialog.Filter = @"Palette files (*.xml)|*.xml|All files (*.*)|(*.*)";
                    dialog.Title = @"Save Palette As";

                    // Get the actual file selected by the user
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        paletteFilename = dialog.FileName;
                        ((KryptonPalette)kryptonManager1.GlobalPalette).SetCustomisedKryptonPaletteFilePath(Path.GetFullPath(dialog.FileName));
                        paletteFilename = ((KryptonPalette)kryptonManager1.GlobalPalette).Export(dialog.FileName, true, true);
                        KryptonPaletteHandler.Save(paletteFilename);
                        IsKryptonManagerChanged = true;
                    }

                }

                
            } catch (Exception ex)
            {                
                KryptonMessageBox.Show($"Export to file '{paletteFilename}' failed.\n\n Error:{ex.Message}",
                                @"Palette Export", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
                
            }
        }
        #endregion

        #region kryptonButtonShowContectMenu_Click
        private void ShowMenu(Control control, KryptonContextMenu kryptonContextMenu)
        {
            kryptonContextMenu.Show(control.RectangleToScreen(control.ClientRectangle), KryptonContextMenuPositionH.Left, KryptonContextMenuPositionV.Below);
        }
        private void kryptonButtonShowContectMenu_Click(object sender, EventArgs e)
        {
            ShowMenu((Control)sender, kryptonContextMenuPalette);
        }
        #endregion

        #region Themes - PropertyValueChanged
        private bool isPaletteProperyChanged = false;
        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            isPaletteProperyChanged = true;
            AddDummyDataPaletteDataGridView();
            KryptonPaletteHandler.SetImageListViewPalettes(kryptonManager1, imageListView1);
        }
        #endregion

        #endregion

        
    }
}

