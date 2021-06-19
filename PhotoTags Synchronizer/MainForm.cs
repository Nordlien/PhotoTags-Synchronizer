using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Manina.Windows.Forms;
using System.Reflection;
using MetadataLibrary;
using CefSharp.WinForms;
using System.Threading;
using CefSharp;
using SqliteDatabase;
using WindowsLivePhotoGallery;
using GoogleLocationHistory;
using CameraOwners;
using Exiftool;
using Thumbnails;
using MicrosoftPhotos;
using DataGridViewGeneric;
using LocationNames;
using LibVLCSharp.Shared;
using NHttp;
using System.Net;
using System.Diagnostics;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        private ShowWhatColumns showWhatColumns;

        private readonly Size[] thumbnailSizes =
        {
          new Size ( 64,  64),
          new Size ( 96,  96),
          new Size (128, 128),
          new Size (144, 144),
          new Size (192, 192)  
          //Making this to big will eat memory and create app slow due to limit in memory and sqlite
        };
        private Size ThumbnailSaveSize { get; set; } = new Size(192, 192);
        private Size ThumbnailMaxUpsize { get; set; } = new Size(192, 192);
        private RendererItem defaultImageListViewRenderer;

        private readonly ChromiumWebBrowser browser;
        //private FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

        //Databases
        private SqliteDatabaseUtilities databaseUtilitiesSqliteMetadata;

        private GoogleLocationHistoryDatabaseCache databaseGoogleLocationHistory;
        private ThumbnailDatabaseCache databaseAndCacheThumbnail;
        private CameraOwnersDatabaseCache databaseAndCahceCameraOwner;

        //Exif from media file
        private MetadataDatabaseCache databaseAndCacheMetadataExiftool;
        private MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery;
        private MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos;
        
        private LocationNameLookUpCache databaseLocationAddress;

        private ExiftoolReader exiftoolReader;
        private ExiftoolDataDatabase databaseExiftoolData;
        private ExiftoolWarningDatabase databaseExiftoolWarning;

        private MicrosoftPhotosReader databaseMicrosoftPhotos;
        private WindowsLivePhotoGalleryDatabasePipe databaseWindowsLivePhotGallery;

        private FilesCutCopyPasteDrag filesCutCopyPasteDrag;

        //Cache level
        private int  cacheNumberOfPosters = 10;
        private bool cacheAllMetadatas = false;
        private bool cacheAllThumbnails = false;
        private bool cacheAllWebScraperDataSets = false;

        private bool cacheFolderMetadatas = true;
        private bool cacheFolderThumbnails = true;
        private bool cacheFolderWebScraperDataSets = false;

        //Avoid flickering
        private bool isFormLoading = true;                  //Avoid flicker and on change events going in loop
        private bool isSettingDefaultComboxValues = false;  //Avoid multiple reload when value are set, avoid on value change event
        private bool isTabControlToolboxChanging = false;   //Avoid multiple reload when value are set, avoid on value change event
        private FormWindowState _previousWindowsState = FormWindowState.Normal;
        bool isSlideShowRunning = false;
        int slideShowIntervalMs = 0;

        #region Constructor - MainForm()
        public MainForm()
        {

            SplashForm.UpdateStatus("Initialize VLC player...");

            if (!DesignMode) Core.Initialize();

            SplashForm.UpdateStatus("Initialize component...");
            InitializeComponent();

            //Cache config
            cacheNumberOfPosters = (int)Properties.Settings.Default.CacheNumberOfPosters;
            cacheAllMetadatas = Properties.Settings.Default.CacheAllMetadatas;
            cacheAllThumbnails = Properties.Settings.Default.CacheAllThumbnails;
            cacheAllWebScraperDataSets = Properties.Settings.Default.CacheAllWebScraperDataSets;

            cacheFolderMetadatas = Properties.Settings.Default.CacheFolderMetadatas;
            cacheFolderThumbnails = Properties.Settings.Default.CacheFolderThumbnails;
            cacheFolderWebScraperDataSets = Properties.Settings.Default.CacheFolderWebScraperDataSets;
            
            //VLC
            _libVLC = new LibVLC();
            videoView1.MediaPlayer = new MediaPlayer(_libVLC);

            //treeViewFilter = new TreeWithoutDoubleClick();

            imageListView1.ThumbnailSize = thumbnailSizes[Properties.Settings.Default.ThumbmailViewSizeIndex];
            toolStripButtonThumbnailSize1.Text = "Thumbnail size " + thumbnailSizes[4].Width + "x" + thumbnailSizes[4].Height;
            toolStripButtonThumbnailSize1.ToolTipText = toolStripButtonThumbnailSize1.Text;
            toolStripButtonThumbnailSize2.Text = "Thumbnail size " + thumbnailSizes[3].Width + "x" + thumbnailSizes[3].Height;
            toolStripButtonThumbnailSize2.ToolTipText = toolStripButtonThumbnailSize2.Text;
            toolStripButtonThumbnailSize3.Text = "Thumbnail size " + thumbnailSizes[2].Width + "x" + thumbnailSizes[2].Height;
            toolStripButtonThumbnailSize3.ToolTipText = toolStripButtonThumbnailSize3.Text;
            toolStripButtonThumbnailSize4.Text = "Thumbnail size " + thumbnailSizes[1].Width + "x" + thumbnailSizes[1].Height;
            toolStripButtonThumbnailSize4.ToolTipText = toolStripButtonThumbnailSize4.Text;
            toolStripButtonThumbnailSize5.Text = "Thumbnail size " + thumbnailSizes[0].Width + "x" + thumbnailSizes[0].Height;
            toolStripButtonThumbnailSize5.ToolTipText = toolStripButtonThumbnailSize5.Text;



            SplashForm.UpdateStatus("Initialize load layout...");
            GlobalData.dataGridViewHandlerTags = new DataGridViewHandler(dataGridViewTagsAndKeywords, "Tags", "Metadata/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords);
            GlobalData.dataGridViewHandlerMap = new DataGridViewHandler(dataGridViewMap, "Location", "Location/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeMap);
            GlobalData.dataGridViewHandlerPeople = new DataGridViewHandler(dataGridViewPeople, "People", "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizePeoples);
            GlobalData.dataGridViewHandlerDates = new DataGridViewHandler(dataGridViewDate, "Dates", "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeDates);
            GlobalData.dataGridViewHandlerExiftoolTags = new DataGridViewHandler(dataGridViewExifTool, "Exiftool", "File/Tag Description", (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool);
            GlobalData.dataGridViewHandlerExiftoolWarning = new DataGridViewHandler(dataGridViewExifToolWarning, "MetadataWarning", "File and version/Tag region and command", (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings);
            GlobalData.dataGridViewHandlerProperties = new DataGridViewHandler(dataGridViewProperties, "Properties", "File/Properties", (DataGridViewSize)Properties.Settings.Default.CellSizeProperties);
            GlobalData.dataGridViewHandlerRename = new DataGridViewHandler(dataGridViewRename, "Rename", "Filename/Values", ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize));
            GlobalData.dataGridViewHandlerConvertAndMerge = new DataGridViewHandler(dataGridViewConvertAndMerge, "Convert and Merge", "Full path of media file", ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize));

            SplashForm.UpdateStatus("Populate renderer dropdown...");
            // Populate renderer dropdown
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            int i = 0;
            foreach (Type t in assembly.GetTypes())
            {
                if (t.BaseType == typeof(Manina.Windows.Forms.ImageListView.ImageListViewRenderer))
                {
                    renderertoolStripComboBox.Items.Add(new RendererItem(t));
                    if (t.Name == "RendererDefault")
                    {
                        renderertoolStripComboBox.SelectedIndex = i;
                        defaultImageListViewRenderer = (RendererItem)renderertoolStripComboBox.Items[i];
                    }
                    i++;
                }
            }

            SplashForm.UpdateStatus("Initialize database: metadata cache...");
            databaseUtilitiesSqliteMetadata = new SqliteDatabaseUtilities(DatabaseType.SqliteMetadataDatabase, 10000, 5000);

            databaseGoogleLocationHistory = new GoogleLocationHistoryDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseAndCacheMetadataExiftool = new MetadataDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseAndCacheMetadataExiftool.OnRecordReadToCache += DatabaseAndCacheMetadataExiftool_OnRecordReadToCache;
            databaseAndCacheMetadataExiftool.OnDeleteRecord += DatabaseAndCacheMetadataExiftool_OnDeleteRecord;

            databaseAndCacheThumbnail = new ThumbnailDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseAndCacheThumbnail.UpsizeThumbnailSize = ThumbnailMaxUpsize;

            databaseExiftoolData = new ExiftoolDataDatabase(databaseUtilitiesSqliteMetadata);
            databaseExiftoolWarning = new ExiftoolWarningDatabase(databaseUtilitiesSqliteMetadata);

            databaseAndCahceCameraOwner = new CameraOwnersDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseLocationAddress = new LocationNameLookUpCache(databaseUtilitiesSqliteMetadata, Properties.Settings.Default.ApplicationPreferredLanguages);

            
            //databaseUtilitiesSqliteWindowsLivePhotoGallery = new SqliteDatabaseUtilities(DatabaseType.SqliteWindowsLivePhotoGallaryCache);
            //databaseAndCacheMetadataWindowsLivePhotoGallery = new MetadataDatabaseCache(databaseUtilitiesSqliteWindowsLivePhotoGallery);
            databaseAndCacheMetadataWindowsLivePhotoGallery = new MetadataDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseAndCacheMetadataWindowsLivePhotoGallery.OnRecordReadToCache += DatabaseAndCacheMetadataExiftool_OnRecordReadToCache;
            databaseAndCacheMetadataWindowsLivePhotoGallery.OnDeleteRecord += DatabaseAndCacheMetadataExiftool_OnDeleteRecord;

            //databaseUtilitiesSqliteMicrosoftPhotos = new SqliteDatabaseUtilities(DatabaseType.SqliteMicrosoftPhotosCache);
            //databaseAndCacheMetadataMicrosoftPhotos = new MetadataDatabaseCache(databaseUtilitiesSqliteMicrosoftPhotos);
            databaseAndCacheMetadataMicrosoftPhotos = new MetadataDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseAndCacheMetadataMicrosoftPhotos.OnRecordReadToCache += DatabaseAndCacheMetadataExiftool_OnRecordReadToCache;
            databaseAndCacheMetadataMicrosoftPhotos.OnDeleteRecord += DatabaseAndCacheMetadataExiftool_OnDeleteRecord;

            exiftoolReader = new ExiftoolReader(databaseAndCacheMetadataExiftool, databaseExiftoolData, databaseExiftoolWarning);
            exiftoolReader.MetadataGroupPrioityRead();
            exiftoolReader.afterNewMediaFoundEvent += ExiftoolReader_afterNewMediaFoundEvent;

            try
            {
                Thread threadCache = new Thread(() =>
                {
                    if (cacheAllThumbnails) databaseAndCacheThumbnail.ReadToCacheFolder(null);                    
                    if (cacheAllMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas();
                    if (cacheAllWebScraperDataSets) databaseAndCacheMetadataExiftool.ReadToCacheWebScarpingAllDataSets();
                });
                threadCache.Start();
            }
            catch { }

            filesCutCopyPasteDrag = new FilesCutCopyPasteDrag(databaseAndCacheMetadataExiftool, databaseAndCacheMetadataWindowsLivePhotoGallery,
                databaseAndCacheMetadataMicrosoftPhotos, databaseAndCacheThumbnail, databaseExiftoolData, databaseExiftoolWarning);

            SplashForm.UpdateStatus("Initialize database: Microsoft Photos...");
            try
            {
                databaseMicrosoftPhotos = new MicrosoftPhotosReader();
            }
            catch (Exception e)
            {

                SplashForm.AddWarning("Windows photo warning:\r\n" + e.Message + "\r\n");
                databaseMicrosoftPhotos = null;
            }

            SplashForm.UpdateStatus("Initialize database: Windows Live Photo Gallery...");
            try
            {
                databaseWindowsLivePhotGallery = new WindowsLivePhotoGalleryDatabasePipe();
                databaseWindowsLivePhotGallery.ConnectDatabase(databaseAndCacheMetadataWindowsLivePhotoGallery);
            }
            catch (Exception e)
            {
                SplashForm.AddWarning("Windows Live Photo Gallery warning:\r\n" + e.Message + "\r\n");
                databaseWindowsLivePhotGallery = null;
            }

            SplashForm.UpdateStatus("Initialize ChromiumWebBrowser...");
            //Cef Browser
            browser = new ChromiumWebBrowser("https://www.openstreetmap.org/")
            {
                Dock = DockStyle.Fill,
            };
            browser.BrowserSettings.Javascript = CefState.Enabled;
            //browser.BrowserSettings.WebSecurity = CefState.Enabled;
            browser.BrowserSettings.WebGl = CefState.Enabled;
            browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Disabled;
            browser.BrowserSettings.Plugins = CefState.Enabled;
            this.panelBrowser.Controls.Add(this.browser);


            //toolStripContainer.ContentPanel.Controls.Add(browser);
            //browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            //browser.LoadingStateChanged += OnLoadingStateChanged;
            //browser.ConsoleMessage += OnBrowserConsoleMessage;
            //browser.StatusMessage += OnBrowserStatusMessage;
            //browser.TitleChanged += OnBrowserTitleChanged;
            //browser.AddressChanged += new System.EventHandler(this.OnBrowserAddressChanged);
            browser.AddressChanged += this.OnBrowserAddressChanged;

            isSettingDefaultComboxValues = true;
            //Map
            comboBoxGoogleTimeZoneShift.SelectedIndex = Properties.Settings.Default.ComboBoxGoogleTimeZoneShift;    //0 time shift = 12
            comboBoxGoogleLocationInterval.SelectedIndex = Properties.Settings.Default.ComboBoxGoogleLocationInterval;    //30 minutes Index 2
            comboBoxMapZoomLevel.SelectedIndex = Properties.Settings.Default.ComboBoxMapZoomLevel;     //13 map zoom level 14
            //Rename
            textBoxRenameNewName.Text = Properties.Settings.Default.RenameVariable;
            isSettingDefaultComboxValues = false;
            //Application
            ThumbnailSaveSize = Properties.Settings.Default.ApplicationThumbnail;
            RegionThumbnailHandler.FaceThumbnailSize = Properties.Settings.Default.ApplicationRegionThumbnail;

            isFormLoading = true;

            this.Size = Properties.Settings.Default.MainFormSize;
            this.Location = Properties.Settings.Default.MainFormLocation;

            if (Properties.Settings.Default.IsMainFormMaximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }

            splitContainerFolder.SplitterDistance = Properties.Settings.Default.SplitContainerFolder;
            splitContainerImages.SplitterDistance = Properties.Settings.Default.SplitContainerImages;
            splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
            renderertoolStripComboBox.SelectedIndex = Properties.Settings.Default.RenderertoolStripComboBox;
            toolStripButtonHistortyColumns.Checked = Properties.Settings.Default.ShowHistortyColumns;
            toolStripButtonErrorColumns.Checked = Properties.Settings.Default.ShowErrorColumns;

            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);

            timerShowErrorMessage.Enabled = true;

            PopulateExiftoolToolStripMenuItems();
            PopulateExiftoolWarningToolStripMenuItems();
            PopulatePeopleToolStripMenuItems();

            // Add event handlers for file system watcher.
            /*
            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Changed += new FileSystemEventHandler(FileSystemWatcherOnChanged);
            fileSystemWatcher.Created += new FileSystemEventHandler(FileSystemWatcherOnCreated);
            fileSystemWatcher.Deleted += new FileSystemEventHandler(FileSystemWatcherOnDeleted);
            fileSystemWatcher.Renamed += new RenamedEventHandler(FileSystemWatcherOnRenamed);
            */

            _ThreadHttp = new Thread(() =>
            {
                using (nHttpServer = new HttpServer())
                {
                    nHttpServer.WriteBufferSize = 1024 * 1024 * 10;
                    nHttpServer.RequestReceived -= NHttpServer_RequestReceived;
                    nHttpServer.RequestReceived += NHttpServer_RequestReceived;
                    nHttpServer.StateChanged -= NHttpServer_StateChanged;
                    nHttpServer.StateChanged += NHttpServer_StateChanged;
                    nHttpServer.UnhandledException += NHttpServer_UnhandledException;
                    nHttpServer.EndPoint = new IPEndPoint(IPAddress.Parse(GetLocalIp()), GetOpenPort());
                    Logger.Info("nHTTP server started: " + DateTime.Now.ToString() + " ip: " + nHttpServer.EndPoint.ToString());
                    nHttpServer.Start();
                    WaitApplicationClosing = new AutoResetEvent(false);
                    WaitApplicationClosing.WaitOne();
                    Application.DoEvents();
                }
            });
            _ThreadHttp.Start();
        }

        

        #endregion

        #region Resize and restore windows size when reopen application        
        private void tabControlToolbox_Selecting(object sender, TabControlCancelEventArgs e)
        {
            isTabControlToolboxChanging = true;
        }

        private void tabControlToolbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            isTabControlToolboxChanging = false;
            PopulateDataGridViewForSelectedItemsThread(imageListView1.SelectedItems);
        }

        private void splitContainerMap_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (isTabControlToolboxChanging) return;
            if (_previousWindowsState == FormWindowState.Minimized) return;

            if (!isFormLoading)
            {
                Properties.Settings.Default.SplitContainerMap = splitContainerMap.SplitterDistance;
                Properties.Settings.Default.Save(); 
            }
        }

        private void splitContainerImages_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!isFormLoading)
            {
                Properties.Settings.Default.SplitContainerImages = splitContainerImages.SplitterDistance;
                Properties.Settings.Default.Save();
            }
        }

        private void splitContainerFolder_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (!isFormLoading)
            {
                Properties.Settings.Default.SplitContainerFolder = splitContainerFolder.SplitterDistance;
                Properties.Settings.Default.Save();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (isFormLoading) return;

            _previousWindowsState = this.WindowState;
        }



        private void MainForm_Activated(object sender, EventArgs e)
        {
            SplashForm.BringToFrontSplashForm();
            DataGridViewHandler.BringToFrontFindAndReplace();
        }
        #endregion

        #region MainForm_FormClosing
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            exiftoolReader.MetadataGroupPrioityWrite(); //Updated json config file if new tags found

            if (commonQueueSaveMetadataUpdatedByUser.Count > 0)
            {
                if (MessageBox.Show(
                    "There are " + commonQueueSaveMetadataUpdatedByUser.Count + " unsaved media files. Are you sure you will close application?",
                    "Changed will get lost.", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            browser.Dispose();

            GlobalData.IsApplicationClosing = true;
            GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = true;
            MetadataDatabaseCache.StopCaching = true;
            ThumbnailDatabaseCache.StopCaching = true;

            WaitApplicationClosing.Set();

            SplashForm.ShowSplashScreen("PhotoTags Synchronizer - Closing...", 6, false, false);

            SplashForm.UpdateStatus("Saving layout...");

            //---------------------------------------------------------
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.IsMainFormMaximized = false;
                Properties.Settings.Default.MainFormSize = this.Size;
                Properties.Settings.Default.MainFormLocation = this.Location;

                Properties.Settings.Default.SplitContainerImages = splitContainerImages.SplitterDistance;
                Properties.Settings.Default.SplitContainerFolder = splitContainerFolder.SplitterDistance;
                //Properties.Settings.Default.SplitContainerMap = splitContainerMap.SplitterDistance; //Don't read this (it's wrong size when openened)
            }
            else
            {
                Properties.Settings.Default.IsMainFormMaximized = true;
                Properties.Settings.Default.MainFormSize = this.RestoreBounds.Size;
                Properties.Settings.Default.MainFormLocation = this.RestoreBounds.Location;
            }
            Properties.Settings.Default.Save();
            //---------------------------------------------------------

            SplashForm.UpdateStatus("Closing Exiftool read...");
            if (exiftoolReader != null) exiftoolReader.Close();

            //---------------------------------------------------------

            ImageListViewClearAll(imageListView1);

            imageListView1.Dispose();
            imageListView1.StoppBackgroundThreads();

            //---------------------------------------------------------
            Application.DoEvents();
            Thread.Sleep(200);

            int waitForProcessEndRetray = 30;
            
            SplashForm.UpdateStatus("Stopping ImageView background threads...");
            waitForProcessEndRetray = 30;
            while (!imageListView1.IsBackgroundThreadsStopped() && waitForProcessEndRetray-- > 0)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            SplashForm.UpdateStatus("Stopping fetch metadata background threads...");
            waitForProcessEndRetray = 30;
            while (IsAnyThreadRunning() && waitForProcessEndRetray-- > 0)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            SplashForm.UpdateStatus("Disconnecting databases...");
            databaseUtilitiesSqliteMetadata.DatabaseClose(); //Close database after all background threads stopped

            SplashForm.UpdateStatus("Disposing...");
            imageListView1.Dispose();
            SplashForm.CloseForm();

        }
        #endregion

        #region MainForm_Load / Shown
        private void MainForm_Load(object sender, EventArgs e)
        {

            try
            {
                SplashForm.UpdateStatus("Initialize folder tree...");
                GlobalData.IsPopulatingFolderTree = true;

                this.folderTreeViewFolder.InitFolderTreeView();

                string folder = Properties.Settings.Default.LastFolder;
                if (Directory.Exists(folder))
                    folderTreeViewFolder.DrillToFolder(folder);
                else
                    folderTreeViewFolder.SelectedNode = folderTreeViewFolder.Nodes[0];
                GlobalData.IsPopulatingFolderTree = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                SplashForm.UpdateStatus("Populate search filters...");
                PopulateDatabaseFilter();

                PopulateSelectGroupToolStripMenuItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            


            SplashForm.CloseForm();

            Properties.Settings.Default.Reload();
            RegionStructure.SetAcceptRegionMissmatchProcent((float)Properties.Settings.Default.RegionMissmatchProcent);

            isFormLoading = true;
            this.Size = Properties.Settings.Default.MainFormSize;
            this.Location = Properties.Settings.Default.MainFormLocation;
            this.Activate();

            imageListView1.Focus();

        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            isFormLoading = false;

            PopulateImageListView_FromFolderSelected(false, true);
            FilesSelected();
        }





        #endregion


        
    }


}


    
