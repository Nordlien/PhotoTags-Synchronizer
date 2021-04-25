using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;


namespace PhotoTagsSynchronizer
{
    static class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        static void Main()
        {
            /*
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(Form1_UIThreadException);
            // Set the unhandled exception mode to force all Windows Forms errors to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            // Add the event handler for handling non-UI thread exceptions to the event.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SplashForm.ShowSplashScreen("PhotoTags Synchronizer - Loading...", 15, Properties.Settings.Default.CloseWarningWindowsAutomatically, true);
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
        
            SplashForm.UpdateStatus("Initialize browser.."); //3 
            //Example of setting a command line argument
            //Enables WebRTC
            settings.CefCommandLineArgs.Add("enable-media-stream", "1");

            SplashForm.UpdateStatus("Initialize broswer..."); //4 
            
            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            SplashForm.UpdateStatus("Initialize broswer...."); //5 
            
            mainForm = new MainForm(); //this takes ages
            Application.Run(mainForm);
            
        }

        public static MainForm mainForm = null;

        // Handle the UI exceptions by showing a dialog box, and asking the user whether or not they wish to abort execution.
        private static void Form1_UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            if (mainForm != null && mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action<object, ThreadExceptionEventArgs>(Form1_UIThreadException), sender, t);
                return;
            }

            DialogResult result = DialogResult.Cancel;
            try
            {
                result = ShowThreadExceptionDialog("Windows Forms Error", t.Exception);
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal Windows Forms Error", "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // Exits the program when the user clicks Abort.
            try
            {
                if (result == DialogResult.Abort) Application.Exit();
            }
            catch { }
        }

        // Handle the UI exceptions by showing a dialog box, and asking the user whether or not they wish to abort execution.
        // NOTE: This exception cannot be kept from terminating the application - it can only log the event, and inform the user about it.
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            if (mainForm != null && mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action<object, UnhandledExceptionEventArgs>(CurrentDomain_UnhandledException), sender, e);
                return;
            }

            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "An application error occurred. Please contact the adminstrator with the following information:\n\n";

                // Since we can't prevent the app from terminating, log this to the event log.
                Logger.Error(errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);

            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show("Fatal Non-UI Error",
                        "Fatal Non-UI Error. Could not write the error to the event log. Reason: "
                        + exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }
        }

        // Creates the error message and displays it.
        private static DialogResult ShowThreadExceptionDialog(string title, Exception e)
        {
            string errorMsg = "An application error occurred. Please contact the adminstrator with the following information:\n\n";
            errorMsg = errorMsg + e.Message + "\n\nStack Trace:\n" + e.StackTrace;
            try
            {
                //Logger.Error(errorMsg);
            }
            catch { }
            return MessageBox.Show(errorMsg, title, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
        }
    }
}
