using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using FastColoredTextBoxNS;


namespace PhotoTagsSynchronizer
{
    public partial class FormWebScraper : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ChromiumWebBrowser browser;
        private string webScraperScript = "";

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
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            
            //Wait for the Page to finish loading
            if (e.IsLoading == false)
            {
                

                //Album: document.querySelectorAll("textarea[aria-label^='Rediger albumnavnet']");
                //  defaultValue: "Spiser lunsj"
                //  innerHTML: "Spiser lunsj"
                //  textContent: "Spiser lunsj"
                //  value: "Spiser lunsj"

                //Title: document.querySelectorAll("textarea[aria-label^='Beskrivelse']");
                //  defaultValue: "Trip to Langesund"
                //  innerHTML: "Trip to Langesund"
                //  outerText: "Trip to Langesund"
                //  placeholder: "Legg til en tittel"
                //  textContent: "Trip to Langesund"
                //  value: "Trip to Langesund"
                //  innerText: "Trip to Langesund"

                //https://photos.google.com/album/

                //People: document.querySelectorAll("a[href^='./search/']:not([aria-label='Favoritter'])");
                //  ariaLabel: "Nordlien"
                //  innerText: "Nordlien"
                //  outerText: "Nordlien"

                //Filename: document.querySelectorAll("div[aria-label^='Filnavn']");
                //  innerHTML: "IMG_20200228_180703.jpg"
                //  innerText: "IMG_20200228_180703.jpg"

                //document.querySelectorAll("a[href^='https://']");
                var script1 = @"Array.from(document.getElementsByTagName('a'), x => ({ innerText : x.innerText, class: x.class, jsname: x.jsname, jslog: x.jslog, jsaction: x.jsaction, href : x.href }));";
                browser.EvaluateScriptAsync(webScraperScript).ContinueWith(x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        List<object> list = (List<object>)response.Result;
                        //Do something here (To interact with the UI you must call BeginInvoke)
                        //UpdateList("<a>", list);
                    }
                });

                var script2 = @"Array.from(document.getElementsByTagName('a')).map(x => ({ innerText: x.innerText, click: x.click}));";
                browser.EvaluateScriptAsync(script2).ContinueWith(x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        List<object> list = (List<object>)response.Result;
                    //Do something here (To interact with the UI you must call BeginInvoke)
                    //UpdateList("<a2>", list);

                }
                });

                var script3 = @"Array.from(document.getElementsByTagName('span')).map(x => ( x.innerText));";
                browser.EvaluateScriptAsync(script3).ContinueWith(x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        List<object> list = (List<object>)response.Result;
                    //Do something here (To interact with the UI you must call BeginInvoke)
                    //UpdateList("<span>", list);

                    }
                });

                var script4 = @"Array.from(document.getElementsByTagName('textarea')).map(x => ( x.innerText, class: x.class, jsname: x.jsname, track: x.track, jsaction: x.jsaction, initial-data-value: x.initial-data-value, aria-label: x.aria-label ));";
                browser.EvaluateScriptAsync(script4).ContinueWith(x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        List<object> list = (List<object>)response.Result;
                        //Do something here (To interact with the UI you must call BeginInvoke)
                        //UpdateList("<textarea>", list);

                    }
                    /*
                    <textarea jsname="YPqjbf" jsaction="change:FDSEXc; focus:Jt1EX; blur:fpfTEe; input:Lg5SV; keydown:Hq2uPe;" jslog="61208; track:click" placeholder="Legg til en beskrivelse" 
                        initial-data-value="Beskrivelse2" 
                        class="ajQY2" 
                        spellcheck="false" 
                        aria-label="Beskrivelse" autocomplete="off"></textarea>
                    <textarea jsname="YPqjbf" jsaction="change:FDSEXc; focus:Jt1EX; blur:fpfTEe; input:Lg5SV; keydown:Hq2uPe;" jslog="61208; 
                        track:click" placeholder="Legg til en beskrivelse" initial-data-value="" 
                        class="ajQY2" spellcheck="false" aria-label="Beskrivelse" autocomplete="off">
                    </textarea> 
                    */
                });

                var script5 = @"Array.from(document.getElementsByTagName('img')).map(x => ( { innerText: x.innerText, aria-label: x.aria-label, class: x.class, jsname: x.jsname, style: x.style, click: x.click, src: x.src, width: x.width, height: x.height} ));";
                browser.EvaluateScriptAsync(script5).ContinueWith(x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        List<object> list = (List<object>)response.Result;
                        //Do something here (To interact with the UI you must call BeginInvoke)
                        //UpdateList("<img>", list);
                        /*
                                                
                        Face image
                        <img width="88" height="88" 
                            src="https://lh3.googleusercontent.com/Vpm-qtq-4eF4MWaUiNMUodH_2_31DWftMNnpm9T9PC8zHJd7CRdSpJh6yYOwka1p8QcUHUk7CK6bAGFkcU568Rc3eSw-pdekWdkm6awkQrahsGvIvIpRFH0jXLGQw8Zj8HItwiDRveUK8q3sgpg45pXTA_Fq_IJn4euaggLG3QbCQgNaR7Li-0egnsA-ZpViYTlB3cEoLfiiY-xv00GNQaqQlg5_S4u0wdzElftFP8Izb5S95NXf79GWTexzN56w5KZpz5bvnwOtN65dldAFdaElB3rghAHXXFZtPL3DI_jpR3IUK-xWcdlpuzqfxx4HcHEiTn_kVwYi5P-DDQPdLx1J_QqCUrAPOuux98AHO5e_crZrkCh3M6dBnYsXGF7RnPn9z6MZn6vw-dnqEms9-nor5IlCxs4rK8oCjOUGEiMxnes74-DrCraRc7Vbp34jNApBWU3W23VLERI0XxC677neiGOADq6H3mQfLQqiHVuVB45Mn1vefnayS_TIaJg-FssnA2imMLNV6EA049Y39Mpue7BYyeQDWR4g9p2NqVzNqT1xNM_FblAfvUVmiS-w3u3zchvZxgnI0qMuOXe1lKEB3Sa42Uyug5G6knatz7Hev0XVGA4AEF-BuYgZEKIegR2pc5wh7hVYN0txo_cPgHs202Mud-FTqheORRFMkO8uwFzvGLnB5KLUKriecHHTNLo4KKIJUMeHwb4cU65DkxX9Mh5RktrbhtoGLe6nlCSj=s110-p-k-no-lf" 
                            data-iml="111343.20500004105">
                        
                        Thumbnail real pic 
                        <img jsname="uLHQEd" class="SzDcob" width="318" height="239" 
                            src="https://lh3.googleusercontent.com/V5GJf4ELx8v13NgnjLqx5IqsFUrwSBRdtNShYQvRZRsNfTo6c6j0T7B_e1uFWfaR3_HGj7WAJLd05VOAige900lR_QkEY2aqXSC-2MuGoxve4552xXcZxH4tAtwVHHVskkvTZ40lsVKgfvvHKlPC8eRmtnMDzMZt95e1FQsbCnvN72XvqoZFUhM-KUcLRAM5JuQIcfsiSRnaca8FS8da4PNzgz6puVPyMjS0X7PxbLvoljqX2AIRfscPNYWkNpO4EUYHcHrZbgc9YryT3hibnrO4dGdKen3KU0I5S_Ue99pobjDtmjaZ9JKJKfweM0oVJBIPgiD4lRNjXfp1rAG5ZR4GYGiKlFso1nH2pDodEF8Dl_9tV5iedK-CSmDBA7P4fYZh3KOj4v4UmRHfd_ZKxWevqltONCCS5nUaxKP96wpWc1ROM8T8z3Oxt_sesjZwDEqJ3jpb_fk_KEyN0tK80MRB8UQk79CwGROmWw2J-6E03ZTFtaDVklUmd8Q8C48cLTyEvHMRAQ00tTnq6JPaF4-EmVg9cu2WFyRX1MkEO4t8Bj3xsEtF3n8bVIVlhjtAku2b6Wqc4lckJ8TBSorUK77D5s11VuaCxfRatI5vPdfkrpcMgHro6HLPdRMsHxOWKlia_qDY0hZJMU9Yc_rsxtX_iLjSId7cQTOlFbaxgzt7LFJ63v_t9dDX1vgl-CBF8fnT3mwjDgJRS_ofdLJE--SdPw=w398-h299-no?authuser=0" 
                            style="transform: translate3d(0px, 0px, 0px) rotate(0deg);" data-iml="110258.22000007611" 
                            aria-label="Foto – Liggende – 29. feb. 2020, 09:25:28"> 
                         */
                    }
                });
            }

        }

        private async void Test(IFrame frame)
        {
            //Get all the span elements and create an array that contains their innerText
            var script3 = @"Array.from(document.getElementsByTagName('span')).map(x => ( x.innerText));";
            CefSharp.JavascriptResponse response3 = await frame.EvaluateScriptAsync(script3);

            //Get all the a tags and create an array that contains a list of objects 
            //Second param is the mapping function
            var script1 = @"Array.from(document.getElementsByTagName('a'), x => ({ innerText : x.innerText, href : x.href }));";
            CefSharp.JavascriptResponse response1 = await frame.EvaluateScriptAsync(script1);

            //List of Links, click represents a function pointer which can be used to execute the link click)
            //In .Net the https://cefsharp.github.io/api/88.2.x/html/T_CefSharp_IJavascriptCallback.htm is used
            //to represent the function.
            var script2 = @"Array.from(document.getElementsByTagName('a')).map(x => ({ innerText: x.innerText, click: x.click}));";
            JavascriptResponse javascriptResponse = await frame.EvaluateScriptAsync(script2);
            
            
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Debug.WriteLine("FrameLoadEnd: " + e.Frame.Name + " " + e.HttpStatusCode + " " + e.Url);

            if (e.Frame.IsValid && !e.Frame.IsDisposed)
            {
                IFrame frame = e.Frame;
                

                //Test(frame);
            }

            /*
            if (e.Frame.IsMain)
            {
                ChromiumWebBrowser chromiumWebBrowser = (ChromiumWebBrowser)sender;
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                chromiumWebBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                {
                    Debug.WriteLine("FrameLoadEnd MainFrame: GetSourceAsync() " + stopwatch.Elapsed.ToString());
                    stopwatch.Restart();
                    UpdatedTextboxHtml(taskHtml.Result);
                    Debug.WriteLine("FrameLoadEnd MainFrame: UpdatedTextboxHtml(taskHtml.Result); " + stopwatch.Elapsed.ToString());
                });
            } else
            {
                ChromiumWebBrowser chromiumWebBrowser = (ChromiumWebBrowser)sender;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();
                chromiumWebBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                {
                    Debug.WriteLine("FrameLoadEnd: GetSourceAsync() " + stopwatch.Elapsed.ToString());
                    stopwatch.Restart();
                    UpdatedTextboxHtml(taskHtml.Result);
                    Debug.WriteLine("FrameLoadEnd: UpdatedTextboxHtml(taskHtml.Result); " + stopwatch.Elapsed.ToString());
                });
            }*/
        }

        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, AddressChangedEventArgs>(Browser_AddressChanged), sender, e);
                return;
            }
            
            textBoxBrowserURL.Text = e.Address;
            /*
            ChromiumWebBrowser chromiumWebBrowser = (ChromiumWebBrowser)sender;
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();           
            chromiumWebBrowser.GetSourceAsync().ContinueWith(taskHtml =>
            {
                Debug.WriteLine("AddressChanged: GetSourceAsync() " + stopwatch.Elapsed.ToString());
                stopwatch.Restart();
                UpdatedTextboxHtml(taskHtml.Result);
                Debug.WriteLine("AddressChanged: UpdatedTextboxHtml(taskHtml.Result); " + stopwatch.Elapsed.ToString());
            });
            */
        }

        private void textBoxBrowserURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; //Handle the Keypress event (suppress the Beep)
                browser.Load(textBoxBrowserURL.Text);
            }
        }

        private void buttonBrowserShowDevTool_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        private void UpdateList(string heading, List<object> list)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string, List<object> >(UpdateList), heading, list);
                return;
            }

            fastColoredTextBoxJavaScriptResult.SuspendLayout();
            
            string testString = heading + "\r\n";
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
                    foreach(object item in (List<object>)tag)
                    {
                        testString += item.ToString() + "; ";
                    }
                    testString += "\r\n";
                }
                else if (tag is string)
                {
                    testString += tag.ToString() + "\r\n";
                    //Console.WriteLine(kvp.Key + ": " + kvp.Value);
                }
            }
            fastColoredTextBoxJavaScriptResult.Text += testString;
            fastColoredTextBoxJavaScriptResult.ResumeLayout();
        }

        private void UpdatedTextboxHtml(string html)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(UpdatedTextboxHtml), html);
                return;
            }

            //fastColoredTextBoxHtml.Text = html;
            /*
            Executing JavaScript Code from WinForms / C# https://www.codeproject.com/Articles/990346/Using-HTML-as-UI-Elements-in-a-WinForms-Applicatio


            foreach (HtmlElement etiqueta in webInterno.Document.GetElementsByTagName("input"))
            {
                //MessageBox.Show("input");
                if (etiqueta.GetAttribute("name").Contains("strCurp"))
                {
                    etiqueta.SetAttribute("value", txtCurp.Text);
                }
            }
            */
        }

        private void buttonRunJavaScript_Click(object sender, EventArgs e)
        {
            object EvaluateJavaScriptResult;
            var frame = browser.GetMainFrame();
            var task = frame.EvaluateScriptAsync(fastColoredTextBoxJavaScript.Text, null);

            task.ContinueWith(taskJavaScriptResonse =>
            {
                fastColoredTextBoxJavaScriptResult.SuspendLayout();
                fastColoredTextBoxJavaScriptResult.Text = "Running script...";
                fastColoredTextBoxJavaScriptResult.ResumeLayout();

                if (!taskJavaScriptResonse.IsFaulted)
                {
                    //taskJavaScriptResonse.
                    var response = taskJavaScriptResonse.Result;

                    EvaluateJavaScriptResult = response.Success ? (response.Result ?? "null") : response.Message;
                    fastColoredTextBoxJavaScriptResult.SuspendLayout();
                    if (EvaluateJavaScriptResult is List<object>)
                        UpdateList("Result:", (List<object>)EvaluateJavaScriptResult);
                    else
                        fastColoredTextBoxJavaScriptResult.Text = EvaluateJavaScriptResult.ToString();
                    fastColoredTextBoxJavaScriptResult.ResumeLayout();

                }
                else { 
                    fastColoredTextBoxJavaScriptResult.Text = "Script failed";
                    fastColoredTextBoxJavaScriptResult.ResumeLayout();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
