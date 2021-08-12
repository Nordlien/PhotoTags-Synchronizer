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
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SkinFramework;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        public const string LinkTabAndDataGridViewNameTags = "Tags";
        public const string LinkTabAndDataGridViewNameMap = "Map";
        public const string LinkTabAndDataGridViewNamePeople = "People";
        public const string LinkTabAndDataGridViewNameDates = "Dates";
        public const string LinkTabAndDataGridViewNameExiftool = "Exiftool";
        public const string LinkTabAndDataGridViewNameWarnings = "MetadataWarning";
        public const string LinkTabAndDataGridViewNameProperties = "Properties";
        public const string LinkTabAndDataGridViewNameRename = "Rename";
        public const string LinkTabAndDataGridViewNameConvertAndMerge = "Convert and Merge";

        public void UpdateColorControls(Control control, bool useDarkMode)
        {
            if (control is Button ||
                control is CheckBox ||
                control is CheckedListBox ||
                control is ComboBox ||
                control is DateTimePicker ||
                //control is Form ||
                control is GroupBox ||
                control is HScrollBar || control is VScrollBar ||
                control is Panel ||
                control is ProgressBar ||
                control is PictureBox ||
                control is Label ||
                control is MdiClient ||
                control is RadioButton ||
                control is TabControl ||
                control is TabPage ||
                control is TrackBar ||
                control is TextBox ||
                control is ToolStrip ||                
                control is ToolStripContainer ||
                control is ToolStripContentPanel ||
                control is ToolStripPanel ||
                control is SplitContainer ||

                control is CefSharp.WinForms.ChromiumWebBrowser ||
                control is Cyotek.Windows.Forms.ImageBox ||
                control is DragNDrop.TreeViewWithoutDoubleClick ||
                control is Furty.Windows.Forms.FolderTreeView ||
                control is LibVLCSharp.WinForms.VideoView ||
                control is Manina.Windows.Forms.ImageListView 
                
                )
            {
                if (useDarkMode)
                {
                    control.BackColor = Color.Black;
                    control.ForeColor = Color.Gray;
                } else
                {
                    control.BackColor = SystemColors.Control;
                    control.ForeColor = SystemColors.ControlText;
                }
            }
            else if (control is Form)
            {
                DefaultSkin skin = DefaultSkin.Office2007Luna;
                if (useDarkMode)
                {
                    skin = DefaultSkin.Office2007Obsidian;
                    control.BackColor = Color.Black;
                    control.ForeColor = Color.Gray;
                }
                else
                {
                    skin = DefaultSkin.Office2007Luna;
                    control.BackColor = SystemColors.Control;
                    control.ForeColor = SystemColors.ControlText;
                }
                skinningManager1.DefaultSkin = skin;
            }
            else if (control is DataGridView)
            {
                if (useDarkMode)
                {
                    DataGridView MyDgv = (DataGridView)control;
                    MyDgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                    MyDgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gray;
                    MyDgv.RowHeadersDefaultCellStyle.BackColor = Color.Black;
                    MyDgv.RowHeadersDefaultCellStyle.ForeColor = Color.Gray;

                    MyDgv.DefaultCellStyle.BackColor = Color.DarkGray;
                    MyDgv.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                    MyDgv.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                    MyDgv.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                }
                else
                {
                    DataGridView MyDgv = (DataGridView)control;
                    MyDgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                    MyDgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;
                    MyDgv.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                    MyDgv.RowHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;

                    MyDgv.DefaultCellStyle.BackColor = SystemColors.Window;
                    MyDgv.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                    MyDgv.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                    MyDgv.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                }
            } else
            {
                // Any other non-standard controls should be implemented here aswell...
            }

            foreach (Control subControls in control.Controls)
            {
                UpdateColorControls(subControls, useDarkMode);
            }
        }


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

        private List<AutoKeywordConvertion> autoKeywordConvertions = new List<AutoKeywordConvertion>();

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
            #region Initialize VLC player
            SplashForm.UpdateStatus("Initialize VLC player...");

            if (!DesignMode) Core.Initialize();
            #endregion

            #region Initialize components
            SplashForm.UpdateStatus("Initialize components...");

            InitializeComponent();
            #endregion
            //this.toolStripContainer1.TopToolStripPanel.RowMargin = new Padding(0);
            if (Properties.Settings.Default.ApplicationDarkMode == true) UpdateColorControls(this, Properties.Settings.Default.ApplicationDarkMode);
            this.Invalidate();

            #region Initialize VLC player
            SplashForm.UpdateStatus("Staring VLC player...");
            
            _libVLC = new LibVLC();
            videoView1.MediaPlayer = new MediaPlayer(_libVLC);
            #endregion 

            #region Loading ImageListView renderers
            SplashForm.UpdateStatus("Loading ImageListView renderers...");
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
            
            renderertoolStripComboBox.SelectedIndex = Properties.Settings.Default.RenderertoolStripComboBox;
            SetImageListViewRender();

            imageListView1.TitleLine1 = ChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine1);
            imageListView1.TitleLine2 = ChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine2);
            imageListView1.TitleLine3 = ChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine3);
            imageListView1.TitleLine4 = ChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine4);
            imageListView1.TitleLine5 = ChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine5);
            ImageListViewHandler.SetImageListViewCheckedValues(imageListView1, Properties.Settings.Default.ImageListViewSelectedColumns);
            #endregion

            #region Initialize database connect
            SplashForm.UpdateStatus("Initialize database: connect...");
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
            #endregion

            #region Initialize database: read metadata to cache
            SplashForm.UpdateStatus("Initialize database: read metadata to cache...");
            try
            {
                Thread threadCache = new Thread(() =>
                {
                    
                    if (cacheAllThumbnails) databaseAndCacheThumbnail.ReadToCacheFolder(null);                    
                    if (cacheAllMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas();
                    if (cacheAllWebScraperDataSets) databaseAndCacheMetadataExiftool.ReadToCacheWebScarpingAllDataSets();
                });
                threadCache.Start();
                threadCache.Priority = (ThreadPriority)Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity;
            }
            catch { }
            #endregion 

            filesCutCopyPasteDrag = new FilesCutCopyPasteDrag(databaseAndCacheMetadataExiftool, databaseAndCacheMetadataWindowsLivePhotoGallery,
                databaseAndCacheMetadataMicrosoftPhotos, databaseAndCacheThumbnail, databaseExiftoolData, databaseExiftoolWarning);

            #region Connect to Microsoft Photos
            SplashForm.UpdateStatus("Initialize database: Connect to Microsoft Photos...");
            try
            {
                databaseMicrosoftPhotos = new MicrosoftPhotosReader();
            }
            catch (Exception e)
            {

                SplashForm.AddWarning("Windows photo warning:\r\n" + e.Message + "\r\n");
                databaseMicrosoftPhotos = null;
            }
            #endregion 

            #region Connect to Windows Live Photo Gallery
            SplashForm.UpdateStatus("Initialize database: Connect to Windows Live Photo Gallery...");
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
            #endregion 

            #region Configure ChromiumWebBrowser
            try
            {
                SplashForm.UpdateStatus("Configure ChromiumWebBrowser...");
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
                browser.AddressChanged += this.OnBrowserAddressChanged;
            } catch (Exception ex)
            {
                Logger.Error(ex, "Cef Browser");
            }
            #endregion 

            #region Initialize global data
            SplashForm.UpdateStatus("Initialize global data...");

            #region Setup Global Variables - Cache config
            cacheNumberOfPosters = (int)Properties.Settings.Default.CacheNumberOfPosters;
            cacheAllMetadatas = Properties.Settings.Default.CacheAllMetadatas;
            cacheAllThumbnails = Properties.Settings.Default.CacheAllThumbnails;
            cacheAllWebScraperDataSets = Properties.Settings.Default.CacheAllWebScraperDataSets;

            cacheFolderMetadatas = Properties.Settings.Default.CacheFolderMetadatas;
            cacheFolderThumbnails = Properties.Settings.Default.CacheFolderThumbnails;
            cacheFolderWebScraperDataSets = Properties.Settings.Default.CacheFolderWebScraperDataSets;
            #endregion

            #region Setup Global Variables - ThumbnailSize
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
            #endregion

            #region Setup Global Variables - Link Tab and DataGridView
            tabPageTags.Tag = LinkTabAndDataGridViewNameTags;
            GlobalData.dataGridViewHandlerTags = new DataGridViewHandler(dataGridViewTagsAndKeywords, LinkTabAndDataGridViewNameTags, "Metadata/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords);

            tabPageMap.Tag = LinkTabAndDataGridViewNameMap;
            GlobalData.dataGridViewHandlerMap = new DataGridViewHandler(dataGridViewMap, LinkTabAndDataGridViewNameMap, "Location/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeMap);

            tabPagePeople.Tag = LinkTabAndDataGridViewNamePeople;
            GlobalData.dataGridViewHandlerPeople = new DataGridViewHandler(dataGridViewPeople, LinkTabAndDataGridViewNamePeople, "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizePeoples);

            tabPageDates.Tag = LinkTabAndDataGridViewNameDates;
            GlobalData.dataGridViewHandlerDates = new DataGridViewHandler(dataGridViewDate, LinkTabAndDataGridViewNameDates, "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeDates);

            tabPageExifTool.Tag = LinkTabAndDataGridViewNameExiftool;
            GlobalData.dataGridViewHandlerExiftoolTags = new DataGridViewHandler(dataGridViewExifTool, LinkTabAndDataGridViewNameExiftool, "File/Tag Description", (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool);

            tabPageExifToolWarnings.Tag = LinkTabAndDataGridViewNameWarnings;
            GlobalData.dataGridViewHandlerExiftoolWarning = new DataGridViewHandler(dataGridViewExifToolWarning, LinkTabAndDataGridViewNameWarnings, "File and version/Tag region and command", (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings);

            tabPageProperties.Tag = LinkTabAndDataGridViewNameProperties;
            GlobalData.dataGridViewHandlerProperties = new DataGridViewHandler(dataGridViewProperties, LinkTabAndDataGridViewNameProperties, "File/Properties", (DataGridViewSize)Properties.Settings.Default.CellSizeProperties);
            GlobalData.dataGridViewHandlerProperties.ShowMediaPosterWindowToolStripMenuItemSelectedEvent += DataGridViewHandlerConvertAndMerge_ShowMediaPosterWindowToolStripMenuItemSelectedEvent;

            tabPageRename.Tag = LinkTabAndDataGridViewNameRename;
            GlobalData.dataGridViewHandlerRename = new DataGridViewHandler(dataGridViewRename, LinkTabAndDataGridViewNameRename, "Filename/Values", ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize));
            GlobalData.dataGridViewHandlerRename.ShowMediaPosterWindowToolStripMenuItemSelectedEvent += DataGridViewHandlerConvertAndMerge_ShowMediaPosterWindowToolStripMenuItemSelectedEvent;

            tabPageConvertAndMerge.Tag = LinkTabAndDataGridViewNameConvertAndMerge;
            GlobalData.dataGridViewHandlerConvertAndMerge = new DataGridViewHandler(dataGridViewConvertAndMerge, LinkTabAndDataGridViewNameConvertAndMerge, "Full path of media file", ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize));
            GlobalData.dataGridViewHandlerConvertAndMerge.ShowMediaPosterWindowToolStripMenuItemSelectedEvent += DataGridViewHandlerConvertAndMerge_ShowMediaPosterWindowToolStripMenuItemSelectedEvent;
            #endregion

            #region Setup Global Variables - Map
            isSettingDefaultComboxValues = true;
            comboBoxGoogleTimeZoneShift.SelectedIndex = Properties.Settings.Default.ComboBoxGoogleTimeZoneShift;    //0 time shift = 12
            comboBoxGoogleLocationInterval.SelectedIndex = Properties.Settings.Default.ComboBoxGoogleLocationInterval;    //30 minutes Index 2
            comboBoxMapZoomLevel.SelectedIndex = Properties.Settings.Default.ComboBoxMapZoomLevel;     //13 map zoom level 14
            #endregion 

            #region Setup Global Variables - Rename
            textBoxRenameNewName.Text = Properties.Settings.Default.RenameVariable;
            isSettingDefaultComboxValues = false;
            #endregion 

            #region Setup Global Variables - Thumbnail
            ThumbnailSaveSize = Properties.Settings.Default.ApplicationThumbnail;
            RegionThumbnailHandler.FaceThumbnailSize = Properties.Settings.Default.ApplicationRegionThumbnail;
            #endregion
            #endregion

            autoKeywordConvertions = AutoKeywordHandler.PopulateList(AutoKeywordHandler.ReadDataSetFromXML());

            #region Initialize layout setup

            #region Initialize layout setup - Windows Size and Splitters
            SplashForm.UpdateStatus("Initialize layout setup: Sizes...");
            isFormLoading = true; //MainForm_Shown(object sender, EventArgs e) -> isFormLoading = false;

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
            splitContainerFolder.BorderStyle = BorderStyle.None;
            splitContainerFolder.SplitterDistance = Properties.Settings.Default.SplitContainerFolder;
            splitContainerImages.BorderStyle = BorderStyle.None;
            splitContainerImages.SplitterDistance = Properties.Settings.Default.SplitContainerImages;
            splitContainerMap.BorderStyle = BorderStyle.None;
            splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
            
            toolStripButtonHistortyColumns.Checked = Properties.Settings.Default.ShowHistortyColumns;
            toolStripButtonErrorColumns.Checked = Properties.Settings.Default.ShowErrorColumns;

            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);

            timerShowErrorMessage.Enabled = true;
            #endregion 

            this.SuspendLayout();

            #region Initialize layout setup - Initialize layout toolstrip: Exiftool
            SplashForm.UpdateStatus("Initialize layout toolstrip: Exiftool...");
            PopulateExiftoolToolStripMenuItems();
            #endregion 

            #region Initialize layout setup - Initialize layout toolstrip: Warnings
            SplashForm.UpdateStatus("Initialize layout toolstrip: Warnings...");
            PopulateExiftoolWarningToolStripMenuItems();
            #endregion 

            #region Initialize layout setup - Initialize layout toolstrip: People
            SplashForm.UpdateStatus("Initialize layout toolstrip: People...");
            PopulatePeopleToolStripMenuItems();
            #endregion 

            #endregion

            #region Initialize nHTTP server
            SplashForm.UpdateStatus("Initialize nHTTP server...");
            try
            {
                _ThreadHttp = new Thread(() =>
                {
                    try
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
                            nHttpServerThreadWaitApplicationClosing = new AutoResetEvent(false);
                            nHttpServerThreadWaitApplicationClosing.WaitOne();
                            Application.DoEvents();
                        }
                    } catch(Exception ex)
                    {
                        Logger.Error(ex);
                    }
                });
                _ThreadHttp.Start();
                _ThreadHttp.Priority = (ThreadPriority)Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity;
            } catch(Exception ex)
            {
                Logger.Error(ex);
            }
            #endregion


            this.ResumeLayout();
        }
        #endregion

        #region Resize and restore windows size when reopen application        
        private void tabControlToolbox_Selecting(object sender, TabControlCancelEventArgs e)
        {
            isTabControlToolboxChanging = true;
        }

        private void tabControlToolbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                isTabControlToolboxChanging = false;
                PopulateDataGridViewForSelectedItemsThread(imageListView1.SelectedItems);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Was not able to to populate data grid view");
                Logger.Error(ex);
            }
        }

        private void splitContainerMap_SplitterMoved(object sender, SplitterEventArgs e)
        {
            /*
            if (isTabControlToolboxChanging) return;
            if (_previousWindowsState == FormWindowState.Minimized) return;

            if (!isFormLoading)
            {
                Properties.Settings.Default.SplitContainerMap = splitContainerMap.SplitterDistance;
            }
            */
        }

        private void splitContainerImages_SplitterMoved(object sender, SplitterEventArgs e)
        {
            /*
            if (!isFormLoading)
            {
                Properties.Settings.Default.SplitContainerImages = splitContainerImages.SplitterDistance;
            }
            */
        }

        private void splitContainerFolder_SplitterMoved(object sender, SplitterEventArgs e)
        {
            /*
            if (!isFormLoading)
            {
                Properties.Settings.Default.SplitContainerFolder = splitContainerFolder.SplitterDistance;
            }
            */
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (isFormLoading)
            {
                /*splitContainerFolder.SplitterDistance = Properties.Settings.Default.SplitContainerFolder;
                splitContainerImages.SplitterDistance = Properties.Settings.Default.SplitContainerImages;
                splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;*/
                return;
            }
            
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

            try
            {
                exiftoolReader.MetadataGroupPrioritiesWrite(); //Updated json config file if new tags found
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Can't save settings, Metadata Group Priorities");
            }

            try
            {
                try
                {
                    browser.Dispose();
                } catch { }

                GlobalData.IsApplicationClosing = true;
                GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = true;
                MetadataDatabaseCache.StopCaching = true;
                ThumbnailDatabaseCache.StopCaching = true;

                //Close down nHTTP server;
                nHttpServerThreadWaitApplicationClosing.Set();

                SplashForm.ShowSplashScreen("PhotoTags Synchronizer - Closing...", 6, false, false);

                SplashForm.UpdateStatus("Saving layout...");

                

                //---------------------------------------------------------
                try
                {
                    if (this.WindowState == FormWindowState.Normal)
                    {
                        Properties.Settings.Default.IsMainFormMaximized = false;
                        Properties.Settings.Default.MainFormSize = this.Size;
                        Properties.Settings.Default.MainFormLocation = this.Location;
                    }
                    else
                    {
                        Properties.Settings.Default.IsMainFormMaximized = true;
                        Properties.Settings.Default.MainFormSize = this.RestoreBounds.Size;
                        Properties.Settings.Default.MainFormLocation = this.RestoreBounds.Location;
                    }

                    Properties.Settings.Default.SplitContainerImages = splitContainerImages.SplitterDistance;
                    Properties.Settings.Default.SplitContainerFolder = splitContainerFolder.SplitterDistance;
                    Properties.Settings.Default.SplitContainerMap = splitContainerMap.SplitterDistance;
                } catch { }

                try
                {
                    Properties.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Can't save settings");
                }
                //---------------------------------------------------------

                SplashForm.UpdateStatus("Closing Exiftool read...");
                try
                {
                    if (exiftoolReader != null) exiftoolReader.Close();
                }
                catch { }

                //---------------------------------------------------------

                try
                {
                    ImageListViewClearAll(imageListView1);

                    imageListView1.Dispose();
                    imageListView1.StoppBackgroundThreads();
                }
                catch { }

                //---------------------------------------------------------
                Application.DoEvents();
                Task.Delay(200).Wait(); 

                int waitForProcessEndRetray = 30;

                SplashForm.UpdateStatus("Stopping ImageView background threads...");
                try
                {
                    waitForProcessEndRetray = 30;
                    while (!imageListView1.IsBackgroundThreadsStopped() && waitForProcessEndRetray-- > 0)
                    {
                        Application.DoEvents();
                        Task.Delay(200).Wait();
                    }
                } catch { }

                SplashForm.UpdateStatus("Stopping fetch metadata background threads...");
                try
                {
                    waitForProcessEndRetray = 30;
                    while (IsAnyThreadRunning() && waitForProcessEndRetray-- > 0)
                    {
                        Application.DoEvents();
                        Task.Delay(100).Wait();
                    }
                }
                catch { }

                SplashForm.UpdateStatus("Disconnecting databases...");
                try
                {
                    databaseUtilitiesSqliteMetadata.DatabaseClose(); //Close database after all background threads stopped
                }
                catch { }

                SplashForm.UpdateStatus("Disposing...");
                try
                {
                    imageListView1.Dispose();
                }
                catch { }

                SplashForm.CloseForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Problems during close all threads and other process during closing application");
            }

        }
        #endregion

        #region MainForm_Load
        private void MainForm_Load(object sender, EventArgs e)
        {
            
            SplashForm.CloseForm();

            Properties.Settings.Default.Reload();
            RegionStructure.SetAcceptRegionMissmatchProcent((float)Properties.Settings.Default.RegionMissmatchProcent);

            isFormLoading = true;
            this.Size = Properties.Settings.Default.MainFormSize;
            this.Location = Properties.Settings.Default.MainFormLocation;
            this.Activate();

            imageListView1.Focus();
        }
        #endregion

        #region MainForm_Shown
        private void MainForm_Shown(object sender, EventArgs e)
        {
            isFormLoading = false;
            

            #region Initialize folder tree...
            //If in Form_Load
            //System.InvalidOperationException: 'Cross-thread operation not valid: Control 'toolStrip1' accessed from a thread other than the thread it was created on.'
            try
            {
                SplashForm.UpdateStatus("Initialize folder tree...");
                GlobalData.IsPopulatingFolderTree = true;

                this.folderTreeViewFolder.InitFolderTreeView();
                folderTreeViewFolder.SuspendLayout();
                string folder = Properties.Settings.Default.LastFolder;
                if (Directory.Exists(folder))
                    folderTreeViewFolder.DrillToFolder(folder);
                else
                    folderTreeViewFolder.SelectedNode = folderTreeViewFolder.Nodes[0];
                folderTreeViewFolder.ResumeLayout();
                GlobalData.IsPopulatingFolderTree = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion

            #region Populate search filters...
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
            #endregion

            InitializeDataGridViewHandler();

            PopulateImageListView_FromFolderSelected(false, true);
            FilesSelected();
        }

        #endregion

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {

        }
    }


}


    
