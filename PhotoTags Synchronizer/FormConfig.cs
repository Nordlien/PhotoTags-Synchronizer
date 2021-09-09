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
using SqliteDatabase;
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

namespace PhotoTagsSynchronizer
{
    public partial class FormConfig : KryptonForm
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public MetadataReadPrioity MetadataReadPrioity { get; set; } 
        public CameraOwnersDatabaseCache DatabaseAndCacheCameraOwner { get; set; }
        public LocationNameLookUpCache DatabaseLocationNames { get; set; }
        public LocationNameLookUpCache DatabaseAndCacheLocationAddress { get; set; }
        public SqliteDatabaseUtilities DatabaseUtilitiesSqliteMetadata { get; set; }

        private readonly ChromiumWebBrowser browser;

        public Size[] ThumbnailSizes { get; set; }

        private Dictionary<MetadataPriorityKey, MetadataPriorityValues> metadataPrioityDictionaryCopy = new Dictionary<MetadataPriorityKey, MetadataPriorityValues>();
        private AutoCorrect autoCorrect = new AutoCorrect();

        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeywordAdd = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeuwordDelete = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerKeywordWriteTags = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArgument = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConcatImagesAsVideoArguFile = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConcatVideoArgument = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConcatVideoArguFile = null;
        private FastColoredTextBoxHandler fastColoredTextBoxHandlerConvertAndMergeConvertVideoArgument = null;

        private bool isPopulation = false;

        public FormConfig()
        {
            InitializeComponent();
            browser = new ChromiumWebBrowser("https://www.openstreetmap.org/")
            {
                Dock = DockStyle.Fill,
            };
            browser.BrowserSettings.Javascript = CefState.Enabled;
            //browser.BrowserSettings.WebSecurity = CefState.Enabled;
            browser.BrowserSettings.WebGl = CefState.Enabled;
            browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Disabled;
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

            //ThemeManager.PropagateThemeSelector(kryptonComboBoxThemes);


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
            autoCorrect = AutoCorrect.ConvertConfigValue(Properties.Settings.Default.AutoCorrect);
            PopulateAutoCorrectPoperties();

            //AutoKeywords
            LoadAutoKeywords();

            //Metadata Write
            fastColoredTextBoxHandlerKeywordAdd = new FastColoredTextBoxHandler(fastColoredTextBoxMetadataWriteKeywordAdd, true, MetadataReadPrioity.MetadataPrioityDictionary);
            fastColoredTextBoxHandlerKeuwordDelete = new FastColoredTextBoxHandler(fastColoredTextBoxMetadataWriteKeywordDelete, true, MetadataReadPrioity.MetadataPrioityDictionary);
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
            string logFilename = GetLogFileName("logfile");
            if (string.IsNullOrWhiteSpace(logFilename)) logFilename = "PhotoTagsSynchronizer_Log.txt";

            try
            {

                if (File.Exists(logFilename))
                {
                    fastColoredTextBoxShowLog.OpenBindingFile(logFilename, Encoding.UTF8);
                    fastColoredTextBoxShowLog.IsChanged = false;
                    fastColoredTextBoxShowLog.ClearUndo();
                    GC.Collect();
                    GC.GetTotalMemory(true);
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Was not able top open the log file.\r\n\r\n" + ex.Message);
            }

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show("Was not able top open the log file.\r\n\r\n" + ex.Message);
            }
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
                //Application
                Properties.Settings.Default.ApplicationThumbnail = ThumbnailSizes[comboBoxApplicationThumbnailSizes.SelectedIndex];
                Properties.Settings.Default.ApplicationRegionThumbnail = ThumbnailSizes[comboBoxApplicationRegionThumbnailSizes.SelectedIndex];

                Properties.Settings.Default.ApplicationPreferredLanguages = textBoxApplicationPreferredLanguages.Text;
                Properties.Settings.Default.MaxRowsInSearchResult = (int)numericUpDownApplicationMaxRowsInSearchResult.Value;
                Properties.Settings.Default.SuggestRegionNameNearbyDays = (int)numericUpDownPeopleSuggestNameDaysInterval.Value;
                Properties.Settings.Default.SuggestRegionNameTopMostCount = (int)numericUpDownPeopleSuggestNameTopMost.Value;
                Properties.Settings.Default.RegionMissmatchProcent = (float)numericUpDownRegionMissmatchProcent.Value;
                Properties.Settings.Default.LocationAccuracyLatitude = (float)numericUpDownLocationAccuracyLatitude.Value;
                Properties.Settings.Default.LocationAccuracyLongitude = (float)numericUpDownLocationAccuracyLongitude.Value;
                Properties.Settings.Default.AvoidOfflineMediaFiles = checkBoxApplicationAvoidReadMediaFromCloud.Checked;
                Properties.Settings.Default.AvoidReadExifFromCloud = checkBoxApplicationAvoidReadExifFromCloud.Checked;
                Properties.Settings.Default.ImageViewLoadThumbnailOnDemandMode = checkBoxApplicationImageListViewCacheModeOnDemand.Checked;

                Properties.Settings.Default.CacheNumberOfPosters = (int)numericUpDownCacheNumberOfPosters.Value;
                Properties.Settings.Default.CacheAllMetadatas = checkBoxCacheAllMetadatas.Checked;
                Properties.Settings.Default.CacheAllThumbnails = checkBoxCacheAllThumbnails.Checked;
                Properties.Settings.Default.CacheAllWebScraperDataSets = checkBoxCacheAllWebScraperDataSets.Checked;
                Properties.Settings.Default.CacheFolderMetadatas = checkBoxCacheFolderMetadatas.Checked;
                Properties.Settings.Default.CacheFolderThumbnails = checkBoxCacheFolderThumbnails.Checked;
                Properties.Settings.Default.CacheFolderWebScraperDataSets = checkBoxCacheFolderWebScraperDataSets.Checked;

                //Debug
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
                MessageBox.Show("Failed to save config.\r\n\r\n" + ex.Message);
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
            numericUpDownPeopleSuggestNameDaysInterval.Value = Properties.Settings.Default.SuggestRegionNameNearbyDays;
            numericUpDownPeopleSuggestNameTopMost.Value = Properties.Settings.Default.SuggestRegionNameTopMostCount;
            numericUpDownRegionMissmatchProcent.Value = (decimal)Properties.Settings.Default.RegionMissmatchProcent;

            numericUpDownLocationAccuracyLatitude.Value = (decimal)Properties.Settings.Default.LocationAccuracyLatitude;
            numericUpDownLocationAccuracyLongitude.Value = (decimal)Properties.Settings.Default.LocationAccuracyLongitude;

            checkBoxApplicationAvoidReadMediaFromCloud.Checked = Properties.Settings.Default.AvoidOfflineMediaFiles;
            checkBoxApplicationAvoidReadExifFromCloud.Checked = Properties.Settings.Default.AvoidReadExifFromCloud;
            checkBoxApplicationImageListViewCacheModeOnDemand.Checked = Properties.Settings.Default.ImageViewLoadThumbnailOnDemandMode;

            numericUpDownCacheNumberOfPosters.Value = (int)Properties.Settings.Default.CacheNumberOfPosters;
            checkBoxCacheAllMetadatas.Checked = Properties.Settings.Default.CacheAllMetadatas;
            checkBoxCacheAllThumbnails.Checked = Properties.Settings.Default.CacheAllThumbnails;
            checkBoxCacheAllWebScraperDataSets.Checked = Properties.Settings.Default.CacheAllWebScraperDataSets;
            checkBoxCacheFolderMetadatas.Checked = Properties.Settings.Default.CacheFolderMetadatas;
            checkBoxCacheFolderThumbnails.Checked = Properties.Settings.Default.CacheFolderThumbnails;
            checkBoxCacheFolderWebScraperDataSets.Checked = Properties.Settings.Default.CacheFolderWebScraperDataSets;

            //Debug
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
                    if (dataSet != null) dataSet.WriteXml(FileHandler.GetLocalApplicationDataPath("AutoKeywords.xml", false));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AutoKeywords failed to saved");
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
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AutoKeywords failed to saved");
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
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridView, "CameraMakeModelOwner", "Camera Make/Model", DataGridViewSize.ConfigSize);
            DataGridViewHandler.Clear(dataGridView, DataGridViewSize.ConfigSize);
            //contextMenuStripMetadataRead contextMenuStripMetadataRead

            DateTime dateTimeEditable = DateTime.Now;

            columnIndexOwner = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Owner", dateTimeEditable, FileEntryVersion.Current), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

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

                
                DataGridViewComboBoxCell dataGridViewComboBoxCellCameraOwners = new DataGridViewComboBoxCell();
                dataGridViewComboBoxCellCameraOwners.FlatStyle = FlatStyle.Flat;
                dataGridViewComboBoxCellCameraOwners.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                dataGridViewComboBoxCellCameraOwners.Items.AddRange(DatabaseAndCacheCameraOwner.ReadCameraOwners().ToArray());

                if (cameraOwner.Owner != null && !dataGridViewComboBoxCellCameraOwners.Items.Contains(cameraOwner.Owner)) dataGridViewComboBoxCellCameraOwners.Items.Insert(0, cameraOwner.Owner);

                DataGridViewHandler.SetCellControlType(dataGridView, columnIndexOwner, rowIndex, dataGridViewComboBoxCellCameraOwners);

                if (!string.IsNullOrWhiteSpace(cameraOwner.Owner) && dataGridViewComboBoxCellCameraOwners.Items.Contains(cameraOwner.Owner))
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndexOwner, rowIndex, cameraOwner.Owner);
                else
                    DataGridViewHandler.SetCellValue(dataGridView, columnIndexOwner, rowIndex, null);
                
            }
            isCellValueUpdating = false;
        }
        #endregion

        #region Camera owner - SaveMetadataCameraOwner
        private void SaveMetadataCameraOwner(DataGridView dataGridView)
        {
            int rowCount = DataGridViewHandler.GetRowCount(dataGridView);

            CommonDatabaseTransaction commonDatabaseTransaction = DatabaseUtilitiesSqliteMetadata.TransactionBegin(CommonDatabaseTransaction.TransactionReadCommitted);
            for (int row = 0; row < rowCount; row++) 
            {
                DataGridViewGenericRow dataGridViewGenericRow = DataGridViewHandler.GetRowDataGridViewGenericRow(dataGridView, row);
                if (!dataGridViewGenericRow.IsHeader)
                {
                    string camerMakeModelOwner = DataGridViewHandler.GetCellValueNullOrStringTrim(dataGridView, columnIndexOwner, row);
                    if (camerMakeModelOwner == CameraOwnersDatabaseCache.MissingLocationsOwners) camerMakeModelOwner = null;
                    CameraOwner cameraOwner = new CameraOwner(dataGridViewGenericRow.HeaderName, dataGridViewGenericRow.RowName, camerMakeModelOwner);
                    DatabaseAndCacheCameraOwner.SaveCameraMakeModelAndOwner(commonDatabaseTransaction, cameraOwner);
                }
            }
            DatabaseAndCacheCameraOwner.CameraMakeModelAndOwnerMakeDirty();
            DatabaseUtilitiesSqliteMetadata.TransactionCommit(commonDatabaseTransaction);

        }
        #endregion 

        #region Camera owner - KeyDown
        private void dataGridViewCameraOwner_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException(); //JTN: Need add back
            //DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion 

        #region Camera owner - CellBeginEdit
        private void dataGridViewCameraOwner_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
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

        #region Camera owner - EditingControlShowing
        private void dataGridViewCameraOwner_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            DataGridViewComboBoxEditingControl combocontrol = e.Control as DataGridViewComboBoxEditingControl;
            if (combocontrol != null)
            {
                //set dropdown style editable combobox
                if (combocontrol.DropDownStyle != ComboBoxStyle.DropDown) combocontrol.DropDownStyle = ComboBoxStyle.DropDown;                
            }
        }
        #endregion

        #region Camera owner - CellValidating
        private void dataGridViewCameraOwner_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            try
            {
                DataGridViewComboBoxCell cell = dataGridView.CurrentCell as DataGridViewComboBoxCell;
                if (cell != null && !cell.Items.Contains(e.FormattedValue))
                {
                    cell.Items.Insert(0, e.FormattedValue);
                    if (dataGridView.IsCurrentCellDirty)
                    {
                        dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
                    cell.Value = cell.Items[0];
                }
                if (cell.Value.ToString() == CameraOwnersDatabaseCache.MissingLocationsOwners) cell.Value = null;
            }
            catch
            {
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
            DataGridViewHandler dataGridViewHandler = new DataGridViewHandler(dataGridView, "LocationNames", "Location names", DataGridViewSize.ConfigSize);
            DataGridViewHandler.Clear(dataGridView, DataGridViewSize.ConfigSize);

            DateTime dateTimeEditable = DateTime.Now;

            columnIndexName = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Name", dateTimeEditable, FileEntryVersion.Current), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

            columnIndexCity = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("City", dateTimeEditable, FileEntryVersion.Current), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

            columnIndexRegion = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Region", dateTimeEditable, FileEntryVersion.Current), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

            columnIndexCountry = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Country", dateTimeEditable, FileEntryVersion.Current), //Heading
                null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

            locationNames = DatabaseLocationNames.ReadLocationNames();
            PopulateMetadataLocationNames(dataGridView, locationNames);
        }
        #endregion

        #region Location names - Search For New Locations In Media Files
        private void searchForNewLocationsInMediaFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            DataGridView dataGridView = dataGridViewLocationNames;
            Dictionary<LocationCoordinate, LocationDescription> locationFound = DatabaseLocationNames.FindNewLocation();
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

                   
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Error saving JSON file!\r\n\r\n" + ex.Message, "Was not able to save JSON file");
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading JSON file!\r\n\r\n" + ex.Message, "Was not able to load JSON file");
            }
        }
        #endregion

        #region Location names - SaveMetadataLocation
        private void SaveMetadataLocation(DataGridView dataGridView)
        {
            int rowCount = DataGridViewHandler.GetRowCount(dataGridView);

            CommonDatabaseTransaction commonDatabaseTransaction = DatabaseUtilitiesSqliteMetadata.TransactionBegin(CommonDatabaseTransaction.TransactionReadCommitted);
            
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

                    if (locationNames.ContainsKey(locationCoordinate) && locationNames[locationCoordinate] != locationDescription)
                    DatabaseLocationNames.AddressUpdate(new LocationCoordinateAndDescription(locationCoordinate, locationDescription));
                }
            }
            DatabaseAndCacheCameraOwner.CameraMakeModelAndOwnerMakeDirty();
            DatabaseUtilitiesSqliteMetadata.TransactionCommit(commonDatabaseTransaction);

        }
        #endregion 

        #region Location names - DataGridView - KeyDown
        private void dataGridViewLocationNames_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException(); //JTN: Need add back
            //DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion 

        #region Location names - DataGridView - CellBeginEdit
        private void dataGridViewLocationNames_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
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
        
        #region  Location names - BrowserURL_KeyPress
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

        #region toolStripMenuItemShowCoordinateOnMap_Click
        private void toolStripMenuItemShowCoordinateOnMap_Click(object sender, EventArgs e)
        {
            GetLocationAndShow(MapProvider.OpenStreetMap);
        }
        #endregion

        #region toolStripMenuItemShowCoordinateOnGoogleMap_Click
        private void toolStripMenuItemShowCoordinateOnGoogleMap_Click(object sender, EventArgs e)
        {
            GetLocationAndShow(MapProvider.GoogleMap);
        }
        #endregion

        #endregion

        #region Location names - ReloadLocationUsingNominatim_Click 
        Thread threadReloadLocationUsingNominatim = null;
        bool isConfigClosing = false;
        private void toolStripMenuItemMapReloadLocationUsingNominatim_Click(object sender, EventArgs e)
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
                        DatabaseAndCacheLocationAddress.DeleteLocation(dataGridViewGenericRow?.LocationCoordinate); //Delete from database cache

                        LocationCoordinateAndDescription locationCoordinateAndDescription = DatabaseAndCacheLocationAddress.AddressLookup(
                            dataGridViewGenericRow?.LocationCoordinate, locationAccuracyLatitude, locationAccuracyLongitude);

                        DataGridViewHandler.SetCellValue(dataGridView, columnIndexName, rowIndex, locationCoordinateAndDescription?.Description.Name);
                        DataGridViewHandler.SetCellValue(dataGridView, columnIndexCity, rowIndex, locationCoordinateAndDescription?.Description.City);
                        DataGridViewHandler.SetCellValue(dataGridView, columnIndexRegion, rowIndex, locationCoordinateAndDescription?.Description.Region);
                        DataGridViewHandler.SetCellValue(dataGridView, columnIndexCountry, rowIndex, locationCoordinateAndDescription?.Description.Country);
                    }
                    
                }

            });

            threadReloadLocationUsingNominatim.Start();

            

        }
        #endregion

        #region Location names - Cut_Click
        private void toolStripMenuItemMapCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Location names - Copy_Click
        private void toolStripMenuItemMapCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;

            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region Location names - Paste_Click
        private void toolStripMenuItemMapPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;

            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion

        #region Location names - Delete_Click
        private void toolStripMenuItemMapDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = true;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
            DataGridViewHandler.Refresh(dataGridView);
            GlobalData.IsDataGridViewCutPasteDeleteFindReplaceInProgress = false;
        }
        #endregion 

        #region Location names - Undo_Click
        private void toolStripMenuItemMapUndo_Click(object sender, EventArgs e)
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
        private void toolStripMenuItemMapRedo_Click(object sender, EventArgs e)
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
        private void toolStripMenuItemMapFind_Click(object sender, EventArgs e)
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
        private void toolStripMenuItemMapReplace_Click(object sender, EventArgs e)
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
        private void toolStripMenuItemMapMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Location names - RemoveFavorite_Click
        private void toolStripMenuItemMapRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion 

        #region Location names - ToggleFavorite_Click
        private void toolStripMenuItemMapToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewLocationNames;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Location names - ShowFavorite_Click
        private void toolStripMenuItemMapShowFavorite_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException(); //JTN: Need add back
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            //DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region Location names - HideEqual_Click
        private void toolStripMenuItemMapHideEqual_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException(); //JTN: Need add back
            DataGridView dataGridView = dataGridViewLocationNames;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetHideEqualColumns(dataGridView, !DataGridViewHandler.HideEqualColumns(dataGridView));
            //DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.HideEqualColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

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

            int columnIndex1 = DataGridViewHandler.AddColumnOrUpdateNew(dataGridView,
                new FileEntryAttribute("Priority", dateTimeEditable, FileEntryVersion.Current), //Heading
                    null, null, ReadWriteAccess.AllowCellReadAndWrite, ShowWhatColumns.HistoryColumns,
                    new DataGridViewGenericCellStatus(MetadataBrokerType.Empty, SwitchStates.Off, true));

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

        #region AutoKeywords

        private void dataGridViewAutoKeywords_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            dataGridView.Rows[e.RowIndex].HeaderCell.Value = "*" + (dataGridView.Rows[e.RowIndex].Index + 1).ToString();
        }

        #region AutoKeywords - Cut
        private void toolStripMenuItemAutoKeywordCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }
        #endregion

        #region AutoKeywords - Copy
        private void toolStripMenuItemAutoKeywordCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
        }
        #endregion

        #region AutoKeywords - Paste
        private void toolStripMenuItemAutoKeywordPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
        }
        #endregion

        #region AutoKeywords - Delete
        private void toolStripMenuItemAutoKeywordDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }
        #endregion

        #region AutoKeywords - Undo
        private void toolStripMenuItemAutoKeywordUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.UndoDataGridView(dataGridView);
        }
        #endregion

        #region AutoKeywords - Redo
        private void toolStripMenuItemRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            ClipboardUtility.RedoDataGridView(dataGridView);
        }
        #endregion

        #region AutoKeywords - Find
        private void toolStripMenuItemFind_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region AutoKeywords - Replace
        private void toolStripMenuItemReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewAutoKeywords;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region AutoKeywords - KeyDown
        private void dataGridViewAutoKeywords_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException(); //JTN: Need add back
            //DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region AutoKeywords - CellBeginEdit
        private void dataGridViewAutoKeywords_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
        }
        #endregion

        #endregion

        #region Metadata Read - Keydown and Item Click, Clipboard
        #region Metadata Read - Cut
        private void toolStripMenuItemMetadataReadCut_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, true);
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }
        #endregion

        #region Metadata Read - Copy
        private void toolStripMenuItemMetadataReadCopy_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.CopyDataGridViewSelectedCellsToClipboard(dataGridView, false);
        }
        #endregion

        #region Metadata Read - Paste
        private void toolStripMenuItemMetadataReadPaste_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.PasteDataGridViewSelectedCellsFromClipboard(dataGridView);
        }
        #endregion

        #region Metadata Read - Delete
        private void toolStripMenuItemMetadataReadDelete_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.DeleteDataGridViewSelectedCells(dataGridView);
        }
        #endregion

        #region Metadata Read - Undo
        private void toolStripMenuItemMetadataReadUndo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.UndoDataGridView(dataGridView);
        }
        #endregion

        #region Metadata Read - Redo
        private void toolStripMenuItemMetadataReadRedo_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            ClipboardUtility.RedoDataGridView(dataGridView);
        }
        #endregion

        #region Metadata Read - Find
        private void toolStripMenuItemMetadataReadFind_Click(object sender, EventArgs e)
        {
            //string header = DataGridViewHandlerX.headerKeywords;
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region Metadata Read - Replace
        private void toolStripMenuItemMetadataReadReplace_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            //string header = DataGridViewHandlerTagsAndKeywords.headerKeywords;
            DataGridViewHandler.ActionFindAndReplace(dataGridView, false);
            //ValitedatePaste(dataGridView, header);
            DataGridViewHandler.Refresh(dataGridView);
        }
        #endregion

        #region Metadata Read - MarkFavorite
        private void toolStripMenuItemMetadataReadMarkFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Set);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Metadata Read - RemoveFavorite
        private void toolStripMenuItemMetadataReadRemoveFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Remove);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Metadata Read - ToggleFavorite
        private void toolStripMenuItemMetadataReadToggleFavorite_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            DataGridViewHandler.ActionSetRowsFavouriteState(dataGridView, NewState.Toggle);
            DataGridViewHandler.FavouriteWrite(dataGridView, DataGridViewHandler.GetFavoriteList(dataGridView));
        }
        #endregion

        #region Metadata Read - ShowFavorite
        private void toolStripMenuItemMetadataReadShowFavorite_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException(); //JTN: Need add back
            DataGridView dataGridView = dataGridViewMetadataReadPriority;
            if (!dataGridView.Enabled) return;

            DataGridViewHandler.SetShowFavouriteColumns(dataGridView, !DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            //DataGridViewHandler.UpdatedStripMenuItem(dataGridView, (ToolStripMenuItem)sender, DataGridViewHandler.ShowFavouriteColumns(dataGridView));
            DataGridViewHandler.SetRowsVisbleStatus(dataGridView, DataGridViewHandler.HideEqualColumns(dataGridView), DataGridViewHandler.ShowFavouriteColumns(dataGridView));
        }
        #endregion

        #region Metadata Read - KeyDown
        private void dataGridViewMetadataReadPriority_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException(); //JTN: Need add back
            //DataGridViewHandler.KeyDownEventHandler(sender, e);
        }
        #endregion

        #region Metadata Read - CellBeginEdit
        private void dataGridViewMetadataReadPriority_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = ((DataGridView)sender);
            if (!dataGridView.Enabled) return;

            ClipboardUtility.PushToUndoStack(dataGridView);
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
        private void EnableDropShadow(bool enabled)
        {
            UseDropShadow = enabled;
        }

        private void buttonOffice2010Blue_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteOffice2010Blue;
            propertyGrid.SelectedObject = kryptonPaletteOffice2010Blue;

            EnableDropShadow(true);
        }

        private void buttonOffice2010Silver_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteOffice2010Silver;
            propertyGrid.SelectedObject = kryptonPaletteOffice2010Silver;

            EnableDropShadow(true);
        }

        private void buttonOffice2010Black_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteOffice2010Black;
            propertyGrid.SelectedObject = kryptonPaletteOffice2010Black;

            EnableDropShadow(true);
        }

        private void buttonOffice2007Blue_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteOffice2007Blue;
            propertyGrid.SelectedObject = kryptonPaletteOffice2007Blue;

            EnableDropShadow(true);
        }

        private void buttonOffice2007Silver_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteOffice2007Silver;
            propertyGrid.SelectedObject = kryptonPaletteOffice2007Silver;

            EnableDropShadow(true);
        }

        private void buttonOffice2007Black_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteOffice2007Black;
            propertyGrid.SelectedObject = kryptonPaletteOffice2007Black;

            EnableDropShadow(true);
        }

        private void buttonOffice2003_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteOffice2003;
            propertyGrid.SelectedObject = kryptonPaletteOffice2003;

            EnableDropShadow(true);
        }

        private void buttonSystem_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteSystem;
            propertyGrid.SelectedObject = kryptonPaletteSystem;

            EnableDropShadow(false);
        }

        private void buttonSparkleBlue_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteSparkleBlue;
            propertyGrid.SelectedObject = kryptonPaletteSparkleBlue;

            EnableDropShadow(true);
        }

        private void buttonSparkleOrange_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteSparkleOrange;
            propertyGrid.SelectedObject = kryptonPaletteSparkleOrange;

            EnableDropShadow(true);
        }

        private void buttonSparklePurple_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteSparklePurple;
            propertyGrid.SelectedObject = kryptonPaletteSparklePurple;

            EnableDropShadow(true);
        }

        private void buttonCustom_Click(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPalette = kryptonPaletteCustom;
            propertyGrid.SelectedObject = kryptonPaletteCustom;

            EnableDropShadow(false);

            kryptonButtonApplicationThemesExport.Enabled = true;
        }

        

        private void kryptonButtonApplicationThemesImport_Click(object sender, EventArgs e)
        {
            try
            {
                kryptonPaletteCustom.Import();

                kryptonManager1.GlobalPalette = kryptonPaletteCustom;
                kryptonManager1.GlobalPaletteMode = PaletteModeManager.Custom;

                /*
                ThemeManager.SetTheme(kryptonComboBoxThemes.Text, kryptonManager1);
                ThemeManager.ApplyGlobalTheme(kryptonManager1, ThemeManager.GetPaletteMode(kryptonManager1));

                IPalette palette = KryptonManager.CurrentGlobalPalette;
                //Font font = palette.GetContentShortTextFont(PaletteContentStyle.LabelNormalControl, PaletteState.Normal);
                //propertyGrid1.Font = font;
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void kryptonButtonApplicationThemesExport_Click(object sender, EventArgs e)
        {
            kryptonPaletteCustom.Export();

            kryptonButtonApplicationThemesExport.Enabled = false;
        }
        #endregion
    }
}

