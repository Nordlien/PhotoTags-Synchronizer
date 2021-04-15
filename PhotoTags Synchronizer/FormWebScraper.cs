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
using FastColoredTextBoxNS;
using MetadataLibrary;
using Newtonsoft.Json;
//using PhotoTagsSynchronizer.Properties;

namespace PhotoTagsSynchronizer
{
    public partial class FormWebScraper : Form
    {
        public MetadataDatabaseCache DatabaseAndCacheMetadataExiftool { get; set; }
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ChromiumWebBrowser browser;
        private AutoResetEvent autoResetEventWaitPageLoading = null;
        private AutoResetEvent autoResetEventWaitPageLoaded = null;
        private bool isProcessRunning = false;
        private int javaScriptExecuteTimeout = 5000;
        private int webScrapingRetry = 20;
        private int webScrapingDelayOurScriptToRun = 200;
        private int webScrapingDelayInPageScriptToRun = 1000;
        private int waitEventPageStartLoadingTimeout = 3000;
        private int waitEventPageLoadedTimeout = 20000;
        private int webScrapingPageDownCount = 5;
        private string webScrapingName = MetadataLibrary.MetadataDatabaseCache.WebScapingFolderName;
        private string[] webScrapingStartPages;

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

        private bool stopRequested = false;

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
            buttonWebScrapingStart.Enabled = enabled;
            buttonWebScrapingSave.Enabled = enabled;

            buttonWebScrapingLoadPackage.Enabled = enabled && (listViewCategoryGroups.Items.Count > 0);
            listViewCategoryGroups.Enabled = enabled && (listViewCategoryGroups.Items.Count > 0);            
        }
        #endregion 

        #region Init
        public FormWebScraper()
        {
            InitializeComponent();
            IsAnyPageLoaded = false;

            autoResetEventWaitPageLoaded = new AutoResetEvent(false);
            autoResetEventWaitPageLoading = new AutoResetEvent(false);

            lvwColumnSorter = new ListViewColumnSorter();
            listViewLinks.ListViewItemSorter = lvwColumnSorter;
            listViewCategoryGroups.ListViewItemSorter = lvwColumnSorter;

            

            browser = new ChromiumWebBrowser("https://photos.google.com/")
            {
                Dock = DockStyle.Fill,
            };
            browser.BrowserSettings.Javascript = CefState.Enabled;
            browser.BrowserSettings.WebSecurity = CefState.Enabled;
            browser.BrowserSettings.WebGl = CefState.Enabled;
            browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Disabled;
            browser.BrowserSettings.Plugins = CefState.Enabled;
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
        }
        #endregion 

        #region Browser - LoadingStateChanged
        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Debug.WriteLine("Browser_LoadingStateChanged" + e.IsLoading);
            if (e.IsLoading == false) IsAnyPageLoaded = true;
            if (e.IsLoading == true && autoResetEventWaitPageLoading != null) autoResetEventWaitPageLoading.Set();
            if (e.IsLoading == false && autoResetEventWaitPageLoaded != null) autoResetEventWaitPageLoaded.Set();
        }
        #endregion

        #region Browser - FrameLoadEnd
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Debug.WriteLine("FrameLoadEnd: " + e.Frame.Name + " " + e.HttpStatusCode + " " + e.Url);

            if (!e.Frame.IsMain && e.Frame.IsValid && !e.Frame.IsDisposed)
            {
            }

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

            Debug.WriteLine("Browser_AddressChanged: " + e.Address);
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
            if (sleep) Thread.Sleep(webScrapingDelayInPageScriptToRun);
            return result;
        }
        #endregion 

        #region GUI - textBoxBrowserURL_KeyPress
        private void textBoxBrowserURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; //Handle the Keypress event (suppress the Beep)
                browser.Load(textBoxBrowserURL.Text);
            }
        }
        #endregion

        #region GUI - buttonBrowserShowDevTool_Click
        private void buttonBrowserShowDevTool_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }
        #endregion

        #region class ScrapingResult
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
                int hashCode = 1517053096;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Url);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
                hashCode = hashCode * -1521134295 + PictureInfoScreenHidden.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(LinkPhoto);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Album);
                hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(AlbumOthers);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MediaFile);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LocationName);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tag);
                hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(Tags);
                hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(Peoples);
                hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(LinksAlbum);
                hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(LinksTags);
                hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(LinksLocation);
                hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(LinksPeople);
                return hashCode;
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

        #region Scraping - Add Result
        private class LinkCategory
        {
            [JsonProperty("Name")]
            public string Name { get; set; } = "";

            [JsonProperty("Link")]
            public string Link { get; set; } = "";

            [JsonProperty("Category")]
            public string Category { get; set; } = "";

            [JsonProperty("LastRead")]
            public DateTime? LastRead { get; set; } = null;
        }

        Dictionary<string, LinkCategory> _linkCatergories = new Dictionary<string, LinkCategory>();
        Dictionary<string, Metadata> _metaDataDictionary = new Dictionary<string, Metadata>();

        List<string> _urlLoadingFailed = new List<string>();

        #region Scraping - GetScrapingResult
        private ScrapingResult GetScrapingResult(List<object> list)
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

        #endregion

        #region RunScript sync
        public async Task<object> EvaluateScript(string script, object defaultValue, TimeSpan timeout)
        {
            object result = defaultValue;
            if (browser.IsBrowserInitialized && !browser.IsDisposed && !browser.Disposing)
            {
                try
                {
                    var task = browser.EvaluateScriptAsync(script, timeout);
                    await task.ContinueWith(res => {
                        if (!res.IsFaulted)
                        {
                            var response = res.Result;
                            result = response.Success ? (response.Result ?? "null") : response.Message;
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

        #region GUI - show result javaScript
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
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            fastColoredTextBoxJavaScriptResult.ResumeLayout();
            IsProcessRunning = false;
        }
        #endregion

        #region GUI - Save JavaScript
        private void buttonSaveJavaScript_Click(object sender, EventArgs e)
        {
            IsProcessRunning = true;
            Properties.Settings.Default.WebScraperScript = fastColoredTextBoxJavaScript.Text;
            Properties.Settings.Default.Save();
            IsProcessRunning = false;
        }
        #endregion

        #region Scraping - With Retry
        private async Task<ScrapingResult> Scraping(string script, int retryWhenVerifyFails, bool verifyDiffrentResult, bool verifyPhotoLinksCount, bool verifyMediaFileFound)
        {
            bool newFound = true;
            ScrapingResult scrapingResult = null;
            ScrapingResult lastScrapingResult = null;

            do
            {
                Debug.WriteLine("Scraping, retry left: " + retryWhenVerifyFails);
                Thread.Sleep(webScrapingDelayOurScriptToRun); //Give script some time to run

                try
                {
                    object result = await EvaluateScript(script, null, TimeSpan.FromMilliseconds(javaScriptExecuteTimeout));
                    if (result is List<object>)
                    {
                        scrapingResult = GetScrapingResult((List<object>)result);
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
                    Logger.Error("Task<ScrapingResult> Scraping: " + ex.Message);
                    newFound = false;
                    retryWhenVerifyFails = 0;
                }

                lastScrapingResult = scrapingResult;
            } while (!newFound && retryWhenVerifyFails-- > 0);

            return scrapingResult;
        }
        #endregion

        

        #region Scraping - Media Files in URL
        private async Task<object> ScrapingMediaFiles(string script, string url, string tag, string album, string people, string location, 
            int retryWhenVerifyFails, Dictionary<string, Metadata> metaDataDictionary, List<string> urlLoadingFailed)
        {
            Debug.WriteLine(url);
            browser.Load(url);

            WaitPageLoadedEvent();
            

            ScrapingResult scrapingResult = await Scraping(script, retryWhenVerifyFails, true, true, false);

            if (scrapingResult != null && scrapingResult.LinkPhoto.Count>0)
            {
                browser.Load(scrapingResult.LinkPhoto[0]);
                WaitPageLoadedEvent();

                bool isPageLoading;
                do
                {
                    isPageLoading = false;
                    scrapingResult = await Scraping(script, retryWhenVerifyFails, true, false, true);

                    if (scrapingResult != null && scrapingResult.PictureInfoScreenHidden)
                    {
                        SendKeyBrowser(Keys.I);
                        Thread.Sleep(webScrapingDelayInPageScriptToRun);
                        scrapingResult = await Scraping(script, retryWhenVerifyFails, true, false, true);
                    }

                    if (scrapingResult?.MediaFile == null && scrapingResult?.Url != null)
                    {
                        browser.Load(scrapingResult?.Url);
                        WaitPageLoadedEvent();
                        scrapingResult = await Scraping(script, retryWhenVerifyFails, true, false, true);
                    }

                    if (scrapingResult?.MediaFile != null)
                    {
                        if (!metaDataDictionary.ContainsKey(scrapingResult.MediaFile))
                            metaDataDictionary.Add(scrapingResult.MediaFile, new Metadata(MetadataBrokerType.WebScraping));

                        Metadata metadata = metaDataDictionary[scrapingResult.MediaFile];

                        metadata.FileName = scrapingResult.MediaFile;
                        metadata.FileDirectory = scrapingResult.Url;
                        metadata.FileDateCreated = DateTime.Now;
                        metadata.FileDateModified = DateTime.Now;
                        metadata.FileLastAccessed = DateTime.Now;

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

            return null;
        }
        #endregion




        #region Scraping - Catergories
        private void AddUpdatedCategoryList(Dictionary<string, LinkCategory> linkCatergories, Dictionary<string, string> links, string categryName)
        {
            foreach (KeyValuePair<string, string> keyValuePair in links)
            {
                LinkCategory linkCategory;
                if (!linkCatergories.ContainsKey(keyValuePair.Key)) linkCatergories.Add(keyValuePair.Key, new LinkCategory());
                linkCategory = linkCatergories[keyValuePair.Key];
                linkCategory.Category = categryName;
                linkCategory.Link = keyValuePair.Key;
                linkCategory.Name = keyValuePair.Value;
                //linkCategory.LastRead = null;
            }
        }
        #endregion 

        #region CategryLinks - Scraping
        private async Task<ScrapingResult> ScrapingCategory(string script, string url, Dictionary<string, LinkCategory> linkCatergories)            
        {
            Debug.WriteLine(url);
            browser.Load(url);
            WaitPageLoadedEvent();

            int scrollPageDownCount = webScrapingPageDownCount;
            bool newFound;
            ScrapingResult scrapingResult;
            ScrapingResult lastScrapingResult = null;

            do {
                scrapingResult = await Scraping(script, webScrapingRetry, true, false, false);

                if (scrapingResult != null)
                {                    
                    AddUpdatedCategoryList(linkCatergories, scrapingResult.LinksAlbum, "Album");
                    AddUpdatedCategoryList(linkCatergories, scrapingResult.LinksTags, "Tag");
                    AddUpdatedCategoryList(linkCatergories, scrapingResult.LinksPeople, "People");
                    AddUpdatedCategoryList(linkCatergories, scrapingResult.LinksLocation, "Location");
                }

                SendKeyBrowser(Keys.PageDown);

                if (lastScrapingResult != scrapingResult)
                {
                    newFound = true;
                    scrollPageDownCount = webScrapingPageDownCount;
                    Debug.WriteLine("-----Unequal found");
                }                    
                else
                {
                    newFound = false;
                    scrollPageDownCount--;
                    Debug.WriteLine("-----EQUAL FOUND");
                }
                lastScrapingResult = scrapingResult;
                
                Debug.WriteLine(
                    (!stopRequested) + " || (" + 
                    (!newFound) + "&&" + 
                    (scrollPageDownCount > 0) + ") =" + 
                    (!newFound && scrollPageDownCount > 0) + " == " 
                    + (!stopRequested && (newFound || (!newFound && scrollPageDownCount > 0))) + " " + scrollPageDownCount);

            } while (!stopRequested && (newFound || (!newFound && scrollPageDownCount > 0)));
            
            return scrapingResult;
        }
        #endregion

        private const int itemViewSubItemName = 0;
        private const int itemViewSubItemCategry = 1;
        private const int itemViewSubItemLink = 2;
        private const int itemViewSubItemLastUsed = 3;

        #region CategryLinks - Updated GUI
        private void UpdatedCategryLinksGUI(Dictionary<string, LinkCategory> linkCatergories)
        {
            foreach (LinkCategory linkCategory in linkCatergories.Values)
            {
                if (!string.IsNullOrWhiteSpace(linkCategory.Name))
                {
                    int itemFoundIndex = -1;
                    for (int itemIndex = 0; itemIndex < listViewLinks.Items.Count; itemIndex++)
                    {
                        if (listViewLinks.Items[itemIndex].SubItems[itemViewSubItemLink].Text == linkCategory.Link)
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
                    }
                    else
                    {
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemName].Text = linkCategory.Name;
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemCategry].Text = linkCategory.Category;
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLink].Text = linkCategory.Link;
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLastUsed].Text = linkCategory.LastRead.ToString();
                        listViewLinks.Items[itemFoundIndex].SubItems[itemViewSubItemLastUsed].Tag = linkCategory.LastRead;
                    }
                }
            }
        }
        #endregion 

        #region GUI - Start Scraping - Categories
        private async void WebScrapingCategories_Click(object sender, EventArgs e)        
        {
            IsProcessRunning = true;
            try
            {                
                
                listViewLinks.Items.Clear();

                string script = Properties.Settings.Default.WebScraperScript;

                foreach (string webPage in webScrapingStartPages) if (!stopRequested) _ = await ScrapingCategory(script, webPage.Trim(), _linkCatergories);
                /*
                if (!stopRequested) _ = await ScrapingCategory(script, "https://photos.google.com/things", _linkCatergories);
                if (!stopRequested) _ = await ScrapingCategory(script, "https://photos.google.com/places", _linkCatergories);
                if (!stopRequested) _ = await ScrapingCategory(script, "https://photos.google.com/albums", _linkCatergories);
                if (!stopRequested) _ = await ScrapingCategory(script, "https://photos.google.com/people", _linkCatergories);*/

                UpdatedCategryLinksGUI(_linkCatergories);
                WebScrapingWriteStatus(_linkCatergories);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IsProcessRunning = false;
        }
        #endregion


        #region WebScraping - Filename
        private string WebScrapingFilename()
        {
            try
            {
                string jsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer");
                if (!Directory.Exists(jsonPath)) Directory.CreateDirectory(jsonPath);
                return Path.Combine(jsonPath, "Status.WebScrapring.json");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
        #endregion

        #region WebScraping - ReadStatus
        private Dictionary<string, LinkCategory> WebScrapingReadStatus()
        {
            Dictionary<string, LinkCategory> result = new Dictionary<string, LinkCategory>();
            string filename = WebScrapingFilename();
            try
            {
                if (File.Exists(filename))
                {
                    result = JsonConvert.DeserializeObject<Dictionary<string, LinkCategory>>(File.ReadAllText(filename));
                }
            } catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }
        #endregion

        #region WebScraping - WriteStatus
        private void WebScrapingWriteStatus(Dictionary<string, LinkCategory> linkCatergories)
        {
            string filename = WebScrapingFilename();
            try
            {
                File.WriteAllText(filename, JsonConvert.SerializeObject(linkCatergories, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion 

        #region GUI - Start Scraping - Selected Media Files
        private async void buttonWebScrapingStart_Click(object sender, EventArgs e)
        {
            IsProcessRunning = true;
            
            try
            {
                //string script = Properties.Settings.Default.WebScraperScript;
                string script = fastColoredTextBoxJavaScript.Text;

                _urlLoadingFailed.Clear();
                foreach (ListViewItem listViewItem in listViewLinks.Items)
                {
                    if (listViewItem.Checked)
                    {
                        string url = listViewItem.SubItems[itemViewSubItemLink].Text;

                        if (_linkCatergories.ContainsKey(url))
                        {
                            LinkCategory linkCategory = _linkCatergories[url];
                            switch (linkCategory.Category)
                            {
                                case "Tag":
                                    await ScrapingMediaFiles(script, linkCategory.Link, linkCategory.Name, null, null, null, webScrapingRetry, _metaDataDictionary, _urlLoadingFailed);
                                    break;
                                case "Album":
                                    await ScrapingMediaFiles(script, linkCategory.Link, null, linkCategory.Name, null, null, webScrapingRetry, _metaDataDictionary, _urlLoadingFailed);
                                    break;
                                case "People":
                                    await ScrapingMediaFiles(script, linkCategory.Link, null, null, linkCategory.Name, null, webScrapingRetry, _metaDataDictionary, _urlLoadingFailed);
                                    break;
                                case "Location":
                                    await ScrapingMediaFiles(script, linkCategory.Link, null, null, null, linkCategory.Name, webScrapingRetry, _metaDataDictionary, _urlLoadingFailed);
                                    break;
                            }
                            if (!stopRequested)
                            {
                                linkCategory.LastRead = DateTime.Now;
                            }
                            UpdatedCategryLinksGUI(_linkCatergories);
                        }
                    }
                }

                WebScrapingWriteStatus(_linkCatergories);

                fastColoredTextBoxJavaScriptResult.Text = "Scraping result:\r\n";
                CountMetadata(_metaDataDictionary);

                if (_urlLoadingFailed.Count > 0) fastColoredTextBoxJavaScriptResult.Text += "Urls failed to load: " + _urlLoadingFailed.Count + "\r\n";
                foreach (string failedUrl in _urlLoadingFailed) fastColoredTextBoxJavaScriptResult.Text += "Regions found: " + failedUrl + "\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            IsProcessRunning = false;
        }
        #endregion

        #region GUI - FormClosing
        private void FormWebScraper_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsProcessRunning)
            {
                MessageBox.Show("Need wait process that are running has stopped, before closing window");
                e.Cancel = true;
            }
        }
        #endregion

        #region GUI - List View Sort
        private ListViewColumnSorter lvwColumnSorter;
        private void listViewLinks_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending) lvwColumnSorter.Order = SortOrder.Descending;
                else lvwColumnSorter.Order = SortOrder.Ascending;                
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            ((ListView)sender).Sort();
        }

        private void listViewCategoryGroups_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending) lvwColumnSorter.Order = SortOrder.Descending;
                else lvwColumnSorter.Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            ((ListView)sender).Sort();
        }
        #endregion

        #region GUI - Select all, none, toggle in ListView
        private void buttonWebScrapingSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewLinks.Items) listViewItem.Checked = true;            
        }

        private void buttonWebScrapingSelectNotRead_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewLinks.Items) listViewItem.Checked = !(listViewItem.SubItems[itemViewSubItemLastUsed].Tag is DateTime);
        }

        private void buttonWebScrapingToggle_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewLinks.Items) listViewItem.Checked = !listViewItem.Checked;
        }

        private void buttonWebScrapingSelectNone_Click(object sender, EventArgs e)
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

        #region GUI - WebScraping - Save - Click
        private void buttonWebScrapingSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                buttonWebScrapingSave.Enabled = false;
                if (_metaDataDictionary.Count > 0)
                {
                    DateTime dateTimeSaveDate = DateTime.Now;
                    DatabaseAndCacheMetadataExiftool.TransactionBeginBatch();
                    foreach (Metadata metadata in _metaDataDictionary.Values)
                    {
                        metadata.FileDateModified = dateTimeSaveDate;
                        metadata.FileDirectory = webScrapingName;
                        metadata.FileSize = -1;
                        DatabaseAndCacheMetadataExiftool.WebScrapingWrite(metadata);
                    }
                    DatabaseAndCacheMetadataExiftool.TransactionCommitBatch();
                    _webScrapingPackages.Insert(0, dateTimeSaveDate);
                    UpdatedWebScrapingPackageList(_webScrapingPackages);
                }
                buttonWebScrapingSave.Enabled = false;
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region GUI - UpdatedWebScrapingPackageList
        private void UpdatedWebScrapingPackageList(List<DateTime> webScrapingPackages)
        {
            listViewCategoryGroups.Items.Clear();
            foreach (DateTime dateTime in webScrapingPackages)
            {
                ListViewItem listViewItem = new ListViewItem(dateTime.ToString());
                listViewItem.Tag = dateTime;
                listViewCategoryGroups.Items.Add(listViewItem);
                
            }

            if (webScrapingPackages.Count == 0)
            {
                buttonWebScrapingLoadPackage.Enabled = false;
                listViewCategoryGroups.Enabled = false;
            }
            else
            {
                buttonWebScrapingLoadPackage.Enabled = true;
                listViewCategoryGroups.Enabled = true;
                listViewCategoryGroups.Items[0].Selected = true;
            }
        }
        #endregion 


        List<DateTime> _webScrapingPackages = new List<DateTime>();

        #region GUI - FormWebScraper_Load
        private void FormWebScraper_Load(object sender, EventArgs e)
        {
            _webScrapingPackages = DatabaseAndCacheMetadataExiftool.ListWebScraperPackages(MetadataBrokerType.WebScraping, webScrapingName);
            UpdatedWebScrapingPackageList(_webScrapingPackages);

            _linkCatergories = WebScrapingReadStatus();
            UpdatedCategryLinksGUI(_linkCatergories);
        }
        #endregion 

        #region GUI - CountMetadata
        private void CountMetadata(Dictionary<string, Metadata> metadataList)
        {
            int mediaFiles = 0;
            int titles = 0;
            int albumNames = 0;
            int locationNames = 0;
            int keywordTags = 0;
            int regions = 0;

            foreach (Metadata metadata in _metaDataDictionary.Values)
            {
                mediaFiles++;
                if (metadata.PersonalTitle != null) titles++;
                if (metadata.PersonalAlbum != null) albumNames++;
                if (metadata.LocationName != null) locationNames++;
                foreach (KeywordTag keywordTag in metadata.PersonalKeywordTags) keywordTags++;
                foreach (RegionStructure regionStructure in metadata.PersonalRegionList) regions++;
            }

            fastColoredTextBoxJavaScriptResult.Text += "Media files found: " + mediaFiles.ToString() + "\r\n" +
                "Titles found: " + titles.ToString() + "\r\n" +
                "Album Names found: " + albumNames.ToString() + "\r\n" +
                "Location Names found: " + locationNames.ToString() + "\r\n" +
                "Keyword Tags found: " + keywordTags.ToString() + "\r\n" +
                "Regions found: " + regions.ToString() + "\r\n\r\n";
        }
        #endregion 

        #region GUI - Load WebScraping Package
        private void buttonWebScrapingLoadPackage_Click(object sender, EventArgs e)
        {
            buttonWebScrapingLoadPackage.Enabled = false;
            buttonWebScrapingSave.Enabled = false;
            DatabaseAndCacheMetadataExiftool.OnReadRecord -= DatabaseAndCacheMetadataExiftool_OnReadRecord;
            DatabaseAndCacheMetadataExiftool.OnReadRecord += DatabaseAndCacheMetadataExiftool_OnReadRecord;
            readCount = 0;
            foreach (ListViewItem listViewItem in listViewCategoryGroups.Items)
            {
                if (listViewItem.Checked && listViewItem.Tag is DateTime)
                {
                    DateTime packageDateTime = (DateTime)listViewItem.Tag;

                    DatabaseAndCacheMetadataExiftool.ReadLot(MetadataBrokerType.WebScraping, webScrapingName, null, packageDateTime, true);
                    List<FileEntryBroker> fileEntryBrokers = DatabaseAndCacheMetadataExiftool.ListMediafilesInWebScraperPackages(MetadataBrokerType.WebScraping, webScrapingName, packageDateTime);

                    fastColoredTextBoxJavaScriptResult.Text = "Before merge:\r\n";
                    CountMetadata(_metaDataDictionary);

                    foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
                    {
                        Metadata metadata = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOrDatabase(fileEntryBroker);
                        if (_metaDataDictionary.ContainsKey(metadata.FileName)) _metaDataDictionary[metadata.FileName] = Metadata.MergeMetadatas(_metaDataDictionary[metadata.FileName], metadata);
                        else _metaDataDictionary.Add(metadata.FileName, metadata);
                    }

                    fastColoredTextBoxJavaScriptResult.Text += "After merge:\r\n";
                    CountMetadata(_metaDataDictionary);
                    
                }
            }
            buttonWebScrapingLoadPackage.Enabled = true;
            buttonWebScrapingSave.Enabled = true;
        }
        private int readCount = 0;
        private Stopwatch stopwatchCounter = new Stopwatch();

        private void DatabaseAndCacheMetadataExiftool_OnReadRecord(object sender, ReadRecordEventArgs e)
        {
            readCount++;

            if (!stopwatchCounter.IsRunning) stopwatchCounter.Start();
            if (stopwatchCounter.ElapsedMilliseconds > 300)
            {
                Application.DoEvents();
                stopwatchCounter.Restart();
            }
            toolStripStatusLabelStatus.Text = "Read count: " + readCount.ToString();
        }

        #endregion

        #region GUI - WebScrapingCategoryGroup - Select - All/Toggle/None
        private void buttonWebScrapingCategoryGroupSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewCategoryGroups.Items) listViewItem.Checked = true;
        }

        private void buttonWebScrapingCategoryGroupSelectToggle_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewCategoryGroups.Items) listViewItem.Checked = !listViewItem.Checked;
        }

        private void buttonWebScrapingCategoryGroupSelectNone_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in listViewCategoryGroups.Items) listViewItem.Checked = false;
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
            else if (decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out decimal num))
            {
                compareResult = decimal.Compare(num, Convert.ToDecimal(listviewY.SubItems[ColumnToSort].Text));
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
