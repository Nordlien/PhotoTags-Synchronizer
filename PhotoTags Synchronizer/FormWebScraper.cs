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
                    "Load last metadata dataset?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, showCtrlCopy: true) == DialogResult.Yes)
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
                KryptonMessageBox.Show("Need wait process that are running has stopped, before closing window", "Process still working...", MessageBoxButtons.OK, MessageBoxIcon.Information, showCtrlCopy: true);
                e.Cancel = true;
            }
            if (isDataUnsaved)
            {
                if (KryptonMessageBox.Show("DataSet is unsaved! Will you close widthout saving data?", "Warning, unsaved DataSet", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, showCtrlCopy: true) == DialogResult.Cancel) e.Cancel = true;
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
            if (sleep) Task.Delay(webScrapingDelayInPageScriptToRun).Wait();
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
                KryptonMessageBox.Show(ex.Message, "Running JavaScript failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);

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
                KryptonMessageBox.Show(ex.Message, "Can't save settings...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show(ex.Message, "WebScraping failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
            IsProcessRunning = false;
        }
        #endregion

        #region json - WebScraping - Filename
        private string WebScrapingFilename(string filename)
        {
            try
            {
                return FileHandler.GetLocalApplicationDataPath(filename, false, this);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Get LocalApplicationDataPath failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show(ex.Message, "DeserializeObject failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show(ex.Message, "SerializeObject failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show(ex.Message, "DeserializeObject failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                File.WriteAllText(filename, JsonConvert.SerializeObject(webScrapingDataSet, Formatting.Indented));
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "SerializeObject and save failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show(ex.Message, "Start WebScraping failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show(ex.Message, "Start WebScaring categories failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
                KryptonMessageBox.Show(ex.Message, "Saveing WebScraping failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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
            buttonWebScrapingLoadPackage.Enabled = false;
            buttonWebScrapingSave.Enabled = false;

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

            buttonWebScrapingLoadPackage.Enabled = true;
            buttonWebScrapingSave.Enabled = true;
        }


        #endregion

        #region GUI - Delete WebScraping DataSet
        private void buttonSpecNavigatorDataSetSelectDelete_Click(object sender, EventArgs e)
        {
            buttonSpecNavigatorDataSetSelectDelete.Enabled = ButtonEnabled.False;

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

            buttonSpecNavigatorDataSetSelectDelete.Enabled = ButtonEnabled.True; 
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
            _metadataDataTotalMerged.Clear();
            ClearDataSetDescription();
            fastColoredTextBoxJavaScriptResult.Text = "Metadata DataSet cleared\r\n";
            
            MetadataDataSetCount metadataDataSetCountAll = MetadataDataSetCount.CountMetadataDataSet(_metadataDataTotalMerged);
            fastColoredTextBoxJavaScriptResult.Text +=
                "Current count for DataSet\r\n" +
                metadataDataSetCountAll.ToString() +
                fastColoredTextBoxJavaScriptResult.Text;
        }









        #endregion

        #region GUI - WebScrapingAddUserTags_Click
        private void kryptonButtonWebScrapingAddUserTags_Click(object sender, EventArgs e)
        {
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
        #endregion

        #region Export
        private void buttonWebScrapingExportDataSet_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                //saveFileDialog1.InitialDirectory = @ "C:\";
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
                    buttonWebScrapingExportDataSet.Enabled = false;
                    buttonWebScrapingImportDataSet.Enabled = false;

                    var output = JsonConvert.SerializeObject(_metadataDataTotalMerged, Formatting.Indented);
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, output, Encoding.UTF8);
                    KryptonMessageBox.Show(_metadataDataTotalMerged.Count.ToString() + " metadata exported", "Metadata exported", MessageBoxButtons.OK, MessageBoxIcon.Information, showCtrlCopy: true);

                    buttonWebScrapingExportDataSet.Enabled = true;
                    buttonWebScrapingImportDataSet.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error saving JSON file!\r\n\r\n" + ex.Message, "Was not able to save JSON file", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }
        #endregion

        #region Import
        private void buttonWebScrapingImportDataSet_Click(object sender, EventArgs e)
        {
            //DataGridView dataGridView = dataGridViewLocationNames;

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

                    KryptonMessageBox.Show(readResultSet.Count.ToString() + " locations imported", "Location file imported", MessageBoxButtons.OK, MessageBoxIcon.Information, showCtrlCopy: true);
                }
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show("Error loading JSON file!\r\n\r\n" + ex.Message, "Was not able to load JSON file", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
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

            if (listviewX.SubItems[ColumnToSort].Tag is DateTime && listviewY.SubItems[ColumnToSort].Tag is DateTime)
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
