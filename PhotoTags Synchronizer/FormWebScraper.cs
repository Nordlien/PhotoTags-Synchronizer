using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using MetadataLibrary;
using Newtonsoft.Json;
using Krypton.Toolkit;
using FileHandeling;
using System.Web;
using System.Text;
using TimeZone;

namespace PhotoTagsSynchronizer
{
    public partial class FormWebScraper : KryptonForm
    {
        public MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ChromiumWebBrowser browser;
        private AutoResetEvent autoResetEventWaitPageLoading = null;
        private AutoResetEvent autoResetEventWaitPageLoaded = null;

        #region Variables - flags
        private bool isProcessRunning = false;
        private bool isDataUnsaved = false;
        private bool stopRequested = false;
        #endregion 

        #region Variables - Config 
        private int javaScriptExecuteTimeout = 5000;
        private int webScrapingRetry = 20;
        private int webScrapingDelayOurScriptToRun = 200;
        private int webScrapingDelayInPageScriptToRun = 1000;
        private int waitEventPageStartLoadingTimeout = 3000;
        private int waitEventPageLoadedTimeout = 20000;
        private int webScrapingPageDownCount = 5;
        private string webScrapingName = MetadataLibrary.MetadataDatabaseCache.WebScapingFolderName;
        private string[] webScrapingStartPages;
        #endregion 

        Dictionary<DateTime, WebScrapingDataSet> _webScrapingDataSet = new Dictionary<DateTime, WebScrapingDataSet>();       
        Dictionary<string, WebScrapingLinks> _linkCatergories = new Dictionary<string, WebScrapingLinks>(StringComparer.OrdinalIgnoreCase);
        Dictionary<string, Metadata> _metadataDataTotalMerged = new Dictionary<string, Metadata>(StringComparer.OrdinalIgnoreCase);
        List<string> _urlLoadingFailed = new List<string>();

        #region Variables - Const - Column ids
        private const int itemViewSubItemLinksCategoryName = 0;
        private const int itemViewSubItemLinksCategoryType = 1;
        private const int itemViewSubItemLinksLink = 2;
        private const int itemViewSubItemLinksReadDate = 3;
        private const int itemViewSubItemLinksCountMediaFiles = 4;
        private const int itemViewSubItemLinksCountTitles = 5;
        private const int itemViewSubItemLinksCountAlbumNames = 6;
        private const int itemViewSubItemLinksCountLocationNames = 7;
        private const int itemViewSubItemLinksCountKeywordTags = 8;
        private const int itemViewSubItemLinksCountRegions = 9;
        
        private const int itemViewSubItemDataSetDate = 0;
        private const int itemViewSubItemDataSetDataSetName = 1;
        private const int itemViewSubItemDataSetCountMediaFiles = 2;
        private const int itemViewSubItemDataSetCountTitles = 3;
        private const int itemViewSubItemDataSetCountAlbumNames = 5;
        private const int itemViewSubItemDataSetCountLocationNames = 5;
        private const int itemViewSubItemDataSetCountKeywordTags = 6;
        private const int itemViewSubItemDataSetCountRegions = 7;
        #endregion

        #region class - MetadataDataSetCount
        private class MetadataDataSetCount
        {
            public MetadataDataSetCount()
            {
            }

            [JsonProperty("MediaFiles")]
            public int MediaFiles { get; set; } = 0;

            [JsonProperty("Titles")]
            public int Titles { get; set; } = 0;

            [JsonProperty("AlbumNames")]
            public int AlbumNames { get; set; } = 0;

            [JsonProperty("LocationNames")]
            public int LocationNames { get; set; } = 0;

            [JsonProperty("KeywordTags")]
            public int KeywordTags { get; set; } = 0;

            [JsonProperty("Regions")]
            public int Regions { get; set; } = 0;

            public override string ToString()
            {
                return
                    "Media files found in DataSet: " + MediaFiles.ToString() + "\r\n" +
                    "Titles found: " + Titles.ToString() + "\r\n" +
                    "Album Names found: " + AlbumNames.ToString() + "\r\n" +
                    "Location Names found: " + LocationNames.ToString() + "\r\n" +
                    "Keyword Tags found: " + KeywordTags.ToString() + "\r\n" +
                    "Regions found: " + Regions.ToString() + "\r\n\r\n"; ;
            }

            public void Add(Metadata metadata)
            {
                if (metadata != null)
                {
                    MediaFiles++;
                    if (metadata.PersonalTitle != null) Titles++;
                    if (metadata.PersonalAlbum != null) AlbumNames++;
                    if (metadata.LocationName != null) LocationNames++;
                    KeywordTags += metadata.PersonalKeywordTags.Count;
                    Regions += metadata.PersonalRegionList.Count;
                }
            }

            public static MetadataDataSetCount CountMetadataDataSet(Dictionary<string, Metadata> metadataList)
            {
                MetadataDataSetCount metadataDataSetCount = new MetadataDataSetCount();
                foreach (Metadata metadata in metadataList.Values) metadataDataSetCount.Add(metadata);
                return metadataDataSetCount;
            }

            public override bool Equals(object obj)
            {
                return obj is MetadataDataSetCount count &&
                       MediaFiles == count.MediaFiles &&
                       Titles == count.Titles &&
                       AlbumNames == count.AlbumNames &&
                       LocationNames == count.LocationNames &&
                       KeywordTags == count.KeywordTags &&
                       Regions == count.Regions;
            }

            public override int GetHashCode()
            {
                int hashCode = -1214819291;
                hashCode = hashCode * -1521134295 + MediaFiles.GetHashCode();
                hashCode = hashCode * -1521134295 + Titles.GetHashCode();
                hashCode = hashCode * -1521134295 + AlbumNames.GetHashCode();
                hashCode = hashCode * -1521134295 + LocationNames.GetHashCode();
                hashCode = hashCode * -1521134295 + KeywordTags.GetHashCode();
                hashCode = hashCode * -1521134295 + Regions.GetHashCode();
                return hashCode;
            }

            public static bool operator ==(MetadataDataSetCount left, MetadataDataSetCount right)
            {
                return EqualityComparer<MetadataDataSetCount>.Default.Equals(left, right);
            }

            public static bool operator !=(MetadataDataSetCount left, MetadataDataSetCount right)
            {
                return !(left == right);
            }
        }



        #endregion

        #region class - WebScrapingLinks
        private class WebScrapingLinks
        {
            [JsonProperty("Name")]
            public string Name { get; set; } = "";

            [JsonProperty("Link")]
            public string Link { get; set; } = "";

            [JsonProperty("Category")]
            public string Category { get; set; } = "";

            [JsonProperty("LastRead")]
            public DateTime? LastRead { get; set; } = null;

            [JsonProperty("MetadataDataSetCount")]
            public MetadataDataSetCount MetadataDataSetCount { get; set; }
        }
        #endregion 

        #region class - WebScrapingDataSet
        private class WebScrapingDataSet
        {
            public WebScrapingDataSet()
            {
            }

            public WebScrapingDataSet(DateTime writtenDate)
            {
                WrittenDate = writtenDate;
            }

            [JsonProperty("WrittenDate")]
            public DateTime WrittenDate { get; set; }

            [JsonProperty("Description")]
            public string Description { get; set; } = "";

            [JsonProperty("MetadataDataSetCount")]
            public MetadataDataSetCount MetadataDataSetCount { get; set; }
        }
        #endregion 

        #region class - ScrapingResult
        public class ScrapingResult : IEquatable<ScrapingResult>
        {
            public string Url { get; set; } = null;
            public string Title { get; set; } = null;
            public string Description { get; set; } = null;
            public bool PictureInfoScreenHidden { get; set; } = true;
            public List<string> LinkPhoto { get; set; } = new List<string>();
            public string Album { get; set; } = null;
            public List<string> AlbumOthers { get; set; } = new List<string>();
            public string MediaFile { get; set; } = null;
            public string LocationName { get; set; } = null;
            public string Tag { get; set; } = null;
            public List<string> Tags { get; set; } = new List<string>();
            public List<string> Peoples { get; set; } = new List<string>();
            public Dictionary<string, string> LinksAlbum = new Dictionary<string, string>();
            public Dictionary<string, string> LinksTags = new Dictionary<string, string>();
            public Dictionary<string, string> LinksPeople = new Dictionary<string, string>();
            public Dictionary<string, string> LinksLocation = new Dictionary<string, string>();

            public override bool Equals(object obj)
            {
                return Equals(obj as ScrapingResult);
            }

            public bool Equals(ScrapingResult other)
            {
                if (Url != other.Url) return false;
                if (Title != other.Title) return false;
                if (Description != other.Description) return false;
                if (PictureInfoScreenHidden != other.PictureInfoScreenHidden) return false;

                if (LinkPhoto.Count != other.LinkPhoto.Count) return false;
                for (int index = 0; index < LinkPhoto.Count; index++) if (LinkPhoto[index] != other.LinkPhoto[index])
                        return false;

                if (Album != other.Album) return false;

                if (AlbumOthers.Count != other.AlbumOthers.Count) return false;
                for (int index = 0; index < AlbumOthers.Count; index++) if (AlbumOthers[index] != other.AlbumOthers[index])
                        return false;


                if (MediaFile != other.MediaFile)
                    return false;
                if (LocationName != other.LocationName) return false;
                if (Tag != other.Tag) return false;

                if (Tags.Count != other.Tags.Count)
                    return false;
                for (int index = 0; index < Tags.Count; index++) if (Tags[index] != other.Tags[index])
                        return false;

                if (Peoples.Count != other.Peoples.Count)
                    return false;
                for (int index = 0; index < Peoples.Count; index++) if (Peoples[index] != other.Peoples[index])
                        return false;

                if (LinksAlbum.Count != other.LinksAlbum.Count) return false;
                foreach (string key in LinksAlbum.Keys) if (!other.LinksAlbum.ContainsKey(key) || LinksAlbum[key] != other.LinksAlbum[key])
                        return false;

                if (LinksTags.Count != other.LinksTags.Count) return false;
                foreach (string key in LinksTags.Keys) if (!other.LinksTags.ContainsKey(key) || LinksTags[key] != other.LinksTags[key]) return false;

                if (LinksPeople.Count != other.LinksPeople.Count) return false;
                foreach (string key in LinksPeople.Keys) if (!other.LinksPeople.ContainsKey(key) || LinksPeople[key] != other.LinksPeople[key]) return false;

                if (LinksLocation.Count != other.LinksLocation.Count) return false;
                foreach (string key in LinksLocation.Keys) if (!other.LinksLocation.ContainsKey(key) || LinksLocation[key] != other.LinksLocation[key]) return false;

                return true;

            }

            public override int GetHashCode()
            {
                throw new NotFiniteNumberException();
            }

            public static bool operator ==(ScrapingResult left, ScrapingResult right)
            {
                return EqualityComparer<ScrapingResult>.Default.Equals(left, right);
            }

            public static bool operator !=(ScrapingResult left, ScrapingResult right)
            {
                return !(left == right);
            }

        }
        #endregion


        #region IsProcessRunning
        private bool IsProcessRunning {
            get  { return isProcessRunning; }
            set
            {
                isProcessRunning = value;
                SetButtonState(!value);
                if (isProcessRunning)
                {
                    buttonWebScrapingStop.Enabled = true;
                    stopRequested = false;
                } else
                {                    
                    buttonWebScrapingStop.Enabled = false;
                    stopRequested = false;
                }
            }
        }
        #endregion 

        #region IsAnyPageLoaded
        private bool isAnyPageLoaded = true;
        private bool IsAnyPageLoaded
        {
            get { return isAnyPageLoaded; }
            set
            {
                if (isAnyPageLoaded != value) //Only updated on changes
                {
                    isAnyPageLoaded = value;
                    SetButtonState(value);
                }
            }
        }
        #endregion

        #region SetButtonState
        private void SetButtonState(bool enabled)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<bool>(SetButtonState), enabled);
                return;
            }

            buttonBrowserShowDevTool.Enabled = enabled;
            buttonRunJavaScript.Enabled = enabled;
            buttonSaveJavaScript.Enabled = enabled;
            buttonWebScrapingCategories.Enabled = enabled;
            buttonWebScrapingCategoriesStart.Enabled = enabled;
            buttonWebScrapingSearchStart.Enabled = enabled;
            buttonWebScrapingSave.Enabled = enabled;
            buttonWebScrapingClearDataSet.Enabled = enabled;
            kryptonButtonWebScrapingAddUserTags.Enabled = enabled;
            buttonWebScrapingExportDataSet.Enabled = enabled;
            buttonWebScrapingImportDataSet.Enabled = enabled;

            buttonWebScrapingLoadPackage.Enabled = enabled && (listViewDataSetDates.Items.Count > 0);
            listViewDataSetDates.Enabled = enabled && (listViewDataSetDates.Items.Count > 0);            
        }
        #endregion

        #region Form 

        #region Form - Init
        public FormWebScraper()
        {
            InitializeComponent();
            IsAnyPageLoaded = false;

            autoResetEventWaitPageLoaded = new AutoResetEvent(false);
            autoResetEventWaitPageLoading = new AutoResetEvent(false);

            listViewColumnSorterLinks = new ListViewColumnSorter();
            listViewLinks.ListViewItemSorter = listViewColumnSorterLinks;
            listViewColumnSorterDataSet = new ListViewColumnSorter();
            listViewDataSetDates.ListViewItemSorter = listViewColumnSorterDataSet;
            
            if (!GlobalData.isRunningWinSmode)
            {
                browser = new ChromiumWebBrowser("https://photos.google.com/") { Dock = DockStyle.Fill, };
                browser.BrowserSettings.Javascript = CefState.Enabled;
                //browser.BrowserSettings.WebSecurity = CefState.Enabled;
                browser.BrowserSettings.WebGl = CefState.Enabled;
                //browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Disabled;
                //browser.BrowserSettings.Plugins = CefState.Enabled;
                this.panelBrowser.Controls.Add(this.browser);

                browser.AddressChanged += Browser_AddressChanged;
                browser.FrameLoadEnd += Browser_FrameLoadEnd;
                browser.LoadingStateChanged += Browser_LoadingStateChanged;
            }

            fastColoredTextBoxJavaScript.Text = Properties.Settings.Default.WebScraperScript;

            javaScriptExecuteTimeout = Properties.Settings.Default.JavaScriptExecuteTimeout;
            waitEventPageLoadedTimeout = Properties.Settings.Default.WaitEventPageLoadedTimeout;
            waitEventPageStartLoadingTimeout = Properties.Settings.Default.WaitEventPageStartLoadingTimeout;
            //textBox.Text = Properties.Settings.Default.WebScraperScript;
            webScrapingStartPages = Properties.Settings.Default.WebScraperStartPages.Replace("\r", "").Split('\n');
            webScrapingDelayInPageScriptToRun = Properties.Settings.Default.WebScrapingDelayInPageScriptToRun;
            webScrapingDelayOurScriptToRun = Properties.Settings.Default.WebScrapingDelayOurScriptToRun;
            webScrapingPageDownCount = Properties.Settings.Default.WebScrapingPageDownCount;
            webScrapingRetry = Properties.Settings.Default.WebScrapingRetry;
            kryptonTextBoxWebScrapingSearch.Text = Properties.Settings.Default.WebScraperSearch;
        }
        #endregion 

        #region Form - Load
        private void FormWebScraper_Load(object sender, EventArgs e)
        {
            //DatabaseAndCacheMetadataExiftool.OnReadRecord -= DatabaseAndCacheMetadataExiftool_OnReadRecord;
            DatabaseAndCacheMetadataExiftool.OnRecordReadToCacheParameter += DatabaseAndCacheMetadataExiftool_OnReadRecord;

            List<DateTime> webScrapingDataSetDates = DatabaseAndCacheMetadataExiftool.ListWebScraperDataSet(MetadataBrokerType.WebScraping, webScrapingName);
            _webScrapingDataSet = WebScrapingDataSetStatusRead(webScrapingDataSetDates);
            UpdatedWebScrapingDataSetList(_webScrapingDataSet);

            _linkCatergories = WebScrapingLinksStatusRead();
            CategryLinksShowInListView(_linkCatergories);

            listViewColumnSorterDataSet.SortColumn = 0;
            listViewColumnSorterDataSet.Order = SortOrder.Descending;
            listViewDataSetDates.Sort();
        }
        #endregion 

        #region Form - Shown
        private void FormWebScraper_Shown(object sender, EventArgs e)
        {
            if (_webScrapingDataSet.Count > 0)
            {
                DateTime dataSetDateTime = DateTime.MinValue;
                foreach (DateTime dateTime in _webScrapingDataSet.Keys) if (dateTime > dataSetDateTime) dataSetDateTime = dateTime;

                if (KryptonMessageBox.Show(
                    "To contine expand last dataset with new scraping data, you can load lastest version of saved dataset now. " + 
                    "You can also load and merge any version of metadata dataset later also within 'the WebScraping Tool window'.\r\n\r\n" + 
                    "Do you want to load last dataset with metadatas? \r\n\r\n" +
                    "Click yes, if you want continue expant last dataset of metadatas and merge with new scraping data.\r\n"+
                    "Click no, if you want to start with emoty dataset of scraping metadata.", 
                    "Load last metadata dataset?", (KryptonMessageBoxButtons)MessageBoxButtons.YesNo, KryptonMessageBoxIcon.Question, showCtrlCopy: true) == DialogResult.Yes)
                {

                    IsProcessRunning = true;
                    buttonWebScrapingStop.Enabled = false;

                    Dictionary<string, Metadata> metadataDictionaryLoaded = LoadDataSetFromDatabase(dataSetDateTime);
                    MetadataDictionaryMerge(_metadataDataTotalMerged, metadataDictionaryLoaded);

                    MetadataDataSetCount metadataDataSetCountLoaded = MetadataDataSetCount.CountMetadataDataSet(metadataDictionaryLoaded);

                    fastColoredTextBoxJavaScriptResult.Text = "DataSet loaded...\r\n";
                    fastColoredTextBoxJavaScriptResult.Text += "Count for this DataSet: " + dataSetDateTime.ToString() + "\r\n";
                    fastColoredTextBoxJavaScriptResult.Text += metadataDataSetCountLoaded.ToString();

                    AddDataSetDesciption("Database");
                    IsProcessRunning = false;
                }
            }
        }
        #endregion 

        #region Form - Closing
        private void FormWebScraper_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.WebScraperSearch = kryptonTextBoxWebScrapingSearch.Text;
            Properties.Settings.Default.WebScraperScript = fastColoredTextBoxJavaScript.Text;
            if (IsProcessRunning)
            {
                KryptonMessageBox.Show("Need wait process that are running has stopped, before closing window", "Process still working...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Information, showCtrlCopy: true);
                e.Cancel = true;
            }
            if (isDataUnsaved)
            {
                if (KryptonMessageBox.Show("DataSet is unsaved! Will you close widthout saving data?", "Warning, unsaved DataSet", (KryptonMessageBoxButtons)MessageBoxButtons.OKCancel, KryptonMessageBoxIcon.Warning, showCtrlCopy: true) == DialogResult.Cancel) e.Cancel = true;
            }
        }
        #endregion

        #endregion

        #region Browser

        #region Browser - LoadingStateChanged
        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading == false) IsAnyPageLoaded = true;
            if (e.IsLoading == true && autoResetEventWaitPageLoading != null) autoResetEventWaitPageLoading.Set();
            if (e.IsLoading == false && autoResetEventWaitPageLoaded != null) autoResetEventWaitPageLoaded.Set();
        }
        #endregion

        #region Browser - FrameLoadEnd
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
        }
        #endregion

        #region Browser - AddressChanged
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

        #region Browser - SendKey
        private void SendKeyBrowser(Keys key)
        {
            //KeyEvent for Right-arrow
            KeyEvent keyEvent = new KeyEvent();
            keyEvent.WindowsKeyCode = (int)key;
            keyEvent.FocusOnEditableField = true;
            keyEvent.IsSystemKey = false;
            keyEvent.Type = KeyEventType.KeyDown;

            browser.GetBrowser().GetHost().SendKeyEvent(keyEvent);
        }
        #endregion

        #region Browser - Wait Page Loaded Event
        private bool WaitPageLoadedEvent(bool sleep = true)
        {
            bool result = autoResetEventWaitPageLoaded.WaitOne(waitEventPageLoadedTimeout);
            Application.DoEvents();
            if (sleep) 
                Task.Delay(webScrapingDelayInPageScriptToRun).Wait();
            return result;
        }
        #endregion

        #region Browser - GUI - textBoxBrowserURL_KeyPress
        private void textBoxBrowserURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; //Handle the Keypress event (suppress the Beep)
                browser.Load(textBoxBrowserURL.Text);
            }
        }
        #endregion

        #region Broswer - GUI - buttonBrowserShowDevTool_Click
        private void buttonBrowserShowDevTool_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }
        #endregion

        #endregion


        #region GUI - ShowResult(List<object>list) -> fastColoredTextBoxJavaScriptResult
        private void ShowResult(List<object> list)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<List<object>>(ShowResult), list);
                return;
            }

            fastColoredTextBoxJavaScriptResult.SuspendLayout();

            string testString = "";
            foreach (object tag in list)
            {
                if (tag is System.Dynamic.ExpandoObject)
                {
                    foreach (KeyValuePair<string, object> keyValuePair in (System.Dynamic.ExpandoObject)tag)
                    {
                        testString += "Key: " + keyValuePair.Key.ToString() + "\r\nValue: " + (keyValuePair.Value == null ? "(null)" : keyValuePair.Value.ToString()) + "\r\n";
                    }
                }
                else if (tag is List<object>)
                {
                    foreach (object item in (List<object>)tag) testString += item.ToString() + "; ";
                    testString += "\r\n";
                }
                else if (tag is string) testString += tag.ToString() + "\r\n";

            }
            fastColoredTextBoxJavaScriptResult.Text = testString;
            fastColoredTextBoxJavaScriptResult.ResumeLayout();
        }
        #endregion

        #region GUI - Run JavaScript
        private async void buttonRunJavaScript_Click(object sender, EventArgs e)
        {
            IsProcessRunning = true;
            fastColoredTextBoxJavaScriptResult.SuspendLayout();
            fastColoredTextBoxJavaScriptResult.Text = "Running script...";
            fastColoredTextBoxJavaScriptResult.ResumeLayout();

            try
            {
                object result = await EvaluateScript(fastColoredTextBoxJavaScript.Text, null, TimeSpan.FromMilliseconds(javaScriptExecuteTimeout));

                if (result is List<object>) ShowResult((List<object>)result);
                else fastColoredTextBoxJavaScriptResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Running JavaScript failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);

            }
            fastColoredTextBoxJavaScriptResult.ResumeLayout();
            IsProcessRunning = false;
        }
        #endregion

        #region GUI - Save JavaScript
        private void buttonSaveJavaScript_Click(object sender, EventArgs e)
        {
            IsProcessRunning = true;
            try
            {
                Properties.Settings.Default.WebScraperScript = fastColoredTextBoxJavaScript.Text;
            } catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Can't save settings...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            IsProcessRunning = false;
        }
        #endregion

        #region JavaScript - EvaluateScript 
        public async Task<object> EvaluateScript(string script, object defaultValue, TimeSpan timeout)
        {
            object result = defaultValue;
            if (browser.IsBrowserInitialized && !browser.IsDisposed && !browser.Disposing)
            {
                try
                {
                    var task = browser.EvaluateScriptAsync(script, timeout);
                    await task.ContinueWith(res => {
                        if (!res.IsFaulted && !res.IsCanceled)
                        {
                            var response = res.Result;
                            result = response.Success ? (response?.Result ?? "null") : response.Message;
                        }
                    }).ConfigureAwait(false); // <-- This makes the task to synchronize on a different context
                }
                catch (Exception e)
                {
                    Logger.Error(e.InnerException.Message);
                }
            }
            return result;
        }
        #endregion

        #region Scraping - ConvertScrapingListToClass
        private ScrapingResult ConvertScrapingListToClass(List<object> list)
        {

            ScrapingResult scrapingResult = new ScrapingResult();

            foreach (object tagObject in list)
            {
                if (tagObject is List<object> tagList)
                {
                    string key = tagList[0].ToString();
                    string value = tagList[1].ToString();
                    string param = (tagList.Count > 2 ? tagList[2].ToString() : null);
                    switch (key)
                    {
                        case "url":
                            scrapingResult.Url = value;
                            break;
                        case "title":
                            scrapingResult.Title = value;
                            break;
                        case "picture info screen":
                            if (value == "none") scrapingResult.PictureInfoScreenHidden = true;
                            else scrapingResult.PictureInfoScreenHidden = false;
                            break;
                        case "photo link":
                            if (!scrapingResult.LinkPhoto.Contains(param)) scrapingResult.LinkPhoto.Add(param);
                            break;
                        case "album":
                            scrapingResult.Album = tagList[1].ToString();
                            break;
                        case "album other":
                            if (!scrapingResult.AlbumOthers.Contains(value)) scrapingResult.AlbumOthers.Add(value);
                            break;

                        case "album link":
                            if (!scrapingResult.LinksAlbum.ContainsKey(param)) scrapingResult.LinksAlbum.Add(param, value);
                            break;
                        case "tag link":
                            if (!scrapingResult.LinksTags.ContainsKey(param)) scrapingResult.LinksTags.Add(param, value);
                            break;
                        case "location link":
                            if (!scrapingResult.LinksLocation.ContainsKey(param)) scrapingResult.LinksLocation.Add(param, value);
                            break;
                        case "people link":
                            if (!scrapingResult.LinksPeople.ContainsKey(param)) scrapingResult.LinksPeople.Add(param, value);
                            break;


                        case "mediafile":
                            scrapingResult.MediaFile = value;
                            break;
                        case "description":
                            scrapingResult.Description = value;
                            break;
                        case "tag":
                            if (!scrapingResult.Tags.Contains(value)) scrapingResult.Tags.Add(value);
                            break;
                        case "people":
                            if (!scrapingResult.Peoples.Contains(value)) scrapingResult.Peoples.Add(value);
                            break;
                            /*default:
                                throw new NotImplementedException();*/
                    }
                }
            }

            return scrapingResult;
        }
        #endregion 

        #region Scraping - EvaluateScriptWithScraping (script, retry) - async
        private async Task<ScrapingResult> EvaluateScriptWithScraping(string script, int retryWhenVerifyFails, bool verifyDiffrentResult, bool verifyPhotoLinksCount, bool verifyMediaFileFound)
        {
            bool newFound = true;
            ScrapingResult scrapingResult = null;
            ScrapingResult lastScrapingResult = null;

            do
            {
                Task.Delay(webScrapingDelayOurScriptToRun).Wait(); //Give script some time to run

                try
                {
                    object result = await EvaluateScript(script, null, TimeSpan.FromMilliseconds(javaScriptExecuteTimeout));
                    if (result is List<object>)
                    {
                        scrapingResult = ConvertScrapingListToClass((List<object>)result);
                    }
                    else
                    {
                        Logger.Error("EvaluateScript: " + (result == null ? "null" : result.ToString()));
                        newFound = false;
                    }

                    
                    if (verifyDiffrentResult)
                    {
                        if (lastScrapingResult != scrapingResult)
                            newFound = true;
                        else
                            newFound = false;
                    }

                    if (scrapingResult != null)
                    {
                        if (verifyPhotoLinksCount && scrapingResult.LinkPhoto.Count <= 0) newFound = false;
                        if (verifyMediaFileFound && scrapingResult.MediaFile == null) newFound = false;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Task<ScrapingResult> Scraping: ");
                    newFound = false;
                    retryWhenVerifyFails = 0;
                }

                lastScrapingResult = scrapingResult;
            } while (!newFound && retryWhenVerifyFails-- > 0);

            return scrapingResult;
        }
        #endregion

        #region Scraping - Media Files in URL
        private async Task<Dictionary<string, Metadata>> ScrapingMediaFiles(string script, string url, string tag, string album, string people, string location, 
            int retryWhenVerifyFails, List<string> urlLoadingFailed)
        {   
            Dictionary<string, Metadata> metadataDictionary = new Dictionary<string, Metadata>();

            browser.Load(url);

            WaitPageLoadedEvent();
            
            ScrapingResult scrapingResult = await EvaluateScriptWithScraping(script, retryWhenVerifyFails, true, true, false);

            if (scrapingResult != null && scrapingResult.LinkPhoto.Count>0)
            {
                browser.Load(scrapingResult.LinkPhoto[0]);
                WaitPageLoadedEvent();

                bool isPageLoading;
                do
                {
                    isPageLoading = false;
                    scrapingResult = await EvaluateScriptWithScraping(script, retryWhenVerifyFails, true, false, true);

                    if (scrapingResult != null && scrapingResult.PictureInfoScreenHidden)
                    {
                        SendKeyBrowser(Keys.I);
                        Task.Delay(webScrapingDelayInPageScriptToRun).Wait();
                        scrapingResult = await EvaluateScriptWithScraping(script, retryWhenVerifyFails, true, false, true);
                    }

                    if (scrapingResult?.MediaFile == null && scrapingResult?.Url != null)
                    {
                        browser.Load(scrapingResult?.Url);
                        WaitPageLoadedEvent();
                        scrapingResult = await EvaluateScriptWithScraping(script, retryWhenVerifyFails, true, false, true);
                    }

                    if (scrapingResult?.MediaFile != null)
                    {
                        if (!metadataDictionary.ContainsKey(scrapingResult.MediaFile))
                            metadataDictionary.Add(scrapingResult.MediaFile, new Metadata(MetadataBrokerType.WebScraping));

                        Metadata metadata = metadataDictionary[scrapingResult.MediaFile];

                        metadata.FileName = scrapingResult.MediaFile;
                        metadata.FileDirectory = scrapingResult.Url;
                        metadata.FileDateCreated = DateTime.Now;
                        metadata.FileDateModified = DateTime.Now;
                        metadata.FileDateAccessed = DateTime.Now;

                        if (scrapingResult.Description != null) metadata.PersonalTitle = scrapingResult.Description;
                        
                        //Album
                        if (album != null) metadata.PersonalAlbum = album;
                        if (metadata.PersonalAlbum == null && scrapingResult.AlbumOthers.Count > 0) metadata.PersonalAlbum = scrapingResult.AlbumOthers[0];

                        //Add Album to keyword tag
                        if (album != null) metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(album, 0.95f));
                        foreach (string albumName in scrapingResult.AlbumOthers) metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(albumName, 0.95f));

                        //Title
                        if (scrapingResult.Description != null) 
                            metadata.PersonalTitle = scrapingResult.Description;
                        
                        //Location
                        if (scrapingResult.LocationName != null) metadata.LocationName = scrapingResult.LocationName;
                        if (location != null) metadata.LocationName = location;

                        //Keyword
                        if (scrapingResult.Tag != null) metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(scrapingResult.Tag, 0.95f));
                        if (tag != null) metadata.PersonalKeywordTagsAddIfNotExists(new KeywordTag(tag, 0.95f));
                        
                        //Region Face / People
                        if (people != null) metadata.PersonalRegionListAddIfNameNotExists(new RegionStructure(people, "Face", 0, 0, 1, 1, RegionStructureTypes.WindowsLivePhotoGallery));
                        foreach (string peopleName in scrapingResult.Peoples)
                            metadata.PersonalRegionListAddIfNameNotExists(new RegionStructure(peopleName, "Face", 0, 0, 1, 1, RegionStructureTypes.WindowsLivePhotoGallery));
                    } else
                    {
                        urlLoadingFailed.Add((scrapingResult?.Url == null ? "" : scrapingResult?.Url));
                        Logger.Warn("Failed to load Url: " + (scrapingResult?.Url == null ? "" : scrapingResult?.Url));
                    }


                    SendKeyBrowser(Keys.Right);
                    if (autoResetEventWaitPageLoading.WaitOne(waitEventPageStartLoadingTimeout)) isPageLoading = true;
                } while (!stopRequested && isPageLoading && WaitPageLoadedEvent(false));
            }

            return metadataDictionary; 
        }
        #endregion

        #region Scraping - CategoryLinks - AddUpdated
        private void CategoryLinksAddUpdate(Dictionary<string, WebScrapingLinks> linkCatergories, Dictionary<string, string> links, string categryName)
        {
            foreach (KeyValuePair<string, string> keyValuePair in links)
            {
                WebScrapingLinks linkCategory;
                if (!linkCatergories.ContainsKey(keyValuePair.Key)) linkCatergories.Add(keyValuePair.Key, new WebScrapingLinks());
                linkCategory = linkCatergories[keyValuePair.Key];
                linkCategory.Category = categryName;
                linkCategory.Link = keyValuePair.Key;
                linkCategory.Name = keyValuePair.Value;
                //linkCategory.LastRead = null;
            }
        }
        #endregion 

        #region Scraping - CategoryLinks
        private async Task<ScrapingResult> ScrapingCategoryLinks(string script, string url, Dictionary<string, WebScrapingLinks> linkCatergories)            
        {
            browser.Load(url);
            WaitPageLoadedEvent();

            int scrollPageDownCount = webScrapingPageDownCount;
            bool newFound;
            ScrapingResult scrapingResult;
            ScrapingResult lastScrapingResult = null;

            do {
                scrapingResult = await EvaluateScriptWithScraping(script, webScrapingRetry, true, false, false);

                if (scrapingResult != null)
                {                    
                    CategoryLinksAddUpdate(linkCatergories, scrapingResult.LinksAlbum, "Album");
                    CategoryLinksAddUpdate(linkCatergories, scrapingResult.LinksTags, "Tag");
                    CategoryLinksAddUpdate(linkCatergories, scrapingResult.LinksPeople, "People");
                    CategoryLinksAddUpdate(linkCatergories, scrapingResult.LinksLocation, "Location");
                }

                SendKeyBrowser(Keys.PageDown);

                if (lastScrapingResult != scrapingResult)
                {
                    newFound = true;
                    scrollPageDownCount = webScrapingPageDownCount;
                }                    
                else
                {
                    newFound = false;
                    scrollPageDownCount--;
                }
                lastScrapingResult = scrapingResult;

            } while (!stopRequested && (newFound || (!newFound && scrollPageDownCount > 0)));
            
            return scrapingResult;
        }
        #endregion

        #region GUI - Show CategryLinks - Updated ListView
        private void CategryLinksShowInListView(Dictionary<string, WebScrapingLinks> linkCatergories)
        {
            foreach (WebScrapingLinks linkCategory in linkCatergories.Values)
            {
                if (!string.IsNullOrWhiteSpace(linkCategory.Name))
                {
                    int itemFoundIndex = -1;
                    for (int itemIndex = 0; itemIndex < listViewLinks.Items.Count; itemIndex++)
                    {
                        if (listViewLinks.Items[itemIndex].SubItems[itemViewSubItemLinksLink].Text == linkCategory.Link)
                        {
                            itemFoundIndex = itemIndex;
                            break;
                        }
                    }

                    if (itemFoundIndex == -1)
                    {
                        ListViewItem listViewItem = listViewLinks.Items.Add(linkCategory.Name);
                        listViewItem.SubItems.Add(linkCategory.Category);
                        listViewItem.SubItems.Add(linkCategory.Link);
                        ListViewItem.ListViewSubItem listViewItemLastRead = listViewItem.SubItems.Add(linkCategory.LastRead.ToString());
                        listViewItemLastRead.Tag = linkCategory.LastRead;

                        listViewItem.SubItems.Add(linkCategory.MetadataDataSetCount?.MediaFiles.ToString());
                        listViewItem.SubItems.Add(linkCategory.MetadataDataSetCount?.Titles.ToString());
                        listViewItem.SubItems.Add(linkCategory.MetadataDataSetCount?.AlbumNames.ToString());
                        listViewItem.SubItems.Add(linkCategory.MetadataDataSetCount?.LocationNames.ToString());
                        listViewItem.SubItems.Add(linkCategory.MetadataDataSetCount?.KeywordTags.ToString());
                        listViewItem.SubItems.Add(linkCategory.MetadataDataSetCount?.Regions.ToString());
                    }
                    else
                    {
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksCategoryName].Text = linkCategory.Name;
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksCategoryType].Text = linkCategory.Category;
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksLink].Text = linkCategory.Link;
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksReadDate].Text = linkCategory.LastRead.ToString();
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksReadDate].Tag = linkCategory.LastRead;

                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksCountMediaFiles].Text = linkCategory.MetadataDataSetCount?.MediaFiles.ToString();
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksCountTitles].Text = linkCategory.MetadataDataSetCount?.Titles.ToString(); 
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksCountAlbumNames].Text = linkCategory.MetadataDataSetCount?.AlbumNames.ToString();
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksCountLocationNames].Text = linkCategory.MetadataDataSetCount?.LocationNames.ToString();
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksCountKeywordTags].Text = linkCategory.MetadataDataSetCount?.KeywordTags.ToString();
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLinksCountRegions].Text = linkCategory.MetadataDataSetCount?.Regions.ToString();
                    }
                        
                    
                }
            }
        }
        #endregion 

        #region GUI - Start Scraping - CategoryLinks
        private async void WebScrapingCategoryLinks_Click(object sender, EventArgs e)        
        {
            IsProcessRunning = true;
            try
            {                
                
                listViewLinks.Items.Clear();

                string script = Properties.Settings.Default.WebScraperScript;

                foreach (string webPage in webScrapingStartPages) if (!stopRequested) _ = await ScrapingCategoryLinks(script, webPage.Trim(), _linkCatergories);

                CategryLinksShowInListView(_linkCatergories);
                WebScrapingLinksStatusWrites(_linkCatergories);
            } catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "WebScraping failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            IsProcessRunning = false;
        }
        #endregion

        #region json - WebScraping - Filename
        private string WebScrapingFilename(string filename)
        {
            try
            {
                return FileHandler.GetLocalApplicationDataPath(filename, deleteOldTempFile: false);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Get LocalApplicationDataPath failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            return null;
        }
        #endregion

        #region json - WebScraping - Links Status - Read
        private Dictionary<string, WebScrapingLinks> WebScrapingLinksStatusRead()
        {
            Dictionary<string, WebScrapingLinks> result = new Dictionary<string, WebScrapingLinks>();
            string filename = WebScrapingFilename("Status.WebScraping.Links.json");
            try
            {
                if (File.Exists(filename))
                {
                    result = JsonConvert.DeserializeObject<Dictionary<string, WebScrapingLinks>>(File.ReadAllText(filename));
                }
            } catch (Exception ex) 
            {
                KryptonMessageBox.Show(ex.Message, "DeserializeObject failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            return result;
        }
        #endregion

        #region json - WebScraping - Links Status - Write
        private void WebScrapingLinksStatusWrites(Dictionary<string, WebScrapingLinks> linkCatergories)
        {
            string filename = WebScrapingFilename("Status.WebScraping.Links.json");
            try
            {
                File.WriteAllText(filename, JsonConvert.SerializeObject(linkCatergories, Formatting.Indented));
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "SerializeObject failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region json - WebScraping - DataSet Status - Read
        private Dictionary<DateTime, WebScrapingDataSet> WebScrapingDataSetStatusRead(List<DateTime> dataSetDateTimes)
        {
            Dictionary<DateTime, WebScrapingDataSet> result = new Dictionary<DateTime, WebScrapingDataSet>();
            string filename = WebScrapingFilename("Status.WebScraping.DataSet.json");
            try
            {
                Dictionary<DateTime, WebScrapingDataSet> oldStatus = new Dictionary<DateTime, WebScrapingDataSet>();
                if (File.Exists(filename)) oldStatus = JsonConvert.DeserializeObject<Dictionary<DateTime, WebScrapingDataSet>>(File.ReadAllText(filename));
                
                foreach (DateTime dateTime in dataSetDateTimes)
                {
                    //Add only when exist in database
                    if (oldStatus.ContainsKey(dateTime)) result.Add(dateTime, oldStatus[dateTime]);
                    else result.Add(dateTime, new WebScrapingDataSet(dateTime));
                } 
                
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "DeserializeObject failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            return result;
        }
        #endregion

        #region json - WebScraping - DataSet Status - Write
        private void WebScrapingDataSetStatusWrite(Dictionary<DateTime, WebScrapingDataSet> webScrapingDataSet)
        {
            string filename = WebScrapingFilename("Status.WebScraping.DataSet.json");
            try
            {
                SetButtonState(enabled: false);
                File.WriteAllText(filename, JsonConvert.SerializeObject(webScrapingDataSet, Formatting.Indented));
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "SerializeObject and save failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                SetButtonState(enabled: true);
            }
        }
        #endregion

        #region Delete Link Categories
        private void buttonSpecNavigatorCategoriesDeleteSelected_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtonState(enabled: false);
                List<ListViewItem> listViewItemsToDelete = new List<ListViewItem>();
                foreach (ListViewItem listViewItem in listViewLinks.Items)
                {
                    if (listViewItem.Checked) listViewItemsToDelete.Add(listViewItem);
                }
                foreach (ListViewItem listViewItem in listViewItemsToDelete)
                {
                    if (_linkCatergories.ContainsKey(listViewItem.SubItems[itemViewSubItemLinksLink].Text))
                    {
                        _linkCatergories.Remove(listViewItem.SubItems[itemViewSubItemLinksLink].Text);
                    }
                    listViewLinks.Items.Remove(listViewItem);
                }

                WebScrapingLinksStatusWrites(_linkCatergories); //Updated LastRead field
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error occured!\r\n\r\n" + ex.Message, "Error occured", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                SetButtonState(enabled: true);
            }
        }
        #endregion

        #region GUI - Start Scraping - Search
        private async void buttonWebScrapingSearchStart_Click(object sender, EventArgs e)
        {
            IsProcessRunning = true;

            try
            {
                string script = fastColoredTextBoxJavaScript.Text;

                _urlLoadingFailed.Clear();
                MetadataDataSetCount metadataDataSetCountBefore = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);
                fastColoredTextBoxJavaScriptResult.Text = "WebScarping stareted...\r\n" + metadataDataSetCountBefore.ToString() + "\r\n--------------------------------------\r\n\r\n";

                string lastScan = Properties.Settings.Default.WebScraperSearchLastScan;
                string searchText = kryptonTextBoxWebScrapingSearch.Text.Replace("{LastScan:yyyy-MM-dd}", lastScan);
                string url = Properties.Settings.Default.WebScraperSearchUrl;
                url = url + (url.EndsWith("\\") || url.EndsWith("/") || searchText.StartsWith("\\") || searchText.StartsWith("/") ? "" : "/") + HttpUtility.UrlPathEncode(searchText);


                Dictionary<string, Metadata> metadataDateSetScraped = null;
                

                metadataDateSetScraped = await ScrapingMediaFiles(script, url, null, null, null, null, webScrapingRetry, _urlLoadingFailed);
                AddDataSetDesciption("Search");

                MetadataDictionaryMerge(_metadataDataTotalMerged, metadataDateSetScraped);
                MetadataDataSetCount metadataDataSetCountRead = MetadataDataSetCount.CountMetadataDataSet(metadataDateSetScraped);
                fastColoredTextBoxJavaScriptResult.Text += "Scraping result " + kryptonTextBoxWebScrapingSearch.Text + " :\r\n" + metadataDataSetCountRead.ToString() + "\r\n";

                MetadataDataSetCount metadataDataSetCountMerged = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);
                if (metadataDataSetCountBefore != metadataDataSetCountMerged) isDataUnsaved = true; //If changes found

                WebScrapingLinksStatusWrites(_linkCatergories); //Updated LastRead field

                fastColoredTextBoxJavaScriptResult.Text = "WebScarping done...\r\n" +
                    metadataDataSetCountMerged.ToString() +
                    "\r\n--------------------------------------\r\n\r\n" +
                    fastColoredTextBoxJavaScriptResult.Text;

                if (_urlLoadingFailed.Count > 0) fastColoredTextBoxJavaScriptResult.Text += "Urls failed to load: " + _urlLoadingFailed.Count + "\r\n";
                foreach (string failedUrl in _urlLoadingFailed) fastColoredTextBoxJavaScriptResult.Text += "Regions found: " + failedUrl + "\r\n";

                Properties.Settings.Default.WebScraperSearchLastScan = DateTime.Now.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Start WebScraping failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }

            IsProcessRunning = false;
        }
        #endregion

        #region GUI - Start Scraping - Selected Media Files
        private async void buttonWebScrapingCategoriesStart_Click(object sender, EventArgs e)
        {
            IsProcessRunning = true;
            
            try
            {
                string script = fastColoredTextBoxJavaScript.Text;

                _urlLoadingFailed.Clear();
                MetadataDataSetCount metadataDataSetCountBefore = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);
                fastColoredTextBoxJavaScriptResult.Text = "WebScarping stareted...\r\n" +
                    metadataDataSetCountBefore.ToString() + "\r\n--------------------------------------\r\n\r\n";

                foreach (ListViewItem listViewItem in listViewLinks.Items)
                {
                    Dictionary<string, Metadata> metadataDateSetScraped = null;

                    if (listViewItem.Checked)
                    {
                        string url = listViewItem.SubItems[itemViewSubItemLinksLink].Text;

                        
                        if (_linkCatergories.ContainsKey(url))
                        {

                            WebScrapingLinks linkCategory = _linkCatergories[url];
                            switch (linkCategory.Category)
                            {
                                case "Tag":
                                    metadataDateSetScraped = await ScrapingMediaFiles(script, linkCategory.Link, linkCategory.Name, null, null, null, webScrapingRetry,  _urlLoadingFailed);
                                    break;
                                case "Album":
                                    metadataDateSetScraped = await ScrapingMediaFiles(script, linkCategory.Link, null, linkCategory.Name, null, null, webScrapingRetry, _urlLoadingFailed);
                                    break;
                                case "People":
                                    metadataDateSetScraped = await ScrapingMediaFiles(script, linkCategory.Link, null, null, linkCategory.Name, null, webScrapingRetry, _urlLoadingFailed);
                                    break;
                                case "Location":
                                    metadataDateSetScraped = await ScrapingMediaFiles(script, linkCategory.Link, null, null, null, linkCategory.Name, webScrapingRetry, _urlLoadingFailed);
                                    break;
                                case "Search":
                                    metadataDateSetScraped = await ScrapingMediaFiles(script, linkCategory.Link, linkCategory.Name, null, null, null, webScrapingRetry, _urlLoadingFailed);
                                    break; 
                                default:
                                    throw new NotSupportedException();
                            }

                            if (stopRequested) break;

                            AddDataSetDesciption(linkCategory.Name);

                            linkCategory.LastRead = DateTime.Now;
                            linkCategory.MetadataDataSetCount = MetadataDataSetCount.CountMetadataDataSet(metadataDateSetScraped);
                            CategryLinksShowInListView(_linkCatergories); //Updated LastRead field

                            MetadataDictionaryMerge(_metadataDataTotalMerged, metadataDateSetScraped);
                            MetadataDataSetCount metadataDataSetCountRead = MetadataDataSetCount.CountMetadataDataSet(metadataDateSetScraped);
                            fastColoredTextBoxJavaScriptResult.Text += "Scraping result " + linkCategory.Name + " :\r\n" + metadataDataSetCountRead.ToString() + "\r\n";
                        }
                    }
                }

                
                MetadataDataSetCount metadataDataSetCountMerged = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);

                if (metadataDataSetCountBefore != metadataDataSetCountMerged) 
                    isDataUnsaved = true; //If changes found

                WebScrapingLinksStatusWrites(_linkCatergories); //Updated LastRead field

                fastColoredTextBoxJavaScriptResult.Text = "WebScarping done...\r\n" +
                    metadataDataSetCountMerged.ToString() + 
                    "\r\n--------------------------------------\r\n\r\n" +
                    fastColoredTextBoxJavaScriptResult.Text;

                if (_urlLoadingFailed.Count > 0) fastColoredTextBoxJavaScriptResult.Text += "Urls failed to load: " + _urlLoadingFailed.Count + "\r\n";
                foreach (string failedUrl in _urlLoadingFailed) fastColoredTextBoxJavaScriptResult.Text += "Regions found: " + failedUrl + "\r\n";
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Start WebScaring categories failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }

            IsProcessRunning = false;
        }
        #endregion

        #region GUI - List View Sort
        private ListViewColumnSorter listViewColumnSorterLinks;
        private ListViewColumnSorter listViewColumnSorterDataSet;

        private void listViewLinks_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == listViewColumnSorterLinks.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (listViewColumnSorterLinks.Order == SortOrder.Ascending) listViewColumnSorterLinks.Order = SortOrder.Descending;
                else listViewColumnSorterLinks.Order = SortOrder.Ascending;                
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                listViewColumnSorterLinks.SortColumn = e.Column;
                listViewColumnSorterLinks.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            ((ListView)sender).Sort();
        }

        private void listViewDataSet_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == listViewColumnSorterDataSet.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (listViewColumnSorterDataSet.Order == SortOrder.Ascending) listViewColumnSorterDataSet.Order = SortOrder.Descending;
                else listViewColumnSorterDataSet.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                listViewColumnSorterDataSet.SortColumn = e.Column;
                listViewColumnSorterDataSet.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            ((ListView)sender).Sort();
        }
        #endregion

        #region GUI - Select all, none, toggle in ListView
        private void buttonSpecNavigatorCategoriesSelectEmpty_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewLinks.Items) listViewItem.Checked = !(listViewItem.SubItems[itemViewSubItemLinksReadDate].Tag is DateTime);
        }

        private void buttonSpecNavigatorCategoriesSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewLinks.Items) listViewItem.Checked = true;
        }

        private void buttonSpecNavigatorCategoriesSelectToggle_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewLinks.Items) listViewItem.Checked = !listViewItem.Checked;
        }

        private void buttonSpecNavigatorCategoriesSelectNone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewLinks.Items) listViewItem.Checked = false;
        }
        #endregion

        #region GUI - WebScraping - Stop - Click
        private void buttonWebScrapingStop_Click(object sender, EventArgs e)
        {
            stopRequested = true;
            buttonWebScrapingStop.Enabled = false;
        }
        #endregion

        #region DataSet Description
        private string dataSetDescriptionName = ""; 
        private string GetDataSetDescription()
        {
            return dataSetDescriptionName;
        }

        private void ClearDataSetDescription()
        {
            dataSetDescriptionName = "";
        }

        private void AddDataSetDesciption(string addName)
        {
            if (!dataSetDescriptionName.Contains(addName + " ")) dataSetDescriptionName = addName + " " + dataSetDescriptionName;
        }
        #endregion 

        #region GUI - WebScraping - Save - Click
        private void buttonWebScrapingSave_Click(object sender, EventArgs e)
        {            
            try
            {
                SetButtonState(enabled: false);
                int writedCount = 0;
                fastColoredTextBoxJavaScriptResult.Text = "Saving:\r\n";
                buttonWebScrapingSave.Enabled = false;
                if (_metadataDataTotalMerged.Count > 0)
                {
                    DateTime dateTimeSaveDate = DateTime.Now;
                    foreach (Metadata metadata in _metadataDataTotalMerged.Values)
                    {
                        writedCount++;
                        UpdatedCounter("Write: ", writedCount); 
                        metadata.FileDateModified = dateTimeSaveDate;
                        metadata.FileDirectory = webScrapingName;
                        metadata.FileSize = -1;
                        DatabaseAndCacheMetadataExiftool.WebScrapingWrite(metadata);
                    }

                    WebScrapingDataSet webScrapingDataSet = new WebScrapingDataSet();
                    webScrapingDataSet.WrittenDate = dateTimeSaveDate;
                    webScrapingDataSet.Description = GetDataSetDescription();
                    webScrapingDataSet.MetadataDataSetCount = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);
                    _webScrapingDataSet.Add(dateTimeSaveDate, webScrapingDataSet);
                    UpdatedWebScrapingDataSetList(_webScrapingDataSet);

                    WebScrapingDataSetStatusWrite(_webScrapingDataSet);
                    fastColoredTextBoxJavaScriptResult.Text = "DataSet saved\r\n"
                        + webScrapingDataSet.MetadataDataSetCount.ToString();
                }
                isDataUnsaved = false;
                buttonWebScrapingSave.Enabled = false;                
            } catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Saveing WebScraping failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            } finally
            {
                SetButtonState(enabled: true);
            }
        }

        #endregion

        #region GUI - UpdatedWebScrapingPackageList
        private void UpdatedWebScrapingDataSetList(Dictionary<DateTime, WebScrapingDataSet> webScrapingDataSets)
        {
            listViewDataSetDates.Items.Clear();
            foreach (KeyValuePair<DateTime, WebScrapingDataSet> keyValuePairs in webScrapingDataSets)
            {
                ListViewItem listViewItem = new ListViewItem(TimeZone.TimeZoneLibrary.ToStringSortable(keyValuePairs.Value.WrittenDate));
                listViewItem.Tag = keyValuePairs.Value.WrittenDate;
                listViewDataSetDates.Items.Add(listViewItem);
                listViewItem.SubItems.Add(keyValuePairs.Value.Description);
                listViewItem.SubItems.Add(keyValuePairs.Value.MetadataDataSetCount?.MediaFiles.ToString());
                listViewItem.SubItems.Add(keyValuePairs.Value.MetadataDataSetCount?.Titles.ToString());
                listViewItem.SubItems.Add(keyValuePairs.Value.MetadataDataSetCount?.AlbumNames.ToString());
                listViewItem.SubItems.Add(keyValuePairs.Value.MetadataDataSetCount?.LocationNames.ToString());
                listViewItem.SubItems.Add(keyValuePairs.Value.MetadataDataSetCount?.KeywordTags.ToString());
                listViewItem.SubItems.Add(keyValuePairs.Value.MetadataDataSetCount?.Regions.ToString());
            }

            if (webScrapingDataSets.Count == 0)
            {
                buttonWebScrapingLoadPackage.Enabled = false;
                listViewDataSetDates.Enabled = false;
            }
            else
            {
                buttonWebScrapingLoadPackage.Enabled = true;
                listViewDataSetDates.Enabled = true;
                listViewDataSetDates.Items[0].Selected = true;
            }
        }
        #endregion 

        #region GUI - Update Status 
        private Stopwatch stopwatchCounter = new Stopwatch();

        private void UpdatedCounter(string text, int counter)
        {
            if (!stopwatchCounter.IsRunning) stopwatchCounter.Start();
            if (stopwatchCounter.ElapsedMilliseconds > 300)
            {
                //Application.DoEvents();
                stopwatchCounter.Restart();
            }
            toolStripStatusLabelStatus.Text = text + counter.ToString();
        }

        private void DatabaseAndCacheMetadataExiftool_OnReadRecord(object sender, ReadToCacheParameterRecordEventArgs e)
        {
            readCount++;
            UpdatedCounter("Read: ", readCount);
        }
        #endregion

        #region Load DataSet from Database
        private Dictionary<string, Metadata> LoadDataSetFromDatabase(DateTime dataSetDateTime)
        {
            Dictionary<string, Metadata> metaDataDictionary = new Dictionary<string, Metadata>();

            DatabaseAndCacheMetadataExiftool.ReadToCacheWhereParameters(MetadataBrokerType.WebScraping, webScrapingName, null, dataSetDateTime, true);
            List<FileEntryBroker> fileEntryBrokers = DatabaseAndCacheMetadataExiftool.ListMediafilesInWebScraperPackages(MetadataBrokerType.WebScraping, webScrapingName, dataSetDateTime);

            foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
            {
                Metadata metadata = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBroker);

                if (metadata != null)
                {
                    if (metaDataDictionary.ContainsKey(metadata.FileName))
                        metaDataDictionary[metadata.FileName] = Metadata.MergeMetadatas(metaDataDictionary[metadata.FileName], metadata);
                    else
                        metaDataDictionary.Add(metadata.FileName, metadata);
                }
            }

            return metaDataDictionary;
        }
        #endregion

        #region Metadata - Dictionary - Merge
        private void MetadataDictionaryMerge(Dictionary<string, Metadata> metadataDictionaryDestination, Dictionary<string, Metadata> metadataDictionarySource)
        {
            //Merge
            foreach (KeyValuePair<string, Metadata> metadataKeyValuePair in metadataDictionarySource)
            {
                if (metadataDictionaryDestination.ContainsKey(metadataKeyValuePair.Value.FileName))
                    metadataDictionaryDestination[metadataKeyValuePair.Key] = Metadata.MergeMetadatas(_metadataDataTotalMerged[metadataKeyValuePair.Key], metadataKeyValuePair.Value);
                else
                    metadataDictionaryDestination.Add(metadataKeyValuePair.Key, metadataKeyValuePair.Value);
            }
        }
        #endregion 

        #region GUI - Load Selected DataSet dates from Database
        private int readCount = 0;

        private void buttonWebScrapingLoadPackage_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtonState(enabled: false);

                fastColoredTextBoxJavaScriptResult.Text = "Load and merge dataset...\r\n";

                fastColoredTextBoxJavaScriptResult.Text += "Counts before merge:\r\n";
                MetadataDataSetCount metadataDataSetCountBeforeMerge = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);
                fastColoredTextBoxJavaScriptResult.Text += metadataDataSetCountBeforeMerge.ToString();

                readCount = 0;
                foreach (ListViewItem listViewItem in listViewDataSetDates.Items)
                {
                    if (listViewItem.Checked && listViewItem.Tag is DateTime)
                    {
                        DateTime dataSetDateTime = (DateTime)listViewItem.Tag;

                        Dictionary<string, Metadata> metadataDictionaryLoaded = LoadDataSetFromDatabase(dataSetDateTime);
                        MetadataDictionaryMerge(_metadataDataTotalMerged, metadataDictionaryLoaded);

                        MetadataDataSetCount metadataDataSetCountLoaded = MetadataDataSetCount.CountMetadataDataSet(metadataDictionaryLoaded);

                        fastColoredTextBoxJavaScriptResult.Text += "Count for this DataSet: " + dataSetDateTime.ToString() + "\r\n";
                        fastColoredTextBoxJavaScriptResult.Text += metadataDataSetCountLoaded.ToString();

                        AddDataSetDesciption("Database");
                    }
                }

                MetadataDataSetCount metadataDataSetCountAll = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);
                fastColoredTextBoxJavaScriptResult.Text =
                    "DataSet loaded...\r\n" +
                    "Count after loaded and merge all DataSet\r\n" + metadataDataSetCountAll.ToString() + "\r\n-------------------------------\r\n" +
                    fastColoredTextBoxJavaScriptResult.Text;
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error occured!\r\n\r\n" + ex.Message, "Error occured", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                SetButtonState(enabled: true);
            }
        }


        #endregion

        #region GUI - Delete WebScraping DataSet
        private void buttonSpecNavigatorDataSetSelectDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtonState(enabled: false);

                fastColoredTextBoxJavaScriptResult.Text = "Deleteing DataSet\r\n";

                foreach (ListViewItem listViewItem in listViewDataSetDates.Items)
                {
                    if (listViewItem.Checked && listViewItem.Tag is DateTime)
                    {
                        DateTime packageDateTime = (DateTime)listViewItem.Tag;

                        int rowsAffected = DatabaseAndCacheMetadataExiftool.DeleteDirectoryAndHistory(MetadataBrokerType.WebScraping, webScrapingName, packageDateTime);
                        fastColoredTextBoxJavaScriptResult.Text += rowsAffected.ToString() + " rows deleted for date: " + packageDateTime.ToString() + "\r\n";
                    }
                }

                List<DateTime> webScrapingDataSetDates = DatabaseAndCacheMetadataExiftool.ListWebScraperDataSet(MetadataBrokerType.WebScraping, webScrapingName);
                _webScrapingDataSet = WebScrapingDataSetStatusRead(webScrapingDataSetDates);
                UpdatedWebScrapingDataSetList(_webScrapingDataSet);

            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error occured!\r\n\r\n" + ex.Message, "Error occured", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                SetButtonState(enabled: true);
            }
        }
        #endregion

        #region GUI - WebScrapingCategoryGroup - Select - All/Toggle/None

        private void buttonSpecNavigatorDataSetSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewDataSetDates.Items) listViewItem.Checked = true;
        }

        private void buttonSpecNavigatorDataSetSelectToggle_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewDataSetDates.Items) listViewItem.Checked = !listViewItem.Checked;
        }

        private void buttonSpecNavigatorDataSetSelectNone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewDataSetDates.Items) listViewItem.Checked = false;
        }
        #endregion

        #region GUI - Clear Active DataSet
        private void buttonWebScrapingClearDataSet_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtonState(enabled: false);
                _metadataDataTotalMerged.Clear();
                ClearDataSetDescription();
                fastColoredTextBoxJavaScriptResult.Text = "Metadata DataSet cleared\r\n";

                MetadataDataSetCount metadataDataSetCountAll = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);
                fastColoredTextBoxJavaScriptResult.Text +=
                    "Current count for DataSet\r\n" +
                    metadataDataSetCountAll.ToString() +
                    fastColoredTextBoxJavaScriptResult.Text;
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error occured!\r\n\r\n" + ex.Message, "Error occured", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                SetButtonState(enabled: true);
            }
        }
        #endregion

        #region GUI - WebScrapingAddUserTags_Click
        private void kryptonButtonWebScrapingAddUserTags_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtonState(enabled: false);

                string result = KryptonInputBox.Show("Enter new tags you like to do web scraping for. You can add multimple tags that are sepearted", "User tags", "examples; portrait photography; football");

                string[] newTags = result.Split(new string[] { System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator, "\r\n", ";", ",", "\n", "\r", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                string prefixUrl = Properties.Settings.Default.WebScraperSearchTagUrlPrefix;
                if (!prefixUrl.EndsWith("/") && !prefixUrl.EndsWith("\\")) prefixUrl = prefixUrl + "/";

                ScrapingResult scrapingResult = new ScrapingResult();
                foreach (string newTag in newTags)
                {
                    string newTagTrim = newTag.Trim();
                    string newLink = prefixUrl + HttpUtility.UrlPathEncode(newTagTrim);
                    if (!scrapingResult.LinksTags.ContainsKey(newLink)) scrapingResult.LinksTags.Add(newLink, newTagTrim);
                }

                CategoryLinksAddUpdate(_linkCatergories, scrapingResult.LinksTags, "Search");
                UpdatedWebScrapingDataSetList(_webScrapingDataSet);
                CategryLinksShowInListView(_linkCatergories);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error occured!\r\n\r\n" + ex.Message, "Error occured", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            finally
            {
                SetButtonState(enabled: true);
            }
        }
        #endregion

        #region Export
        private void buttonWebScrapingExportDataSet_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.FileName = TimeZoneLibrary.ToStringFilename(DateTime.Now) + " WebScraper Metadatas.json";
                saveFileDialog1.Title = "Save locations as JSON";
                saveFileDialog1.CheckFileExists = false;
                saveFileDialog1.CheckPathExists = false;
                saveFileDialog1.DefaultExt = "json";
                saveFileDialog1.Filter = "JSON files (*.json)|*.json";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    SetButtonState(enabled: false);

                    var output = JsonConvert.SerializeObject(_metadataDataTotalMerged, Formatting.Indented);
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, output, Encoding.UTF8);
                    KryptonMessageBox.Show(_metadataDataTotalMerged.Count.ToString() + " metadata exported", "Metadata exported", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Information, showCtrlCopy: true);

                    SetButtonState(enabled: true);
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error saving JSON file!\r\n\r\n" + ex.Message, "Was not able to save JSON file", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            } finally
            {
                SetButtonState(enabled: true);
            }
        }
        #endregion

        #region Import
        private void buttonWebScrapingImportDataSet_Click(object sender, EventArgs e)
        {
            try
            {
                SetButtonState(enabled: false);

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
                    buttonWebScrapingExportDataSet.Enabled = false;
                    buttonWebScrapingImportDataSet.Enabled = false;

                    fastColoredTextBoxJavaScriptResult.Text = "Import and merge dataset...\r\n";
                    fastColoredTextBoxJavaScriptResult.Text += "Counts before import and merge:\r\n";
                    
                    string input = System.IO.File.ReadAllText(openFileDialog1.FileName, Encoding.UTF8);

                    MetadataDataSetCount metadataDataSetCountBeforeMerge = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);
                    fastColoredTextBoxJavaScriptResult.Text += metadataDataSetCountBeforeMerge.ToString();


                    Dictionary<string, Metadata> readResultSet = JsonConvert.DeserializeObject<Dictionary<string, Metadata>>(input);
                    MetadataDictionaryMerge(_metadataDataTotalMerged, readResultSet);

                    MetadataDataSetCount metadataDataSetCountLoaded = MetadataDataSetCount.CountMetadataDataSet(readResultSet);

                    fastColoredTextBoxJavaScriptResult.Text = "DataSet imported...\r\n";
                    fastColoredTextBoxJavaScriptResult.Text += metadataDataSetCountLoaded.ToString();

                    buttonWebScrapingExportDataSet.Enabled = true;
                    buttonWebScrapingImportDataSet.Enabled = true;

                    KryptonMessageBox.Show(readResultSet.Count.ToString() + " locations imported", "Location file imported", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Information, showCtrlCopy: true);
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error loading JSON file!\r\n\r\n" + ex.Message, "Was not able to load JSON file", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            } finally
            {
                SetButtonState(enabled: true);
            }
        }
        #endregion
    }


    #region ListView Sorting
    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            if (listviewX.SubItems.Count <= ColumnToSort || listviewY.SubItems.Count <= ColumnToSort)
                compareResult = 0;
            else if (listviewX.SubItems[ColumnToSort].Tag is DateTime && listviewY.SubItems[ColumnToSort].Tag is DateTime)
            {
                compareResult = DateTime.Compare((DateTime)listviewX.SubItems[ColumnToSort].Tag, (DateTime)listviewY.SubItems[ColumnToSort].Tag);
            }
            else if (decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out decimal numX) && decimal.TryParse(listviewY.SubItems[ColumnToSort].Text, out decimal numY))
            {
                compareResult = decimal.Compare(numX, numY);
            }
            else
            {
                // Compare the two items
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            }

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        public int SortColumn
        {
            set { ColumnToSort = value; }
            get { return ColumnToSort; }
        }

        public SortOrder Order
        {
            set { OrderOfSort = value; }
            get { return OrderOfSort; }
        }

    }
    #endregion
}

/* Search words string: Adventure,Aeroplanes,Aircraft,Airport,Ale,Amusement Parks,Animal,Animal Shelter,Animals,Baby Carriage,Balcony,Ball,Balloon,Beard,Bed,Bed Sheet,Bedding,Bedroom,Beer,Birds,Birthday,Birthday Cake,Boats,Bride,Bridges,Camping,Cars,Child,Chocolate,Chocolate Cake,Christmas,Christmas Decoration,Christmas Lights,Christmas Ornament,Christmas Tree,Climbing,Coffee Cup,Convertible,Crawling,Cream,Cross-Country Skiing,Cushion,Dining Room,Dinner,Dogs,Facade,Family,Fast Food,Fish,Fishing,Flag,Flame,Flower Bouquet,Flowers,Foam,Food,Football,Football Player,Forests,Fountain,Fruit,Gift,Goggles,Holiday,Infant,Kindergarden,Kitchen,Laptop,Laughter,Lego,Motorcycle,Nap,Night,Ocean,Outdoor Recreation,Parade,Parks,Party,Pigs,Reading,Restaurant,Retail-Store,Rivers,Santa Claus,Ships,Ski,Skiing,Sleep,Smile,Snow,Student,Stuffed Toy,Sunglasses,Swan,Sweater,Swimming,Swimming pool,Tan,Theatre,Town Square,Toys,Tractors,Tuxedo,Umbrella,Valley,Vehicle,Water,Water Parks,Wedding
Adventure
Aeroplanes
Air Force
Aircraft
Airport
Alcohol
Alcoholic Beverage
Ale
All-Terrain Vehicle
Alley
Amber
Amusement Parks
Animal
Animal Shelter
Antique
Apartment
Arcade
Arch
Architecture
Arctic
Arena
Army
Art
Art Gallery
Asphalt
Auto Show
Azure
Baby Carriage
Back
Backyard
Balance
Balcony
Ball
Balloon
Barefoot
Bass Guitar
Bathing
Bathtub
Bay
Beanie
Beard
Bed
Bed Sheet
Bedding
Bedroom
Bedtime
Beed
Beer
Bicycle Saddle
Birds
Birthday
Birthday Cake
Blazer
Blizzard
Boating
Boats
Book
Bookcase
Boot
Bottle
Bowl
Bowling
Box
Branch
Bread
Breakfast
Brick
Brickwork
Bride
Bridges
Brunch
Building
Bull
Bumper
Buttercream
Cabinetry
Cable
Camouflage
Camping
Campus
Cap
Cape
Car Park
Carpet
Cars
Castles
Cathedrals
Centrepiece
Ceramic
Ceremony
Chair
Chapel
Child
Chocolate
Chocolate Cake
Christmas
Christmas Decoration
Christmas Lights
Christmas Ornament
Christmas Tree
Circle
City
Cityscape
Class
Classic Car
Cliffs
Climbing
Cloud
Coast
Coat
Cobblestone
Coffee Cup
Community
Compact Car
Computers
Concerts
Condominium
Confectionery
Convention
Conversation
Convertible
Cooking
Cosmetics
Costume
Couch
Crawling
Cream
Cross-Country Skiing
Crowd
Cuisine
Culinary Art
Cupboard
Curtain
Cushion
Cutlery
Cycle Sport
Cycling
Dancing
Dining Room
Dinner
Diving
Dock
Document
Dogs
Domestic Pig
Door
Downtown
Drawer
Dress
Drink
Drinking
Driving
Drum
Drummer
Drums
Ducks
Ear
Easter
Eating
Electronics
Exhibition
Extreme Sport
Facade
Fair
Family
Farms
Fashion
Fast Food
Father
Fault
Field
Film
Fir
Fires
Fireworks
Fish
Fisherman
Fishing
Flag
Flag Of The United States
Flame
Flare
Flight
Flora
Flower Bouquet
Flowers
Foam
Fondant
Foot
Football
Football Player
Forests
Fork
Formal Wear
Fountain
Frost
Fruit
Fun
Fur
Garden
Gate
Geology
Gift
Gingerbread
Glaciers
Glass
Glasses
Glove
Goggles
Gown
Graduation
Grandparent
Groom
Grove
Guitar
High-Heeled Footwear
Holiday
Home
Homework
Hood
Hoodie
Horn
Hospitals
House
Houseplant
Hug
Hut
Ice
Ice Rink
Ice Skating
Icing
Infant
Iris
Iron
Island
Jeans
Jersey
Jewellery
Keyboard
Kindergarden
Kitchen
Ladder
Lakes
Lamp
Landmarks
Landscape
Landscaping
Lane
Laptop
Laughter
Lcd Tv
Leaf
Leather
Lecture
Leggings
Lego
Lens Flare
Light Fixture
Lightning
Lilac
Lipstick
Living Room
Log Cabin
Long Hair
Love
Lumber
Lunch
Meadow
Meal
Meat
Meeting
Memorial
Mesh
Metal
Metropolis
Microphone
Military
Military Aircraft
Mirror
Monuments
Mother
Motorboat
Motorcycle
Motorcycling
Mountain Bike
Mountain Range
Mountains
Moustache
Mouth
Mudflat
Multimedia
Muscle
Music
Musical Ensemble
Musical Instrument
Musician
Nail
Nap
Nature
Necklace
Necktie
New Year
Night
Nightclubs
Nursery
Ocean
Office
Outdoor Recreation
Pajamas
Palaces
Parade
Parking
Parks
Party
Pasture
Patient
Pattern
Pedestrian
People
Percussion
Performance
Performing Arts
Petal
Photo Shoot
Picnics
Picture Frame
Piers
Pigs
Pillow
Pine
Plant
Plant Stem
Plantation
Plaster
Plastic
Plate
Play
Playground
Playground Slide
Plaza
Plush
Plywood
Pond
Porcelain
Port
Portrait Photography
Prairie
Presentation
Propeller
Property
Public Speaking
Purple
Rainforest
Ranch
Rave
Reading
Red Hair
Reflection
Reservoir
Resort
Restaurant
Retail-Store
Ridge
Ritual
Rivers
Rock
Rock Climbing
Roof
Room
Rope
Rose
Ruins
Running
Rural Area
Sailboats
Sailing
Sailing Ship
Salad
Sand
Santa Claus
Saucer
Scarf
Scooter
Screenshots
Sculpture
Sea
Seabird
Seafood
Sedan
Selfies
Seminar
Senior Citizen
Shack
Shadow
Shed
Shelf
Ships
Shirt
Shoal
Shoe
Shopping
Shore
Shorts
Shrines
Shrub
Siberian Husky
Sibling
Sidewalk
Singer
Singing
Sitting
Ski
Ski Mountaineering
Skiff
Skiing
Skin
Sky
Skylines
Skyscrapers
Sled Dog
Sledding
Sleds
Sleep
Slope
Smile
Smoke
Snow
Snowboarding
Sock
Soft Drink
Soil
Sole
Space
Spire
Sport Utility Vehicle
Sports
Sports Car
Spring
Spruce
Square
Stage
Stairs
Stars
Statues
Steel
Steeple
Stream
Street Light
String Instrument
Student
Stuffed Toy
Suburb
Suit
Summit
Sunglasses
Supercar
Supper
Swan
Sweater
Swimming
Swimming Pool
Swimwear
Table
Tablecloth
Tableware
Tall Ship
Tall Ships
Tan
Theatre
Tide
Tights
Tile
Tire
Toddler
Toe
Tower Block
Towers
Town
Town Square
Tractors
Trail
Training
Trains
Trampoline
Tree
Trousers
Truck
Trunk
Tulip
Turquoise
Tuxedo
Twig
Umbrella
Vacation
Valley
Vegetable
Vehicle
Veil
Village
Vintage Car
Visual Arts
Wagons
Walking
Walkway
Wall
Water
Water Bird
Water Feature
Water Parks
Watercraft
Wave
Weapon
Wedding
Wedding Dress
Wetland
Whipped Cream
Wind Wave
Window
Window Blind
Windshield
Wine
Wine Glass
Wing
Winter
Winter Sport
Winter Storm
Wood
Woodland
Wool
Wrinkle
Yachts
Yard
Youth
*/

/* Search words
Anole
À la carte food
Abbey
Abdomen
Abrasive saw
Abseiling
Absolut vodka
Academic conference
Academic dress
Acanthocereus tetragonus
Accipitriformes
Acerola
Acerola family
Acianthera
Acrobatics
Acrylic paint
Action figure
Acura mdx
Adaptation
Adventure
Adventure game
Adventure racing
Advertising
Aegean cat
Aeolian landform
Aerial photography
Aerospace engineering
Aerospace manufacturer
Afro
Afterglow
Agama
Agaric
Agaricaceae
Agaricomycetes
Agaricus
Agati
Agave
Agave azul
Agriculture
Air force
Air gun
Air travel
Air-raid shelter
Airbus
Airbus a320 family
Airbus a330
Aircraft
Aircraft carrier
Aircraft engine
Airedale terrier
Airline
Airliner
Airplane
Airport
Airport apron
Airport terminal
Airsoft
Airsoft gun
Aisle
Album
Alcohol
Alcoholic beverage
Alfalfa
Algae
Alismatales
All-purpose flour
Alley
Allium
Alloy wheel
Alluvial fan
Aloe
Alpaca
alpine aster
alpine forget-me-not
Alps
Alstroemeriaceae
Aluminium
Alyssum
Amaranth
Amaranth family
Amaryllis belladonna
Amaryllis family
Amber
American aspen
American food
American hairless terrier
American larch
American Mourning Dove
American pit bull terrier
American pitch pine
American pokeweed
American Redstart
American shorthair
American staffordshire terrier
American wirehair
Ammunition
Amphibian
Amphibious transport dock
Amphibious warfare ship
Amphitheatre
Amusement park
Ananas
Anchor
Anchovy (food)
Ancient dog breeds
Ancient greek temple
Ancient history
Ancient roman architecture
Ancient rome
Anemone
Angelica
Angling
Angora rabbit
Anguidae
Animal migration
Animal shelter
Animal sports
Animated cartoon
Animation
Anime
Ankle
Annual plant
Anole
Ant
Antarctic flora
Antenna
Anthropology
Anthurium
Antique
Antique car
Antler
Aonori
Apartment
Apatura
Apiary
appetizer
Apple
Apple mint
Aqua
Aquarium
Aquatic plant
Aqueduct
Arachnid
Araneus
Arcade
Arch
Archaeological site
Archaeology
Archipelago
Architecture
Arctic
Arctic ocean
Arctostaphylos
Arecales
Arena
Arête
Argynnis
Aristotelia chilensis
Arizona Cypress
Arm
Armored car
Army
aromatic aster
Arrosticini
Arrow
Arrowgrass
Arrowroot
Arrowroot family
Arroyo
Art
Art exhibition
Art gallery
Art model
Artemisia
Arthropod
Artichoke thistle
Artifact
Artificial flower
Artificial hair integrations
Artificial turf
Artisan
Artist
Artocarpus
Artwork
Arugula
Arum
Arum family
Ascalapha odorata
Asclepiadoideae
Ash
Asian
Asian semi-longhair
Asparagus
Asphalt
Aster
Asterales
Astronomical object
Astronomy
Athletic shoe
Athletics
Atlantic canary
Atlas
Atmosphere
Atmospheric phenomenon
Attalea speciosa
Attic
Aubretia
Audience
Audio accessory
Audio equipment
Auditorium
Audubon’s Cottontail
Auricularia
Aurora
Australian kelpie
Australian terrier
Auto part
Auto racing
Auto show
Autograph
Automotive carrying rack
Automotive design
Automotive engine part
Automotive exhaust
Automotive exterior
Automotive fog light
Automotive lighting
Automotive luggage rack
Automotive mirror
Automotive navigation system
Automotive side-view mirror
Automotive tail & brake light
Automotive tire
Automotive wheel system
Automotive window part
Autumn
Aviation
Avocado
Awning
Axolotl
Azalea
Azure
Baby
baby blue eyes
Baby carriage
Baby Products
Back
Backlighting
Backpack
Backpacking
Backyard
Badlands
Bag
Baggage
Baidarka
Bailey bridge
Bait
Bake sale
Baked goods
Bakery
Baking
Balance
Balcony
Ball
Ball game
Ballistic missile submarine
balsam fir
Baluster
Bamboo
Bamboo shoot
Banana
Banana family
Banana leaf
Band plays
band winged grasshoppers
Bangs
Bank
Banknote
Banksia
Banner
Baptistery
Bar
Barbary fig
Barbed wire
Barberry family
Barbet
Barechested
Barge
Barley
Barlia
Barn
Barque
Barquentine
Barware
Basement
Basil
Basilica
Basket
Basketball
Basketball court
Basmati
Batholith
Bathroom
Bathroom sink
Battlecruiser
Battleship
Bay
Bayou
Bazaar
Beach
Beach moonflower
Bead
Beak
Beaked Toad
Beam
Beam bridge
Bean
Bean sprouts
Beanie
Beard
beardtongue
Beauty
Bed
Bed frame
Bed sheet
Bedding
Bedrock
Bedroom
Bedtime
Bee
Bee pollen
Beehive
Beekeeper
Beer
Beer bottle
Beetle
Beetroot
Begging
Begonia
Beige
Bell
Bell 206
Bell peppers and chili peppers
Bell tower
Bellflower
Bellflower family
Bench
Bengal
Bentley continental gt
Berberis
Berry
Bicycle
Bicycle accessory
Bicycle chain
Bicycle clothing
Bicycle drivetrain part
Bicycle fork
Bicycle frame
Bicycle handlebar
Bicycle helmet
Bicycle motocross
Bicycle part
Bicycle pedal
Bicycle racing
Bicycle saddle
Bicycle tire
Bicycle wheel
Bicycles–Equipment and supplies
Bidet
Bight
Bigtree
Billboard
Biome
Birch
Birch family
Bird
Bird feeder
Bird food
Bird migration
Bird nest
Bird of prey
Bird supply
Bird’s-eye
Bird’s-eye view
Birth
Bison
Bitter orange
Bittern
Black
Black capped Chickadee
Black cat
Black cherry
Black fly
Black hair
Black norwegian elkhound
Black oak
Black-and-white
black-eyed susan
Blackberry
Blackbird
Blackboard
Blacksmith
Blanket
Blazer
Blister beetles
Blizzard
Block party
Blond
Blossom
blowflies
Blowout
Blue
Blue jay
blue wood aster
Blue-collar worker
Bluebird
Bluebonnet
Bmw
Bmw 3 series
Bmw 3 series (e21)
Bmw 3 series (e90)
Bmw m3
Bmw x1
Boa
Boa constrictor
Boar
Boardsport
Boardwalk
Boat
Boating
Boats and boating–Equipment and supplies
Body jewelry
Body of water
Body piercing
Bodybuilding
Boeing
Boeing 737
Boeing 737 next generation
Bog
Bolete
Bolonka
Boloria
Bombay
Bombycidae
Bombyx mori
Bone
Bonfire
Bonnet
Bonsai
Bony-fish
Book
Book cover
Bookcase
Bookselling
Borage
Borage family
Borassus flabellifer
Border terrier
Boreal Toad
Botanical garden
Botany
Bottle
Bottled water
Boulder
Bouldering
Bouleuterion
Bouquet
Boutique
Boutique hotel
Bouzouki
Bovine
Bowed string instrument
Box
Box girder bridge
Box turtle
Boxer
Boxing
Bracelet
Braided river
Brake
Bramble
Brambling
Bran
Branch
Branched asphodel
Brand
Brass
Brass instrument
Brassica
Brassica rapa
Breadboard
Breakfast
Breakwater
Breckland thyme
Brewery
Brick
Brickwork
Bridal clothing
Bride
Bridge
Brig
Brigantine
Brimstones
British semi-longhair
British shorthair
Broadcasting
Broccoflower
Broccoli
Brochure
Bromeliaceae
Bronze
Bronze sculpture
Broomrape
Broomrape family
Brown
Brown hair
Brown snake
Brug
Brunch
Brush
Brush-footed butterfly
Brutalist architecture
Bucket
Buckthorn family
Bud
buddleia
Budgie
Buffaloberries
Bufo
Bug
Building
Building insulation
Bulbul
Bulk carrier
Bull
Bulldozer
Bulletin board
Bullmastiff
Bullsnake
Bumblebee
Bumper
Bumper part
Bumper sticker
Bunker
Burdock
Burmese
Burnet rose
Burro
Bus
Bus stop
Bush tomato
Bushmeat
Business
Businessperson
Butomus
Butte
Butterbur
buttercup
Butterfly
Button
Byzantine architecture
Cabbage
Cabbage butterfly
Cabinetry
Cable
Cable car
Cable management
Cable television
Cable-stayed bridge
Cactus
Caesalpinia
Café
Cafeteria
Cage
Cairn terrier
Calabaza
Calamondin
Caldera
Calf
california lilac
California live oak
California wild rose
Calipers
Calligraphy
Calm
Calochortus
Camberwell Beauty
Camel
Camelid
Camellia
Camellia sasanqua
Camera
Camera accessory
Camera lens
Camera operator
Cameras & optics
Camouflage
Camp
Campfire
Camping
Campus
Canada columbine
Canadian fir
Canal
Canal tunnel
Candle
Canidae
Canna family
Canna lily
Canning
Cannonball tree
Canoe
Canoe birch
Canoe sprint
Canoeing
Canola
Canopy
Cantilever bridge
Canyon
Canyoning
Cap
Capacitor
Cape
Car
Car dealership
Car seat
Car seat cover
Caramel color
Caravanserai
Carbonated soft drinks
Carburetor
Cardigan
Cardinal
Cargo
Cargo ship
Caribbean
Carillon
Carmine
Carnival
Carnivore
Carnivorous plant
Carolina Chickadee
Carolina rose
Carpenter
Carpenter bee
Carpet
Cart
Carton
Cartoon
Carving
Caryophyllales
Casaurina glauca
Cash crop
Castilleja
Castle
Casuarina
Cat
Cat supply
Catalytic converter
Catchfly
Caterpillar
Catfish
Cathedral
Cattleya
Cauliflower
Cavaquinho
Cave
Caving
CD
Ceiling
Ceiling fixture
Celestial event
Cello
Cement
Cemetery
Centaurium
Centella
Centrepiece
Ceramic
Cereal
Ceremony
Cestrum
Cg artwork
Chaga mushroom
Chain-link fencing
Chainsaw carving
Chair
Chalk
Chalkhill blue
Chamaemelum nobile
Chameleon
chamomile
Champignon mushroom
Championship
Channel
Chaparral
Chapel
Charadriiformes
Charcoal
Chard
Chartreux
Chasmanthe
chastetree
Château
Check-in
Cheek
Cheering
Chelonoidis
Chemical compound
Cherry
Cherry blossom
Cherry Tomatoes
Chervil
Chest
Chest of drawers
Chestnut
Chevrolet
Chevrolet 150
Chevrolet bel air
Chevrolet captiva
Chevrolet equinox
Chevrolet styleline
Chickadee
Chicken
Chicken coop
Chicory
Chihuahua
Child
Child art
Childbirth
Chimney
Chin
China rose
Chinese celery
Chinese chicken salad
Chinese food
Chinese peony
Chipmunk
Chives
Chlorophyta
Chocolate
Chokeberry
Chokecherry
Chopper
Choreography
Choy sum
Christmas
Chrysanthemum coronarium
Chrysanths
Chrysler
Chrysopogon zizanioides
Church
Church bell
Chute
Cicada
Cigar
Cigarette
Cinder cone
Cinquefoil
Circle
Circuit component
Circuit prototyping
Circular saw
Cirneco dell’etna
Cirque
Citric acid
Citrullus
Citrus
City
City car
Cityscape
Clary
Class
Classic
Classic car
Classical architecture
Classical sculpture
Classroom
Claw
Clay
Cleaner
Cleanliness
Cleavers
Clematis
Clementine
Clergy
Cliff
Cliff dwelling
Climbing
Clinic
Clipper
Clock
Clock tower
Close-up
closed blue gentian
Clothes dryer
Clothes hanger
Clothing
Cloud
Clover
Clubmoss
Clutch
Coal
Coast
Coastal and oceanic landforms
Coasteering
Coat
Cobalt blue
Cobblestone
Cockapoo
Cockatiel
Cockatoo
Cockpit
Cockroach
Coconut
Coffee
Coffee cup
Coffee table
Coffeehouse
Coin
Cola
Cold saw
Collaboration
Collage
Collar
Collard greens
Collection
College
Colorado blue columbine
Colorado spruce
Colorfulness
Colt
Colubridae
Columbian spruce
Columbine
Column
Combat sport
Combat vehicle
Combretaceae
Comfort
Comfort food
Comfrey
Comics
Commemorative plaque
Commercial building
Commercial vehicle
Common blue
Common chameleon
Common fig
common peony
Common persimmon
Common rue
Common sage
common shepherd’s purse
Common snapping turtle
Common tormentil
Common yabby
common yellow violet
Communication Device
Community
Community centre
Compact car
Compact cassette
Compact mpv
Compact sport utility vehicle
Companion dog
Company
Compass
Competition
Competition event
Composite material
Compost
Computer
Computer accessory
Computer component
Computer desk
Computer hardware
Computer keyboard
Computer monitor
Computer monitor accessory
Computer program
Computer speaker
Computer terminal
Concert
Concert hall
Conch
Concrete
Concrete bridge
Condominium
Cone
Coneflower
Confectionery
Conference hall
Conformation show
Conifer
Conifer cone
Constellation
Construction
Construction equipment
Construction worker
Contact lens
Contact sport
Container ship
Control panel
Control tower
Convenience food
Convenience store
Convent
Convention
Convention center
Conversation
Convertible
Cookie
Cooking plantain
Cookware and bakeware
Cool
Coonhound
Cooper’s Hawk
Coquelicot
Coraciiformes
Coral
coral aloe
Coral reef
Coregonus lavaretus
Coriander
Cormorant
Corn
Corn kernels
Corn on the cob
Corn smut
Cornales
Corporate headquarters
Corybas
Cosmos caudatus
Costume
Costume hat
Costus family
Cottage
Couch
Countertop
Coupé
Course
Courthouse
Courtyard
Cove
Cow parsley
Cow-goat family
Cowslip
Coyote
Crab
Craft
Cranberry
Crane
Crane vessel (floating)
Crankset
Crassocephalum
Crater lake
Crayfish
Cream
Creative arts
Creek
Creeking
creeping thistle
creeping wood sorrel
Crenate orchid cactus
Crescent
cretan crocus
Cretons
Cricket
Cricket-like insect
Crimson columbine
Crinum
Crochet
Crocosmia
Crocosmia × crocosmiiflora
Crocus
Croft
Crop
Cross
Cross country running
Cross-country cycling
Cross-stitch
Crossover suv
Crossword
Crow
Crow-like bird
Crowd
Cruciferous vegetables
Crucifix
Cruise ship
Crustacean
Crypt
Crystal
Cuatro
Cuckoo
Cuckoo flower
Cuculiformes
Cucumber
Cucumber, gourd, and melon family
Cucumis
Cucurbita
Cuisine
Cumulus
Cup
Cupboard
Curb
Curcuma
Currant
Curry
Curtain
Cushion
Custom car
Customer
Cut flowers
Cutlery
Cutting board
cutworms
Cycad
Cyclamen
Cycle sport
Cycling
Cyclo-cross
Cyclo-cross bicycle
Cyclone
Cylinder
Cymbal
Cymric
Cynara
Cynthia (subgenus)
Cypress family
Cypripedium
Dacia 1300
Dactylorhiza praetermissa
Daewoo lanos
Dairy
Dairy cow
Daisy
Daisy family
Dam
Dame’s rocket
Dance
Dancer
Dandelion
Dandelion coffee
Daphne
Dark green fritillary
Darkling beetles
Darkness
Data storage device
Data transfer cable
Date palm
Datura
Datura inoxia
Davidson’s Plum
Dawn
Day dress
Dayflower
Dayflower family
Daylighting
Daylily
Daytime
Decapoda
Deciduous
Deck
Deer
Delicacy
Delicatessen
Delphinium
Demolition
Demonstration
Den
Dendrobium
Denim
Desert
Desert horned lizard
Desert Palm
Desert tortoise
Design
Desk
Desktop computer
Dessert
Dessert wine
Destroyer
Destroyer escort
Devil’s tongue
Devon rex
Dew
Diagram
Dianthus
Diaper bag
Diary
Dicotyledon
Diesel fuel
Digital camera
Digital compositing
Digital SLR
Digitalis
Dike
Dill
Dining room
Dinner
Dinosaur
Diode
Diospyros
Dirt road
Disco
Dish
Dishware
Display advertising
Display board
Display case
Display device
Display window
Distaff thistles
Distilled beverage
Ditch
Dock
Dock landing ship
Document
Dodge caravan
Dodge journey
Dog
Dog breed
Dog clothes
Dog sports
Dog walking
Dogbane family
Dome
Domestic long-haired cat
Domestic pig
Domestic rabbit
Domestic short-haired cat
Domesticated turkey
Doodle
Door
Door handle
Double bass
Double-decker bus
Downhill mountain biking
Downtown
Dragon
Dragon li
Dragon lizard
Dragonflies and damseflies
Dragonfly
Drain cleaner
Drainage
Drainage basin
Drama
Drawing
Dreadlocks
Dredging
Dress
Dress shirt
Driftwood
Drill
Drilling rig
Drink
Drinking
Drinkware
Driveway
Driving
Drizzle
Drop
Drought
Drum
Drum stick
Drummer
Drums
Dry lake
Duathlon
Duck
Ducks, geese and swans
Dune
Dung beetle
Dungeon
Dusk
Dust
Dutch clover
Dutch smoushond
Dutchman’s pipe
Duvet
Duvet cover
Dvd
Dvd player
Dye
E-boat
e-book readers
Ear
Earth
Earthquake
Earthworm
Earwigs
Eastern Cottontail
eastern hemlock
Eastern prickly pear
Eating
Ebony trees and persimmons
Echidna
Echinoderm
Eclipse
Eco hotel
Ecoregion
Edelweiss
Edible mushroom
Edsel ranger
Education
Eel
Egg
Eggplant
Egyptian lavender
Egyptian temple
Einkorn wheat
Elaeis
Elbow
Elderberry
Electric blue
Electrical connector
Electrical network
Electrical supply
Electrical wiring
Electricity
Electronic component
Electronic device
Electronic engineering
Electronic instrument
Electronic signage
Electronics
Electronics accessory
Eleusine coracana
Eleutherodactylus
Elevator
Elk
Elymus repens
Emberizidae
Emblem
Embroidery
Emergency
emperor moths
Empetrum
Employment
Emu
Endive
Endurance sports
Enduro
Energy drink
Engine
Engineer
Engineering
English lavender
english marigold
Ensete
Entertainment
Envelope
Environmental art
Epazote
Ephedra
Epidendrum
Epiphyllum
Erg
Erosion
Escalator
Escarpment
Eschscholzia californica
Estate
Estuary
Eucalyptus
Eumenidae
Euphrasia
Eurasian golden oriole
European herring gull
European marsh thistle
european michaelmas daisy
European plum
European robin
European shorthair
European Starling
Eurovans
Euryops pectinatus
Evening
Evening primrose
Evening primrose family
Evening stock
Event
Evergreen
Evergreen rose
Everlasting sweet pea
Executive car
Exercise
Exhaust system
Exhibition
Explosion
Extinct volcano
Extradosed bridge
Extreme sport
Eye
Eyebrow
Eyelash
Eyewear
Facade
Face
Facial expression
Facial hair
Factory
Fair
Falconiformes
Family
Family car
Family reunion
Farm
Farmer
Farmhouse
Farmworker
Farro
Fashion
Fashion accessory
Fashion design
Fast food
Fast food restaurant
Fault
Fawn
Feather
Feathered hair
Feeder fish
Feist
Felidae
Fell
Fen
Fence
Fender
Fennel
Fennel flower
Feral goat
Fern
Fernleaf lavender
Ferns and horsetails
Ferry
Festival
Fête
Fiat
Fiat 125
Fiat 126
Fiat 500
Fiction
Fictional character
Fiddlehead fern
Field
Field trial
Fig
Figleaf gourd
Figurine
Figwort
Film camera
Film studio
Fin
Finch
Finger
Finger food
Fir
Fire
fire cherry
Fire station
Firearm
Fireplace
Fireworks
First generation chevrolet aveo
Fish
Fish pond
Fish products
Fisheye lens
Fishing
Fishing bait
Fishing net
Fishing rod
Fishing trawler
Fishing vessel
Fishmonger
Fixed link
Fjord
Flag
Flag Day (USA)
Flag of the united states
Flagstone
Flame
Flap
Flash photography
Flat panel display
Flatland bmx
Flatweed
Flatworm
Flax
Flea market
Flesh
Flight
Flight instruments
Flightless bird
Flip (acrobatic)
Floating production storage and offloading
Flock
Flood
Floodlight
Floodplain
Floor
Flooring
Floral design
Floribunda
florist gayfeather
Floristry
Flour
Flower
Flower Arranging
Flowering plant
Flowerpot
Fluid
Fluvial landforms of streams
Flxible metro
Flxible new look bus
Fly
Flyer
Flying disc
Foal
Fodder
Fog
Folk instrument
Font
Food
Food court
Food grain
Food group
Foot
Football
Football player
Footprint
Footwear
Forb
Ford
Ford cortina
Ford escape
Ford fiesta
Ford kuga
Forehead
Forest
Forge
Forget-me-not
Formal wear
Formation
Formosan mountain dog
Forsythia
Fortification
Fossil
Fountain
Fouquieria
Four o’clock family
Fowl
Fox squirrel
Fox terrier
Fractal art
Freckle
Free climbing
Free solo climbing
Freeride
Freestyle bmx
Freestyle walking
Freeway
Freezing
freight car
Freight transport
French bulldog
French lavender
Freshwater aquarium
Freshwater marsh
Fried food
Friendship
Frigate
Fritillaria
Frog
Frost
Fruit
Fruit tree
Fuchsia
Fuel
Fuel gauge
Fuki
Full breakfast
Full moon
Full-rigged ship
Full-size car
Fumaria
Fun
Function hall
Fungus
Fur
Furniture
Futon
Gadget
Gaelic football
Galanthus
Galápagos tortoise
Galium
Galliformes
Games
Garage
Garage door
Garden
Garden buildings
Garden cosmos
Garden roses
Gardener
Gardenia
Gardening
Garlic chives
Garnish
Gas
Gas mask
Gate
Gauge
Gaz-21
Gaz-m20 pobeda
Gazebo
Geastrales
Gecko
General aviation
Gentian family
Gentiana
Gentleman
Geological phenomenon
Geologist
Geology
Georgia pine
Geraniaceae
Geraniales
Geranium
Gerbera
German shorthaired pointer
German spitz
Germanders
Gesneriad family
Gesture
Geum
Geyser
Giant dog breed
Giant granadilla
Gift basket
Gila monster
Gilia
Ginger family
Girder bridge
Glacial lake
Glacial landform
Glacier
Gladiolus
Glanville fritillary
Glass
Glass bottle
Glasses
Glechoma hederacea
Glitter
globe thistle
Gloss
Gnetae
Goal
Goat
Goat-antelope
Goats
Golden samphire
Goldenrod
Goldfish
Golf course
Gomashio
Goose
Gopher tortoise
Gourd
Gown
Gps navigation device
Graduation
Graffiti
Graham flour
Grain
Granite
Granite dome
Grapevine family
Graphic design
Graphics
Grass
Grass family
Grass snake
Grasshopper
Grassland
Grave
Gravel
Grazing
Great dane
Great Spangled Fritillary
Greater burdock
Green
Green algae
Green heron
Greengrocer
Greenhouse
Grevillea
Grey
Grey squirrel
Grille
Groat
Grocer
Grocery store
Ground beetle
Groundcover
Groupset
Grouse
Grove
Guanaco
Guard dog
Guard rail
Guided missile destroyer
Guinea pig
Guitar
Guitarist
Guizotia abyssinica
Gulf Fritillary
Gull
Gun
Gun turret
Gyromitra
Hacienda
hackmatack
Hair
Hair accessory
Hair care
Hair coloring
Hair tie
Hairstyle
Half marathon
Hall
Hammer drill
Hamper
Hand
Hand luggage
Handrail
Handwriting
Handymax
Hangar
Happy
Harbor
Hard hat
Hardtop
Hardware accessory
Hardware programmer
Hardwood
Hare
Harebell
Harvestman
Harvestmen
Hat
Hatchback
Havana brown
hawk moths
Hawker
Hay
Haze
Hazmat suit
Hdmi
Head
Head restraint
Headband
Headgear
Headlamp
Headland
Headphones
Headpiece
Headquarters
Headset
Headstone
Health care
Health care provider
Heart
Hearth
Heat
heater
heath aster
heather
Heavy lift ship
Hedge
Hedgehog cactus
Heliconia
Helicopter
Helicopter rotor
Helmet
Hemp
Hemp family
Hen-of-the-wood
Heracleum (plant)
Herb
Herbaceous plant
Herd
Herding
Hericium
Hericium erinaceus
Herring
Heteromeles
Hierochloe
High brown fritillary
High-speed rail
Highland
Highway
Hiking
Hiking boot
Hiking equipment
Hill
Hill station
Hip
Hippeastrum
Hippophae
Historic house
Historic site
History
Hofmannophila pseudospretella
Holiday
Holly
Hollyhocks
Holy places
Home
Home accessories
Home appliance
Home door
Home fencing
Homework
Hominy
Honda
Honda cr-v
Honda element
Honda fit
Honeybee
Honeydew
Honeysuckle
Honeysuckle family
Hood
Hoodie
Hordeum
Horizon
Horn
Hornwort
Horse
Horsetail
Horsetail family
Hose
Hospital
Hospital bed
Hostel
Hot hatch
Hot rod
Hot spring
Hotel
Hound
House
House finch
house fly
House sparrow
Household appliance accessory
Household cleaning supply
Household supply
Houseplant
Housewarming party
Hoverfly
Hub gear
Hubcap
Huckleberry
Hug
Hula hoop
Human
Human body
Human leg
Human settlement
Hummingbird
Humpback bridge
Humulus lupulus
Hunting dog
Husk
Hut
Hutch
Hyacinth
Hyacinth bean
Hybrid bicycle
Hybrid tea rose
Hybrid vehicle
Hydrangea
Hydrangea serrata
Hydrangeaceae
Hyla
Hymenocallis
Hypericaceae
Hypericum
Hyssopus
Hyundai
Hyundai santa fe
Hyundai tucson
I/o card
Ibis
Ibizan hound
Ice
Ice cap
Ice climbing
Ice plant family
Iceberg
Iceburg lettuce
Icicle
Identity document
Igneous rock
Iguania
Iguanidae
Ikebana
Illustration
Impact crater
Impatiens
Incandescent light bulb
India hyacinth
indian blanket
Indian cuisine
Individual sports
Indoor games and sports
Industry
Inflatable
Inflatable boat
Infrastructure
Ingredient
Ink
Inlet
Inonotus
Input device
Insect
Interaction
Interactive kiosk
Interior design
International rules football
Intersection
Intrusion
Inventory
Invertebrate
Iris
Iris family
Irish water spaniel
Iron
Ironweed
Island
Islet
Issoria
Isuzu trooper
Italian food
Italian greyhound
Ivy
Ivy family
Jack pine
Jackal
Jacket
Jaguar s-type
Japanese beetle
Japanese Camellia
Japanese ginger
Jasmine
Jasmine rice
Jaw
Jay
Jazz
Jazz club
Jeans
Jeep
Jeep commander (xk)
Jeep wrangler
Jersey
Jet engine
Jewellery
Jewelry making
Jiaogulan
Job
Jogging
Joint
Juggling
Julia child rose
Jumping Cholla
Junco
Junction
Jungle
Juniper
Junk food
Kaffir lime
Kai-lan
Kale
Kayak
Kayaking
Kennel club
Key lime
Khanqah
Kia forte
Kia motors
Kia sorento
Kia soul
Kia sportage
Kiosk
Kitchen
Kitten
Kiwifruit
Klippe
Knee
Knife
Knit cap
Knitting
Kobza
Koi
Komatsuna
Komodo dragon
Korat
Kurilian bobtail
Label
Labradoodle
Lace
Lace wig
Lacerta
Lacinato kale
Lacustrine plain
Lady
Lady Banks’ rose
Ladybug
Lagoon
Lagotto romagnolo
Lake
Lake district
Lamb and mutton
Lamiales
Laminate flooring
Lamp
Land lot
Land rover discovery
Land vehicle
Landing
Landing craft
Landmark
Landscape
Landscape lighting
Landscaping
Landslide
Lane
Lantana
Lantana camara
Lantern
Lap
Laptop
Laptop part
Laptop replacement keyboard
Larch
Large münsterländer
Large-flowered cactus
Large-flowered evening primrose
Larix lyalliiSubalpine Larch
Lark
Larva
Laser
Laugh
Launch
Laundry
Laundry supply
Laurel family
Lava
Lava cave
Lava plain
Lava tube
Lavandula dentata
Lavender
Lawn
Lawn ornament
Layered hair
Lcd tv
Leaf
Leaf beetle
Leaf vegetable
Learning
Leash
Leather
Lecture
Led display
Led-backlit lcd display
Leek
Leg
Leggings
Legume
Legume family
Leisure
Leisure centre
Lemon
Lemongrass
Lens
Lens flare
Lentil
Lepidopterist
lesser burdock
Lesser skullcap
Letter
Lettuce
Leucaena
Levee
Lexus
Lexus es
Lexus rx
Lhasa apso
Library
License
Lifejacket
Light
Light bulb
Light commercial vehicle
Light cruiser
Light fixture
Light switch
Light-emitting diode
Lighter aboard ship
Lighthouse
Lighting
Lighting accessory
Lightning
Lilac
Lily
Lily family
Lime
Limestone
Line
Line art
Linen
Linens
Lingerie
Lingonberry
Lingzhi mushroom
Lion
Lip
Lip gloss
Liqueur
Liquid
Liquid bubble
Listed building
Litter
Liver
Liverwort
Livestock
Living room
Lizard
Llama
Lobby
Lobelia
loblolly pine
Lobster
Local food
Loch
Lock
Locomotive
Locust
lodgepole pine
Loft
Log bridge
Log cabin
Logging
Logo
Long hair
Long-distance running
Longboard
Loosestrife and pomegranate family
Loudspeaker
Lovage
Love
Lovebird
Loveseat
Lower Keys Marsh Rabbit
Luffa
Luggage and bags
Lumber
Lunar eclipse
Lunch
Luo han guo
Lupin
Lupinus mutabilis
Lurcher
Lute
Luxury vehicle
Luxury yacht
Lycaenid
Lymantria dispar dispar
Maar
Machine
Machine tool
Mackerel
Macro photography
Madagascar hissing cockroach
Magazine
Magenta
Magnolia family
Magnolia Warbler
Magur
Mahonia
Maidenhair tree
Mail
Maine coon
Major appliance
Majorelle blue
Makhtesh
Malaysian food
Male
Mallard
Mallow family
Malva
Malvales
Mammal
Management of hair loss
Mandarin orange
Mane
Manhole
Manor house
Mansion
Mantidae
Mantis
Map
Maple
Marabou stork
Marathon
Marble
Marching
Mare
Marina
Marine biology
Marine invertebrates
Marjoram
Market
Marketplace
Maroon
Marsh
Marsh pea
Marsupial
Masala
Mask
Mason jar
Massif
Mast
Mat
Material property
Matsutake
Mattress
Mattress pad
Mausoleum
Maya city
mayflies
Mazda
Mazda cx-5
Mazda cx-7
Mazda cx-9
Mazda3
Meadow
Meadowsweet
Meal
Mealworm
Measuring instrument
Meat
Mechanical fan
Media
Medical assistant
Medical imaging
Medical procedure
Medicinal mushroom
Medicine
Medieval architecture
Mediterranean food
Meeting
Mehndi
Melastome family
Melitaea
Meller’s chameleon
Melon
Membrane-winged insect
Memorial
Menispermaceae
Menu
Mercedes-benz
Mercedes-benz c-class
Mercedes-benz clk-class
Mercedes-benz cls-class
Mercedes-benz e-class
Mercedes-benz w212
Mercedes-benz w221
Mertensia
Mesh
Metal
Metalworking
Meteorological phenomenon
Meter
Metro
Metro station
Metropolis
Metropolitan area
Mexican food
Meyer lemon
Meze
Microcontroller
Microphone
Microvan
Mid-size car
Middle ages
Midnight
Military
Military camouflage
Military organization
Military uniform
Military vehicle
Milk
Milkweed
Milling
Mimosa
Mimosa tenuiflora
Miner
Mineral
Mineral spring
Mini SUV
Miniature
Miniature pinscher
Miniature Poodle
Miniature schnauzer
Minibus
Mining
Minivan
Miridae
Mirror
Mirrorless interchangeable-lens camera
Mishloach manot
Missile boat
Mission
Mist
Mitsubishi
Mixed-use
Mixture
Mobile device
Mobile phone
Mobile phone accessories
Mobile phone case
mock orange
Mode of transport
Model car
Modern art
Modern dance
Moisture
Mold
Molding
Molluscs
Mombins
Momordica charantia
Monarch butterfly
Monastery
Money
Mongolian food
Monitor lizard
monkshood
Monochrome
Monochrome photography
Monolith
Monotreme
Monument
Moon
Moonflower
Moonlight
Moonlight cactus
Moraine
Morning
Morning glory
Morning glory family
Mortuary temple
Mosaic
Moschatel family
Moscow watchdog
Mosque
Mosquito
Moss
Moth
Moth Orchid
Motherboard
Moths and butterflies
Motif
Motor ship
Motor vehicle
Motorcycle
Motorsport
Mound
Mound-building termites
Mount scenery
Mountain
Mountain bike
Mountain bike racing
Mountain biking
Mountain Bluebird
Mountain goat
Mountain pass
Mountain range
Mountain river
Mountain village
Mountaineer
Mountaineering
Mountainous landforms
Moustache
Mouth
Movie palace
Movie theater
Mud
Mudflat
Mug
Mugwort
Mulberry family
Mulch
Multi-sport event
Multiflora rose
Multimedia
Munchkin
Mung bean
Mural
Muscle
Museum
Mushroom
Music
Music artist
Music venue
Musical ensemble
Musical instrument
Musical instrument accessory
Musical theatre
Musician
Musk deer
Muskmelon
Muskrat
Mustang horse
Mustard
Mustard and cabbage family
Mustard greens
Mustard plant
Mythology
Nachos
Nail
Nap
Narcissus
Narrow-body aircraft
Narrow-leaved sundrops
Narrows
National historic landmark
National monument
National park
Native raspberry
Nativity scene
Natural arch
Natural environment
Natural foods
Natural landscape
Natural material
Natural rubber
Nature
Nature reserve
Naval architecture
Naval ship
Navarin
Navy
Nebelung
Neck
Nectar
Needlework
Neighbourhood
Neon
Neon sign
Neotinea ustulata
Nepenthes
Nepeta
Nest
Net
Net-winged insects
Netbook
Nettle family
Network interface controller
Networking cables
new england aster
new york aster
Newspaper
Newsprint
Newt
Night
night heron
Nightclub
Nightingale
Nightlight
Nightshade family
Nissan
Nissan altima
Nissan cube
Nissan figaro
Nissan murano
Nissan primastar
Nissan qashqai
Nissan rogue
Nissan sentra
Nissan teana
Non-alcoholic beverage
Non-Sporting Group
Non-vascular land plant
Nonbuilding structure
Nopal
Nordic walking
Norfolk terrier
North american fraternity and sorority housing
Northern Cardinal
Northern hardwood forest
Norwegian forest cat
Norwich terrier
Nose
Notchback
Notebook
Novel
Noxious weed
Nuclear power plant
Number
Numeric keypad
Nunatak
Nurse
Nursing
Nut
Nymphalis
Oak
Oar
Oasis
Oat bran
Obedience training
Obelisk
Observation tower
Ocean
Ocicat
Ocimum
Ocimum tenuiflorum
Odometer
Oecanthidae
Off-road vehicle
Off-roading
Office
Office equipment
Office supplies
Official residence
Oil tanker
Oily fish
old field clover
Old World flycatcher
Old World oriole
Old-growth forest
Oleaster family
Olive
Onion
Ononis
Opel
Opel olympia
Open water swimming
Opera house
Operating system
Ophrys
Ophthalmology
Optoelectronics
Orange
orange hawkweed
Orange lily
Orator
Orb-weaver spider
Orchestra
Orchestra pit
Orchid
Orchids of the philippines
oregon pine
Organ
Organism
Organization
Oriental longhair
Orienteering
Orzo
Ostrich
Ostrich fern
Oud
Outcrop
Outdoor bench
Outdoor furniture
Outdoor play equipment
Outdoor recreation
Outdoor structure
Outdoor table
Outer space
Outerwear
Outhouse
Outlet store
Output device
Overcoat
Overhead power line
Overpass
Owl
Oxbow lake
Oyster mushroom
Pachyphytum
Pacific rhododendron
Pack animal
Packaging and labeling
Pad thai
Paddle
Paddy field
Paint
Paintball
Painting
Paisley
Palace
Palm tree
Panda
Panorama
Pansy
Paper
Paper product
Parachute
Parade
Parakeet
Parallel
Parasite
Pariah dog
Parish
Park
Parking
Parking lot
Parking meter
Parrot
Parsley
Parsley family
Party
Party supply
Passenger
Passenger car
Passenger ship
Passion flower
Passion flower family
Passive circuit component
Pasta
Pastry
Pasture
Patchouli
Patchwork
Path
Patient
Patio
Patrol boat
Patrol boat, river
Pattern
Paurotis Palm
Pavilion
Paw
Payment card
Pc game
Pea
Peach
Peach palm
Pearl
Pearl onion
Pebble
Peccary
Pedestrian
Pedestrian crossing
Pedicel
Pen
Pencil
Penguin
Peninsula
Peniocereus
Penny bun
Pennyroyal
Peony
People
People in nature
People on beach
Perching bird
Percussion
Percussionist
Perennial plant
perennial sowthistle
perforate st john’s wort
Performance
Performance art
Performance car
Performing arts
Performing arts center
Pergola
Perilla
Perilla frutescens
Persian
Personal computer
Personal computer hardware
Personal luxury car
Personal protective equipment
Personal water craft
Peruvian lily
Pest
Pet supply
Petal
Pezizales
Phallales
Pharaoh hound
Pharmaceutical drug
Phasianidae
Photo caption
Photo shoot
Photograph
Photographic paper
Photography
Photomontage
Phragmites
Phyllanthus family
Physical fitness
Phytolaccaceae
Pianist
Piciformes
Picket fence
Pickling
Pickup truck
Picnic
Picture frame
Pier
Pieridae
Pigeons and doves
Pill
Pillow
Pilot boat
Pinakbet
pincushion flower
Pine
Pine family
Pine nut
Pine Siskin
Pineapple
Pink
Pink evening primrose
Pink family
Pink peppercorn
pink quill
Pinkladies
Pipe
Pistacia lentiscus
Pit bull
Pit cave
Pittosporaceae
Pixie-bob
Place card
Place of worship
Placemat
Plain
Plan
Plane
Plane-tree family
Planet
Plank
Plant
Plant community
Plant pathology
Plant stem
Plantago
Plantation
Plaster
Plastic
Plastic bag
Plastic bottle
Plastic wrap
Plate
Plate girder bridge
Plateau
Play
Player
Playground
Playground slide
Playing in the snow
Plaza
Plebejus
Pleurotus eryngii
Plucked string instruments
Plum pine
Plum tomato
Plumbing
Plumbing fixture
Plush
Plymouth cranbrook
Plywood
Poales
Pocket
Podenco canario
Podophyllum peltatum
Poi
Point-and-shoot camera
Pointer
Polar ice cap
Polder
Pole
Pole dance
Police
Police car
Pollen
Pollinator
Pollution
Polydactyl cat
Polyommatus
Polyporales
Polystachya
Pomeranian
Pond
Pond turtle
Pontederia
Pontiac vibe
Pony
Poodle
Poodle crossbreed
Pool
Pop music
Poppy
Poppy family
Porcelain
Porch
Porsche
Port
Portable communications device
Porthole
Portrait
Portrait photography
Portuguese water dog
Post-it note
Poster
Poster session
Pot roast
Potato
Pottery
Poultry
Powder
Power station
Prairie
Pražský krysařík
Precipitation
Prescription drug
Present
Presentation
Preserved food
Presidential palace
Prickly pear
Prickly rose
Primula
Printmaking
Prism
Private school
Produce
Product
Professor
Project
Projection screen
Projector accessory
Promontory
Propane
Propeller
Propeller-driven aircraft
Property
Prophet
Protea family
Proteales
Protest
Prunus
Prunus spinosa
Prussian asparagus
Psychedelic art
Pub
Public event
Public library
Public space
Public speaking
Public transport
Public utility
Publication
Puddle
Pumi
Pump
Pumping station
Pumpkin
Pupa
Puppy
Puppy love
Purple
Purple loosestrife
Purple Martin
Purple passionflower
Purple salsify
Purslane family
Puzzle
Pyramid
Pyrenean mastiff
Pyrenean shepherd
Python
Python family
Quarry
Quill
Quilt
Quilting
Rabbit
Rabbits and Hares
Race car
Racer
Racing
Racing bicycle
Racing video game
Radar
Radio telescope
Radio-controlled helicopter
Radish
Rafeiro do alentejo
Raft
Rafting
Ragamuffin
Railroad car
Railway
Rain
Rain and snow mixed
Rain gauge
Rainbow
Rainforest
Raised beach
Rallying
Rambutan
Ramp
Ranch
Range rover
Rangpur
Rapeseed
Rapid
Rapini
Rare breed (dog)
Rat
Ratite
Rattlesnake
Ravine
Ray-finned fish
Reading
Real estate
Rear-view mirror
Rebellion
Receipt
Recipe
Recreation
Recreational fishing
Rectangle
Recycling
Recycling bin
Red
Red bud
Red clover
Red eared slider
Red juniper
Red leaf lettuce
Red onion
red pine
Red shouldered Hawk
Red sky at morning
Redbone coonhound
Redwood
Reef
Reflecting pool
Reflection
Reflex camera
Refried beans
Refrigerator
Regularity rally
Reindeer
Reinforced concrete
Relief
Religious institute
Religious item
Renault 12
Renault kangoo
Renault trafic
Reptile
Research institute
Reservoir
Residential area
Resistor
Resort
Resort town
Rest area
Restaurant
Restroom
Retail
Retriever
Retro style
Rhodesian ridgeback
Rhododendron
Rhubarb
Rickshaw
Ridge
Riding boot
Rifle
Rim
Ring
Ringed-worm
Riparian forest
Riparian zone
Ritual
River
River Birch
River delta
River island
River juniper
Road
Road bicycle
Road bicycle racing
Road cycling
Road surface
Road trip
robin
Rock
Rock climbing
Rock concert
Rock dove
Rock fishing
Rock python
rock rose
Rock-climbing equipment
Rodent
Rodeo
Rogaining
Rolling
Rolling stock
Romaine lettuce
Roman temple
Romance
Roof
Roof lantern
Roof rack
Room
Rooster
Root
Root vegetable
Rope
Rosa × centifolia
Rosa canina
Rosa dumalis
Rosa gallica
Rosa nutkana
Rosa rubiginosa
Rosa rugosa
Rosa wichuraiana
Rose
Rose family
Rose order
Rosemary
Rosy garlic
Rotifer
Rotorcraft
round leaved liverleaf
Rowan
Rowing
Roystonea
Rubble
Ruby crowned Kinglet
Ruins
Running
Runway
Rural area
Russian blue
Russian olive
Russkiy toy
Russula integra
Rust
Rutabaga
RV
Rye
Saab 92
Saba banana
Sabal minor
Sabal palmetto
Safari
Sage
Saguaro
Sahara
Sail
Sailboat
Sailing
Sailing ship
Salad
Salamander
Salt marsh
Salt-cured meat
Saltbush
San Pedro cactus
Sand
Sandpiper
Sapodilla family
Sardine
Sash window
Satay
Sauerbraten
Savanna
Savannah
Savoy cabbage
Saw palmetto
Scaffolding
Scale model
Scaled reptile
Scallion
Scaphosepalum
Scar
Scarf
Scene
Schematic
Schipperke
Schisandra
Schnitzel
Schnoodle
School
School bus
Schooner
Science
Scilla
Scion
Scooter
Scrap
Screen
Screenshot
Scrub Jay
Sculpture
Sea
sea aster
Sea beet
Sea cave
Sea ice
Sea kale
Sea kayak
Sea lettuce
Sea turtle
Sea urchin
Seabird
Seafood
Seamount
Seat 133
Seat belt
Seat of local government
Seaweed
Security lighting
Sedan
Sedge family
Seed
Seedless fruit
Segmental bridge
Sego lily
Self-help book
Self-portrait
Selfie
Selling
Seminar
Senna
Sensor
Serbian bellflower
Serveware
Service
Sesame
Sewing
Sewing machine
Sewing machine needle
Shack
Shadbush
Shade
Shadow
Shallot
Sharing
Sharp shinned Hawk
Shed
Sheep
Sheep shearer
Sheep’s sorrel
Shelf
shellbark hickory
Shelving
Shepherd
Shield volcano
Shih tzu
Shiitake
Ship
Shipping container
Shipwreck
Shirt
Shoe
Shooting
Shooting sport
Shopkeeper
Shopping
Shopping mall
Shore
Shorebird
shortleaf black spruce
Shorts
shortstraw pine
Shoulder
Shout
Shower
Shrine
Shrub
Shrubland
Siam tulip
Siberian
Side dish
Sidewalk
Sidewinder
Siding
Sign
Sign language
Signage
signaling device
Silene nutans
Silhouette
Silk
Silk tree
Sill
Silver
silvertip fir
Silybum
Simit
Singer
Singing
Singing sand
Single-lens reflex camera
Singleleaf pine
Sink
Sinkhole
Siren
sitka spruce
Sitting
Skateboard
Skateboarding
Skateboarding Equipment
Skatepark
Skerry
Sketch
Skewer
Ski mountaineering
Ski touring
Skiff
Skin
Skink
Skull
Skullcap
Sky
Skyline
Skyscraper
Skyway
Sleep
Sleeve
Sloop
Slope
Slug
Small appliance
Small münsterländer
Small terrier
Small to medium-sized cats
small white aster
Smartphone
smartweed-buckwheat family
Smile
Smoke
smooth aster
Smooth newt
smooth Solomon’s seal
Snack
Snails and slugs
Snake
Snap pea
Snapshot
Snout
Snout moths
Snow
Snowdrop
Snowshoe
Soccer
Soccer-specific stadium
Social group
Social work
Sofa bed
Software
Software engineering
Soil
Solanales
Solanum
Solar panel
Soldering iron
Soldier
Sole
Solomon’s plume
Solomon’s seal
Song
Song Sparrow
Songbird
Sorbus
Sorghum
Sorrel
Sound
Sound card
Sousaphone
Southernwood
Sow thistles
Sowing
Space
Space bar
Spacecraft
Spaniel
Spanish missions in california
Spanish water dog
Sparkler
Sparrow
Spathoglottis
Spear thistle
Spearmint
Spectacle
Speedboat
Speedometer
Speleothem
Sphere
Spider
Spider web
spiderwort
Spinach
Spiral
Spire
Spit
Spitz
Split-rail fence
Spoil tip
Spoke
Spoon
Sport climbing
Sport utility vehicle
Sport venue
Sporting Group
Sporting lucas terrier
Sports
Sports car
Sports equipment
Sports gear
Sports sedan
Sports training
Sports uniform
Sportswear
Spotted knapweed
Spring
spring crocus
Spring greens
Spring peeper
Sprouting
Spruce
Spruce-fir forest
Spurge family
Square
Squash
Squirrel
St. bernard
Stachys affinis
Stack
Stadium
Stage
Stage equipment
Stage is empty
Stain
Stained glass
Stairs
Stalagmite
Stall
Stallion
Stand up paddle surfing
Standard Poodle
Standing
Star
State park
Stately home
Stationery
Statue
Steam
Steam engine
Steamed rice
Steel
Steel-cut oats
Steeple
Steering part
Steering wheel
Stele
Stemware
Steppe
Sticker
Still life
Still life photography
Stir frying
Stitch
Stitchwort
Stock dove
Stock photography
Stone carving
Stone tool
Stone wall
Stonecrop family
Stony coral
Storage tank
Storm
Strait
Stratovolcano
Straw
Stream
Stream bed
Street
Street art
Street dog
Street fashion
Street food
Street light
Street performance
Street sign
Street sports
Street stunts
Streetball
String instrument
Student
Studio
studio couch
Studio monitor
Stuffed toy
Stuffing
Style
Subaru
Subaru outback
Subcompact car
Submarine
Subshrub
Suburb
Subway
Succulent plant
Suezmax
sugar pine
Suidae
Suit
Suit actor
Suite
Sulfur Cosmos
Suliformes
Summer
Summer savory
Summer squash
Summit
Sun
Sun hat
Sunday roast
sunflower
Sunglasses
Sunlight
Sunrise
Sunset
Supercar
Superfood
Superfruit
Supermarket
Supermini
Supper
Surface water sports
Surfboard
Surfer hair
Surfing
Suspension bridge
Suzuki
Suzuki swift
Swallow
Swamp
swamp birch
Sweater
sweet birch
Sweet corn
Sweet grass
Sweet gum
Sweet peas
Sweet scabious
Sweetness
Sweetscented bedstraw
Swimming
Swimming pool
Swing bridge
Switch
Symbol
Symmetry
Synagogue
Synthetic rubber
T-shirt
Tabby cat
Table
Tablecloth
Tablet computer
Tableware
Tachinidae
Tachometer
Tail
Take-out food
Takeoff
Talent show
Tall ship
Tamarillo
Tanacetum balsamita
Tangelo
Tangerine
Tango
Tank
Tansy
Tap
Tapestry
Tar
Tarantula
Tarn
Tarpaulin
tasmanian flax-lily
Taste
Tata safari
Tatsoi
Tavern
tawhana
Taxi
Taxus baccata
Tea
Teacher
Teacup
Teal
Team
Team sport
Technical drawing
Technology
Teddy bear
Teff
Telecommunications engineering
Television
Television set
Television transmitter
Temperate broadleaf and mixed forest
Temperate coniferous forest
Temple
Tent
Terminalia catappa
Termite
Terrace
Terrain
Terrapin
Terrestrial animal
Terrestrial plant
Terrier
texas bluebonnet
Text
Textile
Tgv
Thai food
Thai ridgeback
Thatching
Theaceae
Theater curtain
Theatre
Theatrical scenery
Thermokarst
Thigh
Thimbleberry
Thistle
Thorns, spines, and prickles
Thoroughfare
Thread
Throat
Throw pillow
Thumb
Thunder
Thunderstorm
thuya
Ti plant
Tibetan spaniel
Tibetan terrier
Tick
Ticket
Tickseed
Tidal marsh
Tide
Tide pool
Tie
Tights
Tile
Tin
Tin can
Tints and shades
Tire
Tire care
Titan arum
Toad
toad lily
Tobacco
Tobacco products
Toddler
Toe
Toilet
Toilet seat
Tomatillo
Tomato
tommie crocus
Tool
Toolroom
Tooth
Top
Torch lily
Torii
Tortoise
Tosa
Touchpad
Tour bus service
Tourism
Tourist attraction
Tournament
Tower
Tower block
Town
Town square
Townsend’s Warbler
Toy
Toy dog
Toy Poodle
Toy vehicle
Toyger
Toyota
Toyota camry solara
Toyota matrix
Toyota prius
Toyota yaris
Track
Trade
Trademark
Tradition
Traditional sport
Traffic
Traffic congestion
Traffic light
Traffic sign
Trail
Trail riding
Trailer
trailer truck
Train
Train station
Training
Tram
Transistor
Transmission tower
Transmitter station
Transparency
Transparent material
Transport
Travel
Travel trailer
Tray
Tread
Tree
Tree frog
tree poppy
Tree stump
Treeing walker coonhound
Trekking pole
Tremella
Trestle
Triangle
Triathlon
Tributary
Tricycle
Trillium
Trioceros
Trip computer
Tripod
Triticale
Triumphal arch
Trolleybus
Trophy hunting
Tropical and subtropical coniferous forests
Tropics
Trousers
Trout
Truck
True frog
True salamanders and newts
trumpet creeper
Trunk
Truss bridge
Tuba
Tuber
Tufted Titmouse
Tugboat
Tulip
Tundra
Tunnel
Turkey
Turkey Vulture
Turkish angora
Turkish coffee
Turkish van
Turquoise
Turret
Turtle
Tuxedo
Tv tuner card
Twig
Twine
twinflower
Two needle pinyon pine
Typewriter
Ultramarathon
Umbrella
Undergarment
Underwater
Underwing moths
Unesco world heritage site
Unidentified flying object
Uniform
Universe
University
Urban area
Urban design
Urinal
Urtica
Usb cable
Vacation
Valdivian temperate rain forest
Valencia orange
Valley
Van
Varanidae
Varnish
Vascular plant
Vase
Vault
Vaz-2101
Vegan nutrition
Vegetable
Vegetarian food
Vegetation
Vehicle
Vehicle brake
Vehicle door
Vehicle registration plate
Vein
Velvet
Venison
Verbascum
Verbena
Verbena family
Vernissage
Vertebrate
Viaduct
Viburnum
Vicuña
Video card
Vietnamese food
Vigna unguiculata subsp. sesquipedalis
Vihuela
Villa
Village
Vine
Vineyard
Vintage car
Viola
Violet
Violet family
Violin
Violin family
Viper
Virginia lungwort
Vision care
Visual arts
Visual effect lighting
Vitis
Vizsla
Volcanic crater
Volcanic field
Volcanic landform
Volcanic plug
Volcanic rock
Volkswagen
Volkswagen beetle
Volkswagen gli
Volkswagen golf
Volkswagen golf mk3
Volkswagen golf mk4
Volkswagen gti
Volkswagen jetta
Volkswagen karmann ghia
Volkswagen passat
Volkswagen pointer
Volvo amazon
Volvo v70
Volvo xc70
Wadi
Waist
Walking
Walkway
Wall
Wall lizard
Wall plate
Wallaby
Wallflower
Wallpaper
Warehouse
Warship
Wartburg 353
Wasabi
Washing
Washing machine
Wasp
Wasserfall
Waste
Waste container
Waste containment
Wat
Water
Water bird
Water bottle
Water dog
Water feature
Water forget me not
water lily
Water park
Water resources
water smartweed
Water sport
Water tank
Water tower
Water transportation
Watercolor paint
Watercourse
Watercraft
Watercraft rowing
Waterfall
Waterfowl
Watermelon
Waterway
Wave
Weaving
webbing clothes moth
Website
Wedding
Wedding dress
Weed
Weevil
West highland white terrier
West indian gherkin
Western Kingbird
Western Tanager
western yellow pine
Wetland
Wheat
Wheel
Whippet
Whiptail
Whiskers
Whisky
White
White pine
White rice
white throated sparrow
white trillium
White-collar worker
Whiteboard
Whitewater kayaking
Whole food
Whole grain
Whole-wheat flour
Wide-body aircraft
Wig
wild cabbage
wild carrot
Wild cat
wild cranesbill
wild pansy
wild sweet potato
Wild turkey
Wilderness
Wildfire
Wildflower
Wildlife
Wildlife biologist
Wind
Wind farm
Wind instrument
Wind turbine
Wind wave
windflower
Windjammer
Windmill
Window
Window blind
Window covering
Window film
Window screen
Window treatment
Windscreen wiper
Windshield
Windsor chair
Windsurfing
Wine
Wine bottle
Wine glass
Wing
Winter
Winter melon
Winter savory
Winter squash
Winter storm
Wire
Wire fencing
Wok
Wolf spider
Wonders of the world
Wood
Wood ear
Wood flooring
Wood Frog
wood rabbit
Wood sorrel family
Wood stain
Woodland
Woodpecker finch
Woodwind instrument
Woody plant
Wool
Woolen
Woolflowers
Woolly sunflower
Workbench
Working animal
Working dog
Workshop
World
World rally championship
Worm
Woven fabric
Wreath
Wren
Wrinkle
Wrist
Writing
Xanthorrhoeaceae
Xanthosoma
Xylophone
Yacht
Yak
Yard
Yarrow
Yellow
Yellow fir
yellow iris
Yellow nutsedge
Yellow rumped Warbler
yellow toadflax
Yew family
Yorkshire terrier
Youth
Yucca
Zaminkand
Zebra crossing
zedoary
Zingiberales
Zinnia angustifolia
Zipper
Zombie
Zoo
Zooplankton
Zucchini 
*/