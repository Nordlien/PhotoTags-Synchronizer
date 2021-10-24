using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CameraOwners;
using GoogleLocationHistory;
using SqliteDatabase;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public partial class LocationHistoryImportForm : KryptonForm
    {
        public SqliteDatabaseUtilities databaseTools { get; set; }
        public CameraOwnersDatabaseCache databaseAndCahceCameraOwner { get; set; }
        private Stopwatch timer = new Stopwatch();
        private DateTime timerIntervalCheck = DateTime.Now;
        private long _locationsCount = 0;
        private long _filePosition = 0;
        private long _fileLength = 0;

        public LocationHistoryImportForm()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;

            Properties.Settings.Default.Reload();                      
        }

        public void Init()
        {
            //System.Collections.Specialized.StringCollection locationUsers = Properties.Settings.Default.LocationUsers;
            List<string> locationUsers = databaseAndCahceCameraOwner.ReadCameraOwners();
            if (locationUsers != null)
            {
                foreach (string item in locationUsers) comboBoxUserAccount.Items.Add(item);
                comboBoxUserAccount.Text = Properties.Settings.Default.LocationUser;
            }
        }

        private void buttonImportLocationHistory_Click(object sender, EventArgs e)
        {
            if (databaseTools == null) return;

            if (comboBoxUserAccount.Text.Trim() == "")
            {
                KryptonMessageBox.Show("You need to enter a name for the import");
                return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = Properties.Settings.Default.LastGoogleLocationFolder,
                Title = "Browse for Google Location History",
                AddExtension = true,
                Multiselect = false,

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "Google Location History file",
                Filter = "json files (*.json) and KML files (*.kml)|*.json;*.kml",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = false
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                databaseAndCahceCameraOwner.MakeCameraOwnersDirty();
                try
                {
                    Properties.Settings.Default.LastGoogleLocationFolder = Path.GetDirectoryName(openFileDialog1.FileName);
                    Properties.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Can't save settings");
                }

                string jsonFilename = openFileDialog1.FileName;
                string userAccount = comboBoxUserAccount.Text.Trim();

                Properties.Settings.Default.Reload();
                List<string> locationUsers = databaseAndCahceCameraOwner.ReadCameraOwners();

                try
                {
                    Properties.Settings.Default.LocationUser = comboBoxUserAccount.Text;
                    Properties.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Can't save settings");
                }

                this.Enabled = false;

                
                timer.Restart();

                _locationsCount = 0;
                _filePosition = 0;
                _fileLength = 0;
                timerIntervalCheck = DateTime.Now;

                switch (Path.GetExtension(jsonFilename).ToLowerInvariant())
                {
                    case ".json":
                        GoogleLocationHistoryJson googleLocationHistoryJson = new GoogleLocationHistoryJson(databaseTools);
                        googleLocationHistoryJson.LocationFoundParam += GoogleLocationHistoryJson_LocationFoundParam;
                        googleLocationHistoryJson.ReadJsonAndWriteToCache(jsonFilename, userAccount, false);
                        break;
                    case ".kml":
                        GoogleLocationHistoryKML googleLocationHistoryKML = new GoogleLocationHistoryKML(databaseTools);
                        googleLocationHistoryKML.LocationFoundParam += GoogleLocationHistoryKML_LocationFoundParam;
                        googleLocationHistoryKML.ReadJsonAndWriteToCache(jsonFilename, userAccount);
                        break;
                }
                timer.Stop();

                UpdateLoadingStatus(true);
                this.Enabled = true;
                statusStripStatus.Refresh();
                this.Refresh();

                this.DialogResult = DialogResult.OK;
            }
        }

        private void UpdateLoadingStatus(bool forceUpdate)
        {
            double elapseMilliseconds = (DateTime.Now - timerIntervalCheck).TotalMilliseconds;
            if (elapseMilliseconds >= 300)
            {
                //Application.DoEvents();
                timerIntervalCheck = DateTime.Now;
                forceUpdate = true;
            }
            int value;
            if (_fileLength == 0 ) value = 100;
            else value = (int)(100f / _fileLength * _filePosition);

            if (toolStripProgressBarprogressBarLoading.Value != value || forceUpdate || value == 100)
            {
                toolStripProgressBarprogressBarLoading.Maximum = 100;
                toolStripProgressBarprogressBarLoading.Value = value;
                TimeSpan timeTaken = timer.Elapsed;
                toolStripStatusLabelStatus.Text = "Locations found: " + _locationsCount + " Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
                statusStripStatus.Refresh();
                //Application.DoEvents();
            }
        }

        private void GoogleLocationHistoryJson_LocationFoundParam(object sender, long locationsCount, long filePosition, long fileLength)
        {
            _locationsCount = locationsCount; //Only to updated final count
            _filePosition = filePosition;
            _fileLength = fileLength;
            UpdateLoadingStatus(false);
        }
        private void GoogleLocationHistoryKML_LocationFoundParam(object sender, long locationsCount, long filePosition, long fileLength)
        {
            _locationsCount = locationsCount; //Only to updated final count
            _filePosition = filePosition;
            _fileLength = fileLength;
            UpdateLoadingStatus(false);
        }

        private void comboBoxUserAccount_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBoxUserAccount.SelectedIndex > -1 && 
                comboBoxUserAccount.SelectedIndex < comboBoxUserAccount.Items.Count &&
                comboBoxUserAccount.SelectedItem.ToString() == CameraOwnersDatabaseCache.MissingLocationsOwners) comboBoxUserAccount.Text = null;
            if (comboBoxUserAccount.Text == CameraOwnersDatabaseCache.MissingLocationsOwners) comboBoxUserAccount.Text = null;
        }
    }
}
