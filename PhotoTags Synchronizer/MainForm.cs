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
using Krypton.Navigator;
using FileDateTime;
using ColumnNamesAndWidth;

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : KryptonForm
    {

        #region Global Variables
        public const string LinkTabAndDataGridViewNameTags = "Tags";
        public const string LinkTabAndDataGridViewNameMap = "Map";
        public const string LinkTabAndDataGridViewNamePeople = "People";
        public const string LinkTabAndDataGridViewNameDates = "Dates";
        public const string LinkTabAndDataGridViewNameExiftool = "Exiftool";
        public const string LinkTabAndDataGridViewNameWarnings = "MetadataWarning";
        public const string LinkTabAndDataGridViewNameProperties = "Properties";
        public const string LinkTabAndDataGridViewNameRename = "Rename";
        public const string LinkTabAndDataGridViewNameConvertAndMerge = "Convert and Merge";

        private string nameImageListView;
        private string nameFolderTreeViewFolder;
        private string nameDataGridViewConvertAndMerge;
        private string nameDataGridViewDate;
        private string nameDataGridViewExifTool;
        private string nameDataGridViewExifToolWarning;
        private string nameDataGridViewMap;
        private string nameDataGridViewPeople;
        private string nameDataGridViewProperties;
        private string nameDataGridViewRename;
        private string nameDataGridViewTagsAndKeywords;
        private List<string> oneDriveNetworkNames = new List<string>(); 

        private ProgressBar progressBarBackground = new ProgressBar();
        private ProgressBar progressBarSaveConvert = new ProgressBar();
        private ProgressBar progressBarLazyLoading = new ProgressBar();

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
        private RendererItem imageListViewSelectedRenderer = new RendererItem();

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

        private FileDateTimeReader fileDateTimeReader = null; 
        //Cache level
        private int cacheNumberOfPosters = 10;
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
        #endregion

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
                KryptonMessageBox.Show(ex.Message, "Was not able to load VLC player", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
                return;
            }
            #endregion

            #region Initialize components
            FormSplash.UpdateStatus("Initialize components...");
            InitializeComponent();
            #endregion

            #region InitializeComponent - Krypton

            fileDateTimeReader = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);

            KryptonPalette kryptonPalette = KryptonPaletteHandler.Load(Properties.Settings.Default.KryptonPaletteFullFilename, Properties.Settings.Default.KryptonPaletteName);
            KryptonPaletteHandler.SetPalette(this, kryptonManager1, kryptonPalette, KryptonPaletteHandler.IsSystemPalette, Properties.Settings.Default.KryptonPaletteDropShadow);
            KryptonPaletteHandler.SetImageListViewPalettes(kryptonManager1, imageListView1);

            this.kryptonRibbonGroupCustomControlToolsProgressBackground.CustomControl = progressBarBackground;
            this.kryptonRibbonGroupCustomControlToolsProgressSave.CustomControl = progressBarSaveConvert;
            this.kryptonRibbonGroupCustomControlToolsProgressLazyloading.CustomControl = progressBarLazyLoading;

            this.kryptonRibbonGroupButtonHomeSortColumn.KryptonContextMenu = kryptonContextMenuFileSystemColumnSort;
            this.imageListView1.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.kryptonRibbonGroupButtonPreviewSlideshowTimeInterval.KryptonContextMenu = kryptonContextMenuPreviewSlideshowInterval;
            this.treeViewFolderBrowser1.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.dataGridViewConvertAndMerge.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.dataGridViewDate.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.dataGridViewExiftool.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.dataGridViewExiftoolWarning.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.dataGridViewMap.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.dataGridViewPeople.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.dataGridViewProperties.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.dataGridViewRename.KryptonContextMenu = kryptonContextMenuGenericBase;
            this.dataGridViewTagsAndKeywords.KryptonContextMenu = kryptonContextMenuGenericBase;

            this.kryptonContextMenuItemGenericRegionRename1.Click += KryptonContextMenuItemGenericRegionRenameGeneric_Click;
            this.kryptonContextMenuItemGenericRegionRename2.Click += KryptonContextMenuItemGenericRegionRenameGeneric_Click;
            this.kryptonContextMenuItemGenericRegionRename3.Click += KryptonContextMenuItemGenericRegionRenameGeneric_Click;
            //this.kryptonContextMenuItemGenericRegionRenameFromLastUsed.Click += KryptonContextMenuItemRegionRenameGeneric_Click;
            //this.kryptonContextMenuItemGenericRegionRenameListAll.Click += KryptonContextMenuItemRegionRenameGeneric_Click;
            //this.kryptonContextMenuSeparatorGenericEndOfRegionRename
            this.kryptonContextMenuItemGenericCut.Click += KryptonContextMenuItemGenericCut_Click;
            this.kryptonContextMenuItemGenericCopy.Click += KryptonContextMenuItemGenericCopy_Click;
            this.kryptonContextMenuItemGenericCopyText.Click += KryptonContextMenuItemGenericCopyText_Click;
            this.kryptonContextMenuItemGenericFastCopyNoOverwrite.Click += KryptonContextMenuItemGenericFastCopyNoOverwrite_Click;
            this.kryptonContextMenuItemGenericFastCopyWithOverwrite.Click += KryptonContextMenuItemGenericFastCopyWithOverwrite_Click;
            this.kryptonContextMenuItemGenericPaste.Click += KryptonContextMenuItemGenericPaste_Click;
            this.kryptonContextMenuItemGenericDelete.Click += KryptonContextMenuItemGenericFileSystemDelete_Click;
            this.kryptonContextMenuItemGenericRename.Click += KryptonContextMenuItemGenericFileSystemRename_Click;
            this.kryptonContextMenuItemGenericUndo.Click += KryptonContextMenuItemGenericUndo_Click;
            this.kryptonContextMenuItemGenericRedo.Click += KryptonContextMenuItemGenericRedo_Click;
            this.kryptonContextMenuItemGenericFind.Click += KryptonContextMenuItemGenericFind_Click;
            this.kryptonContextMenuItemGenericReplace.Click += KryptonContextMenuItemGenericReplace_Click;
            this.kryptonContextMenuItemGenericSave.Click += KryptonContextMenuItemGenericSave_Click;
            //this.kryptonContextMenuSeparatorGenericEndOfClipboard
            this.kryptonContextMenuItemGenericRefreshFolder.Click += KryptonContextMenuItemGenericFileSystemRefreshFolder_Click;
            this.kryptonContextMenuItemGenericReadSubfolders.Click += KryptonContextMenuItemGenericReadSubfolders_Click;
            this.kryptonContextMenuItemGenericOpenFolderLocation.Click += KryptonContextMenuItemGenericOpenExplorerLocation_Click;
            this.kryptonContextMenuItemGenericOpen.Click += KryptonContextMenuItemGenericOpen_Click;
            this.kryptonContextMenuItemOpenAndAssociateWithDialog.Click += KryptonContextMenuItemOpenAndAssociateWithDialog_Click;
            this.kryptonContextMenuItemGenericOpenVerbEdit.Click += KryptonContextMenuItemGenericFileSystemVerbEdit_Click;
            this.kryptonContextMenuItemGenericRunCommand.Click += KryptonContextMenuItemGenericFileSystemRunCommand_Click;
            //this.kryptonContextMenuSeparatorGenericEndOfFileSystem,
            this.kryptonContextMenuItemGenericAutoCorrectRun.Click += KryptonContextMenuItemGenericAutoCorrectRun_Click;
            this.kryptonContextMenuItemGenericAutoCorrectForm.Click += KryptonContextMenuItemGenericAutoCorrectForm_Click;
            this.kryptonContextMenuItemGenericMetadataRefreshLast.Click += KryptonContextMenuItemGenericMetadataRefreshLast_Click;
            this.kryptonContextMenuItemGenericMetadataDeleteHistory.Click += KryptonContextMenuItemGenericMetadataReloadDeleteHistory_Click;
            //this.kryptonContextMenuSeparatorGenericEndOfMetadata,
            this.kryptonContextMenuItemGenericRotate270.Click += KryptonContextMenuItemGenericRotate270_Click;
            this.kryptonContextMenuItemGenericRotate180.Click += KryptonContextMenuItemGenericRotate180_Click;
            this.kryptonContextMenuItemGenericRotate90.Click += KryptonContextMenuItemGenericRotate90_Click;
            //this.kryptonContextMenuSeparatorEndOfRotate,
            this.kryptonContextMenuItemGenericFavoriteAdd.Click += KryptonContextMenuItemGenericFavoriteAdd_Click;
            this.kryptonContextMenuItemGenericFavoriteDelete.Click += KryptonContextMenuItemGenericFavoriteDelete_Click;
            this.kryptonContextMenuItemGenericFavoriteToggle.Click += KryptonContextMenuItemFavoriteToggle_Click;
            //this.kryptonContextMenuSeparatorGenericEndOfFavorite,
            this.kryptonContextMenuItemGenericRowShowFavorite.Click += KryptonContextMenuItemGenericRowShowFavorite_Click;
            this.kryptonContextMenuItemGenericRowHideEqual.Click += KryptonContextMenuItemGenericRowHideEqual_Click;
            //this.kryptonContextMenuSeparatorGenericEndOfShowHideRows,
            this.kryptonContextMenuItemGenericTriStateOn.Click += KryptonContextMenuItemGenericTriStateOn_Click;
            this.kryptonContextMenuItemGenericTriStateOff.Click += KryptonContextMenuItemGenericTriStateOff_Click;
            this.kryptonContextMenuItemGenericTriStateToggle.Click += KryptonContextMenuItemGenericTriStateToggle_Click;
            //this.kryptonContextMenuSeparatorGenericEndOfTriState,
            this.kryptonContextMenuItemGenericMediaViewAsPoster.Click += KryptonContextMenuItemGenericMediaViewAsPoster_Click;
            this.kryptonContextMenuItemGenericMediaViewAsFull.Click += KryptonContextMenuItemGenericMediaViewAsFull_Click;
            //this.kryptonContextMenuSeparatorMap,
            this.kryptonContextMenuItemMapShowCoordinateOnOpenStreetMap.Click += KryptonContextMenuItemMapShowCoordinateOnOpenStreetMap_Click;
            this.kryptonContextMenuItemMapShowCoordinateOnGoogleMap.Click += KryptonContextMenuItemMapShowCoordinateOnGoogleMap_Click;
            this.kryptonContextMenuItemMapReloadUsingNominatim.Click += KryptonContextMenuItemMapReloadUsingNominatim_Click;
            this.kryptonContextMenuItemToolLocationAnalytics.Click += KryptonContextMenuItemToolLocationAnalytics_Click;

            //
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFilename.Click += KryptonContextMenuRadioButtonFileSystemColumnSortFilename_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate.Click += KryptonContextMenuRadioButtonFileSystemColumnSortSmarteDate_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileDate.Click += KryptonContextMenuRadioButtonFileSystemColumnSortFileDate_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate.Click += KryptonContextMenuRadioButtonFileSystemColumnSortFileCreateDate_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate.Click += KryptonContextMenuRadioButtonFileSystemColumnSortFileModifiedDate_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken.Click += KryptonContextMenuRadioButtonFileSystemColumnSortMediaDateTaken_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum.Click += KryptonContextMenuRadioButtonFileSystemColumnSortMediaAlbum_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle.Click += KryptonContextMenuRadioButtonFileSystemColumnSortMediaTitle_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription.Click += KryptonContextMenuRadioButtonFileSystemColumnSortMediaDescription_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaComments.Click += KryptonContextMenuRadioButtonFileSystemColumnSortMediaComments_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor.Click += KryptonContextMenuRadioButtonFileSystemColumnSortMediaAuthor_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortMediaRating.Click += KryptonContextMenuRadioButtonFileSystemColumnSortMediaRating_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationName.Click += KryptonContextMenuRadioButtonFileSystemColumnSortLocationName_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState.Click += KryptonContextMenuRadioButtonFileSystemColumnSortLocationRegionState_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCity.Click += KryptonContextMenuRadioButtonFileSystemColumnSortLocationCity_Click;
            this.kryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry.Click += KryptonContextMenuRadioButtonFileSystemColumnSortLocationCountry_Click;
            this.kryptonContextMenuItemFileSystemColumnSortClear.Click += KryptonContextMenuItemFileSystemColumnSortClear_Click;
            //this.kryptonContextMenuItemsCloseMenuList

            this.kryptonContextMenuRadioButtonSlideshow2sec.Click += KryptonContextMenuRadioButtonSlideshow2sec_Click;
            this.kryptonContextMenuRadioButtonSlideshow4sec.Click += KryptonContextMenuRadioButtonSlideshow4sec_Click;
            this.kryptonContextMenuRadioButtonSlideshow6sec.Click += KryptonContextMenuRadioButtonSlideshow6sec_Click;
            this.kryptonContextMenuRadioButtonSlideshow8sec.Click += KryptonContextMenuRadioButtonSlideshow8sec_Click;
            this.kryptonContextMenuRadioButtonSlideshow10sec.Click += KryptonContextMenuRadioButtonSlideshow10sec_Click;
            this.kryptonContextMenuItemPreviewSlideshowIntervalStop.Click += KryptonContextMenuItemPreviewSlideshowIntervalStop_Click;
            //this.kryptonContextMenuItemsPreviewSlideshowIntervalList.Items.AddRange(new Krypton.Toolkit.KryptonContextMenuItemBase[] {

            nameImageListView = this.imageListView1.Name;
            nameFolderTreeViewFolder = this.treeViewFolderBrowser1.Name;
            nameDataGridViewConvertAndMerge = this.dataGridViewConvertAndMerge.Name;
            nameDataGridViewDate = this.dataGridViewDate.Name;
            nameDataGridViewExifTool = this.dataGridViewExiftool.Name;
            nameDataGridViewExifToolWarning = this.dataGridViewExiftoolWarning.Name;
            nameDataGridViewMap = this.dataGridViewMap.Name;
            nameDataGridViewPeople = this.dataGridViewPeople.Name;
            nameDataGridViewProperties = this.dataGridViewProperties.Name;
            nameDataGridViewRename = this.dataGridViewRename.Name;
            nameDataGridViewTagsAndKeywords = this.dataGridViewTagsAndKeywords.Name;

            #endregion

            SetPreviewRibbonEnabledStatus(previewStartEnabled: false, enabled: false);

            #region Initialize VLC player
            FormSplash.UpdateStatus("Staring VLC player...");

            _libVLC = new LibVLC();
            videoView1.MediaPlayer = new MediaPlayer(_libVLC);
            #endregion 

            #region Loading ImageListView renderers
            FormSplash.UpdateStatus("Loading ImageListView renderers...");


            Manina.Windows.Forms.View listViewSelectedMode;

            KryptonContextMenuItems kryptonContextMenuItems = (KryptonContextMenuItems)kryptonContextMenuImageListViewModeThumbnailRenders.Items[0];
            kryptonRibbonGroupButtonImageListViewModeThumbnailRenders.KryptonContextMenu = kryptonContextMenuImageListViewModeThumbnailRenders;

            bool isRendererAssigned = false;
            bool isDefaultRendererAssigned = false;
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            foreach (Type t in assembly.GetTypes())
            {
                if (t.BaseType == typeof(Manina.Windows.Forms.ImageListView.ImageListViewRenderer))
                {
                    KryptonContextMenuItem kryptonContextMenuItem = new KryptonContextMenuItem();
                    kryptonContextMenuItem.Text = t.Name;
                    kryptonContextMenuItem.Tag = new RendererItem(t);
                    kryptonContextMenuItem.Click += KryptonContextMenuItemRenderers_Click;
                    kryptonContextMenuItems.Items.Add(kryptonContextMenuItem);

                    if (!isDefaultRendererAssigned || !isRendererAssigned) imageListViewSelectedRenderer = (RendererItem)kryptonContextMenuItem.Tag;
                    if (!isRendererAssigned && t.Name == "RendererDefault")
                    {
                        imageListViewSelectedRenderer = (RendererItem)kryptonContextMenuItem.Tag;
                        isDefaultRendererAssigned = true;
                    }

                    if (t.Name == Properties.Settings.Default.ImageListViewRendererName)
                    {
                        imageListViewSelectedRenderer = (RendererItem)kryptonContextMenuItem.Tag;
                        isRendererAssigned = true;
                    }
                }
            }

            listViewSelectedMode = (Manina.Windows.Forms.View)Properties.Settings.Default.ImageListViewViewMode;

            SetImageListViewRender(listViewSelectedMode, imageListViewSelectedRenderer);

            imageListView1.TitleLine1 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine1);
            imageListView1.TitleLine2 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine2);
            imageListView1.TitleLine3 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine3);
            imageListView1.TitleLine4 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine4);
            imageListView1.TitleLine5 = FormChooseColumns.GetColumnTypeByText(Properties.Settings.Default.ImageListViewTitleLine5);
            ColumnNamesAndWidthHandler.SetImageListViewCheckedValues(imageListView1, Properties.Settings.Default.ImageListViewSelectedColumns);
            ColumnNamesAndWidthHandler.SetColumnNameWithWidth(
                ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsImageListView),
                imageListView1);
            #endregion

            #region Initialize database connect
            FormSplash.UpdateStatus("Initialize database: connect...");
            try
            {
                databaseUtilitiesSqliteMetadata = new SqliteDatabaseUtilities(DatabaseType.SqliteMetadataDatabase, 10000, 5000);
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Was not able to start the database...", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
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
                threadCache.Priority = (ThreadPriority)Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity;
                threadCache.Start();
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
                //browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Disabled;
                browser.BrowserSettings.Plugins = CefState.Enabled;
                this.panelBrowser.Controls.Add(this.browser);
                browser.AddressChanged += this.OnBrowserAddressChanged;
            }
            catch (Exception ex)
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
            SetThumbnailSize(Properties.Settings.Default.ThumbmailViewSizeIndex);

            kryptonRibbonGroupButtonThumbnailSizeXLarge.ToolTipTitle = "Thumbnail size " + thumbnailSizes[4].Width + "x" + thumbnailSizes[4].Height;
            kryptonRibbonGroupButtonThumbnailSizeLarge.ToolTipTitle = "Thumbnail size " + thumbnailSizes[3].Width + "x" + thumbnailSizes[3].Height;
            kryptonRibbonGroupButtonThumbnailSizeMedium.ToolTipTitle = "Thumbnail size " + thumbnailSizes[2].Width + "x" + thumbnailSizes[2].Height;
            kryptonRibbonGroupButtonThumbnailSizeSmall.ToolTipTitle = "Thumbnail size " + thumbnailSizes[1].Width + "x" + thumbnailSizes[1].Height;
            kryptonRibbonGroupButtonThumbnailSizeXSmall.ToolTipTitle = "Thumbnail size " + thumbnailSizes[0].Width + "x" + thumbnailSizes[0].Height;
            #endregion

            #region Setup Global Variables - Link Tab and DataGridView
            //kryptonPageToolboxTags
            kryptonPageToolboxTags.Tag = LinkTabAndDataGridViewNameTags;
            GlobalData.dataGridViewHandlerTags = new DataGridViewHandler(dataGridViewTagsAndKeywords, (KryptonPalette)kryptonManager1.GlobalPalette, 
                LinkTabAndDataGridViewNameTags, "Metadata/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords);

            kryptonPageToolboxMap.Tag = LinkTabAndDataGridViewNameMap;
            GlobalData.dataGridViewHandlerMap = new DataGridViewHandler(dataGridViewMap, (KryptonPalette)kryptonManager1.GlobalPalette, 
                LinkTabAndDataGridViewNameMap, "Location/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeMap);

            kryptonPageToolboxPeople.Tag = LinkTabAndDataGridViewNamePeople;
            GlobalData.dataGridViewHandlerPeople = new DataGridViewHandler(dataGridViewPeople, (KryptonPalette)kryptonManager1.GlobalPalette, 
                LinkTabAndDataGridViewNamePeople, "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizePeoples);

            kryptonPageToolboxDates.Tag = LinkTabAndDataGridViewNameDates;
            GlobalData.dataGridViewHandlerDates = new DataGridViewHandler(dataGridViewDate, (KryptonPalette)kryptonManager1.GlobalPalette, 
                LinkTabAndDataGridViewNameDates, "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeDates);

            kryptonPageToolboxExiftool.Tag = LinkTabAndDataGridViewNameExiftool;
            GlobalData.dataGridViewHandlerExiftoolTags = new DataGridViewHandler(dataGridViewExiftool, (KryptonPalette)kryptonManager1.GlobalPalette, 
                LinkTabAndDataGridViewNameExiftool, "File/Tag Description", (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool);

            kryptonPageToolboxWarnings.Tag = LinkTabAndDataGridViewNameWarnings;
            GlobalData.dataGridViewHandlerExiftoolWarning = new DataGridViewHandler(dataGridViewExiftoolWarning, (KryptonPalette)kryptonManager1.GlobalPalette, 
                LinkTabAndDataGridViewNameWarnings, "File and version/Tag region and command", (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings);

            kryptonPageToolboxProperties.Tag = LinkTabAndDataGridViewNameProperties;
            GlobalData.dataGridViewHandlerProperties = new DataGridViewHandler(dataGridViewProperties, (KryptonPalette)kryptonManager1.GlobalPalette, 
                LinkTabAndDataGridViewNameProperties, "File/Properties", (DataGridViewSize)Properties.Settings.Default.CellSizeProperties);

            kryptonPageToolboxRename.Tag = LinkTabAndDataGridViewNameRename;
            GlobalData.dataGridViewHandlerRename = new DataGridViewHandler(dataGridViewRename, (KryptonPalette)kryptonManager1.GlobalPalette, 
                LinkTabAndDataGridViewNameRename, "Filename/Values", ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize),
                    ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsRenameLarge),
                    ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsRenameMedium),
                    ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsRenameSmall)
                );

            kryptonPageToolboxConvertAndMerge.Tag = LinkTabAndDataGridViewNameConvertAndMerge;
            GlobalData.dataGridViewHandlerConvertAndMerge = new DataGridViewHandler(dataGridViewConvertAndMerge, (KryptonPalette)kryptonManager1.GlobalPalette, 
                LinkTabAndDataGridViewNameConvertAndMerge, "Full path of media file", 
                ((DataGridViewSize)Properties.Settings.Default.CellSizeConvertAndMerge | DataGridViewSize.RenameConvertAndMergeSize),
                    ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsConvertAndMergeLarge),
                    ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsConvertAndMergeMedium),
                    ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsConvertAndMergeSmall)
                );
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

            kryptonWorkspaceCellFolderSearchFilter.StarSize = Properties.Settings.Default.WorkspaceCellFolderSearchFilterStarSize; //"313*,50*"
            kryptonWorkspaceCellMediaFiles.StarSize = Properties.Settings.Default.WorkspaceCellMediaFilesStarSize; //"367*,50*"
            kryptonWorkspaceCellToolbox.StarSize = Properties.Settings.Default.WorkspaceCellToolboxStarSize; //"674*,50*"
            kryptonWorkspaceCellToolboxMapBroswer.StarSize = Properties.Settings.Default.WorkspaceCellToolboxMapBroswerStarSize; //"50*,211*"
            kryptonWorkspaceCellToolboxMapBroswerProperties.StarSize = Properties.Settings.Default.WorkspaceCellToolboxMapBroswerPropertiesStarSize; //"50*,35"
            kryptonWorkspaceCellToolboxMapDetails.StarSize = Properties.Settings.Default.WorkspaceCellToolboxMapDetailsStarSize; //"50*,497*"
            kryptonWorkspaceCellToolboxMapProperties.StarSize = Properties.Settings.Default.WorkspaceCellToolboxMapPropertiesStarSize; //"50*,29"
            kryptonWorkspaceCellToolboxRenameResult.StarSize = Properties.Settings.Default.WorkspaceCellToolboxRenameResultStarSize; //"50*,650*"
            kryptonWorkspaceCellToolboxRenameVariables.StarSize = Properties.Settings.Default.WorkspaceCellToolboxRenameVariablesStarSize; //"50*,132"
            kryptonWorkspaceCellToolboxTagsDetails.StarSize = Properties.Settings.Default.WorkspaceCellToolboxTagsDetailsStarSize; //"50*,272*"
            kryptonWorkspaceCellToolboxTagsKeywords.StarSize = Properties.Settings.Default.WorkspaceCellToolboxTagsKeywordsStarSize; //"50*,510*"
            kryptonWorkspaceCellFolderSearchFilter.NavigatorMode = (Krypton.Navigator.NavigatorMode)Properties.Settings.Default.WorkspaceCellFolderSearchFilterNavigatorMode;
            #endregion

            this.SuspendLayout();

            #region Initialize layout setup - Show/Hide error and History            
            SetRibbonGridViewColumnsButtonsHistoricalAndError(Properties.Settings.Default.ShowHistortyColumns, Properties.Settings.Default.ShowErrorColumns);
            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(kryptonRibbonGroupButtonDataGridViewColumnsHistory.Checked, kryptonRibbonGroupButtonDataGridViewColumnsErrors.Checked);
            #endregion

            timerShowErrorMessage.Enabled = true;

            #region Initialize layout setup - Initialize layout toolstrip: Exiftool
            FormSplash.UpdateStatus("Initialize layout toolstrip: Exiftool & ExiftoolWarnings...");
            PopulateExiftoolToolStripMenuItems();
            #endregion 

            #region Initialize layout setup - Initialize layout toolstrip: People
            FormSplash.UpdateStatus("Initialize layout toolstrip: People...");
            PopulatePeopleToolStripMenuItems();
            #endregion

            #region Initialize layout - Sort Order - ImageListView
            try
            {
                SetImageListViewSortByRadioButton(imageListView1, (ColumnType)Properties.Settings.Default.ImageListViewSortingColumn, (SortOrder)Properties.Settings.Default.ImageListViewSortingOrder);
                ImageListViewSortByCheckedRadioButton(false);
            }
            catch { }
            #endregion

            this.ResumeLayout();
            #endregion

            #region OneDriveNetworkNames - for automatic remove
            if (!oneDriveNetworkNames.Contains(Environment.MachineName)) oneDriveNetworkNames.Add(Environment.MachineName);
            try
            {
                Thread scanForComputers = new Thread(() =>
                {
                    try
                    {
                        Trinet.Networking.NetworkCompuersAndSharesHandler networkCompuersAndSharesHandler = new Trinet.Networking.NetworkCompuersAndSharesHandler();

                        List<string> listOfNetworkNames = new List<string>();
                        if (!listOfNetworkNames.Contains(Environment.MachineName)) listOfNetworkNames.Add(Environment.MachineName);

                        networkCompuersAndSharesHandler.ScanForComputers();
                        foreach (string computerName in networkCompuersAndSharesHandler.ComputerNames)
                        {
                            if (!listOfNetworkNames.Contains(computerName)) listOfNetworkNames.Add(computerName);
                        }
                        oneDriveNetworkNames = listOfNetworkNames; //Swap to new list;
                    }
                    catch { }
                });
                scanForComputers.Priority = (ThreadPriority)Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity;
                scanForComputers.Start();
            }
            catch { }
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
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                });
                _ThreadHttp.Priority = (ThreadPriority)Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity;
                _ThreadHttp.Start();                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            #endregion

            MaximizeOrRestoreWorkspaceMainCellAndChilds();
        }

        
        #endregion

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
        private bool isClosing = false;
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (commonQueueSaveMetadataUpdatedByUser.Count > 0 || IsAnyDataUnsaved())
            {
                if (KryptonMessageBox.Show(
                    "There are " + commonQueueSaveMetadataUpdatedByUser.Count + " unsaved media files in queue.\r\n" +
                    (IsAnyDataUnsaved() ? "You have unsaved changes in DataGridView\r\n" : "") +
                    "Are you sure you will close application?",
                    "Press Ok will quit application and changed will get lost.\r\n" +
                    "Press Cancel and return back to application.", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, true) == DialogResult.Cancel)
                {
                    isClosing = false;
                    e.Cancel = true;
                    return;
                } else
                {
                    ImageListViewClearAll(imageListView1);
                    OnImageListViewSelect_FilesSelectedOrNoneSelected(true); //Even when 0 selected files, allocate data and flags, etc...
                }
            }

            if (!isClosing)
            {
                isClosing = true;
            
                try
                {
                    exiftoolReader.MetadataGroupPrioritiesWrite(); //Updated json config file if new tags found
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Can't save settings, Metadata Group Priorities", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
                }

                try
                {
                    try
                    {
                        browser.Dispose();
                    }
                    catch { }

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

                        kryptonWorkspaceCellToolboxTagsDetails.HideAllPages();
                        Properties.Settings.Default.WorkspaceCellMediaFilesStarSize = kryptonWorkspaceCellMediaFiles.StarSize; //"367*,50*"
                        Properties.Settings.Default.WorkspaceCellToolboxStarSize = kryptonWorkspaceCellToolbox.StarSize; //"674*,50*"
                        Properties.Settings.Default.WorkspaceCellToolboxMapBroswerStarSize = kryptonWorkspaceCellToolboxMapBroswer.StarSize; //"50*,211*"
                        Properties.Settings.Default.WorkspaceCellToolboxMapBroswerPropertiesStarSize = kryptonWorkspaceCellToolboxMapBroswerProperties.StarSize; //"50*,35"
                        Properties.Settings.Default.WorkspaceCellToolboxMapDetailsStarSize = kryptonWorkspaceCellToolboxMapDetails.StarSize; //"50*,497*"
                        Properties.Settings.Default.WorkspaceCellToolboxMapPropertiesStarSize = kryptonWorkspaceCellToolboxMapProperties.StarSize; //"50*,29"
                        Properties.Settings.Default.WorkspaceCellToolboxRenameResultStarSize = kryptonWorkspaceCellToolboxRenameResult.StarSize; //"50*,650*"
                        Properties.Settings.Default.WorkspaceCellToolboxRenameVariablesStarSize = kryptonWorkspaceCellToolboxRenameVariables.StarSize; //"50*,132"
                        Properties.Settings.Default.WorkspaceCellToolboxTagsDetailsStarSize = kryptonWorkspaceCellToolboxTagsDetails.StarSize; //"50*,272*"
                        Properties.Settings.Default.WorkspaceCellToolboxTagsKeywordsStarSize = kryptonWorkspaceCellToolboxTagsKeywords.StarSize; //"50*,510*"
                        Properties.Settings.Default.WorkspaceCellFolderSearchFilterNavigatorMode = (int)kryptonWorkspaceCellFolderSearchFilter.NavigatorMode;
                        if (kryptonWorkspaceCellFolderSearchFilter.NavigatorMode != NavigatorMode.OutlookMini)
                            Properties.Settings.Default.WorkspaceCellFolderSearchFilterStarSize = kryptonWorkspaceCellFolderSearchFilter.StarSize; //"313*,50*"

                    }
                    catch { }

                    try
                    {
                        Properties.Settings.Default.Save();
                    }
                    catch (Exception ex)
                    {
                        KryptonMessageBox.Show(ex.Message, "Can't save settings", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
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
                    }
                    catch { }

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
                    KryptonMessageBox.Show(ex.Message, "Problems during close all threads and other process during closing application", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
                }
            }
            isClosing = false;
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

                try
                {

                    Raccoom.Windows.Forms.TreeStrategyShell32Provider shell32Provider = new Raccoom.Windows.Forms.TreeStrategyShell32Provider();
                    shell32Provider.EnableContextMenu = true;
                    shell32Provider.ShowAllShellObjects = true;
                    treeViewFolderBrowser1.DataSource = shell32Provider; // new Raccoom.Windows.Forms.TreeStrategyFolderBrowserProvider();
                    string folder = Properties.Settings.Default.LastFolder;

                    treeViewFolderBrowser1.Populate(folder);
                    if (treeViewFolderBrowser1.SelectedNode == null && treeViewFolderBrowser1.Nodes.Count >= 1)
                    {
                        filesCutCopyPasteDrag.TreeViewFolderBrowserRefreshTreeNode(treeViewFolderBrowser1, treeViewFolderBrowser1.Nodes[0]);
                    }
                    
                }
                catch (Exception ee)
                {
                    KryptonMessageBox.Show(Application.ProductName + "\r\n\r\n" + ee.Message, "Initialize folder tree failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
                }

                GlobalData.IsPopulatingFolderTree = false;
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Initialize folder tree failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
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
                KryptonMessageBox.Show(ex.Message, "Populate search failed...", MessageBoxButtons.OK, MessageBoxIcon.Error, true);
            }
            #endregion

            ImageListView_Aggregate_FromFolder(false, true);
            OnImageListViewSelect_FilesSelectedOrNoneSelected(false);

            MaximizeOrRestoreWorkspaceMainCellAndChilds();
            SetNavigatorModeSearch((NavigatorMode)Properties.Settings.Default.WorkspaceCellFolderSearchFilterNavigatorMode);
        }
        #endregion

        #region SetNavigatorModeSearch
        private void SetNavigatorModeSearch(NavigatorMode navigatorMode)
        {
            kryptonWorkspaceMain.SuspendWorkspaceLayout();
            kryptonWorkspaceCellFolderSearchFilter.SuspendLayout();
            kryptonWorkspaceCellMediaFiles.SuspendLayout();
            kryptonWorkspaceCellToolbox.SuspendLayout();

            if (navigatorMode == NavigatorMode.OutlookMini)
            {
                kryptonWorkspaceCellFolderSearchFilter.Button.ButtonSpecs[0].Visible = false;

                Size newSize = new System.Drawing.Size(400, Math.Max(200, this.Size.Height - 200));                
                this.kryptonPageFolderSearchFilterFolder.MinimumSize = newSize;
                this.kryptonPageFolderSearchFilterSearch.MinimumSize = newSize;
                this.kryptonPageFolderSearchFilterFilter.MinimumSize = newSize;

                Properties.Settings.Default.WorkspaceCellFolderSearchFilterStarSize = kryptonWorkspaceCellFolderSearchFilter.StarSize; //"313*,50*"

                kryptonWorkspaceCellFolderSearchFilter.NavigatorMode = NavigatorMode.OutlookMini;
                kryptonWorkspaceCellFolderSearchFilter.StarSize = kryptonWorkspaceCellFolderSearchFilter.PreferredSize.Width + ",50*";
                buttonSpecNavigatorExpandCollapse.TypeRestricted = PaletteNavButtonSpecStyle.ArrowRight;
            }
            else
            {
                kryptonWorkspaceCellFolderSearchFilter.Button.ButtonSpecs[0].Visible = true;

                this.kryptonPageFolderSearchFilterFolder.MinimumSize = new System.Drawing.Size(50, 50);
                this.kryptonPageFolderSearchFilterSearch.MinimumSize = new System.Drawing.Size(50, 50);
                this.kryptonPageFolderSearchFilterFilter.MinimumSize = new System.Drawing.Size(50, 50);

                kryptonWorkspaceCellFolderSearchFilter.NavigatorMode = NavigatorMode.OutlookFull;
                kryptonWorkspaceCellFolderSearchFilter.StarSize = Properties.Settings.Default.WorkspaceCellFolderSearchFilterStarSize; //eg. "313*,50*"
                buttonSpecNavigatorExpandCollapse.TypeRestricted = PaletteNavButtonSpecStyle.ArrowLeft;
            }

            kryptonWorkspaceMain.ResumeWorkspaceLayout();
            kryptonWorkspaceCellFolderSearchFilter.ResumeLayout();
            kryptonWorkspaceCellMediaFiles.ResumeLayout();
            kryptonWorkspaceCellToolbox.ResumeLayout();

            
        }

        private void buttonSpecNavigatorExpandCollapse_Click(object sender, EventArgs e)
        {
            if (kryptonWorkspaceCellFolderSearchFilter.NavigatorMode == NavigatorMode.OutlookFull)
                SetNavigatorModeSearch(NavigatorMode.OutlookMini);
            else
                SetNavigatorModeSearch(NavigatorMode.OutlookFull);

        }

        private void kryptonPageFolderSearchFilterSearch_Resize(object sender, EventArgs e)
        {
            tableLayoutPanelSerachSearch.Width = Math.Max(kryptonPageFolderSearchFilterSearch.Width - 25, tableLayoutPanelSearchKeywords.MinimumSize.Width + 5);
            tableLayoutPanelSerachActions.Width = tableLayoutPanelSerachSearch.Width;
        }

        private void kryptonPageToolboxTagsDetails_Resize(object sender, EventArgs e)
        {
            tableLayoutPanelTags.Width = Math.Max(kryptonPageToolboxTagsDetails.Width - 25, tableLayoutPanelTags.MinimumSize.Width);
        }

        private void RefreshHackSearchFilter()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(RefreshHackSearchFilter));
                return;
            }

            kryptonPageSearchFilterAction.Invalidate();
            kryptonPageSearchFilterAction.Refresh();

        }

        private void kryptonWorkspaceCellFolderSearchFilter_DisplayPopupPage(object sender, PopupPageEventArgs e)
        {
            Task.Delay(250).ContinueWith(t => RefreshHackSearchFilter());
            Task.Delay(500).ContinueWith(t => RefreshHackSearchFilter());
            Task.Delay(100).ContinueWith(t => RefreshHackSearchFilter());
        }










































        #endregion

        private void dataGridViewTagsAndKeywords_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex));
        }

        private void dataGridViewConvertAndMerge_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex));
        }

        private void dataGridViewRename_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex));
        }

        private void dataGridViewProperties_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex));
        }

        private void dataGridViewExiftoolWarning_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex));
        }

        private void dataGridViewExiftool_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex));
        }

        private void dataGridViewDate_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex));
        }

        private void dataGridViewMap_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex));
        }

        private void dataGridViewPeople_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            DataGridViewHandler.SetColumnDirtyFlag(dataGridView, e.ColumnIndex, IsDataGridViewColumnDirty(dataGridView, e.ColumnIndex));
        }
    }

}



