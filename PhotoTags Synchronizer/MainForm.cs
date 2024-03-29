﻿using System;
using System.Drawing;
using System.Windows.Forms;
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
using System.Threading.Tasks;
using System.Collections.Generic;
using Krypton.Toolkit;
using Krypton.Navigator;
using FileDateTime;
using ColumnNamesAndWidth;
using System.IO;
using NLog;
using SharpKml.Dom.GX;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;
using SharpKml.Dom.Atom;
using SharpKml.Dom;
using System.Net.Sockets;
using GoogleCast;
using System.Runtime.Remoting.Messaging;
using CefSharp.DevTools.CacheStorage;
using ApplicationAssociations;

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

        private System.Windows.Forms.ProgressBar progressBarBackground = new System.Windows.Forms.ProgressBar();
        private System.Windows.Forms.ProgressBar progressBarSaveConvert = new System.Windows.Forms.ProgressBar();
        private System.Windows.Forms.ProgressBar progressBarLazyLoading = new System.Windows.Forms.ProgressBar();

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
        private ThumbnailPosterDatabaseCache databaseAndCacheThumbnailPoster;
        private CameraOwnersDatabaseCache databaseAndCahceCameraOwner;

        //Exif from media file
        private MetadataDatabaseCache databaseAndCacheMetadataExiftool;
        private MetadataDatabaseCache databaseAndCacheMetadataWindowsLivePhotoGallery;
        private MetadataDatabaseCache databaseAndCacheMetadataMicrosoftPhotos;

        private LocationNameDatabaseAndLookUpCache databaseLocationNameAndLookUp;

        private ExiftoolReader exiftoolReader;
        private ExiftoolDataDatabase databaseExiftoolData;
        private ExiftoolWarningDatabase databaseExiftoolWarning;

        private MicrosoftPhotosReader databaseMicrosoftPhotos;
        private WindowsLivePhotoGalleryDatabasePipe databaseWindowsLivePhotGallery;

        private FilesCutCopyPasteDrag filesCutCopyPasteDrag;

        private List<AutoKeywordConvertion> autoKeywordConvertions = new List<AutoKeywordConvertion>();

        private FileDateTimeReader fileDateTimeReader = null;
        //Cache level
        private int cacheNumberOfPosters;
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
            try
            {
                #region Initialize VLC player
                FormSplash.UpdateStatus("Initialize VLC player...");
                try
                {
                    if (!DesignMode) Core.Initialize();
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Was not able to load VLC player", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                    return;
                }
                #endregion

                #region Initialize components
                FormSplash.UpdateStatus("Initialize components...");
                InitializeComponent();
                //this.Refresh();
                //CalculatePanelSize(this);
                
                #endregion

                #region InitializeComponent - Krypton

                fileDateTimeReader = new FileDateTimeReader(Properties.Settings.Default.RenameDateFormats);

                KryptonCustomPaletteBase KryptonCustomPaletteBase = KryptonCustomPaletteBaseHandler.Load(Properties.Settings.Default.KryptonCustomPaletteBaseFullFilename, Properties.Settings.Default.KryptonCustomPaletteBaseName);
                KryptonCustomPaletteBaseHandler.SetPalette(this, kryptonManager1, KryptonCustomPaletteBase, KryptonCustomPaletteBaseHandler.IsSystemPalette, Properties.Settings.Default.KryptonCustomPaletteBaseDropShadow);
                KryptonCustomPaletteBaseHandler.SetImageListViewPalettes(kryptonManager1, imageListView1);

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
                this.kryptonContextMenuItemGenericRegionRename4.Click += KryptonContextMenuItemGenericRegionRenameGeneric_Click;
                this.kryptonContextMenuItemGenericRegionRename5.Click += KryptonContextMenuItemGenericRegionRenameGeneric_Click;
                //this.kryptonContextMenuItemGenericRegionRenameMostUsed
                //this.kryptonContextMenuItemGenericRegionRenameMostUsedList

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
                this.kryptonContextMenuItemMapSaveExactLocation.Click += KryptonContextMenuItemMapSaveExactLocation_Click;

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

                comboBoxRenameVariableList.Items.Clear();
                comboBoxRenameVariableList.Items.AddRange(DataGridViewHandlerRename.ListOfRenameVariables);
                #endregion

                SetPreviewRibbonEnabledStatus(previewStartEnabled: false, enabled: false);


                #region Initialize VLC player
                FormSplash.UpdateStatus("Staring VLC player...");
                try
                {
                    _libVLC = new LibVLC();
                    videoView1.MediaPlayer = new MediaPlayer(_libVLC);
                }
                catch
                {
                    _libVLC = null;
                    videoView1 = null;
                    FormSplash.UpdateStatus("Staring VLC player failed...");
                    Thread.Sleep(5000);
                }
                #endregion

                FileHandeling.FileHandler.SetLocalApplicationDataPath(Properties.Settings.Default.TempDataFolder);
                Properties.Settings.Default.TempDataFolder = FileHandeling.FileHandler.GetLocalApplicationDataPath("");

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
                    databaseUtilitiesSqliteMetadata = new SqliteDatabaseUtilities(DatabaseType.SqliteMetadataDatabase, FileHandeling.FileHandler.GetLocalApplicationDataPath(""));
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Was not able to start the database...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                    Close();
                    return;
                }
                databaseGoogleLocationHistory = new GoogleLocationHistoryDatabaseCache(databaseUtilitiesSqliteMetadata);
                databaseAndCacheMetadataExiftool = new MetadataDatabaseCache(databaseUtilitiesSqliteMetadata);
                databaseAndCacheMetadataExiftool.AllowedDateFormats = Properties.Settings.Default.RenameDateFormats;
                databaseAndCacheMetadataExiftool.OnRecordReadToCache += DatabaseAndCacheMetadataExiftool_OnRecordReadToCache;
                databaseAndCacheMetadataExiftool.OnDeleteRecord += DatabaseAndCacheMetadataExiftool_OnDeleteRecord;

                databaseAndCacheThumbnailPoster = new ThumbnailPosterDatabaseCache(databaseUtilitiesSqliteMetadata);
                databaseAndCacheThumbnailPoster.UpsizeThumbnailSize = ThumbnailMaxUpsize;

                databaseExiftoolData = new ExiftoolDataDatabase(databaseUtilitiesSqliteMetadata);
                databaseExiftoolWarning = new ExiftoolWarningDatabase(databaseUtilitiesSqliteMetadata);

                databaseAndCahceCameraOwner = new CameraOwnersDatabaseCache(databaseUtilitiesSqliteMetadata);
                databaseLocationNameAndLookUp = new LocationNameDatabaseAndLookUpCache(databaseUtilitiesSqliteMetadata, Properties.Settings.Default.ApplicationPreferredLanguages);

                //databaseUtilitiesSqliteWindowsLivePhotoGallery = new SqliteDatabaseUtilities(DatabaseType.SqliteWindowsLivePhotoGallaryCache);
                //databaseAndCacheMetadataWindowsLivePhotoGallery = new MetadataDatabaseCache(databaseUtilitiesSqliteWindowsLivePhotoGallery);
                databaseAndCacheMetadataWindowsLivePhotoGallery = new MetadataDatabaseCache(databaseUtilitiesSqliteMetadata);
                databaseAndCacheMetadataWindowsLivePhotoGallery.AllowedDateFormats = Properties.Settings.Default.RenameDateFormats;
                databaseAndCacheMetadataWindowsLivePhotoGallery.OnRecordReadToCache += DatabaseAndCacheMetadataExiftool_OnRecordReadToCache;
                databaseAndCacheMetadataWindowsLivePhotoGallery.OnDeleteRecord += DatabaseAndCacheMetadataExiftool_OnDeleteRecord;

                //databaseUtilitiesSqliteMicrosoftPhotos = new SqliteDatabaseUtilities(DatabaseType.SqliteMicrosoftPhotosCache);
                //databaseAndCacheMetadataMicrosoftPhotos = new MetadataDatabaseCache(databaseUtilitiesSqliteMicrosoftPhotos);
                databaseAndCacheMetadataMicrosoftPhotos = new MetadataDatabaseCache(databaseUtilitiesSqliteMetadata);
                databaseAndCacheMetadataMicrosoftPhotos.AllowedDateFormats = Properties.Settings.Default.RenameDateFormats;
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

                        if (cacheAllThumbnails) databaseAndCacheThumbnailPoster.ReadToCacheFolder(null);
                        if (cacheAllMetadatas) databaseAndCacheMetadataExiftool.ReadToCacheAllMetadatas();
                        if (cacheAllWebScraperDataSets) databaseAndCacheMetadataExiftool.ReadToCacheWebScarpingAllDataSets();
                    });
                    threadCache.Priority = (ThreadPriority)Properties.Settings.Default.ApplicationDebugBackgroundThreadPrioity;
                    threadCache.Start();
                }
                catch { }
                #endregion

                filesCutCopyPasteDrag = new FilesCutCopyPasteDrag(databaseAndCacheMetadataExiftool, databaseAndCacheMetadataWindowsLivePhotoGallery,
                    databaseAndCacheMetadataMicrosoftPhotos, databaseAndCacheThumbnailPoster, databaseExiftoolData, databaseExiftoolWarning);
                filesCutCopyPasteDrag.OnFileSystemAction += FilesCutCopyPasteDrag_OnFileSystemAction;

                #region Connect to Microsoft Photos
                FormSplash.UpdateStatus("Initialize database: Connect to Microsoft Photos...");
                try
                {
                    if (!File.Exists(SqliteDatabaseUtilities.GetMicrosoftPhotosDatabaseOriginalFile()) &&
                    !File.Exists(SqliteDatabaseUtilities.GetMicrosoftPhotosDatabaseBackupFile(
                        FileHandeling.FileHandler.GetLocalApplicationDataPath("")))) GlobalData.doesMircosoftPhotosExists = false;

                    if (GlobalData.doesMircosoftPhotosExists) databaseMicrosoftPhotos = new MicrosoftPhotosReader(FileHandeling.FileHandler.GetLocalApplicationDataPath(""));
                    if (!File.Exists(SqliteDatabaseUtilities.GetMicrosoftPhotosDatabaseBackupFile(FileHandeling.FileHandler.GetLocalApplicationDataPath("")))) GlobalData.doesMircosoftPhotosExists = false;

                    try
                    {
                        GlobalData.doesMircosoftPhotosHaveData = true;
                        if (GlobalData.doesMircosoftPhotosExists) databaseMicrosoftPhotos.Read(MetadataBrokerType.MicrosoftPhotos, "Check if contains data");
                    }
                    catch
                    {
                        GlobalData.doesMircosoftPhotosHaveData = false;
                        GlobalData.doesMircosoftPhotosExists = false; 
                    }

                }
                catch (Exception e)
                {
                    GlobalData.doesMircosoftPhotosExists = false;
                    FormSplash.AddWarning("Windows photo warning:\r\n" + e.Message + "\r\n");
                    databaseMicrosoftPhotos = null;
                }
                #endregion

                #region Connect to Windows Live Photo Gallery
                FormSplash.UpdateStatus("Initialize database: Connect to Windows Live Photo Gallery...");
                try
                {
                    if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft\\Windows Live Photo Gallery\\Pictures.pd6")))
                        GlobalData.doesWindowsLivePhotoGalleryExists = false;

                    if (GlobalData.doesWindowsLivePhotoGalleryExists)
                    {
                        databaseWindowsLivePhotGallery = new WindowsLivePhotoGalleryDatabasePipe();
                        databaseWindowsLivePhotGallery.ConnectDatabase(databaseAndCacheMetadataWindowsLivePhotoGallery);
                    }
                }
                catch (Exception e)
                {
                    GlobalData.doesWindowsLivePhotoGalleryExists = false;
                    FormSplash.AddWarning("Windows Live Photo Gallery warning:\r\n" + e.Message + "\r\n");
                    databaseWindowsLivePhotGallery = null;
                }
                #endregion

                #region Configure ChromiumWebBrowser
                if (!GlobalData.isRunningWinSmode)
                {
                    try
                    {
                        FormSplash.UpdateStatus("Configure ChromiumWebBrowser...");
                        browser = new ChromiumWebBrowser("https://www.openstreetmap.org/");
                        //{
                        //    Dock = DockStyle.Fill,
                        //};
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "Cef Browser");
                    }

                    try
                    {
                        browser.BrowserSettings.Javascript = (Properties.Settings.Default.BrowserSettingsJavaScript ? CefState.Enabled : CefState.Disabled);
                        //browser.BrowserSettings.WebSecurity = CefState.Enabled;
                        browser.BrowserSettings.WebGl = (Properties.Settings.Default.BrowserSettingsEnableMediaStream ? CefState.Enabled : CefState.Disabled);
                        //browser.BrowserSettings.UniversalAccessFromFileUrls = CefState.Disabled;
                        //browser.BrowserSettings.Plugins = CefState.Enabled;
                        this.panelBrowser.Controls.Add(this.browser);
                        browser.AddressChanged += this.OnBrowserAddressChanged;
                    }
                    catch (Exception ex)
                    {
                        browser = null;

                        Logger.Error(ex, "Cef Browser");
                    }
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

                kryptonRibbonGroupButtonThumbnailSizeXLarge.ToolTipValues.Heading = "Thumbnail size " + thumbnailSizes[4].Width + "x" + thumbnailSizes[4].Height;
                kryptonRibbonGroupButtonThumbnailSizeLarge.ToolTipValues.Heading = "Thumbnail size " + thumbnailSizes[3].Width + "x" + thumbnailSizes[3].Height;
                kryptonRibbonGroupButtonThumbnailSizeMedium.ToolTipValues.Heading = "Thumbnail size " + thumbnailSizes[2].Width + "x" + thumbnailSizes[2].Height;
                kryptonRibbonGroupButtonThumbnailSizeSmall.ToolTipValues.Heading = "Thumbnail size " + thumbnailSizes[1].Width + "x" + thumbnailSizes[1].Height;
                kryptonRibbonGroupButtonThumbnailSizeXSmall.ToolTipValues.Heading = "Thumbnail size " + thumbnailSizes[0].Width + "x" + thumbnailSizes[0].Height;
                #endregion

                #region Setup Global Variables - Link Tab and DataGridView
                //kryptonPageToolboxTags
                kryptonPageToolboxTags.Tag = LinkTabAndDataGridViewNameTags;
                GlobalData.dataGridViewHandlerTags = new DataGridViewHandler(dataGridViewTagsAndKeywords, (KryptonCustomPaletteBase)kryptonManager1.GlobalPalette,
                    LinkTabAndDataGridViewNameTags, "Metadata/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeKeywords, allowUserToAddRow: true);

                kryptonPageToolboxMap.Tag = LinkTabAndDataGridViewNameMap;
                GlobalData.dataGridViewHandlerMap = new DataGridViewHandler(dataGridViewMap, (KryptonCustomPaletteBase)kryptonManager1.GlobalPalette,
                    LinkTabAndDataGridViewNameMap, "Location/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeMap, allowUserToAddRow: false);

                kryptonPageToolboxPeople.Tag = LinkTabAndDataGridViewNamePeople;
                GlobalData.dataGridViewHandlerPeople = new DataGridViewHandler(dataGridViewPeople, (KryptonCustomPaletteBase)kryptonManager1.GlobalPalette,
                    LinkTabAndDataGridViewNamePeople, "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizePeoples, allowUserToAddRow: true);

                kryptonPageToolboxDates.Tag = LinkTabAndDataGridViewNameDates;
                GlobalData.dataGridViewHandlerDates = new DataGridViewHandler(dataGridViewDate, (KryptonCustomPaletteBase)kryptonManager1.GlobalPalette,
                    LinkTabAndDataGridViewNameDates, "Name/Files", (DataGridViewSize)Properties.Settings.Default.CellSizeDates, allowUserToAddRow: false);

                kryptonPageToolboxExiftool.Tag = LinkTabAndDataGridViewNameExiftool;
                GlobalData.dataGridViewHandlerExiftoolTags = new DataGridViewHandler(dataGridViewExiftool, (KryptonCustomPaletteBase)kryptonManager1.GlobalPalette,
                    LinkTabAndDataGridViewNameExiftool, "File/Tag Description", (DataGridViewSize)Properties.Settings.Default.CellSizeExiftool, allowUserToAddRow: false);

                kryptonPageToolboxWarnings.Tag = LinkTabAndDataGridViewNameWarnings;
                GlobalData.dataGridViewHandlerExiftoolWarning = new DataGridViewHandler(dataGridViewExiftoolWarning, (KryptonCustomPaletteBase)kryptonManager1.GlobalPalette,
                    LinkTabAndDataGridViewNameWarnings, "File and version/Tag region and command", (DataGridViewSize)Properties.Settings.Default.CellSizeWarnings, allowUserToAddRow: false);

                kryptonPageToolboxProperties.Tag = LinkTabAndDataGridViewNameProperties;
                GlobalData.dataGridViewHandlerProperties = new DataGridViewHandler(dataGridViewProperties, (KryptonCustomPaletteBase)kryptonManager1.GlobalPalette,
                    LinkTabAndDataGridViewNameProperties, "File/Properties", (DataGridViewSize)Properties.Settings.Default.CellSizeProperties, allowUserToAddRow: false);

                kryptonPageToolboxRename.Tag = LinkTabAndDataGridViewNameRename;
                GlobalData.dataGridViewHandlerRename = new DataGridViewHandler(dataGridViewRename, (KryptonCustomPaletteBase)kryptonManager1.GlobalPalette,
                    LinkTabAndDataGridViewNameRename, "Filename/Values", ((DataGridViewSize)Properties.Settings.Default.CellSizeRename | DataGridViewSize.RenameConvertAndMergeSize),
                        ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsRenameLarge),
                        ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsRenameMedium),
                        ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsRenameSmall), allowUserToAddRow: false
                    );

                kryptonPageToolboxConvertAndMerge.Tag = LinkTabAndDataGridViewNameConvertAndMerge;
                GlobalData.dataGridViewHandlerConvertAndMerge = new DataGridViewHandler(dataGridViewConvertAndMerge, (KryptonCustomPaletteBase)kryptonManager1.GlobalPalette,
                    LinkTabAndDataGridViewNameConvertAndMerge, "Full path of media file",
                    ((DataGridViewSize)Properties.Settings.Default.CellSizeConvertAndMerge | DataGridViewSize.RenameConvertAndMergeSize),
                        ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsConvertAndMergeLarge),
                        ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsConvertAndMergeMedium),
                        ColumnNamesAndWidthHandler.ConvertConfigStringToColumnNameAndWidths(Properties.Settings.Default.ColumnNameAndWithsConvertAndMergeSmall), allowUserToAddRow: false
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
                ThumbnailRegionHandler.FaceThumbnailSize = Properties.Settings.Default.ApplicationRegionThumbnail;
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
                PopulatePeopleToolStripMenuItems(null,
                    Properties.Settings.Default.SuggestRegionNameNearbyDays,
                    Properties.Settings.Default.SuggestRegionNameNearByContextMenuCount,
                    Properties.Settings.Default.SuggestRegionNameMostUsedContextMenuCount,
                    Properties.Settings.Default.ApplicationSizeOfRegionNamesGroup,
                    Properties.Settings.Default.RenameDateFormats);
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
                
                CalculatePanelSize(this, 115);

                #endregion

                #region OneDriveNetworkNames - for automatic remove

                #region Get stored NetworkNames - Incase of thread fails
                string[] networkNamesFromConfig =
                        FormConfig.ConvertStringWithSepeartorToArray(Properties.Settings.Default.OneDriveDuplicatesNetworkNames);
                foreach (string networkName in networkNamesFromConfig) if (!oneDriveNetworkNames.Contains(networkName)) oneDriveNetworkNames.Add(networkName);
                #endregion

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

                            string[] networkNamesFromConfigThread =
                                FormConfig.ConvertStringWithSepeartorToArray(Properties.Settings.Default.OneDriveDuplicatesNetworkNames);
                            foreach (string networkName in networkNamesFromConfigThread) if (!listOfNetworkNames.Contains(networkName)) listOfNetworkNames.Add(networkName);

                            networkCompuersAndSharesHandler.ScanForComputers();
                            foreach (string computerName in networkCompuersAndSharesHandler.ComputerNames)
                            {
                                if (!listOfNetworkNames.Contains(computerName)) listOfNetworkNames.Add(computerName);
                            }
                            oneDriveNetworkNames = listOfNetworkNames; //Swap to new list;

                            Properties.Settings.Default.OneDriveDuplicatesNetworkNames = FormConfig.ConvertArrayToStringWithSepeartor(oneDriveNetworkNames.ToArray(), "\r\n");
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
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Form Constructor failed", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                return;
            }
        }

        #endregion

        #region
        private void CalculatePanelSize(KryptonForm sender, int kryptonRibbonHeight) 
        {
            panelMain.Top = kryptonRibbonHeight; //kryptonRibbonMain.Height + 1;
            panelMain.Left = 1;

            panelMain.Height = ((KryptonForm)sender).Height - (
                kryptonRibbonMain.Height +
                kryptonStatusStrip1.Height +
                ((KryptonForm)sender).RealWindowBorders.Top +
                ((KryptonForm)sender).RealWindowBorders.Bottom);
            panelMain.Width = ((KryptonForm)sender).Width - ((KryptonForm)sender).RealWindowBorders.Left - ((KryptonForm)sender).RealWindowBorders.Right;

            //

            dataGridViewTagsAndKeywords.Top = 0;
            dataGridViewTagsAndKeywords.Left = 0;
            dataGridViewTagsAndKeywords.Width = kryptonPageToolboxTagsKeywords.Width;
            dataGridViewTagsAndKeywords.Height = kryptonPageToolboxTagsKeywords.Height;

        }

        #endregion 

        #region Resize and restore windows size when reopen application
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (isFormLoading) return;
            _previousWindowsState = this.WindowState;

            CalculatePanelSize((KryptonForm)sender, kryptonRibbonMain.Height);
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            FormSplash.BringToFrontSplashForm();
            DataGridViewHandler.BringToFrontFindAndReplace();
        }
        #endregion

        #region MainForm_FormClosing
        private bool isClosingProcesAlreadyStarted = false;
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isClosingProcesAlreadyStarted) return;
            isClosingProcesAlreadyStarted = true;

            if (exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.Count > 0 || IsAnyDataUnsaved())
            {
                if (KryptonMessageBox.Show(
                    (exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.Count > 0 ?
                        "There are " + exiftoolSave_QueueSaveUsingExiftool_MetadataToSaveUpdatedByUser.Count + " unsaved media files in queue.\r\n" : "") +
                    (IsAnyDataUnsaved() ?
                        "You have unsaved changes in DataGridView\r\n" : "") +
                    "\r\nAre you sure you will close application?",
                    "Press Ok will quit application and changed will get lost.\r\n" +
                    "Press Cancel and return back to application.", (KryptonMessageBoxButtons)MessageBoxButtons.OKCancel, KryptonMessageBoxIcon.Warning, showCtrlCopy: true) == DialogResult.Cancel)
                {
                    isClosingProcesAlreadyStarted = false;
                    e.Cancel = true;
                    return;
                }
                else
                {
                    GlobalData.IsApplicationClosing = true;
                    imageListView1.StopThreads();
                    ImageListViewHandler.ClearAllAndCaches(imageListView1);
                    ImageListView_SelectionChanged_Action_ImageListView_DataGridView(true); //Even when 0 selected files, allocate data and flags, etc...
                }
            }

            MetadataDatabaseCache.StopApplication = true;

            try
            {
                exiftoolReader.MetadataGroupPrioritiesWrite(); //Updated json config file if new tags found
            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Can't save settings, Metadata Group Priorities", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }

            try
            {
                if (browser != null) browser.Dispose();
            }
            catch { }

            try
            {
                GlobalData.IsStopAndEmptyExiftoolReadQueueRequest = true;
                MetadataDatabaseCache.StopCaching = true;
                ThumbnailPosterDatabaseCache.StopCaching = true;

                //databaseMicrosoftPhotos

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
                    KryptonMessageBox.Show(ex.Message, "Can't save settings", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
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
                    imageListView1.StopThreads();
                    ImageListViewHandler.ClearAllAndCaches(imageListView1);
                    imageListView1.Dispose();
                }
                catch { }

                //---------------------------------------------------------
                //Application.DoEvents();
                Task.Delay(200).Wait();

                int waitForProcessEndRetray = 30;

                FormSplash.UpdateStatus("Stopping ImageView background threads...");
                try
                {
                    waitForProcessEndRetray = 30;
                    while (!imageListView1.IsBackgroundThreadsStopped() && waitForProcessEndRetray-- > 0)
                    {
                        //Application.DoEvents();
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
                        //Application.DoEvents();
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
                KryptonMessageBox.Show(ex.Message, "Problems during close all threads and other process during closing application", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }

            isClosingProcesAlreadyStarted = false;
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
            try
            {
                isFormLoading = false;

                #region Initialize folder tree...
                //If in Form_Load
                //System.InvalidOperationException: 'Cross-thread operation not valid: Control 'toolStrip1' accessed from a thread other than the thread it was created on.'
                try
                {
                    FormSplash.UpdateStatus("Initialize folder tree...");
                    GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = true;

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
                            TreeViewFolderBrowserHandler.RefreshTreeNode(treeViewFolderBrowser1, treeViewFolderBrowser1.Nodes[0]);
                        }

                    }
                    catch (Exception ee)
                    {
                        KryptonMessageBox.Show(Application.ProductName + "\r\n\r\n" + ee.Message, "Initialize folder tree failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                    }

                    GlobalData.DoNotTrigger_TreeViewFolder_BeforeAndAfterSelect = false;
                }
                catch (Exception ex)
                {
                    KryptonMessageBox.Show(ex.Message, "Initialize folder tree failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
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
                    KryptonMessageBox.Show(ex.Message, "Populate search failed...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                }
                #endregion

                #region Compile Exiftool Hack
                /*
                |ERROR|PhotoTagsSynchronizer.MainForm|ThreadCollectMetadataExiftool - 
	            Running Exiftool failed.|System.Exception: Encode/Encoding.pm did not return a true value at C:/Perl/lib/Encode.pm line 265.
	            Compilation failed in require at C:/Perl/lib/Archive/Zip/Archive.pm line 21.
	            BEGIN failed--compilation aborted at C:/Perl/lib/Archive/Zip/Archive.pm line 24.
	            Compilation failed in require at C:/Perl/lib/Archive/Zip.pm line 265.
	            Compilation failed in require at -e line 165.
                */
                String path = NativeMethods.GetFullPathOfFile("exiftool.exe");
                ApplicationActivation.ProcessRun(path, "-ver", false, true);

                #endregion

                ImageListView_FetchListOfMediaFiles_FromFolder_and_Aggregate(false, true);
                treeViewFolderBrowser1.Focus();
                ImageListView_SelectionChanged_Action_ImageListView_DataGridView(false);

                MaximizeOrRestoreWorkspaceMainCellAndChilds();
                SetNavigatorModeSearch((NavigatorMode)Properties.Settings.Default.WorkspaceCellFolderSearchFilterNavigatorMode);

                CalculatePanelSize(this, kryptonRibbonMain.Height);
                this.Refresh();

                #region Show About Page
                if (Properties.Settings.Default.ShowAboutPage)
                {
                    About();
                    Properties.Settings.Default.ShowAboutPage = false;
                }
                #endregion

                #region ShowDatabaseNotFoundWarning
                if (Properties.Settings.Default.ShowDatabaseNotFoundWarning)
                {
                    if (!GlobalData.doesMircosoftPhotosExists || !GlobalData.doesWindowsLivePhotoGalleryExists || GlobalData.isRunningWinSmode)
                    {
                        KryptonMessageBox.Show(
                            //Windows S mode
                            (GlobalData.isRunningWinSmode ? "\r\nWindows is Running Windows 10/11 S mode, this will reduce functionality on this app.\r\n" +
                            "- Showing maps on ChromiumWebBrowser will not work\r\n" +
                            "- Running DOS commands will not work...\r\n" +
                            "- Reading data from Exiftool will not work, in general nothing will work\r\n" : "") +
                            //Database sources
                            "\r\nPhotoTags-Synchronizer works better with connected sources.\r\n\r\n" +
                            "Tried connect to:\r\n" + 
                            (GlobalData.doesMircosoftPhotosExists ? "Mircosoft Photos (Connected)\r\n" : "Mircosoft Photos (Not connected)\r\n") +
                            (GlobalData.doesMircosoftPhotosHaveData ? "" : "Mircosoft Photos (Doesn't contains data)\r\n") +
                            (GlobalData.doesWindowsLivePhotoGalleryExists ? "Windows Live Photo Gallery (Connected)" : "Windows Live Photo Gallery (Not connected)\r\n"),
                            "PhotoTags-Synchronizer works better with...", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Information, showCtrlCopy: true);
                        Properties.Settings.Default.ShowDatabaseNotFoundWarning = false;
                    }
                }
                #endregion 

            }
            catch (Exception ex)
            {
                KryptonMessageBox.Show(ex.Message, "Form Load failed", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
                return;
            }
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
            //tableLayoutPanelSerachSearch.Width = Math.Max(kryptonPageFolderSearchFilterSearch.Width - 25, tableLayoutPanelSearchKeywords.MinimumSize.Width + 5);
            //tableLayoutPanelSerachActions.Width = tableLayoutPanelSerachSearch.Width;
        }

        private void kryptonPageToolboxTagsDetails_Resize(object sender, EventArgs e)
        {
            //tableLayoutPanelTags.Width = Math.Max(kryptonPageToolboxTagsDetails.Width - 25, tableLayoutPanelTags.MinimumSize.Width);
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



        private void kryptonRibbonGroupButtonHomeHelpUserGuide_Click(object sender, EventArgs e)
        {
            ApplicationAssociations.ApplicationActivation.OpenUserGuide();
        }

        private void kryptonRibbonGroupButtonToolsHelpUserGuide_Click(object sender, EventArgs e)
        {
            ApplicationAssociations.ApplicationActivation.OpenUserGuide();
        }


        private static void SetNlogLogLevel(NLog.LogLevel newMinLoglevel)
        {
            // Uncomment these to enable NLog logging. NLog exceptions are swallowed by default.
            ////NLog.Common.InternalLogger.LogFile = @"C:\Temp\nlog.debug.log";
            ////NLog.Common.InternalLogger.LogLevel = LogLevel.Debug;

            if (newMinLoglevel == NLog.LogLevel.Off)
            {
                NLog.LogManager.SuspendLogging();
            }
            else
            {
                if (!NLog.LogManager.IsLoggingEnabled()) NLog.LogManager.ResumeLogging();

                foreach (var rule in NLog.LogManager.Configuration.LoggingRules)
                {
                    // Iterate over all levels up to and including the target, (re)enabling them.

                    bool targetFound = false;
                    foreach (var target in rule.Targets)
                    {
                        if (target.Name == "logfile") targetFound = true;
                    }
                    if (targetFound)
                    {
                        foreach (var logLevel in NLog.LogLevel.AllLevels)
                        {
                            if (logLevel.Ordinal >= newMinLoglevel.Ordinal)
                            {
                                rule.EnableLoggingForLevel(logLevel);
                            }
                            else
                            {
                                rule.DisableLoggingForLevel(logLevel);
                            }
                        }
                    }
                }
            }

            LogManager.ReconfigExistingLoggers();
        }

        public static void UpdateSettings()
        {
            #region Setting - LogLevel
            try
            {
                switch (Properties.Settings.Default.LogLevel.ToUpper())
                {
                    case "OFF":
                        SetNlogLogLevel(NLog.LogLevel.Off);
                        break;
                    case "TRACE":
                        SetNlogLogLevel(NLog.LogLevel.Trace);
                        break;
                    case "DEBUG":
                        SetNlogLogLevel(NLog.LogLevel.Debug);
                        break;
                    case "INFO":
                        SetNlogLogLevel(NLog.LogLevel.Info);
                        break;
                    case "WARN":
                        SetNlogLogLevel(NLog.LogLevel.Warn);
                        break;
                    case "ERROR":
                        SetNlogLogLevel(NLog.LogLevel.Error);
                        break;
                    case "FATAL":
                        SetNlogLogLevel(NLog.LogLevel.Fatal);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "UpdateSettings failed. ");
            }
            #endregion

            #region WriteMetadataTags
            try
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.WriteMetadataTags.Trim()))
                {
                    Properties.Settings.Default.WriteMetadataTags = Properties.Settings.Default.WriteMetadataTagsReset;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "UpdateSettings failed. ");
            }
            #endregion
        }

        #region kryptonRibbonGroupButtonCopyOldMicrosoftPhotosDatabaseToLegacy_Click
        private void kryptonRibbonGroupButtonCopyOldMicrosoftPhotosDatabaseToLegacy_Click(object sender, EventArgs e)
        {
            if (KryptonMessageBox.Show(
                "Microsoft Windows Photos exist in 3 versions.\r\n" +
                "\r\n" +
                "The first version arrived in Windows 10, then Microsoft created a new version in 2023, but kept the old version and renamed it to Microsoft Photos Legacy.So if you used the oldest version, then Microsoft Photos Legacy doesn’t have all data from the oldest version.\r\n" +
                "\r\n" +
                "This tool will copy data from the first version of Microsoft Photo app, to Microsoft Photo Legacy app.\r\n" +
                "\r\n"+
                "Do you want to copy from old to Legacy app?",
                "Copy Microsoft Photos database file!",
                (KryptonMessageBoxButtons)MessageBoxButtons.OKCancel, KryptonMessageBoxIcon.Warning, showCtrlCopy: true) == DialogResult.Cancel)
            {
                return;
            }

            try 
            {
                #region Make sure copy is posible
                File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows10old + "\\LocalState\\MediaDb.v1.sqlite"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows11Legacy + "\\LocalState\\MediaDb.v1.sqlite.temp"), true);
                #endregion

                #region Make sure backup is possible
                File.Move (
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows11Legacy + "\\LocalState\\MediaDb.v1.sqlite"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows11Legacy + "\\LocalState\\MediaDb.v1.sqlite.backup"));

                try
                {
                    File.Move(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows11Legacy + "\\LocalState\\MediaDb.v1.sqlite-shm"),
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows11Legacy + "\\LocalState\\MediaDb.v1.sqlite-shm.backup"));
                } catch 
                {
                    //Ignore errors
                }

                try
                {
                    File.Move(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows11Legacy + "\\LocalState\\MediaDb.v1.sqlite-wal"),
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows11Legacy + "\\LocalState\\MediaDb.v1.sqlite-wal.backup"));
                } catch
                {
                    //Ignore errors
                }
                #endregion

                #region Use the backup
                File.Move(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows11Legacy + "\\LocalState\\MediaDb.v1.sqlite.temp"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Packages\\" + SqliteDatabaseUtilities.MicrosoftWindowsPhotosWindows11Legacy + "\\LocalState\\MediaDb.v1.sqlite"));
                #endregion

                KryptonMessageBox.Show("The database from Old Microsoft Windows Photos app was copied to Microsoft Windows Photos Legacy app", "Copying Microsoft Photos database done", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
            catch (Exception ex) {
                string helpText = "";
                if (ex.HResult == -2147024864)
                {
                    helpText = "\r\n\r\n" +
                        "Microsoft Windows Photos are been used.\r\n" +
                        "Therefore the files is locked and can be moved.\r\n" +
                        "Try to stop both the Microsoft Microsoft Photos and \r\n" +
                        "Microsoft Microsoft Photos Legacy app and then retry...\r\n";

                }
                KryptonMessageBox.Show(ex.Message + helpText, "Copying Microsoft Photos database failed", (KryptonMessageBoxButtons)MessageBoxButtons.OK, KryptonMessageBoxIcon.Error, showCtrlCopy: true);
            }
        }

        #endregion


        
    }


}



