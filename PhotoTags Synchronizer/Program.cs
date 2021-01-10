using System;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace PhotoTagsSynchronizer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SplashForm.ShowSplashScreen("PhotoTags Synchronizer - Loading...", 14, Properties.Settings.Default.CloseWarningWindowsAutomatically, true);
            SplashForm.UpdateStatus("Initialize DLL files..."); //1 

            if (Environment.Is64BitProcess)
            {
                File.Copy(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "x64\\sqlite3.dll"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlite3.dll"), true);
            }
            else 
            {
                File.Copy(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "x86\\sqlite3.dll"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlite3.dll"), true);
            }

            

            //Monitor parent process exit and close subprocesses if parent process exits first
            //This will at some point in the future becomes the default
            SplashForm.UpdateStatus("Initialize broswer..."); //2 

            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

            //For Windows 7 and above, best to include relevant app.manifest entries as well
            Cef.EnableHighDPISupport();

            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoTagsSynchronizer\\BrowserCache"),
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:68.0) Gecko/20100101 Firefox/68.0"
            };
        
            SplashForm.UpdateStatus("Initialize broswer.."); //3 
            //Example of setting a command line argument
            //Enables WebRTC
            settings.CefCommandLineArgs.Add("enable-media-stream", "1");

            SplashForm.UpdateStatus("Initialize broswer..."); //4 
            
            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            SplashForm.UpdateStatus("Initialize broswer...."); //5 
            MainForm mainForm = new MainForm(); //this takes ages
            
            Application.Run(mainForm);

        }
    }
}
