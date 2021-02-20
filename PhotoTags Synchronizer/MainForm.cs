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
using System.Collections.Generic;
using LibVLCSharp.Shared;
using System.Linq;
using System.Diagnostics;
using NHttp;
using ImageAndMovieFileExtentions;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace PhotoTagsSynchronizer
{
    
    public partial class MainForm : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
        private Size ThumbnailSaveSize {get; set;} = new Size(192, 192);
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

        

        //VLC
        private LibVLC _libVLC;
        private MediaPlayer vlcMediaPlayerVideoView = null;
        private MediaPlayer vlcMediaPlayerChromecast = null;
        private RendererDiscoverer vlcRendererDiscoverer;
        
        private List<LibVLCSharp.Shared.RendererItem> vlcRendererItems = new List<LibVLCSharp.Shared.RendererItem>();

        //Avoid flickering
        private bool isFormLoading = true;                  //Avoid flicker and on change events going in loop
        private bool isSettingDefaultComboxValues = false;  //Avoid multiple reload when value are set, avoid on value change event
        private bool isTabControlToolboxChanging = false;   //Avoid multiple reload when value are set, avoid on value change event
        private FormWindowState _previousWindowsState = FormWindowState.Normal;

        #region Constructor - MainForm()
        public MainForm()
        {

            SplashForm.UpdateStatus("Initialize VLC player...");  

            if (!DesignMode) Core.Initialize();

            SplashForm.UpdateStatus("Initialize component..."); 
            InitializeComponent();

            //VLC
            _libVLC = new LibVLC();
            vlcMediaPlayerVideoView = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = vlcMediaPlayerVideoView;

            RendererDescription vlcRendererDescription;
            vlcRendererDescription = _libVLC.RendererList.FirstOrDefault(r => r.Name.Equals("microdns_renderer"));          
            vlcRendererDiscoverer = new RendererDiscoverer(_libVLC, vlcRendererDescription.Name);
            vlcRendererDiscoverer.ItemAdded += _rendererDiscoverer_ItemAdded;
            vlcRendererDiscoverer.ItemDeleted += _rendererDiscoverer_ItemDeleted;            
            vlcRendererDiscoverer.Start();

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
            GlobalData.dataGridViewHandlerRename = new DataGridViewHandler(dataGridViewRename, "Rename", "Filename/Values", ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameSize));

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
            databaseAndCacheThumbnail = new ThumbnailDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseAndCacheThumbnail.UpsizeThumbnailSize = ThumbnailMaxUpsize;

            databaseExiftoolData = new ExiftoolDataDatabase(databaseUtilitiesSqliteMetadata);
            databaseExiftoolWarning = new ExiftoolWarningDatabase(databaseUtilitiesSqliteMetadata);

            databaseAndCahceCameraOwner = new CameraOwnersDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseLocationAddress = new LocationNameLookUpCache(databaseUtilitiesSqliteMetadata, Properties.Settings.Default.ApplicationPreferredLanguages);

            //databaseUtilitiesSqliteWindowsLivePhotoGallery = new SqliteDatabaseUtilities(DatabaseType.SqliteWindowsLivePhotoGallaryCache);
            //databaseAndCacheMetadataWindowsLivePhotoGallery = new MetadataDatabaseCache(databaseUtilitiesSqliteWindowsLivePhotoGallery);
            databaseAndCacheMetadataWindowsLivePhotoGallery = new MetadataDatabaseCache(databaseUtilitiesSqliteMetadata);

            //databaseUtilitiesSqliteMicrosoftPhotos = new SqliteDatabaseUtilities(DatabaseType.SqliteMicrosoftPhotosCache);
            //databaseAndCacheMetadataMicrosoftPhotos = new MetadataDatabaseCache(databaseUtilitiesSqliteMicrosoftPhotos);
            databaseAndCacheMetadataMicrosoftPhotos = new MetadataDatabaseCache(databaseUtilitiesSqliteMetadata);

            exiftoolReader = new ExiftoolReader(databaseAndCacheMetadataExiftool, databaseExiftoolData, databaseExiftoolWarning);
            exiftoolReader.MetadataGroupPrioityRead();
            exiftoolReader.afterNewMediaFoundEvent += ExiftoolReader_afterNewMediaFoundEvent;

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
                databaseWindowsLivePhotGallery.Connect(databaseAndCacheMetadataWindowsLivePhotoGallery);
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
            browser.BrowserSettings.WebSecurity = CefState.Enabled;
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
            if (WindowState == FormWindowState.Minimized) return;
            if (WindowState == FormWindowState.Maximized) return;
            if (_previousWindowsState == FormWindowState.Minimized) return;

            if (!isFormLoading)
            {
                Properties.Settings.Default.SplitContainerMap = splitContainerMap.SplitterDistance;
                Properties.Settings.Default.Save();
            }
        }

        private void splitContainerImages_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) return;
            if (WindowState == FormWindowState.Maximized) return;
            if (!isFormLoading)
            {
                Properties.Settings.Default.SplitContainerImages = splitContainerImages.SplitterDistance;
                Properties.Settings.Default.Save();
            }
        }

        private void splitContainerFolder_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) return;
            if (WindowState == FormWindowState.Maximized) return;
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
            GlobalData.IsApplicationClosing = true;

            SplashForm.ShowSplashScreen("PhotoTags Synchronizer - Closing...", 7, false, false);

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

            SplashForm.UpdateStatus("Stopping ImageView caching proccess...");
            int colsedownRetaies = 30;
            while ((GlobalData.retrieveThumbnailCount > 0 || GlobalData.retrieveImageCount > 0) && colsedownRetaies-- > 0)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            SplashForm.UpdateStatus("Stopping ImageView background threads...");
            colsedownRetaies = 30;
            while (!imageListView1.IsBackgroundThreadsStopped() && colsedownRetaies-- > 0)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            SplashForm.UpdateStatus("Stopping fetch metadata background threads...");
            colsedownRetaies = 30;
            while (IsAnyThreadRunning() && colsedownRetaies-- > 0)
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
            SplashForm.UpdateStatus("Initialize folder tree...");
            GlobalData.IsPopulatingFolderTree = true;

            this.folderTreeViewFolder.InitFolderTreeView();

            Properties.Settings.Default.Reload();
            string folder = Properties.Settings.Default.LastFolder;
            if (Directory.Exists(folder))
                folderTreeViewFolder.DrillToFolder(folder);
            else
                folderTreeViewFolder.SelectedNode = folderTreeViewFolder.Nodes[0];            
            GlobalData.IsPopulatingFolderTree = false;

            SplashForm.UpdateStatus("Populate filters...");
            //PopulateDatabaseFilter();

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

            PopulateImageListViewBasedOnSelectedFolderAndOrFilter(false, true);
            //List<FileEntry> imageListViewFileEntryItems = ImageListViewAggregateWithFilesFromFolder(folderTreeViewFolder.GetSelectedNodePath(), false);

            FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems);

            //PopulateTreeViewFolderFilterThread(imageListViewFileEntryItems);
        }






        #endregion



        private List<string> previewItems = new List<string>();
        private int previewMediaindex = 0;
        private bool canPlayAndPause = false;

        private void mediaPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nHttpServer == null)
            {
                nHttpServer = new HttpServer();
                nHttpServer.WriteBufferSize = 1024 * 1024 * 10;
                nHttpServer.RequestReceived += NHttpServer_RequestReceived;
                nHttpServer.StateChanged += NHttpServer_StateChanged;
                nHttpServer.UnhandledException += NHttpServer_UnhandledException;
                nHttpServer.EndPoint = new IPEndPoint(IPAddress.Parse(GetLocalIp()), GetOpenPort());
                Console.WriteLine("nHTTP server started: " + nHttpServer.EndPoint.ToString());
                nHttpServer.Start();
            }

            toolStripDropDownButtonChromecastList.DropDownItems.Clear();
            foreach (LibVLCSharp.Shared.RendererItem rendererItem in vlcRendererItems)
            {
                ToolStripMenuItem toolStripDropDownItem = new ToolStripMenuItem();
                toolStripDropDownItem.Click += ToolStripDropDownItemPreviewChromecast_Click;
                toolStripDropDownItem.Text = rendererItem.Name;
                toolStripDropDownItem.Tag = rendererItem;
                toolStripDropDownButtonChromecastList.DropDownItems.Add(toolStripDropDownItem);
            }
            
            videoView1.MediaPlayer.EnableKeyInput = true;
            videoView1.MediaPlayer.EnableHardwareDecoding = true;
            videoView1.MediaPlayer.EnableKeyInput = true;

            vlcMediaPlayerVideoView.Backward += VlcMediaPlayerVideoView_Backward;
            vlcMediaPlayerVideoView.Forward += VlcMediaPlayerVideoView_Forward; ;
            vlcMediaPlayerVideoView.Buffering += VlcMediaPlayerVideoView_Buffering;
            vlcMediaPlayerVideoView.EncounteredError += VlcMediaPlayerVideoView_EncounteredError;
            
            vlcMediaPlayerVideoView.Muted += VlcMediaPlayerVideoView_Muted;
            vlcMediaPlayerVideoView.Opening += VlcMediaPlayerVideoView_Opening;

            vlcMediaPlayerVideoView.Paused += VlcMediaPlayerVideoView_Paused;
            vlcMediaPlayerVideoView.PositionChanged += VlcMediaPlayerVideoView_PositionChanged;

            vlcMediaPlayerVideoView.EndReached += VlcMediaPlayerVideoView_EndReached;
            vlcMediaPlayerVideoView.Playing += VlcMediaPlayerVideoView_Playing;

            vlcMediaPlayerVideoView.Stopped += VlcMediaPlayerVideoView_Stopped;

            vlcMediaPlayerVideoView.TimeChanged += VlcMediaPlayerVideoView_TimeChanged;
            vlcMediaPlayerVideoView.Unmuted += VlcMediaPlayerVideoView_Unmuted;
            vlcMediaPlayerVideoView.VolumeChanged += VlcMediaPlayerVideoView_VolumeChanged;


            panelMediaPreview.Dock = DockStyle.Fill;
            panelMediaPreview.Visible = true;

            imageBoxPreview.Visible = false;
            imageBoxPreview.Dock = DockStyle.Fill;

            videoView1.Visible = false;
            videoView1.Dock = DockStyle.Fill;

            previewItems.Clear();
            toolStripDropDownButtonMediaList.DropDownItems.Clear();
            for (int selectedItemIndex = 0; selectedItemIndex < imageListView1.SelectedItems.Count; selectedItemIndex++)
            {
                previewItems.Add(imageListView1.SelectedItems[selectedItemIndex].FileFullPath);

                ToolStripMenuItem toolStripDropDownItem = new ToolStripMenuItem();
                toolStripDropDownItem.Click += ToolStripDropDownItemPreviewMedia_Click;
                toolStripDropDownItem.Text = imageListView1.SelectedItems[selectedItemIndex].FileFullPath;
                toolStripDropDownItem.Tag = selectedItemIndex;
                toolStripDropDownButtonMediaList.DropDownItems.Add(toolStripDropDownItem);
            }
            if (previewItems.Count > 0)
            {
                previewMediaindex = 0;
                ShowPreviewItem(previewItems[previewMediaindex]);
            }          
        }

        #region nHttpServer - 
        HttpServer nHttpServer = null;
        public string GetLocalIp()
        {
            string localIP = null;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }

        private int GetOpenPort()
        {
            int PortStartIndex = 51000 + (new Random()).Next(0, 1000);
            int PortEndIndex = PortStartIndex + 1000;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            for (int port = PortStartIndex; port < PortEndIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            return unusedPort;
        }

        #region nHttpServer - UnhandledException
        private void NHttpServer_UnhandledException(object sender, HttpExceptionEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, HttpExceptionEventArgs>(NHttpServer_UnhandledException), sender, e);
                return;
            }
            toolStripLabelMediaPreviewStatus.Text = "nHTTP server unhandled exception...";
            Logger.Error(e.Request.ToString());
        }
        #endregion 

        #region nHttpServer - StateChanged
        private void NHttpServer_StateChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(NHttpServer_StateChanged), sender, e);
                return;
            }
            if (nHttpServer != null) toolStripLabelMediaPreviewStatus.Text = "nHTTP server new state: " + nHttpServer.State.ToString();
            Console.WriteLine("nHTTP server new state: " + nHttpServer.State.ToString());
        }
        #endregion 

        #region nHttpServer - RequestReceived
        private void NHttpServer_RequestReceived(object sender, HttpRequestEventArgs e)
        {
            /*
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, HttpRequestEventArgs>(NHttpServer_RequestReceived), sender, e);
                return;
            }*/

            //Process.Start(String.Format("http://{0}", server.EndPoint) + "/chromecast?index=2&loadmedia=" + System.Web.HttpUtility.UrlEncode(@"C:\data\path 2\path2\file name.jpg"));

            byte[] mediaByteArray = null;
            string mediaFullFilename = e.Request.Params["loadmedia"];
            string indexString = e.Request.Params["index"];
            if (int.TryParse(indexString, out int indexMediaFile))
            {
                if (indexMediaFile > -1  && indexMediaFile < previewItems.Count) mediaFullFilename = previewItems[indexMediaFile];
            }

            //if (e.Request.Path.ToLower() == "/favicon.ico") bilde = File.ReadAllBytes("favicon.png");
            if (e.Request.Path.ToLower() == "/chromecast" && mediaFullFilename != null)
            {                
                if (ImageAndMovieFileExtentionsUtility.IsImageFormat(mediaFullFilename))
                {
                    mediaByteArray = ImageAndMovieFileExtentionsUtility.LoasImageAsJpeg(mediaFullFilename);
                    e.Response.ContentType = "image/jpeg";
                }
                else if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(mediaFullFilename))
                {
                    mediaByteArray = File.ReadAllBytes(mediaFullFilename);
                    e.Response.ContentType = "video/mp4";
                }
            }

            if (mediaByteArray != null) e.Response.OutputStream.Write(mediaByteArray, 0, mediaByteArray.Length);
            else e.Response.Status = "404 Not Found";

        }
        #endregion

        #endregion 

        #region VlcMediaplayer 

        #region VlcMediaplayer - Pause
        private void VlcMediaPlayerVideoView_Paused(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_Paused), sender, e);
                return;
            }

            toolStripButtonMediaPreviewPlay.Enabled = true;
            toolStripButtonMediaPreviewPause.Enabled = false;
            toolStripButtonMediaPreviewFastBackward.Enabled = false;
            toolStripButtonMediaPreviewFastForward.Enabled = false;
            toolStripTraceBarItemMediaPreviewTimer.Enabled = true;
        }
        #endregion

        #region VlcMediaplayer - Opening
        private void VlcMediaPlayerVideoView_Opening(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_Opening), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player opening...";
            toolStripButtonMediaPreviewPlay.Enabled = false;
            toolStripButtonMediaPreviewPause.Enabled = false;
            toolStripButtonMediaPreviewFastBackward.Enabled = false;
            toolStripButtonMediaPreviewFastForward.Enabled = false;
            toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
        }
        #endregion

        #region VlcMediaplayer - EndReached
        private void VlcMediaPlayerVideoView_EndReached(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_EndReached), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player end reached...";
            toolStripButtonMediaPreviewPlay.Enabled = true;
            toolStripButtonMediaPreviewPause.Enabled = false;
            toolStripButtonMediaPreviewFastBackward.Enabled = false;
            toolStripButtonMediaPreviewFastForward.Enabled = false;
            toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
        }
        #endregion

        #region VlcMediaplayer - Error
        private void VlcMediaPlayerVideoView_EncounteredError(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_EncounteredError), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player error encountered...";
            toolStripButtonMediaPreviewPlay.Enabled = false;
            toolStripButtonMediaPreviewPause.Enabled = false;
            toolStripButtonMediaPreviewFastBackward.Enabled = false;
            toolStripButtonMediaPreviewFastForward.Enabled = false;
            toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
        }
        #endregion

        #region VlcMediaplayer - Buffering
        private void VlcMediaPlayerVideoView_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, MediaPlayerBufferingEventArgs>(VlcMediaPlayerVideoView_Buffering), sender, e);
                return;
            }

            //toolStripButtonMediaPreviewPlay.Enabled = false;
            //toolStripButtonMediaPreviewPause.Enabled = false;
            //toolStripButtonMediaPreviewFastBackward.Enabled = false;
            //toolStripButtonMediaPreviewFastForward.Enabled = false;

        }
        #endregion

        #region VlcMediaplayer - Stopped
        private void VlcMediaPlayerVideoView_Stopped(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_Stopped), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player stopped...";
            toolStripButtonMediaPreviewPlay.Enabled = true;
            toolStripButtonMediaPreviewPause.Enabled = false;
            toolStripButtonMediaPreviewFastBackward.Enabled = false;
            toolStripButtonMediaPreviewFastForward.Enabled = false;
            toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
        }
        #endregion

        #region VlcMediaplayer - Playing
        private void VlcMediaPlayerVideoView_Playing(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_Playing), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player playing...";
            toolStripButtonMediaPreviewPlay.Enabled = false;
            toolStripButtonMediaPreviewPause.Enabled = true;
            toolStripButtonMediaPreviewFastBackward.Enabled = true;
            toolStripButtonMediaPreviewFastForward.Enabled = true;
            toolStripTraceBarItemMediaPreviewTimer.Enabled = true;
        }
        #endregion

        #region VlcMediaplayer - Backward
        private void VlcMediaPlayerVideoView_Backward(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_Backward), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player backward...";
        }
        #endregion

        #region VlcMediaplayer - Forward
        private void VlcMediaPlayerVideoView_Forward(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_Forward), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player forward...";
        }
        #endregion

        #region VlcMediaplayer - VolumeChanged
        private void VlcMediaPlayerVideoView_VolumeChanged(object sender, MediaPlayerVolumeChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, MediaPlayerVolumeChangedEventArgs>(VlcMediaPlayerVideoView_VolumeChanged), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player volume changed to " + ((int)e.Volume*100).ToString() + "%...";
        }
        #endregion

        #region VlcMediaplayer - Muted
        private void VlcMediaPlayerVideoView_Muted(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_Muted), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player muted...";
        }
        #endregion

        #region VlcMediaplayer - Unmuted
        private void VlcMediaPlayerVideoView_Unmuted(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(VlcMediaPlayerVideoView_Unmuted), sender, e);
                return;
            }

            toolStripLabelMediaPreviewStatus.Text = "VLC player unmuted...";
        }
        #endregion

        #region VlcMediaplayer - ConvertMsToHuman
        private string ConvertMsToHuman(long ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
        }
        #endregion

        #region VlcMediaplayer - TimeChanged
        Stopwatch stopwachMediaTimeChanged = new Stopwatch();
        private void VlcMediaPlayerVideoView_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, MediaPlayerTimeChangedEventArgs>(VlcMediaPlayerVideoView_TimeChanged), sender, e);
                return;
            }
            

            if (stopwachMediaTimeChanged.IsRunning && stopwachMediaTimeChanged.ElapsedMilliseconds < 300) return;
            stopwachMediaTimeChanged.Restart();

            if (vlcMediaPlayerVideoView.Length == -1)
            {                
                toolStripLabelMediaPreviewTimer.Text = "Timer: No video";
            }
            else
            {
                toolStripLabelMediaPreviewTimer.Text = ConvertMsToHuman (e.Time) + "/" + ConvertMsToHuman(vlcMediaPlayerVideoView.Length);
            }
        }
        #endregion

        #region VlcMediaplayer - PositionChanged
        Stopwatch stopwachMediaPositionChanged = new Stopwatch();
        private void VlcMediaPlayerVideoView_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, MediaPlayerPositionChangedEventArgs>(VlcMediaPlayerVideoView_PositionChanged), sender, e);
                return;
            }
            
            if (stopwachMediaPositionChanged.IsRunning && stopwachMediaPositionChanged.ElapsedMilliseconds < 300) return;
            stopwachMediaPositionChanged.Restart();

            if (vlcMediaPlayerVideoView.Length == -1)
            {
                toolStripTraceBarItemMediaPreviewTimerUpdating = true;
                toolStripTraceBarItemMediaPreviewTimer.TrackBar.Value = 0;
                toolStripTraceBarItemMediaPreviewTimer.TrackBar.Maximum = 100;
                toolStripTraceBarItemMediaPreviewTimerUpdating = false;
            }
            else
            {
                toolStripTraceBarItemMediaPreviewTimerUpdating = true;
                toolStripTraceBarItemMediaPreviewTimer.TrackBar.Maximum = 100;
                toolStripTraceBarItemMediaPreviewTimer.Value = (int)(e.Position * 100);
                toolStripTraceBarItemMediaPreviewTimerUpdating = false;
            }
        }
        #endregion

        #region VlcMediaplayer - SeekPosition ValueChanged 
        private bool toolStripTraceBarItemMediaPreviewTimerUpdating = false;
        private void toolStripTraceBarItemSeekPosition_ValueChanged(object sender, EventArgs e)
        {
            if (toolStripTraceBarItemMediaPreviewTimerUpdating) return;
            vlcMediaPlayerVideoView.Pause(); //pause 
            vlcMediaPlayerVideoView.Position = (float)toolStripTraceBarItemMediaPreviewTimer.TrackBar.Value / 100;
            vlcMediaPlayerVideoView.Play(); //resume
        }
        #endregion 

        #region VlcMediaplayer - FastBackward_Click
        private void toolStripButtonMediaPreviewFastBackward_Click(object sender, EventArgs e)
        {
            vlcMediaPlayerVideoView.Time -= 10000;
        }
        #endregion

        #region VlcMediaplayer - FastForward_Click
        private void toolStripButtonMediaPreviewFastForward_Click(object sender, EventArgs e)
        {
            vlcMediaPlayerVideoView.Time += 10000;
        }
        #endregion

        #endregion

        #region Vlc Chromecast

        #region Vlc Chromecast - Chromecast Device Discoverer - Deleted
        private void _rendererDiscoverer_ItemDeleted(object sender, RendererDiscovererItemDeletedEventArgs e)
        {
            if (vlcRendererItems.Contains(e.RendererItem)) vlcRendererItems.Remove(e.RendererItem);
        }
        #endregion

        #region Vlc Chromecast - Chromecast Device Discoverer - Added
        private void _rendererDiscoverer_ItemAdded(object sender, RendererDiscovererItemAddedEventArgs e)
        {
            //Console.WriteLine($"New item discovered: {e.RendererItem.Name} of type {e.RendererItem.Type}");
            //if (e.RendererItem.CanRenderVideo) Console.WriteLine("Can render video");
            //if (e.RendererItem.CanRenderAudio) Console.WriteLine("Can render audio");
            //Console.WriteLine("Chromecast icon: " + (e.RendererItem.IconUri == null ? "" : e.RendererItem.IconUri));
            // add newly found renderer item to local collection
            if (e.RendererItem.CanRenderVideo && !vlcRendererItems.Contains(e.RendererItem)) vlcRendererItems.Add(e.RendererItem);
        }
        #endregion 

        #region Vlc Chromecast - Chromecast_Click
        LibVLCSharp.Shared.RendererItem selectedVlcRendererItem = null;
        private void ToolStripDropDownItemPreviewChromecast_Click(object sender, EventArgs e)
        {
            // abort casting if no renderer items were found
            if (!vlcRendererItems.Any())
            {
                MessageBox.Show("No renderer items found. Abort casting...");
                return;
            }

            foreach (ToolStripMenuItem toolStripDropDownItem in toolStripDropDownButtonChromecastList.DropDownItems)
            {
                toolStripDropDownItem.Checked = false;
            }

            ToolStripMenuItem clickedToolStripMenuItem = (ToolStripMenuItem)sender;
            clickedToolStripMenuItem.Checked = true;
            LibVLCSharp.Shared.RendererItem rendererItem = (LibVLCSharp.Shared.RendererItem)clickedToolStripMenuItem.Tag;
            selectedVlcRendererItem = rendererItem;

            // create the mediaplayer
            vlcMediaPlayerChromecast = new MediaPlayer(_libVLC);
            // set the previously discovered renderer item (chromecast) on the mediaplayer if you set it to null, it will start to render normally (i.e. locally) again
            vlcMediaPlayerChromecast.SetRenderer(rendererItem);

        }
        #endregion 

        #region Vlc Chromecast - Device Selected
        private void toolStripDropDownButtonChromecastList_Click(object sender, EventArgs e)
        {
            if (vlcMediaPlayerChromecast == null)
            {
                toolStripDropDownButtonChromecastList.ShowDropDown();
            }
            else
            {
                //Test(previewItems, @"c:\Users\nordl\OneDrive\Pictures JTNs OneDrive\TestTags\output.mp4");

                if (vlcMediaPlayerChromecast == null) return;

                var media = new LibVLCSharp.Shared.Media(_libVLC, previewItems[previewMediaindex], FromType.FromPath);

                // create the mediaplayer
                vlcMediaPlayerChromecast = new MediaPlayer(_libVLC);
                // set the previously discovered renderer item (chromecast) on the mediaplayer if you set it to null, it will start to render normally (i.e. locally) again
                vlcMediaPlayerChromecast.SetRenderer(selectedVlcRendererItem);

                // start the playback
                //vlcMediaPlayerChromecast.Play(media);
            }
        }
        #endregion

        #endregion 

        
        #region Preview

        #region Preview - ShowPreviewItem
        private void ShowPreviewItem(string fullFilename)
        {

            videoView1.MediaPlayer.Stop();

            if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilename))
            {
                canPlayAndPause = true;
                videoView1.MediaPlayer.Play(new Media(_libVLC, fullFilename, FromType.FromPath));

                videoView1.Visible = true;
                imageBoxPreview.Visible = false;

            }
            if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilename))
            {
                canPlayAndPause = false;

                toolStripButtonMediaPreviewPlay.Enabled = false;
                toolStripButtonMediaPreviewPause.Enabled = false;
                toolStripButtonMediaPreviewFastBackward.Enabled = false;
                toolStripButtonMediaPreviewFastForward.Enabled = false;
                toolStripTraceBarItemMediaPreviewTimer.Enabled = false;

                imageBoxPreview.Image = ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.LoadImage(fullFilename);
                imageBoxPreview.Visible = true;
                imageBoxPreview.ZoomToFit();

                videoView1.Visible = false;

            }
        }
        #endregion 

        #region Preview - Previous
        private void toolStripButtonMediaPreviewPrevious_Click(object sender, EventArgs e)
        {
            if (previewItems.Count == 0) return;
            previewMediaindex--;
            if (previewMediaindex < 0) previewMediaindex = previewItems.Count - 1;
            if (previewItems.Count > 0) ShowPreviewItem(previewItems[previewMediaindex]);            
        }
        #endregion 

        #region Preview - Next
        private void toolStripButtonMediaPreviewNext_Click(object sender, EventArgs e)
        {
            if (previewItems.Count == 0) return;
            previewMediaindex++;
            if (previewMediaindex > previewItems.Count - 1) previewMediaindex = 0;
            if (previewItems.Count > 0) ShowPreviewItem(previewItems[previewMediaindex]);
        }
        #endregion 

        #region Preview - DropDown - Media Selected
        private void ToolStripDropDownItemPreviewMedia_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedToolStripMenuItem = (ToolStripMenuItem)sender;
            previewMediaindex = (int)clickedToolStripMenuItem.Tag;
            ShowPreviewItem(clickedToolStripMenuItem.Text);
        }
        #endregion

        #region Preview - Play
        private void toolStripButtonMediaPreviewPlay_Click(object sender, EventArgs e)
        {

            if (vlcMediaPlayerVideoView.Length != -1)
            {
                if (!vlcMediaPlayerVideoView.WillPlay)
                {
                    if (previewItems.Count > 0) ShowPreviewItem(previewItems[previewMediaindex]);
                }
                else
                {
                    if (canPlayAndPause) vlcMediaPlayerVideoView.Play();
                }
            }
        }
        #endregion

        #region Preview - Pause
        private void toolStripButtonMediaPreviewPause_Click(object sender, EventArgs e)
        {
            if (canPlayAndPause) videoView1.MediaPlayer.Pause();
        }
        #endregion

        #region Preview - Close
        private void toolStripButtonMediaPreviewClose_Click(object sender, EventArgs e)
        {
            videoView1.MediaPlayer.Stop();
            panelMediaPreview.Visible = false;
        }
        #endregion

        #endregion 


    }


}


    
