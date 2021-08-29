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
using PhotoTagsCommonComponets;
using Krypton.Toolkit;
using FileHandeling;

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : KryptonForm
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
        private string filenameLayoutMain;
        private string filenameLayoutMap;
        private string filenameLayoutRename;
        private string filenameLayoutTags;
        
        public void UpdateColorControls(Control control, bool useDarkMode)
        {
            if (useDarkMode)
            {
                //kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2010Black;
            }
            else
            {
                //kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2007Silver;
            }
            /*
            Color LighterColor(Color color, float correctionfactory = 50f)
            {
                correctionfactory = correctionfactory / 100f;
                const float rgb255 = 255f;
                return Color.FromArgb(
                    (int)((float)color.R + ((rgb255 - (float)color.R) * correctionfactory)), 
                    (int)((float)color.G + ((rgb255 - (float)color.G) * correctionfactory)), 
                    (int)((float)color.B + ((rgb255 - (float)color.B) * correctionfactory))
                    );
            }

            Color DarkerColor(Color color, float correctionfactory = 50f)
            {
                const float hundredpercent = 100f;
                return Color.FromArgb(
                    (int)(((float)color.R / hundredpercent) * (hundredpercent - correctionfactory)),
                    (int)(((float)color.G / hundredpercent) * (hundredpercent - correctionfactory)), 
                    (int)(((float)color.B / hundredpercent) * (hundredpercent - correctionfactory))
                    );
            }
            if (useDarkMode)
            {
                DataGridViewHandler.ColorCellEditable = SystemColors.ControlDarkDark;
                DataGridViewHandler.ColorFavourite = LighterColor(SystemColors.ControlDarkDark, 20);
                DataGridViewHandler.ColorReadOnly = DarkerColor(SystemColors.ControlDarkDark, 20);                
                DataGridViewHandler.ColorReadOnlyFavourite = DarkerColor(SystemColors.ControlDarkDark, 10);
                DataGridViewHandler.ColorError = Color.FromArgb(128, 32, 32);
          
                
                DataGridViewHandler.ColorHeader = Color.Black;
                
                DataGridViewHandler.ColorHeaderImage = SystemColors.ControlDarkDark;
                DataGridViewHandler.ColorHeaderError = Color.Red;
                DataGridViewHandler.ColorHeaderWarning = Color.Yellow;
                DataGridViewHandler.ColorRegionFace = Color.Black;
            }
            else
            {
                DataGridViewHandler.ColorReadOnly = SystemColors.GradientInactiveCaption;
                DataGridViewHandler.ColorError = Color.FromArgb(255, 192, 192);
                DataGridViewHandler.ColorFavourite = SystemColors.ControlLight;
                DataGridViewHandler.ColorReadOnlyFavourite = SystemColors.MenuHighlight;
                DataGridViewHandler.ColorHeader = SystemColors.Control;
                DataGridViewHandler.ColorCellEditable = SystemColors.ControlLightLight;
                DataGridViewHandler.ColorHeaderImage = Color.LightSteelBlue;
                DataGridViewHandler.ColorHeaderError = Color.Red;
                DataGridViewHandler.ColorHeaderWarning = Color.Yellow;
                DataGridViewHandler.ColorRegionFace = Color.White;
            }
            UpdateColorControlsRecursive(control, useDarkMode);
            //this.Refresh();
            */
        }

        public void UpdateColorControlsRecursive(Control control, bool useDarkMode)
        {
            if (control is Button ||
                control is CheckBox ||
                control is CheckedListBox ||
                
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
                control is PhotoTagsCommonComponets.TreeViewWithoutDoubleClick ||
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
                
                //DefaultSkin skin = DefaultSkin.Office2007Luna;
                
               
            }
            else if (control is DataGridView)
            {
                if (useDarkMode)
                {
                    DataGridView MyDgv = (DataGridView)control;
                    MyDgv.BackgroundColor = SystemColors.ControlDarkDark;
                    MyDgv.GridColor = SystemColors.ControlDark;

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
                    MyDgv.BackgroundColor = SystemColors.AppWorkspace;
                    MyDgv.GridColor = SystemColors.ControlDark;

                    MyDgv.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                    MyDgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;
                    MyDgv.RowHeadersDefaultCellStyle.BackColor = SystemColors.Control;
                    MyDgv.RowHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;

                    MyDgv.DefaultCellStyle.BackColor = SystemColors.Window;
                    MyDgv.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                    MyDgv.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                    MyDgv.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                }
            }
            else if (control is PhotoTagsCommonComponets.ComboBoxCustom)
            {
                if (useDarkMode)
                {
                    ((ComboBoxCustom)control).BorderColor = SystemColors.ControlDarkDark;
                    ((ComboBoxCustom)control).BackColor = Color.Black;
                    ((ComboBoxCustom)control).ForeColor = Color.Gray;
                }
                else
                {
                    ((ComboBoxCustom)control).BorderColor = SystemColors.Control;
                    ((ComboBoxCustom)control).BackColor = SystemColors.ControlDarkDark; ;
                    ((ComboBoxCustom)control).ForeColor = SystemColors.ControlText;
                }
            }
            else
            {
                // Any other non-standard controls should be implemented here aswell...
            }

            foreach (Control subControls in control.Controls)
            {
                UpdateColorControlsRecursive(subControls, useDarkMode);
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
        private FormWindowState _previousWindowsState = FormWindowState.Normal;
        bool isSlideShowRunning = false;
        int slideShowIntervalMs = 0;

        #region Constructor - MainForm()
        public MainForm()
        {
            #region Initialize VLC player
            FormSplash.UpdateStatus("Initialize VLC player...");
            try
            {
                if (!DesignMode) Core.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Was not able to load VLC player");
                return;
            }
            #endregion

            #region Initialize components
            FormSplash.UpdateStatus("Initialize components...");

            InitializeComponent();
            #endregion

           

            this.toolStripContainerStripMainForm.RenderMode = ToolStripRenderMode.Professional;
            this.toolStripContainerStripMainForm.Renderer = new PhotoTagsCommonComponets.ToolStripProfessionalRendererWithoutLines();
            this.toolStripContainerStripMediaPreview.RenderMode = ToolStripRenderMode.Professional;
            this.toolStripContainerStripMediaPreview.Renderer = new PhotoTagsCommonComponets.ToolStripProfessionalRendererWithoutLines();
            //this.toolStripContainer1.TopToolStripPanel.RowMargin = new Padding(0);
            if (Properties.Settings.Default.ApplicationDarkMode == true) UpdateColorControls(this, Properties.Settings.Default.ApplicationDarkMode);


            #region Initialize VLC player
            FormSplash.UpdateStatus("Staring VLC player...");
            
            _libVLC = new LibVLC();
            videoView1.MediaPlayer = new MediaPlayer(_libVLC);
            #endregion 

            #region Loading ImageListView renderers
            FormSplash.UpdateStatus("Loading ImageListView renderers...");
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

            imageListView1.TitleLine1 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine1);
            imageListView1.TitleLine2 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine2);
            imageListView1.TitleLine3 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine3);
            imageListView1.TitleLine4 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine4);
            imageListView1.TitleLine5 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine5);
            ImageListViewHandler.SetImageListViewCheckedValues(imageListView1, Properties.Settings.Default.ImageListViewSelectedColumns);
            #endregion

            #region Initialize database connect
            FormSplash.UpdateStatus("Initialize database: connect...");
            try
            {
                databaseUtilitiesSqliteMetadata = new SqliteDatabaseUtilities(DatabaseType.SqliteMetadataDatabase, 10000, 5000);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Was not able to start the database");
                Close();
                return;
            }
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
            FormSplash.UpdateStatus("Initialize database: read metadata to cache...");
            try
            {
                Thread threadCache = new Thread(() =>
                {
                    
                    if (cacheAllThumbnails) databaseAndCacheThumbnail.ReadToCacheFolder(null);                    
                    if (cacheAllMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas();
                    if (cacheAllWebScraperDataSets) databaseAndCacheMetadataExiftool.ReadToCacheWebScarpingAllDataSets();
                });
                threadCache.Start();
                if (threadCache.IsAlive) threadCache.Priority = (ThreadPriority)Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity;
            }
            catch { }
            #endregion 

            filesCutCopyPasteDrag = new FilesCutCopyPasteDrag(databaseAndCacheMetadataExiftool, databaseAndCacheMetadataWindowsLivePhotoGallery,
                databaseAndCacheMetadataMicrosoftPhotos, databaseAndCacheThumbnail, databaseExiftoolData, databaseExiftoolWarning);

            #region Connect to Microsoft Photos
            FormSplash.UpdateStatus("Initialize database: Connect to Microsoft Photos...");
            try
            {
                databaseMicrosoftPhotos = new MicrosoftPhotosReader();
            }
            catch (Exception e)
            {

                FormSplash.AddWarning("Windows photo warning:\r\n" + e.Message + "\r\n");
                databaseMicrosoftPhotos = null;
            }
            #endregion 

            #region Connect to Windows Live Photo Gallery
            FormSplash.UpdateStatus("Initialize database: Connect to Windows Live Photo Gallery...");
            try
            {
                databaseWindowsLivePhotGallery = new WindowsLivePhotoGalleryDatabasePipe();
                databaseWindowsLivePhotGallery.ConnectDatabase(databaseAndCacheMetadataWindowsLivePhotoGallery);
            }
            catch (Exception e)
            {
                FormSplash.AddWarning("Windows Live Photo Gallery warning:\r\n" + e.Message + "\r\n");
                databaseWindowsLivePhotGallery = null;
            }
            #endregion 

            #region Configure ChromiumWebBrowser
            try
            {
                FormSplash.UpdateStatus("Configure ChromiumWebBrowser...");
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
            FormSplash.UpdateStatus("Initialize global data...");

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
            //kryptonPageToolboxTags
            kryptonPageToolboxTags.Tag = LinkTabAndDataGridViewNameTags;
            GlobalData.dataGridViewHandlerTags = new DataGridViewHandler(dataGridViewTagsAndKeywords, LinkTabAndDataGridViewNameTags, "Metadata/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords);

            kryptonPageToolboxMap.Tag = LinkTabAndDataGridViewNameMap;
            GlobalData.dataGridViewHandlerMap = new DataGridViewHandler(dataGridViewMap, LinkTabAndDataGridViewNameMap, "Location/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeMap);

            kryptonPageToolboxPeople.Tag = LinkTabAndDataGridViewNamePeople;
            GlobalData.dataGridViewHandlerPeople = new DataGridViewHandler(dataGridViewPeople, LinkTabAndDataGridViewNamePeople, "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizePeoples);

            kryptonPageToolboxDates.Tag = LinkTabAndDataGridViewNameDates;
            GlobalData.dataGridViewHandlerDates = new DataGridViewHandler(dataGridViewDate, LinkTabAndDataGridViewNameDates, "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeDates);

            kryptonPageToolboxExiftool.Tag = LinkTabAndDataGridViewNameExiftool;
            GlobalData.dataGridViewHandlerExiftoolTags = new DataGridViewHandler(dataGridViewExifTool, LinkTabAndDataGridViewNameExiftool, "File/Tag Description", (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool);

            kryptonPageToolboxWarnings.Tag = LinkTabAndDataGridViewNameWarnings;
            GlobalData.dataGridViewHandlerExiftoolWarning = new DataGridViewHandler(dataGridViewExifToolWarning, LinkTabAndDataGridViewNameWarnings, "File and version/Tag region and command", (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings);

            kryptonPageToolboxProperties.Tag = LinkTabAndDataGridViewNameProperties;
            GlobalData.dataGridViewHandlerProperties = new DataGridViewHandler(dataGridViewProperties, LinkTabAndDataGridViewNameProperties, "File/Properties", (DataGridViewSize)Properties.Settings.Default.CellSizeProperties);
            GlobalData.dataGridViewHandlerProperties.ShowMediaPosterWindowToolStripMenuItemSelectedEvent += DataGridViewHandlerConvertAndMerge_ShowMediaPosterWindowToolStripMenuItemSelectedEvent;

            kryptonPageToolboxRename.Tag = LinkTabAndDataGridViewNameRename;
            GlobalData.dataGridViewHandlerRename = new DataGridViewHandler(dataGridViewRename, LinkTabAndDataGridViewNameRename, "Filename/Values", ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize));
            GlobalData.dataGridViewHandlerRename.ShowMediaPosterWindowToolStripMenuItemSelectedEvent += DataGridViewHandlerConvertAndMerge_ShowMediaPosterWindowToolStripMenuItemSelectedEvent;

            kryptonPageToolboxConvertAndMerge.Tag = LinkTabAndDataGridViewNameConvertAndMerge;
            GlobalData.dataGridViewHandlerConvertAndMerge = new DataGridViewHandler(dataGridViewConvertAndMerge, LinkTabAndDataGridViewNameConvertAndMerge, "Full path of media file", ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize));
            GlobalData.dataGridViewHandlerConvertAndMerge.ShowMediaPosterWindowToolStripMenuItemSelectedEvent += DataGridViewHandlerConvertAndMerge_ShowMediaPosterWindowToolStripMenuItemSelectedEvent;
            #endregion

            isSettingDefaultComboxValues = true;
            #region Setup Global Variables - Map
            comboBoxGoogleTimeZoneShift.SelectedIndex = Properties.Settings.Default.ComboBoxGoogleTimeZoneShift;    //0 time shift = 12
            comboBoxGoogleLocationInterval.SelectedIndex = Properties.Settings.Default.ComboBoxGoogleLocationInterval;    //30 minutes Index 2
            comboBoxMapZoomLevel.SelectedIndex = Properties.Settings.Default.ComboBoxMapZoomLevel;     //13 map zoom level 14
            #endregion 

            #region Setup Global Variables - Rename
            textBoxRenameNewName.Text = Properties.Settings.Default.RenameVariable;
            #endregion
            isSettingDefaultComboxValues = false;

            #region Setup Global Variables - Thumbnail
            ThumbnailSaveSize = Properties.Settings.Default.ApplicationThumbnail;
            RegionThumbnailHandler.FaceThumbnailSize = Properties.Settings.Default.ApplicationRegionThumbnail;
            #endregion
            #endregion

            autoKeywordConvertions = AutoKeywordHandler.PopulateList(AutoKeywordHandler.ReadDataSetFromXML());

            #region Initialize layout setup

            #region Initialize layout setup - Windows Size and Splitters
            FormSplash.UpdateStatus("Initialize layout setup: Sizes...");
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

            filenameLayoutMain = FileHandler.GetLocalApplicationDataPath("LayoutMain.xml", false);
            filenameLayoutMap = FileHandler.GetLocalApplicationDataPath("LayoutMap.xml", false);
            filenameLayoutRename = FileHandler.GetLocalApplicationDataPath("LayoutRename.xml", false);
            filenameLayoutTags = FileHandler.GetLocalApplicationDataPath("LauoutTags.xml", false);

            /*
            splitContainerFolder.BorderStyle = BorderStyle.None;
            splitContainerFolder.SplitterDistance = Properties.Settings.Default.SplitContainerFolder;
            splitContainerImages.BorderStyle = BorderStyle.None;
            splitContainerImages.SplitterDistance = Properties.Settings.Default.SplitContainerImages;
            splitContainerMap.BorderStyle = BorderStyle.None;
            splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
            */
            try
            {
                if (File.Exists(filenameLayoutMain)) kryptonWorkspaceMain.LoadLayoutFromFile(filenameLayoutMain);
                if (File.Exists(filenameLayoutMap)) kryptonWorkspaceToolboxMap.LoadLayoutFromFile(filenameLayoutMap);
                if (File.Exists(filenameLayoutRename)) kryptonWorkspaceToolboxRename.LoadLayoutFromFile(filenameLayoutRename);
                if (File.Exists(filenameLayoutTags)) kryptonWorkspaceToolboxTags.LoadLayoutFromFile(filenameLayoutTags);
            } catch
            {

            }
            toolStripButtonHistortyColumns.Checked = Properties.Settings.Default.ShowHistortyColumns;
            toolStripButtonErrorColumns.Checked = Properties.Settings.Default.ShowErrorColumns;

            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);

            timerShowErrorMessage.Enabled = true;
            #endregion 

            this.SuspendLayout();

            #region Initialize layout setup - Initialize layout toolstrip: Exiftool
            FormSplash.UpdateStatus("Initialize layout toolstrip: Exiftool...");
            PopulateExiftoolToolStripMenuItems();
            #endregion 

            #region Initialize layout setup - Initialize layout toolstrip: Warnings
            FormSplash.UpdateStatus("Initialize layout toolstrip: Warnings...");
            PopulateExiftoolWarningToolStripMenuItems();
            #endregion 

            #region Initialize layout setup - Initialize layout toolstrip: People
            FormSplash.UpdateStatus("Initialize layout toolstrip: People...");
            PopulatePeopleToolStripMenuItems();
            #endregion 

            #endregion

            #region Initialize nHTTP server
            FormSplash.UpdateStatus("Initialize nHTTP server...");
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

                
        private void kryptonWorkspaceCellToolbox_SelectedPageChanged(object sender, EventArgs e)
        {
            if (isFormLoading) return;
            try
            {
                
                PopulateDataGridViewForSelectedItemsThread(imageListView1.SelectedItems);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Was not able to to populate data grid view");
                Logger.Error(ex);
            }
        }

        #region Resize and restore windows size when reopen application
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (isFormLoading) return;
            _previousWindowsState = this.WindowState;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            FormSplash.BringToFrontSplashForm();
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

                FormSplash.ShowSplashScreen("PhotoTags Synchronizer - Closing...", 6, false, false);

                FormSplash.UpdateStatus("Saving layout...");

                

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

                    /*Properties.Settings.Default.SplitContainerImages = splitContainerImages.SplitterDistance;
                    Properties.Settings.Default.SplitContainerFolder = splitContainerFolder.SplitterDistance;
                    Properties.Settings.Default.SplitContainerMap = splitContainerMap.SplitterDistance;*/
                    

                    kryptonWorkspaceMain.SaveLayoutToFile(filenameLayoutMain);
                    kryptonWorkspaceToolboxMap.SaveLayoutToFile(filenameLayoutMap);
                    kryptonWorkspaceToolboxRename.SaveLayoutToFile(filenameLayoutRename);
                    kryptonWorkspaceToolboxTags.SaveLayoutToFile(filenameLayoutTags);

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

                FormSplash.UpdateStatus("Closing Exiftool read...");
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

                FormSplash.UpdateStatus("Stopping ImageView background threads...");
                try
                {
                    waitForProcessEndRetray = 30;
                    while (!imageListView1.IsBackgroundThreadsStopped() && waitForProcessEndRetray-- > 0)
                    {
                        Application.DoEvents();
                        Task.Delay(200).Wait();
                    }
                } catch { }

                FormSplash.UpdateStatus("Stopping fetch metadata background threads...");
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

                FormSplash.UpdateStatus("Disconnecting databases...");
                try
                {
                    databaseUtilitiesSqliteMetadata.DatabaseClose(); //Close database after all background threads stopped
                }
                catch { }

                FormSplash.UpdateStatus("Disposing...");
                try
                {
                    imageListView1.Dispose();
                }
                catch { }

                FormSplash.CloseForm();
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
            
            FormSplash.CloseForm();

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
                FormSplash.UpdateStatus("Initialize folder tree...");
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
                FormSplash.UpdateStatus("Populate search filters...");
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

    }


}


    
