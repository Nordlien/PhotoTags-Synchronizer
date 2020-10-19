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

namespace PhotoTagsSynchronizer
{

    public partial class MainForm : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private ShowWhatColumns showWhatColumns;

        private readonly Size[] thumbnailSizes =
        {
          new Size (128, 128),  //0
          new Size (144, 144),  //1
          new Size (192, 192),  //2
          new Size (256, 256),  //3
          new Size (384, 384)   //4 //Making this to big will eat memory and create app slow due to limit in momory and sqlite
        };
        private const int defaultThumbnailSizeNumber = 1;
        private Size maxThumbnailSize
        {
            get { return thumbnailSizes[thumbnailSizes.Length - 3]; }
        }

        private readonly ChromiumWebBrowser browser;
        private FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

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

        private MicrosoftPhotosReader databaseWindowsPhotos;
        private WindowsLivePhotoGalleryDatabasePipe databaseWindowsLivePhotGallery;

        private FilesCutCopyPasteDrag filesCutCopyPasteDrag;

        private RendererItem defaultRendererItem;


        //Avoid flickering
        private bool isFormLoading = true;                  //Avoid flicker and on change events going in loop
        private bool isSettingDefaultComboxValues = false;  //Avoid multiple reload when value are set, avoid on value change event
        private bool isTabControlToolboxChanging = false;   //Avoid multiple reload when value are set, avoid on value change event
        private FormWindowState _previousWindowsState = FormWindowState.Normal;

        #region Constructor - MainForm()
        public MainForm()
        {

            SplashForm.UpdateStatus("Initialize component..."); //6 
            InitializeComponent();
            imageListView1.ThumbnailSize = thumbnailSizes[defaultThumbnailSizeNumber];
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
                        defaultRendererItem = (RendererItem)renderertoolStripComboBox.Items[i];
                    }
                    i++;
                }
            }

            SplashForm.UpdateStatus("Initialize database: metadata cache...");
            databaseUtilitiesSqliteMetadata = new SqliteDatabaseUtilities(DatabaseType.SqliteMetadataDatabase, 10000, 5000);

            databaseGoogleLocationHistory = new GoogleLocationHistoryDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseAndCacheMetadataExiftool = new MetadataDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseAndCacheThumbnail = new ThumbnailDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseExiftoolData = new ExiftoolDataDatabase(databaseUtilitiesSqliteMetadata);
            databaseExiftoolWarning = new ExiftoolWarningDatabase(databaseUtilitiesSqliteMetadata);

            databaseAndCahceCameraOwner = new CameraOwnersDatabaseCache(databaseUtilitiesSqliteMetadata);
            databaseLocationAddress = new LocationNameLookUpCache(databaseUtilitiesSqliteMetadata);

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
                databaseWindowsPhotos = new MicrosoftPhotosReader();
            }
            catch (Exception e)
            {
                SplashForm.AddWarning("Windows photo warning:\r\n" + e.Message + "\r\n");
                databaseWindowsPhotos = null;
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

            splitContainerMain.SplitterDistance = Properties.Settings.Default.SplitContainerMain;
            splitContainerFolder.SplitterDistance = Properties.Settings.Default.SplitContainerFolder;
            splitContainerImages.SplitterDistance = Properties.Settings.Default.SplitContainerImages;
            splitContainerMap.SplitterDistance = Properties.Settings.Default.SplitContainerMap;
            renderertoolStripComboBox.SelectedIndex = Properties.Settings.Default.RenderertoolStripComboBox;
            toolStripButtonHistortyColumns.Checked = Properties.Settings.Default.ShowHistortyColumns;
            toolStripButtonErrorColumns.Checked = Properties.Settings.Default.ShowErrorColumns;

            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);
            
            timerShowErrorMessage.Enabled = true;

            PopulateExiftoolToolStripMenuItems();
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
            PopulateDetailsOnSelectedImageListViewItemsOnActiveDataGridViewInvoke(imageListView1.SelectedItems);
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

        private void splitContainerMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) return;
            if (WindowState == FormWindowState.Maximized) return;
            if (!isFormLoading)
            {
                Properties.Settings.Default.SplitContainerMain = splitContainerMain.SplitterDistance;
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

        private void MainForm_Shown(object sender, EventArgs e)
        {
            isFormLoading = false;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            SplashForm.BringToFrontSplashForm();
            DataGridViewHandler.BringToFrontFindAndReplace();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (queueSaveMetadataUpdatedByUser.Count > 0)
            {
                if (MessageBox.Show(
                    "There are " + queueSaveMetadataUpdatedByUser.Count + " unsaved media files. Are you sure you will close application?",
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

                Properties.Settings.Default.SplitContainerMain = splitContainerMain.SplitterDistance;
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

            SplashForm.UpdateStatus("Discounect databases...");
            databaseUtilitiesSqliteMetadata.DatabaseClose(); //Close database after all background threads stopped

            SplashForm.UpdateStatus("Dispose...");
            imageListView1.Dispose();
            SplashForm.CloseForm();

        }
        #endregion

        #region MainForm_Load
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
            
            FolderSelected_AggregateListViewWithFilesFromFolder(folderTreeViewFolder.GetSelectedNodePath(), false);
            FilesSelected(); //PopulateSelectedImageListViewItemsAndClearAllDataGridViewsInvoke(imageListView1.SelectedItems);

            GlobalData.IsPopulatingFolderTree = false;
            SplashForm.CloseForm();

            Properties.Settings.Default.Reload();

            isFormLoading = true;
            this.Size = Properties.Settings.Default.MainFormSize;
            this.Location = Properties.Settings.Default.MainFormLocation;
            this.Activate();

        }



        #endregion

        #region Switch Renderers
        private struct RendererItem
        {
            public Type Type;

            public override string ToString()
            {
                return Type.Name;
            }

            public RendererItem(Type type)
            {
                Type = type;
            }
        }

        private void renderertoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isFormLoading) return;
            Properties.Settings.Default.RenderertoolStripComboBox = renderertoolStripComboBox.SelectedIndex;
            Properties.Settings.Default.Save();
            // Change the renderer
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            RendererItem item = (RendererItem)renderertoolStripComboBox.SelectedItem;
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(item.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }
        #endregion

        #region Switch View Modes
        private void thumbnailsToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Thumbnails;
            rendererToolStripLabel.Visible = true;
            renderertoolStripComboBox.Visible = true;
            toolStripSeparatorRenderer.Visible = true;

            renderertoolStripComboBox.SelectedIndex = Properties.Settings.Default.RenderertoolStripComboBox;
            renderertoolStripComboBox_SelectedIndexChanged(null, null);
        }

        private void galleryToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Gallery;
            rendererToolStripLabel.Visible = false;
            renderertoolStripComboBox.Visible = false;
            toolStripSeparatorRenderer.Visible = false;

            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(defaultRendererItem.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }

        private void paneToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Pane;
            rendererToolStripLabel.Visible = false;
            renderertoolStripComboBox.Visible = false;
            toolStripSeparatorRenderer.Visible = false;

            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(defaultRendererItem.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }

        private void detailsToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Details;
            rendererToolStripLabel.Visible = false;
            renderertoolStripComboBox.Visible = false;
            toolStripSeparatorRenderer.Visible = false;

            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(defaultRendererItem.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }
        #endregion

        #region Modify Column Headers
        private void columnsToolStripButton_Click(object sender, EventArgs e)
        {
            ChooseColumns form = new ChooseColumns();
            form.imageListView = imageListView1;
            form.ShowDialog();
        }
        #endregion

        #region Change Thumbnail Size

        private void toolStripButtonThumbnailSize1_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[4];
        }

        private void toolStripButtonThumbnailSize2_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[3];
        }

        private void toolStripButtonThumbnailSize3_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[2];
        }

        private void toolStripButtonThumbnailSize4_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[1];
        }

        private void toolStripButtonThumbnailSize5_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = thumbnailSizes[0];
        }
        #endregion

        #region Rotate Selected Images
        private void rotateCCWToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
                "PhotoTagsSynchronizerApplication", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    item.BeginEdit();
                    using (Image img = Manina.Windows.Forms.Utility.LoadImageWithoutLock(item.FullFileName))
                    {
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        img.Save(item.FullFileName);
                    }
                    item.Update();
                    item.EndEdit();
                }
            }
        }

        private void rotateCWToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
                "PhotoTagsSynchronizerApplication", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    item.BeginEdit();
                    using (Image img = Manina.Windows.Forms.Utility.LoadImageWithoutLock(item.FullFileName))
                    {
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        img.Save(item.FullFileName);
                    }
                    item.Update();
                    item.EndEdit();
                }
            }
        }
        #endregion


        #region SetGridViewSize Small Medium Big
        private void SetGridViewSize(DataGridViewSize size)
        {
            switch (tabControlToolbox.TabPages[tabControlToolbox.SelectedIndex].Tag.ToString())
            {
                case "Tags":
                    DataGridViewHandler.SetCellSize(dataGridViewTagsAndKeywords, size, false);
                    Properties.Settings.Default.CellSizeKeywords = (int)size;
                    break;
                case "Map":
                    DataGridViewHandler.SetCellSize(dataGridViewMap, size, false);
                    Properties.Settings.Default.CellSizeMap = (int)size;
                    break;
                case "People":
                    DataGridViewHandler.SetCellSize(dataGridViewPeople, size, true);
                    Properties.Settings.Default.CellSizePeoples = (int)size;
                    break;
                case "Date":
                    DataGridViewHandler.SetCellSize(dataGridViewDate, size, false);
                    Properties.Settings.Default.CellSizeDates = (int)size;
                    break;
                case "ExifTool":
                    DataGridViewHandler.SetCellSize(dataGridViewExifTool, size, false);
                    Properties.Settings.Default.CellSizeExiftool = (int)size;
                    break;
                case "Warning":
                    DataGridViewHandler.SetCellSize(dataGridViewExifToolWarning, size, false);
                    Properties.Settings.Default.CellSizeWarnings = (int)size;
                    break;
                case "Properties":
                    DataGridViewHandler.SetCellSize(dataGridViewProperties, size, false);
                    Properties.Settings.Default.CellSizeProperties = (int)size;
                    break;
                case "Rename":
                    DataGridViewHandler.SetCellSize(dataGridViewRename, (size | DataGridViewSize.RenameSize), false);
                    Properties.Settings.Default.CellSizeRename = (int)size;
                    break;
                default:
                    throw new Exception("Not implemented");
            }
        }

        private void toolStripButtonGridBig_Click(object sender, EventArgs e)
        {
            SetGridViewSize(DataGridViewSize.Large);
        } 

        private void toolStripButtonGridNormal_Click(object sender, EventArgs e)
        {
            SetGridViewSize(DataGridViewSize.Medium);
        }

        private void toolStripButtonGridSmall_Click(object sender, EventArgs e)
        {
            SetGridViewSize(DataGridViewSize.Small);
        }
        #endregion

        private void toolStripButtonConfig_Click(object sender, EventArgs e)
        {
            using (Config config = new Config())
            {
                exiftoolReader.MetadataReadPrioity.ReadOnlyOnce();
                config.MetadataReadPrioity = exiftoolReader.MetadataReadPrioity;
                config.Init();
                config.ShowDialog();
            }
        }



        private void toolStripButtonHistortyColumns_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowHistortyColumns = toolStripButtonHistortyColumns.Checked;
            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);
            UpdateMetadataOnSelectedFilesOnActiveDataGrivView(imageListView1.SelectedItems);
        }

        private void toolStripButtonErrorColumns_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowErrorColumns = toolStripButtonErrorColumns.Checked;
            showWhatColumns = ShowWhatColumnHandler.SetShowWhatColumns(toolStripButtonHistortyColumns.Checked, toolStripButtonErrorColumns.Checked);
            UpdateMetadataOnSelectedFilesOnActiveDataGrivView(imageListView1.SelectedItems);
        }

        
    }
}




/////////////////////////////////////////////////////////////////
//https://github.com/radioman/greatmaps / Maps library
//https://www.nuget.org/packages/Nominatim.API/ Address lockup

//Image reading
//https://imageprocessor.org/imageprocessor/imagefactory/tint/ Image Processor

//Exiftool
//Start Exiftool Process in a better way
//https://github.com/madelson/MedallionShell/ //Star commadn Shell and support encoding
//Another option would be to HTML encode the problematic characters and use exiftool's -E (escapeHTML) option. For example -E -City="&#x158;&#xED;&#x10D;any"
//https://github.com/Ruslan-B/FFmpeg.AutoGen Exiftool Wrapper
//https://github.com/AerisG222/NExifTool Exiftool Wrapper

//FFMPEG
//https://xabe.net/product/xabe-ffmpeg/ FFMPEG Wrapper
//https://www.nrecosite.com/video_converter_net.aspx FFMPEG Embedded
//var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
//ffMpeg.GetVideoThumbnail(pathToVideoFile, thumbJpegStream,5);
/*
    * NReco.VideoConverter (FFMpeg wrapper)
-------------------------------------
Website (release notes, examples etc): https://www.nrecosite.com/video_converter_net.aspx
API documentation: https://www.nrecosite.com/doc/NReco.VideoConverter/
Nuget package: https://www.nuget.org/packages/NReco.VideoConverter/

NReco.VideoConverter (FFMpeg wrapper) - customer settinings / args
https://stackoverflow.com/questions/34234263/how-to-use-nreco-ffmpeg-convertmedia-with-filter-complex
ffMpeg.ConvertMedia(this.Video + ".mov", 
    null, // autodetect by input file extension 
    outPutVideo1 + ".mp4", 
    null, // autodetect by output file extension 
    new NReco.VideoConverter.ConvertSettings() {
        CustomOutputArgs = " -filter_complex \"[0] yadif=0:-1:0,scale=iw*sar:ih,scale='if(gt(a,16/9),1280,-2)':'if(gt(a,16/9),-2,720)'[scaled];[scaled] pad=1280:720:(ow-iw)/2:(oh-ih)/2:black \" -c:v libx264 -c:a mp3 -ab 128k "
    }
);

License
-------
VideoConverter can be used for FREE in single-deployment projects (websites, intranet/extranet) or applications for company's internal business purposes (redistributed only internally inside the company). 
Commercial license (included into enterprise source code pack) is required for:
1) Applications for external redistribution (ISV)
2) SaaS deployments

How to use
----------
var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
ffMpeg.ConvertMedia("input.mov", "output.mp4", Format.mp4);
*/

//Metadata
//Taglib.sharp
//https://github.com/mono/taglib-sharp _ Metadata Read and Write ***** LOT OF FORMATS *****
//Example: Get thumbnail from file -- https://stackoverflow.com/questions/17904184/using-taglib-to-display-the-cover-art-in-a-image-box-in-wpf
//
//https://github.com/drewnoakes/metadata-extractor




