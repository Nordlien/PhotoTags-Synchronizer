using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using PhotoTagsSynchronizer.Properties;

namespace PhotoTagsSynchronizer
{
    public partial class FormSplash : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //Delegate for cross thread call to close
        private delegate void CloseDelegate();
        private delegate void UpdateStatusDelegate();
        private delegate void AddWarningDelegate();
        private delegate void BringFromToFrontDelegate();

        //The type of form to be displayed as the splash screen.
        private static FormSplash splashForm;
        private static string _status;
        private static int _taskWhere;
        private static int _numTasks;
        private static bool hasWarningOccurred = false;
        private static string _warningMessage = "";
        private static bool _isWindowsClosed = false;
        private static string _title = "PhotoTags Synchronizer...";
        private static bool _closeWarningChecked = false;
        private static bool _showWarningCheckBox = false;
        private static Thread _thread = null;

        static public void ShowSplashScreen(string title, int numberOfTasks, bool closeWarningAutomaticlly, bool showCloseWarningCheckBox)
        {
            _thread = null;
            _waitForUserCloseSplash = null;

            _taskWhere = 0;
            _numTasks = numberOfTasks;
            _closeWarningChecked = closeWarningAutomaticlly;
            _showWarningCheckBox = showCloseWarningCheckBox;
            _title = title;

            hasWarningOccurred = false;
            _warningMessage = "";
            _isWindowsClosed = false;

            if (_thread == null || !_thread.IsAlive)
            {
                if (splashForm == null)
                {
                    _thread = new Thread(new ThreadStart(FormSplash.ShowForm));
                    _thread.IsBackground = true;
                    _thread.SetApartmentState(ApartmentState.STA);
                    _thread.Start();
                }
            }
            else throw new Exception("thread still alive");
        }

        public FormSplash()
        {
            InitializeComponent();

            this.SuspendLayout();
            this.UseWaitCursor = true;

            this.tabControlMessages.Appearance = TabAppearance.FlatButtons;
            this.tabControlMessages.ItemSize = new Size(0, 1);
            this.tabControlMessages.SizeMode = TabSizeMode.Fixed;

            this.checkBoxCloseWarning.Checked = _closeWarningChecked;
            this.Text = _title; 
            this.labelStatus.Text = _status;
            this.progressBar.Maximum = _numTasks;
            this.progressBar.Value = _taskWhere;
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        static private void ShowForm()
        {
            try
            {
                splashForm = new FormSplash();
                Application.Run(splashForm);
                
            } catch
            {

            } finally
            {
                splashForm = null;
            }
        }

        static private bool NeedShowWarningAndWaitUser()
        {
            return (hasWarningOccurred && !splashForm.checkBoxCloseWarning.Checked);
        }

        static private void CloseIfNoWarning()
        {
            if (NeedShowWarningAndWaitUser())
            {
                splashForm.Text = _title;

                splashForm.labelStatus.Visible = false;
                splashForm.progressBar.Visible = false;
                splashForm.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                splashForm.ControlBox = true;
                splashForm.MaximizeBox = false;
                splashForm.MinimizeBox = false;
                splashForm.HelpButton = false;
                splashForm.UseWaitCursor = false;
                splashForm.Refresh();
                //Application.DoEvents();
            }
            else
            {                
                _isWindowsClosed = false;
                splashForm.Close();                
            }
        }

        static public void CloseForm()
        {
            if (splashForm == null || !splashForm.IsHandleCreated) return; 
            splashForm.Invoke(new CloseDelegate(FormSplash.CloseIfNoWarning));
        }

        private void SplashForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isWindowsClosed = true;
            if (_waitForUserCloseSplash != null)
            {
                _waitForUserCloseSplash.Set();
            }
        }

        static private AutoResetEvent _waitForUserCloseSplash = null;

        static public void CloseFormAndWait()
        {
            if (_waitForUserCloseSplash != null) throw new Exception("Should be null");
            while (splashForm == null) Task.Delay(3).Wait();
            while (!splashForm.IsHandleCreated) Task.Delay(3).Wait();

            if (splashForm == null) throw new Exception("Can't close when not open");
            if (splashForm == null && !splashForm.IsHandleCreated) new Exception("Missing handler");

            if (NeedShowWarningAndWaitUser()) _waitForUserCloseSplash = new AutoResetEvent(false);

            CloseForm();

            if (_waitForUserCloseSplash != null) _waitForUserCloseSplash.WaitOne();
            _waitForUserCloseSplash = null;

            splashForm = null;
        }


        static private void UpdateStatusInternal ()
        {
            if (splashForm != null)
            {
                splashForm.labelStatus.Text = _status;
                splashForm.progressBar.Value = _taskWhere;
                splashForm.Refresh();
            }
        }

       
        static public void UpdateStatus(string status)
        {
            
            Logger.Debug(status);

            _status = status;
            _taskWhere++;
            try
            {
                if (splashForm != null && splashForm.IsHandleCreated)
                {
                    splashForm.Invoke(new UpdateStatusDelegate(FormSplash.UpdateStatusInternal));
                    splashForm.ShowRandomsPage();
                }
            } catch { }
        }

        public enum MessagePage
        {
            Warning,
            KeepYourTags,
            InternetAccess,
            DelayReading,
            ImportLocation
        }
        
        private void ShowMessagesPage(MessagePage page)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<MessagePage>(ShowMessagesPage), page);
                return;
            }
            switch (page)
            {
                case MessagePage.Warning:
                    tabControlMessages.SelectedTab = tabPageWarning;
                    break;
                case MessagePage.KeepYourTags:
                    tabControlMessages.SelectedTab = tabPageKeepYourTags;
                    break;
                case MessagePage.InternetAccess:
                    tabControlMessages.SelectedTab = tabPageInternetAccess;
                    break;
                case MessagePage.DelayReading:
                    tabControlMessages.SelectedTab = tabPageDelayReading;
                    break;
                case MessagePage.ImportLocation:
                    tabControlMessages.SelectedTab = tabPageImportLocation;
                    break;
            }
        }

        private Stopwatch stopwatch = null;
        private void ShowRandomsPage()
        {            
            if (!hasWarningOccurred && (stopwatch == null || stopwatch.ElapsedMilliseconds > 5000))
            {
                if (stopwatch == null)
                {
                    stopwatch = new Stopwatch();
                    stopwatch.Start();
                }

                stopwatch.Restart();

                byte splashScreenNumber = Properties.Settings.Default.SlashScreenTipNumber;
                switch (splashScreenNumber)
                {
                    case 1:
                        ShowMessagesPage(MessagePage.KeepYourTags);
                        Properties.Settings.Default.SlashScreenTipNumber = 2;
                        break;
                    case 2:
                        ShowMessagesPage(MessagePage.InternetAccess);
                        Properties.Settings.Default.SlashScreenTipNumber = 3;
                        break;
                    case 3:
                        ShowMessagesPage(MessagePage.DelayReading);
                        Properties.Settings.Default.SlashScreenTipNumber = 4;
                        break;
                    case 4:
                        ShowMessagesPage(MessagePage.ImportLocation);
                        Properties.Settings.Default.SlashScreenTipNumber = 1;
                        break;
                }
            }
        }

        static private void AddWarningsInternal()
        {
            if (splashForm != null)
            {
                hasWarningOccurred = true;
                splashForm.ShowMessagesPage(MessagePage.Warning);                
                splashForm.textBoxWarning.Text = splashForm.textBoxWarning.Text + "\r\n" + _warningMessage ;
                splashForm.textBoxWarning.Visible = true;
                splashForm.textBoxWarning.Select(0, 0);
                splashForm.labelWarnings.Visible = true;
                if (_showWarningCheckBox) splashForm.checkBoxCloseWarning.Visible = true;
            }
        }

        static public void AddWarning(string warningMessage)
        {
            _warningMessage = warningMessage;

            if (splashForm != null && splashForm.IsHandleCreated)
            {
                splashForm.Invoke(new AddWarningDelegate(FormSplash.AddWarningsInternal));
            }
        }

        static private void BringFormToFrontInternal()
        {
            if (splashForm != null)
            {
                if (_isWindowsClosed) return;
                //Bring to front workaround
                //splashForm.WindowState = FormWindowState.Minimized;
                //splashForm.WindowState = FormWindowState.Normal;
                splashForm.Activate();
                splashForm.BringToFront();
            }
        }

        static public void BringToFrontSplashForm()
        {
            if (_isWindowsClosed) return;
            if (splashForm!=null)
                splashForm.Invoke(new BringFromToFrontDelegate(FormSplash.BringFormToFrontInternal));
        }

        private void checkBoxCloseWarning_CheckedChanged(object sender, EventArgs e)
        {
            try { 
                Properties.Settings.Default.CloseWarningWindowsAutomatically = this.checkBoxCloseWarning.Checked;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Can't save settings...", MessageBoxButtons.OK, MessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        private void linkLabelHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ApplicationAssociations.ApplicationActivation.OpenUserGuide();
        }
    }
}
