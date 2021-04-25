using Manina.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Filter");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusFilesAndSelected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarSaveProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripProgressBarDataGridViewLoading = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusQueue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusAction = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerFolder = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageFilterFolder = new System.Windows.Forms.TabPage();
            this.folderTreeViewFolder = new Furty.Windows.Forms.FolderTreeView();
            this.contextMenuStripTreeViewFolder = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemTreeViewFolderCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderRefreshFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderReadSubfolders = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderReload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderClearCache = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageFilterSearch = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.checkBoxSerachFitsAllValues = new System.Windows.Forms.CheckBox();
            this.panelSearchFilter = new System.Windows.Forms.Panel();
            this.groupBoxSearchKeywords = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxSearchNeedAllKeywords = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBoxSearchKeyword = new System.Windows.Forms.ComboBox();
            this.checkBoxSearchWithoutKeyword = new System.Windows.Forms.CheckBox();
            this.groupBoxSearchRating = new System.Windows.Forms.GroupBox();
            this.checkBoxSearchRatingEmpty = new System.Windows.Forms.CheckBox();
            this.checkBoxSearchRating5 = new System.Windows.Forms.CheckBox();
            this.checkBoxSearchRating4 = new System.Windows.Forms.CheckBox();
            this.checkBoxSearchRating3 = new System.Windows.Forms.CheckBox();
            this.checkBoxSearchRating2 = new System.Windows.Forms.CheckBox();
            this.checkBoxSearchRating1 = new System.Windows.Forms.CheckBox();
            this.checkBoxSearchRating0 = new System.Windows.Forms.CheckBox();
            this.groupBoxSearchPeople = new System.Windows.Forms.GroupBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.checkedListBoxSearchPeople = new System.Windows.Forms.CheckedListBox();
            this.checkBoxSearchWithoutRegions = new System.Windows.Forms.CheckBox();
            this.checkBoxSearchNeedAllNames = new System.Windows.Forms.CheckBox();
            this.groupBoxSearchExtra = new System.Windows.Forms.GroupBox();
            this.checkBoxSearchHasWarning = new System.Windows.Forms.CheckBox();
            this.groupBoxSearchMediaTaken = new System.Windows.Forms.GroupBox();
            this.checkBoxSearchMediaTakenIsNull = new System.Windows.Forms.CheckBox();
            this.dateTimePickerSearchDateFrom = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.dateTimePickerSearchDateTo = new System.Windows.Forms.DateTimePicker();
            this.groupBoxSearchTags = new System.Windows.Forms.GroupBox();
            this.checkBoxSearchUseAndBetweenTextTagFields = new System.Windows.Forms.CheckBox();
            this.comboBoxSearchComments = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxSearchAlbum = new System.Windows.Forms.ComboBox();
            this.comboBoxSearchTitle = new System.Windows.Forms.ComboBox();
            this.comboBoxSearchDescription = new System.Windows.Forms.ComboBox();
            this.comboBoxSearchLocationCountry = new System.Windows.Forms.ComboBox();
            this.comboBoxSearchLocationName = new System.Windows.Forms.ComboBox();
            this.comboBoxSearchLocationState = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBoxSearchLocationCity = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.tabPageFilterTags = new System.Windows.Forms.TabPage();
            this.treeViewFilter = new DragNDrop.TreeViewWithoutDoubleClick();
            this.splitContainerImages = new System.Windows.Forms.SplitContainer();
            this.imageListView1 = new Manina.Windows.Forms.ImageListView();
            this.contextMenuStripImageListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemImageListViewCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.copyFileNamesToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewRefreshFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImageListViewAutoCorrect = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileWithAssociatedApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMediaFilesWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFileWithAssociatedApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSelectedToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateCW90ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotate180ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ratateCCW270ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControlToolbox = new System.Windows.Forms.TabControl();
            this.tabPageTags = new System.Windows.Forms.TabPage();
            this.labelTagsInformation = new System.Windows.Forms.Label();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.comboBoxAuthor = new System.Windows.Forms.ComboBox();
            this.comboBoxMediaAiConfidence = new System.Windows.Forms.ComboBox();
            this.dataGridViewTagsAndKeywords = new System.Windows.Forms.DataGridView();
            this.contextMenuStripTagsAndKeywords = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cutToolStripMenuTagsBrokerCut = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuTagsBrokerCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuTagsBrokerPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuTagsBrokerDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuTags = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuTags = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuTag = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuTag = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuTagsBrokerSave = new System.Windows.Forms.ToolStripMenuItem();
            this.markAsFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAsFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleFavoriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFavoriteRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideEqualRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTagsBrokerCopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTagsAndKeywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTagsAndKeywordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTagsAndKeywordMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBoxRating = new System.Windows.Forms.GroupBox();
            this.radioButtonRating5 = new System.Windows.Forms.RadioButton();
            this.radioButtonRating4 = new System.Windows.Forms.RadioButton();
            this.radioButtonRating3 = new System.Windows.Forms.RadioButton();
            this.radioButtonRating2 = new System.Windows.Forms.RadioButton();
            this.radioButtonRating1 = new System.Windows.Forms.RadioButton();
            this.comboBoxAlbum = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxComments = new System.Windows.Forms.ComboBox();
            this.labelComments = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.comboBoxDescription = new System.Windows.Forms.ComboBox();
            this.comboBoxTitle = new System.Windows.Forms.ComboBox();
            this.tabPagePeople = new System.Windows.Forms.TabPage();
            this.dataGridViewPeople = new System.Windows.Forms.DataGridView();
            this.contextMenuStripPeople = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemPeopleRenameFromLast1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleRenameFromLast2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleRenameFromLast3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleRenameFromMostUsed = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleRenameFromAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemPeopleSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleRenameSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeoplePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleShowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleHideEqualRows = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleTogglePeopleTag = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleSelectPeopleTag = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleRemovePeopleTag = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleShowRegionSelector = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPeopleMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageMap = new System.Windows.Forms.TabPage();
            this.splitContainerMap = new System.Windows.Forms.SplitContainer();
            this.comboBoxGoogleLocationInterval = new System.Windows.Forms.ComboBox();
            this.comboBoxGoogleTimeZoneShift = new System.Windows.Forms.ComboBox();
            this.dataGridViewMap = new System.Windows.Forms.DataGridView();
            this.contextMenuStripMap = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemMapCut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapShowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapHideEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapCopyNotOverwrite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapCopyAndOverwrite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowCoordinateOnMap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapReloadLocationUsingNominatim = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMapMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBoxBrowserURL = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.comboBoxMapZoomLevel = new System.Windows.Forms.ComboBox();
            this.panelBrowser = new System.Windows.Forms.Panel();
            this.tabPageDate = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridViewDate = new System.Windows.Forms.DataGridView();
            this.tabPageExifTool = new System.Windows.Forms.TabPage();
            this.dataGridViewExifTool = new System.Windows.Forms.DataGridView();
            this.contextMenuStripExifTool = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemExiftoolAssignCompositeTag = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolSHowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolHideEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageExifToolWarning = new System.Windows.Forms.TabPage();
            this.dataGridViewExifToolWarning = new System.Windows.Forms.DataGridView();
            this.contextMenuStripExiftoolWarning = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemtoolExiftoolWarningAssignCompositeTag = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningFind = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningMarkFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningRemoveFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningToggleFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningShowFavorite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningHideEqual = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExiftoolWarningMediaPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageFileProperties = new System.Windows.Forms.TabPage();
            this.dataGridViewProperties = new System.Windows.Forms.DataGridView();
            this.tabPageFileRename = new System.Windows.Forms.TabPage();
            this.buttonRenameUpdate = new System.Windows.Forms.Button();
            this.buttonRenameSave = new System.Windows.Forms.Button();
            this.comboBoxRenameVariableList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRenameNewName = new System.Windows.Forms.TextBox();
            this.dataGridViewRename = new System.Windows.Forms.DataGridView();
            this.tabPageMediaConverter = new System.Windows.Forms.TabPage();
            this.panelConvertAndMerge = new System.Windows.Forms.Panel();
            this.dataGridViewConvertAndMerge = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.thumbnailsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.galleryToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.paneToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.detailsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorRenderer = new System.Windows.Forms.ToolStripSeparator();
            this.rendererToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this.renderertoolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.columnsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonThumbnailSize1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonThumbnailSize2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonThumbnailSize3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonThumbnailSize4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonThumbnailSize5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.rotateCCWToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.rotate180ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.rotateCWToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPreview = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSelectPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonSelectGroupBy = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemSelectSameDay = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectSame3Day = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectSameWeek = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectSame2week = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectSameMonth = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectFallbackOnFileCreated = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSelectMax10items = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectMax30items = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectMax50items = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectMax100items = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSelectSameLocationName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectSameCity = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectSameDistrict = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectSameCountry = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonSelectNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonGridBig = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGridNormal = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGridSmall = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonHistortyColumns = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonErrorColumns = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonImportGoogleLocation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAllMetadata = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonConfig = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWebScraper = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.imageListFilter = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.timerShowErrorMessage = new System.Windows.Forms.Timer(this.components);
            this.timerActionStatusRemove = new System.Windows.Forms.Timer(this.components);
            this.timerStartThread = new System.Windows.Forms.Timer(this.components);
            this.timerShowExiftoolSaveProgress = new System.Windows.Forms.Timer(this.components);
            this.timerStatusUpdate = new System.Windows.Forms.Timer(this.components);
            this.timerUpdateDataGridViewLoadingProgressbarRemove = new System.Windows.Forms.Timer(this.components);
            this.panelMediaPreview = new System.Windows.Forms.Panel();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.imageBoxPreview = new Cyotek.Windows.Forms.ImageBox();
            this.videoView1 = new LibVLCSharp.WinForms.VideoView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonMediaPreviewPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMediaPreviewNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMediaPreviewPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMediaPreviewPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMediaPreviewFastBackward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMediaPreviewFastForward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMediaPreviewStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonChromecastList = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemMediaChromecast = new System.Windows.Forms.ToolStripMenuItem();
            this.tv2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonMediaList = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemPreviewSlideShowMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemPreviewSlideShow2sec = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPreviewSlideShow4sec = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPreviewSlideShow6sec = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPreviewSlideShow8sec = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPreviewSlideShow10sec = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPreviewSlideShowStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonMediaPreviewRotateCCW = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMediaPreviewRotate180 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMediaPreviewRotateCW = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMediaPreviewClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripTraceBarItemMediaPreviewTimer = new DragNDrop.ToolStripTraceBarItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelMediaPreviewTimer = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelMediaPreviewStatus = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.timerFindGoogleCast = new System.Windows.Forms.Timer(this.components);
            this.timerPreviewNextTimer = new System.Windows.Forms.Timer(this.components);
            this.timerSaveProgessRemoveProgress = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFolder)).BeginInit();
            this.splitContainerFolder.Panel1.SuspendLayout();
            this.splitContainerFolder.Panel2.SuspendLayout();
            this.splitContainerFolder.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageFilterFolder.SuspendLayout();
            this.contextMenuStripTreeViewFolder.SuspendLayout();
            this.tabPageFilterSearch.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panelSearchFilter.SuspendLayout();
            this.groupBoxSearchKeywords.SuspendLayout();
            this.groupBoxSearchRating.SuspendLayout();
            this.groupBoxSearchPeople.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBoxSearchExtra.SuspendLayout();
            this.groupBoxSearchMediaTaken.SuspendLayout();
            this.groupBoxSearchTags.SuspendLayout();
            this.tabPageFilterTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImages)).BeginInit();
            this.splitContainerImages.Panel1.SuspendLayout();
            this.splitContainerImages.Panel2.SuspendLayout();
            this.splitContainerImages.SuspendLayout();
            this.contextMenuStripImageListView.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControlToolbox.SuspendLayout();
            this.tabPageTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTagsAndKeywords)).BeginInit();
            this.contextMenuStripTagsAndKeywords.SuspendLayout();
            this.groupBoxRating.SuspendLayout();
            this.tabPagePeople.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeople)).BeginInit();
            this.contextMenuStripPeople.SuspendLayout();
            this.tabPageMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMap)).BeginInit();
            this.splitContainerMap.Panel1.SuspendLayout();
            this.splitContainerMap.Panel2.SuspendLayout();
            this.splitContainerMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMap)).BeginInit();
            this.contextMenuStripMap.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPageDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDate)).BeginInit();
            this.tabPageExifTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExifTool)).BeginInit();
            this.contextMenuStripExifTool.SuspendLayout();
            this.tabPageExifToolWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExifToolWarning)).BeginInit();
            this.contextMenuStripExiftoolWarning.SuspendLayout();
            this.tabPageFileProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).BeginInit();
            this.tabPageFileRename.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRename)).BeginInit();
            this.tabPageMediaConverter.SuspendLayout();
            this.panelConvertAndMerge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConvertAndMerge)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.panelMediaPreview.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainerFolder);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1387, 892);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(1387, 950);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusFilesAndSelected,
            this.toolStripProgressBarSaveProgress,
            this.toolStripProgressBarDataGridViewLoading,
            this.toolStripStatusQueue,
            this.toolStripStatusAction});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1387, 30);
            this.statusStrip.TabIndex = 0;
            // 
            // toolStripStatusFilesAndSelected
            // 
            this.toolStripStatusFilesAndSelected.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusFilesAndSelected.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusFilesAndSelected.Name = "toolStripStatusFilesAndSelected";
            this.toolStripStatusFilesAndSelected.Size = new System.Drawing.Size(133, 24);
            this.toolStripStatusFilesAndSelected.Text = "Files: 0 Selected: 0";
            // 
            // toolStripProgressBarSaveProgress
            // 
            this.toolStripProgressBarSaveProgress.Name = "toolStripProgressBarSaveProgress";
            this.toolStripProgressBarSaveProgress.Size = new System.Drawing.Size(100, 22);
            this.toolStripProgressBarSaveProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBarSaveProgress.ToolTipText = "Save progress";
            this.toolStripProgressBarSaveProgress.Visible = false;
            // 
            // toolStripProgressBarDataGridViewLoading
            // 
            this.toolStripProgressBarDataGridViewLoading.Name = "toolStripProgressBarDataGridViewLoading";
            this.toolStripProgressBarDataGridViewLoading.Size = new System.Drawing.Size(100, 22);
            this.toolStripProgressBarDataGridViewLoading.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.toolStripProgressBarDataGridViewLoading.ToolTipText = "Load metadata status";
            this.toolStripProgressBarDataGridViewLoading.Value = 100;
            this.toolStripProgressBarDataGridViewLoading.Visible = false;
            // 
            // toolStripStatusQueue
            // 
            this.toolStripStatusQueue.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusQueue.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusQueue.Name = "toolStripStatusQueue";
            this.toolStripStatusQueue.Size = new System.Drawing.Size(56, 24);
            this.toolStripStatusQueue.Text = "Queue";
            // 
            // toolStripStatusAction
            // 
            this.toolStripStatusAction.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusAction.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusAction.Name = "toolStripStatusAction";
            this.toolStripStatusAction.Size = new System.Drawing.Size(124, 24);
            this.toolStripStatusAction.Text = "Waiting actions...";
            // 
            // splitContainerFolder
            // 
            this.splitContainerFolder.BackColor = System.Drawing.Color.Black;
            this.splitContainerFolder.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainerFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerFolder.Location = new System.Drawing.Point(0, 0);
            this.splitContainerFolder.Name = "splitContainerFolder";
            // 
            // splitContainerFolder.Panel1
            // 
            this.splitContainerFolder.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainerFolder.Panel2
            // 
            this.splitContainerFolder.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerFolder.Panel2.Controls.Add(this.splitContainerImages);
            this.splitContainerFolder.Size = new System.Drawing.Size(1387, 892);
            this.splitContainerFolder.SplitterDistance = 338;
            this.splitContainerFolder.SplitterWidth = 10;
            this.splitContainerFolder.TabIndex = 0;
            this.splitContainerFolder.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerFolder_SplitterMoved);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageFilterFolder);
            this.tabControl1.Controls.Add(this.tabPageFilterSearch);
            this.tabControl1.Controls.Add(this.tabPageFilterTags);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(338, 892);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageFilterFolder
            // 
            this.tabPageFilterFolder.Controls.Add(this.folderTreeViewFolder);
            this.tabPageFilterFolder.Location = new System.Drawing.Point(4, 26);
            this.tabPageFilterFolder.Name = "tabPageFilterFolder";
            this.tabPageFilterFolder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilterFolder.Size = new System.Drawing.Size(330, 862);
            this.tabPageFilterFolder.TabIndex = 0;
            this.tabPageFilterFolder.Text = "Folder";
            this.tabPageFilterFolder.UseVisualStyleBackColor = true;
            // 
            // folderTreeViewFolder
            // 
            this.folderTreeViewFolder.AllowDrop = true;
            this.folderTreeViewFolder.ContextMenuStrip = this.contextMenuStripTreeViewFolder;
            this.folderTreeViewFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderTreeViewFolder.HideSelection = false;
            this.folderTreeViewFolder.ItemHeight = 16;
            this.folderTreeViewFolder.Location = new System.Drawing.Point(3, 3);
            this.folderTreeViewFolder.Name = "folderTreeViewFolder";
            this.folderTreeViewFolder.Size = new System.Drawing.Size(324, 856);
            this.folderTreeViewFolder.TabIndex = 0;
            this.folderTreeViewFolder.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.folderTreeViewFolder_ItemDrag);
            this.folderTreeViewFolder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.folderTreeView1_AfterSelect);
            this.folderTreeViewFolder.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.folderTreeViewFolder_NodeMouseClick);
            this.folderTreeViewFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.folderTreeViewFolder_DragDrop);
            this.folderTreeViewFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.folderTreeViewFolder_DragEnter);
            this.folderTreeViewFolder.DragOver += new System.Windows.Forms.DragEventHandler(this.folderTreeViewFolder_DragOver);
            this.folderTreeViewFolder.DragLeave += new System.EventHandler(this.folderTreeViewFolder_DragLeave);
            // 
            // contextMenuStripTreeViewFolder
            // 
            this.contextMenuStripTreeViewFolder.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripTreeViewFolder.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemTreeViewFolderCut,
            this.toolStripMenuItemTreeViewFolderCopy,
            this.toolStripMenuItemTreeViewFolderPaste,
            this.toolStripMenuItemTreeViewFolderDelete,
            this.toolStripMenuItemTreeViewFolderRefreshFolder,
            this.toolStripMenuItemTreeViewFolderReadSubfolders,
            this.toolStripMenuItemTreeViewFolderReload,
            this.toolStripMenuItemTreeViewFolderClearCache,
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata,
            this.openFolderLocationToolStripMenuItem});
            this.contextMenuStripTreeViewFolder.Name = "contextMenuStripImageListView";
            this.contextMenuStripTreeViewFolder.Size = new System.Drawing.Size(390, 264);
            // 
            // toolStripMenuItemTreeViewFolderCut
            // 
            this.toolStripMenuItemTreeViewFolderCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.Cut;
            this.toolStripMenuItemTreeViewFolderCut.Name = "toolStripMenuItemTreeViewFolderCut";
            this.toolStripMenuItemTreeViewFolderCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemTreeViewFolderCut.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemTreeViewFolderCut.Text = "Cut";
            this.toolStripMenuItemTreeViewFolderCut.Click += new System.EventHandler(this.toolStripMenuItemTreeViewFolderCut_Click);
            // 
            // toolStripMenuItemTreeViewFolderCopy
            // 
            this.toolStripMenuItemTreeViewFolderCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemTreeViewFolderCopy.Name = "toolStripMenuItemTreeViewFolderCopy";
            this.toolStripMenuItemTreeViewFolderCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemTreeViewFolderCopy.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemTreeViewFolderCopy.Text = "Copy";
            this.toolStripMenuItemTreeViewFolderCopy.Click += new System.EventHandler(this.toolStripMenuItemTreeViewFolderCopy_Click);
            // 
            // toolStripMenuItemTreeViewFolderPaste
            // 
            this.toolStripMenuItemTreeViewFolderPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.Paste;
            this.toolStripMenuItemTreeViewFolderPaste.Name = "toolStripMenuItemTreeViewFolderPaste";
            this.toolStripMenuItemTreeViewFolderPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItemTreeViewFolderPaste.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemTreeViewFolderPaste.Text = "Paste";
            this.toolStripMenuItemTreeViewFolderPaste.Click += new System.EventHandler(this.toolStripMenuItemTreeViewFolderPaste_Click);
            // 
            // toolStripMenuItemTreeViewFolderDelete
            // 
            this.toolStripMenuItemTreeViewFolderDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.Delete;
            this.toolStripMenuItemTreeViewFolderDelete.Name = "toolStripMenuItemTreeViewFolderDelete";
            this.toolStripMenuItemTreeViewFolderDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemTreeViewFolderDelete.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemTreeViewFolderDelete.Text = "Delete";
            this.toolStripMenuItemTreeViewFolderDelete.Click += new System.EventHandler(this.toolStripMenuItemTreeViewFolderDelete_Click);
            // 
            // toolStripMenuItemTreeViewFolderRefreshFolder
            // 
            this.toolStripMenuItemTreeViewFolderRefreshFolder.Image = global::PhotoTagsSynchronizer.Properties.Resources.RefreshFolder;
            this.toolStripMenuItemTreeViewFolderRefreshFolder.Name = "toolStripMenuItemTreeViewFolderRefreshFolder";
            this.toolStripMenuItemTreeViewFolderRefreshFolder.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.toolStripMenuItemTreeViewFolderRefreshFolder.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemTreeViewFolderRefreshFolder.Text = "Refresh folder";
            this.toolStripMenuItemTreeViewFolderRefreshFolder.Click += new System.EventHandler(this.toolStripMenuItemTreeViewFolderRefreshFolder_Click);
            // 
            // toolStripMenuItemTreeViewFolderReadSubfolders
            // 
            this.toolStripMenuItemTreeViewFolderReadSubfolders.Image = global::PhotoTagsSynchronizer.Properties.Resources.SubfoldersFolder;
            this.toolStripMenuItemTreeViewFolderReadSubfolders.Name = "toolStripMenuItemTreeViewFolderReadSubfolders";
            this.toolStripMenuItemTreeViewFolderReadSubfolders.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripMenuItemTreeViewFolderReadSubfolders.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemTreeViewFolderReadSubfolders.Text = "Read subfolders";
            this.toolStripMenuItemTreeViewFolderReadSubfolders.Click += new System.EventHandler(this.toolStripMenuItemTreeViewFolderReadSubfolders_Click);
            // 
            // toolStripMenuItemTreeViewFolderReload
            // 
            this.toolStripMenuItemTreeViewFolderReload.Image = global::PhotoTagsSynchronizer.Properties.Resources.Reload;
            this.toolStripMenuItemTreeViewFolderReload.Name = "toolStripMenuItemTreeViewFolderReload";
            this.toolStripMenuItemTreeViewFolderReload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.toolStripMenuItemTreeViewFolderReload.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemTreeViewFolderReload.Text = "Reload thumbnail and metadata";
            this.toolStripMenuItemTreeViewFolderReload.Click += new System.EventHandler(this.toolStripMenuItemTreeViewFolderReload_Click);
            // 
            // toolStripMenuItemTreeViewFolderClearCache
            // 
            this.toolStripMenuItemTreeViewFolderClearCache.Image = global::PhotoTagsSynchronizer.Properties.Resources.DeleteHistory;
            this.toolStripMenuItemTreeViewFolderClearCache.Name = "toolStripMenuItemTreeViewFolderClearCache";
            this.toolStripMenuItemTreeViewFolderClearCache.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.toolStripMenuItemTreeViewFolderClearCache.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemTreeViewFolderClearCache.Text = "Clear thumbnail and metadata history";
            this.toolStripMenuItemTreeViewFolderClearCache.Click += new System.EventHandler(this.toolStripMenuItemTreeViewFolderClearCache_Click);
            // 
            // toolStripMenuItemTreeViewFolderAutoCorrectMetadata
            // 
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.Image = global::PhotoTagsSynchronizer.Properties.Resources.AutoCorrect;
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.Name = "toolStripMenuItemTreeViewFolderAutoCorrectMetadata";
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.Text = "AutoCorrect metadata";
            this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata.Click += new System.EventHandler(this.toolStripMenuItemTreeViewFolderAutoCorrectMetadata_Click);
            // 
            // openFolderLocationToolStripMenuItem
            // 
            this.openFolderLocationToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileExplorer;
            this.openFolderLocationToolStripMenuItem.Name = "openFolderLocationToolStripMenuItem";
            this.openFolderLocationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.openFolderLocationToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.openFolderLocationToolStripMenuItem.Text = "Open Folder Location";
            this.openFolderLocationToolStripMenuItem.Click += new System.EventHandler(this.openFolderLocationToolStripMenuItem_Click);
            // 
            // tabPageFilterSearch
            // 
            this.tabPageFilterSearch.Controls.Add(this.panel4);
            this.tabPageFilterSearch.Location = new System.Drawing.Point(4, 25);
            this.tabPageFilterSearch.Name = "tabPageFilterSearch";
            this.tabPageFilterSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilterSearch.Size = new System.Drawing.Size(330, 863);
            this.tabPageFilterSearch.TabIndex = 2;
            this.tabPageFilterSearch.Text = "Search";
            this.tabPageFilterSearch.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkBoxSerachFitsAllValues);
            this.panel4.Controls.Add(this.panelSearchFilter);
            this.panel4.Controls.Add(this.buttonSearch);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(324, 857);
            this.panel4.TabIndex = 18;
            // 
            // checkBoxSerachFitsAllValues
            // 
            this.checkBoxSerachFitsAllValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSerachFitsAllValues.AutoSize = true;
            this.checkBoxSerachFitsAllValues.Checked = true;
            this.checkBoxSerachFitsAllValues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSerachFitsAllValues.Location = new System.Drawing.Point(3, 822);
            this.checkBoxSerachFitsAllValues.Name = "checkBoxSerachFitsAllValues";
            this.checkBoxSerachFitsAllValues.Size = new System.Drawing.Size(54, 21);
            this.checkBoxSerachFitsAllValues.TabIndex = 26;
            this.checkBoxSerachFitsAllValues.Text = "And";
            this.checkBoxSerachFitsAllValues.UseVisualStyleBackColor = true;
            // 
            // panelSearchFilter
            // 
            this.panelSearchFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearchFilter.AutoScroll = true;
            this.panelSearchFilter.AutoScrollMinSize = new System.Drawing.Size(319, 797);
            this.panelSearchFilter.Controls.Add(this.groupBoxSearchKeywords);
            this.panelSearchFilter.Controls.Add(this.groupBoxSearchRating);
            this.panelSearchFilter.Controls.Add(this.groupBoxSearchPeople);
            this.panelSearchFilter.Controls.Add(this.groupBoxSearchExtra);
            this.panelSearchFilter.Controls.Add(this.groupBoxSearchMediaTaken);
            this.panelSearchFilter.Controls.Add(this.groupBoxSearchTags);
            this.panelSearchFilter.Location = new System.Drawing.Point(0, 0);
            this.panelSearchFilter.Name = "panelSearchFilter";
            this.panelSearchFilter.Size = new System.Drawing.Size(319, 807);
            this.panelSearchFilter.TabIndex = 2;
            // 
            // groupBoxSearchKeywords
            // 
            this.groupBoxSearchKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchKeywords.Controls.Add(this.label9);
            this.groupBoxSearchKeywords.Controls.Add(this.checkBoxSearchNeedAllKeywords);
            this.groupBoxSearchKeywords.Controls.Add(this.label15);
            this.groupBoxSearchKeywords.Controls.Add(this.comboBoxSearchKeyword);
            this.groupBoxSearchKeywords.Controls.Add(this.checkBoxSearchWithoutKeyword);
            this.groupBoxSearchKeywords.Location = new System.Drawing.Point(2, 534);
            this.groupBoxSearchKeywords.MinimumSize = new System.Drawing.Size(313, 134);
            this.groupBoxSearchKeywords.Name = "groupBoxSearchKeywords";
            this.groupBoxSearchKeywords.Size = new System.Drawing.Size(313, 134);
            this.groupBoxSearchKeywords.TabIndex = 2;
            this.groupBoxSearchKeywords.TabStop = false;
            this.groupBoxSearchKeywords.Text = "Keywords";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(124, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(163, 17);
            this.label9.TabIndex = 44;
            this.label9.Text = "Separate keywords with ;";
            // 
            // checkBoxSearchNeedAllKeywords
            // 
            this.checkBoxSearchNeedAllKeywords.AutoSize = true;
            this.checkBoxSearchNeedAllKeywords.Checked = true;
            this.checkBoxSearchNeedAllKeywords.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchNeedAllKeywords.Location = new System.Drawing.Point(124, 76);
            this.checkBoxSearchNeedAllKeywords.Name = "checkBoxSearchNeedAllKeywords";
            this.checkBoxSearchNeedAllKeywords.Size = new System.Drawing.Size(194, 21);
            this.checkBoxSearchNeedAllKeywords.TabIndex = 43;
            this.checkBoxSearchNeedAllKeywords.Text = "When contain all keywords";
            this.checkBoxSearchNeedAllKeywords.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(4, 27);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 17);
            this.label15.TabIndex = 16;
            this.label15.Text = "Keyword:";
            // 
            // comboBoxSearchKeyword
            // 
            this.comboBoxSearchKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchKeyword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchKeyword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchKeyword.FormattingEnabled = true;
            this.comboBoxSearchKeyword.Location = new System.Drawing.Point(124, 24);
            this.comboBoxSearchKeyword.Name = "comboBoxSearchKeyword";
            this.comboBoxSearchKeyword.Size = new System.Drawing.Size(183, 25);
            this.comboBoxSearchKeyword.TabIndex = 28;
            // 
            // checkBoxSearchWithoutKeyword
            // 
            this.checkBoxSearchWithoutKeyword.AutoSize = true;
            this.checkBoxSearchWithoutKeyword.Location = new System.Drawing.Point(124, 103);
            this.checkBoxSearchWithoutKeyword.Name = "checkBoxSearchWithoutKeyword";
            this.checkBoxSearchWithoutKeyword.Size = new System.Drawing.Size(185, 21);
            this.checkBoxSearchWithoutKeyword.TabIndex = 41;
            this.checkBoxSearchWithoutKeyword.Text = "Or without any keywords";
            this.checkBoxSearchWithoutKeyword.UseVisualStyleBackColor = true;
            // 
            // groupBoxSearchRating
            // 
            this.groupBoxSearchRating.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchRating.Controls.Add(this.checkBoxSearchRatingEmpty);
            this.groupBoxSearchRating.Controls.Add(this.checkBoxSearchRating5);
            this.groupBoxSearchRating.Controls.Add(this.checkBoxSearchRating4);
            this.groupBoxSearchRating.Controls.Add(this.checkBoxSearchRating3);
            this.groupBoxSearchRating.Controls.Add(this.checkBoxSearchRating2);
            this.groupBoxSearchRating.Controls.Add(this.checkBoxSearchRating1);
            this.groupBoxSearchRating.Controls.Add(this.checkBoxSearchRating0);
            this.groupBoxSearchRating.Location = new System.Drawing.Point(3, 116);
            this.groupBoxSearchRating.MinimumSize = new System.Drawing.Size(313, 54);
            this.groupBoxSearchRating.Name = "groupBoxSearchRating";
            this.groupBoxSearchRating.Size = new System.Drawing.Size(313, 54);
            this.groupBoxSearchRating.TabIndex = 9;
            this.groupBoxSearchRating.TabStop = false;
            this.groupBoxSearchRating.Text = "Rating";
            // 
            // checkBoxSearchRatingEmpty
            // 
            this.checkBoxSearchRatingEmpty.AutoSize = true;
            this.checkBoxSearchRatingEmpty.Location = new System.Drawing.Point(284, 20);
            this.checkBoxSearchRatingEmpty.Name = "checkBoxSearchRatingEmpty";
            this.checkBoxSearchRatingEmpty.Size = new System.Drawing.Size(37, 21);
            this.checkBoxSearchRatingEmpty.TabIndex = 37;
            this.checkBoxSearchRatingEmpty.Text = "?";
            this.checkBoxSearchRatingEmpty.UseVisualStyleBackColor = true;
            // 
            // checkBoxSearchRating5
            // 
            this.checkBoxSearchRating5.AutoSize = true;
            this.checkBoxSearchRating5.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.checkBoxSearchRating5.Location = new System.Drawing.Point(200, 20);
            this.checkBoxSearchRating5.Name = "checkBoxSearchRating5";
            this.checkBoxSearchRating5.Size = new System.Drawing.Size(42, 24);
            this.checkBoxSearchRating5.TabIndex = 4;
            this.checkBoxSearchRating5.UseVisualStyleBackColor = true;
            // 
            // checkBoxSearchRating4
            // 
            this.checkBoxSearchRating4.AutoSize = true;
            this.checkBoxSearchRating4.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.checkBoxSearchRating4.Location = new System.Drawing.Point(152, 20);
            this.checkBoxSearchRating4.Name = "checkBoxSearchRating4";
            this.checkBoxSearchRating4.Size = new System.Drawing.Size(42, 24);
            this.checkBoxSearchRating4.TabIndex = 3;
            this.checkBoxSearchRating4.UseVisualStyleBackColor = true;
            // 
            // checkBoxSearchRating3
            // 
            this.checkBoxSearchRating3.AutoSize = true;
            this.checkBoxSearchRating3.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.checkBoxSearchRating3.Location = new System.Drawing.Point(104, 20);
            this.checkBoxSearchRating3.Name = "checkBoxSearchRating3";
            this.checkBoxSearchRating3.Size = new System.Drawing.Size(42, 24);
            this.checkBoxSearchRating3.TabIndex = 2;
            this.checkBoxSearchRating3.UseVisualStyleBackColor = true;
            // 
            // checkBoxSearchRating2
            // 
            this.checkBoxSearchRating2.AutoSize = true;
            this.checkBoxSearchRating2.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.checkBoxSearchRating2.Location = new System.Drawing.Point(56, 20);
            this.checkBoxSearchRating2.Name = "checkBoxSearchRating2";
            this.checkBoxSearchRating2.Size = new System.Drawing.Size(42, 24);
            this.checkBoxSearchRating2.TabIndex = 1;
            this.checkBoxSearchRating2.UseVisualStyleBackColor = true;
            // 
            // checkBoxSearchRating1
            // 
            this.checkBoxSearchRating1.AutoSize = true;
            this.checkBoxSearchRating1.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.checkBoxSearchRating1.Location = new System.Drawing.Point(8, 20);
            this.checkBoxSearchRating1.Name = "checkBoxSearchRating1";
            this.checkBoxSearchRating1.Size = new System.Drawing.Size(42, 24);
            this.checkBoxSearchRating1.TabIndex = 0;
            this.checkBoxSearchRating1.UseVisualStyleBackColor = true;
            // 
            // checkBoxSearchRating0
            // 
            this.checkBoxSearchRating0.AutoSize = true;
            this.checkBoxSearchRating0.Location = new System.Drawing.Point(248, 20);
            this.checkBoxSearchRating0.Name = "checkBoxSearchRating0";
            this.checkBoxSearchRating0.Size = new System.Drawing.Size(38, 21);
            this.checkBoxSearchRating0.TabIndex = 36;
            this.checkBoxSearchRating0.Text = "0";
            this.checkBoxSearchRating0.UseVisualStyleBackColor = true;
            // 
            // groupBoxSearchPeople
            // 
            this.groupBoxSearchPeople.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchPeople.Controls.Add(this.panel5);
            this.groupBoxSearchPeople.Location = new System.Drawing.Point(3, 674);
            this.groupBoxSearchPeople.MinimumSize = new System.Drawing.Size(313, 120);
            this.groupBoxSearchPeople.Name = "groupBoxSearchPeople";
            this.groupBoxSearchPeople.Size = new System.Drawing.Size(313, 130);
            this.groupBoxSearchPeople.TabIndex = 26;
            this.groupBoxSearchPeople.TabStop = false;
            this.groupBoxSearchPeople.Text = "People:";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.checkedListBoxSearchPeople);
            this.panel5.Controls.Add(this.checkBoxSearchWithoutRegions);
            this.panel5.Controls.Add(this.checkBoxSearchNeedAllNames);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 20);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(307, 107);
            this.panel5.TabIndex = 16;
            // 
            // checkedListBoxSearchPeople
            // 
            this.checkedListBoxSearchPeople.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxSearchPeople.FormattingEnabled = true;
            this.checkedListBoxSearchPeople.Location = new System.Drawing.Point(3, 62);
            this.checkedListBoxSearchPeople.Name = "checkedListBoxSearchPeople";
            this.checkedListBoxSearchPeople.Size = new System.Drawing.Size(300, 4);
            this.checkedListBoxSearchPeople.TabIndex = 0;
            // 
            // checkBoxSearchWithoutRegions
            // 
            this.checkBoxSearchWithoutRegions.AutoSize = true;
            this.checkBoxSearchWithoutRegions.Location = new System.Drawing.Point(121, 35);
            this.checkBoxSearchWithoutRegions.Name = "checkBoxSearchWithoutRegions";
            this.checkBoxSearchWithoutRegions.Size = new System.Drawing.Size(170, 21);
            this.checkBoxSearchWithoutRegions.TabIndex = 44;
            this.checkBoxSearchWithoutRegions.Text = "Or whitout any regions";
            this.checkBoxSearchWithoutRegions.UseVisualStyleBackColor = true;
            // 
            // checkBoxSearchNeedAllNames
            // 
            this.checkBoxSearchNeedAllNames.AutoSize = true;
            this.checkBoxSearchNeedAllNames.Checked = true;
            this.checkBoxSearchNeedAllNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchNeedAllNames.Location = new System.Drawing.Point(121, 8);
            this.checkBoxSearchNeedAllNames.Name = "checkBoxSearchNeedAllNames";
            this.checkBoxSearchNeedAllNames.Size = new System.Drawing.Size(175, 21);
            this.checkBoxSearchNeedAllNames.TabIndex = 43;
            this.checkBoxSearchNeedAllNames.Text = "When contain all names";
            this.checkBoxSearchNeedAllNames.UseVisualStyleBackColor = true;
            // 
            // groupBoxSearchExtra
            // 
            this.groupBoxSearchExtra.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchExtra.Controls.Add(this.checkBoxSearchHasWarning);
            this.groupBoxSearchExtra.Location = new System.Drawing.Point(3, 480);
            this.groupBoxSearchExtra.Name = "groupBoxSearchExtra";
            this.groupBoxSearchExtra.Size = new System.Drawing.Size(313, 48);
            this.groupBoxSearchExtra.TabIndex = 2;
            this.groupBoxSearchExtra.TabStop = false;
            this.groupBoxSearchExtra.Text = "Attributes:";
            // 
            // checkBoxSearchHasWarning
            // 
            this.checkBoxSearchHasWarning.AutoSize = true;
            this.checkBoxSearchHasWarning.Location = new System.Drawing.Point(124, 23);
            this.checkBoxSearchHasWarning.Name = "checkBoxSearchHasWarning";
            this.checkBoxSearchHasWarning.Size = new System.Drawing.Size(177, 21);
            this.checkBoxSearchHasWarning.TabIndex = 29;
            this.checkBoxSearchHasWarning.Text = "Has warning message(s)";
            this.checkBoxSearchHasWarning.UseVisualStyleBackColor = true;
            // 
            // groupBoxSearchMediaTaken
            // 
            this.groupBoxSearchMediaTaken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchMediaTaken.Controls.Add(this.checkBoxSearchMediaTakenIsNull);
            this.groupBoxSearchMediaTaken.Controls.Add(this.dateTimePickerSearchDateFrom);
            this.groupBoxSearchMediaTaken.Controls.Add(this.label14);
            this.groupBoxSearchMediaTaken.Controls.Add(this.label17);
            this.groupBoxSearchMediaTaken.Controls.Add(this.dateTimePickerSearchDateTo);
            this.groupBoxSearchMediaTaken.Location = new System.Drawing.Point(3, 3);
            this.groupBoxSearchMediaTaken.MinimumSize = new System.Drawing.Size(313, 107);
            this.groupBoxSearchMediaTaken.Name = "groupBoxSearchMediaTaken";
            this.groupBoxSearchMediaTaken.Size = new System.Drawing.Size(313, 107);
            this.groupBoxSearchMediaTaken.TabIndex = 3;
            this.groupBoxSearchMediaTaken.TabStop = false;
            this.groupBoxSearchMediaTaken.Text = "Media taken:";
            // 
            // checkBoxSearchMediaTakenIsNull
            // 
            this.checkBoxSearchMediaTakenIsNull.AutoSize = true;
            this.checkBoxSearchMediaTakenIsNull.Location = new System.Drawing.Point(124, 86);
            this.checkBoxSearchMediaTakenIsNull.Name = "checkBoxSearchMediaTakenIsNull";
            this.checkBoxSearchMediaTakenIsNull.Size = new System.Drawing.Size(166, 21);
            this.checkBoxSearchMediaTakenIsNull.TabIndex = 32;
            this.checkBoxSearchMediaTakenIsNull.Text = "Or when missing value";
            this.checkBoxSearchMediaTakenIsNull.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerSearchDateFrom
            // 
            this.dateTimePickerSearchDateFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerSearchDateFrom.Checked = false;
            this.dateTimePickerSearchDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerSearchDateFrom.Location = new System.Drawing.Point(124, 23);
            this.dateTimePickerSearchDateFrom.MaxDate = new System.DateTime(2200, 12, 31, 0, 0, 0, 0);
            this.dateTimePickerSearchDateFrom.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerSearchDateFrom.Name = "dateTimePickerSearchDateFrom";
            this.dateTimePickerSearchDateFrom.ShowCheckBox = true;
            this.dateTimePickerSearchDateFrom.Size = new System.Drawing.Size(183, 24);
            this.dateTimePickerSearchDateFrom.TabIndex = 14;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 29);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(103, 17);
            this.label14.TabIndex = 15;
            this.label14.Text = "DateTaken >=:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 59);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(103, 17);
            this.label17.TabIndex = 30;
            this.label17.Text = "DateTaken <=:";
            // 
            // dateTimePickerSearchDateTo
            // 
            this.dateTimePickerSearchDateTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerSearchDateTo.Checked = false;
            this.dateTimePickerSearchDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerSearchDateTo.Location = new System.Drawing.Point(124, 53);
            this.dateTimePickerSearchDateTo.MaxDate = new System.DateTime(2200, 12, 31, 0, 0, 0, 0);
            this.dateTimePickerSearchDateTo.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerSearchDateTo.Name = "dateTimePickerSearchDateTo";
            this.dateTimePickerSearchDateTo.ShowCheckBox = true;
            this.dateTimePickerSearchDateTo.Size = new System.Drawing.Size(183, 24);
            this.dateTimePickerSearchDateTo.TabIndex = 31;
            // 
            // groupBoxSearchTags
            // 
            this.groupBoxSearchTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearchTags.Controls.Add(this.checkBoxSearchUseAndBetweenTextTagFields);
            this.groupBoxSearchTags.Controls.Add(this.comboBoxSearchComments);
            this.groupBoxSearchTags.Controls.Add(this.label8);
            this.groupBoxSearchTags.Controls.Add(this.label7);
            this.groupBoxSearchTags.Controls.Add(this.label6);
            this.groupBoxSearchTags.Controls.Add(this.label3);
            this.groupBoxSearchTags.Controls.Add(this.comboBoxSearchAlbum);
            this.groupBoxSearchTags.Controls.Add(this.comboBoxSearchTitle);
            this.groupBoxSearchTags.Controls.Add(this.comboBoxSearchDescription);
            this.groupBoxSearchTags.Controls.Add(this.comboBoxSearchLocationCountry);
            this.groupBoxSearchTags.Controls.Add(this.comboBoxSearchLocationName);
            this.groupBoxSearchTags.Controls.Add(this.comboBoxSearchLocationState);
            this.groupBoxSearchTags.Controls.Add(this.label13);
            this.groupBoxSearchTags.Controls.Add(this.comboBoxSearchLocationCity);
            this.groupBoxSearchTags.Controls.Add(this.label12);
            this.groupBoxSearchTags.Controls.Add(this.label11);
            this.groupBoxSearchTags.Controls.Add(this.label10);
            this.groupBoxSearchTags.Location = new System.Drawing.Point(3, 175);
            this.groupBoxSearchTags.MinimumSize = new System.Drawing.Size(313, 271);
            this.groupBoxSearchTags.Name = "groupBoxSearchTags";
            this.groupBoxSearchTags.Size = new System.Drawing.Size(313, 299);
            this.groupBoxSearchTags.TabIndex = 2;
            this.groupBoxSearchTags.TabStop = false;
            this.groupBoxSearchTags.Text = "Search inside text field tags:";
            // 
            // checkBoxSearchUseAndBetweenTextTagFields
            // 
            this.checkBoxSearchUseAndBetweenTextTagFields.AutoSize = true;
            this.checkBoxSearchUseAndBetweenTextTagFields.Location = new System.Drawing.Point(124, 272);
            this.checkBoxSearchUseAndBetweenTextTagFields.Name = "checkBoxSearchUseAndBetweenTextTagFields";
            this.checkBoxSearchUseAndBetweenTextTagFields.Size = new System.Drawing.Size(108, 21);
            this.checkBoxSearchUseAndBetweenTextTagFields.TabIndex = 26;
            this.checkBoxSearchUseAndBetweenTextTagFields.Text = "Need all to fit";
            this.checkBoxSearchUseAndBetweenTextTagFields.UseVisualStyleBackColor = true;
            // 
            // comboBoxSearchComments
            // 
            this.comboBoxSearchComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchComments.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchComments.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchComments.FormattingEnabled = true;
            this.comboBoxSearchComments.Location = new System.Drawing.Point(124, 114);
            this.comboBoxSearchComments.Name = "comboBoxSearchComments";
            this.comboBoxSearchComments.Size = new System.Drawing.Size(183, 25);
            this.comboBoxSearchComments.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 17);
            this.label8.TabIndex = 4;
            this.label8.Text = "Comments:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 17);
            this.label7.TabIndex = 3;
            this.label7.Text = "Description:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "Title:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Album:";
            // 
            // comboBoxSearchAlbum
            // 
            this.comboBoxSearchAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchAlbum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchAlbum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchAlbum.FormattingEnabled = true;
            this.comboBoxSearchAlbum.Location = new System.Drawing.Point(124, 23);
            this.comboBoxSearchAlbum.Name = "comboBoxSearchAlbum";
            this.comboBoxSearchAlbum.Size = new System.Drawing.Size(183, 25);
            this.comboBoxSearchAlbum.TabIndex = 17;
            // 
            // comboBoxSearchTitle
            // 
            this.comboBoxSearchTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchTitle.FormattingEnabled = true;
            this.comboBoxSearchTitle.Location = new System.Drawing.Point(124, 52);
            this.comboBoxSearchTitle.Name = "comboBoxSearchTitle";
            this.comboBoxSearchTitle.Size = new System.Drawing.Size(183, 25);
            this.comboBoxSearchTitle.TabIndex = 18;
            // 
            // comboBoxSearchDescription
            // 
            this.comboBoxSearchDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchDescription.FormattingEnabled = true;
            this.comboBoxSearchDescription.Location = new System.Drawing.Point(124, 83);
            this.comboBoxSearchDescription.Name = "comboBoxSearchDescription";
            this.comboBoxSearchDescription.Size = new System.Drawing.Size(183, 25);
            this.comboBoxSearchDescription.TabIndex = 19;
            // 
            // comboBoxSearchLocationCountry
            // 
            this.comboBoxSearchLocationCountry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchLocationCountry.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchLocationCountry.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchLocationCountry.FormattingEnabled = true;
            this.comboBoxSearchLocationCountry.Location = new System.Drawing.Point(124, 238);
            this.comboBoxSearchLocationCountry.Name = "comboBoxSearchLocationCountry";
            this.comboBoxSearchLocationCountry.Size = new System.Drawing.Size(183, 25);
            this.comboBoxSearchLocationCountry.TabIndex = 25;
            // 
            // comboBoxSearchLocationName
            // 
            this.comboBoxSearchLocationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchLocationName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchLocationName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchLocationName.FormattingEnabled = true;
            this.comboBoxSearchLocationName.Location = new System.Drawing.Point(124, 145);
            this.comboBoxSearchLocationName.Name = "comboBoxSearchLocationName";
            this.comboBoxSearchLocationName.Size = new System.Drawing.Size(183, 25);
            this.comboBoxSearchLocationName.TabIndex = 22;
            // 
            // comboBoxSearchLocationState
            // 
            this.comboBoxSearchLocationState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchLocationState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchLocationState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchLocationState.FormattingEnabled = true;
            this.comboBoxSearchLocationState.Location = new System.Drawing.Point(124, 207);
            this.comboBoxSearchLocationState.Name = "comboBoxSearchLocationState";
            this.comboBoxSearchLocationState.Size = new System.Drawing.Size(183, 25);
            this.comboBoxSearchLocationState.TabIndex = 24;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 148);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 17);
            this.label13.TabIndex = 10;
            this.label13.Text = "Location:";
            // 
            // comboBoxSearchLocationCity
            // 
            this.comboBoxSearchLocationCity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchLocationCity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSearchLocationCity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSearchLocationCity.FormattingEnabled = true;
            this.comboBoxSearchLocationCity.Location = new System.Drawing.Point(124, 176);
            this.comboBoxSearchLocationCity.Name = "comboBoxSearchLocationCity";
            this.comboBoxSearchLocationCity.Size = new System.Drawing.Size(183, 25);
            this.comboBoxSearchLocationCity.TabIndex = 23;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 179);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 17);
            this.label12.TabIndex = 11;
            this.label12.Text = "City:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 210);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 17);
            this.label11.TabIndex = 12;
            this.label11.Text = "State:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 241);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 17);
            this.label10.TabIndex = 13;
            this.label10.Text = "Country:";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(118, 813);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(202, 37);
            this.buttonSearch.TabIndex = 42;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonFilterSearch_Click);
            // 
            // tabPageFilterTags
            // 
            this.tabPageFilterTags.Controls.Add(this.treeViewFilter);
            this.tabPageFilterTags.Location = new System.Drawing.Point(4, 25);
            this.tabPageFilterTags.Name = "tabPageFilterTags";
            this.tabPageFilterTags.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilterTags.Size = new System.Drawing.Size(330, 863);
            this.tabPageFilterTags.TabIndex = 1;
            this.tabPageFilterTags.Text = "Filter";
            this.tabPageFilterTags.UseVisualStyleBackColor = true;
            // 
            // treeViewFilter
            // 
            this.treeViewFilter.CheckBoxes = true;
            this.treeViewFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewFilter.Location = new System.Drawing.Point(3, 3);
            this.treeViewFilter.Name = "treeViewFilter";
            treeNode1.Name = "NodeFolder";
            treeNode1.Tag = "Filter";
            treeNode1.Text = "Filter";
            this.treeViewFilter.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeViewFilter.Size = new System.Drawing.Size(324, 857);
            this.treeViewFilter.TabIndex = 0;
            this.treeViewFilter.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewFilter_BeforeCheck);
            this.treeViewFilter.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFilter_AfterCheck);
            // 
            // splitContainerImages
            // 
            this.splitContainerImages.BackColor = System.Drawing.Color.Black;
            this.splitContainerImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerImages.Location = new System.Drawing.Point(0, 0);
            this.splitContainerImages.Name = "splitContainerImages";
            // 
            // splitContainerImages.Panel1
            // 
            this.splitContainerImages.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerImages.Panel1.Controls.Add(this.imageListView1);
            // 
            // splitContainerImages.Panel2
            // 
            this.splitContainerImages.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerImages.Panel2.Controls.Add(this.panel1);
            this.splitContainerImages.Size = new System.Drawing.Size(1039, 892);
            this.splitContainerImages.SplitterDistance = 484;
            this.splitContainerImages.SplitterWidth = 10;
            this.splitContainerImages.TabIndex = 0;
            this.splitContainerImages.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerImages_SplitterMoved);
            // 
            // imageListView1
            // 
            this.imageListView1.AllowDrag = true;
            this.imageListView1.AllowDrop = true;
            this.imageListView1.CacheLimit = "0";
            this.imageListView1.CacheMode = Manina.Windows.Forms.CacheMode.Continuous;
            this.imageListView1.ContextMenuStrip = this.contextMenuStripImageListView;
            this.imageListView1.DefaultImage = ((System.Drawing.Image)(resources.GetObject("imageListView1.DefaultImage")));
            this.imageListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListView1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("imageListView1.ErrorImage")));
            this.imageListView1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.imageListView1.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.imageListView1.Location = new System.Drawing.Point(0, 0);
            this.imageListView1.Name = "imageListView1";
            this.imageListView1.RetryOnError = false;
            this.imageListView1.Size = new System.Drawing.Size(484, 892);
            this.imageListView1.TabIndex = 1;
            this.imageListView1.Text = "";
            this.imageListView1.ItemHover += new Manina.Windows.Forms.ItemHoverEventHandler(this.imageListView1_ItemHover);
            this.imageListView1.ItemDoubleClick += new Manina.Windows.Forms.ItemDoubleClickEventHandler(this.imageListView1_ItemDoubleClick);
            this.imageListView1.SelectionChanged += new System.EventHandler(this.imageListView1_SelectionChanged);
            this.imageListView1.ThumbnailCaching += new Manina.Windows.Forms.ThumbnailCachingEventHandler(this.imageListView1_ThumbnailCaching);
            this.imageListView1.RetrieveItemImage += new Manina.Windows.Forms.RetrieveItemImageEventHandler(this.imageListView1_RetrieveImage);
            this.imageListView1.RetrieveItemThumbnail += new Manina.Windows.Forms.RetrieveItemThumbnailEventHandler(this.imageListView1_RetrieveItemThumbnail);
            this.imageListView1.RetrieveItemMetadataDetails += new Manina.Windows.Forms.RetrieveItemMetadataDetailsEventHandler(this.imageListView1_RetrieveItemMetadataDetails);
            // 
            // contextMenuStripImageListView
            // 
            this.contextMenuStripImageListView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripImageListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemImageListViewCut,
            this.toolStripMenuItemImageListViewCopy,
            this.copyFileNamesToClipboardToolStripMenuItem,
            this.toolStripMenuItemImageListViewPaste,
            this.toolStripMenuItemImageListViewDelete,
            this.toolStripMenuItemImageListViewRefreshFolder,
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata,
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory,
            this.toolStripMenuItemImageListViewSelectAll,
            this.toolStripMenuItemImageListViewAutoCorrect,
            this.openFileWithAssociatedApplicationToolStripMenuItem,
            this.openMediaFilesWithToolStripMenuItem,
            this.editFileWithAssociatedApplicationToolStripMenuItem,
            this.runSelectedToolStripMenuItem1,
            this.openWithDialogToolStripMenuItem,
            this.openFileLocationToolStripMenuItem,
            this.rotateCW90ToolStripMenuItem,
            this.rotate180ToolStripMenuItem,
            this.ratateCCW270ToolStripMenuItem,
            this.mediaPreviewToolStripMenuItem});
            this.contextMenuStripImageListView.Name = "contextMenuStripImageListView";
            this.contextMenuStripImageListView.Size = new System.Drawing.Size(390, 524);
            // 
            // toolStripMenuItemImageListViewCut
            // 
            this.toolStripMenuItemImageListViewCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.Cut;
            this.toolStripMenuItemImageListViewCut.Name = "toolStripMenuItemImageListViewCut";
            this.toolStripMenuItemImageListViewCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemImageListViewCut.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemImageListViewCut.Text = "Cut";
            this.toolStripMenuItemImageListViewCut.Click += new System.EventHandler(this.toolStripMenuItemImageListViewCut_Click);
            // 
            // toolStripMenuItemImageListViewCopy
            // 
            this.toolStripMenuItemImageListViewCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemImageListViewCopy.Name = "toolStripMenuItemImageListViewCopy";
            this.toolStripMenuItemImageListViewCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemImageListViewCopy.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemImageListViewCopy.Text = "Copy";
            this.toolStripMenuItemImageListViewCopy.Click += new System.EventHandler(this.toolStripMenuItemImageListViewCopy_Click);
            // 
            // copyFileNamesToClipboardToolStripMenuItem
            // 
            this.copyFileNamesToClipboardToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.copyFileNamesToClipboardToolStripMenuItem.Name = "copyFileNamesToClipboardToolStripMenuItem";
            this.copyFileNamesToClipboardToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.copyFileNamesToClipboardToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.copyFileNamesToClipboardToolStripMenuItem.Text = "Copy file names to Clipboard";
            this.copyFileNamesToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyFileNamesToClipboardToolStripMenuItem_Click);
            // 
            // toolStripMenuItemImageListViewPaste
            // 
            this.toolStripMenuItemImageListViewPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.Paste;
            this.toolStripMenuItemImageListViewPaste.Name = "toolStripMenuItemImageListViewPaste";
            this.toolStripMenuItemImageListViewPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItemImageListViewPaste.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemImageListViewPaste.Text = "Paste";
            this.toolStripMenuItemImageListViewPaste.Click += new System.EventHandler(this.toolStripMenuItemImageListViewPaste_Click);
            // 
            // toolStripMenuItemImageListViewDelete
            // 
            this.toolStripMenuItemImageListViewDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.Delete;
            this.toolStripMenuItemImageListViewDelete.Name = "toolStripMenuItemImageListViewDelete";
            this.toolStripMenuItemImageListViewDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemImageListViewDelete.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemImageListViewDelete.Text = "Delete";
            this.toolStripMenuItemImageListViewDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // toolStripMenuItemImageListViewRefreshFolder
            // 
            this.toolStripMenuItemImageListViewRefreshFolder.Image = global::PhotoTagsSynchronizer.Properties.Resources.RefreshFolder;
            this.toolStripMenuItemImageListViewRefreshFolder.Name = "toolStripMenuItemImageListViewRefreshFolder";
            this.toolStripMenuItemImageListViewRefreshFolder.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.toolStripMenuItemImageListViewRefreshFolder.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemImageListViewRefreshFolder.Text = "Refresh folder";
            this.toolStripMenuItemImageListViewRefreshFolder.Click += new System.EventHandler(this.toolStripMenuItemRefreshFolder_Click);
            // 
            // toolStripMenuItemImageListViewReloadThumbnailAndMetadata
            // 
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Image = global::PhotoTagsSynchronizer.Properties.Resources.Reload;
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Name = "toolStripMenuItemImageListViewReloadThumbnailAndMetadata";
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Text = "Reload thumbnail and metadata";
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadata.Click += new System.EventHandler(this.toolStripMenuItemReloadThumbnailAndMetadata_Click);
            // 
            // toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory
            // 
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Image = global::PhotoTagsSynchronizer.Properties.Resources.DeleteHistory;
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Name = "toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadata" +
    "History";
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Text = "Clear thumbnail and metadata history";
            this.toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory.Click += new System.EventHandler(this.toolStripMenuItemReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory_Click);
            // 
            // toolStripMenuItemImageListViewSelectAll
            // 
            this.toolStripMenuItemImageListViewSelectAll.Image = global::PhotoTagsSynchronizer.Properties.Resources.SelectAll;
            this.toolStripMenuItemImageListViewSelectAll.Name = "toolStripMenuItemImageListViewSelectAll";
            this.toolStripMenuItemImageListViewSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.toolStripMenuItemImageListViewSelectAll.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemImageListViewSelectAll.Text = "Select All";
            this.toolStripMenuItemImageListViewSelectAll.Click += new System.EventHandler(this.toolStripMenuItemSelectAll_Click);
            // 
            // toolStripMenuItemImageListViewAutoCorrect
            // 
            this.toolStripMenuItemImageListViewAutoCorrect.Image = global::PhotoTagsSynchronizer.Properties.Resources.AutoCorrect;
            this.toolStripMenuItemImageListViewAutoCorrect.Name = "toolStripMenuItemImageListViewAutoCorrect";
            this.toolStripMenuItemImageListViewAutoCorrect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.T)));
            this.toolStripMenuItemImageListViewAutoCorrect.Size = new System.Drawing.Size(389, 26);
            this.toolStripMenuItemImageListViewAutoCorrect.Text = "AutoCorrect metadata";
            this.toolStripMenuItemImageListViewAutoCorrect.Click += new System.EventHandler(this.toolStripMenuItemImageListViewAutoCorrect_Click);
            // 
            // openFileWithAssociatedApplicationToolStripMenuItem
            // 
            this.openFileWithAssociatedApplicationToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.Image_Open;
            this.openFileWithAssociatedApplicationToolStripMenuItem.Name = "openFileWithAssociatedApplicationToolStripMenuItem";
            this.openFileWithAssociatedApplicationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.openFileWithAssociatedApplicationToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.openFileWithAssociatedApplicationToolStripMenuItem.Text = "Open";
            this.openFileWithAssociatedApplicationToolStripMenuItem.Click += new System.EventHandler(this.openFileWithAssociatedApplicationToolStripMenuItem_Click);
            // 
            // openMediaFilesWithToolStripMenuItem
            // 
            this.openMediaFilesWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.openMediaFilesWithToolStripMenuItem.Name = "openMediaFilesWithToolStripMenuItem";
            this.openMediaFilesWithToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.openMediaFilesWithToolStripMenuItem.Text = "Open media files with...";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(118, 26);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // editFileWithAssociatedApplicationToolStripMenuItem
            // 
            this.editFileWithAssociatedApplicationToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.Image_Edit;
            this.editFileWithAssociatedApplicationToolStripMenuItem.Name = "editFileWithAssociatedApplicationToolStripMenuItem";
            this.editFileWithAssociatedApplicationToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.editFileWithAssociatedApplicationToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.editFileWithAssociatedApplicationToolStripMenuItem.Text = "Edit";
            this.editFileWithAssociatedApplicationToolStripMenuItem.Click += new System.EventHandler(this.editFileWithAssociatedApplicationToolStripMenuItem_Click);
            // 
            // runSelectedToolStripMenuItem1
            // 
            this.runSelectedToolStripMenuItem1.Image = global::PhotoTagsSynchronizer.Properties.Resources.Run_Command;
            this.runSelectedToolStripMenuItem1.Name = "runSelectedToolStripMenuItem1";
            this.runSelectedToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.runSelectedToolStripMenuItem1.Size = new System.Drawing.Size(389, 26);
            this.runSelectedToolStripMenuItem1.Text = "Run batch app or command...";
            this.runSelectedToolStripMenuItem1.Click += new System.EventHandler(this.runSelectedLocationToolStripMenuItem_Click);
            // 
            // openWithDialogToolStripMenuItem
            // 
            this.openWithDialogToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.Association_Filetype;
            this.openWithDialogToolStripMenuItem.Name = "openWithDialogToolStripMenuItem";
            this.openWithDialogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openWithDialogToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.openWithDialogToolStripMenuItem.Text = "Open and associate with dialog...";
            this.openWithDialogToolStripMenuItem.Click += new System.EventHandler(this.openWithDialogToolStripMenuItem_Click);
            // 
            // openFileLocationToolStripMenuItem
            // 
            this.openFileLocationToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FileExplorer;
            this.openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            this.openFileLocationToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.L)));
            this.openFileLocationToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.openFileLocationToolStripMenuItem.Text = "Open file Location";
            this.openFileLocationToolStripMenuItem.Click += new System.EventHandler(this.openFileLocationToolStripMenuItem_Click);
            // 
            // mediaPreviewToolStripMenuItem
            // 
            this.mediaPreviewToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Preview;
            this.mediaPreviewToolStripMenuItem.Name = "mediaPreviewToolStripMenuItem";
            this.mediaPreviewToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mediaPreviewToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.mediaPreviewToolStripMenuItem.Text = "Media preview";
            this.mediaPreviewToolStripMenuItem.Click += new System.EventHandler(this.mediaPreviewToolStripMenuItem_Click);
            // 
            // rotateCW90ToolStripMenuItem
            // 
            this.rotateCW90ToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.Rotate90CW;
            this.rotateCW90ToolStripMenuItem.Name = "rotateCW90ToolStripMenuItem";
            this.rotateCW90ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D9)));
            this.rotateCW90ToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.rotateCW90ToolStripMenuItem.Text = "Rotate CW - 90°";
            this.rotateCW90ToolStripMenuItem.ToolTipText = "Rotate CW - 90°";
            this.rotateCW90ToolStripMenuItem.Click += new System.EventHandler(this.rotateCW90ToolStripMenuItem_Click);
            // 
            // rotate180ToolStripMenuItem
            // 
            this.rotate180ToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.Rotate180;
            this.rotate180ToolStripMenuItem.Name = "rotate180ToolStripMenuItem";
            this.rotate180ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.rotate180ToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.rotate180ToolStripMenuItem.Text = "Rotate 180°";
            this.rotate180ToolStripMenuItem.ToolTipText = "Rotate 180°";
            this.rotate180ToolStripMenuItem.Click += new System.EventHandler(this.rotate180ToolStripMenuItem_Click);
            // 
            // ratateCCW270ToolStripMenuItem
            // 
            this.ratateCCW270ToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.Rotate90CCW;
            this.ratateCCW270ToolStripMenuItem.Name = "ratateCCW270ToolStripMenuItem";
            this.ratateCCW270ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.ratateCCW270ToolStripMenuItem.Size = new System.Drawing.Size(389, 26);
            this.ratateCCW270ToolStripMenuItem.Text = "Ratate CCW - 270°";
            this.ratateCCW270ToolStripMenuItem.ToolTipText = "Ratate CCW - 270°";
            this.ratateCCW270ToolStripMenuItem.Click += new System.EventHandler(this.ratateCCW270ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tabControlToolbox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(545, 892);
            this.panel1.TabIndex = 0;
            // 
            // tabControlToolbox
            // 
            this.tabControlToolbox.Controls.Add(this.tabPageTags);
            this.tabControlToolbox.Controls.Add(this.tabPagePeople);
            this.tabControlToolbox.Controls.Add(this.tabPageMap);
            this.tabControlToolbox.Controls.Add(this.tabPageDate);
            this.tabControlToolbox.Controls.Add(this.tabPageExifTool);
            this.tabControlToolbox.Controls.Add(this.tabPageExifToolWarning);
            this.tabControlToolbox.Controls.Add(this.tabPageFileProperties);
            this.tabControlToolbox.Controls.Add(this.tabPageFileRename);
            this.tabControlToolbox.Controls.Add(this.tabPageMediaConverter);
            this.tabControlToolbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlToolbox.Location = new System.Drawing.Point(0, 0);
            this.tabControlToolbox.Name = "tabControlToolbox";
            this.tabControlToolbox.SelectedIndex = 0;
            this.tabControlToolbox.Size = new System.Drawing.Size(545, 892);
            this.tabControlToolbox.TabIndex = 0;
            this.tabControlToolbox.SelectedIndexChanged += new System.EventHandler(this.tabControlToolbox_SelectedIndexChanged);
            this.tabControlToolbox.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControlToolbox_Selecting);
            // 
            // tabPageTags
            // 
            this.tabPageTags.Controls.Add(this.labelTagsInformation);
            this.tabPageTags.Controls.Add(this.labelAuthor);
            this.tabPageTags.Controls.Add(this.comboBoxAuthor);
            this.tabPageTags.Controls.Add(this.comboBoxMediaAiConfidence);
            this.tabPageTags.Controls.Add(this.dataGridViewTagsAndKeywords);
            this.tabPageTags.Controls.Add(this.label5);
            this.tabPageTags.Controls.Add(this.groupBoxRating);
            this.tabPageTags.Controls.Add(this.comboBoxAlbum);
            this.tabPageTags.Controls.Add(this.label4);
            this.tabPageTags.Controls.Add(this.comboBoxComments);
            this.tabPageTags.Controls.Add(this.labelComments);
            this.tabPageTags.Controls.Add(this.labelDescription);
            this.tabPageTags.Controls.Add(this.labelTitle);
            this.tabPageTags.Controls.Add(this.comboBoxDescription);
            this.tabPageTags.Controls.Add(this.comboBoxTitle);
            this.tabPageTags.Location = new System.Drawing.Point(4, 26);
            this.tabPageTags.Name = "tabPageTags";
            this.tabPageTags.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTags.Size = new System.Drawing.Size(537, 862);
            this.tabPageTags.TabIndex = 1;
            this.tabPageTags.Tag = "Tags";
            this.tabPageTags.Text = "Tags";
            this.tabPageTags.ToolTipText = "Metadata tags";
            this.tabPageTags.UseVisualStyleBackColor = true;
            // 
            // labelTagsInformation
            // 
            this.labelTagsInformation.AutoSize = true;
            this.labelTagsInformation.Location = new System.Drawing.Point(292, 152);
            this.labelTagsInformation.Name = "labelTagsInformation";
            this.labelTagsInformation.Size = new System.Drawing.Size(493, 17);
            this.labelTagsInformation.TabIndex = 15;
            this.labelTagsInformation.Text = "Use right mouse button, Context Menu key or Shift-F10 to open context menu";
            // 
            // labelAuthor
            // 
            this.labelAuthor.AutoSize = true;
            this.labelAuthor.Location = new System.Drawing.Point(6, 118);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(50, 17);
            this.labelAuthor.TabIndex = 13;
            this.labelAuthor.Text = "Author";
            // 
            // comboBoxAuthor
            // 
            this.comboBoxAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAuthor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxAuthor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxAuthor.FormattingEnabled = true;
            this.comboBoxAuthor.Location = new System.Drawing.Point(88, 115);
            this.comboBoxAuthor.Name = "comboBoxAuthor";
            this.comboBoxAuthor.Size = new System.Drawing.Size(440, 25);
            this.comboBoxAuthor.TabIndex = 12;
            this.comboBoxAuthor.TextChanged += new System.EventHandler(this.comboBoxAuthor_TextChanged);
            // 
            // comboBoxMediaAiConfidence
            // 
            this.comboBoxMediaAiConfidence.FormattingEnabled = true;
            this.comboBoxMediaAiConfidence.Items.AddRange(new object[] {
            "90% Confidence",
            "80% Confidence",
            "70% Confidence",
            "60% Confidence",
            "50% Confidence",
            "40% Confidence",
            "30% Confidence",
            "20% Confidence",
            "10% Confidence"});
            this.comboBoxMediaAiConfidence.Location = new System.Drawing.Point(88, 182);
            this.comboBoxMediaAiConfidence.Name = "comboBoxMediaAiConfidence";
            this.comboBoxMediaAiConfidence.Size = new System.Drawing.Size(198, 25);
            this.comboBoxMediaAiConfidence.TabIndex = 11;
            this.comboBoxMediaAiConfidence.SelectedIndexChanged += new System.EventHandler(this.comboBoxMediaAiConfidence_SelectedIndexChanged);
            // 
            // dataGridViewTagsAndKeywords
            // 
            this.dataGridViewTagsAndKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTagsAndKeywords.ColumnHeadersHeight = 200;
            this.dataGridViewTagsAndKeywords.ContextMenuStrip = this.contextMenuStripTagsAndKeywords;
            this.dataGridViewTagsAndKeywords.Location = new System.Drawing.Point(3, 213);
            this.dataGridViewTagsAndKeywords.Name = "dataGridViewTagsAndKeywords";
            this.dataGridViewTagsAndKeywords.RowHeadersWidth = 51;
            this.dataGridViewTagsAndKeywords.RowTemplate.Height = 24;
            this.dataGridViewTagsAndKeywords.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTagsAndKeywords.ShowCellErrors = false;
            this.dataGridViewTagsAndKeywords.ShowRowErrors = false;
            this.dataGridViewTagsAndKeywords.Size = new System.Drawing.Size(531, 626);
            this.dataGridViewTagsAndKeywords.TabIndex = 10;
            this.dataGridViewTagsAndKeywords.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewTagsAndKeywords_CellBeginEdit);
            this.dataGridViewTagsAndKeywords.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTagsAndKeywords_CellMouseClick);
            this.dataGridViewTagsAndKeywords.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewTagsAndKeywords_CellPainting);
            this.dataGridViewTagsAndKeywords.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTagsAndKeywords_CellValueChanged);
            this.dataGridViewTagsAndKeywords.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridViewTagsAndKeywords_RowsAdded);
            this.dataGridViewTagsAndKeywords.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewTagsAndKeywords_KeyDown);
            // 
            // contextMenuStripTagsAndKeywords
            // 
            this.contextMenuStripTagsAndKeywords.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripTagsAndKeywords.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuTagsBrokerCut,
            this.copyToolStripMenuTagsBrokerCopy,
            this.pasteToolStripMenuTagsBrokerPaste,
            this.deleteToolStripMenuTagsBrokerDelete,
            this.undoToolStripMenuTags,
            this.redoToolStripMenuTags,
            this.findToolStripMenuTag,
            this.replaceToolStripMenuTag,
            this.toolStripMenuTagsBrokerSave,
            this.markAsFavoriteToolStripMenuItem,
            this.removeAsFavoriteToolStripMenuItem,
            this.toggleFavoriteToolStripMenuItem,
            this.showFavoriteRowsToolStripMenuItem,
            this.hideEqualRowsToolStripMenuItem,
            this.toolStripMenuItemTagsBrokerCopyText,
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText,
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem,
            this.selectTagsAndKeywordsToolStripMenuItem,
            this.removeTagsAndKeywordsToolStripMenuItem,
            this.toolStripMenuItemTagsAndKeywordMediaPreview});
            this.contextMenuStripTagsAndKeywords.Name = "contextMenuStripMap";
            this.contextMenuStripTagsAndKeywords.Size = new System.Drawing.Size(521, 524);
            // 
            // cutToolStripMenuTagsBrokerCut
            // 
            this.cutToolStripMenuTagsBrokerCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.Cut;
            this.cutToolStripMenuTagsBrokerCut.Name = "cutToolStripMenuTagsBrokerCut";
            this.cutToolStripMenuTagsBrokerCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuTagsBrokerCut.Size = new System.Drawing.Size(520, 26);
            this.cutToolStripMenuTagsBrokerCut.Text = "Cut";
            this.cutToolStripMenuTagsBrokerCut.Click += new System.EventHandler(this.cutToolStripMenuTagsBrokerCut_Click);
            // 
            // copyToolStripMenuTagsBrokerCopy
            // 
            this.copyToolStripMenuTagsBrokerCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.copyToolStripMenuTagsBrokerCopy.Name = "copyToolStripMenuTagsBrokerCopy";
            this.copyToolStripMenuTagsBrokerCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuTagsBrokerCopy.Size = new System.Drawing.Size(520, 26);
            this.copyToolStripMenuTagsBrokerCopy.Text = "Copy";
            this.copyToolStripMenuTagsBrokerCopy.Click += new System.EventHandler(this.copyToolStripMenuTagsBrokerCopy_Click);
            // 
            // pasteToolStripMenuTagsBrokerPaste
            // 
            this.pasteToolStripMenuTagsBrokerPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.Paste;
            this.pasteToolStripMenuTagsBrokerPaste.Name = "pasteToolStripMenuTagsBrokerPaste";
            this.pasteToolStripMenuTagsBrokerPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuTagsBrokerPaste.Size = new System.Drawing.Size(520, 26);
            this.pasteToolStripMenuTagsBrokerPaste.Text = "Paste";
            this.pasteToolStripMenuTagsBrokerPaste.Click += new System.EventHandler(this.pasteToolStripMenuTagsBrokerPaste_Click);
            // 
            // deleteToolStripMenuTagsBrokerDelete
            // 
            this.deleteToolStripMenuTagsBrokerDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.Delete;
            this.deleteToolStripMenuTagsBrokerDelete.Name = "deleteToolStripMenuTagsBrokerDelete";
            this.deleteToolStripMenuTagsBrokerDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteToolStripMenuTagsBrokerDelete.Size = new System.Drawing.Size(520, 26);
            this.deleteToolStripMenuTagsBrokerDelete.Text = "Delete";
            this.deleteToolStripMenuTagsBrokerDelete.Click += new System.EventHandler(this.deleteToolStripMenuTagsBrokerDelete_Click);
            // 
            // undoToolStripMenuTags
            // 
            this.undoToolStripMenuTags.Image = global::PhotoTagsSynchronizer.Properties.Resources.Undo;
            this.undoToolStripMenuTags.Name = "undoToolStripMenuTags";
            this.undoToolStripMenuTags.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuTags.Size = new System.Drawing.Size(520, 26);
            this.undoToolStripMenuTags.Text = "Undo";
            this.undoToolStripMenuTags.Click += new System.EventHandler(this.undoToolStripMenuTags_Click);
            // 
            // redoToolStripMenuTags
            // 
            this.redoToolStripMenuTags.Image = global::PhotoTagsSynchronizer.Properties.Resources.Redo;
            this.redoToolStripMenuTags.Name = "redoToolStripMenuTags";
            this.redoToolStripMenuTags.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuTags.Size = new System.Drawing.Size(520, 26);
            this.redoToolStripMenuTags.Text = "Redo";
            this.redoToolStripMenuTags.Click += new System.EventHandler(this.redoToolStripMenuTags_Click);
            // 
            // findToolStripMenuTag
            // 
            this.findToolStripMenuTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.Find;
            this.findToolStripMenuTag.Name = "findToolStripMenuTag";
            this.findToolStripMenuTag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuTag.Size = new System.Drawing.Size(520, 26);
            this.findToolStripMenuTag.Text = "Find";
            this.findToolStripMenuTag.Click += new System.EventHandler(this.findToolStripMenuTag_Click);
            // 
            // replaceToolStripMenuTag
            // 
            this.replaceToolStripMenuTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.Replace;
            this.replaceToolStripMenuTag.Name = "replaceToolStripMenuTag";
            this.replaceToolStripMenuTag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceToolStripMenuTag.Size = new System.Drawing.Size(520, 26);
            this.replaceToolStripMenuTag.Text = "Replace";
            this.replaceToolStripMenuTag.Click += new System.EventHandler(this.replaceToolStripMenuTag_Click);
            // 
            // toolStripMenuTagsBrokerSave
            // 
            this.toolStripMenuTagsBrokerSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.save_32;
            this.toolStripMenuTagsBrokerSave.Name = "toolStripMenuTagsBrokerSave";
            this.toolStripMenuTagsBrokerSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuTagsBrokerSave.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuTagsBrokerSave.Text = "Save";
            this.toolStripMenuTagsBrokerSave.Click += new System.EventHandler(this.toolStripMenuTagsBrokerSave_Click);
            // 
            // markAsFavoriteToolStripMenuItem
            // 
            this.markAsFavoriteToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect;
            this.markAsFavoriteToolStripMenuItem.Name = "markAsFavoriteToolStripMenuItem";
            this.markAsFavoriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.markAsFavoriteToolStripMenuItem.Size = new System.Drawing.Size(520, 26);
            this.markAsFavoriteToolStripMenuItem.Text = "Mark as favorite";
            this.markAsFavoriteToolStripMenuItem.Click += new System.EventHandler(this.markAsFavoriteToolStripMenuItem_Click);
            // 
            // removeAsFavoriteToolStripMenuItem
            // 
            this.removeAsFavoriteToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove;
            this.removeAsFavoriteToolStripMenuItem.Name = "removeAsFavoriteToolStripMenuItem";
            this.removeAsFavoriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.removeAsFavoriteToolStripMenuItem.Size = new System.Drawing.Size(520, 26);
            this.removeAsFavoriteToolStripMenuItem.Text = "Remove as favorite";
            this.removeAsFavoriteToolStripMenuItem.Click += new System.EventHandler(this.removeAsFavoriteToolStripMenuItem_Click);
            // 
            // toggleFavoriteToolStripMenuItem
            // 
            this.toggleFavoriteToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle;
            this.toggleFavoriteToolStripMenuItem.Name = "toggleFavoriteToolStripMenuItem";
            this.toggleFavoriteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toggleFavoriteToolStripMenuItem.Size = new System.Drawing.Size(520, 26);
            this.toggleFavoriteToolStripMenuItem.Text = "Toggle favorite";
            this.toggleFavoriteToolStripMenuItem.Click += new System.EventHandler(this.toggleFavoriteToolStripMenuItem_Click);
            // 
            // showFavoriteRowsToolStripMenuItem
            // 
            this.showFavoriteRowsToolStripMenuItem.Name = "showFavoriteRowsToolStripMenuItem";
            this.showFavoriteRowsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.showFavoriteRowsToolStripMenuItem.Size = new System.Drawing.Size(520, 26);
            this.showFavoriteRowsToolStripMenuItem.Text = "Show favorite rows";
            this.showFavoriteRowsToolStripMenuItem.Click += new System.EventHandler(this.showFavoriteRowsToolStripMenuItem_Click);
            // 
            // hideEqualRowsToolStripMenuItem
            // 
            this.hideEqualRowsToolStripMenuItem.Name = "hideEqualRowsToolStripMenuItem";
            this.hideEqualRowsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.hideEqualRowsToolStripMenuItem.Size = new System.Drawing.Size(520, 26);
            this.hideEqualRowsToolStripMenuItem.Text = "Hide equal rows";
            this.hideEqualRowsToolStripMenuItem.Click += new System.EventHandler(this.hideEqualRowsToolStripMenuItem_Click);
            // 
            // toolStripMenuItemTagsBrokerCopyText
            // 
            this.toolStripMenuItemTagsBrokerCopyText.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemTagsBrokerCopyText.Image")));
            this.toolStripMenuItemTagsBrokerCopyText.Name = "toolStripMenuItemTagsBrokerCopyText";
            this.toolStripMenuItemTagsBrokerCopyText.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemTagsBrokerCopyText.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemTagsBrokerCopyText.Text = "Copy selected values to media file without overwrite";
            this.toolStripMenuItemTagsBrokerCopyText.Click += new System.EventHandler(this.toolStripMenuItemTagsCopyText_Click);
            // 
            // toolStripMenuItemTagsAndKeywordsBrokerOverwriteText
            // 
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Image")));
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Name = "toolStripMenuItemTagsAndKeywordsBrokerOverwriteText";
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Text = "Copy selected values to media file and overwrite";
            this.toolStripMenuItemTagsAndKeywordsBrokerOverwriteText.Click += new System.EventHandler(this.toolStripMenuItemTagsOverwriteText_Click);
            // 
            // toggleTagsAndKeywordsSelectionToolStripMenuItem
            // 
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.KeywordToggle;
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.Name = "toggleTagsAndKeywordsSelectionToolStripMenuItem";
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.Size = new System.Drawing.Size(520, 26);
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.Text = "Toggle selected keyword tag";
            this.toggleTagsAndKeywordsSelectionToolStripMenuItem.Click += new System.EventHandler(this.toggleTagSelectionToolStripMenuItem_Click);
            // 
            // selectTagsAndKeywordsToolStripMenuItem
            // 
            this.selectTagsAndKeywordsToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.KeywordSelect;
            this.selectTagsAndKeywordsToolStripMenuItem.Name = "selectTagsAndKeywordsToolStripMenuItem";
            this.selectTagsAndKeywordsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Space)));
            this.selectTagsAndKeywordsToolStripMenuItem.Size = new System.Drawing.Size(520, 26);
            this.selectTagsAndKeywordsToolStripMenuItem.Text = "Set selected keyword tags";
            this.selectTagsAndKeywordsToolStripMenuItem.Click += new System.EventHandler(this.selectTagToolStripMenuItem_Click);
            // 
            // removeTagsAndKeywordsToolStripMenuItem
            // 
            this.removeTagsAndKeywordsToolStripMenuItem.Image = global::PhotoTagsSynchronizer.Properties.Resources.KeywordDelete;
            this.removeTagsAndKeywordsToolStripMenuItem.Name = "removeTagsAndKeywordsToolStripMenuItem";
            this.removeTagsAndKeywordsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.removeTagsAndKeywordsToolStripMenuItem.Size = new System.Drawing.Size(520, 26);
            this.removeTagsAndKeywordsToolStripMenuItem.Text = "Remove selected keyword tags";
            this.removeTagsAndKeywordsToolStripMenuItem.Click += new System.EventHandler(this.removeTagToolStripMenuItem_Click);
            // 
            // toolStripMenuItemTagsAndKeywordMediaPreview
            // 
            this.toolStripMenuItemTagsAndKeywordMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Preview;
            this.toolStripMenuItemTagsAndKeywordMediaPreview.Name = "toolStripMenuItemTagsAndKeywordMediaPreview";
            this.toolStripMenuItemTagsAndKeywordMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemTagsAndKeywordMediaPreview.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemTagsAndKeywordMediaPreview.Text = "Media Preview";
            this.toolStripMenuItemTagsAndKeywordMediaPreview.Click += new System.EventHandler(this.toolStripMenuItemTagsAndKeywordMediaPreview_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Tags";
            // 
            // groupBoxRating
            // 
            this.groupBoxRating.Controls.Add(this.radioButtonRating5);
            this.groupBoxRating.Controls.Add(this.radioButtonRating4);
            this.groupBoxRating.Controls.Add(this.radioButtonRating3);
            this.groupBoxRating.Controls.Add(this.radioButtonRating2);
            this.groupBoxRating.Controls.Add(this.radioButtonRating1);
            this.groupBoxRating.Location = new System.Drawing.Point(9, 142);
            this.groupBoxRating.Name = "groupBoxRating";
            this.groupBoxRating.Size = new System.Drawing.Size(277, 38);
            this.groupBoxRating.TabIndex = 8;
            this.groupBoxRating.TabStop = false;
            this.groupBoxRating.Text = "Rating";
            // 
            // radioButtonRating5
            // 
            this.radioButtonRating5.AutoSize = true;
            this.radioButtonRating5.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.radioButtonRating5.Location = new System.Drawing.Point(233, 10);
            this.radioButtonRating5.Name = "radioButtonRating5";
            this.radioButtonRating5.Size = new System.Drawing.Size(41, 24);
            this.radioButtonRating5.TabIndex = 4;
            this.radioButtonRating5.TabStop = true;
            this.radioButtonRating5.UseVisualStyleBackColor = true;
            this.radioButtonRating5.CheckedChanged += new System.EventHandler(this.radioButtonRating5_CheckedChanged);
            // 
            // radioButtonRating4
            // 
            this.radioButtonRating4.AutoSize = true;
            this.radioButtonRating4.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.radioButtonRating4.Location = new System.Drawing.Point(175, 10);
            this.radioButtonRating4.Name = "radioButtonRating4";
            this.radioButtonRating4.Size = new System.Drawing.Size(41, 24);
            this.radioButtonRating4.TabIndex = 3;
            this.radioButtonRating4.TabStop = true;
            this.radioButtonRating4.UseVisualStyleBackColor = true;
            this.radioButtonRating4.CheckedChanged += new System.EventHandler(this.radioButtonRating4_CheckedChanged);
            // 
            // radioButtonRating3
            // 
            this.radioButtonRating3.AutoSize = true;
            this.radioButtonRating3.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.radioButtonRating3.Location = new System.Drawing.Point(121, 10);
            this.radioButtonRating3.Name = "radioButtonRating3";
            this.radioButtonRating3.Size = new System.Drawing.Size(41, 24);
            this.radioButtonRating3.TabIndex = 2;
            this.radioButtonRating3.TabStop = true;
            this.radioButtonRating3.UseVisualStyleBackColor = true;
            this.radioButtonRating3.CheckedChanged += new System.EventHandler(this.radioButtonRating3_CheckedChanged);
            // 
            // radioButtonRating2
            // 
            this.radioButtonRating2.AutoSize = true;
            this.radioButtonRating2.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.radioButtonRating2.Location = new System.Drawing.Point(62, 10);
            this.radioButtonRating2.Name = "radioButtonRating2";
            this.radioButtonRating2.Size = new System.Drawing.Size(41, 24);
            this.radioButtonRating2.TabIndex = 1;
            this.radioButtonRating2.TabStop = true;
            this.radioButtonRating2.UseVisualStyleBackColor = true;
            this.radioButtonRating2.CheckedChanged += new System.EventHandler(this.radioButtonRating2_CheckedChanged);
            // 
            // radioButtonRating1
            // 
            this.radioButtonRating1.AutoSize = true;
            this.radioButtonRating1.Image = global::PhotoTagsSynchronizer.Properties.Resources.star;
            this.radioButtonRating1.Location = new System.Drawing.Point(6, 10);
            this.radioButtonRating1.Name = "radioButtonRating1";
            this.radioButtonRating1.Size = new System.Drawing.Size(41, 24);
            this.radioButtonRating1.TabIndex = 0;
            this.radioButtonRating1.TabStop = true;
            this.radioButtonRating1.UseVisualStyleBackColor = true;
            this.radioButtonRating1.CheckedChanged += new System.EventHandler(this.radioButtonRating1_CheckedChanged);
            // 
            // comboBoxAlbum
            // 
            this.comboBoxAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAlbum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxAlbum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxAlbum.FormattingEnabled = true;
            this.comboBoxAlbum.Location = new System.Drawing.Point(88, 7);
            this.comboBoxAlbum.Name = "comboBoxAlbum";
            this.comboBoxAlbum.Size = new System.Drawing.Size(440, 25);
            this.comboBoxAlbum.TabIndex = 7;
            this.comboBoxAlbum.TextChanged += new System.EventHandler(this.comboBoxAlbum_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Album";
            // 
            // comboBoxComments
            // 
            this.comboBoxComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxComments.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxComments.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxComments.FormattingEnabled = true;
            this.comboBoxComments.Location = new System.Drawing.Point(88, 88);
            this.comboBoxComments.Name = "comboBoxComments";
            this.comboBoxComments.Size = new System.Drawing.Size(440, 25);
            this.comboBoxComments.TabIndex = 5;
            this.comboBoxComments.TextChanged += new System.EventHandler(this.comboBoxComments_TextChanged);
            // 
            // labelComments
            // 
            this.labelComments.AutoSize = true;
            this.labelComments.Location = new System.Drawing.Point(6, 91);
            this.labelComments.Name = "labelComments";
            this.labelComments.Size = new System.Drawing.Size(75, 17);
            this.labelComments.TabIndex = 4;
            this.labelComments.Text = "Comments";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(6, 64);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(76, 17);
            this.labelDescription.TabIndex = 3;
            this.labelDescription.Text = "Description";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(6, 37);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(32, 17);
            this.labelTitle.TabIndex = 2;
            this.labelTitle.Text = "Title";
            // 
            // comboBoxDescription
            // 
            this.comboBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxDescription.FormattingEnabled = true;
            this.comboBoxDescription.Location = new System.Drawing.Point(88, 61);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(440, 25);
            this.comboBoxDescription.TabIndex = 1;
            this.comboBoxDescription.TextChanged += new System.EventHandler(this.comboBoxDescription_TextChanged);
            // 
            // comboBoxTitle
            // 
            this.comboBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTitle.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxTitle.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxTitle.FormattingEnabled = true;
            this.comboBoxTitle.Location = new System.Drawing.Point(88, 34);
            this.comboBoxTitle.Name = "comboBoxTitle";
            this.comboBoxTitle.Size = new System.Drawing.Size(440, 25);
            this.comboBoxTitle.TabIndex = 0;
            this.comboBoxTitle.TextChanged += new System.EventHandler(this.comboBoxTitle_TextChanged);
            // 
            // tabPagePeople
            // 
            this.tabPagePeople.Controls.Add(this.dataGridViewPeople);
            this.tabPagePeople.Location = new System.Drawing.Point(4, 25);
            this.tabPagePeople.Name = "tabPagePeople";
            this.tabPagePeople.Size = new System.Drawing.Size(537, 863);
            this.tabPagePeople.TabIndex = 2;
            this.tabPagePeople.Tag = "People";
            this.tabPagePeople.Text = "People";
            this.tabPagePeople.ToolTipText = "Metadata people regions";
            this.tabPagePeople.UseVisualStyleBackColor = true;
            // 
            // dataGridViewPeople
            // 
            this.dataGridViewPeople.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPeople.ColumnHeadersHeight = 29;
            this.dataGridViewPeople.ContextMenuStrip = this.contextMenuStripPeople;
            this.dataGridViewPeople.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPeople.Name = "dataGridViewPeople";
            this.dataGridViewPeople.RowHeadersWidth = 51;
            this.dataGridViewPeople.RowTemplate.Height = 24;
            this.dataGridViewPeople.Size = new System.Drawing.Size(534, 866);
            this.dataGridViewPeople.TabIndex = 0;
            this.dataGridViewPeople.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewPeople_CellBeginEdit);
            this.dataGridViewPeople.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPeople_CellEndEdit);
            this.dataGridViewPeople.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPeople_CellEnter);
            this.dataGridViewPeople.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPeople_CellMouseClick);
            this.dataGridViewPeople.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPeople_CellMouseDown);
            this.dataGridViewPeople.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPeople_CellMouseLeave);
            this.dataGridViewPeople.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPeople_CellMouseMove);
            this.dataGridViewPeople.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewPeople_CellMouseUp);
            this.dataGridViewPeople.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewPeople_CellPainting);
            this.dataGridViewPeople.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dataGridViewPeople_CellParsing);
            this.dataGridViewPeople.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewPeople_EditingControlShowing);
            this.dataGridViewPeople.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewPeople_KeyDown);
            // 
            // contextMenuStripPeople
            // 
            this.contextMenuStripPeople.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripPeople.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemPeopleRenameFromLast1,
            this.toolStripMenuItemPeopleRenameFromLast2,
            this.toolStripMenuItemPeopleRenameFromLast3,
            this.toolStripMenuItemPeopleRenameFromMostUsed,
            this.toolStripMenuItemPeopleRenameFromAll,
            this.toolStripMenuItemPeopleCut,
            this.toolStripMenuItemPeopleCopy,
            this.toolStripMenuItemPeoplePaste,
            this.toolStripMenuItemPeopleDelete,
            this.toolStripMenuItemPeopleUndo,
            this.toolStripMenuItemPeopleRedo,
            this.toolStripMenuItemPeopleFind,
            this.toolStripMenuItemPeopleReplace,
            this.toolStripMenuItemPeopleSave,
            this.toolStripMenuItemPeopleMarkFavorite,
            this.toolStripMenuItemPeopleRemoveFavorite,
            this.toolStripMenuItemPeopleToggleFavorite,
            this.toolStripMenuItemPeopleShowFavorite,
            this.toolStripMenuItemPeopleHideEqualRows,
            this.toolStripMenuItemPeopleTogglePeopleTag,
            this.toolStripMenuItemPeopleSelectPeopleTag,
            this.toolStripMenuItemPeopleRemovePeopleTag,
            this.toolStripMenuItemPeopleShowRegionSelector,
            this.toolStripMenuItemPeopleMediaPreview});
            this.contextMenuStripPeople.Name = "contextMenuStripMap";
            this.contextMenuStripPeople.Size = new System.Drawing.Size(368, 628);
            // 
            // toolStripMenuItemPeopleRenameFromLast1
            // 
            this.toolStripMenuItemPeopleRenameFromLast1.Name = "toolStripMenuItemPeopleRenameFromLast1";
            this.toolStripMenuItemPeopleRenameFromLast1.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleRenameFromLast1.Tag = "Unknown 1";
            this.toolStripMenuItemPeopleRenameFromLast1.Text = "Rename #1 - Unknown 1";
            this.toolStripMenuItemPeopleRenameFromLast1.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameFromLast1_Click);
            // 
            // toolStripMenuItemPeopleRenameFromLast2
            // 
            this.toolStripMenuItemPeopleRenameFromLast2.Name = "toolStripMenuItemPeopleRenameFromLast2";
            this.toolStripMenuItemPeopleRenameFromLast2.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleRenameFromLast2.Tag = "Unknown 2";
            this.toolStripMenuItemPeopleRenameFromLast2.Text = "Rename #2 - Unknown 2";
            this.toolStripMenuItemPeopleRenameFromLast2.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameFromLast2_Click);
            // 
            // toolStripMenuItemPeopleRenameFromLast3
            // 
            this.toolStripMenuItemPeopleRenameFromLast3.Name = "toolStripMenuItemPeopleRenameFromLast3";
            this.toolStripMenuItemPeopleRenameFromLast3.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleRenameFromLast3.Tag = "Unknown 3";
            this.toolStripMenuItemPeopleRenameFromLast3.Text = "Rename #3 - Unknown 3";
            this.toolStripMenuItemPeopleRenameFromLast3.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameFromLast3_Click);
            // 
            // toolStripMenuItemPeopleRenameFromMostUsed
            // 
            this.toolStripMenuItemPeopleRenameFromMostUsed.Name = "toolStripMenuItemPeopleRenameFromMostUsed";
            this.toolStripMenuItemPeopleRenameFromMostUsed.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleRenameFromMostUsed.Text = "Rename - From most used";
            // 
            // toolStripMenuItemPeopleRenameFromAll
            // 
            this.toolStripMenuItemPeopleRenameFromAll.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemPeopleSelected,
            this.toolStripMenuItemPeopleRenameSelected});
            this.toolStripMenuItemPeopleRenameFromAll.Name = "toolStripMenuItemPeopleRenameFromAll";
            this.toolStripMenuItemPeopleRenameFromAll.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleRenameFromAll.Text = "Rename - List all";
            // 
            // ToolStripMenuItemPeopleSelected
            // 
            this.ToolStripMenuItemPeopleSelected.Name = "ToolStripMenuItemPeopleSelected";
            this.ToolStripMenuItemPeopleSelected.Size = new System.Drawing.Size(163, 26);
            this.ToolStripMenuItemPeopleSelected.Text = "(Unknown)";
            // 
            // toolStripMenuItemPeopleRenameSelected
            // 
            this.toolStripMenuItemPeopleRenameSelected.Name = "toolStripMenuItemPeopleRenameSelected";
            this.toolStripMenuItemPeopleRenameSelected.Size = new System.Drawing.Size(163, 26);
            this.toolStripMenuItemPeopleRenameSelected.Text = "Me";
            this.toolStripMenuItemPeopleRenameSelected.Click += new System.EventHandler(this.toolStripMenuItemPeopleRenameSelected_Click);
            // 
            // toolStripMenuItemPeopleCut
            // 
            this.toolStripMenuItemPeopleCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.Cut;
            this.toolStripMenuItemPeopleCut.Name = "toolStripMenuItemPeopleCut";
            this.toolStripMenuItemPeopleCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemPeopleCut.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleCut.Text = "Cut";
            this.toolStripMenuItemPeopleCut.Click += new System.EventHandler(this.toolStripMenuItemPeopleCut_Click);
            // 
            // toolStripMenuItemPeopleCopy
            // 
            this.toolStripMenuItemPeopleCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemPeopleCopy.Name = "toolStripMenuItemPeopleCopy";
            this.toolStripMenuItemPeopleCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemPeopleCopy.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleCopy.Text = "Copy";
            this.toolStripMenuItemPeopleCopy.Click += new System.EventHandler(this.toolStripMenuItemPeopleCopy_Click);
            // 
            // toolStripMenuItemPeoplePaste
            // 
            this.toolStripMenuItemPeoplePaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.Paste;
            this.toolStripMenuItemPeoplePaste.Name = "toolStripMenuItemPeoplePaste";
            this.toolStripMenuItemPeoplePaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.toolStripMenuItemPeoplePaste.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeoplePaste.Text = "Paste";
            this.toolStripMenuItemPeoplePaste.Click += new System.EventHandler(this.toolStripMenuItemPeoplePaste_Click);
            // 
            // toolStripMenuItemPeopleDelete
            // 
            this.toolStripMenuItemPeopleDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.Delete;
            this.toolStripMenuItemPeopleDelete.Name = "toolStripMenuItemPeopleDelete";
            this.toolStripMenuItemPeopleDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemPeopleDelete.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleDelete.Text = "Delete";
            this.toolStripMenuItemPeopleDelete.Click += new System.EventHandler(this.toolStripMenuItemPeopleDelete_Click);
            // 
            // toolStripMenuItemPeopleUndo
            // 
            this.toolStripMenuItemPeopleUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Undo;
            this.toolStripMenuItemPeopleUndo.Name = "toolStripMenuItemPeopleUndo";
            this.toolStripMenuItemPeopleUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemPeopleUndo.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleUndo.Text = "Undo";
            this.toolStripMenuItemPeopleUndo.Click += new System.EventHandler(this.toolStripMenuItemPeopleUndo_Click);
            // 
            // toolStripMenuItemPeopleRedo
            // 
            this.toolStripMenuItemPeopleRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Redo;
            this.toolStripMenuItemPeopleRedo.Name = "toolStripMenuItemPeopleRedo";
            this.toolStripMenuItemPeopleRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemPeopleRedo.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleRedo.Text = "Redo";
            this.toolStripMenuItemPeopleRedo.Click += new System.EventHandler(this.toolStripMenuItemPeopleRedo_Click);
            // 
            // toolStripMenuItemPeopleFind
            // 
            this.toolStripMenuItemPeopleFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.Find;
            this.toolStripMenuItemPeopleFind.Name = "toolStripMenuItemPeopleFind";
            this.toolStripMenuItemPeopleFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemPeopleFind.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleFind.Text = "Find";
            this.toolStripMenuItemPeopleFind.Click += new System.EventHandler(this.toolStripMenuItemPeopleFind_Click);
            // 
            // toolStripMenuItemPeopleReplace
            // 
            this.toolStripMenuItemPeopleReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.Replace;
            this.toolStripMenuItemPeopleReplace.Name = "toolStripMenuItemPeopleReplace";
            this.toolStripMenuItemPeopleReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemPeopleReplace.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleReplace.Text = "Replace";
            this.toolStripMenuItemPeopleReplace.Click += new System.EventHandler(this.toolStripMenuItemPeopleReplace_Click);
            // 
            // toolStripMenuItemPeopleSave
            // 
            this.toolStripMenuItemPeopleSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.save_32;
            this.toolStripMenuItemPeopleSave.Name = "toolStripMenuItemPeopleSave";
            this.toolStripMenuItemPeopleSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemPeopleSave.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleSave.Text = "Save";
            this.toolStripMenuItemPeopleSave.Click += new System.EventHandler(this.toolStripMenuItemPeopleSave_Click);
            // 
            // toolStripMenuItemPeopleMarkFavorite
            // 
            this.toolStripMenuItemPeopleMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect;
            this.toolStripMenuItemPeopleMarkFavorite.Name = "toolStripMenuItemPeopleMarkFavorite";
            this.toolStripMenuItemPeopleMarkFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemPeopleMarkFavorite.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleMarkFavorite.Text = "Mark as favorite";
            this.toolStripMenuItemPeopleMarkFavorite.Click += new System.EventHandler(this.toolStripMenuItemPeopleMarkFavorite_Click);
            // 
            // toolStripMenuItemPeopleRemoveFavorite
            // 
            this.toolStripMenuItemPeopleRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove;
            this.toolStripMenuItemPeopleRemoveFavorite.Name = "toolStripMenuItemPeopleRemoveFavorite";
            this.toolStripMenuItemPeopleRemoveFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemPeopleRemoveFavorite.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleRemoveFavorite.Text = "Remove as favorite";
            this.toolStripMenuItemPeopleRemoveFavorite.Click += new System.EventHandler(this.toolStripMenuItemPeopleRemoveFavorite_Click);
            // 
            // toolStripMenuItemPeopleToggleFavorite
            // 
            this.toolStripMenuItemPeopleToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle;
            this.toolStripMenuItemPeopleToggleFavorite.Name = "toolStripMenuItemPeopleToggleFavorite";
            this.toolStripMenuItemPeopleToggleFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemPeopleToggleFavorite.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleToggleFavorite.Text = "Toggle favorite";
            this.toolStripMenuItemPeopleToggleFavorite.Click += new System.EventHandler(this.toolStripMenuItemPeopleToggleFavorite_Click);
            // 
            // toolStripMenuItemPeopleShowFavorite
            // 
            this.toolStripMenuItemPeopleShowFavorite.Name = "toolStripMenuItemPeopleShowFavorite";
            this.toolStripMenuItemPeopleShowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemPeopleShowFavorite.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleShowFavorite.Text = "Show favorite rows";
            this.toolStripMenuItemPeopleShowFavorite.Click += new System.EventHandler(this.toolStripMenuItemPeopleShowFavorite_Click);
            // 
            // toolStripMenuItemPeopleHideEqualRows
            // 
            this.toolStripMenuItemPeopleHideEqualRows.Name = "toolStripMenuItemPeopleHideEqualRows";
            this.toolStripMenuItemPeopleHideEqualRows.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemPeopleHideEqualRows.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleHideEqualRows.Text = "Hide equal rows";
            this.toolStripMenuItemPeopleHideEqualRows.Click += new System.EventHandler(this.toolStripMenuItemPeopleHideEqualRows_Click);
            // 
            // toolStripMenuItemPeopleTogglePeopleTag
            // 
            this.toolStripMenuItemPeopleTogglePeopleTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.KeywordToggle;
            this.toolStripMenuItemPeopleTogglePeopleTag.Name = "toolStripMenuItemPeopleTogglePeopleTag";
            this.toolStripMenuItemPeopleTogglePeopleTag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.toolStripMenuItemPeopleTogglePeopleTag.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleTogglePeopleTag.Text = "Toggle selected people tag";
            this.toolStripMenuItemPeopleTogglePeopleTag.Click += new System.EventHandler(this.toolStripMenuItemPeopleTogglePeopleTag_Click);
            // 
            // toolStripMenuItemPeopleSelectPeopleTag
            // 
            this.toolStripMenuItemPeopleSelectPeopleTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.KeywordSelect;
            this.toolStripMenuItemPeopleSelectPeopleTag.Name = "toolStripMenuItemPeopleSelectPeopleTag";
            this.toolStripMenuItemPeopleSelectPeopleTag.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Space)));
            this.toolStripMenuItemPeopleSelectPeopleTag.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleSelectPeopleTag.Text = "Set selected people tags";
            this.toolStripMenuItemPeopleSelectPeopleTag.Click += new System.EventHandler(this.toolStripMenuItemPeopleSelectPeopleTag_Click);
            // 
            // toolStripMenuItemPeopleRemovePeopleTag
            // 
            this.toolStripMenuItemPeopleRemovePeopleTag.Image = global::PhotoTagsSynchronizer.Properties.Resources.KeywordDelete;
            this.toolStripMenuItemPeopleRemovePeopleTag.Name = "toolStripMenuItemPeopleRemovePeopleTag";
            this.toolStripMenuItemPeopleRemovePeopleTag.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.toolStripMenuItemPeopleRemovePeopleTag.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleRemovePeopleTag.Text = "Remove selected people tags";
            this.toolStripMenuItemPeopleRemovePeopleTag.Click += new System.EventHandler(this.toolStripMenuItemPeopleRemovePeopleTag_Click);
            // 
            // toolStripMenuItemPeopleShowRegionSelector
            // 
            this.toolStripMenuItemPeopleShowRegionSelector.Image = global::PhotoTagsSynchronizer.Properties.Resources.RegionSelector;
            this.toolStripMenuItemPeopleShowRegionSelector.Name = "toolStripMenuItemPeopleShowRegionSelector";
            this.toolStripMenuItemPeopleShowRegionSelector.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripMenuItemPeopleShowRegionSelector.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleShowRegionSelector.Text = "Show Region Selector";
            this.toolStripMenuItemPeopleShowRegionSelector.Click += new System.EventHandler(this.toolStripMenuItemPeopleShowRegionSelector_Click);
            // 
            // toolStripMenuItemPeopleMediaPreview
            // 
            this.toolStripMenuItemPeopleMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Preview;
            this.toolStripMenuItemPeopleMediaPreview.Name = "toolStripMenuItemPeopleMediaPreview";
            this.toolStripMenuItemPeopleMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemPeopleMediaPreview.Size = new System.Drawing.Size(367, 26);
            this.toolStripMenuItemPeopleMediaPreview.Text = "Media Preview";
            this.toolStripMenuItemPeopleMediaPreview.Click += new System.EventHandler(this.toolStripMenuItemPeopleMediaPreview_Click);
            // 
            // tabPageMap
            // 
            this.tabPageMap.Controls.Add(this.splitContainerMap);
            this.tabPageMap.Location = new System.Drawing.Point(4, 25);
            this.tabPageMap.Name = "tabPageMap";
            this.tabPageMap.Size = new System.Drawing.Size(537, 863);
            this.tabPageMap.TabIndex = 3;
            this.tabPageMap.Tag = "Map";
            this.tabPageMap.Text = "Map";
            this.tabPageMap.ToolTipText = "Metadata Locations and GPS tags";
            this.tabPageMap.UseVisualStyleBackColor = true;
            // 
            // splitContainerMap
            // 
            this.splitContainerMap.BackColor = System.Drawing.Color.Black;
            this.splitContainerMap.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainerMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMap.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMap.Name = "splitContainerMap";
            this.splitContainerMap.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMap.Panel1
            // 
            this.splitContainerMap.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerMap.Panel1.Controls.Add(this.comboBoxGoogleLocationInterval);
            this.splitContainerMap.Panel1.Controls.Add(this.comboBoxGoogleTimeZoneShift);
            this.splitContainerMap.Panel1.Controls.Add(this.dataGridViewMap);
            // 
            // splitContainerMap.Panel2
            // 
            this.splitContainerMap.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerMap.Panel2.Controls.Add(this.panel3);
            this.splitContainerMap.Panel2.Controls.Add(this.panelBrowser);
            this.splitContainerMap.Size = new System.Drawing.Size(537, 863);
            this.splitContainerMap.SplitterDistance = 391;
            this.splitContainerMap.SplitterWidth = 10;
            this.splitContainerMap.TabIndex = 5;
            this.splitContainerMap.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerMap_SplitterMoved);
            // 
            // comboBoxGoogleLocationInterval
            // 
            this.comboBoxGoogleLocationInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGoogleLocationInterval.FormattingEnabled = true;
            this.comboBoxGoogleLocationInterval.Items.AddRange(new object[] {
            "1 minute",
            "5 minutes",
            "30 minutes",
            "1 hour",
            "1 day",
            "2 days",
            "10 days"});
            this.comboBoxGoogleLocationInterval.Location = new System.Drawing.Point(150, 3);
            this.comboBoxGoogleLocationInterval.Name = "comboBoxGoogleLocationInterval";
            this.comboBoxGoogleLocationInterval.Size = new System.Drawing.Size(178, 25);
            this.comboBoxGoogleLocationInterval.TabIndex = 14;
            this.comboBoxGoogleLocationInterval.SelectedIndexChanged += new System.EventHandler(this.comboBoxGoogleLocationInterval_SelectedIndexChanged);
            // 
            // comboBoxGoogleTimeZoneShift
            // 
            this.comboBoxGoogleTimeZoneShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGoogleTimeZoneShift.FormattingEnabled = true;
            this.comboBoxGoogleTimeZoneShift.Items.AddRange(new object[] {
            "Time zone -12",
            "Time zone -11",
            "Time zone -10",
            "Time zone -9",
            "Time zone -8",
            "Time zone -7",
            "Time zone -6",
            "Time zone -5",
            "Time zone -4",
            "Time zone -3",
            "Time zone -2",
            "Time zone -1",
            "Time zone 0",
            "Time zone 1",
            "Time zone 2",
            "Time zone 3",
            "Time zone 4",
            "Time zone 5",
            "Time zone 6",
            "Time zone 7",
            "Time zone 8",
            "Time zone 9",
            "Time zone 10",
            "Time zone 11",
            "Time zone 12"});
            this.comboBoxGoogleTimeZoneShift.Location = new System.Drawing.Point(5, 3);
            this.comboBoxGoogleTimeZoneShift.Name = "comboBoxGoogleTimeZoneShift";
            this.comboBoxGoogleTimeZoneShift.Size = new System.Drawing.Size(139, 25);
            this.comboBoxGoogleTimeZoneShift.TabIndex = 13;
            this.comboBoxGoogleTimeZoneShift.SelectedIndexChanged += new System.EventHandler(this.comboBoxGoogleTimeZoneShift_SelectedIndexChanged);
            // 
            // dataGridViewMap
            // 
            this.dataGridViewMap.AllowUserToAddRows = false;
            this.dataGridViewMap.AllowUserToDeleteRows = false;
            this.dataGridViewMap.AllowUserToResizeRows = false;
            this.dataGridViewMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMap.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewMap.ColumnHeadersHeight = 29;
            this.dataGridViewMap.ContextMenuStrip = this.contextMenuStripMap;
            this.dataGridViewMap.GridColor = System.Drawing.SystemColors.AppWorkspace;
            this.dataGridViewMap.Location = new System.Drawing.Point(1, 34);
            this.dataGridViewMap.Name = "dataGridViewMap";
            this.dataGridViewMap.RowHeadersWidth = 51;
            this.dataGridViewMap.RowTemplate.Height = 24;
            this.dataGridViewMap.ShowCellErrors = false;
            this.dataGridViewMap.ShowCellToolTips = false;
            this.dataGridViewMap.ShowEditingIcon = false;
            this.dataGridViewMap.ShowRowErrors = false;
            this.dataGridViewMap.Size = new System.Drawing.Size(537, 354);
            this.dataGridViewMap.TabIndex = 10;
            this.dataGridViewMap.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewMap_CellBeginEdit);
            this.dataGridViewMap.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewMap_CellMouseDoubleClick);
            this.dataGridViewMap.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewMap_CellPainting);
            this.dataGridViewMap.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMap_CellValueChanged);
            this.dataGridViewMap.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridMap_KeyDown);
            // 
            // contextMenuStripMap
            // 
            this.contextMenuStripMap.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemMapCut,
            this.toolStripMenuItemMapCopy,
            this.toolStripMenuItemMapPaste,
            this.toolStripMenuItemMapDelete,
            this.toolStripMenuItemMapUndo,
            this.toolStripMenuItemMapRedo,
            this.toolStripMenuItemMapFind,
            this.toolStripMenuItemMapReplace,
            this.toolStripMenuItemMapSave,
            this.toolStripMenuItemMapMarkFavorite,
            this.toolStripMenuItemMapRemoveFavorite,
            this.toolStripMenuItemMapToggleFavorite,
            this.toolStripMenuItemMapShowFavorite,
            this.toolStripMenuItemMapHideEqual,
            this.toolStripMenuItemMapCopyNotOverwrite,
            this.toolStripMenuItemMapCopyAndOverwrite,
            this.toolStripMenuItemShowCoordinateOnMap,
            this.toolStripMenuItemMapReloadLocationUsingNominatim,
            this.toolStripMenuItemMapMediaPreview});
            this.contextMenuStripMap.Name = "contextMenuStripMap";
            this.contextMenuStripMap.Size = new System.Drawing.Size(521, 498);
            // 
            // toolStripMenuItemMapCut
            // 
            this.toolStripMenuItemMapCut.Image = global::PhotoTagsSynchronizer.Properties.Resources.Cut;
            this.toolStripMenuItemMapCut.Name = "toolStripMenuItemMapCut";
            this.toolStripMenuItemMapCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.toolStripMenuItemMapCut.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapCut.Text = "Cut";
            this.toolStripMenuItemMapCut.Click += new System.EventHandler(this.toolStripMenuItemMapCut_Click);
            // 
            // toolStripMenuItemMapCopy
            // 
            this.toolStripMenuItemMapCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemMapCopy.Name = "toolStripMenuItemMapCopy";
            this.toolStripMenuItemMapCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemMapCopy.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapCopy.Text = "Copy";
            this.toolStripMenuItemMapCopy.Click += new System.EventHandler(this.toolStripMenuItemMapCopy_Click);
            // 
            // toolStripMenuItemMapPaste
            // 
            this.toolStripMenuItemMapPaste.Image = global::PhotoTagsSynchronizer.Properties.Resources.Paste;
            this.toolStripMenuItemMapPaste.Name = "toolStripMenuItemMapPaste";
            this.toolStripMenuItemMapPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.toolStripMenuItemMapPaste.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapPaste.Text = "Paste";
            this.toolStripMenuItemMapPaste.Click += new System.EventHandler(this.toolStripMenuItemMapPaste_Click);
            // 
            // toolStripMenuItemMapDelete
            // 
            this.toolStripMenuItemMapDelete.Image = global::PhotoTagsSynchronizer.Properties.Resources.Delete;
            this.toolStripMenuItemMapDelete.Name = "toolStripMenuItemMapDelete";
            this.toolStripMenuItemMapDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.toolStripMenuItemMapDelete.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapDelete.Text = "Delete";
            this.toolStripMenuItemMapDelete.Click += new System.EventHandler(this.toolStripMenuItemMapDelete_Click);
            // 
            // toolStripMenuItemMapUndo
            // 
            this.toolStripMenuItemMapUndo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Undo;
            this.toolStripMenuItemMapUndo.Name = "toolStripMenuItemMapUndo";
            this.toolStripMenuItemMapUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.toolStripMenuItemMapUndo.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapUndo.Text = "Undo";
            this.toolStripMenuItemMapUndo.Click += new System.EventHandler(this.toolStripMenuItemMapUndo_Click);
            // 
            // toolStripMenuItemMapRedo
            // 
            this.toolStripMenuItemMapRedo.Image = global::PhotoTagsSynchronizer.Properties.Resources.Redo;
            this.toolStripMenuItemMapRedo.Name = "toolStripMenuItemMapRedo";
            this.toolStripMenuItemMapRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.toolStripMenuItemMapRedo.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapRedo.Text = "Redo";
            this.toolStripMenuItemMapRedo.Click += new System.EventHandler(this.toolStripMenuItemMapRedo_Click);
            // 
            // toolStripMenuItemMapFind
            // 
            this.toolStripMenuItemMapFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.Find;
            this.toolStripMenuItemMapFind.Name = "toolStripMenuItemMapFind";
            this.toolStripMenuItemMapFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.toolStripMenuItemMapFind.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapFind.Text = "Find";
            this.toolStripMenuItemMapFind.Click += new System.EventHandler(this.toolStripMenuItemMapFind_Click);
            // 
            // toolStripMenuItemMapReplace
            // 
            this.toolStripMenuItemMapReplace.Image = global::PhotoTagsSynchronizer.Properties.Resources.Replace;
            this.toolStripMenuItemMapReplace.Name = "toolStripMenuItemMapReplace";
            this.toolStripMenuItemMapReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItemMapReplace.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapReplace.Text = "Replace";
            this.toolStripMenuItemMapReplace.Click += new System.EventHandler(this.toolStripMenuItemMapReplace_Click);
            // 
            // toolStripMenuItemMapSave
            // 
            this.toolStripMenuItemMapSave.Image = global::PhotoTagsSynchronizer.Properties.Resources.save_32;
            this.toolStripMenuItemMapSave.Name = "toolStripMenuItemMapSave";
            this.toolStripMenuItemMapSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItemMapSave.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapSave.Text = "Save";
            this.toolStripMenuItemMapSave.Click += new System.EventHandler(this.toolStripMenuItemMapSave_Click);
            // 
            // toolStripMenuItemMapMarkFavorite
            // 
            this.toolStripMenuItemMapMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect;
            this.toolStripMenuItemMapMarkFavorite.Name = "toolStripMenuItemMapMarkFavorite";
            this.toolStripMenuItemMapMarkFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemMapMarkFavorite.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapMarkFavorite.Text = "Mark as favorite";
            this.toolStripMenuItemMapMarkFavorite.Click += new System.EventHandler(this.toolStripMenuItemMapMarkFavorite_Click);
            // 
            // toolStripMenuItemMapRemoveFavorite
            // 
            this.toolStripMenuItemMapRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove;
            this.toolStripMenuItemMapRemoveFavorite.Name = "toolStripMenuItemMapRemoveFavorite";
            this.toolStripMenuItemMapRemoveFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemMapRemoveFavorite.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapRemoveFavorite.Text = "Remove as favorite";
            this.toolStripMenuItemMapRemoveFavorite.Click += new System.EventHandler(this.toolStripMenuItemMapRemoveFavorite_Click);
            // 
            // toolStripMenuItemMapToggleFavorite
            // 
            this.toolStripMenuItemMapToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle;
            this.toolStripMenuItemMapToggleFavorite.Name = "toolStripMenuItemMapToggleFavorite";
            this.toolStripMenuItemMapToggleFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemMapToggleFavorite.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapToggleFavorite.Text = "Toggle favorite";
            this.toolStripMenuItemMapToggleFavorite.Click += new System.EventHandler(this.toolStripMenuItemMapToggleFavorite_Click);
            // 
            // toolStripMenuItemMapShowFavorite
            // 
            this.toolStripMenuItemMapShowFavorite.Name = "toolStripMenuItemMapShowFavorite";
            this.toolStripMenuItemMapShowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemMapShowFavorite.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapShowFavorite.Text = "Show favorite rows";
            this.toolStripMenuItemMapShowFavorite.Click += new System.EventHandler(this.toolStripMenuItemMapShowFavorite_Click);
            // 
            // toolStripMenuItemMapHideEqual
            // 
            this.toolStripMenuItemMapHideEqual.Name = "toolStripMenuItemMapHideEqual";
            this.toolStripMenuItemMapHideEqual.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemMapHideEqual.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapHideEqual.Text = "Hide equal rows";
            this.toolStripMenuItemMapHideEqual.Click += new System.EventHandler(this.toolStripMenuItemMapHideEqual_Click);
            // 
            // toolStripMenuItemMapCopyNotOverwrite
            // 
            this.toolStripMenuItemMapCopyNotOverwrite.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemMapCopyNotOverwrite.Image")));
            this.toolStripMenuItemMapCopyNotOverwrite.Name = "toolStripMenuItemMapCopyNotOverwrite";
            this.toolStripMenuItemMapCopyNotOverwrite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemMapCopyNotOverwrite.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapCopyNotOverwrite.Text = "Copy selected values to media file without overwrite";
            this.toolStripMenuItemMapCopyNotOverwrite.Click += new System.EventHandler(this.toolStripMenuItemMapCopyNotOverwrite_Click);
            // 
            // toolStripMenuItemMapCopyAndOverwrite
            // 
            this.toolStripMenuItemMapCopyAndOverwrite.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemMapCopyAndOverwrite.Image")));
            this.toolStripMenuItemMapCopyAndOverwrite.Name = "toolStripMenuItemMapCopyAndOverwrite";
            this.toolStripMenuItemMapCopyAndOverwrite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemMapCopyAndOverwrite.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapCopyAndOverwrite.Text = "Copy selected values to media file and overwrite";
            this.toolStripMenuItemMapCopyAndOverwrite.Click += new System.EventHandler(this.toolStripMenuItemMapCopyAndOverwrite_Click);
            // 
            // toolStripMenuItemShowCoordinateOnMap
            // 
            this.toolStripMenuItemShowCoordinateOnMap.Image = global::PhotoTagsSynchronizer.Properties.Resources.ShowLocation;
            this.toolStripMenuItemShowCoordinateOnMap.Name = "toolStripMenuItemShowCoordinateOnMap";
            this.toolStripMenuItemShowCoordinateOnMap.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemShowCoordinateOnMap.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemShowCoordinateOnMap.Text = "Show Coordinate on Map";
            this.toolStripMenuItemShowCoordinateOnMap.Click += new System.EventHandler(this.toolStripMenuItemShowCoordinateOnMap_Click);
            // 
            // toolStripMenuItemMapReloadLocationUsingNominatim
            // 
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Image = global::PhotoTagsSynchronizer.Properties.Resources.LocationReload;
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Name = "toolStripMenuItemMapReloadLocationUsingNominatim";
            this.toolStripMenuItemMapReloadLocationUsingNominatim.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Text = "Reload Location using Nominatim";
            this.toolStripMenuItemMapReloadLocationUsingNominatim.Click += new System.EventHandler(this.toolStripMenuItemMapReloadLocationUsingNominatim_Click);
            // 
            // toolStripMenuItemMapMediaPreview
            // 
            this.toolStripMenuItemMapMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Preview;
            this.toolStripMenuItemMapMediaPreview.Name = "toolStripMenuItemMapMediaPreview";
            this.toolStripMenuItemMapMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemMapMediaPreview.Size = new System.Drawing.Size(520, 26);
            this.toolStripMenuItemMapMediaPreview.Text = "Media Preview";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textBoxBrowserURL);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.comboBoxMapZoomLevel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 432);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(537, 30);
            this.panel3.TabIndex = 17;
            // 
            // textBoxBrowserURL
            // 
            this.textBoxBrowserURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBrowserURL.Location = new System.Drawing.Point(144, 3);
            this.textBoxBrowserURL.Name = "textBoxBrowserURL";
            this.textBoxBrowserURL.Size = new System.Drawing.Size(384, 24);
            this.textBoxBrowserURL.TabIndex = 9;
            this.textBoxBrowserURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxBrowserURL_KeyPress);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(1, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 26);
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // comboBoxMapZoomLevel
            // 
            this.comboBoxMapZoomLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMapZoomLevel.FormattingEnabled = true;
            this.comboBoxMapZoomLevel.Items.AddRange(new object[] {
            "Zoom 1",
            "Zoom 2",
            "Zoom 3",
            "Zoom 4",
            "Zoom 5",
            "Zoom 6",
            "Zoom 7",
            "Zoom 8",
            "Zoom 9",
            "Zoom 10",
            "Zoom 11",
            "Zoom 12",
            "Zoom 13",
            "Zoom 14",
            "Zoom 15",
            "Zoom 16",
            "Zoom 17",
            "Zoom 18"});
            this.comboBoxMapZoomLevel.Location = new System.Drawing.Point(41, 3);
            this.comboBoxMapZoomLevel.Name = "comboBoxMapZoomLevel";
            this.comboBoxMapZoomLevel.Size = new System.Drawing.Size(97, 25);
            this.comboBoxMapZoomLevel.TabIndex = 15;
            this.comboBoxMapZoomLevel.SelectedIndexChanged += new System.EventHandler(this.comboBoxMapZoomLevel_SelectedIndexChanged);
            // 
            // panelBrowser
            // 
            this.panelBrowser.BackColor = System.Drawing.SystemColors.Control;
            this.panelBrowser.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBrowser.Location = new System.Drawing.Point(0, 0);
            this.panelBrowser.Name = "panelBrowser";
            this.panelBrowser.Size = new System.Drawing.Size(537, 462);
            this.panelBrowser.TabIndex = 1;
            // 
            // tabPageDate
            // 
            this.tabPageDate.Controls.Add(this.textBox1);
            this.tabPageDate.Controls.Add(this.dataGridViewDate);
            this.tabPageDate.Location = new System.Drawing.Point(4, 25);
            this.tabPageDate.Name = "tabPageDate";
            this.tabPageDate.Size = new System.Drawing.Size(537, 863);
            this.tabPageDate.TabIndex = 6;
            this.tabPageDate.Tag = "Date";
            this.tabPageDate.Text = "Date";
            this.tabPageDate.ToolTipText = "Metadata date tags";
            this.tabPageDate.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(6, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(528, 80);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // dataGridViewDate
            // 
            this.dataGridViewDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDate.ColumnHeadersHeight = 29;
            this.dataGridViewDate.Location = new System.Drawing.Point(0, 89);
            this.dataGridViewDate.Name = "dataGridViewDate";
            this.dataGridViewDate.RowHeadersWidth = 51;
            this.dataGridViewDate.RowTemplate.Height = 24;
            this.dataGridViewDate.Size = new System.Drawing.Size(534, 778);
            this.dataGridViewDate.TabIndex = 0;
            this.dataGridViewDate.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewDate_CellBeginEdit);
            this.dataGridViewDate.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDate_CellEndEdit);
            this.dataGridViewDate.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewDate_CellPainting);
            this.dataGridViewDate.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDate_CellValueChanged);
            this.dataGridViewDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewDate_KeyDown);
            // 
            // tabPageExifTool
            // 
            this.tabPageExifTool.Controls.Add(this.dataGridViewExifTool);
            this.tabPageExifTool.Location = new System.Drawing.Point(4, 25);
            this.tabPageExifTool.Name = "tabPageExifTool";
            this.tabPageExifTool.Size = new System.Drawing.Size(537, 863);
            this.tabPageExifTool.TabIndex = 4;
            this.tabPageExifTool.Tag = "ExifTool";
            this.tabPageExifTool.Text = "ExifTool";
            this.tabPageExifTool.ToolTipText = "List all metadatas";
            this.tabPageExifTool.UseVisualStyleBackColor = true;
            // 
            // dataGridViewExifTool
            // 
            this.dataGridViewExifTool.AllowUserToAddRows = false;
            this.dataGridViewExifTool.ColumnHeadersHeight = 29;
            this.dataGridViewExifTool.ContextMenuStrip = this.contextMenuStripExifTool;
            this.dataGridViewExifTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExifTool.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewExifTool.Name = "dataGridViewExifTool";
            this.dataGridViewExifTool.RowHeadersWidth = 51;
            this.dataGridViewExifTool.RowTemplate.Height = 24;
            this.dataGridViewExifTool.Size = new System.Drawing.Size(537, 863);
            this.dataGridViewExifTool.TabIndex = 0;
            this.dataGridViewExifTool.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewExifTool_CellBeginEdit);
            this.dataGridViewExifTool.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewExifTool_CellPainting);
            this.dataGridViewExifTool.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewExifTool_KeyDown);
            // 
            // contextMenuStripExifTool
            // 
            this.contextMenuStripExifTool.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripExifTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemExiftoolAssignCompositeTag,
            this.toolStripMenuItemExiftoolCopy,
            this.toolStripMenuItemExiftoolFind,
            this.toolStripMenuItemExiftoolMarkFavorite,
            this.toolStripMenuItemExiftoolRemoveFavorite,
            this.toolStripMenuItemExiftoolToggleFavorite,
            this.toolStripMenuItemExiftoolSHowFavorite,
            this.toolStripMenuItemExiftoolHideEqual,
            this.toolStripMenuItemMediaPreview});
            this.contextMenuStripExifTool.Name = "contextMenuStripMap";
            this.contextMenuStripExifTool.Size = new System.Drawing.Size(303, 238);
            // 
            // toolStripMenuItemExiftoolAssignCompositeTag
            // 
            this.toolStripMenuItemExiftoolAssignCompositeTag.Name = "toolStripMenuItemExiftoolAssignCompositeTag";
            this.toolStripMenuItemExiftoolAssignCompositeTag.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolAssignCompositeTag.Text = "Assign Composite Tag";
            // 
            // toolStripMenuItemExiftoolCopy
            // 
            this.toolStripMenuItemExiftoolCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemExiftoolCopy.Name = "toolStripMenuItemExiftoolCopy";
            this.toolStripMenuItemExiftoolCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemExiftoolCopy.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolCopy.Text = "Copy";
            this.toolStripMenuItemExiftoolCopy.Click += new System.EventHandler(this.toolStripMenuItemExiftoolCopy_Click);
            // 
            // toolStripMenuItemExiftoolFind
            // 
            this.toolStripMenuItemExiftoolFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.Find;
            this.toolStripMenuItemExiftoolFind.Name = "toolStripMenuItemExiftoolFind";
            this.toolStripMenuItemExiftoolFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemExiftoolFind.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolFind.Text = "Find";
            this.toolStripMenuItemExiftoolFind.Click += new System.EventHandler(this.toolStripMenuItemExiftoolFind_Click);
            // 
            // toolStripMenuItemExiftoolMarkFavorite
            // 
            this.toolStripMenuItemExiftoolMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect;
            this.toolStripMenuItemExiftoolMarkFavorite.Name = "toolStripMenuItemExiftoolMarkFavorite";
            this.toolStripMenuItemExiftoolMarkFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolMarkFavorite.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolMarkFavorite.Text = "Mark as favorite";
            this.toolStripMenuItemExiftoolMarkFavorite.Click += new System.EventHandler(this.toolStripMenuItemExiftoolMarkFavorite_Click);
            // 
            // toolStripMenuItemExiftoolRemoveFavorite
            // 
            this.toolStripMenuItemExiftoolRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove;
            this.toolStripMenuItemExiftoolRemoveFavorite.Name = "toolStripMenuItemExiftoolRemoveFavorite";
            this.toolStripMenuItemExiftoolRemoveFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolRemoveFavorite.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolRemoveFavorite.Text = "Remove as favorite";
            this.toolStripMenuItemExiftoolRemoveFavorite.Click += new System.EventHandler(this.toolStripMenuItemExiftoolRemoveFavorite_Click);
            // 
            // toolStripMenuItemExiftoolToggleFavorite
            // 
            this.toolStripMenuItemExiftoolToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle;
            this.toolStripMenuItemExiftoolToggleFavorite.Name = "toolStripMenuItemExiftoolToggleFavorite";
            this.toolStripMenuItemExiftoolToggleFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolToggleFavorite.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolToggleFavorite.Text = "Toggle favorite";
            this.toolStripMenuItemExiftoolToggleFavorite.Click += new System.EventHandler(this.toolStripMenuItemExiftoolToggleFavorite_Click);
            // 
            // toolStripMenuItemExiftoolSHowFavorite
            // 
            this.toolStripMenuItemExiftoolSHowFavorite.Name = "toolStripMenuItemExiftoolSHowFavorite";
            this.toolStripMenuItemExiftoolSHowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemExiftoolSHowFavorite.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolSHowFavorite.Text = "Show favorite rows";
            this.toolStripMenuItemExiftoolSHowFavorite.Click += new System.EventHandler(this.toolStripMenuItemExiftoolShowFavorite_Click);
            // 
            // toolStripMenuItemExiftoolHideEqual
            // 
            this.toolStripMenuItemExiftoolHideEqual.Name = "toolStripMenuItemExiftoolHideEqual";
            this.toolStripMenuItemExiftoolHideEqual.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemExiftoolHideEqual.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolHideEqual.Text = "Hide equal rows";
            this.toolStripMenuItemExiftoolHideEqual.Click += new System.EventHandler(this.toolStripMenuItemExiftoolHideEqual_Click);
            // 
            // toolStripMenuItemMediaPreview
            // 
            this.toolStripMenuItemMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Preview;
            this.toolStripMenuItemMediaPreview.Name = "toolStripMenuItemMediaPreview";
            this.toolStripMenuItemMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemMediaPreview.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemMediaPreview.Text = "Media Preview";
            this.toolStripMenuItemMediaPreview.Click += new System.EventHandler(this.toolStripMenuItemMediaPreview_Click);
            // 
            // tabPageExifToolWarning
            // 
            this.tabPageExifToolWarning.Controls.Add(this.dataGridViewExifToolWarning);
            this.tabPageExifToolWarning.Location = new System.Drawing.Point(4, 25);
            this.tabPageExifToolWarning.Name = "tabPageExifToolWarning";
            this.tabPageExifToolWarning.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExifToolWarning.Size = new System.Drawing.Size(537, 863);
            this.tabPageExifToolWarning.TabIndex = 5;
            this.tabPageExifToolWarning.Tag = "Warning";
            this.tabPageExifToolWarning.Text = "Warnings";
            this.tabPageExifToolWarning.ToolTipText = "Metadata mismatch warnings";
            this.tabPageExifToolWarning.UseVisualStyleBackColor = true;
            // 
            // dataGridViewExifToolWarning
            // 
            this.dataGridViewExifToolWarning.AllowUserToAddRows = false;
            this.dataGridViewExifToolWarning.ColumnHeadersHeight = 29;
            this.dataGridViewExifToolWarning.ContextMenuStrip = this.contextMenuStripExiftoolWarning;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewExifToolWarning.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewExifToolWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExifToolWarning.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewExifToolWarning.Name = "dataGridViewExifToolWarning";
            this.dataGridViewExifToolWarning.ReadOnly = true;
            this.dataGridViewExifToolWarning.RowHeadersWidth = 51;
            this.dataGridViewExifToolWarning.RowTemplate.Height = 24;
            this.dataGridViewExifToolWarning.Size = new System.Drawing.Size(531, 857);
            this.dataGridViewExifToolWarning.TabIndex = 0;
            this.dataGridViewExifToolWarning.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewExifToolWarning_CellBeginEdit);
            this.dataGridViewExifToolWarning.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewExifToolWarning_CellPainting);
            this.dataGridViewExifToolWarning.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewExifToolWarningData_KeyDown);
            // 
            // contextMenuStripExiftoolWarning
            // 
            this.contextMenuStripExiftoolWarning.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripExiftoolWarning.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemtoolExiftoolWarningAssignCompositeTag,
            this.toolStripMenuItemExiftoolWarningCopy,
            this.toolStripMenuItemExiftoolWarningFind,
            this.toolStripMenuItemExiftoolWarningMarkFavorite,
            this.toolStripMenuItemExiftoolWarningRemoveFavorite,
            this.toolStripMenuItemExiftoolWarningToggleFavorite,
            this.toolStripMenuItemExiftoolWarningShowFavorite,
            this.toolStripMenuItemExiftoolWarningHideEqual,
            this.toolStripMenuItemExiftoolWarningMediaPreview});
            this.contextMenuStripExiftoolWarning.Name = "contextMenuStripMap";
            this.contextMenuStripExiftoolWarning.Size = new System.Drawing.Size(303, 238);
            // 
            // toolStripMenuItemtoolExiftoolWarningAssignCompositeTag
            // 
            this.toolStripMenuItemtoolExiftoolWarningAssignCompositeTag.Name = "toolStripMenuItemtoolExiftoolWarningAssignCompositeTag";
            this.toolStripMenuItemtoolExiftoolWarningAssignCompositeTag.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemtoolExiftoolWarningAssignCompositeTag.Text = "Assign Composite Tag";
            // 
            // toolStripMenuItemExiftoolWarningCopy
            // 
            this.toolStripMenuItemExiftoolWarningCopy.Image = global::PhotoTagsSynchronizer.Properties.Resources.Copy;
            this.toolStripMenuItemExiftoolWarningCopy.Name = "toolStripMenuItemExiftoolWarningCopy";
            this.toolStripMenuItemExiftoolWarningCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItemExiftoolWarningCopy.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolWarningCopy.Text = "Copy";
            this.toolStripMenuItemExiftoolWarningCopy.Click += new System.EventHandler(this.toolStripMenuItemExiftoolWarningCopy_Click);
            // 
            // toolStripMenuItemExiftoolWarningFind
            // 
            this.toolStripMenuItemExiftoolWarningFind.Image = global::PhotoTagsSynchronizer.Properties.Resources.Find;
            this.toolStripMenuItemExiftoolWarningFind.Name = "toolStripMenuItemExiftoolWarningFind";
            this.toolStripMenuItemExiftoolWarningFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.toolStripMenuItemExiftoolWarningFind.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolWarningFind.Text = "Find";
            this.toolStripMenuItemExiftoolWarningFind.Click += new System.EventHandler(this.toolStripMenuItemExiftoolWarningFind_Click);
            // 
            // toolStripMenuItemExiftoolWarningMarkFavorite
            // 
            this.toolStripMenuItemExiftoolWarningMarkFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteSelect;
            this.toolStripMenuItemExiftoolWarningMarkFavorite.Name = "toolStripMenuItemExiftoolWarningMarkFavorite";
            this.toolStripMenuItemExiftoolWarningMarkFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolWarningMarkFavorite.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolWarningMarkFavorite.Text = "Mark as favorite";
            this.toolStripMenuItemExiftoolWarningMarkFavorite.Click += new System.EventHandler(this.toolStripMenuItemExiftoolWarningMarkFavorite_Click);
            // 
            // toolStripMenuItemExiftoolWarningRemoveFavorite
            // 
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteRemove;
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.Name = "toolStripMenuItemExiftoolWarningRemoveFavorite";
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.Text = "Remove as favorite";
            this.toolStripMenuItemExiftoolWarningRemoveFavorite.Click += new System.EventHandler(this.toolStripMenuItemExiftoolWarningRemoveFavorite_Click);
            // 
            // toolStripMenuItemExiftoolWarningToggleFavorite
            // 
            this.toolStripMenuItemExiftoolWarningToggleFavorite.Image = global::PhotoTagsSynchronizer.Properties.Resources.FavoriteToggle;
            this.toolStripMenuItemExiftoolWarningToggleFavorite.Name = "toolStripMenuItemExiftoolWarningToggleFavorite";
            this.toolStripMenuItemExiftoolWarningToggleFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.toolStripMenuItemExiftoolWarningToggleFavorite.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolWarningToggleFavorite.Text = "Toggle favorite";
            this.toolStripMenuItemExiftoolWarningToggleFavorite.Click += new System.EventHandler(this.toolStripMenuItemExiftoolWarningToggleFavorite_Click);
            // 
            // toolStripMenuItemExiftoolWarningShowFavorite
            // 
            this.toolStripMenuItemExiftoolWarningShowFavorite.Name = "toolStripMenuItemExiftoolWarningShowFavorite";
            this.toolStripMenuItemExiftoolWarningShowFavorite.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemExiftoolWarningShowFavorite.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolWarningShowFavorite.Text = "Show favorite rows";
            this.toolStripMenuItemExiftoolWarningShowFavorite.Click += new System.EventHandler(this.toolStripMenuItemExiftoolWarningShowFavorite_Click);
            // 
            // toolStripMenuItemExiftoolWarningHideEqual
            // 
            this.toolStripMenuItemExiftoolWarningHideEqual.Name = "toolStripMenuItemExiftoolWarningHideEqual";
            this.toolStripMenuItemExiftoolWarningHideEqual.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.toolStripMenuItemExiftoolWarningHideEqual.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolWarningHideEqual.Text = "Hide equal rows";
            this.toolStripMenuItemExiftoolWarningHideEqual.Click += new System.EventHandler(this.toolStripMenuItemExiftoolWarningHideEqual_Click);
            // 
            // toolStripMenuItemExiftoolWarningMediaPreview
            // 
            this.toolStripMenuItemExiftoolWarningMediaPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Preview;
            this.toolStripMenuItemExiftoolWarningMediaPreview.Name = "toolStripMenuItemExiftoolWarningMediaPreview";
            this.toolStripMenuItemExiftoolWarningMediaPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.toolStripMenuItemExiftoolWarningMediaPreview.Size = new System.Drawing.Size(302, 26);
            this.toolStripMenuItemExiftoolWarningMediaPreview.Text = "Media Preview";
            this.toolStripMenuItemExiftoolWarningMediaPreview.Click += new System.EventHandler(this.toolStripMenuItemExiftoolWarningMediaPreview_Click);
            // 
            // tabPageFileProperties
            // 
            this.tabPageFileProperties.Controls.Add(this.dataGridViewProperties);
            this.tabPageFileProperties.Location = new System.Drawing.Point(4, 25);
            this.tabPageFileProperties.Name = "tabPageFileProperties";
            this.tabPageFileProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFileProperties.Size = new System.Drawing.Size(537, 863);
            this.tabPageFileProperties.TabIndex = 7;
            this.tabPageFileProperties.Tag = "Properties";
            this.tabPageFileProperties.Text = "Properties";
            this.tabPageFileProperties.ToolTipText = "File windows properties";
            this.tabPageFileProperties.UseVisualStyleBackColor = true;
            // 
            // dataGridViewProperties
            // 
            this.dataGridViewProperties.AllowUserToAddRows = false;
            this.dataGridViewProperties.AllowUserToDeleteRows = false;
            this.dataGridViewProperties.AllowUserToOrderColumns = true;
            this.dataGridViewProperties.ColumnHeadersHeight = 29;
            this.dataGridViewProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProperties.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewProperties.Name = "dataGridViewProperties";
            this.dataGridViewProperties.RowHeadersWidth = 51;
            this.dataGridViewProperties.RowTemplate.Height = 24;
            this.dataGridViewProperties.Size = new System.Drawing.Size(531, 857);
            this.dataGridViewProperties.TabIndex = 0;
            this.dataGridViewProperties.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewProperties_CellBeginEdit);
            this.dataGridViewProperties.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewProperties_CellPainting);
            this.dataGridViewProperties.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewProperties_KeyDown);
            // 
            // tabPageFileRename
            // 
            this.tabPageFileRename.Controls.Add(this.buttonRenameUpdate);
            this.tabPageFileRename.Controls.Add(this.buttonRenameSave);
            this.tabPageFileRename.Controls.Add(this.comboBoxRenameVariableList);
            this.tabPageFileRename.Controls.Add(this.label2);
            this.tabPageFileRename.Controls.Add(this.label1);
            this.tabPageFileRename.Controls.Add(this.textBoxRenameNewName);
            this.tabPageFileRename.Controls.Add(this.dataGridViewRename);
            this.tabPageFileRename.Location = new System.Drawing.Point(4, 25);
            this.tabPageFileRename.Name = "tabPageFileRename";
            this.tabPageFileRename.Size = new System.Drawing.Size(537, 863);
            this.tabPageFileRename.TabIndex = 8;
            this.tabPageFileRename.Tag = "Rename";
            this.tabPageFileRename.Text = "Rename";
            this.tabPageFileRename.ToolTipText = "Rename files";
            this.tabPageFileRename.UseVisualStyleBackColor = true;
            // 
            // buttonRenameUpdate
            // 
            this.buttonRenameUpdate.Location = new System.Drawing.Point(122, 75);
            this.buttonRenameUpdate.Name = "buttonRenameUpdate";
            this.buttonRenameUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonRenameUpdate.TabIndex = 7;
            this.buttonRenameUpdate.Text = "Update";
            this.buttonRenameUpdate.UseVisualStyleBackColor = true;
            this.buttonRenameUpdate.Click += new System.EventHandler(this.buttonRenameUpdate_Click);
            // 
            // buttonRenameSave
            // 
            this.buttonRenameSave.Location = new System.Drawing.Point(203, 75);
            this.buttonRenameSave.Name = "buttonRenameSave";
            this.buttonRenameSave.Size = new System.Drawing.Size(75, 23);
            this.buttonRenameSave.TabIndex = 6;
            this.buttonRenameSave.Text = "Rename";
            this.buttonRenameSave.UseVisualStyleBackColor = true;
            this.buttonRenameSave.Click += new System.EventHandler(this.buttonRenameSave_Click);
            // 
            // comboBoxRenameVariableList
            // 
            this.comboBoxRenameVariableList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRenameVariableList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRenameVariableList.FormattingEnabled = true;
            this.comboBoxRenameVariableList.Items.AddRange(new object[] {
            "%Trim%",
            "%FileName%",
            "%FileNameWithoutDateTime%",
            "%Extension%",
            "%MediaFileNow_DateTime%",
            "%Media_DateTime%",
            "%Media_yyyy%",
            "%Media_MM%",
            "%Media_dd%",
            "%Media_HH%",
            "%Media_mm%",
            "%Media_ss%",
            "%File_DateTime%",
            "%File_yyyy%",
            "%File_MM%",
            "%File_dd%",
            "%File_HH%",
            "%File_mm%",
            "%File_ss%",
            "%Now_DateTime%",
            "%Now_yyyy%",
            "%Now_MM%",
            "%Now_dd%",
            "%Now_HH%",
            "%Now_mm%",
            "%Now_ss%",
            "%GPS_DateTimeUTC%",
            "%MediaAlbum%",
            "%MediaTitle%",
            "%MediaDescription%",
            "%MediaAuthor%",
            "%LocationName%",
            "%LocationCountry%",
            "%LocationState%"});
            this.comboBoxRenameVariableList.Location = new System.Drawing.Point(122, 14);
            this.comboBoxRenameVariableList.Name = "comboBoxRenameVariableList";
            this.comboBoxRenameVariableList.Size = new System.Drawing.Size(412, 25);
            this.comboBoxRenameVariableList.TabIndex = 4;
            this.comboBoxRenameVariableList.SelectionChangeCommitted += new System.EventHandler(this.comboBoxRenameVariableList_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "List of variables:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "New file name:";
            // 
            // textBoxRenameNewName
            // 
            this.textBoxRenameNewName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRenameNewName.Location = new System.Drawing.Point(122, 45);
            this.textBoxRenameNewName.Name = "textBoxRenameNewName";
            this.textBoxRenameNewName.Size = new System.Drawing.Size(412, 24);
            this.textBoxRenameNewName.TabIndex = 1;
            this.textBoxRenameNewName.Leave += new System.EventHandler(this.textBoxRenameNewName_Leave);
            // 
            // dataGridViewRename
            // 
            this.dataGridViewRename.AllowUserToAddRows = false;
            this.dataGridViewRename.AllowUserToDeleteRows = false;
            this.dataGridViewRename.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewRename.ColumnHeadersHeight = 29;
            this.dataGridViewRename.Location = new System.Drawing.Point(0, 108);
            this.dataGridViewRename.Name = "dataGridViewRename";
            this.dataGridViewRename.RowHeadersWidth = 51;
            this.dataGridViewRename.RowTemplate.Height = 24;
            this.dataGridViewRename.Size = new System.Drawing.Size(534, 759);
            this.dataGridViewRename.TabIndex = 0;
            this.dataGridViewRename.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewRename_CellBeginEdit);
            this.dataGridViewRename.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewRename_CellPainting);
            this.dataGridViewRename.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewRename_KeyDown);
            // 
            // tabPageMediaConverter
            // 
            this.tabPageMediaConverter.Controls.Add(this.panelConvertAndMerge);
            this.tabPageMediaConverter.Location = new System.Drawing.Point(4, 25);
            this.tabPageMediaConverter.Name = "tabPageMediaConverter";
            this.tabPageMediaConverter.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMediaConverter.Size = new System.Drawing.Size(537, 863);
            this.tabPageMediaConverter.TabIndex = 9;
            this.tabPageMediaConverter.Tag = "ConvertAndMerge";
            this.tabPageMediaConverter.Text = "Convert & Merge";
            this.tabPageMediaConverter.UseVisualStyleBackColor = true;
            // 
            // panelConvertAndMerge
            // 
            this.panelConvertAndMerge.Controls.Add(this.dataGridViewConvertAndMerge);
            this.panelConvertAndMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConvertAndMerge.Location = new System.Drawing.Point(3, 3);
            this.panelConvertAndMerge.Name = "panelConvertAndMerge";
            this.panelConvertAndMerge.Size = new System.Drawing.Size(531, 857);
            this.panelConvertAndMerge.TabIndex = 0;
            // 
            // dataGridViewConvertAndMerge
            // 
            this.dataGridViewConvertAndMerge.AllowDrop = true;
            this.dataGridViewConvertAndMerge.AllowUserToAddRows = false;
            this.dataGridViewConvertAndMerge.AllowUserToDeleteRows = false;
            this.dataGridViewConvertAndMerge.ColumnHeadersHeight = 29;
            this.dataGridViewConvertAndMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewConvertAndMerge.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewConvertAndMerge.Name = "dataGridViewConvertAndMerge";
            this.dataGridViewConvertAndMerge.RowHeadersWidth = 51;
            this.dataGridViewConvertAndMerge.RowTemplate.Height = 24;
            this.dataGridViewConvertAndMerge.Size = new System.Drawing.Size(531, 857);
            this.dataGridViewConvertAndMerge.TabIndex = 1;
            this.dataGridViewConvertAndMerge.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewConvertAndMerge_CellPainting);
            this.dataGridViewConvertAndMerge.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewConvertAndMerge_DragDrop);
            this.dataGridViewConvertAndMerge.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewConvertAndMerge_DragOver);
            this.dataGridViewConvertAndMerge.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewConvertAndMerge_KeyDown);
            this.dataGridViewConvertAndMerge.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewConvertAndMerge_MouseDown);
            this.dataGridViewConvertAndMerge.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewConvertAndMerge_MouseMove);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1387, 892);
            this.panel2.TabIndex = 2;
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thumbnailsToolStripButton,
            this.galleryToolStripButton,
            this.paneToolStripButton,
            this.detailsToolStripButton,
            this.toolStripSeparatorRenderer,
            this.rendererToolStripLabel,
            this.renderertoolStripComboBox,
            this.toolStripSeparator3,
            this.columnsToolStripButton,
            this.toolStripSeparator1,
            this.toolStripButtonThumbnailSize1,
            this.toolStripButtonThumbnailSize2,
            this.toolStripButtonThumbnailSize3,
            this.toolStripButtonThumbnailSize4,
            this.toolStripButtonThumbnailSize5,
            this.toolStripSeparator5,
            this.rotateCCWToolStripButton,
            this.rotate180ToolStripButton,
            this.rotateCWToolStripButton,
            this.toolStripButtonPreview,
            this.toolStripSeparator2,
            this.toolStripButtonSelectPrevious,
            this.toolStripDropDownButtonSelectGroupBy,
            this.toolStripButtonSelectNext,
            this.toolStripSeparator11,
            this.toolStripButtonGridBig,
            this.toolStripButtonGridNormal,
            this.toolStripButtonGridSmall,
            this.toolStripSeparator7,
            this.toolStripButtonHistortyColumns,
            this.toolStripButtonErrorColumns,
            this.toolStripSeparator6,
            this.toolStripButtonImportGoogleLocation,
            this.toolStripButtonSaveAllMetadata,
            this.toolStripSeparator4,
            this.toolStripButtonConfig,
            this.toolStripButtonWebScraper,
            this.toolStripButtonAbout});
            this.toolStrip.Location = new System.Drawing.Point(4, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1118, 28);
            this.toolStrip.TabIndex = 0;
            // 
            // thumbnailsToolStripButton
            // 
            this.thumbnailsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.thumbnailsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("thumbnailsToolStripButton.Image")));
            this.thumbnailsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.thumbnailsToolStripButton.Name = "thumbnailsToolStripButton";
            this.thumbnailsToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.thumbnailsToolStripButton.Text = "Thumbnails";
            this.thumbnailsToolStripButton.Click += new System.EventHandler(this.thumbnailsToolStripButton_Click);
            // 
            // galleryToolStripButton
            // 
            this.galleryToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.galleryToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("galleryToolStripButton.Image")));
            this.galleryToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.galleryToolStripButton.Name = "galleryToolStripButton";
            this.galleryToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.galleryToolStripButton.Text = "Gallery";
            this.galleryToolStripButton.Click += new System.EventHandler(this.galleryToolStripButton_Click);
            // 
            // paneToolStripButton
            // 
            this.paneToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paneToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("paneToolStripButton.Image")));
            this.paneToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paneToolStripButton.Name = "paneToolStripButton";
            this.paneToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.paneToolStripButton.Text = "Pane";
            this.paneToolStripButton.Click += new System.EventHandler(this.paneToolStripButton_Click);
            // 
            // detailsToolStripButton
            // 
            this.detailsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.detailsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("detailsToolStripButton.Image")));
            this.detailsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.detailsToolStripButton.Name = "detailsToolStripButton";
            this.detailsToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.detailsToolStripButton.Text = "Details";
            this.detailsToolStripButton.Click += new System.EventHandler(this.detailsToolStripButton_Click);
            // 
            // toolStripSeparatorRenderer
            // 
            this.toolStripSeparatorRenderer.Name = "toolStripSeparatorRenderer";
            this.toolStripSeparatorRenderer.Size = new System.Drawing.Size(6, 28);
            // 
            // rendererToolStripLabel
            // 
            this.rendererToolStripLabel.Name = "rendererToolStripLabel";
            this.rendererToolStripLabel.Size = new System.Drawing.Size(44, 25);
            this.rendererToolStripLabel.Text = "View:";
            this.rendererToolStripLabel.ToolTipText = "View layout for Image Liste viewer";
            // 
            // renderertoolStripComboBox
            // 
            this.renderertoolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.renderertoolStripComboBox.Name = "renderertoolStripComboBox";
            this.renderertoolStripComboBox.Size = new System.Drawing.Size(121, 28);
            this.renderertoolStripComboBox.ToolTipText = "Select view mode for Image List Viwer";
            this.renderertoolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.renderertoolStripComboBox_SelectedIndexChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 28);
            // 
            // columnsToolStripButton
            // 
            this.columnsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.columnsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("columnsToolStripButton.Image")));
            this.columnsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.columnsToolStripButton.Name = "columnsToolStripButton";
            this.columnsToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.columnsToolStripButton.Text = "Choose Columns for the Image View...";
            this.columnsToolStripButton.Click += new System.EventHandler(this.columnsToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButtonThumbnailSize1
            // 
            this.toolStripButtonThumbnailSize1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonThumbnailSize1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonThumbnailSize1.Image")));
            this.toolStripButtonThumbnailSize1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonThumbnailSize1.Name = "toolStripButtonThumbnailSize1";
            this.toolStripButtonThumbnailSize1.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonThumbnailSize1.Text = "Thumbnail size 200x200";
            this.toolStripButtonThumbnailSize1.Click += new System.EventHandler(this.toolStripButtonThumbnailSize1_Click);
            // 
            // toolStripButtonThumbnailSize2
            // 
            this.toolStripButtonThumbnailSize2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonThumbnailSize2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonThumbnailSize2.Image")));
            this.toolStripButtonThumbnailSize2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonThumbnailSize2.Name = "toolStripButtonThumbnailSize2";
            this.toolStripButtonThumbnailSize2.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonThumbnailSize2.Text = "Thumbnail size 150x150";
            this.toolStripButtonThumbnailSize2.Click += new System.EventHandler(this.toolStripButtonThumbnailSize2_Click);
            // 
            // toolStripButtonThumbnailSize3
            // 
            this.toolStripButtonThumbnailSize3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonThumbnailSize3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonThumbnailSize3.Image")));
            this.toolStripButtonThumbnailSize3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonThumbnailSize3.Name = "toolStripButtonThumbnailSize3";
            this.toolStripButtonThumbnailSize3.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonThumbnailSize3.Text = "Thumbnail size 120x120";
            this.toolStripButtonThumbnailSize3.Click += new System.EventHandler(this.toolStripButtonThumbnailSize3_Click);
            // 
            // toolStripButtonThumbnailSize4
            // 
            this.toolStripButtonThumbnailSize4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonThumbnailSize4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonThumbnailSize4.Image")));
            this.toolStripButtonThumbnailSize4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonThumbnailSize4.Name = "toolStripButtonThumbnailSize4";
            this.toolStripButtonThumbnailSize4.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonThumbnailSize4.Text = "Thumbnail size 96x96";
            this.toolStripButtonThumbnailSize4.Click += new System.EventHandler(this.toolStripButtonThumbnailSize4_Click);
            // 
            // toolStripButtonThumbnailSize5
            // 
            this.toolStripButtonThumbnailSize5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonThumbnailSize5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonThumbnailSize5.Image")));
            this.toolStripButtonThumbnailSize5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonThumbnailSize5.Name = "toolStripButtonThumbnailSize5";
            this.toolStripButtonThumbnailSize5.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonThumbnailSize5.Text = "Thumbnail size 48x48";
            this.toolStripButtonThumbnailSize5.Click += new System.EventHandler(this.toolStripButtonThumbnailSize5_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 28);
            // 
            // rotateCCWToolStripButton
            // 
            this.rotateCCWToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rotateCCWToolStripButton.Image = global::PhotoTagsSynchronizer.Properties.Resources.Rotate90CCW;
            this.rotateCCWToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotateCCWToolStripButton.Name = "rotateCCWToolStripButton";
            this.rotateCCWToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.rotateCCWToolStripButton.Text = "Rotate Counter-clockwise";
            this.rotateCCWToolStripButton.Click += new System.EventHandler(this.rotateCCWToolStripButton_Click);
            // 
            // rotate180ToolStripButton
            // 
            this.rotate180ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rotate180ToolStripButton.Image = global::PhotoTagsSynchronizer.Properties.Resources.Rotate180;
            this.rotate180ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotate180ToolStripButton.Name = "rotate180ToolStripButton";
            this.rotate180ToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.rotate180ToolStripButton.Text = "Rotate 180°";
            this.rotate180ToolStripButton.Click += new System.EventHandler(this.rotate180ToolStripButton_Click);
            // 
            // rotateCWToolStripButton
            // 
            this.rotateCWToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rotateCWToolStripButton.Image = global::PhotoTagsSynchronizer.Properties.Resources.Rotate90CW;
            this.rotateCWToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotateCWToolStripButton.Name = "rotateCWToolStripButton";
            this.rotateCWToolStripButton.Size = new System.Drawing.Size(29, 25);
            this.rotateCWToolStripButton.Text = "Rotate Clockwise";
            this.rotateCWToolStripButton.Click += new System.EventHandler(this.rotateCWToolStripButton_Click);
            // 
            // toolStripButtonPreview
            // 
            this.toolStripButtonPreview.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPreview.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Preview;
            this.toolStripButtonPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPreview.Name = "toolStripButtonPreview";
            this.toolStripButtonPreview.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonPreview.Text = "Preview media";
            this.toolStripButtonPreview.ToolTipText = "Preview media";
            this.toolStripButtonPreview.Click += new System.EventHandler(this.toolStripButtonPreview_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButtonSelectPrevious
            // 
            this.toolStripButtonSelectPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectPrevious.Image = global::PhotoTagsSynchronizer.Properties.Resources.Select_Previous;
            this.toolStripButtonSelectPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectPrevious.Name = "toolStripButtonSelectPrevious";
            this.toolStripButtonSelectPrevious.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonSelectPrevious.Text = "Select Previous Group";
            this.toolStripButtonSelectPrevious.Click += new System.EventHandler(this.toolStripButtonSelectPrevious_Click);
            // 
            // toolStripDropDownButtonSelectGroupBy
            // 
            this.toolStripDropDownButtonSelectGroupBy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonSelectGroupBy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSelectSameDay,
            this.toolStripMenuItemSelectSame3Day,
            this.toolStripMenuItemSelectSameWeek,
            this.toolStripMenuItemSelectSame2week,
            this.toolStripMenuItemSelectSameMonth,
            this.toolStripMenuItemSelectFallbackOnFileCreated,
            this.toolStripSeparator13,
            this.toolStripMenuItemSelectMax10items,
            this.toolStripMenuItemSelectMax30items,
            this.toolStripMenuItemSelectMax50items,
            this.toolStripMenuItemSelectMax100items,
            this.toolStripSeparator12,
            this.toolStripMenuItemSelectSameLocationName,
            this.toolStripMenuItemSelectSameCity,
            this.toolStripMenuItemSelectSameDistrict,
            this.toolStripMenuItemSelectSameCountry});
            this.toolStripDropDownButtonSelectGroupBy.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonSelectGroupBy.Image")));
            this.toolStripDropDownButtonSelectGroupBy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonSelectGroupBy.Name = "toolStripDropDownButtonSelectGroupBy";
            this.toolStripDropDownButtonSelectGroupBy.Size = new System.Drawing.Size(130, 25);
            this.toolStripDropDownButtonSelectGroupBy.Text = "Select group by:";
            // 
            // toolStripMenuItemSelectSameDay
            // 
            this.toolStripMenuItemSelectSameDay.Name = "toolStripMenuItemSelectSameDay";
            this.toolStripMenuItemSelectSameDay.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectSameDay.Text = "Select on same Day";
            this.toolStripMenuItemSelectSameDay.Click += new System.EventHandler(this.toolStripMenuItemSelectSameDay_Click);
            // 
            // toolStripMenuItemSelectSame3Day
            // 
            this.toolStripMenuItemSelectSame3Day.Name = "toolStripMenuItemSelectSame3Day";
            this.toolStripMenuItemSelectSame3Day.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectSame3Day.Text = "Select within 3 Day range";
            this.toolStripMenuItemSelectSame3Day.Click += new System.EventHandler(this.toolStripMenuItemSelectSame3Day_Click);
            // 
            // toolStripMenuItemSelectSameWeek
            // 
            this.toolStripMenuItemSelectSameWeek.Name = "toolStripMenuItemSelectSameWeek";
            this.toolStripMenuItemSelectSameWeek.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectSameWeek.Text = "Select within Week range (7 days)";
            this.toolStripMenuItemSelectSameWeek.Click += new System.EventHandler(this.toolStripMenuItemSelectSameWeek_Click);
            // 
            // toolStripMenuItemSelectSame2week
            // 
            this.toolStripMenuItemSelectSame2week.Name = "toolStripMenuItemSelectSame2week";
            this.toolStripMenuItemSelectSame2week.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectSame2week.Text = "Select within 2 weeks range (14 days)";
            this.toolStripMenuItemSelectSame2week.Click += new System.EventHandler(this.toolStripMenuItemSelectSame2week_Click);
            // 
            // toolStripMenuItemSelectSameMonth
            // 
            this.toolStripMenuItemSelectSameMonth.Name = "toolStripMenuItemSelectSameMonth";
            this.toolStripMenuItemSelectSameMonth.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectSameMonth.Text = "Select within Month range (30 days)";
            this.toolStripMenuItemSelectSameMonth.Click += new System.EventHandler(this.toolStripMenuItemSelectSameMonth_Click);
            // 
            // toolStripMenuItemSelectFallbackOnFileCreated
            // 
            this.toolStripMenuItemSelectFallbackOnFileCreated.Name = "toolStripMenuItemSelectFallbackOnFileCreated";
            this.toolStripMenuItemSelectFallbackOnFileCreated.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectFallbackOnFileCreated.Text = "Use File Create date when Date Taken missing";
            this.toolStripMenuItemSelectFallbackOnFileCreated.Click += new System.EventHandler(this.toolStripMenuItemSelectFallbackOnFileCreated_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(391, 6);
            // 
            // toolStripMenuItemSelectMax10items
            // 
            this.toolStripMenuItemSelectMax10items.Name = "toolStripMenuItemSelectMax10items";
            this.toolStripMenuItemSelectMax10items.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectMax10items.Text = "Select max 10 media files";
            this.toolStripMenuItemSelectMax10items.Click += new System.EventHandler(this.toolStripMenuItemSelectMax10items_Click);
            // 
            // toolStripMenuItemSelectMax30items
            // 
            this.toolStripMenuItemSelectMax30items.Name = "toolStripMenuItemSelectMax30items";
            this.toolStripMenuItemSelectMax30items.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectMax30items.Text = "Select max 30 media files";
            this.toolStripMenuItemSelectMax30items.Click += new System.EventHandler(this.toolStripMenuItemSelectMax30items_Click);
            // 
            // toolStripMenuItemSelectMax50items
            // 
            this.toolStripMenuItemSelectMax50items.Name = "toolStripMenuItemSelectMax50items";
            this.toolStripMenuItemSelectMax50items.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectMax50items.Text = "Select max 50 media files";
            this.toolStripMenuItemSelectMax50items.Click += new System.EventHandler(this.toolStripMenuItemSelectMax50items_Click);
            // 
            // toolStripMenuItemSelectMax100items
            // 
            this.toolStripMenuItemSelectMax100items.Name = "toolStripMenuItemSelectMax100items";
            this.toolStripMenuItemSelectMax100items.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectMax100items.Text = "Select max 100 media files";
            this.toolStripMenuItemSelectMax100items.Click += new System.EventHandler(this.toolStripMenuItemSelectMax100items_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(391, 6);
            // 
            // toolStripMenuItemSelectSameLocationName
            // 
            this.toolStripMenuItemSelectSameLocationName.Name = "toolStripMenuItemSelectSameLocationName";
            this.toolStripMenuItemSelectSameLocationName.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectSameLocationName.Text = "Select same Location Name";
            this.toolStripMenuItemSelectSameLocationName.Click += new System.EventHandler(this.toolStripMenuItemSelectSameLocationName_Click);
            // 
            // toolStripMenuItemSelectSameCity
            // 
            this.toolStripMenuItemSelectSameCity.Name = "toolStripMenuItemSelectSameCity";
            this.toolStripMenuItemSelectSameCity.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectSameCity.Text = "Select same City";
            this.toolStripMenuItemSelectSameCity.Click += new System.EventHandler(this.toolStripMenuItemSelectSameCity_Click);
            // 
            // toolStripMenuItemSelectSameDistrict
            // 
            this.toolStripMenuItemSelectSameDistrict.Name = "toolStripMenuItemSelectSameDistrict";
            this.toolStripMenuItemSelectSameDistrict.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectSameDistrict.Text = "Select same District";
            this.toolStripMenuItemSelectSameDistrict.Click += new System.EventHandler(this.toolStripMenuItemSelectSameDistrict_Click);
            // 
            // toolStripMenuItemSelectSameCountry
            // 
            this.toolStripMenuItemSelectSameCountry.Name = "toolStripMenuItemSelectSameCountry";
            this.toolStripMenuItemSelectSameCountry.Size = new System.Drawing.Size(394, 26);
            this.toolStripMenuItemSelectSameCountry.Text = "Select same Country";
            this.toolStripMenuItemSelectSameCountry.Click += new System.EventHandler(this.toolStripMenuItemSelectSameCountry_Click);
            // 
            // toolStripButtonSelectNext
            // 
            this.toolStripButtonSelectNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectNext.Image = global::PhotoTagsSynchronizer.Properties.Resources.Select_Next;
            this.toolStripButtonSelectNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectNext.Name = "toolStripButtonSelectNext";
            this.toolStripButtonSelectNext.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonSelectNext.Text = "Select Next Group";
            this.toolStripButtonSelectNext.Click += new System.EventHandler(this.toolStripButtonSelectNext_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButtonGridBig
            // 
            this.toolStripButtonGridBig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGridBig.Image = global::PhotoTagsSynchronizer.Properties.Resources.GridBig;
            this.toolStripButtonGridBig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGridBig.Name = "toolStripButtonGridBig";
            this.toolStripButtonGridBig.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonGridBig.Text = "Big Grid";
            this.toolStripButtonGridBig.Click += new System.EventHandler(this.toolStripButtonGridBig_Click);
            // 
            // toolStripButtonGridNormal
            // 
            this.toolStripButtonGridNormal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGridNormal.Image = global::PhotoTagsSynchronizer.Properties.Resources.GridNormal;
            this.toolStripButtonGridNormal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGridNormal.Name = "toolStripButtonGridNormal";
            this.toolStripButtonGridNormal.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonGridNormal.Text = "Normal Grid";
            this.toolStripButtonGridNormal.Click += new System.EventHandler(this.toolStripButtonGridNormal_Click);
            // 
            // toolStripButtonGridSmall
            // 
            this.toolStripButtonGridSmall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGridSmall.Image = global::PhotoTagsSynchronizer.Properties.Resources.GridSmall;
            this.toolStripButtonGridSmall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGridSmall.Name = "toolStripButtonGridSmall";
            this.toolStripButtonGridSmall.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonGridSmall.Text = "Small Grid";
            this.toolStripButtonGridSmall.Click += new System.EventHandler(this.toolStripButtonGridSmall_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButtonHistortyColumns
            // 
            this.toolStripButtonHistortyColumns.Checked = true;
            this.toolStripButtonHistortyColumns.CheckOnClick = true;
            this.toolStripButtonHistortyColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonHistortyColumns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHistortyColumns.Image = global::PhotoTagsSynchronizer.Properties.Resources.GridHistoryColumn;
            this.toolStripButtonHistortyColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHistortyColumns.Name = "toolStripButtonHistortyColumns";
            this.toolStripButtonHistortyColumns.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonHistortyColumns.Text = "Show or Hide Historty Columns";
            this.toolStripButtonHistortyColumns.CheckedChanged += new System.EventHandler(this.toolStripButtonHistortyColumns_CheckedChanged);
            // 
            // toolStripButtonErrorColumns
            // 
            this.toolStripButtonErrorColumns.Checked = true;
            this.toolStripButtonErrorColumns.CheckOnClick = true;
            this.toolStripButtonErrorColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonErrorColumns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonErrorColumns.Image = global::PhotoTagsSynchronizer.Properties.Resources.GridErrorColumn;
            this.toolStripButtonErrorColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonErrorColumns.Name = "toolStripButtonErrorColumns";
            this.toolStripButtonErrorColumns.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonErrorColumns.Text = "Show or Hide Error Columns";
            this.toolStripButtonErrorColumns.CheckedChanged += new System.EventHandler(this.toolStripButtonErrorColumns_CheckedChanged);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButtonImportGoogleLocation
            // 
            this.toolStripButtonImportGoogleLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImportGoogleLocation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImportGoogleLocation.Image")));
            this.toolStripButtonImportGoogleLocation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImportGoogleLocation.Name = "toolStripButtonImportGoogleLocation";
            this.toolStripButtonImportGoogleLocation.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonImportGoogleLocation.Text = "Import Google Locations";
            this.toolStripButtonImportGoogleLocation.Click += new System.EventHandler(this.toolStripButtonImportGoogleLocation_Click);
            // 
            // toolStripButtonSaveAllMetadata
            // 
            this.toolStripButtonSaveAllMetadata.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSaveAllMetadata.Image = global::PhotoTagsSynchronizer.Properties.Resources.save_32;
            this.toolStripButtonSaveAllMetadata.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAllMetadata.Name = "toolStripButtonSaveAllMetadata";
            this.toolStripButtonSaveAllMetadata.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonSaveAllMetadata.Text = "Save changes";
            this.toolStripButtonSaveAllMetadata.Click += new System.EventHandler(this.toolStripButtonSaveAllMetadata_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripButtonConfig
            // 
            this.toolStripButtonConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonConfig.Image = global::PhotoTagsSynchronizer.Properties.Resources.Config;
            this.toolStripButtonConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConfig.Name = "toolStripButtonConfig";
            this.toolStripButtonConfig.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonConfig.Text = "Config";
            this.toolStripButtonConfig.Click += new System.EventHandler(this.toolStripButtonConfig_Click);
            // 
            // toolStripButtonWebScraper
            // 
            this.toolStripButtonWebScraper.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonWebScraper.Image = global::PhotoTagsSynchronizer.Properties.Resources.WebScraping;
            this.toolStripButtonWebScraper.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWebScraper.Name = "toolStripButtonWebScraper";
            this.toolStripButtonWebScraper.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonWebScraper.Text = "WebScraper";
            this.toolStripButtonWebScraper.Click += new System.EventHandler(this.toolStripButtonWebScraper_Click);
            // 
            // toolStripButtonAbout
            // 
            this.toolStripButtonAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAbout.Image = global::PhotoTagsSynchronizer.Properties.Resources.About;
            this.toolStripButtonAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAbout.Name = "toolStripButtonAbout";
            this.toolStripButtonAbout.Size = new System.Drawing.Size(29, 25);
            this.toolStripButtonAbout.Text = "About";
            this.toolStripButtonAbout.Click += new System.EventHandler(this.toolStripButtonAbout_Click);
            // 
            // imageListFilter
            // 
            this.imageListFilter.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFilter.ImageStream")));
            this.imageListFilter.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListFilter.Images.SetKeyName(0, "Unchecked");
            this.imageListFilter.Images.SetKeyName(1, "Checked");
            this.imageListFilter.Images.SetKeyName(2, "Union");
            this.imageListFilter.Images.SetKeyName(3, "Intersection");
            this.imageListFilter.Images.SetKeyName(4, "Blank");
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = resources.GetString("openFileDialog.Filter");
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.ShowReadOnly = true;
            // 
            // timerShowErrorMessage
            // 
            this.timerShowErrorMessage.Interval = 1500;
            this.timerShowErrorMessage.Tick += new System.EventHandler(this.timerShowErrorMessage_Tick);
            // 
            // timerActionStatusRemove
            // 
            this.timerActionStatusRemove.Interval = 1500;
            this.timerActionStatusRemove.Tick += new System.EventHandler(this.timerActionStatusRemove_Tick);
            // 
            // timerStartThread
            // 
            this.timerStartThread.Enabled = true;
            this.timerStartThread.Interval = 1000;
            this.timerStartThread.Tick += new System.EventHandler(this.timerStartThread_Tick);
            // 
            // timerShowExiftoolSaveProgress
            // 
            this.timerShowExiftoolSaveProgress.Enabled = true;
            this.timerShowExiftoolSaveProgress.Interval = 400;
            this.timerShowExiftoolSaveProgress.Tick += new System.EventHandler(this.timerShowExiftoolSaveProgress_Tick);
            // 
            // timerStatusUpdate
            // 
            this.timerStatusUpdate.Enabled = true;
            this.timerStatusUpdate.Interval = 400;
            this.timerStatusUpdate.Tick += new System.EventHandler(this.timerStatusUpdate_Tick);
            // 
            // timerUpdateDataGridViewLoadingProgressbarRemove
            // 
            this.timerUpdateDataGridViewLoadingProgressbarRemove.Interval = 1000;
            this.timerUpdateDataGridViewLoadingProgressbarRemove.Tick += new System.EventHandler(this.timerUpdateDataGridViewLoadingProgressbarRemove_Tick);
            // 
            // panelMediaPreview
            // 
            this.panelMediaPreview.Controls.Add(this.toolStripContainer2);
            this.panelMediaPreview.Location = new System.Drawing.Point(0, 300);
            this.panelMediaPreview.Name = "panelMediaPreview";
            this.panelMediaPreview.Size = new System.Drawing.Size(865, 200);
            this.panelMediaPreview.TabIndex = 7;
            this.panelMediaPreview.Visible = false;
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.imageBoxPreview);
            this.toolStripContainer2.ContentPanel.Controls.Add(this.videoView1);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(865, 141);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(865, 200);
            this.toolStripContainer2.TabIndex = 3;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // imageBoxPreview
            // 
            this.imageBoxPreview.BackColor = System.Drawing.Color.Black;
            this.imageBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.imageBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBoxPreview.GridDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imageBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.imageBoxPreview.Name = "imageBoxPreview";
            this.imageBoxPreview.Size = new System.Drawing.Size(865, 141);
            this.imageBoxPreview.TabIndex = 3;
            this.imageBoxPreview.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // videoView1
            // 
            this.videoView1.BackColor = System.Drawing.Color.Black;
            this.videoView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.videoView1.Location = new System.Drawing.Point(0, 0);
            this.videoView1.MediaPlayer = null;
            this.videoView1.Name = "videoView1";
            this.videoView1.Size = new System.Drawing.Size(865, 141);
            this.videoView1.TabIndex = 2;
            this.videoView1.Text = "videoView1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonMediaPreviewPrevious,
            this.toolStripButtonMediaPreviewNext,
            this.toolStripButtonMediaPreviewPlay,
            this.toolStripButtonMediaPreviewPause,
            this.toolStripButtonMediaPreviewFastBackward,
            this.toolStripButtonMediaPreviewFastForward,
            this.toolStripButtonMediaPreviewStop,
            this.toolStripDropDownButtonChromecastList,
            this.toolStripDropDownButtonMediaList,
            this.toolStripMenuItemPreviewSlideShowMenu,
            this.toolStripButtonMediaPreviewRotateCCW,
            this.toolStripButtonMediaPreviewRotate180,
            this.toolStripButtonMediaPreviewRotateCW,
            this.toolStripButtonMediaPreviewClose,
            this.toolStripTraceBarItemMediaPreviewTimer,
            this.toolStripSeparator8,
            this.toolStripLabelMediaPreviewTimer,
            this.toolStripSeparator9,
            this.toolStripLabelMediaPreviewStatus,
            this.toolStripSeparator10});
            this.toolStrip1.Location = new System.Drawing.Point(4, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(845, 59);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonMediaPreviewPrevious
            // 
            this.toolStripButtonMediaPreviewPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewPrevious.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Previous;
            this.toolStripButtonMediaPreviewPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewPrevious.Name = "toolStripButtonMediaPreviewPrevious";
            this.toolStripButtonMediaPreviewPrevious.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewPrevious.Text = "Previous media";
            this.toolStripButtonMediaPreviewPrevious.Click += new System.EventHandler(this.toolStripButtonMediaPreviewPrevious_Click);
            // 
            // toolStripButtonMediaPreviewNext
            // 
            this.toolStripButtonMediaPreviewNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewNext.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Next;
            this.toolStripButtonMediaPreviewNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewNext.Name = "toolStripButtonMediaPreviewNext";
            this.toolStripButtonMediaPreviewNext.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewNext.Text = "Next media";
            this.toolStripButtonMediaPreviewNext.Click += new System.EventHandler(this.toolStripButtonMediaPreviewNext_Click);
            // 
            // toolStripButtonMediaPreviewPlay
            // 
            this.toolStripButtonMediaPreviewPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewPlay.Enabled = false;
            this.toolStripButtonMediaPreviewPlay.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Play;
            this.toolStripButtonMediaPreviewPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewPlay.Name = "toolStripButtonMediaPreviewPlay";
            this.toolStripButtonMediaPreviewPlay.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewPlay.Text = "Play";
            this.toolStripButtonMediaPreviewPlay.Click += new System.EventHandler(this.toolStripButtonMediaPreviewPlay_Click);
            // 
            // toolStripButtonMediaPreviewPause
            // 
            this.toolStripButtonMediaPreviewPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewPause.Enabled = false;
            this.toolStripButtonMediaPreviewPause.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Pause;
            this.toolStripButtonMediaPreviewPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewPause.Name = "toolStripButtonMediaPreviewPause";
            this.toolStripButtonMediaPreviewPause.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewPause.Text = "Pause";
            this.toolStripButtonMediaPreviewPause.Click += new System.EventHandler(this.toolStripButtonMediaPreviewPause_Click);
            // 
            // toolStripButtonMediaPreviewFastBackward
            // 
            this.toolStripButtonMediaPreviewFastBackward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewFastBackward.Enabled = false;
            this.toolStripButtonMediaPreviewFastBackward.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_FastBackward;
            this.toolStripButtonMediaPreviewFastBackward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewFastBackward.Name = "toolStripButtonMediaPreviewFastBackward";
            this.toolStripButtonMediaPreviewFastBackward.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewFastBackward.Text = "Fast Backward";
            this.toolStripButtonMediaPreviewFastBackward.Click += new System.EventHandler(this.toolStripButtonMediaPreviewFastBackward_Click);
            // 
            // toolStripButtonMediaPreviewFastForward
            // 
            this.toolStripButtonMediaPreviewFastForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewFastForward.Enabled = false;
            this.toolStripButtonMediaPreviewFastForward.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_FastForward;
            this.toolStripButtonMediaPreviewFastForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewFastForward.Name = "toolStripButtonMediaPreviewFastForward";
            this.toolStripButtonMediaPreviewFastForward.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewFastForward.Text = "Fast Forward";
            this.toolStripButtonMediaPreviewFastForward.Click += new System.EventHandler(this.toolStripButtonMediaPreviewFastForward_Click);
            // 
            // toolStripButtonMediaPreviewStop
            // 
            this.toolStripButtonMediaPreviewStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewStop.Enabled = false;
            this.toolStripButtonMediaPreviewStop.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Stop;
            this.toolStripButtonMediaPreviewStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewStop.Name = "toolStripButtonMediaPreviewStop";
            this.toolStripButtonMediaPreviewStop.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewStop.Text = "Stop chromecast application";
            this.toolStripButtonMediaPreviewStop.Click += new System.EventHandler(this.toolStripButtonMediaPreviewStop_Click);
            // 
            // toolStripDropDownButtonChromecastList
            // 
            this.toolStripDropDownButtonChromecastList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonChromecastList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemMediaChromecast,
            this.tv2ToolStripMenuItem});
            this.toolStripDropDownButtonChromecastList.Enabled = false;
            this.toolStripDropDownButtonChromecastList.Image = global::PhotoTagsSynchronizer.Properties.Resources.Chromecast;
            this.toolStripDropDownButtonChromecastList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonChromecastList.Name = "toolStripDropDownButtonChromecastList";
            this.toolStripDropDownButtonChromecastList.Size = new System.Drawing.Size(34, 56);
            this.toolStripDropDownButtonChromecastList.Text = "Select Chromecast device";
            // 
            // toolStripMenuItemMediaChromecast
            // 
            this.toolStripMenuItemMediaChromecast.Name = "toolStripMenuItemMediaChromecast";
            this.toolStripMenuItemMediaChromecast.Size = new System.Drawing.Size(114, 26);
            this.toolStripMenuItemMediaChromecast.Text = "Tv1";
            // 
            // tv2ToolStripMenuItem
            // 
            this.tv2ToolStripMenuItem.Name = "tv2ToolStripMenuItem";
            this.tv2ToolStripMenuItem.Size = new System.Drawing.Size(114, 26);
            this.tv2ToolStripMenuItem.Text = "Tv2";
            // 
            // toolStripDropDownButtonMediaList
            // 
            this.toolStripDropDownButtonMediaList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonMediaList.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Preview;
            this.toolStripDropDownButtonMediaList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonMediaList.Name = "toolStripDropDownButtonMediaList";
            this.toolStripDropDownButtonMediaList.Size = new System.Drawing.Size(34, 56);
            this.toolStripDropDownButtonMediaList.Text = "Select media for preview";
            this.toolStripDropDownButtonMediaList.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // toolStripMenuItemPreviewSlideShowMenu
            // 
            this.toolStripMenuItemPreviewSlideShowMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemPreviewSlideShowMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemPreviewSlideShow2sec,
            this.toolStripMenuItemPreviewSlideShow4sec,
            this.toolStripMenuItemPreviewSlideShow6sec,
            this.toolStripMenuItemPreviewSlideShow8sec,
            this.toolStripMenuItemPreviewSlideShow10sec,
            this.toolStripMenuItemPreviewSlideShowStop});
            this.toolStripMenuItemPreviewSlideShowMenu.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemPreviewSlideShowMenu.Image")));
            this.toolStripMenuItemPreviewSlideShowMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripMenuItemPreviewSlideShowMenu.Name = "toolStripMenuItemPreviewSlideShowMenu";
            this.toolStripMenuItemPreviewSlideShowMenu.Size = new System.Drawing.Size(92, 56);
            this.toolStripMenuItemPreviewSlideShowMenu.Text = "SlideShow";
            // 
            // toolStripMenuItemPreviewSlideShow2sec
            // 
            this.toolStripMenuItemPreviewSlideShow2sec.Name = "toolStripMenuItemPreviewSlideShow2sec";
            this.toolStripMenuItemPreviewSlideShow2sec.Size = new System.Drawing.Size(206, 26);
            this.toolStripMenuItemPreviewSlideShow2sec.Text = "SlideShow 2 sec";
            this.toolStripMenuItemPreviewSlideShow2sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow2sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShow4sec
            // 
            this.toolStripMenuItemPreviewSlideShow4sec.Name = "toolStripMenuItemPreviewSlideShow4sec";
            this.toolStripMenuItemPreviewSlideShow4sec.Size = new System.Drawing.Size(206, 26);
            this.toolStripMenuItemPreviewSlideShow4sec.Text = "SlideShow 4 sec";
            this.toolStripMenuItemPreviewSlideShow4sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow4sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShow6sec
            // 
            this.toolStripMenuItemPreviewSlideShow6sec.Name = "toolStripMenuItemPreviewSlideShow6sec";
            this.toolStripMenuItemPreviewSlideShow6sec.Size = new System.Drawing.Size(206, 26);
            this.toolStripMenuItemPreviewSlideShow6sec.Text = "SlideShow 6 sec";
            this.toolStripMenuItemPreviewSlideShow6sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow6sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShow8sec
            // 
            this.toolStripMenuItemPreviewSlideShow8sec.Name = "toolStripMenuItemPreviewSlideShow8sec";
            this.toolStripMenuItemPreviewSlideShow8sec.Size = new System.Drawing.Size(206, 26);
            this.toolStripMenuItemPreviewSlideShow8sec.Text = "SlideShow 8 sec";
            this.toolStripMenuItemPreviewSlideShow8sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow8sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShow10sec
            // 
            this.toolStripMenuItemPreviewSlideShow10sec.Name = "toolStripMenuItemPreviewSlideShow10sec";
            this.toolStripMenuItemPreviewSlideShow10sec.Size = new System.Drawing.Size(206, 26);
            this.toolStripMenuItemPreviewSlideShow10sec.Text = "SlideShow 10 sec";
            this.toolStripMenuItemPreviewSlideShow10sec.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShow10sec_Click);
            // 
            // toolStripMenuItemPreviewSlideShowStop
            // 
            this.toolStripMenuItemPreviewSlideShowStop.Enabled = false;
            this.toolStripMenuItemPreviewSlideShowStop.Name = "toolStripMenuItemPreviewSlideShowStop";
            this.toolStripMenuItemPreviewSlideShowStop.Size = new System.Drawing.Size(206, 26);
            this.toolStripMenuItemPreviewSlideShowStop.Text = "SlideShow stop";
            this.toolStripMenuItemPreviewSlideShowStop.Click += new System.EventHandler(this.toolStripMenuItemPreviewSlideShowStop_Click);
            // 
            // toolStripButtonMediaPreviewRotateCCW
            // 
            this.toolStripButtonMediaPreviewRotateCCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewRotateCCW.Image = global::PhotoTagsSynchronizer.Properties.Resources.Rotate90CCW;
            this.toolStripButtonMediaPreviewRotateCCW.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewRotateCCW.Name = "toolStripButtonMediaPreviewRotateCCW";
            this.toolStripButtonMediaPreviewRotateCCW.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewRotateCCW.Text = "Rotate CCW";
            this.toolStripButtonMediaPreviewRotateCCW.Click += new System.EventHandler(this.toolStripButtonMediaPreviewRotateCCW_Click);
            // 
            // toolStripButtonMediaPreviewRotate180
            // 
            this.toolStripButtonMediaPreviewRotate180.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewRotate180.Image = global::PhotoTagsSynchronizer.Properties.Resources.Rotate180;
            this.toolStripButtonMediaPreviewRotate180.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewRotate180.Name = "toolStripButtonMediaPreviewRotate180";
            this.toolStripButtonMediaPreviewRotate180.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewRotate180.Text = "Rotate 180";
            this.toolStripButtonMediaPreviewRotate180.Click += new System.EventHandler(this.toolStripButtonMediaPreviewRotate180_Click);
            // 
            // toolStripButtonMediaPreviewRotateCW
            // 
            this.toolStripButtonMediaPreviewRotateCW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewRotateCW.Image = global::PhotoTagsSynchronizer.Properties.Resources.Rotate90CW;
            this.toolStripButtonMediaPreviewRotateCW.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewRotateCW.Name = "toolStripButtonMediaPreviewRotateCW";
            this.toolStripButtonMediaPreviewRotateCW.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewRotateCW.Text = "Rotate CW";
            this.toolStripButtonMediaPreviewRotateCW.Click += new System.EventHandler(this.toolStripButtonMediaPreviewRotateCW_Click);
            // 
            // toolStripButtonMediaPreviewClose
            // 
            this.toolStripButtonMediaPreviewClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMediaPreviewClose.Image = global::PhotoTagsSynchronizer.Properties.Resources.Media_Close;
            this.toolStripButtonMediaPreviewClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMediaPreviewClose.Name = "toolStripButtonMediaPreviewClose";
            this.toolStripButtonMediaPreviewClose.Size = new System.Drawing.Size(29, 56);
            this.toolStripButtonMediaPreviewClose.Text = "Close preview media";
            this.toolStripButtonMediaPreviewClose.Click += new System.EventHandler(this.toolStripButtonMediaPreviewClose_Click);
            // 
            // toolStripTraceBarItemMediaPreviewTimer
            // 
            this.toolStripTraceBarItemMediaPreviewTimer.Name = "toolStripTraceBarItemMediaPreviewTimer";
            this.toolStripTraceBarItemMediaPreviewTimer.Size = new System.Drawing.Size(205, 56);
            this.toolStripTraceBarItemMediaPreviewTimer.Text = "Video timer";
            this.toolStripTraceBarItemMediaPreviewTimer.ToolTipText = "Video timer";
            this.toolStripTraceBarItemMediaPreviewTimer.ValueChanged += new System.EventHandler(this.toolStripTraceBarItemSeekPosition_ValueChanged);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 59);
            // 
            // toolStripLabelMediaPreviewTimer
            // 
            this.toolStripLabelMediaPreviewTimer.Name = "toolStripLabelMediaPreviewTimer";
            this.toolStripLabelMediaPreviewTimer.Size = new System.Drawing.Size(81, 56);
            this.toolStripLabelMediaPreviewTimer.Text = "Timer: 0.00";
            this.toolStripLabelMediaPreviewTimer.ToolTipText = "Timer";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 59);
            // 
            // toolStripLabelMediaPreviewStatus
            // 
            this.toolStripLabelMediaPreviewStatus.Name = "toolStripLabelMediaPreviewStatus";
            this.toolStripLabelMediaPreviewStatus.Size = new System.Drawing.Size(49, 56);
            this.toolStripLabelMediaPreviewStatus.Text = "Status";
            this.toolStripLabelMediaPreviewStatus.ToolTipText = "Status";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 59);
            // 
            // timerFindGoogleCast
            // 
            this.timerFindGoogleCast.Interval = 20000;
            this.timerFindGoogleCast.Tick += new System.EventHandler(this.timerFindGoogleCast_Tick);
            // 
            // timerPreviewNextTimer
            // 
            this.timerPreviewNextTimer.Interval = 2000;
            this.timerPreviewNextTimer.Tick += new System.EventHandler(this.timerPreviewNextTimer_Tick);
            // 
            // timerSaveProgessRemoveProgress
            // 
            this.timerSaveProgessRemoveProgress.Enabled = true;
            this.timerSaveProgessRemoveProgress.Interval = 500;
            this.timerSaveProgessRemoveProgress.Tick += new System.EventHandler(this.timerSaveProgessRemoveProgress_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1387, 950);
            this.Controls.Add(this.panelMediaPreview);
            this.Controls.Add(this.toolStripContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.Text = "PhotoTags Synchronizer";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainerFolder.Panel1.ResumeLayout(false);
            this.splitContainerFolder.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFolder)).EndInit();
            this.splitContainerFolder.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageFilterFolder.ResumeLayout(false);
            this.contextMenuStripTreeViewFolder.ResumeLayout(false);
            this.tabPageFilterSearch.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panelSearchFilter.ResumeLayout(false);
            this.groupBoxSearchKeywords.ResumeLayout(false);
            this.groupBoxSearchKeywords.PerformLayout();
            this.groupBoxSearchRating.ResumeLayout(false);
            this.groupBoxSearchRating.PerformLayout();
            this.groupBoxSearchPeople.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.groupBoxSearchExtra.ResumeLayout(false);
            this.groupBoxSearchExtra.PerformLayout();
            this.groupBoxSearchMediaTaken.ResumeLayout(false);
            this.groupBoxSearchMediaTaken.PerformLayout();
            this.groupBoxSearchTags.ResumeLayout(false);
            this.groupBoxSearchTags.PerformLayout();
            this.tabPageFilterTags.ResumeLayout(false);
            this.splitContainerImages.Panel1.ResumeLayout(false);
            this.splitContainerImages.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImages)).EndInit();
            this.splitContainerImages.ResumeLayout(false);
            this.contextMenuStripImageListView.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControlToolbox.ResumeLayout(false);
            this.tabPageTags.ResumeLayout(false);
            this.tabPageTags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTagsAndKeywords)).EndInit();
            this.contextMenuStripTagsAndKeywords.ResumeLayout(false);
            this.groupBoxRating.ResumeLayout(false);
            this.groupBoxRating.PerformLayout();
            this.tabPagePeople.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeople)).EndInit();
            this.contextMenuStripPeople.ResumeLayout(false);
            this.tabPageMap.ResumeLayout(false);
            this.splitContainerMap.Panel1.ResumeLayout(false);
            this.splitContainerMap.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMap)).EndInit();
            this.splitContainerMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMap)).EndInit();
            this.contextMenuStripMap.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPageDate.ResumeLayout(false);
            this.tabPageDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDate)).EndInit();
            this.tabPageExifTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExifTool)).EndInit();
            this.contextMenuStripExifTool.ResumeLayout(false);
            this.tabPageExifToolWarning.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExifToolWarning)).EndInit();
            this.contextMenuStripExiftoolWarning.ResumeLayout(false);
            this.tabPageFileProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProperties)).EndInit();
            this.tabPageFileRename.ResumeLayout(false);
            this.tabPageFileRename.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRename)).EndInit();
            this.tabPageMediaConverter.ResumeLayout(false);
            this.panelConvertAndMerge.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewConvertAndMerge)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelMediaPreview.ResumeLayout(false);
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel rendererToolStripLabel;
        private System.Windows.Forms.ToolStripComboBox renderertoolStripComboBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton detailsToolStripButton;
        private System.Windows.Forms.ToolStripButton thumbnailsToolStripButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusFilesAndSelected;
        private System.Windows.Forms.Timer timerShowErrorMessage;
        private System.Windows.Forms.ToolStripButton columnsToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton galleryToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRenderer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton rotateCCWToolStripButton;
        private System.Windows.Forms.ToolStripButton rotateCWToolStripButton;
        private System.Windows.Forms.ToolStripButton paneToolStripButton;
        private System.Windows.Forms.SplitContainer splitContainerFolder;
        private System.Windows.Forms.SplitContainer splitContainerImages;
        private ImageListView imageListView1;
        private System.Windows.Forms.Panel panel1;
        private Furty.Windows.Forms.FolderTreeView folderTreeViewFolder;
        private System.Windows.Forms.TabControl tabControlToolbox;
        private System.Windows.Forms.TabPage tabPageTags;
        private System.Windows.Forms.TabPage tabPagePeople;
        private System.Windows.Forms.TabPage tabPageExifTool;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusQueue;
        private System.Windows.Forms.DataGridView dataGridViewExifTool;
        private System.Windows.Forms.ComboBox comboBoxDescription;
        private System.Windows.Forms.ComboBox comboBoxTitle;
        private System.Windows.Forms.DataGridView dataGridViewTagsAndKeywords;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBoxRating;
        private System.Windows.Forms.RadioButton radioButtonRating5;
        private System.Windows.Forms.RadioButton radioButtonRating4;
        private System.Windows.Forms.RadioButton radioButtonRating3;
        private System.Windows.Forms.RadioButton radioButtonRating2;
        private System.Windows.Forms.RadioButton radioButtonRating1;
        private System.Windows.Forms.ComboBox comboBoxAlbum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxComments;
        private System.Windows.Forms.Label labelComments;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.DataGridView dataGridViewPeople;
        private System.Windows.Forms.TabPage tabPageMap;
        private System.Windows.Forms.SplitContainer splitContainerMap;
        private System.Windows.Forms.DataGridView dataGridViewMap;
        private System.Windows.Forms.TextBox textBoxBrowserURL;
        private System.Windows.Forms.Panel panelBrowser;
        private System.Windows.Forms.ComboBox comboBoxGoogleLocationInterval;
        private System.Windows.Forms.ComboBox comboBoxGoogleTimeZoneShift;
        private System.Windows.Forms.ComboBox comboBoxMapZoomLevel;
        private System.Windows.Forms.ToolStripButton toolStripButtonThumbnailSize1;
        private System.Windows.Forms.ToolStripButton toolStripButtonThumbnailSize2;
        private System.Windows.Forms.ToolStripButton toolStripButtonThumbnailSize3;
        private System.Windows.Forms.ToolStripButton toolStripButtonThumbnailSize4;
        private System.Windows.Forms.ToolStripButton toolStripButtonThumbnailSize5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripButton toolStripButtonImportGoogleLocation;
        private System.Windows.Forms.TabPage tabPageExifToolWarning;
        private System.Windows.Forms.DataGridView dataGridViewExifToolWarning;
        private System.Windows.Forms.ComboBox comboBoxMediaAiConfidence;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTagsAndKeywords;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTagsBrokerCopyText;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTagsAndKeywordsBrokerOverwriteText;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveAllMetadata;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusAction;
        private System.Windows.Forms.Timer timerActionStatusRemove;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripImageListView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewReloadThumbnailAndMetadata;
        private System.Windows.Forms.TabPage tabPageDate;
        private System.Windows.Forms.DataGridView dataGridViewDate;
        private System.Windows.Forms.ToolStripButton toolStripButtonAbout;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.ComboBox comboBoxAuthor;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuTagsBrokerCut;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuTagsBrokerCopy;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuTagsBrokerPaste;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuTagsBrokerDelete;
        private System.Windows.Forms.Label labelTagsInformation;
        private System.Windows.Forms.ToolStripMenuItem toggleTagsAndKeywordsSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuTags;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuTags;
        private System.Windows.Forms.TabPage tabPageFileProperties;
        private System.Windows.Forms.DataGridView dataGridViewProperties;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuTag;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuTag;
        private System.Windows.Forms.ToolStripMenuItem selectTagsAndKeywordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTagsAndKeywordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewReloadThumbnailAndMetadataClearThumbnailAndMetadataHistory;
        private System.Windows.Forms.ToolStripMenuItem markAsFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeAsFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleFavoriteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFavoriteRowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideEqualRowsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMap;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapRedo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapReplace;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapShowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapHideEqual;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCopyNotOverwrite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapCopyAndOverwrite;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButtonGridBig;
        private System.Windows.Forms.ToolStripButton toolStripButtonGridNormal;
        private System.Windows.Forms.ToolStripButton toolStripButtonGridSmall;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRemovePeopleTag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleTogglePeopleTag;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripPeople;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeoplePaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleUndo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRedo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleReplace;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleShowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleHideEqualRows;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleSelectPeopleTag;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfig;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageFilterFolder;
        private System.Windows.Forms.TabPage tabPageFilterTags;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromAll;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPeopleSelected;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameSelected;
        private System.Windows.Forms.ToolStripButton toolStripButtonErrorColumns;
        private System.Windows.Forms.ToolStripButton toolStripButtonHistortyColumns;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.TabPage tabPageFileRename;
        private System.Windows.Forms.DataGridView dataGridViewRename;
        private System.Windows.Forms.ComboBox comboBoxRenameVariableList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRenameNewName;
        private System.Windows.Forms.Button buttonRenameSave;
        private System.Windows.Forms.Button buttonRenameUpdate;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewRefreshFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewSelectAll;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeViewFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderCut;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderRefreshFolder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderReload;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderClearCache;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderReadSubfolders;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripExifTool;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolAssignCompositeTag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolSHowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolHideEqual;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowCoordinateOnMap;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTreeViewFolderAutoCorrectMetadata;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuTagsBrokerSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImageListViewAutoCorrect;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Timer timerStartThread;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapReloadLocationUsingNominatim;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripExiftoolWarning;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemtoolExiftoolWarningAssignCompositeTag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningFind;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningMarkFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningRemoveFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningToggleFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningShowFavorite;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningHideEqual;
        private System.Windows.Forms.Timer timerShowExiftoolSaveProgress;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyFileNamesToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileWithAssociatedApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editFileWithAssociatedApplicationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWithDialogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSelectedToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openMediaFilesWithToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListFilter;
        private System.Windows.Forms.TabPage tabPageFilterSearch;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox checkBoxSearchNeedAllKeywords;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.CheckBox checkBoxSearchWithoutKeyword;
        private System.Windows.Forms.DateTimePicker dateTimePickerSearchDateTo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox checkBoxSearchHasWarning;
        private System.Windows.Forms.ComboBox comboBoxSearchKeyword;
        private System.Windows.Forms.GroupBox groupBoxSearchPeople;
        private System.Windows.Forms.CheckBox checkBoxSearchNeedAllNames;
        private System.Windows.Forms.CheckedListBox checkedListBoxSearchPeople;
        private System.Windows.Forms.ComboBox comboBoxSearchLocationCountry;
        private System.Windows.Forms.ComboBox comboBoxSearchLocationState;
        private System.Windows.Forms.ComboBox comboBoxSearchLocationCity;
        private System.Windows.Forms.ComboBox comboBoxSearchLocationName;
        private System.Windows.Forms.ComboBox comboBoxSearchComments;
        private System.Windows.Forms.ComboBox comboBoxSearchDescription;
        private System.Windows.Forms.ComboBox comboBoxSearchTitle;
        private System.Windows.Forms.ComboBox comboBoxSearchAlbum;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dateTimePickerSearchDateFrom;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBoxSearchRating;
        private System.Windows.Forms.CheckBox checkBoxSearchRatingEmpty;
        private System.Windows.Forms.CheckBox checkBoxSearchRating5;
        private System.Windows.Forms.CheckBox checkBoxSearchRating4;
        private System.Windows.Forms.CheckBox checkBoxSearchRating3;
        private System.Windows.Forms.CheckBox checkBoxSearchRating2;
        private System.Windows.Forms.CheckBox checkBoxSearchRating1;
        private System.Windows.Forms.CheckBox checkBoxSearchRating0;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBoxSearchExtra;
        private System.Windows.Forms.GroupBox groupBoxSearchKeywords;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBoxSearchMediaTaken;
        private System.Windows.Forms.GroupBox groupBoxSearchTags;
        private System.Windows.Forms.CheckBox checkBoxSerachFitsAllValues;
        private System.Windows.Forms.CheckBox checkBoxSearchMediaTakenIsNull;
        private System.Windows.Forms.Panel panelSearchFilter;
        private System.Windows.Forms.CheckBox checkBoxSearchWithoutRegions;
        private System.Windows.Forms.CheckBox checkBoxSearchUseAndBetweenTextTagFields;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromLast1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromMostUsed;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromLast2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleRenameFromLast3;
        private DragNDrop.TreeViewWithoutDoubleClick treeViewFilter;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarDataGridViewLoading;
        private System.Windows.Forms.Timer timerStatusUpdate;
        private System.Windows.Forms.Timer timerUpdateDataGridViewLoadingProgressbarRemove;
        private System.Windows.Forms.ToolStripMenuItem mediaPreviewToolStripMenuItem;
        private System.Windows.Forms.Panel panelMediaPreview;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewPrevious;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewNext;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewPlay;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewPause;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewClose;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonChromecastList;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMediaChromecast;
        private System.Windows.Forms.ToolStripMenuItem tv2ToolStripMenuItem;
        private LibVLCSharp.WinForms.VideoView videoView1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonMediaList;
        private Cyotek.Windows.Forms.ImageBox imageBoxPreview;
        private DragNDrop.ToolStripTraceBarItem toolStripTraceBarItemMediaPreviewTimer;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewFastBackward;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewFastForward;
        private System.Windows.Forms.ToolStripLabel toolStripLabelMediaPreviewTimer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripLabel toolStripLabelMediaPreviewStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.TabPage tabPageMediaConverter;
        private System.Windows.Forms.Panel panelConvertAndMerge;
        private System.Windows.Forms.DataGridView dataGridViewConvertAndMerge;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewStop;
        private System.Windows.Forms.ToolStripButton rotate180ToolStripButton;
        private System.Windows.Forms.Timer timerFindGoogleCast;
        private System.Windows.Forms.ToolStripDropDownButton toolStripMenuItemPreviewSlideShowMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPreviewSlideShow2sec;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPreviewSlideShow4sec;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPreviewSlideShow6sec;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPreviewSlideShow8sec;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPreviewSlideShow10sec;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPreviewSlideShowStop;
        private System.Windows.Forms.Timer timerPreviewNextTimer;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewRotateCCW;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewRotate180;
        private System.Windows.Forms.ToolStripButton toolStripButtonMediaPreviewRotateCW;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarSaveProgress;
        private System.Windows.Forms.Timer timerSaveProgessRemoveProgress;
        private System.Windows.Forms.ToolStripButton toolStripButtonPreview;
        private System.Windows.Forms.ToolStripMenuItem rotateCW90ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotate180ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ratateCCW270ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectPrevious;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSelectGroupBy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameDay;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameCountry;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectNext;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameLocationName;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameCity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSame3Day;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameWeek;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSame2week;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameMonth;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectMax10items;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectMax30items;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectMax50items;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectMax100items;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectSameDistrict;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectFallbackOnFileCreated;
        private System.Windows.Forms.ToolStripButton toolStripButtonWebScraper;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleShowRegionSelector;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPeopleMediaPreview;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMapMediaPreview;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTagsAndKeywordMediaPreview;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMediaPreview;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExiftoolWarningMediaPreview;
    }
}

