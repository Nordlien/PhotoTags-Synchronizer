using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ChromiumWebBrowser browser;
        private string webScraperScript = "";
        private AutoResetEvent autoResetEventWaitPageLoaded = null;

        #region Init
        public FormWebScraper()
        {
            InitializeComponent();

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

            webScraperScript = Properties.Settings.Default.WebScraperScript;
            fastColoredTextBoxJavaScript.Text = webScraperScript;
        }
        #endregion 

        #region Browser - LoadingStateChanged
        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Debug.WriteLine("Browser_LoadingStateChanged" + e.IsLoading);
            //Wait for the Page to finish loading
            if (e.IsLoading == false )
            {
                /*
                bool failed;
                int retry = 3;
                do
                {
                    failed = false;
                    Application.DoEvents();
                    if (checkBoxClickNext.Checked) Thread.Sleep(500);

                    //Invoke(new Action(async delegate ()
                    //{
                    //waiting code
                    //int nretry = 100;
                    //while (nretry > 0){
                    //...
                    //Thread.Sleep(100);
                    //    }
                    //}

                    browser.EvaluateScriptAsync(webScraperScript).ContinueWith(x =>
                    {
                        var response = x.Result;

                        if (response.Success && response.Result != null)
                        {
                            List<object> list = (List<object>)response.Result;
                            if (checkBoxRecord.Checked) AddScrapingResult(list);

                            if (list.Count < 2) failed = true;       
                        }
                        else
                        {
                            failed = true;
                            //Debug
                        }
                    });
                    
                } while (failed && retry-- > 0);


                if (checkBoxClickNext.Checked)
                {
                    //Application.DoEvents();
                    

                    //KeyEvent for Right-arrow
                    KeyEvent k2 = new KeyEvent();
                    k2.WindowsKeyCode = 0x27;
                    k2.FocusOnEditableField = true;
                    k2.IsSystemKey = false;
                    k2.Type = KeyEventType.KeyDown;

                    browser.GetBrowser().GetHost().SendKeyEvent(k2);

                }
                */

                if (autoResetEventWaitPageLoaded != null) autoResetEventWaitPageLoaded.Set();
            }

        }
        #endregion

        #region Browser - FrameLoadEnd
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Debug.WriteLine("FrameLoadEnd: " + e.Frame.Name + " " + e.HttpStatusCode + " " + e.Url);

            if (!e.Frame.IsMain && e.Frame.IsValid && !e.Frame.IsDisposed)
            {
                IFrame frame = e.Frame;
                /*
                browser.EvaluateScriptAsync(webScraperScript).ContinueWith(x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        List<object> list = (List<object>)response.Result;
                        if (checkBoxRecord.Checked) AddScrapingResult(list);
                    }
                });
                */


            }

            /*
            if (e.Frame.IsMain)
            {
            } else
            {
                chromiumWebBrowser.GetSourceAsync().ContinueWith(taskHtml =>
            }*/
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

        #region Scraping - Add Result
        Dictionary<string, string> linksAlbum = new Dictionary<string, string>();
        Dictionary<string, string> linksTags = new Dictionary<string, string>();

        public class ScrapingResult
        {
            public string Url { get; set; } = null;
            public string Title { get; set; } = null;
            public string Description { get; set; } = null;

            public bool PictureInfoScreenHidden = true;
            public List<string> PhotoLinks { get; set; } = new List<string>();
            public string Album { get; set; }  = null;
            public List<string> AlbumOthers { get; set; } = new List<string>();
            public string MediaFile { get; set; } = null;
            public string LocationName { get; set; } = null;
            public string Tag { get; set; } = null;
            public List<string> Tags { get; set; } = new List<string>();
            public List<string> Peoples { get; set; } = new List<string>();

            //public Dictionary<string, string> LinksAlbum = new Dictionary<string, string>();
            //public Dictionary<string, string> LinksTags = new Dictionary<string, string>();

        }

        private ScrapingResult AddScrapingResult(List<object> list)
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
                            if (!scrapingResult.PhotoLinks.Contains(param)) scrapingResult.PhotoLinks.Add(param);
                            break;
                        case "album":
                            scrapingResult.Album = tagList[1].ToString();
//textBoxActiveAlbum.Text = value;
                            break;
                        case "album other":
                            if (!scrapingResult.AlbumOthers.Contains(value)) scrapingResult.AlbumOthers.Add(value);
                            break;
                        case "album link":
                            if (!linksAlbum.ContainsKey(param))
                            {
linksAlbum.Add(param, value);
                                ListViewItem itemAlbum = listViewLinks.Items.Add(value);
                                itemAlbum.SubItems.Add(param);
                            }
                            break;
                        case "tag link":
                            if (!linksTags.ContainsKey(param))
                            {
linksTags.Add(param, value);
                                ListViewItem itemTag = listViewLinks.Items.Add(value);
                                itemTag.SubItems.Add(param);
                            }                    
                            break;
                        case "mediafile":
                            scrapingResult.MediaFile = value;
                            break;
                        case "description":
                            scrapingResult.Description = value;
                            break;
                        case "tag":
                            if (!scrapingResult.Tags.Contains(value)) scrapingResult.Tags.Add(value);
//textBoxActiveTag.Text = value;
                            break;
                        case "people":
                            if (!scrapingResult.Peoples.Contains(value)) scrapingResult.Peoples.Add(value);
                            break;
                        /*default:
                            throw new NotImplementedException();*/
                    }
                    //foreach (object item in (List<object>)tag) testString += item.ToString() + "; ";
                }
            }

            if (scrapingResult.Album == null && linksAlbum.ContainsKey(textBoxBrowserURL.Text))
            {
                scrapingResult.Album = linksAlbum[textBoxBrowserURL.Text];
                textBoxActiveAlbum.Text = scrapingResult.Album;

                scrapingResult.Tag = null;
                textBoxActiveTag.Text = "";
            }

            if (scrapingResult.Tag == null && linksTags.ContainsKey(textBoxBrowserURL.Text))
            {
                scrapingResult.Tag = linksTags[textBoxBrowserURL.Text];
                textBoxActiveTag.Text = scrapingResult.Tag;

                scrapingResult.Album = null;
                textBoxActiveAlbum.Text = scrapingResult.Album;
            }

            return scrapingResult;
        }
        #endregion

        #region GUI - show result javaScript
        private void ShowResult(List<object> list)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<List<object> >(ShowResult), list);
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
                    foreach(object item in (List<object>)tag) testString += item.ToString() + "; ";                    
                    testString += "\r\n";
                }
                else if (tag is string) testString += tag.ToString() + "\r\n";
                
            }
            fastColoredTextBoxJavaScriptResult.Text = testString;
            fastColoredTextBoxJavaScriptResult.ResumeLayout();
        }
        #endregion

        #region RunScript async
        private void RunScript(string script)
        {
            object EvaluateJavaScriptResult;
            var frame = browser.GetMainFrame();
            var task = frame.EvaluateScriptAsync(script, null);

            task.ContinueWith(taskJavaScriptResonse =>
            {
                fastColoredTextBoxJavaScriptResult.SuspendLayout();
                fastColoredTextBoxJavaScriptResult.Text = "Running script...";
                fastColoredTextBoxJavaScriptResult.ResumeLayout();

                if (!taskJavaScriptResonse.IsFaulted)
                {                    
                    var response = taskJavaScriptResonse.Result;

                    EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
                    fastColoredTextBoxJavaScriptResult.SuspendLayout();
                    if (EvaluateJavaScriptResult is List<object>)
                        ShowResult((List<object>)EvaluateJavaScriptResult);
                    else
                        fastColoredTextBoxJavaScriptResult.Text = EvaluateJavaScriptResult.ToString();

                    fastColoredTextBoxJavaScriptResult.ResumeLayout();
                }
                else
                {
                    fastColoredTextBoxJavaScriptResult.Text = "Script failed";
                    fastColoredTextBoxJavaScriptResult.ResumeLayout();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion RunScript async

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
                    Console.WriteLine(e.InnerException.Message);
                }
            }
            return result;
        }
        #endregion 

        private async void buttonRunJavaScript_Click(object sender, EventArgs e)
        {
            fastColoredTextBoxJavaScriptResult.SuspendLayout();
            fastColoredTextBoxJavaScriptResult.Text = "Running script...";
            fastColoredTextBoxJavaScriptResult.ResumeLayout();

            object result = await EvaluateScript(fastColoredTextBoxJavaScript.Text, null, TimeSpan.FromMilliseconds(3000));

            if (result is List<object>) ShowResult((List<object>)result);
            else fastColoredTextBoxJavaScriptResult.Text = result.ToString();
            
            fastColoredTextBoxJavaScriptResult.ResumeLayout();
            //RunScript(fastColoredTextBoxJavaScript.Text); 
        }


        private async Task<object> ScrapingMediaFiles(string url)
        {
            Debug.WriteLine(url);
            browser.Load(url);
            autoResetEventWaitPageLoaded.WaitOne(10000);
            Application.DoEvents();
            Thread.Sleep(1000);
            ScrapingResult scrapingResult = await Scraping();



            if (scrapingResult.PhotoLinks.Count>0)
            {
                browser.Load(scrapingResult.PhotoLinks[0]);
                Application.DoEvents();
                autoResetEventWaitPageLoaded.WaitOne(10000);
                Thread.Sleep(300);
                scrapingResult = await Scraping();
                if (scrapingResult.PictureInfoScreenHidden)
                {
                    SendKeyBrowser(Keys.I);
                    Thread.Sleep(100);
                }
                do
                {
                    scrapingResult = await Scraping();
                    if (scrapingResult.PictureInfoScreenHidden)
                {
                    SendKeyBrowser(Keys.I);
                    Thread.Sleep(100);
                }

                
                    SendKeyBrowser(Keys.Right);
                } while (autoResetEventWaitPageLoaded.WaitOne(10000));
            }

            return null;
        }

        private async void buttonWebScrapingStart_Click(object sender, EventArgs e)
        {
            //List<string> listOfSelectedCategoryUrls = new List<string>();

            //ListViewItem itemTag = listViewLinks.Items.Add(value);
            //itemTag.SubItems.Add(param);
            foreach (ListViewItem listViewItem in listViewLinks.Items)
            {
                if (listViewItem.Checked)
                {
                    string url = listViewItem.SubItems[1].Text;
                    //if (!listOfSelectedCategoryUrls.Contains(url)) listOfSelectedCategoryUrls.Add(url);
                    await ScrapingMediaFiles(url);
                }
            }

        }

        private async Task<ScrapingResult> Scraping()
        {
            ScrapingResult scrapingResult = null;
            Debug.WriteLine("Scraping");
            object result = await EvaluateScript(fastColoredTextBoxJavaScript.Text, null, TimeSpan.FromMilliseconds(3000));
            if (result is List<object>)
            {
                scrapingResult = AddScrapingResult((List<object>)result);
                ShowResult((List<object>)result);
            }
            return scrapingResult;
        }
        
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
        
        private async void WebScrapingCategories_Click(object sender, EventArgs e)        
        {
            autoResetEventWaitPageLoaded = new AutoResetEvent(false);

            Debug.WriteLine("https://photos.google.com/things");
            browser.Load("https://photos.google.com/things");
            autoResetEventWaitPageLoaded.WaitOne(10000);

            int scrollPagnDownCount = 15;
            object test;
            for (int i = 0; i < scrollPagnDownCount; i++)
            {
                test = await Scraping();
                SendKeyBrowser(Keys.PageDown);
                Thread.Sleep(100);
            }

            Debug.WriteLine("https://photos.google.com/places");
            browser.Load("https://photos.google.com/places");
            autoResetEventWaitPageLoaded.WaitOne(10000);
            for (int i = 0; i < scrollPagnDownCount; i++)
            {
                test = await Scraping();
                SendKeyBrowser(Keys.PageDown);
                Thread.Sleep(100);
            }

            Debug.WriteLine("https://photos.google.com/albums");
            browser.Load("https://photos.google.com/albums");
            autoResetEventWaitPageLoaded.WaitOne(10000);
            for (int i = 0; i < scrollPagnDownCount; i++)
            {
                test = await Scraping();
                SendKeyBrowser(Keys.PageDown);
                Thread.Sleep(100);
            }

            Debug.WriteLine("https://photos.google.com/people");
            browser.Load("https://photos.google.com/people");
            autoResetEventWaitPageLoaded.WaitOne(10000);
            for (int i = 0; i < scrollPagnDownCount; i++)
            {
                test = await Scraping();
                SendKeyBrowser(Keys.PageDown);
                Thread.Sleep(100);
            }

        }
    }
}
