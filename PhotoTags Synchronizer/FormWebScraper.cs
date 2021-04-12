using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using FastColoredTextBoxNS;
using MetadataLibrary;

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
        private int wapScrapingDelayInPageScriptToRun = 1000;
        private int waitEventPageStartLoadingTimeout = 3000;
        private int waitEventPageLoadedTimeout = 20000;
        private int webScrapingPageDownCount = 5;
        private string webScrapingName = MetadataLibrary.MetadataDatabaseCache.WebScapingFolderName;


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

            buttonWebScrapingLoadPackage.Enabled = enabled && (comboBoxWebScapingLoadPackage.Items.Count > 0);
            comboBoxWebScapingLoadPackage.Enabled = enabled && (comboBoxWebScapingLoadPackage.Items.Count > 0);            
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
            if (sleep) Thread.Sleep(wapScrapingDelayInPageScriptToRun);
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
        Dictionary<string, string> _linksAlbum = new Dictionary<string, string>();
        Dictionary<string, string> _linksTags = new Dictionary<string, string>();
        Dictionary<string, string> _linksPeople = new Dictionary<string, string>();
        Dictionary<string, string> _linksLocation = new Dictionary<string, string>();
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
                        Thread.Sleep(wapScrapingDelayInPageScriptToRun);
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
        private async Task<ScrapingResult> ScrapingCategory(string script, string url, 
            Dictionary<string, string> linksAlbum, 
            Dictionary<string, string> linkTags,
            Dictionary<string, string> linkLocation,
            Dictionary<string, string> linkPeople)
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
                    foreach (KeyValuePair<string, string> keyValuePair in scrapingResult.LinksAlbum)
                    {
                        if (!linksAlbum.ContainsKey(keyValuePair.Key)) linksAlbum.Add(keyValuePair.Key, keyValuePair.Value);
                        else linksAlbum[keyValuePair.Key] = keyValuePair.Value;
                    }

                    foreach (KeyValuePair<string, string> keyValuePair in scrapingResult.LinksTags)
                    {
                        if (!linkTags.ContainsKey(keyValuePair.Key)) linkTags.Add(keyValuePair.Key, keyValuePair.Value);
                        else linkTags[keyValuePair.Key] = keyValuePair.Value;
                    }

                    foreach (KeyValuePair<string, string> keyValuePair in scrapingResult.LinksLocation)
                    {
                        if (!linkLocation.ContainsKey(keyValuePair.Key)) linkLocation.Add(keyValuePair.Key, keyValuePair.Value);
                        else linkLocation[keyValuePair.Key] = keyValuePair.Value;
                    }

                    foreach (KeyValuePair<string, string> keyValuePair in scrapingResult.LinksPeople)
                    {
                        if (!linkPeople.ContainsKey(keyValuePair.Key)) linkPeople.Add(keyValuePair.Key, keyValuePair.Value);
                        else linkPeople[keyValuePair.Key] = keyValuePair.Value;
                    }
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

        #region GUI - Start Scraping - Categories
        private async void WebScrapingCategories_Click(object sender, EventArgs e)        
        {
            IsProcessRunning = true;
            try
            {                
                _linksAlbum.Clear();
                _linksTags.Clear();
                listViewLinks.Items.Clear();

                string script = Properties.Settings.Default.WebScraperScript;

                if (!stopRequested) _ = await ScrapingCategory(script, "https://photos.google.com/things", _linksAlbum, _linksTags, _linksLocation, _linksPeople);
                if (!stopRequested) _ = await ScrapingCategory(script, "https://photos.google.com/places", _linksAlbum, _linksTags, _linksLocation, _linksPeople);
                if (!stopRequested) _ = await ScrapingCategory(script, "https://photos.google.com/albums", _linksAlbum, _linksTags, _linksLocation, _linksPeople);
                if (!stopRequested) _ = await ScrapingCategory(script, "https://photos.google.com/people", _linksAlbum, _linksTags, _linksLocation, _linksPeople);


                foreach (KeyValuePair<string, string> keyValuePair in _linksAlbum)
                {
                    if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
                    {
                        ListViewItem listViewItem = listViewLinks.Items.Add(keyValuePair.Value);
                        listViewItem.SubItems.Add("Album");
                        listViewItem.SubItems.Add(keyValuePair.Key);
                    }
                }

                foreach (KeyValuePair<string, string> keyValuePair in _linksTags)
                {
                    if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
                    {
                        ListViewItem listViewItem = listViewLinks.Items.Add(keyValuePair.Value);
                        listViewItem.SubItems.Add("Tag");
                        listViewItem.SubItems.Add(keyValuePair.Key);
                    }
                }

                foreach (KeyValuePair<string, string> keyValuePair in _linksPeople)
                {
                    if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
                    {
                        ListViewItem listViewItem = listViewLinks.Items.Add(keyValuePair.Value);
                        listViewItem.SubItems.Add("People");
                        listViewItem.SubItems.Add(keyValuePair.Key);
                    }
                }

                foreach (KeyValuePair<string, string> keyValuePair in _linksLocation)
                {
                    if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
                    {
                        ListViewItem listViewItem = listViewLinks.Items.Add(keyValuePair.Value);
                        listViewItem.SubItems.Add("Location");
                        listViewItem.SubItems.Add(keyValuePair.Key);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IsProcessRunning = false;
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
                        string name = listViewItem.SubItems[0].Text;
                        string type = listViewItem.SubItems[1].Text;
                        string url = listViewItem.SubItems[2].Text;
                        switch (type)
                        {
                            case "Tag":
                                await ScrapingMediaFiles(script, url, name, null, null, null, webScrapingRetry, _metaDataDictionary, _urlLoadingFailed);
                                break;
                            case "Album":
                                await ScrapingMediaFiles(script, url, null, name, null, null, webScrapingRetry, _metaDataDictionary, _urlLoadingFailed);
                                break;
                            case "People":
                                await ScrapingMediaFiles(script, url, null, null, name, null, webScrapingRetry, _metaDataDictionary, _urlLoadingFailed);
                                break;
                            case "Location":
                                await ScrapingMediaFiles(script, url, null, null, null, name, webScrapingRetry, _metaDataDictionary, _urlLoadingFailed);
                                break;
                        }
                    }
                }

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
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
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

        
        private void UpdatedWebScrapingPackageList(List<DateTime> webScrapingPackages)
        {
            comboBoxWebScapingLoadPackage.Items.Clear();
            webScrapingPackages.Sort();
            foreach (DateTime dateTime in webScrapingPackages) comboBoxWebScapingLoadPackage.Items.Insert(0, dateTime.ToString());
            
            if (webScrapingPackages.Count == 0)
            {
                buttonWebScrapingLoadPackage.Enabled = false;
                comboBoxWebScapingLoadPackage.Enabled = false;
            }
            else
            {
                buttonWebScrapingLoadPackage.Enabled = true;
                comboBoxWebScapingLoadPackage.Enabled = true;
                comboBoxWebScapingLoadPackage.SelectedIndex = 0;
            }
            
        }

        List<DateTime> _webScrapingPackages = new List<DateTime>();
        private void FormWebScraper_Load(object sender, EventArgs e)
        {
            _webScrapingPackages = DatabaseAndCacheMetadataExiftool.ListWebScraperPackages(MetadataBrokerType.WebScraping, webScrapingName);
            UpdatedWebScrapingPackageList(_webScrapingPackages);
        }

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
            fastColoredTextBoxJavaScriptResult.Text += "Media files found: " + mediaFiles.ToString() + "\r\n";
            fastColoredTextBoxJavaScriptResult.Text += "Titles found: " + titles.ToString() + "\r\n";
            fastColoredTextBoxJavaScriptResult.Text += "Album Names found: " + albumNames.ToString() + "\r\n";
            fastColoredTextBoxJavaScriptResult.Text += "Location Names found: " + locationNames.ToString() + "\r\n";
            fastColoredTextBoxJavaScriptResult.Text += "Keyword Tags found: " + keywordTags.ToString() + "\r\n";
            fastColoredTextBoxJavaScriptResult.Text += "Regions found: " + regions.ToString() + "\r\n";
        }

        #region GUI - Load WebScraping Package
        private void buttonWebScrapingLoadPackage_Click(object sender, EventArgs e)
        {
            buttonWebScrapingLoadPackage.Enabled = false;
            buttonWebScrapingSave.Enabled = false;
            if (_webScrapingPackages.Count == comboBoxWebScapingLoadPackage.Items.Count) 
            {
                List<FileEntryBroker> fileEntryBrokers = DatabaseAndCacheMetadataExiftool.ListMediafilesInWebScraperPackages(MetadataBrokerType.WebScraping, webScrapingName, _webScrapingPackages[comboBoxWebScapingLoadPackage.SelectedIndex]);

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
            buttonWebScrapingLoadPackage.Enabled = true;
            buttonWebScrapingSave.Enabled = true;
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

            decimal num = 0;
            if (decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out num))
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
