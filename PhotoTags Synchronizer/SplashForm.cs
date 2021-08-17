using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public partial class SplashForm : KryptonForm
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //Delegate for cross thread call to close
        private delegate void CloseDelegate();
        private delegate void UpdateStatusDelegate();
        private delegate void AddWarningDelegate();
        private delegate void BringFromToFrontDelegate();

        //The type of form to be displayed as the splash screen.
        private static SplashForm splashForm;
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
                    _thread = new Thread(new ThreadStart(SplashForm.ShowForm));
                    _thread.IsBackground = true;
                    _thread.SetApartmentState(ApartmentState.STA);
                    _thread.Start();
                }
            }
            else throw new Exception("thread still alive");
        }

        public SplashForm()
        {
            InitializeComponent();

            this.SuspendLayout();
            this.UseWaitCursor = true;
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
            splashForm = new SplashForm();
            Application.Run(splashForm);
            splashForm = null;            
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
            splashForm.Invoke(new CloseDelegate(SplashForm.CloseIfNoWarning));
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
                    splashForm.Invoke(new UpdateStatusDelegate(SplashForm.UpdateStatusInternal));
                }
            } catch { }
        }

        static private void AddWarningsInternal()
        {
            if (splashForm != null)
            {
                hasWarningOccurred = true;
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
                splashForm.Invoke(new AddWarningDelegate(SplashForm.AddWarningsInternal));
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
                splashForm.Invoke(new BringFromToFrontDelegate(SplashForm.BringFormToFrontInternal));
        }

        private void checkBoxCloseWarning_CheckedChanged(object sender, EventArgs e)
        {
            try { 
                Properties.Settings.Default.CloseWarningWindowsAutomatically = this.checkBoxCloseWarning.Checked;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Can't save settings");
            }
        }

        
    }
}
