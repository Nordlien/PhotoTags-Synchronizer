using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using MetadataLibrary;
using SqliteDatabase;

namespace PhotoTagsSynchronizer
{
    public partial class FormDatabaseCleaner : KryptonForm
    {
        public MetadataDatabaseCache DatabaseAndCacheMetadataExiftool;
        public SqliteDatabaseUtilities databaseUtilitiesSqliteMetadata;
        //private MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery;
        //private MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos;
        //public SqliteDatabaseUtilities DatabaseUtilitiesSqliteMetadata { get; set; }
        private Stopwatch stopWatch = new Stopwatch();
        public static string CorruptFile = "CorruptFile";

        public FormDatabaseCleaner()
        {
            InitializeComponent();
        }

        private void kryptonButtonDatabaseCleanerExiftoolData_Click(object sender, EventArgs e)
        {
            DatabaseAndCacheMetadataExiftool.OnRecordReadToCacheParameter += DatabaseAndCacheMetadataExiftool_OnRecordReadToCacheParameter;
            DatabaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas();
            DatabaseAndCacheMetadataExiftool.OnRecordReadToCacheParameter -= DatabaseAndCacheMetadataExiftool_OnRecordReadToCacheParameter;
            

            List<FileEntryBroker> fileEntryBrokers = DatabaseAndCacheMetadataExiftool.GetAllCacheData();
            
            List<FileEntryBroker> fileEntryBrokersDelete = new List<FileEntryBroker>();
            
            foreach (FileEntryBroker fileEntryBroker in fileEntryBrokers)
            {
                if (fileEntryBroker.Broker == MetadataBrokerType.ExifTool)
                {
                    if (!File.Exists(fileEntryBroker.FileFullPath)) fileEntryBrokersDelete.Add(fileEntryBroker);
                    else
                    {
                        Metadata metadata = DatabaseAndCacheMetadataExiftool.ReadMetadataFromCacheOnly(fileEntryBroker);
                        if (metadata != null && (metadata.FileMimeType == null || metadata.FileMimeType == CorruptFile)) fileEntryBrokersDelete.Add(fileEntryBroker);
                    }
                    UpdateStatus("Needs cleaning: " + fileEntryBrokersDelete.Count);
                }
            }

            
            DatabaseAndCacheMetadataExiftool.OnDeleteRecord += DatabaseAndCacheMetadataExiftool_OnDeleteRecord;
            DatabaseAndCacheMetadataExiftool.DeleteFileEntries(fileEntryBrokersDelete);
            DatabaseAndCacheMetadataExiftool.OnDeleteRecord -= DatabaseAndCacheMetadataExiftool_OnDeleteRecord;
            kryptonLabelStatus.Text = "Done cleaning...";
        }

        private void UpdateStatus(string text)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(UpdateStatus), text);
                return;
            }
            if (!stopWatch.IsRunning) stopWatch.Start();
            if (stopWatch.ElapsedMilliseconds > 300)
            {
                kryptonLabelStatus.Text = text;
                kryptonLabelStatus.Refresh();
                stopWatch.Restart();
            }
        }

        private void DatabaseAndCacheMetadataExiftool_OnDeleteRecord(object sender, DeleteRecordEventArgs e)
        {
            UpdateStatus("Cleaning: " + e.Count);
        }

        
        private void DatabaseAndCacheMetadataExiftool_OnRecordReadToCacheParameter(object sender, ReadToCacheParameterRecordEventArgs e)
        {
            UpdateStatus("Reading: " + e.MetadataCount + " / " + e.KeywordCount + " / " + e.RegionCount);
        }

        private void kryptonButtonCheckDatabase_Click(object sender, EventArgs e)
        {
            //PRAGMA optimize;
            //PRAGMA foreign_key_check;
            //PRAGMA quick_check;
            //"PRAGMA lock_status;";
            UpdateStatus("Started: PRAGMA optimize;");
            string result = databaseUtilitiesSqliteMetadata.PRAGMA_Run("PRAGMA optimize;");
            UpdateStatus("Ended: PRAGMA optimize; " + result);

            UpdateStatus("Started: PRAGMA foreign_key_check;");
            result = databaseUtilitiesSqliteMetadata.PRAGMA_Run("PRAGMA foreign_key_check;");
            UpdateStatus("Ended: PRAGMA foreign_key_check; " + result);

            UpdateStatus("Started: PRAGMA quick_check;");
            result = databaseUtilitiesSqliteMetadata.PRAGMA_Run("PRAGMA quick_check;");
            UpdateStatus("Ended: PRAGMA quick_check; " + result);
        }
    }
}
